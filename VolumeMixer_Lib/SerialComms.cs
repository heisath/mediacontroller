using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace VolumeMixer_Lib
{
    public class SerialComms
    {
        public string SerialPortName { get; set; }

        Thread serviceThread;
        SerialPort serialPort;
        int processNumber;
        List<AudioSession> sessions;

        public void Start()
        {
            SerialPortName = "COM6";
            serviceThread = new Thread(main);
            serviceThread.Start();
        }
        public void Stop()
        {
            serviceThread.Abort();
        }

        void main()
        {

            sessions = AudioSession.GetAudioSessions(AudioChanged);

            while (true)
            {
                try
                {
                    if (serialPort == null)
                    {
                        serialPort = new SerialPort(SerialPortName);
                        serialPort.NewLine = "\n";
                        serialPort.BaudRate = 115200;
                        serialPort.DataReceived += SerialPort_DataReceived;
                    }
                    else if (!serialPort.IsOpen)
                    {
                        serialPort.Open();
                        serialPort.DtrEnable = true;
                        serialPort.RtsEnable = true;

                        processNumber = 0;
                        SerialPort_WriteSession();
                        sessions[processNumber].Activate();
                    }

                    List<AudioSession> newSessions = AudioSession.GetAudioSessions(AudioChanged);
                    bool changed = false;

                    if (newSessions.Count != sessions.Count)
                        changed = true;
                    else
                    {
                        for (int i = 0; i < newSessions.Count; i++)
                        {
                            if (newSessions[i].ProcessID != sessions[i].ProcessID) { changed = true; break; }
                        }
                    }
                    if (changed)
                    {
                        // try to retain position
                        int oldProcId = sessions[processNumber].ProcessID;
                        AudioSession.SessionTypeEnum sessionType = sessions[processNumber].SessionType;
                        sessions[processNumber].DeActivate();

                        sessions = newSessions;

                        int newIndex = sessions.FindIndex((x) => x.ProcessID == oldProcId && x.SessionType == sessionType);
                        if (newIndex >= 0)
                        {
                            processNumber = newIndex;
                        }
                        else
                        {
                            processNumber = 0;
                        }

                        sessions[processNumber].Activate();
                        SerialPort_WriteSession();
                    }


                    Thread.Sleep(500);
                }
                catch (ThreadAbortException)
                {
                    // clean up then exit
                    break;
                }
                catch (System.IO.IOException)
                {
                    break;
                }

            }

            serialPort.Close();
            Console.WriteLine("Service died");
        }

        private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                while (serialPort.BytesToRead > 0)
                {
                    string line = serialPort.ReadLine().Trim();
                    if (line.Contains("PAGE"))
                    {
                        sessions[processNumber].DeActivate();
                        if (line == ": NEXT_PAGE")
                        {
                            processNumber++;
                        }
                        else if (line == ": PREV_PAGE")
                        {
                            processNumber--;
                        }
                        
                        if (processNumber >= sessions.Count) processNumber = 0;
                        if (processNumber < 0) processNumber = sessions.Count - 1;

                        sessions[processNumber].Activate();
                        SerialPort_WriteSession();
                    }
                    else if (line.StartsWith(": VOL"))
                    {
                        sessions[processNumber].Volume = Convert.ToSingle(line.Replace(": VOL", ""));
                    }
                    else if (line == ": RES_VOL")
                    {
                        if (sessions[processNumber].Volume > 50)
                            sessions[processNumber].Volume = 0;
                        else if (sessions[processNumber].Volume < 50)
                            sessions[processNumber].Volume = 100;
                        SerialPort_WriteSession();
                    }
                }
            }
            catch (Exception)
            {
                if (serialPort != null && serialPort.IsOpen)
                {
                    serialPort.DiscardInBuffer();
                }
            }
        }

        private int AudioChanged()
        {
            SerialPort_WriteSession();
            return 0;
        }

        private void SerialPort_WriteSession()
        {
            serialPort.WriteLine((int)sessions[processNumber].SessionType + ":" + (int)sessions[processNumber].Volume + ":" + sessions[processNumber].AppNameShort);

        }
    }
}
