﻿using System;
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
using System.Management;

namespace RemoteDisplayManager
{
    public partial class Form1 : Form
    {
        private bool connected;
        private string lastStatus;
        private DisplaySerialManager dsm;
        private List<String> rawHistory;

        public Form1()
        {
            rawHistory = new List<String>();

            connected = false;
            lastStatus = "";
            dsm = new DisplaySerialManager();

            InitializeComponent();
            portBoxDropDown(null, null);

            try{
                MXIEScraper.SetMXIETopmost();
            }catch (Exception e){
            }
        }

        private void portBoxDropDown(object sender, EventArgs e)
        {
            var portnames1 = SerialPort.GetPortNames();
            serialPortBox.DataSource = portnames1;
        }

        private void connectButtonClicked(object sender, EventArgs e)
        {
            if (!connected)
            {
                try 
                {
                    Console.WriteLine("Opening \"" + serialPortBox.Text + "\"");
                    dsm.open(serialPortBox.Text);
                    connected = true;
                    connectButton.Text = "Stop";

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
                backlightBox.Enabled = false;
                connectButton.Text = "Connect";

                dsm.close();
            }
        }

        int lineno = 0;
        private void serialTimerCallback(object sender, EventArgs e)
        {
            if (connected)
            {
                dsm.update();
                
                String raw = dsm.getRaw();
                if (raw.Length > 0)
                {
                    char[] delims = new char[] { '\r', '\n' };
                    foreach (var line in raw.Split(delims, StringSplitOptions.RemoveEmptyEntries))
                        rawHistory.Add("" + (lineno++) + ": " + line);

                    for (int i = 0; i < rawHistory.Count - 10; i++)
                        rawHistory.RemoveAt(0);

                    rawResponsesTextBox.Text = String.Join("\r\n", rawHistory);
                    rawResponsesTextBox.SelectionStart = rawResponsesTextBox.Text.Length;
                    rawResponsesTextBox.ScrollToCaret();
                }

                if (dsm.responses.Count > 0)
                {
                    String s = (String)dsm.responses.Dequeue();

                    // We got a response from the device. Reset the timeout timer
                    connectedBox.Checked = true;
                    connectionTimeoutTimer.Stop();
                    connectionTimeoutTimer.Start();
                    switch (s[0])
                    {
                        case 's':
                            String status = "_" + s.Substring(1);
                            Console.WriteLine("Set status: \"" + status + "\"");

                            mxieTimer.Stop();

                            MXIEScraper.SetStatus(status);
                            System.Threading.Thread.Sleep(500);

                            var tmp = getStatusFromMXIE();
                            tmp = getStatusFromMXIE();
                            Console.WriteLine("After OCR, new status is \"" + tmp + "\"");

                            if (mxieCheckBox.Checked)
                            {
                                mxieTimerCallback(null, null);
                                mxieTimer.Start();
                            }
                            break;

                        case 'h':
                            String voltage = s.Substring(1);
                            Console.WriteLine("Got heartbeat: " + s + ", voltage = " + voltage);
                            break;

                        default:
                            Console.WriteLine("Unknown: <" + s + ">");
                            rawResponsesTextBox.Text += s + "\r\n";
                            break;
                    }
                }
            }
        }

        private String getStatusFromMXIE()
        {
            Image img = MXIEScraper.GetStatusImage();
            mxiePicture.Image = img;
            return MXIEScraper.ExtractStatus(img);
        }


        private void mxieTimerCallback(object sender, EventArgs e)
        {
            if (mxieCheckBox.Checked)
            {
                try
                {
                    String s = getStatusFromMXIE();
                    if (s != lastStatus)
                    {
                        Console.WriteLine("MXIE: <" + s + ">");
                        setStatus(s);
                    }
                    //MXIEScraper.SetStatus("Hello world");
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
            if (connected)
            {
                if (newstatus != lastStatus)
                {
                    lastStatus = newstatus;
                    StatusCurrentTextBox.Text = newstatus;

                    var cmd = Commands.SetTextCompositeCommand(newstatus);
                    dsm.write(cmd);
                }
            }
        }


        private void changeBacklight(object sender, EventArgs e)
        {
            Console.WriteLine("Set backlight: " + backlightBox.Text);
            dsm.write(Commands.BacklightCommand(backlightBox.Text));
        }

        // When this timeout expires, we are no longer connected to the device
        private void connectedTimerCallback(object sender, EventArgs e)
        {
            connectedBox.Checked = false;
        }

        private void setStatusButtonClick(object sender, EventArgs e)
        {
            setStatus(textBox1.Text);
        }

        private void sendRawButtonClick(object sender, EventArgs e)
        {
            dsm.write(textBox1.Text);
        }

        private void pingTimerCallback(object sender, EventArgs e)
        {
            Console.WriteLine("Sending ping");
            dsm.write(Commands.PingCommand());
        }
    }
}
