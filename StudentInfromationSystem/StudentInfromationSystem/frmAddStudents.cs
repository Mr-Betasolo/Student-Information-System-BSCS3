using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using MySql.Data.MySqlClient;

namespace StudentInfromationSystem
{
    public partial class frmAddStudents : Form
    {
        public frmAddStudents()
        {
            InitializeComponent();
        }

        StudentAddClass student = new StudentAddClass();
        private int count = 0;

        private void frmAddStudents_Load(object sender, EventArgs e)
        {
            btn_prev.Visible = false;
            btn_add.Visible = false;
            btn_add.Visible = false;
            gb_guardian.Visible = false;
            gb_school.Visible = false;
            txt_fname.Focus();
            cbGender.SelectedIndex = 0;
            cbStrands.SelectedIndex = 0;
            cbYear.SelectedIndex = 0;
            cbCourse.SelectedIndex = 0;
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
                btn_add.Visible = true;
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
                txt_lname.Focus();
                count = 0;
            }
            else if (count == 2) {
                gb_school.Visible = true;
                gb_guardian.Visible = false;
                btn_next.Visible = true;
                btn_add.Visible =false;
                txt_shs.Focus();
                count = 1;
            }
        }

        private void btn_add_Click(object sender, EventArgs e)
        {
            //for add students button
            /**
             * @fn, @md, @ln, @gender, @age, @birthdate, @nationality, @religion, @civil, @address, @phoneNum, @email, @photo,
             * @sSchool, @sStrands, @sSpecialization, @sGraduated, @gName, @gOccupation, @gRelation, @gAddress, @gPhone
            **/
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
                        if (student.insertStudent(lname, fname, mname, year, course, gender, age, birthdate, nationality, religion, civil, address, phoneNum, email, image, shsPrev, shsStrand, shsSpecialization, shsYear, gName, gOccupation, gRelation, gAddress, gPhoneNum))
                        {
                            MessageBox.Show(fname + " " + lname + " was added.", "Add student", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                    MessageBox.Show("Fill up the empty fields", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else 
            {
                MessageBox.Show("Incorrect age or birthdate", "Info Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            

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

            }
            else if (count == 1)
            {
                txt_shs.Text = "";
                cbStrands.SelectedIndex = 0;
                txt_specialization.Text = "";
                num_yrgraduated.Value = 2010;
            }
            else if (count == 2) {
                txt_Guardian.Text = "";
                txt_guardianOccupation.Text = "";
                txt_guardianRelation.Text = "";
                txt_guardianAddress.Text = "";
                txt_guardianCell.Text = "";
            }
        }

        private void btn_upload_Click(object sender, EventArgs e)
        {
            // Select photo from your computer
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

        // functions
        bool verify() {

            if (txt_fname.Text == "" && txt_mname.Text == "" && txt_lname.Text == "" && txt_nationality.Text == "" && 
                txt_religion.Text == "" && txt_civil.Text == "" && txt_address.Text == "" && txt_cell.Text == "" && txt_email.Text == "") {
                return false;
            } else
            {
                return true;
            }
        }

        public void  clearAll() {
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
            txt_shs.Text = "";
            cbStrands.SelectedIndex = 0;
            txt_specialization.Text = "";
            num_yrgraduated.Value = 2010;
            txt_Guardian.Text = "";
            txt_guardianOccupation.Text = "";
            txt_guardianRelation.Text = "";
            txt_guardianAddress.Text = "";
            txt_guardianCell.Text = "";
            img_studentpic.Image = null;
        }

    }
}
