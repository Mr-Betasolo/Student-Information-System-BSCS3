using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using MySql.Data.MySqlClient;
using System.IO;

namespace StudentInfromationSystem
{
    public partial class frmSignup : Form
    {
        private bool checkClosed = false;
        private void frmSignup_Load(object sender, EventArgs e)
        {
            //Form
            checkClosed = false;
        }

        public frmSignup()
        {
            InitializeComponent();
        }

        private void lblBack_Click(object sender, EventArgs e)
        {
            // this if for switching back to the login form
            backToFormOne();
        }

        private void chkShowPass_CheckedChanged(object sender, EventArgs e)
        {
            // this is for toggling the password
            txtConfirmPw.PasswordChar = chkShowPass.Checked ? '\0' : '*';
            txtPw.PasswordChar = chkShowPass.Checked ? '\0' : '*';
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            clearFields();
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            string connString = "server=localhost;database=db_login;uid=root;password=Password;";
            MySqlConnection conn = new MySqlConnection(connString);

            string firstName = txtFname.Text;
            string lastName = txtLname.Text;
            string email = txtEmail.Text != "example@gmail.com" ? txtEmail.Text : "";
            string phoneNum = txtNum.Text != "+639xxxxxxxxx" ? txtNum.Text : "";
            string password = txtPw.Text;
            string confirmPass = txtConfirmPw.Text;
            string username = txt_username.Text;
            bool isValid = false, isPwValid = false, isEmailValid = false, isNumValid = false;

            if (firstName == "" || lastName == "" || email == "" || phoneNum == "" || password == "" || confirmPass == "" || username == "")
            {
                MessageBox.Show("Fill up all the fields", "Signup Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                isValid = false;
            }
            else {
                isValid = true;
            }

            if (password != "" && confirmPass != "") {
                isPwValid = checkPassword(password, confirmPass);
            }

            if (email != "") {
                isEmailValid = checkEmail(email);
            }

            if (phoneNum != "") {
                isNumValid = checkNumber(phoneNum);
            }

            // for profile picture
            byte[] imgByte = null;
            if (imgProfile.Image != null) {
                MemoryStream ms = new MemoryStream();
                imgProfile.Image.Save(ms, imgProfile.Image.RawFormat);
                imgByte = ms.GetBuffer();
                ms.Close();
            }


            if (isValid == true && isPwValid == true && isEmailValid == true && isNumValid == true) {
                try
                {
                    // added
                    conn.Open();
                    string readQuery = "SELECT * FROM tblUser WHERE username = @user";
                    MySqlDataReader reader;
                    MySqlCommand cmd1 = new MySqlCommand(readQuery, conn);
                    cmd1.Parameters.AddWithValue("@user", username);
                    reader = cmd1.ExecuteReader();

                    if (reader.HasRows)
                    {
                        MessageBox.Show("Username is already taken. Try another one", "Signup Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        conn.Close();
                    }
                    else {
                        reader.Close();
                        MessageBox.Show("Connecting to Server...", "", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        string insertQuery = "INSERT INTO tblUser(first_name, last_name, username, email, phone_number, password, profile_pic) VALUES(@fname, @lname, @username, @email, @phonenum, @pass, @profilepic)";
                        MySqlCommand cmd = new MySqlCommand(insertQuery, conn);

                        cmd.Parameters.AddWithValue("@fname", firstName);
                        cmd.Parameters.AddWithValue("@lname", lastName);
                        cmd.Parameters.AddWithValue("@username", username);
                        cmd.Parameters.AddWithValue("@email", email);
                        cmd.Parameters.AddWithValue("@phonenum", phoneNum);
                        cmd.Parameters.AddWithValue("@pass", password);
                        cmd.Parameters.AddWithValue("@profilepic", imgByte);

                        cmd.ExecuteNonQuery();

                        conn.Close();
                        MessageBox.Show("You are now registered!", "Signup Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        backToFormOne();
                    }
                    // added

                }
                catch (MySqlException ex)
                {
                    MessageBox.Show(ex.Message.ToString(), "Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    switch (ex.Number)
                    {
                        case 0:
                            MessageBox.Show("Cannot connect to server. Contact administrator", "Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            break;
                        case 1045:
                            MessageBox.Show("Invalid username or password", "Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            break;
                    }

                }

            }

        }

        private void btnInsert_Click(object sender, EventArgs e)
        {
            try {
                OpenFileDialog opnfd = new OpenFileDialog();
                opnfd.Filter = "Image Files(*.jpg; *.jpeg; *.png; *.bmp)|*.jpg; *.jpeg; *.png; *.bmp";
                opnfd.Title = "Select your Profile Picture";
                if (opnfd.ShowDialog() == DialogResult.OK)
                {
                    imgProfile.Image = Image.FromFile(opnfd.FileName);
                }
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }
            
        }

        // code for the textbox placeholder
        private void txtEmail_Enter(object sender, EventArgs e)
        {
            if (txtEmail.Text == "example@gmail.com") {
                txtEmail.Text = "";
                txtEmail.ForeColor = Color.Black;
            }
        }

        private void txtEmail_Leave(object sender, EventArgs e)
        {
            if (txtEmail.Text == "")
            {
                txtEmail.Text = "example@gmail.com";
                txtEmail.ForeColor = Color.Gray;
            }
        }

        private void txtNum_Enter(object sender, EventArgs e)
        {
            if (txtNum.Text == "+639xxxxxxxxx")
            {
                txtNum.Text = "";
                txtNum.ForeColor = Color.Black;
            }
        }

        private void txtNum_Leave(object sender, EventArgs e)
        {
            if (txtNum.Text == "")
            {
                txtNum.Text = "+639xxxxxxxxx";
                txtNum.ForeColor = Color.Gray;
            }
        }
        // code for the textbox placeholder END

        // functions
        public void clearFields() {
            // Clearing the text fields
            txtFname.Text = "";
            txtLname.Text = "";
            txtPw.Text = "";
            txtConfirmPw.Text = "";

            if (txtEmail.Text != "example@gmail.com") {
                txtEmail.Text = "";
                txtEmail.Text = "example@gmail.com";
                txtEmail.ForeColor = Color.Gray;
            }

            if (txtNum.Text != "+639xxxxxxxxx") {
                txtNum.Text = "";
                txtNum.Text = "+639xxxxxxxxx";
                txtNum.ForeColor = Color.Gray;
            }

        }

        public bool checkPassword(string pw, string confirmPw) {

            Regex regex = new Regex(@"(?=^.{8,}$)(?=.*\d)(?=.*[a-zA-Z])");
            Match match = regex.Match(pw);

            if (pw != confirmPw)
            {
                MessageBox.Show("Password doesn't match", "Signup Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else if (pw.Length < 8)
            {
                MessageBox.Show("Password must be more than 8 characters", "Signup Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else if (!match.Success)
            {
                MessageBox.Show("Password must contain uppercase and lowercase characters \n and a number", "Signup Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else {
                return true;
            }
        }

        public bool checkEmail(string email) {

            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            Match match = regex.Match(email);

            if (match.Success) {
                return true;
            }
            else {
                MessageBox.Show("Incorrect email format", "Signup Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

        }

        public bool checkNumber(string num) {
            Regex regex = new Regex(@"^(^\+639[\d]{9})$");
            Match match = regex.Match(num);
            if (match.Success)
            {
                return true;
            }
            else {
                MessageBox.Show("Incorrect phone number format", "Signup Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        public void backToFormOne() {
            checkClosed = true;
            this.Close();
            frmLogin form1 = new frmLogin();
            form1.ShowDialog();
        }

        private void frmSignup_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (checkClosed == false)
            {
                frmLogin form1 = new frmLogin();
                form1.Show();
            }
        }
    }
}
