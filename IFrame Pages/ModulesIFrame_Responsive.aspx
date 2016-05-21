<%@ Page Language="VB" MasterPageFile= "~/TabPages/ProjectMain_Responsive.master"  AutoEventWireup="false" CodeFile="ModulesIFrame_Responsive.aspx.vb" Inherits="ModulesIFrame_Responsive"  %>


<asp:Content ID="Content2" ContentPlaceHolderID="ProjectMAIN_content" runat="server">

     <script type="text/javascript" src="<%= Session("_VT_JQueryFileLocation")%>Scripts/jquery-1.11.1.min.js"></script>

<script type="text/javascript">



    $(document).ready(function () {

        //SmcN 27/05/2014 Insterted Javscript here to read the width of the Browser window
        var screenwidth = screen.width;
        var hdnBrowserWidth = document.getElementById('<%=hdnBrowserWidth.ClientID%>');
        hdnBrowserWidth.value = screenwidth;


    });

</script>
      

<iframe id = "ModulesFrame"  runat="server"  src= "http://server2.verifytechnologies.com/SupplierModule/?did=vtp-tki"    style="width: 100%; height:4000px">
    <asp:HiddenField ID="hdnBrowserWidth" runat="server" />

</iframe>


</asp:Content>

