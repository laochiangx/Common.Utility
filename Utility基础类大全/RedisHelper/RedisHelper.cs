using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;


namespace DotNet.Utilities.RedisHelper
{
    /// <summary>
    /// 这是一个手动封装的完整RedisHelper
    /// </summary>
    public class RedisHelper
    {
        /// <summary>
        /// 连接字符串
        /// </summary>
        private static readonly string ConnectionString;
        /// <summary>
        /// redis 连接对象
        /// </summary>
        private static IConnectionMultiplexer _connMultiplexer;
        /// <summary>
        /// 默认的key值（用来当作RedisKey的前缀）
        /// </summary>
        public static string DefaultKey { get; private set; }
        /// <summary>
        /// 锁tomcat 
        /// </summary>
        private static readonly object Locker = new object();
        /// <summary>
        /// 数据库访问对象
        /// </summary>
        private readonly IDatabase _db;
        /// <summary>
        /// 采用双重锁单例模式，保证数据访问对象有且仅有一个
        /// </summary>
        /// <returns></returns>
        public IConnectionMultiplexer GetConnectionRedisMultiplexer()
        {
            if ((_connMultiplexer == null || !_connMultiplexer.IsConnected))
            {
                lock (Locker)
                {
                    if ((_connMultiplexer == null || !_connMultiplexer.IsConnected))
                    {
                        _connMultiplexer = ConnectionMultiplexer.Connect(ConnectionString);
                    }
                }
            }
            return _connMultiplexer;
        }
        /// <summary>
        /// 添加事务处理
        /// </summary>
        /// <returns></returns>
        public ITransaction GetTransaction()
        {
            //创建事务
            return _db.CreateTransaction();
        }

        #region 构造函数
        /// <summary>
        /// 静态的构造函数,
        /// 构造函数是属于类的，而不是属于实例的
        /// 就是说这个构造函数只会被执行一次。也就是在创建第一个实例或引用任何静态成员之前，由.NET自动调用。
        /// </summary>
        static RedisHelper()
        {
            ConnectionString = "127.0.0.1:6375";
            _connMultiplexer = ConnectionMultiplexer.Connect(ConnectionString);
            DefaultKey = "finchina";
            RegisterEvent();
        }
        /// <summary>
        /// 重载构造器，获取redis内部数据库的交互式连接
        /// </summary>
        /// <param name="db">要获取的数据库ID</param>
        public RedisHelper(int db = -1)
        {
            _db = _connMultiplexer.GetDatabase(db);
        }
        #endregion

