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
        public TextReader streamReader;
        public int position;
        public int lineNumber;

        char a = (char)0x25;
        public char GetCurrentChar => currentChar;
        public char GetPreviousChar => previousChar;

        public int GetLineNumber => lineNumber;

        public int GetPosition => position;

        public char GetEOFSign => EOF;

        public char EOF = '\uffff';
        private char currentChar;
        public char previousChar;

        public FileReader(TextReader reader)
        {
            streamReader = reader;
            currentChar = (char)streamReader.Read();
        }

        public void Next()
        {
            //if (streamReader.EndOfStream)
            //    currentChar = EOF;
            //else
            //{
                previousChar = currentChar;
                currentChar = (char)streamReader.Read();
                if (!CheckIfTwoCharNewLine())
                    if (!UpdateLocationIfNewLine())
                        position++;
            //}
        }


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
            // unnecessary:
            //UpdateLocationIfNewLine();
            //Next();
            //if (CheckIfTwoCharNewLine())
            //    Next();
        }
    }

}
