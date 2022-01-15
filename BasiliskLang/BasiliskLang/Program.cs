using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasiliskLang
{
    class Program
    {
        public static void PrintNode(Node node)
        {
            Console.WriteLine(node.type + " \tchildren: " + node.children.Count);
            foreach (Node child in node.children)
                PrintNode(child);
        }
        public static void Main()
        {
            string source = Path.GetFullPath("..\\..\\..\\..\\Programs\\test.txt");
            IReader reader;
            using (StreamReader sr = new StreamReader(source))
            {
                reader = new FileReader(sr);
                Scanner scanner = new Scanner(reader);
                //scanner.NextToken();
                //while (scanner.currentToken.type != Tokens.TokenType.EOF)
                //{
                //    scanner.currentToken.PrintTokenInfo();
                //    scanner.NextToken();
                //}
                Parser parser = new Parser(scanner);
                ProgramRoot pr = parser.Parse();
                PrintNode(pr);

            }
        }

        
    }
}
