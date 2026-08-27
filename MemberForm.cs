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
    public partial class MemberForm : Form
    {
        SqlConnection conn = new SqlConnection(
            @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=D:\Parami\SUSL\SUSL 02nd Year 02nd Semester\CS Practical\Assignment\22APP5855 - M. A. P. S. Perera\LibraryManagementSystem\LibraryDB.mdf;Integrated Security=True"
            );
        public MemberForm()
        {
            InitializeComponent();
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void MemberForm_Load(object sender, EventArgs e)
        {
            LoadMembers();
        }

        private void LoadMembers()
        {
            try
            {
                conn.Open();

                SqlDataAdapter da = new SqlDataAdapter(
                    "SELECT * FROM Members", conn);

                DataTable dt = new DataTable();
                da.Fill(dt);

                dgvMembers.DataSource = dt;

                conn.Close();

                lblTotal.Text = "Total Members : " + dt.Rows.Count;

            }

            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);

                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (txtMemberName.Text == "" ||
        cmbMemberType.Text == "" ||
        txtContact.Text == "" ||
        txtEmail.Text == "" ||
        cmbStatus.Text == "")
            {
                MessageBox.Show("Please fill all fields.");
                return;
            }

            if (!long.TryParse(txtContact.Text, out _))
            {
                MessageBox.Show("Contact Number must contain only digits.");
                txtContact.Focus();
                return;
            }

            try
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(
                    "INSERT INTO Members(MemberName, MemberType, ContactNumber, Email, Status) VALUES (@MemberName, @MemberType, @ContactNumber, @Email, @Status)",
                    conn);

                cmd.Parameters.AddWithValue("@MemberName", txtMemberName.Text);
                cmd.Parameters.AddWithValue("@MemberType", cmbMemberType.Text);
                cmd.Parameters.AddWithValue("@ContactNumber", txtContact.Text);
                cmd.Parameters.AddWithValue("@Email", txtEmail.Text);
                cmd.Parameters.AddWithValue("@Status", cmbStatus.Text);

                cmd.ExecuteNonQuery();

                conn.Close();

                MessageBox.Show("Member added successfully.");

                LoadMembers();

                btnClear.PerformClick();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
        }

        private void dgvMembers_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvMembers.Rows[e.RowIndex];

                txtMemberName.Text = row.Cells["MemberName"].Value.ToString();
                cmbMemberType.Text = row.Cells["MemberType"].Value.ToString();
                txtContact.Text = row.Cells["ContactNumber"].Value.ToString();
                txtEmail.Text = row.Cells["Email"].Value.ToString();
                cmbStatus.Text = row.Cells["Status"].Value.ToString();
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (dgvMembers.CurrentRow == null)
            {
                MessageBox.Show("Please select a member.");
                return;
            }

            int id = Convert.ToInt32(dgvMembers.CurrentRow.Cells["MemberID"].Value);

            try
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(
                "UPDATE Members SET MemberName=@MemberName, MemberType=@MemberType, ContactNumber=@ContactNumber, Email=@Email, Status=@Status WHERE MemberID=@MemberID", conn);

                cmd.Parameters.AddWithValue("@MemberID", id);
                cmd.Parameters.AddWithValue("@MemberName", txtMemberName.Text);
                cmd.Parameters.AddWithValue("@MemberType", cmbMemberType.Text);
                cmd.Parameters.AddWithValue("@ContactNumber", txtContact.Text);
                cmd.Parameters.AddWithValue("@Email", txtEmail.Text);
                cmd.Parameters.AddWithValue("@Status", cmbStatus.Text);

                cmd.ExecuteNonQuery();

                conn.Close();

                MessageBox.Show("Member updated successfully.");

                LoadMembers();

                btnClear.PerformClick();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if(dgvMembers.CurrentRow == null)
    {
                MessageBox.Show("Please select a member.");
                return;
            }

            DialogResult result = MessageBox.Show(
                "Are you sure you want to delete this member?",
                "Confirm Delete",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                int id = Convert.ToInt32(dgvMembers.CurrentRow.Cells["MemberID"].Value);

                try
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(
                        "DELETE FROM Members WHERE MemberID=@MemberID", conn);

                    cmd.Parameters.AddWithValue("@MemberID", id);

                    cmd.ExecuteNonQuery();

                    conn.Close();

                    MessageBox.Show("Member deleted successfully.");

                    LoadMembers();

                    btnClear.PerformClick();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);

                    if (conn.State == ConnectionState.Open)
                        conn.Close();
                }
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                conn.Open();

                SqlDataAdapter da = new SqlDataAdapter(
                "SELECT * FROM Members WHERE MemberName LIKE @Search OR MemberType LIKE @Search", conn);

                da.SelectCommand.Parameters.AddWithValue("@Search", "%" + txtsearch.Text + "%");

                DataTable dt = new DataTable();

                da.Fill(dt);

                dgvMembers.DataSource = dt;

                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtMemberName.Clear();
            txtContact.Clear();
            txtEmail.Clear();
            txtsearch.Clear();

            cmbMemberType.SelectedIndex = -1;
            cmbStatus.SelectedIndex = -1;

            txtMemberName.Focus();
        }


    }
}
