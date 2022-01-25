using BasiliskLang.Helpers;
using BasiliskLang.Interpreter;
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
            Console.Write(node.type + " \tchildren: " + node.children.Count);
            if (node.children != null)
                Console.WriteLine();
            foreach (Node child in node.children)
                if(child != null)
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
                IErrorHandler errorHandler = new ErrorHandler();
                Parser parser = new Parser(scanner, errorHandler);
                ProgramRoot pr = parser.Parse();
                IVisitor visitor = new Visitor();
                pr.Accept(visitor);
                //PrintNode(pr);
            }

            //DateTime a = new DateTime(2022, 2, 30, 14, 11, 10);
            //DateTime b = new DateTime(2021, 1, 1, 13, 10, 10);
            //Console.WriteLine(a);
            //Console.WriteLine(b);
            //var c = a - b;
            //Console.WriteLine(c);
            //Console.WriteLine(c.Hours);

            //TimeSpan ts = new TimeSpan(0, 0, 0, 0);
            //TimeSpan ts2 = new TimeSpan(1, 2, 3, 4);
            //Console.WriteLine(ts);
            //Console.WriteLine(ts2);
            //Console.WriteLine(ts + ts2);
        }
    }
}
