using Esemka_Examination_V2.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.DirectoryServices.Protocols;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Esemka_Examination_V2
{
    public partial class FormUser : Form
    {
        DataBaseDataContext db = new DataBaseDataContext();
        public FormUser()
        {
            InitializeComponent();
        }

        void enable(bool e)
        {
            cboRole.Enabled = tbUsername.Enabled = tbPassword.Enabled = tbName.Enabled = tbEmail.Enabled = tbPhone.Enabled = cboGender.Enabled = tbAddress.Enabled = !e;
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

        void showDataCbo()
        {
            var list = new List<string>();
            list.Add("All");
            list.Add("Administrator");
            list.Add("Examiner");
            list.Add("Participant");
            cboFilter.DataSource = list;

            cboRole.DataSource = db.roles.ToList();
            cboRole.ValueMember = "id";
            cboRole.DisplayMember = "name";

            var listGender = new List<string>();
            listGender.Add("Male");
            listGender.Add("Female");

            cboGender.DataSource = listGender;

        }

        void showData()
        {
            dgvData.Columns.Clear();

            var query = db.users.Where(x => x.deleted_at == null).ToList();

            if (cboFilter.Text != "All")
            {
                query = query.Where(x => x.role.name == cboFilter.Text && x.name.StartsWith(tbSearch.Text)).ToList();
            }
            else
            {
                query = query.Where(x => x.name.StartsWith(tbSearch.Text)).ToList();
            }

            dgvData.DataSource = query.Select(x => new
            {
                x.id,
                role = x.role.name,
                x.username,
                password = Support.MD5( x.password),
                x.name,
                x.email,
                x.phone,
                x.gender,
                x.address,
                x.created_at,
                x.deleted_at
            }).ToList();

            //var btnEdit = new DataGridViewButtonColumn();
            //btnEdit.Text = "Edit";
            //btnEdit.HeaderText = "";
            //btnEdit.UseColumnTextForButtonValue = true;
            //btnEdit.Name = "btnEdit";
            //btnEdit.DefaultCellStyle.BackColor = Color.FromArgb(251, 168, 52);
            //btnEdit.DefaultCellStyle.ForeColor = Color.White;
            //btnEdit.FlatStyle = FlatStyle.Flat;
            //btnEdit.DefaultCellStyle.Font = new Font("Popins Semibold", 8, FontStyle.Bold);

            //var btnDelete = new DataGridViewButtonColumn();
            //btnDelete.Text = "Delete";
            //btnDelete.HeaderText = "";
            //btnDelete.UseColumnTextForButtonValue = true;
            //btnDelete.Name = "btnDelete";
            //btnDelete.DefaultCellStyle.color = Color.Red;
            //btnDelete.DefaultCellStyle.ForeColor = Color.White;
            //btnDelete.FlatStyle = FlatStyle.Flat;
            //btnDelete.DefaultCellStyle.Font = new Font("Popins Semibold", 8, FontStyle.Bold);

            ////dgvData.Columns.Add(btnEdit);
            //dgvData.Columns.Add(btnDelete);
        }

        private void FormUser_Load(object sender, EventArgs e)
        {
            Support.enableField(this);
            showData();
            showDataCbo();

            enableAndVisible(true);
            tbId.Enabled = false;
        }

        private void cboFilter_SelectedValueChanged(object sender, EventArgs e)
        {
            showData();
        }

        private void tbSearch_TextChanged(object sender, EventArgs e)
        {
            showData();
        }

        int selectedId;
        private void dgvData_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                selectedId = (int)dgvData.Rows[e.RowIndex].Cells["id"].Value;
                tbId.Text = dgvData.Rows[e.RowIndex].Cells["id"].Value.ToString();
                cboRole.Text = dgvData.Rows[e.RowIndex].Cells["role"].Value.ToString();
                tbUsername.Text = dgvData.Rows[e.RowIndex].Cells["username"].Value.ToString();
                tbPassword.Text = dgvData.Rows[e.RowIndex].Cells["password"].Value.ToString();
                tbName.Text = dgvData.Rows[e.RowIndex].Cells["name"].Value.ToString();
                tbEmail.Text = dgvData.Rows[e.RowIndex].Cells["email"].Value.ToString();
                tbPhone.Text = dgvData.Rows[e.RowIndex].Cells["phone"].Value.ToString();
                cboGender.Text = dgvData.Rows[e.RowIndex].Cells["gender"].Value.ToString();
                tbAddress.Text = dgvData.Rows[e.RowIndex].Cells["address"].Value.ToString();
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
            var queryCheck = db.users.FirstOrDefault(x => x.username == tbUsername.Text);

            if (queryCheck != null && mode == "insert")
            {
                Support.msw("Field username must be uniqe among database");
                return;
            }

            if (tbUsername.Text.Length < 4)
            {
                Support.msw("Field username must be more than 3 characters");
                return;
            }

            if (tbPassword.Text.Length < 5 || tbPassword.Text.Length > 12)
            {
                Support.msw("Field password  must be betwen 5 and 12 characters");
                return;
            }

            if (tbName.Text == "")
            {
                Support.msw("Field name must be filled");
                return;
            }

            if (tbEmail.Text == "")
            {
                Support.msw("Field email must be filled");
                return;
            }

            if (tbPhone.Text == "" || !tbPhone.Text.All(x => char.IsDigit(x)))
            {
                Support.msw("Field phone must be filled and must be numeric");
                return;
            }

            if (tbAddress.Text == "")
            {
                Support.msw("Field address must be filled");
                return;

            }

            try
            {
                if (mode == "insert")
                {
                    var query = new user();
                    query.role_id = Convert.ToInt32(cboRole.SelectedValue.ToString());
                    query.username = tbUsername.Text;
                    query.password = tbPassword.Text;
                    query.phone = tbPhone.Text;
                    query.address = tbAddress.Text;
                    query.email = tbEmail.Text;
                    query.name = tbName.Text;
                    query.gender = cboGender.Text;
                    query.created_at = DateTime.Now;

                    db.users.InsertOnSubmit(query);
                    db.SubmitChanges();
                    showData();
                    enableAndVisible(true);
                    Support.clearField(this);
                    Support.msi("Insert data success");
                }

                if (mode == "update")
                {
                    var query = db.users.FirstOrDefault(x => x.id == Convert.ToInt32(tbId.Text));

                    if (query != null)
                    {
                        query.role_id = (int)cboRole.SelectedValue;
                        query.username = tbUsername.Text;
                        query.password = tbPassword.Text;
                        query.phone = tbPhone.Text;
                        query.address = tbAddress.Text;
                        query.email = tbEmail.Text;
                        query.name = tbName.Text;
                        query.gender = cboGender.Text;

                        db.SubmitChanges();
                        showData();
                        enableAndVisible(true);
                        Support.clearField(this);
                        Support.msi("Update data success");
                    }
                }

            } catch(Exception ex)
            {
                Support.mse(ex.Message);
            }
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
                var query = db.users.First(x => x.id == Convert.ToInt32(tbId.Text));

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
