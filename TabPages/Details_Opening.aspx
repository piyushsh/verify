<%@ Page   Language="VB" MasterPageFile= "~/TabPages/ProjectMain.master" AutoEventWireup="false" CodeFile="Details_Opening.aspx.vb" Inherits="TabPages_Details_Opening"  EnableEventValidation="false"%>


<%@ Register Assembly="Infragistics4.Web.v13.2, Version=13.2.20132.2028, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" namespace="Infragistics.Web.UI.DisplayControls" tagprefix="ig" %>
<%@ Register Assembly="Infragistics4.Web.v13.2, Version=13.2.20132.2028, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" namespace="Infragistics.Web.UI.EditorControls" tagprefix="ig" %>
<%@ Register Assembly="Infragistics4.Web.v13.2, Version=13.2.20132.2028, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" namespace="Infragistics.Web.UI.GridControls" tagprefix="ig" %>
<%@ Register Assembly="Infragistics4.Web.v13.2, Version=13.2.20132.2028, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" namespace="Infragistics.Web.UI.ListControls" tagprefix="ig" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ProjectMAIN_content" Runat="Server">
         
      <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        
            <ContentTemplate>
    
    
    <script type="text/javascript">
        function HideMsgPopup() 
        { 
            var modal = $find("<%=ModalPopupExtender1.BehaviorID%>");
            modal.hide(); 
        }
            function HideMsgPopupEdit() 
        { 
            var modal = $find("<%=ModalPopupExtender2.BehaviorID%>");
            modal.hide(); 
        }
        function HideMsgPopup3() {
            var modal = $find("<%=ModalPopupExtender3.BehaviorID%>");
            modal.hide();
        }
        
           function ShowResendClose(sender, e) 
        { 
            __doPostBack(sender,e);
        }  
        
               function trim(stringToTrim) {
	                return stringToTrim.replace(/^\s+|\s+$/g,"");
                }
                function ltrim(stringToTrim) {
	                return stringToTrim.replace(/^\s+/,"");
                }
                function rtrim(stringToTrim) {
	                return stringToTrim.replace(/\s+$/,"");
                }

           function EditItemClose(sender, e) 
        { 
            var UnitOfSale =document.getElementById("<%=hdnUnitOfSale.ClientID%>");   
            var TooLow 
            var OrderType = document.getElementById("<%=lblOrderType.ClientID%>"); 
            
            if (rtrim(OrderType.innerText) != "Flexi") {
            
            if(UnitOfSale.value == 1 ) {
             var txtQty=document.getElementById("<%=txtPnlNewOrderQty.ClientID%>"); 
             var OrderedQty=document.getElementById("<%=lblPnlOrderedQtyEdit.ClientID%>"); 
             var OutstandingQty=document.getElementById("<%=lblPnlOutstandingQtyEdit.ClientID%>"); 
             
            if (parseFloat(txtQty.value) < (parseFloat(OrderedQty.innerText) - parseFloat(OutstandingQty.innerText))) {
                TooLow = "True"
            }else {
                TooLow = "False"
            }
                          
           } else {
            var txtWeight=document.getElementById("<%=txtPnlNewOrderWgt.ClientID%>"); 
             var OrderedWgt=document.getElementById("<%=lblPnlOrderedWgtEdit.ClientID%>"); 
             var OutstandingWgt=document.getElementById("<%=lblPnlOutstandingWgtEdit.ClientID%>"); 
             
            if (parseFloat(txtWeight.value) < (parseFloat(OrderedWgt.innerText) - parseFloat(OutstandingWgt.innerText))) {
                TooLow = "True"
            }else {
                TooLow = "False"
            }
           }
            } else {
                TooLow = "False"
            }
             if (TooLow == "True"){
             
                    alert('You cannot set the ordered amount to Less that has been delivered.');
                        
             }else {
            __doPostBack(sender,e);
            }
        }  
        
        

         function LoadResendPanel(PickType) 
        {
           var PCOnly =document.getElementById("<%=hdnPCOnly.ClientID%>");   
         
            if (PickType == "PCOnly"){
                PCOnly.value = "PCOnly"
            }else{
                PCOnly.value = "HH"
            }
            
            var modal = $find("<%=ModalPopupExtender1.BehaviorID%>");
            modal.show(); 
            return false;
        }

     function LoadEditPanel() 
        {
            var modal = $find("<%=ModalPopupExtender2.BehaviorID%>");
            modal.show(); 
            return false;
        }
        
         

 function CalcQtyFromWeight()

            {
            var DoCalc =document.getElementById("<%=hidCalcWgtFromQty.ClientID%>"); 

            var numWeightPerUnit=document.getElementById("<%=lblPerUnitWeight.ClientID%>"); 

            var txtQty=document.getElementById("<%=txtPnlQtyToPick.ClientID%>"); 

            var txtWeight=document.getElementById("<%=txtPnlWgtToPick.ClientID%>"); 
             if ((DoCalc.value) == "YES" ) {
                      if (parseFloat(txtWeight.value) > 0 ) {
                txtQty.innerText = String(parseInt(parseFloat(txtWeight.value) / parseFloat(numWeightPerUnit.value)));
                         }
                }
            }        

        function CalcWeightFromQty()

            {

                var calculatedwgt
                var UnitOfSale = document.getElementById("<%=hdnUnitOfSale.ClientID%>");
                if (UnitOfSale.value != 1) {
                    var DoCalc = document.getElementById("<%=hidCalcWgtFromQty.ClientID%>");

                    var numWeightPerUnit = document.getElementById("<%=lblPerUnitWeight.ClientID%>");

                    var txtQty = document.getElementById("<%=txtPnlQtyToPick.ClientID%>");

                    var txtWeight = document.getElementById("<%=txtPnlWgtToPick.ClientID%>");
                    if ((DoCalc.value) == "YES") {
                        if (parseFloat(txtQty.value) > 0) {
                            calculatedwgt = parseFloat(txtQty.value) * parseFloat(numWeightPerUnit.value);
                            calculatedwgt = roundNumber(calculatedwgt, 4);
                            txtWeight.innerText = String(calculatedwgt)
                    }
                }
            }
            }        

