﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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

namespace Lab1.View
{
    /// <summary>
    /// Interaction logic for LPInputTable.xaml
    /// </summary>
    public partial class LPInputTable : UserControl
    {
        public LPInputTable()
        {

        }

        private void AddLimitClick(object sender, RoutedEventArgs e)
        {
        	if(LimitNames.Count() >= MaxLimitCount)
        		return;
            LimitNames.Add(LimitNames.First()[0].ToString() + LimitNames.Count());
            for(int i = 0; i < Coefs.Count(); i++)
            	Coefs[i].Add(0);
        }
        private void RemoveLimitClick(object sender, RoutedEventArgs e)
        {
        	if(LimitNames.Count() <= MinLimitCount)
        		return;
            LimitNames.RemoveAt(LimitNames.Count() - 1);
            for(int i = 0; i < Coefs.Count(); i++)
            	Coefs[i].RemoveAt(Coefs[i].Count() - 1);
        }
        private void AddVarClick(object sender, RoutedEventArgs e)
        {
        	if(VarNames.Count() >= MaxVarCount)
        		return;
            VarNames.Add(VarNames.First()[0].ToString() + VarNames.Count());
            Coefs.Add(new ObservableCollection(new double[LimitNames.Count()]));
        }
        private void RemoveVarClick(object sender, RoutedEventArgs e)
        {
        	if(VarNames.Count() <= MinVarCount)
        		return;
            VarNames.RemoveAt(VarNames.Count() - 1);
            Coefs.RemoveAt(VarNames.Count() - 1);
        }


        public int MinLimitCount { get; set; } = 2;
        public int MinVarCount { get; set; } = 2;
        public int MaxLimitCount { get; set; } = 10;
        public int MaxVarCount { get; set; } = 10;

        public ObservableCollection<ObservableCollection<double>> Coefs
        {
            get { return (ObservableCollection<ObservableCollection<double>>) GetValue(CoefsProperty); }
            set { SetValue(CoefsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for LimitValues.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CoefsProperty =
            DependencyProperty.Register("Coefs", typeof(ObservableCollection<ObservableCollection<double>>), typeof(LPInputTable), new PropertyMetadata(new ObservableCollection<ObservableCollection<double>>()));


        public ObservableCollection<double> LimitValues
        {
            get { return (ObservableCollection<double>)GetValue(LimitValuesProperty); }
            set { SetValue(LimitValuesProperty, value); }
        }

        // Using a DependencyProperty as the backing store for LimitValues.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LimitValuesProperty =
            DependencyProperty.Register("LimitValues", typeof(ObservableCollection<double>), typeof(LPInputTable), new PropertyMetadata(new ObservableCollection<double>()));


        public ObservableCollection<string> LimitNames
        {
            get { return (ObservableCollection<string>)GetValue(LimitNamesProperty); }
            set { SetValue(LimitNamesProperty, value); }
        }

        // Using a DependencyProperty as the backing store for LimitNames.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LimitNamesProperty =
            DependencyProperty.Register("LimitNames", typeof(ObservableCollection<string>), typeof(LPInputTable), new PropertyMetadata(new ObservableCollection<string>()));

        public ObservableCollection<string> VarNames
        {
            get { return (ObservableCollection<string>)GetValue(VarNamesProperty); }
            set { SetValue(VarNamesProperty, value); }
        }

        // Using a DependencyProperty as the backing store for VarNames.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty VarNamesProperty =
            DependencyProperty.Register("VarNames", typeof(ObservableCollection<string>), typeof(LPInputTable), new PropertyMetadata(new ObservableCollection<string>()));

        public ObservableCollection<double> VarValues
        {
            get { return (ObservableCollection<double>)GetValue(VarValuesProperty); }
            set { SetValue(VarValuesProperty, value); }
        }

        // Using a DependencyProperty as the backing store for VarValues.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty VarValuesProperty =
            DependencyProperty.Register("VarValues", typeof(ObservableCollection<double>), typeof(LPInputTable), new PropertyMetadata(new ObservableCollection<double>()));

    }
}
