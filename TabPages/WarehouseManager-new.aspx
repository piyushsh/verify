<%@ Page Title="" Language="VB"   AutoEventWireup="false" CodeFile="WarehouseManager-new.aspx.vb"  EnableEventValidation="false" Inherits="TabPages_WarehouseManager" %>



<%@ Register Assembly="Infragistics4.Web.v13.2, Version=13.2.20132.2028, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" namespace="Infragistics.Web.UI.DisplayControls" tagprefix="ig" %>
<%@ Register Assembly="Infragistics4.Web.v13.2, Version=13.2.20132.2028, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" namespace="Infragistics.Web.UI.EditorControls" tagprefix="ig" %>
<%@ Register Assembly="Infragistics4.Web.v13.2, Version=13.2.20132.2028, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" namespace="Infragistics.Web.UI.GridControls" tagprefix="ig" %>
<%@ Register Assembly="Infragistics4.Web.v13.2, Version=13.2.20132.2028, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" namespace="Infragistics.Web.UI.ListControls" tagprefix="ig" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%--   <script type="text/javascript" src="<%= Session("_VT_JQueryFileLocation")%>Scripts/jquery-1.10.1.min.js"></script>--%>


<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    
    <!-- Library CSS Files -->
    <link rel="stylesheet" href="../App_Themes/lib/bootstrap.min.css"/>
    
    <link href="../jg_styles.css" rel="stylesheet" type="text/css" />
    <link href="../Verify_Infragistics.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/VT_Responsive_V1.css" rel="stylesheet" />
    
    <!-- New CSS Files -->
    <link rel="stylesheet" href="../App_Themes/resp/main.css"/>
    <link rel="stylesheet" href="../App_Themes/resp/common.css"/>

    <style type="text/css">
        .VerifyGrid_Report_Frame {
            margin-right: 0px;
        }
            
.VT_ActionButton
{
	border: 1px solid #682225;
	background-color: #FBF2ED;
	font-family: Arial;
	font-style: italic;
	font-weight: bold;
	color: #682225;
	border-radius: 2px 2px 2px 2px;
}
        .auto-style16 {
            height: 32px;
        }
        .auto-style17 {
            height: 28px;
        }
        .auto-style20 {
            float: inherit;
            width: 2102px;
        }
        </style>
        <script language="javascript" type="text/javascript" src="../BusyBox.js"></script>

</head>
<body style="margin-top:0px" onbeforeunload="ShowSpinner();">

 
        <script type="text/javascript" src="..\scripts\json2.js">    </script>
    <script type="text/javascript" src="..\Scripts\jquery-1.10.2.js"></script>
<script type="text/javascript" src="..\Scripts\jquery.tmpl.js"></script>
    
    <!-- Library Scripts -->
    <script src="../Scripts/bootstrap.min.js"></script>
    <script src="../Scripts/respond.min.js"></script>
    
    

     <center>


    <form id="form1" runat="server">


        <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true" AsyncPostBackTimeout="360000">
                <Scripts>
                    <asp:ScriptReference Path="../Scripts/CommonScripts.js" />
                    <asp:ScriptReference Path="../Scripts/VT_Infragistics.js" />
                </Scripts>
            </asp:ScriptManager>

                <iframe id="BusyBox1" name="BusyBox1" frameborder="0" scrolling="no" ondrop="return false;">
                </iframe>
            

              <script language="javascript" type="text/javascript">

                 
            </script>
        
        
        <!-- Header & Navigation -->
    
    <header>
            <div class="navbar navbar-default">
                    <div class="container">
                        <div class="navbar-header">
                            <button type="button" class="navbar-toggle" data-toggle="collapse" data-target=".navbar-collapse">
                                <span class="icon-bar"></span>
                                <span class="icon-bar"></span>
                                <span class="icon-bar"></span>
                            </button>
                            <div class="navbar-brand" runat="server" href="#">
                                <asp:ImageButton ID="btnAnchorPoint" runat="server" ImageUrl="~/App_Themes/TabButtons/AnchorButton_Light.gif" CssClass="menu-btn"/>
                            </div>
                        </div>
                        <div class="navbar-collapse collapse pull-left">
                            <ul class="nav navbar-nav">
                                <li><asp:Button ID="btnTOP_Quotes" runat="server"  Text="Quotes" CssClass="VT_UnSelectTab_Light" BorderStyle="None" /></li>
                                <li><asp:Button ID="btnTOP_Planning" runat="server" Text="Planning" CssClass="VT_UnSelectTab_Light" BorderStyle="None" /></li>
                                <li><asp:Button ID="btnTOP_Orders" runat="server" Text="SalesOrders" CssClass="VT_UnSelectTab_Light" BorderStyle="None" /></li>
                                <li><asp:Button ID="btnTOP_Details" runat="server" Text="Details" CssClass="VT_UnSelectTab_Light" BorderStyle="None" /></li>
                                <li><asp:Button ID="btnTOP_Fulfill" runat="server" Text="Fulfillment" CssClass="VT_UnSelectTab_Light" BorderStyle="None" /></li>
                                <li><asp:Button ID="btnTOP_Deliveries" runat="server" Text="Dispatches" CssClass="VT_UnSelectTab_Light" BorderStyle="None" /></li>
                                <li><asp:Button ID="btnTOP_Printouts" runat="server" Text="Reports" CssClass="VT_UnSelectTab_Light" BorderStyle="None" /></li>
                                <li><asp:Button ID="btnTOP_SliceAndDice" runat="server" Text="SliceAndDice" CssClass="VT_UnSelectTab_Light" BorderStyle="None" /></li>
                                <li><asp:Button ID="btnTOP_Contracts" runat="server" Text="Contracts" CssClass="VT_UnSelectTab_Light" BorderStyle="None" /></li>
                                <li><asp:Button ID="btnTOP_SYSADMIN" runat="server" Text="More..." CssClass="VT_UnSelectTab_Light" BorderStyle="None" /></li>
                                <li class="dropdown">
                                    <a id="more_links" href="" data-toggle="dropdown" role="button" aria-haspopup="true" aria-expanded="false">More Link &gt;&gt;</a>
                                    <ul class="dropdown-menu" aria-labelledby="more_links">
                                        <li><asp:Button ID="Button1" runat="server" Text="More Button..." CssClass="VT_UnSelectTab_Light" BorderStyle="None" /></li>
                                        <li><asp:Button ID="Button2" runat="server" Text="More Button..." CssClass="VT_UnSelectTab_Light" BorderStyle="None" /></li>
                                        <li><asp:Button ID="Button3" runat="server" Text="More Button..." CssClass="VT_UnSelectTab_Light" BorderStyle="None" /></li>
                                        <li role="separator" class="divider"></li>
                                        <li><a href="">Manage Customers link</a></li>
                                        <li role="separator" class="divider"></li>
                                        <li><a href="">Manage Customers link</a></li>
                                        <li><a href="">Manage Customers link</a></li>
                                        <li><a href="">Manage Customers</a></li>
                                        <li role="separator" class="divider"></li>
                                        <li><a href="">Manage Customers</a></li>
                                        <li><a href="">Manage Customers</a></li>
                                        <li><a href="">Manage Customers</a></li>
                                    </ul>
                                </li>
                            </ul>
                            <!-- Verify Logo -->
                        </div>
                        <div class="pull-right logo_container">
                            <asp:Image ID="imgBannerLogo" runat="server" ImageUrl="~/App_Themes/TabButtons/VerifyLogo_Responsive.jpg" CssClass="img-responsive"/>
