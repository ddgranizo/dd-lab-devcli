using System;
using System.Collections.Generic;
using System.Text;

namespace DDCli.Models
{
    public class RegisteredTemplate
    {
        public string TemplateName { get; set; }
        public string Description { get; }
        public string Path { get; set; }

        public RegisteredTemplate()
        {

        }

        public RegisteredTemplate(string path, string templateName, string description)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentException("message", nameof(path));
            }
            if (string.IsNullOrEmpty(templateName))
            {
                throw new ArgumentException("message", nameof(templateName));
            }

            TemplateName = templateName;
            Description = description;
            Path = path;
        }


    }
}
