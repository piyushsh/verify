<%@ Page Title="" Language="VB" MasterPageFile="~/TabPages/ProjectMain.master" AutoEventWireup="false" CodeFile="AcknowledgeOrders.aspx.vb" Inherits="TabPages_AcknowledgeOrders" %>

<%@ MasterType VirtualPath="~/TabPages/ProjectMain.master" %>

<%@ Register Assembly="Infragistics4.Web.v13.2, Version=13.2.20132.2028, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" namespace="Infragistics.Web.UI.DisplayControls" tagprefix="ig" %>
<%@ Register Assembly="Infragistics4.Web.v13.2, Version=13.2.20132.2028, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" namespace="Infragistics.Web.UI.EditorControls" tagprefix="ig" %>
<%@ Register Assembly="Infragistics4.Web.v13.2, Version=13.2.20132.2028, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" namespace="Infragistics.Web.UI.GridControls" tagprefix="ig" %>
<%@ Register Assembly="Infragistics4.Web.v13.2, Version=13.2.20132.2028, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" namespace="Infragistics.Web.UI.ListControls" tagprefix="ig" %>



<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ProjectMAIN_content" Runat="Server">


       <asp:UpdatePanel id="UpdatePanel1" runat="server" UpdateMode="Conditional" >

         <Triggers>
            <asp:PostBackTrigger ControlID="btnExport" />
           
        </Triggers>
    
        <ContentTemplate>

            <script type="text/javascript" id="AddService">
               // function DeselectGridRows(sender, e) {
               
                  //  var grid = $find("<%=wdgRepGrid.ClientID%>");
               //     var selection = grid.get_behaviors().get_selection();
                //    var rows = selection.get_selectedRows();
                //    rows.remove(rows.getItem(0));
                   

                //}

                function ForcePostback(sender, e) {
                   
                }

                function HideMsgPopupMsg() {
                    var modal = $find("<%=ModalPopupExtenderMsg.BehaviorID%>");
                      modal.hide();
                  }
                </script>


    <div style="margin-left: 10px">

    

  <table style="width: 699px">
        <tr>
            <td style="font-family: Arial; font-size: 8px">
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:ImageButton ID="btnBack1" runat="server" AlternateText="Back" ImageUrl="~/App_Themes/Billing/Buttons/Back.gif" />
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:ImageButton ID="btnBack2" runat="server" AlternateText="back" ImageUrl="~/App_Themes/Billing/Buttons/Back.gif" />
            </td>
                 </tr>
        <tr>
            <td style="font-family: Arial; font-size: 8px">
                                &nbsp;</td>
                 </tr>
      <tr>
          <td style="border: 1px solid #808080">

               &nbsp;&nbsp; <asp:Label ID="Label2" runat="server" Font-Bold="True" Font-Names="Arial" fontsize ="11px"
                                        ForeColor="Black" Text="Sales Order Details " 
                                        Width="180px" Font-Size="12pt"></asp:Label> <asp:Label ID="Label4" runat="server" Font-Bold="True" Font-Names="Arial" fontsize ="11px"
                                        ForeColor="Black" Text="( where Status = 'Open - SendAcknowledge' )" 
                                        Width="300px" Font-Size="10pt"></asp:Label>


              <ig:WebDataGrid ID="wdgRepGrid" runat="server"
                  DataKeyFields="VT_UniqueIndex"
                  AltItemCssClass="VerifyGrid_Report_AlternateRowSmall"
                  AutoGenerateColumns="False" Font-Names="Arial"
                  ItemCssClass="VerifyGrid_Report_RowSmall"
                  EnableTheming="True" FooterCaptionCssClass="VerifyGrid_Report_Footer"
                  HeaderCaptionCssClass="VerifyGrid_Report_HeaderGreenSmall" BackColor="White" 
                  CssClass="VerifyGrid_Report_Frame" Height="480px">
                  
                
                  <Columns>

                      <ig:BoundDataField DataFieldName="SO_ContiguousNum" Key="SO_ContiguousNum" Width="70px"
                          DataType="System.String">
                          <Header Text="Order No:" />
                      </ig:BoundDataField>
                     <ig:BoundDataField DataFieldName="SalesItemNum" Key="SalesItemNum"  Width="40px" > 
                        <Header Text="Item:"/>
                     </ig:BoundDataField>
                      <ig:BoundDataField DataFieldName="Other_CustPO" Key="Other_CustPO" Width="80px"
                          DataType="System.String">
                          <Header Text="Purchase Order:" />
                      </ig:BoundDataField>



                      <ig:BoundDataField DataFieldName="SoldTo_Name" Key="SoldTo_Name" Width="130px"
                          DataType="System.String">
                          <Header Text="Customer - Sold To:" />
                      </ig:BoundDataField>
                      <ig:BoundDataField DataFieldName="Site" Key="Site" Width="60px"
                          DataType="System.String">
                          <Header Text="Site:" />
                      </ig:BoundDataField>
                      <ig:BoundDataField DataFieldName="ProductCode" Key="ProductCode" Width="80px"
                          DataType="System.String">
                          <Header Text="Product:" />
                      </ig:BoundDataField>
                      <ig:BoundDataField DataFieldName="DateCreated" Key="DateCreated" Width="70px"
                          DataType="System.DateTime">
                          <Header Text="Order Date:" />
                      </ig:BoundDataField>
                      <ig:BoundDataField DataFieldName="Item_DateRequested" Key="Item_DateRequested" Width="85px"
                          DataType="System.DateTime" DataFormatString="{0:d}">
                          <Header Text="Required Date:" />
                      </ig:BoundDataField>

                      <ig:BoundDataField DataFieldName="Item_DateOut" Key="Item_DateOut" Width="85px" DataFormatString="{0:d}" DataType="System.DateTime">
                          <Header Text="Date Out:" />
                      </ig:BoundDataField>

                      <ig:BoundDataField DataFieldName="Item_DateArrival" Key="Item_DateArrival" Width="85px" DataFormatString="{0:d}" DataType="System.DateTime">
                          <Header Text="Arrival:" />
                      </ig:BoundDataField>

                      <ig:BoundDataField DataFieldName="QuantityRequested" Key="QuantityRequested" Width="80px"
                          DataType="System.Int32">
                          <Header Text="Quantity:" />
                      </ig:BoundDataField>
                      <ig:BoundDataField DataFieldName="DeliverTo_Name" Key="DeliverTo_Name" Width="120px"
                          DataType="System.String">
                          <Header Text="Ship To:" />
                      </ig:BoundDataField>
                      <ig:BoundDataField DataFieldName="BillTo_Name" Key="BillTo_Name" Width="100px"
                          DataType="System.String">
                          <Header Text="Invoice To:" />
                      </ig:BoundDataField>
                      <ig:BoundDataField DataFieldName="SalesOrderNum" Key="SalesOrderNum" Width="70px"
                          DataType="System.String">
                          <Header Text="Sys.Num:" />
                      </ig:BoundDataField>
             
                  </Columns>
                  <Behaviors>
                      <ig:EditingCore>
                          <Behaviors>
                              <ig:CellEditing Enabled="true">
                                  <ColumnSettings>

                                      <ig:EditingColumnSetting ColumnKey="Item_DateOut" ReadOnly="True" />
                                      <ig:EditingColumnSetting ColumnKey="Item_DateArrival" ReadOnly="True"  />

                                     <ig:EditingColumnSetting ColumnKey="SalesOrderNum" ReadOnly="True" />
                                      <ig:EditingColumnSetting ColumnKey="Other_CustPO" ReadOnly="True" />
                                      <ig:EditingColumnSetting ColumnKey="SoldTo_Name" ReadOnly="True" />
                                      <ig:EditingColumnSetting ColumnKey="Site" ReadOnly="True" />
                                      <ig:EditingColumnSetting ColumnKey="ProductCode" ReadOnly="True" />
                                      <ig:EditingColumnSetting ColumnKey="DateCreated" ReadOnly="True" />
                                      <ig:EditingColumnSetting ColumnKey="Item_DateRequested" ReadOnly="True" />
                                      
                                      <ig:EditingColumnSetting ColumnKey="QuantityRequested" ReadOnly="True" />
                                      <ig:EditingColumnSetting ColumnKey="DeliverTo_Name" ReadOnly="True" />
                                      <ig:EditingColumnSetting ColumnKey="BillTo_Name" ReadOnly="True" />
                                                         

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
                              <ig:ColumnSummaryInfo ColumnKey="SalesOrderNum">
                                  <Summaries>
                                      <ig:Summary SummaryType="Count" />
                                  </Summaries>
                              </ig:ColumnSummaryInfo>
                          </ColumnSummaries>
  
                          <ColumnSettings>
                              
                                   <ig:SummaryRowSetting ColumnKey="SalesOrderNum" 
                                        EnableColumnSummaryOptions="True" EnableFormatting="True" FormatString=" {1}" 
                                        ShowSummaryButton="False">
                                    </ig:SummaryRowSetting>
   
                                    <ig:SummaryRowSetting ColumnKey="SO_ContiguousNum" 
                                         EnableColumnSummaryOptions="False"  ShowSummaryButton="False">
                                    </ig:SummaryRowSetting>
                               <ig:SummaryRowSetting ColumnKey="SalesItemNum" 
                                         EnableColumnSummaryOptions="False"  ShowSummaryButton="False">
                                    </ig:SummaryRowSetting>

                                    <ig:SummaryRowSetting ColumnKey="Other_CustPO" 
                                         EnableColumnSummaryOptions="False"  ShowSummaryButton="False">
                                    </ig:SummaryRowSetting>
                                    <ig:SummaryRowSetting ColumnKey="SoldTo_Name" 
                                         EnableColumnSummaryOptions="False"  ShowSummaryButton="False">
                                    </ig:SummaryRowSetting>
                                    <ig:SummaryRowSetting ColumnKey="Site" 
                                         EnableColumnSummaryOptions="False"  ShowSummaryButton="False">
                                    </ig:SummaryRowSetting>
                                     <ig:SummaryRowSetting ColumnKey="ProductCode" 
                                         EnableColumnSummaryOptions="False"  ShowSummaryButton="False">
                                    </ig:SummaryRowSetting>
                                    <ig:SummaryRowSetting ColumnKey="DateCreated" 
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
                                     <ig:SummaryRowSetting ColumnKey="QuantityRequested" 
                                         EnableColumnSummaryOptions="False"  ShowSummaryButton="False">
                                    </ig:SummaryRowSetting>
                                    <ig:SummaryRowSetting ColumnKey="DeliverTo_Name" 
                                         EnableColumnSummaryOptions="False"  ShowSummaryButton="False">
                                    </ig:SummaryRowSetting>
                                    <ig:SummaryRowSetting ColumnKey="BillTo_Name" 
                                         EnableColumnSummaryOptions="False"  ShowSummaryButton="False">
                                    </ig:SummaryRowSetting>
                         
                            
                          </ColumnSettings>
                      </ig:SummaryRow>


                      <ig:Filtering Enabled="true" FilterButtonCssClass="Filter_LAlign">
                          <ColumnSettings>
                              <ig:ColumnFilteringSetting ColumnKey="SalesOrderNum" Enabled="true" />
                          </ColumnSettings>
                          <ColumnFilters>
                              <ig:ColumnFilter ColumnKey="SalesOrderNum">
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

                       <ig:Selection CellClickAction="Row" RowSelectType="Multiple" CellSelectType="Multiple" SelectedCellCssClass="SelectedRows">
                            </ig:Selection>

                  </Behaviors>

              </ig:WebDataGrid>
              <br />
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
              <asp:ImageButton ID="btnSaveDateChanges" runat="server" ImageUrl="~/App_Themes/Billing/Buttons/Save.gif" Visible="False" />
               &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:ImageButton ID="btnExport" runat="server" AlternateText="Export" 
                                    ToolTip="Click to Export the data from this Category" ImageUrl="~/App_Themes/Billing/Buttons/Export.GIF" 
                                    BackColor="#996600"  />
               &nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;<asp:ImageButton ID="btnMarkAsAcknowledged" runat="server" ImageUrl="~/App_Themes/Billing/Buttons/MarkAsAcknowledged.gif" AlternateText="Mark as Acknowledged" />
               &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
              <asp:ImageButton ID="btnPrintAcknowledgement" runat="server" ImageUrl="~/App_Themes/Billing/Buttons/Print-Acknowledgement.jpg" AlternateText="Print Acknowledgement" />
          &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
          </td>
      </tr>
        <tr>
 <td style="font-family: Arial; font-size: 8px">
                                &nbsp;&nbsp;</td>
        </tr>
              <tr>
 <td style="font-family: Arial; font-size: 8px">
                                &nbsp;&nbsp;</td>
        </tr>
      <tr>
          <td>
              
          </td>
      </tr>
        
     
      <tr>
          <td>

              
             

                


          </td>
      </tr>
        
     
 </table>

                <ig:WebExcelExporter ID="VerifyExporter" runat="server">
                    </ig:WebExcelExporter> 

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
                        ImageUrl="~/App_Themes/Buttons/OK.gif" />
                      
                </center>
                <center>
                </center>
                    
            </asp:Panel>
    <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtenderMsg" runat="server" 
        BackgroundCssClass="modalBackground" CancelControlID="cmdMsgOK" 
        DropShadow="True" oncancelscript="HideMsgPopupMsg()" PopupControlID="pnlMsg" 
        TargetControlID="pnlMsg">
    </ajaxToolkit:ModalPopupExtender>
    </div>

       </ContentTemplate>

    

           </asp:UpdatePanel> 

</asp:Content>

