using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using NAudio.Wave;
using NAudio.Dsp;

namespace MandalaGenerator
{
    class AudioAnalyzer
    {
        WasapiLoopbackCapture capture;

        public float Bass = 0;
        public float avgBass = 0;

        NAudio.Dsp.Complex[] fftBuffer;
        int fftPos = 0;

        const int FFT_SIZE = 1024;

        public event Action? OnKick;

        public void Start()
        {
            fftBuffer = new NAudio.Dsp.Complex[FFT_SIZE];

            capture = new WasapiLoopbackCapture();

            capture.DataAvailable += Capture_DataAvailable;

            capture.StartRecording();
        }

        private void Capture_DataAvailable(
            object? sender,
            WaveInEventArgs e)
        {
            for (int i = 0; i < e.BytesRecorded; i += 4)
            {
                float sample =
                    BitConverter.ToSingle(
                        e.Buffer,
                        i);

                fftBuffer[fftPos].X = sample;
                fftBuffer[fftPos].Y = 0;

                fftPos++;

                if (fftPos >= FFT_SIZE)
                {
                    fftPos = 0;

                    AnalyzeFFT();
                }
            }
        }

        void AnalyzeFFT()
        {
            FastFourierTransform.FFT(
                true,
                (int)Math.Log2(FFT_SIZE),
                fftBuffer);

            float bass = 0;

            for (int i = 2; i < 15; i++)
            {
                bass +=
                    (float)Math.Sqrt(
                        fftBuffer[i].X *
                        fftBuffer[i].X +

                        fftBuffer[i].Y *
                        fftBuffer[i].Y);
            }

            Bass = bass;

            avgBass = avgBass * 0.9f + Bass * 0.1f;

            if (Bass > avgBass * 1.5f)
            {
                OnKick?.Invoke();
            }
        }
    }
}
