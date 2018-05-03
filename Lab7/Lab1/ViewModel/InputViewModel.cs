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
            InitData();
        }

        public void InitData()
        {
            LimitCount = 2;
            VarCount = 2;

            Vars = new ObservableCollection<Var>
            {
                new Var{Value = 1 },
                new Var{Value = 1 }
            };
            LimitNames = new ObservableCollection<string>
            {
                "A1", "A2"
            };
            Limits = new ObservableCollection<Limit>
            {

                new Limit{Value = 2, Sign = "<=" },
                new Limit{Value = 2, Sign = "<=" },
            };
            VarNames = new ObservableCollection<string>
            {
                "B1", "B2"
            };

            Coefs = new ObservableCollection<ObservableCollection<double>>{
                new ObservableCollection<double> { 2, 1 },
                new ObservableCollection<double> { 1, 2 }
            };
        }

        public void InitData2()
        {
            LimitCount = 2;
            VarCount = 2;

            Vars = new ObservableCollection<Var>
            {
                new Var{Value = 3 },
                new Var{Value = 1 }
            };
            LimitNames = new ObservableCollection<string>
            {
                "A1", "A2"
            };
            Limits = new ObservableCollection<Limit>
            {

                new Limit{Value = 6, Sign = "<=" },
                new Limit{Value = 3, Sign = "<=" },
            };
            VarNames = new ObservableCollection<string>
            {
                "B1", "B2"
            };

            Coefs = new ObservableCollection<ObservableCollection<double>>{
                new ObservableCollection<double> { 2, 3 },
                new ObservableCollection<double> { 2, -3 }
            };
        }


        void InitTest()
        {
            LimitCount = 2;
            VarCount = 2;

            Vars = new ObservableCollection<Var>
            {
                new Var{Value = 8 },
                new Var{Value = 6 }
            };
            LimitNames = new ObservableCollection<string>
            {
                "A1", "A2"
            };
            Limits = new ObservableCollection<Limit>
            {

                new Limit{Value = 19, Sign = "<=" },
                new Limit{Value = 16, Sign = "<=" },
            };
            VarNames = new ObservableCollection<string>
            {
                "B1", "B2"
            };

            Coefs = new ObservableCollection<ObservableCollection<double>>{
                new ObservableCollection<double> { 2, 5 },
                new ObservableCollection<double> { 4, 1 }
            };
        }        
    }
}
