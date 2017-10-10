using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;

public partial class ExcuteSQLParm : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        //初始化参数
        SqlParameter myparm = new SqlParameter();
        //获取参数的名字
        myparm.ParameterName = txtparm.Text;
        //设置变量的类型和长度
        myparm.SqlDbType = SqlDbType.NVarChar;
        myparm.Size = 20;
        //获取参数的值
        myparm.Value = txtvalue.Text;
        //获取要执行的命令
        string sql = txtsql.Text;
        SqlCommand cmd = new SqlCommand();
        //定义对象资源保存的范围，一旦using范围结束，将释放对方所占的资源
        using (SqlConnection conn = new SqlConnection(SqlHelper.ConnectionStringLocalTransaction))
        {
            //打开连接
            conn.Open();
            //调用执行方法
            SqlHelper.ExecuteNonQuery(conn, CommandType.Text, sql, myparm);
            
            Response.Write("<font color=red>操作完成！请检查数据库！</font>");
        }
    }
}
