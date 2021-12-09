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
            // WERSJA DEMONSTRACYJNA Z DWOMA PRZYKŁADAMI Z DOKUMENTACJI
            //string source = Path.GetFullPath("..\\..\\..\\..\\Programs\\datetime_i_period.txt.txt");
            //string source = Path.GetFullPath("..\\..\\..\\..\\Programs\\int.txt");
            //string source = Path.GetFullPath("..\\..\\..\\..\\Programs\\blad.txt");
            //string source = "\"";
            Scanner scanner = new Scanner(source, InputType.String);
            scanner.Scan();
            // ========================================================

            // przeniesienie otwierania pliku do usinga na poziomie maina
        }
    }
}
