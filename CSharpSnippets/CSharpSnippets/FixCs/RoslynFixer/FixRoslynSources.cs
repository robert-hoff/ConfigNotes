using System.Diagnostics;
using System.Numerics;
using System.Text.RegularExpressions;
using CSharpSnippets.FileIO;

#pragma warning disable IDE0051 // Remove unused private members
namespace CSharpSnippets.FixCs.RoslynFixer
{
    public class FixRoslynSources
    {
        // Roslyn preferences
        private const int DEFAULT_DESIRED_BLANK_LINES_AT_EOF = 0;
        private const int DEFAULT_EOL_PREFERENCE = FileWriter.WINDOWS_ENDINGS;
        private const int DEFAULT_BOM_PREFERENCE = FileWriter.SAVE_UTF_FILE_WITH_BOM;

        // private const int GENERIC_UNASSIGNED = 0;
        private const int GEN_LINE = 1;
        private const int BLANK_LINE = 2;
        private const int COMMENT_LINE = 4;
        private const int COMMENT_TRAILING = 8;
        private const int COMMENT_BLOCK = 16;
        // private const int COMMENT_BLOCK_INLINED = 32;
        private const int DOC_COMMENT = 64;
        private const int QUAD_COMMENT = 128;
        private const int MULTISTRING = 256;
        private const int OPENING_BRACKET = 512;
        private const int CLOSING_BRACKET = 1024;

        // trim lines for the following source types
        //private const bool TRIM_GEN_UNASSIGNED = true;
        //private const bool TRIM_GEN_LINE = true;
        //private const bool TRIM_BLANK_LINE = true;
        //private const bool TRIM_COMMENT_LINE = true;
        //private const bool TRIM_COMMENT_TRAILING = true;
        //private const bool TRIM_COMMENT_BLOCK = false;
        //private const bool TRIM_COMMENT_BLOCK_INLINED = false;
        //private const bool TRIM_DOC_COMMENT = false;
        //private const bool TRIM_QUAD_COMMENT = false;
        //private const bool TRIM_MULTISTRING = false;

        private const bool TRIM_GEN_UNASSIGNED = false;
        private const bool TRIM_GEN_LINE = false;
        private const bool TRIM_BLANK_LINE = false;
        private const bool TRIM_COMMENT_LINE = false;
        private const bool TRIM_COMMENT_TRAILING = false;
        private const bool TRIM_COMMENT_BLOCK = false;
        private const bool TRIM_COMMENT_BLOCK_INLINED = false;
        private const bool TRIM_DOC_COMMENT = false;
        private const bool TRIM_QUAD_COMMENT = true;
        private const bool TRIM_MULTISTRING = false;
        private const bool TRIM_GEN_LINE_BRACKETS = false;

        private string[] sourceTypeLabels = {
            "GENERIC_UNASSIGNED", "GEN_LINE", "BLANK_LINE", "COMMENT_LINE", "COMMENT_TRAILING", "COMMENT_BLOCK",
            "COMMENT_BLOCK_INLINED", "DOC_COMMENT", "QUAD_COMMENT", "MULTISTRING", "OPENING_BRACKET", "CLOSING_BRACKET"};

        private string[] reportLabels = {
            "line", "line", "blank line", "line comment", "trailing comment", "block comment",
            "block comment inlined", "doc comment", "doc comment", "multistring line", "line", "line"};

        private string GetSourceTypeDescription(int sourceType)
        {
            var bitPosition = BitOperations.Log2((uint) sourceType & ~(uint) (sourceType - 1)) + 1;
            return sourceTypeLabels[bitPosition];
        }

        private string GetReportLabel(int sourceType)
        {
            var bitPosition = BitOperations.Log2((uint) sourceType & ~(uint) (sourceType - 1)) + 1;
            return reportLabels[bitPosition];
        }

        private bool[] trimSourceType = {
            TRIM_GEN_UNASSIGNED, TRIM_GEN_LINE, TRIM_BLANK_LINE, TRIM_COMMENT_LINE, TRIM_COMMENT_TRAILING,
            TRIM_COMMENT_BLOCK, TRIM_COMMENT_BLOCK_INLINED, TRIM_DOC_COMMENT, TRIM_QUAD_COMMENT, TRIM_MULTISTRING,
            TRIM_GEN_LINE_BRACKETS, TRIM_GEN_LINE_BRACKETS
        };

        private bool TrimEndOfLineForSourceType(int sourceType)
        {
            var bitPosition = BitOperations.Log2((uint) sourceType & ~(uint) (sourceType - 1)) + 1;
            return trimSourceType[bitPosition];
        }

