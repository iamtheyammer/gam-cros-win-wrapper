# GetInput.cs
Allows you to get input from the user with different forms of such.

## `getInput(String instructionText, String inputBoxPrefill, String title, Button button)`
- Allows you to get some input from the user in single string form.

### Inputs
  - `String instructionText` - the main instructions for the user, will be in a large clear area
  - `String inputBoxPrefill` - (overload optional) what will be prefilled in the input box
  - `String title` - (overload optional) the window title
  - `Button button` - (overload optional) (custom Button class, defined in ObjectClasses.md) allows you to add an extra button if you so choose. You can not return both the extra button press event and the text they inputted.

### Outputs
A string with either "ExtraButtonClicked" if they clicked the extra button or whatever was in the input field when they clicked OK.

### Overloads
| `instructionText` | `inputBoxPrefill` | `title` | `button` |
| :------------- | :------------- | :-- | :-- |
| ✓ | ✓ | ✓ | ✓ |
| ✓ | ✓ | "Enter input here..." | ✕ |
| ✓ | "Enter value here..." | "InputWindow" | ✕ |
| ✓ | ✓ | "InputWindow" | ✕ |

## `GetDeprovisionReason()`
- Gets the deprovision reason for a device. Either:
  - Same model replacement
  - Different model replacement
  - Retiring device

### Inputs
None

### Outputs
- `int` containing the deprovison reason:

| They pressed cancel/closed window | Same model replacement | Different model replacement | Retiring device |
| :------------- | :------------- | :-- | :-- |
| 0 | 1 | 2 | 3 |

## `GetFileSelection (String fileType)`
- Opens a file selector box, looking for a `fileType`

### Inputs
- `String fileType` - a file extension without the dot, like `doc`, `png` or `csv`.

### Outputs
- `string` - absolute path (starts at `C:\`) to the file

## `ShowInfoDialog(string title, string subject, string fullText, Button extraButton)`
- Opens a small, informational dialog to provide the user info.

### Inputs
- `string title` - window title
- `string subject` - the large header text in the dialog
- `string fullText` - the full paragraph text for the user to see
- `Button extraButton` - (overload optional) (custom class, defined in ObjectClasses.md) an extra button, if you'd like. If this button is clicked, the function will return true, otherwise it will return false.

### Overloads
| `title` | `subject` | `fullText` | `button` |
| :------------- | :------------- | :-- | :-- |
| ✓ | ✓ | ✓ | ✓ |
| ✓ | ✓ | ✓ | ✕ |
