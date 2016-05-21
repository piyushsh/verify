Imports BPADotNetCommonFunctions
Imports BPADotNetCommonFunctions.VT_CommonFunctions.MatrixFunctions
Imports System.Data.SqlClient
Imports System.Data
Imports System.Drawing
Imports System.Data.DataTableExtensions
Imports Infragistics.Web.UI.GridControls

Partial Class TabPages_PlanningOrdersNew
    Inherits MyBasePage

    Private _getMatrixOrderDataForOrderNum As DataTable

    Private Property GetMatrixOrderDataForOrderNum(dtOtherStatus As DataTable) As DataTable
        Get
            Return _getMatrixOrderDataForOrderNum
        End Get
        Set(value As DataTable)
            _getMatrixOrderDataForOrderNum = value
        End Set
    End Property




    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim objDataPreserve As New BPADotNetCommonFunctions.VT_DataPreserve.DataPreserve



        If Not IsPostBack Then
            FillData()
            InitialiseAndBindEmptyOrderItemsGrid()
            InitialiseAndBindEmptyServiceItemsGrid()
            If Session("Accounts") = "MFGPRO" Then
                With wdgRepGrid
                    .Columns("Site").Hidden = False
                    .Columns("Kanban").Hidden = False

                End With
            Else
                With wdgRepGrid
                    .Columns("Site").Hidden = True
                    .Columns("Kanban").Hidden = True

                End With
            End If
        Else
            wdgRepGrid.DataSource = objDataPreserve.GetWDGDataFromSession(wdgRepGrid)
            wdgOpenOtherStatus.DataSource = objDataPreserve.GetWDGDataFromSession(wdgOpenOtherStatus)
            wdgOrderItems.DataSource = objDataPreserve.GetWDGDataFromSession(wdgOrderItems)
            wdgServiceItems.DataSource = objDataPreserve.GetWDGDataFromSession(wdgServiceItems)
        End If

        'SmcN 26/01/2015 Set the grid width if required
        If Me.CurrentSession.VT_BrowserWindowWidth < 1400 And Not Me.CurrentSession.VT_BrowserWindowWidth Is Nothing Then
            wdgRepGrid.Width = Me.CurrentSession.VT_BrowserWindowWidth - 54
            wdgOpenOtherStatus.Width = Me.CurrentSession.VT_BrowserWindowWidth - 54
        End If

    End Sub

    Public Function GetOrderDataForStatus(ByVal strconn As String, ByVal strStatus As String) As DataTable

        'This function returns All sales Order numbers and status for a given status

        Dim strsql As String = ""
        Dim dt As New DataTable

        strsql = strsql + "SELECT SalesOrderNum, Status, DateCreated"
        strsql = strsql + " from tls_SalesOrders "
        strsql = strsql + " WHERE tls_SalesOrders.Status = '" + strStatus + "' "

        Dim myConnection As New SqlConnection(strconn)
        Dim comSQL As New SqlCommand(strsql, myConnection)

        comSQL.CommandTimeout = 300

        Dim myAdapter As New SqlDataAdapter(comSQL)

        myConnection.Open()
        myAdapter.Fill(dt)

        'Close the connection when done with it.
        myConnection.Close()

        GetOrderDataForStatus = dt

    End Function



    Sub FillData()
        Dim objG As New BPADotNetCommonFunctions.VT_ReportFunctions.GeneralFunctions
        Dim objSales As New BPADotNetCommonFunctions.SalesFunctions.Sales_Orders

        Dim strConn As String = Session("_VT_DotNetConnString")


        Dim objDataPreserve As New BPADotNetCommonFunctions.VT_DataPreserve.DataPreserve
        Dim dtoutput As New DataTable
        Dim strStatusSearch As String = GetGlobalResourceObject("Resource", "Order_AwaitingPlanning")
        Dim dt, dtOtherStatus, dtOtherStatusOutput As DataTable

        dt = GetOrderDataForStatus(Session("_VT_DotNetConnString"), strStatusSearch)
        If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
            'get data from grids
            dtoutput = objSales.GetMatrixOrderDataForOrderNum(dt, strConn, Me.CurrentSession.VT_tlsNumFields)
            If dtoutput IsNot Nothing AndAlso dtoutput.Rows.Count > 0 Then
                'Clean up the DataTable columns
                Dim astrColNames(4, 1) As String
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

                dtoutput = objG.CleanColumnFormats(dtoutput, astrColNames)

                objDataPreserve.BindDataToWDG(dtoutput, wdgRepGrid)
            Else
                InitialiseAndBindEmptyPlanningGrid()
            End If
        Else
            InitialiseAndBindEmptyPlanningGrid()
        End If

        'Save a copy of this data table for checking which Values have changed when storing changes later.
        Session("_VT_AwaitingPlaning") = dtoutput.Copy

        'build where clause
        strStatusSearch = GetGlobalResourceObject("Resource", "Order_AwaitingIssue")
        strStatusSearch = strStatusSearch + "' OR tls_SalesOrders.Status = '"
        strStatusSearch = strStatusSearch + GetGlobalResourceObject("Resource", "Order_OnHold")
        strStatusSearch = strStatusSearch + "' OR tls_SalesOrders.Status = '"
        strStatusSearch = strStatusSearch + GetGlobalResourceObject("Resource", "Order_Open")
        strStatusSearch = strStatusSearch + "' OR tls_SalesOrders.Status = '"
        strStatusSearch = strStatusSearch + GetGlobalResourceObject("Resource", "Order_OpenPartShipped")
        strStatusSearch = strStatusSearch + "' OR tls_SalesOrders.Status = '"
        strStatusSearch = strStatusSearch + GetGlobalResourceObject("Resource", "Order_New")
        strStatusSearch = strStatusSearch + "' OR tls_SalesOrders.Status = '"
        strStatusSearch = strStatusSearch + GetGlobalResourceObject("Resource", "Order_PreIssued")
        strStatusSearch = strStatusSearch + "' OR tls_SalesOrders.Status = '"
        strStatusSearch = strStatusSearch + GetGlobalResourceObject("Resource", "Order_SendAcknowledge")

        dtOtherStatus = GetOrderDataForStatus(Session("_VT_DotNetConnString"), strStatusSearch)

        If dtOtherStatus IsNot Nothing AndAlso dtOtherStatus.Rows.Count > 0 Then
            'get data from grids
            dtOtherStatusOutput = objSales.GetMatrixOrderDataForOrderNum(dtOtherStatus, strConn, Me.CurrentSession.VT_tlsNumFields)

            If dtOtherStatusOutput IsNot Nothing AndAlso dtOtherStatusOutput.Rows.Count > 0 Then
                'Clean up the DataTable columns
                Dim astrColNames(2, 1) As String
                astrColNames(0, 0) = "Item_DateOut"
                astrColNames(0, 1) = "CONVERT_TO_DATE"
                astrColNames(1, 0) = "DateCreated"
                astrColNames(1, 1) = "CONVERT_TO_DATE"
                astrColNames(2, 0) = "Item_DateRequested"
                astrColNames(2, 1) = "CONVERT_TO_DATE"

                dtOtherStatusOutput = objG.CleanColumnFormats(dtOtherStatusOutput, astrColNames)
                objDataPreserve.BindDataToWDG(dtOtherStatusOutput, wdgOpenOtherStatus)
            Else
                InitialiseAndBindEmptyOpenOrderGrid()
            End If
        Else
            InitialiseAndBindEmptyOpenOrderGrid()
        End If

    End Sub

    Protected Sub btnSaveDateChanges_Click(sender As Object, e As EventArgs) Handles btnSaveDateChanges.Click

        SavePlanningDates()
        PlanningGridUpdatePanel.Update()


    End Sub

    Function SavePlanningDates() As Boolean

        'SmcN 03/03/2014  This function compares the current state of the Planning grid with a copy stored when the page was opened
        ' Any changes to the dates are then stored to the relevant Sales Order - Order Items Matrix 

        'NOTE: There is a hidden Sales Order Items grid on this page that must match the sales Order items grid on the Add Products Page !

        Dim dtTemp As New DataTable
        Dim dtCompare As New DataTable
        Dim dtOrderDetails As New DataTable
        Dim dtTempOrderRows As New DataTable
        Dim objDataPreserve As New BPADotNetCommonFunctions.VT_DataPreserve.DataPreserve
        Dim objSales As New BPADotNetCommonFunctions.SalesFunctions.Sales_Orders

        Dim strConn As String = Session("_VT_DotNetConnString")

        Dim objG As New BPADotNetCommonFunctions.VT_ReportFunctions.GeneralFunctions
        Dim intCnt As Integer = 0
        Dim intLoopCnt As Integer = 0
        Dim strCurrentSalesOrderNum As String = ""
        Dim strSearch As String

        'Get the data source of the Planning grid
        dtTemp = objDataPreserve.GetWDGDataFromSession(wdgRepGrid)

        'Get the copy of the Planning grid stored when the page was opened.
        dtCompare = Session("_VT_AwaitingPlaning")

        'Sort both grids on SalesOrderNum and Index field to ensure we will be comparing the correct rows against each other
        dtTemp.DefaultView.Sort = "SalesOrderNum, VT_UniqueIndex" & " ASC"
        dtCompare.DefaultView.Sort = "SalesOrderNum, VT_UniqueIndex" & " ASC"

        'loop through the data grid and write out any dates that have changed
        If dtTemp.Rows.Count > 0 Then

            For intCnt = 0 To dtTemp.Rows.Count - 1

                'SmcN 02/05/2014 Set these value to a date type so that comparison tests dont blow up
                If IsDBNull(dtTemp.Rows(intCnt).Item("Item_DateOut")) Then
                    dtTemp.Rows(intCnt).Item("Item_DateOut") = Date.MinValue
                Else
                    If IsDate(dtTemp.Rows(intCnt).Item("Item_DateOut")) = False Then
                        If dtTemp.Rows(intCnt).Item("Item_DateOut") = "" Then
                            dtTemp.Rows(intCnt).Item("Item_DateOut") = Date.MinValue
                        End If
                    End If

                End If
                If IsDBNull(dtTemp.Rows(intCnt).Item("Item_DateArrival")) Then
                    dtTemp.Rows(intCnt).Item("Item_DateArrival") = Date.MinValue
                Else
                    If IsDate(dtTemp.Rows(intCnt).Item("Item_DateArrival")) = False Then
                        If dtTemp.Rows(intCnt).Item("Item_DateArrival") = "" Then
                            dtTemp.Rows(intCnt).Item("Item_DateArrival") = Date.MinValue
                        End If
                    End If

                End If

                If IsDBNull(dtCompare.Rows(intCnt).Item("Item_DateOut")) Then
                    dtCompare.Rows(intCnt).Item("Item_DateOut") = Date.MinValue
                Else
                    If IsDate(dtTemp.Rows(intCnt).Item("Item_DateOut")) = False Then
                        If dtTemp.Rows(intCnt).Item("Item_DateOut") = "" Then
                            dtTemp.Rows(intCnt).Item("Item_DateOut") = Date.MinValue
                        End If
                    End If

                End If

                If IsDBNull(dtCompare.Rows(intCnt).Item("Item_DateArrival")) Then
                    dtCompare.Rows(intCnt).Item("Item_DateArrival") = Date.MinValue
                Else
                    If IsDate(dtTemp.Rows(intCnt).Item("Item_DateArrival")) = False Then
                        If dtTemp.Rows(intCnt).Item("Item_DateArrival") = "" Then
                            dtTemp.Rows(intCnt).Item("Item_DateArrival") = Date.MinValue
                        End If
                    End If

                End If

                If IsDBNull(dtCompare.Rows(intCnt).Item("MachineNum")) Then
                    dtCompare.Rows(intCnt).Item("MachineNum") = ""
                End If
                If IsDBNull(dtCompare.Rows(intCnt).Item("Kanban")) Then
                    dtCompare.Rows(intCnt).Item("Kanban") = False
                End If
                If IsDBNull(dtTemp.Rows(intCnt).Item("MachineNum")) Then
                    dtTemp.Rows(intCnt).Item("MachineNum") = ""
                End If
                If IsDBNull(dtTemp.Rows(intCnt).Item("Kanban")) Then
                    dtTemp.Rows(intCnt).Item("Kanban") = False

                End If
                If dtTemp.Rows(intCnt).Item("Item_DateOut") <> dtCompare.Rows(intCnt).Item("Item_DateOut") Or dtTemp.Rows(intCnt).Item("Item_DateArrival") <> dtCompare.Rows(intCnt).Item("Item_DateArrival") _
                Or dtTemp.Rows(intCnt).Item("MachineNum") <> dtCompare.Rows(intCnt).Item("MachineNum") Or dtTemp.Rows(intCnt).Item("Kanban").ToString <> dtCompare.Rows(intCnt).Item("Kanban").ToString Then

                    'Get the SalesOrder ITEM details for this row, but only if the 'strCurrentSalesOrderNum' control variable has changed
                    If strCurrentSalesOrderNum <> dtTemp.Rows(intCnt).Item("SalesOrderNum") Then

                        'SmcN    If we are in here then some date value have t be save. However we have to save these value to the actual sales order details in the Matrix
                        ' So we nned to load these details and loop through the items in the sales order iteself to store the appropriate dates.

                        strCurrentSalesOrderNum = dtTemp.Rows(intCnt).Item("SalesOrderNum")

                        'Get the Sales Order Items from the Matrix (for the appropriate Type of items)
                        If UCase(dtTemp.Rows(intCnt).Item("ItemType")) = "PRODUCT" Then
                            'this is a product item
                            dtOrderDetails = objSales.GetSalesOrderItemsFromMatrix("ORDERITEMS", CInt(strCurrentSalesOrderNum), strConn, Me.CurrentSession.VT_tlsNumFields)
                        Else
                            'this is a service item
                            dtOrderDetails = objSales.GetSalesOrderItemsFromMatrix("SERVICEITEMS", CInt(strCurrentSalesOrderNum), strConn, Me.CurrentSession.VT_tlsNumFields)
                        End If
                        If dtOrderDetails.Columns.Contains("MachineNum") = False Then
                            dtOrderDetails.Columns.Add("MachineNum")
                        End If
                        If dtOrderDetails.Columns.Contains("ViewHistory") = False Then
                            dtOrderDetails.Columns.Add("ViewHistory")
                        End If
                        If dtOrderDetails.Rows.Count > 0 Then
                            If IsDBNull(dtOrderDetails.Rows(0).Item("MachineNum")) Then
                                dtOrderDetails.Rows(0).Item("MachineNum") = ""
                            End If
                        End If
                        If dtOrderDetails.Columns.Contains("Kanban") = False Then
                            dtOrderDetails.Columns.Add("Kanban")
                        End If
                        If dtOrderDetails.Rows.Count > 0 Then
                            If IsDBNull(dtOrderDetails.Rows(0).Item("Kanban")) Then
                                dtOrderDetails.Rows(0).Item("Kanban") = ""
                            End If
                        End If
                        If dtOrderDetails.Rows.Count > 0 Then
                            'Select the relevant order item rows from the main dtTemp datatable
                            strSearch = "SalesOrderNum = '" & strCurrentSalesOrderNum & "'"
                            dtTempOrderRows = objG.SearchDataTable(strSearch, dtTemp)

                            'Now filter out either Product or Service items from this grid 
                            If UCase(dtTemp.Rows(intCnt).Item("ItemType")) = "PRODUCT" Then
                                strSearch = "ItemType = 'Product'"
                                dtTempOrderRows = objG.SearchDataTable(strSearch, dtTempOrderRows)
                            Else
                                strSearch = "ItemType = 'Service'"
                                dtTempOrderRows = objG.SearchDataTable(strSearch, dtTempOrderRows)
                            End If
                            If dtTempOrderRows.Columns.Contains("MachineNum") = False Then
                                dtTempOrderRows.Columns.Add("MachineNum")
                            End If
                            If dtTempOrderRows.Columns.Contains("Kanban") = False Then
                                dtTempOrderRows.Columns.Add("Kanban")
                            End If

                            If dtTempOrderRows.Rows.Count > 0 Then
                                'sort the datatables to ensure they will be compared correctly
                                dtOrderDetails.DefaultView.Sort = "SalesOrderNum, VT_UniqueIndex" & " ASC"
                                dtTempOrderRows.DefaultView.Sort = "SalesOrderNum, VT_UniqueIndex" & " ASC"

                                'loop through the 'dtOrderDetails' table and update data fields as required
                                For intLoopCnt = 0 To dtOrderDetails.Rows.Count - 1

                                    If IsDBNull(dtTempOrderRows.Rows(intLoopCnt).Item("Item_DateOut")) Then
                                        dtTempOrderRows.Rows(intLoopCnt).Item("Item_DateOut") = Date.MinValue
                                    End If
                                    If IsDBNull(dtTempOrderRows.Rows(intLoopCnt).Item("Item_DateArrival")) Then
                                        dtTempOrderRows.Rows(intLoopCnt).Item("Item_DateArrival") = Date.MinValue
                                    End If
                                    If IsDBNull(dtTempOrderRows.Rows(intLoopCnt).Item("MachineNum")) Then
                                        dtTempOrderRows.Rows(intLoopCnt).Item("MachineNum") = ""
                                    End If
                                    If IsDBNull(dtTempOrderRows.Rows(intLoopCnt).Item("Kanban")) Then
                                        dtTempOrderRows.Rows(intLoopCnt).Item("Kanban") = ""
                                    End If
                                    If dtTempOrderRows.Rows(intLoopCnt).Item("Item_DateOut") = Date.MinValue Then
                                        dtOrderDetails.Rows(intLoopCnt).Item("Item_DateOut") = DBNull.Value
                                        dtTempOrderRows.Rows(intLoopCnt).Item("Item_DateOut") = DBNull.Value
                                    Else
                                        dtOrderDetails.Rows(intLoopCnt).Item("Item_DateOut") = dtTempOrderRows.Rows(intLoopCnt).Item("Item_DateOut")
                                    End If

                                    If dtTempOrderRows.Rows(intLoopCnt).Item("Item_DateArrival") = Date.MinValue Then
                                        dtOrderDetails.Rows(intLoopCnt).Item("Item_DateArrival") = DBNull.Value
                                        dtTempOrderRows.Rows(intLoopCnt).Item("Item_DateArrival") = DBNull.Value
                                    Else
                                        dtOrderDetails.Rows(intLoopCnt).Item("Item_DateArrival") = dtTempOrderRows.Rows(intLoopCnt).Item("Item_DateArrival")
                                    End If

                                    If IsDBNull(dtTempOrderRows.Rows(intLoopCnt).Item("MachineNum")) = False Then

                                        dtOrderDetails.Rows(intLoopCnt).Item("MachineNum") = dtTempOrderRows.Rows(intLoopCnt).Item("MachineNum")
                                    End If
                                    If IsDBNull(dtTempOrderRows.Rows(intLoopCnt).Item("Kanban")) = False Then

                                        dtOrderDetails.Rows(intLoopCnt).Item("Kanban") = dtTempOrderRows.Rows(intLoopCnt).Item("Kanban")
                                    End If

                                Next

                                'Now write out the updated 'dtOrderDetails' data
                                If UCase(dtTemp.Rows(intCnt).Item("ItemType")) = "PRODUCT" Then
                                    'This is a product item

                                    objDataPreserve.BindDataToWDG(dtOrderDetails, wdgOrderItems)

                                    'Save Order Items to the MATRIX table
                                    objSales.SaveSalesItemsToMatrix("OrderItems", wdgOrderItems, CInt(strCurrentSalesOrderNum), strConn, Me.CurrentSession.VT_tlsNumFields)
                                Else
                                    'This is a service item
                                    objDataPreserve.BindDataToWDG(dtOrderDetails, wdgServiceItems)

                                    'Save Order Items to the MATRIX table
                                    objSales.SaveSalesItemsToMatrix("ServiceItems", wdgServiceItems, CInt(strCurrentSalesOrderNum), strConn, Me.CurrentSession.VT_tlsNumFields)
                                End If

                            Else
                                'Possible error because no Order details found for this sales order number in the dtTemp datatable
                            End If

                        Else
                            ' Possible Error because no order details found for this Sales Order Number
                        End If

                    End If

                    'SmcN 02/05/2014 Set columns back to null if required
                    'If these values are left as 'Date.MinValue' they display as 01/01/0001 in the grid and then the user has to scroll the date selector all the way back to a date in this century
                    ' If they set to 'DBnull' then they remain blank and the deate selector defaults to Today when it is popped up.

                    If dtTemp.Rows(intCnt).Item("Item_DateOut") = Date.MinValue Then
                        dtTemp.Rows(intCnt).Item("Item_DateOut") = DBNull.Value
                    End If
                    If dtTemp.Rows(intCnt).Item("Item_DateArrival") = Date.MinValue Then
                        dtTemp.Rows(intCnt).Item("Item_DateArrival") = DBNull.Value
                    End If

                    If dtCompare.Rows(intCnt).Item("Item_DateOut") = Date.MinValue Then
                        dtCompare.Rows(intCnt).Item("Item_DateOut") = DBNull.Value
                    End If
                    If dtCompare.Rows(intCnt).Item("Item_DateArrival") = Date.MinValue Then
                        dtCompare.Rows(intCnt).Item("Item_DateArrival") = DBNull.Value
                    End If



                Else
                    'SmcN 02/05/2014 Set columns back to null if required
                    'If these values are left as 'Date.MinValue' they display as 01/01/0001 in the grid and then the user has to scroll the date selector all the way back to a date in this century
                    ' If they set to 'DBnull' then they remain blank and the deate selector defaults to Today when it is popped up.

                    If dtTemp.Rows(intCnt).Item("Item_DateOut") = Date.MinValue Then
                        dtTemp.Rows(intCnt).Item("Item_DateOut") = DBNull.Value
                    End If
                    If dtTemp.Rows(intCnt).Item("Item_DateArrival") = Date.MinValue Then
                        dtTemp.Rows(intCnt).Item("Item_DateArrival") = DBNull.Value
                    End If

                    If dtCompare.Rows(intCnt).Item("Item_DateOut") = Date.MinValue Then
                        dtCompare.Rows(intCnt).Item("Item_DateOut") = DBNull.Value
                    End If
                    If dtCompare.Rows(intCnt).Item("Item_DateArrival") = Date.MinValue Then
                        dtCompare.Rows(intCnt).Item("Item_DateArrival") = DBNull.Value
                    End If

                End If


            Next

            SavePlanningDates = True
        Else
            SavePlanningDates = False

        End If


    End Function

    Protected Sub btnSendCIMLoad_Click(sender As Object, e As EventArgs) Handles btnSendCIMLoad.Click

        'SmcN  19/02/2014 This function will move process appropriate sales orders through the CIM load process step
        ' Order will only progress if;
        '   1. Both 'Item_DateOut' and 'Item_DateArrival' have a value
        '   2. Item_DateArrival and Item_DateOut have a value that is greater than or equal to TODAY

        'First save the grid to ensure any dates entered were saved
        Dim objDataPreserve As New BPADotNetCommonFunctions.VT_DataPreserve.DataPreserve
        Dim objC As New VT_CommonFunctions.CommonFunctions
        Dim dtOutput As New DataTable

        dtOutput = objC.SerialiseFullWebDataGrid(wdgRepGrid)
        objDataPreserve.BindDataToWDG(dtOutput, wdgRepGrid)

        SavePlanningDates()

        'No move forward with CIM load
        Dim objDBF As New MatrixFunctionsForSales.MatrixFunctions_Sales
        Dim objG As New BPADotNetCommonFunctions.VT_ReportFunctions.GeneralFunctions

        Dim dtTemp As New DataTable

        Dim strCurrentSalesOrderNum As String = ""
        Dim strCurrentSONum As String = ""

        Dim strAlertComment As String = ""

        Dim dteItem_dateOut As Date
        Dim dteItem_dateArrival As Date

        Dim strDisplaymessage As String = ""
        Dim intNumProcessed As Integer = 0
        Dim intCnt As Integer
        Dim blnCIMLoad As Boolean = True
        Dim blnLatePlannedOrders As Boolean = False
        Dim strLatePlannedOrders As String

        'Get the data source of the grid
        dtTemp = objDataPreserve.GetWDGDataFromSession(wdgRepGrid)

        'Exit this function if there are no rows
        If dtTemp.Rows.Count = 0 Then
            Exit Sub
        End If


        'sort the datatable
        dtTemp.DefaultView.Sort = "SalesOrderNum, SalesItemNum" & " ASC"

        Dim strNoPlanningRules As String = UCase(objC.GetConfigItem("NoPlanningRules"))
        Dim dtDateAdded As Date
        Dim blnIsKanban As Boolean

        'Loop through the orders and process
        For intCnt = 0 To dtTemp.Rows.Count - 1

            'Set the checkdates
            dteItem_dateOut = VT_SetToDateFormat(dtTemp, intCnt, "Item_DateOut")
            dteItem_dateArrival = VT_SetToDateFormat(dtTemp, intCnt, "Item_DateArrival")
            dtDateAdded = VT_SetToDateFormat(dtTemp, intCnt, "Item_DateRequested")
            If dtTemp.Rows(intCnt).Item("Kanban") = True Then
                blnIsKanban = True
            Else
                blnIsKanban = False
            End If
            'initialise the 'strCurrentSalesOrderNum' for the first pass
            If intCnt = 0 Then
                strCurrentSalesOrderNum = dtTemp.Rows(intCnt).Item("SalesOrderNum")
                strCurrentSONum = dtTemp.Rows(intCnt).Item("SO_ContiguousNum")
            End If

            'For each order we need to check that every line item has the 'Item_DateOut' and 'Item_DateArrival' set
            If strCurrentSalesOrderNum <> dtTemp.Rows(intCnt).Item("SalesOrderNum") Then
                'this is new sales order so process any previous Sales order here

                If intCnt > 0 Then
                    ' only do this if we are not on the very first row
                    If blnCIMLoad = True Then

                        ' if all rows on this Sales Order passed the checks then process this order through CIM load and move to the next status
                        Dim blnCSVWrite As Boolean = False
                        If Session("Accounts") = "MFGPRO" Then
                            blnCSVWrite = WriteCSVfileOutput(strCurrentSalesOrderNum, dtTemp)
                        Else
                            blnCSVWrite = True
                        End If

                        'if the CSV file was created sucessfully then update the order Status
                        If blnCSVWrite = True Then
                            MoveOrderthroughCIMLoad(strCurrentSalesOrderNum, strAlertComment)
                            '
                        End If


                    Else
                        'This order will not be processed through to CIM load because some dates failed the checks so log it's number here for alert comment later
                        strAlertComment = strAlertComment & strCurrentSONum & ","
                    End If

                    'Set the control items for the next Sales Order check
                    strCurrentSalesOrderNum = dtTemp.Rows(intCnt).Item("SalesOrderNum")
                    strCurrentSONum = dtTemp.Rows(intCnt).Item("SO_ContiguousNum")
                    blnCIMLoad = True
                End If

            End If

            If Session("Accounts") = "MFGPRO" Then
                'Check the planning dates 
                If DateDiff(DateInterval.Day, dteItem_dateOut, dteItem_dateArrival) >= 0 Then
                    If strNoPlanningRules <> "YES" Then
                        If dteItem_dateOut > PortalFunctions.Now And dteItem_dateArrival > PortalFunctions.Now Then
                            'They passed so do nothing 
                            'add an input box here for a comment if the order is more than 20 working days late

                            If Weekdays(dtDateAdded, dteItem_dateOut) >= 20 And blnIsKanban = False Then
                                blnLatePlannedOrders = True
                                'we only want to add each order once, not once for every item on the order
                                If InStr(strLatePlannedOrders, strCurrentSONum & "," & strCurrentSalesOrderNum & ";") = 0 Then
                                    strLatePlannedOrders = strLatePlannedOrders & strCurrentSONum & "," & strCurrentSalesOrderNum & ";"

                                End If

                            End If

                        Else
                            'They failed so set the control value to false
                            blnCIMLoad = False
                        End If
                    Else 'need to make sure that the dates are not blank
                        If dteItem_dateArrival = #12:00:00 AM# Or dteItem_dateOut = #12:00:00 AM# Then
                            blnCIMLoad = False
                        End If
                    End If

                Else
                    'They failed so set the control value to false
                    blnCIMLoad = False

                End If
            Else
                If dteItem_dateArrival = #12:00:00 AM# Or dteItem_dateOut = #12:00:00 AM# Then
                    blnCIMLoad = False
                End If
            End If


        Next

        'We need to check the last order in the list here (outside the loop)
        If blnCIMLoad = True Then
            ' if all rows on this Sales Order passed the checks then process this order through CIM load and move to the next status
            Dim blnCSVWrite As Boolean = False
            If Session("Accounts") = "MFGPRO" Then
                blnCSVWrite = WriteCSVfileOutput(strCurrentSalesOrderNum, dtTemp)
            Else
                blnCSVWrite = True
            End If

            'if the CSV file was created sucessfully then update the order Status
            If blnCSVWrite = True Then
                MoveOrderthroughCIMLoad(strCurrentSalesOrderNum, strAlertComment)
            End If
        Else
            'This order will not be processed through to CIM load because so dates failed the checks so log it's number here for alert comment later
            strAlertComment = strAlertComment & strCurrentSONum & ","
        End If

        'If there is an alert comment then display it here
        If Session("Accounts") = "MFGPRO" Then

            If blnLatePlannedOrders = True Then
                Session("LatePlannedOrders") = strLatePlannedOrders
                Response.Redirect("LatePlannedOrders.aspx")

                'Dim strtemp As String = InputBox("Please add the reason for the late planning of order number: " & strCurrentSONum, "Late planning comment", "")
                'save this comment as CimComment under the details node for the order in the matrix

            Else
                If strCurrentSalesOrderNum <> "" And strAlertComment <> "" Then
                    lblMsgLocal.Text = "The following Sales Orders have not been processed because some dates were empty or incorrectly set <br/>" & strAlertComment
                    ModalPopupExtenderMsgLocal.Show()
                End If
            End If
        End If


        'Reload the data for the grids
        FillData()

        'Also update the 'Other Status' grid which lists the items that are open but have already been planned
        PlanningGridUpdatePanel.Update()
        ItemsPlannedUpdatePanel.Update()


    End Sub

    Public Shared Function Weekdays(ByRef startDate As Date, ByRef endDate As Date) As Integer
        Dim numWeekdays As Integer
        Dim totalDays As Integer
        Dim WeekendDays As Integer
        numWeekdays = 0
        WeekendDays = 0

        totalDays = DateDiff(DateInterval.Day, startDate, endDate) + 1

        For i As Integer = 1 To totalDays

            If DatePart(dateinterval.weekday, startDate) = 1 Then
                WeekendDays = WeekendDays + 1
            End If
            If DatePart(dateinterval.weekday, startDate) = 7 Then
                WeekendDays = WeekendDays + 1
            End If
            startDate = DateAdd("d", 1, startDate)
        Next

        numWeekdays = totalDays - WeekendDays

        Return numWeekdays
    End Function

    Function MoveOrderthroughCIMLoad(ByRef strCurrentSalesOrderNum As String, ByRef strAlertComment As String) As Boolean

        'SmcN 24/02/2014 This function updates the status of the Order and writes to the Audit Log

        Dim blnCIMLoad As Boolean = True
        Dim objForms As New VT_Forms.Forms
        Dim objTasks As New VT_eQOInterface.eQOInterface
        Dim objTele As New VTDBFunctions.VTDBFunctions.TelesalesFunctions
        Dim dtTemp As New DataTable
        Dim objSales As New BPADotNetCommonFunctions.SalesFunctions.Sales_Orders

        Dim strConn As String = Session("_VT_DotNetConnString")


        'Set status to Submitted for Product Approval
        Dim strTemp As String = GetGlobalResourceObject("Resource", "Order_SendAcknowledge")
        objForms.SetJobStatusText(strTemp, CLng(strCurrentSalesOrderNum))
        objTasks.SetLastActivity(CLng(strCurrentSalesOrderNum), PortalFunctions.Now.Date)

        dtTemp = objTele.GetOrderForNum(CLng(strCurrentSalesOrderNum))
        Dim lngSalesOrderID As Long = dtTemp.Rows(0).Item("SalesOrderId")
        objTele.UpdateOrderStatus(lngSalesOrderID, strTemp)

        Dim ds As New DataSet
        Dim strSalesOrderNum As String
        ds = objTele.GetOrderForId(lngSalesOrderID)
        If ds.Tables(0).Rows.Count > 0 Then
            strSalesOrderNum = ds.Tables(0).Rows(0).Item("SalesOrderNum")
            objSales.SaveGeneralItemsToSalesMatrix("DateSentForCIM", "Details", PortalFunctions.Now.ToString("s"), CInt(strSalesOrderNum), strConn)

        End If


        'audit log
        Dim strType As String
        strType = "Sales Order"
        objForms.WriteToAuditLog(CLng(strCurrentSalesOrderNum), strType, PortalFunctions.Now, Session("_VT_CurrentUserName") & "[On Planning Page]:", 0, "Order Status changed to " & strTemp & ": All required dates entered by planner", "Audit Record", "SysAdmin")

        MoveOrderthroughCIMLoad = True

    End Function

    Function WriteCSVfileOutput(ByRef strCurrentSalesOrderNum As String, ByRef dtTemp As DataTable) As Boolean

        'SmcN 27/02/2014 This function writes a physcial CSV file output which will be imported into MfgPro

        Dim objG As New BPADotNetCommonFunctions.VT_ReportFunctions.GeneralFunctions
        Dim objSales As New BPADotNetCommonFunctions.SalesFunctions.Sales_Orders

        Dim strConn As String = Session("_VT_DotNetConnString")

        Dim dtSwap As New DataTable

        Dim dtOrderItems As New DataTable
        Dim dtCustomerDetails As New DataTable
        Dim intCnt As Integer = 1
        Dim intLoop As Integer
        Dim strTemp As String
        'set the default value
        WriteCSVfileOutput = False

        ' read the CSV drop folder from wfo_Config
        Dim objBPA As New BPADotNetCommonFunctions.VT_CommonFunctions.CommonFunctions
        Dim strCSVPath As String = objBPA.GetConfigItem("SalesPortalCSVFolder")

        If strCSVPath = "" Then
            'No path found so display error message and exit sub
            WriteCSVfileOutput = False
            Master.ShowMessagePanel("No PATH found for the CSV file output in the Config table. The sales Order [" & strCurrentSalesOrderNum & "] has not been processed")
            Exit Function
        End If


        ' select the Order to send CSV file detials on 
        dtOrderItems = objG.SearchDataTable("SalesOrderNum = '" & Trim(strCurrentSalesOrderNum) & "'", dtTemp)

        dtCustomerDetails = objSales.GetSalesOrderItemsFromMatrix("CUSTOMERITEMS", CInt(strCurrentSalesOrderNum), strConn, Me.CurrentSession.VT_tlsNumFields)

        'SmcN 28/05/2014 Need to strip out any comma's from data elements in the output stream
        If dtCustomerDetails.Rows.Count > 0 Then
            For intLoop = 0 To dtCustomerDetails.Columns.Count - 1
                If Not IsDBNull(dtCustomerDetails.Rows(0).Item(intLoop)) Then
                    strTemp = dtCustomerDetails.Rows(0).Item(intLoop)
                    If strTemp <> "" Then
                        dtCustomerDetails.Rows(0).Item(intLoop) = Replace(strTemp, ",", "")
                    End If
                Else
                    'if it is a dbNull then make it a null string
                    dtCustomerDetails.Rows(0).Item(intLoop) = ""
                End If
            Next
        End If

        'if the item is Lab then the site should be defaulted to 008
        For i = 0 To dtOrderItems.Rows.Count - 1
            If UCase(Trim(dtOrderItems.Rows(i).Item("ProductCode"))) = "LAB" Then
                dtOrderItems.Rows(i).Item("Site") = "008"
                dtCustomerDetails.Rows(0).Item("Other_Site") = "008"
            End If
        Next
        If dtOrderItems.Rows.Count > 0 AndAlso dtCustomerDetails.Rows.Count > 0 Then
            'need to change the data type on the order item number to allow sorting

            dtSwap = dtOrderItems.Clone

            dtSwap.Columns("SalesItemNum").DataType = GetType(System.Int32)

            ''For intCnt = 0 To dtOrderItems.Rows.Count-1
            ''    If dtOrderItems.Rows(intCnt).Item("SalesItemNum") = "" Then
            ''        dtOrderItems.Rows(intCnt).Item("SalesItemNum") = CStr(dtOrderItems.Rows.Count + 1)

            ''    End If
            ''Next

            For intCnt = 0 To dtOrderItems.Rows.Count - 1

                dtSwap.ImportRow(dtOrderItems.Rows(intCnt))
            Next

            dtOrderItems.Clear()
            dtOrderItems = dtSwap

            dtOrderItems.DefaultView.Sort = "SalesItemNum ASC"
            Dim dtOrdersToSendToCSV As Data.DataTable = dtOrderItems.DefaultView.ToTable

            'Reindex the 'SalesItemNum' column of this Sales Order. Both Products and Service items now need to be processed as 1 grid
            For intCnt = 0 To dtOrdersToSendToCSV.Rows.Count - 1

                dtOrdersToSendToCSV.Rows(intCnt).Item("SalesItemNum") = intCnt + 1
            Next

            Dim strStringOut As String = ""
            Dim dteTemp As Date
            intCnt = 1

            ' append the file name
            Dim strFileName As String = "SO-ORD-" & Trim(dtOrdersToSendToCSV.Rows(0).Item("SO_ContiguousNum")) & ".csv"
            strCSVPath = System.IO.Path.Combine(strCSVPath, strFileName)
            Dim origstring As String

            For Each drOut As Data.DataRow In dtOrdersToSendToCSV.Rows

                'SmcN 28/05/2014 Need to strip out any comma's from data elements in the output stream
                If drOut.ItemArray.Length > 0 Then
                    For intLoop = 0 To drOut.ItemArray.Length - 1
                        If Not IsDBNull(drOut.Item(intLoop)) Then
                            strTemp = drOut.Item(intLoop)
                            If strTemp <> "" Then
                                drOut.Item(intLoop) = Replace(strTemp, ",", "")
                            End If
                        Else
                            'if it is a dbNull then make it a null string
                            drOut.Item(intLoop) = ""
                        End If
                    Next
                End If




                'A - Order Num
                strStringOut += Trim(drOut.Item("SO_ContiguousNum")) + ","
                'B - SoldTo Code
                strStringOut += Trim(dtCustomerDetails.Rows(0).Item("SoldTo_Code")) + ","
                'C - BillTo Code
                strStringOut += Trim(dtCustomerDetails.Rows(0).Item("BillTo_Code")) + ","
                'D - ShipTo Code
                strStringOut += Trim(dtCustomerDetails.Rows(0).Item("DeliverTo_Code")) + ","
                'E - Order Date
                dteTemp = dtCustomerDetails.Rows(0).Item("DeliverTo_OrderDate")
                strStringOut += Format(dteTemp, "d") + ","
                'F - PO Num
                strStringOut += Trim(dtCustomerDetails.Rows(0).Item("Other_CustPO")) + ","
                'G - Contact
                strStringOut += Trim(dtCustomerDetails.Rows(0).Item("SoldTo_ContactName")) + ","
                'H - Site
                strStringOut += Trim(dtCustomerDetails.Rows(0).Item("Other_Site")) + ","
                'I - SO Comment Flag
                strStringOut += "Yes" + ","
                'J - IntraStat
                strStringOut += Trim(dtCustomerDetails.Rows(0).Item("Other_Interstat")) + ","
                'K - Delivery Terms (Default to ExWorks if no terms specified)
                If dtCustomerDetails.Rows(0).Item("Other_DeliveryTerms") <> "" Then
                    strStringOut += Trim(dtCustomerDetails.Rows(0).Item("Other_DeliveryTerms")) + ","
                Else
                    strStringOut += "ExWorks" + ","
                End If

                'L - Actual SO Comment 

                origstring = Trim(dtCustomerDetails.Rows(0).Item("Other_Comment"))
                origstring = origstring.Replace(vbCr, "").Replace(vbLf, "")

                strStringOut += origstring + ","
                'M - Item Number
                strStringOut += Trim(drOut.Item("SalesItemNum")) + ","
                'N - PartNum
                strStringOut += Trim(drOut.Item("ProductCode")) + ","
                'O - Site
                strStringOut += Trim(dtCustomerDetails.Rows(0).Item("Other_Site")) + ","
                'P - Order Qty
                strStringOut += Trim(drOut.Item("QuantityRequested").ToString) + ","
                'Q - Unit Price
                strStringOut += Trim(drOut.Item("PO_UnitPrice")) + ","
                'R - Date Arrival
                dteTemp = drOut.Item("Item_DateArrival")
                strStringOut += Format(dteTemp, "d") + ","
                'S - Date Out
                dteTemp = drOut.Item("Item_DateOut")
                strStringOut += Format(dteTemp, "d") + ","
                'T - Item Comment Flag
                strStringOut += "Yes" + ","
                'U - Item Comment

                'we need to remove any carriage returns from the string or it will not write correctly to the csv file
                origstring = Trim(drOut.Item("Comment"))
                origstring = origstring.Replace(vbCr, "").Replace(vbLf, "")


                strStringOut += Trim(origstring) + ","
                'V - Delivery Terms (Default to ExWorks if no terms specified)
                If dtCustomerDetails.Rows(0).Item("Other_DeliveryTerms") <> "" Then
                    strStringOut += Trim(dtCustomerDetails.Rows(0).Item("Other_DeliveryTerms")) + ","
                Else
                    strStringOut += "ExWorks" + ","
                End If
                strStringOut += vbCrLf

                intCnt = intCnt + 1

            Next
            Dim objU As New BPADotNetCommonFunctions.UtilFunctions



            strTemp = objU.WriteTextToFile(strStringOut, strCSVPath)

            If Left(strTemp, 6) = "Error:" Then
                Master.ShowMessagePanel(String.Format("There was a problem writing the CSV file to {0}. {1})", strCSVPath, strTemp))
                Exit Function
            End If


            WriteCSVfileOutput = True

            ' Append the file to the sales Order

        Else
            'There are no order items for this order so error and abort  
            WriteCSVfileOutput = False
            Master.ShowMessagePanel("No Order items found !!   The sales Order [" & strCurrentSalesOrderNum & "] has not been processed")
            Exit Function
        End If



    End Function


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


    Sub InitialiseAndBindEmptyPlanningGrid()

        Dim dtItemsForGrid As New DataTable
        Dim objDataPreserve As New BPADotNetCommonFunctions.VT_DataPreserve.DataPreserve

        'build a blank datatable and bind the grids to these - incase there is no data
        dtItemsForGrid.Columns.Add("SalesOrderNum")
        dtItemsForGrid.Columns.Add("Cust_PO")
        dtItemsForGrid.Columns.Add("DeliverTo_Name")
        dtItemsForGrid.Columns.Add("Other_Site")
        dtItemsForGrid.Columns.Add("ProductCode")
        dtItemsForGrid.Columns.Add("OrderDate")
        dtItemsForGrid.Columns.Add("DeliverTo_RequestedDate")
        dtItemsForGrid.Columns.Add("DueDate")
        dtItemsForGrid.Columns.Add("QuantityRequested")
        dtItemsForGrid.Columns.Add("InvoiceTo")
        dtItemsForGrid.Columns.Add("SoldTo")
        dtItemsForGrid.Columns.Add("DateOut")
        dtItemsForGrid.Columns.Add("BillTo_Name")
        dtItemsForGrid.Columns.Add("Other_CustPO")
        dtItemsForGrid.Columns.Add("DeliverTo_DateOut")
        dtItemsForGrid.Columns.Add("SoldTo_Name")
        dtItemsForGrid.Columns.Add("DateCreated")
        dtItemsForGrid.Columns.Add("Item_DateOut")


        ' call the AddAddtional columns function
        'AddAdditionalProductColumns(dtItemsForGrid)

        'Bind the Grid
        objDataPreserve.BindDataToWDG(dtItemsForGrid, wdgRepGrid)


    End Sub

    Sub InitialiseAndBindEmptyOpenOrderGrid()

        Dim dtItemsForGrid As New DataTable

        Dim objDataPreserve As New BPADotNetCommonFunctions.VT_DataPreserve.DataPreserve

        ' build a blank datatable and bind the grids to these

        dtItemsForGrid.Columns.Add("SalesOrderNum")
        dtItemsForGrid.Columns.Add("Cust_PO")
        dtItemsForGrid.Columns.Add("DeliverTo_Name")
        dtItemsForGrid.Columns.Add("Site")
        dtItemsForGrid.Columns.Add("ProductCode")
        dtItemsForGrid.Columns.Add("OrderDate")
        dtItemsForGrid.Columns.Add("DeliverTo_RequestedDate")
        dtItemsForGrid.Columns.Add("DueDate")
        dtItemsForGrid.Columns.Add("QuantityRequested")
        dtItemsForGrid.Columns.Add("InvoiceTo")
        dtItemsForGrid.Columns.Add("SoldTo")
        dtItemsForGrid.Columns.Add("Status")
        dtItemsForGrid.Columns.Add("BillTo_Name")
        dtItemsForGrid.Columns.Add("Other_CustPO")
        dtItemsForGrid.Columns.Add("DeliverTo_DateOut")
        dtItemsForGrid.Columns.Add("SoldTo_Name")
        dtItemsForGrid.Columns.Add("DateCreated")
        dtItemsForGrid.Columns.Add("Item_DateOut")

        ' call the AddAddtional columns function
        'AddAdditionalProductColumns(dtItemsForGrid)

        'Bind the Grid
        objDataPreserve.BindDataToWDG(dtItemsForGrid, wdgOpenOtherStatus)


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

    Sub InitialiseAndBindEmptyServiceItemsGrid()

        Dim dtItemsForGrid As New DataTable
        Dim dtServiceItems As New DataTable
        Dim objDataPreserve As New BPADotNetCommonFunctions.VT_DataPreserve.DataPreserve

        'There is no sales order yet so build a blank datatable and bind the grids to these

        dtServiceItems.Columns.Add("ProductCode")
        dtServiceItems.Columns.Add("ProductId")
        dtServiceItems.Columns.Add("ProductName")

        dtServiceItems.Columns.Add("Quantity")
        dtServiceItems.Columns.Add("Weight")
        dtServiceItems.Columns.Add("UnitPrice")
        dtServiceItems.Columns.Add("TotalExclVat")
        dtServiceItems.Columns.Add("VAT")
        dtServiceItems.Columns.Add("TotalPrice")
        dtServiceItems.Columns.Add("Item_DateRequested")
        dtServiceItems.Columns.Add("Item_DateOut")
        dtServiceItems.Columns.Add("Item_DateArrival")

        dtServiceItems.Columns.Add("Comment")
        dtServiceItems.Columns.Add("OrderServiceItemId")
        dtServiceItems.Columns.Add("VATRate")

        dtServiceItems.Columns.Add("TraceCodeDesc")
        dtServiceItems.Columns.Add("LocationId")
        dtServiceItems.Columns.Add("AdditionOrder")
        dtServiceItems.Columns.Add("ProductSellBy")
        dtServiceItems.Columns.Add("VT_UniqueIndex")
        dtServiceItems.Columns.Add("UnitOfSale")

        'Bind the Grid
        objDataPreserve.BindDataToWDG(dtServiceItems, wdgServiceItems)

    End Sub


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

    Protected Sub btnExport_Click(sender As Object, e As EventArgs) Handles btnExport.Click
        VerifyExporter.DownloadName = "Sales_Awaiting_Planning_VerifyTechnologies"
        VerifyExporter.Export(wdgRepGrid)
    End Sub

    'Protected Sub cmdInputOK_Click(sender As Object, e As ImageClickEventArgs) Handles cmdInputOK.Click
    '    'audit log
    '    Dim strType As String
    '    Dim strAuditComment As String
    '    Dim objforms As New VT_Forms.Forms
    '    Dim objSales As New BPADotNetCommonFunctions.SalesFunctions.Sales_Orders


    '    strAuditComment = "Late planning Comment: " & strtemp
    '    strType = "Sales Order"
    '    objforms.WriteToAuditLog(CLng(strCurrentSalesOrderNum), strType, Now, Session("_VT_CurrentUserName"), Session("_VT_CurrentUserId"), strAuditComment, "Audit Record", "SysAdmin")

    '    'Save the Planning comment in the matrix

    '    objSales.SaveGeneralItemsToSalesMatrix("LatePlanningComment", "Details", "[" & Session("_VT_CurrentUserName") & "] " & strAuditComment, CInt(strCurrentSalesOrderNum), Session("_VT_DotNetConnString"))
    '    objSales.SaveGeneralItemsToSalesMatrix("LatePlanningComment_Name", "Details", Session("_VT_CurrentUserName"), CInt(strCurrentSalesOrderNum), Session("_VT_DotNetConnString"))
    '    objSales.SaveGeneralItemsToSalesMatrix("LatePlanningComment_ID", "Details", CStr(Session("_VT_CurrentUserId")), CInt(strCurrentSalesOrderNum), Session("_VT_DotNetConnString"))

    'End Sub
End Class



