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
            Panel("heading", "Asset Tracking", width: 100);

            Panel("heading", "Asset Tracking 1");
            ListAssets();
        }

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
            new Column("Price", 10),
            new Column("Currency", 10),
            new Column("Local Price", 1)
        };

        static string PrintAssetByColumns(Asset asset, List<Column> cols)
        {
            string row = "";
            foreach (Column column in cols)
            {
                if (asset.GetType().GetProperty(column.PropertyName) != null)
                {
                    var propertyValue = asset.GetType().GetProperty(column.PropertyName).GetValue(asset);
                    string propAsstring = "";

                    if (propertyValue is DateTime) propAsstring = Convert.ToDateTime(propertyValue).ToString("yyyy-MM-dd");
                    else if (propertyValue is long) propAsstring = FormatN(Convert.ToInt64(propertyValue));
                    else propAsstring = propertyValue.ToString();

                    row += propAsstring + new string(' ', Math.Max(column.Width - propAsstring.ToString().Length, 1));
                }
                else row += new string(' ', column.Width);
            }
            return row;
        }

        static void ListAssets()
        {
            Panel("list", cols: columns, list: assets);
            Console.ReadLine();
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



        // HELPER METHODS

        // Populate the "asset" List with some dummy data
        static void AddTestData()
        {
            assets.Add(new Asset("Phone", "Apple", "iPhone XS", "Sweden", DateTime.Now, 999));
            assets.Add(new Asset("Phone", "Apple", "iPhone X", "Spain", DateTime.Now.AddYears(-1), 888));
        }

        static void Panel(string partToPrint, string content = "",
            int width = 0, int margin = 0, int hPadding = 2, int vPadding = 0, int border = 1,
            string textAlign = "", string panel = "", string text = "",
            bool highlight = false, string highlightColor = "", string highlightTextColor = "",
            string subheading = "", List<Column> cols = null, List<Asset> list = null)
        {
            // new comment
            // second comment
            // This code is for temporary branch
            // This code is for temporary-2 branch

            //width = (width != 0) ? width : Console.WindowWidth - (margin * 2) - (hPadding * 2) - 2;
            content = TextAlign(content, width, textAlign);
            //margin = (margin != 0) ? margin : (Console.WindowWidth - margin - width - hPadding) / 2;

            ConsoleColor panelColor, textColor, panelColorHighlight, textColorHighlight;
            if (Enum.TryParse(FirstLetterToUpper(panel), out panelColor)) { }
            else panelColor = ConsoleColor.Gray;
            if (Enum.TryParse(FirstLetterToUpper(text), out textColor)) { }
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
            switch (partToPrint.ToLower())
            {
                case "top":
                    Console.ForegroundColor = panelColor;
                    Console.Write(new string(' ', margin));
                    if (border == 1) Console.WriteLine("┌" + new string('─', width + (hPadding * 2)) + "┐");
                    for (int i = 0; i < vPadding; i++) Panel("br", width: width, margin: margin);
                    break;
                case "bottom":
                    for (int i = 0; i < vPadding; i++) Panel("br", width: width, margin: margin);
                    Console.Write(new string(' ', margin));
                    Console.ForegroundColor = panelColor;
                    if (border == 1) Console.WriteLine("└" + new string('─', width + (hPadding * 2)) + "┘");
                    Console.ResetColor();
                    break;
                case "left":
                    Console.ResetColor();
                    Console.Write(new string(' ', margin));
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
                    Console.Write(new string(' ', Math.Max(width + margin + hPadding + 1 - Console.CursorLeft, 0)));
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
                    for (int i = 0; i < vPadding; i++) Panel("br", width: width);
                    Panel("left", width: width, border: border, highlight: highlight, highlightColor: highlightColor, highlightTextColor: highlightTextColor, margin: margin);
                    Console.Write(content);
                    Panel("right", width: width, border: border, margin: margin);
                    for (int i = 0; i < vPadding; i++) Panel("br", width: width);
                    break;
                case "hr":
                    Panel("left", width: width, margin: margin);
                    Console.Write(new string('─', width));
                    Panel("right", width: width, margin: margin);
                    break;
                case "br":
                    Panel("left", width: width, margin: margin);
                    Console.Write(" ");
                    Panel("right", width: width, margin: margin);
                    break;
                case "heading":
                    Console.WriteLine("");
                    Panel("top", width: width, vPadding: 1, margin: margin);
                    Panel("row", content, textAlign: "center", width: width, margin: margin);
                    if (subheading != "")
                    {
                        Panel("br");
                        Panel("row", subheading, textAlign: "center", width: width, margin: margin);
                    }
                    Panel("bottom", width: width, vPadding: 1, margin: margin);
                    break;
                case "list":
                    string topRow = "";
                    int listWidth = 0;
                    foreach (Column col in cols)
                    {
                        topRow += col.Name + new string(' ', col.Width - col.Name.Length);
                        listWidth += col.Width;
                    }
                    listWidth = Math.Max(listWidth, width);
                    Panel("top", width: listWidth, vPadding: 1);
                    Panel("row", topRow, width: listWidth);
                    Panel("hr", width: listWidth);
                    foreach (var item in list) Panel("row", PrintAssetByColumns(item, cols), width: listWidth);
                    Panel("bottom", width: listWidth, vPadding: 1);
                    break;
            }
        }


        // Generates a panel window and draws it onto the consol to show output in a nice way
        static void PrintPanel(string partToPrint, string content = "", int margin = 14, int hPadding = 2, int vPadding = 0, int border = 1, string textAlign = "", string panel = "", string text = "", bool highlight = false, string highlightColor = "", string highlightTextColor = "", string subheading = "")
        {
            int panelWidth = Console.WindowWidth - (margin * 2) - (hPadding * 2) - 2;
            content = TextAlign(content, panelWidth, textAlign);

            ConsoleColor panelColor, textColor, panelColorHighlight, textColorHighlight;
            if (Enum.TryParse(FirstLetterToUpper(panel), out panelColor)) { }
            else panelColor = ConsoleColor.Gray;
            if (Enum.TryParse(FirstLetterToUpper(text), out textColor)) { }
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
            switch (partToPrint.ToLower())
            {
                case "top":
                    Console.ForegroundColor = panelColor;
                    Console.Write(new string(' ', margin));
                    if (border == 1) Console.WriteLine("┌" + new string('─', panelWidth + (hPadding * 2)) + "┐");
                    for (int i = 0; i < vPadding; i++) PrintPanel("br", margin: margin);
                    break;
                case "bottom":
                    for (int i = 0; i < vPadding; i++) PrintPanel("br", margin: margin);
                    Console.Write(new string(' ', margin));
                    Console.ForegroundColor = panelColor;
                    if (border == 1) Console.WriteLine("└" + new string('─', panelWidth + (hPadding * 2)) + "┘");
                    Console.ResetColor();
                    break;
                case "left":
                    Console.ResetColor();
                    Console.Write(new string(' ', margin));
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
                    Console.Write(new string(' ', Math.Max(panelWidth + margin + hPadding + 1 - Console.CursorLeft, 0)));
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
                    for (int i = 0; i < vPadding; i++) PrintPanel("br");
                    PrintPanel("left", border: border, highlight: highlight, highlightColor: highlightColor, highlightTextColor: highlightTextColor, margin: margin);
                    Console.Write(content);
                    PrintPanel("right", border: border, margin: margin);
                    for (int i = 0; i < vPadding; i++) PrintPanel("br");
                    break;
                case "hr":
                    PrintPanel("left", margin: margin);
                    Console.Write(new string('─', panelWidth));
                    PrintPanel("right", margin: margin);
                    break;
                case "br":
                    PrintPanel("left", margin: margin);
                    Console.Write(" ");
                    PrintPanel("right", margin: margin);
                    break;
                case "heading":
                    Console.WriteLine("");
                    PrintPanel("top", vPadding: 1, margin: margin);
                    PrintPanel("row", content, textAlign: "center", margin: margin);
                    if (subheading != "")
                    {
                        PrintPanel("br");
                        PrintPanel("row", subheading, textAlign: "center", margin: margin);
                    }
                    PrintPanel("bottom", vPadding: 1, margin: margin);
                    break;
            }
        }

        static string TextAlign(string text = "", int boxLength = 1, string textAlign = "left")
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

        static string? FirstLetterToUpper(string str)
        {
            if (str == null) return null;
            if (str.Length > 1) return char.ToUpper(str[0]) + str.Substring(1).ToLower();
            return str.ToUpper();
        }

    }
}
