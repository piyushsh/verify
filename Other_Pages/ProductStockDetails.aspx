<%@ Page Language="VB" MasterPageFile="~/TabPages/ProjectMain.master" AutoEventWireup="false" CodeFile="ProductStockDetails.aspx.vb" Inherits="Other_Pages_ProductStockDetails" %>



<%@ Register Assembly="Infragistics4.Web.v13.2, Version=13.2.20132.2028, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" namespace="Infragistics.Web.UI.DisplayControls" tagprefix="ig" %>
<%@ Register Assembly="Infragistics4.Web.v13.2, Version=13.2.20132.2028, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" namespace="Infragistics.Web.UI.EditorControls" tagprefix="ig" %>
<%@ Register Assembly="Infragistics4.Web.v13.2, Version=13.2.20132.2028, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" namespace="Infragistics.Web.UI.GridControls" tagprefix="ig" %>
<%@ Register Assembly="Infragistics4.Web.v13.2, Version=13.2.20132.2028, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" namespace="Infragistics.Web.UI.ListControls" tagprefix="ig" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ProjectMAIN_content" Runat="Server">

                  
                <table style="width: 940px">
      
        <tr>
            <td style="width: 222px; height: 26px;">
                        &nbsp;</td>
            <td style="height: 26px">
                        &nbsp;</td>
            <td style="height: 26px">
                        <asp:ImageButton ID="btnBack" runat="server" ImageUrl="~/APP_THEMES/Billing/Buttons/Back-Button.gif" />
                </td>
            <td style="height: 26px">
                &nbsp;</td>
        </tr>
      
        <tr>
            <td style="width: 222px; height: 26px;">
                        <asp:Label ID="lblPageTitle" runat="server" Font-Bold="True" 
                            style="font-size: 14pt; font-family: Arial">Stock Details for:</asp:Label>
                    </td>
            <td style="height: 26px">
                        <asp:Label ID="lblProductName" runat="server" Font-Bold="True" 
                            style="font-size: 14pt; font-family: Arial"></asp:Label>
                    </td>
            <td style="height: 26px">
                &nbsp;</td>
            <td style="height: 26px">
                </td>
        </tr>
        </table>
                  
                   <table style="width: 950px">
                       <tr>
                           <td>
            
            <table style="width: 930px; font-family: Arial;">
            <tr>
                <td style="width: 220px; text-align: right;">
                            <asp:Label ID="Label8" runat="server" Font-Bold="True" Font-Size="10pt" 
                                Text="Product Code:" Font-Names="Arial"></asp:Label>
                        </td>
                <td style="width: 245px; text-align: left">
                            <asp:Label ID="lblProductCode" runat="server" Font-Bold="False" Font-Names="Arial"
                                Font-Size="10pt" Width="144px"></asp:Label></td>
                <td style="width: 220px; text-align: right;">
                            <asp:Label ID="lblOrderTypeLabel" runat="server" Font-Bold="True" Font-Size="10pt" 
                                Text="In Stock:" style="color: #000066"></asp:Label>
                        </td>
                <td style="width: 245px; text-align: left">
                            <asp:Label ID="lblInStock" runat="server" Font-Bold="False" Font-Names="Arial"
                                Font-Size="10pt" Width="144px" style="color: #333399"></asp:Label>
                        </td>
            </tr>
            <tr>
                <td style="width: 220px; text-align: right;">
                            <asp:Label ID="Label11" runat="server" Font-Bold="True" Font-Size="10pt" 
                                Text="Allocated:" Width="210px"></asp:Label>
                        </td>
                <td style="width: 245px; text-align: left">
                            <asp:Label ID="lblAllocated" runat="server" Font-Bold="False" Font-Names="Arial"
                                Font-Size="10pt" Width="146px"></asp:Label></td>
                <td style="width: 220px; text-align: right;">
                            <asp:Label ID="Label10" runat="server" Font-Bold="True" Font-Size="10pt" 
                                Text="Available:"></asp:Label>
                        </td>
                <td style="width: 245px; text-align: left">
                            <asp:Label ID="lblAvailable" runat="server" Font-Bold="False" 
                                Font-Names="Arial" Font-Size="10pt" Width="148px"></asp:Label>
                        </td>
            </tr>
            <tr>
                <td style="width: 220px; text-align: right;">
                            <asp:Label ID="Label15" runat="server" Font-Bold="True" Font-Size="10pt" 
                                Text="Detail:"></asp:Label>
                        </td>
                <td style="width: 245px; text-align: left">
                            <asp:Label ID="lblProductDetail" runat="server" Font-Bold="False" Font-Names="Arial"
                                Font-Size="10pt" Width="210px"></asp:Label>
                        </td>
                <td style="width: 220px; text-align: right;">
                            <asp:Label ID="Label16" runat="server" Font-Bold="True" Font-Size="10pt" 
                                Text="On Order:"></asp:Label>
                        </td>
                <td style="width: 245px; text-align: left">
                            <asp:Label ID="lblOnOrder" runat="server" Font-Bold="False" 
                                Font-Names="Arial" Font-Size="10pt" Width="148px"></asp:Label>
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
                        <asp:Label ID="lblPageTitle0" runat="server" Font-Bold="True" 
                            style="font-size: 14pt; font-family: Arial">Batch Details:</asp:Label>
                           </td>
                       </tr>
                       <tr>
                           <td>
               
                
