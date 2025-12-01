using doAn.popUp.quanlySanPham.sanPham;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
namespace doAn.quanLySanPham
{
    public partial class SanPham : UserControl
    {

        public string MaSanPham { get; set; }
        public string AnhDaiDien { get; set; }


        public SanPham()
        {
            InitializeComponent();
        }

        public void setData (string TenSanPham, string AnhDaiDien)
        {
            //Khong the di thang tu string sang, phai EP KIEU @@
            //xai package de no hieu la cai string luu trong database => path
            //co Package ho tro chuyen do luon
            //CAI O TREN DAY LA KO CO TAC DUNG GI HET A'

            string path = AnhDaiDien;

            //Console.WriteLine("nigga: "+path);

            pictureBox.Image = Image.FromFile(path);

            lblTenSanPham.Text = TenSanPham;
        }

        private void btnChiTiet_Click(object sender, EventArgs e)
        {
            var ct = new ChiTietSanPham(MaSanPham);
            ct.ShowDialog();
        }

        
    }
}
