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
    public partial class RegisterForm : Form
    {
        SqlConnection conn = new SqlConnection(
            @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=D:\Parami\SUSL\SUSL 02nd Year 02nd Semester\CS Practical\Assignment\22APP5855 - M. A. P. S. Perera\LibraryManagementSystem\LibraryDB.mdf;Integrated Security=True"
            );
        public RegisterForm()
        {
            InitializeComponent();
        }

        private void RegisterForm_Load(object sender, EventArgs e)
        {

        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            if (txtUsername.Text == "" ||  txtPassword.Text == "" || txtConfirmPassword.Text == "")
            {
                MessageBox.Show("Please fill all fields.");
                return;
            }
            if (txtPassword.Text != txtConfirmPassword.Text)
            {
                MessageBox.Show("Password do not match.");
                txtConfirmPassword.Clear();
                txtConfirmPassword.Focus();
                return;
            }
            try
            {
                conn.Open();
                SqlCommand checkCmd = new SqlCommand(
                    "SELECT COUNT(*) FROM Users WHERE Username=@Username", conn
                    );

                checkCmd.Parameters.AddWithValue("@Username", txtUsername.Text);

                int count = (int)checkCmd.ExecuteScalar();
                if (count > 0)
                {
                    MessageBox.Show("Username already exists.");
                    conn.Close();
                    return;
                }
                SqlCommand cmd = new SqlCommand(
                    "INSERT INTO Users (Username,  Password) VALUES(@Username,@Password)", conn);

                cmd.Parameters.AddWithValue("@Username", txtUsername.Text);
                cmd.Parameters.AddWithValue("@Password", txtPassword.Text);

                cmd.ExecuteNonQuery();

                MessageBox.Show("Registration Successful.");

                txtUsername.Clear();
                txtPassword.Clear();
                txtConfirmPassword.Clear();

                txtUsername.Focus();

                conn.Close();

            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);

                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
        }

        private void linkLogin_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            LoginForm login = new LoginForm();
            login.Show();

            this.Hide();
        }

        
    }
}
