﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.IO;
using Vannatech.CoreAudio.Enumerations;
using Vannatech.CoreAudio.Interfaces;

namespace VolumeMixer_Lib
{
    /// <summary>
    /// Controls audio using the Windows CoreAudio API
    /// from: http://stackoverflow.com/questions/14306048/controling-volume-mixer
    /// and: http://netcoreaudio.codeplex.com/
    /// </summary>
    public static class AudioManager
    {

        public static List<AudioSession> GetAudioSessions()
        {
            List<AudioSession> mmProcs = EnumerateAudioSessions(ERole.eMultimedia);
            List<AudioSession> cmProcs = EnumerateAudioSessions(ERole.eCommunications);

            return mmProcs.Concat(cmProcs).ToList();
        }
        public static List<AudioSession> EnumerateAudioSessions(ERole currentRole)
        {
            IMMDeviceEnumerator deviceEnumerator = null;
            IAudioSessionEnumerator sessionEnumerator = null;
            IAudioSessionManager2 mgr = null;
            IMMDevice speakers = null;
            try
            {
                // get the speakers (1st render + multimedia) device
                deviceEnumerator = (IMMDeviceEnumerator)(new MMDeviceEnumerator());
                deviceEnumerator.GetDefaultAudioEndpoint(EDataFlow.eRender, currentRole, out speakers);

                // activate the session manager. we need the enumerator
                Guid IID_IAudioSessionManager2 = typeof(IAudioSessionManager2).GUID;
                speakers.Activate(IID_IAudioSessionManager2, 0, IntPtr.Zero, out object o);
                mgr = (IAudioSessionManager2)o;

                // enumerate sessions for on this device
                mgr.GetSessionEnumerator(out sessionEnumerator);
                sessionEnumerator.GetCount(out int count);

                List<AudioSession> results = new List<AudioSession>();

                for (int i = 0; i < count; ++i)
                {
                    IAudioSessionControl ctl = null;
                    try
                    {
                        sessionEnumerator.GetSession(i, out ctl);
                        
                        results.Add(new AudioSession(ctl, currentRole));
                    }
                    catch(Exception)
                    {
                        if (ctl != null) Marshal.ReleaseComObject(ctl);
                    }
                }

                return results;
            }
            finally
            {
                if (sessionEnumerator != null) Marshal.ReleaseComObject(sessionEnumerator);
                if (mgr != null) Marshal.ReleaseComObject(mgr);
                if (speakers != null) Marshal.ReleaseComObject(speakers);
                if (deviceEnumerator != null) Marshal.ReleaseComObject(deviceEnumerator);
            }
        }

        #region Master Volume Manipulation

        /// <summary>
        /// Gets the current master volume in scalar values (percentage)
        /// </summary>
        /// <returns>-1 in case of an error, if successful the value will be between 0 and 100</returns>
        public static float GetMasterVolume(ERole role)
        {
            IAudioEndpointVolume masterVol = null;
            try
            {
                masterVol = GetMasterVolumeObject(role);
                if (masterVol == null)
                    return -1;

                masterVol.GetMasterVolumeLevelScalar(out float volumeLevel);
                return volumeLevel * 100;
            }
            finally
            {
                if (masterVol != null)
                    Marshal.ReleaseComObject(masterVol);
            }
        }

        /// <summary>
        /// Gets the mute state of the master volume. 
        /// While the volume can be muted the <see cref="GetMasterVolume"/> will still return the pre-muted volume value.
        /// </summary>
        /// <returns>false if not muted, true if volume is muted</returns>
        public static bool GetMasterVolumeMute(ERole role)
        {
            IAudioEndpointVolume masterVol = null;
            try
            {
                masterVol = GetMasterVolumeObject(role);
                if (masterVol == null)
                    return false;

                masterVol.GetMute(out bool isMuted);
                return isMuted;
            }
            finally
            {
                if (masterVol != null)
                    Marshal.ReleaseComObject(masterVol);
            }
        }

