using System;

namespace BasiliskLang
{
    public class RuntimeException : Exception
    {
        public RuntimeException(string message, int lineNumber, int position) :
            base("RUNTIME ERROR::Line: " + lineNumber + " Position: " + position + " " + message)
        { }
        public RuntimeException(string message) :
            base("RUNTIME ERROR:: " + message)
        { }
    }
}
