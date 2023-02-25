using System.Diagnostics;

namespace CSharpSnippets.EditorConfigs
{
    public static class EditorConfigsSnippets
    {
        public static void RunSnippets()
        {
            PrintConfigs();
            // TypeCast();
            // ExplicitTupleNames();
        }

        public static void PrintConfigs()
        {
            for (int i = 2243; i <= 2260; i += 1)
            {
                Debug.WriteLine($"# CA{i:0000}");
                Debug.WriteLine($"dotnet_diagnostic.CA{i:0000}.severity = none");
                Debug.WriteLine($"");
            }
        }

        public static void TypeCast()
        {
            double myDouble = 9.0;
            int myInt = (int) myDouble;
            Debug.WriteLine($"myInt = {myInt}");
        }

        public static void ExplicitTupleNames()
        {
            (string name, int age) customer = ("Hoff Inc", 30);
            string name = customer.name;
            Debug.WriteLine($"name = {name}");
        }
    }
}

