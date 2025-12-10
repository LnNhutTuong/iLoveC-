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
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace doAn.popUp.quanlySanPham.sanPham
{
    public partial class ChiTietSanPham : Form
    {
        //tạo cái đường dẫn tương đối để lưu vào db
        private string tuongDoiPathAnh = "";

        List<string> maTonTai = new List<string>();


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



            //Console.WriteLine(name);

            // QONG TRUANG
            // muon object => string  thi can .Value
            // bỏ 

            txtMaSanPham.Text = myData.Rows[0]["MaSanPham"].ToString();
            txtTenSanPham.Text = myData.Rows[0]["TenSanPham"].ToString();
            //lay gian tiep
            cboDanhMuc.SelectedValue = maDanhMuc;
            cboThuongHieu.SelectedValue = maThuongHieu;
            txtMoTa.Text = myData.Rows[0]["MoTa"].ToString();
            txtTriGia.Text = myData.Rows[0]["TriGia"].ToString();

            tuongDoiPathAnh = myData.Rows[0]["AnhDaiDien"].ToString();
            string name = Regex.Match(tuongDoiPathAnh, @"([^\\\/]+)(?=\.[^.]+$)").Value;
            btnThayAnh.Text = name;


            //Console.WriteLine("Skibidi: "+btnThayAnh.Text);
            //cai nay la sao?
            // cai nay tao thanh 1 cai path
            // combine no ghep lai                 V
            //thang ngay mui ten la lay duong dan tu bin
            //khuc sau la tu db 
            //booooom ghep lai thanh 1 path
            string imgPath = Path.Combine(Application.StartupPath, tuongDoiPathAnh);
            Console.WriteLine(imgPath);
            string defaultImg = Path.Combine(Application.StartupPath, "images/noImg.jfif");

            if (!File.Exists(imgPath) || string.IsNullOrWhiteSpace(imgPath))
            {
                pictureBox.Image = Image.FromFile(defaultImg);
            }
            else
            {
                //Console.WriteLine("success");
                pictureBox.Image = Image.FromFile(imgPath);
            }

            




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

                string filename = Path.GetFileName(path);

                string uploadFolder = Path.Combine(Application.StartupPath, "Upload");
                Directory.CreateDirectory(uploadFolder);

                string destPath = Path.Combine(uploadFolder, filename);

                File.Copy(path, destPath, true);


                //Luc nay la do cai thang nay la bien toan cuc
                //Nen la gan no thanh cai nay
                //combine: ghep 2 thang trong kia lai thanh 1 duong dan

                tuongDoiPathAnh = Path.Combine("Upload", filename);

                // đổi text nút thành tên file
                //TẠI SAO KHÔNG DUDWOJ ÁDSAJDIAWJDPOAKDOPASKDPKAWOPD
                btnThayAnh.Text = filename;

                pictureBox.Image = Image.FromFile(destPath);
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

                ((QlSanPham)Application.OpenForms["QlSanPham"]).danhSachSP.LayDuLieu();

                this.Close();
            }
        }

        int gia = 0;
        private void btnLuu_Click(object sender, EventArgs e)
        {
            MyDataTable sanPham = new MyDataTable();
            sanPham.OpenConnection();
            SqlCommand sanPhamCmdC = new SqlCommand("SELECT MaSanPham FROM SanPham");
            sanPham.Fill(sanPhamCmdC);
            //string maDaCo = sanPham.Rows.ToString();


            string maMoi = txtMaSanPham.Text.ToUpper().Trim();


            foreach (DataRow row in sanPham.Rows)
            {
                string ma = row["MaSanPham"].ToString();
                maTonTai.Add(ma);
            }

            try
            {             
                if (string.IsNullOrEmpty(txtMaSanPham.Text))
                {
                    MessageBox.Show("Không được bỏ trống mã!");
                    return;
                }
                else if (txtMaSanPham.TextLength != 5)
                {
                    MessageBox.Show("Mã phải đủ 5 \n" + "Đã nhập: " + txtMaSanPham.TextLength);
                    return;
                }
                else if (maMoi != maSP && maTonTai.Contains(maMoi))
                {
                    MessageBox.Show("Mã này đã tồn tại!!");
                    return;
                }
                else if (string.IsNullOrEmpty(txtTenSanPham.Text))
                {
                    MessageBox.Show("Không được bỏ trống tên!");
                    return;
                }
                else if (string.IsNullOrEmpty(txtMoTa.Text))
                {
                    MessageBox.Show("Không được bỏ trống Mô tả!");
                    return;
                }
                else if(string.IsNullOrEmpty(txtTriGia.Text))
                {
                    MessageBox.Show("Không được bỏ trống Mô tả!");
                    return;
                }
                else if (!int.TryParse(txtTriGia.Text, out gia))
                {
                    MessageBox.Show("Giá trị bắt buộc phải là số!");
                    return;
                }
                // San Pham
                Console.WriteLine("Row 0:niek");
                //string oldPath = myData.Rows[0]["AnhDaiDien"].ToString();

                //string newPath = string.IsNullOrEmpty(path) ? oldPath : path;


                string sql = @"UPDATE SanPham                           
                              SET  MaSanPham = @MaSanPhamMoi,
                                   TenSanPham = @TenSanPham,
                                   MaDanhMuc = @MaDanhMuc,
                                   MaThuongHieu = @MaThuongHieu,
                                   AnhDaiDien = @tuongDoiPathAnh,
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
                cmd.Parameters.AddWithValue("@tuongDoiPathAnh", tuongDoiPathAnh);



                cmd.Parameters.AddWithValue("@MoTa", txtMoTa.Text);
                cmd.Parameters.AddWithValue("@TriGia", txtTriGia.Text);


                myData.Update(cmd);

                MessageBox.Show("Lưu dữ liệu thành công!");

                //Neu co don hang thi phai update trong don hang luon 

                string sqlD = @"UPDATE  ChiTietSanPham
                                SET     MaSanPham = @MaSanPhamMoi
                                WHERE   MaSanPham = @MaSanPhamCu";

                //LOAD 2 lan
                // 1 cho popup
                ChiTietSanPham_Load(sender, e);
                // 1 cho list
                // YASH

                ((QlSanPham)Application.OpenForms["QlSanPham"]).danhSachSP.LayDuLieu();

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
