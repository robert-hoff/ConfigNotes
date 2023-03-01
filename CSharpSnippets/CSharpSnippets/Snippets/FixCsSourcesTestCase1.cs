using System.Diagnostics;
namespace CSharpSnippets.Snippets
{
    internal class FixCsSourcesTestCase1
    {
        public static void Run()
        {
            /*
             * This is currently marked incorrectly as a comment
             */
            string notMultline1 = @"  //  "" "" ";


            /* hello comment */ string multiline1 = @"  //  ""
                    1
                    "" """"
                    "" ";

            // string multiline1 = @"  //  ""
            string multiline2 = @"
                    1
                    ;;
                    ;;

                    "; string sdsdf = "";


            string multiline3 = @"
Block[B0] - Entry
    Statements (0)
    Next (Regular) Block[B1]
Block[B1] - Block
    Predecessors: [B0]
    Statements (1)
        ISimpleAssignmentOperation (OperationKind.SimpleAssignment, Type: System.Int32, IsImplicit) (Syntax: '= 0')
          Left: 
            IParameterReferenceOperation: p (OperationKind.ParameterReference, Type: System.Int32, IsImplicit) (Syntax: '= 0')
          Right: 
            ILiteralOperation (OperationKind.Literal, Type: System.Int32, Constant: 0) (Syntax: '0')

    Next (Regular) Block[B2]
Block[B2] - Exit
    Predecessors: [B1]
    Statements (0)"
           ;


        //";

        // ;


        /*

        string multiline2 = @"
                1
                ";


         */



        double a = 3;
            double b = 4;
            Console.WriteLine($"Area of the right triangle with legs of {a} and {b /* hello */} is {0.5 * a * b}");
            Console.WriteLine($"Length {($"hello".PadRight(20)) /*  */}");

            Console.WriteLine($"Length {($"h\"ello".PadRight(20)) /* " " " " " */}"); /// comment with " "" " and other crap in it
            Console.WriteLine($"Length  /*    */ ");


            /*
            Console.WriteLine($"Area of the /* right triangle with legs of ");     */

            /*
            Console.WriteLine($"Area of the { right triangle with legs of ");   }  */


            /*
             *
             *
             *
             *
             *
             *
        //             *
             *
             *
             *
             *
             *
             *
             *
             *
             */



            Debug.WriteLine($"/*   */");



            string urlstring = "https://github.com/robert-hoff";
            string weirdUrlstring = " //github.com/robert-hoff";

            string somestring = "\"";



            /* */

            /*
             * normal multiline comment
             */

            /* */ /*
             * weird multiline comment
             /* */ // sdfsdf

            string notMultline2 = @" @ ""  "; // hkhkjh "" " ";

            string multiline4 = @" @ "" ""; // hkhkjh
                     "" // hello ""
                     "; // hello



            string multiline5 = @"

                    /*

                    */
                     ";

            string source = @"#line 1 """"""
            file.cs
            """"""U8";


            string helloz = (@"
                file.cs
                ""
                " + "\"\"\"");




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


            /*
             *
             *   /*  */

            /*
            *
            *
            *
            */



            string hello = null;
            Debug.WriteLine($"{hello}");
        }
    }
}

