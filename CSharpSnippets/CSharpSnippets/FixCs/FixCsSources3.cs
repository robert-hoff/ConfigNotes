using System.Diagnostics;
using System.Text.RegularExpressions;
using CSharpSnippets.FileIO;
using CSharpSnippets.PrintMethods;

namespace CSharpSnippets.FixCs
{
    public class FixCsSources3
    {
        // Roslyn preferences
        private const int DEFAULT_DESIRED_BLANK_LINES_AT_EOF = 1;
        private const int DEFAULT_EOL_PREFERENCE = FileWriter.WINDOWS_ENDINGS;
        private const int DEFAULT_BOM_PREFERENCE = FileWriter.SAVE_UTF_FILE_WITH_BOM;

        public enum SourceType
        {
            GenericUnassigned = 1,
            GenericLine = 2,
            BlankLine = 3,
            CommentLine = 4,
            CommentLineTrailing = 5,
            CommentBlock = 6,
            CommentBlockInlined = 7,
            DocComment = 8,
            QuadComment = 9,
            MultiString = 10,
            OpeningBracket = 11,
            ClosingBracket = 12,
        }

        private int[] sourceType;
        private string fileNamePath;
        private int fileNr;
        private string[] sourceLines;
        private string[] trimmedSource;

        public FixCsSources3(string fileNamePath, int fileNr, bool showFixedLines = false)
        {
            this.fileNamePath = fileNamePath;
            this.fileNr = fileNr;
            Debug.WriteLine($" {fileNr,9} Fixing C# source for {fileNamePath}");
            sourceLines = ReadFileAsStringArray(fileNamePath);
            sourceType = new int[sourceLines.Length];
            trimmedSource = GetTrimmedSource(sourceLines);
            AnalyseSourceFirstPass();
            if (showFixedLines)
            {
                for (int i = 0; i < sourceLines.Length; i++)
                {
                    Debug.WriteLine($"{sourceLines[i],-140} {(SourceType) sourceType[i]}");
                }
            }

            // MarkLinesWithSourceAnalysis(fileNamePath, DEFAULT_EOL_PREFERENCE, DEFAULT_BOM_PREFERENCE);
        }

        public enum SourceState
        {
            None, InsideBlockComment, InsideMultiString
        }




        //private void MarkLinesForRemoval()
        //{

        //}





        private void AnalyseSourceFirstPass()
        {
            SourceState sourceState = SourceState.None;
            for (int i = 0; i < trimmedSource.Length; i++)
            {
                switch (sourceState)
                {
                    case SourceState.None:

                        int linetype = GetLineType(trimmedSource, i);





                        switch (linetype)
                        {
                            case BLANK_LINE:
                                sourceType[i] |= (int) SourceType.BlankLine;
                                break;

                            case GENERIC_LINE:
                                sourceType[i] |= (int) SourceType.GenericLine;
                                break;

                            case INLINE_COMMENT_LINE:
                                if (trimmedSource[i].IndexOf("//") == 0 || trimmedSource[i].IndexOf("#pragma") == 0)
                                {
                                    if (trimmedSource[i].IndexOf("////") > NOINDX)
                                    {
                                        sourceType[i] |= (int) SourceType.QuadComment;
                                    }
                                    else if (trimmedSource[i].IndexOf("///") > NOINDX)
                                    {
                                        sourceType[i] |= (int) SourceType.DocComment;
                                    }
                                    else
                                    {
                                        sourceType[i] |= (int) SourceType.CommentLine;
                                    }
                                }
                                else
                                {
                                    sourceType[i] |= (int) SourceType.CommentLineTrailing;
                                }
                                break;

                            case BLOCK_COMMENT_LINE:
                                // unambigously start of a block comment
                                if (trimmedSource[i].IndexOf("/*") == 0)
                                {
                                    sourceType[i] |= (int) SourceType.CommentBlock;
                                    sourceState = SourceState.InsideBlockComment;
                                }
                                else
                                {
                                    Debug.WriteLine($"fileNr = {fileNr,6} non-isolated start of block comment found i = {i + 1,10} file={fileNamePath}");
                                }
                                break;

                            case MULTILINE_CANDIDATE:
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
                                    sourceType[i] |= (int) SourceType.MultiString;
                                }
                                else
                                {
                                    // Debug.WriteLine($"fileNr = {fileNr,6} multiline candidate found, but not a multiline i = {i + 1,10} file={fileNamePath}");
                                    sourceType[i] |= (int) SourceType.GenericLine;
                                }
                                break;

                            default:
                                throw new Exception("should never happen");
                        }



                        break;



                    case SourceState.InsideBlockComment:
                        // -- could potentially cleanup these
                        //if (string.IsNullOrEmpty(trimmedSource[i]))
                        //{
                        //    Debug.WriteLine($"fileNr = {fileNr,6} blank line found in block comment i = {i + 1,10} file={fileNamePath}");
                        //}
                        sourceState = SourceState.InsideBlockComment;
                        sourceType[i] |= (int) SourceType.CommentBlock;
                        if (trimmedSource[i].LastIndexOf("*/") > -1)
                        {
                            sourceState = SourceState.None;
                        }
                        break;

                    case SourceState.InsideMultiString:
                        sourceState = SourceState.InsideMultiString;
                        sourceType[i] |= (int) SourceType.MultiString;
                        if (MultilineDelimiterEnd(trimmedSource[i]))
                        {
                            // if (trimmedSource[i].IndexOf("//") > trimmedSource[i].LastIndexOf(";"))
                            // {
                            //     Debug.WriteLine($"fileNr = {fileNr,6} trailing comment found in multilinedelimiter i = {i + 1,10} file={fileNamePath}");
                            //     Debug.WriteLine($"{trimmedSource[i]}");
                            // }
                            sourceState = SourceState.None;
                        }
                        break;

                    default:
                        throw new Exception("cannot happen");
                }
            }
        }

