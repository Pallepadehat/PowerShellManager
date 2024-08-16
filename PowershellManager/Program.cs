using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Runspaces;

class Program
{
    private static readonly string scriptsDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "scripts");

    static void Main(string[] args)
    {
        while (true)
        {
            List<string> scripts = GetAvailableScripts();

            if (scripts.Count == 0)
            {
                Console.WriteLine("No PowerShell scripts found in the 'scripts' directory.");
                Console.WriteLine("Press any key to exit...");
                Console.ReadKey();
                return;
            }

            DisplayMenu(scripts);

            string? choice = Console.ReadLine();

            if (string.IsNullOrEmpty(choice))
            {
                Console.WriteLine("Invalid input. Please try again.");
                continue;
            }

            if (choice.ToLower() == "q")
            {
                return;
            }

            if (int.TryParse(choice, out int scriptIndex) && scriptIndex > 0 && scriptIndex <= scripts.Count)
            {
                RunPowerShellScript(scripts[scriptIndex - 1]);
            }
            else
            {
                Console.WriteLine("Invalid choice. Please try again.");
            }

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
            Console.Clear();
        }
    }

    static List<string> GetAvailableScripts()
    {
        Console.WriteLine($"Searching for scripts in: {scriptsDirectory}");

        if (!Directory.Exists(scriptsDirectory))
        {
            Console.WriteLine("Scripts directory does not exist. Creating it now.");
            Directory.CreateDirectory(scriptsDirectory);
        }

        var scripts = Directory.GetFiles(scriptsDirectory, "*.ps1")
                        .Select(Path.GetFileName)
                        .Where(name => name != null)
                        .ToList()!;

        Console.WriteLine($"Found {scripts.Count} script(s).");
        foreach (var script in scripts)
        {
            Console.WriteLine($"  - {script}");
        }

        return scripts;
    }

    static void DisplayMenu(List<string> scripts)
    {
        Console.WriteLine("Available PowerShell Scripts:");
        for (int i = 0; i < scripts.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {scripts[i]}");
        }
        Console.WriteLine("Q. Quit");
        Console.Write("Enter your choice: ");
    }

    static void RunPowerShellScript(string scriptName)
    {
        string scriptPath = Path.Combine(scriptsDirectory, scriptName);

        using (PowerShell ps = PowerShell.Create())
        {
            ps.AddScript(File.ReadAllText(scriptPath));

            Console.WriteLine($"Running PowerShell script: {scriptName}");

            try
            {
                var results = ps.Invoke();

                if (ps.HadErrors)
                {
                    Console.WriteLine("Errors occurred while running the script:");
                    foreach (var error in ps.Streams.Error)
                    {
                        Console.WriteLine(error.ToString());
                    }
                }
                else
                {
                    foreach (var result in results)
                    {
                        Console.WriteLine(result);
                    }
                    Console.WriteLine("PowerShell script execution completed.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }
}
