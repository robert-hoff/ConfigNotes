using System.Diagnostics;
namespace CSharpSnippets.Snippets
{
    internal class NullableTypes2
    {
        public static void Run()
        {
            /* */

            /*
             * normal multiline comment
             */

            /* */ /*
             * weird multiline comment
             /* */ // sdfsdf

            string hello = null;
            Debug.WriteLine($"{hello}");
        }
    }
}

