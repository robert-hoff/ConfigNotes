using CSharpSnippets.FormElements;
using CSharpSnippets.Testruns;

namespace CSharpSnippets
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            // FormSnippets();
            RunSnippets();
        }

        static void FormSnippets()
        {
            ApplicationConfiguration.Initialize();
            Application.Run(new MainForm());
        }

        static void RunSnippets()
        {
            TestFileIO.RunTrials();
        }
    }
}

