//------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2010 , Jirisoft , Ltd. 
//------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;

namespace DotNet.Utilities
{
    /// <summary>
    /// BUResourceManager
    /// 资源管理器
    /// 
    ///	修改纪录
    ///		2007.05.16 版本：1.0 JiRiGaLa	重新调整代码的规范化。
    /// 
    /// 版本：1.0
    /// 
    /// <author>
    ///		<name>JiRiGaLa</name>
    ///		<date>2007.05.16</date>
    /// </author> 
    /// </summary>
    [XmlRoot("resources")]
    public class Resources
    {
        private SortedList<String, String> indexs = new SortedList<String, String>();
        
        [XmlElement("language")]
        public string language = string.Empty;
        [XmlElement("displayName")]
        public string displayName = string.Empty;
        [XmlElement("version")]
        public string version = string.Empty;
        [XmlElement("author")]
        public string author = string.Empty;
        [XmlElement("description")]
        public string description = string.Empty;
        [XmlElement("items", typeof(Items))]
        public Items items;

        public void createIndex()
        {
            indexs.Clear();
            if (items == null)
            {
                return;
            }
            indexs = new SortedList<String, String>(items.items.Length);
            for (int i = 0; i < items.items.Length; i++)
            {
                #if DEBUG
                    try
                    {
                        indexs.Add(items.items[i].key, items.items[i].value);
                    }
                    catch
                    {
                        throw (new Exception(items.items[i].key + items.items[i].value));
                    }
                #else
                    indexs.Add(items.items[i].key, items.items[i].value);
                #endif
            }
        }

        public string Get(string key)
        {
            if (!indexs.ContainsKey(key))
            {
                return string.Empty;
            }
            return indexs[key];
        }

        /// <summary>
        /// JiRiGaLa 2007.05.02
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool Set(string key, string value)
        {
            if (!indexs.ContainsKey(key))
            {
                return false;
            }
            indexs[key] = value;
            for (int i = 0; i < items.items.Length; i++)
            {
                if (items.items[i].key == key)
                {
                    items.items[i].value = value;
                    break;
                }
            }
            return true;
        }
    }

    public class Items
    {
        [XmlElement("item", typeof(Item))]
        public Item[] items;
    }


    public class Item
    {
        [XmlAttribute("key")]
        public string key = string.Empty;
        [XmlText]
        public string value = string.Empty;
    }


    internal class ResourcesSerializer
    {
        public static Resources DeSerialize(string filePath)
        {
            System.Xml.Serialization.XmlSerializer XmlSerializer = new System.Xml.Serialization.XmlSerializer(typeof(Resources));
            System.IO.FileStream FileStream = new System.IO.FileStream(filePath, System.IO.FileMode.Open);
            Resources Resources = XmlSerializer.Deserialize(FileStream) as Resources;
            FileStream.Close();
            return Resources;
        }

        public static void Serialize(string filePath, Resources Resources)
        {
            System.Xml.Serialization.XmlSerializer XmlSerializer = new System.Xml.Serialization.XmlSerializer(typeof(Resources));
            System.IO.FileStream FileStream = new System.IO.FileStream(filePath, System.IO.FileMode.Create);
            XmlSerializer.Serialize(FileStream, Resources);
            FileStream.Close();
        }
    }
}
