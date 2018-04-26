using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1.ViewModel
{
    class CanonicalFormViewModel
    {
        public double[,] Matrix { get; set; }
        public double[] FreeMembers { get; set; }

        public List<string> Equastions { get; set; }
    }
}
