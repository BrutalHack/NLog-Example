using NLog;
using NLog.Targets;
using UnityEngine;

[Target("UnityDebugLog")]
public sealed class UnityDebugLogTarget : TargetWithLayout
{
    // Unity only knows 3 log levels: Info, Warn, Error.
    protected override void Write(LogEventInfo logEvent)
    {
        string logMessage = Layout.Render(logEvent);
        if (logEvent.Level == LogLevel.Warn)
        {
            Debug.LogWarning(logMessage);
        }
        else if (logEvent.Level == LogLevel.Error ||
                 logEvent.Level == LogLevel.Fatal)
        {
            Debug.LogError(logMessage);
        }
        else if (logEvent.Level == LogLevel.Info ||
                 logEvent.Level == LogLevel.Trace ||
                 logEvent.Level == LogLevel.Debug)
        {
            Debug.Log(logMessage);
        }
        else
        {
            // Unknown Loglevel
            Debug.Log(logMessage);
        }
    }
}