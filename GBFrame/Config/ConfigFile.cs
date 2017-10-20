using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GBFrame
{
    public class ConfigFile
    {
        private Dictionary<string, string> mKeyValuePair;

        private string mFile;
        private ConfigFile(string file) {
            mFile = file;
            DecodeConfigFile();
        }

        private void DecodeConfigFile() {
            FileStream file = File.OpenRead(mFile);
            BufferedStream buffered = new BufferedStream(file);
        }

        public static ConfigFile Open(string filePath) {
            return new ConfigFile(filePath);
        }
    }
}
