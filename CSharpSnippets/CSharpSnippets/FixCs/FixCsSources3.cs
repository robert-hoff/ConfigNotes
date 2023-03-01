using System.Diagnostics;
using System.Text.RegularExpressions;
using CSharpSnippets.PrintMethods;

namespace CSharpSnippets.FixCs
{
    public class FixCsSources3
    {
        public enum SourceType
        {
            GenericUnassigned = 0,
            GenericLine = 1,
            BlankLine = 2,
            CommentLine = 4,
            CommentLineTrailing = 8,
            CommentBlock = 16,
            CommentBlockInlined = 32,
            DocComment = 64,
            QuadComment = 128,
            MultiString = 256,
        }

        private int[] sourceType;
        private bool[] sourceTypeValidState = Enumerable.Repeat(true, 512).ToArray();
        // FIXME - this is a bit excessive
        private string[] reportSourceState = Enumerable.Repeat("", 512).ToArray();
        private string fileNamePath;
        private int fileNr;
        private string[] sourceLines;
        private string[] trimmedSource;
        private Dictionary<int, bool> knownTrailingComments = new();


        public FixCsSources3(string fileNamePath, int fileNr)
        {
            knownTrailingComments[5] = true;
            knownTrailingComments[15] = true;
            knownTrailingComments[21] = true;
            knownTrailingComments[47] = true;
            knownTrailingComments[50] = true;
            knownTrailingComments[51] = true;
            knownTrailingComments[52] = true;
            knownTrailingComments[53] = true;
            knownTrailingComments[77] = true;
            knownTrailingComments[87] = true;
            knownTrailingComments[90] = true;
            knownTrailingComments[137] = true;
            knownTrailingComments[142] = true;
            knownTrailingComments[143] = true;
            knownTrailingComments[146] = true;
            knownTrailingComments[147] = true;
            knownTrailingComments[166] = true;
            knownTrailingComments[194] = true;
            knownTrailingComments[219] = true;
            knownTrailingComments[234] = true;
            knownTrailingComments[241] = true;
            knownTrailingComments[250] = true;
            knownTrailingComments[271] = true;
            knownTrailingComments[279] = true;

            SetValidStates(sourceTypeValidState, reportSourceState);
            this.fileNamePath = fileNamePath;
            this.fileNr = fileNr;
            Debug.WriteLine($" {fileNr,9} Fixing C# source for {fileNamePath}");
            sourceLines = ReadFileAsStringArray(fileNamePath);
            sourceType = new int[sourceLines.Length];
            trimmedSource = GetTrimmedSource(sourceLines);

            // GeneralFormatData.ShowStringArray(trimmedSource);
            AnalyseSourceFirstPass();

            //for (int i = 0; i < sourceLines.Length; i++)
            //{
            //    Debug.WriteLine($"{sourceLines[i],-140} {reportSourceState[sourceType[i]]}");
            //}
        }

        public enum SourceState
        {
            None, InsideBlockComment, InsideMultiString
        }

