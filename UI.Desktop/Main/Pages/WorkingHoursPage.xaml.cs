using System.Globalization;
using System.Windows;
using System.Windows.Controls;

namespace UI.Desktop.Main.Pages
{
    /// <summary>
    /// Interaction logic for WorkingHoursPage.xaml
    /// </summary>
    public partial class WorkingHoursPage : Page
    {
        MainWindow mainWindow;

        public WorkingHoursPage()
        {
            InitializeComponent();
        }

        public void setMainWindow(MainWindow _mainWindow)
        {
            mainWindow = _mainWindow;
        }

        public void setUp()
        {
            WageTextBox.Text = "0" + NumberFormatInfo.CurrentInfo.CurrencyDecimalSeparator + "00";
            InvoiceCommentTextBox.Text = "Vielen Dank für Ihren Auftrag. Bitte überweisen Sie den Rechnungsbetrag innerhalb von 7 Tagen auf das o.a. Konto nach Erhalt der Rechnung.";
        }

        private void BusinessHoursCountTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            decimal businessHours;

            if (!string.IsNullOrWhiteSpace(BusinessHoursCountTextBox.Text))
            {
                if (BusinessHoursCountTextBox.Text.Contains("."))
                    BusinessHoursCountTextBox.Text = BusinessHoursCountTextBox.Text.Replace('.', char.Parse(NumberFormatInfo.CurrentInfo.CurrencyDecimalSeparator));
                else if (BusinessHoursCountTextBox.Text.Contains(","))
                    BusinessHoursCountTextBox.Text = BusinessHoursCountTextBox.Text.Replace(',', char.Parse(NumberFormatInfo.CurrentInfo.CurrencyDecimalSeparator));

                if (decimal.TryParse(BusinessHoursCountTextBox.Text, out businessHours))
                {
                    mainWindow.mainViewModel.businessHours = decimal.Parse(BusinessHoursCountTextBox.Text);
                }
            }
        }

        private void WageTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            decimal hourlyWage;

