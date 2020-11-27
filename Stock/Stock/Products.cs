using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Stock
{
    public partial class Products : Form
    {
        public Products()
        {
            InitializeComponent();
        }
        //**************************************************************
        private void Products_Load(object sender, EventArgs e)
        {
            comboBoxStatus.SelectedIndex = 0;
            //Reading data
            loadData();
        }
        //**************************************************************
        private void btnAdd_Click(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection("Data Source=.;Initial Catalog=Stock;Integrated Security=True");
            //Insert logic
            con.Open();
            bool status = false;
            if (comboBoxStatus.SelectedIndex == 0)
            {
                status = true;
            }
            else
            {
                status = false;
            }

            var sqlQuery = "";
            if (IfProductsExists(con, txtBoxProductCode.Text)) //Update
            {
                sqlQuery = @"UPDATE [Products] SET [ProductName] = '" +txtBoxProductName.Text+ "' ,[ProductStatus] = '" +status+ "' WHERE [ProductCode] = '" +txtBoxProductCode.Text+ "'";
            }
            else //Insert
            {
                sqlQuery = @"INSERT INTO [Stock].[dbo].[Products]([ProductCode], [ProductName], [ProductStatus])VALUES
                            ('" +txtBoxProductCode.Text+ "', '" +txtBoxProductName.Text+ "', '" + status + "')";
            }

            SqlCommand cmd = new SqlCommand(sqlQuery, con);
            cmd.ExecuteNonQuery();
            con.Close();
            //Reading data
            loadData();
        }
        //**************************************************************
        private bool IfProductsExists(SqlConnection con, string ProductCode)
        {
            SqlDataAdapter sda = new SqlDataAdapter("Select 1 From [Products] WHERE [ProductCode] = '" + ProductCode+ "'", con);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            if (dt.Rows.Count > 0)
                return true;
            else
                return false;
        }
        //**************************************************************
        private void loadData()
        {
            SqlConnection con = new SqlConnection("Data Source=.;Initial Catalog=Stock;Integrated Security=True");
            SqlDataAdapter sda = new SqlDataAdapter("Select * From [Stock].[dbo].[Products]", con);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            dataGridView.Rows.Clear();
            foreach (DataRow item in dt.Rows)
            {
                int n = dataGridView.Rows.Add();
                dataGridView.Rows[n].Cells[0].Value = item["ProductCode"].ToString();
                dataGridView.Rows[n].Cells[1].Value = item["ProductName"].ToString();
                if ((bool)item["ProductStatus"])
                {
                    dataGridView.Rows[n].Cells[2].Value = "Active";
                }
                else
                {
                    dataGridView.Rows[n].Cells[2].Value = "Deactive";
                }
            }
        }
        //**************************************************************
        //Read data to select show on text box
        private void dataGridView_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            txtBoxProductCode.Text = dataGridView.SelectedRows[0].Cells[0].Value.ToString();
            txtBoxProductName.Text = dataGridView.SelectedRows[0].Cells[1].Value.ToString();
            if (dataGridView.SelectedRows[0].Cells[2].Value.ToString() == "Active")
            {
                comboBoxStatus.SelectedIndex = 0;
            }
            else
            {
                comboBoxStatus.SelectedIndex = 1;
            }
        }
        //**************************************************************
        private void btnDelete_Click(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection("Data Source=.;Initial Catalog=Stock;Integrated Security=True");
            var sqlQuery = "";
            if (IfProductsExists(con, txtBoxProductCode.Text)) //Update
            {
                con.Open();
                sqlQuery = @"DELETE FROM [Products] WHERE [ProductCode] = '" + txtBoxProductCode.Text + "'";
                SqlCommand cmd = new SqlCommand(sqlQuery, con);
                cmd.ExecuteNonQuery();
                con.Close();
            }
            else //Insert
            {
                MessageBox.Show("Record Not Exists...!", "Select Row", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            //Reading data
            loadData();
        }
    }
}