function CalcQtyFromWeightEdit()

            {
             
            var txtWeight=document.getElementById("<%=txtPnlNewOrderWgt.ClientID%>"); 
                
            var DoCalc =document.getElementById("<%=hidCalcWgtFromQty.ClientID%>"); 

            var numWeightPerUnit=document.getElementById("<%=lblPerUnitWeight.ClientID%>"); 

            var txtQty=document.getElementById("<%=txtPnlNewOrderQty.ClientID%>"); 
            
         
              if ((DoCalc.value) == "YES" ) {
                      if (parseFloat(txtWeight.value) > 0 ) {
                txtQty.innerText = String(parseInt(parseFloat(txtWeight.value) / parseFloat(numWeightPerUnit.value)));
                         }
                }
            }        

        function CalcWeightFromQtyEdit()

            {
                 var UnitOfSale =document.getElementById("<%=hdnUnitOfSale.ClientID%>");   
             if(UnitOfSale.value != 1 ) {
                var DoCalc =document.getElementById("<%=hidCalcWgtFromQty.ClientID%>"); 

                var numWeightPerUnit=document.getElementById("<%=lblPerUnitWeight.ClientID%>"); 

               var txtQty=document.getElementById("<%=txtPnlNewOrderQty.ClientID%>"); 

                var txtWeight=document.getElementById("<%=txtPnlNewOrderWgt.ClientID%>"); 
                if ((DoCalc.value) == "YES" ) {
                    if (parseFloat(txtQty.value) > 0 ) {
                        txtWeight.innerText = String(parseFloat(txtQty.value)* parseFloat(numWeightPerUnit.value));
                    }
                }
            }
            }    
          

 
                
    </script>
    
   
  
     
    <div style="margin-left:10px; margin-top:10px; ">
    
    
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
                            <asp:Label ID="lblOrderStatus" runat="server" Font-Bold="False" 
                                Font-Names="Arial" Font-Size="10pt" Width="249px" 
                    ForeColor="#990000"></asp:Label></td>
        </tr>
        
        <tr style="font-size: 10pt">
            <td align="left" valign="top" style="border: 1px none #808080;">
            
                <table align="left" cellpadding="0" cellspacing="0" 
                    
                    style="padding: 2px; width: 930px; float: left; vertical-align: top; text-align: left;">
                    <tr>
                        <td style="border-top-style: solid;  border-left-style: solid; border-top-width: 1px;  border-left-width: 1px; border-top-color: #BEA027;  border-left-color: #BEA027">
             
                            <asp:Label ID="Label8" runat="server" Font-Bold="True" Font-Size="9pt" 
                                Text="Customer PO Number:" Font-Names="Arial" Width="132px"></asp:Label>
                        </td>
                        <td style="border-top-style: solid;  border-left-style: solid; border-top-width: 1px;  border-left-width: 1px; border-top-color: #BEA027;  border-left-color: #BEA027">
             
                            <asp:Label ID="Label11" runat="server" Font-Bold="True" Font-Size="9pt" 
                                Text="Requested Delivery Date:" Width="152px"></asp:Label>
                        </td>
                        <td style="border-top-style: solid;  border-left-style: solid; border-top-width: 1px; border-left-width: 1px; border-top-color: #BEA027; border-left-color: #BEA027">
             
                            <asp:Label ID="Label15" runat="server" Font-Bold="True" Font-Size="9pt" 
                                Text="Delivery Customer:" Width="158px"></asp:Label>
                        </td>
                        <td style="border-left: 1px solid #BEA027; border-top: 1px solid #BEA027; width: 153px;">
             
                            <asp:Label ID="Label16" runat="server" Font-Bold="True" Font-Size="9pt" 
                                Text="Delivery Address:" Width="150px"></asp:Label>
                        </td>
                        <td style="border-top-style: solid;  border-left-style: solid; border-top-width: 1px;  border-left-width: 1px; border-top-color: #BEA027;  border-left-color: #BEA027">
             
                            <asp:Label ID="lblOrderTypeLabel" runat="server" Font-Bold="True" Font-Size="9pt" 
                                Text="Order Type:" style="color: #000066" Width="123px"></asp:Label>
                        </td>
                        <td style="border-top-style:solid;  border-left-style: solid; border-top-width: 1px;  border-left-width: 1px; border-top-color: #BEA027;  border-left-color: #BEA027">
             
                            <asp:Label ID="Label10" runat="server" Font-Bold="True" Font-Size="9pt" 
                                Text="Order Date:" Width="89px"></asp:Label>
                        </td>
                        <td style="border-top-style: solid; border-right-style: solid; border-left-style: solid; border-top-width: 1px; border-right-width: 1px; border-left-width: 1px; border-top-color: #BEA027; border-right-color: #BEA027; border-left-color: #BEA027">
             
                            <asp:Label ID="Label28" runat="server" Font-Bold="True" Font-Size="9pt" 
                                Text="Priority:" Width="70px"></asp:Label>
                        </td>
                    </tr>
                    <tr style="vertical-align: top; text-align: left">
                        <td style="border-bottom-style: solid;  border-left-style: solid; border-bottom-width: 1px;  border-left-width: 1px; border-bottom-color: #BEA027;  border-left-color: #BEA027">
             
                            <asp:Label ID="lblPONumber" runat="server" Font-Bold="False" Font-Names="Arial"
                                Font-Size="10pt" Width="122px"></asp:Label>
                            
                    <asp:TextBox ID="txtPO" runat="server" Font-Size="9pt"></asp:TextBox>
                            
                        </td>
                        <td style="border-bottom-style: solid;  border-left-style: solid; border-bottom-width: 1px;  border-left-width: 1px; border-bottom-color: #BEA027;  border-left-color: #BEA027">
             
                             <asp:Label ID="lblRequestedDeliveryDate" runat="server" Font-Bold="False" Font-Names="Arial"
                                Font-Size="9pt" Width="146px"></asp:Label>
                            
                                                      
                            <ig:WebDatePicker ID="dteDeliveryDate" runat="server" DisplayModeFormat="d" 
                                       StyleSetName="Default">
                            </ig:WebDatePicker>

                        </td>
                        <td style="border-bottom-style: solid;  border-left-style: solid; border-bottom-width: 1px;  border-left-width: 1px; border-bottom-color: #BEA027;  border-left-color: #BEA027">
             
                            <asp:Label ID="lblDeliveryCustomer" runat="server" Font-Bold="False" Font-Names="Arial"
                                Font-Size="9pt" Width="164px"></asp:Label>
                                <asp:DropDownList ID="ddlDeliveryCustomer" runat="server" Height="25px" 
                                                Width="240px" AutoPostBack="True">
                                                <asp:ListItem></asp:ListItem>
                                            </asp:DropDownList>
                        </td>
                        <td style="border-left: 1px solid #BEA027; border-bottom: 1px solid #BEA027; width: 153px;">
             
                            <asp:Label ID="lblCustAddress" runat="server" Font-Bold="False" Font-Names="Arial"
                                Font-Size="9pt" Width="144px"></asp:Label></td>
                        <td style="border-bottom-style: solid;  border-left-style: solid; border-bottom-width: 1px;  border-left-width: 1px; border-bottom-color: #BEA027; border-left-color: #BEA027">
             
                            <asp:Label ID="lblOrderType" runat="server" Font-Bold="False" Font-Names="Arial"
                                Font-Size="9pt" Width="120px" style="color: #333399"></asp:Label>
                        </td>
                        <td style="border-bottom-style: solid;  border-left-style: solid; border-bottom-width: 1px;  border-left-width: 1px; border-bottom-color: #BEA027;  border-left-color: #BEA027">
             
                            <asp:Label ID="lblOrderDate" runat="server" Font-Bold="False" 
                                Font-Names="Arial" Font-Size="9pt" Width="88px"></asp:Label>
                        </td>
                        <td style="border-bottom-style: solid; border-right-style: solid; border-left-style: solid; border-bottom-width: 1px; border-right-width: 1px; border-left-width: 1px; border-bottom-color: #BEA027; border-right-color: #BEA027; border-left-color: #BEA027">
             
                    <asp:TextBox ID="txtPriority" runat="server" Width="60px" Font-Size="9pt"></asp:TextBox>
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
                    <asp:TextBox ID="txtComment" runat="server" Height="41px" Width="527px"></asp:TextBox>
                            
                        </td>
                        <td>
                        <asp:ImageButton ID="btnSave" runat="server" 
                            ImageUrl="~/App_Themes/Billing/Buttons/Save.gif" ToolTip="Save Changes made to order details." />
                        </td>
                    </tr>
                </table>
            
            
            </td>
        </tr>
    </table>
    
    
        <table style="border-style: none; width: 970px;  border-top-width: 0px;">
            <tr>
                <td style="border-style: none; border-top-width: 1px; border-right-width: 1px; border-left-width: 1px; border-top-color: #808080; border-right-color: #808080; border-left-color: #808080; font-family: Arial; font-size: 2px; background-color: #E6E6CC;">
                        &nbsp;</td>
            </tr>
            <tr>
                <td style="">
                        <table style="width: 800px">
                            <tr>
                                <td>
                                    <asp:Button ID="btnResendAll" runat="server" CssClass="VT_FormMasterButton_Micro" Text="Send for picking" Width="110px" />
                                            </td>
                                <td>
                                    &nbsp;</td>
                                <td>
                                                <asp:Button ID="btnAddToOrder" runat="server" CssClass="VT_FormMasterButton_Micro" Text="Add to this order" Width="110px" />
                                            </td>
                                <td>
                                    &nbsp;</td>
                                <td>
                                    <asp:Button ID="btnCloseOrder" runat="server" CssClass="VT_FormMasterButton_Micro" Text="Close Order" Width="110px" />
                                            </td>
                                <td>
                                    &nbsp;</td>
                                <td>
                                    <asp:Button ID="btnPrint" runat="server" CssClass="VT_FormMasterButton_Micro" Text="Print Sales Order" Width="110px" />
                                            </td>
                                <td>
                                    &nbsp;</td>
                                <td>
                                    <asp:Button ID="btnAttachments" runat="server" CssClass="VT_FormMasterButton_Micro" Text="View Attachments" Width="110px" />
                                </td>
                                <td>
                                       <asp:Button ID="btnAuditLog" runat="server" CssClass="VT_FormMasterButton_Micro" Text="View Audit log" Width="110px" />