            if (!string.IsNullOrWhiteSpace(WageTextBox.Text))
            {
                if (WageTextBox.Text.Contains("."))
                    WageTextBox.Text = WageTextBox.Text.Replace('.', char.Parse(NumberFormatInfo.CurrentInfo.CurrencyDecimalSeparator));
                else if (WageTextBox.Text.Contains(","))
                    WageTextBox.Text = WageTextBox.Text.Replace(',', char.Parse(NumberFormatInfo.CurrentInfo.CurrencyDecimalSeparator));

                if (decimal.TryParse(WageTextBox.Text, out hourlyWage))
                {
                    if (!WageTextBox.Text.Contains(NumberFormatInfo.CurrentInfo.CurrencyDecimalSeparator))
                        WageTextBox.Text += NumberFormatInfo.CurrentInfo.CurrencyDecimalSeparator + "00";

                    mainWindow.mainViewModel.hourlyWage = decimal.Parse(WageTextBox.Text);
                }
            }
        }

        private void InvoiceCommentTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            mainWindow.mainViewModel.invoiceComment = InvoiceCommentTextBox.Text;
        }

        private void InvoiceTextTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            mainWindow.mainViewModel.invoiceText = InvoiceTextTextBox.Text;
        }

        private void AddAssistentButton_Click(object sender, RoutedEventArgs e)
        {
            AssistentBusinessHoursCountLabel.IsEnabled = true;
            AssistentBusinessHoursCountTextBox.IsEnabled = true;
            AssistentWageTextBox.Text = "0" + NumberFormatInfo.CurrentInfo.CurrencyDecimalSeparator + "00";
            AssistentWageLabel.IsEnabled = true;
            AssistentWageTextBox.IsEnabled = true;
            AddAssistentButton.IsEnabled = false;
            RemoveAssistentButton.IsEnabled = true;
            AddAssistentButton.IsEnabled = false;
            RemoveAssistentButton.IsEnabled = true;

            AddAssistent2Button.IsEnabled = true;

            mainWindow.mainViewModel.assistentIsEnabled = true;
        }

        private void RemoveAssistentButton_Click(object sender, RoutedEventArgs e)
        {
            AssistentBusinessHoursCountLabel.IsEnabled = false;
            AssistentBusinessHoursCountTextBox.IsEnabled = false;
            AssistentBusinessHoursCountTextBox.Text = "";
            AssistentWageLabel.IsEnabled = false;
            AssistentWageTextBox.IsEnabled = false;
            AssistentWageTextBox.Text = "";
            AddAssistentButton.IsEnabled = true;
            RemoveAssistentButton.IsEnabled = false;

            Assistent2BusinessHoursCountLabel.IsEnabled = false;
            Assistent2BusinessHoursCountTextBox.IsEnabled = false;
            Assistent2WageLabel.IsEnabled = false;
            Assistent2WageTextBox.IsEnabled = false;
            AddAssistent2Button.IsEnabled = false;

            mainWindow.mainViewModel.assistentIsEnabled = false;
        }

        private void AddAssistent2Button_Click(object sender, RoutedEventArgs e)
        {
            Assistent2BusinessHoursCountLabel.IsEnabled = true;
            Assistent2BusinessHoursCountTextBox.IsEnabled = true;
            Assistent2WageTextBox.Text = "0" + NumberFormatInfo.CurrentInfo.CurrencyDecimalSeparator +"00";
            Assistent2WageLabel.IsEnabled = true;
            Assistent2WageTextBox.IsEnabled = true;
            AddAssistent2Button.IsEnabled = false;
            RemoveAssistent2Button.IsEnabled = true;

            RemoveAssistentButton.IsEnabled = false;
            mainWindow.mainViewModel.assistent2IsEnabled = true;
        }

        private void RemoveAssistent2Button_Click(object sender, RoutedEventArgs e)
        {
            Assistent2BusinessHoursCountLabel.IsEnabled = false;
            Assistent2BusinessHoursCountTextBox.IsEnabled = false;
            Assistent2BusinessHoursCountTextBox.Text = "";
            Assistent2WageLabel.IsEnabled = false;
            Assistent2WageTextBox.IsEnabled = false;
            Assistent2WageTextBox.Text = "";
            AddAssistent2Button.IsEnabled = true;
            RemoveAssistent2Button.IsEnabled = false;

            RemoveAssistentButton.IsEnabled = true;
            mainWindow.mainViewModel.assistent2IsEnabled = false;
        }

        private void AssistentBusinessHoursCountTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            decimal assistentBusinessHours;

            if (!string.IsNullOrWhiteSpace(AssistentBusinessHoursCountTextBox.Text))
            {
                if (AssistentBusinessHoursCountTextBox.Text.Contains("."))
                    AssistentBusinessHoursCountTextBox.Text = AssistentBusinessHoursCountTextBox.Text.Replace('.', char.Parse(NumberFormatInfo.CurrentInfo.CurrencyDecimalSeparator));
                else if (AssistentBusinessHoursCountTextBox.Text.Contains(","))
                    AssistentBusinessHoursCountTextBox.Text = AssistentBusinessHoursCountTextBox.Text.Replace(',', char.Parse(NumberFormatInfo.CurrentInfo.CurrencyDecimalSeparator));

                if (decimal.TryParse(AssistentBusinessHoursCountTextBox.Text, out assistentBusinessHours))
                {
                    mainWindow.mainViewModel.assistentBusinessHours = decimal.Parse(AssistentBusinessHoursCountTextBox.Text);
                }
            }
        }

        private void AssistentWageTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            decimal assistentWage;

            if (!string.IsNullOrWhiteSpace(AssistentWageTextBox.Text))
            {
                if (AssistentWageTextBox.Text.Contains("."))
                    AssistentWageTextBox.Text = AssistentWageTextBox.Text.Replace('.', char.Parse(NumberFormatInfo.CurrentInfo.CurrencyDecimalSeparator));
                else if (AssistentWageTextBox.Text.Contains(","))
                    AssistentWageTextBox.Text = AssistentWageTextBox.Text.Replace(',', char.Parse(NumberFormatInfo.CurrentInfo.CurrencyDecimalSeparator));

                if (decimal.TryParse(AssistentWageTextBox.Text, out assistentWage))
                {
                    if (!AssistentWageTextBox.Text.Contains(NumberFormatInfo.CurrentInfo.CurrencyDecimalSeparator))
                        WageTextBox.Text += NumberFormatInfo.CurrentInfo.CurrencyDecimalSeparator + "00";

                    mainWindow.mainViewModel.assistentWage = decimal.Parse(AssistentWageTextBox.Text);
                }
            }
        }

        private void Assistent2BusinessHoursCountTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            decimal assistent2BusinessHours;

            if (!string.IsNullOrWhiteSpace(Assistent2BusinessHoursCountTextBox.Text))
            {
                if (Assistent2BusinessHoursCountTextBox.Text.Contains("."))
                    Assistent2BusinessHoursCountTextBox.Text = Assistent2BusinessHoursCountTextBox.Text.Replace('.', char.Parse(NumberFormatInfo.CurrentInfo.CurrencyDecimalSeparator));
                else if (Assistent2BusinessHoursCountTextBox.Text.Contains(","))
                    Assistent2BusinessHoursCountTextBox.Text = Assistent2BusinessHoursCountTextBox.Text.Replace(',', char.Parse(NumberFormatInfo.CurrentInfo.CurrencyDecimalSeparator));

                if (decimal.TryParse(Assistent2BusinessHoursCountTextBox.Text, out assistent2BusinessHours))
                {
                    mainWindow.mainViewModel.assistent2BusinessHours = decimal.Parse(Assistent2BusinessHoursCountTextBox.Text);
                }
            }
        }

        private void Assistent2WageTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            decimal assistentWage2;

            if (!string.IsNullOrWhiteSpace(Assistent2WageTextBox.Text))
            {
                if (Assistent2WageTextBox.Text.Contains("."))
                    Assistent2WageTextBox.Text = Assistent2WageTextBox.Text.Replace('.', char.Parse(NumberFormatInfo.CurrentInfo.CurrencyDecimalSeparator));
                else if (Assistent2WageTextBox.Text.Contains(","))
                    Assistent2WageTextBox.Text = Assistent2WageTextBox.Text.Replace(',', char.Parse(NumberFormatInfo.CurrentInfo.CurrencyDecimalSeparator));

                if (decimal.TryParse(Assistent2WageTextBox.Text, out assistentWage2))
                {
                    if (!Assistent2WageTextBox.Text.Contains(NumberFormatInfo.CurrentInfo.CurrencyDecimalSeparator))
                        WageTextBox.Text += NumberFormatInfo.CurrentInfo.CurrencyDecimalSeparator + "00";

                    mainWindow.mainViewModel.assistent2Wage = decimal.Parse(Assistent2WageTextBox.Text);
                }
            }
        }


        private void Control_MouseEnter(object sender, RoutedEventArgs e)
        {
            Control control = sender as Control;

            if (!mainWindow.controlGotFocus)
            {
                mainWindow.TooltipLabel.Content = control.ToolTip;
            }
        }

        private void Control_MouseLeave(object sender, RoutedEventArgs e)
        {
            if (!mainWindow.controlGotFocus)
            {
                mainWindow.TooltipLabel.Content = "";
            }
        }

        public void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox control = sender as TextBox;

            mainWindow.controlGotFocus = true;

            if (control.ToolTip != null)
                mainWindow.TooltipLabel.Content = control.ToolTip;
        }

        public void Control_LostFocus(object sender, RoutedEventArgs e)
        {
            mainWindow.controlGotFocus = false;
            mainWindow.TooltipLabel.Content = "";
        }
    }
}
