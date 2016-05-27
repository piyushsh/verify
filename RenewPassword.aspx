<%@ Page Language="VB" AutoEventWireup="false" CodeFile="RenewPassword.aspx.vb" Inherits="RenewPassword" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="jg_styles.css" rel="stylesheet" type="text/css" />
    <link href="App_Themes/VT_Responsive_V1.css" rel="stylesheet" type="text/css" />
    <link href="App_Themes/Verify_Infragistics_Responsive_Ver2.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript" >
        function RenewPassword() {
            if ((document.getElementById("<%=txtPass.ClientID%>").value == '') || (document.getElementById("<%=txtConfirmPass.ClientID%>").value == ''))  {
                alert('<%= Resources.Resource.NoPassword%>');
                return false;
            }
            if (document.getElementById("<%=txtPass.ClientID%>").value != document.getElementById("<%=txtConfirmPass.ClientID%>").value) {
                alert('<%= Resources.Resource.PasswordMustMatch%>');
                return false;
            }
            
            var hdn = document.getElementById("<%=hdnPasswordComplex.ClientID%>")
            if (hdn.value == 'YES')
                if (!checkPassword(document.getElementById("<%=txtPass.ClientID%>").value)) {
                    alert('<%= Resources.Resource.PasswordNotStrong%>');
                    document.getElementById("<%=txtPass.ClientID%>").focus();
                    return false;
                }
        }

        function checkPassword(str) {
            var re = new RegExp(document.getElementById("<%=hdnPasswordCheckRegEx.ClientID%>").value);
            return re.test(str);
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
        .auto-style2 {
            text-align: right;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>

        <asp:HiddenField ID="hdnPasswordComplex" runat="server" />
        <asp:HiddenField ID="hdnPasswordCheckRegEx" runat="server" />
        <asp:HiddenField ID="hdnPasswordCannotReuse" runat="server" />

        <div style="padding: 20px; margin: 60px;">



            <asp:Image ID="imgBannerLogo" runat="server" ImageUrl="~/App_Themes/TabButtons/VerifyLogo_Responsive.jpg" />

            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
												<asp:Button ID="btnBack" runat="server" Font-Names="Verdana" Width="72px" Text="Back" Font-Bold="true" CssClass="VT_ActionButton"></asp:Button>
            <br />
            <br />
            <br />
            <table class="auto-style1" style="width: 80%; background-color: #C4D9B5;">
                <tr>
                    <td style="text-align: center" colspan="2">
                        <asp:Label ID="lblExpired" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="12pt"></asp:Label>
                        
                    </td>
                </tr>
                <tr>
                    <td style="text-align: center" colspan="2">
                    <asp:Label ID="lblInst" runat="server" Font-Names="Arial" Font-Size="10pt"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style1" colspan="2">&nbsp;&nbsp;
                    </td>
                </tr>
                <tr>
                    <td class="auto-style2">
                        <asp:Label ID="lblPass1" runat="server" Font-Names="Arial" Font-Size="10pt">New Password:</asp:Label>
                        &nbsp;&nbsp;&nbsp;&nbsp;
                    </td>
                    <td style="text-align: left">
                    <asp:TextBox ID="txtPass" runat="server" TextMode="Password"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style2">
                        <asp:Label ID="lblPass2" runat="server" Font-Names="Arial" Font-Size="10pt">Confirm Password:</asp:Label>
                        &nbsp;&nbsp;&nbsp;&nbsp;
                    </td>
                    <td style="text-align: left">
                    <asp:TextBox ID="txtConfirmPass" runat="server" TextMode="Password"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: center" colspan="2">
                        &nbsp;</td>
                </tr>
                <tr>
                    <td style="text-align: center" colspan="2">
                        <asp:Button ID="btnSet" runat="server" Font-Names="Verdana" Width="250px" Text="Change my Password" Font-Bold="true" CssClass="VT_ActionButton"></asp:Button></td>
                </tr>
                <tr>
                    <td style="text-align: center" colspan="2">
                        &nbsp;</td>
                </tr>
                <tr>
                    <td style="text-align: center" colspan="2">
                    <asp:Label ID="lblPasswordInst" runat="server" Font-Names="Arial" Font-Size="8pt"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: center" colspan="2">
                    <asp:Label ID="lblPasswordReuse" runat="server" Font-Names="Arial" Font-Size="8pt"></asp:Label>
                    </td>
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