        private static bool CheckSourceTypeMatch(int sourceDescription, int sourceType)
        {
            return (sourceDescription & sourceType) > 0;
        }

        private int[] sourceLineDescription;
        private string fileNamePath;
        private int fileNr;
        private string[] sourceLines;
        private string[] trimmedSource;

        string[] blankLineLabel = { "", "part of 2 or more blank line", "following bracket", "leading bracket" };
        int DOUBLE_BLANK_LINE = 1;
        int BLANK_LINE_FOLLOWING_BRACKET = 2;
        int BLANK_LINE_LEADING_BRACKET = 3;
        int BLANK_LINE_SET_AT_EOF = 4;

        private int[] removeBlankLineOfType;
        // private bool[] blankLinesForRemoval;
        private int desiredBlankLinesAtEof;

        public FixRoslynSources(
            string fileNamePath,
            int fileNr = -1,
            bool showSourceAnalysis = false,
            bool writeFile = false,
            bool showFixedFile = false,
            bool showReport = false,
            int desiredBlankLinesAtEof = DEFAULT_DESIRED_BLANK_LINES_AT_EOF)
        {
            this.fileNamePath = fileNamePath;
            this.fileNr = fileNr;
            this.desiredBlankLinesAtEof = desiredBlankLinesAtEof;
            Debug.WriteLine($" {fileNr,9} Fixing C# source for {fileNamePath}");
            sourceLines = ReadFileAsStringArray(fileNamePath, desiredBlankLinesAtEof);
            sourceLineDescription = new int[sourceLines.Length];
            trimmedSource = GetTrimmedSource(sourceLines);
            AnalyseSourceFirstPass();
            if (showSourceAnalysis) { ShowSourceAnalysis(); }
            // blankLinesForRemoval = new bool[sourceLines.Length];
            removeBlankLineOfType = new int[sourceLines.Length];
            IdentifyBlankLines();

            if (writeFile)
            {
                WriteChangesToFile(fileNamePath, EOL: DEFAULT_EOL_PREFERENCE, useBom: DEFAULT_BOM_PREFERENCE);
            }
            if (showFixedFile)
            {
                ShowFixedFile();
            }

            if (showReport)
            {
                ShowReport();
            }

            // MarkLinesWithSourceAnalysis(fileNamePath, DEFAULT_EOL_PREFERENCE, DEFAULT_BOM_PREFERENCE);
        }

        public List<string> GetReport()
        {
            List<string> report = new();
            for (var i = 0; i < sourceLines.Length - desiredBlankLinesAtEof; i++)
            {
                var trimmedLength = sourceLines[i].TrimEnd().Length;
                var unalteredLength = sourceLines[i].Length;
                if (removeBlankLineOfType[i] > 0)
                {
                    report.Add($"{fileNamePath[20..],-80} line {i,-6} remove blank line {blankLineLabel[removeBlankLineOfType[i]]}");
                }
                else if (trimmedLength < unalteredLength)
                {
                    var spaceCount = unalteredLength - trimmedLength;
                    report.Add($"{fileNamePath[20..],-80} line {i,-6} trim {spaceCount} trailing spaces for {GetReportLabel(sourceLineDescription[i])}");
                }
            }
            return report;
        }

        private void ShowReport()
        {
            foreach (var line in GetReport())
            {
                Debug.WriteLine($"{line}");
            }
        }

        public enum SourceState
        {
            None, InsideBlockComment, InsideMultiString
        }

        private void IdentifyBlankLines()
        {
            IdentifyDoubleBlankLines();
            IdentifyBlankLinesFollowingBracket();
            IdentifyBlankLinesLeadingBracket();
            SetDesiredEndOfFileBlankLines();
        }

        private void WriteChangesToFile(string fileNamePath, int EOL, int useBom)
        {
            FileWriter fw = new(fileNamePath, EOL: EOL, useBom: useBom);
            for (var i = 0; i < sourceLines.Length; i++)
            {
                // if (!blankLinesForRemoval[i])
                if (removeBlankLineOfType[i] > 0)
                {
                    var trimEndOfLine = TrimEndOfLineForSourceType(sourceLineDescription[i]);
                    if (trimEndOfLine)
                    {
                        fw.WriteLine(sourceLines[i].TrimEnd());
                    }
                    else
                    {
                        fw.WriteLine(sourceLines[i]);
                    }
                }
            }
            FileWriter.CloseFileWriter(fw);
        }

