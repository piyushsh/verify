<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Seerys_Invoice.aspx.vb" Inherits="SeerysInvoiceTemplate" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="ajaxToolkit" %>

<%@ Register assembly="Infragistics4.Web.v13.2, Version=13.2.20132.2028, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" namespace="Infragistics.Web.UI.GridControls" tagprefix="ig" %>



<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .style1
        {
            width: 1000px;
            height: 2100px;
        }
        .style2
        {
            width: 996px;
        }
        .style3
        {
            width: 600px;
        }
        .style4
        {
             width: 1000px;
            font-size: 12pt;
        }
        .style5
        {
            width: 1000px;
            font-size: 22pt;
        }
        .style6
        {
            width: 1118px;
        }
        .style7
        {
            width: 1000px;
            font-size: 14pt;
        }
        .style8
        {
            width: 1000px;
            font-size: 11pt;
        }
    .ig_Control
{
	background-color:White;
	font-size:xx-small;
	font-family: verdana;
	color:Black;
}


.ig_Control
{
	background-color:White;
	font-size:xx-small;
	font-family: verdana;
	color:Black;
}


.VerifyGrid_Report_HeaderGreen
{
	font-family: Arial;
	font-size: 11pt;
	font-weight: bold;
	color: #FFFFFF;
	background-color: #008000;
	vertical-align: middle;
	text-align: center;
}


.ig_Item
{
	background-color:White;
	font-size:10px;
	font-family: verdana;
	padding-left:1px;
}


.ig_Item
{
	background-color:White;
	font-size:10px;
	font-family: verdana;
	padding-left:1px;
}


.Filter_LAlign
{
	 float:left;
	 background-color:Transparent;
	 border-style:solid;
	 border-width:0px;
	 height: 18px;

}


.ig_Alt
{
	border-top:solid 1px #E3EFFF;
	border-right:solid 0px #FFFFFF;
	border-bottom:solid 1px #E3EFFF;
	border-left:solid 0px #FFFFFF;
}


.ig_Alt
{
	border-top:solid 1px #E3EFFF;
	border-right:solid 0px #FFFFFF;
	border-bottom:solid 1px #E3EFFF;
	border-left:solid 0px #FFFFFF;
}


        .style9
        {
            text-align: center;
            font-size: 12pt;
        }
        .style10
        {
            font-size: 14pt;
            font-family: "Times New Roman", Times, serif;
        }
        .style11
        {
            width: 690px;
            font-size: 14pt;
        }
        .style12
        {
            width: 690px;
        }
            
        .style21
        {
            width: 709px;
        }
        
        .style13
        {
            width: 226px;
        }
        .style14
        {
            font-size: 12pt;
            width: 226px;
        }
        
        .style15
        {
            width: 226px;
            height: 23px;
        }
        .style16
        {
            height: 23px;
        }
        </style>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>

    <div style="margin: 10px">
    
        <table class="style1" style="width: 1000px">
            <tr>
                <td style="text-align: left; vertical-align: top">
                    <table class="style2">
                        <tr>
                            <td style="vertical-align: top; text-align: left">
                                &nbsp;</td>
                            <td style="width: 50px">
                                &nbsp;</td>
                            <td style="vertical-align: top; text-align: right">
                    <table class="style6">
                        <tr>
                            <td style="vertical-align: top; text-align: left; width: 710px; height: 204px;" 
                                class="style21">
                                <asp:Image ID="Image1"  width ="709px" Height="203px" runat="server"/>
                            </td>
                            <td style="width: 50px">
                                &nbsp;</td>
                            <td style="vertical-align: top; text-align: right">
                                <table class="style3" 
                                    
                                    style="font-family: Arial; font-size: 10pt; vertical-align: top; text-align: left; width: 400px;">
                                    <tr>
                                        <td class="style5">
                                            INVOICE</td>
                                        <td>
                                            &nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td class="style13">
                                            &nbsp; &nbsp;</td>
                                        <td>
                                            &nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td class="style14">
                                            <strong>Number</strong></td>
                                        <td>
                                            <asp:Label ID="lblInvoiceNum" runat="server" Font-Names="Arial" 
                                                Font-Size="12pt"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="style13">
                                            &nbsp; &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td class="style14">
                                            <strong>Date</strong></td>
                                        <td>
                                            <asp:Label ID="lblInvoiceDate" runat="server" Font-Names="Arial" 
                                                Font-Size="12pt"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="style13">
                                            &nbsp;&nbsp; &nbsp;</td>
                                        <td>
                                            &nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td class="style14">
                                            <strong>Customer P.O. Num</strong></td>
                                        <td>
                                            <asp:Label ID="lblCustomerPO" runat="server" Font-Names="Arial" 
                                                Font-Size="12pt"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="style15">
                                            </td>
                                        <td class="style16">
                                            </td>
                                    </tr>
                                </table>
                                <br />
                            </td>
                        </tr>
                    </table>
                                <br />
                            </td>
                        </tr>
                    </table>
                    <br />
                    <table class="style6" style="font-family: Arial; font-size: 10pt">
                        <tr>
                            <td style="width: 100px">
                                &nbsp;&nbsp;</td>
                            <td class="style11">
                                <strong>Ship To: 
                                <asp:Label ID="lblDeliveryCustomer" runat="server" Font-Names="Arial" 
                                    Font-Size="11pt"></asp:Label>
                                </strong></td>
                            <td style="width: 200px">
                                &nbsp; &nbsp;</td>
                            <td class="style7" style="width: 600px">
                                <strong>Bill To:&nbsp;
                                <asp:Label ID="lblBillingCustomer" runat="server" Font-Names="Arial" 
                                    Font-Size="11pt"></asp:Label>
                                </strong></td>
                        </tr>
                        <tr>
                            <td style="width: 100px">
                                &nbsp;</td>
                            <td class="style12">
                                <asp:Label ID="lblShipAddress" runat="server" Font-Names="Arial" 
                                    Font-Size="11pt"></asp:Label>
                                <br />
                                <asp:Label ID="InvoiceNum1" runat="server" Font-Names="Arial" Font-Size="11pt" 
                                    Text="Tel. "></asp:Label>
                                <asp:Label ID="lblShipPhone" runat="server" Font-Names="Arial" Font-Size="11pt"></asp:Label>
                                <br />
                                <asp:Label ID="InvoiceNum2" runat="server" Font-Names="Arial" Font-Size="11pt" 
                                    Text="Fax"></asp:Label>
