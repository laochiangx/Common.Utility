<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ExcuteScalar.aspx.cs" Inherits="ExcuteScalar" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>无标题页</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table style="width: 431px">
            <tr>
                <td style="width: 177px">
                    要执行的语句</td>
            </tr>
            <tr>
                <td style="width: 177px">
                    <asp:TextBox ID="txtsql" runat="server" Width="283px" Text="select name from mytable where id=2"></asp:TextBox></td>
            </tr>
            <tr>
                <td style="width: 177px">
                    <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="执行" Width="90px" /></td>
            </tr>
            <tr>
                <td style="width: 177px">
                    返回的结果是</td>
            </tr>
            <tr>
                <td style="width: 177px">
                    <asp:TextBox ID="txtscalar" runat="server" Width="276px"></asp:TextBox></td>
            </tr>
        </table>
    
    </div>
    </form>
</body>
</html>
