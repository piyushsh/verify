Imports BPADotNetCommonFunctions
Imports System.Data.SqlClient
Imports System.Data
Imports System.IO
Imports System.Drawing
Imports VTDBFunctions.VTDBFunctions

Partial Class TabPages_Fulfill_Opening
    Inherits MyBasePage

    Const cView = 0
    Const cReference = 1
    Const cName = 2


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then



            ddlProducts.Attributes.Add("onchange", "javascript:HandleProductOnAddPanel('" & ddlProducts.ClientID & "', '" & lblPnlProdMessage.ClientID & "', '" & txtProductCode.ClientID & "')")
            txtProductCode.Attributes.Add("onblur", "javascript:HandleProductCodeOnAddPanel('" & txtProductCode.ClientID & "', '" & lblPnlProdMessage.ClientID & "', '" & CascadingDropDownAddProduct.BehaviorID & "', '" & CascadingDropDownAddCategory.BehaviorID & "')")

            cmdOKConfirm.OnClientClick = String.Format("EditItemOKClose('{0}','{1}')", cmdOKConfirm.UniqueID, "")
            cmdOKConfirmEdit.OnClientClick = String.Format("AddBatchItemOKClose('{0}','{1}')", cmdOKConfirmEdit.UniqueID, "")

            cmdMsgCancel2.OnClientClick = String.Format("CancelClicked('{0}','{1}')", cmdMsgCancel2.UniqueID, "")
            cmdMsgOk2.OnClientClick = String.Format("OkClicked('{0}','{1}')", cmdMsgOk2.UniqueID, "")


        
            txtpnlTraceCode_NS.Attributes.Add("onblur", "javascript:HandleTraceCodeOnEditPanel('" & txtpnlTraceCode_NS.ClientID & "', '" & hdnPnlTraceCodeId.ClientID & "', '" & lblPnlTraceMessage.ClientID & "')")
            txtPnlSerialNum.Attributes.Add("onblur", "javascript:QtyWgtForSerialNumEditPanel('" & txtPnlSerialNum.ClientID & "', '" & txtPnlQtyToPick.ClientID & "', '" & txtPnlWgtToPick.ClientID & "', '" & ddlLocationEdit.ClientID & "', '" & lblPnlSerialMessage.ClientID & "')")

            txtPnlNewBatchTrace.Attributes.Add("onblur", "javascript:HandleTraceCodeOnAddPanel('" & txtPnlNewBatchTrace.ClientID & "', '" & hdnPnlAddTraceCodeId.ClientID & "', '" & lblPnlAddTraceMessage.ClientID & "')")
            txtPnlNewBatchSerialNum.Attributes.Add("onblur", "javascript:QtyWgtForSerialNumAddPanel('" & txtPnlNewBatchSerialNum.ClientID & "', '" & txtPnlNewBatchQty.ClientID & "', '" & txtPnlNewBatchWgt.ClientID & "', '" & ddlNewBatchLocationEdit.ClientID & "', '" & lblPnlAddSerialMessage.ClientID & "')")
            cmdProdAdd.OnClientClick = String.Format("return ShowAddClose('{0}','{1}')", cmdProdAdd.UniqueID, "")


            PopulateLocationsCombo()


            Select Case UCase(ddlMainViewType.SelectedValue)
                Case "ORDER"
                    FillGridForOrdersView()
                    SetGridHeaders(ddlMainViewType.SelectedValue)
                Case "PRODUCT"
                    FillGridForProductView()
                    SetGridHeaders(ddlMainViewType.SelectedValue)
                Case "LOCATION"
                    FillGridForLocationsView()
                    SetGridHeaders(ddlMainViewType.SelectedValue)
            End Select


            Dim objCommonFuncs As New BPADotNetCommonFunctions.VT_CommonFunctions.CommonFunctions
            Dim strSerialNums As String = objCommonFuncs.GetConfigItem("SerialNums")

            Me.CurrentSession.VT_UserSerialNumbers = False

            If UCase(objCommonFuncs.GetConfigItem("ShowAddByScanning")) = "YES" Then
                btnAddSkid.Visible = True
            Else
                btnAddSkid.Visible = False

            End If

            If UCase(strSerialNums) = "YES" Then
                lblPnlSerialNum.Visible = True
                txtPnlSerialNum.Visible = True
                txtPnlNewBatchSerialNum.Visible = True
                lblPnlNewBatchSerialNum.Visible = True
                Me.CurrentSession.VT_UserSerialNumbers = True

            Else
                lblPnlSerialNum.Visible = False
                txtPnlSerialNum.Visible = False
                txtPnlNewBatchSerialNum.Visible = False
                lblPnlNewBatchSerialNum.Visible = False
                Me.CurrentSession.VT_UserSerialNumbers = False

            End If


            Dim strCalcWeightFromQty As String = objCommonFuncs.GetConfigItem("CalcWeightFromQty")

            hidCalcWgtFromQty.Value = UCase(strCalcWeightFromQty)

            If UCase(strCalcWeightFromQty) = "YES" Then
                Me.CurrentSession.VT_CalcQtyFromWgt = True
            Else
                Me.CurrentSession.VT_CalcQtyFromWgt = False
            End If
            If Session("CameFromProductStockPage") = True Then
                Session("CameFromProductStockPage") = False
                wdgOrderItemsClicked(Me.CurrentSession.VT_SelectedFulfilOrderItemsGridRow_V2)

                'If Session("_VT_OrdersGrid") IsNot Nothing Then
                '    wdgOrderItems.DataSource = Session("_VT_OrdersGrid")
                'End If
                'If Session("_VT_ProductGrid") IsNot Nothing Then
                '    wdgOrderItemsProds.DataSource = Session("_VT_ProductGrid")
                'End If
                'If Session("_VT_LocationsGrid") IsNot Nothing Then
                '    wdgOrderItemsLocns.DataSource = Session("_VT_LocationsGrid")
                'End If
                'If Session("_VT_ItemDetails") IsNot Nothing Then
                '    wdgDetails.DataSource = Session("_VT_ItemDetails")
                'End If
                'If Session("_VT_DetailsProducts") IsNot Nothing Then
                '    wdgDetailsProducts.DataSource = Session("_VT_DetailsProducts")
                'End If
                'If Session("_VT_DetailsLocations") IsNot Nothing Then
                '    wdgDetailsLocations.DataSource = Session("_VT_DetailsLocations")
                'End If
            Else
                Me.CurrentSession.VT_SelectedFulfilOrderItemsGridRow_V2 = Nothing
            End If
            If Session("VT_ReturnPage") <> "" And Not Session("VT_ReturnPage") Is Nothing Then
                ReSelectItemInGrid()
                Session("VT_ReturnPage") = ""
            End If
        Else
            'This is a postback so rebind the grids
            If Session("_VT_OrdersGrid") IsNot Nothing Then
                wdgOrderItems.datasource = Session("_VT_OrdersGrid")
            End If
            If Session("_VT_ProductGrid") IsNot Nothing Then
                wdgOrderItemsProds.datasource = Session("_VT_ProductGrid")
            End If
            If Session("_VT_LocationsGrid") IsNot Nothing Then
                wdgOrderItemsLocns.datasource = Session("_VT_LocationsGrid")
            End If
            If Session("_VT_ItemDetails") IsNot Nothing Then
                wdgDetails.DataSource = Session("_VT_ItemDetails")
            End If
            If Session("_VT_DetailsProducts") IsNot Nothing Then
                wdgDetailsProducts.DataSource = Session("_VT_DetailsProducts")
            End If
            If Session("_VT_DetailsLocations") IsNot Nothing Then
                wdgDetailsLocations.DataSource = Session("_VT_DetailsLocations")
            End If


        End If

    End Sub


    Protected Sub wdgOrderItems_ActiveCellChanged(sender As Object, e As Infragistics.Web.UI.GridControls.ActiveCellEventArgs) Handles wdgOrderItems.ActiveCellChanged

        Dim objC As New BPADotNetCommonFunctions.VT_CommonFunctions.CommonFunctions

        'Serialise the Select DataRow
        Dim intActiveRowIndex As Integer = wdgOrderItems.Behaviors.Activation.ActiveCell.Row.Index

        Me.CurrentSession.VT_SelectedFulfilOrderItemsGridRow_V2 = objC.SerialiseWebDataGridRow(wdgOrderItems, intActiveRowIndex)

        wdgOrderItems.Behaviors.Activation.ActiveCell = wdgOrderItems.Rows(intActiveRowIndex).Items(1)
        Me.CurrentSession.VT_SelectedFulfillDetailsGridRow_V2 = Nothing

        wdgOrderItemsClicked(Me.CurrentSession.VT_SelectedFulfilOrderItemsGridRow_V2)

    End Sub

    Protected Sub wdgOrderItems_RowSelectionChanged(sender As Object, e As Infragistics.Web.UI.GridControls.SelectedRowEventArgs) Handles wdgOrderItems.RowSelectionChanged

        Dim objC As New BPADotNetCommonFunctions.VT_CommonFunctions.CommonFunctions

        'Serialise the Select DataRow
        Dim RowSelected As Infragistics.Web.UI.GridControls.SelectedRowCollection = wdgOrderItems.Behaviors.Selection.SelectedRows
        Dim intActiveRowIndex As Integer = RowSelected(0).Index

        Me.CurrentSession.VT_SelectedFulfilOrderItemsGridRow_V2 = objC.SerialiseWebDataGridRow(wdgOrderItems, intActiveRowIndex)
        Me.CurrentSession.VT_SelectedFulfillDetailsGridRow_V2 = Nothing
        wdgOrderItemsClicked(Me.CurrentSession.VT_SelectedFulfilOrderItemsGridRow_V2)

    End Sub

    Sub wdgOrderItemsClicked(ByVal dtSelectedRow As DataTable)

        'need to fill the session variables needed if a new item is to be added to the order
        Dim dt As New DataTable
        Dim objDBAccess As New SalesOrdersFunctions.SalesOrders
        Dim strOrderNumber As String
        Dim dtOrderDetails As New DataTable
        Dim objBPA As New VTDBFunctions.VTDBFunctions.TelesalesFunctions

        Select Case UCase(ddlMainViewType.SelectedValue)
            Case "ORDER"
                'strOrderNumber = uwgOrderItems.DisplayLayout.ActiveRow.Cells.FromKey("OrderNumber").Text
                strOrderNumber = dtSelectedRow.Rows(0).Item("OrderNumber")

                dtOrderDetails = objBPA.GetOrderForNum(CLng(strOrderNumber))

                dt = objDBAccess.getOrderDetailsFromHHForOrderNum(strOrderNumber)
                If dt.Rows.Count > 0 Then
                    Me.CurrentSession.VT_CustomerID = dt.Rows(0).Item("CustomerId")
                    Session("_VT_CustomerID") = dt.Rows(0).Item("CustomerId")
                    Me.CurrentSession.VT_OrderPO = dt.Rows(0).Item("PONumber")
                    Me.CurrentSession.VT_SalesOrderNum = dt.Rows(0).Item("OrderNumber")
                    Me.CurrentSession.VT_DeliveryCustomerID = dt.Rows(0).Item("DeliveryCustomerID")
                    Me.CurrentSession.VT_OrderDate = dt.Rows(0).Item("OrderDate")
                    Me.CurrentSession.VT_SalesOrderID = objDBAccess.GetTSOrderIDForOrderNum(strOrderNumber)
                    lblComment.Text = Trim(dtOrderDetails.Rows(0).Item("Comment"))
                End If
                FillDetailsGridForOrdersView()
                lblComment.Visible = True
                lblConmmentTitle.Visible = True

                SetGridHeaders(ddlMainViewType.SelectedValue)

            Case "PRODUCT"
                FillDetailsGridForProductView()
                SetGridHeaders(ddlMainViewType.SelectedValue)

            Case "LOCATION"
                FillDetailsGridForLocationsView()
                SetGridHeaders(ddlMainViewType.SelectedValue)

        End Select


        UpdatePanel1.Update()

    End Sub


    Protected Sub wdgOrderItemsLocns_ActiveCellChanged(sender As Object, e As Infragistics.Web.UI.GridControls.ActiveCellEventArgs) Handles wdgOrderItemsLocns.ActiveCellChanged

        Dim objC As New BPADotNetCommonFunctions.VT_CommonFunctions.CommonFunctions

        'Serialise the Select DataRow
        Dim intActiveRowIndex As Integer = wdgOrderItemsLocns.Behaviors.Activation.ActiveCell.Row.Index

        Me.CurrentSession.VT_SelectedItemLocnsGridRow_V2 = objC.SerialiseWebDataGridRow(wdgOrderItemsLocns, intActiveRowIndex)

        wdgOrderItemsLocns.Behaviors.Activation.ActiveCell = wdgOrderItemsLocns.Rows(intActiveRowIndex).Items(1)
        Me.CurrentSession.VT_SelectedFulfillDetailsGridRow_V2 = Nothing
        wdgOrderItemsLocnsClicked(Me.CurrentSession.VT_SelectedItemLocnsGridRow_V2)

    End Sub

    Protected Sub wdgOrderItemsLocns_RowSelectionChanged(sender As Object, e As Infragistics.Web.UI.GridControls.SelectedRowEventArgs) Handles wdgOrderItemsLocns.RowSelectionChanged

        Dim objC As New BPADotNetCommonFunctions.VT_CommonFunctions.CommonFunctions

        'Serialise the Select DataRow
        Dim RowSelected As Infragistics.Web.UI.GridControls.SelectedRowCollection = wdgOrderItemsLocns.Behaviors.Selection.SelectedRows
        Dim intActiveRowIndex As Integer = RowSelected(0).Index
        Me.CurrentSession.VT_SelectedItemLocnsGridRow_V2 = objC.SerialiseWebDataGridRow(wdgOrderItemsLocns, intActiveRowIndex)
        Me.CurrentSession.VT_SelectedFulfillDetailsGridRow_V2 = Nothing
        wdgOrderItemsLocnsClicked(Me.CurrentSession.VT_SelectedItemLocnsGridRow_V2)

    End Sub

    Sub wdgOrderItemsLocnsClicked(ByVal dtSelectedRow As DataTable)
        'need to fill the session variables needed if a new item is to be added to the order
        Dim dt As New DataTable
        Dim objDBAccess As New SalesOrdersFunctions.SalesOrders

        Select Case UCase(ddlMainViewType.SelectedValue)

            Case "LOCATION"

                lblComment.Visible = False
                lblConmmentTitle.Visible = False

                FillDetailsGridForLocationsView()
                SetGridHeaders(ddlMainViewType.SelectedValue)

        End Select

        UpdatePanel1.Update()
    End Sub


    Protected Sub wdgOrderItemsProds_ActiveCellChanged(sender As Object, e As Infragistics.Web.UI.GridControls.ActiveCellEventArgs) Handles wdgOrderItemsProds.ActiveCellChanged
        Dim objC As New BPADotNetCommonFunctions.VT_CommonFunctions.CommonFunctions

        'Serialise the Select DataRow
        Dim intActiveRowIndex As Integer = wdgOrderItemsProds.Behaviors.Activation.ActiveCell.Row.Index

        Me.CurrentSession.VT_SelectedItemProdsGridRow_V2 = objC.SerialiseWebDataGridRow(wdgOrderItemsProds, intActiveRowIndex)

        wdgOrderItemsProds.Behaviors.Activation.ActiveCell = wdgOrderItemsProds.Rows(intActiveRowIndex).Items(1)
        Me.CurrentSession.VT_SelectedFulfillDetailsGridRow_V2 = Nothing
        wdgOrderItemsProdsClicked(Me.CurrentSession.VT_SelectedItemProdsGridRow_V2)

    End Sub

    Protected Sub wdgOrderItemsProds_RowSelectionChanged(sender As Object, e As Infragistics.Web.UI.GridControls.SelectedRowEventArgs) Handles wdgOrderItemsProds.RowSelectionChanged

        Dim objC As New BPADotNetCommonFunctions.VT_CommonFunctions.CommonFunctions

        'Serialise the Select DataRow
        Dim RowSelected As Infragistics.Web.UI.GridControls.SelectedRowCollection = wdgOrderItemsProds.Behaviors.Selection.SelectedRows
        Dim intActiveRowIndex As Integer = RowSelected(0).Index

        Me.CurrentSession.VT_SelectedItemProdsGridRow_V2 = objC.SerialiseWebDataGridRow(wdgOrderItemsProds, intActiveRowIndex)
        Me.CurrentSession.VT_SelectedFulfillDetailsGridRow_V2 = Nothing
        wdgOrderItemsProdsClicked(Me.CurrentSession.VT_SelectedItemProdsGridRow_V2)

    End Sub

    Sub wdgOrderItemsProdsClicked(ByVal dtSelectedRow As DataTable)
        'need to fill the session variables needed if a new item is to be added to the order
        Dim dt As New DataTable
        Dim objDBAccess As New SalesOrdersFunctions.SalesOrders

        Select Case UCase(ddlMainViewType.SelectedValue)

            Case "PRODUCT"

                FillDetailsGridForProductView()
                lblComment.Visible = False
                lblConmmentTitle.Visible = False

                SetGridHeaders(ddlMainViewType.SelectedValue)

        End Select

        UpdatePanel1.Update()

    End Sub


    Protected Sub wdgDetails_ActiveCellChanged(sender As Object, e As Infragistics.Web.UI.GridControls.ActiveCellEventArgs) Handles wdgDetails.ActiveCellChanged
        Dim objC As New BPADotNetCommonFunctions.VT_CommonFunctions.CommonFunctions

        'Serialise the Select DataRow
        Dim intActiveRowIndex As Integer = wdgDetails.Behaviors.Activation.ActiveCell.Row.Index

        Me.CurrentSession.VT_SelectedFulfillDetailsGridRow_V2 = objC.SerialiseWebDataGridRow(wdgDetails, intActiveRowIndex)

        wdgDetails.Behaviors.Activation.ActiveCell = wdgDetails.Rows(intActiveRowIndex).Items(1)



        wdgDetailsClicked(Me.CurrentSession.VT_SelectedFulfillDetailsGridRow_V2)

    End Sub

    Protected Sub wdgDetails_RowSelectionChanged(sender As Object, e As Infragistics.Web.UI.GridControls.SelectedRowEventArgs) Handles wdgDetails.RowSelectionChanged
        Dim objC As New BPADotNetCommonFunctions.VT_CommonFunctions.CommonFunctions

        'Serialise the Select DataRow
        Dim RowSelected As Infragistics.Web.UI.GridControls.SelectedRowCollection = wdgDetails.Behaviors.Selection.SelectedRows
        Dim intActiveRowIndex As Integer = RowSelected(0).Index

        Me.CurrentSession.VT_SelectedFulfillDetailsGridRow_V2 = objC.SerialiseWebDataGridRow(wdgDetails, intActiveRowIndex)


        wdgDetailsClicked(Me.CurrentSession.VT_SelectedFulfillDetailsGridRow_V2)

    End Sub

    Sub wdgDetailsClicked(ByRef dtSelectedRow As DataTable)

        Dim objDB As New ProductsFunctions.Products
        Dim lngRecordId As Long
        Dim intUoS As Integer

        With dtSelectedRow.Rows(0)
            Dim lngProductId As Long = objDB.GetProductIDForCode(.Item("ProductCode"))
            lngRecordId = CDbl(IIf(IsNumeric(.Item("RecordId")), .Item("RecordId"), 0))

            intUoS = objDB.GetUnitOfSale(lngProductId)

            Me.CurrentSession.VT_SelectedOrderRecordId = lngRecordId
            Me.CurrentSession.VT_ProdSelectProdID = lngProductId
        End With

        'Set the unit price for the select panels
        'Check Price against the original price stored in lookup table.
        Dim dblUnitPrice, dblTotalPrice As Double
        Dim dtOrderItemRecord As New DataTable
        Dim dblQuantity, dblOrderedQty, dblQtyOrWeight, dblOrderedWgt As Double
        Dim objSales As New SalesOrdersFunctions.SalesOrders

        dtOrderItemRecord = objSales.GetRecordsForIds(CStr(lngRecordId))

        With dtOrderItemRecord.Rows(0)
            'Equate values here, using protectors
            If Not IsDBNull(.Item("PriceSoldFor")) Then
                If IsNumeric(.Item("PriceSoldFor")) = False Then
                    If IsNumeric(Replace(.Item("PriceSoldFor"), ".", ",")) = True Then
                        dblTotalPrice = CDbl(Replace(.Item("PriceSoldFor"), ".", ","))
                    End If
                Else
                    dblTotalPrice = CDbl(.Item("PriceSoldFor"))
                End If

            Else
                dblTotalPrice = 0
            End If
            If Not IsDBNull(.Item("Quantity")) Then
                dblQuantity = .Item("Quantity")
            Else
                dblQuantity = 0
            End If
            If Not IsDBNull(.Item("OrderedQty")) Then
                dblOrderedQty = .Item("OrderedQty")
            Else
                dblOrderedQty = 0
            End If
            If Not IsDBNull(.Item("QtyOrWeight")) Then
                dblQtyOrWeight = .Item("QtyOrWeight")
            Else
                dblQtyOrWeight = 0
            End If
            If Not IsDBNull(.Item("OrderedWgt")) Then
                dblOrderedWgt = .Item("OrderedWgt")
            Else
                dblOrderedWgt = 0
            End If

            If intUoS = 1 Then 'unit pricing
                'if the qty is 0 then use the ordered qty to calculate the price
                'ortherwise use the qty as this will be the price that is listed

                If dblQuantity > 0 Then
                    If dblQuantity > 0 Then
                        dblUnitPrice = dblTotalPrice / dblQuantity
                    Else
                        dblUnitPrice = dblTotalPrice
                    End If
                Else
                    If dblOrderedQty > 0 Then
                        dblUnitPrice = dblTotalPrice / dblOrderedQty
                    Else
                        dblUnitPrice = dblTotalPrice
                    End If
                End If
            Else
                'if the qty is 0 then use the ordered qty to calculate the price
                'ortherwise use the qty as this will be the price that is listed
                If dblQuantity > 0 Then
                    If dblQtyOrWeight > 0 Then
                        dblUnitPrice = dblTotalPrice / dblQtyOrWeight
                    Else
                        dblUnitPrice = dblTotalPrice
                    End If
                Else
                    If dblOrderedWgt > 0 Then
                        dblUnitPrice = dblTotalPrice / dblOrderedWgt
                    Else
                        dblUnitPrice = dblTotalPrice
                    End If
                End If
            End If
        End With
        dblUnitPrice = Math.Round(dblUnitPrice, 2)


        Me.CurrentSession.VT_SelectedUnitPrice = dblUnitPrice

        FillEditPanel()

        UpdatePanel1.Update()

    End Sub


    Protected Sub wdgDetailsProducts_ActiveCellChanged(sender As Object, e As Infragistics.Web.UI.GridControls.ActiveCellEventArgs) Handles wdgDetailsProducts.ActiveCellChanged
        Dim objC As New BPADotNetCommonFunctions.VT_CommonFunctions.CommonFunctions

        'Serialise the Select DataRow
        Dim intActiveRowIndex As Integer = wdgOrderItemsProds.Behaviors.Activation.ActiveCell.Row.Index

        Me.CurrentSession.VT_SelectedFulfillDetailsProdsGridRow_V2 = objC.SerialiseWebDataGridRow(wdgDetailsProducts, intActiveRowIndex)

        wdgDetailsProducts.Behaviors.Activation.ActiveCell = wdgDetailsProducts.Rows(intActiveRowIndex).Items(1)


        wdgDetailsProductsClicked(Me.CurrentSession.VT_SelectedFulfillDetailsProdsGridRow_V2)

    End Sub

    Protected Sub wdgDetailsProducts_DataBound(sender As Object, e As System.EventArgs) Handles wdgDetailsProducts.DataBound

    End Sub

    Protected Sub wdgDetailsProducts_RowSelectionChanged(sender As Object, e As Infragistics.Web.UI.GridControls.SelectedRowEventArgs) Handles wdgDetailsProducts.RowSelectionChanged
        Dim objC As New BPADotNetCommonFunctions.VT_CommonFunctions.CommonFunctions

        'Serialise the Select DataRow
        Dim RowSelected As Infragistics.Web.UI.GridControls.SelectedRowCollection = wdgDetails.Behaviors.Selection.SelectedRows
        Dim intActiveRowIndex As Integer = RowSelected(0).Index

        Me.CurrentSession.VT_SelectedFulfillDetailsProdsGridRow_V2 = objC.SerialiseWebDataGridRow(wdgDetailsProducts, intActiveRowIndex)

        wdgDetailsProductsClicked(Me.CurrentSession.VT_SelectedFulfillDetailsProdsGridRow_V2)

    End Sub

    Sub wdgDetailsProductsClicked(ByVal dtSelectedRow As DataTable)
        Dim objDB As New ProductsFunctions.Products
        Dim lngRecordId As Long

        With dtSelectedRow.Rows(0)
            Dim lngId As Long = objDB.GetProductIDForCode(.Item("ProductCode"))
            lngRecordId = CDbl(IIf(IsNumeric(.Item("RecordId")), .Item("RecordId"), 0))

            Me.CurrentSession.VT_SelectedOrderRecordId = lngRecordId
            Me.CurrentSession.VT_ProdSelectProdID = lngId
        End With

        FillEditPanelFromDetailsProducts()

        UpdatePanel1.Update()
    End Sub


    Protected Sub wdgDetailsLocations_ActiveCellChanged(sender As Object, e As Infragistics.Web.UI.GridControls.ActiveCellEventArgs) Handles wdgDetailsLocations.ActiveCellChanged
        Dim objC As New BPADotNetCommonFunctions.VT_CommonFunctions.CommonFunctions

        'Serialise the Select DataRow
        Dim intActiveRowIndex As Integer = wdgDetailsLocations.Behaviors.Activation.ActiveCell.Row.Index

        Me.CurrentSession.VT_SelectedFulfillDetailsLocnsGridRow_V2 = objC.SerialiseWebDataGridRow(wdgDetailsLocations, intActiveRowIndex)


        wdgDetailsLocationsClicked(Me.CurrentSession.VT_SelectedFulfillDetailsLocnsGridRow_V2)

    End Sub

    Protected Sub wdgDetailsLocations_RowAdding(sender As Object, e As Infragistics.Web.UI.GridControls.RowAddingEventArgs) Handles wdgDetailsLocations.RowAdding

    End Sub

    Protected Sub wdgDetailsLocations_RowSelectionChanged(sender As Object, e As Infragistics.Web.UI.GridControls.SelectedRowEventArgs) Handles wdgDetailsLocations.RowSelectionChanged
        Dim objC As New BPADotNetCommonFunctions.VT_CommonFunctions.CommonFunctions

        'Serialise the Select DataRow
        Dim RowSelected As Infragistics.Web.UI.GridControls.SelectedRowCollection = wdgDetailsLocations.Behaviors.Selection.SelectedRows
        Dim intActiveRowIndex As Integer = RowSelected(0).Index

        Me.CurrentSession.VT_SelectedFulfillDetailsLocnsGridRow_V2 = objC.SerialiseWebDataGridRow(wdgDetailsLocations, intActiveRowIndex)

        wdgDetailsLocations.Behaviors.Activation.ActiveCell = wdgDetailsLocations.Rows(intActiveRowIndex).Items(1)

        wdgDetailsLocationsClicked(Me.CurrentSession.VT_SelectedFulfillDetailsLocnsGridRow_V2)

    End Sub

    Sub wdgDetailsLocationsClicked(ByVal dtSelectedRow As DataTable)

        Dim objDB As New ProductsFunctions.Products
        Dim lngRecordId As Long

        With dtSelectedRow.Rows(0)
            Dim lngId As Long = objDB.GetProductIDForCode(.Item("ProductCode"))
            lngRecordId = CDbl(IIf(IsNumeric(.Item("RecordId")), .Item("RecordId"), 0))

            Me.CurrentSession.VT_SelectedOrderRecordId = lngRecordId
            Me.CurrentSession.VT_ProdSelectProdID = lngId
        End With

        FillEditPanelFromDetailsProducts()

        UpdatePanel1.Update()
    End Sub


    Protected Sub Page_PreRender(sender As Object, e As EventArgs) Handles Me.PreRender

        'If Session("_VT_OrdersGrid") IsNot Nothing Then
        '    wdgOrderItems.DataSource = Session("_VT_OrdersGrid")
        'End If
        'If Session("_VT_ProductGrid") IsNot Nothing Then
        '    wdgOrderItemsProds.DataSource = Session("_VT_ProductGrid")
        'End If
        'If Session("_VT_LocationsGrid") IsNot Nothing Then
        '    wdgOrderItemsLocns.DataSource = Session("_VT_LocationsGrid")
        'End If
        'If Session("_VT_ItemDetails") IsNot Nothing Then
        '    wdgOrderItems.DataSource = Session("_VT_ItemDetails")
        'End If


        'UpdatePanel1.Update()


    End Sub



    Sub FillGridForOrdersView()
        Dim objDB As New SalesOrdersFunctions.SalesOrders
        Dim dt As New DataTable
        Dim strIsPublished As String
        Dim strAllowNonHHPicking As String
        Dim objCommonFuncs As New BPADotNetCommonFunctions.VT_CommonFunctions.CommonFunctions

        strIsPublished = UCase(objCommonFuncs.GetConfigItem("IsPublished"))
        strAllowNonHHPicking = UCase(objCommonFuncs.GetConfigItem("AllowNonHHPicking"))

        ' At this point either Handhelds are not in use so we can display all the OrderItems
        ' or Handhelds are in use but NonHHPicking is allowed so we must show only the PCOnly order items
        Dim blnOnlyShowPCItems As Boolean
        If (strIsPublished = "YES" And strAllowNonHHPicking = "YES") Then
            blnOnlyShowPCItems = True
        Else
            blnOnlyShowPCItems = False
        End If


        dt = objDB.GetTodaysOrderItemsByOrder(blnOnlyShowPCItems)

        wdgOrderItems.Visible = True
        wdgOrderItemsProds.Visible = False
        wdgOrderItemsLocns.Visible = False

        btnAddNewBatch.Visible = True
        btnConfirmOrder.Visible = True
        btnEditOrderItem.Visible = True
        btnAddNewItem.Visible = True

        wdgDetails.Visible = True
        wdgDetailsProducts.Visible = False
        wdgDetailsLocations.Visible = False


        'Add Addtional fields required to the Datatable
        dt.Columns.Add("DocketStatus")
        'Bind the datatable
        Session("_VT_OrdersGrid") = dt
        wdgOrderItems.DataSource = Session("_VT_OrdersGrid")
        wdgOrderItems.DataBind()



    End Sub

    Sub FillDetailsGridForOrdersView()
        Dim objDB As New SalesOrdersFunctions.SalesOrders
        Dim dt As New DataTable
        Dim strOrderNumber As String
        Dim objProd As New VTDBFunctions.VTDBFunctions.ProductsFunctions
        Dim objTrace As New VTDBFunctions.VTDBFunctions.TraceFunctions
        Dim lngProductId As Long

        'strOrderNumber = uwgOrderItems.DisplayLayout.ActiveRow.Cells.FromKey("OrderNumber").Text

        strOrderNumber = Me.CurrentSession.VT_SelectedFulfilOrderItemsGridRow_V2.Rows(0).Item("OrderNumber")

        dt = objDB.GetDetailsGridItemsByOrder(strOrderNumber)

        'Add the required columns to the DataTable
        dt.Columns.Add("ProductName")
        dt.Columns.Add("LocationName")
        dt.Columns.Add("SaleType")
        dt.Columns.Add("VATCharged")

        'Fill these values in the datatable
        For intCnt As Integer = 0 To dt.Rows.Count - 1
            lngProductId = objProd.GetProductIdForCode(dt.Rows(intCnt).Item("ProductId"))

            dt.Rows(intCnt).Item("ProductName") = Trim(objProd.GetProductNameForId(lngProductId))
            dt.Rows(intCnt).Item("LocationName") = Trim(objTrace.GetLocationTextForId(dt.Rows(intCnt).Item("Location")))
            dt.Rows(intCnt).Item("SaleType") = objProd.GetUnitOfSale(lngProductId)
        Next
        'sort the datatable by product code so that all batches of the same product appear together


        Session("_VT_ItemDetails") = dt
        wdgDetails.DataSource = Session("_VT_ItemDetails")
        wdgDetails.DataBind()


    End Sub


    Sub FillGridForProductView()
        Dim objDB As New SalesOrdersFunctions.SalesOrders
        Dim dt As New DataTable

        dt = objDB.GetTodaysOrderItemsByProduct

        btnAddNewBatch.Visible = False
        btnConfirmOrder.Visible = False
        btnEditOrderItem.Visible = True
        btnAddNewItem.Visible = False

        wdgOrderItems.Visible = False
        wdgOrderItemsProds.Visible = True
        wdgOrderItemsLocns.Visible = False

        wdgDetails.Visible = False
        wdgDetailsProducts.Visible = True
        wdgDetailsLocations.Visible = False

        Session("_VT_ProductGrid") = dt
        wdgOrderItemsProds.DataSource = Session("_VT_ProductGrid")
        wdgOrderItemsProds.DataBind()


    End Sub

    Sub FillDetailsGridForProductView()
        Dim objDB As New SalesOrdersFunctions.SalesOrders
        Dim dt As New DataTable
        Dim strProductCode As String
        Dim lngProductId As Long
        Dim objProd As New VTDBFunctions.VTDBFunctions.ProductsFunctions

        strProductCode = Me.CurrentSession.VT_SelectedItemProdsGridRow_V2.Rows(0).Item("ProductCode")

        lngProductId = objProd.GetProductIdForCode(strProductCode)

        dt = objDB.GetDetailsGridItemsByProduct(strProductCode)

        'Add additional columns required
        dt.Columns.Add("SaleType")
        For Each dr As DataRow In dt.Rows
            dr.Item("CustomerName") = Trim(dr.Item("CustomerName"))
            dr.Item("TraceCode") = Trim(dr.Item("TraceCode"))

            dr.Item("SaleType") = objProd.GetUnitOfSale(lngProductId)

        Next

        Session("_VT_DetailsProducts") = dt
        wdgDetailsProducts.DataSource = Session("_VT_DetailsProducts")
        wdgDetailsProducts.DataBind()

        If Me.CurrentSession.VT_UserSerialNumbers = False Then
            wdgDetailsProducts.Columns.FromKey("SerialNum").Hidden = True
            wdgDetailsProducts.Columns.FromKey("Barcode").Hidden = True
        Else
            wdgDetailsProducts.Columns.FromKey("SerialNum").Hidden = False
            wdgDetailsProducts.Columns.FromKey("Barcode").Hidden = False
        End If


    End Sub


    Sub FillGridForLocationsView()
        Dim objDB As New SalesOrdersFunctions.SalesOrders
        Dim dt As New DataTable

        dt = objDB.GetTodaysOrderItemsByLocation

        wdgOrderItems.Visible = False
        wdgOrderItemsProds.Visible = False
        wdgOrderItemsLocns.Visible = True

        btnAddNewBatch.Visible = False
        btnConfirmOrder.Visible = False
        btnEditOrderItem.Visible = True
        btnAddNewItem.Visible = False

        wdgDetails.Visible = False
        wdgDetailsProducts.Visible = False
        wdgDetailsLocations.Visible = True

        Session("_VT_LocationsGrid") = dt
        wdgOrderItemsLocns.DataSource = Session("_VT_LocationsGrid")
        wdgOrderItemsLocns.DataBind()

    End Sub

    Sub FillDetailsGridForLocationsView()
        Dim objDB As New SalesOrdersFunctions.SalesOrders
        Dim dt As New DataTable
        Dim strProductCode As String
        Dim strLocation As String
        Dim objProd As New VTDBFunctions.VTDBFunctions.ProductsFunctions
        Dim objTrace As New VTDBFunctions.VTDBFunctions.TraceFunctions
        Dim lngProductId As Long

        strProductCode = Me.CurrentSession.VT_SelectedItemLocnsGridRow_V2.Rows(0).Item("ProductCode")
        strLocation = Me.CurrentSession.VT_SelectedItemLocnsGridRow_V2.Rows(0).Item("Location")

        lngProductId = objProd.GetProductIdForCode(strProductCode)

        dt = objDB.GetDetailsGridItemsByLocation(strLocation, strProductCode)

        'Add additional columns required
        dt.Columns.Add("SaleType")
        For Each dr As DataRow In dt.Rows
            dr.Item("CustomerName") = Trim(dr.Item("CustomerName"))
            dr.Item("TraceCode") = Trim(dr.Item("TraceCode"))

            dr.Item("SaleType") = objProd.GetUnitOfSale(lngProductId)

        Next

        Session("_VT_DetailsLocations") = dt
        wdgDetailsLocations.DataSource = Session("_VT_DetailsLocations")
        wdgDetailsLocations.DataBind()

        If Me.CurrentSession.VT_UserSerialNumbers = False Then
            wdgDetailsLocations.Columns.FromKey("SerialNum").Hidden = True
            wdgDetailsLocations.Columns.FromKey("Barcode").Hidden = True
        Else
            wdgDetailsLocations.Columns.FromKey("SerialNum").Hidden = False
            wdgDetailsLocations.Columns.FromKey("Barcode").Hidden = False
        End If

    End Sub


    Sub FillAddNewPanel()
        Dim objDB As New ProductsFunctions.Products
        Dim dblOutstandingQty As Double
        Dim dblOutstandingWgt As Double
        Dim dblOrdered As Double
        Dim dblFulfilledQty As Double
        Dim dblFulfilledWgt As Double
        Dim lngProductId As Long
        Dim intUnitOfSale As Integer
        Dim objProd As New VTDBFunctions.VTDBFunctions.ProductsFunctions

        txtpnlTraceCode_NS.Text = ""
        txtPnlWgtToPick.Text = ""
        txtPnlQtyToPick.Text = ""
        txtPnlNewBatchWgt.Text = ""
        txtPnlNewBatchQty.Text = ""

        With Me.CurrentSession.VT_SelectedFulfillDetailsGridRow_V2.Rows(0)

            lblPnlProductCodeEdit_NS.Text = .Item("ProductCode")
            lblPnlProductNameEdit.Text = objDB.GetProductNameForCode(.Item("ProductCode"))

            dblOrdered = CDbl(IIf(IsNumeric(.Item("OrderedWgt")), .Item("OrderedWgt"), 0))
            dblFulfilledWgt = CDbl(IIf(IsNumeric(.Item("Weight")), .Item("Weight"), 0))
            dblOutstandingWgt = dblOrdered - dblFulfilledWgt

            dblOrdered = CDbl(IIf(IsNumeric(.Item("OrderedQty")), .Item("OrderedQty"), 0))
            dblFulfilledQty = CDbl(IIf(IsNumeric(.Item("Quantity")), .Item("Quantity"), 0))
            dblOutstandingQty = dblOrdered - dblFulfilledQty

            lblPnlOrderedWgtEdit.Text = .Item("OrderedWgt")
            lblPnlOrderedQtyEdit.Text = .Item("OrderedQty")
            lblPnlOutstandingQtyEdit.Text = dblOutstandingQty
            lblPnlOutstandingWgtEdit.Text = dblOutstandingWgt
            lblPnlDeliveredQtyEdit_NS.Text = dblFulfilledQty
            lblPnlDeliveredWgtEdit_NS.Text = dblFulfilledWgt
            txtpnlTraceCode_NS.Text = .Item("TraceCode")
        End With

        lngProductId = objDB.GetProductIDForCode(Trim(lblPnlProductCodeEdit_NS.Text))
        hdnPnlAddProductId.Value = lngProductId
        intUnitOfSale = objDB.GetUnitOfSale(lngProductId)
        hdnUnitOfSale.Value = intUnitOfSale

        lblPerUnitWeight.Value = objProd.GetProductAvgWeightPerUnit(lngProductId)


        If intUnitOfSale = 1 Then
            lblPnlOrderedWgtEdit.Visible = False
            lblPnlOrderedWgtLabelEdit_NS.Visible = False
            lblPnlOutstandingWgtEdit.Visible = False
            lblPnlOutstandingWgtLabelEdit_NS.Visible = False
            txtPnlNewBatchWgt.Visible = False
            lblPnlNewBatchWgtLabel_NS.Visible = False
        Else
            lblPnlOrderedWgtEdit.Visible = True
            lblPnlOrderedWgtLabelEdit_NS.Visible = True
            lblPnlOutstandingWgtEdit.Visible = True
            lblPnlOutstandingWgtLabelEdit_NS.Visible = True
            txtPnlNewBatchWgt.Visible = True
            lblPnlNewBatchWgtLabel_NS.Visible = True
        End If

    End Sub

    Sub FillEditPanel()
        Dim objDB As New ProductsFunctions.Products
        Dim dblOutstandingQty As Double
        Dim dblOutstandingWgt As Double
        Dim dblOrdered As Double
        Dim dblFulfilled As Double
        Dim lngProductId As Long
        Dim intUnitOfSale As Integer
        Dim objProd As New VTDBFunctions.VTDBFunctions.ProductsFunctions

        'Key="ProductCode"
        'Key="ProductName"
        'Key="OrderedQty"
        'Key="OrderedWgt"
        'Key="Quantity"
        'Key="Weight"
        'Key="TraceCode"
        'Key="PriceSoldFor"

        txtpnlTraceCode_NS.Text = ""
        txtPnlWgtToPick.Text = ""
        txtPnlQtyToPick.Text = ""

        With Me.CurrentSession.VT_SelectedFulfillDetailsGridRow_V2.Rows(0)

            lblPnlProductCode_NS.Text = .Item("ProductCode")
            lblPnlProductName_NS.Text = .Item("ProductName") '  objDB.GetProductNameForCode(.FromKey("ProductCode").GetText)

            dblOrdered = CDbl(IIf(IsNumeric(.Item("OrderedWgt")), .Item("OrderedWgt"), 0))
            dblFulfilled = CDbl(IIf(IsNumeric(.Item("Weight")), .Item("Weight"), 0))
            dblOutstandingWgt = dblOrdered - dblFulfilled

            dblOrdered = CDbl(IIf(IsNumeric(.Item("OrderedQty")), .Item("OrderedQty"), 0))
            dblFulfilled = CDbl(IIf(IsNumeric(.Item("Quantity")), .Item("Quantity"), 0))
            dblOutstandingQty = dblOrdered - dblFulfilled

            lblPnlOrderedWgt_NS.Text = .Item("OrderedWgt")
            lblPnlOrderedQty_NS.Text = .Item("OrderedQty")
            lblPnlOutstandingQty_NS.Text = dblOutstandingQty
            lblPnlOutstandingWgt_NS.Text = dblOutstandingWgt
            txtpnlTraceCode_NS.Text = .Item("TraceCode")
            hdnTotalPrice.Value = CDbl(IIf(IsNumeric(.Item("PriceSoldFor")), .Item("PriceSoldFor"), 0))
            hdnTotalVAT.Value = CDbl(IIf(IsNumeric(.Item("VATCharged")), .Item("VATCharged"), 0))

        End With

        lngProductId = objDB.GetProductIDForCode(Trim(lblPnlProductCode_NS.Text))
        hdnPnlProductId.Value = lngProductId
        intUnitOfSale = objDB.GetUnitOfSale(lngProductId)
        hdnUnitOfSale.Value = intUnitOfSale

        lblPerUnitWeight.Value = objProd.GetProductAvgWeightPerUnit(lngProductId)

        If intUnitOfSale = 1 Then
            lblPnlOrderedWgt_NS.Visible = False
            lblPnlOrderedWgtLabel_NS.Visible = False
            lblPnlOutstandingWgt_NS.Visible = False
            lblPnlOutstandingWgtLabel_NS.Visible = False
            txtPnlWgtToPick.Visible = False
            lblPnlWgtToPickLabel_NS.Visible = False
        Else
            lblPnlOrderedWgt_NS.Visible = True
            lblPnlOrderedWgtLabel_NS.Visible = True
            lblPnlOutstandingWgt_NS.Visible = True
            lblPnlOutstandingWgtLabel_NS.Visible = True
            txtPnlWgtToPick.Visible = True
            lblPnlWgtToPickLabel_NS.Visible = True
        End If

    End Sub

    Sub FillEditPanelFromDetailsProducts()
        Dim objDB As New ProductsFunctions.Products
        Dim dblOutstandingQty As Double
        Dim dblOutstandingWgt As Double
        Dim dblOrdered As Double
        Dim dblFulfilled As Double
        Dim lngProductId As Long
        Dim intUnitOfSale As Integer
        Dim objProd As New VTDBFunctions.VTDBFunctions.ProductsFunctions

        'Key="ProductCode"
        'Key="ProductName"
        'Key="OrderedQty"
        'Key="OrderedWgt"
        'Key="Quantity"
        'Key="Weight"
        'Key="TraceCode"
        'Key="PriceSoldFor"

        With Me.CurrentSession.VT_SelectedFulfillDetailsProdsGridRow_V2.Rows(0)
            lblPnlProductCode_NS.Text = .Item("ProductCode").GetText
            lblPnlProductName_NS.Text = objDB.GetProductNameForCode(.Item("ProductCode"))

            dblOrdered = CDbl(IIf(IsNumeric(.Item("OrderedWgt")), .Item("OrderedWgt"), 0))
            dblFulfilled = CDbl(IIf(IsNumeric(.Item("Weight")), .Item("Weight"), 0))
            dblOutstandingWgt = dblOrdered - dblFulfilled

            dblOrdered = CDbl(IIf(IsNumeric(.Item("OrderedQty")), .Item("OrderedQty"), 0))
            dblFulfilled = CDbl(IIf(IsNumeric(.Item("Quantity")), .Item("Quantity"), 0))
            dblOutstandingQty = dblOrdered - dblFulfilled

            lblPnlOrderedWgt_NS.Text = .Item("OrderedWgt")
            lblPnlOrderedQty_NS.Text = .Item("OrderedQty")
            lblPnlOutstandingQty_NS.Text = dblOutstandingQty
            lblPnlOutstandingWgt_NS.Text = dblOutstandingWgt
            txtpnlTraceCode_NS.Text = .Item("TraceCode")

        End With

        lngProductId = objDB.GetProductIDForCode(Trim(lblPnlProductCode_NS.Text))
        hdnPnlProductId.Value = lngProductId
        intUnitOfSale = objDB.GetUnitOfSale(lngProductId)
        hdnUnitOfSale.Value = intUnitOfSale

        lblPerUnitWeight.Value = objProd.GetProductAvgWeightPerUnit(lngProductId)

        If intUnitOfSale = 1 Then
            lblPnlOrderedWgt_NS.Visible = False
            lblPnlOrderedWgtLabel_NS.Visible = False
            lblPnlOutstandingWgt_NS.Visible = False
            lblPnlOutstandingWgtLabel_NS.Visible = False
            txtPnlWgtToPick.Visible = False
            lblPnlWgtToPickLabel_NS.Visible = False
        Else
            lblPnlOrderedWgt_NS.Visible = True
            lblPnlOrderedWgtLabel_NS.Visible = True
            lblPnlOutstandingWgt_NS.Visible = True
            lblPnlOutstandingWgtLabel_NS.Visible = True
            txtPnlWgtToPick.Visible = True
            lblPnlWgtToPickLabel_NS.Visible = True
        End If

    End Sub

    Sub SetGridHeaders(ByVal strType As String)

        Select Case UCase(strType)
            Case "ORDER"
                lblTypeSelectedHeader.Text = "Sales Orders:"
                lblTypeSelectedSubHead1.Text = "( these are sales orders that are awaiting fulfilment )"
                If Me.CurrentSession.VT_SelectedFulfilOrderItemsGridRow_V2 IsNot Nothing Then
                    lblTypeDetailsHeader.Text = "Items in selected Order: "
                    lblDetailsItemSubHeader.Text = "[ " & Me.CurrentSession.VT_SelectedFulfilOrderItemsGridRow_V2.Rows(0).Item("OrderNumber") & " - " & Me.CurrentSession.VT_SelectedFulfilOrderItemsGridRow_V2.Rows(0).Item("CustomerName") & " ]"
                Else
                    lblTypeDetailsHeader.Text = "Please select a Sales Order in the grid above "
                    lblDetailsItemSubHeader.Text = ""
                End If

            Case "PRODUCT"
                lblTypeSelectedHeader.Text = "Products on Orders:"
                lblTypeSelectedSubHead1.Text = "( these are all the Products that require picking on orders awaiting fulfilment )"
                If Me.CurrentSession.VT_SelectedItemProdsGridRow_V2 IsNot Nothing Then
                    lblTypeDetailsHeader.Text = "Sales Orders that have the selected product: "
                    lblDetailsItemSubHeader.Text = "[ " & Me.CurrentSession.VT_SelectedItemProdsGridRow_V2.Rows(0).Item("ProductCode") & " - " & Me.CurrentSession.VT_SelectedItemProdsGridRow_V2.Rows(0).Item("ProductName") & " ]"
                Else
                    lblTypeDetailsHeader.Text = "Please select a Product code in the grid above "
                    lblDetailsItemSubHeader.Text = ""
                End If

            Case "LOCATION"
                lblTypeSelectedHeader.Text = "Locations on Orders:"
                lblTypeSelectedSubHead1.Text = "( these are all the Locations where products should be picked from, on orders awaiting fulfilment )"
                If Me.CurrentSession.VT_SelectedItemLocnsGridRow_V2 IsNot Nothing Then
                    lblTypeDetailsHeader.Text = "Sales Orders that have the selected product: "
                    lblDetailsItemSubHeader.Text = "[ " & Me.CurrentSession.VT_SelectedItemLocnsGridRow_V2.Rows(0).Item("ProductCode") & " - " & Me.CurrentSession.VT_SelectedItemLocnsGridRow_V2.Rows(0).Item("ProductName") & " ]"
                Else
                    lblTypeDetailsHeader.Text = "Please select a Location in the grid above "
                    lblDetailsItemSubHeader.Text = ""
                End If

        End Select

    End Sub

    Protected Sub cmdOKConfirm_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles cmdOKConfirm.Click
        Dim lngRecordId As Long
        Dim dblFulfillQty As Double
        Dim dblFulfillWgt As Double
        Dim strTraceCode As String
        Dim lngLocationId As Long
        Dim strSerial As String
        Dim strBarcode As String
        Dim strPriceSoldFor As String
        Dim strVAT As String
        Dim dblQtyORWeightComplete As String


        Dim objDB As New SalesOrdersFunctions.SalesOrders
        Dim objCommonFuncs As New BPADotNetCommonFunctions.VT_CommonFunctions.CommonFunctions
        Dim strSerialNums As String = objCommonFuncs.GetConfigItem("SerialNums")

        'If Not uwgDetails.DisplayLayout.ActiveRow Is Nothing Then
        '    lngRecordId = CDbl(IIf(IsNumeric(uwgDetails.DisplayLayout.ActiveRow.Cells.FromKey("RecordId").GetText), uwgDetails.DisplayLayout.ActiveRow.Cells.FromKey("RecordId").GetText, 0))
        lngRecordId = Me.CurrentSession.VT_SelectedOrderRecordId
        dblFulfillQty = CDbl(IIf(IsNumeric(txtPnlQtyToPick.Text), txtPnlQtyToPick.Text, 0))
        dblFulfillWgt = CDbl(IIf(IsNumeric(txtPnlWgtToPick.Text), txtPnlWgtToPick.Text, 0))
        strTraceCode = Trim(txtpnlTraceCode_NS.Text)
        lngLocationId = ddlLocationEdit.SelectedValue
        If UCase(strSerialNums) = "YES" Then
            Dim dsSerial As New DataSet
            Dim objTraceData As New VTDBFunctions.VTDBFunctions.TraceDataFunctions
            Dim objTrace As New VTDBFunctions.VTDBFunctions.TraceFunctions
            Dim lngTraceCodeId As Long

            strSerial = Trim(txtPnlSerialNum.Text)
            lngTraceCodeId = objTrace.GetTraceCodeIDForDescAndProduct(hdnPnlProductId.Value, strTraceCode)
            dsSerial = objTraceData.GetDetailsForSerialNumAndTraceCodeId(strSerial, lngTraceCodeId)
            If dsSerial.Tables(0).Rows.Count > 0 Then
                strBarcode = dsSerial.Tables(0).Rows(0).Item("Barcode")
            Else
                '   strSerial = 0
                strBarcode = ""
            End If
        Else
            strSerial = ""
            strBarcode = ""
        End If


        'If the transaction sell by type is different from the product track by 

        Select Case hdnUnitOfSale.Value
            Case 1
                dblQtyORWeightComplete = dblFulfillQty

            Case Else 'undefined default to by weight
                dblQtyORWeightComplete = dblFulfillWgt

        End Select


        'Check Price against the original price stored in lookup table.
        Dim dblUnitPrice As Double
        Dim dblUnitVAT As Double
        Dim dtOrderItemRecord As New DataTable
        Dim dblTotalPrice As Double
        Dim dblTotalVAT As Double
        Dim dsSOItemDetails As New DataSet
        Dim objTelesales As New VTDBFunctions.VTDBFunctions.TelesalesFunctions
        Dim blnUnitPriceExists As Boolean
        Dim lngOrderItemId As Long



        dtOrderItemRecord = objDB.GetRecordsForIds(CStr(lngRecordId))

        lngOrderItemId = 0

        With dtOrderItemRecord.Rows(0)
            dblTotalPrice = .Item("PriceSoldFor")
            dblTotalVAT = .Item("VATCharged")

            blnUnitPriceExists = False

            lngOrderItemId = dtOrderItemRecord.Rows(0).Item("ItemNumber")

            If dtOrderItemRecord.Rows(0).Item("ItemNumber") > 0 Then
                dsSOItemDetails = objTelesales.GetOrderItemForId(dtOrderItemRecord.Rows(0).Item("ItemNumber"))
                If dsSOItemDetails.Tables(0).Rows.Count > 0 Then
                    blnUnitPriceExists = True

                    dblUnitPrice = dsSOItemDetails.Tables(0).Rows(0).Item("UnitPrice")
                    If hdnUnitOfSale.Value = 1 Then 'unit pricing
                        'if the qty is 0 then use the ordered qty to calculate the price
                        'ortherwise use the qty as this will be the price that is listed

                        If CDbl(dsSOItemDetails.Tables(0).Rows(0).Item("Quantity")) > 0 Then
                            dblUnitVAT = CDbl(dsSOItemDetails.Tables(0).Rows(0).Item("VAT")) / CDbl(dsSOItemDetails.Tables(0).Rows(0).Item("Quantity"))
                        Else
                            dblUnitVAT = CDbl(dsSOItemDetails.Tables(0).Rows(0).Item("VAT")) / CDbl(dsSOItemDetails.Tables(0).Rows(0).Item("QuantityRequested"))
                        End If
                    Else
                        'if the wgt is 0 then use the ordered wgt to calculate the price
                        'ortherwise use the wgt as this will be the price that is listed
                        If CDbl(dsSOItemDetails.Tables(0).Rows(0).Item("Weight")) > 0 Then
                            dblUnitVAT = CDbl(dsSOItemDetails.Tables(0).Rows(0).Item("VAT")) / CDbl(dsSOItemDetails.Tables(0).Rows(0).Item("Weight"))
                        Else
                            dblUnitVAT = CDbl(dsSOItemDetails.Tables(0).Rows(0).Item("VAT")) / CDbl(dsSOItemDetails.Tables(0).Rows(0).Item("WeightRequested"))
                        End If
                    End If
                End If
            End If

            If blnUnitPriceExists = False Then

                If hdnUnitOfSale.Value = 1 Then 'unit pricing
                    'if the qty is 0 then use the ordered qty to calculate the price
                    'ortherwise use the qty as this will be the price that is listed

                    If CDbl(.Item("Quantity")) > 0 Then
                        dblUnitPrice = CDbl(.Item("PriceSoldFor")) / CDbl(.Item("Quantity"))
                        dblUnitVAT = CDbl(.Item("VATCharged")) / CDbl(.Item("Quantity"))
                    Else
                        dblUnitPrice = CDbl(.Item("PriceSoldFor")) / CDbl(.Item("OrderedQty"))
                        dblUnitVAT = CDbl(.Item("VATCharged")) / CDbl(.Item("OrderedQty"))
                    End If
                Else
                    'if the wgt is 0 then use the ordered wgt to calculate the price
                    'ortherwise use the wgt as this will be the price that is listed
                    If CDbl(.Item("QtyOrWeight")) > 0 Then
                        dblUnitPrice = CDbl(.Item("PriceSoldFor")) / CDbl(.Item("QtyOrWeight"))
                        dblUnitVAT = CDbl(.Item("VATCharged")) / CDbl(.Item("QtyOrWeight"))
                    Else
                        dblUnitPrice = CDbl(.Item("PriceSoldFor")) / CDbl(.Item("OrderedWgt"))
                        dblUnitVAT = CDbl(.Item("VATCharged")) / CDbl(.Item("OrderedWgt"))
                    End If
                End If
            End If


        End With
        dblUnitPrice = Math.Round(dblUnitPrice, Me.CurrentSession.VT_DecimalPlaces)

        If dblQtyORWeightComplete <> 0 Then
            dblTotalPrice = dblQtyORWeightComplete * dblUnitPrice
            dblTotalVAT = dblQtyORWeightComplete * dblUnitVAT

            dblTotalPrice = Math.Round(dblTotalPrice, Me.CurrentSession.VT_DecimalPlaces, System.MidpointRounding.AwayFromZero)
            dblTotalVAT = Math.Round(dblTotalVAT, Me.CurrentSession.VT_DecimalPlaces, System.MidpointRounding.AwayFromZero)

        Else
            dblTotalPrice = Math.Round(dblTotalPrice, Me.CurrentSession.VT_DecimalPlaces, System.MidpointRounding.AwayFromZero)
            dblTotalVAT = Math.Round(dblTotalVAT, Me.CurrentSession.VT_DecimalPlaces, System.MidpointRounding.AwayFromZero)
        End If

        strPriceSoldFor = CStr(dblTotalPrice)
        strVAT = CStr(dblTotalVAT)


        objDB.SaveOrderItemChanges(lngRecordId, strTraceCode, dblFulfillQty, dblFulfillWgt, lngLocationId, strPriceSoldFor, strVAT, 1, strSerial, strBarcode)

        Dim strTeleDebugLog As String = objCommonFuncs.GetConfigItem("TelesalesDebugLog")
        If UCase(strTeleDebugLog) = "ON" Then
            Dim strLog As String
            Dim f As New BPADotNetCommonFunctions.VT_CommonFunctions.CommonFunctions
            strLog = "update batch:  Order: " & dtOrderItemRecord.Rows(0).Item("OrderNumber") & " UnitPrice: " & CStr(dblUnitPrice) & " Amount: " & CStr(dblQtyORWeightComplete) & " Total: " & strPriceSoldFor & " Batch Code: " & strTraceCode & " ProductCode:" & Trim(dtOrderItemRecord.Rows(0).Item("ProductId")) & " OrderItemId:" & CStr(lngOrderItemId)
            f.DebugLog(0, strLog, "Sales Portal", PortalFunctions.Now)
        End If

        Select Case UCase(ddlMainViewType.SelectedValue)
            Case "ORDER"

                FillDetailsGridForOrdersView()
            Case "PRODUCT"

                FillDetailsGridForProductView()
            Case "LOCATION"

                FillDetailsGridForLocationsView()
        End Select


    End Sub

    Protected Sub btnViewProductDetails_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnViewProductDetails.Click

        Dim blnProductSelected As Boolean

        blnProductSelected = False

        Select Case UCase(ddlMainViewType.SelectedValue)
            Case "ORDER"
                If IsNothing(Me.CurrentSession.VT_SelectedFulfilOrderItemsGridRow_V2) Then
                    blnProductSelected = False
                Else
                    blnProductSelected = True
                End If
            Case "PRODUCT"
                If IsNothing(Me.CurrentSession.VT_SelectedItemProdsGridRow_V2) Then
                    blnProductSelected = False
                Else
                    blnProductSelected = True
                End If

            Case "LOCATION"
                If IsNothing(Me.CurrentSession.VT_SelectedItemLocnsGridRow_V2) Then
                    blnProductSelected = False
                Else
                    blnProductSelected = True
                End If

        End Select

        If blnProductSelected = True Then

            Dim lngId As Long
            Dim objProd As New VTDBFunctions.VTDBFunctions.ProductsFunctions

            Select Case UCase(ddlMainViewType.SelectedValue)
                Case "ORDER"
                    lngId = objProd.GetProductIdForCode(Me.CurrentSession.VT_SelectedFulfillDetailsGridRow_V2.Rows(0).Item("ProductCode"))

                Case "PRODUCT"
                    ' lngId = objProd.GetProductIdForCode(Me.CurrentSession.VT_SelectedItemProdsGridRow_V2.Rows(0).Item("ProductCode"))
                    lngId = objProd.GetProductIdForCode(Me.CurrentSession.VT_SelectedFulfillDetailsProdsGridRow_V2.Rows(0).Item("ProductCode"))


                Case "LOCATION"
                    ' lngId = objProd.GetProductIdForCode(Me.CurrentSession.VT_SelectedItemLocnsGridRow_V2.Rows(0).Item("ProductCode"))
                    lngId = objProd.GetProductIdForCode(Me.CurrentSession.VT_SelectedFulfillDetailsLocnsGridRow_V2.Rows(0).Item("ProductCode"))
            End Select

            Me.CurrentSession.VT_ProdSelectProdID = lngId

            Session("VT_FromPage") = "Fulfill_Opening"

            Response.Redirect("~/Other_Pages/ProductStockDetails.aspx")
        End If
    End Sub

    Function CheckAllowedToEdit() As Boolean
        Dim blnAllowed As Boolean

        blnAllowed = False

        CheckAllowedToEdit = blnAllowed

    End Function


    Protected Sub cmdOKConfirmEdit_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles cmdOKConfirmEdit.Click
        'add a new item to the tcd_tblOrderItems table. It should have an item id of 0 so that
        'the system can recognise that it has been added at this stage. 
        If IsNumeric(txtPnlNewBatchQty.Text) = True Or IsNumeric(txtPnlNewBatchWgt.Text) = True Then
            Dim objtracedata As New VTDBFunctions.VTDBFunctions.TraceDataFunctions
            Dim strProductCode As String
            Dim dblWeight As Double = 0
            Dim dblQty As Double = 0
            Dim strTraceCode As String
            Dim lngRecordId As Long
            Dim strSerial As String
            Dim strBarcode As String
            Dim lngLocationId As Long
            Dim strPriceSoldFor As String
            Dim strVAT As String
            Dim dblQtyORWeightComplete As String
            Dim dblUnitPrice As Double
            Dim dblUnitVAT As Double
            Dim dtOrderItemRecord As New DataTable
            Dim dblTotalPrice As Double
            Dim dblTotalVAT As Double
            Dim objDB As New SalesOrdersFunctions.SalesOrders

            Dim objCommonFuncs As New BPADotNetCommonFunctions.VT_CommonFunctions.CommonFunctions
            Dim strSerialNums As String = objCommonFuncs.GetConfigItem("SerialNums")

            If IsNumeric(txtPnlNewBatchQty.Text) Then
                dblQty = CDbl(txtPnlNewBatchQty.Text)

            End If

            If IsNumeric(txtPnlNewBatchWgt.Text) Then
                dblWeight = txtPnlNewBatchWgt.Text
            End If

            strProductCode = Trim(lblPnlProductCodeEdit_NS.Text)


            lngRecordId = Me.CurrentSession.VT_SelectedOrderRecordId

            strTraceCode = Trim(txtPnlNewBatchTrace.Text)

            lngLocationId = ddlNewBatchLocationEdit.SelectedValue


            If UCase(strSerialNums) = "YES" Then
                Dim dsSerial As New DataSet

                Dim objTrace As New VTDBFunctions.VTDBFunctions.TraceFunctions
                Dim lngTraceCodeId As Long

                strSerial = Trim(txtPnlNewBatchSerialNum.Text)
                lngTraceCodeId = objTrace.GetTraceCodeIDForDescAndProduct(hdnPnlAddProductId.Value, strTraceCode)
                dsSerial = objtracedata.GetDetailsForSerialNumAndTraceCodeId(strSerial, lngTraceCodeId)
                If dsSerial.Tables(0).Rows.Count > 0 Then
                    strBarcode = dsSerial.Tables(0).Rows(0).Item("Barcode")
                Else
                    '   strSerial = ""
                    strBarcode = ""
                End If
            Else
                strSerial = ""
                strBarcode = ""
            End If

            Select Case hdnUnitOfSale.Value
                Case 1
                    dblQtyORWeightComplete = dblQty

                Case Else 'undefined default to by weight
                    dblQtyORWeightComplete = dblWeight

            End Select

            Dim dsSOItemDetails As New DataSet
            Dim objTelesales As New VTDBFunctions.VTDBFunctions.TelesalesFunctions
            Dim blnUnitPriceExists As Boolean
            Dim lngOrderItemId As Long

            'Check Price against the original price stored in lookup table.
            dtOrderItemRecord = objDB.GetRecordsForIds(CStr(lngRecordId))

            lngOrderItemId = 0

            With dtOrderItemRecord.Rows(0)
                With dtOrderItemRecord.Rows(0)
                    dblTotalPrice = .Item("PriceSoldFor")
                    dblTotalVAT = .Item("VATCharged")

                    blnUnitPriceExists = False
                    lngOrderItemId = dtOrderItemRecord.Rows(0).Item("ItemNumber")

                    If dtOrderItemRecord.Rows(0).Item("ItemNumber") > 0 Then
                        dsSOItemDetails = objTelesales.GetOrderItemForId(dtOrderItemRecord.Rows(0).Item("ItemNumber"))
                        If dsSOItemDetails.Tables(0).Rows.Count > 0 Then
                            blnUnitPriceExists = True

                            dblUnitPrice = dsSOItemDetails.Tables(0).Rows(0).Item("UnitPrice")
                            If hdnUnitOfSale.Value = 1 Then 'unit pricing
                                'if the qty is 0 then use the ordered qty to calculate the price
                                'ortherwise use the qty as this will be the price that is listed

                                If CDbl(dsSOItemDetails.Tables(0).Rows(0).Item("Quantity")) > 0 Then
                                    dblUnitVAT = CDbl(dsSOItemDetails.Tables(0).Rows(0).Item("VAT")) / CDbl(dsSOItemDetails.Tables(0).Rows(0).Item("Quantity"))
                                Else
                                    dblUnitVAT = CDbl(dsSOItemDetails.Tables(0).Rows(0).Item("VAT")) / CDbl(dsSOItemDetails.Tables(0).Rows(0).Item("QuantityRequested"))
                                End If
                            Else
                                'if the wgt is 0 then use the ordered wgt to calculate the price
                                'ortherwise use the wgt as this will be the price that is listed
                                If CDbl(dsSOItemDetails.Tables(0).Rows(0).Item("Weight")) > 0 Then
                                    dblUnitVAT = CDbl(dsSOItemDetails.Tables(0).Rows(0).Item("VAT")) / CDbl(dsSOItemDetails.Tables(0).Rows(0).Item("Weight"))
                                Else
                                    dblUnitVAT = CDbl(dsSOItemDetails.Tables(0).Rows(0).Item("VAT")) / CDbl(dsSOItemDetails.Tables(0).Rows(0).Item("WeightRequested"))
                                End If
                            End If
                        End If
                    End If

                    If blnUnitPriceExists = False Then

                        If hdnUnitOfSale.Value = 1 Then 'unit pricing
                            'if the qty is 0 then use the ordered qty to calculate the price
                            'ortherwise use the qty as this will be the price that is listed

                            If CDbl(.Item("Quantity")) > 0 Then
                                dblUnitPrice = CDbl(.Item("PriceSoldFor")) / CDbl(.Item("Quantity"))
                                dblUnitVAT = CDbl(.Item("VATCharged")) / CDbl(.Item("Quantity"))
                            Else
                                dblUnitPrice = CDbl(.Item("PriceSoldFor")) / CDbl(.Item("OrderedQty"))
                                dblUnitVAT = CDbl(.Item("VATCharged")) / CDbl(.Item("OrderedQty"))
                            End If
                        Else
                            'if the wgt is 0 then use the ordered wgt to calculate the price
                            'ortherwise use the wgt as this will be the price that is listed
                            If CDbl(.Item("QtyOrWeight")) > 0 Then
                                dblUnitPrice = CDbl(.Item("PriceSoldFor")) / CDbl(.Item("QtyOrWeight"))
                                dblUnitVAT = CDbl(.Item("VATCharged")) / CDbl(.Item("QtyOrWeight"))
                            Else
                                dblUnitPrice = CDbl(.Item("PriceSoldFor")) / CDbl(.Item("OrderedWgt"))
                                dblUnitVAT = CDbl(.Item("VATCharged")) / CDbl(.Item("OrderedWgt"))
                            End If
                        End If
                    End If


                End With
            End With
            dblUnitPrice = Math.Round(dblUnitPrice, 2)

            If dblQtyORWeightComplete <> 0 Then
                dblTotalPrice = dblQtyORWeightComplete * dblUnitPrice
                dblTotalVAT = dblQtyORWeightComplete * dblUnitVAT

                dblTotalPrice = Math.Round(dblTotalPrice, 2, System.MidpointRounding.AwayFromZero)
                dblTotalVAT = Math.Round(dblTotalVAT, 2, System.MidpointRounding.AwayFromZero)

            Else
                dblTotalPrice = Math.Round(dblTotalPrice, 2, System.MidpointRounding.AwayFromZero)
                dblTotalVAT = Math.Round(dblTotalVAT, 2, System.MidpointRounding.AwayFromZero)
            End If

            strPriceSoldFor = CStr(dblTotalPrice)
            strVAT = CStr(dblTotalVAT)

            With dtOrderItemRecord.Rows(0)

                objtracedata.InsertSOToTrace(.Item("PONumber"), .Item("OrderDate"), .Item("CustomerID"), 0, strProductCode, 0, 0, _
                                       False, .Item("OrderNumber"), 0, 0, 6, strBarcode, 0, 1, strTraceCode, dblTotalVAT, dblTotalPrice, .Item("DeliveryCustomerID"), lngLocationId, _
                                       .Item("ChargedByType"), strSerial, strBarcode, CInt(dblQty), dblWeight)

                Dim strTeleDebugLog As String = objCommonFuncs.GetConfigItem("TelesalesDebugLog")

                If UCase(strTeleDebugLog) = "ON" Then
                    Dim strLog As String
                    Dim f As New BPADotNetCommonFunctions.VT_CommonFunctions.CommonFunctions
                    strLog = "add new batch:  Order: " & .Item("OrderNumber") & " UnitPrice: " & CStr(dblUnitPrice) & " Amount: " & CStr(dblQtyORWeightComplete) & " Total: " & strPriceSoldFor & " Batch Code: " & strTraceCode & " ProductCode:" & Trim(strProductCode) & " OrderItemId:" & CStr(lngOrderItemId)
                    f.DebugLog(0, strLog, "Sales Portal", PortalFunctions.Now)
                End If

            End With
        End If

        '''''''**************************************************

        'If blnPCOnly = True Then
        '    objTraceData.MarkSOTcdItemAsPCOnly(lngSalesOrderItemId, blnPCOnly)
        'End If


        '''*******************************************



        Select Case UCase(ddlMainViewType.SelectedValue)
            Case "ORDER"

                FillDetailsGridForOrdersView()
            Case "PRODUCT"

                FillDetailsGridForProductView()
            Case "LOCATION"

                FillDetailsGridForLocationsView()
        End Select
        ' End If


        ''''''**************************************************************





    End Sub


    Protected Sub ddlMainViewType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlMainViewType.SelectedIndexChanged
        Select Case UCase(ddlMainViewType.SelectedValue)
            Case "ORDER"
                If wdgDetails.DataSource IsNot Nothing Then
                    wdgDetails.ClearDataSource()
                End If

                FillGridForOrdersView()
                lblComment.Visible = True
                lblConmmentTitle.Visible = True

                SetGridHeaders(ddlMainViewType.SelectedValue)

            Case "PRODUCT"
                If wdgDetailsProducts.DataSource IsNot Nothing Then
                    wdgDetailsProducts.ClearDataSource()
                End If

                FillGridForProductView()
                lblComment.Visible = False
                lblConmmentTitle.Visible = False

                SetGridHeaders(ddlMainViewType.SelectedValue)

            Case "LOCATION"
                If wdgDetailsLocations.DataSource IsNot Nothing Then
                    wdgDetailsLocations.ClearDataSource()
                End If

                FillGridForLocationsView()
                lblComment.Visible = False
                lblConmmentTitle.Visible = False

                SetGridHeaders(ddlMainViewType.SelectedValue)

        End Select

        UpdatePanel1.Update()

    End Sub


    Protected Sub plnEditOrder_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles plnAddItem.Load
        'this is the add new item panel

    End Sub

    Protected Sub Panel3_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Panel3.Load
        'this is the edit item panel

    End Sub

    Protected Sub btnEditOrderItem_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnEditOrderItem.Click
        If Me.CurrentSession.VT_SelectedFulfillDetailsGridRow_V2 IsNot Nothing Then
            FillEditPanel()
            ModalPopupExtender1.Show()
        Else
            ' Master.ShowMessagePanel("You must select an item to Edit!")
            lblMsg.Text = "You must select an item to Edit!"
            ModalPopupExtenderMsg.Show()

        End If
    End Sub

    Protected Sub btnConfirmOrder_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnConfirmOrder.Click
        Dim lngSalesOrderId, lngSalesOrderItemId As Long
        Dim intTraceCodeId, intSalesOrderNumber As Integer
        Dim dsOrder As New DataSet
        Dim dsOrderItem As New DataSet
        Dim objTele As New VTDBFunctions.VTDBFunctions.TelesalesFunctions
        Dim strOrderStatus, strOrderItemStatus, strAuditComment As String
        Dim objDisp As New VT_Display.DisplayFuncs
        Dim objForms As New VT_Forms.Forms
        Dim objTraceDbFunc As New VTDBFunctions.VTTraceDBAccess.VTTraceDBAccess
        Dim dblQtyWeight As Double
        Dim lngProduct As Long
        Dim objProduct As New VTDBFunctions.VTDBFunctions.ProductsFunctions

        lngSalesOrderId = Me.CurrentSession.VT_SalesOrderID
        Session("CurrentSalesOrderId") = lngSalesOrderId

        ' get the details of the original Sales Order
        dsOrder = objTele.GetOrderForId(lngSalesOrderId)
        strOrderStatus = Trim(dsOrder.Tables(0).Rows(0).Item("Status"))
        If strOrderStatus = TelesalesFunctions.cOrderClosedPartiallyComplete Or strOrderStatus = TelesalesFunctions.cOrderComplete Then
            'objDisp.DisplayMessage(Page, "This order is already closed!")
            lblMsg.Text = "This order is already closed!"
            ModalPopupExtenderMsg.Show()

            Exit Sub
        End If

        Dim objdb As New SalesOrdersFunctions.SalesOrders
        Dim objProducts As New ProductsFunctions.Products
        Dim strTraceCode As String
        Dim dblQty As Double
        Dim dblWgt As Double
        Dim dtOrderItem As DataTable
        Dim lngCustId As Long
        Dim strCustName As String
        Dim objCust As New VTDBFunctions.VTDBFunctions.CustomerFunctions
        Dim strPriceCharged As String
        Dim PONum As String
        Dim intSaleType As Integer
        Dim strErrorMsg As String
        Dim intLocationId As Integer
        Dim intUOM As Integer
        Dim dblVat As Double
        Dim dblUnitVAT As Double
        Dim lngDeliveryCustId As Long
        Dim strDocketNum As String
        Dim dblDocketNum As Double
        Dim lngUserId As Long
        Dim lngRecordId As Long
        Dim strSerial As String
        Dim strBarcode As String
        Dim dblTotalPrice As Double
        Dim dblUnitPrice As Double
        Dim objCommonFuncs As New BPADotNetCommonFunctions.VT_CommonFunctions.CommonFunctions
        Dim objHH As New VTDBFunctions.VTDBFunctions.Handheld
        Dim dteTxnDateTime As DateTime

        Dim strSerialNums As String = objCommonFuncs.GetConfigItem("SerialNums")
        Dim strDocketNumTag As String = objCommonFuncs.GetConfigItem("DocketNumTag")

        Dim strTemp As String
        strTemp = Format(PortalFunctions.Now.Date, "dd-MM-yyyy")
        strDocketNum = Mid(strTemp, 7, 4) + Mid(strTemp, 4, 2) + Mid(strTemp, 1, 2)
        strTemp = Format(PortalFunctions.Now, "Long Time")
        strDocketNum += Hour(PortalFunctions.Now).ToString.PadLeft(2, "0") + Minute(PortalFunctions.Now).ToString.PadLeft(2, "0") + Second(PortalFunctions.Now).ToString.PadLeft(2, "0")
        dblDocketNum = Math.Round(CDbl(strDocketNum), 0)
        'Add unique tag to docket num to indicate it is from PC
        strDocketNum = Trim(strDocketNum) + Trim(strDocketNumTag)

        dteTxnDateTime = PortalFunctions.Now
        Dim MyRow As DataRow
        Dim dblOrderedQty As Double
        Dim dblOrderedWgt As Double
        Dim blnPartFulfilled As Boolean = False


        For Each MyRow In Session("_VT_ItemDetails").rows
            dblQty = MyRow.Item("Quantity")
            dblWgt = MyRow.Item("QtyOrWeight")
            dblOrderedQty = MyRow.Item("OrderedQty")
            dblOrderedWgt = MyRow.Item("OrderedWgt")

            intSaleType = MyRow.Item("SaleType")

            If (intSaleType = 1) Then
                If dblQty <> dblOrderedQty Then
                    blnPartFulfilled = True
                End If
                If dblQty = 0 Then
                    ' objDisp.DisplayMessage(Page, "There are items on the order that are not fulfilled, either fulfill the items or delete the rows.")
                    lblMsg.Text = "There are items on the order that are not fulfilled, either fulfill the items or delete the rows."
                    ModalPopupExtenderMsg.Show()

                    Exit Sub
                End If
            Else
                If dblWgt <> dblOrderedWgt Then
                    blnPartFulfilled = True
                End If
                If dblWgt = 0 Then
                    '  objDisp.DisplayMessage(Page, "There are items on the order that are not fulfilled, either fulfill the items or delete the rows.")
                    lblMsg.Text = "There are items on the order that are not fulfilled, either fulfill the items or delete the rows."
                    ModalPopupExtenderMsg.Show()

                    Exit Sub
                End If
            End If
        Next
        Dim objutil As New VTDBFunctions.VTDBFunctions.UtilFunctions

        For Each MyRow In Session("_VT_ItemDetails").rows

            If Me.CurrentSession.VT_AllowOrderEditBeforeCommit = True Then

                'Leave the order in tcd_tblSalesorders
                'Save the float and string docket numbers
                'update the status 

                lngRecordId = MyRow.Item("RecordId")


                objHH.UpdateHHSODocketNumFloatByRecord(lngRecordId, dblDocketNum)
                objHH.UpdateHHSODocketNumStringByRecord(lngRecordId, strDocketNum)
                objHH.UpdateHHSODocketStatusByRecord(lngRecordId, GetGlobalResourceObject("Resource", "Docket_AwaitingCommitment"))
                objHH.UpdateHHSOTxnDateByRecord(lngRecordId, dteTxnDateTime)

            Else
                ' get the details of the current item
                lngRecordId = MyRow.Item("RecordId")
                lngSalesOrderItemId = objdb.GetItemNumForRecordID(lngRecordId)
                lngProduct = objProduct.GetProductIdForCode(MyRow.Item("ProductID"))
                strTraceCode = MyRow.Item("TraceCode")
                dblQty = MyRow.Item("Quantity")
                dblWgt = MyRow.Item("QtyorWeight")

                dtOrderItem = objdb.GetOrderItemForRecordId(lngRecordId)
                If dtOrderItem.Rows.Count > 0 Then


                    lngCustId = dtOrderItem.Rows(0).Item("CustomerId")
                    strCustName = objCust.GetCustomerNameForId(lngCustId)

                    intSaleType = dtOrderItem.Rows(0).Item("SaleType")
                    PONum = dtOrderItem.Rows(0).Item("PONumber")
                    intLocationId = dtOrderItem.Rows(0).Item("Location")
                    lngDeliveryCustId = dtOrderItem.Rows(0).Item("DeliveryCustomerID")
                    '    strDocketNum = dtOrderItem.Rows(0).Item("DocketNumber")
                    If UCase(strSerialNums) = "YES" Then
                        If IsDBNull(dtOrderItem.Rows(0).Item("SerialNum")) = True Then
                            strSerial = ""
                        Else
                            strSerial = dtOrderItem.Rows(0).Item("SerialNum")
                        End If
                        If IsDBNull(dtOrderItem.Rows(0).Item("Barcode")) = True Then
                            strBarcode = ""
                        Else
                            strBarcode = dtOrderItem.Rows(0).Item("Barcode")
                        End If
                    Else
                        strSerial = ""
                        strBarcode = ""
                    End If
                    lngSalesOrderId = dtOrderItem.Rows(0).Item("OrderNumber")

                    intUOM = objProduct.GetUnitOfSale(lngProduct, "ASP")
                    dblTotalPrice = dtOrderItem.Rows(0).Item("PriceSoldFor")
                    If dblTotalPrice <> 0 Then
                        If intUOM = 1 Then
                            dblUnitPrice = dblTotalPrice / dblQty
                        Else
                            dblUnitPrice = dblTotalPrice / dblWgt
                        End If
                        strPriceCharged = CStr(Math.Round(dblUnitPrice, Me.CurrentSession.VT_DecimalPlaces, MidpointRounding.AwayFromZero))
                    Else
                        strPriceCharged = "0.00"
                    End If

                    If IsNumeric(dtOrderItem.Rows(0).Item("VatCharged")) Then
                        dblVat = dtOrderItem.Rows(0).Item("VatCharged")
                    Else
                        dblVat = 0
                    End If

                    If dblVat <> 0 Then
                        If intUOM = 1 Then
                            dblUnitVAT = dblVat / dblQty
                        Else
                            dblUnitVAT = dblVat / dblWgt
                        End If

                    Else
                        dblUnitVAT = 0
                    End If
                End If



                lngUserId = IIf(IsNumeric(Session("_VT_CurrentUserId")), Session("_VT_CurrentUserId"), 0)
                '     lngSalesOrderId = IIf(IsNumeric(Me.CurrentSession.VT_SalesOrderID), Me.CurrentSession.VT_SalesOrderID, 0)
                objutil.LogAction("InFull opening page")
                Try
                    Dim dblPriceCHarged As Double
                    If IsNumeric(strPriceCharged) Then
                        dblPriceCHarged = CDbl(strPriceCharged)
                    Else
                        dblPriceCHarged = 0
                    End If

                    objTraceDbFunc.FulfillOrder(lngProduct, strTraceCode, "", CLng(dblQty), dblWgt, PortalFunctions.Now, lngCustId, strCustName,
                                                    intSaleType, dblPriceCHarged, strErrorMsg, PONum, CLng(intLocationId), intUOM,
                                                     dblUnitVAT, lngDeliveryCustId, True, strDocketNum, lngUserId, , lngSalesOrderId, lngSalesOrderItemId,
, , , , , strSerial, strBarcode, , , , , True)
                Catch ex As Exception
                    objutil.LogAction(ex.Message)

                End Try
                objutil.LogAction("After Fulfill order")
                Session("CurrentSalesOrderNum") = lngSalesOrderId

                'now delete the item from the tcd_tblSalesOrders table
                objdb.DeleteOrderItemFromTCDTable(lngRecordId)

                'if we are using sage we need to send this to sage also
                Dim objcommon As New SupportFunctions.Support
                Dim objSage As Object
                Dim strClassName As String
                Dim strtxndata As String
                Dim blnIsExchequer As Boolean

                Dim strAccoutType As String = objCommonFuncs.GetConfigItem("InvoiceType")
                Dim straccountsPackage As String = objCommonFuncs.GetConfigItem("AccountsPackage")
                If UCase(strAccoutType) = "EXCHEQUER-UNI" Then
                    blnIsExchequer = True
                Else
                    blnIsExchequer = False
                End If

                If UCase(straccountsPackage) = "QUICKBOOKS" Then
                    blnIsExchequer = True
                End If

                strClassName = objcommon.GetSageUploadDllName
                If strClassName <> "" And blnIsExchequer = False Then
                    objSage = CreateObject(strClassName)
                    strtxndata = Trim(objProducts.GetProductCode(lngProduct)) & "," & PortalFunctions.Now.Date & ",4," &
                        IIf((intUOM = 1), CLng(dblQty), dblWgt) & ",," & strTraceCode & "," & CStr(strPriceCharged)

                    objSage.SendStockMovementToSage(strtxndata)
                End If
            End If
        Next
        Me.CurrentSession.blnOrderStatusChanged = True

        If UCase(objCommonFuncs.GetConfigItem("AskToForceCloseOrder")) = "YES" And blnPartFulfilled Then
            

        Else

            ' objDisp.DisplayMessage(Page, "This order has been Completed!")
            lblMsg.Text = "This order has been completed!"
            ModalPopupExtenderMsg.Show()
        End If
        lblDetailsItemSubHeader.Text = ""

        Select Case UCase(ddlMainViewType.SelectedValue)
            Case "ORDER"
                FillGridForOrdersView()
                If wdgDetails.DataSource IsNot Nothing Then
                    wdgDetails.ClearDataSource()
                End If
                Me.CurrentSession.VT_SelectedFulfillDetailsGridRow_V2 = Nothing
                Me.CurrentSession.VT_SelectedFulfilOrderItemsGridRow_V2 = Nothing

            Case "PRODUCT"
                FillGridForProductView()
                If wdgDetailsProducts.DataSource IsNot Nothing Then
                    wdgDetailsProducts.ClearDataSource()
                End If
                Me.CurrentSession.VT_SelectedFulfillDetailsProdsGridRow_V2 = Nothing
                Me.CurrentSession.VT_SelectedItemProdsGridRow_V2 = Nothing

            Case "LOCATION"
                FillGridForLocationsView()
                If wdgDetailsLocations.DataSource IsNot Nothing Then
                    wdgDetailsLocations.ClearDataSource()
                End If
                Me.CurrentSession.VT_SelectedFulfillDetailsLocnsGridRow_V2 = Nothing
                Me.CurrentSession.VT_SelectedItemLocnsGridRow_V2 = Nothing


        End Select


        UpdatePanel1.Update()
        If UCase(objCommonFuncs.GetConfigItem("AskToForceCloseOrder")) = "YES" And blnPartFulfilled Then
            LblOkCancelMsg.Text = "You are completing an order where the ordered qty/weight does not exactly match the fulfilled qty/weight. Do you want to force this order to status closed? Click ok to close the order part filled or cancel to leave it open -part filled"

            ModalPopupExtender3.Show()

        End If

    End Sub

    Protected Sub btnAddNewBatch_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnAddNewBatch.Click
        If Me.CurrentSession.VT_SelectedFulfillDetailsGridRow_V2 IsNot Nothing Then
            FillAddNewPanel()
            ModalPopupExtender2.Show()

        Else

            'Master.ShowMessagePanel("You must select a Product from the list of products in order to add a new Batch!")
            lblMsg.Text = "You must select a Product from the list of products in order to add a new Batch!"
            ModalPopupExtenderMsg.Show()
        End If
    End Sub

    Protected Sub btnProduceItem_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnProduceItem.Click
        Session("VT_FromPage") = "Fulfill_Opening"
        'save all selected RecordIDs for all selected rows in the grid


        Dim strSelectedRecords As String = ""

        Dim i As Integer

        'For i = 0 To Session("_VT_ItemDetails").rows.count - 1
        '    If uwgDetails.Rows(i).Selected = True Then

        '        strSelectedRecords = strSelectedRecords & uwgDetails.Rows(i).Cells.FromKey("RecordId").GetText & ","

        '    End If
        'Next

        'trim off the last ","
        If Right(strSelectedRecords, 1) = "," Then
            strSelectedRecords = Left(strSelectedRecords, Len(strSelectedRecords) - 1)
        End If

        Session("SelectedRecords") = strSelectedRecords
        Response.Redirect("~/Other_Pages/ProduceItemForOrder.aspx")
    End Sub

    Protected Sub btnDeleteItem_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnDeleteItem.Click
        If Me.CurrentSession.VT_SelectedFulfillDetailsGridRow_V2 IsNot Nothing Then
            Dim objDB As New SalesOrdersFunctions.SalesOrders

            objDB.DeleteOrderItemFromTCDTable(Me.CurrentSession.VT_SelectedOrderRecordId)

            Select Case UCase(ddlMainViewType.SelectedValue)
                Case "ORDER"
                    FillDetailsGridForOrdersView()
                Case "PRODUCT"
                    FillDetailsGridForProductView()
                Case "LOCATION"
                    FillDetailsGridForLocationsView()
            End Select

            UpdatePanel1.Update()
        Else
            lblMsg.Text = "You must select a Product from the list to delete"
            ModalPopupExtenderMsg.Show()
        End If

    End Sub

    Protected Sub btnAddNewItem_Click1(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnAddNewItem.Click
        If Me.CurrentSession.VT_SelectedFulfilOrderItemsGridRow_V2 IsNot Nothing Then
            'Session("VT_FromPage") = "Fulfill_Opening"

            'Response.Redirect("~/Other_Pages/AddProductToDelivery.aspx")
            modalAddProd.Show()

        Else
            '  Master.ShowMessagePanel("You must select an Order from the Orders grid before you can add an item!")
            lblMsg.Text = "You must select an Order from the Orders grid before you can add an item!"
            ModalPopupExtenderMsg.Show()

        End If
    End Sub



    Public Sub PopulateLocationsCombo()
        Dim ds As Data.DataSet
        Dim dt As Data.DataTable
        Dim objProd As New VTDBFunctions.VTDBFunctions.TraceFunctions


        ddlLocationEdit.Items.Clear()

        ds = objProd.GetAllLocations

        dt = ds.Tables(0)
        For Each dr As DataRow In dt.Rows
            dr.Item("LocationText") = Trim(dr.Item("LocationText"))
        Next

        ddlLocationEdit.DataSource = dt
        ddlLocationEdit.DataTextField = "LocationText"
        ddlLocationEdit.DataValueField = "LocationID"
        ddlLocationEdit.DataBind()

        ddlNewBatchLocationEdit.DataSource = dt
        ddlNewBatchLocationEdit.DataTextField = "LocationText"
        ddlNewBatchLocationEdit.DataValueField = "LocationID"
        ddlNewBatchLocationEdit.DataBind()


    End Sub



    Protected Sub cmdProdAdd_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles cmdProdAdd.Click
        Dim objbpa As New VTDBFunctions.VTDBFunctions.ProductsFunctions
        Dim lngProductId As Long
        Dim dblQuantity As Double
        Dim dblWeight As Double
        Dim dblNetPrice As Double
        Dim cell As New Infragistics.WebUI.UltraWebGrid.UltraGridCell
        Dim strTraceCode As String
        Dim dblUnitPrice As Double
        Dim row As New Infragistics.WebUI.UltraWebGrid.UltraGridRow
        Dim objDisp As New VT_Display.DisplayFuncs
        'Dim dblQty As Double
        Dim intUnitOfSale As Integer
        Dim objProducts As New VTDBFunctions.VTDBFunctions.ProductsFunctions
        Dim dsProduct As DataSet
        Dim dblInstock As Double
        Dim dblExtendedPrice As Double
        Dim dblVATRate As Double
        Dim dblVATAmount As Double
        Dim objForms As New VT_Forms.Forms
        Dim dsTraceCodes As DataSet
        Dim strTraceCodeId As String
        Dim strLocationId As String
        Dim objTrace As New TraceFunctions
        Dim ObjTele As New VTDBFunctions.VTDBFunctions.TelesalesFunctions
        Dim objTraceData As New VTDBFunctions.VTDBFunctions.TraceDataFunctions
        Dim objCommonFuncs As New BPADotNetCommonFunctions.VT_CommonFunctions.CommonFunctions
        Dim dsOrderItems As New DataSet
        Dim strQuery As String
        Dim drQueryRows() As DataRow
        Dim objSO As New SalesOrdersFunctions.SalesOrders
        Dim strProductCode As String
        Dim dsRowsToAdd As New DataSet
        Dim strSerialNum As String
        Dim strBarcode As String
        lblMsg.Text = ""


        lngProductId = ddlProducts.SelectedValue

        strProductCode = objProducts.GetProductCode(lngProductId)


        'Check that product is not already in order.
        dsOrderItems = ObjTele.GetOrderItems(Me.CurrentSession.VT_SalesOrderID)

        strQuery = " ProductID = " & CStr(lngProductId)
        drQueryRows = dsOrderItems.Tables(0).Select(strQuery)

        If UBound(drQueryRows) >= 0 Then
            If UBound(drQueryRows) > 0 Or drQueryRows(0).Item("ProductID") = lngProductId Then
                lblMsg.Text = "You cannot add a product that is already in the order, edit the item in the order instead."
                ModalPopupExtenderMsg.Show()
                Exit Sub

            End If
        End If


        Dim blnDontAdd As Boolean


        blnDontAdd = False

        dblQuantity = CDbl(IIf(IsNumeric(txtQuantity.Text), txtQuantity.Text, 0))
        dblWeight = CDbl(IIf(IsNumeric(txtWeight.Text), txtWeight.Text, 0))
        intUnitOfSale = objbpa.GetUnitOfSale(lngProductId)
        If intUnitOfSale = 1 Then  ' by unit
            If dblQuantity = 0 Then
                blnDontAdd = True
            End If
        Else
            If dblWeight = 0 Then
                blnDontAdd = True
            End If
        End If



        If blnDontAdd = False Then
            strLocationId = "0"

            dsProduct = objProducts.GetProductForId(lngProductId)
            If dsProduct.Tables(0).Rows.Count > 0 Then
                intUnitOfSale = objProducts.GetUnitOfSale(lngProductId)

                ' Find available Qty or Weight

                dblInstock = IIf(IsDBNull(dsProduct.Tables(0).Rows(0).Item("InStock")), 0, dsProduct.Tables(0).Rows(0).Item("InStock"))

                Dim strPackage As String

                dblUnitPrice = objProducts.GetProdPackPrice(lngProductId, Me.CurrentSession.VT_CustomerID, strPackage)

                'Check for empty or undefined VAT rate on the product
                If IsDBNull(dsProduct.Tables(0).Rows(0).Item("VAT_Rate")) = True OrElse UCase(Trim(dsProduct.Tables(0).Rows(0).Item("VAT_Rate"))) = "NAN" Then
                    dblVATRate = 0
                Else
                    dblVATRate = dsProduct.Tables(0).Rows(0).Item("VAT_Rate")
                End If


                If intUnitOfSale = 1 Then
                    dblVATAmount = (dblQuantity * dblUnitPrice) * (dblVATRate / 100)
                    dblVATAmount = Math.Round(dblVATAmount, 2, System.MidpointRounding.AwayFromZero)
                    ' reformat VATRate for easier calculation
                    dblVATRate = 1 + (dblVATRate / 100)
                    dblExtendedPrice = dblQuantity * dblUnitPrice * dblVATRate
                    dblExtendedPrice = Math.Round(dblExtendedPrice, 2, System.MidpointRounding.AwayFromZero)
                Else
                    dblVATAmount = (dblWeight * dblUnitPrice) * (dblVATRate / 100)
                    dblVATAmount = Math.Round(dblVATAmount, 2, System.MidpointRounding.AwayFromZero)
                    ' reformat VATRate for easier calculation
                    dblVATRate = 1 + (dblVATRate / 100)
                    dblExtendedPrice = dblWeight * dblUnitPrice * dblVATRate
                    dblExtendedPrice = Math.Round(dblExtendedPrice, 2, System.MidpointRounding.AwayFromZero)
                End If


                ' Check if there is an installation specific DLL and retrieve the ChargedBy Type
                ' for this product and customer
                Dim strCustomerDLL As String = System.Configuration.ConfigurationSettings.AppSettings("InstallationDLL")
                Dim strChargedByType As String
                If strCustomerDLL <> "" Then
                    Dim objCustDLL As TTIInstallationSpecific.Interface
                    objCustDLL = CreateObject(strCustomerDLL)
                    ' If this function doesn't return YES it implies that this Product is not for sale to this Customer
                    strChargedByType = objCustDLL.GetCustomerChargedByType(Session("_VT_BPA.NetConnString"), lngProductId, Me.CurrentSession.VT_CustomerID, PortalFunctions.Now.Date)
                Else
                    strChargedByType = "Use Default"
                End If

                ' Insert available qty or weight Order item 


                If strTraceCodeId = "" Or strTraceCodeId = "0" Then
                    strTraceCodeId = "0"
                    strLocationId = "0"
                End If

                Dim strAllowbackOrders As String
                Dim objDB As New SalesOrdersFunctions.SalesOrders

                strAllowbackOrders = UCase(System.Configuration.ConfigurationSettings.AppSettings("AllowBackOrders"))


                strSerialNum = ""
                strBarcode = ""
                strTraceCode = ""




                objTraceData.InsertSOToTrace(Me.CurrentSession.VT_OrderPO, Me.CurrentSession.VT_OrderDate, Me.CurrentSession.VT_CustomerID, 0, strProductCode, dblWeight, dblQuantity, _
                             False, Me.CurrentSession.VT_SalesOrderNum, 0, 0, 6, strBarcode, 0, 1, strTraceCode, dblVATAmount, dblExtendedPrice - dblVATAmount, _
                              Me.CurrentSession.VT_DeliveryCustomerID, CLng(strLocationId), strChargedByType, strSerialNum, strBarcode, CInt(dblQuantity), dblWeight)


            End If

        End If


        Select Case UCase(ddlMainViewType.SelectedValue)
            Case "ORDER"

                FillDetailsGridForOrdersView()
            Case "PRODUCT"

                FillDetailsGridForProductView()
            Case "LOCATION"

                FillDetailsGridForLocationsView()
        End Select
    End Sub

    Protected Sub cmdMsgOk2_Click(sender As Object, e As ImageClickEventArgs) Handles cmdMsgOk2.Click
        Dim objtele As New VTDBFunctions.VTDBFunctions.TelesalesFunctions
        Dim objForms As New BPADotNetCommonFunctions.VT_Forms.Forms
        Dim intSalesOrderNumber As Integer
        Dim lngSalesOrderId As Long
        Dim strAuditComment As String
        Dim dsOrder As New DataSet

        If IsNumeric(Session("CurrentSalesOrderId")) Then
            lngSalesOrderId = Session("CurrentSalesOrderId")
            intSalesOrderNumber = Session("CurrentSalesOrderNum")
        Else
            Exit Sub
        End If
        ' now close the order
        objtele.CompleteSalesOrder(lngSalesOrderId, objtele.cOrderClosedPartiallyComplete)

        ' Also write the Order status to the Job table.
        objForms.SetJobStatusText(TelesalesFunctions.cOrderClosedPartiallyComplete, intSalesOrderNumber)

        ' Delete the items for this order from the Handheld table
        objtele.DeleteHandheldSalesOrder(intSalesOrderNumber)

        ' write to the AuditLog
        strAuditComment = "Incomplete Order manually Closed"
        objForms.WriteToAuditLog(intSalesOrderNumber, "Telesales", PortalFunctions.Now(), Session("_VT_CurrentUserName"), Session("_VT_CurrentUserId"), strAuditComment, "Audit Record", "")

        lblMsg.Text = "This order has been completed!"
        ModalPopupExtenderMsg.Show()
    End Sub


    Protected Sub cmdMsgCancel2_Click(sender As Object, e As ImageClickEventArgs) Handles cmdMsgCancel2.Click
        lblMsg.Text = "This order has been completed!"
        ModalPopupExtenderMsg.Show()
    End Sub

    Protected Sub btnExport_Click(sender As Object, e As ImageClickEventArgs) Handles btnExport.Click

        Select Case UCase(ddlMainViewType.SelectedValue)
            Case "ORDER"
                VerifyExporter.DownloadName = "FulfillDetails_OrderView"
                VerifyExporter.Export(wdgDetails)
            Case "PRODUCT"
                VerifyExporter.DownloadName = "FulfillDetails_ProductView"
                VerifyExporter.Export(wdgDetailsProducts)
            Case "LOCATION"
                VerifyExporter.DownloadName = "FulfillDetails_LocationView"
                VerifyExporter.Export(wdgDetailsLocations)
        End Select


    End Sub

    'Protected Sub hdnSkidNumber_ValueChanged(sender As Object, e As EventArgs) Handles hdnSkidNumber.ValueChanged

    '    txtSkidNumber.Text = hdnSkidNumber.Value
    '    getSkidInformation(hdnSkidNumber.Value)


    'End Sub
    Sub ReSelectItemInGrid()
        Select Case UCase(ddlMainViewType.SelectedValue)
            Case "ORDER"
                Me.CurrentSession.VT_SelectedFulfillDetailsGridRow_V2 = Nothing

                '  wdgOrderItemsClicked(Me.CurrentSession.VT_SelectedFulfilOrderItemsGridRow_V2)


            Case "PRODUCT"
                Me.CurrentSession.VT_SelectedFulfillDetailsGridRow_V2 = Nothing
                ' wdgOrderItemsProdsClicked(Me.CurrentSession.VT_SelectedItemProdsGridRow_V2)

            Case "LOCATION"
                Me.CurrentSession.VT_SelectedFulfillDetailsGridRow_V2 = Nothing
                '  wdgOrderItemsLocnsClicked(Me.CurrentSession.VT_SelectedItemLocnsGridRow_V2)


        End Select



    End Sub
    Protected Sub btnAddSkid_Click(sender As Object, e As ImageClickEventArgs) Handles btnAddSkid.Click
        If Me.CurrentSession.VT_SelectedFulfillDetailsGridRow_V2 IsNot Nothing Then
            Dim objProd As New VTDBFunctions.VTDBFunctions.ProductsFunctions

            Session("ProductName") = Me.CurrentSession.VT_SelectedFulfillDetailsGridRow_V2.Rows(0).Item("ProductName")
            Session("ProductID") = objProd.GetProductIdForName(Me.CurrentSession.VT_SelectedFulfillDetailsGridRow_V2.Rows(0).Item("ProductName"))
            Session("TraceCodeDesc") = Me.CurrentSession.VT_SelectedFulfillDetailsGridRow_V2.Rows(0).Item("TraceCode")

            Session("VT_ReturnPage") = "Fulfill_Opening.aspx"
            Response.Redirect("SkidLabelManagement_Food.aspx")

        Else
            ' Master.ShowMessagePanel("You must select an item to Edit!")
            lblMsg.Text = "You must select an item to Edit!"
            ModalPopupExtenderMsg.Show()

        End If
    End Sub
End Class
