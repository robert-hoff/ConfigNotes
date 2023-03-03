using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSharpSnippets.FileIO;

namespace CSharpSnippets.Snippets
{
    public class RunFileQueries
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

