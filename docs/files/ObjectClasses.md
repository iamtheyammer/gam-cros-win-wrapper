# ObjectClasses.cs
Holds class definitions used as objects only. Has no methods. All classes are public and non-static.

## OrgUnit
A Google Admin organizational unit.

| Attribute name | Attribute description | Attribute type | Get-able | Set-able |
| :------------- | :------------- | :-- | :-- | :-- |
| OrgUnitPath | Absolute path of an OrgUnit | string | ✓ | ✓ |
| OrgUnitName | Name of the OrgUnit | string | ✓ | ✓ |
| OrgUnitDescription | Description of the OrgUnit | ✓ | ✓ |

## BasicDeviceInfo
Holds information about a Chromebook.

| Attribute name | Attribute description | Attribute type | Get-able | Set-able |
| :------------- | :------------- | :-- | :-- | :-- |
| DeviceId | self-explanatory | string | ✓ | ✓ |
| LastSync | Last time this device synched to the cloud | string | ✓ | ✓ |
| SerialNumber | self-explanatory | string | ✓ | ✓ |
| Status | Current enrollment status-- like "DEPROVISIONED" or "ACTIVE" | string | ✓ | ✓ |
| Notes | self-explanatory | string | ✓ | ✓ |
| AssetId | self-explanatory | string | ✓ | ✓ |
| Location | self-explanatory | string | ✓ | ✓ |
| Error | If there was an error getting information about this device | bool | ✓ | ✓ |
| ErrorText | If there was an error, what is the full text | string | ✓ | ✓ |

## Button
Holds extremely basic information about a button.

| Attribute name | Attribute description | Attribute type | Get-able | Set-able |
| :------------- | :------------- | :-- | :-- | :-- |
| IsEnabled | self-explanatory | bool | ✓ | ✓ |
| Text | Should be called Content, the content of the button | bool | ✓ | ✓ |
