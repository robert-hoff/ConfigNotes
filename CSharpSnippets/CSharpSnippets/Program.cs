using CSharpSnippets.FormElements;

namespace CSharpSnippets
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            FormSnippets();
        }

        static void FormSnippets()
        {
            ApplicationConfiguration.Initialize();
            Application.Run(new MainForm());
        }
    }
}

