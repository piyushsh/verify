<%@ Page Title="" Language="VB" MasterPageFile="~/Mobile/mDetailsMaster.master" AutoEventWireup="false" CodeFile="mSummaryPayment.aspx.vb" Inherits="Mobile_mSummaryPayment" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mProjectMAIN_content" Runat="Server">
     <meta name="viewport" content="width=device-width">
       <div style="text-align: left">

    <asp:Label ID="Label12" runat="server" font-bold="True" font-names="Arial" 
                                    font-size="10pt" forecolor="#B38700" text="Payment summary" 
                                    width="129px" Height="16px"></asp:Label>
                                            <br />
              <table><tr><td>    <asp:Label ID="lblCust0" runat="server" Font-Bold="True" 
                                                Font-Names="Arial" Font-Size="9pt">Order num:</asp:Label></td><td class="auto-style1"> 
                                            <asp:Label ID="lblOrderNum" runat="server" Font-Bold="False" font-names="Arial" 
                                                font-size="9pt" width="147px" Height="16px"></asp:Label>
                                        </td></tr>
                  <tr><td> <asp:Label ID="Label1" runat="server" Font-Bold="True" 
                                                Font-Names="Arial" Font-Size="9pt">Customer:</asp:Label></td><td> <asp:Label ID="lblCust" runat="server" Font-Bold="False" font-names="Arial" 
                                                font-size="9pt" width="147px" Height="16px"></asp:Label></td></tr>
                  <tr><td> <asp:Label ID="Label13" runat="server" Font-Bold="True" 
                                                Font-Names="Arial" Font-Size="9pt">Total this order:</asp:Label></td><td> <asp:Label ID="lblTotalThisOrder" runat="server" Font-Bold="True" font-names="Arial" 
                                                font-size="9pt" width="147px" Height="16px"></asp:Label></td></tr>
                   <tr><td> <asp:Label ID="Label14" runat="server" Font-Bold="True" 
                                                Font-Names="Arial" Font-Size="9pt">Total due for cust:</asp:Label></td><td> <asp:Label ID="lblTotalDue" runat="server" Font-Bold="False" font-names="Arial" 
                                                font-size="9pt" width="147px" Height="16px"></asp:Label></td></tr>
                   <tr><td> <asp:Label ID="Label15" runat="server" Font-Bold="True" 
                                                Font-Names="Arial" Font-Size="9pt">Paid in cash:</asp:Label></td><td>
                <asp:TextBox ID="txtCash" runat="server" Font-Size="9pt" Height="22px" Width="150px"></asp:TextBox>
                       </td></tr>
              </table> 
           <asp:CheckBox ID="chkIncludePrices" runat="server" Checked="True" Font-Size="9pt" Text="Include prices on docket" Visible="False" />
           <br />
           <asp:Button ID="btnComplete" runat="server" CssClass="VT_ActionButton" Height="20px" Text="Complete Order" Font-Size="9pt" /> 
           <br />
           </div> 

 </asp:Content>

