using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

public partial class Show : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.QueryString["type"] != null && Request.QueryString["type"].ToString() != "")
        {
            string id, code;
            code = Request.QueryString["type"].ToString();
            id = StrOperation.Transform3(code);
            lblID.Text = id;
        }
    }
}
