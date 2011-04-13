namespace barcodeReader
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.gb_image = new System.Windows.Forms.GroupBox();
            this.pb_barcode = new System.Windows.Forms.PictureBox();
            this.gb_data = new System.Windows.Forms.GroupBox();
            this.wb_browser = new System.Windows.Forms.WebBrowser();
            this.btn_run = new System.Windows.Forms.Button();
            this.tb_errors = new System.Windows.Forms.Label();
            this.btn_stop = new System.Windows.Forms.Button();
            this.gb_image.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pb_barcode)).BeginInit();
            this.SuspendLayout();
            // 
            // gb_image
            // 
            this.gb_image.Controls.Add(this.pb_barcode);
            this.gb_image.Location = new System.Drawing.Point(12, 106);
            this.gb_image.Name = "gb_image";
            this.gb_image.Size = new System.Drawing.Size(332, 268);
            this.gb_image.TabIndex = 0;
            this.gb_image.TabStop = false;
            this.gb_image.Text = "Image";
            // 
            // pb_barcode
            // 
            this.pb_barcode.Location = new System.Drawing.Point(6, 19);
            this.pb_barcode.Name = "pb_barcode";
            this.pb_barcode.Size = new System.Drawing.Size(320, 240);
            this.pb_barcode.TabIndex = 0;
            this.pb_barcode.TabStop = false;
            // 
            // gb_data
            // 
            this.gb_data.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.gb_data.Location = new System.Drawing.Point(350, 106);
            this.gb_data.Name = "gb_data";
            this.gb_data.Size = new System.Drawing.Size(379, 268);
            this.gb_data.TabIndex = 1;
            this.gb_data.TabStop = false;
            this.gb_data.Text = "Data";
            // 
            // wb_browser
            // 
            this.wb_browser.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.wb_browser.Location = new System.Drawing.Point(13, 380);
            this.wb_browser.MinimumSize = new System.Drawing.Size(20, 20);
            this.wb_browser.Name = "wb_browser";
            this.wb_browser.Size = new System.Drawing.Size(717, 137);
            this.wb_browser.TabIndex = 2;
            // 
            // btn_run
            // 
            this.btn_run.Location = new System.Drawing.Point(12, 13);
            this.btn_run.Name = "btn_run";
            this.btn_run.Size = new System.Drawing.Size(75, 23);
            this.btn_run.TabIndex = 3;
            this.btn_run.Text = "Run";
            this.btn_run.UseVisualStyleBackColor = true;
            this.btn_run.Click += new System.EventHandler(this.btn_run_Click);
            // 
            // tb_errors
            // 
            this.tb_errors.AutoSize = true;
            this.tb_errors.Location = new System.Drawing.Point(175, 18);
            this.tb_errors.Name = "tb_errors";
            this.tb_errors.Size = new System.Drawing.Size(0, 13);
            this.tb_errors.TabIndex = 4;
            // 
            // btn_stop
            // 
            this.btn_stop.Location = new System.Drawing.Point(94, 13);
            this.btn_stop.Name = "btn_stop";
            this.btn_stop.Size = new System.Drawing.Size(75, 23);
            this.btn_stop.TabIndex = 5;
            this.btn_stop.Text = "Stop";
            this.btn_stop.UseVisualStyleBackColor = true;
            this.btn_stop.Click += new System.EventHandler(this.btn_stop_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(741, 529);
            this.Controls.Add(this.btn_stop);
            this.Controls.Add(this.tb_errors);
            this.Controls.Add(this.btn_run);
            this.Controls.Add(this.wb_browser);
            this.Controls.Add(this.gb_data);
            this.Controls.Add(this.gb_image);
            this.Name = "Form1";
            this.Text = "Form1";
            this.gb_image.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pb_barcode)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox gb_image;
        private System.Windows.Forms.PictureBox pb_barcode;
        private System.Windows.Forms.GroupBox gb_data;
        private System.Windows.Forms.WebBrowser wb_browser;
        private System.Windows.Forms.Button btn_run;
        private System.Windows.Forms.Label tb_errors;
        private System.Windows.Forms.Button btn_stop;
    }
}

