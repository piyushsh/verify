Imports BPADotNetCommonFunctions
Imports System.Data.SqlClient
Imports System.Data
Imports VTDBFunctions
Imports Infragistics.Web.UI.GridControls
Imports Microsoft.Office.Interop
Imports BPADotNetCommonFunctions.VT_CommonFunctions.MatrixFunctions
Imports VTDBFunctions.VTDBFunctions

Partial Class MobilePages_mOrdersOpening
    Inherits MyBasePage
    Protected Sub btnViewAttachments_Click(sender As Object, e As EventArgs)
        Dim strVTStore As String = Replace(ConfigurationManager.AppSettings("VTStore"), "XXXX", Session("_VT_CurrentDID"))
        Dim StartNodeText As String = "SalesOrders\OrderNum_" + Me.CurrentSession.VT_SalesOrderNum.ToString
        strVTStore = strVTStore + "&Auth=Ok&UID=" + Session("_VT_CurrentUserId").ToString + "&ShowBanner=No&StartNodeText=" + StartNodeText + "&UserName=" + Session("_VT_CurrentUserName")


        Session("_VT_PageToReturnToFromModulesIFrame") = "~/TabPages/Orders_Opening_New.aspx" 'Me.CurrentSession.VT_SelectedFormPath

        Session("_VT_iFrameHeight") = 580
        Dim objVTM As New VTDBFunctions.VTDBFunctions.VTMatrixFunctions
        Dim objData As New BPADotNetCommonFunctions.VT_DataPreserve.DataPreserve

        Me.CurrentSession.VT_ProjectInIFrame = strVTStore
        Response.Redirect("~/IFrame Pages/ModulesIFrame.aspx")
    End Sub

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        Dim objDisp As New DisplayFunctions
        Dim objConn As New VTDBFunctions.VTDBFunctions.DBConn
        Dim strconn As String = objConn.SQLConnStringForDotNet

        If Not IsPostBack Then
            objDisp.SetupStatusDropdown(ddlStatus, strconn)
            'set the date pickers to today by default
            dteEnd.Value = PortalFunctions.Now.Date
            dteStart.Value = PortalFunctions.Now.Date
            Startsearch()

        End If

    End Sub
  
    Protected Sub btnEdit_Click(sender As Object, e As EventArgs) Handles btnEdit.Click

        If IsNothing(Me.CurrentSession.VT_SelectedSalesOrderRow_V2) Then
            lblMsg.Text = ("You must select an order before you can edit it!")
            ModalPopupExtender1.Show()

            Exit Sub

        End If

        Dim strStatusForEdit = Trim(Me.CurrentSession.VT_SelectedSalesOrderRow_V2.Rows(0).Item("Status"))

        'SmcN 22/05/2014 Proper CheckOut/Edit Form locking has now been implemented so always show the Edit button so users can review All order data as required.
        Dim objCommon As New BPADotNetCommonFunctions.VT_CommonFunctions.CommonFunctions
        Dim strCompany As String = objCommon.GetConfigItem("SystemFormsFolder")
        ' If UCase(strCompany) <> "STERIPACK" Then
        'IF status is Open - Pre Issued then you can edit the order so enable button
        If strStatusForEdit = GetGlobalResourceObject("Resource", "Order_PreIssued") _
            Or strStatusForEdit = GetGlobalResourceObject("Resource", "Order_OnHold") _
            Or strStatusForEdit = GetGlobalResourceObject("Resource", "Order_OnHold_System") _
            Or strStatusForEdit = GetGlobalResourceObject("Resource", "Order_OnHold_Manual") _
            Or strStatusForEdit = GetGlobalResourceObject("Resource", "Order_AwaitingPlanning") _
             Or strStatusForEdit = GetGlobalResourceObject("Resource", "Order_AwaitingIssue") Then

            btnEdit.Visible = True
        Else
            'Not allowed to edit

            lblMsg.Text = ("You are not allowed to Edit a Sales Order which is at the status of [ " & strStatusForEdit & " ]")
            ModalPopupExtender1.Show()

            Exit Sub

        End If
        '  End If


        Dim objTelesales As New SalesOrdersFunctions.SalesOrders
        Dim objSO As New VTDBFunctions.VTDBFunctions.TelesalesFunctions
        Dim dsOrder As New DataSet

        ''Fill pages array
        'objTelesales.FillOrderPagesArray()

        'Open New Order
        Me.CurrentSession.VT_NewOrderNum = Me.CurrentSession.VT_SalesOrderNum
        Me.CurrentSession.VT_NewOrderID = Me.CurrentSession.VT_SalesOrderID
        Me.CurrentSession.VT_CurrentNewOrderStatus = Me.CurrentSession.VT_JobStatus

        dsOrder = objSO.GetOrderForId(Me.CurrentSession.VT_SalesOrderID)

        If IsNothing(dsOrder) = False AndAlso dsOrder.Tables(0).Rows.Count > 0 Then
            Me.CurrentSession.VT_NewOrderCustomerID = dsOrder.Tables(0).Rows(0).Item("CustomerId")
            Me.CurrentSession.VT_NewOrderDeliveryCustomerID = dsOrder.Tables(0).Rows(0).Item("DeliveryCustomerId")
            Me.CurrentSession.VT_CustomerPO = IIf(IsDBNull(dsOrder.Tables(0).Rows(0).Item("CustomerPO")), "", dsOrder.Tables(0).Rows(0).Item("CustomerPO"))

        Else
            Me.CurrentSession.VT_NewOrderCustomerID = 0
            Me.CurrentSession.VT_NewOrderDeliveryCustomerID = 0
        End If

        Me.CurrentSession.VT_NewOrderUseBillingCustomer = False
        Me.CurrentSession.VT_NewOrderSelectedFormName = ""

        'SmcN 13/05/2014 Set the return path for the New Order Pages
   
        Session("_VT_SalesOrderReturnPath") = "mOrdersOpening.aspx"


        Response.Redirect("mNewOrderDetails.aspx")

    End Sub

    Protected Sub btnSearch_Click(sender As Object, e As ImageClickEventArgs) Handles btnSearch.Click
        Startsearch()
    End Sub

    Public Sub Startsearch()
        Dim strFilter As String = ""
        '    If Me.CurrentSession.VT_FilterOn = False Then

        Dim strIndexCol As String
        Dim strIndexType As String
        Dim blnCriteriaAdded As Boolean
        Dim intCnt As Integer
        Dim blnMatrixColCheck As Boolean = False
        Dim strSOPrimaryOrderNumKey As String



        'SMCN 25/05/2014 Added sections to Deal with Columns that come from the Matrix
        'A seperate filter string  'VT_MatrixFilterText' is filled for any columns that ar ein the matrix.
        ' This filter string can be later used to futhr filter down any records that are returned from the schema on the Main search query
        'First define a list of the matrix columns
        Dim astrItems(4) As String
        astrItems(0) = "ReasonOnHold"
        astrItems(1) = "DeliverTo_DateOut"
        astrItems(2) = "StatusBefore_OnHold"
        astrItems(3) = "SO_ContiguousNum"
        astrItems(4) = "CustomerOrderDate"


        If Me.CurrentSession.blnOrderStatusChanged <> True Then
            Me.CurrentSession.VT_GridFilterText = ""
            Me.CurrentSession.VT_MatrixFilterText = ""
        End If


        blnCriteriaAdded = False


        If ddlStatus.Text <> "" Then
            If blnCriteriaAdded = True Then
                Me.CurrentSession.VT_GridFilterText = Me.CurrentSession.VT_GridFilterText & " AND "
            End If
            If Me.CurrentSession.VT_GridFilterText <> " wfo_BatchTable.JobStatusText LIKE '" + CStr(ddlStatus.Text) & "%'" Then
                ' Status is stored in JobStatusText
                Me.CurrentSession.VT_GridFilterText = Me.CurrentSession.VT_GridFilterText & " wfo_BatchTable.JobStatusText LIKE '" & CStr(ddlStatus.Text) & "%'"
                blnCriteriaAdded = True
            End If

        End If

        If chkCurrentUserOnly.Checked = True Then
            If blnCriteriaAdded = True Then
                Me.CurrentSession.VT_GridFilterText = Me.CurrentSession.VT_GridFilterText & " AND "
            End If
            If Me.CurrentSession.VT_GridFilterText <> " tls_SalesOrders.PersonLoggingOrder = " & Session("_VT_CurrentUserId") Then
                ' Status is stored in JobStatusText
                Me.CurrentSession.VT_GridFilterText = Me.CurrentSession.VT_GridFilterText & " tls_SalesOrders.PersonLoggingOrder = " & Session("_VT_CurrentUserId")
                blnCriteriaAdded = True
            End If
        End If

        Dim lngDateLength As Long

        If dteStart.Value <> Nothing And dteEnd.Value <> Nothing Then
            lngDateLength = DateDiff(DateInterval.Day, dteStart.Value, dteEnd.Value)
        End If

        If Me.CurrentSession.VT_GridFilterText = "" And dteStart.Value = Nothing And dteEnd.Value = Nothing Then
            lblMsg.Text = "You have no search criteria selected." & vbCrLf & "If you run this search it will retrieve all the orders in the system and so could take a long time. "

            ModalPopupExtender1.Show()
        ElseIf Left(ddlStatus.Text, 6) = "Closed" Then
            lblMsg.Text = "If you run this search it may retrieve a large number of orders in the system and so could take a long time. "
            ModalPopupExtender1.Show()
        ElseIf lngDateLength > 60 Then

            lblMsg.Text = "If you run this search it may retrieve a large number of orders in the system and so could take a long time. "
            ModalPopupExtender1.Show()
        Else
            fillgrid()

        End If

    End Sub
    Sub fillgrid()
        Dim strFilter As String
        Dim dteStartDate As Date
        Dim dteEndDate As Date
        Dim strConn As String = Session("_VT_DotNetConnString")
        Dim dtTemp As DataTable
        Dim objDataPreserve As New BPADotNetCommonFunctions.VT_DataPreserve.DataPreserve
      
        strFilter = ""

        Me.CurrentSession.VT_JobID = 0
        If Me.CurrentSession.VT_GridFilterText <> "" Then
            strFilter = Me.CurrentSession.VT_GridFilterText
        End If

        If dteStart.Value = Nothing Then
            dteStartDate = #12:00:00 AM#
        Else
            dteStartDate = dteStart.Value
        End If

        If dteEnd.Value = Nothing Then
            dteEndDate = #12:00:00 AM#
        Else
            dteEndDate = dteEnd.Value
        End If


        If Me.CurrentSession.VT_GridFilterText = "" And dteStart.Value = Nothing And dteEnd.Value = Nothing Then
            Exit Sub
        End If

        Dim dt As New DataTable
        Dim objSales As New BPADotNetCommonFunctions.SalesFunctions.Sales_Orders

        dt = objSales.GetDataForSalesOrdersOpeningGrid(strFilter, dteStartDate, dteEndDate, strConn, Me.CurrentSession.VT_OrderTypesEnabled, Me.CurrentSession.VT_tlsNumFields, Me.CurrentSession.VT_MatrixFilterText)


        objDataPreserve.BindDataToWDG(dt, wdgJobs)
     

        Me.CurrentSession.VT_OrdersDataTable = dt
        'clear any previously selected rows, the data in the grid may be different now but the old row will remain selected
        wdgJobs.Behaviors.Selection.SelectedRows.Clear()
        wdgJobs.Behaviors.Activation.ActiveCell = Nothing
        Session("SelectedRowIndex") = Nothing
        Me.CurrentSession.VT_SelectedSalesOrderRow_V2 = Nothing

      
    End Sub

        Protected Sub btnClearAllTop_Click(sender As Object, e As ImageClickEventArgs) Handles btnClearAllTop.Click
        'SmCN 26/05/2014 this function is for clearing the 'Advanced Search' from the filter

        Dim blnCriteriaAdded As Boolean
        Dim strSOPrimaryOrderNumKey As String

        chkCurrentUserOnly.Checked = False

        ddlStatus.ClearSelection()
        dteEnd.Value = PortalFunctions.Now.Date
        dteStart.Value = PortalFunctions.Now.Date

        Me.CurrentSession.VT_MatrixFilterText = ""
        Me.CurrentSession.VT_GridFilterText = ""


    End Sub

    Protected Sub wdgJobs_ActiveCellChanged(sender As Object, e As Infragistics.Web.UI.GridControls.ActiveCellEventArgs) Handles wdgJobs.ActiveCellChanged

        SelectRow()


    End Sub




    Protected Sub btnNewOrder_Click(sender As Object, e As EventArgs) Handles btnNewOrder.Click
        Dim objTelesales As New SalesOrdersFunctions.SalesOrders
        'Fill pages array
  
            'Open New Order
            Me.CurrentSession.VT_NewOrderNum = 0
            Me.CurrentSession.VT_NewOrderID = 0
            Me.CurrentSession.VT_CurrentNewOrderStatus = ""
            Me.CurrentSession.VT_NewOrderCustomerID = 0
            Me.CurrentSession.VT_NewOrderDeliveryCustomerID = 0
            Me.CurrentSession.VT_NewOrderUseBillingCustomer = False
            Me.CurrentSession.VT_NewOrderSelectedFormName = ""
            Me.CurrentSession.VT_NewOrderMatrixID = 0

            'Set the return path

            Session("_VT_SalesOrderReturnPath") = "mOrdersOpening.aspx"



        Response.Redirect("mOrderDetails.aspx")


    End Sub

    Sub SelectRow()
        Dim objC As New VT_CommonFunctions.CommonFunctions

        'serialise the selected data row
        Dim RowSelected As Infragistics.Web.UI.GridControls.SelectedRowCollection = wdgJobs.Behaviors.Selection.SelectedRows
        Dim intActiveRowIndex As Integer = RowSelected(0).Index
        Session("SelectedRowIndex") = intActiveRowIndex
        Me.CurrentSession.VT_SelectedSalesOrderRow_V2 = objC.SerialiseWebDataGridRow(wdgJobs, intActiveRowIndex)

        'Set the control variables for the currently selected Sales Order
        Dim lngId As Long = CType(Me.CurrentSession.VT_SelectedSalesOrderRow_V2.Rows(0).Item("JobID"), Long)
        Me.CurrentSession.VT_JobID = lngId
        Me.CurrentSession.VT_SalesOrderNum = lngId
        Dim lngSalesOrderID As Long = Me.CurrentSession.VT_SelectedSalesOrderRow_V2.Rows(0).Item("SalesOrderId")
        Me.CurrentSession.VT_SalesOrderID = lngSalesOrderID

        'SMcN 06/03/2014  Add Store for Contiguous Sales Order Num
        If Me.CurrentSession.VT_SelectedSalesOrderRow_V2.Columns.Contains("SO_ContiguousNum") Then
            Me.CurrentSession.VT_SalesContiguousNum = Me.CurrentSession.VT_SelectedSalesOrderRow_V2.Rows(0).Item("SO_ContiguousNum")
        End If


        Dim strCustomerName As String = Me.CurrentSession.VT_SelectedSalesOrderRow_V2.Rows(0).Item("CustomerName")
        Me.CurrentSession.VT_CustomerName = strCustomerName
        Session("_VT_SelectedWorkOrderGridRow") = Me.CurrentSession.VT_SelectedSalesOrderRow_V2
        Session("_VT_SelectedOrderItemGridRow") = Nothing
        Session("_VT_SelectedDocketGridRow") = Nothing
        Dim strStatus As String = Me.CurrentSession.VT_SelectedSalesOrderRow_V2.Rows(0).Item("Status")
        Me.CurrentSession.VT_JobStatus = strStatus

        Me.CurrentSession.VT_OrderType = Trim(Me.CurrentSession.VT_SelectedSalesOrderRow_V2.Rows(0).Item("OrderType"))

        Dim strPriority As String = Me.CurrentSession.VT_SelectedSalesOrderRow_V2.Rows(0).Item("Priority")
        Me.CurrentSession.VT_OrderPriority = strPriority

        Dim ds As New DataSet
        Dim objProduction As New ProductionFunctions.Production
        ds = objProduction.GetJobSpecifics(lngId)
        If ds.Tables(0).Rows.Count > 0 Then
            Me.CurrentSession.VT_JobComment = IIf(IsDBNull(ds.Tables(0).Rows(0).Item("Comment")), "", ds.Tables(0).Rows(0).Item("Comment"))
        Else
            Me.CurrentSession.VT_JobComment = ""
        End If

        Dim objForms As New VT_Forms.Forms
        Me.CurrentSession.VT_NewOrderMatrixID = objForms.GetMatrixLinkIdForJob(lngId)
        Me.CurrentSession.VT_OrderMatrixID = objForms.GetMatrixLinkIdForJob(lngId)

    End Sub
    Private Sub wdgJobs_RowSelectionChanged(sender As Object, e As SelectedRowEventArgs) Handles wdgJobs.RowSelectionChanged
        SelectRow()
    End Sub
End Class
