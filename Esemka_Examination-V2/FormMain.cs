using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Esemka_Examination_V2
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
        }

        private void btnType_Click(object sender, EventArgs e)
        {
            panelContainer.Controls.Clear();
            var f = new FormType();
            f.TopLevel = false;

            panelContainer.Controls.Add(f);
            f.Show();

            btnType.BackColor = Color.FromArgb(251, 168, 52);

            btnUser.BackColor = Color.DodgerBlue;
            btnRoom.BackColor = Color.DodgerBlue;
            btnNewCase.BackColor = Color.DodgerBlue;
            btnViewCase.BackColor = Color.DodgerBlue;
            btnSchedule.BackColor = Color.DodgerBlue; 
        }

        private void btnRoom_Click(object sender, EventArgs e)
        {
            panelContainer.Controls.Clear();
            var f = new FormRoom();
            f.TopLevel = false;

            panelContainer.Controls.Add(f);
            f.Show();

            btnRoom.BackColor = Color.FromArgb(251, 168, 52);

            btnUser.BackColor = Color.DodgerBlue;
            btnType.BackColor = Color.DodgerBlue;
            btnNewCase.BackColor = Color.DodgerBlue;
            btnViewCase.BackColor = Color.DodgerBlue;
            btnSchedule.BackColor = Color.DodgerBlue;
        }

        private void btnUser_Click(object sender, EventArgs e)
        {

            panelContainer.Controls.Clear();
            var f = new FormUser();
            f.TopLevel = false;

            panelContainer.Controls.Add(f);
            f.Show();

            btnUser.BackColor = Color.FromArgb(251, 168, 52);

            btnRoom.BackColor = Color.DodgerBlue;
            btnType.BackColor = Color.DodgerBlue;
            btnNewCase.BackColor = Color.DodgerBlue;
            btnViewCase.BackColor = Color.DodgerBlue;
            btnSchedule.BackColor = Color.DodgerBlue;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (panelMaster.Visible == true)
            {
                panelMaster.Visible = false;
            } else
            {
                panelMaster.Visible = true;
            }
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            panelMaster.Visible = false;
            panelCase.Visible = false;
            panelSchedule.Visible = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (panelCase.Visible == true)
            {
                panelCase.Visible = false;
            }
            else
            {
                panelCase.Visible = true;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (panelSchedule.Visible == true)
            {
                panelSchedule.Visible = false;
            }
            else
            {
                panelSchedule.Visible = true;
            }
        }

        private void btnViewCase_Click(object sender, EventArgs e)
        {
            btnViewCase.BackColor = Color.FromArgb(251, 168, 52);

            btnUser.BackColor = Color.DodgerBlue;
            btnRoom.BackColor = Color.DodgerBlue;
            btnNewCase.BackColor = Color.DodgerBlue;
            btnType.BackColor = Color.DodgerBlue;
            btnSchedule.BackColor = Color.DodgerBlue;
        }

        private void btnNewCase_Click(object sender, EventArgs e)
        {
            btnNewCase.BackColor = Color.FromArgb(251, 168, 52);

            btnUser.BackColor = Color.DodgerBlue;
            btnRoom.BackColor = Color.DodgerBlue;
            btnType.BackColor = Color.DodgerBlue;
            btnViewCase.BackColor = Color.DodgerBlue;
            btnSchedule.BackColor = Color.DodgerBlue;
        }

        private void btnSchedule_Click(object sender, EventArgs e)
        {
            btnSchedule.BackColor = Color.FromArgb(251, 168, 52);

            btnUser.BackColor = Color.DodgerBlue;
            btnRoom.BackColor = Color.DodgerBlue;
            btnNewCase.BackColor = Color.DodgerBlue;
            btnViewCase.BackColor = Color.DodgerBlue;
            btnType.BackColor = Color.DodgerBlue;
        }
    }
}
