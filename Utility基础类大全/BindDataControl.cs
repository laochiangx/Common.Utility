using System.Web.UI.WebControls;
using System.Web.UI;
using System.Data;
using System.Data.SqlClient;

namespace Utilities
{
    /// <summary>
    /// 数据展示控件 绑定数据类
    /// </summary>
    public class BindDataControl
    {
        #region 绑定服务器数据控件 简单绑定DataList
        /// <summary>
        /// 简单绑定DataList
        /// </summary>
        /// <param name="ctrl">控件ID</param>
        /// <param name="mydv">数据视图</param>
        public static void BindDataList(Control ctrl, DataView mydv)
        {
            ((DataList)ctrl).DataSourceID = null;
            ((DataList)ctrl).DataSource = mydv;
            ((DataList)ctrl).DataBind();
        }
        #endregion

        #region 绑定服务器数据控件 SqlDataReader简单绑定DataList
        /// <summary>
        /// SqlDataReader简单绑定DataList
        /// </summary>
        /// <param name="ctrl">控件ID</param>
        /// <param name="mydv">数据视图</param>
        public static void BindDataReaderList(Control ctrl, SqlDataReader mydv)
        {
            ((DataList)ctrl).DataSourceID = null;
            ((DataList)ctrl).DataSource = mydv;
            ((DataList)ctrl).DataBind();
        }
        #endregion

        #region 绑定服务器数据控件 简单绑定GridView
        /// <summary>
        /// 简单绑定GridView
        /// </summary>
        /// <param name="ctrl">控件ID</param>
        /// <param name="mydv">数据视图</param>
        public static void BindGridView(Control ctrl, DataView mydv)
        {
            ((GridView)ctrl).DataSourceID = null;
            ((GridView)ctrl).DataSource = mydv;
            ((GridView)ctrl).DataBind();
        }
        #endregion

        /// <summary>
        /// 绑定服务器控件 简单绑定Repeater
        /// </summary>
        /// <param name="ctrl">控件ID</param>
        /// <param name="mydv">数据视图</param>
        public static void BindRepeater(Control ctrl, DataView mydv)
        {
            ((Repeater)ctrl).DataSourceID = null;
            ((Repeater)ctrl).DataSource = mydv;
            ((Repeater)ctrl).DataBind();
        }
    }
}
