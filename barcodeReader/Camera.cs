﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

using VideoCapture2005;
using System.IO;

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
            string x = Directory.GetCurrentDirectory();

            return new Bitmap("..\\..\\..\\barcodeExamples\\barcode3.bmp");
        }
    }
}