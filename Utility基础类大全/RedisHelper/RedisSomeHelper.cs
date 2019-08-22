using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using ServiceStack.Redis;
using System.Linq;

namespace RedisSomeHelper
{
    public class PooledRedisClientHelper : IDisposable
    {
        /*copyright@2013 All Rights Reserved
         * servicestack.redis为github中的开源项目
         * redis是一个典型的k/v型数据库
         * redis共支持五种类型的数据 string,list,hash,set,sortedset
         * 
         * string是最简单的字符串类型
         * 
         * list是字符串列表，其内部是用双向链表实现的，因此在获取/设置数据时可以支持正负索引
         * 也可以将其当做堆栈结构使用
         * 
         * hash类型是一种字典结构，也是最接近RDBMS的数据类型，其存储了字段和字段值的映射，但字段值只能是
         * 字符串类型，散列类型适合存储对象，建议使用对象类别和ID构成键名，使用字段表示对象属性，字
         * 段值存储属性值，例如：car:2 price 500 ,car:2  color black,用redis命令设置散列时，命令格式
         * 如下：HSET key field value，即key，字段名，字段值
         * 
         * set是一种集合类型，redis中可以对集合进行交集，并集和互斥运算
         *           
         * sorted set是在集合的基础上为每个元素关联了一个“分数”，我们能够
         * 获得分数最高的前N个元素，获得指定分数范围内的元素，元素是不同的，但是"分数"可以是相同的
         * set是用散列表和跳跃表实现的，获取数据的速度平均为o(log(N))
         * 
         * 需要注意的是，redis所有数据类型都不支持嵌套
         * redis中一般不区分插入和更新操作，只是命令的返回值不同
         * 在插入key时，如果不存在，将会自动创建
         *
         * 以下方法为基本的设置数据和取数据
         */

        /// <summary>
        /// 连接池管理器
        /// </summary>
        /// <param name="readWriteHosts"></param>
        /// <param name="readOnlyHosts"></param>
        /// <returns></returns>
        private static PooledRedisClientManager CreateManager(string[] readWriteHosts, string[] readOnlyHosts)
        {
            //WriteServerList：可写的Redis链接地址。
            //ReadServerList：可读的Redis链接地址。
            //MaxWritePoolSize：最大写链接数。
            //MaxReadPoolSize：最大读链接数。
            //AutoStart：自动重启。
            //LocalCacheTime：本地缓存到期时间，单位:秒。
            //RecordeLog：是否记录日志,该设置仅用于排查redis运行时出现的问题,如redis工作正常,请关闭该项。
            //RedisConfigInfo类是记录redis连接信息，此信息和配置文件中的RedisConfig相呼应
            //支持读写分离，均衡负载 
            return new PooledRedisClientManager(readWriteHosts, readOnlyHosts,
                new RedisClientManagerConfig
                {
                    MaxWritePoolSize = 5,
                    MaxReadPoolSize = 5,
                    AutoStart = true,
                });
        }

        /// <summary>
        /// 创建连接
        /// </summary>
        private readonly static PooledRedisClientManager PooleClient =
            CreateManager(
                new[] { ConfigHelper.ReadWriteHosts },
                new[] { ConfigHelper.ReadOnlyHosts }
            );
        //private static RedisClient redisCli = new RedisClient("192.168.101.165", 6379, "123456");

        #region String

        /// <summary>
        /// 获取key,返回string格式
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetValueString(string key)
        {
            using (IRedisClient redis = PooleClient.GetClient())
            {
                string value = redis.GetValue(key);
                return value;
            }
        }

        /// <summary>
        /// 返回Redis是否包含当前KEY
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool ContainsKey(string key)
        {
            using (IRedisClient redis = PooleClient.GetClient())
            {
                return redis.ContainsKey(key);
            }
        }

        /// <summary>
        /// 获取key,返回泛型格式
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static T GetT<T>(string key)
        {
            using (IRedisClient redis = PooleClient.GetClient())
            {
                return redis.Get<T>(key);
            }
        }

        /// <summary>
        /// 设置key,传入泛型格式
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static void SetT<T>(string key, T value)
        {
            using (IRedisClient redis = PooleClient.GetClient())
            {
                if (!redis.Set(key, value))
                {
                    if (redis.ContainsKey(key) && !redis.Remove(key))
                        throw new Exception(string.Format("redis产生脏数据,{0}=>{1}", key, value));
                }
            }
        }

