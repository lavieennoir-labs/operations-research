using Lab1.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                if (currentTableIdx != value)
                    return;
                currentTableIdx = value;
                RaisePropertyChanged("CurrentTableIdx");
                RaisePropertyChanged("CurrentTableData");
                RaisePropertyChanged("CurrentTableViewIdx");
                RaisePropertyChanged("CurrentTable");
                RaisePropertyChanged("CurrentTableIsNotFirst");
                RaisePropertyChanged("CurrentTableIsNotLast");
                RaisePropertyChanged("MainColumn");
                RaisePropertyChanged("MainRow");
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
            var table = firstTable.Clone();
            while(!table.IsOptimalPlan())
            {
                SymplexTables.Add(table);
                table = table.GetNextPlan();
            }
            //add optimal table to end
            SymplexTables.Add(table);
        }

        public void NextTable()
        {
            CurrentTableIdx++;
        }
    }
}
