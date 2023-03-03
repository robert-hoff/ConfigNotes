using System.Diagnostics;
using System.Text.RegularExpressions;
using CSharpSnippets.FileIO;

namespace CSharpSnippets.Snippets
{
    public class RegexReplacements
    {
        public static void Run()
        {
            TestMultilineDelimiterStart();
            // TestMultilineDelimiterEnd();
            // MultilineTestcases();
            // SingleTestMultilineDelimiter();
            // RegexExample4();
            // RegexExample3();
            // RegexExample2();
            // RegexExample1();
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

        public static void MultilineTestcases()
        {
            int[] lineNumbers = { 10, 11, 12, 13, 14, 15, 16, 17, 18, 19 };
            foreach (int ln in lineNumbers)
            {
                int ind = ln - 1;
                string testdata = ReadDataFromFile.ReadLineAsString("roslyn-testcases.txt", ind);
                string reduce1 = Regex.Replace(testdata, "{.*}", "").Replace("\"\"", "");
                string reduce2 = matchStringLiterals.Replace(reduce1, "", 1);
                string reduce3 = matchStringLiterals.Replace(reduce2, "", 1);
                Debug.WriteLine($"Testing on: {testdata,-100} Reduced to: {reduce3}");
            }
        }

        public static void SingleTestMultilineDelimiter()
        {
            string testdata = ReadDataFromFile.ReadLineAsString("roslyn-testcases.txt", 23, removeTrailingComment: true);
            Debug.WriteLine($"{testdata,-100} {MultilineDelimiterEnd(testdata)}");
        }

        public static void RegexExample4()
        {
            string testdata = ReadDataFromFile.ReadLineAsString("roslyn-testcases.txt", 22, removeTrailingComment: true);
            Debug.WriteLine($"{testdata}");
            string mystr = "\"\\n";
        }

        public static void RegexExample3()
        {
            string testdata = ReadDataFromFile.ReadLineAsString("roslyn-testcases.txt", 9);
            testdata = Regex.Replace(testdata, "{.*}", "");
            Debug.WriteLine($"{testdata}");
        }

        /*
         * replace only first occurrence
         */
        public static void RegexExample2()
        {
            string testdata = "abc\"def\"ghijklmnopq\"rst\"uvwxyz";
            Regex stringReplace = new Regex("\".*?\"");
            testdata = stringReplace.Replace(testdata, "", 1);
            Debug.WriteLine($"{testdata}");
        }

        public static void RegexExample1()
        {
            string testdata = "ab{cd}efg";
            testdata = Regex.Replace(testdata, "{.*}", "");
            Debug.WriteLine($"{testdata}");
        }

        /*
         * regex for matching string literals
         * using regex
         *
         *      \"(?:\\.|[^\\"])*\"
         *
         * this seems to be an alternative
         *
         *      \"(\\.|[^\"])*\"
         *
         * possibly this is a failing testcase for the regular expressions
         *
         *      "\"\"\""
         *
         */
        public static readonly Regex matchStringLiterals = new Regex("\\\"(?:\\\\.|[^\\\\\"])*\\\"");

        // C# code is a multi-line string delimeter (start) if it has a single " after removing double ""
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

