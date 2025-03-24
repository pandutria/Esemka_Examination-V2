using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Esemka_Examination_V2
{
    public partial class FormType : Form
    {
        DataBaseDataContext db = new DataBaseDataContext();
        public FormType()
        {
            InitializeComponent();
        }

        void enable(bool e)
        {
            tbCode.Enabled = tbName.Enabled =!e;
        }

        void visible(bool v)
        {
            btnInsert.Visible = btnUpdate.Visible = btnDelete.Visible = v;

            btnSave.Visible = btnCancel.Visible = !v;
        }

        void enableAndVisible(bool e)
        {
            enable(e);
            visible(e);
        }

        void showData()
        {

            dgvData.Columns.Clear();

            var query = db.types.Where(x => x.deleted_at == null).ToList();

            dgvData.DataSource = query.Select(x => new
            {
                x.id,
                x.code,
                x.name,
                x.created_at,
                x.deleted_at
            }).ToList();
        }

        private void FormType_Load(object sender, EventArgs e)
        {
            Support.enableField(this);
            showData();

            enableAndVisible(true);
            tbId.Enabled = false;
        }

        private void tbSearch_TextChanged(object sender, EventArgs e)
        {
            showData();
        }

        private void dgvData_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                tbId.Text = dgvData.Rows[e.RowIndex].Cells["id"].Value.ToString();
                tbName.Text = dgvData.Rows[e.RowIndex].Cells["name"].Value.ToString();
                tbCode.Text = dgvData.Rows[e.RowIndex].Cells["code"].Value.ToString();
            }
        }

        string mode;

        private void btnInsert_Click(object sender, EventArgs e)
        {
            mode = "insert";
            enableAndVisible(false);
            Support.clearField(this);
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (tbId.Text == "")
            {
                Support.msw("Click row !!!");
                return;
            }

            mode = "update";
            enableAndVisible(false);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (tbCode.Text == "" || tbName.Text == "")
            {
                Support.msw("All fields must be filled");
                return;
            }


            if (tbCode.Text.Length != 3 && tbCode.Text.Length != 5)
            {
                Support.msw("Field code Must be either XXX or XXXYY, where X is alphabet A-Z and Y is number 0-9");
                return;
            }

            if (tbCode.Text.Length == 3)
            {
                if (!tbCode.Text.All(x => char.IsLetter(x)) || tbCode.Text != tbCode.Text.ToUpper())
                {
                    Support.msw("Field code Must be either XXX or XXXYY, where X is alphabet A-Z and Y is number 0-9");
                    return;
                }
            }

            if (tbCode.Text.Length == 5)
            {
                if (!tbCode.Text.Substring(0, 1).All(x => char.IsLetter(x)) || !tbCode.Text.Substring(0, 2).All(x => char.IsLetter(x)) ||
                    !tbCode.Text.Substring(0, 3).All(x => char.IsLetter(x)) || !tbCode.Text.Substring(3, 1).All(x => char.IsDigit(x)) || !tbCode.Text.Substring(3, 2).All(x => char.IsDigit(x))
                    || tbCode.Text.Substring(0, 1) != tbCode.Text.ToUpper().Substring(0, 1) || tbCode.Text.Substring(0, 2) != tbCode.Text.ToUpper().Substring(0, 2) || tbCode.Text.Substring(0, 3) != tbCode.Text.ToUpper().Substring(0, 3))
                {
                    Support.msw("Field code Must be either XXX or XXXYY, where X is alphabet A-Z and Y is number 0-9");
                    return;
                }
            }

            if (mode == "insert")
            {
                var query = new type();
                query.code = tbCode.Text;
                query.name = tbName.Text;
                query.created_at = DateTime.Now;

                db.types.InsertOnSubmit(query);
                db.SubmitChanges();
                showData();
                enableAndVisible(true);
                Support.clearField(this);
                Support.msi("Insert data success");
            }

            if (mode == "update")
            {
                var query = db.types.FirstOrDefault(x => x.id == Convert.ToInt32(tbId.Text));

                if (query != null)
                {
                    query.code = tbCode.Text;
                    query.name = tbName.Text;

                    db.SubmitChanges();
                    showData();
                    enableAndVisible(true);
                    Support.clearField(this);
                    Support.msi("Update data success");
                }
            }
        }

        private void tbCode_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (tbId.Text == "")
            {
                Support.msw("Click row !!!");
                return;
            }

            var dialog = MessageBox.Show("Are you sure want to delete this data?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (dialog == DialogResult.Yes)
            {
                var query = db.types.First(x => x.id == Convert.ToInt32(tbId.Text));

                query.deleted_at = DateTime.Now;
                db.SubmitChanges();
                showData();
                enableAndVisible(true);
                Support.clearField(this);
                Support.msi("Delete data success");
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            enableAndVisible(true);
            Support.clearField(this);
        }
    }
}
