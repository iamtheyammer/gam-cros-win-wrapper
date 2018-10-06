## Preferences

This directory holds files that will be included with the installer packages. Currently it holds `preferences.json`, which holds the settings for this application.


| Preference | Description | Type |
| :------------- | :-------| :----|
| SerialNumberAssetIdPriority | Tells whether we search for serials first or asset IDs first | `bool` |
| ShowWarningWhenImportingFromCSVFile | Tells whether when we import data from a CSV file we should show a warning | `bool` |

Want to make a new preference? Update:
- Preferences.cs
- PreferencesWindow.xaml.cs
