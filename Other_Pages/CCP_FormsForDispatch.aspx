<%@ Page Title="" Language="VB"  AutoEventWireup="false" CodeFile="CCP_FormsForDispatch.aspx.vb" EnableEventValidation="False" Inherits="OtherPages_CCP_FormsForDispatch" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="ajaxToolkit" %>
<%@ Register Assembly="Infragistics4.Web.v13.2, Version=13.2.20132.2028, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" namespace="Infragistics.Web.UI.DisplayControls" tagprefix="ig" %>
<%@ Register Assembly="Infragistics4.Web.v13.2, Version=13.2.20132.2028, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" namespace="Infragistics.Web.UI.EditorControls" tagprefix="ig" %>
<%@ Register Assembly="Infragistics4.Web.v13.2, Version=13.2.20132.2028, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" namespace="Infragistics.Web.UI.GridControls" tagprefix="ig" %>
<%@ Register Assembly="Infragistics4.Web.jQuery.v13.2, Version=13.2.20132.2028, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" namespace="Infragistics.Web.UI.EditorControls" tagprefix="ig" %>
<%@ Register Assembly="Infragistics4.WebUI.WebHtmlEditor.v13.2, Version=13.2.20132.2028, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" namespace="Infragistics.WebUI.WebHtmlEditor" tagprefix="ighedit" %>
<%@ Register Assembly="Infragistics4.WebUI.UltraWebChart.v13.2, Version=13.2.20132.2028, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" namespace="Infragistics.WebUI.UltraWebChart" tagprefix="igchart" %>
<%@ Register Assembly="Infragistics4.WebUI.UltraWebChart.v13.2, Version=13.2.20132.2028, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" namespace="Infragistics.UltraChart.Resources.Appearance" tagprefix="igchartprop" %>
<%@ Register Assembly="Infragistics4.WebUI.UltraWebChart.v13.2, Version=13.2.20132.2028, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" namespace="Infragistics.UltraChart.Data" tagprefix="igchartdata" %>
<%@ Register Assembly="Infragistics4.Web.v13.2, Version=13.2.20132.2028, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" namespace="Infragistics.Web.UI.ListControls" tagprefix="ig" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
    <head id="Head1" runat="server">
    <title></title>
            <style type="text/css">
        .style1
        {
            height: 4px;
        }
    </style>
  

    <link href="../jg_styles.css" rel="stylesheet" type="text/css" />
    <link href="../Verify_Infragistics.css" rel="stylesheet" type="text/css" />
</head>

