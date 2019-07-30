using System;

namespace DDCli_Installer
{
    class Program
    {

        private const string CliPath = @"C:\Users\daniel.diazg\MyRepos\dd-lab-devcli\Main\Source\DDCli\bin\Release\netcoreapp2.2\win10-x64";

        static void Main(string[] args)
        {
            SetPathEnvironment();
        }

        private static void SetPathEnvironment()
        {
            const string name = "PATH";
            string pathVar = System.Environment.GetEnvironmentVariable(name);
            string completePath = string.Format(";{0}", CliPath);
            if (!pathVar.Contains(completePath))
            {
                var value = string.Format("{0}{1}", pathVar,completePath);
                var target = EnvironmentVariableTarget.Machine;
                System.Environment.SetEnvironmentVariable(name, value, target);
            }
        }
    }
}
