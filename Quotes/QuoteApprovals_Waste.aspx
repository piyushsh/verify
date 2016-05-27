<%@ Page Title="" Language="VB" MasterPageFile="~/Quotes/FormsMaster.master" AutoEventWireup="false" CodeFile="QuoteApprovals_Waste.aspx.vb" Inherits="Quotes_QuoteApprovals_Waste" %>
<%@ MasterType TypeName= "MyMasterPage" %>
<%@ Register TagPrefix="igsch" Namespace="Infragistics.WebUI.WebSchedule" Assembly="Infragistics2.WebUI.WebDateChooser.v7.3, Version=7.3.20073.38, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>

<%@ Register assembly="Infragistics2.WebUI.UltraWebGrid.v7.3, Version=7.3.20073.38, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" namespace="Infragistics.WebUI.UltraWebGrid" tagprefix="igtbl1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="DetailsMaster_ContentPlaceHolder" Runat="Server">
 
    <asp:HiddenField ID="hdnWhichButton" runat="server" />


      <script type="text/javascript">


          function HidePasswordModalPopup() {
            var modal = $find("<%=ModalPopupExtenderPass.BehaviorID%>");
            modal.hide();
        }

        function Close(sender, e) {
            __doPostBack(sender, e);
        }

        function HideMsgPopup() {
            var modal = $find("<%=ModalPopupExtenderMsg.BehaviorID%>");
            modal.hide();
        }


        function ShowPasswordPanel(button) {
            //store which button was clicked in the hidden field
            var hdn = document.getElementById("<%=hdnWhichButton.ClientID%>");
            hdn.value = button;

            var txt = document.getElementById("<%=txtPassword.ClientID%>");
            txt.innerText = '';

            var modal = $find("<%=ModalPopupExtenderPass.BehaviorID%>");
            modal.show();
            txt.focus();
            return false;
        }




  </script>
 <p>
        <table style="width: 690px">
         
            <tr>
                <td>
                    <table style="font-size: 10pt; width: 690px; font-family: Arial">
                        <tr>
                            <td align="left" style=" height: 120px;" 
                                valign="top">
                                <span style="font-size: 11pt"><strong>
                                <asp:Label ID="Label7" runat="server" font-bold="True" font-names="Arial" 
                                    font-size="11pt" forecolor="#B38700" text="Tasks and Approvals" 
                                    width="205px"></asp:Label>
                                <br />
                                </strong>
                                <table style="width: 689px ">
                                    <tr>
                                        <td style="height: 21px">
                                <span style="font-size: 11pt">
                                                        <table style="width: 339px; border-right: darkgray 1px solid; border-top: darkgray 1px solid;
                border-left: darkgray 1px solid; border-bottom: darkgray 1px solid;">
                                                            <tr>
                                                                <td colspan="2">
                                <span style="font-size: 11pt"><strong>
                                <asp:Label ID="Label8" runat="server" font-bold="True" font-names="Arial" 
                                    font-size="11pt" forecolor="#B38700" text="Health &amp; Safety Approval" 
                                    width="205px"></asp:Label>
                                </strong>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                <span style="font-size: 11pt"><strong>
                                            <asp:Label ID="lblJobIdCaption10" runat="server" Font-Bold="True" 
                                                Font-Names="Arial" Font-Size="10pt" Width="87px">Comment:</asp:Label>
                                            </strong></span>
                                                                </td>
                                                                <td>
                                                                    &nbsp;</td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="2">
                                                                    <asp:TextBox ID="txtHSComment" runat="server" Height="68px" Width="279px"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:ImageButton ID="btnHSApprove" runat="server" 
                                                                        ImageUrl="~/App_Themes/Billing/Buttons/Approve.gif" />
                                                                </td>
                                                                <td>
                                                                    <asp:ImageButton ID="btnHSReject" runat="server" 
                                                                        ImageUrl="~/App_Themes/Billing/Buttons/Reject.gif" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                <span style="font-size: 11pt"><strong>
                                            <asp:Label ID="lblJobIdCaption11" runat="server" Font-Bold="True" 
                                                Font-Names="Arial" Font-Size="10pt" Width="87px">Status:</asp:Label>
                                            </strong>
                                </span>
                                                                </td>
                                                                <td>
                                                                    <span style="font-size: 11pt">
                                                                    <asp:TextBox ID="txtHSStatus" runat="server" ReadOnly="True" Width="192px"></asp:TextBox>
                                </span>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="2">
                                <span style="font-size: 11pt"><strong>
                                            <asp:Label ID="lblJobIdCaption12" runat="server" Font-Bold="True" 
                                                Font-Names="Arial" Font-Size="10pt" Width="238px">Completed By:</asp:Label>
                                            </strong>
                                </span>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="2">
                                                                    <span style="font-size: 11pt"><strong>
                                                                    <asp:TextBox ID="txtHSCompletedBy" runat="server" ReadOnly="True" Width="312px"></asp:TextBox>
                                            </strong>
                                </span>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                <span style="font-size: 11pt"><strong>
                                            <asp:Label ID="lblJobIdCaption13" runat="server" Font-Bold="True" 
                                                Font-Names="Arial" Font-Size="10pt" Width="87px">Date:</asp:Label>
                                            </strong>
                                </span>
                                                                </td>
                                                                <td>
                                                                    <span style="font-size: 11pt"><strong>
                                                                    <asp:TextBox ID="txtHSDate" runat="server" ReadOnly="True" Width="192px"></asp:TextBox>
                                            </strong>
                                </span>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    &nbsp;</td>
                                                                <td>
                                                                    &nbsp;</td>
                                                            </tr>
                                                        </table>
                                            </td>
                                        <td style="height: 21px">
                                <span style="font-size: 11pt">
                                                        <table style="width: 339px; border-right: darkgray 1px solid; border-top: darkgray 1px solid;
                border-left: darkgray 1px solid; border-bottom: darkgray 1px solid;">
                                                            <tr>
                                                                <td colspan="2">
                                <span style="font-size: 11pt"><strong>
                                <asp:Label ID="Label9" runat="server" font-bold="True" font-names="Arial" 
                                    font-size="11pt" forecolor="#B38700" text="Classification Signoff" 
                                    width="205px"></asp:Label>
                                </strong>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                <span style="font-size: 11pt"><strong>
                                            <asp:Label ID="lblJobIdCaption6" runat="server" Font-Bold="True" 
                                                Font-Names="Arial" Font-Size="10pt" Width="87px">Comment:</asp:Label>
                                            </strong></span>
                                                                </td>
                                                                <td>
                                                                    &nbsp;</td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="2">
                                                                    <asp:TextBox ID="txtClassComment" runat="server" Height="68px" Width="279px"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:ImageButton ID="btnClassApprove" runat="server" 
                                                                        ImageUrl="~/App_Themes/Billing/Buttons/Approve.gif" />
                                                                </td>
                                                                <td>
                                                                    <asp:ImageButton ID="btnClassReject" runat="server" 
                                                                        ImageUrl="~/App_Themes/Billing/Buttons/Reject.gif" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                <span style="font-size: 11pt"><strong>
                                            <asp:Label ID="lblJobIdCaption7" runat="server" Font-Bold="True" 
                                                Font-Names="Arial" Font-Size="10pt" Width="87px">Status:</asp:Label>
                                            </strong>
                                </span>
                                                                </td>
                                                                <td>
                                <span style="font-size: 11pt">
                                                                    <asp:TextBox ID="txtClassStatus" runat="server" ReadOnly="True" Width="192px"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                <span style="font-size: 11pt"><strong>
                                            <asp:Label ID="lblJobIdCaption8" runat="server" Font-Bold="True" 
                                                Font-Names="Arial" Font-Size="10pt" Width="125px">Completed By:</asp:Label>
                                            </strong>
                                </span>
                                                                </td>
                                                                <td>
                                                                    &nbsp;</td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="2">
                                                                    <span style="font-size: 11pt"><strong>
                                                                    <asp:TextBox ID="txtClassCompletedBy" runat="server" ReadOnly="True" 
                                                                        Width="312px"></asp:TextBox>
                                            </strong>
                                </span>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                <span style="font-size: 11pt"><strong>
                                            <asp:Label ID="lblJobIdCaption9" runat="server" Font-Bold="True" 
                                                Font-Names="Arial" Font-Size="10pt" Width="87px">Date:</asp:Label>
                                            </strong>
                                </span>
                                                                </td>
                                                                <td>
                                                                    <span style="font-size: 11pt"><strong>
                                                                    <asp:TextBox ID="txtClassDate" runat="server" ReadOnly="True" Width="192px"></asp:TextBox>
                                            </strong>
                                </span>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    &nbsp;</td>
                                                                <td>
                                                                    &nbsp;</td>
                                                            </tr>
                                                        </table>
                                            </td>
                                    </tr>
                                                <tr>
                                                    <td colspan="2" style="height: 21px">
                                                        </td>
                                                    <td style="height: 21px">
                                                        </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            &nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td>
                                            &nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td>
                        <igtbl1:ultrawebgrid id="uwgTasks_NS" runat="server" Width="687px" 
                            Height="214px" style="left: 48px; top: 0px">
							<DisplayLayout ColWidthDefault="" AutoGenerateColumns="False" RowHeightDefault="20px"
								Version="4.00" SelectTypeRowDefault="Single" BorderCollapseDefault="Separate"
								AllowColSizingDefault="Free" Name="uwgDeliveries" RowSelectorsDefault="No" 
                                AllowUpdateDefault="NotSet">
								<HeaderStyleDefault BorderStyle="Solid" ForeColor="White" BackColor="#B28700" 
                                    HorizontalAlign="Left" Font-Bold="True">
									<BorderDetails ColorTop="178, 135, 0" WidthLeft="1px" ColorLeft="178, 135, 0" 
                                        ColorBottom="178, 135, 0" ColorRight="178, 135, 0" WidthRight="1px"></BorderDetails>
								</HeaderStyleDefault>
								<FrameStyle Width="670px" BorderWidth="1px" Font-Size="9pt" Font-Names="Arial" BorderStyle="Solid"
									Height="214px" BackColor="White"></FrameStyle>
								<FooterStyleDefault BorderWidth="1px" BorderStyle="Solid" BackColor="LightGray">
									<BorderDetails ColorTop="White" WidthLeft="1px" WidthTop="1px" ColorLeft="White"></BorderDetails>
								</FooterStyleDefault>
								<EditCellStyleDefault BorderWidth="0px" BorderStyle="None"></EditCellStyleDefault>
								<SelectedRowStyleDefault ForeColor="Black" BackColor="#FFFFD0" Font-Names="Arial" Font-Size="9pt"></SelectedRowStyleDefault>
								<RowStyleDefault BorderColor="White" BackColor="#F2F2CC" Font-Names="Arial" Font-Size="9pt" ForeColor="Black" Font-Overline="False">
									<Padding Left="3px"></Padding>
									<BorderDetails WidthLeft="1px" ColorLeft="White" ColorRight="White" WidthRight="1px"></BorderDetails>
								</RowStyleDefault>
                                <ActivationObject BorderColor="" BorderWidth="">
                                </ActivationObject>
                                <RowAlternateStyleDefault BackColor="#EBEBC4" BorderColor="White" Font-Names="Arial"
                                    Font-Size="9pt" ForeColor="Black">
                                    <BorderDetails ColorLeft="White" ColorRight="White" WidthLeft="1px" WidthRight="1px" ColorBottom="White" ColorTop="White" />
                                </RowAlternateStyleDefault>
                                <Images>
                                    <SortAscendingImage Url="./ig_tblSortAsc_White.gif" />
                                    <SortDescendingImage Url="./ig_tblSortDesc_White.gif" />
                                </Images>
                                   <ClientSideEvents InitializeLayoutHandler="InitializeGridLayoutHandler" />
							</DisplayLayout>
							<Bands>
								<igtbl1:UltraGridBand AllowSorting="Yes">
									<Columns>
                                        <igtbl1:UltraGridColumn Key="TaskDescription" BaseColumnName="TaskDescription" 
                                            Width="300px">
                                            <header caption="Task">
                                            </header>
                                        </igtbl1:UltraGridColumn>
                                        <igtbl1:UltraGridColumn BaseColumnName="Done" HeaderText="Supplier" 
                                            Key="Done">
                                            <Header Caption="Complete">
                                                <RowLayoutColumnInfo OriginX="2" />
                                            </Header>
                                            <Footer>
                                                <RowLayoutColumnInfo OriginX="2" />
                                            </Footer>
                                        </igtbl1:UltraGridColumn>
                                        <igtbl1:UltraGridColumn BaseColumnName="DateDue" HeaderText="Delivery Note No." 
                                            Key="DateDue">
                                            <CellStyle Font-Names="Arial" Font-Size="9pt" Wrap="True">
                                            </CellStyle>
                                            <Header Caption="Date Due">
                                                <RowLayoutColumnInfo OriginX="1" />
                                            </Header>
                                            <Footer>
                                                <RowLayoutColumnInfo OriginX="1" />
                                            </Footer>
                                        </igtbl1:UltraGridColumn>
                                        <igtbl1:UltraGridColumn BaseColumnName="CompletedBy" HeaderText="PO Number" 
                                            Key="CompletedBy">
                                            <Header Caption="CompletedBy">
                                                <RowLayoutColumnInfo OriginX="3" />
                                            </Header>
                                            <Footer>
                                                <RowLayoutColumnInfo OriginX="3" />
                                            </Footer>
                                        </igtbl1:UltraGridColumn>
									    <igtbl1:UltraGridColumn BaseColumnName="DateCompleted" Key="DateCompleted">
                                            <Header Caption="Date Completed">
                                                <RowLayoutColumnInfo OriginX="4" />
                                            </Header>
                                            <Footer>
                                                <RowLayoutColumnInfo OriginX="4" />
                                            </Footer>
                                        </igtbl1:UltraGridColumn>
                                        <igtbl1:UltraGridColumn BaseColumnName="OriginatorID" Key="OriginatorID">
                                            <Header Caption="Originator">
                                                <RowLayoutColumnInfo OriginX="5" />
                                            </Header>
                                            <Footer>
                                                <RowLayoutColumnInfo OriginX="5" />
                                            </Footer>
                                        </igtbl1:UltraGridColumn>
                                        <igtbl1:UltraGridColumn BaseColumnName="Coment" Key="Comment" Width="120px">
                                            <CellStyle Font-Names="Arial" Font-Size="9pt" Wrap="True">
                                            </CellStyle>
                                            <Header Caption="Comment">
                                                <RowLayoutColumnInfo OriginX="6" />
                                            </Header>
                                            <Footer>
                                                <RowLayoutColumnInfo OriginX="6" />
                                            </Footer>
                                        </igtbl1:UltraGridColumn>
                                        <igtbl1:UltraGridColumn BaseColumnName="TaskID" Hidden="True" Key="TaskID">
                                            <Header Caption="TaskID">
                                                <RowLayoutColumnInfo OriginX="7" />
                                            </Header>
                                            <Footer>
                                                <RowLayoutColumnInfo OriginX="7" />
                                            </Footer>
                                        </igtbl1:UltraGridColumn>
									</Columns>
                                    <AddNewRow View="NotSet" Visible="NotSet">
                                    </AddNewRow>
								</igtbl1:UltraGridBand>
							</Bands>
						</igtbl1:ultrawebgrid>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            &nbsp;</td>
                                    </tr>
                                </table>
                                </span>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td style="height: 23px">
                </td>
            </tr>
           
           
          
        </table>


         <asp:Panel style="DISPLAY: none" ID="PanelPass" runat="server" Height="168px" Width="500px" BackColor="White" BorderStyle="Solid" BorderWidth="1px" >
        <br />
        <div style="margin:15px">
        <span style="font-family: Arial"><strong>You must enter your Password to proceed.<br />
            <br />
        </strong></span>
        
        <table style="width: 464px">
            <tr>
                <td style="width: 100px; height: 21px">
                    <strong><span style="font-family: Arial">Password:</span></strong></td>
                <td style="width: 100px; height: 21px">
                    <asp:TextBox ID="txtPassword" runat="server" Width="208px" TextMode="Password"></asp:TextBox></td>
            </tr>
        </table>
        </div> 
        <br />
        <strong><span style="font-family: Arial"></span></strong>
        &nbsp; &nbsp; &nbsp;&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
        &nbsp; &nbsp; &nbsp;
        <asp:ImageButton ID="cmdCancelPass" runat="server" 
                 ImageUrl="~/App_Themes/Billing/Buttons/Cancel.gif" />
        &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
        &nbsp;
        <asp:ImageButton ID="cmdOKPass" runat="server" 
                 ImageUrl="~/App_Themes/Billing/Buttons/OK.gif" /><br />
    </asp:Panel>
    <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtenderPass" runat="server" BackgroundCssClass="modalBackground"
        CancelControlID="cmdCancelPass" DropShadow="true" OnCancelScript="HidePasswordModalPopup()"
        PopupControlID="PanelPass" TargetControlID="PanelPass" >
    </ajaxToolkit:ModalPopupExtender> 



     <asp:Panel ID="pnlMsg" runat="server" BackColor="White" BorderStyle="Solid" BorderWidth="1px"
                                    Height="183px" Width="900px" Style="display: none ">
                                    <br />
                                    <center>
                                        <strong><span style="font-family: Arial">&nbsp; </span></strong><strong><span style="font-family: Arial">
                                        </span></strong>
                                    </center>
                                    <center>
                                        <span style="font-family: Arial">
                                            <asp:Label ID="lblInfo" runat="server" Font-Bold="True" Font-Names="Arial"></asp:Label>
                                        </span>&nbsp;<br />
                                        <br />
                                    </center>
                                    <center>
                                        <br />
                                        <asp:ImageButton ID="cmdOkInfo" runat="server" 
                                            ImageUrl="~/App_Themes/Billing/Buttons/OK.gif" />
                                    </center>
                                </asp:Panel>
                                <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtenderMsg" runat="server" BackgroundCssClass="modalBackground"
                                    CancelControlID="cmdOkInfo" DropShadow="True" OnCancelScript="HideMsgPopup()"
                                    PopupControlID="pnlMsg" TargetControlID="pnlMsg">
                                </ajaxToolkit:ModalPopupExtender>




    </p>
    </asp:Content>