<body>
    <form id="form1" runat="server">

    <asp:ScriptManager ID="ScriptManager1" runat="server">
        <Scripts>
            <asp:ScriptReference Path="Scripts/CommonScripts.js" />
        </Scripts>
    </asp:ScriptManager>




    <table cellpadding="0" cellspacing="0" >
        <tr>
            <td style="margin-left: 20px">
                <table id="tblReportControl0" cellpadding="0" cellspacing="0" 
                    style="width: 985px">
                    <tr>
                        <td>
                                          
                           <asp:Label ID="lblReportName" runat="server" Font-Bold="True" Font-Names="Arial"
                                Font-Size="11pt" Text="CCP Data for Dispatch Docket #"
                                Width="280px" ForeColor="#CC9900"></asp:Label>
                            &nbsp;
                                                 <asp:Label ID="lblDocketNum" runat="server" Font-Bold="False" Font-Names="Arial"
                                                     Font-Size="11pt" Text="Selected Docket..."
                                                     Width="500px"></asp:Label>


                            <asp:ImageButton ID="btnRunReport" runat="server"
                                ImageUrl="~/App_Themes/Buttons/Activate.gif"
                                Style="text-decoration: underline" />


                        </td>
                    </tr>
                    <tr>
                        <td  style="font-family: Arial; font-size: 3px">&nbsp;&nbsp;&nbsp; &nbsp;</td>
                    </tr>
                    <tr>
                        <td style="border-width: 2px; border-color: #C0C0C0; font-family: Arial; font-size: 3px; border-top-style: solid;">&nbsp;&nbsp; &nbsp;</td>
                    </tr>
                    <tr>
                        <td style="font-family: Arial; font-size: 3px">&nbsp;&nbsp; &nbsp;</td>
                    </tr>
                </table>
            </td>
            <td>&nbsp;</td>
        </tr>
    </table>
        <table>
            <tr>
                <td>
                            <asp:Label ID="Label8" runat="server" Font-Bold="False" Font-Names="Arial"
        Font-Size="11pt" Text="This Document details all Critical Control Point Form Data for products on a selected Dispatch Docket"
        ></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td><asp:Label ID="Label4" runat="server" Font-Bold="False" Font-Names="Arial"
        Font-Size="11pt" Text="Ingredients Hierarchy Table - "
       ></asp:Label>
                            <asp:Label ID="Label5" runat="server" Font-Bold="False" Font-Names="Arial"
        Font-Size="10pt" Text="Items in "
       ></asp:Label>
        <asp:Label ID="Label6" runat="server" Font-Bold="False" Font-Names="Arial" ForeColor="Green" Font-Italic="true"
        Font-Size="10pt" Text="green "
        ></asp:Label>
                <asp:Label ID="Label7" runat="server" Font-Bold="False" Font-Names="Arial" 
        Font-Size="10pt" Text="in this table are not produced by this supplier and therefore don't appear in the CCP tables below"
       ></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                     <asp:Label ID="Label9" runat="server" Font-Bold="False" Font-Names="Arial" 
        Font-Size="10pt" Text=" - Trace codes are appended to product names in square brackets i.e. [ ]"
       ></asp:Label>
                </td>
            </tr>
        </table>     
   
    <ig:WebDataGrid ID="wdgIngredientsHierarchy" runat="server"
        AltItemCssClass="VerifyGrid_Report_AlternateRow"
        AutoGenerateColumns="False" Font-Names="Arial"
        ItemCssClass="VerifyGrid_Report_Row"
        EnableTheming="True" FooterCaptionCssClass="VerifyGrid_Report_Footer"
        HeaderCaptionCssClass="VerifyGrid_Report_HeaderGreenSmall" BackColor="White"
        CssClass="VerifyGrid_Report_Frame">
        <Columns>
            <ig:BoundDataField DataFieldName="Dispatch_Item" Key="Dispatch_Item" Width="200px"
                EnableMultiline="True">
                <Header Text="Dispatch Item (Level 0):" />
            </ig:BoundDataField>
            <ig:BoundDataField DataFieldName="Ingredients_Level_1" Key="Ingredients_Level_1" Width="200px"
                EnableMultiline="True">
                <Header Text="Ingredients Level 1:" />
            </ig:BoundDataField>
                        <ig:BoundDataField DataFieldName="Ingredients_Level_2" Key="Ingredients_Level_2" Width="200px"
                EnableMultiline="True">
                <Header Text="Ingredients Level 2:" />
            </ig:BoundDataField>
                        <ig:BoundDataField DataFieldName="Ingredients_Level_3" Key="Ingredients_Level_3" Width="200px"
                EnableMultiline="True">
                <Header Text="Ingredients Level 3:" />
            </ig:BoundDataField>
                        <ig:BoundDataField DataFieldName="Ingredients_Level_4" Key="Ingredients_Level_4" Width="200px"
                EnableMultiline="True">
                <Header Text="Ingredients Level 4:" />
            </ig:BoundDataField>
            <ig:BoundDataField DataFieldName="Ingredients_Level_5" Key="Ingredients_Level_5" Width="200px"
                EnableMultiline="True">
                <Header Text="Ingredients Level 5:" />
            </ig:BoundDataField>
             <ig:BoundDataField DataFieldName="MatrixLinkId" Key="MatrixLinkId" Hidden = "True" 
                EnableMultiline="True">
                
            </ig:BoundDataField>

          
        </Columns>
        <Behaviors>
            <ig:Activation ActiveRowCssClass="SelectedRow">
            </ig:Activation>
            <ig:ColumnResizing>
            </ig:ColumnResizing>
            <ig:Sorting Enabled="True"
                AscendingImageUrl="~/App_Themes/Buttons/Up_Arrow_small.gif"
                DescendingImageUrl="~/App_Themes/Buttons/Down_Arrow_small.gif">
            </ig:Sorting>
            <ig:Selection CellClickAction="Row" RowSelectType="Single">
                <AutoPostBackFlags RowSelectionChanged="True" />
            </ig:Selection>
        </Behaviors>
    </ig:WebDataGrid>
    <br />
    <asp:Label ID="Label3" runat="server" Font-Bold="False" Font-Names="Arial"
        Font-Size="11pt" Text="Cooking and Cooling CCP Data"
        Width="300px"></asp:Label>
    <br />
    <ig:WebDataGrid ID="wdgCookCoolData" runat="server"
        AltItemCssClass="VerifyGrid_Report_AlternateRow"
        AutoGenerateColumns="False" Font-Names="Arial"
        ItemCssClass="VerifyGrid_Report_Row"
        EnableTheming="True" FooterCaptionCssClass="VerifyGrid_Report_Footer"
        HeaderCaptionCssClass="VerifyGrid_Report_HeaderGreenSmall" BackColor="White"
        CssClass="VerifyGrid_Report_Frame">
        <Columns>
            <ig:BoundDataField DataFieldName="DocketNum" Key="DocketNum" Width="110px" Hidden="true"
                EnableMultiline="True">
                <Header Text="Docket #:" />
            </ig:BoundDataField>
            <%---
            <ig:BoundDataField DataFieldName="LineItemNum" Key="LineItemNum" Width="50px"
                EnableMultiline="True">
                <Header Text="Line Item:" />
            </ig:BoundDataField>
            ---%>
            <ig:BoundDataField DataFieldName="Product_Name" Key="Product_Name" Width="150px"
                EnableMultiline="True">
                <Header Text="Product Name:" />
            </ig:BoundDataField>
            <ig:BoundDataField DataFieldName="Generation" Key="Generation" Width="70px"
                EnableMultiline="True">
                <Header Text="Ingredient Level:" />
            </ig:BoundDataField>
            <ig:BoundDataField DataFieldName="Parent_Product_Name" Key="Parent_Product_Name" Width="150px"
                EnableMultiline="True">
                <Header Text="Parent Product Name:" />
            </ig:BoundDataField>
            <ig:BoundDataField DataFieldName="TraceCodeDesc" Key="TraceCodeDesc" Width="100px"
                EnableMultiline="True">
                <Header Text="Trace Code:" />
            </ig:BoundDataField>
            <ig:BoundDataField DataFieldName="GridName" Key="GridName" Width="100px"
                EnableMultiline="True">
                <Header Text="Data Type:" />
            </ig:BoundDataField>
            <ig:BoundDataField DataFieldName="Field0" Key="Field0" Width="70px"
                EnableMultiline="True">
                <Header Text="Container:" />
            </ig:BoundDataField>
            <ig:BoundDataField DataFieldName="Field1" Key="Field1" Width="118px"
                DataFormatString="{0:G}" DataType="System.DateTime"
                EnableMultiline="True">
                <Header Text="Start:" />
            </ig:BoundDataField>
            <ig:BoundDataField DataFieldName="Field2" Key="Field2" Width="118px"
                DataFormatString="{0:G}" DataType="System.DateTime"
                EnableMultiline="True">
                <Header Text="Finish:" />
            </ig:BoundDataField>
            <ig:BoundDataField DataFieldName="Field3" Key="Field3" Width="55px"
                DataFormatString="{0:T}" DataType="System.DateTime"
                EnableMultiline="True">
                <Header Text="Period:" />
            </ig:BoundDataField>
            <ig:BoundDataField DataFieldName="Field4" Key="Field4" Width="55px"
                EnableMultiline="True">
                <Header Text="Temp(c):" />
            </ig:BoundDataField>
            <ig:BoundDataField DataFieldName="Field5" Key="Field5" Width="55px"
                EnableMultiline="True">
                <Header Text="Status:" />
            </ig:BoundDataField>
            <ig:BoundDataField DataFieldName="Field6" Key="Field6" Width="55px"
                EnableMultiline="True">
                <Header Text="Retry:" />
            </ig:BoundDataField>
            <ig:BoundDataField DataFieldName="Field7" Key="Field7" Width="95px"
                EnableMultiline="True">
                <Header Text="Entered By:" />
            </ig:BoundDataField>
            <ig:BoundDataField DataFieldName="Field8" Key="Field8" Width="118px"
                EnableMultiline="True">
                <Header Text="Date Entered:" />
            </ig:BoundDataField>
            <ig:BoundDataField DataFieldName="Field9" Key="Field9" Width="180px"
                EnableMultiline="True">
                <Header Text="Comment:" />
            </ig:BoundDataField>
        </Columns>
        <Behaviors>
            <ig:Activation ActiveRowCssClass="SelectedRow">
            </ig:Activation>
            <ig:ColumnResizing>
            </ig:ColumnResizing>
            <ig:Sorting Enabled="True"
                AscendingImageUrl="~/App_Themes/Buttons/Up_Arrow_small.gif"
                DescendingImageUrl="~/App_Themes/Buttons/Down_Arrow_small.gif">
            </ig:Sorting>
            <ig:Selection CellClickAction="Row" RowSelectType="Single">
                <AutoPostBackFlags RowSelectionChanged="True" />
            </ig:Selection>
        </Behaviors>
    </ig:WebDataGrid>
    <br />
    <asp:Label ID="Label1" runat="server" Font-Bold="False" Font-Names="Arial"
        Font-Size="11pt" Text="Temperature Spot Check CCP Data  -  Ingredient Temperature Details"
        Width="600px"></asp:Label>
    <br />
    <ig:WebDataGrid ID="wdgStartData" runat="server"
        AltItemCssClass="VerifyGrid_Report_AlternateRow"
        AutoGenerateColumns="False" Font-Names="Arial"
        ItemCssClass="VerifyGrid_Report_Row"
        EnableTheming="True" FooterCaptionCssClass="VerifyGrid_Report_Footer"
        HeaderCaptionCssClass="VerifyGrid_Report_HeaderGreenSmall" BackColor="White"
        CssClass="VerifyGrid_Report_Frame">
        <Columns>
            <ig:BoundDataField DataFieldName="DocketNum" Key="DocketNum" Width="110px" Hidden="true"
                EnableMultiline="True">
                <Header Text="Docket #:" />
            </ig:BoundDataField>
            <%---
            <ig:BoundDataField DataFieldName="LineItemNum" Key="LineItemNum" Width="50px"
                EnableMultiline="True">
                <Header Text="Line Item:" />
            </ig:BoundDataField>
            ---%>
            <ig:BoundDataField DataFieldName="Field3" Key="Field3" Width="150px"
                EnableMultiline="True">
                <Header Text="Product Name:" />
            </ig:BoundDataField>
            <ig:BoundDataField DataFieldName="Generation" Key="Generation" Width="70px"
                EnableMultiline="True">
                <Header Text="Ingredient Level:" />
            </ig:BoundDataField>
            <ig:BoundDataField DataFieldName="Product_Name" Key="Product_Name" Width="150px"
                EnableMultiline="True">
                <Header Text="Parent Product Name:" />
            </ig:BoundDataField>
            <ig:BoundDataField DataFieldName="TraceCodeDesc" Key="TraceCodeDesc" Width="100px"
                EnableMultiline="True">
                <Header Text="Trace Code:" />
            </ig:BoundDataField>
            <ig:BoundDataField DataFieldName="GridName" Key="GridName" Width="100px"
                EnableMultiline="True">
                <Header Text="Data Type:" />
            </ig:BoundDataField>
            <ig:BoundDataField DataFieldName="Field0" Key="Field0" Width="70px"
                EnableMultiline="True">
                <Header Text="Container:" />
            </ig:BoundDataField>
            <ig:BoundDataField DataFieldName="Field1" Key="Field1" Width="118px"
                DataFormatString="{0:G}" DataType="System.DateTime"
                EnableMultiline="True">
                <Header Text="Check Time:" />
            </ig:BoundDataField>
            <ig:BoundDataField DataFieldName="Field2" Key="Field2" Width="65px"
                EnableMultiline="True">
                <Header Text="Temp(c):" />
            </ig:BoundDataField>

            <ig:BoundDataField DataFieldName="Field4" Key="Field4" Width="55px"
                EnableMultiline="True">
                <Header Text="Batch:" />
            </ig:BoundDataField>
            <ig:BoundDataField DataFieldName="Field5" Key="Field5" Width="55px"
                EnableMultiline="True">
                <Header Text="Status:" />
            </ig:BoundDataField>
            <ig:BoundDataField DataFieldName="Field6" Key="Field6" Width="95px"
                EnableMultiline="True">
                <Header Text="Entered By:" />
            </ig:BoundDataField>
            <ig:BoundDataField DataFieldName="Field7" Key="Field7" Width="118px"
                EnableMultiline="True">
                <Header Text="Date Entered:" />
            </ig:BoundDataField>
            <ig:BoundDataField DataFieldName="Field8" Key="Field8" Width="180px"
                EnableMultiline="True">
                <Header Text="Comment:" />
            </ig:BoundDataField>
        </Columns>
        <Behaviors>
            <ig:Activation ActiveRowCssClass="SelectedRow">
            </ig:Activation>
            <ig:ColumnResizing>
            </ig:ColumnResizing>
            <ig:Sorting Enabled="True"
                AscendingImageUrl="~/App_Themes/Buttons/Up_Arrow_small.gif"
                DescendingImageUrl="~/App_Themes/Buttons/Down_Arrow_small.gif">
            </ig:Sorting>
            <ig:Selection CellClickAction="Row" RowSelectType="Single">
                <AutoPostBackFlags RowSelectionChanged="True" />
            </ig:Selection>
        </Behaviors>
    </ig:WebDataGrid>
    <br />
    <asp:Label ID="Label2" runat="server" Font-Bold="False" Font-Names="Arial"
        Font-Size="11pt" Text="Temperature Spot Check CCP Data  -  Cook Temperature Spot Check Details"
        Width="650px"></asp:Label>
    <br />
    <ig:WebDataGrid ID="wdgSpotTempData" runat="server"
        AltItemCssClass="VerifyGrid_Report_AlternateRow"
        AutoGenerateColumns="False" Font-Names="Arial"
        ItemCssClass="VerifyGrid_Report_Row"
        EnableTheming="True" FooterCaptionCssClass="VerifyGrid_Report_Footer"
        HeaderCaptionCssClass="VerifyGrid_Report_HeaderGreenSmall" BackColor="White"
        CssClass="VerifyGrid_Report_Frame">
        <Columns>
            <ig:BoundDataField DataFieldName="DocketNum" Key="DocketNum" Width="110px" Hidden="true"
                EnableMultiline="True">
                <Header Text="Docket #:" />
            </ig:BoundDataField>
            <%---
            <ig:BoundDataField DataFieldName="LineItemNum" Key="LineItemNum" Width="50px"
                EnableMultiline="True">
                <Header Text="Line Item:" />
            </ig:BoundDataField>
            ---%>
            <ig:BoundDataField DataFieldName="Product_Name" Key="Product_Name" Width="150px"
                EnableMultiline="True">
                <Header Text="Product Name:" />
            </ig:BoundDataField>
            <ig:BoundDataField DataFieldName="Generation" Key="Generation" Width="70px"
                EnableMultiline="True">
                <Header Text="Ingredient Level:" />
            </ig:BoundDataField>
            <ig:BoundDataField DataFieldName="Parent_Product_Name" Key="Parent_Product_Name" Width="150px"
                EnableMultiline="True">
                <Header Text="Parent Product Name:" />
            </ig:BoundDataField>
            <ig:BoundDataField DataFieldName="TraceCodeDesc" Key="TraceCodeDesc" Width="100px"
                EnableMultiline="True">
                <Header Text="Trace Code:" />
            </ig:BoundDataField>
            <ig:BoundDataField DataFieldName="GridName" Key="GridName" Width="100px"
                EnableMultiline="True">
                <Header Text="Data Type:" />
            </ig:BoundDataField>
            <ig:BoundDataField DataFieldName="Field0" Key="Field0" Width="90px"
                EnableMultiline="True">
                <Header Text="Spot Check#:" />
            </ig:BoundDataField>
            <ig:BoundDataField DataFieldName="Field1" Key="Field1" Width="118px"
                DataFormatString="{0:G}" DataType="System.DateTime"
                EnableMultiline="True">
                <Header Text="SpotCheckTime:" />
            </ig:BoundDataField>

            <ig:BoundDataField DataFieldName="Field2" Key="Field2" Width="65px"
                EnableMultiline="True">
                <Header Text="Temp(c):" />
            </ig:BoundDataField>
            <ig:BoundDataField DataFieldName="Field3" Key="Field3" Width="55px"
                EnableMultiline="True">
                <Header Text="Status:" />
            </ig:BoundDataField>
            <ig:BoundDataField DataFieldName="Field4" Key="Field4" Width="95px"
                EnableMultiline="True">
                <Header Text="Entered By:" />
            </ig:BoundDataField>
            <ig:BoundDataField DataFieldName="Field5" Key="Field5" Width="118px"
                EnableMultiline="True">
                <Header Text="Date Entered:" />
            </ig:BoundDataField>
            <ig:BoundDataField DataFieldName="Field6" Key="Field6" Width="180px"
                EnableMultiline="True">
                <Header Text="Comment:" />
            </ig:BoundDataField>
        </Columns>
        <Behaviors>
            <ig:Activation ActiveRowCssClass="SelectedRow">
            </ig:Activation>
            <ig:ColumnResizing>
            </ig:ColumnResizing>
            <ig:Sorting Enabled="True"
                AscendingImageUrl="~/App_Themes/Buttons/Up_Arrow_small.gif"
                DescendingImageUrl="~/App_Themes/Buttons/Down_Arrow_small.gif">
            </ig:Sorting>
            <ig:Selection CellClickAction="Row" RowSelectType="Single">
                <AutoPostBackFlags RowSelectionChanged="True" />
            </ig:Selection>
        </Behaviors>
    </ig:WebDataGrid>


    </form>
    </body>


</html>

