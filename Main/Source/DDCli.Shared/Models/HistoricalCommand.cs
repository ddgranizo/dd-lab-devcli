using System;
using System.Collections.Generic;
using System.Text;

namespace DDCli.Models
{
    public class HistoricalCommand
    {
        public string Command { get; set; }
        public DateTime ExecutedOn { get; set; }
        public InputRequest InputRequest { get; }

        public HistoricalCommand()
        {

        }
        public HistoricalCommand(InputRequest inputRequest)
        {
            InputRequest = inputRequest ?? throw new ArgumentNullException(nameof(inputRequest));
            Command = string.Join(" ", inputRequest.Args);
        }
    }
}
