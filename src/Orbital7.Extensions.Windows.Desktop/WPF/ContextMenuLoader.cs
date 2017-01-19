using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace Orbital7.Extensions.Windows.Desktop.WPF
{
    public static class ContextMenuLoader
    {
        public static void AddContextMenuItems(IList items, ContextMenu contextMenu, RoutedEventHandler handler, bool addSeparator)
        {
            if (items.Count > 0)
            {
                // Add a separator if items already exist.
                if (addSeparator && (contextMenu.Items.Count > 0))
                    contextMenu.Items.Add(new Separator());

                // Loop through the items and add a button for each.
                foreach (var item in items)
                {
                    MenuItem menuItem = new MenuItem();
                    menuItem.Header = item.ToString();
                    menuItem.Tag = item;
                    menuItem.Click += handler;
                    contextMenu.Items.Add(menuItem);
                }
            }
        }
    }
}
