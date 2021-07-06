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
using DGVPrinterHelper;


namespace StudentInfromationSystem
{
    public partial class frmViewTable : Form
    {
        DBConnect conn = new DBConnect();
        StudentAddClass student = new StudentAddClass();
        DGVPrinter printer = new DGVPrinter();
        public frmViewTable()
        {
            InitializeComponent();
        }

        private void frmViewTable_Load(object sender, EventArgs e)
        {
            if (student.getTotalStudents() != 0)
            {
                btn_print.Enabled = true;
            } else {
                btn_print.Enabled = false;
            }
            cbSearchBy.SelectedIndex = 0;
            showTable();
        }

        // show the list of students in the table
        public void showTable() {
            DataGridView_student.DataSource = student.getStudentList();
            
            DataGridViewImageColumn imageColumn = new DataGridViewImageColumn();
            imageColumn = (DataGridViewImageColumn)DataGridView_student.Columns[15];
            imageColumn.ImageLayout = DataGridViewImageCellLayout.Zoom;
        }

        //function for search
        public  DataTable getStudent(string search) {
            conn.openConnect();
            string searchBox = txt_Search.Text;
            string searchCmd = "SELECT * FROM tblStudentInfo WHERE "+ search +" LIKE '" + searchBox + "%'";
            MySqlCommand cmd = new MySqlCommand(searchCmd, conn.getconnection);
            MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
            DataTable table = new DataTable();
            adapter.Fill(table);
            conn.closeConnect();
            return table;
        }

        public void searchStudent() {
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

            DataGridViewImageColumn imageColumn = new DataGridViewImageColumn();
            imageColumn = (DataGridViewImageColumn)DataGridView_student.Columns[15];
            imageColumn.ImageLayout = DataGridViewImageCellLayout.Zoom;
        }

        

        private void button1_Click(object sender, EventArgs e)
        {
            searchStudent();
        }

        private void txt_Search_TextChanged(object sender, EventArgs e)
        {
            searchStudent();
        }

        private void btn_print_Click(object sender, EventArgs e)
        {
            //DGVPrinter helper
            printer.Title = "Students List";
            printer.SubTitle = string.Format("Date: {0}", DateTime.Now.Date);
            printer.SubTitleFormatFlags = StringFormatFlags.LineLimit | StringFormatFlags.NoClip;
            printer.PageNumbers = true;
            printer.PageNumberInHeader = false;
            printer.PorportionalColumns = true;
            printer.HeaderCellAlignment = StringAlignment.Near;
            printer.Footer = "foxlearn";
            printer.FooterSpacing = 5;
            printer.printDocument.DefaultPageSettings.Landscape = true;
            printer.PrintDataGridView(DataGridView_student);
        }


    }
}