        // removes double blanks (or multiple blank lines) and any leading
        // blank lines at the beginning of the file.
        public void IdentifyDoubleBlankLines()
        {
            for (var i = 1; i < sourceLines.Length - desiredBlankLinesAtEof; i++)
            {
                if (CheckSourceTypeMatch(sourceLineDescription[i], BLANK_LINE) &&
                    CheckSourceTypeMatch(sourceLineDescription[i - 1], BLANK_LINE))
                {
                    // blankLinesForRemoval[i] = true;
                    removeBlankLineOfType[i] = DOUBLE_BLANK_LINE;
                }
            }
        }

        /*
         * Mark any blank lines following an opening bracket '{'
         */
        private void IdentifyBlankLinesFollowingBracket()
        {
            var bracketEncountered = false;
            for (var i = 0; i < sourceLines.Length; i++)
            {
                if (CheckSourceTypeMatch(sourceLineDescription[i], OPENING_BRACKET))
                {
                    bracketEncountered = true;
                }
                if (!CheckSourceTypeMatch(sourceLineDescription[i], BLANK_LINE) &&
                    !CheckSourceTypeMatch(sourceLineDescription[i], OPENING_BRACKET))
                {
                    bracketEncountered = false;
                }
                if (bracketEncountered && CheckSourceTypeMatch(sourceLineDescription[i], BLANK_LINE))
                {
                    // blankLinesForRemoval[i] = true;
                    removeBlankLineOfType[i] = BLANK_LINE_FOLLOWING_BRACKET;
                }
            }
        }

        /*
         * Mark any blank lines leading a closing bracket '}'
         */
        private void IdentifyBlankLinesLeadingBracket()
        {
            var bracketEncountered = false;
            for (var i = sourceLines.Length - 1; i >= 0; i--)
            {
                if (CheckSourceTypeMatch(sourceLineDescription[i], CLOSING_BRACKET))
                {
                    bracketEncountered = true;
                }
                if (!CheckSourceTypeMatch(sourceLineDescription[i], BLANK_LINE) &&
                    !CheckSourceTypeMatch(sourceLineDescription[i], CLOSING_BRACKET))
                {
                    bracketEncountered = false;
                }
                if (bracketEncountered && CheckSourceTypeMatch(sourceLineDescription[i], BLANK_LINE))
                {
                    // blankLinesForRemoval[i] = true;
                    removeBlankLineOfType[i] = BLANK_LINE_LEADING_BRACKET;
                }
            }
        }

        public void SetDesiredEndOfFileBlankLines()
        {
            if (desiredBlankLinesAtEof <= 0) { return; }
            for (var i = sourceLines.Length - desiredBlankLinesAtEof;
                i >= 0 && CheckSourceTypeMatch(sourceLineDescription[i], BLANK_LINE);
                i--)
            {
                // blankLinesForRemoval[i] = true;
                removeBlankLineOfType[i] = BLANK_LINE_SET_AT_EOF;
            }
        }

