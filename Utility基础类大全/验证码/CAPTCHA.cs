 
using System;
using System.Web;
using System.Drawing;
using System.Security.Cryptography;
using System.Threading;

namespace Common.Utility
{
    /// <summary>
    /// 验证码类
    /// </summary>
    public static class StringCAPTCHA
    {
		/// <summary>
		/// create a random key
		/// </summary>
		static readonly Random Random = new Random(~unchecked((int)DateTime.Now.Ticks));
        static readonly char[] NumberList = {'1','2','3','4','5','6','7','8','9'};
        static readonly char[] CharList = { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };
        static readonly char[] MixedList = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'J', 'K', 'L', 'M', 'N', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' }; //remove I & O

		#region 生成随机数字
		/// <summary>
		/// 生成随机数字
		/// </summary>
		/// <param name="Length">生成长度</param>
		public static string Number(int Length)
        {
            return Create(Length, false,NumberList);
        }

        /// <summary>
        /// 生成随机数字
        /// </summary>
        /// <param name="Length">生成长度</param>
        /// <param name="Sleep">是否要在生成前将当前线程阻止以避免重复</param>
        public static string Number(int Length, bool Sleep)
        {
            return Create(Length, Sleep, NumberList);
        }
		#endregion

		#region 生成随机字母与数字
		/// <summary>
		/// 生成随机字母与数字
		/// </summary>
		/// <param name="Length">生成长度</param>
		public static string Mixed(int Length)
        {
            return Create(Length, false,MixedList);
        }

        /// <summary>
        /// 生成随机字母与数字
        /// </summary>
        /// <param name="Length">生成长度</param>
        /// <param name="Sleep">是否要在生成前将当前线程阻止以避免重复</param>
        public static string Mixed(int Length, bool Sleep)
        {
            return Create(Length, Sleep, MixedList);
        }
		#endregion

		#region 生成随机纯字母随机数
		/// <summary>
		/// 生成随机纯字母随机数
		/// </summary>
		/// <param name="Length">生成长度</param>
		public static string Char(int Length)
        {
            return Create(Length, false, CharList);
        }

        /// <summary>
        /// 生成随机纯字母随机数
        /// </summary>
        /// <param name="Length">生成长度</param>
        /// <param name="Sleep">是否要在生成前将当前线程阻止以避免重复</param>
        public static string Char(int Length, bool Sleep)
        {
            return Create(Length, Sleep, CharList);
        }
		#endregion

		/// <summary>
		/// Create the CAPTCHA specified Length, Sleep and List.
		/// </summary>
		/// <returns>The create.</returns>
		/// <param name="Length">Length.</param>
		/// <param name="Sleep">If set to <c>true</c> sleep.</param>
		/// <param name="List">List create CAPTCHA based on</param>
		private static string Create(int Length, bool Sleep, char[] List)
		{
			if (Sleep) Thread.Sleep(3);
			char[] Pattern = List;
			string result = string.Empty;
			int n = Pattern.Length;

			for (int i = 0; i < Length; i++)
			{
				int rnd = Random.Next(0, n);
				result += Pattern[rnd];
			}
			return result;
		}



    }

    /// <summary>
    /// 验证图片类
    /// </summary>
    public class PictureCAPTCHA
    {
        #region 私有字段
        private string Text { get; set; }
        private Bitmap Image { get; set; }
        private int LetterCount { set; get; }  //验证码位数
        private int Type { set; get; }
        private int letterWidth = 16;  //单个字体的宽度范围
        private int letterHeight = 20; //单个字体的高度范围
        private static  Random Random = new Random(~unchecked((int)DateTime.Now.Ticks));
        private Font[] fonts = 
        {
           new Font(new FontFamily("Times New Roman"),10 +Random.Next(1),FontStyle.Regular),
           new Font(new FontFamily("Georgia"), 10 + Random.Next(1),FontStyle.Regular),
           new Font(new FontFamily("Arial"), 10 + Random.Next(1),FontStyle.Regular),
           new Font(new FontFamily("Comic Sans MS"), 10 + Random.Next(1),FontStyle.Regular)
        };
        #endregion

      

