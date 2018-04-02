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
    public partial class Student_Form : Form
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        user u;
        public Student_Form(user u)
        {
            InitializeComponent();
            this.u = u;
        }
        private void Student_Profile_Form_Load(object sender, EventArgs e)
        {
            db.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, db.users);
            studentDashboard1.setinfo(u);
            Menu_Minimize.Visible = false;
            Logo_Minimize.Visible = false;
            currenteventpanel.Visible = false;
            dashboardbtn.selected = true;
            student_profile_user_cntrl1.Visible = false;
            StudentProjectPanel.Visible = false;
            studentDashboard1.Visible = true;
        }
        
        public Form RefToForm1 { get; set; }
       
        private void Close_Button_Click(object sender, EventArgs e)
        {
            this.Close();
            this.RefToForm1.Show();
            //this.Visible = false;
            //Environment.Exit(0);
            /*Form1 f = new Form1();
            f.Show();*/
        }

        private void bunifuImageButton2_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
        
        private void bunifuFlatButton4_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            this.RefToForm1.Show();
        }

        private void Menu_Minimize_Click(object sender, EventArgs e)
        {
            Menu.Visible = true;
            Logo.Visible = true;
            Menu_Minimize.Visible = false;
            Logo_Minimize.Visible = false;
            Slide_Panel.Visible = false;
            Slide_Panel.Width = 215;
            Panel_Animator.ShowSync(Slide_Panel);
        }

        private void Menu_Click(object sender, EventArgs e)
        {
            Slide_Panel.Visible = false;
            Menu_Minimize.Visible = true;
            Logo_Minimize.Visible = true;
            Slide_Panel.Width = 50;
            Panel_Animator.ShowSync(Slide_Panel);
        }

        private void dashboardbtn_Click(object sender, EventArgs e)
        {
            db.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, db.users);
            user ur = db.users.SingleOrDefault(x => x.Id == u.Id);
            studentDashboard1.setinfo(ur);
            student_profile_user_cntrl1.Visible = false;
            StudentProjectPanel.Visible = false;
            currenteventpanel.Visible = false;
            studentDashboard1.Visible = true;
        }

        private void bunifuFlatButton2_Click(object sender, EventArgs e)
        {
            db.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, db.users);
            user usr = db.users.SingleOrDefault(x => x.Id == u.Id);
            student_profile_user_cntrl1.setinfo(usr);
            studentDashboard1.Visible = false;
            StudentProjectPanel.Visible = false;
            currenteventpanel.Visible = false;
            student_profile_user_cntrl1.Visible = true;
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

        private void bunifuFlatButton3_Click(object sender, EventArgs e)
        {
            DataClasses1DataContext db = new DataClasses1DataContext();
            db.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, db.Projects);
            studentDashboard1.Visible = false;
            student_profile_user_cntrl1.Visible = false;
            currenteventpanel.Visible = false;
            StudentProjectPanel.Visible = true;

            Project pjt = db.Projects.SingleOrDefault(x => x.avg_Rating == null);
            if (pjt != null)
            {

                var data = from pro in db.Projects
                           where pro.studentId == u.Id
                           select new { pro.Id, pro.title, pro.avg_Rating, pro.eventCategory.categoryname, pro.publishDate, pro.studentId };
                bunifuCustomDataGrid1.DataSource = data;
            }
            else if (pjt == null)
            {
                getAvgrating();
                var data = from pro in db.Projects
                           where pro.studentId == u.Id
                           select new { pro.Id, pro.title, pro.avg_Rating, pro.eventCategory.categoryname, pro.publishDate, pro.studentId };
                bunifuCustomDataGrid1.DataSource = data;
            }

                this.bunifuCustomDataGrid1.Sort(this.bunifuCustomDataGrid1.Columns["avg_Rating"], ListSortDirection.Descending);
                this.bunifuCustomDataGrid1.Columns["avg_Rating"].HeaderText = "Average Rating";
                this.bunifuCustomDataGrid1.Columns["title"].HeaderText = "Project Title";
                this.bunifuCustomDataGrid1.Columns["categoryname"].HeaderText = "Category";
                this.bunifuCustomDataGrid1.Columns["publishDate"].HeaderText = "Publishing Date";
                this.bunifuCustomDataGrid1.Columns["studentId"].HeaderText = "Student Id";
        }

        private void bunifuCustomDataGrid1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int id = Int32.Parse(bunifuCustomDataGrid1.Rows[e.RowIndex].Cells["Id"].Value.ToString());
            Project pr = db.Projects.SingleOrDefault(x => x.Id == id);
            if (pr != null)
            {
                ProjectView p = new ProjectView(pr);
                p.getpublisher(u);
                p.Show();
            }
        }

        private void bunifuMaterialTextbox1_OnValueChanged(object sender, EventArgs e)
        {
            var data = from pro in db.Projects
                       where pro.title.Contains(bunifuMaterialTextbox1.Text) && pro.studentId==u.Id
                       select new { pro.Id, pro.title, pro.avg_Rating, pro.eventCategory.categoryname, pro.publishDate, pro.studentId };
            bunifuCustomDataGrid1.DataSource = data;

            this.bunifuCustomDataGrid1.Columns["avg_Rating"].HeaderText = "Average Rating (out of 5)";
            this.bunifuCustomDataGrid1.Columns["title"].HeaderText = "Project Title";
            this.bunifuCustomDataGrid1.Columns["categoryname"].HeaderText = "Category";
            this.bunifuCustomDataGrid1.Columns["publishDate"].HeaderText = "Publishing Date";
            this.bunifuCustomDataGrid1.Columns["studentId"].HeaderText = "Student Id";
        }

        private void bunifuFlatButton8_Click(object sender, EventArgs e)
        {
            studentDashboard1.Visible = false;
            student_profile_user_cntrl1.Visible = false;
            StudentProjectPanel.Visible = false;
            currenteventpanel.Visible = true;
            
            var data = from eb in db.events
                       where eb.enddate >= DateTime.Today
                       select new {eb.Id, eb.title, eb.startdate, eb.enddate};
                bunifuCustomDataGrid2.DataSource = data;

            this.bunifuCustomDataGrid2.Columns["id"].HeaderText = "Id Of Event";
            this.bunifuCustomDataGrid2.Columns["title"].HeaderText = "Title Of Event";
            this.bunifuCustomDataGrid2.Columns["startdate"].HeaderText = "Start Date";
            this.bunifuCustomDataGrid2.Columns["enddate"].HeaderText = "End Date";
        }

        private void bunifuCustomDataGrid2_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int id = Int32.Parse(bunifuCustomDataGrid2.Rows[e.RowIndex].Cells["Id"].Value.ToString());
            @event evt = db.events.SingleOrDefault(x => x.Id == id);
            if (evt != null)
            {
                AddProjectForm af = new AddProjectForm(u, evt);
                af.ReftoAdd = this;
                af.Show();

            }
        }
    }
}
