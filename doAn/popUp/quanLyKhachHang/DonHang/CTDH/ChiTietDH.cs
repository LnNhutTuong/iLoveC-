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
        //pham vi event kieu ten
        public event Action deletePr;


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
            spDaChon.Clear(); 
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

            // ----- PHAN CHIA THIEN HA -----

            flowSP.Controls.Clear();
            MyDataTable sanPham = new MyDataTable();
            if (sanPham.OpenConnection())
            {
                string sanPhamSql = "SELECT * FROM SanPham";
                SqlCommand sanPhamCmd = new SqlCommand(sanPhamSql);
                sanPham.Fill(sanPhamCmd);

                MyDataTable spChon = new MyDataTable();
                spChon.OpenConnection();
                string spChonSql = @"SELECT MaSanPham FROM ChiTietDonHang WHERE MaDonHang = @MaDonHang";
                SqlCommand spChonCmd = new SqlCommand(spChonSql);
                spChonCmd.Parameters.Add("@MaDonHang", maDH);
                spChon.Fill(spChonCmd);

                var dsSpChon = spChon.Rows.Cast<DataRow>().Select(r => r["MaSanPham"].ToString().ToUpper()).ToList();

                foreach (DataRow row in sanPham.Rows)
                {
                    SanPham sp = new SanPham();

                    sp.MaSanPham = row["MaSanPham"].ToString().ToUpper();

                    if (dsSpChon.Contains(sp.MaSanPham))
                    {
                        sp._mode = "unselect";      
                        sp.BackColor = Color.Green;
                        spDaChon.Add(new SanPham {MaSanPham = sp.MaSanPham, triGia = Convert.ToInt32(row["TriGia"])});
                    }
                    else
                    {
                        sp._mode = "select";
                        sp.BackColor = Color.White;
                    }


                    sp.AnhDaiDien = row["AnhDaiDien"].ToString();
                    sp.setData(row["TenSanPham"].ToString().ToUpper(),row["AnhDaiDien"].ToString());
                    sp.triGia = Convert.ToInt32(row["TriGia"]);
                    sp.HuyChon += (s, e) =>
                    {
                        var item = (SanPham)s;
                        spDaChon.RemoveAll(x => x.MaSanPham == item.MaSanPham);
                        item._mode = "select";
                        item.setData(row["TenSanPham"].ToString().ToUpper(), row["AnhDaiDien"].ToString());
                        item.BackColor = Color.White;
                        tienVaTinh();
                    };

                    sp.ChonSanPham += (s, e) =>
                    {
                        var item = (SanPham)s;
                        if (!spDaChon.Any(x => x.MaSanPham == item.MaSanPham))
                        {
                            spDaChon.Add(item);
                        }
                        item._mode = "unselect";
                        item.BackColor = Color.Green;
                        item.setData(row["TenSanPham"].ToString().ToUpper(), row["AnhDaiDien"].ToString());
                        tienVaTinh();
                    };
                    tienVaTinh();

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

            deletePr?.Invoke();

            DialogResult kq;
            kq = MessageBox.Show("Bạn có muốn xóa đơn hàng " + lblMaDonHang.Text + " không?", "Xóa",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (kq == DialogResult.Yes)
            {

                string sqlD = @"DELETE FROM ChiTietDonHang WHERE MaDonHang = @MaDonHang";

                SqlCommand cmdD = new SqlCommand(sqlD);

                cmdD.Parameters.Add("@MaDonHang", SqlDbType.NVarChar, 5).Value = lblMaDonHang.Text;

                string sql = @"DELETE FROM DonHang WHERE MaDonHang = @MaDonHang";

                SqlCommand cmd = new SqlCommand(sql);

                cmd.Parameters.Add("@MaDonHang", SqlDbType.NVarChar, 5).Value = lblMaDonHang.Text;

               
                myData.Update(cmd);

                MessageBox.Show("Đã xóa thành công!");

                this.Close();


            }
            this.DialogResult = DialogResult.OK;
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            int tongSoLuong = spDaChon.Count;

            string sql = @"UPDATE DonHang                          
                              SET TrangThai = @TrangThai,
                                  GhiChu = @GhiChu,
                                  TongSoLuong = @TongSoLuong
                               WHERE MaDonHang = @MaDonHang";

            SqlCommand cmd = new SqlCommand(sql);

            cmd.Parameters.AddWithValue("@MaDonHang", lblMaDonHang.Text);
            cmd.Parameters.AddWithValue("@TrangThai", cboTrangThai.SelectedValue);
            cmd.Parameters.AddWithValue("@GhiChu", txtGhiChu.Text);
            cmd.Parameters.AddWithValue("@TongSoLuong", tongSoLuong);
            myData.Update(cmd);

            string sqlD = @"DELETE FROM ChiTietDonHang WHERE MaDonHang = @MaDonHang";
            SqlCommand cmdD= new SqlCommand(sqlD);
            cmdD.Parameters.AddWithValue("@MaDonHang", lblMaDonHang.Text);
            myData.Update(cmdD);

            foreach (SanPham sp in spDaChon)
            {
                string sqlI = @"INSERT INTO ChiTietDonHang (MaDonHang, MaSanPham)
                    VALUES (@MaDonHang, @MaSanPham)";

                SqlCommand cmdI = new SqlCommand(sqlI);
                cmdI.Parameters.AddWithValue("@MaDonHang", lblMaDonHang.Text);
                cmdI.Parameters.AddWithValue("@MaSanPham", sp.MaSanPham);

                myData.Update(cmdI);
            }


            MessageBox.Show("Chỉnh sửa thành công!");
            ChiTietDonHang_Load(sender, e);
            this.DialogResult = DialogResult.OK;
        }

        private void btnTaiLai_Click(object sender, EventArgs e)
        {
            ChiTietDonHang_Load(sender, e);
            this.DialogResult = DialogResult.OK;
        }
    }
}
