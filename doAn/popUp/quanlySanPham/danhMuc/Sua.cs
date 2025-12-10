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

namespace doAn.popUp.sanPham
{
    public partial class Sua : Form
    {

        List<string> maDaCo = new List<string>();

        private readonly BindingSource olddata;

        string macu;

        public Sua(BindingSource _olddata)
        {
            InitializeComponent();
            olddata= _olddata;
        }

        private void Sua_Load(object sender, EventArgs e)
        {
            DataRowView rowSelect = (DataRowView)olddata.Current;

            macu = rowSelect["MaDanhMuc"].ToString().ToUpper().Trim();

            txtMaDanhMuc.Text = rowSelect["MaDanhMuc"].ToString();
            txtTenDanhMuc.Text = rowSelect["TenDanhMuc"].ToString();


        }

        private void btnDongY_Click(object sender, EventArgs e)
        {

            MyDataTable danhMuc = new MyDataTable();
            danhMuc.OpenConnection();

            SqlCommand danhMucCmd = new SqlCommand(@"SELECT MaDanhMuc FROM DanhMuc");
            danhMuc.Fill(danhMucCmd);
            foreach (DataRow row in danhMuc.Rows)
            {
                string ma = row["MaDanhMuc"].ToString();
                maDaCo.Add(ma);


            }

            DataRowView rowSelect = (DataRowView)olddata.Current;

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
            if (maDaCo.Contains(txtMaDanhMuc.Text.ToUpper().Trim())
                && txtMaDanhMuc.Text.ToUpper().Trim() != macu)
            {
                MessageBox.Show("Mã này đã tồn tại");
                return;
            }
            else if (string.IsNullOrEmpty(txtTenDanhMuc.Text))
            {
                MessageBox.Show("Không được bỏ trống tên!");
                return;
            }

            rowSelect.BeginEdit();

            rowSelect["MaDanhMuc"] = txtMaDanhMuc.Text.ToUpper().Trim();
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