        private void AnalyseSourceFirstPass()
        {
            SourceState sourceState = SourceState.None;
            for (var i = 0; i < trimmedSource.Length; i++)
            {
                switch (sourceState)
                {
                    case SourceState.None:

                        var linetype = GetLineType(trimmedSource, i);

                        if (linetype == BLANK_LINE_TYPE)
                        {
                            sourceLineDescription[i] = BLANK_LINE;
                        }
                        else if (linetype == GENERIC_LINE_TYPE)
                        {
                            sourceLineDescription[i] |= trimmedSource[i] switch
                            {
                                "{" => OPENING_BRACKET,
                                "}" => CLOSING_BRACKET,
                                _ => GEN_LINE
                            };
                        }
                        else if (linetype == INLINE_COMMENT_TYPE)
                        {
                            if (trimmedSource[i].IndexOf("//") == 0 || trimmedSource[i].IndexOf("#pragma") == 0)
                            {
                                if (trimmedSource[i].IndexOf("////") > NOINDX)
                                {
                                    sourceLineDescription[i] = QUAD_COMMENT;
                                }
                                else if (trimmedSource[i].IndexOf("///") > NOINDX)
                                {
                                    sourceLineDescription[i] = DOC_COMMENT;
                                }
                                else
                                {
                                    sourceLineDescription[i] = COMMENT_LINE;
                                }
                            }
                            else
                            {
                                sourceLineDescription[i] = COMMENT_TRAILING;
                                if (trimmedSource[i][..trimmedSource[i].IndexOf("//")].Trim().Equals("{")) { sourceLineDescription[i] |= OPENING_BRACKET; }
                                if (trimmedSource[i][..trimmedSource[i].IndexOf("//")].Trim().Equals("}")) { sourceLineDescription[i] |= CLOSING_BRACKET; }
                            }
                        }
                        else if (linetype == BLOCK_COMMENT_LINE)
                        {
                            if (trimmedSource[i].IndexOf("/*") == 0)
                            {
                                sourceLineDescription[i] = COMMENT_BLOCK;
                                sourceState = SourceState.InsideBlockComment;
                            }
                            else
                            {
                                Debug.WriteLine($"fileNr = {fileNr,6} non-isolated start of block comment found i = {i + 1,10} file={fileNamePath}");
                            }
                        }
                        else if (linetype == MULTILINE_CANDIDATE_TYPE)
                        {
                            if (MultilineDelimiterStart(trimmedSource[i]))
                            {
                                if (trimmedSource[i].IndexOf("//") > -1 && trimmedSource[i].IndexOf("//") < trimmedSource[i].IndexOf("@\""))
                                {
                                    Debug.WriteLine($"fileNr = {fileNr,6} double slash found before multiline delimiter i = {i + 1,10} file={fileNamePath}");
                                }
                                if (trimmedSource[i].IndexOf("\"") > -1 && trimmedSource[i].IndexOf("\"") < trimmedSource[i].IndexOf("@\""))
                                {
                                    Debug.WriteLine($"fileNr = {fileNr,6} single quote found before multiline delimiter i = {i + 1,10} file={fileNamePath}");
                                }
                                sourceState = SourceState.InsideMultiString;
                                sourceLineDescription[i] = MULTISTRING;
                            }
                            else
                            {
                                // Debug.WriteLine($"fileNr = {fileNr,6} multiline candidate found, but not a multiline i = {i + 1,10} file={fileNamePath}");
                                sourceLineDescription[i] = GEN_LINE;
                            }
                        }
                        else
                        {
                            throw new Exception("Should not happen");
                        }
                        break;

                    case SourceState.InsideBlockComment:
                        // -- check for blank lines in block comments
                        //if (string.IsNullOrEmpty(trimmedSource[i])) {}
                        sourceState = SourceState.InsideBlockComment;
                        sourceLineDescription[i] = COMMENT_BLOCK;
                        if (trimmedSource[i].LastIndexOf("*/") > -1)
                        {
                            sourceState = SourceState.None;
                        }
                        break;

                    case SourceState.InsideMultiString:
                        sourceState = SourceState.InsideMultiString;
                        sourceLineDescription[i] = MULTISTRING;
                        if (MultilineDelimiterEnd(trimmedSource[i]))
                        {
                            // -- check for trailing comments
                            // if (trimmedSource[i].IndexOf("//") > trimmedSource[i].LastIndexOf(";")) {}
                            sourceState = SourceState.None;
                        }
                        break;

                    default:
                        throw new Exception("Should not happen");
                }
            }
        }

        // source-line is a multi-line string delimeter (start) if it has a single " after removing double ""
        public static bool MultilineDelimiterStart(string line)
        {
            if (line.IndexOf("@\"") > -1)
            {
                var reduced = line.Replace("{.*}", "").Replace("\"\"", "");
                return reduced.IndexOf("\"") != -1 && reduced.LastIndexOf("\"") == reduced.IndexOf("\"");
            }
            else
            {
                return false;
            }
        }

        // code is a multi-line string delimeter (end) if it has a single " after all double ""
        public static bool MultilineDelimiterEnd(string line)
        {
            var reduced = Regex.Replace(line, "{.*}", "").Replace("\"\"", "");
            return reduced.IndexOf("\"") != -1 &&
                (reduced.IndexOf("\"") == reduced.Length - 1 || reduced[reduced.IndexOf("\"") + 1] != '"');
        }

        private const int NOINDX = -1;
        private const int GENERIC_LINE_TYPE = 1;
        private const int BLANK_LINE_TYPE = 3;
        private const int INLINE_COMMENT_TYPE = 4;
        private const int BLOCK_COMMENT_LINE = 5;
        private const int MULTILINE_CANDIDATE_TYPE = 6;
        private static Regex matchStringLiterals = new("\\\"(?:\\\\.|[^\\\\\"])*\\\"");