        // source-line is a multi-line string delimeter (start) if it has a single " after removing all double ""
        public static bool MultilineDelimiterStart(string line)
        {
            if (line.IndexOf("@\"") > -1)
            {
                string reduced = line.Replace("{.*}", "").Replace("\"\"", "");
                return reduced.IndexOf("\"") != -1 && reduced.LastIndexOf("\"") == reduced.IndexOf("\"");
            }
            else
            {
                return false;
            }
        }

        // code is a multi-line string delimeter (end)if it has a single " after all double ""
        public static bool MultilineDelimiterEnd(string line)
        {
            string reduced = Regex.Replace(line, "{.*}", "").Replace("\"\"", "");
            return reduced.IndexOf("\"") != -1 &&
                (reduced.IndexOf("\"") == reduced.Length - 1 || reduced[reduced.IndexOf("\"") + 1] != '"');
        }


        private const int NOINDX = -1;
        private const int GENERIC_LINE = 1;
        private const int GENERIC_LINE_WARNING = 2;
        private const int BLANK_LINE = 3;
        private const int INLINE_COMMENT_LINE = 4;
        private const int BLOCK_COMMENT_LINE = 5;
        private const int MULTILINE_CANDIDATE = 6;
        // private const int FAILED_TO_PARSE = 7;
        private static Regex matchStringLiterals = new Regex("\\\"(?:\\\\.|[^\\\\\"])*\\\"");

        private static int GetLineType(string[] trimmedSource, int i)
        {
            if (string.IsNullOrEmpty(trimmedSource[i]))
            {
                return BLANK_LINE;
            }
            // matched comments include uri strings (and strings in general)
            int inlineCommentPos = trimmedSource[i].IndexOf("//");
            int blockCommentPos =
                trimmedSource[i].IndexOf("/*") > NOINDX && trimmedSource[i].IndexOf("*/") == NOINDX ?
                trimmedSource[i].IndexOf("/*") : NOINDX;
            int multiLineStringPos = trimmedSource[i].IndexOf("@\"");
            int quotePos = trimmedSource[i].IndexOf("\"");
            int leastNonZeroIndex = LeastNonZero(inlineCommentPos, blockCommentPos, multiLineStringPos);
            if (i > 7)
            {
                int dafsdaf = 23;

            }
            string trimma = trimmedSource[i];


            while (quotePos > NOINDX && leastNonZeroIndex > 0 && quotePos < leastNonZeroIndex)
            {
                if (i > 7)
                {
                    int asdfsdf = 098;
                }

                trimmedSource[i] = Regex.Replace(trimmedSource[i], "{.*}", "");
                trimmedSource[i] = matchStringLiterals.Replace(trimmedSource[i], "", 1);

                string trimma2 = trimmedSource[i];
                int sfsdf = 90;

                // Debug.WriteLine($"{trimmedSource[i]}");


                inlineCommentPos = inlineCommentPos > NOINDX ? trimmedSource[i].IndexOf("//") : NOINDX;
                blockCommentPos = blockCommentPos > NOINDX ? trimmedSource[i].IndexOf("/*") : NOINDX;
                multiLineStringPos = multiLineStringPos > NOINDX ? trimmedSource[i].IndexOf("@\"") : NOINDX;
                quotePos = trimmedSource[i].IndexOf("\"");
                leastNonZeroIndex = LeastNonZero(inlineCommentPos, blockCommentPos, multiLineStringPos);

                if (i > 7)
                {
                    int asdfsdfs = 098;
                }
                //if (i == 9604)
                //{
                //    trimmedSource[0] = ".";
                //    return INLINE_COMMENT_LINE;
                //}

                if (i == 1843)
                {
                    trimmedSource[0] = ".";
                    return INLINE_COMMENT_LINE;
                }

            }
            if (multiLineStringPos == NOINDX && inlineCommentPos == NOINDX && blockCommentPos == NOINDX)
            {
                return GENERIC_LINE;
            }
            if (inlineCommentPos > NOINDX && blockCommentPos > NOINDX && inlineCommentPos == blockCommentPos ||
                inlineCommentPos > NOINDX && multiLineStringPos > NOINDX && inlineCommentPos == multiLineStringPos ||
                blockCommentPos > NOINDX && multiLineStringPos > NOINDX && blockCommentPos == multiLineStringPos)
            { throw new Exception("Should never happen"); }
            if (inlineCommentPos == leastNonZeroIndex) { return INLINE_COMMENT_LINE; }
            if (blockCommentPos == leastNonZeroIndex) { return BLOCK_COMMENT_LINE; }
            if (multiLineStringPos == leastNonZeroIndex) { return MULTILINE_CANDIDATE; }
            throw new Exception("Should never happen");
        }

