using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace doAn.popUp
{
    public partial class Them : Form
    {
        private readonly BindingSource newdata;

        public Them(BindingSource _newdata)
        {
            InitializeComponent();
            newdata = _newdata;
        }

        private void btnDongY_Click(object sender, EventArgs e)
        {

            DataTable dt = (DataTable)newdata.DataSource;

            if (txtMaDanhMuc.Text.Trim() == null)
            {
                MessageBox.Show("Không được bỏ trống mã!");
            }
            else if (txtMaDanhMuc.TextLength > 5 || txtMaDanhMuc.MaxLength < 5)
            {
                MessageBox.Show("Mã phải đủ 5 \n" + "Đã nhập: " + txtMaDanhMuc.TextLength);

            }
            else if (txtTenDanhMuc.Text.Trim() == null)
            {
                MessageBox.Show("Không được bỏ trống tên!");
            }

            dt.Rows.Add(txtMaDanhMuc.Text, txtTenDanhMuc.Text);

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
