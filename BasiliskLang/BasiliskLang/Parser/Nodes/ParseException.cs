using System;

namespace BasiliskLang
{
    public class ParseException : Exception
    {
        public ParseException(string message, int lineNumber, int position) : 
            base("SYNTAX ERROR:: Line: " + lineNumber + " Position: " + position + " " + message)
        { }
        public ParseException(string message) :
            base("SYNTAX ERROR:: " + message)
        { }
    }
}
