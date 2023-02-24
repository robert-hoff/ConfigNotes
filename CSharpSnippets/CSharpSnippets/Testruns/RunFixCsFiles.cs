using CSharpSnippets.FixCs;

namespace CSharpSnippets.Testruns
{
    public class RunFixCsFiless
    {
        public static void RunTrials()
        {
            FixStylesForAllCsFiles();
            // CopyOverEditorConfigFile();
        }

        public static void FixStylesForAllCsFiles()
        {
            FixCsSources.Run();
        }

        public static void CopyOverEditorConfigFile()
        {
            CopyFilesIntoStaging.CopyOverEditorconfigFile();
        }
    }
}

