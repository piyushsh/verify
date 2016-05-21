<%@ Page Language="VB" MasterPageFile="~/TabPages/ProjectMain.master" AutoEventWireup="false" CodeFile="ShippingDetails.aspx.vb" Inherits="TabPages_ShippingDetails" %>

<%@ Register Assembly="Infragistics4.Web.v13.2, Version=13.2.20132.2028, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" namespace="Infragistics.Web.UI.DisplayControls" tagprefix="ig" %>
<%@ Register Assembly="Infragistics4.Web.v13.2, Version=13.2.20132.2028, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" namespace="Infragistics.Web.UI.EditorControls" tagprefix="ig" %>
<%@ Register Assembly="Infragistics4.Web.v13.2, Version=13.2.20132.2028, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" namespace="Infragistics.Web.UI.GridControls" tagprefix="ig" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ProjectMAIN_content" runat="server">

 

<div style="margin-left: 10px;">

                                    <asp:Label ID="lblStatusHeader" runat="server" Font-Bold="True" 
                                    Font-Names="Arial" Font-Size="12pt" Text="Shipping details for order : "></asp:Label>

                                        <asp:Label ID="lblOrderNum" 
                                    runat="server" Font-Names="Arial" Font-Size="12pt" 
                                    Font-Bold="True" ForeColor="#B38700"></asp:Label>
                                                     <br />
    <br />

    <table>
        <tr><td>
            <asp:Label ID="lblCont_NS" runat="server" Text="Container ID:" Font-Names="Arial"></asp:Label></td><td>
                <asp:TextBox ID="strContainerId" runat="server"></asp:TextBox>
            </td><td><asp:Label ID="DeliveryDocNum_NS" runat="server" Text="Delivery doc num:" Font-Names="Arial"></asp:Label></td><td>
            <asp:TextBox ID="strDeliveryDocNum" runat="server"></asp:TextBox>
            </td></tr>
        <tr><td>
            <asp:Label ID="InvDate_NS" runat="server" Text="Invoice date:" Font-Names="Arial"></asp:Label></td><td>
                <asp:TextBox ID="strInvDate" runat="server"></asp:TextBox>
            </td><td><asp:Label ID="lblinvNum_NS" runat="server" Text="BOL:" Font-Names="Arial"></asp:Label></td><td>
            <asp:TextBox ID="strInvNum" runat="server"></asp:TextBox>
            </td></tr>
        <tr><td>
            <asp:Label ID="lblCurrentETA_NS" runat="server" Text="Current ETA:" Font-Names="Arial"></asp:Label></td><td>
                <asp:TextBox ID="strCurrentETA" runat="server"></asp:TextBox>
            </td><td><asp:Label ID="lblShipper_NS" runat="server" Text="Shipper:" Font-Names="Arial"></asp:Label></td><td>
            <asp:TextBox ID="strShipper" runat="server"></asp:TextBox>
            </td></tr>
