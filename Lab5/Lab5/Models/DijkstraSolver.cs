using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lab5.Models
{
    public class DijkstraSolver
    {
        public List<double[]> DistTables;
        public List<int[]> PathTables;
        public List<bool[]> IsShortestTables;
        public List<int> CurrentCellList;

        public double[][] Matrix;
        public int NodeCount;
        public int StartCell;

        public int CurrentCell
        {
            get => CurrentCellList.Last();
            set => CurrentCellList[CurrentCellList.Count - 1] = value;
        }

        public double[] DistTable
        {
            get => DistTables.Last();
        }

        public bool[] IsShortestTable
        {
            get => IsShortestTables.Last();
        }

        public int[] PathTable
        {
            get => PathTables.Last();
        }


        public void Solve()
        {
            Init();

            while(!IsShortestTable.All(i => i)) //while not all pathes are shortest
            {
                UpdateDistToCurrent();
                UpdateCurrent();
            }
        }

        void UpdateDistToCurrent()
        {
            CurrentCellList.Add(CurrentCell);
            DistTables.Add((double[])DistTable.Clone());
            IsShortestTables.Add((bool[])IsShortestTable.Clone());
            PathTables.Add((int[])PathTable.Clone());

            for (int i = 0; i < NodeCount; i++)
                if (!IsShortestTable[i] && !Double.IsNaN(Matrix[CurrentCell][i]))
                {
                    DistTable[i] = Math.Min(DistTable[i],
                        DistTable[CurrentCell] + Matrix[CurrentCell][i]);
                    PathTable[i] = CurrentCell;
                }


        }

        void UpdateCurrent()
        {
            double minDist = Double.PositiveInfinity;
            int minIdx = -1;
            for (int i = 0; i < NodeCount; i++)
                if(!IsShortestTable[i] && minDist > DistTable[i])
                {
                    minDist = DistTable[i];
                    minIdx = i;
                }
            CurrentCell = minIdx;
            if (minIdx == -1)
                for (int i = 0; i < IsShortestTable.Count(); i++)
                    IsShortestTable[i] = true;
            else
                IsShortestTable[minIdx] = true;
        }

        void Init()
        {
            CurrentCellList = new List<int>();
            CurrentCellList.Add(0);

            DistTables = new List<double[]>();
            DistTables.Add(new double[NodeCount]);

            IsShortestTables = new List<bool[]>();
            IsShortestTables.Add(new bool[NodeCount]);

            PathTables = new List<int[]>();
            PathTables.Add(new int[NodeCount]);


            for (int i = 0; i < NodeCount; i++)
            {
                DistTable[i] = Double.PositiveInfinity;
                IsShortestTable[i] = false;
                PathTable[i] = -1;
            }

            CurrentCell = StartCell;
            DistTable[CurrentCell] = 0;
            IsShortestTable[CurrentCell] = true;
        }
    }
}