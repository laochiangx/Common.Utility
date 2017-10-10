<%@ Page Language="C#" AutoEventWireup="true"  CodeFile="Default.aspx.cs" Inherits="_Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>SqlHelper使用示例代码</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="~/ExcuteSQL.aspx">简单SQL语句</asp:HyperLink>
        <br />
        <asp:HyperLink ID="HyperLink2" runat="server" NavigateUrl="~/ExcuteSQLParm.aspx">带参数的简单SQL语句</asp:HyperLink>
        <br />
        <asp:HyperLink ID="HyperLink3" runat="server" NavigateUrl="~/ExcuteProc.aspx">简单存储过程</asp:HyperLink>
        <br />
        <asp:HyperLink ID="HyperLink4" runat="server" NavigateUrl="~/ExcuteProcParm.aspx">带参数的存储过程</asp:HyperLink>
        <br />
        <asp:HyperLink ID="HyperLink5" runat="server" NavigateUrl="~/ExcuteScalar.aspx">只返回一个字段</asp:HyperLink>
        <br />
        <asp:HyperLink ID="HyperLink6" runat="server" NavigateUrl="~/ExcuteReader.aspx">返回数据集</asp:HyperLink></div>
    </form>
    <a href="http://www.51aspx.com" target="_blank">download from 51aspx.com</a>

</body>
</html>
