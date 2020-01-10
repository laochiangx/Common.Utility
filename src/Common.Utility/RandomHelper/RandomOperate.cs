 
using System;
using System.Linq;

namespace Common.Utility
{
    public class RandomOperate
    {
        static readonly Random Random = new Random(~unchecked((int)DateTime.Now.Ticks));

        /// <summary>
        /// Generates the check code with unique number.
        /// </summary>
        /// <returns>The check code number.</returns>
        /// <param name="codeCount">Code count. Max 10</param>
        public string GenerateCheckCodeNum(int codeCount)
        {
            codeCount = codeCount > 10 ? 10 :codeCount;   // unable to return unique number list longer than 10

            int[] arrInt = {0,1,2,3,4,5,6,7,8,9};
		    arrInt = arrInt.OrderBy(c => Guid.NewGuid()).ToArray<int>();// make the array in random order

            string str = string.Empty;

            for (int i = 0; i < codeCount; i++)
            {
                str += arrInt[i];
            }
            return str;
        }

		//方法二：随机生成字符串（数字和字母混和）
		/// <summary>
		/// Generates the check code with number and char
		/// </summary>
		/// <returns>The check code.</returns>
		/// <param name="CodeCount">Code lenght.</param>
		public string GenerateCheckCode(int CodeCount)
        {
            char[] MixedList = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I','J', 'K', 'L', 'M', 'N','O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' }; //remove I & O
			return GetRandomCode(MixedList, CodeCount);
		}

        #region
       /// <summary>
       /// Gets the random code.
       /// </summary>
       /// <returns>The random code.</returns>
       /// <param name="CharList">All char want to generate.</param>
       /// <param name="CodeLength">Code lenght.</param>
        private string GetRandomCode(char[] CharList, int CodeLength)
        {
			string result = string.Empty;
			for (int i = 0; i < CodeLength; i++)
			{
				int rnd = Random.Next(0, CharList.Length);
				result += CharList[rnd];
			}
			return result;
        }

        #endregion
    }
}
