/**
 * \file Barcode.cs
 * \instructor Ignac Kolenko
 * \course Multimedia Application Development
 * \assignment Milestone #3
 * \author Samuel Lewis, Hekar, Thomas
 * \brief
 *  
 */


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
        public static byte threshold = 128;
        public static Int32 rowsToAverage = 50;
        private static string[] widths = null;

        public string barcode = "";
        public Int32 uncertainty = 0;
        private Bitmap originalImage = null;
        public Bitmap Image
        {
            get{return this.originalImage;}
        }

        /// <summary>
        /// Attempts to create a barcode out of an image
        /// </summary>
        /// <param name="Image">Image to read</param>
        /// <exception cref="BarcodeException">No valid barcode found in image.</exception>
        public Barcode(Bitmap image)
        {
            if (image.Height < Barcode.rowsToAverage)
            {
                throw new BarcodeException("Image to short.");
            }
            DecodeImage(image);
            this.originalImage = (Bitmap)Bitmapper.Copy(image);
        }

        /// <summary>
        /// Attempts to create a barcode out of a string
        /// </summary>
        /// <param name="barcode">String to interpet as barcode</param>
        /// <exception cref="BarcodeException">No valid barcode found in string.</exception>
        public Barcode(String barcode)
        {
            if (ValidateBarCode(barcode))
            {
                this.barcode = barcode;
            }
            else
            {
                throw new BarcodeException("Checksum fail: " + barcode);
            }
        }

        
        /// <summary>
        /// Attempts to read a barcode out of an image
        /// </summary>
        /// <param name="Image">Image to read</param>
        /// <returns>barcode</returns>
        private void DecodeImage(Bitmap Image)
        {
            this.barcode = "";

            Bitmapper.ThresholdImage(Image, threshold);

            Double[] lineWidth = new Double[59];

            Int32 startRow = (Image.Height / 2) - (rowsToAverage / 2);

            for (int i = 0; i < rowsToAverage; ++i )
            {
                // Read in a single line
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
                            ++offset;
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

            DecodeThicknesses(lineWidth);

            if (ValidateBarCode(barcode) == false)
            {
                throw new BarcodeException("Checksum fail: " + barcode);
            }
        }

        /// <summary>
        /// Takes an array of thicknesses and converts this into a barcode number value
        /// </summary>
        /// <param name="lines">array of thicknesses</param>
        /// <returns>barcode</returns>
        private void DecodeThicknesses( Double[] lines )
        {
            Boolean valid = true;
            if( lines.Length != 59 )
            {
                uncertainty += 2;
            }

            //Check start
            if (lines[0] != 1 || lines[1] != 1 || lines[2] != 1)
            {
                uncertainty += 1;
            }
            //Check End
            if (lines[56] != 1 || lines[57] != 1 || lines[58] != 1)
            {
                uncertainty += 1;
            }
            //Check middle
            if (lines[27] != 1 || lines[28] != 1 || lines[29] != 1 || lines[30] != 1 || lines[31] != 1)
            {
                throw new BarcodeException("Middle sequence incorrect");
            }

            //remove checks
            Double[] numbers = new Double[48];
            Array.Copy(lines, 3, numbers, 0, 24);
            Array.Copy(lines, 32, numbers, 24, 24);

            // Decode the barcode through the bar widths
            for (int i = 0; i < numbers.Length; i += 4)
            {
                string lineWidth = numbers[i].ToString() + numbers[i + 1].ToString() + numbers[i + 2].ToString() + numbers[i + 3].ToString();

                char nextNumber;
                Int32 numberUncertainty;

                if (i == 11)
                {
                    //Checksum must be exact
                    nextNumber = findBestMatch(lineWidth, out numberUncertainty, 0);
                }
                else
                {
                    nextNumber = findBestMatch(lineWidth, out numberUncertainty);
                }
                if( nextNumber == '_' )
                {
                    valid = false;
                }
                this.uncertainty += numberUncertainty;
                this.barcode += nextNumber;
            }
            if (valid == false)
            {
                throw new BarcodeException("Unreadable number(s): " + barcode);
            }
        }

        /// <summary>
        /// Takes a string representing a set of bar width and tries to find a matching barcode number
        /// </summary>
        /// <param name="lineWidths">Bar thicknesses</param>
        /// <param name="uncertainty">Output parameter, how much it trues it's choise. Lower is better.</param>
        /// <param name="differenceTolerence">How close the line widths have to match the stored examples</param>
        /// <returns>barcode</returns>
        private char findBestMatch(string lineWidth, out Int32 uncertainty, Int32 differenceTolerence = 2)
        {
            if (Barcode.widths == null)
            {
                Barcode.widths = new string[10]{
                    "3211",
                    "2221",
                    "2122",
                    "1411",
                    "1132",
                    "1231",
                    "1114",
                    "1312",
                    "1213",
                    "3112"
                };
            }

            char bestMatch = '_';
            Int32 matchFactor = Int32.MaxValue;
            Int32 offset = 0;
            Int32 difference = 0;

            foreach(string width in widths)
            {
                difference = 0;
                for (int i = 0; i < width.Length; ++i)
                {
                    difference += Math.Abs(lineWidth[i] - width[i]);
                }

                if (difference <= matchFactor && difference <= differenceTolerence)
                {
                    matchFactor = difference;
                    bestMatch = (char)(offset + '0');
                }
                ++offset;
            }
            uncertainty = matchFactor;
            return bestMatch;
        }

        /// <summary>
        /// Returns the green value for the middle 3/4 of one line of the image
        /// </summary>
        /// <param name="image">The image to read line from</param>
        /// <param name="row">Which line to return</param>
        /// <returns>the read line</returns>
        private Int32[] ReadLine( Bitmap image, Int32 row )
        {
            Int32[] rowRead = new Int32[image.Width];

            //TODO: get this working!
#if false
            Rectangle rect = new Rectangle(0, 0, image.Width, image.Height);
            System.Drawing.Imaging.BitmapData bmpData =
                image.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadWrite,
                             System.Drawing.Imaging.PixelFormat.Format32bppRgb);

            IntPtr ptr = bmpData.Scan0;

            int numPixels = bmpData.Width * image.Height;
            int numBytes = numPixels * sizeof(int);

            byte[] rgbValues = new byte[numBytes];

            Marshal.Copy(ptr, rgbValues, 0, numBytes);
            Int32 start = (image.Width * row + eighthWidth + 2)*4;
            Int32 end = start + (eighthWidth * 24);
            for (int i = start; i < end; i += sizeof(int))
            {
                try
                {
                    rowRead[(i - start)/4] = rgbValues[i];
                }
                catch (Exception ex)
                {
                    throw;
                }
            }

            Marshal.Copy(rgbValues, 0, ptr, numBytes);
            image.UnlockBits(bmpData);

            return rowRead;
#endif // false
            
            //TODO make this use other way
            for (int i = 0; i < image.Width; ++i)
            {
                rowRead[i] = image.GetPixel(i, row).G;
            }
            return rowRead;
            
        }

        /// <summary>
        /// Checks the checksum against the number in the barcode
        /// </summary>
        /// <param name="barcode">String representing a barcode</param>
        /// <returns>true if checksum is valid</returns>
        private Boolean ValidateBarCode(string barcode)
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
