   M S B A M L   `   `   `  ���� H  DChromebookGUI, Version=1.1.3.0, Culture=neutral, PublicKeyToken=null-clr-namespace:ChromebookGUIChromebookGUI  %    ChromebookGUI.PreferencesWindow   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     -localclr-namespace:ChromebookGUI       � Title$  PreferencesWindow��$	��450��$	��800��.��� .+��� -    ��Name$ SaveAndCloseButton��$��Save and Close��$
��Left=�$��660,392,0,0q�$	��Top=�$	��124���� -   $ CancelButton��$��Cancel��$
��Left=�$��519,392,0,0q�$	��Top=�$	��124���� -    ��Name$ PrioritiesTextBlock��$
��Left=�$��	10,55,0,0q�$
��Wrap=�$��
Priorities��$	��Top=�$��25��$��81��$��18(��� -    ��Name$2 ,SearchForAssetIdsBeforeSerialNumbersCheckBox�� � ToolTip$� �When searching for devices, Serial Numbers and Asset IDs use different queries. If you search for Asset IDs more often, tick this box. If you search for Serial Numbers more often, leave it unticked.��$0��*Search for Asset IDs before Serial Numbers��$
��Left=�$��	15,85,0,0q�$	��Top=�$	��254���� -   $ InstructionTextBlock��$
��Left=�$��	10,10,0,0q�$
��Wrap=�$/��)Hover over a setting for more information��$	��Top=�$��24(��� -   $ PrioritiesTextBlock_Copy��$
��Left=�$��
10,127,0,0q�$
��Wrap=�$��Warnings��$	��Top=�$��25��$��81��$��18(��� -   $) #ShowWarningWhenImportingFromCSVFile��$R LWhen importing from a CSV file, show the default warning. Untick to silence.��$/��)Show warning when importing from CSV file��$
��Left=�$��
15,157,0,0q�$	��Top=�$	��254���� -   $ UpdatesTextBlock��$
��Left=�$��
372,55,0,0q�$
��Wrap=�$��Updates��$	��Top=�$��25��$��81��$��18(��� -	   $+ %PromptWhenUpdatesAreAvailableCheckBox��$6��0Prompt when updates are available (recommended!)��$� �When updates are available for this app, let me know when the app opens. Turning this off will only check for updates when you go to Help -> Check for updates... ��$
��Left=�$��
388,91,0,0q�$	��Top=��� -
   $ ViewsTextBlock��$
��Left=�$��
11,194,0,0q�$
��Wrap=�$��Views��$	��Top=�$��25��$��81��$��18(��� -   $3 -UseTextBoxLayoutInsteadOfButtonLayoutCheckBox��$� �There are 2 layouts in ChromebookGUI: the classic Button layout and the Text Box layout. Use this to set your default view. You can go to the View menu to change on the fly.��$:��4Use the Text Box layout instead of the Button layout��$
��Left=�$��
15,224,0,0q�$	��Top=�$	��297���� -   $ TelemetryTextBlock��$
��Left=�$��372,124,0,0q�$
��Wrap=�$��	Telemetry��$	��Top=�$��25��$��81��$��18(��� -   $$ AllowEnhancedTelemetryCheckbox��$-��'Send crash data to developers (please!)��$� �The data sent to developers includes personal information, including your email. It may also include information about queries and current devices. It is only available to contributors to the GitHub project. It makes debugging much easier. (You must restart ChromebookGUI for this to take effect).��$
��Left=�$��388,160,0,0q�$	��Top=