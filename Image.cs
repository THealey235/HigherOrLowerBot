using System.Drawing;
using Tesseract;
using Microsoft.Data.Sqlite;
using Dapper;

namespace HigherOrLowerBot;

public struct Image(string id, Rectangle rect, Bitmap bitmap)
{
    private readonly string path = (Path.Combine(Directory.GetCurrentDirectory(), $"{id}.jpeg"));
    public Bitmap Capture = bitmap;
    public readonly Rectangle Rect = rect;
    public string[] Result;
    TesseractEngine Engine = new(Path.Combine(Directory.GetCurrentDirectory(), "tessdata"), "eng");

    public void ReadText()
    {
        using (Page page = Engine.Process(Capture))
        {
            Result = page.GetText().Split("\n");
        }
    }

    //For debugg
    public void ToJpeg()
    {
        Capture.Save(path, System.Drawing.Imaging.ImageFormat.Jpeg);
    }
}

//Structure of the database
public class DataBase
{
    public string Name { get; set; }
    public int Num { get; set; }
}
