using System;
using System.Collections.Generic;
using ToDoList_delamort.Controller;

namespace ToDoList_delamort.Views
{
    public class Terminal
    {
        private Dictionary<string, Action<string>> commandDictionary;
        private ControllerCommand controllerCommand;
        private LogWriter logWriter;

        public Terminal()
        {
            controllerCommand = new ControllerCommand();
            logWriter = new LogWriter("C:\\Users\\theos\\Desktop\\Todolist\\LogFilePath\\Todolist_log.log");
            InitializeCommandDictionary();
        }

        public void Run()
        {
            while (true)
            {
                Console.WriteLine("=== Command Line ===");
                string command = Console.ReadLine();
                logWriter.LogAction(command);
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
                { "SortComplete", command => controllerCommand.GetPercentageCompleted() },
                { "SorrtNotComplete", command => controllerCommand.GetPercentageNotCompleted() },
                { "SortPriority", command => controllerCommand.GetPercentageByPriority() },
                { "Log", command => logWriter.PrintLog() },
                { "ZipLog", command => logWriter.ZipLogForDay() },
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
            Console.WriteLine("Update <Number> <TaskName> <Description> <NewPriority> <NewDueDate>");
            Console.WriteLine("List Affiche la liste des tâches.");
            Console.WriteLine("Delete <Number Of task>");
            Console.WriteLine("Complete <TaskName>");
            Console.WriteLine("SortComplete Affiche le pourcentage de tâches complétées.");
            Console.WriteLine("SortNotComplete Affiche le pourcentage de tâches non complétées.");
            Console.WriteLine("SortPriority Affiche le pourcentage de tâches par priorité.");
            Console.WriteLine("Log Affiche le journal.");
            Console.WriteLine("ZipLog Compresse le journal d'une journée.");
            Console.WriteLine("Help Affiche ce guide.");
            Console.WriteLine("Exit Quitte l'application.");
        }
    }
}
