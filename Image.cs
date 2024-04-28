#pragma warning disable CA1416
using System.Drawing;
using Tesseract;

namespace HigherOrLowerBot;

public struct Image(string id, Rectangle rect, Bitmap bitmap)
{
    private readonly string path = (Path.Combine(Directory.GetCurrentDirectory(), $"{id}.jpeg"));
    public Bitmap Capture = bitmap;
    public readonly Rectangle Rect = rect;
    TesseractEngine Engine = new(Path.Combine(Directory.GetCurrentDirectory(), "tessdata"), "eng");

    public void ReadText()
    {
        using (Page page = Engine.Process(Capture))
        {
            var result = page.GetText().Split("\n");

            result = result.Except(new List<string> { string.Empty }).ToArray();

            for (int i = 0; i < result.Length; i++)
            {
                result[i] = result[i].Replace(",", "");
            }

            TryInserting(result);
            
        }

    }
    
    private static void TryInserting(string[] result)
    {
        try
        {
            if (Int32.TryParse(result[1], out var x))
            {
                SqliteDataAccess.LoadData(result[0], Int32.Parse(result[1]));
                Console.WriteLine("Processed.");
            }
            else
            {
                Console.WriteLine("Subheading read by Tess is NaN");
                Console.Write("Text read was: ");
                foreach (var i in result)
                {
                    Console.Write("Item: " + i + ", ");
                }
                Console.WriteLine("\nCan you help?\nIn the format: (Name, Num) or \"skip\"");
                result = Console.ReadLine().Split(", ");
                TryInserting(result);
            }
        }
        catch (Exception)
        {
            Console.WriteLine("Did not capture 2 items");
        }
    }

    //For debugg
    public void ToJpeg()
    {
        Capture.Save(path, System.Drawing.Imaging.ImageFormat.Jpeg);
    }
}
