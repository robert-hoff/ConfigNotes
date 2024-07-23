#pragma warning disable IDE0005 // Using directive is unnecessary.
using System.Diagnostics;
using System.Numerics;
using CSharpSnippets.EditorConfigs;
using CSharpSnippets.FixCs;
using CSharpSnippets.FixCs.RoslynFixer;
using CSharpSnippets.FormElements;
using CSharpSnippets.Snippets;

namespace CSharpSnippets
{
    public static class Program
    {
        [STAThread]
        private static void Main()
        {
            FormSnippets();
            // CSharpSnippets();
            // EditorConfigs();
            // CopyOverConfigs();
            // FixSelf();
        }

        public static void FormSnippets()
        {
            ApplicationConfiguration.Initialize();

            // MainForm form = new MainForm();
            // ListBoxSelectionDemo form = new ListBoxSelectionDemo();
            MenuAndContextMenuDemo form = new MenuAndContextMenuDemo();

            Application.Run(form);
            form.Dispose();
        }

        public static void CSharpSnippets()
        {
            // CheckMultilineDelimiter.Run();
            // NullableTypes.Run();
            // TestFileIO.Run();
            FixCsSources.Run();
            // FixCsSources2.CleanupRoslynSources();
            // RoslynRegexReplacements.Run();
            // GenericCSharp.Run();
            // AnalyseCsSources2.Run();
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

