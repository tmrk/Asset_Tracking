using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text.Json;

namespace Asset_Tracking_20220504
{
    internal class Program
    {
        static void Main(string[] args)
        {
            LoadData(dataPath);
            CurrentContentShown();

            ShowMenu(mainMenu);
            SelectMenu(mainMenu);
        }

        static void CurrentContentShown()
        {
            Panel("heading", "Asset Tracking", width: 50, color: "yellow", tMargin: 2, bMargin: 2);
            ListAssets();
        }

        // === SETTING UP SOME VARIABLES ===

        static string dataPath = Path.Combine(Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName, @"data.json");

        static CultureInfo ci = new CultureInfo("en-UK");

        static List<Asset> assets = new List<Asset>();

        static List<Column> columns = new List<Column>
        {
            new Column("Type", 10),
            new Column("Brand", 10),
            new Column("Model", 10),
            new Column("Office", 10),
            new Column("Purchase Date", 14),
            new Column("Price", 6),
            new Column("Currency"),
            new Column("Local Price")
        };

        static List<menuFunction> mainMenu = new List<menuFunction>
        {
            new menuFunction("Sort by type of asset", ()=> ListAssets()),
            new menuFunction("Sort by purchase date", ()=> ListAssets()),
            new menuFunction("Mark end of life assets", ()=> ListAssets()),
            new menuFunction("Quit", ()=> Environment.Exit(0))
        };


        // === PROGRAM METHODS ===

        // Returns a string from an Asset that spaces the asset's properties based on the passed List that defines column names and column widths
        static string PrintAssetByColumns(Asset asset, List<Column> cols, int colspan = 1)
        {
            string row = "";
            for (int i = 0; i < cols.Count; i++)
            {
                Column column = cols[i];
                if (i == cols.Count - 1) colspan = 0; // the last column will not have colspan
                if (asset.GetType().GetProperty(column.PropertyName) != null)
                {
                    var propertyValue = asset.GetType().GetProperty(column.PropertyName).GetValue(asset);
                    string propertyString = "";

                    if (propertyValue is DateTime) propertyString = Convert.ToDateTime(propertyValue).ToString("yyyy-MM-dd");
                    else if (propertyValue is long) propertyString = TextAlign(FormatN(Convert.ToInt64(propertyValue)), column.Width, "right");
                    else propertyString = propertyValue.ToString();

                    // If the content is longer than the column width, cut it down and add "~" / "…"
                    if (propertyString.Length > column.Width) propertyString = propertyString.Substring(0, column.Width - 1) + "~";

                    row += propertyString + new string(' ', Math.Max(column.Width - propertyString.Length + colspan, 0));
                }
                else row += new string(' ', column.Width + colspan);
            }
            return row;
        }

        static void ListAssets()
        {
            Panel("table", cols: columns, rows: assets);
        }

        static void ListAssetsHighlighted()
        {
            Panel("table", cols: columns, rows: assets);
        }

        static void LoadData(string path)
        {
            if (File.Exists(path))
            {
                string json = File.ReadAllText(path);
                assets = JsonSerializer.Deserialize<List<Asset>>(json);
            }
            else
            {
                AddTestData();
                SaveData(path);
                LoadData(path);
            }
        }

        static void SaveData(string path)
        {
            File.WriteAllText(path, JsonSerializer.Serialize(assets));
        }



        // === HELPER METHODS ===

        // Populate the "asset" List with some dummy data
        static void AddTestData()
        {
            assets.Add(new Asset("Phone", "Apple", "iPhone XS", "Sweden", DateTime.Now, 999));
            assets.Add(new Asset("Phone", "Apple", "iPhone X", "Spain", DateTime.Now.AddYears(-1), 888));
        }

