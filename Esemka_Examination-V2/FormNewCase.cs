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
    public partial class FormNewCase : Form
    {
        public FormNewCase()
        {
            InitializeComponent();
        }

        void countDgv()
        {
            var countDgv = dgvData.Rows.Count - 1;
            tbNumber.Text = countDgv.ToString();
        }

        private void FormNewCase_Load(object sender, EventArgs e)
        {
            Support.enableField(this);
            tbNumber.Enabled = false;
            tbNumber.Text = 0.ToString();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            new FormNewQuestion().ShowDialog();

            dgvData.Rows.Add(FormNewQuestion.text, FormNewQuestion.option_a, FormNewQuestion.option_b, FormNewQuestion.option_c, FormNewQuestion.option_d, FormNewQuestion.correct_answer, btnAction.Text = "Remove");
            countDgv();
        }

        private void dgvData_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dgvData.Columns["btnAction"].Index && e.RowIndex >= 0)
            {
                dgvData.Rows.RemoveAt(e.RowIndex);
                countDgv();
            }
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            if (tbCode.Text.Length != 9)
            {
                Support.msw("Field  code Must be CASEXXXXX, where X is number 0-9");
                return;
            }

            if (tbCode.Text.Substring(0, 1) != "C" || tbCode.Text.Substring(1, 1) != "A" || tbCode.Text.Substring(2, 1) != "S"
                || tbCode.Text.Substring(3, 1) != "E" || !tbCode.Text.Substring(4, 5).All(x => char.IsDigit(x)))
            {
                Support.msw("Field  code Must be CASEXXXXX, where X is number 0-9");
                return;
            }

            if (tbNumber.Text == "")
            {
                Support.msw("Question must be add");
                return;
            }

            var db = new DataBaseDataContext();

            var query = new @case();
            query.creator_id = FormLogin.user.id;
            query.code = tbCode.Text;
            query.number_of_questions = Convert.ToInt32( tbNumber.Text);
            query.created_at = DateTime.Now;

            db.cases.InsertOnSubmit(query);
            db.SubmitChanges();

            foreach (DataGridViewRow row in dgvData.Rows)
            {
                if (row.Cells[0].Value != null)
                {

                    var queryDetail = new cases_detail();
                    queryDetail.case_id = query.id;
                    queryDetail.text = row.Cells[0].Value.ToString();
                    queryDetail.option_a = row.Cells[1].Value.ToString();
                    queryDetail.option_b = row.Cells[2].Value.ToString();
                    queryDetail.option_c = row.Cells[3].Value.ToString();
                    queryDetail.option_d = row.Cells[4].Value.ToString();
                    queryDetail.correct_answer = row.Cells[5].Value.ToString();
                    queryDetail.created_at = DateTime.Now;

                    db.cases_details.InsertOnSubmit(queryDetail);
                    db.SubmitChanges();
                }

            }

            db.SubmitChanges();
            Support.msi("submit data success");
            Support.clearField(this);
            
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Support.clearField(this);
        }
    }
}
