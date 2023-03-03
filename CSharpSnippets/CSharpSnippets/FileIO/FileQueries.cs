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

        public static void ShowCsFilesWalkDirectory(string path, string fileExtension)
        {
            foreach (string file in GetCsFilesWalkDirectory(path, fileExtension))
            {
                Debug.WriteLine($"{file}");
            }
        }

        public static List<string> GetCsFilesWalkDirectory(string path, string fileExtension)
        {
            return Directory.GetFileSystemEntries(path, $"*.{fileExtension}", SearchOption.AllDirectories).ToList();
        }

        public static string[] ReadFileAsStringArray(string filenamepath)
        {
            return File.ReadAllLines($"{filenamepath}");
        }

        public static List<string> ReadFileAsStringList(string filenamepath)
        {
            return File.ReadAllLines($"{filenamepath}").ToList();
        }
    }
}

