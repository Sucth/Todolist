using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using ToDoList_delamort.Model;
using ToDoList_delamort.Views;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Timers;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using System.Reflection.Metadata;
using ToDoList_delamort.Controler;
using System.IO;

namespace ToDoList_delamort.Controller
{
    public class controllerCommandTask
    {
        private userController userController;
        private LogWriter logWriter;
        public controllerCommandTask() 
        {
            userController = new userController();
            logWriter = new LogWriter("..\\bin\\Release\\net7.0\\LogFilePath\\Todolist_log.log");
        }
        /// <summary>
        /// Adds a new task to the database with the specified details, including user assignment.
        /// <param name="command">The command string containing user name, task name, description, due date, and priority.</param>
        /// </summary>

        public void AddTask(string command)
        {
            string[] commandSplitted = command.Split(' ');

            if (commandSplitted.Length < 5)
            {
                Console.WriteLine("Invalid command format. Use: User Username Name Description Dueate Priority");
                return;
            }

            string username = commandSplitted[0];
            string nameCommand = commandSplitted[1];
            string descriptionTask = commandSplitted[2];
            if (!DateTime.TryParse(commandSplitted[3], out DateTime dateTask))
            {
                Console.WriteLine("Invalid date format. Use a valid date format,'dd-mm-yyyy'.");
                return;
            }
            if (!Enum.TryParse(commandSplitted[4], true, out Priority priorityTask))
            {
                Console.WriteLine("Invalid priority. Use a valid priority value.");
                return;
            }

            try
            {
                using (var _db = new ToDolistContext())
                {
                    var user = _db.Users.FirstOrDefault(u => u.Name == username);

                    if (user == null)
                    {
                        Console.WriteLine($"The user with name {username} does not exist.");
                        return;
                    }

                    var task = new Task
                    {
                        Name = nameCommand,
                        CreationDate = DateTime.Now,
                        DueDate = dateTask,
                        Description = descriptionTask,
                        Priority = priorityTask,
                        IsCompleted = false,
                        TaskUsers = new List<TaskUser> { new TaskUser { User = user } }
                    };
                    if (string.IsNullOrWhiteSpace(descriptionTask))
                    {
                        var timer = new System.Timers.Timer(60000);
                        timer.Elapsed += DisplayTasksView.TimerElapsedAdd;
                        timer.AutoReset = false;
                        timer.Start();
                    }
                    _db.Tasks.Add(task);
                    _db.SaveChanges();

                    Console.WriteLine("Task added successfully!");
                }
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    Console.WriteLine("Erreur : " + ex.InnerException.Message);
                }
                else
                {
                    Console.WriteLine("Erreur : " + ex.Message);
                }
            }
        }
        /// <summary>
        /// Updates an existing task in the database with the specified details.
        /// <param name="command">The command string containing task ID, new name, description, due date, and priority.</param>
        /// </summary>

        public void UpdateTask(string command)
        {
            string[] commandSplitted = command.Split(' ');

            if (commandSplitted.Length != 5)
            {
                Console.WriteLine("Invalid command format. Use: taskId name description dueDate priority");
                return;
            }

            if (!int.TryParse(commandSplitted[0], out int taskId))
            {
                Console.WriteLine("Invalid taskId format. Use a valid integer taskId.");
                return;
            }

            string nameCommand = commandSplitted[1];
            string descriptionTask = commandSplitted[2];

            if (!DateTime.TryParse(commandSplitted[3], out DateTime dateTask))
            {
                Console.WriteLine("Invalid dueDate format. Use a valid date format, e.g., 'yyyy-MM-dd HH:mm:ss'.");
                return;
            }

            if (!Enum.TryParse(commandSplitted[4], true, out Priority priorityTask))
            {
                Console.WriteLine("Invalid priority. Use a valid priority value.");
                return;
            }

            try
            {
                
                using (var _db = new ToDolistContext())
                {
                    var task = _db.Tasks.FirstOrDefault(t => t.Id == taskId);
                    if (task != null)
                    {
                        task.Name = nameCommand;
                        task.DueDate = dateTask;
                        task.Description = descriptionTask;
                        task.Priority = priorityTask;
                        _db.SaveChanges();
                        Console.WriteLine("Task updated!");
                    }
                    if (string.IsNullOrWhiteSpace(descriptionTask))
                    {
                        var timer = new System.Timers.Timer(60000);
                        timer.Elapsed += DisplayTasksView.TimerElapsedUpdate;
                        timer.AutoReset = false;
                        timer.Start();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }
        /// <summary>
        /// Deletes a task from the database using the specified task ID.
        /// <param name="command">The task ID to be deleted.</param>
        /// </summary>

        public void DeleteTask(string command)
        {
            if (!int.TryParse(command, out int taskId))
            {
                Console.WriteLine("Invalid taskId format. Use a valid integer taskId.");
                return;
            }

            try
            {
                using (var _db = new ToDolistContext())
                {
                    var task = _db.Tasks.FirstOrDefault(t => t.Id == taskId);
                    if (task != null)
                    {
                        _db.Tasks.Remove(task);
                        _db.SaveChanges();
                        Console.WriteLine("Task deleted!");
                    }
                    else
                    {
                        Console.WriteLine("Task not found.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }
    }
}