        /// <summary>
        /// Sets the master volume to a specific level
        /// </summary>
        /// <param name="newLevel">Value between 0 and 100 indicating the desired scalar value of the volume</param>
        public static void SetMasterVolume(ERole role, float newLevel)
        {
            IAudioEndpointVolume masterVol = null;
            try
            {
                masterVol = GetMasterVolumeObject(role);
                if (masterVol == null)
                    return;

                masterVol.SetMasterVolumeLevelScalar(newLevel / 100, Guid.Empty);
            }
            finally
            {
                if (masterVol != null)
                    Marshal.ReleaseComObject(masterVol);
            }
        }

        /// <summary>
        /// Increments or decrements the current volume level by the <see cref="stepAmount"/>.
        /// </summary>
        /// <param name="stepAmount">Value between -100 and 100 indicating the desired step amount. Use negative numbers to decrease
        /// the volume and positive numbers to increase it.</param>
        /// <returns>the new volume level assigned</returns>
        public static float StepMasterVolume(ERole role, float stepAmount)
        {
            IAudioEndpointVolume masterVol = null;
            try
            {
                masterVol = GetMasterVolumeObject(role);
                if (masterVol == null)
                    return -1;

                float stepAmountScaled = stepAmount / 100;

                // Get the level
                masterVol.GetMasterVolumeLevelScalar(out float volumeLevel);

                // Calculate the new level
                float newLevel = volumeLevel + stepAmountScaled;
                newLevel = Math.Min(1, newLevel);
                newLevel = Math.Max(0, newLevel);

                masterVol.SetMasterVolumeLevelScalar(newLevel, Guid.Empty);

                // Return the new volume level that was set
                return newLevel * 100;
            }
            finally
            {
                if (masterVol != null)
                    Marshal.ReleaseComObject(masterVol);
            }
        }

        /// <summary>
        /// Mute or unmute the master volume
        /// </summary>
        /// <param name="isMuted">true to mute the master volume, false to unmute</param>
        public static void SetMasterVolumeMute(ERole role, bool isMuted)
        {
            IAudioEndpointVolume masterVol = null;
            try
            {
                masterVol = GetMasterVolumeObject(role);
                if (masterVol == null)
                    return;

                masterVol.SetMute(isMuted, Guid.Empty);
            }
            finally
            {
                if (masterVol != null)
                    Marshal.ReleaseComObject(masterVol);
            }
        }

        /// <summary>
        /// Switches between the master volume mute states depending on the current state
        /// </summary>
        /// <returns>the current mute state, true if the volume was muted, false if unmuted</returns>
        public static bool ToggleMasterVolumeMute(ERole role)
        {
            IAudioEndpointVolume masterVol = null;
            try
            {
                masterVol = GetMasterVolumeObject(role);
                if (masterVol == null)
                    return false;

                masterVol.GetMute(out bool isMuted);
                masterVol.SetMute(!isMuted, Guid.Empty);

                return !isMuted;
            }
            finally
            {
                if (masterVol != null)
                    Marshal.ReleaseComObject(masterVol);
            }
        }

        private static IAudioEndpointVolume GetMasterVolumeObject(ERole role)
        {
            IMMDeviceEnumerator deviceEnumerator = null;
            IMMDevice speakers = null;
            try
            {
                deviceEnumerator = (IMMDeviceEnumerator)(new MMDeviceEnumerator());
                deviceEnumerator.GetDefaultAudioEndpoint(EDataFlow.eRender, role, out speakers);

                Guid IID_IAudioEndpointVolume = typeof(IAudioEndpointVolume).GUID;
                speakers.Activate(IID_IAudioEndpointVolume, 0, IntPtr.Zero, out object o);
                IAudioEndpointVolume masterVol = (IAudioEndpointVolume)o;

                return masterVol;
            }
            finally
            {
                if (speakers != null) Marshal.ReleaseComObject(speakers);
                if (deviceEnumerator != null) Marshal.ReleaseComObject(deviceEnumerator);
            }
        }

        #endregion
    }

    // #region Abstracted COM interfaces from Windows CoreAudio API
    [ComImport]
    [Guid("BCDE0395-E52F-467C-8E3D-C4579291692E")]
    internal class MMDeviceEnumerator
    {
    }
    
}

