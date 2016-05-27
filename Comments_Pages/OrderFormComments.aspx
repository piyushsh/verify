<%@ Page Language="VB"  MasterPageFile= "~/TabPages/ProjectMain.master" AutoEventWireup="false" EnableEventValidation="false" CodeFile="OrderFormComments.aspx.vb" Inherits="Order_OrderFormComment"  %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<%@ Register assembly="Infragistics4.Web.v13.2, Version=13.2.20132.2028, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" namespace="Infragistics.Web.UI.GridControls" tagprefix="ig" %>


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
            var myCell = getActiveCell("<%=wdgApplicationThreads.ClientID%>");

            if (myCell == null) {
                var hdn1 = document.getElementById("<%=hdnNoThreadSelected.ClientID%>");
                alert(hdn1.value);
                return false;
            }
            myCell = getActiveCell("<%=wdgApplicationComments.ClientID%>");

            if (myCell == null) {
                var hdn1 = document.getElementById("<%=hdnNoCommentSelected.ClientID%>");
                alert(hdn1.value);
                return false;
            }
        }


        function LoadCommentPanel() {

            var myCell = getActiveCell("<%=wdgApplicationThreads.ClientID%>");

            if (myCell == null) {
                var hdn1 = document.getElementById("<%=hdnNoThreadSelected.ClientID%>");
                alert(hdn1.value);
                return false;
            }

            var txt1 = document.getElementById("<%=txtComment.ClientID%>");
            txt1.value = "";
            var modal = $find("<%=ModalPopupExtender1.BehaviorID%>");
            modal.show();

            //  enable the ok button
            var cmdOK = document.getElementById("<%=cmdNewOk.ClientID%>");
            cmdOK.disabled = false;

            return false;
        }

        function LoadNewThreadPanel() {
            var modal = $find("<%=ModalPopupExtender2.BehaviorID%>");
            var txt1 = document.getElementById("<%=txtNewThreadComment.ClientID%>");
            txt1.value = "";

            modal.show();

            //  enable the ok button
            var cmdOK = document.getElementById("<%=cmdNewThreadOk.ClientID%>");
            cmdOK.disabled = false;
            return false;
        }

        function goToCommentControl() {
            var myCell = getActiveCell("<%=wdgApplicationThreads.ClientID%>");

            if (myCell == null) {
                var hdn1 = document.getElementById("<%=hdnNoThreadSelected.ClientID%>");
                alert(hdn1.value);
                return false;
            }
            else {
                var activeRow = getRowForCell(myCell);

                var Control = getCellValueFromKey(activeRow, 'hdnControl');
                if (Control == null) {
                    var hdn2 = document.getElementById("<%=hdnNoPageForComment.ClientID%>");
                    alert(hdn2.value);
                    return false;
                }
                else
                    return true;
            }
        }


        function HideModalPopup() {
            var modal = $find("<%=ModalPopupExtender1.BehaviorID%>");
            modal.hide();
        }

        function HideModalPopup2() {
            var modal = $find("<%=ModalPopupExtender2.BehaviorID%>");
            modal.hide();
        }

        function AddNewCommentClickUpdate(sender, e) {
            var txt1 = document.getElementById("<%=txtComment.ClientID%>");
            var cmdOK = document.getElementById("<%=cmdNewOk.ClientID%>");


            if (txt1.value == "") {
                var hdn2 = document.getElementById("<%=hdnNoCommentPrompt.ClientID%>");
                alert(hdn2.value);
                return false;
            }
            else {
                // disable the Ok button to prevent multiple clicks from creating multiple entries
                cmdOK.disabled = true;
                __doPostBack(sender, e);
            }
        }

        function AddNewThreadClickUpdate(sender, e) {
            var txt1 = document.getElementById("<%=txtNewThreadComment.ClientID%>");
            var cmdOK = document.getElementById("<%=cmdNewThreadOk.ClientID%>");


            if (txt1.value == "") {
                var hdn2 = document.getElementById("<%=hdnNoCommentPrompt.ClientID%>");
                alert(hdn2.value);
                return false;
            }
            else {
                // disable the Ok button to prevent multiple clicks from creating multiple entries
                cmdOK.disabled = true;
                __doPostBack(sender, e);
            }
        }


    </script>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div style="padding: 10px">
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
<ig:WebDataGrid ID="wdgApplicationThreads" runat="server" AltItemCssClass="VerifyGrid_Report_AlternateRow" AutoGenerateColumns="False" 
                                BackColor="White" EnableTheming="True" Font-Names="Arial" FooterCaptionCssClass="VerifyGrid_Report_Footer" 
                                HeaderCaptionCssClass="VerifyGrid_Report_HeaderGold_10pt" Height="230px" ItemCssClass="VerifyGrid_Report_Row" EnableAjax="False">
                                <Columns>
                                    <ig:BoundDataField DataFieldName="Field0" Key="Id" Width="50px" Hidden="True">
                                        <Header Text="Id:" />
                                    </ig:BoundDataField>
                                    <ig:BoundDataField DataFieldName="Field1" Key="Comment" Width="450px">
                                        <Header Text="Thread:" />
                                    </ig:BoundDataField>
                                    <ig:BoundDataField DataFieldName="Field2" DataType="System.DateTime" Key="DateCreated" Width="120px" DataFormatString="{0:d}">
                                        <Header Text="Date Created:" />
                                    </ig:BoundDataField>
                                    <ig:BoundDataField DataFieldName="Field3" DataType="System.String" Key="CreatedBy" Width="150px">
                                        <Header Text="Created By:" />
                                    </ig:BoundDataField>
                                    <ig:BoundDataField DataFieldName="Field6"  Key="hdnPage" Width="80px" Hidden="True">
                                    </ig:BoundDataField>
                                    <ig:BoundDataField DataFieldName="Field7"  Key="hdnControl" Width="80px" Hidden="True">
                                    </ig:BoundDataField>
                                    <ig:BoundCheckBoxField DataFieldName="Field8" Key="FieldLinked" Width="50px" CssClass="ColumnCenter" DataType="System.Boolean">
                                        <Header Text="Field Linked?">
                                        </Header>
                                    </ig:BoundCheckBoxField>
                                    <ig:BoundDataField DataFieldName="Field9" Key="NumComments" Width="50px">
                                        <Header Text="Items in Thread:" />
                                    </ig:BoundDataField>

                                </Columns>
                                <behaviors>
                                    <ig:Activation ActiveRowCssClass="SelectedRow" ActiveCellCssClass="VerifyGrid_Report_Row">
                                    </ig:Activation>
                                    <ig:ColumnResizing>
                                    </ig:ColumnResizing>
                                    <ig:Selection CellClickAction="Row" RowSelectType="Single">
                                        <AutoPostBackFlags RowSelectionChanged="True" />
                                    </ig:Selection>
                                    <ig:Sorting AscendingImageUrl="~/App_Themes/Buttons/Up_Arrow_small.gif" DescendingImageUrl="~/App_Themes/Buttons/Down_Arrow_small.gif" Enabled="True">
                                    </ig:Sorting>
                                    <ig:Filtering>
                                    </ig:Filtering>
                                </behaviors>
                            </ig:WebDataGrid>                        </td>
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
<ig:WebDataGrid ID="wdgApplicationComments" runat="server" AltItemCssClass="VerifyGrid_Report_AlternateRow" AutoGenerateColumns="False" BackColor="White" CssClass="VerifyGrid_Report_Frame" EnableTheming="True" Font-Names="Arial" FooterCaptionCssClass="VerifyGrid_Report_Footer" HeaderCaptionCssClass="VerifyGrid_Report_HeaderGold_10pt" Height="230px" ItemCssClass="VerifyGrid_Report_Row" EnableAjax="False">
                                <Columns>
                                    <ig:BoundDataField DataFieldName="Field0" Hidden="True" Key="Id" Width="50px">
                                        <Header Text="Id:" />
                                    </ig:BoundDataField>
                                    <ig:BoundDataField DataFieldName="Field4" Key="DateCleared" Width="70px" DataFormatString="{0:G}" >
                                        <Header Text="Date Viewed:" />
                                    </ig:BoundDataField>
                                    <ig:BoundDataField DataFieldName="Field5" DataType="System.String" Key="ClearedBy" Width="80px">
                                        <Header Text="Viewed By:" />
                                    </ig:BoundDataField>
                                    <ig:BoundDataField DataFieldName="Field9"  Key="Target" Width="80px">
                                        <Header Text="Target:" />
                                    </ig:BoundDataField>
                                    <ig:BoundDataField DataFieldName="Field1"  Key="Comment" Width="350px">
                                        <Header Text="Comment:" />
                                    </ig:BoundDataField>
                                    <ig:BoundDataField DataFieldName="Field3"  Key="CreatedBy" Width="90px">
                                        <Header Text="Created By:" />
                                    </ig:BoundDataField>
                                    <ig:BoundDataField DataFieldName="Field2" DataFormatString="{0:G}" DataType="System.DateTime" Key="DateCreated" Width="120px">
                                        <Header Text="Date Created:" />
                                    </ig:BoundDataField>

                                    <ig:BoundDataField DataFieldName="Field6" Key="hdnPage" Hidden="True" >
                                    </ig:BoundDataField>
                                    <ig:BoundDataField DataFieldName="Field7" Key="hdnControl" Hidden="True" >
                                    </ig:BoundDataField>
                                    <ig:BoundDataField DataFieldName="Field10" Key="SourceId" Hidden="True" >
                                    </ig:BoundDataField>
                                </Columns>
                                <behaviors>
                                    <ig:Activation ActiveRowCssClass="SelectedRow">
                                    </ig:Activation>
                                    <ig:ColumnResizing>
                                    </ig:ColumnResizing>
                                    <ig:Selection CellClickAction="Row" RowSelectType="Single">
                                        <AutoPostBackFlags RowSelectionChanged="True" />
                                    </ig:Selection>
                                    <ig:Sorting AscendingImageUrl="~/App_Themes/Buttons/Up_Arrow_small.gif" DescendingImageUrl="~/App_Themes/Buttons/Down_Arrow_small.gif" Enabled="True">
                                    </ig:Sorting>
                                    <ig:Filtering>
                                    </ig:Filtering>
                                </behaviors>
                            </ig:WebDataGrid>                        </td>
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

    
            <asp:Panel ID="pnlNewComment"  runat="server" BackColor="White" style="display:"
                meta:resourcekey="pnlCommentResource1" Width="750px">
                <div style="padding: 20px">
                    <center>
            <table style="width: 100%">
                <tr>
                    <td>
                        <asp:Label ID="lblNewCommentCaption" runat="server" Font-Bold="True" 
                            Font-Names="Arial" Font-Size="10pt" 
                            Text="Use this panel to add a Comment to the selected Thread."></asp:Label>
                    </td>
                </tr>
            </table>

                        <table style="border: 1px solid #808080; width: 698px; font-family: Arial; font-size: 10pt; vertical-align: middle; text-align: center;">
                            <tr>
                                <td style="height: 26px; text-align: left;" colspan="2">
                                    <asp:Label ID="Label1" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="10pt" Text="Who is the Target for this Comment ?"></asp:Label>
                                </td>
                                <td style="width: 101px; height: 26px;">
                                    &nbsp;</td>
                                <td style="height: 26px; text-align: left;">
                                    &nbsp;</td>
                            </tr>
                            <tr>
                                <td style="width: 144px; height: 26px;">
                                    <asp:Label ID="Label3" runat="server" Font-Names="Arial" Font-Size="10pt" Text="Category"></asp:Label>
                                </td>
                                <td style="width: 110px; height: 26px;">
                                    <asp:DropDownList ID="ddlCommentTargetCategory" runat="server" Width="191px">
                                    </asp:DropDownList>
                                    <cc1:CascadingDropDown ID="CascadingDropDown1" runat="server" category="Category" loadingtext="Loading Categories ..." prompttext="Select Category .." servicemethod="GetPersonnelCategories" servicepath="~/ModuleServices.asmx" targetcontrolid="ddlCommentTargetCategory">
                                    </cc1:CascadingDropDown>
                                </td>
                                <td style="width: 101px; height: 26px;">
                                    <asp:Label ID="Label5" runat="server" Font-Names="Arial" Font-Size="10pt" Text="User"></asp:Label>
                                </td>
                                <td style="height: 26px; text-align: left;">
                                    <asp:DropDownList ID="ddlCommentTarget" runat="server" Width="191px">
                                    </asp:DropDownList>
                                    <cc1:CascadingDropDown ID="CascadingDropDown2" runat="server" category="Users" loadingtext="Loading Users ..." parentcontrolid="ddlCommentTargetCategory" prompttext="Select User ..." servicemethod="GetPersonnelInCategory" servicepath="~/ModuleServices.asmx" targetcontrolid="ddlCommentTarget">
                                    </cc1:CascadingDropDown>
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
                                        ImageUrl="~/App_Themes/Buttons/Cancel.gif" 
                                        meta:resourcekey="cmdNewCancelResource1" />
                                </td>
                                <td>
                                    <asp:ImageButton ID="cmdNewOk" runat="server" 
                                        ImageUrl="~/App_Themes/Buttons/Ok.gif" 
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
                OnCancelScript="HideModalPopup()" PopupControlID="pnlNewComment" 
                TargetControlID="pnlNewComment">
            </cc1:ModalPopupExtender>
 
 
             <asp:Panel ID="pnlNewThread"  runat="server" BackColor="White" 
                meta:resourcekey="pnlNewThreadResource1" Width="750px" style="display:">
                <div style="padding: 20px">
                    <center>
            <table style="width: 100%">
                <tr>
                    <td>
                        <asp:Label ID="lblNewThreadCaption" runat="server" Font-Bold="True" 
                            Font-Names="Arial" Font-Size="10pt" 
                            Text="Use this panel to create a new Thread."></asp:Label>
                    </td>
                </tr>
            </table>

                        <table style="border: 1px solid #808080; width: 698px; font-family: Arial; font-size: 10pt; vertical-align: middle; text-align: center;">
                            <tr>
                                <td style="height: 26px; text-align: left;" colspan="2">
                                    <asp:Label ID="lblCommentTarget" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="10pt" Text="Who is the Target for this Comment ?"></asp:Label>
                                </td>
                                <td style="width: 101px; height: 26px;">
                                    &nbsp;</td>
                                <td style="height: 26px; text-align: left;">
                                    &nbsp;</td>
                            </tr>
                            <tr>
                                <td style="width: 144px; height: 26px;">
                                    <asp:Label ID="Label2" runat="server" Font-Names="Arial" Font-Size="10pt" Text="Category"></asp:Label>
                                </td>
                                <td style="width: 110px; height: 26px;">
                                    <asp:DropDownList ID="ddlThreadTargetCategory" runat="server" Width="191px">
                                    </asp:DropDownList>
                                    <cc1:CascadingDropDown ID="CascadingDropDown3" runat="server" category="Category" loadingtext="Loading Categories ..." prompttext="Select Category .." servicemethod="GetPersonnelCategories" servicepath="~/ModuleServices.asmx" targetcontrolid="ddlThreadTargetCategory">
                                    </cc1:CascadingDropDown>
                                </td>
                                <td style="width: 101px; height: 26px;">
                                    <asp:Label ID="Label6" runat="server" Font-Names="Arial" Font-Size="10pt" Text="User"></asp:Label>
                                </td>
                                <td style="height: 26px; text-align: left;">
                                    <asp:DropDownList ID="ddlThreadTarget" runat="server" Width="191px">
                                    </asp:DropDownList>
                                    <cc1:CascadingDropDown ID="CascadingDropDown4" runat="server" category="Users" loadingtext="Loading Users ..." parentcontrolid="ddlThreadTargetCategory" prompttext="Select User ..." servicemethod="GetPersonnelInCategory" servicepath="~/ModuleServices.asmx" targetcontrolid="ddlThreadTarget">
                                    </cc1:CascadingDropDown>
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
                                        ImageUrl="~/App_Themes/Buttons/Cancel.gif" 
                                        meta:resourcekey="cmdNewThreadCancelResource1" />
                                </td>
                                <td>
                                    <asp:ImageButton ID="cmdNewThreadOk" runat="server" 
                                        ImageUrl="~/App_Themes/Buttons/Ok.gif" 
                                        meta:resourcekey="cmdNewThreadOkResource1" />
                                </td>
                            </tr>
                        </table>
                        <br />
                    </center>
                </div>
            </asp:Panel>
            <cc1:ModalPopupExtender ID="ModalPopupExtender2" runat="server" 
                BackgroundCssClass="modalBackground" CancelControlID="cmdNewThreadCancel" 
                DropShadow="True" DynamicServicePath="" Enabled="True" 
                OnCancelScript="HideModalPopup2()" PopupControlID="pnlNewThread" 
                TargetControlID="pnlNewThread">
            </cc1:ModalPopupExtender>
           
         <br />
           
    <asp:HiddenField ID="hdnNoThreadSelected" runat="server" />
    <asp:HiddenField ID="hdnNoPageForComment" runat="server" />
    <asp:HiddenField ID="hdnNoCommentPrompt" runat="server" />
    <asp:HiddenField ID="hdnNoCommentSelected" runat="server" />
    

</body>

</asp:Content>
