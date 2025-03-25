using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Esemka_Examination_V2
{
    public partial class FormSchedule : Form
    {
        DataBaseDataContext db = new DataBaseDataContext();
        int schedule_id = -1;
        public FormSchedule()
        {
            InitializeComponent();
        }

        void enableFieldSchedule(bool e)
        {
            tbExaminerName.Enabled = !e;
            tbCaseCode.Enabled = !e;
            tbRoomCode.Enabled = !e;
            dtpTime.Enabled = !e;
        }

        void enableButtonSchedule(bool v)
        {
            btnInsert.Visible = btnUpdate.Visible = btnDelete.Visible = v;

            btnSave.Visible = btnCancel.Visible = !v;
            btnDeleteSelected.Enabled = !v;
            btnDeleteAll.Enabled = !v;

        }

        void enableFieldAndButtonSchedule(bool e)
        {
            enableButtonSchedule(e);
            enableFieldSchedule(e);
        }

        void enableFieldParticipant(bool e)
        {
            tbParticipantName.Enabled = !e;
        }

        void enableButtonParticipant(bool e)
        {
            btnAdd.Enabled = !e;
            btnDeleteSelected.Enabled = !e;
            btnDeleteAll.Enabled = !e;
        }

        void enableFieldAndButtonParticipant(bool e)
        {
            enableButtonParticipant(e);
            enableFieldParticipant(e);
        }

        void showDataSchedule()
        {
            dgvDataSchedule.Columns.Clear();

            var query = db.schedules.Where(x => x.deleted_at == null)
                .Select(x => new
                {
                    x.id,
                    examiner = x.user.name,
                    room = x.room.code,
                    cases = x.@case.code,
                    x.time,
                    x.created_at,
                    x.deleted_at,
                    examinerId = x.examiner_id,
                    roomId = x.room.id,
                    caseid = x.case_id,

                });

            dgvDataSchedule.DataSource = query;

            dgvDataSchedule.Columns["examinerId"].Visible = false;
            dgvDataSchedule.Columns["roomId"].Visible = false;
            dgvDataSchedule.Columns["caseId"].Visible = false;
        }

        void showDataParticipant()
        {
            dgvDataParticipant.Columns.Clear();

            var query = db.schedules_participants.Where(x => x.schedule_id == schedule_id && x.deleted_at == null)
                .Select(x => new
                {
                    x.schedule_id,
                    x.participant_id,
                    name = x.user.name
                });

            dgvDataParticipant.DataSource = query;
        }

        private void FormSchedule_Load(object sender, EventArgs e)
        {
            Support.enableField(this);
            enableFieldAndButtonSchedule(true);
            enableFieldAndButtonParticipant(true);
            showDataSchedule();
            tbExaminerId.Enabled = tbRoomId.Enabled = tbCaseId.Enabled = tbParticipantId.Enabled = false;


        }

        private void dgvDataSchedule_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                schedule_id = (int)dgvDataSchedule.Rows[e.RowIndex].Cells["id"].Value;
                tbExaminerId.Text = dgvDataSchedule.Rows[e.RowIndex].Cells["examinerId"].Value.ToString();
                tbRoomId.Text = dgvDataSchedule.Rows[e.RowIndex].Cells["roomId"].Value.ToString();
                tbCaseId.Text = dgvDataSchedule.Rows[e.RowIndex].Cells["caseid"].Value.ToString();

                tbExaminerName.Text = dgvDataSchedule.Rows[e.RowIndex].Cells["examiner"].Value.ToString();
                tbRoomCode.Text = dgvDataSchedule.Rows[e.RowIndex].Cells["room"].Value.ToString();
                tbCaseCode.Text = dgvDataSchedule.Rows[e.RowIndex].Cells["cases"].Value.ToString();
                dtpTime.Value = (DateTime)dgvDataSchedule.Rows[e.RowIndex].Cells["time"].Value;

                showDataParticipant();
                enableFieldAndButtonParticipant(false);
            }
        }

        private void btnInsert_Click(object sender, EventArgs e)
        {
            enableFieldAndButtonSchedule(false);
            enableFieldAndButtonParticipant(true);
            Support.clearField(this);
            mode = "insert";
        }

        string mode;

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (tbExaminerId.Text == "")
            {
                Support.msw("Click row!!");
                return;
            }

            mode = "update";
            enableFieldAndButtonSchedule(false);
            enableFieldAndButtonParticipant(true);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (tbExaminerId.Text == "")
            {
                Support.msw("Click row!!");
                return;
            }

            enableFieldAndButtonParticipant(true);

            var query = db.schedules.FirstOrDefault(x => x.id == schedule_id);
            var dialog = MessageBox.Show("Are you sure want to delete this data?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            
            if (dialog == DialogResult.Yes)
            {
                query.deleted_at = DateTime.Now;
                db.SubmitChanges();
                Support.msi("delete data success");
                showDataSchedule();
                showDataParticipant();
                enableFieldAndButtonSchedule(true);
                enableFieldAndButtonParticipant(true);
                Support.clearField(this);
            }
        }

        private void tbExaminerName_TextChanged(object sender, EventArgs e)
        {
            var query = db.users.FirstOrDefault(x => x.name == tbExaminerName.Text);

            if (query != null)
            {
                tbExaminerId.Text = query.id.ToString();
            }

        }

        private void tbRoomCode_TextChanged(object sender, EventArgs e)
        {
            var query = db.rooms.FirstOrDefault(x => x.code == tbRoomCode.Text);

            if (query != null)
            {
                tbRoomId.Text = query.id.ToString();
            }
        }

        private void tbCaseCode_TextChanged(object sender, EventArgs e)
        {
            var query = db.cases.FirstOrDefault(x => x.code == tbCaseCode.Text);

            if (query != null)
            {
                tbCaseId.Text = query.id.ToString();
            }
        }

        private void tbParticipantName_TextChanged(object sender, EventArgs e)
        {
            var query = db.users.FirstOrDefault(x => x.name == tbParticipantName.Text);

            if (query != null)
            {
                tbParticipantId.Text = query.id.ToString();
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (tbParticipantName.Text == "")
            {
                Support.msw("Field participant must be filled");
                return;
            }

            var queryCheck = db.users.FirstOrDefault(x => x.name == tbParticipantName.Text);

            if (queryCheck != null)
            {
                if (queryCheck.role_id != 3)
                {
                    Support.msw("Data in field is not participant");
                    return;
                }

                var query = new schedules_participant();
                query.schedule_id = schedule_id;
                query.participant_id = Convert.ToInt32(tbParticipantId.Text);
                query.created_at = DateTime.Now;

                db.schedules_participants.InsertOnSubmit(query);
                db.SubmitChanges();
                Support.msi("add data success");
                showDataSchedule();
                showDataParticipant();
                enableFieldAndButtonParticipant(true);
                Support.clearField(this);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (tbExaminerName.Text == "" || tbRoomCode.Text == "" || tbCaseCode.Text == "")
            {
                Support.msw("All field must be filled");
                return;
            }

            if (dtpTime.Value < DateTime.Now)
            {
                Support.msw("time Must be greater than current time");
                return;
            }

            var queryCheckExaminer = db.users.FirstOrDefault(x => x.name == tbExaminerName.Text);

            if (queryCheckExaminer != null)
            {
                if (queryCheckExaminer.role_id != 2)
                {
                    Support.msw("Field examiner is not examiner");
                    return;
                } 
            } else
            {
                Support.msw("Field examiner must be exixt in table user");
                return;
            }

            var queryCheckExaminerSchedule = db.schedules.FirstOrDefault(x => x.examiner_id == Convert.ToInt32(tbExaminerId.Text) && x.time == dtpTime.Value);

            if (queryCheckExaminerSchedule != null && mode != "update")
            {
                Support.msw("Examiner must be has no other schedule that intersect with this schedule");
                return;
            }

            var queryCheckRoom = db.rooms.FirstOrDefault(x => x.id == Convert.ToInt32(tbRoomId.Text));
            
            if (queryCheckRoom == null)
            {
                Support.msw("Field room must be exixt in table room");
                return;
            }

            var queryCheckRoomSchedule = db.schedules.FirstOrDefault(x => x.room_id == Convert.ToInt32(tbRoomId.Text) && x.time == dtpTime.Value);

            if (queryCheckRoomSchedule != null && mode != "update")
            {
                Support.msw("Room must be has no other schedule that intersect with this schedule");
                return;
            }

            var queryCheckCapacity = db.schedules.Where(x => x.room_id == Convert.ToInt32(tbRoomId.Text)).Count();

            var queryRoomCapacity = db.rooms.FirstOrDefault(x => x.id == Convert.ToInt32(tbRoomId.Text));

            if (queryRoomCapacity != null)
            {
                if (queryCheckCapacity > queryRoomCapacity.capacity)
                {
                    Support.msw("Room capacity can accommodate number of participants");
                    return;
                }
            }

            try
            {
                if (mode == "insert")
                {
                    var query = new schedule();
                    query.examiner_id = Convert.ToInt32( tbExaminerId.Text);
                    query.type_id = 1;
                    query.room_id = Convert.ToInt32(tbRoomId.Text);
                    query.case_id = Convert.ToInt32(tbCaseId.Text);
                    query.time = dtpTime.Value;
                    query.created_at = DateTime.Now;

                    db.schedules.InsertOnSubmit(query);
                    db.SubmitChanges();
                    Support.msi("insert data success");
                    showDataSchedule();
                    showDataParticipant();
                    enableFieldAndButtonSchedule(true);
                    enableFieldAndButtonParticipant(true);
                    Support.clearField(this);

                }

                if (mode == "update")
                {
                    var query = db.schedules.FirstOrDefault(x => x.id == schedule_id);
                    query.examiner_id = Convert.ToInt32(tbExaminerId.Text);
                    query.type_id = 1;
                    query.room_id = Convert.ToInt32(tbRoomId.Text);
                    query.case_id = Convert.ToInt32(tbCaseId.Text);
                    query.time = dtpTime.Value;
                    query.created_at = DateTime.Now;

                    db.SubmitChanges();
                    Support.msi("update data success");
                    showDataSchedule();
                    showDataParticipant();
                    enableFieldAndButtonSchedule(true);
                    enableFieldAndButtonParticipant(true);
                    Support.clearField(this);
                }

            } catch (Exception ex)
            {
                Support.mse(ex.Message);
            }

        }

        private void dgvDataParticipant_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                tbParticipantName.Text = dgvDataParticipant.Rows[e.RowIndex].Cells["name"].Value.ToString();
            }
        }

        private void btnDeleteSelected_Click(object sender, EventArgs e)
        {
            if (tbParticipantName.Text == "")
            {
                Support.msw("click row!!");
                return;
            }

            var dialog = MessageBox.Show("Are you sure want to delete this data?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            var query = db.schedules_participants.FirstOrDefault(x => x.participant_id == Convert.ToInt32(tbParticipantId.Text) && x.schedule_id == schedule_id);
            if (dialog == DialogResult.Yes)
            {
                query.deleted_at = DateTime.Now;
                db.SubmitChanges();
                Support.msi("delete data success");
                showDataSchedule();
                showDataParticipant();
                enableFieldAndButtonParticipant(true);
                enableButtonSchedule(true);
                Support.clearField(this);
            }
        }

        private void btnDeleteAll_Click(object sender, EventArgs e)
        {
            if (tbExaminerId.Text == "")
            {
                Support.msw("Select schedule first!!");
                return;
            }

            var query = db.schedules_participants.Where(x => x.schedule_id == schedule_id);

            var dialog = MessageBox.Show("Are you sure want to delete this data?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            
            if (dialog == DialogResult.Yes)
            {
                foreach (var i in query)
                {
                    i.deleted_at = DateTime.Now;
                    db.SubmitChanges();
                }

                db.SubmitChanges();
                Support.msi("delete all data success");
                showDataSchedule();
                showDataParticipant();
                enableFieldAndButtonParticipant(true);
                enableButtonSchedule(true);
                Support.clearField(this);
            }

            
        }
    }
}
