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
        public int RawCount { get; set; }
        public int ProductCount { get; set; }

        public ObservableCollection<HorizontalListItem> Raw { get; set; }        
        public ObservableCollection<HorizontalListItem> Products { get; set; }

        public string[] RawNames { get; set; }
        public string[] ProductNames { get; set; }

        public ObservableCollection<ObservableCollection<double>> ProductCost { get; set; }

        public InputViewModel()
        {
            RawCount = 4;
            ProductCount = 4;

            Raw = new ObservableCollection<HorizontalListItem>(new List<HorizontalListItem>
            {
                new HorizontalListItem()
                {
                    Header = "A1",
                    Value = 1260
                },
                new HorizontalListItem()
                {
                    Header = "A2",
                    Value = 900
                },
                new HorizontalListItem()
                {
                    Header = "A3",
                    Value = 530
                },
                new HorizontalListItem()
                {
                    Header = "A4",
                    Value = 210
                },
            });

            Products = new ObservableCollection<HorizontalListItem>(new List<HorizontalListItem>
            {
                new HorizontalListItem()
                {
                    Header = "B1",
                    Value = 8
                },
                new HorizontalListItem()
                {
                    Header = "B2",
                    Value = 10
                },
                new HorizontalListItem()
                {
                    Header = "B3",
                    Value = 12
                },
                new HorizontalListItem()
                {
                    Header = "B4",
                    Value = 18
                },
            });

            RawNames = new string[] { "A1", "A2", "A3", "A4" };
            ProductNames = new string[] { "B1", "B2", "B3", "B4" };

            ProductCost = new ObservableCollection<ObservableCollection<double>>(
                new List<ObservableCollection<double>> { 
                new ObservableCollection<double>(new List<double> { 2, 4, 6, 8 }),
                new ObservableCollection<double>(new List<double> { 2, 2, 0, 6 }),
                new ObservableCollection<double>(new List<double> { 0, 1, 1, 2}),
                new ObservableCollection<double>(new List<double> { 1, 0, 1, 0})
                });
        }
    }
}
