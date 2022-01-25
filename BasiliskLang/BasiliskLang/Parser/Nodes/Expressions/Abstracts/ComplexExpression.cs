using BasiliskLang.Tokens;

namespace BasiliskLang
{
    public abstract class ComplexExpression : Expression
    {
        public OperatorType Operation {get;}
        public Expression Left => children[0] as Expression;
        public Expression Right => children[1] as Expression;
        public ComplexExpression(NodeType _type, Expression _left, Expression _right, Token token) : base(_type) 
        {
            children.Add(_left);
            Operation = ConvertTokenToOperatorType(token);
            children.Add(_right);
        }
        public OperatorType ConvertTokenToOperatorType(Token token)
        {
            if (token.type == TokenType.Plus)
                return OperatorType.Add;
            else if (token.type == TokenType.Minus)
                return OperatorType.Subtract;
            else if (token.type == TokenType.And)
                return OperatorType.And;
            else if (token.type == TokenType.Or)
                return OperatorType.Or;
            else if (token.type == TokenType.Multiply)
                return OperatorType.Multiply;
            else if (token.type == TokenType.Divide)
                return OperatorType.Divide;
            else if (token.type == TokenType.Equal)
                return OperatorType.Equal;
            else if (token.type == TokenType.NotEqual)
                return OperatorType.NotEqual;
            else if (token.type == TokenType.Greater)
                return OperatorType.Greater;
            else if (token.type == TokenType.GreaterEqual)
                return OperatorType.GreaterEqual;
            else if (token.type == TokenType.Less)
                return OperatorType.Less;
            else if (token.type == TokenType.LessEqual)
                return OperatorType.LessEqual;
            else
                throw new ParseException("Unnknown token", token.lineNumber, token.position);
        }

    }
}
