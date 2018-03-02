using Lab1.Model;
using Lab1.View.Pages;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Lab1.ViewModel
{
    class SymplexTablesViewModels : ViewModel
    {
        public ObservableCollection<SymplexTable> SymplexTables { get; set; } = new ObservableCollection<SymplexTable>();
        
        private int currentTableIdx = 3;
        public int CurrentTableIdx
        {
            get => currentTableIdx;
            set
            {
                if (currentTableIdx == value)
                    return;
                currentTableIdx = value;
                RaisePropertyChanged("CurrentTableIdx");
                RaisePropertyChanged("CurrentTableData");
                RaisePropertyChanged("CurrentTableViewIdx");
                RaisePropertyChanged("CurrentTable");
                RaisePropertyChanged("CurrentTableIsNotFirst");
                RaisePropertyChanged("CurrentTableIsNotLast");
                RaisePropertyChanged("CurrentTableIsLast");
                RaisePropertyChanged("MainColumn");
                RaisePropertyChanged("MainRow");
                RaisePropertyChanged("ResultText");
            }
        }

        public string MainColumn
        {
            get => "X" + (CurrentTable.MainCol + 1);
        }

        public string MainRow
        {
            get =>
                CurrentTable.Basis[CurrentTable.MainRow] == CurrentTable.A.GetLength(1) - 1 ?
                    "F" : "X" + (CurrentTable.Basis[CurrentTable.MainRow] + 1);
        }

        public bool CurrentTableIsNotFirst { get => CurrentTableIdx != 0; }
        public bool CurrentTableIsNotLast { get => CurrentTableIdx != SymplexTables.Count()-1; }
        public bool CurrentTableIsLast { get => CurrentTableIdx == SymplexTables.Count()-1; }

        public int CurrentTableViewIdx
        {
            get => CurrentTableIdx + 1;
        }

        public SymplexTable CurrentTable
        {
            get => SymplexTables[CurrentTableIdx];
        }

        public string[,] CurrentTableData
        {
            get => CurrentTable.AsTable;
        }

        public string ResultText
        {
            get
            {
                int productCount = CurrentTable.A.GetLength(1) - CurrentTable.A.GetLength(0);
                int rawCount = CurrentTable.A.GetLength(0) - 1;
                var products = new double[productCount];
                var raws = new double[rawCount];

                for (int i = 0; i < CurrentTable.Basis.Length; i++)
                    if (CurrentTable.Basis[i] < productCount)
                        products[CurrentTable.Basis[i]] = CurrentTable.B[i];
                    else if(CurrentTable.Basis[i] < CurrentTable.A.GetLength(1) - 1)
                        raws[CurrentTable.Basis[i] - productCount] = CurrentTable.B[i];

                var sb = new StringBuilder();
                sb.Append("Було визначено, що максимальний прибуток (F = ").
                    Append(CurrentTable.B[CurrentTable.B.Length - 1]).
                    Append(" ум. од.) може бути досягнутий ");
                if (products.Where(i => i > 0).Count() > 0)
                {
                    sb.Append("при виробництві такої кількості продукції: ").
                        Append(Environment.NewLine);
                    for (int i = 0; i < products.Length; i++)
                        if (products[i] != 0)
                            sb.Append(products[i]).
                                Append(" одииниць продукції B").
                                Append(i + 1).
                                Append(". ");
                }
                else
                    sb.Append("без виробництва продукції.");
                
                if (raws.Where(i => i > 0).Count() > 0)
                {
                    sb.Append(Environment.NewLine).Append("За таких умов лишаться не використані: ");
                    for (int i = 0; i < raws.Length; i++)
                        if (raws[i] != 0)
                            sb.Append(raws[i]).
                                Append(" одииниць сировини А").
                                Append(i + 1).
                                Append(". ");
                }
                return sb.ToString();
            }
        }

        public List<string> TableHeaders
        {
            get
            {
                var header = new List<string> { "Базис", "Вільний член" };
                for (int i = 1; i < CurrentTable.A.GetLength(1); i++)
                    header.Add("X" + i);
                header.Add("Оціночні відношення");
                return header;
            }
        }

        /// <summary>
        /// Iterates through plans looking for the optimal one
        /// </summary>
        public void CountTables(SymplexTable firstTable)
        {
            try
            {
                var table = firstTable.Clone();
                while (!table.IsOptimalPlan())
                {
                    SymplexTables.Add(table);
                    table = table.GetNextPlan();
                }
                //add optimal table to end
                for (int i = 0; i < table.MarkingRelations.Length; i++)
                    table.MarkingRelations[i] = Double.PositiveInfinity;
                SymplexTables.Add(table);
                CurrentTableIdx = 0;
            }
            catch(InvalidOperationException)
            {
                MessageBox.Show("Оптимальний план не існує для таких вхідних даних.",
                        "Помилка", MessageBoxButton.OK, MessageBoxImage.Warning);
                (Application.Current.MainWindow.DataContext as PageManager).CurrentPage = new Input();               
            }
        }
    }
}
