using doAn.popUp.quanlySanPham.sanPham;
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
                    sp._mode = "view";
                    sp.setData(row["TenSanPham"].ToString(), row["AnhDaiDien"].ToString());

                    // += them su kien (1) 
                    // (s,e) => giong giong function arrow js (2)
                    // 1 + 2 = tu tao cai su kien ben UC roi nen qua day goi ra
                    // (1) se hoat dong khi su kien duoc goi thanh cong
                    // va chay (2)
                    sp.XemChiTiet += (s, e) =>
                    {
                        ChiTietSanPham ct = new ChiTietSanPham(sp.MaSanPham);
                        ct.ShowDialog();
                    };  

                    flowLayoutPanel.Controls.Add(sp);
                }
            }
        }

        private void Sp_XemChiTiet(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void DanhSachSP_Load(object sender, System.EventArgs e)
        {
            LayDuLieu();
        }
    }
}
