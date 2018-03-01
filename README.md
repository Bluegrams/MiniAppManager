# MiniAppManager
[![NuGet](https://img.shields.io/nuget/v/MiniAppManager.WPF.svg?label=nuget+for+WPF)](https://www.nuget.org/packages/MiniAppManager.WPF/)
[![NuGet](https://img.shields.io/nuget/v/MiniAppManager.WinForms.svg?label=nuget+for+WinForms)](https://www.nuget.org/packages/MiniAppManager.WinForms/)

Automatically saves an application's location, size and window state and adds a simple 'About' box 
to your WinForms or WPF application.

## What it is
MiniAppManager is a small library that handles saving some basic application settings, such as location
and size, for a WinForms or WPF app. This makes sure the app will be started in the same state it was 
closed before.

#### Overview:
* Automatically stores app's location, size and window state
* Simple 'About' box showing assembly information, project's website and icon...
* 'About' box supports switching of app's language

## How it works
1. Add the nuget package for [WPF](https://www.nuget.org/packages/MiniAppManager.WPF) or [WinForms](https://www.nuget.org/packages/MiniAppManager.WinForms/) to your project.
2. Add these using directives (for a WPF app):
```csharp
using Bluegrams.Application;
using Bluegrams.Application.WPF;
```
3. Initialize MiniAppManager (example from [TestWpfApp](TestWpfApp/MainWindow.xaml.cs), usage for WinForms is similar):
```csharp
// Define a new variable for the manager of this app.
MiniAppManager man;

public MainWindow()
{
    var baseUri = BaseUriHelper.GetBaseUri(this);
    BitmapSource img = new BitmapImage(new Uri(baseUri, @"/bluelogo.png"));
    
    // Create a new instance of MiniAppManager for WPF with some data used in the 'About' box.
    man = new MiniAppManager(this, Color.FromRgb(51, 85, 119), img, 
                              new Link("http://example.org", "Example.org"), 
                              new Link("https://opensource.org/licenses/MIT", "MIT License"));

    // (Optional) Specifiy a list of cultures your application supports to fill 
    // a combo box that allows switching between these.
    // If this property is not specified, the combo box won't be visible on the 'About' box.
    man.SupportedCultures = new CultureInfo[] { new CultureInfo("en"), new CultureInfo("de") };

    //Initialize the manager. Please make sure this method is called BEFORE you initialize your window.
    man.Initialize();

    //Initialize the window.
    InitializeComponent();
}
```
4. (Optional) Open the 'About' box:
```csharp
private void butAbout_Click(object sender, RoutedEventArgs e)
{
    man.ShowAboutBox();
}
```
5. That's it.

## License
This project is licensed under the [BSD-2-Clause](LICENSE) license.
