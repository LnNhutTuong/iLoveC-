using doAn.main.quanLyKhachHang;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace doAn.quanLyKhachHang
{
    public partial class mainKH : Form
    {
        public string MaNhanVien { get; set; }
        public string TenNhanVien { get; set; }

        public mainKH(bool fromAdmin)
        {
            InitializeComponent();

            if (fromAdmin)
            {
                btnThoat.Text = "Quay lại";

                btnThoat.Click -= btnThoat_Click;

                btnThoat.Click += (s, e) => this.Close();

                btnDoiMatKhau.Visible = false;
            }

        }

        void LayDuLieu()
        {

            KhachHang kh = new KhachHang();
            DonHang dh = new DonHang();
            ThongKe tk = new ThongKe();

            tabKhachHang.Controls.Add(kh);
            tabDonHang.Controls.Add(dh);
            tabThongKe.Controls.Add(tk);
        }

        private void mainKH_Load(object sender, EventArgs e)
        {

            txtNhanVien.Text = "Nhân viên: " + TenNhanVien;
              LayDuLieu();
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnDoiMatKhau_Click(object sender, EventArgs e)
        {
            ChangePass cp = new ChangePass(MaNhanVien);            
            cp.ShowDialog();
        }

        private void tabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl.SelectedTab == tabKhachHang)
            {
                lblTieuDe.Text = "Quản lý Khách Hàng";
            }
            else if (tabControl.SelectedTab == tabDonHang)
            {
                lblTieuDe.Text = "Quản lý Đơn Hàng";
            }
        }
    }
}