        private static int GetLineType(string[] trimmedSource, int i)
        {
            if (string.IsNullOrEmpty(trimmedSource[i]))
            {
                return BLANK_LINE_TYPE;
            }
            // matched comments include uri strings (and strings in general)
            var inlineCommentPos = trimmedSource[i].IndexOf("//");
            var blockCommentPos =
                trimmedSource[i].IndexOf("/*") > NOINDX && trimmedSource[i].IndexOf("*/") == NOINDX ?
                trimmedSource[i].IndexOf("/*") : NOINDX;
            var multiLineStringPos = trimmedSource[i].IndexOf("@\"");
            var quotePos = trimmedSource[i].IndexOf("\"");
            var leastNonZeroIndex = LeastNonZero(inlineCommentPos, blockCommentPos, multiLineStringPos);
            while (quotePos > NOINDX && leastNonZeroIndex > 0 && quotePos < leastNonZeroIndex)
            {
                trimmedSource[i] = Regex.Replace(trimmedSource[i], "{.*}", "");
                trimmedSource[i] = matchStringLiterals.Replace(trimmedSource[i], "", 1);
                inlineCommentPos = inlineCommentPos > NOINDX ? trimmedSource[i].IndexOf("//") : NOINDX;
                blockCommentPos = blockCommentPos > NOINDX ? trimmedSource[i].IndexOf("/*") : NOINDX;
                multiLineStringPos = multiLineStringPos > NOINDX ? trimmedSource[i].IndexOf("@\"") : NOINDX;
                quotePos = trimmedSource[i].IndexOf("\"");
                leastNonZeroIndex = LeastNonZero(inlineCommentPos, blockCommentPos, multiLineStringPos);
            }
            if (multiLineStringPos == NOINDX && inlineCommentPos == NOINDX && blockCommentPos == NOINDX)
            {
                return GENERIC_LINE_TYPE;
            }
            if (inlineCommentPos > NOINDX && blockCommentPos > NOINDX && inlineCommentPos == blockCommentPos ||
                inlineCommentPos > NOINDX && multiLineStringPos > NOINDX && inlineCommentPos == multiLineStringPos ||
                blockCommentPos > NOINDX && multiLineStringPos > NOINDX && blockCommentPos == multiLineStringPos)
            { throw new Exception("Should never happen"); }
            if (inlineCommentPos == leastNonZeroIndex) { return INLINE_COMMENT_TYPE; }
            if (blockCommentPos == leastNonZeroIndex) { return BLOCK_COMMENT_LINE; }
            if (multiLineStringPos == leastNonZeroIndex) { return MULTILINE_CANDIDATE_TYPE; }
            throw new Exception("Should never happen");
        }

        private static int LeastNonZero(int val1, int val2, int val3)
        {
            List<int> vals = new() { val1, val2, val3 };
            vals.Sort();
            if (vals[0] >= 0) { return vals[0]; }
            if (vals[1] >= 0) { return vals[1]; }
            return vals[2];
        }

        public static string[] GetTrimmedSource(string[] sourceLines)
        {
            var trimmedSource = new string[sourceLines.Length];
            for (var i = 0; i < sourceLines.Length; i++)
            {
                trimmedSource[i] = sourceLines[i].Trim();
            }
            return trimmedSource;
        }

        public static string[] ReadFileAsStringArray(string filenamepath, int padSourceWithBlankLines = 0)
        {
            var lineCount = File.ReadLines(filenamepath).Count() + padSourceWithBlankLines;
            var sourceLines = Enumerable.Repeat("", lineCount).ToArray();
            var lineInd = 0;
            foreach (var line in File.ReadAllLines($"{filenamepath}"))
            {
                sourceLines[lineInd++] = line;
            }
            return sourceLines;
        }

        private void MarkLinesWithSourceAnalysis(string fileNamePath, int eolPreference, int bomPreference)
        {
            FileWriter fw = new(fileNamePath, EOL: eolPreference, useBom: bomPreference);
            for (var i = 0; i < sourceLines.Length; i++)
            {
                fw.WriteLine($"{sourceLines[i],-80}    // {GetSourceTypeDescription(sourceLineDescription[i])}");
            }
            FileWriter.CloseFileWriter(fw);
        }

        private void ShowSourceAnalysis()
        {
            for (var i = 0; i < sourceLines.Length; i++)
            {
                Debug.WriteLine($"{sourceLines[i],-140} {GetSourceTypeDescription(sourceLineDescription[i])}");
                // Debug.WriteLine($"{sourceLines[i],-140} {sourceLineDescription[i]}");
            }
        }

        private void ShowFixedFile()
        {
            for (var i = 0; i < sourceLines.Length; i++)
            {
                // if (!blankLinesForRemoval[i])
                if (removeBlankLineOfType[i] > 0)
                {
                    Debug.WriteLine($"{sourceLines[i]}");
                }
            }
        }
    }
}

