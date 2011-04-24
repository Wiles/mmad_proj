using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using System.Runtime.InteropServices;
using System.Drawing;

namespace barcodeReader
{
    /// <summary>
    /// Processes images for UPC-a barcode
    /// </summary>
    class Barcode
    {
        static public byte threshold = 128;
        static private Int32 rowsToAverage = 20;

        /// <summary>
        /// Attempts to read a barcode out of an image
        /// </summary>
        /// <param name="Image">Image to read</param>
        /// <returns>barcode</returns>
        public static string DecodeImage(Bitmap Image)
        {
            string barcode = "";

            Bitmapper.ThresholdImage(Image, threshold);

            Double[] lineWidth = new Double[59];

            Int32 startRow = (Image.Height / 2) - (rowsToAverage / 2);

            for (int i = 0; i < rowsToAverage; ++i )
            {
                Int32[] row = ReadLine(Image, startRow + i);
                Int32 offset = -1;
                Int32 currentColor = row[0];
                Int32 rowWidth = 0;
                for (int j = 0; j < row.Length; ++j)
                {
                    if (row[j] == currentColor)
                    {
                        ++rowWidth;
                        continue;
                    }
                    else
                    {
                        if (offset == -1)
                        {
                            ++offset;
                            currentColor = row[j];
                            rowWidth = 1;
                        }
                        else
                        {
                            currentColor = row[j];
                            lineWidth[offset] += rowWidth;
                            rowWidth = 1;
                            offset++;
                            if (offset == lineWidth.Length)
                            {
                                break;
                            }
                        }
                    }
                }
            }

            //Average out reads
            for (Int32 i = 0; i < lineWidth.Length; ++i )
            {
                lineWidth[i] /= rowsToAverage;
            }

            //scale line thicknesses
            Double thickness = (lineWidth[0] + lineWidth[1] + lineWidth[2]) / 3;

            for (Int32 i = 0; i < lineWidth.Length; ++i)
            {
                //This part is important gotta get scaling of the line thicknesses right.
                if (lineWidth[i] < thickness*1.5)
                {
                    lineWidth[i] = 1;
                }
                else if (lineWidth[i] < thickness * 2.5)
                {
                    lineWidth[i] = 2;
                }
                else if (lineWidth[i] < thickness * 3.5)
                {
                    lineWidth[i] = 3;
                }
                else
                {
                    lineWidth[i] = 4;
                }
            }

            barcode = DecodeThicknesses(lineWidth);

            if (ValidateBarCode(barcode) == false)
            {
                throw new ArgumentException("Invalid barcode: " + barcode);
            }
            return barcode;
        }

        /// <summary>
        /// Takes an array of thicknesses and converts this into a barcode number value
        /// </summary>
        /// <param name="lines">array of thicknesses</param>
        /// <returns>barcode</returns>
        private static string DecodeThicknesses( Double[] lines )
        {
            if( lines.Length != 59 )
            {
                throw new ArgumentException("Must have 30 bars");
            }

            //Check start
            if (lines[0] != 1 || lines[1] != 1 || lines[2] != 1)
            {
                throw new ArgumentException("Start sequence incorrect");
            }
            //Check End
            if (lines[56] != 1 || lines[57] != 1 || lines[58] != 1)
            {
                throw new ArgumentException("End sequence incorrect");
            }
            //Check middle
            if (lines[27] != 1 || lines[28] != 1 || lines[29] != 1)
            {
                throw new ArgumentException("Middle sequence incorrect");
            }

            //remove checks
            Double[] numbers = new Double[48];
            Array.Copy(lines, 3, numbers, 0, 24);
            Array.Copy(lines, 32, numbers, 24, 24);

            string barcode = "";
            for (int i = 0; i < numbers.Length; i += 4)
            {
                string lineWidth = numbers[i].ToString() + numbers[i + 1].ToString() + numbers[i + 2].ToString() + numbers[i + 3].ToString(); 
                switch( lineWidth )
                {
                    case "3211":
                        barcode += "0";
                        break;
                    case "2221":
                        barcode += "1";
                        break;
                    case "2122":
                        barcode += "2";
                        break;
                    case "1411":
                        barcode += "3";
                        break;
                    case "1132":
                        barcode += "4";
                        break;
                    case "1231":
                        barcode += "5";
                        break;
                    case "1114":
                        barcode += "6";
                        break;
                    case "1312":
                        barcode += "7";
                        break;
                    case "1213":
                        barcode += "8";
                        break;
                    case "3112":
                        barcode += "9";
                        break;
                    default:
                        //TODO error
                        break;

                }
            }
            return barcode;
        }

        /// <summary>
        /// Returns the green value for the middle 3/4 of one line of the image
        /// </summary>
        /// <param name="image">The image to read line from</param>
        /// <param name="row">Which line to return</param>
        /// <returns>the read line</returns>
        private static Int32[] ReadLine( Bitmap image, Int32 row )
        {
            /*
            Rectangle rect = new Rectangle(0, 0, image.Width, image.Height);
            System.Drawing.Imaging.BitmapData bmpData =
                image.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadWrite,
                             System.Drawing.Imaging.PixelFormat.Format32bppRgb);

            IntPtr ptr = bmpData.Scan0;

            int numPixels = bmpData.Width * image.Height;
            int numBytes = numPixels * sizeof(int);

            byte[] rgbValues = new byte[numBytes];
            Int32[] grayscaledRow = new Int32[image.Width];

            Marshal.Copy(ptr, rgbValues, 0, numBytes);

            for (int i = 0; i < image.Width; ++i)
            {
                Int32 x = ((i * 4) + 2) + image.Width * row;
                grayscaledRow[i] = rgbValues[((i*4) + 2) + image.Width * row];
            }

            Marshal.Copy(rgbValues, 0, ptr, numBytes);
            image.UnlockBits(bmpData);

            return grayscaledRow;
             */

            //TODO make this use other way
            Int32 eighthWidth = image.Width / 8;
            Int32[] rowRead = new Int32[eighthWidth * 6];
            for (int i = eighthWidth; i < eighthWidth * 7; ++i)
            {
                rowRead[i - eighthWidth] = image.GetPixel(i, row).G;
            }
            return rowRead;


        }

        private static Boolean ValidateBarCode(string barcode)
        {
            Boolean isValid = true;

            Int32 oddTotal = 0;
            Int32 evenTotal = 0;

            if (barcode.Length < 12)
            {
                return false;
            }

            for (int i = 0; i < (barcode.Length - 1); ++i)
            {
                if ((i - 1) % 2 == 0)
                {
                    evenTotal += barcode[i] - '0';
                }
                else
                {
                    oddTotal += barcode[i] - '0';
                }
            }
            oddTotal *= 3;
            Int32 total = oddTotal + evenTotal;

            total %= 10;

            if ((10 - total) != (barcode[barcode.Length - 1] - '0'))
            {
                isValid = false;
            }

            return isValid;
        }
    }
}
