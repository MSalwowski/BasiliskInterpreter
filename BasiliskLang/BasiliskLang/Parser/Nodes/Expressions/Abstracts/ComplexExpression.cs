using BasiliskLang.Tokens;
using System.Collections.Generic;
using System.Linq;

namespace BasiliskLang
{
    public abstract class ComplexExpression : Expression
    {
        public List<OperatorType> Operations { get; }
        public List<Expression> Components => children.Cast<Expression>().ToList();
        public ComplexExpression(NodeType type, List<Expression> expressions, List<Token> tokens) : base(type) 
        {
            children.AddRange(expressions);
            Operations = new List<OperatorType>();
            foreach (var token in tokens)
                Operations.Add(ConvertTokenToOperatorType(token));
            if (Components.Count - 1 != Operations.Count)
                throw new ParseException("Number of operators doesn't match number of expressions");
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
