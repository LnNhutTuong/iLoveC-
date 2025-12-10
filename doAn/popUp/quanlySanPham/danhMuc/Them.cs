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

namespace doAn.popUp
{
    public partial class Them : Form
    {
        List<string> maDaCo = new List<string>();
        private readonly BindingSource newdata;

        public Them(BindingSource _newdata)
        {
            InitializeComponent();
            newdata = _newdata;
        }

        private void btnDongY_Click(object sender, EventArgs e)
        {

            MyDataTable danhMuc = new MyDataTable();
            danhMuc.OpenConnection();
            SqlCommand danhMucCmd = new SqlCommand(@"SELECT MaDanhMuc FROM DanhMuc");
            danhMuc.Fill(danhMucCmd);
            foreach (DataRow row in danhMuc.Rows) {
                string ma = row["MaDanhMuc"].ToString();
                maDaCo.Add(ma);
            }

            DataTable dt = (DataTable)newdata.DataSource;

            if (string.IsNullOrEmpty(txtMaDanhMuc.Text))
            {
                MessageBox.Show("Không được bỏ trống mã!");
                return;
            }
            else if (txtMaDanhMuc.TextLength != 5)
            {
                MessageBox.Show("Mã phải đủ 5 \n" + "Đã nhập: " + txtMaDanhMuc.TextLength);
                return;
            }
            else if (maDaCo.Contains(txtMaDanhMuc.Text.ToUpper().Trim()))
            {
                MessageBox.Show("Mã này đã tồn tại");
                return;
            }
            else if (string.IsNullOrEmpty(txtTenDanhMuc.Text))
            {
                MessageBox.Show("Không được bỏ trống tên!");
                return;
            }

            dt.Rows.Add(txtMaDanhMuc.Text.ToUpper().Trim(), txtTenDanhMuc.Text);

            this.DialogResult = DialogResult.OK;

            MessageBox.Show("Thêm thành công!!");
            this.Close();
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
