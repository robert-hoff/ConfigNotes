using CSharpSnippets.EditorConfigs;
using CSharpSnippets.FixCs;
using CSharpSnippets.FormElements;
using CSharpSnippets.Snippets;

namespace CSharpSnippets
{
    public static class Program
    {
        [STAThread]
        private static void Main()
        {
            // FormSnippets();
            // CSharpSnippets();
            // EditorConfigs();
            // RunSnippets();
            CopyOverConfigs();
            // FixSelf();
        }

        public static void FormSnippets()
        {
            ApplicationConfiguration.Initialize();
            MainForm mainForm = new MainForm();
            Application.Run(mainForm);
            mainForm.Dispose();
        }

        public static void CSharpSnippets()
        {
            NullableTypes.Run();
        }

        public static void EditorConfigs()
        {
            EditorConfigsSnippets.RunSnippets();
        }

        public static void RunSnippets()
        {
            // TestFileIO.RunTrials();
            FixCsSources.Run();
        }

        public static void CopyOverConfigs() { CopyOverEditorConfig.CopyOverEditorconfigFile(); }
        public static void FixSelf() { FixCsSources.FixSelf(); }
    }
}

