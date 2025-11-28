using doAn.quanLySanPham;
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
    public partial class ChiTietSanPham : Form
    {

        MyDataTable myData = new MyDataTable();
        string maSP;


        public ChiTietSanPham(string _maSP)
        {
            InitializeComponent();
            maSP = _maSP;
            myData.OpenConnection();
           
        }

        public void LayDuLieu()
        {       

            SqlCommand sanPhamCmd = new SqlCommand("SELECT * FROM SanPham WHERE MaSanPham = @MaSanPham");
            sanPhamCmd.Parameters.AddWithValue("@MaSanPham", maSP);
            myData.Fill(sanPhamCmd);

            //Khong lay truc tiep duoc vi neu lay truc tiep thi no se hieu la
            //lay thang dau tien trong bang Danhmcu va thuonghieu chu ko phai
            //la thang danh muc cua thang dau tien trong bang sanpham
            string maDanhMuc = myData.Rows[0]["MaDanhMuc"].ToString();
            string maThuongHieu = myData.Rows[0]["MaThuongHieu"].ToString();

            MyDataTable danhMuc = new MyDataTable();
            danhMuc.OpenConnection();
            SqlCommand danhMucSql = new SqlCommand("SELECT * FROM DanhMuc");
            danhMuc.Fill(danhMucSql);

            cboDanhMuc.DataSource = danhMuc;
            //Hien ra
            cboDanhMuc.DisplayMember = "TenDanhMuc";
            //Value
            cboDanhMuc.ValueMember = "MaDanhMuc";




            MyDataTable thuongHieu = new MyDataTable();
            thuongHieu.OpenConnection();
            SqlCommand thuonghieuSql = new SqlCommand("SELECT * FROM ThuongHieu");
            thuongHieu.Fill(thuonghieuSql);

            cboThuongHieu.DataSource = thuongHieu;
            //Hien ra
            cboThuongHieu.DisplayMember = "TenThuongHieu";
            //Value
            cboThuongHieu.ValueMember = "MaThuongHieu";
                     


            txtMaSanPham.Text = myData.Rows[0]["MaSanPham"].ToString();
            txtTenSanPham.Text = myData.Rows[0]["TenSanPham"].ToString();
            //lay gian tiep
            cboDanhMuc.SelectedValue = maDanhMuc;
            cboThuongHieu.SelectedValue = maThuongHieu;
            txtMoTa.Text = myData.Rows[0]["MoTa"].ToString();
            txtTriGia.Text = myData.Rows[0]["TriGia"].ToString();

            OnOff(false);
        }

        public void OnOff(bool value)
        {
            txtMaSanPham.Enabled = value;
            txtTenSanPham.Enabled = value;
            cboDanhMuc.Enabled = value;
            cboThuongHieu.Enabled = value;
            txtMoTa.Enabled = value;
            txtTriGia.Enabled = value;
           
            btnSua.Enabled = !value;
            btnXoa.Enabled = !value;

            btnTaiLai.Enabled = value;
            btnLuu.Enabled = value;

        }


        private void ChiTietSanPham_Load(object sender, EventArgs e)
        {
            LayDuLieu();
            OnOff(false);
        }     

        private void btnSua_Click(object sender, EventArgs e)
        {
            OnOff(true);

        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            DialogResult kq;
            kq = MessageBox.Show("Bạn có muốn xóa sản phẩm " + txtTenSanPham.Text + " không?", "Xóa",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (kq == DialogResult.Yes)
            {
                string sql = @"DELETE FROM SanPham WHERE MaSanPham = @MaSanPham";

                SqlCommand cmd = new SqlCommand(sql);

                cmd.Parameters.Add("@MaSanPham", SqlDbType.NVarChar, 5).Value = txtMaSanPham.Text;

                myData.Update(cmd);

                MessageBox.Show("Đã xóa thành công!");

                ((mainSP)Application.OpenForms["mainSP"]).danhSachSP.LayDuLieu();

                this.Close();
            }
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            try
            {
                string sql = @"UPDATE SanPham                           
                              SET  TenSanPham = @TenSanPham,
                                   MaDanhMuc = @MaDanhMuc,
                                   MaThuongHieu = @MaThuongHieu,
                                   MoTa = @MoTa,
                                   TriGia = @TriGia                                             
                               WHERE MaSanPham = @MaSanPham";

                SqlCommand cmd = new SqlCommand(sql);

                cmd.Parameters.AddWithValue("@MaSanPham", txtMaSanPham.Text);
                cmd.Parameters.AddWithValue("@TenSanPham", txtTenSanPham.Text);
                cmd.Parameters.AddWithValue("@MaDanhMuc", cboDanhMuc.SelectedValue.ToString());
                cmd.Parameters.AddWithValue("@MaThuongHieu", cboThuongHieu.SelectedValue.ToString());
                cmd.Parameters.AddWithValue("@MoTa", txtMoTa.Text);
                cmd.Parameters.AddWithValue("@TriGia", txtTriGia.Text);


                myData.Update(cmd);

                MessageBox.Show("Lưu dữ liệu thành công!");
                ChiTietSanPham_Load(sender, e);
            }

            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lưu: " + ex.Message);
            }
        }

        private void btnTaiLai_Click(object sender, EventArgs e)
        {
            ChiTietSanPham_Load(sender, e);
        }
    }
}
