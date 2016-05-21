Imports BPADotNetCommonFunctions
Imports BPADotNetCommonFunctions.VT_CommonFunctions.MatrixFunctions
Imports System.Data.SqlClient
Imports System.Data
Imports System.Drawing
Imports System.Data.DataTableExtensions
Imports VTDBFunctions.VTDBFunctions


Partial Class TabPages_DispatchDocsDashboard
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

            Dim dtShortOutput As DataTable = objDataPreserve.GetWDGDataFromSession(wdgRepGrid)
            Dim intNumDays As Integer


        End If

        If Not Session("_VT_DispDocsManagerRowSelected") Is Nothing Then
            'Try to locate to the previous selected row if possible. NOTE: The original row may not now be in the table if its status changed from On-hold
            'if there is a selected row saved then show it as selected

            Dim dt As DataTable = Session("_VT_DispDocsManagerRowSelected")
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
        Dim Orderid As Long = Me.CurrentSession.VT_SalesOrderID

        ''If OrderId > 0 Then
        ''    'There is a valid OrderID so read the Order related details and fill the grid
        ''    dsOrderDetails = objTelesales.GetOrderForId(OrderId)
        ''    Me.CurrentSession.VT_SalesOrderNum = dsOrderDetails.Tables(0).Rows(0).Item("SalesOrderNum")


        ''    ' ---------- Get data and bind the Order Items Grid ---------
        ''    dtTemp = objSales.GetSalesOrderItemsFromMatrix("JobStepItems", CInt(Me.CurrentSession.VT_SalesOrderNum), strConn, Me.CurrentSession.VT_tlsNumFields)

        ''    If dtTemp.Rows.Count > 0 Then


        ''        dtItemsForGrid = dtTemp



        ''        objDataPreserve.BindDataToWDG(dtItemsForGrid, wdgRepGrid)

        ''    Else
        'There are no items for this sales order yet so bind an empty grid structure
        InitialiseAndBindEmptyOrderItemsGrid()


        ''    End If
        ''Else

        ''    InitialiseAndBindEmptyOrderItemsGrid()

        ''End If




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
        dtItemsForGrid.Columns.Add("SoldTo_Category")
        dtItemsForGrid.Columns.Add("SoldTo_Address")
        dtItemsForGrid.Columns.Add("SoldTo_Name")
        dtItemsForGrid.Columns.Add("SO_ContiguousNum")
        dtItemsForGrid.Columns.Add("Site")
        dtItemsForGrid.Columns.Add("DispatchDocNum")
        dtItemsForGrid.Columns.Add("WorkItemNum")
        dtItemsForGrid.Columns.Add("ItemDesc")
        dtItemsForGrid.Columns.Add("RoleResp")
        dtItemsForGrid.Columns.Add("Item_OnHoldPersonResponsible")
        dtItemsForGrid.Columns.Add("ViewComments")
        dtItemsForGrid.Columns.Add("UnreadComments")
        dtItemsForGrid.Columns.Add("ReasonOnHold")
        dtItemsForGrid.Columns.Add("DashboardComment")
        dtItemsForGrid.Columns.Add("ESTTime")
        dtItemsForGrid.Columns.Add("TimeUnits")
        dtItemsForGrid.Columns.Add("Item_DateStart")
        dtItemsForGrid.Columns.Add("Item_DateFinish")
        dtItemsForGrid.Columns.Add("Difference")
        dtItemsForGrid.Columns.Add("Status")
        dtItemsForGrid.Columns.Add("preReqItemNum")
        dtItemsForGrid.Columns.Add("preReqItemID")
        dtItemsForGrid.Columns.Add("AlertTime")
        dtItemsForGrid.Columns.Add("AlarmTime")
        dtItemsForGrid.Columns.Add("Item_OnHoldPersonResponsibleID")
        dtItemsForGrid.Columns.Add("SalesOrderId")
        dtItemsForGrid.Columns.Add("SalesOrderItemId")
        dtItemsForGrid.Columns.Add("JobItemId")
        dtItemsForGrid.Columns.Add("SalesORderNum")

        'this is for prototype purposes only. Hardcoding some test data
        Dim Orderid As Long = Me.CurrentSession.VT_SalesOrderID
        Dim dsOrderDetails As New DataSet
        Dim objTelesales As New VTDBFunctions.VTDBFunctions.TelesalesFunctions
        Dim objSales As New BPADotNetCommonFunctions.SalesFunctions.Sales_Orders
        Dim dtTemp As New DataTable
        Dim strConn As String = Session("_VT_DotNetConnString")
        Dim dr As DataRow
        Dim dtOrderItems As New DataTable

        If Orderid > 0 Then
            'There is a valid OrderID so read the Order related details and fill the grid
            dsOrderDetails = objTelesales.GetOrderForId(Orderid)
            Me.CurrentSession.VT_SalesOrderNum = dsOrderDetails.Tables(0).Rows(0).Item("SalesOrderNum")


            ' ---------- Get data and bind the Order Items Grid ---------
            dtTemp = objSales.GetSalesOrderItemsFromMatrix("CustomerItems", CInt(Me.CurrentSession.VT_SalesOrderNum), strConn, Me.CurrentSession.VT_tlsNumFields)
            dtOrderItems = objSales.GetSalesOrderItemsFromMatrix("OrderItems", CInt(Me.CurrentSession.VT_SalesOrderNum), strConn, Me.CurrentSession.VT_tlsNumFields)

            If dtTemp.Rows.Count > 0 Then
                dr = dtItemsForGrid.Rows.Add
                With dtTemp.Rows(0)
                    dr.Item("SoldTo_Category") = .Item("SoldTo_Category")
                    dr.Item("Site") = "APZU4760187"
                    dr.Item("SoldTo_Name") = .Item("SoldTo_Name")

                End With
            End If

            If dtOrderItems.Rows.Count > 0 Then

                With dtOrderItems.Rows(0)
                    dr.Item("SO_ContiguousNum") = .Item("SO_ContiguousNum")

                End With
            End If

            dr.Item("SalesOrderId") = Orderid
            dr.Item("SalesORderNum") = Me.CurrentSession.VT_SalesOrderNum
            dr.Item("DispatchDocNum") = "8066104957"
            dr.Item("ItemDesc") = "SOB"
            dr.Item("WorkItemNum") = "1"

            If dtTemp.Rows.Count > 0 Then
                dr = dtItemsForGrid.Rows.Add
                With dtTemp.Rows(0)
                    dr.Item("SoldTo_Category") = .Item("SoldTo_Category")
                    dr.Item("Site") = "APZU4760187"
                    dr.Item("SoldTo_Name") = .Item("SoldTo_Name")

                End With
            End If

            If dtOrderItems.Rows.Count > 0 Then

                With dtOrderItems.Rows(0)
                    dr.Item("SO_ContiguousNum") = .Item("SO_ContiguousNum")

                End With
            End If

            dr.Item("SalesOrderId") = Orderid
            dr.Item("SalesORderNum") = Me.CurrentSession.VT_SalesOrderNum
            dr.Item("DispatchDocNum") = "8066104957"
            dr.Item("ItemDesc") = "Bill Of Lading"
            dr.Item("WorkItemNum") = "2"



            If dtTemp.Rows.Count > 0 Then
                dr = dtItemsForGrid.Rows.Add
                With dtTemp.Rows(0)
                    dr.Item("SoldTo_Category") = .Item("SoldTo_Category")
                    dr.Item("Site") = "APZU4760187"
                    dr.Item("SoldTo_Name") = .Item("SoldTo_Name")

                End With
            End If

            If dtOrderItems.Rows.Count > 0 Then

                With dtOrderItems.Rows(0)
                    dr.Item("SO_ContiguousNum") = .Item("SO_ContiguousNum")

                End With
            End If

            dr.Item("SalesOrderId") = Orderid
            dr.Item("SalesORderNum") = Me.CurrentSession.VT_SalesOrderNum
            dr.Item("DispatchDocNum") = "8066104957"
            dr.Item("ItemDesc") = "Cert Of Analysis"
            dr.Item("WorkItemNum") = "3"



            If dtTemp.Rows.Count > 0 Then
                dr = dtItemsForGrid.Rows.Add
                With dtTemp.Rows(0)
                    dr.Item("SoldTo_Category") = .Item("SoldTo_Category")
                    dr.Item("Site") = "APZU4760187"
                    dr.Item("SoldTo_Name") = .Item("SoldTo_Name")

                End With
            End If

            If dtOrderItems.Rows.Count > 0 Then

                With dtOrderItems.Rows(0)
                    dr.Item("SO_ContiguousNum") = .Item("SO_ContiguousNum")

                End With
            End If

            dr.Item("SalesOrderId") = Orderid
            dr.Item("SalesORderNum") = Me.CurrentSession.VT_SalesOrderNum
            dr.Item("DispatchDocNum") = "8066104957"
            dr.Item("ItemDesc") = "DOA"
            dr.Item("WorkItemNum") = "4"



            If dtTemp.Rows.Count > 0 Then
                dr = dtItemsForGrid.Rows.Add
                With dtTemp.Rows(0)
                    dr.Item("SoldTo_Category") = .Item("SoldTo_Category")
                    dr.Item("Site") = "APZU4760187"
                    dr.Item("SoldTo_Name") = .Item("SoldTo_Name")

                End With
            End If

            If dtOrderItems.Rows.Count > 0 Then

                With dtOrderItems.Rows(0)
                    dr.Item("SO_ContiguousNum") = .Item("SO_ContiguousNum")

                End With
            End If

            dr.Item("SalesOrderId") = Orderid
            dr.Item("SalesORderNum") = Me.CurrentSession.VT_SalesOrderNum
            dr.Item("DispatchDocNum") = "8066104957"
            dr.Item("ItemDesc") = "Radiation Cert"
            dr.Item("WorkItemNum") = "5"



            If dtTemp.Rows.Count > 0 Then
                dr = dtItemsForGrid.Rows.Add
                With dtTemp.Rows(0)
                    dr.Item("SoldTo_Category") = .Item("SoldTo_Category")
                    dr.Item("Site") = "APZU4760187"
                    dr.Item("SoldTo_Name") = .Item("SoldTo_Name")

                End With
            End If

            If dtOrderItems.Rows.Count > 0 Then

                With dtOrderItems.Rows(0)
                    dr.Item("SO_ContiguousNum") = .Item("SO_ContiguousNum")

                End With
            End If

            dr.Item("SalesOrderId") = Orderid
            dr.Item("SalesORderNum") = Me.CurrentSession.VT_SalesOrderNum
            dr.Item("DispatchDocNum") = "8066104957"
            dr.Item("ItemDesc") = "Certified doc"
            dr.Item("WorkItemNum") = "6"



            If dtTemp.Rows.Count > 0 Then
                dr = dtItemsForGrid.Rows.Add
                With dtTemp.Rows(0)
                    dr.Item("SoldTo_Category") = .Item("SoldTo_Category")
                    dr.Item("Site") = "APZU4760187"
                    dr.Item("SoldTo_Name") = .Item("SoldTo_Name")

                End With
            End If

            If dtOrderItems.Rows.Count > 0 Then

                With dtOrderItems.Rows(0)
                    dr.Item("SO_ContiguousNum") = .Item("SO_ContiguousNum")

                End With
            End If

            dr.Item("SalesOrderId") = Orderid
            dr.Item("SalesORderNum") = Me.CurrentSession.VT_SalesOrderNum
            dr.Item("DispatchDocNum") = "8066104957"
            dr.Item("ItemDesc") = "Legalisation"
            dr.Item("WorkItemNum") = "7"




            If dtTemp.Rows.Count > 0 Then
                dr = dtItemsForGrid.Rows.Add
                With dtTemp.Rows(0)
                    dr.Item("SoldTo_Category") = .Item("SoldTo_Category")
                    dr.Item("Site") = "APZU4760187"
                    dr.Item("SoldTo_Name") = .Item("SoldTo_Name")

                End With
            End If

            If dtOrderItems.Rows.Count > 0 Then

                With dtOrderItems.Rows(0)
                    dr.Item("SO_ContiguousNum") = .Item("SO_ContiguousNum")

                End With
            End If

            dr.Item("SalesOrderId") = Orderid
            dr.Item("SalesORderNum") = Me.CurrentSession.VT_SalesOrderNum
            dr.Item("DispatchDocNum") = "8066104957"
            dr.Item("ItemDesc") = "Dubai Invoice"
            dr.Item("WorkItemNum") = "8"


            If dtTemp.Rows.Count > 0 Then
                dr = dtItemsForGrid.Rows.Add
                With dtTemp.Rows(0)
                    dr.Item("SoldTo_Category") = .Item("SoldTo_Category")
                    dr.Item("Site") = "APZU4760187"
                    dr.Item("SoldTo_Name") = .Item("SoldTo_Name")

                End With
            End If

            If dtOrderItems.Rows.Count > 0 Then

                With dtOrderItems.Rows(0)
                    dr.Item("SO_ContiguousNum") = .Item("SO_ContiguousNum")

                End With
            End If

            dr.Item("SalesOrderId") = Orderid
            dr.Item("SalesORderNum") = Me.CurrentSession.VT_SalesOrderNum
            dr.Item("DispatchDocNum") = "8066104957"
            dr.Item("ItemDesc") = "Pouch Sent To Market"
            dr.Item("WorkItemNum") = "9"
        End If


        'Bind the Grid
        objDataPreserve.BindDataToWDG(dtItemsForGrid, wdgRepGrid)


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



    Protected Sub btnBack1_Click(sender As Object, e As ImageClickEventArgs) Handles btnBack1.Click, btnBack2.Click

        Response.Redirect("~/TabPages/WarehouseManager.aspx")

    End Sub



    Protected Sub wdgRepGrid_ActiveCellChanged(sender As Object, e As Infragistics.Web.UI.GridControls.ActiveCellEventArgs) Handles wdgRepGrid.ActiveCellChanged

        ''''Dim objC As New VT_CommonFunctions.CommonFunctions
        ''''If Not e Is Nothing Then
        ''''    Dim intActiveRowIndex As Integer = e.CurrentActiveCell.Row.Index
        ''''    If Not objC Is Nothing Then
        ''''        Session("_VT_DispDocsManagerRowSelected") = objC.SerialiseWebDataGridRow(wdgRepGrid, intActiveRowIndex)

        ''''        Session("_VT_DispDocsManagerRowSelectedIndex") = e.CurrentActiveCell.Row.Index
        ''''        SetUpOrderSessionVariables()
        ''''    Else
        ''''        lblMsg.Text = "An Error has occurred With the grid On this page. Please Exit the portal And Then reopen it. Apologies For the inconvenience."
        ''''        ModalPopupExtenderMsg.Show()
        ''''    End If
        ''''Else
        ''''    lblMsg.Text = "An Error has occurred With the grid On this page. Please Exit the portal And Then reopen it. Apologies For the inconvenience."
        ''''    ModalPopupExtenderMsg.Show()
        ''''End If

    End Sub

    Public Function PortalRootNode() As String
        ' retrieve the Root node of the current portal
        ' we will use it to create a consistent path to the image files
        Dim strCompletePath As String = Server.MapPath("~")

        'ServerRootPath = strCompletePath
        PortalRootNode = Mid(strCompletePath, InStrRev(strCompletePath, "\") + 1)

    End Function


    Protected Sub cmdSavePersonAssign_Click(sender As Object, e As ImageClickEventArgs) Handles cmdSavePersonAssign.Click
        If Session("_VT_DispDocsManagerRowSelected") IsNot Nothing Then

            Dim astrColNamesAndValues(1, 1) As String
            astrColNamesAndValues(0, 0) = "Item_OnHoldPersonResponsible"
            astrColNamesAndValues(0, 1) = ddlUsers_NS.SelectedItem.Text
            astrColNamesAndValues(1, 0) = "Item_OnHoldPersonResponsibleID"
            astrColNamesAndValues(1, 1) = ddlUsers_NS.SelectedItem.Value

            UpdateGridCellValue(astrColNamesAndValues)


        End If
        CheckOrderBackIn()

    End Sub

    Function UpdateGridCellValue(ByVal astrColNamesAndValues(,) As String) As Boolean
        'SmcN 26/03/2014  This function adds or edits the value in the specified Cell.


        ''''Dim dtTVGrid As New DataTable
        ''''Dim dtSOItems As New DataTable
        ''''Dim objDataPreserve As New BPADotNetCommonFunctions.VT_DataPreserve.DataPreserve
        ''''Dim objDBF As New MatrixFunctionsForSales.MatrixFunctions_Sales
        ''''Dim intSelectedRowIndex As Integer
        ''''Dim intSalesOrderNum As Integer
        ''''Dim intSOItemRow As Integer
        ''''Dim strColName As String
        ''''Dim strColValue As String
        ''''Dim blnOrderItemsExist As Boolean
        ''''Dim objSales As New BPADotNetCommonFunctions.SalesFunctions.Sales_Orders

        ''''Dim strConn As String = Session("_VT_DotNetConnString")

        '''''Read the OnHoldTV grid
        ''''dtTVGrid = objDataPreserve.GetWDGDataFromSession(wdgRepGrid)

        ''''If dtTVGrid.Columns.Contains("Item_OnHoldPersonResponsibleID") = False Then
        ''''    dtTVGrid.Columns.Add("Item_OnHoldPersonResponsibleID")
        ''''End If
        ''''If dtTVGrid.Columns.Contains("MachineNum") = False Then
        ''''    dtTVGrid.Columns.Add("MachineNum")
        ''''End If
        ''''If dtTVGrid.Columns.Contains("Kanban") = False Then
        ''''    dtTVGrid.Columns.Add("Kanban")
        ''''End If
        '''''Set the pointers to the correct row
        ''''intSelectedRowIndex = Session("_VT_DispDocsManagerRowSelected").rows(0).item("VT_ActiveRowIndex")
        ''''intSalesOrderNum = Session("_VT_DispDocsManagerRowSelected").rows(0).item("SalesOrderNum")

        '''''Now we need to get the Sales order items for this sales order number from the matrix
        ''''dtSOItems = objSales.GetSalesOrderItemsFromMatrix("OrderItems", intSalesOrderNum, strConn, Me.CurrentSession.VT_tlsNumFields)

        ''''If dtSOItems.Rows.Count = 0 Then
        ''''    blnOrderItemsExist = False
        ''''Else
        ''''    blnOrderItemsExist = True
        ''''End If


        '''''loop through the columns and update the relevant values
        ''''For intSOItemRow = 0 To dtSOItems.Rows.Count - 1
        ''''    For intCnt = 0 To astrColNamesAndValues.GetLength(0) - 1

        ''''        strColName = astrColNamesAndValues(intCnt, 0)
        ''''        strColValue = astrColNamesAndValues(intCnt, 1)

        ''''        ' dtTVGrid.Rows(intSelectedRowIndex).Item(strColName) = strColValue

        ''''        dtSOItems.Rows(intSOItemRow).Item(strColName) = strColValue
        ''''        If strColName = "Item_OnHoldPersonResponsibleID" Then
        ''''            If ddlAssignToCust.Items(0).Selected = True Then

        ''''                Dim strsql As String = "Update cus_Customers Set CustomerPersonResponsible = " & ddlUsers_NS.SelectedItem.Value & " where CustomerId = " & dtTVGrid.Rows(intSelectedRowIndex).Item("CustomerId")
        ''''                Dim objdb As New VTDBFunctions.VTDBFunctions.DBFunctions
        ''''                objdb.ExecuteSQLQuery(strsql)


        ''''            End If
        ''''        End If
        ''''    Next
        ''''Next

        '''''loop through the columns and update the relevant values
        ''''For intSOItemRow = 0 To dtTVGrid.Rows.Count - 1
        ''''    If dtTVGrid.Rows(intSOItemRow).Item("SalesOrderNum") = intSalesOrderNum Then
        ''''        For intCnt = 0 To astrColNamesAndValues.GetLength(0) - 1

        ''''            strColName = astrColNamesAndValues(intCnt, 0)
        ''''            strColValue = astrColNamesAndValues(intCnt, 1)

        ''''            dtTVGrid.Rows(intSOItemRow).Item(strColName) = strColValue

        ''''        Next
        ''''    End If

        ''''Next

        '''''Rebind the ONHoldTV grid
        ''''objDataPreserve.BindDataToWDG(dtTVGrid, wdgRepGrid)



        ''''Dim intNumDays As Integer


        '''''Save Order Items to the MATRIX table
        ''''objSales.SaveSalesItemsToMatrix("JobStepItems", wdgRepGrid, intSalesOrderNum, strConn, Me.CurrentSession.VT_tlsNumFields)

        UpdateGridCellValue = True

    End Function

    Protected Sub btnExport_Click(sender As Object, e As ImageClickEventArgs) Handles btnExport.Click
        VerifyExporter.DownloadName = "SalesOrders_OnHold_Dashboard_VerifyTechnologies"
        VerifyExporter.Export(wdgRepGrid)
    End Sub

    '----All comments related functions
    Protected Sub cmdSaveComment_Click(sender As Object, e As ImageClickEventArgs) Handles cmdSaveComment.Click
        ''''If Session("_VT_DispDocsManagerRowSelected") IsNot Nothing Then

        ''''    Dim astrColNamesAndValues(0, 1) As String
        ''''    astrColNamesAndValues(0, 0) = "Item_OnHoldTVComment"
        ''''    astrColNamesAndValues(0, 1) = txtHoldTVComment_NS.Text

        ''''    UpdateCommentForAllOrderItems(astrColNamesAndValues)
        ''''End If

        ''''CheckOrderBackIn()

    End Sub

    Protected Sub btnViewComments_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs)

        SetUpOrderSessionVariables()

        'Setup the appropriate return path
        Me.CurrentSession.OptionsPageToReturnTo = "~/TabPages/HoldQueueManager.aspx"

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

        ''''Dim dtTVGrid As New DataTable
        ''''Dim dtSOItems As New DataTable
        ''''Dim objDataPreserve As New BPADotNetCommonFunctions.VT_DataPreserve.DataPreserve
        ''''Dim objDBF As New MatrixFunctionsForSales.MatrixFunctions_Sales
        ''''Dim intSelectedRowIndex As Integer
        ''''Dim intSalesOrderNum As Integer
        ''''Dim intSOItemRow As Integer
        ''''Dim strColName As String
        ''''Dim strColValue As String
        ''''Dim blnOrderItemsExist As Boolean
        ''''Dim objSales As New BPADotNetCommonFunctions.SalesFunctions.Sales_Orders

        ''''Dim strConn As String = Session("_VT_DotNetConnString")

        '''''Read the OnHoldTV grid
        ''''dtTVGrid = objDataPreserve.GetWDGDataFromSession(wdgRepGrid)
        ''''If dtTVGrid.Columns.Contains("MachineNum") = False Then
        ''''    dtTVGrid.Columns.Add("MachineNum")
        ''''End If
        ''''If dtTVGrid.Columns.Contains("Kanban") = False Then
        ''''    dtTVGrid.Columns.Add("Kanban")
        ''''End If
        '''''Set the pointers to the correct row
        ''''intSelectedRowIndex = Session("_VT_DispDocsManagerRowSelected").rows(0).item("VT_ActiveRowIndex")
        ''''intSalesOrderNum = Session("_VT_DispDocsManagerRowSelected").rows(0).item("SalesOrderNum")

        '''''Now we need to get the Sales order items for this sales order number from the matrix
        ''''dtSOItems = objSales.GetSalesOrderItemsFromMatrix("OrderItems", intSalesOrderNum, strConn, Me.CurrentSession.VT_tlsNumFields)
        ''''If dtSOItems.Columns.Contains("MachineNum") = False Then
        ''''    dtSOItems.Columns.Add("MachineNum")
        ''''End If
        ''''If dtSOItems.Columns.Contains("Kanban") = False Then
        ''''    dtSOItems.Columns.Add("Kanban")
        ''''End If
        ''''If dtSOItems.Columns.Contains("ViewHistory") = False Then
        ''''    dtSOItems.Columns.Add("ViewHistory")
        ''''End If
        ''''If dtSOItems.Rows.Count = 0 Then
        ''''    blnOrderItemsExist = False
        ''''Else
        ''''    blnOrderItemsExist = True
        ''''End If


        '''''loop through the columns and update the relevant values
        ''''For intSOItemRow = 0 To dtSOItems.Rows.Count - 1
        ''''    For intCnt = 0 To astrColNamesAndValues.GetLength(0) - 1

        ''''        strColName = astrColNamesAndValues(intCnt, 0)
        ''''        strColValue = astrColNamesAndValues(intCnt, 1)

        ''''        ' dtTVGrid.Rows(intSelectedRowIndex).Item(strColName) = strColValue

        ''''        dtSOItems.Rows(intSOItemRow).Item(strColName) = strColValue

        ''''    Next
        ''''Next

        '''''loop through the columns and update the relevant values
        ''''For intSOItemRow = 0 To dtTVGrid.Rows.Count - 1
        ''''    If dtTVGrid.Rows(intSOItemRow).Item("SalesOrderNum") = intSalesOrderNum Then
        ''''        For intCnt = 0 To astrColNamesAndValues.GetLength(0) - 1

        ''''            strColName = astrColNamesAndValues(intCnt, 0)
        ''''            strColValue = astrColNamesAndValues(intCnt, 1)

        ''''            dtTVGrid.Rows(intSOItemRow).Item(strColName) = strColValue

        ''''        Next
        ''''    End If

        ''''Next

        '''''Rebind the ONHoldTV grid
        ''''objDataPreserve.BindDataToWDG(dtTVGrid, wdgRepGrid)




        '''''Save Order Items to the MATRIX table
        ''''objSales.SaveSalesItemsToMatrix("JobStepItems", wdgRepGrid, intSalesOrderNum, strConn, Me.CurrentSession.VT_tlsNumFields)

        '''''also add this comment to the comments engine
        ''''Dim objApp As New CommentFunctions
        ''''Dim strCommentTarget As String
        ''''Dim intCommentTargetId As Integer

        ''''strCommentTarget = GetGlobalResourceObject("Resource", "Everybody")
        ''''intCommentTargetId = 0


        ''''Dim strCommentSource As String = Session("_VT_CurrentUserName")
        ''''Dim intCommentSourceId As Integer = Session("_VT_CurrentUserId")
        ''''Dim intIdToUse As Integer

        ''''intIdToUse = Me.CurrentSession.VT_NewOrderMatrixID


        ''''objApp.StoreComment(intIdToUse, astrColNamesAndValues(0, 1), "", Nothing, strCommentSource, strCommentTarget, intCommentSourceId, intCommentTargetId)


        UpdateCommentForAllOrderItems = True
    End Function


    '--------End of section containing all comments related functions
    Protected Sub btnViewOrder_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs)

        SetUpOrderSessionVariables()

        ''Setup the appropriate return path
        ''''Session("_VT_SalesOrderReturnPath") = "TabPages/HoldQueueManager.aspx"

        '''''save the currently selected customer
        ''''Dim dtTemp As New DataTable
        ''''dtTemp = Session("_VT_DispDocsManagerRowSelected")

        ''''Dim objCust As New VTDBFunctions.VTDBFunctions.CustomerFunctions

        ''''If dtTemp.Rows.Count > 0 Then
        ''''    With dtTemp.Rows(0)

        ''''        Session("SelectedCustomerID") = objCust.GetCustomerIdForRef(.Item("SoldTo_Code"))
        ''''        Me.CurrentSession.strNewOrderCustomerName = objCust.GetCustomerNameForId(Session("SelectedCustomerId"))
        ''''        Me.CurrentSession.VT_CustomerPO = IIf(IsDBNull(.Item("Other_CustPO")), "", .Item("Other_CustPO"))

        ''''    End With
        ''''End If


        '''''Redirect to the Products Page
        ''''Response.Redirect(Me.CurrentSession.aVT_DetailsPageOptionsPages(1))

    End Sub


    Function SetUpOrderSessionVariables() As Boolean

        Dim objForms As New VT_Forms.Forms
        Dim dtTemp As New DataTable
        dtTemp = Session("_VT_DispDocsManagerRowSelected")

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

                Me.CurrentSession.VT_JobStatus = .Item("Status")

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

            Me.CurrentSession.VT_JobStatus = .Item("Status")

            Me.CurrentSession.VT_NewOrderMatrixID = objForms.GetMatrixLinkIdForJob(lngId)
            Me.CurrentSession.VT_OrderMatrixID = objForms.GetMatrixLinkIdForJob(lngId)

            Me.CurrentSession.VT_NewOrderNum = Me.CurrentSession.VT_SalesOrderNum
            Me.CurrentSession.VT_NewOrderID = Me.CurrentSession.VT_SalesOrderID
            Me.CurrentSession.VT_CurrentNewOrderStatus = Me.CurrentSession.VT_JobStatus


        End With


        SetUpOrderSessionVariablesPerRow = True

    End Function

    Protected Sub btnOnHoldTV_Click(sender As Object, e As ImageClickEventArgs) Handles btnOnHoldTV.Click
        If chkShowCustDetails.Checked = True Then
            Session("_VT_ShowCustomerDetailsInGrid") = "True"
        Else
            Session("_VT_ShowCustomerDetailsInGrid") = "False"
        End If
        ' Response.Redirect("~/TabPages/HoldQueueTV.aspx")

    End Sub



    Protected Sub btnViewPrices_Click(sender As Object, e As ImageClickEventArgs) Handles btnViewPrices.Click
        ''''Dim dtTemp As New DataTable
        ''''dtTemp = Session("_VT_DispDocsManagerRowSelected")
        ''''Dim objProd As New VTDBFunctions.VTDBFunctions.ProductsFunctions
        ''''Dim objCust As New VTDBFunctions.VTDBFunctions.CustomerFunctions

        ''''If dtTemp.Rows.Count > 0 Then
        ''''    With dtTemp.Rows(0)

        ''''        Session("SelectedProductId") = objProd.GetProductIdForCode(.Item("ProductCode"))

        ''''        Session("SelectedCustomerID") = objCust.GetCustomerIdForRef(.Item("SoldTo_Code"))
        ''''        If Session("SelectedProductId") = "" Then 'that product is not yet in our system, we can't edit the prices for it
        ''''            lblMsg.Text = "You cannot add Or edit prices For a product that has Not yet been added To the eq trace database."
        ''''            ModalPopupExtenderMsg.Show()
        ''''            Exit Sub

        ''''        End If
        ''''        'Setup the appropriate return path
        ''''        Me.CurrentSession.OptionsPageToReturnTo = "~/TabPages/HoldQueueManager.aspx"
        ''''        Response.Redirect("~/TabPages/EditPriceList.aspx")
        ''''    End With
        ''''End If

    End Sub




    Protected Sub wdgRepGrid_InitializeRow(sender As Object, e As Infragistics.Web.UI.GridControls.RowEventArgs) Handles wdgRepGrid.InitializeRow

        If e.Row.Items.FindItemByKey("UnreadComments").Text = "MFG" Then
            e.Row.Items.FindItemByKey("UnreadComments").Text = "<img src='mail.gif'></img>"
        End If
    End Sub


    Protected Sub btnAddNewComment_Click(sender As Object, e As ImageClickEventArgs) Handles btnAddNewComment.Click
        ''''Dim strSalesORderNum As String
        ''''Dim dtTemp As New DataTable
        ''''Dim strLock As String
        ''''Dim strUserName As String
        ''''Dim struserID As String

        ''''dtTemp = Session("_VT_DispDocsManagerRowSelected")

        ''''If dtTemp.Rows.Count > 0 Then
        ''''    strSalesORderNum = dtTemp.Rows(0).Item("SalesOrderNum")
        ''''    strLock = IsThisOrderLockedForEdit(strSalesORderNum)
        ''''    If strLock <> "NO" Then 'it is locked, show a message
        ''''        strUserName = Mid(strLock, InStr(strLock, "UserName:") + 9)
        ''''        strUserName = Left(strUserName, InStr(strUserName, ",") - 1)
        ''''        struserID = Mid(strLock, InStr(strLock, "UserId:") + 7)
        ''''        struserID = Left(struserID, InStr(struserID, ",") - 1)
        ''''        hdnAlertUserId.Value = struserID
        ''''        hdnAlertJobId.Value = strSalesORderNum
        ''''        hdnAlertSONum.Value = dtTemp.Rows(0).Item("SO_ContiguousNum")

        ''''        'if it is already checked out to the currently logged in user then they can edit it
        ''''        If Session("_VT_CurrentUserId").ToString = struserID Then
        ''''            ModalPopupExtenderMessage.Show()
        ''''        Else
        ''''            lblTask.Text = "The selected order is currently being edited by: " & strUserName & ". It cannot be edited until that user has finished with it."
        ''''            ModalPopupExtenderTask.Show()
        ''''        End If


        ''''    Else 'not locked, check it out to the current user and continue
        ''''        If CheckOrderOutToCurrentUser() Then

        ''''            ModalPopupExtenderMessage.Show()
        ''''        End If


        ''''    End If

        ''''Else
        ''''    lblMsg.Text = "You must select a line first before you add or edit a comment!"
        ''''    ModalPopupExtenderMsg.Show()

        ''''End If

    End Sub
    Function CheckOrderOutToCurrentUser() As Boolean
        CheckOrderOutToCurrentUser = True

        'SmcN 22/05/2014  Handle Edit/Lock facilities.
        ''''Dim strFormLock As String = ""
        ''''Dim objPres As New VT_DataPreserve.DataPreserve
        ''''Dim objVTM As New VTDBFunctions.VTDBFunctions.VTMatrixFunctions
        ''''Dim objData As New BPADotNetCommonFunctions.VT_DataPreserve.DataPreserve
        ''''Dim intIdToUse As Integer
        ''''Dim strMessage As String
        ''''Dim objForms As New VT_Forms.Forms
        ''''Dim lngUserId As Long = Session("_VT_CurrentUserId")


        '''''SmcN 02/06/2014 If the order status has progressed too far then you are not allowed to edit the Order
        ''''If Me.CurrentSession.VT_CurrentNewOrderStatus = GetGlobalResourceObject("Resource", "Order_SendAcknowledge") Then
        ''''    CheckOrderOutToCurrentUser = False

        ''''    Exit Function
        ''''End If

        ''''If Me.CurrentSession.VT_CurrentNewOrderStatus = GetGlobalResourceObject("Resource", "Order_AwaitingPlanning") Then
        ''''    CheckOrderOutToCurrentUser = False

        ''''    Exit Function
        ''''End If
        ''''If Me.CurrentSession.VT_CurrentNewOrderStatus.Contains("Closed") Then
        ''''    CheckOrderOutToCurrentUser = False

        ''''    Exit Function

        ''''End If


        ''''If IsNothing(Me.CurrentSession.VT_OrderMatrixID) OrElse Me.CurrentSession.VT_OrderMatrixID = 0 Then
        ''''    'Do nothing the Form should have defaulted to Checked Out - Edit Mode on page load if this was a new item
        ''''    CheckOrderOutToCurrentUser = False
        ''''Else
        ''''    intIdToUse = Me.CurrentSession.VT_OrderMatrixID
        ''''    ' get the CheckIn status for this item
        ''''    strFormLock = objVTM.ReadFormLock(Session("_VT_DotNetConnString"), intIdToUse, Me.CurrentSession.FormDataTable)
        ''''    If strFormLock = "" Then
        ''''        ' if it is not checked out then check it out to this user

        ''''        ' the form is not currently locked so we can lock it (check it out) to the current user
        ''''        objVTM.LockFormForWriting(Session("_VT_DotNetConnString"), intIdToUse, Me.CurrentSession.FormDataTable, objData.CreateFormLockString(Session("_VT_CurrentUserName"), Session("_VT_UserHostAddress"), Session("_VT_CurrentUserId")))

        ''''        Me.CurrentSession.VT_FormCheckedOutToCurrentUser = True
        ''''        CheckOrderOutToCurrentUser = True
        ''''    End If

        ''''End If
    End Function

    Sub CheckOrderBackIn()
        ''''Dim objPres As New VT_DataPreserve.DataPreserve
        ''''Dim objVTM As New VTDBFunctions.VTDBFunctions.VTMatrixFunctions
        ''''Dim objData As New BPADotNetCommonFunctions.VT_DataPreserve.DataPreserve
        ''''Dim intIdToUse As Integer
        ''''Dim strFormLock As String = ""


        ''''If IsNothing(Me.CurrentSession.VT_OrderMatrixID) OrElse Me.CurrentSession.VT_OrderMatrixID = 0 Then
        ''''    'Do nothing the Form should have defaulted to Checked Out - Edit Mode on page load if this was a new item
        ''''Else
        ''''    intIdToUse = Me.CurrentSession.VT_OrderMatrixID

        ''''    Dim blnIsFormCheckedOutByCurrentUser As Boolean = objData.IsFormCheckedOutByCurrentUser(Session("_VT_DotNetConnString"), Session("_VT_CurrentUserId"), intIdToUse, Me.CurrentSession.FormDataTable)
        ''''    If blnIsFormCheckedOutByCurrentUser = True Then
        ''''        objVTM.LockFormForWriting(Session("_VT_DotNetConnString"), intIdToUse, Me.CurrentSession.FormDataTable, "")


        ''''        Me.CurrentSession.VT_FormCheckedOutToCurrentUser = False

        ''''    End If

        ''''End If
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
        ''''Dim strSalesORderNum As String
        ''''Dim dtTemp As New DataTable
        ''''Dim strLock As String
        ''''Dim strUserName As String
        ''''Dim struserID As String

        ''''dtTemp = Session("_VT_DispDocsManagerRowSelected")

        ''''If dtTemp.Rows.Count > 0 Then
        ''''    strSalesORderNum = dtTemp.Rows(0).Item("SalesOrderNum")
        ''''    strLock = IsThisOrderLockedForEdit(strSalesORderNum)
        ''''    If strLock <> "NO" Then 'it is locked, show a message
        ''''        strUserName = Mid(strLock, InStr(strLock, "UserName:") + 9)
        ''''        strUserName = Left(strUserName, InStr(strUserName, ",") - 1)
        ''''        struserID = Mid(strLock, InStr(strLock, "UserId:") + 7)
        ''''        struserID = Left(struserID, InStr(struserID, ",") - 1)
        ''''        hdnAlertUserId.Value = struserID
        ''''        hdnAlertJobId.Value = strSalesORderNum
        ''''        hdnAlertSONum.Value = dtTemp.Rows(0).Item("SO_ContiguousNum")

        ''''        'if it is already checked out to the currently logged in user then they can edit it
        ''''        If Session("_VT_CurrentUserId").ToString = struserID Then
        ''''            ModalPopupExtenderPersonAssign.Show()
        ''''        Else
        ''''            lblTask.Text = "The selected order is currently being edited by: " & strUserName & ". It cannot be edited until that user has finished with it."
        ''''            ModalPopupExtenderTask.Show()
        ''''        End If


        ''''    Else 'not locked, check it out to the current user and continue
        ''''        If CheckOrderOutToCurrentUser() Then

        ''''            ModalPopupExtenderPersonAssign.Show()
        ''''        End If


        ''''    End If

        ''''Else
        ''''    lblMsg.Text = "You must select a line first before you assign a person!"
        ''''    ModalPopupExtenderMsg.Show()

        ''''End If
    End Sub

    Protected Sub cmdPersonAssignCancel_Click(sender As Object, e As ImageClickEventArgs) Handles cmdPersonAssignCancel.Click
        CheckOrderBackIn()

    End Sub
End Class



