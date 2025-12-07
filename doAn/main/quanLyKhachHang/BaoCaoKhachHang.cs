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

namespace doAn.main.quanLyKhachHang
{
    public partial class BaoCaoKhachHang : Form
    {
        public BaoCaoKhachHang()
        {
            InitializeComponent();
        }

        private void BaoCaoKhachHang_Load(object sender, EventArgs e)
        {
            MyDataTable khachHang = new MyDataTable();
            khachHang.OpenConnection();
            SqlCommand khachHangCmd = new SqlCommand("SELECT Count(MaKhachHang) as SoLuong,PhanCap " +
                "                                     FROM KhachHang GROUP BY PhanCap");
            khachHang.Fill(khachHangCmd);


            //Console.WriteLine(soLuongDonHang);
            //string rank = khachHang.Rows[0]["PhanCap"].ToString();

            int none = 0;
            int dong = 0;
            int bac = 0;
            int vang = 0;
            int kimCuong = 0;

            foreach (DataRow r in khachHang.Rows)
            {
                string rank = r["PhanCap"].ToString();
                int count = Convert.ToInt32(r["SoLuong"]);

                if (rank == "0") none = count;
                else if (rank == "1") dong = count;
                else if (rank == "1") bac = count;
                else if (rank == "2") vang = count;
                else if (rank == "3") kimCuong = count;

                lblTong.Text = "Tổng số khách hàng: " + count;
            }



            chart1.Series.Clear();
            Series s = new Series("Tổng số lượng khách hàng/rank");
            //s.ChartType = SeriesChartType.Doughnut;
            s.IsValueShownAsLabel = true;
            s.LabelFormat = "#,##0 Khách hàng";
            int n0ne = s.Points.AddXY("Chưa có loại", none);
            s.Points[n0ne].Color = Color.Yellow;
            int bronze = s.Points.AddXY("Đồng", dong);
            s.Points[bronze].Color = Color.Brown;
            int silver = s.Points.AddXY("Bạc", bac);
            s.Points[silver].Color = Color.Silver;
            int gold = s.Points.AddXY("Vàng", vang);
            s.Points[gold].Color = Color.Gold;
            int diamond = s.Points.AddXY("Kim cương", kimCuong);
            s.Points[diamond].Color = Color.Blue;


            chart1.Series.Add(s);

        }
    }
}
