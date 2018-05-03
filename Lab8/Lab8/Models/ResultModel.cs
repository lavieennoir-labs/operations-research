using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lab8.Models
{
    public class ResultModel
    {
        public bool HavePureStrategySolution { get; set; }
        public bool ShrinkedHavePureStrategySolution { get; set; }

        public double StartUpperCost { get; set; }
        public double StartLowerCost { get; set; }
        public double StartUpperCostColIdx { get; set; }
        public double StartLowerCostRowIdx { get; set; }

        public List<double[][]> MatrixSnapshots { get; set; }

        public double ResultUpperCost { get; set; }
        public double ResultLowerCost { get; set; }
        public double ResultUpperCostColIdx { get; set; }
        public double ResultLowerCostRowIdx { get; set; }

        public double GameValue { get; set; }
        public List<SymplexTableModel> SymplexTableSnapshots { get; set; }

        public double[] AOptimalStrategy { get; set; }
        public double[] BOptimalStrategy { get; set; }

        public string AOptimalStrategyString
        {
            get => "(" + String.Join("; ", 
                AOptimalStrategy.Select(v => v.ToString("N2"))
                ) + ")";
        }

        public string BOptimalStrategyString
        {
            get => "(" + String.Join("; ", 
                BOptimalStrategy.Select(v => v.ToString("N2"))
                ) + ")";
        }

    }
}