        /// <summary>
        /// 设置key,传入泛型格式和过期时间
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="exp"></param>
        public static void Set<T>(string key, T value, DateTime exp)
        {
            using (IRedisClient redis = PooleClient.GetClient())
            {
                if (!redis.Set(key, value, exp))
                {
                    if (redis.ContainsKey(key) && !redis.Remove(key))
                        throw new Exception(string.Format("redis产生脏数据,{0}=>{1}", key, value));
                }
            }
        }

        /// <summary>
        /// 删除key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static void Remove(string key)
        {
            using (IRedisClient redis = PooleClient.GetClient())
            {
                if (redis.ContainsKey(key))
                    if (!redis.Remove(key)) throw new Exception(string.Format("redis移除失败,key={0}", key));
            }
        }

        /// <summary>
        /// 事务回滚删除key(异常打印紧急日志，仅供事务回滚调用)
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static void RollbackRemove(string key)
        {
            using (IRedisClient redis = PooleClient.GetClient())
            {
                if (redis.ContainsKey(key) && !redis.Remove(key))
                    throw new Exception(string.Format("redis移除失败,key={0}", key));
            }
        }

        #endregion

        #region hash
        /// <summary>
        /// 获得某个hash型key下的所有字段
        /// </summary>
        /// <param name="hashId"></param>
        /// <returns></returns>
        public static List<string> GetHashFields(string hashId)
        {
            using (IRedisClient redis = PooleClient.GetClient())
            {
                List<string> hashFields = redis.GetHashKeys(hashId);
                return hashFields;
            }
        }

        /// <summary>
        /// 获得某个hash型key下的所有值
        /// </summary>
        /// <param name="hashId"></param>
        /// <returns></returns>
        public static List<string> GetHashValues(string hashId)
        {
            using (IRedisClient redis = PooleClient.GetClient())
            {
                List<string> hashValues = redis.GetHashValues(hashId);
                return hashValues;
            }
        }

        /// <summary>
        /// 获得hash型key某个字段的值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="field"></param>
        public static string GetHashField(string key, string field)
        {
            using (IRedisClient redis = PooleClient.GetClient())
            {
                string value = redis.GetValueFromHash(key, field);
                return value;
            }
        }

        /// <summary>
        /// 设置hash型key某个字段的值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="field"></param>
        /// <param name="value"></param>
        public static void SetHashField(string key, string field, string value)
        {
            using (IRedisClient redis = PooleClient.GetClient())
            {
                redis.SetEntryInHash(key, field, value);
            }
        }

        /// <summary>
        ///使某个字段增加
        /// </summary>
        /// <param name="key"></param>
        /// <param name="field"></param>
        /// <param name="incre"></param>
        /// <returns></returns>
        public static void SetHashIncr(string key, string field, int incre)
        {
            using (IRedisClient redis = PooleClient.GetClient())
            {
                redis.IncrementValueInHash(key, field, incre);
            }

        }

        #endregion

        #region List

        /// <summary>
        /// 向list类型数据添加成员，向列表底部(右侧)添加
        /// </summary>
        /// <param name="item"></param>
        /// <param name="setId"></param>
        public static void AddItemToListRight(string setId, string item)
        {
            using (IRedisClient redis = PooleClient.GetClient())
            {
                redis.RemoveItemFromList(setId, item);
                redis.AddItemToList(setId, item);
            }
        }

        /// <summary>
        /// 从list类型数据读取所有成员
        /// </summary>
        public static List<string> GetAllItems(string list)
        {
            using (IRedisClient redis = PooleClient.GetClient())
            {
                List<string> listMembers = redis.GetAllItemsFromList(list);
                return listMembers;
            }
        }

        /// <summary>
        /// 从list类型数据指定索引处获取数据，支持正索引和负索引
        /// </summary>
        /// <param name="list"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static string GetItemFromList(string list, int index)
        {
            using (IRedisClient redis = PooleClient.GetClient())
            {
                string item = redis.GetItemFromList(list, index);
                return item;
            }
        }

        /// <summary>
        /// 向列表底部（右侧）批量添加数据
        /// </summary>
        /// <param name="list"></param>
        /// <param name="values"></param>
        public static void GetRangeToList(string list, List<string> values)
        {
            using (IRedisClient redis = PooleClient.GetClient())
            {
                redis.AddRangeToList(list, values);
            }
        }

        #endregion

        #region Set 集合

        /// <summary>
        /// 向集合中添加数据
        /// </summary>
        /// <param name="item"></param>
        /// <param name="set"></param>
        public static void GetItemToSet(string item, string set)
        {
            using (IRedisClient redis = PooleClient.GetClient())
            {
                redis.AddItemToSet(item, set);
            }
        }