<%--                            <img src="../assets/images/logo.png"  class="img-responsive"/>--%>
                            <span class="portal_title">Sales Management Portal</span>
                        </div>
                    </div>
                </div>
</header>
        

<div class="container">
    <div class="row page-info">
        <div class="col-md-5 text-left">
            <asp:Label ID="Label1" runat="server" CssClass="page-title" Text="Dispatches"></asp:Label>        
        </div>
        <div class="col-md-6 user_info_container">
            <div class="user-info">
                <span>Welcome : <strong>John Gleeson</strong></span>
                <span>Tue. 09/04/2016</span>
                <span><strong>14.35</strong> (GMT +1)</span>
            </div>
        </div>
        <div class="col-md-1 logout">
            <a href="#"><span class="glyphicon glyphicon-arrow-right"></span> Logout</a>
        </div>
    </div>
    
    <!-- Search Section -->
    <div class="search_section">
        <div class="row">
            <div class="col-md-3 view_type_container">
                <asp:Label ID="Label21" runat="server" CssClass="label-brown" Text="Select View Type"></asp:Label>
                
                <asp:DropDownList ID="ddlView" runat="server" AutoPostBack="True">
                    <asp:ListItem Value="1">Show all order ITEMs awaiting Dispatch</asp:ListItem>
                    <asp:ListItem Value="2">Show sales Orders awaiting Dispatch</asp:ListItem>
                    <asp:ListItem Value="3">Show dispatch dockets between selected dates</asp:ListItem>
                    <asp:ListItem Value="4">Show &#39;part shipped&#39; sales orders currently in the system</asp:ListItem>
                </asp:DropDownList>
                
            </div>
            <div class="col-md-6 filters_container">
                <div class="row">
                    <div class="col-md-2">
                        <asp:Label ID="Label2" runat="server" CssClass="label-brown" Text="Search"></asp:Label>
                    </div>
                    <div class="col-md-2">
                        <asp:Label ID="Label3" runat="server" CssClass="label-normal" Text="Status"></asp:Label>
                        <asp:DropDownList ID="DropDownList_Status" runat="server" AutoPostBack="True">
                            <asp:ListItem Value="1">All</asp:ListItem>
                            <asp:ListItem Value="2">Dumy Value 1</asp:ListItem>
                            <asp:ListItem Value="3">Dumy Value 2</asp:ListItem>
                            <asp:ListItem Value="4">Dumy Value 3</asp:ListItem>
                        </asp:DropDownList>
                    </div>
            
                    <div class="col-md-3 date_filter_container">
                        <div class="label_container">
                            <asp:Label ID="Label4" runat="server" CssClass="label-normal" Text="Date"></asp:Label>
                        </div>
                        <div class="input_container">
                            <div>
                                <ig:WebMonthCalendar runat="server" ID="webMonthCalendar2">
                                </ig:WebMonthCalendar>
                                <ig:WebDatePicker runat="server" id="wdpFrom" DisplayModeFormat="d" CssClass="Infragistrics_Date" DropDownCalendarID="webMonthCalendar2">
                                </ig:WebDatePicker>    
                            </div>
                            <br/>
                            <div>
                                <ig:WebMonthCalendar runat="server" ID="webMonthCalendar1">
                                </ig:WebMonthCalendar>
                                <ig:WebDatePicker runat="server" id="wdpTo" DisplayModeFormat="d" CssClass="Infragistrics_Date" DropDownCalendarID="webMonthCalendar1">
                                </ig:WebDatePicker>
                            </div>
                        </div>
                    </div>
                    
                    <div class="col-md-3">
                        <asp:Label ID="Label6" runat="server" CssClass="label-normal" Text="Customer"></asp:Label>
                        <asp:DropDownList ID="DropDownList_Customer" runat="server" AutoPostBack="True">
                            <asp:ListItem Value="1">All</asp:ListItem>
                            <asp:ListItem Value="2">Dumy Value 1</asp:ListItem>
                            <asp:ListItem Value="3">Dumy Value 2</asp:ListItem>
                            <asp:ListItem Value="4">Dumy Value 3</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    
                    <div class="col-md-2 clear_all_filters_container">
                        <a href="#"><span class="glyphicon glyphicon-minus-sign"></span> Clear All Filters</a>
                    </div>
                </div>
            </div>
            <div class="col-md-3 view_type_title_container">
                <asp:Label ID="Label7" runat="server" CssClass="title" Text="Order Items Awaiting Dispatch"></asp:Label>  
            </div>
        </div>
    </div>
    <!-- Advance Search -->
    <div class="advance_search_section">
        <div class="row">
            <div class="col-md-2 col-md-offset-10 advance_search_link_container">
                <a href="#"><span class="glyphicon glyphicon-chevron-down"></span> ADVANCED SEARCH</a>
            </div>
        </div>
    </div>

</div>


    <div style="margin-left: 10px">

        <asp:Label ID="lblOrderType" runat="server" Visible="False" Font-Names="Arial" 
                            Font-Size="10pt"></asp:Label>

        <br />
                               
                                            <asp:Label ID="Label5" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="14pt" ForeColor="Black" Text="Dispatches Management dashboard   [ Sales Orders ]"></asp:Label>
                                        &nbsp;&nbsp;&nbsp;

        <br />

        <table cellpadding="0" cellspacing="0"  style="width: 100%">
            <tr>
                <td>
                    
                    
                   <div id="Div1"  style=" width:900px; margin: 0 auto;" runat="server">

                           <div class="VT_TabBackground_Light" style="width: 900px; margin: 0 auto">

                            <div  style="width: 900px; margin: 0 auto">
                                     <table cellpadding="0" cellspacing="0" class="VT_TabBackground_Light">
                                                            <tr style="height: 32px">
                                                                <td >

                                                                    <table cellpadding="0" cellspacing="0" >
                                                                        <tr>
                                                                            <td>
                                                                                
                                                                            </td>
                                                                            <td>
                                                                                <asp:Image ID="Image2" runat="server" AlternateText="Select View Type" ImageUrl="~/App_Themes/Images/ArrowSolid.png" />

                                                                            </td>
                                                                        </tr>
                                                                    </table>

                                                                </td>
                                                                <td>&nbsp;</td>
                                                                <td>
                               
