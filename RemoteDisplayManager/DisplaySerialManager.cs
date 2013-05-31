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

        public static String TextLine1Command(String text)
        {
            return "<t" + text + ">";
        }

        public static String TextLine2Command(String text)
        {
            return "<u" + text + ">";
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