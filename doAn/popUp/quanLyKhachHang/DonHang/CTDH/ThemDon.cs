using doAn.quanLySanPham;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace doAn.popUp.quanLyKhachHang.DonHang.ChiTietDonHang
{
    public partial class ThemDon : Form
    {
        private readonly BindingSource newdata;
        MyDataTable dataTable = new MyDataTable();

        public ThemDon(BindingSource _newdata)
        {
            InitializeComponent();
            dataTable.OpenConnection();
            newdata = _newdata;
            LayDuLieu();
            loadcBo();
        }

        void loadcBo()
        {
            string maKH = cboMaKhachHang.SelectedValue.ToString();

            MyDataTable khachHang = new MyDataTable();
            khachHang.OpenConnection();

            SqlCommand khachHangCmd = new SqlCommand(@"SELECT * FROM KhachHang WHERE MaKhachHang = @MaKhachHang");
            khachHangCmd.Parameters.Add("@MaKhachHang", maKH);
            khachHang.Fill(khachHangCmd);

            Console.WriteLine(maKH);
            if (khachHang.Rows.Count > 0)
            {
                txtTenKhachHang.Text = khachHang.Rows[0]["TenKhachHang"].ToString();
                txtDiaChi.Text = khachHang.Rows[0]["DiaChi"].ToString();
                txtSoDienThoai.Text = khachHang.Rows[0]["Sdt"].ToString();
            }
        }


        void LayDuLieu()
        {
            //SAN PHAM
            

            flowSP.Controls.Clear();
            MyDataTable sanPham = new MyDataTable();
            if (sanPham.OpenConnection())
            {

                string sanPhamSql = "SELECT * FROM SanPham";
                SqlCommand sanPhamCmd = new SqlCommand(sanPhamSql);
                sanPham.Fill(sanPhamCmd);

                foreach (DataRow row in sanPham.Rows)
                {
                    SanPham sp = new SanPham();

                    sp.MaSanPham = row["MaSanPham"].ToString().ToUpper();
                    sp.AnhDaiDien = row["AnhDaiDien"].ToString();
                    sp._mode = "select";
                    sp.setData(row["TenSanPham"].ToString().ToUpper(), row["AnhDaiDien"].ToString());

                    sp.ChonSanPham += (sS, eS) =>
                    {
                        SanPham sp1 =  (SanPham)sS;

                        sp1.MaSanPham = row["MaSanPham"].ToString().ToUpper();
                        sp1.AnhDaiDien = row["AnhDaiDien"].ToString();
                        sp1._mode = "unselect";
                        sp1.setData(row["TenSanPham"].ToString().ToUpper(), row["AnhDaiDien"].ToString());
                        sp1.HuyChon += (sU, eU) =>
                        {
                            SanPham sp2 = (SanPham)sU;
                            sp2.MaSanPham = row["MaSanPham"].ToString().ToUpper();
                            sp2.AnhDaiDien = row["AnhDaiDien"].ToString();
                            sp2._mode = "select";
                            sp2.setData(row["TenSanPham"].ToString().ToUpper(), row["AnhDaiDien"].ToString());                                                     
                        };
                    };
                    flowSP.Controls.Add(sp);                    
                }

                //KHACH HANG
                MyDataTable khachHang = new MyDataTable();
                khachHang.OpenConnection();
                string khachHangSql = "SELECT * FROM KhachHang";
                SqlCommand khachHangCmd = new SqlCommand(khachHangSql);
                khachHang.Fill(khachHangCmd);

                cboMaKhachHang.DataSource = khachHang;
                cboMaKhachHang.DisplayMember = "MaKhachHang";
                cboMaKhachHang.ValueMember = "MaKhachHang";

                //HoaDon
                MyDataTable hoaDon = new MyDataTable();
                hoaDon.OpenConnection();
                string hoaDonSql = "SELECT * FROM HoaDon";
                SqlCommand hoaDonCmd = new SqlCommand(khachHangSql);
                hoaDon.Fill(hoaDonCmd);
            }


        }

        private void btnDongY_Click(object sender, EventArgs e)
        {
            DataTable dt = (DataTable)newdata.DataSource;

            if (string.IsNullOrWhiteSpace(txtMaDonHang.Text))
            {
                MessageBox.Show("Không được bỏ trống Mã đơn hàng!");
                return;
            }
            else if (txtMaDonHang.TextLength > 5 || txtMaDonHang.MaxLength < 5)
            {
                MessageBox.Show("Mã phải đủ 5 \n");
                return;

            }
            
            string sql = @"INSERT INTO DonHang
                            VALUES
                            (@MaSanPham, @TenSanPham, @MaDanhMuc, @MaThuongHieu, @TriGia, @AnhDaiDien, @MoTa)";

            SqlCommand cmd = new SqlCommand(sql);

            cmd.Parameters.Add("@MaSanPham", SqlDbType.NVarChar, 5).Value = txtMaSanPham.Text.ToUpper();
            cmd.Parameters.Add("@TenSanPham", SqlDbType.NVarChar, 50).Value = txtTenSanPham.Text;
            cmd.Parameters.Add("@MaDanhMuc", SqlDbType.NVarChar, 5).Value = cboDanhMuc.SelectedValue;
            cmd.Parameters.Add("@MaThuongHieu", SqlDbType.NVarChar, 50).Value = cboThuongHieu.SelectedValue;
            cmd.Parameters.Add("@TriGia", SqlDbType.Decimal).Value = triGia;
            cmd.Parameters.Add("@AnhDaiDien", SqlDbType.NVarChar, 255).Value = path;
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

        private void cboMaKhachHang_SelectedIndexChanged(object sender, EventArgs e)
        {
            loadcBo();
        }      
    }
}
