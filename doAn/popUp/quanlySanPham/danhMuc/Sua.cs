using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace doAn.popUp.sanPham
{
    public partial class Sua : Form
    {
        private readonly BindingSource olddata;

        public Sua(BindingSource _olddata)
        {
            InitializeComponent();
            olddata= _olddata;
        }

        private void Sua_Load(object sender, EventArgs e)
        {
            DataRowView rowSelect = (DataRowView)olddata.Current;
            
            txtMaDanhMuc.Text = rowSelect["MaDanhMuc"].ToString();
            txtTenDanhMuc.Text = rowSelect["TenDanhMuc"].ToString();

        }

        private void btnDongY_Click(object sender, EventArgs e)
        {
            DataRowView rowSelect = (DataRowView)olddata.Current;

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

            rowSelect.BeginEdit();

            rowSelect["MaDanhMuc"] = txtMaDanhMuc.Text;
            rowSelect["TenDanhMuc"] = txtTenDanhMuc.Text;

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
