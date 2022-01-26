using System;
using System.Collections.Generic;
using NUnit.Framework;
using BasiliskLang.Tokens;
using BasiliskLang;
using System.IO;
using Moq;
using BasiliskLang.Helpers;

namespace Tests
{
    public class ParserTests
    {
        [Test]
        public void ParseDefinition()
        {
            Mock<BlockStatement> blockStatement = new Mock<BlockStatement>(null);
            FunctionDefinition expectedResult = new FunctionDefinition("test", null, blockStatement.Object);

            #region input-setup
            Stack<Token> inputTokens = new Stack<Token>();
            inputTokens.Push(new WritableToken(TokenType.RightCurlyBracket));
            inputTokens.Push(new WritableToken(TokenType.Int));
            inputTokens.Push(new WritableToken(TokenType.Return));
            inputTokens.Push(new WritableToken(TokenType.LeftCurlyBracket));
            inputTokens.Push(new WritableToken(TokenType.Colon));
            inputTokens.Push(new WritableToken(TokenType.RightParanthesis));
            inputTokens.Push(new WritableToken(TokenType.LeftParanthesis));
            inputTokens.Push(new WritableToken(TokenType.Identifier).WithValue("test"));
            inputTokens.Push(new WritableToken(TokenType.Define));
            #endregion

            Mock<IScanner> scanner = new Mock<IScanner>();
            scanner.Setup(s => s.NextToken())
                   .Callback(() => scanner.Setup(s => s.currentToken)
                                          .Returns(inputTokens.Pop())
                   );
            Mock<IErrorHandler> errorHandler = new Mock<IErrorHandler>();
            Parser parser = new Parser(scanner.Object, errorHandler.Object);
            scanner.Object.NextToken();
            var result = parser.ParseDefinition();
            
            Assert.AreEqual(expectedResult, result);
        }
        [Test]
        public void IfStatement()
        {
            Mock<Expression> expression = new Mock<Expression>(NodeType.Expression);
            Mock<BlockStatement> blockStatement = new Mock<BlockStatement>(null);
            IfStatement expectedResult = new IfStatement(expression.Object, blockStatement.Object);
            #region input-setup
            Stack<Token> inputTokens = new Stack<Token>();
            inputTokens.Push(new WritableToken(TokenType.EOF));
            inputTokens.Push(new WritableToken(TokenType.RightCurlyBracket));
            inputTokens.Push(new WritableToken(TokenType.Int));
            inputTokens.Push(new WritableToken(TokenType.Return));
            inputTokens.Push(new WritableToken(TokenType.LeftCurlyBracket));
            inputTokens.Push(new WritableToken(TokenType.Colon));
            inputTokens.Push(new WritableToken(TokenType.RightParanthesis));
            inputTokens.Push(new WritableToken(TokenType.Int));
            inputTokens.Push(new WritableToken(TokenType.Greater));
            inputTokens.Push(new WritableToken(TokenType.Identifier));
            inputTokens.Push(new WritableToken(TokenType.LeftParanthesis));
            inputTokens.Push(new WritableToken(TokenType.If));
            #endregion

            Mock<IScanner> scanner = new Mock<IScanner>();
            scanner.Setup(s => s.NextToken())
                   .Callback(() => scanner.Setup(s => s.currentToken)
                                          .Returns(inputTokens.Pop())
                   );
            Mock<IErrorHandler> errorHandler = new Mock<IErrorHandler>();
            Parser parser = new Parser(scanner.Object, errorHandler.Object);
            scanner.Object.NextToken();
            var result = parser.ParseIfStatement();
            Assert.AreEqual(expectedResult, result);
        }
        [Test]
        public void WhileStatement()
        {
            Mock<Expression> expression = new Mock<Expression>(NodeType.Expression);
            Mock<BlockStatement> blockStatement = new Mock<BlockStatement>(null);
            WhileStatement expectedResult = new WhileStatement(expression.Object, blockStatement.Object);
            #region input-setup
            Stack<Token> inputTokens = new Stack<Token>();
            inputTokens.Push(new WritableToken(TokenType.EOF));
            inputTokens.Push(new WritableToken(TokenType.RightCurlyBracket));
            inputTokens.Push(new WritableToken(TokenType.Int));
            inputTokens.Push(new WritableToken(TokenType.Return));
            inputTokens.Push(new WritableToken(TokenType.LeftCurlyBracket));
            inputTokens.Push(new WritableToken(TokenType.Colon));
            inputTokens.Push(new WritableToken(TokenType.RightParanthesis));
            inputTokens.Push(new WritableToken(TokenType.Int));
            inputTokens.Push(new WritableToken(TokenType.Greater));
            inputTokens.Push(new WritableToken(TokenType.Identifier));
            inputTokens.Push(new WritableToken(TokenType.LeftParanthesis));
            inputTokens.Push(new WritableToken(TokenType.While));
            #endregion

            Mock<IScanner> scanner = new Mock<IScanner>();
            scanner.Setup(s => s.NextToken())
                   .Callback(() => scanner.Setup(s => s.currentToken)
                                          .Returns(inputTokens.Pop())
                   );
            Mock<IErrorHandler> errorHandler = new Mock<IErrorHandler>();
            Parser parser = new Parser(scanner.Object, errorHandler.Object);
            scanner.Object.NextToken();
            var result = parser.ParseWhileStatement();
            Assert.AreEqual(expectedResult, result);
        }
        [Test]
        public void ReturnStatement()
        {
            ReturnStatement expectedResult = new ReturnStatement();
            Stack<Token> inputTokens = new Stack<Token>();
            #region input
            inputTokens.Push(new WritableToken(TokenType.EOF));
            inputTokens.Push(new WritableToken(TokenType.Return));
            #endregion
            Mock<IScanner> scanner = new Mock<IScanner>();
            scanner.Setup(s => s.NextToken())
                   .Callback(() => scanner.Setup(s => s.currentToken)
                                          .Returns(inputTokens.Pop())
                   );
            Mock<IErrorHandler> errorHandler = new Mock<IErrorHandler>();
            Parser parser = new Parser(scanner.Object, errorHandler.Object);
            scanner.Object.NextToken();
            var result = parser.ParseReturnStatement();
            Assert.AreEqual(expectedResult, result);
        }
        [Test]
        public void AssignStatement()
        {
            Mock<Expression> expression = new Mock<Expression>(NodeType.Expression);
            AssignStatement expectedResult = new AssignStatement(new Assignable("test"), expression.Object);
            Stack<Token> inputTokens = new Stack<Token>();
            #region input
            inputTokens.Push(new WritableToken(TokenType.EOF));
            inputTokens.Push(new WritableToken(TokenType.Int));
            inputTokens.Push(new WritableToken(TokenType.Assign));
            #endregion
            Mock<IScanner> scanner = new Mock<IScanner>();
            scanner.Setup(s => s.NextToken())
                   .Callback(() => scanner.Setup(s => s.currentToken)
                                          .Returns(inputTokens.Pop())
                   );
            Mock<IErrorHandler> errorHandler = new Mock<IErrorHandler>();
            Parser parser = new Parser(scanner.Object, errorHandler.Object);
            scanner.Object.NextToken();
            Assignable realAssignable = new Assignable("test");
            var result = parser.ParseAssignStatement(realAssignable);
            Assert.AreEqual(expectedResult, result);
        }
    }
}