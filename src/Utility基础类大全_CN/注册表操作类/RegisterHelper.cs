using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;
using System.Reflection;

namespace HD.Helper.Common
{
    /// <summary>
    /// 注册表辅助类
    /// </summary>
    public class RegisterHelper
    {
        /// <summary>
        /// 默认注册表基项
        /// </summary>
        private string baseKey = "Software";

        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="baseKey">基项的名称</param>
        public RegisterHelper()
        {

        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="baseKey">基项的名称</param>
        public RegisterHelper(string baseKey)
        {
            this.baseKey = baseKey;

        }
        #endregion

        #region 公共方法

        /// <summary>
        /// 写入注册表,如果指定项已经存在,则修改指定项的值
        /// </summary>
        /// <param name="keytype">注册表基项枚举</param>
        /// <param name="key">注册表项,不包括基项</param>
        /// <param name="name">值名称</param>
        /// <param name="values">值</param>
        public void SetValue(KeyType keytype, string key, string name, string values)
        {
            RegistryKey rk = (RegistryKey)GetRegistryKey(keytype);
            RegistryKey software = rk.OpenSubKey(baseKey, true);
            RegistryKey rkt = software.CreateSubKey(key);
            if (rkt != null)
            {
                rkt.SetValue(name, values);
            }
        }


        /// <summary>
        /// 读取注册表
        /// </summary>
        /// <param name="keytype">注册表基项枚举</param>
        /// <param name="key">注册表项,不包括基项</param>
        /// <param name="name">值名称</param>
        /// <returns>返回字符串</returns>
        public string GetValue(KeyType keytype, string key, string name)
        {
            RegistryKey rk = (RegistryKey)GetRegistryKey(keytype);
            RegistryKey software = rk.OpenSubKey(baseKey, true);
            RegistryKey rkt = software.OpenSubKey(key);

            if (rkt != null)
            {
                return rkt.GetValue(name).ToString();
            }
            else
            {
                return string.Empty;
            }
        }


        /// <summary>
        /// 删除注册表中的值
        /// </summary>
        /// <param name="keytype">注册表基项枚举</param>
        /// <param name="key">注册表项名称,不包括基项</param>
        /// <param name="name">值名称</param>
        public void DeleteValue(KeyType keytype, string key, string name)
        {
            RegistryKey rk = (RegistryKey)GetRegistryKey(keytype);
            RegistryKey software = rk.OpenSubKey(baseKey, true);
            RegistryKey rkt = software.OpenSubKey(key, true);

            if (rkt != null)
            {
                object value = rkt.GetValue(name);
                if (value != null)
                {
                    rkt.DeleteValue(name, true);
                }    
            }
        }


        /// <summary>
        /// 删除注册表中的指定项
        /// </summary>
        /// <param name="keytype">注册表基项枚举</param>
        /// <param name="key">注册表中的项,不包括基项</param>
        /// <returns>返回布尔值,指定操作是否成功</returns>
        public void DeleteSubKey(KeyType keytype, string key)
        {
            RegistryKey rk = (RegistryKey)GetRegistryKey(keytype);
            RegistryKey software = rk.OpenSubKey(baseKey, true);
            if (software != null)
            {
                software.DeleteSubKeyTree(key);
            }
        }


        /// <summary>
        /// 判断指定项是否存在
        /// </summary>
        /// <param name="keytype">基项枚举</param>
        /// <param name="key">指定项字符串</param>
        /// <returns>返回布尔值,说明指定项是否存在</returns>
        public bool IsExist(KeyType keytype, string key)
        {
            RegistryKey rk = (RegistryKey)GetRegistryKey(keytype);
            RegistryKey software = rk.OpenSubKey(baseKey);
            RegistryKey rkt = software.OpenSubKey(key);
            if (rkt != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        /// <summary>
        /// 检索指定项关联的所有值
        /// </summary>
        /// <param name="keytype">基项枚举</param>
        /// <param name="key">指定项字符串</param>
        /// <returns>返回指定项关联的所有值的字符串数组</returns>
        public string[] GetValues(KeyType keytype, string key)
        {
            RegistryKey rk = (RegistryKey)GetRegistryKey(keytype);
            RegistryKey software = rk.OpenSubKey(baseKey, true);
            RegistryKey rkt = software.OpenSubKey(key);
            string[] names = rkt.GetValueNames();

            if (names.Length == 0)
            {
                return names;
            }
            else
            {
                string[] values = new string[names.Length];

                int i = 0;

                foreach (string name in names)
                {
                    values[i] = rkt.GetValue(name).ToString();

                    i++;
                }

                return values;
            }

        }

        /// <summary>
        /// 将对象所有属性写入指定注册表中
        /// </summary>
        /// <param name="keytype">注册表基项枚举</param>
        /// <param name="key">注册表项,不包括基项</param>
        /// <param name="obj">传入的对象</param>
        public void SetObjectValue(KeyType keyType, string key, Object obj)
        {
            if (obj != null)
            {
                Type t = obj.GetType();

                string name;
                object value;
                foreach (var p in t.GetProperties())
                {
                    if (p != null)
                    {
                        name = p.Name;
                        value = p.GetValue(obj, null);
                        this.SetValue(keyType, key, name, value.ToString());
                    }
                }
            }
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 返回RegistryKey对象
        /// </summary>
        /// <param name="keyType">注册表基项枚举</param>
        /// <returns></returns>
        private object GetRegistryKey(KeyType keyType)
        {
            RegistryKey rk = null;

            switch (keyType)
            {
                case KeyType.HKEY_CLASS_ROOT:
                    rk = Registry.ClassesRoot;
                    break;
                case KeyType.HKEY_CURRENT_USER:
                    rk = Registry.CurrentUser;
                    break;
                case KeyType.HKEY_LOCAL_MACHINE:
                    rk = Registry.LocalMachine;
                    break;
                case KeyType.HKEY_USERS:
                    rk = Registry.Users;
                    break;
                case KeyType.HKEY_CURRENT_CONFIG:
                    rk = Registry.CurrentConfig;
                    break;
            }

            return rk;
        }

        #endregion

        #region 枚举
        /// <summary>
        /// 注册表基项枚举
        /// </summary>
        public enum KeyType : int
        {
            /// <summary>
            /// 注册表基项 HKEY_CLASSES_ROOT
            /// </summary>
            HKEY_CLASS_ROOT,
            /// <summary>
            /// 注册表基项 HKEY_CURRENT_USER
            /// </summary>
            HKEY_CURRENT_USER,
            /// <summary>
            /// 注册表基项 HKEY_LOCAL_MACHINE
            /// </summary>
            HKEY_LOCAL_MACHINE,
            /// <summary>
            /// 注册表基项 HKEY_USERS
            /// </summary>
            HKEY_USERS,
            /// <summary>
            /// 注册表基项 HKEY_CURRENT_CONFIG
            /// </summary>
            HKEY_CURRENT_CONFIG
        }
        #endregion

    }
}
