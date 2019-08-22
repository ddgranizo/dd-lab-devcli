using System;
using System.Collections.Generic;
using System.Text;

namespace DDCli.Models
{
    public class RegisteredPipeline
    {
        public string PipelineName { get; set; }
        public string Description { get; }
        public string Path { get; set; }

        public RegisteredPipeline()
        {

        }

        public RegisteredPipeline(string path, string pipelineName, string description)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentException("message", nameof(path));
            }
            if (string.IsNullOrEmpty(pipelineName))
            {
                throw new ArgumentException("message", nameof(pipelineName));
            }

            PipelineName = pipelineName;
            Description = description;
            Path = path;
        }


    }
}
