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
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace doAn
{
    public partial class Login : Form
    {
        string hoVaTen = "";
        public Login()
        {
            Flash flash = new Flash();
            flash.ShowDialog();
            InitializeComponent();
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

                SqlCommand cmd = new SqlCommand("SELECT MaNhanVien, MatKhau, TenNhanVien FROM NhanVien WHERE MaNhanVien = @MaNhanVien AND MatKhau = @MatKhau");
                cmd.Parameters.Add("@MaNhanVien", SqlDbType.NVarChar, 5).Value = txtMaNhanVien.Text;
                cmd.Parameters.Add("@MatKhau", SqlDbType.NVarChar, 100).Value = txtMatKhau.Text;
                dataTable.Fill(cmd);

                if (dataTable.Rows.Count == 0)
                {
                    MessageBox.Show("Sai tài khoản hoặc mật khẩu!");
                    txtMaNhanVien.Focus();
                    return;
                }

                string MaNV = dataTable.Rows[0]["MaNhanVien"].ToString();
                string TenNV = dataTable.Rows[0]["TenNhanVien"].ToString();

                string role = MaNV.Substring(0, 2).ToUpper();

                // gửi sang Main
                Main main = (Main)this.MdiParent;
                main.setTrangThai(MaNV, TenNV);
                main.PhanQuyen(role);

                this.Close();

                this.Close();
            }
        }
    }
}