</td>
                                <td> <table cellpadding="0" cellspacing="0" class="auto-style1">
                                                    <tr>
                                                        <td>
                                                            <asp:Button ID="btnCommentVIEW" runat="server" CssClass="VT_FormMasterButton_Micro" Text="View comments" Width="110px" />
                                                            </td>
                                                        <td>
                                                            <asp:Button ID="btnShipping" runat="server" CssClass="VT_FormMasterButton_Micro" Text="Shipping details" Width="110px" />
                                                        </td>
                                                     
                                                          <td>
                                                              <asp:Button ID="btnOrderDelivered" runat="server" CssClass="VT_FormMasterButton_Micro" Text="Order Delivered" Width="114px" Height="27px" /></td>
                                                       
                                                    </tr>
                                                </table>
                                            </td>
                            </tr>
                        </table>
                    </td>
            </tr>
            
            <tr>
                <td style="">
                        <asp:Label ID="lblGridTitle" runat="server" Font-Bold="True" 
                            style="font-size: 14pt; font-family: Arial">Items on this Sales Order</asp:Label>
                        <asp:Label ID="lblGridTitle0" runat="server" Font-Bold="False" 
                            style="font-size: 9pt; font-family: Arial">(click on an item to display more details...)</asp:Label>
                    </td>
            </tr>
            <tr>
                <td style="">
                        
                   <ig:WebDataGrid ID="wdgOrderItems" runat="server" AltItemCssClass="VerifyGrid_Report_AlternateRow" AutoGenerateColumns="False" BackColor="White" CssClass="VerifyGrid_Report_Frame" EnableAjax="False" EnableTheming="True" Font-Names="Arial" FooterCaptionCssClass="VerifyGrid_Report_Footer" HeaderCaptionCssClass="VerifyGrid_Report_HeaderGreen_10pt" Height="300px" ItemCssClass="VerifyGrid_Report_Row">
                    <Columns>
                        
                        <ig:TemplateDataField Key="View" Width="40px" CssClass="Center" Hidden="True">
                            <ItemTemplate>
                                <asp:ImageButton ID="btnRowView" runat="server" 
                                    ImageUrl="~/App_Themes/Billing/Buttons/ViewEdit.gif" onclick="btnRowView_Click"  />
                            </ItemTemplate>
                            <Header Text="View:" />
                        </ig:TemplateDataField>

                        <ig:BoundDataField DataFieldName="Item" Key="Item" Width="40px">
                            <header Text="Item:"/>
                        </ig:BoundDataField>


                        
                                <ig:BoundDataField DataFieldName="ProductName" Key="Product" Width="200px">
                                    <Header Text="Product Name:"/>
                                </ig:BoundDataField>
                                

                        <ig:GroupField Key="GroupField_OrderDetails">
                            <Columns>
                                <ig:BoundDataField DataFieldName="QuantityRequested" Key="QtyOrdered" Width="54px" CssClass="RAlign">
                                    <Header Text="Qty:"/>
                                </ig:BoundDataField>
                                <ig:BoundDataField DataFieldName="WeightRequested" Key="WgtOrdered" Width="54px" CssClass="RAlign">
                                    <Header Text="Wgt:"/>
                                </ig:BoundDataField>
                            </Columns>
                          <Header Text="Order Details">
                            </Header>
                        </ig:GroupField>
                        
                        <ig:GroupField Key="GroupField_DeliveryDetails">
                            <Columns>
						        <ig:BoundDataField DataFieldName="Quantity" Key="QtyDelivered" Width="54px" CssClass="RAlign">
                                    <Header Text="Qty:"/>
                                </ig:BoundDataField>
                                <ig:BoundDataField DataFieldName="Weight" Key="WgtDelivered" Width="54px" CssClass="RAlign">
                                    <Header Text="Wgt:"/>
                                </ig:BoundDataField>
                             </Columns>
                          <Header Text="Delivery Details">
                            </Header>
                        </ig:GroupField>

                        <ig:GroupField Key="GroupField_PriceDetails">
                            <Columns>
                                <ig:BoundDataField DataFieldName="UnitPrice" Key="UnitPrice" DataFormatString="{0:###,###,##0.00}" Width="54px" CssClass="RAlign">
                                    <Header Text="Unit:"/>
                                </ig:BoundDataField>
                                <ig:BoundDataField DataFieldName="VAT" Key="VAT" DataFormatString="{0:###,###,##0.00}" Width="60px" CssClass="RAlign">
                                    <Header Text="Tax:" />
                                </ig:BoundDataField>
                                <ig:BoundDataField DataFieldName="TotalPrice" Key="Total" DataFormatString="{0:###,###,##0.00}" Width="70px" CssClass="RAlign">
                                    <Header Text="Total:"/>
                                </ig:BoundDataField>
                             </Columns>
                                <Header Text="Price Details">
                            </Header>
                        </ig:GroupField>

                        <ig:BoundDataField DataFieldName="Status" Key="Status" Width="130px" CssClass="RAlign">
                            <Header Text="Status:"/>
                        </ig:BoundDataField>
                        <ig:BoundDataField DataFieldName="SalesOrderItemID" Hidden="True" Key="SalesOrderItemID">
                            <Header Text="SalesOrderItemID"/>
                        </ig:BoundDataField>
						<ig:BoundDataField DataFieldName="ProductID" Hidden="True" Key="ProductID" 
                            DataType="System.Int32">
                            <Header Text="ProductID"/>
                        </ig:BoundDataField>
                        <ig:BoundDataField DataFieldName="Comment" Key="Comment" Width ="300" >

                                                        <Header Text="Comment:"/>
                        </ig:BoundDataField>
					</Columns>
                    <Behaviors>
                        <ig:Activation ActiveRowCssClass="SelectedRow">
                            <AutoPostBackFlags ActiveCellChanged="True" />
                        </ig:Activation>
                        <ig:ColumnResizing>
                        </ig:ColumnResizing>
                        <ig:SummaryRow EmptyFooterText="" SummariesCssClass="SummaryRow" FormatString="{1}">
                            <ColumnSummaries>
                                <ig:ColumnSummaryInfo ColumnKey="Product">
                                    <Summaries>
                                        <ig:Summary SummaryType="Count" />
                                    </Summaries>
                                </ig:ColumnSummaryInfo>
                                 <ig:ColumnSummaryInfo ColumnKey="VAT">
                                    <Summaries>
                                        <ig:Summary SummaryType="Sum" />
                                    </Summaries>
                                </ig:ColumnSummaryInfo>
                                  <ig:ColumnSummaryInfo ColumnKey="Total">
                                    <Summaries>
                                        <ig:Summary SummaryType="Sum" />
                                    </Summaries>
                                </ig:ColumnSummaryInfo>
                            </ColumnSummaries>
                            <SummaryButton HoverImageUrl="~/App_Themes/Buttons/Add_Plus_Small.gif" />
                            <ColumnSettings>
                                
                                <ig:SummaryRowSetting ColumnKey="Product" 
                                     EnableColumnSummaryOptions="True" EnableFormatting="True" FormatString="Count = {1}" 
                                    ShowSummaryButton="False">
                                </ig:SummaryRowSetting>
                                <ig:SummaryRowSetting ColumnKey="VAT" 
                                     EnableColumnSummaryOptions="True" EnableFormatting="True" FormatString=" {1}" 
                                    ShowSummaryButton="False">
                                </ig:SummaryRowSetting>
                                 <ig:SummaryRowSetting ColumnKey="Total" 
                                     EnableColumnSummaryOptions="True" EnableFormatting="True" FormatString=" {1}" 
                                    ShowSummaryButton="False">
                                </ig:SummaryRowSetting>

                                
                               <ig:SummaryRowSetting ColumnKey="Item" 
                                    EnableColumnSummaryOptions="False"  ShowSummaryButton="False">
                                </ig:SummaryRowSetting>
                                <ig:SummaryRowSetting ColumnKey="ProductName" 
                                    EnableColumnSummaryOptions="False"  ShowSummaryButton="False">
                                </ig:SummaryRowSetting>
                                <ig:SummaryRowSetting ColumnKey="QtyOrdered" 
                                    EnableColumnSummaryOptions="False"  ShowSummaryButton="False">
                                </ig:SummaryRowSetting>
                                <ig:SummaryRowSetting ColumnKey="WgtOrdered" 
                                    EnableColumnSummaryOptions="False"  ShowSummaryButton="False">
                                </ig:SummaryRowSetting>
                                <ig:SummaryRowSetting ColumnKey="QtyDelivered" 
                                    EnableColumnSummaryOptions="False"  ShowSummaryButton="False">
                                </ig:SummaryRowSetting>
                                <ig:SummaryRowSetting ColumnKey="WgtDelivered" 
                                    EnableColumnSummaryOptions="False"  ShowSummaryButton="False">
                                </ig:SummaryRowSetting>
                                <ig:SummaryRowSetting ColumnKey="UnitPrice" 
                                    EnableColumnSummaryOptions="False"  ShowSummaryButton="False">
                                </ig:SummaryRowSetting>
                                <ig:SummaryRowSetting ColumnKey="Status" 
                                    EnableColumnSummaryOptions="False"  ShowSummaryButton="False">
                                </ig:SummaryRowSetting>
                                <ig:SummaryRowSetting ColumnKey="SalesOrderItemID" 
                                    EnableColumnSummaryOptions="False"  ShowSummaryButton="False">
                                </ig:SummaryRowSetting>
                                <ig:SummaryRowSetting ColumnKey="ProductID" 
                                    EnableColumnSummaryOptions="False"  ShowSummaryButton="False">
                                </ig:SummaryRowSetting>
                                  <ig:SummaryRowSetting ColumnKey="Comment" 
                                    EnableColumnSummaryOptions="False"  ShowSummaryButton="False">
                                </ig:SummaryRowSetting>
                                
                            </ColumnSettings>
                        </ig:SummaryRow>
                        <ig:Filtering Enabled="true" FilterButtonCssClass="Filter_LAlign">
                            <ColumnSettings>
                                <ig:ColumnFilteringSetting ColumnKey="Product" Enabled="true" />
                            </ColumnSettings>
                            <ColumnFilters>
                                <ig:ColumnFilter ColumnKey="Product">
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
                       
                </ig:WebDataGrid>

                    

                </td>
            </tr>
            <tr>
                <td style="">
                        
                                  <table style="width: 1000px; background-color: #D8E9D9;">
                                <tr>
                                
                                    <td style="font-family: Arial; font-size: 4px" class="auto-style2">
                                        &nbsp;</td>
                                </tr>
                                <tr>
                                    
                                    <td class="auto-style2">
                                        <asp:Label ID="Label7" runat="server" Font-Bold="True" 
                                            style="font-size: 12pt; font-family: Arial" 
                                            Text="Delivery Details for the sales item selected above:" Font-Names="Arial" 
                                            Font-Size="10pt" Font-Italic="True"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                   
                                    <td class="auto-style2">
                                        <table align="left" cellpadding="0" cellspacing="0" 
                                            style="width: 920px; float: left">
                                            <tr>
                                                <td style="width: 710px">
                                                   
                                                    <ig:WebDataGrid ID="wdgDeliverys" runat="server" AltItemCssClass="VerifyGrid_Report_AlternateRow" AutoGenerateColumns="False" BackColor="White" CssClass="VerifyGrid_Report_Frame" EnableAjax="False" EnableTheming="True" Font-Names="Arial" FooterCaptionCssClass="VerifyGrid_Report_Footer" HeaderCaptionCssClass="VerifyGrid_Report_HeaderGreen_10pt" Height="140px" ItemCssClass="VerifyGrid_Report_Row">
                                                                <Columns>
                                                                    <ig:BoundDataField DataFieldName="Item" Key="Item" Width="60px">
                                                                        <header Text="Number:"/>
                                                                    </ig:BoundDataField>
                                                                    <ig:BoundDataField DataFieldName="DateOfTransaction" DataFormatString="{0:d}" Key="Date" Width="90px">
                                                                        <Header Text="Date:"/>
                                                                    </ig:BoundDataField>
                                                                    <ig:BoundDataField DataFieldName="Quantity" Key="Qty" Width="80px">
                                                                        <Header Text="Dispatched Quantity:"/>
                                                                    </ig:BoundDataField>
                                                                    <ig:BoundDataField DataFieldName="Weight" Key="Weight" Width="80px">
                                                                        <Header Text="Dispatched Weight:"/>
                                                                    </ig:BoundDataField>
                                                                    <ig:BoundDataField DataFieldName="Driver" Key="Driver" Width="110px">
                                                                        <Header Text="Driver:"/>
                                                                    </ig:BoundDataField>
                                                                    <ig:BoundDataField DataFieldName="DocketNum" Key="DocketNum" Width="90px">
                                                                        <Header Text="Docket Number:"/>
                                                                    </ig:BoundDataField>
                                                                    <ig:BoundDataField DataFieldName="InvoiceNum" Key="InvoiceNum" Width="80px">
                                                                        <Header Text="Invoice Number:"/>
                                                                    </ig:BoundDataField>
                                                                    <ig:BoundDataField DataFieldName="SyncID" Hidden="True" Key="SyncID" Width="70px">
                                                                        <Header Text="SyncID:"/>
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
                                <ig:ColumnSummaryInfo ColumnKey="DocketNum">
                                    <Summaries>
                                        <ig:Summary SummaryType="Count" />
                                    </Summaries>
                                </ig:ColumnSummaryInfo>
                                <ig:ColumnSummaryInfo ColumnKey="DocketNum">
                                    <Summaries>
                                        <ig:Summary />
                                    </Summaries>
                                </ig:ColumnSummaryInfo>
                            </ColumnSummaries>
                            <SummaryButton HoverImageUrl="~/App_Themes/Buttons/Add_Plus_Small.gif" />
                            <ColumnSettings>
                                <ig:SummaryRowSetting ColumnKey="DocketNum">
                                    <SummarySettings>
                                        <ig:SummarySetting SummaryType="Count" />
                                        <ig:SummarySetting />
                                    </SummarySettings>
                                </ig:SummaryRowSetting>
                            </ColumnSettings>
                        </ig:SummaryRow>
                        <ig:Filtering Enabled="true" FilterButtonCssClass="Filter_LAlign">
                            <ColumnSettings>
                                <ig:ColumnFilteringSetting ColumnKey="DocketNum" Enabled="true" />
                            </ColumnSettings>
                            <ColumnFilters>
                                <ig:ColumnFilter ColumnKey="DocketNum">
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
                </ig:WebDataGrid>

                                                     <br />
                                                     <br />
                                                
                                                
                                                
                                                </td>
                                                <td style="text-align: left; vertical-align: top">
                                                    <table align="left" cellpadding="0" cellspacing="0" 
                                                        
                                                        style="width: 190px; float: left; vertical-align: text-top; text-align: center;">
                                                        <tr>
                                                            <td>
                                                                <strong><span style="font-size: 14pt; color: #b28700; font-family: Arial">
                                                                <asp:ImageButton ID="btnViewProductDetails" runat="server" 
                                                                    
                                                                    ImageUrl="~/App_Themes/Billing/Buttons/View-Product-Stock-Position-Blue.gif" ToolTip="View the current stock position for the selected order item." />
                                                                </span></strong>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:ImageButton ID="btnResendOnePopupPC" runat="server" 
                                                                    AlternateText="Send Selected Item for Picking on PC" 
                                                                    ImageUrl="~/App_Themes/Billing2/Buttons/Send-NonHH-Picking-Blue.gif" 
                                                                    Visible="False" ToolTip="Send the selected order item for picking on the PC only." />
                                                                <asp:ImageButton ID="btnResendOne" runat="server" 
                                                                    AlternateText="Send Selected Item for Picking" 
                                                                    ImageUrl="~/App_Themes/Billing/Buttons/Send-SELECTED-Picking-Blue.gif" ToolTip="Send the selected order item for picking." />
                                                                <br />
                                                                <asp:ImageButton ID="btnResendOnePopup" runat="server" 
                                                                    AlternateText="Send Selected Item for Picking" 
                                                                    ImageUrl="~/App_Themes/Billing/Buttons/Send-SELECTED-Picking-Blue.gif" 
                                                                    style="margin-left: 0px" ToolTip="Send the selected order item for picking." />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:ImageButton ID="btnEditOrderItem" runat="server" 
                                                                    ImageUrl="~/App_Themes/Billing/Buttons/Edit-Selected-Item-blue.gif" ToolTip="Change the quantity or weight ordered for the selected order item." />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                      <tr style="border-style: none; vertical-align: bottom;">
                                         
                                          <td style="border-style: none" class="auto-style2">
                                              &nbsp;&nbsp; &nbsp;</td>
                                      </tr>
                                      <tr style="border-style: none; vertical-align: top; background-color: #FFFFFF;">
                                         
                                          <td style="border-style: none; vertical-align: top; text-align: left;" class="auto-style2">
                                              &nbsp;&nbsp; &nbsp;</td>
                                      </tr>
                            </table>
                           
                </td>
            </tr>
            <tr>
                <td style="border-style: none; border-top-width: 1px; border-right-width: 1px; border-left-width: 1px; border-top-color: #808080; border-right-color: #808080; border-left-color: #808080; font-family: Arial; font-size: 2px; background-color: #E6E6CC;">
                        &nbsp;</td>
            </tr>
            <tr>
                <td>
                    
                    &nbsp;
                    
                    &nbsp;</td>
            </tr>
            <tr>
                <td>
                    
                    <table style="width: 930px">
                        <tr>
                            <td>
                        <asp:Label ID="lblPicking" runat="server" Font-Bold="True" 
                            style="font-size: 12pt; font-family: Arial" 
                                        Text="Items Currently Scheduled for Picking" Font-Names="Arial" 
                                    Font-Size="11pt"></asp:Label>
                                &nbsp;<asp:Label ID="lblPickingExtra" runat="server" Font-Names="Arial" Font-Size="9pt" 
                                    
                                    
                                    Text="(These items are already on a handheld or will be downloaded to a handheld during the next Synchronisation)"></asp:Label>
