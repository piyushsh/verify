<%@ Page Title="" Language="VB"  AutoEventWireup="false" CodeFile="EditPriceList.aspx.vb"  EnableEventValidation="false" Inherits="EditPriceList" %>

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
    
<body style="margin-top:0px" >

    <script type="text/javascript">
               


        function ShowResendClose(sender, e) {
            __doPostBack(sender, e);
        }

        function HideMsgPopup() {
            var modal = $find("<%=ModalPopupExtender1.BehaviorID%>");
              modal.hide();
          }
          function HideMsgPopupMsg() {
              var modal = $find("<%=ModalPopupExtenderMsg.BehaviorID%>");
                  modal.hide();
              }
                         

    

 


        function MessageUpdate(sender, e) {
            VTPostback(sender, e);
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
            

     

    <div style="margin-left: 10px">

    

  <table>
        <tr>
            <td style="font-family: Arial; font-size: 8px; vertical-align: top; text-align: left;">
                                &nbsp;&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<table class="auto-style1">
                                    <tr>
                                        <td style="width: 250px">&nbsp;</td>
                                        <td>&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;</td>
                                        <td style="width: 600px">
                                       <table> <tr><td><asp:Label ID="Label9" runat="server" Font-Size="10pt" Text="Selected customer:"></asp:Label></td>
                                           <td><asp:Label ID="lblSelectedCust" runat="server" Font-Size="10pt" Font-Bold="True"></asp:Label></td></tr>
                                           <tr><td><asp:Label ID="Label3" runat="server" Font-Size="10pt" Text="Selected Product code:"></asp:Label></td>
                                               <td><asp:Label ID="lblSelectedProductCode" runat="server" Font-Size="10pt" Font-Bold="True"></asp:Label></td></tr>
                                             
                                       </table>
                                            
                                        </td>
                                        <td>
                                 <asp:ImageButton ID="btnBack2" runat="server" AlternateText="back" ImageUrl="~/App_Themes/Billing/Buttons/Back.gif" />
                                        &nbsp;<asp:Label ID="Label8" runat="server" Font-Names="Arial" Font-Size="10pt" Text="To On Hold dashboard"></asp:Label>
                                        </td>
                                        <td style="width: 200px">
                                            &nbsp; &nbsp;</td>
                                        <td>
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
          <td align="left" >

   
              <ig:WebDataGrid ID="wdgCustPriceBreaks" runat="server" AltItemCssClass="VerifyGrid_Report_AlternateRow" AutoGenerateColumns="False" BackColor="White" CssClass="VerifyGrid_Report_Frame" EnableTheming="True" Font-Names="Arial" FooterCaptionCssClass="VerifyGrid_Report_Footer" HeaderCaptionCssClass="VerifyGrid_Report_HeaderGold" Height="180px" ItemCssClass="VerifyGrid_Report_Row" Width="913px">
                  <Columns>
                      <ig:BoundDataField DataFieldName="CustomerName" Key="CustomerName" Width="140px">
                          <Header Text="Customer Name:" />
                      </ig:BoundDataField>
                      <ig:BoundDataField DataFieldName="CustomerCode" Key="CustomerCode" Width="80px">
                          <Header Text="Customer Code:" />
                      </ig:BoundDataField>
                      <ig:BoundDataField DataFieldName="ProductName" Key="ProductName" Width="80px">
                          <Header Text="Product Name:" />
                      </ig:BoundDataField>
                      <ig:BoundDataField DataFieldName="FromQty" Key="FromQty" Width="140px">
                          <Header Text="Qty greater than or equal to:" />
                      </ig:BoundDataField>
                      <ig:BoundDataField DataFieldName="Special_Price" Key="Special_Price" Width="80px">
                          <Header Text="Qty Price:" />
                      </ig:BoundDataField>
                      <ig:BoundDataField DataFieldName="QuoteReference" Key="QuoteReference" Width="300px">
                          <Header Text="Quote Ref:" />
                      </ig:BoundDataField>
                      <ig:BoundDataField DataFieldName="Comment" Key="Comment" Width="300px">
                          <Header Text="Comment:" />
                      </ig:BoundDataField>
                       <ig:BoundDataField DataFieldName="Currency" Key="Currency" Width="80px">
                                <Header Text="Currency:" />
                            </ig:BoundDataField>
                            <ig:BoundDataField DataFieldName="DateOfQuote" Key="DateOfQuote" Width="100px" DataFormatString="{0:d}" DataType="System.DateTime">
                                <Header Text="Date Of Quote:" />
                            </ig:BoundDataField>
                      <ig:BoundDataField DataFieldName="CustomerId" Hidden="true" Key="CustomerId" Width="140px">
                      </ig:BoundDataField>
                      <ig:BoundDataField DataFieldName="ProductId" Hidden="true" Key="ProductId" Width="140px">
                      </ig:BoundDataField>
                  </Columns>
                  <Behaviors>
                      <ig:Activation ActiveRowCssClass="SelectedRow">
                      </ig:Activation>
                      <ig:ColumnResizing>
                      </ig:ColumnResizing>
                      <ig:Filtering Enabled="true" FilterButtonCssClass="Filter_LAlign">
                          <ColumnSettings>
                              <ig:ColumnFilteringSetting ColumnKey="ProductName" Enabled="true" />
                          </ColumnSettings>
                          <ColumnFilters>
                              <ig:ColumnFilter ColumnKey="ProductName">
                                  <ConditionWrapper>
                                      <ig:RuleTextNode />
                                  </ConditionWrapper>
                              </ig:ColumnFilter>
                          </ColumnFilters>
                          <EditModeActions EnableOnKeyPress="True" />
                      </ig:Filtering>
                      <ig:Sorting AscendingImageUrl="~/App_Themes/Buttons/Up_Arrow_small.gif" DescendingImageUrl="~/App_Themes/Buttons/Down_Arrow_small.gif" Enabled="True">
                      </ig:Sorting>
                      <ig:Selection CellClickAction="Row" RowSelectType="Single" SelectedCellCssClass="SelectedRows">
                          <AutoPostBackFlags RowSelectionChanged="True" />
                      </ig:Selection>
                      <ig:SummaryRow>
                          <ColumnSummaries>
                              <ig:ColumnSummaryInfo ColumnKey="FromQty">
                              </ig:ColumnSummaryInfo>
                          </ColumnSummaries>
                          <ColumnSettings>
                              <ig:SummaryRowSetting ColumnKey="CustomerName" ShowSummaryButton="False">
                              </ig:SummaryRowSetting>
                              <ig:SummaryRowSetting ColumnKey="CustomerCode" ShowSummaryButton="False">
                              </ig:SummaryRowSetting>
                              <ig:SummaryRowSetting ColumnKey="ProductName" ShowSummaryButton="False">
                              </ig:SummaryRowSetting>
                              <ig:SummaryRowSetting ColumnKey="FromQty" ShowSummaryButton="False">
                              </ig:SummaryRowSetting>
                              <ig:SummaryRowSetting ColumnKey="Special_Price" ShowSummaryButton="False">
                              </ig:SummaryRowSetting>
                              <ig:SummaryRowSetting ColumnKey="Comment" ShowSummaryButton="False">
                              </ig:SummaryRowSetting>
                              <ig:SummaryRowSetting ColumnKey="QuoteReference" ShowSummaryButton="False">
                              </ig:SummaryRowSetting>
                          </ColumnSettings>
                      </ig:SummaryRow>
                  </Behaviors>
              </ig:WebDataGrid>
              <br />
<asp:ImageButton ID="btnAddPriceBreak" runat="server" ImageUrl="~/App_Themes/Buttons/Add-To-This-List.gif" />
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:ImageButton ID="btnEditPriceBreak" runat="server" ImageUrl="~/App_Themes/Buttons/Edit-Selected-Item.gif" />
              &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
               &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
              <asp:ImageButton ID="btnDeletePriceBreak" runat="server" ImageUrl="~/App_Themes/Buttons/Delete-Selected-Item.gif" />
              &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:ImageButton ID="btnBack1" runat="server" AlternateText="Back" ImageUrl="~/App_Themes/Billing/Buttons/Back.gif" />
               &nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
          </td>
      </tr>
        <tr>
 <td style="font-family: Arial; font-size: 8px">
                                &nbsp;&nbsp;</td>
        </tr>
              <tr>
 <td style="font-family: Arial; font-size: 8px">


                                 
                



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

              
             

                


              <asp:HiddenField ID="hdnAddEdit" runat="server" />

              
             

                


          </td>
      </tr>
        
     
      <tr>
          <td>

              
             

                
    <asp:Panel ID="pnlAddProductPricing" runat="server" Height="469px" Width="600px" 
        BackColor="White" style="display:none"  >
        <center>
            <br />
        <asp:Label ID="lblAddEditPrice" runat="server" Font-Bold="True" 
            Font-Size="13pt" Text="Add Product Pricing for the selected Customer" 
            Font-Names="Arial" style="text-align: center"></asp:Label>
        </center>
        <br />
        <table>
            <tr>
                <td class="auto-style3">
                    <center>
                    <table style="margin: 6px; padding: 6px; width: 572px;">
                        <tr>
                            <td style="width: 200px; vertical-align: top; text-align: right;">
                                <asp:Label ID="lblCustomer" runat="server" Font-Bold="True" Font-Names="Arial" 
                                    Font-Size="10pt">Product Category:</asp:Label>
                            </td>
                            <td class="auto-style12">
                                <asp:DropDownList ID="ddlProductCategory_NS" runat="server" 
                                    Width="255px">
                                </asp:DropDownList>
                            </td>
                            <td class="auto-style9">
                                &nbsp;</td>
                        </tr>
                        <tr>
                            <td style="width: 200px; vertical-align: top; text-align: right;">
                                <asp:Label ID="lbl2" runat="server" Font-Bold="True" Font-Names="Arial" 
                                    Font-Size="10pt">Product:</asp:Label>
                            </td>
                            <td class="auto-style13">
                                <asp:DropDownList ID="ddlProduct_NS" runat="server" Width="255px">
                                </asp:DropDownList>
                                    <asp:Label ID="lblProductName" runat="server" Font-Bold="True" Font-Names="Arial" 
                                    Font-Size="10pt"></asp:Label>
                            </td>
                            <td class="auto-style11">
                                &nbsp;</td>
                        </tr>
                        <tr>
                            <td style="width: 200px; vertical-align: top; text-align: right;">
                                <asp:Label ID="lblPriceType" runat="server" Font-Bold="True" Font-Names="Arial" 
                                    Font-Size="10pt">Price Type:</asp:Label>
                            </td>
                            <td style="vertical-align: top; text-align: left;" class="auto-style13">
                                <asp:DropDownList ID="ddlPriceType_NS" runat="server" Width="200px" onchange="PriceTypeChange();">
                                    <asp:ListItem Value="1">Price Break(s)</asp:ListItem>
                                    <asp:ListItem Value="2">Special Price (this product)</asp:ListItem>
                                       
                                </asp:DropDownList>
                            </td>
                            <td class="auto-style11">
                                &nbsp;</td>
                        </tr>
                           
                            <tr>
                            <td style="width: 200px; vertical-align: top; text-align: right;">
                                <asp:Label ID="lblQtyFrom" runat="server" Font-Bold="True" Font-Names="Arial" 
                                    Font-Size="10pt">Quantity (Greater than or equal to):</asp:Label>
                            </td>
                            <td style="vertical-align: top; text-align: left;" class="auto-style13" >
                                <asp:TextBox ID="txtQtyFrom"  runat="server"></asp:TextBox>
                            </td>
                            <td class="auto-style9">
                                &nbsp;</td>
                        </tr>
                        <tr>
                            <td style="vertical-align: top; text-align: right;" class="auto-style4">
                                <asp:Label ID="lbl3" runat="server" Font-Bold="True" Font-Names="Arial" 
                                    Font-Size="10pt">Price:</asp:Label>
                            </td>
                            <td style="vertical-align: top; text-align: left;" class="auto-style13">
                                &nbsp;<ig:WebNumericEditor ID="txtSpecialPrice_NS" runat="server" MaxDecimalPlaces="5">
                                </ig:WebNumericEditor>
                            </td>
                            <td class="auto-style11">
                                </td>
                        </tr>
                            <tr>
                            <td style="width: 200px; vertical-align: top; text-align: right;">
                                <asp:Label ID="Label1" runat="server" Font-Bold="True" Font-Names="Arial" 
                                    Font-Size="10pt">Quote Ref.:</asp:Label>
                            </td>
                            <td style="vertical-align: top; text-align: left;" class="auto-style13">
                                <ig:WebTextEditor ID="txtQuoteRef" runat="server" Width="300px">
                                </ig:WebTextEditor>
                            </td>
                            <td class="auto-style10">
                                &nbsp;</td>
                        </tr>
                            <tr>
                            <td style="width: 200px; vertical-align: top; text-align: right;">
                                <asp:Label ID="Label4" runat="server" Font-Bold="True" Font-Names="Arial" 
                                    Font-Size="10pt">Comment:</asp:Label>
                            </td>
                            <td style="vertical-align: top; text-align: left;" class="auto-style13">
                                <ig:WebTextEditor ID="txtComment" runat="server" Height="59px" TextMode="MultiLine" Width="247px">
                                </ig:WebTextEditor>
                            </td>
                            <td class="auto-style10">
                                &nbsp;</td>
                        </tr>
                         <tr>
                            <td class="auto-style20" style="vertical-align: top; text-align: right;">
                                <asp:Label ID="Label22" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="10pt">Currency:</asp:Label>
                            </td>
                            <td class="auto-style13" style="vertical-align: top; text-align: left;">
                                <asp:TextBox ID="txtCurrency" runat="server" Width="50px"></asp:TextBox>
                            </td>
                            <td class="auto-style10">&nbsp;</td>
                        </tr>
                        <tr>
                            <td class="auto-style20" style="vertical-align: top; text-align: right;">
                                <asp:Label ID="Label23" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="10pt">Date of Quote:</asp:Label>
                            </td>
                            <td class="auto-style13" style="vertical-align: top; text-align: left;">
                                <ig:WebDatePicker ID="wdpDateOfQuote" runat="server">
                                </ig:WebDatePicker>
                            </td>
                            <td class="auto-style10">&nbsp;</td>
                        </tr>
                    </table>
                    </center>
                    <br />
                    <center>
                    <table border="0" cellpadding="0" cellspacing="0" style="font-size: 10pt; width: 600px; font-family: Arial">
                        <tr>
                            <td align="left" valign="top" style="width: 50px">
                                &nbsp;&nbsp;</td>
                            <td align="left" valign="top">
                                <strong><span style="font-size: 14pt; font-family: Arial">
                                <asp:ImageButton ID="cmdCancel_ProdPrice" runat="server" 
                                    ImageUrl="~/App_Themes/Buttons/Cancel.gif" />
                                </span></strong>
                            </td>
                            <td align="left" valign="top">
                                <strong><span style="font-size: 14pt; font-family: Arial">
                                <asp:ImageButton ID="cmdSave_ProdPrice" runat="server" 
                                    ImageUrl="~/App_Themes/Billing/Buttons/Save-Button-small.gif" />
                                </span></strong>
                            </td>
                        </tr>
                    </table>
                    </center>

                    <br />
                </td>
            </tr>
        </table>
    </asp:Panel>
    <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender1" runat="server" 
        BackgroundCssClass="modalBackground" CancelControlID="cmdCancel_ProdPrice" 
        DropShadow="True" oncancelscript="HideMsgPopup()" PopupControlID="pnlAddProductPricing" 
        TargetControlID="pnlAddProductPricing">
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


