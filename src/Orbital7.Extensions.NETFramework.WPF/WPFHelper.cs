using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Orbital7.Extensions.NETFramework.WPF
{
    public static class WPFHelper
    {
        public static void SetVisible(UIElement element, bool isVisible)
        {
            if (isVisible)
                element.Visibility = Visibility.Visible;
            else
                element.Visibility = Visibility.Collapsed;
        }

        public static void FillComboBox(ComboBox combobox, IList items, 
            object selectedItem = null, bool selectFirstIfSelectedItemIsNull = false)
        {
            combobox.Items.Clear();

            foreach (object item in items)
                combobox.Items.Add(item);

            if (selectedItem == null && selectFirstIfSelectedItemIsNull && combobox.Items.Count > 0)
                combobox.SelectedItem = combobox.Items[0];
            else if (selectedItem != null)
                combobox.SelectedItem = selectedItem;
        }

        public static void FillListBox(ListBox listbox, IList items,
            object selectedItem = null, bool selectFirstIfSelectedItemIsNull = false)
        {
            listbox.Items.Clear();

            foreach (object item in items)
                listbox.Items.Add(item);

            if (selectedItem == null && selectFirstIfSelectedItemIsNull && listbox.Items.Count > 0)
                listbox.SelectedItem = listbox.Items[0];
            else if (selectedItem != null)
                listbox.SelectedItem = selectedItem;
        }

        public static void SelectFirstListBoxItem(ListBox listbox)
        {
            if (listbox.Items.Count > 0)
                listbox.SelectedIndex = 0;
        }

        public static ScrollViewer FindScroll(Control control)
        {
            var borderDecorator = VisualTreeHelper.GetChild(control, 0) as Decorator;

            if (borderDecorator != null)
                return borderDecorator.Child as ScrollViewer;
            else
                return null;
        }
    }
}
