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
        public mainKH()
        {
            InitializeComponent();
        }

        void LayDuLieu()
        {
            KhachHang kh = new KhachHang();
            DonHang dh = new DonHang();

            tabKhachHang.Controls.Add(kh);
            tabDonHang.Controls.Add(dh);

        }

        private void mainKH_Load(object sender, EventArgs e)
        {
            LayDuLieu();
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
