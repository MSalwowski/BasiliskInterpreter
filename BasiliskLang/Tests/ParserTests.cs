//using System;
//using System.Collections.Generic;
//using NUnit.Framework;
//using BasiliskLang.Tokens;
//using BasiliskLang;
//using System.IO;
//using Moq;

//namespace Tests
//{
//    public class ParserTests
//    {
//        Identifier identifier;
//        Definition definition;
//        IfStatement ifStatement;
//        WhileStatement whileStatement;
//        BlockStatement blockStatement;
//        ReturnStatement returnStatement;
//        LogicExpression logicExpression;
//        RelationExpression relationExpression;
//        AdditiveExpression additiveExpression;
//        MultiplicativeExpression multiplicativeExpression;
//        UnaryExpression unaryExpression;
//        Assignable assignable; // without "."
//        Mock<IntValue> intValue;
//        Mock<BoolValue> boolValue;

//        [SetUp]
//        public void SetUp()
//        {
//            identifier = new Identifier(new WritableToken(TokenType.Identifier));
//            definition = new Definition();
//            blockStatement = new BlockStatement();
//            ifStatement = new IfStatement();
//            whileStatement = new WhileStatement();
//            returnStatement = new ReturnStatement();
//            logicExpression = new LogicExpression();
//            relationExpression = new RelationExpression();
//            additiveExpression = new AdditiveExpression();
//            multiplicativeExpression = new MultiplicativeExpression();
//            unaryExpression = new UnaryExpression();
//            assignable = new Assignable(identifier);
//            intValue = new Mock<IntValue>(new WritableToken(TokenType.Int));
//            boolValue = new Mock<BoolValue>(new WritableToken(TokenType.Bool));
//        }
//        [Test]
//        public void DefinitionWithoutParameters()
//        {
//            #region tree-setup
//            unaryExpression.SetExpression(intValue.Object);
//            multiplicativeExpression.SetLeft(unaryExpression);
//            additiveExpression.SetLeft(multiplicativeExpression);
//            relationExpression.SetLeft(additiveExpression);
//            logicExpression.SetLeft(relationExpression);
//            returnStatement.SetExpression(logicExpression);
//            blockStatement.SetStatements(new List<Statement>() { returnStatement });
//            definition.SetIdentifier(identifier);
//            definition.SetBlock(blockStatement);

//            ProgramRoot expectedRoot = new ProgramRoot();
//            expectedRoot.SetDefinitions(new List<Definition>() { definition });
//            #endregion
//            #region input-setup
//            Stack<Token> inputTokens = new Stack<Token>();
//            inputTokens.Push(new WritableToken(TokenType.EOF));
//            inputTokens.Push(new WritableToken(TokenType.RightCurlyBracket));
//            inputTokens.Push(new WritableToken(TokenType.Int));
//            inputTokens.Push(new WritableToken(TokenType.Return));
//            inputTokens.Push(new WritableToken(TokenType.LeftCurlyBracket));
//            inputTokens.Push(new WritableToken(TokenType.Colon));
//            inputTokens.Push(new WritableToken(TokenType.RightParanthesis));
//            inputTokens.Push(new WritableToken(TokenType.LeftParanthesis));
//            inputTokens.Push(new WritableToken(TokenType.Identifier));
//            inputTokens.Push(new WritableToken(TokenType.Define));
//            #endregion

//            Mock<IScanner> scanner = new Mock<IScanner>();
//            scanner.Setup(s => s.NextToken())
//                   .Callback(() => scanner.Setup(s => s.currentToken)
//                                          .Returns(inputTokens.Pop())
//                   );
//            Parser parser = new Parser(scanner.Object);
//            ProgramRoot pr = parser.Parse();
//            Assert.AreEqual(expectedRoot, pr);
//        }
//        [Test]
//        public void DefinitionWithOneParameter()
//        {
//            #region tree-setup 
//            unaryExpression.SetExpression(assignable);
//            multiplicativeExpression.SetLeft(unaryExpression);
//            additiveExpression.SetLeft(multiplicativeExpression);
//            relationExpression.SetLeft(additiveExpression);
//            logicExpression.SetLeft(relationExpression);
//            returnStatement.SetExpression(logicExpression);
//            blockStatement.SetStatements(new List<Statement>() { returnStatement });
//            definition.SetIdentifier(identifier);
//            definition.SetParameters(new List<Assignable>() { assignable });
//            definition.SetBlock(blockStatement);

//            ProgramRoot expectedRoot = new ProgramRoot();
//            expectedRoot.SetDefinitions(new List<Definition>() { definition });
//            #endregion
//            #region input-setup
//            Stack<Token> inputTokens = new Stack<Token>();
//            inputTokens.Push(new WritableToken(TokenType.EOF));
//            inputTokens.Push(new WritableToken(TokenType.RightCurlyBracket));
//            inputTokens.Push(new WritableToken(TokenType.Identifier));
//            inputTokens.Push(new WritableToken(TokenType.Return));
//            inputTokens.Push(new WritableToken(TokenType.LeftCurlyBracket));
//            inputTokens.Push(new WritableToken(TokenType.Colon));
//            inputTokens.Push(new WritableToken(TokenType.RightParanthesis));
//            inputTokens.Push(new WritableToken(TokenType.Identifier));
//            inputTokens.Push(new WritableToken(TokenType.LeftParanthesis));
//            inputTokens.Push(new WritableToken(TokenType.Identifier));
//            inputTokens.Push(new WritableToken(TokenType.Define));
//            #endregion