<%--                                                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:DropDownList ID="ddlView" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="11pt" Width="400px" AutoPostBack="True">--%>
<%--                                                                        <asp:ListItem Value="1">Show all order ITEMs awaiting Dispatch</asp:ListItem>--%>
<%--                                                                        <asp:ListItem Value="2">Show sales Orders awaiting Dispatch</asp:ListItem>--%>
<%--                                                                        <asp:ListItem Value="3">Show dispatch dockets between selected dates</asp:ListItem>--%>
<%--                                                                        <asp:ListItem Value="4">Show &#39;part shipped&#39; sales orders currently in the system</asp:ListItem>--%>
<%--                                                                    </asp:DropDownList>--%>
                                                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Button ID="btnBackTop" runat="server" CssClass="VT_ActionButton" Text="View Sales Portal Tabs" />
                                                                    &nbsp;  


                                                                </td>
                                                            </tr>
                                                        </table>

                            </div>

                               <asp:Panel ID="pnlDateSelect" runat="server">
                                   <table class="auto-style16">
                                       <tr>
                                           <td style="vertical-align: top; text-align: right">
                                               <asp:Label ID="lblFrom" runat="server" Text="From:" Font-Bold="True" Font-Names="Arial" Font-Size="11pt"></asp:Label>
                                           </td>
                                           <td class="auto-style17">
                                               
                                           </td>
                                           <td style="vertical-align: top; text-align: right">
                                               <asp:Label ID="lblTo" runat="server" Text="To:" Font-Bold="True" Font-Names="Arial" Font-Size="11pt"></asp:Label>
                                           </td>
                                           <td class="auto-style17">
                                               
                                           </td>
                                           <td class="auto-style17">
                                               <asp:Button ID="btnRefresh" runat="server" Text="Refresh" />
                                           </td>
                                       </tr>
                                   </table>
                               </asp:Panel>

                              <br />


                        </div>
                            
                            
                       
                            

                        </div> 
                    
                </td>
            </tr>
            <tr>
                <td>
                                            <asp:Label ID="Label11" runat="server" Font-Italic="True" ForeColor="#993300" Text="Number of Order Items:" Visible="False"></asp:Label>
                               
                                &nbsp;&nbsp;&nbsp;<asp:Label ID="lblNumItems" runat="server" Text="Label" Visible="False"></asp:Label>
                                            &nbsp;
                    <div><ig:WebDataGrid ID="wdgOrders" runat="server" AltItemCssClass="VerifyGrid_Report_AlternateRow" AutoGenerateColumns="False" BackColor="White" CssClass="VerifyGrid_Report_Frame" EnableAjax="False" EnableTheming="True" Font-Names="Arial" FooterCaptionCssClass="VerifyGrid_Report_Footer" HeaderCaptionCssClass="VerifyGrid_Report_HeaderGreen_10pt" Height="200px" ItemCssClass="VerifyGrid_Report_Row" Visible="False">
                        <Columns>
                            <ig:BoundDataField DataFieldName="OrderNumber" Key="OrderNumber" Width="100px">
                                <Header Text="Order:" />
                            </ig:BoundDataField>
                            <ig:BoundDataField DataFieldName="CustomerName" Key="CustomerName" Width="300px">
                                <Header Text="Customer:" />
                            </ig:BoundDataField>
                            <ig:BoundDataField DataFieldName="RequestedDeliveryDate" DataFormatString="{0:d}" Key="DueDate" Width="150px">
                                <Header Text="Due Date:" />
                            </ig:BoundDataField>
                            <ig:BoundDataField DataFieldName="Priority" Key="Priority" Width="300px">
                                <Header Text="Priority:" />
                            </ig:BoundDataField>
                            <ig:BoundDataField DataFieldName="DocketStatus" Key="DocketStatus" Width="200px">
                                <Header Text="Status:" />
                            </ig:BoundDataField>
                        </Columns>
                        <Behaviors>
                            <ig:Activation ActiveRowCssClass="SelectedRow">
                                <AutoPostBackFlags ActiveCellChanged="True" />
                            </ig:Activation>
                            <ig:ColumnResizing>
                            </ig:ColumnResizing>
                            <ig:SummaryRow EmptyFooterText="" SummariesCssClass="SummaryRow">
                                <ColumnSummaries>
                                    <ig:ColumnSummaryInfo ColumnKey="CustomerName">
                                        <Summaries>
                                            <ig:Summary SummaryType="Count" />
                                        </Summaries>
                                    </ig:ColumnSummaryInfo>
                                    <ig:ColumnSummaryInfo ColumnKey="CustomerName">
                                        <Summaries>
                                            <ig:Summary />
                                        </Summaries>
                                    </ig:ColumnSummaryInfo>
                                </ColumnSummaries>
                                <SummaryButton HoverImageUrl="~/App_Themes/Buttons/Add_Plus_Small.gif" />
                                <ColumnSettings>
                                    <ig:SummaryRowSetting ColumnKey="CustomerName">
                                        <SummarySettings>
                                            <ig:SummarySetting SummaryType="Count" />
                                            <ig:SummarySetting />
                                        </SummarySettings>
                                    </ig:SummaryRowSetting>
                                </ColumnSettings>
                            </ig:SummaryRow>
                            <ig:Filtering Enabled="true" FilterButtonCssClass="Filter_LAlign">
                                <ColumnSettings>
                                    <ig:ColumnFilteringSetting ColumnKey="CustomerName" Enabled="true" />
                                </ColumnSettings>
                                <ColumnFilters>
                                    <ig:ColumnFilter ColumnKey="CustomerName">
                                        <ConditionWrapper>
                                            <ig:RuleTextNode />
                                        </ConditionWrapper>
                                    </ig:ColumnFilter>
                                </ColumnFilters>
                                <EditModeActions EnableOnKeyPress="True" />
                            </ig:Filtering>
                            <ig:Sorting AscendingImageUrl="~/App_Themes/Buttons/Up_Arrow_small.gif" DescendingImageUrl="~/App_Themes/Buttons/Down_Arrow_small.gif" Enabled="True">
                            </ig:Sorting>
                        </Behaviors>
                    </ig:WebDataGrid></div>
                                            &nbsp; &nbsp;</td>
            </tr>
            <tr>
                <td>

              
              <ig:WebDataGrid ID="wdgRepGrid" runat="server"
                  DataKeyFields="VT_UniqueIndex"
                  AltItemCssClass="VerifyGrid_Report_AlternateRowSmall"
                  AutoGenerateColumns="False" Font-Names="Arial"
                  ItemCssClass="VerifyGrid_Report_RowSmall"
                  EnableTheming="True" FooterCaptionCssClass="VerifyGrid_Report_Footer"
                  HeaderCaptionCssClass="VerifyGrid_Report_HeaderGreenSmall" BackColor="White"
                  CssClass="VerifyGrid_Report_Frame" Height="500px" EnableAjax ="false" >
                  
                
                  <Columns>

                      <ig:GroupField Key="GroupField_CustomerDetails" >
                        <Columns>
                              <ig:BoundDataField DataFieldName="Customer" Key="Customer" Width="200px"
                                  DataType="System.String">
                                  <Header Text="Name" />
                              </ig:BoundDataField>
                              <ig:BoundDataField DataFieldName="SONum" Key="SONum" Width="100px"
                                  DataType="System.String">
                                  <Header Text="S.O. Num" />
                              </ig:BoundDataField>
                            
                        </Columns>
                        <Header Text="Customer Details" />
                     </ig:GroupField>


                     <ig:GroupField Key="GroupField_ProductDetails" >
                        <Columns>
                               <ig:BoundDataField DataFieldName="ProdLine" Key="ProdLine" Width="50px"
                                  DataType="System.String" Hidden ="true" >
                                  <Header Text="Prod. line" />
                              </ig:BoundDataField>
                      
                              <ig:BoundDataField DataFieldName="LineK" Key="LineK" Width="50px"
                                  DataType="System.String" Hidden ="true">
                                  <Header Text="Line K" />
                              </ig:BoundDataField>
                             <ig:BoundDataField DataFieldName="ProductName" Key="ProductName"  Width="100px" > 
                                <Header Text="Name:"/>
                             </ig:BoundDataField>
                              <ig:BoundDataField DataFieldName="ItemNum" Key="ItemNum"  Width="70px" > 
                                <Header Text="Code:"/>
                             </ig:BoundDataField>

                        </Columns>
                        <Header Text="Product Details" />
                     </ig:GroupField>

                      <ig:GroupField Key="GroupField_OrderDetails" >
                        <Columns>
                          
                     
                         
                          <ig:BoundDataField DataFieldName="DueDate" Key="DueDate" Width="70px"
                              DataType="System.String">
                              <Header Text="Due Date:" />
                          </ig:BoundDataField>

                          <ig:BoundDataField DataFieldName="QtyOrdered" Key="QtyOrdered" Width="70px"
                               DataType="System.Double">
                              <Header Text="Qty:" />
                          </ig:BoundDataField>
                          <ig:BoundDataField DataFieldName="WgtOrdered" Key="WgtOrdered" Width="70px"
                               DataType="System.Double">
                              <Header Text="Wgt:" />
                          </ig:BoundDataField>
                          <ig:BoundDataField DataFieldName="UOM" Key="UOM" Width="50px"
                              DataType="System.String">
                              <Header Text="UOM:" />
                          </ig:BoundDataField>
                            
                        </Columns>
                        <Header Text="Order Details" />
                     </ig:GroupField>

                      <ig:GroupField Key="GroupField_OnHand" >
                        <Columns>

                            <ig:BoundDataField DataFieldName="QtyOnHand" Key="QtyOnHand" Width="70px" 
                              DataType="System.String">
                              <Header Text="Qty:" />
                          </ig:BoundDataField>

                        </Columns>
                        <Header Text="Inventory Available" />
                     </ig:GroupField>

                       <ig:GroupField Key="GroupField_Shipped" >
                        <Columns>
                             <ig:BoundDataField DataFieldName="TraceCode" Key="Tracecode"  Width="90px" > 
                            <Header Text="Trace code:"/>
                              </ig:BoundDataField>
                            <ig:BoundDataField DataFieldName="QtyShipped" Key="QtyShipped" Width="70px"
                               DataType="System.Double">
                              <Header Text="Qty:" />
                          </ig:BoundDataField>
                           <ig:BoundDataField DataFieldName="WgtShipped" Key="WgtShipped" Width="70px"
                               DataType="System.Double">
                              <Header Text="Wgt:" />
                          </ig:BoundDataField>
                             <ig:BoundDataField DataFieldName="DocketNum" Key="DocketNum" Width="90px" 
                                  DataType="System.String" Hidden ="true">
                                  <Header Text="Dispatch #:" />
                              </ig:BoundDataField>
                        </Columns>
                        <Header Text="Shipped Details" />
                     </ig:GroupField>

                       <ig:GroupField Key="GroupField_Planning" >
                        <Columns>
                            <ig:BoundDataField DataFieldName="QtyToInvoice" Key="QtyToInvoice" Width="90px" 
                              DataType="System.String" Hidden ="true">
                              <Header Text="Invoice Qty:" />
                          </ig:BoundDataField>
                          <ig:BoundDataField DataFieldName="OpenSOQty" Key="OpenSOQty" Width="70px" 
                              DataType="System.String">
                              <Header Text="Open SOQty:" />
                          </ig:BoundDataField>
                          <ig:BoundDataField DataFieldName="AwaitProduction" Key="AwaitProduction" Width="76px" 
                              DataType="System.String">
                              <Header Text="Production" />
                          </ig:BoundDataField>
                          <ig:BoundDataField DataFieldName="AwaitPicking" Key="AwaitPicking" Width="70px" 
                              DataType="System.String">
                              <Header Text="Picking" />
                          </ig:BoundDataField>
                          <ig:BoundDataField DataFieldName="AwaitInvoicing" Key="AwaitInvoicing" Width="70px" 
                              DataType="System.String">
                              <Header Text="Invoicing." />
                          </ig:BoundDataField>
                          <ig:BoundDataField DataFieldName="PercentReadyToShip" Key="PercentReadyToShip" Width="70px" 
                              DataType="System.String">
                              <Header Text="% ready:" />
                          </ig:BoundDataField>

                            </Columns>
                        <Header Text="Planning Details " />
                     </ig:GroupField>

                       <ig:GroupField Key="GroupField_Jobs" >
                        <Columns>
                             <ig:BoundDataField DataFieldName="JobId" Key="JobId"  Width="90px"> 
                            <Header Text="JobId:"/>
                              </ig:BoundDataField>
                               <ig:BoundDataField DataFieldName="JobStatus" Key="JobStatus"  Width="90px"> 
                            <Header Text="Job Status:"/>
                              </ig:BoundDataField>
                            
                                                       
                        </Columns>
                        <Header Text="Job Details" />
                     </ig:GroupField>

                                           
                      <ig:BoundDataField DataFieldName="PriceSoldFor" Key="PriceSoldFor" Width="70px" 
                          DataType="System.String" Hidden ="true" >
                          <Header Text="Price sold for:" />
                      </ig:BoundDataField>
                      <ig:BoundDataField DataFieldName="Location" Key="Location" Width="70px" 
                          DataType="System.String" Hidden ="true" >
                          <Header Text="Location:" />
                      </ig:BoundDataField>
                      <ig:BoundDataField DataFieldName="RecordId" Key="RecordId" Width="70px" 
                          DataType="System.String" Hidden ="true" >
                          <Header Text="Record Id:" />
                      </ig:BoundDataField>
                      <ig:BoundDataField DataFieldName="VatCharged" Key="VatCharged" Width="70px" 
                          DataType="System.String" Hidden ="true" >
                          <Header Text="Vat Charged:" />
                      </ig:BoundDataField>
                      <ig:BoundDataField DataFieldName="SerialNum" Key="SerialNum" Width="70px" 
                          DataType="System.String" Hidden ="true" >
                          <Header Text="SeriaNum:" />
                      </ig:BoundDataField>
                      <ig:BoundDataField DataFieldName="Barcode" Key="Barcode" Width="70px" 
                          DataType="System.String" Hidden ="true" >
                          <Header Text="Barcode:" />
                      </ig:BoundDataField>
                      <ig:BoundDataField DataFieldName="SaleType" Key="SaleType" Width="70px" 
                          DataType="System.String" Hidden ="true" >
                          <Header Text="Sale type:" />
                      </ig:BoundDataField>
              
                      <ig:BoundDataField DataFieldName="InvoiceNum" Key="InvoiceNum" Width="70px" 
                          DataType="System.String" Hidden ="true" >
                          <Header Text="Invoice #:" />
                      </ig:BoundDataField>

                  </Columns>

                  

                  <Behaviors>
                      <ig:EditingCore>
                          <Behaviors>
                              <ig:CellEditing Enabled="false">
                                  <ColumnSettings>
                                    

                                      <ig:EditingColumnSetting ColumnKey="SONum" ReadOnly="True" />
                                      <ig:EditingColumnSetting ColumnKey="ItemNum" ReadOnly="True"  />

                                     <ig:EditingColumnSetting ColumnKey="DueDate" ReadOnly="True" />
                                      <ig:EditingColumnSetting ColumnKey="QtyOrdered" ReadOnly="True" />
                                      <ig:EditingColumnSetting ColumnKey="UOM" ReadOnly="True" />
                                      <ig:EditingColumnSetting ColumnKey="QtyOnHand" ReadOnly="True" />
                                      <ig:EditingColumnSetting ColumnKey="QtyShipped" ReadOnly="True" />
                                      <ig:EditingColumnSetting ColumnKey="QtyToInvoice" ReadOnly="True" />
                                      <ig:EditingColumnSetting ColumnKey="OpenSoQty" ReadOnly="True" />
                                      <ig:EditingColumnSetting ColumnKey="ProdLine" ReadOnly="True" />
                                      <ig:EditingColumnSetting ColumnKey="Customer" ReadOnly="True" />
                                      <ig:EditingColumnSetting ColumnKey="Linek" ReadOnly="True" />
                                   

                                  </ColumnSettings>
                              </ig:CellEditing>
                          </Behaviors>
                      </ig:EditingCore>

                      <ig:Activation ActiveRowCssClass="SelectedRow">
                      </ig:Activation>
                      <ig:ColumnResizing>
                      </ig:ColumnResizing>

                      <ig:SummaryRow EmptyFooterText="" SummariesCssClass="SummaryRow">
                          <ColumnSummaries>
                              <ig:ColumnSummaryInfo ColumnKey="SONum">
                                  <Summaries>
                                      <ig:Summary SummaryType="Count" />
                                  </Summaries>
                              </ig:ColumnSummaryInfo>
                               <ig:ColumnSummaryInfo ColumnKey="QtyOrdered">
                                  <Summaries>
                                      <ig:Summary SummaryType="Sum" />
                                  </Summaries>
                              </ig:ColumnSummaryInfo>
                                 <ig:ColumnSummaryInfo ColumnKey="WgtOrdered">
                                  <Summaries>
                                      <ig:Summary SummaryType="Sum" />
                                  </Summaries>
                              </ig:ColumnSummaryInfo>
                               <ig:ColumnSummaryInfo ColumnKey="QtyShipped">
                                  <Summaries>
                                      <ig:Summary SummaryType="Sum" />
                                  </Summaries>
                              </ig:ColumnSummaryInfo>
                               <ig:ColumnSummaryInfo ColumnKey="WgtShipped">
                                  <Summaries>
                                      <ig:Summary SummaryType="Sum" />
                                  </Summaries>
                              </ig:ColumnSummaryInfo>
                          </ColumnSummaries>
  
                          <ColumnSettings>
                              
                                   <ig:SummaryRowSetting ColumnKey="SONum" 
                                        EnableColumnSummaryOptions="True" ShowSummaryButton="False">
                                    </ig:SummaryRowSetting>

                                     <ig:SummaryRowSetting ColumnKey="ItemNum" 
                                         EnableColumnSummaryOptions="False"  ShowSummaryButton="False">
                                    </ig:SummaryRowSetting>
                                    <ig:SummaryRowSetting ColumnKey="DueDate" 
                                         EnableColumnSummaryOptions="False"  ShowSummaryButton="False">
                                    </ig:SummaryRowSetting>
                               <ig:SummaryRowSetting ColumnKey="ProductName" 
                                         EnableColumnSummaryOptions="False"  ShowSummaryButton="False">
                                    </ig:SummaryRowSetting>
                               <ig:SummaryRowSetting ColumnKey="Tracecode" 
                                         EnableColumnSummaryOptions="False"  ShowSummaryButton="False">
                                    </ig:SummaryRowSetting>
                                    <ig:SummaryRowSetting ColumnKey="QtyOrdered" 
                                         EnableColumnSummaryOptions="False"  ShowSummaryButton="False">
                                    </ig:SummaryRowSetting>
                               <ig:SummaryRowSetting ColumnKey="WgtOrdered" 
                                         EnableColumnSummaryOptions="False"  ShowSummaryButton="False">
                                    </ig:SummaryRowSetting>
                                    <ig:SummaryRowSetting ColumnKey="UOM" 
                                         EnableColumnSummaryOptions="False"  ShowSummaryButton="False">
                                    </ig:SummaryRowSetting>
                                    <ig:SummaryRowSetting ColumnKey="QtyOnHand" 
                                         EnableColumnSummaryOptions="False"  ShowSummaryButton="False">
                                    </ig:SummaryRowSetting>
                                    <ig:SummaryRowSetting ColumnKey="QtyShipped" 
                                         EnableColumnSummaryOptions="False"  ShowSummaryButton="False">
                                    </ig:SummaryRowSetting>
                              <ig:SummaryRowSetting ColumnKey="WgtShipped" 
                                         EnableColumnSummaryOptions="False"  ShowSummaryButton="False">
                                    </ig:SummaryRowSetting>
                                    <ig:SummaryRowSetting ColumnKey="QtyToInvoice" 
                                         EnableColumnSummaryOptions="False"  ShowSummaryButton="False">
                                    </ig:SummaryRowSetting>
                                     <ig:SummaryRowSetting ColumnKey="OpenSOQty" 
                                         EnableColumnSummaryOptions="False"  ShowSummaryButton="False">
                                    </ig:SummaryRowSetting>
                                     <ig:SummaryRowSetting ColumnKey="ProdLine" 
                                         EnableColumnSummaryOptions="False"  ShowSummaryButton="False">
                                    </ig:SummaryRowSetting>
                                 <ig:SummaryRowSetting ColumnKey="Customer" 
                                         EnableColumnSummaryOptions="False"  ShowSummaryButton="False">
                                    </ig:SummaryRowSetting>
                                 <ig:SummaryRowSetting ColumnKey="LineK" 
                                         EnableColumnSummaryOptions="False"  ShowSummaryButton="False">
                                    </ig:SummaryRowSetting>
                              <ig:SummaryRowSetting ColumnKey="AwaitProduction" 
                                         EnableColumnSummaryOptions="False"  ShowSummaryButton="False">
                                    </ig:SummaryRowSetting>
                              <ig:SummaryRowSetting ColumnKey="AwaitPicking" 
                                         EnableColumnSummaryOptions="False"  ShowSummaryButton="False">
                                    </ig:SummaryRowSetting>
                              <ig:SummaryRowSetting ColumnKey="AwaitInvoicing" 
                                         EnableColumnSummaryOptions="False"  ShowSummaryButton="False">
                                    </ig:SummaryRowSetting>
                              <ig:SummaryRowSetting ColumnKey="PercentReadyToShip" 
                                         EnableColumnSummaryOptions="False"  ShowSummaryButton="False">
                                    </ig:SummaryRowSetting>
                                <ig:SummaryRowSetting ColumnKey="DocketNum" 
                                         EnableColumnSummaryOptions="False"  ShowSummaryButton="False">
                                    </ig:SummaryRowSetting>

                                <ig:SummaryRowSetting ColumnKey="InvoiceNum" 
                                         EnableColumnSummaryOptions="False"  ShowSummaryButton="False">
                                    </ig:SummaryRowSetting>

                                <ig:SummaryRowSetting ColumnKey="Location" 
                                         EnableColumnSummaryOptions="False"  ShowSummaryButton="False">
                                    </ig:SummaryRowSetting>
                                 <ig:SummaryRowSetting ColumnKey="JobId" 
                                         EnableColumnSummaryOptions="False"  ShowSummaryButton="False">
                                    </ig:SummaryRowSetting>
                                   <ig:SummaryRowSetting ColumnKey="JobStatus" 
                                         EnableColumnSummaryOptions="False"  ShowSummaryButton="False">
                                    </ig:SummaryRowSetting>
                        
                          </ColumnSettings>
                      </ig:SummaryRow>


                      <ig:Filtering Enabled="true" FilterButtonCssClass="Filter_LAlign">
                          <ColumnSettings>
                              <ig:ColumnFilteringSetting ColumnKey="SONum" Enabled="true" />
                          </ColumnSettings>
                          <ColumnFilters>
                              <ig:ColumnFilter ColumnKey="SONum">
                                  <ConditionWrapper>
                                      <ig:RuleTextNode />
                                  </ConditionWrapper>
                              </ig:ColumnFilter>
                          </ColumnFilters>
                          <EditModeActions EnableOnKeyPress="True" />

                          <EditModeActions EnableOnKeyPress="True"></EditModeActions>
                      </ig:Filtering>
                      <ig:Sorting Enabled="True"
                          AscendingImageUrl="~/App_Themes/Buttons/Up_Arrow_small.gif"
                          DescendingImageUrl="~/App_Themes/Buttons/Down_Arrow_small.gif">
                      </ig:Sorting>

                      <ig:Selection RowSelectType="Multiple">
                      </ig:Selection>

                  </Behaviors>

              </ig:WebDataGrid>

                </td>
            </tr>
            <tr>
          <td style="border: 1px solid #808080; vertical-align: top; display: inline-block; align-content:center; text-align: center; display: block; " class="auto-style20">

              
