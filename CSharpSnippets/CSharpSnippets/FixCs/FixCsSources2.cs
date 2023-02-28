using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using CSharpSnippets.FileIO;

namespace CSharpSnippets.FixCs
{
    internal class FixCsSources2
    {
        private const string SOURCE_DIR = @"Z:/github/roslyn/src/Analyzers";
        // private const string TEST_FILE = @"Z:/github/roslyn/src/Analyzers/Core/Analyzers/AbstractBuiltInCodeStyleDiagnosticAnalyzer.cs";
        // private const string TEST_FILE = @"Z:\github\roslyn\src\Analyzers\CSharp\Tests\MakeMethodAsynchronous\MakeMethodAsynchronousTests.cs";
        private const string TEST_FILE = @"Z:\github\ConfigNotes\CSharpSnippets\CSharpSnippets\Snippets\NullableTypes.cs";
        // private const string TEST_FILE = @"Z:\github\ConfigNotes\CSharpSnippets\CSharpSnippets\Snippets\NullableTypes2.cs";

        public static void CleanupRoslynSources()
        {
            // FixAllFiles();
            // FixSingleFile();
            // ShowFixesForFile(TEST_FILE);
            // ShowCsFilesWalkDirectory();
            new FixCsSources2(TEST_FILE);
        }

        // Roslyn preferences
        // private const int DEFAULT_DESIRED_BLANK_LINES_AT_EOF = 1;
        // private const int DEFAULT_EOL_PREFERENCE = FileWriter.WINDOWS_ENDINGS;
        // private const int DEFAULT_BOM_PREFERENCE = FileWriter.SAVE_UTF_FILE_WITH_BOM;

        // My preferences
        private const int DEFAULT_DESIRED_BLANK_LINES_AT_EOF = 2;
        private const int DEFAULT_EOL_PREFERENCE = FileWriter.LINUX_ENDINGS;
        private const int DEFAULT_BOM_PREFERENCE = FileWriter.SAVE_UTF_FILE_WITHOUT_BOM;


        private string[] sourceLines;
        private bool[] extraneousBlankLines;
        private bool[] multistringLines;
        private bool[] commentedLines;

        public FixCsSources2(
            string fileNamePath,
            int blankLinesAtEof = DEFAULT_DESIRED_BLANK_LINES_AT_EOF,
            int eolPreference = DEFAULT_EOL_PREFERENCE,
            int bomPreference = DEFAULT_BOM_PREFERENCE
            )
        {
            List<string> lines = File.ReadAllLines($"{fileNamePath}").ToList();
            for (int i = 0; i < blankLinesAtEof; i++)
            {
                lines.Add(string.Empty);
            }
            sourceLines = lines.ToArray();
            extraneousBlankLines = new bool[sourceLines.Length];
            // multistringLines = new bool[sourceLines.Length];
            multistringLines = IdentifyMultistringLines();

            // commentedLines = new bool[sourceLines.Length];
            commentedLines = IdentifyCommentedLines();

            // RemoveExtraneousLinesAtEof(sourceLines, extraneousBlankLines, blankLinesAtEof);
            // IdentifyBlankLinesFollowingBracket(sourceLines, extraneousBlankLines);
            // IdentifyBlankLinesLeadingBracket(sourceLines, extraneousBlankLines);
            // ApplyCleanup(fileNamePath, eolPreference, bomPreference);



            for (int i = 0; i < multistringLines.Length; i++)
            {
                Debug.WriteLine($"{i,2:#0}   multistring = {(multistringLines[i] ? "True" : "F   ")} " +
                    $"commented = {(commentedLines[i] ? "True" : "F   ")}");
            }
        }