//            Mock<IScanner> scanner = new Mock<IScanner>();
//            scanner.Setup(s => s.NextToken())
//                   .Callback(() => scanner.Setup(s => s.currentToken)
//                                          .Returns(inputTokens.Pop())
//                   );
//            Parser parser = new Parser(scanner.Object);
//            ProgramRoot pr = parser.Parse();
//            Assert.AreEqual(expectedRoot, pr);
//        }
//        [Test]
//        public void DefinitionWithMultipleParameters()
//        {
//            #region tree-setup
//            unaryExpression.SetExpression(assignable);
//            multiplicativeExpression.SetLeft(unaryExpression);
//            additiveExpression.SetLeft(multiplicativeExpression);
//            relationExpression.SetLeft(additiveExpression);
//            logicExpression.SetLeft(relationExpression);
//            returnStatement.SetExpression(logicExpression);
//            blockStatement.SetStatements(new List<Statement>() { returnStatement });
//            definition.SetIdentifier(identifier);
//            definition.SetParameters(new List<Assignable>() { assignable, assignable, assignable });
//            definition.SetBlock(blockStatement);

//            ProgramRoot expectedRoot = new ProgramRoot();
//            expectedRoot.SetDefinitions(new List<Definition>() { definition });
//            #endregion
//            #region input-setup
//            Stack<Token> inputTokens = new Stack<Token>();
//            inputTokens.Push(new WritableToken(TokenType.EOF));
//            inputTokens.Push(new WritableToken(TokenType.RightCurlyBracket));
//            inputTokens.Push(new WritableToken(TokenType.Identifier));
//            inputTokens.Push(new WritableToken(TokenType.Return));
//            inputTokens.Push(new WritableToken(TokenType.LeftCurlyBracket));
//            inputTokens.Push(new WritableToken(TokenType.Colon));
//            inputTokens.Push(new WritableToken(TokenType.RightParanthesis));
//            inputTokens.Push(new WritableToken(TokenType.Identifier));
//            inputTokens.Push(new WritableToken(TokenType.Comma));
//            inputTokens.Push(new WritableToken(TokenType.Identifier));
//            inputTokens.Push(new WritableToken(TokenType.Comma));
//            inputTokens.Push(new WritableToken(TokenType.Identifier));
//            inputTokens.Push(new WritableToken(TokenType.LeftParanthesis));
//            inputTokens.Push(new WritableToken(TokenType.Identifier));
//            inputTokens.Push(new WritableToken(TokenType.Define));
//            #endregion

//            Mock<IScanner> scanner = new Mock<IScanner>();
//            scanner.Setup(s => s.NextToken())
//                   .Callback(() => scanner.Setup(s => s.currentToken)
//                                          .Returns(inputTokens.Pop())
//                   );
//            Parser parser = new Parser(scanner.Object);
//            ProgramRoot pr = parser.Parse();
//            Assert.AreEqual(expectedRoot, pr);
//        }
//        [Test]
//        public void TwoDefinitions()
//        {
//            #region tree-setup
//            unaryExpression.SetExpression(intValue.Object);
//            multiplicativeExpression.SetLeft(unaryExpression);
//            additiveExpression.SetLeft(multiplicativeExpression);
//            relationExpression.SetLeft(additiveExpression);
//            logicExpression.SetLeft(relationExpression);
//            returnStatement.SetExpression(logicExpression);
//            blockStatement.SetStatements(new List<Statement>() { returnStatement });
//            definition.SetIdentifier(identifier);
//            definition.SetBlock(blockStatement);

//            ProgramRoot expectedRoot = new ProgramRoot();
//            expectedRoot.SetDefinitions(new List<Definition>() { definition, definition });
//            #endregion
//            #region input-setup
//            Stack<Token> inputTokens = new Stack<Token>();
//            inputTokens.Push(new WritableToken(TokenType.EOF));
//            //second-definition
//            inputTokens.Push(new WritableToken(TokenType.RightCurlyBracket));
//            inputTokens.Push(new WritableToken(TokenType.Int));
//            inputTokens.Push(new WritableToken(TokenType.Return));
//            inputTokens.Push(new WritableToken(TokenType.LeftCurlyBracket));
//            inputTokens.Push(new WritableToken(TokenType.Colon));
//            inputTokens.Push(new WritableToken(TokenType.RightParanthesis));
//            inputTokens.Push(new WritableToken(TokenType.LeftParanthesis));
//            inputTokens.Push(new WritableToken(TokenType.Identifier));
//            inputTokens.Push(new WritableToken(TokenType.Define));
//            //first-definition
//            inputTokens.Push(new WritableToken(TokenType.RightCurlyBracket));
//            inputTokens.Push(new WritableToken(TokenType.Int));
//            inputTokens.Push(new WritableToken(TokenType.Return));
//            inputTokens.Push(new WritableToken(TokenType.LeftCurlyBracket));
//            inputTokens.Push(new WritableToken(TokenType.Colon));
//            inputTokens.Push(new WritableToken(TokenType.RightParanthesis));
//            inputTokens.Push(new WritableToken(TokenType.LeftParanthesis));
//            inputTokens.Push(new WritableToken(TokenType.Identifier));
//            inputTokens.Push(new WritableToken(TokenType.Define));
//            #endregion

