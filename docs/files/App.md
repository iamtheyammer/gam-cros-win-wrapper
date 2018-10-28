# App.xaml and App.xaml.cs

## App.xaml
Empty (pretty much) since this is not a console application. It does however define which functions in App.xaml.cs will run on startup and exit.

## App.xaml.cs

Holds two functions: `Application_Startup` and `Application_Exit`.
- `Application_Startup`
  - Disables main window buttons
  - Shows main window
  - Checks for updates
  - Initializes preferences
- `Application_Exit`
  - Saves preferences
