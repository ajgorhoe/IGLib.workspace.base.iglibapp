using IG.Lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// using static System.Net.Mime.MediaTypeNames;
using Microsoft.Maui.Controls;
//using Application = Microsoft.Maui.Controls.Application;

namespace IG.App
{

    /// <summary>Implementation of clipboard service that uses MAUI's default clipboard. Can be injected
    /// into ViewModel classes that should not be aware of UI (views).</summary>
    internal class AlertServiceDefault : IAlertService
    {

        /// <inheritdoc/>
        public Task ShowAlertAsync(string title, string message, string cancel = "OK")
        {
            return Application.Current.MainPage.DisplayAlert(title, message, cancel);
        }

        /// <inheritdoc/>
        public Task<bool> ShowConfirmationAsync(string title, string message, string accept = "Yes", string cancel = "No")
        {
            return Application.Current.MainPage.DisplayAlert(title, message, accept, cancel);
        }


        /// <inheritdoc/>
        public void ShowAlert(string title, string message, string cancel = "OK")
        {
            Application.Current.MainPage.Dispatcher.Dispatch(async () =>
                await ShowAlertAsync(title, message, cancel)
            );
        }


        /// <inheritdoc/>
        public void ShowConfirmation(string title, string message, Action<bool> callback,
                                     string accept = "Yes", string cancel = "No")
        {
            Application.Current.MainPage.Dispatcher.Dispatch(async () =>
            {
                bool answer = await ShowConfirmationAsync(title, message, accept, cancel);
                callback(answer);
            });
        }




    }
}
