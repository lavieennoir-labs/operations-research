using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lab8.Models
{
    public class SymplexTableModel
    {
        public string[] BasisNames { get; set; }
        public string[] VariableNames { get; set; }

        public double[] FreeMembers { get; set; }
        public double?[] MeaurementRelations { get; set; }

        public double[][] Coefs { get; set; }

        public int Num { get; set; }

        public int MainRowIdx { get; set; }
        public int MainColIdx { get; set; }
    }
}