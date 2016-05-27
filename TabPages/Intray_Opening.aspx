<%@ Page Language="vb"   MasterPageFile= "~/TabPages/ProjectMain.master"  AutoEventWireup="false" Inherits="Intray_Opening" CodeFile="Intray_Opening.aspx.vb" %>
<%@ Register TagPrefix="igcmbo" Namespace="Infragistics.WebUI.WebCombo" Assembly="Infragistics2.WebUI.WebCombo.v7.3, Version=7.3.20073.38, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>

<%@ Register TagPrefix="igtbl1" Namespace="Infragistics.WebUI.UltraWebGrid" Assembly="Infragistics2.WebUI.UltraWebGrid.v7.3, Version=7.3.20073.38, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>


<asp:Content ID="Content2" ContentPlaceHolderID="ProjectMAIN_content" runat="server">
    <table border="0" cellpadding="0" cellspacing="0" style="width: 950px; height: 450px;">
                <tr>
                    <td style="vertical-align: top; width: 15px; position: static; background-color: white;
                        text-align: left">
                        &nbsp;</td>
                    <td style="vertical-align: top; width: 787px; position: static; text-align: left; margin-left: 10px;">
                        
                        
                        <table border="0" cellpadding="0" cellspacing="0" style="width: 936px">
                            <tr>
                                <td style="width: 70px; vertical-align: top; position: static; text-align: left; height: 36px;">
                                    <span style="font-size: 12pt; color: #b38700; font-family: Arial"><strong><span style="font-size: 14pt">
                                        Select:</span>&nbsp;</strong></span></td>
                                <td style="width: 440px; vertical-align: top; position: static; text-align: left; font-size: 12pt; height: 36px;">
                                    <asp:RadioButtonList ID="rblType" runat="server" AutoPostBack="True" Font-Bold="True"
                                        Font-Names="Arial" Font-Size="10pt" ForeColor="Black" RepeatDirection="Horizontal"
                                        Width="432px" Height="24px">
                                        <asp:ListItem Selected="True" Value="0">In Box (Your Activities)</asp:ListItem>
                                        <asp:ListItem Value="1">Out Box (Activities you Originated)</asp:ListItem>
                                    </asp:RadioButtonList></td>
                                <td style="width: 200px; vertical-align: top; position: static; text-align: left; font-size: 12pt;">
                                    <strong><span style="font-family: Arial"><span style="color: #b38700; font-size: 14pt;">Displaying Activities:</span>
                                    </span></strong></td>
                                <td style="width: 118px; vertical-align: top; position: static; text-align: left; font-size: 12pt; height: 36px;">
                                    <igcmbo:WebCombo ID="ddlType" runat="server" BackColor="White" BorderColor="LightGray"
                                        BorderStyle="Solid" BorderWidth="1px" DropImage1="/ig_common/images/ig_cmbodownxp_olive.gif"
                                        DropImage2="/ig_common/images/ig_cmbodownxp_olive2.gif" DropImageXP1="/ig_common/images/ig_cmbodownxp_olive.gif"
                                        DropImageXP2="/ig_common/images/ig_cmbodownxp_olive2.gif" ForeColor="Black"
                                        SelBackColor="147, 160, 112" SelForeColor="White" Version="3.00" Font-Names="Arial" Font-Size="10pt" Font-Bold="True" Width="180px">
                                        <Columns>
                                            <igtbl1:UltraGridColumn Width="160px">
                                                <header fixedheaderindicator="None">
