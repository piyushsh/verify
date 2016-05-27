<%@ Page Title="" Language="VB" MasterPageFile="~/Mobile/mDetailsMaster.master" AutoEventWireup="false" CodeFile="mOrderDetails.aspx.vb" Inherits="Mobile_mOrderDetails" %>


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

        function SelectCustomer(sender, e) {

            VTPostback(sender, e);

        }

        function ShowCustSearchPopup() {
            var modal = $find("<%=ModalPopupSearchCust.BehaviorID%>");
                 modal.show();


                 return false;
        }
        function HideSearchCustPopup() {
            var modal = $find("<%=ModalPopupSearchCust.BehaviorID%>");
            modal.hide();
        }

        function SearchDeliveryCust() {
        

            // Create an XMLHttpRequest object
            if (window.XMLHttpRequest) {
                request = new XMLHttpRequest();
            }
            else if (window.ActiveXObject) {
                request = new ActiveXObject("Microsoft.XMLHTTP");
            }
          

            // If the request is created successfully...
            if (request) {
                var txtSearchString = document.getElementById("<%=txtSearchString.ClientID%>");
                var SearchString = txtSearchString.value;

                var ddlBillingCustomer = document.getElementById("<%=ddlBillingCustomer.ClientID%>");
                var BillingCust = ddlBillingCustomer.value;

                request.onreadystatechange = ReceiveCustInquiryRows;
                request.open("GET", "DeliveryCustXML.aspx?SearchString=" + SearchString + "&BillingCust=" + BillingCust, true);
                request.send(null);

            }
        }

        function wdgCust_Initialize(sender, args) {
            //    grid = sender;

        }
        function wdgCust_Activation_ActiveCellChanging(sender, eventArgs) {
            ///<summary>  
            ///  
            ///</summary>  
            ///<param name="sender" type="Infragistics.Web.UI.WebDataGrid"></param>  
            ///<param name="eventArgs" type="Infragistics.Web.UI.ActiveCellChangingEventArgs"></param>  

            //Add code to handle your event here.  


            PageMethods.SetSessionVariableValue('CustomerId', sender.get_behaviors().get_selection().get_selectedRows(0).getItem(0).get_cellByColumnKey("CustomerId").get_text());

           
        }// -->  
        function ClearCustInquiryGrid(grid) {

            dataSource = grid.get_dataSource();
            var dataSourceLength = dataSource.length;
            for (var i = 0; i < dataSourceLength; i++) {
                dataSource.pop();
            }
        
            grid.set_dataSource(dataSource);
            grid.applyClientBinding();

        }
        function ReceiveCustInquiryRows(e) {
            if (request.readyState == 4 && request.status == 200) {
                // delete the existing rows in the grid
                var grid = $find("<%=wdgCust.ClientID%>");
              //Empty the grid
              // $(grid._elements.dataTbl.lastChild).empty();


              // Get data from the XML document
              var xml = request.responseXML;
              var dataRows = xml.getElementsByTagName("CustParts");

              //first add empty rows to fill with data
              // for (var x = 0; x < dataRows.length; x++) {
              //    rows = new Array(grid.get_columns().get_length(), dataRows.length); // create a new empty row

              // }
              // var oRows = grid.get_rows();
              //  grid.get_behaviors().get_editingCore().get_behaviors().get_rowDeleting().deleteRows(oRows);

              // var nLines = grid.get_rows().get_length();
              // for (i = 0; i < nLines; i++) {
              //     grid.get_rows().remove(grid.get_rows().get_row(i));
              // }
              ClearCustInquiryGrid(grid);

              var dataSource = grid.get_dataSource();
         
              for (var x = 0; x < dataRows.length; x++) {
                  var dataRow = dataRows[x];
                  var CC = dataRow.getElementsByTagName("CustomerCode")[0];
                  var CN = dataRow.getElementsByTagName("CustomerName")[0];
                  var SP = dataRow.getElementsByTagName("CustomerId")[0];
                 

                  // var SN = dataRow.getElementsByTagName("SerialNum")[0];
                  //  var BC = dataRow.getElementsByTagName("Barcode")[0];

                  dataSource.push({ "CustomerCode": CC.text, "CustomerName": CN.text, "CustomerId": SP.text });

              }

              grid.set_dataSource(dataSource);
              grid.get_ajaxIndicator().show(grid);
              grid.applyClientBinding();
              grid.get_ajaxIndicator().hide(grid);


          }
      }

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

        <table><tr><td>    <asp:Label ID="lblCust0" runat="server" Font-Bold="True" 
                                                Font-Names="Arial" Font-Size="9pt">Order num:</asp:Label></td><td class="auto-style1"> 
                                            <asp:Label ID="lblOrderNum" runat="server" Font-Bold="False" font-names="Arial" 
                                                font-size="9pt" width="147px" Height="16px"></asp:Label>
                                        </td><td>
            <asp:Button ID="btnSave" runat="server" CssClass="VT_ActionButton" Height="20px" Text="Save" Font-Size="9pt" />
                </td></tr>
            <tr><td>    <asp:Label ID="lblCust" runat="server" Font-Bold="True" 
                                                Font-Names="Arial" Font-Size="9pt">Customer:</asp:Label></td><td class="auto-style1"> <asp:DropDownList ID="ddlBillingCustomer" runat="server" Height="22px" 
                                                Width="150px" AutoPostBack="True" Font-Size="9pt">
                                                <asp:ListItem> Sample Customer</asp:ListItem>
                                            </asp:DropDownList></td><td></td></tr>
            <tr><td>
                            <span style="font-size: 11pt"><strong>
                                            <asp:Label ID="lblJobIdCaption1" runat="server" Font-Bold="True" 
                                                Font-Names="Arial" Font-Size="9pt">Deliver To:</asp:Label>
                                            </strong></span>
                            </td><td class="auto-style1">
                                            <asp:Label ID="lblDeliverTo" runat="server" Font-Bold="False" font-names="Arial" 
                                                font-size="9pt" width="148px" Height="16px"></asp:Label>
                                        </td><td>
            <asp:Button ID="btnSearch" runat="server" CssClass="VT_ActionButton" Height="20px" Text="Search" Font-Size="9pt" />
                </td></tr>
            <tr><td></td><td class="auto-style1">
                <asp:TextBox ID="txtNewCust" runat="server" Font-Size="9pt" Height="22px" Width="150px"></asp:TextBox>
                </td><td>
            <asp:Button ID="btnAddCust" runat="server" CssClass="VT_ActionButton" Height="20px" Text="Add" Font-Size="9pt" />
                </td></tr>
            <tr><td>
                            <span style="font-size: 11pt"><strong>
                                            <asp:Label ID="lblOrderDate" runat="server" Font-Bold="True" 
                                                Font-Names="Arial" Font-Size="9pt">Order date:</asp:Label>
                                            </strong></span>
                            </td><td class="auto-style1">
