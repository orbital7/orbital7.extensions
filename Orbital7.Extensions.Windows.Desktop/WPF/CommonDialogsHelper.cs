using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace Orbital8.Utility.WPF
{
    public static class CommonDialogsHelper
    {
        public static string DefaultOpenCaption = "Open";

        public static string DefaultSaveAsCaption = "Save As";

        public static string BuildFilter(string description, List<string> extensions)
        {
            string filter = description + "|";

            foreach (string extension in extensions)
                filter += "*" + extension + ";";

            return filter.PruneEnd(1);
        }

        public static string ShowOpenDialog(DependencyObject owner, string filter)
        {
            return ShowOpenDialog(owner, DefaultOpenCaption, filter, String.Empty);
        }

        public static string ShowOpenDialog(DependencyObject owner, string title, string filter)
        {
            return ShowOpenDialog(owner, title, filter, String.Empty);
        }

        public static string ShowOpenDialog(DependencyObject owner, string title, string filter, string initFilename)
        {
            string filePath = string.Empty;

            try
            {
                // Form the dialog.
                OpenFileDialog dialog = new OpenFileDialog();
                dialog.Title = title;
                dialog.Filter = filter;
                dialog.FileName = initFilename;

                // Determine the owning window.
                Window owningWindow = null;
                if (owner != null) Window.GetWindow(owner);

                // Show the window with an owner.
                bool? result = false;
                if (owningWindow != null)
                    result = dialog.ShowDialog(owningWindow);
                // Else no show with no owner.
                else
                    result = dialog.ShowDialog();

                // Record.
                if (result == true)
                    filePath = dialog.FileName;
            }
            catch (Exception error)
            {
                MessageBoxHelper.ShowError(owner, error);
            }

            return filePath;
        }

        public static string ShowSaveDialog(DependencyObject owner, string filter)
        {
            return ShowSaveDialog(owner, DefaultSaveAsCaption, filter);
        }

        public static string ShowSaveDialog(DependencyObject owner, string title, string filter)
        {
            return ShowSaveDialog(owner, title, filter, String.Empty);
        }

        public static string ShowSaveDialog(DependencyObject owner, string title, string filter, string initFilename)
        {
            string filePath = String.Empty;

            try
            {
                // Form the dialog.
                SaveFileDialog dialog = new SaveFileDialog();
                dialog.Title = title;
                dialog.Filter = filter;
                dialog.FileName = initFilename;

                // Determine the owning window.
                Window owningWindow = null;
                if (owner != null) Window.GetWindow(owner);

                // Show the window with an owner.
                bool? result = false;
                if (owningWindow != null)
                    result = dialog.ShowDialog(owningWindow);
                // Else no show with no owner.
                else
                    result = dialog.ShowDialog();

                // Continue if successful result.
                if (result == true)
                {
                    // Record the filename.
                    filePath = dialog.FileName;

                    // Parse filter for number of filetypes.
                    string[] filterSubstrings = filter.Parse("|*");
                    // If there is only one filetype, this string will be split into two pieces.
                    if (filterSubstrings.Length == 2)
                    {
                        // The extension will be the second part.
                        string reqExtension = filterSubstrings[1];
                        // Verify.
                        if (!filePath.EndsWith(reqExtension, StringComparison.CurrentCultureIgnoreCase))
                            filePath += reqExtension;
                    }
                }
            }
            catch (Exception error)
            {
                MessageBoxHelper.ShowError(owner, error);
            }

            return filePath;
        }
    }
}
