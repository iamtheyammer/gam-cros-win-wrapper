# Updates.cs

## `IsNewestVersion()`
- Returns a string that acts like a bool with whether the software is up to date. It is checking only for the current Software.Type, so if `Software.Type = "beta"` it will look for the current beta release.

### Inputs
None

### Outputs
- string-bool-thing containing status:
  - `"true"` - software is up to date
  - `"false"` - software is not up to date
  - `"error"` - error getting current version. most likely no internet connection

## `CheckForUpdates()`
- Checks for updates, and returns "true", if an update is available, "false", if one is not and "error" if an error occurs.

### Inputs
None

### Outputs
- string-bool-thing with the software update-ness:
  - `"true"` - an update is available, and the user has already been prompted of such
  - `"false"` - the software is up to date
  - `"error"` - error getting current version. most likely no internet connection
