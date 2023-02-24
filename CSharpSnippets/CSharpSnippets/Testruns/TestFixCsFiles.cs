using CSharpSnippets.FixCs;

namespace CSharpSnippets.Testruns
{
    public class TestFixCsFiles
    {
        public static void RunTrials()
        {
            CopyOverEditorConfigFile();
        }

        public static void CopyOverEditorConfigFile()
        {
            CopyFilesIntoStaging.CopyOverEditorconfigFile();
        }
    }
}

