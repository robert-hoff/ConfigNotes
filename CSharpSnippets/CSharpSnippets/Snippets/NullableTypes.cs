using System.Diagnostics;
namespace CSharpSnippets.Snippets
{
    internal class NullableTypes
    {
        public static void Run()
        {
            string source = "";

            CompileAndVerifyIL(source, "C.M", @"
                    .
                    .
                    ");


            string hello = null;
            Debug.WriteLine($"{ hello}");
        }


        public static void CompileAndVerifyIL(string s, string t, string u)
        {
        } // comment



    }
}

