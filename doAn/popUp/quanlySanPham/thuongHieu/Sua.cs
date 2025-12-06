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

namespace doAn.popUp.thuongHieu
{
    public partial class Sua : Form
    {
        List<string> thuongHieuDaCo = new List<string>();

        private readonly BindingSource olddata;

        public Sua(BindingSource _olddata)
        {
            InitializeComponent();
            olddata = _olddata;
        }

        private void Sua_Load(object sender, EventArgs e)
        {
            DataRowView rowSelect = (DataRowView)olddata.Current;

            txtMaThuongHieu.Text = rowSelect["MaThuongHieu"].ToString();
            txtTenThuongHieu.Text = rowSelect["TenThuongHieu"].ToString();
        }

        private void btnDongY_Click(object sender, EventArgs e)
        {

            MyDataTable thuongHieu = new MyDataTable();
            thuongHieu.OpenConnection();
            SqlCommand thuongHieuCmd = new SqlCommand("SELECT MaThuongHieu FROM ThuongHieu");
            thuongHieu.Fill(thuongHieuCmd);
            foreach (DataRow row in thuongHieu.Rows)
            {
                string ma = row["MaThuongHieu"].ToString();
                thuongHieuDaCo.Add(ma);
            }

            DataRowView rowSelect = (DataRowView)olddata.Current;

            if (txtMaThuongHieu.Text.Trim() == null)
            {
                MessageBox.Show("Không được bỏ trống mã!");
            }
            else if (txtMaThuongHieu.TextLength != 5)
            {
                MessageBox.Show("Mã phải đủ 5 \n" + "Đã nhập: " + txtMaThuongHieu.TextLength);

            }
            else if (thuongHieuDaCo.Contains(txtMaThuongHieu.Text.ToUpper().Trim()))
            {
                MessageBox.Show("Mã này đã tồn tại!!");
                return;
            }
            else if (txtTenThuongHieu.Text.Trim() == null)
            {
                MessageBox.Show("Không được bỏ trống tên!");
            }

            rowSelect.BeginEdit();

            rowSelect["MaThuongHieu"] = txtMaThuongHieu.Text.ToUpper().Trim();
            rowSelect["TenThuongHieu"] = txtTenThuongHieu.Text;

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
