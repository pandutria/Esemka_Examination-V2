using Esemka_Examination_V2.Data;
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
    public partial class FormLogin : Form
    {
        DataBaseDataContext db = new DataBaseDataContext();
        public FormLogin()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Support.enableField(this);
            bunifuCheckBox1.Checked = true;

            tbUsername.Text = tbPassword.Text = "root";
        }

        private void bunifuButton2_Click(object sender, EventArgs e)
        {
            tbUsername.Clear();
            tbPassword.Clear();
        }

        private void bunifuCheckBox1_CheckedChanged(object sender, Bunifu.UI.WinForms.BunifuCheckBox.CheckedChangedEventArgs e)
        {
            if (bunifuCheckBox1.Checked)
            {
                tbPassword.UseSystemPasswordChar = false;

            } else
            {
                tbPassword.UseSystemPasswordChar = true;
            }
        }

        public static user user;

        private void bunifuButton1_Click(object sender, EventArgs e)
        {
            if (tbUsername.Text == "" || tbPassword.Text == "")
            {
                Support.msw("All field must be filled");
                return;
            }

            var query = db.users.FirstOrDefault(x => x.username == tbUsername.Text && x.password == tbPassword.Text);

            if (query != null)
            {
                user = query;
                new FormMain(query.name).Show();
                Hide();
            } else
            {
                Support.msw("Your data is not valid, please try again!!");
            }
        }
    }
}