<RowLayoutColumnInfo SpanY="0"></RowLayoutColumnInfo>
</header>
                                            </igtbl1:UltraGridColumn>
                                        </Columns>
                                        <ExpandEffects ShadowColor="LightGray" Type="Pixelate" />
                                        <DropDownLayout BorderCollapse="Separate" RowHeightDefault="15pt" Version="3.00" RowSelectors="No" TableLayout="Fixed" >
                                            <HeaderStyle BackColor="#8BA169" BorderStyle="Solid">
                                                <BorderDetails ColorLeft="White" ColorTop="White" WidthLeft="1px"
                                                    WidthTop="1px" />
                                            </HeaderStyle>
                                            <FrameStyle BackColor="White" BorderStyle="Ridge" BorderWidth="2px" Cursor="Default"
                                                Font-Names="Arial" Font-Size="10pt" Height="130px" Width="325px">
                                            </FrameStyle>
                                            <RowStyle BorderColor="Gray" BorderStyle="None" BorderWidth="1px" BackColor="White" Font-Names="Arial" Font-Size="10pt">
                                                <BorderDetails WidthLeft="0px" WidthTop="0px" />
                                            </RowStyle>
                                            <SelectedRowStyle BackColor="#93A070"
                                                ForeColor="White" />
                                            <RowAlternateStyle Font-Names="Arial" Font-Size="10pt">
                                            </RowAlternateStyle>
                                            <RowSelectorStyle Font-Names="Arial" Font-Size="10pt">
                                            </RowSelectorStyle>
                                        </DropDownLayout>
                                        <Rows>
                                            <igtbl1:UltraGridRow Height="">
                                                <cells>
<igtbl1:UltraGridCell Text="Open Activities"></igtbl1:UltraGridCell>
</cells>
                                            </igtbl1:UltraGridRow>
                                            <igtbl1:UltraGridRow Height="">
                                                <cells>
<igtbl1:UltraGridCell Text="Completed Activities"></igtbl1:UltraGridCell>
</cells>
                                            </igtbl1:UltraGridRow>
                                            <igtbl1:UltraGridRow Height="">
                                                <cells>
