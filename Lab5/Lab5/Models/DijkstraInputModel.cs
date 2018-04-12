using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lab5.Models
{
    public class DijkstraInputModel
    {
        public int NodeCount { get; set; }

        public int MatrixSize { get; set; }
        
        public int StartIdx { get; set; }


        public string MatrixStr
        {
            get => String.Join("|", Matrix.SelectMany(x => x).ToArray());
            set { }
        }

        public List<List<double?>> Matrix { get; set; }
    }
}