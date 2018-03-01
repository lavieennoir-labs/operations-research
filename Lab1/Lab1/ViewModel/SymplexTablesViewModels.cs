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
        public ObservableCollection<SymplexTable> SymplexTables { get; set; }


        private int currentTableIdx = 0;
        public int CurrentTableIdx
        {
            get => currentTableIdx;
            set
            {
                if (currentTableIdx != value)
                    return;
                currentTableIdx = value;
                RaisePropertyChanged("CurrentTableIdx");
                RaisePropertyChanged("CurrentTable");
            }
        }
        
        public SymplexTable CurrentTable
        {
            get => SymplexTables[CurrentTableIdx];
        }
    }
}