        #region 通用方法
        /// <summary>
        /// 添加 key 的前缀
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private static string AddKeyPrefix(string key)
        {
            return $"{DefaultKey}:{key}";
        }
        /// <summary>
        /// 序列化,用于存储对象
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private static byte[] Serialize(object obj)
        {
            try
            {
                if (obj == null)
                    return null;
                var binaryFormatter = new BinaryFormatter();
                using (var memoryStream = new MemoryStream())
                {
                    binaryFormatter.Serialize(memoryStream, obj);
                    var data = memoryStream.ToArray();
                    return data;
                }
            }
            catch (SerializationException ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 反序列化，用于解码对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        private static T Deserialize<T>(byte[] data)
        {
            if (data == null)
                return default(T);
            var binaryFormatter = new BinaryFormatter();
            using (var memoryStream = new MemoryStream(data))
            {
                var result = (T)binaryFormatter.Deserialize(memoryStream);
                return result;
            }
        }
        #endregion 

        #region stringGet 
        /// <summary>
        /// 设置key，并保存字符串（如果key 已存在，则覆盖）
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="redisValue"></param>
        /// <param name="expried"></param>
        /// <returns></returns>
        public bool StringSet(string redisKey, string redisValue, TimeSpan? expried = null)
        {
            redisKey = AddKeyPrefix(redisKey);
            return _db.StringSet(redisKey, redisValue, expried);
        }
        /// <summary>
        /// 保存多个key-value
        /// </summary>
        /// <param name="keyValuePairs"></param>
        /// <returns></returns>
        public bool StringSet(IEnumerable<KeyValuePair<RedisKey, RedisValue>> keyValuePairs)
        {
            keyValuePairs =
                keyValuePairs.Select(x => new KeyValuePair<RedisKey, RedisValue>(AddKeyPrefix(x.Key), x.Value));
            return _db.StringSet(keyValuePairs.ToArray());
        }
        /// <summary>
        /// 获取字符串
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="expired"></param>
        /// <returns></returns>
        public string StringGet(string redisKey, TimeSpan? expired = null)
        {
            try
            {
                redisKey = AddKeyPrefix(redisKey);
                return _db.StringGet(redisKey);
            }
            catch (TypeAccessException ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 存储一个对象，该对象会被序列化存储
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="redisKey"></param>
        /// <param name="redisValue"></param>
        /// <param name="expired"></param>
        /// <returns></returns>
        public bool StringSet<T>(string redisKey, T redisValue, TimeSpan? expired = null)
        {
            redisKey = AddKeyPrefix(redisKey);
            var json = Serialize(redisKey);
            return _db.StringSet(redisKey, json, expired);
        }
        /// <summary>
        /// 获取一个对象(会进行反序列化)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="redisKey"></param>
        /// <param name="expired"></param>
        /// <returns></returns>
        public T StringSet<T>(string redisKey, TimeSpan? expired = null)
        {
            redisKey = AddKeyPrefix(redisKey);
            return Deserialize<T>(_db.StringGet(redisKey));
        }

        /// <summary>
        /// 保存一个字符串值
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="redisValue"></param>
        /// <param name="expired"></param>
        /// <returns></returns>
        public async Task<bool> StringSetAddKeyPrefix(string redisKey, string redisValue, TimeSpan? expired = null)
        {
            redisKey = AddKeyPrefix(redisKey);
            return  _db.StringSet(redisKey, redisValue, expired);
        }
        /// <summary>
        /// 保存一个字符串值
        /// </summary>
        /// <param name="keyValuePairs"></param>
        /// <returns></returns>
        public async Task<bool> StringSetkeyValuePairs(IEnumerable<KeyValuePair<RedisKey, RedisValue>> keyValuePairs)
        {
            keyValuePairs
                = keyValuePairs.Select(x => new KeyValuePair<RedisKey, RedisValue>(AddKeyPrefix(x.Key), x.Value));
            return  _db.StringSet(keyValuePairs.ToArray());
        }
        /// <summary>
        /// 获取单个值
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="redisValue"></param>
        /// <param name="expired"></param>
        /// <returns></returns>
        public async Task<string> StringGet(string redisKey, string redisValue, TimeSpan? expired = null)
        {
            redisKey = AddKeyPrefix(redisKey);
            return  _db.StringGet(redisKey);
        }
        /// <summary>
        /// 存储一个对象（该对象会被序列化保存）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="redisKey"></param>
        /// <param name="redisValue"></param>
        /// <param name="expired"></param>
        /// <returns></returns>
        public async Task<bool> StringSet<T>(string redisKey, string redisValue, TimeSpan? expired = null)
        {
            redisKey = AddKeyPrefix(redisKey);
            var json = Serialize(redisValue);
            return  _db.StringSet(redisKey, json, expired);
        }
        /// <summary>
        /// 获取一个对象（反序列化）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="redisKey"></param>
        /// <param name="redisValue"></param>
        /// <param name="expired"></param>
        /// <returns></returns>
        public async Task<T> StringGet<T>(string redisKey, string redisValue, TimeSpan? expired = null)
        {
            redisKey = AddKeyPrefix(redisKey);
            return Deserialize<T>( _db.StringGet(redisKey));
        }
        #endregion

        #region  Hash
        /// <summary>
        /// 判断字段是否在hash中
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="redisValue"></param>
        /// <returns></returns>
        public bool HashExist(string redisKey, string hashField)
        {
            redisKey = AddKeyPrefix(redisKey);
            return _db.HashExists(redisKey, hashField);
        }
        /// <summary>
        /// 从hash 中删除字段
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="hashField"></param>
        /// <returns></returns>
        public bool HashDelete(string redisKey, string hashField)
        {
            redisKey = AddKeyPrefix(redisKey);
            return _db.HashDelete(redisKey, hashField);
        }
        /// <summary>
        /// 从hash中移除指定字段
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="hashField"></param>
        /// <returns></returns>
        public long HashDelete(string redisKey, IEnumerable<RedisValue> hashField)
        {
            redisKey = AddKeyPrefix(redisKey);
            return _db.HashDelete(redisKey, hashField.ToArray());
        }
        /// <summary>
        /// 在hash中设定值
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="hashField"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool HashSet(string redisKey, string hashField, string value)
        {
            redisKey = AddKeyPrefix(redisKey);
            return _db.HashSet(redisKey, hashField, value);
        }
        /// <summary>
        /// 从Hash 中获取值
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="hashField"></param>
        /// <returns></returns>
        public RedisValue HashGet(string redisKey, string hashField)
        {
            redisKey = AddKeyPrefix(redisKey);
            return _db.HashGet(redisKey, hashField);
        }
        /// <summary>
        /// 从Hash 中获取值
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="hashField"></param>
        /// <returns></returns>
        public RedisValue[] HashGet(string redisKey, RedisValue[] hashField)
        {
            redisKey = AddKeyPrefix(redisKey);
            return _db.HashGet(redisKey, hashField);
        }
        /// <summary>
        /// 从hash 返回所有的key值
        /// </summary>
        /// <param name="redisKey"></param>
        /// <returns></returns>
        public IEnumerable<RedisValue> HashKeys(string redisKey)
        {
            redisKey = AddKeyPrefix(redisKey);
            return _db.HashKeys(redisKey);
        }
        /// <summary>
        /// 根据key返回hash中的值
        /// </summary>
        /// <param name="redisKey"></param>
        /// <returns></returns>
        public RedisValue[] HashValues(string redisKey)
        {
            redisKey = AddKeyPrefix(redisKey);
            return _db.HashValues(redisKey);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="redisKey"></param>
        /// <param name="hashField"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool HashSet<T>(string redisKey, string hashField, T value)
        {
            redisKey = AddKeyPrefix(redisKey);
            var json = Serialize(value);
            return _db.HashSet(redisKey, hashField, json);
        }
        /// <summary>
        /// 在hash 中获取值 （反序列化）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="redisKey"></param>
        /// <param name="hashField"></param>
        /// <returns></returns>
        public T HashGet<T>(string redisKey, string hashField)
        {
            redisKey = AddKeyPrefix(redisKey);
            return Deserialize<T>(_db.HashGet(redisKey, hashField));
        }
        /// <summary>
        /// 判断字段是否存在hash 中
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="hashField"></param>
        /// <returns></returns>
        public async Task<bool> HashExists(string redisKey, string hashField)
        {
            redisKey = AddKeyPrefix(redisKey);
            return  _db.HashExists(redisKey, hashField);
        }
        /// <summary>
        /// 从hash中移除指定字段
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="hashField"></param>
        /// <returns></returns>
        public async Task<bool> HashDeleteByRedisKey(string redisKey, string hashField)
        {
            redisKey = AddKeyPrefix(redisKey);
            return  _db.HashDelete(redisKey, hashField);
        }
        /// <summary>
        /// 从hash中移除指定字段
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="hashField"></param>
        /// <returns></returns>
        public async Task<long> HashDeleteByRedisKey(string redisKey, IEnumerable<RedisValue> hashField)
        {
            redisKey = AddKeyPrefix(redisKey);
            return  _db.HashDelete(redisKey, hashField.ToArray());
        }
        /// <summary>
        /// 在hash 设置值
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="hashField"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public async Task<bool> HashSetByRedisKey(string redisKey, string hashField, string value)
        {
            redisKey = AddKeyPrefix(redisKey);
            return  _db.HashSet(redisKey, hashField, value);
        }
        /// <summary>
        /// 在hash 中设定值
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="hashFields"></param>
        /// <returns></returns>
        public async Task HashSet(string redisKey, IEnumerable<HashEntry> hashFields)
        {
            redisKey = AddKeyPrefix(redisKey);
             _db.HashSet(redisKey, hashFields.ToArray());
        }
        /// <summary>
        /// 在hash 中设定值
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="hashField"></param>
        /// <returns></returns>
        public async Task<RedisValue> HashGetByRedisKey(string redisKey, string hashField)
        {
            redisKey = AddKeyPrefix(redisKey);
            return  _db.HashGet(redisKey, hashField);
        }
        /// <summary>
        /// 在hash 中获取值
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="hashField"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public async Task<IEnumerable<RedisValue>> HashGet(string redisKey, RedisValue[] hashField, string value)
        {
            redisKey = AddKeyPrefix(redisKey);
            return  _db.HashGet(redisKey, hashField);
        }
        /// <summary>
        /// 从hash返回所有的字段值
        /// </summary>
        /// <param name="redisKey"></param>
        /// <returns></returns>
        public async Task<IEnumerable<RedisValue>> HashKeysByRedisKey(string redisKey)
        {
            redisKey = AddKeyPrefix(redisKey);
            return  _db.HashKeys(redisKey);
        }
        /// <summary>
        /// 返回hash中所有的值
        /// </summary>
        /// <param name="redisKey"></param>
        /// <returns></returns>
        public async Task<IEnumerable<RedisValue>> HashValuesByRedisKey(string redisKey)
        {
            redisKey = AddKeyPrefix(redisKey);
            return  _db.HashValues(redisKey);
        }
        /// <summary>
        /// 在hash 中设定值（序列化）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="redisKey"></param>
        /// <param name="hashField"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public async Task<bool> HashSetAsync<T>(string redisKey, string hashField, T value)
        {
            redisKey = AddKeyPrefix(redisKey);
            var json = Serialize(value);
            return  _db.HashSet(redisKey, hashField, json);
        }
        /// <summary>
        /// 在hash中获取值（反序列化）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="redisKey"></param>
        /// <param name="hashField"></param>
        /// <returns></returns>
        public async Task<T> HashGetAsync<T>(string redisKey, string hashField)
        {
            redisKey = AddKeyPrefix(redisKey);
            return Deserialize<T>( _db.HashGet(redisKey, hashField));
        }
        #endregion

        #region list
        /// <summary>
        /// 移除并返回key所对应列表的第一个元素
        /// </summary>
        /// <param name="redisKey"></param>
        /// <returns></returns>
        public string ListLeftPop(string redisKey)
        {
            redisKey = AddKeyPrefix(redisKey);
            return _db.ListLeftPop(redisKey);
        }
        /// <summary>
        /// 移除并返回key所对应列表的最后一个元素
        /// </summary>
        /// <param name="redisKey"></param>
        /// <returns></returns>
        public string ListRightPop(string redisKey)
        {
            redisKey = AddKeyPrefix(redisKey);
            return _db.ListRightPop(redisKey);
        }
        /// <summary>
        /// 移除指定key及key所对应的元素
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="redisValue"></param>
        /// <returns></returns>
        public long ListRemove(string redisKey, string redisValue)
        {
            redisKey = AddKeyPrefix(redisKey);
            return _db.ListRemove(redisKey, redisValue);
        }
        /// <summary>
        /// 在列表尾部插入值，如果键不存在，先创建再插入值
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="redisValue"></param>
        /// <returns></returns>
        public long ListRightPush(string redisKey, string redisValue)
        {
            redisKey = AddKeyPrefix(redisKey);
            return _db.ListRightPush(redisKey, redisValue);
        }
        /// <summary>
        /// 在列表头部插入值，如果键不存在，先创建再插入值
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="redisValue"></param>
        /// <returns></returns>
        public long ListLeftPush(string redisKey, string redisValue)
        {
            redisKey = AddKeyPrefix(redisKey);
            return _db.ListLeftPush(redisKey, redisValue);
        }
        /// <summary>
        /// 返回列表上该键的长度，如果不存在，返回0
        /// </summary>
        /// <param name="redisKey"></param>
        /// <returns></returns>
        public long ListLength(string redisKey)
        {
            redisKey = AddKeyPrefix(redisKey);
            return _db.ListLength(redisKey);
        }
        /// <summary>
        /// 返回在该列表上键所对应的元素
        /// </summary>
        /// <param name="redisKey"></param>
        /// <returns></returns>
        public IEnumerable<RedisValue> ListRange(string redisKey)
        {
            try
            {
                redisKey = AddKeyPrefix(redisKey);
                return _db.ListRange(redisKey);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 移除并返回存储在该键列表的第一个元素
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="redisKey"></param>
        /// <returns></returns>
        public T ListLeftPop<T>(string redisKey)
        {
            redisKey = AddKeyPrefix(redisKey);
            return Deserialize<T>(_db.ListLeftPop(redisKey));
        }
        /// <summary>
        /// 移除并返回该列表上的最后一个元素
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="redisKey"></param>
        /// <returns></returns>
        public T ListRightPop<T>(string redisKey)
        {
            redisKey = AddKeyPrefix(redisKey);
            return Deserialize<T>(_db.ListRightPop(redisKey));
        }
        /// <summary>
        /// 在列表尾部插入值，如果键不存在，先创建再插入值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="redisKey"></param>
        /// <param name="redisValue"></param>
        /// <returns></returns>
        public long ListRightPush<T>(string redisKey, T redisValue)
        {
            redisKey = AddKeyPrefix(redisKey);
            return _db.ListRightPush(redisKey, Serialize(redisValue));
        }
        /// <summary>
        /// 在列表头部插入值，如果键不存在，创建后插入值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="redisKey"></param>
        /// <param name="redisValue"></param>
        /// <returns></returns>
        public long ListLeftPush<T>(string redisKey, T redisValue)
        {
            redisKey = AddKeyPrefix(redisKey);
            return _db.ListRightPush(redisKey, Serialize(redisValue));
        }
        /// <summary>
        /// 移除并返回存储在该键列表的第一个元素
        /// </summary>
        /// <param name="redisKey"></param>
        /// <returns></returns>
        public async Task<string> ListLeftPopByRedisKey(string redisKey)
        {
            redisKey = AddKeyPrefix(redisKey);
            return  _db.ListLeftPop(redisKey);
        }
        /// <summary>
        /// 移除并返回存储在该键列表的最后一个元素
        /// </summary>
        /// <param name="redisKey"></param>
        /// <returns></returns>
        public async Task<string> ListRightPopByRedisKey(string redisKey)
        {
            redisKey = AddKeyPrefix(redisKey);
            return  _db.ListRightPop(redisKey);
        }
        /// <summary>
        /// 移除列表指定键上与值相同的元素
        /// </summary>
        /// <param name="redisKey"></param>
        /// <returns></returns>
        public async Task<long> ListRemoveByRedisKey(string redisKey, string redisValue)
        {
            redisKey = AddKeyPrefix(redisKey);
            return  _db.ListRemove(redisKey, redisValue);
        }
        /// <summary>
        /// 在列表尾部差入值，如果键不存在，先创建后插入
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="redisValue"></param>
        /// <returns></returns>
        public async Task<long> ListRightPushByRedisKey(string redisKey, string redisValue)
        {
            redisKey = AddKeyPrefix(redisKey);
            return  ListRightPush(redisKey, redisValue);
        }
        /// <summary>
        /// 在列表头部插入值，如果键不存在，先创建后插入
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="redisValue"></param>
        /// <returns></returns>
        public async Task<long> ListLeftPushByRedisKey(string redisKey, string redisValue)
        {
            redisKey = AddKeyPrefix(redisKey);
            return  _db.ListLeftPush(redisKey, redisValue);
        }
        /// <summary>
        /// 返回列表上的长度，如果不存在，返回0
        /// </summary>
        /// <param name="redisKey"></param>
        /// <returns></returns>
        public async Task<long> ListLengthByRedisKey(string redisKey)
        {
            redisKey = AddKeyPrefix(redisKey);
            return  _db.ListLength(redisKey);
        }
        /// <summary>
        /// 返回在列表上键对应的元素
        /// </summary>
        /// <param name="redisKey"></param>
        /// <returns></returns>
        public async Task<IEnumerable<RedisValue>> ListRangeByRedisKey(string redisKey)
        {
            redisKey = AddKeyPrefix(redisKey);
            return  _db.ListRange(redisKey);
        }
        /// <summary>
        /// 移除并返回存储在key对应列表的第一个元素
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="redisKey"></param>
        /// <returns></returns>
        public async Task<T> ListLeftPopAsync<T>(string redisKey)
        {
            redisKey = AddKeyPrefix(redisKey);
            return Deserialize<T>( _db.ListLeftPop(redisKey));
        }
        /// <summary>
        /// 移除并返回存储在key 对应列表的最后一个元素
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="redisKey"></param>
        /// <returns></returns>
        public async Task<T> ListRightPopAsync<T>(string redisKey)
        {
            redisKey = AddKeyPrefix(redisKey);
            return Deserialize<T>( _db.ListRightPop(redisKey));
        }
        /// <summary>
        /// 在列表尾部插入值，如果值不存在，先创建后写入值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="redisKey"></param>
        /// <param name="redisValue"></param>
        /// <returns></returns>
        public async Task<long> ListRightPushAsync<T>(string redisKey, string redisValue)
        {
            redisKey = AddKeyPrefix(redisKey);
            return  _db.ListRightPush(redisKey, Serialize(redisValue));
        }
        /// <summary>
        /// 在列表头部插入值，如果值不存在，先创建后写入值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="redisKey"></param>
        /// <param name="redisValue"></param>
        /// <returns></returns>
        public async Task<long> ListLeftPushAsync<T>(string redisKey, string redisValue)
        {
            redisKey = AddKeyPrefix(redisKey);
            return  _db.ListLeftPush(redisKey, Serialize(redisValue));
        }
        #endregion

        #region SortedSet 操作
        /// <summary>
        /// sortedset 新增
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="member"></param>
        /// <param name="score"></param>
        /// <returns></returns>
        public bool SortedSetAdd(string redisKey, string member, double score)
        {
            redisKey = AddKeyPrefix(redisKey);
            return _db.SortedSetAdd(redisKey, member, score);
        }
        /// <summary>
        /// 在有序集合中返回指定范围的元素，默认情况下由低到高
        /// </summary>
        /// <param name="redisKey"></param>
        /// <returns></returns>
        public IEnumerable<RedisValue> SortedSetRangeByRank(string redisKey)
        {
            redisKey = AddKeyPrefix(redisKey);
            return _db.SortedSetRangeByRank(redisKey);
        }
        /// <summary>
        /// 返回有序集合的个数
        /// </summary>
        /// <param name="redisKey"></param>
        /// <returns></returns>
        public long SortedSetLength(string redisKey)
        {
            redisKey = AddKeyPrefix(redisKey);
            return _db.SortedSetLength(redisKey);
        }
        /// <summary>
        /// 返回有序集合的元素个数
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="member"></param>
        /// <returns></returns>
        public bool SortedSetLength(string redisKey, string member)
        {
            redisKey = AddKeyPrefix(redisKey);
            return _db.SortedSetRemove(redisKey, member);
        }
        /// <summary>
        ///  sorted set Add
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="redisKey"></param>
        /// <param name="member"></param>
        /// <param name="score"></param>
        /// <returns></returns>
        public bool SortedSetAdd<T>(string redisKey, T member, double score)
        {
            redisKey = AddKeyPrefix(redisKey);
            var json = Serialize(member);
            return _db.SortedSetAdd(redisKey, json, score);
        }

        #region SortedSet-Async
        /// <summary>
        /// SortedSet 新增
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="member"></param>
        /// <param name="score"></param>
        /// <returns></returns>
        public async Task<bool> SortedSetAddByRedisKey(string redisKey, string member, double score)
        {
            redisKey = AddKeyPrefix(redisKey);
            return  _db.SortedSetAdd(redisKey, member, score);
        }
        /// <summary>
        /// 在有序集合中返回指定范围的元素，默认情况下由低到高
        /// </summary>
        /// <param name="redisKey"></param>
        /// <returns></returns>
        public async Task<IEnumerable<RedisValue>> SortedSetRangeByRankByRedisKey(string redisKey)
        {
            redisKey = AddKeyPrefix(redisKey);
            return  _db.SortedSetRangeByRank(redisKey);
        }
        /// <summary>
        /// 返回有序集合的元素个数
        /// </summary>
        /// <param name="redisKey"></param>
        /// <returns></returns>
        public async Task<long> SortedSetLengthByRedisKey(string redisKey)
        {
            redisKey = AddKeyPrefix(redisKey);
            return  _db.SortedSetLength(redisKey);
        }
        /// <summary>
        /// 返回有序集合的元素个数
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="member"></param>
        /// <returns></returns>
        public async Task<bool> SortedSetRemove(string redisKey, string member)
        {
            redisKey = AddKeyPrefix(redisKey);
            return  _db.SortedSetRemove(redisKey, member);
        }
        /// <summary>
        /// SortedSet 新增
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="redisKey"></param>
        /// <param name="member"></param>
        /// <param name="score"></param>
        /// <returns></returns>
        public async Task<bool> SortedSetAddAsync<T>(string redisKey, T member, double score)
        {
            redisKey = AddKeyPrefix(redisKey);
            var json = Serialize(member);
            return  _db.SortedSetAdd(redisKey, json, score);
        }
        #endregion SortedSet-Async

        #endregion

        #region key
        /// <summary>
        /// 移除指定key
        /// </summary>
        /// <param name="redisKey"></param>
        /// <returns></returns>
        public bool KeyDelete(string redisKey)
        {
            redisKey = AddKeyPrefix(redisKey);
            return _db.KeyDelete(redisKey);
        }
        /// <summary>
        /// 删除指定key
        /// </summary>
        /// <param name="redisKeys"></param>
        /// <returns></returns>
        public long KeyDelete(IEnumerable<string> redisKeys)
        {
            var keys = redisKeys.Select(x => (RedisKey)AddKeyPrefix(x));
            return _db.KeyDelete(keys.ToArray());
        }
        /// <summary>
        /// 检验key是否存在
        /// </summary>
        /// <param name="redisKey"></param>
        /// <returns></returns>
        public bool KeyExists(string redisKey)
        {
            redisKey = AddKeyPrefix(redisKey);
            return _db.KeyExists(redisKey);
        }
        /// <summary>
        /// 重命名key
        /// </summary>
        /// <param name="oldKeyName"></param>
        /// <param name="newKeyName"></param>
        /// <returns></returns>
        public bool KeyReName(string oldKeyName, string newKeyName)
        {
            oldKeyName = AddKeyPrefix(oldKeyName);
            return _db.KeyRename(oldKeyName, newKeyName);
        }
        /// <summary>
        /// 设置key 的过期时间
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="expired"></param>
        /// <returns></returns>
        public bool KeyExpire(string redisKey, TimeSpan? expired = null)
        {
            redisKey = AddKeyPrefix(redisKey);
            return _db.KeyExpire(redisKey, expired);
        }

        #region key-async
        /// <summary>
        /// 移除指定的key
        /// </summary>
        /// <param name="redisKey"></param>
        /// <returns></returns>
        public async Task<bool> KeyDeleteByRedisKey(string redisKey)
        {
            redisKey = AddKeyPrefix(redisKey);
            return  _db.KeyDelete(redisKey);
        }
        /// <summary>
        /// 删除指定的key
        /// </summary>
        /// <param name="redisKeys"></param>
        /// <returns></returns>
        public async Task<long> KeyDeleteByRedisKey(IEnumerable<string> redisKeys)
        {
            var keys = redisKeys.Select(x => (RedisKey)AddKeyPrefix(x));
            return  _db.KeyDelete(keys.ToArray());
        }
        /// <summary>
        /// 检验key 是否存在
        /// </summary>
        /// <param name="redisKey"></param>
        /// <returns></returns>
        public async Task<bool> KeyExistsByRedisKey(string redisKey)
        {
            redisKey = AddKeyPrefix(redisKey);
            return  _db.KeyExists(redisKey);
        }
        /// <summary>
        /// 重命名key
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="redisNewKey"></param>
        /// <returns></returns>
        public async Task<bool> KeyRename(string redisKey, string redisNewKey)
        {
            redisKey = AddKeyPrefix(redisKey);
            return  _db.KeyRename(redisKey, redisNewKey);
        }
        /// <summary>
        /// 设置 key 时间
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="expired"></param>
        /// <returns></returns>
        public async Task<bool> KeyExpireByRedisKey(string redisKey, TimeSpan? expired)
        {
            redisKey = AddKeyPrefix(redisKey);
            return  _db.KeyExpire(redisKey, expired);
        }
        #endregion key-async

        #endregion

        #region 发布订阅
        /// <summary>
        /// 订阅
        /// </summary>
        /// <param name="channel">频道</param>
        /// <param name="handle">事件</param>
        public void Subscribe(RedisChannel channel, Action<RedisChannel, RedisValue> handle)
        {
            //getSubscriber() 获取到指定服务器的发布者订阅者的连接
            var sub = _connMultiplexer.GetSubscriber();
            //订阅执行某些操作时改变了 优先/主动 节点广播
            sub.Subscribe(channel, handle);
        }
        /// <summary>
        /// 发布
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public long Publish(RedisChannel channel, RedisValue message)
        {
            var sub = _connMultiplexer.GetSubscriber();
            return sub.Publish(channel, message);
        }
        /// <summary>
        /// 发布（使用序列化）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="channel"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public long Publish<T>(RedisChannel channel, T message)
        {
            var sub = _connMultiplexer.GetSubscriber();
            return sub.Publish(channel, Serialize(message));
        }

        #region 发布订阅-async
        /// <summary>
        /// 订阅
        /// </summary>
        /// <param name="redisChannel"></param>
        /// <param name="handle"></param>
        /// <returns></returns>
        public async Task SubscribeByRedisKey(RedisChannel redisChannel, Action<RedisChannel, RedisValue> handle)
        {
            var sub = _connMultiplexer.GetSubscriber();
             sub.Subscribe(redisChannel, handle);
        }
        /// <summary>
        /// 发布
        /// </summary>
        /// <param name="redisChannel"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task<long> PublishByRedisKey(RedisChannel redisChannel, RedisValue message)
        {
            var sub = _connMultiplexer.GetSubscriber();
            return  sub.Publish(redisChannel, message);
        }
        /// <summary>
        /// 发布（使用序列化）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="redisChannel"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task<long> PublishAsync<T>(RedisChannel redisChannel, T message)
        {
            var sub = _connMultiplexer.GetSubscriber();
            return  sub.Publish(redisChannel, Serialize(message));
        }
        #endregion 发布订阅-async

        #endregion

        #region 注册事件
        /// <summary>
        /// 注册事件
        /// </summary>
        private static void RegisterEvent()
        {
            _connMultiplexer.ConnectionRestored += ConnMultiplexer_ConnectionRestored;
            _connMultiplexer.ConnectionFailed += ConnMultiplexer_ConnectionFailed;
            _connMultiplexer.ErrorMessage += ConnMultiplexer_ErrorMessage;
            _connMultiplexer.ConfigurationChanged += ConnMultiplexer_ConfigurationChanged;
            _connMultiplexer.HashSlotMoved += ConnMultiplexer_HashSlotMoved;
            _connMultiplexer.InternalError += ConnMultiplexer_InternalError;
            _connMultiplexer.ConfigurationChangedBroadcast += ConnMultiplexer_ConfigurationChangedBroadcast;
        }
        /// <summary>
        /// 重新配置广播时(主从同步更改)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void ConnMultiplexer_ConfigurationChangedBroadcast(object sender, EndPointEventArgs e)
        {
            Console.WriteLine($"{nameof(ConnMultiplexer_ConfigurationChangedBroadcast)}: {e.EndPoint}");
        }
        /// <summary>
        /// 发生内部错误时(调试用)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void ConnMultiplexer_InternalError(object sender, InternalErrorEventArgs e)
        {
            Console.WriteLine($"{nameof(ConnMultiplexer_InternalError)}: {e.Exception}");
        }
        /// <summary>
        /// 更改集群时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void ConnMultiplexer_HashSlotMoved(object sender, HashSlotMovedEventArgs e)
        {
            Console.WriteLine($"{nameof(ConnMultiplexer_HashSlotMoved)}: {nameof(e.OldEndPoint)}-{e.OldEndPoint} To {nameof(e.NewEndPoint)}-{e.NewEndPoint} ");
        }
        /// <summary>
        /// 配置更改时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void ConnMultiplexer_ConfigurationChanged(object sender, EndPointEventArgs e)
        {
            Console.WriteLine($"{nameof(ConnMultiplexer_ConfigurationChanged)}: {e.EndPoint}");
        }
        /// <summary>
        /// 发生错误时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void ConnMultiplexer_ErrorMessage(object sender, RedisErrorEventArgs e)
        {
            Console.WriteLine($"{nameof(ConnMultiplexer_ErrorMessage)}: {e.Message}");
        }
        /// <summary>
        /// 物理连接失败时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void ConnMultiplexer_ConnectionFailed(object sender, ConnectionFailedEventArgs e)
        {
            Console.WriteLine($"{nameof(ConnMultiplexer_ConnectionFailed)}: {e.Exception}");
        }
        /// <summary>
        /// 建立物理连接时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void ConnMultiplexer_ConnectionRestored(object sender, ConnectionFailedEventArgs e)
        {
            Console.WriteLine($"{nameof(ConnMultiplexer_ConnectionRestored)}: {e.Exception}");
        }
        #endregion

    }
}
