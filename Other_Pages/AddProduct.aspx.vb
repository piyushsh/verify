Imports BPADotNetCommonFunctions
Imports System.Data.SqlClient
Imports System.Data

Imports VTDBFunctions.VTDBFunctions

Partial Class Other_Pages_AddProduct
    Inherits MyBasePage


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not IsPostBack Then
            lblMsg.Text = ""

            With ProductSelect1
                .ApplyPricingForCustomer = Session("_VT_CurrentCustomerId")

                .CalcQtyFromWeight = Me.CurrentSession.VT_CalcQtyFromWgt

                If Me.CurrentSession.VT_ProdSelectProdID > 0 Then
                    .PreSelectProductID = Me.CurrentSession.VT_ProdSelectProdID
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
        Dim dblTotalPrice As Double
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


        dblQuantity = ProductSelect1.Quantity
        dblWeight = ProductSelect1.Weight
        intUnitOfSale = objbpa.GetUnitOfSale(lngProductId)
        If intUnitOfSale = 1 Then  ' by unit
            If dblQuantity = 0 Then
                'objDisp.DisplayMessage(Me, "You must enter a quantity for this product before it can be added to the Order.")
                lblMsg.Text = "You must enter a quantity for this product before it can be added to the Order."

                Exit Sub
            End If
        Else
            If dblWeight = 0 Then
                lblMsg.Text = "You must enter a Weight for this product before it can be added to the Order."
                Exit Sub
            End If
        End If


        strLocationId = "0"

        dsProduct = objProducts.GetProductForId(lngProductId)
        intUnitOfSale = objProducts.GetUnitOfSale(lngProductId)

        ' Find available Qty or Weight

        dblInstock = ProductSelect1.QtyInStock

        dblUnitPrice = ProductSelect1.UnitPrice


        dblVATRate = ProductSelect1.VatPercent

        If intUnitOfSale = 1 Then
            dblVATAmount = (dblQuantity * dblUnitPrice) * (dblVATRate / 100)
            ' reformat VATRate for easier calculation
            dblVATRate = 1 + (dblVATRate / 100)
            dblExtendedPrice = dblQuantity * dblUnitPrice * dblVATRate
        Else
            dblVATAmount = (dblWeight * dblUnitPrice) * (dblVATRate / 100)
            ' reformat VATRate for easier calculation
            dblVATRate = 1 + (dblVATRate / 100)
            dblExtendedPrice = dblWeight * dblUnitPrice * dblVATRate
        End If
        dblTotalPrice = dblTotalPrice + dblExtendedPrice

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

        If ProductSelect1.TracecodeId > 0 Then
            strTraceCodeId = CStr(ProductSelect1.TracecodeId)
            strLocationId = CStr(ProductSelect1.LocationId)

        Else
            'if there is no trace code and location specified we should look for all available trace codes
            'for the selected product and choose the one with the oldest sell by date and qty in stock > qty ordered
            Dim strFindTraceCode As String = objCommonFuncs.GetConfigItem("IfNoTraceSelected")

            Dim strDisplay As String = objCommonFuncs.GetConfigItem("UseByOrBestBefore")
            Dim k As Integer

            If strFindTraceCode = "FindOldestBatch" Then


                If strDisplay = "SELLBY" Then
                    dsTraceCodes = objProducts.GetSortedTraceCodesForProduct(lngProductId, "SellByDate")

                Else
                    dsTraceCodes = objProducts.GetSortedTraceCodesForProduct(lngProductId, "UseByDate")

                End If
                'this dataset is ordered by usebydate so we just have to go through it until we find one with enought qty in stock
                Dim dsLocations As DataSet

                For j = 0 To dsTraceCodes.Tables(0).Rows.Count - 1
                    If intUnitOfSale = 1 Then
                        If dsTraceCodes.Tables(0).Rows(j).Item("NumInStores") >= dblQuantity Then
                            strTraceCodeId = CStr(dsTraceCodes.Tables(0).Rows(j).Item("TraceCodeId"))
                            dsLocations = objTrace.GetLocationData(dsTraceCodes.Tables(0).Rows(j).Item("TraceCodeId"), 0)
                            strLocationId = "0"
                            If dsLocations.Tables(0).Rows.Count > 0 Then
                                'loop through the locations table until we find a row with non 0 value for qty or weight
                                For k = 0 To dsLocations.Tables(0).Rows.Count - 1
                                    If intUnitOfSale = 1 Then
                                        If dsLocations.Tables(0).Rows(k).Item("Quantity") > 0 Then
                                            strLocationId = CStr(dsLocations.Tables(0).Rows(k).Item("LocationID"))
                                            Exit For
                                        End If
                                    Else

                                        If dsLocations.Tables(0).Rows(k).Item("Weight") > 0 Then
                                            strLocationId = CStr(dsLocations.Tables(0).Rows(k).Item("LocationID"))
                                            Exit For
                                        End If
                                    End If


                                Next

                            Else
                                strLocationId = "0"

                            End If

                            Exit For
                        End If
                    Else
                        If dsTraceCodes.Tables(0).Rows(j).Item("NumInStores") >= dblWeight Then
                            strTraceCodeId = CStr(dsTraceCodes.Tables(0).Rows(j).Item("TraceCodeId"))
                            dsLocations = objTrace.GetLocationData(dsTraceCodes.Tables(0).Rows(j).Item("TraceCodeId"), 0)
                            If dsLocations.Tables(0).Rows.Count > 0 Then
                                For k = 0 To dsLocations.Tables(0).Rows.Count
                                    If intUnitOfSale = 1 Then
                                        If dsLocations.Tables(0).Rows(k).Item("Quantity") > 0 Then
                                            strLocationId = CStr(dsLocations.Tables(0).Rows(k).Item("LocationID"))
                                            Exit For
                                        End If
                                    Else

                                        If dsLocations.Tables(0).Rows(k).Item("Weight") > 0 Then
                                            strLocationId = CStr(dsLocations.Tables(0).Rows(k).Item("LocationID"))
                                            Exit For
                                        End If
                                    End If


                                Next
                            Else
                                strLocationId = "0"

                            End If
                            Exit For

                        End If
                    End If

                Next
            Else
                strTraceCodeId = "0"
                strLocationId = "0"
            End If

            If strTraceCodeId = "" Then
                strTraceCodeId = "0"
                strLocationId = "0"
            End If

        End If

        If strTraceCodeId = "" Or strTraceCodeId = "0" Then
            strTraceCodeId = "0"
            strLocationId = "0"
        End If

        Dim strAllowbackOrders As String
        Dim objDB As New SalesOrdersFunctions.SalesOrders

        strAllowbackOrders = UCase(System.Configuration.ConfigurationSettings.AppSettings("AllowBackOrders"))

        If UCase(Session("VT_FromPage")) = "FULFILL_OPENING" Then
            'save the order item to the tcd_tblSalesOrder table, not the telesales table
            strTraceCode = ProductSelect1.TraceCodeDesc
            If strTraceCode = "N/A" Then
                strTraceCode = ""
            End If

            objDB.InsertFulfillSOToTrace(Me.CurrentSession.VT_OrderPO, System.DateTime.UtcNow, Me.CurrentSession.VT_CustomerID, 0, strProductCode, dblWeight, CLng(dblQuantity),
                       False, Me.CurrentSession.VT_SalesOrderNum, 0, 0, 6, "", Me.CurrentSession.VT_SalesOrderID, 1, strTraceCode, dblVATAmount, dblExtendedPrice - dblVATAmount, Me.CurrentSession.VT_DeliveryCustomerID, CLng(strLocationId), strChargedByType)

        Else
            'Don't send call off order immediately to handheld.
            If UCase(Me.CurrentSession.VT_OrderType) = "CALL-OFF" Then
                ObjTele.InsertOrderItem(Me.CurrentSession.VT_SalesOrderID, Me.CurrentSession.VT_SalesOrderNum, _
                                        lngProductId, dblQuantity, dblWeight, strTraceCodeId, dblUnitPrice, dblVATAmount, _
                                        dblExtendedPrice, Me.CurrentSession.VT_OrderPO, Me.CurrentSession.VT_CustomerID, strAllowbackOrders, _
                                        Me.CurrentSession.VT_DeliveryCustomerID, CLng(strLocationId), strChargedByType, _
                                         False, False)
            Else
                ObjTele.InsertOrderItem(Me.CurrentSession.VT_SalesOrderID, Me.CurrentSession.VT_SalesOrderNum, _
                                        lngProductId, dblQuantity, dblWeight, strTraceCodeId, dblUnitPrice, dblVATAmount, _
                                        dblExtendedPrice, Me.CurrentSession.VT_OrderPO, Me.CurrentSession.VT_CustomerID, strAllowbackOrders, _
                                        Me.CurrentSession.VT_DeliveryCustomerID, CLng(strLocationId), strChargedByType)
            End If


            objSO.UpdateOrderTotal(Me.CurrentSession.VT_SalesOrderID, Me.CurrentSession.VT_SalesOrderNum)

            Dim objCommon As New BPADotNetCommonFunctions.VT_Forms.Forms
            Dim strAuditLog As String

            strAuditLog = "New Item added to order, Product: " & ProductSelect1.ProductName & ", Qty: " & CStr(dblQuantity) & " , Wgt: " & CStr(dblWeight)

            objCommon.WriteToAuditLog(Me.CurrentSession.VT_JobID, "Telesales", System.DateTime.UtcNow, Session("_VT_CurrentUserName"),
                                        Session("_VT_CurrentUserId"), strAuditLog, "Audit Record", "", "ASP")

        End If
      


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
