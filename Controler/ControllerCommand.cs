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

namespace ToDoList_delamort.Controller
{
    public class ControllerCommand
    {
        /// <summary>
        /// Adds a new task to the database with the specified details, including user assignment.
        /// <param name="command">The command string containing user name, task name, description, due date, and priority.</param>
        /// </summary>

        public void AddTask(string command)
        {
            string[] commandSplitted = command.Split(' ');

            if (commandSplitted.Length < 5)
            {
                Console.WriteLine("Invalid command format. Use: username name description date priority");
                return;
            }

            string username = commandSplitted[0];
            string nameCommand = commandSplitted[1];
            string descriptionTask = commandSplitted[2];
            if (!DateTime.TryParse(commandSplitted[3], out DateTime dateTask))
            {
                Console.WriteLine("Invalid date format. Use a valid date format, e.g., 'yyyy-MM-dd'.");
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
                        Console.WriteLine($"L'utilisateur avec le nom {username} n'existe pas.");
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
                        timer.Elapsed += TimerElapsedAdd;
                        timer.AutoReset = false;
                        timer.Start();
                    }
                    _db.Tasks.Add(task);
                    _db.SaveChanges();

                    Console.WriteLine("Tâche ajoutée avec succès !");
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
            Console.WriteLine("0:" + commandSplitted[0] + "  1:" + commandSplitted[1]+ "  2:" + commandSplitted[2] + "  3:" + commandSplitted[3] + "  4:" + commandSplitted[4]);

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
                        timer.Elapsed += TimerElapsedUpdate;
                        timer.AutoReset = false;
                        timer.Start();
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
        /// <summary>
        /// Lists all tasks in the database.
        /// </summary>

        public void ListTasks()
        {
            List<Task> tasks = GetTasks();
            DisplayTasksView.DisplayTasks(tasks);
        }
        /// <summary>
        /// Retrieves a list of all tasks from the database.
        /// </summary>
        /// <returns>List of tasks.</returns>

        public List<Task> GetTasks()
        {
            using (var _db = new ToDolistContext())
            {
                return _db.GetTasksWithUsers();
            }
        }
        /// <summary>
        /// Retrieves a list of all tasks with associated users from the database.
        /// </summary>
        /// <returns>List of tasks with users.</returns>

        public List<Task> GetTasksWithUsers()
        {
            using (var _db = new ToDolistContext())
            {
                return _db.Tasks.Include(t => t.TaskUsers).ThenInclude(tu => tu.User).ToList();
            }
        }
        /// <summary>
        /// Marks a task as completed in the database using the specified task ID.
        /// <param name="command">The task ID to be marked as completed.</param>
        /// </summary>

        public void CompleteTask(string command)
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
                        task.IsCompleted = true;
                        _db.SaveChanges();
                        Console.WriteLine("Task marked as completed!");
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
        /// <summary>
        /// Calculates and displays the percentage of completed tasks in the database.
        /// </summary>

        public void GetPercentageCompleted()
        {
            Console.WriteLine("bite");    
            using (var _db = new ToDolistContext())
            {
                var totalTasks = _db.Tasks.Count();
                var completedTasks = _db.Tasks.Count(t => t.IsCompleted);
                double percentage = (double)completedTasks / totalTasks * 100;

                Console.WriteLine($"Pourcentage de tâches complétées : {percentage}%");
            }
        }
        /// <summary>
        /// Calculates and displays the percentage of non-completed tasks in the database.
        /// </summary>

        public void GetPercentageNotCompleted()
        {
            using (var _db = new ToDolistContext())
            {
                var totalTasks = _db.Tasks.Count();
                var notCompletedTasks = _db.Tasks.Count(t => !t.IsCompleted);
                double percentage = (double)notCompletedTasks / totalTasks * 100;

                Console.WriteLine($"Pourcentage de tâches non complétées : {percentage}%");
            }
        }
        /// <summary>
        /// Calculates and displays the percentage of tasks by priority in the database.
        /// </summary>

        public void GetPercentageByPriority()
        {
            using (var _db = new ToDolistContext())
            {
                var totalTasks = _db.Tasks.Count();
                var lowPriorityTasks = _db.Tasks.Count(t => t.Priority == Priority.Low);
                var mediumPriorityTasks = _db.Tasks.Count(t => t.Priority == Priority.Medium);
                var highPriorityTasks = _db.Tasks.Count(t => t.Priority == Priority.High);

                double percentageLow = (double)lowPriorityTasks / totalTasks * 100;
                double percentageMedium = (double)mediumPriorityTasks / totalTasks * 100;
                double percentageHigh = (double)highPriorityTasks / totalTasks * 100;

                Console.WriteLine($"Pourcentage de tâches à faible priorité : {percentageLow}%");
                Console.WriteLine($"Pourcentage de tâches à priorité moyenne : {percentageMedium}%");
                Console.WriteLine($"Pourcentage de tâches à haute priorité : {percentageHigh}%");
            }
        }
        /// <summary>
        /// Timer elapsed event handler for reminding the user to add a description to a task.
        /// </summary>

        private static void TimerElapsedAdd(object sender, ElapsedEventArgs e)
        {
            Console.WriteLine($"Rappel : Veuillez ajouter une description pour un de vos tache.");
        }
        /// <summary>
        /// Timer elapsed event handler for reminding the user to add a description when updating a task.
        /// </summary>

        private static void TimerElapsedUpdate(object sender, ElapsedEventArgs e)
        {
            Console.WriteLine($"Rappel : Vous avez modifier une de vos tache ssans mettre de description.");
        }
    }
}
