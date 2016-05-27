<%@ Page Language="VB" MasterPageFile= "~/TabPages/ProjectMain.master"  AutoEventWireup="false" CodeFile="ModulesIFrame.aspx.vb" Inherits="ModulesIFrame"  %>


<asp:Content ID="Content2" ContentPlaceHolderID="ProjectMAIN_content" runat="server">

 
      

    <asp:ImageButton ID="btnback" runat="server" ImageUrl="~/App_Themes/Billing2/Buttons/Back.gif" />

 
      

<iframe id = "ModulesFrame"  runat="server"  src= "http://server2.verifytechnologies.com/SupplierModule/?did=vtp-tki"  width="100%" frameborder="0" height="1540" >

</iframe>


</asp:Content>

