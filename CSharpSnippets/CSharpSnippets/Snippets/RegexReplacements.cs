using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CSharpSnippets.FileIO;

namespace CSharpSnippets.Snippets
{
    public class RegexReplacements
    {
        public static void Run()
        {
            // RegexReplacement3();
            // RegexReplacement2();
            // RegexReplacement1();
        }


        public static void RegexReplacement4()
        {
            string multi = @"
                {  }  // dgdfg
/* */
             "" ";




        }


        public static void RegexReplacement3()
        {
            string testdata = ReadDataFromFile.ReadLineAsString("stringdatatest.txt", 9);

            // Console.WriteLine($"Length {($"h\"ello".PadRight(20)) /* " " " " " */}"); /// comment with " "" " and other crap in it
            // Debug.WriteLine($"{testdata}");
            testdata = Regex.Replace(testdata, "{.*}", "");
            Debug.WriteLine($"{testdata}");

        }

        /*
         * replace only first occurance
         */
        public static void RegexReplacement2()
        {
            string testdata = "abc\"def\"ghijklmnopq\"rst\"uvwxyz";
            Regex stringReplace = new Regex("\".*?\"");
            testdata = stringReplace.Replace(testdata, "", 1);
            Debug.WriteLine($"{testdata}");
        }



        public static void RegexReplacement1()
        {
            string testdata = "ab{cd}efg";
            testdata = Regex.Replace(testdata, "{.*}", "");
            Debug.WriteLine($"{testdata}");
        }
    }
}

