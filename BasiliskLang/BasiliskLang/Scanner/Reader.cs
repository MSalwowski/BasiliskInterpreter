using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasiliskLang
{
    public class Reader
    {
        private string source;
        private StreamReader streamReader;
        private int position = 0;
        private int lineNumber = 0;
        private char EOF = '\0'; // znak konca tekstu w stringa (?)

        public Reader(string source, InputType inputType)
        {
            // TODO: przenieść do abstrakcji dwa typy readerów - jeden z pliku drugi z inputu
            if(inputType == InputType.File)
            {
                this.source = source;
                streamReader = new StreamReader(source);
            }
            else if(inputType == InputType.String)
            {
                //strireader - bo to przekombinowane
                MemoryStream memoryStream = new MemoryStream();
                StreamWriter streamWriter = new StreamWriter(memoryStream);
                streamWriter.Write(source);
                streamWriter.Flush();
                memoryStream.Position = 0;
                streamReader = new StreamReader(memoryStream);
            }
                
        }

        // moze nie zwracac tylko trzymac biezacy znak w readerze?
        // obsluga znakow nowej linii
        public char GetNextChar()
        {
            if (streamReader.EndOfStream)
                return EOF;
            char currentChar;
            currentChar = (char)streamReader.Read();
            UpdateLocation(currentChar);
            // wyniesc obsluge komentarzy do leksera
            if (currentChar == '#')
            {
                while (currentChar != '\n')
                {
                    if (streamReader.EndOfStream)
                        return EOF;
                    currentChar = (char)streamReader.Read();
                    UpdateLocation(currentChar);
                }
                UpdateLocation(currentChar);
            }
            return currentChar;
        }

        public char PeekNextChar()
        {
            if (streamReader.EndOfStream)
                return EOF;
            char currentChar;
            currentChar = (char)streamReader.Peek();
            if (currentChar == '#')
            {
                while (currentChar != '\n')
                {
                    if (streamReader.EndOfStream)
                        return EOF;
                    currentChar = (char)streamReader.Peek();
                }
            }
            return currentChar;
        }
        
        private void UpdateLocation(char currentChar)
        {
            if (currentChar == '\n')
            {
                position = 0;
                lineNumber++;
            }
            else
                position++;
        }
        public int LineNumber { get { return position; } }
        public int Position { get { return lineNumber; } }
    }
}
