﻿using Android.App;
using Android.Content.PM;
using Avalonia;
using Avalonia.Android;

namespace MobileSandbox.Android
{
    [Activity(Label = "MobileSandbox.Android", Theme = "@style/MyTheme.NoActionBar", Icon = "@drawable/icon", LaunchMode = LaunchMode.SingleInstance, ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize)]
    public class MainActivity : AvaloniaActivity<App>
    {
    }
}
