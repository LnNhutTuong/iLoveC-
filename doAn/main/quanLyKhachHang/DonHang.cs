using doAn.popUp.quanLyKhachHang.DonHang;
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
    public partial class DonHang : UserControl
    {
        string maDH { get; set; }

        private BindingSource data = new BindingSource();
        MyDataTable dataTable = new MyDataTable();
        public DonHang()
        {
            InitializeComponent();
            dataTable.OpenConnection();
        }

        public void LayDuLieu()
        {
            dataTable.Clear();

            //dataGridView
            dataTable.OpenConnection();

            //Dat lon ten r
            string donHangSql = @"SELECT *
                                  FROM DonHang";
            SqlCommand donHangCmd = new SqlCommand(donHangSql);
            dataTable.Fill(donHangCmd);




            //gan du lieu vao nguon
            data.DataSource = dataTable;

            //gan nguon du lieu vao bang
            dataGridView.DataSource = data;


            //foreach (DataTable col in dataTable.Columns)
            //{
            //    Console.WriteLine(col.Columns.Count);
            //}

        }

        private void DonHang_Load(object sender, EventArgs e)
        {
            dataGridView.AutoGenerateColumns = false;
            LayDuLieu();
        }

        private void dataGridView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dataGridView.Columns[e.ColumnIndex].Name == "TrangThai" && e.Value != null)
            {
                object cell = dataGridView.Rows[e.RowIndex].Cells["TrangThai"].Value;

                int trangThai = cell is DBNull ? 0 : Convert.ToInt32(cell);

                if (trangThai == 0)
                {
                    e.Value = "Chuẩn bị";
                }
                else if (trangThai == 1)
                {
                    e.Value = "Đang giao";
                }
                else if (trangThai == 2)
                {
                    e.Value = "Giao thành công";
                }
                else if (trangThai == 3)
                {
                    e.Value = "Giao thất bại";
                }
                else
                {
                    e.Value = "Hàng hoàn";
                }

                e.FormattingApplied = true;
            }

            //if (dataGridView.Columns[e.ColumnIndex].Name == "GhiChu")
            //{
            //    object ghiChu = dataGridView.Rows[e.RowIndex].Cells["GhiChu"].Value;

            //    if (string.IsNullOrWhiteSpace(ghiChu.ToString()))
            //    {
            //        e.Value = "Không có ghi chú :)!";
            //        e.FormattingApplied = true;
            //    }
            //    else
            //    {
            //        e.Value = ghiChu.ToString();
            //        e.FormattingApplied = true;
            //    }
            //}
        }

      

        private void dataGridView_CellClick_1(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                if (dataGridView.Columns[e.ColumnIndex].Name == "ChiTietDonHang")
                {
                    string maDH = dataGridView.Rows[e.RowIndex].Cells["MaDonHang"].Value.ToString();

                    ChiTietDonHang ctdh = new ChiTietDonHang(maDH);
                    ctdh.ShowDialog();
                }
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {

        }

        private void btnTaiLai_Click(object sender, EventArgs e)
        {
            DonHang_Load(sender, e);
        }
    }
}

