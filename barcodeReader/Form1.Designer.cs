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
            this.gb_data = new System.Windows.Forms.GroupBox();
            this.wb_browser = new System.Windows.Forms.WebBrowser();
            this.btn_run = new System.Windows.Forms.Button();
            this.tb_errors = new System.Windows.Forms.Label();
            this.btn_stop = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.nud_thres = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pb_barcode = new System.Windows.Forms.PictureBox();
            this.pb_threshold = new System.Windows.Forms.PictureBox();
            this.gb_image.SuspendLayout();
            this.gb_data.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nud_thres)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pb_barcode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pb_threshold)).BeginInit();
            this.SuspendLayout();
            // 
            // gb_image
            // 
            this.gb_image.Controls.Add(this.pictureBox1);
            this.gb_image.Controls.Add(this.pb_barcode);
            this.gb_image.Location = new System.Drawing.Point(12, 71);
            this.gb_image.Name = "gb_image";
            this.gb_image.Size = new System.Drawing.Size(332, 268);
            this.gb_image.TabIndex = 0;
            this.gb_image.TabStop = false;
            this.gb_image.Text = "Raw";
            // 
            // gb_data
            // 
            this.gb_data.Controls.Add(this.pb_threshold);
            this.gb_data.Location = new System.Drawing.Point(350, 71);
            this.gb_data.Name = "gb_data";
            this.gb_data.Size = new System.Drawing.Size(335, 268);
            this.gb_data.TabIndex = 1;
            this.gb_data.TabStop = false;
            this.gb_data.Text = "Thresholded";
            // 
            // wb_browser
            // 
            this.wb_browser.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.wb_browser.Location = new System.Drawing.Point(6, 19);
            this.wb_browser.MinimumSize = new System.Drawing.Size(20, 20);
            this.wb_browser.Name = "wb_browser";
            this.wb_browser.Size = new System.Drawing.Size(658, 147);
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
            this.tb_errors.Location = new System.Drawing.Point(156, 18);
            this.tb_errors.Name = "tb_errors";
            this.tb_errors.Size = new System.Drawing.Size(33, 13);
            this.tb_errors.TabIndex = 4;
            this.tb_errors.Text = "None";
            // 
            // btn_stop
            // 
            this.btn_stop.Location = new System.Drawing.Point(12, 42);
            this.btn_stop.Name = "btn_stop";
            this.btn_stop.Size = new System.Drawing.Size(75, 23);
            this.btn_stop.TabIndex = 5;
            this.btn_stop.Text = "Stop";
            this.btn_stop.UseVisualStyleBackColor = true;
            this.btn_stop.Click += new System.EventHandler(this.btn_stop_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(93, 42);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(57, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Threshold:";
            // 
            // nud_thres
            // 
            this.nud_thres.Location = new System.Drawing.Point(156, 40);
            this.nud_thres.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.nud_thres.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nud_thres.Name = "nud_thres";
            this.nud_thres.Size = new System.Drawing.Size(54, 20);
            this.nud_thres.TabIndex = 7;
            this.nud_thres.Value = new decimal(new int[] {
            128,
            0,
            0,
            0});
            this.nud_thres.ValueChanged += new System.EventHandler(this.nud_thres_ValueChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(95, 18);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(55, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Last Error:";
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.wb_browser);
            this.groupBox1.Location = new System.Drawing.Point(12, 345);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(672, 172);
            this.groupBox1.TabIndex = 9;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Data Lookup";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::barcodeReader.Properties.Resources.overlayImage;
            this.pictureBox1.Location = new System.Drawing.Point(86, 138);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(160, 3);
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            // 
            // pb_barcode
            // 
            this.pb_barcode.Location = new System.Drawing.Point(6, 19);
            this.pb_barcode.Name = "pb_barcode";
            this.pb_barcode.Size = new System.Drawing.Size(320, 240);
            this.pb_barcode.TabIndex = 0;
            this.pb_barcode.TabStop = false;
            // 
            // pb_threshold
            // 
            this.pb_threshold.Location = new System.Drawing.Point(6, 19);
            this.pb_threshold.Name = "pb_threshold";
            this.pb_threshold.Size = new System.Drawing.Size(320, 240);
            this.pb_threshold.TabIndex = 2;
            this.pb_threshold.TabStop = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(703, 529);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.nud_thres);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btn_stop);
            this.Controls.Add(this.tb_errors);
            this.Controls.Add(this.btn_run);
            this.Controls.Add(this.gb_data);
            this.Controls.Add(this.gb_image);
            this.Name = "Form1";
            this.Text = "Form1";
            this.gb_image.ResumeLayout(false);
            this.gb_data.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.nud_thres)).EndInit();
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pb_barcode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pb_threshold)).EndInit();
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
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown nud_thres;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox pb_threshold;
    }
}

