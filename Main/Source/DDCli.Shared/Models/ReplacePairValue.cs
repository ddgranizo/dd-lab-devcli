using System;
using System.Collections.Generic;
using System.Text;

namespace DDCli.Models
{
    public class ReplacePairValue
    {
        public ReplacePairValue(ReplacePair replacedPair, string value)
        {
            ReplacedPair = replacedPair 
                ?? throw new ArgumentNullException(nameof(replacedPair));
            Value = value;
        }

        public ReplacePair ReplacedPair { get; set; }
        public string Value { get; set; }
    }
}
