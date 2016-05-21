<%@ Page Language="VB" MasterPageFile="~/TabPages/ProjectMain.master" AutoEventWireup="false" CodeFile="AddProduct.aspx.vb" Inherits="Other_Pages_AddProduct" %>

<%@ Register tagprefix="igtbl" namespace="Infragistics.WebUI.UltraWebGrid" Assembly="Infragistics2.WebUI.UltraWebGrid.v7.3, Version=7.3.20073.38, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>

<%@ Register src="../ProductSelect.ascx" tagname="ProductSelect" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ProjectMAIN_content" Runat="Server">
                  
                <table style="width: 940px">
     
        <tr>
            <td style="width: 400px; height: 26px;">
                        &nbsp;</td>
           
           
            <td style="height: 26px; text-align: right;">
                        <asp:ImageButton ID="btnBack" runat="server" ImageUrl="~/APP_THEMES/Billing/Buttons/Back-Button.gif" />
                </td>
        </tr>
     
        <tr>
            <td style="width: 400px; height: 26px;">
                        <asp:Label ID="lblPageTitle" runat="server" Font-Bold="True" 
                            style="font-size: 14pt; font-family: Arial">Select Product to Add to Order</asp:Label>
                    </td>
           
           
            <td style="height: 26px">
                </td>
        </tr>
        </table>
                  
                   <table style="width: 950px">
                       <tr>
                           <td>
                               <uc1:ProductSelect ID="ProductSelect1" runat="server" />
                           </td>
                       </tr>
                       <tr>
                           <td>
                               <table style="width: 900px">
                                   <tr>
                                       <td>
                                           &nbsp;</td>
                                       <td>
                                           &nbsp;</td>
                                       <td style="width: 173px">
                                           <asp:ImageButton ID="btnAdd" runat="server" 
                                               ImageUrl="~/App_Themes/Billing2/Buttons/Add-Product.gif" />
                                       </td>
                                       <td>
                                  <asp:Label ID="lblMsg" runat="server" Font-Bold="True" Font-Names="Arial" 
                                      Font-Size="12pt" ForeColor="Red"></asp:Label>
                                       </td>
                                   </tr>
                                
                               </table>
                               
                           </td>
                       </tr>
                         <tr>
                          <td>
         &nbsp;
                              <br />
                              <asp:Panel ID="Panel1" runat="server" Height="145px" style="text-align: center">
                                  <br />
                                  <br />
                                  <br />
                                  <br />
                                  <asp:ImageButton ID="bnCancel" runat="server" 
                                      ImageUrl="~/App_Themes/Billing/Buttons/OK.gif" />
                              </asp:Panel>
             
    
    
                           

   
                              <br />

         </td>
       </tr>
                                              </table>
        
                </asp:Content>

