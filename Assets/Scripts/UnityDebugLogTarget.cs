using NLog;
using NLog.Config;
using NLog.Targets;

[Target("UnityDebugLog")]
public sealed class UnityDebugLogTarget : TargetWithLayout
{
    // Unity only knows 3 log levels: Info, Warn, Error.
    protected override void Write(LogEventInfo logEvent)
    {
        string logMessage = Layout.Render(logEvent);
        if (logEvent.Level == LogLevel.Warn)
        {
            UnityEngine.Debug.LogWarning(logMessage);
        }
        else if (logEvent.Level == LogLevel.Error ||
                 logEvent.Level == LogLevel.Fatal)
        {
            UnityEngine.Debug.LogError(logMessage);
        }
        else if (logEvent.Level == LogLevel.Info ||
                 logEvent.Level == LogLevel.Trace ||
                 logEvent.Level == LogLevel.Debug)
        {
            UnityEngine.Debug.Log(logMessage);
        }
        else
        {
            // Unknown Loglevel
            UnityEngine.Debug.Log(logMessage);
        }
    }
}