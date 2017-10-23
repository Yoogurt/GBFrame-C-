using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GBFrame.Test
{
    class ConfigFileTest
    {
        static void Main(string[] arg) {

            ConfigFile file = ConfigFile.Open("C:\\Users\\monitor\\Desktop\\config.ini");
            // file.Add("test" , "gb");
            // file.Add("look" , "me");
            // file.Commit();

            Console.WriteLine(file.Value(""));
            Console.WriteLine(file.Value("test"));
            Console.WriteLine(file.Value("look"));

            Console.ReadKey();
        }
    }
}
