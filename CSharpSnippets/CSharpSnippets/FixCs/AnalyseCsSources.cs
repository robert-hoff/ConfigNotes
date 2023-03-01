using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpSnippets.FixCs
{
    public class AnalyseCsSources
    {
        public static void Run()
        {
            Trial1();
        }

        public static void Trial1()
        {
            string[] files = GetCsFilesWalkDirectory(@"Z:/github/roslyn/src");

            List<string> filesAndSizes = new List<string>();
            foreach (string file in files)
            {
                FileInfo info = new FileInfo(file);
                // Debug.WriteLine($"{info.Length}");
                filesAndSizes.Add($"{info.Length,8:#######0}         {file}");
            }

            filesAndSizes.Sort();
            filesAndSizes.Reverse();
            for (int i = 0; i < filesAndSizes.Count; i++)
            {
                Debug.WriteLine($"{i,5} {filesAndSizes[i]}");
            }
        }


        public static string[] GetCsFilesWalkDirectory(string path)
        {
            return Directory.GetFileSystemEntries(path, "*.cs", SearchOption.AllDirectories);
        }
    }
}

