using PowerShellManager.Services;
using PowerShellManager.UI;

namespace PowerShellManager
{
    class Program
    {
        static void Main(string[] args)
        {
            // Set up dependencies
            string scriptsDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "scripts");
            var scriptService = new ScriptService(scriptsDirectory);

            // Initialize and run the menu
            var menu = new Menu(scriptService);
            menu.DisplayMainMenu();
        }
    }
}
