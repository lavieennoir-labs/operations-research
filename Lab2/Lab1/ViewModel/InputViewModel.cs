using Lab1.Model;
using Lab1.View.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1.ViewModel
{
    class InputViewModel : ViewModel
    {
        public int LimitCount { get; set; }
        public int VarCount { get; set; }
        
        public ObservableCollection<Limit> Limits { get; set; }
        public ObservableCollection<string> LimitNames { get; set; }
        public ObservableCollection<Var> Vars { get; set; }
        public ObservableCollection<string> VarNames { get; set; }
        public ObservableCollection<string> Signs { get; set; }

        public ObservableCollection<ObservableCollection<double>> Coefs { get; set; }

        public InputViewModel()
        {
            InitDualSimplex();
        }

        void InitSimplex()
        {
            LimitCount = 2;
            VarCount = 5;

            Vars = new ObservableCollection<Var>
            {
                new Var{Value = -50 },
                new Var{Value = -80 },
                new Var{Value = -10 },
                new Var{Value = -240 },
                new Var{Value = 0 },
                new Var{Value = 10 },
                new Var{Value = 20},
            };
            LimitNames = new ObservableCollection<string>
            {
                "A1", "A2"
            };
            Limits = new ObservableCollection<Limit>
            {

                new Limit{Value = 2, Sign = "<=" },
                new Limit{Value = 4, Sign = "<=" },
            };
            VarNames = new ObservableCollection<string>
            {
                "B1", "B2", "B3", "B4", "B5", "B6", "B7"
            };

            Coefs = new ObservableCollection<ObservableCollection<double>>{
                new ObservableCollection<double> { -1, 0, -1, -4, 3, 1, 1 },
                new ObservableCollection<double> { 0, -1, 2, -1, -1, 1, 4 }
            };
        }

        void InitDualSimplex()
        {
            LimitCount = 5;
            VarCount = 2;

            Vars = new ObservableCollection<Var>
            {
                new Var { Value = 2 },
                new Var { Value = 4 }
            };
            LimitNames = new ObservableCollection<string>
            {
                "A1", "A2", "A3", "A4", "A5", "A6", "A7"
            };
            Limits = new ObservableCollection<Limit>
            {
                new Limit{Value = 50, Sign = "<=" },
                new Limit{Value = 80, Sign = "<=" },
                new Limit{Value = -10, Sign = ">=" },
                new Limit{Value = 240, Sign = "<=" },
                new Limit{Value = 0, Sign = "<=" },
                new Limit{Value = 10, Sign = ">=" },
                new Limit{Value = 20, Sign = ">=" },
            };
            VarNames = new ObservableCollection<string>
            {
                "B1", "B2"
            };

            Coefs = new ObservableCollection<ObservableCollection<double>>{
                new ObservableCollection<double> { 1, 0 },
                new ObservableCollection<double> { 0, 1 },
                new ObservableCollection<double> { -1, 2},
                new ObservableCollection<double> { 4, 1},
                new ObservableCollection<double> { -3, 1},
                new ObservableCollection<double> { 1, 1},
                new ObservableCollection<double> { 1, 4}
                };
        }
    }
}
