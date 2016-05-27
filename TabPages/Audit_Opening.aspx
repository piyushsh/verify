<%@ Page Language="VB" MasterPageFile="~/TabPages/ProjectMain.master" AutoEventWireup="false" CodeFile="Audit_Opening.aspx.vb" Inherits="TabPages_Audit_Opening"  %>


<%@ Register Assembly="Infragistics4.Web.v13.2, Version=13.2.20132.2028, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" namespace="Infragistics.Web.UI.DisplayControls" tagprefix="ig" %>
<%@ Register Assembly="Infragistics4.Web.v13.2, Version=13.2.20132.2028, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" namespace="Infragistics.Web.UI.EditorControls" tagprefix="ig" %>
<%@ Register Assembly="Infragistics4.Web.v13.2, Version=13.2.20132.2028, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" namespace="Infragistics.Web.UI.GridControls" tagprefix="ig" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ProjectMAIN_content" Runat="Server">

    <script type="text/javascript">
       

 
                
    </script>

    <table style="width: 945px">
        <tr>
            <td class="auto-style2">
    
    
    <table style="font-size: 10pt; width: 950px; font-family: Arial">
        <tr>
            <td align="left" valign="top" style="background-color: #E6E6CC">
                <asp:Label ID="lblWO" runat="server" BackColor="#E6E6CC" Font-Bold="True" Font-Names="Arial"
                    Font-Size="11pt" Height="22px" Text="Order: Name Here " Width="179px"></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:Label ID="Label14" runat="server" Font-Bold="True" Font-Size="12pt" 
                                Text="Customer:"></asp:Label>
                        &nbsp;<asp:Label ID="lblCustomerName" runat="server" Font-Bold="False" Font-Names="Arial"
                                Font-Size="10pt" Width="231px"></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:Label ID="Label12" runat="server" Font-Bold="True" Font-Size="12pt" 
                                style="text-align: right" Text="Order Status:"></asp:Label>
                        &nbsp;&nbsp;
                            <asp:Label ID="lblorderStatus" runat="server" Font-Bold="False" 
                                Font-Names="Arial" Font-Size="10pt" Width="249px"></asp:Label></td>
        </tr>
        
        <tr style="font-size: 10pt">
            <td align="left" valign="top" style="border: 1px none #808080;">
            
                <table align="left" cellpadding="0" cellspacing="0" 
                    style="padding: 2px; width: 930px; float: left">
                    <tr>
                        <td style="border-top-style: solid;  border-left-style: solid; border-top-width: 1px;  border-left-width: 1px; border-top-color: #BEA027;  border-left-color: #BEA027">
             
                            <asp:Label ID="Label8" runat="server" Font-Bold="True" Font-Size="9pt" 
                                Text="Customer PO Number:" Font-Names="Arial" Width="132px" Height="16px"></asp:Label>
                        </td>
                        <td style="border-top-style: solid;  border-left-style: solid; border-top-width: 1px;  border-left-width: 1px; border-top-color: #BEA027;  border-left-color: #BEA027">
             
                            <asp:Label ID="Label11" runat="server" Font-Bold="True" Font-Size="9pt" 
                                Text="Requested Delivery Date:" Width="149px"></asp:Label>
                        </td>
                        <td style="border-top-style: solid;  border-left-style: solid; border-top-width: 1px; border-left-width: 1px; border-top-color: #BEA027; border-left-color: #BEA027">
             
                            <asp:Label ID="Label15" runat="server" Font-Bold="True" Font-Size="9pt" 
                                Text="Delivery Customer:" Width="156px"></asp:Label>
                        </td>
                        <td style="border-top-style: solid;  border-left-style: solid; border-top-width: 1px;  border-left-width: 1px; border-top-color: #BEA027;  border-left-color: #BEA027">
             
                            <asp:Label ID="Label16" runat="server" Font-Bold="True" Font-Size="9pt" 
                                Text="Delivery Address:" Width="150px"></asp:Label>
                        </td>
                        <td style="border-top-style: solid;  border-left-style: solid; border-top-width: 1px;  border-left-width: 1px; border-top-color: #BEA027;  border-left-color: #BEA027">
             
                            <asp:Label ID="lblOrderTypeLabel" runat="server" Font-Bold="True" Font-Size="9pt" 
                                Text="Order Type:" style="color: #000066" Width="123px"></asp:Label>
                        </td>
                        <td style="border-top-style:solid;  border-left-style: solid; border-top-width: 1px;  border-left-width: 1px; border-top-color: #BEA027;  border-left-color: #BEA027">
             
                            <asp:Label ID="Label10" runat="server" Font-Bold="True" Font-Size="9pt" 
                                Text="Order Date:" Width="96px"></asp:Label>
                        </td>
                        <td style="border-top-style: solid; border-right-style: solid; border-left-style: solid; border-top-width: 1px; border-right-width: 1px; border-left-width: 1px; border-top-color: #BEA027; border-right-color: #BEA027; border-left-color: #BEA027">
             
                            <asp:Label ID="Label28" runat="server" Font-Bold="True" Font-Size="9pt" 
                                Text="Priority:" Width="63px"></asp:Label>
                        </td>
                    </tr>
                    <tr style="vertical-align: top; text-align: left">
                        <td style="border-bottom-style: solid;  border-left-style: solid; border-bottom-width: 1px;  border-left-width: 1px; border-bottom-color: #BEA027;  border-left-color: #BEA027">
             
                            <asp:Label ID="lblPONumber" runat="server" Font-Bold="False" Font-Names="Arial"
                                Font-Size="9pt" Width="127px"></asp:Label>
                            
                        </td>
                        <td style="border-bottom-style: solid;  border-left-style: solid; border-bottom-width: 1px;  border-left-width: 1px; border-bottom-color: #BEA027;  border-left-color: #BEA027">
             
                             <asp:Label ID="lblRequestedDeliveryDate" runat="server" Font-Bold="False" Font-Names="Arial"
                                Font-Size="9pt" Width="134px"></asp:Label>
                            
                            
                        </td>
                        <td style="border-bottom-style: solid;  border-left-style: solid; border-bottom-width: 1px;  border-left-width: 1px; border-bottom-color: #BEA027;  border-left-color: #BEA027">
             
                            <asp:Label ID="lblDeliveryCustomer" runat="server" Font-Bold="False" Font-Names="Arial"
                                Font-Size="9pt" Width="153px"></asp:Label>
                        </td>
                        <td style="border-bottom-style: solid;  border-left-style: solid; border-bottom-width: 1px;  border-left-width: 1px; border-bottom-color: #BEA027;  border-left-color: #BEA027">
             
                            <asp:Label ID="lblCustAddress" runat="server" Font-Bold="False" Font-Names="Arial"
                                Font-Size="9pt" Width="144px"></asp:Label></td>
                        <td style="border-bottom-style: solid;  border-left-style: solid; border-bottom-width: 1px;  border-left-width: 1px; border-bottom-color: #BEA027; border-left-color: #BEA027">
             
                            <asp:Label ID="lblOrderType" runat="server" Font-Bold="False" Font-Names="Arial"
                                Font-Size="9pt" Width="120px" style="color: #333399"></asp:Label>
                        </td>
                        <td style="border-bottom-style: solid;  border-left-style: solid; border-bottom-width: 1px;  border-left-width: 1px; border-bottom-color: #BEA027;  border-left-color: #BEA027">
             
                            <asp:Label ID="lblOrderDate" runat="server" Font-Bold="False" 
                                Font-Names="Arial" Font-Size="9pt" Width="90px"></asp:Label>
                        </td>
                        <td style="border-bottom-style: solid; border-right-style: solid; border-left-style: solid; border-bottom-width: 1px; border-right-width: 1px; border-left-width: 1px; border-bottom-color: #BEA027; border-right-color: #BEA027; border-left-color: #BEA027">
             
                            <asp:Label ID="lblPriority" runat="server" Font-Bold="False" 
                                Font-Names="Arial" Font-Size="9pt" Width="60px"></asp:Label>
                        </td>
                    </tr>
                </table>
            
            
            </td>
        </tr>
        
        <tr style="font-size: 10pt">
            <td align="left" valign="top" style="border: 1px none #808080;">
            
                <table align="left" style="width: 930px; float: left">
                    <tr>
                        <td>
                    <asp:Label ID="Label13" runat="server" Font-Bold="True" Font-Size="10pt" 
                        Text="Comment:" style="color: #000066"></asp:Label>
                        </td>
                        <td>
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td>
                             <asp:Label ID="lblComment" runat="server" style="color: #000066"></asp:Label>
                            
                        </td>
                        <td>
                            &nbsp;</td>
                    </tr>
                </table>
            
            
            </td>
        </tr>
    </table>
    
    
            </td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td align="right" class="auto-style2">
                <asp:ImageButton ID="BtnBack" runat="server" ImageUrl="~/App_Themes/Buttons/Back-Button.gif" />
            </td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td class="auto-style2">
                        <asp:Label ID="Label7" runat="server" Font-Bold="True" 
                            style="font-size: 14pt; font-family: Arial" 
                                        Text="Audit Trail for this Sales Order"></asp:Label>
                                </td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td class="auto-style2">
                <ig:WebDataGrid ID="wdgAudit" runat="server" 
        AltItemCssClass="VerifyGrid_Report_AlternateRow" AutoGenerateColumns="False" 
        BackColor="White" CssClass="VerifyGrid_Report_Frame" EnableTheming="True" 
        Font-Names="Arial" FooterCaptionCssClass="VerifyGrid_Report_Footer" 
        HeaderCaptionCssClass="VerifyGrid_Report_HeaderGold_10pt" Height="214px" 
        ItemCssClass="VerifyGrid_Report_Row" Width="938px" EnableAjax="False">
        <Columns>
            <ig:BoundDataField DataFieldName="DateAndTime" Key="Date" Width="140px" DataFormatString="{0:G}">
                <Header Text="Date:"/>
            </ig:BoundDataField>
            <ig:BoundDataField DataFieldName="Comment" Key="Supplier" Width="500px">
                <Header Text="Comment:"/>
            </ig:BoundDataField>
            <ig:BoundDataField DataFieldName="UserName" Key="User" Width="120px">
                <Header Text="Logged by:"/>
            </ig:BoundDataField>
            <ig:BoundDataField DataFieldName="RecordType" Key="RecordType" Width="120px">
                <Header Text="Type:"/>
            </ig:BoundDataField>
           
                                                                 
        </Columns>
        <Behaviors>
        <ig:Activation ActiveRowCssClass="SelectedRow">
            <AutoPostBackFlags ActiveCellChanged="True" />
        </ig:Activation>
        <ig:ColumnResizing>
        </ig:ColumnResizing>
      
      
        <ig:Sorting Enabled="True" 
            AscendingImageUrl="~/App_Themes/Buttons/Up_Arrow_small.gif" 
            DescendingImageUrl="~/App_Themes/Buttons/Down_Arrow_small.gif" >
        </ig:Sorting>

    </Behaviors>
  </ig:WebDataGrid>
                   
                                </td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td class="auto-style2">
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td class="auto-style2">
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
    </table>
</asp:Content>

<asp:Content ID="Content2" runat="server" contentplaceholderid="head">
    <style type="text/css">
        .auto-style2 {
            width: 861px;
        }
    </style>
</asp:Content>


