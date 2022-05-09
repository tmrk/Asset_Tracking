using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset_Tracking_20220504
{
    public class Asset
    {
        public Asset(string type, string brand, string model, string office, DateTime purchaseDate, double localPrice)
        {
            Type = type;
            Brand = brand;
            Model = model;
            Office = office;
            PurchaseDate = purchaseDate;
            LocalPrice = localPrice;
        }

        public string Type { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public string Office { get; set; }
        public DateTime PurchaseDate { get; set; }
        public double LocalPrice { get; set; }
        public string DefaultCurrency { get { return "USD"; } }
        public string Currency {
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
                    case "United Kingdom": case "UK":
                        return "GBP";
                    case "Austria": case "Belgium": case "Cyprus": case "Estonia": case "Finland": case "France": case "": case "Germany": case "Greece": case "Ireland": case "Italy": case "Latvia": case "Lithuania": case "Luxembourg": case "Malta": case "the Netherlands": case "Portugal": case "Slovakia": case "Slovenia": case "Spain": 
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
            DateTime ExpiryDate = PurchaseDate.AddYears(3);
            return PurchaseDate < DateTime.Now.AddYears(-3).AddMonths(numberOfMonths);
        }
    }

}
