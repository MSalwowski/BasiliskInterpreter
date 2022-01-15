using System;
using BasiliskLang.Tokens;

namespace BasiliskLang
{
    public class IntValue : SimpleExpression
    {
        public int value;
        public IntValue(Token _token) : base(NodeType.Int)
        {
            value = Convert.ToInt32(_token.value);
        }
    }
}
