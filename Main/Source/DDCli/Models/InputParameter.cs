using System;
using System.Collections.Generic;
using System.Text;

namespace DDCli.Models
{
    public class InputParameter
    {

        public bool HasValue { get; set; }
        public string RawStringValue { get; set; }
        public InputParameter(string parameter, string rawStringValue)
        {
            if (string.IsNullOrEmpty(parameter))
            {
                throw new ArgumentException("message", nameof(parameter));
            }

            RawStringValue = rawStringValue;
            ParameterName = parameter;
            HasValue = !string.IsNullOrEmpty(rawStringValue);
        }

        public string ParameterName { get; }
    }
}
