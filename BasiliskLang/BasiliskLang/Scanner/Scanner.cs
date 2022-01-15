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
        public IReader reader;
        public Dictionary<string, Func<int, int, string, Token>> tokensDefinitions = new Dictionary<string, Func<int, int, string, Token>>();
        public List<string> keywords = new List<string>();
        int currentTokenLineNumber;
        int currentTokenPosition;
        public Token currentToken { get; private set; }

        public Scanner(IReader reader)
        {
            this.reader = reader;
            InitializeDefinitions();
        }
        private void InitializeDefinitions()
        {
            #region signs
            tokensDefinitions.Add("+", (line, column, value) => new Token(TokenType.Plus, line, column));
            tokensDefinitions.Add("-", (line, column, value) => new Token(TokenType.Minus, line, column));
            tokensDefinitions.Add("*", (line, column, value) => new Token(TokenType.Multiply, line, column));
            tokensDefinitions.Add("/", (line, column, value) => new Token(TokenType.Divide, line, column));
            tokensDefinitions.Add("&&", (line, column, value) => new Token(TokenType.And, line, column));
            tokensDefinitions.Add("||", (line, column, value) => new Token(TokenType.Or, line, column));
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
            tokensDefinitions.Add("eof", (line, column, value) => new Token(TokenType.EOF, line, column));
            #endregion
            #region types
            tokensDefinitions.Add("int", (line, column, value) => new Token(TokenType.Int, line, column, value));
            tokensDefinitions.Add("double", (line, column, value) => new Token(TokenType.Double, line, column, value));
            tokensDefinitions.Add("string", (line, column, value) => new Token(TokenType.String, line, column, value));
            tokensDefinitions.Add("datetime", (line, column, value) => new Token(TokenType.DateTime, line, column, value));
            tokensDefinitions.Add("period", (line, column, value) => new Token(TokenType.Period, line, column, value));
            tokensDefinitions.Add("identifier", (line, column, value) => new Token(TokenType.Identifier, line, column, value));
            tokensDefinitions.Add("invalid", (line, column, value) => new Token(TokenType.Invalid, line, column, value));
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

        public void NextToken()
        {
            while(Char.IsWhiteSpace(reader.GetCurrentChar) || reader.GetCurrentChar == '#')
            {
                if (reader.GetCurrentChar == '#')
                    reader.SkipLineComment('#');
                else
                    reader.Next();
            }
            currentTokenLineNumber = reader.GetLineNumber;
            currentTokenPosition = reader.GetPosition;
            if (reader.GetCurrentChar == reader.GetEOFSign)
                BuildEOFToken();
            else
                BuildNextToken();
        }
        public void BuildNextToken()
        {
            if (BuildAlphaToken())
                return;
            if (BuildNumberToken())
                return;
            if (BuildSpecialToken())
                return;
            if (BuildStringToken())
                return;
            BuildInvalidToken();
        }
        public bool BuildAlphaToken() {
            if (Char.IsLetter(reader.GetCurrentChar) || reader.GetCurrentChar == '_' || reader.GetCurrentChar == '$')
            {
                StringBuilder token = new StringBuilder();
                do
                {
                    token.Append(reader.GetCurrentChar);
                    reader.Next();
                }
                while (Char.IsLetterOrDigit(reader.GetCurrentChar) || reader.GetCurrentChar == '_' || reader.GetCurrentChar == '$');
                if (keywords.Contains(token.ToString()))
                {
                    // keywords
                    currentToken = tokensDefinitions[token.ToString()].Invoke(currentTokenLineNumber, currentTokenPosition, null);
                    return true;
                }
                else 
                {
                    // identifier
                    currentToken = tokensDefinitions["identifier"].Invoke(currentTokenLineNumber, currentTokenPosition, token.ToString());
                    return true;
                }
            }
            return false;
        }
        public bool BuildNumberToken() 
        {
            StringBuilder token = new StringBuilder();
            //#region sign
            //if (reader.GetCurrentChar == '-')
            //{
            //    token.Append(reader.GetCurrentChar);
            //    reader.Next();
            //}
            //#endregion
            // sign will be deducted in parser
            #region integer
            if (reader.GetCurrentChar == '0')
            {
                token.Append(reader.GetCurrentChar);
                reader.Next();
            }
            else if (Char.IsDigit(reader.GetCurrentChar))
            {
                token.Append(reader.GetCurrentChar);
                reader.Next();
                while(Char.IsDigit(reader.GetCurrentChar))
                {
                    token.Append(reader.GetCurrentChar);
                    reader.Next();
                }
            }
            #endregion
            #region fraction
            if(reader.GetCurrentChar == '.')
            {
                token.Append(reader.GetCurrentChar);
                reader.Next();
                while (Char.IsDigit(reader.GetCurrentChar))
                {
                    token.Append(reader.GetCurrentChar);
                    reader.Next();
                }
            }
            #endregion
            #region exponent
            if(reader.GetCurrentChar == 'e' || reader.GetCurrentChar == 'E')
            {
                token.Append(reader.GetCurrentChar);
                reader.Next();
                if(reader.GetCurrentChar == '-' || reader.GetCurrentChar == '+')
                {
                    token.Append(reader.GetCurrentChar);
                    reader.Next();
                }
                while (Char.IsDigit(reader.GetCurrentChar))
                {
                    token.Append(reader.GetCurrentChar);
                    reader.Next();
                }
            }
            #endregion
            if(token.Length > 0)
            {
                // creating unary minus caused problems, so its necessary to create it right here
                //if (token.Length == 1)
                //    if(token[0] == '-')
                //    {
                //        currentToken = tokensDefinitions["-"].Invoke(currentTokenLineNumber, currentTokenPosition, null);
                //        return true;
                //    }
                if (token.ToString().Contains('.'))
                    currentToken = tokensDefinitions["double"].Invoke(currentTokenLineNumber, currentTokenPosition, token.ToString());
                else
                    currentToken = tokensDefinitions["int"].Invoke(currentTokenLineNumber, currentTokenPosition, token.ToString());
                return true;
            }
            return false;
        }
        public bool BuildSpecialToken() {
            StringBuilder token = new StringBuilder();
            token.Append(reader.GetCurrentChar);
            if(tokensDefinitions.ContainsKey(token.ToString()) || reader.GetCurrentChar == '!' || reader.GetCurrentChar == '&' || reader.GetCurrentChar == '|')
            {
                reader.Next();
                token.Append(reader.GetCurrentChar);
                if(tokensDefinitions.ContainsKey(token.ToString()))
                {
                    // two-sign tokens
                    currentToken = tokensDefinitions[token.ToString()].Invoke(currentTokenLineNumber, currentTokenPosition, null);
                    reader.Next();
                    return true;
                }
                else
                {
                    // one-sign tokens
                    // TODO: how to handle taken char???
                    if(token[0] == '!' || token[0] == '&' || token[0] == '|')
                    {
                        currentToken = tokensDefinitions["invalid"].Invoke(currentTokenLineNumber, currentTokenPosition, token.ToString());
                        reader.Next();
                        return true;
                    }
                    else
                    {
                        token.Length--;
                        currentToken = tokensDefinitions[token.ToString()].Invoke(currentTokenLineNumber, currentTokenPosition, null);
                        return true;
                    }
                }
            }
            return false;
        }
        public bool BuildStringToken()
        {
            if(reader.GetCurrentChar == '"')
            {
                StringBuilder token = new StringBuilder();
                reader.Next();
                while(reader.GetCurrentChar != '"')
                {
                    if (reader.GetCurrentChar == '\\')
                    {
                        reader.Next();
                        switch (reader.GetCurrentChar)
                        {
                            case 'n':
                                token.Append('\n');
                                break;
                            case 'b':
                                token.Append('\b');
                                break;
                            case 'f':
                                token.Append('\f');
                                break;
                            case 'r':
                                token.Append('\r');
                                break;
                            case 't':
                                token.Append('\t');
                                break;
                            case '"':
                                token.Append('"');
                                break;
                            case '\\':
                                token.Append('\\');
                                break;
                            default:
                                currentToken = tokensDefinitions["invalid"].Invoke(currentTokenLineNumber, currentTokenPosition, token.ToString());
                                reader.Next();
                                return true;
                        }
                        reader.Next();
                    }
                    else if (reader.GetCurrentChar == '\n' || reader.GetCurrentChar == '\r' || reader.GetCurrentChar == reader.GetEOFSign)
                    {
                        currentToken = tokensDefinitions["invalid"].Invoke(currentTokenLineNumber, currentTokenPosition, token.ToString());
                        reader.Next();
                        return true;
                    }
                    else
                    {
                        token.Append(reader.GetCurrentChar);
                        reader.Next();
                    }
                    
                }
                reader.Next();
                currentToken = tokensDefinitions["string"].Invoke(currentTokenLineNumber, currentTokenPosition, token.ToString());
                return true;
            }
            return false;
        }
        public void BuildInvalidToken() { currentToken = tokensDefinitions["invalid"].Invoke(currentTokenLineNumber, currentTokenPosition, reader.GetCurrentChar.ToString()); reader.Next(); }
        public void BuildEOFToken() {
            currentToken = tokensDefinitions["eof"].Invoke(currentTokenLineNumber, currentTokenPosition, null);
        }
    }
}