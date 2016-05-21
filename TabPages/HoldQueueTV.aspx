<%@ Page Title="" Language="VB"  AutoEventWireup="false" CodeFile="HoldQueueTV.aspx.vb"  EnableEventValidation="false" Inherits="TabPages_HoldQueueTV" %>

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
<body style="margin-top:0px" onbeforeunload="busyBox.Show();">

    <script type="text/javascript">
               

        var nTVIdleTime = new Number(0);
        var nTVPageIdleTime = new Number(0);
        var nTVRefreshTime = new Number(0);  //The timeout value in minutes
        var nTVPageTime = new Number(0); //the interval for paging in seconds
       
        $(document).ready(function () {
            
            //nTVRefreshTime = 2   //Refresh time in minutes
               
            //    TVtimerIncrement();
            //    //Increment the idle time counter every minute.
            //    var idleTVInterval = setInterval(TVtimerIncrement, 60000); // 1 minute
                        
            //  nTVRefreshTime = 10   //Refresh time in minutes
            nTVPageTime = 120 //time in seconds

            TVPagetimerIncrement();
            var idlePageInterval = setInterval(TVPagetimerIncrement, 1000); //1 second
            //  TVtimerIncrement();
            //Increment the idle time counter every minute.
            //    var idleTVInterval = setInterval(TVtimerIncrement, 60000); // 1 minute
            });

        function TVPagetimerIncrement() {

            var nTVCountDownValue = new Number(nTVPageTime - nTVPageIdleTime);

            if (nTVPageIdleTime >= nTVPageTime) { // nTimeout is the timeout limit value
                NextPage();
            }

            nTVPageIdleTime = nTVPageIdleTime + 1;
        }

        function NextPage() {
            var grid = $find("wdgRepGrid");

            nTVPageIdleTime = 0
            if (grid.get_behaviors().get_paging().get_pageIndex() < grid.get_behaviors().get_paging().get_pageCount() - 1) {

                grid.get_behaviors().get_paging().set_pageIndex(grid.get_behaviors().get_paging().get_pageIndex() + 1);
            }
            else {
                //go back to the first page
                grid.get_behaviors().get_paging().set_pageIndex(0);
              
            }
        }


            function TVtimerIncrement() {
                // if the TimeoutRunning flag is set increment the timer
                var nTVTimeoutRunning = 'YES'
                if (nTVTimeoutRunning == "YES") {
                    var nTVCountDownValue = new Number(nTVRefreshTime - nTVIdleTime);
                    
                    var Caption = document.getElementById("<%=lblRefresh.ClientID%>");
                    if (nTVCountDownValue > 1) {
                        Caption.innerText = 'Refresh in ' + nTVCountDownValue.toString() + ' minutes.';
                    }
                    else {
                        Caption.innerText = 'Refresh in ' + nTVCountDownValue.toString() + ' minute.';
                    }
                   
                                          
                    if (nTVIdleTime >= nTVRefreshTime) { // nTimeout is the timeout limit value
                        __doPostBack('', '');
                    }

                    nTVIdleTime = nTVIdleTime + 1;
                }


            }


        function OnHoldUpdate(sender, e) {
            VTPostback(sender, e);
        }

                        

        function HideMsgPopup() {
            var modal = $find("<%=ModalPopupExtenderMessage.BehaviorID%>");
            modal.hide();
        }

        function HideAssignPopup() {
            var modal = $find("<%=ModalPopupExtenderPersonAssign.BehaviorID%>");
             modal.hide();
         }


        function MessageUpdate(sender, e) {
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

           var modal = $find("<%=ModalPopupExtenderMessage.BehaviorID%>");
            modal.show();
            return false;
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

    

  <table width="100%">
        <tr>
            <td style="font-family: Arial; font-size: 8px; vertical-align: top; text-align: left;">
                                &nbsp;&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<table class="auto-style1">
                                    <tr>
                                        <td><asp:Image ID="Image1" runat="server" ImageUrl="~/App_Themes/Images/SteripackLogo_Small.jpg" />
                                        </td>
                                        <td>&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;</td>
                                        <td>
                                            <asp:Label ID="Label5" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="14pt" ForeColor="Black" Text="On-Hold queue TV   [ Sales Orders ]"></asp:Label>
                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                        </td>
                                        <td>
                                            <asp:Label ID="lblRefresh" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="11pt" ForeColor="#003300" Text="Refresh in 10 minutes" Visible="False"></asp:Label>
                                        </td>
                                        <td style="width: 200px">
                                            &nbsp; &nbsp;</td>
                                        <td>
                                 <asp:ImageButton ID="btnBack2" runat="server" AlternateText="back" ImageUrl="~/App_Themes/Billing/Buttons/Back.gif" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="font-family: Arial; font-size: 10px; font-weight: bold">&nbsp;</td>
                                        <td style="font-family: Arial; font-size: 10px; font-weight: bold">&nbsp;</td>
                                        <td style="font-family: Arial; font-size: 10pt; font-weight: bold">
                                            &nbsp;&nbsp;&nbsp;
                                            <asp:Label ID="Label10" runat="server" Font-Italic="True" ForeColor="#000099" Text="Number of Sales Orders:"></asp:Label>
&nbsp;<asp:Label ID="lblNumOrders" runat="server" Text="Label"></asp:Label>
                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                            <asp:Label ID="Label11" runat="server" Font-Italic="True" ForeColor="#993300" Text="Number of Order Items:" Visible="False"></asp:Label>
&nbsp;
                                            <asp:Label ID="lblNumItems" runat="server" Text="Label" Visible="False"></asp:Label>
                                        </td>
                                        <td style="font-family: Arial;  font-weight: bold">
                                            <asp:Label ID="Label12" runat="server" Font-Italic="True" ForeColor="#000099" Text="System hold - missing part:" Font-Size="10pt"></asp:Label>
                                            &nbsp;
                                            <asp:Label ID="lblMissingPart" runat="server" Text="Label" Font-Size="10pt"></asp:Label>
                                        &nbsp;&nbsp;&nbsp;
                                            <asp:Label ID="Label13" runat="server" Font-Italic="True" ForeColor="#000099" Text="missing price:" Font-Size="10pt"></asp:Label>
                                            &nbsp;
                                            <asp:Label ID="lblMissingPrice" runat="server" Text="Label" Font-Size="10pt"></asp:Label>
                                        </td>
                                        <td style="font-family: Arial; font-size: 10px; font-weight: bold;">
                                            &nbsp;</td>
                                        <td style="font-family: Arial; font-size: 10px; font-weight: bold">
                                            &nbsp;</td>
                                    </tr>
     <tr>
                                        <td style="font-family: Arial; font-size: 10px; font-weight: bold">&nbsp;</td>
                                        <td style="font-family: Arial; font-size: 10px; font-weight: bold">&nbsp;</td>
                                        <td style="font-family: Arial; font-size: 10pt; font-weight: bold">
                                            &nbsp;&nbsp;&nbsp;
                                            <asp:Label ID="Label1" runat="server" Font-Italic="True" ForeColor="#000099" Text="0-3 days"></asp:Label>
&nbsp;<asp:Label ID="lblzerotothree" runat="server" Text="Label"></asp:Label>
                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                            <asp:Label ID="Label3" runat="server" Font-Italic="True" ForeColor="#000099" Text="4-5 days"></asp:Label>
                                            
&nbsp;
                                            <asp:Label ID="lblfourtofive" runat="server" Text="Label"></asp:Label>
                                             &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                            <asp:Label ID="Label2" runat="server" Font-Italic="True" ForeColor="#000099" Text=">5 days"></asp:Label>
                                            &nbsp;
                                            <asp:Label ID="lblMoreThanFive" runat="server" Text="Label" ></asp:Label>
                                        </td>
                                        <td style="font-family: Arial; font-size: 10px; font-weight: bold">
                                            <asp:Label ID="Label14" runat="server" Font-Italic="True" ForeColor="#000099" Text="Manual hold:" Font-Size="10pt"></asp:Label>
                                            &nbsp;&nbsp;
                                            <asp:Label ID="lblManualHold" runat="server" Text="Label" Font-Size="10pt"></asp:Label>
                                        </td>
                                        <td style="font-family: Arial; font-size: 10px; font-weight: bold;">
                                            &nbsp;</td>
                                        <td style="font-family: Arial; font-size: 10px; font-weight: bold">
                                            &nbsp;</td>
                                    </tr>
                                </table>
            </td>
                 </tr>
        <tr>
            <td style="font-family: Arial; font-size: 8px">
                                &nbsp;</td>
                 </tr>
      <tr>
          <td style="border: 1px solid #808080; vertical-align: top; display: inline-block; align-content:center; text-align: center; display: block; float:inherit;">

              <div style="display:inline-block">

              
              <ig:WebDataGrid ID="wdgRepGrid" runat="server"
                  DataKeyFields="VT_UniqueIndex"
                  AltItemCssClass="VerifyGrid_Report_AlternateRowSmall"
                  AutoGenerateColumns="False" Font-Names="Arial"
                  ItemCssClass="VerifyGrid_Report_RowSmall"
                  EnableTheming="True" FooterCaptionCssClass="VerifyGrid_Report_Footer"
                  HeaderCaptionCssClass="VerifyGrid_Report_HeaderGreenSmall" BackColor="White"
                  CssClass="VerifyGrid_Report_Frame" EnableAjax="False">
                  
                
                  <Columns>

                      <ig:BoundDataField DataFieldName="SO_ContiguousNum" Key="SO_ContiguousNum" Width="70px"
                          DataType="System.String">
                          <Header Text="Order No:" />
                      </ig:BoundDataField>
                       <ig:BoundDataField Hidden="true"  DataFieldName="SalesItemNum" Key="SalesItemNum"  Width="50px" > 
                        <Header Text="Item:"/>
                     </ig:BoundDataField>

                      <ig:BoundDataField Hidden="true" DataFieldName="Other_CustPO" Key="Other_CustPO" Width="80px"
                          DataType="System.String">
                          <Header Text="Cust. P.O.:" />
                      </ig:BoundDataField>

                        <ig:BoundDataField DataFieldName="SoldTo_Name" Key="SoldTo_Name" Width="120px"
                          DataType="System.String" Hidden ="true" >
                          <Header Text="Customer:" />
                      </ig:BoundDataField>
                      <ig:BoundDataField DataFieldName="SoldTo_ContactName" Key="SoldTo_ContactName" Width="70px"
                          DataType="System.String" Hidden ="true" >
                          <Header Text="Cust contact:" />
                      </ig:BoundDataField>

                      <ig:BoundDataField DataFieldName="SoldTo_Code" Key="SoldTo_Code" Width="80px"
                          DataType="System.String">
                          <Header Text="Sold To:" />
                      </ig:BoundDataField>
                      <ig:BoundDataField Hidden="true" DataFieldName="Site" Key="Site" Width="60px"
                          DataType="System.String">
                          <Header Text="Site:" />
                      </ig:BoundDataField>

            <ig:GroupField Key="GroupField_PartNumbers" Hidden ="true" >
                 <Columns>
                        <ig:BoundDataField DataFieldName="ProductCode" Key="ProductCode" Width="80px"
                          DataType="System.String">
                          <Header Text="Steripack:" />
                      </ig:BoundDataField>
                     <ig:BoundDataField DataFieldName="CustomerCode" Key="CustomerCode" Width="120px"
                          DataType="System.String">
                          <Header Text="Customer:" />
                      </ig:BoundDataField>
                    </Columns>
            <Header Text="Part Numbers"/>
         </ig:GroupField>

            <ig:GroupField Key="GroupField_Dates">
                 <Columns>
                      <ig:BoundDataField DataFieldName="DateCreated" Key="DateCreated" Width="70px"
                          DataType="System.DateTime">
                          <Header Text="Ordered:" />
                      </ig:BoundDataField>
                      <ig:BoundDataField DataFieldName="Item_DateRequested" Key="Item_DateRequested" Width="85px"
                          DataType="System.DateTime" DataFormatString="{0:d}">
                          <Header Text="Required:" />
                      </ig:BoundDataField>
                     <ig:BoundDataField DataFieldName="DateOnHold" Key="DateOnHold" Width="95px">
                          <Header Text="# days on hold:" />
                      </ig:BoundDataField>
                      <ig:BoundDataField Hidden="True" DataFieldName="Item_DateOut" Key="Item_DateOut" Width="85px" DataFormatString="{0:d}" DataType="System.DateTime">
                          <Header Text="Ship Out:" />
                      </ig:BoundDataField>

                      <ig:BoundDataField Hidden="True" DataFieldName="Item_DateArrival" Key="Item_DateArrival" Width="85px" DataFormatString="{0:d}" DataType="System.DateTime">
                          <Header Text="Arrival:" />
                      </ig:BoundDataField>

                     </Columns>
            <Header Text="Dates"/>
         </ig:GroupField>

                       <ig:BoundDataField DataFieldName="ReasonOnHold" Key="ReasonOnHold" Width="350px" CssClass = "ColText_8pt">
                            <Header Text="On Hold Comment:" />
                        </ig:BoundDataField>


               <ig:GroupField Key="GroupField_Prices">
                 <Columns>
                      <ig:BoundDataField hidden="true" DataFieldName="QuantityRequested" Key="QuantityRequested" Width="80px"
                          DataType="System.Int32">
                          <Header Text="Quantity:" />
                      </ig:BoundDataField>
                       <ig:BoundDataField hidden="true" DataFieldName="PO_UnitPrice" Key="PO_UnitPrice" Width="70px" CssClass="RAlign" DataType="System.Double" DataFormatString ="{0:N3}">
                        <Header Text="PO Unit:"/>
                    </ig:BoundDataField>
                       <ig:BoundDataField hidden="true" DataFieldName="UnitPrice" Key="UnitPrice" Width="70px" CssClass="RAlign" DataType="System.Double" DataFormatString ="{0:N3}">
                        <Header Text="Sys. Unit:"/>
                    </ig:BoundDataField>
                    <ig:BoundDataField Hidden="true" DataFieldName="UnitDifference" Key="UnitDifference" Width="70px" CssClass="RAlign" DataType="System.Double" DataFormatString ="{0:N3}">
                        <Header Text="Difference:"/>
                    </ig:BoundDataField>
                      <ig:BoundDataField DataFieldName="Value" Key="Value" Width="70px" CssClass="RAlign" DataType="System.Double" DataFormatString ="{0:N3}">
                        <Header Text="Value:"/>
                    </ig:BoundDataField>
                      </Columns>
            <Header Text="Prices"/>
         </ig:GroupField>

              <ig:GroupField Key="GroupField_Update">
                 <Columns>
                       <ig:BoundDataField DataFieldName="Item_OnHoldPersonResponsible" Key="Item_OnHoldPersonResponsible" Width="140px"  >
                          <Header Text="Person Responsible"/>
                         </ig:BoundDataField>
                     <ig:BoundDataField DataFieldName="Item_OnHoldTVComment" Key="Item_OnHoldTVComment" Width="300px"  >
                            <Header Text="TV Comment"/>
                     </ig:BoundDataField>
                 </Columns>
            <Header Text="Update"/>
         </ig:GroupField>

                      <ig:BoundDataField Hidden="true" DataFieldName="DeliverTo_Code" Key="DeliverTo_Code" Width="90px"
                          DataType="System.String">
                          <Header Text="Ship To:" />
                      </ig:BoundDataField>
                      <ig:BoundDataField Hidden="true" DataFieldName="BillTo_Code" Key="BillTo_Code" Width="90px"
                          DataType="System.String">
                          <Header Text="Invoice To:" />
                      </ig:BoundDataField>

                     <ig:BoundDataField Hidden="True" DataFieldName="SalesOrderNum" Key="SalesOrderNum" Width="30px"  >
                            <Header Text="SalesOrderNum"/>
                    </ig:BoundDataField>

                     <ig:BoundDataField Hidden="True" DataFieldName="Item_OnHoldPersonResponsibleID" Key="Item_OnHoldPersonResponsibleID" Width="30px"  >
                        <Header Text="Item_OnHoldPersonResponsibleID"/>
                    </ig:BoundDataField>
                      
                  </Columns>





                  <Behaviors>
                      <ig:EditingCore>
                          <Behaviors>
                              <ig:CellEditing Enabled="true">
                                  <ColumnSettings>

                                      <ig:EditingColumnSetting ColumnKey="Item_DateOut" ReadOnly="True" />
                                      <ig:EditingColumnSetting ColumnKey="Item_DateArrival" ReadOnly="True"  />

                                     <ig:EditingColumnSetting ColumnKey="SO_ContiguousNum" ReadOnly="True" />
                                      <ig:EditingColumnSetting ColumnKey="Other_CustPO" ReadOnly="True" />
                                      <ig:EditingColumnSetting ColumnKey="SoldTo_Name" ReadOnly="True" />
                                      <ig:EditingColumnSetting ColumnKey="Site" ReadOnly="True" />
                                      <ig:EditingColumnSetting ColumnKey="ProductCode" ReadOnly="True" />
                                      <ig:EditingColumnSetting ColumnKey="DateCreated" ReadOnly="True" />
                                      <ig:EditingColumnSetting ColumnKey="Item_DateRequested" ReadOnly="True" />
                                      <ig:EditingColumnSetting ColumnKey="DateOnHold" ReadOnly="True" />
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
   
                                    <ig:SummaryRowSetting ColumnKey="SalesItemNum" 
                                         EnableColumnSummaryOptions="False"  ShowSummaryButton="False">
                                    </ig:SummaryRowSetting>
                                    <ig:SummaryRowSetting ColumnKey="Other_CustPO" 
                                         EnableColumnSummaryOptions="False"  ShowSummaryButton="False">
                                    </ig:SummaryRowSetting>
                                    <ig:SummaryRowSetting ColumnKey="SoldTo_Code" 
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
                              <ig:SummaryRowSetting ColumnKey="DateOnHold" 
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
                                    <ig:SummaryRowSetting ColumnKey="PO_UnitPrice" 
                                         EnableColumnSummaryOptions="False"  ShowSummaryButton="False">
                                    </ig:SummaryRowSetting>
                                    <ig:SummaryRowSetting ColumnKey="UnitDifference" 
                                         EnableColumnSummaryOptions="False"  ShowSummaryButton="False">
                                    </ig:SummaryRowSetting>
                                     <ig:SummaryRowSetting ColumnKey="UnitPrice" 
                                         EnableColumnSummaryOptions="False"  ShowSummaryButton="False">
                                    </ig:SummaryRowSetting>
                                    <ig:SummaryRowSetting ColumnKey="Item_OnHoldPersonResponsible" 
                                         EnableColumnSummaryOptions="False"  ShowSummaryButton="False">
                                    </ig:SummaryRowSetting>
                                     <ig:SummaryRowSetting ColumnKey="Item_OnHoldPersonResponsibleID" 
                                         EnableColumnSummaryOptions="False"  ShowSummaryButton="False">
                                    </ig:SummaryRowSetting>
                                    <ig:SummaryRowSetting ColumnKey="Item_OnHoldTVComment" 
                                         EnableColumnSummaryOptions="False"  ShowSummaryButton="False">
                                    </ig:SummaryRowSetting>

                                    <ig:SummaryRowSetting ColumnKey="DeliverTo_Code" 
                                         EnableColumnSummaryOptions="False"  ShowSummaryButton="False">
                                    </ig:SummaryRowSetting>
                                    <ig:SummaryRowSetting ColumnKey="BillTo_Code" 
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
                          <EditModeActions EnableOnKeyPress="True" />

                          <EditModeActions EnableOnKeyPress="True"></EditModeActions>
                      </ig:Filtering>
                      <ig:Sorting Enabled="True"
                          AscendingImageUrl="~/App_Themes/Buttons/Up_Arrow_small.gif"
                          DescendingImageUrl="~/App_Themes/Buttons/Down_Arrow_small.gif">
                      </ig:Sorting>
                       <ig:Paging PageSize="18" Enabled ="true" >
                      </ig:Paging>
                  </Behaviors>

              </ig:WebDataGrid>

                  </div>
              <br />
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
              &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
               &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:ImageButton ID="btnBack1" runat="server" AlternateText="Back" ImageUrl="~/App_Themes/Billing/Buttons/Back.gif" ToolTip="Back to ON-Hold Dashboard" />
               &nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
          </td>
      </tr>
        <tr>
 <td style="font-family: Arial; font-size: 8px">
                                &nbsp;&nbsp;</td>
        </tr>
              <tr>
 <td style="font-family: Arial; font-size: 8px">


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


