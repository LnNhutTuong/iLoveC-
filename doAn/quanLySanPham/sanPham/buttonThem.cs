using doAn.popUp.quanlySanPham.sanPham;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace doAn.quanLySanPham.sanPham
{
    public partial class ThemButton : UserControl
    {
        //DAC BIET
        //BINDING LA DUONG DAN DU LIEU
        //Binding ben day la dung cho cbo
        //Binding nhung thang kia thi dung cho dataGridView va cbo
        private BindingSource data = new BindingSource();
        MyDataTable dataTable = new MyDataTable();

        public ThemButton()
        {
            InitializeComponent();
            dataTable.OpenConnection();

            LayDuLieu();
        }

        public void LayDuLieu()
        {
            dataTable.Clear();
            
            //duong dan du lieu di tu nguon`: DATABASE to cBo
            data.DataSource = dataTable;

            //dataGridView
            dataTable.OpenConnection();

            string danhMucSql = @" SELECT *                                       
                                    FROM SanPham";
            SqlCommand danhMucSqlCmd = new SqlCommand(danhMucSql);
            dataTable.Fill(danhMucSqlCmd);


            //gan du lieu vao nguon

            //gan nguon du lieu vao bang

            //foreach (DataTable col in dataTable.Columns)
            //{
            //    Console.WriteLine(col.Columns.Count);
            //}

        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            Them puThem = new Them(data);
            puThem.ShowDialog();
        }
    }
}
