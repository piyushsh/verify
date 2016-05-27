<%@ Page   Language="VB" MasterPageFile= "~/TabPages/ProjectMain.master" AutoEventWireup="false" CodeFile="Fulfill_Opening.aspx.vb" Inherits="TabPages_Fulfill_Opening"  EnableEventValidation="false"%>

<%@ MasterType VirtualPath="~/TabPages/ProjectMain.master" %>



<%@ Register Assembly="Infragistics4.Web.v13.2, Version=13.2.20132.2028, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" namespace="Infragistics.Web.UI.DisplayControls" tagprefix="ig" %>
<%@ Register Assembly="Infragistics4.Web.v13.2, Version=13.2.20132.2028, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" namespace="Infragistics.Web.UI.EditorControls" tagprefix="ig" %>
<%@ Register Assembly="Infragistics4.Web.v13.2, Version=13.2.20132.2028, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" namespace="Infragistics.Web.UI.GridControls" tagprefix="ig" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ProjectMAIN_content" Runat="Server">

    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        
        <Triggers>
            <asp:PostBackTrigger ControlID="btnExport" />
        </Triggers>

            <ContentTemplate>
                 <script type="text/javascript" src="..\scripts\json2.js">    </script>
    <script type="text/javascript" src="..\Scripts\jquery-1.10.2.js"></script>
