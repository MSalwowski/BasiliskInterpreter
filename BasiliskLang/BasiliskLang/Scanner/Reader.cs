using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasiliskLang
{
    public interface IReader
    {
        public int GetLineNumber { get; }
        public int GetPosition { get; }
        public char GetCurrentChar { get; }
        public char GetPreviousChar { get; }
        public char GetEOFSign { get; }
        public void Next();
        public bool CheckIfTwoCharNewLine();
        public bool UpdateLocationIfNewLine();
        public void SkipLineComment(char commentSign);
    }
    public class FileReader : IReader
    {
        public List<char> newLineChars = new List<char> { '\r', '\n', (char)0x25, (char)0x36 };
        public StreamReader streamReader;
        public int position;
        public int lineNumber;

        char a = (char)0x25;
        public char GetCurrentChar => currentChar;
        public char GetPreviousChar => previousChar;

        public int GetLineNumber => lineNumber;

        public int GetPosition => position;

        public char GetEOFSign => EOF;

        public char EOF = '\0'; // znak konca tekstu w stringa (?)
        private char currentChar;
        public char previousChar;

        public FileReader(StreamReader reader)
        {
            streamReader = reader;
            currentChar = (char)streamReader.Read();
        }

        public void Next()
        {
            if (streamReader.EndOfStream)
                currentChar = EOF;
            else
            {
                previousChar = currentChar;
                currentChar = (char)streamReader.Read();
                if (!CheckIfTwoCharNewLine())
                    if (!UpdateLocationIfNewLine())
                        position++;
            }
        }
        //public void UpdateLocation()
        //{  
        //    if (currentChar == '\r' || currentChar == '\n')
        //    {
        //        if (previousChar != currentChar && (previousChar == '\r' || previousChar == '\n')) { } //skip
        //        else
        //        {
        //            position = 0;
        //            lineNumber++;
        //        }
        //    }
        //    else
        //        position++;
        //}

        public bool CheckIfTwoCharNewLine()
        {
            if (previousChar == '\n' || previousChar == '\r')
                if (currentChar != previousChar && (previousChar == '\r' || previousChar == '\n'))
                    return true;
            return false;
        }
        public bool UpdateLocationIfNewLine()
        {
            if(newLineChars.Contains(currentChar))
            {
                position = 0;
                lineNumber++;
                return true;
            }
            return false;
        }
        public void SkipLineComment(char commentSign)
        {
            while (!newLineChars.Contains(currentChar) && currentChar != EOF)
                Next();
            UpdateLocationIfNewLine();
            Next();
            if (CheckIfTwoCharNewLine())
                Next();
        }
    }

    //public class InputReader : IReader
    //{
    //    public StringReader stringReader;
    //    public int position;
    //    public int lineNumber;
    //    public char GetCurrentChar => currentChar;
    //    public char GetPreviousChar => previousChar;


    //    public int GetLineNumber => lineNumber;

    //    public int GetPosition => position;
    //    public char GetEOFSign => EOF;

    //    public char EOF = '\0'; // znak konca tekstu w stringa (?)
    //    public char currentChar;
    //    public char previousChar;
    //    public InputReader(StringReader reader)
    //    {
    //        stringReader = reader;
    //        currentChar = (char)stringReader.Read();
    //    }
    //    public void Next()
    //    {
    //        if (currentChar == -1)
    //            currentChar = EOF;
    //        else
    //        {
    //            previousChar = currentChar;
    //            currentChar = (char)stringReader.Read();
    //            UpdateLocation();
    //        }
    //    }

    //    public void UpdateLocation()
    //    {
    //        if (currentChar == '\r' || currentChar == '\n')
    //        {
    //            if (previousChar != currentChar && (previousChar == '\r' || previousChar == '\n')) { } //skip
    //            else
    //            {
    //                position = 0;
    //                lineNumber++;
    //            }
    //        }
    //        else
    //            position++;
    //    }
    //}

}
