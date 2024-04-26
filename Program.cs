using System.Drawing.Imaging;
using System.Drawing;
using System.Numerics;
using System.Data.Common;

static class Program
{
    static string mode;
    //Boxes to capture text/colour to see whether a round has ended or read text on the screen
    static Dictionary<string, Rectangle> rects = new Dictionary<string, Rectangle>();
    static Color green = Color.FromArgb(255, 0, 216, 90);
    static Color red = Color.FromArgb(255, 241, 66, 66);
    static bool hasFailed = false;


    public static void Main()
    {
        //All measurements for maximised window
        //1080p
        /*rects.Add("Left", new Rectangle(123, 366, 772, 290));
        rects.Add("Right", new Rectangle(1028, 365, 870, 300));
        rects.Add("Centre", new Rectangle(960, 480, 1, 1));*/
        //1440p
        rects.Add("Left", new Rectangle(100, 550, 1117, 373));
        rects.Add("Right", new Rectangle(1370, 550, 1148, 365));
        rects.Add("Centre", new Rectangle(1280, 650, 1, 1));
        

        mode = Console.ReadLine();

        awaitEndOfRound();
        Console.WriteLine("Round End!");
       
    }

    //takes screenshot on primary monitor on co-ordinates specified in Rectangle parameter, Saves it in same file 
    static void Screenshot(string key)
    {
        try
        {
            Bitmap captureBitmap = new Bitmap(rects[key].Width, rects[key].Height, PixelFormat.Format32bppArgb);
            Graphics captureGraphics = Graphics.FromImage(captureBitmap);
            captureGraphics.CopyFromScreen(rects[key].Left, rects[key].Top, 0, 0, rects[key].Size);
            Console.WriteLine(captureBitmap.GetPixel(0, 0));
            captureBitmap.Save(@$"C:\dev\HigherOrLowerBot\Capture{key}.jpg", ImageFormat.Jpeg);
            Console.WriteLine("Capture Taken!");
        }
        catch (Exception e)
        {
            Console.WriteLine("Capture Failed");
        }
    }

    static Func<int, int> negCheck = x  => (x > 0) ? x : x * -1;

    static void awaitEndOfRound()
    {
        int now;
        int lastFrame = Environment.TickCount;
        Color color;

        while (true)
        {
            now = Environment.TickCount;
            int delta = now - lastFrame;
            lastFrame += 5000000;

            if (delta < 5000000)
            {
                System.Threading.Thread.Sleep((5000000 - negCheck(delta)) / 10000) ;
            }

            Bitmap captureBitmap = new Bitmap(1, 1, PixelFormat.Format32bppArgb);
            Graphics captureGraphics = Graphics.FromImage(captureBitmap);
            captureGraphics.CopyFromScreen(rects["Centre"].Left, rects["Centre"].Top, 0, 0, rects["Centre"].Size);
            color = captureBitmap.GetPixel(0, 0);
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

