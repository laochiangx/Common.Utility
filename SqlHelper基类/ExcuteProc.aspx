<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ExcuteProc.aspx.cs" Inherits="ExcuteProc" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>执行简单存储过程（无参数）</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table style="width: 447px">
            <tr>
                <td style="height: 30px">
                    要执行的存储过程的名字（无参数）：</td>
            </tr>
            <tr>
                <td>
                    <asp:TextBox ID="TextBox1" runat="server" Width="167px" Text="updateTable"></asp:TextBox></td>
            </tr>
            <tr>
                <td>
                    <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="执行" Width="111px" /></td>
            </tr>
        </table>
    
    </div>
    </form>
</body>
</html>
