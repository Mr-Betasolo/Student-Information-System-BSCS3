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
using System.IO;

namespace StudentInfromationSystem
{
    public partial class frmEditStudents : Form
    {
        StudentAddClass student = new StudentAddClass();
        DBConnect conn = new DBConnect();
        public frmEditStudents()
        {
            InitializeComponent();
        }

        private int count = 0;

        private void frmEditStudents_Load(object sender, EventArgs e)
        {
            // when form loads
            panel_grid.Visible = false;
            cbSearchBy.SelectedIndex = 0;
            showTable();
        }
        private void btn_clear_Click(object sender, EventArgs e)
        {
            if (count == 0)
            {
                txt_fname.Text = "";
                txt_mname.Text = "";
                txt_lname.Text = "";
                cbGender.SelectedIndex = 0;
                numAge.Value = 17;
                dt_birthdate.ResetText();
                txt_nationality.Text = "";
                txt_religion.Text = "";
                txt_civil.Text = "";
                txt_address.Text = "";
                txt_cell.Text = "";
                txt_email.Text = "";
                img_studentpic.Image = null;
                cbCourse.SelectedIndex = 0;
                cbYear.SelectedIndex = 0;

            }
            else if (count == 1)
            {
                txt_shs.Text = "";
                cbStrands.SelectedIndex = 0;
                txt_specialization.Text = "";
                num_yrgraduated.Value = 2010;
            }
            else if (count == 2)
            {
                txt_Guardian.Text = "";
                txt_guardianOccupation.Text = "";
                txt_guardianRelation.Text = "";
                txt_guardianAddress.Text = "";
                txt_guardianCell.Text = "";
            }
        }
        // function for clearing all fields
        public void clearAll()
        {
            txt_fname.Text = "";
            txt_mname.Text = "";
            txt_lname.Text = "";
            cbGender.SelectedIndex = 0;
            numAge.Value = 17;
            dt_birthdate.ResetText();
            txt_nationality.Text = "";
            txt_religion.Text = "";
            txt_civil.Text = "";
            txt_address.Text = "";
            txt_cell.Text = "";
            txt_email.Text = "";
            img_studentpic.Image = null;
            cbCourse.SelectedIndex = 0;
            cbYear.SelectedIndex = 0;
            txt_shs.Text = "";
            cbStrands.SelectedIndex = 0;
            txt_specialization.Text = "";
            num_yrgraduated.Value = 2010;
            txt_Guardian.Text = "";
            txt_guardianOccupation.Text = "";
            txt_guardianRelation.Text = "";
            txt_guardianAddress.Text = "";
            txt_guardianCell.Text = "";
        }

        private void btn_next_Click(object sender, EventArgs e)
        {
            // when next button was clicked
            if (count == 0)
            {
                gb_personal.Visible = false;
                gb_school.Visible = true;
                btn_prev.Visible = true;
                txt_shs.Focus();
                count = 1;
            }
            else if (count == 1)
            {
                gb_school.Visible = false;
                gb_guardian.Visible = true;
                btn_next.Visible = false;
                btn_save.Visible = true;
                txt_Guardian.Focus();
                count = 2;
            }
        }

        private void btn_prev_Click(object sender, EventArgs e)
        {
            if (count == 1)
            {
                gb_personal.Visible = true;
                gb_school.Visible = false;
                btn_prev.Visible = false;
                txt_fname.Focus();
                count = 0;
            }
            else if (count == 2)
            {
                gb_school.Visible = true;
                gb_guardian.Visible = false;
                btn_next.Visible = true;
                btn_save.Visible = false;
                txt_shs.Focus();
                count = 1;
            }
        }