&nbsp;<asp:Label ID="lblShipFax" runat="server" Font-Names="Arial" Font-Size="11pt"></asp:Label>
                            </td>
                            <td style="width: 200px">
                                &nbsp;</td>
                            <td style="width: 600px; vertical-align: top; text-align: left;">
                                <asp:Label ID="lblBillAddress" runat="server" Font-Names="Arial" 
                                    Font-Size="11pt"></asp:Label>
                                <br />
                                <asp:Label ID="InvoiceNum3" runat="server" Font-Names="Arial" Font-Size="11pt" 
                                    Text="Tel. "></asp:Label>
                                <asp:Label ID="lblBillPhone" runat="server" Font-Names="Arial" Font-Size="11pt"></asp:Label>
                                <br />
                                <asp:Label ID="InvoiceNum4" runat="server" Font-Names="Arial" Font-Size="11pt" 
                                    Text="Fax"></asp:Label>
&nbsp;<asp:Label ID="lblBillFax" runat="server" Font-Names="Arial" Font-Size="11pt"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 100px">
                                &nbsp;</td>
                            <td class="style12">
                                &nbsp;</td>
                            <td style="width: 200px">
                                &nbsp;</td>
                            <td style="width: 600px; vertical-align: top; text-align: left;">
                                &nbsp;</td>
                        </tr>
                        <tr>
                            <td style="width: 100px">
                                &nbsp;</td>
                            <td class="style12">
                                <strong><span class="style8">Ship Via::</span></strong>
                                <asp:Label ID="lblShipVia" runat="server" Font-Names="Arial" Font-Size="11pt"></asp:Label>
                            </td>
                            <td style="width: 200px">
                                &nbsp;</td>
                            <td style="width: 600px; vertical-align: top; text-align: left;">
                                <strong><span class="style8">Due Date:</span></strong>
                                <asp:Label ID="lblDueDate" runat="server" Font-Names="Arial" Font-Size="11pt"></asp:Label>
                            </td>
                        </tr>
                    </table>
                    <br />
                    <table cellpadding="0" cellspacing="0" class="style6" 
                        style="border: 1px solid #808080; font-family: Arial; font-size: 10pt; vertical-align: middle; text-align: center;">
                        <tr style="font-family: Arial; font-size: 11pt; font-weight: bold">
                            <td style="border: 1px solid #808080">
                                Cust I.D.</td>
                            <td style="border: 1px solid #808080">
                                Sales Person</td>
                            <td style="border: 1px solid #808080">
                                Order Date</td>
                            <td style="border: 1px solid #808080">
                                Required Date</td>
                            <td style="border: 1px solid #808080">
                                Ship Date</td>
                            <td style="border: 1px solid #808080">
                                Terms</td>
                            <td style="border: 1px solid #808080">
                                Currency</td>
                        </tr>
                        <tr>
                            <td style="border: 1px solid #808080">
                                <asp:Label ID="lblCustID" runat="server" Font-Names="Arial" Font-Size="11pt"></asp:Label>
                            </td>
                            <td style="border: 1px solid #808080">
                                <asp:Label ID="lblSalesPerson" runat="server" Font-Names="Arial" 
                                    Font-Size="11pt"></asp:Label>
                            </td>
                            <td style="border: 1px solid #808080">
                                <asp:Label ID="lblOrderDate" runat="server" Font-Names="Arial" Font-Size="11pt"></asp:Label>
                            </td>
                            <td style="border: 1px solid #808080">
                                <asp:Label ID="lblRequiredDate" runat="server" Font-Names="Arial" 
                                    Font-Size="11pt"></asp:Label>
                            </td>
                            <td style="border: 1px solid #808080">
                                <asp:Label ID="lblShipDate" runat="server" Font-Names="Arial" Font-Size="11pt"></asp:Label>
                            </td>
                            <td style="border: 1px solid #808080">
                                <asp:Label ID="lblTerms" runat="server" Font-Names="Arial" Font-Size="11pt"></asp:Label>
                            </td>
                            <td style="border: 1px solid #808080">
                                <asp:Label ID="lblCurrency" runat="server" Font-Names="Arial" Font-Size="11pt"></asp:Label>
                            </td>
                        </tr>
                    </table>
                    <table class="style6" style="font-family: Arial; font-size: 10pt">
                        <tr>
                            <td>
                                &nbsp; &nbsp;</td>
                        </tr>
                        <tr>
                            <td class="style4">
                                <strong>Products Sold with this Order:</strong></td>
                        </tr>
                        <tr>
                            <td>
                               
                                
                                