<ig:WebDataGrid ID="wdgBatchSummary" runat="server"
        AltItemCssClass="VerifyGrid_Report_AlternateRow" 
    AutoGenerateColumns="False" Font-Names="Arial" 
        ItemCssClass="VerifyGrid_Report_Row" 
        EnableTheming="True" FooterCaptionCssClass="VerifyGrid_Report_Footer" 
        HeaderCaptionCssClass="VerifyGrid_Report_HeaderGold" BackColor="White" 
        CssClass="VerifyGrid_Report_Frame" Height="250px" Width="780px" EnableAjax="False">
    <Columns>
        <ig:BoundDataField DataFieldName="TraceCodeDesc" Key="TraceCodeDesc" Width="100px" >
            <Header Text="Trace Code:" />
        </ig:BoundDataField>
        <ig:BoundDataField DataFieldName="Quantity" Key="Quantity" Width="80px" >
            <Header Text="Quantity:" />
        </ig:BoundDataField>
         <ig:BoundDataField DataFieldName="Weight" Key="Weight" Width="80px">
            <Header Text="Weight:" />
        </ig:BoundDataField>
         <ig:BoundDataField DataFieldName="Location" Key="Location" Width="200px">
            <Header Text="Location:" />
        </ig:BoundDataField>
       
        <ig:BoundDataField DataFieldName="TraceCodeID" Key="TraceCodeID" hidden="true" Width="20px" >
            <Header Text="TraceCodeID:" />
        </ig:BoundDataField>
       
       
    </Columns>
    <Behaviors>
        <ig:Activation ActiveRowCssClass="SelectedRow">
        </ig:Activation>
        <ig:ColumnResizing>
        </ig:ColumnResizing>

        
        <ig:Selection CellClickAction="Row" RowSelectType="Single">
            <AutoPostBackFlags RowSelectionChanged="True" />
        </ig:Selection>
    </Behaviors>
</ig:WebDataGrid>


                           </td>
                       </tr>
                       <tr>
                           <td>
        
              
                               <ig:WebDataGrid ID="wdgBatchSerialDetails" runat="server"
        AltItemCssClass="VerifyGrid_Report_AlternateRow" 
    AutoGenerateColumns="False" Font-Names="Arial" 
        ItemCssClass="VerifyGrid_Report_Row" 
        EnableTheming="True" FooterCaptionCssClass="VerifyGrid_Report_Footer" 
        HeaderCaptionCssClass="VerifyGrid_Report_HeaderGold" BackColor="White" 
        CssClass="VerifyGrid_Report_Frame" Height="250px" Width="780px" EnableAjax="False">
    <Columns>
        <ig:BoundDataField DataFieldName="TraceCodeDesc" Key="TraceCodeDesc" Width="100px" >
            <Header Text="Trace Code:" />
        </ig:BoundDataField>
        <ig:BoundDataField DataFieldName="Qty" Key="Quantity" Width="80px" >
            <Header Text="Quantity:" />
        </ig:BoundDataField>
         <ig:BoundDataField DataFieldName="Wgt" Key="Weight" Width="80px">
            <Header Text="Weight:" />
        </ig:BoundDataField>
         <ig:BoundDataField DataFieldName="LocationID" Key="Location" Width="200px">
            <Header Text="Location:" />
        </ig:BoundDataField>
        <ig:BoundDataField DataFieldName="SerialNum" Key="SerialNumber" Width="100px">
            <Header Text="Serial Num:" />
        </ig:BoundDataField>
        <ig:BoundDataField DataFieldName="TraceCodeID" Key="TraceCodeID" hidden="true" Width="20px" >
            <Header Text="TraceCodeID:" />
        </ig:BoundDataField>
       
       
    </Columns>
    <Behaviors>
        <ig:Activation ActiveRowCssClass="SelectedRow">
        </ig:Activation>
        <ig:ColumnResizing>
        </ig:ColumnResizing>

        
        <ig:Selection CellClickAction="Row" RowSelectType="Single">
            <AutoPostBackFlags RowSelectionChanged="True" />
        </ig:Selection>
    </Behaviors>
