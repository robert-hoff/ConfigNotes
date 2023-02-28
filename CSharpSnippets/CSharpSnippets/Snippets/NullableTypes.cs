using System.Diagnostics;
namespace CSharpSnippets.Snippets
{
    internal class NullableTypes
    {
        public static void Run()
        {
            string notmulti = @" @ ""  "; // hkhkjh "" " ";

            string multi = @" @ "" ""; // hkhkjh
                     "" // hello ""
                     "; // hello


            // /*

            /* //
             *
             *
             */

            /* // /*

            dsfsdf
            */

            /* */

            /* */ /*
             * multiline comment
             *
             *
             *
             *
             /* */ // sdfsdf

            string hello = null;
            Debug.WriteLine($"{hello}");
        }
    }
}

