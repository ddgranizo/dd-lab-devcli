namespace DDCli.Interfaces
{
    public interface IPromptCommandService
    {
        
        void OpenExplorer(string path);
        void Run(string workingDirectory, string fileName, string parameters, bool asRoot = false, bool async = false);
        void RunCommand(string command, bool async = false);
        void RunCommandConEmu(string command, bool async = false);
    }
}