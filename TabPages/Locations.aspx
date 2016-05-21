<%@ Page Language="VB" MasterPageFile="~/TabPages/ProjectMain.master"  AutoEventWireup="false" CodeFile="Locations.aspx.vb" Inherits="LocationsPage"  %>

<%@ MasterType virtualpath="~/TabPages/ProjectMain.master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>


<%@ Register Assembly="Infragistics4.Web.v13.2, Version=13.2.20132.2028, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" namespace="Infragistics.Web.UI.DisplayControls" tagprefix="ig" %>
<%@ Register Assembly="Infragistics4.Web.v13.2, Version=13.2.20132.2028, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" namespace="Infragistics.Web.UI.EditorControls" tagprefix="ig" %>
<%@ Register Assembly="Infragistics4.Web.v13.2, Version=13.2.20132.2028, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" namespace="Infragistics.Web.UI.GridControls" tagprefix="ig" %>
<%@ Register Assembly="Infragistics4.Web.v13.2, Version=13.2.20132.2028, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" namespace="Infragistics.Web.UI.ListControls" tagprefix="ig" %>


<asp:Content ID="Content2" ContentPlaceHolderID="ProjectMAIN_content" runat="server">

<script type="text/javascript" id="Loc">
    function HidePopup() {
        var modal = $find("<%=ModalPopup.BehaviorID%>");
        modal.hide();
    }

    function LocationClose(sender, e) {
        var txtLoc = document.getElementById("<%=txtLocationText.ClientID%>");
        if (txtLoc.value == '') {
            alert('You must enter a Location!');
            return false;
        }

        __doPostBack(sender, e);
    }

    function ConfirmDeleteLocal() {
        var grid = $find("<%= wdgLocations.ClientID%>");
        var myrow = grid.get_behaviors().get_selection().get_selectedRows(0);
        if (myrow == null) {
            alert('You must select a Location to delete!');
            return false;
        } else {

        }

    }

    function LoadPanel(AddOrEdit) {
        document.getElementById('<%=hdnAddOrEdit.ClientID%>').value = AddOrEdit;
        var txtLoc = document.getElementById("<%=txtLocationText.ClientID%>");
        if (AddOrEdit == 'Add') {
            txtLoc.value = '';
        }
        else {
            
            var grid = $find("<%= wdgLocations.ClientID%>");
            var myrow = grid.get_behaviors().get_selection().get_selectedRows(0);
            if (myrow == null) {
                alert('You must select a Location to edit!');
                return false;
            }
            
             //txtLoc.value = myrow.getItem(0).get_cellByColumnKey("LocationText").get_text()
        }

        var modal = $find("<%=ModalPopup.BehaviorID%>");
        modal.show();
        return false;
    }

</script>

