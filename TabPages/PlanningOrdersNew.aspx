<%@ Page Title="" Language="VB" MasterPageFile="~/TabPages/ProjectMain.master" AutoEventWireup="false" CodeFile="PlanningOrdersNew.aspx.vb" Inherits="TabPages_PlanningOrdersNew" %>

<%@ MasterType VirtualPath="~/TabPages/ProjectMain.master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>


<%@ Register Assembly="Infragistics4.Web.v13.2, Version=13.2.20132.2028, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" namespace="Infragistics.Web.UI.DisplayControls" tagprefix="ig" %>
<%@ Register Assembly="Infragistics4.Web.v13.2, Version=13.2.20132.2028, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" namespace="Infragistics.Web.UI.EditorControls" tagprefix="ig" %>
<%@ Register Assembly="Infragistics4.Web.v13.2, Version=13.2.20132.2028, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" namespace="Infragistics.Web.UI.GridControls" tagprefix="ig" %>
<%@ Register Assembly="Infragistics4.Web.v13.2, Version=13.2.20132.2028, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" namespace="Infragistics.Web.UI.ListControls" tagprefix="ig" %>



<asp:Content ID="Content2" ContentPlaceHolderID="ProjectMAIN_content" Runat="Server">

    <script type="text/javascript">
     function HideModalPopupMsg() {
         var modal = $find("<%=ModalPopupExtenderMsgLocal.BehaviorID%>");
                 modal.hide();
             }
        function HideModalPopupInput() {
            var modal = $find("<%=ModalPopupExtenderInput.BehaviorID%>");
                 modal.hide();
             }

 </script>

    <div style="margin-left: 10px">

    

  <table style="width: 699px">
        <tr>
            <td style="font-family: Arial; font-size: 8px">
                                &nbsp;&nbsp;&nbsp;&nbsp; </td>
                 </tr>
      <tr>
          <td style="border: 1px solid #808080">

      <asp:UpdatePanel id="PlanningGridUpdatePanel" runat="server" UpdateMode="Conditional" >

         <Triggers>
            <asp:PostBackTrigger ControlID="btnExport" />
        </Triggers>
    
        <ContentTemplate>


               &nbsp;&nbsp; <asp:Label ID="Label2" runat="server" Font-Bold="True" Font-Names="Arial" fontsize ="11px"
                                        ForeColor="Black" Text="Sales Order Details " 
                                        Width="180px" Font-Size="12pt"></asp:Label> <asp:Label ID="Label4" runat="server" Font-Bold="True" Font-Names="Arial" fontsize ="11px"
                                        ForeColor="Black" Text="( where Status = 'Open - Awaiting Planning' )" 
                                        Width="300px" Font-Size="10pt"></asp:Label>

               <ig:WebMonthCalendar ID="webMonthCalendar1" runat="server">
                                            </ig:WebMonthCalendar>



              <ig:WebDataGrid ID="wdgRepGrid" runat="server"
                  DataKeyFields="VT_UniqueIndex"
                  AltItemCssClass="VerifyGrid_Report_AlternateRowSmall"
                  AutoGenerateColumns="False" Font-Names="Arial"
                  ItemCssClass="VerifyGrid_Report_RowSmall"
                  EnableTheming="True" FooterCaptionCssClass="VerifyGrid_Report_Footer"
                  HeaderCaptionCssClass="VerifyGrid_Report_HeaderGreenSmall" BackColor="White"
                  CssClass="VerifyGrid_Report_Frame" Height="280px">
                  
                 <EditorProviders>
                     
                     <ig:DatePickerProvider ID="DatePickerProvider1" EditorControl-Nullable="true"  EditorControl-DataMode="DateOrDBNull">
                    
                        <EditorControl runat="server" DataMode="DateOrDBNull" ClientIDMode="Predictable" ID="DateProvider1" DropDownCalendarID="webMonthCalendar1"></EditorControl>
                     </ig:DatePickerProvider>
                    
                       <ig:DatePickerProvider ID="DatePickerProvider2" EditorControl-Nullable="true"  EditorControl-DataMode="DateOrDBNull">
                    
                        <EditorControl runat="server" DataMode="DateOrDBNull" ClientIDMode="Predictable" ID="EditorControl1" DropDownCalendarID="webMonthCalendar1"></EditorControl>
                     </ig:DatePickerProvider>
                                              
                     
                  </EditorProviders>

                  <Columns>

                      
                        <ig:BoundDataField DataFieldName="SO_ContiguousNum" Key="SO_ContiguousNum" Width="70px"
                            DataType="System.String">
                            <Header Text="Order No:" />
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
                          <Header Text="Code:" />
                      </ig:BoundDataField>
                       <ig:BoundDataField  hidden="true" DataFieldName="ItemType" Key="ItemType" Width="56px"
                          DataType="System.String">
                          <Header Text="Type:" />
                      </ig:BoundDataField>
                      <ig:BoundDataField DataFieldName="DateCreated" Key="DateCreated" Width="78px"
                          DataType="System.DateTime">
                          <Header Text="Order Date:" />
                      </ig:BoundDataField>
                      <ig:BoundDataField DataFieldName="Item_DateRequested" Key="Item_DateRequested" Width="78px"
                          DataType="System.DateTime" DataFormatString="{0:d}">
                          <Header Text="Required Date:" />
                      </ig:BoundDataField>

                      <ig:BoundDataField DataFieldName="Item_DateOut" Key="Item_DateOut" Width="78px" DataFormatString="{0:d}" DataType="System.DateTime">
                          <Header Text="Date Out:" />
                      </ig:BoundDataField>

                      <ig:BoundDataField DataFieldName="Item_DateArrival" Key="Item_DateArrival" Width="78px" DataFormatString="{0:d}" DataType="System.DateTime">
                          <Header Text="Arrival:" />
                      </ig:BoundDataField>

                      <ig:BoundDataField DataFieldName="QuantityRequested" Key="QuantityRequested" Width="64px"
                          DataType="System.Int32">
                          <Header Text="Quantity:" />
                      </ig:BoundDataField>
                      <ig:BoundCheckBoxField DataFieldName="Kanban" Key="Kanban" Width="50px" ToolTipChecked="This is a kanban item" ToolTipUnchecked="This is NOT a kanban item" CssClass="Center" ToolTipPartial="Click here to mark this item as kanban">
                <CheckBox CheckedImageUrl="../App_Themes/Grid Buttons/Paid.gif"></CheckBox>
                <Header Text="Kanban:" />
            </ig:BoundCheckBoxField>
                      <ig:BoundDataField DataFieldName="PlanningComment" Key="PlanningComment" Width="200px"
                          DataType="System.String">
                          <Header Text="Prod. Eng. Comment:" />
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
                          <Header Text="Sys. Num:" />
                      </ig:BoundDataField>
                       
                       <ig:BoundDataField DataFieldName="SalesItemNum" Key="SalesItemNum" Width="100px" Hidden ="true"
                          DataType="System.String">
                          <Header Text="Item Num:" />
                      </ig:BoundDataField>
                       <ig:BoundDataField DataFieldName="CustomerCode" Key="CustomerCode" Width="100px" Hidden ="true"
                          DataType="System.String">
                          <Header Text="CustomerCode:" />
                      </ig:BoundDataField>
                       <ig:BoundDataField DataFieldName="PO_UnitPrice" Key="PO_UnitPrice" Width="100px" Hidden ="true"
                          DataType="System.String">
                          <Header Text="PO_UnitPrice:" />
                      </ig:BoundDataField>
                       <ig:BoundDataField DataFieldName="Comment" Key="Comment" Width="100px" Hidden ="true"
                          DataType="System.String">
                          <Header Text="Comment:" />
                      </ig:BoundDataField>
                      <ig:BoundDataField DataFieldName="MachineNum" Key="MachineNum" Width="100px" 
                          DataType="System.String">
                          <Header Text="Machine Num:" />
                      </ig:BoundDataField>
                       
                                           
                  </Columns>
                  <Behaviors>
                      <ig:EditingCore>
                          <Behaviors>
                              <ig:CellEditing Enabled="true">
                                  <ColumnSettings>

                                      <ig:EditingColumnSetting ColumnKey="Item_DateOut" EditorID="DatePickerProvider1" />
                                      <ig:EditingColumnSetting ColumnKey="Item_DateArrival" EditorID="DatePickerProvider2" />

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
                                      <ig:EditingColumnSetting ColumnKey="MachineNum" ReadOnly="False" />
                                                  <ig:EditingColumnSetting ColumnKey="Kanban" ReadOnly="False" />               

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
   

                                    <ig:SummaryRowSetting ColumnKey="Other_CustPO" 
                                         EnableColumnSummaryOptions="False"  ShowSummaryButton="False">
                                    </ig:SummaryRowSetting>
                                    <ig:SummaryRowSetting ColumnKey="SoldTo_Name" 
                                         EnableColumnSummaryOptions="False"  ShowSummaryButton="False">
                                    </ig:SummaryRowSetting>
                                    <ig:SummaryRowSetting ColumnKey="Site" 
                                         EnableColumnSummaryOptions="False"  ShowSummaryButton="False">
                                    </ig:SummaryRowSetting>
                                   <ig:SummaryRowSetting ColumnKey="PlanningComment" 
                                         EnableColumnSummaryOptions="False"  ShowSummaryButton="False">
                                    </ig:SummaryRowSetting>
                                     <ig:SummaryRowSetting ColumnKey="ProductCode" 
                                         EnableColumnSummaryOptions="False"  ShowSummaryButton="False">
                                    </ig:SummaryRowSetting>
                                    <ig:SummaryRowSetting ColumnKey="ItemType" 
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

                                     <ig:SummaryRowSetting ColumnKey="SO_ContiguousNum" 
                                         EnableColumnSummaryOptions="False"  ShowSummaryButton="False">
                                    </ig:SummaryRowSetting>
                                     <ig:SummaryRowSetting ColumnKey="SalesItemNum" 
                                         EnableColumnSummaryOptions="False"  ShowSummaryButton="False">
                                    </ig:SummaryRowSetting>
                                     <ig:SummaryRowSetting ColumnKey="CustomerCode" 
                                         EnableColumnSummaryOptions="False"  ShowSummaryButton="False">
                                    </ig:SummaryRowSetting>
                                     <ig:SummaryRowSetting ColumnKey="PO_UnitPrice" 
                                         EnableColumnSummaryOptions="False"  ShowSummaryButton="False">
                                    </ig:SummaryRowSetting>
                                    <ig:SummaryRowSetting ColumnKey="Comment" 
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

                  </Behaviors>

              </ig:WebDataGrid>


 
        <asp:Panel ID="pnlMsgLocal" runat="server" BackColor="White" Width="750px" Style="display:none "
            meta:resourcekey="pnlCommentResource1">
            <div style="padding: 20px">
                <center>
                    <br />
                    <asp:Label ID="lblMsgLocal" runat="server" Font-Bold="True" 
                        Font-Names="Arial" Font-Size="11pt"
                        ForeColor="#996633" Text="Message"></asp:Label>
                    <br />
                    <br />
                    <br />
                    <asp:ImageButton ID="cmdOK" runat="server" ImageUrl="~/App_Themes/Buttons/Ok.gif"
                        meta:resourceKey="cmdOKStatusResource1" />
                    <br />
                    <br />
                </center>
            </div>
        </asp:Panel>
        <cc1:ModalPopupExtender ID="ModalPopupExtenderMsgLocal" TargetControlID="pnlMsgLocal" CancelControlID="cmdOk"
            BackgroundCssClass="modalBackground" DropShadow="True" OnCancelScript="HideModalPopupMsg()"
            PopupControlID="pnlMsgLocal" runat="server" DynamicServicePath="" Enabled="True" >
        </cc1:ModalPopupExtender>
       
             <asp:Panel ID="pnlInputBox" runat="server" BackColor="White" Width="750px" Style="display:none">
            <div style="padding: 20px">
                <center>
                    <br />
                    <asp:Label ID="lblInputMessage" runat="server" Font-Bold="True" 
                        Font-Names="Arial" Font-Size="11pt"
                        ForeColor="#996633" Text="Please enter a comment to explain the late planning for order: "></asp:Label>
                    <br />
                    <asp:TextBox ID="txtInputbox" runat="server" Width ="200" ></asp:TextBox>
                    <br />
                    <br />
                    <asp:ImageButton ID="cmdInputOK" runat="server" ImageUrl="~/App_Themes/Buttons/Ok.gif"
                        meta:resourceKey="cmdOKStatusResource1" />
                    <br />
                    <br />
                </center>
            </div>
        </asp:Panel>
        <cc1:ModalPopupExtender ID="ModalPopupExtenderInput" TargetControlID="pnlInputBox" CancelControlID="cmdOk"
            BackgroundCssClass="modalBackground" DropShadow="True" OnCancelScript="HideModalPopupInput()"
            PopupControlID="pnlInputBox" runat="server" DynamicServicePath="" Enabled="True" >
        </cc1:ModalPopupExtender>


              <br />
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
               <asp:Button ID="btnSaveDateChanges" runat="server" CssClass="VT_ActionButton" Text="Save" />
          &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
               <asp:Button ID="btnSendCIMLoad" runat="server" CssClass="VT_ActionButton" Text="Complete Planning" />
          &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
               <asp:Button ID="btnExport" runat="server" CssClass="VT_ActionButton" Text="Export to Excel" />
               &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;

        </ContentTemplate>

    </asp:UpdatePanel>
                                     

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
            <td >
                                &nbsp;&nbsp; <asp:Label ID="Label5" runat="server" Font-Bold="True" Font-Names="Arial" fontsize ="11px"
                                        ForeColor="Black" Text="Sales Order Details " 
                                        Width="180px" Font-Size="12pt"></asp:Label> <asp:Label ID="Label6" runat="server" Font-Bold="True" Font-Names="Arial" fontsize ="11px"
                                        ForeColor="Black" Text="( ALL Status except   'Open - Awaiting Planning' )" 
                                        Width="340px" Font-Size="10pt"></asp:Label></td>
                 </tr>
      <tr>
          <td>

       <asp:UpdatePanel id="ItemsPlannedUpdatePanel" runat="server" UpdateMode="Conditional" >
    
        <ContentTemplate>

              <ig:WebDataGrid ID="wdgOpenOtherStatus" runat="server"
                  AltItemCssClass="VerifyGrid_Report_AlternateRowSmall"
                  AutoGenerateColumns="False" Font-Names="Arial"
                  ItemCssClass="VerifyGrid_Report_RowSmall"
                  EnableTheming="True" FooterCaptionCssClass="VerifyGrid_Report_Footer"
                  HeaderCaptionCssClass="VerifyGrid_Report_HeaderBeigeSmall" BackColor="White"
                                    CssClass="VerifyGrid_Report_Frame" Height="280px">
                  
                  <Columns>

                      <ig:BoundDataField DataFieldName="SO_ContiguousNum" Key="SO_ContiguousNum" Width="70px"
                          DataType="System.String">
                          <Header Text="Order No:" />
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
                      <ig:BoundDataField DataFieldName="PlanningComment" Key="PlanningComment" Width="200px"
                          DataType="System.String">
                          <Header Text="Prod. Eng. Comment:" />
                      </ig:BoundDataField>
                      <ig:BoundDataField DataFieldName="ProductCode" Key="ProductCode" Width="80px"
                          DataType="System.String">
                          <Header Text="Code:" />
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
                          <Header Text="Sys. Num:" />
                      </ig:BoundDataField>
                      
                  </Columns>
                  <Behaviors>
                      <ig:EditingCore>
                          <Behaviors>
                              <ig:CellEditing Enabled="true">
                                  <ColumnSettings>

                                      
                                     <ig:EditingColumnSetting ColumnKey="SalesOrderNum" ReadOnly="True" />
                                      <ig:EditingColumnSetting ColumnKey="Other_CustPO" ReadOnly="True" />
                                      <ig:EditingColumnSetting ColumnKey="SoldTo_Name" ReadOnly="True" />
                                      <ig:EditingColumnSetting ColumnKey="Site" ReadOnly="True" />
                                      <ig:EditingColumnSetting ColumnKey="ProductCode" ReadOnly="True" />
                                      <ig:EditingColumnSetting ColumnKey="DateCreated" ReadOnly="True" />
                                      <ig:EditingColumnSetting ColumnKey="Item_DateRequested" ReadOnly="True" />
                                       <ig:EditingColumnSetting ColumnKey="Item_DateOut" ReadOnly="True" />
                                       <ig:EditingColumnSetting ColumnKey="Item_DateArrival" ReadOnly="True" />
                                      
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
                                       <ig:SummaryRowSetting ColumnKey="Other_CustPO" 
                                         EnableColumnSummaryOptions="False"  ShowSummaryButton="False">
                                    </ig:SummaryRowSetting>
                                    <ig:SummaryRowSetting ColumnKey="SoldTo_Name" 
                                         EnableColumnSummaryOptions="False"  ShowSummaryButton="False">
                                    </ig:SummaryRowSetting>
                                    <ig:SummaryRowSetting ColumnKey="Site" 
                                         EnableColumnSummaryOptions="False"  ShowSummaryButton="False">
                                    </ig:SummaryRowSetting>
                                    <ig:SummaryRowSetting ColumnKey="PlanningComment" 
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

                  </Behaviors>

              </ig:WebDataGrid>

                      </ContentTemplate>

    </asp:UpdatePanel>

          </td>
      </tr>
        
     
      <tr>
          <td>

              
             
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
                    <ig:BoundDataField DataFieldName="ViewHistory" Key="ViewHistory"  Width="50px" Hidden ="true" > 
                        <Header Text="ViewHistory:"/>
                     </ig:BoundDataField>
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
                     <ig:BoundDataField DataFieldName="Site" Key="Site"  Width="60px" >
                        <Header Text="Site:"/>
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
      <ig:BoundDataField DataFieldName="MachineNum" Key="MachineNum" Width="100px" 
                          DataType="System.String">
                          <Header Text="Machine Num:" />
                      </ig:BoundDataField>
                       <ig:BoundCheckBoxField DataFieldName="Kanban" Key="Kanban" Width="50px" ToolTipChecked="This is a kanban item" ToolTipUnchecked="This is NOT a kanban item" CssClass="Center" ToolTipPartial="Click here to mark this item as kanban">
                <CheckBox CheckedImageUrl="../App_Themes/Grid Buttons/Paid.gif"></CheckBox>
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

 
 <ig:EditingCore>
                <Behaviors>

                    <ig:CellEditing Enabled="true">
                        <EditModeActions  EnableF2="true" EnableOnActive="true" MouseClick="Single"  />
                       

                        <ColumnSettings>
                            <ig:EditingColumnSetting ColumnKey="PriceDifference" ReadOnly="True" />
                            <ig:EditingColumnSetting ColumnKey="ProductCode" ReadOnly="True" />
                            <ig:EditingColumnSetting ColumnKey="ProductName" ReadOnly="True" />
                             <ig:EditingColumnSetting ColumnKey="Item_QuoteReference" ReadOnly="True" />
                           
                            <ig:EditingColumnSetting ColumnKey="View" ReadOnly="True" />
                            <ig:EditingColumnSetting ColumnKey="ProductId" ReadOnly="True" />
                        </ColumnSettings>
                        
                         
                    </ig:CellEditing>
                </Behaviors>
            </ig:EditingCore>
    
 </Behaviors>
                </ig:WebDataGrid>  
                