<asp:Button ID="btnEdit" runat="server" CssClass="VT_ActionButton" Text="Edit Selected Item" Width="110px" />
                                    
                            &nbsp;<asp:Button ID="btnAddBatch" runat="server" CssClass="VT_ActionButton" Text="Add New Batch" Width="110px" />
                                    
                            &nbsp;<asp:Button ID="btnAddItem" runat="server" CssClass="VT_ActionButton" Text="Add New Item" Width="110px" />
                                    
                            &nbsp;<asp:Button ID="btnDeleteItem" runat="server" CssClass="VT_ActionButton" Text="Delete selected item" Width="130px" />
                                    
                            &nbsp;
                                <asp:Button ID="btnViewStock" runat="server" CssClass="VT_ActionButton" Text="View product stock position" Visible="False" Width="186px" />
                                    
                            &nbsp;
                                <asp:Button ID="btnAddSkid" runat="server" CssClass="VT_ActionButton" Text="Add skid to order" Width="160px" />
                                    
                            &nbsp;<asp:Button ID="btnCompleteOrder" runat="server" CssClass="VT_ActionButton" Text="Complete order" />
                                    
                            &nbsp; <asp:Button ID="btnPrintDispDoc" runat="server" CssClass="VT_ActionButton" Text="Print Dispatch Docket" Width="180px" />
                                    
                            &nbsp; <asp:Button ID="btnPrintDispDocwPrices" runat="server" CssClass="VT_ActionButton" Text="Print Dispatch Docket w/ prices" Width="210px" />
                                    
                            &nbsp;<asp:Button ID="btnPrintInvoice" runat="server" CssClass="VT_ActionButton" Text="Print Invoice" />
                                    
                           &nbsp;<asp:Button ID="btnPrintCCP" runat="server" CssClass="VT_ActionButton" Text="Print CCP" />
                                    
             &nbsp;<asp:Button ID="btnMergeDockets" runat="server" CssClass="VT_ActionButton" Text="Merge dockets" />
                                    
             &nbsp; <asp:Button ID="btnAssignTruck" runat="server" CssClass="VT_ActionButton" Text="Assign truck" />
                 &nbsp;<asp:Button ID="btnDispatchDashboard" runat="server" CssClass="VT_ActionButton" Text="Dispatch docs" />
                 &nbsp; <asp:Button ID="btnExecuteJob" runat="server" CssClass="VT_ActionButton" Text="View/Create Job" />
                &nbsp;&nbsp;&nbsp;<asp:Label ID="lblBatchForJob" runat="server" Text="BatchName"  Font-Bold="True" Visible="False"></asp:Label>
                                            &nbsp;
               <asp:TextBox ID="txtBatchForJob" runat="server" Width="100px" Font-Names="Arial" 
                                    Font-Size="9pt"   ></asp:TextBox>
                                    
              <br />
                                    
              <br />