        private bool[] IdentifyCommentedLines()
        {
            bool[] commentedLines = new bool[sourceLines.Length];
            bool insideMultilineComment = false;
            for (int i = 0; i < sourceLines.Length; i++)
            {
                if (multistringLines[i]) { continue; }
                bool singleLineComment = !insideMultilineComment && SingleLineOrInlineComment(sourceLines[i]);
                bool multiLineCommentStart = !singleLineComment && !insideMultilineComment ?
                        MultilineCommentDelimiterStart(sourceLines[i]) :
                        false;
                insideMultilineComment |= multiLineCommentStart;

                commentedLines[i] = singleLineComment || insideMultilineComment;

                insideMultilineComment = !singleLineComment && !multiLineCommentStart && insideMultilineComment ?
                        !MultilineCommentDelimiterEnd(sourceLines[i]) :
                        insideMultilineComment;

                // correct version
                /*
                bool singleLineComment = !insideMultilineComment && SingleLineOrInlineComment(sourceLines[i]);
                insideMultilineComment = singleLineComment ?
                    false :
                    insideMultilineComment ?
                        !MultilineCommentDelimiterEnd(sourceLines[i]) :
                        MultilineCommentDelimiterStart(sourceLines[i]);

                commentedLines[i] = singleLineComment || insideMultilineComment;
                */
            }
            return commentedLines;
        }

        /*
         * C# code is a single-line comment, or inline comment, if it has any instance of //
         * (and in unusual cases also preceds any instances of /*)
         *
         * C# code is also a single-line comment, or inline comment,
         * if the last instance /* has a closing */
        public static bool SingleLineOrInlineComment(string line)
        {
            return line.IndexOf("/*") == -1 ?
                line.IndexOf("//") > -1 : // no instance of '/*'
                line.IndexOf("//") > -1 ?
                    line.IndexOf("//") < line.IndexOf("/*") : // instance of '/*' preceeded by '//'
                    line.LastIndexOf("/*") < line.LastIndexOf("*/"); // instance of '/*' but followed by '*/'
        }

        /*
         * C# code is a multi-line comment delimeter (start) if it has any instance of /*
         * (and is not recognised as a single-line comment, and not already part of a multi-line comment)
         */
        public static bool MultilineCommentDelimiterStart(string line)
        {
            return line.IndexOf("/*") > -1;
        }

        /*
         * C# code is a multi-line comment delimeter (end) if it has any instance of */
        public static bool MultilineCommentDelimiterEnd(string line)
        {
            return line.IndexOf("*/") > -1;
        }

        private bool[] IdentifyMultistringLines()
        {
            bool[] multistringLines = new bool[sourceLines.Length];
            bool insideMultilineString = false;
            for (int i = 0; i < sourceLines.Length; i++)
            {
                insideMultilineString = insideMultilineString ?
                    !MultilineDelimiterEnd(sourceLines[i]) :
                    MultilineDelimiterStart(sourceLines[i]);
                multistringLines[i] = insideMultilineString;
            }
            return multistringLines;
        }

        /*
         * C# code is a multi-line string delimeter (start) if it has @" without " but not ""
         *
         */
        public static bool MultilineDelimiterStart(string line)
        {
            return line.IndexOf("@\"") > -1 &&
                !Regex.IsMatch(line[(line.IndexOf("@\"") + 2)..], "^\\\"[^\\\"]|[^\\\"]\\\"[^\\\"]");
        }

        /*
         * C# code is a multi-line string delimeter (end) if it has " but not ""
         *
         */
        public static bool MultilineDelimiterEnd(string line)
        {
            return Regex.IsMatch(line, "^\\\"[^\\\"]|[^\\\"]\\\"[^\\\"]");
        }





        private void ApplyCleanup(string fileNamePath, int eolPreference, int bomPreference)
        {
            FileWriter fw = new FileWriter(fileNamePath, EOL: eolPreference, useBom: bomPreference);
            for (int i = 0; i < sourceLines.Length; i++)
            {
                if (!extraneousBlankLines[i])
                {
                    fw.WriteLine(sourceLines[i]);
                }
            }
            FileWriter.CloseFileWriter(fw);
        }

