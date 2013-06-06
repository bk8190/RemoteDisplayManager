using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RemoteDisplayManager
{
    public partial class Form1 : Form
    {
        private bool connected;
        private string lastStatus;
        private DisplaySerialManager dsm;
        private int cbidx;

        public Form1()
        {
            connected = false;
            lastStatus = "";
            dsm = new DisplaySerialManager();
            cbidx = 0;

            InitializeComponent();

            portBoxDropDown(null, null);
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
                    button2.Enabled       = true;
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
                button2.Enabled       = false;
                backlightBox.Enabled  = false;

                dsm.close();
            }
        }


        private void serialTimerCallback(object sender, EventArgs e)
        {
            if (connected)
            {
                dsm.update();
                if (dsm.responses.Count > 0)
                {
                    String s = (String)dsm.responses.Dequeue();

                    // We got a response from the device. Reset the timeout timer
                    connectedBox.Checked = true;
                    connectionTimeoutTimer.Stop();
                    connectionTimeoutTimer.Start();

                    switch (s[0])
                    {
                        case 'e':
                            Console.WriteLine("Got ping");
                            break;

                        case 'u':
                            String status = s.Substring(1);
                            Console.WriteLine("Got: set status \"" + status + "\"");
                            MXIEScraper.SetStatus(status);
                            break;

                        case 'h':
                            String voltage = s.Substring(1);
                            Console.WriteLine("Got heartbeat: " + s + ", voltage = " + voltage);
                            break;

                        default:
                            rawResponsesTextBox.Text += s + "\r\n";
                            break;
                    }
                }
            }
        }


        private void mxieTimerCallback(object sender, EventArgs e)
        {
            // Update the MXIE status
            if (mxieCheckBox.Checked)
            {
                try
                {
                    Image img = MXIEScraper.GetStatusImage();
                    mxiePicture.Image = img;
                    String newstatus = MXIEScraper.ExtractStatus(img);

                    StatusCurrentTextBox.Text = newstatus;
                    setStatus(newstatus);

                    MXIEScraper.SetStatus("Hello world");
                }
                catch (Exception ex)
                {
                    System.Console.WriteLine(ex.ToString());
                    StatusCurrentTextBox.Text = "ERROR: " + ex.Message;// ToString();
                }
            }
        }

        private void setStatus(String newstatus)
        {
            //if (connected)
            {
                if (newstatus != lastStatus)
                {
                    lastStatus = newstatus;

                    var text = newstatus.Replace('\n', ' ').Replace('\r', ' ');

                    var words = text.Split(' ');
                    var result = new List<string>();
                    result.Add("");
                    var idx = 0;

                    foreach (var word in words)
                    {
                        if (word == "")
                            continue;

                        Console.WriteLine("word: <" + word + ">");
                        if (result[idx].Length + word.Length + 1 <= 16)
                        {
                            if (result[idx].Length > 0)
                                result[idx] += " ";
                            result[idx] += word;
                        }
                        else
                        {
                            idx++;
                            result.Add(word);
                        }
                    }

                    if (result.Count > 0)
                    {
                        Console.WriteLine("Line 0: <" + result[0] + ">");
                        // dsm.write(Commands.QuickTextCommand(result[0]));

                        if (result.Count > 1)
                        {
                            Console.WriteLine("Line 1: <" + result[1] + ">");
                            // dsm.write(Commands.TextCommand(result[1], 1));
                        }
                    }
                    else
                    {
                        Console.WriteLine("Clearing");
                        dsm.write(Commands.ClearCommand());
                    }

                }
            }
        }

        private void copyStatus(object sender, EventArgs e)
        {
            setStatus(textBox1.Text);
        }

        private void sendRaw(object sender, EventArgs e)
        {
            dsm.write(textBox1.Text);
        }

        private void changeBacklight(object sender, EventArgs e)
        {
            Console.WriteLine("Set backlight: " + backlightBox.Text);
            var s = Commands.BacklightCommand(backlightBox.Text);
            dsm.write(s);
        }

        // When this timeout expires, we are no longer connected to the device
        private void connectedTimerCallback(object sender, EventArgs e)
        {
            connectedBox.Checked = false;
        }

        // Send a ping every 5 seconds
        private void pingTimerCallback(object sender, EventArgs e)
        {
            if (connected)
            {
                Console.WriteLine("Sending ping");
                dsm.write(Commands.PingCommand());
            }
        }

    }
}
