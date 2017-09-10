using System;
using System.Net;
using System.Collections.Generic;
using System.Threading.Tasks;
using Plugin.DeviceInfo;
using Plugin.FirebasePushNotification;
using securesignature.Models;
using securesignature.Services;
using Xamarin.Forms;

namespace securesignature.Views
{
    public partial class MainPage : ContentPage
    {
        //Objects
        DeviceInfo device = new DeviceInfo();
        //GenericRequest genDevice = new GenericRequest();

        //Instance of the registration service
        private RegistrationIntentService _regServ = new RegistrationIntentService();
        private FCMSignatureService _fcmServ = new FCMSignatureService();

        //initialization values
		public void init()
		{
            NavigationPage.SetHasNavigationBar(this, false); //Hide the navigation bar
			UIDefaults uid = new UIDefaults();
			MainLayout.BackgroundColor = uid.backgroundColor;

            labelPageTitle.TextColor = uid.textColor;
            labelPageTitle.FontSize = uid.titleFontSize;
            labelCompany.TextColor = uid.textColor;
            labelCompanyDetails.TextColor = uid.textColor;
		}

        #region Constructors

        public MainPage()
        {
			InitializeComponent();
            init();

            //FCM Integration
            CrossFirebasePushNotification.Current.OnTokenRefresh += (s, p) =>
            {

				Device.BeginInvokeOnMainThread(async () =>
                {
					string encodedToken = WebUtility.UrlEncode(p.Token);
					System.Diagnostics.Debug.WriteLine($"TOKEN : {encodedToken}");

                    if (await VerifyDevice())
                    {
                        string deviceToken = await _regServ.BuildDeviceTokenAsync(device.key, getUUID());
                        await _fcmServ.updateTokenToDBAsync(deviceToken, encodedToken);
                    }
                    else
                    {
                        await Navigation.PushAsync(new FailurePage(getUUID(), encodedToken));
                    }
                });

            };

            CrossFirebasePushNotification.Current.OnNotificationReceived += (s, p) =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    notificationHandler(p.Data);
                });
            };

            CrossFirebasePushNotification.Current.OnNotificationOpened += (s, p) =>
            {
				Device.BeginInvokeOnMainThread(() =>
				{
					notificationHandler(p.Data);
				});

            };
        }

        //Handles all notifications sent from Encore
        private void notificationHandler(IDictionary<string, string> Data)
        {
            SignatureRequest req = new SignatureRequest();
            foreach (var data in Data)
                req = _fcmServ.parseData(data.Value);

            if (req.Action.Equals("CAPTURE")) 
                Navigation.PushAsync(new SigningPage(req));
            else
                Navigation.PopToRootAsync();
        }
        #endregion


        //Retreive the Device ID, MD5, and call RegistrationIntentService
        public async Task<bool> VerifyDevice()
		{
            //Call service to check if device is already registered
            this.device = await _regServ.checkDevice(getUUID());

            return (device.key.Equals("") ? false : true);
		}

		public string getUUID()
		{
			return EasyEncryption.MD5.ComputeMD5Hash(CrossDeviceInfo.Current.Id).ToString();
			//return "aa9ea3b5f371b0a4adfb5c68f24b95fa"; //Keeping for development, Should remove in the Beta.

		}
    }
}
