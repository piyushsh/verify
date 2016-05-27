<%@ Page Language="VB" AutoEventWireup="false" CodeFile="mPortalLogin.aspx.vb" Inherits="MobilePages_mPortalLogin" %>
<meta name=”viewport” content="width=device-width"/>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">

        .auto-style1
        {
            font-size: small;
        }
        .auto-style2 {
            height: 28px;
            position: static;
            width: 394px;
        }
        .auto-style3 {
            width: 200px;
        }
        .auto-style4 {
            width: 248px;
        }
        .auto-style5 {
            width: 248px;
            height: 30px;
        }
        .auto-style7 {
            width: 57px;
        }
        .auto-style8 {
            width: 52px;
        }
        .auto-style9 {
            width: 1px;
        }
        .auto-style10 {
            height: 28px;
            position: static;
            width: 200px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
			<table id="table2" style=" padding: 0px; margin: 0px; vertical-align: top; position: static; text-align: left; width: 359px; height: 450px;" >
				<tr>
					<td style="padding: 0px; margin: 0px; vertical-align: top; text-align: left; background-color: #C4D9B5;" class="auto-style10">
                        &nbsp; &nbsp;</td>
				</tr>
				<tr>
					<td style="padding: 0px; margin: 0px; vertical-align: top; text-align: left;" class="auto-style10">
                        &nbsp; &nbsp;</td>
				</tr>
				<tr>
					<td style="padding: 0px; margin: 0px; vertical-align: middle; text-align: left; font-family: Calibri; font-size: 18px; font-weight: normal; color: #014021; font-style: italic;" class="auto-style10">
                        Sales Management Portal</td>
				</tr>
				<tr>
					<td style="padding: 0px; margin: 0px; vertical-align: top; text-align: center; font-family: Calibri; font-weight: normal; color: #014021;" class="auto-style10">
                        &nbsp;&nbsp; &nbsp;</td>
				</tr>
				<tr>
					<td style="padding: 0px; margin: 0px; vertical-align: top; text-align: center;" class="auto-style10">

                        <div style=" margin: 0 auto">

                        <table class="auto-style1" style=" background-color: #C4D9B5;">
                            <tr>
                                <td style="background-color: #C4D9B5" class="auto-style4">&nbsp;</td>
                            </tr>
                            <tr>
                                <td class="auto-style4">
                                    <table class="auto-style2" >
                                        <tr>
                                            <td style="vertical-align: top; text-align: left;" class="auto-style8">
												<asp:label id="Label4" runat="server" ForeColor="Black" Font-Names="Arial" Font-Size="9pt"
													Font-Bold="True" Width="60px">User ID</asp:label></td>
                                            <td style="vertical-align: top; text-align: left; width: 6px;">&nbsp;&nbsp;</td>
                                            <td style="vertical-align: top; text-align: left">
												<asp:textbox id="txtUserName" tabIndex="1" runat="server" Font-Names="Arial" Width="170px" Height="20px"
													MaxLength="40" BorderStyle="None"></asp:textbox></td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td class="auto-style4">
                                    <table class="auto-style2" >
                                        <tr>
                                            <td style="vertical-align: top; text-align: left;" class="auto-style7">
												<asp:label id="Label3" runat="server" ForeColor="Black" Font-Names="Arial" Font-Size="9pt"
													Font-Bold="True" Width="60px">Password</asp:label></td>
                                            <td style="vertical-align: top; text-align: left; width: 6px;" class="auto-style9">&nbsp;&nbsp;</td>
                                            <td style="vertical-align: top; text-align: left">
												<asp:textbox id="txtPassword" tabIndex="2" runat="server" Width="170px" Height="20px" MaxLength="20"
													TextMode="Password" BorderStyle="None"></asp:textbox></td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td class="auto-style4">&nbsp; &nbsp;</td>
                            </tr>
                            <tr>
                                <td style="vertical-align: top; text-align: left" class="auto-style5">
												<asp:button id="btnLogin" runat="server" Font-Names="Verdana" Width="72px" Text="Login" Font-Bold="true" CssClass="VT_ActionButton"></asp:button></td>
                            </tr>
                            <tr>
                                <td class="auto-style4">&nbsp; &nbsp;<asp:label id="lblVer" runat="server" Height="9px" Font-Size="9pt" Font-Names="Arial" ForeColor="Black"></asp:label></td>
                            </tr>
                        </table>

                        </div>
                    </td>
				</tr>
				<tr>
					<td style="padding: 0px; margin: 0px; vertical-align: top; text-align: left;" class="auto-style10">
                        <asp:Label ID="lblURL" runat="server" Font-Names="Arial" Font-Size="6pt"></asp:Label>
                        </td>
				</tr>
				

				<tr>
                <td id="MasterPage_FooterSection" style="vertical-align: middle; text-align: left;" class="auto-style3">
                    
                                   
                    <asp:ImageMap ID="immPageFooter" runat="server" ImageUrl="~/App_Themes/Footer/PoweredByVerify_withBars.jpg">
                        <asp:RectangleHotSpot Bottom="84" Left="2" NavigateUrl="Http://www.VerifyTechnologies.com"
                                Right="245" Target="_blank" Top="2" />
                    </asp:ImageMap>


                </td>
            </tr>

				<tr>
					<td style="padding: 0px; margin: 0px; vertical-align: top; text-align: left;" class="auto-style10">
                        &nbsp;&nbsp;</td>
				</tr>


	       </table>
		
		
    </div>
    </form>
</body>
</html>
