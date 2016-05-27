<%@ Page Language="VB" MasterPageFile= "~/TabPages/ProjectMain.master"  AutoEventWireup="false" CodeFile="ErrorPage.aspx.vb" Inherits="ErrorPage"  %>


<asp:Content ID="Content1" ContentPlaceHolderID="ProjectMAIN_content"  Runat="Server"> 

<span style="font-size: 12pt; color: #cc0000">Error Has Occured Loading Page!<br />
    </span>
    <br />
    <asp:label id="lblError" runat="server" text="Error Here" Font-Names="Arial" Font-Size="10pt" Height="76px" Width="750px"></asp:label>



</asp:Content>