&nbsp;</td>
                        </tr>
                        <tr>
                            <td>

                                <br />
                                <ig:WebDataGrid ID="wdgHandheldItems" runat="server" AltItemCssClass="VerifyGrid_Report_AlternateRow" AutoGenerateColumns="False" BackColor="White" CssClass="VerifyGrid_Report_Frame" EnableAjax="False" EnableTheming="True" Font-Names="Arial" FooterCaptionCssClass="VerifyGrid_Report_Footer" HeaderCaptionCssClass="VerifyGrid_Report_HeaderGreen_10pt" Height="190px" ItemCssClass="VerifyGrid_Report_Row" Width="847px">
                                       <Columns>
                                        <ig:BoundDataField DataFieldName="ProductName" Key="Product" Width="350px">
                                            <header Text="Product Description:"/>
                                        </ig:BoundDataField>
                                        <ig:BoundDataField DataFieldName="OrderedQty" Key="Qty" Width="80px">
                                            <Header Text="Quantity Requested:"/>
                                        </ig:BoundDataField>
                                        <ig:BoundDataField DataFieldName="OrderedWgt" Key="Weight" Width="90px">
                                            <Header Text="Weight Requested:"/>
                                         </ig:BoundDataField>
                                        <ig:BoundDataField DataFieldName="Synched" Key="DeliveryID" Width="110px">
                                            <Header Text="Has Been Synchronised?" />
                                        </ig:BoundDataField>
									    <ig:BoundDataField DataFieldName="Return" Key="Return" Width="200px">
                                            <Header Text="" />
                                        </ig:BoundDataField>
									</Columns>                                                               
                    <Behaviors>
                        <ig:ColumnResizing>
                        </ig:ColumnResizing>
                    </Behaviors>
                </ig:WebDataGrid>


                                 <br />
                                 <br />

                                
                                </td>
                        </tr>
                        </table>
                    
                </td>
            </tr>
            </table>

    
    </div>
    
    
                 <asp:HiddenField ID="lblPerUnitWeight" runat="server" />
                   
                <asp:HiddenField ID="hidCalcWgtFromQty" runat="server" />

                 <asp:HiddenField ID="hdnUnitPrice" runat="server" />
                 <asp:HiddenField ID="hdnUnitOfSale" runat="server" />
                  <asp:HiddenField ID="hdnPCOnly" runat="server" />


    
                <asp:Panel ID="Panel3" runat="server" backcolor="White" borderstyle="Solid" 
                    borderwidth="1px" Height="312px" width="900px" style="display:none">
                    
                    <div style="margin-left:10px">
                    
                    <table style="width: 830px">
                        <tr>
                            <td style="width: 142px">
                                &nbsp;</td>
                            <td style="text-align: center;" colspan="2">
                                <asp:Label ID="Label37" runat="server" Font-Bold="True" Font-Names="Arial" 
                                    Font-Overline="False" Font-Size="16pt" Text="Enter Amount to be Picked"></asp:Label>
                            </td>
                            <td>
                                &nbsp;</td>
                        </tr>
                        <tr>
                            <td style="width: 142px">
                                &nbsp;</td>
                            <td style="width: 249px">
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>
                        </tr>
                        <tr>
                            <td style="width: 142px">
                                <asp:Label ID="Label9" runat="server" Font-Bold="True" Font-Names="Arial" 
                                    Font-Size="10pt" Text="Product Name:"></asp:Label>
                            </td>
                            <td style="width: 249px">
                                <asp:Label ID="lblPnlProductName" runat="server" Font-Bold="False" 
                                    Font-Names="Arial" Font-Size="10pt"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label20" runat="server" Font-Bold="True" Font-Names="Arial" 
                                    Font-Size="10pt" Text="Product Code:" Visible="False"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblPnlProductCode" runat="server" Font-Bold="False" 
                                    Font-Names="Arial" Font-Size="10pt" Visible="False"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 142px">
                                &nbsp;</td>
                            <td style="width: 249px">
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>
                        </tr>
                        <tr>
                            <td style="width: 142px">
                                <asp:Label ID="Label21" runat="server" Font-Bold="True" Font-Names="Arial" 
                                    Font-Size="10pt" Text="Ordered Quantity:"></asp:Label>
                            </td>
                            <td style="width: 249px">
                                <asp:Label ID="lblPnlOrderedQty" runat="server" Font-Bold="False" 
                                    Font-Names="Arial" Font-Size="10pt"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblPnlOrderedWgtLabel" runat="server" Font-Bold="True" 
                                    Font-Names="Arial" Font-Size="10pt" Text="Ordered Weight:"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblPnlOrderedWgt" runat="server" Font-Bold="False" 
                                    Font-Names="Arial" Font-Size="10pt"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 142px">
                                &nbsp;</td>
                            <td style="width: 249px">
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>
                        </tr>
                        <tr>
                            <td style="width: 142px">
                                <asp:Label ID="Label23" runat="server" Font-Bold="True" Font-Names="Arial" 
                                    Font-Size="10pt" Text="Outstanding Quantity:"></asp:Label>
                            </td>
                            <td style="width: 249px">
                                <asp:Label ID="lblPnlOutstandingQty" runat="server" Font-Bold="False" 
                                    Font-Names="Arial" Font-Size="10pt"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblPnlOutstandingWgtLabel" runat="server" Font-Bold="True" 
                                    Font-Names="Arial" Font-Size="10pt" Text="Outstanding Weight:"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblPnlOutstandingWgt" runat="server" Font-Bold="False" 
                                    Font-Names="Arial" Font-Size="10pt"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 142px">
                                &nbsp;</td>
                            <td style="width: 249px">
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:Label ID="Label25" runat="server" Font-Bold="True" Font-Names="Arial" 
                                    Font-Size="10pt" Text="Send to Handheld for Picking:"></asp:Label>
                            </td>
                            <td>
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>
                        </tr>
                        <tr>
                            <td style="width: 142px">
                                <asp:Label ID="Label26" runat="server" Font-Bold="True" Font-Names="Arial" 
                                    Font-Size="10pt" Text="Quantity:"></asp:Label>
                            </td>
                            <td style="width: 249px">
                                <asp:TextBox ID="txtPnlQtyToPick" runat="server" onkeyup="CalcWeightFromQty();"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Label ID="lblPnlWgtToPickLabel" runat="server" Font-Bold="True" 
                                    Font-Names="Arial" Font-Size="10pt" Text="Weight:"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtPnlWgtToPick" runat="server" onkeyup="CalcQtyFromWeight();"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 142px">
                                &nbsp;</td>
                            <td style="width: 249px">
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>
                        </tr>
                        <tr>
                            <td style="width: 142px">
                                &nbsp;</td>
                            <td style="width: 249px">
                                <asp:ImageButton ID="cmdOKConfirm" runat="server" 
                                    ImageUrl="~/App_Themes/Billing/Buttons/OK.gif" />
                            </td>
                            <td>
                                <asp:ImageButton ID="btnCancel" runat="server" AlternateText="Cancel" 
                                    ImageUrl="~/App_Themes/Billing/Buttons/Cancel.gif" />
                            </td>
                            <td>
                                &nbsp;</td>
                        </tr>
                    </table>
                    <br />
                  
                    
                    </div>
                  
                   </asp:Panel>
                
    
    
                    <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender1" runat="server" 
                BackgroundCssClass="modalBackground" CancelControlID="btnCancel" 
                DropShadow="True" oncancelscript="HideMsgPopup()" PopupControlID="Panel3" 
                TargetControlID="Panel3">
            </ajaxToolkit:ModalPopupExtender>

   
   
   
    
                <asp:Panel ID="plnEditOrder" runat="server" backcolor="White" borderstyle="Solid" 
                    borderwidth="1px" height="362px" width="900px" style="display:none">
                    
                    <div style="margin-left:10px; height: 277px;">
                    
                    <table style="width: 830px">
                        <tr>
                            <td style="width: 18px">
                                &nbsp;</td>
                            <td style="width: 142px">
                                &nbsp;</td>
                            <td colspan="2" style="text-align: center;">
                                <asp:Label ID="Label36" runat="server" Font-Bold="True" Font-Names="Arial" 
                                    Font-Overline="False" Font-Size="16pt" Text="Enter the New Order Amount"></asp:Label>
                            </td>
                            <td>
                                &nbsp;</td>
                        </tr>
                        <tr>
                            <td style="width: 18px">
                                &nbsp;</td>
                            <td style="width: 142px">
                                &nbsp;</td>
                            <td style="width: 249px">
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>
                        </tr>
                        <tr>
                            <td style="width: 18px">
                                &nbsp;</td>
                            <td style="width: 142px">
                                <asp:Label ID="Label29" runat="server" Font-Bold="True" 
                                    Font-Names="Arial" Font-Size="10pt" Text="Product Name:"></asp:Label>
                            </td>
                            <td style="width: 249px">
                                <asp:Label ID="lblPnlProductNameEdit" runat="server" Font-Bold="False" Font-Names="Arial" 
                                    Font-Size="10pt"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label30" runat="server" Font-Bold="True" 
                                    Font-Names="Arial" Font-Size="10pt" Visible="False" Text="Product Code:"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblPnlProductCodeEdit" runat="server" Font-Bold="False" 
                                    Font-Names="Arial" Font-Size="10pt" Visible="False"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 18px">
                                &nbsp;</td>
                            <td style="width: 142px">
                                &nbsp;</td>
                            <td style="width: 249px">
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>
                        </tr>
                        <tr>
                            <td style="width: 18px">
                                &nbsp;</td>
                            <td style="width: 142px">
                                <asp:Label ID="Label31" runat="server" Font-Bold="True" 
                                    Font-Names="Arial" Font-Size="10pt" Text="Ordered Quantity:"></asp:Label>
                            </td>
                            <td style="width: 249px">
                                <asp:Label ID="lblPnlOrderedQtyEdit" runat="server" Font-Bold="False" Font-Names="Arial" 
                                    Font-Size="10pt"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblPnlOrderedWgtLabelEdit" runat="server" Font-Bold="True" 
                                    Font-Names="Arial" Font-Size="10pt" Text="Ordered Weight:"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblPnlOrderedWgtEdit" runat="server" Font-Bold="False" 
                                    Font-Names="Arial" Font-Size="10pt"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 18px">
                                &nbsp;</td>
                            <td style="width: 142px">
                                &nbsp;</td>
                            <td style="width: 249px">
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>
                        </tr>
                        <tr>
                            <td style="width: 18px">
                                &nbsp;</td>
                            <td style="width: 142px">
                                <asp:Label ID="Label32" runat="server" Font-Bold="True" 
                                    Font-Names="Arial" Font-Size="10pt" Text="Outstanding Quantity:"></asp:Label>
                            </td>
                            <td style="width: 249px">
                                <asp:Label ID="lblPnlOutstandingQtyEdit" runat="server" Font-Bold="False" Font-Names="Arial" 
                                    Font-Size="10pt"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblPnlOutstandingWgtLabelEdit" runat="server" Font-Bold="True" 
                                    Font-Names="Arial" Font-Size="10pt" Text="Outstanding Weight:"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblPnlOutstandingWgtEdit" runat="server" Font-Bold="False" 
                                    Font-Names="Arial" Font-Size="10pt"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 18px">
                                &nbsp;</td>
                            <td style="width: 142px">
                                &nbsp;</td>
                            <td style="width: 249px">
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>
                        </tr>
                        <tr>
                            <td style="width: 18px">
                                &nbsp;</td>
                            <td style="width: 142px">
                                <asp:Label ID="Label35" runat="server" Font-Bold="True" 
                                    Font-Names="Arial" Font-Size="10pt" Text="Delivered Quantity:"></asp:Label>
                            </td>
                            <td style="width: 249px">
                                <asp:Label ID="lblPnlDeliveredQtyEdit" runat="server" Font-Bold="False" 
                                    Font-Names="Arial" Font-Size="10pt"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblPnlDeliveredWgtLabelEdit" runat="server" Font-Bold="True" 
                                    Font-Names="Arial" Font-Size="10pt" Text="Delivered Weight:"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblPnlDeliveredWgtEdit" runat="server" Font-Bold="False" 
                                    Font-Names="Arial" Font-Size="10pt"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 18px">
                                &nbsp;</td>
                            <td style="width: 142px">
                                &nbsp;</td>
                            <td style="width: 249px">
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>
                        </tr>
                        <tr>
                            <td style="width: 18px">
                                &nbsp;</td>
                            <td colspan="2">
                                <asp:Label ID="Label33" runat="server" Font-Bold="True" Font-Names="Arial" 
                                    Font-Size="10pt" Text="New Order Amount:"></asp:Label>
                            </td>
                            <td>
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>
                        </tr>
                        <tr>
                            <td style="width: 18px">
                                &nbsp;</td>
                            <td style="width: 142px">
                                <asp:Label ID="Label34" runat="server" Font-Bold="True" Font-Names="Arial" 
                                    Font-Size="10pt" Text="Quantity:"></asp:Label>
                            </td>
                            <td style="width: 249px">
                                <asp:TextBox ID="txtPnlNewOrderQty" runat="server" 
                                    onkeyup="CalcWeightFromQtyEdit();"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Label ID="lblPnlNewOrderWgtLabel" runat="server" Font-Bold="True" 
                                    Font-Names="Arial" Font-Size="10pt" Text="Weight:"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtPnlNewOrderWgt" runat="server" 
                                    onkeyup="CalcQtyFromWeightEdit();"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 18px">
                                &nbsp;</td>
                            <td style="width: 142px">
                                &nbsp;</td>
                            <td style="width: 249px">
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>
                        </tr>
                        <tr>
                            <td style="width: 18px">
                                &nbsp;</td>
                            <td style="width: 142px">
                                &nbsp;</td>
                            <td style="width: 249px">
                                <asp:ImageButton ID="cmdOKConfirmEdit" runat="server" 
                                    ImageUrl="~/App_Themes/Billing/Buttons/OK.gif" />
                            </td>
                            <td>
                                <asp:ImageButton ID="cmdCancelEdit" runat="server" AlternateText="Cancel" 
                                    ImageUrl="~/App_Themes/Billing/Buttons/Cancel.gif" />
                            </td>
                            <td>
                                &nbsp;</td>
                        </tr>
                    </table>
                    <br />
                  
                    
                        <br />
                  
                    
                        <br />
                  
                    
                    </div>
                  
                   </asp:Panel>
                  <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender2" runat="server" 
                    BackgroundCssClass="modalBackground" CancelControlID="cmdCancelEdit" 
                    DropShadow="True" oncancelscript="HideMsgPopupEdit()" PopupControlID="plnEditOrder" 
                    TargetControlID="plnEditOrder">
                </ajaxToolkit:ModalPopupExtender>

   
                  <br />

                  <asp:Panel ID="PanelMsg" runat="server" backcolor="White" borderstyle="Solid" 
                    borderwidth="1px" Height="196px" width="900px" style="display:none">
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
                        <asp:ImageButton ID="cmdMsgOK" runat="server" 
                            ImageUrl="~/App_Themes/Billing/Buttons/OK.gif" />
                      
                    </center>
                    <center>
                    </center>
                    
                </asp:Panel>
                <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender3" runat="server" 
                    BackgroundCssClass="modalBackground" CancelControlID="cmdMsgOK" 
                    DropShadow="True" oncancelscript="HideMsgPopup3()" PopupControlID="PanelMsg" 
                    TargetControlID="PanelMsg">
                </ajaxToolkit:ModalPopupExtender>

                 
    
      <br />

  </ContentTemplate>
       
        </asp:UpdatePanel>


                        </asp:Content>




