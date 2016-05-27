Imports BPADotNetCommonFunctions
Imports BPADotNetCommonFunctions.VT_CommonFunctions.MatrixFunctions
Imports System.Data.SqlClient
Imports System.Data
Imports System.Drawing
Imports System.Data.DataTableExtensions
Imports VTDBFunctions.VTDBFunctions


Partial Class TabPages_TruckView
    Inherits MyBasePage


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        On Error GoTo Errorhandler

        'need to pop up the panels from the code behind because when a page method is called from the javascript
        'function, it triggers a postback, so when you try to pop up a panel depending on the result of a page method
        'it posts back so the panel is unloaded again

        'btnAddNewComment.Attributes.Add("onclick", "return ShowMessageModalPopup();")
        ' btnAssignToPerson.Attributes.Add("onclick", "return ShowPersonAssignModalPopup();")
        cmdSaveComment.OnClientClick = String.Format("MessageUpdate('{0}','{1}')", cmdSaveComment.UniqueID, "")
        cmdSavePersonAssign.OnClientClick = String.Format("PersonAssignUpdate('{0}','{1}')", cmdSavePersonAssign.UniqueID, "")

        cmdTaskOk.OnClientClick = String.Format("AlertUpdate('{0}','{1}')", cmdTaskOk.UniqueID, "")

        Dim objDataPreserve As New BPADotNetCommonFunctions.VT_DataPreserve.DataPreserve

        Me.CurrentSession.FormDataTable = SalesOrderFormDataTable
        Me.CurrentSession.FormListTable = SalesOrderFormListTable
  

        If Not IsPostBack Then
            FillData()



        Else
            wdgRepGrid.DataSource = objDataPreserve.GetWDGDataFromSession(wdgRepGrid)
            wdgOrderItems.DataSource = objDataPreserve.GetWDGDataFromSession(wdgOrderItems)
            Dim dtShortOutput As DataTable = objDataPreserve.GetWDGDataFromSession(wdgRepGrid)
            Dim intNumDays As Integer

           
        End If

        If Not Session("_VT_TruckViewRowSelected") Is Nothing Then
            'Try to locate to the previous selected row if possible. NOTE: The original row may not now be in the table if its status changed from On-hold
            'if there is a selected row saved then show it as selected

            Dim dt As DataTable = Session("_VT_TruckViewRowSelected")
            If dt.Rows.Count > 0 Then
                Dim intIndex As Integer = dt.Rows(0).Item("VT_ActiveRowIndex")
                'check that the SO number is the same for the saved active index and that row in the grid now
                If wdgRepGrid.Rows.Count > intIndex Then
                    If dt.Rows(0).Item("SO_ContiguousNum") = wdgRepGrid.Rows(intIndex).Items(2).Text Then
                        wdgRepGrid.Behaviors.Activation.ActiveCell = wdgRepGrid.Rows(intIndex).Items(2)

                        Dim selectedRows As Infragistics.Web.UI.GridControls.SelectedRowCollection = wdgRepGrid.Behaviors.Selection.SelectedRows
                        Dim wgdRow As Infragistics.Web.UI.GridControls.GridRecord = wdgRepGrid.Rows(0)
                        selectedRows.Add(wgdRow)
                    End If
                End If

            End If

        End If
        'SmcN 26/01/2015 Set the grid width if required
        If Me.CurrentSession.VT_BrowserWindowWidth < 1400 And Not Me.CurrentSession.VT_BrowserWindowWidth Is Nothing Then
            wdgRepGrid.Width = Me.CurrentSession.VT_BrowserWindowWidth - 54

        End If
        Exit Sub

