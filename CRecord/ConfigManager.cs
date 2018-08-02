using System.IO;
using System.IO.Compression;

namespace CRecord
{
    public class ConfigManager
    {
        string config_file = null;

        public ConfigManager(string config_file)
        {
            this.config_file = config_file;
        }

        public Config ParsedConfig
        {
            get
            {
                if (File.Exists(config_file))
                    return ParseConfig(config_file, Util.Temp_Path);
                else
                    return new Config();
            }
        }

        Config ParseConfig(string file, string temp)
        {
            var c = new Config();
            ZipFile.ExtractToDirectory(file, temp);
            var gr = temp + "GC_RATE";
            var pf = temp + "PREVIEW_FPS";
            var ofp = temp + "OUTPUT_FPS";
            var q = temp + "QUALITY";
            var ofi = temp + "OUTPUT_FILE";
            if (File.Exists(gr))
                c.GC_RATE = File.ReadAllBytes(gr).ToInt32();
            if (File.Exists(pf))
                c.PREVIEW_FPS = File.ReadAllBytes(pf).ToInt32();
            if (File.Exists(ofp))
                c.OUTPUT_FPS = File.ReadAllBytes(ofp).ToInt32();
            if (File.Exists(q))
                c.QUALITY = File.ReadAllBytes(q).ToInt32();
            if (File.Exists(ofi))
                c.OUTPUT_FILE = File.ReadAllText(ofi);
            foreach (string s in Directory.GetFiles(temp))
                File.Delete(s);
            foreach (string s in Directory.GetDirectories(temp))
                Directory.Delete(s);
            Directory.Delete(temp);
            return c;
        }

        public void SaveConfig(Config c)
        {
            string temp = Util.Temp_Path;
            var gr = temp + "GC_RATE";
            var pf = temp + "PREVIEW_FPS";
            var ofp = temp + "OUTPUT_FPS";
            var q = temp + "QUALITY";
            var ofi = temp + "OUTPUT_FILE";
            File.WriteAllBytes(gr, c.GC_RATE.ToByteArray());
            File.WriteAllBytes(pf, c.PREVIEW_FPS.ToByteArray());
            File.WriteAllBytes(ofp, c.OUTPUT_FPS.ToByteArray());
            File.WriteAllBytes(q, c.QUALITY.ToByteArray());
            File.WriteAllText(ofi, c.OUTPUT_FILE);
            if (File.Exists(config_file))
                File.Delete(config_file);
            ZipFile.CreateFromDirectory(temp, config_file);
            foreach (string s in Directory.GetFiles(temp))
                File.Delete(s);
            foreach (string s in Directory.GetDirectories(temp))
                Directory.Delete(s);
            Directory.Delete(temp);
        }
    }
}
