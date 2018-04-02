using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Virtual_Judge_Panel
{
    public partial class admin_profile_usr_cntrl : UserControl
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        string paths;
        string fileName = "empty";
        OpenFileDialog open;
        user u;
        public admin_profile_usr_cntrl()
        {
            InitializeComponent();
            
            
        }

        public void setinfo(user u)
        {

            bunifuMaterialTextbox1.Text = u.name;
            bunifuMaterialTextbox2.Text = u.username;
            bunifuMaterialTextbox3.Text = u.password;
            bunifuMaterialTextbox4.Text = u.email;
            bunifuMaterialTextbox5.Text = u.security_question;
            paths = Application.StartupPath.Substring(0, (Application.StartupPath.Length - 10));
            if (u.img_path!=null)
            {
                pictureBox1.ImageLocation = paths + u.img_path;
            }
            this.u = u;
             
            
        }
     

        private void bunifuFlatButton1_Click(object sender, EventArgs e)
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
                    pictureBox1.ImageLocation = open.FileName.ToString();

                }
            }
        }
        public int sss=0;

        private void bunifuFlatButton2_Click(object sender, EventArgs e)
        {
            user us = db.users.SingleOrDefault(x=> x.Id==u.Id);
            sss++;
            if (us != null)
            {

                if (!fileName.Equals("empty"))
                {
                    us.name = bunifuMaterialTextbox1.Text;
                    us.username = bunifuMaterialTextbox2.Text;
                    us.password = bunifuMaterialTextbox3.Text;
                    us.email = bunifuMaterialTextbox4.Text;
                    us.security_question = bunifuMaterialTextbox5.Text;
                    System.IO.File.Copy(open.FileName, paths + "\\UsersImages\\" + sss + fileName);
                    us.img_path = "\\UsersImages\\" + sss + fileName;
                    db.SubmitChanges();
                    db.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, db.users);
                    MessageBox.Show("Profile Updated !!");
                    setinfo(us);
                    
                }
                else
                {
                    us.name = bunifuMaterialTextbox1.Text;
                    us.username = bunifuMaterialTextbox2.Text;
                    us.password = bunifuMaterialTextbox3.Text;
                    us.email = bunifuMaterialTextbox4.Text;
                    us.security_question = bunifuMaterialTextbox5.Text;
                    db.SubmitChanges();
                    db.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, db.users);
                    MessageBox.Show("Profile Updated !!");
                    setinfo(us);
                }
      
                
            }
        }



    }
}
