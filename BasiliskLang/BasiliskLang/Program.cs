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
        public static void Main()
        {
            string source = Path.GetFullPath("..\\..\\..\\..\\Programs\\test.txt");
            IReader reader;
            using (StreamReader sr = new StreamReader(source))
            {
                reader = new FileReader(sr);
                Scanner scanner = new Scanner(reader);
                while(scanner.currentToken?.type != Tokens.TokenType.EOF)
                {
                    scanner.NextToken();
                    scanner.currentToken.PrintTokenInfo();
                }
            }
        }
    }
}
