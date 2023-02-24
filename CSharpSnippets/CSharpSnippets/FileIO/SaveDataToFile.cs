namespace CSharpSnippets.FileIO
{
    public class SaveDataToFile
    {
        private const string DEFAULT_OUTPUT_DIR = "../../../../data-output";

        public static void SaveSingleColumnIntData(string filename, ICollection<int> data, bool saveAsHex = false)
        {
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
            fileWriter.CloseStreamWriter();
        }
    }
}

