using Lab1.Model;
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
    /// Interaction logic for Input.xaml
    /// </summary>
    public partial class Input : Page
    {
        public Input()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //convert current data to to canoniical form
            var canonicalForm = new CanonicalFormConverter().Convert(this.DataContext as InputViewModel);

            var pageManager = Application.Current.MainWindow.DataContext as PageManager;
            pageManager.CurrentPage = new CanonicalForm();
            pageManager.CurrentPage.DataContext = canonicalForm;
        }

        private void matrix_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            var tb = (e.EditingElement as TextBox);
            if (!IsValidReading(tb.Text))
                tb.Text = tmp.ToString();
        }

        bool IsValidReading(string value)
        {
            if (!Double.TryParse(value, out Double v))
                return false;
            return !v.Equals(Double.NegativeInfinity) && !v.Equals(Double.PositiveInfinity) && v >= 0;
        }

        private void matrix_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            var m = (DataContext as InputViewModel).ProductCost;
            tmp = m[matrix.SelectedIndex][e.Column.DisplayIndex];
        }
        double tmp;
    }
}
