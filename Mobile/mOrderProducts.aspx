<%@ Page Title="" Language="VB" MasterPageFile="~/Mobile/mDetailsMaster.master" AutoEventWireup="false" CodeFile="mOrderProducts.aspx.vb" Inherits="Mobile_mOrderProducts" %>

<%@ Register assembly="Infragistics4.Web.v13.2, Version=13.2.20132.2028, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" namespace="Infragistics.Web.UI.DisplayControls" tagprefix="ig" %>


<%@ Register Assembly="Infragistics4.Web.v13.2, Version=13.2.20132.2028, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" namespace="Infragistics.Web.UI.DisplayControls" tagprefix="ig" %>
<%@ Register Assembly="Infragistics4.Web.v13.2, Version=13.2.20132.2028, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" namespace="Infragistics.Web.UI.EditorControls" tagprefix="ig" %>
<%@ Register Assembly="Infragistics4.Web.v13.2, Version=13.2.20132.2028, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" namespace="Infragistics.Web.UI.GridControls" tagprefix="ig" %>
<%@ Register Assembly="Infragistics4.Web.v13.2, Version=13.2.20132.2028, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" namespace="Infragistics.Web.UI.ListControls" tagprefix="ig" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
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

        .igg_Control{width:auto !important; height:auto !important; overflow: visible !important;}
.igg_Control
{
	background-color:#FFFFFF;
	background-repeat:repeat-x;
	font-size:11px;
	border:solid 1px #999999;
}

.ig_Control
{
	background-color:White;
	font-size:xx-small;
	font-family: verdana;
	color:Black;
}


