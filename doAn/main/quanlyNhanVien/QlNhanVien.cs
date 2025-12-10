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
using BC = BCrypt.Net.BCrypt;

namespace doAn.quanLyNguoIDung
{
    public partial class QlNhanVien : Form
    {
        public string NameNhanVien {  get; set; }

        private BindingSource data = new BindingSource();
        MyDataTable dataTable = new MyDataTable();


        string maNhanVien = "";
        string hashCu = ""; 
        public QlNhanVien()
        {
            InitializeComponent();
            dataTable.OpenConnection();
            txtMatKhau.UseSystemPasswordChar = true;
        }

        public void LayDuLieu()
        {
            dataTable.Clear();
            
            MyDataTable chucVu = new MyDataTable();
            chucVu.OpenConnection();

            //Chucvu
            string chucVuSql = "SELECT *FROM ChucVu WHERE MaChucVu <> 'AD'";
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

            string nhanVienSql = "SELECT * FROM NhanVien WHERE MaNhanVien <> 'ADMIN'";
            SqlCommand nhanVienCmd = new SqlCommand(nhanVienSql);
            dataTable.Fill(nhanVienCmd);

            //gan du lieu vao nguon
            data.DataSource = dataTable;

            //gan nguon du lieu vao bang
            dataGridView.DataSource = data;


            txtMaNhanVien.DataBindings.Clear();
            txtTenNhanVien.DataBindings.Clear();
            txtMatKhau.DataBindings.Clear();
            txtSoDienThoai.DataBindings.Clear();
            txtEmail.DataBindings.Clear();
            cboChucVu.DataBindings.Clear();

            txtMaNhanVien.DataBindings.Add("Text", data, "MaNhanVien");
            txtTenNhanVien.DataBindings.Add("Text", data, "TenNhanVien");
            //txtMatKhau.DataBindings.Add("Text", data, "MatKhau");
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

            lblMatKhau.Visible = false;
            txtMatKhau.Visible = false;

            txtSoDienThoai.Enabled = value;
            txtEmail.Enabled = value;
            cboChucVu.Enabled = value;

            btnAnHienMatKhau.Visible = false;

            btnThem.Enabled = !value;
            btnSua.Enabled = !value;
            btnXoa.Enabled = !value;

            btnLuu.Enabled = value;
            btnTaiLai.Enabled = value;

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
            //txtMatKhau.PasswordChar = '*';
            showPass = false;
            OnOff(false);
        }

        bool showPass = false;

        List<string> maTonTai = new List<string>();

