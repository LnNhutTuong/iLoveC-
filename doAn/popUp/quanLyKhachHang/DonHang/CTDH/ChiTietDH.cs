using doAn.main.quanLyKhachHang;
using doAn.quanLyKhachHang;
using doAn.quanLySanPham;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace doAn.popUp.quanLyKhachHang.DonHang
{
    public partial class ChiTietDH : Form
    {
        MyDataTable myData = new MyDataTable();
        string maDH;
        //  ----- RẤT LẰN TÀ LÀ QUẰN -----
        List<SanPham> spDaChon = new List<SanPham>();
        //int tongSoLuong = spDaChon.Count;
        public ChiTietDH(string _maDH)
        {
            InitializeComponent();
            maDH = _maDH;
            myData.OpenConnection();

        }

        void tienVaTinh()
        {
            int total = spDaChon.Sum(x => x.triGia);

            lblSoLuong.Text = "Số lượng: " + spDaChon.Count;
            lblTongTien.Text = "Tổng tiền: " + total;
        }

        public void LayDuLieu()
        {
            //Rất lú vì cái bảng trung gian (dùng cho việc có nhiều hàng trong 1 đơn) nên bắt buộc phải join
            MyDataTable chiTietDonHang = new MyDataTable();
            chiTietDonHang.OpenConnection();
            SqlCommand chiTietDonHangSql = new SqlCommand(@"SELECT  dh.MaDonHang,
                                                                    dh.MaKhachHang,
                                                                    kh.TenKhachHang,
                                                                    kh.Sdt,
                                                                    kh.DiaChi,
                                                                    dh.TrangThai,
                                                                    dh.GhiChu,
                                                                    sp.TenSanPham,
                                                                    ctdh.SoLuong,
                                                                    ctdh.ThanhTien
                                                            FROM DonHang dh
                                                            JOIN KhachHang kh ON dh.MaKhachHang = kh.MaKhachHang
                                                            LEFT JOIN ChiTietDonHang ctdh ON dh.MaDonHang = ctdh.MaDonHang   
                                                            LEFT JOIN SanPham sp ON ctdh.MaSanPham = sp.MaSanPham                                                            
                                                            WHERE dh.MaDonHang = @MaDonHang");
            chiTietDonHangSql.Parameters.AddWithValue("@MaDonHang", maDH);
            chiTietDonHang.Fill(chiTietDonHangSql);

            lblMaDonHang.Text = chiTietDonHang.Rows[0]["MaDonHang"].ToString();
            txtMaKhachHang.Text = chiTietDonHang.Rows[0]["MaKhachHang"].ToString();
            txtTenKhachHang.Text = chiTietDonHang.Rows[0]["TenKhachHang"].ToString();
            txtDiaChi.Text = chiTietDonHang.Rows[0]["DiaChi"].ToString();
            txtSoDienThoai.Text = chiTietDonHang.Rows[0]["Sdt"].ToString();


            //Qua met, tham khao chat gpt 100% ko suy nghi duoc gi het
            //Bat buoc phai co cai mang rieng de tiet kiem thoi gian viet
            //vi ko co cai bang nao de lam chuyen nay
            var listTrangThai = new[]
            {
                new { Value = 0, Text = "Chuẩn bị" },
                new { Value = 1, Text = "Đang giao" },
                new { Value = 2, Text = "Giao thành công" },
                new { Value = 3, Text = "Giao thất bại" },
                new { Value = 4, Text = "Hàng hoàn" }
            };

            cboTrangThai.DataSource = listTrangThai;
            cboTrangThai.DisplayMember = "Text";
            cboTrangThai.ValueMember = "Value";

            cboTrangThai.SelectedValue = Convert.ToInt32(chiTietDonHang.Rows[0]["TrangThai"]);

            txtGhiChu.Text = chiTietDonHang.Rows[0]["GhiChu"].ToString();

            lblSoLuong.Text ="Số lượng: "+ chiTietDonHang.Rows[0]["SoLuong"].ToString();
            lblTongTien.Text = "Tổng tiền: " + chiTietDonHang.Rows[0]["ThanhTien"].ToString();

            // ----- PHAN CHIA THIEN HA -----

            flowSP.Controls.Clear();
            MyDataTable sanPham = new MyDataTable();
            if (sanPham.OpenConnection())
            {
                SqlCommand cmd = new SqlCommand(@"
                                                SELECT sp.MaSanPham, sp.TenSanPham, sp.AnhDaiDien, sp.TriGia
                                                FROM ChiTietDonHang ctdh
                                                JOIN SanPham sp ON ctdh.MaSanPham = sp.MaSanPham
                                                WHERE ctdh.MaDonHang = @MaDonHang");

                cmd.Parameters.AddWithValue("@MaDonHang", maDH);

                sanPham.Fill(cmd);

                foreach (DataRow row in sanPham.Rows)
                {
                    SanPham sp = new SanPham();

                    sp.MaSanPham = row["MaSanPham"].ToString().ToUpper();
                    sp.AnhDaiDien = row["AnhDaiDien"].ToString();
                    sp._mode = "unselect";
                    sp.setData(row["TenSanPham"].ToString().ToUpper(),row["AnhDaiDien"].ToString());
                    sp.triGia = Convert.ToInt32(row["TriGia"]);
                    sp.HuyChon += (sD, eD) =>
                    {
                        SanPham item = (SanPham)sD;
                        item.MaSanPham = row["MaSanPham"].ToString().ToUpper();
                        item.AnhDaiDien = row["AnhDaiDien"].ToString();
                        item._mode = "select";
                        item.setData(row["TenSanPham"].ToString().ToUpper(), row["AnhDaiDien"].ToString());
                        spDaChon.Remove(item);
                        tienVaTinh();
                        item.HuyChon += (sS, eS) =>
                        {
                            SanPham item1 = new SanPham();
                            item1.MaSanPham = row["MaSanPham"].ToString().ToUpper();
                            item1.AnhDaiDien = row["AnhDaiDien"].ToString();
                            item1._mode = "unselect";
                            item1.setData(row["TenSanPham"].ToString().ToUpper(), row["AnhDaiDien"].ToString());
                            spDaChon.Add(item);
                            tienVaTinh();
                        };
                    };
                    flowSP.Controls.Add(sp);
                }
            }
        }

        public void OnOff(bool value)
        {
            cboTrangThai.Enabled = value;
            txtGhiChu.Enabled = value;

            btnSua.Enabled = !value;
            btnXoa.Enabled = !value;

            btnTaiLai.Enabled = value;
            btnLuu.Enabled = value;

        }

        private void ChiTietDonHang_Load(object sender, EventArgs e)
        {
            this.Text = "Đơn hàng: " + lblMaDonHang.Text;
            LayDuLieu();
            OnOff(false);

            Console.WriteLine(lblMaDonHang.Text);
        }      

        private void btnSua_Click(object sender, EventArgs e)
        {
            OnOff(true);
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            DialogResult kq;
            kq = MessageBox.Show("Bạn có muốn xóa đơn hàng " + lblMaDonHang.Text + " không?", "Xóa",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (kq == DialogResult.Yes)
            {
                string sql = @"DELETE FROM DonHang WHERE MaDonHang = @MaDonHang";

                SqlCommand cmd = new SqlCommand(sql);

                cmd.Parameters.Add("@MaDonHang", SqlDbType.NVarChar, 5).Value = lblMaDonHang.Text;

                myData.Update(cmd);

                MessageBox.Show("Đã xóa thành công!");

                this.Close();
            }
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            string sql = @"UPDATE DonHang                          
                              SET TrangThai = @TrangThai,
                                  GhiChu = @GhiChu
                               WHERE MaDonHang = @MaDonHang";

            SqlCommand cmd = new SqlCommand(sql);

            cmd.Parameters.AddWithValue("@MaDonHang", lblMaDonHang.Text);
            cmd.Parameters.AddWithValue("@TrangThai", cboTrangThai.SelectedValue);
            cmd.Parameters.AddWithValue("@GhiChu", txtGhiChu.Text);

            myData.Update(cmd);
            MessageBox.Show("Chỉnh sửa thành công!");
            ChiTietDonHang_Load(sender, e);
        }

        private void btnTaiLai_Click(object sender, EventArgs e)
        {
            ChiTietDonHang_Load(sender, e);
        }
    }
}
