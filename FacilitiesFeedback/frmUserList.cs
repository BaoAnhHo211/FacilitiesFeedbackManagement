using BusinessObject.Models;
using DataAccess.Repository;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace FacilitiesFeedback
{
    public partial class frmUserList : Form
    {
        IUserRepository userRepository = new UserRepository();
        public User currentUser { get; set; }
        List<User> userList = new List<User>();
        int CurrentRow;
        public frmUserList()
        {
            InitializeComponent();
        }

        private void frmUserList_Load(object sender, System.EventArgs e)
        {
            ShowData();
        }

        private void ShowData()
        {
            userList = (List<User>)userRepository.GetUsers();
            dgvUser.DataSource = null;
            dgvUser.DataSource = userList;
            dgvUser.Columns["FeedbackStaffs"].Visible = false;
            dgvUser.Columns["FeedbackUsers"].Visible = false;
        }

        private void dgvUser_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            frmUserDetail frmUserDetail = new frmUserDetail
            {
                user = (User)dgvUser.Rows[e.RowIndex].DataBoundItem,
                isAdmin = true,
            };
            if (frmUserDetail.ShowDialog() == DialogResult.OK)
            {
                ShowData();
            }
        }

        private void btnDelete_Click(object sender, System.EventArgs e)
        {
            try
            {
                var user = (User)dgvUser.Rows[CurrentRow].DataBoundItem;
                if (user.Id == currentUser.Id)
                {
                    throw new Exception("This is your account! Cannot Delete");
                }
                var Confirm = MessageBox.Show("Are you want to delete this User?", "Confirm Delete!!!", MessageBoxButtons.YesNo);
                if (Confirm == DialogResult.Yes)
                {
                    userRepository.DeleteUser(user);
                    MessageBox.Show("Delete Successful", "Result", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ShowData();
                }
            }
            catch (Exception ex)
            {
                new frmMessageBox(ex.Message).ShowDialog();
            }
        }

        private void dgvUser_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            CurrentRow = e.RowIndex;
        }
    }
}
