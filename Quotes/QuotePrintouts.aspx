<%@ Page Title="" Language="VB" MasterPageFile="~/Quotes/FormsMaster.master" AutoEventWireup="false" CodeFile="QuotePrintouts.aspx.vb" Inherits="Quotes_QuotePrintouts" %>
<%@ MasterType TypeName= "MyMasterPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="DetailsMaster_ContentPlaceHolder" Runat="Server">
    <table style="width: 690px">
        <tr>
            <td>
                    <table style="font-size: 10pt; width: 690px; font-family: Arial">
                        <tr>
                            <td align="left" style="border-right: darkgray 1px solid; border-top: darkgray 1px solid;
                border-left: darkgray 1px solid; border-bottom: darkgray 1px solid; height: 120px;" 
                                valign="top">
                                <span style="font-size: 11pt"><strong>
                                <asp:Label ID="Label9" runat="server" font-bold="True" font-names="Arial" 
                                    font-size="11pt" forecolor="#B38700" text="Printouts" width="205px"></asp:Label>
                                <br />
                                <table style="font-size: 10pt; width: 690px; font-family: Arial">
                                 
                                    <tr>
                                        <td align="left" 
                                            style="vertical-align: top; width: 199px; text-align: right; height: 18px;" 
                                            valign="top">
                                            <b><span style="font-size: 11pt"><strong>
                                            <asp:Label ID="lblBatchCaption1" runat="server" Font-Bold="True" 
                                                Font-Names="Arial" Font-Size="10pt" Width="170px">Select Template:</asp:Label>
                                            </strong></span></b>
                                        </td>
                                        <td align="left" style="height: 18px;" valign="top" colspan="2">
                                            <asp:DropDownList ID="ddlReports_NS" runat="server" Height="23px" Width="287px">
                                            </asp:DropDownList>
                                        </td>
                                        <td align="left" style="width: 200px; height: 18px;" valign="top" colspan="2">
                                            &nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td align="left" style="vertical-align: top; width: 199px; text-align: right" 
                                            valign="top">
                                            &nbsp;</td>
                                        <td align="left" valign="top" colspan="4">
                                            &nbsp;</td>
                                    </tr>
                                   
                                    <tr>
                                        <td align="left" style="vertical-align: top; width: 199px; text-align: left" 
                                            valign="top">
                                <span style="font-size: 11pt"><strong>
                                            <b>
                                            <asp:Label ID="lblBatchCaption2" runat="server" Font-Bold="True" 
                                                Font-Names="Arial" Font-Size="10pt" Width="170px">Header:</asp:Label>
                                            </b>
                                </strong></span>
                                        </td>
                                        <td align="left" valign="top" colspan="4">
                                            &nbsp;</td>
                                    </tr>
                                   
                                    <tr>
                                        <td align="left" style="vertical-align: top; text-align: left" 
                                            valign="top" colspan="5">
                                            <asp:TextBox ID="txtQuoteHeader" runat="server" style="margin-left: 0px" 
                                                Width="669px" Height="118px"></asp:TextBox>
                                        </td>
                                    </tr>
                                   
                                    <tr>
                                        <td align="left" style="vertical-align: top; width: 199px; text-align: left" 
                                            valign="top">
                                            &nbsp;</td>
                                        <td align="left" valign="top" colspan="4">
                                            &nbsp;</td>
                                    </tr>
                                   
                                    <tr>
                                        <td align="left" style="vertical-align: top; width: 199px; text-align: left" 
                                            valign="top">
                                <span style="font-size: 11pt"><strong>
                                            <b>
                                            <asp:Label ID="lblBatchCaption3" runat="server" Font-Bold="True" 
                                                Font-Names="Arial" Font-Size="10pt" Width="170px">Body:</asp:Label>
                                            </b>
                                </strong></span>
                                        </td>
                                        <td align="left" valign="top" colspan="4">
                                            &nbsp;</td>
                                    </tr>
                                   
                                    <tr>
                                        <td align="left" style="vertical-align: top; text-align: left; " 
                                            valign="top" colspan="5">
                                <span style="font-size: 11pt"><strong>
                                            <asp:TextBox ID="txtQuoteBody" runat="server" style="margin-left: 0px" 
                                                Width="669px" Height="118px"></asp:TextBox>
                                </strong></span>
                                        </td>
                                    </tr>
                                   
                                    <tr>
                                        <td align="left" style="vertical-align: top; width: 199px; text-align: left; " 
                                            valign="top">
                                            &nbsp;</td>
                                        <td align="left" valign="top" colspan="4">
                                            &nbsp;</td>
                                    </tr>
                                   
                                    <tr>
                                        <td align="left" style="vertical-align: top; width: 199px; text-align: left; " 
                                            valign="top">
                                <span style="font-size: 11pt"><strong>
                                            <b>
                                            <asp:Label ID="lblBatchCaption4" runat="server" Font-Bold="True" 
                                                Font-Names="Arial" Font-Size="10pt" Width="170px">Footer:</asp:Label>
                                            </b>
                                </strong></span>
                                        </td>
                                        <td align="left" valign="top" colspan="4">
                                            &nbsp;</td>
                                    </tr>
                                   
                                    <tr>
                                        <td align="left" style="vertical-align: top; text-align: left; " 
                                            valign="top" colspan="5">
                                <span style="font-size: 11pt"><strong>
                                            <asp:TextBox ID="txtQuoteFooter" runat="server" style="margin-left: 0px" 
                                                Width="669px" Height="118px"></asp:TextBox>
                                </strong></span>
                                        </td>
                                    </tr>
                                   
                                    <tr>
                                        <td align="left" style="vertical-align: top; width: 199px; text-align: left; " 
                                            valign="top">
                                            &nbsp;</td>
                                        <td align="left" valign="top" colspan="4">
                                            &nbsp;</td>
                                    </tr>
                                   
                                    <tr>
                                        <td align="left" style="vertical-align: top; width: 199px; text-align: left; " 
                                            valign="top">
                                            &nbsp;</td>
                                        <td align="left" valign="top">
                                            <asp:ImageButton ID="btnPrint" runat="server" 
                                                ImageUrl="~/App_Themes/Billing/Buttons/Print.gif" />
                                        </td>
                                        <td align="left" valign="top" colspan="2">
                                            &nbsp;</td>
                                        <td align="left" valign="top">
                                            &nbsp;</td>
                                    </tr>
                                    </table>
                                </strong></span>
                            </td>
                        </tr>
                    </table>
                </td>
        </tr>
        <tr>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td>
                &nbsp;</td>
        </tr>
    </table>
</asp:Content>

