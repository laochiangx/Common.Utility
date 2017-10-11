using System;

namespace DotNet.Utilities
{
    /// <summary>
    /// 农历属性
    /// </summary>
    public class CNDate
    {
        /// <summary>
        /// 农历年(整型)
        /// </summary>
        public int cnIntYear = 0;
        /// <summary>
        /// 农历月份(整型)
        /// </summary>
        public int cnIntMonth = 0;
        /// <summary>
        /// 农历天(整型)
        /// </summary>
        public int cnIntDay = 0;
        /// <summary>
        /// 农历年(支干)
        /// </summary>
        public string cnStrYear = "";
        /// <summary>
        /// 农历月份(字符)
        /// </summary>
        public string cnStrMonth = "";
        /// <summary>
        /// 农历天(字符)
        /// </summary>
        public string cnStrDay = "";
        /// <summary>
        /// 农历属象
        /// </summary>
        public string cnAnm = "";
        /// <summary>
        /// 二十四节气
        /// </summary>
        public string cnSolarTerm = "";
        /// <summary>
        /// 阴历节日
        /// </summary>
        public string cnFtvl = "";
        /// <summary>
        /// 阳历节日
        /// </summary>
        public string cnFtvs = "";
    }

    /// <summary>
    /// 公历转农历
    /// </summary>
    public class ChinaDate
    {
        #region 私有方法
        private static long[] lunarInfo = new long[] { 0x04bd8, 0x04ae0, 0x0a570, 0x054d5, 0x0d260, 0x0d950, 0x16554,
															   0x056a0, 0x09ad0, 0x055d2, 0x04ae0, 0x0a5b6, 0x0a4d0, 0x0d250, 0x1d255, 0x0b540, 0x0d6a0, 0x0ada2, 0x095b0,
															   0x14977, 0x04970, 0x0a4b0, 0x0b4b5, 0x06a50, 0x06d40, 0x1ab54, 0x02b60, 0x09570, 0x052f2, 0x04970, 0x06566,
															   0x0d4a0, 0x0ea50, 0x06e95, 0x05ad0, 0x02b60, 0x186e3, 0x092e0, 0x1c8d7, 0x0c950, 0x0d4a0, 0x1d8a6, 0x0b550,
															   0x056a0, 0x1a5b4, 0x025d0, 0x092d0, 0x0d2b2, 0x0a950, 0x0b557, 0x06ca0, 0x0b550, 0x15355, 0x04da0, 0x0a5d0,
															   0x14573, 0x052d0, 0x0a9a8, 0x0e950, 0x06aa0, 0x0aea6, 0x0ab50, 0x04b60, 0x0aae4, 0x0a570, 0x05260, 0x0f263,
															   0x0d950, 0x05b57, 0x056a0, 0x096d0, 0x04dd5, 0x04ad0, 0x0a4d0, 0x0d4d4, 0x0d250, 0x0d558, 0x0b540, 0x0b5a0,
															   0x195a6, 0x095b0, 0x049b0, 0x0a974, 0x0a4b0, 0x0b27a, 0x06a50, 0x06d40, 0x0af46, 0x0ab60, 0x09570, 0x04af5,
															   0x04970, 0x064b0, 0x074a3, 0x0ea50, 0x06b58, 0x055c0, 0x0ab60, 0x096d5, 0x092e0, 0x0c960, 0x0d954, 0x0d4a0,
															   0x0da50, 0x07552, 0x056a0, 0x0abb7, 0x025d0, 0x092d0, 0x0cab5, 0x0a950, 0x0b4a0, 0x0baa4, 0x0ad50, 0x055d9,
															   0x04ba0, 0x0a5b0, 0x15176, 0x052b0, 0x0a930, 0x07954, 0x06aa0, 0x0ad50, 0x05b52, 0x04b60, 0x0a6e6, 0x0a4e0,
															   0x0d260, 0x0ea65, 0x0d530, 0x05aa0, 0x076a3, 0x096d0, 0x04bd7, 0x04ad0, 0x0a4d0, 0x1d0b6, 0x0d250, 0x0d520,
															   0x0dd45, 0x0b5a0, 0x056d0, 0x055b2, 0x049b0, 0x0a577, 0x0a4b0, 0x0aa50, 0x1b255, 0x06d20, 0x0ada0 };

        private static int[] year20 = new int[] { 1, 4, 1, 2, 1, 2, 1, 1, 2, 1, 2, 1 };
        private static int[] year19 = new int[] { 0, 3, 0, 1, 0, 1, 0, 0, 1, 0, 1, 0 };
        private static int[] year2000 = new int[] { 0, 3, 1, 2, 1, 2, 1, 1, 2, 1, 2, 1 };
        private static String[] nStr1 = new String[] { "", "正", "二", "三", "四", "五", "六", "七", "八", "九", "十", "十一", "十二" };
        private static String[] Gan = new String[] { "甲", "乙", "丙", "丁", "戊", "己", "庚", "辛", "壬", "癸" };
        private static String[] Zhi = new String[] { "子", "丑", "寅", "卯", "辰", "巳", "午", "未", "申", "酉", "戌", "亥" };
        private static String[] Animals = new String[] { "鼠", "牛", "虎", "兔", "龙", "蛇", "马", "羊", "猴", "鸡", "狗", "猪" };
        private static String[] solarTerm = new String[] { "小寒", "大寒", "立春", "雨水", "惊蛰", "春分", "清明", "谷雨", "立夏", "小满", "芒种", "夏至", "小暑", "大暑", "立秋", "处暑", "白露", "秋分", "寒露", "霜降", "立冬", "小雪", "大雪", "冬至" };
        private static int[] sTermInfo = { 0, 21208, 42467, 63836, 85337, 107014, 128867, 150921, 173149, 195551, 218072, 240693, 263343, 285989, 308563, 331033, 353350, 375494, 397447, 419210, 440795, 462224, 483532, 504758 };
        private static String[] lFtv = new String[] { "0101农历春节", "0202 龙抬头节", "0115 元宵节", "0505 端午节", "0707 七夕情人节", "0815 中秋节", "0909 重阳节", "1208 腊八节", "1114 李君先生生日", "1224 小年", "0100除夕" };
        private static String[] sFtv = new String[] { "0101 新年元旦",
														 "0202 世界湿地日",
														 "0207 国际声援南非日",
														 "0210 国际气象节",
														 "0214 情人节",
														 "0301 国际海豹日",
														 "0303 全国爱耳日",
														 "0308 国际妇女节",
														 "0312 植树节 孙中山逝世纪念日",
														 "0314 国际警察日",
														 "0315 国际消费者权益日",
														 "0317 中国国医节 国际航海日",
														 "0321 世界森林日 消除种族歧视国际日",
														 "0321 世界儿歌日",
														 "0322 世界水日",
														 "0323 世界气象日",
														 "0324 世界防治结核病日",
														 "0325 全国中小学生安全教育日",
														 "0330 巴勒斯坦国土日",
														 "0401 愚人节 全国爱国卫生运动月(四月) 税收宣传月(四月)",
														 "0407 世界卫生日",
														 "0422 世界地球日",
														 "0423 世界图书和版权日",
														 "0424 亚非新闻工作者日",
														 "0501 国际劳动节",
														 "0504 中国五四青年节",
														 "0505 碘缺乏病防治日",
														 "0508 世界红十字日",
														 "0512 国际护士节",
														 "0515 国际家庭日",
														 "0517 世界电信日",
														 "0518 国际博物馆日",
														 "0520 全国学生营养日",
														 "0523 国际牛奶日",
														 "0531 世界无烟日",
														 "0601 国际儿童节",
														 "0605 世界环境日",
														 "0606 全国爱眼日",
														 "0617 防治荒漠化和干旱日",
														 "0623 国际奥林匹克日",
														 "0625 全国土地日",
														 "0626 国际反毒品日",
														 "0701 中国共产党建党日 世界建筑日",
														 "0702 国际体育记者日",
														 "0707 中国人民抗日战争纪念日",
														 "0711 世界人口日",
														 "0730 非洲妇女日",
														 "0801 中国建军节",
														 "0808 中国男子节(爸爸节)",
														 "0815 日本正式宣布无条件投降日",
														 "0908 国际扫盲日 国际新闻工作者日",
														 "0910 教师节",
														 "0914 世界清洁地球日",
														 "0916 国际臭氧层保护日",
														 "0918 九·一八事变纪念日",
														 "0920 全国爱牙日",
														 "0927 世界旅游日",
														 "1001 国庆节 世界音乐日 国际老人节",
														 "1001 国际音乐日",
														 "1002 国际和平与民主自由斗争日",
														 "1004 世界动物日",
														 "1008 全国高血压日",
														 "1008 世界视觉日",
														 "1009 世界邮政日 万国邮联日",
														 "1010 辛亥革命纪念日 世界精神卫生日",
														 "1013 世界保健日 国际教师节",
														 "1014 世界标准日",
														 "1015 国际盲人节(白手杖节)",
														 "1016 世界粮食日",
														 "1017 世界消除贫困日",
														 "1022 世界传统医药日",
														 "1024 联合国日 世界发展信息日",
														 "1031 世界勤俭日",
														 "1107 十月社会主义革命纪念日",
														 "1108 中国记者日",
														 "1109 全国消防安全宣传教育日",
														 "1110 世界青年节",
														 "1111 国际科学与和平周(本日所属的一周)",
														 "1112 孙中山诞辰纪念日",
														 "1114 世界糖尿病日",
														 "1117 国际大学生节 世界学生节",
														 "1121 世界问候日 世界电视日",
														 "1129 国际声援巴勒斯坦人民国际日",
														 "1201 世界艾滋病日",
														 "1203 世界残疾人日",
														 "1205 国际经济和社会发展志愿人员日",
														 "1208 国际儿童电视日",
														 "1209 世界足球日",
														 "1210 世界人权日",
														 "1212 西安事变纪念日",
														 "1213 南京大屠杀(1937年)纪念日！紧记血泪史！",
														 "1221 国际篮球日",
														 "1224 平安夜",
														 "1225 圣诞节",
														 "1226 毛主席诞辰",
														 "1229 国际生物多样性日" };


