#pragma warning disable IDE0005 // Using directive is unnecessary.
using CSharpSnippets.EditorConfigs;
using CSharpSnippets.FixCs;
using CSharpSnippets.FormElements;
using CSharpSnippets.Snippets;
using CSharpSnippets.Testruns;

namespace CSharpSnippets
{
    public static class Program
    {
        [STAThread]
        private static void Main()
        {
            // FormSnippets();
            CSharpSnippets();
            // EditorConfigs();
            // CopyOverConfigs();
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
            // CheckMultilineDelimiter.Run();
            // NullableTypes.Run();
            // TestFileIO.Run();
            // FixCsSources.Run();
            // FixCsSources2.CleanupRoslynSources();
            // RegexReplacements.Run();
            // AnalyseCsSources.Run();
            // GenericCSharp.Run();
            AnalyseCsSources2.Run();
            // PrintSourceTypeArray.Run();
        }

        public static void EditorConfigs()
        {
            EditorConfigsSnippets.RunSnippets();
        }

        public static void CopyOverConfigs() { CopyOverEditorConfig.CopyOverEditorconfigFile(); }
        public static void FixSelf() { FixCsSources.FixSelf(); }
    }
}

