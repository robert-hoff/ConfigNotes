using System.Diagnostics;

namespace CSharpSnippets.FileIO
{
    public class FileQueries
    {
        /*
         * file-count   size(bytes)    filenamepath
         * ----------   -----------    ------------
         *
         */
        public static void ShowFileSizesForFiles(List<string> fileNamePaths)
        {
            List<string> filesAndSizes = new List<string>();
            foreach (string file in fileNamePaths)
            {
                FileInfo info = new FileInfo(file);
                filesAndSizes.Add($"{info.Length,8:#######0}         {file}");
            }
            filesAndSizes.Sort();
            filesAndSizes.Reverse();
            for (int i = 0; i < filesAndSizes.Count; i++)
            {
                Debug.WriteLine($"{i,5} {filesAndSizes[i]}");
            }
        }

        public static List<string> GetCsFilesWalkDirectory(string path, string fileextension)
        {
            return Directory.GetFileSystemEntries(path, $"*.{fileextension}", SearchOption.AllDirectories).ToList();
        }
    }
}

