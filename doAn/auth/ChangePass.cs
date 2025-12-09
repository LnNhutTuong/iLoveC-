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
using BC = BCrypt.Net.BCrypt;
namespace doAn
{
    public partial class ChangePass : Form
    {
        public string MaNhanVien { get; set; }


        public ChangePass(string _manv)
        {
            InitializeComponent();
            MaNhanVien = _manv;
            txtMatKhauMoi.UseSystemPasswordChar = true;
            txtMatKhauCu.UseSystemPasswordChar = true;

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
            else if (txtMatKhauMoi.Text == txtMatKhauCu.Text)

            {
                MessageBox.Show("Mật khẩu mới và cũ khớp nhau", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                MyDataTable mk = new MyDataTable();
                mk.OpenConnection();
                string Sqlmk = "SELECT MatKhau FROM NhanVien WHERE MaNhanVien = @MaNhanVien";
                SqlCommand Cmdmk = new SqlCommand(Sqlmk);
                Cmdmk.Parameters.Add("@MaNhanVien", SqlDbType.NVarChar, 50).Value = MaNhanVien;
                mk.Fill(Cmdmk);

                string hashCu = mk.Rows[0]["MatKhau"].ToString();


                MyDataTable dmk = new MyDataTable();
                dmk.OpenConnection();

                string sql = @"UPDATE NhanVien SET MatKhau = @MatKhauMoi WHERE MaNhanVien = @MaNhanVien";
                SqlCommand cmd = new SqlCommand(sql);


                if (!BC.Verify(txtMatKhauCu.Text, hashCu))
                {
                    MessageBox.Show("Mật khẩu cũ không đúng!", "Lỗi");
                    return;
                }

                string hashMoi = BC.HashPassword(txtMatKhauMoi.Text);

                cmd.Parameters.Add("@MatKhauMoi", SqlDbType.VarChar, 100).Value = hashMoi;
                cmd.Parameters.Add("@MaNhanVien", SqlDbType.VarChar, 100).Value = MaNhanVien;

                dmk.Update(cmd);

                MessageBox.Show("Đổi mật khẩu thành công");

                //this.Close();
            }
        }

        private void btnAnHienMatKhau_Click(object sender, EventArgs e)
        {
            if (txtMatKhauCu.UseSystemPasswordChar && txtMatKhauMoi.UseSystemPasswordChar)
            {
                txtMatKhauCu.UseSystemPasswordChar = false;
                txtMatKhauMoi.UseSystemPasswordChar = false;

                btnAnHienMatKhau.BackgroundImage = Properties.Resources.visible;
            }
            else
            {
                txtMatKhauCu.UseSystemPasswordChar = true;
                txtMatKhauMoi.UseSystemPasswordChar = true;
                btnAnHienMatKhau.BackgroundImage = Properties.Resources.hide;
            }
        }
    }
}
