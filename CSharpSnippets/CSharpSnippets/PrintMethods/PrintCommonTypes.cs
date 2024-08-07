using System.Diagnostics;
using CSharpSnippets.FileIO;

namespace CSharpSnippets.PrintMethods
{
    public static class PrintCommonTypes
    {
        public static void ShowBoolArray(bool[] boolArray)
        {
            string boolStr = "";
            for (int i = 0; i < boolArray.Length; i++)
            {
                boolStr += boolArray[i] ? "T " : "F ";
            }
            Debug.WriteLine($"{boolStr.Trim()}");
        }

        public static void ShowStringArray(string[] stringArray)
        {
            for (int i = 0; i < stringArray.Length; i++)
            {
                Debug.WriteLine($"{stringArray[i]}");
            }
        }

        public static void ShowStringList(List<string> stringList)
        {
            for (int i = 0; i < stringList.Count; i++)
            {
                Debug.WriteLine($"{stringList[i]}");
            }
        }

        public static void ShowFile(string fileNamePath)
        {
            string[] sourceLines = FileQueries.ReadFileAsStringArray(fileNamePath);
            foreach (string line in sourceLines)
            {
                Debug.WriteLine($"{line}");
            }
        }
    }
}

