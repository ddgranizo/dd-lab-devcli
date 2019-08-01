using System;
using System.Collections.Generic;
using System.Text;

namespace DDCli.Models
{
    public class StoredCliData
    {
        public List<CommandAlias> CommandAlias { get; set; }

        public StoredCliData()
        {
            CommandAlias = new List<CommandAlias>();
        }
    }
}