        private void AnalyseSourceFirstPass()
        {
            // check for trailing comments
            //for (int i = 0; i < trimmedSource.Length; i++)
            //{
            //    if (trimmedSource[i].IndexOf("//") > -1 && trimmedSource[i].IndexOf("//") > 0) {
            //        Debug.WriteLine($"fileNr = {fileNr,6} trailing comment found at line i = {i,10} file={fileNamePath}");
            //        Debug.WriteLine($"{sourceLines[i]}");
            //    }
            //}

            // check for opening block comments that are not at the start of line
            //for (int i = 0; i < trimmedSource.Length; i++)
            //{
            //    if (BlockCommentStart(trimmedSource[i]) && trimmedSource[i].IndexOf("/*") > 0)
            //    {
            //        Debug.WriteLine($"fileNr = {fileNr,6} trailing block comment found at line i = {(i+1),10} file={fileNamePath}");
            //    }
            //}


            // sourceType[100] |= (int) SourceType.CommentBlock;
            // Debug.WriteLine($"{sourceTypeValidState[sourceType[100]]}");



            //for (int i = 0; i < trimmedSource.Length; i++)
            //{
            //    if (MultilineDelimiterStart(trimmedSource[i]))
            //    {
            //        if (trimmedSource[i].IndexOf("\"") < trimmedSource[i].IndexOf("@\""))
            //        {
            //            Debug.WriteLine($"fileNr = {fileNr,6} multiline delimter line i = {(i + 1),10} file={fileNamePath}");
            //        }
            //    }
            //}

            //for (int i = 0; i < trimmedSource.Length; i++)
            //{
            //    if (trimmedSource[i].IndexOf("////") > -1 && trimmedSource[i].IndexOf("////") > 0)
            //    {
            //        Debug.WriteLine($"fileNr = {fileNr,6} trailing QUAD comment found at line i = {i,10} file={fileNamePath}");
            //        Debug.WriteLine($"{sourceLines[i]}");
            //    }
            //}



            SourceState sourceState = SourceState.None;

            for (int i = 0; i < trimmedSource.Length; i++)
            {
                // remove string literals if they are found to interfere
                // RemoveAnyProblematicStringLiterals(trimmedSource, i);
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

                            case GENERIC_LINE_WARNING:
                                sourceType[i] |= (int) SourceType.GenericLine;
                                Debug.WriteLine($"fileNr = {fileNr,6} WARN reached end of method in GetLineType(..) i = {i + 1,10} file={fileNamePath}");
                                break;

                            case INLINE_COMMENT_LINE:
                                sourceType[i] |= (int) SourceType.CommentLine;
                                // unambigously a single-line comment
                                if (trimmedSource[i].IndexOf("//") == 0 || trimmedSource[i].IndexOf("#pragma") == 0)
                                {
                                    // TODO differentiate between normal, doc and quad commments
                                    sourceType[i] |= (int) SourceType.CommentLine;
                                }
                                else
                                {
                                    //if (!knownTrailingComments.ContainsKey(fileNr))
                                    //{
                                    //    Debug.WriteLine($"fileNr = {fileNr,6} trailing comment at line i = {i + 1,10} file={fileNamePath}");
                                    //    Debug.WriteLine($"{trimmedSource[i]}");
                                    //}
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

                            case FAILED_TO_PARSE:
                                Debug.WriteLine($"fileNr = {fileNr,6} parse error, terminating at i = {i + 1,10} file={fileNamePath}");
                                return;
                        }
                        break;

                    case SourceState.InsideBlockComment:
                        // -- could potentially remove such blank lines as part of cleanup
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
                            if (trimmedSource[i].IndexOf("//") > trimmedSource[i].LastIndexOf(";"))
                            {
                                Debug.WriteLine($"fileNr = {fileNr,6} trailing comment found in multilinedelimiter i = {i + 1,10} file={fileNamePath}");
                                Debug.WriteLine($"{trimmedSource[i]}");
                            }
                            sourceState = SourceState.None;
                        }
                        break;

                    default:
                        throw new Exception("cannot happen");
                }
            }
        }



        private const int NOIND = -1;

        private const int GENERIC_LINE = 1;
        private const int GENERIC_LINE_WARNING = 2;
        private const int BLANK_LINE = 3;
        private const int INLINE_COMMENT_LINE = 4;
        private const int BLOCK_COMMENT_LINE = 5;
        private const int MULTILINE_CANDIDATE = 6;
        private const int FAILED_TO_PARSE = 7;

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
                trimmedSource[i].IndexOf("/*") > NOIND && trimmedSource[i].IndexOf("*/") == NOIND ?
                trimmedSource[i].IndexOf("/*") : NOIND;
            int multiLineStringPos = trimmedSource[i].IndexOf("@\"");

            int quotePos = trimmedSource[i].IndexOf("\"");
            int leastNonZeroIndex = LeastNonZero(inlineCommentPos, blockCommentPos, multiLineStringPos);
            while (quotePos > NOIND && leastNonZeroIndex > 0 && quotePos < leastNonZeroIndex)
            {
                trimmedSource[i] = Regex.Replace(trimmedSource[i], "{.*}", "");
                trimmedSource[i] = matchStringLiterals.Replace(trimmedSource[i], "", 1);
                inlineCommentPos = inlineCommentPos > NOIND ? trimmedSource[i].IndexOf("//") : NOIND;
                blockCommentPos = blockCommentPos > NOIND ? trimmedSource[i].IndexOf("/*") : NOIND;
                multiLineStringPos = multiLineStringPos > NOIND ? trimmedSource[i].IndexOf("@/") : NOIND;
                quotePos = trimmedSource[i].IndexOf("\"");
                leastNonZeroIndex = LeastNonZero(inlineCommentPos, blockCommentPos, multiLineStringPos);
            }
            if (multiLineStringPos == NOIND && inlineCommentPos == NOIND && blockCommentPos == NOIND)
            {
                return GENERIC_LINE;
            }

            if (inlineCommentPos > NOIND && blockCommentPos > NOIND && inlineCommentPos == blockCommentPos ||
                inlineCommentPos > NOIND && multiLineStringPos > NOIND && inlineCommentPos == multiLineStringPos ||
                blockCommentPos > NOIND && multiLineStringPos > NOIND && blockCommentPos == multiLineStringPos)
            { throw new Exception("Should never happen"); }

            if (inlineCommentPos == leastNonZeroIndex)
            {
                return INLINE_COMMENT_LINE;
            }
            if (blockCommentPos == leastNonZeroIndex)
            {
                return BLOCK_COMMENT_LINE;
            }
            if (multiLineStringPos == leastNonZeroIndex)
            {
                return MULTILINE_CANDIDATE;
            }









            /*
            // multi   inline   block
            // -       -        *
            // +       -        +     no instances found
            // -       +        +     no instances found
            // +       +        +     no instances found
            if (blockCommentPos > NOIND && (multiLineStringPos > NOIND || inlineCommentPos > NOIND))
            {
                Debug.WriteLine($"Circumstance not implemented " +
                    $"blockCommentPos > NOIND = {blockCommentPos > NOIND} " +
                    $"multiLineStringPos > NOIND = {multiLineStringPos > NOIND} " +
                    $"inlineCommentPos > NOIND = {inlineCommentPos > NOIND}");
                return FAILED_TO_PARSE;
            }

            // multi   inline   block
            // -       -        *
            if (blockCommentPos > NOIND)
            {
                return BLOCK_COMMENT_LINE;
            }

            // multi   inline   block
            // +       -        -
            // +       +         -
            if (multiLineStringPos > NOIND && (
                inlineCommentPos == NOIND || inlineCommentPos > NOIND && multiLineStringPos < inlineCommentPos))
            {
                return MULTILINE_CANDIDATE;
            }


            while (quotePos > NOIND && quotePos < inlineCommentPos)
            {
                trimmedSource[i] = Regex.Replace(trimmedSource[i], "{.*}", "");
                trimmedSource[i] = matchStringLiterals.Replace(trimmedSource[i], "", 1);
                quotePos = trimmedSource[i].IndexOf("\"");
                inlineCommentPos = trimmedSource[i].IndexOf("//");
                multiLineStringPos = trimmedSource[i].IndexOf("@\"");
                if (multiLineStringPos > NOIND && (
                    inlineCommentPos == NOIND || inlineCommentPos > NOIND && multiLineStringPos < inlineCommentPos))
                {
                    return MULTILINE_CANDIDATE;
                }
            }

            if (inlineCommentPos > NOIND)
            {
                return INLINE_COMMENT_LINE;
            }
            if (multiLineStringPos == NOIND && inlineCommentPos == NOIND && blockCommentPos == NOIND)
            {
                return GENERIC_LINE;
            }
            */

            // return GENERIC_LINE_WARNING;
            throw new Exception("Should never happen");
        }


        private static int LeastNonZero(int val1, int val2, int val3)
        {
            List<int> vals = new();
            vals.Add(val1);
            vals.Add(val2);
            vals.Add(val3);
            vals.Sort();
            if (vals[0] < 0 && vals[1] < 0) { return vals[2]; }
            if (vals[0] < 0) { return vals[1]; }
            return vals[0];
        }

        /*
         * C# code is a multi-line string delimeter (start) if it has a single " after removing double ""
         *
         */
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

        /*
         * C# code is a multi-line string delimeter (end)if it has a single " after removing double ""
         *
         */
        public static bool MultilineDelimiterEnd(string line)
        {
            string reduced = line.Replace("{.*}", "").Replace("\"\"", "");
            return reduced.IndexOf("\"") != -1 && reduced.LastIndexOf("\"") == reduced.IndexOf("\"");
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


    }
}