</ig:WebDataGrid>
                
                           </td>
                       </tr>
                       <tr>
                           <td>
                               <asp:CheckBox ID="chkShowNegative" runat="server" AutoPostBack="True" 
                                   Font-Names="Arial" Font-Size="10pt" Text="Show Negative Batches" 
                                   Visible="False" />
                           </td>
                       </tr>
                       <tr>
                           <td>
                        <asp:Label ID="lblPageTitle1" runat="server" Font-Bold="True" 
                            style="font-size: 14pt; font-family: Arial">Allocated to Orders:</asp:Label>
                           </td>
                       </tr>
                       <tr>
                           <td>
        
        
      
                               <ig:WebDataGrid ID="wdgSalesOrders" runat="server"
        AltItemCssClass="VerifyGrid_Report_AlternateRow" 
    AutoGenerateColumns="False" Font-Names="Arial" 
        ItemCssClass="VerifyGrid_Report_Row" 
        EnableTheming="True" FooterCaptionCssClass="VerifyGrid_Report_Footer" 
        HeaderCaptionCssClass="VerifyGrid_Report_HeaderGreen" BackColor="White" 
        CssClass="VerifyGrid_Report_Frame" Height="250px" Width="860px" EnableAjax="False">
    <Columns>
        <ig:BoundDataField DataFieldName="OrderNum" Key="SalesOrderNum" Width="120px" >
            <Header Text="Sales Order Num:" />
        </ig:BoundDataField>
        <ig:BoundDataField DataFieldName="DeliveryCustomer" Key="DeliveryCustomer" Width="250px" >
            <Header Text="Customer:" />
        </ig:BoundDataField>
         <ig:BoundDataField DataFieldName="ReqQty" Key="QuantityRequested" Width="100px">
            <Header Text="Qty Required:" />
        </ig:BoundDataField>
         <ig:BoundDataField DataFieldName="ReqWgt" Key="WeightRequested" Width="100px">
            <Header Text="Wgt Required:" />
        </ig:BoundDataField>
        <ig:BoundDataField DataFieldName="ReqDate" Key="DueDate" Width="80px">
            <Header Text="Due Date:" />
        </ig:BoundDataField>
        <ig:BoundDataField DataFieldName="Status" Key="Status" Width="120px">
            <Header Text="Status:" />
        </ig:BoundDataField>
        <ig:BoundDataField DataFieldName="SalesOrderID" Key="SalesOrderID" hidden="true" Width="20px" >
            <Header Text="SalesOrderID:" />
        </ig:BoundDataField>
       
       
    </Columns>
    <Behaviors>
        <ig:Activation ActiveRowCssClass="SelectedRow">
        </ig:Activation>
        <ig:ColumnResizing>
        </ig:ColumnResizing>

        
        <ig:Selection CellClickAction="Row" RowSelectType="Single">
            <AutoPostBackFlags RowSelectionChanged="True" />
        </ig:Selection>
    </Behaviors>
</ig:WebDataGrid>


                
                           </td>
                       </tr>
                       <tr>
                           <td>
                               &nbsp;</td>
                       </tr>
        </table>
        
                </asp:Content>

