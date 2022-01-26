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
        public static void PrintNodes(Node node)
        {
            Console.Write(node.type + " \tchildren: " + node.children.Count);
            if (node.children != null)
                Console.WriteLine();
            foreach (Node child in node.children)
                if(child != null)
                    PrintNodes(child);
        }
        public static void Main()
        {
            string source = Path.GetFullPath("..\\..\\..\\..\\Programs\\program.txt");
            IReader reader;
            using (StreamReader sr = new StreamReader(source))
            {
                reader = new FileReader(sr);
                Scanner scanner = new Scanner(reader);
                IErrorHandler errorHandler = new ErrorHandler();
                Parser parser = new Parser(scanner, errorHandler);
                ProgramRoot pr = null;
                try
                {
                    pr = parser.Parse();
                    IVisitor visitor = new Visitor();
                    pr.Accept(visitor);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.InnerException != null ? e.InnerException.Message : e.Message);
                }
            }
        }
    }
}
