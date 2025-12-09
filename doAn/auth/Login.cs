using doAn.main;
using doAn.quanLyKhachHang;
using doAn.quanLyNguoIDung;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BC = BCrypt.Net.BCrypt;
namespace doAn
{
    public partial class Login : Form
    {
        public Login()
        {         
            InitializeComponent();
            txtMatKhau.UseSystemPasswordChar = true;
        }

        private void txtMatKhau_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnDangNhap_Click(sender, e);
            }
        }

        private void btnDangNhap_Click(object sender, EventArgs e)
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

                SqlCommand cmd = new SqlCommand("SELECT MaNhanVien, MatKhau, TenNhanVien FROM NhanVien WHERE MaNhanVien = @MaNhanVien");

                cmd.Parameters.Add("@MaNhanVien", SqlDbType.NVarChar, 5).Value = txtMaNhanVien.Text.ToUpper().Trim();

                dataTable.Fill(cmd);

                if (dataTable.Rows.Count == 0)
                {
                    MessageBox.Show("Sai mã nhân viên!");
                    txtMaNhanVien.Focus();
                    return;
                }

                string matKhauHash = dataTable.Rows[0]["MatKhau"].ToString();

                if (!BC.Verify(txtMatKhau.Text, matKhauHash))
                {
                    MessageBox.Show("Sai mật khẩu!");
                    txtMatKhau.Focus();
                    return;
                }

                if (txtMatKhau.Text.Trim() == "123456")
                {
                    MessageBox.Show("Bạn đang để mật khẩu mặc định!!!\n Hãy đổi mật khẩu để tăng tính bảo mật!", "", MessageBoxButtons.OK);
                }

                //get data
                string MaNV = dataTable.Rows[0]["MaNhanVien"].ToString().ToUpper();
                string TenNV = dataTable.Rows[0]["TenNhanVien"].ToString();

                //get 2 ky tu dau
                string role = MaNV.Substring(0, 2).ToUpper();

                //to main ->>>>>>>>>>>>>
                Main main = (Main)Application.OpenForms["Main"];
                main.LGmaNV = MaNV;
                main.setTrangThai(MaNV, TenNV);
                main.PhanQuyen(role);

                this.Close();

            }
        }

        private void btnAnHienMatKhau_Click(object sender, EventArgs e)
        {
            if (txtMatKhau.UseSystemPasswordChar)
            {
                txtMatKhau.UseSystemPasswordChar = false;
                btnAnHienMatKhau.BackgroundImage = Properties.Resources.visible;
            }
            else
            {
                txtMatKhau.UseSystemPasswordChar = true;
                btnAnHienMatKhau.BackgroundImage = Properties.Resources.hide;
            }
        }
    }
}
