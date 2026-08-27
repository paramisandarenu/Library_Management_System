using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace LibraryManagementSystem
{
    public partial class LoginForm : Form
    {
        SqlConnection conn = new SqlConnection(
            @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=D:\Parami\SUSL\SUSL 02nd Year 02nd Semester\CS Practical\Assignment\22APP5855 - M. A. P. S. Perera\LibraryManagementSystem\LibraryDB.mdf;Integrated Security=True"
            );
        public LoginForm()
        {
            InitializeComponent();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {

        }

        private void checkShowPassword_CheckedChanged(object sender, EventArgs e)
        {

            if (checkShowPassword.Checked)
            {
                txtPassword.UseSystemPasswordChar = false;
            }
            else
            {
                txtPassword.UseSystemPasswordChar = true;
            }
            
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (txtUsername.Text == "" || txtPassword.Text == "")
            {
                MessageBox.Show("Please enter username and password.");
                return;
            }

            try
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(
                "SELECT COUNT(*) FROM Users WHERE Username=@Username AND Password=@Password", conn);

                cmd.Parameters.AddWithValue("@Username", txtUsername.Text);
                cmd.Parameters.AddWithValue("@Password", txtPassword.Text);

                int count = (int)cmd.ExecuteScalar();

                if (count > 0)
                {
                    MessageBox.Show("Login Successful.");

                    DashboardForm dashboard = new DashboardForm();
                    dashboard.Show();

                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Invalid Username or Password.");

                    txtPassword.Clear();
                    txtPassword.Focus();
                }

                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
            }

        private void linkRegister_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            RegisterForm register = new RegisterForm();
            register.Show();

            this.Hide();
        }
    }
}
