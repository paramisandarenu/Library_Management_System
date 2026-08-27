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
    public partial class BorrowReturnForm : Form
    {
        SqlConnection conn = new SqlConnection(
            @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=D:\Parami\SUSL\SUSL 02nd Year 02nd Semester\CS Practical\Assignment\22APP5855 - M. A. P. S. Perera\LibraryManagementSystem\LibraryDB.mdf;Integrated Security=True"
            );
        public BorrowReturnForm()
        {
            InitializeComponent();
        }

        private void BorrowReturnForm_Load(object sender, EventArgs e)
        {
            LoadMembers();
            LoadBooks();
            LoadBorrowRecords();
        }

        private void LoadMembers()
        {
            conn.Open();

            SqlDataAdapter da = new SqlDataAdapter(
                "SELECT MemberID,MemberName FROM Members", conn);

            DataTable dt = new DataTable();

            da.Fill(dt);

            cmbMember.DataSource = dt;
            cmbMember.DisplayMember = "MemberName";
            cmbMember.ValueMember = "MemberID";

            conn.Close();
        }

        private void LoadBooks()
        {
            conn.Open();

            SqlDataAdapter da = new SqlDataAdapter(
                "SELECT BookID,BookTitle FROM Books", conn);

            DataTable dt = new DataTable();

            da.Fill(dt);

            cmbBook.DataSource = dt;
            cmbBook.DisplayMember = "BookTitle";
            cmbBook.ValueMember = "BookID";

            conn.Close();
        }

        private void LoadBorrowRecords()
        {
            conn.Open();

            SqlDataAdapter da = new SqlDataAdapter(

        @"SELECT BorrowID,
MemberName,
BookTitle,
BorrowDate,
DueDate,
ReturnDate,
ReturnStatus
FROM BorrowRecords
INNER JOIN Members
ON BorrowRecords.MemberID=Members.MemberID
INNER JOIN Books
ON BorrowRecords.BookID=Books.BookID", conn);

            DataTable dt = new DataTable();

            da.Fill(dt);

            dgvBorrow.DataSource = dt;

            conn.Close();
        }

        private void btnBorrow_Click(object sender, EventArgs e)
        {
            conn.Open();

            SqlCommand check = new SqlCommand(
                "SELECT AvailableCopies FROM Books WHERE BookID=@BookID", conn);

            check.Parameters.AddWithValue("@BookID", cmbBook.SelectedValue);

            int copies = Convert.ToInt32(check.ExecuteScalar());

            if (copies == 0)
            {
                MessageBox.Show("Book is not available.");

                conn.Close();
                return;
            }

            SqlCommand cmd = new SqlCommand(

        @"INSERT INTO BorrowRecords
(MemberID,BookID,BorrowDate,DueDate,ReturnStatus)

VALUES
(@MemberID,@BookID,@BorrowDate,@DueDate,'Not Returned')", conn);

            cmd.Parameters.AddWithValue("@MemberID", cmbMember.SelectedValue);
            cmd.Parameters.AddWithValue("@BookID", cmbBook.SelectedValue);
            cmd.Parameters.AddWithValue("@BorrowDate", DateTime.Now);
            cmd.Parameters.AddWithValue("@DueDate", DateTime.Now.AddDays(14));

            cmd.ExecuteNonQuery();

            SqlCommand update = new SqlCommand(

        "UPDATE Books SET AvailableCopies=AvailableCopies-1 WHERE BookID=@BookID", conn);

            update.Parameters.AddWithValue("@BookID", cmbBook.SelectedValue);

            update.ExecuteNonQuery();

            conn.Close();

            MessageBox.Show("Book Borrowed Successfully.");

            LoadBorrowRecords();
        }

        private void btnReturn_Click(object sender, EventArgs e)
        {
            if (dgvBorrow.CurrentRow == null)
            {
                MessageBox.Show("Select a borrow record.");
                return;
            }

            int borrowID = Convert.ToInt32(dgvBorrow.CurrentRow.Cells["BorrowID"].Value);

            conn.Open();

            SqlCommand cmd = new SqlCommand(

        @"UPDATE BorrowRecords
SET ReturnDate=@ReturnDate,
ReturnStatus='Returned'
WHERE BorrowID=@BorrowID", conn);

            cmd.Parameters.AddWithValue("@BorrowID", borrowID);
            cmd.Parameters.AddWithValue("@ReturnDate", DateTime.Now);

            cmd.ExecuteNonQuery();

            SqlCommand update = new SqlCommand(

        @"UPDATE Books
SET AvailableCopies=AvailableCopies+1
WHERE BookID=
(
SELECT BookID
FROM BorrowRecords
WHERE BorrowID=@BorrowID
)", conn);

            update.Parameters.AddWithValue("@BorrowID", borrowID);

            update.ExecuteNonQuery();

            conn.Close();

            MessageBox.Show("Book Returned.");

            LoadBorrowRecords();
        }

        private void btnsearch_Click(object sender, EventArgs e)
        {
            conn.Open();

            SqlDataAdapter da = new SqlDataAdapter(

        @"SELECT BorrowID,
MemberName,
BookTitle,
BorrowDate,
DueDate,
ReturnDate,
ReturnStatus
FROM BorrowRecords
INNER JOIN Members
ON BorrowRecords.MemberID=Members.MemberID
INNER JOIN Books
ON BorrowRecords.BookID=Books.BookID
WHERE ReturnStatus=@Status", conn);

            da.SelectCommand.Parameters.AddWithValue("@Status", cmbSearchStatus.Text);

            DataTable dt = new DataTable();

            da.Fill(dt);

            dgvBorrow.DataSource = dt;

            conn.Close();
        }


    }
}
