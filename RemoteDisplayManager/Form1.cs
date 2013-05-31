using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RemoteDisplayManager
{
    public partial class Form1 : Form
    {
        private bool connected;
        private string lastStatus;
        private DisplaySerialManager dsm;

        public Form1()
        {
            connected = false;
            lastStatus = "";
            dsm = new DisplaySerialManager();

            InitializeComponent();

            System.Console.WriteLine("derp");
        }

        private void portBoxDropDown(object sender, EventArgs e)
        {
            var ports = SerialPort.GetPortNames();
            serialPortBox.DataSource = ports;
        }

        private void connectButtonClicked(object sender, EventArgs e)
        {
            if (!connected)
            {
                try
                {
                    dsm.open(serialPortBox.Text);
                    connected = true;
                    serialPortBox.Enabled = false;
                    button1.Enabled       = true;
                    backlightBox.Enabled  = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString(), "Error opening port");
                }
            }
            else
            {
                connected = false;
                serialPortBox.Enabled = true;
                button1.Enabled       = false;
                backlightBox.Enabled  = false;

                dsm.close();
            }
        }

        private void timerCallback(object sender, EventArgs e)
        {
            // Update the MXIE status
            try
            {
                string newstatus = MXIEScraper.GetStatus();
                StatusCurrentTextBox.Text = newstatus;
                if (newstatus != lastStatus)
                {
                    Console.WriteLine("New status: " + newstatus);
                    lastStatus = newstatus;
                }
            }
            catch(Exception ex)
            {
                StatusCurrentTextBox.Text = ex.ToString();
            }

            if (connected)
            {
                dsm.update();
                if (dsm.responses.Count > 0)
                {
                    String s = (String) dsm.responses.Dequeue();
                    MXIEScraper.SetStatus(s);
                    rawResponsesTextBox.Text += s + "\r\n";
                }
            }
        }

        private void copyStatus(object sender, EventArgs e)
        {
               dsm.write(textBox1.Text);
        }

        private void changeBacklight(object sender, EventArgs e)
        {
            Console.WriteLine("Set backlight: " + backlightBox.Text);
            var s = Commands.BacklightCommand(backlightBox.Text);
            dsm.write(s);
        }

    }
}
