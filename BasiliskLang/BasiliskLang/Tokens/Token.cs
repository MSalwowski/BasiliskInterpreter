using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasiliskLang.Tokens
{
    public enum TokenType
    {
        #region signs
        Plus,
        Minus,
        Multiply,
        Divide,
        And,
        Or,
        LeftParanthesis,
        RightParanthesis,
        LeftCurlyBracket,
        RightCurlyBracket,
        Assign,
        Equal,
        NotEqual,
        Greater,
        GreaterEqual,
        Less,
        LessEqual,
        Colon,
        Comma,
        Dot,
        Quote,
        EOF,
        #endregion
        #region types
        Int,
        Double,
        String,
        DateTime,
        Period,
        Identifier,
        Invalid,
        #endregion
        #region keywords
        If,
        Else,
        While,
        Define,
        Return,
        Print
        #endregion
    }
    public class Token : IEquatable<Token>
    {
        // TODO: zmiana dostępności na private, public tylko dla demonstracji
        public TokenType type { get; set; }
        public int lineNumber { get; set; }
        public int position { get; set; }
        public string value { get; set; }
        // ===================================================================

        public Token(TokenType _type, int _lineNumber, int _postion, string _value = null)
        {
            this.type = _type;
            this.lineNumber = _lineNumber;
            position = _postion;
            value = _value;
        }
        // TODELETE: only for presentation
        public void PrintTokenInfo()
        {
            Console.WriteLine("line no: " + lineNumber + "\tcolumn no: " + position + "\ttype: " + type + (value != null ? "\t\tvalue:" + value : ""));
        }
        // ===============================
        public bool Equals(Token other)
        {
            if (this.type == other.type && this.value == other.value && this.lineNumber == other.lineNumber && this.position == other.position)
                return true;
            return false;
        }

    }
}