<ig:WebDataGrid ID="wdgServiceItems" runat="server" DataKeyFields="VT_UniqueIndex"
                  
                   AltItemCssClass="VerifyGrid_Report_AlternateRow" 
    AutoGenerateColumns="False" Font-Names="Arial" 
        ItemCssClass="VerifyGrid_Report_Row" 
        EnableTheming="True" FooterCaptionCssClass="VerifyGrid_Report_Footer" 
        HeaderCaptionCssClass="VerifyGrid_Report_LightBlue_10pt" BackColor="White" 
        CssClass="VerifyGrid_Report_Frame" Height="4px" Width="4px" EnableAjax="False">
                    
 <Columns>
 

 <ig:GroupField Key="GroupField_Product">
                <Columns>
                    <ig:BoundDataField DataFieldName="ProductCode" Key="ProductCode"  Width="80px" > 
                        <Header Text="Code:"/>
                    </ig:BoundDataField>
                    <ig:BoundDataField DataFieldName="ProductName" Key="ProductName"  Width="200px" >
                        <Header Text="Name:"/>
                   </ig:BoundDataField>
                        </Columns>
            <Header Text="Service"/>
         </ig:GroupField>

 <ig:GroupField Key="GroupField_Amount">
                <Columns>
                    <ig:BoundDataField DataFieldName="Quantity" Key="Quantity"  Width="50px" CssClass="RAlign" DataType="System.Int32" Hidden="True">
                        <Header Text="Qty:"/>
                    </ig:BoundDataField>
                    <ig:BoundDataField DataFieldName="Weight" Key="Weight"  Width="50px" CssClass="RAlign" DataType="System.Double"  >
                        <Header Text="Amt:"/>
                    </ig:BoundDataField>

                        </Columns>
            <Header Text="Amount"/>
         </ig:GroupField>

 <ig:GroupField Key="GroupField_Pricing">
                <Columns>
                    <ig:BoundDataField DataFieldName="UnitPrice" Key="UnitPrice" Width="70px" CssClass="RAlign" DataType="System.Double">
                        <Header Text="Unit:"/>
                    </ig:BoundDataField>
                    <ig:BoundDataField DataFieldName="TotalExclVAT" Key="TotalExclVAT"  Width="70px" CssClass="RAlign" DataType="System.Double"  >
                        <Header Text="Price:"/>
                     </ig:BoundDataField>
                    <ig:BoundDataField DataFieldName="VAT" Key="VAT" Width="70px" CssClass="RAlign" DataType="System.Double">
                        <Header Text="Tax:"/>
                    </ig:BoundDataField>
                    <ig:BoundDataField DataFieldName="TotalPrice" Key="TotalPrice" Width="70px"  CssClass="RAlign" DataType="System.Double">
                        <Header Text="Total:"/>
                    </ig:BoundDataField>
                        </Columns>
            <Header Text="Pricing"/>
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
             <Header Text="Comment"/>
        </ig:BoundDataField>

 <ig:BoundDataField DataFieldName="ProductId" Hidden="True" 
            Key="ProductId" Width="30px" CssClass="RAlign">
            <Header Text="ProductId"/>
        </ig:BoundDataField>
 <ig:BoundDataField DataFieldName="OrderServiceItemId" Hidden="True" 
                Key="OrderServiceItemId" Width="30px" CssClass="RAlign">
            <Header Text="OrderItemId"/>
        </ig:BoundDataField>
 <ig:BoundDataField DataFieldName="VATRate" Hidden="False" 
            Key="VATRate" Width="80px" CssClass="RAlign">
            <Header Text="Tax Rate:"/>
        </ig:BoundDataField>
 <ig:BoundDataField DataFieldName="TraceCodeDesc" Hidden="True" 
            Key="TraceCodeDesc" Width="30px" >
            <Header Text="TraceCode"/>
        </ig:BoundDataField>
 <ig:BoundDataField Hidden="True" Key="LocationId" DataFieldName="LocationId" 
                Width="30px" >
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


 </Columns>

 <Behaviors>
 <ig:Filtering Enabled = "true" FilterButtonCssClass="Filter_LAlign">
            
            <EditModeActions EnableOnKeyPress="True" />
        </ig:Filtering >
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
                        EnableColumnSummaryOptions="True" EnableFormatting="True" FormatString="{1}" 
                        ShowSummaryButton="False">
                    </ig:SummaryRowSetting>
                    <ig:SummaryRowSetting ColumnKey="VAT" 
                        EnableColumnSummaryOptions="True" EnableFormatting="True" FormatString="{1}" 
                        ShowSummaryButton="False">
                    </ig:SummaryRowSetting>
                    <ig:SummaryRowSetting ColumnKey="TotalPrice" 
                        EnableColumnSummaryOptions="True" EnableFormatting="True" FormatString="{1}" 
                        ShowSummaryButton="False">
                    </ig:SummaryRowSetting>

                     <ig:SummaryRowSetting ColumnKey="ViewHistory" 
                        EnableColumnSummaryOptions="False"  ShowSummaryButton="False">
                    </ig:SummaryRowSetting>
                    <ig:SummaryRowSetting ColumnKey="ProductCode" 
                        EnableColumnSummaryOptions="False"  ShowSummaryButton="False">
                    </ig:SummaryRowSetting>
                     <ig:SummaryRowSetting ColumnKey="ProductName" 
                        EnableColumnSummaryOptions="False"  ShowSummaryButton="False">
                    </ig:SummaryRowSetting>
                    <ig:SummaryRowSetting ColumnKey="Quantity" 
                        EnableColumnSummaryOptions="False"  ShowSummaryButton="False">
                    </ig:SummaryRowSetting>
                    <ig:SummaryRowSetting ColumnKey="Weight" 
                        EnableColumnSummaryOptions="False"  ShowSummaryButton="False">
                    </ig:SummaryRowSetting>
                     <ig:SummaryRowSetting ColumnKey="UnitPrice" 
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

                    <ig:SummaryRowSetting ColumnKey="Comment" 
                        EnableColumnSummaryOptions="False"  ShowSummaryButton="False">
                    </ig:SummaryRowSetting>
                    <ig:SummaryRowSetting ColumnKey="VATRate" 
                        EnableColumnSummaryOptions="False"  ShowSummaryButton="False">
                    </ig:SummaryRowSetting>


                </ColumnSettings>
            </ig:SummaryRow>

