using Lab1.Model;
using Lab1.ViewModel;
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

namespace Lab1.View.Pages
{
    /// <summary>
    /// Interaction logic for CanonicalForm.xaml
    /// </summary>
    public partial class CanonicalForm : Page
    {
        public CanonicalForm()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //move canoniical form to symplex table
            var pageManager = Application.Current.MainWindow.DataContext as PageManager;

            var symplexVM = new SymplexTablesViewModels();
            try
            {
                symplexVM.CountTables(
                    SymplexTable.GetFromCanonicalForm(this.DataContext as CanonicalFormViewModel));

            }
            catch (InvalidOperationException exc)
            {
                MessageBox.Show("Оптимальний план не існує для таких вхідних даних. " + exc.Message,
                        "Помилка", MessageBoxButton.OK, MessageBoxImage.Warning);
                pageManager.CurrentPage = new Input();
                return;
            }

            pageManager.CurrentPage = new SymplexTables();
            pageManager.CurrentPage.DataContext = symplexVM;
        }
    }
}
