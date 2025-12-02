namespace doAn.quanLySanPham
{
    partial class SanPham
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.btnHanhDong = new System.Windows.Forms.Button();
            this.lblTenSanPham = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.pictureBox);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.btnHanhDong);
            this.splitContainer1.Panel2.Controls.Add(this.lblTenSanPham);
            this.splitContainer1.Size = new System.Drawing.Size(200, 248);
            this.splitContainer1.SplitterDistance = 166;
            this.splitContainer1.TabIndex = 0;
            // 
            // pictureBox
            // 
            this.pictureBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox.Location = new System.Drawing.Point(0, 0);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(200, 166);
            this.pictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox.TabIndex = 0;
            this.pictureBox.TabStop = false;
            // 
            // btnHanhDong
            // 
            this.btnHanhDong.Location = new System.Drawing.Point(54, 39);
            this.btnHanhDong.Name = "btnHanhDong";
            this.btnHanhDong.Size = new System.Drawing.Size(97, 25);
            this.btnHanhDong.TabIndex = 2;
            this.btnHanhDong.Text = "Hành động";
            this.btnHanhDong.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnHanhDong.UseVisualStyleBackColor = true;
            this.btnHanhDong.Click += new System.EventHandler(this.btnHanhDong_Click);
            // 
            // lblTenSanPham
            // 
            this.lblTenSanPham.AutoEllipsis = true;
            this.lblTenSanPham.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTenSanPham.Location = new System.Drawing.Point(31, 10);
            this.lblTenSanPham.MaximumSize = new System.Drawing.Size(150, 26);
            this.lblTenSanPham.MinimumSize = new System.Drawing.Size(150, 26);
            this.lblTenSanPham.Name = "lblTenSanPham";
            this.lblTenSanPham.Size = new System.Drawing.Size(150, 26);
            this.lblTenSanPham.TabIndex = 0;
            this.lblTenSanPham.Text = "Tên sản phẩm";
            this.lblTenSanPham.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // SanPham
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.White;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.splitContainer1);
            this.Margin = new System.Windows.Forms.Padding(6, 3, 5, 3);
            this.Name = "SanPham";
            this.Size = new System.Drawing.Size(200, 248);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Button btnHanhDong;
        private System.Windows.Forms.Label lblTenSanPham;
        private System.Windows.Forms.PictureBox pictureBox;
    }
}
