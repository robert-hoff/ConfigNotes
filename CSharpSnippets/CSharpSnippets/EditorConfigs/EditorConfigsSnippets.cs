using System.Diagnostics;
namespace CSharpSnippets.EditorConfigs
{
    public class EditorConfigsSnippets
    {
        public static void RunSnippets()
        {
            ExplicitTupleNames();
        }

        public static void ExplicitTupleNames()
        {
            (string name, int age) customer = ("Hoff Inc", 30);
            string name = customer.Item1;
            Debug.WriteLine($"name = {name}");
        }
    }
}

