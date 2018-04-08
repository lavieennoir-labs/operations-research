using Lab3.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab3.ViewModel
{
    class ResultTables : ViewModelBase
    {
        public ObservableCollection<ObservableCollection<string>> CurrentTable
        {
            get
            {
                if (Tables == null)
                    return new ObservableCollection<ObservableCollection<string>>
                    {
                        new ObservableCollection<string>
                        {
                            "Empty", "1"
                        },
                        new ObservableCollection<string>
                        {
                            "Empty", "2"
                        },

                    };

                return Tables[CurrentTableIdx];
            }
        }

        double totalSum = 0;
        public double TotalSum
        {
            get
            {
                return totalSum;
            }
            set
            {
                totalSum = value;
                RaisePropertyChanged("TotalSum");
            }
        }

        int currentTableIdx = 0;
        public int CurrentTableIdx
        {
            get
            {
                return currentTableIdx;
            }
            set
            {
                currentTableIdx = value;
                RaisePropertyChanged("CurrentTableIdx");
                RaisePropertyChanged("BasisIndex");
                RaisePropertyChanged("BasisIndexI");
                RaisePropertyChanged("BasisIndexJ");
                RaisePropertyChanged("TableNum");
                RaisePropertyChanged("CurrentTable");
                RaisePropertyChanged("CurrentRawPotential");
                RaisePropertyChanged("CurrentNeedPotential");
            }
        }

        public int TableNum
        {
            get
            {
                return CurrentTableIdx + 1;
            }
        }

        public int TableCount
        {
            get
            {
                return Tables.Count();
            }
        }
        
        public int BasisIndexI
        {
            get => BasisIndex.Item1 + 1;
        }
        public int BasisIndexJ
        {
            get => BasisIndex.Item2 + 1;
        }
        public Tuple<int, int> BasisIndex
        {
            get
            {
                if (BasisIndexes == null)
                    return new Tuple<int, int>(-1, -1);
                return BasisIndexes[CurrentTableIdx];
            }
        }

        public List<Tuple<int, int>> basisIndexes;
        public List<Tuple<int, int>> BasisIndexes
        {
            get
            {
                return basisIndexes;
            }
            set
            {
                basisIndexes = value;
                RaisePropertyChanged("BasisIndexes");
                RaisePropertyChanged("BasisIndex");
                RaisePropertyChanged("BasisIndexI");
                RaisePropertyChanged("BasisIndexJ");
                RaisePropertyChanged("CurrentTableIdx");
            }
        }

        public List<ObservableCollection<ObservableCollection<string>>> tables;
        public List<ObservableCollection<ObservableCollection<string>>> Tables
        {
            get
            {
                return tables;
            }
            set
            {
                tables = value;
                RaisePropertyChanged("Tables");
                RaisePropertyChanged("TableCount");
                RaisePropertyChanged("CurrentTableIdx");
                RaisePropertyChanged("BasisIndex");
                RaisePropertyChanged("BasisIndexI");
                RaisePropertyChanged("BasisIndexJ");
                RaisePropertyChanged("TableNum");
                RaisePropertyChanged("CurrentTable");
            }
        }

        public List<ObservableCollection<DoubleWrapper>> rawPotential;
        public List<ObservableCollection<DoubleWrapper>> RawPotential
        {
            get
            {
                return rawPotential;
            }
            set
            {
                rawPotential = value;
                RaisePropertyChanged("RawPotential");
                RaisePropertyChanged("CurrentRawPotential");
            }
        }

        public List<ObservableCollection<DoubleWrapper>> needPotential;
        public List<ObservableCollection<DoubleWrapper>> NeedPotential
        {
            get
            {
                return needPotential;
            }
            set
            {
                needPotential = value;
                RaisePropertyChanged("NeedPotential");
                RaisePropertyChanged("CurrentNeedPotential");
            }
        }

        public ObservableCollection<DoubleWrapper> CurrentRawPotential
        {
            get
            {
                if (Tables == null)
                    return new ObservableCollection<DoubleWrapper>
                    {
                            0
                    };

                return RawPotential[CurrentTableIdx];
            }
        }

        public ObservableCollection<DoubleWrapper> CurrentNeedPotential
        {
            get
            {
                if (Tables == null)
                    return new ObservableCollection<DoubleWrapper>
                    {
                            0
                    };

                return NeedPotential[CurrentTableIdx];
            }
        }
    }
}
