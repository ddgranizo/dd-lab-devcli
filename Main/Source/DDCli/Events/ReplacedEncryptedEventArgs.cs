using System;
using System.Collections.Generic;
using System.Text;

namespace DDCli.Events
{
    public class ReplacedEncryptedEventArgs : EventArgs
    {
        public ReplacedEncryptedEventArgs(string encrypted)
        {
            Encrypted = encrypted;
        }

        public string Encrypted { get; }
    }
}
