using System;
using System.Collections.Generic;
using ToDoList_delamort.Controller;

namespace ToDoList_delamort.Views
{
    public class Terminal
    {
        private Dictionary<string, Action<string>> commandDictionary;
        private ControllerCommand controllerCommand;

        public Terminal()
        {
            controllerCommand = new ControllerCommand();
            InitializeCommandDictionary();
        }

        public void Run()
        {
            while (true)
            {
                Console.WriteLine("=== Command Line ===");
                string command = Console.ReadLine();
                ExecuteCommand(command);
            }
        }

        public void InitializeCommandDictionary()
        {
            commandDictionary = new Dictionary<string, Action<string>>
            {
                { "Add", command => controllerCommand.AddTask(command) },
                { "Update", command => controllerCommand.UpdateTask(command) },
                { "List", command => controllerCommand.ListTasks(controllerCommand.GetTasks())},
                { "Delete", command => controllerCommand.DeleteTask(command) },
                { "Complete", command => controllerCommand.CompleteTask(command) },
                { "Filter1", command => controllerCommand.GetPercentageCompleted() },
                { "Filter2", command => controllerCommand.GetPercentageNotCompleted() },
                { "Filter3", command => controllerCommand.GetPercentageByPriority() },
                { "Help", command => DisplayGuide() },
                { "Exit", command => Environment.Exit(0) }
            };
        }

        public void ExecuteCommand(string command)
        {
            string[] parts = command.Split(' ');
            string commandName = parts[0];
            Console.WriteLine(commandName);

            if (commandDictionary.ContainsKey(commandName))
            {
                string commandArguments = string.Join(" ", parts, 1, parts.Length - 1);
                commandDictionary[commandName](commandArguments);
            }
            else
            {
                Console.WriteLine("Invalid command. Type 'Help' for a list of commands.");
            }
        }

        private void DisplayGuide()
        {
            Console.WriteLine("Available commands:");
            Console.WriteLine("Add <TaskName> <Description> <Priority> <DueDate>");
            Console.WriteLine("Update <TaskName> <Description> <NewPriority> <NewDueDate>");
            Console.WriteLine("List");
            Console.WriteLine("Delete <TaskName>");
            Console.WriteLine("DeletePriority <Priority>");
            Console.WriteLine("Complete <TaskName>");
            Console.WriteLine("Filter Completed/Incomplete");
            Console.WriteLine("Filter DueDate <Date>");
            Console.WriteLine("Filter Priority <Priority>");
            Console.WriteLine("Help");
            Console.WriteLine("Exit");
        }
    }
}
