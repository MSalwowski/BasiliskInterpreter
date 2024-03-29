﻿using System;
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
        public bool AssertTokenTypeOrRaiseError(string message, bool shouldThrow, params TokenType[] shouldBe)
        {
            if (!shouldBe.Contains(scanner.currentToken.type))
            {
                errorHandler.Error(message, scanner.currentToken.lineNumber, scanner.currentToken.position, shouldThrow);
                return false;
            }
            return true;
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
            Dictionary<(string,int),FunctionDefinition> functionsDefinitions = ParseDefinitions();
            List<Statement> statements = ParseStatements();
            return new ProgramRoot(functionsDefinitions, statements);
        }
        // definitions 				=   definition, {definition};
        public Dictionary<(string, int), FunctionDefinition> ParseDefinitions()
        {
            scanner.NextToken();
            Dictionary<(string, int), FunctionDefinition> functionDefinitions = new Dictionary<(string, int), FunctionDefinition>();
            FunctionDefinition functionDefinition = ParseDefinition();
            while (functionDefinition != null)
            {
                functionDefinitions.Add((functionDefinition.FunctionName, functionDefinition.Parameters == null ? 0 : functionDefinition.Parameters.Count()), functionDefinition);
                scanner.NextToken();
                functionDefinition = ParseDefinition();
            }
            return functionDefinitions;
        }
        // definition  				=   "def", identifier, '(', parameters, ')', ':', block;
        public FunctionDefinition ParseDefinition()
        {
            if (!AssertTokenType(TokenType.Define))
                return null;

            scanner.NextToken();
            AssertTokenTypeOrRaiseError("Expected identifier", true, TokenType.Identifier);
            var identifier = scanner.currentToken.value;

            scanner.NextToken();
            AssertTokenTypeOrRaiseError("Expected left paranthesis", false, TokenType.LeftParanthesis);

            scanner.NextToken();
            List<string> parameters = ParseParameters();

            AssertTokenTypeOrRaiseError("Expected right paranthesis", false, TokenType.RightParanthesis);

            scanner.NextToken();
            if(AssertTokenTypeOrRaiseError("Expected colon", false, TokenType.Colon))
                scanner.NextToken();

            BlockStatement blockStatement = ParseBlockStatement();

            FunctionDefinition definition = new FunctionDefinition(identifier, parameters, blockStatement);
            return definition;
        }
        // parameters  				=   [identifier, {',', identifier}];
        public List<string> ParseParameters() 
        {
            List<string> parameters = new List<string>();
            if (!AssertTokenType(TokenType.Identifier))
                return parameters;
            var parameter = scanner.currentToken.value;
            parameters.Add(parameter);
            scanner.NextToken();
            while (AssertTokenType(TokenType.Comma))
            {
                scanner.NextToken();
                AssertTokenTypeOrRaiseError("Expected identifier", true, TokenType.Identifier);
                parameter = scanner.currentToken.value;
                parameters.Add(parameter);
                scanner.NextToken();
            }
            return parameters;
        }
        // assignable  				=   identifier, ['.', identifier];
        public Assignable ParseAssignable()
        {
            if (!AssertTokenType(TokenType.Identifier))
                return null;
            var identifier = scanner.currentToken.value;
            scanner.NextToken();
            if (AssertTokenType(TokenType.Dot))
            {
                scanner.NextToken();
                AssertTokenTypeOrRaiseError("Expected identifier", true, TokenType.Identifier);
                var property = scanner.currentToken.value;
                scanner.NextToken();
                return new Assignable(identifier, property);
            }
            else
                return new Assignable(identifier);
        }
        // statements               =   { statement };
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
            if(AssertTokenTypeOrRaiseError("Expected left curly bracket", false, TokenType.LeftCurlyBracket))
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
        // if_statement    			=   "if", '(', logic_expression, ')', ':', block, ["else", ':', block];
        public IfStatement ParseIfStatement()
        {
            if (!AssertTokenType(TokenType.If))
                return null;

            scanner.NextToken();
            AssertTokenTypeOrRaiseError("Expected left paranthesis", false, TokenType.LeftParanthesis);

            scanner.NextToken();
            Expression condition = ParseLogicExpression();
            AssertNodeOrRaiseError("Expected condition", true, condition);

            AssertTokenTypeOrRaiseError("Expected right paranthesis", false, TokenType.RightParanthesis);

            scanner.NextToken();
            if(AssertTokenTypeOrRaiseError("Expected colon", false, TokenType.Colon))
                scanner.NextToken();

            BlockStatement trueStatement = ParseBlockStatement();
            AssertNodeOrRaiseError("Expected at least one statement", true, trueStatement);

            scanner.NextToken();
            if (!AssertTokenType(TokenType.Else))
                return new IfStatement(condition, trueStatement);

            scanner.NextToken();
            if(AssertTokenTypeOrRaiseError("Expected colon", false, TokenType.Colon))
                scanner.NextToken();

            BlockStatement falseStatement = ParseBlockStatement();
            return new IfStatement(condition, trueStatement, falseStatement);
        }
        // while_statement 			=   "while", '(', logic_expression, ')', block;
        public WhileStatement ParseWhileStatement()
        {
            if (!AssertTokenType(TokenType.While))
                return null;

            scanner.NextToken();
            AssertTokenTypeOrRaiseError("Expected left paranthesis", false, TokenType.LeftParanthesis);

            scanner.NextToken();
            Expression condition = ParseLogicExpression();
            AssertNodeOrRaiseError("Expected condition", true, condition);
            
            AssertTokenTypeOrRaiseError("Expected right paranthesis", false, TokenType.RightParanthesis);

            scanner.NextToken();

            if (AssertTokenTypeOrRaiseError("Expected colon", false, TokenType.Colon))
                scanner.NextToken();

            BlockStatement blockStatement = ParseBlockStatement();
            return new WhileStatement(condition, blockStatement);
        }
        // return_statement			=	"return", logic_expression;
        public ReturnStatement ParseReturnStatement()
        {
            if (!AssertTokenType(TokenType.Return))
                return null;
            scanner.NextToken();
            Expression expression = ParseLogicExpression();
            return new ReturnStatement(expression);
        }
        // assign_statement			=   assignable, assign_operator, logic_expression;
        public AssignStatement ParseAssignStatement(Assignable assignable) 
        {
            if (!AssertTokenType(TokenType.Assign))
                return null;
            scanner.NextToken();
            Expression expression = ParseLogicExpression();
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
        // arguments   				=   logic_expression, {',', logic_expression};
        public List<Expression> ParseArguments()
        {
            Expression argument = ParseLogicExpression();
            if (!AssertNode(argument))
                return null;
            List<Expression> arguments = new List<Expression>();
            arguments.Add(argument);
            while (AssertTokenType(TokenType.Comma))
            {
                scanner.NextToken();
                argument = ParseLogicExpression();
                AssertNodeOrRaiseError("Expected argument", true, argument);
                arguments.Add(argument);
            }
            return arguments;
        }
        // logic_expression	    	=   relation_expression, {logic_operator, relation_expression};
        public Expression ParseLogicExpression()
        {
            Expression leftExpression = ParseRelationExpression();
            if (!AssertNode(leftExpression))
                return null;
            if (!AssertTokenType(TokenType.And, TokenType.Or))
                return leftExpression;
            List<Token> operationTokens = new List<Token>();
            List<Expression> components = new List<Expression>();
            components.Add(leftExpression);
            do
            {
                operationTokens.Add(scanner.currentToken);
                scanner.NextToken();
                Expression rightExpression = ParseRelationExpression();
                AssertNodeOrRaiseError("Expected expression", true, rightExpression);
                components.Add(rightExpression);
            } while (AssertTokenType(TokenType.And, TokenType.Or));
            return new LogicExpression(components, operationTokens);
        }
        // relation_expression     	=   additive_expression, {relation_operator, additive_expression};
        public Expression ParseRelationExpression()
        {
            Expression leftExpression = ParseAdditiveExpression();
            if (!AssertNode(leftExpression))
                return null;
            if (!AssertTokenType(TokenType.Equal, TokenType.NotEqual, TokenType.Greater, TokenType.GreaterEqual, TokenType.Less, TokenType.LessEqual))
                return leftExpression;
            List<Token> operationTokens = new List<Token>();
            List<Expression> components = new List<Expression>();
            components.Add(leftExpression);
            do
            {
                operationTokens.Add(scanner.currentToken);
                scanner.NextToken();
                Expression rightExpression = ParseAdditiveExpression();
                AssertNodeOrRaiseError("Expected expression", true, rightExpression);
                components.Add(rightExpression);
            } while (AssertTokenType(TokenType.Equal, TokenType.NotEqual, TokenType.Greater, TokenType.GreaterEqual, TokenType.Less, TokenType.LessEqual));
            return new RelationExpression(components, operationTokens);

        }
        // additive_expression	    =   multiplicative_expression, {additive_operator, multiplicative_expression};
        public Expression ParseAdditiveExpression()
        {
            Expression leftExpression = ParseMultiplicativeExpression();
            if (!AssertNode(leftExpression))
                return null;
            if(!AssertTokenType(TokenType.Plus, TokenType.Minus))
                return leftExpression;
            List<Token> operationTokens = new List<Token>();
            List<Expression> components = new List<Expression>();
            components.Add(leftExpression);
            do
            {
                operationTokens.Add(scanner.currentToken);
                scanner.NextToken();
                Expression rightExpression = ParseMultiplicativeExpression();
                AssertNodeOrRaiseError("Expected expression", true, rightExpression);
                components.Add(rightExpression);
            } while (AssertTokenType(TokenType.Plus, TokenType.Minus));
            return new AdditiveExpression(components, operationTokens);
        }
        // multiplicative_expression=   unary_expression, { multiplicative_operator, unary_expression};
        public Expression ParseMultiplicativeExpression()
        {
            Expression leftExpression = ParseUnaryExpression();
            if (!AssertNode(leftExpression))
                return null;
            if (!AssertTokenType(TokenType.Multiply, TokenType.Divide))
                return leftExpression;
            List<Token> operationTokens = new List<Token>();
            List<Expression> components = new List<Expression>();
            components.Add(leftExpression);
            do
            {
                operationTokens.Add(scanner.currentToken);
                scanner.NextToken();
                Expression rightExpression = ParseUnaryExpression();
                AssertNodeOrRaiseError("Expected expression", true, rightExpression);
                components.Add(rightExpression);
            } while (AssertTokenType(TokenType.Multiply, TokenType.Divide));
            return new MultiplicativeExpression(components, operationTokens);
        }
        // unary_expression	    	=   ["-"], value_expression;
        public Expression ParseUnaryExpression()
        {
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
        //value_expression  		=	int
        //                              float
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
            expression = ParseFloat();
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
            return null; 
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
        //function_call_expression      =   assignable, '(', [arguments], ')';
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
            Expression expression = ParseLogicExpression();
            AssertNodeOrRaiseError("Expected expression", true, expression);
            AssertTokenTypeOrRaiseError("Expected right paranthesis", false, TokenType.RightParanthesis);
            scanner.NextToken();
            return new GroupedExpression(expression);
        }
        public IntValueExpression ParseInt()
        {
            if (AssertTokenType(TokenType.Int))
            {
                var intValue = new IntValueExpression(scanner.currentToken);
                scanner.NextToken();
                return intValue;
            }
            else
                return null;
        }
        public FloatValueExpression ParseFloat()
        {
            if (AssertTokenType( TokenType.Float))
            {
                var doubleValue = new FloatValueExpression(scanner.currentToken);
                scanner.NextToken();
                return doubleValue;
            }
            else
                return null;
        }
        public StringValueExpression ParseString()
        {
            if (AssertTokenType(TokenType.String))
            {
                var stringValue = new StringValueExpression(scanner.currentToken);
                scanner.NextToken();
                return stringValue;
            }
            else
                return null;
        }
        public BoolValueExpression ParseBool()
        {
            if (AssertTokenType(TokenType.Bool))
            {
                var boolValue = new BoolValueExpression(scanner.currentToken);
                scanner.NextToken();
                return boolValue;
            }
            else
                return null;
        }
        #endregion
    }
}
