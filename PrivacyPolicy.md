# ChromebookGUI Privacy Policy
## When this policy takes effect
This policy takes effect when data from your usage is sent back to developers (contributors to this repository). Currently, this only includes crash and error reporting to Sentry.io.

## What we send
### With your permission
(See [your choices](#your-choices)), we may send these types of data:

- Email you're signed into GAM with
  - If you experience a crash, we may send you an email asking for more information about the crash.
- Details about your current device
  - May include:
    - Device ID
    - Serial Number
    - Notes
    - Asset ID
    - User
    - Status
    - Organizational unit
  - We use this to understand the context of the crash, as many occur due to the way gam reports data. Seeing this data is the only way to reproduce the crash.
- Text in the Omnibar
  - We use this to understand how the Omnibar parsed your query.
- A list of your organizational units
  - We use this to investigate crashes with the org unit selector
- Your current preferences
  - Used to investigate preferences-related crashes along with understanding the crash context
  
### May always be sent
- Your IP address (per Sentry)
  - Honestly, this isn't too useful, but Sentry collects it. We sometimes use it to link related crashes.
- Versions of:
  - .NET
  - Windows
  - ChromebookGUI
  - GAM
  - Packages installed by the ChromebookGUI installer
  - Sentry SDK
  - We use these versions to help replicate the crash.
- Crash call stack
  - The most helpful thing: tells us what function the error occured in.
 
  
## Your Choices
### First launch
When ChromebookGUI v>1.0.11 is installed, users will be prompted for a telemetry consent to send the data types listed [here](#with-your-permission). If they deny, only data in [this](#may-always-be-sent) section will be sent. If they accept, all data listed [here](#what-we-send) will be sent to Sentry.

You can see a screenshot (with personal information blurred) of what a full report looks like to me (just email and globals with this one) [here](/images/sample-sentry.png).

### After the first launch
At any time, you may grant/revoke extra telemetry permissions by opening ChrombookGUI, going to File -> Preferences... and checking/unchecking the box labeled "Send crash data to developers".

## What we do with the data
We use this data to fix crashes in the app. Honestly, it's invaluable. This way, you don't have to open a GitHub issue to help me improve ChromebookGUI. We don't collect donations, so this is the best thing you can do do help ChromebookGUI.

It helps me to replicate your issue so I can find the source and resolve it.

## Sentry.io's Privacy Policy
For your reference, you can see our error reporting utility (Sentry.io)'s privacy policy [here](https://sentry.io/privacy/)

## Thank you
I know it's hard to give up data, but this is pretty much the only way for me to resolve crashes without opening a GitHub issue. 
Thank you for using my software and trusting me with your data.

p.s. if you disable [enhanced telemetry](#with-your-permission) and you encounter a crash, please open a GitHub issue with all requested information.
