<%@ Page Language="VB" MasterPageFile="~/TabPages/ProjectMain.master"  AutoEventWireup="false" CodeFile="SystemAdmin_Opening.aspx.vb" Inherits="SystemAdmin_Opening"  %>


<asp:Content ID="Content2" ContentPlaceHolderID="ProjectMAIN_content" runat="server">

 

<div style="margin-left: 10px;">
    <br />
    <table class="style2">
        <tr>
            <td>
                <asp:Label ID="Label6" runat="server" Font-Bold="True" Font-Names="Arial" 
                    Font-Size="11pt" ForeColor="Black" Text="Customer Items"></asp:Label>
            </td>
        </tr>
        <tr>
            <td style="border: 1px solid #CC9900">
                <table class="style3" 
                    style="font-family: Arial; font-size: 10pt; color: #000000; text-align: left; vertical-align: top;">
                    <tr>
                        <td style="width: 260px">
                <asp:ImageButton ID="btnCustomers" runat="server" 
                    ImageUrl="~/App_Themes/Billing/Buttons/Manage-Customers.GIF" 
                    AlternateText="Manage Customers" />
                        </td>
                        <td style="vertical-align: top; text-align: left">
                <asp:Label ID="lblConfig" runat="server" Text="Use this to manage your Customer database. Including customer details, customer contracts, contact points, etc.,"
                    Width="529px"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 260px">
                            &nbsp;</td>
                        <td style="vertical-align: top; text-align: left">
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td style="width: 260px">
                <asp:ImageButton ID="btnPriceLists" runat="server" 
                    AlternateText="Invoice Check" 
                    ImageUrl="~/App_Themes/Billing/Buttons/Manage-PriceLists.gif" />
                        </td>
                        <td style="vertical-align: top; text-align: left">
                <asp:Label ID="Label3" runat="server" Text="Use this to configure your PriceLists and set prices for particular Customers and Products"
                    Width="670px"></asp:Label></td>
                    </tr>
                    <tr>
                        <td style="width: 260px">
                            &nbsp;</td>
                        <td style="vertical-align: top; text-align: left">
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td style="width: 260px">
                <asp:ImageButton ID="btnContracts" runat="server" 
                    AlternateText="Invoice Check" 
                    ImageUrl="~/App_Themes/Buttons/Manage-Contracts.gif" />
                        </td>
                        <td style="vertical-align: top; text-align: left">
                <asp:Label ID="Label4" runat="server" Text="Use this to manage the overall Contacts database. This includes Supplier contracts, Customer Contracts and other contracts"
                    Width="670px"></asp:Label></td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;
                &nbsp;</td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="Label7" runat="server" Font-Bold="True" Font-Names="Arial" 
                    Font-Size="11pt" ForeColor="Black" Text="Personnel Items"></asp:Label>
            </td>
        </tr>
        <tr>
            <td style="border: 1px solid #CC9900">
                <table class="style3" 
                    style="font-family: Arial; font-size: 10pt; color: #000000; text-align: left; vertical-align: top;">
                    <tr>
                        <td style="width: 260px">
                <asp:ImageButton ID="btnPersonnelModule" runat="server" ImageUrl="~/App_Themes/Billing/Buttons/Manage-Personnel.gif" AlternateText="Manage Personnel" /></td>
                        <td style="vertical-align: top; text-align: left">
                <asp:Label ID="lblPersonnel" runat="server" 
                    
                    Text="Use this to Manage your Users database. You can Add and Edit user details from here. This also allows you to re-set passwords and other key user details for this system. This also includes setting up user privledges within the system."></asp:Label>
                        </td>
                    </tr>
                    </table>
            </td>
        </tr>
        <tr>
            <td>
                &nbsp; &nbsp;</td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="Label8" runat="server" Font-Bold="True" Font-Names="Arial" 
                    Font-Size="11pt" ForeColor="Black" Text="Product Items"></asp:Label>
            </td>
        </tr>
        <tr>
            <td style="border: 1px solid #CC9900">
                <table class="style3" 
                    style="font-family: Arial; font-size: 10pt; color: #000000; text-align: left; vertical-align: top;">
                    <tr>
                        <td style="width: 260px">
                <asp:ImageButton ID="btnProductsModule" runat="server" 
                    ImageUrl="~/App_Themes/Billing/Buttons/Manage-Products.gif" 
                    AlternateText="Manage Personnel" />
                        </td>
                        <td style="vertical-align: top; text-align: left">
                <asp:Label ID="lblConfig0" runat="server" Text="Use this to manage your Products database. Including products details, Specifications, BOM's, Ingeredients, etc."
                    Width="529px"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 260px">
                            &nbsp;</td>
                        <td style="vertical-align: top; text-align: left">
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td style="width: 260px">
                <asp:ImageButton ID="btnManageLocations" runat="server" 
                    ImageUrl="~/App_Themes/Billing/Buttons/Manage-Locations.gif" 
                    AlternateText="Manage Locations" />
                        </td>
                        <td style="vertical-align: top; text-align: left">
                <asp:Label ID="Label2" runat="server" Text="Use this to manage the Inventory Locations database. You can edit and create new locations from here."
                    Width="670px"></asp:Label></td>
                    </tr>
                    <tr>
                        <td style="width: 260px">
                            &nbsp;</td>
                        <td style="vertical-align: top; text-align: left">
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td style="width: 260px">
                <asp:ImageButton ID="btnInventoryCounts" runat="server" 
                    ImageUrl="~/App_Themes/Buttons/Manage-InventoryCounts.gif" 
                    AlternateText="Manage Locations" />
                        </td>
                        <td style="vertical-align: top; text-align: left">
                <asp:Label ID="Label5" runat="server" Text="Use this to manage the Inventory counts (stock takes). You can create new inventory cycle counts here or review historical inventory counts that have been completed previously."
                    Width="670px"></asp:Label></td>
                    </tr>
                    <tr>
                        <td style="width: 260px">
                            &nbsp; &nbsp;</td>
                        <td style="vertical-align: top; text-align: left">
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td style="width: 260px">
                <asp:ImageButton ID="btnTransactionManager" runat="server" 
                    ImageUrl="~/App_Themes/Buttons/Manage-Transactions.gif" 
                    AlternateText="Manage Locations" />
                        </td>
                        <td style="vertical-align: top; text-align: left">
                <asp:Label ID="Label10" runat="server" Text="Use this to enter basic transactions such as Good Inwards transaction, Inventory count transactions, Adjustment Transactions, etc. "
                    Width="670px"></asp:Label></td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                &nbsp; &nbsp;</td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="Label9" runat="server" Font-Bold="True" Font-Names="Arial" 
                    Font-Size="11pt" ForeColor="Black" Text="Utility Items"></asp:Label>
            </td>
        </tr>
        <tr>
            <td style="border: 1px solid #CC9900">
                <table class="style3" 
                    style="font-family: Arial; font-size: 10pt; color: #000000; text-align: left; vertical-align: top;">
                    <tr>
                        <td style="width: 260px">
                <asp:ImageButton ID="btnTransactions" runat="server" 
                    AlternateText="View Transactions" 
                    ImageUrl="~/App_Themes/Billing/Buttons/View-Transactions.gif" />
                        </td>
                        <td style="vertical-align: top; text-align: left">
                <asp:Label ID="lblTransactions" runat="server" Text="Use this to view the low level transactions from the handheld and through the system (for the selected Sales Order)."
                    Width="604px"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 260px">
                            &nbsp;</td>
                        <td style="vertical-align: top; text-align: left">
                            &nbsp;</td>
                    </tr>
                       <tr>
                        <td style="width: 260px">
                            <asp:Button ID="btnJobSteps" runat="server" Text="Workflow Sequences" CssClass="VT_ActionButton" />
                           </td>
                        <td style="vertical-align: top; text-align: left">
                <asp:Label ID="lblTransactions0" runat="server" Text="Use this to view and edit the job steps in each workflow sequence"
                    Width="604px"></asp:Label>
                           </td>
                    </tr>
                     <tr>
                        <td style="width: 260px">
                            &nbsp;</td>
                        <td style="vertical-align: top; text-align: left">
                            &nbsp;</td>
                    </tr>
                       <tr>
                        <td style="width: 260px">
                            <asp:Button ID="btnRoles" runat="server" Text="Job Roles" CssClass="VT_ActionButton" />
                           </td>
                        <td style="vertical-align: top; text-align: left">
                <asp:Label ID="Label1" runat="server" Text="Use this to access the Roles module"
                    Width="604px"></asp:Label>
                           </td>
                    </tr>

                     <tr>
                        <td style="width: 260px">
                            &nbsp;</td>
                        <td style="vertical-align: top; text-align: left">
                            &nbsp;</td>
                    </tr>
                       <tr>
                        <td style="width: 260px">
                            <asp:Button ID="btnImportOrders" runat="server" Text="Run excel import" CssClass="VT_ActionButton" />
                           </td>
                        <td style="vertical-align: top; text-align: left">
                <asp:Label ID="Label11" runat="server" Text="Use this to import sales orders from excel"
                    Width="604px"></asp:Label>
                           </td>
                    </tr>
                       <tr>
                        <td style="width: 260px">
                            &nbsp;</td>
                        <td style="vertical-align: top; text-align: left">
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td style="width: 260px">
                <asp:ImageButton ID="btnInvoiceCheck" runat="server" 
                    AlternateText="Invoice Check" 
                    ImageUrl="~/App_Themes/Billing/Buttons/View-Transactions.gif" />
                        </td>
                        <td style="vertical-align: top; text-align: left">
                <asp:Label ID="lblSageInvoices" runat="server" Text="Use this to check invoices posted to the accounts system against delivery dockets issued within this system"
                    Width="604px"></asp:Label>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;&nbsp;</td>
        </tr>
        </table>
    <br />


</div>
    &nbsp;

</asp:Content>

<asp:Content ID="Content3" runat="server" contentplaceholderid="head">
    <style type="text/css">
        .style2
        {
            width: 942px;
        }
        .style3
        {
            width: 940px;
        }
    </style>
</asp:Content>


