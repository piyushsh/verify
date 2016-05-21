<%@ Page Language="VB" MasterPageFile= "mMasterPage.master" EnableEventValidation="false"  AutoEventWireup="false" CodeFile="mOrdersOpening.aspx.vb" Inherits="MobilePages_mOrdersOpening" %>

<%@ MasterType VirtualPath="mMasterPage.master" %>




<%@ Register Assembly="Infragistics4.Web.v13.2, Version=13.2.20132.2028, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" namespace="Infragistics.Web.UI.DisplayControls" tagprefix="ig" %>
<%@ Register Assembly="Infragistics4.Web.v13.2, Version=13.2.20132.2028, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" namespace="Infragistics.Web.UI.EditorControls" tagprefix="ig" %>
<%@ Register Assembly="Infragistics4.Web.v13.2, Version=13.2.20132.2028, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" namespace="Infragistics.Web.UI.GridControls" tagprefix="ig" %>
<%@ Register Assembly="Infragistics4.Web.v13.2, Version=13.2.20132.2028, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" namespace="Infragistics.Web.UI.ListControls" tagprefix="ig" %>

<asp:Content ID="Content2" ContentPlaceHolderID="mProjectMAIN_content" runat="server">
    <meta name="viewport" content="width=device-width">

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
    

<body>
    
    <div style="text-align: left">
        <table><tr><td>
                    <table style="width: 312px; float: left; background-color: #EDE5C8; height: 34px;">
                       <tr><td class="auto-style5"> <asp:Label ID="Label10" runat="server" Font-Bold="True" Font-Names="Arial" 
                                    Font-Size="9pt" ForeColor="#4A803D" Text="Status:"></asp:Label></td><td class="auto-style6"> <asp:DropDownList ID="ddlStatus" runat="server" Font-Overline="False" Font-Size="9pt" Font-Underline="False">
                                </asp:DropDownList></td>

                           <td colspan="2">  <asp:CheckBox ID="chkCurrentUserOnly" runat="server" Font-Names="Arial" Font-Size="Smaller" Text="current user only" AutoPostBack="True" Checked="True" />
           
                              </td>
                       </tr>
                         <tr>
                     
           
                           
                         
                               
                            
                            <td class="auto-style5" >
                                <asp:Label ID="Label12" runat="server" Font-Bold="True" Font-Names="Arial" 
                                    Font-Size="9pt" ForeColor="#4A803D" Text="Start:"></asp:Label></td><td class="auto-style6">
<ig:WebDatePicker runat="server" id="dteStart" DisplayModeFormat="d" Width="100px" DropDownCalendarID="webMonthCalendar1" Height="20px">
</ig:WebDatePicker>
                            </td>
                             <td class="auto-style3" >
                                <asp:Label ID="Label1" runat="server" Font-Bold="True" Font-Names="Arial" 
                                    Font-Size="9pt" ForeColor="#4A803D" Text="End:"></asp:Label>

                            </td>
                           
                            <td  >
                                <ig:WebDatePicker runat="server" id="dteEnd" DisplayModeFormat="d" Width="100px" DropDownCalendarID="webMonthCalendar2">
