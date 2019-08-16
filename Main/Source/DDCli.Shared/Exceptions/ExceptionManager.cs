using System;
using System.Collections.Generic;
using System.Text;

namespace DDCli.Exceptions
{
    public static class ExceptionManager
    {
        public static void RaiseException(string message)
        {
            RaiseException(new Exception(message)) ;
        }
        public static void RaiseException(Exception ex)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("###########################");
            sb.AppendLine("###########################");
            sb.AppendLine("####      ERROR!      #####");
            sb.AppendLine("###########################");
            sb.AppendLine("###########################");
            sb.AppendLine("################# ------ >>");
            sb.AppendLine($"Error message: {ex.Message}");
            sb.AppendLine("################# << ------");
            Console.WriteLine(sb.ToString());
            throw ex;
        }
    }
}
