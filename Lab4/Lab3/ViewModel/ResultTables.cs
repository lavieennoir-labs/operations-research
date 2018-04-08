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
                RaisePropertyChanged("Costs");
                RaisePropertyChanged("Tarifs");
                RaisePropertyChanged("CurrentCost");
                RaisePropertyChanged("CurrentTarif");
                RaisePropertyChanged("Tarifs");
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
                RaisePropertyChanged("CurrentCost");
                RaisePropertyChanged("CurrentTarif");
                RaisePropertyChanged("Costs");
                RaisePropertyChanged("Tarifs");
                RaisePropertyChanged("TableCount");
                RaisePropertyChanged("CurrentTableIdx");
                RaisePropertyChanged("BasisIndex");
                RaisePropertyChanged("BasisIndexI");
                RaisePropertyChanged("BasisIndexJ");
                RaisePropertyChanged("TableNum");
                RaisePropertyChanged("CurrentTable");
            }
        }

        public List<ObservableCollection<ObservableCollection<string>>> costs;
        public List<ObservableCollection<ObservableCollection<string>>> Costs
        {
            get
            {
                return costs;
            }
            set
            {
                costs = value;
                RaisePropertyChanged("Costs");
                RaisePropertyChanged("CurrentCost");
            }
        }

        public ObservableCollection<ObservableCollection<string>> CurrentCost
        {
            get
            {
                if (Costs == null)
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

                return Costs[CurrentTableIdx];
            }
        }

        public StringWrapper CurrentTarif
        {
            get
            {
                if (Tarifs == null)
                    return "";

                return Tarifs[CurrentTableIdx];
            }
        }

        public List<StringWrapper> tarifs;
        public List<StringWrapper> Tarifs
        {
            get
            {
                return tarifs;
            }
            set
            {
                tarifs = value;
                RaisePropertyChanged("Tarifs");
                RaisePropertyChanged("CurrentTarif");
            }
        }

        public List<ObservableCollection<StringWrapper>> rawPotential;
        public List<ObservableCollection<StringWrapper>> RawPotential
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

        public List<ObservableCollection<StringWrapper>> needPotential;
        public List<ObservableCollection<StringWrapper>> NeedPotential
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

        public ObservableCollection<StringWrapper> CurrentRawPotential
        {
            get
            {
                if (Tables == null)
                    return new ObservableCollection<StringWrapper>
                    {
                            ""
                    };

                return RawPotential[CurrentTableIdx];
            }
        }

        public ObservableCollection<StringWrapper> CurrentNeedPotential
        {
            get
            {
                if (Tables == null)
                    return new ObservableCollection<StringWrapper>
                    {
                            ""
                    };

                return NeedPotential[CurrentTableIdx];
            }
        }
    }
}
