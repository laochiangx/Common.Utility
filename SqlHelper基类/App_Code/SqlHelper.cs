using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Collections;
using System.Data.SqlClient;

/// <summary>
/// 数据库的通用访问代码
/// 此类为抽象类，不允许实例化，在应用时直接调用即可
/// </summary>
public abstract class SqlHelper
{
    //获取数据库连接字符串，其属于静态变量且只读，项目中所有文档可以直接使用，但不能修改
    public static readonly string ConnectionStringLocalTransaction = ConfigurationManager.ConnectionStrings["pubsConnectionString"].ConnectionString;

    // 哈希表用来存储缓存的参数信息，哈希表可以存储任意类型的参数。
    private static Hashtable parmCache = Hashtable.Synchronized(new Hashtable());

    /// <summary>
    ///执行一个不需要返回值的SqlCommand命令，通过指定专用的连接字符串。
    /// 使用参数数组形式提供参数列表 
    /// </summary>
    /// <remarks>
    /// 使用示例：
    ///  int result = ExecuteNonQuery(connString, CommandType.StoredProcedure, "PublishOrders", new SqlParameter("@prodid", 24));
    /// </remarks>
    /// <param name="connectionString">一个有效的数据库连接字符串</param>
    /// <param name="commandType">SqlCommand命令类型 (存储过程， T-SQL语句， 等等。)</param>
    /// <param name="commandText">存储过程的名字或者 T-SQL 语句</param>
    /// <param name="commandParameters">以数组形式提供SqlCommand命令中用到的参数列表</param>
    /// <returns>返回一个数值表示此SqlCommand命令执行后影响的行数</returns>
    public static int ExecuteNonQuery(string connectionString, CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
    {

        SqlCommand cmd = new SqlCommand();

        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            //通过PrePareCommand方法将参数逐个加入到SqlCommand的参数集合中
            PrepareCommand(cmd, conn, null, cmdType, cmdText, commandParameters);
            int val = cmd.ExecuteNonQuery();

            //清空SqlCommand中的参数列表
            cmd.Parameters.Clear();
            return val;
        }
    }
    