<ig:WebDataGrid ID="wdgRepGrid" runat="server" 
        AltItemCssClass="VerifyGrid_Report_AlternateRow" 
    AutoGenerateColumns="False" Font-Names="Arial" 
        ItemCssClass="VerifyGrid_Report_Row" 
        EnableTheming="True" FooterCaptionCssClass="VerifyGrid_Report_Footer" 
        HeaderCaptionCssClass="VerifyGrid_Report_HeaderGreen" BackColor="White" 
        CssClass="VerifyGrid_Report_Frame" Height="450px">
    <Columns>
         <ig:BoundDataField CssClass="RAlign" DataFieldName="SalesOrderItemId" Key="SalesOrderItemId" 
            Width="120px" DataType="System.Int32" Hidden="True">
            <Header Text="SalesOrderItemId" />
<Header Text="SalesOrderItemId"></Header>
        </ig:BoundDataField>
        <ig:BoundDataField CssClass="RAlign" DataFieldName="ProductId" Key="ProductId" 
            Width="100px" DataType="System.Int32">
            <Header Text="Product ID:" />
<Header Text="Product ID:"></Header>
        </ig:BoundDataField>
        <ig:BoundDataField DataFieldName="Product_Name" Key="Product_Name" Width="280px" 
            EnableMultiline="True" DataType="System.String">
            <Header Text="Product Name:" />
<Header Text="Product Name:"></Header>
        </ig:BoundDataField>
        <ig:BoundDataField DataFieldName="TraceCodeDesc" Key="TraceCodeDesc" Width="110px" 
            DataType="System.String">
            <Header Text="Trace Code:" />
<Header Text="Trace Code:"></Header>
        </ig:BoundDataField>
        <ig:BoundDataField CssClass="RAlign" DataFieldName="QuantityRequested" Key="QuantityRequested" 
            Width="110px" DataType="System.Int32">
            <Header Text="Qty Ordered:" />
<Header Text="Qty Ordered:"></Header>
        </ig:BoundDataField>
         <ig:BoundDataField DataFieldName="WeightRequested" Key="WeightRequested" Width="110px" 
            DataFormatString="{0:#.##}" DataType="System.Decimal" CssClass="RAlign">
            <Header Text="Wgt Ordered:" />
