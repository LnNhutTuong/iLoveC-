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
        public int triGia { get; set; }


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

        // ----- PEAK -----
        //Thêm hành động của nút tùy vô hành động

        //event tu tao luon *vip*
        public event EventHandler XemChiTiet;
        public event EventHandler ChonSanPham;
        public event EventHandler HuyChon;
        public event EventHandler XoaSanPham;


        private string mode;
        public string _mode
        {
            //lay cai text cua nut ra
            get => mode;
            set
            {
                //xam`
                mode = value;

                switch (value)
                {
                    case "view":
                        btnHanhDong.Text = "Xem chi tiết";
                        break;
                    case "select":
                        btnHanhDong.Text = "Chọn";
                        break;
                    case "unselect":
                        btnHanhDong.Text = "Hủy chọn";
                        break;
                    case "delete":
                        btnHanhDong.Text = "Xóa";
                        break;
                }
            }
        }

        private void btnHanhDong_Click(object sender, EventArgs e)
        {
            if(mode == "view")
            {
                //invoke:
                // thấy thì trả lời ko thì im mồm
                XemChiTiet?.Invoke(this, EventArgs.Empty);
            }
            else if(mode == "select")
            {
                ChonSanPham?.Invoke(this, EventArgs.Empty);
            }
            else if (mode == "unselect")
            {
                HuyChon?.Invoke(this, EventArgs.Empty);
            }
            else if(mode == "delete")
            {
                XoaSanPham?.Invoke(this, EventArgs.Empty);
            }
        }


      



    }
}
