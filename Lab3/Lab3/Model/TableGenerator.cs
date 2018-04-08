using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab3.Model
{
    class TableGenerator
    {
        public static ObservableCollection<ObservableCollection<string>> GetCountTable(
            double[,] count)
        {
            var table = new ObservableCollection<ObservableCollection<string>>();
            int rawCount = count.GetLength(0);
            int needCount = count.GetLength(1);
            for (int i = 0; i < rawCount; i++)
            {
                table.Add(new ObservableCollection<string>());
                for (int j = 0; j < needCount; j++)
                    if (count[i, j].Equals(Double.NaN))
                        table[i].Add("-");
                    else
                        table[i].Add(count[i, j].ToString("N2"));
            }
            return table;
        }

        public static ObservableCollection<DoubleWrapper> GetPotentialTable(
            double[] potentials)
        {
            var table = new ObservableCollection<DoubleWrapper>();
            int Count = potentials.Length;
            for (int i = 0; i < Count; i++)
            {
                table.Add(potentials[i]);
            }
            return table;
        }
    }
}
