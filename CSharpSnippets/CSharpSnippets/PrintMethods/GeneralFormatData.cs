using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpSnippets.PrintMethods
{
    public class GeneralFormatData
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


        //public static void ShowFile(string fileNamePath)
        //{
        //    string[] sourceLines = ReadFileAsStringArray(fileNamePath);
        //    foreach (string line in sourceLines)
        //    {
        //        Debug.WriteLine($"{line}");
        //    }
        //}
    }
}

