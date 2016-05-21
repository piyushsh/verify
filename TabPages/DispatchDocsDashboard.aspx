<%@ Page Title="" Language="VB"  AutoEventWireup="false" CodeFile="DispatchDocsDashboard.aspx.vb"  EnableEventValidation="false" Inherits="TabPages_DispatchDocsDashboard" %>

<%@ Register Assembly="Infragistics4.Web.v13.2, Version=13.2.20132.2028, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" namespace="Infragistics.Web.UI.DisplayControls" tagprefix="ig" %>
<%@ Register Assembly="Infragistics4.Web.v13.2, Version=13.2.20132.2028, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" namespace="Infragistics.Web.UI.EditorControls" tagprefix="ig" %>
<%@ Register Assembly="Infragistics4.Web.v13.2, Version=13.2.20132.2028, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" namespace="Infragistics.Web.UI.GridControls" tagprefix="ig" %>
<%@ Register Assembly="Infragistics4.Web.v13.2, Version=13.2.20132.2028, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" namespace="Infragistics.Web.UI.ListControls" tagprefix="ig" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
   <script type="text/javascript" src="<%= Session("_VT_JQueryFileLocation")%>Scripts/jquery-1.10.1.min.js"></script>


<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="../jg_styles.css" rel="stylesheet" type="text/css" />
    <link href="../Verify_Infragistics.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .auto-style1
        {
            width: 100%;
        }
    </style>
        <script language="javascript" type="text/javascript" src="../BusyBox.js"></script>

