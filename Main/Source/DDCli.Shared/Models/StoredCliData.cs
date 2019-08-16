using System;
using System.Collections.Generic;
using System.Text;

namespace DDCli.Models
{
    public class StoredCliData
    {
        public List<CommandAlias> CommandAlias { get; set; }
        public List<CliParameter> Parameters { get; set; }
        public List<RegisteredTemplate> RegisteredTemplates { get; set; }

        public StoredCliData()
        {
            CommandAlias = new List<CommandAlias>();
            Parameters = new List<CliParameter>();
            RegisteredTemplates = new List<RegisteredTemplate>();
        }
    }
}
