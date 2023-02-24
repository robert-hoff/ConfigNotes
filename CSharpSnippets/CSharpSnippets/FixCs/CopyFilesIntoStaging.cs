using System.Diagnostics;

namespace CSharpSnippets.FixCs
{
    public class CopyFilesIntoStaging
    {
        private const string SOURCE_DIR = "Z:/github/ConfigNotes/CSharpSnippets";
        private const string TARGET_DIR = "Z:/github/ConfigNotes/CSharpConfig";

        /*
         * Copy over any new changes made to .editorconfig.
         *
         */
        public static void CopyOverEditorconfigFile()
        {
            string sourceFile = $"{SOURCE_DIR}/.editorconfig";
            string targetFile = $"{TARGET_DIR}/.editorconfig";
            DateTime sourceLastWrite = File.GetLastWriteTime(sourceFile);
            DateTime targetLastWrite = File.Exists(targetFile) ? File.GetLastWriteTime(targetFile) : new DateTime();
            if (targetLastWrite > sourceLastWrite)
            {
                Debug.WriteLine($"target file {targetLastWrite} is newer than source file {sourceLastWrite}, " +
                    $"check that this is what you want to do");
            }
            else
            {
                Debug.WriteLine($"copying over {targetFile} to {sourceFile}");
                File.Copy(sourceFile, targetFile, overwrite: true);
            }
        }
    }
}