<ig:ColumnResizing>
        </ig:ColumnResizing>

 <ig:Sorting Enabled="True" 
            AscendingImageUrl="~/App_Themes/Buttons/Up_Arrow_small.gif" 
            DescendingImageUrl="~/App_Themes/Buttons/Down_Arrow_small.gif" >
        </ig:Sorting>
 <ig:Selection CellClickAction="Cell" RowSelectType="Single">
            <AutoPostBackFlags RowSelectionChanged="True" />
        </ig:Selection>
 <ig:EditingCore>
                <Behaviors>
                    <ig:CellEditing Enabled="true">
                        
                        <ColumnSettings>
                            <ig:EditingColumnSetting ColumnKey="Quantity" />
                            <ig:EditingColumnSetting ColumnKey="Weight" />
                            <ig:EditingColumnSetting ColumnKey="UnitPrice" />
                            <ig:EditingColumnSetting ColumnKey="TotalExclVAT" />
                            <ig:EditingColumnSetting ColumnKey="VATCharged" />
                            <ig:EditingColumnSetting ColumnKey="TotalPrice" />
                            <ig:EditingColumnSetting ColumnKey="ProductCode" ReadOnly="True" />
                            <ig:EditingColumnSetting ColumnKey="ProductName" ReadOnly="True" />
                            
                            <ig:EditingColumnSetting ColumnKey="View" ReadOnly="True" />
                            <ig:EditingColumnSetting ColumnKey="ProductId" ReadOnly="True" />
                        </ColumnSettings>
                        <EditModeActions  MouseClick="Single"  />
                         
                    </ig:CellEditing>
                </Behaviors>
            </ig:EditingCore>
 </Behaviors>

                </ig:WebDataGrid>

          </td>
      </tr>
        
     
 </table>



                <ig:WebExcelExporter ID="VerifyExporter" runat="server">
        </ig:WebExcelExporter>

    </div>

 

</asp:Content>

