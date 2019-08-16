using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Data.OracleClient;
using System.Linq;
using System.Text;

[Obsolete]
public class RedisHelper
{
    private static string connstr = System.Configuration.ConfigurationManager.AppSettings["RedisConnectStr"];// "127.0.0.1:6379,allowadmin=true";
    private static ConnectionMultiplexer conn = ConnectionMultiplexer.Connect(connstr);
    private static IDatabase db = conn.GetDatabase(1);

    /// <summary>
    /// 获取系统的redis key前缀
    /// </summary>
    /// <param name="resourceid">资源Id</param>
    /// <returns></returns>
    public static string GetMyKey(string resourceid = "")
    {
        string Key = "report_";
        if (!string.IsNullOrWhiteSpace(resourceid))
        {
            Key = string.Format("report_res_{0}", resourceid);
        }
        return Key;
    }

    #region String 可以设置过期时间

    /// <summary>
    /// 保存单个key value
    /// </summary>
    /// <param name="key">Redis Key</param>
    /// <param name="value">保存的值</param>
    /// <param name="expiry">过期时间</param>
    /// <returns></returns>
    public static bool SetStringKey(string key, string value, TimeSpan? expiry = default(TimeSpan?))
    {
        return db.StringSet(key, value, expiry);
    }

    /// <summary>
    /// 保存多个key value
    /// </summary>
    /// <param name="arr">key</param>
    /// <returns></returns>
    public static bool SetStringKey(KeyValuePair<RedisKey, RedisValue>[] arr)
    {
        return db.StringSet(arr);
    }

    /// <summary>
    /// 保存一个对象
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key"></param>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static bool SetStringKey<T>(string key, T obj, TimeSpan? expiry = default(TimeSpan?))
    {
        string json = JsonConvert.SerializeObject(obj);
        return db.StringSet(key, json, expiry);
    }

    /// <summary>
    /// 获取单个key的值
    /// </summary>
    /// <param name="key">Redis Key</param>
    /// <returns></returns>

    public static RedisValue GetStringKey(string key)
    {
        return db.StringGet(key);
    }


    /// <summary>
    /// 获取多个Key
    /// </summary>
    /// <param name="listKey">Redis Key集合</param>
    /// <returns></returns>
    public static RedisValue[] GetStringKey(List<RedisKey> listKey)
    {
        return db.StringGet(listKey.ToArray());
    }

    /// <summary>
    /// 获取一个key的对象
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key"></param>
    /// <returns></returns>
    public static T GetStringKey<T>(string key)
    {
        return JsonConvert.DeserializeObject<T>(db.StringGet(key));
    }


    #endregion

    #region Hash

    /// <summary>
    /// 保存一个集合
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key">Redis Key</param>
    /// <param name="list">数据集合</param>
    /// <param name="getModelId"></param>
    public static void HashSet<T>(string key, List<T> list, Func<T, string> getModelId)
    {
        List<HashEntry> listHashEntry = new List<HashEntry>();
        foreach (var item in list)
        {
            string json = JsonConvert.SerializeObject(item);
            listHashEntry.Add(new HashEntry(getModelId(item), json));
        }
        db.HashSet(key, listHashEntry.ToArray());
    }

    /// <summary>
    /// 获取Hash中的单个key的值
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key">Redis Key</param>
    /// <param name="hasFildValue">RedisValue</param>
    /// <returns></returns>
    public static T GetHashKey<T>(string key, string hasFildValue)
    {
        if (!string.IsNullOrWhiteSpace(key) && !string.IsNullOrWhiteSpace(hasFildValue))
        {
            RedisValue value = db.HashGet(key, hasFildValue);
            if (!value.IsNullOrEmpty)
            {
                return JsonConvert.DeserializeObject<T>(value);
            }
        }
        return default(T);
    }

    /// <summary>
    /// 获取hash中的多个key的值
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key">Redis Key</param>
    /// <param name="listhashFields">RedisValue value</param>
    /// <returns></returns>
    public static List<T> GetHashKey<T>(string key, List<RedisValue> listhashFields)
    {
        List<T> result = new List<T>();
        if (!string.IsNullOrWhiteSpace(key) && listhashFields.Count > 0)
        {
            RedisValue[] value = db.HashGet(key, listhashFields.ToArray());
            foreach (var item in value)
            {
                if (!item.IsNullOrEmpty)
                {
                    result.Add(JsonConvert.DeserializeObject<T>(item));
                }
            }
        }
        return result;
    }

    /// <summary>
    /// 获取hashkey所有Redis key
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key"></param>
    /// <returns></returns>
    public static List<T> GetHashAll<T>(string key)
    {
        List<T> result = new List<T>();
        RedisValue[] arr = db.HashKeys(key);
        foreach (var item in arr)
        {
            if (!item.IsNullOrEmpty)
            {
                result.Add(JsonConvert.DeserializeObject<T>(item));
            }
        }
        return result;
    }

    /// <summary>
    /// 获取hashkey所有的值
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key"></param>
    /// <returns></returns>
    public static List<T> HashGetAll<T>(string key)
    {
        List<T> result = new List<T>();
        HashEntry[] arr = db.HashGetAll(key);
        foreach (var item in arr)
        {
            if (!item.Value.IsNullOrEmpty)
            {
                result.Add(JsonConvert.DeserializeObject<T>(item.Value));
            }
        }
        return result;
    }

    /// <summary>
    /// 删除hasekey
    /// </summary>
    /// <param name="key"></param>
    /// <param name="hashField"></param>
    /// <returns></returns>
    public static bool DeleteHase(RedisKey key, RedisValue hashField)
    {
        return db.HashDelete(key, hashField);
    }

    #endregion

    #region key

    /// <summary>
    /// 删除单个key
    /// </summary>
    /// <param name="key">redis key</param>
    /// <returns>是否删除成功</returns>
    public static bool KeyDelete(string key)
    {
        return db.KeyDelete(key);
    }

    /// <summary>
    /// 删除多个key
    /// </summary>
    /// <param name="keys">rediskey</param>
    /// <returns>成功删除的个数</returns>
    public static long keyDelete(RedisKey[] keys)
    {
        return db.KeyDelete(keys);
    }

    /// <summary>
    /// 判断key是否存储
    /// </summary>
    /// <param name="key">redis key</param>
    /// <returns></returns>
    public static bool KeyExists(string key)
    {
        return db.KeyExists(key);
    }

    /// <summary>
    /// 重新命名key
    /// </summary>
    /// <param name="key">就的redis key</param>
    /// <param name="newKey">新的redis key</param>
    /// <returns></returns>
    public static bool KeyRename(string key, string newKey)
    {
        return db.KeyRename(key, newKey);
    }
    #endregion


    /// <summary>
    /// 追加值
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    public static void StringAppend(string key, string value)
    {
        ////追加值，返回追加后长度
        long appendlong = db.StringAppend(key, value);
    }
}