//            Mock<IScanner> scanner = new Mock<IScanner>();
//            scanner.Setup(s => s.NextToken())
//                   .Callback(() => scanner.Setup(s => s.currentToken)
//                                          .Returns(inputTokens.Pop())
//                   );
//            Parser parser = new Parser(scanner.Object);
//            ProgramRoot pr = parser.Parse();
//            Assert.AreEqual(expectedRoot, pr);
//        }
//        [Test]
//        public void DefinitionWithoutBodyThrowsException()
//        {
//            #region tree-setup
//            unaryExpression.SetExpression(intValue.Object);
//            multiplicativeExpression.SetLeft(unaryExpression);
//            additiveExpression.SetLeft(multiplicativeExpression);
//            relationExpression.SetLeft(additiveExpression);
//            logicExpression.SetLeft(relationExpression);
//            returnStatement.SetExpression(logicExpression);
//            blockStatement.SetStatements(new List<Statement>() { returnStatement });
//            definition.SetIdentifier(identifier);
//            definition.SetBlock(blockStatement);

//            ProgramRoot expectedRoot = new ProgramRoot();
//            expectedRoot.SetDefinitions(new List<Definition>() { definition });
//            #endregion
//            #region input-setup
//            Stack<Token> inputTokens = new Stack<Token>();
//            inputTokens.Push(new WritableToken(TokenType.EOF));
//            inputTokens.Push(new WritableToken(TokenType.Colon));
//            inputTokens.Push(new WritableToken(TokenType.RightParanthesis));
//            inputTokens.Push(new WritableToken(TokenType.LeftParanthesis));
//            inputTokens.Push(new WritableToken(TokenType.Identifier));
//            inputTokens.Push(new WritableToken(TokenType.Define));
//            #endregion

//            Mock<IScanner> scanner = new Mock<IScanner>();
//            scanner.Setup(s => s.NextToken())
//                   .Callback(() => scanner.Setup(s => s.currentToken)
//                                          .Returns(inputTokens.Pop())
//                   );
//            Parser parser = new Parser(scanner.Object);
//            var ex = Assert.Throws<ParseException>(() => parser.Parse());
//            Assert.That(ex.Message.EndsWith("Expected left curly bracket"));
//        }
//        [Test]
//        public void IfStatement()
//        {
//            #region tree-setup
//            // int in condition
//            unaryExpression.SetExpression(intValue.Object);
//            multiplicativeExpression.SetLeft(unaryExpression);
//            additiveExpression.SetLeft(multiplicativeExpression);
//            // assignable in condition
//            UnaryExpression unaryExpression_assignable = new UnaryExpression();
//            MultiplicativeExpression multiplicativeExpression_assignable = new MultiplicativeExpression();
//            AdditiveExpression additiveExpression_assignable = new AdditiveExpression();
//            unaryExpression_assignable.SetExpression(assignable);
//            multiplicativeExpression_assignable.SetLeft(unaryExpression_assignable);
//            additiveExpression_assignable.SetLeft(multiplicativeExpression_assignable);
//            // condition
//            RelationExpression condition = new RelationExpression();
//            condition.SetLeft(additiveExpression_assignable);
//            condition.SetOperation(TokenType.Greater);
//            condition.SetRight(additiveExpression);
//            logicExpression.SetLeft(condition);
//            // if body - return
//            relationExpression.SetLeft(additiveExpression);
//            LogicExpression logicExpression_return = new LogicExpression();
//            logicExpression_return.SetLeft(relationExpression);
//            returnStatement.SetExpression(logicExpression_return);
//            blockStatement.SetStatements(new List<Statement>() { returnStatement });
//            // ifstatement
//            ifStatement.SetCondition(logicExpression);
//            ifStatement.SetTrueBlockStatement(blockStatement);

//            ProgramRoot expectedRoot = new ProgramRoot();
//            expectedRoot.SetStatements(new List<Statement>() { ifStatement });
//            #endregion
//            #region input-setup
//            Stack<Token> inputTokens = new Stack<Token>();
//            inputTokens.Push(new WritableToken(TokenType.EOF));
//            inputTokens.Push(new WritableToken(TokenType.RightCurlyBracket));
//            inputTokens.Push(new WritableToken(TokenType.Int));
//            inputTokens.Push(new WritableToken(TokenType.Return));
//            inputTokens.Push(new WritableToken(TokenType.LeftCurlyBracket));
//            inputTokens.Push(new WritableToken(TokenType.Colon));
//            inputTokens.Push(new WritableToken(TokenType.RightParanthesis));
//            inputTokens.Push(new WritableToken(TokenType.Int));
//            inputTokens.Push(new WritableToken(TokenType.Greater));
//            inputTokens.Push(new WritableToken(TokenType.Identifier));
//            inputTokens.Push(new WritableToken(TokenType.LeftParanthesis));
//            inputTokens.Push(new WritableToken(TokenType.If));
//            #endregion

//            Mock<IScanner> scanner = new Mock<IScanner>();
//            scanner.Setup(s => s.NextToken())
//                   .Callback(() => scanner.Setup(s => s.currentToken)
//                                          .Returns(inputTokens.Pop())
//                   );
//            Parser parser = new Parser(scanner.Object);
//            ProgramRoot pr = parser.Parse();
//            Assert.AreEqual(expectedRoot, pr);
//        }
//        [Test]
//        public void IfElseStatement()
//        {
//            #region tree-setup
//            // int in condition
//            unaryExpression.SetExpression(intValue.Object);
//            multiplicativeExpression.SetLeft(unaryExpression);
//            additiveExpression.SetLeft(multiplicativeExpression);
//            // assignable in condition
//            UnaryExpression unaryExpression_assignable = new UnaryExpression();
//            MultiplicativeExpression multiplicativeExpression_assignable = new MultiplicativeExpression();
//            AdditiveExpression additiveExpression_assignable = new AdditiveExpression();
//            unaryExpression_assignable.SetExpression(assignable);
//            multiplicativeExpression_assignable.SetLeft(unaryExpression_assignable);
//            additiveExpression_assignable.SetLeft(multiplicativeExpression_assignable);
//            // condition
//            RelationExpression condition = new RelationExpression();
//            condition.SetLeft(additiveExpression_assignable);
//            condition.SetOperation(TokenType.Greater);
//            condition.SetRight(additiveExpression);
//            logicExpression.SetLeft(condition);
//            // if body - return
//            relationExpression.SetLeft(additiveExpression);
//            LogicExpression logicExpression_return = new LogicExpression();
//            logicExpression_return.SetLeft(relationExpression);
//            returnStatement.SetExpression(logicExpression_return);
//            blockStatement.SetStatements(new List<Statement>() { returnStatement });
//            // ifstatement
//            ifStatement.SetCondition(logicExpression);
//            ifStatement.SetTrueBlockStatement(blockStatement);
//            ifStatement.SetFalseBlockStatement(blockStatement);

