using Acr.UserDialogs;
using MvvmCross.Forms.Platforms.Uap.Views;
using System;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;

namespace Chat.UWP
{
    public abstract class UwpApp : MvxWindowsApplication<Setup, Chat.Clinet.Core.App, Chat.App, MainPage>
    {
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
            IActivatedEventArgs activatedArgs = AppInstance.GetActivatedEventArgs();

            // If the Windows shell indicates a recommended instance, then
            // the app can choose to redirect this activation to that instance instead.
            if (AppInstance.RecommendedInstance != null)
            {
                AppInstance.RecommendedInstance.RedirectActivationTo();
            }
            else
            {
                // Define a key for this instance, based on some app-specific logic.
                // If the key is always unique, then the app will never redirect.
                // If the key is always non-unique, then the app will always redirect
                // to the first instance. In practice, the app should produce a key
                // that is sometimes unique and sometimes not, depending on its own needs.
                string key = Guid.NewGuid().ToString(); // always unique.
                                                        //string key = "Some-App-Defined-Key"; // never unique.
                var instance = AppInstance.FindOrRegisterInstanceForKey(key);
                if (instance.IsCurrentInstance)
                {
                    global::Xamarin.Forms.Forms.Init(e);
                    UserDialogs.Init();
                }
                else
                {
                    // Some other instance has registered for this key, so we'll 
                    // redirect this activation to that instance instead.
                    instance.RedirectActivationTo();
                }
            }
            base.OnLaunched(e);
        }
    }
}
