using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Linq;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Virtual_Judge_Panel
{
    public partial class ProjectView : Form
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        Project p;
        user u;
        public ProjectView(Project p)
        {
            InitializeComponent();
            this.p = p;
        }

        public void getpublisher(user u)
        {
            this.u = u;
        }

        public void forOrg( user u)
        {
            this.u = u;
            bunifuFlatButton1.Visible = false;
            bunifuFlatButton2.Visible = false;
            bunifuCustomTextbox1.ReadOnly = true;
            bunifuCustomTextbox2.ReadOnly = true;
            bunifuCustomTextbox3.ReadOnly = true;
            bunifuRating1.Visible = true;
            label1.Visible = true;
        }

        private void bunifuImageButton2_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void Close_Button_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ProjectView_Load(object sender, EventArgs e)
        {
            bunifuCustomTextbox1.Text = p.title;
            bunifuCustomTextbox2.Text = p.discription;
            bunifuCustomTextbox3.Text = p.videolink;
            axShockwaveFlash1.Movie = p.videolink;
            bunifuCustomTextbox4.Text = p.filelink;

            
            var cmnt = from cm in db.Comments
                        where cm.project_id == p.Id
                        select cm;
            foreach (var i in cmnt)
            {
                Label l = new Label();
                l.Text = i.comment_text;
                l.ForeColor = Color.Blue;
                l.Font = new Font("Arial", 14, FontStyle.Bold);
                l.AutoSize = true;
                flowLayoutPanel1.Controls.Add(l);
            }

            var data = from cc in db.eventCategories
                       where cc.Id==p.categoryId
                       select cc;
            foreach (var i in data)
            {
                var data2 = from evt in db.events
                            where evt.Id == i.eventid
                            select evt;
                foreach (var j in data2)
                {
                    var data3 = from ec in db.eventCategories
                                where ec.eventid == j.Id
                                select ec;
                    comboBox1.DataSource = data3;
                    comboBox1.DisplayMember = "categoryname";
                    comboBox1.ValueMember = "id";
                    comboBox1.SelectedValue = p.categoryId;
                }
            }

            
        }

        private void bunifuFlatButton1_Click(object sender, EventArgs e)
        {
            Project pg = db.Projects.SingleOrDefault(x=>x.Id == p.Id);
            if (pg != null)
            {
                bunifuCustomTextbox1.Text = "";
                bunifuCustomTextbox2.Text = "";
                bunifuCustomTextbox3.Text = "";
                bunifuCustomTextbox4.Text = "";
                axShockwaveFlash1.Movie = null;
                db.Projects.DeleteOnSubmit(pg);
                db.SubmitChanges();
                MessageBox.Show("Project Deleted!!");
            }
            db.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, db.Projects);
        }

        private void bunifuFlatButton2_Click(object sender, EventArgs e)
        {
            Project pg = db.Projects.SingleOrDefault(x => x.Id == p.Id);
            if (pg != null)
            {
                pg.title = bunifuCustomTextbox1.Text;
                pg.discription = bunifuCustomTextbox2.Text;
                pg.videolink = bunifuCustomTextbox3.Text;
                axShockwaveFlash1.Movie = bunifuCustomTextbox3.Text;
                pg.filelink = bunifuCustomTextbox4.Text;
                pg.categoryId = Int32.Parse(comboBox1.SelectedValue.ToString());
                db.SubmitChanges();
                MessageBox.Show("Project Updated!!");
            }
            db.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, db.Projects);
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

        private void bunifuRating1_onValueChanged(object sender, EventArgs e)
        {
            try
            {
                DataClasses1DataContext db = new DataClasses1DataContext();
                Rating r = db.Ratings.SingleOrDefault(x => x.projectid == p.Id && x.uid == u.Id);
                if (r == null)
                {
                    Rating rr = new Rating();
                    rr.rating1 = bunifuRating1.Value;
                    rr.uid = u.Id;
                    rr.projectid = p.Id;
                    p.avg_Rating = 0;
                    db.Ratings.InsertOnSubmit(rr);
                    db.SubmitChanges();
                    getAvgrating();
                    db.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, db.Projects);

                }
                else if (r != null)
                {
                    r.rating1 = bunifuRating1.Value;
                    r.uid = u.Id;
                    r.projectid = p.Id;
                    db.SubmitChanges();
                    getAvgrating();
                    db.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, db.Projects);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            
            
          
        }

        private void bunifuFlatButton3_Click(object sender, EventArgs e)
        {
            
        }

        private void bunifuFlatButton3_Click_1(object sender, EventArgs e)
        {
            Comment c = new Comment();
            Label l = new Label();
            l.Text =textBox1.Text +"  -By  "+ this.u.username.ToString();
            l.ForeColor = Color.Blue;
            l.Font = new Font("Arial", 14, FontStyle.Bold);
            l.AutoSize = true;
            c.comment_text = l.Text;
            c.uid = this.u.Id;
            c.project_id = this.p.Id;
            flowLayoutPanel1.Controls.Add(l);
            db.Comments.InsertOnSubmit(c);
            db.SubmitChanges();
        }
    }
}
