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
    public partial class frmTickets : Form
    {
        public frmTickets()
        {
            InitializeComponent();

            timer1.Start();     // displays the current time in the title bar
        }

        // setting the default values
        private int minutesPerWindow = 5;
        private int guestsPerWindow = 5;
        private DateTime startTime;
        private DateTime endTime;
        private int initialTickNum = 1;

        // 
        private int timeSlotIndex = 0;
        private int currentTimeSlotIndex = -1;

        // creating a list for the time slots
        private List<TimeSlot> timeWindows;
        // creating a queue to add time slots and remove expired time slots
        private Queue<Ticket> ticketQueue;

        private void frmTickets_Load(object sender, EventArgs e)
        {
            // displays the Options dialog box when the application first loads
            SetOptions();
        }

        // a timer control that gets the current time
        private void timer1_Tick(object sender, EventArgs e)
        {
            DateTime time = DateTime.Now;
            this.Text = time.ToLongTimeString();
        }

        // exits the application
        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

　
        private void btnOptions_Click(object sender, EventArgs e)
        {
            // warns the user that the time slots will be erased if they select the options button
            string msg = "This action will delete all outstanding tickets. " +
                "Are you sure you want to continue?";
            if (MessageBox.Show(msg, "Warning",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning)
                    == DialogResult.Yes)
                SetOptions();
        }

        // method to set the data entered into the Options dialog box
        private void SetOptions()
        {
            startTime = DateTime.Now;
            endTime = startTime.AddHours(4);    
            frmOptions optionsForm = new frmOptions();
            // sets the minutes per window to 5
            optionsForm.MinutesPerWindow = this.minutesPerWindow;
            // sets the guests per window to 5
            optionsForm.GuestsPerWindow = this.guestsPerWindow;
            // set the start time the current time
            optionsForm.StartTime = this.startTime;
            // set the end time
            optionsForm.EndTime = this.endTime;
            optionsForm.InitialTickNum = this.initialTickNum;
            timer2.Enabled = false;     
            if (optionsForm.ShowDialog() == DialogResult.OK)
            {
                timer2.Enabled = true;
                this.minutesPerWindow = optionsForm.MinutesPerWindow;
                this.guestsPerWindow = optionsForm.GuestsPerWindow;
                this.startTime = optionsForm.StartTime;
                this.endTime = optionsForm.EndTime;
                this.initialTickNum = optionsForm.InitialTickNum;
                this.CreateTimeWindows();
                this.startTime = timeWindows[0].StartTime;
                this.currentTimeSlotIndex = -1;
                ticketQueue = new Queue<Ticket>();
                UpdateDisplay();
                lstTicketBox.Items.Clear();
                btnIssueTicket.Enabled = true;
            }
        }

　
        private void CreateTimeWindows()
        {
            timeWindows = new List<TimeSlot>();
            DateTime windowStart = this.startTime.AddMinutes(minutesPerWindow);
            while (windowStart < this.endTime.AddMinutes(minutesPerWindow))
            {
                timeWindows.Add(new TimeSlot(windowStart,
                    this.minutesPerWindow));
                windowStart = windowStart.AddMinutes(this.minutesPerWindow);
            }
        }

        // method to issue a ticket
        private void btnIssueTicket_Click(object sender, EventArgs e)
        {
            Ticket t = new Ticket();    // creates an object of the ticket class
            t.TimeWindow = timeSlotIndex; 
            TimeSlot tw = timeWindows[timeSlotIndex];
            tw.GuestsAssigned += 1;     // increments the guest that are assigned 

            ticketQueue.Enqueue(t);     // adds a new time slot to the ticket box
            if (tw.GuestsAssigned == guestsPerWindow)
            {
                if (timeSlotIndex + 1 == timeWindows.Count)
                {
                    btnIssueTicket.Enabled = false;
                    lblNextEntry.Text = "Sold out.";
                }
                else
                    timeSlotIndex++;
            }
            string msg;
            UpdateDisplay();
            msg = "Ticket " + t.Number + ": " +
                tw.StartTime.ToShortTimeString();
            lstTicketBox.Items.Add(msg);    // adds each ticket slot to the ticket box
        }

        // method that updates the number of outstanding tickets and the next available time slot
        private void UpdateDisplay()
        {
            lblOutstandingTicket.Text = ticketQueue.Count.ToString();
            if (btnIssueTicket.Enabled)
            {
                if (timeSlotIndex < timeWindows.Count)
                {
                    TimeSlot tw = timeWindows[timeSlotIndex];
                    lblNextEntry.Text = tw.StartTime.ToShortTimeString();
                }
            }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            // indicates whether the current time slot is open or closed
            this.Text = DateTime.Now.ToLongTimeString();
            if (DateTime.Now > startTime && 
                DateTime.Now < endTime.AddMinutes(minutesPerWindow))
            {
                this.Text += " (Open)";
            }
            else
            {
                this.Text += " (Closed)";
                lblTicketNumbers.Text = "";
            }

　
            if (DateTime.Now.Second == 0)
            {
                bool newTimeSlotStarted = false;
                if (currentTimeSlotIndex < timeWindows.Count - 1 &&                
                    DateTime.Now.ToShortTimeString() == 
                    (timeWindows[currentTimeSlotIndex + 1])
                    .StartTime.ToShortTimeString())
                    {
                        newTimeSlotStarted = true;
                        currentTimeSlotIndex++;
                    }
                

                if (newTimeSlotStarted && currentTimeSlotIndex == 
                    timeWindows.Count - 1)
                {
                    btnIssueTicket.Enabled = false;
                    lblNextEntry.Text = "Closed";
                }

                if (newTimeSlotStarted)
                {
                    int initialTicket = 0;
                    bool initialTicketFound = false;
                    int lastTicket = 0;

                    while (ticketQueue.Count >= 1)
                    {
                        Ticket t = ticketQueue.Peek();
                        TimeSlot ts = timeWindows[t.TimeWindow];
                        if (ts.StartTime < DateTime.Now)
                        {
                            ticketQueue.Dequeue();
                            if (!initialTicketFound)
                            {
                                initialTicket = t.Number;
                                initialTicketFound = true;
                            }
                            lastTicket = t.Number;
                            lstTicketBox.Items.RemoveAt(0);
                        }
                        else
                            break;
                    }

                    if (!initialTicketFound)
                        lblTicketNumbers.Text = "";
                    else if (initialTicket == lastTicket)
                        lblTicketNumbers.Text = initialTicket.ToString();
                    else
                        lblTicketNumbers.Text = initialTicket.ToString() +
                                " - " + lastTicket.ToString();

                    if (lblNextEntry.Text == DateTime.Now.ToShortTimeString())
                        timeSlotIndex++;

                    UpdateDisplay();
                }
            }
        }
    }
}
