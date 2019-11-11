using DDCli.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace DDCli.Exceptions
{
    public static class ExceptionManager
    {
        public static void RaiseException(ILoggerService loggerService, string message)
        {
            RaiseException(loggerService, new Exception(message)) ;
        }
        public static void RaiseException(ILoggerService loggerService, Exception ex)
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
            loggerService.Log(sb.ToString());
            throw ex;
        }
    }
}
