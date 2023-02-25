namespace CSharpSnippets.FileIO
{
    public static class SaveDataToFile
    {
        private const string DEFAULT_OUTPUT_DIR = "../../../../data-output";

        public static void SaveSingleColumnIntData(string filename, ICollection<int> data, bool saveAsHex = false)
        {
            if (data == null)
            {
                throw new Exception("ICollection<int> data was null");
            }
            string filenamepath = $"{DEFAULT_OUTPUT_DIR}/{filename}";
            FileWriter fileWriter = new FileWriter(filenamepath);
            foreach (int d in data)
            {
                if (saveAsHex)
                {
                    fileWriter.WriteLine($"0x{d:X}");
                }
                else
                {
                    fileWriter.WriteLine($"{d}");
                }
            }
            FileWriter.CloseFileWriter(fileWriter);
        }

        public static void SaveSingleColumnStringData(string filename, ICollection<string> data)
        {
            if (data == null)
            {
                throw new Exception("ICollection<string> data was null");
            }
            string filenamepath = $"{DEFAULT_OUTPUT_DIR}/{filename}";
            FileWriter fileWriter = new FileWriter(filenamepath);
            foreach (string d in data)
            {
                fileWriter.WriteLine($"{d}");
            }
            FileWriter.CloseFileWriter(fileWriter);
        }
    }
}

