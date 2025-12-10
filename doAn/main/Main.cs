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
using BC = BCrypt.Net.BCrypt;

namespace doAn.main
{

    public partial class Main : Form
    {
        public string LGmaNV { get; set; }

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
                        btnDangNhap.Text = "Đăng nhập";
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
        BaoCaoKhachHang bcKhachHang = null;
        public Main()
        {
            Flash flash = new Flash();
            flash.ShowDialog();
            InitializeComponent();

            //string test = "123456";
            //string hash = BC.HashPassword(test);
            //Console.WriteLine(hash);

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
            btnDangNhap.Enabled = true;

            mnuQuanLy.Enabled = false;
            mnuThongKe.Enabled = false;

            mnuDoiMatKhau.Visible = false;
            mnuDangXuat.Visible = false;

            btnNhanVien.Visible = false;
            btnSanPham.Visible = false;
            btnKhachHang.Visible = false;
            btnDonHang.Visible = false;
        }


        void admin()
        {
            mnuQuanLy.Enabled = true;
            mnuThongKe.Enabled = true;

            btnNhanVien.Visible = true;
            btnSanPham.Visible = true;
            btnKhachHang.Visible = true;
            btnDonHang.Visible = true;
                
            mnuBcKh.Visible = true;
            mnuBcSP.Visible = true;

            mnuDoiMatKhau.Visible = true;
            mnuDangXuat.Visible = true;
        }

        void qlSP()
        {
            mnuQuanLy.Enabled = true;
            mnuThongKe.Enabled = true;

            btnNhanVien.Visible = false;
            btnKhachHang.Visible = false;
            btnDonHang.Visible = false;

            mnuBcKh.Visible = false;

            btnSanPham.Visible = true;
            mnuBcSP.Visible = true;

            mnuDoiMatKhau.Visible = true;
            mnuDangXuat.Visible = true;
        }

        void qlNV()
        {
            mnuQuanLy.Enabled = true;

            btnNhanVien.Visible = true;

            btnSanPham.Visible = false;
            btnKhachHang.Visible = false;
            btnDonHang.Visible = false;

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

            btnKhachHang.Visible = true;
            btnDonHang.Visible = true;
            mnuBcKh.Visible = true;

            btnNhanVien.Visible = false;
            btnSanPham.Visible = false;
            mnuBcSP.Visible = false;

            mnuDoiMatKhau.Visible = true;
            mnuDangXuat.Visible = true;
        }

        public void setTrangThai(string MaNV, string TenNV)
        {
            this.mode = "logined";
            btnDangNhap.Visible = false;
            this.ThongTinNhanVien += (s, e) =>
            {
                lblTrangThai.Text = "Nhân viên: " + MaNV + " - " + TenNV;
            };

            this.ThongTinNhanVien?.Invoke(this, EventArgs.Empty);
        }

        public void PhanQuyen(string quyen)
        {
            if (quyen.StartsWith("AD") || quyen.StartsWith("GD")) admin();
            else if (quyen.StartsWith("SP")) qlSP();
            else if (quyen.StartsWith("NS")) qlNV();
            else if (quyen.StartsWith("KH")) qlKH();
        }

        private void Main_Load(object sender, EventArgs e)
        {
            ChuaDangNhap();
        }

        private void btnDangNhap_Click_1(object sender, EventArgs e)
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
            btnDangNhap.Visible = true;
            lblTrangThai.Text = "Chưa đăng nhập";
            ChuaDangNhap();

            //đóng hết con
            foreach (Form f in this.MdiChildren)
            {
                f.Close();
            }

            Login login = new Login();
            login.Show();
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

        private void mnuBcKh_Click(object sender, EventArgs e)
        {
            if (bcKhachHang == null || bcKhachHang.IsDisposed)
            {
                bcKhachHang = new BaoCaoKhachHang();
                bcKhachHang.MdiParent = this;
                Dock = DockStyle.Fill;
                bcKhachHang.Show();
            }
            else
            {
                bcKhachHang.Activate();
            }
        }

        private void btnNhanVien_Click(object sender, EventArgs e)
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

        private void btnSanPham_Click(object sender, EventArgs e)
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

        private void btnKhachHang_Click(object sender, EventArgs e)
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

        private void btnDonHang_Click(object sender, EventArgs e)
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

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}