        /// <summary>
        /// 传回农历y年的总天数
        /// </summary>
        private static int lYearDays(int y)
        {
            int i, sum = 348;
            for (i = 0x8000; i > 0x8; i >>= 1)
            {
                if ((lunarInfo[y - 1900] & i) != 0)
                    sum += 1;
            }
            return (sum + leapDays(y));
        }

        /// <summary>
        /// 传回农历y年闰月的天数
        /// </summary>
        private static int leapDays(int y)
        {
            if (leapMonth(y) != 0)
            {
                if ((lunarInfo[y - 1900] & 0x10000) != 0)
                    return 30;
                else
                    return 29;
            }
            else
                return 0;
        }

        /// <summary>
        /// 传回农历y年闰哪个月 1-12 , 没闰传回 0
        /// </summary>
        private static int leapMonth(int y)
        {
            return (int)(lunarInfo[y - 1900] & 0xf);
        }

        /// <summary>
        /// 传回农历y年m月的总天数
        /// </summary>
        private static int monthDays(int y, int m)
        {
            if ((lunarInfo[y - 1900] & (0x10000 >> m)) == 0)
                return 29;
            else
                return 30;
        }

        /// <summary>
        /// 传回农历y年的生肖
        /// </summary>
        private static String AnimalsYear(int y)
        {
            return Animals[(y - 4) % 12];
        }

        /// <summary>
        /// 传入月日的offset 传回干支,0=甲子
        /// </summary>
        private static String cyclicalm(int num)
        {
            return (Gan[num % 10] + Zhi[num % 12]);
        }

        /// <summary>
        /// 传入offset 传回干支, 0=甲子
        /// </summary>
        private static String cyclical(int y)
        {
            int num = y - 1900 + 36;
            return (cyclicalm(num));
        }

        /// <summary>
        /// 传出农历.year0 .month1 .day2 .yearCyl3 .monCyl4 .dayCyl5 .isLeap6
        /// </summary>
        private long[] Lunar(int y, int m)
        {
            long[] nongDate = new long[7];
            int i = 0, temp = 0, leap = 0;
            DateTime baseDate = new DateTime(1900 + 1900, 2, 31);
            DateTime objDate = new DateTime(y + 1900, m + 1, 1);
            TimeSpan ts = objDate - baseDate;
            long offset = (long)ts.TotalDays;
            if (y < 2000)
                offset += year19[m - 1];
            if (y > 2000)
                offset += year20[m - 1];
            if (y == 2000)
                offset += year2000[m - 1];
            nongDate[5] = offset + 40;
            nongDate[4] = 14;

            for (i = 1900; i < 2050 && offset > 0; i++)
            {
                temp = lYearDays(i);
                offset -= temp;
                nongDate[4] += 12;
            }
            if (offset < 0)
            {
                offset += temp;
                i--;
                nongDate[4] -= 12;
            }
            nongDate[0] = i;
            nongDate[3] = i - 1864;
            leap = leapMonth(i); // 闰哪个月
            nongDate[6] = 0;

            for (i = 1; i < 13 && offset > 0; i++)
            {
                // 闰月
                if (leap > 0 && i == (leap + 1) && nongDate[6] == 0)
                {
                    --i;
                    nongDate[6] = 1;
                    temp = leapDays((int)nongDate[0]);
                }
                else
                {
                    temp = monthDays((int)nongDate[0], i);
                }

                // 解除闰月
                if (nongDate[6] == 1 && i == (leap + 1))
                    nongDate[6] = 0;
                offset -= temp;
                if (nongDate[6] == 0)
                    nongDate[4]++;
            }

            if (offset == 0 && leap > 0 && i == leap + 1)
            {
                if (nongDate[6] == 1)
                {
                    nongDate[6] = 0;
                }
                else
                {
                    nongDate[6] = 1;
                    --i;
                    --nongDate[4];
                }
            }
            if (offset < 0)
            {
                offset += temp;
                --i;
                --nongDate[4];
            }
            nongDate[1] = i;
            nongDate[2] = offset + 1;
            return nongDate;
        }

        /// <summary>
        /// 传出y年m月d日对应的农历.year0 .month1 .day2 .yearCyl3 .monCyl4 .dayCyl5 .isLeap6
        /// </summary>
        private static long[] calElement(int y, int m, int d)
        {
            long[] nongDate = new long[7];
            int i = 0, temp = 0, leap = 0;

            DateTime baseDate = new DateTime(1900, 1, 31);

            DateTime objDate = new DateTime(y, m, d);
            TimeSpan ts = objDate - baseDate;

            long offset = (long)ts.TotalDays;

            nongDate[5] = offset + 40;
            nongDate[4] = 14;

            for (i = 1900; i < 2050 && offset > 0; i++)
            {
                temp = lYearDays(i);
                offset -= temp;
                nongDate[4] += 12;
            }
            if (offset < 0)
            {
                offset += temp;
                i--;
                nongDate[4] -= 12;
            }
            nongDate[0] = i;
            nongDate[3] = i - 1864;
            leap = leapMonth(i); // 闰哪个月
            nongDate[6] = 0;

            for (i = 1; i < 13 && offset > 0; i++)
            {
                // 闰月
                if (leap > 0 && i == (leap + 1) && nongDate[6] == 0)
                {
                    --i;
                    nongDate[6] = 1;
                    temp = leapDays((int)nongDate[0]);
                }
                else
                {
                    temp = monthDays((int)nongDate[0], i);
                }

                // 解除闰月
                if (nongDate[6] == 1 && i == (leap + 1))
                    nongDate[6] = 0;
                offset -= temp;
                if (nongDate[6] == 0)
                    nongDate[4]++;
            }

            if (offset == 0 && leap > 0 && i == leap + 1)
            {
                if (nongDate[6] == 1)
                {
                    nongDate[6] = 0;
                }
                else
                {
                    nongDate[6] = 1;
                    --i;
                    --nongDate[4];
                }
            }
            if (offset < 0)
            {
                offset += temp;
                --i;
                --nongDate[4];
            }
            nongDate[1] = i;
            nongDate[2] = offset + 1;
            return nongDate;
        }

