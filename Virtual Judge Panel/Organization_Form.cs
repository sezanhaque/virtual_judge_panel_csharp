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
    public partial class Organization_Form : Form
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        user u;
        public Organization_Form(user u)
        {
            InitializeComponent();
            this.u = u;
        }
        private void Judge_Profile_Form_Load(object sender, EventArgs e)
        {
            db.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, db.users);
            orgDashboard1.setinfo(u);
            Menu_Minimize.Visible = false;
            Logo_Minimize.Visible = false;
            ProjectPanle.Visible = false;
            Dashboardbtn.selected = true;
            org_profile_user_cntrl1.Visible = false;
            orgDashboard1.Visible = true;
            orgDashboard1.setinfo(u);

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

        private void bunifuFlatButton4_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            this.RefToForm1.Show();
        }

        private void Dashboardbtn_Click(object sender, EventArgs e)
        {
            org_profile_user_cntrl1.Visible = false;
            ProjectPanle.Visible = false;
            orgDashboard1.Visible = true;
        }

        private void bunifuFlatButton2_Click(object sender, EventArgs e)
        {
            db.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, db.users);
            user usr = db.users.SingleOrDefault(x => x.Id == u.Id);
            orgDashboard1.Visible = false;
            ProjectPanle.Visible = false;
            org_profile_user_cntrl1.Visible = true;
            org_profile_user_cntrl1.setinfo(usr);
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
                db.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, db.users);
            }

        }

        private void bunifuFlatButton3_Click(object sender, EventArgs e)
        {
            DataClasses1DataContext db = new DataClasses1DataContext();
            db.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, db.Projects);
            db.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, db.users);
            db.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, db.events);
            db.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, db.eventCategories);
            db.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, db.Ratings);
            
            orgDashboard1.Visible = false;
            org_profile_user_cntrl1.Visible = false;
            ProjectPanle.Visible = true;

            Project pjt = db.Projects.SingleOrDefault(x => x.avg_Rating == null);
            if (pjt != null)
            {

                var data = from pro in db.Projects
                           where pro.Id > 0
                           select new { pro.Id, pro.title, pro.avg_Rating, pro.eventCategory.categoryname, pro.publishDate, pro.studentId };
                bunifuCustomDataGrid2.DataSource = data;
            }
            else if (pjt == null)
            {
                db.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, db.users);
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

        private void bunifuCustomDataGrid2_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int id = Int32.Parse(bunifuCustomDataGrid2.Rows[e.RowIndex].Cells["Id"].Value.ToString());
            Project pr = db.Projects.SingleOrDefault(x => x.Id == id);
            if (pr != null)
            {
                ProjectView p = new ProjectView(pr);
                p.forOrg(u);
                p.Show();

            }
        }
    }
}
