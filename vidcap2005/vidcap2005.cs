using System;
using System.Collections.Generic;
using System.Text;
using System.Security;
using System.Runtime.InteropServices;
using System.Drawing;

namespace VideoCapture2005
{
    public class vidcap2005
    {
        // needed to deallocate HBITMAP from TakeSnapshot()
        [DllImport("gdi32.dll")]
        private static extern int DeleteObject(IntPtr hObject);

        // the following are connectors to VIDCAP2005.DLL
        [DllImport("vidcap2005.dll")]
        private static extern int DetermineNumberOfVCapDevices();

        [DllImport("vidcap2005.dll")]
        private static extern void GetVCapDeviceName (int dev, StringBuilder buffer, int maxLen);

        [DllImport("vidcap2005.dll")]
        private static extern IntPtr InitCap(IntPtr hwnd, int w, int h, int fps, int whichDevice, IntPtr pCallback);

        [DllImport("vidcap2005.dll")]
        private static extern void ControlCap(IntPtr pCap, bool fOn);

        [DllImport("vidcap2005.dll")]
        private static extern void EndCap(IntPtr pCap);

        [DllImport("vidcap2005.dll")]
        private static extern void SelectSource(IntPtr pCap, int iSource);

        [DllImport("vidcap2005.dll")]
        private static extern IntPtr TakeSnapshot(IntPtr pCap);

        private IntPtr ptrCapture;

        enum vcap_sources {
            VCAP_SOURCE_SVIDEO,
            VCAP_SOURCE_COMPOSITE,
            VCAP_SOURCE_TV
        };

        public vidcap2005()
        {
            ptrCapture = IntPtr.Zero;
        }

        public int GetNumberOfCaptureDevices()
        {
            return DetermineNumberOfVCapDevices();
        }

        public string GetCaptureDeviceName(int dev)
        {
            StringBuilder buf = new StringBuilder (512);

            // from http://msdn2.microsoft.com/en-us/library/x3txb6xc.aspx 
            // you will learn that to marshall (exchange) a fixed width buffer
            // from .NET down to an unmanaged DLL, StringBuilder is the object
            // to use, and NOT a char array, byte array or string object

            GetVCapDeviceName (dev, buf, buf.Capacity);
            return buf.ToString();
        }

        public bool InitializeCapture(int w, int h, int fps, int whichDevice)
        {
            // for first cut at this interface, we won't support Callbacks,
            // as we then have to deal with managed/unmanaged access to raw data
            // being sent from DLL back to our C# application. yuck.

            ptrCapture = InitCap(IntPtr.Zero, w, h, fps, whichDevice, IntPtr.Zero);
            return (ptrCapture == IntPtr.Zero) ? false : true;
        }

        public void ControlCapture(bool flag_on)
        {
            if (ptrCapture != IntPtr.Zero)
                ControlCap(ptrCapture, flag_on);
        }

        public void EndCapture()
        {
            if (ptrCapture != IntPtr.Zero)
                EndCap(ptrCapture);
            ptrCapture = IntPtr.Zero;
        }

        public void ChangeSource(int source)
        {
            SelectSource(ptrCapture, source);
        }

        public Bitmap GetSnapshot()
        {
            IntPtr bmp;
            Bitmap bm;

            if (ptrCapture != IntPtr.Zero)
            {
                bmp = TakeSnapshot(ptrCapture);

                // convert HBITMAP (IntPtr) into a Bitmap object
                // and delete the original HBITMAP as we don't need it
                // after that ...

                bm = Image.FromHbitmap(bmp);
                DeleteObject(bmp);
            }
            else
            {
                bm = null;
            }
            return bm;
        }

    }
}
