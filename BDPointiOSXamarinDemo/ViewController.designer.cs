// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace BDPointiOSXamarinDemo
{
	[Register ("ViewController")]
	partial class ViewController
	{
		[Outlet]
		UIKit.UITextField destinationIdTextFiled { get; set; }

		[Outlet]
		UIKit.UIButton initialiseSDKButton { get; set; }

		[Outlet]
		UIKit.UITextField projectIdTextField { get; set; }

		[Outlet]
		UIKit.UIButton startGeoTriggeringButton { get; set; }

		[Outlet]
		UIKit.UIButton startTempoButton { get; set; }

		[Outlet]
		UIKit.UITextView statusLog { get; set; }

		[Outlet]
		UIKit.UIButton stopGeoTriggeringButton { get; set; }

		[Outlet]
		UIKit.UIButton stopTempoButton { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (projectIdTextField != null) {
				projectIdTextField.Dispose ();
				projectIdTextField = null;
			}

			if (initialiseSDKButton != null) {
				initialiseSDKButton.Dispose ();
				initialiseSDKButton = null;
			}

			if (startGeoTriggeringButton != null) {
				startGeoTriggeringButton.Dispose ();
				startGeoTriggeringButton = null;
			}

			if (stopGeoTriggeringButton != null) {
				stopGeoTriggeringButton.Dispose ();
				stopGeoTriggeringButton = null;
			}

			if (destinationIdTextFiled != null) {
				destinationIdTextFiled.Dispose ();
				destinationIdTextFiled = null;
			}

			if (startTempoButton != null) {
				startTempoButton.Dispose ();
				startTempoButton = null;
			}

			if (stopTempoButton != null) {
				stopTempoButton.Dispose ();
				stopTempoButton = null;
			}

			if (statusLog != null) {
				statusLog.Dispose ();
				statusLog = null;
			}
		}
	}
}
