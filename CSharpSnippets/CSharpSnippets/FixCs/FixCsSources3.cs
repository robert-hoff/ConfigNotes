using System.Diagnostics;
using System.Text.RegularExpressions;
using CSharpSnippets.PrintMethods;

namespace CSharpSnippets.FixCs
{
    public class FixCsSources3
    {
        public enum SourceType
        {
            Generic = 0,
            Blank = 1,
            CommentLine = 2,
            CommentLineTrailing = 4,
            CommentBlock = 8,
            CommentBlockInlined = 16,
            DocComment = 32,
            QuadComment = 64,
            MultiString = 128,
        }


        private int[] sourceType;
        private bool[] sourceTypeValidState = Enumerable.Repeat(true, 256).ToArray();
        private string[] reportSourceState = Enumerable.Repeat("", 256).ToArray();
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

            SetValidStates(sourceTypeValidState, reportSourceState);
            this.fileNamePath = fileNamePath;
            this.fileNr = fileNr;
            Debug.WriteLine($"{fileNr,9} {fileNamePath}");
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
                RemoveAnyProblematicStringLiterals(trimmedSource, i);

                switch (sourceState)
                {
                    case SourceState.None:
                        if (string.IsNullOrEmpty(trimmedSource[i]))
                        {
                            sourceType[i] |= (int) SourceType.Blank;
                        }
                        else if (trimmedSource[i].IndexOf("//") > -1)
                        {
                            // unambigously a single-line comment
                            if (trimmedSource[i].IndexOf("//") == 0 || trimmedSource[i].IndexOf("#pragma") == 0)
                            {
                                // TODO differentiate between normal, doc and quad commments
                                sourceType[i] |= (int) SourceType.CommentLine;
                            }
                            else
                            {
                                if (!knownTrailingComments.ContainsKey(fileNr))
                                {
                                    Debug.WriteLine($"fileNr = {fileNr,6} trailing commentZ found at line i = {i + 1,10} file={fileNamePath}");
                                    Debug.WriteLine($"{trimmedSource[i]}");
                                }
                                sourceType[i] |= (int) SourceType.CommentLineTrailing;
                            }
                        }
                        else if (trimmedSource[i].IndexOf("/*") > -1 && trimmedSource[i].IndexOf("*/") == -1)
                        {
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
                        }
                        else if (MultilineDelimiterStart(trimmedSource[i]))
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
                        break;
                    case SourceState.InsideBlockComment:
                        if (string.IsNullOrEmpty(trimmedSource[i]))
                        {
                            Debug.WriteLine($"fileNr = {fileNr,6} blank line found in block comment i = {i + 1,10} file={fileNamePath}");
                        }
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

        private static Regex matchStringLiterals = new Regex("\\\"(?:\\\\.|[^\\\\\"])*\\\"");

        private void RemoveAnyProblematicStringLiterals(string[] trimmedSource, int i)
        {
            string trimmerSourceString = trimmedSource[i];



            trimmedSource[i] = Regex.Replace(trimmedSource[i], "{.*}", "");
            while (trimmedSource[i].IndexOf("\"") > -1 && trimmedSource[i].IndexOf("\"") < trimmedSource[i].IndexOf("//"))
            {
                trimmedSource[i] = matchStringLiterals.Replace(trimmedSource[i], "", 1);
            }
        }




        public static bool BlockCommentStart(string line)
        {
            return line.IndexOf("/*") > -1 && line.IndexOf("*/") == -1;
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
            validSourceStates[(int) SourceType.Generic] = true;
            validSourceStates[(int) SourceType.Blank] = true;
            validSourceStates[(int) SourceType.CommentLine] = true;
            validSourceStates[(int) SourceType.CommentLineTrailing] = true;
            validSourceStates[(int) SourceType.CommentBlock] = true;
            validSourceStates[(int) SourceType.CommentBlockInlined] = true;
            validSourceStates[(int) SourceType.DocComment] = true;
            validSourceStates[(int) SourceType.QuadComment] = true;
            validSourceStates[(int) SourceType.MultiString] = true;
            reportSourceState[(int) SourceType.Generic] = $"{SourceType.Generic}";
            reportSourceState[(int) SourceType.Blank] = $"{SourceType.Blank}";
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

