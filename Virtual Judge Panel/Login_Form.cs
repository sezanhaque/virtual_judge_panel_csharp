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

namespace Virtual_Judge_Panel
{
    public partial class Login_Form : Form
    {
        public Login_Form()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            DataClasses1DataContext db = new DataClasses1DataContext();
            db.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, db.users);
            Sign_Up_Panel.Visible = false;
            var data = from type in db.account_types
                       where type.Id > 1
                       select type;
            comboBox1.DataSource = data;
            comboBox1.DisplayMember = "types";
            comboBox1.ValueMember = "Id";

        }

        private void bunifuThinButton21_Click(object sender, EventArgs e)
        {
            DataClasses1DataContext db = new DataClasses1DataContext();
            db.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, db.users);
            string usr = Username_Custom_textbox.Text;
            string pss = Password_Custom_textbox.Text;
            user u = db.users.SingleOrDefault(x => x.username.Equals(usr) && x.password.Equals(pss));

            if (u != null)
            {
                if (u.account_type_id == 1)
                {
                    Admin_Form profile_form = new Admin_Form(u);
                    profile_form.Show();
                    profile_form.RefToForm1 = this;
                    this.Visible = false;
                }
                else if (u.account_type_id == 2)
                {
                    Student_Form s = new Student_Form(u);
                    s.Show();
                    s.RefToForm1 = this;
                    this.Visible = false;
                }
                else if (u.account_type_id == 3)
                {
                    Organization_Form org = new Organization_Form(u);
                    org.Show();
                    org.RefToForm1 = this;
                    this.Visible = false;
                }

            }
            else if (u == null)
            {
                MessageBox.Show("User or Password is Incorrect!!");
            }
        }
        private void Close_Button_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }
        private void Minimize_Button_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
        private void Signup_Button_Click(object sender, EventArgs e)
        {
            Sign_Up_Panel.Visible = true;
        }

        private void bunifuThinButton21_Click_1(object sender, EventArgs e)
        {
            DataClasses1DataContext db = new DataClasses1DataContext();
            Sign_Up_Panel.Visible = false;
            Login_GradientPanel.Visible = true;
        }

        private void bunifuThinButton22_Click_1(object sender, EventArgs e)
        {
            if (Username_Reg_Custom_Textobox.Text.Equals("") || Password_Reg_Custom_Textbox.Text.Equals("") || Email_Reg_Custom_Textbox.Text.Equals(""))
            {
                MessageBox.Show("Fill up all the fields!!");
            }
            else
            {
                if (Password_Reg_Custom_Textbox.Text.Equals(Confirmpassword_Reg_Custom_Textbox.Text))
                {
                    DataClasses1DataContext db = new DataClasses1DataContext();
                    user u = new user();
                    u.username = Username_Reg_Custom_Textobox.Text;
                    u.password = Password_Reg_Custom_Textbox.Text;
                    u.email = Email_Reg_Custom_Textbox.Text;
                    u.account_type_id = Int32.Parse(comboBox1.SelectedValue.ToString());
                    db.users.InsertOnSubmit(u);
                    db.SubmitChanges();
                    MessageBox.Show("Registration successful !!");
                    db.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, db.users);

                    Sign_Up_Panel.Visible = false;
                    Login_GradientPanel.Visible = true;
                }
                else
                {
                    MessageBox.Show("Password don not match!!");
                }
            }
        }
    }
}