//            ProgramRoot expectedRoot = new ProgramRoot();
//            expectedRoot.SetStatements(new List<Statement>() { ifStatement });
//            #endregion
//            #region input-setup
//            Stack<Token> inputTokens = new Stack<Token>();
//            inputTokens.Push(new WritableToken(TokenType.EOF));
//            inputTokens.Push(new WritableToken(TokenType.RightCurlyBracket));
//            inputTokens.Push(new WritableToken(TokenType.Int));
//            inputTokens.Push(new WritableToken(TokenType.Return));
//            inputTokens.Push(new WritableToken(TokenType.LeftCurlyBracket));
//            inputTokens.Push(new WritableToken(TokenType.Colon));
//            inputTokens.Push(new WritableToken(TokenType.Else));
//            inputTokens.Push(new WritableToken(TokenType.RightCurlyBracket));
//            inputTokens.Push(new WritableToken(TokenType.Int));
//            inputTokens.Push(new WritableToken(TokenType.Return));
//            inputTokens.Push(new WritableToken(TokenType.LeftCurlyBracket));
//            inputTokens.Push(new WritableToken(TokenType.Colon));
//            inputTokens.Push(new WritableToken(TokenType.RightParanthesis));
//            inputTokens.Push(new WritableToken(TokenType.Int));
//            inputTokens.Push(new WritableToken(TokenType.Greater));
//            inputTokens.Push(new WritableToken(TokenType.Identifier));
//            inputTokens.Push(new WritableToken(TokenType.LeftParanthesis));
//            inputTokens.Push(new WritableToken(TokenType.If));
//            #endregion

//            Mock<IScanner> scanner = new Mock<IScanner>();
//            scanner.Setup(s => s.NextToken())
//                   .Callback(() => scanner.Setup(s => s.currentToken)
//                                          .Returns(inputTokens.Pop())
//                   );
//            Parser parser = new Parser(scanner.Object);
//            ProgramRoot pr = parser.Parse();
//            Assert.AreEqual(expectedRoot, pr);
//        }
//        [Test]
//        public void IfWithoutConditionThrowsException()
//        {
//            #region tree-setup
//            unaryExpression.SetExpression(intValue.Object);
//            multiplicativeExpression.SetLeft(unaryExpression);
//            additiveExpression.SetLeft(multiplicativeExpression);
//            relationExpression.SetLeft(additiveExpression);
//            LogicExpression logicExpression_return = new LogicExpression();
//            logicExpression_return.SetLeft(relationExpression);
//            returnStatement.SetExpression(logicExpression_return);
//            blockStatement.SetStatements(new List<Statement>() { returnStatement });
//            ifStatement.SetTrueBlockStatement(blockStatement);

//            ProgramRoot expectedRoot = new ProgramRoot();
//            expectedRoot.SetStatements(new List<Statement>() { ifStatement });
//            #endregion
//            #region input-setup
//            Stack<Token> inputTokens = new Stack<Token>();
//            inputTokens.Push(new WritableToken(TokenType.EOF));
//            inputTokens.Push(new WritableToken(TokenType.RightCurlyBracket));
//            inputTokens.Push(new WritableToken(TokenType.Int));
//            inputTokens.Push(new WritableToken(TokenType.Return));
//            inputTokens.Push(new WritableToken(TokenType.LeftCurlyBracket));
//            inputTokens.Push(new WritableToken(TokenType.Colon));
//            inputTokens.Push(new WritableToken(TokenType.RightParanthesis));
//            inputTokens.Push(new WritableToken(TokenType.LeftParanthesis));
//            inputTokens.Push(new WritableToken(TokenType.If));
//            #endregion

//            Mock<IScanner> scanner = new Mock<IScanner>();
//            scanner.Setup(s => s.NextToken())
//                   .Callback(() => scanner.Setup(s => s.currentToken)
//                                          .Returns(inputTokens.Pop())
//                   );
//            Parser parser = new Parser(scanner.Object);
//            var ex = Assert.Throws<ParseException>(() => parser.Parse());
//            Assert.That(ex.Message.EndsWith("Expected condition"));
//        }
//        [Test]
//        public void WhileStatement()
//        {
//            #region tree-setup
//            // int in condition
//            unaryExpression.SetExpression(intValue.Object);
//            multiplicativeExpression.SetLeft(unaryExpression);
//            additiveExpression.SetLeft(multiplicativeExpression);
//            // assignable in condition
//            UnaryExpression unaryExpression_assignable = new UnaryExpression();
//            MultiplicativeExpression multiplicativeExpression_assignable = new MultiplicativeExpression();
//            AdditiveExpression additiveExpression_assignable = new AdditiveExpression();
//            unaryExpression_assignable.SetExpression(assignable);
//            multiplicativeExpression_assignable.SetLeft(unaryExpression_assignable);
//            additiveExpression_assignable.SetLeft(multiplicativeExpression_assignable);
//            // condition
//            RelationExpression condition = new RelationExpression();
//            condition.SetLeft(additiveExpression_assignable);
//            condition.SetOperation(TokenType.Greater);
//            condition.SetRight(additiveExpression);
//            logicExpression.SetLeft(condition);
//            // if body - return
//            relationExpression.SetLeft(additiveExpression);
//            LogicExpression logicExpression_return = new LogicExpression();
//            logicExpression_return.SetLeft(relationExpression);
//            returnStatement.SetExpression(logicExpression_return);
//            blockStatement.SetStatements(new List<Statement>() { returnStatement });
//            // ifstatement
//            whileStatement.SetCondition(logicExpression);
//            whileStatement.SetBlockStatement(blockStatement);

