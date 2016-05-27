Imports BPADotNetCommonFunctions
Imports BPADotNetCommonFunctions.VT_CommonFunctions.MatrixFunctions
Imports System.Data.SqlClient
Imports System.Data
Imports System.Drawing
Imports System.Data.DataTableExtensions

Partial Class TabPages_AcknowledgeOrders
    Inherits MyBasePage


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim objDataPreserve As New BPADotNetCommonFunctions.VT_DataPreserve.DataPreserve
        ' btnMarkAsAcknowledged.OnClientClick = String.Format("return DeselectGridRows('{0}','{1}')", btnMarkAsAcknowledged.UniqueID, "")
        cmdMsgOK.OnClientClick = String.Format("return ForcePostback('{0}','{1}')", cmdMsgOK.UniqueID, "")

        If Not IsPostBack Then
            FillData()
            InitialiseAndBindEmptyOrderItemsGrid()
        Else
            wdgRepGrid.DataSource = objDataPreserve.GetWDGDataFromSession(wdgRepGrid)

        End If

        'SmcN 26/01/2015 Set the grid width if required
        If Me.CurrentSession.VT_BrowserWindowWidth < 1400 And Not Me.CurrentSession.VT_BrowserWindowWidth Is Nothing Then
            wdgRepGrid.Width = Me.CurrentSession.VT_BrowserWindowWidth - 54

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

        '  Close the connection when done with it.
        myConnection.Close()

        GetOrderDataForStatus = dt

    End Function



    Sub FillData()
        Dim objG As New BPADotNetCommonFunctions.VT_ReportFunctions.GeneralFunctions
        Dim objSales As New BPADotNetCommonFunctions.SalesFunctions.Sales_Orders
        Dim objC As New VT_CommonFunctions.CommonFunctions
        Dim strConn As String = Session("_VT_DotNetConnString")


        Dim objDataPreserve As New BPADotNetCommonFunctions.VT_DataPreserve.DataPreserve
        Dim dtoutput As New DataTable
        Dim strStatusSearch As String = GetGlobalResourceObject("Resource", "Order_SendAcknowledge")
        Dim dt As DataTable

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

                'SmcN 28/05/2014  Sort the grid here on the Sales Order Number and then the item number.
                dtoutput = objG.SortDataTable("SO_ContiguousNum ASC, SalesItemNum ASC", dtoutput)
                'If wdgRepGrid.Rows.Count > 0 Then
                '    wdgRepGrid.Behaviors.Selection.SelectedRows.Add(wdgRepGrid.Rows(0))
                '    Session("_VT_AcknowledgementRowSelected") = objC.SerialiseWebDataGridRow(wdgRepGrid, 0)
                'End If
                'wdgRepGrid.Behaviors.Selection.SelectedRows.Clear()
                'wdgRepGrid.Behaviors.Activation.ActiveCell = Nothing

                objDataPreserve.BindDataToWDG(dtoutput, wdgRepGrid)
                UpdatePanel1.Update()

            Else
                InitialiseAndBindEmptyPlanningGrid()
            End If
        Else

            InitialiseAndBindEmptyPlanningGrid()
        End If

        'Save a copy of this data table for checking which Values have changed when storing changes later.
        Session("_VT_AwaitingPlaning") = dtoutput.Copy


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


    Sub InitialiseAndBindEmptyPlanningGrid()

        Dim dtItemsForGrid As New DataTable
        Dim objDataPreserve As New BPADotNetCommonFunctions.VT_DataPreserve.DataPreserve

        'build a blank datatable and bind the grids to these - incase there is no data
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
        'objDataPreserve.BindDataToWDG(dtItemsForGrid, wdgOpenOtherStatus)


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
        'objDataPreserve.BindDataToWDG(dtItemsForGrid, wdgOrderItems)


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

            dtThis.Columns.Add("ProductName", Type.GetType("System.String"))
            dtThis.Columns.Add("ProductCode", Type.GetType("System.String"))
            dtThis.Columns.Add("TotalExclVat")
            dtThis.Columns.Add("VATRate")
            dtThis.Columns.Add("UnitPriceCurrency", Type.GetType("System.String"))
            dtThis.Columns.Add("VATCurrency", Type.GetType("System.String"))
            dtThis.Columns.Add("TotalPriceCurrency", Type.GetType("System.String"))
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

            dtThis.Columns.Add("Site")

            dtThis.Columns.Add("SO_ContiguousNum")
            dtThis.Columns.Add("SalesOrderNum")
            dtThis.Columns.Add("SalesOrderId")
            dtThis.Columns.Add("CustomerId")

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

    Protected Sub btnMarkAsAcknowledged_Click(sender As Object, e As ImageClickEventArgs) Handles btnMarkAsAcknowledged.Click

        'Set the status of the selected row to 'Closed'
        'SmcN 23/10/2013   Note this status s bein gset to closed fo rth emoment becasue at this point the Sales order process is completed within Steripack
        'At a future date it is intended that The Warehouse will be added to the process and fulfillment will be completed via the portal.

        Dim objForms As New VT_Forms.Forms
        Dim objTasks As New VT_eQOInterface.eQOInterface
        Dim objTele As New VTDBFunctions.VTDBFunctions.TelesalesFunctions
        Dim dtTemp As New DataTable
        Dim strCurrentSalesOrderNum As String
        Dim objSales As New BPADotNetCommonFunctions.SalesFunctions.Sales_Orders

        Dim strConn As String = Session("_VT_DotNetConnString")
        Dim selectedProductRows As Infragistics.Web.UI.GridControls.SelectedRowCollection = wdgRepGrid.Behaviors.Selection.SelectedRows

        Dim strTemp As String
        'If Session("_VT_AcknowledgementRowSelected") IsNot Nothing AndAlso Session("_VT_AcknowledgementRowSelected").rows.count > 0 Then
        For Each wdgRow As Infragistics.Web.UI.GridControls.GridRecord In selectedProductRows
            strCurrentSalesOrderNum = wdgRow.Items.FindItemByKey("SalesOrderNum").Value
            Dim lngSalesOrderID As Long
            'Set status to Closed
            'if this is steripack then the order will be closed after this 
            dtTemp = objTele.GetOrderForNum(CLng(strCurrentSalesOrderNum))
            lngSalesOrderID = dtTemp.Rows(0).Item("SalesOrderId")

            If Session("Accounts") = "MFGPRO" Then
                strTemp = GetGlobalResourceObject("Resource", "Order_Closed")
           

                objForms.SetJobStatusText(strTemp, CLng(strCurrentSalesOrderNum))
                objTasks.SetLastActivity(CLng(strCurrentSalesOrderNum), PortalFunctions.Now.Date)


                objTele.UpdateOrderStatus(lngSalesOrderID, strTemp)

                'save the date closed
                objSales.SaveGeneralItemsToSalesMatrix("DateClosed", "Details", PortalFunctions.Now.ToString("s"), CInt(strCurrentSalesOrderNum), strConn)
            Else
                'issue the order for dispatch
                'if this order has already been issued do nothing
                If Me.CurrentSession.VT_CurrentNewOrderStatus <> GetGlobalResourceObject("Resource", "Order_New") Then
                    HandleIssue(Session("_VT_CurrentUserId"), Session("_VT_CurrentUserName"), strCurrentSalesOrderNum, lngSalesOrderID)
                End If

                End If

                'audit log
                Dim strType As String
                strType = "Sales Order"
            objForms.WriteToAuditLog(CLng(strCurrentSalesOrderNum), strType, PortalFunctions.Now, Session("_VT_CurrentUserName") & "[Acknowledge Page]:", 0, "Order Confirmed as Acknowledged to Customer ", "Audit Record", "SysAdmin")
            Session("OrdersAcknowledged") = "YES"
                'lblMsg.Text = "Your order has successfully been acknowledged"
                'ModalPopupExtenderMsg.Show()
        Next
        'lblMsg.Text = "No row selected! Please select a row in the grid and click on the button again"
        'ModalPopupExtenderMsg.Show()

        ''no row selected
        'End If

        Session("_VT_AcknowledgementRowSelected") = Nothing
        FillData()

    End Sub
    Sub HandleIssue(ByVal intUserId As Integer, ByVal strUserName As String, ByVal lngOrderNum As Long, ByVal lngOrderId As Long)
        Dim strMessage As String
        Dim dtNewOrder As New DataTable
        Dim objTelesales As New VTDBFunctions.VTDBFunctions.TelesalesFunctions
        Dim objPer As New PersonnelModuleFunctions
        Dim objTasks As New VT_eQOInterface.eQOInterface
        Dim strAuditComment As String
        Dim objForms As New VT_Forms.Forms
        Dim lngNewOrderNum As Long
        Dim strOrderComment As String
        Dim strRoute As String
        Dim dteDeliveryDate As Date
        Dim strPriority As String
        Dim strPO As String
        Dim dsOrderJobDetails As New DataSet
        Dim objPdn As New VTDBFunctions.VTDBFunctions.Production
        Dim strOrderType As String
        Dim dsOrderItems As New DataSet
        Dim lngSalesOrderItemId As Long
        Dim intTraceCodeId As Integer
        Dim dtTemp As New DataTable
        Dim objSales As New BPADotNetCommonFunctions.SalesFunctions.Sales_Orders

        Dim strConn As String = Session("_VT_DotNetConnString")


        If lngOrderNum <> 0 Then

            ' ''Run the Validation checks
            ''dtTemp = objSales.GetSalesOrderItemsFromMatrix("OrderItems", CInt(Me.CurrentSession.VT_SalesOrderNum), strConn, Me.CurrentSession.VT_tlsNumFields)
            ''strMessage = ""

            ''If dtTemp.Rows.Count = 0 Then
            ''    strMessage = strMessage & "  There are no items on this order ! <br/>"
            ''End If

            ''For intCnt As Integer = 0 To dtTemp.Rows.Count - 1
            ''    If IsDBNull(dtTemp.Rows(intCnt).Item("ProductCode")) Or dtTemp.Rows(intCnt).Item("ProductCode") = "" Then
            ''        strMessage = strMessage & "  Line Item " & CStr(dtTemp.Rows(intCnt).Item("VT_UniqueIndex") + 1) & " does not have a product code ! <br/>"
            ''    End If
            ''    If dtTemp.Rows(intCnt).Item("PriceDifference") > 0.001 Then
            ''        strMessage = strMessage & "  Line Item " & CStr(dtTemp.Rows(intCnt).Item("VT_UniqueIndex") + 1) & " Has a price difference <br/>"
            ''    End If

            ''Next

            ''dtTemp = objSales.GetSalesOrderItemsFromMatrix("CustomerItems", CInt(Me.CurrentSession.VT_SalesOrderNum), strConn, Me.CurrentSession.VT_tlsNumFields)

            ''If dtTemp.Rows.Count = 0 Then
            ''    strMessage = strMessage & "  There are no Customer datails saved for this order ! <br/>"
            ''End If

            ''If IsDBNull(dtTemp.Rows(0).Item("DeliverTo_RequestedDate")) Or dtTemp.Rows(0).Item("DeliverTo_RequestedDate") = "" Then
            ''    strMessage = strMessage & "  There is no 'Requested Delivery' date entered on the Customer Details page for this Sales order ! <br/>"
            ''End If
            ''If Session("Accounts") = "MFGPRO" Then
            ''    If IsDBNull(dtTemp.Rows(0).Item("DeliverTo_DateOut")) Or dtTemp.Rows(0).Item("DeliverTo_DateOut") = "" Then
            ''        strMessage = strMessage & "  There is no 'DateOut' date entered on the Customer Details page for this Sales order ! <br/>"
            ''    End If
            ''    If IsDBNull(dtTemp.Rows(0).Item("DeliverTo_DeliveryDate")) Or dtTemp.Rows(0).Item("DeliverTo_DeliveryDate") = "" Then
            ''        strMessage = strMessage & "  There is no 'Actual Delivery' date entered on the Customer Details page for this Sales order ! <br/>"
            ''    End If
            ''End If

            If strMessage = "" Then
                lngNewOrderNum = lngOrderNum

                dtNewOrder = objTelesales.GetOrderForNum(lngNewOrderNum)

                'Get the priority from the wfo_batchtable 
                dsOrderJobDetails = objPdn.GetWFOBatchTableData(lngNewOrderNum)

                strOrderComment = Trim(IIf(IsDBNull(dtNewOrder.Rows(0).Item("Comment")), "", dtNewOrder.Rows(0).Item("Comment")))
                strPO = Trim(IIf(IsDBNull(dtNewOrder.Rows(0).Item("CustomerPO")), "", dtNewOrder.Rows(0).Item("CustomerPO")))
                strRoute = Trim(IIf(IsDBNull(dtNewOrder.Rows(0).Item("Route")) = True, "", dtNewOrder.Rows(0).Item("Route")))
                strPriority = Trim(IIf(IsDBNull(dsOrderJobDetails.Tables(0).Rows(0).Item("SystemID")), "", dsOrderJobDetails.Tables(0).Rows(0).Item("SystemID")))
                dteDeliveryDate = dtNewOrder.Rows(0).Item("RequestedDeliveryDate")

                'Save the order extra data to the handheld
                objTelesales.SaveHandheldExtraData(lngNewOrderNum, strOrderComment, strRoute, dteDeliveryDate, strPriority)


                strOrderType = IIf(IsDBNull(dtNewOrder.Rows(0).Item("Type")), "", dtNewOrder.Rows(0).Item("Type"))

                Dim lnglocationId As Long

                ' Depending on type send to handheld
                'Don't send call off order immediately to handheld.
                If UCase(strOrderType) = "CALL-OFF" Then
                Else
                    'Get the order items 
                    dsOrderItems = objTelesales.GetOrderItems(lngOrderId)

                    For Each drRow As DataRow In dsOrderItems.Tables(0).Rows
                        lngSalesOrderItemId = drRow.Item("SalesOrderItemID")
                        intTraceCodeId = drRow.Item("TraceCodeId")

                        objTelesales.SendItemForPicking(lngOrderId, lngSalesOrderItemId, drRow.Item("QuantityRequested"), drRow.Item("WeightRequested"), intTraceCodeId)
                    Next
                End If
                'setting this boolean in case all of the items on this order are for backorder.
                'if they are then todays order should not be saved with no items in it. 

                'Set status to new
                objForms.SetJobStatusText(GetGlobalResourceObject("Resource", "Order_New"), lngNewOrderNum)
                objTasks.SetLastActivity(lngNewOrderNum, PortalFunctions.Now.Date)
                objTelesales.UpdateOrderStatus(lngOrderId, GetGlobalResourceObject("Resource", "Order_New"))
                Me.CurrentSession.VT_CurrentNewOrderStatus = GetGlobalResourceObject("Resource", "Order_New")
                ' hdnCurrentStatus.Value = Me.CurrentSession.VT_CurrentNewOrderStatus


                'audit log
                Dim strType As String
                strAuditComment = "Order has been Issued "
                strType = "Sales Order"
                objForms.WriteToAuditLog(lngOrderNum, strType, PortalFunctions.Now, strUserName, intUserId, strAuditComment, "Audit Record", "SysAdmin")

                'Set the buttons up again
                'SetUpUI()

                'save true value to this variable so that the status will update on the orders grid
                Me.CurrentSession.blnOrderStatusChanged = True
                ' hide the Issue button so we cannot issue it again
                '  btnIssue.Visible = False
                Me.CurrentSession.blnOrderStatusChanged = True

                strMessage = "Order has been Issued."
                'lblNewMsg.Text = strMessage
                'ModalPopupExtenderNewMessage.Show()

                If IsNothing(Me.CurrentSession.VT_OrdersDataTable) = False Then
                    Me.CurrentSession.VT_OrdersDataTable.Clear()
                End If

            Else
                'lblMessage.Text = strMessage
                'ModalPopupExtender6.Show()
            End If

        Else
            strMessage = "You must save the order before you can issue it."
            'lblMessage.Text = strMessage
            'ModalPopupExtender6.Show()
        End If


    End Sub
    Protected Sub btnPrintAcknowledgement_Click(sender As Object, e As ImageClickEventArgs) Handles btnPrintAcknowledgement.Click
        Dim objCommonFuncs As New BPADotNetCommonFunctions.VT_CommonFunctions.CommonFunctions
        Dim strTemplate As String = objCommonFuncs.GetConfigItem("CustAcknowledgementSteripackPDF")

        Dim strTemplatePath As String
        strTemplatePath = "FormTemplates\SalesOrders\" + strTemplate

        If System.IO.File.Exists(System.IO.Path.Combine(Server.MapPath("~"), strTemplatePath)) = False Then

            Exit Sub

        End If

        Session("_VT_PageToReturnToAfterDisplay") = "~/TabPages/AcknowledgeOrders.aspx"
        Session("_VT_DisplayDocPageMessage") = "Click the Back button to return to the Acknowledge Orders page"

        Response.Redirect("~\" + strTemplatePath)

    End Sub

    'Protected Sub wdgRepGrid_ActiveCellChanged(sender As Object, e As Infragistics.Web.UI.GridControls.ActiveCellEventArgs) Handles wdgRepGrid.ActiveCellChanged

    '    Dim objC As New VT_CommonFunctions.CommonFunctions

    '    Dim intActiveRowIndex As Integer = e.CurrentActiveCell.Row.Index

    '    '  wdgServiceItems.Behaviors.Activation.ActiveCell = wdgServiceItems.Rows(intActiveRowIndex).Items(1)

    '    Session("_VT_AcknowledgementRowSelected") = objC.SerialiseWebDataGridRow(wdgRepGrid, intActiveRowIndex)


    'End Sub

    Protected Sub btnExport_Click(sender As Object, e As ImageClickEventArgs) Handles btnExport.Click
        VerifyExporter.DownloadName = "Verify_Technologies_GridExport"
        VerifyExporter.Export(wdgRepGrid)
    End Sub

    Protected Sub wdgRepGrid_PreRender(sender As Object, e As EventArgs) Handles wdgRepGrid.PreRender
        If Session("OrdersAcknowledged") = "YES" Then
            Session("OrdersAcknowledged") = ""
            wdgRepGrid.Behaviors.Selection.SelectedRows.Clear()
            wdgRepGrid.Behaviors.Activation.ActiveCell = Nothing
        End If


    End Sub

    'Protected Sub wdgRepGrid_RowSelectionChanged(sender As Object, e As Infragistics.Web.UI.GridControls.SelectedRowEventArgs) Handles wdgRepGrid.RowSelectionChanged
    '    Dim objC As New VT_CommonFunctions.CommonFunctions

    '    Dim intActiveRowIndex As Integer = e.CurrentSelectedRows(0).Index


    '    '  wdgServiceItems.Behaviors.Activation.ActiveCell = wdgServiceItems.Rows(intActiveRowIndex).Items(1)

    '    Session("_VT_AcknowledgementRowSelected") = objC.SerialiseWebDataGridRow(wdgRepGrid, intActiveRowIndex)
    'End Sub
End Class



