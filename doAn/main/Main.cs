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
        public event EventHandler ThongTinNhanVien;

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
                    case "logined":
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
            Flash flash = new Flash();
            flash.ShowDialog();
            InitializeComponent();

            this.mode = "login";
            mnuThongTin.Text = "Đăng nhập";
            this.DangNhap += (s, e) =>
            {
                if (login == null || login.IsDisposed)
                {
                    login = new Login();
                    login.MdiParent = this;
                    login.Show();
                }
                else
                {
                    login.StartPosition = FormStartPosition.CenterParent;
                    login.Activate();
                }
            };
        }

        void ChuaDangNhap()
        {
            mnuThongTin.Enabled = true;

            mnuQuanLy.Enabled = false;
            mnuThongKe.Enabled = false;

            mnuDoiMatKhau.Visible = false;
            mnuDangXuat.Visible = false;
        }


        void admin()
        {
            mnuQuanLy.Enabled = true;
            mnuThongKe.Enabled = true;

            mnuNhanVien.Visible = true;
            mnuSanPham.Visible = true;
            mnuKhachHang.Visible = true;
            mnuDonHang.Visible = true;
            mnuKhoSanPham.Visible = true;

            mnuBcKh.Visible = true;
            mnuBcSP.Visible = true;
            mnuBcK.Visible = true;

            mnuDoiMatKhau.Visible = true;
            mnuDangXuat.Visible = true;
        }

        void qlSP()
        {
            mnuQuanLy.Enabled = true;
            mnuThongKe.Enabled = true;

            mnuNhanVien.Visible = false;
            mnuKhachHang.Visible = false;
            mnuDonHang.Visible = false;

            mnuBcKh.Visible = false;

            mnuSanPham.Visible = true;
            mnuKhoSanPham.Visible = true;
            mnuBcSP.Visible = true;
            mnuBcK.Visible = true;

            mnuDoiMatKhau.Visible = true;
            mnuDangXuat.Visible = true;
        }

        void qlNV()
        {
            mnuQuanLy.Enabled = true;

            mnuNhanVien.Visible = true;

            mnuSanPham.Visible = false;
            mnuKhachHang.Visible = false;
            mnuDonHang.Visible = false;
            mnuKhoSanPham.Visible = false;

            mnuThongKe.Visible = false;
            mnuBcKh.Visible = false;
            mnuBcSP.Visible = false;
            mnuBcK.Visible = false;

            mnuDoiMatKhau.Visible = true;
            mnuDangXuat.Visible = true;
        }

        void qlKH()
        {
            mnuQuanLy.Enabled = true;
            mnuThongKe.Enabled = true;

            mnuKhachHang.Visible = true;
            mnuDonHang.Visible = true;
            mnuBcKh.Visible = true;

            mnuNhanVien.Visible = false;
            mnuSanPham.Visible = false;
            mnuKhoSanPham.Visible = false;
            mnuBcSP.Visible = false;
            mnuBcK.Visible = false;

            mnuDoiMatKhau.Visible = true;
            mnuDangXuat.Visible = true;
        }

        public void setTrangThai(string MaNV,string TenNV)
        {
            this.mode = "logined";

            this.ThongTinNhanVien += (s, e) =>
            {
                mnuThongTin.Text = MaNV + " - " + TenNV;
                lblTrangThai.Text = "Nhân viên: " + TenNV;
            };

            this.ThongTinNhanVien?.Invoke(this, EventArgs.Empty);
        }

        public void PhanQuyen(string quyen)
        {
            if (quyen.StartsWith("AD")) admin();
            else if (quyen.StartsWith("SP")) qlSP();
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
            else if(mode == "logined")
            {
                ThongTinNhanVien?.Invoke(this, EventArgs.Empty);
            }
        }

       
    }
}
