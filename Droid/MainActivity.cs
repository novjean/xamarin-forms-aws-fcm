//using System;

using Android.App;
//using Android.Content;
using Android.Content.PM;
//using Android.Runtime;
//using Android.Views;
//using Android.Widget;
using Android.OS;
using Plugin.FirebasePushNotification;
using Firebase;
using Amazon;

namespace securesignature.Droid
{
    [Activity(Label = "securesignature.Droid", Icon = "@drawable/icon", Theme = "@style/MyTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);


            LoadApplication(new App());

            //Logging AWS outputs and displaying to sys console
			//var loggingConfig = AWSConfigs.LoggingConfig;
			//loggingConfig.LogMetrics = true;
			//loggingConfig.LogResponses = ResponseLoggingOption.Always;
			//loggingConfig.LogMetricsFormat = LogMetricsFormatOption.JSON;
			//loggingConfig.LogTo = LoggingOptions.SystemDiagnostics;

            //Configuring the default region for all service clients (Not needed?)
            AWSConfigs.AWSRegion = "us-west-2";

			//Initializing the firebase app
            //Clean the previous solution if IllegalState Exception aka FirebaseApp Initialization pops up
            //Occurs when new Nugets are added especially related to PCL
			FirebaseApp app = FirebaseApp.InitializeApp(Android.App.Application.Context);

			FirebasePushNotificationManager.ProcessIntent(Intent);
        }
    }
}
