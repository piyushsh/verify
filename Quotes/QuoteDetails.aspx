<%@ Page Title="" Language="VB" MasterPageFile="~/Quotes/FormsMaster.master" AutoEventWireup="false" CodeFile="QuoteDetails.aspx.vb" Inherits="Quotes_QuoteDetails" %>

<%@ Register TagPrefix="igsch" Namespace="Infragistics.WebUI.WebSchedule" Assembly="Infragistics2.WebUI.WebDateChooser.v7.3, Version=7.3.20073.38, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="DetailsMaster_ContentPlaceHolder" Runat="Server">
    <p>
    
        <table style="width: 690px">
         
            <tr>
                <td>
                    <table style="font-size: 10pt; width: 690px; font-family: Arial">
                        <tr>
                            <td align="left" style="border-right: darkgray 1px solid; border-top: darkgray 1px solid;
                border-left: darkgray 1px solid; border-bottom: darkgray 1px solid; height: 120px;" 
                                valign="top">
                                <span style="font-size: 11pt"><strong>
                                <asp:Label ID="Label7" runat="server" font-bold="True" font-names="Arial" 
                                    font-size="11pt" forecolor="#B38700" text="Customer Details:" 
                                    width="205px"></asp:Label>
                                <br />
                                <table style="font-size: 10pt; width: 689px; font-family: Arial">
                                    <tr>
                                        <td align="left" style="vertical-align: top; width: 199px; text-align: right" 
                                            valign="top">
                                            <span style="font-size: 11pt"><strong>
                                            <asp:Label ID="lblJobIdCaption0" runat="server" Font-Bold="True" 
                                                Font-Names="Arial" Font-Size="10pt" Width="170px">Customer:</asp:Label>
                                            </strong></span>
                                        </td>
                                        <td align="left" colspan="2" valign="top">
                                            <asp:DropDownList ID="ddlBillingCustomer" runat="server" Height="25px" 
                                                Width="295px" AutoPostBack="True">
                                                <asp:ListItem> Sample Customer</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td align="left" style="width: 200px" valign="top">
                                            &nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td align="left" 
                                            style="vertical-align: top; width: 199px; text-align: right; height: 18px;" 
                                            valign="top">
                                            <b><span style="font-size: 11pt"><strong>
                                            <asp:Label ID="lblBatchCaption" runat="server" Font-Bold="True" 
                                                Font-Names="Arial" Font-Size="10pt" Width="170px">Contact:</asp:Label>
                                            </strong></span></b>
                                        </td>
                                        <td align="left" style="height: 18px;" valign="top" colspan="2">
                                <span style="font-size: 11pt"><strong>
                                            <asp:DropDownList ID="ddlContact" runat="server" Height="25px" 
                                                Width="295px" AutoPostBack="True">
                                                <asp:ListItem> Sample Customer</asp:ListItem>
                                            </asp:DropDownList>
                                </strong></span>
                                        </td>
                                        <td align="left" style="width: 200px; height: 18px;" valign="top">
                                            &nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td align="left" style="vertical-align: top; width: 199px; text-align: right" 
                                            valign="top">
                                            <b><span style="font-size: 11pt"><strong>
                                            <asp:Label ID="lblRefCaption" runat="server" Font-Bold="True" 
                                                Font-Names="Arial" Font-Size="10pt" Width="170px">Phone:</asp:Label>
                                            </strong></span></b>
                                        </td>
                                        <td align="left" style="width: 243px" valign="top">
                                            <asp:Label ID="lblPhone" runat="server" Font-Bold="False" font-names="Arial" 
                                                font-size="10pt" width="163px"></asp:Label>
                                        </td>
                                        <td align="left" style="vertical-align: top; width: 150px; text-align: right" 
                                            valign="top">
                                            &nbsp;</td>
                                        <td align="left" style="width: 200px" valign="top">
                                            &nbsp;</td>
                                    </tr>
                                    <tr>
                               
                                        <td align="left" style="vertical-align: top; width: 199px; text-align: right" 
                                            valign="top">
                                <span style="font-size: 11pt">
                                            <strong><b>
                                            <asp:Label ID="lblProdVerCaption2" runat="server" Font-Bold="True" 
                                                Font-Names="Arial" Font-Size="10pt" Width="170px">On Hold?:</asp:Label>
                                            </b></strong>
                                            </span>
                                        </td>
                                        <td align="left" style="width: 243px" valign="top">
                                            <asp:Label ID="lblOnHold" runat="server" Font-Bold="False" Font-Names="Arial" 
                                                Font-Size="10pt" Width="163px">No</asp:Label>
                                        </td>
                                        <td align="left" style="vertical-align: top; width: 150px; text-align: right" 
                                            valign="top">
                                <span style="font-size: 11pt">
                                            <strong><b>
                                            <asp:Label ID="lblProdVerCaption1" runat="server" Font-Bold="True" 
                                                Font-Names="Arial" Font-Size="10pt" Width="170px">Terms:</asp:Label>
                                            </b></strong>
                                            </span>
                                        </td>
                                        <td align="left" style="width: 200px" valign="top">
                                            <asp:Label ID="lblTerms" runat="server" Font-Bold="False" Font-Names="Arial" 
                                                Font-Size="10pt" Width="170px"></asp:Label>
                                        </td>
                              
                                    </tr>
                                    <tr>
                                        <td align="left" style="vertical-align: top; width: 199px; text-align: right; " 
                                            valign="top">
                                            <strong><span style="font-size: 11pt"><b>
                                            <asp:Label ID="lblQtyCaption" runat="server" Font-Bold="True" 
                                                Font-Names="Arial" Font-Size="10pt" Width="170px">Customer Discount:</asp:Label>
                                            </b></span></strong>
                                        </td>
                                        <td align="left" style="width: 243px" valign="top">
                                            <span style="font-size: 11pt">
                                            <asp:Label ID="lblDiscount" runat="server" Font-Bold="False" font-names="Arial" 
                                                font-size="10pt" width="157px">0%</asp:Label>
                                            </span>
                                        </td>
                                        <td align="left" style="vertical-align: top; width: 150px; text-align: right" 
                                            valign="top">
                                            <strong><span style="font-size: 11pt"><b>
                                            <asp:Label ID="lblProdVerCaption" runat="server" Font-Bold="True" 
                                                Font-Names="Arial" Font-Size="10pt" Width="170px">Tax Exempt:</asp:Label>
                                            </b></span></strong>
                                        </td>
                                        <td align="left" style="width: 200px" valign="top">
                                            <span style="font-size: 11pt">
                                            <asp:Label ID="lblVATExempt" runat="server" Font-Bold="False" 
                                                font-names="Arial" font-size="10pt" width="157px">Yes</asp:Label>
                                            </span>
                                        </td>
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
                    <asp:HiddenField ID="hdnCustomerContactId" runat="server" />
                </td>
            </tr>
            <tr>
                <td>
                    <table style="font-size: 10pt; width: 690px; font-family: Arial">
                        <tr>
                            <td align="left" style="border-right: darkgray 1px solid; border-top: darkgray 1px solid;
                border-left: darkgray 1px solid; border-bottom: darkgray 1px solid; height: 120px;" 
                                valign="top">
                                <span style="font-size: 11pt"><strong>
                                <asp:Label ID="Label8" runat="server" font-bold="True" font-names="Arial" 
                                    font-size="11pt" forecolor="#B38700" text="Order Logistics Details:" 
                                    width="233px"></asp:Label>
                                <br />
                                <table style="font-size: 10pt; width: 689px; font-family: Arial">
                                    <tr>
                                        <td align="left" style="vertical-align: top; text-align: right" 
                                            valign="top" colspan="2">
                                            <asp:CheckBox ID="chkDeliveryCustsOnly" runat="server" 
                                                Text="Only show delivery customers for selected billing customer " />
                                        </td>
                                        <td align="left" style="width: 200px" valign="top">
                                            &nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td align="left" style="vertical-align: top; width: 199px; text-align: right" 
                                            valign="top">
                                            <span style="font-size: 11pt"><strong>
                                            <asp:Label ID="lblJobIdCaption1" runat="server" Font-Bold="True" 
                                                Font-Names="Arial" Font-Size="10pt" Width="170px">Deliver To:</asp:Label>
                                            </strong></span>
                                        </td>
                                        <td align="left" valign="top">
                                            <asp:DropDownList ID="ddlDeliveryCustomer" runat="server" Height="25px" 
                                                Width="295px" AutoPostBack="True">
                                                <asp:ListItem>Sample Delivery Customer</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td align="left" style="width: 200px" valign="top">
                                            &nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td align="left" 
                                            style="vertical-align: top; width: 199px; text-align: right; height: 18px;" 
                                            valign="top">
                                            <b><span style="font-size: 11pt"><strong>
                                            <asp:Label ID="lblBatchCaption0" runat="server" Font-Bold="True" 
                                                Font-Names="Arial" Font-Size="10pt" Width="170px">Requested Delivery Date:</asp:Label>
                                            </strong></span></b>
                                        </td>
                                        <td align="left" style="height: 18px;" valign="top">
                                <igsch:WebDateChooser ID="dteRequestedDeliveryDate" runat="server" Font-Names="Arial" 
                                    NullDateLabel="Select Delivery Date..." Width="186px" Height="16px" Value="">
                                    <CalendarLayout>
                                        <CalendarStyle BorderStyle="None" Font-Names="Arial">
                                        </CalendarStyle>
                                    </CalendarLayout>
                                </igsch:WebDateChooser>
                                        </td>
                                        <td align="left" style="width: 200px; height: 18px;" valign="top">
                                            &nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td align="left" 
                                            style="vertical-align: top; width: 199px; text-align: right; height: 18px;" 
                                            valign="top">
                                <span style="font-size: 11pt"><strong>
                                            <b>
                                            <asp:Label ID="lblBatchCaption2" runat="server" Font-Bold="True" 
                                                Font-Names="Arial" Font-Size="10pt" Width="170px">Route:</asp:Label>
                                            </b>
                                </strong></span>
                                        </td>
                                        <td align="left" style="height: 18px;" valign="top">
                                <span style="font-size: 11pt"><strong>
                                            <asp:Label ID="lblRoute" runat="server" Font-Bold="False" font-names="Arial" 
                                                font-size="10pt" width="163px"></asp:Label>
                                </strong></span>
                                        </td>
                                        <td align="left" style="width: 200px; height: 18px;" valign="top">
                                            &nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td align="left" style="vertical-align: top; width: 199px; text-align: right" 
                                            valign="top">
                                            <b><span style="font-size: 11pt"><strong>
                                            <asp:Label ID="lblRefCaption0" runat="server" Font-Bold="True" 
                                                Font-Names="Arial" Font-Size="10pt" Width="170px">Address:</asp:Label>
                                            </strong></span></b>
                                        </td>
                                        <td align="left" valign="top">
                                            <asp:Label ID="lblAddress" runat="server" Font-Bold="False" font-names="Arial" 
                                                font-size="10pt" width="163px" Height="106px"></asp:Label>
                                        </td>
                                        <td align="left" style="width: 200px" valign="top">
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
                    <table style="font-size: 10pt; width: 690px; font-family: Arial">
                        <tr>
                            <td align="left" style="border-right: darkgray 1px solid; border-top: darkgray 1px solid;
                border-left: darkgray 1px solid; border-bottom: darkgray 1px solid; height: 120px;" 
                                valign="top">
                                <span style="font-size: 11pt"><strong>
                                <asp:Label ID="Label9" runat="server" font-bold="True" font-names="Arial" 
                                    font-size="11pt" forecolor="#B38700" text="Other Info:" width="205px"></asp:Label>
                                <br />
                                <table style="font-size: 10pt; width: 689px; font-family: Arial">
                                 
                                    <tr>
                                        <td align="left" 
                                            style="vertical-align: top; width: 199px; text-align: right; height: 18px;" 
                                            valign="top">
                                <span style="font-size: 11pt"><strong>
                                            <b>
                                            <asp:Label ID="lblBatchCaption3" runat="server" Font-Bold="True" 
                                                Font-Names="Arial" Font-Size="10pt" Width="170px">Date Started:</asp:Label>
                                            </b>
                                </strong></span>
                                        </td>
                                        <td align="left" style="height: 18px;" valign="top">
                                <span style="font-size: 11pt"><strong>
                                            <asp:Label ID="lblDateStarted" runat="server" Font-Bold="False" font-names="Arial" 
                                                font-size="10pt" width="163px"></asp:Label>
                                </strong></span>
                                        </td>
                                        <td align="left" style="width: 200px; height: 18px;" valign="top">
                                            &nbsp;</td>
                                    </tr>
                                 
                                    <tr>
                                        <td align="left" 
                                            style="vertical-align: top; width: 199px; text-align: right; height: 18px;" 
                                            valign="top">
                                <span style="font-size: 11pt"><strong>
                                            <b>
                                            <asp:Label ID="lblBatchCaption4" runat="server" Font-Bold="True" 
                                                Font-Names="Arial" Font-Size="10pt" Width="170px">Date Issued:</asp:Label>
                                            </b>
                                </strong></span>
                                        </td>
                                        <td align="left" style="height: 18px;" valign="top">
                                <span style="font-size: 11pt"><strong>
                                            <asp:Label ID="lblDateIssued" runat="server" Font-Bold="False" font-names="Arial" 
                                                font-size="10pt" width="163px"></asp:Label>
                                </strong></span>
                                        </td>
                                        <td align="left" style="width: 200px; height: 18px;" valign="top">
                                            &nbsp;</td>
                                    </tr>
                                 
                                    <tr>
                                        <td align="left" 
                                            style="vertical-align: top; width: 199px; text-align: right; height: 18px;" 
                                            valign="top">
                                            <b><span style="font-size: 11pt"><strong>
                                            <asp:Label ID="lblBatchCaption1" runat="server" Font-Bold="True" 
                                                Font-Names="Arial" Font-Size="10pt" Width="170px">Customer P.O./Reference:</asp:Label>
                                            </strong></span></b>
                                        </td>
                                        <td align="left" style="height: 18px;" valign="top">
                                            <asp:TextBox ID="txtCustomerRef" runat="server" Width="260px"></asp:TextBox>
                                        </td>
                                        <td align="left" style="width: 200px; height: 18px;" valign="top">
                                            &nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td align="left" style="vertical-align: top; width: 199px; text-align: right" 
                                            valign="top">
                                            <b><span style="font-size: 11pt"><strong>
                                            <asp:Label ID="lblRefCaption1" runat="server" Font-Bold="True" 
                                                Font-Names="Arial" Font-Size="10pt" Width="170px">Comment:</asp:Label>
                                            </strong></span></b>
                                        </td>
                                        <td align="left" valign="top" colspan="2">
                                <span style="font-size: 11pt"><strong>
                                            <asp:TextBox ID="txtComment" runat="server" Width="427px" Height="64px" 
                                                TextMode="MultiLine" Font-Names="Arial"></asp:TextBox>
                                </strong></span>
                                        </td>
                                    </tr>
                                   
                                    <tr>
                                        <td align="left" style="vertical-align: top; width: 199px; text-align: right; " 
                                            valign="top">
                                            &nbsp;</td>
                                        <td align="left" valign="top" colspan="2">
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
                    <asp:Button ID="Button1" runat="server" Text="Button" Visible="False" />
                </td>
            </tr>
          
        </table>
    </p>
    </asp:Content>

