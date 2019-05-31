using Logic.UI;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;

namespace UI.Desktop.Main.Pages
{
    /// <summary>
    /// Interaction logic for MaterialPage.xaml
    /// </summary>
    public partial class MaterialPage : Page
    {
        MainWindow mainWindow;
        Hasher hasher = new Hasher();

        public MaterialPage()
        {
            InitializeComponent();
        }

        public void setMainWindow(MainWindow _mainWindow)
        {
            mainWindow = _mainWindow;
        }

        private void MaterialListDataGrid_Add_Click(object sender, RoutedEventArgs e)
        {
            int count;
            decimal price;
            string message;
            string caption = "Fehlerhafte Eingabe!";
            MessageBoxButton messageBoxButton = MessageBoxButton.OK;
            MessageBoxImage messageBoxImage = MessageBoxImage.Warning;

            if (!string.IsNullOrWhiteSpace(MaterialDescriptionTextBox.Text))
            {
                if (int.TryParse(MaterialCountTextBox.Text, out count) && int.Parse(MaterialCountTextBox.Text) > 0)
                {
                    if (MaterialPriceTextBox.Text.Contains("."))
                        MaterialPriceTextBox.Text = MaterialPriceTextBox.Text.Replace('.', char.Parse(NumberFormatInfo.CurrentInfo.CurrencyDecimalSeparator));
                    else if (MaterialPriceTextBox.Text.Contains(","))
                        MaterialPriceTextBox.Text = MaterialPriceTextBox.Text.Replace(',', char.Parse(NumberFormatInfo.CurrentInfo.CurrencyDecimalSeparator));

                    if (decimal.TryParse(MaterialPriceTextBox.Text, out price))
                    {
                        if (!MaterialPriceTextBox.Text.Contains(NumberFormatInfo.CurrentInfo.CurrencyDecimalSeparator))
                            MaterialPriceTextBox.Text += NumberFormatInfo.CurrentInfo.CurrencyDecimalSeparator + "00";

                        var data = new Materialliste { Bezeichnung = mainWindow.mainViewModel.materialDescription, Anzahl = mainWindow.mainViewModel.materialCount, Preis = mainWindow.mainViewModel.materialPrice };
                        Material m = new Material();

                        MaterialListDataGrid.Items.Add(data);
                        m.Description = mainWindow.mainViewModel.materialDescription;
                        m.Count = mainWindow.mainViewModel.materialCount;
                        m.Price = mainWindow.mainViewModel.materialPrice;
                        mainWindow.mainViewModel.MaterialList.Add(m);

                        mainWindow.mainViewModel.addMaterialToList(mainWindow.mainViewModel.materialPath);
                        mainWindow.buildMaterialList(true);

                        MaterialDescriptionTextBox.Text = "";
                        MaterialCountTextBox.Text = "0";
                        MaterialPriceTextBox.Text = "0";
                    }
                    else
                    {
                        message = "Die Eingabe für das Feld '" + PriceLabel.Content + "' ist fehlerhaft!";

                        MessageBox.Show(message, caption, messageBoxButton, messageBoxImage);
                    }
                }
                else
                {
                    message = "Die Eingabe für das Feld '" + CountLabel.Content + "' ist fehlerhaft!";

                    MessageBox.Show(message, caption, messageBoxButton, messageBoxImage);
                }
            }
            else
            {
                message = "Die Eingabe für das Feld '" + DescriptionLabel.Content + "' ist fehlerhaft!";

                MessageBox.Show(message, caption, messageBoxButton, messageBoxImage);
            }
        }

        private void MaterialListDataGrid_Remove_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = MaterialListDataGrid.SelectedItem;

            if (selectedItem != null)
            {
                mainWindow.mainViewModel.MaterialList.RemoveAt(MaterialListDataGrid.Items.IndexOf(selectedItem));
                MaterialListDataGrid.Items.Remove(selectedItem);
            }
            else
            {
                MessageBox.Show("Es muss ein Item ausgewählt werden, um es zu löschen.", "Keine Auswahl getroffen!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void RemoveMaterialItemsButton_Click(object sender, RoutedEventArgs e)
        {
            if (MaterialListBox.SelectedIndex > -1)
            {
                ListBoxItem selectedListBoxItem = (ListBoxItem)MaterialListBox.SelectedItem;

                mainWindow.mainViewModel.removeMaterialFromList((string)selectedListBoxItem.Content, mainWindow.mainViewModel.materialPath);
                MaterialListBox.Items.Remove(selectedListBoxItem);
            }
        }

        private void MaterialListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (MaterialListBox.SelectedIndex != -1)
            {
                ListBoxItem selectedItem = (ListBoxItem)MaterialListBox.SelectedItem;
                string[] data = mainWindow.profileHandler.read((string)selectedItem.Content, mainWindow.mainViewModel.materialPath);

                if (data.Length > 0)
                {
                    MaterialDescriptionTextBox.Text = hasher.decrypt(data[0]);
                    MaterialCountTextBox.Text = hasher.decrypt(data[1]);
                    MaterialPriceTextBox.Text = hasher.decrypt(data[2]);
                }
            }
        }

        private void DescriptionTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            mainWindow.mainViewModel.materialDescription = MaterialDescriptionTextBox.Text;
        }

        private void CountTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            int count;

            if (!string.IsNullOrWhiteSpace(MaterialCountTextBox.Text))
                if (int.TryParse(MaterialCountTextBox.Text, out count))
                    mainWindow.mainViewModel.materialCount = int.Parse(MaterialCountTextBox.Text);
        }

        private void PriceTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            decimal price;

            if (!string.IsNullOrWhiteSpace(MaterialPriceTextBox.Text))
                if (decimal.TryParse(MaterialPriceTextBox.Text, out price))
                    mainWindow.mainViewModel.materialPrice = decimal.Parse(MaterialPriceTextBox.Text);
        }

        private void MaterialNumericTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;

            if (textBox.Text == "0")
                textBox.Text = "";

            TextBox_GotFocus(sender, e);
        }

        private void MaterialNumericTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            double numericValue;

            if (string.IsNullOrWhiteSpace(textBox.Text) || !double.TryParse(textBox.Text, out numericValue))
                textBox.Text = "0";

            Control_LostFocus(sender, e);
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
