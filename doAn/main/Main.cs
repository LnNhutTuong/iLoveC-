using doAn.main.quanLyKhachHang;
using doAn.quanLyKhachHang;
using doAn.quanLyNguoIDung;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace doAn.main
{
    public partial class Main : Form
    {

        public event EventHandler DangNhap;

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
                    case "login":
                        mnuThongTin.Text = "Đăng nhập";
                        break;
                }
            }
        }

        Login login = null;
        ChangePass change = null;

        QlNhanVien qlNhanVien = null;
        QlSanPham qlSanPham = null;

        DonHang donHang = null;
        KhachHang khachHang = null;

        public Main()
        {        
            InitializeComponent();
        }

        void ChuaDangNhap()
        {
            //mnuThongTin.Text = "Đăng nhập";

            mnuDoiMatKhau.Visible = false;
            mnuDangXuat.Visible = false;

            this.mode = "login";
            this.DangNhap += (s, e) =>
            {
                mnuThongTin.Text = "Đăng nhập";
                Login lg = new Login();
                lg.ShowDialog();
                
            };
        
            mnuQuanLy.Enabled = false;
            mnuThongKe.Enabled = false;
        }

       

        void qlSP()
        {

            mnuNhanVien.Visible = false;           

            mnuKhachHang.Visible = false;
            mnuDonHang.Visible = false;

            mnuBcKh.Visible = false;
        }

        void qlNV()
        {            
            mnuSanPham.Visible = false;

            mnuThongKe.Visible = false;
            
        }

        void qlKH()
        {          
            mnuNhanVien.Visible = false;
            mnuSanPham.Visible = false;
            mnuKhoSanPham.Visible = false;

            mnuBcSP.Visible = false;
            mnuBcK.Visible = false;
        }

        public void setTrangThai(string MaNV,string TenNV)
        {
            lblTrangThai.Text = "Nhân viên: " + TenNV;
        }

        public void PhanQuyen(string quyen)
        {
            if (quyen.StartsWith("SP")) qlSP();
            else if (quyen.StartsWith("NV")) qlNV();
            else if (quyen.StartsWith("KH")) qlKH();
        }

        

        private void Main_Load(object sender, EventArgs e)
        {
            ChuaDangNhap();
        }

        private void mnuThongTin_Click(object sender, EventArgs e)
        {
            if(mode == "login")
            {
                DangNhap?.Invoke(this, EventArgs.Empty);
            }           
        }      
    }
}