<Header Text="Wgt Ordered:"></Header>
        </ig:BoundDataField>
        <ig:BoundDataField CssClass="RAlign" DataFieldName="Quantity" Key="Quantity" 
            Width="110px" DataType="System.Int32">
            <Header Text="Qty Fulfilled:" />
<Header Text="Qty Fulfilled:"></Header>
        </ig:BoundDataField>
         <ig:BoundDataField DataFieldName="Weight" Key="Weight" Width="110px" 
            DataFormatString="{0:#.##}" DataType="System.Decimal" CssClass="RAlign">
            <Header Text="Wgt Fulfilled:" />
<Header Text="Wgt Fulfilled:"></Header>
        </ig:BoundDataField>
        <ig:BoundDataField DataFieldName="UnitPrice" Key="UnitPrice" Width="120px" 
            DataFormatString="{0:c}" DataType="System.Decimal" CssClass="RAlign">
            <Header Text="Unit Price:" />
<Header Text="Unit Price:"></Header>
        </ig:BoundDataField>
        
        <ig:BoundDataField DataFieldName="TotalPrice" Key="TotalPrice" Width="130px" 
            DataFormatString="{0:c}" DataType="System.Decimal" CssClass="RAlign">
            <Header Text="Total Price:" />
<Header Text="Total Price:"></Header>
        </ig:BoundDataField>
        
    </Columns>
    <Behaviors>
        <ig:Activation ActiveRowCssClass="SelectedRow">
        </ig:Activation>
        <ig:ColumnResizing>
        </ig:ColumnResizing>
        <ig:SummaryRow EmptyFooterText="" SummariesCssClass="SummaryRow">
            <ColumnSummaries>
                <ig:ColumnSummaryInfo ColumnKey="TotalPrice">
                    <Summaries>
                        <ig:Summary SummaryType="Sum" />
                    </Summaries>
                </ig:ColumnSummaryInfo>
               
                <ig:ColumnSummaryInfo ColumnKey="Product_Name">
                    <Summaries>
                        <ig:Summary />
                    </Summaries>
                </ig:ColumnSummaryInfo>
            </ColumnSummaries>
            <SummaryButton HoverImageUrl="~/App_Themes/Buttons/Add_Plus_Small.gif" />

<SummaryButton HoverImageUrl="~/App_Themes/Buttons/Add_Plus_Small.gif"></SummaryButton>
            <ColumnSettings>
                <ig:SummaryRowSetting ColumnKey="TotalPrice">
                </ig:SummaryRowSetting>
            </ColumnSettings>
        </ig:SummaryRow>
        <ig:Filtering Enabled = "true" FilterButtonCssClass="Filter_LAlign">
            <ColumnSettings>
                <ig:ColumnFilteringSetting ColumnKey="CustomerName" Enabled="true" />
            </ColumnSettings>
            
            <EditModeActions EnableOnKeyPress="True" />

<EditModeActions EnableOnKeyPress="True"></EditModeActions>
        </ig:Filtering >
        <ig:Selection CellClickAction="Row" RowSelectType="Single">
        </ig:Selection>
        <ig:Sorting Enabled="True" 
            AscendingImageUrl="~/App_Themes/Buttons/Up_Arrow_small.gif" 
            DescendingImageUrl="~/App_Themes/Buttons/Down_Arrow_small.gif" >
        </ig:Sorting>
    </Behaviors>
</ig:WebDataGrid>
                               
                                
                                
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp; &nbsp;</td>
                        </tr>
                        <tr>
                            <td </td>
                        </tr>
                        <tr>
                            <td>

