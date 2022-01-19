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

        public WritableToken WithValue(string _value)
        {
            this.value = _value;
            return this;
        }

        public new bool Equals(Token other)
        {
            if (this.type == other.type)
                return true;
            return false;
        }
    }
}
