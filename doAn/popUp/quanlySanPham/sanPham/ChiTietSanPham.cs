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


            string path = myData.Rows[0]["AnhDaiDien"].ToString();

            //Console.WriteLine(name);

            // QONG TRUANG
            // muon object => string  thi can .Value
            // bỏ 
            string name = Regex.Match(path, @"([^\\\/]+)(?=\.[^.]+$)").Value;

            txtMaSanPham.Text = myData.Rows[0]["MaSanPham"].ToString();
            txtTenSanPham.Text = myData.Rows[0]["TenSanPham"].ToString();
            //lay gian tiep
            cboDanhMuc.SelectedValue = maDanhMuc;
            cboThuongHieu.SelectedValue = maThuongHieu;
            btnThayAnh.Text = name;
            txtMoTa.Text = myData.Rows[0]["MoTa"].ToString();
            txtTriGia.Text = myData.Rows[0]["TriGia"].ToString();

            pictureBox.Image = Image.FromFile(path);

            OnOff(false);
        }

        public void OnOff(bool value)
        {
            txtMaSanPham.Enabled = value;
            txtTenSanPham.Enabled = value;
            cboDanhMuc.Enabled = value;
            cboThuongHieu.Enabled = value;
            btnThayAnh.Enabled = value;
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

        string path = "";
        private void btnThayAnh_Click(object sender, EventArgs e)
        {
            var dialog = new OpenFileDialog();
            dialog.Title = "Chọn file ảnh";
            dialog.Filter = "Image Files|*.png;*.jpg;*.jpeg";

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                 path = dialog.FileName;

                string name = Regex.Match(path, @"([^\\\/]+)(?=\.[^.]+$)").Value;

                btnThayAnh.Text = name;

                pictureBox.Image = Image.FromFile(dialog.FileName);
            }
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
                string oldPath = myData.Rows[0]["AnhDaiDien"].ToString();

                string newPath = string.IsNullOrEmpty(path) ? oldPath : path;


                string sql = @"UPDATE SanPham                           
                              SET  MaSanPham = @MaSanPhamMoi,
                                   TenSanPham = @TenSanPham,
                                   MaDanhMuc = @MaDanhMuc,
                                   MaThuongHieu = @MaThuongHieu,
                                   AnhDaiDien = @path,
                                   MoTa = @MoTa,
                                   TriGia = @TriGia                                             
                               WHERE MaSanPham = @MaSanPhamCu";

                SqlCommand cmd = new SqlCommand(sql);

                //                      RẤT LÚ VÌ LÍ DO LÀM UI
                // cái này có nghĩa là nó sẽ add vào cái textbox trên cái popup đó
                // mà đó giờ lần đầu làm cái này nên ko hiểu tưởng đâu nó lưu vào cái db luôn nhưng đéo               
                //Ma moi = maMoi
                cmd.Parameters.AddWithValue("@MaSanPhamMoi", txtMaSanPham.Text.ToUpper());
                //Ma cu = maCu
                cmd.Parameters.AddWithValue("@MaSanPhamCu", maSP);

                // ĐÂY MỚI CHÍNH LÀ CODE DÙNG CHO VIỆC LƯU VÀO TRONG DB 
                // CÒN TRÊN KIA CHỈ LƯU VÀO TXT CỦA POPUP THÔI FUCK YOU
                // THẰNG NÀY NÓ CÒN ĐẶC BIỆT CÁI NỮA LÀ DO KHÚC LẤY DỮ LIỆU MÌNH PHẢI XÀI 
                // CÁI ROWS[0] NÊN MỚI BỊ NHƯ THẾ NÀY
                // VÌ NÊU KO CẬP NHẬT KIỂU NÀY VÀO DB THÌ HÀM LOAD NÓ SẼ VẪN HIỂU LÀ CÁI MÃ CŨ
                // NHƯNG CÁI VẤN ĐỀ LÀ KHI XỬ DỤNG BTN NÀY THÌ MÌNH SẼ THAY ĐỔI ĐƯỢC CÁI DANH MỤC VÀ THƯƠNG HIỆU
                // ==>>>>>> THẰNG maSP cũ sẽ đánh lộn like "tại sao t bị đổi cái này rồi,
                // cái row[0] danh mục và thương hiệu
                // của t biến mất r, trả lại cho t!!" =>> bật messagebox lên báo lỗi
                // Từ cái này rút ra được khuyết điểm mới của messagebox: Nó báo lỗi thật nhưng ko chỉ điểm.
                // đó là điểm yếu của Messagebox
                //Ma moi = maSP trong daubuoi
                maSP = txtMaSanPham.Text.ToUpper();



                cmd.Parameters.AddWithValue("@TenSanPham", txtTenSanPham.Text);
                cmd.Parameters.AddWithValue("@MaDanhMuc", cboDanhMuc.SelectedValue.ToString());
                cmd.Parameters.AddWithValue("@MaThuongHieu", cboThuongHieu.SelectedValue.ToString());




                //RAT KHO
                cmd.Parameters.AddWithValue("@path", newPath);



                cmd.Parameters.AddWithValue("@MoTa", txtMoTa.Text);
                cmd.Parameters.AddWithValue("@TriGia", txtTriGia.Text);


                myData.Update(cmd);

                MessageBox.Show("Lưu dữ liệu thành công!");

                //LOAD 2 lan
                // 1 cho popup
                ChiTietSanPham_Load(sender, e);
                // 1 cho list
                // YASH

                ((mainSP)Application.OpenForms["mainSP"]).danhSachSP.LayDuLieu();   
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
