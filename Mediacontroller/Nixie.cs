using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;

namespace Mediacontroller
{
    class Nixie
    {
        public SerialPort serialPort;

        public Nixie(string portName)
        {
            serialPort = new SerialPort(portName, 9600);
            try
            {
                if (serialPort != null)
                {
                    if (!serialPort.IsOpen) serialPort.Open();
                }
            }
            catch (Exception) { }

        }

        public void send(byte status, byte nixie1, byte nixie2, byte nixie3, byte nixie4, byte misc)
        {
            if (!serialPort.IsOpen) return;
            byte[] outp = new byte[4];
            
            outp[0] = status;
            outp[1] = (byte)( (nixie1 * 10) + nixie2);
            outp[2] = (byte)( (nixie3 * 10) + nixie4);
            outp[3] = misc;

            serialPort.Write(outp, 0, 4);
        }

        public void RunAsClock()
        {
            if (!serialPort.IsOpen) return;
            byte[] outp = new byte[4];

            outp[0] = 1;
            outp[1] = (byte)DateTime.Now.Hour;
            outp[2] = (byte)DateTime.Now.Minute;
            outp[3] = (byte)DateTime.Now.Second;

            serialPort.Write(outp, 0, 4);
        }

    }
}
