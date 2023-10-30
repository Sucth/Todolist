using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            Console.WriteLine($"Task ID: {task.Id}, Name: {task.Name}, Description: {task.Description}, Creation Date: {task.CreationDate}, Due Date: {task.DueDate:MM/dd/yyyy}, Priority: {task.Priority}, Status: {status}");
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
    }

}
