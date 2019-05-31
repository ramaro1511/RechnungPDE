using System;
using System.Windows;
using System.Windows.Controls;

namespace UI.Desktop.Main.Pages
{
    /// <summary>
    /// Interaction logic for InvoiceInfoPage.xaml
    /// </summary>
    public partial class InvoiceInfoPage : Page
    {
        MainWindow mainWindow;

        public InvoiceInfoPage()
        {
            InitializeComponent();
        }

        public void setMainWindow(MainWindow _mainWindow)
        {
            mainWindow = _mainWindow;
        }

        public void setUp()
        {
            InvoiceDatePicker.SelectedDate = DateTime.Today;
        }

        private void FileNameCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            FileNameTextBox.IsEnabled = true;
        }

        private void FileNameCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            FileNameTextBox.IsEnabled = false;
        }

        private void FileNameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            mainWindow.mainViewModel.fileName = FileNameTextBox.Text;
            mainWindow.regexCheck();
        }

        private void InvoiceNumberTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            mainWindow.mainViewModel.invoiceNumber = InvoiceNumberTextBox.Text;

            if (FileNameCheckBox.IsChecked == false)
                mainWindow.buildFileName(sender, e);
        }

        private void InvoiceDatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            mainWindow.mainViewModel.invoiceDate = string.Format("{0:dd.MM.yyyy}", InvoiceDatePicker.SelectedDate);
        }

        private void CustomerNumberTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            mainWindow.mainViewModel.customerNumber = CustomerNumberTextBox.Text;
        }

        private void InvoiceDatePicker_GotFocus(object sender, RoutedEventArgs e)
        {
            DatePicker control = sender as DatePicker;

            mainWindow.controlGotFocus = true;

            if (control.ToolTip != null)
                mainWindow.TooltipLabel.Content = control.ToolTip;
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
