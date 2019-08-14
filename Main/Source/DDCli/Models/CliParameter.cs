using System;
using System.Collections.Generic;
using System.Text;

namespace DDCli.Models
{
    public class CliParameter
    {
        public string Key { get; set; }
        public string Value { get; set; }
        public bool IsEncrypted { get; set; }
        public bool IsAutoIncrement { get; set; }

        public CliParameter(string key, string value, bool isEncrypted = false, bool isAutoIncrement = false)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentException("message", nameof(key));
            }

            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentException("message", nameof(value));
            }

            Key = key;
            Value = value;
            IsEncrypted = isEncrypted;
            IsAutoIncrement = isAutoIncrement;
        }

        public CliParameter()
        {
        }
    }
}
