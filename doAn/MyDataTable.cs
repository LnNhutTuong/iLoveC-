using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace doAn
{
    internal class MyDataTable : DataTable
    {
        SqlConnection connection;
        SqlDataAdapter adapter;
        SqlCommand command;


        //Taoj chuooxi keets noosi
        public string ConnectionString()
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();

            builder["Server"] = ".\\SQLEXPRESS";
            builder["Database"] = "QLCuaHangQuanAo";
            builder["Integrated Security"] = "True";

            return builder.ConnectionString;
        }

        //Senkaimon
        public bool OpenConnection()
        {
            try
            {
                if(connection == null)
                {
                    connection = new SqlConnection(ConnectionString());
                }
                if(connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                return true;
            }
            catch
            {
                connection.Close();
                return false;
            }
        }

        //Chay Select
        public void Fill(SqlCommand selectCommand)
        {
            command = selectCommand;

            try
            {
                command.Connection = connection;
                adapter = new SqlDataAdapter();
                adapter.SelectCommand = command;
                this.Clear();
                adapter.Fill(this);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi:" + ex.Message, "Lỗi truy vấn", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //chay Insert, Update, Delete
        public int Update(SqlCommand insertUpdateDeleteCommand)
        {
            int result = 0;
            SqlTransaction transaction = null;

            try
            {
                transaction = connection.BeginTransaction();

                insertUpdateDeleteCommand.Connection = connection;
                insertUpdateDeleteCommand.Transaction = transaction;
                result = insertUpdateDeleteCommand.ExecuteNonQuery();

                this.AcceptChanges();
                transaction.Commit();
            }
            catch (Exception ex)
            {
                if (transaction == null)
                {
                    transaction.Rollback();
                }
                MessageBox.Show("Lỗi:" + ex.Message, "Lỗi truy vấn", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return result;
        }
    }
}
