using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace DDCli.Models
{
    public class DDTemplateConfig
    {
        [JsonProperty("templateName")]
        public string TemplateName { get; set; }
        [JsonProperty("replacePairs")]
        public List<ReplacePair> ReplacePairs { get; set; }
        [JsonProperty("ignorePathPatterns")]
        public List<string> IgnorePathPatterns { get; set; }
    }

    
    public class ReplacePair
    {

        [JsonProperty("replaceDescription")]
        public string ReplaceDescription { get; set; }
        [JsonProperty("oldValue")]
        public string OldValue { get; set; }
        [JsonProperty("applyForDirectories")]
        public bool ApplyForDirectories { get; set; }
        [JsonProperty("applyForFileNames")]
        public bool ApplyForFileNames { get; set; }
        [JsonProperty("applyForFileContents")]
        public bool ApplyForFileContents { get; set; }
        [JsonProperty("applyForFilePattern")]
        public string ApplyForFilePattern { get; set; }
    }

}
