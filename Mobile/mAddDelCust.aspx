<%@ Page Title="" Language="VB" MasterPageFile="~/Mobile/mDetailsMaster.master" AutoEventWireup="false" CodeFile="mAddDelCust.aspx.vb" Inherits="Mobile_mAddDelCust" %>


<%@ Register assembly="Infragistics4.Web.v13.2, Version=13.2.20132.2028, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" namespace="Infragistics.Web.UI.DisplayControls" tagprefix="ig" %>


<%@ Register Assembly="Infragistics4.Web.v13.2, Version=13.2.20132.2028, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" namespace="Infragistics.Web.UI.DisplayControls" tagprefix="ig" %>
<%@ Register Assembly="Infragistics4.Web.v13.2, Version=13.2.20132.2028, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" namespace="Infragistics.Web.UI.EditorControls" tagprefix="ig" %>
<%@ Register Assembly="Infragistics4.Web.v13.2, Version=13.2.20132.2028, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" namespace="Infragistics.Web.UI.GridControls" tagprefix="ig" %>
<%@ Register Assembly="Infragistics4.Web.v13.2, Version=13.2.20132.2028, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" namespace="Infragistics.Web.UI.ListControls" tagprefix="ig" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
   
        <script type="text/javascript" src="..\scripts\json2.js">
    </script>
    <script type="text/javascript" src="..\Scripts\jquery-1.10.2.js"></script>
<script type="text/javascript" src="..\Scripts\jquery.tmpl.js"></script>
    <script type="text/javascript">

       

    </script>

     <style type="text/css">

.igte_EditWithButtons
{
	font-size: 10px;
	font-family: Segoe UI, Verdana, Arial, Sans-Serif;
	background-color: #FFFFFF;
	height:20px;
	width: 130px;
	outline: 0;
	border:solid 1px #999999;
}


.igte_EditWithButtons
{
	font-size: 10px;
	font-family: Segoe UI, Verdana, Arial, Sans-Serif;
	background-color: #FFFFFF;
	height:20px;
	width: 130px;
	outline: 0;
	border:solid 1px #999999;
}


.igte_EditWithButtons
{
	font-size: 10px;
	font-family: Segoe UI, Verdana, Arial, Sans-Serif;
	background-color: #FFFFFF;
	height:20px;
	width: 130px;
	outline: 0;
	border:solid 1px #999999;
}


.igte_EditWithButtons
{
	font-size: 10px;
	font-family: Segoe UI, Verdana, Arial, Sans-Serif;
	background-color: #FFFFFF;
	height:20px;
	width: 130px;
	outline: 0;
	border:solid 1px #999999;
}


.igte_EditWithButtons
{
	font-size: 10px;
	font-family: Segoe UI, Verdana, Arial, Sans-Serif;
	background-color: #FFFFFF;
	outline: 0;
	border:solid 1px #999999;
}


.igte_Inner
{
	border-width: 0;
}

.igte_Inner
{
	border-width: 0;
}

.igte_Inner
{
	border-width: 0;
}

.igte_Inner
{
	border-width: 0;
}


.igte_Inner
{
	border-width: 0;
}

.igte_EditInContainer
{
	font-size: 12px;
	font-family: Segoe UI, Verdana, Arial, Sans-Serif;
	background-color: Transparent;
	border-width:0px;
	outline:0;
}

.igte_EditInContainer
{
	font-size: 12px;
	font-family: Segoe UI, Verdana, Arial, Sans-Serif;
	background-color: Transparent;
	border-width:0px;
	outline:0;
}

.igte_EditInContainer
{
	font-size: 12px;
	font-family: Segoe UI, Verdana, Arial, Sans-Serif;
	background-color: Transparent;
	border-width:0px;
	outline:0;
}

.igte_EditInContainer
{
	font-size: 12px;
	font-family: Segoe UI, Verdana, Arial, Sans-Serif;
	background-color: Transparent;
	border-width:0px;
	outline:0;
}

.igte_EditInContainer
{
	font-size: 12px;
	font-family: Segoe UI, Verdana, Arial, Sans-Serif;
	background-color: Transparent;
	border-width:0px;
	outline:0;
}

.igte_Button
{
	background-color: #8EBEE0;
	background-image: url(C:/Sources/Sales Portal/SalesPortal Cross Browser/ig_res/Default/images/igte_spinbuttonbg.gif);
	background-repeat: repeat-x;
	line-height:normal;
	border:solid 1px #699BC9;
	color: #FFFFFF;
	cursor: pointer;
}

.igte_ButtonSize
{
	width:15px;
	height:100%;
}

.igte_Button
{
	background-color: #8EBEE0;
	background-image: url(C:/Sources/Sales Portal/SalesPortal Cross Browser/ig_res/Default/images/igte_spinbuttonbg.gif);
	background-repeat: repeat-x;
	line-height:normal;
	border:solid 1px #699BC9;
	color: #FFFFFF;
	cursor: pointer;
}

.igte_ButtonSize
{
	width:15px;
	height:100%;
}

