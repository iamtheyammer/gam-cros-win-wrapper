# Oddities
Things you should know if you're contributing to this project:

- In MainWindow.xaml.cs, I use a `dynamic` to hold the current view (`currentView`), and it must have the method `ToggleMainWindowButtons(bool value)`