        private void btnLuu_Click(object sender, EventArgs e)
        {

            MyDataTable nhanVien = new MyDataTable();
            nhanVien.OpenConnection();
            SqlCommand nhanVienCmd = new SqlCommand("Select MaNhanVien,MatKhau FROM NhanVien");
            nhanVien.Fill(nhanVienCmd);

            foreach(DataRow row in nhanVien.Rows)
            {
                string ma = row["MaNhanVien"].ToString();
                maTonTai.Add(ma);
            }

            lblMatKhau.Visible = true;
            txtMatKhau.Visible = true;
            txtMatKhau.PasswordChar = '•';
            if (txtMaNhanVien.Text.Trim() == "")
            {
                MessageBox.Show("Mã nhân viên không được bỏ trống!", "LỖI",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (txtMaNhanVien.Text.Length != 5)
            {
                MessageBox.Show("Mã nhân viên phải đủ 5!", "LỖI",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (txtTenNhanVien.Text.Trim()=="")
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
            else if (txtSoDienThoai.Text.Length != 10)
            {
                MessageBox.Show("Số điện thoại phải là 10 số \n" + "Bạn đã nhập: " + txtSoDienThoai.Text.Length + " số!", "LỖI",
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
                        if (maTonTai.Contains(txtMaNhanVien.Text.ToUpper().Trim()))
                        {
                            MessageBox.Show("Mã này đã tồn tại!!");
                            return;
                        }
                        else
                        {
                            string sql = @"INSERT INTO NhanVien
                                       VALUES (@MaNhanVien,@MaChucVu, @TenNhanVien, @MatKhau, @Sdt, @Email)";

                            SqlCommand cmd = new SqlCommand(sql);

                            cmd.Parameters.Add("@MaNhanVien", SqlDbType.NVarChar, 5).Value = txtMaNhanVien.Text.ToUpper();
                            cmd.Parameters.Add("@MaChucVu", SqlDbType.NVarChar, 5).Value = cboChucVu.SelectedValue.ToString();
                            cmd.Parameters.Add("@TenNhanVien", SqlDbType.NVarChar, 50).Value = txtTenNhanVien.Text;
                            cmd.Parameters.Add("@MatKhau", SqlDbType.NVarChar, 100).Value = BC.HashPassword("123456");
                            cmd.Parameters.Add("@Sdt", SqlDbType.NVarChar, 11).Value = txtSoDienThoai.Text;
                            cmd.Parameters.Add("@Email", SqlDbType.NVarChar, 50).Value = txtEmail.Text;

                            MessageBox.Show("Thêm thành công!", "Thông báo",
                            MessageBoxButtons.OK);

                            dataTable.Update(cmd);
                        }                         
                    }
                    else
                    {
             
                        string matKhauMoi;
                        hashCu = dataTable.Rows[data.Position]["MatKhau"].ToString();

                        if (txtMatKhau.Text.Trim() == "")
                        {
                            // ko doi -> hash cu
                            matKhauMoi = hashCu;
                        }
                        else
                        {
                            // doi -> hash moi
                            matKhauMoi = BC.HashPassword(txtMatKhau.Text);
                        }

                        string sql = @"UPDATE NhanVien
                                       SET MaNhanVien = @MaNhanVienMoi,
                                            TenNhanVien = @TenNhanVien,
                                            MatKhau = @MatKhau,
                                            MaChucVu = @MaChucVu,
                                            Sdt = @Sdt,
                                            Email = @Email 
                                       WHERE MaNhanVien = @MaNhanVienCu";

                            SqlCommand cmd = new SqlCommand(sql);

                            cmd.Parameters.Add("@MaNhanVienMoi", SqlDbType.NVarChar, 5).Value = txtMaNhanVien.Text.ToUpper();
                            cmd.Parameters.Add("@MaNhanVienCu", SqlDbType.NVarChar, 5).Value = dataGridView.CurrentRow.Cells["MaNhanVie"].Value.ToString(); ;
                            cmd.Parameters.Add("@MaChucVu", SqlDbType.NVarChar, 5).Value = cboChucVu.SelectedValue.ToString();
                            cmd.Parameters.Add("@TenNhanVien", SqlDbType.NVarChar, 50).Value = txtTenNhanVien.Text;
                            cmd.Parameters.Add("@MatKhau", SqlDbType.NVarChar, 100).Value = matKhauMoi;
                            cmd.Parameters.Add("@Sdt", SqlDbType.NVarChar, 11).Value = txtSoDienThoai.Text;
                            cmd.Parameters.Add("@Email", SqlDbType.NVarChar, 50).Value = txtEmail.Text;

                            dataTable.Update(cmd);

                            MessageBox.Show("Sửa thành công !!");
                                             
                    }
                    Main_Load(sender, e);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            DialogResult kq;
            kq = MessageBox.Show("Bạn có muốn xóa nhân viên này " + txtTenNhanVien.Text + " không?", "Xóa",
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

        private void btnTaiLai_Click(object sender, EventArgs e)
        {
            txtMatKhau.PasswordChar = '•';
            Main_Load(sender, e);

        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            maNhanVien = txtMaNhanVien.Text;
            //txtMatKhau.PasswordChar = '\0';
            OnOff(true);
            lblMatKhau.Visible = true;
            txtMatKhau.Visible = true;
            btnAnHienMatKhau.Visible = true;

            txtMatKhau.Text = "";

        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            maNhanVien = "";

            txtMaNhanVien.Clear();
            txtTenNhanVien.Clear();
            txtSoDienThoai.Clear();
            txtEmail.Clear();
            cboChucVu.Text = "";

            txtMaNhanVien.Focus();

            OnOff(true);
        }

        private void btnAnHienMatKhau_Click(object sender, EventArgs e)
        {
            if (txtMatKhau.UseSystemPasswordChar)
            {
                txtMatKhau.UseSystemPasswordChar = false;
                btnAnHienMatKhau.BackgroundImage = Properties.Resources.visible;
            }
            else
            {
                txtMatKhau.UseSystemPasswordChar = true;
                btnAnHienMatKhau.BackgroundImage = Properties.Resources.hide;
            }
        }

        private void dataGridView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (!showPass)
            {
                if (dataGridView.Columns[e.ColumnIndex].Name == "MatKhau" && e.Value != null)
                {
                    e.Value = "••••••••••";
                    e.FormattingApplied = true;
                }
            }
        }
    }
}
