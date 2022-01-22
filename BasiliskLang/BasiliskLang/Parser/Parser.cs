using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BasiliskLang.Helpers;
using BasiliskLang.Tokens;

namespace BasiliskLang
{
    public class Parser
    {
        IScanner scanner;
        IErrorHandler errorHandler;
        public Parser(IScanner _scanner, IErrorHandler _errorHandler)
        {
            scanner = _scanner;
            errorHandler = _errorHandler;
        }
        public ProgramRoot Parse()
        {
            ProgramRoot programRoot = ParseProgram();
            return programRoot;
        }
        #region helpers
        public bool AssertTokenType(params TokenType[] shouldBe)
        {
            if (!shouldBe.Contains(scanner.currentToken.type))
                return false;
            return true;
        }
        public void AssertTokenTypeOrRaiseError(string message, bool shouldThrow, params TokenType[] shouldBe)
        {
            if (!shouldBe.Contains(scanner.currentToken.type))
                errorHandler.Error(message, scanner.currentToken.lineNumber, scanner.currentToken.position, shouldThrow);
                
        }
        public bool AssertNode(Node node)
        {
            if (node == null)
                return false;
            return true;
        }
        public void AssertNodeOrRaiseError(string message, bool shouldThrow, Node node)
        {
            if(node == null)
                errorHandler.Error(message, scanner.currentToken.lineNumber, scanner.currentToken.position, shouldThrow);
        }
        public void AssertAtLeastOneNodeOrRaiseError(string message, IEnumerable<Node> nodes)
        {
            if (!nodes.Any())
                errorHandler.Error(message, scanner.currentToken.lineNumber, scanner.currentToken.position, true);
        }
        #endregion
        #region grammar
        // program     				=   definitions, statements;
        public ProgramRoot ParseProgram()
        {
            Dictionary<(string,int),Definition> functionsDefinitions = ParseDefinitions();
            List<Statement> statements = ParseStatements();
            return new ProgramRoot(functionsDefinitions, statements);
        }
        // definitions 				=   definition, {definition};
        public Dictionary<(string, int), Definition> ParseDefinitions()
        {
            scanner.NextToken();
            Dictionary<(string, int), Definition> functionDefinitions = new Dictionary<(string, int), Definition>();
            Definition functionDefinition = ParseDefinition();
            while (functionDefinition != null)
            {
                functionDefinitions.Add((functionDefinition.Identifier.Name, functionDefinition.Parameters == null ? 0 : functionDefinition.Parameters.Count()), functionDefinition);
                scanner.NextToken();
                functionDefinition = ParseDefinition();
            }
            return functionDefinitions;
        }
        // definition  				=   "def", identifier, '(', parameters, ')', ':', block;
        public Definition ParseDefinition()
        {
            if (!AssertTokenType(TokenType.Define))
                return null;

            scanner.NextToken();
            AssertTokenTypeOrRaiseError("Expected identifier", true, TokenType.Identifier);
            Identifier identifier = new Identifier(scanner.currentToken.value);

            scanner.NextToken();
            AssertTokenTypeOrRaiseError("Expected left paranthesis", false, TokenType.LeftParanthesis);

            scanner.NextToken();
            List<Assignable> parameters = ParseParameters();

            scanner.NextToken();
            AssertTokenTypeOrRaiseError("Expected colon", false, TokenType.Colon);

            scanner.NextToken();
            BlockStatement blockStatement = ParseBlockStatement();

            Definition definition = new Definition(identifier, parameters, blockStatement);
            return definition;
        }
        // parameters  				=   [assignable, {',', assignable}];
        public List<Assignable> ParseParameters() 
        {
            List<Assignable> parameters = new List<Assignable>();
            Assignable parameter = ParseAssignable();

            if (!AssertNode(parameter))
                return null;
            parameters.Add(parameter);
            while (AssertTokenType(TokenType.Comma))
            {
                scanner.NextToken();
                parameter = ParseAssignable();
                AssertNodeOrRaiseError("Expected parameter", true, parameter);
                parameters.Add(parameter);
            }
            // if parameter == null na samym poczatku to moze nextToken?
            return parameters;
        }
        // assignable  				=   identifier, ['.', identifier]
        public Assignable ParseAssignable()
        {
            Identifier identifier = ParseIdentifier();
            if (!AssertNode(identifier))
                return null;
            Assignable assignable = new Assignable(identifier);
            scanner.NextToken();
            if (AssertTokenType(TokenType.Dot))
            {
                scanner.NextToken();
                Identifier property = ParseIdentifier();
                AssertNodeOrRaiseError("Expected identifier", true, property);
                scanner.NextToken();
                return new Assignable(identifier, property);
            }
            else
                return new Assignable(identifier);
        }
        // identifier
        public Identifier ParseIdentifier()
        {
            if(AssertTokenType(TokenType.Identifier))
                return new Identifier(scanner.currentToken.value);
            return null;
        }
        // statements					=   [statement, {statement}];
        public List<Statement> ParseStatements()
        {
            List<Statement> statements = new List<Statement>();
            Statement statement = ParseStatement();
            while (statement != null)
            {
                statements.Add(statement);
                statement = ParseStatement();
            }
            return statements;
        }
        // block       				=   '{', statements, '}';
        public BlockStatement ParseBlockStatement()
        {
            AssertTokenTypeOrRaiseError("Expected left curly bracket", false, TokenType.LeftCurlyBracket);
            scanner.NextToken();
            List<Statement> statements = ParseStatements();
            AssertAtLeastOneNodeOrRaiseError("Expected at least one statement in block statement", statements);
            AssertTokenTypeOrRaiseError("Expected right curly bracket", false, TokenType.RightCurlyBracket);
            return new BlockStatement(statements);
        }
        // statement 				=	if_statement
        //                              while_statement
        //                              return_statement
        //                              assignable, assignable_statement;
        public Statement ParseStatement()
        {
            Statement statement = ParseIfStatement();
            if (AssertNode(statement))
                return statement;
            statement = ParseWhileStatement();
            if (AssertNode(statement))
                return statement;
            statement = ParseReturnStatement();
            if (AssertNode(statement))
                return statement;
            statement = ParseAssignableStatement();
            if (AssertNode(statement))
                return statement;
            return null;
        }
        // assignable_statement     =   assign_statement
        //                              call;
        public Statement ParseAssignableStatement()
        {
            Assignable assignable = ParseAssignable();
            if (!AssertNode(assignable))
                return null;
            Statement statement = ParseAssignStatement(assignable);
            if (AssertNode(statement))
                return statement;
            statement = ParseFunctionCallStatement(assignable);
            AssertNodeOrRaiseError("Expected function call or assignement statement", true, statement);
            return statement;

        }

