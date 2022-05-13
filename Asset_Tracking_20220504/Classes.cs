using System;
using System.Globalization;

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

    internal class Asset
    {
        public Asset(string type, string brand, string model, string office, DateTime purchaseDate, double localPrice)
        {
            Type = type;
            Brand = brand;
            Model = model;
            Office = office;
            PurchaseDate = purchaseDate;
            LocalPrice = localPrice;
            DefaultCurrency = "USD";
        }

        public string Type { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public string Office { get; set; }
        public DateTime PurchaseDate { get; set; }
        public double LocalPrice { get; set; }
        public string DefaultCurrency { get; set; }
        public string Currency
        {
            get
            {
                switch (Office)
                {
                    case "Sweden":
                        return "SEK";
                    case "Denmark":
                        return "DKK";
                    case "Norway":
                        return "NOK";
                    case "United Kingdom":
                    case "UK":
                        return "GBP";
                    case "Austria": case "Belgium": case "Cyprus": case "Estonia": case "Finland": case "France": case "Germany": case "Greece": case "Ireland": case "Italy": case "Latvia": case "Lithuania": case "Luxembourg": case "Malta": case "the Netherlands": case "Portugal": case "Slovakia": case "Slovenia": case "Spain":
                        return "EUR";
                    default:
                        return DefaultCurrency;
                }
            }
        }
        public double PriceUSD
        {
            get
            {
                double rate = 1;
                switch (Currency)
                {
                    case "EUR": rate = 1.05; break;
                    case "GBP": rate = 1.23; break;
                    case "DKK": rate = 0.14; break;
                    case "SEK": rate = 0.1; break;
                }
                return LocalPrice * rate;
            }
        }
        public bool EndOfLife(int numberOfMonths = 3)
        {
            return PurchaseDate < DateTime.Now.AddYears(-3).AddMonths(numberOfMonths);
        }
    }

    internal class MenuFunction
    {
        public MenuFunction(string description, Action action)
        {
            Description = description;
            Action = action;
        }
        public string Description { get; set; }
        public Action Action { get; set; }
    }
}