    /// <summary>
    ///执行一条不返回结果的SqlCommand，通过一个已经存在的数据库连接 
    /// 使用参数数组提供参数
    /// </summary>
    /// <remarks>
    /// 使用示例：  
    ///  int result = ExecuteNonQuery(conn, CommandType.StoredProcedure, "PublishOrders", new SqlParameter("@prodid", 24));
    /// </remarks>
    /// <param name="conn">一个现有的数据库连接</param>
    /// <param name="commandType">SqlCommand命令类型 (存储过程， T-SQL语句， 等等。)</param>
    /// <param name="commandText">存储过程的名字或者 T-SQL 语句</param>
    /// <param name="commandParameters">以数组形式提供SqlCommand命令中用到的参数列表</param>
    /// <returns>返回一个数值表示此SqlCommand命令执行后影响的行数</returns>
    public static int ExecuteNonQuery(SqlConnection connection, CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
    {

        SqlCommand cmd = new SqlCommand();

        PrepareCommand(cmd, connection, null, cmdType, cmdText, commandParameters);
        int val = cmd.ExecuteNonQuery();
        cmd.Parameters.Clear();
        return val;
    }

    /// <summary>
    /// 执行一条不返回结果的SqlCommand，通过一个已经存在的数据库事物处理 
    /// 使用参数数组提供参数
    /// </summary>
    /// <remarks>
    /// 使用示例： 
    ///  int result = ExecuteNonQuery(trans, CommandType.StoredProcedure, "PublishOrders", new SqlParameter("@prodid", 24));
    /// </remarks>
    /// <param name="trans">一个存在的 sql 事物处理</param>
    /// <param name="commandType">SqlCommand命令类型 (存储过程， T-SQL语句， 等等。)</param>
    /// <param name="commandText">存储过程的名字或者 T-SQL 语句</param>
    /// <param name="commandParameters">以数组形式提供SqlCommand命令中用到的参数列表</param>
    /// <returns>返回一个数值表示此SqlCommand命令执行后影响的行数</returns>
    public static int ExecuteNonQuery(SqlTransaction trans, CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
    {
        SqlCommand cmd = new SqlCommand();
        PrepareCommand(cmd, trans.Connection, trans, cmdType, cmdText, commandParameters);
        int val = cmd.ExecuteNonQuery();
        cmd.Parameters.Clear();
        return val;
    }

    /// <summary>
    /// 执行一条返回结果集的SqlCommand命令，通过专用的连接字符串。
    /// 使用参数数组提供参数
    /// </summary>
    /// <remarks>
    /// 使用示例：  
    ///  SqlDataReader r = ExecuteReader(connString, CommandType.StoredProcedure, "PublishOrders", new SqlParameter("@prodid", 24));
    /// </remarks>
    /// <param name="connectionString">一个有效的数据库连接字符串</param>
    /// <param name="commandType">SqlCommand命令类型 (存储过程， T-SQL语句， 等等。)</param>
    /// <param name="commandText">存储过程的名字或者 T-SQL 语句</param>
    /// <param name="commandParameters">以数组形式提供SqlCommand命令中用到的参数列表</param>
    /// <returns>返回一个包含结果的SqlDataReader</returns>
    public static SqlDataReader ExecuteReader(string connectionString, CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
    {
        SqlCommand cmd = new SqlCommand();
        SqlConnection conn = new SqlConnection(connectionString);

        // 在这里使用try/catch处理是因为如果方法出现异常，则SqlDataReader就不存在，
        //CommandBehavior.CloseConnection的语句就不会执行，触发的异常由catch捕获。
        //关闭数据库连接，并通过throw再次引发捕捉到的异常。
        try
        {
            PrepareCommand(cmd, conn, null, cmdType, cmdText, commandParameters);
            SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            cmd.Parameters.Clear();
            return rdr;
        }
        catch
        {
            conn.Close();
            throw;
        }
    }

    /// <summary>
    /// 执行一条返回第一条记录第一列的SqlCommand命令，通过专用的连接字符串。 
    /// 使用参数数组提供参数
    /// </summary>
    /// <remarks>
    /// 使用示例：  
    ///  Object obj = ExecuteScalar(connString, CommandType.StoredProcedure, "PublishOrders", new SqlParameter("@prodid", 24));
    /// </remarks>
    /// <param name="connectionString">一个有效的数据库连接字符串</param>
    /// <param name="commandType">SqlCommand命令类型 (存储过程， T-SQL语句， 等等。)</param>
    /// <param name="commandText">存储过程的名字或者 T-SQL 语句</param>
    /// <param name="commandParameters">以数组形式提供SqlCommand命令中用到的参数列表</param>
    /// <returns>返回一个object类型的数据，可以通过 Convert.To{Type}方法转换类型</returns>
    public static object ExecuteScalar(string connectionString, CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
    {
        SqlCommand cmd = new SqlCommand();

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            PrepareCommand(cmd, connection, null, cmdType, cmdText, commandParameters);
            object val = cmd.ExecuteScalar();
            cmd.Parameters.Clear();
            return val;
        }
    }

    /// <summary>
    /// 执行一条返回第一条记录第一列的SqlCommand命令，通过已经存在的数据库连接。
    /// 使用参数数组提供参数
    /// </summary>
    /// <remarks>
    /// 使用示例： 
    ///  Object obj = ExecuteScalar(connString, CommandType.StoredProcedure, "PublishOrders", new SqlParameter("@prodid", 24));
    /// </remarks>
    /// <param name="conn">一个已经存在的数据库连接</param>
    /// <param name="commandType">SqlCommand命令类型 (存储过程， T-SQL语句， 等等。)</param>
    /// <param name="commandText">存储过程的名字或者 T-SQL 语句</param>
    /// <param name="commandParameters">以数组形式提供SqlCommand命令中用到的参数列表</param>
    /// <returns>返回一个object类型的数据，可以通过 Convert.To{Type}方法转换类型</returns>
    public static object ExecuteScalar(SqlConnection connection, CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
    {
        SqlCommand cmd = new SqlCommand();

        PrepareCommand(cmd, connection, null, cmdType, cmdText, commandParameters);
        object val = cmd.ExecuteScalar();
        cmd.Parameters.Clear();
        return val;
    }

    /// <summary>
    /// 缓存参数数组
    /// </summary>
    /// <param name="cacheKey">参数缓存的键值</param>
    /// <param name="cmdParms">被缓存的参数列表</param>
    public static void CacheParameters(string cacheKey, params SqlParameter[] commandParameters)
    {
        parmCache[cacheKey] = commandParameters;
    }

    /// <summary>
    /// 获取被缓存的参数
    /// </summary>
    /// <param name="cacheKey">用于查找参数的KEY值</param>
    /// <returns>返回缓存的参数数组</returns>
    public static SqlParameter[] GetCachedParameters(string cacheKey)
    {
        SqlParameter[] cachedParms = (SqlParameter[])parmCache[cacheKey];

        if (cachedParms == null)
            return null;

        //新建一个参数的克隆列表
        SqlParameter[] clonedParms = new SqlParameter[cachedParms.Length];

        //通过循环为克隆参数列表赋值
        for (int i = 0, j = cachedParms.Length; i < j; i++)
            //使用clone方法复制参数列表中的参数
            clonedParms[i] = (SqlParameter)((ICloneable)cachedParms[i]).Clone();

        return clonedParms;
    }

    /// <summary>
    /// 为执行命令准备参数
    /// </summary>
    /// <param name="cmd">SqlCommand 命令</param>
    /// <param name="conn">已经存在的数据库连接</param>
    /// <param name="trans">数据库事物处理</param>
    /// <param name="cmdType">SqlCommand命令类型 (存储过程， T-SQL语句， 等等。)</param>
    /// <param name="cmdText">Command text，T-SQL语句 例如 Select * from Products</param>
    /// <param name="cmdParms">返回带参数的命令</param>
    private static void PrepareCommand(SqlCommand cmd, SqlConnection conn, SqlTransaction trans, CommandType cmdType, string cmdText, SqlParameter[] cmdParms)
    {

        //判断数据库连接状态
        if (conn.State != ConnectionState.Open)
            conn.Open();

        cmd.Connection = conn;
        cmd.CommandText = cmdText;

        //判断是否需要事物处理
        if (trans != null)
            cmd.Transaction = trans;

        cmd.CommandType = cmdType;

        if (cmdParms != null)
        {
            foreach (SqlParameter parm in cmdParms)
                cmd.Parameters.Add(parm);
        }
    }

    /// <summary>
    /// 执行查询，返回结果集中的第一行第一列的值，忽略其他行列
    /// </summary>
    /// <param name="sql"></param>
    /// <returns></returns>
    public static object ExcuteScalar(string sql)
    {
        using (SqlConnection con = new SqlConnection(ConnectionStringLocalTransaction))
        {
            con.Open();
            SqlCommand cmd = new SqlCommand(sql, con);
            con.Close();
            return cmd.ExecuteScalar();           
        }
    }


    /// <summary>
    /// 执行查询
    /// </summary>
    /// <param name="sql">有效的sql语句</param>
    /// <param name="param">返回DataReader</param>
    /// <returns>返回DataReader</returns>
    public static SqlDataReader ExcuteReader(string sql, SqlParameter[] param)
    {
        SqlConnection con = new SqlConnection(ConnectionStringLocalTransaction);
        con.Open();
        SqlCommand cmd = new SqlCommand(sql, con);
        cmd.Parameters.AddRange(param);
        SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
        cmd.Parameters.Clear();
        return reader;
    }


    /// <summary>
    /// 执行查询
    /// </summary>
    /// <param name="sql">有效的sql语句</param>
    /// <returns>返回DataReader</returns>
    public static SqlDataReader ExcuteReader(string sql)
    {
        SqlConnection con = new SqlConnection(ConnectionStringLocalTransaction);
        con.Open();
        SqlCommand cmd = new SqlCommand(sql, con);
        return cmd.ExecuteReader(CommandBehavior.CloseConnection);
    }


    /// <summary>
    /// 执行查询的基方法
    /// </summary>
    /// <param name="sql">有效的sql语句</param>
    /// <returns>返回DataTable</returns>
    public static DataTable ExcuteDataQuery(string sql)
    {
        using (SqlConnection con = new SqlConnection(ConnectionStringLocalTransaction))
        {
            con.Open();
            SqlDataAdapter sda = new SqlDataAdapter(sql, con);
            DataTable table = new DataTable();
            sda.Fill(table);
            con.Close();
            return table;
        }
    }


    /// <summary>
    /// 执行增，删，改的基方法
    /// </summary>
    /// <param name="sql">有效的sql语句</param>
    /// <param name="param">参数集合</param>
    /// <returns>影响的行数</returns>
    public static int ExcuteNonQuery(string sql, SqlParameter[] param)
    {
        using (SqlConnection con = new SqlConnection(ConnectionStringLocalTransaction))
        {
            con.Open();
            SqlCommand cmd = new SqlCommand(sql, con);
            if (param != null)
            {
                cmd.Parameters.AddRange(param);
            }
            int count = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            con.Close();
            return count;
        }
    }

    //每页显示5条数据
    static int pageSize = 10;
    /// <summary>
    /// 查询共有多少行，然后直接返回总页码
    /// </summary>
    /// <returns></returns>
    public static int GetAllBookCount()
    {
        int num = 0;
        int pageCount = 0;
        string sql = "select count(0) from TB_BookInfo";
        num = Convert.ToInt32(ExcuteScalar(sql));
        pageCount = num % pageSize != 0 ? (num / pageSize) + 1 : num / pageSize;
        return pageCount;
    }

    /// <summary>
    /// 准备命令
    /// </summary>
    /// <param name="con"></param>
    /// <param name="cmd"></param>
    /// <param name="textcmd"></param>
    /// <param name="cmdType"></param>
    /// <param name="param"></param>
    public static void PreparedCommd(SqlConnection con, SqlCommand cmd, string textcmd, CommandType cmdType, SqlParameter[] param)
    {
        try
        {
            if (con.State != ConnectionState.Open)
            {
                con.Open();
            }
            cmd.Connection = con;
            cmd.CommandText = textcmd;
            cmd.CommandType = cmdType;

            if (param != null)
            {
                foreach (SqlParameter p in param)
                {
                    cmd.Parameters.Add(p);
                }
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    /// <summary>
    /// 执行增、删、改
    /// </summary>
    /// <param name="textcmd">sql语句或者存储过程</param>
    /// <param name="cmdType">类型</param>
    /// <param name="param">参数</param>
    /// <returns>返回int类型的数据</returns>
    public static int ExecuteNonQuery(string textcmd,SqlParameter[] param, CommandType cmdType)
    {
        using (SqlConnection con = new SqlConnection(ConnectionStringLocalTransaction))
        {
            SqlCommand cmd = new SqlCommand();
            PreparedCommd(con, cmd, textcmd, cmdType, param);
            int num = cmd.ExecuteNonQuery();
            return num;
        }
    }

    /// <summary>
    /// 读取一行一列的数据
    /// </summary>
    /// <param name="textmd"></param>
    /// <param name="cmdType"></param>
    /// <param name="param"></param>
    /// <returns></returns>
    public static object ExecuteScalar(string textmd, CommandType cmdType, SqlParameter[] param)
    {
        using (SqlConnection con = new SqlConnection(ConnectionStringLocalTransaction))
        {
            SqlCommand cmd = new SqlCommand();
            PreparedCommd(con, cmd, textmd, cmdType, param);
            return cmd.ExecuteScalar();
        }
    }

    /// <summary>
    /// 读取一行一列的数据
    /// </summary>
    /// <param name="textmd"></param>
    /// <param name="cmdType"></param>
    /// <param name="param"></param>
    /// <returns></returns>
    public static object ExecuteScalar(string SQL)
    {
        using (SqlConnection con = new SqlConnection(ConnectionStringLocalTransaction))
        {
            con.Open();
            SqlCommand cmd = new SqlCommand(SQL, con);
            return cmd.ExecuteScalar();
        }
    }

    /// <summary>
    /// 查询
    /// </summary>
    /// <param name="textcmd"></param>
    /// <param name="cmdType"></param>
    /// <param name="param"></param>
    /// <returns></returns>
    public static SqlDataReader ExecuteReader(string textcmd, CommandType cmdType, SqlParameter[] param)
    {
        SqlConnection con = new SqlConnection(ConnectionStringLocalTransaction);
        SqlCommand cmd = new SqlCommand();
        try
        {
            //PreparedCommd(con, cmd, textcmd, cmdType, param);
            SqlDataReader read = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            return read;
        }
        catch (Exception ex)
        {
            con.Close();
            throw new Exception(ex.Message);
        }
    }

    /// <summary>
    /// 查询返回DataTable
    /// </summary>
    /// <param name="sql"></param>
    /// <returns></returns>
    public static DataTable ExecuteReader(string sql)
    {
        SqlConnection con = new SqlConnection(ConnectionStringLocalTransaction);
        DataTable dt = new DataTable();
        try
        {
            SqlDataAdapter da = new SqlDataAdapter(sql, con);
            da.Fill(dt);
        }
        catch (Exception)
        {

            throw;
        }
        return dt;
    }

    /// <summary>
    /// 用户登录
    /// </summary>
    /// <param name="UserName"></param>
    /// <param name="UserPwd"></param>
    /// <returns></returns>
    public static int CheckLogin(string UserName, string UserPwd)
    {
        int num = 0;
        try
        {
            string sql = "select * from TB_UserInfo  where [user_Name]='" + UserName + "' and user_pwd='" + UserPwd + "'";
            num = Convert.ToInt32(ExecuteScalar(sql));
        }
        catch (Exception ex)
        {
            throw ex;
        }
        return num;
    }

   
}