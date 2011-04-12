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
        Camera cam = new Camera();
        public Form1()
        {
            InitializeComponent();
        }

        private void btn_run_Click(object sender, EventArgs e)
        {
            while (true)
            {
                try
                {
                    Bitmap curBarCode = cam.AcquireImage();

                    pb_barcode.Image = Bitmapper.Resize(curBarCode, pb_barcode.Width, pb_barcode.Height);
                    if( curBarCode != null )
                    {
                        string code = Barcode.DecodeImage(ref curBarCode);
                        wb_browser.Navigate(new Uri("http://www.upcdatabase.com/item/" + code));
                    }
                }
                catch(Exception ex)
                {
                    tb_errors.Text = ex.Message;
                    //MessageBox.Show("0");
                    continue;
                }
                break;
            }
        }
    }
}
