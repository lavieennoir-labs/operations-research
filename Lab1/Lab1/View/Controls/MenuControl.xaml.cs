using Lab1.Model;
using Lab1.View.Pages;
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

namespace Lab1.View.Controls
{
    /// <summary>
    /// Interaction logic for MenuControl.xaml
    /// </summary>
    public partial class MenuControl : UserControl
    {
        public MenuControl()
        {
            InitializeComponent();
        }

        private void MenuItemInput_Click(object sender, RoutedEventArgs e)
        {
            (Application.Current.MainWindow.DataContext as PageManager).CurrentPage = new Input();
        }

        private void MenuItemCanon_Click(object sender, RoutedEventArgs e)
        {
            var pm = (Application.Current.MainWindow.DataContext as PageManager);
            if (pm.CurrentPage.GetType().Equals(typeof(Input)))
            {
                pm.CurrentPage = new CanonicalForm()
                {
                    DataContext = new CanonicalFormConverter().Convert(pm.CurrentPage.DataContext as InputViewModel)
                };
            }
            else
            {
                pm.CurrentPage = new CanonicalForm()
                {
                    DataContext = new CanonicalFormConverter().Convert(new InputViewModel())
                };
            }

        }

        private void MenuItemSymplex_Click(object sender, RoutedEventArgs e)
        {
            var pm = (Application.Current.MainWindow.DataContext as PageManager);
            if (pm.CurrentPage.GetType().Equals(typeof(Input)))
            {
                var canonicalForm = new CanonicalFormConverter().Convert(pm.CurrentPage.DataContext as InputViewModel);

                var symplexVM = new SymplexTablesViewModels();
                try
                {
                    symplexVM.CountTables(
                        SymplexTable.GetFromCanonicalForm(canonicalForm));
                }
                catch (InvalidOperationException)
                {
                    MessageBox.Show("Оптимальний план не існує для таких вхідних даних.", 
                        "Помилка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    pm.CurrentPage = new Input();
                    return;
                }
                try
                {
                    pm.CurrentPage = new SymplexTables()
                    {
                        DataContext = symplexVM
                    };
                }
                catch
                {
                    MessageBox.Show("Оптимальний план не існує для таких вхідних даних.",
                        "Помилка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    pm.CurrentPage = new Input();
                    return;
                }
            }
            else if(pm.CurrentPage.GetType().Equals(typeof(CanonicalForm)))
            {
                var symplexVM = new SymplexTablesViewModels();
                try
                {
                    symplexVM.CountTables(
                        SymplexTable.GetFromCanonicalForm(pm.CurrentPage.DataContext as CanonicalFormViewModel));
                }
                catch (InvalidOperationException)
                {
                    MessageBox.Show("Оптимальний план не існує для таких вхідних даних.",
                        "Помилка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    pm.CurrentPage = new Input();
                    return;
                }
                try
                {
                    pm.CurrentPage = new SymplexTables();
                    pm.CurrentPage.DataContext = symplexVM;
                }
                catch
                {
                    MessageBox.Show("Оптимальний план не існує для таких вхідних даних.",
                        "Помилка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    pm.CurrentPage = new Input();
                    return;
                }
            }
        }
    }
}
