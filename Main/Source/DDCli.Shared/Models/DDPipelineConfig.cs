using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace DDCli.Models
{
    public class DDPipelineConfig
    {
        [JsonProperty("pipelineConstants")]
        public Dictionary<string,string> PipelineConstants { get; set; }
        [JsonProperty("commands")]
        public List<PipeLineCommandDefinition> Commands { get; set; }
    }


    public class PipeLineCommandDefinition
    {
        [JsonProperty("command")]
        public string Command { get; set; }

        [JsonProperty("consoleInputs")]
        public List<string> ConsoleInputs { get; set; }

        [JsonProperty("isDisabled")]
        public bool IsDisabled { get; set; }
    }

}
