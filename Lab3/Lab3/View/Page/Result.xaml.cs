using Lab3.ViewModel;
using System;
using System.Collections.Generic;
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
    /// Interaction logic for Result.xaml
    /// </summary>
    public partial class Result : System.Windows.Controls.Page
    {
        public Result()
        {
            InitializeComponent();  
            var tables = DataContext as ResultTables;

            Loaded += (obj, e) => 
                tables.CurrentTableIdx = 0;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //lt
            var tables = DataContext as ResultTables;

            if(tables.CurrentTableIdx > 0)
                tables.CurrentTableIdx--;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //gt
            var tables = DataContext as ResultTables;

            if (tables.CurrentTableIdx < tables.TableCount - 1)
                tables.CurrentTableIdx++;
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            var pm = Application.Current.MainWindow.DataContext as PageManager;
            pm.CurrentPage = pm.loadedPages.Where(p => p is Input).First();
        }
    }
}
