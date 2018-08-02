namespace CRecord
{
    public class Config
    {
        public int GC_RATE { get; set; } = 500;
        public int PREVIEW_FPS { get; set; } = 10;
        public int OUTPUT_FPS { get; set; } = 30;
        public int QUALITY { get; set; } = 100;
        public string OUTPUT_FILE { get; set; } = "D:\\recording.mp4";

        public Config()
        {

        }

        public Config(int gcRate, int preFps, int outFps, int quality, string outFile)
        {
            GC_RATE = gcRate;
            PREVIEW_FPS = preFps;
            OUTPUT_FPS = outFps;
            QUALITY = quality;
            OUTPUT_FILE = outFile;
        }
    }
}