.igg_Control{width:auto !important; height:auto !important; overflow: visible !important;}
.igg_Control
{
	background-color:#FFFFFF;
	background-repeat:repeat-x;
	font-size:11px;
	border:solid 1px #999999;
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


.ig_Control
{
	background-color:White;
	font-size:xx-small;
	font-family: verdana;
	color:Black;
}


.igg_Control{overflow: visible !important;
        }
.igg_Control
{
	background-color:#FFFFFF;
	background-repeat:repeat-x;
	font-size:11px;
	border:solid 1px #999999;
}

.ig_Control
{
	background-color:White;
	font-size:xx-small;
	font-family: verdana;
	color:Black;
}


.igg_Control{overflow: visible !important;
        }
.igg_Control
{
	background-color:#FFFFFF;
	background-repeat:repeat-x;
	font-size:11px;
	border:solid 1px #999999;
}

.ig_Control
{
	background-color:White;
	font-size:xx-small;
	font-family: verdana;
	color:Black;
}


.igg_Header
{
	background-color:#6EA3CC;
	background-repeat:repeat-x;
	background-image: url(C:/Sources/Sales Portal/SalesPortal Cross Browser/ig_res/Default/images/igg_header.gif);
	color: White;
}

.igg_Header
{
	background-color:#6EA3CC;
	background-repeat:repeat-x;
	background-image: url(C:/Sources/Sales Portal/SalesPortal Cross Browser/ig_res/Default/images/igg_header.gif);
	color: White;
}

.igg_Header
{
	background-color:#6EA3CC;
	background-repeat:repeat-x;
	background-image: url(C:/Sources/Sales Portal/SalesPortal Cross Browser/ig_res/Default/images/igg_header.gif);
	color: White;
}

.igg_Header
{
	background-color:#6EA3CC;
	background-repeat:repeat-x;
	background-image: url(C:/Sources/Sales Portal/SalesPortal Cross Browser/ig_res/Default/images/igg_header.gif);
	color: White;
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


    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mProjectMAIN_content" Runat="Server">
     <meta name="viewport" content="width=device-width">
       <div style="text-align: left">
    <asp:Label ID="Label12" runat="server" font-bold="True" font-names="Arial" 
                                    font-size="10pt" forecolor="#B38700" text="Products for order :" 
                                    width="129px" Height="16px"></asp:Label>
                                            <asp:Label ID="lblOrderNum" runat="server" Font-Bold="False" font-names="Arial" 
                                                font-size="9pt" width="147px" Height="16px"></asp:Label>
                                        <br />

               <ig:WebDataGrid ID="wdgOrderItems" runat="server"
                   DataKeyFields="VT_UniqueIndex"
                  AltItemCssClass="VerifyGrid_Report_AlternateRow" 
    AutoGenerateColumns="False" Font-Names="Arial" 
        ItemCssClass="VerifyGrid_Report_Row" 
        EnableTheming="True" FooterCaptionCssClass="VerifyGrid_Report_Footer" 
        HeaderCaptionCssClass="VerifyGrid_Report_HeaderBeige_10pt" BackColor="White" 
        CssClass="VerifyGrid_Report_Frame" Height="100px"  EnableAjax="False" >
                   
                   
 
<Columns>

<ig:TemplateDataField Key="ViewHistory" Width="50px" CssClass="Center" Hidden="true" >
                <ItemTemplate>
                    <asp:ImageButton ID="btnViewHistory" runat="server" 
                        ImageUrl="~/App_Themes/Grid Buttons/ViewEdit.gif"  />
                       </ItemTemplate>
                <Header Text="View History:" />
            </ig:TemplateDataField>

 


                    <ig:BoundDataField DataFieldName="SalesItemNum" Key="SalesItemNum"  Width="50px" DataType="System.Int32" Hidden="true" > 
                        <Header Text="Item:"/>
                     </ig:BoundDataField>
                    <ig:BoundDataField DataFieldName="ProductCode" Key="ProductCode"  Width="80px" > 
                        <Header Text="Prod Code:"/>
                    </ig:BoundDataField>
                    <ig:BoundDataField DataFieldName="CustomerCode" Key="CustomerCode"  Width="80px" Hidden="true"> 
                        <Header Text="Cust Code:"/>
                    </ig:BoundDataField>
                     <ig:BoundDataField DataFieldName="CustomerRev" Key="CustomerRev"  Width="40px" Hidden="true" > 
                        <Header Text="Rev:"/>
                    </ig:BoundDataField>
                    <ig:BoundDataField DataFieldName="ProductName" Key="ProductName"  Width="200px" >
                        <Header Text="Name:"/>
                    </ig:BoundDataField>
                     <ig:BoundDataField DataFieldName="Site" Key="Site"  Width="60px" Hidden="true">
                        <Header Text="Site:"/>
                    </ig:BoundDataField>


                    <ig:BoundDataField DataFieldName="QuantityRequested" Key="QuantityRequested"  Width="70px" CssClass="RAlign" DataType="System.Int32" DataFormatString ="{0:N0}" >
                        <Header Text="Qty:"/>
                    </ig:BoundDataField>
                     <ig:BoundDataField DataFieldName="txtUoM" Key="txtUoM"  Width="70px" Hidden="True">
                        <Header Text="UoM:"/>
                    </ig:BoundDataField>
                    <ig:BoundDataField DataFieldName="WeightRequested" Key="WeightRequested"  Width="50px" CssClass="RAlign" DataType="System.Double" >
                        <Header Text="Wgt:"/>
                    </ig:BoundDataField>



                    <ig:BoundDataField DataFieldName="PO_UnitPrice" Key="PO_UnitPrice" Width="70px" CssClass="RAlign" DataType="System.Double" DataFormatString ="{0:N5}" Hidden="true">
                        <Header Text="PO Unit:"/>
                    </ig:BoundDataField>
                    
                    <ig:BoundDataField DataFieldName="PO_TotalPrice" Key="PO_TotalPrice" Width="76px"  CssClass="RAlign" DataType="System.Double" DataFormatString ="{0:N2}">
                        <Header Text="Sale Price:"/>
                    </ig:BoundDataField>

                    <ig:BoundDataField DataFieldName="PriceDifference" Key="PriceDifference" Width="80px"  CssClass="RAlign" DataType="System.Double" DataFormatString ="{0:N2}" Hidden="true">
                        <Header Text="Difference:"/>
                    </ig:BoundDataField>

                    <ig:BoundDataField DataFieldName="UnitPrice" Key="UnitPrice" Width="70px" CssClass="RAlign" DataType="System.Double" DataFormatString ="{0:N5}" Hidden="true">
                        <Header Text="Unit:"/>
                    </ig:BoundDataField>
                    <ig:BoundDataField DataFieldName="TotalExclVAT" Key="TotalExclVAT"  Width="70px" CssClass="RAlign" DataType="System.Double"  DataFormatString ="{0:N2}" Hidden="true">
                        <Header Text="Price:"/>
                     </ig:BoundDataField>
                    <ig:BoundDataField DataFieldName="VAT" Key="VAT" Width="70px" CssClass="RAlign" DataType="System.Double" DataFormatString ="{0:N2}" Hidden="true">
                        <Header Text="Tax:"/>
                    </ig:BoundDataField>
                    <ig:BoundDataField DataFieldName="TotalPrice" Key="TotalPrice" Width="76px"  CssClass="RAlign" DataType="System.Double" DataFormatString ="{0:N2}">
                        <Header Text="DB price:"/>
                    </ig:BoundDataField>



                     <ig:BoundDataField DataFieldName="DimLength" Key="DimLength" Width="60px" CssClass="RAlign" Hidden="true">
                        <Header Text="Length:"/>
                    </ig:BoundDataField>
                    <ig:BoundDataField DataFieldName="DimWidth" Key="DimWidth" Width="60px" CssClass="RAlign" Hidden="true">
                        <Header Text="Width:"/>
                    </ig:BoundDataField>
                     


                     <ig:BoundDataField DataFieldName="Item_DateRequested" Key="Item_DateRequested" Width="70px" CssClass="RAlign" DataType="System.DateTime" DataFormatString ="{0:d}" Hidden="true">
                        <Header Text="Requested:"/>
                    </ig:BoundDataField>
                    <ig:BoundDataField DataFieldName="Item_DateOut" Key="Item_DateOut" Width="70px" CssClass="RAlign" DataType="System.DateTime" DataFormatString ="{0:d}" Hidden="true">
                        <Header Text="DateOut:"/>
                    </ig:BoundDataField>
                     <ig:BoundDataField DataFieldName="Item_DateArrival" Key="Item_DateArrival" Width="70px" CssClass="RAlign" DataType="System.DateTime" DataFormatString ="{0:d}" Hidden="true">
                        <Header Text="Arrival:"/>
                    </ig:BoundDataField>
                  
 


 <ig:BoundDataField DataFieldName="Comment" Key="Comment" Width="200px" Hidden="true" >
             <Header Text="Comment:"/>
        </ig:BoundDataField>
 <ig:BoundDataField DataFieldName="Item_QuoteReference" Key="Item_QuoteReference" Width="70px" Hidden="true">
             <Header Text="Quote Ref:"/>
        </ig:BoundDataField>

 <ig:BoundDataField DataFieldName="VATRate" 
            Key="VATRate" Width="80px" CssClass="RAlign" Hidden="true">
            <Header Text="Tax Rate:"/>
        </ig:BoundDataField>

 <ig:BoundDataField DataFieldName="Item_SalesDBUnit" 
            Key="Item_SalesDBUnit" Width="80px" CssClass="RAlign" DataType="System.Double" DataFormatString ="{0:N5}" Hidden="true">
            <Header Text="SalesDB Unit:"/>
        </ig:BoundDataField>
 <ig:BoundDataField DataFieldName="Item_SalesDBTotal" 
            Key="Item_SalesDBTotal" Width="80px" CssClass="RAlign" DataType="System.Double" DataFormatString ="{0:N2}" Hidden="true">
            <Header Text="SalesDB Total:"/>
        </ig:BoundDataField>
 <ig:BoundDataField DataFieldName="Item_CustomUnit" 
            Key="Item_CustomUnit" Width="70px" CssClass="RAlign" DataType="System.Double" DataFormatString ="{0:N5}" Hidden="true">
            <Header Text="Custom Unit:"/>
        </ig:BoundDataField>
 <ig:BoundDataField DataFieldName="Item_CustomTotal" 
            Key="Item_CustomTotal" Width="80px" CssClass="RAlign" DataType="System.Double" DataFormatString ="{0:N2}" Hidden="true">
            <Header Text="Custom Total:"/>
        </ig:BoundDataField>
 
 <ig:BoundDataField DataFieldName="Item_PartNumGrandTotal" 
            Key="Item_PartNumGrandTotal" Width="80px" CssClass="RAlign" DataType="System.Int32" DataFormatString ="{0:N0}" Hidden="true">
            <Header Text="P/N Total:"/>
        </ig:BoundDataField>

 
 <ig:BoundDataField Hidden="True" DataFieldName="Item_OnHold" Key="Item_OnHold" Width="30px">
                <Header Text="Item_OnHold"/>
        </ig:BoundDataField>
 <ig:BoundDataField Hidden="True" DataFieldName="Item_OnHoldDate" Key="Item_OnHoldDate" Width="30px">
                <Header Text="Item_OnHoldDate"/>
        </ig:BoundDataField>
 <ig:BoundDataField Hidden="True" DataFieldName="Item_OnHoldPersonResponsible" Key="Item_OnHoldPersonResponsible" Width="30px"  >
                <Header Text="OnHoldPersonResponsible"/>
        </ig:BoundDataField>
 <ig:BoundDataField Hidden="True" DataFieldName="Item_OnHoldPersonResponsibleID" Key="Item_OnHoldPersonResponsibleID" Width="30px"  >
                <Header Text="Item_OnHoldPersonResponsibleID"/>
        </ig:BoundDataField>
 <ig:BoundDataField Hidden="True" DataFieldName="Item_OnHoldTVComment" Key="Item_OnHoldTVComment" Width="30px"  >
                <Header Text="Item_OnHoldTVComment"/>
        </ig:BoundDataField>



<ig:BoundDataField DataFieldName="ProductId" Hidden="True" 
            Key="ProductId" Width="30px" CssClass="RAlign">
            <Header Text="ProductId"/>
        </ig:BoundDataField>
<ig:BoundDataField DataFieldName="SalesOrderItemId" Hidden="True" 
                Key="SalesOrderItemId" Width="30px" CssClass="RAlign">
            <Header Text="OrderItemId"/>
        </ig:BoundDataField>

<ig:BoundDataField DataFieldName="TraceCodeDesc"    Key="TraceCodeDesc" Width="60px" >
            <Header Text="TraceCode"/>
        </ig:BoundDataField>
                       <ig:BoundDataField Hidden="True" Key="LocationId" DataFieldName="LocationId"  Width="30px" >
            <Header Text="LocationId"/>
        </ig:BoundDataField>
<ig:BoundDataField Hidden="True" DataFieldName="AdditionOrder" Key="AdditionOrder" Width="30px"  CssClass="RAlign" >
                <Header Text="AdditionOrder"/>
       </ig:BoundDataField>
<ig:BoundDataField Hidden="True" DataFieldName="UnitOfSale" Key="UnitOfSale" Width="30px"  CssClass="RAlign" >
                <Header Text="UnitOfSale"/>
       </ig:BoundDataField>

<ig:BoundDataField Hidden="True" DataFieldName="ProductSellBy" Key="ProductSellBy" Width="30px"  >
                <Header Text="ProductSellBy"/>
        </ig:BoundDataField>
 <ig:BoundDataField Hidden="True" DataFieldName="UnitPriceCurrency" Key="UnitPriceCurrency" Width="30px"  >
                <Header Text="UnitPriceCurrency"/>
        </ig:BoundDataField>

 <ig:BoundDataField Hidden="True" DataFieldName="ProductCategory" Key="ProductCategory" Width="30px"  >
                <Header Text="ProductCategory"/>
        </ig:BoundDataField>

<ig:BoundDataField Hidden="True" DataFieldName="VT_UniqueIndex" Key="VT_UniqueIndex" Width="30px"  >
                <Header Text="VT_UniqueIndex"/>
        </ig:BoundDataField>


 <ig:BoundDataField Hidden="True" DataFieldName="SalesOrderNum" Key="SalesOrderNum" Width="30px"  >
                <Header Text="SalesOrderNum"/>
        </ig:BoundDataField>

 <ig:BoundDataField Hidden="True" DataFieldName="SalesOrderId" Key="SalesOrderId" Width="30px"  >
                <Header Text="SalesOrderId"/>
        </ig:BoundDataField>
 <ig:BoundDataField Hidden="True" DataFieldName="SO_ContiguousNum" Key="SO_ContiguousNum" Width="30px"  >
                <Header Text="SO_ContiguousNum"/>
        </ig:BoundDataField>

 <ig:BoundDataField Hidden="True" DataFieldName="CustomerId" Key="CustomerId" Width="30px"  >
                <Header Text="CustomerId"/>
        </ig:BoundDataField>
 <ig:BoundDataField Hidden="True" DataFieldName="MachineNum" Key="MachineNum" Width="30px"  >
                <Header Text="MachineNum"/>
        </ig:BoundDataField>
 <ig:BoundCheckBoxField hidden="true"  DataFieldName="Kanban" Key="Kanban" Width="50px" ToolTipChecked="This is a kanban item" ToolTipUnchecked="This is NOT a kanban item" CssClass="Center" ToolTipPartial="Click here to mark this item as kanban">
                <CheckBox CheckedImageUrl="../App_Themes/Grid Icons/Paid.gif"></CheckBox>
                <Header Text="Kanban:" />
            </ig:BoundCheckBoxField>
</Columns>

<Behaviors>
 

                   <ig:Activation Enabled="true" ActiveRowCssClass="SelectedRow" >
                <AutoPostBackFlags ActiveCellChanged="True" />
        </ig:Activation>

 
 <ig:SummaryRow EmptyFooterText="" SummariesCssClass="SummaryRow">
                <ColumnSummaries>
                    
                    <ig:ColumnSummaryInfo ColumnKey="PO_TotalPrice">
                        <Summaries>
                            <ig:Summary SummaryType="Sum" />
                        </Summaries>
                    </ig:ColumnSummaryInfo>
                     <ig:ColumnSummaryInfo ColumnKey="PriceDifference">
                        <Summaries>
                            <ig:Summary SummaryType="Sum" />
                        </Summaries>
                    </ig:ColumnSummaryInfo>
 

                    <ig:ColumnSummaryInfo ColumnKey="TotalExclVAT">
                        <Summaries>
                            <ig:Summary SummaryType="Sum" />
                        </Summaries>
                    </ig:ColumnSummaryInfo>
                     <ig:ColumnSummaryInfo ColumnKey="VAT">
                        <Summaries>
                            <ig:Summary SummaryType="Sum" />
                        </Summaries>
                    </ig:ColumnSummaryInfo>
                     <ig:ColumnSummaryInfo ColumnKey="TotalPrice">
                        <Summaries>
                            <ig:Summary SummaryType="Sum" />
                        </Summaries>
                    </ig:ColumnSummaryInfo>
                </ColumnSummaries>
                <ColumnSettings >
                    <ig:SummaryRowSetting ColumnKey="TotalExclVAT" 
                        EnableColumnSummaryOptions="True" EnableFormatting="True" FormatString=" {1}" 
                        ShowSummaryButton="False">
                    </ig:SummaryRowSetting>
                    
                    <ig:SummaryRowSetting ColumnKey="VAT" 
                        EnableColumnSummaryOptions="True" EnableFormatting="True" FormatString=" {1}" 
                        ShowSummaryButton="False">
                    </ig:SummaryRowSetting>
                    <ig:SummaryRowSetting ColumnKey="TotalPrice" 
                        EnableColumnSummaryOptions="True" EnableFormatting="True" FormatString=" {1}" 
                        ShowSummaryButton="False">
                    </ig:SummaryRowSetting>

                    <ig:SummaryRowSetting ColumnKey="PO_TotalPrice" 
                        EnableColumnSummaryOptions="True" EnableFormatting="True" FormatString=" {1}" 
                        ShowSummaryButton="False">
                    </ig:SummaryRowSetting>
                     <ig:SummaryRowSetting ColumnKey="PriceDifference" 
                        EnableColumnSummaryOptions="True" EnableFormatting="True" FormatString=" {1}" 
                        ShowSummaryButton="False">
                    </ig:SummaryRowSetting>

                     <ig:SummaryRowSetting ColumnKey="ViewHistory" 
                        EnableColumnSummaryOptions="False"  ShowSummaryButton="False">
                    </ig:SummaryRowSetting>
                    <ig:SummaryRowSetting ColumnKey="SalesItemNum" 
                        EnableColumnSummaryOptions="False"  ShowSummaryButton="False">
                    </ig:SummaryRowSetting>
                    <ig:SummaryRowSetting ColumnKey="ProductCode" 
                        EnableColumnSummaryOptions="False"  ShowSummaryButton="False">
                    </ig:SummaryRowSetting>
                    <ig:SummaryRowSetting ColumnKey="CustomerCode" 
                        EnableColumnSummaryOptions="False"  ShowSummaryButton="False">
                    </ig:SummaryRowSetting>
                    <ig:SummaryRowSetting ColumnKey="CustomerRev" 
                        EnableColumnSummaryOptions="False"  ShowSummaryButton="False">
                    </ig:SummaryRowSetting>
                     <ig:SummaryRowSetting ColumnKey="ProductName" 
                        EnableColumnSummaryOptions="False"  ShowSummaryButton="False">
                    </ig:SummaryRowSetting>
                    <ig:SummaryRowSetting ColumnKey="Site" 
                        EnableColumnSummaryOptions="False"  ShowSummaryButton="False">
                    </ig:SummaryRowSetting>
                    <ig:SummaryRowSetting ColumnKey="QuantityRequested" 
                        EnableColumnSummaryOptions="False"  ShowSummaryButton="False">
                    </ig:SummaryRowSetting>
                     <ig:SummaryRowSetting ColumnKey="txtUoM" 
                        EnableColumnSummaryOptions="False"  ShowSummaryButton="False">
                    </ig:SummaryRowSetting>
                    <ig:SummaryRowSetting ColumnKey="WeightRequested" 
                        EnableColumnSummaryOptions="False"  ShowSummaryButton="False">
                    </ig:SummaryRowSetting>
                     <ig:SummaryRowSetting ColumnKey="UnitPrice" 
                        EnableColumnSummaryOptions="False"  ShowSummaryButton="False">
                    </ig:SummaryRowSetting>
                     <ig:SummaryRowSetting ColumnKey="PO_UnitPrice" 
                        EnableColumnSummaryOptions="False"  ShowSummaryButton="False">
                    </ig:SummaryRowSetting>
                     <ig:SummaryRowSetting ColumnKey="DimLength" 
                        EnableColumnSummaryOptions="False"  ShowSummaryButton="False">
                    </ig:SummaryRowSetting>
                     <ig:SummaryRowSetting ColumnKey="Item_DateRequested" 
                        EnableColumnSummaryOptions="False"  ShowSummaryButton="False">
                    </ig:SummaryRowSetting>
                     <ig:SummaryRowSetting ColumnKey="Item_DateOut" 
                        EnableColumnSummaryOptions="False"  ShowSummaryButton="False">
                    </ig:SummaryRowSetting>
                    <ig:SummaryRowSetting ColumnKey="Item_DateArrival" 
                        EnableColumnSummaryOptions="False"  ShowSummaryButton="False">
                    </ig:SummaryRowSetting>

                    
                     <ig:SummaryRowSetting ColumnKey="DimWidth" 
                        EnableColumnSummaryOptions="False"  ShowSummaryButton="False">
                    </ig:SummaryRowSetting>
                   
                    <ig:SummaryRowSetting ColumnKey="Comment" 
                        EnableColumnSummaryOptions="False"  ShowSummaryButton="False">
                    </ig:SummaryRowSetting>
                     <ig:SummaryRowSetting ColumnKey="Item_QuoteReference" 
                        EnableColumnSummaryOptions="False"  ShowSummaryButton="False">
                    </ig:SummaryRowSetting>
                    <ig:SummaryRowSetting ColumnKey="VATRate" 
                        EnableColumnSummaryOptions="False"  ShowSummaryButton="False">
                    </ig:SummaryRowSetting>
                    <ig:SummaryRowSetting ColumnKey="Item_SalesDBUnit" 
                        EnableColumnSummaryOptions="False"  ShowSummaryButton="False">
                    </ig:SummaryRowSetting>

                    <ig:SummaryRowSetting ColumnKey="Item_SalesDBTotal" 
                        EnableColumnSummaryOptions="False"  ShowSummaryButton="False">
                    </ig:SummaryRowSetting>

                    <ig:SummaryRowSetting ColumnKey="Item_CustomUnit" 
                        EnableColumnSummaryOptions="False"  ShowSummaryButton="False">
                    </ig:SummaryRowSetting>

                    <ig:SummaryRowSetting ColumnKey="Item_CustomTotal" 
                        EnableColumnSummaryOptions="False"  ShowSummaryButton="False">
                    </ig:SummaryRowSetting>

                    <ig:SummaryRowSetting ColumnKey="Item_PartNumGrandTotal" 
                        EnableColumnSummaryOptions="False"  ShowSummaryButton="False">
                    </ig:SummaryRowSetting>
                     <ig:SummaryRowSetting ColumnKey="TraceCodeDesc" 
                        EnableColumnSummaryOptions="False"  ShowSummaryButton="False">
                    </ig:SummaryRowSetting>


                </ColumnSettings>
            </ig:SummaryRow>



 <ig:Selection Enabled="true" ></ig:Selection>
</Behaviors>
 </ig:WebDataGrid>
           
           <table><tr><td> <asp:Button ID="btnAddNew" runat="server" CssClass="VT_ActionButton" Height="20px" Text="Add new" Font-Size="9pt" /></td>
               <td> <asp:Button ID="btnAddReturn" runat="server" CssClass="VT_ActionButton" Height="20px" Text="Add Return" Font-Size="9pt" /></td>
               <td> <asp:Button ID="btnEdit" runat="server" CssClass="VT_ActionButton" Height="20px" Text="Edit" Font-Size="9pt" /></td>
               <td> <asp:Button ID="btnDelete" runat="server" CssClass="VT_ActionButton" Height="20px" Text="Delete" Font-Size="9pt" /></td></tr></table>     
           
              <asp:HiddenField id="hdnItemNum" runat ="server" />
          
           </div> 
</asp:Content>

