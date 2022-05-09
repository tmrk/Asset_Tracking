using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset_Tracking_20220504
{
    internal class Column
    {
        public Column(string name = "", int width = 1, string propertyName = "")
        {
            Name = name;
            Width = Math.Max(width, name.Length);
            PropertyName = propertyName.Length != 0 ? propertyName : new CultureInfo("en-UK").TextInfo.ToTitleCase(name).Replace(" ", "");
        }
        public string Name { get; set; }
        public int Width { get; set; }
        public string PropertyName { get; set; }
    }

}
