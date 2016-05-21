<%@ Page Title="" Language="VB" AutoEventWireup="false" CodeFile="AssignToTruck.aspx.vb" EnableEventValidation="false" Inherits="TabPages_AssignToTruck" %>


<%@ Register assembly="Infragistics4.Web.v13.2, Version=13.2.20132.2028, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" namespace="Infragistics.Web.UI.DisplayControls" tagprefix="ig" %>

<%@ Register Assembly="Infragistics4.Web.v13.2, Version=13.2.20132.2028, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" namespace="Infragistics.Web.UI.EditorControls" tagprefix="ig" %>
<%@ Register Assembly="Infragistics4.Web.v13.2, Version=13.2.20132.2028, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" namespace="Infragistics.Web.UI.GridControls" tagprefix="ig" %>
<%@ Register Assembly="Infragistics4.Web.v13.2, Version=13.2.20132.2028, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" namespace="Infragistics.Web.UI.ListControls" tagprefix="ig" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
   <script type="text/javascript" src="<%= Session("_VT_JQueryFileLocation")%>Scripts/jquery-1.10.1.min.js"></script>


<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="../jg_styles.css" rel="stylesheet" type="text/css" />
    <link href="../Verify_Infragistics.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/VT_Responsive_V1.css" rel="stylesheet" />


  
    <style type="text/css">


    </style>
</head>
<body>
     <script type="text/javascript" src="..\scripts\json2.js">    </script>
    <script type="text/javascript" src="..\Scripts\jquery-1.10.2.js"></script>
<script type="text/javascript" src="..\Scripts\jquery.tmpl.js"></script>
    <script type="text/javascript"></script>

    <form id="form1" runat="server">
          <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true" AsyncPostBackTimeout="360000">
                <Scripts>
                    <asp:ScriptReference Path="../Scripts/CommonScripts.js" />
                    <asp:ScriptReference Path="../Scripts/VT_Infragistics.js" />
                </Scripts>
            </asp:ScriptManager>
    <div>
    
                                <asp:ImageButton ID="btnBack1" runat="server" AlternateText="Back" ImageUrl="~/App_Themes/Billing/Buttons/Back.gif" />
        <br />
        <br />
                               
                                            <asp:Label ID="Label5" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="14pt" ForeColor="Black" Text="Assign this order to a truck : "></asp:Label>
                                        <br />
        <br />
        <asp:Label ID="Label9" runat="server" Font-Size="10pt" Text="Selected Order:"></asp:Label>&nbsp;<asp:Label ID="lblSelectedOrder" runat="server" Font-Bold="True"></asp:Label>
        <br />
        <br />
      
                              <table> <tr><td><asp:Label ID="Label10" runat="server" Font-Size="10pt" Text="Select Truck:"></asp:Label></td>
                                  <td><asp:DropDownList ID="ddlTrucks" runat="server" Width="256px" Height="20px">
                                </asp:DropDownList></td>
                                  <td><asp:Label ID="Label1" runat="server" Font-Size="10pt" Text="Add Drivers:"></asp:Label></td><td><asp:DropDownList ID="ddlDrivers" runat="server" Height="20px" Width="215px">
                                </asp:DropDownList><span style="font-size: 11pt">
                                <asp:Button ID="cmdAdd" runat="server" Text="Add"/>
                                </span></td>

                                      </tr>

                                  
                                  <tr><td></td><td></td><td>
                                      <asp:Button ID="btnClear" runat="server" Text="Clear Drivers" />
                                      </td><td>
                                <ig:WebDataGrid ID="wdgDrivers" runat="server" AltItemCssClass="VerifyGrid_Report_AlternateRow" AutoGenerateColumns="False" BackColor="#C1C1A2" EnableAjax="False" EnableTheming="True" Font-Names="Arial" FooterCaptionCssClass="VerifyGrid_Report_Footer" HeaderCaptionCssClass="VerifyGrid_Report_HeaderFormMaster" Height="132px" 
                                    ItemCssClass="VerifyGrid_Report_Row" DataKeyFields="DriverID"
                                EnableDataViewState="True" EnableViewState="True"  >

                                    <Columns>
                                        <ig:BoundDataField DataFieldName="Driver" Key="Driver" width="256px">
                                            <Header Text="Driver" />
                                        </ig:BoundDataField>
                                        <ig:BoundDataField DataFieldName="DriverID" Hidden="True" Key="DriverID">
                                        </ig:BoundDataField>
                                    </Columns>
                                    <Behaviors>
                                        <ig:Activation ActiveRowCssClass="SelectedRow">
                                        </ig:Activation>
                                        <ig:Selection CellClickAction="Row" RowSelectType="Single">
                                            <AutoPostBackFlags RowSelectionChanged="True" />
                                        </ig:Selection>
                                        <ig:EditingCore>
                                            <Behaviors>
                                                <ig:RowAdding>
                                                </ig:RowAdding>
                                            </Behaviors>
                                        </ig:EditingCore>
                                    </Behaviors>
                                </ig:WebDataGrid>
                                      </td></tr>
                              </table> 
            
             
                                
                                <br />
        <br />

                                
                                
                                <asp:Button ID="btnSaveAndClose" runat="server" CssClass="VT_ActionButton" Text="Save and Close" Width="120px" />
                                    
                                
                                
        <br />
    
    </div>
    </form>
</body>
</html>
