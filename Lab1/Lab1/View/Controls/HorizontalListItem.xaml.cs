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
    /// Interaction logic for HorizontalListItem.xaml
    /// </summary>
    public partial class HorizontalListItem : UserControl
    {


        public string Header
        {
            get { return (string)GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Header.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HeaderProperty =
            DependencyProperty.Register("Header", typeof(string), typeof(HorizontalListItem), new PropertyMetadata(""));




        public double Value
        {
            get { return (double)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Value.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(double), 
                typeof(HorizontalListItem), new PropertyMetadata(0.0), new ValidateValueCallback(IsValidReading));

        static bool IsValidReading(object value)
        {
            return true;
            //if (!Double.TryParse(value.ToString(), out double v))
            //    return false;
            //return !v.Equals(Double.NegativeInfinity) && !v.Equals(Double.PositiveInfinity) && v >= 0;
        }

        public HorizontalListItem()
        {
            InitializeComponent();
        }
        
        double tmp;

        private void text_GotFocus(object sender, RoutedEventArgs e)
        {
            tmp = Double.Parse(text.Text);
        }

        private void text_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!IsValidReading(text.Text))
                text.Text = tmp.ToString();
            else
                tmp = Double.Parse(text.Text);
        }
    }
}
