using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vannatech.CoreAudio.Enumerations;
using Vannatech.CoreAudio.Interfaces;

namespace VolumeMixer_Lib
{
    public class AudioSession
    {
        public enum SessionTypeEnum
        {
            CONSOLE = 0,
            MUTLIMEDIA = 1,
            COMMUNICATION = 2
        }

        public int ProcessID { get; set; }
        public string AppName { get; set; }
        public SessionTypeEnum SessionType { get; set; }
        

        public string AppNameShort { get => AppName.Substring(0, Math.Min(AppName.Length, 20)); }

        public float Volume
        {
            get
            {
                if (SessionType == SessionTypeEnum.MUTLIMEDIA) AudioManager.SetRoleMultimedia();
                if (SessionType == SessionTypeEnum.COMMUNICATION) AudioManager.SetRoleCommunication();
                return AudioManager.GetApplicationVolume(ProcessID) ?? 0;
            }
            set
            {
                audioSessionEvent.IsActive = false;
                if (SessionType == SessionTypeEnum.MUTLIMEDIA) AudioManager.SetRoleMultimedia();
                if (SessionType == SessionTypeEnum.COMMUNICATION) AudioManager.SetRoleCommunication();
                AudioManager.SetApplicationVolume(ProcessID, value);
                audioSessionEvent.IsActive = true;
            }
        }

        public void Activate()
        {
            if (isActive) return;

            if (SessionType == SessionTypeEnum.MUTLIMEDIA) AudioManager.SetRoleMultimedia();
            if (SessionType == SessionTypeEnum.COMMUNICATION) AudioManager.SetRoleCommunication();
            __session = AudioManager.RegisterSessionNotification(ProcessID, audioSessionEvent);

            isActive = true;
        }
        public void DeActivate()
        {
            if (!isActive) return;

            if (SessionType == SessionTypeEnum.MUTLIMEDIA) AudioManager.SetRoleMultimedia();
            if (SessionType == SessionTypeEnum.COMMUNICATION) AudioManager.SetRoleCommunication();
            AudioManager.UnregisterSessionNotification(__session, audioSessionEvent);

            isActive = false;
        }


        public static List<AudioSession> GetAudioSessions(Func<int> audioChanged)
        {
            AudioManager.SetRoleMultimedia();
            List<KeyValuePair<int, string>> mmProcs = AudioManager.EnumerateAudioSessions();
            AudioManager.SetRoleCommunication();
            List<KeyValuePair<int, string>> cmProcs = AudioManager.EnumerateAudioSessions();

            List<AudioSession> sessions = new List<AudioSession>();

            foreach (var item in mmProcs)
            {
                sessions.Add(new AudioSession()
                {
                    AppName = item.Value,
                    ProcessID = item.Key,
                    SessionType = SessionTypeEnum.MUTLIMEDIA,
                    audioSessionEvent = new AudioSessionEvents()
                    {
                        OnVolumeChanged = audioChanged
                    }
                });

            }
            foreach (var item in cmProcs)
            {
                sessions.Add(new AudioSession()
                {
                    AppName = item.Value,
                    ProcessID = item.Key,
                    SessionType = SessionTypeEnum.COMMUNICATION,
                    audioSessionEvent = new AudioSessionEvents()
                    {
                        OnVolumeChanged = audioChanged
                    }
                });
            }

            return sessions;
        }

        private AudioSessionEvents audioSessionEvent;
        private IAudioSessionControl2 __session;
        private bool isActive;
    }

    internal class AudioSessionEvents : IAudioSessionEvents
    {
        public Func<int> OnVolumeChanged { get; set; }
        public bool IsActive { get; set; } = true;

        public int OnChannelVolumeChanged(uint channelCount, IntPtr newVolumes, uint channelIndex, ref Guid eventContext)
        {
            return 0;
        }

        public int OnDisplayNameChanged(string displayName, ref Guid eventContext)
        {
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
            return 0;
        }

        public int OnSimpleVolumeChanged(float volume, bool isMuted, ref Guid eventContext)
        {
            if (!IsActive) return 0;

            Console.WriteLine("Volume change detected" + volume);
            OnVolumeChanged?.Invoke();
            return 0;
        }

        public int OnStateChanged(AudioSessionState state)
        {
            return 0;
        }
    }
}
