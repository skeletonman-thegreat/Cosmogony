using System;
using System.IO;

public static class SimpleLogger
{
    private static readonly string logFilePath = "Debug.txt";

    public static void Log(string message)
    {
        // Ensure thread safety with lock if logging from multiple threads
        lock (logFilePath)
        {
            using (StreamWriter writer = new StreamWriter(logFilePath, true))
            {
                writer.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}");
            }
        }
    }
}
