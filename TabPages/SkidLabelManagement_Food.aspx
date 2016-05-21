<%@ Page Title="" Language="VB" MasterPageFile="~/TabPages/ProjectMain.master" AutoEventWireup="false" CodeFile="SkidLabelManagement_Food.aspx.vb" Inherits="Other_Pages_SkidLabelManagement_Food" %>

 
<%@ Register Assembly="Infragistics4.Web.v13.2, Version=13.2.20132.2028, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" namespace="Infragistics.Web.UI.DisplayControls" tagprefix="ig" %>
<%@ Register Assembly="Infragistics4.Web.v13.2, Version=13.2.20132.2028, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" namespace="Infragistics.Web.UI.EditorControls" tagprefix="ig" %>
<%@ Register Assembly="Infragistics4.Web.v13.2, Version=13.2.20132.2028, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" namespace="Infragistics.Web.UI.GridControls" tagprefix="ig" %>
<%@ Register Assembly="Infragistics4.Web.v13.2, Version=13.2.20132.2028, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" namespace="Infragistics.Web.UI.ListControls" tagprefix="ig" %>


   
<asp:Content ID="Content1" ContentPlaceHolderID="ProjectMAIN_content" Runat="Server">
    <%--  <link href="../jg_styles.css" rel="stylesheet" type="text/css" />
    <link href="../Verify_Infragistics.css" rel="stylesheet" type="text/css" />
   --%>
    <script type="text/javascript" id="printComment">
        
        function HideSpotCheckModalPopup(sender, e) {
            
                  // now postback
            __doPostBack(sender, e);
            }



        function HandleKeyPress(control, allow, e) {
          
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
                var Barcode = SplitBarcode(ScannedData);

                if (Barcode.AI95 != '') {                    

                            var hdnSkidNumber = document.getElementById("<%=hdnSkidNumber.ClientID%>");
                    hdnSkidNumber.value = Barcode.AI95
                            __doPostBack(control.id, e);

                            return false;
                        }
                    }

                }

            
        
    
    </script>
    
    <link href="jg_styles.css" rel="stylesheet" type="text/css" />
    <link href="Verify_Infragistics.css" rel="stylesheet" type="text/css" />
    <div style="margin-left:10px; margin-top:10px">
        
        <table style="font-size: 10pt; width: 950px; font-family: Arial">
            <tr style="font-size: 10pt">
                <td align="left" valign="top">
                    <div style="margin-left:10px; margin-top:10px">
                        <table style="font-size: 10pt; width: 950px; font-family: Arial">
                            <tr style="font-size: 10pt">
                                <td align="center" valign="top">
                                    <asp:Label ID="Label1" runat="server" Font-Bold="True" Font-Names="Arial" 
                                        Font-Size="16pt" Text="Add skid to order " ForeColor="#996600"></asp:Label>
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                    <asp:ImageButton ID="btnBack" runat="server" 
                                        ImageUrl="~/App_Themes/Buttons/Back-Button.gif" />
                                </td>
                            </tr>
                            <tr style="font-size: 10pt">
                                <td valign="top" style="vertical-align: top; text-align: left; font-family: Arial, Helvetica, sans-serif; font-size: 10pt">
                                    &nbsp;</td>
                            </tr>
                            <tr style="font-size: 10pt">
                                <td valign="top" style="border: 1px solid #808080; vertical-align: top; text-align: left; font-family: Arial, Helvetica, sans-serif; font-size: 10pt">
                                    <table style="width: 100%">
                                        <tr>
                                            <td style="width: 80px">
                                                &nbsp;</td>
                                            <td style="width: 160px">
                                                &nbsp;</td>
                                            <td style="width: 70px">
                                                <asp:Label ID="Label8" runat="server" Font-Bold="False" Font-Names="Arial" 
                                                    Font-Size="11pt" Text="Product:"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblProductName_NS" runat="server" Font-Bold="True" Font-Names="Arial" 
                                                    Font-Size="11pt"></asp:Label>
                                            </td>
                                            <td style="width: 100px">
                                                &nbsp;</td>
                                            <td>
                                                &nbsp;</td>
                                            <td>&nbsp;</td>
                                            <td>&nbsp;</td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr style="font-size: 10pt">
                                <td valign="top" style="vertical-align: top; text-align: left; font-family: Arial, Helvetica, sans-serif; font-size: 10pt">
                                    &nbsp;
                                    &nbsp;</td>
                            </tr>
                            <tr style="font-size: 10pt">
                                <td valign="top" style="vertical-align: top; text-align: left; font-family: Arial, Helvetica, sans-serif; font-size: 10pt">
                                    <table style="width: 100%">
                                        <tr>
                                           
                                         
                                       
                                          
                                            <td>&nbsp;</td>
                                            <td style="width:200px">
                                                
                                                              <asp:Label ID="Label2" runat="server" Font-Bold="False" Font-Names="Arial" 
                                        Font-Size="12pt" Text="Scan Skid Label" ForeColor="Black"></asp:Label>
                                                
                                            </td>
                                               <td style="width:200px">
                                              
                                                
                                                       <asp:TextBox ID="txtSkidScan" runat="server"  onkeypress="return HandleKeyPress(this, 'alphanumeric', '');"></asp:TextBox>
                                                
                                            </td>
                                            <td>

                                            </td>
                                            <td>       &nbsp;</td>
                                            <td>
                                                &nbsp;</td>
                                        </tr>
                                        <tr>
                                           
                                          
                                            <td>&nbsp;</td>
                                             <td style="width:200px">
                                               <asp:Label ID="Label3" runat="server" Font-Bold="False" Font-Names="Arial" 
                                        Font-Size="12pt" Text="Or" ForeColor="Black"></asp:Label></td>
                                          <td style="width:200px">
                                                  &nbsp;</td>
                                            <td>

                                            </td>
                                                                                        <td>&nbsp;</td>
                                            <td>&nbsp;</td>
                                        </tr>
                                         <tr>
                                           
                                        
                                            <td>&nbsp;</td>
                                                <td style="width:200px">
                                                
                                               <asp:Label ID="Label25" runat="server" Font-Bold="False" Font-Names="Arial" 
                                        Font-Size="12pt" Text="Enter Skid Number" ForeColor="Black"></asp:Label>
                                                
                                            </td>
                                           <td style="width:200px">
                                                
                                                  <asp:TextBox ID="txtSkidNumber" runat="server"></asp:TextBox>
                                                   <asp:ImageButton ID="btnSearchForSkid" runat="server" 
                                     AlternateText="Print Label for Selected Item" 
                                     ImageUrl="~/App_Themes/Buttons/Search.gif" />
                                            </td>
                                             <td>
                                                                                  
                                    </td>
                                                                                         <td> &nbsp;</td>
                                            <td>       &nbsp;</td>
                                           
                                        </tr>

                                     
                                      
                                    </table>
                                </td>
                            </tr>
                            
                            
                            
                            </table>
                        <table style="width: 940px">
                            <tr>
                                <td style="width: 934px">
                                    &nbsp;</td>
                            </tr>
                            <tr>
                                <td style="width: 934px">
                                    &nbsp;
                                    &nbsp;<asp:Label ID="Label17" runat="server" Font-Bold="True" Font-Names="Arial" 
                                        Font-Size="12pt" Text="Skid Label Record" ForeColor="Black"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 934px">
                                    
                                
                                    <ig:WebDataGrid ID="wdgSkidinfo" runat="server" AltItemCssClass="VerifyGrid_Report_AlternateRow" AutoGenerateColumns="False" BackColor="White" CssClass="VerifyGrid_Report_Frame" EnableTheming="True" Font-Names="Arial" FooterCaptionCssClass="VerifyGrid_Report_Footer" HeaderCaptionCssClass="VerifyGrid_Report_HeaderGold" Height="180px" ItemCssClass="VerifyGrid_Report_Row" Width="913px">
                                        <Columns>
                                            <ig:BoundDataField DataFieldName="FormId" Key="FormId" Width="50px" Hidden="true">
                            <Header Text="Form Id:" />
                        </ig:BoundDataField>
                         <ig:BoundDataField DataFieldName="ProductionJobId" Key="ProductionJobId" Width="100px">
                            <Header Text="Job/Batch:" />
                        </ig:BoundDataField>
                        <ig:BoundDataField DataFieldName="SkidNumber" Key="SkidNumber" Width="150px">
                            <Header Text="Skid #:" />
                        </ig:BoundDataField>
                        <ig:BoundDataField DataFieldName="FormName" Key="FormName" Width="150px">
                            <Header Text="Case Serial #:" />
                        </ig:BoundDataField>
                        <ig:BoundDataField DataFieldName="JulianDate" Key="JulianDate" Width="150px">
                            <Header Text="Julian Date:" />
                        </ig:BoundDataField>
                         <ig:BoundDataField DataFieldName="QAInitials" Key="QAInitials" Width="150px">
                            <Header Text="QA Person:" />
                        </ig:BoundDataField>
                         <ig:BoundDataField DataFieldName="QADate" Key="QADate" Width="150px">
                            <Header Text="QA Date:" />
                        </ig:BoundDataField>
                                        </Columns>
                                        <Behaviors>
                                            <ig:Activation ActiveRowCssClass="SelectedRow">
                                            </ig:Activation>
                                         <%--   <ig:ColumnResizing>
                                            </ig:ColumnResizing>
                                            <ig:Filtering Enabled="true" FilterButtonCssClass="Filter_LAlign">
                                                <ColumnSettings>
                                                    <ig:ColumnFilteringSetting ColumnKey="FormName" Enabled="true" />
                                                </ColumnSettings>
                                                <ColumnFilters>
                                                    <ig:ColumnFilter ColumnKey="FormName">
                                                        <ConditionWrapper>
                                                            <ig:RuleTextNode />
                                                        </ConditionWrapper>
                                                    </ig:ColumnFilter>
                                                </ColumnFilters>
                                                <EditModeActions EnableOnKeyPress="True" />
                                            </ig:Filtering>
                                            <ig:Sorting AscendingImageUrl="~/App_Themes/Buttons/Up_Arrow_small.gif" DescendingImageUrl="~/App_Themes/Buttons/Down_Arrow_small.gif" Enabled="True">
                                            </ig:Sorting>--%>
                                            <ig:Selection CellClickAction="Row" RowSelectType="Multiple" Enabled="true" SelectedCellCssClass="SelectedRows">
                                            </ig:Selection>
                                           <%-- <ig:SummaryRow>
                                                <ColumnSummaries>
                                                    <ig:ColumnSummaryInfo ColumnKey="FormName">
                                                    </ig:ColumnSummaryInfo>
                                                </ColumnSummaries>
                                                <ColumnSettings>
                                                    <ig:SummaryRowSetting ColumnKey="FormId" ShowSummaryButton="False">
                                    </ig:SummaryRowSetting>
                                    <ig:SummaryRowSetting ColumnKey="ProductionJobId" ShowSummaryButton="False">
                                    </ig:SummaryRowSetting>
                                    <ig:SummaryRowSetting ColumnKey="SkidNumber" ShowSummaryButton="False">
                                    </ig:SummaryRowSetting>
                                    <ig:SummaryRowSetting ColumnKey="FormName" ShowSummaryButton="False">
                                    </ig:SummaryRowSetting>
                                    <ig:SummaryRowSetting ColumnKey="JulianDate" ShowSummaryButton="False">
                                    </ig:SummaryRowSetting>
                                    <ig:SummaryRowSetting ColumnKey="QAInitials" ShowSummaryButton="False">
                                    </ig:SummaryRowSetting>
                                    <ig:SummaryRowSetting ColumnKey="QADate" ShowSummaryButton="False">
                                    </ig:SummaryRowSetting>
                                                </ColumnSettings>
                                            </ig:SummaryRow>--%>
                                        </Behaviors>
                                    </ig:WebDataGrid>
                                    <br />
                                    
                                
  
                                
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 934px">
                                    &nbsp;</td>
                            </tr>
                            <tr>
                                <td>
                                        &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                                     &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                                        <asp:ImageButton ID="btnSelectAll" runat="server" ImageUrl="~/App_Themes/Buttons/SelectAll.gif" />
                                        <asp:ImageButton ID="btnAddSelected" runat="server" ImageUrl="~/App_Themes/Buttons/Add-Selected-Items.gif" />
                                </td>
                            </tr>
                        </table>
                        <br />
                    </div>
                </td>
            </tr>
        </table>
        <asp:HiddenField ID="hdnSkidNumber" runat ="server"  />
                                                                                                                                                                                                                                                                                                                                                                                                                             
           
        </div>

        </asp:Content>

