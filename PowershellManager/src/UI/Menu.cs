using System;
using System.Collections.Generic;
using ToolBox;
using PowerShellManager.Core.Interfaces;

namespace PowerShellManager.UI
{
    public class Menu
    {
        private readonly IScriptService _scriptService;

        public Menu(IScriptService scriptService)
        {
            _scriptService = scriptService;
            Console.CursorVisible = true;
        }

        public void DisplayMainMenu()
        {
            Console.CursorVisible=false;
            _scriptService.RefreshScripts();
            DisplayWelcomeMessage();

            while (true)
            {
                var selection = DisplayMenu();

                switch (selection)
                {
                    case "Run Script":
                        List<Choice> scriptChoices = GetScriptChoices();

                        if (scriptChoices.Count > 0)
                        {
                            HandleScriptSelection(scriptChoices);
                        }
                        break;

                    case "Refresh":
                        _scriptService.RefreshScripts();
                        Console.WriteLine("Scripts have been refreshed.");
                        Console.WriteLine("\nPress any key to continue");
                        Console.ReadKey();
                        break;

                    case "Open Scripts Folder":
                        _scriptService.OpenScriptsFolder();
                        Console.WriteLine("The folder is now opening ");
                        Console.WriteLine("\nPress any key to continue");
                        Console.ReadKey();
                        break;

                    case "Exit":
                        return;

                    default:
                        Console.WriteLine("Invalid selection. Please try again.");
                        break;
                }

               
                Console.Clear();
                DisplayWelcomeMessage(); // Keep the welcome message visible after clearing the console
            }
        }

        private void DisplayWelcomeMessage()
        {
            Console.WriteLine("\r\n\r\n ________  ________  ___       __   _______   ________  ________  ___  ___  _______   ___       ___          \r\n|\\   __  \\|\\   __  \\|\\  \\     |\\  \\|\\  ___ \\ |\\   __  \\|\\   ____\\|\\  \\|\\  \\|\\  ___ \\ |\\  \\     |\\  \\         \r\n\\ \\  \\|\\  \\ \\  \\|\\  \\ \\  \\    \\ \\  \\ \\   __/|\\ \\  \\|\\  \\ \\  \\___|\\ \\  \\\\\\  \\ \\   __/|\\ \\  \\    \\ \\  \\        \r\n \\ \\   ____\\ \\  \\\\\\  \\ \\  \\  __\\ \\  \\ \\  \\_|/_\\ \\   _  _\\ \\_____  \\ \\   __  \\ \\  \\_|/_\\ \\  \\    \\ \\  \\       \r\n  \\ \\  \\___|\\ \\  \\\\\\  \\ \\  \\|\\__\\_\\  \\ \\  \\_|\\ \\ \\  \\\\  \\\\|____|\\  \\ \\  \\ \\  \\ \\  \\_|\\ \\ \\  \\____\\ \\  \\____  \r\n   \\ \\__\\    \\ \\_______\\ \\____________\\ \\_______\\ \\__\\\\ _\\ ____\\_\\  \\ \\__\\ \\__\\ \\_______\\ \\_______\\ \\_______\\\r\n    \\|__|     \\|_______|\\|____________|\\|_______|\\|__|\\|__|\\_________\\|__|\\|__|\\|_______|\\|_______|\\|_______|\r\n                                                          \\|_________|                                       \r\n                                                                                                             \r\n                                                                                                             \r\n\r\n");
        }

        private string DisplayMenu()
        {
            var menu = new SelectionPrompt()
                .Title("PowerShell Script Manager Menu")
                .TitleColor(ConsoleColor.Green)
                .PageSize(10)
                .MoreChoicesText("(Use arrow keys to navigate)")
                .AddChoices(new[]
                {
                    new Choice("Run Script"),
                    new Choice("Refresh"),
                    new Choice("Open Scripts Folder"),
                    new Choice("Exit")
                })
                .ChoiceColor(ConsoleColor.Green)
                .ClearConsole(false);

            return menu.Prompt();
        }

        private List<Choice> GetScriptChoices()
        {
            List<string> scripts = _scriptService.GetAvailableScripts();

            // Convert scripts to a list of Choice objects
            List<Choice> scriptChoices = scripts.ConvertAll(script => new Choice(script, isSubChoice: true));

            return scriptChoices;
        }

        private void HandleScriptSelection(List<Choice> scriptChoices)
        {
            Console.Clear();
            DisplayWelcomeMessage();

            var scriptMenu = new SelectionPrompt()
                .Title("Select a script to run:")
                .TitleColor(ConsoleColor.Green)
                .PageSize(10)
                .AddChoices(scriptChoices)
                .AddChoices(new List<Choice> { new Choice("Exit") }) // Adding the Exit choice
                .ChoiceColor(ConsoleColor.Green)
                .ClearConsole(false);

            string selectedScript = scriptMenu.Prompt();

            // Check if user selected "Exit" or pressed "Escape"
            if (selectedScript == "Exit" || selectedScript == null)
            {
                return; // Return to the previous menu
            }

            // Run the selected script
            _scriptService.RunScript(selectedScript);
        }

    }
}
