using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MenuItems
{
    class Program
    {
        static void Main(string[] args)
        {
            var path = Directory.GetCurrentDirectory();
            DirectoryInfo root = new DirectoryInfo(path);
            for (int i = 0; i < 3; i++)
            {
                root = Directory.GetParent(path);
                path = root.ToString();
            }
            var file = Directory.GetFiles(path, "*.csv");

            var menuList = ParseCsvInList(file);

            var list = SortParsedCsvList(menuList);

            Print(list);

        }
        public static List<MenuItem> ParseCsvInList(string[] files)
        {

            var list = new List<MenuItem>();

            using (TextFieldParser parser = new TextFieldParser(files[0]))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(";");
                string headLines = parser.ReadLine();

                while (!parser.EndOfData)
                {
                    string[] fields = parser.ReadFields();

                    var menuItem = new MenuItem();

                    menuItem.ID = int.Parse(fields[0]);
                    menuItem.MenuName = fields[1];
                    if (fields[2] == "NULL")
                    {
                        menuItem.ParentId = null;
                    }
                    else
                    {
                        menuItem.ParentId = int.Parse(fields[2]);
                    }
                    menuItem.IsHidden = bool.Parse(fields[3]);
                    menuItem.LinkUrl = fields[4];

                    list.Add(menuItem);

                }
            }

            return list;

        }
        public static List<MenuItem> SortParsedCsvList(List<MenuItem> menuList)
        {
            var list = new List<MenuItem>();
            foreach (var item in menuList)
            {
                MenuItem parent;
                if (item.ParentId == null)
                {
                    list.Add(item);
                }
                else
                {
                    parent = menuList.SingleOrDefault(p => p.ID == item.ParentId);
                    parent.MenuItems.Add(item);
                }
            }
            return list;
        }
        public static void Print(List<MenuItem> list)
        {
            foreach (var item in list.Where(i => i.IsHidden == false).OrderBy(i => i.MenuName))
            {
                Console.WriteLine(item.MenuName);
                Print(item.MenuItems);
            }
        }
      
            
    }
}
