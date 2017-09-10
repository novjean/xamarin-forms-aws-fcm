using System;
using PCLStorage;
using securesignature.Models;
using securesignature.Services;
using SignaturePad.Forms;
using Xamarin.Forms;

namespace securesignature.Views
{
    public partial class SigningPage : ContentPage
    {
        //Declaration
		private SignatureRequest req;

		//Instance of Services
		private AWSServices _awsServ = new AWSServices();

        //Initialization of values
        public void init(){
			//Hide the navigation bar
			NavigationPage.SetHasNavigationBar(this, false); 

            //Default UI values class
			UIDefaults uid = new UIDefaults();
			
            MainLayout.BackgroundColor = uid.backgroundColor;

			labelPageTitle.TextColor = uid.textColor;
            labelPageTitle.FontSize = uid.titleFontSize;
			labelCompany.TextColor = uid.textColor;
			labelCompanyDetails.TextColor = uid.textColor;
            activitySpinner.BackgroundColor = uid.backgroundColor;

            string[] messages = req.Messages.ToArray();
            if (messages.Length > 0){
                labelMessage1.Text = messages[0];
                labelMessage2.Text = messages[1];
                labelMessage3.Text = messages[2];

                labelMessage1.FontSize = labelMessage2.FontSize = 
                    labelMessage3.FontSize = uid.messageFontSize;
                labelMessage1.TextColor = labelMessage2.TextColor =
                    labelMessage3.TextColor = uid.textColor;
			}
        }

        #region Constructors

        //Default constructor for rendering
        public SigningPage()
        {
            InitializeComponent();
            init();
        }

        //Main constructor
        public SigningPage(SignatureRequest req)
        {
			this.req = req;
			InitializeComponent();
			init();
        }
        #endregion

        #region Buttons and Taps
        //Method for when the signature is completed
        private async void OnSendClicked(object sender, EventArgs e)
        {
            activitySpinner.IsVisible = true;

            //Accessing file system and creating file
            var folder = FileSystem.Current.LocalStorage;

            var file = await folder.CreateFileAsync($"{req.RequestId}",
                                                    CreationCollisionOption.ReplaceExisting);
            //Creating the image of signature
            var settings = new ImageConstructionSettings
            {
                BackgroundColor = Color.White,
                StrokeColor = Color.Black,
            };


            try
            {
                using (var stream = await signaturePad.GetImageStreamAsync(SignatureImageFormat.Png,
                                                                           settings))
                using (var fileStream = await file.OpenAsync(FileAccess.ReadAndWrite))
                {
                    //Trying to stop the file from being created as blank (Needs work)
                    if (!stream.Equals(null))
                        await stream.CopyToAsync(fileStream);
                }

                
                activitySpinner.IsVisible = false;
                //Push to S3
                if(_awsServ.SaveFileToS3(file)){
					//Display Thank you for signing. (Optional)
					await DisplayAlert("Thank you",
								   "Thanks for signing, " +
								   "Your signature has been received.",
								   "OK");
					//Clear the signature from pad after the file is saved.
					signaturePad.Clear();
                    //Popping back to the mainpage
                    await Navigation.PopToRootAsync();             }
                else{
                    await DisplayAlert("Upload Failed",
                                       "Your signature has not been received. Send Again.",
                                       "OK");
                }

            }
            catch (NullReferenceException)
            {
                activitySpinner.IsVisible = false;
                //Log Error here
                System.Diagnostics.Debug.WriteLine("Null Reference Exception caught");
                await DisplayAlert("Error", "Please sign.", "OK");
            }
            catch (Exception)
            {
                activitySpinner.IsVisible = false;
                //Log error
                await DisplayAlert("Oops!", 
                                   "Something went wrong. Please make sure to sign.", 
                                   "OK");
            }
            activitySpinner.IsVisible = false;
        }

        void OnCancelClicked(object sender, System.EventArgs e)
        {
            Navigation.PopAsync();
        }
        #endregion
    }
}
