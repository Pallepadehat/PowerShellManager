using System.Collections.Generic;

namespace PowerShellManager.Core.Interfaces
{
    public interface IScriptService
    {
        void RefreshScripts();
        List<string> GetAvailableScripts();
        void OpenScriptsFolder();
        void RunScript(string scriptName);
    }
}