        // Generates a panel window and draws it onto the consol to show output in a nice way
        static void Panel(string partToPrint, string content = "",
            int width = 0, int hMargin = 0, int tMargin = 0, int bMargin = 0, int hPadding = 2, int vPadding = 0, int border = 1, int colspan = 1,
            string textAlign = "", string color = "", string fontColor = "",
            bool highlight = false, string highlightColor = "", string highlightTextColor = "",
            string subheading = "", List<Column> cols = null, List<Asset> rows = null)
        {
            if (width == 0) width = Console.WindowWidth - (hMargin * 2) - (hPadding * 2) - 2; // sets the panel to full window width if no width is defined
            if (hMargin == 0) hMargin = ((Console.WindowWidth - hMargin - width - hPadding) / 2) - (border * 2); // centers the panel if no hMargin is defined

            content = TextAlign(content, width, textAlign);
            ConsoleColor panelColor, textColor, panelColorHighlight, textColorHighlight;
            if (Enum.TryParse(FirstLetterToUpper(color), out panelColor)) { }
            else panelColor = ConsoleColor.Gray;
            if (Enum.TryParse(FirstLetterToUpper(fontColor), out textColor)) { }
            else textColor = ConsoleColor.Black;
            if (highlightColor != "" || highlightTextColor != "") highlight = true;
            if (highlight)
            {
                if (Enum.TryParse(FirstLetterToUpper(highlightColor), out panelColorHighlight)) { }
                else panelColorHighlight = ConsoleColor.DarkGreen;
                if (Enum.TryParse(FirstLetterToUpper(highlightTextColor), out textColorHighlight)) { }
                else textColorHighlight = ConsoleColor.White;
            }
            else
            {
                panelColorHighlight = panelColor;
                textColorHighlight = textColor;
            }
            if (tMargin != 0) for (int i = 0; i < tMargin; i++) Console.WriteLine("");
            switch (partToPrint.ToLower())
            {
                case "top":
                    Console.ForegroundColor = panelColor;
                    Console.Write(new string(' ', hMargin));
                    if (border == 1) Console.WriteLine("┌" + new string('─', width + (hPadding * 2)) + "┐");
                    for (int i = 0; i < vPadding; i++) Panel("br", width: width, hMargin: hMargin, color: color, fontColor: fontColor);
                    break;
                case "bottom":
                    for (int i = 0; i < vPadding; i++) Panel("br", width: width, hMargin: hMargin, color: color, fontColor: fontColor);
                    Console.Write(new string(' ', hMargin));
                    Console.ForegroundColor = panelColor;
                    if (border == 1) Console.WriteLine("└" + new string('─', width + (hPadding * 2)) + "┘");
                    Console.ResetColor();
                    break;
                case "left":
                    Console.ResetColor();
                    Console.Write(new string(' ', hMargin));
                    if (border == 1)
                    {
                        Console.ForegroundColor = panelColor;
                        Console.Write("│");
                        Console.ResetColor();
                    }
                    if (highlight)
                    {
                        Console.BackgroundColor = panelColorHighlight;
                        Console.ForegroundColor = textColorHighlight;
                    }
                    else
                    {
                        Console.BackgroundColor = panelColor;
                        Console.ForegroundColor = textColor;
                    }
                    Console.Write(new string(' ', hPadding));
                    break;
                case "right":
                    Console.Write(new string(' ', Math.Max(width + hMargin + hPadding + 1 - Console.CursorLeft, 0)));
                    Console.Write(new string(' ', hPadding));
                    Console.ResetColor();
                    if (border == 1)
                    {
                        Console.ForegroundColor = panelColor;
                        Console.WriteLine("│");
                    }
                    else Console.WriteLine("");
                    break;
                case "row":
                    for (int i = 0; i < vPadding; i++) Panel("br", width: width, color: color, fontColor: fontColor);
                    Panel("left", width: width, border: border, color: color, fontColor: fontColor, highlight: highlight, highlightColor: highlightColor, highlightTextColor: highlightTextColor, hMargin: hMargin);
                    Console.Write(content);
                    Panel("right", width: width, border: border, color: color, fontColor: fontColor, hMargin: hMargin);
                    for (int i = 0; i < vPadding; i++) Panel("br", width: width, color: color, fontColor: fontColor);
                    break;
                case "hr":
                    Panel("left", width: width, hMargin: hMargin, color: color, fontColor: fontColor);
                    Console.Write(new string('─', width));
                    Panel("right", width: width, hMargin: hMargin, color: color, fontColor: fontColor);
                    break;
                case "br":
                    Panel("left", width: width, hMargin: hMargin, color: color, fontColor: fontColor);
                    Console.Write(" ");
                    Panel("right", width: width, hMargin: hMargin, color: color, fontColor: fontColor);
                    break;
                case "heading":
                    Panel("top", width: width, vPadding: 1, hMargin: hMargin, color: color, fontColor: fontColor);
                    Panel("row", content, textAlign: "center", width: width, hMargin: hMargin, color: color, fontColor: fontColor);
                    if (subheading != "")
                    {
                        Panel("br", color: color);
                        Panel("row", subheading, textAlign: "center", width: width, hMargin: hMargin, color: color, fontColor: fontColor);
                    }
                    Panel("bottom", width: width, vPadding: 1, hMargin: hMargin, color: color, fontColor: fontColor);
                    break;
                case "table":
                    string topRow = "";
                    int listWidth = 0;
                    for (int i = 0; i < cols.Count; i++)
                    {
                        Column col = cols[i];
                        int topRowColspan = colspan;
                        if (i == cols.Count - 1) topRowColspan = 0;
                        topRow += col.Name + new string(' ', col.Width - col.Name.Length + topRowColspan);
                        listWidth += col.Width + topRowColspan;
                    }
                    Panel("top", width: listWidth, vPadding: 1, color: color, fontColor: fontColor);
                    Panel("row", topRow, width: listWidth, color: color, fontColor: fontColor);
                    Panel("hr", width: listWidth, color: color, fontColor: fontColor);
                    foreach (var item in rows)
                    {
                        
                        if (item.EndOfLife(3))
                        {
                            highlightColor = "Red";
                            highlightTextColor = "";

                        }
                        else if (item.EndOfLife(6))
                        {
                            highlightColor = "Yellow";
                            highlightTextColor = "Black";
                        }
                        Panel("row", PrintAssetByColumns(item, cols, colspan: colspan), width: listWidth, color: color, fontColor: fontColor, highlightColor: highlightColor, highlightTextColor: highlightTextColor);
                    }
                    Panel("bottom", width: listWidth, vPadding: 1, color: color, fontColor: fontColor);
                    break;
            }
            if (bMargin != 0) for (int i = 0; i < bMargin; i++) Console.WriteLine("");
        }

