using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using ToDoList_delamort.Controller;
using ToDoList_delamort.Model;
using ToDoList_delamort.Views;

namespace ToDoList_delamort.Controler
{
    public class controllerFunctionTask
    {
        private userController userController;
        private controllerCommandTask controllerCommandTask;
        private LogWriter logWriter;
        public controllerFunctionTask()
        {
            userController = new userController();
            controllerCommandTask = new controllerCommandTask();
            logWriter = new LogWriter("..\\bin\\Release\\net7.0\\LogFilePath\\Todolist_log.log");
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
            using (var _db = new ToDolistContext())
            {
                var totalTasks = _db.Tasks.Count();
                var completedTasks = _db.Tasks.Count(t => t.IsCompleted);
                double percentage = (double)completedTasks / totalTasks * 100;

                Console.WriteLine($"Percentage of tasks completed: {percentage}%");
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

                Console.WriteLine($"Percentage of uncompleted tasks: {percentage}%");
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

                Console.WriteLine($"Percentage of low priority tasks: {percentageLow}%");
                Console.WriteLine($"Percentage of medium priority tasks: {percentageMedium}%");
                Console.WriteLine($"Percentage of high priority tasks: {percentageHigh}%");
            }
        }
        public void ExecuteCommandTxt()
        {
            Console.WriteLine("Which file to run :\n");
            string name = Console.ReadLine();
            string filePath = "C:\\Users\\theos\\Desktop\\Todolist\\" + name;
            if (!File.Exists("C:\\Users\\theos\\Desktop\\Todolist\\" + name))
            {
                Console.WriteLine("File Doesn't Exist.");
                return;
            }

            string[] commandSplited;
            IEnumerable<string> lines = File.ReadLines(filePath);

            foreach (string line in lines)
            {
                commandSplited = line.Split(' ');
                Console.WriteLine(line);



                string commandArguments = string.Join(" ", commandSplited, 1, commandSplited.Length - 1);
                switch (commandSplited[0])
                {
                    case "Add":
                        controllerCommandTask.AddTask(commandArguments);
                        break;

                    case "Update":
                        controllerCommandTask.UpdateTask(commandArguments);
                        break;

                    case "List":
                        ListTasks();
                        break;

                    case "Delete":
                        controllerCommandTask.DeleteTask(commandArguments);
                        break;

                    case "Complete":
                        CompleteTask(commandArguments);
                        break;

                    case "SortComplete":
                        GetPercentageCompleted();
                        break;

                    case "SortNotComplete":
                        GetPercentageNotCompleted();
                        break;

                    case "SortPriority":
                        GetPercentageByPriority();
                        break;

                    case "ListUser":
                        DisplayTasksView.Displayuser();
                        break;

                    case "AddUser":
                        userController.CreateUser(commandArguments);
                        break;

                    case "DeleteUser":
                        userController.DeleteUser(commandArguments);
                        break;

                    case "AllUserTask":
                        userController.GetTasksByUser(commandArguments);
                        break;

                    case "AllTaskUser":
                        userController.GetUsersByTask(commandArguments);
                        break;

                    case "NoTaskUser":
                        userController.GetUsersWithoutTasks();
                        break;

                    case "Log":
                        logWriter.PrintLog();
                        break;

                    case "ZipLog":
                        logWriter.ZipLogForDay();
                        break;

                    case "Help":
                        Terminal.DisplayGuide();
                        break;

                    default:
                        Console.WriteLine("Unknown command. Type 'Help' for a list of available commands.");
                        break;
                }
            }
        }
        public void CreateTxtFile()
        {
            Console.WriteLine("Name For Files :");
            string name = Console.ReadLine();

            if (name.Contains(".txt"))
            {
                if (File.Exists("C:\\Users\\theos\\Desktop\\Todolist\\" + name))
                {
                    Console.WriteLine("File Doesn't Exist.");
                    return;
                }
                File.CreateText("C:\\Users\\theos\\Desktop\\Todolist\\" + name);
            }
            else
            {
                Console.WriteLine("Your file is not a text");
            }
        }
        public void EditTxtFile()
        {
            Console.WriteLine("File's Name :");
            string name = Console.ReadLine();

            if (!File.Exists("C:\\Users\\theos\\Desktop\\Todolist\\" + name))
            {
                Console.WriteLine("File Doesn't Exist.");
                return;
            }

            Console.WriteLine("Wrote :\n");
            string content = Console.ReadLine();

            using (StreamWriter sw = File.AppendText("C:\\Users\\theos\\Desktop\\Todolist\\" + name))
            {
                sw.Write(content);
            }
        }
        public void DeleteTxtFile()
        {
            Console.WriteLine("File's Name :");
            string name = Console.ReadLine();

            if (File.Exists("C:\\Users\\theos\\Desktop\\Todolist\\" + name))
            {
                File.Delete("C:\\Users\\theos\\Desktop\\Todolist\\" + name);
                Console.WriteLine("Fichier supprimé : " + name);
            }
            else
            {
                Console.WriteLine("File Doesn't Exist.");
            }
        }

    }
}
