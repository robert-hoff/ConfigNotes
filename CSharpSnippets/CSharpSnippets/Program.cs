using CSharpSnippets.EditorConfigs;
using CSharpSnippets.FormElements;
using CSharpSnippets.Testruns;

namespace CSharpSnippets
{
    public static class Program
    {
        [STAThread]
        private static void Main()
        {
            // FormSnippets();
            RunSnippets();
        }

        public static void FormSnippets()
        {
            ApplicationConfiguration.Initialize();
            Application.Run(new MainForm());
        }

        public static void EditorConfigs()
        {
            EditorConfigsSnippets.RunSnippets();
        }

        public static void RunSnippets()
        {
            // TestFileIO.RunTrials();
            RunFixCsFiless.RunTrials();
        }
    }
}

