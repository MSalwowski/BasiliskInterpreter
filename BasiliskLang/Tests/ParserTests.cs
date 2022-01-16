using System;
using System.Collections.Generic;
using NUnit.Framework;
using BasiliskLang.Tokens;
using BasiliskLang;
using System.IO;
using Moq;

namespace Tests
{
    public class ParserTests
    {
        [Test]
        public void DefinitionWithoutParameters()
        {
            ProgramRoot expectedRoot = new ProgramRoot();
            Identifier identifier = new Identifier(new WritableToken(TokenType.Identifier));
            Definition definition = new Definition();
            BlockStatement blockStatement = new BlockStatement();
            ReturnStatement returnStatement = new ReturnStatement();
            LogicExpression logicExpression = new LogicExpression();
            RelationExpression relationExpression = new RelationExpression();
            AdditiveExpression additiveExpression = new AdditiveExpression();
            MultiplicativeExpression multiplicativeExpression = new MultiplicativeExpression();
            UnaryExpression unaryExpression = new UnaryExpression();
            IntValue intValue = new IntValue(new Token(TokenType.Int, 0, 0, "1"));
            unaryExpression.SetExpression(intValue);
            multiplicativeExpression.SetLeft(unaryExpression);
            additiveExpression.SetLeft(multiplicativeExpression);
            relationExpression.SetLeft(additiveExpression);
            logicExpression.SetLeft(relationExpression);
            returnStatement.SetExpression(logicExpression);
            blockStatement.SetStatements(new List<Statement>() { returnStatement });
            definition.SetIdentifier(identifier);
            definition.SetBlock(blockStatement);
            expectedRoot.SetDefinitions(new List<Definition>() { definition });

            Stack<Token> inputTokens = new Stack<Token>();
            inputTokens.Push(new WritableToken(TokenType.EOF));
            inputTokens.Push(new WritableToken(TokenType.RightCurlyBracket));
            inputTokens.Push(new WritableToken(TokenType.Int));
            inputTokens.Push(new WritableToken(TokenType.Return));
            inputTokens.Push(new WritableToken(TokenType.LeftCurlyBracket));
            inputTokens.Push(new WritableToken(TokenType.Colon));
            inputTokens.Push(new WritableToken(TokenType.RightParanthesis));
            inputTokens.Push(new WritableToken(TokenType.LeftParanthesis));
            inputTokens.Push(new WritableToken(TokenType.Identifier));
            inputTokens.Push(new WritableToken(TokenType.Define));

            Mock<IScanner> scanner = new Mock<IScanner>();
            scanner.Setup(s => s.NextToken())
                   .Callback(() => scanner.Setup(s => s.currentToken)
                                          .Returns(inputTokens.Pop())
                   );
            Parser parser = new Parser(scanner.Object);
            ProgramRoot pr = parser.Parse();
            Assert.AreEqual(expectedRoot, pr);
            
        }

   
     
   
        
        
    }
}