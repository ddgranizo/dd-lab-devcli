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


        /// <summary>
        /// Get the command name behind an alias
        /// </summary>
        /// <returns></returns>
        string GetAliasedCommand(string alias);




        bool ExistsParameter(string parameter);

        void AddParameter(string key, string value, bool isEncrypted, bool isAutoIncrement);

        void DeleteParameter(string key);

        string GetParameterValue(string key);

        void UpdateParameter(string key, string newValue);

        List<string> GetParametersWithValues();

        List<CliParameter> GetParameters();

        void AddTemplate(string path, string templateName, string description);


        void DeleteTemplate(string templateName);

        List<string> GetTemplatesWithValues();

        List<RegisteredTemplate> GetTemplates();

        bool ExistsTemplate(string templateName);

        string GetTemplatePath(string templateName);

        void UpdateAutoIncrements(List<string> autoincrementParameters);

        bool ExistsPipeline(string pipelineName);

        void AddPipeline(string path, string pipelineName, string description);

        void DeletePipeline(string pipelineName);

        List<RegisteredPipeline> GetPipelines();

        string GetPipelinePath(string pipelineName);

        void AddCommandToHistorical(HistoricalCommand command);

        List<HistoricalCommand> GetCommandsFromHistorical(int count = 10);
    }
}