//            ProgramRoot expectedRoot = new ProgramRoot();
//            expectedRoot.SetStatements(new List<Statement>() { whileStatement });
//            #endregion
//            #region input-setup
//            Stack<Token> inputTokens = new Stack<Token>();
//            inputTokens.Push(new WritableToken(TokenType.EOF));
//            inputTokens.Push(new WritableToken(TokenType.RightCurlyBracket));
//            inputTokens.Push(new WritableToken(TokenType.Int));
//            inputTokens.Push(new WritableToken(TokenType.Return));
//            inputTokens.Push(new WritableToken(TokenType.LeftCurlyBracket));
//            inputTokens.Push(new WritableToken(TokenType.Colon));
//            inputTokens.Push(new WritableToken(TokenType.RightParanthesis));
//            inputTokens.Push(new WritableToken(TokenType.Int));
//            inputTokens.Push(new WritableToken(TokenType.Greater));
//            inputTokens.Push(new WritableToken(TokenType.Identifier));
//            inputTokens.Push(new WritableToken(TokenType.LeftParanthesis));
//            inputTokens.Push(new WritableToken(TokenType.While));
//            #endregion

//            Mock<IScanner> scanner = new Mock<IScanner>();
//            scanner.Setup(s => s.NextToken())
//                   .Callback(() => scanner.Setup(s => s.currentToken)
//                                          .Returns(inputTokens.Pop())
//                   );
//            Parser parser = new Parser(scanner.Object);
//            ProgramRoot pr = parser.Parse();
//            Assert.AreEqual(expectedRoot, pr);
//        }
//        [Test]
//        public void WhileWithoutConditionThrowsException()
//        {
//            #region tree-setup
//            unaryExpression.SetExpression(intValue.Object);
//            multiplicativeExpression.SetLeft(unaryExpression);
//            additiveExpression.SetLeft(multiplicativeExpression);
//            relationExpression.SetLeft(additiveExpression);
//            LogicExpression logicExpression_return = new LogicExpression();
//            logicExpression_return.SetLeft(relationExpression);
//            returnStatement.SetExpression(logicExpression_return);
//            blockStatement.SetStatements(new List<Statement>() { returnStatement });
//            whileStatement.SetBlockStatement(blockStatement);

//            ProgramRoot expectedRoot = new ProgramRoot();
//            expectedRoot.SetStatements(new List<Statement>() { whileStatement });
//            #endregion
//            #region input-setup
//            Stack<Token> inputTokens = new Stack<Token>();
//            inputTokens.Push(new WritableToken(TokenType.EOF));
//            inputTokens.Push(new WritableToken(TokenType.RightCurlyBracket));
//            inputTokens.Push(new WritableToken(TokenType.Int));
//            inputTokens.Push(new WritableToken(TokenType.Return));
//            inputTokens.Push(new WritableToken(TokenType.LeftCurlyBracket));
//            inputTokens.Push(new WritableToken(TokenType.Colon));
//            inputTokens.Push(new WritableToken(TokenType.RightParanthesis));
//            inputTokens.Push(new WritableToken(TokenType.LeftParanthesis));
//            inputTokens.Push(new WritableToken(TokenType.While));
//            #endregion

//            Mock<IScanner> scanner = new Mock<IScanner>();
//            scanner.Setup(s => s.NextToken())
//                   .Callback(() => scanner.Setup(s => s.currentToken)
//                                          .Returns(inputTokens.Pop())
//                   );
//            Parser parser = new Parser(scanner.Object);
//            var ex = Assert.Throws<ParseException>(() => parser.Parse());
//            Assert.That(ex.Message.EndsWith("Expected condition"));
//        }
//        [Test]
//        public void ReturnExpression()
//        {
//            #region tree-setup
//            unaryExpression.SetExpression(assignable);
//            multiplicativeExpression.SetLeft(unaryExpression);
//            additiveExpression.SetLeft(multiplicativeExpression);
//            relationExpression.SetLeft(additiveExpression);
//            logicExpression.SetLeft(relationExpression);
//            returnStatement.SetExpression(logicExpression);

//            ProgramRoot expectedRoot = new ProgramRoot();
//            expectedRoot.SetStatements(new List<Statement>() { returnStatement });
//            #endregion
//            #region input-setup
//            Stack<Token> inputTokens = new Stack<Token>();
//            inputTokens.Push(new WritableToken(TokenType.EOF));
//            inputTokens.Push(new WritableToken(TokenType.Identifier));
//            inputTokens.Push(new WritableToken(TokenType.Return));
//            #endregion

