using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab3.Model
{
    //PotentialMethod
    partial class TransportTask
    {
        public List<double[]> RawPotentialTables;
        public List<double[]> NeedPotentialTables;
        public List<Tuple<int, int>> BasisIndexes;

        double[] NeedPotentials;
        double[] RawPotentials;

        int BasisRaw;
        int BasisNeed;

        public List<double[,]> CountTables;

        public bool IsOptimalSolution()
        {
            for (int i = 0; i < RawCount; i++)
                for (int j = 0; j < NeedCount; j++)
                        if (RawPotentials[i] + NeedPotentials[j] - Cost[i, j] <= 0)
                            return false;
            return true;                    
        }


        public void UpdateBasis()
        {
            double max = Double.NegativeInfinity;
            BasisRaw = -1;
            BasisNeed = -1;
            for (int i = 0; i < RawCount; i++)
                for (int j = 0; j < NeedCount; j++)
                    if (RawPotentials[i] + NeedPotentials[j] - Cost[i, j] > max)
                    {
                        max = RawPotentials[i] + NeedPotentials[j] - Cost[i, j];
                        BasisRaw = i;
                        BasisNeed = j;
                    }
            BasisIndexes.Add(new Tuple<int, int>(BasisRaw, BasisNeed));
        }


        public void GetNextPlan()
        {
            UpdateBasis();
            var cycle = new PotentialMethodCycle();
            cycle.CreateCycle(
                new PotentialMethodCycle.CycleItem
                {
                    Cost = Cost[BasisRaw, BasisNeed],
                    Count = 0,
                    i = BasisRaw,
                    j = BasisNeed,
                    IsPositive = true
                },
                Count, 
                Cost);
            //get minimal negaive item

            var MinValue = cycle.Items.Where(ci => ci.IsPositive == false).Min(ci => ci.Count);

            //create new basis
            Count[BasisRaw, BasisNeed] = 0;

            //update counts with MinValue
            for (int i = 0; i < cycle.Items.Count(); i++)
                if (cycle.Items[i].IsPositive)
                {
                    Count[cycle.Items[i].i, cycle.Items[i].j] += MinValue;
                    cycle.Items[i].Count += MinValue;
                }
                else
                {
                    Count[cycle.Items[i].i, cycle.Items[i].j] -= MinValue;
                    cycle.Items[i].Count -= MinValue;
                }
            //free cell
            var freeItem = cycle.Items.Where(ci => ci.Count.Equals(0) && ci.IsPositive == false).First();
            Count[freeItem.i, freeItem.j] = Double.NaN;

            AddCountTable();
        }

        void AddCountTable()
        {
            CountTables.Add((double[,])Count.Clone());
        }


        public void CountPotentials()
        {
            NeedPotentials = new double[NeedCount];
            RawPotentials = new double[RawCount];

            double[] filledCellsCount = new double[RawCount];
            //get the most filled row
            for (int i = 0; i < RawCount; i++)
                for (int j = 0; j < NeedCount; j++)
                    if (!Count[i, j].Equals(Double.NaN))
                        filledCellsCount[i]++;

            double max = filledCellsCount.Max();
            int initialPotential = -1;
            for (int i = 0; i < RawCount; i++)
                if (filledCellsCount[i].Equals(max))
                {
                    initialPotential = i;
                    break;
                }

            new PotentialCounter().CountPotentials(
                out RawPotentials, out NeedPotentials, Cost, Count, initialPotential);


        }


    }
}