        private static String getChinaDate(int day)
        {
            String a = "";
            if (day == 10)
                return "初十";
            if (day == 20)
                return "二十";
            if (day == 30)
                return "三十";
            int two = (int)((day) / 10);
            if (two == 0)
                a = "初";
            if (two == 1)
                a = "十";
            if (two == 2)
                a = "廿";
            if (two == 3)
                a = "三";
            int one = (int)(day % 10);
            switch (one)
            {
                case 1:
                    a += "一";
                    break;
                case 2:
                    a += "二";
                    break;
                case 3:
                    a += "三";
                    break;
                case 4:
                    a += "四";
                    break;
                case 5:
                    a += "五";
                    break;
                case 6:
                    a += "六";
                    break;
                case 7:
                    a += "七";
                    break;
                case 8:
                    a += "八";
                    break;
                case 9:
                    a += "九";
                    break;
            }
            return a;
        }

        private static DateTime sTerm(int y, int n)
        {
            double ms = 31556925974.7 * (y - 1900);
            double ms1 = sTermInfo[n];
            DateTime offDate = new DateTime(1900, 1, 6, 2, 5, 0);
            offDate = offDate.AddMilliseconds(ms);
            offDate = offDate.AddMinutes(ms1);
            return offDate;
        }

        static string FormatDate(int m, int d)
        {
            return string.Format("{0:00}{1:00}", m, d);
        }
        #endregion

        #region 公有方法
        /// <summary>
        /// 传回公历y年m月的总天数
        /// </summary>
        public static int GetDaysByMonth(int y, int m)
        {
            int[] days = new int[] { 31, DateTime.IsLeapYear(y) ? 29 : 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };
            return days[m - 1];
        }

        /// <summary>
        /// 根据日期值获得周一的日期
        /// </summary>
        /// <param name="dt">输入日期</param>
        /// <returns>周一的日期</returns>
        public static DateTime GetMondayDateByDate(DateTime dt)
        {
            double d = 0;
            switch ((int)dt.DayOfWeek)
            {
                //case 1: d = 0; break;
                case 2: d = -1; break;
                case 3: d = -2; break;
                case 4: d = -3; break;
                case 5: d = -4; break;
                case 6: d = -5; break;
                case 0: d = -6; break;
            }
            return dt.AddDays(d);
        }

        /// <summary>
        /// 获取农历
        /// </summary>
        public static CNDate getChinaDate(DateTime dt)
        {
            CNDate cd = new CNDate();
            int year = dt.Year;
            int month = dt.Month;
            int date = dt.Day;
            long[] l = calElement(year, month, date);
            cd.cnIntYear = (int)l[0];
            cd.cnIntMonth = (int)l[1];
            cd.cnIntDay = (int)l[2];
            cd.cnStrYear = cyclical(year);
            cd.cnAnm = AnimalsYear(year);
            cd.cnStrMonth = nStr1[(int)l[1]];
            cd.cnStrDay = getChinaDate((int)(l[2]));
            string smd = dt.ToString("MMdd");

            string lmd = FormatDate(cd.cnIntMonth, cd.cnIntDay);
            for (int i = 0; i < solarTerm.Length; i++)
            {
                string s1 = sTerm(dt.Year, i).ToString("MMdd");
                if (s1.Equals(dt.ToString("MMdd")))
                {
                    cd.cnSolarTerm = solarTerm[i];
                    break;
                }
            }
            foreach (string s in sFtv)
            {
                string s1 = s.Substring(0, 4);
                if (s1.Equals(smd))
                {
                    cd.cnFtvs = s.Substring(4, s.Length - 4);
                    break;
                }
            }
            foreach (string s in lFtv)
            {
                string s1 = s.Substring(0, 4);
                if (s1.Equals(lmd))
                {
                    cd.cnFtvl = s.Substring(4, s.Length - 4);
                    break;
                }
            }
            dt = dt.AddDays(1);
            year = dt.Year;
            month = dt.Month;
            date = dt.Day;
            l = calElement(year, month, date);
            lmd = FormatDate((int)l[1], (int)l[2]);
            if (lmd.Equals("0101")) cd.cnFtvl = "除夕";
            return cd;
        }
        #endregion
    }

    /// <summary>
    /// 中国日历
    /// </summary>
    //-------------------------------------------------------------------------------
    //调用:
    //ChineseCalendar c = new ChineseCalendar(new DateTime(1990, 01, 15));
    //StringBuilder dayInfo = new StringBuilder();
    //dayInfo.Append("阳历：" + c.DateString + "\r\n");
    //dayInfo.Append("农历：" + c.ChineseDateString + "\r\n");
    //dayInfo.Append("星期：" + c.WeekDayStr);
    //dayInfo.Append("时辰：" + c.ChineseHour + "\r\n");
    //dayInfo.Append("属相：" + c.AnimalString + "\r\n");
    //dayInfo.Append("节气：" + c.ChineseTwentyFourDay + "\r\n");
    //dayInfo.Append("前一个节气：" + c.ChineseTwentyFourPrevDay + "\r\n");
    //dayInfo.Append("下一个节气：" + c.ChineseTwentyFourNextDay + "\r\n");
    //dayInfo.Append("节日：" + c.DateHoliday + "\r\n");
    //dayInfo.Append("干支：" + c.GanZhiDateString + "\r\n");
    //dayInfo.Append("星宿：" + c.ChineseConstellation + "\r\n");
    //dayInfo.Append("星座：" + c.Constellation + "\r\n");
    //-------------------------------------------------------------------------------
    public class ChineseCalendar
    {
        #region 内部结构
        /// <summary>
        /// 阳历
        /// </summary>
        private struct SolarHolidayStruct
        {
            public int Month;
            public int Day;
            public int Recess; //假期长度
            public string HolidayName;
            public SolarHolidayStruct(int month, int day, int recess, string name)
            {
                Month = month;
                Day = day;
                Recess = recess;
                HolidayName = name;
            }
        }

        /// <summary>
        /// 农历
        /// </summary>
        private struct LunarHolidayStruct
        {
            public int Month;
            public int Day;
            public int Recess;
            public string HolidayName;

            public LunarHolidayStruct(int month, int day, int recess, string name)
            {
                Month = month;
                Day = day;
                Recess = recess;
                HolidayName = name;
            }
        }

        private struct WeekHolidayStruct
        {
            public int Month;
            public int WeekAtMonth;
            public int WeekDay;
            public string HolidayName;

            public WeekHolidayStruct(int month, int weekAtMonth, int weekDay, string name)
            {
                Month = month;
                WeekAtMonth = weekAtMonth;
                WeekDay = weekDay;
                HolidayName = name;
            }
        }
        #endregion

        #region 内部变量
        private DateTime _date;
        private DateTime _datetime;
        private int _cYear;
        private int _cMonth;
        private int _cDay;
        private bool _cIsLeapMonth; //当月是否闰月
        private bool _cIsLeapYear;  //当年是否有闰月
        #endregion

