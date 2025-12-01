using doAn.quanLySanPham;
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

namespace doAn
{
    public partial class mainSP : Form
    {
        public DanhSachSP danhSachSP;

        public string TenNhanVien { get; set; }


        public mainSP()
        {
            InitializeComponent();
            
        }

       private void LayDuLieu()
        {

            uscDanhMuc danhMuc = new uscDanhMuc();
            uscThuongHieu thuongHieu = new uscThuongHieu();
            danhSachSP = new DanhSachSP(); //MOST difficult 
                                           // tai vi thang nay nhieu con qua nen phai gan
                                           // vao bien toan cuc (hieu don gian la vay @@)

            tabDanhMuc.Controls.Add(danhMuc);
            tabThuongHieu.Controls.Add(thuongHieu);
            tabSanPham.Controls.Add(danhSachSP);
        }

        private void mainSP_Load(object sender, EventArgs e)
        {
            txtNhanVien.Text = "Nhân viên: " + TenNhanVien;
            LayDuLieu();
        }

        private void tabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl.SelectedTab == tabDanhMuc)
            {
                lblTieuDe.Text = "Quản lý Danh Mục";
            }
            else if (tabControl.SelectedTab == tabThuongHieu)
            {
                lblTieuDe.Text = "Quản lý Thương Hiệu";
            }
            else if (tabControl.SelectedTab == tabSanPham)
            {
                lblTieuDe.Text = "Quản lý Sản phẩm";
            }

        }

      

        private void btnThoat_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnDoiMatKhau_Click(object sender, EventArgs e)
        {

        }

      
    }
}
