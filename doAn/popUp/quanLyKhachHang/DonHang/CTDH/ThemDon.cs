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

        //  ----- RẤT LẰN TÀ LÀ QUẰN -----
        List<SanPham> spDaChon = new List<SanPham>();
        //int tongSoLuong = spDaChon.Count;
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

        void tienVaTinh()
        {
            int total = spDaChon.Sum(x => x.triGia);

            lblSoLuong.Text = "Số lượng: " + spDaChon.Count;
            lblTongTien.Text = "Tổng tiền: " + total;
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
                    sp.triGia = Convert.ToInt32(row["TriGia"]);                
                    sp.ChonSanPham += (sS, eS) =>
                    {
                        SanPham item = (SanPham)sS;
                        item._mode = "unselect";
                        item.setData(row["TenSanPham"].ToString().ToUpper(), row["AnhDaiDien"].ToString());
                        sp.triGia = Convert.ToInt32(row["TriGia"]);
                        spDaChon.Add(item);
                        tienVaTinh();
                        item.HuyChon += (sU, eU) =>
                        {
                            SanPham item1 = (SanPham)sU;
                            item._mode = "select";
                            item.setData(row["TenSanPham"].ToString().ToUpper(), row["AnhDaiDien"].ToString());
                            sp.triGia = Convert.ToInt32(row["TriGia"]);
                            spDaChon.Remove(item);
                            tienVaTinh();
                        };
                    };                    
                    flowSP.Controls.Add(sp);
                    tienVaTinh();
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
            else if (txtMaDonHang.Text.Length != 5)
            {
                MessageBox.Show("Mã phải đủ 5 ");
                return;
            }
            else if (spDaChon.Count == 0)
            {
                MessageBox.Show("Phải chọn ít nhất 1 sản phẩm");
                return;
            }
            else
            {
                // thằng này là lưu cái đơn hàng của từng món hàng có nghĩa nó chỉ vừa
                // lưu cái hàng nào được mua
                try
                {
                    int tongSoLuong = spDaChon.Count;

                    string sql = @"INSERT INTO DonHang
                  VALUES (@MaDonHang, @MaKhachHang, @NgayLap, @TrangThai, @GhiChu, @TongSoLuong)";

                    SqlCommand cmd = new SqlCommand(sql);
                    cmd.Parameters.Add("@MaDonHang", txtMaDonHang.Text.ToUpper());
                    cmd.Parameters.Add("@MaKhachHang", cboMaKhachHang.SelectedValue.ToString());
                    cmd.Parameters.Add("@NgayLap", DateTime.Now);
                    cmd.Parameters.Add("@TrangThai", "0");

                    if (string.IsNullOrWhiteSpace(txtGhiChu.Text))
                    {
                        txtGhiChu.Text = "Không có ghi chú :)!";
                    }
                    cmd.Parameters.Add("@GhiChu", txtGhiChu.Text);

                    cmd.Parameters.Add("@TongSoLuong",tongSoLuong);

                    dataTable.Update(cmd);


                    // cái này là lưu vào cái bill
                    foreach (SanPham sp in spDaChon)
                    {
                        int total = spDaChon.Sum(x => sp.triGia);

                        string sqlC = @"INSERT INTO ChiTietDonHang 
                                     VALUES (@MaDonHang, @MaSanPham, @SoLuong, @ThanhTien)";

                        SqlCommand cmdC = new SqlCommand(sqlC);

                        cmdC.Parameters.Add("@MaDonHang", txtMaDonHang.Text.ToUpper());
                        cmdC.Parameters.Add("@MaSanPham", sp.MaSanPham);
                        dataTable.Update(cmdC);
                    }

                    MessageBox.Show("Thêm đơn hàng thành công!YASH");
                    this.Close();
                }
                catch
                {
                    MessageBox.Show("Đơn hàng đã tồn tại");
                }
            }
               
        }

        private void btnHuy_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cboMaKhachHang_SelectedIndexChanged(object sender, EventArgs e)
        {
            loadcBo();
        }
    }
}
