# MiniAppManager
[![NuGet](https://img.shields.io/nuget/v/MiniAppManager.WPF.svg?label=nuget+for+WPF&colorB=004880)](https://www.nuget.org/packages/MiniAppManager.WPF/)
[![NuGet](https://img.shields.io/nuget/v/MiniAppManager.WinForms.svg?label=nuget+for+WinForms&colorB=004880)](https://www.nuget.org/packages/MiniAppManager.WinForms/)

Automatically saves a window's state, makes settings portable, checks for updates
and adds a simple 'About' box to your WinForms or WPF application.

## What it is
MiniAppManager is a small library that handles saving some basic application settings, such as location
and size, as well as custom properties for a WinForms or WPF app. This makes sure the app will be started
in the same state it was closed before.

#### Overview:
* Automatically store an app's location, size, window state and any custom window property
* Portable mode for managed properties and any custom settings
* Simple 'About' box showing assembly information, product website, license and icon etc.
* 'About' box supports switching of app's language
* Automatically check for new updates

## How it works
In the following, some examples show the different features of MiniAppManager. To get started, just:
* Add the nuget package for [WPF](https://www.nuget.org/packages/MiniAppManager.WPF) 
	or [WinForms](https://www.nuget.org/packages/MiniAppManager.WinForms/) to your project.
* Add these using directives (for a WPF app):
```csharp
using Bluegrams.Application;
using Bluegrams.Application.WPF;
```
**Note:** All examples below are given for a WPF app (see [TestWpfApp](TestWpfApp/MainWindow.xaml.cs) for more),
the usage for WinForms is similar.  
**Second note:** All examples apply to MiniAppManager v.0.4.x or higher.

#### Example 1: Persist Window State
After initialized, MiniAppManager automatically saves and restores some common properties of an app window 
(see [here](https://github.com/bluegrams/MiniAppManager/wiki/List-of-Managed-Settings)). Custom properties can be easily added.
```csharp
// Define a new variable for the manager of this app.
MiniAppManager manager;

public MainWindow()
{
    // Create a new instance of MiniAppManager for WPF.
    manager = new MiniAppManager(this);

    // Add any public property of the window with this method to let its value
    // be saved when the application is closed and loaded when it starts.
    manager.AddManagedProperty(nameof(this.OpenCount));

    // Initialize the manager. Please make sure this method is called BEFORE the window is initialized.
    manager.Initialize();

    //Initialize the window.
    InitializeComponent();
}
```

#### Example 2: Make Settings Portable
If MiniAppManager is run in portable mode, all managed properties are saved in a file in the app folder.
MiniAppManager can also make any additional application settings portable. 
[Learn more](https://github.com/bluegrams/MiniAppManager/wiki/Portable-Mode)
```csharp
// Define a new variable for the manager of this app.
MiniAppManager manager;

public MainWindow()
{
    // Create a new instance of MiniAppManager for WPF. 
    // The second parameter specifies that the manager should be run in portable mode.
    manager = new MiniAppManager(this, true);

    // Make additional application settings portable.
    manager.MakePortable(Properties.Settings.Default);

    // Initialize the manager and the window.
    manager.Initialize();
    InitializeComponent();
}
```

#### Example 3: Application 'About' Box
MiniAppManager can show a standardized window containing information about your application.
```csharp
private void butAbout_Click(object sender, RoutedEventArgs e)
{
    // Load the icon used in the 'About' box.
    var baseUri = BaseUriHelper.GetBaseUri(this);
    BitmapSource icon = new BitmapImage(new Uri(baseUri, @"/bluelogo.png"));
    // Show the 'About' box.
    // The shown data is specified as assembly attributes in AssemblyInfo.cs.
    manager.ShowAboutBox(icon);
}
```
Some additional information for this window can be given with a few custom assembly attributes:
```csharp
[assembly: ProductWebsite("http://example.org", "Example.org")]
[assembly: ProductLicense("https://opensource.org/licenses/BSD-3-Clause", "BSD-3-Clause License")]
[assembly: ProductColor(51, 85, 119)]
// Specifiy a list of cultures the application explicitly supports to fill a combo box 
// that allows switching between these. If this property is not specified, 
// the combo box won't be visible on the 'About' box.
[assembly: SupportedCultures("de", "en")]
```

#### Example 4: Check for App Updates
After created as in Example 1 or 2, MiniAppManager can check for updates at a given URL. 
It expects an XML file containing update information in [this form](TestWpfApp/AppUpdateExample.xml).
```csharp
private void Window_Loaded(object sender, RoutedEventArgs e)
{
    // Specify if an informational message box should be shown if an update is available.
    manager.UpdateNotifyMode = UpdateNotifyMode.Always;
    // This event is fired when update checking has finished.
    manager.CheckForUpdatesCompleted += delegate (object s, UpdateCheckEventArgs args)
    {
        System.Diagnostics.Debug.WriteLine("Update check completed.");
    };
    // Tell the manager to check for updates at the given URL. An XML file 
    // containing a serialized AppUpdate object is expected at that location.
    // This method should also be called before the initialization of the window.
    manager.CheckForUpdates("https://raw.githubusercontent.com/bluegrams/MiniAppManager/master/TestWpfApp/AppUpdateExample.xml");
}
```

## License
This project is licensed under the [BSD-3-Clause](LICENSE) license.
