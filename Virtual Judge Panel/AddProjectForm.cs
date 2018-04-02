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
    public partial class AddProjectForm : Form
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        user u;
        @event evt;
        public AddProjectForm(user u, @event evt)
        {
            InitializeComponent();
            this.u = u;
            this.evt=evt;
        }

        private void bunifuImageButton2_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        public Form ReftoAdd { get; set; }
        private void Close_Button_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void bunifuFlatButton2_Click(object sender, EventArgs e)
        {


            if (!titletextbox.Text.Equals("") || !discriptiontextbox.Text.Equals(""))
            {

                string titles = titletextbox.Text;
                string[] lst = titles.Split(new Char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
                string discriptions = discriptiontextbox.Text;
                string[] lst2 = discriptions.Split(new Char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

                Project p = new Project();
                
                p.title = titles;
                p.discription = discriptions;
                p.videolink = videotextbox.Text;
                p.categoryId = Int32.Parse(comboBox1.SelectedValue.ToString());
                p.studentId = u.Id;
                p.publishDate = DateTime.Today;
                p.filelink = textBox1.Text;
                db.Projects.InsertOnSubmit(p);
                db.SubmitChanges();
                MessageBox.Show("Project added!");


            }
            else
            {
                MessageBox.Show("No field can be empty!!");
            }



        }

        private void AddProjectForm_Load(object sender, EventArgs e)
        {
            var data = from cc in db.eventCategories
                       where cc.eventid == this.evt.Id
                       select cc;

            comboBox1.DataSource = data;
            comboBox1.DisplayMember = "categoryname";
            comboBox1.ValueMember = "id";

        }

        private void bunifuFlatButton1_Click(object sender, EventArgs e)
        {
           
        }
    }
}
