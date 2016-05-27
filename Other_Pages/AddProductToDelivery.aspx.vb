Imports BPADotNetCommonFunctions
Imports System.Data.SqlClient
Imports System.Data

Imports VTDBFunctions.VTDBFunctions

Partial Class Other_Pages_AddProductToDelivery
    Inherits MyBasePage


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not IsPostBack Then
            lblMsg.Text = ""

            With ProductSelect1
                .ApplyPricingForCustomer = Me.CurrentSession.VT_CustomerID
                .BatchMode = True
                .BatchesOn = True
                .CalcQtyFromWeight = Me.CurrentSession.VT_CalcQtyFromWgt

                If Me.CurrentSession.VT_ProdSelectProdID > 0 Then
                    .PreSelectProductID = Me.CurrentSession.VT_ProdSelectProdID
                    .PreSelectunitprice = Me.CurrentSession.VT_SelectedUnitPrice
                End If


                '.CustomerName = "Abbot Ireland"
                ' get customer details
                Dim ds1 As DataSet
                Dim objDBFuncs As New VTDBFunctions.VTDBFunctions.CustomerFunctions
                ds1 = objDBFuncs.GetCustomerDetailsForId(CLng(Me.CurrentSession.VT_CustomerID))

                .IsVatExempt = CBool(ds1.Tables(0).Rows(0).Item("VATExempt"))
                .CustomerDiscount = ds1.Tables(0).Rows(0).Item("DiscountPercent")
                If UCase(System.Configuration.ConfigurationSettings.AppSettings("ShowBatchCodesInSelector")) = "YES" Then
                    .BatchMode = True
                Else
                    .BatchMode = False
                End If

                If UCase(System.Configuration.ConfigurationSettings.AppSettings("ShowProductCodesInSelector")) = "YES" Then
                    .ShowProductCodeWithName = True
                Else
                    .ShowProductCodeWithName = False
                End If


                .DBConnForInstallationDLL = Session("_VT_BPA.NetConnString")

            End With



            Dim i As Integer
            Dim ds As New DataSet
            Dim objBPA As New TelesalesFunctions
            Dim objProducts As New VTDBFunctions.VTDBFunctions.ProductsFunctions

            ' configure the ProductSelectControl

            With ProductSelect1
                .FillControl()
            End With
            If Me.CurrentSession.VT_ProdSelectProdID > 0 Then 'select this product in the selector

            End If
            Session("_VT_AllowBackOrder") = UCase(System.Configuration.ConfigurationSettings.AppSettings("AllowBackOrders"))

        End If

    End Sub




    Protected Sub btnAdd_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnAdd.Click
        Dim objbpa As New VTDBFunctions.VTDBFunctions.ProductsFunctions
        Dim lngProductId As Long
        Dim dblQuantity As Double
        Dim dblWeight As Double
        Dim dblNetPrice As Double
        Dim cell As New Infragistics.WebUI.UltraWebGrid.UltraGridCell
        Dim strTraceCode As String
        Dim dblUnitPrice As Double
        Dim row As New Infragistics.WebUI.UltraWebGrid.UltraGridRow
        Dim objDisp As New VT_Display.DisplayFuncs
        'Dim dblQty As Double
        Dim intUnitOfSale As Integer
        Dim objProducts As New VTDBFunctions.VTDBFunctions.ProductsFunctions
        Dim dsProduct As DataSet
        Dim dblInstock As Double
        Dim dblExtendedPrice As Double
        Dim dblVATRate As Double
        Dim dblVATAmount As Double
        Dim objForms As New VT_Forms.Forms
        Dim dsTraceCodes As DataSet
        Dim strTraceCodeId As String
        Dim strLocationId As String
        Dim objTrace As New TraceFunctions
        Dim ObjTele As New VTDBFunctions.VTDBFunctions.TelesalesFunctions
        Dim objTraceData As New VTDBFunctions.VTDBFunctions.TraceDataFunctions
        Dim objCommonFuncs As New BPADotNetCommonFunctions.VT_CommonFunctions.CommonFunctions
        Dim dsOrderItems As New DataSet
        Dim strQuery As String
        Dim drQueryRows() As DataRow
        Dim objSO As New SalesOrdersFunctions.SalesOrders
        Dim strProductCode As String
        Dim dsRowsToAdd As New DataSet
        Dim strSerialNum As String
        Dim strBarcode As String
        lblMsg.Text = ""


        lngProductId = ProductSelect1.ProductId

        strProductCode = ProductSelect1.ProductCode

        'Check that product is not already in order.
        dsOrderItems = ObjTele.GetOrderItems(Me.CurrentSession.VT_SalesOrderID)

        strQuery = " ProductID = " & CStr(lngProductId)
        drQueryRows = dsOrderItems.Tables(0).Select(strQuery)

        If UBound(drQueryRows) >= 0 Then
            If UBound(drQueryRows) > 0 Or drQueryRows(0).Item("ProductID") = lngProductId Then
                '  objDisp.DisplayMessage(Me, "You cannot add a product that is already in the order, edit the item in the order instead.")

                ' lblMsg.Text = "You cannot add a product that is already in the order, edit the item in the order instead."
                ' if the Ok button is clicked on the popup execution will continue at cmdOKConfirm_Click
                ' if the Cancel button is clicked nothing further will happen
                '    ModalPopupExtender1.Show()

                'Exit Sub
            End If
        End If


        '.Add("TraceCodeId", System.Type.GetType("System.Int64"))
        '.Add("TraceCode", System.Type.GetType("System.String"))
        '.Add("Location", System.Type.GetType("System.String"))
        '.Add("LocationId", System.Type.GetType("System.Int64"))
        '.Add("InStock", System.Type.GetType("System.Double"))
        '.Add("Weight", System.Type.GetType("System.Double"))
        '.Add("Quantity", System.Type.GetType("System.Double"))
        '.Add("UnitPrice", System.Type.GetType("System.Double"))
        '.Add("NetPrice", System.Type.GetType("System.Double"))
        '.Add("VAT", System.Type.GetType("System.Double"))
        '.Add("TotalPrice", System.Type.GetType("System.Double"))
        '.Add("SerialNum", System.Type.GetType("System.String"))
        '.Add("Barcode", System.Type.GetType("System.String"))


        dsRowsToAdd = ProductSelect1.GridRowData
        Dim blnDontAdd As Boolean


        For Each drRow As DataRow In dsRowsToAdd.Tables(0).Rows
            blnDontAdd = False

            dblQuantity = drRow.Item("Quantity")
            dblWeight = drRow.Item("Weight")
            intUnitOfSale = objbpa.GetUnitOfSale(lngProductId)
            If intUnitOfSale = 1 Then  ' by unit
                If dblQuantity = 0 Then
                    blnDontAdd = True
                End If
            Else
                If dblWeight = 0 Then
                    blnDontAdd = True
                End If
            End If

            If blnDontAdd = False Then
                strLocationId = "0"

                dsProduct = objProducts.GetProductForId(lngProductId)
                intUnitOfSale = objProducts.GetUnitOfSale(lngProductId)

                ' Find available Qty or Weight

                dblInstock = drRow.Item("InStock")

                dblUnitPrice = drRow.Item("UnitPrice")


                dblVATRate = drRow.Item("VAT")

                If intUnitOfSale = 1 Then
                    dblVATAmount = (dblQuantity * dblUnitPrice) * (dblVATRate / 100)
                    dblVATAmount = Math.Round(dblVATAmount, 2, System.MidpointRounding.AwayFromZero)
                    ' reformat VATRate for easier calculation
                    dblVATRate = 1 + (dblVATRate / 100)
                    dblExtendedPrice = dblQuantity * dblUnitPrice * dblVATRate
                    dblExtendedPrice = Math.Round(dblExtendedPrice, 2, System.MidpointRounding.AwayFromZero)
                Else
                    dblVATAmount = (dblWeight * dblUnitPrice) * (dblVATRate / 100)
                    dblVATAmount = Math.Round(dblVATAmount, 2, System.MidpointRounding.AwayFromZero)
                    ' reformat VATRate for easier calculation
                    dblVATRate = 1 + (dblVATRate / 100)
                    dblExtendedPrice = dblWeight * dblUnitPrice * dblVATRate
                    dblExtendedPrice = Math.Round(dblExtendedPrice, 2, System.MidpointRounding.AwayFromZero)
                End If

                dblNetPrice = drRow.Item("NetPrice")

                ' Check if there is an installation specific DLL and retrieve the ChargedBy Type
                ' for this product and customer
                Dim strCustomerDLL As String = System.Configuration.ConfigurationSettings.AppSettings("InstallationDLL")
                Dim strChargedByType As String
                If strCustomerDLL <> "" Then
                    Dim objCustDLL As TTIInstallationSpecific.Interface
                    objCustDLL = CreateObject(strCustomerDLL)
                    ' If this function doesn't return YES it implies that this Product is not for sale to this Customer
                    strChargedByType = objCustDLL.GetCustomerChargedByType(Session("_VT_BPA.NetConnString"), lngProductId, Me.CurrentSession.VT_CustomerID, System.DateTime.UtcNow.Date)
                Else
                    strChargedByType = "Use Default"
                End If

                ' Insert available qty or weight Order item 

                    strTraceCodeId = CStr(drRow.Item("TraceCodeId"))
                    strLocationId = CStr(drRow.Item("LocationId"))

                If strTraceCodeId = "" Or strTraceCodeId = "0" Then
                    strTraceCodeId = "0"
                    strLocationId = "0"
                End If

                Dim strAllowbackOrders As String
                Dim objDB As New SalesOrdersFunctions.SalesOrders

                strAllowbackOrders = UCase(System.Configuration.ConfigurationSettings.AppSettings("AllowBackOrders"))


                If Me.CurrentSession.VT_UserSerialNumbers = False Then
                    strSerialNum = ""
                    strBarcode = ""
                Else
                    strSerialNum = drRow.Item("SerialNum")
                    strBarcode = drRow.Item("Barcode")
                End If



                If UCase(Session("VT_FromPage")) = "FULFILL_OPENING" Then
                    'save the order item to the tcd_tblSalesOrder table, not the telesales table
                    strTraceCode = drRow.Item("TraceCode")
                    If strTraceCode = "N/A" Then
                        strTraceCode = ""
                    End If


                    objTraceData.InsertSOToTrace(Me.CurrentSession.VT_OrderPO, Me.CurrentSession.VT_OrderDate, Me.CurrentSession.VT_CustomerID, 0, strProductCode, 0, 0, _
                                 False, Me.CurrentSession.VT_SalesOrderNum, 0, 0, 6, strBarcode, 0, 1, strTraceCode, dblVATAmount, dblExtendedPrice - dblVATAmount, _
                                  Me.CurrentSession.VT_DeliveryCustomerID, CLng(strLocationId), strChargedByType, strSerialNum, strBarcode, CInt(dblQuantity), dblWeight)


                End If
            End If
        Next

        Select Case Session("VT_FromPage")
            Case "Details"
                Response.Redirect("~/TabPages/Details_Opening.aspx")
            Case "Planning"
                Response.Redirect("~/TabPages/Planning_Opening.aspx")
            Case "OrderList"
                Response.Redirect("~/TabPages/Orders_Opening.aspx")
            Case "Deliveries"
                Response.Redirect("~/TabPages/WarehouseManager.aspx")
            Case "Fulfill_Opening"
                Response.Redirect("~/TabPages/Fulfill_Opening.aspx")
            Case Else
                Response.Redirect("~/TabPages/Orders_Opening.aspx")

        End Select
    End Sub

    Protected Sub btnBack_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnBack.Click
        Select Case Session("VT_FromPage")
            Case "Details"
                Response.Redirect("~/TabPages/Details_Opening.aspx")
            Case "Planning"
                Response.Redirect("~/TabPages/Planning_Opening.aspx")
            Case "OrderList"
                Response.Redirect("~/TabPages/Orders_Opening.aspx")
            Case "Deliveries"
                Response.Redirect("~/TabPages/WarehouseManager.aspx")
            Case "Fulfill_Opening"
                Response.Redirect("~/TabPages/Fulfill_Opening.aspx")
            Case Else
                Response.Redirect("~/TabPages/Orders_Opening.aspx")

        End Select
    End Sub
End Class