<ig:WebDatePicker runat="server" id="dteOrderDate" DisplayModeFormat="d" Width="150px" DropDownCalendarID="webMonthCalendar1" Height="20px">
</ig:WebDatePicker>
                            </td><td></td></tr>
            <tr><td>
                            <span style="font-size: 11pt"><strong>
                                            <asp:Label ID="lblDelDate" runat="server" Font-Bold="True" 
                                                Font-Names="Arial" Font-Size="9pt" Width="80px">Delivery date:</asp:Label>
                                            </strong></span>
                            </td><td class="auto-style1">
                                <ig:WebDatePicker runat="server" id="dteDelDate" DisplayModeFormat="d" Width="150px" DropDownCalendarID="webMonthCalendar2">
</ig:WebDatePicker>
                               
                               
                            </td><td>
                    &nbsp;</td></tr>
                        <tr><td>
                            <span style="font-size: 11pt"><strong>
                                            <asp:Label ID="Label1" runat="server" Font-Bold="True" 
                                                Font-Names="Arial" Font-Size="9pt" Width="80px">Customer PO:</asp:Label>
                                            </strong></span>
                            </td><td class="auto-style1">
                               
                               
                               
                <asp:TextBox ID="txtCustPO" runat="server" Font-Size="9pt" Height="22px" Width="150px"></asp:TextBox>
                               
                               
                               
                            </td><td>
                </td></tr>
        </table>
           <table><tr><td>
                            <span style="font-size: 11pt"><strong>
                                            <asp:Label ID="lblDelDate0" runat="server" Font-Bold="True" 
                                                Font-Names="Arial" Font-Size="9pt" Height="16px" Width="80px">Comment:</asp:Label>
                                            </strong></span>
                            </td><td>
                <asp:TextBox ID="txtComment" runat="server" Font-Size="9pt" Height="50px" Width="212px" TextMode="MultiLine"></asp:TextBox>
                </td></tr>
               <tr><td>
                            <span style="font-size: 11pt"><strong>
                                            <asp:Label ID="lblDelDate3" runat="server" Font-Bold="True" 
                                                Font-Names="Arial" Font-Size="9pt" Height="16px" Width="80px">Payment:</asp:Label>
                                            </strong></span>
                            </td><td>
                       <asp:RadioButtonList ID="rbPaymenttype" runat="server" Font-Size="9pt">
                           <asp:ListItem Selected="True">Cash</asp:ListItem>
                           <asp:ListItem>Credit</asp:ListItem>
                       </asp:RadioButtonList>
                   </td></tr>
           </table>
              <asp:Label ID="Label12" runat="server" font-bold="True" font-names="Arial" 
                                    font-size="10pt" forecolor="#B38700" text="Customer Summary:" 
                                    width="230px"></asp:Label>
                                <table><tr><td>
                            <span style="font-size: 11pt"><strong>
                                            <asp:Label ID="lblDelDate1" runat="server" Font-Bold="True" 
                                                Font-Names="Arial" Font-Size="9pt" Height="16px" Width="77px">Total due:</asp:Label>
                                            </strong></span>
                                    </td><td>
                            <span style="font-size: 11pt"><strong>
                                            <asp:Label ID="lblTotalDue" runat="server" Font-Bold="True" 
                                                Font-Names="Arial" Font-Size="9pt" Height="16px" Width="77px"></asp:Label>
                                            </strong></span>
                                    </td></tr>
                                    <tr><td>
                            <span style="font-size: 11pt"><strong>
                                            <asp:Label ID="lblDelDate2" runat="server" Font-Bold="True" 
                                                Font-Names="Arial" Font-Size="9pt" Height="16px" Width="77px">On hold;</asp:Label>
                                            </strong></span>
                                        </td><td>
                            <span style="font-size: 11pt"><strong>
                                            <asp:Label ID="lblOnHold" runat="server" Font-Bold="True" 
                                                Font-Names="Arial" Font-Size="9pt" Height="16px" Width="77px"></asp:Label>
                                            </strong></span></td></tr>
                                </table>
            <asp:Button ID="btnprevious" runat="server" CssClass="VT_ActionButton" Height="20px" Text="View previous orders" Font-Size="9pt" />
                <br />
              <ig:WebMonthCalendar runat="server" ID="webMonthCalendar1" Font-Size="9pt" Width="100px"></ig:WebMonthCalendar>
               <ig:WebMonthCalendar runat="server" ID="webMonthCalendar2" Font-Size="9pt" Width="100px"></ig:WebMonthCalendar>
                                                    
                    <ig:WebDataGrid ID="wdgCustomerItems" runat="server"
                   DataKeyFields="VT_UniqueIndex"
                  
                   AltItemCssClass="VerifyGrid_Report_AlternateRow" 
    AutoGenerateColumns="False" Font-Names="Arial" 
        ItemCssClass="VerifyGrid_Report_Row" 
        EnableTheming="True" FooterCaptionCssClass="VerifyGrid_Report_Footer" 
        HeaderCaptionCssClass="VerifyGrid_Report_HeaderBeige_10pt" BackColor="White" 
        CssClass="VerifyGrid_Report_Frame" Height="380px" Width="764px" Visible="False">
                   
 <Columns>
                    <ig:BoundDataField DataFieldName="SoldTo_CategoryID" Key="SoldTo_CategoryID"  Width="70px" > 
                        <Header Text="SoldTo_CategoryID:"/>
                    </ig:BoundDataField>
                    <ig:BoundDataField DataFieldName="SoldTo_Category" Key="SoldTo_Category"  Width="70px" > 
                        <Header Text="SoldTo_Category:"/>
                    </ig:BoundDataField>
                    <ig:BoundDataField DataFieldName="SoldTo_Id" Key="SoldTo_Id"  Width="70px" >
                        <Header Text="SoldTo_Id:"/>
                    </ig:BoundDataField>
                    <ig:BoundDataField DataFieldName="SoldTo_Name" Key="SoldTo_Name"  Width="60px" >
                        <Header Text="SoldTo_Name:"/>
                    </ig:BoundDataField>
                     <ig:BoundDataField DataFieldName="SoldTo_Code" Key="SoldTo_Code"  Width="60px" >
                        <Header Text="SoldTo_Code:"/>
                    </ig:BoundDataField>
                    <ig:BoundDataField DataFieldName="SoldTo_Address" Key="SoldTo_Address"  Width="60px" >
                        <Header Text="SoldTo_Address:"/>
                    </ig:BoundDataField>
                    <ig:BoundDataField DataFieldName="SoldTo_OnHold" Key="SoldTo_OnHold"  Width="70px" > 
                        <Header Text="SoldTo_OnHold:"/>
                    </ig:BoundDataField>
                    <ig:BoundDataField DataFieldName="SoldTo_FaxNum" Key="SoldTo_FaxNum"  Width="70px" >
                        <Header Text="SoldTo_FaxNum:"/>
                    </ig:BoundDataField>
                    <ig:BoundDataField DataFieldName="SoldTo_TaxExempt" Key="SoldTo_TaxExempt"  Width="60px" >
                        <Header Text="SoldTo_Discount:"/>
                    </ig:BoundDataField>
                     <ig:BoundDataField DataFieldName="SoldTo_ContactId" Key="SoldTo_ContactId"  Width="60px" >
                        <Header Text="SoldTo_ContactId:"/>
                    </ig:BoundDataField>
                    <ig:BoundDataField DataFieldName="SoldTo_ContactName" Key="SoldTo_ContactName"  Width="60px" >
                        <Header Text="SoldTo_ContactName:"/>
                    </ig:BoundDataField>
                    <ig:BoundDataField DataFieldName="SoldTo_Phone" Key="SoldTo_Phone"  Width="60px" >
                        <Header Text="SoldTo_Phone:"/>
                    </ig:BoundDataField>
                    <ig:BoundDataField DataFieldName="SoldTo_Terms" Key="SoldTo_Terms"  Width="60px" >
                        <Header Text="Qty:"/>
                    </ig:BoundDataField>

                    <ig:BoundDataField DataFieldName="DeliverTo_CategoryID" Key="DeliverTo_CategoryID"  Width="70px" > 
                        <Header Text="DeliverTo_CategoryID:"/>
                    </ig:BoundDataField>
                    <ig:BoundDataField DataFieldName="DeliverTo_Category" Key="DeliverTo_Category"  Width="70px" > 
                        <Header Text="DeliverTo_Category:"/>
                    </ig:BoundDataField>
                    <ig:BoundDataField DataFieldName="DeliverTo_Id" Key="DeliverTo_Id"  Width="70px" >
                        <Header Text="DeliverTo_Id:"/>
                    </ig:BoundDataField>
                    <ig:BoundDataField DataFieldName="DeliverTo_Name" Key="DeliverTo_Name"  Width="60px" >
                        <Header Text="DeliverTo_Name:"/>
                    </ig:BoundDataField>
                         <ig:BoundDataField DataFieldName="DeliverTo_Code" Key="DeliverTo_Code"  Width="60px" >
                        <Header Text="DeliverTo_Code:"/>
                    </ig:BoundDataField>
 
                    <ig:BoundDataField DataFieldName="DeliverTo_ContactId" Key="DeliverTo_ContactId"  Width="60px" >
                        <Header Text="DeliverTo_ContactId:"/>
                    </ig:BoundDataField>
                    <ig:BoundDataField DataFieldName="DeliverTo_ContactName" Key="DeliverTo_ContactName"  Width="60px" >
                        <Header Text="DeliverTo_ContactName:"/>
                    </ig:BoundDataField>
                    <ig:BoundDataField DataFieldName="DeliverTo_Phone" Key="DeliverTo_Phone"  Width="60px" >
                        <Header Text="DeliverTo_Phone:"/>
                    </ig:BoundDataField>
                    <ig:BoundDataField DataFieldName="DeliverTo_Route" Key="DeliverTo_Route"  Width="60px" >
                        <Header Text="DeliverTo_Route:"/>
                    </ig:BoundDataField>
                    <ig:BoundDataField DataFieldName="DeliverTo_Address" Key="DeliverTo_Address"  Width="60px" >
                        <Header Text="DeliverTo_Address:"/>
                    </ig:BoundDataField>
                    <ig:BoundDataField DataFieldName="DeliverTo_OrderDate" Key="DeliverTo_OrderDate"  Width="60px" >
                        <Header Text="DeliverTo_OrderDate:"/>
                    </ig:BoundDataField>
                    <ig:BoundDataField DataFieldName="DeliverTo_RequestedDate" Key="DeliverTo_RequestedDate"  Width="60px" >
                        <Header Text="DeliverTo_RequestedDate:"/>
                    </ig:BoundDataField>
                    
                     <ig:BoundDataField DataFieldName="DeliverTo_DateArrival" Key="DeliverTo_DateArrival"  Width="60px" >
                        <Header Text="DeliverTo_DateArrival:"/>
                    </ig:BoundDataField>

     
                    <ig:BoundDataField DataFieldName="BillTo_Id" Key="BillTo_Id"  Width="70px" >
                        <Header Text="BillTo_Id:"/>
                    </ig:BoundDataField>
                    <ig:BoundDataField DataFieldName="BillTo_Name" Key="BillTo_Name"  Width="60px" >
                        <Header Text="BillTo_Name:"/>
                    </ig:BoundDataField>
                     <ig:BoundDataField DataFieldName="BillTo_Code" Key="BillTo_Code"  Width="60px" >
                        <Header Text="BillTo_Code:"/>
                    </ig:BoundDataField>
                     <ig:BoundDataField DataFieldName="BillTo_Address" Key="BillTo_Address"  Width="60px" >
                        <Header Text="BillTo_Address:"/>
                    </ig:BoundDataField>
       
                    
                     <ig:BoundDataField DataFieldName="BillTo_ContactId" Key="BillTo_ContactId"  Width="60px" >
                        <Header Text="BillTo_ContactId:"/>
                    </ig:BoundDataField>
                    <ig:BoundDataField DataFieldName="BillTo_ContactName" Key="BillTo_ContactName"  Width="60px" >
                        <Header Text="BillTo_ContactName:"/>
                    </ig:BoundDataField>
                    <ig:BoundDataField DataFieldName="BillTo_Phone" Key="BillTo_Phone"  Width="60px" >
                        <Header Text="BillTo_Phone:"/>
                    </ig:BoundDataField>
                    

                    <ig:BoundDataField DataFieldName="Other_OrderType" Key="Other_OrderType"  Width="60px" >
                        <Header Text="Other_OrderType:"/>
                    </ig:BoundDataField>
                    <ig:BoundDataField DataFieldName="Other_CustPO" Key="Other_CustPO"  Width="60px" >
                        <Header Text="Other_CustPO:"/>
                    </ig:BoundDataField>
                    <ig:BoundDataField DataFieldName="Other_Priority" Key="Other_Priority"  Width="60px" >
                        <Header Text="Other_Priority:"/>
                    </ig:BoundDataField> 
                    <ig:BoundDataField DataFieldName="Other_Site" Key="Other_Site"  Width="60px" >
                        <Header Text="Other_Priority:"/>
                    </ig:BoundDataField> 
                     <ig:BoundDataField DataFieldName="Other_QuoteRef" Key="Other_QuoteRef"  Width="60px" >
                        <Header Text="Other_Priority:"/>
                    </ig:BoundDataField>        
                    <ig:BoundDataField DataFieldName="Other_Comment" Key="Other_Comment"  Width="60px" >
                        <Header Text="Other_Comment:"/>
                    </ig:BoundDataField>   
                     <ig:BoundDataField DataFieldName="Other_Interstat" Key="Other_Interstat"  Width="60px" >
                        <Header Text="Other_Interstat:"/>
                    </ig:BoundDataField>   
                    <ig:BoundDataField DataFieldName="Other_DeliveryTerms" Key="Other_DeliveryTerms"  Width="60px" >
                        <Header Text="Other_DeliveryTerms:"/>
                    </ig:BoundDataField> 
                         <ig:BoundDataField DataFieldName="Other_SalesPersonCode" Key="Other_SalesPersonCode"  Width="60px" >
                        <Header Text="Other_SalesPersonCode:"/>
                    </ig:BoundDataField>        



 </Columns>
 <Behaviors>
 
 </Behaviors>
                </ig:WebDataGrid>                

           <br />
           <asp:Panel ID ="pnlSearchCust" runat ="server" BackColor="White"  Width="296px"  BorderColor="#666666" BorderStyle="Solid" BorderWidth="1px" style="display:none" Height="281px" >
               <asp:Label ID="Label13" runat="server" Text="Search delivery customer" Font-Bold="True" Font-Names="Arial"></asp:Label>
               <br />
               <br />
               <asp:Label ID="Label14" runat="server" Text="Enter customer name or part of name:"></asp:Label>
               <br />
               <asp:TextBox ID="txtSearchString" runat="server" Height="21px" Width="213px"></asp:TextBox>
               <asp:Button ID="cmdSearch" runat="server" CssClass="VT_ActionButton" Font-Size="9pt" Height="20px" Text="Search" />
               <br />
               Select from the list below and click Select<br />
               <ig:WebDataGrid ID="wdgCust" runat="server" AjaxIndicator-Enabled="True" AltItemCssClass="VerifyGrid_Report_AlternateRow" AutoGenerateColumns="False" BackColor="White" CssClass="VerifyGrid_Report_Frame" EnableClientRendering="True" EnableTheming="True" Font-Names="Arial" FooterCaptionCssClass="VerifyGrid_Report_Footer" HeaderCaptionCssClass="VerifyGrid_Report_HeaderGreen_10pt" Height="151px" ItemCssClass="VerifyGrid_Report_Row">
                   <ClientEvents Initialize="wdgCust_Initialize" />
                   <AjaxIndicator BlockArea="Control" RelativeToControl="True" />
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
                           <ActivationClientEvents ActiveCellChanged="wdgCust_Activation_ActiveCellChanging" />
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


           </asp:Panel>
                       <ajaxToolkit:ModalPopupExtender ID="ModalPopupSearchCust" runat="server" 
                    BackgroundCssClass="modalBackground" CancelControlID="cmdCancel" 
                    DropShadow="True" oncancelscript="HideSearchCustPopup()" PopupControlID="pnlSearchCust" 
                    TargetControlID="pnlSearchCust">
                </ajaxToolkit:ModalPopupExtender>

           </div>

</asp:Content>

