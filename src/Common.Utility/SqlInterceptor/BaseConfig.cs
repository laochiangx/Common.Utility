using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;

namespace Common.Utilities
{
    public abstract class BaseConfig<TConfig>
    {
        private TConfig _current;

        public TConfig Current
        {
            get
            {
                if (_current == null)
                {
                    string filePath = GetFilePath();
                    if (!string.IsNullOrEmpty(filePath)
                        && File.Exists(filePath))
                    {
                        _current = FileSerialize.SerializerXml<TConfig>(filePath);
                    }
                }
                return _current;
            }
        }

        public virtual string GetRootPath()
        {
            string path = ConfigurationManager.AppSettings.Get("ConfigPath");

            if (string.IsNullOrEmpty(path))
            {
                path = AppDomain.CurrentDomain.BaseDirectory + "\\Config";
            }
            return path;
        }

        public abstract string GetPathName();

        public virtual string GetFilePath()
        {
            string path = GetRootPath();
            string pathName = GetPathName();
            return string.Format("{0}\\{1}", path, pathName);
        }
    }
}