</ig:WebDatePicker>
                               
                               
                            </td>
                       </tr>

                        <tr><td class="auto-style5"> <asp:ImageButton ID="btnClearAllTop" runat="server" 
                                    ImageUrl="~/App_Themes/Billing/Buttons/Clear-All-TOP_m.gif" /></td>
                            <td class="auto-style6"> <asp:ImageButton ID="btnSearch" runat="server" 
                                    ImageUrl="~/App_Themes/Billing/Buttons/Search_m.gif" style="margin-left: 0px" /></td>
                        </tr>
                    </table>
                </td></tr><tr><td>
        <ig:WebDataGrid ID="wdgJobs" runat="server" AltItemCssClass="VerifyGrid_Report_AlternateRow" 
            AutoGenerateColumns="False" BackColor="White" CssClass="VerifyGrid_Report_Frame" 
                        EnableTheming="True" Font-Names="Arial" FooterCaptionCssClass="VerifyGrid_Report_Footer" 
            HeaderCaptionCssClass="VerifyGrid_Report_HeaderGreen_m" Height="193px" 
                         ItemCssClass="VerifyGrid_Report_Row"  >
                        <Columns>
                             <ig:TemplateDataField Key="ViewAttachments" Width="35px" CssClass="Center" Hidden ="true" >
                <ItemTemplate>
                    <asp:ImageButton ID="btnViewAttachments" runat="server" 
                        ImageUrl="~/App_Themes/Grid Buttons/View-Document-Set.gif" onclick="btnViewAttachments_Click" />
                </ItemTemplate>
                <Header Text="Att:" />
            </ig:TemplateDataField>
                            <ig:BoundDataField DataFieldName="SO_ContiguousNum" Key="SO_ContiguousNum" Width="50px">
                                <Header Text="SO num:" />
                            </ig:BoundDataField>
                            <ig:BoundDataField DataFieldName="ExtraData2" Key="PONumber" Width="100px" Hidden ="true" >
                                <Header Text="Cust PO Num:" />
                            </ig:BoundDataField>
                        
                            <ig:BoundDataField DataFieldName="ExtraData1" Key="CustomerName" Width="80px">
                                <Header Text="Customer:" />
                            </ig:BoundDataField>
                            <ig:BoundDataField DataFieldName="JobStatusText" Key="Status" Width="50px" >
                                <Header Text="Status:" />
                            </ig:BoundDataField>
                            <ig:BoundDataField DataFieldName="InvoiceNum" Key="InvoiceNum" Width="90px" Hidden ="true">
                                <Header Text="Invoice num:" />
                            </ig:BoundDataField>
                            <ig:BoundDataField DataFieldName="BatchDateStarted" DataFormatString="{0:d}" DataType="System.DateTime" Key="OrderDate" Width="50px">
                                <Header Text="Date:" />
                            </ig:BoundDataField>
                            <ig:BoundDataField DataFieldName="CustomerOrderDate" DataFormatString="{0:d}" DataType="System.DateTime" Key="CustomerOrderDate" Width="85px" Hidden ="true">
                                <Header Text="Cust. Order Date:" />
                            </ig:BoundDataField>
 
                            <ig:BoundDataField DataFieldName="ExtraData3" DataFormatString="{0:###,###,##0.00}" DataType="System.Decimal" Key="OrderValue" Width="50px">
                                <Header Text="Value:" />
                            </ig:BoundDataField>
                            <ig:BoundDataField DataFieldName="RequestedDeliveryDate" DataFormatString="{0:d}" DataType="System.DateTime" Key="RequestedDelivery" Width="85px" Hidden ="true">
                                <Header Text="Requested Delivery:" />
                            </ig:BoundDataField>

                            <ig:BoundDataField DataFieldName="ReasonOnHold" Key="ReasonOnHold" Width="250px" CssClass = "ColText_8pt" Hidden ="true">
                                <Header Text="On Hold Comment:" />
                            </ig:BoundDataField>
                                 <ig:BoundDataField DataFieldName="Priority" Key="Priority" Width="80px" Hidden ="true">
                                <Header Text="Priority:" />
                            </ig:BoundDataField>
                            <ig:BoundDataField DataFieldName="DeliverTo_DateOut" DataFormatString="{0:d}" DataType="System.DateTime" Key="DeliverTo_DateOut" Width="85px" Hidden ="true">
                                <Header Text="Planned Date Out:" />
                            </ig:BoundDataField>
                            <ig:BoundDataField DataFieldName="Comment" Key="Comment" Width="150px">
                                <Header Text="Order Comment:" />
                            </ig:BoundDataField>
                            <ig:BoundDataField DataFieldName="UploadComplete" Key="UploadComplete" Width="72px" Hidden ="true">
                                <Header Text="Posted to Accounts:" />
                            </ig:BoundDataField>
                        
                            <ig:BoundDataField DataFieldName="OperatorId" Key="LoggedBy" Width="100px" Hidden ="true">
                                <Header Text="Logged By:" />
                            </ig:BoundDataField>
                        
                       

                            <ig:BoundDataField DataFieldName="JobId" Key="SalesOrderNum" Width="62px" Hidden ="true">
                                <Header Text="Internal SO Num:" />
                            </ig:BoundDataField>


                            <ig:BoundDataField DataFieldName="Route" Key="Route" Hidden="True" Width="100px" >
                                <Header Text="Route:" />
                            </ig:BoundDataField>
                             <ig:BoundDataField DataFieldName="Type" Key="OrderType" Hidden="True" Width="70px" >
                                <Header Text="Type:" />
                            </ig:BoundDataField>
                        
                            <ig:BoundDataField DataFieldName="JobId" Hidden="True" Key="JobId" Width="30px">
                                <Header Text="ID:" />
                            </ig:BoundDataField>
                            <ig:BoundDataField DataFieldName="SalesOrderId" Hidden="True" Key="SalesOrderID">
                                <Header Text="SoId:" />
                            </ig:BoundDataField>
                            <ig:BoundDataField DataFieldName="ModelType" Hidden="True" Key="ModelType">
                                <Header Text="ModelType:" />
                            </ig:BoundDataField>
                            <ig:BoundDataField DataFieldName="StatusBefore_OnHold" Hidden="True" Key="StatusBefore_OnHold">
                                <Header Text="StatusBefore_OnHold:" />
                            </ig:BoundDataField>
                            <ig:BoundDataField DataFieldName="SoldTo_Code" Key="SoldTo_Code" Hidden ="true">
                                <Header Text="Cust ref.:" />
                            </ig:BoundDataField>
                            <ig:BoundDataField DataFieldName="LatePlanningComment" Key="LatePlanningComment" Width="150px" Hidden ="true">
                                <Header Text="Late Planning Comment:" />
                            </ig:BoundDataField>
                        </Columns>

                        <Behaviors>

                             
    <ig:Activation ActiveRowCssClass="SelectedRow">
                           
                            </ig:Activation>
                      
                       <ig:ColumnResizing>
                       </ig:ColumnResizing>
                       <ig:Selection CellClickAction="Row" RowSelectType="Single">
         
        </ig:Selection>


                            <ig:Sorting AscendingImageUrl="~/App_Themes/Buttons/Up_Arrow_small.gif" DescendingImageUrl="~/App_Themes/Buttons/Down_Arrow_small.gif" Enabled="True">
                            </ig:Sorting>

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
                      
                            </ig:Filtering>

                                                 



                          

                        </Behaviors>
                    </ig:WebDataGrid>
                    </td></tr></table>
        
                                         <table><tr><td><asp:Button ID="btnEdit" runat="server" CssClass="VT_ActionButton_m" Text="Edit Select Item" OnClick="btnEdit_Click" Font-Size="9pt" /></td>
                                             <td><asp:Button ID="btnNewOrder" runat="server" CssClass="VT_ActionButton_m" Text="Add Sales Order" Font-Size="9pt" /></td></tr></table>   
                                    <table> <tr >
                
       
                   
                            
                            <td >
                                
                                <ig:WebMonthCalendar runat="server" ID="webMonthCalendar1" Font-Size="9pt" Width="100px"></ig:WebMonthCalendar>
                            </td>
                            
                            <td  >
                                
                                <ig:WebMonthCalendar runat="server" ID="webMonthCalendar2" Font-Size="9pt" Width="100px"></ig:WebMonthCalendar>
                            </td>
                        
                        </tr></table>
                                                            
            <asp:Panel ID="Panel3" runat="server" backcolor="White" borderstyle="Solid" style="display:none"
                        borderwidth="1px" Height="196px" width="900px">
                         <br />
                        <br />
                        <center>
                            <strong><span style="FONT-FAMILY: Arial">&nbsp; </span></strong><strong>
                            <span style="FONT-FAMILY: Arial"></span></strong>
                        </center>
                        <center>
                            <span style="FONT-FAMILY: Arial">
                            <asp:Label ID="lblMsg" runat="server" Font-Bold="True" Font-Names="Arial"></asp:Label>
                            </span>&nbsp;<br />
                            <br />
                        </center>
                        <center>
                            <br />
                            <asp:ImageButton ID="cmdOKConfirm" runat="server" 
                                ImageUrl="~/App_Themes/Billing/Buttons/OK.gif" />
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:ImageButton 
                                ID="btnCancel" runat="server" AlternateText="Cancel" 
                                ImageUrl="~/App_Themes/Billing/Buttons/Cancel.gif" />
                        </center>
                     
                    
                    </asp:Panel>
        <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender1" runat="server" 
            BackgroundCssClass="modalBackground" CancelControlID="btnCancel" 
            DropShadow="True" oncancelscript="HideMsgPopup()" PopupControlID="Panel3" 
            TargetControlID="Panel3">
        </ajaxToolkit:ModalPopupExtender>
                        
                            
    </div>

