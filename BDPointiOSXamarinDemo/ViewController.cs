using System;
using Foundation;
using PointSDK.iOS;
using UIKit;

namespace BDPointiOSXamarinDemo
{
    public partial class ViewController : UIViewController, IBDPSessionDelegate, IBDPLocationDelegate
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

			locationManager.SessionDelegate = this;
			locationManager.LocationDelegate = this;

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
             * apiKey       The API key generated for your app in the Bluedot Point Access
             * packageName  The package name of your app created in the Bluedot Point Access
             * userName     The user name you used to login to the Bluedot Point Access
             */
			if (locationManager.AuthenticationState != BDAuthenticationState.Authenticated)
			{
                locationManager.AuthenticateWithApiKey("dee11930-ebff-11e5-8e27-bc305bf60831");
			}
			else
			{
				updateLog("Already Authenticated");
			}
		}

		private void updateLog(String s)
		{
			String display = StatusLog.Text + "\n" + s;
			StatusLog.Text = display;
		}

		public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }

		[Export("didCheckIntoFence:inZone:atCoordinate:onDate:willCheckOut:withCustomData:")]
		public void DidCheckIntoFence(BDFenceInfo fence, BDZoneInfo zoneInfo, BDLocationCoordinate2D coordinate, NSDate date, bool willCheckOut, NSDictionary customData)
		{
			updateLog("Checked into fence");
		}

		[Export("didCheckOutFromFence:inZone:onDate:withDuration:withCustomData:")]
		public void DidCheckOutFromFence(BDFenceInfo fence, BDZoneInfo zoneInfo, NSDate date, nuint checkedInDuration, NSDictionary customData)
		{
			updateLog("Checked out from fence");
		}

		[Export("didCheckIntoBeacon:inZone:withProximity:onDate:willCheckOut:withCustomData:")]
		public void DidCheckIntoBeacon(BDBeaconInfo beacon, BDZoneInfo zoneInfo, CoreLocation.CLProximity proximity, NSDate date, bool willCheckOut, NSDictionary customData)
		{
			updateLog("Checked into beacon");
		}

		[Export("didCheckOutFromBeacon:inZone:withProximity:onDate:withDuration:withCustomData:")]
		public void DidCheckOutFromBeacon(BDBeaconInfo beacon, BDZoneInfo zoneInfo, CoreLocation.CLProximity proximity, NSDate date, nuint checkedInDuration, NSDictionary customData)
		{
			updateLog("Checked out from beacon");
		}

        public void WillAuthenticateWithApiKey(string apiKey)
		{
			updateLog("Authenticating..");
		}

		public void AuthenticationWasSuccessful()
		{
			updateLog("Authentication was successful");
            Authenticate.SetTitle("Logout", UIControlState.Normal);
		}

		public void AuthenticationWasDeniedWithReason(string reason)
		{
			updateLog("Authentication denied");
            Authenticate.SetTitle("Authenticate", UIControlState.Normal);
		}

		public void AuthenticationFailedWithError(NSError error)
		{
			updateLog("Authentication failed");
			Authenticate.SetTitle("Authenticate", UIControlState.Normal);
		}

		public void DidEndSession()
		{
			updateLog("Session ended");
			Authenticate.SetTitle("Authenticate", UIControlState.Normal);
		}

		public void DidEndSessionWithError(NSError error)
		{
			updateLog("Session ended with error");
			Authenticate.SetTitle("Authenticate", UIControlState.Normal);
		}

		[Export("didUpdateZoneInfo:")]
		public void DidUpdateZoneInfo(NSSet zoneInfos)
		{
			updateLog("Zone Info updated");
		}
    }
}
