using Lab3.Model;
using Lab3.Model.DiferentialRents;
using Lab3.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Lab3.View.Page
{
    /// <summary>
    /// Interaction logic for Input.xaml
    /// </summary>
    public partial class Input : System.Windows.Controls.Page
    {
        public Input()
        {
            InitializeComponent();
        }

        private void AddRawClick(object sender, RoutedEventArgs e)
        {
            var input = DataContext as TransportTaskInput;

            input.RawCount++;
            input.Raws.Add(0);
            input.Cost.Add(
                new ObservableCollection<DoubleWrapper>(
                    Enumerable.Range(0, input.NeedCount).Select(i => new DoubleWrapper())));
        }

        private void RemoveRawClick(object sender, RoutedEventArgs e)
        {
            var input = DataContext as TransportTaskInput;
            if (input.RawCount <= 2)
                return;

            input.RawCount--;
            input.Raws.RemoveAt(input.RawCount);
            input.Cost.RemoveAt(input.RawCount);
        }

        private void AddNeedClick(object sender, RoutedEventArgs e)
        {
            var input = DataContext as TransportTaskInput;

            input.NeedCount++;
            input.Needs.Add(0);
            for (int i = 0; i < input.RawCount; i++)
                input.Cost[i].Add(0);
        }

        private void RemoveNeedClick(object sender, RoutedEventArgs e)
        {
            var input = DataContext as TransportTaskInput;
            if (input.NeedCount <= 2)
                return;

            input.NeedCount--;
            input.Needs.RemoveAt(input.NeedCount);
            for (int i = 0; i < input.RawCount; i++)
                input.Cost[i].RemoveAt(input.NeedCount);
        }

        //private void Button_Click(object sender, RoutedEventArgs e)
        //{
        //    var input = DataContext as TransportTaskInput;
        //    //create array;
        //    double[,] cost = new double[input.RawCount, input.NeedCount];
        //    for (int i = 0; i < input.RawCount; i++)
        //        for (int j = 0; j < input.NeedCount; j++)
        //            cost[i, j] = input.Cost[i][j];
        //    //solve task
        //    TransportTask transportTask = new TransportTask
        //    {
        //        RawCount = input.RawCount,
        //        NeedCount = input.NeedCount,
        //        Raw = input.Raws.Select(dw => dw.Value).ToArray(),
        //        Need = input.Needs.Select(dw => dw.Value).ToArray(),
        //        RawClone = input.Raws.Select(dw => dw.Value).ToArray(),
        //        NeedClone = input.Needs.Select(dw => dw.Value).ToArray(),
        //        Cost = cost,
        //        CostOriginal = (double[,])cost.Clone(),
        //        BasicPlan = input.BasicPlan
        //    };

        //    transportTask.Solve();
        //    var total = transportTask.GetTotalCost();

        //    var tables = new ResultTables();
        //    tables.Tables = transportTask.CountTables.
        //        Select(ct => TableGenerator.GetCountTable(ct)).ToList();
        //    tables.RawPotential = transportTask.RawPotentialTables.
        //        Select(ct => TableGenerator.GetPotentialTable(ct)).ToList();
        //    tables.NeedPotential = transportTask.NeedPotentialTables.
        //        Select(ct => TableGenerator.GetPotentialTable(ct)).ToList();
        //    tables.TotalSum = total;
        //    tables.BasisIndexes = transportTask.BasisIndexes;

        //    var pm = Application.Current.MainWindow.DataContext as PageManager;
        //    pm.loadedPages.Where(p => p is Result).First().DataContext = tables;
        //    pm.CurrentPage = pm.loadedPages.Where(p => p is Result).First();
        //    //pm.CurrentPage.DataContext = tables;
        //    (pm.CurrentPage.DataContext as ResultTables).Tables = tables.Tables;
        //    //RaisePropertyChanged("CurrentTable");
        //}
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var input = DataContext as TransportTaskInput;
            //create array;
            double[,] cost = new double[input.RawCount, input.NeedCount];
            for (int i = 0; i < input.RawCount; i++)
                for (int j = 0; j < input.NeedCount; j++)
                    cost[i, j] = input.Cost[i][j];
            //solve task
            DiffRent transportTask = new DiffRent
            {
                RawCount = input.RawCount,
                NeedCount = input.NeedCount,
                Raw = input.Raws.Select(dw => dw.Value).ToArray(),
                Need = input.Needs.Select(dw => dw.Value).ToArray(),
                RawClone = input.Raws.Select(dw => dw.Value).ToArray(),
                NeedClone = input.Needs.Select(dw => dw.Value).ToArray(),
                Cost = cost,
                CostOriginal = (double[,])cost.Clone()
            };

            transportTask.Solve();
            var total = transportTask.GetTotalCost();

            var tables = new ResultTables();
            tables.Tarifs = transportTask.MinimalTarifLists.
                Select(ct => TableGenerator.GetTarifList(ct)).ToList();
            tables.Tables = transportTask.CountTables.
                Select(ct => TableGenerator.GetCountTable(ct)).ToList();
            tables.Costs = transportTask.CostTables.
                Select(ct => TableGenerator.GetCountTable(ct)).ToList();
            //надлишок
            tables.RawPotential = transportTask.SatisfiedLists.
                Select(ct => TableGenerator.GetSatisfiedTable(ct)).ToList();
            //різниця
            tables.NeedPotential = transportTask.RentaLists.
                Select(ct => TableGenerator.GetRentaTable(ct)).ToList();
            tables.TotalSum = total;
            tables.BasisIndexes = null;

            var pm = Application.Current.MainWindow.DataContext as PageManager;
            pm.loadedPages.Where(p => p is Result).First().DataContext = tables;
            pm.CurrentPage = pm.loadedPages.Where(p => p is Result).First();
            //pm.CurrentPage.DataContext = tables;
            (pm.CurrentPage.DataContext as ResultTables).Tables = tables.Tables;
            //RaisePropertyChanged("CurrentTable");
        }
    }
}
