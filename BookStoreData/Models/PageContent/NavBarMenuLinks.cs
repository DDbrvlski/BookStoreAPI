using BookStoreData.Models.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStoreData.Models.PageContent
{
    public class NavBarMenuLinks : DictionaryTable
    {
        public string Path { get; set; }
        public int Position { get; set; }
    }
}
