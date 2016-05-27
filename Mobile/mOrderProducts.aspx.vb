Imports BPADotNetCommonFunctions
Imports BPADotNetCommonFunctions.VT_CommonFunctions.MatrixFunctions
Imports System.Data.SqlClient
Imports System.Data
Imports System.Drawing
Imports System.Web.UI
Imports Infragistics.Web.UI.GridControls

Partial Class Mobile_mOrderProducts
    Inherits MyBasePage
    Public Function SaveNewOrder(ByRef strMessage As String) As Long


        'At this point we have saved a new Order so we can Sign it out to Current User
        Dim blnDetermineCheckOutStatus As Boolean = False
        Dim intIdToUse As Integer = 0
        Dim strFormLock As String = ""
        Dim objPres As New VT_DataPreserve.DataPreserve
        Dim objVTM As New VTDBFunctions.VTDBFunctions.VTMatrixFunctions
        Dim objData As New BPADotNetCommonFunctions.VT_DataPreserve.DataPreserve

        intIdToUse = CurrentSession.VT_NewOrderMatrixID

        Me.CurrentSession.VT_FormCheckedOutToCurrentUser = True
        objVTM.LockFormForWriting(Session("_VT_DotNetConnString"), intIdToUse, Me.CurrentSession.FormDataTable, objData.CreateFormLockString(Session("_VT_CurrentUserName"), Session("_VT_UserHostAddress"), Session("_VT_CurrentUserId")))

        SaveNewOrder = CurrentSession.VT_NewOrderID

    End Function

    Public Function StoreFormSpecificData() As String
        Dim strMessage As String = ""

        Dim blnSaved As Boolean

        If Me.CurrentSession.VT_NewOrderID = 0 Then
            Me.CurrentSession.VT_NewOrderID = SaveNewOrder(strMessage)
        End If


        blnSaved = SaveOrderItems()

        StoreFormSpecificData = strMessage


    End Function

    Function SaveOrderItems() As Boolean

    End Function
    Function ValidateMe() As String
        Dim objDisp As New VT_Display.DisplayFuncs
        Dim mpContentPlaceHolder As ContentPlaceHolder

        ValidateMe = ""

        'Check first that data is valid to be saved.

        Dim i As Integer
        Dim objProducts As New VTDBFunctions.VTDBFunctions.ProductsFunctions
        Dim lngProductID As Long
        Dim intUnitOfSale As Integer
        Dim dblQuantity As Double
        Dim dblWeight As Double
        Dim strTemp As String
        Dim objDataPreserve As New BPADotNetCommonFunctions.VT_DataPreserve.DataPreserve

        Dim dtGridRows As New Data.DataTable
        dtGridRows = objDataPreserve.GetWDGDataFromSession(wdgOrderItems)

        ' Go through grid 
        For i = 0 To dtGridRows.Rows.Count - 1
            ' Find available Qty or Weight
            If IsNumeric(dtGridRows.Rows(i).Item("QuantityRequested")) Then
                dblQuantity = CDbl(dtGridRows.Rows(i).Item("QuantityRequested"))
            Else
                dblQuantity = 0
            End If

            If IsNumeric(dtGridRows.Rows(i).Item("WeightRequested")) Then
                dblWeight = CDbl(dtGridRows.Rows(i).Item("WeightRequested"))
            Else
                dblWeight = 0
            End If

            If dblWeight > 0 Or dblQuantity > 0 Then
                If IsNumeric(dtGridRows.Rows(i).Item("ProductId")) Then
                    lngProductID = CLng(dtGridRows.Rows(i).Item("ProductId"))

                    intUnitOfSale = objProducts.GetUnitOfSale(lngProductID)



                    If dblQuantity = 0 And dblWeight = 0 Then
                    Else
                        If intUnitOfSale = 1 And dblQuantity = 0 Then
                            strTemp = "The item in Row " + CStr(i + 1) + " - "
                            strTemp = strTemp + Trim(dtGridRows.Rows(i).Item("ProductName")) + " - is measured by Quantity and the Quantity selected is 0.\n\n"
                            strTemp = strTemp + "Please edit before saving order."

                            ValidateMe = strTemp
                            Exit Function

                        ElseIf intUnitOfSale <> 1 And dblWeight = 0 Then
                            strTemp = "The item in Row " + CStr(i + 1) + " - "
                            strTemp = strTemp + Trim(dtGridRows.Rows(i).Item("ProductName")) + " - is measured by Weight and the Weight selected is 0.\n\n"
                            strTemp = strTemp + "Please edit before saving order."

                            ValidateMe = strTemp
                            Exit Function
                        End If
                    End If

                End If
            End If
        Next i


    End Function

    Protected Sub btnAddNew_Click(sender As Object, e As EventArgs) Handles btnAddNew.Click

        'need to set up session variables for add/edit screen
        Dim objDataPreserve As New BPADotNetCommonFunctions.VT_DataPreserve.DataPreserve
        Dim dtOrderGrid As New DataTable
        dtOrderGrid = objDataPreserve.GetWDGDataFromSession(wdgOrderItems)
        hdnItemNum.Value = dtOrderGrid.Rows.Count + 1

        Session("SelectedProductId") = 0
        Session("ItemNum") = hdnItemNum.Value
        Response.Redirect("mAddEditProduct.aspx")
    End Sub

    Private Sub Mobile_mOrderProducts_Load(sender As Object, e As EventArgs) Handles Me.Load
        Dim objDataPreserve As New BPADotNetCommonFunctions.VT_DataPreserve.DataPreserve

        If Not IsPostBack Then
            FillData(Me.CurrentSession.VT_NewOrderID)

        Else
            wdgOrderItems.DataSource = objDataPreserve.GetWDGDataFromSession(wdgOrderItems)


        End If
    End Sub

    Sub FillData(OrderID As Long)
        'SmcN 28/11/2013  - Created this function to work with the Sales Order items stored in the 'tls_GridData' Matrix
        Dim objDataPreserve As New BPADotNetCommonFunctions.VT_DataPreserve.DataPreserve
        Dim objTelesales As New VTDBFunctions.VTDBFunctions.TelesalesFunctions
        Dim objWF As New WorkFlowFunctionsForSales.WorkFlowFunctions_Sales
        Dim dtItemsForGrid As New DataTable
        Dim dtServiceItems As New DataTable
        Dim dtTemp As New DataTable
        Dim dsOrderDetails As New DataSet
        Dim dsOrderItems As New DataSet
        Dim objDBF As New MatrixFunctionsForSales.MatrixFunctions_Sales
        Dim objG As New BPADotNetCommonFunctions.VT_ReportFunctions.GeneralFunctions
        Dim objSales As New BPADotNetCommonFunctions.SalesFunctions.Sales_Orders
        Dim strConn As String = Session("_VT_DotNetConnString")
        Dim dvTemp As Data.DataView

        If OrderID > 0 Then
            'There is a valid OrderID so read the Order related details and fill the grid
            dsOrderDetails = objTelesales.GetOrderForId(OrderID)

            Me.CurrentSession.VT_SalesOrderNum = dsOrderDetails.Tables(0).Rows(0).Item("SalesOrderNum")
            lblOrderNum.Text = Me.CurrentSession.VT_SalesOrderNum

            ' ---------- Get data and bind the Order Items Grid ---------
            dtTemp = objSales.GetSalesOrderItemsFromMatrix("OrderItems", CInt(Me.CurrentSession.VT_SalesOrderNum), strConn, Me.CurrentSession.VT_tlsNumFields)

            If dtTemp.Rows.Count > 0 Then
                'there are items for this sales order already so proceed with display

                If dtTemp.Columns.Contains("Item_OnHoldPersonResponsibleID") = False Then
                    dtTemp.Columns.Add("Item_OnHoldPersonResponsibleID")
                End If

                If dtTemp.Columns.Contains("MachineNum") = False Then
                    dtTemp.Columns.Add("MachineNum")
                End If
                If dtTemp.Columns.Contains("Kanban") = False Then
                    dtTemp.Columns.Add("Kanban")
                End If
                'Convert the Date colums to a date field and proper format
                Dim astrColNames(17, 1) As String
                astrColNames(0, 0) = "Item_DateRequested"
                astrColNames(0, 1) = "CONVERT_TO_DATE"
                astrColNames(1, 0) = "Item_DateOut"
                astrColNames(1, 1) = "CONVERT_TO_DATE"
                astrColNames(2, 0) = "Item_DateArrival"
                astrColNames(2, 1) = "CONVERT_TO_DATE"
                astrColNames(3, 0) = "SalesItemNum"
                astrColNames(3, 1) = "CONVERT_TO_INT"
                astrColNames(4, 0) = "PO_UnitPrice"
                astrColNames(4, 1) = "CONVERT_TO_DOUBLE"
                astrColNames(5, 0) = "PO_TotalPrice"
                astrColNames(5, 1) = "CONVERT_TO_DOUBLE"
                astrColNames(6, 0) = "PriceDifference"
                astrColNames(6, 1) = "CONVERT_TO_DOUBLE"

                astrColNames(7, 0) = "QuantityRequested"
                astrColNames(7, 1) = "CONVERT_TO_DOUBLE"
                astrColNames(8, 0) = "WeightRequested"
                astrColNames(8, 1) = "CONVERT_TO_DOUBLE"
                astrColNames(9, 0) = "UnitPrice"
                astrColNames(9, 1) = "CONVERT_TO_DOUBLE"
                astrColNames(10, 0) = "TotalExclVAT"
                astrColNames(10, 1) = "CONVERT_TO_DOUBLE"
                astrColNames(11, 0) = "VAT"
                astrColNames(11, 1) = "CONVERT_TO_DOUBLE"
                astrColNames(12, 0) = "TotalPrice"
                astrColNames(12, 1) = "CONVERT_TO_DOUBLE"

                astrColNames(13, 0) = "Item_SalesDBUnit"
                astrColNames(13, 1) = "CONVERT_TO_DOUBLE"
                astrColNames(14, 0) = "Item_SalesDBTotal"
                astrColNames(14, 1) = "CONVERT_TO_DOUBLE"
                astrColNames(15, 0) = "Item_CustomUnit"
                astrColNames(15, 1) = "CONVERT_TO_DOUBLE"
                astrColNames(16, 0) = "Item_CustomTotal"
                astrColNames(16, 1) = "CONVERT_TO_DOUBLE"
                astrColNames(17, 0) = "Item_PartNumGrandTotal"
                astrColNames(17, 1) = "CONVERT_TO_INT"


                dtTemp = objG.CleanColumnFormats(dtTemp, astrColNames)

                dtItemsForGrid = dtTemp


                'SMcN 09/10/2013 Change the types of the relevant columns to be numeric
                'This is necessary to facilite proper sum totaling on the WebDataGrid
                'Also you cannot change the data TYPE of a datatable column if it already has data so need to use a tempory table
                Dim dtSwap As New DataTable
                dtSwap = dtItemsForGrid.Clone

                dtSwap.Columns("QuantityRequested").DataType = GetType(System.Int32)
                dtSwap.Columns("WeightRequested").DataType = GetType(System.Double)
                dtSwap.Columns("UnitPrice").DataType = GetType(System.Double)
                dtSwap.Columns("TotalExclVAT").DataType = GetType(System.Double)
                dtSwap.Columns("VAT").DataType = GetType(System.Double)
                dtSwap.Columns("TotalPrice").DataType = GetType(System.Double)

                dtSwap.Columns("PO_UnitPrice").DataType = GetType(System.Double)
                dtSwap.Columns("PO_TotalPrice").DataType = GetType(System.Double)
                dtSwap.Columns("PriceDifference").DataType = GetType(System.Double)

                dtSwap.Columns("Item_SalesDBUnit").DataType = GetType(System.Double)
                dtSwap.Columns("Item_SalesDBTotal").DataType = GetType(System.Double)
                dtSwap.Columns("Item_CustomUnit").DataType = GetType(System.Double)
                dtSwap.Columns("Item_CustomTotal").DataType = GetType(System.Double)
                dtSwap.Columns("Item_PartNumGrandTotal").DataType = GetType(System.Int32)


                For intCnt As Integer = 0 To dtItemsForGrid.Rows.Count - 1
                    dtSwap.ImportRow(dtItemsForGrid.Rows(intCnt))
                Next

                dtItemsForGrid.Clear()
                dtItemsForGrid = dtSwap


                'Format The Grid (This function also binds the grid)
                FormatGridAndRows(dtItemsForGrid, wdgOrderItems)
                hdnItemNum.Value = dtItemsForGrid.Rows.Count + 1

            Else
                'There are no items for this sales order yet so bind an empty grid structure
                InitialiseAndBindEmptyOrderItemsGrid()

                hdnItemNum.Value = 1
            End If


        Else
            'There is no sales Order yet so we construct empty datatable and bind these
            InitialiseAndBindEmptyOrderItemsGrid()
            ' InitialiseAndBindEmptyServiceItemsGrid()
        End If

    End Sub

    Private Sub FormatGridAndRows(ByRef dtItemRows As DataTable, ByRef wdgThis As Infragistics.Web.UI.GridControls.WebDataGrid)

        'SmcN 15/04/2014  This function Formats Row to show 'On_hold' rows. It also Binids the grid

        Dim objG As New BPADotNetCommonFunctions.VT_ReportFunctions.GeneralFunctions
        Dim objDataPreserve As New BPADotNetCommonFunctions.VT_DataPreserve.DataPreserve
        Dim dvtemp As Data.DataView
        Dim astrColNames(1, 1) As String

        If dtItemRows.Rows.Count > 0 Then

            'Convert the Date colums to a date field and proper format
            If dtItemRows.Columns("SalesItemNum").DataType = GetType(System.String) Then
                'include the 'SalesItemNum' in the column type conversion
                ReDim astrColNames(3, 1)
                astrColNames(0, 0) = "Item_DateRequested"
                astrColNames(0, 1) = "CONVERT_TO_DATE"
                astrColNames(1, 0) = "Item_DateOut"
                astrColNames(1, 1) = "CONVERT_TO_DATE"
                astrColNames(2, 0) = "Item_DateArrival"
                astrColNames(2, 1) = "CONVERT_TO_DATE"
                astrColNames(3, 0) = "SalesItemNum"
                astrColNames(3, 1) = "CONVERT_TO_INT"
            Else
                'Dont include 'SalesItemNum' in the column type conversion
                ReDim astrColNames(2, 1)
                astrColNames(0, 0) = "Item_DateRequested"
                astrColNames(0, 1) = "CONVERT_TO_DATE"
                astrColNames(1, 0) = "Item_DateOut"
                astrColNames(1, 1) = "CONVERT_TO_DATE"
                astrColNames(2, 0) = "Item_DateArrival"
                astrColNames(2, 1) = "CONVERT_TO_DATE"
            End If

            dtItemRows = objG.CleanColumnFormats(dtItemRows, astrColNames)



            'Sort the grid on the SalesItemNum Column
            If dtItemRows.Rows.Count >= 1 Then
                dtItemRows.DefaultView.Sort = "SalesItemNum ASC"
                dvtemp = dtItemRows.DefaultView
                dvtemp.Sort = "SalesItemNum ASC"
                dtItemRows = dvtemp.ToTable
            End If


            'Re-index the SalesItemNum column to take accout of any deletions
            For intCnt = 0 To dtItemRows.Rows.Count - 1
                dtItemRows.Rows(intCnt).Item("SalesItemNum") = intCnt + 1
            Next

        End If


        'Rebind the grid here
        objDataPreserve.BindDataToWDG(dtItemRows, wdgThis)

        'Apply Grid Formatting here
        For intLoop As Integer = 0 To dtItemRows.Rows.Count - 1
            If UCase(dtItemRows.Rows(intLoop).Item("Item_OnHold")) = "YES" Then
                'Set the formatting on the grid - Set the Background colour
                wdgThis.Rows(intLoop).CssClass = "Row_Salmon"
            End If

        Next


    End Sub
    Sub InitialiseAndBindEmptyOrderItemsGrid()

        Dim dtItemsForGrid As New DataTable
        Dim dtServiceItems As New DataTable
        Dim objDataPreserve As New BPADotNetCommonFunctions.VT_DataPreserve.DataPreserve

        'There is no sales order yet so build a blank datatable and bind the grids to these
        dtItemsForGrid.Columns.Add("QuantityRequested")
        dtItemsForGrid.Columns.Add("Quantity")
        dtItemsForGrid.Columns.Add("txtUoM")
        dtItemsForGrid.Columns.Add("WeightRequested")
        dtItemsForGrid.Columns.Add("Weight")
        dtItemsForGrid.Columns.Add("UnitPrice")
        dtItemsForGrid.Columns.Add("VAT")
        dtItemsForGrid.Columns.Add("TotalPrice")
        dtItemsForGrid.Columns.Add("Comment")
        dtItemsForGrid.Columns.Add("ProductId")
        dtItemsForGrid.Columns.Add("SalesOrderItemId")
        dtItemsForGrid.Columns.Add("TraceCodeDesc")
        dtItemsForGrid.Columns.Add("OrderItemId")

        ' call the AddAddtional columns function
        AddAdditionalProductColumns(dtItemsForGrid)

        'Bind the Grid
        objDataPreserve.BindDataToWDG(dtItemsForGrid, wdgOrderItems)


    End Sub
    Function AddAdditionalProductColumns(ByRef dtThis As DataTable) As Boolean
        'SMcN 26/11/2013
        'This function adds additional columns needed to the PRODUCTS datatable

        'First Check if the columns already exist (have been added before)

        If dtThis.Columns.Contains("Site") = True Then
            'Columns Already exist so this arrary of columns would hav ebeen added before so do nothing
            AddAdditionalProductColumns = False
        Else
            'Columns don't exist so add them here

            dtThis.Columns.Add("SalesItemNum")
            dtThis.Columns.Add("ProductName", Type.GetType("System.String"))
            dtThis.Columns.Add("ProductCode", Type.GetType("System.String"))
            dtThis.Columns.Add("CustomerCode", Type.GetType("System.String"))
            dtThis.Columns.Add("CustomerRev", Type.GetType("System.String"))
            dtThis.Columns.Add("TotalExclVat")
            dtThis.Columns.Add("VATRate")
            dtThis.Columns.Add("UnitPriceCurrency", Type.GetType("System.String"))

            dtThis.Columns.Add("LocationId")
            dtThis.Columns.Add("AdditionOrder")
            dtThis.Columns.Add("ProductSellBy")

            dtThis.Columns.Add("UnitOfSale")
            dtThis.Columns.Add("DimLength")
            dtThis.Columns.Add("DimWidth")
            dtThis.Columns.Add("PO_UnitPrice")
            dtThis.Columns.Add("PO_TotalPrice")
            dtThis.Columns.Add("PriceDifference")

            dtThis.Columns.Add("Item_DateRequested")
            dtThis.Columns.Add("Item_DateOut")
            dtThis.Columns.Add("Item_DateArrival")
            dtThis.Columns.Add("Item_QuoteReference")
            dtThis.Columns.Add("Item_DeliveryTerms")
            dtThis.Columns.Add("Item_OnHold")
            dtThis.Columns.Add("Item_OnHoldDate")
            dtThis.Columns.Add("Item_OnHoldPersonResponsible")
            dtThis.Columns.Add("Item_OnHoldPersonResponsibleID")
            dtThis.Columns.Add("Item_OnHoldTVComment")

            dtThis.Columns.Add("Site")

            dtThis.Columns.Add("SO_ContiguousNum")
            dtThis.Columns.Add("SalesOrderNum")
            dtThis.Columns.Add("SalesOrderId")
            dtThis.Columns.Add("CustomerId")

            dtThis.Columns.Add("Item_SalesDBUnit")
            dtThis.Columns.Add("Item_SalesDBTotal")
            dtThis.Columns.Add("Item_CustomUnit")
            dtThis.Columns.Add("Item_CustomTotal")
            dtThis.Columns.Add("Item_PartNumGrandTotal")

            AddAdditionalProductColumns = True
        End If

    End Function

    Private Sub btnEdit_Click(sender As Object, e As EventArgs) Handles btnEdit.Click
        'need to set up session variables for add/edit screen
        Dim objDataPreserve As New BPADotNetCommonFunctions.VT_DataPreserve.DataPreserve
        Dim dtOrderGrid As New DataTable
        dtOrderGrid = objDataPreserve.GetWDGDataFromSession(wdgOrderItems)
        hdnItemNum.Value = dtOrderGrid.Rows.Count + 1

        Session("SelectedProductId") = 0
        Session("ItemNum") = hdnItemNum.Value
        Response.Redirect("mAddEditProduct.aspx")
    End Sub

    Private Sub wdgOrderItems_RowSelectionChanged(sender As Object, e As SelectedRowEventArgs) Handles wdgOrderItems.RowSelectionChanged
        Dim objC As New VT_CommonFunctions.CommonFunctions

        'serialise the selected data row
        Dim RowSelected As Infragistics.Web.UI.GridControls.SelectedRowCollection = wdgOrderItems.Behaviors.Selection.SelectedRows
        Dim intActiveRowIndex As Integer = RowSelected(0).Index
        'wdgOrderItems.Behaviors.Activation.ActiveCell = wdgOrderItems.Rows(intActiveRowIndex).Items(1)

        Me.CurrentSession.VT_AddingProductsSelectedRow_V2 = objC.SerialiseWebDataGridRow(wdgOrderItems, intActiveRowIndex)

    End Sub
End Class
