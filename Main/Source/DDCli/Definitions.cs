using System;
using System.Collections.Generic;
using System.Text;

namespace DDCli
{
    public class Definitions
    {
        public const string PasswordOfuscator = "********";
        public static string[] AvailableTrueStrings = new string[] {
                "true",
                "yes",
                "1",
                "si" };
        public const string TemplateConfigFilename = "ddtemplate.json";
        public const string PipelineConfigFilename = "ddpipeline.json";
    }
}
