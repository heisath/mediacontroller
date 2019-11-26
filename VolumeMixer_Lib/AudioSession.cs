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
        private IAudioSessionControl ctl;
        private IAudioSessionControl2 ctl2;
        private ISimpleAudioVolume volumectl;
        private ERole role;

        public delegate void DataChangedHandler(AudioSession sender);
        public event DataChangedHandler OnDataChanged;

        public delegate void DisposedHandler(AudioSession sender);
        public event DisposedHandler OnDisposed;

        private Guid guid;

        public AudioSession(IAudioSessionControl ctl, ERole role)
        {
            this.ctl = ctl;
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
                uint cpid;
                ctl2.GetProcessId(out cpid);
                return cpid;
            }
        }

        public string AppName
        {
            get
            {
                string appname;
                ctl.GetDisplayName(out appname);

                if (appname == "")
                {
                    var process = Process.GetProcessById((int)ProcessID);
                    appname = process.MainWindowTitle;

                    if (appname == "")
                    {
                        appname = process.MainModule.FileVersionInfo.FileDescription;
                    }
                }

                if (appname.Contains(@"@%SystemRoot%\System32\AudioSrv.Dll"))
                {
                    appname = "System";
                }

                return appname;
            }
        }
        public string AppNameShort { get => AppName.Substring(0, Math.Min(AppName.Length, 20)); }

        public enum SessionTypeEnum
        {
            CONSOLE = 0,
            MUTLIMEDIA = 1,
            COMMUNICATION = 2
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
                float level;
                volumectl.GetMasterVolume(out level);
                return level * 100;
            }
            set
            {
                volumectl.SetMasterVolume(value / 100, guid);
            }
        }

        public bool Mute
        {
            get
            {
                bool mute;
                volumectl.GetMute(out mute);
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

            UnregisterSessionNotification();
            if (ctl != null) Marshal.ReleaseComObject(ctl);
            if (ctl2 != null) Marshal.ReleaseComObject(ctl2);
            if (volumectl != null) Marshal.ReleaseComObject(volumectl);
        }

        private void RegisterSessionNotification()
        {
            ctl2.RegisterAudioSessionNotification(this);
        }
        private void UnregisterSessionNotification()
        {
            ctl2.UnregisterAudioSessionNotification(this);
        }

        #region ### IAudioSessionEvents implementation ###

        private float lastValue = -1;
        public int OnChannelVolumeChanged(uint channelCount, IntPtr newVolumes, uint channelIndex, ref Guid eventContext)
        {
            return 0;
        }

        public int OnDisplayNameChanged(string displayName, ref Guid eventContext)
        {
            OnDataChanged?.Invoke(this);
            Console.WriteLine("OnDisplayNameChanged invoke");
            return 0;
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
                    OnDataChanged?.Invoke(this);
                    Console.WriteLine("OnSimpleVolumeChanged invoke");
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