        private void btn_save_Click(object sender, EventArgs e)
        {
            // save the edited data
            int id;
            if (txt_id.Text == "") 
            {
                id = -1;
            }
            else
            {
                id = Convert.ToInt32(txt_id.Text);
            }
            string fname = txt_fname.Text;
            string mname = txt_mname.Text;
            string lname = txt_lname.Text;
            string year = cbYear.Text;
            string course = cbCourse.Text;
            string gender = cbGender.Text;
            int age = Convert.ToInt32(numAge.Value);
            DateTime birthdate = dt_birthdate.Value;
            string nationality = txt_nationality.Text;
            string religion = txt_religion.Text;
            string civil = txt_civil.Text;
            string address = txt_address.Text;
            string phoneNum = txt_cell.Text;
            string email = txt_email.Text;
            string shsPrev = txt_shs.Text;
            string shsStrand = cbStrands.Text;
            string shsSpecialization = txt_specialization.Text;
            int shsYear = Convert.ToInt32(num_yrgraduated.Value);
            string gName = txt_Guardian.Text;
            string gOccupation = txt_guardianOccupation.Text;
            string gRelation = txt_guardianRelation.Text;
            string gAddress = txt_guardianAddress.Text;
            string gPhoneNum = txt_guardianCell.Text;
            // to get the photo
            byte[] image = null;
            if (img_studentpic.Image != null)
            {
                MemoryStream ms = new MemoryStream();
                img_studentpic.Image.Save(ms, img_studentpic.Image.RawFormat);
                image = ms.GetBuffer();
                ms.Close();
            }

            int bornYear = dt_birthdate.Value.Year;
            int thisYear = DateTime.Now.Year;

            if (thisYear - bornYear == age || (thisYear - bornYear) - 1 == age)
            {
                if (verify())
                {
                    try
                    {
                        if (student.updateStudent(id, lname, fname, mname, year, course, gender, age, birthdate, nationality, religion, civil, address, phoneNum, email, image, shsPrev, shsStrand, shsSpecialization, shsYear, gName, gOccupation, gRelation, gAddress, gPhoneNum))
                        {
                            MessageBox.Show("Data updated successfully", "Update Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            clearAll();
                        }
                    }
                    catch (Exception err)
                    {
                        MessageBox.Show(err.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("There are no datas to edit", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Incorrect age or birthdate", "Info Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void btn_upload_Click(object sender, EventArgs e)
        {
            // upload new image
            try
            {
                OpenFileDialog opnfd = new OpenFileDialog();
                opnfd.Filter = "Image Files(*.jpg; *.jpeg; *.png; *.bmp)|*.jpg; *.jpeg; *.png; *.bmp";
                opnfd.Title = "Select your Profile Picture";
                if (opnfd.ShowDialog() == DialogResult.OK)
                {
                    img_studentpic.Image = Image.FromFile(opnfd.FileName);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void rb_TextView_CheckedChanged(object sender, EventArgs e)
        {
            if (rb_TextView.Checked == true)
            {
                panel_grid.Visible = false;
                btn_clear.Enabled = true;
                btn_next.Visible = true;
                btn_prev.Visible = true;
            } else 
            { 
                panel_grid.Visible = true;
                btn_clear.Enabled = false;
                btn_next.Visible = false;
                btn_prev.Visible = false;
                btn_save.Visible = false;
            }
        }

        // show table
        public void showTable() {
            DataGridView_student.DataSource = student.getStudentList();
            DataGridViewImageColumn imageColumn = new DataGridViewImageColumn();
            imageColumn = (DataGridViewImageColumn)DataGridView_student.Columns[15];
            imageColumn.ImageLayout = DataGridViewImageCellLayout.Zoom;
        }

        // for searching student
        public DataTable getStudent(string search)
        {
            conn.openConnect();
            string searchBox = txt_search.Text;
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

            DataGridViewImageColumn imageColumn = new DataGridViewImageColumn();
            imageColumn = (DataGridViewImageColumn)DataGridView_student.Columns[15];
            imageColumn.ImageLayout = DataGridViewImageCellLayout.Zoom;
        }

        private void txt_search_TextChanged(object sender, EventArgs e)
        {
            searchStudent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (txt_search.Text != "") { 
                searchStudent();
                getDatainTable();
            }
        }

        private void DataGridView_student_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            getDatainTable();
        }

        //function to verify if empty fields
        bool verify()
        {

            if (txt_fname.Text == "" && txt_mname.Text == "" && txt_lname.Text == "" && txt_nationality.Text == "" &&
                txt_religion.Text == "" && txt_civil.Text == "" && txt_address.Text == "" && txt_cell.Text == "" && txt_email.Text == "")
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public void getDatainTable() {
            /**
             * @fn, @md, @ln, @year, @course, @gender, @age, @birthdate, @nationality, @religion, @civil, 
             * @address, @phoneNum, @email, @photo, @sSchool, @sStrands, @sSpecialization, @sGraduated, @gName,
             * @gOccupation, @gRelation, @gAddress, @gPhone
            **/

            txt_id.Text = DataGridView_student.CurrentRow.Cells[0].Value.ToString();
            txt_lname.Text = DataGridView_student.CurrentRow.Cells[1].Value.ToString();
            txt_fname.Text = DataGridView_student.CurrentRow.Cells[2].Value.ToString();
            txt_mname.Text = DataGridView_student.CurrentRow.Cells[3].Value.ToString();
            cbYear.Text = DataGridView_student.CurrentRow.Cells[4].Value.ToString();
            cbCourse.Text = DataGridView_student.CurrentRow.Cells[5].Value.ToString();
            cbGender.Text = DataGridView_student.CurrentRow.Cells[6].Value.ToString();
            numAge.Text = DataGridView_student.CurrentRow.Cells[7].Value.ToString();
            dt_birthdate.Value = (DateTime)DataGridView_student.CurrentRow.Cells[8].Value;
            txt_nationality.Text = DataGridView_student.CurrentRow.Cells[9].Value.ToString();
            txt_religion.Text = DataGridView_student.CurrentRow.Cells[10].Value.ToString();
            txt_civil.Text = DataGridView_student.CurrentRow.Cells[11].Value.ToString();
            txt_address.Text = DataGridView_student.CurrentRow.Cells[12].Value.ToString();
            txt_cell.Text = DataGridView_student.CurrentRow.Cells[13].Value.ToString();
            txt_email.Text = DataGridView_student.CurrentRow.Cells[14].Value.ToString();

            if (!DBNull.Value.Equals(DataGridView_student.CurrentRow.Cells[15].Value))
            {
                byte[] img = (byte[])DataGridView_student.CurrentRow.Cells[15].Value;
                MemoryStream ms = new MemoryStream(img);
                img_studentpic.Image = Image.FromStream(ms);
            }
            else
            {
                img_studentpic.Image = null;
            }
            txt_shs.Text = DataGridView_student.CurrentRow.Cells[16].Value.ToString();
            cbStrands.Text = DataGridView_student.CurrentRow.Cells[17].Value.ToString();
            txt_specialization.Text = DataGridView_student.CurrentRow.Cells[18].Value.ToString();
            num_yrgraduated.Text = DataGridView_student.CurrentRow.Cells[19].Value.ToString();
            txt_Guardian.Text = DataGridView_student.CurrentRow.Cells[20].Value.ToString();
            txt_guardianOccupation.Text = DataGridView_student.CurrentRow.Cells[21].Value.ToString();
            txt_guardianRelation.Text = DataGridView_student.CurrentRow.Cells[22].Value.ToString();
            txt_guardianAddress.Text = DataGridView_student.CurrentRow.Cells[23].Value.ToString();
            txt_guardianCell.Text = DataGridView_student.CurrentRow.Cells[24].Value.ToString();
        }
    }
}
