using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lab5.Models
{
    public class FloydResultModel
    {
        public int StageCount { get; set; }
        public int CurrentStage { get; set; }

        public double?[][] Matrix { get; set; }
        public List<double[,]> DistTables { get; set; }
        public List<int[,]> PathTables { get; set; }

        
        public double[,] DistTable
        {
            get => DistTables[CurrentStage];
        }

        public int[,] PathTable
        {
            get => PathTables[CurrentStage];
        }
    }
}