using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Orbital7.Extensions.Xamarin
{
    public class PopupService
        : IPopupService
    {
        public async Task<bool> DisplayAlertAsync(
            string title,
            string message,
            string accept,
            string cancel)
        {
            var page = GetCurrentPage();
            if (page != null)
                return await page.DisplayAlert(
                    title,
                    message,
                    accept,
                    cancel);

            return false;
        }

        public async Task<string> DisplayActionSheetAsync(
            string title,
            string cancel,
            string destruction,
            params string[] buttons)
        {
            return await GetCurrentPage().DisplayActionSheet(
                title,
                cancel,
                destruction,
                buttons);
        }

        public async Task<string> DisplayPromptAsync(
            string title,
            string message,
            string accept = "OK",
            string cancel = "Cancel",
            string placeholder = null,
            int maxLength = -1,
            Keyboard keyboard = null,
            string initialValue = "")
        {
            return await GetCurrentPage().DisplayPromptAsync(
                title,
                message,
                accept,
                cancel,
                placeholder,
                maxLength,
                keyboard,
                initialValue);
        }

        public async Task DisplayBusyAsync(
            string busyMessage,
            Color? backgroundColor,
            Color? foregroundColor = null)
        {
            await HideBusyAsync();

            BusyPopupManager.ShowingBusyPopup = true;
            await PopupNavigation.Instance.PushAsync(
                new BusyPopupPage(busyMessage, backgroundColor, foregroundColor),
                animate: true);
        }

        public async Task HideBusyAsync()
        {
            if (BusyPopupManager.ShowingBusyPopup)
            {
                BusyPopupManager.ShowingBusyPopup = false;
                await PopupNavigation.Instance.PopAsync(animate: true);
            }
        }

        public static Page GetCurrentPage()
        {
            Page page = null;

            if (Application.Current.MainPage.Navigation.ModalStack.Count > 0)
                page = Application.Current.MainPage.Navigation.ModalStack.Last();
            
            if (page == null && Application.Current.MainPage.Navigation.NavigationStack.Count > 0)
                page = Application.Current.MainPage.Navigation.NavigationStack.Last();
            
            if (page == null)
                page = Application.Current.MainPage;

            return page;
        }
    }
}
