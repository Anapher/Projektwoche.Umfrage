using System;
using System.Windows;
using System.Windows.Controls;
using MahApps.Metro.Controls;
using Projektwoche.Umfrage.Auswertung.ViewModels;

namespace Projektwoche.Umfrage.Auswertung
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel();
            Loaded += OnLoaded;
            Title = $"JGS Umfrage Auswertung [ID: {MainViewModel.MacAddress}]";
        }

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            ((MainViewModel) DataContext).InitializeWebInfo();
        }

        private void BirthyearShortcutButtonOnClick(object sender, RoutedEventArgs e)
        {
            BirthyearNumericUpDown.Value = int.Parse(((Button) sender).Content.ToString());
        }

        private void NumericUpDown1OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double?> e)
        {
            Switch(sender, Reverse2NumericUpDown);
        }

        private void Switch(object sender, NumericUpDown next)
        {
            var source = (NumericUpDown) sender;
            if (source.Value != null)
                next.Focus();
        }

        private void Reverse2NumericUpDown_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double?> e)
        {
            Switch(sender, Reverse3NumericUpDown);
        }

        private void Reverse3NumericUpDown_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double?> e)
        {
            Switch(sender, Reverse4NumericUpDown);
        }

        private void Reverse4NumericUpDown_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double?> e)
        {
            Switch(sender, Reverse5NumericUpDown);
        }

        private void Reverse5NumericUpDown_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double?> e)
        {
            Switch(sender, Reverse6NumericUpDown);
        }

        private void Reverse6NumericUpDown_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double?> e)
        {
            Switch(sender, Reverse7NumericUpDown);
        }
    }
}