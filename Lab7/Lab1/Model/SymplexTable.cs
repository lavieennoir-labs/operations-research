using Lab1.View.Pages;
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
                MarkingRelations = new double[canonicalForm.FreeMembers.Length],
                MarkingRelationsDual = new double[canonicalForm.Matrix.GetLength(1) - 1]
            };

            table.CheckDualSimplex();

            if (table.UsedDualSimplex)
            {
                for (int i = 0; i < table.A.GetLength(1); i++)
                    table.A[table.A.GetLength(0) - 1, i] *= -1;
                table.MainRow = table.GetMainRowDual();
                table.UpdateMarkingRelationsDual();
                table.MainCol = table.GetMainColDual();
            }
            else
            {
                table.MainCol = table.GetMainCol();
                table.UpdateMarkingRelations();
                table.MainRow = table.GetMainRow();
            }
            return table;
        }

        public SymplexTable GetNextPlan()
        {
            var table = this.Clone();
            if(!IsGomoriTable)
                table.ApplyRectangleRule(this);

            //CheckDualSimplex();
            if(table.UsedDualSimplex)
            {
                table.MainRow = table.GetMainRowDual();
                table.UpdateMarkingRelationsDual();
                table.MainCol = table.GetMainColDual();
            }
            else
            {
                table.MainCol = table.GetMainCol();
                table.UpdateMarkingRelations();
                table.MainRow = table.GetMainRow();
            }
            return table;
        }

        public bool UsedDualSimplex = false;

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
        public double[] MarkingRelationsDual;

        public int MainRow;
        public int MainCol;

        /// <summary>
        /// Represent Data of symplex table as a grid
        /// </summary>
        public string[,] AsTable {
            get
            {
                string[,] table = new string[B.Length + 1, A.GetLength(1) + 2];

                for (int i = 0; i < Basis.Length; i++)
                    table[i, 0] = Basis[i] == A.GetLength(1) - 1 ? "F" : "X" + (Basis[i] + 1);
                for (int i = 0; i < B.Length; i++)
                    table[i, 1] = B[i].ToString("N2");
                table[B.Length, 0] = "Відношення";

                if (IsOptimalPlan())
                {
                    for (int i = 0; i < MarkingRelations.Length - 1; i++)
                        table[i, A.GetLength(1)] = "";
                    for (int i = 1; i < table.GetLength(1); i++)
                        table[B.Length, i] = "";
                }
                else
                {
                    if (UsedDualSimplex)
                    {
                        table[B.Length, 1] = "-";
                        for (int i = 0; i < A.GetLength(1) - 1; i++)
                            table[B.Length, i + 2] =
                                MarkingRelationsDual[i].Equals(Double.PositiveInfinity) ? "-" :
                                    MarkingRelationsDual[i].ToString("N2");
                        for (int i = 0; i < MarkingRelations.Length - 1; i++)
                            table[i, table.GetLength(1) - 1] = "";
                    }
                    else
                    {
                        for (int i = 0; i < MarkingRelations.Length - 1; i++)
                            table[i, table.GetLength(1) - 1] = MarkingRelations[i].ToString("N2");

                        //last Marking relation for F is empty
                        table[B.Length - 1, table.GetLength(1) - 1] = "";

                        for (int i = 1; i < A.GetLength(1) + 1; i++)
                            table[B.Length, i] = "";
                    }
                }

                for (int i = 0; i < B.Length; i++)
                    for (int j = 2; j < table.GetLength(1) - 1; j++)
                        table[i, j] = A[i, j - 2].ToString("N2");

                return table;
            }
        }

        public void CheckDualSimplex()
        {
            UsedDualSimplex = B.Min() < 0;
        }

        public int GetMainRowDual()
        {
            double min = Double.MaxValue;
            int minIdx = -1;
            //search lowest negative coef
            for (int i = 0; i < B.Length - 1; i++)
                if (min > B[i])
                {
                    min = B[i];
                    minIdx = i;
                }
            return minIdx;
        }
        public void UpdateMarkingRelationsDual()
        {
            for (int i = 0; i < A.GetLength(1) - 1; i++)
                MarkingRelationsDual[i] = A[MainRow, i] > 0 || Math.Abs(A[B.Length - 1, i]) < 0.001  ?
                    Double.PositiveInfinity : -A[B.Length - 1, i] / A[MainRow, i];
        }
        public int GetMainColDual()
        {
            if (MarkingRelationsDual.All(m => Double.IsInfinity(m)))
                UpdateMarkingRelationsDual();
            //double min = Double.MaxValue;
            var vals = MarkingRelationsDual.Select(
                x => Double.IsInfinity(x) ? Double.PositiveInfinity : Math.Round(x, 3)).ToList();
            int minIdx = MarkingRelationsDual.Select(
                x => Double.IsInfinity(x) ? Double.PositiveInfinity : Math.Round(x, 3)).
                ToList().LastIndexOf(vals.Min());
            //search lowest negative coef
            //for (int i = 0; i < MarkingRelationsDual.Length; i++)
            //    if (min > MarkingRelationsDual[i] )
            //    {
            //        min = MarkingRelationsDual[i];
            //        minIdx = i;
            //    }
            return minIdx;
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
                MarkingRelations[i] = A[i, MainCol] <= 0.001 ? Double.PositiveInfinity : Math.Abs(B[i] / A[i, MainCol]);
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
            if(old.MainCol == -1)
            {
                if (old.UsedDualSimplex)
                    old.MainCol = old.GetMainColDual();
                else
                    old.MainCol = old.GetMainCol();
            }
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
            if (flag > 1000) return true;
                //flag++;

            if (UsedDualSimplex)
            {
                return B[GetMainRowDual()] >= 0;
            }
            else
            {
                for (int i = 0; i < B.Length - 1; i++)
                    if (A[B.Length - 1, i] < 0)
                        return false;
                return true;
            }
        }
        public static int flag = 0;
        

        public SymplexTable Clone()
        {
            var table = new SymplexTable()
            {
                B = (double[])B.Clone(),
                A = (double[,])A.Clone(),
                Basis = (int[])Basis.Clone(),
                MarkingRelations = (double[])MarkingRelations.Clone(),
                MarkingRelationsDual = (double[])MarkingRelationsDual.Clone(),
                MainCol = MainCol,
                MainRow = MainRow,
                UsedDualSimplex = UsedDualSimplex,
                UseIntegerSolution = UseIntegerSolution,
                InitialVarCount = InitialVarCount
            };
            return table;
        }

        #region Gomori method
        public bool UseIntegerSolution = false;
        public bool IsGomoriTable = false;
        public int InitialVarCount;

        public bool IsOptimalIntegerPlan()
        {
            if (flag > 100) return true;
            //flag++;


            for (int i = 0; i < InitialVarCount; i++)
                if (Math.Abs(B[i] - Math.Round(B[i])) > 0.001)
                    return false;
            return true;
        }

        public SymplexTable ModifyGomoriTable()
        {
            SymplexTable result = this.Clone();

            //get idx of row with min Fractional part of number
            int idx = -1;
            double max = Double.NegativeInfinity;
            for(int i = 0; i < B.Length - 1; i++)
                if(GetFractionalPart(B[i]) > max)
                {
                    idx = i;
                    max = GetFractionalPart(B[i]);
                }
            //modify Basis, B, A, MarkingRelations and MarkingRelationsDual 
            //with new basis
            var tmpBasis = result.Basis.ToList();
            tmpBasis.Add(result.Basis.Length+2);
            result.Basis = tmpBasis.ToArray();

            var tmpB = result.B.ToList();
            tmpB.Insert(result.B.Length - 1, -GetFractionalPart(result.B[idx]));
            result.B = tmpB.ToArray();


            var tmpA = new double[A.GetLength(0) + 1, A.GetLength(1) + 1];
            for (int i = 0; i < A.GetLength(0) - 1; i++)
            {
                for (int j = 0; j < A.GetLength(1) - 1; j++)
                    tmpA[i, j] = A[i, j];
                tmpA[i, A.GetLength(1) - 1] = 0;
                tmpA[i, A.GetLength(1)] = A[i, A.GetLength(1) - 1];
            }
            tmpA[A.GetLength(0) - 1, A.GetLength(1) - 1] = 1;
            tmpA[A.GetLength(0), A.GetLength(1)] = 1;
            for (int j = 0; j < A.GetLength(1) - 1; j++)
            {
                tmpA[A.GetLength(0) - 1, j] = -GetFractionalPart(A[idx, j]);
                tmpA[A.GetLength(0) , j] = A[A.GetLength(0) - 1, j];
            }
            result.A = tmpA;

            result.MarkingRelations = Enumerable.Repeat(
                Double.PositiveInfinity, result.B.Length).ToArray();
            result.MarkingRelationsDual = Enumerable.Repeat(
                Double.PositiveInfinity, result.A.GetLength(1) - 1).ToArray();

            return result;
        }

        double GetFractionalPart(double val)
        {
            return val >= 0 ? val - Math.Truncate(val) : 1 + (val - Math.Truncate(val));
        }

        #endregion
    }
}
