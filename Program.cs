using System.Drawing.Imaging;
using System.Drawing;

static class Program
{
    static string mode;
    static Rectangle rectLeft = new Rectangle(123, 366, 772, 290);
    static Rectangle rectRight = new Rectangle(1028, 365, 870, 300);

    public static void Main()
    {
        //Console.Write("Mode (learn/do): ");
        mode = Console.ReadLine();
        Screenshot(rectLeft, 0);
        Screenshot(rectRight, 1);

    }

    //takes screenshot on primary monitor on co-ordinates specified in Rectangle parameter, Saves it in same file 
    static void Screenshot(Rectangle captureRectangle, int rectNum)
    {
        try
        {
            Bitmap captureBitmap = new Bitmap(1024, 768, PixelFormat.Format32bppArgb);
            Graphics captureGraphics = Graphics.FromImage(captureBitmap);
            captureGraphics.CopyFromScreen(captureRectangle.Left, captureRectangle.Top, 0, 0, captureRectangle.Size);
            captureBitmap.Save(@$"C:\dev\HigherOrLowerBot\Capture{rectNum}.jpg", ImageFormat.Jpeg);
            Console.WriteLine("Capture Taken!");
        }
        catch (Exception e)
        {
            Console.WriteLine("Capture Failed");
        }
    }
}

