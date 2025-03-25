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
            Support.enableField(this);
        }

        private void bunifuPanel2_Click(object sender, EventArgs e)
        {

        }

        private void bunifuPanel1_Click(object sender, EventArgs e)
        {

        }

        private void bunifuButton1_Click(object sender, EventArgs e)
        {
            new FormLogin().Show();
            Hide();
        }

        private void bunifuButton2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void FormMDI_Load(object sender, EventArgs e)
        {
            Support.enableField(this);
        }
    }
}
