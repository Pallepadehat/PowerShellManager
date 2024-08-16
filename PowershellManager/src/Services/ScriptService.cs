using System;
using System.Collections.Generic;
using System.IO;
using System.Management.Automation;
using PowerShellManager.Core.Interfaces;

namespace PowerShellManager.Services
{
    public class ScriptService : IScriptService
    {
        private readonly string _scriptsDirectory;
        private List<string> _scripts;

        public ScriptService(string scriptsDirectory)
        {
            _scriptsDirectory = scriptsDirectory;
            _scripts = new List<string>();
        }

        public void RefreshScripts()
        {
            if (Directory.Exists(_scriptsDirectory))
            {
                _scripts = new List<string>(Directory.GetFiles(_scriptsDirectory, "*.ps1"));
            }
        }

        public List<string> GetAvailableScripts()
        {
            return _scripts;
        }

        public void OpenScriptsFolder()
        {
            System.Diagnostics.Process.Start("explorer.exe", _scriptsDirectory);
        }

        public void RunScript(string scriptName)
        {
            string scriptPath = Path.Combine(_scriptsDirectory, scriptName);

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
                        Console.WriteLine("\nPress any key to continue");
                        Console.ReadKey();
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
                        Console.WriteLine("\nPress any key to continue");
                        Console.ReadKey();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                }
            }
        }
    }
}
