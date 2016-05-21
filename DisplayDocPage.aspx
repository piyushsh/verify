<%@ Page Language="VB" AutoEventWireup="false"   CodeFile="DisplayDocPage.aspx.vb" Inherits="DisplayPage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >

<head>
    <style type="text/css">
        .auto-style1
        {
            width: 800px;
        }
        .auto-style2
        {
            width: 917px;
        }
    </style>
</head>

<body>
    <form id="form1" runat="server">

    
    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        
        <br />
        <table class="auto-style1">
            <tr>
                <td>
        <asp:ImageButton ID="btnBack" runat="server" ImageUrl="~/App_Themes/Buttons/back-button.GIF" />
                </td>
                <td style="width: 700px;" class="auto-style2">
        <asp:Label ID="lblMsg" runat="server" Font-Bold="True" Font-Names="Arial" ForeColor="#B38700"
            Width="971px" Font-Size="11pt"></asp:Label>
                </td>
                <td>
        <asp:ImageButton ID="btnBack0" runat="server" ImageUrl="~/App_Themes/Buttons/back-button.GIF" />
                </td>
            </tr>
        </table>
        <br />

        <iframe id = "PageFrame" runat="server"   
            src= "http://server2.verifytechnologies.com/SupplierModule/?did=vtp-tki" frameborder=0 style="width: 100%; height: 700px" >

        </iframe>
        <br />
        </div>
    
    </form>
    
    </body>
    
    