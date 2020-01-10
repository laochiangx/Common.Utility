using Common.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Utilities
{
    public class DefaultWeight<T> where T : WeightObject
    {

        /// <summary>
        /// 参考:http://www.cnblogs.com/foolin/archive/2012/03/22/2412632.html
        /// 算法：
        /// 1.每个广告项权重+1命名为w，防止为0情况。
        /// 2.计算出总权重n。
        /// 3.每个广告项权重w加上从0到(n-1)的一个随机数（即总权重以内的随机数），得到新的权重排序值s。
        /// 4.根据得到新的权重排序值s进行排序，取前面s最大几个。
        /// </summary>
        /// <param name="list">原始列表</param>
        /// <param name="count">随机抽取条数</param>
        /// <returns></returns>


        private List<T> _weightObjects;



        public DefaultWeight(List<T> weightObjects)
        {
            this._weightObjects = weightObjects;
        }


        /// <summary>
        /// 重置权重列表值，
        /// 当列表的一些状态发生改变后
        /// </summary>
        public void ResetWeightList(List<T> weightObjects)
        {
            this._weightObjects = weightObjects;
        }


        public T GetWeight()
        {
            return GetWeightList(1).FirstOrDefault();
        }

        public List<T> GetWeightList(int count)
        {
            if (_weightObjects == null || _weightObjects.Count <= count || count <= 0)
            {
                return _weightObjects;
            }

            //计算权重总和
            int totalWeights = 0;
            for (int i = 0; i < _weightObjects.Count; i++)
            {
                totalWeights += _weightObjects[i].Weight + 1;  //权重+1，防止为0情况。
            }

            //随机赋值权重
            Random ran = new Random(RandomHelper.GetRandomSeed());  //GetRandomSeed()随机种子，防止快速频繁调用导致随机一样的问题 
            List<KeyValuePair<int, int>> wlist = new List<KeyValuePair<int, int>>();    //第一个int为list下标索引、第一个int为权重排序值
            for (int i = 0; i < _weightObjects.Count; i++)
            {
                int w = (_weightObjects[i].Weight + 1) + ran.Next(0, totalWeights);   // （权重+1） + 从0到（总权重-1）的随机数
                wlist.Add(new KeyValuePair<int, int>(i, w));
            }

            //排序
            wlist.Sort(
              delegate (KeyValuePair<int, int> kvp1, KeyValuePair<int, int> kvp2)
              {
                  return kvp2.Value - kvp1.Value;
              });

            //根据实际情况取排在最前面的几个
            List<T> result = new List<T>();
            for (int i = 0; i < count; i++)
            {
                T entiy = _weightObjects[wlist[i].Key];
                result.Add(entiy);
            }

            //随机法则
            return result;
        }
    }
}
