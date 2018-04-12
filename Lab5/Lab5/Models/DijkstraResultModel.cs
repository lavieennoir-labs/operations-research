using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lab5.Models
{
    public class DijkstraResultModel
    {
        public int StartCell { get; set; }

        public int StageCount { get; set; }
        public int CurrentStage { get; set; }

        public double?[][] Matrix { get; set; }
        public List<double[]> DistTables { get; set; }
        public List<int[]> PathTables { get; set; }
        public List<bool[]> IsShortestTables { get; set; }
        public List<int> CurrentCellList { get; set; }


        public int CurrentCell
        {
            get => CurrentCellList[CurrentStage];
        }

        public double[] DistTable
        {
            get => DistTables[CurrentStage];
        }

        public bool[] IsShortestTable
        {
            get => IsShortestTables[CurrentStage];
        }

        public int[] PathTable
        {
            get => PathTables[CurrentStage];
        }
    }
}