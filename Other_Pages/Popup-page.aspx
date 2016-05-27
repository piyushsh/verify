<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Popup-page.aspx.vb" Inherits="Popup_page" %>

<%@ Register src="~/ProductSelect.ascx" tagname="ProductSelect" tagprefix="uc1" %>



<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
   
    <uc1:ProductSelect ID="ProductSelect1" runat="server" />
    </div>
    </form>
</body>
</html>