&nbsp;<asp:Button ID="btnSendForPicking" runat="server" CssClass="VT_ActionButton" Text="Send for picking" Width="120px" />
                                    
                        &nbsp; &nbsp;<asp:Button ID="btnExport" runat="server" CssClass="VT_ActionButton" Text="Export to Excel" Width="120px" />
                                    
                        &nbsp;&nbsp;
                                <asp:Button ID="btnWarehouseTV" runat="server" CssClass="VT_ActionButton" Text="Warehouse TV" Width="120px" />
                                    
               &nbsp;&nbsp;&nbsp;&nbsp;
               <asp:CheckBox ID="chkShowRedandAmberOnly" runat="server" Height="23px" Text="Show only red and amber items on TV" Font-Names="Arial" Font-Size="9pt" />
              &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
          </td>
            </tr>
            <tr>
                <td>
                    <ig:WebExcelExporter ID="VerifyExporter" runat="server">
        </ig:WebExcelExporter>
                
                </td>
            </tr>
            <tr>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td>
                                <asp:ImageMap ID="ImageMap2" runat="server" ImageUrl="~/APP_THEMES/Footer/Footer-Verify.gif">
                                                    <asp:RectangleHotSpot Bottom="41" Left="2" NavigateUrl="Http://www.VerifyTechnologies.com"
                                                        Right="925" Target="_blank" Top="2" />
                                                </asp:ImageMap>

                </td>
            </tr>
        </table>

        <br />
        <br />


