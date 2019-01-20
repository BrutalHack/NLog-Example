using System;
using UnityEngine;

public class WriteLogMessages : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        CustomLogger.Info("Application started.");
        Debug.Log("This is a normal Unity Debug.Log message for comparison.");
        CustomLogger.Info("Welcome to the NLog Sample Project. Our log is being written to Unitys persistentDataPath.");
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

        CustomLogger.Error("End of game reached. Please restart.");
    }
}