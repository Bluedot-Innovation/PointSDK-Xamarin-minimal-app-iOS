// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace BDPointiOSXamarinDemo
{
    [Register ("ViewController")]
    partial class ViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        public UIKit.UIButton Authenticate { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        public UIKit.UITextView StatusLog { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (Authenticate != null) {
                Authenticate.Dispose ();
                Authenticate = null;
            }

            if (StatusLog != null) {
                StatusLog.Dispose ();
                StatusLog = null;
            }
        }
    }
}