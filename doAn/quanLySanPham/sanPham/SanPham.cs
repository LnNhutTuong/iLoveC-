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

namespace doAn.quanLySanPham
{
    public partial class SanPham : UserControl
    {

        public string MaSanPham { get; set; }

        public SanPham()
        {
            InitializeComponent();
        }

        public void setData (string TenSanPham)
        {
            lblTenSanPham.Text = TenSanPham;
        }

        private void btnChiTiet_Click(object sender, EventArgs e)
        {
            var ct = new ChiTietSanPham(MaSanPham);
            ct.ShowDialog();
        }
    }
}
