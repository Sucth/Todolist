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

//Update 1 test2 test2 31/12/2023 Medium
namespace ToDoList_delamort.Controller
{
    public class ControllerCommand
    {
        public void AddTask(string command)
        {
            string[] commandSplitted = command.Split(' ');

            if (commandSplitted.Length < 4)
            {
                Console.WriteLine("Invalid command format. Use: name description date priority");
                return;
            }

            string nameCommand = commandSplitted[0];
            string descriptionTask = commandSplitted[1];
            if (!DateTime.TryParse(commandSplitted[2], out DateTime dateTask))
            {
                Console.WriteLine("Invalid date format. Use a valid date format, e.g., 'yyyy-MM-dd HH:mm:ss'.");
                return;
            }
            if (!Enum.TryParse(commandSplitted[3], true, out Priority priorityTask))
            {
                Console.WriteLine("Invalid priority. Use a valid priority value.");
                return;
            }

            try
            {
                using (var _db = new ToDolistContext())
                {
                    var task = new Task
                    {
                        Name = nameCommand,
                        CreationDate = DateTime.Now,
                        DueDate = dateTask,
                        Description = descriptionTask,
                        Priority = priorityTask,
                        IsCompleted = false
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
                Console.WriteLine("Erreur : " + ex.Message);
            }
        }

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

        public void ListTasks(List<Task> tasks)
        {
            DisplayTasksView.DisplayTasks(tasks);
        }

        public List<Task> GetTasks()
        {
            using (var _db = new ToDolistContext())
            {
                return _db.Tasks.ToList();
            }
        }


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

        private static void TimerElapsedAdd(object sender, ElapsedEventArgs e)
        {
            Console.WriteLine($"Rappel : Veuillez ajouter une description pour un de vos tache.");
        }
        private static void TimerElapsedUpdate(object sender, ElapsedEventArgs e)
        {
            Console.WriteLine($"Rappel : Vous avez modifier une de vos tache ssans mettre de description.");
        }

    }
}
