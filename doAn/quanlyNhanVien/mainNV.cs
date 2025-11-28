using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace doAn.quanLyNguoIDung
{
    public partial class mainNV : Form
    {


        private BindingSource data = new BindingSource();
        MyDataTable dataTable = new MyDataTable();


        string maNhanVien = "";

        public mainNV()
        {
            InitializeComponent();
            dataTable.OpenConnection();
        }

        public void LayDuLieu()
        {
            dataTable.Clear();
          
            MyDataTable chucVu = new MyDataTable();
            chucVu.OpenConnection();

            //Chucvu
            string chucVuSql = "SELECT *FROM ChucVu";
            SqlCommand cmd = new SqlCommand(chucVuSql);
            chucVu.Fill(cmd);

            //gridview
            var showName = (DataGridViewComboBoxColumn)dataGridView.Columns["ChucVu"];

            showName.DataSource = chucVu;
            showName.DisplayMember = "TenChucVu";
            showName.ValueMember = "MaChucVu";

            showName.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;

            //Nguon
            cboChucVu.DataSource = chucVu;

            //Hien ra
            cboChucVu.DisplayMember = "TenChucVu";
            //Value
            cboChucVu.ValueMember = "MaChucVu";

            string nhanVienSql = "SELECT * FROM NhanVien";
            SqlCommand nhanVienCmd = new SqlCommand(nhanVienSql);
            dataTable.Fill(nhanVienCmd);         

            //gan du lieu vao nguon
            data.DataSource = dataTable;

            //gan nguon du lieu vao bang
            dataGridView.DataSource = data;


            txtMaNhanVien.DataBindings.Clear();
            txtTenNhanVien.DataBindings.Clear();
            txtSoDienThoai.DataBindings.Clear();
            txtEmail.DataBindings.Clear();
            cboChucVu.DataBindings.Clear();

            txtMaNhanVien.DataBindings.Add("Text", data, "MaNhanVien");
            txtTenNhanVien.DataBindings.Add("Text", data, "TenNhanVien");
            txtSoDienThoai.DataBindings.Add("Text", data, "Sdt");
            txtEmail.DataBindings.Add("Text", data, "Email");
            cboChucVu.DataBindings.Add("SelectedValue", data, "MaChucVu");


            //foreach (DataTable col in dataTable.Columns)
            //{
            //    Console.WriteLine(col.Columns.Count);
            //}
        }

        public void OnOff(bool value)
        {
            txtMaNhanVien.Enabled = value;
            txtTenNhanVien.Enabled = value;
            txtSoDienThoai.Enabled = value;
            txtEmail.Enabled = value;
            cboChucVu.Enabled = value;


            btnThem.Enabled = !value;
            btnSua.Enabled = !value;
            btnXoa.Enabled = !value;

            btnTaiLai.Enabled = value;
            btnLuu.Enabled = value;

        }

        public bool KiemTraEmail(string email)
        {
            try
            {
                //lan1
                MailAddress m = new MailAddress(email);

                //lan2
                return m.Address == email;
            }
            catch (FormatException)
            {
                return false;
            }
        }

        private void Main_Load(object sender, EventArgs e)
        {
            LayDuLieu();
            OnOff(false);
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            maNhanVien = "";
            maNhanVien = "";

            txtMaNhanVien.Clear();
            txtTenNhanVien.Clear();
            txtSoDienThoai.Clear();
            txtEmail.Clear();
            cboChucVu.Text = "";

            txtMaNhanVien.Focus();

            OnOff(true);
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            maNhanVien = txtMaNhanVien.Text;

            OnOff(true);
        }

        private void btnTaiLai_Click(object sender, EventArgs e)
        {
            Main_Load(sender, e);
        }

        private void btnXoa_Click_1(object sender, EventArgs e)
        {
            DialogResult kq;
            kq = MessageBox.Show("Bạn có muốn xóa thương hiệu này " + txtTenNhanVien.Text + " không?", "Xóa",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (kq == DialogResult.Yes)
            {
                string sql = @"DELETE FROM NhanVien WHERE MaNhanVien = @MaNhanVien";

                SqlCommand cmd = new SqlCommand(sql);

                cmd.Parameters.Add("@MaNhanVien", SqlDbType.NVarChar, 5).Value = txtMaNhanVien.Text;

                dataTable.Update(cmd);

                Main_Load(sender, e);
            }
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            if (txtMaNhanVien.Text.Trim() == "")
            {
                MessageBox.Show("Mã nhân viên không được bỏ trống!", "LỖI",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (txtTenNhanVien.Text.Trim() == "")
            {
                MessageBox.Show("Tên nhân viên không được bỏ trống!", "LỖI",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            //------------So Dien Thoai
            else if (txtSoDienThoai.Text.Trim() == "")
            {
                MessageBox.Show("Số điện thoại không được bỏ trống!", "LỖI",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (txtSoDienThoai.Text.Length > 10 || txtSoDienThoai.Text.Length < 10)
            {
                MessageBox.Show("Số điện thoại phải là 11 số \n" + "Bạn đã nhập: " + txtSoDienThoai.Text.Length + " số!", "LỖI",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (!txtSoDienThoai.Text.StartsWith("0"))
            {
                MessageBox.Show("Số điện thoại phải bắt đầu từ số 0", "LỖI",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            //------------EMAIL
            else if (txtEmail.Text.Trim() == "")
            {
                MessageBox.Show("Email không được bỏ trống ", "LỖI",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (KiemTraEmail(txtEmail.Text) == false)
            {
                MessageBox.Show("Email không đúng định dạng ", "LỖI",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                try
                {
                    //Them moi
                    if (maNhanVien == "")
                    {
                        string sql = @"INSERT INTO NhanVien
                                       VALUES (@MaNhanVien,@MaChucVu, @TenNhanVien, @MatKhau, @Sdt, @Email)";

                        SqlCommand cmd = new SqlCommand(sql);

                        cmd.Parameters.Add("@MaNhanVien", SqlDbType.NVarChar, 5).Value = txtMaNhanVien.Text;
                        cmd.Parameters.Add("@MaChucVu", SqlDbType.NVarChar, 5).Value = cboChucVu.SelectedValue.ToString();
                        cmd.Parameters.Add("@TenNhanVien", SqlDbType.NVarChar, 50).Value = txtTenNhanVien.Text;
                        cmd.Parameters.Add("@MatKhau", SqlDbType.NVarChar, 50).Value = "123456";
                        cmd.Parameters.Add("@Sdt", SqlDbType.NVarChar, 11).Value = txtSoDienThoai.Text;
                        cmd.Parameters.Add("@Email", SqlDbType.NVarChar, 50).Value = txtEmail.Text;

                        MessageBox.Show("Thêm thành công!", "Thông báo",
                        MessageBoxButtons.OK);

                        dataTable.Update(cmd);
                    }
                    else
                    {
                        string sql = @"UPDATE NhanVien
                                       SET MaNhanVien = @MaNhanVienMoi,
                                            TenNhanVien = @TenNhanVien,
                                            MaChucVu = @MaChucVu,
                                            Sdt = @Sdt,
                                            Email = @Email 
                                       WHERE MaNhanVien = @MaNhanVienCu";

                        SqlCommand cmd = new SqlCommand(sql);

                        cmd.Parameters.Add("@MaNhanVienMoi", SqlDbType.NVarChar, 5).Value = txtMaNhanVien.Text;
                        cmd.Parameters.Add("@MaNhanVienCu", SqlDbType.NVarChar, 5).Value = maNhanVien;
                        cmd.Parameters.Add("@MaChucVu", SqlDbType.NVarChar, 5).Value = cboChucVu.SelectedValue.ToString();
                        cmd.Parameters.Add("@TenNhanVien", SqlDbType.NVarChar, 50).Value = txtTenNhanVien.Text;
                        cmd.Parameters.Add("@Sdt", SqlDbType.NVarChar, 11).Value = txtSoDienThoai.Text;
                        cmd.Parameters.Add("@Email", SqlDbType.NVarChar, 50).Value = txtEmail.Text;

                        dataTable.Update(cmd);
                    }
                    Main_Load(sender, e);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dataGridView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dataGridView.Columns[e.ColumnIndex].Name == "MatKhau")
            {
                e.Value = "••••••••••";
            }
        }

        private void toolStripStatusLabel1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