        /*
         * Mark any blank lines following an opening bracket '{'
         */
        private static void IdentifyBlankLinesFollowingBracket(string[] lines, bool[] extraneousBlankLines)
        {
            bool bracketEncountered = false;
            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].Length > 0 && lines[i].Trim().Equals("{"))
                {
                    bracketEncountered = true;
                }
                if (!string.IsNullOrWhiteSpace(lines[i]) && !lines[i].Trim().Equals("{"))
                {
                    bracketEncountered = false;
                }
                if (bracketEncountered && string.IsNullOrWhiteSpace(lines[i]))
                {
                    extraneousBlankLines[i] = true;
                }
            }
        }

        /*
         * Mark any blank lines leading a closing bracket '}'
         */
        private static void IdentifyBlankLinesLeadingBracket(string[] lines, bool[] extraneousBlankLines)
        {
            bool bracketEncountered = false;
            for (int i = lines.Length - 1; i >= 0; i--)
            {
                if (lines[i].Length > 0 && lines[i].Trim().Equals("}"))
                {
                    bracketEncountered = true;
                }
                if (!string.IsNullOrWhiteSpace(lines[i]) && !lines[i].Trim().Equals("}"))
                {
                    bracketEncountered = false;
                }
                if (bracketEncountered && string.IsNullOrWhiteSpace(lines[i]))
                {
                    extraneousBlankLines[i] = true;
                }
            }
        }

        /*
         * Mark any blank lines beyond desiredBlankLines (must be padded with at least desiredBlankLines)
         */
        private static void RemoveExtraneousLinesAtEof(string[] lines, bool[] extraneousBlankLines, int desiredBlankLines)
        {
            for (int i = lines.Length - desiredBlankLines;
                i >= 0 && string.IsNullOrWhiteSpace(lines[i]);
                i--)
            {
                extraneousBlankLines[i] = true;
            }
        }







        /*
        // replace line endings and trim trailing spaces
        public static void FixStylesForFile(string filenamepath)
        {
            List<string> sourceLines = GetModifiedSourceLines(filenamepath);
            FileWriter fw = new FileWriter(filenamepath, EOL: EOL_PREFERENCE, useBom: BOM_PREFERENCE);
            foreach (string line in sourceLines)
            {
                fw.WriteLine(line);
            }
            FileWriter.CloseFileWriter(fw);
        }

        public static void ShowFixesForFile(string filenamepath)
        {
            List<string> sourceLines = GetModifiedSourceLines(filenamepath);
            foreach (string item in sourceLines)
            {
                Debug.WriteLine($"{item}");
            }
            Debug.WriteLine($"END");
        }

        public static List<string> GetModifiedSourceLines(string filenamepath)
        {
            List<string> sourceLines = ReadFileAsStringList(filenamepath);
            // sourceLines = RemoveDoubleBlankLines(sourceLines);
            sourceLines = RemoveBlankLinesFollowingBracket(sourceLines);
            sourceLines = RemoveBlankLinesLeadingBracket(sourceLines);
            sourceLines = SetDesiredEndOfFileBlankLines(sourceLines, DESIRED_BLANK_LINES_AT_EOF);
            sourceLines = RemoveTrailingSpacesAndUnifyEndings(sourceLines);
            return sourceLines;
        }

        // Add or remove blank lines at end of file to match preference
        public static List<string> SetDesiredEndOfFileBlankLines(List<string> lines, int desiredBlankLines = 1)
        {
            if (desiredBlankLines <= 0)
            {
                throw new Exception("this method only makes sense for desiredBlankLines >= 1");
            }

            List<string> sourceLines = lines;
            for (int i = 0; i < desiredBlankLines; i++)
            {
                sourceLines.Add("\n");
            }
            bool[] removeLines = new bool[sourceLines.Count];
            for (int i = sourceLines.Count - desiredBlankLines;
                i >= 0 && string.IsNullOrWhiteSpace(sourceLines[i]);
                i--)
            {
                removeLines[i] = true;
            }
            return ApplyRemoveListedIndexes(sourceLines, removeLines);
        }

        // Remove any blank lines leading open bracket '}' but not applied to '{'
        public static List<string> RemoveBlankLinesLeadingBracket(List<string> lines)
        {
            bool[] removeLines = new bool[lines.Count];
            bool bracketEncountered = false;
            for (int i = lines.Count - 1; i >= 0; i--)
            {
                if (lines[i].Length > 0 && lines[i].Trim().Equals("}"))
                {
                    bracketEncountered = true;
                }
                if (!string.IsNullOrWhiteSpace(lines[i]) && !lines[i].Trim().Equals("}"))
                {
                    bracketEncountered = false;
                }
                if (bracketEncountered && string.IsNullOrWhiteSpace(lines[i]))
                {
                    removeLines[i] = true;
                }
            }
            return ApplyRemoveListedIndexes(lines, removeLines);
        }

        // Remove any blank lines following open bracket '{' but not applied to '}'
        public static List<string> RemoveBlankLinesFollowingBracket(List<string> lines)
        {
            bool[] removeLines = new bool[lines.Count];
            bool bracketEncountered = false;
            for (int i = 0; i < lines.Count; i++)
            {
                if (lines[i].Length > 0 && lines[i].Trim().Equals("{"))
                {
                    bracketEncountered = true;
                }
                if (!string.IsNullOrWhiteSpace(lines[i]) && !lines[i].Trim().Equals("{"))
                {
                    bracketEncountered = false;
                }
                if (bracketEncountered && string.IsNullOrWhiteSpace(lines[i]))
                {
                    removeLines[i] = true;
                }
            }
            return ApplyRemoveListedIndexes(lines, removeLines);
        }


        // removes double blanks (or multiple blank lines) and any leading
        // blank lines at the beginning of the file.
        public static List<string> RemoveDoubleBlankLines(List<string> lines)
        {
            bool[] removeLines = new bool[lines.Count];
            int prevBlank = -1;
            for (int i = 0; i < lines.Count; i++)
            {
                if (string.IsNullOrEmpty(lines[i].Trim()))
                {
                    if (i - 1 == prevBlank)
                    {
                        removeLines[i] = true;
                    }
                    prevBlank = i;
                }
                // stop at the last bracket
                if (lines[i].Length > 0 && lines[i][0].Equals('}'))
                {
                    break;
                }
            }
            return ApplyRemoveListedIndexes(lines, removeLines);
        }

        public static List<string> ApplyRemoveListedIndexes(List<string> lines, bool[] removeLines)
        {
            List<string> modifiedLines = new();
            for (int i = 0; i < removeLines.Length; i++)
            {
                if (!removeLines[i])
                {
                    modifiedLines.Add(lines[i]);
                }
            }
            return modifiedLines;
        }

        public static List<string> RemoveTrailingSpacesAndUnifyEndings(List<string> lines)
        {
            List<string> modifiedLines = new();
            foreach (string line in lines)
            {
                string modifiedLine = line.TrimEnd();
                modifiedLines.Add(modifiedLine);
            }
            return modifiedLines;
        }

        public static List<string> ReadFileAsStringList(string filenamepath, bool omitEmptyLines = false)
        {
            List<string> data = new();
            string[] lines = File.ReadAllLines($"{filenamepath}");
            foreach (string line in lines)
            {
                if (!omitEmptyLines || !string.IsNullOrEmpty(line.Trim()))
                {
                    data.Add(line);
                }
            }
            return data;
        }

        public static void ShowCsFilesWalkDirectory()
        {
            foreach (string file in GetCsFilesWalkDirectory(SOURCE_DIR))
            {
                Debug.WriteLine($"{file}");
            }
        }

        public static string[] GetCsFilesWalkDirectory(string path)
        {
            return Directory.GetFileSystemEntries(path, "*.cs", SearchOption.AllDirectories);
        }
        */
    }
}