<script type="text/javascript" src="..\Scripts\jquery.tmpl.js"></script>
    <script type="text/javascript">

       
        function HandleKeyPress(panel, control, allow) {

            enterClicked = false;

            var key;
            var e = window.event;
            key = e.keyCode ? e.keyCode : e.which;
            if (key == 13)
                enterClicked = true;
            else
                enterClicked = false;

            if (enterClicked == true) {
                var ScannedData = control.value;
                control.value = '';
                var lblSelectedProduct;

                var btnOKEdit = document.getElementById("<%=cmdOKConfirm.ClientID%>");
                var btnOKAdd = document.getElementById("<%=cmdOKConfirmEdit.ClientID%>");
                
                 
                var Barcode = SplitBarcode(ScannedData);
                if (Barcode.GTIN != '')
                    btnOKAdd.disabled = false
                     btnOKEdit.disabled = false

                    if (panel == 'Edit') {
                        var txtAmount = document.getElementById("<%=txtPnlWgtToPick.ClientID%>");
                        var txtTrace = document.getElementById("<%=txtpnlTraceCode_NS.ClientID%>");
                        var lblSelectedProductText = document.getElementById("<%=lblPnlProductName_NS.ClientID%>");
                        lblSelectedProduct = lblSelectedProductText.innerText;

                        if (txtAmount != null)  // the weight box may not be visible
                            txtAmount.value = Barcode.Weight;

                        txtTrace.value = Barcode.BatchNumber;

                        HandleScanOnEditPanel("<%=txtpnlTraceCode_NS.ClientID%>", Barcode.GTIN, "<%=hdnPnlTraceCodeId.ClientID%>", "<%=lblPnlProductMessage.ClientID%>");
                    

                    }
                    else if (panel == 'Add') {
                        var txtAmount = document.getElementById("<%=txtPnlNewBatchWgt.ClientID%>");
                        var txtTrace = document.getElementById("<%=txtPnlNewBatchTrace.ClientID%>");
                        var lblSelectedProductText = document.getElementById("<%=lblPnlProductNameEdit.ClientID%>");
                        lblSelectedProduct = lblSelectedProductText.innerText;

                        if (txtAmount != null)  // the weight box may not be visible
                            txtAmount.value = Barcode.Weight;

                        txtTrace.value = Barcode.BatchNumber;

                        HandleScanOnAddPanel("<%=txtPnlNewBatchTrace.ClientID%>", Barcode.GTIN, "<%=hdnPnlAddTraceCodeId.ClientID%>", "<%=lblPnlAddProductMessage.ClientID%>");
                        var txtMessageAdd = document.getElementById("<%=lblPnlAddProductMessage.ClientID%>");
                    


                    }



                return false;
            }
            else {
                // handle other keys

                if (allow == 'numeric') {
                    if (isNumberKey(event) == false)
                        return false;
                }
                else if (allow == 'alpha') {
                    if (isAlphaKey(event) == false)
                        return false;
                }
                else if (allow == 'alphanumeric') {
                    if (isAlphaNumericKey(event) == false)
                        return false;
                }

            }
        }




        function ShowAddClose(sender, e) {

            var ddlProd = document.getElementById("<%=ddlProducts.ClientID%>");

            if (ddlProd.options[ddlProd.selectedIndex].innerText == "Select Product ..") {
                alert('You must select a Product!');
                return false;
            }


            // Check for valid qty or weight        
            var UoS = document.getElementById("<%=lblPnlProdMessage.ClientID%>");
            if (UoS.value == 'QUANTITY') {
                var txtQty = document.getElementById("<%=txtQuantity.ClientID%>");
                if (txtQty.value == "") {
                    alert('You must enter a Quantity');
                    return false;
                }
            }
            else {
                var txtAmount = document.getElementById("<%=txtWeight.ClientID%>");
                if (txtAmount.value == "") {
                    alert('You must enter an Amount');
                    return false;
                }
            }




            __doPostBack(sender, e);
        }

        function HandleProductCodeOnAddPanel(src, destMessageCtrl, ddlProdCascadeCtrl, ddlProdCatCascadeCtrl) {
            var prodCode = document.getElementById(src);
            if (prodCode.value != '') {
                // call server side method - pass the set of controls we want to update in an array structure
                var context = new Array();
                context[0] = destMessageCtrl;
                context[1] = ddlProdCascadeCtrl;
                context[2] = ddlProdCatCascadeCtrl;

                PageMethods.GetProductDataForCodeWebMethod(prodCode.value, CodeChangeCallSuccess, CodeChangeCallFailed, context);
            }

        }




        function CodeChangeCallSuccess(result, context, methodName) {
            if (result == '[]') {
                alert('Not a valid Product Code');
                return false;
            }
            else {
                // the result of a Successful PageMethod call is a datatable
                var jsResult = JSON.parse(result);
                var dataRow = jsResult[0];

                var intUOS = dataRow.UnitOfSale;
                var UOSText;
                if (intUOS == 1)
                    UOSText = 'QUANTITY';
                else
                    UOSText = 'WEIGHT';

                // set the destination message control with the UnitofSale text
                var dest = document.getElementById(context[0]);
                dest.value = UOSText;

                // set the destination product dropdowns

                // get a reference to the parent CDD - the actual parameter for the $find will vary depending on whether you are in a master or child page
                var ddlCategory = $find(context[2]);

                // force a change in the parent CDD
                ddlCategory.set_SelectedValue('', '');
                ddlCategory._onParentChange(null, false);

                // set the parent value
                // NB the set_SelectedValue function takes a string param!
                ddlCategory.set_SelectedValue(dataRow.Product_LineID.toString());

                // get a reference to the child CDD
                var ddlProd = $find(context[1]);
                ddlProd._onParentChange(null, true);
                // set the child value
                ddlProd.set_SelectedValue(dataRow.ProductID.toString());

                var hdnProductValue = document.getElementById("<%=hdnProductValue.ClientID%>");
              hdnProductValue.value = dataRow.ProductID

              // store the Unit of Sale of the selected product
              var hdnProductUnitOfSale = document.getElementById("<%=hdnUnitOfSale.ClientID%>");
              hdnProductUnitOfSale.value = intUOS

              // store the Per Unit Weight of the selected product
              var dblPerUnitWeight = dataRow.AvgWeightPerUnit;
              var hdnProductPerUnitWeight = document.getElementById("<%=lblPerUnitWeight.ClientID%>");
              hdnProductPerUnitWeight.value = dblPerUnitWeight


              // If we are on the Selected Batch grid we refresh the batches grid here
              if (context[0].indexOf('lblPnlSBProdMessage') > 0)
                  SelectedSBProductChanged();

          }

      }
      // alert message on some failure
      function CodeChangeCallFailed(result, context, methodName) {
          alert(result.get_message());
      }



        function HandleProductOnAddPanel(src, destMessageCtrl, destProdCodeCtrl) {
            var prod = document.getElementById(src);
            if (prod.options[prod.selectedIndex].innerText == "Select Product ..") {
                destMessageCtrl.value = "";
            } else {
                var myindex = prod.selectedIndex
                var SelValue = prod.options[myindex].value
                // call server side method - pass the set of controls we want to update in an array structure
                var context = new Array();
                context[0] = destMessageCtrl;
                context[1] = destProdCodeCtrl;

                PageMethods.GetProductDataForIdWebMethod(SelValue, DDLChangeCallSuccess, DDLChangeCallFailed, context);
            }
        }
        function DDLChangeCallSuccess(result, context, methodName) {
            // the result of a Successful PageMethod call is a datatable
            var jsResult = JSON.parse(result);
            var dataRow = jsResult[0];

            var intUOS = dataRow.UnitOfSale;
            var UOSText;
            if (intUOS == 1)
                UOSText = 'QUANTITY';
            else
                UOSText = 'WEIGHT';

            // set the destination message control with the UnitofSale text
            var dest = document.getElementById(context[0]);
            dest.value = UOSText;
            // set the destination product code control with the ProductCode text
            var destProdCode = document.getElementById(context[1]);
            destProdCode.value = dataRow.Catalog_Number;

            // store the id of the selected product
            var hdnProductValue = document.getElementById("<%=hdnProductValue.ClientID%>");
            hdnProductValue.value = dataRow.ProductID

            // If we are on the Selected Batch grid we refresh the batches grid here
            if (context[0].indexOf('lblPnlSBProdMessage') > 0)
                SelectedSBProductChanged();

        }
        // alert message on some failure
        function DDLChangeCallFailed(result, context, methodName) {
            alert(result.get_message());
        }

        function HideMsgPopup() 
        { 
            var modal = $find("<%=ModalPopupExtender1.BehaviorID%>");
            modal.hide(); 
        }
            function HideMsgPopupAddNew(sender, e) 
        { 
            __doPostBack(sender, e);
        }

        function HideMsgPopupMsg() {
            var modal = $find("<%=ModalPopupExtenderMsg.BehaviorID%>");
            modal.hide();
        }

        function HideMsgPopupMsg3() {
            var modal = $find("<%=ModalPopupExtender3.BehaviorID%>");
                modal.hide();
        }

        function HideMsgPopupEdit() {
            var modal = $find("<%=ModalPopupExtender2.BehaviorID%>");
            modal.hide();
        }
       

        function HideMsgPopupOK() 
        { 
          
            var modal = $find("<%=ModalPopupExtender1.BehaviorID%>");
            modal.hide(); 
            __doPostBack();
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
                var txtQty = document.getElementById("<%=txtPnlNewBatchQty.ClientID%>"); 
             var OrderedQty=document.getElementById("<%=lblPnlOrderedQtyEdit.ClientID%>"); 
             var OutstandingQty=document.getElementById("<%=lblPnlOutstandingQtyEdit.ClientID%>"); 
             
            if (parseFloat(txtQty.value) < (parseFloat(OrderedQty.innerText) - parseFloat(OutstandingQty.innerText))) {
                TooLow = "True"
            }else {
                TooLow = "False"
            }
                          
           } else {
            var txtWeight = document.getElementById("<%=txtPnlNewBatchWgt.ClientID%>"); 
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

        function HideMsgPopupAdd() {
            var modal = $find("<%=modalAddProd.BehaviorID%>");
            modal.hide();
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
            var UnitOfSale =document.getElementById("<%=hdnUnitOfSale.ClientID%>");   
            if(UnitOfSale.value != 1 ) {
                var DoCalc =document.getElementById("<%=hidCalcWgtFromQty.ClientID%>"); 

                var numWeightPerUnit=document.getElementById("<%=lblPerUnitWeight.ClientID%>"); 

               var txtQty=document.getElementById("<%=txtPnlQtyToPick.ClientID%>"); 

                var txtWeight=document.getElementById("<%=txtPnlWgtToPick.ClientID%>"); 
                if ((DoCalc.value) == "YES" ) {
                    if (parseFloat(txtQty.value) > 0 ) {
                        calculatedwgt = parseFloat(txtQty.value) * parseFloat(numWeightPerUnit.value);
                        calculatedwgt = roundNumber(calculatedwgt, 4);
                        txtWeight.innerText = String(calculatedwgt);
                    }
                }
            }
            }        

function CalcQtyFromWeightEdit()

            {

                var txtWeight = document.getElementById("<%=txtPnlNewBatchWgt.ClientID%>"); 
                
            var DoCalc =document.getElementById("<%=hidCalcWgtFromQty.ClientID%>"); 

            var numWeightPerUnit=document.getElementById("<%=lblPerUnitWeight.ClientID%>");

            var txtQty = document.getElementById("<%=txtPnlNewBatchQty.ClientID%>"); 
            
         
              if ((DoCalc.value) == "YES" ) {
                      if (parseFloat(txtWeight.value) > 0 ) {
                txtQty.innerText = String(parseInt(parseFloat(txtWeight.value) / parseFloat(numWeightPerUnit.value)));
                         }
                }
            }        

        function CalcWeightFromQtyEdit()

            {
                var calculatedwgt
                 var UnitOfSale =document.getElementById("<%=hdnUnitOfSale.ClientID%>");   
             if(UnitOfSale.value != 1 ) {
                var DoCalc =document.getElementById("<%=hidCalcWgtFromQty.ClientID%>"); 

                var numWeightPerUnit=document.getElementById("<%=lblPerUnitWeight.ClientID%>");

                var txtQty = document.getElementById("<%=txtPnlNewBatchQty.ClientID%>");

                var txtWeight = document.getElementById("<%=txtPnlNewBatchWgt.ClientID%>"); 
                if ((DoCalc.value) == "YES" ) {
                    if (parseFloat(txtQty.value) > 0) {
                     calculatedwgt = parseFloat(txtQty.value)* parseFloat(numWeightPerUnit.value);
                        calculatedwgt = roundNumber(calculatedwgt, 4);
                        txtWeight.innerText = String(calculatedwgt);
                    }
                }
            }
            }



            function HandleTraceCodeOnEditPanel(src, dest, destMessage) {
                var ctrl = document.getElementById(src);
                var prod = document.getElementById("<%=hdnPnlProductId.ClientID%>"); 
                // call server side method
                PageMethods.GetTraceCodeExistsForTraceDescAndProdId(ctrl.value, prod.value, CallSuccess, CallFailed, destMessage);
                PageMethods.GetTraceCodeIDForTraceDescAndProdId(ctrl.value, prod.value, CallSuccess, CallFailed, dest);
             }

        function HandleScanOnEditPanel(src,CommCode, dest, destMessage) {
            var ctrl = document.getElementById(src);
            var prod = document.getElementById("<%=hdnPnlProductId.ClientID%>");
             // call server side method
            PageMethods.GetScanIsValid(ctrl.value, prod.value, CommCode, CallSuccess, CallFailed, destMessage);
            PageMethods.GetTraceCodeIDForTraceDescAndProdId(ctrl.value, prod.value, CallSuccess, CallFailed, dest);



            }


        


            // set the destination textbox value with the ContactName
            function CallSuccess(res, destCtrl) {
                var dest = document.getElementById(destCtrl);
                dest.value = res;
            }
            // alert message on some failure
            function CallFailed(res, destCtrl) {
                alert(res.get_message());
            }

            function QtyWgtForSerialNumEditPanel(src, destqty, destwgt, destLocn, destMessage) {
                var ctrl = document.getElementById(src);
                var trace = document.getElementById("<%=hdnPnlTraceCodeId.ClientID%>");
                // call server side method
                PageMethods.CheckExistsForSerialNumAndTraceId(ctrl.value, trace.value, CallSuccess, CallFailed, destMessage);
                PageMethods.GetCurrentQtyForSerialNumAndTraceId(ctrl.value, trace.value, CallSuccess, CallFailed, destqty);
                PageMethods.GetCurrentWgtForSerialNumAndTraceId(ctrl.value, trace.value, CallSuccess, CallFailed, destwgt);
                PageMethods.GetCurrentLocnForSerialNumAndTraceId(ctrl.value, trace.value, CallSuccess, CallFailed, destLocn);
         
            }

            function HandleTraceCodeOnAddPanel(src, dest, destMessage) {
                var ctrl = document.getElementById(src);
                var prod = document.getElementById("<%=hdnPnlAddProductId.ClientID%>");
                // call server side method
                PageMethods.GetTraceCodeExistsForTraceDescAndProdId(ctrl.value, prod.value, CallSuccess, CallFailed, destMessage);
                PageMethods.GetTraceCodeIDForTraceDescAndProdId(ctrl.value, prod.value, CallSuccess, CallFailed, dest);
            }

        function HandleScanOnAddPanel(src, CommCode, dest, destMessage) {
            var ctrl = document.getElementById(src);
            var prod = document.getElementById("<%=hdnPnlAddProductId.ClientID%>");
               // call server side method
            PageMethods.GetScanIsValid(ctrl.value, prod.value, CommCode, CallSuccess, CallFailed, destMessage);
            PageMethods.GetTraceCodeIDForTraceDescAndProdId(ctrl.value, prod.value, CallSuccess, CallFailed, dest);
            }


       
            function QtyWgtForSerialNumAddPanel(src, destqty, destwgt, destLocn, destMessage) {
                var ctrl = document.getElementById(src);
                var trace = document.getElementById("<%=hdnPnlAddTraceCodeId.ClientID%>");
                // call server side method
                PageMethods.CheckExistsForSerialNumAndTraceId(ctrl.value, trace.value, CallSuccess, CallFailed, destMessage);
                PageMethods.GetCurrentQtyForSerialNumAndTraceId(ctrl.value, trace.value, CallSuccess, CallFailed, destqty);
                PageMethods.GetCurrentWgtForSerialNumAndTraceId(ctrl.value, trace.value, CallSuccess, CallFailed, destwgt);
                PageMethods.GetCurrentLocnForSerialNumAndTraceId(ctrl.value, trace.value, CallSuccess, CallFailed, destLocn);

            }


        function CancelClicked(sender, e) {
            __doPostBack(sender, e);
        }

        function OkClicked(sender, e) {
            __doPostBack(sender, e);
        }

        function EditItemOKClose(sender, e) {
            // check for valid Batch Number
            var txtTrace = document.getElementById("<%=txtpnlTraceCode_NS.ClientID%>");
            if (txtTrace.value == "") {
                alert('You must enter a Batch Code');
                return false;
            }

           var txtScanMessage = document.getElementById("<%=lblPnlProductMessage.ClientID%>");
            if (txtScanMessage.value != "") {
                alert('You must scan a valid Product');
                return false;
            }

                        __doPostBack(sender, e);
        }


                

        function AddBatchItemOKClose(sender, e) {
            // check for valid Batch Number
            var txtTrace = document.getElementById("<%=txtPnlNewBatchTrace.ClientID%>");
                 if (txtTrace.value == "") {
                     alert('You must enter a Batch Code');
                     return false;
                 }

                 var txtScanMessage = document.getElementById("<%=lblPnlAddProductMessage.ClientID%>");
            if (txtScanMessage.value != "") {
                alert('You must scan a valid Product');
                return false;
            }

            __doPostBack(sender, e);
        }




    </script>
    <script language="javascript" type="text/javascript" src="../BusyBox.js"></script>
     
     
    
    <div style="margin-left:10px; margin-top:10px">
    

        <table style="border-style: none; width: 930px;  border-top-width: 0px;">
            <tr>
                <td style="border-style: none; border-top-width: 1px; border-right-width: 1px; border-left-width: 1px; border-top-color: #808080; border-right-color: #808080; border-left-color: #808080; font-family: Arial; font-size: 2px; background-color: #E6E6CC;" 
                    colspan="6">
                        &nbsp;</td>
            </tr>
            
            <tr>
                <td style="width: 390px">
                        <asp:Label ID="lblGridTitle" runat="server" Font-Bold="True" 
                            style="font-size: 14pt; font-family: Arial">Items Scheduled for Fulfilment</asp:Label>
                    </td>
                <td style="width: 220px">
                        <asp:Image ID="imgView" runat="server" 
                                        ImageUrl="~/App_Themes/Images/SelectViewType.gif" 
                                        AlternateText="Select View Type" />
                    </td>
                <td style="">
                        <asp:DropDownList ID="ddlMainViewType" runat="server" Width="150px" 
                            AutoPostBack="True" Font-Bold="True" Font-Names="Arial" Font-Size="11pt">
                            <asp:ListItem>Order</asp:ListItem>
                            <asp:ListItem>Product</asp:ListItem>
                            <asp:ListItem>Location</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                <td style="">
                        &nbsp;</td>
                <td style="">
                        <asp:Label ID="lblOrderType" runat="server" Visible="False" Font-Names="Arial" 
                            Font-Size="10pt"></asp:Label>
                    </td>
                <td style="">
                        <asp:ImageButton ID="btnProduceItem" runat="server" 
                                                                    
                            ImageUrl="~/App_Themes/Billing/Buttons/Produce-Item-ForOrder-blue.gif" 
                            Visible="False" />
                    </td>
            </tr>
            <tr>
                <td style="vertical-align: top; text-align: left" colspan="6">

                


                    &nbsp;&nbsp;&nbsp;
                    <asp:Label ID="lblTypeSelectedHeader" runat="server" Font-Bold="True" Font-Italic="True" Font-Names="Arial" Font-Size="12pt" ForeColor="Black" Text="Sales Orders:"></asp:Label>
                    &nbsp;&nbsp;<asp:Label ID="lblTypeSelectedSubHead1" runat="server" Font-Bold="True" Font-Italic="True" Font-Names="Arial" Font-Size="10pt" ForeColor="Black" Text="( these are sales orders that awaiting fulfilment )"></asp:Label>


                        
                        </td>
            </tr>

            <tr>
                <td colspan="6" style="vertical-align: top; text-align: left">
                    <ig:WebDataGrid ID="wdgOrderItems" runat="server" AltItemCssClass="VerifyGrid_Report_AlternateRow" AutoGenerateColumns="False" BackColor="White" CssClass="VerifyGrid_Report_Frame" EnableAjax="False" EnableTheming="True" Font-Names="Arial" FooterCaptionCssClass="VerifyGrid_Report_Footer" HeaderCaptionCssClass="VerifyGrid_Report_HeaderGreen_10pt" Height="250px" ItemCssClass="VerifyGrid_Report_Row" Width="980px">
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
                            <ig:BoundDataField DataFieldName="DocketStatus" Key="DocketStatus">
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
                    </ig:WebDataGrid>
                    <ig:WebDataGrid ID="wdgOrderItemsProds" runat="server" AltItemCssClass="VerifyGrid_Report_AlternateRow" AutoGenerateColumns="False" BackColor="White" CssClass="VerifyGrid_Report_Frame" EnableAjax="False" EnableTheming="True" Font-Names="Arial" FooterCaptionCssClass="VerifyGrid_Report_Footer" HeaderCaptionCssClass="VerifyGrid_Report_HeaderGreen_10pt" Height="200px" ItemCssClass="VerifyGrid_Report_Row" Width="552px">
                        <Columns>
                            <ig:BoundDataField DataFieldName="ProductId" Key="ProductCode" Width="200px">
                                <Header Text="Product Code" />
                            </ig:BoundDataField>
                            <ig:BoundDataField DataFieldName="Product_Name" Key="ProductName" Width="350px">
                                <Header Text="Product Name" />
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
                                    <ig:ColumnSummaryInfo ColumnKey="ProductName">
                                        <Summaries>
                                            <ig:Summary SummaryType="Count" />
                                        </Summaries>
                                    </ig:ColumnSummaryInfo>
                                    <ig:ColumnSummaryInfo ColumnKey="ProductName">
                                        <Summaries>
                                            <ig:Summary />
                                        </Summaries>
                                    </ig:ColumnSummaryInfo>
                                </ColumnSummaries>
                                <SummaryButton HoverImageUrl="~/App_Themes/Buttons/Add_Plus_Small.gif" />
                                <ColumnSettings>
                                    <ig:SummaryRowSetting ColumnKey="ProductName">
                                        <SummarySettings>
                                            <ig:SummarySetting SummaryType="Count" />
                                            <ig:SummarySetting />
                                        </SummarySettings>
                                    </ig:SummaryRowSetting>
                                </ColumnSettings>
                            </ig:SummaryRow>
                            <ig:Filtering Enabled="true" FilterButtonCssClass="Filter_LAlign">
                                <ColumnSettings>
                                    <ig:ColumnFilteringSetting ColumnKey="ProductName" Enabled="true" />
                                </ColumnSettings>
                                <ColumnFilters>
                                    <ig:ColumnFilter ColumnKey="ProductName">
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
                    <ig:WebDataGrid ID="wdgOrderItemsLocns" runat="server" AltItemCssClass="VerifyGrid_Report_AlternateRow" AutoGenerateColumns="False" BackColor="White" CssClass="VerifyGrid_Report_Frame" EnableAjax="False" EnableTheming="True" Font-Names="Arial" FooterCaptionCssClass="VerifyGrid_Report_Footer" HeaderCaptionCssClass="VerifyGrid_Report_HeaderGreen_10pt" Height="200px" ItemCssClass="VerifyGrid_Report_Row" Width="832px">
                        <Columns>
                            <ig:BoundDataField DataFieldName="LocationText" Key="Location" Width="200px">
                                <Header Text="Location" />
                            </ig:BoundDataField>
                            <ig:BoundDataField DataFieldName="Catalog_Number" Key="ProductCode" Width="140px">
                                <Header Text="Product Code" />
                            </ig:BoundDataField>
                            <ig:BoundDataField DataFieldName="Product_Name" Key="ProductName" Width="250px">
                                <Header Text="Product Name" />
                            </ig:BoundDataField>
                            <ig:BoundDataField DataFieldName="OrderedQty" Key="OrderedQty" Width="60px">
                                <Header Text="Ordered Qty" />
                            </ig:BoundDataField>
                            <ig:BoundDataField DataFieldName="OrderedWgt" Key="OrderedWgt" Width="60px">
                                <Header Text="Ordered Wgt" />
                            </ig:BoundDataField>
                            <ig:BoundDataField DataFieldName="Quantity" Key="Quantity" Width="60px">
                                <Header Text="Quantity" />
                            </ig:BoundDataField>
                            <ig:BoundDataField DataFieldName="qtyorWeight" Key="qtyorWeight" Width="60px">
                                <Header Text="Weight" />
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
                                    <ig:ColumnSummaryInfo ColumnKey="ProductName">
                                        <Summaries>
                                            <ig:Summary SummaryType="Count" />
                                        </Summaries>
                                    </ig:ColumnSummaryInfo>
                                    <ig:ColumnSummaryInfo ColumnKey="ProductName">
                                        <Summaries>
                                            <ig:Summary />
                                        </Summaries>
                                    </ig:ColumnSummaryInfo>
                                </ColumnSummaries>
                                <SummaryButton HoverImageUrl="~/App_Themes/Buttons/Add_Plus_Small.gif" />
                                <ColumnSettings>
                                    <ig:SummaryRowSetting ColumnKey="ProductName">
                                        <SummarySettings>
                                            <ig:SummarySetting SummaryType="Count" />
                                            <ig:SummarySetting />
                                        </SummarySettings>
                                    </ig:SummaryRowSetting>
                                </ColumnSettings>
                            </ig:SummaryRow>
                            <ig:Filtering Enabled="true" FilterButtonCssClass="Filter_LAlign">
                                <ColumnSettings>
                                    <ig:ColumnFilteringSetting ColumnKey="ProductName" Enabled="true" />
                                </ColumnSettings>
                                <ColumnFilters>
                                    <ig:ColumnFilter ColumnKey="ProductName">
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

            <tr >
                <td style="vertical-align: top; text-align: left" colspan="6">
                        &nbsp;

                </td>
            </tr>

            <tr >
                <td style="vertical-align: top; text-align: left; background-color: #C0C0C0;" colspan="6">
                        &nbsp;

                        <asp:Label ID="lblTypeDetailsHeader" runat="server" Font-Bold="True" Font-Italic="True" Font-Names="Arial" Font-Size="11pt" ForeColor="Black" Text="Items in selected Order:  "></asp:Label>
                        <asp:Label ID="lblDetailsItemSubHeader" runat="server" Font-Bold="True" Font-Italic="True" Font-Names="Arial" Font-Size="10pt" ForeColor="Black" Text=" ...Selected item here "></asp:Label>

                </td>
            </tr>

            <tr>
                <td style="vertical-align: top; text-align: left" colspan="6">
                        
                                  <table style="width: 930px; background-color: #D8E9D9;">
                                  
                                <tr>
                                  
                                    
                                    <td style="width: 720px">
                                        <table align="left" cellpadding="0" cellspacing="0" 
                                            style="width: 920px; float: left; background-color: #D8E9D9;">
                                            <tr>
                                                <td style="width: 710px">
                                                    

                                                    <br />
<ig:WebDataGrid ID="wdgDetails" runat="server" 
        AltItemCssClass="VerifyGrid_Report_AlternateRow" AutoGenerateColumns="False" 
        BackColor="White" CssClass="VerifyGrid_Report_Frame" EnableTheming="True" 
        Font-Names="Arial" FooterCaptionCssClass="VerifyGrid_Report_Footer" 
        HeaderCaptionCssClass="VerifyGrid_Report_HeaderGold_10pt" Height="300px" 
        ItemCssClass="VerifyGrid_Report_Row" Width="703px" EnableAjax="False">
        <Columns>
            <ig:BoundDataField DataFieldName="ProductId" Key="ProductCode" Width="60px">
                <Header Text="Product Code:"/>
            </ig:BoundDataField>
            <ig:BoundDataField DataFieldName="ProductName" Key="ProductName" Width="180px">
                <Header Text="Product Name:"/>
            </ig:BoundDataField>
            <ig:BoundDataField DataFieldName="OrderedQty" Key="OrderedQty" Width="60px">
                <Header Text="Order Qty:"/>
            </ig:BoundDataField>
            <ig:BoundDataField DataFieldName="OrderedWgt" Key="OrderedWgt" Width="70px">
                <Header Text="Order Wgt:"/>
            </ig:BoundDataField>
            <ig:BoundDataField DataFieldName="Quantity" Key="Quantity" Width="60px">
                <Header Text="Fulfilled Quantity:" />
             </ig:BoundDataField>
            <ig:BoundDataField DataFieldName="QtyOrWeight" Key="Weight" Width="70px">
                <Header Text="Fulfill Weight:"/>
             </ig:BoundDataField>
            <ig:BoundDataField DataFieldName="TraceCode" Key="TraceCode" Width="120px">
                <Header Text="Trace Code:"/>
               </ig:BoundDataField>
            <ig:BoundDataField DataFieldName="PriceSoldFor" Key="PriceSoldFor" Width="70px">
                <Header Text="Price: "/>
             </ig:BoundDataField>
            <ig:BoundDataField DataFieldName="LocationName" Key="Location" Width="80px">
                <Header Text="Location:"/>
            </ig:BoundDataField>
            <ig:BoundDataField DataFieldName="RecordId" Hidden="True" Key="RecordId" Width="10px">
                <Header Text="RecordId:"/>
            </ig:BoundDataField>
             <ig:BoundDataField DataFieldName="VATCharged" Hidden="True" Key="VATCharged" Width="10px">
                <Header Text="Tax:"/>
            </ig:BoundDataField>
            
            <ig:BoundDataField DataFieldName="SerialNum" Hidden="True" Key="SerialNum" Width="10px">
                <Header Text="Serial Num"/>
            </ig:BoundDataField>
            <ig:BoundDataField DataFieldName="Barcode" Hidden="True" Key="Barcode" Width="10px">
                <Header Text="Barcode"/>
            </ig:BoundDataField>                               
            <ig:BoundDataField DataFieldName="SaleType" Hidden="True" Key="SaleType" Width="10px">
                <Header Text="SaleType"/>
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
                <ig:ColumnSummaryInfo ColumnKey="ProductName">
                    <Summaries>
                        <ig:Summary SummaryType="Count" />
                    </Summaries>
                </ig:ColumnSummaryInfo>
                
                <ig:ColumnSummaryInfo ColumnKey="ProductName">
                    <Summaries>
                        <ig:Summary />
                    </Summaries>
                </ig:ColumnSummaryInfo>
            </ColumnSummaries>
            <SummaryButton HoverImageUrl="~/App_Themes/Buttons/Add_Plus_Small.gif" />
            <ColumnSettings>
                <ig:SummaryRowSetting ColumnKey="ProductName">
                    <SummarySettings>
                        <ig:SummarySetting SummaryType="Count" />
                        <ig:SummarySetting />
                    </SummarySettings>
                </ig:SummaryRowSetting>
                
            </ColumnSettings>
        </ig:SummaryRow>
        <ig:Filtering Enabled = "true" FilterButtonCssClass="Filter_LAlign">
            <ColumnSettings>
                <ig:ColumnFilteringSetting ColumnKey="ProductName" Enabled="true" />
            </ColumnSettings>
            <ColumnFilters>
                <ig:ColumnFilter ColumnKey="ProductName">
                    <ConditionWrapper>
                        <ig:RuleTextNode />
                    </ConditionWrapper>
                </ig:ColumnFilter>
                
            </ColumnFilters>
            <EditModeActions EnableOnKeyPress="True" />
        </ig:Filtering >
        <ig:Sorting Enabled="True" 
            AscendingImageUrl="~/App_Themes/Buttons/Up_Arrow_small.gif" 
            DescendingImageUrl="~/App_Themes/Buttons/Down_Arrow_small.gif" >
        </ig:Sorting>

    </Behaviors>
  </ig:WebDataGrid>
                                                   

                                                          

                                                    
                                                    <ig:WebDataGrid ID="wdgDetailsProducts" runat="server" 
        AltItemCssClass="VerifyGrid_Report_AlternateRow" AutoGenerateColumns="False" 
        BackColor="White" CssClass="VerifyGrid_Report_Frame" EnableTheming="True" 
        Font-Names="Arial" FooterCaptionCssClass="VerifyGrid_Report_Footer" 
        HeaderCaptionCssClass="VerifyGrid_Report_HeaderGold_10pt" Height="200px" 
        ItemCssClass="VerifyGrid_Report_Row" Width="703px" EnableAjax="False">
        <Columns>
            <ig:BoundDataField DataFieldName="OrderNumber" Key="OrderNumber" Width="60px">
                <Header Text="Order Num:"/>
            </ig:BoundDataField>
            <ig:BoundDataField DataFieldName="CustomerName" Key="CustomerName" Width="180px">
                <Header Text="Customer Name:"/>
            </ig:BoundDataField>
            <ig:BoundDataField DataFieldName="OrderedQty" Key="OrderedQty" Width="60px">
                <Header Text="Order Qty:"/>
            </ig:BoundDataField>
            <ig:BoundDataField DataFieldName="OrderedWgt" Key="OrderedWgt" Width="70px">
                <Header Text="Order Wgt:"/>
            </ig:BoundDataField>
            <ig:BoundDataField DataFieldName="Quantity" Key="Quantity" Width="60px">
                <Header Text="Fulfilled Quantity:" />
             </ig:BoundDataField>
            <ig:BoundDataField DataFieldName="QtyOrWeight" Key="Weight" Width="70px">
                <Header Text="Fulfill Weight:"/>
             </ig:BoundDataField>
            <ig:BoundDataField DataFieldName="TraceCode" Key="TraceCode" Width="70px">
                <Header Text="Trace Code:"/>
               </ig:BoundDataField>
            <ig:BoundDataField DataFieldName="PriceSoldFor" Key="PriceSoldFor" Width="70px">
                <Header Text="Price: "/>
             </ig:BoundDataField>
            <ig:BoundDataField DataFieldName="LocationText" Key="Location" Width="80px">
                <Header Text="Location:"/>
            </ig:BoundDataField>
            <ig:BoundDataField DataFieldName="RecordId" Hidden="True" Key="RecordId" Width="10px">
                <Header Text="RecordId:"/>
            </ig:BoundDataField>
            
            <ig:BoundDataField DataFieldName="SerialNum" Key="SerialNum" Width="10px">
                <Header Text="Serial Num"/>
            </ig:BoundDataField>
            <ig:BoundDataField DataFieldName="Barcode"  Key="Barcode" Width="10px">
                <Header Text="Barcode"/>
            </ig:BoundDataField>                               
            <ig:BoundDataField DataFieldName="SaleType" Hidden="True" Key="SaleType" Width="10px">
                <Header Text="SaleType"/>
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
        <ig:Filtering Enabled = "true" FilterButtonCssClass="Filter_LAlign">
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
        </ig:Filtering >
        <ig:Sorting Enabled="True" 
            AscendingImageUrl="~/App_Themes/Buttons/Up_Arrow_small.gif" 
            DescendingImageUrl="~/App_Themes/Buttons/Down_Arrow_small.gif" >
        </ig:Sorting>

    </Behaviors>
  </ig:WebDataGrid>
                                                   

                                                    <ig:WebDataGrid ID="wdgDetailsLocations" runat="server" 
        AltItemCssClass="VerifyGrid_Report_AlternateRow" AutoGenerateColumns="False" 
        BackColor="White" CssClass="VerifyGrid_Report_Frame" EnableTheming="True" 
        Font-Names="Arial" FooterCaptionCssClass="VerifyGrid_Report_Footer" 
        HeaderCaptionCssClass="VerifyGrid_Report_HeaderGold_10pt" Height="200px" 
        ItemCssClass="VerifyGrid_Report_Row" Width="703px" EnableAjax="False">
        <Columns>
            <ig:BoundDataField DataFieldName="ProductId" Key="ProductCode" Width="70px">
                <Header Text="Product Code:"/>
            </ig:BoundDataField>
            <ig:BoundDataField DataFieldName="OrderNumber" Key="OrderNumber" Width="70px">
                <Header Text="Order Num:"/>
            </ig:BoundDataField>
            <ig:BoundDataField DataFieldName="CustomerName" Key="CustomerName" Width="220px">
                <Header Text="Customer Name:"/>
            </ig:BoundDataField>
            <ig:BoundDataField DataFieldName="OrderedQty" Key="OrderedQty" Width="60px">
                <Header Text="Order Qty:"/>
            </ig:BoundDataField>
            <ig:BoundDataField DataFieldName="OrderedWgt" Key="OrderedWgt" Width="70px">
                <Header Text="Order Wgt:"/>
            </ig:BoundDataField>
            <ig:BoundDataField DataFieldName="Quantity" Key="Quantity" Width="60px">
                <Header Text="Fulfilled Quantity:" />
             </ig:BoundDataField>
            <ig:BoundDataField DataFieldName="QtyOrWeight" Key="Weight" Width="70px">
                <Header Text="Fulfill Weight:"/>
             </ig:BoundDataField>
            <ig:BoundDataField DataFieldName="TraceCode" Key="TraceCode" Width="70px">
                <Header Text="Trace Code:"/>
               </ig:BoundDataField>
            
            <ig:BoundDataField DataFieldName="RecordId" Hidden="True" Key="RecordId" Width="10px">
                <Header Text="RecordId:"/>
            </ig:BoundDataField>
            
            <ig:BoundDataField DataFieldName="SerialNum" Key="SerialNum" Width="10px">
                <Header Text="Serial Num"/>
            </ig:BoundDataField>
            <ig:BoundDataField DataFieldName="Barcode"  Key="Barcode" Width="10px">
                <Header Text="Barcode"/>
            </ig:BoundDataField>                               
            <ig:BoundDataField DataFieldName="SaleType" Hidden="True" Key="SaleType" Width="10px">
                <Header Text="SaleType"/>
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
        <ig:Filtering Enabled = "true" FilterButtonCssClass="Filter_LAlign">
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
        </ig:Filtering >
        <ig:Sorting Enabled="True" 
            AscendingImageUrl="~/App_Themes/Buttons/Up_Arrow_small.gif" 
            DescendingImageUrl="~/App_Themes/Buttons/Down_Arrow_small.gif" >
        </ig:Sorting>

    </Behaviors>
  </ig:WebDataGrid>
                                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                    <asp:ImageButton ID="btnExport" runat="server" ImageUrl="~/App_Themes/Billing/Buttons/Export.GIF" />
                                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                    <br />




                                                </td>
                                                <td style="text-align: left; vertical-align: top">
                                                    <table align="left" cellpadding="0" cellspacing="0" 
                                                        
                                                        
                                                        
                                                        style="width: 190px; float: left; vertical-align: text-top; text-align: center; padding-top: 6px; padding-bottom: 6px; background-color: #D8E9D9;">
                                                        <tr>
                                                            <td align="left">
                                                                <asp:ImageButton ID="btnEditOrderItem" runat="server" 
                                                                    ImageUrl="~/App_Themes/Billing/Buttons/Edit-Selected-Item-blue.gif" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left">
                                                                <asp:ImageButton ID="btnAddNewBatch" runat="server" 
                                                                    ImageUrl="~/App_Themes/Billing/Buttons/Add_New_batch_blue.jpg" 
                                                                    AlternateText="Add New Batch" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left">
                                                                <asp:ImageButton ID="btnAddNewItem" runat="server" 
                                                                    ImageUrl="~/App_Themes/Billing/Buttons/Add-New-Item-blue.gif" />
                                                                <br />
                                                            </td>
                                                        </tr>
                                                         <tr>
                                                            <td align="left">
                                                                <asp:ImageButton ID="btnAddSkid" runat="server" 
                                                                    ImageUrl="~/App_Themes/Billing/Buttons/Add_SkidtoOrder_blue.jpg" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left">
                                                                <asp:ImageButton ID="btnDeleteItem" runat="server" 
                                                                    ImageUrl="~/App_Themes/Billing/Buttons/Delete_Selected_item_blue.jpg" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left">
                                                                <strong><span style="font-size: 14pt; color: #b28700; font-family: Arial">
                                                                <asp:ImageButton ID="btnViewProductDetails" runat="server" 
                                                                    ImageUrl="~/App_Themes/Billing/Buttons/View-Product-Stock-Position-Blue.gif" />
                                                                </span></strong>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left">
                                                                <asp:ImageButton ID="btnConfirmOrder" runat="server" 
                                                                    ImageUrl="~/App_Themes/Billing/Buttons/Complete-Order-blue.gif" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left">&nbsp;</td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                      <tr style="border-style: none; vertical-align: top;">
                                          <td style="width: 900px; height: 66px;">
                                              <asp:Label ID="lblConmmentTitle" runat="server" Font-Bold="True" 
                                                  style="font-size: 11pt; font-family: Arial" Font-Names="Arial" Font-Size="11pt">Order Comment:</asp:Label>

                                                    <asp:Label ID="lblComment" runat="server" Font-Bold="False" 
                                                  style="font-size: 11pt; font-family: Arial" Font-Names="Arial" Font-Size="10pt"></asp:Label>
                                          </td>
                                      </tr>
                                      <tr style="border-style: none; vertical-align: top; background-color: #FFFFFF;">
                                          <td style="text-align: right; vertical-align: top;">
                                              &nbsp;</td>
                                      </tr>
                            </table>
                           
                </td>
            </tr>
            <tr>
                <td style="border-style: none; border-top-width: 1px; border-right-width: 1px; border-left-width: 1px; border-top-color: #808080; border-right-color: #808080; border-left-color: #808080; font-family: Arial; font-size: 2px; background-color: #E6E6CC;" 
                    colspan="6">
                        &nbsp;</td>
            </tr>
            <tr>
                <td colspan="6">
                    
                    &nbsp;
                    
                    &nbsp;</td>
            </tr>
            <tr>
                <td colspan="6">
                    
                    <table style="width: 930px">
                        <tr>
                            <td>
                                &nbsp;&nbsp;</td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;</td>
                        </tr>
                        </table>
                    
                </td>
            </tr>
            </table>

    
    </div>
    
    
    
                <asp:Panel ID="Panel3" runat="server" backcolor="White" borderstyle="Solid" 
                    borderwidth="1px" Height="369px" width="900px" style="display:">
                    
                    <div style="margin-left:10px">
                    
                    <table style="width: 830px">
                        <tr>
                            <td style="width: 142px">
                                &nbsp;</td>
                            <td style="text-align: center;" colspan="2">
                                <asp:Label ID="Label37_NS" runat="server" Font-Bold="True" Font-Names="Arial" 
                                    Font-Overline="False" Font-Size="16pt" Text="Enter Amount Fulfilled"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtScan_NS" runat="server" Font-Bold="True" Width="166px" 
                                    Visible="False">...  SCAN Here ....</asp:TextBox>
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
                                <asp:HiddenField ID="hdnPnlProductId" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 142px">
                                <asp:Label ID="Label9_NS" runat="server" Font-Bold="True" Font-Names="Arial" 
                                    Font-Size="10pt" Text="Product Name:"></asp:Label>
                            </td>
                            <td style="width: 249px">
                                <asp:Label ID="lblPnlProductName_NS" runat="server" Font-Bold="False" 
                                    Font-Names="Arial" Font-Size="10pt"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label20_NS" runat="server" Font-Bold="True" Font-Names="Arial" 
                                    Font-Size="10pt" Text="Product Code:" Visible="False"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblPnlProductCode_NS" runat="server" Font-Bold="False" 
                                    Font-Names="Arial" Font-Size="10pt" Visible="False"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 142px">
                                <asp:Label ID="Label21_NS" runat="server" Font-Bold="True" Font-Names="Arial" 
                                    Font-Size="10pt" Text="Ordered Quantity:"></asp:Label>
                            </td>
                            <td style="width: 249px">
                                <asp:Label ID="lblPnlOrderedQty_NS" runat="server" Font-Bold="False" 
                                    Font-Names="Arial" Font-Size="10pt"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblPnlOrderedWgtLabel_NS" runat="server" Font-Bold="True" 
                                    Font-Names="Arial" Font-Size="10pt" Text="Ordered Weight:"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblPnlOrderedWgt_NS" runat="server" Font-Bold="False" 
                                    Font-Names="Arial" Font-Size="10pt"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 142px">
                                <asp:Label ID="Label23_NS" runat="server" Font-Bold="True" Font-Names="Arial" 
                                    Font-Size="10pt" Text="Outstanding Quantity:"></asp:Label>
                            </td>
                            <td style="width: 249px">
                                <asp:Label ID="lblPnlOutstandingQty_NS" runat="server" Font-Bold="False" 
                                    Font-Names="Arial" Font-Size="10pt"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblPnlOutstandingWgtLabel_NS" runat="server" Font-Bold="True" 
                                    Font-Names="Arial" Font-Size="10pt" Text="Outstanding Weight:"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblPnlOutstandingWgt_NS" runat="server" Font-Bold="False" 
                                    Font-Names="Arial" Font-Size="10pt"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="height: 28px;" colspan="2">
                                <asp:TextBox ID="lblPnlProductMessage" runat="server" BorderColor="White" BorderStyle="None" BorderWidth="0px" Font-Bold="True" ForeColor="Red"  ReadOnly="True" Width="392px"></asp:TextBox>
                                </td>
                            <td style="height: 28px">
                                </td>
                            <td style="height: 28px">
                                </td>
                        </tr>
                        <tr>
                            <td style="width: 142px; height: 28px;">
                                <asp:Label ID="lblPnlOutstandingWgtLabel0_NS" runat="server" Font-Bold="True" 
                                    Font-Names="Arial" Font-Size="10pt" Text="Trace Code:"></asp:Label>
                            </td>
                            <td style="width: 249px; height: 28px;">
                                <asp:TextBox ID="txtpnlTraceCode_NS" runat="server" 
                                    onkeyup="CalcQtyFromWeight();"  onkeypress="return HandleKeyPress('Edit', this, 'alphanumeric');"></asp:TextBox>
                                <asp:HiddenField ID="hdnPnlTraceCodeId" runat="server" />
                            </td>
                            <td style="height: 28px">
                                <asp:Label ID="lblPnlSerialNum" runat="server" Font-Bold="True" 
                                    Font-Names="Arial" Font-Size="10pt" Text="Serial Number:"></asp:Label>
                            </td>
                            <td style="height: 28px">
                                <asp:TextBox ID="txtPnlSerialNum" runat="server" onkeyup="CalcWeightFromQty();"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" style="height: 28px;">
                                <asp:TextBox ID="lblPnlTraceMessage" runat="server" BorderColor="White" 
                                    BorderStyle="None" BorderWidth="0px" Font-Bold="True" ForeColor="Red" 
                                     ReadOnly="True" Width="392px"></asp:TextBox>
                            </td>
                            <td colspan="2" style="height: 28px">
                                <asp:TextBox ID="lblPnlSerialMessage" runat="server" Font-Bold="True" 
                                    ForeColor="Red"  ReadOnly="True" 
                                    Width="392px" BorderColor="White" BorderStyle="None" BorderWidth="0px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td >
                                <asp:Label ID="Label26_NS" runat="server" Font-Bold="True" Font-Names="Arial" 
                                    Font-Size="10pt" Text="Fulfilled Quantity:"></asp:Label>
                                    
                            </td>
                            <td>
                                <asp:TextBox ID="txtPnlQtyToPick" runat="server" onkeyup="CalcWeightFromQty();"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Label ID="lblPnlWgtToPickLabel_NS" runat="server" Font-Bold="True" 
                                    Font-Names="Arial" Font-Size="10pt" Text="Fulfilled Weight:"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtPnlWgtToPick" runat="server" onkeyup="CalcQtyFromWeight();" 
                                     onkeypress="return HandleKeyPress('Edit', this, 'alphanumeric');"></asp:TextBox>
                            </td>
                        </tr>
                        <tr><td></td><td></td><td></td><td>&nbsp;</td></tr>
                        <tr>
                            <td style="width: 142px">
                                <asp:Label ID="Label27_NS" runat="server" Font-Bold="True" Font-Names="Arial" 
                                    Font-Size="10pt" Text="Location:"></asp:Label>
                            </td>
                            <td style="width: 249px">
                                <span style="font-size: 11pt"><strong>
                                <asp:DropDownList ID="ddlLocationEdit" runat="server" Width="256px">
                                </asp:DropDownList>
                                </strong></span></td>
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
                                    ImageUrl="~/App_Themes/Billing/Buttons/OK.gif" CausesValidation="False" />
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

   
    <asp:HiddenField ID="lblPerUnitWeight" runat="server" />

   
    <asp:HiddenField ID="hidCalcWgtFromQty" runat="server" />

   
    
                <asp:HiddenField ID="hdnTotalPrice" runat="server" />

   
    
                <asp:HiddenField ID="hdnUnitOfSale" runat="server" />

   
    
                <asp:HiddenField ID="hdnPCOnly" runat="server" />

   
    
                <asp:HiddenField ID="hdnTotalVAT" runat="server" />

   
    
                <br />
                <asp:HiddenField ID="hdnProductValue" runat="server" />

   
    
                <asp:Panel ID="plnAddItem" runat="server" backcolor="White" borderstyle="Solid" 
                    borderwidth="1px" height="390px" width="900px" style="display:">
                    
                    <div style="margin-left:10px; height: 391px;">
                    
                    <table style="width: 830px">
                        <tr>
                            <td style="width: 18px">
                                &nbsp;</td>
                            <td style="width: 142px">
                                &nbsp;</td>
                            <td colspan="2" style="text-align: center;">
                                <asp:Label ID="Label36_NS" runat="server" Font-Bold="True" Font-Names="Arial" 
                                    Font-Overline="False" Font-Size="16pt" 
                                    Text="Enter the New Batch for Order Item"></asp:Label>
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
                                <asp:Label ID="Label29_NS" runat="server" Font-Bold="True" 
                                    Font-Names="Arial" Font-Size="10pt" Text="Product Name:"></asp:Label>
                            </td>
                            <td style="width: 249px">
                                <asp:Label ID="lblPnlProductNameEdit" runat="server" Font-Bold="False" Font-Names="Arial" 
                                    Font-Size="10pt"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label30_NS" runat="server" Font-Bold="True" 
                                    Font-Names="Arial" Font-Size="10pt" Visible="False" Text="Product Code:"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblPnlProductCodeEdit_NS" runat="server" Font-Bold="False" 
                                    Font-Names="Arial" Font-Size="10pt" Visible="False"></asp:Label>
                                &nbsp;<asp:HiddenField ID="hdnPnlAddProductId" runat="server" />
                            </td>
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
                                <asp:Label ID="lblPnlOrderedWgtLabelEdit_NS" runat="server" Font-Bold="True" 
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
                                <asp:Label ID="Label32_NS" runat="server" Font-Bold="True" 
                                    Font-Names="Arial" Font-Size="10pt" Text="Outstanding Quantity:"></asp:Label>
                            </td>
                            <td style="width: 249px">
                                <asp:Label ID="lblPnlOutstandingQtyEdit" runat="server" Font-Bold="False" Font-Names="Arial" 
                                    Font-Size="10pt"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblPnlOutstandingWgtLabelEdit_NS" runat="server" Font-Bold="True" 
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
                                <asp:Label ID="Label35_NS" runat="server" Font-Bold="True" 
                                    Font-Names="Arial" Font-Size="10pt" Text="Delivered Quantity:"></asp:Label>
                            </td>
                            <td style="width: 249px">
                                <asp:Label ID="lblPnlDeliveredQtyEdit_NS" runat="server" Font-Bold="False" 
                                    Font-Names="Arial" Font-Size="10pt"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblPnlDeliveredWgtLabelEdit_NS" runat="server" Font-Bold="True" 
                                    Font-Names="Arial" Font-Size="10pt" Text="Delivered Weight:"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblPnlDeliveredWgtEdit_NS" runat="server" Font-Bold="False" 
                                    Font-Names="Arial" Font-Size="10pt"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 18px">
                                &nbsp;</td>
                            <td colspan="2">
                                <asp:TextBox ID="lblPnlAddProductMessage" runat="server" BorderColor="White" BorderStyle="None" BorderWidth="0px" Font-Bold="True" ForeColor="Red" ReadOnly="True" Width="392px"></asp:TextBox>
                            </td>
                            <td>
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>
                        </tr>
                        <tr>
                            <td style="width: 18px; height: 59px;">
                                </td>
                            <td style="width: 142px; height: 59px;">
                                <asp:Label ID="lblPnlOutstandingWgtLabel0_NS0" runat="server" Font-Bold="True" 
                                    Font-Names="Arial" Font-Size="10pt" Text="Trace Code:"></asp:Label>
                            </td>
                            <td style="width: 249px; height: 59px;">
                                <asp:TextBox ID="txtPnlNewBatchTrace" runat="server" 
                                    onkeyup="CalcQtyFromWeightEdit();"  onkeypress="return HandleKeyPress('Add', this, 'alphanumeric');"></asp:TextBox>
                                <asp:HiddenField ID="hdnPnlAddTraceCodeId" runat="server" />
                            </td>
                            <td style="height: 59px">
                                <asp:Label ID="lblPnlNewBatchSerialNum" runat="server" Font-Bold="True" 
                                    Font-Names="Arial" Font-Size="10pt" Text="Serial Number:"></asp:Label>
                            </td>
                            <td style="height: 59px">
                                <asp:TextBox ID="txtPnlNewBatchSerialNum" runat="server" 
                                    onkeyup="CalcWeightFromQty();"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 18px">
                                &nbsp;</td>
                            <td colspan="2">
                                <asp:TextBox ID="lblPnlAddTraceMessage" runat="server" BorderColor="White" 
                                    BorderStyle="None" BorderWidth="0px" Font-Bold="True" ForeColor="Red" 
                                     ReadOnly="True" Width="392px"></asp:TextBox>
                            </td>
                            <td colspan="2">
                                <asp:TextBox ID="lblPnlAddSerialMessage" runat="server" BorderColor="White" 
                                    BorderStyle="None" BorderWidth="0px" Font-Bold="True" ForeColor="Red" 
                                     ReadOnly="True" Width="392px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 18px">
                                &nbsp;</td>
                            <td style="width: 142px">
                                <asp:Label ID="Label34_NS" runat="server" Font-Bold="True" Font-Names="Arial" 
                                    Font-Size="10pt" Text="Quantity fulfilled:"></asp:Label>
                            </td>
                            <td style="width: 249px">
                                <asp:TextBox ID="txtPnlNewBatchQty" runat="server" 
                                    onkeyup="CalcWeightFromQtyEdit();"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Label ID="lblPnlNewBatchWgtLabel_NS" runat="server" Font-Bold="True" 
                                    Font-Names="Arial" Font-Size="10pt" Text="Weight fulfilled:"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtPnlNewBatchWgt" runat="server" 
                                    onkeyup="CalcQtyFromWeightEdit();"  onkeypress="return HandleKeyPress('Add', this, 'alphanumeric');"></asp:TextBox>
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
                                <asp:Label ID="Label27_NS0" runat="server" Font-Bold="True" Font-Names="Arial" 
                                    Font-Size="10pt" Text="Location:"></asp:Label>
                            </td>
                            <td style="width: 249px">
                                <span style="font-size: 11pt"><strong>
                                <asp:DropDownList ID="ddlNewBatchLocationEdit" runat="server" Width="256px">
                                </asp:DropDownList>
                                </strong></span></td>
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
                    DropShadow="True" oncancelscript="HideMsgPopupEdit()" PopupControlID="plnAddItem" 
                    TargetControlID="plnAddItem">
                </ajaxToolkit:ModalPopupExtender>

                 <asp:Panel ID="pnlMsg" runat="server" backcolor="White" borderstyle="Solid" 
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
                <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtenderMsg" runat="server" 
                    BackgroundCssClass="modalBackground" CancelControlID="cmdMsgOK" 
                    DropShadow="True" oncancelscript="HideMsgPopupMsg()" PopupControlID="pnlMsg" 
                    TargetControlID="pnlMsg">
                </ajaxToolkit:ModalPopupExtender>
   
                <asp:Panel ID="pnlMsg3" runat="server" backcolor="White" borderstyle="Solid" 
                    borderwidth="1px" Height="196px" width="900px" style="display:none">
                     <br />
                    <br />
                    <center>
                        <strong><span style="FONT-FAMILY: Arial">&nbsp; </span></strong><strong>
                        <span style="FONT-FAMILY: Arial"></span></strong>
                    </center>
                    <center>
                        <span style="FONT-FAMILY: Arial">
                        <asp:Label ID="LblOkCancelMsg" runat="server" Font-Bold="True" Font-Names="Arial"></asp:Label>
                        </span>&nbsp;<br />
                        <br />
                    </center>
                    <center>
                        <br />
                        <asp:ImageButton ID="cmdMsgOk2" runat="server" 
                            ImageUrl="~/App_Themes/Billing/Buttons/OK.gif" />
                        <asp:ImageButton ID="cmdMsgCancel2" runat="server" AlternateText="Cancel" 
                                    ImageUrl="~/App_Themes/Billing/Buttons/Cancel.gif" />
                    </center>
                    <center>
                    </center>
                    
                </asp:Panel>
                <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender3" runat="server" 
                    BackgroundCssClass="modalBackground" CancelControlID="cmdMsgCancel2" 
                    DropShadow="True" oncancelscript="HideMsgPopupMsg3()" PopupControlID="pnlMsg3" 
                    TargetControlID="pnlMsg3">
                </ajaxToolkit:ModalPopupExtender>
                  <br />
                            <asp:Panel ID="pnlProductAdd" runat="server" BackColor="White"  Height="377px" 
                    BorderColor="#666666" BorderStyle="Solid" BorderWidth="1px" style="display:" >
                    <table style="width: 700px; height: 264px;">
                        <tr>
                            <td colspan="4">
                                &nbsp;&nbsp;&nbsp;
                                <asp:Label ID="Label8" runat="server" Font-Bold="True" Font-Names="Arial" 
                                    Font-Size="16pt" Text="Add Product"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>
                        </tr>
                        <tr>
                            <td style="text-align: right; vertical-align: top">
                                <asp:Label ID="lblText1" runat="server" Font-Bold="True" Font-Names="Arial" 
                                    Font-Size="10pt" Text="Category:"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlCategory" runat="server"  Width="250px" 
                                    TabIndex="106">
                                </asp:DropDownList>
                            </td>
                            <td style="text-align: right">
                                <asp:Label ID="lblText4" runat="server" Font-Bold="True" Font-Names="Arial" 
                                    Font-Size="10pt" Text="Product Code:"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtProductCode" runat="server" TabIndex="100"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: right; vertical-align: top">
                                <asp:Label ID="lblText2" runat="server" Font-Bold="True" Font-Names="Arial" 
                                    Font-Size="10pt" Text="Product:"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlProducts" runat="server"  Width="250px" 
                                    TabIndex="7">
                                </asp:DropDownList>
                            </td>
                            <td style="text-align: right">
                                <asp:Label ID="lblText" runat="server" Font-Bold="True" Font-Names="Arial" 
                                    Font-Size="10pt" Text="Quantity:"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtQuantity" runat="server" TabIndex="101"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>
                            <td style="height: 26px; text-align: right;">
                                <asp:Label ID="lblText0" runat="server" Font-Bold="True" Font-Names="Arial" 
                                    Font-Size="10pt" Font-Underline="False" Text="Weight:"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtWeight" runat="server" TabIndex="102"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                               &nbsp;&nbsp;&nbsp;
                                <span style="font-size: 11pt"><strong>
                                <asp:Label ID="lblOutputWeight0" runat="server" Font-Bold="True" 
                                    Font-Names="Arial" Font-Size="10pt" 
                                    Text="NOTE: This product is tracked by: "></asp:Label>
                                <asp:TextBox ID="lblPnlProdMessage" runat="server" BorderColor="White" 
                                    BorderStyle="None" BorderWidth="0px" Font-Bold="True" ForeColor="Red" 
                                    ReadOnly="True" Width="150px"></asp:TextBox>
                                </strong></span></td>
                            <td>
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>
                        </tr>
                        <tr>
                            <td style="vertical-align: middle; text-align: right">
                                <asp:Label ID="lblText3" runat="server" Font-Bold="True" Font-Names="Arial" 
                                    Font-Size="10pt" Text="Comment:"></asp:Label>
                            </td>
                            <td colspan="3">
                                <asp:TextBox ID="txtComment" runat="server" Width="568px" Font-Names="Arial" 
                                    Font-Size="9pt" Height="70px" TextMode="MultiLine" TabIndex="103"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;</td>
                            <td>
                                <asp:ImageButton ID="cmdCancelAdd" runat="server" 
                                    ImageUrl="~/App_Themes/Billing/Buttons/Cancel.gif" TabIndex="105" />
                            </td>
                            <td>
                                <asp:ImageButton ID="cmdProdAdd" runat="server" 
                                    ImageUrl="~/App_Themes/Billing/Buttons/Add.GIF" TabIndex="104" />
                            </td>
                            <td>
                                &nbsp;</td>
                        </tr>
                    </table>
                     <ajaxToolkit:CascadingDropDown ID="CascadingDropDownAddCategory" runat="server" 
                            Category="Category" LoadingText="Loading Product Categories ..." 
                            PromptText="Select Category .." ServiceMethod="GetProductCategories" 
                            ServicePath="~/ModuleServices.asmx" TargetControlID="ddlCategory">
                        </ajaxToolkit:CascadingDropDown>
                        <ajaxToolkit:CascadingDropDown ID="CascadingDropDownAddProduct" runat="server" 
                            LoadingText="Loading Products ..." ParentControlID="ddlCategory" 
                            PromptText="Select Product .." ServiceMethod="GetProductsInCategory" 
                            ServicePath="~/ModuleServices.asmx" TargetControlID="ddlProducts" Category="Products">
                        </ajaxToolkit:CascadingDropDown>

                </asp:Panel>

                 <ajaxToolkit:ModalPopupExtender ID="modalAddProd" runat="server" 
                    BackgroundCssClass="modalBackground" CancelControlID="cmdCancelAdd" 
                    DropShadow="True" oncancelscript="HideMsgPopupAdd()" PopupControlID="pnlProductAdd" 
                    TargetControlID="pnlProductAdd">
                </ajaxToolkit:ModalPopupExtender> 




                
                <ig:WebExcelExporter ID="VerifyExporter" runat="server">
        </ig:WebExcelExporter>


                </ContentTemplate>
       
        </asp:UpdatePanel>
    
     </asp:Content>

