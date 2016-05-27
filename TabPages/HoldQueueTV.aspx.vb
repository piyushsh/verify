Imports BPADotNetCommonFunctions
Imports BPADotNetCommonFunctions.VT_CommonFunctions.MatrixFunctions
Imports System.Data.SqlClient
Imports System.Data
Imports System.Drawing
Imports System.Data.DataTableExtensions
Imports VTDBFunctions.VTDBFunctions


Partial Class TabPages_HoldQueueTV
    Inherits MyBasePage


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.wdgRepGrid.Behaviors.CreateBehavior(Of Infragistics.Web.UI.GridControls.Paging)()

        'Handle PageIndexChanged event 
        ' Me.wdgRepGrid.Behaviors.Paging.PagingClientEvents.PageIndexChanged = "WebDataGrid1_PageIndexChanged"


        Me.wdgRepGrid.Behaviors.Paging.PagerTemplate = New CustomPagerTemplate()

        cmdSaveComment.OnClientClick = String.Format("MessageUpdate('{0}','{1}')", cmdSaveComment.UniqueID, "")
        cmdSavePersonAssign.OnClientClick = String.Format("PersonAssignUpdate('{0}','{1}')", cmdSavePersonAssign.UniqueID, "")


        Dim objDataPreserve As New BPADotNetCommonFunctions.VT_DataPreserve.DataPreserve

        '  If Not IsPostBack Then
        FillData()

        'Else
        'wdgRepGrid.DataSource = objDataPreserve.GetWDGDataFromSession(wdgRepGrid)
        ''apply grid formatting
        'Dim dtShortOutput As DataTable = objDataPreserve.GetWDGDataFromSession(wdgRepGrid)
        'Dim intNumDays As Integer

        'lblNumOrders.Text = dtShortOutput.Rows.Count
        'Dim intzeroToThree As Integer
        'Dim intfourtofive As Integer
        'Dim intmorethanfive As Integer
        'Dim intMissingPart As Integer
        'Dim intMissingPrice As Integer
        'Dim intManualHold As Integer

        'For intLoop As Integer = 0 To wdgRepGrid.Rows.Count - 1
        '    If Trim(wdgRepGrid.Rows(intLoop).Items.FindItemByKey("DateOnHold").Text) <> "" Then
        '        intNumDays = wdgRepGrid.Rows(intLoop).Items.FindItemByKey("DateOnHold").Text

        '        If intNumDays >= 5 Then
        '            'Set the formatting on the grid - Set the Background colour
        '            wdgRepGrid.Rows(intLoop).CssClass = "Row_Salmon"
        '            ' intmorethanfive = intmorethanfive + 1
        '        ElseIf intNumDays >= 3 Then
        '            'Set the formatting on the grid - Set the Background colour
        '            wdgRepGrid.Rows(intLoop).CssClass = "Row_Yellow"
        '            '  intfourtofive = intfourtofive + 1
        '        Else
        '            ' intzeroToThree = intzeroToThree + 1
        '        End If

        '    End If
        'Next

        'For intLoop As Integer = 0 To dtShortOutput.Rows.Count - 1
        '    If Trim(dtShortOutput.Rows(intLoop).Item("DateOnHold")) <> "" Then
        '        intNumDays = dtShortOutput.Rows(intLoop).Item("DateOnHold")

        '        If intNumDays >= 5 Then
        '            'Set the formatting on the grid - Set the Background colour
        '            intmorethanfive = intmorethanfive + 1
        '        ElseIf intNumDays >= 3 Then
        '            'Set the formatting on the grid - Set the Background colour
        '            intfourtofive = intfourtofive + 1
        '        Else
        '            intzeroToThree = intzeroToThree + 1
        '        End If

        '    End If

        '    If IsDBNull(dtShortOutput.Rows(intLoop).Item("ReasonOnHold")) = False Then
        '        If InStr(UCase(dtShortOutput.Rows(intLoop).Item("ReasonOnHold")), "NO PRODUCT") > 0 Then
        '            intMissingPart = intMissingPart + 1
        '        ElseIf InStr(UCase(dtShortOutput.Rows(intLoop).Item("ReasonOnHold")), "PRICE DIFFERENCE") > 0 Then
        '            intMissingPrice = intMissingPrice + 1

        '        Else
        '            intManualHold = intManualHold + 1
        '        End If

        '    End If

        'Next
        ''set the labels for num days on hold
        'lblfourtofive.Text = intfourtofive
        'lblMoreThanFive.Text = intmorethanfive
        'lblzerotothree.Text = intzeroToThree
        'lblManualHold.Text = intManualHold
        'lblMissingPart.Text = intMissingPart
        'lblMissingPrice.Text = intMissingPrice
        'End If

        'SmcN 02/06/2014 Set the Grid width to the Client Browser width
        If Not Me.CurrentSession.VT_BrowserWindowWidth Is Nothing Then
            wdgRepGrid.Width = Me.CurrentSession.VT_BrowserWindowWidth - 54
        End If

    End Sub

    Public Function GetOrderDataForStatus(ByVal strconn As String, ByVal strStatus As String) As DataTable

        'This function returns All sales Order numbers and status for a given status

        Dim strsql As String = ""
        Dim dt As New DataTable

        strsql = strsql & "SELECT SalesOrderNum, Status, DateCreated, RequestedDeliveryDate, TotalValue"
        strsql = strsql & " from tls_SalesOrders "
        strsql = strsql & strStatus

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

        strStatusSearch = " WHERE tls_SalesOrders.Status = '" & GetGlobalResourceObject("Resource", "Order_OnHold") & "'"
        strStatusSearch = strStatusSearch & " OR tls_SalesOrders.Status = '" & GetGlobalResourceObject("Resource", "Order_OnHold_System") & "'"
        strStatusSearch = strStatusSearch & " OR tls_SalesOrders.Status = '" & GetGlobalResourceObject("Resource", "Order_OnHold_Manual") & "'"

        dt = GetOrderDataForStatus(Session("_VT_DotNetConnString"), strStatusSearch)

        lblNumOrders.Text = "0"

        If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
            'get data from grids
            dtoutput = objSales.GetMatrixOrderDataForOrderNum(dt, strConn, Me.CurrentSession.VT_tlsNumFields)
            If dtoutput IsNot Nothing AndAlso dtoutput.Rows.Count > 0 Then
                'Clean up the DataTable columns
                Dim astrColNames(3, 1) As String
                astrColNames(0, 0) = "Item_DateOut"
                astrColNames(0, 1) = "CONVERT_TO_DATE"
                astrColNames(1, 0) = "DateCreated"
                astrColNames(1, 1) = "CONVERT_TO_DATE"
                astrColNames(2, 0) = "Item_DateRequested"
                astrColNames(2, 1) = "CONVERT_TO_DATE"
                astrColNames(3, 0) = "Item_DateArrival"
                astrColNames(3, 1) = "CONVERT_TO_DATE"

                dtoutput = objG.CleanColumnFormats(dtoutput, astrColNames)

                'Calculate the price difference column values
                If dtoutput.Columns.Contains("UnitDifference") = False Then
                    dtoutput.Columns.Add("UnitDifference")
                End If

                For intCnt = 0 To dtoutput.Rows.Count - 1
                    dtoutput.Rows(intCnt).Item("UnitDifference") = IIf(dtoutput.Rows(intCnt).Item("PO_UnitPrice") <> "", dtoutput.Rows(intCnt).Item("PO_UnitPrice"), 0) - dtoutput.Rows(intCnt).Item("UnitPrice")
                Next

                'Add in the extra data items to this datatable These items are read from the Details page in the matrix     SmcN 06/05/2014
                'first we need to fill this grid with the relevant MatrixLinkId's
                Dim strWhereQualifier As String = " JobStatusText = '" & GetGlobalResourceObject("Resource", "Order_OnHold_System") & "' OR JobStatusText = '" & GetGlobalResourceObject("Resource", "Order_OnHold_Manual") & "'"
                strerr = objG.GetBlockOfMatrixIdsFromBatchTable(strConn, dtoutput, "SalesOrderNum", "Sales Order", strWhereQualifier)

                Dim astrItems(0) As String
                astrItems(0) = "ReasonOnHold"

                Dim strFormName As String = "Details"
                Dim strKeyField As String = "MatrixLinkId"

                strerr = objM.GetBlockOfMatrixDataItems(strConn, strFormName, VTMatrixFunctions.Sales, astrItems, dtoutput, strKeyField)
                'at this point all data for all items are in the datatable
                'we need to copy this datatable structure then copy only one line per order, it should be the line that includes a comment if a comment exists for any line
                '18.07.2014 AM

                'need to get the date on hold from the matrix
                astrItems(0) = "DateOnHold"
                strFormName = "Details"
                strKeyField = "MatrixLinkId"
                strerr = objM.GetBlockOfMatrixDataItems(strConn, strFormName, VTMatrixFunctions.Sales, astrItems, dtoutput, strKeyField)


                Dim dtShortOutput As DataTable = dtoutput.Clone
                Dim strlastorderNum As String

                For i = 0 To dtoutput.Rows.Count - 1
                    If strlastorderNum <> dtoutput.Rows(i).Item("SO_ContiguousNum") Then
                        'this is a new order, save the first line
                        dtShortOutput.Rows.Add(dtoutput.Rows(i).ItemArray)
                        If IsDBNull(dtShortOutput.Rows(dtShortOutput.Rows.Count - 1).Item("DateOnHold")) = False AndAlso dtShortOutput.Rows(dtShortOutput.Rows.Count - 1).Item("DateOnHold") <> "" Then
                            dtShortOutput.Rows(dtShortOutput.Rows.Count - 1).Item("DateOnHold") = DateDiff(DateInterval.Day, dtShortOutput.Rows(dtShortOutput.Rows.Count - 1).Item("DateOnHold"), PortalFunctions.Now.Date)
                        Else
                            dtShortOutput.Rows(dtShortOutput.Rows.Count - 1).Item("DateOnHold") = ""
                        End If
                    Else
                        'check if there is a comment, if so then save the comment to the row for this order
                        If IsDBNull(dtoutput.Rows(i).Item("Item_OnHoldTVComment")) = False Then
                            If Trim(dtoutput.Rows(i).Item("Item_OnHoldTVComment")) <> "" Then
                                dtShortOutput.Rows(dtShortOutput.Rows.Count - 1).Item("Item_OnHoldTVComment") = dtoutput.Rows(i).Item("Item_OnHoldTVComment")
                            End If

                        End If
                    End If
                    strlastorderNum = dtoutput.Rows(i).Item("SO_ContiguousNum")
                Next

                Dim astrColNames2(0, 1) As String
                astrColNames2(0, 0) = "DateOnHold"
                astrColNames2(0, 1) = "CONVERT_TO_INT"

                dtShortOutput = objG.CleanColumnFormats(dtShortOutput, astrColNames2)
                dtShortOutput = objG.SortDataTable("DateOnHold DESC", dtShortOutput)

                objDataPreserve.BindDataToWDG(dtShortOutput, wdgRepGrid)

                Dim intNumDays As Integer
                Dim intZeroToThree As Integer
                Dim intFourToFive As Integer
                Dim intMoreThanFive As Integer
                Dim intMissingPart As Integer
                Dim intMissingPrice As Integer
                Dim intManualHold As Integer

                lblNumOrders.Text = dtShortOutput.Rows.Count
                'Apply Grid Formatting here
                For intLoop As Integer = 0 To wdgRepGrid.Rows.Count - 1
                    If Trim(wdgRepGrid.Rows(intLoop).Items.FindItemByKey("DateOnHold").Text) <> "" Then
                        intNumDays = wdgRepGrid.Rows(intLoop).Items.FindItemByKey("DateOnHold").Text

                        If intNumDays >= 5 Then
                            'Set the formatting on the grid - Set the Background colour
                            wdgRepGrid.Rows(intLoop).CssClass = "Row_Salmon"
                            ' intmorethanfive = intmorethanfive + 1
                        ElseIf intNumDays >= 3 Then
                            'Set the formatting on the grid - Set the Background colour
                            wdgRepGrid.Rows(intLoop).CssClass = "Row_Yellow"
                            '  intfourtofive = intfourtofive + 1
                        Else
                            ' intzeroToThree = intzeroToThree + 1
                        End If

                    End If
                Next

                Dim strPreviousStatus As String
                Dim objForms As New VT_Forms.Forms
                Dim objTasks As New VT_eQOInterface.eQOInterface
                Dim objTele As New VTDBFunctions.VTDBFunctions.TelesalesFunctions
                Dim lngOrderNum As Long
                Dim lngOrderId As Long
                Dim dtOrderGrid As New DataTable
                Dim objWF As New WorkFlowFunctionsForSales.WorkFlowFunctions_Sales
                Dim dtdetails As New DataTable
                ReDim astrItems(0)
                Dim objcommon As New VTDBFunctions.VTTraceDBAccess.VTTraceDBAccess

                Dim IsMalaysia As Boolean
                If UCase(Trim(objcommon.GetConfigItem("InstallationCompany") = "STERIPACKMALAYSIA")) Then
                    IsMalaysia = True

                Else
                    IsMalaysia = False

                End If



                For intLoop As Integer = 0 To dtShortOutput.Rows.Count - 1
                    If Trim(dtShortOutput.Rows(intLoop).Item("DateOnHold")) <> "" Then
                        intNumDays = dtShortOutput.Rows(intLoop).Item("DateOnHold")

                        If intNumDays >= 5 Then
                            'Set the formatting on the grid - Set the Background colour
                            intMoreThanFive = intMoreThanFive + 1
                        ElseIf intNumDays >= 3 Then
                            'Set the formatting on the grid - Set the Background colour
                            intFourToFive = intFourToFive + 1
                        Else
                            intZeroToThree = intZeroToThree + 1
                        End If

                    End If
                    If IsDBNull(dtShortOutput.Rows(intLoop).Item("ReasonOnHold")) = False Then
                        If InStr(UCase(dtShortOutput.Rows(intLoop).Item("ReasonOnHold")), "NO PRODUCT") > 0 Then
                            intMissingPart = intMissingPart + 1
                            'need to check here if the part number has been added yet
                            '***********************************************************


                            lngOrderNum = dtShortOutput.Rows(0).Item("SalesOrderNum")


                            dtOrderGrid = objSales.GetSalesOrderItemsFromMatrix("OrderItems", CInt(Me.CurrentSession.VT_SalesOrderNum), strConn, Me.CurrentSession.VT_tlsNumFields)
                            If Not IsNothing(dtOrderGrid) AndAlso dtOrderGrid.Rows.Count > 0 Then
                                'Run Validation Checks on the Product items
                                'Note: this will also set the Order Status
                                'First Reset the Order Status to ensure that Validation runs through correctly for this scenario
                                objWF.SteripackOrderItemsValidationChecks(dtOrderGrid)

                                If Session("HoldDataToSave") = "YES" Then
                                    'Rebind the 'wdgOrderItems' Grid again here to ensure that and additions are updated on the grid
                                    objDataPreserve.BindDataToWDG(dtOrderGrid, wdgOrderItems)

                                    'Save Order Items to the MATRIX table
                                    objSales.SaveSalesItemsToMatrix("OrderItems", wdgOrderItems, CInt(Me.CurrentSession.VT_SalesOrderNum), strConn, Me.CurrentSession.VT_tlsNumFields)
                                End If

                            End If


                            '***********************************************************************

                        ElseIf InStr(UCase(dtShortOutput.Rows(intLoop).Item("ReasonOnHold")), "PRICE DIFFERENCE") > 0 Then
                            intMissingPrice = intMissingPrice + 1

                            If IsMalaysia Then
                                lngOrderNum = dtShortOutput.Rows(0).Item("SalesOrderNum")

                                'need to check if a price has been added to mfgpro. If this is Malaysia then a live lookup to mfgpro is used for prices
                                dtOrderGrid = objSales.GetSalesOrderItemsFromMatrix("OrderItems", CInt(Me.CurrentSession.VT_SalesOrderNum), strConn, Me.CurrentSession.VT_tlsNumFields)
                                If Not IsNothing(dtOrderGrid) AndAlso dtOrderGrid.Rows.Count > 0 Then
                                    'Run Validation Checks on the Product items
                                    'Note: this will also set the Order Status
                                    'First Reset the Order Status to ensure that Validation runs through correctly for this scenario
                                    objWF.SteripackOrderItemsValidationChecks(dtOrderGrid)

                                    If Session("HoldDataToSave") = "YES" Then
                                        'Rebind the 'wdgOrderItems' Grid again here to ensure that and additions are updated on the grid
                                        objDataPreserve.BindDataToWDG(dtOrderGrid, wdgOrderItems)

                                        'Save Order Items to the MATRIX table
                                        objSales.SaveSalesItemsToMatrix("OrderItems", wdgOrderItems, CInt(Me.CurrentSession.VT_SalesOrderNum), strConn, Me.CurrentSession.VT_tlsNumFields)
                                    End If

                                End If
                            End If

                        Else

                            intManualHold = intManualHold + 1
                            End If

                    End If


                Next
                lblNumItems.Text = dtShortOutput.Rows.Count
                'set the labels for num days on hold
                lblfourtofive.Text = intFourToFive
                lblMoreThanFive.Text = intMoreThanFive
                lblzerotothree.Text = intZeroToThree
                lblManualHold.Text = intManualHold
                lblMissingPart.Text = intMissingPart
                lblMissingPrice.Text = intMissingPrice
            Else
                InitialiseAndBindEmptyOrderItemsGrid()
            End If

            'if the checkbox to show customer details is checked then show or hide the appropriate columns in the grid
            If Session("_VT_ShowCustomerDetailsInGrid") = "TRUE" Then
                With wdgRepGrid
                    .Columns("SoldTo_Name").Hidden = False
                    .Columns("SoldTo_ContactName").Hidden = False

                End With
            Else
                With wdgRepGrid
                    .Columns("SoldTo_Name").Hidden = True
                    .Columns("SoldTo_ContactName").Hidden = True

                End With
            End If
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

        Response.Redirect("~/TabPages/HoldQueueManager.aspx")

    End Sub



    Protected Sub wdgRepGrid_ActiveCellChanged(sender As Object, e As Infragistics.Web.UI.GridControls.ActiveCellEventArgs) Handles wdgRepGrid.ActiveCellChanged

        Dim objC As New VT_CommonFunctions.CommonFunctions

        Dim intActiveRowIndex As Integer = e.CurrentActiveCell.Row.Index

        '  wdgServiceItems.Behaviors.Activation.ActiveCell = wdgServiceItems.Rows(intActiveRowIndex).Items(1)

        Session("_VT_OnHOldTVRowSelected") = objC.SerialiseWebDataGridRow(wdgRepGrid, intActiveRowIndex)


    End Sub

    Public Function PortalRootNode() As String
        ' retrieve the Root node of the current portal
        ' we will use it to create a consistent path to the image files
        Dim strCompletePath As String = Server.MapPath("~")

        'ServerRootPath = strCompletePath
        PortalRootNode = Mid(strCompletePath, InStrRev(strCompletePath, "\") + 1)

    End Function





    Private Class CustomPagerTemplate
        Implements ITemplate
#Region "ITemplate Members"

        Public Sub InstantiateIn(ByVal container As Control) Implements ITemplate.InstantiateIn
            'Dim prevButton As New HtmlButton()
            'prevButton.ID = "PrevButton"
            'prevButton.InnerText = "Prev"
            'prevButton.Attributes("onclick") = "return PreviousPage()"


            Dim nextButton As New HtmlButton()
            nextButton.InnerText = "Next"
            nextButton.Attributes("onclick") = "return NextPage()"

            'container.Controls.Add(prevButton)
            'container.Controls.Add(ddl)
            container.Controls.Add(nextButton)
        End Sub

#End Region
    End Class



    Protected Sub wdgRepGrid_PageIndexChanged(sender As Object, e As Infragistics.Web.UI.GridControls.PagingEventArgs) Handles wdgRepGrid.PageIndexChanged
        'add colour coding to the next page
        Dim i As Integer
        Dim intNumDays As Integer

        For i = 0 To wdgRepGrid.Rows.Count - 1
            If Trim(wdgRepGrid.Rows(i).Items.FindItemByKey("DateOnHold").Text) <> "" Then
                intNumDays = wdgRepGrid.Rows(i).Items.FindItemByKey("DateOnHold").Text
                Select Case intNumDays
                    Case Is >= 5
                        wdgRepGrid.Rows(i).CssClass = "Row_Salmon"
                    Case Is >= 3
                        wdgRepGrid.Rows(i).CssClass = "Row_Yellow"
             
                End Select
            End If

        Next

     
    End Sub




End Class



