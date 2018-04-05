﻿using Lab1.View.Pages;
using Lab1.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Lab1.Model
{
    class SymplexTable
    {
        public static SymplexTable GetFromCanonicalForm(CanonicalFormViewModel canonicalForm)
        {
            var table = new SymplexTable()
                {
                    B = canonicalForm.FreeMembers,
                    A = canonicalForm.Matrix,
                    Basis = Enumerable.Range(
                        canonicalForm.Matrix.GetLength(1) - canonicalForm.Matrix.GetLength(0),
                        canonicalForm.Matrix.GetLength(0)).ToArray(),
                    MarkingRelations = new double[canonicalForm.FreeMembers.Length]
                };
                table.MainCol = table.GetMainCol();
                table.UpdateMarkingRelations();
                table.MainRow = table.GetMainRow();
            return table;
        }

        public SymplexTable GetNextPlan()
        {
            var table = this.Clone();
            table.ApplyRectangleRule(this);
            table.MainCol = table.GetMainCol();
            table.UpdateMarkingRelations();
            table.MainRow = table.GetMainRow();
            return table;
        }



        /// <summary>
        /// Free members
        /// </summary>
        public double[] B;

        /// <summary>
        /// Variables
        /// </summary>
        public double[,] A;

        public int[] Basis;

        public double[] MarkingRelations;

        public int MainRow;
        public int MainCol;

        /// <summary>
        /// Represent Data of symplex table as a grid
        /// </summary>
        public string[,] AsTable {
            get
            {
                string[,] table = new string[B.Length, A.GetLength(1) + 2];

                for (int i = 0; i < Basis.Length; i++)
                    table[i, 0] = Basis[i] == A.GetLength(1) - 1 ? "F" : "X" + (Basis[i] + 1);
                for (int i = 0; i < B.Length; i++)
                    table[i, 1] = B[i].ToString("N2");

                if (IsOptimalPlan())
                    for (int i = 0; i < MarkingRelations.Length - 1; i++)
                        table[i, table.GetLength(1) - 1] = "";
                else
                {
                    for (int i = 0; i < MarkingRelations.Length - 1; i++)
                        table[i, table.GetLength(1) - 1] = MarkingRelations[i].ToString("N2");

                    //last Marking relation for F is empty
                    table[B.Length - 1, table.GetLength(1) - 1] = "";
                }

                for (int i = 0; i < B.Length; i++)
                    for (int j = 2; j < table.GetLength(1) - 1; j++)
                        table[i, j] = A[i, j - 2].ToString("N2");

                return table;
            }
        }

        public int GetMainCol()
        {
            double min = Double.MaxValue;
            int minIdx = -1;
            //search lowest negative coef
            for(int i = 0; i < A.GetLength(1); i++)
                if(min > A[B.Length - 1,i])
                {
                    min = A[B.Length - 1, i];
                    minIdx = i;
                }
            if (minIdx == -1) throw new InvalidOperationException("Всі коефіцієнти у останньому рядку додатні.");
            return minIdx;
        }


        /// <summary>
        /// Caclulate marking relations based on main row
        /// </summary>
        public void UpdateMarkingRelations()
        {
            for (int i = 0; i < B.Length; i++)
                MarkingRelations[i] = A[i, MainCol] - 0 < 0.0001 ? Double.PositiveInfinity : B[i] / A[i, MainCol];
        }


        public int GetMainRow()
        {
            double min = Double.MaxValue;
            int minIdx = -1;
            //search lowest Marking relation
            for (int i = 0; i < MarkingRelations.Length - 1; i++)
                if (min > MarkingRelations[i])
                {
                    min = MarkingRelations[i];
                    minIdx = i;
                }
            if (minIdx == -1)
                throw new InvalidOperationException(
                    "Задача не містить скінченного оптимуму.");
            return minIdx;
        }

        public void ApplyRectangleRule(SymplexTable old)
        {
            //cahnge basis
            Basis[MainRow] = old.MainCol;
            //handle main col
            for (int i = 0; i < B.Length; i++)
                if (i != old.MainRow)
                    A[i, old.MainCol] = 0;

            //handle main row        
            for (int i = 0; i < A.GetLength(1); i++)
                if (i == old.MainCol)
                    A[old.MainRow, i] = 1;
                else
                    A[old.MainRow, i] = old.A[old.MainRow, i] / old.A[old.MainRow, old.MainCol];
            B[old.MainRow] = old.B[old.MainRow] / old.A[old.MainRow, old.MainCol];
            //other coefs
            for (int i = 0; i < B.Length; i++)
            {
                if (i == old.MainRow) continue;
                B[i] = old.B[i] - old.A[i, old.MainCol] * old.B[old.MainRow] / old.A[old.MainRow, old.MainCol];
                for (int j = 0; j < A.GetLength(1); j++)
                    if (j != old.MainCol)
                        A[i, j] = old.A[i, j] - old.A[i, old.MainCol] * old.A[old.MainRow, j] / old.A[old.MainRow, old.MainCol];
            }

        }

        /// <summary>
        /// Check wether current plan is optimal
        /// </summary>
        public bool IsOptimalPlan()
        {
            return A[B.Length - 1, GetMainCol()] >= 0;
        }

        

        public SymplexTable Clone()
        {
            var table = new SymplexTable()
            {
                B = (double[])B.Clone(),
                A = (double[,])A.Clone(),
                Basis = (int[])Basis.Clone(),
                MarkingRelations = (double[])MarkingRelations.Clone(),
                MainCol = MainCol,
                MainRow = MainRow
            };
            return table;
        }
    }
}
