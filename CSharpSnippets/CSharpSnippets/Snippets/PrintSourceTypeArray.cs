using System.Diagnostics;
using CSharpSnippets.PrintMethods;

namespace CSharpSnippets.Snippets
{
    public class PrintSourceTypeArray
    {
        //public enum SourceType
        //{
        //    Gen = 0,
        //    Blk = 1,
        //    CLn = 2,
        //    CLT = 4,
        //    CBlo = 8,
        //    CBin = 16,
        //    DC = 32,
        //    MStr = 64,
        //}

        public enum SourceType
        {
            Generic = 0,
            Blank = 1,
            CommentLine = 2,
            CommentLineTrailing = 4,
            CommentBlock = 8,
            CommentBlockInlined = 16,
            DocComment = 32,
            MultiString = 64,
        }

        public static void Run()
        {
            Trial1();
        }

        public static void Trial1()
        {
            for (int i = 0; i < 128; i++)
            {
                string SourceStr = PrintSourceTypes(boolArray(i));
                Debug.WriteLine($"{i,3}    {SourceStr}");
            }
        }

        public static string PrintSourceTypes(bool[] myBools)
        {
            string types = $"{(myBools[8] ? "ERROR" : "OK   "),-40}";
            if (myBools[0]) { types += $"{(SourceType)(0),-40} "; }
            if (myBools[1]) { types += $"{(SourceType)(1),-40} "; }
            if (myBools[2]) { types += $"{(SourceType)(2),-40} "; }
            if (myBools[3]) { types += $"{(SourceType)(4),-40} "; }
            if (myBools[4]) { types += $"{(SourceType)(8),-40} "; }
            if (myBools[5]) { types += $"{(SourceType)(16),-40} "; }
            if (myBools[6]) { types += $"{(SourceType)(32),-40} "; }
            if (myBools[7]) { types += $"{(SourceType)(64),-40} "; }
            return types.Trim();
        }

        public static bool[] boolArray(int i)
        {
            bool[] boolArray = new bool[9];
            boolArray[0] = i == 0;
            boolArray[1] = (i & 1) > 0;
            boolArray[2] = (i & 2) > 0;
            boolArray[3] = (i & 4) > 0;
            boolArray[4] = (i & 8) > 0;
            boolArray[5] = (i & 16) > 0;
            boolArray[6] = (i & 32) > 0;
            boolArray[7] = (i & 64) > 0;
            // error state
            if ((i & 1) > 0 && i > 1)
            {
                boolArray[8] = true;
            }
            if (countTrue(boolArray) >= 2)
            {
                boolArray[8] = true;
            }
            return boolArray;
        }

        public static int countTrue(bool[] boolArray)
        {
            int trueCount = 0;
            foreach (bool v in boolArray)
            {
                if (v) { trueCount++; }
            }
            return trueCount;
        }
    }
}

