<%@ Page Title="" Language="VB" MasterPageFile="~/Quotes/FormsMaster.master" AutoEventWireup="false" CodeFile="QuoteProducts.aspx.vb" Inherits="Quotes_QuoteProducts" EnableEventValidation="false" %>

<%@ Register assembly="Infragistics2.WebUI.UltraWebGrid.v7.3, Version=7.3.20073.38, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" namespace="Infragistics.WebUI.UltraWebGrid" tagprefix="igtbl1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="DetailsMaster_ContentPlaceHolder" Runat="Server">
  <script type="text/javascript">

      


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
                                <igtbl1:UltraGridColumn BaseColumnName="Quantity" Key="Quantity" Width="50px">
                                    <Header Caption="Qty">
                                        <RowLayoutColumnInfo OriginX="2" />
                                    </Header>
                                    <Footer>
                                        <RowLayoutColumnInfo OriginX="2" />
                                    </Footer>
                                </igtbl1:UltraGridColumn>
                                <igtbl1:UltraGridColumn BaseColumnName="Weight" Key="Weight" Width="50px">
                                    <Header Caption="Wgt">
                                        <RowLayoutColumnInfo OriginX="3" />
                                    </Header>
                                    <Footer>
                                        <RowLayoutColumnInfo OriginX="3" />
                                    </Footer>
                                </igtbl1:UltraGridColumn>
                                <igtbl1:UltraGridColumn BaseColumnName="UnitPrice" Key="UnitPrice" Width="80px">
                                    <Header Caption="UnitPrice">
                                        <RowLayoutColumnInfo OriginX="4" />
                                    </Header>
                                    <Footer>
                                        <RowLayoutColumnInfo OriginX="4" />
                                    </Footer>
                                </igtbl1:UltraGridColumn>
                                <igtbl1:UltraGridColumn BaseColumnName="VAT" Key="VATCharged" 
                                    Width="70px" FooterTotal="Sum">
                                    <Header Caption="Tax">
                                        <RowLayoutColumnInfo OriginX="5" />
                                    </Header>
                                    <Footer Total="Sum">
                                        <RowLayoutColumnInfo OriginX="5" />
                                    </Footer>
                                </igtbl1:UltraGridColumn>
                                <igtbl1:UltraGridColumn BaseColumnName="TotalPrice" Key="Price" Width="70px" 
                                    FooterTotal="Sum">
                                    <Header Caption="Price">
                                        <RowLayoutColumnInfo OriginX="6" />
                                    </Header>
                                    <Footer Total="Sum">
                                        <RowLayoutColumnInfo OriginX="6" />
                                    </Footer>
                                </igtbl1:UltraGridColumn>
                                <igtbl1:UltraGridColumn BaseColumnName="A" Key="A" Width="20px">
                                    <HeaderStyle BackColor="#CC0000">
                                    <BorderDetails ColorBottom="204, 0, 0" ColorLeft="204, 0, 0" 
                                        ColorRight="204, 0, 0" ColorTop="204, 0, 0" />
                                    </HeaderStyle>
                                    <Header Caption="A">
                                        <RowLayoutColumnInfo OriginX="7" />
                                    </Header>
                                    <Footer>
                                        <RowLayoutColumnInfo OriginX="7" />
                                    </Footer>
                                </igtbl1:UltraGridColumn>
                                <igtbl1:UltraGridColumn BaseColumnName="B" Key="B" Width="20px">
                                    <HeaderStyle BackColor="#CC0000">
                                    <BorderDetails ColorBottom="204, 0, 0" ColorLeft="204, 0, 0" 
                                        ColorRight="204, 0, 0" ColorTop="204, 0, 0" />
                                    </HeaderStyle>
                                    <Header Caption="B">
                                        <RowLayoutColumnInfo OriginX="8" />
                                    </Header>
                                    <Footer>
                                        <RowLayoutColumnInfo OriginX="8" />
                                    </Footer>
                                </igtbl1:UltraGridColumn>
                                <igtbl1:UltraGridColumn BaseColumnName="C" Key="C" Width="20px">
                                    <HeaderStyle BackColor="#CC0000">
                                    <BorderDetails ColorBottom="204, 0, 0" ColorLeft="204, 0, 0" 
                                        ColorRight="204, 0, 0" ColorTop="204, 0, 0" />
                                    </HeaderStyle>
                                    <Header Caption="C">
                                        <RowLayoutColumnInfo OriginX="9" />
                                    </Header>
                                    <Footer>
                                        <RowLayoutColumnInfo OriginX="9" />
                                    </Footer>
                                </igtbl1:UltraGridColumn>
                                <igtbl1:UltraGridColumn BaseColumnName="D" 
                                                                        Key="D" Width="20px">
                                    <HeaderStyle BackColor="#CC0000">
                                    <BorderDetails ColorBottom="204, 0, 0" ColorLeft="204, 0, 0" 
                                        ColorRight="204, 0, 0" ColorTop="204, 0, 0" />
                                    </HeaderStyle>
                                    <Header Caption="D">
                                        <RowLayoutColumnInfo OriginX="10" />
                                    </Header>
                                    <Footer>
                                        <RowLayoutColumnInfo OriginX="10" />
                                    </Footer>
                                </igtbl1:UltraGridColumn>
                                <igtbl1:UltraGridColumn BaseColumnName="E" Key="E" Width="20px">
                                    <HeaderStyle BackColor="#CC0000">
                                    <BorderDetails ColorBottom="204, 0, 0" ColorLeft="204, 0, 0" 
                                        ColorRight="204, 0, 0" ColorTop="204, 0, 0" />
                                    </HeaderStyle>
                                    <header caption="E">
                                        <rowlayoutcolumninfo originx="11" />
                                    </header>
                                    <footer>
                                        <rowlayoutcolumninfo originx="11" />
                                    </footer>
                                </igtbl1:UltraGridColumn>
                                <igtbl1:UltraGridColumn BaseColumnName="F" 
                                    Key="F" Width="20px">
                                    <Header Caption="F">
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
                                <igtbl1:UltraGridColumn BaseColumnName="G" 
                                    Key="G" Width="20px">
                                    <Header Caption="G">
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
                                <igtbl1:UltraGridColumn BaseColumnName="H" Key="H" Width="20px">
                                    <Header Caption="H">
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
                                <igtbl1:UltraGridColumn BaseColumnName="I" Key="I" Width="20px">
                                    <Header Caption="I">
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
                                <igtbl1:UltraGridColumn BaseColumnName="J" Key="J" Width="20px">
                                    <Header Caption="J">
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
                                <igtbl1:UltraGridColumn BaseColumnName="K" Key="K" Width="20px">
                                    <Header Caption="K">
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
                                <igtbl1:UltraGridColumn BaseColumnName="L" Key="L" Width="20px">
                                    <Header Caption="L">
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
                                <igtbl1:UltraGridColumn BaseColumnName="M" Key="M" Width="20px">
                                    <Header Caption="M">
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
                                <igtbl1:UltraGridColumn BaseColumnName="N" Key="N" Width="20px">
                                    <Header Caption="N">
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
                                <igtbl1:UltraGridColumn BaseColumnName="O" Key="O" Width="20px">
                                    <Header Caption="O">
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
                                <igtbl1:UltraGridColumn BaseColumnName="Comment" Key="Comment">
                                    <Header Caption="Comment">
                                        <RowLayoutColumnInfo OriginX="22" />
                                    </Header>
                                    <Footer>
                                        <RowLayoutColumnInfo OriginX="22" />
                                    </Footer>
                                </igtbl1:UltraGridColumn>
                                <igtbl1:UltraGridColumn BaseColumnName="ProductId" Hidden="True" 
                                    Key="ProductId">
                                    <Header Caption="ProductId">
                                        <RowLayoutColumnInfo OriginX="23" />
                                    </Header>
                                    <Footer>
                                        <RowLayoutColumnInfo OriginX="23" />
                                    </Footer>
                                </igtbl1:UltraGridColumn>
                                <igtbl1:UltraGridColumn BaseColumnName="QuoteItemId" Hidden="True" 
                                    Key="QuoteItemId">
                                    <Header Caption="QuoteItemId">
                                        <RowLayoutColumnInfo OriginX="24" />
                                    </Header>
                                    <Footer>
                                        <RowLayoutColumnInfo OriginX="24" />
                                    </Footer>
                                </igtbl1:UltraGridColumn>
                                <igtbl1:UltraGridColumn BaseColumnName="VATRate" Hidden="True" Key="VATRate">
                                    <Header Caption="VATRate">
                                        <RowLayoutColumnInfo OriginX="25" />
                                    </Header>
                                    <Footer>
                                        <RowLayoutColumnInfo OriginX="25" />
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
            <td>
                &nbsp;</td>
          </tr>
        <tr>
            <td>
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
         
            <td>
                <br />

               
                <br />
                <asp:Panel ID="pnlProductAdd" runat="server" BackColor="White"  Height="255px">
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
                                    Font-Size="10pt" Text="Quantity:"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtQuantity" runat="server"></asp:TextBox>
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
                                    Font-Size="10pt" Font-Underline="True" Text="Weight:"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtWeight" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <span style="font-size: 11pt"><strong>
                                <asp:Label ID="lblOutputWeight0" runat="server" Font-Bold="True" 
                                    Font-Names="Arial" Font-Size="10pt" Text="This product is tracked by: "></asp:Label>
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
                <br />


                 

                <br />
            </td>
        </tr>
    </table>
 </asp:Content>

