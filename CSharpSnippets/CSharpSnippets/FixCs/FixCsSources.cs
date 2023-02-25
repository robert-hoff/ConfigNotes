using System.Diagnostics;
using CSharpSnippets.FileIO;

namespace CSharpSnippets.FixCs
{
    internal class FixCsSources
    {
        // private const string SOURCE_DIR = "../../../";
        private const string SOURCE_DIR = @"X:\checkouts\VRF-Main\TestVRFDev";

        private const string TEST_FILE = @"../../../Program.cs";
        private const int DESIRED_BLANK_LINES_AT_EOF = 2;
        private const int EOL_PREFERENCE = FileWriter.LINUX_ENDINGS;
        private const int BOM_PREFERENCE = FileWriter.SAVE_UTF_FILE_WITHOUT_BOM;

        // Roslyn files and preferences
        // private const string SOURCE_DIR = @"Z:/github/roslyn/src/Analyzers";
        // private const string TEST_FILE = @"Z:/github/roslyn/src/Analyzers/Core/Analyzers/AbstractBuiltInCodeStyleDiagnosticAnalyzer.cs";
        // private const int DESIRED_BLANK_LINES_AT_EOF = 1;
        // private const int EOL_PREFERENCE = FileWriter.WINDOWS_ENDINGS;
        // private const int BOM_PREFERENCE = FileWriter.SAVE_UTF_FILE_WITH_BOM;

        public static void Run()
        {
            // FixAllFiles();
            // FixSingleFile();
            ShowFixesForFile(TEST_FILE);
            // ShowCsFilesWalkDirectory();
        }

        public static void FixAllFiles()
        {
            foreach (string filenamepath in GetCsFilesWalkDirectory(SOURCE_DIR))
            {
                FixStylesForFile(filenamepath);
            }
        }

        public static void FixSingleFile()
        {
            FixStylesForFile(TEST_FILE);
        }

        public static void FixSelf()
        {
            string sourceDir = "../../../";
            foreach (string filenamepath in GetCsFilesWalkDirectory(sourceDir))
            {
                FixStylesForFile(filenamepath);
            }
        }

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
            sourceLines = RemoveDoubleBlankLines(sourceLines);
            sourceLines = RemoveBlankLinesFollowingBracket(sourceLines);
            sourceLines = RemoveBlankLinesLeadingBracket(sourceLines);
            sourceLines = SetDesiredEndOfFileBlankLines(sourceLines, DESIRED_BLANK_LINES_AT_EOF);
            sourceLines = RemoveTrailingSpacesAndUnifyEndings(sourceLines);
            return sourceLines;
        }

        /*
         * Add or remove blank lines at end of file to match preference
         *
         */
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

        /*
         * Remove any blank lines leading open braces '}' but not applied to '{'
         *
         */
        public static List<string> RemoveBlankLinesLeadingBracket(List<string> lines)
        {
            bool[] removeLines = new bool[lines.Count];
            int prevSingleBracket = -1;
            for (int i = lines.Count - 1; i >= 0; i--)
            {
                if (lines[i].Length > 0 && lines[i].Trim().Equals("}"))
                {
                    prevSingleBracket = i;
                }
                if (prevSingleBracket == i + 1 && string.IsNullOrWhiteSpace(lines[i]))
                {
                    removeLines[i] = true;
                    prevSingleBracket = i;
                }
            }
            return ApplyRemoveListedIndexes(lines, removeLines);
        }

        /*
         * Remove any blank lines following open braces '{' but not applied to '}'
         *
         */
        public static List<string> RemoveBlankLinesFollowingBracket(List<string> lines)
        {
            bool[] removeLines = new bool[lines.Count];
            int prevSingleBracket = -1;
            for (int i = 0; i < lines.Count; i++)
            {
                if (lines[i].Length > 0 && lines[i].Trim().Equals("{"))
                {
                    prevSingleBracket = i;
                }
                if (prevSingleBracket == i - 1 && string.IsNullOrWhiteSpace(lines[i]))
                {
                    removeLines[i] = true;
                    prevSingleBracket = i;
                }
            }
            return ApplyRemoveListedIndexes(lines, removeLines);
        }

        /*
         * removes double blanks (or multiple blank lines) and any leading
         * blank lines at the beginning of the file.
         *
         */
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
    }
}

