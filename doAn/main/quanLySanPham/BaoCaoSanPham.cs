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
using System.Windows.Forms.DataVisualization.Charting;

namespace doAn.main.quanLySanPham
{
    public partial class BaoCaoSanPham : Form
    {
        MyDataTable myData = new MyDataTable();

        public BaoCaoSanPham()
        {
            InitializeComponent();
            myData.OpenConnection();
        }

        private void BaoCaoSanPham_Load(object sender, EventArgs e)
        {     
            MyDataTable tongSanPham = new MyDataTable();
            tongSanPham.OpenConnection();
            //Lay ra duoc so luong sp
            SqlCommand tongSanPhamCmd = new SqlCommand(@"SELECT COUNT(*) FROM SanPham");
            tongSanPham.Fill(tongSanPhamCmd);
            int tong = Convert.ToInt32(tongSanPham.Rows[0][0]);

            MyDataTable spCoDon = new MyDataTable();
            spCoDon.OpenConnection();
            //Lay ra duoc so luong sp
            SqlCommand spCoDonCmd = new SqlCommand(@"SELECT COUNT(DISTINCT sp.MaSanPham) 
                                                         FROM SanPham sp
                                                         JOIN ChiTietDonHang ctdh ON ctdh.MaSanPham = sp.MaSanPham");
            spCoDon.Fill(spCoDonCmd);
            int daban = Convert.ToInt32(spCoDon.Rows[0][0]);

            int tonkho = tong - daban;
            lblTong.Text ="Tổng số sản phẩm: "+ tong.ToString();

            chart1.Series.Clear();
            Series s = new Series("Tình trạng sản phẩm");
            s.ChartType = SeriesChartType.Doughnut;
            s.IsValueShownAsLabel = true;
            s.LabelFormat = "#,##0 Sản phẩm";
            s.Points.AddXY("Đã bán", daban);
            s.Points.AddXY("Tồn kho", tonkho);

            chart1.Series.Add(s);

        }
    }
}
