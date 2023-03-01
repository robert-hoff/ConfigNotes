using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSharpSnippets.PrintMethods;

namespace CSharpSnippets.FixCs
{
    public class AnalyseCsSources2
    {

        private const string SOURCE_DIR = @"Z:/github/roslyn/src/";
        // private const string TEST_FILE = @"Z:/github/roslyn/src/Analyzers/Core/Analyzers/AbstractBuiltInCodeStyleDiagnosticAnalyzer.cs";
        // private const string TEST_FILE = @"Z:\github\roslyn\src\Analyzers\CSharp\Tests\MakeMethodAsynchronous\MakeMethodAsynchronousTests.cs";
        private const string TEST_FILE = @"Z:\github\ConfigNotes\CSharpSnippets\CSharpSnippets\Snippets\FixCsSourcesTestCase1.cs";



        public static void Run()
        {
            // Analysis2();
            Analysis1();
        }

        public static void Analysis2()
        {
            int fileNr = 219; // Z:/github/roslyn/src/Tools\PrepareTests\MinimizeUtil.cs
            // int fileNr = 5; // Z:/github/roslyn/src/Compilers\Shared\BuildServerConnection.cs
            // int fileNr = 106; // Z:/github/roslyn/src/Interactive\HostTest\InteractiveHostDesktopTests.cs
            // int fileNr = 15; // Z:/github/roslyn/src/Dependencies\CodeAnalysis.Debugging\CustomDebugInfoReader.cs
            // int fileNr = 50; // Z:/github/roslyn/src/Dependencies\Collections\SegmentedDictionary`2.cs
            // int fileNr = 271; // Z:/github/roslyn/src/Workspaces\MSBuildTest\VisualStudioMSBuildWorkspaceTests.cs
            string[] files = GetFileList(SOURCE_DIR);
            new FixCsSources3(files[fileNr], fileNr);
            // new FixCsSources3("Z:/github/roslyn/src/Dependencies\\Collections\\SegmentedDictionary`2.cs", 50);
        }


        public static void Analysis1()
        {
            string[] files = GetFileList(SOURCE_DIR);
            for (int i = 350; i < files.Length; i++)
            {
                new FixCsSources3(files[i], i);
                if (i == 500)
                {
                    break;
                }
            }
        }

        public static string[] GetFileList(string path)
        {
            return Directory.GetFileSystemEntries(path, "*.cs", SearchOption.AllDirectories);
        }
    }
}

