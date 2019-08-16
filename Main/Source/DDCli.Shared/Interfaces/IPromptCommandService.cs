namespace DDCli.Interfaces
{
    public interface IPromptCommandService
    {
        
        void OpenExplorer(string path);
        void Run(string workingDirectory, string fileName, string parameters, bool asRoot = false, bool async = false);
        string RunCommand(string command, string filename = null, string workingDirectory = null);
        //void RunCommandConEmu(string command, bool async = false);
    }
}