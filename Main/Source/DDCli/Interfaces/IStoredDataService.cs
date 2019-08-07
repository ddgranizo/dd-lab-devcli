using DDCli.Models;
using System.Collections.Generic;

namespace DDCli.Interfaces
{
    public interface IStoredDataService
    {


        /// <summary>
        /// Check if exists alias in the stored data
        /// </summary>
        /// <param name="alias"></param>
        /// <returns></returns>
        bool ExistsAlias(string alias);


        /// <summary>
        /// Add alias to the sotred data
        /// </summary>
        /// <param name="command"></param>
        /// <param name="alias"></param>
        void AddAlias(string command, string alias);



        /// <summary>
        /// Delete alias from the stored data
        /// </summary>
        /// <param name="alias"></param>
        void DeleteAlias(string alias);


        /// <summary>
        /// Get all registered alias in the stored data
        /// </summary>
        /// <returns></returns>
        List<string> GetAliasWithCommand();


        /// <summary>
        /// Get all registered alias in the stored data
        /// </summary>
        /// <returns></returns>
        List<string> GetAlias();
    }
}