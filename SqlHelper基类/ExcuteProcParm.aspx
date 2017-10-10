<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ExcuteProcParm.aspx.cs" Inherits="ExcuteProcParm" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>带参数存储过程</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
            <table style="width: 396px; height: 159px">
            <tr>
                <td>
                    参数名：<asp:TextBox ID="txtparm" runat="server" Width="256px" Text="id"></asp:TextBox></td>
            </tr>
            <tr>
                <td>
                    参数值：<asp:TextBox ID="txtvalue" runat="server" Width="252px" Text="1"></asp:TextBox></td>
            </tr>
            <tr>
                <td>
                    要执行的存储过程名：</td>
            </tr>
            <tr>
                <td>
                    <asp:TextBox ID="txtsqlexec" runat="server" Width="315px" Text="updateTableByParm"></asp:TextBox></td>
            </tr>
            <tr>
                <td>
                    <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="执行" Width="116px" /></td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
