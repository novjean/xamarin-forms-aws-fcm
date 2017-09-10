using System;
using System.Threading.Tasks;
using securesignature.Models;
using securesignature.Services;
using Xamarin.Forms;

namespace securesignature.Views
{
    public partial class FailurePage : ContentPage
    {
        //Declaration
        private string uuid;
        private string token;

        //Instance of Services
        private RegistrationIntentService _regServ = new RegistrationIntentService();
        private FCMSignatureService _fcmServ = new FCMSignatureService();

        public void init()
        {
            //Hiding Back navigation
            NavigationPage.SetHasBackButton(this, false);
            //Hide the navigation bar
            NavigationPage.SetHasNavigationBar(this, false);

            UIDefaults uid = new UIDefaults();
            MainLayout.BackgroundColor = uid.backgroundColor;

            displayMessage.Text = "Please contact our support team at \n(800) 909-3630" +
                " and provide the device id: \n\n";
            displayUUID.Text = uuid;

            labelPageTitle.TextColor = uid.textColor;
            labelPageTitle.FontSize = uid.titleFontSize;
            labelCompany.TextColor = uid.textColor;
            labelCompanyDetails.TextColor = uid.textColor;
            displayMessage.FontSize = uid.messageFontSize;
            displayMessage.TextColor = uid.textColor;
            displayUUID.TextColor = uid.textColor;
            displayUUID.FontSize = uid.messageFontSize + 1;

        }

        #region Constructors
        //Default constructor
        public FailurePage()
        {
            InitializeComponent();
            init();
        }

        public FailurePage(string uuid, string token)
        {
            this.token = token;
            this.uuid = uuid;

			InitializeComponent();
			init();
            PostDeviceIDToDBAsync(uuid);

            CheckStatusRepeaterAsync();
        }

        public async void PostDeviceIDToDBAsync(string uuid)
        {
            await _regServ.PushDeviceIdentifierAsync(uuid);
        }
        #endregion

        private async void CheckStatusRepeaterAsync()
        {
            //int i = 1;

            while (true)
            {
                DeviceInfo device = await _regServ.checkDevice(uuid);

                if (!device.key.Equals("")){
                    string deviceToken = await _regServ.BuildDeviceTokenAsync(device.key, uuid);
                    await _fcmServ.updateTokenToDBAsync(deviceToken, token);
                    break;
                }
                    
                //Check uuid with DB every 5 seconds
                await Task.Delay(5000);
            }
            await Navigation.PopAsync();
        }
    }
}