        /// <summary>
        /// 获得集合中所有数据
        /// </summary>
        /// <param name="set"></param>
        /// <returns></returns>
        public static HashSet<string> GetAllItemsFromSet(string set)
        {
            using (IRedisClient redis = PooleClient.GetClient())
            {
                HashSet<string> items = redis.GetAllItemsFromSet(set);
                return items;
            }
        }

        /// <summary>
        /// 获取fromSet集合和其他集合不同的数据
        /// </summary>
        /// <param name="fromSet"></param>
        /// <param name="toSet"></param>
        /// <returns></returns>
        public static HashSet<string> GetSetDiff(string fromSet, params string[] toSet)
        {
            using (IRedisClient redis = PooleClient.GetClient())
            {
                HashSet<string> diff = redis.GetDifferencesFromSet(fromSet, toSet);
                return diff;
            }
        }

        /// <summary>
        /// 获得所有集合的并集
        /// </summary>
        /// <param name="set"></param>
        /// <returns></returns>
        public static HashSet<string> GetAllItemsFromSortedSetDescGetSetUnion(params string[] set)
        {
            using (IRedisClient redis = PooleClient.GetClient())
            {
                HashSet<string> union = redis.GetUnionFromSets(set);
                return union;
            }
        }

        /// <summary>
        /// 获得所有集合的交集
        /// </summary>
        /// <param name="set"></param>
        /// <returns></returns>
        public static HashSet<string> GetSetInter(params string[] set)
        {
            using (IRedisClient redis = PooleClient.GetClient())
            {
                HashSet<string> inter = redis.GetIntersectFromSets(set);
                return inter;
            }
        }

        #endregion

        #region sorted set 有序集合

        /// <summary>
        /// 向有序集合中添加元素，若元素已存在，则该元素权重+1
        /// </summary>
        /// <param name="set">search</param>
        /// <param name="value">{\"id\":1,\"type\":1,\"title\":\"标题\",\"des\":\"描述信息\"}</param>
        public static void AddItemToSortedSet(string set, string value)
        {
            using (IRedisClient redis = PooleClient.GetClient())
            {
                if (redis.SortedSetContainsItem(set, value)) IncrementItemInSortedSet(set, value);
                else redis.AddItemToSortedSet(set, value, 1);
            }
        }

        /// <summary>
        /// 向有序集合中添加元素，若元素已存在，则该元素权重+1
        /// </summary>
        /// <param name="set"></param>
        /// <param name="value">{\"id\":1,\"type\":1,\"title\":\"标题\",\"des\":\"描述信息\",\"img\":\"图片路径\",\"stime\":\"开始时间\"}</param>
        /// <param name="score"></param>
        public static void AddItemToSortedSet(string set, string value, double score = 1)
        {
            using (IRedisClient redis = PooleClient.GetClient())
            {
                if (redis.SortedSetContainsItem(set, value))
                {
                    score = redis.GetItemScoreInSortedSet(set, value) + 1;
                    redis.RemoveItemFromSortedSet(set, value);//如果存在先移除
                }
                redis.AddItemToSortedSet(set, value, score);
            }
        }

        /// <summary>
        /// 对集合中指定元素的权重+1（线程安全，容易假死，待解决）
        /// </summary>
        /// <param name="set"></param>
        /// <param name="value"></param>
        public static void IncrementItemInSortedSet(string set, string value)
        {
            using (IRedisClient redis = PooleClient.GetClient())
            {
                redis.IncrementItemInSortedSet(set, value, 1);
            }
        }

        /// <summary>
        /// 在有序集合中删除元素
        /// </summary>
        /// <param name="set"></param>
        /// <param name="value"></param>
        public static void RemoveItemFromSortedSet(string set, string value)
        {
            using (IRedisClient redis = PooleClient.GetClient())
            {
                if (redis.SortedSetContainsItem(set, value)) redis.RemoveItemFromSortedSet(set, value);
            }
        }

        /// <summary>
        /// 在有序集合中修改元素
        /// </summary>
        /// <param name="set"></param>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        public static void UpdateItemFromSortedSet(string set, string oldValue, string newValue)
        {
            if (oldValue != newValue)
            {
                RemoveItemFromSortedSet(set, oldValue);
                AddItemToSortedSet(set, newValue);
            }
        }

