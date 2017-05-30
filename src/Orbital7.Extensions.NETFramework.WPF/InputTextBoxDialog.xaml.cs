using System;
using System.Windows;
using System.Windows.Controls;

namespace Orbital7.Extensions.NETFramework.WPF
{
    public partial class InputTextBoxDialog : Window
    {
        public string Value
        {
            get { return inputValue.Text.Trim(); }
        }

        public InputTextBoxDialog(string title, string description, string initalValue)
        {
            InitializeComponent();

            // Setup.
            this.Title = title;
            textDescription.Text = description;
            inputValue.Text = initalValue;

            // Select all the text.
            inputValue.SelectAll();
            inputValue.Focus();
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

        private void inputValue_TextChanged(object sender, TextChangedEventArgs e)
        {
            buttonOK.IsEnabled = !String.IsNullOrEmpty(this.Value);
        }

        //private void Window_Loaded(object sender, RoutedEventArgs e)
        //{
            
        //    FocusManager.SetFocusedElement(this, inputValue);
        //}
    }
}
