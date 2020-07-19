using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Vannatech.CoreAudio.Enumerations;
using Vannatech.CoreAudio.Interfaces;

namespace VolumeMixer_Lib
{
    public class AudioSession : IDisposable, IAudioSessionEvents
    {
        readonly double A = -100.0 / 3;
        readonly double B = 100.0 / 3;
        readonly double C = Math.Log(16.0);


        private readonly IAudioSessionControl2 ctl2;
        private readonly ISimpleAudioVolume volumectl;
        private readonly ERole role;

        public delegate void DataChangedHandler(AudioSession sender);
        public event DataChangedHandler OnDataChanged;

        public delegate void DisposedHandler(AudioSession sender);
        public event DisposedHandler OnDisposed;

        private Guid guid;

        public AudioSession(IAudioSessionControl ctl, ERole role)
        {
            this.ctl2 = ctl as IAudioSessionControl2;
            this.volumectl = ctl as ISimpleAudioVolume;
            this.role = role;
            this.guid = Guid.NewGuid();
            this.RegisterSessionNotification();
        }

        public uint ProcessID
        {
            get
            {
                ctl2.GetProcessId(out uint cpid);
                return cpid;
            }
        }

        public string AppName
        {
            get
            {
                ctl2.GetDisplayName(out string appname);

                if (appname == null || appname == "")
                {
                    var process = Process.GetProcessById((int)ProcessID);
                    appname = process.MainWindowTitle;

                    if (appname == "")
                    {
                        appname = process.MainModule.FileVersionInfo.FileDescription;
                    }
                }

                if (appname != null && appname.Contains(@"@%SystemRoot%\System32\AudioSrv.Dll"))
                {
                    appname = "System";
                }

                return appname ?? "";
            }
        }
        public string AppNameShort { get => AppName.Substring(0, Math.Min(AppName.Length, 20)); }

        public enum SessionTypeEnum
        {
            CONSOLE = 0,
            MUTLIMEDIA = 1,
            COMMUNICATION = 2
        }

        public bool IsActive
        {
            get
            {
                //otherwise compare file description of executable
                try
                {

                    GetWindowThreadProcessId(GetForegroundWindow(), out int fwnd_pid);
                    if (fwnd_pid == ProcessID) return true; // Procid fits perfect -> return true

                    if (ProcessID == 0) return false;
                    var processOwn = Process.GetProcessById((int)ProcessID).MainModule.FileVersionInfo.FileDescription;
                    var processForeground = Process.GetProcessById(fwnd_pid).MainModule.FileVersionInfo.FileDescription;
                    return processOwn == processForeground;
                }
                catch (Exception)
                {
                    return false;
                }

            }
        }

        public SessionTypeEnum SessionType
        {
            get
            {
                switch (role)
                {
                    case ERole.eMultimedia:
                        return SessionTypeEnum.MUTLIMEDIA;
                    case ERole.eCommunications:
                        return SessionTypeEnum.COMMUNICATION;
                    default:
                        return SessionTypeEnum.CONSOLE;
                }
            }
        }

        public float Volume
        {
            get
            {
                volumectl.GetMasterVolume(out float level);

                double SliderValue = Math.Log((level * 500.0 - A) / B) / C;
                return (float)SliderValue * 100.0F;
            }
            set
            {
                double DisplayValue = (A + B * Math.Exp(C * value / 100.0F)) / 500.0;
                volumectl.SetMasterVolume((float)DisplayValue, guid);
            }
        }

        public bool Mute
        {
            get
            {
                volumectl.GetMute(out bool mute);
                return mute;
            }
            set
            {
                volumectl.SetMute(value, guid);
            }
        }

        public void Dispose()
        {
            OnDisposed?.Invoke(this);
            try
            {
                UnregisterSessionNotification();
                if (ctl2 != null) Marshal.ReleaseComObject(ctl2);
                if (volumectl != null) Marshal.ReleaseComObject(volumectl);
            }
            catch (Exception) { }
        }

        private void RegisterSessionNotification()
        {
            ctl2.RegisterAudioSessionNotification(this);
        }
        private void UnregisterSessionNotification()
        {
            ctl2.UnregisterAudioSessionNotification(this);
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int GetWindowThreadProcessId(IntPtr handle, out int processId);

        #region ### IAudioSessionEvents implementation ###

        private float lastValue = -1;
        public int OnChannelVolumeChanged(uint channelCount, IntPtr newVolumes, uint channelIndex, ref Guid eventContext)
        {
            return 0;
        }

        public int OnDisplayNameChanged(string displayName, ref Guid eventContext)
        {
             try
             {
                OnDataChanged?.Invoke(this);
                Console.WriteLine("OnDisplayNameChanged invoke");
                return 0;
             } catch(Exception) { }
        }

        public int OnGroupingParamChanged(ref Guid groupingId, ref Guid eventContext)
        {
            return 0;
        }

        public int OnIconPathChanged(string iconPath, ref Guid eventContext)
        {
            return 0;
        }

        public int OnSessionDisconnected(AudioSessionDisconnectReason disconnectReason)
        {
            // Console.WriteLine("Session Disconnect invoke");
            //  this.Dispose();
            return 0;
        }

        public int OnSimpleVolumeChanged(float volume, bool isMuted, ref Guid eventContext)
        {
            if (volume != lastValue)
            {
                if (eventContext != guid)
                {
                    try
                    {
                        OnDataChanged?.Invoke(this);
                        Console.WriteLine("OnSimpleVolumeChanged invoke");
                    } catch(Exception) { }
                }
                lastValue = volume;
            }
            return 0;
        }

        public int OnStateChanged(AudioSessionState state)
        {
            return 0;
        }
        #endregion
    }


}
