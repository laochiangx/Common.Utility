using System;
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

public partial class _Default : System.Web.UI.Page 
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        string id, disguise, random;
        random = StrOperation.RandString();
        disguise = StrOperation.DisGuise();
        id = StrOperation.DeTransform3(txtID.Text.Trim().ToString());
        Response.Redirect("Show.aspx?type=" + id + "&target=" + random + "&id=" + disguise);
    }
}
