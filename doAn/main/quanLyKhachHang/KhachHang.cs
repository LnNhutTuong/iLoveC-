using doAn.popUp.quanLyKhachHang.khachHang;
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

namespace doAn.quanLyKhachHang
{
    public partial class KhachHang : UserControl
    {
        private BindingSource data = new BindingSource();
        MyDataTable dataTable = new MyDataTable();

        public KhachHang()
        {
            InitializeComponent();
            dataTable.OpenConnection();
        }

        public void LayDuLieu()
        {
            dataTable.Clear();

            //dataGridView
            dataTable.OpenConnection();

            string khachHangSql = @" SELECT k.*,                                        
                                          ISNULL(COUNT(h.MaDonHang), 0) AS SoLuong
                                    FROM KhachHang k
                                    LEFT JOIN DonHang h ON k.MaKhachHang = h.MaKhachHang
                                    GROUP BY k.MaKhachHang,
                                             k.TenKhachHang,
                                             k.DiaChi,
                                             k.Sdt,
                                             k.Email,
                                             k.PhanCap;   ";
            SqlCommand khachHangCmd = new SqlCommand(khachHangSql);
            dataTable.Fill(khachHangCmd);




            //gan du lieu vao nguon
            data.DataSource = dataTable;

            //gan nguon du lieu vao bang
            dataGridView.DataSource = data;


            //foreach (DataTable col in dataTable.Columns)
            //{
            //    Console.WriteLine(col.Columns.Count);
            //}

        }

        private void dataGridView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {

            //if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

            if (dataGridView.Columns[e.ColumnIndex].Name == "PhanCap" && e.Value != null)
            {
                object cell = dataGridView.Rows[e.RowIndex].Cells["SoLuong"].Value;

                int soDon = cell is DBNull ? 0 : Convert.ToInt32(cell);

                if (soDon <= 5)
                {
                    e.Value = "Đồng";
                }
                else if (soDon <= 10)
                {
                    e.Value = "Bạc";
                }
                else if (soDon <= 40)
                {
                    e.Value = "Vàng";
                }
                else
                {
                    e.Value = "Kim cương";
                }

                e.FormattingApplied = true;
            }
        }



        private void KhachHang_Load(object sender, EventArgs e)
        {
            LayDuLieu();
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            Them puThem = new Them(data);
            puThem.ShowDialog();
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            try
            {
                //Phai ket thuc het tat ca chinh sua thi moi luu duoc
                data.EndEdit();

                //lay chuoi ket noi de ket noi vao database
                //Chuoi ket noi dung de thuc hien viec lay cau truc cua bang trong database
                string connString = dataTable.ConnectionString();

                //dung using vi khi su dung xong nhung thu trong day thi xoa lien
                //using: luu tren ram -> xai xong bien mat! like
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    //thuc hien ket noi
                    conn.Open();

                    //Sau khi ket noi duoc thi class nay se thuc hien
                    //chay lenh SELECT.
                    //QUAN TRONG!!!!!!: La cai lenh select nay ko phai
                    //la lay du lieu don gian, ma tro thanh lenh de "kiem tra"
                    //"Kiem tra" su thay doi = them, xoa, sua.
                    SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM KhachHang", conn);

                    //CLASS DINH CAO PHONG DO HOYASH
                    //Sau khi adapter kiem tra xong thi Class nay se nhan ket qua
                    //Va bat dau thuc hien cac lenh INSERT, DELETE, UPDATE, tuong ung theo
                    //nhung du lieu da duoc them, xoa, sua
                    SqlCommandBuilder builder = new SqlCommandBuilder(adapter);

                    //updata tu bang len database
                    adapter.Update(dataTable);
                }
                KhachHang_Load(sender, e);
                MessageBox.Show("Lưu dữ liệu thành công!");

            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lưu: " + ex.Message);
            }
        }

        private void btnTaiLai_Click(object sender, EventArgs e)
        {
            LayDuLieu();

        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (data.Current == null)
            {
                MessageBox.Show("Vui lòng chọn dòng cần xóa!");
            }
            if (data.Current != null)
            {

                DataRowView row = (DataRowView)data.Current;
                string maKH = row["MaKhachHang"].ToString();

                MyDataTable donhang = new MyDataTable();
                donhang.OpenConnection();
                string sql = @"SELECT Count(*) FROM DonHang WHERE MaKhachHang = @MaKhachHang";
                SqlCommand cmd = new SqlCommand(sql);
                cmd.Parameters.AddWithValue("@MaKhachHang", maKH);
                donhang.Fill(cmd);

                //ExecuteScalar: tra ve thang dau tien trong sql
                int soDon = Convert.ToInt32(cmd.ExecuteScalar());

                Console.WriteLine(soDon);

               
                    if (soDon > 0)
                    {
                        if (MessageBox.Show("Bạn có chắc muốn xóa?", "Xác nhận", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                        }
                    }
                    else
                    {
                        MessageBox.Show("Khách hàng này đang có đơn hàng! \n Không thể xóa!!");
                    data.RemoveCurrent();

                    }


            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            Sua puSua = new Sua(data);
            puSua.ShowDialog();
        }
    }
}
