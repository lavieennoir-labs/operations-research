using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lab5.Models
{
    public class FloydSolver
    {
        public List<double[,]> DistTables;
        public List<int[,]> PathTables;

        public double[][] Matrix;
        public int NodeCount;
        

        public double[,] DistTable
        {
            get => DistTables.Last();
        }

        public int[,] PathTable
        {
            get => PathTables.Last();
        }


        public void Solve()
        {
            Init();
            
            Iterate();
        }

        public void Iterate()
        {
            for (int k = 0; k < NodeCount; k++)
            {
                for (int i = 0; i < NodeCount; i++)
                    for (int j = 0; j < NodeCount; j++)
                        if (i != j && j != k && k != i &&
                            DistTable[i, j] > DistTable[i, k] + DistTable[k, j])
                        {
                            DistTable[i, j] = DistTable[i, k] + DistTable[k, j];
                            PathTable[i, j] = k;
                        }
            DistTables.Add((double[,])DistTable.Clone());
            PathTables.Add((int[,])PathTable.Clone());
            }
        }
       

        void Init()
        {
            DistTables = new List<double[,]>();
            DistTables.Add(new double[NodeCount, NodeCount]);

            PathTables = new List<int[,]>();
            PathTables.Add(new int[NodeCount, NodeCount]);


            for (int i = 0; i < NodeCount; i++)
                for (int j = 0; j < NodeCount; j++)
                    {
                        if (i == j)
                            DistTable[i, j] = Double.NaN;
                        else if (Double.IsNaN(Matrix[i][j]))
                            DistTable[i, j] = Double.PositiveInfinity;
                        else
                            DistTable[i, j] = Matrix[i][j];

                        if (i == j)
                            PathTable[i, j] = -1;
                        else
                            PathTable[i, j] = i;
                    }
        }
    }
}