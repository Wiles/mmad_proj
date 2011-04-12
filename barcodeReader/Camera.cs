using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

using VideoCapture2005;

namespace barcodeReader
{
    class Camera
    {
        vidcap2005 cap = null;
        public Camera()
        {
            /*
            string s = "";

            cap = new vidcap2005();
            cap.InitializeCapture(640, 480, 30, 0);
            cap.ControlCapture(true);
             */
        }
        public Bitmap AcquireImage()
        {
            return new Bitmap("..\\barcodeExamples\\bitmap4.bmp");
        }
    }
}
