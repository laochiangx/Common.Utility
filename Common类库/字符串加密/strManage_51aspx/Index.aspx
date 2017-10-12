<%@ Page language="c#" Inherits="StringOperation.Index" CodeFile="Index.aspx.cs" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>Index</title>
		<meta name="GENERATOR" Content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" Content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
	</HEAD>
	<body>
		<form id="Form1" method="post" runat="server">
			<FONT face="宋体">
				<asp:TextBox id="TextBox1" runat="server"></asp:TextBox><BR>
				<BR>
				<asp:Button id="Button1" runat="server" Text="转换字符" onclick="Button1_Click"></asp:Button>
				<asp:Button id="Button2" runat="server" Text="转换字符(Reverse)" onclick="Button2_Click"></asp:Button><BR>
				<asp:Button id="Button3" runat="server" Text="加密" onclick="Button3_Click"></asp:Button>
				<asp:Button id="Button6" runat="server" Text="解密" onclick="Button6_Click"></asp:Button><BR>
				<asp:Button id="Button4" runat="server" Text="SHA1加密" onclick="Button4_Click"></asp:Button>
				<asp:Button id="Button5" runat="server" Text="MD5加密" onclick="Button5_Click"></asp:Button><BR>
				<asp:Button id="Button7" runat="server" Text="加密方式一" onclick="Button7_Click"></asp:Button>
				<asp:Button id="Button8" runat="server" Text="解密方式一" onclick="Button8_Click"></asp:Button><BR>
				<asp:Button id="Button10" runat="server" Text="加密方式三" onclick="Button10_Click"></asp:Button>
				<asp:Button id="Button11" runat="server" Text="解密方式三" onclick="Button11_Click"></asp:Button><BR>
				<asp:Button id="Button12" runat="server" Text="反转字符" onclick="Button12_Click"></asp:Button><BR>
				<BR>
			</FONT>
		</form>
	</body>
</HTML>
