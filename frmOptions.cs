using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AssigningTickets
{
    public partial class frmOptions : Form
    {
        public frmOptions()
        {
            InitializeComponent();
        }

        public int MinutesPerWindow;
        public int GuestsPerWindow;
        public DateTime StartTime;
        public DateTime EndTime;
        public int InitialTickNum;

        private void frmOptions_Load(object sender, EventArgs e)
        {
            txtMinPerWin.Text = this.MinutesPerWindow.ToString();
            txtGuests.Text = this.GuestsPerWindow.ToString();
            txtStart.Text = this.StartTime.ToShortTimeString(); 
            txtEnd.Text = this.EndTime.ToShortTimeString();
            txtFirstTicketNum.Text = this.InitialTickNum.ToString();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            // the if-statement that runs each text box through validation
            if (IsDataValid())
            {
                this.MinutesPerWindow = Convert.ToInt16(txtMinPerWin.Text);
                this.GuestsPerWindow = Convert.ToInt16(txtGuests.Text);
                this.StartTime = DateTime.Parse(txtStart.Text);
                this.EndTime = DateTime.Parse(txtEnd.Text);
                Ticket.SetNextTicketNumber(Convert.ToInt16
                    (txtFirstTicketNum.Text));
                this.DialogResult = DialogResult.OK;
            }
        }

        // the method to validate the data entered into the Options dialog box
        private bool IsDataValid()
        {
            if (!IsPresent(txtMinPerWin, "Minutes per window"))
                return false;
            if (!IsInt32(txtMinPerWin, "Minutes per window"))
                return false;
            if (!IsPresent(txtGuests, "Guests per window"))
                return false;
            if (!IsInt32(txtGuests, "Guest per window"))
                return false;
            if (!IsPresent(txtStart, "Start time"))
                return false;
            if (!IsDateTime(txtStart, "Start time"))
                return false;
            if (!IsPresent(txtEnd, "End time"))
                return false;
            if (!IsDateTime(txtEnd, "End time"))
                return false;
            if (!IsPresent(txtFirstTicketNum, "First ticket number"))
                return false;
            if (!IsInt32(txtFirstTicketNum, "First ticket number"))
                return false;

            DateTime startTime = Convert.ToDateTime(txtStart.Text);
            DateTime endTime = Convert.ToDateTime(txtEnd.Text);
            int minutesPerWindow = Convert.ToInt32(txtMinPerWin.Text);

            // validates whether the end time entered is after the start time
            if (startTime.AddMinutes(minutesPerWindow) > endTime)
            {
                MessageBox.Show("End time must be at least " +
                    minutesPerWindow +
                        " minutes after start time.", "Entry Error");
                return false;
            }

            return true;
        }

        // the method that checks whether data is entered into the text box
        private bool IsPresent(TextBox textBox, string name)
        {
            if (textBox.Text == "")
            {
                MessageBox.Show(name + " is a required field.", "Entry Error");
                textBox.Focus();
                return false;
            }
            return true;
        }

        // the method that checks whether a valid number is entered into the text box 
        private bool IsInt32(TextBox textBox, string name)
        {
            int number = 0;
            if (Int32.TryParse(textBox.Text, out number))
            {
                return true;
            }
            else
            {
                MessageBox.Show(name + " must be a valid integer.", "Entry Error");
                textBox.Focus();
                return false;
            }
        }

        // the method that checks whether a valid time is entered into the text box
        private bool IsDateTime(TextBox textBox, string name)
        {
            DateTime dateTime = new DateTime();
            if (DateTime.TryParse(textBox.Text, out dateTime))
            {
                return true;
            }
            else
            {
                MessageBox.Show(name + " must be a valid date.", "Entry Error");
                textBox.Focus();
                return false;
            }
        }

　
    }
}
