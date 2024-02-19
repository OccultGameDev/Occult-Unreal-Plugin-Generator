# Unreal Plugin Generator
 A Tool for quickly generating the core required files needed to compile a Plugin.

## Command Line Arguments
```
Set the Target Directory we want to generate the Plugin in:
-d "C:/Directory"

Set the Name of the Plugin:
-n "MyCoolPluginName"

If set to true, the Plugin will open a GUI with the supplied Command Line Args without generating
-g true

Set the Copyright Notice to be generated at the top of the files.
-c "My Excellent Copyright Notice. All Rights Reserved."
```

## Template Arguments
Template Arguments will be replaced in the final generated file.
```
{COPYRIGHT_HEADER} - Will display the Text supplied, or fallback to a default Epic Games Copyright Notice.
{PLUGIN_API} - Will generate an API for your Plugin so it can be accessed by other modules.
{PLUGIN_NAME} - Will display a sanitized, code-safe Name for the Plugin.
{LOG_CHANNEL} - If Logging is enabled, it will display the Log for this plugin, otherwise it will fall back to LogTemp.
{LOG_CHANNEL_DEFINE} - Will attempt to display the Log for this plugin, without a Fallback.
```

