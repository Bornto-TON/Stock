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

namespace Stock
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtBoxUserName.Clear();
            txtBoxPassword.Clear();
            txtBoxUserName.Focus(); //Start blink cursor again at User name box
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            //TO-DO check login user name & password
            SqlConnection con = new SqlConnection("Data Source=.;Initial Catalog=Stock;Integrated Security=True");
            SqlDataAdapter sda = new SqlDataAdapter(@"SELECT * FROM [Stock].[dbo].[Login] Where UserName = '"+txtBoxUserName.Text+"' and Password = '"+txtBoxPassword.Text+"'", con);
            DataTable dt = new DataTable();
            sda.Fill(dt);

            //Check condition access
            if (dt.Rows.Count == 1 || dt.Rows.Count == 2 || dt.Rows.Count == 3)
            {
                this.Hide();
                StockMain main = new StockMain();
                main.Show(); //Go to stock main page
            }
            else
            {
                MessageBox.Show("Invalid User Name & Password...!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
