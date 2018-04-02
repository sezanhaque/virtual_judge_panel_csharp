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
    public partial class Admin_Form : Form
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        user u;
        int id;
        public Admin_Form(user u)
        {
            InitializeComponent();
            this.u = u;
        }
        private void Profile_Form_Load(object sender, EventArgs e)
        {
            db.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, db.users);
            db.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, db.events);
            adminDashboard1.setinfo(u);
            Menu_Minimize.Visible = false;
            Logo_Minimize.Visible = false;
            adminDashboard1.Visible = false;
            ProjectPanle.Visible = false;
            allusersPanel.Visible = false;
            EventPanel.Visible = false;
            dashboardbtn.selected = true;
            admin_profile_usr_cntrl1.Visible = false;
            adminDashboard1.Visible = true;
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void Close_Button_Click(object sender, EventArgs e)
        {
            this.Close();
            this.RefToForm1.Show();
            //this.Visible = false;
            //Environment.Exit(0);
            /*Form1 f = new Form1();
            f.Show();*/
        }

        private void bunifuFlatButton2_Click(object sender, EventArgs e)
        {
            db.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, db.users);
            user usr = db.users.SingleOrDefault(x => x.Id == u.Id);
            admin_profile_usr_cntrl1.setinfo(usr);
            adminDashboard1.Visible = false;
            allusersPanel.Visible = false;
            ProjectPanle.Visible = false;
            EventPanel.Visible = false;
            admin_profile_usr_cntrl1.Visible = true;
        }
        private void bunifuFlatButton4_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            this.RefToForm1.Show();
        }
        public Form RefToForm1 { get; set; }
        
        private void Menu_Button_Click(object sender, EventArgs e)
        {
            //bunifuCustomDataGrid2.Width += 150;
            //bunifuCustomDataGrid1.Width -= 150;

            Menu.Visible = true;
            Logo.Visible = true;
            Menu_Minimize.Visible = false;
            Logo_Minimize.Visible = false;
            Slide_Panel.Visible = false;
            Slide_Panel.Width = 215;
            Panel_Animator.ShowSync(Slide_Panel);

        }

        private void Slide_Menu_Click(object sender, EventArgs e)
        {
            //bunifuCustomDataGrid2.Width -= 150;
            //bunifuCustomDataGrid1.Width += 150;

            Slide_Panel.Visible = false;
            Menu_Minimize.Visible = true;
            Logo_Minimize.Visible = true;
            Slide_Panel.Width = 50;
            Panel_Animator.ShowSync(Slide_Panel);

            
        }
        private void bunifuFlatButton1_Click(object sender, EventArgs e)
        {
            db.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, db.users);
            user ur = db.users.SingleOrDefault(x=>x.Id==u.Id);
            adminDashboard1.setinfo(ur);
            admin_profile_usr_cntrl1.Visible = false;
            allusersPanel.Visible = false;
            ProjectPanle.Visible = false;
            EventPanel.Visible = false;
            adminDashboard1.Visible = true;
        }
        private void bunifuImageButton2_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
        private void bunifuFlatButton1_Click_1(object sender, EventArgs e)
        {
            
            adminDashboard1.Visible = false;
            admin_profile_usr_cntrl1.Visible = false;
            ProjectPanle.Visible = false;
            EventPanel.Visible = false;
            allusersPanel.Visible = true;
            var data = from usr in db.users
                       where usr.Id > 1
                       select new { usr.Id, usr.name, usr.username, usr.password, usr.email, usr.account_type.types };
            bunifuCustomDataGrid1.DataSource=data;

            var data2 = from type in db.account_types
                        where type.Id > 1
                        select type;
            userfiltercombox.DataSource = data2;
            userfiltercombox.DisplayMember = "types";
            userfiltercombox.ValueMember = "Id";

            userfiltercombox.SelectedIndex = -1;
            userfiltercombox.Text = "Select A Type";

        }

        public void getAvgrating()
        {
            db.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, db.users);

            var data = from pro in db.Projects
                       where pro.Id > 0
                       select pro;
            foreach (Project p in data)
            {
                var data2 = from r in db.Ratings
                            where r.projectid == p.Id
                            select r;
                p.avg_Rating = 0.0;
                int c = 0;
                foreach (Rating rt in data2)
                {
                    c += 1;
                    p.avg_Rating += rt.rating1;

                }
                p.avg_Rating = p.avg_Rating / c;
                db.SubmitChanges();
            }

        }

        private void projectbutton_Click(object sender, EventArgs e)
        {
            DataClasses1DataContext db = new DataClasses1DataContext();
            db.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, db.Projects);
            admin_profile_usr_cntrl1.Visible = false;
            adminDashboard1.Visible = false;
            allusersPanel.Visible = false;
            EventPanel.Visible = false;
            ProjectPanle.Visible = true;

            Project pjt = db.Projects.SingleOrDefault(x => x.avg_Rating == null);
            if (pjt != null)
            {

                var data = from pro in db.Projects
                           where pro.Id>0
                           select new { pro.Id, pro.title, pro.avg_Rating, pro.eventCategory.categoryname, pro.publishDate, pro.studentId };
                bunifuCustomDataGrid2.DataSource = data;
            }
            else if (pjt == null)
            {
               getAvgrating();
                var data = from pro in db.Projects
                           where pro.Id>0
                           select new { pro.Id, pro.title, pro.avg_Rating, pro.eventCategory.categoryname, pro.publishDate, pro.studentId };
                bunifuCustomDataGrid2.DataSource = data;
            }

            
            this.bunifuCustomDataGrid2.Sort(this.bunifuCustomDataGrid2.Columns["avg_Rating"], ListSortDirection.Descending);
            this.bunifuCustomDataGrid2.Columns["avg_Rating"].HeaderText = "Average Rating";
            this.bunifuCustomDataGrid2.Columns["title"].HeaderText = "Project Title";
            this.bunifuCustomDataGrid2.Columns["categoryname"].HeaderText = "Category";
            this.bunifuCustomDataGrid2.Columns["publishDate"].HeaderText = "Publishing Date";
            this.bunifuCustomDataGrid2.Columns["studentId"].HeaderText = "Student Id";
        }

        private void bunifuCustomDataGrid1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            id = Int32.Parse(bunifuCustomDataGrid1.Rows[e.RowIndex].Cells["Id"].Value.ToString());
            user usr = db.users.SingleOrDefault(x => x.Id == id);
            if (usr != null)
            {
                viewuser v = new viewuser(usr);
                v.Show();
            }
        }

        private void userfiltercombox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (userfiltercombox.SelectedIndex == -1)
            {
                var data = from usr in db.users
                           where usr.Id > 1
                           select new { usr.Id, usr.name, usr.username, usr.password, usr.email, usr.account_type.types };
                bunifuCustomDataGrid1.DataSource = data;
            }
            else if (userfiltercombox.SelectedIndex != -1)
            {
                var data = from usr in db.users
                           where usr.account_type_id == userfiltercombox.SelectedIndex + 2
                           select new { usr.Id, usr.name, usr.username, usr.password, usr.email, usr.account_type.types };
                bunifuCustomDataGrid1.DataSource = data;
            }
        }

        private void bunifuMaterialTextbox1_OnValueChanged(object sender, EventArgs e)
        {
            var data = from usr in db.users
                       where usr.name.Contains(bunifuMaterialTextbox1.Text) && usr.Id > 1
                       select new { usr.Id, usr.name, usr.username, usr.password, usr.email, usr.account_type.types };
            bunifuCustomDataGrid1.DataSource = data;
        }

        private void bunifuCustomDataGrid2_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int id = Int32.Parse(bunifuCustomDataGrid2.Rows[e.RowIndex].Cells["Id"].Value.ToString());
            Project pr = db.Projects.SingleOrDefault(x => x.Id == id);
            if (pr != null)
            {
                ProjectView p = new ProjectView(pr);
                p.getpublisher(u);
                p.Show();
            }
        }

        private void bunifuMaterialTextbox2_OnValueChanged(object sender, EventArgs e)
        {
            var data = from pro in db.Projects
                       where pro.title.Contains(bunifuMaterialTextbox2.Text)
                       select new { pro.Id, pro.title, pro.avg_Rating, pro.eventCategory.categoryname, pro.publishDate, pro.studentId };
            bunifuCustomDataGrid2.DataSource = data;
            this.bunifuCustomDataGrid2.Columns["avg_Rating"].HeaderText = "Average Rating (out of 5)";
            this.bunifuCustomDataGrid2.Columns["title"].HeaderText = "Project Title";
            this.bunifuCustomDataGrid2.Columns["categoryname"].HeaderText = "Category";
            this.bunifuCustomDataGrid2.Columns["publishDate"].HeaderText = "Publishing Date";
            this.bunifuCustomDataGrid2.Columns["studentId"].HeaderText = "Student Id";
        }

        public void eventscomboupdate()
        {
            DataClasses1DataContext db = new DataClasses1DataContext();
            db.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, db.events);
            comboBox2.DataSource = db.events;
            comboBox2.DisplayMember = "title";
            comboBox2.ValueMember = "id";
            comboBox2.SelectedIndex = -1;
            comboBox2.Text = "Select";

            comboBox3.DataSource = db.events;
            comboBox3.DisplayMember = "title";
            comboBox3.ValueMember = "id";
            comboBox3.SelectedIndex = -1;
            comboBox3.Text = "Select";
        
        }
        private void bunifuFlatButton2_Click_1(object sender, EventArgs e)
        {
            db.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, db.users);
            db.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, db.events);
            admin_profile_usr_cntrl1.Visible = false;
            adminDashboard1.Visible = false;
            allusersPanel.Visible = false;
            ProjectPanle.Visible = false;
            EventPanel.Visible = true;

            var data = from evt in db.events
                       where evt.Id > 0
                       select evt;
            if (data != null)
            {
                eventscomboupdate();
            }
        }

        private void bunifuFlatButton5_Click(object sender, EventArgs e)
        {
            @event evt = new @event();
            evt.title = evttitletextbox.Text;
            evt.startdate = startdatebox.Value;
            evt.enddate = enddatebox.Value;
            db.@events.InsertOnSubmit(evt);
            db.SubmitChanges();
            db.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, db.events);
            eventscomboupdate();
            MessageBox.Show("Event Added!");
            
        }

        private void bunifuFlatButton3_Click_1(object sender, EventArgs e)
        {
            eventCategory ec = new eventCategory();
            ec.categoryname = categorynamebox.Text;
            ec.eventid = Int32.Parse(comboBox3.SelectedValue.ToString());
            db.eventCategories.InsertOnSubmit(ec);
            db.SubmitChanges();
            MessageBox.Show("Category added.\n You can add more category.");
        }
    }
}
