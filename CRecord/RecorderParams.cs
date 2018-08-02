using Accord.Video.FFMPEG;
using System.Windows.Forms;

namespace CRecord
{
    public class RecorderParams
    {
        public RecorderParams(string filename, int FrameRate, VideoCodec codec, int bitrate)
        {
            FileName = filename;
            FramesPerSecond = FrameRate;
            Codec = codec;
            BitRate = bitrate;

            Height = Screen.PrimaryScreen.Bounds.Height;
            Width = Screen.PrimaryScreen.Bounds.Width;
            BitDepth = Screen.PrimaryScreen.BitsPerPixel;
        }

        public string FileName { get; private set; }
        public int FramesPerSecond { get; private set; }
        public int BitRate { get; private set; }
        public VideoCodec Codec { get; private set; }

        public int Height { get; private set; }
        public int Width { get; private set; }
        public int BitDepth { get; private set; }
    }
}