        #region 基础数据
        #region 基本常量
        private const int MinYear = 1900;
        private const int MaxYear = 2050;
        private static DateTime MinDay = new DateTime(1900, 1, 30);
        private static DateTime MaxDay = new DateTime(2049, 12, 31);
        private const int GanZhiStartYear = 1864; //干支计算起始年
        private static DateTime GanZhiStartDay = new DateTime(1899, 12, 22);//起始日
        private const string HZNum = "零一二三四五六七八九";
        private const int AnimalStartYear = 1900; //1900年为鼠年
        private static DateTime ChineseConstellationReferDay = new DateTime(2007, 9, 13);//28星宿参考值,本日为角
        #endregion

        #region 阴历数据
        /// <summary>
        /// 来源于网上的农历数据
        /// </summary>
        /// <remarks>
        /// 数据结构如下，共使用17位数据
        /// 第17位：表示闰月天数，0表示29天   1表示30天
        /// 第16位-第5位（共12位）表示12个月，其中第16位表示第一月，如果该月为30天则为1，29天为0
        /// 第4位-第1位（共4位）表示闰月是哪个月，如果当年没有闰月，则置0
        ///</remarks>
        private static int[] LunarDateArray = new int[]{
                0x04BD8,0x04AE0,0x0A570,0x054D5,0x0D260,0x0D950,0x16554,0x056A0,0x09AD0,0x055D2,
                0x04AE0,0x0A5B6,0x0A4D0,0x0D250,0x1D255,0x0B540,0x0D6A0,0x0ADA2,0x095B0,0x14977,
                0x04970,0x0A4B0,0x0B4B5,0x06A50,0x06D40,0x1AB54,0x02B60,0x09570,0x052F2,0x04970,
                0x06566,0x0D4A0,0x0EA50,0x06E95,0x05AD0,0x02B60,0x186E3,0x092E0,0x1C8D7,0x0C950,
                0x0D4A0,0x1D8A6,0x0B550,0x056A0,0x1A5B4,0x025D0,0x092D0,0x0D2B2,0x0A950,0x0B557,
                0x06CA0,0x0B550,0x15355,0x04DA0,0x0A5B0,0x14573,0x052B0,0x0A9A8,0x0E950,0x06AA0,
                0x0AEA6,0x0AB50,0x04B60,0x0AAE4,0x0A570,0x05260,0x0F263,0x0D950,0x05B57,0x056A0,
                0x096D0,0x04DD5,0x04AD0,0x0A4D0,0x0D4D4,0x0D250,0x0D558,0x0B540,0x0B6A0,0x195A6,
                0x095B0,0x049B0,0x0A974,0x0A4B0,0x0B27A,0x06A50,0x06D40,0x0AF46,0x0AB60,0x09570,
                0x04AF5,0x04970,0x064B0,0x074A3,0x0EA50,0x06B58,0x055C0,0x0AB60,0x096D5,0x092E0,
                0x0C960,0x0D954,0x0D4A0,0x0DA50,0x07552,0x056A0,0x0ABB7,0x025D0,0x092D0,0x0CAB5,
                0x0A950,0x0B4A0,0x0BAA4,0x0AD50,0x055D9,0x04BA0,0x0A5B0,0x15176,0x052B0,0x0A930,
                0x07954,0x06AA0,0x0AD50,0x05B52,0x04B60,0x0A6E6,0x0A4E0,0x0D260,0x0EA65,0x0D530,
                0x05AA0,0x076A3,0x096D0,0x04BD7,0x04AD0,0x0A4D0,0x1D0B6,0x0D250,0x0D520,0x0DD45,
                0x0B5A0,0x056D0,0x055B2,0x049B0,0x0A577,0x0A4B0,0x0AA50,0x1B255,0x06D20,0x0ADA0,
                0x14B63        
                };

        #endregion

        #region 星座名称
        private static string[] _constellationName = 
                { 
                    "白羊座", "金牛座", "双子座", 
                    "巨蟹座", "狮子座", "处女座", 
                    "天秤座", "天蝎座", "射手座", 
                    "摩羯座", "水瓶座", "双鱼座"
                };
        #endregion

        #region 二十四节气
        private static string[] _lunarHolidayName = 
                    { 
                    "小寒", "大寒", "立春", "雨水", 
                    "惊蛰", "春分", "清明", "谷雨", 
                    "立夏", "小满", "芒种", "夏至", 
                    "小暑", "大暑", "立秋", "处暑", 
                    "白露", "秋分", "寒露", "霜降", 
                    "立冬", "小雪", "大雪", "冬至"
                    };
        #endregion

        #region 二十八星宿
        private static string[] _chineseConstellationName =
            {
                  //四        五      六         日        一      二      三  
                "角木蛟","亢金龙","女土蝠","房日兔","心月狐","尾火虎","箕水豹",
                "斗木獬","牛金牛","氐土貉","虚日鼠","危月燕","室火猪","壁水獝",
                "奎木狼","娄金狗","胃土彘","昴日鸡","毕月乌","觜火猴","参水猿",
                "井木犴","鬼金羊","柳土獐","星日马","张月鹿","翼火蛇","轸水蚓" 
            };
        #endregion

        #region 节气数据
        private static string[] SolarTerm = new string[] { "小寒", "大寒", "立春", "雨水", "惊蛰", "春分", "清明", "谷雨", "立夏", "小满", "芒种", "夏至", "小暑", "大暑", "立秋", "处暑", "白露", "秋分", "寒露", "霜降", "立冬", "小雪", "大雪", "冬至" };
        private static int[] sTermInfo = new int[] { 0, 21208, 42467, 63836, 85337, 107014, 128867, 150921, 173149, 195551, 218072, 240693, 263343, 285989, 308563, 331033, 353350, 375494, 397447, 419210, 440795, 462224, 483532, 504758 };
        #endregion

        #region 农历相关数据
        private static string ganStr = "甲乙丙丁戊己庚辛壬癸";
        private static string zhiStr = "子丑寅卯辰巳午未申酉戌亥";
        private static string animalStr = "鼠牛虎兔龙蛇马羊猴鸡狗猪";
        private static string nStr1 = "日一二三四五六七八九";
        private static string nStr2 = "初十廿卅";
        private static string[] _monthString =
                {
                    "出错","正月","二月","三月","四月","五月","六月","七月","八月","九月","十月","十一月","腊月"
                };
        #endregion

