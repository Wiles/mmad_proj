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
    /// Allows programme to interact with a camera without knowing the specifics
    /// </summary>
    interface Camera
    {       
        /// <summary>
        /// Gets and returns a bitmap image from the camera device
        /// </summary>
        /// <returns>Bitmap image</returns>
        Bitmap AcquireImage();
        void Dispose();
    }
}
