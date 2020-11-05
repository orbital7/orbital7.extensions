using Rg.Plugins.Popup.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Orbital7.Extensions.Xamarin
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BusyPopupPage : PopupPage
    {
        public BusyPopupPage(
            string message,
            Color? backgroundColor = null,
            Color? foregroundColor = null)
        {
            InitializeComponent();
            busyMessage.Text = message;
            busyMessage.TextColor = foregroundColor ?? Color.White;
            busyIndicator.Color = foregroundColor ?? Color.White;
            stackLayout.BackgroundColor = backgroundColor ?? Color.Black;
        }

        protected override void OnDisappearing()
        {

        }

        protected override bool OnBackButtonPressed()
        {
            return true;
        }

        protected override bool OnBackgroundClicked()
        {
            return false;
        }
    }
}