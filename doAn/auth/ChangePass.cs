using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Lifetime;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace doAn
{
    public partial class ChangePass : Form
    {
        public string MaNhanVien { get; set; }


        public ChangePass(string _manv)
        {
            InitializeComponent();
            MaNhanVien = _manv;
            
            this.Text = "Mã nhân viên: " + MaNhanVien;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnDangNhap_Click(object sender, EventArgs e)
        {
            if (txtMatKhauMoi.Text.Trim() == "")
            {
                MessageBox.Show("Mật khẩu không được bỏ trống!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtMatKhauMoi.Focus();
            }
            else if (txtMatKhauCu.Text.Trim() == "")
            {
                MessageBox.Show("Mật khẩu không được bỏ trống!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtMatKhauCu.Focus();
            }
            else if (txtMatKhauMoi.Text != txtMatKhauCu.Text)

            {
                MessageBox.Show("Mật khẩu mới và cũ không khớp", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                MyDataTable dmk = new MyDataTable();
                dmk.OpenConnection();

                string sql = @"UPDATE NhanVien SET MatKhau = @MatKhauMoi WHERE MaNhanVien = @MaNhanVien";
                SqlCommand cmd = new SqlCommand(sql);

                cmd.Parameters.Add("@MatKhauMoi", SqlDbType.VarChar, 255).Value = txtMatKhauMoi.Text;
                cmd.Parameters.Add("@MaNhanVien", SqlDbType.VarChar, 50).Value = MaNhanVien;

                dmk.Update(cmd);
                this.Close();
            }
        }
    }
}
