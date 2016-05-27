<%@ Page Language="VB" MasterPageFile="~/TabPages/ProjectMain.master" AutoEventWireup="false" CodeFile="SelectPrintout.aspx.vb" Inherits="Other_Pages_SelectPrintout" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ProjectMAIN_content" Runat="Server">

                  
                <br />
    <table style="width: 940px">
        <tr>
            <td>
                <table style="width: 935px">
                    <tr>
                        <td style="width: 241px">
                            &nbsp;</td>
                        <td style="width: 241px">
                            &nbsp;</td>
                        <td style="text-align: right">
                        <asp:ImageButton ID="btnBack" runat="server" ImageUrl="~/APP_THEMES/Billing/Buttons/Back-Button.gif" />
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 241px">
                            &nbsp;</td>
                        <td style="width: 241px">
                        <asp:Label ID="lblTypeTitle" runat="server" Font-Bold="True" 
                            style="font-size: 14pt; font-family: Arial">Order Number:</asp:Label>
                        </td>
                        <td>
                        <asp:Label ID="lblOrderNumber" runat="server" Font-Bold="True" 
                            style="font-size: 14pt; font-family: Arial"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 241px">
                            &nbsp;</td>
                        <td style="width: 241px">
                            &nbsp;</td>
                        <td>
                            <asp:Label ID="lblOrderId" runat="server" Font-Bold="False" Font-Names="Arial"
                                Font-Size="10pt" Width="144px" Visible="False"></asp:Label></td>
                    </tr>
                    <tr>
                        <td style="height: 23px; width: 241px;">
                            &nbsp;</td>
                        <td style="height: 23px; width: 241px;">
                            <asp:Label ID="Label9" runat="server" Font-Bold="True" Font-Size="10pt" 
                                Text="Select Order Printout Template:" Font-Names="Arial"></asp:Label>
                        </td>
                        <td style="height: 23px">
                            <asp:DropDownList ID="ddlReport" runat="server" Height="30px" Width="283px" 
                                AutoPostBack="True">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 241px">
                            &nbsp;</td>
                        <td style="width: 241px">
                            &nbsp;</td>
                        <td>
                            &nbsp;</td>
                    </tr>
                </table>
            </td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td align="center">
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:ImageButton ID="btnRun" runat="server" AlternateText="Print Report" 
                    ImageUrl="~/App_Themes/Billing2/Buttons/Run Report.gif" />
            </td>
            <td>
                </td>
        </tr>
        <tr>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
    </table>
                            
              
                  
                  
        
                </asp:Content>

