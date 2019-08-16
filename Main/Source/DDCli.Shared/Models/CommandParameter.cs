using System;
using System.Collections.Generic;
using System.Text;

namespace DDCli.Models
{
    public class CommandParameter
    {
        public string ParameterName { get; set; }
        public bool ValueBool { get; set; }
        public int ValueInt { get; set; }
        public decimal ValueDecimal { get; set; }
        public string ValueString { get; set; }
        public Guid ValueGuid { get; set; }
        public object Value { get; set; }

        public CommandParameter(string parameterName)
        {
            ValueBool = true;
            ParameterName = parameterName ?? throw new ArgumentNullException(nameof(parameterName));
        }

        public CommandParameter(string parameterName, object value)
        {
            ParameterName = parameterName ?? throw new ArgumentNullException(nameof(parameterName));
            Value = value ?? throw new ArgumentNullException(nameof(value));
        }

        public CommandParameter(string parameterName, bool value)
        {
            ParameterName = parameterName ?? throw new ArgumentNullException(nameof(parameterName));
            ValueBool = value;
        }
        public CommandParameter(string parameterName, int value)
        {
            ParameterName = parameterName ?? throw new ArgumentNullException(nameof(parameterName));
            ValueInt = value;
        }

        public CommandParameter(string parameterName, string value)
        {
            ParameterName = parameterName ?? throw new ArgumentNullException(nameof(parameterName));
            ValueString = value ?? throw new ArgumentNullException(nameof(value));
        }

        public CommandParameter(string parameterName, Guid value)
        {
            ParameterName = parameterName ?? throw new ArgumentNullException(nameof(parameterName));
            ValueGuid = value;
        }

        public CommandParameter(string parameterName, decimal value)
        {
            ParameterName = parameterName ?? throw new ArgumentNullException(nameof(parameterName));
            ValueDecimal = value;
        }
    }
}
