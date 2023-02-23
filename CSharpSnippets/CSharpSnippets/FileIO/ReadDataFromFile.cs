﻿using CSharpSnippets.FileO;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpSnippets.FileIO
{
    class ReadDataFromFile
    {
        // static readonly string INPUT_FOLDER = "../../../../data-input/";
        public const string INPUT_FOLDER = "../../../../data-input-samples/";

        public static List<int> ReadSingleColumnIntData(string filename, string folder = INPUT_FOLDER)
        {
            List<int> data = new();
            string[] lines = File.ReadAllLines($"{folder}/{filename}");
            foreach (string line in lines)
            {
                if (line.Trim().Length > 0)
                {
                    data.Add(int.Parse(line));
                }
            }
            return data;
        }
        public static List<string> ReadSingleColumnStringData(string filename, string folder = INPUT_FOLDER)
        {
            List<string> data = new();
            string[] lines = File.ReadAllLines($"{folder}/{filename}");
            foreach (string line in lines)
            {
                if (line.Trim().Length > 0)
                {
                    data.Add(line.Trim());
                }
            }
            return data;
        }
    }
}