<div style="margin-left: 10px;">
    &nbsp;<asp:label id="Label6" runat="server" font-bold="True" font-names="Arial" font-size="11pt"
            forecolor="#B38700" text="Location Configuration" width="258px"></asp:label>
        <br />
    <br />
    
    <table style="border: thin solid #B38700; font-size: 10pt; width: 940px; font-family: Arial">

        <tr>
            <td style="vertical-align: top; width: 750px; position: static; text-align: center" 
                colspan="3">
                        
                        <br />
                        

                   <ig:WebDataGrid ID="wdgLocations" runat="server" AltItemCssClass="VerifyGrid_Report_AlternateRow" 
                       AutoGenerateColumns="False" BackColor="White" CssClass="VerifyGrid_Report_Frame" 
                       EnableTheming="True" Font-Names="Arial" FooterCaptionCssClass="VerifyGrid_Report_Footer"
                       HeaderCaptionCssClass="VerifyGrid_Report_HeaderGreen_10pt" Height="300px" 
                       ItemCssClass="VerifyGrid_Report_Row" EnableAjax="False">
                    <Columns>
                     

                        <ig:bounddatafield DataFieldName="LocationId" Key="LocationId" Width="100px">
                            <header Text="LocationId:"/>
                        </ig:bounddatafield>


                        
                                <ig:bounddatafield DataFieldName="LocationText" Key="LocationText" Width="300px">
                                    <Header Text="Location Name:"/>
                                </ig:bounddatafield>
                                

                     

                        <ig:bounddatafield DataFieldName="LocationPosition" Key="LocationPosition" Width="130px" CssClass="RAlign">
                            <Header Text="Position:"/>
                        </ig:bounddatafield>
                       
					</Columns>
                    <Behaviors>
                        <ig:activation ActiveRowCssClass="SelectedRow">
                           
                            <AutoPostBackFlags ActiveCellChanged="True" />
                           
                        </ig:activation>
                        <ig:columnresizing>
                        </ig:columnresizing>
                     
                        <ig:filtering Enabled="true" FilterButtonCssClass="Filter_LAlign">
                            <ColumnSettings>
                                <ig:columnfilteringsetting ColumnKey="LocationText" Enabled="true" />
                            </ColumnSettings>
                            <ColumnFilters>
                                <ig:columnfilter ColumnKey="LocationText">
                                    <ConditionWrapper>
                                        <ig:ruletextnode />
                                    </ConditionWrapper>
                                </ig:columnfilter>
                            </ColumnFilters>
                            <EditModeActions EnableOnKeyPress="True" />
                        </ig:filtering>
                        <ig:sorting AscendingImageUrl="~/App_Themes/Buttons/Up_Arrow_small.gif" DescendingImageUrl="~/App_Themes/Buttons/Down_Arrow_small.gif" Enabled="True">
                        </ig:sorting>
                        <ig:Selection CellClickAction="Row" RowSelectType="Single">
                        </ig:Selection>
                    </Behaviors>
                       
                </ig:WebDataGrid>

                    

            </td>
        </tr>

        <tr>
            <td style="vertical-align: top; width: 750px; position: static; " 
                class="style2">
                <asp:ImageButton ID="btnAdd" runat="server" AlternateText="Add" 
                    ImageUrl="~/App_Themes/Billing/Buttons/Add-To-This-List.gif" />
            </td>
            <td style="vertical-align: top; width: 750px; position: static; " 
                class="style2">
                <asp:ImageButton ID="btnEdit" runat="server" AlternateText="Edit" 
                    ImageUrl="~/App_Themes/Billing/Buttons/Edit-Selected-Item.gif" />
            </td>
            <td style="vertical-align: top; width: 750px; position: static; " 
                class="style2">
                <asp:ImageButton ID="btnDelete" runat="server" AlternateText="Delete" 
                    ImageUrl="~/App_Themes/Billing/Buttons/Delete-Selected-Item.gif" />
            </td>
        </tr>

        </table>
    &nbsp;<asp:HiddenField ID="hdnAddOrEdit" runat="server" />
    <br />


</div>
    &nbsp;

                    <asp:Panel ID="pnlLocation" runat="server" backcolor="White" 
            borderstyle="Solid" style="display:none "
                    borderwidth="1px" Height="239px" width="900px">
                    <br />
                    <center>
                        <span style="FONT-FAMILY: Arial">
                        <asp:Label ID="lbl1" runat="server" Font-Bold="True" Font-Names="Arial">Location Details</asp:Label>
                        </span>&nbsp;<br /> <br />
                        <table class="style3">
                            <tr>
                                <td>
                                    <asp:Label ID="lblLocText" runat="server" Font-Bold="True" Font-Names="Arial" 
                                        Text="Location Text:"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtLocationText" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label1" runat="server" Font-Bold="True" Font-Names="Arial" 
                                        Text="Location Position:"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtPosition" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                        <br />
                        <br />
                    </center>
                    <center>
                        <br />
                        <asp:ImageButton ID="cmdOk" runat="server" 
                            ImageUrl="~/App_Themes/Billing/Buttons/ok.gif" />
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:ImageButton 
                            ID="cmdCancel" runat="server" AlternateText="Cancel" 
                            ImageUrl="~/App_Themes/Billing/Buttons/Cancel.gif" />
                    </center>
                </asp:Panel>
                
                
                <ajaxtoolkit:modalpopupextender ID="ModalPopup" runat="server" 
                    BackgroundCssClass="modalBackground" CancelControlID="cmdCancel" 
                    DropShadow="True" oncancelscript="HidePopup()" PopupControlID="pnlLocation" 
                    TargetControlID="pnlLocation">
                </ajaxtoolkit:modalpopupextender>


</asp:Content>



