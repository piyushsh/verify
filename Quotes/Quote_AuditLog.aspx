<%@ Page Title="" Language="VB" MasterPageFile="~/Quotes/FormsMaster.master" AutoEventWireup="false" CodeFile="Quote_AuditLog.aspx.vb" Inherits="Quotes_Quote_AuditLog" %>

<%@ Register TagPrefix="igsch" Namespace="Infragistics.WebUI.WebSchedule" Assembly="Infragistics2.WebUI.WebDateChooser.v7.3, Version=7.3.20073.38, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>

<%@ Register assembly="Infragistics2.WebUI.UltraWebGrid.v7.3, Version=7.3.20073.38, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" namespace="Infragistics.WebUI.UltraWebGrid" tagprefix="igtbl1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="DetailsMaster_ContentPlaceHolder" Runat="Server">
    <p>
    
        <table style="width: 689px">
         
            <tr>
                <td>
                    <table style="font-size: 10pt; width: 688px; font-family: Arial">
                        <tr>
                            <td align="left" style="border-right: darkgray 1px solid; border-top: darkgray 1px solid;
                border-left: darkgray 1px solid; border-bottom: darkgray 1px solid; height: 120px;" 
                                valign="top">
                                <span style="font-size: 11pt"><strong>
                                <asp:Label ID="Label7" runat="server" font-bold="True" font-names="Arial" 
                                    font-size="11pt" forecolor="#B38700" text="Audit Log" 
                                    width="205px"></asp:Label>
                                <br />
                                </strong>
                                <table style="width: 687px">
                                    <tr>
                                        <td>
                                            &nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td>
                        <igtbl1:ultrawebgrid id="uwgAudit" runat="server" Width="669px" 
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
								<FrameStyle Width="669px" BorderWidth="1px" Font-Size="9pt" Font-Names="Arial" BorderStyle="Solid"
									Height="214px" BackColor="White"></FrameStyle>
								<FooterStyleDefault BorderWidth="1px" BorderStyle="Solid" BackColor="LightGray">
									<BorderDetails ColorTop="White" WidthLeft="1px" WidthTop="1px" ColorLeft="White"></BorderDetails>
								</FooterStyleDefault>

<ClientSideEvents InitializeLayoutHandler="InitializeGridLayoutHandler"></ClientSideEvents>

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
<BorderDetails ColorLeft="White" ColorTop="White" ColorRight="White" ColorBottom="White" WidthLeft="1px" WidthRight="1px"></BorderDetails>
                                </RowAlternateStyleDefault>
                                <Images>
                                    <SortAscendingImage Url="./ig_tblSortAsc_White.gif" />
                                    <SortDescendingImage Url="./ig_tblSortDesc_White.gif" />
                                    <SortAscendingImage Url="./ig_tblSortAsc_White.gif"></SortAscendingImage><SortDescendingImage Url="./ig_tblSortDesc_White.gif"></SortDescendingImage>
                                </Images>
                                   <ClientSideEvents InitializeLayoutHandler="InitializeGridLayoutHandler" />
							</DisplayLayout>
							<Bands>
								<igtbl1:UltraGridBand AllowSorting="Yes">
									<Columns>
                                        <igtbl1:UltraGridColumn Key="Date" BaseColumnName="DateAndTime" 
                                            Format="dd/MM/yyyy hh:mm">
                                            <header caption="Date:">
                                            </header>
                                            <CellStyle Font-Names="Arial" Font-Size="9pt">
                                            </CellStyle>
                                        </igtbl1:UltraGridColumn>
                                        <igtbl1:UltraGridColumn BaseColumnName="Comment" HeaderText="Supplier" 
                                            Key="Comment">
                                            <CellStyle Font-Names="Arial" Font-Size="9pt" Wrap="True">
                                            </CellStyle>
                                            <Header Caption="Comment:">
                                                <RowLayoutColumnInfo OriginX="2" />
<RowLayoutColumnInfo OriginX="1"></RowLayoutColumnInfo>
                                            </Header>
                                            <Footer>
                                                <RowLayoutColumnInfo OriginX="2" />
<RowLayoutColumnInfo OriginX="1"></RowLayoutColumnInfo>
                                            </Footer>
                                        </igtbl1:UltraGridColumn>
                                        <igtbl1:UltraGridColumn BaseColumnName="UserName" HeaderText="Delivery Note No." 
                                            Key="User">
                                            <CellStyle Font-Names="Arial" Font-Size="9pt" Wrap="True">
                                            </CellStyle>
                                            <Header Caption="Logged By:">
                                                <RowLayoutColumnInfo OriginX="1" />
<RowLayoutColumnInfo OriginX="2"></RowLayoutColumnInfo>
                                            </Header>
                                            <Footer>
                                                <RowLayoutColumnInfo OriginX="1" />
<RowLayoutColumnInfo OriginX="2"></RowLayoutColumnInfo>
                                            </Footer>
                                        </igtbl1:UltraGridColumn>
                                        <igtbl1:UltraGridColumn BaseColumnName="RecordType" HeaderText="PO Number" 
                                            Key="RecordType">
                                            <Header Caption="Type:">
                                                <RowLayoutColumnInfo OriginX="3" />
<RowLayoutColumnInfo OriginX="3"></RowLayoutColumnInfo>
                                            </Header>
                                            <Footer>
                                                <RowLayoutColumnInfo OriginX="3" />
<RowLayoutColumnInfo OriginX="3"></RowLayoutColumnInfo>
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
                    <table style="font-size: 10pt; width: 688px; font-family: Arial">
                        <tr>
                            <td align="left" style="border-right: darkgray 1px solid; border-top: darkgray 1px solid;
                border-left: darkgray 1px solid; border-bottom: darkgray 1px solid; height: 120px;" 
                                valign="top">
                                &nbsp;</td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;</td>
            </tr>
          
        </table>
    </p>
    </asp:Content>

