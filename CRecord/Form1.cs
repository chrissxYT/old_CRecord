using Accord.Video.FFMPEG;
using System;
using System.IO;
using System.Threading;
using System.Timers;
using System.Windows.Forms;

namespace CRecord
{
    public partial class Form1 : Form
    {
        bool framerateOk = true;
        bool qualityOk = true;
        bool recording = false;
        Recorder recorder = null;
        volatile bool running = true;
        Thread gc = null;
        volatile Config config = null;
        volatile ConfigManager configManager = new ConfigManager(Path.Combine(Environment.CurrentDirectory, "..\\config\\.cfg"));

        public Form1()
        {
            InitializeComponent();
            if (!Directory.Exists(Path.Combine(Environment.CurrentDirectory, "..\\config")))
                Directory.CreateDirectory(Path.Combine(Environment.CurrentDirectory, "..\\config"));
        }

        void textBox1_TextChanged(object sender, EventArgs e)
        {
            framerateOk = int.TryParse(textBox1.Text, out int i);
            if (framerateOk)
                trackBar1.Value = i;
        }

        void button1_Click(object sender, EventArgs e)
        {
            if (recording)
            {
                recorder.Dispose();
                recorder = null;
                button1.Text = "Start";
            }
            else
            {
                if (!framerateOk || int.Parse(textBox1.Text) > 120)
                {
                    MessageBox.Show("Please enter the framerate correctly!");
                    return;
                }
                if (!qualityOk || int.Parse(textBox3.Text) > 100000)
                {
                    MessageBox.Show("Please enter the quality correctly!");
                    return;
                }
                
                recorder = new Recorder(new RecorderParams(textBox2.Text, int.Parse(textBox1.Text), GetCodec(), int.Parse(textBox3.Text)));
                recorder.Start();

                button1.Text = "Stop";
            }
            recording = !recording;
        }

        void textBox3_TextChanged(object sender, EventArgs e)
        {
            qualityOk = int.TryParse(textBox3.Text, out int i);
            if (qualityOk)
                trackBar2.Value = i;
        }

        void Form1_Load(object sender, EventArgs e)
        {
            config = configManager.ParsedConfig;
            trackBar3.Value = config.GC_RATE;
            trackBar4.Value = config.PREVIEW_FPS;
            trackBar2.Value = config.QUALITY;
            trackBar1.Value = config.OUTPUT_FPS;
            textBox2.Text = config.OUTPUT_FILE;

            gc = new Thread(() =>
            {
                while (running)
                {
                    Thread.Sleep(config.GC_RATE);
                    GC.Collect();
                }
            });
            gc.Start();

            var screenTimer = new System.Timers.Timer();
            screenTimer.Interval = 1000 / config.PREVIEW_FPS;
            screenTimer.Elapsed += new ElapsedEventHandler((object s, ElapsedEventArgs args) =>
            {
                screenTimer.Interval = 1000 / config.PREVIEW_FPS;
                pictureBox1.Image = Util.Screenshot();
            });
            screenTimer.Start();
        }

        void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            running = false;
            if (recording)
            {
                recorder.Dispose();
                recorder = null;
            }
            gc.Join();
            configManager.SaveConfig(config);
            Environment.Exit(0);
        }

        void trackBar1_Scroll(object sender, EventArgs e)
        {
            textBox1.Text = trackBar1.Value.ToString();
            config.OUTPUT_FPS = trackBar1.Value;
        }

        void trackBar2_Scroll(object sender, EventArgs e)
        {
            textBox3.Text = trackBar2.Value.ToString();
            config.QUALITY = trackBar2.Value;
        }

        void trackBar3_Scroll(object sender, EventArgs e)
        {
            textBox4.Text = trackBar3.Value+"ms";
            config.GC_RATE = trackBar3.Value;
        }

        void button2_Click(object sender, EventArgs e)
        {
            var sfd = new SaveFileDialog();
            sfd.Filter = "Video Files|*.mp4;*.mov;*.avi";
            if (sfd.ShowDialog().Equals(DialogResult.OK))
                textBox2.Text = sfd.FileName;
        }

        void trackBar4_Scroll(object sender, EventArgs e)
        {
            textBox5.Text = trackBar4.Value.ToString();
            config.PREVIEW_FPS = trackBar4.Value;
        }

        void textBox2_TextChanged(object sender, EventArgs e)
        {
            config.OUTPUT_FILE = textBox2.Text;
        }

        VideoCodec GetCodec()
        {
            switch (listBox1.SelectedIndex)
            {
                case 0: return VideoCodec.H265;
                case 1: return VideoCodec.H264;
                case 2: return VideoCodec.H263P;
                case 3: return VideoCodec.MPEG2;
                case 4: return VideoCodec.MPEG4;
                case 5: return VideoCodec.MSMPEG4v2;
                case 6: return VideoCodec.MSMPEG4v3;
                case 7: return VideoCodec.VP8;
                case 8: return VideoCodec.VP9;
                case 9: return VideoCodec.FFV1;
                case 10: return VideoCodec.FFVHUFF;
                case 11: return VideoCodec.FLV1;
                case 12: return VideoCodec.Raw;
                case 13: return VideoCodec.Theora;
                case 14: return VideoCodec.WMV1;
                case 15: return VideoCodec.WMV2;
                default: MessageBox.Show("Cannot recognize your codec, please send a bug report containing the codec you selected to chrissx!"); return VideoCodec.Default;
            }
        }

        //GC RATE BUTTON
        void button4_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This value determines how often your computer will clean unused RAM by this program, lowering this value will lower RAM use but slightly increase CPU-usage.", "I help you, bro ^^");
        }

        //CODEC BUTTON
        void button8_Click(object sender, EventArgs e)
        {
            MessageBox.Show("The codec the video should be encoded in, we recommend H264 or H265.", "I help you, bro ^^");
        }

        //OUTPUT FILE BUTTON
        void button7_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Where do you want to save your recording?", "I help you, bro ^^");
        }

        //FPS BUTTON
        void button5_Click(object sender, EventArgs e)
        {
            MessageBox.Show("The frames per second the output video has.", "I help you, bro ^^");
        }

        //BITRATE BUTTON
        void button6_Click(object sender, EventArgs e)
        {
            MessageBox.Show("The bitrate of the output video(in kbit/s).", "I help you, bro ^^");
        }

        //PREVIEW FPS BUTTON
        void button3_Click(object sender, EventArgs e)
        {
            MessageBox.Show("The frames per second of the preview, setting this higher will increase your CPU and RAM usage by a lot, so set it to max 15 if you don't have a NASA-PC.", "I help you, bro ^^");
        }
    }
}
