using doAn.popUp.quanlySanPham.sanPham;
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

namespace doAn.main.quanLyKhachHang
{
    public partial class ThongKe : UserControl
    {
        MyDataTable myData = new MyDataTable();
        public ThongKe()
        {
            InitializeComponent();
            myData.OpenConnection();
        }

        void LayDuLieu()
        {
            flowLayoutPanel.Controls.Clear();
            MyDataTable sanPham = new MyDataTable();
            sanPham.OpenConnection();
            string sanPhamSql = "SELECT * FROM SanPham";
            SqlCommand sanPhamCmd = new SqlCommand(sanPhamSql);
            sanPham.Fill(sanPhamCmd);
            foreach (DataRow row in sanPham.Rows)
            {
                SanPham sp = new SanPham();
                sp.MaSanPham = row["MaSanPham"].ToString().ToUpper();
                sp.AnhDaiDien = row["AnhDaiDien"].ToString();
                sp._mode = "view";
                sp.setData(row["TenSanPham"].ToString().ToUpper(), row["AnhDaiDien"].ToString());
                sp.triGia = Convert.ToInt32(row["TriGia"]);
                sp.XemChiTiet += (sS, eS) =>
                {
                    ChiTietSanPham ctsp = new ChiTietSanPham(sp.MaSanPham);
                    ctsp.ShowDialog();
                };
                flowLayoutPanel.Controls.Add(sp);
            }
        }

        private void ThongKe_Load(object sender, EventArgs e)
        {
            LayDuLieu();
        }
    }
}
