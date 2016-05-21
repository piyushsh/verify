<%@ Page Language="VB" MasterPageFile= "~/TabPages/ProjectMain.master" AutoEventWireup="false" CodeFile="Printouts_Opening.aspx.vb" Inherits="TabPages_Printouts_Opening"  %>

<%@ Register tagprefix="igtbl1" namespace="Infragistics.WebUI.UltraWebGrid" Assembly="Infragistics2.WebUI.UltraWebGrid.v7.3, Version=7.3.20073.38, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ProjectMAIN_content" Runat="Server">
    <div style="margin-left:10px; margin-top:10px">
    <table style="font-size: 10pt; width: 950px; font-family: Arial">
        <tr style="font-size: 10pt">
            <td align="left" valign="top">
            
                &nbsp;</td>
        </tr>
        </table>
    <table style="width: 940px">
        <tr>
            <td>
                <table style="width: 935px">
                    <tr>
                        <td style="width: 183px">
                            <asp:Label ID="Label8" runat="server" Font-Bold="True" Font-Size="10pt" 
                                Text="Select Report Category:" Font-Names="Arial"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlCategory" runat="server" Height="30px" Width="283px" 
                                AutoPostBack="True">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 183px">
                            &nbsp;</td>
                        <td>
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td style="height: 23px; width: 183px;">
                            <asp:Label ID="Label9" runat="server" Font-Bold="True" Font-Size="10pt" 
                                Text="Select Report to run:" Font-Names="Arial"></asp:Label>
                        </td>
                        <td style="height: 23px">
                            <asp:DropDownList ID="ddlReport" runat="server" Height="30px" Width="283px" 
                                AutoPostBack="True">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 183px">
                            &nbsp;</td>
                        <td>
                            &nbsp;</td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
   
                &nbsp;
                <asp:Label ID="Label17" runat="server" Text="Report - Control Parameters" 
                    Font-Bold="True" Font-Names="Arial" Font-Size="10pt"></asp:Label>
            &nbsp;<asp:Label ID="Label18" runat="server" Font-Names="Arial" Font-Size="10pt" 
                    Text="(Enter the Values you want to run the report for here)"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                        <igtbl1:ultrawebgrid id="uwgParams" runat="server" Width="800px" 
                            Height="193px" style="left: 48px; top: 0px">
							<DisplayLayout ColWidthDefault="" AutoGenerateColumns="False" 
                                AllowSortingDefault="OnClient" RowHeightDefault="20px"
								Version="4.00" SelectTypeRowDefault="Single" HeaderClickActionDefault="SortSingle" BorderCollapseDefault="Separate"
								AllowColSizingDefault="Free" Name="uwgDeliveries" CellClickActionDefault="Edit" 
                                RowSelectorsDefault="No" AllowUpdateDefault="Yes">
								<HeaderStyleDefault BorderStyle="Solid" ForeColor="White" BackColor="#006600" HorizontalAlign="Left">
									<BorderDetails ColorTop="0, 102, 0" WidthLeft="1px" ColorLeft="0, 102, 0" ColorBottom="0, 102, 0" ColorRight="0, 102, 0" WidthRight="1px"></BorderDetails>
								</HeaderStyleDefault>
								<FrameStyle Width="800px" BorderWidth="1px" Font-Size="9pt" Font-Names="Arial" BorderStyle="Solid"
									Height="193px" BackColor="White"></FrameStyle>
								<FooterStyleDefault BorderWidth="1px" BorderStyle="Solid" BackColor="LightGray">
									<BorderDetails ColorTop="White" WidthLeft="1px" WidthTop="1px" ColorLeft="White"></BorderDetails>
								</FooterStyleDefault>
								<EditCellStyleDefault BorderWidth="0px" BorderStyle="None"></EditCellStyleDefault>
								<SelectedRowStyleDefault ForeColor="Black" BackColor="#FFFFD0" Font-Names="Arial" Font-Size="9pt"></SelectedRowStyleDefault>
								<RowStyleDefault BorderColor="White" BackColor="#F2F2CC" Font-Names="Arial" Font-Size="9pt" ForeColor="Black" Font-Overline="False">
									<Padding Left="3px"></Padding>
									<BorderDetails WidthLeft="1px" ColorLeft="White" ColorRight="White" 
                                        WidthRight="1px" ColorBottom="White" ColorTop="White" StyleBottom="Solid" 
                                        StyleLeft="Solid" StyleRight="Solid" StyleTop="Solid"></BorderDetails>
								</RowStyleDefault>
                                <ActivationObject BorderColor="" BorderWidth="">
                                </ActivationObject>
                                <RowAlternateStyleDefault BackColor="#EBEBC4" BorderColor="White" Font-Names="Arial"
                                    Font-Size="9pt" ForeColor="Black">
                                    <BorderDetails ColorLeft="White" ColorRight="Black" WidthLeft="1px" 
                                        WidthRight="1px" ColorBottom="White" ColorTop="White" StyleBottom="Solid" 
                                        StyleLeft="Solid" StyleRight="Solid" StyleTop="Solid" />
                                </RowAlternateStyleDefault>
                                <Images>
                                    <SortAscendingImage Url="./ig_tblSortAsc_White.gif" />
                                    <SortDescendingImage Url="./ig_tblSortDesc_White.gif" />
                                </Images>
							</DisplayLayout>
							<Bands>
								<igtbl1:UltraGridBand AllowSorting="Yes">
									<Columns>
                                        <igtbl1:UltraGridColumn Key="Parameter Name" AllowUpdate="No">
                                            <header caption="Parameter Name">
                                            </header>
                                        </igtbl1:UltraGridColumn>
                                        <igtbl1:UltraGridColumn HeaderText="Delivery Note No." 
                                            Key="Type" AllowUpdate="No">
                                            <Header Caption="Type">
                                                <RowLayoutColumnInfo OriginX="1" />
                                            </Header>
                                            <Footer>
                                                <RowLayoutColumnInfo OriginX="1" />
                                            </Footer>
                                        </igtbl1:UltraGridColumn>
                                        <igtbl1:UltraGridColumn HeaderText="Supplier" 
                                            Key="Value" AllowUpdate="Yes">
                                            <Header Caption="Value">
                                                <RowLayoutColumnInfo OriginX="2" />
                                            </Header>
                                            <Footer>
                                                <RowLayoutColumnInfo OriginX="2" />
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
        <tr>
            <td>
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:ImageButton ID="btnRun" runat="server" AlternateText="Print Report" 
                    ImageUrl="~/App_Themes/Billing2/Buttons/Run Report.gif" />
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;</td>
        </tr>
    </table>
    <br />
    </div>
</asp:Content>

