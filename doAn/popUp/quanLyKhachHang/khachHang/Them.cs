using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


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
        List<string> maDaCo = new List<string>();

        private void btnDongY_Click(object sender, EventArgs e)
        {

            MyDataTable khachHang = new MyDataTable();
            khachHang.OpenConnection();
            SqlCommand danhMucCmd = new SqlCommand(@"SELECT MaKhachHang FROM KhachHang");
            khachHang.Fill(danhMucCmd);
            foreach (DataRow row in khachHang.Rows)
            {
                string ma = row["MaKhachHang"].ToString();
                maDaCo.Add(ma);
            }

            DataTable dt = (DataTable)newdata.DataSource;

            if (string.IsNullOrEmpty(txtMaKhachHang.Text))
            {
                MessageBox.Show("Không được bỏ trống mã!");
                return;
            }
            else if (txtMaKhachHang.TextLength !=5)
            {
                MessageBox.Show("Mã phải đủ 5 \n" + txtMaKhachHang.TextLength);
                return;
            }
            else if (maDaCo.Contains(txtMaKhachHang.Text.Trim().ToUpper()))
            {
                MessageBox.Show("Mã này đã tồn tại");
                return;
            }
            else if (string.IsNullOrEmpty(txtTenKhachHang.Text))
            {
                MessageBox.Show("Không được bỏ trống tên!");
                return;
            }
            else if (string.IsNullOrEmpty(txtDiaChi.Text))
            {
                MessageBox.Show("Không được bỏ trống địa chỉ!");
                return;

            }
            else if (!decimal.TryParse(txtSoDienThoai.Text, out sdt))
            {
                MessageBox.Show("Số điện thoại phải là số!", "LỖI",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else if (string.IsNullOrEmpty(txtSoDienThoai.Text))
            {
                MessageBox.Show("Không được bỏ trống Số điện thoại!");
                return;
            }
            else if (txtSoDienThoai.Text.Length != 10)
            {
                MessageBox.Show("Số điện thoại phải là 10 số \n" + "Bạn đã nhập: " + txtSoDienThoai.Text.Length + " số!", "LỖI",
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
            else if (string.IsNullOrEmpty(txtEmail.Text))
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

            dt.Rows.Add(txtMaKhachHang.Text, txtTenKhachHang.Text, txtDiaChi.Text,  sdt, txtEmail.Text, 0);

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