//            Mock<IScanner> scanner = new Mock<IScanner>();
//            scanner.Setup(s => s.NextToken())
//                   .Callback(() => scanner.Setup(s => s.currentToken)
//                                          .Returns(inputTokens.Pop())
//                   );
//            Parser parser = new Parser(scanner.Object);
//            ProgramRoot pr = parser.Parse();
//            Assert.AreEqual(expectedRoot, pr);
//        }
//        [Test]
//        public void Return()
//        {
//            #region tree-setup
//            ProgramRoot expectedRoot = new ProgramRoot();
//            expectedRoot.SetStatements(new List<Statement>() { returnStatement });
//            #endregion
//            #region input-setup
//            Stack<Token> inputTokens = new Stack<Token>();
//            inputTokens.Push(new WritableToken(TokenType.EOF));
//            inputTokens.Push(new WritableToken(TokenType.Return));
//            #endregion

//            Mock<IScanner> scanner = new Mock<IScanner>();
//            scanner.Setup(s => s.NextToken())
//                   .Callback(() => scanner.Setup(s => s.currentToken)
//                                          .Returns(inputTokens.Pop())
//                   );
//            Parser parser = new Parser(scanner.Object);
//            ProgramRoot pr = parser.Parse();
//            Assert.AreEqual(expectedRoot, pr);
//        }
//        [Test]
//        public void CallStatement()
//        {
//            #region tree-setup
//            Assignable expectedAssignable = new Assignable(new Identifier(new WritableToken(TokenType.Identifier)));
//            FunctionCallStatement expectedCall = new Call();
//            expectedCall.SetAssignable(expectedAssignable);
//            ProgramRoot expectedRoot = new ProgramRoot();
//            expectedRoot.SetStatements(new List<Statement>() { expectedCall });
//            #endregion
//            #region input-setup
//            Stack<Token> inputTokens = new Stack<Token>();
//            inputTokens.Push(new WritableToken(TokenType.EOF));
//            inputTokens.Push(new WritableToken(TokenType.RightParanthesis));
//            inputTokens.Push(new WritableToken(TokenType.LeftParanthesis));
//            inputTokens.Push(new WritableToken(TokenType.Identifier));
//            #endregion

//            Mock<IScanner> scanner = new Mock<IScanner>();
//            scanner.Setup(s => s.NextToken())
//                   .Callback(() => scanner.Setup(s => s.currentToken)
//                                          .Returns(inputTokens.Pop())
//                   );
//            Parser parser = new Parser(scanner.Object);
//            ProgramRoot pr = parser.Parse();
//            Assert.AreEqual(expectedRoot, pr);
//        }
//        [Test]
//        public void CallWithArguments()
//        {

//            #region tree-setup
//            Assignable expectedAssignable = new Assignable(new Identifier(new WritableToken(TokenType.Identifier)));
//            unaryExpression.SetExpression(intValue.Object);
//            multiplicativeExpression.SetLeft(unaryExpression);
//            additiveExpression.SetLeft(multiplicativeExpression);
//            relationExpression.SetLeft(additiveExpression);
//            logicExpression.SetLeft(relationExpression);
//            FunctionCallStatement expectedCall = new Call();
//            expectedCall.SetAssignable(expectedAssignable);
//            expectedCall.SetArguments(new List<Expression>() { logicExpression, logicExpression });
//            ProgramRoot expectedRoot = new ProgramRoot();
//            expectedRoot.SetStatements(new List<Statement>() { expectedCall });
//            #endregion
//            #region input-setup
//            Stack<Token> inputTokens = new Stack<Token>();
//            inputTokens.Push(new WritableToken(TokenType.EOF));
//            inputTokens.Push(new WritableToken(TokenType.RightParanthesis));
//            inputTokens.Push(new WritableToken(TokenType.Int));
//            inputTokens.Push(new WritableToken(TokenType.Comma));
//            inputTokens.Push(new WritableToken(TokenType.Int));
//            inputTokens.Push(new WritableToken(TokenType.LeftParanthesis));
//            inputTokens.Push(new WritableToken(TokenType.Identifier));
//            #endregion

//            Mock<IScanner> scanner = new Mock<IScanner>();
//            scanner.Setup(s => s.NextToken())
//                   .Callback(() => scanner.Setup(s => s.currentToken)
//                                          .Returns(inputTokens.Pop())
//                   );
//            Parser parser = new Parser(scanner.Object);
//            ProgramRoot pr = parser.Parse();
//            Assert.AreEqual(expectedRoot, pr);
//        }
//        [Test]
//        public void AssignStatement() 
//        {
//            #region tree-setup
//            Assignable expectedAssignable = new Assignable(new Identifier(new WritableToken(TokenType.Identifier)));
//            unaryExpression.SetExpression(intValue.Object);
//            multiplicativeExpression.SetLeft(unaryExpression);
//            additiveExpression.SetLeft(multiplicativeExpression);
//            relationExpression.SetLeft(additiveExpression);
//            logicExpression.SetLeft(relationExpression);
//            AssignStatement expectedAssignStatement = new AssignStatement();
//            expectedAssignStatement.SetLeft(expectedAssignable);
//            expectedAssignStatement.SetRight(logicExpression);
//            ProgramRoot expectedRoot = new ProgramRoot();
//            expectedRoot.SetStatements(new List<Statement>() { expectedAssignStatement });
//            #endregion
//            #region input-setup
//            Stack<Token> inputTokens = new Stack<Token>();
//            inputTokens.Push(new WritableToken(TokenType.EOF));
//            inputTokens.Push(new WritableToken(TokenType.Int));
//            inputTokens.Push(new WritableToken(TokenType.Assign));
//            inputTokens.Push(new WritableToken(TokenType.Identifier));
//            #endregion