        private static int LeastNonZero(int val1, int val2, int val3)
        {
            var vals = new List<int> { val1, val2, val3 };
            vals.Sort();
            if (vals[0] >= 0) { return vals[0]; }
            if (vals[1] >= 0) { return vals[1]; }
            return vals[2];
        }

        private static void SetValidStates(bool[] validSourceStates, string[] reportSourceState)
        {
            //validSourceStates[(int) SourceType.GenericUnassigned] = true;
            //validSourceStates[(int) SourceType.BlankLine] = true;
            //validSourceStates[(int) SourceType.CommentLine] = true;
            //validSourceStates[(int) SourceType.CommentLineTrailing] = true;
            //validSourceStates[(int) SourceType.CommentBlock] = true;
            //validSourceStates[(int) SourceType.CommentBlockInlined] = true;
            //validSourceStates[(int) SourceType.DocComment] = true;
            //validSourceStates[(int) SourceType.QuadComment] = true;
            //validSourceStates[(int) SourceType.MultiString] = true;

            reportSourceState[(int) SourceType.GenericUnassigned] = $"{SourceType.GenericUnassigned}";
            reportSourceState[(int) SourceType.GenericLine] = $"{SourceType.GenericLine}";
            reportSourceState[(int) SourceType.BlankLine] = $"{SourceType.BlankLine}";
            reportSourceState[(int) SourceType.CommentLine] = $"{SourceType.CommentLine}";
            reportSourceState[(int) SourceType.CommentLineTrailing] = $"{SourceType.CommentLineTrailing}";
            reportSourceState[(int) SourceType.CommentBlock] = $"{SourceType.CommentBlock}";
            reportSourceState[(int) SourceType.CommentBlockInlined] = $"{SourceType.CommentBlockInlined}";
            reportSourceState[(int) SourceType.DocComment] = $"{SourceType.DocComment}";
            reportSourceState[(int) SourceType.QuadComment] = $"{SourceType.QuadComment}";
            reportSourceState[(int) SourceType.MultiString] = $"{SourceType.MultiString}";
        }

        public static string[] GetTrimmedSource(string[] sourceLines)
        {
            string[] trimmedSource = new string[sourceLines.Length];
            for (int i = 0; i < sourceLines.Length; i++)
            {
                trimmedSource[i] = sourceLines[i].Trim();
            }
            return trimmedSource;
        }

        public static string[] ReadFileAsStringArray(string filenamepath)
        {
            return File.ReadAllLines($"{filenamepath}");
        }




        private void MarkLinesWithSourceAnalysis(string fileNamePath, int eolPreference, int bomPreference)
        {
            FileWriter fw = new FileWriter(fileNamePath, EOL: eolPreference, useBom: bomPreference);
            for (int i = 0; i < sourceLines.Length; i++)
            {
                fw.WriteLine($"{sourceLines[i],-80}    // {(SourceType) sourceType[i]}");
            }

            FileWriter.CloseFileWriter(fw);
        }
    }
}

