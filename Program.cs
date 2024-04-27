using System.Drawing.Imaging;
using System.Drawing;
using Tesseract;

public struct Image
{
    string ID;
    string path;
    public string result;
    public Bitmap Capture;
    public Rectangle Rect;

   TesseractEngine engine = new TesseractEngine(Path.Combine(Directory.GetCurrentDirectory(), "tessdata"), "eng");

    public Image(string id, Rectangle rect, Bitmap bitmap)
    {
        Rect = rect;
        Capture = bitmap;
        ID = id;
        path = (Path.Combine(Directory.GetCurrentDirectory(), $"{ID}.jpeg"));
    }

    public void readText()
    {
        ToJpeg();
        var ocrInput = Pix.LoadFromFile(path);
        using (Page page = engine.Process(ocrInput))
        {
            result = page.GetText();
        }
        Console.WriteLine(result);
    } 

    public void ToJpeg()
    {
        Capture.Save(path, System.Drawing.Imaging.ImageFormat.Jpeg);
    }
}


static class Program
{
    static string mode;
    //Boxes to capture text/colour to see whether a round has ended or read text on the screen
    static Dictionary<string, Image> images = new Dictionary<string, Image>();
    static bool hasFailed = false;
    static Color green = Color.FromArgb(255, 0, 216, 90);
    static Color red = Color.FromArgb(255, 241, 66, 66);


    public static void Main()
    {

        //All measurements for a maximised window
        //1080p
        /*images.Add("Left", new Image("Left",new Rectangle(123, 366, 772, 290), new Bitmap(772, 290, PixelFormat.Format32bppArgb)));
        images.Add("Right", new Image("Right",new Rectangle(1028, 365, 870, 300), new Bitmap(870, 300, PixelFormat.Format32bppArgb)));
        images.Add("Centre", new Image("Centre",new Rectangle(960, 480, 1, 1), new Bitmap(1, 1, PixelFormat.Format32bppArgb)));*/
        //1440p
        images.Add("Left", new Image("Left", new Rectangle(100, 550, 1117, 373), new Bitmap(1117, 373, PixelFormat.Format32bppArgb)));
        images.Add("Right", new Image("Right", new Rectangle(1370, 550, 1148, 365), new Bitmap(1148, 365, PixelFormat.Format32bppArgb)));
        images.Add("Centre", new Image("Centre", new Rectangle(1280, 650, 1, 1), new Bitmap(1, 1, PixelFormat.Format32bppArgb)));

        while (true)
        {
            Console.WriteLine("Waiting");
            awaitEndOfRound();
            Console.WriteLine("Round End!");

            Screenshot("Left");
            Screenshot("Right");
        }
    }

    //takes screenshot on primary monitor on co-ordinates specified in Rectangle parameter, Saves it in same file 
    static void Screenshot(string key)
    {
        Graphics captureGraphics = Graphics.FromImage(images[key].Capture);
        captureGraphics.CopyFromScreen(images[key].Rect.Left, images[key].Rect.Top, 0, 0, images[key].Rect.Size);
        if (key != "Centre")
        {
            images[key].readText();
        }
    }

    static Func<int, int> negCheck = x => (x > 0) ? x : x * -1;

    //waits until the round has ended: sees whether one of the middle pixels has changed to green/red
    static void awaitEndOfRound()
    {
        int now;
        int lastFrame = Environment.TickCount;
        Color color;

        while (true)
        {
            now = Environment.TickCount;
            int delta = now - lastFrame;
            //this is inacurate but womp womp
            lastFrame = now;

            if (delta < 5000000)
            {
                System.Threading.Thread.Sleep((5000000 - negCheck(delta)) / 10000);
            }

            //slightly altered version of the screenshot method (bitmap is not saved as a jpeg)
            Screenshot("Centre");
            color = images["Centre"].Capture.GetPixel(0, 0);
            if (color == green)
            {
                break;
            }
            else if (color == red)
            {
                hasFailed = true;
                break;
            }


        }
    }
}

