Imports BPADotNetCommonFunctions
Imports System.Data.SqlClient
Imports System.Data

Imports VTDBFunctions.VTDBFunctions

Partial Class Other_Pages_ProductStockDetails
    Inherits MyBasePage


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim objDataPreserve As New BPADotNetCommonFunctions.VT_DataPreserve.DataPreserve

        If Not IsPostBack Then

            Dim objProds As New VTDBFunctions.VTDBFunctions.ProductsFunctions
            Dim ds As New DataSet
            Dim dsAddData As New DataSet
            Dim drRow() As DataRow
            Dim dblAllocated As Double
            Dim dblInStock As Double
            Dim dblOnOrder As Double

            Dim objCommonFuncs As New BPADotNetCommonFunctions.VT_CommonFunctions.CommonFunctions
            Dim strTempDecimal As String = objCommonFuncs.GetConfigItem("TelesalesDecimalPlaces")
            If strTempDecimal = "" OrElse IsNumeric(strTempDecimal) = False Then
                Me.CurrentSession.VT_DecimalPlaces = 4
            Else
                If CInt(strTempDecimal) < 3 Then
                    Me.CurrentSession.VT_DecimalPlaces = 2
                Else
                    Me.CurrentSession.VT_DecimalPlaces = CInt(strTempDecimal)
                End If
            End If



            ds = objProds.GetProductForId(Me.CurrentSession.VT_ProdSelectProdID)

            dblAllocated = IIf(IsDBNull(ds.Tables(0).Rows(0).Item("Allocated")), 0, ds.Tables(0).Rows(0).Item("Allocated"))
            dblInStock = IIf(IsDBNull(ds.Tables(0).Rows(0).Item("InStock")), 0, ds.Tables(0).Rows(0).Item("InStock"))
            dblOnOrder = IIf(IsDBNull(ds.Tables(0).Rows(0).Item("OnOrder")), 0, ds.Tables(0).Rows(0).Item("OnOrder"))

            dblAllocated = Math.Round(dblAllocated, Me.CurrentSession.VT_DecimalPlaces)
            dblInStock = Math.Round(dblInStock, Me.CurrentSession.VT_DecimalPlaces)
            dblOnOrder = Math.Round(dblOnOrder, Me.CurrentSession.VT_DecimalPlaces)

            lblAllocated.Text = CStr(dblAllocated)
            lblInStock.Text = CStr(dblInStock)
            lblOnOrder.Text = CStr(dblOnOrder)


            lblProductCode.Text = IIf(IsDBNull(ds.Tables(0).Rows(0).Item("Catalog_Number")), "", ds.Tables(0).Rows(0).Item("Catalog_Number"))
            lblProductName.Text = IIf(IsDBNull(ds.Tables(0).Rows(0).Item("Product_Name")), "", ds.Tables(0).Rows(0).Item("Product_Name"))

            lblAvailable.Text = CDbl(lblInStock.Text) - CDbl(lblAllocated.Text)


            dsAddData = objProds.GetProductAdditionalData(Me.CurrentSession.VT_ProdSelectProdID)
            If dsAddData.Tables(0).Rows.Count > 0 Then
                drRow = dsAddData.Tables(0).Select("DataItemName = 'Detail'")
                If drRow.GetUpperBound(0) > -1 Then
                    lblProductDetail.Text = IIf(IsDBNull(drRow(0).Item("DataItemValue")), "", drRow(0).Item("DataItemValue"))
                End If
            End If

            FillOrdersGrid()

            Dim strSerialEnabled As String = objCommonFuncs.GetConfigItem("SerialNums")

            If UCase(strSerialEnabled) = "YES" Then
                FillBatchesSerialGridNew()
                wdgBatchSummary.Visible = False
                chkShowNegative.Visible = True
            Else
                FillBatchesGrid()
                wdgBatchSerialDetails.Visible = False
                chkShowNegative.Visible = False
            End If

        Else
            'this is a postback so rebind the grids
            wdgBatchSerialDetails.DataSource = objDataPreserve.GetWDGDataFromSession(wdgBatchSerialDetails)
            wdgBatchSummary.DataSource = objDataPreserve.GetWDGDataFromSession(wdgBatchSummary)
            wdgSalesOrders.DataSource = objDataPreserve.GetWDGDataFromSession(wdgSalesOrders)

        End If

    End Sub


    Private Sub FillOrdersGrid()
        Dim objDataPreserve As New BPADotNetCommonFunctions.VT_DataPreserve.DataPreserve
        Dim dt As New Data.DataTable
        Dim objfuncs As New SalesOrdersFunctions.SalesOrders

        dt = objfuncs.getAllOpenOrdersForProduct(PortalFunctions.Now.Date, PortalFunctions.Now.Date, Me.CurrentSession.VT_ProdSelectProdID, True)

        'Bind the Grid
        objDataPreserve.BindDataToWDG(dt, wdgSalesOrders)

    End Sub


    Private Sub FillBatchesGrid()

        Dim ds As New Data.DataSet
        Dim objfuncs As New ProductsFunctions.Products
        Dim objProd As New TraceFunctions
        Dim objDataPreserve As New BPADotNetCommonFunctions.VT_DataPreserve.DataPreserve
        Dim dtTemp As New DataTable

        ds = objfuncs.GetTraceCodesLocationData(Me.CurrentSession.VT_ProdSelectProdID)

        For Each drRow As DataRow In ds.Tables(0).Rows
            If (IsDBNull(drRow.Item("Quantity")) = False AndAlso drRow.Item("Quantity") = 0) And (IsDBNull(drRow.Item("Weight")) = False AndAlso drRow.Item("Weight") = 0) Then
                drRow.Delete()
            End If
        Next
        ds.AcceptChanges()

        dtTemp = ds.Tables(0)

        'Add colums if necessary
        If dtTemp.Columns.Contains("TraceCodeDesc") Then
            'do nothing
        Else
            'Add the columns
            dtTemp.Columns.Add("TraceCodeDesc")
            dtTemp.Columns.Add("Location")

        End If

        For Each drRow As DataRow In dtTemp.Rows
            drRow.Item("TraceCodeDesc") = objProd.GetTracecodeDescForId(drRow.Item("TraceCodeID"))
            drRow.Item("Location") = objProd.GetLocationTextForId(drRow.Item("LocationId"))
        Next

        'Bind the Grid
        objDataPreserve.BindDataToWDG(dtTemp, wdgBatchSummary)


    End Sub

    Private Sub FillBatchesSerialGrid()

        Dim ds As New Data.DataSet
        Dim objfuncs As New ProductsFunctions.Products
        Dim objProd As New TraceFunctions
        Dim dblCurrentQty As Double
        Dim dblCurrentWgt As Double
        Dim objDataPreserve As New BPADotNetCommonFunctions.VT_DataPreserve.DataPreserve
        Dim dtTemp As New DataTable

        ds = objfuncs.GetSerialNumberDataForProduct(Me.CurrentSession.VT_ProdSelectProdID)

        For Each drRow As DataRow In ds.Tables(0).Rows

            dblCurrentQty = objfuncs.GetCurrentQtyWgtForSerialNum(drRow.Item("SerialNum"), drRow.Item("TraceCodeId"), drRow.Item("LocationId"), 1)
            dblCurrentWgt = objfuncs.GetCurrentQtyWgtForSerialNum(drRow.Item("SerialNum"), drRow.Item("TraceCodeId"), drRow.Item("LocationId"), 0)
            drRow.Item("Qty") = dblCurrentQty
            drRow.Item("Wgt") = dblCurrentWgt

            If (IsDBNull(drRow.Item("Qty")) = False AndAlso drRow.Item("Qty") = 0) And (IsDBNull(drRow.Item("Wgt")) = False AndAlso drRow.Item("Wgt") = 0) Then
                drRow.Delete()
            End If
        Next
        ds.AcceptChanges()

        dtTemp = ds.Tables(0)

        'Add colums if necessary
        If dtTemp.Columns.Contains("TraceCodeDesc") Then
            'do nothing
        Else
            'Add the columns
            dtTemp.Columns.Add("TraceCodeDesc")
            dtTemp.Columns.Add("Location")

        End If

        For Each drRow As DataRow In dtTemp.Rows
            '     uwgRow.Cells.FromKey("TraceCodeDesc").Value = objProd.GetTracecodeDescForId(uwgRow.Cells.FromKey("TraceCodeDesc").Value)
            drRow.Item("Location") = objProd.GetLocationTextForId(drRow.Item("LocationID"))

        Next

        'Bind the Grid
        objDataPreserve.BindDataToWDG(dtTemp, wdgBatchSerialDetails)

    End Sub



    Private Sub FillBatchesSerialGridNew()

        Dim ds As New Data.DataSet
        Dim objfuncs As New ProductsFunctions.Products
        Dim objProd As New TraceFunctions
        Dim dblCurrentQty As Double
        Dim dblCurrentWgt As Double
        Dim lngLocationID As Long
        Dim dsSerialNumDetails As New DataSet
        Dim objSerial As New VTDBFunctions.VTDBFunctions.SerialNumFunctions
        Dim intRowToUse As Integer
        Dim strCurrentSerialNum As String
        Dim lngTempLatestLocationID As Long
        Dim j As Integer
        Dim dblTotalWeightEmptySerial As Double
        Dim dblBatchInStock As Double
        Dim objTrace As New VTDBFunctions.VTDBFunctions.TraceDataFunctions
        Dim dsTraceCode As New Data.DataSet

        Dim objDataPreserve As New BPADotNetCommonFunctions.VT_DataPreserve.DataPreserve
        Dim dtTemp As New DataTable

        Try
            ds = objfuncs.GetSerialNumberDataForProduct(Me.CurrentSession.VT_ProdSelectProdID)


            For Each drRow As DataRow In ds.Tables(0).Rows
                dblTotalWeightEmptySerial = 0
                strCurrentSerialNum = IIf(IsDBNull(drRow.Item("SerialNum")), "", drRow.Item("SerialNum"))

                dsSerialNumDetails = objSerial.GetDetailsForSerialNumAndTraceId(strCurrentSerialNum, drRow.Item("TraceCodeId"))

                dsTraceCode = objTrace.GetBatchDetailsForId(drRow.Item("TraceCodeId"))
                If ds.Tables(0).Rows.Count > 0 Then
                    dblBatchInStock = dsTraceCode.Tables(0).Rows(0).Item("NumInStores")
                Else
                    dblBatchInStock = 0
                End If

                intRowToUse = 0

                If strCurrentSerialNum <> "" Then
                    lngTempLatestLocationID = objSerial.GetMostRecentLocationIdForSerial(strCurrentSerialNum, drRow.Item("TraceCodeId"))

                    For j = 0 To dsSerialNumDetails.Tables(0).Rows.Count - 1

                        If lngTempLatestLocationID = dsSerialNumDetails.Tables(0).Rows(j).Item("LocationId") Then
                            intRowToUse = j
                        End If

                    Next
                Else
                    For j = 0 To dsSerialNumDetails.Tables(0).Rows.Count - 1

                        If drRow.Item("LocationId") = dsSerialNumDetails.Tables(0).Rows(j).Item("LocationId") Then
                            intRowToUse = j
                        End If

                        dblTotalWeightEmptySerial = dblTotalWeightEmptySerial + dsSerialNumDetails.Tables(0).Rows(j).Item("Weight")
                    Next
                End If

                dblCurrentQty = dsSerialNumDetails.Tables(0).Rows(intRowToUse).Item("Quantity")
                dblCurrentWgt = dsSerialNumDetails.Tables(0).Rows(intRowToUse).Item("Weight")
                lngLocationID = dsSerialNumDetails.Tables(0).Rows(intRowToUse).Item("LocationId")
                dblCurrentWgt = Math.Round(dblCurrentWgt, Me.CurrentSession.VT_DecimalPlaces)
                drRow.Item("Qty") = dblCurrentQty
                drRow.Item("Wgt") = dblCurrentWgt

                If chkShowNegative.Checked = True Then
                    If (IsDBNull(drRow.Item("Qty")) = False AndAlso drRow.Item("Qty") = 0) And (IsDBNull(drRow.Item("Wgt")) = False AndAlso drRow.Item("Wgt") < 0.001) Then
                        drRow.Delete()
                    ElseIf strCurrentSerialNum <> "" And lngTempLatestLocationID <> drRow.Item("LocationId") Then
                        drRow.Delete()
                    Else
                        drRow.Item("locationID") = lngLocationID
                    End If
                Else
                    If dblBatchInStock < 0.04 Then
                        drRow.Delete()
                    Else
                        If strCurrentSerialNum <> "" Then
                            If (IsDBNull(drRow.Item("Qty")) = False AndAlso drRow.Item("Qty") < 1) And (IsDBNull(drRow.Item("Wgt")) = False AndAlso drRow.Item("Wgt") < 0.001) Then
                                drRow.Delete()
                            ElseIf strCurrentSerialNum <> "" And lngTempLatestLocationID <> drRow.Item("LocationId") Then
                                drRow.Delete()
                            Else
                                drRow.Item("locationID") = lngLocationID
                            End If

                        End If

                        If strCurrentSerialNum = "" And dblTotalWeightEmptySerial < 0.001 Then
                            drRow.Delete()
                        End If
                    End If
                End If


            Next
            ds.AcceptChanges()

            'Add colums if necessary
            If ds.Tables(0).Columns.Contains("TraceCodeDesc") Then
                'do nothing
            Else
                'Add the columns
                ds.Tables(0).Columns.Add("TraceCodeDesc")
                ds.Tables(0).Columns.Add("Location")

            End If

            For Each drRow As DataRow In ds.Tables(0).Rows
                '     uwgRow.Cells.FromKey("TraceCodeDesc").Value = objProd.GetTracecodeDescForId(uwgRow.Cells.FromKey("TraceCodeDesc").Value)
                drRow.Item("Location") = objProd.GetLocationTextForId(drRow.Item("LocationID"))
            Next
        Catch
        End Try

        'Bind the Grid
        objDataPreserve.BindDataToWDG(ds.Tables(0), wdgBatchSerialDetails)

    End Sub


    Protected Sub btnBack_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnBack.Click
        Select Case Session("VT_FromPage")
            Case "Details"
                Response.Redirect("~/TabPages/Details_Opening.aspx")
            Case "Planning"
                Response.Redirect("~/TabPages/Planning_Opening.aspx")
            Case "OrderList"
                Response.Redirect("~/TabPages/Orders_Opening_New.aspx")
            Case "Deliveries"
                Response.Redirect("~/TabPages/WarehouseManager.aspx")
            Case "Fulfill_Opening"
                Session("CameFromProductStockPage") = True
                Response.Redirect("~/TabPages/Fulfill_Opening.aspx")
            Case Else
                Response.Redirect("~/TabPages/Orders_Opening_New.aspx")

        End Select
    End Sub

    Protected Sub chkShowNegative_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkShowNegative.CheckedChanged
        FillBatchesSerialGridNew()
    End Sub
End Class