.igte_Button
{
	background-color: #8EBEE0;
	background-image: url(C:/Sources/Sales Portal/SalesPortal Cross Browser/ig_res/Default/images/igte_spinbuttonbg.gif);
	background-repeat: repeat-x;
	line-height:normal;
	border:solid 1px #699BC9;
	color: #FFFFFF;
	cursor: pointer;
}

.igte_ButtonSize
{
	width:15px;
	height:100%;
}

.igte_Button
{
	background-color: #8EBEE0;
	background-image: url(C:/Sources/Sales Portal/SalesPortal Cross Browser/ig_res/Default/images/igte_spinbuttonbg.gif);
	background-repeat: repeat-x;
	line-height:normal;
	border:solid 1px #699BC9;
	color: #FFFFFF;
	cursor: pointer;
}

.igte_ButtonSize
{
	width:15px;
	height:100%;
}

.igte_ButtonSize
{
	width:15px;
	height:100%;
}

        .igte_Button
{
	background-color: #8EBEE0;
	background-image: url(C:/Sources/Sales Portal/SalesPortal Cross Browser/ig_res/Default/images/igte_spinbuttonbg.gif);
	background-repeat: repeat-x;
	line-height:normal;
	border:solid 1px #699BC9;
	color: #FFFFFF;
	cursor: pointer;
}

        .auto-style1 {
            width: 151px;
        }

    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mProjectMAIN_content" Runat="Server">
     <meta name="viewport" content="width=device-width">
       <div style="text-align: left">
              <asp:Label ID="Label12" runat="server" font-bold="True" font-names="Arial" 
                                    font-size="10pt" forecolor="#B38700" text="Add new delivery customer" 
                                    width="230px"></asp:Label>
                                <br />

        <table><tr><td>    <asp:Label ID="lblCust0" runat="server" Font-Bold="True" 
                                                Font-Names="Arial" Font-Size="9pt">Billing cust.:</asp:Label></td><td class="auto-style1"> 
                            <span style="font-size: 11pt"><strong>
                                            <asp:Label ID="lblBillingCust" runat="server" Font-Bold="True" 
                                                Font-Names="Arial" Font-Size="9pt" Height="16px" Width="77px"></asp:Label>
                                            </strong></span>
                                        </td><td>
                &nbsp;</td></tr>
            <tr><td>    <asp:Label ID="lblCust" runat="server" Font-Bold="True" 
                                                Font-Names="Arial" Font-Size="9pt">New cust name:</asp:Label></td><td class="auto-style1"> 
                <asp:TextBox ID="txtNewCust0" runat="server" Font-Size="9pt" Height="22px" Width="150px"></asp:TextBox>
                </td><td>
            <asp:Button ID="btnSave0" runat="server" CssClass="VT_ActionButton" Height="20px" Text="Save" Font-Size="9pt" />
                </td></tr>
            <tr><td>
                            <span style="font-size: 11pt"><strong>
                                            <asp:Label ID="lblJobIdCaption1" runat="server" Font-Bold="True" 
                                                Font-Names="Arial" Font-Size="9pt">Address:</asp:Label>
                                            </strong></span>
                            </td><td class="auto-style1">
                <asp:TextBox ID="txtAddress" runat="server" Font-Size="9pt" Height="22px" Width="150px"></asp:TextBox>
                                        </td><td>
                    &nbsp;</td></tr>
            <tr><td>
                            <span style="font-size: 11pt"><strong>
                                            <asp:Label ID="lblOrderDate0" runat="server" Font-Bold="True" 
                                                Font-Names="Arial" Font-Size="9pt">Contact name:</asp:Label>
                                            </strong></span>
                            </td><td class="auto-style1">
                <asp:TextBox ID="txtContact" runat="server" Font-Size="9pt" Height="22px" Width="150px"></asp:TextBox>
                </td><td>
                    &nbsp;</td></tr>
            <tr><td>
                            <span style="font-size: 11pt"><strong>
                                            <asp:Label ID="lblPhone" runat="server" Font-Bold="True" 
                                                Font-Names="Arial" Font-Size="9pt">Phone:</asp:Label>
                                            </strong></span>
                            </td><td class="auto-style1">
                <asp:TextBox ID="txtPhone" runat="server" Font-Size="9pt" Height="22px" Width="150px"></asp:TextBox>
                            </td><td></td></tr>
            <tr><td>
                            <span style="font-size: 11pt"><strong>
                                            <asp:Label ID="lblDelDate" runat="server" Font-Bold="True" 
                                                Font-Names="Arial" Font-Size="9pt" Width="80px">email:</asp:Label>
                                            </strong></span>
                            </td><td class="auto-style1">
                <asp:TextBox ID="txtemail" runat="server" Font-Size="9pt" Height="22px" Width="150px"></asp:TextBox>
                               
                               
                            </td><td>
                    &nbsp;</td></tr>
                        <tr><td>
                            &nbsp;</td><td class="auto-style1">
                               
                               
                               
                                &nbsp;</td><td>
                </td></tr>
        </table>
          
           </div>

</asp:Content>

