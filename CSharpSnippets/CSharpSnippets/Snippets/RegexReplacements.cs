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
            TestMultilineDelimiterEnd();
            // SingleTestMultilineDelimiter();
            // RegexReplacement6();
            // RegexReplacement5();
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
                string testdata = ReadDataFromFile.ReadLineAsString("roslyn-testcases.txt", ind, removeTrailingComment: true);
                Debug.WriteLine($"{testdata,-100} {MultilineDelimiterStart(testdata)}");
            }
        }

        public static void TestMultilineDelimiterEnd()
        {
            // true, false, true, true, true, true
            int[] lineNumbers = { 7, 8, 9, 22, 23, 24 };
            foreach (int ln in lineNumbers)
            {
                int ind = ln - 1;
                string testdata = ReadDataFromFile.ReadLineAsString("roslyn-testcases.txt", ind, removeTrailingComment: true);
                Debug.WriteLine($"line#={ln,3} ###{testdata,-100}### {MultilineDelimiterEnd(testdata)}");
            }
        }

        // [Fact(Skip = "https://github.com/dotnet/roslyn/issues/46414")] CommentLineTrailing

        public static void SingleTestMultilineDelimiter()
        {
            string testdata = ReadDataFromFile.ReadLineAsString("roslyn-testcases.txt", 23, removeTrailingComment: true);
            // Debug.WriteLine($"{testdata,-100} {MultilineDelimiterStart(testdata)}");
            Debug.WriteLine($"{testdata,-100} {MultilineDelimiterEnd(testdata)}");
            // Debug.WriteLine($"{testdata,-100} {MultilineDelimiterEndOldOld(testdata)}");
        }

        // using regex \"(?:\\.|[^\\"])*\"
        // -- there are failing cases
        // "\"\"\""
        private static Regex matchStringLiterals = new Regex("\\\"(?:\\\\.|[^\\\\\"])*\\\"");
        // this was derived as an alternative \"(\\.|[^\"])*\"
        // private static Regex matchStringLiterals = new Regex("\\\"(\\\\.|[^\\\"])*\\\"");

        public static void RegexReplacement6()
        {
            string testdata = ReadDataFromFile.ReadLineAsString("roslyn-testcases.txt", 22, removeTrailingComment: true);
            Debug.WriteLine($"{testdata}");

            string mystr = "\"\\n";
        }

        public static void RegexReplacement5()
        {
            // string testdata = ReadDataFromFile.ReadLineAsString("roslyn-testcases.txt", 9);
            // string testdata = ReadDataFromFile.ReadLineAsString("roslyn-testcases.txt", 10);
            // string testdata = ReadDataFromFile.ReadLineAsString("roslyn-testcases.txt", 11);
            // string testdata = ReadDataFromFile.ReadLineAsString("roslyn-testcases.txt", 12);
            // string testdata = ReadDataFromFile.ReadLineAsString("roslyn-testcases.txt", 13);
            // string testdata = ReadDataFromFile.ReadLineAsString("roslyn-testcases.txt", 14);
            // string testdata = ReadDataFromFile.ReadLineAsString("roslyn-testcases.txt", 16);
            // string testdata = ReadDataFromFile.ReadLineAsString("roslyn-testcases.txt", 17);
            string testdata = ReadDataFromFile.ReadLineAsString("roslyn-testcases.txt", 18);
            // Debug.WriteLine($"{testdata}");

            Debug.WriteLine($"{testdata}");

            string reduce = Regex.Replace(testdata, "{.*}", "").Replace("\"\"", "");
            string result1 = matchStringLiterals.Replace(reduce, "", 1);
            string result2 = matchStringLiterals.Replace(result1, "", 1);
        }

        public static void RegexReplacement3()
        {
            string testdata = ReadDataFromFile.ReadLineAsString("roslyn-testcases.txt", 9);

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

        public static bool MultilineDelimiterEnd(string line)
        {
            string reduced = Regex.Replace(line, "{.*}", "").Replace("\"\"", "");
            return reduced.IndexOf("\"") != -1 && (reduced.IndexOf("\"") == reduced.Length - 1 || reduced[reduced.IndexOf("\"") + 1] != '"');
        }
    }
}

