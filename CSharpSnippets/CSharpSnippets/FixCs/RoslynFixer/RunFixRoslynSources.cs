using System.Diagnostics;
using CSharpSnippets.FileIO;

namespace CSharpSnippets.FixCs.RoslynFixer
{
    public static class RunFixRoslynSources
    {
        public static void Run()
        {
            RunFixesSingleFile();
            // RunFixesBatch();
            // FullReport();
        }

        private const string SOURCE_DIR = @"Z:/github/roslyn/src/";
        public static void RunFixesSingleFile()
        {
            // NOTE - actually I don't really need to supply the file name, I should get
            // this using the index
            List<(int, string)> selectedCases = new(){
                (5, @"Z:/github/roslyn/src/Compilers\Shared\BuildServerConnection.cs"),
                (15, @"Z:/github/roslyn/src/Dependencies\CodeAnalysis.Debugging\CustomDebugInfoReader.cs"),
                (50, @"Z:/github/roslyn/src/Dependencies\Collections\SegmentedDictionary`2.cs"),
                (219, @"Z:/github/roslyn/src/Tools\PrepareTests\MinimizeUtil.cs"),
                (106, @"Z:/github/roslyn/src/Interactive\HostTest\InteractiveHostDesktopTests.cs"),
                (271, @"Z:/github/roslyn/src/Workspaces\MSBuildTest\VisualStudioMSBuildWorkspaceTests.cs"),
                (273, @"Z:/github/roslyn/src/Workspaces\MSBuildTest\VisualStudioMSBuildWorkspaceTests.cs"),
                (351, @"Z:/github/roslyn/src/Compilers\Core\MSBuildTaskTests\CscTests.cs"),
                (353, @"Z:/github/roslyn/src/Compilers\Core\MSBuildTaskTests\DotNetSdkTests.cs"),
                (1162, @"Z:/github/roslyn/src/EditorFeatures\CSharpTest\QuickInfo\SemanticQuickInfoSourceTests.cs"),
                (7898, @"Z:/github/roslyn/src/Compilers\CSharp\Test\Emit\CodeGen\CodeGenDynamicTests.cs"),
                (8366, @"Z:/github/roslyn/src/Compilers\CSharp\Test\Emit\CodeGen\CodeGenDynamicTests.cs")
            };
            var files = GetFileList(SOURCE_DIR);
            new FixRoslynSources(files[selectedCases[0].Item1], fileNr: selectedCases[0].Item1,
                showSourceAnalysis: false, writeFile: false, showFixedFile: false, showReport: true);
        }

        public static void RunFixesBatch()
        {
            var files = GetFileList(SOURCE_DIR);
            for (var i = 0; i < files.Length; i++)
            {
                new FixRoslynSources(files[i], i, writeFile: true);
                if (i == 500)
                {
                    break;
                }
            }
        }

        public static void FullReport()
        {
            List<string> fullReport = new();
            var files = GetFileList(SOURCE_DIR);
            for (var i = 0; i < files.Length; i++)
            {
                FixRoslynSources fixCs = new(files[i], i, writeFile: false);
                fullReport.AddRange(fixCs.GetReport());
                if (i == 20000)
                {
                    break;
                }
            }
            SaveFullReport(fullReport);
        }

        public static void SaveFullReport(List<string> fullReport)
        {
            Debug.WriteLine($"SAVING REPORT");
            FileWriter fw = new(@"Z:\github\ConfigNotes\CSharpSnippets\data-output\report.txt");
            foreach (var line in fullReport)
            {
                fw.WriteLine(line);
            }
            FileWriter.CloseFileWriter(fw);
            Debug.WriteLine($"DONE");
        }

        public static string[] GetFileList(string path)
        {
            return Directory.GetFileSystemEntries(path, "*.cs", SearchOption.AllDirectories);
        }
    }
}

