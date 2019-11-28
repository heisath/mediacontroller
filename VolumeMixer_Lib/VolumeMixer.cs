using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace VolumeMixer_Lib
{
    public class VolumeMixer
    {
        public string SerialPortName { get; set; }

        Thread serviceThread;
        SerialPort serialPort;
        readonly int[] sessionNo = new int[3];
        List<AudioSession> sessions;

        public delegate void OnOutputChangedHandler(int new_output);
        public event OnOutputChangedHandler OnOutputChanged;

        private bool SerialPortReady { get => serialPort != null && serialPort.IsOpen; }

        public void Start(string port)
        {
            SerialPortName = port;
            serviceThread = new Thread(Main);
            serviceThread.Start();
        }
        public void Stop()
        {
            serviceThread.Abort();
        }

        private void InitSessions()
        {
            for (int i = 0; i < 3; i++)
            {
                sessionNo[i] = 0;
                NextInSessions(i, 0);
            }
            SerialPort_WriteSession();
        }

        private void Main()
        {

            sessions = AudioManager.GetAudioSessions();

            sessions.ForEach((x) =>
            {
                x.OnDataChanged += SessionDataChanged;
            });

            int recon_counter = 0;

            while (true)
            {
                try
                {
                    if (serialPort == null)
                    {
                        serialPort = new SerialPort(SerialPortName)
                        {
                            NewLine = "\n",
                            BaudRate = 115200
                        };
                        serialPort.DataReceived += SerialPort_DataReceived;
                    }
                    else if (!serialPort.IsOpen)
                    {
                        serialPort.Open();
                        serialPort.DtrEnable = true;
                        serialPort.RtsEnable = true;

                        recon_counter = 0;

                        InitSessions();
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

                    // if session on slot 0 is no longer active, search for another active one.
                    if (!sessions[sessionNo[0]].IsActive)
                    {
                        var old = sessionNo[0];
                        NextInSessions(0, 1);
                        if (sessionNo[0] != old)
                            SerialPort_WriteSession(0);
                    }

                    Thread.Sleep(3000);
                }
                catch (ThreadAbortException)
                {
                    // clean up then exit
                    break;
                }
                catch (System.IO.IOException)
                {
                    if (recon_counter > 5) break;
                    recon_counter++;
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
                    if (line.StartsWith("PREV_PAGE"))
                    {
                        int index = Convert.ToInt32(line.Replace("PREV_PAGE", ""));
                        NextInSessions(index, -1);
                        SerialPort_WriteSession(index);
                    }
                    else if (line.StartsWith("NEXT_PAGE"))
                    {
                        int index = Convert.ToInt32(line.Replace("NEXT_PAGE", ""));
                        NextInSessions(index, 1);
                        SerialPort_WriteSession(index);
                    }
                    else if (line.StartsWith("VOL"))
                    {
                        string[] parts = line.Replace("VOL", "").Split(':');
                        int index = Convert.ToInt32(parts[0]);
                        float level = Convert.ToSingle(parts[1]);
                        sessions[sessionNo[index]].Volume = level;

                        SerialPort_WriteSession(index, true);
                    }
                    else if (line.StartsWith("RES_VOL"))
                    {
                        int index = Convert.ToInt32(line.Replace("RES_VOL", ""));

                        if (sessions[sessionNo[index]].Volume > 50)
                            sessions[sessionNo[index]].Volume = 0;
                        else if (sessions[sessionNo[index]].Volume < 50)
                            sessions[sessionNo[index]].Volume = 100;

                        SessionDataChanged(sessions[sessionNo[index]]);
                    }
                    else if (line.StartsWith("SEL_OUT"))
                    {
                        int out_index = Convert.ToInt32(line.Replace("SEL_OUT", ""));
                        OnOutputChanged?.Invoke(out_index);
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
        private void SerialPort_WriteSession(int index = -1, bool all_except = false)
        {
            if (!SerialPortReady) return;

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


        private void NextInSessions(int index, int dir)
        {
            if (dir == 0)
            {
                if (index == 1) return;

                NextInSessions(index, 1);
            }
            int loops = 0;
            while (true)
            {
                sessionNo[index] += dir;
                if (sessionNo[index] >= sessions.Count) sessionNo[index] = 0;
                if (sessionNo[index] < 0) sessionNo[index] = sessions.Count - 1;

                switch (index)
                {
                    case 0:
                        if (sessions[sessionNo[index]].IsActive) return;
                        break;
                    case 1:
                        return;
                    case 2:
                        if (sessions[sessionNo[index]].SessionType == AudioSession.SessionTypeEnum.COMMUNICATION) return;
                        break;
                }

                if (loops > sessions.Count)
                {
                    sessionNo[index] = 0;
                    return;
                }

                loops++;
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

    
    }
}