<ig:WebDataGrid ID="wdgOrderItems" runat="server"
                   DataKeyFields="VT_UniqueIndex"
                  
                   AltItemCssClass="VerifyGrid_Report_AlternateRow" 
    AutoGenerateColumns="False" Font-Names="Arial" 
        ItemCssClass="VerifyGrid_Report_Row" 
        EnableTheming="True" FooterCaptionCssClass="VerifyGrid_Report_Footer" 
        HeaderCaptionCssClass="VerifyGrid_Report_HeaderBeige_10pt" BackColor="White" 
        CssClass="VerifyGrid_Report_Frame" Height="8px" Width="4px" EnableAjax="False">
                   
 <Columns>

     

 <ig:GroupField Key="GroupField_Product">
                <Columns>
                    <ig:BoundDataField DataFieldName="SalesItemNum" Key="SalesItemNum"  Width="50px" > 
                        <Header Text="Item:"/>
                     </ig:BoundDataField>
                    <ig:BoundDataField DataFieldName="ProductCode" Key="ProductCode"  Width="80px" > 
                        <Header Text="Steri Code:"/>
                    </ig:BoundDataField>
                    <ig:BoundDataField DataFieldName="CustomerCode" Key="CustomerCode"  Width="80px" > 
                        <Header Text="Cust Code:"/>
                    </ig:BoundDataField>
                     <ig:BoundDataField DataFieldName="CustomerRev" Key="CustomerRev"  Width="40px" > 
                        <Header Text="Rev:"/>
                    </ig:BoundDataField>
                    <ig:BoundDataField DataFieldName="ProductName" Key="ProductName"  Width="200px" >
                        <Header Text="Name:"/>
                    </ig:BoundDataField>
                     

                       </Columns>
            <Header Text="Product"/>
         </ig:GroupField>

 <ig:GroupField Key="GroupField_Amount">
                <Columns>
                    <ig:BoundDataField DataFieldName="QuantityRequested" Key="QuantityRequested"  Width="70px" CssClass="RAlign" DataType="System.Int32" DataFormatString ="{0:N0}" >
                        <Header Text="Qty:"/>
                    </ig:BoundDataField>
                     <ig:BoundDataField DataFieldName="txtUoM" Key="txtUoM"  Width="70px" Hidden="True">
                        <Header Text="UoM:"/>
                    </ig:BoundDataField>
                    <ig:BoundDataField DataFieldName="WeightRequested" Key="WeightRequested"  Width="50px" CssClass="RAlign" DataType="System.Double" Hidden="True">
                        <Header Text="Wgt:"/>
                    </ig:BoundDataField>
                       </Columns>
            <Header Text="Amount"/>
         </ig:GroupField>

 <ig:GroupField Key="GroupField_Pricing">
                <Columns>
                    <ig:BoundDataField DataFieldName="UnitPrice" Key="UnitPrice" Width="70px" CssClass="RAlign" DataType="System.Double" DataFormatString ="{0:N3}">
                        <Header Text="Unit:"/>
                    </ig:BoundDataField>
                    <ig:BoundDataField DataFieldName="TotalExclVAT" Key="TotalExclVAT"  Width="70px" CssClass="RAlign" DataType="System.Double"  DataFormatString ="{0:N2}">
                        <Header Text="Price:"/>
                     </ig:BoundDataField>
                    <ig:BoundDataField DataFieldName="VAT" Key="VAT" Width="70px" CssClass="RAlign" DataType="System.Double" DataFormatString ="{0:N2}">
                        <Header Text="Tax:"/>
                    </ig:BoundDataField>
                    <ig:BoundDataField DataFieldName="TotalPrice" Key="TotalPrice" Width="76px"  CssClass="RAlign" DataType="System.Double" DataFormatString ="{0:N2}">
                        <Header Text="Total:"/>
                    </ig:BoundDataField>
                       </Columns>
            <Header Text="System Pricing"/>
         </ig:GroupField>

     <ig:GroupField Key="GroupField_PO_Pricing">
                <Columns>
                    <ig:BoundDataField DataFieldName="PO_UnitPrice" Key="PO_UnitPrice" Width="70px" CssClass="RAlign" DataType="System.Double" DataFormatString ="{0:N3}">
                        <Header Text="PO Unit:"/>
                    </ig:BoundDataField>
                    
                    <ig:BoundDataField DataFieldName="PO_TotalPrice" Key="PO_TotalPrice" Width="76px"  CssClass="RAlign" DataType="System.Double" DataFormatString ="{0:N2}">
                        <Header Text="PO Total:"/>
                    </ig:BoundDataField>

                    <ig:BoundDataField DataFieldName="PriceDifference" Key="PriceDifference" Width="80px"  CssClass="RAlign" DataType="System.Double" DataFormatString ="{0:N2}">
                        <Header Text="Difference:"/>
                    </ig:BoundDataField>
                       </Columns>
            <Header Text="PO Pricing"/>
         </ig:GroupField>

     <ig:GroupField Key="GroupField_Dimensions">
                <Columns>
                     <ig:BoundDataField DataFieldName="DimLength" Key="DimLength" Width="60px" CssClass="RAlign" >
                        <Header Text="Length:"/>
                    </ig:BoundDataField>
                    <ig:BoundDataField DataFieldName="DimWidth" Key="DimWidth" Width="60px" CssClass="RAlign" >
                        <Header Text="Width:"/>
                    </ig:BoundDataField>
                </Columns>
            <Header Text="Dimensions"/>
         </ig:GroupField>

     <ig:GroupField Key="GroupField_Dates">
                <Columns>
                     <ig:BoundDataField DataFieldName="Item_DateRequested" Key="Item_DateRequested" Width="70px" CssClass="RAlign" DataType="System.DateTime" DataFormatString ="{0:d}">
                        <Header Text="Requested:"/>
                    </ig:BoundDataField>
                    <ig:BoundDataField DataFieldName="Item_DateOut" Key="Item_DateOut" Width="70px" CssClass="RAlign" DataType="System.DateTime" DataFormatString ="{0:d}">
                        <Header Text="DateOut:"/>
                    </ig:BoundDataField>
                     <ig:BoundDataField DataFieldName="Item_DateArrival" Key="Item_DateArrival" Width="70px" CssClass="RAlign" DataType="System.DateTime" DataFormatString ="{0:d}">
                        <Header Text="Arrival:"/>
                    </ig:BoundDataField>
                </Columns>
            <Header Text="Dates"/>
         </ig:GroupField>

 
    <ig:BoundDataField DataFieldName="Comment" Key="Comment" Width="200px" >
             <Header Text="Comment:"/>
        </ig:BoundDataField>
     <ig:BoundDataField DataFieldName="Item_QuoteReference" Key="Item_QuoteReference" Width="70px" >
             <Header Text="Quote Ref:"/>
        </ig:BoundDataField>

      <ig:BoundDataField DataFieldName="VATRate" 
            Key="VATRate" Width="80px" CssClass="RAlign">
            <Header Text="Tax Rate:"/>
        </ig:BoundDataField>

     <ig:BoundDataField DataFieldName="Item_SalesDBUnit" 
            Key="Item_SalesDBUnit" Width="80px" CssClass="RAlign" DataType="System.Double" DataFormatString ="{0:N3}">
            <Header Text="SalesDB Unit:"/>
        </ig:BoundDataField>
     <ig:BoundDataField DataFieldName="Item_SalesDBTotal" 
            Key="Item_SalesDBTotal" Width="80px" CssClass="RAlign" DataType="System.Double" DataFormatString ="{0:N2}">
            <Header Text="SalesDB Total:"/>
        </ig:BoundDataField>
      <ig:BoundDataField DataFieldName="Item_CustomUnit" 
            Key="Item_CustomUnit" Width="70px" CssClass="RAlign" DataType="System.Double" DataFormatString ="{0:N3}">
            <Header Text="Custom Unit:"/>
        </ig:BoundDataField>
     <ig:BoundDataField DataFieldName="Item_CustomTotal" 
            Key="Item_CustomTotal" Width="80px" CssClass="RAlign" DataType="System.Double" DataFormatString ="{0:N2}">
            <Header Text="Custom Total:"/>
        </ig:BoundDataField>
     
     <ig:BoundDataField DataFieldName="Item_PartNumGrandTotal" 
            Key="Item_PartNumGrandTotal" Width="80px" CssClass="RAlign" DataType="System.Int32" DataFormatString ="{0:N0}">
            <Header Text="P/N Total:"/>
        </ig:BoundDataField>

    
     <ig:BoundDataField Hidden="True" DataFieldName="Item_OnHold" Key="Item_OnHold" Width="30px"  >
                <Header Text="Item_OnHold"/>
        </ig:BoundDataField>
          <ig:BoundDataField Hidden="True" DataFieldName="Item_OnHoldDate" Key="Item_OnHoldDate" Width="30px"  >
                <Header Text="Item_OnHoldDate"/>
        </ig:BoundDataField>
     <ig:BoundDataField Hidden="True" DataFieldName="Item_OnHoldPersonResponsible" Key="Item_OnHoldPersonResponsible" Width="30px"  >
                <Header Text="OnHoldPersonResponsible"/>
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

 <ig:BoundDataField DataFieldName="TraceCodeDesc" Hidden="True"   Key="TraceCodeDesc" Width="30px" >
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
     <ig:BoundDataField Hidden="True" DataFieldName="Site" Key="Site"  Width="60px"  >
                        <Header Text="Site:"/>
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



                </ColumnSettings>
            </ig:SummaryRow>

 

    
 </Behaviors>
                </ig:WebDataGrid>
  
                


            <%--  <ig:WebMonthCalendar runat="server" ID="webMonthCalendar1"></ig:WebMonthCalendar>
              <ig:WebMonthCalendar runat="server" ID="webMonthCalendar2"></ig:WebMonthCalendar>--%>

              
                
              <asp:HiddenField ID="lblPerUnitWeight" runat="server" />
               
    <asp:HiddenField ID="hidCalcWgtFromQty" runat="server" />
                  
                <asp:HiddenField ID="hdnTotalPrice" runat="server" />
                   
                <asp:HiddenField ID="hdnUnitOfSale" runat="server" />

                     <asp:HiddenField ID="hdnPCOnly" runat="server" />
               
    
                <asp:HiddenField ID="hdnTotalVAT" runat="server" />
                  
                <br />
                <asp:HiddenField ID="hdnProductValue" runat="server" />

   
    
     </div>

    </form>

         </center>
</body>
</html>


