<%@ Page Language="VB" MasterPageFile="~/TabPages/ProjectMain.master" EnableEventValidation="false" AutoEventWireup="false" CodeFile="HandheldTxns.aspx.vb" Inherits="TabPages_HandheldTxns"  %>
<%@ Register TagPrefix="igtbl1" Namespace="Infragistics.WebUI.UltraWebGrid" Assembly="Infragistics2.WebUI.UltraWebGrid.v7.3, Version=7.3.20073.38, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>

<%@ Register tagprefix="igsch" namespace="Infragistics.WebUI.WebSchedule" Assembly="Infragistics2.WebUI.WebDateChooser.v7.3, Version=7.3.20073.38, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ProjectMAIN_content" Runat="Server">

<script type="text/javascript">
         

 
                
    </script>


    <table border="0" cellpadding="0" cellspacing="0" 
        style="padding: 0px; margin: 0px; width: 950px; height: 410px;">
                <tr>
                    <td style="vertical-align: top; width: 15px; position: static; background-color: white;
                        text-align: left; ">
                        &nbsp;</td>
                    <td style="vertical-align: top; position: static; text-align: left; margin-left: 10px;" 
                        colspan="2">
    
    
                      <table style="font-size: 10pt; width: 950px; font-family: Arial">
        <tr>
            <td align="left" valign="top" style="background-color: #E6E6CC">
                <asp:Label ID="lblWO" runat="server" BackColor="#E6E6CC" Font-Bold="True" Font-Names="Arial"
                    Font-Size="11pt" Height="22px" Text="Transaction View " Width="179px"></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            &nbsp;&nbsp;
                            </td>
        </tr>
        <tr style="font-size: 10pt">
            <td align="left" valign="top" style="border: 1px solid #808080;">
            
            <table style="width: 930px; font-family: Arial;">
            <tr>
                <td style="width: 219px; text-align: right;">
                            &nbsp;</td>
                <td style="width: 245px; text-align: left">
                            &nbsp;</td>
                <td style="width: 220px; text-align: right;">
                            &nbsp;</td>
                <td style="width: 245px; text-align: left">
                            &nbsp;</td>
            </tr>
            <tr>
                <td style="width: 219px; text-align: right; height: 20px;">
                            </td>
                <td style="width: 245px; text-align: left; height: 20px;">
                            </td>
                <td style="width: 220px; text-align: right; height: 20px;">
                            </td>
                <td style="width: 245px; text-align: left; height: 20px;">
                            </td>
            </tr>
            <tr>
                <td style="width: 219px; text-align: right;">
                            &nbsp;</td>
                <td style="width: 245px; text-align: left">
                            &nbsp;</td>
                <td style="width: 220px; text-align: right;">
                            &nbsp;</td>
                <td style="width: 245px; text-align: left">
                            &nbsp;</td>
            </tr>
            <tr>
                <td style="width: 219px; text-align: right;">
                            &nbsp;</td>
                <td style="text-align: left" colspan="3">
                            &nbsp;</td>
            </tr>
            <tr>
                <td style="width: 219px; text-align: right;">
                    &nbsp;</td>
                <td style="text-align: left" colspan="3">
                        &nbsp;</td>
            </tr>
        </table>
            
            
            </td>
        </tr>
    </table>
    
    
                    </td>
                </tr>
                <tr>
                    <td style="vertical-align: top; width: 15px; position: static; background-color: white;
                        text-align: left; ">
                        &nbsp;</td>
                    <td style="vertical-align: top; width: 360px; position: static; text-align: left; margin-left: 10px;">
                        &nbsp;</td>
                    <td style="vertical-align: top; width: 386px; position: static; text-align: left; margin-left: 10px;">
                        &nbsp;</td>
                </tr>
                <tr>
                    <td style="vertical-align: top; width: 15px; position: static; background-color: white;
                        text-align: left; ">
                        &nbsp;</td>
                    <td style="border: thin solid #C0C0C0; vertical-align: top; position: static; text-align: left; margin-left: 10px;" 
                        colspan="2">
                        &nbsp;</td>
                </tr>
                <tr>
                    <td style="vertical-align: top; width: 15px; position: static; background-color: white;
                        text-align: left; ">
                        &nbsp;</td>
                    <td style="vertical-align: top; position: static; text-align: left; margin-left: 10px;" 
                        colspan="2">
                        &nbsp;</td>
                </tr>
                <tr>
                    <td style="vertical-align: top; width: 15px; position: static; background-color: white;
                        text-align: left; ">
                        &nbsp;</td>
                    <td style="vertical-align: top; position: static; text-align: left; margin-left: 10px;" 
                        colspan="2">
                        &nbsp;&nbsp;
                        &nbsp;</td>
                </tr>
                <tr>
                    <td style="vertical-align: top; width: 15px; position: static; background-color: white;
                        text-align: left; height: 9px;">
                        </td>
                    <td style="border-left: 1px solid #808080; border-right: 1px solid #808080; border-top: 1px solid #808080; vertical-align: top; position: static; text-align: left; margin-left: 10px; height: 9px; " 
                        colspan="2">
                        <asp:Label ID="lblGridTitle" runat="server" Font-Bold="True" 
                            style="font-size: 14pt; font-family: Arial">Sales Orders Table on Handheld</asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="vertical-align: top; width: 15px; position: static; background-color: white;
                        text-align: left; height: 200px;">
                        </td>
                    <td style="border-left: 1px solid #808080; border-right: 1px solid #808080; border-bottom: 1px solid #808080; vertical-align: top; position: static; text-align: left; margin-left: 10px; " 
                        colspan="2">
                        <igtbl1:ultrawebgrid id="uwgtcd_tblSalesOrders" runat="server" Width="920px" 
                            Height="176px" style="left: 48px; top: 0px">
							<DisplayLayout ColWidthDefault="" 
                                AllowSortingDefault="OnClient" RowHeightDefault="20px"
								Version="4.00" SelectTypeRowDefault="Single" HeaderClickActionDefault="SortSingle" BorderCollapseDefault="Separate"
								AllowColSizingDefault="Free" Name="uwgDeliveries" CellClickActionDefault="RowSelect" 
                                RowSelectorsDefault="No" AllowUpdateDefault="Yes">
								<HeaderStyleDefault BorderStyle="Solid" ForeColor="White" BackColor="#B28700" 
                                    HorizontalAlign="Left" Font-Bold="True" Font-Names="Arial" 
                                    Font-Overline="False" Font-Size="10pt" Wrap="True">
									<Padding Bottom="6px" Left="1px" Right="1px" Top="2px" />
									<BorderDetails ColorTop="178, 135, 0" WidthLeft="1px" ColorLeft="178, 135, 0" 
                                        ColorBottom="178, 135, 0" ColorRight="178, 135, 0" WidthRight="1px"></BorderDetails>
								</HeaderStyleDefault>
								<FrameStyle Width="920px" BorderWidth="1px" Font-Size="9pt" Font-Names="Arial" BorderStyle="Solid"
									Height="176px" BackColor="White"></FrameStyle>
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
                                    <AddNewRow View="NotSet" Visible="NotSet">
                                    </AddNewRow>
								</igtbl1:UltraGridBand>
							</Bands>
						</igtbl1:ultrawebgrid>
						
						
						
		     		</td>
                </tr>
                <tr>
                    <td style="vertical-align: top; width: 15px; position: static; background-color: white;
                        text-align: left; height: 9px;">
                        </td>
                    <td style="border-left: 1px solid #808080; border-right: 1px solid #808080; border-top: 1px solid #808080; vertical-align: top; position: static; text-align: left; margin-left: 10px; height: 9px; " 
                        colspan="2">
                        &nbsp;</td>
                </tr>
                                      
                 <tr>
                    <td style="vertical-align: top; width: 15px; position: static; background-color: white;
                        text-align: left; height: 9px;">
                        </td>
                    <td style="border-left: 1px solid #808080; border-right: 1px solid #808080; border-top: 1px solid #808080; vertical-align: top; position: static; text-align: left; margin-left: 10px; height: 9px; " 
                        colspan="2">
                        <asp:Label ID="Label2" runat="server" Font-Bold="True" 
                            style="font-size: 14pt; font-family: Arial">Transactions</asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="vertical-align: top; width: 15px; position: static; background-color: white;
                        text-align: left; height: 200px;">
                        &nbsp;</td>
                    <td style="border-left: 1px solid #808080; border-right: 1px solid #808080; border-bottom: 1px solid #808080; vertical-align: top; position: static; text-align: left; margin-left: 10px; " 
                        colspan="2">
                        <igtbl1:ultrawebgrid id="uwgTransactions" runat="server" Width="920px" 
                            Height="176px" style="left: 48px; top: 0px">
							<DisplayLayout ColWidthDefault="" 
                                AllowSortingDefault="OnClient" RowHeightDefault="20px"
								Version="4.00" SelectTypeRowDefault="Single" HeaderClickActionDefault="SortSingle" BorderCollapseDefault="Separate"
								AllowColSizingDefault="Free" Name="uwgDeliveries" CellClickActionDefault="RowSelect" 
                                RowSelectorsDefault="No" AllowUpdateDefault="Yes">
								<HeaderStyleDefault BorderStyle="Solid" ForeColor="White" BackColor="#B28700" 
                                    HorizontalAlign="Left" Font-Bold="True" Font-Names="Arial" 
                                    Font-Overline="False" Font-Size="10pt" Wrap="True">
									<Padding Bottom="6px" Left="1px" Right="1px" Top="2px" />
									<BorderDetails ColorTop="178, 135, 0" WidthLeft="1px" ColorLeft="178, 135, 0" 
                                        ColorBottom="178, 135, 0" ColorRight="178, 135, 0" WidthRight="1px"></BorderDetails>
								</HeaderStyleDefault>
								<FrameStyle Width="920px" BorderWidth="1px" Font-Size="9pt" Font-Names="Arial" BorderStyle="Solid"
									Height="176px" BackColor="White"></FrameStyle>
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
                                    <AddNewRow View="NotSet" Visible="NotSet">
                                    </AddNewRow>
								</igtbl1:UltraGridBand>
							</Bands>
						</igtbl1:ultrawebgrid>
						
						
						
		     		</td>
                </tr>
                
                
                 <tr>
                    <td style="vertical-align: top; width: 15px; position: static; background-color: white;
                        text-align: left; height: 9px;">
                        </td>
                    <td style="border-left: 1px solid #808080; border-right: 1px solid #808080; border-top: 1px solid #808080; vertical-align: top; position: static; text-align: left; margin-left: 10px; height: 9px; " 
                        colspan="2">
                        <asp:Label ID="Label3" runat="server" Font-Bold="True" 
                            style="font-size: 14pt; font-family: Arial">Intermediate Transaction Table</asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="vertical-align: top; width: 15px; position: static; background-color: white;
                        text-align: left; height: 200px;">
                        &nbsp;</td>
                    <td style="border-left: 1px solid #808080; border-right: 1px solid #808080; border-bottom: 1px solid #808080; vertical-align: top; position: static; text-align: left; margin-left: 10px; " 
                        colspan="2">
                        <igtbl1:ultrawebgrid id="uwgIntermediateTxns" runat="server" Width="920px" 
                            Height="178px" style="left: 48px; top: 0px">
							<DisplayLayout ColWidthDefault="" 
                                AllowSortingDefault="OnClient" RowHeightDefault="20px"
								Version="4.00" SelectTypeRowDefault="Single" HeaderClickActionDefault="SortSingle" BorderCollapseDefault="Separate"
								AllowColSizingDefault="Free" Name="uwgDeliveries" CellClickActionDefault="RowSelect" 
                                RowSelectorsDefault="No" AllowUpdateDefault="Yes">
								<HeaderStyleDefault BorderStyle="Solid" ForeColor="White" BackColor="#B28700" 
                                    HorizontalAlign="Left" Font-Bold="True" Font-Names="Arial" 
                                    Font-Overline="False" Font-Size="10pt" Wrap="True">
									<Padding Bottom="6px" Left="1px" Right="1px" Top="2px" />
									<BorderDetails ColorTop="178, 135, 0" WidthLeft="1px" ColorLeft="178, 135, 0" 
                                        ColorBottom="178, 135, 0" ColorRight="178, 135, 0" WidthRight="1px"></BorderDetails>
								</HeaderStyleDefault>
								<FrameStyle Width="920px" BorderWidth="1px" Font-Size="9pt" Font-Names="Arial" BorderStyle="Solid"
									Height="178px" BackColor="White"></FrameStyle>
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
                                    <AddNewRow View="NotSet" Visible="NotSet">
                                    </AddNewRow>
								</igtbl1:UltraGridBand>
							</Bands>
						</igtbl1:ultrawebgrid>
						
						
						
		     		</td>
                </tr>
                
                
                 <tr>
                    <td style="vertical-align: top; width: 15px; position: static; background-color: white;
                        text-align: left; height: 9px;">
                        </td>
                    <td style="border-left: 1px solid #808080; border-right: 1px solid #808080; border-top: 1px solid #808080; vertical-align: top; position: static; text-align: left; margin-left: 10px; height: 9px; " 
                        colspan="2">
                        <asp:Label ID="Label4" runat="server" Font-Bold="True" 
                            style="font-size: 14pt; font-family: Arial">Handheld Transaction Backup</asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="vertical-align: top; width: 15px; position: static; background-color: white;
                        text-align: left; height: 200px;">
                        &nbsp;</td>
                    <td style="border-left: 1px solid #808080; border-right: 1px solid #808080; border-bottom: 1px solid #808080; vertical-align: top; position: static; text-align: left; margin-left: 10px; " 
                        colspan="2">
                        <igtbl1:ultrawebgrid id="uwgHHBackup" runat="server" Width="920px" 
                            Height="178px" style="left: 48px; top: 0px">
							<DisplayLayout ColWidthDefault="" 
                                AllowSortingDefault="OnClient" RowHeightDefault="20px"
								Version="4.00" SelectTypeRowDefault="Single" HeaderClickActionDefault="SortSingle" BorderCollapseDefault="Separate"
								AllowColSizingDefault="Free" Name="uwgDeliveries" CellClickActionDefault="RowSelect" 
                                RowSelectorsDefault="No" AllowUpdateDefault="Yes">
								<HeaderStyleDefault BorderStyle="Solid" ForeColor="White" BackColor="#B28700" 
                                    HorizontalAlign="Left" Font-Bold="True" Font-Names="Arial" 
                                    Font-Overline="False" Font-Size="10pt" Wrap="True">
									<Padding Bottom="6px" Left="1px" Right="1px" Top="2px" />
									<BorderDetails ColorTop="178, 135, 0" WidthLeft="1px" ColorLeft="178, 135, 0" 
                                        ColorBottom="178, 135, 0" ColorRight="178, 135, 0" WidthRight="1px"></BorderDetails>
								</HeaderStyleDefault>
								<FrameStyle Width="920px" BorderWidth="1px" Font-Size="9pt" Font-Names="Arial" BorderStyle="Solid"
									Height="178px" BackColor="White"></FrameStyle>
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
                                    <AddNewRow View="NotSet" Visible="NotSet">
                                    </AddNewRow>
								</igtbl1:UltraGridBand>
							</Bands>
						</igtbl1:ultrawebgrid>
						
						
						
		     		</td>
                </tr>
                
                 <tr>
                    <td style="vertical-align: top; width: 15px; position: static; background-color: white;
                        text-align: left; height: 9px;">
                        </td>
                    <td style="border-left: 1px solid #808080; border-right: 1px solid #808080; border-top: 1px solid #808080; vertical-align: top; position: static; text-align: left; margin-left: 10px; height: 9px; " 
                        colspan="2">
                        <asp:Label ID="Label5" runat="server" Font-Bold="True" 
                            style="font-size: 14pt; font-family: Arial">Handheld Transactions</asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="vertical-align: top; width: 15px; position: static; background-color: white;
                        text-align: left; height: 200px;">
                        &nbsp;</td>
                    <td style="border-left: 1px solid #808080; border-right: 1px solid #808080; border-bottom: 1px solid #808080; vertical-align: top; position: static; text-align: left; margin-left: 10px; " 
                        colspan="2">
                        <igtbl1:ultrawebgrid id="uwgHHTxns" runat="server" Width="920px" 
                            Height="176px" style="left: 48px; top: 0px">
							<DisplayLayout ColWidthDefault="" 
                                AllowSortingDefault="OnClient" RowHeightDefault="20px"
								Version="4.00" SelectTypeRowDefault="Single" HeaderClickActionDefault="SortSingle" BorderCollapseDefault="Separate"
								AllowColSizingDefault="Free" Name="uwgDeliveries" CellClickActionDefault="RowSelect" 
                                RowSelectorsDefault="No" AllowUpdateDefault="Yes">
								<HeaderStyleDefault BorderStyle="Solid" ForeColor="White" BackColor="#B28700" 
                                    HorizontalAlign="Left" Font-Bold="True" Font-Names="Arial" 
                                    Font-Overline="False" Font-Size="10pt" Wrap="True">
									<Padding Bottom="6px" Left="1px" Right="1px" Top="2px" />
									<BorderDetails ColorTop="178, 135, 0" WidthLeft="1px" ColorLeft="178, 135, 0" 
                                        ColorBottom="178, 135, 0" ColorRight="178, 135, 0" WidthRight="1px"></BorderDetails>
								</HeaderStyleDefault>
								<FrameStyle Width="920px" BorderWidth="1px" Font-Size="9pt" Font-Names="Arial" BorderStyle="Solid"
									Height="176px" BackColor="White"></FrameStyle>
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
                                    <AddNewRow View="NotSet" Visible="NotSet">
                                    </AddNewRow>
								</igtbl1:UltraGridBand>
							</Bands>
						</igtbl1:ultrawebgrid>
						
						
						
		     		</td>
                </tr>
                
                
                <tr>
                    <td style="vertical-align: top; width: 15px; position: static; background-color: white;
                        text-align: left">
                    </td>
                    <td colspan="2">
                        <table style="width: 890px">
                            <tr>
                                <td rowspan="2">
                                    &nbsp;</td>
                                <td style="height: 40px">
                                
                                    <br />
                                    </td>
                            </tr>
                            <tr>
                                <td style="border-top-style: solid; border-right-style: solid; border-left-style: solid; border-top-width: 1px; border-right-width: 1px; border-left-width: 1px; border-top-color: #808080; border-right-color: #808080; border-left-color: #808080">
                                &nbsp;
                                    </td>
                            </tr>
                            <tr>
                                <td>
                                    &nbsp;</td>
                                <td style="border-right-style: solid; border-left-style: solid; border-right-width: 1px; border-left-width: 1px; border-right-color: #808080; border-left-color: #808080">
                                    &nbsp;</td>
                            </tr>
                            <tr>
                                <td>
                                    &nbsp;</td>
                                <td style="border-right-style: solid; border-bottom-style: solid; border-left-style: solid; border-right-width: 1px; border-bottom-width: 1px; border-left-width: 1px; border-right-color: #808080; border-bottom-color: #808080; border-left-color: #808080">
                        <table style="width: 800px">
                            <tr>
                                <td style="width: 100px">
                                    &nbsp;</td>
                                <td>
                                    &nbsp;</td>
                                <td>
                                    &nbsp;</td>
                            </tr>
                            </table>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td style="vertical-align: top; width: 15px; position: static; background-color: white;
                        text-align: left">
                        &nbsp;</td>
                    <td colspan="2">
                        &nbsp;
                        &nbsp;</td>
                </tr>
                <tr>
                    <td style="vertical-align: top; width: 15px; position: static; background-color: white;
                        text-align: left">
                        &nbsp;</td>
                    <td style="border-left: 1px solid #808080; border-right: 1px solid #808080; border-top: 1px solid #808080;" 
                        colspan="2">
                                &nbsp;
                                &nbsp;
                                </td>
                </tr>
                <tr>
                    <td style="vertical-align: top; width: 15px; position: static; background-color: white;
                        text-align: left">
                        &nbsp;</td>
                    <td style="border-left: width: 20px; 1px solid #808080; border-right: 1px solid #808080;" 
                        colspan="2">
                        &nbsp;</td>
                </tr>
                <tr>
                    <td style="vertical-align: top; width: 15px; position: static; background-color: white;
                        text-align: left">
                        &nbsp;</td>
                    <td style="border-left: 1px solid #808080; border-right: 1px solid #808080; border-bottom: 1px solid #808080;" 
                        colspan="2">
                        <table style="width: 750px">
                            <tr>
                                <td style="width: 100px">
                                    &nbsp;</td>
                                <td>
                                    &nbsp;</td>
                                <td>
                                    &nbsp;</td>
                                <td>
                                    &nbsp;</td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td style="vertical-align: top; width: 15px; position: static; background-color: white;
                        text-align: left">
                        &nbsp;</td>
                    <td colspan="2">
                        &nbsp;</td>
                </tr>
                </table>    
    <asp:Panel ID="Panel1" runat="server" BackColor="White" BorderStyle="Solid" BorderWidth="1px"
        Height="168px" Style="display: " Width="568px">
        <br />
        <br />
        <div style="margin: 15px">
            <strong><span style="font-family: Arial"></span></strong>&nbsp;&nbsp;<br />
            <br />
            <center>
                <table style="width: 488px">
                    <tr>
                        <td style="width: 159px; height: 21px">
                            <strong><span style="font-family: Arial">
                                <asp:Label ID="lblSelectCat" runat="server"  Text="Supplier Category:"
                                    Width="96px"></asp:Label></span></strong></td>
                        <td style="width: 100px; height: 21px">
                            <asp:DropDownList ID="ddlCategory" runat="server"  Width="232px">
                            </asp:DropDownList></td>
                    </tr>
                    <tr>
                        <td style="width: 159px">
                            <strong><span style="font-family: Arial">
                                <asp:Label ID="lblSelectName" runat="server"  Text="Supplier Name:"></asp:Label></span></strong></td>
                        <td style="width: 100px">
                            <asp:DropDownList ID="ddlSupplier" runat="server"  Width="232px">
                            </asp:DropDownList></td>
                    </tr>
                </table>
            </center>
            <center>
                <table style="width: 496px">
                    <tr>
                        <td style="width: 160px; height: 21px">
                            <strong><span style="font-family: Arial">
                                <asp:Label ID="lblNoteNumCaption" runat="server"  Text="Delivery Note No.:"></asp:Label></span></strong></td>
                        <td style="width: 56px; height: 21px">
                            <asp:TextBox ID="txtDeliveryNoteNum" runat="server"  Width="312px"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td style="width: 160px">
                            <asp:Label ID="lblProdCaption" runat="server" Font-Bold="True" Font-Names="Arial"
                                 Text="Product:"></asp:Label></td>
                        <td style="width: 56px">
                            <asp:TextBox ID="txtProduct" runat="server"  Width="312px"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td style="width: 160px">
                            <asp:Label ID="lblQtyCaption" runat="server" Font-Bold="True" Font-Names="Arial"
                                 Text="Quantity:"></asp:Label></td>
                        <td style="width: 56px">
                            <asp:TextBox ID="txtQty" runat="server"  Width="256px"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td style="width: 160px">
                            <strong><span style="font-family: Arial">
                                <asp:Label ID="lblPONumCaption" runat="server"  Text="PO No.:"></asp:Label></span></strong></td>
                        <td style="width: 56px">
                            <asp:TextBox ID="txtPONum" runat="server"  Width="256px"></asp:TextBox></td>
                    </tr>
                </table>
            </center>
        </div>
        <ajaxToolkit:CascadingDropDown ID="CascadingDropDown1" runat="server" Category="Category"
            LoadingText="Loading Categories ..." PromptText="Select Category .." ServiceMethod="GetSupplierCategories"
            ServicePath="~/ModuleServices.asmx" TargetControlID="ddlCategory">
        </ajaxToolkit:CascadingDropDown>
        <ajaxToolkit:CascadingDropDown ID="CascadingDropDown2" runat="server" Category="Customers"
            LoadingText="Loading Prospects ..." ParentControlID="ddlCategory" PromptText="Select Supplier .."
            ServiceMethod="GetSuppliersInCategory" ServicePath="~/ModuleServices.asmx" TargetControlID="ddlSupplier">
        </ajaxToolkit:CascadingDropDown>
        &nbsp;
        <center>
            <asp:ImageButton ID="cmdCancel" runat="server" ImageUrl="~/App_Themes/Buttons/Cancel.gif" />
            &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
            &nbsp;
            <asp:ImageButton ID="cmdOK" runat="server" ImageUrl="~/App_Themes/Buttons/OK.gif" /><br />
        </center>
    </asp:Panel>
    </asp:Content>

