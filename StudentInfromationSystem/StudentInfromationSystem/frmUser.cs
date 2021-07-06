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

namespace StudentInfromationSystem
{
    public partial class frmUser : Form
    {
        StudentAddClass student = new StudentAddClass();
        private bool checkCloseReady = true;
        public frmUser(string fname, byte[] img)
        {
            InitializeComponent();
            lbl_title.Text = "Welcome to Student Information System,  " + fname;
            if (img == null)
            {
                pictureBox1.Image = null;
            }
            else {

                MemoryStream ms = new MemoryStream(img);
                pictureBox1.Image = Image.FromStream(ms);
            }
            
        }

        

        // to show register form
        private Form activeForm = null;
        private void openChildForm(Form childForm) {
            if (activeForm != null) 
                activeForm.Close();
            activeForm = childForm;
            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;
            panel_title.Visible = false;
            panel_main.Controls.Add(childForm);
            panel_main.Tag = childForm;
            childForm.BringToFront();
            childForm.Show();
        }

        private void frmUser_Load(object sender, EventArgs e)
        {
            // when form load
            string total = student.getTotalStudents().ToString();
            lbl_total.Text = total;
            checkCloseReady = true;

        }

        private void btn_add_Click(object sender, EventArgs e)
        {
            openChildForm(new frmAddStudents());
            lbl_total.Visible = false;
            lbl_total1.Visible = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            openChildForm(new frmViewTable());
            lbl_total.Visible = false;
            lbl_total1.Visible = false;
        }

        private void btnHome_Click(object sender, EventArgs e)
        {
            if (activeForm != null) {
                activeForm.Close();
            }
            panel_main.Controls.Add(panel_cover);
            panel_title.Visible = true;

            string total = student.getTotalStudents().ToString();
            lbl_total.Text = total;
            lbl_total.Visible = true;
            lbl_total1.Visible = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            openChildForm(new frmEditStudents());
            lbl_total.Visible = false;
            lbl_total1.Visible = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            openChildForm(new frmDropStudents());
            lbl_total.Visible = false;
            lbl_total1.Visible = false;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            DialogResult dlg = MessageBox.Show("Are you sure you want to logout?", "Logout", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if (dlg.Equals(DialogResult.Yes)) {
                checkCloseReady = false;
                frmLogin form1 = new frmLogin();
                this.Close();
                form1.ShowDialog();
            }
        }

        private void frmUser_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (checkCloseReady == true)
            {
                if (MessageBox.Show("Are you sure you want to exit the app?",
                       "Student Information System",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Information) == DialogResult.No)
                {
                    e.Cancel = true;
                }
            }
            
        }
    }
}
