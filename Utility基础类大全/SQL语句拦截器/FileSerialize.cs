using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Common.Utilities
{
    /// <summary>
    /// 文件序列化
    /// </summary>
    public class FileSerialize
    {
        public static T SerializerXml<T>(string filePath)
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                using (FileStream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    return (T)serializer.Deserialize(stream);
                }
            }
            catch (Exception ex)
            {
            }
            return default(T);
        }
    }
}
