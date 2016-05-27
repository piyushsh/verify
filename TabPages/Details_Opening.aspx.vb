Imports BPADotNetCommonFunctions
Imports System.Data.SqlClient
Imports System.Data
Imports System.IO
Imports System.Drawing
Imports VTDBFunctions.VTDBFunctions
Imports Infragistics.Web.UI.GridControls

Partial Class TabPages_Details_Opening
    Inherits MyBasePage

    Const cView = 0
    Const cReference = 1
    Const cName = 2


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim objDataPreserve As New BPADotNetCommonFunctions.VT_DataPreserve.DataPreserve

        ' register the Add buttons with the ScriptManager
        Dim TheManager As New ScriptManager
        TheManager = Master.FindControl("ScriptManager1")
        TheManager.RegisterAsyncPostBackControl(btnResendOnePopup)

        btnResendOnePopup.Attributes.Add("onclick", "return LoadResendPanel('HH');")
        btnResendOnePopupPC.Attributes.Add("onclick", "return LoadResendPanel('PCOnly');")

        btnEditOrderItem.Attributes.Add("onclick", "return LoadEditPanel();")

        cmdOKConfirm.OnClientClick = String.Format("ShowResendClose('{0}','{1}')", cmdOKConfirm.UniqueID, "")
        cmdOKConfirmEdit.OnClientClick = String.Format("EditItemClose('{0}','{1}')", cmdOKConfirmEdit.UniqueID, "")


        If Not IsPostBack Then
            Dim objCommonFuncs As New BPADotNetCommonFunctions.VT_CommonFunctions.CommonFunctions
            Dim strAllowNonHHPicking As String = objCommonFuncs.GetConfigItem("AllowNonHHPicking")

            If objCommonFuncs.GetConfigItem("ShowTruckButtons") = "YES" Then
                btnOrderDelivered.Visible = True
            Else
                btnOrderDelivered.Visible = False
            End If

            lblMsg.Text = ""

            If UCase(Me.CurrentSession.VT_OrderType) = "CALL-OFF" Then
                btnResendAll.Visible = False
                btnResendOne.Visible = False
                btnResendOnePopup.Visible = True
                If UCase(strAllowNonHHPicking) = "YES" Then
                    btnResendOnePopupPC.Visible = True
                Else
                    btnResendOnePopupPC.Visible = False
                End If
            Else
                btnResendAll.Visible = True
                btnResendOne.Visible = True
                btnResendOnePopup.Visible = False
                btnResendOnePopupPC.Visible = False
            End If


            FillGrid()

            Dim lngRowSalesOrderItemID_Selected As Long
            Dim lngRowProductID_Selected As Long

            If wdgOrderItems.Rows.Count > 0 Then
                lngRowSalesOrderItemID_Selected = CLng(wdgOrderItems.Rows(0).Items.FindItemByKey("SalesOrderItemID").Text)
                lngRowProductID_Selected = CLng(wdgOrderItems.Rows(0).Items.FindItemByKey("ProductID").Text)

                FillItemDeliveries(lngRowSalesOrderItemID_Selected, lngRowProductID_Selected)
                FillItemOnHandheldGrid()

            End If
            LoadHeaderInfo()


            'Set up any column formating requirements here
            Dim strCurrency As String
            Dim strTemp As String
            Dim ci As Globalization.CultureInfo
            ci = Globalization.CultureInfo.CurrentCulture
            strCurrency = ci.NumberFormat.CurrencySymbol

            Dim groupField As New GroupField()
            groupField = wdgOrderItems.Columns("GroupField_PriceDetails")
            strTemp = groupField.Header.Text
            groupField.Header.Text = strTemp & " " & strCurrency & " "
            groupField.Header.CssClass = "VerifyGrid_Report_HeaderGreen"

            wdgOrderItems.Behaviors.SummaryRow.ShowSummariesButtons = False
            For Each setting As SummaryRowSetting In wdgOrderItems.Behaviors.SummaryRow.ColumnSettings
                setting.ShowSummaryButton = False
            Next



            Dim strCalcWeightFromQty As String = objCommonFuncs.GetConfigItem("CalcWeightFromQty")

            hidCalcWgtFromQty.Value = UCase(strCalcWeightFromQty)

            If UCase(strCalcWeightFromQty) = "YES" Then
                Me.CurrentSession.VT_CalcQtyFromWgt = True
            Else
                Me.CurrentSession.VT_CalcQtyFromWgt = False
            End If
            DecideAllowAddOrderRow()


            If Left(UCase(lblOrderStatus.Text), 6) = "CLOSED" Then
                btnResendAll.Visible = False
                btnCloseOrder.Visible = False
            End If

            ' activate the first item
            'Dim obje As System.Web.UI.ImageClickEventArgs
            'If uwgOrderItems.Rows.Count > 0 Then
            '    uwgOrderItems.DisplayLayout.ActiveRow = uwgOrderItems.Rows(0)
            '    btnRowView_Click(sender, obje)
            'End If


            'Hde all the editing features for pre-issued orders
            If Trim(lblOrderStatus.Text) = "Open - Pre Issued" Then

                btnResendAll.Visible = False
                btnResendOne.Visible = False
                btnResendOnePopup.Visible = False
                btnResendOnePopupPC.Visible = False
                btnAddToOrder.Visible = False
                btnEditOrderItem.Visible = False
                btnPrint.Visible = False
                btnSave.Visible = False
                btnViewProductDetails.Visible = False
                btnEditOrderItem.Visible = False

                lblDeliveryCustomer.Visible = True
                ddlDeliveryCustomer.Visible = False
                lblPriority.Visible = True
                lblRequestedDeliveryDate.Visible = True
                lblPONumber.Visible = True
                lblComment.Visible = True
                txtPriority.Visible = False
                txtPO.Visible = False
                dteDeliveryDate.Visible = False
                txtComment.Visible = False
                btnSave.Visible = False
            End If


        Else
            'This is a postback so rebind grids
            '
            wdgOrderItems.DataSource = objDataPreserve.GetWDGDataFromSession(wdgOrderItems)

            wdgDeliverys.DataSource = objDataPreserve.GetWDGDataFromSession(wdgDeliverys)

            wdgHandheldItems.DataSource = objDataPreserve.GetWDGDataFromSession(wdgHandheldItems)


        End If

    End Sub
    Protected Sub btnRowView_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        'If IsNothing(uwgOrderItems.DisplayLayout.ActiveRow.Cells.FromKey("SalesOrderItemID").Text) = False AndAlso uwgOrderItems.DisplayLayout.ActiveRow.Cells.FromKey("SalesOrderItemID").Text <> 0 Then


        '    FillItemDeliveries(uwgOrderItems.DisplayLayout.ActiveRow.Cells.FromKey("SalesOrderItemID").Text, uwgOrderItems.DisplayLayout.ActiveRow.Cells.FromKey("ProductID").Text)

        '    'FillCallOffPanelItems(uwgOrderItems.DisplayLayout.ActiveRow)

        '    DecideAllowEditOrderRow(uwgOrderItems.DisplayLayout.ActiveRow.Cells.FromKey("SalesOrderItemID").Text)

        'End If
    End Sub
    Protected Sub LoadHeaderInfo()
        Dim lngSalesOrderId As Long
        Dim dsOrder As New DataSet
        Dim objBPA As New VTDBFunctions.VTDBFunctions.TelesalesFunctions
        Dim lngBillingCustomerID As Long
        Dim lngDeliveryCustomerID As Long
        Dim dsCustomer As New DataSet
        Dim objCust As New VTDBFunctions.VTDBFunctions.CustomerFunctions

        lngSalesOrderId = Me.CurrentSession.VT_SalesOrderID
        dsOrder = objBPA.GetOrderForId(lngSalesOrderId)
        lblComment.Text = IIf(IsDBNull(dsOrder.Tables(0).Rows(0).Item("Comment")), "", dsOrder.Tables(0).Rows(0).Item("Comment"))

        lblOrderDate.Text = IIf(IsDBNull(dsOrder.Tables(0).Rows(0).Item("DateCreated")), "", dsOrder.Tables(0).Rows(0).Item("DateCreated"))

        lblRequestedDeliveryDate.Text = IIf(IsDBNull(dsOrder.Tables(0).Rows(0).Item("RequestedDeliveryDate")), "", dsOrder.Tables(0).Rows(0).Item("RequestedDeliveryDate"))
        lblPONumber.Text = IIf(IsDBNull(dsOrder.Tables(0).Rows(0).Item("CustomerPO")), "", dsOrder.Tables(0).Rows(0).Item("CustomerPO"))
        lblOrderStatus.Text = IIf(IsDBNull(dsOrder.Tables(0).Rows(0).Item("Status")), "", dsOrder.Tables(0).Rows(0).Item("Status"))

        lblPriority.Text = Me.CurrentSession.VT_OrderPriority
        txtPriority.Text = Me.CurrentSession.VT_OrderPriority
        dteDeliveryDate.Value = IIf(IsDBNull(dsOrder.Tables(0).Rows(0).Item("RequestedDeliveryDate")), "", dsOrder.Tables(0).Rows(0).Item("RequestedDeliveryDate"))
        txtPO.Text = Trim(lblPONumber.Text)
        Me.CurrentSession.VT_OrderPO = Trim(lblPONumber.Text)
        txtComment.Text = Trim(lblComment.Text)

        If CheckAllowedToEdit() = False Then
            lblPriority.Visible = True
            lblRequestedDeliveryDate.Visible = True
            lblPONumber.Visible = True
            lblComment.Visible = True
            txtPriority.Visible = False
            txtPO.Visible = False
            dteDeliveryDate.Visible = False
            txtComment.Visible = False
            btnSave.Visible = False
            lblDeliveryCustomer.Visible = True
            ddlDeliveryCustomer.Visible = False
        Else
            lblPriority.Visible = False
            lblRequestedDeliveryDate.Visible = False
            lblPONumber.Visible = False
            lblComment.Visible = False
            txtPriority.Visible = True
            txtPO.Visible = True
            dteDeliveryDate.Visible = True
            txtComment.Visible = True
            btnSave.Visible = True
            lblDeliveryCustomer.Visible = False
            ddlDeliveryCustomer.Visible = True
        End If

        If Me.CurrentSession.VT_OrderTypesEnabled = "YES" Then
            If IsDBNull(dsOrder.Tables(0).Rows(0).Item("Type")) = True OrElse dsOrder.Tables(0).Rows(0).Item("Type") = "" Then
                lblOrderType.Text = "Standard"
            Else
                lblOrderType.Text = dsOrder.Tables(0).Rows(0).Item("Type")
            End If
        Else
            lblOrderType.Text = "Standard"
            '  lblOrderType.Visible = False
            '  lblOrderTypeLabel.Visible = False
        End If

        'need to get the customer details using the ID from this dataset
        lngBillingCustomerID = dsOrder.Tables(0).Rows(0).Item("CustomerId")
        Me.CurrentSession.VT_CustomerID = lngBillingCustomerID
        lngDeliveryCustomerID = dsOrder.Tables(0).Rows(0).Item("DeliveryCustomerId")
        Me.CurrentSession.VT_DeliveryCustomerID = lngDeliveryCustomerID

        If lngBillingCustomerID = lngDeliveryCustomerID Then
            dsCustomer = objCust.GetCustomerDetailsForId(lngBillingCustomerID)
            If dsCustomer.Tables(0).Rows.Count > 0 Then
                lblCustomerName.Text = Trim(dsCustomer.Tables(0).Rows(0).Item("CustomerName"))
                lblDeliveryCustomer.Text = Trim(dsCustomer.Tables(0).Rows(0).Item("CustomerName"))
                If IsDBNull(dsCustomer.Tables(0).Rows(0).Item("DeliveryAddress")) OrElse dsCustomer.Tables(0).Rows(0).Item("DeliveryAddress") = "" Then
                    lblCustAddress.Text = Trim(IIf(IsDBNull(dsCustomer.Tables(0).Rows(0).Item("BillingAddress")), "", dsCustomer.Tables(0).Rows(0).Item("BillingAddress")))
                Else
                    lblCustAddress.Text = Trim(IIf(IsDBNull(dsCustomer.Tables(0).Rows(0).Item("DeliveryAddress")), "", dsCustomer.Tables(0).Rows(0).Item("DeliveryAddress")))
                End If
            Else
                lblCustAddress.Text = ""
                lblCustomerName.Text = ""
                lblDeliveryCustomer.Text = ""
            End If
        Else
            lblCustomerName.Text = Trim(objCust.GetCustomerNameForId(lngBillingCustomerID))
            dsCustomer = objCust.GetCustomerDetailsForId(lngDeliveryCustomerID)
            If dsCustomer.Tables(0).Rows.Count > 0 Then
                lblDeliveryCustomer.Text = Trim(dsCustomer.Tables(0).Rows(0).Item("CustomerName"))
                lblCustAddress.Text = Trim(IIf(IsDBNull(dsCustomer.Tables(0).Rows(0).Item("BillingAddress")), "", dsCustomer.Tables(0).Rows(0).Item("BillingAddress")))
            Else
                lblCustAddress.Text = ""
                lblCustomerName.Text = ""
            End If

        End If

        SetupDeliveryCustDropdown()

        lblWO.Text = "Sales Order: " & CStr(Me.CurrentSession.VT_SalesOrderNum)


    End Sub

    Protected Sub FillGrid()

        Dim dsOrderItems As New DataSet
        Dim objTele As New TelesalesFunctions
        Dim objProd As New VTDBFunctions.VTDBFunctions.ProductsFunctions
        Dim strTemp As String
        Dim strCurrency As String
        Dim objDataPreserve As New BPADotNetCommonFunctions.VT_DataPreserve.DataPreserve
        Dim objSales As New BPADotNetCommonFunctions.SalesFunctions.Sales_Orders

        Dim strConn As String = Session("_VT_DotNetConnString")

        Dim dtItemsMatrix As New DataTable
        Dim dsSalesOrder As New DataSet


        dsOrderItems = objTele.GetOrderItems(Me.CurrentSession.VT_SalesOrderID)
        Dim strSalesOrderNum As String '= dsOrderItems.Tables(0).Rows(0).Item("SalesOrderNum")
        dsSalesOrder = objTele.GetOrderForId(Me.CurrentSession.VT_SalesOrderID)
        If dsSalesOrder.Tables(0).Rows.Count > 0 Then
            strSalesOrderNum = dsSalesOrder.Tables(0).Rows(0).Item("SalesOrderNum")
        Else
            strSalesOrderNum = "0"
        End If


        'Get the Order Items form the matrix
        dtItemsMatrix = objSales.GetSalesOrderItemsFromMatrix("OrderItems", CInt(strSalesOrderNum), strConn, Me.CurrentSession.VT_tlsNumFields)

        Dim ci As Globalization.CultureInfo
        ci = Globalization.CultureInfo.CurrentCulture
        strCurrency = ci.NumberFormat.CurrencySymbol

        Dim i As Integer = 0

        'Add required columns to datatable
        Dim dtTemp As DataTable
        Dim strDrSearch As String
        Dim drSearchData() As DataRow
        dtTemp = dsOrderItems.Tables(0)

        If dtTemp.Columns.Contains("ProductCode") = False Then
            dtTemp.Columns.Add("ProductCode")
        End If
        If dtTemp.Columns.Contains("ProductName") = False Then
            dtTemp.Columns.Add("ProductName")
        End If
        If dtTemp.Columns.Contains("Item") = False Then
            dtTemp.Columns.Add("Item")
        End If
        If dtTemp.Columns.Contains("Comment") = False Then
            dtTemp.Columns.Add("Comment")
        End If

        Dim drComment() As DataRow

        For intCnt As Integer = 0 To dtTemp.Rows.Count - 1
            'removed by AM 2015-08-11. Error generated by select stmt on datatable. Only need to get product name and code here, this select is not necessary
            'If IsNothing(dtItemsMatrix) = False AndAlso dtItemsMatrix.Rows.Count > 0 Then ''Also' added by JD 29/06/2014 - function failing here

            '    'For the product name for the item code in the dtItemsMatrix table
            '    strDrSearch = "ProductId = " & dtTemp.Rows(intCnt).Item("ProductId").ToString
            '    drSearchData = dtItemsMatrix.Select(strDrSearch)

            '    If drSearchData.Length > 0 Then
            '        dtTemp.Rows(intCnt).Item("ProductName") = drSearchData(0).Item("ProductName").ToString
            '        dtTemp.Rows(intCnt).Item("ProductCode") = drSearchData(0).Item("ProductCode").ToString
            '    End If

            'Else
            dtTemp.Rows(intCnt).Item("ProductName") = objProd.GetProductNameForId(dtTemp.Rows(intCnt).Item("ProductID"))
            dtTemp.Rows(intCnt).Item("ProductCode") = objProd.GetProductCode(dtTemp.Rows(intCnt).Item("ProductID"))
            'End If

            dtTemp.Rows(intCnt).Item("Item") = intCnt + 1
            If dtItemsMatrix.Rows.Count > 0 Then
                drComment = dtItemsMatrix.Select("SalesOrderItemId =" & dtTemp.Rows(intCnt).Item("SalesOrderItemID"))
                If drComment.Length > 0 Then
                    dtTemp.Rows(intCnt).Item("Comment") = drComment(0)("Comment")
                End If
            End If

        Next


        'Bind the Grid
        objDataPreserve.BindDataToWDG(dtTemp, wdgOrderItems)



    End Sub

    Sub FillItemDeliveries(ByVal lngOrderItemID As Long, ByVal lngProductID As Long)

        Dim lngSalesOrderId As Long
        Dim dsTrans As New DataSet
        Dim dsProduct As New DataSet
        Dim objDBF As New VTDBFunctions.VTDBFunctions.TelesalesFunctions
        Dim objProd As New VTDBFunctions.VTDBFunctions.ProductsFunctions
        Dim objDataPreserve As New BPADotNetCommonFunctions.VT_DataPreserve.DataPreserve

        lngSalesOrderId = Me.CurrentSession.VT_SalesOrderID

        ' get the transactions from the trc_Transaction table for this OrderItem 
        '   dsTrans = objDBF.GetTransactionsForOrderItemId(lngOrderItemID)

        dsTrans = objDBF.GetTransactionsForOrderNumAndProdID(Me.CurrentSession.VT_SalesOrderNum, lngProductID)

        'Add required columns to datatable
        Dim dtTemp As DataTable
        dtTemp = dsTrans.Tables(0)
        dtTemp.Columns.Add("Item")

        'if there are no rows then add a blank row
        If dtTemp.Rows.Count = 0 Then
            dtTemp.Rows.Add()
        End If

        For intCnt As Integer = 0 To dtTemp.Rows.Count - 1
            dtTemp.Rows(intCnt).Item("Item") = intCnt
        Next

        'Bind the Grid
        objDataPreserve.BindDataToWDG(dtTemp, wdgDeliverys)

    End Sub

    Sub FillItemOnHandheldGrid()

        Dim objCommonFuncs As New BPADotNetCommonFunctions.VT_CommonFunctions.CommonFunctions
        Dim objDataPreserve As New BPADotNetCommonFunctions.VT_DataPreserve.DataPreserve
        Dim strPublished As String = objCommonFuncs.GetConfigItem("IsPublished")



        Dim dtTrans As New DataTable
        Dim dsProduct As New DataSet
        Dim objSO As New SalesOrdersFunctions.SalesOrders
        Dim objProd As New VTDBFunctions.VTDBFunctions.ProductsFunctions

        ' get the transactions from the trc_Transaction table for this OrderItem 
        dtTrans = objSO.GetItemsOnHandheld(Me.CurrentSession.VT_JobID)

        'Add required columns to datatable
        dtTrans.Columns.Add("ProductName")

        For intCnt As Integer = 0 To dtTrans.Rows.Count - 1
            dsProduct = objProd.GetProductForCode(dtTrans.Rows(intCnt).Item("ProductId"))
            dtTrans.Rows(intCnt).Item("ProductName") = dsProduct.Tables(0).Rows(0).Item("Product_Name")
        Next


        'Bind the Grid
        objDataPreserve.BindDataToWDG(dtTrans, wdgHandheldItems)

        If UCase(strPublished) <> "YES" Then
            lblPicking.Visible = False
            lblPickingExtra.Visible = False
            wdgHandheldItems.Visible = False
        End If

    End Sub

    'Protected Sub uwgOrderItems_Click(ByVal sender As Object, ByVal e As Infragistics.WebUI.UltraWebGrid.ClickEventArgs) Handles uwgOrderItems.Click

    '    If IsNothing(uwgOrderItems.DisplayLayout.ActiveRow) = False AndAlso IsNothing(uwgOrderItems.DisplayLayout.ActiveRow.Cells.FromKey("SalesOrderItemID").Text) = False AndAlso uwgOrderItems.DisplayLayout.ActiveRow.Cells.FromKey("SalesOrderItemID").Text <> 0 Then


    '        'FillItemDeliveries(uwgOrderItems.DisplayLayout.ActiveRow.Cells.FromKey("SalesOrderItemID").Text, uwgOrderItems.DisplayLayout.ActiveRow.Cells.FromKey("ProductID").Text)

    '        ''FillCallOffPanelItems(uwgOrderItems.DisplayLayout.ActiveRow)
    '        'FillCallOffPanelItems(uwgOrderItems.DisplayLayout.ActiveRow)

    '        'DecideAllowEditOrderRow(uwgOrderItems.DisplayLayout.ActiveRow.Cells.FromKey("SalesOrderItemID").Text)

    '        'Session("_VT_SelectedOrderItemGridRow") = uwgOrderItems.DisplayLayout.ActiveRow

    '    End If
    'End Sub

    Protected Sub wdgOrderItems_ActiveCellChanged(sender As Object, e As ActiveCellEventArgs) Handles wdgOrderItems.ActiveCellChanged
        Dim objG As New BPADotNetCommonFunctions.VT_ReportFunctions.GeneralFunctions
        Dim objC As New VT_CommonFunctions.CommonFunctions

        'Serialise the Selected DataRow
        Dim intActiveRowIndex As Integer = wdgOrderItems.Behaviors.Activation.ActiveCell.Row.Index
        Me.CurrentSession.VT_SelectedOrderItemGridRow_V2 = objC.SerialiseWebDataGridRow(wdgOrderItems, intActiveRowIndex)

        wdgOrderItems.Behaviors.Activation.ActiveCell = wdgOrderItems.Rows(intActiveRowIndex).Items(1)
        FillItemDeliveries(Me.CurrentSession.VT_SelectedOrderItemGridRow_V2.Rows(0).Item("SalesOrderItemID"), Me.CurrentSession.VT_SelectedOrderItemGridRow_V2.Rows(0).Item("ProductID"))

        FillCallOffPanelItems(Me.CurrentSession.VT_SelectedOrderItemGridRow_V2)

        DecideAllowEditOrderRow(Me.CurrentSession.VT_SelectedOrderItemGridRow_V2.Rows(0).Item("SalesOrderItemID"))

        UpdatePanel1.Update()

    End Sub

    Protected Sub wdgOrderItems_RowSelectionChanged(sender As Object, e As SelectedRowEventArgs) Handles wdgOrderItems.RowSelectionChanged
        Dim objC As New VT_CommonFunctions.CommonFunctions

        'Serialise the Selected DataRow
        Dim RowSelected As Infragistics.Web.UI.GridControls.SelectedRowCollection = wdgOrderItems.Behaviors.Selection.SelectedRows
        Dim intActiveRowIndex As Integer = RowSelected(0).Index
        Me.CurrentSession.VT_SelectedOrderItemGridRow_V2 = objC.SerialiseWebDataGridRow(wdgOrderItems, intActiveRowIndex)

        wdgOrderItems.Behaviors.Activation.ActiveCell = wdgOrderItems.Rows(intActiveRowIndex).Items(1)
        FillItemDeliveries(Me.CurrentSession.VT_SelectedOrderItemGridRow_V2.Rows(0).Item("SalesOrderItemID"), Me.CurrentSession.VT_SelectedOrderItemGridRow_V2.Rows(0).Item("ProductID"))

        FillCallOffPanelItems(Me.CurrentSession.VT_SelectedOrderItemGridRow_V2)

        DecideAllowEditOrderRow(Me.CurrentSession.VT_SelectedOrderItemGridRow_V2.Rows(0).Item("SalesOrderItemID"))

        UpdatePanel1.Update()

    End Sub



    Sub FillCallOffPanelItems(ByVal dtRow As DataTable)

        Dim lngProductID As Long
        Dim intUnitOfSale As Integer
        Dim objProd As New VTDBFunctions.VTDBFunctions.ProductsFunctions
        Dim objTele As New VTDBFunctions.VTDBFunctions.TelesalesFunctions
        Dim strWeigtPerUnit As Double
        '        Dim dblWeigtPerUnit As Double

        ' lblPnlProductCode.Text = 
        lblPnlProductName.Text = Trim(dtRow.Rows(0).Item("Product"))
        lblPnlProductNameEdit.Text = Trim(dtRow.Rows(0).Item("Product"))


        lngProductID = dtRow.Rows(0).Item("ProductId")
        intUnitOfSale = objProd.GetUnitOfSale(lngProductID)
        strWeigtPerUnit = objProd.GetProductAvgWeightPerUnit(lngProductID)
        lblPerUnitWeight.Value = strWeigtPerUnit
        hdnUnitPrice.Value = objTele.GetPriceForProductAndCustomer(lngProductID, Me.CurrentSession.VT_CustomerID, "ASP")
        hdnUnitOfSale.Value = intUnitOfSale

        'Convert Values
        Dim dblQtyOrdered, dblQtyDelivered As Integer
        Dim dblWgtOrdered, dblWgtDelivered As Double
        If dtRow.Rows(0).Item("QtyOrdered") <> "" Then
            dblQtyOrdered = CInt(Trim(dtRow.Rows(0).Item("QtyOrdered")))
        Else
            dblQtyOrdered = 0
        End If
        If dtRow.Rows(0).Item("QtyDelivered") <> "" Then
            dblQtyDelivered = CInt(Trim(dtRow.Rows(0).Item("QtyDelivered")))
        Else
            dblQtyDelivered = 0
        End If
        If dtRow.Rows(0).Item("WgtOrdered") <> "" Then
            dblWgtOrdered = CDbl(Trim(dtRow.Rows(0).Item("WgtOrdered")))
        Else
            dblWgtOrdered = 0.0
        End If
        If dtRow.Rows(0).Item("WgtDelivered") <> "" Then
            dblWgtDelivered = CDbl(Trim(dtRow.Rows(0).Item("WgtDelivered")))
        Else
            dblWgtDelivered = 0.0
        End If

        If intUnitOfSale = 1 Then
            'Picking Panel
            lblPnlOrderedQty.Text = dblQtyOrdered
            lblPnlOutstandingQty.Text = dblQtyOrdered - dblQtyDelivered
            txtPnlQtyToPick.Text = lblPnlOutstandingQty.Text
            lblPnlOrderedWgt.Text = 0
            lblPnlOutstandingWgt.Text = 0
            txtPnlWgtToPick.Text = 0
            lblPnlOrderedWgtLabel.Visible = False
            lblPnlOutstandingWgtLabel.Visible = False
            lblPnlWgtToPickLabel.Visible = False
            lblPnlOrderedWgt.Visible = False
            lblPnlOutstandingWgt.Visible = False
            txtPnlWgtToPick.Visible = False
            'Item Edit Panel
            lblPnlOrderedQtyEdit.Text = dblQtyOrdered
            lblPnlOutstandingQtyEdit.Text = dblQtyOrdered - dblQtyDelivered
            lblPnlDeliveredQtyEdit.Text = dblQtyDelivered
            txtPnlNewOrderQty.Text = ""
            lblPnlOrderedWgtEdit.Text = 0
            lblPnlOutstandingWgtEdit.Text = 0
            lblPnlDeliveredWgtEdit.Text = 0
            txtPnlNewOrderWgt.Text = 0
            lblPnlOrderedWgtLabelEdit.Visible = False
            lblPnlOutstandingWgtLabelEdit.Visible = False
            lblPnlNewOrderWgtLabel.Visible = False
            lblPnlDeliveredWgtLabelEdit.Visible = False
            lblPnlDeliveredWgtEdit.Visible = False
            lblPnlOrderedWgtEdit.Visible = False
            lblPnlOutstandingWgtEdit.Visible = False
            txtPnlNewOrderWgt.Visible = False


        Else
            'Picking Panel
            lblPnlOrderedQty.Text = dblQtyOrdered
            lblPnlOrderedWgt.Text = dblWgtOrdered
            lblPnlOrderedWgtLabel.Visible = True
            lblPnlOutstandingQty.Text = dblQtyOrdered - dblQtyDelivered
            lblPnlOutstandingWgt.Text = dblWgtOrdered - dblWgtDelivered
            lblPnlOutstandingWgtLabel.Visible = True
            lblPnlWgtToPickLabel.Visible = True
            lblPnlDeliveredQtyEdit.Text = dblQtyDelivered
            lblPnlDeliveredWgtEdit.Text = dblWgtDelivered

            txtPnlQtyToPick.Text = lblPnlOutstandingQty.Text
            txtPnlWgtToPick.Text = lblPnlOutstandingWgt.Text
            'Item Edit Panel
            lblPnlOrderedQtyEdit.Text = dblQtyOrdered
            lblPnlOrderedWgtEdit.Text = dblWgtOrdered
            lblPnlOrderedWgtLabelEdit.Visible = True
            lblPnlOutstandingQtyEdit.Text = dblQtyOrdered - dblQtyDelivered
            lblPnlOutstandingWgtEdit.Text = dblWgtOrdered - dblWgtDelivered
            lblPnlOutstandingWgtLabelEdit.Visible = True
            lblPnlNewOrderWgtLabel.Visible = True

            lblPnlDeliveredWgtLabelEdit.Visible = True
            lblPnlDeliveredWgtEdit.Visible = True

            txtPnlNewOrderQty.Text = ""
            txtPnlNewOrderWgt.Text = ""
        End If

    End Sub



    Protected Sub btnCloseOrder_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCloseOrder.Click

        Dim lngSalesOrderId, lngSalesOrderItemId As Long
        Dim intTraceCodeId, intSalesOrderNumber As Integer
        Dim dsOrder As New DataSet
        Dim dsOrderItem As New DataSet
        Dim objTele As New VTDBFunctions.VTDBFunctions.TelesalesFunctions
        Dim strOrderStatus, strOrderItemStatus, strAuditComment As String
        Dim objDisp As New VT_Display.DisplayFuncs
        Dim MyRow As DataRow
        Dim objForms As New VT_Forms.Forms
        Dim dblQtyWeight As Double
        Dim lngProduct As Long
        Dim objProduct As New VTDBFunctions.VTDBFunctions.ProductsFunctions
        Dim dtTemp As New DataTable
        Dim objDataPreserve As New BPADotNetCommonFunctions.VT_DataPreserve.DataPreserve
        Dim objSales As New BPADotNetCommonFunctions.SalesFunctions.Sales_Orders

        Dim strConn As String = Session("_VT_DotNetConnString")


        lngSalesOrderId = Me.CurrentSession.VT_SalesOrderID
        ' get the details of the original Sales Order
        dsOrder = objTele.GetOrderForId(lngSalesOrderId)
        strOrderStatus = Trim(dsOrder.Tables(0).Rows(0).Item("Status"))
        If strOrderStatus = objTele.cOrderClosedPartiallyComplete Or strOrderStatus = objTele.cOrderComplete Then
            '   objDisp.DisplayMessage(Page, "This order is already closed!")
            lblMsg.Text = "This order is already closed!"
            ModalPopupExtender3.Show()
            Exit Sub
        End If

        'save true value to this variable so that the status will update on the orders grid
        Me.CurrentSession.blnOrderStatusChanged = True

        dtTemp = objDataPreserve.GetWDGDataFromSession(wdgOrderItems)

        For Each MyRow In dtTemp.Rows
            ' get the details of the current item
            lngSalesOrderItemId = MyRow.Item("SalesOrderItemID")
            dsOrderItem = objTele.GetOrderItemForId(lngSalesOrderItemId)
            strOrderItemStatus = Trim(dsOrderItem.Tables(0).Rows(0).Item("Status"))

            ' if the item is not already closed
            If strOrderItemStatus <> objTele.cOrderItemClosedPartiallyComplete And _
                strOrderItemStatus <> objTele.cOrderItemComplete Then

                objTele.CloseSalesOrderItem(lngSalesOrderItemId)
                'unallocate the stock that was assigned to this item
                lngProduct = dsOrderItem.Tables(0).Rows(0).Item("ProductId")
                intTraceCodeId = IIf(IsDBNull(dsOrderItem.Tables(0).Rows(0).Item("TraceCodeId")), 0, dsOrderItem.Tables(0).Rows(0).Item("TraceCodeId"))
                If objProduct.GetUnitOfSale(lngProduct) = 1 Then 'by unit
                    dblQtyWeight = dsOrderItem.Tables(0).Rows(0).Item("QuantityRequested") - dsOrderItem.Tables(0).Rows(0).Item("Quantity")
                Else
                    dblQtyWeight = dsOrderItem.Tables(0).Rows(0).Item("WeightRequested") - dsOrderItem.Tables(0).Rows(0).Item("Weight")
                End If

                objProduct.RecalculateAllocated(lngProduct)

            End If

        Next


        ' now close the order
        objTele.CompleteSalesOrder(lngSalesOrderId, objTele.cOrderClosedPartiallyComplete)
        lblOrderStatus.Text = objTele.cOrderClosedPartiallyComplete

        ' Also write the Order status to the Job table.
        objForms.SetJobStatusText(TelesalesFunctions.cOrderClosedPartiallyComplete, Me.CurrentSession.VT_JobID)

        ' Delete the items for this order from the Handheld table
        intSalesOrderNumber = dsOrder.Tables(0).Rows(0).Item("SalesOrderNum")
        objTele.DeleteHandheldSalesOrder(intSalesOrderNumber)

        ' write to the AuditLog
        strAuditComment = "Incomplete Order manually Closed"
        'objForms.WriteToAuditLog(intSalesOrderNumber, "Telesales", Now(), Session("_VT_CurrentUserName"), Session("_VT_CurrentUserId"), strAuditComment, "Audit Record", "")

        'if steripack then use this logging:
        Dim strType As String
        ' strAuditComment = "New Order created"
        strType = "Sales Order"
        objForms.WriteToAuditLog(intSalesOrderNumber, strType, PortalFunctions.Now, Session("_VT_CurrentUserName"), Session("_VT_CurrentUserId"), strAuditComment, "Audit Record", "SysAdmin")


        'save the date closed
        objSales.SaveGeneralItemsToSalesMatrix("DateClosed", "Details", PortalFunctions.Now.ToString("s"), intSalesOrderNumber, strConn)

        FillItemOnHandheldGrid()

        FillGrid()

        Me.CurrentSession.blnOrderStatusChanged = True


        '   objDisp.DisplayMessage(Page, "This order has been closed!")
        lblMsg.Text = "This order has been closed!"
        ModalPopupExtender3.Show()



    End Sub

    Protected Sub btnResendAll_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnResendAll.Click

        ' we will only get here if the Ok action is chosen on the Confirmation Dialog
        Dim objTele As New VTDBFunctions.VTDBFunctions.TelesalesFunctions
        Dim MyRow As DataRow
        Dim dblQtyToSend, dblWgtToSend As Double
        Dim lngSalesOrderId, lngSalesOrderItemId, lngProductId As Long
        Dim intTraceCodeId, intUnitOfSale As Integer
        Dim objDisp As New VT_Display.DisplayFuncs
        Dim dsSalesOrderItem As DataSet
        Dim strTemp As String
        Dim intNumItemsSent As Integer = 0
        Dim blnCanSendThisItem As Boolean
        Dim objProd As New VTDBFunctions.VTDBFunctions.ProductsFunctions
        Dim intCnt As Integer = 0
        Dim dtTemp As New DataTable
        Dim objDataPreserve As New BPADotNetCommonFunctions.VT_DataPreserve.DataPreserve

        lngSalesOrderId = Me.CurrentSession.VT_SalesOrderID

        dtTemp = objDataPreserve.GetWDGDataFromSession(wdgOrderItems)

        For Each MyRow In dtTemp.Rows

            intCnt = intCnt + 1

            If Trim(MyRow.Item("Status")) = objTele.cOrderItemNew And lblOrderStatus.Text.Trim = "Open - New" Then
                blnCanSendThisItem = False
            ElseIf Trim(MyRow.Item("Status")) = objTele.cOrderItemComplete Or _
             Trim(MyRow.Item("Status")) = objTele.cOrderItemClosedPartiallyComplete Then
                blnCanSendThisItem = False
            Else
                blnCanSendThisItem = True
                lngSalesOrderItemId = MyRow.Item("SalesOrderItemID")

                If objTele.HasItemAlreadyBeenSentforPicking(lngSalesOrderItemId) Then
                    blnCanSendThisItem = False
                End If
            End If

            If blnCanSendThisItem = True Then


                dblQtyToSend = Val(MyRow.Item("QuantityRequested")) - Val(MyRow.Item("Quantity"))
                dblWgtToSend = Val(MyRow.Item("WeightRequested")) - Val(MyRow.Item("Weight"))


                dsSalesOrderItem = objTele.GetOrderItemForId(lngSalesOrderItemId)
                lngProductId = dsSalesOrderItem.Tables(0).Rows(0).Item("ProductID")

                intUnitOfSale = objProd.GetUnitOfSale(lngProductId)

                If intUnitOfSale = 1 And dblQtyToSend = 0 Then
                    strTemp = "The item in Row " + CStr(intCnt) + " - "
                    strTemp = strTemp + Trim(MyRow.Item("Product")) + " - is managed by Quantity and the Quantity selected is 0.\n\n"
                    strTemp = strTemp + "This item cannot be sent for picking."
                    '  objDisp.DisplayMessage(Page, strTemp)
                    lblMsg.Text = strTemp
                    ModalPopupExtender3.Show()
                ElseIf intUnitOfSale = 0 And dblWgtToSend = 0 Then
                    strTemp = "The item in Row " + CStr(intCnt) + " - "
                    strTemp = strTemp + Trim(MyRow.Item("Product")) + " - is managed by Weight and the Weight selected is 0.\n\n"
                    strTemp = strTemp + "This item cannot be sent for picking."
                    '   objDisp.DisplayMessage(Page, strTemp)

                    lblMsg.Text = strTemp
                    ModalPopupExtender3.Show()

                Else
                    intTraceCodeId = 0  ' we don't know what TraceCode to use for the remaining stuff
                    objTele.SendItemForPicking(lngSalesOrderId, lngSalesOrderItemId, dblQtyToSend, dblWgtToSend, intTraceCodeId)
                    intNumItemsSent += 1
                End If

            End If

        Next

        'For Each MyRow In uwgOrderItems.Rows
        '    If Trim(MyRow.Cells.FromKey("Status").Text) = objTele.cOrderItemNew And lblOrderStatus.Text.Trim = "Open - New" Then
        '        blnCanSendThisItem = False
        '    ElseIf Trim(MyRow.Cells.FromKey("Status").Text) = objTele.cOrderItemComplete Or _
        '     Trim(MyRow.Cells.FromKey("Status").Text) = objTele.cOrderItemClosedPartiallyComplete Then
        '        blnCanSendThisItem = False
        '    Else
        '        blnCanSendThisItem = True
        '    End If

        '    If blnCanSendThisItem = True Then

        '        lngSalesOrderItemId = MyRow.Cells.FromKey("SalesOrderItemID").Value

        '        dblQtyToSend = Val(MyRow.Cells.FromKey("QtyOrdered").Text) - Val(MyRow.Cells.FromKey("QtyDelivered").Text)
        '        dblWgtToSend = Val(MyRow.Cells.FromKey("WgtOrdered").Text) - Val(MyRow.Cells.FromKey("WgtDelivered").Text)


        '        dsSalesOrderItem = objTele.GetOrderItemForId(lngSalesOrderItemId)
        '        lngProductId = dsSalesOrderItem.Tables(0).Rows(0).Item("ProductID")

        '        intUnitOfSale = objProd.GetUnitOfSale(lngProductId)

        '        If intUnitOfSale = 1 And dblQtyToSend = 0 Then
        '            strTemp = "The item in Row " + CStr(MyRow.Index + 1) + " - "
        '            strTemp = strTemp + Trim(MyRow.Cells.FromKey("Product").Text) + " - is measured by Quantity and the Quantity selected is 0.\n\n"
        '            strTemp = strTemp + "This item cannot be sent for picking."
        '            '  objDisp.DisplayMessage(Page, strTemp)
        '            lblMsg.Text = strTemp
        '            ModalPopupExtender3.Show()
        '        ElseIf intUnitOfSale = 0 And dblWgtToSend = 0 Then
        '            strTemp = "The item in Row " + CStr(MyRow.Index + 1) + " - "
        '            strTemp = strTemp + Trim(MyRow.Cells.FromKey("Product").Text) + " - is measured by Weight and the Weight selected is 0.\n\n"
        '            strTemp = strTemp + "This item cannot be sent for picking."
        '            '   objDisp.DisplayMessage(Page, strTemp)

        '            lblMsg.Text = strTemp
        '            ModalPopupExtender3.Show()

        '        Else
        '            intTraceCodeId = 0  ' we don't know what TraceCode to use for the remaining stuff
        '            objTele.SendItemForPicking(lngSalesOrderId, lngSalesOrderItemId, dblQtyToSend, dblWgtToSend, intTraceCodeId)
        '            intNumItemsSent += 1
        '        End If

        '    End If

        'Next

        FillItemOnHandheldGrid()


        If intNumItemsSent = 0 Then
            lblMsg.Text = "Only Open items or items not already sent for picking can be resent for picking. There are no such items in this order!"
            ModalPopupExtender3.Show()
        Else
            lblMsg.Text = "The (" + CStr(intNumItemsSent) + ") outstanding Order Items will be loaded on the Handheld device when it is next Synchronised!"
            ModalPopupExtender3.Show()
        End If


    End Sub

    Protected Sub btnResendOne_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnResendOne.Click
        ' we will only get here if the Ok action is chosen on the Confirmation Dialog
        Dim objTele As New VTDBFunctions.VTDBFunctions.TelesalesFunctions
        Dim objProd As New VTDBFunctions.VTDBFunctions.ProductsFunctions
        Dim objDisp As New VT_Display.DisplayFuncs
        Dim lngSalesOrderId, lngSalesOrderItemId, lngProductId As Long
        Dim dblQtyToSend, dblWgtToSend As Double
        Dim ActiveRow As Infragistics.WebUI.UltraWebGrid.UltraGridRow
        Dim intTraceCodeId, intUnitOfSale As Integer
        Dim dsSalesOrderItem As DataSet
        Dim strTemp As String


        lngSalesOrderId = Me.CurrentSession.VT_SalesOrderID
        If IsNothing(Me.CurrentSession.VT_SelectedOrderItemGridRow_V2) Then
            '     objDisp.DisplayMessage(Page, "You must select an Item to Resend")
            lblMsg.Text = "You must select an Item to Resend"
            ModalPopupExtender3.Show()
            Exit Sub
        End If

        'ActiveRow = uwgOrderItems.DisplayLayout.ActiveRow
        'lngSalesOrderItemId = ActiveRow.Cells.FromKey("SalesOrderItemID").Value
        lngSalesOrderItemId = Me.CurrentSession.VT_SelectedOrderItemGridRow_V2.Rows(0).Item("SalesOrderItemID")

        If Trim(Me.CurrentSession.VT_SelectedOrderItemGridRow_V2.Rows(0).Item("Status")) = objTele.cOrderItemNew And lblOrderStatus.Text = "New" Then
            '  objDisp.DisplayMessage(Page, "You cannot Resend a New Item. It has already been sent for Picking")
            lblMsg.Text = "You cannot Resend a New Item. It has already been sent for Picking"
            ModalPopupExtender3.Show()
            Exit Sub
        End If

        If Trim(Me.CurrentSession.VT_SelectedOrderItemGridRow_V2.Rows(0).Item("Status")) = objTele.cOrderItemComplete Or _
             Trim(Me.CurrentSession.VT_SelectedOrderItemGridRow_V2.Rows(0).Item("Status")) = objTele.cOrderItemClosedPartiallyComplete Then
            ' objDisp.DisplayMessage(Page, "You cannot Resend a Closed Item.")
            lblMsg.Text = "You cannot Resend a Closed Item for picking."
            ModalPopupExtender3.Show()
            Exit Sub
        End If

        If objTele.HasItemAlreadyBeenSentforPicking(lngSalesOrderItemId) Then
            lblMsg.Text = "This item has already been sent for Picking."
            ModalPopupExtender3.Show()
            Exit Sub
        End If

        dsSalesOrderItem = objTele.GetOrderItemForId(lngSalesOrderItemId)
        lngProductId = dsSalesOrderItem.Tables(0).Rows(0).Item("ProductId")
        intUnitOfSale = objProd.GetUnitOfSale(lngProductId)

        dblQtyToSend = Val(Me.CurrentSession.VT_SelectedOrderItemGridRow_V2.Rows(0).Item("QtyOrdered")) - Val(Me.CurrentSession.VT_SelectedOrderItemGridRow_V2.Rows(0).Item("QtyDelivered"))
        dblWgtToSend = Val(Me.CurrentSession.VT_SelectedOrderItemGridRow_V2.Rows(0).Item("WgtOrdered")) - Val(Me.CurrentSession.VT_SelectedOrderItemGridRow_V2.Rows(0).Item("WgtDelivered"))

        If intUnitOfSale = 1 And dblQtyToSend = 0 Then
            strTemp = "The selected item is measured by Quantity and the Quantity selected is 0.\n\n"
            strTemp = strTemp + "This item cannot be sent for picking."
            'objDisp.DisplayMessage(Page, strTemp)
            lblMsg.Text = strTemp
            ModalPopupExtender3.Show()

        ElseIf intUnitOfSale = 0 And dblWgtToSend = 0 Then
            strTemp = "The selected item is measured by Weight and the Weight selected is 0.\n\n"
            strTemp = strTemp + "This item cannot be sent for picking."
            '    objDisp.DisplayMessage(Page, strTemp)
            lblMsg.Text = strTemp
            ModalPopupExtender3.Show()
        Else
            intTraceCodeId = 0  ' we don't know what TraceCode to use for the remaining stuff
            objTele.SendItemForPicking(lngSalesOrderId, lngSalesOrderItemId, dblQtyToSend, dblWgtToSend, intTraceCodeId)

            '  objDisp.DisplayMessage(Page, "The Order Item will be loaded on the Handheld device when it is next Synchronised!")
            lblMsg.Text = "The Order Item will be loaded on the Handheld device when it is next Synchronised!"
            ModalPopupExtender3.Show()
        End If

        FillItemOnHandheldGrid()

    End Sub

    Protected Sub btnPrint_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnPrint.Click

        Dim objCommonFuncs As New BPADotNetCommonFunctions.VT_CommonFunctions.CommonFunctions
        Dim strDiffOrderPrintouts As String = objCommonFuncs.GetConfigItem("DifferentOrderPrintouts")

        If UCase(strDiffOrderPrintouts) = "YES" Then
            Session("VT_FromPage") = "Details"

            Response.Redirect("~/Other_Pages/SelectPrintout.aspx")
        Else

            Dim strInvoiceMode As String = UCase(objCommonFuncs.GetConfigItem("TelesalesInvoiceDisplayMode"))

            If strInvoiceMode = "ASPX" Then

                '====================
                ' this code is used to generate an Sales Order based on an ASPX page instead of the original code which used DocFlex
                '=======================
                'Use Me.CurrentSession.VT_SalesOrderID as the parameter for filling out the data on the page.


                Dim strTemplate As String = objCommonFuncs.GetConfigItem("TelesalesOrderTemplate")
                If strTemplate = "" Then
                    lblMsg.Text = "No Sales Order template has been configured for this installation"
                    ModalPopupExtender3.Show()
                    Exit Sub
                End If

                Dim strTemplatePath As String = "FormTemplates\SalesOrders\" + strTemplate
                If System.IO.File.Exists(System.IO.Path.Combine(Server.MapPath("~"), strTemplatePath)) = False Then

                    lblMsg.Text = String.Format("The Sales Order template file [{0}] cannot be found.", strTemplatePath)
                    ModalPopupExtender3.Show()
                    Exit Sub

                End If
                Session("_VT_PageToReturnToAfterDisplay") = "~\TabPages\Details_Opening.aspx"
                Response.Redirect("~\" + strTemplatePath)
                '=======================

            Else

                Dim strTemplate As String = "SalesOrderLabel.htm"
                Dim strResolvedFile As String = "SalesOrderLabel_Resolved.htm"

                Dim objCO As New TTHTMLObjectCoordinator.Interface
                ' must change this to parameterise path
                Dim objEQO As New VT_eQOInterface.eQOInterface
                Dim strFile As String = Server.MapPath("~") + "\Reports\General\" + strTemplate
                Dim strNewFileLocation As String = "~\Reports\General\"

                Dim strRTParamValues As String = CStr(Me.CurrentSession.VT_SalesOrderID) + "~"
                Dim strNewFile As String

                strNewFile = objCO.ProcessHTMLTemplate(strFile, strRTParamValues, Session("_VT_BPA.NetConnString"))

                strNewFile = System.IO.Path.GetFileName(strNewFile)
                strNewFile = strNewFileLocation + strNewFile

                Response.Redirect(strNewFile)
            End If
        End If
    End Sub

    Protected Sub cmdOKConfirm_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles cmdOKConfirm.Click

        ' we will only get here if the Ok action is chosen on the Confirmation Dialog
        Dim objTele As New VTDBFunctions.VTDBFunctions.TelesalesFunctions
        Dim objProd As New VTDBFunctions.VTDBFunctions.ProductsFunctions
        Dim objDisp As New VT_Display.DisplayFuncs
        Dim lngSalesOrderId, lngSalesOrderItemId, lngProductId As Long
        Dim dblQtyToSend, dblWgtToSend As Double

        Dim intTraceCodeId, intUnitOfSale As Integer
        Dim dsSalesOrderItem As DataSet
        Dim strTemp As String
        Dim blnPCOnly As Boolean

        lngSalesOrderId = Me.CurrentSession.VT_SalesOrderID
        'If IsNothing(uwgOrderItems.DisplayLayout.ActiveRow) Then
        If IsNothing(Me.CurrentSession.VT_SelectedOrderItemGridRow_V2) Then
            '    objDisp.DisplayMessage(Page, "You must select an Item to Resend")
            lblMsg.Text = "You must select an Item to Resend"
            ModalPopupExtender3.Show()
            Exit Sub
        End If

        'ActiveRow = uwgOrderItems.DisplayLayout.ActiveRow
        'lngSalesOrderItemId = ActiveRow.Cells.FromKey("SalesOrderItemID").Value
        lngSalesOrderItemId = Me.CurrentSession.VT_SelectedOrderItemGridRow_V2.Rows(0).Item("SalesOrderItemID")

        If UCase(Me.CurrentSession.VT_OrderType) <> "CALL-OFF" Then
            If Trim(Me.CurrentSession.VT_SelectedOrderItemGridRow_V2.Rows(0).Item("Status")) = objTele.cOrderItemNew And lblOrderStatus.Text = "New" Then
                '  objDisp.DisplayMessage(Page, "You cannot Resend a New Item. It has already been sent for Picking")
                lblMsg.Text = "You cannot Resend a New Item. It has already been sent for Picking"
                ModalPopupExtender3.Show()
                Exit Sub
            End If
        End If

        If Trim(Me.CurrentSession.VT_SelectedOrderItemGridRow_V2.Rows(0).Item("Status")) = objTele.cOrderItemComplete Or _
             Trim(Me.CurrentSession.VT_SelectedOrderItemGridRow_V2.Rows(0).Item("Status")) = objTele.cOrderItemClosedPartiallyComplete Then
            '  objDisp.DisplayMessage(Page, "You cannot Resend a Closed Item.")
            lblMsg.Text = "You cannot Resend a Closed Item for picking."
            ModalPopupExtender3.Show()
            Exit Sub
        End If

        If objTele.HasItemAlreadyBeenSentforPicking(lngSalesOrderItemId) Then
            lblMsg.Text = "This item has already been sent for Picking."
            ModalPopupExtender3.Show()
            Exit Sub
        End If

        dsSalesOrderItem = objTele.GetOrderItemForId(lngSalesOrderItemId)
        lngProductId = dsSalesOrderItem.Tables(0).Rows(0).Item("ProductId")
        intUnitOfSale = objProd.GetUnitOfSale(lngProductId)

        dblQtyToSend = CInt(Val(txtPnlQtyToPick.Text))
        dblWgtToSend = Val(txtPnlWgtToPick.Text)

        If UCase(hdnPCOnly.Value) = "PCONLY" Then
            blnPCOnly = True
        Else
            blnPCOnly = False
        End If




        If intUnitOfSale = 1 And dblQtyToSend = 0 Then
            strTemp = "The selected item is measured by Quantity and the Quantity selected is 0.\n\n"
            strTemp = strTemp + "This item cannot be sent for picking."
            '   objDisp.DisplayMessage(Page, strTemp)
            lblMsg.Text = strTemp
            ModalPopupExtender3.Show()
        ElseIf intUnitOfSale = 0 And dblWgtToSend = 0 Then
            strTemp = "The selected item is measured by Weight and the Weight selected is 0.\n\n"
            strTemp = strTemp + "This item cannot be sent for picking."
            'objDisp.DisplayMessage(Page, strTemp)
            lblMsg.Text = strTemp
            ModalPopupExtender3.Show()
        Else
            intTraceCodeId = 0  ' we don't know what TraceCode to use for the remaining stuff
            objTele.SendItemForPicking(lngSalesOrderId, lngSalesOrderItemId, dblQtyToSend, dblWgtToSend, intTraceCodeId, , blnPCOnly)
            If blnPCOnly = True Then
                '   objDisp.DisplayMessage(Page, "The Order Item is now available to be fulfiled on the PC.")
                lblMsg.Text = "The Order Item is now available to be fulfiled on the PC."
                ModalPopupExtender3.Show()
            Else
                '   objDisp.DisplayMessage(Page, "The Order Item will be loaded on the Handheld device when it is next Synchronised!")
                lblMsg.Text = "The Order Item will be loaded on the Handheld device when it is next Synchronised!"
                ModalPopupExtender3.Show()
            End If

            Dim strAuditComment As String
            Dim objForms As New VT_Forms.Forms
            Dim strProductCode As String
            strProductCode = objProd.GetProductCode(lngProductId)



            ' write to the AuditLog
            strAuditComment = "Product Code: " & Trim(strProductCode) & " Qty: " & CStr(dblQtyToSend) & " Wgt: " & CStr(dblWgtToSend) & " sent to HH for picking"
            objForms.WriteToAuditLog(Me.CurrentSession.VT_JobID, "Telesales", PortalFunctions.Now(), Session("_VT_CurrentUserName"), Session("_VT_CurrentUserId"), strAuditComment, "Audit Record", "")
        End If



        FillItemOnHandheldGrid()


    End Sub

    Protected Sub btnViewProductDetails_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnViewProductDetails.Click

        'If IsNothing(uwgOrderItems.DisplayLayout.ActiveRow) Then
        'Else
        '    Dim lngId As Long = CType(uwgOrderItems.DisplayLayout.ActiveRow.Cells.FromKey("ProductID").Text, Long)

        '    Me.CurrentSession.VT_ProdSelectProdID = lngId

        '    Session("VT_FromPage") = "Details"

        '    Response.Redirect("~/Other_Pages/ProductStockDetails.aspx")
        'End If

        If Me.CurrentSession.VT_SelectedOrderItemGridRow_V2 IsNot Nothing Then
            Dim lngId As Long = CType(Me.CurrentSession.VT_SelectedOrderItemGridRow_V2.Rows(0).Item("ProductID"), Long)

            Me.CurrentSession.VT_ProdSelectProdID = lngId

            Session("VT_FromPage") = "Details"

            Response.Redirect("~/Other_Pages/ProductStockDetails.aspx")

        End If
    End Sub

    Function CheckAllowedToEdit() As Boolean
        Dim blnAllowed As Boolean
        Dim dtTemp As New DataTable
        Dim objDataPreserve As New BPADotNetCommonFunctions.VT_DataPreserve.DataPreserve

        blnAllowed = False

        'Check the order status, handheld status and Person permission

        If UCase(lblOrderStatus.Text) = "NEW" OrElse Left(UCase(lblOrderStatus.Text), 4) = "OPEN" Then

            dtTemp = objDataPreserve.GetWDGDataFromSession(wdgHandheldItems)
            If Not dtTemp Is Nothing Then
                If dtTemp.Rows.Count = 0 Then
                    blnAllowed = True
                End If
            End If

        End If

        CheckAllowedToEdit = blnAllowed

    End Function

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSave.Click

        Dim objTele As New VTDBFunctions.VTDBFunctions.TelesalesFunctions
        Dim objForms As New VT_Forms.Forms

        Dim dsOrder As DataSet
        Dim strComment As String
        Dim lngCustomerID As Long
        Dim lngCustomerContactID As Long
        Dim lngPersonLogging As Long
        Dim strTemp As String



        dsOrder = objTele.GetOrderForId(Me.CurrentSession.VT_SalesOrderID)
        strComment = IIf(IsDBNull(dsOrder.Tables(0).Rows(0).Item("Comment")), 0, dsOrder.Tables(0).Rows(0).Item("Comment"))
        lngCustomerID = IIf(IsDBNull(dsOrder.Tables(0).Rows(0).Item("CustomerId")), 0, dsOrder.Tables(0).Rows(0).Item("CustomerId"))
        lngCustomerContactID = IIf(IsDBNull(dsOrder.Tables(0).Rows(0).Item("CustomerContactId")), 0, dsOrder.Tables(0).Rows(0).Item("CustomerContactId"))
        lngPersonLogging = IIf(IsDBNull(dsOrder.Tables(0).Rows(0).Item("PersonLoggingOrder")), 0, dsOrder.Tables(0).Rows(0).Item("PersonLoggingOrder"))



        'Save change to delivery date
        Dim objTrace As New VTDBFunctions.VTDBFunctions.TraceDataFunctions

        objTrace.UpdateOrder(lngCustomerID, lngCustomerContactID, dteDeliveryDate.Value, lngPersonLogging, strComment, Me.CurrentSession.VT_SalesOrderID)


        'Save change to Priority
        Dim objProd As New ProductionFunctions.Production
        'priority is no longer set on the details tab. You can set it on the main orders grid or on the order entry pages
        'after discussion on 11/8/2015 it was decided that the priority item would no longer be saved in the systemID field in the
        'wfo_batch table. A new field is added called Priority

        ' objProd.UpdateJobPriority(Me.CurrentSession.VT_JobID, txtPriority.Text)

        Dim objBPA As New VTDBFunctions.VTDBFunctions.TelesalesFunctions

        objBPA.SaveOrderPO(Me.CurrentSession.VT_SalesOrderID, txtPO.Text)

        objBPA.SaveOrderComment(Me.CurrentSession.VT_SalesOrderID, txtComment.Text)


        ' 4. get the delivery customer name  and store it in ExtraData4
        Dim lngDeliveryCustForOrder As Long
        lngDeliveryCustForOrder = ddlDeliveryCustomer.SelectedValue

        strTemp = Trim(ddlDeliveryCustomer.SelectedItem.ToString)
        objForms.SetWFOExtraDataItem(Me.CurrentSession.VT_SalesOrderNum, "ExtraData4", strTemp)

        objTele.UpdateOrderDeliveryCustomer(Me.CurrentSession.VT_SalesOrderID, lngDeliveryCustForOrder)


    End Sub

    Sub DecideAllowAddOrderRow()
        btnAddToOrder.Visible = False

        If Me.CurrentSession.VT_OrderTypesEnabled <> "YES" Then
            Me.CurrentSession.VT_OrderType = ""
        End If

        'If the status is New or open then you can add a row to it.
        If UCase(lblOrderStatus.Text) = "NEW" OrElse Left(UCase(lblOrderStatus.Text), 4) = "OPEN" Then

            Select Case UCase(Me.CurrentSession.VT_OrderType)
                Case "CALL-OFF"
                    btnAddToOrder.Visible = True
                Case "FLEXI"
                    btnAddToOrder.Visible = True
                Case Else
            End Select
        Else
        End If

    End Sub

    Sub DecideAllowEditOrderRow(ByVal lngOrderItemID As Long)

        Dim blnAllowEdit As Boolean
        Dim strItemHHStatus As String
        btnEditOrderItem.Visible = False
        If Me.CurrentSession.VT_OrderTypesEnabled <> "YES" Then
            Me.CurrentSession.VT_OrderType = ""
        End If

        blnAllowEdit = False
        'If the status is New or open then you can edit it a row to it.

        If UCase(lblOrderStatus.Text) = "NEW" OrElse Left(UCase(lblOrderStatus.Text), 4) = "OPEN" Then


            Dim objCommonFuncs As New BPADotNetCommonFunctions.VT_CommonFunctions.CommonFunctions
            Dim strPublished As String = objCommonFuncs.GetConfigItem("IsPublished")

            If UCase(strPublished) = "YES" Then

                Dim dtTrans As New DataTable
                Dim objSO As New SalesOrdersFunctions.SalesOrders
                Dim drItemRows() As DataRow
                Dim strQuery As String
                Dim dsSalesOrderRow As New DataSet
                Dim objTele As New VTDBFunctions.VTDBFunctions.TelesalesFunctions

                dsSalesOrderRow = objTele.GetOrderItemForId(lngOrderItemID)
                ' get the transactions from the trc_Transaction table for this OrderItem 
                dtTrans = objSO.GetItemsOnHandheld(Me.CurrentSession.VT_JobID)

                strQuery = "ItemNumber = " & CStr(lngOrderItemID)

                drItemRows = dtTrans.Select(strQuery)

                If UBound(drItemRows) < 0 Then
                    strItemHHStatus = "NOTONHH"

                Else
                    If UCase(drItemRows(0).Item("Synched")) <> "YES" Then
                        strItemHHStatus = "NOTSYNCHED"
                    Else
                        strItemHHStatus = "SYNCHED"
                    End If
                End If

                Select Case UCase(Me.CurrentSession.VT_OrderType)
                    Case "CALL-OFF"
                        Select Case strItemHHStatus
                            Case "NOTONHH"  'Not on HH
                                blnAllowEdit = True
                            Case "NOTSYNCHED"  'Not Synched
                                blnAllowEdit = True
                            Case "SYNCHED"     'Synched
                                blnAllowEdit = True
                        End Select
                    Case "FLEXI"
                        Select Case strItemHHStatus
                            Case "NOTONHH"  'Not on HH
                                blnAllowEdit = True
                            Case "NOTSYNCHED"  'Not Synched
                                blnAllowEdit = True
                            Case "SYNCHED"     'Synched
                                blnAllowEdit = True
                        End Select
                    Case Else
                        Dim strRowStatus As String
                        If dsSalesOrderRow.Tables(0).Rows.Count > 0 Then
                            strRowStatus = Trim(dsSalesOrderRow.Tables(0).Rows(0).Item("Status"))
                        Else
                            strRowStatus = "Not Found"
                        End If

                        If Left(UCase(strRowStatus), 6) = "CLOSED" Then
                            blnAllowEdit = False
                        Else
                            Select Case strItemHHStatus
                                Case "NOTONHH"  'Not on HH
                                    blnAllowEdit = True
                                Case "NOTSYNCHED"  'Not Synched
                                    blnAllowEdit = True
                                Case "SYNCHED"     'Synched
                                    blnAllowEdit = False
                            End Select
                        End If
                End Select
            Else
                blnAllowEdit = True
            End If
        End If


        'Hde all the editing features for pre-issued orders
        If Trim(lblOrderStatus.Text) = "Open - Pre Issued" Then
            blnAllowEdit = False
        End If

        If blnAllowEdit = True Then
            btnEditOrderItem.Visible = True
        Else
            btnEditOrderItem.Visible = False
        End If


    End Sub


    Protected Sub cmdOKConfirmEdit_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles cmdOKConfirmEdit.Click

        Dim objSO As New SalesOrdersFunctions.SalesOrders
        Dim objTele As New VTDBFunctions.VTDBFunctions.TelesalesFunctions
        Dim lngProductID As Long
        Dim intUnitOfSale As Integer
        Dim objProd As New VTDBFunctions.VTDBFunctions.ProductsFunctions
        Dim lngOrderItemID As Long
        Dim dblNewQty, dblNewWgt, dblQtyDelivered, dblWgtDelivered As Double
        Dim dblNewPrice, dblTotalPrice, dblVAT, dblVATPer As Double
        Dim dblTotalPriceAdd, dblVATAdd As Double
        Dim ds As New Data.DataSet
        Dim objDisp As New VT_Display.DisplayFuncs
        Dim objTraceData As New VTDBFunctions.VTDBFunctions.TraceDataFunctions
        Dim dblDifference As Double
        Dim dblQtyDifference As Double
        Dim dblWgtDifference As Double
        Dim dblDeliveryDifference As Double

        Const DoNothing As String = "DoNothing"
        Const AddPos As String = "AddPos"
        Const AddNeg As String = "AddNeg"
        Const UpdateItem As String = "Update"

        Dim strItemHHStatus As String
        Dim strAddOrEditSOItem As String
        Dim strAddOrEditHH As String

        strAddOrEditHH = DoNothing
        strAddOrEditSOItem = DoNothing


        Dim objCommonFuncs As New BPADotNetCommonFunctions.VT_CommonFunctions.CommonFunctions
        Dim strPublished As String = objCommonFuncs.GetConfigItem("IsPublished")



        'Update telesales order item
        'lngProductID = uwgOrderItems.DisplayLayout.ActiveRow.Cells.FromKey("ProductId").Value
        'intUnitOfSale = objProd.GetUnitOfSale(lngProductID)
        'lngOrderItemID = uwgOrderItems.DisplayLayout.ActiveRow.Cells.FromKey("SalesOrderItemID").Value

        lngProductID = Me.CurrentSession.VT_SelectedOrderItemGridRow_V2.Rows(0).Item("ProductId")
        intUnitOfSale = objProd.GetUnitOfSale(lngProductID)
        lngOrderItemID = Me.CurrentSession.VT_SelectedOrderItemGridRow_V2.Rows(0).Item("SalesOrderItemID")


        ds = objTele.GetOrderItemForId(lngOrderItemID)


        If txtPnlNewOrderQty.Text <> "" Then
            dblNewQty = CDbl(CInt(txtPnlNewOrderQty.Text))
        Else
            dblNewQty = ds.Tables(0).Rows(0).Item("QuantityRequested")
        End If
        If intUnitOfSale <> 1 Then
            If txtPnlNewOrderWgt.Text <> "" Then
                dblNewWgt = CDbl(txtPnlNewOrderWgt.Text)
            Else
                dblNewWgt = ds.Tables(0).Rows(0).Item("WeightRequested")
            End If
        Else
            dblNewWgt = ds.Tables(0).Rows(0).Item("WeightRequested")
        End If

        If intUnitOfSale = 1 Then
            dblDifference = dblNewQty - ds.Tables(0).Rows(0).Item("QuantityRequested")
            dblQtyDifference = dblDifference
            dblWgtDifference = 0
        Else
            dblDifference = dblNewWgt - ds.Tables(0).Rows(0).Item("WeightRequested")
            dblQtyDifference = dblNewQty - ds.Tables(0).Rows(0).Item("QuantityRequested")
            dblWgtDifference = dblDifference
        End If

        dblQtyDelivered = ds.Tables(0).Rows(0).Item("Quantity")
        dblWgtDelivered = ds.Tables(0).Rows(0).Item("Weight")


        ' Check what to update or add
        strItemHHStatus = "NOTONHH"

        If UCase(strPublished) = "YES" Then

            Dim dtTrans As New DataTable
            Dim drItemRows() As DataRow
            Dim strQuery As String
            Dim dblWgtOnHH As Double
            Dim dblQtyOnHH As Double

            ' get the transactions from the trc_Transaction table for this OrderItem 
            dtTrans = objSO.GetItemsOnHandheld(Me.CurrentSession.VT_JobID)

            strQuery = "ItemNumber = " & CStr(lngOrderItemID)

            drItemRows = dtTrans.Select(strQuery)




            If UBound(drItemRows) < 0 Then
                strItemHHStatus = "NOTONHH"

            Else
                If UCase(drItemRows(0).Item("Synched")) <> "YES" Then
                    strItemHHStatus = "NOTSYNCHED"
                Else
                    strItemHHStatus = "SYNCHED"
                    dblQtyOnHH = drItemRows(0).Item("OrderedQty")
                    dblWgtOnHH = drItemRows(0).Item("OrderedWgt")
                End If
            End If

            Select Case UCase(Me.CurrentSession.VT_OrderType)
                Case "CALL-OFF"
                    If dblDifference < 0 Then ' Reducing requested
                        'Select Case strItemHHStatus
                        '    Case "NOTONHH"  'Not on HH
                        '        strAddOrEditSOItem = UpdateItem
                        '        strAddOrEditHH = DoNothing
                        '    Case "NOTSYNCHED"  'Not Synched
                        '        strAddOrEditSOItem = UpdateItem
                        '        strAddOrEditHH = DoNothing
                        '    Case "SYNCHED"     'Synched
                        strAddOrEditSOItem = UpdateItem
                        strAddOrEditHH = DoNothing

                        ' End Select
                    Else
                        strAddOrEditSOItem = UpdateItem
                        strAddOrEditHH = DoNothing
                    End If

                Case "FLEXI"
                    If dblDifference < 0 Then ' Reducing requested
                        Select Case strItemHHStatus
                            Case "NOTONHH"  'Not on HH
                                strAddOrEditSOItem = UpdateItem

                                'If the New amount is less that what has been delivered/picked then adjust 
                                'by the change in the delivery rather than the ordered amount

                                If intUnitOfSale = 1 Then
                                    dblDeliveryDifference = dblNewQty - dblQtyDelivered
                                    If dblDeliveryDifference < 0 Then
                                        dblQtyDifference = dblDeliveryDifference
                                        dblWgtDifference = 0
                                        strAddOrEditHH = AddNeg
                                    Else
                                        strAddOrEditHH = DoNothing
                                    End If
                                Else
                                    dblDeliveryDifference = dblNewWgt - dblWgtDelivered
                                    If dblDeliveryDifference < 0 Then
                                        dblWgtDelivered = dblDeliveryDifference
                                        dblQtyDifference = dblNewQty - dblQtyDelivered
                                        strAddOrEditHH = AddNeg
                                    Else
                                        strAddOrEditHH = DoNothing
                                    End If
                                End If

                            Case "NOTSYNCHED"  'Not Synched
                                strAddOrEditSOItem = UpdateItem
                                strAddOrEditHH = UpdateItem
                            Case "SYNCHED"     'Synched
                                strAddOrEditSOItem = UpdateItem
                                strAddOrEditHH = AddNeg
                        End Select
                    Else
                        Select Case strItemHHStatus
                            Case "NOTONHH"  'Not on HH
                                strAddOrEditSOItem = UpdateItem
                                strAddOrEditHH = DoNothing
                            Case "NOTSYNCHED"  'Not Synched
                                strAddOrEditSOItem = UpdateItem
                                strAddOrEditHH = UpdateItem
                            Case "SYNCHED"     'Synched
                                strAddOrEditSOItem = UpdateItem
                                strAddOrEditHH = AddPos
                        End Select
                    End If
                Case Else
                    If dblDifference < 0 Then ' Reducing requested
                        Select Case strItemHHStatus
                            Case "NOTONHH"  'Not on HH
                                strAddOrEditSOItem = UpdateItem
                                strAddOrEditHH = DoNothing
                            Case "NOTSYNCHED"  'Not Synched
                                strAddOrEditSOItem = UpdateItem
                                strAddOrEditHH = UpdateItem
                            Case "SYNCHED"     'Synched This option won't happen as button is not available here.
                                strAddOrEditSOItem = DoNothing
                                strAddOrEditHH = DoNothing
                        End Select
                    Else
                        Select Case strItemHHStatus
                            Case "NOTONHH"  'Not on HH
                                strAddOrEditSOItem = UpdateItem
                                strAddOrEditHH = DoNothing
                            Case "NOTSYNCHED"  'Not Synched
                                strAddOrEditSOItem = UpdateItem
                                strAddOrEditHH = UpdateItem
                            Case "SYNCHED"     'Synched This option won't happen as button is not available here.
                                strAddOrEditSOItem = DoNothing
                                strAddOrEditHH = DoNothing
                        End Select
                    End If
            End Select
        Else
            strAddOrEditHH = DoNothing
            strAddOrEditSOItem = UpdateItem
        End If





        dblNewPrice = ds.Tables(0).Rows(0).Item("UnitPrice")

        If intUnitOfSale = 1 Then
            If ds.Tables(0).Rows(0).Item("VAT") <> 0 Then
                dblVATPer = ds.Tables(0).Rows(0).Item("VAT") / ds.Tables(0).Rows(0).Item("QuantityRequested")
                dblVAT = dblVATPer * dblNewQty
            Else
                dblVAT = 0
            End If

            dblTotalPrice = (dblNewQty * dblNewPrice) + dblVAT
        Else
            If ds.Tables(0).Rows(0).Item("VAT") <> 0 Then
                dblVATPer = ds.Tables(0).Rows(0).Item("VAT") / ds.Tables(0).Rows(0).Item("WeightRequested")
                dblVAT = dblVATPer * dblNewWgt
            Else
                dblVAT = 0
            End If

            dblTotalPrice = (dblNewWgt * dblNewPrice) + dblVAT

        End If


        If strAddOrEditHH = AddNeg Or strAddOrEditHH = AddPos = True Then

            If intUnitOfSale = 1 Then
                If strAddOrEditHH = AddNeg Then
                    dblQtyDifference = dblQtyDifference * -1

                End If
                If ds.Tables(0).Rows(0).Item("VAT") <> 0 Then
                    dblVATPer = ds.Tables(0).Rows(0).Item("VAT") / ds.Tables(0).Rows(0).Item("QuantityRequested")
                    dblVATAdd = dblVATPer * CInt(dblQtyDifference)
                Else
                    dblVATAdd = 0
                End If

                dblTotalPriceAdd = (CInt(dblQtyDifference) * dblNewPrice) + dblVATAdd
            Else
                If strAddOrEditHH = AddNeg Then
                    dblQtyDifference = dblQtyDifference * -1
                    dblWgtDifference = dblWgtDifference * -1
                End If
                If ds.Tables(0).Rows(0).Item("VAT") <> 0 Then
                    dblVATPer = ds.Tables(0).Rows(0).Item("VAT") / ds.Tables(0).Rows(0).Item("WeightRequested")
                    dblVATAdd = dblVATPer * dblWgtDifference
                Else
                    dblVATAdd = 0
                End If

                dblTotalPriceAdd = (dblWgtDifference * dblNewPrice) + dblVATAdd

            End If
        End If

        'Change or update the item in the sales order
        Select Case strAddOrEditSOItem
            Case UpdateItem
                objTele.UpdateOrderItem(lngOrderItemID, dblNewQty, dblNewWgt, dblQtyDelivered, dblWgtDelivered, dblNewPrice, dblTotalPrice, dblVAT)
            Case AddNeg
            Case AddPos
            Case DoNothing

        End Select

        'change or add an item on the handheld
        Select Case strAddOrEditHH
            Case UpdateItem
                'Update HH item if not synched.
                Dim dsItem As New DataSet
                dsItem = objTraceData.GetTDSalesOrderItem(lngOrderItemID)

                With dsItem.Tables(0).Rows(0)
                    objTraceData.UpdateSOToTrace(.Item("PONumber"), .Item("OrderDate"), .Item("CustomerID"), 0, _
                                                 .Item("ProductID"), dblNewWgt, CLng(dblNewQty), _
                        False, .Item("OrderNumber"), 0, .Item("DriverID"), 6, .Item("CommodityCode"), lngOrderItemID, _
                        .Item("SaleType"), .Item("TraceCode"), dblVAT, dblTotalPrice - dblVAT, .Item("DeliveryCustomerID"), _
                        .Item("Location"), .Item("ChargedByType"))

                End With

            Case AddNeg
                Dim dsItem As New DataSet
                Dim dsTSItem As New DataSet
                Dim strProductCode As String
                Dim dsTSOrder As New DataSet


                If strItemHHStatus = "NOTONHH" Then 'Not on HH

                    dsTSItem = objTele.GetOrderItemForId(lngOrderItemID)
                    dsTSOrder = objTele.GetOrderForId(dsTSItem.Tables(0).Rows(0).Item("SalesOrderId"))
                    strProductCode = objProd.GetProductCode(lngProductID)

                    objTraceData.InsertSOToTrace(dsTSOrder.Tables(0).Rows(0).Item("CustomerPO"), dsTSOrder.Tables(0).Rows(0).Item("DateCreated"), _
                                                 dsTSOrder.Tables(0).Rows(0).Item("CustomerID"), 0, strProductCode, dblWgtDifference, _
                                                 CLng(dblQtyDifference), False, dsTSOrder.Tables(0).Rows(0).Item("SalesOrderNum"), 0, _
                                                 0, 6, "", lngOrderItemID, 2, "", dblVATAdd, dblTotalPriceAdd - dblVATAdd, _
                                                 dsTSOrder.Tables(0).Rows(0).Item("DeliveryCustomerId"), 0, dsTSItem.Tables(0).Rows(0).Item("ChargedByType"))

                Else
                    dsItem = objTraceData.GetTDSalesOrderItem(lngOrderItemID)

                    With dsItem.Tables(0).Rows(0)
                        objTraceData.InsertSOToTrace(.Item("PONumber"), .Item("OrderDate"), .Item("CustomerID"), 0, _
                                                      .Item("ProductID"), dblWgtDifference, CLng(dblQtyDifference), _
                           False, .Item("OrderNumber"), 0, .Item("DriverID"), 6, .Item("CommodityCode"), lngOrderItemID, _
                           2, .Item("TraceCode"), dblVATAdd, dblTotalPriceAdd - dblVATAdd, .Item("DeliveryCustomerID"), _
                          .Item("Location"), .Item("ChargedByType"))
                    End With
                End If
            Case AddPos
                Dim dsItem As New DataSet
                dsItem = objTraceData.GetTDSalesOrderItem(lngOrderItemID)

                With dsItem.Tables(0).Rows(0)
                    objTraceData.InsertSOToTrace(.Item("PONumber"), .Item("OrderDate"), .Item("CustomerID"), 0, _
                                                  .Item("ProductID"), dblWgtDifference, CLng(dblQtyDifference), _
                       False, .Item("OrderNumber"), 0, .Item("DriverID"), 6, .Item("CommodityCode"), lngOrderItemID, _
                        .Item("SaleType"), .Item("TraceCode"), dblVATAdd, dblTotalPriceAdd - dblVATAdd, .Item("DeliveryCustomerID"), _
                      .Item("Location"), .Item("ChargedByType"))
                End With

            Case DoNothing

        End Select

        'Update telesales totals
        objSO.UpdateOrderTotal(Me.CurrentSession.VT_SalesOrderID, Me.CurrentSession.VT_SalesOrderNum)

        Dim objCommon As New BPADotNetCommonFunctions.VT_Forms.Forms
        Dim strAuditLog As String
        If intUnitOfSale = 1 Then
            strAuditLog = "Ordered Amount change in Product: " & CStr(lblPnlProductNameEdit.Text) & ", From Qty: " & CStr(lblPnlOrderedQtyEdit.Text) & " & to New Value Qty: " & CStr(txtPnlNewOrderQty.Text)
        Else
            strAuditLog = "Ordered Amount change in Product: " & CStr(lblPnlProductNameEdit.Text) & ", From Qty: " & CStr(lblPnlOrderedQtyEdit.Text) & " & Wgt: " & CStr(lblPnlOrderedWgtEdit.Text) & " & to New Values Qty: " & CStr(txtPnlNewOrderQty.Text) & " & Wgt: " & CStr(txtPnlNewOrderWgt.Text)
        End If
        objCommon.WriteToAuditLog(Me.CurrentSession.VT_JobID, "Telesales", PortalFunctions.Now, Session("_VT_CurrentUserName"),
                                    Session("_VT_CurrentUserId"), strAuditLog, "Audit Record", "", "ASP")

        Dim strMessage As String

        Select Case strAddOrEditHH
            Case UpdateItem
                strMessage = "The Handheld Item has also been changed."
            Case AddNeg
                strMessage = "The reduction has been sent to the handheld(s), as a Return line item. The handheld(s) will pick it up at the next synchronise."
            Case AddPos
                strMessage = "The additional amount has been sent to the handheld(s), as a New line item. The handheld(s) will pick it up at the next synchronise."
            Case DoNothing
                strMessage = "The change you made has not been sent to the handheld. You may want to manually send the edited item for picking."
        End Select

        '      objDisp.DisplayMessage(Page, strMessage)

        lblMsg.Text = strMessage
        ModalPopupExtender3.Show()


        FillGrid()
        FillItemOnHandheldGrid()

    End Sub



    Protected Sub btnAddToOrder_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAddToOrder.Click
        Session("VT_FromPage") = "Details"

        '  Response.Redirect("~/Other_Pages/AddProduct.aspx")

        Response.Redirect("~/Other_Pages/AddProductToOrder.aspx")
    End Sub





    Protected Sub btnAttachments_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAttachments.Click
        Dim strVTStore As String = Replace(ConfigurationManager.AppSettings("VTStore"), "XXXX", Session("_VT_CurrentDID"))
        Dim StartNodeText As String = "SalesOrders\OrderNum_" + Me.CurrentSession.VT_SalesOrderNum.ToString
        strVTStore = strVTStore + "&Auth=Ok&UID=" + Session("_VT_CurrentUserId").ToString + "&ShowBanner=No&StartNodeText=" + StartNodeText + "&UserName=" + Session("_VT_CurrentUserName")


        Session("_VT_PageToReturnToFromModulesIFrame") = "~/TabPages/Details_Opening.aspx"


        Me.CurrentSession.VT_ProjectInIFrame = strVTStore
        Response.Redirect("~/IFrame Pages/ModulesIFrame.aspx")
    End Sub



    Private Sub SetupDeliveryCustDropdown()
        '   Dim dsCust As New DataSet
        Dim dsContacts As New DataSet
        Dim objCust As New VTDBFunctions.VTDBFunctions.CustomerFunctions
        Dim lngBillingCustId As Long
        Dim objBPA As New VT_CommonFunctions.CommonFunctions
        Dim j As Integer

        lngBillingCustId = Me.CurrentSession.VT_CustomerID

        '    dsCust = objCust.GetCustomerDetailsForId(lngBillingCustId)

        Dim dsCustomersForBilling As New DataSet
        Dim i As Integer
        Dim intIndex As Integer

        'if the checkbox is ticked then show only the customers that have the selected cust as billing customer
        'only do this when the billing customer is selected. NOT the delivery one
        'If chkDeliveryCustsOnly.Checked = True Then

        '    ddlDeliveryCustomer.Items.Clear()

        '    dsCustomersForBilling = objCust.GetCustomersForBillingCust(lngBillingCustId)

        '    Dim dtCustClone As DataTable = dsCustomersForBilling.Tables(0).Clone
        '    dtCustClone.Rows.Clear()

        '    Dim drNewRoww0 As DataRow = dtCustClone.NewRow()
        '    drNewRoww0.Item("CustomerId") = lngBillingCustId
        '    drNewRoww0.Item("CustomerName") = ddlBillingCustomer.SelectedItem.Text
        '    dtCustClone.Rows.Add(drNewRoww0)

        '    For i = 0 To dsCustomersForBilling.Tables(0).Rows.Count - 1
        '        Dim drNew As DataRow = dtCustClone.NewRow()
        '        drNew.Item("CustomerId") = Trim(dsCustomersForBilling.Tables(0).Rows(i).Item("CustomerId"))

        '        If Me.CurrentSession.VT_inclCustomerCodes = True Then
        '            drNew.Item("CustomerName") = Trim(dsCustomersForBilling.Tables(0).Rows(i).Item("CustomerReference")) & " :: " & Trim(dsCustomersForBilling.Tables(0).Rows(i).Item("CustomerName"))
        '        Else
        '            drNew.Item("CustomerName") = Trim(dsCustomersForBilling.Tables(0).Rows(i).Item("CustomerName"))
        '        End If

        '        dtCustClone.Rows.Add(drNew)
        '    Next

        '    ddlDeliveryCustomer.DataSource = dtCustClone
        '    ddlDeliveryCustomer.DataTextField = "CustomerName"
        '    ddlDeliveryCustomer.DataValueField = "CustomerId"
        '    ddlDeliveryCustomer.DataBind()

        '    'If ddlBillingCustomer.SelectedItem.Text <> "" Then
        '    '    ddlDeliveryCustomer.SelectedValue = ddlDeliveryCustomer.Items.FindByText(ddlBillingCustomer.SelectedItem.Text).Value
        '    'End If
        'Else
        dsCustomersForBilling = objCust.GetAllCustomers
        With ddlDeliveryCustomer
            .Items.Clear()
            Dim dtCustClone As DataTable = dsCustomersForBilling.Tables(0).Clone
            dtCustClone.Rows.Clear()


            For i = 0 To dsCustomersForBilling.Tables(0).Rows.Count - 1
                Dim drNew As DataRow = dtCustClone.NewRow()
                drNew.Item("CustomerId") = Trim(dsCustomersForBilling.Tables(0).Rows(i).Item("CustomerId"))
                If Me.CurrentSession.VT_inclCustomerCodes = True Then
                    drNew.Item("CustomerName") = Trim(dsCustomersForBilling.Tables(0).Rows(i).Item("CustomerReference")) & " :: " & Trim(dsCustomersForBilling.Tables(0).Rows(i).Item("CustomerName"))
                Else
                    drNew.Item("CustomerName") = Trim(dsCustomersForBilling.Tables(0).Rows(i).Item("CustomerName"))
                End If
                dtCustClone.Rows.Add(drNew)
            Next

            .DataSource = dtCustClone
            .DataTextField = "CustomerName"
            .DataValueField = "CustomerId"
            .DataBind()

            'if we are showing the customer code as well as the name then this will cause an error here
            Dim strCode As String
            Dim lngid As Long



            If lblDeliveryCustomer.Text <> "" Then
                If Me.CurrentSession.VT_inclCustomerCodes = True Then
                    lngid = objCust.GetCustomerIdForName(lblDeliveryCustomer.Text)
                    strCode = objCust.GetCustomerRefForId(lngid)
                    If lngid > 0 Then
                        .SelectedValue = .Items.FindByText(Trim(strCode) & " :: " & Trim(lblDeliveryCustomer.Text)).Value
                    End If

                Else
                    .SelectedValue = .Items.FindByText(lblDeliveryCustomer.Text).Value
                End If

            End If



        End With
        '   End If

    End Sub



    Protected Sub ddlDeliveryCustomer_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlDeliveryCustomer.SelectedIndexChanged
        Dim dsCust As New DataSet
        Dim objCust As New VTDBFunctions.VTDBFunctions.CustomerFunctions
        Dim lngDeliveryCustId As Long
        Dim dsDelContacts As New DataSet

        lngDeliveryCustId = ddlDeliveryCustomer.SelectedValue
        dsCust = objCust.GetCustomerDetailsForId(lngDeliveryCustId)
        If dsCust.Tables(0).Rows.Count > 0 Then
            With dsCust.Tables(0).Rows(0)
                lblCustAddress.Text = Trim(IIf(IsDBNull(.Item("DeliveryAddress")), "", .Item("DeliveryAddress")))
            End With

        End If
    End Sub

    Protected Sub Page_PreRender(sender As Object, e As EventArgs) Handles Me.PreRender

        ''Set up any column formating requirements here
        'Dim strCurrency As String
        'Dim strTemp As String
        'Dim ci As Globalization.CultureInfo
        'ci = Globalization.CultureInfo.CurrentCulture
        'strCurrency = ci.NumberFormat.CurrencySymbol

        ''strTemp = wdgOrderItems.Columns("UnitPrice").Header.Text
        ''wdgOrderItems.Columns("UnitPrice").Header.Text = strTemp & " " & strCurrency & " "


        'Dim groupField As New GroupField()
        'groupField = wdgOrderItems.Columns("GroupField_PriceDetails")
        'strTemp = groupField.Header.Text
        'groupField.Header.Text = strTemp & " " & strCurrency & " "
        'groupField.Header.CssClass = "VerifyGrid_Report_HeaderGreen"

        ''strTemp = groupField.Columns("UnitPrice").Header.Text
        ''groupField.Columns("UnitPrice").Header.Text = strTemp & " " & strCurrency & " "
        ''groupField.Columns("UnitPrice").Header.CssClass = "VerifyGrid_Report_HeaderGold"

        ''strTemp = groupField.Columns("VAT").Header.Text
        ''groupField.Columns("VAT").Header.Text = strTemp & " " & strCurrency & " "

        ''strTemp = groupField.Columns("Total").Header.Text
        ''groupField.Columns("Total").Header.Text = strTemp & " " & strCurrency & " "


        'wdgOrderItems.Behaviors.SummaryRow.ShowSummariesButtons = False
        'For Each setting As SummaryRowSetting In wdgOrderItems.Behaviors.SummaryRow.ColumnSettings
        '    setting.ShowSummaryButton = False
        'Next



    End Sub


    Protected Sub btnAuditLog_Click(sender As Object, e As EventArgs) Handles btnAuditLog.Click
        Me.CurrentSession.OptionsPageToReturnTo = "Details_Opening.aspx"
        Response.Redirect("Audit_Opening.aspx")

    End Sub





    Protected Sub btnCommentVIEW_Click(sender As Object, e As EventArgs) Handles btnCommentVIEW.Click

        ' check for expired session
        If Session("_VT_SessionLive") <> "YES" Then
            ' if it has expired display a message and exit
            Dim objDisp As New VT_Display.DisplayFuncs
            objDisp.DisplayMessage(Page, VT_Constants.cTimeOutMessage)
            Exit Sub
        End If

        ' jump to the comments page

        Me.CurrentSession.OptionsPageToReturnTo = "~/TabPages/Details_Opening.aspx"


        Response.Redirect("~/Comments_Pages/OrderFormComments.aspx")
    End Sub


    Protected Sub btnOrderDelivered_Click(sender As Object, e As EventArgs) Handles btnOrderDelivered.Click
        Dim lngSalesOrderId, lngSalesOrderItemId As Long
        Dim intTraceCodeId, intSalesOrderNumber As Integer
        Dim dsOrder As New DataSet
        Dim dsOrderItem As New DataSet
        Dim objTele As New VTDBFunctions.VTDBFunctions.TelesalesFunctions
        Dim strOrderStatus, strOrderItemStatus, strAuditComment As String
        Dim objDisp As New VT_Display.DisplayFuncs
        Dim MyRow As DataRow
        Dim objForms As New VT_Forms.Forms
        Dim dblQtyWeight As Double
        Dim lngProduct As Long
        Dim objProduct As New VTDBFunctions.VTDBFunctions.ProductsFunctions
        Dim dtTemp As New DataTable
        Dim objDataPreserve As New BPADotNetCommonFunctions.VT_DataPreserve.DataPreserve
        Dim objSales As New BPADotNetCommonFunctions.SalesFunctions.Sales_Orders

        Dim strConn As String = Session("_VT_DotNetConnString")


        lngSalesOrderId = Me.CurrentSession.VT_SalesOrderID
        ' get the details of the original Sales Order
        dsOrder = objTele.GetOrderForId(lngSalesOrderId)
        strOrderStatus = Trim(dsOrder.Tables(0).Rows(0).Item("Status"))
        If strOrderStatus = "Closed - Delivered" Then
            '   objDisp.DisplayMessage(Page, "This order is already closed!")
            lblMsg.Text = "This order is already delivered!"
            ModalPopupExtender3.Show()
            Exit Sub
        End If

        If strOrderStatus <> TelesalesFunctions.cOrderClosedPartiallyComplete And strOrderStatus <> TelesalesFunctions.cOrderComplete And strOrderStatus <> TelesalesFunctions.cOrderOpenPartShipped Then
            '   objDisp.DisplayMessage(Page, "This order is already closed!")
            lblMsg.Text = "Only orders that have been dispatched can be marked as delivered! This order has not yet been dispatched"
            ModalPopupExtender3.Show()
            Exit Sub
        End If

        'save true value to this variable so that the status will update on the orders grid
        Me.CurrentSession.blnOrderStatusChanged = True

        ' now close the order
        lblOrderStatus.Text = "Closed - Delivered"

        ' Also write the Order status to the Job table.
        objForms.SetJobStatusText("Closed - Delivered", Me.CurrentSession.VT_JobID)
        objTele.UpdateOrderStatus(lngSalesOrderId, "Closed - Delivered")

        ' Delete the items for this order from the Handheld table
        intSalesOrderNumber = dsOrder.Tables(0).Rows(0).Item("SalesOrderNum")
        objTele.DeleteHandheldSalesOrder(intSalesOrderNumber)

        ' write to the AuditLog
        strAuditComment = "Order has been delivered"
        'objForms.WriteToAuditLog(intSalesOrderNumber, "Telesales", Now(), Session("_VT_CurrentUserName"), Session("_VT_CurrentUserId"), strAuditComment, "Audit Record", "")

        'if steripack then use this logging:
        Dim strType As String
        ' strAuditComment = "New Order created"
        strType = "Sales Order"
        objForms.WriteToAuditLog(intSalesOrderNumber, strType, PortalFunctions.Now, Session("_VT_CurrentUserName"), Session("_VT_CurrentUserId"), strAuditComment, "Audit Record", "SysAdmin")


        'save the date closed
        objSales.SaveGeneralItemsToSalesMatrix("DateDelivered", "Details", PortalFunctions.Now.ToString("s"), intSalesOrderNumber, strConn)

        FillItemOnHandheldGrid()

        FillGrid()

        Me.CurrentSession.blnOrderStatusChanged = True


        '   objDisp.DisplayMessage(Page, "This order has been closed!")
        lblMsg.Text = "This order has been marked as delivered"
        ModalPopupExtender3.Show()

    End Sub


    Protected Sub btnShipping_Click(sender As Object, e As EventArgs) Handles btnShipping.Click
        Response.Redirect("ShippingDetails.aspx")
    End Sub
End Class
