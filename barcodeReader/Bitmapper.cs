﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Runtime.InteropServices;
using System.Drawing.Drawing2D;
using System.Drawing;

namespace barcodeReader
{
    class Bitmapper
    {
        const int redOffset = 2;
        const int blueOffset = 1;
        const int greenOffset = 0;
        /// <summary>
        /// Converts a 24/32 bit bitmap to grayscale
        /// </summary>
        /// <param name="image">bitmap to convert</param>
        static public void GrayscaleBitmap(ref Bitmap image)
        {
            const double ratioRed = 0.3;
            const double ratioGreen = 0.59;
            const double ratioBlue = 0.11;

            Rectangle rect = new Rectangle(0, 0, image.Width, image.Height);
            System.Drawing.Imaging.BitmapData bmpData =
                image.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadWrite,
                             System.Drawing.Imaging.PixelFormat.Format32bppRgb);

            IntPtr ptr = bmpData.Scan0;

            int numPixels = bmpData.Width * image.Height;
            int numBytes = numPixels * sizeof(int);

            byte[] rgbValues = new byte[numBytes];

            Marshal.Copy(ptr, rgbValues, 0, numBytes);

            for (int i = 0; i < rgbValues.Length; i += sizeof(int))
            {
                byte gray = (byte)(
                                   (double)(rgbValues[i + greenOffset]) * ratioRed
                                 + (double)(rgbValues[i + blueOffset]) * ratioBlue
                                 + (double)(rgbValues[i + redOffset]) * ratioGreen
                                 );

                rgbValues[i + greenOffset] = gray;
                rgbValues[i + blueOffset] = gray;
                rgbValues[i + redOffset] = gray;
            }

            Marshal.Copy(rgbValues, 0, ptr, numBytes);
            image.UnlockBits(bmpData);
        }

        /// <summary>
        /// Converts a 24/32 bit bitmap to black and white
        /// </summary>
        /// <param name="image">image to convert</param>
        /// <param name="threshold">Black/white luminosity level</param>
        public static void ThresholdImage(ref Bitmap image, byte threshold)
        {
            const byte black = 0xFF;
            const byte white = 0x00;

            //Grayscale image first to get luminousity
            GrayscaleBitmap(ref image);

            Rectangle rect = new Rectangle(0, 0, image.Width, image.Height);
            System.Drawing.Imaging.BitmapData bmpData =
                image.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadWrite,
                             System.Drawing.Imaging.PixelFormat.Format32bppRgb);

            IntPtr ptr = bmpData.Scan0;

            int numPixels = bmpData.Width * image.Height;
            int numBytes = numPixels * sizeof(int);

            byte[] rgbValues = new byte[numBytes];

            Marshal.Copy(ptr, rgbValues, 0, numBytes);

            for (int i = 0; i < rgbValues.Length; i += sizeof(int))
            {
                byte newColor;
                if (rgbValues[i + redOffset] > threshold)
                {
                    newColor = black;
                }
                else
                {
                    newColor = white;
                }

                rgbValues[i + greenOffset] = newColor;
                rgbValues[i + blueOffset] = newColor;
                rgbValues[i + redOffset] = newColor;
            }

            Marshal.Copy(rgbValues, 0, ptr, numBytes);
            image.UnlockBits(bmpData);
        }

        /// <summary>
        /// method for resizing an image
        /// </summary>
        /// <param name="img">the image to resize</param>
        /// <param name="percentage">Percentage of change (i.e for 105% of the original provide 105)</param>
        /// <returns></returns>
        public static Image Resize(Image img, Int32 width, Int32 height)
        {
            //get the height and width of the image
            int originalW = img.Width;
            int originalH = img.Height;

            //get the new size based on the percentage change
            int resizedW = width;
            int resizedH = height;

            //create a new Bitmap the size of the new image
            Bitmap bmp = new Bitmap(resizedW, resizedH);
            //create a new graphic from the Bitmap
            Graphics graphic = Graphics.FromImage((Image)bmp);
            graphic.InterpolationMode = InterpolationMode.HighQualityBicubic;
            //draw the newly resized image
            graphic.DrawImage(img, 0, 0, resizedW, resizedH);
            //dispose and free up the resources
            graphic.Dispose();
            //return the image
            return (Image)bmp;
        }

        public static Image Copy(Image img)
        {
            //create a new Bitmap the size of the new image
            Bitmap bmp = new Bitmap(img.Width, img.Height);
            //create a new graphic from the Bitmap
            Graphics graphic = Graphics.FromImage((Image)bmp);
            graphic.InterpolationMode = InterpolationMode.HighQualityBicubic;
            //draw the newly resized image
            graphic.DrawImage(img, 0, 0, img.Width, img.Width);
            //dispose and free up the resources
            graphic.Dispose();
            //return the image
            return (Image)bmp;
        }
    }
}
