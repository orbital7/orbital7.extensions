using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace Orbital7.Extensions.Windows.Desktop.WPF
{
    public static class MessageBoxHelper
    {

        public static string DefaultCaption = "Notification";
        public static string DefaultErrorCaption = "Error";
        public static string DefaultQuestionCaption = "Question";

        public static void ShowError(DependencyObject owner, string message, string caption)
        {
            ShowMessageBox(owner, message, caption, MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public static void ShowError(DependencyObject owner, string message)
        {
            ShowError(owner, message, DefaultErrorCaption);
        }

        public static void ShowError(DependencyObject owner, Exception exception)
        {
            ShowError(owner, exception.Message);
        }

        public static bool AskQuestion(DependencyObject owner, string message, bool important, string caption)
        {
            // Determine icon.
            MessageBoxImage icon = MessageBoxImage.Question;
            if (important) icon = MessageBoxImage.Exclamation;

            return ShowMessageBox(owner, message, caption, MessageBoxButton.YesNo, icon) == MessageBoxResult.Yes;
        }

        public static bool AskQuestion(DependencyObject owner, string message, string caption)
        {
            return AskQuestion(owner, message, false, caption);
        }

        public static bool AskQuestion(DependencyObject owner, string message)
        {
            return AskQuestion(owner, message, false, DefaultQuestionCaption);
        }

        public static bool AskQuestion(DependencyObject owner, string message, bool important)
        {
            return AskQuestion(owner, message, important, DefaultQuestionCaption);
        }

        public static void ShowMessage(DependencyObject owner, string message, bool important, string caption)
        {
            // Determine icon.
            MessageBoxImage icon = MessageBoxImage.Information;
            if (important) icon = MessageBoxImage.Exclamation;

            ShowMessageBox(owner, message, caption, MessageBoxButton.OK, icon);
        }

        public static void ShowMessage(DependencyObject owner, string message, bool important)
        {
            ShowMessage(owner, message, important, DefaultCaption);
        }

        public static void ShowMessage(DependencyObject owner, string message, string caption)
        {
            ShowMessage(owner, message, false, caption);
        }

        public static void ShowMessage(DependencyObject owner, string message)
        {
            ShowMessage(owner, message, DefaultCaption);
        }

        private static MessageBoxResult ShowMessageBox(DependencyObject owner, string message, string title,
                MessageBoxButton buttons, MessageBoxImage icon)
        {
            // Determine the owning window.
            Window owningWindow = null;
            if (owner != null) owningWindow = Window.GetWindow(owner);

            // Show the window with an owner.
            if (owningWindow != null)
                return MessageBox.Show(owningWindow, message, title, buttons, icon);
            // No owner.
            else
                return MessageBox.Show(message, title, buttons, icon);
        }
    }
}
