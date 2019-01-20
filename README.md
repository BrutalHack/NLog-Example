# NLog-Example
Example Project for integrating NLog in Unity

A detailed article about this sample project will be uploaded to our website in the following days.

## Importing the NLog Assembly

Sadly, Unity projects do not support nuget packages easily. We had setup Unity+NuGet once and it was too tricky in practice.
So instead, we decided to manually include NLog as a .dll file.

So go and get your .dll file from a trustworthy source. We will trust https://www.nuget.org 

1. Visit https://www.nuget.org/packages/NLog/
2. Click on "Download Package" on the right to get the .nupkg file
3. Rename the .nupkg file to .zip (.nupkg is a zip file with a special structure inside)
4. Within the .zip, we need the file lib/net45/NLog.dll (see screenshot below)
5. Copy the NLog.dll file into your Unity project at Assets/Plugins/NLog/NLog.dll

Now open Unity and it will import the .dll correctly.

## Setting up and using NLog

The project contains 3 important script files:
1. WriteLogMessages – This MonoBehaviour provides an example on how to use the CustomLogger
2. UnityDebugLogTarget – This is our custom LogTarget for NLog to write to Unitys console (including the android & ios remote debugging consoles!). We only use Debug.Log, Debug.Warn and Debug.Error. I am sure there is a better way to write our log messages to the console, but this is totally enough. Read more about custom LogTargets here: https://github.com/NLog/NLog/wiki/How-to-write-a-custom-target
3. CustomLogger – This is our NLog logger and it contains 3 interesting parts:
    1. static CustomLogger() – The static constructor of a class is automatically called before the first method is called. Here we initialize NLog. We cannot simply initialize NLog with the typical xml config file, as we can not put the config file where NLog expects it. So we can either configure NLog via Code or maybe read the NLog config from a custom location. We offer an example with configuration via code.
    2. #region Public Logging Methods – This region contains Info, Warn and Error Methods which only do two things: They call the private Method Log and they provide CallerInformation (read more about it here: https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/caller-information). Usually you create one logger for each class by calling  NLog.LogManager.GetCurrentClassLogger()". But by using CallerInformation, we can get the file name (you should only have 1 csharp class per file anyway, so this is enough), method name and source line number. The best part? This happens without any expensive reflection. There is zero overhead. The C# compiler writes this information into the .Net Bytecode (IL) when you build your project.
This also means that this information is still there, after Unity cross compiles your project to iOS, PS4, etc. The Log Entries will still include your original filenames and line numbers.
    3. private static void Log – Here you can see how we use the CallerInformation to output it into the Log. We also use the caller filename as the logger name. And then we simply call logger.log. NLogs configuration handles what happens then.

Now all we have to do is write CustomLogger.Info("Hello there!") and NLog writes the message via Debug.Log and to our log file.
