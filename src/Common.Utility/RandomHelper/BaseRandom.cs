 

using System;

namespace Common.Utility
{
    /// <summary>
    /// BaseRandom
    /// 产生随机数
    /// 
    /// 随机数管理，最大值、最小值可以自己进行设定。
    /// </summary>
    public class BaseRandom
    {
        private  int Minimum { set; get; }
        private  int Maximal { set; get; }
        private  int RandomLength { set; get; }

        private string RandomString { set; get; } = "0123456789ABCDEFGHIJKMLNOPQRSTUVWXYZ";
        private readonly Random Random = new Random(DateTime.Now.Second);

        #region constructor

        BaseRandom()
        {
            Minimum = 1;
            Maximal = 999;
            RandomLength = 6;
        }

        BaseRandom(int Max , int Min)
        {
            Minimum = Min;
            Maximal = Max;
        }

        BaseRandom(int Length)
        {
            RandomLength = Length;
        }

        #endregion


        #region public string GetRandomString() 产生随机字符
        /// <summary>
        /// 产生随机字符
        /// </summary>
        /// <returns>字符串</returns>
        public string GetRandomString()
        {
            return GetRandomString(RandomLength);
        }

        public  string GetRandomString(int stringLength)
        {
			string returnValue = string.Empty;
			for (int i = 0; i < stringLength; i++)
			{
				int r = Random.Next(0, RandomString.Length - 1);
				returnValue += RandomString[r];
			}
			return returnValue;

		}

        #endregion

        #region public static int GetRandom()
        /// <summary>
        /// 产生随机数
        /// </summary>
        /// <returns>随机数</returns>
        public int GetRandom()
        {
            return Random.Next(Minimum, Maximal);
        }
		#endregion

		#region public static int GetRandom(int minimum, int maximal)
		/// <summary>
		/// 产生随机数
		/// </summary>
		/// <param name="minNumber">最小值</param>
		/// <param name="maxNumber">最大值</param>
		/// <returns>随机数</returns>
		public int GetRandom(int minNumber, int maxNumber)
        {
            return Random.Next(minNumber, maxNumber);
        }
        #endregion
    }
}