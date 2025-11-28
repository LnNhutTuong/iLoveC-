using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
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

        private void Sua_Load(object sender, EventArgs e)
        {
            DataRowView rowSelect = (DataRowView)olddata.Current;

            txtMaKhachHang.Text = rowSelect["MaKhachHang"].ToString();
            txtTenKhachHang.Text = rowSelect["TenKhachHang"].ToString();
            txtDiaChi.Text = rowSelect["DiaChi"].ToString();
            txtEmail.Text = rowSelect["Email"].ToString();
            txtSoDienThoai.Text = Convert.ToInt64(rowSelect["Sdt"]).ToString();
        }
    }
}
