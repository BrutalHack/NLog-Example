using System;
using System.Collections.Concurrent;
using System.IO;
using System.Runtime.CompilerServices;
using NLog;
using NLog.Config;
using NLog.Targets;
using UnityEngine;
using Logger = NLog.Logger;

/// <summary>
/// Customized Logging using NLog to target log files and the Unity Editor Log Console. <br/>
/// 
/// </summary>
public static class CustomLogger
{
    private static readonly ConcurrentDictionary<string, Logger> Loggers = new ConcurrentDictionary<string, Logger>();

    /// <summary>
    /// NLog initialization
    /// </summary>
    static CustomLogger()
    {
        Target.Register<UnityDebugLogTarget>("UnityDebugLog"); //generic

        var config = new LoggingConfiguration();
        // Configuration for file logging
        var logFile = new FileTarget("logfile")
        {
            Layout = "${longdate}|${level}|${logger}|${message} ${exception:format=tostring}",
            FileName = Application.persistentDataPath + Path.DirectorySeparatorChar + "${shortdate}-custom.log",
            KeepFileOpen = true,
            ConcurrentWrites = false,
            ArchiveNumbering = ArchiveNumberingMode.Date,
            ArchiveEvery = FileArchivePeriod.Day,
            MaxArchiveFiles = 10
        };

        // Configuration for Unity logging
        // We don't need the date here, as these logs are not saved anywhere.
        // A short timestamp helps debugging, though.
        var logUnity = new UnityDebugLogTarget
        {
            Layout = "${date:format=HH\\:mm\\:ss.ffff} - ${logger} - ${message} ${exception:format=tostring}"
        };

        // Add all loglevels to both log targets
        config.AddRuleForAllLevels(logFile);
        config.AddRuleForAllLevels(logUnity);

        LogManager.Configuration = config;
    }

    #region Public Logging Methods

    public static void Info(string message,
        [CallerMemberName] string callerMemberName = "",
        [CallerFilePath] string callerFilePath = "",
        [CallerLineNumber] int callerLineNumber = 0)
    {
        Log(LogLevel.Info, message, callerMemberName, callerFilePath, callerLineNumber);
    }

    public static void Info(Exception exception, string message = "",
        [CallerMemberName] string callerMemberName = "",
        [CallerFilePath] string callerFilePath = "",
        [CallerLineNumber] int callerLineNumber = 0)
    {
        Log(LogLevel.Info, exception, message, callerMemberName, callerFilePath, callerLineNumber);
    }

    public static void Warn(string message,
        [CallerMemberName] string callerMemberName = "",
        [CallerFilePath] string callerFilePath = "",
        [CallerLineNumber] int callerLineNumber = 0)
    {
        Log(LogLevel.Warn, message, callerMemberName, callerFilePath, callerLineNumber);
    }

    public static void Warn(Exception exception, string message = "",
        [CallerMemberName] string callerMemberName = "",
        [CallerFilePath] string callerFilePath = "",
        [CallerLineNumber] int callerLineNumber = 0)
    {
        Log(LogLevel.Warn, exception, message, callerMemberName, callerFilePath, callerLineNumber);
    }

    public static void Error(string message,
        [CallerMemberName] string callerMemberName = "",
        [CallerFilePath] string callerFilePath = "",
        [CallerLineNumber] int callerLineNumber = 0)
    {
        Log(LogLevel.Error, message, callerMemberName, callerFilePath, callerLineNumber);
    }

    public static void Error(Exception exception, string message = "",
        [CallerMemberName] string callerMemberName = "",
        [CallerFilePath] string callerFilePath = "",
        [CallerLineNumber] int callerLineNumber = 0)
    {
        Log(LogLevel.Error, exception, message, callerMemberName, callerFilePath, callerLineNumber);
    }

    #endregion

    private static void Log(LogLevel logLevel, string message,
        string callerMemberName = "", string callerFilePath = "", int callerLineNumber = 0)
    {
        Log(logLevel, null, message, callerMemberName, callerFilePath, callerLineNumber);
    }

    private static void Log(LogLevel logLevel, Exception exception, string message = "",
        string callerMemberName = "", string callerFilePath = "", int callerLineNumber = 0)
    {
        string logMessage = $"({callerMemberName}:{callerLineNumber}) {message}";
        var logger = GetLogger(callerFilePath);
        logger.Log(logLevel, exception, logMessage);
    }

    /// <summary>
    /// Gets an existing Logger 
    /// </summary>
    /// <param name="callerFilePath"></param>
    /// <returns>An existing logger, if one exists for the given file path. Else, a new logger.</returns>
    private static Logger GetLogger(string callerFilePath)
    {
        if (!Loggers.TryGetValue(callerFilePath, out Logger logger))
        {
            logger = LogManager.GetLogger(Path.GetFileName(callerFilePath));
            Loggers[callerFilePath] = logger;
        }
        return logger;
    }
}