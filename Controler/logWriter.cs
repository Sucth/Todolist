using System;
using System.IO;
using System.IO.Compression;

public class LogWriter
{
    private string logFileName;

    public LogWriter(string logFileName)
    {
        this.logFileName = logFileName;
    }

    public void LogAction(string action)
    {
        string logEntry = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss}: {action}";

        using (StreamWriter writer = File.AppendText(logFileName))
        {
            writer.WriteLine(logEntry);
        }
    }

    public void ZipLogForDay()
    {
        DateTime date = DateTime.Now;
        string logFilePath = $"C:\\Users\\theos\\Desktop\\Todolist\\LogFilePath\\Todolist_log.log";
        string zipFilePath = $"C:\\Users\\theos\\Desktop\\Todolist\\LogFilePath\\ToDoList_log_{date:yyyy-MM-dd}.zip";

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
            Console.WriteLine("Le fichier journal quotidien n'existe pas : " + logFilePath);
        }
    }


    public void PrintLog()
    {
        string filePath = "C:\\Users\\theos\\Desktop\\Todolist\\LogFilePath\\ToDoList_log.log";
        try
        {
            if (File.Exists(filePath))
            {
                string fileContent = File.ReadAllText(filePath);
                Console.WriteLine("Contenu du fichier :");
                Console.WriteLine(fileContent);
            }
            else
            {
                Console.WriteLine("Le fichier n'existe pas.");
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("Une erreur s'est produite : " + e.Message);
        }
    }
}