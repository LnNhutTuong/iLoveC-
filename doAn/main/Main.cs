using doAn.main.quanLyKhachHang;
using doAn.main.quanLySanPham;
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
        public string LGmaNV { get;  set; }

        public event EventHandler DangNhap;
        public event EventHandler ThongTinNhanVien;
        //public event EventHandler DangXuat;


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
                        mnuDangNhap.Text = "Đăng nhập";
                        break;
                    case "logined":
                        break;
                }
            }
        }


        Login login = null;
        ChangePass changepass = null;

        QlNhanVien qlNhanVien = null;

        QlSanPham qlSanPham = null;
        BaoCaoSanPham bcSanPham = null;

        DonHang donHang = null;
        KhachHang khachHang = null;

        public Main()
        {
            Flash flash = new Flash();
            flash.ShowDialog();
            InitializeComponent();

            this.mode = "login";
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
                    login.Activate();
                }
            };
        }

        void ChuaDangNhap()
        {
            mnuDangNhap.Enabled = true;

            mnuQuanLy.Enabled = false;
            mnuThongKe.Enabled = false;

            mnuDoiMatKhau.Visible = false;
            mnuDangXuat.Visible = false;

            mnuNhanVien.Visible = false ;
            mnuSanPham.Visible = false;
            mnuKhachHang.Visible = false;
            mnuDonHang.Visible = false;
        }


        void admin()
        {
            mnuQuanLy.Enabled = true;
            mnuThongKe.Enabled = true;

            mnuNhanVien.Visible = true;
            mnuSanPham.Visible = true;
            mnuKhachHang.Visible = true;
            mnuDonHang.Visible = true;

            mnuBcKh.Visible = true;
            mnuBcSP.Visible = true;

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
            mnuBcSP.Visible = true;

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

            mnuThongKe.Visible = false;
            mnuBcKh.Visible = false;
            mnuBcSP.Visible = false;

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
            mnuBcSP.Visible = false;

            mnuDoiMatKhau.Visible = true;
            mnuDangXuat.Visible = true;
        }

        public void setTrangThai(string MaNV,string TenNV)
        {
            this.mode = "logined";
            mnuDangNhap.Visible = false;
            this.ThongTinNhanVien += (s, e) =>
            {
                lblTrangThai.Text = "Nhân viên: "+MaNV + " - " + TenNV;
            };

            this.ThongTinNhanVien?.Invoke(this, EventArgs.Empty);
        }

        public void PhanQuyen(string quyen)
        {
            if (quyen.StartsWith("AD")) admin();
            else if (quyen.StartsWith("SP")) qlSP();
            else if (quyen.StartsWith("NS")) qlNV();
            else if (quyen.StartsWith("KH")) qlKH();
        }
       
        private void Main_Load(object sender, EventArgs e)
        {
            ChuaDangNhap();
        }
    
        private void mnuThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void mnuDangNhap_Click(object sender, EventArgs e)
        {
            if (mode == "login")
            {
                DangNhap?.Invoke(this, EventArgs.Empty);
            }
            else if (mode == "logined")
            {
                ThongTinNhanVien?.Invoke(this, EventArgs.Empty);
            }
        }

        private void mnuDangXuat_Click(object sender, EventArgs e)
        {
            this.mode = "login";
            mnuDangNhap.Visible = true;
            lblTrangThai.Text = "Chưa đăng nhập";
            ChuaDangNhap();
        }

        private void mnuDoiMatKhau_Click(object sender, EventArgs e)
        {
            //Console.WriteLine(LGmaNV);
            if (changepass == null || changepass.IsDisposed)
            {
                changepass = new ChangePass(LGmaNV);
                changepass.MdiParent = this;
                changepass.Show();
            }
            else
            {
                changepass.Activate();
            }
        }

        private void mnuNhanVien_Click(object sender, EventArgs e)
        {
            if (qlNhanVien == null || qlNhanVien.IsDisposed)
            {
                qlNhanVien = new QlNhanVien();
                qlNhanVien.MdiParent = this;
                Dock = DockStyle.Fill;
                qlNhanVien.Show();
            }
            else
            {
                qlNhanVien.Activate();
            }
        }

        private void mnuSanPham_Click(object sender, EventArgs e)
        {
            if (qlSanPham == null || qlSanPham.IsDisposed)
            {
                qlSanPham = new QlSanPham();
                qlSanPham.MdiParent = this;
                Dock = DockStyle.Fill;
                qlSanPham.Show();
            }
            else
            {
                qlSanPham.Activate();
            }
        }

        private void mnuBcSP_Click(object sender, EventArgs e)
        {
            if (bcSanPham == null || bcSanPham.IsDisposed)
            {
                bcSanPham = new BaoCaoSanPham();
                bcSanPham.MdiParent = this;
                Dock = DockStyle.Fill;
                bcSanPham.Show();
            }
            else
            {
                bcSanPham.Activate();
            }
        }

        private void mnuKhachHang_Click(object sender, EventArgs e)
        {
            if (khachHang == null || khachHang.IsDisposed)
            {
                khachHang = new KhachHang();
                khachHang.MdiParent = this;
                Dock = DockStyle.Fill;
                khachHang.Show();
            }
            else
            {
                khachHang.Activate();
            }
        }

        private void mnuDonHang_Click(object sender, EventArgs e)
        {
            if (donHang == null || donHang.IsDisposed)
            {
                donHang = new DonHang();
                donHang.MdiParent = this;
                Dock = DockStyle.Fill;
                donHang.Show();
            }
            else
            {
                donHang.Activate();
            }
        }

       
    }
}
