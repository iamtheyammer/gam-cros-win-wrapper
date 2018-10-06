# Globals.cs
Contains global variables and methods about them.

## Variables
- `public static string DeviceId`
  - Current device id
  - May not be present
  - Can be `get` and `set`
- `public static string Note`
  - Current device note
  - May not be present
  - Can be `get` and `set`
- `public static string Status`
  - Current device status -- like "DEPROVISIONED" or "ACTIVE"
  - May not be present
  - Can be `get` and `set`
- `public static string User`
  - Current device annotatedUser
  - May not be present
  - Can be `get` and `set`
- `public static string AssetId`
  - Current device asset id
  - May not be present
  - Can be `get` and `set`
- `public static string Location`
  - Current device annotatedLocation
  - May not be present
  - Can be `get` and `set`
- `public static string SerialNumber`
  - Current device serial number
  - May not be present
  - Can be `get` and `set`
- `public static string CsvLocation`
  - If the user imported from a CSV, this is the absolute path to it.
  - May not be present
  - Can be `get` and `set`
- `public static  readonly HttpClient HttpClientObject`
  - A global instance of HttpClient because you shouldn't have more than one
  - Read only

## `DeviceIdExists()`
- Returns a boolean telling you whether a device id is a present global

### Inputs
None

### Outputs
A boolean: true if Globals.DeviceId != null, false otherwise

## `ClearGlobals()`
- Clears globals by setting them all to null

### Inputs
None

### Outputs
None

## `SetGlobalsFromBasicDeviceInfo(BasicDeviceInfo info)`
- Sets globals from a BasicDeviceInfo object (custom class defined in ObjectClasses.md)

### Inputs
- `BasicDeviceInfo info` - the instance of BasicDeviceInfo you want to set globals from

### Outputs
None
