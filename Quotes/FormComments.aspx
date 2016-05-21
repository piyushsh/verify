<%@ Page Language="VB"  MasterPageFile= "~/TabPages/ProjectMain.master" AutoEventWireup="false" CodeFile="FormComments.aspx.vb" Inherits="FormComments"  %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register tagprefix="igtbl" namespace="Infragistics.WebUI.UltraWebGrid" Assembly="Infragistics2.WebUI.UltraWebGrid.v7.3, Version=7.3.20073.38, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>



<asp:Content ID="Content2" ContentPlaceHolderID="ProjectMAIN_content" runat="server">


<style type="text/css">
   .modalBackground
   {
       filter:alpha(opacity=70);
       opacity:0.7;
       background-color:Gray;
   }
</style>
    
<body>

    <script type="text/javascript">

        function ClearComment() {
            var activeRow = igtbl_getActiveRow("<%=ApplicationComments.ClientID%>");

            if (activeRow == null) {
                var hdn1 = document.getElementById("<%=hdnNoCommentSelected.ClientID%>");
                alert(hdn1.value);
                return false;
            }
        }

        function LoadCommentPanel() 
        {
            var modal = $find("<%=ModalPopupExtender1.BehaviorID%>");
            modal.show(); 
            return false;
        }
        
        function LoadNewThreadPanel() 
        {
            var modal = $find("<%=ModalPopupExtender2.BehaviorID%>");
            modal.show(); 
            return false;
        }
        
        function goToCommentControl() 
        {
            var activeRow=igtbl_getActiveRow("<%=ApplicationThreads.ClientID%>");        
            
            if (activeRow == null){
                var hdn1=document.getElementById("<%=hdnNoThreadSelected.ClientID%>");
                alert (hdn1.value);
                return false;
                }
            else 
                {
                var Control = activeRow.getCellFromKey('hdnControl').getValue();
                if (Control == null){
                    var hdn2=document.getElementById("<%=hdnNoPageForComment.ClientID%>");
                    alert (hdn2.value);
                    return false;
                    }
                else
                    return true; 
                }
        }
        

        function HideModalPopup() 
        { 
            var modal = $find("<%=ModalPopupExtender1.BehaviorID%>");
            modal.hide(); 
        }
        
        function HideModalPopup2() 
        { 
            var modal = $find("<%=ModalPopupExtender2.BehaviorID%>");
            modal.hide(); 
        }
        
        function AddNewCommentClickUpdate(sender, e) 
        { 
            var txt1=document.getElementById("<%=txtComment.ClientID%>"); 
         
            
            if (txt1.value == ""){
                var hdn2=document.getElementById("<%=hdnNoCommentPrompt.ClientID%>");
                alert (hdn2.value);
                return false;
                }
            else
                __doPostBack(sender,e); 
        } 
  
        function AddNewThreadClickUpdate(sender, e) 
        { 
            var txt1=document.getElementById("<%=txtNewThreadComment.ClientID%>"); 
         
            
            if (txt1.value == ""){
                var hdn2=document.getElementById("<%=hdnNoCommentPrompt.ClientID%>");
                alert (hdn2.value);
                return false;
                }
            else
                __doPostBack(sender,e); 
        } 
        
        
    </script>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div style="padding: 10">
            <center>
                <br />
                <table style="width: 100%">
                    <tr>
                        <td style="width: 550px; vertical-align: top; text-align: left">
                            &nbsp;
                            <asp:Label ID="Label4" runat="server" Font-Bold="True" Font-Names="Arial" 
                                Font-Size="12pt" ForeColor="#B38700" Text="Comment Threads for :"></asp:Label>
                            &nbsp;<asp:Label ID="lblThreadsTitle" runat="server" Font-Bold="True" 
                                Font-Names="Arial" Font-Size="10pt"></asp:Label>
                        </td>
                        <td>
                            <asp:ImageButton ID="btnBack" runat="server" 
                                ImageUrl="~/App_Themes/Billing/Buttons/Back-Button-Black.gif" />
                        </td>
                        <td>
                            &nbsp;</td>
                    </tr>
                </table>
                <table align="left" cellpadding="0" cellspacing="0" 
                    style="width: 920px; float: left">
                    <tr>
                        <td style="width: 880px; vertical-align: top; text-align: left; font-family: Arial">
                            <igtbl:UltraWebGrid ID="ApplicationThreads" runat="server" Height="120px" 
                                meta:resourcekey="ApplicationCommentsResource1" 
                                style="top: 17px; left: 176px; height: 155px; width: 700px;" Width="880px">
                                <Bands>
                                    <igtbl:UltraGridBand>
                                        <Columns>
                                            <igtbl:UltraGridColumn BaseColumnName="Field0" HeaderText="Id:" Hidden="True" 
                                                Key="Id" Width="50px">
                                                <Header Caption="Id:">
                                                    <RowLayoutColumnInfo OriginX="2" />
                                                </Header>
                                                <Footer>
                                                    <RowLayoutColumnInfo OriginX="2" />
                                                </Footer>
                                            </igtbl:UltraGridColumn>
                                            <igtbl:UltraGridColumn BaseColumnName="Field1" HeaderText="Comment:" 
                                                Key="Comment" Width="450px">
                                                <Header Caption="Comment:">
                                                    <RowLayoutColumnInfo OriginX="3" />
                                                </Header>
                                                <Footer>
                                                    <RowLayoutColumnInfo OriginX="3" />
                                                </Footer>
                                            </igtbl:UltraGridColumn>
                                            <igtbl:UltraGridColumn BaseColumnName="Field2" HeaderText="Date Created:" 
                                                Key="DateCreated" Width="120px">
                                                <Header Caption="Date Created:">
                                                    <RowLayoutColumnInfo OriginX="4" />
                                                </Header>
                                                <Footer>
                                                    <RowLayoutColumnInfo OriginX="4" />
                                                </Footer>
                                            </igtbl:UltraGridColumn>
                                            <igtbl:UltraGridColumn BaseColumnName="Field3" HeaderText="Created By:" 
                                                Key="CreatedBy" Width="150px">
                                                <Header Caption="Created By:">
                                                    <RowLayoutColumnInfo OriginX="4" />
                                                </Header>
                                                <Footer>
                                                    <RowLayoutColumnInfo OriginX="4" />
                                                </Footer>
                                            </igtbl:UltraGridColumn>
                                            <igtbl:UltraGridColumn BaseColumnName="Field6" Hidden="True" Key="hdnPage">
                                                <Header>
                                                    <RowLayoutColumnInfo OriginX="7" />
                                                </Header>
                                                <Footer>
                                                    <RowLayoutColumnInfo OriginX="7" />
                                                </Footer>
                                            </igtbl:UltraGridColumn>
                                            <igtbl:UltraGridColumn BaseColumnName="Field7" Hidden="True" Key="hdnControl">
                                                <Header>
                                                    <RowLayoutColumnInfo OriginX="8" />
                                                </Header>
                                                <Footer>
                                                    <RowLayoutColumnInfo OriginX="8" />
                                                </Footer>
                                            </igtbl:UltraGridColumn>
                                            <igtbl:UltraGridColumn BaseColumnName="Field8" HeaderText="Field Linked?" 
                                                Key="FieldLinked" Type="CheckBox" Width="50px">
                                                <CellStyle HorizontalAlign="Center">
                                                </CellStyle>
                                                <Header Caption="Field Linked?">
                                                    <RowLayoutColumnInfo OriginX="9" />
                                                </Header>
                                                <Footer>
                                                    <RowLayoutColumnInfo OriginX="9" />
                                                </Footer>
                                            </igtbl:UltraGridColumn>
                                            <igtbl:UltraGridColumn BaseColumnName="Field9" HeaderText="Items in Thread:" 
                                                Key="NumComments" Width="50px">
                                                <CellStyle HorizontalAlign="Center">
                                                </CellStyle>
                                                <Header Caption="Items in Thread:">
                                                    <RowLayoutColumnInfo OriginX="10" />
                                                </Header>
                                                <Footer>
                                                    <RowLayoutColumnInfo OriginX="10" />
                                                </Footer>
                                            </igtbl:UltraGridColumn>
                                        </Columns>
                                        <AddNewRow View="NotSet" Visible="NotSet">
                                        </AddNewRow>
                                    </igtbl:UltraGridBand>
                                </Bands>
                                <DisplayLayout AllowColSizingDefault="Free" AllowDeleteDefault="Yes" 
                                    AllowSortingDefault="OnClient" AutoGenerateColumns="False" 
                                    BorderCollapseDefault="Separate" CellClickActionDefault="RowSelect" 
                                    HeaderClickActionDefault="SortMulti" Name="UltraWebGrid1" 
                                    RowHeightDefault="20px" RowSelectorsDefault="No" 
                                    SelectTypeRowDefault="Extended" 
                                    StationaryMarginsOutlookGroupBy="True" TableLayout="Fixed" Version="4.00">
                                    <FilterOptionsDefault>
                                        <FilterDropDownStyle BackColor="White" BorderColor="Silver" BorderStyle="Solid" 
                                            BorderWidth="1px" CustomRules="overflow:auto;" 
                                            Font-Names="Verdana,Arial,Helvetica,sans-serif" Font-Size="11px" Height="300px" 
                                            Width="200px">
                                            <Padding Left="2px" />
                                        </FilterDropDownStyle>
                                        <FilterHighlightRowStyle BackColor="#151C55" ForeColor="White">
                                        </FilterHighlightRowStyle>
                                        <FilterOperandDropDownStyle BackColor="White" BorderColor="Silver" 
                                            BorderStyle="Solid" BorderWidth="1px" CustomRules="overflow:auto;" 
                                            Font-Names="Verdana,Arial,Helvetica,sans-serif" Font-Size="11px">
                                            <Padding Left="2px" />
                                        </FilterOperandDropDownStyle>
                                    </FilterOptionsDefault>
                                    <AddNewBox Hidden="False">
                                        <Style BackColor="Window" BorderColor="InactiveCaption" BorderStyle="Solid" 
                                            BorderWidth="1px">
                                            <BorderDetails ColorLeft="White" ColorTop="White" WidthLeft="1px" 
                                                WidthTop="1px" />
                                        <BorderDetails ColorLeft="White" ColorTop="White" WidthLeft="1px" 
                                                WidthTop="1px" /><BorderDetails ColorLeft="White" ColorTop="White" WidthLeft="1px" 
                                                WidthTop="1px" /><BorderDetails ColorLeft="White" ColorTop="White" WidthLeft="1px" 
                                                WidthTop="1px" /></Style>
                                    </AddNewBox>
                                    <GroupByBox>
                                        <Style BackColor="ActiveBorder" BorderColor="Window">
                                        </Style>
                                    </GroupByBox>
                                    <HeaderStyleDefault BackColor="#B38700" BorderStyle="Solid" font-names="Arial" 
                                        font-size="10pt" forecolor="White" HorizontalAlign="Left">
                                        <BorderDetails colorbottom="179, 135, 0" ColorLeft="179, 135, 0" 
                                            colorright="179, 135, 0" ColorTop="179, 135, 0" WidthLeft="1px" 
                                            WidthTop="1px" />
                                    </HeaderStyleDefault>
                                    <FrameStyle BackColor="Window" BorderColor="InactiveCaption" 
                                        BorderStyle="Solid" BorderWidth="1px" Font-Names="Microsoft Sans Serif" 
                                        Font-Size="8.25pt" Height="120px" Width="880px">
                                    </FrameStyle>
                                    <EditCellStyleDefault BorderStyle="None" BorderWidth="0px">
                                    </EditCellStyleDefault>
                                    <FooterStyleDefault BackColor="LightGray" BorderStyle="Solid" BorderWidth="1px">
                                        <BorderDetails ColorLeft="White" ColorTop="White" WidthLeft="1px" 
                                            WidthTop="1px" />
                                    </FooterStyleDefault>
                                    <RowStyleDefault BackColor="#F2F2CC" BorderColor="Silver" BorderStyle="Solid" 
                                        BorderWidth="1px" Font-Names="Arial" Font-Size="9pt" ForeColor="Black" 
                                        wrap="True">
                                        <Padding Left="2px" />
                                        <BorderDetails colorbottom="White" ColorLeft="White" colorright="White" 
                                            ColorTop="White" widthleft="0px" widthtop="0px" />
                                    </RowStyleDefault>
                                    <GroupByRowStyleDefault BackColor="Control" BorderColor="Window">
                                    </GroupByRowStyleDefault>
                                    <ActivationObject BorderColor="" BorderWidth="">
                                    </ActivationObject>
                                </DisplayLayout>
                            </igtbl:UltraWebGrid>
                        </td>
                        <td style="width: 40px">
                            &nbsp;</td>
                    </tr>
                </table>
                <table align="left" cellpadding="0" cellspacing="0" 
                    style="width: 920px; float: left">
                    <tr>
                        <td style="width: 50px; vertical-align: top; text-align: left;" rowspan="4">
                            <asp:Image ID="Image3" runat="server" 
                                ImageUrl="~/App_Themes/Billing/Buttons/LinkGraphic.gif" />
                        </td>
                        <td style="width: 870px; vertical-align: top; text-align: left;">
                            &nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:ImageButton ID="btnNewThread" runat="server" 
                                AlternateText="Creat New Thread" 
                                ImageUrl="~/App_Themes/Billing/Buttons/Create-New-Thread.gif" />
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:ImageButton ID="btnGoToControl" runat="server" 
                                AlternateText="Go to Associated Form Field" 
                                ImageUrl="~/App_Themes/Billing/Buttons/Go-To-Form-Field.gif" />
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 870px; vertical-align: top; text-align: left;">
                            &nbsp;&nbsp; &nbsp;</td>
                    </tr>
                    <tr>
                        <td style="vertical-align: top; text-align: left">
                            &nbsp;&nbsp;&nbsp; &nbsp;<asp:Label ID="lblCommentsTitle" runat="server" Font-Bold="False" 
                                Font-Names="Arial" Font-Size="11pt">These are the comments for the selected 
                            Thread</asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="vertical-align: top; text-align: left">
                            <igtbl:UltraWebGrid ID="ApplicationComments" runat="server" Height="200px" 
                                meta:resourcekey="ApplicationCommentsResource1" 
                                style="top: -2px; left: 0px; height: 200px; width: 700px;" Width="870px">
                                <Bands>
                                    <igtbl:UltraGridBand>
                                        <Columns>
                                            <igtbl:UltraGridColumn BaseColumnName="Field0" HeaderText="Id:" Hidden="True" 
                                                Key="Id" Width="50px">
                                                <Header Caption="Id:">
                                                </Header>
                                            </igtbl:UltraGridColumn>
                                            <igtbl:UltraGridColumn BaseColumnName="Field4" HeaderText="Date Viewed:" 
                                                Key="DateCleared" Width="70px">
                                                
                                                <Header Caption="Date Viewed:">
                                                    <RowLayoutColumnInfo OriginX="1" />
                                                </Header>
                                                <Footer>
                                                    <RowLayoutColumnInfo OriginX="1" />
                                                </Footer>
                                                
                                            </igtbl:UltraGridColumn>
                                            <igtbl:UltraGridColumn BaseColumnName="Field5" HeaderText="Viewed By:" 
                                                Key="ClearedBy" Width="80px">
                                                <Header Caption="Viewed By:">
                                                    <RowLayoutColumnInfo OriginX="2" />
                                                </Header>
                                                <Footer>
                                                    <RowLayoutColumnInfo OriginX="2" />
                                                </Footer>
                                            </igtbl:UltraGridColumn>
                                            <igtbl:UltraGridColumn BaseColumnName="Field9" Key="Target" 
                                                HeaderText="Target:" Width="80px">
                                                <Header Caption="Target:">
                                                    <RowLayoutColumnInfo OriginX="3" />
                                                </Header>
                                                <Footer>
                                                    <RowLayoutColumnInfo OriginX="3" />
                                                </Footer>
                                            </igtbl:UltraGridColumn>
                                            <igtbl:TemplatedColumn BaseColumnName="Field1" HeaderText="Comment:" 
                                                Key="Comment" Width="350px">
                                                <CellTemplate>
                                                   <table align="left" cellpadding="0" cellspacing="0"  
                                                            style="font-family: Arial; font-size: 11px; color: #000000; width: 350px;" ID="GreenBubble">
                                                        <tr style="height: 15px">
                                                            <td style="vertical-align: bottom; text-align: right; background-image: url('../App_Themes/BubbleImages/Green_Top_left_wbg.jpg'); background-repeat: no-repeat; width: 20px; height: 15px;">&nbsp;</td>
                                                            <td style="background-image: url('../App_Themes/BubbleImages/Green_Top_Shadow_wbg.jpg'); background-repeat: repeat-x; vertical-align: bottom; text-align: left; height: 15px">&nbsp;</td>
                                                            <td style="background-image: url('../App_Themes/BubbleImages/Green_Top_Right_wbg.jpg'); background-repeat: no-repeat; vertical-align: bottom; text-align: left; height: 15px">&nbsp;</td>
                                                        </tr>
                                                        <tr>
                                                            <td style="background-image: url('../App_Themes/BubbleImages/Green_Left_shadow_wbg.jpg'); background-repeat: repeat-y">&nbsp;</td>
                                                            <td style="background-color: #83D92E">
                                                            <%#DataBinder.Eval(Container.DataItem, "Field1")%></td>
                                                            <td style="background-image: url('../App_Themes/BubbleImages/Green_Right_shadow_wbg.jpg'); background-repeat: repeat-y; vertical-align: top; text-align: left">&nbsp;</td>
                                                        </tr>
                                                        <tr>
                                                            <td style="background-image: url('../App_Themes/BubbleImages/Green_bot_left_wbg.jpg'); background-repeat: no-repeat; vertical-align: top; text-align: right; width: 20px;">&nbsp;</td>
                                                            <td style="background-image: url('../App_Themes/BubbleImages/Green_Bot_shadow_wbg.jpg'); background-repeat: repeat-x">&nbsp;</td>
                                                            <td style="background-image: url('../App_Themes/BubbleImages/Green_Bot_Right_wbg.jpg'); background-repeat: no-repeat; width: 20px;">&nbsp;</td>
                                                        </tr>
                                                        
                                                    </table
 
            
                                                </CellTemplate>
                                                
                                                    <cellstyle wrap="True" backcolor="White">
                                                    </cellstyle>
                                                
                                                <Header Caption="Comment:">
                                                    
                                                <RowLayoutColumnInfo OriginX="4" /><RowLayoutColumnInfo OriginX="4" /><RowLayoutColumnInfo OriginX="4" /><RowLayoutColumnInfo OriginX="4" /></Header>
                                                <Footer>
                                                    
                                                <RowLayoutColumnInfo OriginX="4" /><RowLayoutColumnInfo OriginX="4" /><RowLayoutColumnInfo OriginX="4" /><RowLayoutColumnInfo OriginX="4" /></Footer>
                                            </igtbl:TemplatedColumn>
                                            <igtbl:UltraGridColumn BaseColumnName="Field8" HeaderText="Source:" 
                                                Key="Target" Width="80px">
                                                <CellStyle Font-Names="Arial" Font-Size="8pt" Wrap="True">
                                                </CellStyle>
                                                <Header Caption="Target:">
                                                    <RowLayoutColumnInfo OriginX="3" />
                                                </Header>
                                                <Footer>
                                                    <RowLayoutColumnInfo OriginX="3" />
                                                </Footer>
                                                <CellStyle Font-Names="Arial" Font-Size="8pt" Wrap="True">
                                                </CellStyle>
                                                <Header Caption="Source:">
                                                    <RowLayoutColumnInfo OriginX="5" />
                                                </Header>
                                                <Footer>
                                                    <RowLayoutColumnInfo OriginX="5" />
                                                </Footer>
                                            </igtbl:UltraGridColumn>
                                            <igtbl:UltraGridColumn BaseColumnName="Field3" HeaderText="Created By:" 
                                                Key="CreatedBy" Width="90px">
                                                <CellStyle Font-Names="Arial" Font-Size="9pt" Wrap="True">
                                                </CellStyle>
                                                <Header Caption="Created By:">
                                                    <RowLayoutColumnInfo OriginX="6" />
                                                </Header>
                                                <Footer>
                                                    <RowLayoutColumnInfo OriginX="6" />
                                                </Footer>
                                            </igtbl:UltraGridColumn>
                                            <igtbl:UltraGridColumn BaseColumnName="Field2" HeaderText="Date Created:" 
                                                Key="DateCreated" Width="70px">
                                                <CellStyle Font-Names="Arial" Font-Size="9pt">
                                                </CellStyle>
                                                <Header Caption="Date Created:">
                                                    <RowLayoutColumnInfo OriginX="7" />
                                                </Header>
                                                <Footer>
                                                    <RowLayoutColumnInfo OriginX="7" />
                                                </Footer>
                                            </igtbl:UltraGridColumn>
                                            <igtbl:UltraGridColumn BaseColumnName="Field6" 
                                                Key="hdnPage" Hidden="True">
                                                <Header>
                                                    <RowLayoutColumnInfo OriginX="8" />
                                                </Header>
                                                <Footer>
                                                    <RowLayoutColumnInfo OriginX="8" />
                                                </Footer>
                                            </igtbl:UltraGridColumn>
                                            <igtbl:UltraGridColumn BaseColumnName="Field7" Hidden="True" Key="hdnControl">
                                                <Header>
                                                    <RowLayoutColumnInfo OriginX="9" />
                                                </Header>
                                                <Footer>
                                                    <RowLayoutColumnInfo OriginX="9" />
                                                </Footer>
                                            </igtbl:UltraGridColumn>
                                        </Columns>
                                        <AddNewRow View="NotSet" Visible="NotSet">
                                        </AddNewRow>
                                    </igtbl:UltraGridBand>
                                </Bands>
                                <DisplayLayout AllowColSizingDefault="Free" AllowDeleteDefault="Yes" 
                                    AllowSortingDefault="OnClient" AllowUpdateDefault="Yes" 
                                    AutoGenerateColumns="False" BorderCollapseDefault="Separate" 
                                    CellClickActionDefault="RowSelect" HeaderClickActionDefault="SortMulti" 
                                    Name="UltraWebGrid1" RowHeightDefault="20px" RowSelectorsDefault="No" 
                                    SelectTypeRowDefault="Extended"  
                                    StationaryMarginsOutlookGroupBy="True" TableLayout="Fixed" Version="4.00">
                                    <RowAlternateStyleDefault BackColor="White">
                                        <BorderDetails ColorBottom="SlateGray" ColorLeft="White" ColorRight="White" 
                                            ColorTop="White" />
                                    </RowAlternateStyleDefault>
                                    <Pager MinimumPagesForDisplay="2">
                                        <Style BackColor="LightGray" BorderStyle="Solid" BorderWidth="1px">
                                            <BorderDetails ColorLeft="White" ColorTop="White" WidthLeft="1px" 
                                                WidthTop="1px" />
                                        <BorderDetails ColorLeft="White" ColorTop="White" WidthLeft="1px" 
                                                WidthTop="1px" /><BorderDetails ColorLeft="White" ColorTop="White" WidthLeft="1px" 
                                                WidthTop="1px" /><BorderDetails ColorLeft="White" ColorTop="White" WidthLeft="1px" 
                                                WidthTop="1px" /></Style>
                                        <PagerStyle BackColor="LightGray" BorderStyle="Solid" BorderWidth="1px">
                                        <BorderDetails ColorLeft="White" ColorTop="White" WidthLeft="1px" 
                                            WidthTop="1px" />
                                        </PagerStyle>
                                    </Pager>
                                    <FilterOptionsDefault>
                                        <FilterDropDownStyle BackColor="White" BorderColor="Silver" BorderStyle="Solid" 
                                            BorderWidth="1px" CustomRules="overflow:auto;" 
                                            Font-Names="Verdana,Arial,Helvetica,sans-serif" Font-Size="11px" Height="300px" 
                                            Width="200px">
                                            <Padding Left="2px" />
                                        </FilterDropDownStyle>
                                        <FilterHighlightRowStyle BackColor="#151C55" ForeColor="White">
                                        </FilterHighlightRowStyle>
                                        <FilterOperandDropDownStyle BackColor="White" BorderColor="Silver" 
                                            BorderStyle="Solid" BorderWidth="1px" CustomRules="overflow:auto;" 
                                            Font-Names="Verdana,Arial,Helvetica,sans-serif" Font-Size="11px">
                                            <Padding Left="2px" />
                                        </FilterOperandDropDownStyle>
                                    </FilterOptionsDefault>
                                    <AddNewBox Hidden="False">
                                        <Style BackColor="Window" BorderColor="InactiveCaption" BorderStyle="Solid" 
                                            BorderWidth="1px">
                                            <BorderDetails ColorLeft="White" ColorTop="White" WidthLeft="1px" 
                                                WidthTop="1px" />
                                        <BorderDetails ColorLeft="White" ColorTop="White" WidthLeft="1px" 
                                                WidthTop="1px" /><BorderDetails ColorLeft="White" ColorTop="White" WidthLeft="1px" 
                                                WidthTop="1px" /><BorderDetails ColorLeft="White" ColorTop="White" WidthLeft="1px" 
                                                WidthTop="1px" /></Style>
                                        <BoxStyle BackColor="Window" BorderColor="InactiveCaption" BorderStyle="Solid" 
                                            BorderWidth="1px">
                                            <BorderDetails ColorLeft="White" ColorTop="White" WidthLeft="1px" 
                                                WidthTop="1px" />
                                        </BoxStyle>
                                    </AddNewBox>
                                    <GroupByBox>
                                        <Style BackColor="ActiveBorder" BorderColor="Window">
                                        </Style>
                                        <BoxStyle BackColor="ActiveBorder" BorderColor="Window">
                                        </BoxStyle>
                                    </GroupByBox>
                                    <FrameStyle BackColor="Window" BorderColor="InactiveCaption" 
                                        BorderStyle="Solid" BorderWidth="1px" Font-Names="Microsoft Sans Serif" 
                                        Font-Size="8.25pt" Height="200px" Width="870px">
                                    </FrameStyle>
                                    <EditCellStyleDefault BorderStyle="None" BorderWidth="0px">
                                    </EditCellStyleDefault>
                                    <FooterStyleDefault BackColor="LightGray" BorderStyle="Solid" BorderWidth="1px">
                                        <BorderDetails ColorLeft="White" ColorTop="White" WidthLeft="1px" 
                                            WidthTop="1px" />
                                    </FooterStyleDefault>
                                    <HeaderStyleDefault BackColor="#715500" BorderStyle="Solid" font-names="Arial" 
                                        font-size="10pt" forecolor="White" HorizontalAlign="Left">
                                        <BorderDetails colorbottom="113, 85, 0" ColorLeft="113, 85, 0" 
                                            colorright="113, 85, 0" ColorTop="113, 85, 0" WidthLeft="1px" 
                                            WidthTop="1px" />
                                    </HeaderStyleDefault>
                                    <RowStyleDefault BackColor="White" BorderColor="Silver" BorderStyle="Solid" 
                                        BorderWidth="1px" Font-Names="Arial" Font-Size="9pt" ForeColor="Black" 
                                        wrap="True">
                                        <Padding Left="2px" />
                                        <BorderDetails colorbottom="SlateGray" ColorLeft="White" colorright="White" 
                                            ColorTop="White" widthleft="0px" widthtop="0px" />
                                    </RowStyleDefault>
                                    <GroupByRowStyleDefault BackColor="Control" BorderColor="Window">
                                    </GroupByRowStyleDefault>
                                    <ActivationObject BorderColor="" BorderWidth="">
                                    </ActivationObject>
                                </DisplayLayout>
                            </igtbl:UltraWebGrid>
                        </td>
                    </tr>
                </table>
                <br />
            <br />
            <br />
                
                
                
                
            
            </center>
            </div>
            
        </ContentTemplate>
           
    </asp:UpdatePanel>
    
    
    
            <table style="width: 100%">
                <tr>
                    <td>
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>
                    <td>
                        
                        <asp:ImageButton ID="btnAddComment" runat="server" 
                            AlternateText="Add New Comment" 
                            
                            ImageUrl="~/App_Themes/Billing/Buttons/Add-New-Comment.gif" />
                    </td>
                    <td>
                        <asp:ImageButton ID="btnClearComment" runat="server" 
                            AlternateText="Clear Selected Comment" 
                            
                            ImageUrl="~/App_Themes/Billing/Buttons/Mark-Selected-Comment-Viewed.gif" />
                    </td>
                    <td>
                        <asp:ImageButton ID="cmdClearAllComments" runat="server" 
                            AlternateText="Clear All Comments" 
                            ImageUrl="~/App_Themes/Billing/Buttons/Mark-All-Comments-Viewed.gif"  />
                    </td>
                    <td>
                        &nbsp;</td>
                </tr>
            </table>

    
            <asp:Panel ID="pnlNewComment"  style = "display: none" runat="server" BackColor="White" 
                meta:resourcekey="pnlCommentResource1" Width="750px">
                <div style="padding: 20">
                    <center>
            <table style="width: 100%">
                <tr>
                    <td>
                        <asp:Label ID="lblNewCommentCaption" runat="server" Font-Bold="True" 
                            Font-Names="Arial" Font-Size="10pt" 
                            Text="Use this panel to add a Comment to the selected Thread."></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblCommentTarget" runat="server" Font-Names="Arial" Font-Size="10pt" 
                            Text="Who is the Target for this Comment ?"></asp:Label>
                    
                        &nbsp;&nbsp;
                        <asp:DropDownList ID="ddlCommentTarget" runat="server" Height="20px" 
                            Width="165px">
                            <asp:ListItem></asp:ListItem>
                        </asp:DropDownList>
                    
                    </td>
                </tr>
            </table>

                    
                        <table style="width: 100%">
                            <tr>
                                <td style="text-align: right; width: 224px;">
                                    <asp:Label ID="lblComment" runat="server" Font-Bold="True" Font-Names="Arial" 
                                        Font-Size="11pt" meta:resourcekey="lblCommentResource1" 
                                        Text="Enter your comment here:"></asp:Label>
                                </td>
                                <td style="text-align: left; vertical-align: top;">
                                    <asp:TextBox ID="txtComment" runat="server" Font-Names="Arial" Font-Size="9pt" 
                                        Height="50px" meta:resourcekey="txtCommentResource1" TextMode="MultiLine" 
                                        Width="523px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 224px">
                                    <asp:ImageButton ID="cmdNewCancel" runat="server" 
                                        ImageUrl="~/App_Themes/Billing/Buttons/Cancel.gif" 
                                        meta:resourcekey="cmdNewCancelResource1" />
                                </td>
                                <td>
                                    <asp:ImageButton ID="cmdNewOk" runat="server" 
                                        ImageUrl="~/App_Themes/Billing/Buttons/Ok.gif" 
                                        meta:resourcekey="cmdNewOkResource1" />
                                </td>
                            </tr>
                        </table>
                        <br />
                    </center>
                </div>
            </asp:Panel>
            <cc1:ModalPopupExtender ID="ModalPopupExtender1" runat="server" 
                BackgroundCssClass="modalBackground" CancelControlID="cmdNewCancel" 
                DropShadow="True" DynamicServicePath="" Enabled="True" 
                OnCancelScript="HideModalPopup()" X=100 Y=100 PopupControlID="pnlNewComment" 
                TargetControlID="pnlNewComment">
            </cc1:ModalPopupExtender>
 
 
             <br />
 
 
             <asp:Panel ID="pnlNewThread"  runat="server" BackColor="White" 
                meta:resourcekey="pnlNewThreadResource1" Width="750px">
                <div style="padding: 20">
                    <center>
            <table style="width: 100%">
                <tr>
                    <td>
                        <asp:Label ID="lblNewThreadCaption" runat="server" Font-Bold="True" 
                            Font-Names="Arial" Font-Size="10pt" 
                            Text="Use this panel to create a new Thread."></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblNewThreadTarget" runat="server" Font-Names="Arial" Font-Size="10pt" 
                            Text="Who is the Target for this Comment ?"></asp:Label>
                    
                        &nbsp;&nbsp;
                        <asp:DropDownList ID="ddlNewThreadTarget" runat="server" Height="20px" 
                            Width="165px">
                            <asp:ListItem></asp:ListItem>
                        </asp:DropDownList>
                    
                    </td>
                </tr>
            </table>

                    
                        <table style="width: 100%">
                            <tr>
                                <td style="text-align: right; width: 224px;">
                                    <asp:Label ID="lblNewThreadComment" runat="server" Font-Bold="True" Font-Names="Arial" 
                                        Font-Size="11pt" meta:resourcekey="lblNewThreadCommentResource1" 
                                        Text="Enter your comment here:"></asp:Label>
                                </td>
                                <td style="text-align: left; vertical-align: top;">
                                    <asp:TextBox ID="txtNewThreadComment" runat="server" Font-Names="Arial" Font-Size="9pt" 
                                        Height="50px" meta:resourcekey="txtNewThreadCommentResource1" TextMode="MultiLine" 
                                        Width="523px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 224px">
                                    <asp:ImageButton ID="cmdNewThreadCancel" runat="server" 
                                        ImageUrl="~/App_Themes/Billing/Buttons/Cancel.gif" 
                                        meta:resourcekey="cmdNewThreadCancelResource1" />
                                </td>
                                <td>
                                    <asp:ImageButton ID="cmdNewThreadOk" runat="server" 
                                        ImageUrl="~/App_Themes/Billing/Buttons/OK.gif" 
                                        meta:resourcekey="cmdNewThreadOkResource1" />
                                </td>
                            </tr>
                        </table>
                    </center>
                </div>
            </asp:Panel>
            <cc1:ModalPopupExtender ID="ModalPopupExtender2" runat="server" 
                BackgroundCssClass="modalBackground" CancelControlID="cmdNewThreadCancel" 
                DropShadow="True" DynamicServicePath="" Enabled="True" 
                OnCancelScript="HideModalPopup2()" X=100 Y=100 PopupControlID="pnlNewThread" 
                TargetControlID="pnlNewThread">
            </cc1:ModalPopupExtender>
           
         <br />
           
    <asp:HiddenField ID="hdnNoThreadSelected" runat="server" />
    <asp:HiddenField ID="hdnNoPageForComment" runat="server" />
    <asp:HiddenField ID="hdnNoCommentPrompt" runat="server" />
    <asp:HiddenField ID="hdnNoCommentSelected" runat="server" />
    

</body>

</asp:Content>
