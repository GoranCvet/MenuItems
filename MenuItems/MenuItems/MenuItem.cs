using System;
using System.Collections.Generic;
using System.Text;

namespace MenuItems
{
    public class MenuItem
    {
        public MenuItem()
        {
            MenuItems = new List<MenuItem>();
        }
        public int ID { get; set; }
        public string MenuName { get; set; }
        public int? ParentId { get; set; }
        public bool IsHidden { get; set; }
        public string LinkUrl { get; set; }
        public List<MenuItem> MenuItems { get; set; }
    }
}
