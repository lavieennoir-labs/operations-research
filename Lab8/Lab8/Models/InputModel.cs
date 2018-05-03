using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lab8.Models
{
    public class InputModel
    {

        public int MatrixWidthUpdated { get; set; }
        public int MatrixHeightUpdated { get; set; }

        public int MatrixWidth { get; set; }
        public int MatrixHeight { get; set; }

        public double?[][] Matrix { get; set; }
    }
}