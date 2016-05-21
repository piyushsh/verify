<%@ Register TagPrefix="igtbl" Namespace="Infragistics.WebUI.UltraWebGrid" Assembly="Infragistics2.WebUI.UltraWebGrid.v7.3, Version=7.3.20073.38, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igcmbo" Namespace="Infragistics.WebUI.WebCombo" Assembly="Infragistics2.WebUI.WebCombo.v7.3, Version=7.3.20073.38, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtxt" Namespace="Infragistics.WebUI.WebDataInput" Assembly="Infragistics2.WebUI.WebDataInput.v7.3, Version=7.3.20073.38, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Control Language="vb" AutoEventWireup="false" CodeFile="SimpleProductSelect.ascx.vb" Inherits="SimpleProductSelect"  %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<style type="text/css">
    .style3
    {
        width: 920px;
    }
    .style4
    {
        width: 910px;
    }
    .style5
    {
        width: 158px;
    }
    .style6
    {
        width: 95px;
    }
</style>
<script src="JScript1.js" type="text/javascript"></script>
<script lang="JavaScript"> 
var typeAheadInfo = {last:0, 
                     currentString:"", 
                     delay:2000,
                     timeout:null, 
                     reset:function() {this.last=0; this.currentString=""}
                    };


		
function typeAhead() {
   if (window.event && !window.event.ctrlKey) {
      var now = new Date();

      if (typeAheadInfo.currentString == "" || now - typeAheadInfo.last < typeAheadInfo.delay) {

         var myEvent = window.event;
         var selectElement = myEvent.srcElement;
         var keyCode = myEvent.keyCode;

		 // The NumPad returns slightly differant keyCodes then the numbers on the type of the keyboard.
		 // If we subtract 48 it will return the correct keyCode.
		 if (keyCode >= 96 && keyCode <=105) {
			keyCode = keyCode - 48;
		 }      
		 
		 // terminate processing on a Carriage Return or Tab
		 if (keyCode == 13 || keyCode == 9) {
			   __doPostBack('','');
//			   __doPostBack('ProductSelect1$cboProduct','');
               myEvent.cancelBubble = true;
               myEvent.returnValue = false;

               return false;   
		 }
		 
		 // only handle alpha characters
         if (keyCode < 30) {
			return true;	
		 }
		 
         var newChar =  String.fromCharCode(keyCode).toUpperCase();
         typeAheadInfo.currentString += newChar;

         var selectOptions = selectElement.options;
         var txt, nearest;
         for (var i = 0; i < selectOptions.length; i++) {
            // change this from .text to .value to use the value of the item instead of the visual text if desired
            txt = selectOptions[i].text.toUpperCase();
            nearest = (typeAheadInfo.currentString > 
                       txt.substr(0, typeAheadInfo.currentString.length)) ? i : nearest;

            if (txt.indexOf(typeAheadInfo.currentString) == 0) {
               clearTimeout(typeAheadInfo.timeout);
               typeAheadInfo.last = now;
               typeAheadInfo.timeout = setTimeout("typeAheadInfo.reset()", typeAheadInfo.delay);
               selectElement.selectedIndex = i;

               myEvent.cancelBubble = true;
               myEvent.returnValue = false;

               return false;   
            }            
         }
         if (nearest != null) {
            selectElement.selectedIndex = nearest;
         }
      } else {
         clearTimeout(typeAheadInfo.timeout);
      }
      typeAheadInfo.reset();
   }
   return true;
}

// function to cause a PostBack when the Product Selector loses focus
function lostFocus() {
	__doPostBack('ProductSelect1$cboProduct','');
}

// function to clear the Hidden field when the control is clicked
//function clearHiddenField() {
//	document.getElementById("ProductSelect1_hdnValue").value=-1;
//}


// function to handle client-side editing of WebGrid 
function ProductSelectEditHandler(gridName, cellId, keyCode) {

	var cell = igtbl_getCellById(cellId);
	var row = igtbl_getRowById(cellID);		
	if (EditHandler(gridName, cellId, keyCode)) {
		return True;
	}
	/*
	else{
		var WebTab1 = igtab_getTabById("UltraWebTab1");
		var UnitOfSaleLabel  = WebTab1.findControl("ProductSelect1:lblQtyOrWeight");
		var UnitOfSale;
		if (UnitOfSaleLabel.innerHTML == 'This product is measured by quantity'){
			UnitOfSale = 1;
		}
		else{
			UnitOfSale = 0;
		}
		alert(cell.getValue());
		
	}
	
	*/
}
</script>

<table class="style3">
    <tr>
        <td>
            &nbsp;</td>
        <td colspan="2">
            &nbsp;</td>
    </tr>
    <tr>
        <td>
            &nbsp;</td>
        <td colspan="2">
            <table class="style4">
                <tr>
                    <td class="style6">
                        <asp:label id="Label2" runat="server" Width="72px" Height="24px" Font-Bold="True" Font-Size="X-Small"
					Font-Names="Arial" ForeColor="Navy">Category:</asp:label>
                    </td>
                    <td>
                        <asp:dropdownlist id="cboProdCategory" tabIndex="1" runat="server" 
                    Width="150px" Height="24px" AutoPostBack="True"></asp:dropdownlist>
                    </td>
                    <td class="style5">
                        <asp:label id="Label3" runat="server" Width="115px" Height="24px" 
                    Font-Bold="True" Font-Size="X-Small"
					Font-Names="Arial" ForeColor="Navy">Product Code:</asp:label>
                    </td>
                    <td>
                        <asp:textbox id="txtProductCode" tabIndex="2" runat="server" Width="144px" AutoPostBack="True"></asp:textbox>
                    </td>
                </tr>
                <tr>
                    <td class="style6">
                        <asp:label id="Label4" runat="server" Width="64px" Height="16px" Font-Bold="True" Font-Size="X-Small"
					Font-Names="Arial" ForeColor="Navy">Product:</asp:label>
                    </td>
                    <td colspan="3">
                        <asp:dropdownlist id="cboProduct" tabIndex="3" runat="server" Width="504px" Height="24px" AutoPostBack="True"></asp:dropdownlist>
                    </td>
                </tr>
                <tr>
                    <td class="style6">
                        &nbsp;</td>
                    <td>
                        &nbsp;</td>
                    <td class="style5">
                        &nbsp;</td>
                    <td>
                        &nbsp;</td>
                </tr>
            </table>
        </td>
    </tr>
    
    <tr>
        <td>
            &nbsp;</td>
        <td>
           </td>
        <td>
              <INPUT id="hdnUnitOfSale" type="hidden" size="4" name="hdnUnitOfSale" runat="server">
      </td>
    </tr>
   
</table>