        // if_statement    			=   "if", '(', expression, ')', ':', block, ["else", ':', block];
        public IfStatement ParseIfStatement()
        {
            if (!AssertTokenType(TokenType.If))
                return null;

            scanner.NextToken();
            AssertTokenTypeOrRaiseError("Expected left paranthesis", false, TokenType.LeftParanthesis);

            scanner.NextToken();
            Expression condition = ParseExpression();
            AssertNodeOrRaiseError("Expected condition", true, condition);

            AssertTokenTypeOrRaiseError("Expected right paranthesis", false, TokenType.RightParanthesis);

            scanner.NextToken();
            AssertTokenTypeOrRaiseError("Expected colon", false, TokenType.Colon);

            scanner.NextToken();
            BlockStatement trueStatement = ParseBlockStatement();
            AssertNodeOrRaiseError("Expected at least one statement", true, trueStatement);

            scanner.NextToken();
            if (!AssertTokenType(TokenType.Else))
                return new IfStatement(condition, trueStatement);

            scanner.NextToken();
            AssertTokenTypeOrRaiseError("Expected colon", false, TokenType.Colon);

            scanner.NextToken();
            BlockStatement falseStatement = ParseBlockStatement();
            return new IfStatement(condition, trueStatement, falseStatement);
        }
        // while_statement 			=   "while", '(', expression, ')', block;
        public WhileStatement ParseWhileStatement()
        {
            if (!AssertTokenType(TokenType.While))
                return null;

            scanner.NextToken();
            AssertTokenTypeOrRaiseError("Expected left paranthesis", false, TokenType.LeftParanthesis);

            scanner.NextToken();
            Expression condition = ParseExpression();
            AssertNodeOrRaiseError("Expected condition", true, condition);
            
            AssertTokenTypeOrRaiseError("Expected right paranthesis", false, TokenType.RightParanthesis);

            scanner.NextToken();
            AssertTokenTypeOrRaiseError("Expected colon", false, TokenType.Colon);

            scanner.NextToken();
            BlockStatement blockStatement = ParseBlockStatement();
            return new WhileStatement(condition, blockStatement);
        }
        // return_statement			=	"return", expression;
        public ReturnStatement ParseReturnStatement()
        {
            if (!AssertTokenType(TokenType.Return))
                return null;
            scanner.NextToken();
            Expression expression = ParseExpression();
            return new ReturnStatement(expression);
        }
        // assign_statement			=   assignable, assign_operator, expression;
        public AssignStatement ParseAssignStatement(Assignable assignable) 
        {
            if (!AssertTokenType(TokenType.Assign))
                return null;
            scanner.NextToken();
            Expression expression = ParseExpression();
            AssertNodeOrRaiseError("Expected expression", true, expression);
            return new AssignStatement(assignable, expression);
        }
        // call        				=   assignable, '(', [arguments], ')';
        public FunctionCallStatement ParseFunctionCallStatement(Assignable assignable) 
        {
            if (!AssertTokenType(TokenType.LeftParanthesis))
                return null;
            scanner.NextToken();
            List<Expression> arguments = ParseArguments();
            AssertTokenTypeOrRaiseError("Expected right paranthesis", false, TokenType.RightParanthesis);
            scanner.NextToken();
            return new FunctionCallStatement(assignable, arguments);
        }
        // arguments   				=   expression, {',', expression};
        public List<Expression> ParseArguments()
        {
            Expression argument = ParseExpression();
            if (!AssertNode(argument))
                return null;
            List<Expression> arguments = new List<Expression>();
            arguments.Add(argument);
            while (AssertTokenType(TokenType.Comma))
            {
                scanner.NextToken();
                argument = ParseExpression();
                AssertNodeOrRaiseError("Expected argument", true, argument);
                arguments.Add(argument);
            }
            return arguments;
        }
        // expression  				=   logic_expression;
        public Expression ParseExpression()
        {
            return ParseLogicExpression();
        }
        // logic_expression	    	=   relation_expression, {logic_operator, relation_expression};
        public Expression ParseLogicExpression()
        {
            Expression leftExpression = ParseRelationExpression();
            if (!AssertNode(leftExpression))
                return null;
            if (AssertTokenType(TokenType.And, TokenType.Or))
            {
                TokenType operation = scanner.currentToken.type;
                scanner.NextToken();
                Expression rightExpression = ParseRelationExpression();
                AssertNodeOrRaiseError("Expected expression", true, rightExpression);
                return new LogicExpression(leftExpression, rightExpression, operation);
            }
            else
                return leftExpression;
        }
        // relation_expression     	=   additive_expression, {relation_operator, additive_expression};
        public Expression ParseRelationExpression()
        {
            Expression leftExpression = ParseAdditiveExpression();
            if (!AssertNode(leftExpression))
                return null;
            if (AssertTokenType(TokenType.Equal, TokenType.NotEqual, TokenType.Greater, TokenType.GreaterEqual, TokenType.Less, TokenType.LessEqual))
            {
                TokenType operation = scanner.currentToken.type;
                scanner.NextToken();
                Expression rightExpression = ParseAdditiveExpression();
                AssertNodeOrRaiseError("Expected expression", true, rightExpression);
                return new RelationExpression(leftExpression, rightExpression, operation);
            }
            else
                return leftExpression;
            
        }
        // additive_expression	    =   multiplicative_expression, {additive_operator, multiplicative_expression};
        public Expression ParseAdditiveExpression()
        {
            Expression leftExpression = ParseMultiplicativeExpression();
            if (!AssertNode(leftExpression))
                return null;
            if (AssertTokenType(TokenType.Plus, TokenType.Minus))
            {
                TokenType operation = scanner.currentToken.type;
                scanner.NextToken();
                Expression rightExpression = ParseMultiplicativeExpression();
                AssertNodeOrRaiseError("Expected expression", true, rightExpression);
                return new AdditiveExpression(leftExpression, rightExpression, operation);
            }
            else
                return leftExpression;
            
        }
        // multiplicative_expression=   unary_expression, { multiplicative_operator, unary_expression};
        public Expression ParseMultiplicativeExpression()
        {
            Expression leftExpression = ParseUnaryExpression();
            if (!AssertNode(leftExpression))
                return null;
            if (AssertTokenType(TokenType.Multiply, TokenType.Divide))
            {
                TokenType operation = scanner.currentToken.type;
                scanner.NextToken();
                Expression rightExpression = ParseUnaryExpression();
                AssertNodeOrRaiseError("Expected expression", true, rightExpression);
                return new MultiplicativeExpression(leftExpression, rightExpression, operation);
            }
            else
                return leftExpression;
        }
        // unary_expression	    	=   ["-"], simple_expression;
        public Expression ParseUnaryExpression()
        {
            //UnaryExpression unaryExpression = new UnaryExpression();
            if (AssertTokenType(TokenType.Minus))
            {
                scanner.NextToken();
                Expression expression = ParseValueExpression();
                AssertNodeOrRaiseError("Expected expression", true, expression);
                return new UnaryExpression(expression);
            }
            else
                return ParseValueExpression();
        }
        //simple_expression  		=	int
        //                              double
        //                              string
        //                              grouped_expression
        //                              function_call_expression
        //                              assignable
        public Expression ParseValueExpression()
        {
            Expression expression;
            expression = ParseInt();
            if (AssertNode(expression))
                return expression;
            expression = ParseDouble();
            if (AssertNode(expression))
                return expression;
            expression = ParseString();
            if (AssertNode(expression))
                return expression;
            expression = ParseBool();
            if (AssertNode(expression))
                return expression;
            expression = ParseAssignableExpression();
            if (AssertNode(expression))
                return expression;
            expression = ParseGroupedExpression();
            if (AssertNode(expression))
                return expression;
            return null; // we havent find any expression - pass empty one
        }
        public Expression ParseAssignableExpression()
        {
            Assignable assignable = ParseAssignable();
            if (!AssertNode(assignable))
                return null;
            Expression expression = ParseFunctionCallExpression(assignable);
            if (AssertNode(expression))
                return expression;
            return assignable;
        }
        public FunctionCallExpression ParseFunctionCallExpression(Assignable assignable)
        {
            if (!AssertTokenType(TokenType.LeftParanthesis))
                return null;
            scanner.NextToken();
            List<Expression> arguments = ParseArguments();
            AssertTokenTypeOrRaiseError("Expected right paranthesis", false, TokenType.RightParanthesis);
            scanner.NextToken();
            return new FunctionCallExpression(assignable, arguments);
        }
        public GroupedExpression ParseGroupedExpression()
        {
            if (!AssertTokenType(TokenType.LeftParanthesis))
                return null;
            scanner.NextToken();
            Expression expression = ParseExpression();
            AssertNodeOrRaiseError("Expected expression", true, expression);
            scanner.NextToken();
            AssertTokenTypeOrRaiseError("Expected right paranthesis", false, TokenType.RightParanthesis);
            scanner.NextToken();
            return new GroupedExpression(expression);
        }
        public IntValue ParseInt()
        {
            if (AssertTokenType(TokenType.Int))
            {
                var intValue = new IntValue(scanner.currentToken);
                scanner.NextToken();
                return intValue;
            }
            else
                return null;
        }
        public DoubleValue ParseDouble()
        {
            if (AssertTokenType( TokenType.Double))
            {
                var doubleValue = new DoubleValue(scanner.currentToken);
                scanner.NextToken();
                return doubleValue;
            }
            else
                return null;
        }
        public StringValue ParseString()
        {
            if (AssertTokenType(TokenType.String))
            {
                var stringValue = new StringValue(scanner.currentToken);
                scanner.NextToken();
                return stringValue;
            }
            else
                return null;
        }
        public BoolValue ParseBool()
        {
            if (AssertTokenType(TokenType.Bool))
            {
                var boolValue = new BoolValue(scanner.currentToken);
                scanner.NextToken();
                return boolValue;
            }
            else
                return null;
        }
        #endregion
    }
}
