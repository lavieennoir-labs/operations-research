using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab3.Model.DiferentialRents
{
    class DiffRent
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

        public List<TableCell> MinimalTarifs;

        public double[] NotSatisfied;
        public double[] Renta;

        public List<double[,]> CountTables;
        public List<double[,]> CostTables;
        public List<List<TableCell>> MinimalTarifLists;
        public List<double[]> RentaLists;
        public List<double[]> SatisfiedLists;

        public DiffRent()
        {
            CountTables = new List<double[,]>();
            CostTables = new List<double[,]>();
            MinimalTarifLists = new List<List<TableCell>>();
            RentaLists = new List<double[]>();
            SatisfiedLists = new List<double[]>();
        }

        public bool IsClosedType()
        {
            return Raw.Sum().Equals(Need.Sum());
        }

        public bool IsOptimalSolution()
        {
            for (int i = 0; i < NeedCount; i++)
                if (Need[i] > 0)
                    return false;
            return true;
        }

        

        public void Solve()
        {
            Count = new double[RawCount, NeedCount];
            for (int i = 0; i < RawCount; i++)
                for (int j = 0; j < NeedCount; j++)
                    Count[i, j] = Double.NaN;
            //if (IsClosedType())
            //    throw new ArgumentException("Задача закритого типу!");

            CostTables.Add((double[,])Cost.Clone());
            while (!IsOptimalSolution())
            {
                CreateConditionallyOptimalDistribution();
                UpdateNotSatisfied();
                UpdateRenta();
                if (IsOptimalSolution())
                    return;
                UpdateCosts();
                //return;
            }
        }

        public double GetTotalCost()
        {
            double sum = 0;
            for (int i = 0; i < RawCount; i++)
                for (int j = 0; j < NeedCount; j++)
                    if (!Count[i, j].Equals(Double.NaN))
                        sum += CostOriginal[i, j] * Count[i, j];
            return sum;
        }

        public void UpdateNotSatisfied()
        {
            NotSatisfied = new double[RawCount];
            for (int i = 0; i < RawCount; i++)
            {
                var cells = MinimalTarifs.Where(cell => cell.i == i);
                var satisfaction = Raw[i];
                foreach (var cell in cells) //count all cells in coll j
                {
                    //search for cell above
                    //if (MinimalTarifs.
                    //    Where(c => c.j ==
                    //        cell.j && c.i < cell.i).Count() > 0)
                    //    satisfaction = 0;
                    //else
                        satisfaction -= Need[cell.j];
                }
                NotSatisfied[i] = satisfaction;
            }
            //check for 0
            for (int i = 0; i < RawCount; i++)
                if (NotSatisfied[i].Equals(0))
                {
                    var cells = MinimalTarifs.Where(cell => cell.i == i);
                    foreach (var cell in cells)
                    {
                        var connectedCells = MinimalTarifs.
                            Where(connectedCell => connectedCell.j == cell.j && connectedCell.i != cell.i);
                        if (connectedCells.Count() == 0)
                            continue;

                        bool ZeroUpdated = false;
                        foreach (var connectedCell in connectedCells)
                            if (!NotSatisfied[connectedCell.i].Equals(0))
                            {
                                NotSatisfied[i] = NotSatisfied[connectedCell.i] > 0 ?
                                    Double.Epsilon : -Double.Epsilon;
                                ZeroUpdated = true;
                                break;
                            }
                        if (ZeroUpdated)
                            break;
                    }
                }
            SatisfiedLists.Add(NotSatisfied);
        }

        public void UpdateRenta()
        {
            Renta = new double[NeedCount];
            for (int i = 0; i < NeedCount; i++)
            {
                var cell = MinimalTarifs.Where(c => c.j == i).First();
                if (NotSatisfied[cell.i] <= 0)
                {
                    //minminal cost in column
                    var minCost = double.PositiveInfinity;
                    for (int j = 0; j < RawCount; j++)
                        if (minCost > (Cost[j, i] - cell.Cost) 
                            && (Cost[j, i] - cell.Cost) > 0
                            && NotSatisfied[j] > 0)
                            minCost = Cost[j, i] - cell.Cost;
                    Renta[i] = minCost;
                }
                else if(Renta[i].Equals(0)) // if not set set as inf
                    Renta[i] = Double.PositiveInfinity;
            }
            RentaLists.Add(Renta);
        }

        public void UpdateCosts()
        {
            var minRenta = Renta.Min();
            for (int i = 0; i < RawCount; i++)
                if(NotSatisfied[i] <= 0)
                {
                    for (int j = 0; j < NeedCount; j++)
                        Cost[i, j] += minRenta;
                }
            CostTables.Add((double[,])Cost.Clone()); 
        }

        public void CreateConditionallyOptimalDistribution()
        {
            Count = new double[RawCount, NeedCount];
            for (int i = 0; i < RawCount; i++)
                for (int j = 0; j < NeedCount; j++)
                    Count[i, j] = Double.NaN;
            Raw = (double[])RawClone.Clone();
            Need = (double[])NeedClone.Clone();
            MinimalTarifs = new List<TableCell>();

            //get min cost for each col
            List<TableCell>[] minCosts = new List<TableCell>[NeedCount];
            for(int i = 0; i < NeedCount; i++)
            {
                minCosts[i] = new List<TableCell>();
                var min = Double.PositiveInfinity;
                for (int j = 0; j < RawCount; j++)
                    if (Cost[j, i] < min)
                        min = Cost[j, i];
                for (int j = 0; j < RawCount; j++)
                {
                    if (Cost[j, i].Equals(min))
                        minCosts[i].Add(new TableCell
                        {
                            Cost = Cost[j, i],
                            Count = 0,
                            i = j,
                            j = i
                        });
                }
            }
            //sorting
            minCosts = minCosts.OrderBy((cellList) => cellList.Count).ToArray();
            //fill MinimalTarifs
            for (int i = 0; i < NeedCount; i++)
            {
                    MinimalTarifs.AddRange(minCosts[i].OrderBy(cell => cell.i));
                 
            }
            ////sort MinimalTarifs from old values to new
            //if (MinimalTarifLists.Count > 0)
            //{
            //    var lastList = MinimalTarifLists.Last();
            //    for (int j = 0; j < MinimalTarifs.Count(); j++)
            //        if (!lastList.Contains(MinimalTarifs[j]))
            //        {
            //            var tmp = MinimalTarifs[j];
            //            MinimalTarifs.Remove(tmp);
            //            MinimalTarifs.Add(tmp);
            //        }
            //}
            //set minimal tarif values
            foreach (var cell in MinimalTarifs)
            {
                cell.Count = Math.Min(Raw[cell.i], Need[cell.j]);
                Count[cell.i, cell.j] = cell.Count;
                Raw[cell.i] -= cell.Count;
                Need[cell.j] -= cell.Count;
            }
            MinimalTarifLists.Add(MinimalTarifs);
            CountTables.Add(Count);
        }
    }
}
