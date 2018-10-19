using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ReminderPro
{
    public partial class ReminderPlus : Form
    {
        private int reminderId;
        private string updateId;
        private string filepath = "../../db/reminder.json";
        public ReminderPlus()
        {
            InitializeComponent();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void ReminderPlus_Load(object sender, EventArgs e)
        {
            GetReminders();
            cmbTime.SelectedIndex = 1;
        }

        private void GetReminders()
        {
            var jArray = GetData();
            dgReminders.DataSource = jArray;
            var lastItem = jArray.LastOrDefault();
            if (lastItem != null)
            {
                reminderId = Convert.ToInt32(lastItem["Id"]);
            }

            reminderId = reminderId + 1;
            lblName.Text = String.Empty;
            lblTime.Text = String.Empty;
            lblDesc.Text = String.Empty;
            foreach (var remItem in jArray)
            {
                if (remItem["RemDate"].Value<string>() == DateTime.Today.ToShortDateString())
                {
                    lblName.Text = remItem["Name"].ToString();
                    lblTime.Text = remItem["RemTime"].ToString();
                    lblDesc.Text = remItem["Description"].ToString();
                }
            }
        }
        private void btnSubmit_Click(object sender, EventArgs e)
        {
            AddReminder();
            ClearData();
        }

        private void ClearData()
        {
            txtName.Text = string.Empty;
            txtDesc.Text = string.Empty;
            dtpReminderDate.Value = DateTime.Today;
            btnSubmit.Text = "Create";
        }
        private JArray GetData()
        {
            var json = File.ReadAllText(filepath);
            var jArray = JArray.Parse(json);
            return jArray;
        }

        private void AddReminder()
        {
            var newReminderData = "{ " + " 'Id': " + reminderId.ToString() + ", 'Name': '" + txtName.Text +
                                  "', Description: '" + txtDesc.Text + "', RemDate: '" + dtpReminderDate.Value.ToShortDateString() + "', RemTime: '" + cmbTime.SelectedItem + "' } ";
            var reminderArrary = GetData();
            var newReminder = JObject.Parse(newReminderData);
            foreach (var remItem in reminderArrary)
            {
                if (remItem["Id"].Value<string>() == updateId)
                {
                    int Id = remItem["Id"].Value<int>();
                    newReminder["Id"] = Id;
                    remItem["Name"] = newReminder["Name"];
                    remItem["Description"] = newReminder["Description"];
                    remItem["RemDate"] = newReminder["RemDate"];
                    remItem["RemTime"] = newReminder["RemTime"];
                }
            }
            if(btnSubmit.Text == "Create")
            reminderArrary.Add(newReminder);

            string newJsonResult =
                Newtonsoft.Json.JsonConvert.SerializeObject(reminderArrary, Newtonsoft.Json.Formatting.Indented);
            File.WriteAllText(filepath, newJsonResult);
            GetReminders();
        }

        private void cmbTime_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            dgReminders.ClearSelection();
            ClearData();
        }

        private void dgReminders_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            updateId = dgReminders.Rows[e.RowIndex].Cells[0].Value.ToString();
            txtName.Text = dgReminders.Rows[e.RowIndex].Cells[1].Value.ToString();
            txtDesc.Text = dgReminders.Rows[e.RowIndex].Cells[2].Value.ToString();
            dtpReminderDate.Value = Convert.ToDateTime(dgReminders.Rows[e.RowIndex].Cells[3].Value);
            cmbTime.SelectedItem = dgReminders.Rows[e.RowIndex].Cells[4].Value.ToString();
            btnSubmit.Text = "Update";

        }
    }
    
}
