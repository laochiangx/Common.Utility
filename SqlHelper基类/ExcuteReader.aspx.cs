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

public partial class ExcuteReader : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        //获取要执行的命令
        string sql = txtsql.Text;
        SqlCommand cmd = new SqlCommand();
        //定义对象资源保存的范围，一旦using范围结束，将释放对方所占的资源

            //调用执行方法，因为没有参数，所以最后一项直接设置为null
            //注意返回结果是SqlDataReader类型
            SqlDataReader mydr = SqlHelper.ExecuteReader(SqlHelper.ConnectionStringLocalTransaction, 
                CommandType.Text, sql, null);
            //将结果赋予DataList，作为其数据源
            DataList1.DataSource = mydr;
            //绑定数据
            DataList1.DataBind();
    }
}