//            Mock<IScanner> scanner = new Mock<IScanner>();
//            scanner.Setup(s => s.NextToken())
//                   .Callback(() => scanner.Setup(s => s.currentToken)
//                                          .Returns(inputTokens.Pop())
//                   );
//            Parser parser = new Parser(scanner.Object);
//            ProgramRoot pr = parser.Parse();
//            Assert.AreEqual(expectedRoot, pr);
//        }
//        [Test]
//        public void LogicExpression()
//        {

//            #region expected-setup
//            unaryExpression.SetExpression(boolValue.Object);
//            multiplicativeExpression.SetLeft(unaryExpression);
//            additiveExpression.SetLeft(multiplicativeExpression);
//            relationExpression.SetLeft(additiveExpression);
//            logicExpression.SetLeft(relationExpression);
//            logicExpression.SetRight(relationExpression);
//            #endregion
//            #region input-setup
//            Stack<Token> inputTokens = new Stack<Token>();
//            inputTokens.Push(new WritableToken(TokenType.EOF));
//            inputTokens.Push(new WritableToken(TokenType.Bool));
//            inputTokens.Push(new WritableToken(TokenType.And));
//            inputTokens.Push(new WritableToken(TokenType.Bool));
//            inputTokens.Push(new WritableToken(TokenType.Bool));
//            inputTokens.Push(new WritableToken(TokenType.Or));
//            inputTokens.Push(new WritableToken(TokenType.Bool));
//            #endregion

//            Mock<IScanner> scanner = new Mock<IScanner>();
//            scanner.Setup(s => s.NextToken())
//                           .Callback(() => scanner.Setup(s => s.currentToken)
//                                                  .Returns(inputTokens.Pop())
//                           );
//            Parser parser = new Parser(scanner.Object);
//            scanner.Object.NextToken(); // to set scanner.current to first token

//            logicExpression.SetOperation(TokenType.Or);
//            Assert.AreEqual(logicExpression, parser.ParseLogicExpression());
//            logicExpression.SetOperation(TokenType.And);
//            Assert.AreEqual(logicExpression, parser.ParseLogicExpression());

//        }
//        [Test]
//        public void RelationExpression()
//        {
//            #region expected-setup
//            unaryExpression.SetExpression(intValue.Object);
//            multiplicativeExpression.SetLeft(unaryExpression);
//            additiveExpression.SetLeft(multiplicativeExpression);
//            relationExpression.SetLeft(additiveExpression);
//            relationExpression.SetRight(additiveExpression);
//            #endregion
//            #region input-setup
//            Stack<Token> inputTokens = new Stack<Token>();
//            inputTokens.Push(new WritableToken(TokenType.EOF));
//            inputTokens.Push(new WritableToken(TokenType.Int));
//            inputTokens.Push(new WritableToken(TokenType.LessEqual));
//            inputTokens.Push(new WritableToken(TokenType.Int));
//            inputTokens.Push(new WritableToken(TokenType.Int));
//            inputTokens.Push(new WritableToken(TokenType.Less));
//            inputTokens.Push(new WritableToken(TokenType.Int));
//            inputTokens.Push(new WritableToken(TokenType.Int));
//            inputTokens.Push(new WritableToken(TokenType.NotEqual));
//            inputTokens.Push(new WritableToken(TokenType.Int));
//            inputTokens.Push(new WritableToken(TokenType.Int));
//            inputTokens.Push(new WritableToken(TokenType.Equal));
//            inputTokens.Push(new WritableToken(TokenType.Int));
//            inputTokens.Push(new WritableToken(TokenType.Int));
//            inputTokens.Push(new WritableToken(TokenType.GreaterEqual));
//            inputTokens.Push(new WritableToken(TokenType.Int));
//            inputTokens.Push(new WritableToken(TokenType.Int));
//            inputTokens.Push(new WritableToken(TokenType.Greater));
//            inputTokens.Push(new WritableToken(TokenType.Int));
//            #endregion

//            Mock<IScanner> scanner = new Mock<IScanner>();
//            scanner.Setup(s => s.NextToken())
//                   .Callback(() => scanner.Setup(s => s.currentToken)
//                                          .Returns(inputTokens.Pop())
//                   );
//            Parser parser = new Parser(scanner.Object);
//            scanner.Object.NextToken(); // to set scanner.current to first token

//            relationExpression.SetOperation(TokenType.Greater);
//            Assert.AreEqual(relationExpression, parser.ParseRelationExpression());
//            relationExpression.SetOperation(TokenType.GreaterEqual);
//            Assert.AreEqual(relationExpression, parser.ParseRelationExpression());
//            relationExpression.SetOperation(TokenType.Equal);
//            Assert.AreEqual(relationExpression, parser.ParseRelationExpression());
//            relationExpression.SetOperation(TokenType.NotEqual);
//            Assert.AreEqual(relationExpression, parser.ParseRelationExpression());
//            relationExpression.SetOperation(TokenType.Less);
//            Assert.AreEqual(relationExpression, parser.ParseRelationExpression());
//            relationExpression.SetOperation(TokenType.LessEqual);
//            Assert.AreEqual(relationExpression, parser.ParseRelationExpression());
//        }
//        [Test]
//        public void AdditiveExpression()
//        {

//            #region expected-setup
//            unaryExpression.SetExpression(intValue.Object);
//            multiplicativeExpression.SetLeft(unaryExpression);
//            additiveExpression.SetLeft(multiplicativeExpression);
//            additiveExpression.SetRight(multiplicativeExpression);
//            #endregion
//            #region input-setup
//            Stack<Token> inputTokens = new Stack<Token>();
//            inputTokens.Push(new WritableToken(TokenType.EOF));
//            inputTokens.Push(new WritableToken(TokenType.Int));
//            inputTokens.Push(new WritableToken(TokenType.Minus));
//            inputTokens.Push(new WritableToken(TokenType.Int));
//            inputTokens.Push(new WritableToken(TokenType.Int));
//            inputTokens.Push(new WritableToken(TokenType.Plus));
//            inputTokens.Push(new WritableToken(TokenType.Int));
//            #endregion

