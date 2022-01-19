using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BasiliskLang.Tokens;

namespace BasiliskLang
{
    public class Parser
    {
        IScanner scanner;
        public Parser(IScanner _scanner)
        {
            scanner = _scanner;
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
        public void AssertTokenTypeOrRaiseError(string message, params TokenType[] shouldBe)
        {
            if (!shouldBe.Contains(scanner.currentToken.type))
                throw new ParseException(message, scanner.currentToken.lineNumber, scanner.currentToken.position);
        }
        public bool AssertNode(Node node)
        {
            if (node == null)
                return false;
            return true;
        }
        public void AssertNodeOrRaiseError(string message, Node node)
        {
            if(node == null)
                throw new ParseException(message, scanner.currentToken.lineNumber, scanner.currentToken.position);
        }
        #endregion
        #region grammar
        // program     				=   definitions, statements;
        public ProgramRoot ParseProgram()
        {
            ProgramRoot programRoot = new ProgramRoot();
            List<Definition> definitions = ParseDefinitions();
            programRoot.SetDefinitions(definitions);
            List<Statement> statements = ParseStatements();
            programRoot.SetStatements(statements);
            return programRoot;
        }
        // definitions 				=   definition, {definition};
        public List<Definition> ParseDefinitions()
        {
            scanner.NextToken();
            List<Definition> definitions = new List<Definition>();
            Definition definition = ParseDefinition();
            while (definition != null)
            {
                definitions.Add(definition);
                scanner.NextToken();
                definition = ParseDefinition();
            }
            return definitions;
        }
        // definition  				=   "def", identifier, '(', parameters, ')', ':', block;
        public Definition ParseDefinition()
        {
            Definition definition = new Definition();

            if (!AssertTokenType(TokenType.Define))
                return null;

            scanner.NextToken();
            AssertTokenTypeOrRaiseError("Expected identifier", TokenType.Identifier);
            definition.SetIdentifier(new Identifier(scanner.currentToken));

            scanner.NextToken();
            AssertTokenTypeOrRaiseError("Expected left paranthesis", TokenType.LeftParanthesis);

            scanner.NextToken();
            List<Assignable> parameters = ParseParameters();
            definition.SetParameters(parameters);
            //albo tu sprawdzic: jak cos przeszedl dalej to juz nie idz
            scanner.NextToken();
            AssertTokenTypeOrRaiseError("Expected colon", TokenType.Colon);

            scanner.NextToken();
            BlockStatement blockStatement = ParseBlockStatement();
            //AssertNodeOrRaiseError("Expected block statement", blockStatement); - to niepotrzebne bo sprawdzam w blocku
            definition.SetBlock(blockStatement);
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
                AssertNodeOrRaiseError("Expected parameter", parameter);
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
            while (AssertTokenType(TokenType.Dot))
            {
                scanner.NextToken();
                identifier = ParseIdentifier();
                AssertNodeOrRaiseError("Expected identifier", identifier);
                assignable.AddIdentifier(identifier);
                scanner.NextToken();
            }
            return assignable;
        }
        // identifier
        public Identifier ParseIdentifier()
        {
            if(AssertTokenType(TokenType.Identifier))
                return new Identifier(scanner.currentToken);
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
                //scanner.NextToken();
                statement = ParseStatement();
            }
            return statements;
        }
        // block       				=   '{', statements, '}';
        public BlockStatement ParseBlockStatement()
        {
            BlockStatement blockStatement = new BlockStatement();
            AssertTokenTypeOrRaiseError("Expected left curly bracket", TokenType.LeftCurlyBracket);
            scanner.NextToken();
            List<Statement> statements = ParseStatements();
            blockStatement.SetStatements(statements);  
            //scanner.NextToken();
            AssertTokenTypeOrRaiseError("Expected right curly bracket", TokenType.RightCurlyBracket);
            return blockStatement;
        }
        // statement 				=	if_statement
        //                              while_statement
        //                              return_statement
        //                              assign_statement    - both starts with assignable, so quick production check
        //                              call;               -
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
            Assignable assignable = ParseAssignable();
            if(AssertNode(assignable))
            {
                // no scanner.nextToken() because if we created assignable we have already moved scanner
                if (AssertTokenType(TokenType.Assign))
                    return ParseAssignStatement(assignable);
                if (AssertTokenType(TokenType.LeftParanthesis))
                    return ParseCallStatement(assignable);
            }
            return null;
        }
        // if_statement    			=   "if", '(', expression, ')', ':', block, ["else", ':', block];
        public IfStatement ParseIfStatement()
        {
            IfStatement ifStatement = new IfStatement();
            if (!AssertTokenType(TokenType.If))
                return null;

            scanner.NextToken();
            AssertTokenTypeOrRaiseError("Expected left paranthesis", TokenType.LeftParanthesis);

            scanner.NextToken();
            Expression condition = ParseExpression();
            AssertNodeOrRaiseError("Expected condition", condition);
            ifStatement.SetCondition(condition);
            // care: it was changed, may be wrong
            //scanner.NextToken();
            // we expect ')' to be current
            AssertTokenTypeOrRaiseError("Expected right paranthesis", TokenType.RightParanthesis);

            scanner.NextToken();
            AssertTokenTypeOrRaiseError("Expected colon", TokenType.Colon);

            scanner.NextToken();
            BlockStatement trueStatement = ParseBlockStatement();
            AssertNodeOrRaiseError("Expected statement", trueStatement);
            ifStatement.SetTrueBlockStatement(trueStatement);

            scanner.NextToken();
            if (!AssertTokenType(TokenType.Else))
                return ifStatement;

            scanner.NextToken();
            AssertTokenTypeOrRaiseError("Expected colon", TokenType.Colon);

            scanner.NextToken();
            BlockStatement falseStatement = ParseBlockStatement();
            ifStatement.SetFalseBlockStatement(falseStatement);
            return ifStatement;
        }
        // while_statement 			=   "while", '(', expression, ')', block;
        public WhileStatement ParseWhileStatement()
        {
            WhileStatement whileStatement = new WhileStatement();
            if (!AssertTokenType(TokenType.While))
                return null;

            scanner.NextToken();
            AssertTokenTypeOrRaiseError("Expected left paranthesis", TokenType.LeftParanthesis);

            scanner.NextToken();
            Expression condition = ParseExpression();
            AssertNodeOrRaiseError("Expected condition", condition);
            whileStatement.SetCondition(condition);
            
            //scanner.NextToken();
            AssertTokenTypeOrRaiseError("Expected right paranthesis", TokenType.RightParanthesis);

            scanner.NextToken();
            AssertTokenTypeOrRaiseError("Expected colon", TokenType.Colon);

            scanner.NextToken();
            BlockStatement blockStatement = ParseBlockStatement();
            whileStatement.SetBlockStatement(blockStatement);
            return whileStatement;
        }
        // return_statement			=	"return", expression;
        public ReturnStatement ParseReturnStatement()
        {
            ReturnStatement returnStatement = new ReturnStatement();
            if (!AssertTokenType(TokenType.Return))
                return null;

            scanner.NextToken();
            Expression expression = ParseExpression();
            if(AssertNode(expression))
                returnStatement.SetExpression(expression);
            //AssertNodeOrRaiseError("Expected expression", expression);
            //returnStatement.SetExpression(expression);
            return returnStatement;
        }
        // assign_statement			=   assignable, assign_operator, expression;
        public AssignStatement ParseAssignStatement(Assignable assignable) 
        {
            AssignStatement assignStatement = new AssignStatement();
            assignStatement.SetLeft(assignable);

            scanner.NextToken(); // because we know, that current was '='
            Expression expression = ParseExpression();
            AssertNodeOrRaiseError("Expected expression", expression);
            assignStatement.SetRight(expression);
            return assignStatement;
        }
        // call        				=   assignable, '(', [arguments], ')';
        public Call ParseCallStatement(Assignable assignable) 
        {
            Call call = new Call();
            call.SetAssignable(assignable);

            scanner.NextToken(); // because we know that current was '('
            List<Expression> arguments = ParseArguments();
            // check if there are any arguments
            if(AssertNode(arguments?[0]))
                call.SetArguments(arguments);
            AssertTokenTypeOrRaiseError("Expected right paranthesis", TokenType.RightParanthesis);
            // maybe move? its last token in line (perhaps)
            return call;
        }
        // arguments   				=   expression, {',', expression};
        public List<Expression> ParseArguments()
        {
            List<Expression> arguments = new List<Expression>();
            Expression argument = ParseExpression();
            if (!AssertNode(argument))
                return null;
            arguments.Add(argument);
            while (AssertTokenType(TokenType.Comma))
            {
                //consume comma
                scanner.NextToken();
                argument = ParseExpression();
                AssertNodeOrRaiseError("Expected argument", argument);
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
        public LogicExpression ParseLogicExpression()
        {
            LogicExpression logicExpression = new LogicExpression();
            RelationExpression leftExpression = ParseRelationExpression();
            if (!AssertNode(leftExpression))
                return null;
            logicExpression.SetLeft(leftExpression);
            // zakładam, że dostajemy już następny token
            if (AssertTokenType(TokenType.And, TokenType.Or))
            {
                logicExpression.SetOperation(scanner.currentToken.type);
                scanner.NextToken();
                RelationExpression rightExpression = ParseRelationExpression();
                AssertNodeOrRaiseError("Expected expression", rightExpression);
                logicExpression.SetRight(rightExpression);
            }
            return logicExpression;
        }
        // relation_expression     	=   additive_expression, {relation_operator, additive_expression};
        public RelationExpression ParseRelationExpression()
        {
            RelationExpression relationExpression = new RelationExpression();
            AdditiveExpression leftExpression = ParseAdditiveExpression();
            if (!AssertNode(leftExpression))
                return null;
            relationExpression.SetLeft(leftExpression);
            // zakładam, że dostajemy już następny token
            if (AssertTokenType(TokenType.Equal, TokenType.NotEqual, TokenType.Greater, TokenType.GreaterEqual, TokenType.Less, TokenType.LessEqual))
            {
                relationExpression.SetOperation(scanner.currentToken.type);
                scanner.NextToken();
                AdditiveExpression rightExpression = ParseAdditiveExpression();
                AssertNodeOrRaiseError("Expected expression", rightExpression);
                relationExpression.SetRight(rightExpression);
            }
            return relationExpression;
        }
        // additive_expression	    =   multiplicative_expression, {additive_operator, multiplicative_expression};
        public AdditiveExpression ParseAdditiveExpression()
        {
            AdditiveExpression additiveExpression = new AdditiveExpression();
            MultiplicativeExpression leftExpression = ParseMultiplicativeExpression();
            if (!AssertNode(leftExpression))
                return null;
            additiveExpression.SetLeft(leftExpression);
            // zakładam, że dostajemy już następny token
            if(AssertTokenType(TokenType.Plus, TokenType.Minus))
            {
                additiveExpression.SetOperation(scanner.currentToken.type);
                scanner.NextToken();
                MultiplicativeExpression rightExpression = ParseMultiplicativeExpression();
                AssertNodeOrRaiseError("Expected expression", rightExpression);
                additiveExpression.SetRight(rightExpression);
            }
            return additiveExpression;
        }
        // multiplicative_expression=   unary_expression, { multiplicative_operator, unary_expression};
        public MultiplicativeExpression ParseMultiplicativeExpression()
        {
            MultiplicativeExpression multiplicativeExpression = new MultiplicativeExpression();
            UnaryExpression leftExpression = ParseUnaryExpression();
            if (!AssertNode(leftExpression))
                return null;
            multiplicativeExpression.SetLeft(leftExpression);
            // zakładam, że dostajemy już następny token
            //AssertTokenType(new List<TokenType>() { TokenType.And, TokenType.Or });
            if (AssertTokenType(TokenType.Multiply, TokenType.Divide))
            {
                multiplicativeExpression.SetOperation(scanner.currentToken.type);
                scanner.NextToken();
                UnaryExpression rightExpression = ParseUnaryExpression();
                AssertNodeOrRaiseError("Expected expression", rightExpression);
                multiplicativeExpression.SetRight(rightExpression);
            }
            return multiplicativeExpression;
        }
        // unary_expression	    	=   ["-"], simple_expression;
        public UnaryExpression ParseUnaryExpression()
        {
            UnaryExpression unaryExpression = new UnaryExpression();
            if (AssertTokenType(TokenType.Minus))
            {
                unaryExpression.SetNegation();
                scanner.NextToken();
            }
            SimpleExpression expression = ParseSimpleExpression();
            if (!AssertNode(expression))
                return null;
            if (unaryExpression.isNegated)
                AssertNodeOrRaiseError("Expected expression", expression);
            unaryExpression.SetExpression(expression);
            return unaryExpression;
        }
        //simple_expression  		=	int
        //                              double
        //                              string
        //                              assignable
        //                              call;
        //                              '(', expression, ')'
        public SimpleExpression ParseSimpleExpression()
        {
            SimpleExpression simpleExpression;
            simpleExpression = ParseInt();
            if (AssertNode(simpleExpression))
                return simpleExpression;
            simpleExpression = ParseDouble();
            if (AssertNode(simpleExpression))
                return simpleExpression;
            simpleExpression = ParseString();
            if (AssertNode(simpleExpression))
                return simpleExpression;
            simpleExpression = ParseBool();
            if (AssertNode(simpleExpression))
                return simpleExpression;
            simpleExpression = ParseAssignable();
            if (AssertNode(simpleExpression))
            {
                if(AssertTokenType(TokenType.LeftParanthesis))
                    simpleExpression = ParseCallStatement(simpleExpression as Assignable);
                return simpleExpression;
            }
            if(AssertTokenType(TokenType.LeftParanthesis))
            {
                scanner.NextToken();
                Expression expression = ParseExpression();
                AssertNodeOrRaiseError("Expected expression", expression);
                simpleExpression = new GroupedExpression();
                (simpleExpression as GroupedExpression).SetExpression(expression);
                AssertTokenTypeOrRaiseError("Expected right paranthesis", TokenType.RightParanthesis);
                scanner.NextToken();
                return simpleExpression;
            }
            return null; // we havent find any expression - pass empty one
        }
        // int
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
        // double
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
        // string
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
        // bool
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
