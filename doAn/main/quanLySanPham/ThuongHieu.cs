using doAn.popUp.thuongHieu;
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

namespace doAn.quanLySanPham
{
    public partial class uscThuongHieu : UserControl
    {
        private BindingSource data = new BindingSource();
        MyDataTable dataTable = new MyDataTable();

        public uscThuongHieu()
        {
            InitializeComponent();
            dataTable.OpenConnection();
        }

        public void LayDuLieu()
        {
            dataTable.Clear();

            dataTable.OpenConnection();

            string ThuongHieuSql = @"SELECT 
                                        t.*,
                                        COUNT(s.MaSanPham) AS SoLuong
                                    FROM ThuongHieu t
                                    LEFT JOIN SanPham s ON t.MaThuongHieu = s.MaThuongHieu
                                    GROUP BY t.MaThuongHieu, t.TenThuongHieu";
            SqlCommand ThuongHieuCmd = new SqlCommand(ThuongHieuSql);
            dataTable.Fill(ThuongHieuCmd);

            //gan du lieu vao nguon
            data.DataSource = dataTable;

            //gan nguon du lieu vao bang
            dataGridView .DataSource = data;

            //foreach (DataTable col in dataTable.Columns)
            //{
            //    Console.WriteLine(col.Columns.Count);
            //}


        }


        private void btnThem_Click(object sender, EventArgs e)
        {
            var puThem = new Them(data);
            puThem.ShowDialog();
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (data.Current == null)
            {
                MessageBox.Show("Vui lòng chọn dòng cần xóa!");
            }
            else 
            {
                if (MessageBox.Show("Bạn có chắc muốn xóa?", "Xác nhận", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    data.RemoveCurrent();
                }
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (data.Current == null)
            {
                MessageBox.Show("Vui lòng chọn dòng cần sửa!");
            }
            else
            {
                var puSua = new Them(data);
                puSua.ShowDialog();
            }
              
        }

        private void btnTaiLai_Click(object sender, EventArgs e)
        {
            uscThuongHieu_Load(sender, e);
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
                    SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM ThuongHieu", conn);

                    //CLASS DINH CAO PHONG DO HOYASH
                    //Sau khi adapter kiem tra xong thi Class nay se nhan ket qua
                    //Va bat dau thuc hien cac lenh INSERT, DELETE, UPDATE, tuong ung theo
                    //nhung du lieu da duoc them, xoa, sua
                    SqlCommandBuilder builder = new SqlCommandBuilder(adapter);

                    //updata tu bang len database
                    adapter.Update(dataTable);
                }

                MessageBox.Show("Lưu dữ liệu thành công!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lưu: " + ex.Message);
            }
        }

        private void uscThuongHieu_Load(object sender, EventArgs e)
        {
            LayDuLieu();
        }
    }
}
