<%@ Page Language="vb" AutoEventWireup="false" Inherits="CashManager.PortalLogin2" CodeFile="PortalLogin2.aspx.vb" %>
<!DOCTYPE HTML PUBLIC "-//W3C//Dtd HTML 4.0 transitional//EN">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head2" runat="server">
    <title>Login - Sales & Dispatches Portal</title>
    <style type="text/css">
        .auto-style1
        {
            font-size: small;
        }
    </style>
</head>
	
		
	<body style="margin-top: 0px; margin-left: 0px;">


       

         <script type="text/javascript" src="<%= Session("_VT_JQueryFileLocation")%>Scripts/jquery-1.11.1.min.js"></script>


        
         <script type="text/javascript">

                   $(document).ready(function () {
                
                    //SmcN 27/05/2014 Insterted Javscript here to read the width of the Browser window
                    var screenwidth = screen.width;
                    var hdnBrowserWidth = document.getElementById('<%=hdnBrowserWidth.ClientID%>');
                    hdnBrowserWidth.value = screenwidth;
                
                    
            });

      </script>

	<center>
		<form id="Form1" method="post" runat="server">

              <asp:HiddenField ID="hdnBrowserWidth" runat="server" />
			
			<table id="table2" style="width: 900px; padding-right: 0px; padding-left: 0px; padding-bottom: 0px; margin: 0px; vertical-align: top; padding-top: 0px; position: static; text-align: left;" >
				<tr>
					<td style="height: 28px; padding-right: 0px; padding-left: 0px; padding-bottom: 0px; margin: 0px; padding-top: 0px; vertical-align: top; position: static; text-align: left; background-color: #C4D9B5;">
                        &nbsp; &nbsp;</td>
				</tr>
				<tr>
					<td style="height: 28px; padding-right: 0px; padding-left: 0px; padding-bottom: 0px; margin: 0px; padding-top: 0px; vertical-align: top; position: static; text-align: left;">
                        &nbsp; &nbsp;</td>
				</tr>
				<tr>
					<td style="height: 28px; padding-right: 0px; padding-left: 0px; padding-bottom: 0px; margin: 0px; padding-top: 0px; vertical-align: middle; position: static; text-align: center; font-family: Calibri; font-size: 28px; font-weight: normal; color: #014021; font-style: italic;">
                        Sales Management Portal</td>
				</tr>
				<tr>
					<td style="height: 28px; padding-right: 0px; padding-left: 0px; padding-bottom: 0px; margin: 0px; padding-top: 0px; vertical-align: top; position: static; text-align: center; font-family: Calibri; font-size: 28px; font-weight: normal; color: #014021;">
                        &nbsp;&nbsp; &nbsp;</td>
				</tr>
				<tr>
					<td style="height: 28px; padding-right: 0px; padding-left: 0px; padding-bottom: 0px; margin: 0px; padding-top: 0px; vertical-align: top; position: static; text-align: center;">

                        <div style="width: 500px; margin: 0 auto">

                        <table class="auto-style1" style="width: 500px; background-color: #C4D9B5;">
                            <tr>
                                <td style="background-color: #C4D9B5">&nbsp;</td>
                            </tr>
                            <tr>
                                <td>
                                    <table class="auto-style2" style="width: 500px">
                                        <tr>
                                            <td style="vertical-align: top; text-align: right; width: 200px;">
												<asp:label id="Label4" runat="server" ForeColor="Black" Font-Names="Arial" Font-Size="11pt"
													Font-Bold="True">User ID</asp:label></td>
                                            <td style="vertical-align: top; text-align: left; width: 6px;">&nbsp;&nbsp;</td>
                                            <td style="vertical-align: top; text-align: left">
												<asp:textbox id="txtUserName" tabIndex="1" runat="server" Font-Names="Arial" Width="300px" Height="20px"
													MaxLength="40" BorderStyle="None"></asp:textbox></td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <table class="auto-style2" style="width: 500px">
                                        <tr>
                                            <td style="vertical-align: top; text-align: right; width: 200px;">
												<asp:label id="Label3" runat="server" ForeColor="Black" Font-Names="Arial" Font-Size="11pt"
													Font-Bold="True">Password</asp:label></td>
                                            <td style="vertical-align: top; text-align: left; width: 6px;">&nbsp;&nbsp;</td>
                                            <td style="vertical-align: top; text-align: left">
												<asp:textbox id="txtPassword" tabIndex="2" runat="server" Width="300px" Height="20px" MaxLength="20"
													TextMode="Password" BorderStyle="None"></asp:textbox></td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp; &nbsp;</td>
                            </tr>
                            <tr>
                                <td style="vertical-align: top; text-align: right">
												<asp:button id="btnLogin" runat="server" Font-Names="Verdana" Width="72px" Text="Login" Font-Bold="true" CssClass="VT_ActionButton"></asp:button></td>
                            </tr>
                            <tr>
                                <td>&nbsp; &nbsp;<asp:label id="lblVer" runat="server" Width="90px" Height="9px" Font-Size="9pt" Font-Names="Arial" ForeColor="Black"></asp:label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; <asp:HyperLink ID="hypForgot" runat="server" Font-Names="Arial" Font-Size="9pt" NavigateUrl="ForgotPassword.aspx">HyperLink</asp:HyperLink>
                                </td>
                            </tr>
                            <tr style="font-family: Arial, Helvetica, sans-serif; font-size: 8px">
                                <td>&nbsp;&nbsp;&nbsp; &nbsp;</td>
                            </tr>
                        </table>

                        </div>
                    </td>
				</tr>
				<tr>
					<td style="height: 28px; padding-right: 0px; padding-left: 0px; padding-bottom: 0px; margin: 0px; padding-top: 0px; vertical-align: top; position: static; text-align: left;">
                        <asp:Label ID="lblURL" runat="server" Font-Names="Arial" Font-Size="6pt"></asp:Label>
                        </td>
				</tr>
				<tr>
					<td style="height: 28px; padding-right: 0px; padding-left: 0px; padding-bottom: 0px; margin: 0px; padding-top: 0px; vertical-align: top; position: static; text-align: center;">
                        <span style="font-family: Arial; vertical-align: top; text-align: center;">This is a Secure Site. Please use the <strong>
                                        User ID</strong> and <strong>Password</strong> you have been issued to log in.</span></td>
				</tr>
				<tr>
					<td style="height: 28px; padding-right: 0px; padding-left: 0px; padding-bottom: 0px; margin: 0px; padding-top: 0px; vertical-align: top; position: static; text-align: left;">
                        &nbsp;&nbsp;&nbsp;&nbsp; &nbsp;</td>
				</tr>
				<tr>
					<td style="height: 28px; padding-right: 0px; padding-left: 0px; padding-bottom: 0px; margin: 0px; padding-top: 0px; vertical-align: top; position: static; text-align: left;">
                        &nbsp;&nbsp; &nbsp;</td>
				</tr>

				<tr>
                <td id="MasterPage_FooterSection" style="vertical-align: middle; text-align: center; background-image: url('App_Themes/Footer/Footer_Background_bar_1.jpg'); background-repeat: repeat-x;">
                    
                                   
                    <asp:ImageMap ID="immPageFooter" runat="server" ImageUrl="~/App_Themes/Footer/PoweredByVerify_withBars.jpg">
                        <asp:RectangleHotSpot Bottom="84" Left="2" NavigateUrl="Http://www.VerifyTechnologies.com"
                                Right="245" Target="_blank" Top="2" />
                    </asp:ImageMap>


                </td>
            </tr>

				<tr>
					<td style="height: 28px; padding-right: 0px; padding-left: 0px; padding-bottom: 0px; margin: 0px; padding-top: 0px; vertical-align: top; position: static; text-align: left;">
                        &nbsp;&nbsp;</td>
				</tr>


	       </table>
		
		
        <br />
        
	
	
	</form>
		</center>
	</body>
</html>
