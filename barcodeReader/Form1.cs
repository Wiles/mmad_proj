﻿using System;
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
    public partial class Form1 : Form
    {
        Camera cam;
        Mutex mu;
        bool run = false;
        System.Threading.Timer tim = null;

        public delegate void Capture();
        public Capture cap;

        public Form1()
        {
            InitializeComponent();
            cam = new Camera();
            mu = new Mutex();
            cap = new Capture(imgCap);
        }

        private void btn_run_Click(object sender, EventArgs e)
        {
            if (run == false)
            {
                tim = new System.Threading.Timer(new System.Threading.TimerCallback(imageEvent), null, 0, 500);
                run = true;
                btn_run.Enabled = false;
                btn_stop.Enabled = true;
            }
        }

        private void imageEvent(object o)
        {
            this.Invoke(this.cap);
        }

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
                    pb_barcode.Image = Bitmapper.Copy(curBarCode);

                    if (curBarCode != null)
                    {
                        string code = Barcode.DecodeImage(curBarCode);
                        wb_browser.Navigate(new Uri("http://www.upcdatabase.com/item/" + code));
                        run = false;

                        btn_run.Enabled = true;
                        btn_stop.Enabled = false;
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

        private void btn_stop_Click(object sender, EventArgs e)
        {
            if (run)
            {
                if (tim != null)
                {
                    tim.Dispose();
                }
                run = false;
                pb_barcode.Image = null;
                btn_run.Enabled = true;
                btn_stop.Enabled = false;
            }
        }
    }
}
