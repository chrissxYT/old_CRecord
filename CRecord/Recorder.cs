using Accord.Video.FFMPEG;
using System;
using System.Globalization;
using System.Threading;
using DirectShowLib;

namespace CRecord
{
    public class Recorder : IDisposable
    {
        VideoFileWriter writer;
        RecorderParams recorderParams;
        Thread recorderThread;
        volatile bool recording = false;

        public Recorder(RecorderParams recorderParams)
        {
            this.recorderParams = recorderParams;

            writer = new VideoFileWriter();

            recorderThread = new Thread(RecordThreadExec)
            {
                Name = "CRecord - NewRecorder",
                IsBackground = false,
                Priority = ThreadPriority.Highest,
                CurrentCulture = CultureInfo.CurrentCulture,
                CurrentUICulture = CultureInfo.CurrentUICulture
            };
        }

        public void Start()
        {
            writer.Open(recorderParams.FileName, recorderParams.Width, recorderParams.Height, recorderParams.FramesPerSecond, recorderParams.Codec, recorderParams.BitRate);
            recorderThread.Start();
        }

        public void Dispose()
        {
            recording = false;

            recorderThread.Join();

            writer.Dispose();

            writer = null;
            recorderParams = null;
            recorderThread = null;
        }

        void RecordThreadExec()
        {
            int frameInterval = 1000 / recorderParams.FramesPerSecond;
            long lastFrameTime = DateTime.Now.ToBinary();
            while (recording)
            {
                writer.WriteVideoFrame(Util.Screenshot());
                while (DateTime.Now.ToBinary() - lastFrameTime < frameInterval)
                    ;
                lastFrameTime = DateTime.Now.ToBinary();
            }
            writer.Flush();
            writer.Close();
        }
    }

    public class TestRecorder
    {
        public void Test()
        {
            
        }
    }
}
