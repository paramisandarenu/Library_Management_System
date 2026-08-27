using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LibraryManagementSystem
{
    public partial class DashboardForm : Form
    {
        public DashboardForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void btnMembers_Click(object sender, EventArgs e)
        {
            MemberForm member = new MemberForm();
            member.Show();
        }

        private void btnBooks_Click(object sender, EventArgs e)
        {
            BookForm book = new BookForm();
            book.Show();
        }

        private void btnBorrow_Click(object sender, EventArgs e)
        {
            BorrowReturnForm borrow = new BorrowReturnForm();
            borrow.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Do you want to logout?", "Logout", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {

                LoginForm login = new LoginForm();
                login.Show();
                this.Close();
            }
        }
    }
}
