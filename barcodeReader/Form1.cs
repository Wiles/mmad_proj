using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Threading;
using VideoCapture2005;

namespace barcodeReader
{
    /// <summary>
    /// Maining driving class for barcode reader.
    /// </summary>
    public partial class Form1 : Form
    {
        private Int32 readDelay = 250;
        private Camera cam = null;
        private Mutex mu;
        private bool run = false;
        private Int32 webSource = 0;
        private Int32 vidSource = -1;
        private System.Threading.Timer tim = null;
        private string lastBarcode = "";

        private delegate void CaptureImage();
        private CaptureImage cap;

        /// <summary>
        /// Constructor
        /// </summary>
        public Form1()
        {
            InitializeComponent();
            mu = new Mutex();
            cap = new CaptureImage(imgCap);
            nud_thres.Value = Barcode.threshold;

            cb_source.Items.Add("upcdatabase.com");
            cb_source.Items.Add("searchupc.com");
            cb_source.SelectedIndex = 0;
            LoadVidCapSources();
            cb_vidcap.SelectedIndex = 0;
        }

        /// <summary>
        /// Start running reader on click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_run_Click(object sender, EventArgs e)
        {
            start();
            btn_stop.Select();
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
            if (mu.WaitOne(1))
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
                        lastBarcode = code;
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
        /// Takes a barcode and retrives and displays external information about it
        /// </summary>
        /// <param name="barcode">Barcode to attempt to gether information on</param>
        private void DisplayBarcode(string barcode)
        {
            if (webSource == 0)
            {
                wb_browser.Navigate(new Uri("http://www.upcdatabase.com/item/" + barcode));
            }
            else
            {
                wb_browser.Navigate(new Uri("http://searchupc.com/default.aspx?q=" + barcode));
            }
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
                btn_run.Select();
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

        /// <summary>
        /// Stops processing if form loses focus
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void form_lost_focus(object sender, EventArgs e)
        {
            stop();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (webSource == cb_source.SelectedIndex)
            {
                return;
            }
            webSource = cb_source.SelectedIndex;
            if (lastBarcode != "")
            {
                DisplayBarcode(lastBarcode);
            }
        }

        private void cb_vidcap_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cb_vidcap.SelectedIndex == vidSource)
            {
                return;
            }
            try
            {
                if (this.cam != null)
                {
                    this.cam.Dispose();
                }
                Camera cam = new vidcapCamera(cb_vidcap.SelectedIndex);
                this.cam = cam;
                vidSource = cb_vidcap.SelectedIndex;
            }
            catch
            {
                MessageBox.Show("Could Not change devices.");
            }
        }

        private void LoadVidCapSources()
        {
            cb_vidcap.Items.Clear();
            vidcap2005 vid = new vidcap2005();
            for (int i = 0; i < vid.GetNumberOfCaptureDevices(); ++i)
            {
                cb_vidcap.Items.Add(vid.GetCaptureDeviceName(i));
            }
        }

        private void btn_vid_reload_Click(object sender, EventArgs e)
        {
            LoadVidCapSources();
        }
    }
}
