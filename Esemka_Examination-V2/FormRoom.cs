using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace Esemka_Examination_V2
{
    public partial class FormRoom : Form
    {
        DataBaseDataContext db = new DataBaseDataContext();
        public FormRoom()
        {
            InitializeComponent();
        }

        void enable(bool e)
        {
            tbCode.Enabled = tbCapacity.Enabled = !e;
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

            var query = db.rooms.Where(x => x.deleted_at == null).ToList();

            dgvData.DataSource = query.Select(x => new
            {
                x.id,
                x.code,
                x.capacity,
                x.created_at,
                x.deleted_at
            }).ToList();
        }

        private void FormRoom_Load(object sender, EventArgs e)
        {
            Support.enableField(this);
            showData();

            enableAndVisible(true);
            tbId.Enabled = false;
        }

        private void dgvData_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                tbId.Text = dgvData.Rows[e.RowIndex].Cells["id"].Value.ToString();
                tbCapacity.Text = dgvData.Rows[e.RowIndex].Cells["capacity"].Value.ToString();
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
            if (tbCode.Text == "" || tbCapacity.Text == "")
            {
                Support.msw("All fields must be filled");
                return;
            }

            if (!tbCode.Text.Substring(0, 1).Any(x => char.IsLetter(x)) || tbCode.Text.Substring(0, 1) != tbCode.Text.ToUpper().Substring(0, 1) 
                || !tbCode.Text.Substring(1, 3).All(x => char.IsDigit(x)) || tbCode.Text.Length != 4)
            {
                Support.msw("Field code Must be XYYY, where X is alphabet A-Z and Y is number 0-9");
                return;
            }

            if (Convert.ToInt32( tbCapacity.Text) < 1 || !tbCapacity.Text.All(x => char.IsDigit(x)))
            {
                Support.msw("Field capacity Must be numeric greater than 0");
                return;
            }

            try
            {
                if (mode == "insert")
                {
                    var query = new room();
                    query.code = tbCode.Text;
                    query.capacity = Convert.ToInt32( tbCapacity.Text);
                    query.created_at = DateTime.Now;

                    db.rooms.InsertOnSubmit(query);
                    db.SubmitChanges();
                    showData();
                    enableAndVisible(true);
                    Support.clearField(this);
                    Support.msi("Insert data success");
                }

                if (mode == "update")
                {
                    var query = db.rooms.FirstOrDefault(x => x.id == Convert.ToInt32(tbId.Text));

                    if (query != null)
                    {
                        query.code = tbCode.Text;
                        query.capacity = Convert.ToInt32(tbCapacity.Text);

                        db.SubmitChanges();
                        showData();
                        enableAndVisible(true);
                        Support.clearField(this);
                        Support.msi("Update data success");
                    }
                }
            }
            catch (Exception ex)
            {
                Support.mse(ex.Message);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            enableAndVisible(true);
            Support.clearField(this);
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
                var query = db.rooms.First(x => x.id == Convert.ToInt32(tbId.Text));

                query.deleted_at = DateTime.Now;
                db.SubmitChanges();
                showData();
                enableAndVisible(true);
                Support.clearField(this);
                Support.msi("Delete data success");
            }
        }

        private void tbSearch_TextChanged(object sender, EventArgs e)
        {
            showData();
        }
    }
}
