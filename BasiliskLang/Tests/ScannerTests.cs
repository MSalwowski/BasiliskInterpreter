using System;
using System.Collections.Generic;
using NUnit.Framework;
using BasiliskLang.Tokens;
using BasiliskLang;
using System.IO;

namespace Tests
{
    public class ScannerTests
    {
        [Test]
        public void KeywordsLoweAndUpperCase()
        {
            List<Token> expectedTokens = new List<Token>();
            expectedTokens.Add(new Token(TokenType.If, 0, 0));
            expectedTokens.Add(new Token(TokenType.Else, 1, 0));
            expectedTokens.Add(new Token(TokenType.While, 2, 0));
            expectedTokens.Add(new Token(TokenType.Define, 3, 0));
            expectedTokens.Add(new Token(TokenType.Return, 4, 0));
            expectedTokens.Add(new Token(TokenType.Invalid, 5, 0));
            expectedTokens.Add(new Token(TokenType.EOF, 5, 5));

            string input = "If\nelse\nwhile\nDef\nReturn\nPrint";
            using (StringReader sr = new StringReader(input))
            {
                IReader reader = new FileReader(sr);
                Scanner scanner = new Scanner(reader);
                int i = 0;
                do
                {
                    scanner.NextToken();
                    Assert.AreEqual(scanner.currentToken, expectedTokens[i]);
                    i++;
                } while (scanner.currentToken.type != TokenType.EOF);
            }
        }

