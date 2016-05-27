Imports BPADotNetCommonFunctions
Imports System.Data.SqlClient
Imports System.Data
Imports System.IO
Imports VTDBFunctions.VTDBFunctions
Imports BPADotNetCommonFunctions.VT_CommonFunctions.MatrixFunctions
Imports System.Web.UI.HtmlControls

Partial Class Other_Pages_SkidLabelManagement_Food


    Inherits MyBasePage

    Const cView = 0
    Const cReference = 1
    Const cName = 2

    Protected Sub Page_Init(sender As Object, e As System.EventArgs) Handles Me.Init
        Dim css As HtmlGenericControl
        css = New HtmlGenericControl
        css.TagName = "style"
        css.Attributes.Add("type", "text/css")
        css.InnerHtml = "@import ""../Verify_Infragistics.css"";"
        Page.Header.Controls.Add(css)

    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim objDataPreserve As New BPADotNetCommonFunctions.VT_DataPreserve.DataPreserve

        'Put user code to initialize the page here
        If Not IsPostBack Then

            lblProductName_NS.Text = Session("ProductName")


        Else
            'This is a postback so rebind the grid
            wdgSkidinfo.DataSource = objDataPreserve.GetWDGDataFromSession(wdgSkidinfo)
        End If

    End Sub

    Protected Sub btnBack_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnBack.Click

        Dim strReturnPage As String

        strReturnPage = Trim(Session("VT_ReturnPage"))


        Response.Redirect(strReturnPage)

    End Sub

    Function getSkidInformation(ByVal strSkidNumber As String) As DataTable
        Dim objDataPreserve As New BPADotNetCommonFunctions.VT_DataPreserve.DataPreserve
        Dim objP As New ProductionFunctions.Production
        Dim dtSkidInfo As DataTable
        Dim blnBind As Boolean = False
        'get all skid information 
        dtSkidInfo = objP.getSkidNumberParentFolders(strSkidNumber)

        'check if the product matches the item to be fulfilled
        Dim dt As New DataTable
        Dim objDb As New VTDBFunctions.VTDBFunctions.DBFunctions
        If dtSkidInfo.Rows.Count > 0 Then
            dt = objDb.ExecuteSQLReturnDT("Select * from wfo_BatchTable where JobIdDesc = '" & dtSkidInfo.Rows(0).Item("ProductionJobId") & "'")
            If dt.Rows.Count > 0 Then
                If dt.Rows(0).Item("ProductID") <> Session("ProductID") Then
                    'show a message, the skid contains the wrong product
                    blnBind = False
                Else
                    blnBind = True
                End If
            End If
        Else
            blnBind = True
        End If

        If blnBind Then
            objDataPreserve.BindDataToWDG(dtSkidInfo, wdgSkidinfo)
        End If

    End Function

    Protected Sub btnSearchForSkid_Click(sender As Object, e As ImageClickEventArgs) Handles btnSearchForSkid.Click
        If txtSkidNumber.Text <> "" Then
            getSkidInformation(txtSkidNumber.Text)
        Else
            Dim objDisp As New VT_Display.DisplayFuncs
            objDisp.DisplayMessage(Page, "You must enter a skid number")
        End If
    End Sub
    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles hdnSkidNumber.ValueChanged
        txtSkidNumber.Text = hdnSkidNumber.Value
        getSkidInformation(hdnSkidNumber.Value)

    End Sub

    Protected Sub btnSelectAll_Click(sender As Object, e As ImageClickEventArgs) Handles btnSelectAll.Click
        Dim selectedRows As Infragistics.Web.UI.GridControls.SelectedRowCollection = wdgSkidinfo.Behaviors.Selection.SelectedRows

        Dim TheNewGrid As Infragistics.Web.UI.GridControls.WebDataGrid = wdgSkidinfo
        Dim wgdRow As Infragistics.Web.UI.GridControls.GridRecord
        For Each wgdRow In TheNewGrid.Rows
            selectedRows.Add(wgdRow)
        Next wgdRow
    End Sub

    Protected Sub btnAddSelected_Click(sender As Object, e As ImageClickEventArgs) Handles btnAddSelected.Click
        'sort the selected rows by trace code
        Dim dt As New DataTable
        Dim blnTCAlreadyExists As Boolean = False
        Dim dtOrderItemRecord As New DataTable
        Dim objDB As New SalesOrdersFunctions.SalesOrders

        dtOrderItemRecord = objDB.GetRecordsForIds(Me.CurrentSession.VT_SelectedOrderRecordId)
        If dtOrderItemRecord.Rows.Count > 0 Then
            If IsDBNull(dtOrderItemRecord.Rows(0).Item("TraceCode")) = False Then
                If Trim(dtOrderItemRecord.Rows(0).Item("TraceCode")) <> "" Then
                    blnTCAlreadyExists = True
                End If
            End If
        End If

        dt.Columns.Add("TraceCode")
        Dim wgdRow As Infragistics.Web.UI.GridControls.GridRecord
        For Each wgdRow In wdgSkidinfo.Behaviors.Selection.SelectedRows
            dt.Rows.Add()

            dt.Rows(dt.Rows.Count - 1).Item("TraceCode") = wgdRow.Items.FindItemByKey("ProductionJobId").Value
        Next
        Dim dv As New DataView
        Dim sortedDT As New DataTable

        dv = dt.DefaultView
        dv.Sort = "TraceCode desc"
        sortedDT = dv.ToTable()

        Dim i As Integer
        Dim strTraceCode As String
        Dim intProduct As Integer = Session("ProductID")
        Dim strLastTraceCode As String

        Dim dblqtyWeight As Double


        For i = 0 To sortedDT.Rows.Count - 1
            dblqtyWeight = dblqtyWeight + 1
            strTraceCode = sortedDT.Rows(i).Item("TraceCode")
            If strLastTraceCode <> strTraceCode Then
                If i > 0 Then
                    'fulfill with this trace code now and reset qty
                    If blnTCAlreadyExists = False Then
                        FulfillwithTC(strTraceCode, dblqtyWeight)
                    Else
                        blnTCAlreadyExists = True
                        AddSubsequentTC(strTraceCode, dblqtyWeight)
                    End If


                    dblqtyWeight = 0
                End If
                strLastTraceCode = strTraceCode
            End If
        Next
        If sortedDT.Rows.Count > 0 Then
            'fulfill with the last trace code now
            If blnTCAlreadyExists = False Then
                FulfillwithTC(strTraceCode, dblqtyWeight)
            Else
                blnTCAlreadyExists = True
                AddSubsequentTC(strTraceCode, dblqtyWeight)
            End If
        End If

        Dim strReturnPage As String

        strReturnPage = Trim(Session("VT_ReturnPage"))

        Response.Redirect(strReturnPage)

    End Sub
    Sub FulfillwithTC(strtraceCode As String, dblqtyWeight As Double)
        Dim lngRecordId As Long
        Dim dblFulfillQty As Double
        Dim dblFulfillWgt As Double

        Dim lngLocationId As Long
        Dim strSerial As String
        Dim strBarcode As String
        Dim strPriceSoldFor As String
        Dim strVAT As String
        Dim dblQtyORWeightComplete As String
        Dim intProduct As Integer = Session("ProductID")
        Dim intUnitOfSale As Integer
        Dim objProd As New VTDBFunctions.VTDBFunctions.ProductsFunctions

        intUnitOfSale = objProd.GetUnitOfSale(intProduct)

        Dim objDB As New SalesOrdersFunctions.SalesOrders
        Dim objCommonFuncs As New BPADotNetCommonFunctions.VT_CommonFunctions.CommonFunctions
        Dim strSerialNums As String = objCommonFuncs.GetConfigItem("SerialNums")

        'If Not uwgDetails.DisplayLayout.ActiveRow Is Nothing Then
        '    lngRecordId = CDbl(IIf(IsNumeric(uwgDetails.DisplayLayout.ActiveRow.Cells.FromKey("RecordId").GetText), uwgDetails.DisplayLayout.ActiveRow.Cells.FromKey("RecordId").GetText, 0))
        lngRecordId = Me.CurrentSession.VT_SelectedOrderRecordId


        ' lngLocationId = ddlLocationEdit.SelectedValue

        strSerial = ""

        'If the transaction sell by type is different from the product track by 

        dblQtyORWeightComplete = dblqtyWeight

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
                    If intUnitOfSale = 1 Then 'unit pricing
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

                If intUnitOfSale = 1 Then 'unit pricing
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
        If intUnitOfSale = 1 Then
            dblFulfillQty = dblQtyORWeightComplete
        Else
            dblFulfillWgt = dblQtyORWeightComplete
        End If
        Dim objtracedata As New VTDBFunctions.VTDBFunctions.TraceDataFunctions
        objDB.SaveOrderItemChanges(lngRecordId, strtraceCode, dblFulfillQty, dblFulfillWgt, lngLocationId, strPriceSoldFor, strVAT, 1, strSerial, strBarcode)

        Dim strTeleDebugLog As String = objCommonFuncs.GetConfigItem("TelesalesDebugLog")
        If UCase(strTeleDebugLog) = "ON" Then
            Dim strLog As String
            Dim f As New BPADotNetCommonFunctions.VT_CommonFunctions.CommonFunctions
            strLog = "update batch:  Order: " & dtOrderItemRecord.Rows(0).Item("OrderNumber") & " UnitPrice: " & CStr(dblUnitPrice) & " Amount: " & CStr(dblQtyORWeightComplete) & " Total: " & strPriceSoldFor & " Batch Code: " & strtraceCode & " ProductCode:" & Trim(dtOrderItemRecord.Rows(0).Item("ProductId")) & " OrderItemId:" & CStr(lngOrderItemId)
            f.DebugLog(0, strLog, "Sales Portal", PortalFunctions.Now)
        End If



    End Sub
    Sub AddSubsequentTC(strTraceCode As String, dblQtyWgt As Double)
        Dim objtracedata As New VTDBFunctions.VTDBFunctions.TraceDataFunctions
        Dim strProductCode As String
        Dim dblWeight As Double = 0
        Dim dblQty As Double = 0

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

        Dim objProd As New VTDBFunctions.VTDBFunctions.ProductsFunctions

        strProductCode = objProd.GetProductCode(Session("ProductID"))


        lngRecordId = Me.CurrentSession.VT_SelectedOrderRecordId



        ' lngLocationId = ddlNewBatchLocationEdit.SelectedValue



        strSerial = ""
        strBarcode = ""


        dblQtyORWeightComplete = dblQtyWgt

        Dim intProduct As Integer = Session("ProductID")
        Dim intUnitOfSale As Integer

        intUnitOfSale = objProd.GetUnitOfSale(intProduct)


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
                        If intUnitOfSale = 1 Then 'unit pricing
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

                    If intUnitOfSale = 1 Then 'unit pricing
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
        If intUnitOfSale = 1 Then
            dblQty = dblQtyORWeightComplete
        Else
            dblWeight = dblQtyORWeightComplete
        End If

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


    End Sub
End Class
