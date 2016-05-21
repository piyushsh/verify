<%@ Page Title="" Language="VB" MasterPageFile="~/Quotes/FormsMaster.master" AutoEventWireup="false" CodeFile="QuoteProducts_Waste.aspx.vb" Inherits="Quotes_QuoteProducts_Waste" EnableEventValidation="false" %>

<%@ Register assembly="Infragistics2.WebUI.UltraWebGrid.v7.3, Version=7.3.20073.38, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" namespace="Infragistics.WebUI.UltraWebGrid" tagprefix="igtbl1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="DetailsMaster_ContentPlaceHolder" Runat="Server">
    <script type="text/javascript">

        function SaveClassification() {
            var myrow = igtbl_getActiveRow("<%=uwgQuoteItems.ClientID%>");
            if (myrow == null) {
                alert('You must select an item before you can save its Classification!');
                return false
            }

        }

        function DeleteItem() {
            var myrow = igtbl_getActiveRow("<%=uwgQuoteItems.ClientID%>");
            if (myrow == null) {
                alert('You must select an item to Remove!');
                return false
            }
            var answer = confirm("Are you sure you want to Delete this item?\n\n Click Ok if you wish to continue.")
            if (answer)
                return true;
            else
                return false;

        }

      function AddProductClose(sender, e) {

          // now postback
          __doPostBack(sender, e);

      }


      function ShowProductAddPopup() {

          var modal = $find("<%=modalAddProd.BehaviorID%>");
          modal.show();
          return false;
      }


     
      function HideMsgPopupAdd() {
          var modal = $find("<%=modalAddProd.BehaviorID%>");
          modal.hide();
      }


      function HideMsgPopup() {
          var modal = $find("<%=ModalPopupExtenderMessage.BehaviorID%>");
          modal.hide();
      }




      function ShowAddClose(sender, e) {

          var ddlProd = document.getElementById("<%=ddlProducts.ClientID%>");

          if (ddlProd.options[ddlProd.selectedIndex].innerText == "Select Product ..") {
              alert('You must select a Product!');
              return false;
          }


          // Check for valid qty or weight        
          var UoS = document.getElementById("<%=lblPnlProdMessage.ClientID%>");
        
              var txtAmount = document.getElementById("<%=txtWeight.ClientID%>");
              if (txtAmount.value == "") {
                  alert('You must enter an Amount');
                  return false;
              }
                    
          __doPostBack(sender, e);
      }  


     
      function HandleProductOnAddPanel(src, destMessage) {
          var prod = document.getElementById(src);
          if (prod.options[prod.selectedIndex].innerText == "Select Product ..") {
              destMessage.value = "";
          } else {
              var myindex = prod.selectedIndex
              var SelValue = prod.options[myindex].value
              // call server side method
              PageMethods.GetUnitOfSaleTextForProdId(SelValue, CallSuccess, CallFailed, destMessage);
          }
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


    
    </script>


    <table style="width: 690px">
        <tr>
            <td>
                                <span style="font-size: 11pt"><strong>
                                <asp:Label ID="Label7" runat="server" font-bold="True" font-names="Arial" 
                                    font-size="11pt" forecolor="#B38700" 
                    text="Products to Quote" width="205px"></asp:Label>
                                </strong></span>
                            </td>
                 </tr>
        
        <tr>
            <td>
                <table style="border-right: darkgray 1px solid; border-top: darkgray 1px solid;
                border-left: darkgray 1px solid; border-bottom: darkgray 1px solid; width: 650px">
                    <tr>
                        <td>
                                <span style="font-size: 11pt"><strong>
                                            <b>
                                            <asp:Label ID="lblBatchCaption" runat="server" Font-Bold="True" 
                                                Font-Names="Arial" Font-Size="10pt" Width="154px">Current Total:</asp:Label>
                                            </b>
                                </strong></span>
                            </td>
                        <td>
                                <span style="font-size: 11pt"><strong>
                                            <b>
                                            <asp:Label ID="lblBatchCaption0" runat="server" Font-Bold="True" 
                                                Font-Names="Arial" Font-Size="10pt" Width="170px">Current Tax Total:</asp:Label>
                                            </b>
                                </strong></span>
                            </td>
                        <td>
                                <span style="font-size: 11pt"><strong>
                                            <b>
                                            <asp:Label ID="lblBatchCaption1" runat="server" Font-Bold="True" 
                                                Font-Names="Arial" Font-Size="10pt" Width="170px">Current No. Items:</asp:Label>
                                            </b>
                                </strong></span>
                            </td>
                    </tr>
                    <tr>
                        <td>
                                <span style="font-size: 11pt"><strong>
                                            <asp:Label ID="lblTotal" runat="server" Font-Bold="False" Font-Names="Arial" 
                                                Font-Size="10pt" Width="163px"></asp:Label>
                                </strong></span>
                            </td>
                        <td>
                                <span style="font-size: 11pt"><strong>
                                            <asp:Label ID="lblTotalVAT" runat="server" Font-Bold="False" Font-Names="Arial" 
                                                Font-Size="10pt" Width="163px"></asp:Label>
                                </strong></span>
                            </td>
                        <td>
                                <span style="font-size: 11pt"><strong>
                                            <asp:Label ID="lblNumItems" runat="server" Font-Bold="False" Font-Names="Arial" 
                                                Font-Size="10pt" Width="163px"></asp:Label>
                                </strong></span>
                            </td>
                    </tr>
                </table>
            </td>
                 </tr>
        <tr>
            <td>
                <igtbl1:UltraWebGrid ID="uwgQuoteItems" runat="server" Height="252px" style="left: 48px; top: 0px" Width="703px">
                     <DisplayLayout AllowColSizingDefault="Free" AllowSortingDefault="OnClient"  AllowUpdateDefault="Yes" 
                                                            BorderCollapseDefault="Separate" CellClickActionDefault="RowSelect" 
                                                            ColWidthDefault=""  HeaderClickActionDefault="SortSingle" Name="uwgDeliveries" 
                                                            RowHeightDefault="20px" RowSelectorsDefault="No" SelectTypeRowDefault="Extended" 
                                                            Version="4.00" AutoGenerateColumns="False">
                         <HeaderStyleDefault BackColor="#996600" BorderStyle="Solid" ForeColor="White" height="20px" HorizontalAlign="Left" Wrap="True">
                        <Padding Bottom="3px" Left="2px" Right="2px" Top="2px" />
                    
                           <BorderDetails ColorBottom="153, 102, 0" ColorLeft="153, 102, 0" ColorRight="153, 102, 0" ColorTop="153, 102, 0" WidthLeft="1px" WidthRight="1px" />
                         </HeaderStyleDefault>
                    
                            <FrameStyle BorderStyle="Solid" BorderWidth="1px"  Font-Names="Arial" Font-Size="9pt"  Height="252px" Width="703px">
                         </FrameStyle>
                    
                            <Images>
                                    <SortAscendingImage Url="./ig_tblSortAsc_White.gif" />
                                    <SortDescendingImage Url="./ig_tblSortDesc_White.gif" />
                            </Images>
                    
                        <RowAlternateStyleDefault BackColor="#EBEBC4" BorderColor="White"  Font-Names="Arial" Font-Size="9pt"  ForeColor="Black">
                            <BorderDetails ColorBottom="White" ColorLeft="White" ColorRight="White"  ColorTop="White" WidthLeft="1px" WidthRight="1px" />
                        </RowAlternateStyleDefault>
                    
                            <ClientSideEvents InitializeLayoutHandler="InitializeGridLayoutHandler" />
                    
                <EditCellStyleDefault BorderStyle="None" BorderWidth="0px">
                    
                </EditCellStyleDefault>
                    
                <FooterStyleDefault BackColor="LightGray" BorderStyle="Solid" BorderWidth="1px">
                      <BorderDetails ColorLeft="White" ColorTop="White" WidthLeft="1px" WidthTop="1px" />
                </FooterStyleDefault>
                    
                <RowSelectorStyleDefault BackColor="#CCFF33">
                  </RowSelectorStyleDefault>
                    
                 <RowStyleDefault BackColor="#F2F2CC" BorderColor="White" Font-Names="Arial"  Font-Overline="False" Font-Size="9pt"  ForeColor="Black">  <Padding Left="3px" />
                        <BorderDetails ColorLeft="White" ColorRight="White" WidthLeft="1px" WidthRight="1px" />
                 </RowStyleDefault>
                    
                 <SelectedRowStyleDefault BackColor="#B0B000" Font-Names="Arial" Font-Size="9pt"  ForeColor="Black">
                 </SelectedRowStyleDefault>
                    
                <ActivationObject BorderColor="" BorderWidth="">                    
                </ActivationObject>
                    
                </DisplayLayout>
                                 <Bands>
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                     <igtbl1:UltraGridBand AllowSorting="Yes">
                            <Columns>
                                <igtbl1:UltraGridColumn BaseColumnName="ProductId" Key="ProductCode" 
                                    Width="80px">
                                    <Header Caption="Product Code">
                                    </Header>
                                </igtbl1:UltraGridColumn>
                                <igtbl1:UltraGridColumn BaseColumnName="ProductId" Key="ProductName" 
                                    Width="150px">
                                    <Header Caption="Product Name">
                                        <RowLayoutColumnInfo OriginX="1" />
                                    </Header>
                                    <Footer>
                                        <RowLayoutColumnInfo OriginX="1" />
                                    </Footer>
                                </igtbl1:UltraGridColumn>
                                <igtbl1:UltraGridColumn BaseColumnName="Quantity" Key="Quantity" Width="50px" 
                                    Hidden="True">
                                    <Header Caption="Qty">
                                        <RowLayoutColumnInfo OriginX="2" />
                                    </Header>
                                    <Footer>
                                        <RowLayoutColumnInfo OriginX="2" />
                                    </Footer>
                                </igtbl1:UltraGridColumn>
                                <igtbl1:UltraGridColumn BaseColumnName="Weight" Key="Weight" Width="50px">
                                    <Header Caption="Amt">
                                        <RowLayoutColumnInfo OriginX="3" />
                                    </Header>
                                    <Footer>
                                        <RowLayoutColumnInfo OriginX="3" />
                                    </Footer>
                                </igtbl1:UltraGridColumn>
                                <igtbl1:UltraGridColumn BaseColumnName="PackageType" Key="PackageType">
                                    <Header Caption="Package Type">
                                        <RowLayoutColumnInfo OriginX="4" />
                                    </Header>
                                    <Footer>
                                        <RowLayoutColumnInfo OriginX="4" />
                                    </Footer>
                                </igtbl1:UltraGridColumn>
                                <igtbl1:UltraGridColumn BaseColumnName="UnitPrice" Key="UnitPrice" 
                                    Width="80px">
                                    <Header Caption="UnitPrice">
                                        <RowLayoutColumnInfo OriginX="5" />
                                    </Header>
                                    <Footer>
                                        <RowLayoutColumnInfo OriginX="5" />
                                    </Footer>
                                </igtbl1:UltraGridColumn>
                                <igtbl1:UltraGridColumn BaseColumnName="VAT" Key="VATCharged" Width="70px" 
                                    FooterTotal="Sum">
                                    <Header Caption="Tax">
                                        <RowLayoutColumnInfo OriginX="6" />
                                    </Header>
                                    <Footer Total="Sum">
                                        <RowLayoutColumnInfo OriginX="6" />
                                    </Footer>
                                </igtbl1:UltraGridColumn>
                                <igtbl1:UltraGridColumn BaseColumnName="TotalPrice" Key="Price" 
                                    FooterTotal="Sum" Width="70px">
                                    <Header Caption="Price">
                                        <RowLayoutColumnInfo OriginX="7" />
                                    </Header>
                                    <Footer Total="Sum">
                                        <RowLayoutColumnInfo OriginX="7" />
                                    </Footer>
                                </igtbl1:UltraGridColumn>
                                <igtbl1:UltraGridColumn BaseColumnName="A" Key="A" Width="20px">
                                    <HeaderStyle BackColor="#CC0000">
                                    <BorderDetails ColorBottom="204, 0, 0" ColorLeft="204, 0, 0" 
                                        ColorRight="204, 0, 0" ColorTop="204, 0, 0" />
                                    </HeaderStyle>
                                    <Header Caption="A">
                                        <RowLayoutColumnInfo OriginX="8" />
                                    </Header>
                                    <Footer>
                                        <RowLayoutColumnInfo OriginX="8" />
                                    </Footer>
                                </igtbl1:UltraGridColumn>
                                <igtbl1:UltraGridColumn BaseColumnName="B" Key="B" Width="20px">
                                    <HeaderStyle BackColor="#CC0000">
                                    <BorderDetails ColorBottom="204, 0, 0" ColorLeft="204, 0, 0" 
                                        ColorRight="204, 0, 0" ColorTop="204, 0, 0" />
                                    </HeaderStyle>
                                    <Header Caption="B">
                                        <RowLayoutColumnInfo OriginX="9" />
                                    </Header>
                                    <Footer>
                                        <RowLayoutColumnInfo OriginX="9" />
                                    </Footer>
                                </igtbl1:UltraGridColumn>
                                <igtbl1:UltraGridColumn BaseColumnName="C" 
                                                                        Key="C" Width="20px">
                                    <HeaderStyle BackColor="#CC0000">
                                    <BorderDetails ColorBottom="204, 0, 0" ColorLeft="204, 0, 0" 
                                        ColorRight="204, 0, 0" ColorTop="204, 0, 0" />
                                    </HeaderStyle>
                                    <Header Caption="C">
                                        <RowLayoutColumnInfo OriginX="10" />
                                    </Header>
                                    <Footer>
                                        <RowLayoutColumnInfo OriginX="10" />
                                    </Footer>
                                </igtbl1:UltraGridColumn>
                                <igtbl1:UltraGridColumn BaseColumnName="D" Key="D" Width="20px">
                                    <HeaderStyle BackColor="#CC0000">
                                    <BorderDetails ColorBottom="204, 0, 0" ColorLeft="204, 0, 0" 
                                        ColorRight="204, 0, 0" ColorTop="204, 0, 0" />
                                    </HeaderStyle>
                                    <header caption="D">
                                        <rowlayoutcolumninfo originx="11" />
                                    </header>
                                    <footer>
                                        <rowlayoutcolumninfo originx="11" />
                                    </footer>
                                </igtbl1:UltraGridColumn>
                                <igtbl1:UltraGridColumn BaseColumnName="E" 
                                    Key="E" Width="20px">
                                    <Header Caption="E">
                                        <RowLayoutColumnInfo OriginX="12" />
                                    </Header>
                                    <HeaderStyle BackColor="#CC0000">
                                    <BorderDetails ColorBottom="204, 0, 0" ColorLeft="204, 0, 0" 
                                        ColorRight="204, 0, 0" ColorTop="204, 0, 0" />
                                    </HeaderStyle>
                                    <Footer>
                                        <RowLayoutColumnInfo OriginX="12" />
                                    </Footer>
                                </igtbl1:UltraGridColumn>
                                <igtbl1:UltraGridColumn BaseColumnName="F" 
                                    Key="F" Width="20px">
                                    <Header Caption="F">
                                        <RowLayoutColumnInfo OriginX="13" />
                                    </Header>
                                    <HeaderStyle BackColor="#CC0000">
                                    <BorderDetails ColorBottom="204, 0, 0" ColorLeft="204, 0, 0" 
                                        ColorRight="204, 0, 0" ColorTop="204, 0, 0" />
                                    </HeaderStyle>
                                    <Footer>
                                        <RowLayoutColumnInfo OriginX="13" />
                                    </Footer>
                                </igtbl1:UltraGridColumn>
                                <igtbl1:UltraGridColumn BaseColumnName="G" Key="G" Width="20px">
                                    <Header Caption="G">
                                        <RowLayoutColumnInfo OriginX="14" />
                                    </Header>
                                    <HeaderStyle BackColor="#CC0000" >
                                    <BorderDetails ColorBottom="204, 0, 0" ColorLeft="204, 0, 0" 
                                        ColorRight="204, 0, 0" ColorTop="204, 0, 0" />
                                        </HeaderStyle>
                                    <Footer>
                                        <RowLayoutColumnInfo OriginX="14" />
                                    </Footer>
                                </igtbl1:UltraGridColumn>
                                <igtbl1:UltraGridColumn BaseColumnName="H" Key="H" Width="20px">
                                    <Header Caption="H">
                                        <RowLayoutColumnInfo OriginX="15" />
                                     </Header>
                                    <HeaderStyle BackColor="#CC0000" >
                                    <BorderDetails ColorBottom="204, 0, 0" ColorLeft="204, 0, 0" 
                                        ColorRight="204, 0, 0" ColorTop="204, 0, 0" />
                                      </HeaderStyle>
                                    <Footer>
                                        <RowLayoutColumnInfo OriginX="15" />
                                    </Footer>
                                </igtbl1:UltraGridColumn>
                                <igtbl1:UltraGridColumn BaseColumnName="I" Key="I" Width="20px">
                                    <Header Caption="I">
                                        <RowLayoutColumnInfo OriginX="16" />
                                    </Header>
                                    <HeaderStyle BackColor="#CC0000" >
                                   <BorderDetails ColorBottom="204, 0, 0" ColorLeft="204, 0, 0" 
                                        ColorRight="204, 0, 0" ColorTop="204, 0, 0" />
                                   </HeaderStyle>
                                    <Footer>
                                        <RowLayoutColumnInfo OriginX="16" />
                                    </Footer>
                                </igtbl1:UltraGridColumn>
                                <igtbl1:UltraGridColumn BaseColumnName="J" Key="J" Width="20px">
                                    <Header Caption="J">
                                        <RowLayoutColumnInfo OriginX="17" />
                                    </Header>
                                    <HeaderStyle BackColor="#CC0000" >
                                    <BorderDetails ColorBottom="204, 0, 0" ColorLeft="204, 0, 0" 
                                        ColorRight="204, 0, 0" ColorTop="204, 0, 0" />
                                    </HeaderStyle>
                                    <Footer>
                                        <RowLayoutColumnInfo OriginX="17" />
                                    </Footer>
                                </igtbl1:UltraGridColumn>
                                <igtbl1:UltraGridColumn BaseColumnName="K" Key="K" Width="20px">
                                    <Header Caption="K">
                                        <RowLayoutColumnInfo OriginX="18" />
                                    </Header>
                                    <HeaderStyle BackColor="#CC0000" >
                                    <BorderDetails ColorBottom="204, 0, 0" ColorLeft="204, 0, 0" 
                                        ColorRight="204, 0, 0" ColorTop="204, 0, 0" />
                                 </HeaderStyle>
                                    <Footer>
                                        <RowLayoutColumnInfo OriginX="18" />
                                    </Footer>
                                </igtbl1:UltraGridColumn>
                                <igtbl1:UltraGridColumn BaseColumnName="L" Key="L" Width="20px">
                                    <Header Caption="L">
                                        <RowLayoutColumnInfo OriginX="19" />
                                    </Header>
                                    <HeaderStyle BackColor="#CC0000" >
                                    <BorderDetails ColorBottom="204, 0, 0" ColorLeft="204, 0, 0" 
                                        ColorRight="204, 0, 0" ColorTop="204, 0, 0" />
                                 </HeaderStyle>
                                    <Footer>
                                        <RowLayoutColumnInfo OriginX="19" />
                                    </Footer>
                                </igtbl1:UltraGridColumn>
                                <igtbl1:UltraGridColumn BaseColumnName="M" Key="M" Width="20px">
                                    <Header Caption="M">
                                        <RowLayoutColumnInfo OriginX="20" />
                                    </Header>
                                    <HeaderStyle BackColor="#CC0000" >
                                    <BorderDetails ColorBottom="204, 0, 0" ColorLeft="204, 0, 0" 
                                        ColorRight="204, 0, 0" ColorTop="204, 0, 0" />
                                 </HeaderStyle>
                                    <Footer>
                                        <RowLayoutColumnInfo OriginX="20" />
                                    </Footer>
                                </igtbl1:UltraGridColumn>
                                <igtbl1:UltraGridColumn BaseColumnName="N" Key="N" Width="20px">
                                    <Header Caption="N">
                                        <RowLayoutColumnInfo OriginX="21" />
                                    </Header>
                                    <HeaderStyle BackColor="#CC0000" >
                                    <BorderDetails ColorBottom="204, 0, 0" ColorLeft="204, 0, 0" 
                                        ColorRight="204, 0, 0" ColorTop="204, 0, 0" />
                                </HeaderStyle>
                                    <Footer>
                                        <RowLayoutColumnInfo OriginX="21" />
                                    </Footer>
                                </igtbl1:UltraGridColumn>
                                <igtbl1:UltraGridColumn BaseColumnName="O" Key="O" Width="20px">
                                    <HeaderStyle BackColor="#CC0000">
                                    <BorderDetails ColorBottom="204, 0, 0" ColorLeft="204, 0, 0" 
                                        ColorRight="204, 0, 0" ColorTop="204, 0, 0" />
                                    </HeaderStyle>
                                    <Header Caption="O">
                                        <RowLayoutColumnInfo OriginX="22" />
                                    </Header>
                                    <Footer>
                                        <RowLayoutColumnInfo OriginX="22" />
                                    </Footer>
                                </igtbl1:UltraGridColumn>
                                <igtbl1:UltraGridColumn BaseColumnName="Comment" 
                                    Key="Comment">
                                    <Header Caption="Comment">
                                        <RowLayoutColumnInfo OriginX="23" />
                                    </Header>
                                    <Footer>
                                        <RowLayoutColumnInfo OriginX="23" />
                                    </Footer>
                                </igtbl1:UltraGridColumn>
                                <igtbl1:UltraGridColumn BaseColumnName="ProductId" Hidden="True" 
                                    Key="ProductId">
                                    <Header Caption="ProductId">
                                        <RowLayoutColumnInfo OriginX="24" />
                                    </Header>
                                    <Footer>
                                        <RowLayoutColumnInfo OriginX="24" />
                                    </Footer>
                                </igtbl1:UltraGridColumn>
                                <igtbl1:UltraGridColumn BaseColumnName="QuoteItemId" Hidden="True" 
                                    Key="QuoteItemId">
                                    <Header Caption="QuoteItemId">
                                        <RowLayoutColumnInfo OriginX="25" />
                                    </Header>
                                    <Footer>
                                        <RowLayoutColumnInfo OriginX="25" />
                                    </Footer>
                                </igtbl1:UltraGridColumn>
                                <igtbl1:UltraGridColumn BaseColumnName="VATRate" Hidden="True" Key="VATRate">
                                    <Header Caption="VATRate">
                                        <RowLayoutColumnInfo OriginX="26" />
                                    </Header>
                                    <Footer>
                                        <RowLayoutColumnInfo OriginX="26" />
                                    </Footer>
                                </igtbl1:UltraGridColumn>
                                <igtbl1:UltraGridColumn BaseColumnName="PackageTypeID" Hidden="True" 
                                    Key="PackageTypeID">
                                    <Header Caption="PackageTypeID">
                                        <RowLayoutColumnInfo OriginX="27" />
                                    </Header>
                                    <Footer>
                                        <RowLayoutColumnInfo OriginX="27" />
                                    </Footer>
                                </igtbl1:UltraGridColumn>
                            </Columns>
                            <AddNewRow View="NotSet" Visible="NotSet">  
                            </AddNewRow>
                        </igtbl1:UltraGridBand>
                                    <igtbl1:UltraGridBand>
                                        <Columns>
                                            <igtbl1:UltraGridColumn>
                                            </igtbl1:UltraGridColumn>
                                        </Columns>
                                        <addnewrow view="NotSet" visible="NotSet">
                                        </addnewrow>
                                    </igtbl1:UltraGridBand>
             </Bands>
                    
 </igtbl1:UltraWebGrid>
            </td>
        </tr>
        <tr>
            <td style="height: 23px">
                &nbsp;</td>
          </tr>
        <tr>
            <td style="height: 55px">
                <table style="width: 680px">
                    <tr>
                        <td>
                            <asp:ImageButton ID="btnAddProduct" runat="server" 
                                ImageUrl="~/App_Themes/Billing/Buttons/Add-Product-Narrow.JPG" />
                        </td>
                        <td>
                            <asp:ImageButton ID="btnClearEmptyRows" runat="server" 
                                ImageUrl="~/App_Themes/Billing/Buttons/Clear-Empty-Rows-Narrow.JPG" />
                        </td>
                        <td>
                            <asp:ImageButton ID="btnRemoveFromList" runat="server" 
                                ImageUrl="~/App_Themes/Billing/Buttons/Remove-From-List-Narrow.JPG" />
                        </td>
                        <td>
                            <asp:ImageButton ID="btnRefreshList" runat="server" 
                                ImageUrl="~/App_Themes/Billing/Buttons/Refresh-List-Narrow.JPG" />
                        </td>
                        <td>
                            &nbsp;</td>
                    </tr>
                </table>
            </td>
       </tr>
        <tr>
            <td style="height: 55px">
                <igtbl1:UltraWebGrid ID="uwgProductClassification" runat="server" Height="155px" 
                                        Width="684px">
                    <Bands>
                        <igtbl1:ultragridband>
                            <AddNewRow View="NotSet" Visible="NotSet">
                            </AddNewRow>
                            <Columns>
                                <igtbl1:ultragridcolumn BaseColumnName="PhysicalForm" Key="PhysicalForm" 
                                                        Width="80px">
                                    <HeaderStyle Wrap="True" />
                                    <Header Caption="Physical Form:">
                                    </Header>
                                </igtbl1:ultragridcolumn>
                                <igtbl1:ultragridcolumn BaseColumnName="EWC" Key="EWC" 
                                                        Width="80px">
                                    <Header Caption="EWC:">
                                        <RowLayoutColumnInfo OriginX="1" />
                                    </Header>
                                    <Footer>
                                        <RowLayoutColumnInfo OriginX="1" />
                                    </Footer>
                                </igtbl1:ultragridcolumn>
                                <igtbl1:ultragridcolumn BaseColumnName="Hazard" Key="Hazard" Width="60px">
                                    <Header Caption="Hazard:">
                                        <RowLayoutColumnInfo OriginX="2" />
                                    </Header>
                                    <Footer>
                                        <RowLayoutColumnInfo OriginX="2" />
                                    </Footer>
                                </igtbl1:ultragridcolumn>
                                <igtbl1:ultragridcolumn BaseColumnName="UNNum" Key="UNNum" Width="60px">
                                    <HeaderStyle Wrap="True" />
                                    <Header Caption="UN No:">
                                        <RowLayoutColumnInfo OriginX="3" />
                                    </Header>
                                    <Footer>
                                        <RowLayoutColumnInfo OriginX="3" />
                                    </Footer>
                                </igtbl1:ultragridcolumn>
                                <igtbl1:ultragridcolumn BaseColumnName="ProperShipping" Format="dd/MM/yyyy" 
                                                        Key="ProperShipping" Width="220px">
                                    <CellStyle Wrap="True">
                                    </CellStyle>
                                    <Header Caption="Proper Shipping:">
                                        <RowLayoutColumnInfo OriginX="4" />
                                    </Header>
                                    <Footer>
                                        <RowLayoutColumnInfo OriginX="4" />
                                    </Footer>
                                </igtbl1:ultragridcolumn>
                                <igtbl1:ultragridcolumn BaseColumnName="PriHazard" Key="PriHazard" Width="55px">
                                    <HeaderStyle Wrap="True" />
                                    <Header caption="Pri Hazard:">
                                        <RowLayoutColumnInfo OriginX="5" />
                                    </Header>
                                    <Footer>
                                        <RowLayoutColumnInfo OriginX="5" />
                                    </Footer>
                                </igtbl1:ultragridcolumn>
                                <igtbl1:ultragridcolumn BaseColumnName="SecHazard"  
                                                        Key="SecHazard" Width="55px">
                                    <HeaderStyle Wrap="True" />
                                    <Header Caption="Sec Hazard:">
                                        <RowLayoutColumnInfo OriginX="6" />
                                    </Header>
                                    <Footer>
                                        <RowLayoutColumnInfo OriginX="6" />
                                    </Footer>
                                </igtbl1:ultragridcolumn>
                                <igtbl1:UltraGridColumn BaseColumnName="PackingGroup" Key="PackingGroup" 
                                    Width="60px">
                                    <HeaderStyle Wrap="True" />
                                    <Header Caption="Packing Group:">
                                        <RowLayoutColumnInfo OriginX="7" />
                                    </Header>
                                    <Footer>
                                        <RowLayoutColumnInfo OriginX="7" />
                                    </Footer>
                                </igtbl1:UltraGridColumn>
                                <igtbl1:UltraGridColumn BaseColumnName="ProductId" Hidden="True" 
                                    Key="ProductId">
                                    <Header Caption="ProductId">
                                        <RowLayoutColumnInfo OriginX="8" />
                                    </Header>
                                    <Footer>
                                        <RowLayoutColumnInfo OriginX="8" />
                                    </Footer>
                                </igtbl1:UltraGridColumn>
                                <igtbl1:UltraGridColumn BaseColumnName="QuoteItemId" Hidden="True" 
                                    Key="QuoteItemId">
                                    <Header Caption="QuoteItemId">
                                        <RowLayoutColumnInfo OriginX="9" />
                                    </Header>
                                    <Footer>
                                        <RowLayoutColumnInfo OriginX="9" />
                                    </Footer>
                                </igtbl1:UltraGridColumn>
                            </Columns>
                        </igtbl1:ultragridband>
                    </Bands>
                    <DisplayLayout AllowColSizingDefault="Free" AllowColumnMovingDefault="OnServer" 
                                            AllowDeleteDefault="Yes" AllowSortingDefault="OnClient" 
                                            AllowUpdateDefault="Yes" AutoGenerateColumns="False" 
                                            BorderCollapseDefault="Separate" CellClickActionDefault="RowSelect" 
                                            HeaderClickActionDefault="SortMulti" Name="UltraWebGrid2" 
                                            RowHeightDefault="20px" RowSelectorsDefault="No" 
                                            SelectTypeRowDefault="Extended" StationaryMargins="Header" 
                                            StationaryMarginsOutlookGroupBy="True" 
                        TableLayout="Fixed" Version="4.00">
                        <GroupByBox>
                        </GroupByBox>
                        <GroupByRowStyleDefault BackColor="Control" BorderColor="Window">
                        </GroupByRowStyleDefault>
                        <ActivationObject BorderColor="" BorderWidth="">
                        </ActivationObject>
                        <FooterStyleDefault BackColor="LightGray" BorderStyle="Solid" BorderWidth="1px">
                            <BorderDetails ColorLeft="White" ColorTop="White" WidthLeft="1px" 
                                                    WidthTop="1px" />
                        </FooterStyleDefault>
                        <RowStyleDefault BackColor="#F2F2CC" BorderColor="Silver" BorderStyle="Solid" 
                                                BorderWidth="1px" Font-Names="Microsoft Sans Serif" 
                            Font-Size="8.25pt">
                            <BorderDetails ColorLeft="Window" ColorTop="Window" />
                            <Padding Left="3px" />
                        </RowStyleDefault>
                        <FilterOptionsDefault>
                            <FilterOperandDropDownStyle BackColor="White" BorderColor="Silver" 
                                                    BorderStyle="Solid" BorderWidth="1px" CustomRules="overflow:auto;" 
                                                    Font-Names="Verdana,Arial,Helvetica,sans-serif" 
                                Font-Size="11px">
                                <Padding Left="2px" />
                            </FilterOperandDropDownStyle>
                            <FilterHighlightRowStyle BackColor="#151C55" ForeColor="White">
                            </FilterHighlightRowStyle>
                            <FilterDropDownStyle BackColor="White" BorderColor="Silver" BorderStyle="Solid" 
                                                    BorderWidth="1px" CustomRules="overflow:auto;" 
                                                    Font-Names="Verdana,Arial,Helvetica,sans-serif" 
                                Font-Size="11px" Height="300px" 
                                                    Width="200px">
                                <Padding Left="2px" />
                            </FilterDropDownStyle>
                        </FilterOptionsDefault>
                        <HeaderStyleDefault BackColor="#715500" BorderStyle="Solid" Font-Bold="True" 
                                                Font-Size="10pt" ForeColor="White" 
                            HorizontalAlign="Left">
                            <BorderDetails ColorBottom="113, 85, 0" ColorLeft="113, 85, 0" 
                                                    ColorRight="113, 85, 0" ColorTop="113, 85, 0" WidthLeft="1px" 
                                                    WidthTop="1px" />
                        </HeaderStyleDefault>
                        <RowAlternateStyleDefault BackColor="#EBEBC4">
                        </RowAlternateStyleDefault>
                        <EditCellStyleDefault BorderStyle="None" BorderWidth="0px">
                        </EditCellStyleDefault>
                        <FrameStyle  BorderColor="InactiveCaption" 
                                                BorderStyle="Solid" BorderWidth="1px" Font-Names="Microsoft Sans Serif" 
                                                Font-Size="8.25pt" Height="155px" Width="684px">
                        </FrameStyle>
                        <Pager MinimumPagesForDisplay="2">
                        </Pager>
                        <AddNewBox Hidden="False">
                        </AddNewBox>
                        <ClientSideEvents InitializeLayoutHandler="InitializeGridLayoutHandler" />
                    </DisplayLayout>
                </igtbl1:UltraWebGrid>
            </td>
       </tr>
        <tr>
            <td style="height: 31px">
                <asp:ImageButton ID="btnSaveClassification" runat="server" 
                    ImageUrl="~/App_Themes/Billing/Buttons/Save-Classification.jpg" />
            </td>
       </tr>
        <tr>
         
            <td>
                <br />

               
                <br />
                <asp:Panel ID="pnlProductAdd" runat="server" BackColor="White"  Height="296px">
                    <table style="width: 700px">
                        <tr>
                            <td colspan="4">
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
                            <td>
                                <asp:Label ID="lblText1" runat="server" Font-Bold="True" Font-Names="Arial" 
                                    Font-Size="10pt" Text="Category:"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlCategory" runat="server" Height="37px" Width="244px">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="lblText" runat="server" Font-Bold="True" Font-Names="Arial" 
                                    Font-Size="10pt" Text="Quantity:" Visible="False"></asp:Label>
                                       <asp:Label ID="lblText4" runat="server" Font-Bold="True" Font-Names="Arial" 
                                    Font-Size="10pt" Text="PackageType:"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtQuantity" runat="server" Visible="False"></asp:TextBox>
                                <asp:DropDownList ID="ddlPackageType" runat="server" Height="37px" 
                                    Width="160px">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblText2" runat="server" Font-Bold="True" Font-Names="Arial" 
                                    Font-Size="10pt" Text="Product:"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlProducts" runat="server" Height="45px" Width="244px">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="lblText0" runat="server" Font-Bold="True" Font-Names="Arial" 
                                    Font-Size="10pt" Font-Underline="True" Text="Amount:"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtWeight" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <span style="font-size: 11pt"><strong>
                                <asp:Label ID="lblOutputWeight0" runat="server" Font-Bold="True" 
                                    Font-Names="Arial" Font-Size="10pt" Text="This product is tracked by: " 
                                    Visible="False"></asp:Label>
                                <asp:TextBox ID="lblPnlProdMessage" runat="server" BorderColor="White" 
                                    BorderStyle="None" BorderWidth="0px" Font-Bold="True" ForeColor="Red" 
                                    ReadOnly="True" Width="150px" Visible="False"></asp:TextBox>
                                </strong></span></td>
                            <td>
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>
                        </tr>
                        <tr>
                            <td>
                             
                            </td>
                            <td>
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblText3" runat="server" Font-Bold="True" Font-Names="Arial" 
                                    Font-Size="10pt" Text="Comment:"></asp:Label>
                            </td>
                            <td colspan="3">
                                <asp:TextBox ID="txtComment" runat="server" Width="537px"></asp:TextBox>
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
                                <asp:ImageButton ID="cmdCancelAdd" runat="server" 
                                    ImageUrl="~/App_Themes/Billing/Buttons/Cancel.gif" />
                            </td>
                            <td>
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>
                            <td>
                                <asp:ImageButton ID="cmdProdAdd" runat="server" 
                                    ImageUrl="~/App_Themes/Billing2/Buttons/Add.GIF" />
                            </td>
                        </tr>
                    </table>
                     <ajaxToolkit:CascadingDropDown ID="CascadingDropDown1" runat="server" 
                            Category="Category" LoadingText="Loading Product Categories ..." 
                            PromptText="Select Category .." ServiceMethod="GetProductCategories" 
                            ServicePath="~/ModuleServices.asmx" TargetControlID="ddlCategory">
                        </ajaxToolkit:CascadingDropDown>
                        <ajaxToolkit:CascadingDropDown ID="CascadingDropDown2" runat="server" 
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

                <br />
                <asp:Panel ID="PanelMsg" runat="server" backcolor="White" borderstyle="Solid" 
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
                        <asp:ImageButton ID="cmdCloseMessage" runat="server" 
                            ImageUrl="~/App_Themes/Billing/Buttons/ok.gif" />
                    </center>
                    <center>
                    </center>
                </asp:Panel>
                <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtenderMessage" runat="server" 
                    BackgroundCssClass="modalBackground" CancelControlID="cmdCloseMessage" 
                    DropShadow="True" oncancelscript="HideMsgPopup()" PopupControlID="PanelMsg" 
                    TargetControlID="PanelMsg">
                </ajaxToolkit:ModalPopupExtender>
                <br />


                 

                <br />
            </td>
        </tr>
    </table>
 </asp:Content>