<asp:Panel ID="pnlReturns" runat="server">
    <asp:Label ID="Label1" runat="server" Font-Names="Arial" Font-Size="11pt" Font-Bold="True">Products Returned with this order</asp:Label>                            
    <ig:WebDataGrid ID="wdgRepReturns" runat="server" AltItemCssClass="VerifyGrid_Report_AlternateRow"
        AutoGenerateColumns="False" Font-Names="Arial" ItemCssClass="VerifyGrid_Report_Row"
        EnableTheming="True" FooterCaptionCssClass="VerifyGrid_Report_Footer" HeaderCaptionCssClass="VerifyGrid_Report_HeaderRed"
        BackColor="White" CssClass="VerifyGrid_Report_Frame" Height="200px">
        <Columns>
            <ig:BoundDataField CssClass="RAlign" DataFieldName="SalesOrderItemId" Key="SalesOrderItemId"
                Width="120px" DataType="System.Int32" Hidden="True">
                <Header Text="SalesOrderItemId" />
                <Header Text="SalesOrderItemId"></Header>
            </ig:BoundDataField>
            <ig:BoundDataField CssClass="RAlign" DataFieldName="ProductId" Key="ProductId" Width="100px"
                DataType="System.Int32">
                <Header Text="Product ID:" />
                <Header Text="Product ID:"></Header>
            </ig:BoundDataField>
            <ig:BoundDataField DataFieldName="Product_Name" Key="Product_Name" Width="280px"
                EnableMultiline="True" DataType="System.String">
                <Header Text="Product Name:" />
                <Header Text="Product Name:"></Header>
            </ig:BoundDataField>
            <ig:BoundDataField DataFieldName="TraceCodeDesc" Key="TraceCodeDesc" Width="110px"
                DataType="System.String">
                <Header Text="Trace Code:" />
                <Header Text="Trace Code:"></Header>
            </ig:BoundDataField>
            <ig:BoundDataField CssClass="RAlign" DataFieldName="QuantityRequested" Key="QuantityRequested"
                Width="110px" DataType="System.Int32">
                <Header Text="Qty Ordered:" />
                <Header Text="Qty Ordered:"></Header>
            </ig:BoundDataField>
            <ig:BoundDataField DataFieldName="WeightRequested" Key="WeightRequested" Width="110px"
                DataFormatString="{0:#.##}" DataType="System.Decimal" CssClass="RAlign">
                <Header Text="Wgt Ordered:" />
                <Header Text="Wgt Ordered:"></Header>
            </ig:BoundDataField>
            <ig:BoundDataField CssClass="RAlign" DataFieldName="Quantity" Key="Quantity" Width="110px"
                DataType="System.Int32">
                <Header Text="Qty Fulfilled:" />
                <Header Text="Qty Fulfilled:"></Header>
            </ig:BoundDataField>
            <ig:BoundDataField DataFieldName="Weight" Key="Weight" Width="110px" DataFormatString="{0:#.##}"
                DataType="System.Decimal" CssClass="RAlign">
                <Header Text="Wgt Fulfilled:" />
                <Header Text="Wgt Fulfilled:"></Header>
            </ig:BoundDataField>
            <ig:BoundDataField DataFieldName="PriceCharged" Key="UnitPrice" Width="120px" DataFormatString="{0:c}"
                DataType="System.Decimal" CssClass="RAlign">
                <Header Text="Unit Price:" />
                <Header Text="Unit Price:"></Header>
            </ig:BoundDataField>
            <ig:BoundDataField DataFieldName="TotalPrice" Key="TotalPrice" Width="130px" DataFormatString="{0:c}"
                DataType="System.Decimal" CssClass="RAlign">
                <Header Text="Total Price:" />
                <Header Text="Total Price:"></Header>
            </ig:BoundDataField>
        </Columns>
        <Behaviors>
            <ig:Activation ActiveRowCssClass="SelectedRow">
            </ig:Activation>
            <ig:ColumnResizing>
            </ig:ColumnResizing>
            <ig:SummaryRow EmptyFooterText="" SummariesCssClass="SummaryRow">
                <ColumnSummaries>
                    <ig:ColumnSummaryInfo ColumnKey="TotalPrice">
                        <Summaries>
                            <ig:Summary SummaryType="Sum" />
                        </Summaries>
                    </ig:ColumnSummaryInfo>
                    <ig:ColumnSummaryInfo ColumnKey="Product_Name">
                        <Summaries>
                            <ig:Summary />
                        </Summaries>
                    </ig:ColumnSummaryInfo>
                </ColumnSummaries>
                <SummaryButton HoverImageUrl="~/App_Themes/Buttons/Add_Plus_Small.gif" />
                <SummaryButton HoverImageUrl="~/App_Themes/Buttons/Add_Plus_Small.gif"></SummaryButton>
                <ColumnSettings>
                    <ig:SummaryRowSetting ColumnKey="TotalPrice">
                    </ig:SummaryRowSetting>
                </ColumnSettings>
            </ig:SummaryRow>
            <ig:Filtering Enabled="true" FilterButtonCssClass="Filter_LAlign">
                <ColumnSettings>
                    <ig:ColumnFilteringSetting ColumnKey="CustomerName" Enabled="true" />
                </ColumnSettings>
                <EditModeActions EnableOnKeyPress="True" />
                <EditModeActions EnableOnKeyPress="True"></EditModeActions>
            </ig:Filtering>
            <ig:Selection CellClickAction="Row" RowSelectType="Single">
            </ig:Selection>
            <ig:Sorting Enabled="True" AscendingImageUrl="~/App_Themes/Buttons/Up_Arrow_small.gif"
                DescendingImageUrl="~/App_Themes/Buttons/Down_Arrow_small.gif">
            </ig:Sorting>
        </Behaviors>
    </ig:WebDataGrid>
