using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TTSWordAddin
{
    internal class AudioPlayer
    {
        private IWavePlayer waveOutDevice;
        private WaveStream audioFileReader;
        private byte[] audioData;
        private bool isPlaying;
        private bool isPaused;
        private ManualResetEvent playbackCompletedEvent = new ManualResetEvent(false);

        public AudioPlayer(byte[] audioData)
        {
            this.audioData = audioData;
            waveOutDevice = new WaveOutEvent();
            audioFileReader = new WaveFileReader(new MemoryStream(audioData));
            waveOutDevice.Init(audioFileReader);

            waveOutDevice.PlaybackStopped += (sender, args) =>
            {
                if (isPlaying)
                {
                    isPlaying = false;
                    playbackCompletedEvent.Set();
                }
            };
        }

        public void Play()
        {
            if (!isPlaying)
            {
                isPlaying = true;
                if (!isPaused)
                {
                    audioFileReader.Seek(0, SeekOrigin.Begin);
                }
                waveOutDevice.Play();
            }
            else if (isPaused)
            {
                isPaused = false;
                waveOutDevice.Play();
            }
        }

        public void Pause()
        {
            if (isPlaying)
            {
                isPaused = true;
                waveOutDevice.Pause();
            }
        }

        public void Stop()
        {
            if (isPlaying)
            {
                isPlaying = false;
                isPaused = false;
                waveOutDevice.Stop();
                audioFileReader.Seek(0, SeekOrigin.Begin);
            }
        }

        public void PlaySync()
        {
            Play();
            playbackCompletedEvent.WaitOne(); // Block until playback is complete
        }

        public void Dispose()
        {
            Stop();
            waveOutDevice.Dispose();
            audioFileReader.Dispose();
        }
    }
}
