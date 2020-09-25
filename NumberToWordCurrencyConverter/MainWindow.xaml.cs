using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Text.RegularExpressions;

namespace NumberToWordCurrencyConverter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9,-]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void NumberTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (NumberTextBox.Text == "")
                ResultTextBlock.Text = "";
        }

        private void btnConvert_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                NumberToWordConverterService.NumberToWordConverterServiceClient client = new 
                    NumberToWordConverterService.NumberToWordConverterServiceClient();
                string numAsString = NumberTextBox.Text.Trim();
                ResultTextBlock.Text = client.getNumber(numAsString);
            }
            catch (Exception)
            {
                ResultTextBlock.Text = "Some unexpected error occured! Please try again.";
            }
        }
    }
}
