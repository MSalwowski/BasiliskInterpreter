﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasiliskLang.Helpers
{
    public interface IErrorHandler
    {
        void Error(string message, int line, int position, bool shouldContinue = false);
        void PrintError(string message, int line, int position);
    }
    class ErrorHandler : IErrorHandler
    {
        int errorsCount;
        public void Error(string message, int lineNumber, int position , bool shouldThrow = true)
        {
            errorsCount++;
            PrintError(message, lineNumber, position);
            if (!shouldThrow)
                return;
            else
                throw new Exception(message);
        }

        public void PrintError(string message, int lineNumber, int position)
        {
            Console.WriteLine("ERROR::line:" + lineNumber + " position: " + position + "::" + message);
        }
    }
}
