using System;
using System.Collections.Generic;
using System.Text;

namespace DDCli.Utilities
{
    public static class ExceptionInLine
    {
        public static T Run<T>(Func<T> func, Action<Exception> exceptionHandler)
        {
            T value = default(T);
            try
            {
                value = func();
            }
            catch (Exception ex)
            {
                exceptionHandler(ex);
            }
            return value;
        }
    }
}
