using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab3.Model
{
    partial class TransportTask
    {
        public double[,] CostOriginal;
        public double[,] Cost;
        public double[,] Count;

        public double[] Raw;
        public double[] Need;
        public double[] RawClone;
        public double[] NeedClone;

        public int RawCount;
        public int NeedCount;

        public int CurrentRawIdx;
        public int CurrentNeedIdx;


        public bool[] UsedRaw;
        public bool[] UsedNeed;

        public BasicPlan BasicPlan;

        public TransportTask()
        {
        }

        public void Solve()
        {
            RawPotentialTables = new List<double[]>();
            NeedPotentialTables = new List<double[]>();
            BasisIndexes = new List<Tuple<int, int>>();

            CountTables = new List<double[,]>();
            Count = new double[RawCount, NeedCount];
            for (int i = 0; i < RawCount; i++)
                for (int j = 0; j < NeedCount; j++)
                    Count[i, j] = Double.NaN;

            ApplyBasicPlan();
            Need = (double[])NeedClone.Clone();
            Raw = (double[])RawClone.Clone();
            AddCountTable();

            if (!IsNonDegenerateSolution())
                AddZeroCountTransport();

            CountPotentials();
            RawPotentialTables.Add(RawPotentials);
            NeedPotentialTables.Add(NeedPotentials);
            try
            {
                while (!IsOptimalSolution())
                {
                    GetNextPlan();
                    CountPotentials();
                    if (IsOptimalSolution())
                        break;

                    RawPotentialTables.Add(RawPotentials);
                    NeedPotentialTables.Add(NeedPotentials);

                }
            }
            catch(Exception e)
            {
                return;
            }
        }

        public void ApplyBasicPlan()
        {
            switch (BasicPlan)
            {
                case BasicPlan.NorthWestAngle:
                    ApplyNortWestAngle();
                    break;
                case BasicPlan.MinimalElement:
                    ApplyMinimalElement();
                    break;
                case BasicPlan.VogelMethod:
                    ApplyVogelMethod();
                    break;
                default:
                    ApplyNortWestAngle();
                    break;
            }
        }

        public void AddZeroCountTransport()
        {
            double min = Double.PositiveInfinity;
            int idxI = -1;
            int idxJ = -1;
            //find min not used Cost
            for (int i = 0; i < RawCount; i++)
                for (int j = 0; j < NeedCount; j++)
                    if (Count[i, j].Equals(Double.NaN) && Cost[i, j] < min)
                    {
                        min = Cost[i, j];
                        idxI = i;
                        idxJ = j;
                    }
            Count[idxI, idxJ] = 0;
        }


        #region north-west angle

        public void ApplyNortWestAngle()
        {
            CurrentRawIdx = 0;
            CurrentNeedIdx = 0;

            while (CurrentRawIdx < RawCount && CurrentNeedIdx < NeedCount)
            {
                if (Raw[CurrentRawIdx] > Need[CurrentNeedIdx])
                {
                    Count[CurrentRawIdx, CurrentNeedIdx] = Need[CurrentNeedIdx];
                    Raw[CurrentRawIdx] -= Count[CurrentRawIdx, CurrentNeedIdx];
                    Need[CurrentNeedIdx] = 0;
                    CurrentNeedIdx++;
                }
                else
                {
                    Count[CurrentRawIdx, CurrentNeedIdx] = Raw[CurrentRawIdx];
                    Raw[CurrentRawIdx] = 0;
                    Need[CurrentNeedIdx] -= Count[CurrentRawIdx, CurrentNeedIdx];
                    CurrentRawIdx++;
                }
            }
                
        }

        #endregion

        #region minimal element

        public void ApplyMinimalElement()
        {
            UsedRaw = new bool[RawCount];
            UsedNeed = new bool[NeedCount];

            while (UsedRaw.Where(i => i == false).Count() != 0 ||
                UsedNeed.Where(i => i == false).Count() != 0)
            {
                UpdateCurrentCell();
                FillCurrentCell();
            }

        }

        public void UpdateCurrentCell()
        {
            //get minimal available cost
            double min = Double.PositiveInfinity;
            for (int i = 0; i < RawCount; i++)
                for (int j = 0; j < NeedCount; j++)
                    if (!UsedRaw[i] && !UsedNeed[j] && Cost[i, j] < min)
                        min = Cost[i, j];

            //update indexes
            for (int i = 0; i < RawCount; i++)
                for (int j = 0; j < NeedCount; j++)
                    if (!UsedRaw[i] && !UsedNeed[j] && Cost[i, j].Equals(min))
                    {
                        CurrentRawIdx = i;
                        CurrentNeedIdx = j;
                        return;
                    }
            throw new Exception("Minimal Cell Not Found");
        }

        #endregion

        #region Vogel method

        public List<double[]> RowDelta;
        public List<double[]> ColDelta;

        public void ApplyVogelMethod()
        {
            UsedRaw = new bool[RawCount];
            UsedNeed = new bool[NeedCount];

            RowDelta = new List<double[]>();
            ColDelta = new List<double[]>();
            while (Need.Any(i => i > 0))
            {
                NextVogelDeltas();
                UpdateCurrentCellVogel();
                FillCurrentCell();
            }
        }

        private void NextVogelDeltas()
        {
            //add row deltas
            double[] rowDelta = new double[RawCount];
            for (int i = 0; i < RawCount; i++)
                rowDelta[i] = GetRowDelta(i);
            RowDelta.Add(rowDelta);

            //add col deltas
            double[] colDelta = new double[NeedCount];
            for (int i = 0; i < NeedCount; i++)
                colDelta[i] = GetColDelta(i);
            ColDelta.Add(colDelta);
        }

        private void UpdateCurrentCellVogel()
        {
            //get min values
            double rowMin = RowDelta.Last().Max();
            double colMin = ColDelta.Last().Max();
            double minCost = Double.PositiveInfinity;
            int minCostIdx = -1;

            if(rowMin > colMin)
            {
                CurrentRawIdx = RowDelta.Last().ToList().IndexOf(rowMin);
                //get min cost in row
                for(int i = 0; i <  NeedCount; i++)
                    if(!UsedNeed[i] && Cost[CurrentRawIdx,i] < minCost)
                    {
                        minCost = Cost[CurrentRawIdx, i];
                        minCostIdx = i;
                    }
                CurrentNeedIdx = minCostIdx;
            }
            else
            {
                CurrentNeedIdx = ColDelta.Last().ToList().IndexOf(colMin);
                //get min cost in col
                for (int i = 0; i < RawCount; i++)
                    if (!UsedRaw[i] && Cost[i, CurrentNeedIdx] < minCost)
                    {
                        minCost = Cost[i, CurrentNeedIdx];
                        minCostIdx = i;
                    }
                CurrentRawIdx = minCostIdx;
            }
        }

        private double GetRowDelta(int rowIdx)
        {
            //get first min
            double min = Double.PositiveInfinity;
            int minIdx = -1;
                for (int j = 0; j < NeedCount; j++)
                    if (!UsedRaw[rowIdx] && !UsedNeed[j] && Cost[rowIdx, j] < min)
                    {
                        min = Cost[rowIdx, j];
                        minIdx = j;
                    }

            //get second min
            double min2 = Double.PositiveInfinity;
                for (int j = 0; j < NeedCount; j++)
                    if (!UsedRaw[rowIdx] && !UsedNeed[j] && Cost[rowIdx, j] < min2 && j != minIdx)
                        min2 = Cost[rowIdx, j];

            //check for last delta
            if (min2.Equals(Double.PositiveInfinity))
            {
                if (min.Equals(Double.PositiveInfinity))
                    return -1;
                else
                    return 0;
            }

            return min2 - min;
        }

        private double GetColDelta(int colIdx)
        {
            //get first min
            double min = Double.PositiveInfinity;
            int minIdx = -1;
            for (int i = 0; i < RawCount; i++)
                    if (!UsedRaw[i] && !UsedNeed[colIdx] && Cost[i, colIdx] < min)
                    {
                        min = Cost[i, colIdx];
                        minIdx = i;
                    }

            //get second min
            double min2 = Double.PositiveInfinity;
            for (int i = 0; i < RawCount; i++)
                    if (!UsedRaw[i] && !UsedNeed[colIdx] && Cost[i, colIdx] < min2 && i != minIdx)
                        min2 = Cost[i, colIdx];

            //check for last delta
            if (min2.Equals(Double.PositiveInfinity))
                if (min.Equals(Double.PositiveInfinity))
                    return -1;
                else
                    return 0;

            return min2 - min;
        }

        #endregion

        private void FillCurrentCell()
        {
            Count[CurrentRawIdx, CurrentNeedIdx] =
                    Math.Min(Raw[CurrentRawIdx], Need[CurrentNeedIdx]);

            //mark used data
            if (Raw[CurrentRawIdx].Equals(Need[CurrentNeedIdx]))
            {
                Raw[CurrentRawIdx] = 0;
                Need[CurrentNeedIdx] = 0;
                UsedRaw[CurrentRawIdx] = true;
                UsedNeed[CurrentNeedIdx] = true;
            }
            else if (Raw[CurrentRawIdx] > Need[CurrentNeedIdx])
            {
                Raw[CurrentRawIdx] -= Count[CurrentRawIdx, CurrentNeedIdx];
                Need[CurrentNeedIdx] = 0;
                UsedNeed[CurrentNeedIdx] = true;
            }
            else
            {
                Need[CurrentNeedIdx] -= Count[CurrentRawIdx, CurrentNeedIdx];
                Raw[CurrentRawIdx] = 0;
                UsedRaw[CurrentRawIdx] = true;
            }
        }

        public bool IsNonDegenerateSolution()
        {
            return Count.Cast<double>().
                Where(i => !i.Equals(Double.NaN)).Count() ==
                RawCount + NeedCount - 1;
        }

        public double GetTotalCost()
        {
            double sum = 0;
            for (int i = 0; i < RawCount; i++)
                for (int j = 0; j < NeedCount; j++)
                    if (!Count[i, j].Equals(Double.NaN))
                        sum += Cost[i, j] * Count[i, j];
            return sum;
        }
    }
}
