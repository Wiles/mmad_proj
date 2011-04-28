/**
 * \file Form1.cs
 * \instructor Ignac Kolenko
 * \course Multimedia Application Development
 * \assignment Milestone #3
 * \author Ignac Kolenko
 * \brief
 *  Main form of application
 */

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
        /// <summary>
        /// Sleep delay between reads
        /// </summary>
        private Int32 readDelay = 250;

        /// <summary>
        /// Camera for reading in live image
        /// </summary>
        private Camera cam = null;

        /// <summary>
        /// Mutex for singular camera access
        /// </summary>
        private Mutex mu;

        /// <summary>
        /// Are we currently capturing live barcodes?
        /// </summary>
        private bool run = false;

        /// <summary>
        /// Currently selected lookup web source
        /// </summary>
        private Int32 webSource = 0;

        /// <summary>
        /// Identifier for image input device
        /// </summary>
        private Int32 vidSource = -1;

        /// <summary>
        /// Timer for timing reads
        /// </summary>
        private System.Threading.Timer tim = null;

        /// <summary>
        /// Last barcode read
        /// </summary>
        private Barcode lastBarcode = null;

        private delegate void CaptureImage();
        private CaptureImage cap;

        /// <summary>
        /// Constructor
        /// </summary>
        public Form1()
        {
            InitializeComponent();
            pb_threshold.SizeMode = PictureBoxSizeMode.CenterImage;
            mu = new Mutex();
            cap = new CaptureImage(imgCap);
            nud_thres.Value = Barcode.threshold;
            nud_lines.Value = Barcode.rowsToAverage;
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

            // Do allow multiple reads at the same time from timer interrupts
            if (mu.WaitOne(1))
            {
                try
                {
                    Bitmap curBarCode = (Bitmap)cam.AcquireImage();
                    pb_barcode.Image = Bitmapper.Resize(curBarCode, pb_barcode.Width, pb_barcode.Height);
                    curBarCode = PrepImage(curBarCode);
                    Bitmap temp = new Bitmap(curBarCode);
                    Bitmapper.ThresholdImage(temp, Barcode.threshold);
                    temp = (Bitmap)Bitmapper.Resize(temp, pb_barcode.Width, temp.Height);
                    pb_threshold.Image = temp;

                    if (curBarCode != null)
                    {
                        Barcode code = new Barcode(curBarCode);

                        if (code.uncertainty > (Int32)nud_uncertainty.Value)
                        {
                            throw new BarcodeException("Uncertainty to high:" + code.uncertainty);
                        }

                        DisplayBarcode(code.barcode);
                        lastBarcode = code;
                        lb_uncertainty.Text = code.uncertainty.ToString();
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
        /// Stops the application from reading barcodes
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
            if (lastBarcode != null)
            {
                DisplayBarcode(lastBarcode.barcode);
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

        /// <summary>
        /// Find the possible cameras and add them to the selection menu
        /// </summary>
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

        private void nud_lines_ValueChanged(object sender, EventArgs e)
        {
            Barcode.rowsToAverage = (Int32)nud_lines.Value;
        }

        private Bitmap PrepImage(Bitmap image)
        {
            float useWidthPercent = (float)nud_width.Value % 101;
            Int32 useWidth = (Int32)((float)image.Width * (useWidthPercent / 100));
            return Bitmapper.Crop(image,(image.Width - useWidth)/2, (image.Height/2)-(Barcode.rowsToAverage/2), useWidth, Barcode.rowsToAverage);
        }
    }
}