        #region 按公历计算的节日
        private static SolarHolidayStruct[] sHolidayInfo = new SolarHolidayStruct[]{
            new SolarHolidayStruct(1, 1, 1, "元旦"),
            new SolarHolidayStruct(2, 2, 0, "世界湿地日"),
            new SolarHolidayStruct(2, 10, 0, "国际气象节"),
            new SolarHolidayStruct(2, 14, 0, "情人节"),
            new SolarHolidayStruct(3, 1, 0, "国际海豹日"),
            new SolarHolidayStruct(3, 5, 0, "学雷锋纪念日"),
            new SolarHolidayStruct(3, 8, 0, "妇女节"), 
            new SolarHolidayStruct(3, 12, 0, "植树节 孙中山逝世纪念日"), 
            new SolarHolidayStruct(3, 14, 0, "国际警察日"),
            new SolarHolidayStruct(3, 15, 0, "消费者权益日"),
            new SolarHolidayStruct(3, 17, 0, "中国国医节 国际航海日"),
            new SolarHolidayStruct(3, 21, 0, "世界森林日 消除种族歧视国际日 世界儿歌日"),
            new SolarHolidayStruct(3, 22, 0, "世界水日"),
            new SolarHolidayStruct(3, 24, 0, "世界防治结核病日"),
            new SolarHolidayStruct(4, 1, 0, "愚人节"),
            new SolarHolidayStruct(4, 7, 0, "世界卫生日"),
            new SolarHolidayStruct(4, 22, 0, "世界地球日"),
            new SolarHolidayStruct(5, 1, 1, "劳动节"), 
            new SolarHolidayStruct(5, 2, 1, "劳动节假日"),
            new SolarHolidayStruct(5, 3, 1, "劳动节假日"),
            new SolarHolidayStruct(5, 4, 0, "青年节"), 
            new SolarHolidayStruct(5, 8, 0, "世界红十字日"),
            new SolarHolidayStruct(5, 12, 0, "国际护士节"), 
            new SolarHolidayStruct(5, 31, 0, "世界无烟日"), 
            new SolarHolidayStruct(6, 1, 0, "国际儿童节"), 
            new SolarHolidayStruct(6, 5, 0, "世界环境保护日"),
            new SolarHolidayStruct(6, 26, 0, "国际禁毒日"),
            new SolarHolidayStruct(7, 1, 0, "建党节 香港回归纪念 世界建筑日"),
            new SolarHolidayStruct(7, 11, 0, "世界人口日"),
            new SolarHolidayStruct(8, 1, 0, "建军节"), 
            new SolarHolidayStruct(8, 8, 0, "中国男子节 父亲节"),
            new SolarHolidayStruct(8, 15, 0, "抗日战争胜利纪念"),
            new SolarHolidayStruct(9, 9, 0, "毛主席逝世纪念"), 
            new SolarHolidayStruct(9, 10, 0, "教师节"), 
            new SolarHolidayStruct(9, 18, 0, "九·一八事变纪念日"),
            new SolarHolidayStruct(9, 20, 0, "国际爱牙日"),
            new SolarHolidayStruct(9, 27, 0, "世界旅游日"),
            new SolarHolidayStruct(9, 28, 0, "孔子诞辰"),
            new SolarHolidayStruct(10, 1, 1, "国庆节 国际音乐日"),
            new SolarHolidayStruct(10, 2, 1, "国庆节假日"),
            new SolarHolidayStruct(10, 3, 1, "国庆节假日"),
            new SolarHolidayStruct(10, 6, 0, "老人节"), 
            new SolarHolidayStruct(10, 24, 0, "联合国日"),
            new SolarHolidayStruct(11, 10, 0, "世界青年节"),
            new SolarHolidayStruct(11, 12, 0, "孙中山诞辰纪念"), 
            new SolarHolidayStruct(12, 1, 0, "世界艾滋病日"), 
            new SolarHolidayStruct(12, 3, 0, "世界残疾人日"), 
            new SolarHolidayStruct(12, 20, 0, "澳门回归纪念"), 
            new SolarHolidayStruct(12, 24, 0, "平安夜"), 
            new SolarHolidayStruct(12, 25, 0, "圣诞节"), 
            new SolarHolidayStruct(12, 26, 0, "毛主席诞辰纪念")
           };
        #endregion

        #region 按农历计算的节日
        private static LunarHolidayStruct[] lHolidayInfo = new LunarHolidayStruct[]{
            new LunarHolidayStruct(1, 1, 1, "春节"), 
            new LunarHolidayStruct(1, 15, 0, "元宵节"), 
            new LunarHolidayStruct(5, 5, 0, "端午节"), 
            new LunarHolidayStruct(7, 7, 0, "七夕情人节"),
            new LunarHolidayStruct(7, 15, 0, "中元节 盂兰盆节"), 
            new LunarHolidayStruct(8, 15, 0, "中秋节"), 
            new LunarHolidayStruct(9, 9, 0, "重阳节"), 
            new LunarHolidayStruct(12, 8, 0, "腊八节"),
            new LunarHolidayStruct(12, 23, 0, "北方小年(扫房)"),
            new LunarHolidayStruct(12, 24, 0, "南方小年(掸尘)"),
            //new LunarHolidayStruct(12, 30, 0, "除夕")  //注意除夕需要其它方法进行计算
        };
        #endregion

        #region 按某月第几个星期几
        private static WeekHolidayStruct[] wHolidayInfo = new WeekHolidayStruct[]{
            new WeekHolidayStruct(5, 2, 1, "母亲节"), 
            new WeekHolidayStruct(5, 3, 1, "全国助残日"), 
            new WeekHolidayStruct(6, 3, 1, "父亲节"), 
            new WeekHolidayStruct(9, 3, 3, "国际和平日"), 
            new WeekHolidayStruct(9, 4, 1, "国际聋人节"), 
            new WeekHolidayStruct(10, 1, 2, "国际住房日"), 
            new WeekHolidayStruct(10, 1, 4, "国际减轻自然灾害日"),
            new WeekHolidayStruct(11, 4, 5, "感恩节")
        };
        #endregion
        #endregion

        #region 构造函数
        #region 公历日期初始化
        /// <summary>
        /// 用一个标准的公历日期来初使化
        /// </summary>
        public ChineseCalendar(DateTime dt)
        {
            int i;
            int leap;
            int temp;
            int offset;

            CheckDateLimit(dt);

            _date = dt.Date;
            _datetime = dt;

            //农历日期计算部分
            leap = 0;
            temp = 0;

            //计算两天的基本差距
            TimeSpan ts = _date - ChineseCalendar.MinDay;
            offset = ts.Days;

            for (i = MinYear; i <= MaxYear; i++)
            {
                //求当年农历年天数
                temp = GetChineseYearDays(i);
                if (offset - temp < 1)
                    break;
                else
                {
                    offset = offset - temp;
                }
            }
            _cYear = i;

            //计算该年闰哪个月
            leap = GetChineseLeapMonth(_cYear);

            //设定当年是否有闰月
            if (leap > 0)
            {
                _cIsLeapYear = true;
            }
            else
            {
                _cIsLeapYear = false;
            }

            _cIsLeapMonth = false;
            for (i = 1; i <= 12; i++)
            {
                //闰月
                if ((leap > 0) && (i == leap + 1) && (_cIsLeapMonth == false))
                {
                    _cIsLeapMonth = true;
                    i = i - 1;
                    temp = GetChineseLeapMonthDays(_cYear); //计算闰月天数
                }
                else
                {
                    _cIsLeapMonth = false;
                    temp = GetChineseMonthDays(_cYear, i);  //计算非闰月天数
                }

                offset = offset - temp;
                if (offset <= 0) break;
            }

            offset = offset + temp;
            _cMonth = i;
            _cDay = offset;
        }
        #endregion

