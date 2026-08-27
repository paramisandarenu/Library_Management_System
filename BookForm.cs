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
    public partial class BookForm : Form
    {
        SqlConnection conn = new SqlConnection(
            @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=D:\Parami\SUSL\SUSL 02nd Year 02nd Semester\CS Practical\Assignment\22APP5855 - M. A. P. S. Perera\LibraryManagementSystem\LibraryDB.mdf;Integrated Security=True"
            );
        public BookForm()
        {
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void LoadBooks()
        {
            try
            {
                conn.Open();

                SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Books", conn);

                DataTable dt = new DataTable();

                da.Fill(dt);

                dgvBooks.DataSource = dt;

                lblTotalTitles.Text = "Total Titles : " + dt.Rows.Count;

                SqlCommand cmd = new SqlCommand("SELECT SUM(AvailableCopies) FROM Books", conn);

                object total = cmd.ExecuteScalar();

                if (total == DBNull.Value)
                    lblTotalCopies.Text = "Total Copies : 0";
                else
                    lblTotalCopies.Text = "Total Copies : " + total.ToString();

                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
        }

        private void BookForm_Load(object sender, EventArgs e)
        {
            LoadBooks();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (txtBookTitle.Text == "" ||
        txtAuthor.Text == "" ||
        txtShelfLocation.Text == "" ||
        txtAvailableCopies.Text == "")
            {
                MessageBox.Show("Please fill all fields.");
                return;
            }

            if (!int.TryParse(txtAvailableCopies.Text, out int copies) || copies < 0)
            {
                MessageBox.Show("Available Copies must be a non-negative number.");
                return;
            }

            try
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(
                "INSERT INTO Books(BookTitle,Author,ShelfLocation,AvailableCopies) VALUES(@BookTitle,@Author,@Shelf,@Copies)", conn);

                cmd.Parameters.AddWithValue("@BookTitle", txtBookTitle.Text);
                cmd.Parameters.AddWithValue("@Author", txtAuthor.Text);
                cmd.Parameters.AddWithValue("@Shelf", txtShelfLocation.Text);
                cmd.Parameters.AddWithValue("@Copies", copies);

                cmd.ExecuteNonQuery();

                conn.Close();

                MessageBox.Show("Book added successfully.");

                LoadBooks();

                btnClear.PerformClick();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
        }

        private void dgvBooks_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvBooks.Rows[e.RowIndex];

                txtBookTitle.Text = row.Cells["BookTitle"].Value.ToString();
                txtAuthor.Text = row.Cells["Author"].Value.ToString();
                txtShelfLocation.Text = row.Cells["ShelfLocation"].Value.ToString();
                txtAvailableCopies.Text = row.Cells["AvailableCopies"].Value.ToString();
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (dgvBooks.CurrentRow == null)
            {
                MessageBox.Show("Please select a book.");
                return;
            }

            int id = Convert.ToInt32(dgvBooks.CurrentRow.Cells["BookID"].Value);

            conn.Open();

            SqlCommand cmd = new SqlCommand(
            "UPDATE Books SET BookTitle=@BookTitle,Author=@Author,ShelfLocation=@Shelf,AvailableCopies=@Copies WHERE BookID=@BookID", conn);

            cmd.Parameters.AddWithValue("@BookID", id);
            cmd.Parameters.AddWithValue("@BookTitle", txtBookTitle.Text);
            cmd.Parameters.AddWithValue("@Author", txtAuthor.Text);
            cmd.Parameters.AddWithValue("@Shelf", txtShelfLocation.Text);
            cmd.Parameters.AddWithValue("@Copies", txtAvailableCopies.Text);

            cmd.ExecuteNonQuery();

            conn.Close();

            MessageBox.Show("Book updated successfully.");

            LoadBooks();

            btnClear.PerformClick();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {

            if (dgvBooks.CurrentRow == null)
            {
                MessageBox.Show("Please select a book.");
                return;
            }

            DialogResult result = MessageBox.Show(
                "Delete this book?",
                "Confirm",
                MessageBoxButtons.YesNo);

            if (result == DialogResult.Yes)
            {
                int id = Convert.ToInt32(dgvBooks.CurrentRow.Cells["BookID"].Value);

                conn.Open();

                SqlCommand cmd = new SqlCommand(
                    "DELETE FROM Books WHERE BookID=@BookID", conn);

                cmd.Parameters.AddWithValue("@BookID", id);

                cmd.ExecuteNonQuery();

                conn.Close();

                MessageBox.Show("Book deleted.");

                LoadBooks();

                btnClear.PerformClick();
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            conn.Open();

            SqlDataAdapter da = new SqlDataAdapter(
            "SELECT * FROM Books WHERE BookTitle LIKE @Search OR Author LIKE @Search", conn);

            da.SelectCommand.Parameters.AddWithValue("@Search", "%" + txtSearch.Text + "%");

            DataTable dt = new DataTable();

            da.Fill(dt);

            dgvBooks.DataSource = dt;

            conn.Close();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtBookTitle.Clear();
            txtAuthor.Clear();
            txtShelfLocation.Clear();
            txtAvailableCopies.Clear();
            txtSearch.Clear();

            txtBookTitle.Focus();
        }
    }
}
