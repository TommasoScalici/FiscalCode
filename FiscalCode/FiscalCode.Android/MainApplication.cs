using Android.App;
using Android.Runtime;

using Plugin.CurrentActivity;

using System;

namespace FiscalCode.Droid
{
#if DEBUG
    [Application(Debuggable = true, Icon = "@mipmap/icon")]
#else
    [Application(Debuggable = false, Icon = "@mipmap/icon")]
#endif

    public class MainApplication : Application
    {
        public MainApplication(IntPtr handle, JniHandleOwnership transer) : base(handle, transer) { }

        public override void OnCreate()
        {
            base.OnCreate();
            CrossCurrentActivity.Current.Init(this);
        }
    }
}