        // Returns a sring that aligns the passed text inside the width of a containing box
        static string TextAlign(string text = "", int boxLength = 1, string textAlign = "")
        {
            int leftPadding = 0;
            switch (textAlign.ToLower().Trim())
            {
                case "right" or "r":
                    leftPadding = boxLength - text.Length;
                    break;
                case "center" or "c":
                    leftPadding = (boxLength - text.Length) / 2;
                    break;
            }
            return new string(' ', Math.Max(leftPadding, 0)) + text;
        }

        // adds a thousands separator to numbers
        static string FormatN(long number)
        {
            string result = "";
            string chars = number.ToString();
            int numberOfCommas = 0;
            for (int i = chars.Length - 1; i >= 0; i--)
            {
                result = result.Insert(0, chars[i].ToString());
                if ((result.Length - numberOfCommas) % 3 == 0 && i != 0)
                {
                    result = result.Insert(0, ",");
                    numberOfCommas++;
                }
            }
            return result;
        }

        // Returns the same string with its first letter uppercased
        static string? FirstLetterToUpper(string str)
        {
            if (str == null) return null;
            if (str.Length > 1) return char.ToUpper(str[0]) + str.Substring(1);
            return str.ToUpper();
        }

        // Displays a menu UI for the basic functions
        static void ShowMenu(List<menuFunction> menu, int selected = 1)
        {
            Panel("top", width: 50, vPadding: 1);
            if (selected < 1) selected = menu.Count;
            else if (selected > menu.Count) selected = 1;
            for (int i = 0; i < menu.Count; i++)
            {
                Panel("row", "[" + (i + 1) + "] " + menu[i].Description, highlight: i == selected - 1, width: 50);
            }
            Panel("bottom", width: 50, vPadding: 1);
            Console.CursorVisible = false;
            SelectMenu(menu, selected);
        }


        static void SelectMenu(List<menuFunction> menu, int selected = 1)
        {
            ConsoleKeyInfo keyPressed = Console.ReadKey(true);
            Console.Clear();
            CurrentContentShown();
            switch (keyPressed.Key)
            {
                case ConsoleKey.Enter:
                    menu[selected - 1].Action.Invoke();
                    break;
                case ConsoleKey.UpArrow or ConsoleKey.LeftArrow or ConsoleKey.Backspace:
                    ShowMenu(menu, selected - 1);
                    break;
                case ConsoleKey.DownArrow or ConsoleKey.RightArrow or ConsoleKey.Tab:
                    ShowMenu(menu, selected + 1);
                    break;
                default: // If the keyPressed is not arrows/Enter, then check which number it is
                    Int32 keyNumber;
                    if (Int32.TryParse(keyPressed.KeyChar.ToString(), out keyNumber) && keyNumber <= menu.Count)
                    {
                        menu[keyNumber - 1].Action.Invoke();
                    }
                    else ShowMenu(menu, selected);
                    break;
            }
        }
    }

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

        public bool EndOfLife(int numberOfMonths = 3)
        {
            DateTime ExpiryDate = PurchaseDate.AddYears(3);

            return PurchaseDate < DateTime.Now.AddYears(-3).AddMonths(numberOfMonths);
        }

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

    internal class menuFunction
    {
        public menuFunction(string description, Action action)
        {
            Description = description;
            Action = action;
        }
        public string Description { get; set; }
        public Action Action { get; set; }
    }
}
