//TODO: NAPRAWIĆ TESTY
//using System;
//using System.Collections.Generic;
//using NUnit.Framework;
//using BasiliskLang.Tokens;
//using BasiliskLang;

//namespace Tests
//{
//    public class Tests
//    {
//        Scanner scanner;

//        [Test]
//        public void TokenizeKeywords()
//        {
//            List<Token> expectedTokens = new List<Token>();
//            expectedTokens.Add(new Token(TokenType.If, 0, 0));
//            expectedTokens.Add(new Token(TokenType.Else, 3, 0));
//            expectedTokens.Add(new Token(TokenType.While, 8, 0));
//            expectedTokens.Add(new Token(TokenType.Define, 14, 0));
//            expectedTokens.Add(new Token(TokenType.Return, 18, 0));
//            expectedTokens.Add(new Token(TokenType.Print, 25, 0));

//            string input = "If Else While Def Return Print";
//            Scanner scanner = new Scanner(input, InputType.String);
//            scanner.Scan();
//            for(int i=0; i<expectedTokens.Count; i++)
//            {
//                Assert.AreEqual(expectedTokens[i].lineNumber, scanner.tokens[i].lineNumber);
//                Assert.AreEqual(expectedTokens[i].position, scanner.tokens[i].position);
//                Assert.AreEqual(expectedTokens[i].type, scanner.tokens[i].type);
//                Assert.AreEqual(expectedTokens[i].value, scanner.tokens[i].value);
//            }
            
//        }

//        [Test]
//        public void TokenizeTypes()
//        {
//            List<Token> expectedTokens = new List<Token>();
//            expectedTokens.Add(new Token(TokenType.String, 0, 0, "a"));
//            expectedTokens.Add(new Token(TokenType.String, 2, 0, "ab"));
//            expectedTokens.Add(new Token(TokenType.String, 5, 0, "ab3"));
//            expectedTokens.Add(new Token(TokenType.Int, 9, 0, "12"));
//            expectedTokens.Add(new Token(TokenType.Double, 12, 0, "1.1"));

//            string input = "a ab ab3 12 1.1";
//            Scanner scanner = new Scanner(input, InputType.String);
//            scanner.Scan();

//            for (int i = 0; i < expectedTokens.Count; i++)
//            {
//                Assert.AreEqual(expectedTokens[i].lineNumber, scanner.tokens[i].lineNumber);
//                Assert.AreEqual(expectedTokens[i].position, scanner.tokens[i].position);
//                Assert.AreEqual(expectedTokens[i].type, scanner.tokens[i].type);
//                Assert.AreEqual(expectedTokens[i].value, scanner.tokens[i].value);
//            }
//        }

//        [Test]
//        public void TokenizeSigns()
//        {
//            List<Token> expectedTokens = new List<Token>();
//            expectedTokens.Add(new Token(TokenType.Plus, 0, 0));
//            expectedTokens.Add(new Token(TokenType.Minus, 2, 0));
//            expectedTokens.Add(new Token(TokenType.Multiply, 4, 0));
//            expectedTokens.Add(new Token(TokenType.Divide, 6, 0));
//            expectedTokens.Add(new Token(TokenType.LeftParanthesis, 8, 0));
//            expectedTokens.Add(new Token(TokenType.RightParanthesis, 10, 0));
//            expectedTokens.Add(new Token(TokenType.LeftCurlyBracket, 12, 0));
//            expectedTokens.Add(new Token(TokenType.RightCurlyBracket, 14, 0));
//            expectedTokens.Add(new Token(TokenType.Comma, 16, 0));
//            expectedTokens.Add(new Token(TokenType.Dot, 18, 0));
//            expectedTokens.Add(new Token(TokenType.Colon, 20, 0));
//            expectedTokens.Add(new Token(TokenType.Assign, 22, 0));
//            expectedTokens.Add(new Token(TokenType.Equal, 24, 0));
//            expectedTokens.Add(new Token(TokenType.NotEqual, 27, 0));
//            expectedTokens.Add(new Token(TokenType.Less, 30, 0));
//            expectedTokens.Add(new Token(TokenType.LessEqual, 32, 0));
//            expectedTokens.Add(new Token(TokenType.Greater, 35, 0));
//            expectedTokens.Add(new Token(TokenType.GreaterEqual, 37, 0));

//            string input = "+ - * / ( ) { } , . : = == != < <= > >=";
//            Scanner scanner = new Scanner(input, InputType.String);
//            scanner.Scan();

//            for (int i = 0; i < expectedTokens.Count; i++)
//            {
//                Assert.AreEqual(expectedTokens[i].lineNumber, scanner.tokens[i].lineNumber);
//                Assert.AreEqual(expectedTokens[i].position, scanner.tokens[i].position);
//                Assert.AreEqual(expectedTokens[i].type, scanner.tokens[i].type);
//                Assert.AreEqual(expectedTokens[i].value, scanner.tokens[i].value);
//            }
//        }
//        // "\""
//    }
//}