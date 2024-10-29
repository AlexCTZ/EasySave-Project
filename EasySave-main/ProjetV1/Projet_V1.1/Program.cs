using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Json;
using Projet_V1._0;

class Program
{
    static void Main(string[] args)
    {
        Console.CancelKeyPress += Console_CancelKeyPress;
        BackupManager backupManager = new BackupManager(jsonSave.ReadSaveJson());
        Console.WriteLine("choose a language");
        int lang = Convert.ToInt32(Console.ReadLine());
        Console.WriteLine("Choose the log format");
        string logtype = Console.ReadLine();
        ILogs log;
        ILanguageStrings chaines;

        if (lang == 1)
        {
            chaines = new Langfr();
            backupManager.chaines = chaines;
        }
        else if (lang == 2)
        {
            chaines = new LangAn();
            backupManager.chaines = chaines;
        }
        else
        {
            Console.WriteLine("Invalid language choice. Defaulting to English.");
            chaines = new LangAn(); // Vous pouvez choisir un comportement par défaut.
            backupManager.chaines = chaines;
        }

        if (logtype == "XML")
        {
            log = new XmlLog();
            backupManager.log = log;
        }
        else if (logtype == "JSON")
        {
            log = new jsonLog();
            backupManager.log = log;
        }
        else
        {
            Console.WriteLine("Invalid format. JSON is default");
            log = new jsonLog();
            backupManager.log = log;
        }


        // Check if there are command-line arguments
        if (args.Length > 0)
        {
            Console.WriteLine("Command-line arguments:");

            // Display each command-line argument
            foreach (var arg in args)
            {
                if (arg.Contains("-"))
                {
                    // Handle ranges, e.g., 1-3
                    string[] range = arg.Split('-');
                    if (range.Length == 2 && int.TryParse(range[0], out int start) && int.TryParse(range[1], out int end))
                    {
                        for (int i = start; i <= end; i++)
                        {
                            backupManager.ExecuteBackup(new[] { i });
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Invalid range: {arg}");
                    }
                }
                else if (arg.Contains(";"))
                {
                    // Handle individual values separated by semicolon, e.g., 1;3
                    string[] values = arg.Split(';');

                    foreach (var value in values)
                    {
                        if (int.TryParse(value, out int backupNumber))
                        {
                            // Execute backup for each parsed number
                            backupManager.ExecuteBackup(new[] { backupNumber });
                        }
                        else
                        {
                            Console.WriteLine($"Invalid value: {value}");
                        }
                    }
                }
      
                else
                {
                    // Handle single value
                    if (int.TryParse(arg, out int backupNumber))
                    {
                        backupManager.ExecuteBackup(new[] { backupNumber });
                    }
                    else
                    {
                        Console.WriteLine($"Invalid argument: {arg}");
                    }
                }
            }
        }
        else
        {
            Console.WriteLine("No command-line arguments provided.");
        }

        while (true)
        {
            Console.WriteLine(chaines.create);
            Console.WriteLine(chaines.view);
            Console.WriteLine(chaines.edit);
            Console.WriteLine(chaines.del);
            Console.WriteLine(chaines.execone);
            Console.WriteLine(chaines.execall);
            Console.WriteLine(chaines.exit);
            Console.WriteLine("");

            string userInput = Console.ReadLine();

            switch (userInput)
            {
                case "1":
                    if (backupManager.backupJobs.Count >= 5)
                    {
                        Console.WriteLine(chaines.errornbbc);
                    }
                    else
                    {
                        backupManager.CreateBackupJob();
                    }
                    break;

                case "2":
                    backupManager.ViewBackupJobs();
                    break;

                case "3":
                    Console.WriteLine(chaines.selecedit);
                    if (int.TryParse(Console.ReadLine(), out int editIndex))
                    {
                        backupManager.EditBackupJob(editIndex);
                    }
                    break;

                case "4":
                    Console.WriteLine(chaines.selecdel);
                    if (int.TryParse(Console.ReadLine(), out int deleteIndex))
                    {
                        backupManager.DeleteBackupJob(deleteIndex);
                    }
                    break;

                case "5":
                    Console.WriteLine(chaines.selecexe);
                    if (int.TryParse(Console.ReadLine(), out int executeIndex))
                    {
                        backupManager.ExecuteBackup(new[] { executeIndex });
                    }
                    break;

                case "6":
                    backupManager.ExecuteAllBackups();
                    break;

                case "7":
                    Environment.Exit(0);
                    break;

                default:
                    Console.WriteLine(chaines.error);
                    break;
            }
        }
    }


    static void Console_CancelKeyPress(object sender, ConsoleCancelEventArgs e)
    {
        ILanguageStrings chaines;
        chaines = new LangAn();

        Console.WriteLine(chaines.blockcl);
        e.Cancel = true;

    }


}


