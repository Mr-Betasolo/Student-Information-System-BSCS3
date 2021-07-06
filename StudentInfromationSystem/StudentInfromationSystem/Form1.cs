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
    public partial class frmLogin : Form
    {

        private bool checkClosedReady = true;
        public frmLogin()
        {
            InitializeComponent();
        }

        private void frmLogin_Load(object sender, EventArgs e)
        {
            checkClosedReady = true;
        }

        private void label5_Click(object sender, EventArgs e)
        {
            checkClosedReady = false;
            this.Hide();
            frmSignup form2 = new frmSignup();
            form2.Show();
        }

        private void chkShowPass_CheckedChanged(object sender, EventArgs e)
        {
            txtPassword.PasswordChar = chkShowPass.Checked ? '\0' : '*';
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            clearFields();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Text;
            bool isValid = false;

            if (username == "" || password == "")
            {
                MessageBox.Show("Fill up all the fields", "Login Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                isValid = false;
            }
            else {
                isValid = true;
            }

            if (isValid)
            {
                try
                {
                    string connString = "server=localhost;database=db_login;uid=root;password=Password;";
                    MySqlConnection conn = new MySqlConnection(connString);

                    conn.Open();

                    string readQuery = "SELECT * FROM tblUser WHERE username = @user and password = @pw";
                    MySqlDataReader reader;
                    MySqlCommand cmd = new MySqlCommand(readQuery, conn);
                    cmd.Parameters.AddWithValue("@user", username);
                    cmd.Parameters.AddWithValue("@pw", password);
                    reader = cmd.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read()) {
                            string userFname = reader["first_name"].ToString();
                            byte[] img = null;
                            if (!DBNull.Value.Equals(reader["profile_pic"])) { 
                                img = (byte[])reader["profile_pic"];
                            }

                            MessageBox.Show("Login Successfully", "Login", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            checkClosedReady = false;
                            this.Hide();
                            frmUser form3 = new frmUser(userFname, img);
                            form3.Show();
                        }
                    }
                    else 
                    {
                        MessageBox.Show("User not found", "Information", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                    conn.Close();
                }
                catch (Exception err)
                {
                    MessageBox.Show(err.Message, "Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

        }

        // functions
        public void clearFields() {
            txtPassword.Text = "";
            txtUsername.Text = "";
            chkShowPass.Checked = false;
        }

        private void frmLogin_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (checkClosedReady == true) { 
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
