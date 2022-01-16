using BasiliskLang.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasiliskLang
{
    public interface IScanner
    {
        public Token currentToken { get; set; }
        public void NextToken();
    }
}
