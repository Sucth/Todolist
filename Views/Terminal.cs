using System;
using System.Collections.Generic;
using ToDoList_delamort.Controler;
using ToDoList_delamort.Controller;

namespace ToDoList_delamort.Views
{
    public class Terminal
    {
        private Dictionary<string, Action<string>> commandDictionary;
        private controllerCommandTask controllerCommand;
        private controllerFunctionTask controllerFunctionTask;
        private userController userController;
        private LogWriter logWriter;
        private DatabaseTaskExporter DatabaseTaskExporter;
        private DatabaseTaskImporter DatabaseTaskImporter;
        private DatabaseUserExporter DatabaseUserExporter;
        private DatabaseUserImporter DatabaseUserImporter;


        public Terminal()
        {
            DatabaseTaskExporter = new DatabaseTaskExporter();
            DatabaseTaskImporter = new DatabaseTaskImporter();
            DatabaseUserImporter = new DatabaseUserImporter();
            DatabaseUserExporter = new DatabaseUserExporter();
            controllerCommand = new controllerCommandTask();
            userController = new userController();
            controllerFunctionTask = new controllerFunctionTask();
            logWriter = new LogWriter("..\\LogFilePath\\Todolist_log.log");
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
                { "List", command => controllerFunctionTask.ListTasks()},
                { "Delete", command => controllerCommand.DeleteTask(command) },
                { "Complete", command => controllerFunctionTask.CompleteTask(command) },
                { "SortComplete", command => controllerFunctionTask.GetPercentageCompleted() },
                { "SortNotComplete", command => controllerFunctionTask.GetPercentageNotCompleted() },
                { "SortPriority", command => controllerFunctionTask.GetPercentageByPriority() },
                { "ListUser", command => DisplayTasksView.Displayuser() },
                { "AddUser", command => userController.CreateUser(command)},
                { "DeleteUser", command => userController.DeleteUser(command)},
                { "AllUserTask", command => userController.GetTasksByUser(command)},
                { "AllTaskUser", command => userController.GetUsersByTask(command)},
                { "NoTaskUser", command => userController.GetUsersWithoutTasks()},
                { "CommandTxt", command => controllerFunctionTask.ExecuteCommandTxt() },
                { "CreateTxt", command => controllerFunctionTask.CreateTxtFile() },
                { "EditTxt", command => controllerFunctionTask.EditTxtFile() },
                { "DeleteTxt", command => controllerFunctionTask.DeleteTxtFile() },
                { "ExportDbTask", command =>  DatabaseTaskExporter.ExportTasksToCSV(command)},
                { "ImportDbTask", command =>  DatabaseTaskImporter.ImportTasksFromCSV(command)},
                { "ExportDbUser", command =>  DatabaseUserExporter.ExportUserToCSV(command)},
                { "ImportDbUsser", command =>  DatabaseUserImporter.ImportUsersFromCSV(command)},
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

        public static void DisplayGuide()
        {
            Console.WriteLine("Available commands:");
            Console.WriteLine("Add <Task Name> <Description> <Priority> <End Task Date>");
            Console.WriteLine("Update <Number> <Task Name> <Description> <New Priority> <New DueDate>");
            Console.WriteLine("List Affiche la liste des tâches.");
            Console.WriteLine("Delete <Number Of task>");
            Console.WriteLine("Complete <Task Name>");
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
