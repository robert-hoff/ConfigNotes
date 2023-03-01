using System.Diagnostics;
using System.Text.RegularExpressions;
using CSharpSnippets.FileIO;

namespace CSharpSnippets.Snippets
{
    public static class CheckMultilineDelimiter
    {
        public static void Run()
        {
            string myStr3 = ReadDataFromFile.ReadLineAsString("stringdatatest.txt", lineNumber: 0, removeTrailingComment: true);
            Debug.WriteLine($"{myStr3}");
            Debug.WriteLine($"{MultilineDelimiterStart(myStr3)}");
        }

        /*
         * C# code is a multi-line delimeter (start) if it has @" without " but not ""
         *
         */
        public static bool MultilineDelimiterStart(string line)
        {
            string lineWoComment = RemoveTrailingComment(line);
            return lineWoComment.IndexOf("@\"") > -1 &&
                !Regex.IsMatch(lineWoComment[(lineWoComment.IndexOf("@\"") + 2)..], "^\\\"[^\\\"]|[^\\\"]\\\"[^\\\"]");
        }

        /*
         * C# code is a multi-line delimeter (end) if it has " but not ""
         *
         */
        public static bool MultilineDelimiterEnd(string line)
        {
            return Regex.IsMatch(RemoveTrailingComment(line), "^\\\"[^\\\"]|[^\\\"]\\\"[^\\\"]");
        }

        public static string RemoveTrailingComment(string line)
        {
            return line.IndexOf("//") > -1 ? line[..line.IndexOf("//")] : line;
        }


        public static void MultiLineStringExample()
        {
            string multi = @" @ "" "";
                     ""
                     ";
            Debug.WriteLine($"{multi}");
        }
    }
}

