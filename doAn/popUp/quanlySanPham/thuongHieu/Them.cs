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
    public partial class Them : Form
    {
        List <string> thuongHieuDaCo = new List<string> ();
        private readonly BindingSource newdata;
        
      

        public Them(BindingSource _newdata)
        {
            InitializeComponent();
            newdata = _newdata;
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

            DataTable dt = (DataTable)newdata.DataSource;
            
            if(string.IsNullOrEmpty(txtMaThuongHieu.Text))
            {
                MessageBox.Show("Không được bỏ trống mã!");
                return;
            }
            else if (txtMaThuongHieu.TextLength!= 5)
            {
                MessageBox.Show("Mã phải đủ 5 \n" + "Đã nhập: "+ txtMaThuongHieu.TextLength);
                return;
            }
            else if (thuongHieuDaCo.Contains(txtMaThuongHieu.Text.ToUpper().Trim()))
            {
                MessageBox.Show("Mã này đã tồn tại!!");
                return;
            }
            else if (string.IsNullOrEmpty(txtTenThuongHieu.Text))
            {
                MessageBox.Show("Không được bỏ trống tên!");
                return;
            }

            dt.Rows.Add(txtMaThuongHieu.Text.ToUpper().Trim(), txtTenThuongHieu.Text);

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
