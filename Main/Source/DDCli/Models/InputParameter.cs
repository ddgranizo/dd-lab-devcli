using System;
using System.Collections.Generic;
using System.Text;

namespace DDCli.Models
{
    public class InputParameter
    {

        public bool HasValue { get; set; }
        public string RawStringValue { get; set; }
        public string ParameterName { get; }
        public bool IsShortCut { get; set; }

        public InputParameter(string parameter, string rawStringValue, bool isShortCut = false)
        {
            if (string.IsNullOrEmpty(parameter))
            {
                throw new ArgumentException("message", nameof(parameter));
            }

            RawStringValue = rawStringValue;
            IsShortCut = isShortCut;
            ParameterName = parameter;
            HasValue = !string.IsNullOrEmpty(rawStringValue);
        }

    }
}
