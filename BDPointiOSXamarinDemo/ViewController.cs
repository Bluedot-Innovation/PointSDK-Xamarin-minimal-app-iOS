using System;
using PointSDK.iOS;
using UIKit;

namespace BDPointiOSXamarinDemo
{
    public partial class ViewController : UIViewController
    {
        BDLocationManager locationManager;

        protected ViewController(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            // Perform any additional setup after loading the view, typically from a nib.
            updateLog("ViewDidload");
            locationManager = BDLocationManager.Instance;

            Authenticate.TouchUpInside += (o, s) => {
                if (locationManager.AuthenticationState == BDAuthenticationState.Authenticated)
                {
                    Signout();
                }
                else
                {
                    StartAuthentication();
                }
			};

        }

		public void Signout()
		{
			if (locationManager.AuthenticationState == BDAuthenticationState.Authenticated)
            {
				locationManager.LogOut();
			}
			else
            {
				updateLog("Already Logged out");
			}
		}

		private void StartAuthentication()
		{
			/* Start the Bluedot Point Service by providing with the credentials and a ServiceStatusListener, 
             * the app will be notified via the status listener if the Bluedot Point Service started successful.
             * 
             * Parameters
             * apiKey               The API key generated for your app in the Bluedot Point Access
             * authorisationLevel   The location services authorisation level
             */
			if (locationManager.AuthenticationState != BDAuthenticationState.Authenticated)
			{
                locationManager.AuthenticateWithApiKey("__your api key__", BDAuthorizationLevel.Always);
			}
			else
			{
				updateLog("Already Authenticated");
			}
        }

		private void updateLog(string s)
		{
            string display = StatusLog.Text + "\n" + s;
			StatusLog.Text = display;
		}

		public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }
    }
}
