using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace doAn.popUp.quanLyKhachHang.khachHang
{
    public partial class Sua : Form
    {

        decimal sdt;

        private readonly BindingSource olddata;
        public Sua(BindingSource _olddata)
        {
            InitializeComponent();
            olddata = _olddata;
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

        private void Sua_Load(object sender, EventArgs e)
        {
            DataRowView rowSelect = (DataRowView)olddata.Current;

            txtMaKhachHang.Text = rowSelect["MaKhachHang"].ToString();
            txtTenKhachHang.Text = rowSelect["TenKhachHang"].ToString();
            txtDiaChi.Text = rowSelect["DiaChi"].ToString();
            txtEmail.Text = rowSelect["Email"].ToString();
            txtSoDienThoai.Text = rowSelect["Sdt"].ToString();
        }

        private void btnDongY_Click(object sender, EventArgs e)
        {
            DataRowView rowSelect = (DataRowView)olddata.Current;
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

            rowSelect.BeginEdit();

            rowSelect["MaKhachHang"] = txtMaKhachHang.Text;
            rowSelect["TenKhachHang"] = txtTenKhachHang.Text;
            rowSelect["DiaChi"] = txtDiaChi.Text;
            rowSelect["Sdt"] = txtSoDienThoai.Text;
            rowSelect["Email"] = txtEmail.Text;
            
            rowSelect.EndEdit();

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
