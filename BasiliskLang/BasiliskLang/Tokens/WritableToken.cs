using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasiliskLang.Tokens
{
    public class WritableToken : Token
    {
        public WritableToken(TokenType type) : base() { this.type = type; }

        //public WritableToken WithType(TokenType type)
        //{
        //    this.type = type;
        //    return this;
        //}

        public new bool Equals(Token other)
        {
            if (this.type == other.type)
                return true;
            return false;
        }
    }
}
