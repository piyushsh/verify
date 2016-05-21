<%@ Page Title="" Language="VB" MasterPageFile="~/TabPages/ProjectMain.master" AutoEventWireup="false" CodeFile="LatePlannedOrders.aspx.vb" Inherits="TabPages_LatePlannedOrders" %>

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
     function MessageUpdate(sender, e) {
         VTPostback(sender, e);
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

  

                <asp:Label ID="Label2" runat="server" Font-Bold="True" Font-Names="Arial" fontsize ="11px"
                                        ForeColor="Black" Text="Late planned orders" 
                                        Width="180px" Font-Size="12pt"></asp:Label> 
               <br />
               <asp:Label ID="Label3" runat="server" Font-Bold="True" Font-Size="Medium" ForeColor="#FF3300" Text="The orders that you have planned have been sent for CIM load but some were planned more than 20 working days later than the requested delivery date. Below is a list of the late planned orders. Please click on each item in the list to add a comment explaining the late planning. A reason must be given for each order. "></asp:Label>
               <br />
               <br />



              <ig:WebDataGrid ID="wdgOrdersGrid" runat="server"
                  DataKeyFields="VT_UniqueIndex"
                  AltItemCssClass="VerifyGrid_Report_AlternateRowSmall"
                  AutoGenerateColumns="False" Font-Names="Arial"
                  ItemCssClass="VerifyGrid_Report_RowSmall"
                  EnableTheming="True" FooterCaptionCssClass="VerifyGrid_Report_Footer"
                  HeaderCaptionCssClass="VerifyGrid_Report_HeaderGreenSmall" BackColor="White"
                  CssClass="VerifyGrid_Report_Frame" Height="280px">
                  
 

                  <Columns>

                      
                        <ig:BoundDataField DataFieldName="SO_ContiguousNum" Key="SO_ContiguousNum" Width="70px"
                            DataType="System.String">
                            <Header Text="Order No:" />
                        </ig:BoundDataField>
                      <ig:BoundDataField DataFieldName="SalesOrderNum" Key="SalesOrderNum" Width="80px" Hidden="true" 
                          DataType="System.String">
                          <Header Text="Purchase Order:" />
                      </ig:BoundDataField>
                      <ig:BoundDataField DataFieldName="Comment" Key="Comment" Width="800px"
                          DataType="System.String">
                          <Header Text="Late planning reason:" />
                      </ig:BoundDataField>
                     
                  </Columns>
                  <Behaviors>
    

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
   

                                    <ig:SummaryRowSetting ColumnKey="Comment" 
                                         EnableColumnSummaryOptions="False"  ShowSummaryButton="False">
                                    </ig:SummaryRowSetting>
                                   
                                     <ig:SummaryRowSetting ColumnKey="SO_ContiguousNum" 
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
                    <asp:TextBox ID="txtInputbox" runat="server" Width ="634px" Height="76px" TextMode="MultiLine" ></asp:TextBox>
                    <br />
                    <br />
                    <asp:ImageButton ID="cmdInputOK" runat="server" ImageUrl="~/App_Themes/Buttons/Ok.gif"
                        meta:resourceKey="cmdOKStatusResource1" />
                    <br />
                    <br />
                </center>
            </div>
        </asp:Panel>
        <cc1:ModalPopupExtender ID="ModalPopupExtenderInput" TargetControlID="pnlInputBox" CancelControlID="cmdInputOK"
            BackgroundCssClass="modalBackground" DropShadow="True" OnCancelScript="HideModalPopupInput()"
            PopupControlID="pnlInputBox" runat="server" DynamicServicePath="" Enabled="True" >
        </cc1:ModalPopupExtender>
            <asp:HiddenField ID="hdnSalesOrderNum" runat ="server" />

              <br />
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:ImageButton ID="btnBack1" runat="server" AlternateText="Back" ImageUrl="~/App_Themes/Billing/Buttons/Back.gif" ToolTip="Back to Sales Order tab" />
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:ImageButton ID="btnAddNewComment" runat="server" AlternateText="Add New Comment" ImageUrl="~/App_Themes/Billing/Buttons/Add-New-Comment.gif" ToolTip="Add a comment to this dashboard" />
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;


                                     

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
                                &nbsp;&nbsp; </td>
                 </tr>
      <tr>
          <td>

              &nbsp;</td>
      </tr>
        
     
      </table>



    </div>

 

</asp:Content>