        #region constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Common.Utility.PictureCAPTCHA"/> class. default length is 4 with number list 
        /// </summary>
        public PictureCAPTCHA()
        {
            HttpContext.Current.Response.Expires = 0;
            HttpContext.Current.Response.Buffer = true;
            HttpContext.Current.Response.ExpiresAbsolute = DateTime.Now.AddSeconds(-1);
            HttpContext.Current.Response.AddHeader("pragma", "no-cache");
            HttpContext.Current.Response.CacheControl = "no-cache";
            LetterCount = 4;
            Type = 0;
			InitText();
            CreateImage();
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Common.Utility.PictureCAPTCHA"/> class with number list;
        /// </summary>
        /// <param name="Length">Length.</param>
		public PictureCAPTCHA(int Length)
        {
			HttpContext.Current.Response.Expires = 0;
			HttpContext.Current.Response.Buffer = true;
			HttpContext.Current.Response.ExpiresAbsolute = DateTime.Now.AddSeconds(-1);
			HttpContext.Current.Response.AddHeader("pragma", "no-cache");
			HttpContext.Current.Response.CacheControl = "no-cache";
            LetterCount = Length;
            Text = StringCAPTCHA.Number(LetterCount);
			CreateImage();
		}
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Common.Utility.PictureCAPTCHA"/> class.
        /// </summary>
        /// <param name="Length">Length.</param>
        /// <param name="type">Type 0 number , 1 char , 2 mixed.</param>
		public PictureCAPTCHA(int Length,int type)
		{
			HttpContext.Current.Response.Expires = 0;
			HttpContext.Current.Response.Buffer = true;
			HttpContext.Current.Response.ExpiresAbsolute = DateTime.Now.AddSeconds(-1);
			HttpContext.Current.Response.AddHeader("pragma", "no-cache");
			HttpContext.Current.Response.CacheControl = "no-cache";
            LetterCount = Length;
            Type = type;
			InitText();
			CreateImage();
		}

		#endregion

		#region Public function

        public void Redraw(bool NewCAPTCHA ){
            if (NewCAPTCHA)
            {
                InitText();
            }
            CreateImage();
        }

		#endregion


		#region private funcation

        private void InitText()
        {
			switch (Type)
			{
				case 0: Text = StringCAPTCHA.Number(LetterCount); break;
				case 1: Text = StringCAPTCHA.Char(LetterCount); break;
				case 2: Text = StringCAPTCHA.Mixed(LetterCount); break;
				default:

					break;
			}
        }

		/// <summary>
		/// 绘制验证码
		/// </summary>
		private void CreateImage()
		{
			int ImageWidth = this.Text.Length * letterWidth;
			Bitmap Img = new Bitmap(ImageWidth, letterHeight);
			Graphics g = Graphics.FromImage(Img);
			g.Clear(Color.White);
			for (int i = 0; i < 2; i++)
			{
				int x1 = Random.Next(Img.Width - 1);
				int x2 = Random.Next(Img.Width - 1);
				int y1 = Random.Next(Img.Height - 1);
				int y2 = Random.Next(Img.Height - 1);
				g.DrawLine(new Pen(Color.Silver), x1, y1, x2, y2);
			}
			int _x = -12, _y = 0;
			for (int int_index = 0; int_index < this.Text.Length; int_index++)
			{
				_x += Random.Next(12, 16);
				_y = Random.Next(-2, 2);
				string str_char = this.Text.Substring(int_index, 1);
				str_char = Random.Next(1) == 1 ? str_char.ToLower() : str_char.ToUpper();
				Brush newBrush = new SolidBrush(GetRandomColor());
				Point thePos = new Point(_x, _y);
				g.DrawString(str_char, fonts[Random.Next(fonts.Length - 1)], newBrush, thePos);
			}
			for (int i = 0; i < 10; i++)
			{
				int x = Random.Next(Img.Width - 1);
				int y = Random.Next(Img.Height - 1);
				Img.SetPixel(x, y, Color.FromArgb(Random.Next(0, 255), Random.Next(0, 255), Random.Next(0, 255)));
			}
			Img = TwistImage(Img, true, Random.Next(1, 3), Random.Next(4, 6));
			g.DrawRectangle(new Pen(Color.LightGray, 1), 0, 0, ImageWidth - 1, (letterHeight - 1));
			Image = Img;
		}


		/// <summary>
		/// 字体随机颜色
		/// </summary>
		private Color GetRandomColor()
        {
            Random RandomNum_First = new Random((int)DateTime.Now.Ticks);
            Thread.Sleep(RandomNum_First.Next(50));
            Random RandomNum_Sencond = new Random((int)DateTime.Now.Ticks);
            int int_Red = RandomNum_First.Next(180);
            int int_Green = RandomNum_Sencond.Next(180);
            int int_Blue = (int_Red + int_Green > 300) ? 0 : 400 - int_Red - int_Green;
            int_Blue = (int_Blue > 255) ? 255 : int_Blue;
            return Color.FromArgb(int_Red, int_Green, int_Blue);
        }

		/// <summary>
		/// 正弦曲线Wave扭曲图片
		/// </summary>
		/// <param name="srcBmp">图片路径</param>
		/// <param name="bXDir">如果扭曲则选择为True</param>
		/// <param name="dMultValue">波形的幅度倍数，越大扭曲的程度越高,一般为3</param>
		/// <param name="dPhase">波形的起始相位,取值区间[0-2*PI)</param>
		private Bitmap TwistImage(Bitmap srcBmp, bool bXDir, double dMultValue, double dPhase)
        {
            double PI = 6.283185307179586476925286766559;
            Bitmap destBmp = new Bitmap(srcBmp.Width, srcBmp.Height);
            Graphics graph = Graphics.FromImage(destBmp);
            graph.FillRectangle(new SolidBrush(Color.White), 0, 0, destBmp.Width, destBmp.Height);
            graph.Dispose();
            double dBaseAxisLen = bXDir ? (double)destBmp.Height : (double)destBmp.Width;
            for (int i = 0; i < destBmp.Width; i++)
            {
                for (int j = 0; j < destBmp.Height; j++)
                {
                    double dx = 0;
                    dx = bXDir ? (PI * (double)j) / dBaseAxisLen : (PI * (double)i) / dBaseAxisLen;
                    dx += dPhase;
                    double dy = Math.Sin(dx);
                    int nOldX = 0, nOldY = 0;
                    nOldX = bXDir ? i + (int)(dy * dMultValue) : i;
                    nOldY = bXDir ? j : j + (int)(dy * dMultValue);

                    Color color = srcBmp.GetPixel(i, j);
                    if (nOldX >= 0 && nOldX < destBmp.Width
                     && nOldY >= 0 && nOldY < destBmp.Height)
                    {
                        destBmp.SetPixel(nOldX, nOldY, color);
                    }
                }
            }
            srcBmp.Dispose();
            return destBmp;
        }
        #endregion
    }
}