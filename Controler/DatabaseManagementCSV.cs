using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using CsvHelper;
using ToDoList_delamort.Model;

/// <summary>
/// This class manages the export of tasks from the database to a CSV file.
/// </summary>
public class DatabaseTaskExporter
{
    /// <summary>
    /// Exports tasks to a CSV file based on the provided name.
    /// </summary>
    /// <param name="name">Output file name.</param>
    public void ExportTasksToCSV(string name)
    {
        string filePath = "C:\\Users\\theos\\Desktop\\Todolist\\DB_CSV\\" + name;
        string cleanName = Regex.Replace(name, @"[^\w]", "");

        if (cleanName != name)
        {
            Console.WriteLine("Error In Name Format");
            return;
        }
        else if (name.Length > 20)
        {
            Console.WriteLine("Error Name To Long");
            return;
        }
        using (var dbContext = new ToDolistContext())
        {
            var tasks = dbContext.Tasks.ToList();

            using (var writer = new StreamWriter(filePath))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(tasks);
            }
        }
    }
}

/// <summary>
/// This class manages the export of users from the database to a CSV file.
/// </summary>
public class DatabaseUserExporter
{
    /// <summary>
    /// Exports users to a CSV file based on the provided name.
    /// </summary>
    /// <param name="name">Output file name.</param>
    public void ExportUserToCSV(string name)
    {
        string filePath = "C:\\Users\\theos\\Desktop\\Todolist\\DB_CSV\\" + name;
        string cleanName = Regex.Replace(name, @"[^\w]", "");

        if (cleanName != name)
        {
            Console.WriteLine("Error In Name Format");
            return;
        }else if (name.Length > 20) 
        {
            Console.WriteLine("Error Name To Long");
            return;
        }
        using (var dbContext = new ToDolistContext())
        {
            var users = dbContext.Users.ToList();

            using (var writer = new StreamWriter(filePath))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(users);
            }
        }
    }
}

/// <summary>
/// This class manages the import of tasks from a CSV file to the database.
/// </summary>
public class DatabaseTaskImporter
{
    /// <summary>
    /// Imports tasks from a CSV file into the database.
    /// </summary>
    /// <param name="filePath">Path of the CSV file.</param>
    public void ImportTasksFromCSV(string filePath)
    {
        try
        {
            using (var reader = new StreamReader(filePath))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                var tasksFromCsv = csv.GetRecords<TaskFromCsv>();

                using (var dbContext = new ToDolistContext())
                {
                    foreach (var taskData in tasksFromCsv)
                    {
                        var task = new Task
                        {
                            Name = taskData.Name,
                            Description = taskData.Description,
                            CreationDate = DateTime.Parse(taskData.CreationDate),
                            DueDate = DateTime.Parse(taskData.DueDate),
                            Priority = ParsePriority(taskData.Priority),
                            IsCompleted = bool.Parse(taskData.IsCompleted)
                        };

                        dbContext.Tasks.Add(task);
                    }

                    dbContext.SaveChanges();
                }
            }
        }
        catch (FileNotFoundException)
        {
            Console.WriteLine("Le fichier spécifié n'a pas été trouvé.");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Une erreur s'est produite lors de l'importation des tâches : " + ex.Message);
        }
    }
    /// <summary>
    /// Private method to parse priority from a string and convert it to an enumeration.
    /// </summary>
    /// <param name="priorityString">String representing priority.</param>
    /// <returns>The converted priority.</returns>
    private Priority ParsePriority(string priorityString)
    {
        if (Enum.TryParse(priorityString, true, out Priority priority))
        {
            return priority;
        }
        return Priority.Low;
    }
}

/// <summary>
/// This class manages the import of users from a CSV file to the database.
/// </summary>
public class DatabaseUserImporter
{
    /// <summary>
    /// Imports users from a CSV file into the database.
    /// </summary>
    /// <param name="filePath">Path of the CSV file.</param>
    public void ImportUsersFromCSV(string filePath)
    {
        using (var reader = new StreamReader(filePath))
        using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
        {
            var usersFromCsv = csv.GetRecords<UserFromCsv>();

            using (var dbContext = new ToDolistContext())
            {
                foreach (var userData in usersFromCsv)
                {
                    var user = new User
                    {
                        Name = userData.Name,
                    };

                    dbContext.Users.Add(user);
                }

                dbContext.SaveChanges();
            }
        }
    }
}