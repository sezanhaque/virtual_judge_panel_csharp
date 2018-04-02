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
    public partial class studentDashboard : UserControl
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        user u;
        public studentDashboard()
        {
            InitializeComponent();
        }

        public void setdashinfo()
        {

            var data = from pro in db.Projects
                       where pro.studentId == u.Id
                       select pro;
            int c = 0;
            foreach (Project i in data)
            {
                c += 1;
            }
            yourprojecttext.Text = c.ToString();
        }

        public void setinfo(user u)
        {
            this.u = u;


            if (u.name == null && u.security_question == null && u.img_path == null)
            {
                bunifuCircleProgressbar1.Value = 40;
                setdashinfo();
            }

            else if (u.name != null && u.security_question != null && u.img_path == null || u.img_path != null && u.name == null && u.security_question == null)
            {
                bunifuCircleProgressbar1.Value = 80;
                setdashinfo();
            }
            else if (u.name != null && u.security_question != null && u.img_path != null)
            {
                bunifuCircleProgressbar1.Value = 90;
                setdashinfo();
            }
        }
    }
}
