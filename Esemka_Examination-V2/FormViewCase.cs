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
    public partial class FormViewCase : Form
    {
        DataBaseDataContext db = new DataBaseDataContext();
        int case_id;
        int case_detail_id;
        int first_detail_id;
        int last_detail_id;
        int number = 1;
        
        public FormViewCase()
        {
            InitializeComponent();
        }

        void enableField(bool e)
        {
            tbAnswerA.Enabled = tbAnswerB.Enabled = tbAnswerC.Enabled = tbAnswerD.Enabled = tbQuestion.Enabled = cboAnswer.Enabled = !e;
        }

        void enableButton(bool e)
        {
            btnUpdate.Visible = e;
            btnSave.Visible = btnCancel.Visible = !e;
        }

        void enableFieldAndButton(bool e)
        {
            enableButton(e);
            enableField(e);
        }

        void showData()
        {
            dgvData.Columns.Clear();

            var query = db.cases.Where(x => x.deleted_at == null && x.user.name.StartsWith(tbSearch.Text)).ToList()
                .Select(x => new
                {
                    x.id,
                    x.code,
                    creator = x.user.name,
                    x.number_of_questions,
                    x.created_at,
                    x.deleted_at,

                }).ToList();

            dgvData.DataSource = query;
        }

        void showDataDetail()
        {
            var query = db.cases_details.FirstOrDefault(x => x.id == case_detail_id);

            if (query != null)
            {
                tbQuestion.Text = query.text;
                tbAnswerA.Text = query.option_a;
                tbAnswerB.Text = query.option_b;
                tbAnswerC.Text = query.option_c;
                tbAnswerD.Text = query.option_d;

                if (query.correct_answer == tbAnswerA.Text)
                {
                    cboAnswer.Text = "A";
                }

                else if (query.correct_answer == tbAnswerB.Text)
                {
                    cboAnswer.Text = "B";
                }

                else if (query.correct_answer == tbAnswerC.Text)
                {
                    cboAnswer.Text = "C";
                }

                else
                {
                    cboAnswer.Text = "D";
                }

                var queryLastid = db.cases_details.Where(x => x.case_id == case_id).Count();
                last_detail_id = queryLastid;
                lblQuestion.Text = "Question " + number + "/" + queryLastid;
            }
        }

        private void FormViewCase_Load(object sender, EventArgs e)
        {

            Support.enableField(this);
            enableFieldAndButton(true);
            btnPrev.Enabled = false;

            var list = new List<string>();
            list.Add("A");
            list.Add("B");
            list.Add("C");
            list.Add("D");

            cboAnswer.DataSource = list;

            showData();
        }

        private void dgvData_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                case_id = (int)dgvData.Rows[e.RowIndex].Cells["id"].Value;
                var query = db.cases_details.FirstOrDefault(x => x.case_id == case_id);

                if (query != null)
                {
                    case_detail_id = query.id;
                    first_detail_id = query.id;
                }
                number = 1;
                showDataDetail();
            }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            number++;
            case_detail_id++;
            showDataDetail();
            if (number == last_detail_id)
            {
                btnNext.Enabled = false;
            }

            btnPrev.Enabled = true;
        }

        private void btnPrev_Click(object sender, EventArgs e)
        {
            number--;
            case_detail_id--;
            showDataDetail();
            if (number == 1)
            {
                btnPrev.Enabled = false;
            }

            btnNext.Enabled = true;
            
        }

        private void btnLast_Click(object sender, EventArgs e)
        {
            case_detail_id = last_detail_id;
            number = last_detail_id;
            showDataDetail();
            btnNext.Enabled = false;
            btnPrev.Enabled = true;
        }

        private void btnFirst_Click(object sender, EventArgs e)
        {
            case_detail_id = first_detail_id;
            number = 1;
            showDataDetail();
            btnPrev.Enabled = false;
            btnNext.Enabled = true;
        }

        private void tbSearch_TextChanged(object sender, EventArgs e)
        {
            showData();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            enableFieldAndButton(false);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            var query = db.cases_details.FirstOrDefault(x => x.id == case_detail_id);
            query.text = tbQuestion.Text;
            query.option_a = tbAnswerA.Text;
            query.option_b = tbAnswerB.Text;
            query.option_c = tbAnswerC.Text;
            query.option_d = tbAnswerD.Text;

            if (cboAnswer.Text == "A")
            {
                query.correct_answer = tbAnswerA.Text;
            }

            else if (cboAnswer.Text == "B")
            {
                query.correct_answer = tbAnswerB.Text;
            }

            else if (cboAnswer.Text == "C")
            {
                query.correct_answer = tbAnswerC.Text;
            }

            else query.correct_answer = tbAnswerD.Text;

            db.SubmitChanges();
            Support.msi("update data success");
            showData();
            Support.clearField(this);
            enableFieldAndButton(true);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            enableFieldAndButton(true);
        }
    }
}
