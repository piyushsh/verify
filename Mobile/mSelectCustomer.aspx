<%@ Page Title="" Language="VB" MasterPageFile="~/Mobile/mDetailsMaster.master" AutoEventWireup="false" CodeFile="mSelectCustomer.aspx.vb" Inherits="Mobile_mSelectCustomer" %>


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
 <asp:HiddenField ID="hdnDeliveryCust" runat ="server"  />

   <asp:Label ID="Label13" runat="server" Text="Search delivery customer" Font-Bold="True" Font-Names="Arial"></asp:Label>
               <br />
               <br />
               <asp:Label ID="Label14" runat="server" Text="Enter customer name or part of name:"></asp:Label>
               <br />
               <asp:TextBox ID="txtSearchString" runat="server" Height="21px" Width="213px"></asp:TextBox>
               <asp:Button ID="cmdSearch" runat="server" CssClass="VT_ActionButton" Font-Size="9pt" Height="20px" Text="Search" />
               <br />
               Select from the list below and click Select<br />
               <ig:WebDataGrid ID="wdgCust" runat="server" AltItemCssClass="VerifyGrid_Report_AlternateRow" 
                   AutoGenerateColumns="False" BackColor="White" CssClass="VerifyGrid_Report_Frame" 
                   EnableTheming="True" Font-Names="Arial" 
                   FooterCaptionCssClass="VerifyGrid_Report_Footer" HeaderCaptionCssClass="VerifyGrid_Report_HeaderGreen_10pt" 
                   Height="151px" ItemCssClass="VerifyGrid_Report_Row" >
                 
                  
                   <Columns>
                       <ig:BoundDataField DataFieldName="CustomerCode" Key="CustomerCode" Width="50px">
                           <Header Text="Cust code:" />
                       </ig:BoundDataField>
                       <ig:BoundDataField DataFieldName="CustomerName" Key="CustomerName" Width="250px">
                           <Header Text="CustomerName:" />
                       </ig:BoundDataField>
                       <ig:BoundDataField DataFieldName="CustomerID" Key="CustomerID" Width="250px" Hidden ="True">
                           <Header Text="CustomerID:" />
                       </ig:BoundDataField>
                   </Columns>
                   <Behaviors>
                      <ig:Activation ActiveRowCssClass="SelectedRow">
                           
                            </ig:Activation>
                      
                       <ig:ColumnResizing>
                       </ig:ColumnResizing>
                       <ig:Selection CellClickAction="Row" RowSelectType="Single">
         
        </ig:Selection>
                   </Behaviors>
               </ig:WebDataGrid>


               <asp:Button ID="cmdSelect" runat="server" CssClass="VT_ActionButton" Font-Size="9pt" Height="20px" Text="Select" />
               &nbsp;&nbsp;
               <asp:Button ID="cmdCancel" runat="server" CssClass="VT_ActionButton" Font-Size="9pt" Height="20px" Text="Cancel" />
               <br />


        
           </div>

</asp:Content>

