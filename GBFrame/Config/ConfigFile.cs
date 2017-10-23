using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace GBFrame
{
    public sealed class ConfigFile : IEnumerable
    {
        private Dictionary<string, string> mKeyValuePair = new Dictionary<string, string>();

        private string mFile;
        private ConfigFile(string file)
        {
            mFile = file;
            DecodeConfigFile();
        }

        private void DecodeConfigFile()
        {
            try
            {
                FileStream file = File.OpenRead(mFile);
                StreamReader reader = new StreamReader(file);

                string line;
                string[] keyValue;
                while ((line = reader.ReadLine()) != null)
                {
                    try
                    {
                        keyValue = line.Split(new char[] { '=' });
                        if (keyValue.Length != 2)
                            throw new Exception("InVadiate Key Value Length");

                        mKeyValuePair.Add(keyValue[0], keyValue[1]);
                    }
                    catch (Exception e)
                    {
                        Console.Write(e.Message);
                        //ignore
                    }
                }
            }
            catch (Exception)
            {
                //ignore
            }
        }

        public int Count
        {
            get
            {
                return mKeyValuePair.Count;
            }
        }

        public string Value(string key)
        {
            string value = null;
            Value(key, out value);
            return value;
        }

        public void Value(string key, out string value)
        {
            mKeyValuePair.TryGetValue(key, out value);
        }

        public void Add(string key, string value)
        {
            if (mKeyValuePair.ContainsKey(key))
                mKeyValuePair.Remove(key);
            mKeyValuePair.Add(key, value);
        }

        public bool Commit()
        {
            StreamWriter writer = null;
            try
            {
                writer = new StreamWriter(File.Open(mFile, FileMode.Create));
                foreach (KeyValuePair<string, string> keyValue in mKeyValuePair)
                {
                    writer.WriteLine(string.Format("{0}={1}", keyValue.Key, keyValue.Value));
                }
                writer.Flush();
            }
            catch (Exception e)
            {
                Console.Write(e.Message);
                return false;
            }
            finally
            {
                writer?.Close();
            }
            return true;
        }

        public void Clear()
        {
            mKeyValuePair.Clear();
        }

        public bool ContainKey(string key)
        {
            return mKeyValuePair.ContainsKey(key);
        }

        public bool ContainValue(string value)
        {
            return mKeyValuePair.ContainsValue(value);
        }

        public IEnumerator GetEnumerator()
        {
            return ((IEnumerable)mKeyValuePair).GetEnumerator();
        }

        public override string ToString()
        {
            return mKeyValuePair.ToString();
        }

        public static ConfigFile Open(string filePath)
        {
            return new ConfigFile(filePath);
        }
    }
}