        public static List<string> GetAllItemsFromSortedSet(string set)
        {
            using (IRedisClient redis = PooleClient.GetClient())
            {
                return redis.GetAllItemsFromSortedSet(set);
            }
        }

        /// <summary>
        /// 降序查询有序集合中包含指定关键字的序列
        /// </summary>
        /// <param name="set"></param>
        /// <param name="key"></param>
        /// <param name="take"></param>
        public static List<string> GetAllItemsFromSortedSetDescOld(string set, string key, int take)
        {
            key = key.ToLower();
            using (IRedisClient redis = PooleClient.GetClient())
            {
                return redis.GetAllItemsFromSortedSetDesc(set).Where(c => c.ToLower().Contains(key)).Take(take).ToList();
            }
        }

        /// <summary>
        /// 获得某个值在有序集合中的排名，按分数的降序排列
        /// </summary>
        /// <param name="set"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int GetItemIndexInSortedSetDesc(string set, string value)
        {
            using (IRedisClient redis = PooleClient.GetClient())
            {
                var index = (int)redis.GetItemIndexInSortedSetDesc(set, value);
                return index;
            }
        }

        /// <summary>
        /// 获得某个值在有序集合中的排名，按分数的升序排列
        /// </summary>
        /// <param name="set"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int GetItemIndexInSortedSet(string set, string value)
        {
            using (IRedisClient redis = PooleClient.GetClient())
            {
                int index = (int)redis.GetItemIndexInSortedSet(set, value);
                return index;
            }
        }

        /// <summary>
        /// 获得有序集合中某个值得分数
        /// </summary>
        /// <param name="set"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static double GetItemScoreInSortedSet(string set, string value)
        {
            using (IRedisClient redis = PooleClient.GetClient())
            {
                double score = redis.GetItemScoreInSortedSet(set, value);
                return score;
            }
        }

        /// <summary>
        /// 获得有序集合中，某个排名范围的所有值
        /// </summary>
        /// <param name="set"></param>
        /// <param name="beginRank"></param>
        /// <param name="endRank"></param>
        /// <returns></returns>
        public static List<string> GetRangeFromSortedSet(string set, int beginRank, int endRank)
        {
            using (IRedisClient redis = PooleClient.GetClient())
            {
                List<string> valueList = redis.GetRangeFromSortedSet(set, beginRank, endRank);
                return valueList;
            }
        }

        /// <summary>
        /// 获得有序集合中，某个分数范围内的所有值，升序
        /// </summary>
        /// <param name="set"></param>
        /// <param name="beginScore"></param>
        /// <param name="endScore"></param>
        /// <returns></returns>
        public static List<string> GetRangeFromSortedSet(string set, double beginScore, double endScore)
        {
            using (IRedisClient redis = PooleClient.GetClient())
            {
                List<string> valueList = redis.GetRangeFromSortedSetByHighestScore(set, beginScore, endScore);
                return valueList;
            }
        }

        /// <summary>
        /// 获得有序集合中，某个分数范围内的所有值，降序
        /// </summary>
        /// <param name="set"></param>
        /// <param name="beginScore"></param>
        /// <param name="endScore"></param>
        /// <returns></returns>
        public static List<string> GetRangeFromSortedSetDesc(string set, double beginScore, double endScore)
        {
            using (IRedisClient redis = PooleClient.GetClient())
            {
                List<string> vlaueList = redis.GetRangeFromSortedSetByLowestScore(set, beginScore, endScore);
                return vlaueList;
            }
        }

        #endregion

        public void Dispose()
        {
            using (IRedisClient redis = PooleClient.GetClient())
            {
                redis.Dispose();
            }
        }
    }

    /// <summary>
    /// 配置文件
    /// </summary>
    public class ConfigHelper
    {
        /// <summary>
        /// 读写服务器
        /// </summary>
        public static string ReadWriteHosts
        {
            get
            {
                return "127.0.0.1:6379";
                //<add key="ReadWriteHosts" value="127.0.0.1:6379" />
            }
        }

        /// <summary>
        /// 只读服务器
        /// </summary>
        /// <returns></returns>
        public static string ReadOnlyHosts
        {
            get
            {
                return "127.0.0.1:6379";
                //<add key="ReadOnlyHosts" value="127.0.0.1:6379" />
            }
        }

        /// <summary>
        /// 服务项目验证GUID 的过期时间以秒为单位
        /// </summary>
        public static int GuidPastTime
        {
            get
            {
                return Convert.ToInt32("5000");
                //<add key="GuidPastTime" value="5000" />
            }
        }
    }
}