        #region 农历日期初始化
        /// <summary>
        /// 用农历的日期来初使化
        /// </summary>
        /// <param name="cy">农历年</param>
        /// <param name="cm">农历月</param>
        /// <param name="cd">农历日</param>
        /// <param name="LeapFlag">闰月标志</param>
        public ChineseCalendar(int cy, int cm, int cd, bool leapMonthFlag)
        {
            int i, leap, Temp, offset;

            CheckChineseDateLimit(cy, cm, cd, leapMonthFlag);

            _cYear = cy;
            _cMonth = cm;
            _cDay = cd;

            offset = 0;

            for (i = MinYear; i < cy; i++)
            {
                //求当年农历年天数
                Temp = GetChineseYearDays(i);
                offset = offset + Temp;
            }

            //计算该年应该闰哪个月
            leap = GetChineseLeapMonth(cy);
            if (leap != 0)
            {
                this._cIsLeapYear = true;
            }
            else
            {
                this._cIsLeapYear = false;
            }

            if (cm != leap)
            {
                //当前日期并非闰月
                _cIsLeapMonth = false;
            }
            else
            {
                //使用用户输入的是否闰月月份
                _cIsLeapMonth = leapMonthFlag;
            }

            //当年没有闰月||计算月份小于闰月
            if ((_cIsLeapYear == false) || (cm < leap))
            {
                for (i = 1; i < cm; i++)
                {
                    Temp = GetChineseMonthDays(cy, i);//计算非闰月天数
                    offset = offset + Temp;
                }

                //检查日期是否大于最大天
                if (cd > GetChineseMonthDays(cy, cm))
                {
                    throw new Exception("不合法的农历日期");
                }
                //加上当月的天数
                offset = offset + cd;
            }

            //是闰年，且计算月份大于或等于闰月
            else
            {
                for (i = 1; i < cm; i++)
                {
                    //计算非闰月天数
                    Temp = GetChineseMonthDays(cy, i);
                    offset = offset + Temp;
                }

                //计算月大于闰月
                if (cm > leap)
                {
                    Temp = GetChineseLeapMonthDays(cy);   //计算闰月天数
                    offset = offset + Temp;               //加上闰月天数

                    if (cd > GetChineseMonthDays(cy, cm))
                    {
                        throw new Exception("不合法的农历日期");
                    }
                    offset = offset + cd;
                }

                //计算月等于闰月
                else
                {
                    //如果需要计算的是闰月，则应首先加上与闰月对应的普通月的天数
                    if (this._cIsLeapMonth == true)         //计算月为闰月
                    {
                        Temp = GetChineseMonthDays(cy, cm); //计算非闰月天数
                        offset = offset + Temp;
                    }

                    if (cd > GetChineseLeapMonthDays(cy))
                    {
                        throw new Exception("不合法的农历日期");
                    }
                    offset = offset + cd;
                }
            }
            _date = MinDay.AddDays(offset);
        }
        #endregion
        #endregion

        #region 私有函数
        #region GetChineseMonthDays
        /// <summary>
        /// //传回农历y年m月的总天数
        /// </summary>
        private int GetChineseMonthDays(int year, int month)
        {
            if (BitTest32((LunarDateArray[year - MinYear] & 0x0000FFFF), (16 - month)))
            {
                return 30;
            }
            else
            {
                return 29;
            }
        }
        #endregion

        #region GetChineseLeapMonth
        /// <summary>
        /// 传回农历 y年闰哪个月 1-12 , 没闰传回 0
        /// </summary>
        private int GetChineseLeapMonth(int year)
        {
            return LunarDateArray[year - MinYear] & 0xF;
        }
        #endregion

        #region GetChineseLeapMonthDays
        /// <summary>
        /// 传回农历y年闰月的天数
        /// </summary>
        private int GetChineseLeapMonthDays(int year)
        {
            if (GetChineseLeapMonth(year) != 0)
            {
                if ((LunarDateArray[year - MinYear] & 0x10000) != 0)
                {
                    return 30;
                }
                else
                {
                    return 29;
                }
            }
            else
            {
                return 0;
            }
        }
        #endregion

        #region GetChineseYearDays
        /// <summary>
        /// 取农历年一年的天数
        /// </summary>
        private int GetChineseYearDays(int year)
        {
            int i, f, sumDay, info;

            sumDay = 348; //29天*12个月
            i = 0x8000;
            info = LunarDateArray[year - MinYear] & 0x0FFFF;

            //计算12个月中有多少天为30天
            for (int m = 0; m < 12; m++)
            {
                f = info & i;
                if (f != 0)
                {
                    sumDay++;
                }
                i = i >> 1;
            }
            return sumDay + GetChineseLeapMonthDays(year);
        }
        #endregion

        #region GetChineseHour
        /// <summary>
        /// 获得当前时间的时辰
        /// </summary> 
        private string GetChineseHour(DateTime dt)
        {
            int _hour, _minute, offset, i;
            int indexGan;
            string ganHour, zhiHour;
            string tmpGan;

            //计算时辰的地支
            _hour = dt.Hour;    //获得当前时间小时
            _minute = dt.Minute;  //获得当前时间分钟

            if (_minute != 0) _hour += 1;
            offset = _hour / 2;
            if (offset >= 12) offset = 0;
            //zhiHour = zhiStr[offset].ToString();

            //计算天干
            TimeSpan ts = this._date - GanZhiStartDay;
            i = ts.Days % 60;

            //ganStr[i % 10] 为日的天干,(n*2-1) %10得出地支对应,n从1开始
            indexGan = ((i % 10 + 1) * 2 - 1) % 10 - 1;

            tmpGan = ganStr.Substring(indexGan) + ganStr.Substring(0, indexGan + 2);//凑齐12位
            //ganHour = ganStr[((i % 10 + 1) * 2 - 1) % 10 - 1].ToString();

            return tmpGan[offset].ToString() + zhiStr[offset].ToString();
        }
        #endregion

        #region CheckDateLimit
        /// <summary>
        /// 检查公历日期是否符合要求
        /// </summary>
        private void CheckDateLimit(DateTime dt)
        {
            if ((dt < MinDay) || (dt > MaxDay))
            {
                throw new Exception("超出可转换的日期");
            }
        }
        #endregion

        #region CheckChineseDateLimit
        /// <summary>
        /// 检查农历日期是否合理
        /// </summary>
        private void CheckChineseDateLimit(int year, int month, int day, bool leapMonth)
        {
            if ((year < MinYear) || (year > MaxYear))
            {
                throw new Exception("非法农历日期");
            }
            if ((month < 1) || (month > 12))
            {
                throw new Exception("非法农历日期");
            }
            if ((day < 1) || (day > 30)) //中国的月最多30天
            {
                throw new Exception("非法农历日期");
            }
            int leap = GetChineseLeapMonth(year);// 计算该年应该闰哪个月
            if ((leapMonth == true) && (month != leap))
            {
                throw new Exception("非法农历日期");
            }
        }
        #endregion

        #region ConvertNumToChineseNum
        /// <summary>
        /// 将0-9转成汉字形式
        /// </summary>
        private string ConvertNumToChineseNum(char n)
        {
            if ((n < '0') || (n > '9')) return "";
            switch (n)
            {
                case '0':
                    return HZNum[0].ToString();
                case '1':
                    return HZNum[1].ToString();
                case '2':
                    return HZNum[2].ToString();
                case '3':
                    return HZNum[3].ToString();
                case '4':
                    return HZNum[4].ToString();
                case '5':
                    return HZNum[5].ToString();
                case '6':
                    return HZNum[6].ToString();
                case '7':
                    return HZNum[7].ToString();
                case '8':
                    return HZNum[8].ToString();
                case '9':
                    return HZNum[9].ToString();
                default:
                    return "";
            }
        }
        #endregion