//            Mock<IScanner> scanner = new Mock<IScanner>();
//            scanner.Setup(s => s.NextToken())
//                   .Callback(() => scanner.Setup(s => s.currentToken)
//                                          .Returns(inputTokens.Pop())
//                   );
//            Parser parser = new Parser(scanner.Object);
//            scanner.Object.NextToken(); // to set scanner.current to first token

//            additiveExpression.SetOperation(TokenType.Plus);
//            Assert.AreEqual(additiveExpression, parser.ParseAdditiveExpression());
//            additiveExpression.SetOperation(TokenType.Minus);
//            Assert.AreEqual(additiveExpression, parser.ParseAdditiveExpression());

//        }
//        [Test]
//        public void MultiplicativeExpression()
//        {

//            #region expected-setup
//            unaryExpression.SetExpression(intValue.Object);
//            multiplicativeExpression.SetLeft(unaryExpression);
//            multiplicativeExpression.SetRight(unaryExpression);
//            #endregion
//            #region input-setup
//            Stack<Token> inputTokens = new Stack<Token>();
//            inputTokens.Push(new WritableToken(TokenType.EOF));
//            inputTokens.Push(new WritableToken(TokenType.Int));
//            inputTokens.Push(new WritableToken(TokenType.Divide));
//            inputTokens.Push(new WritableToken(TokenType.Int));
//            inputTokens.Push(new WritableToken(TokenType.Int));
//            inputTokens.Push(new WritableToken(TokenType.Multiply));
//            inputTokens.Push(new WritableToken(TokenType.Int));
//            #endregion

//            Mock<IScanner> scanner = new Mock<IScanner>();
//            scanner.Setup(s => s.NextToken())
//                   .Callback(() => scanner.Setup(s => s.currentToken)
//                                          .Returns(inputTokens.Pop())
//                   );
//            Parser parser = new Parser(scanner.Object);
//            scanner.Object.NextToken(); // to set scanner.current to first token

//            multiplicativeExpression.SetOperation(TokenType.Multiply);
//            Assert.AreEqual(multiplicativeExpression, parser.ParseMultiplicativeExpression());
//            multiplicativeExpression.SetOperation(TokenType.Divide);
//            Assert.AreEqual(multiplicativeExpression, parser.ParseMultiplicativeExpression());

//        }
//        // TO RETHINK UNARY EXPRESSION:
//        //[Test]
//        //public void ReturnUnaryExpression() { }
//        [Test]
//        public void SimpleExpression()
//        {
//            #region expected-setup
//            IntValue expectedIntValue = new IntValue(new WritableToken(TokenType.Int));
//            DoubleValue expectedDoubleValue = new DoubleValue(new WritableToken(TokenType.Double).WithValue("1.1"));
//            StringValue expectedStringValue = new StringValue(new WritableToken(TokenType.String));
//            Assignable expectedAssignable = new Assignable(new Identifier(new WritableToken(TokenType.Identifier)));
//            Assignable expectedAssignableWithComponent = new Assignable(new Identifier(new WritableToken(TokenType.Identifier)));
//            expectedAssignableWithComponent.AddIdentifier(new Identifier(new WritableToken(TokenType.Identifier)));
//            FunctionCallStatement expectedCall = new Call();
//            expectedCall.SetAssignable(expectedAssignableWithComponent);
//            #endregion
//            #region input-setup
//            Stack<Token> inputTokens = new Stack<Token>();
//            inputTokens.Push(new WritableToken(TokenType.EOF));
//            // call ("a.a()")
//            inputTokens.Push(new WritableToken(TokenType.RightParanthesis));
//            inputTokens.Push(new WritableToken(TokenType.LeftParanthesis));
//            inputTokens.Push(new WritableToken(TokenType.Identifier));
//            inputTokens.Push(new WritableToken(TokenType.Dot));
//            inputTokens.Push(new WritableToken(TokenType.Identifier));
//            // ident.ident
//            inputTokens.Push(new WritableToken(TokenType.Identifier));
//            inputTokens.Push(new WritableToken(TokenType.Dot));
//            inputTokens.Push(new WritableToken(TokenType.Identifier));
//            // ident
//            inputTokens.Push(new WritableToken(TokenType.Identifier));
//            // string
//            inputTokens.Push(new WritableToken(TokenType.String));
//            // double
//            inputTokens.Push(new WritableToken(TokenType.Double).WithValue("1.1"));
//            // int
//            inputTokens.Push(new WritableToken(TokenType.Int));
//            #endregion

//            Mock<IScanner> scanner = new Mock<IScanner>();
//            scanner.Setup(s => s.NextToken())
//                   .Callback(() => scanner.Setup(s => s.currentToken)
//                                          .Returns(inputTokens.Pop())
//                   );
//            Parser parser = new Parser(scanner.Object);
//            scanner.Object.NextToken(); // to set scanner.current to first token

//            Assert.AreEqual(expectedIntValue, parser.ParseValueExpression());
//            Assert.AreEqual(expectedDoubleValue, parser.ParseValueExpression());
//            Assert.AreEqual(expectedStringValue, parser.ParseValueExpression());
//            Assert.AreEqual(expectedAssignable, parser.ParseValueExpression());
//            Assert.AreEqual(expectedAssignableWithComponent, parser.ParseValueExpression());
//            Assert.AreEqual(expectedCall, parser.ParseValueExpression());
//        }
//    }
//}