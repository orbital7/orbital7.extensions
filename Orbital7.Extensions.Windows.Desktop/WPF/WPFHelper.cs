using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Orbital8.Utility.WPF
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

        public static void FillComboBox(ComboBox combobox, IList list, object selectedItem)
        {
            combobox.Items.Clear();

            foreach (object item in list)
                combobox.Items.Add(item);

            combobox.SelectedItem = selectedItem;
        }

        public static void FillListBox(ListBox listbox, IList list)
        {
            FillListBox(listbox, list, true);
        }

        public static void FillListBox(ListBox listbox, IList list, bool selectFirst)
        {
            listbox.Items.Clear();

            foreach (object item in list)
                listbox.Items.Add(item);

            if (selectFirst)
                SelectFirstListBoxItem(listbox);
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
