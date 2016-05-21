<%@ Register TagPrefix="igtbl" Namespace="Infragistics.WebUI.UltraWebGrid" Assembly="Infragistics2.WebUI.UltraWebGrid.v7.3, Version=7.3.20073.38, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igcmbo" Namespace="Infragistics.WebUI.WebCombo" Assembly="Infragistics2.WebUI.WebCombo.v7.3, Version=7.3.20073.38, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtxt" Namespace="Infragistics.WebUI.WebDataInput" Assembly="Infragistics2.WebUI.WebDataInput.v7.3, Version=7.3.20073.38, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>


<%@ Register Assembly="Infragistics4.Web.v13.2, Version=13.2.20132.2028, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" namespace="Infragistics.Web.UI.DisplayControls" tagprefix="ig" %>
<%@ Register Assembly="Infragistics4.Web.v13.2, Version=13.2.20132.2028, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" namespace="Infragistics.Web.UI.EditorControls" tagprefix="ig" %>
<%@ Register Assembly="Infragistics4.Web.v13.2, Version=13.2.20132.2028, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" namespace="Infragistics.Web.UI.GridControls" tagprefix="ig" %>



<%@ Control Language="vb" AutoEventWireup="false" CodeFile="ProductSelect.ascx.vb" Inherits="ProductSelect"  %>
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
        width: 132px;
    }
    .style6
    {
        width: 95px;
    }
    #Table2
    {
        width: 852px;
    }
    .style7
    {
        width: 380px;
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

<div style="border: 1px solid #808080; margin-left: 10px; width: 900px;">

<table class="style3">
    <tr>
        <td colspan="2">
            <table class="style4">
                <tr>
                    <td class="style6">
                        <asp:label id="Label2" runat="server" Width="72px" Height="24px" 
                            Font-Bold="True" Font-Size="10pt"
					Font-Names="Arial" ForeColor="Navy">Category:</asp:label>
                    </td>
                    <td>
                        <asp:dropdownlist id="cboProdCategory" tabIndex="1" runat="server" 
                    Width="300px" Height="24px" AutoPostBack="True"></asp:dropdownlist>
                    </td>
                    <td class="style5">
                        <asp:label id="Label3" runat="server" Width="150px" Height="24px" 
                    Font-Bold="True" Font-Size="10pt"
					Font-Names="Arial" ForeColor="Navy">Product Code:</asp:label>
                    </td>
                    <td>
                        <asp:textbox id="txtProductCode" tabIndex="2" runat="server" Width="181px" 
                            AutoPostBack="True"></asp:textbox>
                    </td>
                </tr>
                <tr>
                    <td class="style6">
                        <asp:label id="Label4" runat="server" Width="64px" Height="16px" 
                            Font-Bold="True" Font-Size="10pt"
					Font-Names="Arial" ForeColor="Navy">Product:</asp:label>
                    </td>
                    <td colspan="3">
                        <asp:dropdownlist id="cboProduct" tabIndex="3" runat="server" Width="504px" Height="24px" AutoPostBack="True"></asp:dropdownlist>
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:checkbox id="chkShowBatches" tabIndex="5" runat="server" Width="192px" 
                            Height="24px" AutoPostBack="True"
					Text=" Show all batches" Font-Bold="True" Font-Names="Arial" Font-Size="10pt"></asp:checkbox>
                    </td>
                </tr>
                </table>
        </td>
    </tr>
    <tr>
        <td colspan="2">
				<TABLE id="Table2" cellSpacing="1" cellPadding="1" >
					<TR>
						<TD height="40" class="style7">
                            <asp:Label ID="lblStockDetails" runat="server" Font-Bold="True" 
                                Font-Underline="False" Text="Stock Summary (for this product)" 
                                Font-Names="Arial" Font-Size="11pt" Width="350px"></asp:Label>
                        </TD>
						<TD height="4" rowspan="2" 
                            style="vertical-align: bottom; text-align: left; font-family: Arial, Helvetica, sans-serif;">
                            <asp:label id="lblUsesTrace" runat="server" Width="389px" Height="24px" 
                                Font-Bold="True" ForeColor="Red" Font-Names="Arial" Font-Size="10pt"> ** This product is trace code controlled</asp:label>
                            <br />

                            <asp:label id="lblQtyOrWeight" runat="server" Width="395px" Height="24px" 
                                ForeColor="Blue" Font-Names="Arial" Font-Size="10pt" Font-Bold="True"> ** This product is measured by quantity</asp:label></TD>
					</TR>
					<TR>
						<TD class="style7"><igtbl:ultrawebgrid id="grdProduct" tabIndex="3" runat="server" 
                                Width="325px" Height="60px">
								<Rows>
									<igtbl:UltraGridRow Height="30px">
										<Cells>
											<igtbl:UltraGridCell Key="" Text="abc"></igtbl:UltraGridCell>
											<igtbl:UltraGridCell Key="" Text="abc"></igtbl:UltraGridCell>
											<igtbl:UltraGridCell Key="" Text="abc"></igtbl:UltraGridCell>
										</Cells>
									</igtbl:UltraGridRow>
								</Rows>
								<DisplayLayout AutoGenerateColumns="False" RowHeightDefault="20px" Version="4.00" ScrollBar="Never"
									BorderCollapseDefault="Separate" RowSelectorsDefault="No" Name="xctl0xgrdProduct">
									<AddNewBox>
										<Style BorderWidth="1px" BorderStyle="Solid" BackColor="LightGray">


</Style>
<BoxStyle BackColor="LightGray" BorderWidth="1px" BorderStyle="Solid"></BoxStyle>
									</AddNewBox>
									<Pager>
										<Style BorderWidth="1px" BorderStyle="Solid" BackColor="LightGray">



</Style>
<PagerStyle BackColor="LightGray" BorderWidth="1px" BorderStyle="Solid"></PagerStyle>
									</Pager>
									<HeaderStyleDefault BorderStyle="Solid" BackColor="LightGray" 
                                        font-names="Arial" font-size="9pt" height="25px">
										<BorderDetails ColorTop="White" WidthLeft="1px" WidthTop="1px" ColorLeft="White"></BorderDetails>
									</HeaderStyleDefault>
									<FrameStyle Width="325px" BorderWidth="1px" Font-Size="8pt" 
                                        Font-Names="Verdana" BorderStyle="Solid"
										Height="60px"></FrameStyle>
									<FooterStyleDefault BorderWidth="1px" BorderStyle="Solid" BackColor="LightGray">
										<BorderDetails ColorTop="White" WidthLeft="1px" WidthTop="1px" ColorLeft="White"></BorderDetails>
									</FooterStyleDefault>
									<EditCellStyleDefault BorderWidth="0px" BorderStyle="None"></EditCellStyleDefault>
									<RowStyleDefault BorderWidth="1px" BorderColor="Gray" BorderStyle="Solid" 
                                        font-names="Arial" font-overline="False" font-size="9pt">
										<Padding Left="3px"></Padding>
										<BorderDetails WidthLeft="0px" WidthTop="0px"></BorderDetails>
									</RowStyleDefault>

<ActivationObject BorderColor="" BorderWidth=""></ActivationObject>
								</DisplayLayout>
								<Bands>
									<igtbl:UltraGridBand>
										<Columns>
											<igtbl:UltraGridColumn HeaderText="Total Stock" Key="" BaseColumnName="">
												<Footer Key=""></Footer>
												<Header Key="" Caption="Total Stock"></Header>
											</igtbl:UltraGridColumn>
											<igtbl:UltraGridColumn HeaderText="Allocated" Key="" BaseColumnName="">
												<Footer Key="">
<RowLayoutColumnInfo OriginX="1"></RowLayoutColumnInfo>
                                                </Footer>
												<Header Key="" Caption="Allocated">
<RowLayoutColumnInfo OriginX="1"></RowLayoutColumnInfo>
                                                </Header>
											</igtbl:UltraGridColumn>
											<igtbl:UltraGridColumn HeaderText="Free" Key="" BaseColumnName="">
												<Footer Key="">
<RowLayoutColumnInfo OriginX="2"></RowLayoutColumnInfo>
                                                </Footer>
												<Header Key="" Caption="Free">
<RowLayoutColumnInfo OriginX="2"></RowLayoutColumnInfo>
                                                </Header>
											</igtbl:UltraGridColumn>
										</Columns>

<AddNewRow Visible="NotSet" View="NotSet"></AddNewRow>
									</igtbl:UltraGridBand>
								</Bands>
							</igtbl:ultrawebgrid></TD>
					</TR>
				</TABLE>
			</td>
    </tr>
    <tr>
        <td style="font-family: Arial, Helvetica, sans-serif; font-size: 6px">
           </td>
        <td style="font-family: Arial, Helvetica, sans-serif; font-size: 6px">
              <INPUT id="hdnUnitOfSale" type="hidden" size="4" name="hdnUnitOfSale" runat="server">
      </td>
    </tr>
    <tr>
        <td colspan="2">
            <igtbl:ultrawebgrid id="grdTraceCodes" tabIndex="4" runat="server" Width="872px" Height="156px">
					<DisplayLayout AllowSortingDefault="Yes" RowHeightDefault="20px" Version="4.00" HeaderClickActionDefault="SortSingle"
						BorderCollapseDefault="Separate" RowSelectorsDefault="No" Name="xctl0xgrdTraceCodes" ClientSideEvents-InitializeLayoutHandler="InitializeGridLayoutHandler">
						<AddNewBox>
							<Style BorderWidth="1px" BorderStyle="Solid" BackColor="LightGray">


</Style>
<BoxStyle BackColor="LightGray" BorderWidth="1px" BorderStyle="Solid"></BoxStyle>
						</AddNewBox>
						<Pager>
							<Style BorderWidth="1px" BorderStyle="Solid" BackColor="LightGray">



</Style>
<PagerStyle BackColor="LightGray" BorderWidth="1px" BorderStyle="Solid"></PagerStyle>
						</Pager>
						<HeaderStyleDefault Font-Bold="True" BorderStyle="Solid" ForeColor="White" 
                            BackColor="#B28700" height="30px" font-names="Arial" font-overline="False" 
                            font-size="9pt">
							<BorderDetails ColorTop="White" WidthLeft="1px" WidthTop="1px" ColorLeft="White"></BorderDetails>
						</HeaderStyleDefault>
						<FrameStyle Width="872px" BorderWidth="1px" Font-Size="8pt" Font-Names="Verdana" BorderStyle="Solid"
							Height="156px"></FrameStyle>
						<FooterStyleDefault BorderWidth="1px" BorderStyle="Solid" BackColor="LightGray">
							<BorderDetails ColorTop="White" WidthLeft="1px" WidthTop="1px" ColorLeft="White"></BorderDetails>
						</FooterStyleDefault>
						<RowAlternateStyleDefault Font-Names="Arial" Font-Size="9pt" 
                            Font-Strikeout="False">
                        </RowAlternateStyleDefault>
						<ClientSideEvents MouseDownHandler="grdTraceCodes_MouseDownHandler" EditKeyDownHandler="EditHandler"></ClientSideEvents>
						<EditCellStyleDefault BorderWidth="0px" BorderStyle="None"></EditCellStyleDefault>
						<SelectedRowStyleDefault BackColor="#FF8080"></SelectedRowStyleDefault>
						<RowStyleDefault BorderWidth="1px" BorderColor="Gray" BorderStyle="Solid" 
                            font-names="Arial" font-overline="False" font-size="9pt">
							<Padding Left="3px"></Padding>
							<BorderDetails WidthLeft="0px" WidthTop="0px"></BorderDetails>
						</RowStyleDefault>

<ActivationObject BorderColor="" BorderWidth=""></ActivationObject>
					</DisplayLayout>
					<Bands>
						<igtbl:UltraGridBand>
							<Columns>
								<igtbl:UltraGridColumn  Key="TraceCode" EditorControlID=""
									Width="80px" Format="" Formula="" BaseColumnName="" FooterText="">
									<Footer Formula="" Title="" Key="TraceCode" Caption=""></Footer>
									<Header Title="" Key="TraceCode" Caption="Trace Code"></Header>
								</igtbl:UltraGridColumn>
								<igtbl:UltraGridColumn  Key="InStock" EditorControlID="" Width="80px"
									Format="" Formula="" BaseColumnName="" FooterText="">
									<Footer Formula="" Title="" Key="InStock" Caption="">
                                        <RowLayoutColumnInfo OriginX="1"></RowLayoutColumnInfo>
                                    </Footer>
									<Header Title="" Key="InStock" Caption="Free Stock">
<RowLayoutColumnInfo OriginX="1"></RowLayoutColumnInfo>
                                    </Header>
								</igtbl:UltraGridColumn>
								<igtbl:UltraGridColumn  Key="Qty" EditorControlID=""
									Format="" Formula="" BaseColumnName="" FooterText="" AllowUpdate="Yes" Width="50px">
									<Footer Formula="" Title="" Key="Qty" Caption="">
<RowLayoutColumnInfo OriginX="2"></RowLayoutColumnInfo>
                                    </Footer>
									<CellStyle BackColor="#CCFF99">
                                    </CellStyle>
									<Header Title="" Key="Qty" Caption="Qty">
<RowLayoutColumnInfo OriginX="2"></RowLayoutColumnInfo>
                                    </Header>
								</igtbl:UltraGridColumn>
								<igtbl:UltraGridColumn  Key="Wgt" EditorControlID="" Width="50px"
									Format="" Formula="" BaseColumnName="" FooterText="" AllowUpdate="Yes">
									<Footer Formula="" Title="" Key="Wgt" Caption="">
<RowLayoutColumnInfo OriginX="3"></RowLayoutColumnInfo>
                                    </Footer>
									<CellStyle BackColor="#CCFF99">
                                    </CellStyle>
									<Header Title="" Key="Wgt" Caption="Wgt">
<RowLayoutColumnInfo OriginX="3"></RowLayoutColumnInfo>
                                    </Header>
								</igtbl:UltraGridColumn>
								<igtbl:UltraGridColumn  Key="UnitPrice" EditorControlID="" Width="80px"
									Format="" Formula="" BaseColumnName="" AllowUpdate="Yes" FooterText="">
									<Footer Formula="" Title="" Key="UnitPrice" Caption="">
<RowLayoutColumnInfo OriginX="4"></RowLayoutColumnInfo>
                                    </Footer>
									<CellStyle BackColor="#CCFF99">
                                    </CellStyle>
									<Header Title="" Key="UnitPrice" Caption="Unit Price">
<RowLayoutColumnInfo OriginX="4"></RowLayoutColumnInfo>
                                    </Header>
								</igtbl:UltraGridColumn>
								<igtbl:UltraGridColumn  Key="Location" EditorControlID="" Width="80px"
									Format="" Formula="" BaseColumnName="" FooterText="">
									<Footer Formula="" Title="" Key="Location" Caption="">
<RowLayoutColumnInfo OriginX="5"></RowLayoutColumnInfo>
                                    </Footer>
									<Header Title="" Key="Location" Caption="Location">
<RowLayoutColumnInfo OriginX="5"></RowLayoutColumnInfo>
                                    </Header>
								</igtbl:UltraGridColumn>
								<igtbl:UltraGridColumn Key="NetPrice" EditorControlID=""
									Width="80px" Format="" Formula="" BaseColumnName="" FooterText="">
									<Footer Formula="" Title="" Key="NetPrice" Caption="">
<RowLayoutColumnInfo OriginX="6"></RowLayoutColumnInfo>
                                    </Footer>
									<Header Title="" Key="NetPrice" Caption="Net Price">
<RowLayoutColumnInfo OriginX="6"></RowLayoutColumnInfo>
                                    </Header>
								</igtbl:UltraGridColumn>
								<igtbl:UltraGridColumn  Key="VAT" EditorControlID="" Width="50px"
									Format="" Formula="" BaseColumnName="" FooterText="">
									<Footer Formula="" Title="" Key="VAT" Caption="">
<RowLayoutColumnInfo OriginX="7"></RowLayoutColumnInfo>
                                    </Footer>
									<Header Title="" Key="VAT" Caption="Tax %">
<RowLayoutColumnInfo OriginX="7"></RowLayoutColumnInfo>
                                    </Header>
								</igtbl:UltraGridColumn>
								<igtbl:UltraGridColumn  Key="TotalPrice" 
                                    EditorControlID="" Width="80px"
									Format="" Formula="" BaseColumnName="" FooterText="">
									<Footer Formula="" Title="" Key="TotalPrice" Caption="">
<RowLayoutColumnInfo OriginX="8"></RowLayoutColumnInfo>
                                    </Footer>
									<Header Title="" Key="TotalPrice" Caption="Total Price">
<RowLayoutColumnInfo OriginX="8"></RowLayoutColumnInfo>
                                    </Header>
								</igtbl:UltraGridColumn>
								<igtbl:UltraGridColumn  Key="SerialNum" Formula="" 
                                    BaseColumnName="SerialNum">
									<Footer Formula="" Key="SerialNum">
<RowLayoutColumnInfo OriginX="9"></RowLayoutColumnInfo>
                                    </Footer>
									<Header Key="SerialNum" Caption="Serial Number">
<RowLayoutColumnInfo OriginX="9"></RowLayoutColumnInfo>
                                    </Header>
								</igtbl:UltraGridColumn>
								<igtbl:UltraGridColumn  Key="Barcode" Formula="" 
                                    BaseColumnName="Barcode">
									<Footer Formula="" Key="Barcode">
<RowLayoutColumnInfo OriginX="10"></RowLayoutColumnInfo>
                                    </Footer>
									<Header Key="Barcode" Caption="Barcode">
<RowLayoutColumnInfo OriginX="10"></RowLayoutColumnInfo>
                                    </Header>
								</igtbl:UltraGridColumn>
							    <igtbl:UltraGridColumn Key="UseByOrSellBy" EditorControlID="" FooterText="" 
                                    Format="dd/MM/yyyy">
                                    <Header Caption="UseBy/BestBefore" Title="">
                                        <RowLayoutColumnInfo OriginX="11" />
                                    </Header>
                                    <Footer Caption="" Title="">
                                        <RowLayoutColumnInfo OriginX="11" />
                                    </Footer>
                                </igtbl:UltraGridColumn>
                                <igtbl:UltraGridColumn Key="DateRec" EditorControlID="" FooterText="" Format="" 
                                    Width="80px">
                                    <Header Caption="Date Rec." Title="">
                                        <RowLayoutColumnInfo OriginX="12" />
                                    </Header>
                                    <Footer Caption="" Title="">
                                        <RowLayoutColumnInfo OriginX="12" />
                                    </Footer>
                                </igtbl:UltraGridColumn>
							</Columns>

<AddNewRow Visible="NotSet" View="NotSet"></AddNewRow>
						</igtbl:UltraGridBand>
					</Bands>
				</igtbl:ultrawebgrid>
            <br />

            <ig:WebDataGrid ID="wdgDetails" runat="server" 
        AltItemCssClass="VerifyGrid_Report_AlternateRow" AutoGenerateColumns="False" 
        BackColor="White" CssClass="VerifyGrid_Report_Frame" EnableTheming="True" 
        Font-Names="Arial" FooterCaptionCssClass="VerifyGrid_Report_Footer" 
        HeaderCaptionCssClass="VerifyGrid_Report_HeaderGold_10pt" Height="200px" 
        ItemCssClass="VerifyGrid_Report_Row" Width="703px" EnableAjax="False">
        <Columns>
				<ig:BoundDataField   DataFieldName="" Key="TraceCode" width="80px" >
					<Header Text ="Trace Code:" />
				</ig:BoundDataField>
				<ig:BoundDataField  DataFieldName="" Key="InStock" width="80px" >
                       <Header Text="InStock:"  />
                </ig:BoundDataField>
				<ig:BoundDataField  DataFieldName="" Key="Qty"  Width="50px">
					<Header Text="Qty:" />
                </ig:BoundDataField>
				<ig:BoundDataField  DataFieldName="" Key="Wgt"  Width="50px" >
					<Header Text="Wgt:" />
                </ig:BoundDataField>
				<ig:BoundDataField  DataFieldName="" Key="UnitPrice" Width="80px">
					<Header Text="UnitPrice:" />
                </ig:BoundDataField>
				<ig:BoundDataField  DataFieldName="" Key="Location"  Width="80px">
					<Header Text="Location:" />
                </ig:BoundDataField>
				<ig:BoundDataField DataFieldName="" Key="NetPrice" Width="80px" >
					<Header Text="" />
                </ig:BoundDataField>
				<ig:BoundDataField  DataFieldName="" Key="VAT" Width="50px">
					<Header Text="VAT:" />
                </ig:BoundDataField>
				<ig:BoundDataField  DataFieldName="" Key="TotalPrice"  Width="80px">
					<Header Text="TotalPrice" />
                </ig:BoundDataField>
				<ig:BoundDataField  DataFieldName="SerialNum" Key="SerialNum" >
					<Header Text="SerialNum" />
                </ig:BoundDataField>
				<ig:BoundDataField  DataFieldName="Barcode" Key="Barcode"  >
					<Header Text="Barcode" />
                </ig:BoundDataField>
				<ig:BoundDataField  DataFieldName= "" Key="UseByOrSellBy" >
                    <Header Text="UseBy/BestBefore" />
                </ig:BoundDataField>
                <ig:BoundDataField DataFieldName= "" Key="DateRec"  Width="80px">
                    <Header Text="Date Rec." />
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
                <ig:ColumnSummaryInfo ColumnKey="TraceCode">
                    <Summaries>
                        <ig:Summary SummaryType="Count" />
                    </Summaries>
                </ig:ColumnSummaryInfo>
                
                
            </ColumnSummaries>
            <SummaryButton HoverImageUrl="~/App_Themes/Buttons/Add_Plus_Small.gif" />
            <ColumnSettings>
                <ig:SummaryRowSetting ColumnKey="TraceCode">
                    <SummarySettings>
                        <ig:SummarySetting SummaryType="Count" />
                        <ig:SummarySetting />
                    </SummarySettings>
                </ig:SummaryRowSetting>
                
            </ColumnSettings>
        </ig:SummaryRow>
        <ig:Filtering Enabled = "true" FilterButtonCssClass="Filter_LAlign">
            <ColumnSettings>
                <ig:ColumnFilteringSetting ColumnKey="TraceCode" Enabled="true" />
            </ColumnSettings>
            <ColumnFilters>
                <ig:ColumnFilter ColumnKey="TraceCode">
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

            <br />
        </td>
    </tr>
    <tr>
        <td colspan="2">
            &nbsp;</td>
    </tr>
</table>

</div>

