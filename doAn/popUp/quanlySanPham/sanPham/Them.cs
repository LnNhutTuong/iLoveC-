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

namespace doAn.popUp.quanlySanPham.sanPham
{
    public partial class Them : Form
    {
        //Lay du lieu tu ben thang cha
        //thang ben kia co khai bao binding
        // => co the noi duong dan du lieu tu cha xuong con
        //EXTREMELY IMPORTANT
        private readonly BindingSource newdata;

        MyDataTable dataTable = new MyDataTable();

        public Them(BindingSource _newdata)
        {
            InitializeComponent();
            dataTable.OpenConnection();
            newdata = _newdata;
            LayDuLieu();
        }

        public void LayDuLieu()
        {

            //DanhMuc
            MyDataTable danhMuc = new MyDataTable();
            danhMuc.OpenConnection();
            string danhMucSql = "SELECT * FROM DanhMuc";
            SqlCommand danhMucCmd = new SqlCommand(danhMucSql);
            danhMuc.Fill(danhMucCmd);
          
            cboDanhMuc.DataSource = danhMuc;
            cboDanhMuc.DisplayMember = "TenDanhMuc";
            cboDanhMuc.ValueMember = "MaDanhMuc";

            //ThuongHieu
            MyDataTable thuongHieu = new MyDataTable();
            thuongHieu.OpenConnection();
            string thuongHieuSql = "SELECT * FROM ThuongHieu";
            SqlCommand thuongHieuCmd = new SqlCommand(thuongHieuSql);

            thuongHieu.Fill(thuongHieuCmd);

            cboThuongHieu.DataSource = thuongHieu;
            cboThuongHieu.DisplayMember = "TenThuongHieu";
            cboThuongHieu.ValueMember = "MaThuongHieu";
        }

        decimal triGia;

        private void btnDongY_Click(object sender, EventArgs e)
        {
            DataTable dt = (DataTable)newdata.DataSource;

            if (string.IsNullOrWhiteSpace(txtMaSanPham.Text))
            {
                MessageBox.Show("Không được bỏ trống Mã!");
                return;
            }
            else if (txtMaSanPham.TextLength > 5 || txtMaSanPham.MaxLength < 5)
            {
                MessageBox.Show("Mã phải đủ 5 \n");
                return;

            }
            else if (string.IsNullOrWhiteSpace(txtTenSanPham.Text))
            {
                MessageBox.Show("Không được bỏ trống Tên!");
                return;
            }
            //else if (string.IsNullOrWhiteSpace(cboDanhMuc.Text))
            //{
            //    MessageBox.Show("Không được bỏ trống Danh Mục!");
            //}
            //else if (string.IsNullOrWhiteSpace(cboThuongHieu.Text))
            //{
            //    MessageBox.Show("Không được bỏ trống Thương Hiệu!");
            //}
            else if (string.IsNullOrWhiteSpace(txtTriGia.Text))
            {
                MessageBox.Show("Không được bỏ trống Mô Tả!");
                return;
            }
            else if (string.IsNullOrWhiteSpace(txtTriGia.Text))
            {
                MessageBox.Show("Không được bỏ trống Trị Giá!");
                return;
            }
            else if(!decimal.TryParse(txtTriGia.Text, out triGia))
            {
                MessageBox.Show("Trị Giá chỉ được nhập số!");
                return;
            }

            //thang nay them thang vao DATABASE luon
            //WHY? tai vi no phai them vao database luon de cho cai thang listSP
            //cap nhat sau do reload lai thi no moi hien ra san pham
            //con may thang khac vi sao lai chon Row thi la do co cai dataGrid
            //Row = dong khi them vao dataGrid bat buoc phai co nut luu de xu li
            //logic va toi uu nhat
            //That reason why 
           string sql = @"INSERT INTO SanPham
                (MaSanPham, TenSanPham, MaDanhMuc, MaThuongHieu, TriGia, MoTa)
                VALUES (@MaSanPham, @TenSanPham, @MaDanhMuc, @MaThuongHieu, @TriGia, @MoTa)";

            SqlCommand cmd = new SqlCommand(sql);

            cmd.Parameters.Add("@MaSanPham", SqlDbType.NVarChar, 5).Value = txtMaSanPham.Text;
            cmd.Parameters.Add("@TenSanPham", SqlDbType.NVarChar, 50).Value = txtTenSanPham.Text;
            cmd.Parameters.Add("@MaDanhMuc", SqlDbType.NVarChar, 5).Value = cboDanhMuc.SelectedValue;
            cmd.Parameters.Add("@MaThuongHieu", SqlDbType.NVarChar, 50).Value = cboThuongHieu.SelectedValue;
            cmd.Parameters.Add("@TriGia", SqlDbType.Decimal).Value = triGia;
            cmd.Parameters.Add("@MoTa", SqlDbType.NVarChar, 255).Value = txtMoTa.Text;

            dataTable.Update(cmd);
            MessageBox.Show("Thêm thành công!");
            this.Close();
            //Kieu nao cung phai load form danh sach :D
            ((mainSP)Application.OpenForms["mainSP"]).danhSachSP.LayDuLieu();

        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
