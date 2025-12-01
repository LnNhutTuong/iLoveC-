using doAn.quanLySanPham;
using doAn.quanLySanPham.sanPham;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace doAn.quanLySanPham
{
    public partial class DanhSachSP : UserControl
    {

        public DanhSachSP()
        {
            InitializeComponent();
        }

        public void LayDuLieu()
        {
            flowLayoutPanel.Controls.Clear();


            ThemButton them = new ThemButton();
            them.Margin = new Padding(5, 65, 0, 0);
            flowLayoutPanel.Controls.Add(them);



            //QUAN TRONG
            //tai sao? Vi quy luat 2 ban tay
            //1 thang gia va 1 thang that. Thang gia la thang ds chay sau khi them san pham,
            //thang that la ds san pham ban dau

            MyDataTable dt = new MyDataTable();
            if (dt.OpenConnection())
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM SanPham");
                dt.Fill(cmd);
                
                foreach (DataRow row in dt.Rows)
                {
                    SanPham sp = new SanPham();

                    sp.MaSanPham = row["MaSanPham"].ToString();

                    sp.setData(row["TenSanPham"].ToString(), row["AnhDaiDien"].ToString());

                    flowLayoutPanel.Controls.Add(sp);
                }
            }
        }

        private void DanhSachSP_Load(object sender, System.EventArgs e)
        {
            LayDuLieu();
        }
    }
}
