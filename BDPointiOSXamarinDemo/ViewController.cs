using System;
using PointSDK.iOS;
using UIKit;
using Foundation;

namespace BDPointiOSXamarinDemo
{
    public partial class ViewController : UIViewController
    {
        BDLocationManager locationManager;
        public static string projectId = "YourProjectId";
        public static string destinationId = "YourDestinationId";

        protected ViewController(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            // Perform any additional setup after loading the view, typically from a nib.
            locationManager = BDLocationManager.Instance;

            locationManager.RequestWhenInUseAuthorization();

            var keys = new object[] { "key1", "key2" };
            var values = new object[] { "value1", "value2" };
            var dict = NSDictionary.FromObjectsAndKeys(values, keys);

            locationManager.CustomEventMetaData = dict;

            initialiseSDKButton.TouchUpInside += (o, s) => {
                InitializeSDK();
            };

            startGeoTriggeringButton.TouchUpInside += (o, s) => {
                StartGeoTriggering();
            };

            stopGeoTriggeringButton.TouchUpInside += (o, s) => {
                StopGeoTriggering();
            };

            startTempoButton.TouchUpInside += (o, s) => {
                StartTempoTracking();
            };

            stopTempoButton.TouchUpInside += (o, s) => {
                StopTempoTracking();
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
				UpdateLog("Already Logged out");
			}
		}

		public void UpdateLog(string s)
		{
            string display = DateTime.Now.ToString() + ": " + s + "\n" + statusLog.Text;
			statusLog.Text = display;
		}

		public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }

        private void InitializeSDK()
        {
            if (!locationManager.IsInitialized)
            {
                locationManager.InitializeWithProjectId(projectId, (error) =>
                {
                    if (error != null)
                    {
                        UpdateLog("Error initializing SDK: " + error.LocalizedDescription);
                        return;
                    }

                    UpdateLog("SDK Initialized");
                    BDLocationManager.Instance.RequestAlwaysAuthorization();
                });
            }
            else
            {
                UpdateLog("SDK already Initialized");
            }

        }

        private void StartGeoTriggering()
        {
            locationManager.StartGeoTriggeringWithCompletion((error) =>
            {
                if (error != null)
                {
                    UpdateLog("Error Starting GeoTriggering: " + error.LocalizedDescription);
                    return;
                }

                UpdateLog("GeoTriggering Started");
            });
        }

        private void StopGeoTriggering()
        {
            locationManager.StopGeoTriggeringWithCompletion((error) =>
            {
                if (error != null)
                {
                    UpdateLog("Error Stopping GeoTriggering: " + error.LocalizedDescription);
                    return;
                }

                UpdateLog("GeoTriggering Stopped");
            });
        }

        private void StartTempoTracking()
        {
            locationManager.StartTempoTrackingWithDestinationId(destinationId, (error) =>
            {
                if (error != null)
                {
                    UpdateLog("Error Starting Tempo: " + error.LocalizedDescription);
                    return;
                }

                UpdateLog("Tempo Started");
            });
        }

        private void StopTempoTracking()
        {
            locationManager.StopTempoTrackingWithCompletion((error) =>
            {
                if (error != null)
                {
                    UpdateLog("Error Stopping Tempo: " + error.LocalizedDescription);
                    return;
                }

                UpdateLog("Tempo Stopped");
            });
        }
    }
}
