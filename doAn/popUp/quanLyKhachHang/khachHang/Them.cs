using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Mail;


namespace doAn.popUp.quanLyKhachHang.khachHang
{
    public partial class Them : Form
    {
        private readonly BindingSource newdata;

        public Them(BindingSource _newdata)
        {
            InitializeComponent();
            newdata = _newdata;
        }

        public bool KiemTraEmail(string email)
        {
            try
            {
                //lan1
                MailAddress m = new MailAddress(email);

                //lan2
                return m.Address == email;
            }
            catch (FormatException)
            {
                return false;
            }
        }

        decimal sdt;
        private void btnDongY_Click(object sender, EventArgs e)
        {
            DataTable dt = (DataTable)newdata.DataSource;

            if (txtMaKhachHang.Text.Trim() == null)
            {
                MessageBox.Show("Không được bỏ trống mã!");
                return;
            }
            else if (txtMaKhachHang.TextLength > 5 || txtMaKhachHang.MaxLength < 5)
            {
                MessageBox.Show("Mã phải đủ 5 \n" + txtMaKhachHang.TextLength);
                return;
            }
            else if (txtTenKhachHang.Text.Trim() == null)
            {
                MessageBox.Show("Không được bỏ trống tên!");
                return;
            }
            else if (txtDiaChi.Text.Trim() == null)
            {
                MessageBox.Show("Không được bỏ trống tên!");
                return;

            }
            else if (!decimal.TryParse(txtSoDienThoai.Text, out sdt))
            {
                MessageBox.Show("Số điện thoại phải là số!", "LỖI",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else if (txtSoDienThoai.Text.Trim() == null)
            {
                MessageBox.Show("Không được bỏ trống tên!");
                return;
            }
            else if (txtSoDienThoai.Text.Length > 10 || txtSoDienThoai.Text.Length < 10)
            {
                MessageBox.Show("Số điện thoại phải là 11 số \n" + "Bạn đã nhập: " + txtSoDienThoai.Text.Length + " số!", "LỖI",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtSoDienThoai.Clear();
                return;

            }
            else if (!txtSoDienThoai.Text.StartsWith("0"))
            {
                MessageBox.Show("Số điện thoại phải bắt đầu từ số 0", "LỖI",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtSoDienThoai.Clear();
                return;

            }

            //------------EMAIL
            else if (txtEmail.Text.Trim() == "")
            {
                MessageBox.Show("Email không được bỏ trống ", "LỖI",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;

            }
            else if (KiemTraEmail(txtEmail.Text) == false)
            {
                MessageBox.Show("Email không đúng định dạng ", "LỖI",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtEmail.Clear();
                return;
            }

            dt.Rows.Add(txtMaKhachHang.Text, txtTenKhachHang.Text, txtDiaChi.Text,  txtSoDienThoai.Text, txtEmail.Text, 0);

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
