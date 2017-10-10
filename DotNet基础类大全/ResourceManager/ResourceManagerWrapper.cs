//------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2010 , Jirisoft , Ltd. 
//------------------------------------------------------------

using System;
using System.Collections;

namespace DotNet.Utilities
{
    /// <summary>
    /// ResourceManagerWrapper
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
    public class ResourceManagerWrapper
    {
        private volatile static ResourceManagerWrapper instance = null;
        private static object locker = new Object();
        private static string CurrentLanguage = "en-us";

        public static ResourceManagerWrapper Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (locker)
                    {
                        if (instance == null)
                        {
                            instance = new ResourceManagerWrapper();
                        }
                    }
                }
                return instance;
            }
        }

        private ResourceManager ResourceManager;

        public ResourceManagerWrapper()
        {
        }

        public void LoadResources(string path)
        {
            ResourceManager = ResourceManager.Instance;
            ResourceManager.Init(path);
        }

        public string Get(string key)
        {
            return ResourceManager.Get(CurrentLanguage, key);
        }

        public string Get(string lanauage, string key)
        {
            return ResourceManager.Get(lanauage, key);
        }

        public Hashtable GetLanguages()
        {
            return ResourceManager.GetLanguages();
        }

        public Hashtable GetLanguages(string path)
        {
            return ResourceManager.GetLanguages(path);
        }

        public void Serialize(string path, string language, string key, string value)
        {
            Resources Resources = this.GetResources(path, language);
            Resources.Set(key, value);
            string filePath = path + "\\" + language + ".xml";
            ResourceManager.Serialize(Resources, filePath);
        }

        public Resources GetResources(string path, string language)
        {
            string filePath = path + "\\" + language + ".xml";
            return ResourceManager.GetResources(filePath);
        }

        public Resources GetResources(string language)
        {
            return ResourceManager.LanguageResources[language];
        }
    }
}