using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace ToDoList_delamort.Controler
{
    public class userController {
        /// <summary>
        /// Gets all tasks assigned to a user and prints the task names.
        /// <param name="username">The name of the user.</param>
        /// </summary>
        public void GetTasksByUser(string username)
        {
            string[] commandSplitted = username.Split(' ');

            if (commandSplitted.Length < 1)
            {
                Console.WriteLine("Invalid command format.");
                return;
            }
                try
            {
                using (var _db = new ToDolistContext())
                {
                    var user = _db.Users.FirstOrDefault(u => u.Name == username);

                    if (user != null)
                    {
                        var tasks = _db.Tasks.Where(t => t.TaskUsers.Any(tu => tu.UserId == user.Id)).ToList();

                        if (tasks.Any())
                        {
                            Console.WriteLine($"Tâches de l'utilisateur {username}:");
                            foreach (var task in tasks)
                            {
                                Console.WriteLine($"- {task.Name}");
                            }
                        }
                        else
                        {
                            Console.WriteLine($"L'utilisateur {username} n'a pas de tâches assignées.");
                        }
                    }
                    else
                    {
                        Console.WriteLine($"L'utilisateur avec le nom {username} n'a pas été trouvé.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur : " + ex.Message);
            }
        }
        /// <summary>
        /// Gets all users assigned to a task and prints their names.
        /// <param name="taskName">The name of the task.</param>
        /// </summary>
        public void GetUsersByTask(string taskName)
        {
            string[] commandSplitted = taskName.Split(' ');

            if (commandSplitted.Length < 1)
            {
                Console.WriteLine("Invalid command format.");
                return;
            }
            try
            {
                using (var _db = new ToDolistContext())
                {
                    var task = _db.Tasks.FirstOrDefault(t => t.Name == taskName);

                    if (task != null)
                    {
                        var users = task.TaskUsers.Select(tu => tu.User).ToList();

                        if (users.Any())
                        {
                            Console.WriteLine($"Utilisateurs assignés à la tâche {taskName}:");
                            foreach (var user in users)
                            {
                                Console.WriteLine($"- {user.Name}");
                            }
                        }
                        else
                        {
                            Console.WriteLine($"Aucun utilisateur n'est assigné à la tâche {taskName}.");
                        }
                    }
                    else
                    {
                        Console.WriteLine($"La tâche avec le nom {taskName} n'a pas été trouvée.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur : " + ex.Message);
            }
        }
        /// <summary>
        /// Gets users without any assigned tasks and prints their names.
        /// </summary>
        public void GetUsersWithoutTasks()
        {
            try
            {
                using (var _db = new ToDolistContext())
                {
                    var usersWithoutTasks = _db.Users.Where(u => !u.TaskUsers.Any()).ToList();

                    if (usersWithoutTasks.Any())
                    {
                        Console.WriteLine("Utilisateurs sans tâches assignées :");
                        foreach (var user in usersWithoutTasks)
                        {
                            Console.WriteLine($"- {user.Name}");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Tous les utilisateurs ont des tâches assignées.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur : " + ex.Message);
            }
        }
        /// <summary>
        /// Creates a new user with the specified name.
        /// <param name="name">The name of the new user.</param>
        /// </summary>
        public void CreateUser(string name)
        {
            try
            {
                using (var _db = new ToDolistContext())
                {
                    var user = new User
                    {
                        Name = name
                    };

                    _db.Users.Add(user);
                    _db.SaveChanges();

                    Console.WriteLine("Utilisateur créé avec succès !");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur : " + ex.Message);
            }
        }
        /// <summary>
        /// Deletes a user with the specified name.
        /// <param name="username">The name of the user to delete.</param>
        /// </summary>
        public void DeleteUser(string username)
        {
            try
            {
                using (var _db = new ToDolistContext())
                {
                    var user = _db.Users.FirstOrDefault(u => u.Name == username);

                    if (user != null)
                    {
                        _db.Users.Remove(user);
                        _db.SaveChanges();
                        Console.WriteLine("Utilisateur supprimé avec succès !");
                    }
                    else
                    {
                        Console.WriteLine($"L'utilisateur avec le nom {username} n'a pas été trouvé.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur : " + ex.Message);
            }
        }
        /// <summary>
        /// Removes the link between a user and a task using their IDs.
        /// <param name="userId">The ID of the user.</param>
        /// <param name="taskId">The ID of the task.</param>
        /// </summary>
        public void RemoveUserFromTask(int userId, int taskId)
        {
            try
            {
                using (var _db = new ToDolistContext())
                {
                    var taskUser = _db.TaskUsers
                        .FirstOrDefault(tu => tu.UserId == userId && tu.TaskId == taskId);

                    if (taskUser != null)
                    {
                        _db.TaskUsers.Remove(taskUser);
                        _db.SaveChanges();
                        Console.WriteLine("Lien entre l'utilisateur et la tâche supprimé avec succès !");
                    }
                    else
                    {
                        Console.WriteLine("Le lien entre l'utilisateur et la tâche n'a pas été trouvé.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur : " + ex.Message);
            }
        }
        /// <summary>
        /// Gets a list of all users in the database.
        /// </summary>
        /// <returns>List of all users.</returns>
        public static List<User> GetAllUsers()
        {
            try
            {
                using (var _db = new ToDolistContext())
                {
                    return _db.Users.ToList();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur : " + ex.Message);
                return null;
            }
        }
    }
}
