using Foundation;
using UIKit;
using System;
using PointSDK.iOS;
using UserNotifications;

namespace BDPointiOSXamarinDemo
{
	// The UIApplicationDelegate for the application. This class is responsible for launching the
	// User Interface of the application, as well as listening (and optionally responding) to application events from iOS.
	[Register("AppDelegate")]
	public partial class AppDelegate : UIApplicationDelegate
    {
        // class-level declarations

        private Random random = new Random();

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
            BDLocationManager.Instance.GeoTriggeringEventDelegate = new GeoTriggeringEventDelegate(this);
            BDLocationManager.Instance.TempoTrackingDelegate = new TempoTrackingDelegate(this);

            UNUserNotificationCenter.Current.RequestAuthorization(UNAuthorizationOptions.Alert, (approved, err) => {
                if(err != null)
                {
                    Console.WriteLine("Error on requesting user notification authorization: " + err.LocalizedDescription);
                }
            });
            UNUserNotificationCenter.Current.Delegate = new UserNotificationDelegate();

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
            viewController.UpdateLog(s);
        }

        public void sendLocalNotification(String title, String message)
        {
            // Create content
            var content = new UNMutableNotificationContent();
            content.Title = title;
            content.Body = message;

            var trigger = UNTimeIntervalNotificationTrigger.CreateTrigger(1, false);

            var requestID = "request" + random.Next().ToString();
            var request = UNNotificationRequest.FromIdentifier(requestID, content, trigger);

            UNUserNotificationCenter.Current.AddNotificationRequest(request, (err) => {});
        }

    }

    class GeoTriggeringEventDelegate : IBDPGeoTriggeringEventDelegate
    {
        private readonly AppDelegate _appDelegate;

        public GeoTriggeringEventDelegate(AppDelegate app)
        {
            _appDelegate = app;
        }

        public override void DidUpdateZoneInfo()
        {
            // ZoneInfos is no longer passed via the callback, access it via BDLocationManager.Instance.ZoneInfos
            // BDLocationManager.Instance.ZoneInfos can be null when calling SDK's reset method, don't forget to check null
            _appDelegate.updateLog("ZoneInfo Updated: " + (BDLocationManager.Instance.ZoneInfos != null ? BDLocationManager.Instance.ZoneInfos.Description : " empty"));
        }

        public override void DidEnterZone(GeoTriggerEvent enterEvent)
        {
            String message = "Zone: " + enterEvent.ZoneInfo.Name + " Entered";
            _appDelegate.updateLog(message);
            _appDelegate.sendLocalNotification("Entered Zone", message);
        }

        public override void DidExitZone(GeoTriggerEvent exitEvent)
        {
            String message = "Zone: " + exitEvent.ZoneInfo.Name + " Exited";
            _appDelegate.updateLog(message);
            _appDelegate.sendLocalNotification("Exited Zone", message);
        }
    }

    class TempoTrackingDelegate : IBDPTempoTrackingDelegate
    {
        private readonly AppDelegate _appDelegate;

        public TempoTrackingDelegate(AppDelegate app)
        {
            _appDelegate = app;
        }

        public override void TempoTrackingDidUpdate(TempoTrackingUpdate tempoUpdate)
        {
            _appDelegate.updateLog("TempoTrackingDidUpdate: " + tempoUpdate.Description);
        }

        public override void TempoTrackingDidExpire()
        {
            _appDelegate.updateLog("TempoTrackingDidExpire");
        }

        public override void DidStopTrackingWithError(NSError error)
        {
            _appDelegate.updateLog("DidStopTrackingWithError" + error.LocalizedDescription);
        }
    }

    public class UserNotificationDelegate : UNUserNotificationCenterDelegate
    {
        public UserNotificationDelegate()
        {
        }

        public override void WillPresentNotification(UNUserNotificationCenter center, UNNotification notification, Action<UNNotificationPresentationOptions> completionHandler)
        {
            completionHandler(UNNotificationPresentationOptions.Alert);
        }
    }
}