</asp:Panel>                                
                                
                                
                            </td>
                        </tr>
                    </table>
                    <br />
                    <table class="style6" style="font-family: Arial; font-size: 10pt">
                        <tr>
                            <td class="style4" style="width: 100px; text-align: left; vertical-align: top;">
                                <strong style="text-align: left; vertical-align: top">Comment:</strong></td>
                            <td style="vertical-align: top; text-align: left">
                                <asp:Label ID="lblComment" runat="server" Font-Names="Arial" 
                                    Font-Size="11pt" Width="689px"></asp:Label>
                            </td>
                            <td>
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;</td>
                            <td>
                                <table class="style3" style="font-family: Arial; font-size: 10pt">
                                    <tr>
                                        <td class="style8" style="width: 120px">
                                            <strong>Sub-Total</strong></td>
                                        <td>
                                <asp:Label ID="lblSubTotal" runat="server" Font-Names="Arial" 
                                    Font-Size="11pt"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="style8" style="width: 120px">
                                <asp:Label ID="lblCaptionDiscount" runat="server" Font-Names="Arial" 
                                    Font-Size="11pt" Font-Bold="True">Discount</asp:Label>
                                        </td>
                                        <td>
                                <asp:Label ID="lblDiscount" runat="server" Font-Names="Arial" 
                                    Font-Size="11pt"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="style8" style="width: 120px">
                                            <strong>Returns</strong></td>
                                        <td>
                                <asp:Label ID="lblReturns" runat="server" Font-Names="Arial" 
                                    Font-Size="11pt"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="style8" style="width: 120px">
                                            <strong>Freight</strong></td>
                                        <td>
                                <asp:Label ID="lblFreight" runat="server" Font-Names="Arial" 
                                    Font-Size="11pt"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="style8" style="width: 120px">
                                            <strong>VAT</strong></td>
                                        <td>
                                <asp:Label ID="lblTax" runat="server" Font-Names="Arial" 
                                    Font-Size="11pt"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="style9" style="width: 120px; background-color: #C0C0C0;">
                                            <strong>TOTAL</strong></td>
                                        <td>
                                <asp:Label ID="lblGrandTotal" runat="server" Font-Names="Arial" 
                                    Font-Size="11pt" Font-Bold="True"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 100px">
                                &nbsp;&nbsp; &nbsp;</td>
                            <td>
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>
                        </tr>
                    </table>
                    <table class="style6" style="font-family: Arial; font-size: 10pt">
                        <tr>
                            <td class="style8">
                                <strong>Customers Signature:&nbsp; _________________________________________________________________________</strong></td>
                            <td style="width: 100px">
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;</td>
                            <td class="style10" style="width: 600px">
                                <strong>Thank you for your Business !</strong></td>
                        </tr>
                    </table>
                    <br />
                    <br />
                    <br />
                    <table style="width: 1126px; font-family: Arial; font-size: 9pt; vertical-align: top; text-align: center">
                        <tr>
                            <td style="Cl">
                             <asp:Label ID="lblFooter" runat="server"  Text="CLAIMS MUST BE MADE UPON RECEIPT OF GOODS. A SERVICE CHARGE ON OVERDUE ACCOUNTS APPLIES.<BR/>ALL CLAIMS FOR SHORTAGES OR DAMAGE MUST BE REPORTED TO OUR DRIVER OR SALES REPRESENTATIVE. BY ACCEPTING THE DELIVERY YOU ACCEPT AND ACKNOWLEDGE OUR CLAIMS POLICY NOTED ABOVE."></asp:Label>
                                </td>
                        </tr>
                    </table>
                    <br />
                    <br />
                    <br />
                </td>
            </tr>
        </table>
    
    </div>
    </form>
</body>
</html>
