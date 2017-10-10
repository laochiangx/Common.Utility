<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ExcuteSQL.aspx.cs" Inherits="ExcuteSQL" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>无标题页</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        &nbsp;<table style="width: 534px; height: 143px">
            <tr>
                <td style="width: 308px">
                    要执行的语句（添加、删除、更新）：</td>
            </tr>
            <tr>
                <td style="width: 308px">
                    <asp:TextBox ID="TextBox1" runat="server" Width="502px" Text="update myTable set name='my51aspx' where id=1"></asp:TextBox></td>
            </tr>
            <tr>
                <td style="width: 308px">
                    <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="执行" Width="97px" /></td>
            </tr>
        </table>
    
    </div>
    </form>
    <a href="http://www.51aspx.com" target="_blank">download from 51aspx.com</a>

</body>
</html>

