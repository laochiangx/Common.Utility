
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Common.Utilities
{
    public static class DecimalUtility
    {
        public static Decimal? ParseToDecimalValue(object decimalObj)
        {
            if (decimalObj == null) return null;
            Decimal decValue;
            if (!Decimal.TryParse(decimalObj.ToString(), out decValue)) return null;
            return decValue;
        }

        /// <summary>
        /// 转中文大写数字
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ConvertNumToZHUpperCase(decimal value)
        {
            string[] numList = { "零", "壹", "贰", "叁", "肆", "伍", "陆", "柒", "捌", "玖" };
            string[] unitList = { "分", "角", "元", "拾", "佰", "仟", "万", "拾", "佰", "仟", "亿", "拾", "佰", "仟" };

            decimal money = value;
            if (money == 0)
            {
                return "零元整";
            }

            StringBuilder strMoney = new StringBuilder();
            //只取小数后2位



            string strNum = decimal.Truncate(money * 100).ToString();

            int len = strNum.Length;
            int zero = 0;
            for (int i = 0; i < len; i++)
            {
                int num = int.Parse(strNum.Substring(i, 1));
                int unitNum = len - i - 1;

                if (num == 0)
                {
                    zero++;
                    if (unitNum == 2 || unitNum == 6 || unitNum == 10)
                    {
                        if (unitNum == 2 || zero < 4)
                            strMoney.Append(unitList[unitNum]);
                        zero = 0;
                    }
                }
                else
                {

                    if (zero > 0)
                    {
                        strMoney.Append(numList[0]);
                        zero = 0;
                    }
                    strMoney.Append(numList[num]);
                    strMoney.Append(unitList[unitNum]);
                }

            }
            if (zero > 0)
                strMoney.Append("整");

            return strMoney.ToString();
        }

        /// <summary>
        /// 截取指定位数
        /// </summary>
        /// <param name="d"></param>
        /// <param name="s"></param>
        /// <returns></returns>
        public static decimal ToFixed(decimal d, int s)
        {
            decimal sp = Convert.ToDecimal(Math.Pow(10, s));
            return Math.Truncate(d) + Math.Floor((d - Math.Truncate(d)) * sp) / sp;
        }

        /// <summary>
        ///  截取指定位数
        /// </summary>
        /// <param name="d"></param>
        /// <param name="s"></param>
        /// <returns></returns>
        public static double ToFixed(double d, int s)
        {
            double sp = Math.Pow(10, s);
            return Math.Truncate(d) + Math.Floor((d - Math.Truncate(d)) * sp) / sp;
        }
    }
}