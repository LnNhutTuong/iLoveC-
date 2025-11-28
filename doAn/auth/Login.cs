using doAn.quanLyNguoIDung;
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
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            if (txtMaNhanVien.Text.Trim() == "")
            {
                MessageBox.Show("Mã nhân viên không được bỏ trống!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtMaNhanVien.Focus();
            }
            else if (txtMatKhau.Text.Trim() == "")
            {
                MessageBox.Show("Mật khẩu không được bỏ trống!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtMatKhau.Focus();
            }
            else
            {
                MyDataTable dataTable = new MyDataTable();
                dataTable.OpenConnection();

                SqlCommand cmd = new SqlCommand("SELECT MaNhanVien, MatKhau FROM NhanVien WHERE MaNhanVien = @MaNhanVien AND MatKhau = @MatKhau");
                cmd.Parameters.Add("@MaNhanVien", SqlDbType.NVarChar, 5).Value = txtMaNhanVien.Text;
                cmd.Parameters.Add("@MatKhau", SqlDbType.NVarChar, 100).Value = txtMatKhau.Text;
                dataTable.Fill(cmd);

                if (dataTable.Rows.Count == 0)
                {
                    MessageBox.Show("Tên đăng nhập hoặc mật khẩu không chính xác!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtMaNhanVien.Focus();
                    return;
                }

                if (txtMaNhanVien.Text.ToUpper().Substring(0, 2) == "SP")
                {
                    mainSP sp = new mainSP();
                    sp.TenNhanVien = dataTable.Rows[0]["TenNhanVien"].ToString();

                    if (txtMatKhau.Text == "123456")
                    {
                        MessageBox.Show("Đang để mật khẩu mặc định \n Đổi liền đi fuck you","Thông báo!", MessageBoxButtons.OK);
                    }
                    this.Hide();
                    sp.ShowDialog();                
                }

                else if (txtMaNhanVien.Text.ToUpper().Substring(0, 2) == "NS")
                {
                    mainNV nv = new mainNV();
                    //nv.TenNhanVien = dataTable.Rows[0]["TenNhanVien"].ToString();

                    if (txtMatKhau.Text == "123456")
                    {
                        MessageBox.Show("Đang để mật khẩu mặc định \n Đổi liền đi fuck you", "Thông báo!", MessageBoxButtons.OK);
                    }
                    this.Hide();
                    nv.ShowDialog();          
                }            
            }

        }

        private void txtMatKhau_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button1_Click(sender, e);
            }
        }
    }
}
