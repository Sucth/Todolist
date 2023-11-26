using System;
using System.IO;
using System.IO.Compression;

/// <summary>
/// Manages writing and manipulation of log files.
/// </summary>
public class LogWriter
{
    private string logFileName;

    /// <summary>
    /// Initializes an instance of LogWriter with a specified log file name.
    /// </summary>
    /// <param name="logFileName">Name of the log file.</param>
    public LogWriter(string logFileName)
    {
        this.logFileName = logFileName;
    }

    /// <summary>
    /// Records an action with the current date and time into the log file.
    /// </summary>
    /// <param name="action">Action to record in the log file.</param>
    public void LogAction(string action)
    {
        string logEntry = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss}: {action}";

        using (StreamWriter writer = File.AppendText(logFileName))
        {
            writer.WriteLine(logEntry);
        }
    }

    /// <summary>
    /// Compresses the daily log file into a ZIP file.
    /// </summary>
    public void ZipLogForDay()
    {
        DateTime date = DateTime.Now;
        string logFilePath = $"..\\bin\\Release\\net7.0\\LogFilePath\\Todolist_log.log";
        string zipFilePath = $"..\\bin\\Release\\net7.0\\LogFilePath\\ToDoList_log_{date:yyyy-MM-dd}.zip";

        if (File.Exists(logFilePath))
        {
            using (FileStream fileStream = new FileStream(zipFilePath, FileMode.Create))
            {
                using (ZipArchive archive = new ZipArchive(fileStream, ZipArchiveMode.Create))
                {
                    archive.CreateEntryFromFile(logFilePath, Path.GetFileName(logFilePath));
                }
            }
        }
        else
        {
            Console.WriteLine("The daily log file does not exist: " + logFilePath);
        }
    }

    /// <summary>
    /// Displays the content of the log file if it exists.
    /// </summary>
    public void PrintLog()
    {
        string filePath = "..\\bin\\Release\\net7.0\\LogFilePath\\Todolist_log.log";
        try
        {
            if (File.Exists(filePath))
            {
                string fileContent = File.ReadAllText(filePath);
                Console.WriteLine("File Content:");
                Console.WriteLine(fileContent);
            }
            else
            {
                Console.WriteLine("The file does not exist.");
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("An error occurred: " + e.Message);
        }
    }
}
