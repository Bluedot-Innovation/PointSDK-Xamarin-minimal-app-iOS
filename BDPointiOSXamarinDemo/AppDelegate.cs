using Foundation;
using UIKit;
using System;
using PointSDK.iOS;
using CoreLocation;

namespace BDPointiOSXamarinDemo
{
	// The UIApplicationDelegate for the application. This class is responsible for launching the
	// User Interface of the application, as well as listening (and optionally responding) to application events from iOS.
	[Register("AppDelegate")]
	public class AppDelegate : UIApplicationDelegate
    {
        // class-level declarations
		
        public override UIWindow Window
		{
			get;
			set;
		}

        #region UIApplicationDelegate
        public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
		{

            // Override point for customization after application launch.
            // If not required for your application you can safely delete this method
            BDLocationManager.Instance.SessionDelegate = new SessionDelegate(this);
            BDLocationManager.Instance.LocationDelegate = new LocationDelegate(this);

            return true;
		}

		public override void OnResignActivation(UIApplication application)
		{
			// Invoked when the application is about to move from active to inactive state.
			// This can occur for certain types of temporary interruptions (such as an incoming phone call or SMS message) 
			// or when the user quits the application and it begins the transition to the background state.
			// Games should use this method to pause the game.
		}

		public override void DidEnterBackground(UIApplication application)
		{
			// Use this method to release shared resources, save user data, invalidate timers and store the application state.
			// If your application supports background exection this method is called instead of WillTerminate when the user quits.

		}

		public override void WillEnterForeground(UIApplication application)
		{
			// Called as part of the transiton from background to active state.
			// Here you can undo many of the changes made on entering the background.
		}

		public override void OnActivated(UIApplication application)
		{
			// Restart any tasks that were paused (or not yet started) while the application was inactive. 
			// If the application was previously in the background, optionally refresh the user interface.
		}

		public override void WillTerminate(UIApplication application)
		{
			// Called when the application is about to terminate. Save data, if needed. See also DidEnterBackground.
		}
        #endregion

        public void updateLog(String s)
        {
            var viewController = Window.RootViewController as ViewController;
            if (viewController == null)
            {
                return;
            }

            viewController.StatusLog.Text += "\n" + s;
        }

        public void updateAuthenticationStatus(String s)
        {
            var viewController = Window.RootViewController as ViewController;
            if (viewController == null)
            {
                return;
            }

            viewController.Authenticate.SetTitle(s, UIControlState.Normal);
        }
    }

    class SessionDelegate : IBDPSessionDelegate
    {
        private readonly AppDelegate _appDelegate;

        public SessionDelegate(AppDelegate app)
        {
            _appDelegate = app;
        }

        public override void AuthenticationFailedWithError(NSError error)
        {
            _appDelegate.updateLog("Authentication failed");
            _appDelegate.updateAuthenticationStatus("Authenticate");
        }

        public override void AuthenticationWasDeniedWithReason(string reason)
        {
            _appDelegate.updateLog("Authentication denied");
            _appDelegate.updateAuthenticationStatus("Authenticate");
        }

        public override void AuthenticationWasSuccessful()
        {
            _appDelegate.updateLog("Authentication was successful");
            _appDelegate.updateAuthenticationStatus("Logout");
        }

        public override void DidEndSession()
        {
            _appDelegate.updateLog("Session ended");
            _appDelegate.updateAuthenticationStatus("Authenticate");
        }

        public override void DidEndSessionWithError(NSError error)
        {
            _appDelegate.updateLog("Session ended with error");
            _appDelegate.updateAuthenticationStatus("Authenticate");
        }

        public override void WillAuthenticateWithApiKey(string apiKey)
        {
            _appDelegate.updateLog("Authenticating..");
        }
    }

    class LocationDelegate : IBDPLocationDelegate
    {
        private readonly AppDelegate _appDelegate;

        public LocationDelegate(AppDelegate app)
        {
            _appDelegate = app;
        }

        public override void DidCheckIntoBeacon(BDBeaconInfo beacon, BDZoneInfo zoneInfo, BDLocationInfo locationInfo, CLProximity proximity, bool willCheckOut, NSDictionary customData)
        {
            _appDelegate.updateLog("Checked into beacon");
        }

        public override void DidCheckIntoFence(BDFenceInfo fence, BDZoneInfo zoneInfo, BDLocationInfo location, bool willCheckOut, NSDictionary customData)
        {
            _appDelegate.updateLog("Checked into fence");
        }

        public override void DidCheckOutFromBeacon(BDBeaconInfo beacon, BDZoneInfo zoneInfo, CLProximity proximity, NSDate date, nuint checkedInDuration, NSDictionary customData)
        {
            _appDelegate.updateLog("Checked out from beacon");
        }

        public override void DidCheckOutFromFence(BDFenceInfo fence, BDZoneInfo zoneInfo, NSDate date, nuint checkedInDuration, NSDictionary customData)
        {
            _appDelegate.updateLog("Checked out from fence");
        }

        public override void DidStartRequiringUserInterventionForBluetooth()
        {
            _appDelegate.updateLog("Started requiring user intervention for BT");
        }

        public override void DidStartRequiringUserInterventionForLocationServicesAuthorizationStatus(CLAuthorizationStatus authorizationStatus)
        {
            _appDelegate.updateLog("Started requiring user intervention for Location services");
        }

        public override void DidStartRequiringUserInterventionForPowerMode()
        {
            _appDelegate.updateLog("Started requiring user intervention for power mode");
        }

        public override void DidStopRequiringUserInterventionForBluetooth()
        {
            _appDelegate.updateLog("Stopped requiring user intervention for BT");
        }

        public override void DidStopRequiringUserInterventionForLocationServicesAuthorizationStatus(CLAuthorizationStatus authorizationStatus)
        {
            _appDelegate.updateLog("Stopped requiring user intervention for Location services");
        }

        public override void DidStopRequiringUserInterventionForPowerMode()
        {
            _appDelegate.updateLog("Stopped requiring user intervention for Power mode");
        }

        public override void DidUpdateZoneInfo(NSSet zoneInfos)
        {
            _appDelegate.updateLog("Zone info updated");
        }
    }
}

