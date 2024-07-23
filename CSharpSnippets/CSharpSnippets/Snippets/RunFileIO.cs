using System.Diagnostics;
using CSharpSnippets.FileIO;

namespace CSharpSnippets.Snippets
{
    public static class RunFileIO
    {
        private const string DEFAULT_OUTPUT_DIR = "../../../../data-output";

        public static void Run()
        {
            TestSaveIntDataToFile();
            // TestFileWrite2();
            // TestFileWrite1();
            // TestFileReadStringData();
            // TestFileReadIntData();
            // TestParsing();
        }

        public static void TestSaveIntDataToFile()
        {
            var filename = "myintdata.txt";
            List<int> myIntData = new()
            {
                1, 2, 3
            };
            SaveDataToFile.SaveSingleColumnIntData(filename, myIntData);
        }

        public static void TestFileWrite2()
        {
            var outputFilenamepath = $"{DEFAULT_OUTPUT_DIR}/hellofile.html";
            FileWriter fw = new(
                outputFilenamepath,
                showOutputToConsole: false,
                EOL: FileWriter.LINUX_ENDINGS,
                useBom: FileWriter.SAVE_UTF_FILE_WITHOUT_BOM
                );
            fw.WriteHtmlHeader("title", "header");
            fw.WriteLine("I'm an html page");
            fw.Write("1 ");
            fw.Write("2 ");
            fw.Write("3\r\n");
            FileWriter.CloseFileWriter(fw);
        }

        public static void TestFileWrite1()
        {
            var outputFilenamepath = $"{DEFAULT_OUTPUT_DIR}/hellofile.txt";
            FileWriter fw = new FileWriter(
                outputFilenamepath,
                showOutputToConsole: false,
                EOL: FileWriter.LINUX_ENDINGS,
                useBom: FileWriter.SAVE_UTF_FILE_WITHOUT_BOM
                );
            fw.WriteLine("hello");
            fw.Write("1 ");
            fw.Write("2 ");
            fw.Write("3\r\n");
            FileWriter.CloseFileWriter(fw);
        }

        public static void TestFileReadStringData()
        {
            List<string> stringData = ReadDataFromFile.ReadSingleColumnStringData("stringdata.txt");
            foreach (var stringVal in stringData)
            {
                Debug.WriteLine($"{stringVal}");
            }
        }

        public static void TestFileReadIntData()
        {
            List<int> intData = ReadDataFromFile.ReadSingleColumnIntData("intdata.txt");
            foreach (var intVal in intData)
            {
                Debug.WriteLine($"{intVal}");
            }
        }

        // the default parser is tolerant to surrounding whitespace
        public static void TestParsing()
        {
            var myInt = int.Parse("  9   ");
            Debug.WriteLine($"{myInt}");
        }
    }
}

