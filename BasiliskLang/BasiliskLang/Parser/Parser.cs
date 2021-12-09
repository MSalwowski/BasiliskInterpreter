using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasiliskLang
{
    public class Parser
    {
        Scanner scanner;
        public Parser(Scanner _scanner)
        {
            scanner = _scanner;
        }

        public void Parse()
        {
            scanner.Scan();
        }
    }
}
