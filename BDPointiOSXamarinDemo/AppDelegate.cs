using Foundation;
using UIKit;
using System;
using PointSDK.iOS;
using UserNotifications;
using System.Threading;

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

            // Attempt to stop Tempo before app is terminated by the user.
            // Apple doc says app has approximately 5 seconds (https://developer.apple.com/documentation/uikit/uiapplicationdelegate/1623111-applicationwillterminate) but it really depends on the hardware, OS, system resources at the time
            // Waiting or processing too long in `applicationWillTerminate` increases the risk of app being terminated by the OS.
            // Note that this function is only called by the OS when user swipes kill the app. Other cases (crash, device reboot) the OS will not call this function.
            StopTempoIfRunning();

            // or you can schedule a local notification encouraging user to re-launch the app to track their order.
            // Upon relaunching, you can initialize PointSDK, startGeoTriggering and startTempo again to get ETA updates for their order.
        }

        public void StopTempoIfRunning()
        {
            if (BDLocationManager.Instance.IsTempoRunning)
            {
                var semp = new SemaphoreSlim(0);
                BDLocationManager.Instance.StopTempoTrackingWithCompletion((NSError obj) =>
                {
                    semp.Release();
                });

                // Wait for a bit to make sure stopTempo can execute. Under normal network connection, 1-2 seconds should be enough.
                // Note: calling sepaphore.wait() will block the main thread. Blocking main thread for too long can also cause the app process be killed by the OS.
                semp.Wait(1000);
            }
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
        
        public override void OnZoneInfoUpdate(NSSet zoneInfos)
        {
            _appDelegate.updateLog("ZoneInfo Updated");
        }

        public override void DidEnterZone(BDZoneEntryEvent enterEvent)
        {
            String message = "Zone: " + enterEvent.Zone.Name + " Entered";
            _appDelegate.updateLog(message);
            _appDelegate.sendLocalNotification("Entered Zone", message);
        }

        public override void DidExitZone(BDZoneExitEvent exitEvent)
        {
            String message = "Zone: " + exitEvent.Zone.Name + " Exited";
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

