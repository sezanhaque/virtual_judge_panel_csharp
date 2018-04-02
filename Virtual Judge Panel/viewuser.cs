using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Virtual_Judge_Panel
{
    public partial class viewuser : Form
    {
        user u;
        DataClasses1DataContext db = new DataClasses1DataContext();
        string paths;
        OpenFileDialog open;
        string fileName="empty";
        int sss = 0;
        public viewuser(user u)
        {
            InitializeComponent();
            this.u = u;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void viewuser_Load(object sender, EventArgs e)
        {
            bunifuMaterialTextbox1.Text = u.name;
            bunifuMaterialTextbox2.Text = u.username;
            bunifuMaterialTextbox3.Text = u.password;
            bunifuMaterialTextbox4.Text = u.email;
            bunifuMaterialTextbox5.Text = u.security_question;
            var data = from type in db.account_types
                       where type.Id > 1
                       select type;
            comboBox1.DataSource = data;
            comboBox1.DisplayMember = "types";
            comboBox1.ValueMember = "Id";
            comboBox1.SelectedValue = u.account_type_id;
            paths = Application.StartupPath.Substring(0, (Application.StartupPath.Length - 10));
            pictureBox3.ImageLocation = paths + u.img_path;

        }

        private void bunifuFlatButton2_Click(object sender, EventArgs e)
        {
            user us = db.users.SingleOrDefault(x => x.Id == u.Id);
            if (us != null)
            {
                db.users.DeleteOnSubmit(us);
                db.SubmitChanges();
                bunifuMaterialTextbox1.Text = "";
                bunifuMaterialTextbox2.Text = "";
                bunifuMaterialTextbox3.Text = "";
                bunifuMaterialTextbox4.Text = "";
                bunifuMaterialTextbox5.Text = "";
                paths = Application.StartupPath.Substring(0, (Application.StartupPath.Length - 10));
                pictureBox3.ImageLocation = paths + "\\UsersImages\\facebook-avatar.jpg";
                MessageBox.Show("User Deleted!!");
            }
            
        }

        

        private void bunifuFlatButton3_Click(object sender, EventArgs e)
        {
            open = new OpenFileDialog();
            open.InitialDirectory = "C:\\";
            open.Filter = "Image Files(*.jpg)|*.jpg|All Files(*.*)|*.*";
            open.FilterIndex = 1;
            if (open.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (open.CheckFileExists)
                {
                    paths = Application.StartupPath.Substring(0, (Application.StartupPath.Length - 10));
                    fileName = System.IO.Path.GetFileName(open.FileName);
                    pictureBox3.ImageLocation = open.FileName.ToString();
                    
                }
            }
        }


        private void bunifuFlatButton1_Click(object sender, EventArgs e)
        {
            sss++;
            user us = db.users.SingleOrDefault(x => x.Id == u.Id);
            if (us != null)
            {
                if (!fileName.Equals("empty"))
                {
                    System.IO.File.Copy(open.FileName, paths + "\\UsersImages\\" + sss + fileName);
                    us.img_path = "\\UsersImages\\" + sss + fileName;
                    us.name = bunifuMaterialTextbox1.Text;
                    us.username = bunifuMaterialTextbox2.Text;
                    us.password = bunifuMaterialTextbox3.Text;
                    us.email = bunifuMaterialTextbox4.Text;
                    us.security_question = bunifuMaterialTextbox5.Text;
                    us.account_type_id = Int32.Parse(comboBox1.SelectedValue.ToString());
                    db.SubmitChanges();
                    MessageBox.Show("Updated !!");
                }
                else
                {
                    us.name = bunifuMaterialTextbox1.Text;
                    us.username = bunifuMaterialTextbox2.Text;
                    us.password = bunifuMaterialTextbox3.Text;
                    us.email = bunifuMaterialTextbox4.Text;
                    us.security_question = bunifuMaterialTextbox5.Text;
                    us.account_type_id = Int32.Parse(comboBox1.SelectedValue.ToString());
                    db.SubmitChanges();
                    MessageBox.Show("Updated !!");
                }

                
            }
        }
    }
}
