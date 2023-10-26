using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using ToDoList_delamort.Model;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
                    if (string.IsNullOrWhiteSpace(task.Description))
                    {
                        task.ReminderTime = DateTime.Now.AddMinutes(1);
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
    }
}
