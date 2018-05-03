using Lab8.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lab8.Services
{
    public class GameTaskSolver
    {
        public InputModel CurrentInput { get; set; }

        SymplexSolver SymplexSolver { get; set; } = new SymplexSolver();
        CanonicalFormSymplexInput CurrentSymplexInput { get; set; }

        double[][] Matrix { get; set; }

        double StartUpperCost { get; set; }
        double StartLowerCost { get; set; }
        double StartUpperCostColIdx { get; set; }
        double StartLowerCostRowIdx { get; set; }

        double UpperCost { get; set; }
        int UpperCostColIdx { get; set; }
        double LowerCost { get; set; }
        int LowerCostRowIdx { get; set; }

        double GameValue { get; set; }

        public bool HavePureStrategySolution;
        public bool ShrinkedHavePureStrategySolution;

        public double[] AOptimalStrategyList;
        public double[] BOptimalStrategyList;

        public List<double[][]> MatrixSnapshots = new List<double[][]>();


        public void Solve()
        {
            UpdateMatrix();
            HavePureStrategySolution = false;

            MatrixSnapshots = new List<double[][]>();
            MakeMatrixSnapshot(Matrix);


            CountUpperCost();
            CountLowerCost();
            StartUpperCost = UpperCost;
            StartLowerCost = LowerCost;
            StartLowerCostRowIdx = LowerCostRowIdx;
            StartUpperCostColIdx = UpperCostColIdx;
            
            CheckPureStrategySolution();
            if (HavePureStrategySolution) return;

            ShrinkMatrixColumns();
            ShrinkMatrixRows();

            CountUpperCost();
            CountLowerCost();

            CheckPureStrategySolution();
            if (HavePureStrategySolution)
            {
                ShrinkedHavePureStrategySolution = true;
                return;
            }
            

            CreateSymplexTask();
            SolveSymplexTask();
        }


        private void SolveSymplexTask()
        {
            AOptimalStrategyList = new double[Matrix.Length];
            BOptimalStrategyList = new double[Matrix[0].Length];

            SymplexSolver.Solve(CurrentSymplexInput);
            var lastTable = SymplexSolver.SymplexTables.Last();

            GameValue = 1 / lastTable.B.Last();

            //fill B result
            for (int i = 0; i < lastTable.Basis.Length; i++)
                if (lastTable.Basis[i] < BOptimalStrategyList.Length)
                    BOptimalStrategyList[lastTable.Basis[i]] = lastTable.B[i] * GameValue;

            //fill A result
            for (int i = 0; i < AOptimalStrategyList.Length; i++)
                AOptimalStrategyList[i] = 
                    lastTable.A[Matrix.Length, Matrix[0].Length + i] * GameValue;
        }

        private void CreateSymplexTask()
        {
            double[] freeMembers = new double[Matrix.Length + 1];
            double[,] matrix = new double[Matrix.Length + 1, Matrix.Length + Matrix[0].Length];

            for (int i = 0; i < Matrix.Length; i++)
            {
                freeMembers[i] = 1;
                //fill coefs
                for (int j = 0; j < Matrix[0].Length; j++)
                    matrix[i, j] = Matrix[i][j];
            }
            //set diagonal 1
            for (int j = 0; j < Matrix.Length; j++)
                matrix[j, j + Matrix[0].Length] = 1;
            //fill F coefs
            for (int j = 0; j < Matrix[0].Length; j++)
                matrix[Matrix.Length, j] = -1;

            CurrentSymplexInput = new CanonicalFormSymplexInput()
            {
                FreeMembers = freeMembers,
                Matrix = matrix
            };
        }
        
        private void ShrinkMatrixColumns()
        {
            bool wasDeleted = true;
            while(wasDeleted)
            {
                wasDeleted = false;
                //search col to delete
                for(int j = 0; j < Matrix[0].Length; j++)
                {
                    for(int k = 0; k < Matrix[0].Length; k++)
                    {
                        if (j == k)
                            continue;

                        bool canDelete = true;
                        for (int i = 0; i < Matrix.Length; i++)
                        {
                            if (Matrix[i][k] < Matrix[i][j])
                            {
                                canDelete = false;
                                break;
                            }
                        }
                        if (canDelete)
                        {
                            wasDeleted = true;
                            //delete k col
                            Matrix = DeleteCol(Matrix, k);
                            MakeMatrixSnapshot(Matrix);
                            break;
                        }
                    }
                }
            };
        }

        private void ShrinkMatrixRows()
        {
            bool wasDeleted = true;
            while (wasDeleted)
            {
                wasDeleted = false;
                //search row to delete
                for (int i = 0; i < Matrix.Length; i++)
                {
                    for (int k = 0; k < Matrix.Length; k++)
                    {
                        if (i == k)
                            continue;

                        bool canDelete = true;
                        for (int j = 0; j < Matrix[0].Length; j++)
                        {
                            if (Matrix[k][j] > Matrix[i][j])
                            {
                                canDelete = false;
                                break;
                            }
                        }
                        if (canDelete)
                        {
                            wasDeleted = true;
                            //delete k col
                            Matrix = DeleteRow(Matrix, k);
                            MakeMatrixSnapshot(Matrix);
                            break;
                        }
                    }
                }
            };
        }

        private double[][] DeleteCol(double[][] matrix, int k)
        {
            var result = new double[matrix.Length][];
            for(int i = 0; i < result.Length; i++)
            {
                result[i] = new double[matrix[i].Length - 1];

                for (int j = 0; j < result[i].Length; j++)
                    if (j < k)
                        result[i][j] = matrix[i][j];
                    else
                        result[i][j] = matrix[i][j + 1];
            }
            return result;
        }

        private double[][] DeleteRow(double[][] matrix, int k)
        {
            int flag = 0;
            var result = new double[matrix.Length - 1][];
            for (int i = 0; i < result.Length; i++)
            {
                if(i == k)
                    flag = 1;

                result[i] = new double[matrix[i + flag].Length];

                for (int j = 0; j < result[i].Length; j++)
                        result[i][j] = matrix[i + flag][j];
            }
            return result;
        }


        private void MakeMatrixSnapshot(double[][] matrix)
        {
            var tmp = (double[][])(matrix.Clone());
            MatrixSnapshots.Add(tmp);
        }

        private void CheckPureStrategySolution()
        {
            //check wether game is in pure strategies
            if (UpperCost.Equals(LowerCost))
            {
                //get result
                AOptimalStrategyList[LowerCostRowIdx] = 1;
                BOptimalStrategyList[UpperCostColIdx] = 1;
                GameValue = UpperCost;

                HavePureStrategySolution = true;
            }
        }

        private void UpdateMatrix()
        {
            Matrix = new double[CurrentInput.MatrixHeight][];
            for (int i = 0; i < CurrentInput.MatrixHeight; i++)
            {
                Matrix[i] = new double[CurrentInput.MatrixWidth];
                for (int j = 0; j < CurrentInput.MatrixWidth; j++)
                    Matrix[i][j] = CurrentInput.Matrix[i][j] ?? 0;
            }
        }

        private void CountLowerCost()
        {
            var minFromRows = new List<double>();
            for (int i = 0; i < Matrix.Length; i++)
            {
                minFromRows.Add(Double.MaxValue);
                for (int j = 0; j < Matrix[0].Length; j++)
                    if (Matrix[i][j] < minFromRows[i])
                        minFromRows[i] = Matrix[i][j];
            }
            LowerCost = minFromRows.Max();
            LowerCostRowIdx = minFromRows.IndexOf(LowerCost);
        }

        private void CountUpperCost()
        {
            var maxFromCols = new List<double>();
            for(int j = 0; j < Matrix[0].Length; j++)
            {
                maxFromCols.Add(Double.MinValue);
                for (int i = 0; i < Matrix.Length; i++)
                    if (Matrix[i][j] > maxFromCols[j])
                        maxFromCols[j] = Matrix[i][j];
            }
            UpperCost = maxFromCols.Min();
            UpperCostColIdx = maxFromCols.IndexOf(UpperCost);
        }
        

        public ResultModel Result
        {
            get
            {
                return new ResultModel
                {
                    StartUpperCost = StartUpperCost,
                    StartLowerCost = StartLowerCost,
                    StartUpperCostColIdx = StartUpperCostColIdx,
                    StartLowerCostRowIdx = StartLowerCostRowIdx,
                    ResultUpperCost = UpperCost,
                    ResultLowerCost = LowerCost,
                    ResultUpperCostColIdx = UpperCostColIdx,
                    ResultLowerCostRowIdx = LowerCostRowIdx,
                    MatrixSnapshots = MatrixSnapshots,
                    AOptimalStrategy = AOptimalStrategyList,
                    BOptimalStrategy = BOptimalStrategyList,
                    GameValue = GameValue,
                    SymplexTableSnapshots = SymplexSolver.GetSymplexTableModels(),
                    HavePureStrategySolution = HavePureStrategySolution,
                    ShrinkedHavePureStrategySolution = ShrinkedHavePureStrategySolution
                };
            }
        }
    }
}