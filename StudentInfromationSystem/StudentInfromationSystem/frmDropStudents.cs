using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace StudentInfromationSystem
{
    public partial class frmDropStudents : Form
    {
        DBConnect conn = new DBConnect();
        StudentAddClass student = new StudentAddClass();
        public frmDropStudents()
        {
            InitializeComponent();
        }

        private void frmDropStudents_Load(object sender, EventArgs e)
        {
            // when form loads
            if (student.getTotalStudents() != 0)
            {
                btn_drop.Enabled = true;
            }
            else
            {
                btn_drop.Enabled = false;
            }
            cbSearchBy.SelectedIndex = 0;
            showTable();
        }

        private void btn_search_Click(object sender, EventArgs e)
        {
            searchStudent();
        }

        private void txt_Search_TextChanged(object sender, EventArgs e)
        {
            searchStudent();
        }

        private void btn_drop_Click(object sender, EventArgs e)
        {

            if (DataGridView_student.CurrentRow.Cells[0].Value.ToString() != "")
            {
                int id = (int)DataGridView_student.CurrentRow.Cells[0].Value;
                DialogResult dlg = MessageBox.Show("Are you sure you want to drop this student? ", "Drop student", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dlg == DialogResult.Yes)
                {
                    student.dropStudent(id);
                    MessageBox.Show("Student successfully dropped", "Drop student", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    showTable();
                }
            }
            else
            {
                MessageBox.Show("Select a student ", "Drop student", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }

        //function for search
        public DataTable getStudent(string search)
        {
            conn.openConnect();
            string searchBox = txt_Search.Text;
            string searchCmd = "SELECT * FROM tblStudentInfo WHERE " + search + " LIKE '" + searchBox + "%'";
            MySqlCommand cmd = new MySqlCommand(searchCmd, conn.getconnection);
            MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
            DataTable table = new DataTable();
            adapter.Fill(table);
            conn.closeConnect();
            return table;
        }

        public void searchStudent()
        {
            if (cbSearchBy.Text == "Last name")
            {
                DataGridView_student.DataSource = getStudent("last_name");
            }
            else if (cbSearchBy.Text == "First name")
            {
                DataGridView_student.DataSource = getStudent("first_name");
            }
            else if (cbSearchBy.Text == "Year")
            {
                DataGridView_student.DataSource = getStudent("school_year");
            }
            else if (cbSearchBy.Text == "Course")
            {
                DataGridView_student.DataSource = getStudent("course");
            }
            else if (cbSearchBy.Text == "Gender")
            {
                DataGridView_student.DataSource = getStudent("gender");
            }
        }

        public void showTable()
        {
            DataGridView_student.DataSource = student.getStudentList();

            DataGridViewImageColumn imageColumn = new DataGridViewImageColumn();
            imageColumn = (DataGridViewImageColumn)DataGridView_student.Columns[15];
            imageColumn.ImageLayout = DataGridViewImageCellLayout.Zoom;
        }

        private void btn_DropAll_Click(object sender, EventArgs e)
        {
            // for deleting all the students information
            DialogResult dlg = MessageBox.Show("Are you sure you want to delete all the student's information?", "Drop Students", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (dlg.Equals(DialogResult.Yes)) {
                student.dropAllStudents();
                showTable();
            }
        }
    }
}
