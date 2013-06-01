using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemoteDisplayManager
{
    public static class Commands
    {
        public static String BacklightCommand(String state)
        {
            return "<l" + state + ">";
        }

        public static String TextCommand(String text, int row=0, int col=0)
        {
            text = text.Replace('<', '%');
            text = text.Replace('>', '%');
            if (text.Length > 16)
                throw new Exception("Text too long");
            if (row<0 && row>1)
                throw new Exception("Valid rows: 0, 1");
            if (col < 0 || col > 16)
                throw new Exception("Valid cols are [0, 16]");

            char rowc = (char)(97 + row);
            char colc = (char)(97 + col);
            return "<t" + rowc + colc + text + ">";
        }

        public static String QuickTextCommand(String text)
        {
            text = text.Replace('<', '%');
            text = text.Replace('>', '%');
            if (text.Length > 16)
                throw new Exception("Text too long");

            return "<u" + text + ">";
        }

        public static String ClearCommand(int row=3)
        {
            if (row == 0)
                return "<ca>";
            if (row == 1)
                return "<cb>";
            return "<c>";
        }

        public static String PingCommand()
        {
            return "<e>";
        }
    }

    class DisplaySerialManager
    {
        private SerialPort serialPort;
        private bool got_start_delim;
        private string incomplete_string;

        public Queue responses;

        public DisplaySerialManager()
        {
            got_start_delim = false;
            incomplete_string = "";
            responses = new Queue();

            serialPort = new SerialPort();
            serialPort.BaudRate = 57600;
            serialPort.ReadTimeout = 500;
            serialPort.WriteTimeout = 500;
        }

        public void open(string portName)
        {
            serialPort.PortName = portName;
            serialPort.Open();
        }

        public void close()
        {
            serialPort.Close();
            responses.Clear();
        }

        public void write(string s)
        {
            Console.WriteLine("Sending: " + s);
            serialPort.WriteLine(s);
        }

        public void update()
        {
            string input = serialPort.ReadExisting();

            if (input.Length > 0)
            {
                //System.Console.WriteLine("Raw: %" + input + "%");
            }

            foreach (char c in input)
            {
                if (!got_start_delim)
                {
                    if (c == '<')
                    {
                        got_start_delim = true;
                        incomplete_string = "";
                    }
                }
                else
                {
                    if (c == '<')
                    {
                        System.Console.WriteLine("Repeat open delim");
                        got_start_delim = false;
                    }
                    else if (c == '>')
                    {
                        //System.Console.WriteLine("Command: %" + incomplete_string + "%");
                        responses.Enqueue(incomplete_string);
                        got_start_delim = false;
                    }
                    else
                    {
                        incomplete_string += c;
                    }
                }
            }
        }
    }
}