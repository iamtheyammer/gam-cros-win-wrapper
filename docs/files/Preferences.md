## Preferences

This directory holds files that will be included with the installer packages. Currently it holds `preferences.json`, which holds the settings for this application.


| Preference | Description | Type | Default Value |
| :------------- | :-------| :---- | :--- |
| SerialNumberAssetIdPriority | Tells whether we search for serials first or asset IDs first | `bool` | `false` |
| ShowWarningWhenImportingFromCSVFile | Tells whether when we import data from a CSV file we should show a warning | `bool` | `true` |

Want to make a new preference? Update:
- Preferences.cs
- PreferencesWindow.xaml.cs
