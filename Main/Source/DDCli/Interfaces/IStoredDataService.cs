using DDCli.Models;

namespace DDCli.Interfaces
{
    public interface IStoredDataService
    {
        string GetStoredKey();


        bool ExistsAlias(string alias);

        void AddAlias(string command, string alias);


        void DeleteAlias(string alias);
    }
}