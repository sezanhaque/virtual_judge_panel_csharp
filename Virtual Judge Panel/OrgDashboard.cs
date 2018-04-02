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
    public partial class OrgDashboard : UserControl
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        user u;
        public OrgDashboard()
        {
            InitializeComponent();
        }

        public void setinfo(user u)
        {
            this.u = u;


            if (u.name == null && u.security_question == null && u.img_path == null)
            {
                bunifuCircleProgressbar1.Value = 40;
                bunifuCustomLabel3.Text = db.Projects.Count().ToString();
            }

            else if (u.name != null && u.security_question != null && u.img_path == null || u.img_path != null && u.name == null && u.security_question == null)
            {
                bunifuCircleProgressbar1.Value = 80;
                bunifuCustomLabel3.Text = db.Projects.Count().ToString();
            }
            else if (u.name != null && u.security_question != null && u.img_path != null)
            {
                bunifuCircleProgressbar1.Value = 90;
                bunifuCustomLabel3.Text = db.Projects.Count().ToString();
            }
        }
    }
}
