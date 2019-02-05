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
	public class AppDelegate : UIApplicationDelegate, IBDPSessionDelegate, IBDPLocationDelegate
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

            BDLocationManager.Instance.SessionDelegate = this;
            BDLocationManager.Instance.LocationDelegate = this;

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

        #region IBDPSessionDelegate
        public void WillAuthenticateWithApiKey(string apiKey)
        {
            updateLog("Authenticating..");
        }

        public void AuthenticationWasSuccessful()
        {
            updateLog("Authentication was successful");
            updateAuthenticationStatus("Logout");
        }

        public void AuthenticationWasDeniedWithReason(string reason)
        {
            updateLog("Authentication denied");
            updateAuthenticationStatus("Authenticate");
        }

        public void AuthenticationFailedWithError(NSError error)
        {
            updateLog("Authentication failed");
            updateAuthenticationStatus("Authenticate");
        }

        public void DidEndSession()
        {
            updateLog("Session ended");
            updateAuthenticationStatus("Authenticate");
        }

        public void DidEndSessionWithError(NSError error)
        {
            updateLog("Session ended with error");
            updateAuthenticationStatus("Authenticate");
        }
        #endregion

        #region IBDPLocationDelegate
        public void DidCheckIntoFence(BDFenceInfo fence, BDZoneInfo zoneInfo, BDLocationInfo location, bool willCheckOut, NSDictionary customData)
        {
            updateLog("Checked into fence");
        }

        public void DidCheckIntoBeacon(BDBeaconInfo beacon, BDZoneInfo zoneInfo, BDLocationInfo locationInfo, CLProximity proximity, bool willCheckOut, NSDictionary customData)
        {
            updateLog("Checked into beacon");
        }

        public void DidCheckIntoBeacon(BDBeaconInfo beacon, BDZoneInfo zoneInfo, CLProximity proximity, NSDate date, bool willCheckOut, NSDictionary customData)
        {
            updateLog("Checked into beacon");
        }

        public void DidCheckIntoFence(BDFenceInfo fence, BDZoneInfo zoneInfo, BDLocationCoordinate2D coordinate, NSDate date, bool willCheckOut, NSDictionary customData)
        {
            updateLog("Checked into fence");
        }

        public void DidCheckOutFromBeacon(BDBeaconInfo beacon, BDZoneInfo zoneInfo, CLProximity proximity, NSDate date, nuint checkedInDuration, NSDictionary customData)
        {
            updateLog("Checked out from beacon");
        }

        public void DidCheckOutFromFence(BDFenceInfo fence, BDZoneInfo zoneInfo, NSDate date, nuint checkedInDuration, NSDictionary customData)
        {
            updateLog("Checked out from fence");
        }

        public void DidStartRequiringUserInterventionForBluetooth()
        {

        }

        public void DidStartRequiringUserInterventionForLocationServicesAuthorizationStatus(CLAuthorizationStatus authorizationStatus)
        {

        }

        public void DidStartRequiringUserInterventionForPowerMode()
        {

        }

        public void DidStopRequiringUserInterventionForBluetooth()
        {

        }

        public void DidStopRequiringUserInterventionForLocationServicesAuthorizationStatus(CLAuthorizationStatus authorizationStatus)
        {

        }

        public void DidStopRequiringUserInterventionForPowerMode()
        {

        }

        public void DidUpdateZoneInfo(NSSet zoneInfos)
        {
            updateLog("Zone Info updated");
        }
        #endregion

        private void updateLog(String s)
        {
            var viewController = Window.RootViewController as ViewController;
            if (viewController == null)
            {
                return;
            }

            viewController.StatusLog.Text += "\n" + s;
        }

        private void updateAuthenticationStatus(String s)
        {
            var viewController = Window.RootViewController as ViewController;
            if (viewController == null)
            {
                return;
            }

            viewController.Authenticate.SetTitle(s, UIControlState.Normal);
        }
    }
}