</body>
</html>
    </asp:Content> 
<asp:Content ID="Content3" runat="server" contentplaceholderid="head">
    <style type="text/css">
        .VerifyGrid_Report_Frame {}
        .igmc_Control
{
	background-color: #eee;
	border:1px solid #BBBBBB;
	width: 225px;
	height: 200px;
	padding: 0px;
	margin: 0px;
	font-family: Segoe UI, Verdana, Arial, Georgia, Sans-Serif;
}

.igmc_Header
{
	background-color: #6eb6e5;
	background-repeat:repeat-x;
	background-position:top;
	background-image: url(C:/Sources/Sales Portal/SalesPortal Cross Browser/ig_res/Default/images/igmc_headerbg.png);
	border-bottom: 1px solid #6691B8;
	height: 50px;
}

.igmc_NextPrev
{
	margin: 0;
	padding: 0px 8px;
	font-size: 0.8em;
	color: #FFFFFF;
	
}

.igmc_MonthYear
{
	color: #FFFFFF;
	font-size: 1em;
	font-weight: bold;
	padding: 0px 2px;
}

.igmc_DOW
{
	background-color: #eeeeee;
	background-repeat:repeat-x;
	background-position:top;
	background-image: url(C:/Sources/Sales Portal/SalesPortal Cross Browser/ig_res/Default/images/igmc_dowbg.gif);
	font-size: 0.8em;
	border-width:0px;
	border-bottom:solid 1px #CCCCCC;
	padding: 0px ;
	text-align:center;
	font-weight: bold;
	height: 35px;
}

.igmc_Day.igmc_OtherMonthDay
{
	background-image:none;
	color: #a4a4a4;
	font-size:0.9em;
	font-weight:normal;
}

.igmc_OtherMonthDay
{
	color:#AAAAAA;
	background-image: none;
}

.igmc_Day
{
	background-color:#FFFFFF;
	font-size: 0.9em;
	text-align: center;
	border: 1px solid #FFFFFF;
	font-weight: bold;
}

.igmc_WeekendDay
{
	background-color:#f9f9f9;
	border: 1px solid #f9f9f9;
}

.igmc_TodayDay
{
	background-color: #ff9933;
	color: #FFFFFF;
	border: 1px solid #ff8000;
}

.igmc_Footer
{
	background-color: #FFFFFF;
	background-image: url(C:/Sources/Sales Portal/SalesPortal Cross Browser/ig_res/Default/images/igmc_footerbg.gif);
	background-repeat:repeat-x;
	border-top: 1px solid #dddddd;
	font-size:0.8em;
	text-align:center;	
	height: 35px;
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

        .auto-style3 {
            width: 24px;
        }
        .auto-style5 {
            width: 60px;
        }
        .auto-style6 {
            width: 278px;
        }

        </style>
</asp:Content>
 