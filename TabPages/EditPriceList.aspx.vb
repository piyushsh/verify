Imports BPADotNetCommonFunctions
Imports BPADotNetCommonFunctions.VT_CommonFunctions.MatrixFunctions
Imports System.Data.SqlClient
Imports System.Data
Imports System.Drawing
Imports System.Data.DataTableExtensions
Imports VTDBFunctions.VTDBFunctions


Partial Class EditPriceList
    Inherits MyBasePage


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        cmdSave_ProdPrice.OnClientClick = String.Format("ShowResendClose('{0}','{1}')", cmdSave_ProdPrice.UniqueID, "")
       
        Dim objDataPreserve As New BPADotNetCommonFunctions.VT_DataPreserve.DataPreserve


        If Not IsPostBack Then
            'fill the labels with selected customer and product info
            Dim objProd As New VTDBFunctions.VTDBFunctions.ProductsFunctions
            Dim objCust As New VTDBFunctions.VTDBFunctions.CustomerFunctions

            If Session("SelectedProductId") = "" Or Session("SelectedProductId") = 0 Then

                lblSelectedProductCode.Text = ""
            Else

                lblSelectedProductCode.Text = objProd.GetProductCode(Session("SelectedProductId"))

            End If

            If Session("SelectedCustomerId") = 0 Then
                lblSelectedCust.Text = ""
            Else
                lblSelectedCust.Text = objCust.GetCustomerNameForId(Session("SelectedCustomerId"))

            End If
            InitialisePriceBreakGrid()
            SetUpPriceBands()
        Else
            wdgCustPriceBreaks.DataSource = objDataPreserve.GetWDGDataFromSession(wdgCustPriceBreaks)
        End If


    End Sub

    Sub InitialisePriceBreakGrid()

        Dim dtPriceBandsGrid As New DataTable

        With dtPriceBandsGrid
            .Columns.Add("ProductName")
            .Columns.Add("CustomerName")
            .Columns.Add("FromQty")
            .Columns.Add("Special_Price")
            .Columns.Add("Comment")
            .Columns.Add("QuoteReference")
        End With

        Dim objDataPreserve As New BPADotNetCommonFunctions.VT_DataPreserve.DataPreserve
        objDataPreserve.BindDataToWDG(dtPriceBandsGrid, wdgCustPriceBreaks)

    End Sub
    Sub SetUpPriceBands()
        'fill the grid 
        'Dim objDataPreserve As New BPADotNetCommonFunctions.VT_DataPreserve.DataPreserve

        'Dim objdb As New VTDBFunctions.VTDBFunctions.PricingFunctions

        'Dim dtPriceBands As New DataTable
        'Dim dtPriceBandsGrid As New DataTable
        'Dim i As Integer
        'Dim dblLastToQty As Double


        'With dtPriceBandsGrid
        '    .Columns.Add("ProductName")
        '    .Columns.Add("CustomerName")

        '    .Columns.Add("CustomerCode")
        '    .Columns.Add("FromQty")
        '    .Columns.Add("Price")
        '    .Columns.Add("Comment")
        '    .Columns.Add("QuoteReference")
        '    .Columns.Add("Currency")
        '    .Columns.Add("DateOfQuote", Type.GetType("System.DateTime"))
        '    .Columns.Add("ProductId", Type.GetType("System.Int32"))
        '    .Columns.Add("CustomerId", Type.GetType("System.Int32"))

        'End With

        'wdgCustPriceBreaks.ClearDataSource()

        'dtPriceBands = objdb.GetSpecialItemsForCustAndProdBands(Session("SelectedCustomerId"), Session("SelectedProductId"))

        '        For i = 0 To dtPriceBands.Rows.Count - 1
        '            Dim drNew As DataRow = dtPriceBandsGrid.NewRow

        '            With drNew
        '                .Item("ProductName") = dtPriceBands.Rows(i).Item("ProductName").ToString.Trim
        '                .Item("CustomerName") = dtPriceBands.Rows(i).Item("CustomerName").ToString.Trim
        '        .Item("ProductId") = Session("SelectedProductId")
        '        .Item("CustomerId") = Session("SelectedCustomerId")
        '                .Item("CustomerCode") = dtPriceBands.Rows(i).Item("CustomerReference").ToString.Trim
        '                .Item("FromQty") = dtPriceBands.Rows(i).Item("FromQty")
        '                .Item("Price") = dtPriceBands.Rows(i).Item("Special_Price")
        '                .Item("Comment") = dtPriceBands.Rows(i).Item("Comment").ToString.Trim
        '                .Item("QuoteReference") = dtPriceBands.Rows(i).Item("QuoteReference").ToString.Trim
        '            End With
        '            dtPriceBandsGrid.Rows.Add(drNew)
        '        Next



        'objDataPreserve.BindDataToWDG(dtPriceBandsGrid, wdgCustPriceBreaks)
        'Session("VT_PriceBandsGrid") = dtPriceBandsGrid

        '' if there are items in the price break grid select the first one
        'If wdgCustPriceBreaks.Rows.Count > 0 Then
        '    Dim objC As New BPADotNetCommonFunctions.VT_CommonFunctions.CommonFunctions

        '    'Serialise the Select DataRow
        '    Dim intActiveRowIndex As Integer = 0

        '    Session("VT_PriceBreaks_SelectedRow") = objC.SerialiseWebDataGridRow(wdgCustPriceBreaks, intActiveRowIndex)

        '    wdgCustPriceBreaks.Behaviors.Activation.ActiveCell = wdgCustPriceBreaks.Rows(intActiveRowIndex).Items(1)

        '    Dim selectedRows As Infragistics.Web.UI.GridControls.SelectedRowCollection = wdgCustPriceBreaks.Behaviors.Selection.SelectedRows
        '    Dim wgdRow As Infragistics.Web.UI.GridControls.GridRecord = wdgCustPriceBreaks.Rows(0)
        '    selectedRows.Add(wgdRow)

        'End If
        'fill the grid below
        Dim objDataPreserve As New BPADotNetCommonFunctions.VT_DataPreserve.DataPreserve
        Dim objM As New VTDBFunctions.VTDBFunctions.VTMatrixFunctions

        Dim strKeyField As String
        Dim strFormName As String
        Dim strErr As String

        Dim objdb As New VTDBFunctions.VTDBFunctions.PricingFunctions
        Dim dtPriceBands As New DataTable
        Dim dtPriceBandsGrid As New DataTable
        Dim i As Integer
        Dim dblLastToQty As Double

        'Dim selectedProductRows As Infragistics.Web.UI.GridControls.SelectedRowCollection = wdgCustPriceBreaks.Behaviors.Selection.SelectedRows

        With dtPriceBandsGrid
            .Columns.Add("ProductName")
            .Columns.Add("CustomerName")
            .Columns.Add("txtCustPartNum")
            .Columns.Add("CustomerCode")
            .Columns.Add("FromQty", Type.GetType("System.Double"))
            .Columns.Add("Special_Price")
            .Columns.Add("Comment")
            .Columns.Add("QuoteReference")
            .Columns.Add("Currency")
            .Columns.Add("DateOfQuote", Type.GetType("System.DateTime"))
            .Columns.Add("ProductId", Type.GetType("System.Int32"))
            .Columns.Add("CustomerId", Type.GetType("System.Int32"))
        End With

        wdgCustPriceBreaks.ClearDataSource()

        '   For Each wdgRow As Infragistics.Web.UI.GridControls.GridRecord In selectedProductRows
        '  If UCase(wdgRow.Items.FindItemByKey("PriceType").Value) = "Q" Then    ' it is a PRICE BREAK record
        dtPriceBands = objdb.GetSpecialItemsForCustAndProdBands(Session("SelectedCustomerId"), Session("SelectedProductId"))

        'Dim astrItems(0) As String
        'astrItems(0) = "txtCustPartNum"

        'strFormName = "ProductDetails"
        'strKeyField = "MatrixId"
        'strErr = objM.GetBlockOfFormDataItems(Session("_VT_DotNetConnString"), strFormName, VTDBFunctions.VTDBFunctions.VTMatrixFunctions.Materials, astrItems, dtPriceBands, strKeyField)


        dtPriceBandsGrid.Merge(dtPriceBands)
        '   End If


        '   Next

        objDataPreserve.BindDataToWDG(dtPriceBandsGrid, wdgCustPriceBreaks)
        Session("VT_PriceBandsGrid") = dtPriceBandsGrid

        ' if there are items in the price break grid select the first one
        If wdgCustPriceBreaks.Rows.Count > 0 Then
            Dim objC As New BPADotNetCommonFunctions.VT_CommonFunctions.CommonFunctions

            'Serialise the Select DataRow
            Dim intActiveRowIndex As Integer = 0

            Session("VT_PriceBreaks_SelectedRow") = objC.SerialiseWebDataGridRow(wdgCustPriceBreaks, intActiveRowIndex)

            wdgCustPriceBreaks.Behaviors.Activation.ActiveCell = wdgCustPriceBreaks.Rows(intActiveRowIndex).Items(1)

            Dim selectedRows As Infragistics.Web.UI.GridControls.SelectedRowCollection = wdgCustPriceBreaks.Behaviors.Selection.SelectedRows
            Dim wgdRow As Infragistics.Web.UI.GridControls.GridRecord = wdgCustPriceBreaks.Rows(0)
            selectedRows.Add(wgdRow)

        End If

    End Sub
    Protected Sub btnAddPriceBreak_Click(sender As Object, e As ImageClickEventArgs) Handles btnAddPriceBreak.Click


        Dim objProd As New VTDBFunctions.VTDBFunctions.ProductsFunctions
        Dim dtProducts As New DataSet
        Dim intProductLine As Integer

        hdnAddEdit.Value = "ADD"

        'txtQtyFrom.Visible = True
        'txtQtyTo.Visible = True
        'lblQtyFrom.Visible = True
        'lblQtyTo.Visible = True
        lblPriceType.Visible = True
        lblQtyFrom.Visible = True
        ddlPriceType_NS.Visible = True
        txtQtyFrom.Visible = True

        lblAddEditPrice.Text = "Add Product Price for selected Customer"

            'set up add new price pop up
            ddlPriceType_NS.SelectedValue = "1"
            ddlPriceType_NS.Enabled = True
            txtComment.Text = ""
            txtQuoteRef.Text = ""
            txtQtyFrom.Text = ""
            txtSpecialPrice_NS.Text = ""
            lblCustomer.Visible = False
            ddlProductCategory_NS.Visible = False
            ddlProduct_NS.Visible = False
            lblProductName.Visible = True
        lblProductName.Text = objProd.GetProductNameForId(Session("SelectedProductId"))


            ModalPopupExtender1.Show()
        
    End Sub

    Protected Sub btnDeletePriceBreak_Click(sender As Object, e As ImageClickEventArgs) Handles btnDeletePriceBreak.Click
     
        If Not Session("VT_PriceBreaks_SelectedRow") Is Nothing Then
            Dim objDB As New VTDBFunctions.VTDBFunctions.PricingFunctions

            With Session("VT_PriceBreaks_SelectedRow").rows(0)

                objDB.DeleteSpecialPrice(.item("ProductId"), .item("CustomerId"), .item("Special_Price"), "Q", .item("FromQty"))
                objDB.InsertPriceHistoryItem_New(.item("CustomerId"), .item("ProductId"), .item("Special_Price"), PortalFunctions.Now, Session("_VT_CurrentUserName"), "Add", , .item("FromQty"))

                lblMsg.Text = "The selected item has been deleted"
                ModalPopupExtenderMsg.Show()

                SetUpPriceBands()

            End With
            Session("VT_PriceBreaks_SelectedRow") = Nothing
        Else
            lblMsg.Text = "Please select an item from the grid to delete"
            ModalPopupExtenderMsg.Show()

        End If
    End Sub


    Protected Sub wdgCustPriceBreaks_ActiveCellChanged(sender As Object, e As Infragistics.Web.UI.GridControls.ActiveCellEventArgs) Handles wdgCustPriceBreaks.ActiveCellChanged
        Dim objC As New BPADotNetCommonFunctions.VT_CommonFunctions.CommonFunctions
        Dim intActiveRowIndex As Integer = -1
        If wdgCustPriceBreaks.Behaviors.Activation.ActiveCell IsNot Nothing Then
            intActiveRowIndex = wdgCustPriceBreaks.Behaviors.Activation.ActiveCell.Row.Index 'e.CurrentActiveCell.Row.Index '
        Else
            If wdgCustPriceBreaks.Rows.Count > 0 Then
                intActiveRowIndex = 0
            End If
        End If
        'Serialise the Select DataRow if one is active
        If intActiveRowIndex <> -1 Then
            Session("VT_PriceBreaks_SelectedRow") = objC.SerialiseWebDataGridRow(wdgCustPriceBreaks, intActiveRowIndex)
            wdgCustPriceBreaks.Behaviors.Activation.ActiveCell = wdgCustPriceBreaks.Rows(intActiveRowIndex).Items(1)
        End If

    End Sub

    Protected Sub wdgCustPriceBreaks_RowSelectionChanged(sender As Object, e As Infragistics.Web.UI.GridControls.SelectedRowEventArgs) Handles wdgCustPriceBreaks.RowSelectionChanged
        Dim objC As New BPADotNetCommonFunctions.VT_CommonFunctions.CommonFunctions

        'Serialise the Select DataRow
        Dim intActiveRowIndex As Integer = wdgCustPriceBreaks.Behaviors.Activation.ActiveCell.Row.Index

        Session("VT_PriceBreaks_SelectedRow") = objC.SerialiseWebDataGridRow(wdgCustPriceBreaks, intActiveRowIndex)

        wdgCustPriceBreaks.Behaviors.Activation.ActiveCell = wdgCustPriceBreaks.Rows(intActiveRowIndex).Items(1)

    End Sub
    Protected Sub btnEditPriceBreak_Click(sender As Object, e As ImageClickEventArgs) Handles btnEditPriceBreak.Click


        Dim objProd As New VTDBFunctions.VTDBFunctions.ProductsFunctions
        Dim dtProducts As New DataSet
        Dim intProductLine As Integer

        'txtQtyFrom.Visible = True
        'txtQtyTo.Visible = True
        'lblQtyFrom.Visible = True
        'lblQtyTo.Visible = True

        lblAddEditPrice.Text = "Edit Product Price for selected Customer"

        If Not Session("VT_PriceBreaks_SelectedRow") Is Nothing Then
            lblPriceType.Visible = False
            lblQtyFrom.Visible = True
            ddlPriceType_NS.Visible = False
            txtQtyFrom.Visible = True

            'set up add new price pop up
            ddlPriceType_NS.SelectedValue = "1"
            txtComment.Text = Session("VT_PriceBreaks_SelectedRow").rows(0).item("Comment")
            txtQuoteRef.Text = Session("VT_PriceBreaks_SelectedRow").rows(0).item("QuoteReference")
            txtQtyFrom.Text = Session("VT_PriceBreaks_SelectedRow").rows(0).item("FromQty")
            txtSpecialPrice_NS.Text = Session("VT_PriceBreaks_SelectedRow").rows(0).item("Special_Price")
            txtCurrency.Text = Session("VT_PriceBreaks_SelectedRow").rows(0).item("Currency")
            wdpDateOfQuote.Value = Session("VT_PriceBreaks_SelectedRow").rows(0).item("DateOfQuote")

            ddlProductCategory_NS.Visible = False
            lblCustomer.Visible = False
            ddlProduct_NS.Visible = False
            lblProductName.Visible = True
            lblProductName.Text = Session("VT_PriceBreaks_SelectedRow").rows(0).item("ProductName")
            Session("EditPriceBreak") = True
            hdnAddEdit.Value = "EDIT"
            ModalPopupExtender1.Show()
        Else
            'set up add new price pop up
            lblMsg.Text = "Please select a price break to edit"
            ModalPopupExtenderMsg.Show()

        End If

    End Sub

    Protected Sub cmdSave_ProdPrice_Click(sender As Object, e As ImageClickEventArgs) Handles cmdSave_ProdPrice.Click
        Dim objDB As New VTDBFunctions.VTDBFunctions.PricingFunctions
        Dim lngProductId As Long

        'add checks that all data is entered
      

        If txtSpecialPrice_NS.Text = "" Or IsNumeric(txtSpecialPrice_NS.Text) = False Then
            lblMsg.Text = "Please enter a valid number for the price"
            ModalPopupExtenderMsg.Show()
            Exit Sub

        End If

        If ddlPriceType_NS.SelectedValue = "1" Then 'price break' then
            If IsNumeric(txtQtyFrom.Text) = False Then
                lblMsg.Text = "Please enter a valid entry for the Quantity Greater Than figure"
                ModalPopupExtenderMsg.Show()
                Exit Sub
            End If
        Else 'special price

        End If



        If hdnAddEdit.Value = "EDIT" Then
            With Session("VT_PriceBreaks_SelectedRow").rows(0)
                Select Case ddlPriceType_NS.SelectedValue
                    Case "1" 'price break

                        objDB.UpdateSpecialPrice_Steripack(.item("ProductId"), .item("CustomerId"), CDbl(txtSpecialPrice_NS.Text), _
                                                   "Q", "", txtComment.Text, .item("FromQty"), txtQtyFrom.Text, txtQuoteRef.Text, txtCurrency.Text, CDate(wdpDateOfQuote.Value).ToString("s"))
                       
                    Case "2" 'special price
                        objDB.UpdateSpecialPrice_Steripack(.item("ProductId"), .item("CustomerId"), CDbl(txtSpecialPrice_NS.Text), _
                                                  "P", "", txtComment.Text, , , txtQuoteRef.Text)
                End Select
                objDB.InsertPriceHistoryItem_New(.item("CustomerId"), .item("ProductId"), CDbl(txtSpecialPrice_NS.Text), PortalFunctions.Now, Session("_VT_CurrentUserName"), "Edit", txtComment.Text, txtQtyFrom.Text, .item("FromQty"), txtQuoteRef.Text)
            End With
        Else
            'if this is a new  price being added, we should not allow adding for a product and customer that exist
            If ddlProduct_NS.Visible = True Then
                lngProductId = ddlProduct_NS.SelectedValue
            Else
                lngProductId = Session("SelectedProductId")
            End If

            Select Case ddlPriceType_NS.SelectedValue
                Case "1" 'price break
                    objDB.InsertSpecialPrice_Steripack(lngProductId, Session("SelectedCustomerId"), CDbl(txtSpecialPrice_NS.Text), "Q", "", txtComment.Text, txtQtyFrom.Text, txtQuoteRef.Text, txtCurrency.Text, CDate(wdpDateOfQuote.Value).ToString("s"))


                Case "2" 'special price
                    objDB.InsertSpecialPrice_Steripack(lngProductId, Session("SelectedCustomerId"), CDbl(txtSpecialPrice_NS.Text), "P", "", txtComment.Text, , txtQuoteRef.Text)

            End Select
            objDB.InsertPriceHistoryItem_New(Session("SelectedCustomerId"), lngProductId, CDbl(txtSpecialPrice_NS.Text), PortalFunctions.Now, Session("_VT_CurrentUserName"), "Add", txtComment.Text, txtQtyFrom.Text, , txtQuoteRef.Text)
        End If


        'SetupProductsForSelectedCustomers()

        SetUpPriceBands()
    End Sub

    Protected Sub btnBack1_Click(sender As Object, e As ImageClickEventArgs) Handles btnBack1.Click
        Response.Redirect("~/TabPages/HoldQueueManager.aspx")

    End Sub

    Protected Sub btnBack2_Click(sender As Object, e As ImageClickEventArgs) Handles btnBack2.Click
        Response.Redirect("~/TabPages/HoldQueueManager.aspx")
    End Sub
End Class



