using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Orbital7.Extensions.WinForms
{
    public static class CommonDialogsHelper
    {
        public static string DefaultOpenCaption = "Open";
        public static string DefaultSaveAsCaption = "Save";

        public static string ShowFolderBrowseDialog(string description)
        {
            return ShowFolderBrowseDialog(description, String.Empty);
        }

        public static string ShowFolderBrowseDialog(string description, string selectedPath)
        {
            return ShowFolderBrowseDialog(description, selectedPath, true);
        }

        public static string ShowFolderBrowseDialog(string description, string selectedPath, bool showNewFolderButton)
        {
            string outputPath = String.Empty;

            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Description = description;
            dialog.SelectedPath = selectedPath;
            dialog.ShowNewFolderButton = showNewFolderButton;

            // Show it.
            System.Windows.Forms.Application.DoEvents();

            // Try to use the main window.
            DialogResult result = DialogResult.Cancel;
            NativeWindow mainWindow = WindowHelper.GetNativeWindow();
            if (mainWindow != null)
            {
                result = dialog.ShowDialog(mainWindow);
                mainWindow.ReleaseHandle();
            }
            else
            {
                result = dialog.ShowDialog();
            }

            // Continue if successful result.
            if (result == DialogResult.OK)
            {
                // Record the filename.
                outputPath = dialog.SelectedPath;
                System.Windows.Forms.Application.DoEvents();
            }

            return outputPath;
        }

        public static string ShowOpenDialog(string filter)
        {
            return ShowOpenDialog(DefaultOpenCaption, filter, String.Empty);
        }

        public static string ShowOpenDialog(string title, string filter)
        {
            return ShowOpenDialog(title, filter, String.Empty);
        }

        public static string ShowOpenDialog(string title, string filter, string initFilename)
        {
            string filePath = string.Empty;

            try
            {
                // Form the dialog.
                OpenFileDialog dialog = new OpenFileDialog();
                dialog.Title = title;
                dialog.Filter = filter;
                dialog.FileName = initFilename;

                // Show it.
                System.Windows.Forms.Application.DoEvents();

                // Try to get the native window.
                DialogResult result = DialogResult.Cancel;
                NativeWindow mainWindow = WindowHelper.GetNativeWindow();
                if (mainWindow != null)
                {
                    result = dialog.ShowDialog(mainWindow);
                    mainWindow.ReleaseHandle();
                }
                else
                {
                    result = dialog.ShowDialog();
                }

                // Record.
                if (result == DialogResult.OK)
                {
                    System.Windows.Forms.Application.DoEvents();
                    filePath = dialog.FileName;
                }
            }
            catch (Exception error)
            {
                MessageBoxHelper.ShowError(error);
            }

            return filePath;
        }

        public static string ShowSaveDialog(string filter)
        {
            return ShowSaveDialog(DefaultSaveAsCaption, filter);
        }

        public static string ShowSaveDialog(string title, string filter)
        {
            return ShowSaveDialog(title, filter, String.Empty);
        }

        public static string ShowSaveDialog(string title, string filter, string initFilename)
        {
            string filePath = String.Empty;

            try
            {
                // Form the dialog.
                SaveFileDialog dialog = new SaveFileDialog();
                dialog.Title = title;
                dialog.Filter = filter;
                dialog.FileName = initFilename;

                // Show it.
                System.Windows.Forms.Application.DoEvents();

                // Try to use the main window.
                DialogResult result = DialogResult.Cancel;
                NativeWindow mainWindow = WindowHelper.GetNativeWindow();
                if (mainWindow != null)
                {
                    result = dialog.ShowDialog(mainWindow);
                    mainWindow.ReleaseHandle();
                }
                else
                {
                    result = dialog.ShowDialog();
                }

                // Continue if successful result.
                if (result == DialogResult.OK)
                {
                    // Record the filename.
                    filePath = dialog.FileName;
                    System.Windows.Forms.Application.DoEvents();

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
                MessageBoxHelper.ShowError(error);
            }

            return filePath;
        }
    }
}