</head>
<body style="margin-top:0px" onbeforeunload="ShowSpinner();">

    <script type="text/javascript">
               

 
     
        function HideMsgPopupMsg() {
            var modal = $find("<%=ModalPopupExtenderMsg.BehaviorID%>");
                 modal.hide();
             }

        function HideMsgPopup() {
            var modal = $find("<%=ModalPopupExtenderMessage.BehaviorID%>");
            modal.hide();
        }
        function HideTaskPopup() {
            var modal = $find("<%=ModalPopupExtenderTask.BehaviorID%>");
              modal.hide();
          }
        function HideAssignPopup() {
            var modal = $find("<%=ModalPopupExtenderPersonAssign.BehaviorID%>");
             modal.hide();
         }


        function MessageUpdate(sender, e) {
            VTPostback(sender, e);
        }

        function AlertUpdate(sender, e) {
            VTPostback(sender, e);
        }

        function PersonAssignUpdate(sender, e) {

            var ddl = document.getElementById("<%=ddlUsers_NS.ClientID%>");

            if (ddl.options[ddl.selectedIndex].innerText == "Select User ...") {
                alert('You must select a User!');
                return false;
            }
            else
                VTPostback(sender, e);
        }

        function ShowMessageModalPopup() {
            var mycell = getActiveCell("<%=wdgRepGrid.ClientID%>");
            if (mycell == null) {
                alert('You must select a line first before you add or edit a comment!');
                return false
            }

            //check if the item is locked before popping up the panel
            //this check is done when the comment is being added
            //   return PageMethods.IsThisOrderLockedForEdit("60308", CallPageMethodSuccess, CallPageMethodFail) 
              
        }

        function CallPageMethodSuccess(result) {
  

            if (result == 'NO') {
                //the order is not locked so show the Coment panel
                var modal = $find("<%=ModalPopupExtenderMessage.BehaviorID%>");
                modal.show();
                return false;
            } else {
                //the order is locked so show the task panel
                var modal = $find("<%=ModalPopupExtenderTask.BehaviorID%>");
                modal.show();
                return false;
            }
        }

        // alert message on some failure
        function CallPageMethodFail(result) {
            alert(result.get_message());
        }

        function ShowPersonAssignModalPopup() {

            var mycell = getActiveCell("<%=wdgRepGrid.ClientID%>");
            if (mycell == null) {
                alert('You must select a line first before you can assign a person!');
                return false
            }
            var modal = $find("<%=ModalPopupExtenderPersonAssign.BehaviorID%>");
            modal.show();
            return false;
        }

 


       </script>




     <center>


    <form id="form1" runat="server">

        <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true" AsyncPostBackTimeout="360000">
                <Scripts>
                    <asp:ScriptReference Path="../Scripts/CommonScripts.js" />
                    <asp:ScriptReference Path="../Scripts/VT_Infragistics.js" />
                </Scripts>
            </asp:ScriptManager>

                <iframe id="BusyBox1" name="BusyBox1" frameborder="0" scrolling="no" ondrop="return false;">
                </iframe>
            

              <script language="javascript" type="text/javascript">

                  imagePath = "/" + '<%= PortalRootNode %>' + "/images/gears_ani_";
                var busyBox = new BusyBox("BusyBox1", "busyBox", 4, imagePath, ".gif", 125, 147, 207);
            </script>

    <div style="margin-left: 10px">

    <asp:HiddenField ID="hdnAlertUserId" runat="server" />
        <asp:HiddenField ID="hdnAlertJobId" runat="server" />
        <asp:HiddenField ID ="hdnAlertSONum" runat ="server" />

  <table id="MainTable" style="width: 1000px">
        <tr>
            <td style="font-family: Arial; font-size: 8px; vertical-align: top; text-align: left;">
                                &nbsp;&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<table class="auto-style1">
                                    <tr>
                                        <td style="width: 250px">&nbsp;</td>
                                        <td>&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;</td>
                                        <td style="width: 600px">
                                            <asp:Label ID="Label5" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="14pt" ForeColor="Black" Text="Dispatch Documents Management dashboard   [ Sales Orders ]"></asp:Label>
                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                        </td>
                                        <td>
                                 <asp:ImageButton ID="btnBack2" runat="server" AlternateText="back" ImageUrl="~/App_Themes/Billing/Buttons/Back.gif" />
                                        &nbsp;<asp:Label ID="Label8" runat="server" Font-Names="Arial" Font-Size="10pt" Text="To Sales Order (opening) tab"></asp:Label>
                                        </td>
                                        <td style="width: 200px">
                                            &nbsp; &nbsp;</td>
                                        <td>
                                            &nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td style="width: 250px">&nbsp;</td>
                                        <td>&nbsp;</td>
                                        <td style="width: 600px; font-family: Arial; font-size: 10pt;">
                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                            &nbsp;
                                            </td>
                                        <td>
                                            &nbsp;</td>
                                        <td style="width: 200px">
                                            &nbsp;</td>
                                        <td>
                                            &nbsp;</td>
                                    </tr>
                                </table>
            </td>
                 </tr>
        <tr>
            <td style="font-family: Arial; font-size: 8px">
                                &nbsp;
                                &nbsp;</td>
                 </tr>
      <tr>
          <td style="border: 1px solid #808080; vertical-align: top; display: inline-block; align-content:center; text-align: center; display: block; float:inherit;">

              <div style="display:inline-block">



              
                  </div>
              <ig:WebDataGrid ID="wdgRepGrid" runat="server"
                  DataKeyFields="VT_UniqueIndex"
                  AltItemCssClass="VerifyGrid_Report_AlternateRowSmall"
                  AutoGenerateColumns="False" Font-Names="Arial"
                  ItemCssClass="VerifyGrid_Report_RowSmall"
                  EnableTheming="True" FooterCaptionCssClass="VerifyGrid_Report_Footer"
                  HeaderCaptionCssClass="VerifyGrid_Report_HeaderGreenSmall" BackColor="White"
                  CssClass="VerifyGrid_Report_Frame" Height="540px">
                  
                
                 
                   <Columns>
                      <ig:TemplateDataField Key="ViewOrder" Width="30px" CssClass="Center">
                            <ItemTemplate>
                                <asp:ImageButton ID="btnViewOrder" runat="server" 
                                    ImageUrl="~/App_Themes/Grid Buttons/View.gif" onclick="btnViewOrder_Click" />
                            </ItemTemplate>
                            <Header Text="View:" />
                        </ig:TemplateDataField>

                <ig:GroupField Key="GroupField_OrderDetails">
                  <Columns>
                      

                      <ig:BoundDataField DataFieldName="SO_ContiguousNum" Key="SO_ContiguousNum" Width="60px"
                          DataType="System.String">
                          <Header Text="Order #" />
                      </ig:BoundDataField>
                       

                      <ig:BoundDataField DataFieldName="SoldTo_Category" Key="SoldTo_Category" Width="120px"
                          DataType="System.String">
                          <Header Text="Region:" />
                      </ig:BoundDataField>

                      
                      <ig:BoundDataField DataFieldName="SoldTo_Name" Key="SoldTo_Name" Width="120px"
                          DataType="System.String">
                          <Header Text="Customer:" />
                      </ig:BoundDataField>

                      
                       <ig:BoundDataField DataFieldName="DispatchDocNum" Key="DispatchDocNum" Width="120px"
                          DataType="System.String">
                          <Header Text="Shipment:" />
                      </ig:BoundDataField>

                      <ig:BoundDataField DataFieldName="Site" Key="Site" Width="55px"
                          DataType="System.String">
                          <Header Text="Container num:" />
                      </ig:BoundDataField>
                       </Columns>
                         <Header Text="Order details"/>
         </ig:GroupField>
            <ig:GroupField Key="GroupField_WorkItems">
                 <Columns>
                     <ig:BoundDataField DataFieldName="WorkItemNum" Key="WorkItemNum"  Width="30px" > 
                        <Header Text="Item:"/>
                     </ig:BoundDataField>
                     <ig:BoundDataField DataFieldName="ItemDesc" Key="ItemDesc" Width="100px" 
                          DataType="System.String">
                          <Header Text="Item Desc:" />
                      </ig:BoundDataField>
                         <ig:BoundDataField DataFieldName="RoleResp" Key="RoleResp" Width="80px" 
                          DataType="System.String">
                          <Header Text="Role Resp:" />
                      </ig:BoundDataField>

                     <ig:BoundDataField DataFieldName="Item_OnHoldPersonResponsible" Key="Item_OnHoldPersonResponsible" Width="65px"  >
                          <Header Text="Person Resp."/>
                         </ig:BoundDataField>

                        
                   

           
                    </Columns>
            <Header Text="Work item details"/>
         </ig:GroupField>

            

  <ig:GroupField Key="GroupField_Comments">
                 <Columns>
        <ig:TemplateDataField Key="ViewComments" Width="35px" CssClass="Center">
            <ItemTemplate>
                <asp:ImageButton ID="btnViewComments" runat="server" 
                    ImageUrl="~/App_Themes/Grid Buttons/View-Document-Set.gif" onclick="btnViewComments_Click" />
            </ItemTemplate>
            <Header Text="Cmt:" />
        </ig:TemplateDataField>
                <ig:BoundDataField DataFieldName="UnreadComments" Key="UnreadComments" Width="20px" CssClass = "ColText_8pt" >
                       <Header Text="!"  />
               </ig:BoundDataField>
           
               <ig:BoundDataField DataFieldName="ReasonOnHold" Key="ReasonOnHold" Width="150px" CssClass = "ColText_8pt">
                            <Header Text="On Hold Comment:" />
               </ig:BoundDataField>
                       <ig:BoundDataField DataFieldName="DashboardComment" Key="DashboardComment" Width="150px" CssClass = "ColText_8pt" >
                            <Header Text="Dashboard Comment"/>
                     </ig:BoundDataField>
              </Columns>
            <Header Text="Comments"/>
         </ig:GroupField>
  
