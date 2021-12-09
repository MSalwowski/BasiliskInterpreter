using BasiliskLang.Tokens;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasiliskLang
{
    public class Scanner
    {
        public Reader reader;
        public Dictionary<string, Func<int, int, string, Token>> tokensDefinitions = new Dictionary<string, Func<int, int, string, Token>>();
        public List<string> keywords = new List<string>();

        public Scanner(string source, InputType inputType)
        {
            this.reader = new Reader(source, inputType);
            InitializeDefinitions();
        }
        // TOMODIFY: only for presentation
        public List<Token> tokens = new List<Token>();
        public Token Scan()
        {
            ReadAllTokens();
            return null;
        }
        public void ReadAllTokens()
        {
            Token token;
            do
            {
                token = GetNextToken();
                if (token != null)
                {
                    tokens.Add(token);
                    token.PrintTokenInfo();
                }
            } while (token != null);
        }
        // ===============================
        private void InitializeDefinitions()
        {
            #region signs
            tokensDefinitions.Add("+", (line, column, value) => new Token(TokenType.Plus, line, column));
            tokensDefinitions.Add("-", (line, column, value) => new Token(TokenType.Minus, line, column));
            tokensDefinitions.Add("*", (line, column, value) => new Token(TokenType.Multiply, line, column));
            tokensDefinitions.Add("/", (line, column, value) => new Token(TokenType.Divide, line, column));
            tokensDefinitions.Add("(", (line, column, value) => new Token(TokenType.LeftParanthesis, line, column));
            tokensDefinitions.Add(")", (line, column, value) => new Token(TokenType.RightParanthesis, line, column));
            tokensDefinitions.Add("{", (line, column, value) => new Token(TokenType.LeftCurlyBracket, line, column));
            tokensDefinitions.Add("}", (line, column, value) => new Token(TokenType.RightCurlyBracket, line, column));
            tokensDefinitions.Add(",", (line, column, value) => new Token(TokenType.Comma, line, column));
            tokensDefinitions.Add(".", (line, column, value) => new Token(TokenType.Dot, line, column));
            tokensDefinitions.Add(":", (line, column, value) => new Token(TokenType.Colon, line, column));
            tokensDefinitions.Add("=", (line, column, value) => new Token(TokenType.Assign, line, column));
            tokensDefinitions.Add("==", (line, column, value) => new Token(TokenType.Equal, line, column));
            tokensDefinitions.Add("!=", (line, column, value) => new Token(TokenType.NotEqual, line, column));
            tokensDefinitions.Add("<", (line, column, value) => new Token(TokenType.Less, line, column));
            tokensDefinitions.Add("<=", (line, column, value) => new Token(TokenType.LessEqual, line, column));
            tokensDefinitions.Add(">", (line, column, value) => new Token(TokenType.Greater, line, column));
            tokensDefinitions.Add(">=", (line, column, value) => new Token(TokenType.GreaterEqual, line, column));
            #endregion
            #region types
            tokensDefinitions.Add("int", (line, column, value) => new Token(TokenType.Int, line, column, value));
            tokensDefinitions.Add("double", (line, column, value) => new Token(TokenType.Double, line, column, value));
            tokensDefinitions.Add("string", (line, column, value) => new Token(TokenType.String, line, column, value));
            tokensDefinitions.Add("datetime", (line, column, value) => new Token(TokenType.DateTime, line, column, value));
            tokensDefinitions.Add("period", (line, column, value) => new Token(TokenType.Period, line, column, value));
            #endregion
            #region keywords
            tokensDefinitions.Add("if", (line, column, value) => new Token(TokenType.If, line, column));
            tokensDefinitions.Add("else", (line, column, value) => new Token(TokenType.Else, line, column));
            tokensDefinitions.Add("def", (line, column, value) => new Token(TokenType.Define, line, column));
            tokensDefinitions.Add("while", (line, column, value) => new Token(TokenType.While, line, column));
            tokensDefinitions.Add("return", (line, column, value) => new Token(TokenType.Return, line, column));
            tokensDefinitions.Add("print", (line, column, value) => new Token(TokenType.Print, line, column));
            keywords.Add("if");
            keywords.Add("else");
            keywords.Add("def");
            keywords.Add("while");
            keywords.Add("return");
            keywords.Add("print");
            #endregion
        }

        private bool IsSpecialSign(char c)
        {
            // TOTHINK: sprawdzenie znaków (a może tak zostawić?)
            return true;
        }
        public Token GetNextToken()
        {
            char currentChar;
            int lineNumber, position;
            do
            {
                
                currentChar = reader.GetNextChar();
            }
            while (Char.IsWhiteSpace(currentChar));
            // TODO: obsluga konca pliku
            lineNumber = reader.LineNumber;
            position = reader.Position;
            return BuildToken(currentChar, lineNumber, position);
        }

        public Token BuildToken(char firstChar, int lineNumber, int position)
        {
            // podobnie jakw  przypadku readera przechowywac aktualny token, zeby zachowywal sie bardziej jak enumerator (patrz: reader)
            if(Char.IsLetter(firstChar) || firstChar == '_')
            {
                // TODO: obsluga alphy
                return BuildAlphaToken(firstChar, lineNumber, position);
            }
            else if(Char.IsDigit(firstChar))
            {
                // TOTHINK: co z nieważnymi zerami?
                return BuildNumberToken(firstChar, lineNumber, position);
            }
            else if(IsSpecialSign(firstChar))
            {
                return BuildSpecialSignToken(firstChar, lineNumber, position);
            }
            else
            {
                // TOTHINK: obsluga błędnego tokenu tutaj, lub w casie wyżej
            }
            return null; // invalid token
        }
        public Token BuildAlphaToken(char firstChar, int lineNumber, int position)
        {
            // todo:
            if (!Char.IsLetter(firstChar))
                return null;
            // ===
            StringBuilder tokenBuilder = new StringBuilder();
            tokenBuilder.Append(firstChar);
            char currentChar;
            //while(Char.IsDigit(reader.currentChar) || Char.IsLetter(reader.currentChar) || reader.currentChar == '_')
            //{
            //    tokenBuilder.Append(currentChar);
            //    reader.Next();
            //}    
            do
            {
                currentChar = reader.PeekNextChar();
                if (Char.IsDigit(currentChar) || Char.IsLetter(currentChar) || currentChar == '_') // TOTHINK: dodać specialsign, żeby identyfiaktory mogły mieć też inne znaki
                {
                    currentChar = reader.GetNextChar();
                    tokenBuilder.Append(currentChar);
                }
                else
                    break;
            }
            while (true); // <= lepiej byloby tu wsadzic warunek przerywajacy 
            Token token;
            if (tokensDefinitions.ContainsKey(tokenBuilder.ToString().ToLower())) // <= trygetvalue
                token = tokensDefinitions[tokenBuilder.ToString().ToLower()].Invoke(lineNumber, position, null);
            else
                token = tokensDefinitions["string"].Invoke(lineNumber, position, tokenBuilder.ToString());
            // TOTHINK: a co jesli nie pasuje?
            return token;
        }
        public Token BuildStringToken(char firstChar, int lineNumber, int position)
        {
            // TOTHINK: trochę dzika jest ta metoda, możnaby ją przemyśleć (akceptujemy wszystko? nawet entery?)
            StringBuilder tokenBuilder = new StringBuilder();
            tokenBuilder.Append(firstChar);
            char currentChar;
            do
            {
                // DISCLAIMER: nie ma operacji peek, bo chcemy zachłannie zebrać wszystko co jest, aż do znaku końca stringu
                //brak escapeu
                currentChar = reader.GetNextChar();
                if (currentChar == '\"')
                    break;
                else
                    tokenBuilder.Append(currentChar);
            }
            while (true);
            Token token = tokensDefinitions["string"].Invoke(lineNumber, position, tokenBuilder.ToString());
            return token;
        }
        public Token BuildSpecialSignToken(char firstChar, int lineNumber, int position)
        {
            StringBuilder tokenBuilder = new StringBuilder();
            tokenBuilder.Append(firstChar);
            switch (firstChar)
            {
                case '=':
                    {
                        // przygotwanie recept jako dictionary charow, on przejmuje obsluge w funkcji tego co jest ponizej
                        if(reader.PeekNextChar() == '=')
                        {
                            char nextChar = reader.GetNextChar();
                            tokenBuilder.Append(nextChar);
                            return tokensDefinitions[tokenBuilder.ToString()].Invoke(lineNumber, position, null);
                        }
                        else   
                            return tokensDefinitions[tokenBuilder.ToString()].Invoke(lineNumber, position, null);
                    }
                case '!':
                    {
                        if (reader.PeekNextChar() == '=')
                        {
                            char nextChar = reader.GetNextChar();
                            tokenBuilder.Append(nextChar);
                            return tokensDefinitions[tokenBuilder.ToString()].Invoke(lineNumber, position, null);
                        }
                        else
                            // TODO: obsługa niewłaściwego tokenu
                            return null; 
                    }
                case '<':
                    {
                        if (reader.PeekNextChar() == '=')
                        {
                            char nextChar = reader.GetNextChar();
                            tokenBuilder.Append(nextChar);
                            return tokensDefinitions[tokenBuilder.ToString()].Invoke(lineNumber, position, null);
                        }
                        else
                            return tokensDefinitions[tokenBuilder.ToString()].Invoke(lineNumber, position, null);
                    }
                case '>':
                    {
                        if (reader.PeekNextChar() == '=')
                        {
                            char nextChar = reader.GetNextChar();
                            tokenBuilder.Append(nextChar);
                            return tokensDefinitions[tokenBuilder.ToString()].Invoke(lineNumber, position, null);
                        }
                        else
                            return tokensDefinitions[tokenBuilder.ToString()].Invoke(lineNumber, position, null);
                    }
                case '\"':
                    {
                        return BuildStringToken(reader.GetNextChar(), lineNumber, position);
                    }
                default:
                    {
                        // TODO: obsługa nieznego symbolu
                        if (tokensDefinitions.ContainsKey(tokenBuilder.ToString()))
                            return tokensDefinitions[tokenBuilder.ToString()].Invoke(lineNumber, position, null);
                        return null;
                    }
            }    
        }
        
        public Token BuildNumberToken(char firstChar, int lineNumber, int position)
        {
            StringBuilder tokenBuilder = new StringBuilder();
            tokenBuilder.Append(firstChar);
            bool isDouble = false;
            char currentChar;
            do
            {
                currentChar = reader.PeekNextChar();
                if (Char.IsDigit(currentChar))
                {
                    currentChar = reader.GetNextChar();
                    tokenBuilder.Append(currentChar);
                }
                else if (currentChar == '.' && !isDouble)
                {
                    currentChar = reader.GetNextChar();
                    tokenBuilder.Append(currentChar);
                    isDouble = true;
                }
                else
                {
                    // TOTHINK: posiada już w sobie '.', a dostaje kolejną - co robić?
                    break;
                }
            }
            while (true);
            Token token = tokenBuilder.ToString().Contains('.') ? tokensDefinitions["double"].Invoke(lineNumber, position, tokenBuilder.ToString()) : tokensDefinitions["int"].Invoke(lineNumber, position, tokenBuilder.ToString());
            return token;
        }

        
        


    }
}