        #region BitTest32
        /// <summary>
        /// 测试某位是否为真
        /// </summary>
        private bool BitTest32(int num, int bitpostion)
        {
            if ((bitpostion > 31) || (bitpostion < 0))
                throw new Exception("Error Param: bitpostion[0-31]:" + bitpostion.ToString());

            int bit = 1 << bitpostion;

            if ((num & bit) == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        #endregion

        #region ConvertDayOfWeek
        /// <summary>
        /// 将星期几转成数字表示
        /// </summary>
        private int ConvertDayOfWeek(DayOfWeek dayOfWeek)
        {
            switch (dayOfWeek)
            {
                case DayOfWeek.Sunday:
                    return 1;
                case DayOfWeek.Monday:
                    return 2;
                case DayOfWeek.Tuesday:
                    return 3;
                case DayOfWeek.Wednesday:
                    return 4;
                case DayOfWeek.Thursday:
                    return 5;
                case DayOfWeek.Friday:
                    return 6;
                case DayOfWeek.Saturday:
                    return 7;
                default:
                    return 0;
            }
        }
        #endregion

        #region CompareWeekDayHoliday
        /// <summary>
        /// 比较当天是不是指定的第周几
        /// </summary>
        private bool CompareWeekDayHoliday(DateTime date, int month, int week, int day)
        {
            bool ret = false;

            if (date.Month == month) //月份相同
            {
                if (ConvertDayOfWeek(date.DayOfWeek) == day) //星期几相同
                {
                    DateTime firstDay = new DateTime(date.Year, date.Month, 1);//生成当月第一天
                    int i = ConvertDayOfWeek(firstDay.DayOfWeek);
                    int firWeekDays = 7 - ConvertDayOfWeek(firstDay.DayOfWeek) + 1; //计算第一周剩余天数

                    if (i > day)
                    {
                        if ((week - 1) * 7 + day + firWeekDays == date.Day)
                        {
                            ret = true;
                        }
                    }
                    else
                    {
                        if (day + firWeekDays + (week - 2) * 7 == date.Day)
                        {
                            ret = true;
                        }
                    }
                }
            }

            return ret;
        }
        #endregion
        #endregion

        #region  属性
        #region 节日
        #region newCalendarHoliday
        /// <summary>
        /// 计算中国农历节日
        /// </summary>
        public string newCalendarHoliday
        {
            get
            {
                string tempStr = "";
                if (this._cIsLeapMonth == false) //闰月不计算节日
                {
                    foreach (LunarHolidayStruct lh in lHolidayInfo)
                    {
                        if ((lh.Month == this._cMonth) && (lh.Day == this._cDay))
                        {

                            tempStr = lh.HolidayName;
                            break;

                        }
                    }

                    //对除夕进行特别处理
                    if (this._cMonth == 12)
                    {
                        int i = GetChineseMonthDays(this._cYear, 12); //计算当年农历12月的总天数
                        if (this._cDay == i) //如果为最后一天
                        {
                            tempStr = "除夕";
                        }
                    }
                }
                return tempStr;
            }
        }
        #endregion

        #region WeekDayHoliday
        /// <summary>
        /// 按某月第几周第几日计算的节日
        /// </summary>
        public string WeekDayHoliday
        {
            get
            {
                string tempStr = "";
                foreach (WeekHolidayStruct wh in wHolidayInfo)
                {
                    if (CompareWeekDayHoliday(_date, wh.Month, wh.WeekAtMonth, wh.WeekDay))
                    {
                        tempStr = wh.HolidayName;
                        break;
                    }
                }
                return tempStr;
            }
        }
        #endregion

        #region DateHoliday
        /// <summary>
        /// 按公历日计算的节日
        /// </summary>
        public string DateHoliday
        {
            get
            {
                string tempStr = "";

                foreach (SolarHolidayStruct sh in sHolidayInfo)
                {
                    if ((sh.Month == _date.Month) && (sh.Day == _date.Day))
                    {
                        tempStr = sh.HolidayName;
                        break;
                    }
                }
                return tempStr;
            }
        }
        #endregion
        #endregion

        #region 公历日期
        #region Date
        /// <summary>
        /// 取对应的公历日期
        /// </summary>
        public DateTime Date
        {
            get { return _date; }
            set { _date = value; }
        }
        #endregion

        #region WeekDay
        /// <summary>
        /// 取星期几
        /// </summary>
        public DayOfWeek WeekDay
        {
            get { return _date.DayOfWeek; }
        }
        #endregion

        #region WeekDayStr
        /// <summary>
        /// 周几的字符
        /// </summary>
        public string WeekDayStr
        {
            get
            {
                switch (_date.DayOfWeek)
                {
                    case DayOfWeek.Sunday:
                        return "星期日";
                    case DayOfWeek.Monday:
                        return "星期一";
                    case DayOfWeek.Tuesday:
                        return "星期二";
                    case DayOfWeek.Wednesday:
                        return "星期三";
                    case DayOfWeek.Thursday:
                        return "星期四";
                    case DayOfWeek.Friday:
                        return "星期五";
                    default:
                        return "星期六";
                }
            }
        }
        #endregion

        #region DateString
        /// <summary>
        /// 公历日期中文表示法 如一九九七年七月一日
        /// </summary>
        public string DateString
        {
            get
            {
                return "公元" + this._date.ToLongDateString();
            }
        }
        #endregion

        #region IsLeapYear
        /// <summary>
        /// 当前是否公历闰年
        /// </summary>
        public bool IsLeapYear
        {
            get
            {
                return DateTime.IsLeapYear(this._date.Year);
            }
        }
        #endregion

        #region ChineseConstellation
        /// <summary>
        /// 28星宿计算
        /// </summary>
        public string ChineseConstellation
        {
            get
            {
                int offset = 0;
                int modStarDay = 0;

                TimeSpan ts = this._date - ChineseConstellationReferDay;
                offset = ts.Days;
                modStarDay = offset % 28;
                return (modStarDay >= 0 ? _chineseConstellationName[modStarDay] : _chineseConstellationName[27 + modStarDay]);
            }
        }
        #endregion

        #region ChineseHour
        /// <summary>
        /// 时辰
        /// </summary>
        public string ChineseHour
        {
            get
            {
                return GetChineseHour(_datetime);
            }
        }
        #endregion

        #endregion

        #region 农历日期
        #region IsChineseLeapMonth
        /// <summary>
        /// 是否闰月
        /// </summary>
        public bool IsChineseLeapMonth
        {
            get { return this._cIsLeapMonth; }
        }
        #endregion

        #region IsChineseLeapYear
        /// <summary>
        /// 当年是否有闰月
        /// </summary>
        public bool IsChineseLeapYear
        {
            get
            {
                return this._cIsLeapYear;
            }
        }
        #endregion

        #region ChineseDay
        /// <summary>
        /// 农历日
        /// </summary>
        public int ChineseDay
        {
            get { return this._cDay; }
        }
        #endregion

        #region ChineseDayString
        /// <summary>
        /// 农历日中文表示
        /// </summary>
        public string ChineseDayString
        {
            get
            {
                switch (this._cDay)
                {
                    case 0:
                        return "";
                    case 10:
                        return "初十";
                    case 20:
                        return "二十";
                    case 30:
                        return "三十";
                    default:
                        return nStr2[(int)(_cDay / 10)].ToString() + nStr1[_cDay % 10].ToString();

                }
            }
        }
        #endregion

        #region ChineseMonth
        /// <summary>
        /// 农历的月份
        /// </summary>
        public int ChineseMonth
        {
            get { return this._cMonth; }
        }
        #endregion

        #region ChineseMonthString
        /// <summary>
        /// 农历月份字符串
        /// </summary>
        public string ChineseMonthString
        {
            get
            {
                return _monthString[this._cMonth];
            }
        }
        #endregion

        #region ChineseYear
        /// <summary>
        /// 取农历年份
        /// </summary>
        public int ChineseYear
        {
            get { return this._cYear; }
        }
        #endregion

        #region ChineseYearString
        /// <summary>
        /// 取农历年字符串如，一九九七年
        /// </summary>
        public string ChineseYearString
        {
            get
            {
                string tempStr = "";
                string num = this._cYear.ToString();
                for (int i = 0; i < 4; i++)
                {
                    tempStr += ConvertNumToChineseNum(num[i]);
                }
                return tempStr + "年";
            }
        }
        #endregion

        #region ChineseDateString
        /// <summary>
        /// 取农历日期表示法：农历一九九七年正月初五
        /// </summary>
        public string ChineseDateString
        {
            get
            {
                if (this._cIsLeapMonth == true)
                {
                    return "农历" + ChineseYearString + "闰" + ChineseMonthString + ChineseDayString;
                }
                else
                {
                    return "农历" + ChineseYearString + ChineseMonthString + ChineseDayString;
                }
            }
        }
        #endregion

        #region ChineseTwentyFourDay
        /// <summary>
        /// 定气法计算二十四节气,二十四节气是按地球公转来计算的，并非是阴历计算的
        /// </summary>
        /// <remarks>
        /// 节气的定法有两种。古代历法采用的称为"恒气"，即按时间把一年等分为24份，
        /// 每一节气平均得15天有余，所以又称"平气"。现代农历采用的称为"定气"，即
        /// 按地球在轨道上的位置为标准，一周360°，两节气之间相隔15°。由于冬至时地
        /// 球位于近日点附近，运动速度较快，因而太阳在黄道上移动15°的时间不到15天。
        /// 夏至前后的情况正好相反，太阳在黄道上移动较慢，一个节气达16天之多。采用
        /// 定气时可以保证春、秋两分必然在昼夜平分的那两天。
        /// </remarks>
        public string ChineseTwentyFourDay
        {
            get
            {
                DateTime baseDateAndTime = new DateTime(1900, 1, 6, 2, 5, 0); //#1/6/1900 2:05:00 AM#
                DateTime newDate;
                double num;
                int y;
                string tempStr = "";

                y = this._date.Year;

                for (int i = 1; i <= 24; i++)
                {
                    num = 525948.76 * (y - 1900) + sTermInfo[i - 1];

                    newDate = baseDateAndTime.AddMinutes(num);//按分钟计算
                    if (newDate.DayOfYear == _date.DayOfYear)
                    {
                        tempStr = SolarTerm[i - 1];
                        break;
                    }
                }
                return tempStr;
            }
        }

        //当前日期前一个最近节气
        public string ChineseTwentyFourPrevDay
        {
            get
            {
                DateTime baseDateAndTime = new DateTime(1900, 1, 6, 2, 5, 0); //#1/6/1900 2:05:00 AM#
                DateTime newDate;
                double num;
                int y;
                string tempStr = "";

                y = this._date.Year;

                for (int i = 24; i >= 1; i--)
                {
                    num = 525948.76 * (y - 1900) + sTermInfo[i - 1];

                    newDate = baseDateAndTime.AddMinutes(num);//按分钟计算

                    if (newDate.DayOfYear < _date.DayOfYear)
                    {
                        tempStr = string.Format("{0}[{1}]", SolarTerm[i - 1], newDate.ToString("yyyy-MM-dd"));
                        break;
                    }
                }

                return tempStr;
            }

        }

        //当前日期后一个最近节气
        public string ChineseTwentyFourNextDay
        {
            get
            {
                DateTime baseDateAndTime = new DateTime(1900, 1, 6, 2, 5, 0); //#1/6/1900 2:05:00 AM#
                DateTime newDate;
                double num;
                int y;
                string tempStr = "";

                y = this._date.Year;

                for (int i = 1; i <= 24; i++)
                {
                    num = 525948.76 * (y - 1900) + sTermInfo[i - 1];

                    newDate = baseDateAndTime.AddMinutes(num);//按分钟计算

                    if (newDate.DayOfYear > _date.DayOfYear)
                    {
                        tempStr = string.Format("{0}[{1}]", SolarTerm[i - 1], newDate.ToString("yyyy-MM-dd"));
                        break;
                    }
                }
                return tempStr;
            }

        }
        #endregion
        #endregion

        #region 星座
        /// <summary>
        /// 计算指定日期的星座序号 
        /// </summary>
        public string Constellation
        {
            get
            {
                int index = 0;
                int y, m, d;
                y = _date.Year;
                m = _date.Month;
                d = _date.Day;
                y = m * 100 + d;

                if (((y >= 321) && (y <= 419))) { index = 0; }
                else if ((y >= 420) && (y <= 520)) { index = 1; }
                else if ((y >= 521) && (y <= 620)) { index = 2; }
                else if ((y >= 621) && (y <= 722)) { index = 3; }
                else if ((y >= 723) && (y <= 822)) { index = 4; }
                else if ((y >= 823) && (y <= 922)) { index = 5; }
                else if ((y >= 923) && (y <= 1022)) { index = 6; }
                else if ((y >= 1023) && (y <= 1121)) { index = 7; }
                else if ((y >= 1122) && (y <= 1221)) { index = 8; }
                else if ((y >= 1222) || (y <= 119)) { index = 9; }
                else if ((y >= 120) && (y <= 218)) { index = 10; }
                else if ((y >= 219) && (y <= 320)) { index = 11; }
                else { index = 0; }

                return _constellationName[index];
            }
        }
        #endregion

        #region 属相
        #region Animal
        /// <summary>
        /// 计算属相的索引，注意虽然属相是以农历年来区别的，但是目前在实际使用中是按公历来计算的
        /// 鼠年为1,其它类推
        /// </summary>
        public int Animal
        {
            get
            {
                int offset = _date.Year - AnimalStartYear;
                return (offset % 12) + 1;
            }
        }
        #endregion

        #region AnimalString
        /// <summary>
        /// 取属相字符串
        /// </summary>
        public string AnimalString
        {
            get
            {
                int offset = _date.Year - AnimalStartYear; //阳历计算
                //int offset = this._cYear - AnimalStartYear;　农历计算
                return animalStr[offset % 12].ToString();
            }
        }
        #endregion
        #endregion

        #region 天干地支
        #region GanZhiYearString
        /// <summary>
        /// 取农历年的干支表示法如 乙丑年
        /// </summary>
        public string GanZhiYearString
        {
            get
            {
                string tempStr;
                int i = (this._cYear - GanZhiStartYear) % 60; //计算干支
                tempStr = ganStr[i % 10].ToString() + zhiStr[i % 12].ToString() + "年";
                return tempStr;
            }
        }
        #endregion

        #region GanZhiMonthString
        /// <summary>
        /// 取干支的月表示字符串，注意农历的闰月不记干支
        /// </summary>
        public string GanZhiMonthString
        {
            get
            {
                //每个月的地支总是固定的,而且总是从寅月开始
                int zhiIndex;
                string zhi;
                if (this._cMonth > 10)
                {
                    zhiIndex = this._cMonth - 10;
                }
                else
                {
                    zhiIndex = this._cMonth + 2;
                }
                zhi = zhiStr[zhiIndex - 1].ToString();

                //根据当年的干支年的干来计算月干的第一个
                int ganIndex = 1;
                string gan;
                int i = (this._cYear - GanZhiStartYear) % 60; //计算干支
                switch (i % 10)
                {
                    #region ...
                    case 0: //甲
                        ganIndex = 3;
                        break;
                    case 1: //乙
                        ganIndex = 5;
                        break;
                    case 2: //丙
                        ganIndex = 7;
                        break;
                    case 3: //丁
                        ganIndex = 9;
                        break;
                    case 4: //戊
                        ganIndex = 1;
                        break;
                    case 5: //己
                        ganIndex = 3;
                        break;
                    case 6: //庚
                        ganIndex = 5;
                        break;
                    case 7: //辛
                        ganIndex = 7;
                        break;
                    case 8: //壬
                        ganIndex = 9;
                        break;
                    case 9: //癸
                        ganIndex = 1;
                        break;
                    #endregion
                }
                gan = ganStr[(ganIndex + this._cMonth - 2) % 10].ToString();

                return gan + zhi + "月";
            }
        }
        #endregion

        #region GanZhiDayString
        /// <summary>
        /// 取干支日表示法
        /// </summary>
        public string GanZhiDayString
        {
            get
            {
                int i, offset;
                TimeSpan ts = this._date - GanZhiStartDay;
                offset = ts.Days;
                i = offset % 60;
                return ganStr[i % 10].ToString() + zhiStr[i % 12].ToString() + "日";
            }
        }
        #endregion

        #region GanZhiDateString
        /// <summary>
        /// 取当前日期的干支表示法如 甲子年乙丑月丙庚日
        /// </summary>
        public string GanZhiDateString
        {
            get
            {
                return GanZhiYearString + GanZhiMonthString + GanZhiDayString;
            }
        }
        #endregion
        #endregion
        #endregion
    }
}


