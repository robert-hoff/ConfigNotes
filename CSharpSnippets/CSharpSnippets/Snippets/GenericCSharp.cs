using System.Diagnostics;

namespace CSharpSnippets.Snippets
{
    public static class GenericCSharp
    {
        public static void Run()
        {
            // SortingListDescendingOrder();
            List<Product> myList = new() {
                new Product("A"),
                new Product("Z"),
                new Product("C")
        };
            myList = myList.OrderByDescending(x => x.Name).ToList();
            Debug.WriteLine(
                $"{myList[0].Name}" +  // 'Z'
                $"{myList[1].Name}" +  // 'C'
                $"{myList[2].Name}"    // 'A'
            );
        }

        // see https://stackoverflow.com/a/75599091/211764
        public static void SortingListDescendingOrder()
        {
            List<Product> listToSort = new()
            {
                new Product("A"),
                new Product("Z"),
                new Product("C")
            };
            // you can assign the list to itself, use a new var if you need
            //    to keep the original list
            listToSort = listToSort.OrderByDescending(x => x.Name).ToList();
            Debug.WriteLine($"{listToSort[0].Name}");
        }

        class Product
        {
            public string Name { get; private set; }
            public Product(string Name)
            {
                this.Name = Name;
            }
        }

        /*
         * Just checks if I'm allowed to use the null type
         *
         */
        public static void InstantiateNullType()
        {
            string hello = null;
            Debug.WriteLine($"{hello}");
        }

        // add one more line than counted, because the last \n falls out
        // R: strangely, the file read with File.ReadAllLines is the same whether or not I add a blank line at the end
        /*
         * I discovered that the method for reading a file as List<string> sometimes returns the
         * same number of lines for different lengh files
         *
         */
        public static int CountBlankLinesAtEof(List<string> lines)
        {
            if (!string.IsNullOrEmpty(lines[lines.Count - 1].Trim()))
            {
                return 1;
            }
            var blankLinesAtEof = 1;
            while (string.IsNullOrWhiteSpace(lines[lines.Count - blankLinesAtEof].Trim()) &&
                   blankLinesAtEof < lines.Count)
            {
                blankLinesAtEof++;
            }
            return blankLinesAtEof;
        }
    }
}

