﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace DDCli.Models
{

    public enum PipelineIterator
    {
        Number = 1,
        StringArray = 2,
    }

    public class DDPipelineConfig
    {
        [JsonProperty("name")]
        public string PipelineName { get; set; }
        [JsonProperty("constants")]
        public Dictionary<string, string> PipelineConstants { get; set; }
        [JsonProperty("variables")]
        public List<string> PipelineVariables { get; set; }
        [JsonProperty("commands")]
        public List<PipeLineCommandDefinition> Commands { get; set; }
    }


    public class PipeLineCommandDefinition
    {
        [JsonProperty("alias")]
        public string Alias { get; set; }
        [JsonProperty("command")]
        public string Command { get; set; }
        [JsonProperty("consoleInputs")]
        public List<string> ConsoleInputs { get; set; }
        [JsonProperty("isDisabled")]
        public bool IsDisabled { get; set; }
        [JsonProperty("isIteration")]
        public bool IsIteration { get; set; }
        [JsonProperty("iterationType")]
        public PipelineIterator IterationType { get; set; }
        [JsonProperty("fromValue")]
        public int IterationTypeNumberFromValue { get; set; }
        [JsonProperty("toValue")]
        public int IterationTypeNumberToValue { get; set; }
        [JsonProperty("values")]
        public List<string> IterationTypeStringArrayValues { get; set; }
  
    }

}
