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
    public partial class FormNewQuestion : Form
    {

        public FormNewQuestion()
        {
            InitializeComponent();
        }

        public static string text;
        public static string option_a;
        public static string option_b;
        public static string option_c;
        public static string option_d;
        public static string correct_answer;

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            if (tbQuestion.Text == "" || tbAnswerA.Text == "" || tbAnswerB.Text == ""
                || tbAnswerC.Text == "" || tbAnswerD.Text == "")
            {
                Support.msw("All fields must be filled");
                return;
            }

            text = tbQuestion.Text;
            option_a = tbAnswerA.Text;
            option_b = tbAnswerB.Text;
            option_c = tbAnswerC.Text;
            option_d = tbAnswerD.Text;

            if (cboAnswer.Text == "A")
            {
                correct_answer = tbAnswerA.Text;
            }

            else if (cboAnswer.Text == "B")
            {
                correct_answer = tbAnswerB.Text;
            }

            else if (cboAnswer.Text == "C")
            {
                correct_answer = tbAnswerC.Text;
            }

            else correct_answer = tbAnswerD.Text;

            Close();
        }

        private void FormNewQuestion_Load(object sender, EventArgs e)
        {
            Support.enableField(this);
            var list = new List<string>();
            list.Add("A");
            list.Add("B");
            list.Add("C");
            list.Add("D");
            cboAnswer.DataSource = list;
        }
    }
}
