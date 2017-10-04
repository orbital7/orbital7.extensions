using System;
using System.Collections;
using System.Windows;
using System.Windows.Controls;

namespace Orbital7.Extensions.WPF
{
    public partial class InputComboBoxDialog : Window
    {
        public object Value
        {
            get { return comboValue.SelectedValue; }
        }

        public InputComboBoxDialog(string title, string description, IList items,
            object selectedItem = null, bool selectFirstIfSelectedItemIsNull = false)
        {
            InitializeComponent();

            // Setup.
            this.Title = title;
            textDescription.Text = description;
            WPFHelper.FillComboBox(comboValue, items, selectedItem, selectFirstIfSelectedItemIsNull);
            comboValue.Focus();
        }

        private void buttonOK_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }

        private void buttonCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void comboValue_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            buttonOK.IsEnabled = comboValue.SelectedItem != null;
        }

        //private void Window_Loaded(object sender, RoutedEventArgs e)
        //{

        //    FocusManager.SetFocusedElement(this, comboValue);
        //}
    }
}