Errorhandler:
        Dim objlog As New VTDBFunctions.VTDBFunctions.UtilFunctions
        objlog.LogAction("error on page load of hold dashboard: " & Err.Description)
    End Sub



    Sub FillData()
        Dim objG As New BPADotNetCommonFunctions.VT_ReportFunctions.GeneralFunctions
        Dim objDBF As New SalesOrdersFunctions.SalesOrders
        Dim objM As New VTDBFunctions.VTDBFunctions.VTMatrixFunctions
        Dim objSales As New BPADotNetCommonFunctions.SalesFunctions.Sales_Orders

        Dim objDataPreserve As New BPADotNetCommonFunctions.VT_DataPreserve.DataPreserve
        Dim dtoutput As New DataTable
        Dim strStatusSearch As String = ""
        Dim strerr As String
        Dim dt As DataTable
        Dim intCnt As Integer
        Dim strConn As String = Session("_VT_DotNetConnString")

        'need to get all items that are currently in a truck, so all open orders and all closed but not yet delivered

        strStatusSearch = " WHERE tls_SalesOrders.Status = '" & GetGlobalResourceObject("Resource", "Order_New") & "'"
        strStatusSearch = strStatusSearch & " OR tls_SalesOrders.Status = '" & GetGlobalResourceObject("Resource", "Order_OpenPartShipped") & "'"
        strStatusSearch = strStatusSearch & " OR tls_SalesOrders.Status = '" & GetGlobalResourceObject("Resource", "Order_ClosedComplete") & "'"
        strStatusSearch = strStatusSearch & " OR tls_SalesOrders.Status = '" & GetGlobalResourceObject("Resource", "Order_ClosedPartShipped") & "'"

        dt = objSales.GetOrderDataForStatus(Session("_VT_DotNetConnString"), strStatusSearch)

        lblNumOrders.Text = dt.Rows.Count

        If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
            Try
                'get data from grids
                dtoutput = objSales.GetMatrixOrderDataForOrderNum(dt, strConn, Me.CurrentSession.VT_tlsNumFields)
                If dtoutput IsNot Nothing AndAlso dtoutput.Rows.Count > 0 Then
                    lblNumItems.Text = "1"

                    Dim objApp As New CommentFunctions

                    Dim intIdToUse As Integer

                    Dim dtThreads As Data.DataTable = objApp.GetThreads(intIdToUse)
                    'Calculate the price difference column values
                    If dtoutput.Columns.Contains("UnitDifference") = False Then
                        dtoutput.Columns.Add("UnitDifference")
                    End If
                    If dtoutput.Columns.Contains("UnreadComments") = False Then
                        dtoutput.Columns.Add("UnreadComments")
                    End If

                    lblNumItems.Text = "2"

                    Dim dtTemp As DataTable
                    Dim strformlock As String
                    Dim objVTM As New VTDBFunctions.VTDBFunctions.VTMatrixFunctions
                    'Add in the extra data items to this datatable These items are read from the Details page in the matrix     SmcN 06/05/2014
                    'first we need to fill this grid with the relevant MatrixLinkId's
                    Dim strWhereQualifier As String = "" ' " JobStatusText = '" & GetGlobalResourceObject("Resource", "Order_OnHold_System") & "' OR JobStatusText = '" & GetGlobalResourceObject("Resource", "Order_OnHold_Manual") & "'"
                    strerr = objG.GetBlockOfMatrixIdsFromBatchTable(strConn, dtoutput, "SalesOrderNum", "Sales Order", strWhereQualifier)

                    Dim astrItems(0) As String
                    astrItems(0) = "ReasonOnHold"

                    Dim strFormName As String = "Details"
                    Dim strKeyField As String = "MatrixLinkId"
                    Dim dtDisplay As New DataTable

                    strerr = objM.GetBlockOfMatrixDataItems(strConn, strFormName, VTMatrixFunctions.Sales, astrItems, dtoutput, strKeyField)

                    'need to get the date on hold from the matrix, and the new ballinroe items
                    ReDim astrItems(6)

                    astrItems(0) = "DateOnHold"
                    astrItems(1) = "DriverIds"
                    astrItems(2) = "DriverNames"
                    astrItems(3) = "TruckId"
                    astrItems(4) = "TruckName"
                    astrItems(5) = "PickUpLocation"
                    astrItems(6) = "AltRoute"


                    strFormName = "Details"
                    strKeyField = "MatrixLinkId"
                    strerr = objM.GetBlockOfMatrixDataItems(strConn, strFormName, VTMatrixFunctions.Sales, astrItems, dtoutput, strKeyField)
                    Dim dtTxns As New DataTable
                    Dim objdb As New VTDBFunctions.VTDBFunctions.DBFunctions
                    Dim lngSalesOrderItemID As Long
                    Dim lngTraceCode As Long
                    Dim objTrace As New VTDBFunctions.VTDBFunctions.TraceFunctions
                    dtDisplay = dtoutput.Clone
                    Dim objcommon As New BPADotNetCommonFunctions.VT_CommonFunctions.CommonFunctions

                    Dim strServiceCategory As String = objcommon.GetConfigItem("ServiceItemCategory")

                    For intCnt = 0 To dtoutput.Rows.Count - 1
                        If dtoutput.Rows(intCnt).Item("ProductCategory") <> strServiceCategory Then
                            dtoutput.Rows(intCnt).Item("UnitDifference") = IIf(dtoutput.Rows(intCnt).Item("PO_UnitPrice") <> "", dtoutput.Rows(intCnt).Item("PO_UnitPrice"), 0) - dtoutput.Rows(intCnt).Item("UnitPrice")

                            SetUpOrderSessionVariablesPerRow(dtoutput.Rows(intCnt))

                            If UnreadCommentsExist(Me.CurrentSession.VT_OrderMatrixID, Session("_VT_CurrentUserName")) Then
                                dtoutput.Rows(intCnt).Item("UnreadComments") = "MFG"
                            Else
                                dtoutput.Rows(intCnt).Item("UnreadComments") = ""
                            End If



                            'if there is no person responsible then instead of showing a blank space, fill in the words 'Sales support team'
                            If IsDBNull(dtoutput.Rows(intCnt).Item("Item_OnHoldPersonResponsible")) Then
                                dtoutput.Rows(intCnt).Item("Item_OnHoldPersonResponsible") = "Sales Support Team"
                            ElseIf Trim(dtoutput.Rows(intCnt).Item("Item_OnHoldPersonResponsible")) = "" Then
                                dtoutput.Rows(intCnt).Item("Item_OnHoldPersonResponsible") = "Sales Support Team"
                            End If

                            If dtoutput.Rows(intCnt).Item("SalesOrderItemID") <> "" Then
                                lngSalesOrderItemID = dtoutput.Rows(intCnt).Item("SalesOrderItemID")
                            End If
                            'fill in the horse name from the trace code that is stored with the sale txn
                            dtTxns = objdb.ExecuteSQLReturnDT("Select * from trc_Transactions where TransactionType = 6 and salesOrderItemID=" & lngSalesOrderItemID)
                            If dtTxns.Rows.Count > 0 Then
                                If dtTxns.Rows(0).Item("TraceCodeId") > 0 Then
                                    dtoutput.Rows(intCnt).Item("TraceCodeDesc") = objTrace.GetTracecodeDescForId(dtTxns.Rows(0).Item("TraceCodeId"))
                                End If
                            End If
                            dtDisplay.ImportRow(dtoutput.Rows(intCnt))
                        End If

                    Next



                    'Clean up the DataTable columns
                    Dim astrColNames(5, 1) As String
                    astrColNames(0, 0) = "Item_DateOut"
                    astrColNames(0, 1) = "CONVERT_TO_DATE"
                    astrColNames(1, 0) = "DateCreated"
                    astrColNames(1, 1) = "CONVERT_TO_DATE"
                    astrColNames(2, 0) = "Item_DateRequested"
                    astrColNames(2, 1) = "CONVERT_TO_DATE"
                    astrColNames(3, 0) = "Item_DateArrival"
                    astrColNames(3, 1) = "CONVERT_TO_DATE"
                    astrColNames(4, 0) = "SalesItemNum"
                    astrColNames(4, 1) = "CONVERT_TO_INT"
                    astrColNames(5, 0) = "DateOnHold"
                    astrColNames(5, 1) = "CONVERT_TO_INT"

                    dtDisplay = objG.CleanColumnFormats(dtDisplay, astrColNames)

                    'SmcN 28/05/2014  Sort the grid here on the Sales Order Number and then the item number.
                    dtDisplay = objG.SortDataTable("Item_DateRequested ASC, SO_ContiguousNum ASC, SalesItemNum ASC", dtDisplay)
                    ' dtoutput = objG.SortDataTable("DateOnHold DESC", dtoutput)
                    Session("TruckDashboardGrid") = dtDisplay
                    objDataPreserve.BindDataToWDG(dtDisplay, wdgRepGrid)

                    Dim intNumDays As Integer

                    InitialiseAndBindEmptyOrderItemsGrid()

                    lblNumItems.Text = dtDisplay.Rows.Count

                Else
                    InitialiseAndBindEmptyOrderItemsGrid()
                End If
            Catch
                lblMsg.Text = Err.Description
                ModalPopupExtenderMsg.Show()

            End Try

        End If



    End Sub


    Function VT_SetToDateFormat(ByRef dtTemp As DataTable, ByRef intCnt As Integer, ByRef strTemp As String) As Date
        'SmcN 19/02/2014   This function takes a string and returns a date it takes account of Nulls and empty strings and returns a date as date.MinValue if it is.

        Dim dteTemp As Date
        Dim sysType As Type

        If IsDBNull(dtTemp.Rows(intCnt).Item(strTemp)) Then
            dteTemp = Date.MinValue
        Else

            sysType = dtTemp.Rows(intCnt).Item(strTemp).GetType

            If sysType = System.Type.GetType("System.DateTime") Then
                'Handle as date
                If dtTemp.Rows(intCnt).Item(strTemp) IsNot Nothing Then
                    dteTemp = CDate(dtTemp.Rows(intCnt).Item(strTemp))
                Else
                    dteTemp = Date.MinValue
                End If
            Else
                'Otherwise Handle as string
                If dtTemp.Rows(intCnt).Item(strTemp) <> "" Then
                    dteTemp = CDate(dtTemp.Rows(intCnt).Item(strTemp))
                Else
                    dteTemp = Date.MinValue
                End If
            End If


        End If

        VT_SetToDateFormat = dteTemp

    End Function



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
        dtItemsForGrid.Columns.Add("ShowLocked")

        ' call the AddAddtional columns function
        AddAdditionalProductColumns(dtItemsForGrid)

        'Bind the Grid
        objDataPreserve.BindDataToWDG(dtItemsForGrid, wdgOrderItems)


    End Sub

    Function AddAdditionalProductColumns(ByRef dtThis As DataTable) As Boolean
        'SMcN 26/11/2013
        'This function adds additional columns needed to the PRODUCTS datatable

        'First Check if the columns already exist (have been added before)

        If dtThis.Columns.Contains("TruckId") = True Then
            'Columns Already exist so this arrary of columns would have been added before so do nothing
            AddAdditionalProductColumns = False
        Else
            'Columns don't exist so add them here
            ' Sales Order Item Columns


            dtThis.Columns.Add("Status")


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

            'Items from the CustomerItems Node
            dtThis.Columns.Add("DeliverTo_RequestedDate")
            dtThis.Columns.Add("BillTo_Name")
            dtThis.Columns.Add("BillTo_Code")
            dtThis.Columns.Add("Other_CustPO")
            dtThis.Columns.Add("SoldTo_Name")
            dtThis.Columns.Add("SoldTo")
            dtThis.Columns.Add("SoldTo_Code")
            dtThis.Columns.Add("Cust_PO")
            dtThis.Columns.Add("DeliverTo_Name")
            dtThis.Columns.Add("DeliverTo_Code")
            dtThis.Columns.Add("OrderDate")
            dtThis.Columns.Add("DueDate")
            dtThis.Columns.Add("DateCreated")

            dtThis.Columns.Add("TruckId")
            dtThis.Columns.Add("TruckName")
            dtThis.Columns.Add("DriverIDs")
            dtThis.Columns.Add("DriverNames")
            dtThis.Columns.Add("PickUpLocation")
            dtThis.Columns.Add("AltRoute")
            AddAdditionalProductColumns = True
        End If

    End Function

    ''' <summary>
    ''' This function validates the data on the form before it can be saved
    ''' </summary>
    ''' <returns>If there is a Validation problem return a message, Empty String otherwise</returns>
    ''' <remarks></remarks>
    Public Function ValidateMe() As String

    End Function

    ''' <summary>
    '''This function returns whether we should save the current page or not. The Default is to save
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function DataToSave() As Boolean

        DataToSave = True

    End Function



    Protected Sub btnBack1_Click(sender As Object, e As ImageClickEventArgs) Handles btnBack1.Click, btnBack2.Click

        Response.Redirect("~/TabPages/Orders_Opening_New.aspx")

    End Sub



    Protected Sub wdgRepGrid_ActiveCellChanged(sender As Object, e As Infragistics.Web.UI.GridControls.ActiveCellEventArgs) Handles wdgRepGrid.ActiveCellChanged

        Dim objC As New VT_CommonFunctions.CommonFunctions
        If Not e Is Nothing Then
            Dim intActiveRowIndex As Integer = e.CurrentActiveCell.Row.Index
            If Not objC Is Nothing Then
                Session("_VT_TruckViewRowSelected") = objC.SerialiseWebDataGridRow(wdgRepGrid, intActiveRowIndex)

                SetUpOrderSessionVariables()
            Else
                lblMsg.Text = "An error has occurred with the grid on this page. Please exit the portal and then reopen it. Apologies for the inconvenience."
                ModalPopupExtenderMsg.Show()
            End If
        Else
            lblMsg.Text = "An error has occurred with the grid on this page. Please exit the portal and then reopen it. Apologies for the inconvenience."
            ModalPopupExtenderMsg.Show()
        End If

    End Sub

    Public Function PortalRootNode() As String
        ' retrieve the Root node of the current portal
        ' we will use it to create a consistent path to the image files
        Dim strCompletePath As String = Server.MapPath("~")

        'ServerRootPath = strCompletePath
        PortalRootNode = Mid(strCompletePath, InStrRev(strCompletePath, "\") + 1)

    End Function


    Protected Sub cmdSavePersonAssign_Click(sender As Object, e As ImageClickEventArgs) Handles cmdSavePersonAssign.Click
        If Session("_VT_TruckViewRowSelected") IsNot Nothing Then

            Dim astrColNamesAndValues(1, 1) As String
            astrColNamesAndValues(0, 0) = "Item_OnHoldPersonResponsible"
            astrColNamesAndValues(0, 1) = ddlUsers_NS.SelectedItem.Text
            astrColNamesAndValues(1, 0) = "Item_OnHoldPersonResponsibleID"
            astrColNamesAndValues(1, 1) = ddlUsers_NS.SelectedItem.Value

            UpdateGridCellValue(astrColNamesAndValues)


        End If
       
    End Sub

    Function UpdateGridCellValue(ByVal astrColNamesAndValues(,) As String) As Boolean
        'SmcN 26/03/2014  This function adds or edits the value in the specified Cell.
        ' The cell is defined by the strColumnName parameter passed in and also the row as defined by the         Session("_VT_OnHoldManagerRowSelected") value
        'It also saves the comment on the releant line item of the relevant sales order in the Sales Matrix, OrderDetails grid 

        '' ''Dim dtTVGrid As New DataTable
        '' ''Dim dtSOItems As New DataTable
        '' ''Dim objDataPreserve As New BPADotNetCommonFunctions.VT_DataPreserve.DataPreserve
        '' ''Dim objDBF As New MatrixFunctionsForSales.MatrixFunctions_Sales
        '' ''Dim intSelectedRowIndex As Integer
        '' ''Dim intSalesOrderNum As Integer
        '' ''Dim intSOItemRow As Integer
        '' ''Dim strColName As String
        '' ''Dim strColValue As String
        '' ''Dim blnOrderItemsExist As Boolean
        '' ''Dim objSales As New BPADotNetCommonFunctions.SalesFunctions.Sales_Orders

        '' ''Dim strConn As String = Session("_VT_DotNetConnString")

        ' '' ''Read the OnHoldTV grid
        '' ''dtTVGrid = objDataPreserve.GetWDGDataFromSession(wdgRepGrid)

        ' '' ''Set the pointers to the correct row
        '' ''intSelectedRowIndex = Session("_VT_OnHoldManagerRowSelected").rows(0).item("VT_ActiveRowIndex")
        '' ''intSalesOrderNum = Session("_VT_OnHoldManagerRowSelected").rows(0).item("SalesOrderNum")

        ' '' ''Now we need to get the Sales order items for this sales order number from the matrix
        '' ''dtSOItems = objSales.GetSalesOrderItemsFromMatrix("OrderItems", intSalesOrderNum, strConn, Me.CurrentSession.VT_tlsNumFields)
        '' ''If dtSOItems.Rows.Count = 0 Then
        '' ''    blnOrderItemsExist = False
        '' ''Else
        '' ''    blnOrderItemsExist = True
        '' ''End If

        '' ''If dtSOItems.Columns.Contains("Item_OnHoldPersonResponsibleID") = False Then
        '' ''    dtSOItems.Columns.Add("Item_OnHoldPersonResponsibleID")
        '' ''End If
        '' ''If dtSOItems.Columns.Contains("MachineNum") = False Then
        '' ''    dtSOItems.Columns.Add("MachineNum")
        '' ''End If
        '' ''If dtSOItems.Columns.Contains("Kanban") = False Then
        '' ''    dtSOItems.Columns.Add("Kanban")
        '' ''End If
        '' ''If blnOrderItemsExist = True Then
        '' ''    'Find the relevant matching row in the dtSOItems table
        '' ''    For intInnerCnt = 0 To dtSOItems.Rows.Count - 1
        '' ''        If dtSOItems.Rows(intInnerCnt).Item("SalesItemNum") = Session("_VT_OnHoldManagerRowSelected").rows(0).item("SalesItemNum") Then
        '' ''            intSOItemRow = intInnerCnt
        '' ''            Exit For
        '' ''        End If
        '' ''    Next
        '' ''End If

        ' '' ''loop through the columns and update the relevant values
        '' ''For i = 0 To dtSOItems.Rows.Count - 1
        '' ''    For intCnt = 0 To astrColNamesAndValues.GetLength(0) - 1

        '' ''        strColName = astrColNamesAndValues(intCnt, 0)
        '' ''        strColValue = astrColNamesAndValues(intCnt, 1)

        '' ''        dtTVGrid.Rows(intSelectedRowIndex).Item(strColName) = strColValue
        '' ''        If blnOrderItemsExist Then
        '' ''            dtSOItems.Rows(i).Item(strColName) = strColValue
        '' ''        End If

        '' ''        If strColName = "Item_OnHoldPersonResponsibleID" Then
        '' ''            If ddlAssignToCust.Items(0).Selected = True Then

        '' ''                Dim strsql As String = "Update cus_Customers set CustomerPersonResponsible = " & ddlUsers_NS.SelectedItem.Value & " where CustomerId = " & dtTVGrid.Rows(intSelectedRowIndex).Item("CustomerId")
        '' ''                Dim objdb As New VTDBFunctions.VTDBFunctions.DBFunctions
        '' ''                objdb.ExecuteSQLQuery(strsql)


        '' ''            End If
        '' ''        End If

        '' ''    Next

        '' ''Next

        ' '' ''Rebind the ONHoldTV grid
        '' ''objDataPreserve.BindDataToWDG(dtTVGrid, wdgRepGrid)

        ' '' ''Now write out the updated 'dtSOItems' data
        '' ''objDataPreserve.BindDataToWDG(dtSOItems, wdgOrderItems)

        '' ''Dim intNumDays As Integer

        ' '' ''Apply Grid Formatting here
        '' ''For intLoop As Integer = 0 To dtTVGrid.Rows.Count - 1
        '' ''    If Trim(dtTVGrid.Rows(intLoop).Item("DateOnHold")) <> "" Then
        '' ''        intNumDays = dtTVGrid.Rows(intLoop).Item("DateOnHold")
        '' ''        If intNumDays >= 5 Then
        '' ''            'Set the formatting on the grid - Set the Background colour
        '' ''            wdgRepGrid.Rows(intLoop).CssClass = "Row_Salmon"
        '' ''        ElseIf intNumDays >= 3 Then
        '' ''            'Set the formatting on the grid - Set the Background colour
        '' ''            wdgRepGrid.Rows(intLoop).CssClass = "Row_Yellow"
        '' ''        End If

        '' ''    End If

        '' ''Next
        ' '' ''Save Order Items to the MATRIX table
        '' ''objSales.SaveSalesItemsToMatrix("OrderItems", wdgOrderItems, intSalesOrderNum, strConn, Me.CurrentSession.VT_tlsNumFields)


        Dim dtTVGrid As New DataTable
        Dim dtSOItems As New DataTable
        Dim objDataPreserve As New BPADotNetCommonFunctions.VT_DataPreserve.DataPreserve
        Dim objDBF As New MatrixFunctionsForSales.MatrixFunctions_Sales
        Dim intSelectedRowIndex As Integer
        Dim intSalesOrderNum As Integer
        Dim intSOItemRow As Integer
        Dim strColName As String
        Dim strColValue As String
        Dim blnOrderItemsExist As Boolean
        Dim objSales As New BPADotNetCommonFunctions.SalesFunctions.Sales_Orders

        Dim strConn As String = Session("_VT_DotNetConnString")

        'Read the OnHoldTV grid
        dtTVGrid = objDataPreserve.GetWDGDataFromSession(wdgRepGrid)

        If dtTVGrid.Columns.Contains("Item_OnHoldPersonResponsibleID") = False Then
            dtTVGrid.Columns.Add("Item_OnHoldPersonResponsibleID")
        End If
       
        'Set the pointers to the correct row
        intSelectedRowIndex = Session("_VT_TruckViewRowSelected").rows(0).item("VT_ActiveRowIndex")
        intSalesOrderNum = Session("_VT_TruckViewRowSelected").rows(0).item("SalesOrderNum")

        'Now we need to get the Sales order items for this sales order number from the matrix
        dtSOItems = objSales.GetSalesOrderItemsFromMatrix("OrderItems", intSalesOrderNum, strConn, Me.CurrentSession.VT_tlsNumFields)
        If dtSOItems.Columns.Contains("Item_OnHoldPersonResponsibleID") = False Then
            dtSOItems.Columns.Add("Item_OnHoldPersonResponsibleID")
        End If

        If dtSOItems.Rows.Count = 0 Then
            blnOrderItemsExist = False
        Else
            blnOrderItemsExist = True
        End If


        'loop through the columns and update the relevant values
        For intSOItemRow = 0 To dtSOItems.Rows.Count - 1
            For intCnt = 0 To astrColNamesAndValues.GetLength(0) - 1

                strColName = astrColNamesAndValues(intCnt, 0)
                strColValue = astrColNamesAndValues(intCnt, 1)

                ' dtTVGrid.Rows(intSelectedRowIndex).Item(strColName) = strColValue

                dtSOItems.Rows(intSOItemRow).Item(strColName) = strColValue
                If strColName = "Item_OnHoldPersonResponsibleID" Then
                    If ddlAssignToCust.Items(0).Selected = True Then

                        Dim strsql As String = "Update cus_Customers set CustomerPersonResponsible = " & ddlUsers_NS.SelectedItem.Value & " where CustomerId = " & dtTVGrid.Rows(intSelectedRowIndex).Item("CustomerId")
                        Dim objdb As New VTDBFunctions.VTDBFunctions.DBFunctions
                        objdb.ExecuteSQLQuery(strsql)


                    End If
                End If
            Next
        Next

        'loop through the columns and update the relevant values
        For intSOItemRow = 0 To dtTVGrid.Rows.Count - 1
            If dtTVGrid.Rows(intSOItemRow).Item("SalesOrderNum") = intSalesOrderNum Then
                For intCnt = 0 To astrColNamesAndValues.GetLength(0) - 1

                    strColName = astrColNamesAndValues(intCnt, 0)
                    strColValue = astrColNamesAndValues(intCnt, 1)

                    dtTVGrid.Rows(intSOItemRow).Item(strColName) = strColValue

                Next
            End If

        Next

        'Rebind the ONHoldTV grid
        objDataPreserve.BindDataToWDG(dtTVGrid, wdgRepGrid)

        'Now write out the updated 'dtSOItems' data
        objDataPreserve.BindDataToWDG(dtSOItems, wdgOrderItems)

        Dim intNumDays As Integer

        'Apply Grid Formatting here
        For intLoop As Integer = 0 To dtTVGrid.Rows.Count - 1
            If Trim(dtTVGrid.Rows(intLoop).Item("DateOnHold")) <> "" Then
                intNumDays = dtTVGrid.Rows(intLoop).Item("DateOnHold")
                If intNumDays >= 5 Then
                    'Set the formatting on the grid - Set the Background colour
                    wdgRepGrid.Rows(intLoop).CssClass = "Row_Salmon"
                ElseIf intNumDays >= 3 Then
                    'Set the formatting on the grid - Set the Background colour
                    wdgRepGrid.Rows(intLoop).CssClass = "Row_Yellow"
                End If

            End If

        Next
        'Save Order Items to the MATRIX table
        objSales.SaveSalesItemsToMatrix("OrderItems", wdgOrderItems, intSalesOrderNum, strConn, Me.CurrentSession.VT_tlsNumFields)

        UpdateGridCellValue = True

    End Function

    Protected Sub btnExport_Click(sender As Object, e As ImageClickEventArgs) Handles btnExport.Click
        VerifyExporter.DownloadName = "SalesOrders_TruckView_Dashboard_VerifyTechnologies"
        VerifyExporter.Export(wdgRepGrid)
    End Sub

    '----All comments related functions
    Protected Sub cmdSaveComment_Click(sender As Object, e As ImageClickEventArgs) Handles cmdSaveComment.Click
        If Session("_VT_TruckViewRowSelected") IsNot Nothing Then

            Dim astrColNamesAndValues(0, 1) As String
            astrColNamesAndValues(0, 0) = "Item_OnHoldTVComment"
            astrColNamesAndValues(0, 1) = txtHoldTVComment_NS.Text

            UpdateCommentForAllOrderItems(astrColNamesAndValues)
        End If



    End Sub

    Protected Sub btnViewComments_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs)

        SetUpOrderSessionVariables()

        'Setup the appropriate return path
        Me.CurrentSession.OptionsPageToReturnTo = "~/TabPages/TruckViewDashboard.aspx"

        Response.Redirect("~/Comments_Pages/OrderFormComments.aspx")


    End Sub

    Function UnreadCommentsExist(intFormId As Integer, strUsername As String) As Boolean
        Dim objApp As New CommentFunctions
        Dim dtThreads As Data.DataTable = objApp.GetThreads(intFormId)

        Dim dtComments As New Data.DataTable
        Dim intThreadId As Integer
        Dim i, j As Integer
        Dim objPer As New BPADotNetCommonFunctions.PersonnelModuleFunctions

        UnreadCommentsExist = False

        For i = 0 To dtThreads.Rows.Count - 1
            intThreadId = dtThreads.Rows(i).Item("Field0")
            dtComments = objApp.GetThreadComments(intThreadId)
            For j = 0 To dtComments.Rows.Count - 1
                'check if the target is the currently logged in user or 'everybody'
                If Trim(dtComments.Rows(j).Item("Field9")) = strUsername Or Trim(UCase(dtComments.Rows(j).Item("Field9"))) = "EVERYBODY" Then
                    If IsDBNull(dtComments.Rows(j).Item("Field4")) OrElse dtComments.Rows(j).Item("Field4") = "" Then
                        'if that field is empty then the comment is not yet viewed
                        UnreadCommentsExist = True
                        Exit Function
                    End If
                End If
            Next

        Next

    End Function
    Public Function UpdateCommentForAllOrderItems(ByVal astrColNamesAndValues(,) As String) As Boolean
        'AM 2014-08-18 When Steripack enter an comment for one order item on hold, they want that comment to be saved for all items 
        'in the order that do not already contain a comment

        Dim dtTVGrid As New DataTable
        Dim dtSOItems As New DataTable
        Dim objDataPreserve As New BPADotNetCommonFunctions.VT_DataPreserve.DataPreserve
        Dim objDBF As New MatrixFunctionsForSales.MatrixFunctions_Sales
        Dim intSelectedRowIndex As Integer
        Dim intSalesOrderNum As Integer
        Dim intSOItemRow As Integer
        Dim strColName As String
        Dim strColValue As String
        Dim blnOrderItemsExist As Boolean
        Dim objSales As New BPADotNetCommonFunctions.SalesFunctions.Sales_Orders

        Dim strConn As String = Session("_VT_DotNetConnString")

        'Read the OnHoldTV grid
        dtTVGrid = objDataPreserve.GetWDGDataFromSession(wdgRepGrid)

        'Set the pointers to the correct row
        intSelectedRowIndex = Session("_VT_TruckViewRowSelected").rows(0).item("VT_ActiveRowIndex")
        intSalesOrderNum = Session("_VT_TruckViewRowSelected").rows(0).item("SalesOrderNum")

        'Now we need to get the Sales order items for this sales order number from the matrix
        dtSOItems = objSales.GetSalesOrderItemsFromMatrix("OrderItems", intSalesOrderNum, strConn, Me.CurrentSession.VT_tlsNumFields)

        If dtSOItems.Columns.Contains("ViewHistory") = False Then
            dtSOItems.Columns.Add("ViewHistory")
        End If
        If dtSOItems.Rows.Count = 0 Then
            blnOrderItemsExist = False
        Else
            blnOrderItemsExist = True
        End If


        'loop through the columns and update the relevant values
        For intSOItemRow = 0 To dtSOItems.Rows.Count - 1
            For intCnt = 0 To astrColNamesAndValues.GetLength(0) - 1

                strColName = astrColNamesAndValues(intCnt, 0)
                strColValue = astrColNamesAndValues(intCnt, 1)

                ' dtTVGrid.Rows(intSelectedRowIndex).Item(strColName) = strColValue

                dtSOItems.Rows(intSOItemRow).Item(strColName) = strColValue

            Next
        Next

        'loop through the columns and update the relevant values
        For intSOItemRow = 0 To dtTVGrid.Rows.Count - 1
            If dtTVGrid.Rows(intSOItemRow).Item("SalesOrderNum") = intSalesOrderNum Then
                For intCnt = 0 To astrColNamesAndValues.GetLength(0) - 1

                    strColName = astrColNamesAndValues(intCnt, 0)
                    strColValue = astrColNamesAndValues(intCnt, 1)

                    dtTVGrid.Rows(intSOItemRow).Item(strColName) = strColValue

                Next
            End If

        Next

        'Rebind the ONHoldTV grid
        objDataPreserve.BindDataToWDG(dtTVGrid, wdgRepGrid)

        'Now write out the updated 'dtSOItems' data
        objDataPreserve.BindDataToWDG(dtSOItems, wdgOrderItems)

        Dim intNumDays As Integer

       
        'Save Order Items to the MATRIX table
        objSales.SaveSalesItemsToMatrix("OrderItems", wdgOrderItems, intSalesOrderNum, strConn, Me.CurrentSession.VT_tlsNumFields)

        'also add this comment to the comments engine
        Dim objApp As New CommentFunctions
        Dim strCommentTarget As String
        Dim intCommentTargetId As Integer

        strCommentTarget = GetGlobalResourceObject("Resource", "Everybody")
        intCommentTargetId = 0


        Dim strCommentSource As String = Session("_VT_CurrentUserName")
        Dim intCommentSourceId As Integer = Session("_VT_CurrentUserId")
        Dim intIdToUse As Integer

        intIdToUse = Me.CurrentSession.VT_NewOrderMatrixID


        objApp.StoreComment(intIdToUse, astrColNamesAndValues(0, 1), "", Nothing, strCommentSource, strCommentTarget, intCommentSourceId, intCommentTargetId)


        UpdateCommentForAllOrderItems = True
    End Function


    '--------End of section containing all comments related functions
    Protected Sub btnViewOrder_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs)

        SetUpOrderSessionVariables()

        'Setup the appropriate return path
        Session("_VT_SalesOrderReturnPath") = "TabPages/TruckViewDashboard.aspx"

        'save the currently selected customer
        Dim dtTemp As New DataTable
        dtTemp = Session("_VT_TruckViewRowSelected")

        Dim objCust As New VTDBFunctions.VTDBFunctions.CustomerFunctions

        If dtTemp.Rows.Count > 0 Then
            With dtTemp.Rows(0)

                Session("SelectedCustomerID") = objCust.GetCustomerIdForRef(.Item("SoldTo_Code"))
                Me.CurrentSession.strNewOrderCustomerName = objCust.GetCustomerNameForId(Session("SelectedCustomerId"))
                Me.CurrentSession.VT_CustomerPO = IIf(IsDBNull(.Item("Other_CustPO")), "", .Item("Other_CustPO"))

            End With
        End If


        'Redirect to the Products Page
        Response.Redirect(Me.CurrentSession.aVT_DetailsPageOptionsPages(1))

    End Sub


    Function SetUpOrderSessionVariables() As Boolean

        Dim objForms As New VT_Forms.Forms
        Dim dtTemp As New DataTable
        dtTemp = Session("_VT_TruckViewRowSelected")

        If dtTemp.Rows.Count > 0 Then
            With dtTemp.Rows(0)

                'Set the control variables for the currently selected Sales Order
                Dim lngId As Long = CType(.Item("SalesOrderNum"), Long)
                Me.CurrentSession.VT_JobID = lngId
                Me.CurrentSession.VT_SalesOrderNum = lngId
                Dim lngSalesOrderID As Long = .Item("SalesOrderId")
                Me.CurrentSession.VT_SalesOrderID = lngSalesOrderID

                Me.CurrentSession.VT_SalesContiguousNum = .Item("SO_ContiguousNum")

                Me.CurrentSession.VT_CustomerName = .Item("SoldTo_Name")


                Me.CurrentSession.VT_NewOrderMatrixID = objForms.GetMatrixLinkIdForJob(lngId)
                Me.CurrentSession.VT_OrderMatrixID = objForms.GetMatrixLinkIdForJob(lngId)

                Me.CurrentSession.VT_NewOrderNum = Me.CurrentSession.VT_SalesOrderNum
                Me.CurrentSession.VT_NewOrderID = Me.CurrentSession.VT_SalesOrderID
                Me.CurrentSession.VT_CurrentNewOrderStatus = Me.CurrentSession.VT_JobStatus


            End With
        Else

        End If

        SetUpOrderSessionVariables = True

    End Function

    Function SetUpOrderSessionVariablesPerRow(dtTemp As DataRow) As Boolean

        Dim objForms As New VT_Forms.Forms



        With dtTemp

            'Set the control variables for the currently selected Sales Order
            Dim lngId As Long = CType(.Item("SalesOrderNum"), Long)
            Me.CurrentSession.VT_JobID = lngId
            Me.CurrentSession.VT_SalesOrderNum = lngId
            Dim lngSalesOrderID As Long = .Item("SalesOrderId")
            Me.CurrentSession.VT_SalesOrderID = lngSalesOrderID

            Me.CurrentSession.VT_SalesContiguousNum = .Item("SO_ContiguousNum")

            Me.CurrentSession.VT_CustomerName = .Item("SoldTo_Name")


            Me.CurrentSession.VT_NewOrderMatrixID = objForms.GetMatrixLinkIdForJob(lngId)
            Me.CurrentSession.VT_OrderMatrixID = objForms.GetMatrixLinkIdForJob(lngId)

            Me.CurrentSession.VT_NewOrderNum = Me.CurrentSession.VT_SalesOrderNum
            Me.CurrentSession.VT_NewOrderID = Me.CurrentSession.VT_SalesOrderID
            Me.CurrentSession.VT_CurrentNewOrderStatus = Me.CurrentSession.VT_JobStatus


        End With


        SetUpOrderSessionVariablesPerRow = True

    End Function

  



    Protected Sub btnViewPrices_Click(sender As Object, e As ImageClickEventArgs) Handles btnViewPrices.Click
        Dim dtTemp As New DataTable
        dtTemp = Session("_VT_TruckViewRowSelected")
        Dim objProd As New VTDBFunctions.VTDBFunctions.ProductsFunctions
        Dim objCust As New VTDBFunctions.VTDBFunctions.CustomerFunctions

        If dtTemp.Rows.Count > 0 Then
            With dtTemp.Rows(0)

                Session("SelectedProductId") = objProd.GetProductIdForCode(.Item("ProductCode"))

                Session("SelectedCustomerID") = objCust.GetCustomerIdForRef(.Item("SoldTo_Code"))
                If Session("SelectedProductId") = "" Then 'that product is not yet in our system, we can't edit the prices for it
                    lblMsg.Text = "You cannot add or edit prices for a product that has not yet been added to the eq trace database."
                    ModalPopupExtenderMsg.Show()
                    Exit Sub

                End If
                'Setup the appropriate return path
                Me.CurrentSession.OptionsPageToReturnTo = "~/TabPages/TruckViewDashboard.aspx"
                Response.Redirect("~/TabPages/EditPriceList.aspx")
            End With
        End If

    End Sub


    Protected Sub wdgOrderItems_ActiveCellChanged(sender As Object, e As Infragistics.Web.UI.GridControls.ActiveCellEventArgs) Handles wdgOrderItems.ActiveCellChanged
        Session("_VT_TruckViewRowSelected") = e.CurrentActiveCell.Row
        Session("_VT_TruckViewRowSelectedIndex") = e.CurrentActiveCell.Row.Index
    End Sub

    Protected Sub wdgOrderItems_InitializeRow(sender As Object, e As Infragistics.Web.UI.GridControls.RowEventArgs) Handles wdgOrderItems.InitializeRow

    End Sub

    ' ''Protected Sub wdgRepGrid_InitializeRow(sender As Object, e As Infragistics.Web.UI.GridControls.RowEventArgs) Handles wdgRepGrid.InitializeRow
    ' ''    'pull the image from a resource'
    ' ''    Dim exclamationIcon As Bitmap = Resources.Resource.CommentsUnread

    ' ''    'get the data source of the row, I only want this image to appear under certain'
    ' ''    'conditions    '

    ' ''    If e.Row.Items.FindItemByKey("UnreadComments").Text = "!" Then
    ' ''        'here the condition is met, set the image on the correct column, the one'
    ' ''        ' with key of "Descriptor"'
    ' ''        e.Row.Items.FindItemByKey("NotifyUnreadComments").image = "~/App_Themes/Grid Buttons/CommentsUnread.bmp"

    ' ''    Else
    ' ''        e.Row.Items.FindItemByKey("NotifyUnreadComments").Text = ""

    ' ''    End If
    ' ''End Sub

    Protected Sub wdgRepGrid_InitializeRow(sender As Object, e As Infragistics.Web.UI.GridControls.RowEventArgs) Handles wdgRepGrid.InitializeRow
        'If e.Row.Items.FindItemByKey("ShowLocked").Text = "YES" Then
        '    e.Row.Items.FindItemByKey("ShowLocked").Text = "<img src='lock.gif'></img>"
        'End If
        'If e.Row.Items.FindItemByKey("UnreadComments").Text = "MFG" Then
        '    e.Row.Items.FindItemByKey("UnreadComments").Text = "<img src='mail.gif'></img>"
        'End If
    End Sub


    Protected Sub btnAddNewComment_Click(sender As Object, e As ImageClickEventArgs) Handles btnAddNewComment.Click
        Dim strSalesORderNum As String
        Dim dtTemp As New DataTable
        Dim strLock As String
        Dim strUserName As String
        Dim struserID As String

        dtTemp = Session("_VT_TruckViewRowSelected")

        If dtTemp.Rows.Count > 0 Then
            strSalesORderNum = dtTemp.Rows(0).Item("SalesOrderNum")
            strLock = IsThisOrderLockedForEdit(strSalesORderNum)
            If strLock <> "NO" Then 'it is locked, show a message
                strUserName = Mid(strLock, InStr(strLock, "UserName:") + 9)
                strUserName = Left(strUserName, InStr(strUserName, ",") - 1)
                struserID = Mid(strLock, InStr(strLock, "UserId:") + 7)
                struserID = Left(struserID, InStr(struserID, ",") - 1)
                hdnAlertUserId.Value = struserID
                hdnAlertJobId.Value = strSalesORderNum
                hdnAlertSONum.Value = dtTemp.Rows(0).Item("SO_ContiguousNum")

                'if it is already checked out to the currently logged in user then they can edit it
                If Session("_VT_CurrentUserId").ToString = struserID Then
                    ModalPopupExtenderMessage.Show()
                Else
                    lblTask.Text = "The selected order is currently being edited by: " & strUserName & ". It cannot be edited until that user has finished with it."
                    ModalPopupExtenderTask.Show()
                End If


            Else 'not locked, check it out to the current user and continue
                If CheckOrderOutToCurrentUser() Then

                    ModalPopupExtenderMessage.Show()
                End If


            End If

        Else
            lblMsg.Text = "You must select a line first before you add or edit a comment!"
            ModalPopupExtenderMsg.Show()

        End If

    End Sub
    Function CheckOrderOutToCurrentUser() As Boolean
        'SmcN 22/05/2014  Handle Edit/Lock facilities.
        Dim strFormLock As String = ""
        Dim objPres As New VT_DataPreserve.DataPreserve
        Dim objVTM As New VTDBFunctions.VTDBFunctions.VTMatrixFunctions
        Dim objData As New BPADotNetCommonFunctions.VT_DataPreserve.DataPreserve
        Dim intIdToUse As Integer
        Dim strMessage As String
        Dim objForms As New VT_Forms.Forms
        Dim lngUserId As Long = Session("_VT_CurrentUserId")


        'SmcN 02/06/2014 If the order status has progressed too far then you are not allowed to edit the Order
        If Me.CurrentSession.VT_CurrentNewOrderStatus = GetGlobalResourceObject("Resource", "Order_SendAcknowledge") Then
            CheckOrderOutToCurrentUser = False

            Exit Function
        End If

        If Me.CurrentSession.VT_CurrentNewOrderStatus = GetGlobalResourceObject("Resource", "Order_AwaitingPlanning") Then
            CheckOrderOutToCurrentUser = False

            Exit Function
        End If
        If Me.CurrentSession.VT_CurrentNewOrderStatus.Contains("Closed") Then
            CheckOrderOutToCurrentUser = False

            Exit Function

        End If


        If IsNothing(Me.CurrentSession.VT_OrderMatrixID) OrElse Me.CurrentSession.VT_OrderMatrixID = 0 Then
            'Do nothing the Form should have defaulted to Checked Out - Edit Mode on page load if this was a new item
            CheckOrderOutToCurrentUser = False
        Else
            intIdToUse = Me.CurrentSession.VT_OrderMatrixID
            ' get the CheckIn status for this item
            strFormLock = objVTM.ReadFormLock(Session("_VT_DotNetConnString"), intIdToUse, Me.CurrentSession.FormDataTable)
            If strFormLock = "" Then
                ' if it is not checked out then check it out to this user

                ' the form is not currently locked so we can lock it (check it out) to the current user
                objVTM.LockFormForWriting(Session("_VT_DotNetConnString"), intIdToUse, Me.CurrentSession.FormDataTable, objData.CreateFormLockString(Session("_VT_CurrentUserName"), Session("_VT_UserHostAddress"), Session("_VT_CurrentUserId")))

                Me.CurrentSession.VT_FormCheckedOutToCurrentUser = True
                CheckOrderOutToCurrentUser = True
            End If

        End If
    End Function

    Sub CheckOrderBackIn()
        Dim objPres As New VT_DataPreserve.DataPreserve
        Dim objVTM As New VTDBFunctions.VTDBFunctions.VTMatrixFunctions
        Dim objData As New BPADotNetCommonFunctions.VT_DataPreserve.DataPreserve
        Dim intIdToUse As Integer
        Dim strFormLock As String = ""


        If IsNothing(Me.CurrentSession.VT_OrderMatrixID) OrElse Me.CurrentSession.VT_OrderMatrixID = 0 Then
            'Do nothing the Form should have defaulted to Checked Out - Edit Mode on page load if this was a new item
        Else
            intIdToUse = Me.CurrentSession.VT_OrderMatrixID

            Dim blnIsFormCheckedOutByCurrentUser As Boolean = objData.IsFormCheckedOutByCurrentUser(Session("_VT_DotNetConnString"), Session("_VT_CurrentUserId"), intIdToUse, Me.CurrentSession.FormDataTable)
            If blnIsFormCheckedOutByCurrentUser = True Then
                objVTM.LockFormForWriting(Session("_VT_DotNetConnString"), intIdToUse, Me.CurrentSession.FormDataTable, "")


                Me.CurrentSession.VT_FormCheckedOutToCurrentUser = False

            End If

        End If
    End Sub

    Protected Sub cmdTaskOk_Click(sender As Object, e As ImageClickEventArgs) Handles cmdTaskOk.Click

        'this code is ready to add a task for the user who has an order checked out. Commenting out for now because the alert
        'part of the task system is not ready.

        'user has chosen to send an alert to the person who has the order checked out. We need to add a task for that user and 
        'mark it as an alert. 

        'Dim strTargetUser As String = hdnAlertUserId.Value
        'Dim intTargetId As Integer
        'Dim objTasks As New BPADotNetCommonFunctions.VT_eQOInterface.eQOInterface
        'Dim strJobId As String = hdnAlertJobId.Value
        'Dim strTaskDesc As String = "Please check in sales order number: " & hdnAlertSONum.Value & ". Another user would like to edit that order."
        'If IsNumeric(strTargetUser) Then
        '    intTargetId = CInt(strTargetUser)
        '    objTasks.CreateNewTaskV2(IIf(IsNumeric(strJobId), CInt(strJobId), 0), 3, intTargetId, Date.Today, strTaskDesc, "", "SalesOrderCheckInRequested", "", Date.Today, Session("_VT_CurrentUserId"), "", 0, 0, "", 0, "", "", "", "", "", "", "", "", 0, hdnAlertSONum.Value, , , , , , True)

        'End If

    End Sub

    Protected Sub cmdCancelComment_Click(sender As Object, e As ImageClickEventArgs) Handles cmdCancelComment.Click
        CheckOrderBackIn()
    End Sub


    Protected Sub btnAssignToPerson_Click(sender As Object, e As ImageClickEventArgs) Handles btnAssignToPerson.Click
        Dim strSalesORderNum As String
        Dim dtTemp As New DataTable
        Dim strLock As String
        Dim strUserName As String
        Dim struserID As String

        dtTemp = Session("_VT_TruckViewRowSelected")

        If dtTemp.Rows.Count > 0 Then
            strSalesORderNum = dtTemp.Rows(0).Item("SalesOrderNum")
            strLock = IsThisOrderLockedForEdit(strSalesORderNum)
            If strLock <> "NO" Then 'it is locked, show a message
                strUserName = Mid(strLock, InStr(strLock, "UserName:") + 9)
                strUserName = Left(strUserName, InStr(strUserName, ",") - 1)
                struserID = Mid(strLock, InStr(strLock, "UserId:") + 7)
                struserID = Left(struserID, InStr(struserID, ",") - 1)
                hdnAlertUserId.Value = struserID
                hdnAlertJobId.Value = strSalesORderNum
                hdnAlertSONum.Value = dtTemp.Rows(0).Item("SO_ContiguousNum")

                'if it is already checked out to the currently logged in user then they can edit it
                If Session("_VT_CurrentUserId").ToString = struserID Then
                    ModalPopupExtenderPersonAssign.Show()
                Else
                    lblTask.Text = "The selected order is currently being edited by: " & strUserName & ". It cannot be edited until that user has finished with it."
                    ModalPopupExtenderTask.Show()
                End If


            Else 'not locked, check it out to the current user and continue
                If CheckOrderOutToCurrentUser() Then

                    ModalPopupExtenderPersonAssign.Show()
                End If


            End If

        Else
            lblMsg.Text = "You must select a line first before you assign a person!"
            ModalPopupExtenderMsg.Show()

        End If
    End Sub

    Protected Sub cmdPersonAssignCancel_Click(sender As Object, e As ImageClickEventArgs) Handles cmdPersonAssignCancel.Click
        CheckOrderBackIn()

    End Sub
End Class



