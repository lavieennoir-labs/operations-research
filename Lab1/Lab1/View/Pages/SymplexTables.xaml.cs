using Lab1.ViewModel;
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

namespace Lab1.View.Pages
{
    /// <summary>
    /// Interaction logic for SymplexTables.xaml
    /// </summary>
    public partial class SymplexTables : Page
    {

        public SymplexTables()
        {
            InitializeComponent();
        }

        private void ButtonFirst_Click(object sender, RoutedEventArgs e)
        {
            var dc = DataContext as SymplexTablesViewModels;
            dc.CurrentTableIdx = 0;
            DataContext = dc;
        }

        private void ButtonLast_Click(object sender, RoutedEventArgs e)
        {
            var dc = DataContext as SymplexTablesViewModels;
            dc.CurrentTableIdx = dc.SymplexTables.Count() - 1;
            DataContext = dc;
        }

        private void ButtonPrev_Click(object sender, RoutedEventArgs e)
        {
            var dc = DataContext as SymplexTablesViewModels;
            dc.CurrentTableIdx --;
            DataContext = dc;
        }

        private void ButtonNext_Click(object sender, RoutedEventArgs e)
        {
            var dc = DataContext as SymplexTablesViewModels;
            dc.CurrentTableIdx++;
            DataContext = dc;
        }
    }
}
