using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpSnippets.Snippets
{
    public class GenericCSharp
    {
        public static void Run()
        {
            SortingListDescendingOrder();
        }

        // see https://stackoverflow.com/a/75599091/211764
        public static void SortingListDescendingOrder()
        {
            List<Product> listToSort = new List<Product>
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
    }
}

