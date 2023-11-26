using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using ToDoList_delamort.Controler;
using ToDoList_delamort.Controller;

namespace ToDoList_delamort.Views
{
    public class DisplayTasksView
    {
        public static void DisplayTasks(List<Task> tasks)
        {
            Console.WriteLine("Tasks:");
            foreach (var task in tasks)
            {
                string status = task.IsCompleted ? "Completed" : "Not Completed";
                DisplayTaskDetails(task, status);
            }
            Console.WriteLine();
        }

        public static void DisplayTaskDetails(Task task, string status)
        {
            Console.Write($"Task ID: {task.Id}, Users: ");

            if (task.TaskUsers != null && task.TaskUsers.Any())
            {
                foreach (var taskUser in task.TaskUsers)
                {
                    Console.Write($"{taskUser.User?.Name}, ");
                }
            }
            else
            {
                Console.Write("N/A");
            }

            Console.WriteLine($"\nName: {task.Name}, Description: {task.Description}, Creation Date: {task.CreationDate}, Due Date: {task.DueDate:MM/dd/yyyy}, Priority: {task.Priority}, Status: {status}");
        }


        public static void DisplaySortedTasks(List<Task> tasks)
        {
            Console.WriteLine("Tasks:");
            foreach (var task in tasks)
            {
                string status = task.IsCompleted ? "Completed" : "Not Completed";
                Console.WriteLine($"Task ID: {task.Id}, Name: {task.Name}, Description: {task.Description}, Creation Date: {task.CreationDate}, Due Date: {task.DueDate:MM/dd/yyyy}, Priority: {task.Priority}, Status: {status}");
                Console.WriteLine();
            }
        }

        internal static void DisplayTasks(DbSet<Task> tasks)
        {
            throw new NotImplementedException();
        }

        public static void Displayuser()
        {
            List<User> allUsers = userController.GetAllUsers();

            if (allUsers != null)
            {
                foreach (var user in allUsers)
                {
                    Console.WriteLine($"ID: {user.Id}, Name: {user.Name}");
                }
            }
        }
        /// <summary>
        /// Timer elapsed event handler for reminding the user to add a description to a task.
        /// </summary>

        public static void TimerElapsedAdd(object sender, ElapsedEventArgs e)
        {
            Console.WriteLine($"Reminder: Please add a description for one of your tasks.");
        }
        /// <summary>
        /// Timer elapsed event handler for reminding the user to add a description when updating a task.
        /// </summary>
        public static void TimerElapsedUpdate(object sender, ElapsedEventArgs e)
        {
            Console.WriteLine($"Reminder: Please add a description for one of your tasks.");
        }
    }

}
