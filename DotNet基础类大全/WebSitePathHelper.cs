using System.Web;

namespace DotNet.Utilities
{
    /// <summary>
    /// 网站路径操作类
    /// </summary>
    public static class WebSitePathHelper
    {
        /// <summary>
        /// 
        /// </summary>
        public enum SortType
        {
            /// <summary>
            /// 
            /// </summary>
            Photo = 1,
            /// <summary>
            /// 
            /// </summary>
            Article = 5,
            /// <summary>
            /// 
            /// </summary>
            Diary = 7,
            /// <summary>
            /// 
            /// </summary>
            Pic = 2,
            /// <summary>
            /// 
            /// </summary>
            Music = 6,
            /// <summary>
            /// 
            /// </summary>
            AddressList = 4,
            /// <summary>
            /// 
            /// </summary>
            Favorite = 3,
        }
        #region 根据给出的相对地址获取网站绝对地址
        /// <summary>
        /// 根据给出的相对地址获取网站绝对地址
        /// </summary>
        /// <param name="localPath">相对地址</param>
        /// <returns>绝对地址</returns>
        public static string GetWebPath(string localPath)
        {
            string path = HttpContext.Current.Request.ApplicationPath;
            string thisPath;
            string thisLocalPath;
            //如果不是根目录就加上"/" 根目录自己会加"/"
            if (path != "/")
            {
                thisPath = path + "/";
            }
            else
            {
                thisPath = path;
            }
            if (localPath.StartsWith("~/"))
            {
                thisLocalPath = localPath.Substring(2);
            }
            else
            {
                return localPath;
            }
            return thisPath + thisLocalPath;
        }

        #endregion

        #region 获取网站绝对地址
        /// <summary>
        ///  获取网站绝对地址
        /// </summary>
        /// <returns></returns>
        public static string GetWebPath()
        {
            string path = System.Web.HttpContext.Current.Request.ApplicationPath;
            string thisPath;
            //如果不是根目录就加上"/" 根目录自己会加"/"
            if (path != "/")
            {
                thisPath = path + "/";
            }
            else
            {
                thisPath = path;
            }
            return thisPath;
        }
        #endregion

        #region 根据相对路径或绝对路径获取绝对路径
        /// <summary>
        /// 根据相对路径或绝对路径获取绝对路径
        /// </summary>
        /// <param name="localPath">相对路径或绝对路径</param>
        /// <returns>绝对路径</returns>
        public static string GetFilePath(string localPath)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(localPath, @"([A-Za-z]):\\([\S]*)"))
            {
                return localPath;
            }
            else
            {
                return System.Web.HttpContext.Current.Server.MapPath(localPath);
            }
        }
        #endregion
    }
}
