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
    public partial class adminDashboard : UserControl
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        user u;
        public adminDashboard()
        {
            InitializeComponent();
        }

        public void setdashinfo()
        {
            //For totat students
            var data = from us in db.users
                       where us.account_type_id == 2
                       select us;
            int c = 0;
            foreach (user i in data)
            {
                c += 1;
            }
            totalusertext.Text = c.ToString();

            //For total Organizations

            var data2 = from us in db.users
                        where us.account_type_id == 3
                        select us;
            int c2 = 0;
            foreach (user i in data2)
            {
                c2 += 1;
            }
            bunifuCustomLabel5.Text = c2.ToString();

            //for total projects
            bunifuCustomLabel3.Text = db.Projects.Count().ToString();
        
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

        private void adminDashboard_Load(object sender, EventArgs e)
        {
            
        }
    }
}
