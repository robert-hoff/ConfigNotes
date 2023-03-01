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

            // TestMultilineDelimiterStart();
            // TestMultilineDelimiterEnd();
            // SingleTestMultilineDelimiter();
            RegexReplacement5();
            // RegexReplacement4();
            // RegexReplacement3();
            // RegexReplacement2();
            // RegexReplacement1();
        }


        /*
         * C# code is a multi-line string delimeter (start) if it has @" without " but not ""
         *
         */
        public static void TestMultilineDelimiterStart()
        {
            int[] testIndexes = { 0, 1, 2, 3, 4, 5, 14 };
            foreach (int ind in testIndexes)
            {
                string testdata = ReadDataFromFile.ReadLineAsString("stringdatatest.txt", ind, removeTrailingComment: true);
                Debug.WriteLine($"{testdata,-100} {MultilineDelimiterStart(testdata)}");
            }
        }

        public static void TestMultilineDelimiterEnd()
        {
            int[] testIndexes = { 6, 7, 8 };
            foreach (int ind in testIndexes)
            {
                string testdata = ReadDataFromFile.ReadLineAsString("stringdatatest.txt", ind, removeTrailingComment: true);
                Debug.WriteLine($"{testdata,-100} {MultilineDelimiterEnd(testdata)}");
            }
        }


        // [Fact(Skip = "https://github.com/dotnet/roslyn/issues/46414")] CommentLineTrailing



        public static void SingleTestMultilineDelimiter()
        {
            string testdata = ReadDataFromFile.ReadLineAsString("stringdatatest.txt", 4, removeTrailingComment: true);
            Debug.WriteLine($"{testdata,-100} {MultilineDelimiterStart(testdata)}");
        }


        // private static Regex regex = new Regex("\"(?:\\.|[^\\"])*\"");
        // private static Regex regex = new Regex("\"(?:\\.|[^\\"])*\"");


        private static Regex matchStringLiterals = new Regex("\\\"(?:\\\\.|[^\\\\\"])*\\\"");


        public static void RegexReplacement5()
        {
            // string testdata = ReadDataFromFile.ReadLineAsString("stringdatatest.txt", 9);
            // string testdata = ReadDataFromFile.ReadLineAsString("stringdatatest.txt", 10);
            // string testdata = ReadDataFromFile.ReadLineAsString("stringdatatest.txt", 11);
            // string testdata = ReadDataFromFile.ReadLineAsString("stringdatatest.txt", 12);
            // string testdata = ReadDataFromFile.ReadLineAsString("stringdatatest.txt", 13);
            // string testdata = ReadDataFromFile.ReadLineAsString("stringdatatest.txt", 14);
            // string testdata = ReadDataFromFile.ReadLineAsString("stringdatatest.txt", 16);
            string testdata = ReadDataFromFile.ReadLineAsString("stringdatatest.txt", 17);
            // Debug.WriteLine($"{testdata}");


            Debug.WriteLine($"{testdata}");

            string reduce = Regex.Replace(testdata, "{.*}", "").Replace("\"\"", "");
            string result1 = matchStringLiterals.Replace(reduce, "", 1);
            string result2 = matchStringLiterals.Replace(result1, "", 1);
            Debug.WriteLine($"{reduce}");
            Debug.WriteLine($"{result1}");
            Debug.WriteLine($"{result2}");
        }


        /*
         * C# code is a multi-line string delimeter (start) if it has a single " after removing double ""
         *
         */
        public static bool MultilineDelimiterStart(string line)
        {
            if (line.IndexOf("@\"") > -1)
            {
                string reduced = Regex.Replace(line, "{.*}", "").Replace("\"\"", "");
                return reduced.IndexOf("\"") != -1 && reduced.LastIndexOf("\"") == reduced.IndexOf("\"");
            }
            else
            {
                return false;
            }
        }

        /*
         * C# code is a multi-line string delimeter (end)if it has a single " after removing double ""
         *
         */
        public static bool MultilineDelimiterEnd(string line)
        {
            string reduced = Regex.Replace(line, "{.*}", "").Replace("\"\"", "");
            return reduced.IndexOf("\"") != -1 && reduced.LastIndexOf("\"") == reduced.IndexOf("\"");
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

