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
            

            MyDataTable spCoDon = new MyDataTable();
            spCoDon.OpenConnection();
            //Lay ra duoc so luong sp
            SqlCommand spCoDonCmd = new SqlCommand(@"SELECT COUNT(sp.MaSanPham) 
                                                     FROM SanPham sp
                                                     JOIN ChiTietDonHang ctdh ON ctdh.MaSanPham = sp.MaSanPham");
            spCoDon.Fill(spCoDonCmd);

            List<string> spCoDonQua = new List<string>();

            foreach (DataRow codon in spCoDon.Rows)
            {
                spCoDonQua.Add(codon.ToString());
            }

            Console.WriteLine(spCoDonQua);

            //chart1.Series.Clear();

            //Series s = new Series("Số lượng sản phẩm được bán theo thương hiệu");
            //s.ChartType = SeriesChartType.Pie;
            //s.IsValueShownAsLabel = true;                      

            //foreach (DataRow row in thuongHieu.Rows)
            //{
            //    int soLuong = Convert.ToInt32(row["SoLuongSanPham"]);

            //    if (soLuong > 1)
            //    {
            //        string tenTH = row["TenThuongHieu"].ToString();
            //        int pointIndex = s.Points.AddXY(tenTH, soLuong);
            //        s.Points[pointIndex].LegendText = tenTH;
            //    }
            //}

            //chart1.Series.Add(s);

        }
    }
}
