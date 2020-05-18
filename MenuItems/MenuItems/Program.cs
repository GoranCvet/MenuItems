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
            var menuList = new List<MenuItem>();
            var path = Directory.GetCurrentDirectory();
            DirectoryInfo root = new DirectoryInfo(path);
            for (int i = 0; i < 3; i++)
            {
                root = Directory.GetParent(path);
                path = root.ToString();
            }
            var file = Directory.GetFiles(path, "*.csv");

            ParseCsvInList(file, menuList);


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

            RecursivePrint(list);
            Console.ReadLine();

            //foreach (var item in list.OrderBy(i => i.MenuName))
            //{
            //    Console.WriteLine($".{item.MenuName}");
            //    foreach (var child in item.MenuItems.Where(i => i.IsHidden == false).OrderBy(i => i.MenuName))
            //    {
            //        Console.WriteLine($"....{child.MenuName}");
            //        if (child.MenuItems.Any())
            //        {
            //            foreach (var subchild in child.MenuItems)
            //            {
            //                Console.WriteLine($".......{subchild.MenuName}");
            //            }
            //        }
            //    }
            //}

        }
        public static void RecursivePrint(List<MenuItem> list)
        {
            foreach (var item in list.Where(i => i.IsHidden == false).OrderBy(i => i.MenuName))
            {
                Console.WriteLine(item.MenuName);
                if (item.MenuItems.Any())
                {
                    foreach (var x in item.MenuItems.Where(i => i.IsHidden == false).OrderBy(i => i.MenuName))
                    {
                        Console.WriteLine(x.MenuName);
                        RecursivePrint(x.MenuItems);
                    }
                }
            }
        }
        public static List<MenuItem> ParseCsvInList(string[] file, List<MenuItem> list)
        {


            using (TextFieldParser parser = new TextFieldParser(file[0]))
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
    }
}
