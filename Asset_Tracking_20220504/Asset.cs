using System;
using System.Globalization;

namespace Asset_Tracking_20220504
{
    public class Asset
    {
        public Asset(string type, string brand, string model, string office, DateTime purchaseDate, long price, string currency = "USD")
        {
            Type = type;
            Brand = brand;
            Model = model;
            Office = office;
            PurchaseDate = purchaseDate;
            Price = price;
            Currency = currency;
        }

        public string Type { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public string Office { get; set; }
        public DateTime PurchaseDate { get; set; }
        public long Price { get; set; }
        public string Currency { get; set; }

        static long GetLocalPrice()
        {
            return 0;
        }

    }

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
