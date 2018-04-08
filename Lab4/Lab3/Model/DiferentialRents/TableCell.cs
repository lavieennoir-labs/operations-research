using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab3.Model.DiferentialRents
{
    class TableCell
    {
        public double Cost;
        public double Count;
        public int i;
        public int j;

        public override bool Equals(object obj)
        {
            var item = obj as TableCell;
            if (item == null)
                return false;
            return i == item.i && j == item.j;
        }

        public override int GetHashCode()
        {
            var hashCode = -118560031;
            hashCode = hashCode * -1521134295 + i.GetHashCode();
            hashCode = hashCode * -1521134295 + j.GetHashCode();
            return hashCode;
        }
    }
}
