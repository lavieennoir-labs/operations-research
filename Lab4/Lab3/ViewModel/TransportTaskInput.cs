using Lab3.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab3.ViewModel
{
    class TransportTaskInput : ViewModelBase
    {
        public int RawCount { get; set; }
        public int NeedCount { get; set; }
        public ObservableCollection<DoubleWrapper> Raws { get; set; }
        public ObservableCollection<DoubleWrapper> Needs { get; set; }
        public ObservableCollection<ObservableCollection<DoubleWrapper>> Cost { get; set; }
        public BasicPlan BasicPlan { get; set; }

        //default values
        public void GenerateMyInput()
        {
            RawCount = 5;
            NeedCount = 4;
            Raws = new ObservableCollection<DoubleWrapper>
            {
                14, 30, 20, 32, 16
            };
            Needs = new ObservableCollection<DoubleWrapper>
            {
                60, 14, 20, 18
            };
            Cost = new ObservableCollection<ObservableCollection<DoubleWrapper>>
            {
                new ObservableCollection<DoubleWrapper>
                {
                    7.3, 9, 3, 10
                },
                new ObservableCollection<DoubleWrapper>
                {
                    3, 10, 5, 9
                },
                new ObservableCollection<DoubleWrapper>
                {
                    7, 11, 3, 2
                },
                new ObservableCollection<DoubleWrapper>
                {
                    8, 5, 9, 2
                },
                new ObservableCollection<DoubleWrapper>
                {
                    4.8, 9, 10, 5
                },
            };
        }

        public void GenerateTestInput()
        {
            RawCount = 4;
            NeedCount = 5;
            Raws = new ObservableCollection<DoubleWrapper>
            {
                100, 250, 200, 300
            };
            Needs = new ObservableCollection<DoubleWrapper>
            {
                200, 200, 100, 100, 250
            };
            Cost = new ObservableCollection<ObservableCollection<DoubleWrapper>>
            {
                new ObservableCollection<DoubleWrapper>
                {
                    10, 7, 4, 1, 4
                },
                new ObservableCollection<DoubleWrapper>
                {
                    2, 7, 10, 6, 11
                },
                new ObservableCollection<DoubleWrapper>
                {
                    8, 5, 3, 2, 2
                },
                new ObservableCollection<DoubleWrapper>
                {
                    11, 8, 12, 16, 13
                }
            };
        }

        public void GenerateTest4Input()
        {
            RawCount = 3;
            NeedCount = 4;
            Raws = new ObservableCollection<DoubleWrapper>
            {
                160, 140, 170
            };
            Needs = new ObservableCollection<DoubleWrapper>
            {
                120, 50, 190, 110
            };
            Cost = new ObservableCollection<ObservableCollection<DoubleWrapper>>
            {
                new ObservableCollection<DoubleWrapper>
                {
                    7, 8, 1, 2
                },
                new ObservableCollection<DoubleWrapper>
                {
                    4, 5, 9, 8
                },
                new ObservableCollection<DoubleWrapper>
                {
                    9, 2, 3, 6
                }
            };
        }

        public TransportTaskInput()
        {
            GenerateMyInput();
        }
    }
}
