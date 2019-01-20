using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WriteLogMessages : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        LogMessages();
    }

    /// <summary>
    /// Writes 10.000 log messages and outputs the required time. 
    /// </summary>
    private static void LogMessages()
    {
        DateTime startTime = DateTime.UtcNow;
        List<TimeSpan> durations = new List<TimeSpan>();
        for (int x = 0; x < 10; x++)
        {
            for (int i = 0; i < 100; i++)
            {
                CustomLogger.Info("Application started.");
//                Debug.Log("This is a normal Unity Debug.Log message for comparison.");
                CustomLogger.Info(
                    "Welcome to the NLog Sample Project. Our log is being written to Unitys persistentDataPath.");
                CustomLogger.Info(
                    "To find it, read here: https://docs.unity3d.com/ScriptReference/Application-persistentDataPath.html");
                CustomLogger.Warn("No game content found, this is likely just a sample project.");


                // Logging Exceptions
                try
                {
                    throw new InvalidOperationException("This is a manually invoked exception");
                }
                catch (Exception ex)
                {
                    CustomLogger.Error(ex, "Hello this is a Message about the exception");
                }
            }

            durations.Add(DateTime.UtcNow - startTime);
        }

        CustomLogger.Error("End of game reached. Please restart.");
        Debug.Log("Duration: " + durations.Average(span => span.TotalMilliseconds) +
                  "ms. Most of this time goes into the Unity console. Writing to the file is super quick.");
    }
}