        [Test]
        public void Comments() 
        {
            List<Token> expectedTokens = new List<Token>();
            expectedTokens.Add(new Token(TokenType.Identifier, 1, 0, "variable"));
            expectedTokens.Add(new Token(TokenType.Identifier, 3, 0, "sth_b4_comment"));
            expectedTokens.Add(new Token(TokenType.EOF, 5, 28));

            string input = "#starts_with_comment\nvariable\n###sth_after_comment\nsth_b4_comment#u_cant_see_me\n#n#comments_next_to_each_other\n#ends_with_comment_of_course";
            using (StringReader sr = new StringReader(input))
            {
                IReader reader = new FileReader(sr);
                Scanner scanner = new Scanner(reader);
                int i = 0;
                do
                {
                    scanner.NextToken();
                    Assert.AreEqual(scanner.currentToken, expectedTokens[i]);
                    i++;
                } while (scanner.currentToken.type != TokenType.EOF);
            }
        }
        [Test]
        public void StringsWithInvalids()
        {
            List<Token> expectedTokens = new List<Token>();
            expectedTokens.Add(new Token(TokenType.Invalid, 0, 0, "not_closed #not_closed"));
            expectedTokens.Add(new Token(TokenType.String, 1, 0, "a"));
            expectedTokens.Add(new Token(TokenType.Identifier, 1, 3, "b"));
            expectedTokens.Add(new Token(TokenType.String, 1, 4, "c"));
            expectedTokens.Add(new Token(TokenType.String, 2, 0, "a\"b\"c"));
            expectedTokens.Add(new Token(TokenType.String, 3, 0, "first_line\nsecond_line"));
            expectedTokens.Add(new Token(TokenType.String, 4, 0, "be4_tab\tafter_tab"));
            expectedTokens.Add(new Token(TokenType.EOF, 4, 19));

            string input = "\"not_closed #not_closed\n\"a\"b\"c\" #string_identifier_string\n\"a\\\"b\\\"c\" #string\n\"first_line\\nsecond_line\"\n\"be4_tab\tafter_tab\"";
            using (StringReader sr = new StringReader(input))
            {
                IReader reader = new FileReader(sr);
                Scanner scanner = new Scanner(reader);
                int i = 0;
                do
                {
                    scanner.NextToken();
                    Assert.AreEqual(scanner.currentToken, expectedTokens[i]);
                    i++;
                } while (scanner.currentToken.type != TokenType.EOF);
            }
        }
        [Test]
        public void TwoCharSigns()
        {
            List<Token> expectedTokens = new List<Token>();
            expectedTokens.Add(new Token(TokenType.Invalid, 0, 0, "!!"));
            expectedTokens.Add(new Token(TokenType.Invalid, 1, 0, "!a"));
            expectedTokens.Add(new Token(TokenType.Identifier, 1, 2, "bc"));
            expectedTokens.Add(new Token(TokenType.Invalid, 2, 0, "&c"));
            expectedTokens.Add(new Token(TokenType.Invalid, 3, 0, "|d"));
            expectedTokens.Add(new Token(TokenType.NotEqual, 4, 0));
            expectedTokens.Add(new Token(TokenType.Invalid, 4, 2, "!!"));
            expectedTokens.Add(new Token(TokenType.Equal, 4, 4));
            expectedTokens.Add(new Token(TokenType.Assign, 4, 6));
            expectedTokens.Add(new Token(TokenType.Invalid, 4, 7, "!!"));
            expectedTokens.Add(new Token(TokenType.EOF, 4, 9));

            string input = "!!\n!abc\n&c\n|d\n!=!!===!!";
            using (StringReader sr = new StringReader(input))
            {
                IReader reader = new FileReader(sr);
                Scanner scanner = new Scanner(reader);
                int i = 0;
                do
                {
                    scanner.NextToken();
                    Assert.AreEqual(scanner.currentToken, expectedTokens[i]);
                    i++;
                } while (scanner.currentToken.type != TokenType.EOF);
            }
        }
        [Test]
        public void NoInvalids()
        {
            List<Token> expectedTokens = new List<Token>();
            expectedTokens.Add(new Token(TokenType.Define, 0, 0));
            expectedTokens.Add(new Token(TokenType.Identifier, 0, 4, "Gun"));
            expectedTokens.Add(new Token(TokenType.LeftParanthesis, 0, 7));
            expectedTokens.Add(new Token(TokenType.Identifier, 0, 8, "ammo"));
            expectedTokens.Add(new Token(TokenType.RightParanthesis, 0, 12));
            expectedTokens.Add(new Token(TokenType.Colon, 0, 13));
            expectedTokens.Add(new Token(TokenType.LeftCurlyBracket, 1, 0));
            expectedTokens.Add(new Token(TokenType.If, 2, 0));
            expectedTokens.Add(new Token(TokenType.LeftParanthesis, 2, 2));
            expectedTokens.Add(new Token(TokenType.Identifier, 2, 3, "ammo"));
            expectedTokens.Add(new Token(TokenType.Greater, 2, 8));
            expectedTokens.Add(new Token(TokenType.Int, 2, 10, "0"));
            expectedTokens.Add(new Token(TokenType.RightParanthesis, 2, 11));
            expectedTokens.Add(new Token(TokenType.Colon, 2, 12));
            expectedTokens.Add(new Token(TokenType.LeftCurlyBracket, 3, 0));
            expectedTokens.Add(new Token(TokenType.Print, 4, 0));
            expectedTokens.Add(new Token(TokenType.LeftParanthesis, 4, 5));
            expectedTokens.Add(new Token(TokenType.String, 4, 6, "Hit!"));
            expectedTokens.Add(new Token(TokenType.RightParanthesis, 4, 12));
            expectedTokens.Add(new Token(TokenType.RightCurlyBracket, 5, 0));
            expectedTokens.Add(new Token(TokenType.RightCurlyBracket, 6, 0));
            expectedTokens.Add(new Token(TokenType.Identifier, 7, 0, "Gun"));
            expectedTokens.Add(new Token(TokenType.LeftParanthesis, 7, 3));
            expectedTokens.Add(new Token(TokenType.Int, 7, 4, "1"));
            expectedTokens.Add(new Token(TokenType.RightParanthesis, 7, 5));
            expectedTokens.Add(new Token(TokenType.EOF, 7, 6));

            string input = "def Gun(ammo):\n{\nif(ammo > 0):\n{\nprint(\"Hit!\")\n}\n}\nGun(1)";
            using (StringReader sr = new StringReader(input))
            {
                IReader reader = new FileReader(sr);
                Scanner scanner = new Scanner(reader);
                int i = 0;
                do
                {
                    scanner.NextToken();
                    Assert.AreEqual(scanner.currentToken, expectedTokens[i]);
                    i++;
                } while (scanner.currentToken.type != TokenType.EOF);
            }
        }
        // TODO: maybe add one test for numbers
    }
}