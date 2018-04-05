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
        
        public string[] RawNames { get; set; }
        public ObservableCollection<double> Limits { get; set; }
        public ObservableCollection<string> LimitNames { get; set; }
        public ObservableCollection<double> Vars { get; set; }
        public ObservableCollection<string> VarNames { get; set; }
        public string[] ProductNames { get; set; }

        public ObservableCollection<ObservableCollection<double>> Coefs { get; set; }

        public InputViewModel()
        {
            LimitCount = 5;
            VarCount = 2;
            Limits = new ObservableCollection<double>
            {
                50, 80, 90, 10, 105
            };
            Vars = new ObservableCollection<double>
            {
                -3, -5
            };
            LimitNames = new ObservableCollection<string>
            {
                "A1", "A2","A3","A4", "A5"
            };
            VarNames = new ObservableCollection<string>
            {
                "B1", "B2"
            };
           
            Coefs = new ObservableCollection<ObservableCollection<double>>(
                new List<ObservableCollection<double>> { 
                new ObservableCollection<double>(new List<double> { 1, 0 }),
                new ObservableCollection<double>(new List<double> { 0, 1 }),
                new ObservableCollection<double>(new List<double> { 3, -2}),
                new ObservableCollection<double>(new List<double> { 1, -2}),
                new ObservableCollection<double>(new List<double> { 1, 1})
                });
        }
    }
}