<igtbl1:UltraGridCell Text="All Activities"></igtbl1:UltraGridCell>
</cells>
                                            </igtbl1:UltraGridRow>
                                        </Rows>
                                    </igcmbo:WebCombo>
                                </td>
                            </tr>
                        </table>

			            <asp:Label ID="lblNotfound" runat="server" Font-Bold="True" Font-Names="Arial" ForeColor="DarkRed"
                            Text="Label" Visible="False" Width="700px" Font-Size="11pt"></asp:Label>
                        
                        <igtbl1:ultrawebgrid id="uwgMyTasks" runat="server" Width="944px" Height="417px" style="left: -646px; top: 8px"><Bands>
                            <igtbl1:UltraGridBand AllowSorting="Yes">
                                <AddNewRow View="NotSet" Visible="NotSet">
                                </AddNewRow>
                                <Columns>
                                    <igtbl1:TemplatedColumn HeaderText="Action:" Key="Action" Width="30px">
                                        <HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="10pt" ForeColor="White"
                                            HorizontalAlign="Left" Wrap="True" />
                                        <CellTemplate>
                                            <asp:ImageButton ID="btnAction" runat="server" ImageUrl="~/App_Themes/Billing/Grid Icons/View.gif" OnClick="btnAction_Click"  />
                                        </CellTemplate>
                                        <Header Caption="Action:">
                                        </Header>
                                        <CellStyle HorizontalAlign="Center">
                                        </CellStyle>
                                    </igtbl1:TemplatedColumn>
                                    <igtbl1:UltraGridColumn BaseColumnName="TaskID" HeaderText="Activity ID:" Key="TaskID"
                                        Width="38px">
                                        <HeaderStyle Wrap="True" />
                                        <Header Caption="Activity ID:">
                                            <RowLayoutColumnInfo OriginX="1" />
                                        </Header>
                                        <Footer>
                                            <RowLayoutColumnInfo OriginX="1" />
                                        </Footer>
                                        <CellStyle Font-Names="Arial" Font-Size="9pt" HorizontalAlign="Left">
                                        </CellStyle>
                                    </igtbl1:UltraGridColumn>
                                    <igtbl1:UltraGridColumn BaseColumnName="TaskDescription" HeaderText="Activity Type:"
                                        Key="TaskDescription" Width="100px">
                                        <HeaderStyle Wrap="True" />
                                        <Header Caption="Activity Type:">
                                            <RowLayoutColumnInfo OriginX="2" />
                                        </Header>
                                        <Footer>
                                            <RowLayoutColumnInfo OriginX="2" />
                                        </Footer>
                                        <CellStyle Font-Names="Arial" Font-Size="9pt" ForeColor="#7F5400" Wrap="True">
                                        </CellStyle>
                                    </igtbl1:UltraGridColumn>
                                    <igtbl1:UltraGridColumn BaseColumnName="TargetDate" Format="dd/MM/yy" HeaderText="Complete Activity By:"
                                        Key="Complete By" Width="54px">
                                        <HeaderStyle Wrap="True" />
                                        <Header Caption="Complete Activity By:">
                                            <RowLayoutColumnInfo OriginX="3" />
                                        </Header>
                                        <Footer>
                                            <RowLayoutColumnInfo OriginX="3" />
                                        </Footer>
                                        <CellStyle Font-Names="Arial" Font-Size="9pt">
                                        </CellStyle>
                                    </igtbl1:UltraGridColumn>
                                    <igtbl1:UltraGridColumn BaseColumnName="Comment" HeaderText="Sender's Comment:" Key="Comment"
                                        Width="130px">
                                        <HeaderStyle Wrap="True" />
                                        <Header Caption="Sender's Comment:">
                                            <RowLayoutColumnInfo OriginX="4" />
                                        </Header>
                                        <Footer>
                                            <RowLayoutColumnInfo OriginX="4" />
                                        </Footer>
                                        <CellStyle Font-Names="Arial" Font-Size="9pt">
                                        </CellStyle>
                                    </igtbl1:UltraGridColumn>
                                    <igtbl1:UltraGridColumn BaseColumnName="OriginatorId" HeaderText="Activity Sender:"
                                        Key="UserId" Width="80px">
                                        <HeaderStyle Wrap="True" />
                                        <Header Caption="Activity Sender:">
                                            <RowLayoutColumnInfo OriginX="5" />
                                        </Header>
                                        <Footer>
                                            <RowLayoutColumnInfo OriginX="5" />
                                        </Footer>
                                        <CellStyle Font-Names="Arial" Font-Size="9pt" ForeColor="Black">
                                        </CellStyle>
                                    </igtbl1:UltraGridColumn>
                                    <igtbl1:UltraGridColumn HeaderText="Activity Status:" Key="Status" Width="40px">
                                        <HeaderStyle Wrap="True" />
                                        <Header Caption="Activity Status:">
                                            <RowLayoutColumnInfo OriginX="6" />
                                        </Header>
                                        <Footer>
                                            <RowLayoutColumnInfo OriginX="6" />
                                        </Footer>
                                        <CellStyle Font-Names="Arial" Font-Size="9pt" ForeColor="Black">
                                        </CellStyle>
                                    </igtbl1:UltraGridColumn>
                                    <igtbl1:UltraGridColumn BaseColumnName="JobId" HeaderText="Item ID:" Key="JobId"
                                        Width="38px">
                                        <HeaderStyle Wrap="True" />
                                        <Header Caption="Item ID:">
                                            <RowLayoutColumnInfo OriginX="7" />
                                        </Header>
                                        <Footer>
                                            <RowLayoutColumnInfo OriginX="7" />
                                        </Footer>
                                        <CellStyle Font-Names="Arial" Font-Size="9pt" ForeColor="Black">
                                        </CellStyle>
                                    </igtbl1:UltraGridColumn>
                                    <igtbl1:UltraGridColumn BaseColumnName="JobId" Hidden="True" Key="HiddenJobId">
                                        <Header>
                                            <RowLayoutColumnInfo OriginX="8" />
                                        </Header>
                                        <Footer>
                                            <RowLayoutColumnInfo OriginX="8" />
                                        </Footer>
                                    </igtbl1:UltraGridColumn>
                                    <igtbl1:UltraGridColumn BaseColumnName="NextPage" Hidden="True" Key="HiddenNextPage">
                                        <Header>
                                            <RowLayoutColumnInfo OriginX="9" />
                                        </Header>
                                        <Footer>
                                            <RowLayoutColumnInfo OriginX="9" />
                                        </Footer>
                                    </igtbl1:UltraGridColumn>
                                    <igtbl1:UltraGridColumn BaseColumnName="DateCompleted" HeaderText="DateCompleted"
                                        Hidden="True" Key="HiddenDateCompleted">
                                        <Header Caption="DateCompleted">
                                            <RowLayoutColumnInfo OriginX="10" />
                                        </Header>
                                        <Footer>
                                            <RowLayoutColumnInfo OriginX="10" />
                                        </Footer>
                                    </igtbl1:UltraGridColumn>
                                </Columns>
                            </igtbl1:UltraGridBand>
                        </Bands>
                            <DisplayLayout ColWidthDefault="" AutoGenerateColumns="False" AllowSortingDefault="OnClient" RowHeightDefault="20px"
								Version="4.00" SelectTypeRowDefault="Single" HeaderClickActionDefault="SortSingle" BorderCollapseDefault="Separate"
								AllowColSizingDefault="Free" Name="uwgMyTasks" CellClickActionDefault="RowSelect" RowSelectorsDefault="No">
                                <ActivationObject BorderColor="" BorderWidth="">
                                </ActivationObject>
                                <FooterStyleDefault BorderWidth="1px" BorderStyle="Solid" BackColor="LightGray">
                                    <BorderDetails ColorLeft="White" ColorTop="White" WidthLeft="1px" WidthTop="1px" />
                                </FooterStyleDefault>
                                <RowStyleDefault BorderWidth="1px" BorderColor="White" BorderStyle="Solid" BackColor="#F2F2CC" Font-Names="Arial" Font-Size="9pt" ForeColor="Black">
                                    <BorderDetails WidthLeft="0px" WidthTop="0px" />
                                    <Padding Bottom="2px" Left="4px" Top="2px" />
                                </RowStyleDefault>
                                <SelectedRowStyleDefault ForeColor="Black" BackColor="#FFFFD0" Font-Names="Arial" Font-Size="9pt">
                                    <Padding Bottom="2px" Left="4px" Top="2px" />
                                </SelectedRowStyleDefault>
                                <HeaderStyleDefault BackColor="#4B91CC" BorderStyle="Solid" Font-Bold="True" Font-Names="Arial"
                                    Font-Size="10pt" ForeColor="White" HorizontalAlign="Left" Wrap="True">
                                    <BorderDetails ColorBottom="75, 145, 204" ColorLeft="75, 145, 204" ColorRight="75, 145, 204"
                                        ColorTop="75, 145, 204" WidthLeft="1px" WidthTop="1px" />
                                </HeaderStyleDefault>
                                <RowAlternateStyleDefault BackColor="#EBEBC4" BorderColor="White" Font-Names="Arial"
                                    Font-Size="9pt">
                                    <Padding Bottom="2px" Left="4px" Top="2px" />
                                </RowAlternateStyleDefault>
                                <EditCellStyleDefault BorderWidth="0px" BorderStyle="None">
                                </EditCellStyleDefault>
                                <FrameStyle Width="944px" BorderWidth="1px" Font-Size="8pt" Font-Names="Arial" BorderStyle="Solid"
									Height="417px" BackColor="White">
                                </FrameStyle>
                                <Pager>
                                    <Style BackColor="LightGray" BorderStyle="Solid" BorderWidth="1px">
<BorderDetails ColorTop="White" WidthLeft="1px" WidthTop="1px" ColorLeft="White"></BorderDetails>
</Style>
                                </Pager>
                                <AddNewBox>
                                    <Style BackColor="LightGray" BorderStyle="Solid" BorderWidth="1px">
<BorderDetails ColorTop="White" WidthLeft="1px" WidthTop="1px" ColorLeft="White"></BorderDetails>
</Style>
                                </AddNewBox>
                                 <Images>
                                    <SortAscendingImage Url="/ig_common/images/ig_tblSortAsc_White.gif" />
                                    <SortDescendingImage Url="/ig_common/images/ig_tblSortDesc_White.gif" />
                                </Images>
                           </DisplayLayout>
                        </igtbl1:UltraWebGrid>
                        </td>
                </tr>
            </table>
    &nbsp; &nbsp;&nbsp;



</asp:Content>