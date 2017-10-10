using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Text;

public partial class _Default : System.Web.UI.Page 
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }


    /// <summary>
    /// 查询 Users 对象集合
    /// </summary>
    /// <returns>返回List集合</returns>
    public List<Users> findAllUsers()
    {
        string sql = "select * from _Users";
        List<Users> userlist = new List<Users>();
        DataTable table = SqlHelper.ExcuteDataQuery(sql);
        foreach (DataRow row in table.Rows)
        {
            Users user = new Users();
            user.Id = Convert.ToInt32(row["Id"]);
            user.Username = Convert.ToString(row["username"]);
            user.Password = Convert.ToString(row["password"]);


            userlist.Add(user);
        }

        return userlist;
    }
    /// <summary>
    /// 带参数(string UserName)查出要添加的用户是否重复 对象集合
    /// </summary>
    /// <param name="UserName"></param>
    /// <returns></returns>
    public string checkUserByUserName(string username)
    {
        string sql = string.Format("select username from _Users where username='{0}'", username);
        string uname = Convert.ToString(SqlHelper.ExcuteScalar(sql));
        return uname;
    }

    /// <summary>
    /// 登录
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    public int login(Users user)
    {
        string sql = string.Format("select count(*) from _Users where username='{0}' and password='{1}'", user.Username, user.Password);
        int x = (int)SqlHelper.ExcuteScalar(sql);
        return x;

    }
    /// <summary>
    /// 添加用户
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    public int addUser(Users user)
    {
        string sql = "insert into _Users values(@username,@password)";
        SqlParameter[] param = new SqlParameter[] { 
                new SqlParameter("@username",user.Username),
                new SqlParameter("@password",user.Password)
            };
        return SqlHelper.ExcuteNonQuery(sql, param);
    }


    /// <summary>
    /// 用户登录
    /// </summary>
    /// <param name="UserName"></param>
    /// <param name="UserPwd"></param>
    /// <returns></returns>
    public object CheckLogin(string UserName, string UserPwd)
    {
        int num = 0;
        object rs = null;
        num = SqlHelper.CheckLogin(UserName, UserPwd);
        if (num > 0)
        {
            Session["UserName"] = UserName;
            if (Session["UserName"] != null)
            {
                rs = Session["UserName"].ToString().Trim();
            }
        }
        else
        {
            rs = num;
        }
        return rs;
    }

    /// <summary>
    /// 将DataTable转Json返回前台
    /// </summary>
    /// <returns></returns>
    public string CreateJsonParameters(string sql)
    {
        DataTable dt = SqlHelper.ExcuteDataQuery(sql);
        StringBuilder JsonString = new StringBuilder();
        if (dt != null && dt.Rows.Count > 0)
        {
            JsonString.Append("{ ");
            JsonString.Append("\"TableInfo\":[ ");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                JsonString.Append("{ ");
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    if (j < dt.Columns.Count - 1)
                    {
                        if (j == 4)
                        {
                            JsonString.Append("\"" + dt.Columns[j].ColumnName.ToString() + "\":" + "\"" + FrmatTime(Convert.ToDateTime(dt.Rows[i][j])) + "\",");
                        }
                        else
                        {
                            JsonString.Append("\"" + dt.Columns[j].ColumnName.ToString() + "\":" + "\"" + dt.Rows[i][j].ToString() + "\",");
                        }
                        if (j == 2)
                        {
                            JsonString.Append("\"" + dt.Columns[j].ColumnName.ToString() + "\":" + "\"" + CutString30((dt.Rows[i][j]).ToString()) + "\",");
                        }
                    }
                    else if (j == dt.Columns.Count - 1)
                    {
                        JsonString.Append("\"" + dt.Columns[j].ColumnName.ToString() + "\":" + "\"" + CutString(dt.Rows[i][j].ToString()) + "\"");
                    }
                }
                /**/
                /*end Of String*/
                if (i == dt.Rows.Count - 1)
                {
                    JsonString.Append("} ");
                }
                else
                {
                    JsonString.Append("}, ");
                }
            }
            JsonString.Append("]}");
            return JsonString.ToString();
        }
        else
        {
            return null;
        }
    }

    /// <summary>
    /// 截取字符串20
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static string CutString(string str)
    {
        string RStr = "";
        if (str != "" && str != null)
        {
            if (str.Length > 20)
            {
                RStr = str.Substring(0, 20) + "……";
            }
            else
            {
                RStr = str;
            }
        }
        return RStr;
    }

    /// <summary>
    /// 截取字符串20
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static string CutString30(string str)
    {
        string RStr = "";
        if (str != "" && str != null)
        {
            if (str.Length > 5)
            {
                RStr = str.Substring(0, 5) + "……";
            }
            else
            {
                RStr = str;
            }
        }
        return RStr;
    }

    /// <summary>
    /// DateTime类型格式化
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    public static string FrmatTime(DateTime time)
    {
        string Ftime = "";
        Ftime = time.ToString("yyyy年MM月dd日");
        return Ftime;
    }

    /// <summary>
    /// 获取总页码
    /// </summary>
    /// <returns></returns>
    public static int GetPageCount()
    {
        int num = SqlHelper.GetAllBookCount();
        return num;
    }

    //判断Session是否为空
    public int CheckSession()
    {
        int num = 0;
        if (Session["UserName"] != null)
        {
            num = 1;
        }
        else
        {
            num = 0;
        }
        return num;
    }

    /// <summary>
    /// 清空所有Session
    /// </summary>
    /// <returns></returns>
    public int ClearSession()
    {
        Session.Clear();
        return 1;
    }
}