<tr><td>
            <asp:Label ID="lblRegion_NS" runat="server" Text="Region:" Font-Names="Arial"></asp:Label></td><td>
        <asp:TextBox ID="strRegion" runat="server"></asp:TextBox>
    </td><td><asp:Label ID="lblBillDoc_NS" runat="server" Text="Bill doc:" Font-Names="Arial"></asp:Label></td><td>
    <asp:TextBox ID="strBillDoc" runat="server"></asp:TextBox>
    </td></tr>
        <tr><td>
            &nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td></tr>
    </table>
                                    <br />
                                    <ig:WebDataGrid ID="wdgShippingItems" runat="server"
                   DataKeyFields="VT_UniqueIndex"
                  
                   AltItemCssClass="VerifyGrid_Report_AlternateRow" 
    AutoGenerateColumns="False" Font-Names="Arial" 
        ItemCssClass="VerifyGrid_Report_Row" 
        EnableTheming="True" FooterCaptionCssClass="VerifyGrid_Report_Footer" 
        HeaderCaptionCssClass="VerifyGrid_Report_HeaderBeige_10pt" BackColor="White" 
        CssClass="VerifyGrid_Report_Frame" Height="252px"  Visible="True">
                   
 <Columns>
     						
                    <ig:BoundDataField DataFieldName="PfiserRef" Key="PfiserRef" Width="90px"> 
                        <Header Text="PfiserRef:"/>
                    </ig:BoundDataField>
                    <ig:BoundDataField DataFieldName="Destination" Key="Destination" Width="90px"> 
                        <Header Text="Destination:"/>
                    </ig:BoundDataField>
                    <ig:BoundDataField DataFieldName="Carrier" Key="Carrier" Width="90px">
                        <Header Text="Carrier:"/>
                    </ig:BoundDataField>
                    <ig:BoundDataField DataFieldName="CBLNumber" Key="CBLNumber" Width="90px">
                        <Header Text="CBLNumber:"/>
                    </ig:BoundDataField>
                     <ig:BoundDataField DataFieldName="EquipmentNum" Key="EquipmentNum" Width="100px">
                        <Header Text="EquipmentNum:"/>
                    </ig:BoundDataField>
                    <ig:BoundDataField DataFieldName="FirstFeederDepDatePlanned" Key="FirstFeederDepDatePlanned" Width="170px">
                        <Header Text="FirstFeederDepDatePlanned:"/>
                    </ig:BoundDataField>


                    <ig:BoundDataField DataFieldName="FirstFeederDepDateActual" Key="FirstFeederDepDateActual" Width="170px"> 
                        <Header Text="FirstFeederDepDateActual:"/>
                    </ig:BoundDataField>
                    <ig:BoundDataField DataFieldName="LastOceanVessel" Key="LastOceanVessel" Width="110px" >
                        <Header Text="LastOceanVessel:"/>
                    </ig:BoundDataField>
                    <ig:BoundDataField DataFieldName="FirstFeederVessel" Key="FirstFeederVessel" Width="110px">
                        <Header Text="FirstFeederVessel:"/>
                    </ig:BoundDataField>
                     <ig:BoundDataField DataFieldName="LastOceanDepDatePlanned" Key="LastOceanDepDatePlanned" Width="170px">
                        <Header Text="LastOceanDepDatePlanned:"/>
                    </ig:BoundDataField>
                    <ig:BoundDataField DataFieldName="LastOceanDepDateActual" Key="LastOceanDepDateActual" Width="170px">
                        <Header Text="LastOceanDepDateActual:"/>
                    </ig:BoundDataField>
                    <ig:BoundDataField DataFieldName="LastVesselArrDateActual" Key="LastVesselArrDateActual" Width="170px" >
                        <Header Text="LastVesselArrDateActual:"/>
                    </ig:BoundDataField>
			

                    <ig:BoundDataField DataFieldName="LastVesselArrDatePlanned" Key="LastVesselArrDatePlanned" Width="170px" >
                        <Header Text="LastVesselArrDatePlanned:"/>
                    </ig:BoundDataField>

                    <ig:BoundDataField DataFieldName="EquipmentType" Key="EquipmentType" Width="100px" > 
                        <Header Text="EquipmentType:"/>
                    </ig:BoundDataField>
                    <ig:BoundDataField DataFieldName="EquipmentSize" Key="EquipmentSize" Width="100px" > 
                        <Header Text="EquipmentSize:"/>
                    </ig:BoundDataField>
                    <ig:BoundDataField DataFieldName="OriginalPortOfLoading" Key="OriginalPortOfLoading" Width="140px">
                        <Header Text="OriginalPortOfLoading:"/>
                    </ig:BoundDataField>
                    <ig:BoundDataField DataFieldName="LastOceanPort" Key="LastOceanPort" Width="90px">
                        <Header Text="LastOceanPort:"/>
                    </ig:BoundDataField>
                         <ig:BoundDataField DataFieldName="FinalPortOfDischarge" Key="FinalPortOfDischarge" Width="130px">
                        <Header Text="FinalPortOfDischarge:"/>
                    </ig:BoundDataField>
 			
                    <ig:BoundDataField DataFieldName="SealNumber" Key="SealNumber" Width="90px"  >
                        <Header Text="SealNumber:"/>
                    </ig:BoundDataField>
                    <ig:BoundDataField DataFieldName="RecordType" Key="RecordType"  Width="60px" Hidden="true"  >
                        <Header Text="RecordType:"/>
                    </ig:BoundDataField>
                    <ig:BoundDataField DataFieldName="TableProcessId" Key="TableProcessId"  Width="60px"  Hidden="true" >
                        <Header Text="TableProcessId:"/>
                    </ig:BoundDataField>
                    <ig:BoundDataField DataFieldName="ErrorSaving" Key="ErrorSaving"  Width="60px"  Hidden="true">
                        <Header Text="ErrorSaving:"/>
                    </ig:BoundDataField>
                  


 </Columns>
 <Behaviors>
 
 </Behaviors>
                </ig:WebDataGrid>
                                    <br />
</div>
   </asp:Content>

