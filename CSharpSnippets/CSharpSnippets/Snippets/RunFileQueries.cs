using CSharpSnippets.FileIO;

namespace CSharpSnippets.Snippets
{
    public static class RunFileQueries
    {
        public static void Run()
        {
            ShowRoslynSources();
        }

        public static void ShowRoslynSources()
        {
            string roslynDir = @"Z:\github\roslyn";
            List<string> csFiles = FileQueries.GetCsFilesWalkDirectory(roslynDir, "cs");
            FileQueries.ShowFileSizesForFiles(csFiles);
        }
    }
}