<ig:GroupField Key="GroupField_Progress">
                 <Columns>
                      <ig:BoundDataField DataFieldName="ESTTime" Key="ESTTime" Width="60px" 
                          DataType="System.DateTime">
                          <Header Text="Est. Time:" />
                      </ig:BoundDataField>
                      <ig:BoundDataField DataFieldName="TimeUnits" Key="TimeUnits" Width="60px"
                          DataType="System.String" >
                          <Header Text="Time Units:" />
                      </ig:BoundDataField>
      

                      <ig:BoundDataField  DataFieldName="Item_DateStart" Key="Item_DateStart" Width="72px" DataFormatString="{0:d}" DataType="System.DateTime">
                          <Header Text="Start:" />
                      </ig:BoundDataField>

                      <ig:BoundDataField  DataFieldName="Item_DateFinish" Key="Item_DateFinish" Width="72px" DataFormatString="{0:d}" DataType="System.DateTime">
                          <Header Text="Finish:" />
                      </ig:BoundDataField>
                      <ig:BoundDataField  DataFieldName="Difference" Key="Difference" Width="40px"  >
                        <Header Text="Difference"/>
                    </ig:BoundDataField>
                      <ig:BoundDataField  DataFieldName="Status" Key="Status" Width="40px"  >
                        <Header Text="Status"/>
                    </ig:BoundDataField>
                     </Columns>
            <Header Text="Progress"/>
         </ig:GroupField>
                    

                   <ig:GroupField Key="GroupField_Prerequisites" >
                 <Columns>
                      <ig:BoundDataField DataFieldName="preReqItemNum" Key="preReqItemNum" Width="30px"
                          DataType="System.String">
                          <Header Text="ItemNum:" />
                      </ig:BoundDataField>
                       <ig:BoundDataField DataFieldName="preReqItemID" Key="preReqItemID" Width="30px" Hidden ="true"
                          DataType="System.String">
                          <Header Text="ItemNum:" />
                      </ig:BoundDataField>

                       <ig:BoundDataField DataFieldName="AlertTime" Key="AlertTime" Width="40px" CssClass="RAlign" DataType="System.String" >
                        <Header Text="Alert Time:"/>
                    </ig:BoundDataField>
                       <ig:BoundDataField DataFieldName="AlarmTime" Key="AlarmTime" Width="40px" CssClass="RAlign" DataType="System.String">
                        <Header Text="AlarmTime:"/>
                    </ig:BoundDataField>
               
                      </Columns>
            <Header Text="Prerequisites"/>
         </ig:GroupField>
                       <ig:BoundDataField Hidden="True" DataFieldName="Item_OnHoldPersonResponsibleID" Key="Item_OnHoldPersonResponsibleID" Width="30px"  >
                        <Header Text="Item_OnHoldPersonResponsibleID"/>
                    </ig:BoundDataField>

                     <ig:BoundDataField Hidden="True" DataFieldName="SalesOrderId" Key="SalesOrderId" Width="30px"  >
                        <Header Text="SalesOrderId"/>
                    </ig:BoundDataField>
                       <ig:BoundDataField Hidden="True" DataFieldName="SalesOrderItemId" Key="SalesOrderItemId" Width="30px"  >
                        <Header Text="SalesOrderItemId"/>
                    </ig:BoundDataField>
                      
                       <ig:BoundDataField Hidden="True" DataFieldName="JobItemId" Key="JobItemId" Width="30px"  >
                        <Header Text="JobItemId"/>
                    </ig:BoundDataField>
                  </Columns>

                  

                  <Behaviors>
                      <ig:EditingCore>
                          <Behaviors>
                              <ig:CellEditing Enabled="false">
                                  <ColumnSettings>

                                     

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
                              <ig:ColumnSummaryInfo ColumnKey="SO_ContiguousNum">
                                  <Summaries>
                                      <ig:Summary SummaryType="Count" />
                                  </Summaries>
                              </ig:ColumnSummaryInfo>
                          </ColumnSummaries>
  
                           <ColumnSettings>
                              
                                   <ig:SummaryRowSetting ColumnKey="SO_ContiguousNum" 
                                        EnableColumnSummaryOptions="True" EnableFormatting="True" FormatString=" {1}" 
                                        ShowSummaryButton="False">
                                    </ig:SummaryRowSetting>


                               <ig:SummaryRowSetting ColumnKey="SoldTo_Category" 
                                         EnableColumnSummaryOptions="False"  ShowSummaryButton="False">
                                    </ig:SummaryRowSetting>
                               <ig:SummaryRowSetting ColumnKey="SoldTo_Name"
 
                                         EnableColumnSummaryOptions="False"  ShowSummaryButton="False">
                                    </ig:SummaryRowSetting>
                               <ig:SummaryRowSetting ColumnKey="SoldTo_Address"

                                         EnableColumnSummaryOptions="False"  ShowSummaryButton="False">
                                    </ig:SummaryRowSetting>

                                  <ig:SummaryRowSetting ColumnKey="Site" 
                                         EnableColumnSummaryOptions="False"  ShowSummaryButton="False">
                                    </ig:SummaryRowSetting>

                                  <ig:SummaryRowSetting ColumnKey="DispatchDocNum" 
                                         EnableColumnSummaryOptions="False"  ShowSummaryButton="False">
                                    </ig:SummaryRowSetting>

                                  <ig:SummaryRowSetting ColumnKey="WorkItemNum" 
                                         EnableColumnSummaryOptions="False"  ShowSummaryButton="False">
                                    </ig:SummaryRowSetting>


                                     <ig:SummaryRowSetting ColumnKey="ITemDesc" 
                                         EnableColumnSummaryOptions="False"  ShowSummaryButton="False">
                                    </ig:SummaryRowSetting>
                                     <ig:SummaryRowSetting ColumnKey="RoleResp" 
                                         EnableColumnSummaryOptions="False"  ShowSummaryButton="False">
                                    </ig:SummaryRowSetting>
                                    <ig:SummaryRowSetting ColumnKey="Item_OnHoldPersonResponsible" 
                                         EnableColumnSummaryOptions="False"  ShowSummaryButton="False">
                                    </ig:SummaryRowSetting>
                                    <ig:SummaryRowSetting ColumnKey="ViewComments" 
                                         EnableColumnSummaryOptions="False"  ShowSummaryButton="False">
                                    </ig:SummaryRowSetting>
                                    <ig:SummaryRowSetting ColumnKey="UnreadComments" 
                                         EnableColumnSummaryOptions="False"  ShowSummaryButton="False">
                                    </ig:SummaryRowSetting>
                                    <ig:SummaryRowSetting ColumnKey="ReasonOnHold" 
                                         EnableColumnSummaryOptions="False"  ShowSummaryButton="False">
                                    </ig:SummaryRowSetting>
                                    <ig:SummaryRowSetting ColumnKey="DashboardComment" 
                                         EnableColumnSummaryOptions="False"  ShowSummaryButton="False">
                                    </ig:SummaryRowSetting>
                                    <ig:SummaryRowSetting ColumnKey="ESTTime" 
                                         EnableColumnSummaryOptions="False"  ShowSummaryButton="False">
                                    </ig:SummaryRowSetting>
                                     <ig:SummaryRowSetting ColumnKey="TimeUnits" 
                                         EnableColumnSummaryOptions="False"  ShowSummaryButton="False">
                                    </ig:SummaryRowSetting>
                                    <ig:SummaryRowSetting ColumnKey="Item_DateStart" 
                                         EnableColumnSummaryOptions="False"  ShowSummaryButton="False">
                                    </ig:SummaryRowSetting>
                                    <ig:SummaryRowSetting ColumnKey="Item_DateFinish" 
                                         EnableColumnSummaryOptions="False"  ShowSummaryButton="False">
                                    </ig:SummaryRowSetting>
                                    <ig:SummaryRowSetting ColumnKey="Difference" 
                                         EnableColumnSummaryOptions="False"  ShowSummaryButton="False">
                                    </ig:SummaryRowSetting>
                                    <ig:SummaryRowSetting ColumnKey="Status" 
                                         EnableColumnSummaryOptions="False"  ShowSummaryButton="False">
                                    </ig:SummaryRowSetting>
                                    <ig:SummaryRowSetting ColumnKey="preReqItemNum" 
                                         EnableColumnSummaryOptions="False"  ShowSummaryButton="False">
                                    </ig:SummaryRowSetting>
                                    <ig:SummaryRowSetting ColumnKey="AlertTime" 
                                         EnableColumnSummaryOptions="False"  ShowSummaryButton="False">
                                    </ig:SummaryRowSetting>
                              
                                     <ig:SummaryRowSetting ColumnKey="AlarmTime" 
                                         EnableColumnSummaryOptions="False"  ShowSummaryButton="False">
                                    </ig:SummaryRowSetting>
                                    <ig:SummaryRowSetting ColumnKey="ItemDesc" 
                                         EnableColumnSummaryOptions="False"  ShowSummaryButton="False">
                                    </ig:SummaryRowSetting>
                                <ig:SummaryRowSetting ColumnKey="ViewOrder" 
                                         EnableColumnSummaryOptions="False"  ShowSummaryButton="False">
                                    </ig:SummaryRowSetting>
                          </ColumnSettings>
                      </ig:SummaryRow>


                      <ig:Filtering Enabled="true" FilterButtonCssClass="Filter_LAlign">
                          <ColumnSettings>
                              <ig:ColumnFilteringSetting ColumnKey="SO_ContiguousNum" Enabled="true" />
                          </ColumnSettings>
                          <ColumnFilters>
                              <ig:ColumnFilter ColumnKey="SO_ContiguousNum">
                                  <ConditionWrapper>
                                      <ig:RuleTextNode />
                                  </ConditionWrapper>
                              </ig:ColumnFilter>
                          </ColumnFilters>
                                                    <EditModeActions EnableOnKeyPress="True"></EditModeActions>
                      </ig:Filtering>
                      <ig:Sorting Enabled="True"
                          AscendingImageUrl="~/App_Themes/Buttons/Up_Arrow_small.gif"
                          DescendingImageUrl="~/App_Themes/Buttons/Down_Arrow_small.gif">
                      </ig:Sorting>

                      <ig:Selection RowSelectType="Multiple">
                      </ig:Selection>

                  </Behaviors>

              </ig:WebDataGrid>

              <br />
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;<asp:ImageButton ID="btnExport" runat="server" AlternateText="Export to Excel" ImageUrl="~/App_Themes/Billing/Buttons/Export.GIF" ToolTip="Export to excel" />
                            
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
          <asp:ImageButton ID="btnViewPrices" runat="server" AlternateText="View Prices" ImageUrl="~/App_Themes/Buttons/View-PricesForProducts.gif" ToolTip="View/Edit price breaks" />
          &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;<asp:ImageButton ID="btnAddNewComment" runat="server" ImageUrl="~/App_Themes/Billing/Buttons/Add-New-Comment.gif" AlternateText="Add New Comment" ToolTip="Add a comment to this dashboard" />
               &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
               <asp:ImageButton ID="btnAssignToPerson" runat="server" ImageUrl="~/App_Themes/Billing/Buttons/Assign-To-Person.jpg" AlternateText="Assign to Person" ToolTip="Assign selected order to a person" />
          &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; <asp:ImageButton ID="btnOnHoldTV" runat="server" AlternateText="On-hold TV" ImageUrl="~/App_Themes/Billing/Buttons/OnHoldTV.gif" ToolTip="Activate ON-Hold TV view" Visible="False" />
               <asp:CheckBox ID="chkShowCustDetails" runat="server" Height="30px" Text="Show customer details on TV" Font-Names="Arial" Font-Size="9pt" Visible="False" />
              &nbsp;&nbsp;<asp:ImageButton ID="btnBack1" runat="server" AlternateText="Back" ImageUrl="~/App_Themes/Billing/Buttons/Back.gif" ToolTip="Back to Sales Order tab" />
               &nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
          </td>
      </tr>
        <tr>
 <td style="font-family: Arial; font-size: 8px">
                                &nbsp;&nbsp;</td>
        </tr>
              <tr>
 <td style="font-family: Arial; font-size: 8px">


                



   <ig:WebExcelExporter ID="VerifyExporter" runat="server">
        </ig:WebExcelExporter>




 </td>
        </tr>
      <tr>
          <td>
             <asp:ImageMap ID="ImageMap2" runat="server" ImageUrl="~/APP_THEMES/Footer/Footer-Verify.gif">
                                                    <asp:RectangleHotSpot Bottom="41" Left="2" NavigateUrl="Http://www.VerifyTechnologies.com"
                                                        Right="925" Target="_blank" Top="2" />
                                                </asp:ImageMap> 
          </td>
      </tr>
        
     
      <tr>
          <td>

              
             

                


          </td>
      </tr>
        
     
      <tr>
          <td>

              
             

                
               <asp:Panel ID="pnlTask" runat="server" backcolor="White" borderstyle="Solid" Style="display:none "
                    borderwidth="1px" Height="210px" width="900px">
                     <br />
                    <br />
                    <center>
                        <strong><span style="FONT-FAMILY: Arial">&nbsp; </span></strong><strong>
                        <span style="FONT-FAMILY: Arial"></span>
                        <asp:Label ID="lblTask" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="12pt" Text="Comment for Line Item"></asp:Label>
                        </strong>
                    </center>
                   
                    <center>
                        <br />
                        <asp:ImageButton ID="cmdTaskCancel" runat="server" ImageUrl="~/App_Themes/Billing/Buttons/Cancel.gif" meta:resourceKey="cmdCancelSaveResource1" />
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:ImageButton ID="cmdTaskOk" runat="server" 
                            ImageUrl="~/App_Themes/Billing/Buttons/ok.gif" />
                      
                    </center>
                    <center>
                    </center>
                    
                </asp:Panel>
                <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtenderTask" runat="server" 
                    BackgroundCssClass="modalBackground" CancelControlID="cmdTaskCancel" 
                    DropShadow="True" oncancelscript="HideTaskPopup()" PopupControlID="pnlTask" 
                    TargetControlID="pnlTask">
                </ajaxToolkit:ModalPopupExtender>



           <asp:Panel ID="pnlComment" runat="server" backcolor="White" borderstyle="Solid" Style="display:none "
                    borderwidth="1px" Height="210px" width="900px">
                     <br />
                    <br />
                    <center>
                        <strong><span style="FONT-FAMILY: Arial">&nbsp; </span></strong><strong>
                        <span style="FONT-FAMILY: Arial"></span>
                        <asp:Label ID="Label6" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="12pt" Text="Comment for Line Item"></asp:Label>
                        </strong>
                    </center>
                    <center>
                        <asp:TextBox ID="txtHoldTVComment_NS" runat="server" Height="50px" MaxLength="2000" TextMode="MultiLine" Width="600px" Font-Names="Arial" Font-Size="10pt"></asp:TextBox>
                        &nbsp;<br />
                        <br />
                    </center>
                    <center>
                        <br />
                        <asp:ImageButton ID="cmdCancelComment" runat="server" ImageUrl="~/App_Themes/Billing/Buttons/Cancel.gif" meta:resourceKey="cmdCancelSaveResource1" />
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:ImageButton ID="cmdSaveComment" runat="server" 
                            ImageUrl="~/App_Themes/Billing/Buttons/ok.gif" />
                      
                    </center>
                    <center>
                    </center>
                    
                </asp:Panel>
                <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtenderMessage" runat="server" 
                    BackgroundCssClass="modalBackground" CancelControlID="cmdCancelComment" 
                    DropShadow="True" oncancelscript="HideMsgPopup()" PopupControlID="pnlComment" 
                    TargetControlID="pnlComment">
                </ajaxToolkit:ModalPopupExtender>


             

                


          </td>
      </tr>
        
     
      <tr>
          <td>

 
           <asp:Panel ID="pnlPersonAssign" runat="server" backcolor="White" borderstyle="Solid" Style="display:none "
                    borderwidth="1px" Height="240px" width="900px">
                     <br />
                    <br />
                    <center>
                        <strong><span style="FONT-FAMILY: Arial">&nbsp; </span></strong><strong>
                        <span style="FONT-FAMILY: Arial"></span>
                        <asp:Label ID="Label7" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="12pt" Text="Assign Selected person to Line Item"></asp:Label>
                        </strong>
                    </center>
                    <center>
                        &nbsp;<table style="border: 1px solid #808080; width: 698px; font-family: Arial; font-size: 10pt; vertical-align: middle; text-align: center;">
                            <tr>
                                <td style="width: 144px; height: 26px;">
                                    <asp:Label ID="lblCategoryPrompt" runat="server" Font-Names="Arial" Font-Size="10pt" Text="Category"></asp:Label>
                                </td>
                                <td style="width: 110px; height: 26px;">
                                    <asp:DropDownList ID="ddlCategory_NS" runat="server" Width="191px">
                                    </asp:DropDownList>
                                    <ajaxToolkit:CascadingDropDown ID="CascadingDropDown3" runat="server" category="Category" loadingtext="Loading Categories ..." prompttext="Select Category .." servicemethod="GetPersonnelCategories" servicepath="~/ModuleServices.asmx" targetcontrolid="ddlCategory_NS">
                                    </ajaxToolkit:CascadingDropDown>
                                </td>
                                <td style="width: 101px; height: 26px;">
                                    <asp:Label ID="lblUserPrompt" runat="server" Font-Names="Arial" Font-Size="10pt" Text="User"></asp:Label>
                                </td>
                                <td style="height: 26px; text-align: left;">
                                    <asp:DropDownList ID="ddlUsers_NS" runat="server" Width="191px">
                                    </asp:DropDownList>
                                    <ajaxToolkit:CascadingDropDown ID="CascadingDropDown4" runat="server" category="Courses" loadingtext="Loading Users ..." parentcontrolid="ddlCategory_NS" prompttext="Select User ..." servicemethod="GetPersonnelInCategory" servicepath="~/ModuleServices.asmx" targetcontrolid="ddlUsers_NS">
                                    </ajaxToolkit:CascadingDropDown>
                                </td>
                            </tr>
                        </table>
                        <br />
                        <asp:DropDownList ID="ddlAssignToCust" runat="server">
                            <asp:ListItem Selected="True" Value="Assign">Assign permanently to customer</asp:ListItem>
                            <asp:ListItem Value="NOAssign">Assign only for this one PO</asp:ListItem>
                        </asp:DropDownList>
                        <br />
                    </center>
                    <center>
                        <br />
                        <asp:ImageButton ID="cmdPersonAssignCancel" runat="server" ImageUrl="~/App_Themes/Billing/Buttons/Cancel.gif" meta:resourceKey="cmdCancelSaveResource1" />
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:ImageButton ID="cmdSavePersonAssign" runat="server" 
                            ImageUrl="~/App_Themes/Billing/Buttons/ok.gif" />
                      
                    </center>
                    <center>
                    </center>
                    
                </asp:Panel>
                <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtenderPersonAssign" runat="server" 
                    BackgroundCssClass="modalBackground" CancelControlID="cmdPersonAssignCancel" 
                    DropShadow="True" oncancelscript="HideAssignPopup()" PopupControlID="pnlPersonAssign" 
                    TargetControlID="pnlPersonAssign">
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

                


          </td>
      </tr>
        
     
      <tr>
          <td>

              
             

                


              &nbsp;</td>
      </tr>
        
     
 </table>



    </div>

    </form>

         </center>
</body>
</html>


