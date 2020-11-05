using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Orbital7.Extensions.Xamarin
{
    public interface IPopupService
    {
        Task<bool> DisplayAlertAsync(
            string title,
            string message,
            string accept = "OK",
            string cancel = "Cancel");

        Task<string> DisplayActionSheetAsync(
            string title,
            string cancel,
            string destruction,
            params string[] buttons);

        Task<string> DisplayPromptAsync(
            string title,
            string message,
            string accept = "OK",
            string cancel = "Cancel",
            string placeholder = null,
            int maxLength = -1,
            Keyboard keyboard = null,
            string initialValue = "");

        Task DisplayBusyAsync(
            string busyMessage,
            Color? backgroundColor,
            Color? foregroundColor = null);

        Task HideBusyAsync();
    }
}
