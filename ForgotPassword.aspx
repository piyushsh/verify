<%@ Page Language="VB" AutoEventWireup="false" CodeFile="ForgotPassword.aspx.vb" Inherits="ForgotPassword" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="jg_styles.css" rel="stylesheet" type="text/css" />
    <link href="App_Themes/VT_Responsive_V1.css" rel="stylesheet" type="text/css" />
    <link href="App_Themes/Verify_Infragistics_Responsive_Ver2.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript" >
        function GetPassword() {
            if (document.getElementById("<%=txtLogin.ClientID%>").value == '') {
                alert('You must supply a UserId ');
                return false;
            }
        }

        function HideModalPopupMsg() {
            var modal = $find("<%=ModalPopupExtenderMsg.BehaviorID%>");
            modal.hide();
        }
    </script>
    <style type="text/css">
        .auto-style1 {
            height: 26px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <div style="padding: 20px; margin: 20px;">



            <asp:Image ID="imgBannerLogo" runat="server" ImageUrl="~/App_Themes/TabButtons/VerifyLogo_Responsive.jpg" />

            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
												<asp:Button ID="btnBack" runat="server" Font-Names="Verdana" Width="72px" Text="Back" Font-Bold="true" CssClass="VT_ActionButton"></asp:Button>
            <br />
            <br />
            <br />
            <table class="auto-style1" style="width: 850px; text-align: center; background-color: #C4D9B5;">
                <tr>
                    <td class="auto-style1">
                        </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblForgot" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="12pt"></asp:Label>
                        &nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:Label ID="lblInst" runat="server" Font-Names="Arial" Font-Size="10pt"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style1">&nbsp;&nbsp;
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblUser" runat="server" Font-Names="Arial" Font-Size="10pt">User Id:</asp:Label>
                        &nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:TextBox ID="txtLogin" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr style="font-family: Arial, Helvetica, sans-serif; font-size: 8px">
                    <td>
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;</td>
                </tr>
                <tr>
                    <td style="text-align: center">
                        <asp:Button ID="btnSend" runat="server" Font-Names="Verdana" Width="250px" Text="Send me my Password" Font-Bold="true" CssClass="VT_ActionButton"></asp:Button></td>
                </tr>
                <tr>
                    <td style="text-align: center">
                        &nbsp;</td>
                </tr>
            </table>

        </div>

        <asp:Panel ID="pnlMsg" runat="server" BackColor="White" Width="750px" Style="display:none "
            meta:resourcekey="pnlCommentResource1">
            <div style="padding: 20px">
                <center>
                    <br />
                    <asp:Label ID="lblMsg" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="11pt"
                        ForeColor="#996633" Text="Some items are missing from this form!"></asp:Label>
                    <br />
                    <br />
                    <br />
                    <asp:ImageButton ID="cmdOKMissing" runat="server" ImageUrl="~/App_Themes/Buttons/Ok.gif"
                        meta:resourceKey="cmdOKStatusResource1" />
                    <br />
                    <br />
                </center>
            </div>
        </asp:Panel>
        <cc1:ModalPopupExtender ID="ModalPopupExtenderMsg" TargetControlID="pnlMsg" CancelControlID="cmdOkMissing"
            BackgroundCssClass="modalBackground" DropShadow="True" OnCancelScript="HideModalPopupMsg()"
            PopupControlID="pnlMsg" runat="server" DynamicServicePath="" Enabled="True">
        </cc1:ModalPopupExtender>

    </form>
</body>
</html>
