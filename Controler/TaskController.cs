using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoList_delamort.Model;


public class TaskController
{
    private ToDolistContext _db = InitDb.GetDb();


    public void UpdateTask(int taskId, string name, string description, DateTime dueDate, Priority priority)
    {
        using (var transaction = _db.Database.BeginTransaction())
        {
            try
            {
                var task = _db.Tasks.FirstOrDefault(t => t.Id == taskId);

                if (task != null)
                {
                    task.Name = name;
                    task.DueDate = dueDate;
                    task.Description = description;
                    task.Priority = priority;
                    task.ReminderTime = string.IsNullOrWhiteSpace(description) ? DateTime.Now.AddMinutes(1) : null;

                    _db.SaveChanges();
                    transaction.Commit();

                    Console.WriteLine("Task updated!");
                }
                else
                {
                    Console.WriteLine("Task not found.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                transaction.Rollback();
            }
        }
    }

    public void MarkTaskAsCompleted(int taskId)
    {
        using (var transaction = _db.Database.BeginTransaction())
        {
            try
            {
                var task = _db.Tasks.FirstOrDefault(t => t.Id == taskId);

                if (task != null)
                {
                    task.IsCompleted = true;
                    _db.SaveChanges();
                    transaction.Commit();
                    Console.WriteLine("Task marked as completed!");
                }
                else
                {
                    Console.WriteLine("Task not found.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                transaction.Rollback();
            }
        }
    }

    public void DeleteTask(int taskId)
    {
        using (var transaction = _db.Database.BeginTransaction())
        {
            try
            {
                var task = _db.Tasks.FirstOrDefault(t => t.Id == taskId);

                if (task != null)
                {
                    _db.Tasks.Remove(task);
                    _db.SaveChanges();
                    transaction.Commit();
                    Console.WriteLine("Task deleted!");
                }
                else
                {
                    Console.WriteLine("Task not found.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                transaction.Rollback();
            }
        }
    }

    public List<Task> GetTasks()
    {
        return _db.Tasks.ToList();
    }

    public double GetPercentageCompleted()
    {
        return _db.Tasks.Count(task => task.IsCompleted) / (double)_db.Tasks.Count() * 100;
    }

    public double GetPercentageNotCompleted()
    {
        return _db.Tasks.Count(task => !task.IsCompleted) / (double)_db.Tasks.Count() * 100;
    }

    public (double low, double medium, double high) GetPercentageByPriority()
    {
        var totalTasks = _db.Tasks.Count();
        var lowPriorityTasks = _db.Tasks.Count(task => task.Priority == Priority.Low);
        var mediumPriorityTasks = _db.Tasks.Count(task => task.Priority == Priority.Medium);
        var highPriorityTasks = _db.Tasks.Count(task => task.Priority == Priority.High);

        var percentageLow = (double)lowPriorityTasks / totalTasks * 100;
        var percentageMedium = (double)mediumPriorityTasks / totalTasks * 100;
        var percentageHigh = (double)highPriorityTasks / totalTasks * 100;

        return (percentageLow, percentageMedium, percentageHigh);
    }
}
