using System;
using System.Drawing;
using System.IO;

namespace CompressImage
{
    class Program
    {
        public static string MakeThumbnail(string originalImagePath, string thumbnailPath, int width, int height, string mode)
        {
            System.Drawing.Image originalImage = System.Drawing.Image.FromFile(originalImagePath);

            int towidth = width;
            int toheight = height;

            int x = 0;
            int y = 0;
            int ow = originalImage.Width;
            int oh = originalImage.Height;

            switch (mode)
            {
                case "HW":
                    break;
                case "W":
                    toheight = originalImage.Height * width / originalImage.Width;
                    break;
                case "H":
                    towidth = originalImage.Width * height / originalImage.Height;
                    break;
                case "Cut":
                    if ((double)originalImage.Width / (double)originalImage.Height > (double)towidth / (double)toheight)
                    {
                        oh = originalImage.Height;
                        ow = originalImage.Height * towidth / toheight;
                        y = 0;
                        x = (originalImage.Width - ow) / 2;
                    }
                    else
                    {
                        ow = originalImage.Width;
                        oh = originalImage.Width * height / towidth;
                        x = 0;
                        y = (originalImage.Height - oh) / 2;
                    }
                    break;
                default:
                    break;
            }


            System.Drawing.Image bitmap = new System.Drawing.Bitmap(towidth, toheight);


            Graphics g = System.Drawing.Graphics.FromImage(bitmap);


            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;


            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;


            g.Clear(Color.Transparent);


            g.DrawImage(originalImage, new Rectangle(0, 0, towidth, toheight),
             new Rectangle(x, y, ow, oh),
             GraphicsUnit.Pixel);

            try
            {

                bitmap.Save(thumbnailPath, System.Drawing.Imaging.ImageFormat.Jpeg);

            }
            catch (System.Exception e)
            {
                throw e;
            }
            finally
            {
                originalImage.Dispose();
                bitmap.Dispose();
                g.Dispose();
            }
            return "";
        }
        static void Main(string[] args)
        {
           string  originalImagePath = "";
            string thumbnailPath = "";

            Console.WriteLine("Enter You Path:");
            originalImagePath = Console.ReadLine();
            thumbnailPath = originalImagePath;
            var files = Directory.GetFiles(originalImagePath, "*.*", SearchOption.TopDirectoryOnly);

            foreach (string s in files)
            {
                FileInfo info = new FileInfo(s);
                MakeThumbnail(info.FullName,thumbnailPath+"/Thumb_"+info.Name, 270, 340, "sd");
            }
            MakeThumbnail(originalImagePath, thumbnailPath, 130, 240, "sd");

         
        }


        private static  Bitmap ResizeImage(String filename, int maxWidth, int maxHeight)
        {
            using (Image originalImage = Image.FromFile(filename))
            {
                //Caluate new Size
                int newWidth = originalImage.Width;
                int newHeight = originalImage.Height;
                double aspectRatio = (double)originalImage.Width / (double)originalImage.Height;
                if (aspectRatio <= 1 && originalImage.Width > maxWidth)
                {
                    newWidth = maxWidth;
                    newHeight = (int)Math.Round(newWidth / aspectRatio);
                }
                else if (aspectRatio > 1 && originalImage.Height > maxHeight)
                {
                    newHeight = maxHeight;
                    newWidth = (int)Math.Round(newHeight * aspectRatio);
                }
                Bitmap newImage = new Bitmap(newWidth, newHeight);
                using (Graphics g = Graphics.FromImage(newImage))
                {
                    //--Quality Settings Adjust to fit your application
                    g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBilinear;
                    g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                    g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
                    g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                    g.DrawImage(originalImage, 0, 0, newImage.Width, newImage.Height);
                    return newImage;
                }
            }
        }
    }
}
