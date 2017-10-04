using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Orbital7.Extensions.WinForms
{
    public static class MessageBoxHelper
    {
        public static string DefaultCaption = "Notification";
        public static string DefaultErrorCaption = "Error";
        public static string DefaultQuestionCaption = "Question";

        public static void ShowError(string message, string caption)
        {
            ShowMessageBox(message, caption, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public static void ShowError(string message)
        {
            ShowError(message, DefaultErrorCaption);
        }

        public static void ShowError(Exception exception)
        {
            ShowError(exception.Message);
        }

        public static bool AskQuestion(string message, bool important, string caption)
        {
            var icon = MessageBoxIcon.Question;
            if (important) icon = MessageBoxIcon.Exclamation;

            var btns = MessageBoxButtons.YesNo;

            return (ShowMessageBox(message, caption, btns, icon) == DialogResult.Yes);
        }

        public static bool AskQuestion(string message, string caption)
        {
            return AskQuestion(message, false, caption);
        }

        public static bool AskQuestion(string message)
        {
            return AskQuestion(message, false, DefaultQuestionCaption);
        }

        public static bool AskQuestion(string message, bool important)
        {
            return AskQuestion(message, important, DefaultQuestionCaption);
        }

        public static void ShowMessage(string message, bool important, string caption)
        {
            var icon = MessageBoxIcon.Information;
            if (important) icon = MessageBoxIcon.Exclamation;

            ShowMessageBox(message, caption, MessageBoxButtons.OK, icon);
        }

        public static void ShowMessage(string message, bool important)
        {
            ShowMessage(message, important, DefaultCaption);
        }

        public static void ShowMessage(string message, string caption)
        {
            ShowMessage(message, false, caption);
        }

        public static void ShowMessage(string message)
        {
            ShowMessage(message, DefaultCaption);
        }

        private static DialogResult ShowMessageBox(string message, string title,
                MessageBoxButtons buttons, MessageBoxIcon icon)
        {
            var result = DialogResult.OK;

            var mainWindow = WindowHelper.GetNativeWindow();

            if (mainWindow != null)
            {
                result = MessageBox.Show(mainWindow, message, title, buttons, icon);
                mainWindow.ReleaseHandle();
            }
            else
            {
                result = MessageBox.Show(message, title, buttons, icon);
            }

            return result;
        }
    }
}
