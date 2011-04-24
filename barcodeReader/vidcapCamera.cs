using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

using VideoCapture2005;
using System.IO;

namespace barcodeReader
{
    /// <summary>
    /// Implements the camera interface for a vidcap device
    /// </summary>
    class vidcapCamera : Camera, IDisposable
    {
        vidcap2005 cap;

        /// <summary>
        /// Constructor
        /// </summary>
        public vidcapCamera(Int32 source = 0)
        {
            cap = new vidcap2005();
            cap.InitializeCapture(640, 480, 15, source);
            cap.ControlCapture(true);
        }

        /// <summary>
        /// Gets and returns a bitmap image from the camera device
        /// </summary>
        /// <returns>Bitmap image</returns>
        public Bitmap AcquireImage()
        {
            Bitmap image = (Bitmap)Bitmapper.Copy(new Bitmap(cap.GetSnapshot()));
            return image;
        }

        ~vidcapCamera()
        {
            this.Dispose();
        }

        public void Dispose()
        {
            cap.EndCapture();
        }
    }
}
