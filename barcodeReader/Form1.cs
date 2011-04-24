using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Threading;

namespace barcodeReader
{
    /// <summary>
    /// Maining driving class for barcode reader.
    /// </summary>
    public partial class Form1 : Form
    {
        private Int32 readDelay = 250;
        private Camera cam;
        private Mutex mu;
        private bool run = false;
        private System.Threading.Timer tim = null;

        private delegate void CaptureImage();
        private CaptureImage cap;

        /// <summary>
        /// Constructor
        /// </summary>
        public Form1()
        {
            InitializeComponent();
            cam = new vidcapCamera();
            mu = new Mutex();
            cap = new CaptureImage(imgCap);
            nud_thres.Value = Barcode.threshold;
        }

        /// <summary>
        /// Start running reader on click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_run_Click(object sender, EventArgs e)
        {
            start();
        }

        /// <summary>
        /// Time to process another image 
        /// </summary>
        /// <param name="o"></param>
        private void imageEvent(object o)
        {
            this.Invoke(this.cap);
        }

        /// <summary>
        /// Requests and attempts to read an image for a barcode
        /// </summary>
        private void imgCap()
        {
            if (!run)
            {
                return;
            }
            if (mu.WaitOne(10))
            {
                try
                {
                    Bitmap curBarCode = (Bitmap)cam.AcquireImage();
                    pb_barcode.Image = Bitmapper.Resize(curBarCode, pb_barcode.Width, pb_barcode.Height);
                    Bitmap temp = new Bitmap(pb_barcode.Image);
                    Bitmapper.ThresholdImage(temp, Barcode.threshold);
                    pb_threshold.Image = temp;

                    if (curBarCode != null)
                    {
                        string code = Barcode.DecodeImage(curBarCode);
                        DisplayBarcode(code);
                        stop();
                    }
                }
                catch (Exception ex)
                {
                    tb_errors.Text = ex.Message;
                }
                finally
                {
                    mu.ReleaseMutex();
                }
            }
        }

        /// <summary>
        /// Stop processing images on click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_stop_Click(object sender, EventArgs e)
        {
            stop();
        }

        /// <summary>
        /// Takes a barcode and retrives and displays external information about it
        /// </summary>
        /// <param name="barcode">Barcode to attempt to gether information on</param>
        private void DisplayBarcode(string barcode)
        {
            wb_browser.Navigate(new Uri("http://www.upcdatabase.com/item/" + barcode));            
        }

        /// <summary>
        /// Sets up the application for periodically reading a barcode
        /// </summary>
        private void start()
        {
            if (run == false)
            {
                tim = new System.Threading.Timer(new System.Threading.TimerCallback(imageEvent), null, 0, readDelay);
                run = true;
                btn_run.Enabled = false;
                btn_stop.Enabled = true;
            }
        }

        /// <summary>
        /// Stops the applicatin from reading barcodes
        /// </summary>
        private void stop()
        {
            if (run)
            {
                if (tim != null)
                {
                    tim.Dispose();
                }
                run = false;
                btn_run.Enabled = true;
                btn_stop.Enabled = false;
            }
        }

        /// <summary>
        /// Allows user to adjust the threshold value,
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void nud_thres_ValueChanged(object sender, EventArgs e)
        {
            Barcode.threshold = (byte)nud_thres.Value;
        }
    }
}
