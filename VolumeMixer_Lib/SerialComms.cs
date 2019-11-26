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
        int[] sessionNo = new int[3];
        List<AudioSession> sessions;

        public void Start(string port)
        {
            SerialPortName = port;
            serviceThread = new Thread(main);
            serviceThread.Start();
        }
        public void Stop()
        {
            serviceThread.Abort();
        }

        private void initSessions()
        {
            for (int i = 0; i < 3; i++)
            {
                sessionNo[i] = 0;
                NextInSessions(i, 0);
            }
            SerialPort_WriteSession();
        }

        private void main()
        {

            sessions = AudioManager.GetAudioSessions();
            
            sessions.ForEach((x) =>
            {
                x.OnDataChanged += SessionDataChanged;
            });

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

                        initSessions();
                    }

                    List<AudioSession> newSessions = AudioManager.GetAudioSessions();
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
                        for (int i = 0; i < 3; i++)
                        {
                            // try to retain position
                            uint oldProcId = sessions[sessionNo[i]].ProcessID;
                            AudioSession.SessionTypeEnum sessionType = sessions[sessionNo[i]].SessionType;

                            int newIndex = newSessions.FindIndex((x) => x.ProcessID == oldProcId && x.SessionType == sessionType);
                            if (newIndex >= 0)
                            {
                                sessionNo[i] = newIndex;
                            }
                            else
                            {
                                sessionNo[i] = 0;
                            }
                        }

                        sessions.ForEach((x) => x.Dispose());
                        sessions = newSessions;
                        sessions.ForEach((x) => x.OnDataChanged += SessionDataChanged);

                        SerialPort_WriteSession();
                    }
                    else
                    {
                        newSessions.ForEach((x) => x.Dispose());
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
            serialPort.Dispose();
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
                        string[] parts = line.Split(':');
                        int index = Convert.ToInt32(parts[2]);
                        
                        if (parts[1] == "NEXT_PAGE")
                        {
                            NextInSessions(index, 1);
                        }
                        else if (parts[1] == "PREV_PAGE")
                        {
                            NextInSessions(index, -1);
                        }

                        SerialPort_WriteSession(index);
                    }
                    else if (line.StartsWith(":VOL"))
                    {
                        string[] parts = line.Replace(":VOL", "").Split(':');
                        int index = Convert.ToInt32(parts[0]);
                        float level = Convert.ToSingle(parts[1]);
                        sessions[sessionNo[index]].Volume = level;

                        SerialPort_WriteSession(index,true );
                    }
                    else if (line.StartsWith(":RES_VOL"))
                    {
                        int index = Convert.ToInt32(line.Replace(":RES_VOL", ""));

                        if (sessions[sessionNo[index]].Volume > 50)
                            sessions[sessionNo[index]].Volume = 0;
                        else if (sessions[sessionNo[index]].Volume < 50)
                            sessions[sessionNo[index]].Volume = 100;

                        SessionDataChanged(sessions[sessionNo[index]]);
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

        private void NextInSessions(int index, int dir)
        {
            if (dir == 0)
            {
                if (index == 2)
                {
                    while (sessions[sessionNo[index]].SessionType != AudioSession.SessionTypeEnum.COMMUNICATION)
                    {
                        sessionNo[index] += 1;
                        if (sessionNo[index] >= sessions.Count) sessionNo[index] = 0;
                        if (sessionNo[index] < 0) sessionNo[index] = sessions.Count - 1;
                    }
                }
                else
                {
                    return;
                }
            }
            while (true)
            {
                sessionNo[index] += dir;
                if (sessionNo[index] >= sessions.Count) sessionNo[index] = 0;
                if (sessionNo[index] < 0) sessionNo[index] = sessions.Count - 1;

                if (index == 2)
                {
                    if (sessions[sessionNo[index]].SessionType == AudioSession.SessionTypeEnum.COMMUNICATION) break;
                }
                else
                {
                    break;
                }
            }

        }

        private void SessionDataChanged(AudioSession sender)
        {
            for (int i = 0; i < 3; i++)
            {
                if (sessions[sessionNo[i]] == sender)
                    SerialPort_WriteSession(i);
            }
        }

        private void SerialPort_WriteSession(int index = -1, bool all_except = false)
        {
            if (index < 0 || all_except)
            {
                for (int i = 0; i < 3; i++)
                {

                    int t = sessionNo[i];

                    if (index == -1 || (t == sessionNo[index] && i != index))
                        serialPort.WriteLine(i + ":" + (int)sessions[t].SessionType + ":" + (int)sessions[t].Volume + ":" + sessions[t].AppNameShort);
                }
            }
            else
            {
                int t = sessionNo[index];
                serialPort.WriteLine(index + ":" + (int)sessions[t].SessionType + ":" + (int)sessions[t].Volume + ":" + sessions[t].AppNameShort);
            }

        }
    }
}
