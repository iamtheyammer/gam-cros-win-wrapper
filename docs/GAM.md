# GAM.cs
Arguably the most important class here. It holds all of the logic that connects to GAM.

## `RunGAM(String gamCommand)`
- Runs GAM on the command line with passed in "arguments" -- basically what you would type after `gam `.

### Inputs
- `String gamCommand` - what you would type after `gam ` if you were using a command line

### Outputs
- `List<string>` containing, line by line, the output from the command you ran

## `RunGAM(String gamCommand)`
- Performs the same function as RunGAM, but uses a foreach to return the GAM output, seperated by newline characters (`\n`).

### Inputs
Same as `RunGAM`

### Outputs
- `string` containing all lines of output from the command separated by newline characters (`\n`)

## `RunGAMCommasFixed(String gamCommand)`
- Performs the same function as RunGAM + FixCSVCommas.FixCommas.

### Inputs
Same as `RunGAM`

### Outputs
- `List<List<string>>` containing, in nested order from inside to out, each line of the file, then the elements inside of that line.

## `IsDeviceId(string input)`
- Tells you, with a bool, whether your `input` is a device id or not. It does this by counting the number of dashes (`-`) in the `input`, and if there are four, it returns true.

### Inputs
- `string input` - the string to check for a deviceId or not

### Outputs
- `bool` - true if it's a device id false if not

## `GetDeviceId(String input)`
- Takes in a single string, that may contain a(n):
  - Serial Number
  - Asset ID
  - Device ID
  - Email
and processes it, using `RunGAM`, into a device id.

### Inputs
- `string input` - the input you wish to turn into a device id

### Outputs
- `BasicDeviceInfo` (custom class, defined in ObjectClasses.md), with info on this device. Always includes DeviceId, not anything else is guaranteed.
