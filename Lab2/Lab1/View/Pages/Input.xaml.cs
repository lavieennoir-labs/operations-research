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
            var vm = this.DataContext as InputViewModel;
            vm.VarCount = InputTable.VarNames.Count();
            vm.LimitCount = InputTable.LimitNames.Count();

            var canonicalForm = new CanonicalFormConverter().Convert(vm);

            var pageManager = Application.Current.MainWindow.DataContext as PageManager;
            pageManager.CurrentPage = new CanonicalForm();
            pageManager.CurrentPage.DataContext = canonicalForm;
        }
    }
}
