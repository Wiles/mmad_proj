using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace barcodeReader
{
    public partial class Form1 : Form
    {
        Camera cam;
        System.Threading.Timer tim;
        bool run = false;

        public Form1()
        {
            InitializeComponent();
            cam = new Camera();
        }

        private void btn_run_Click(object sender, EventArgs e)
        {
            if (run == false)
            {
                tim = new System.Threading.Timer(new System.Threading.TimerCallback(imgCap), null, 0, 75);
                run = true;
            }
            //while (true)
            //{
            //    try
            //    {
            //        Bitmap curBarCode = cam.AcquireImage();

            //        pb_barcode.Image = curBarCode;
            //        if (curBarCode != null)
            //        {
            //            string code = Barcode.DecodeImage(ref curBarCode);
            //            wb_browser.Navigate(new Uri("http://www.upcdatabase.com/item/" + code));
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        tb_errors.Text = ex.Message;
            //        //MessageBox.Show("0");
            //        continue;
            //    }
            //    break;
            //}
        }

        private void imgCap(object o)
        {
            Bitmap curBarCode = cam.AcquireImage();
            pb_barcode.Image = curBarCode;

            try
            {
                if (curBarCode != null)
                {
                    //string code = Barcode.DecodeImage(ref curBarCode);
                    //wb_browser.Navigate(new Uri("http://www.upcdatabase.com/item/" + code));
                }
            }
            catch (Exception ex)
            {
                tb_errors.Text = ex.Message;
                //MessageBox.Show("0");
            }
        }

        private void buttonStop_Click(object sender, EventArgs e)
        {
            if (run)
            {
                tim.Dispose();
                run = false;
                pb_barcode.Image = null;
            }

        }
    }
}
