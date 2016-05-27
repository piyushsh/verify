Imports BPADotNetCommonFunctions
Imports System.Data.SqlClient
Imports System.Data

Imports VTDBFunctions.VTDBFunctions

Partial Class Other_Pages_AddProductToQuote
    Inherits MyBasePage


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not IsPostBack Then
            lblMsg.Text = ""

            With ProductSelect1
                .ApplyPricingForCustomer = Session("_VT_CurrentCustomerId")
                .BatchMode = True
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
        Dim ObjQuote As New VTDBFunctions.VTDBFunctions.QuoteFunctions
        Dim objTraceData As New VTDBFunctions.VTDBFunctions.TraceDataFunctions
        Dim objCommonFuncs As New BPADotNetCommonFunctions.VT_CommonFunctions.CommonFunctions
        Dim dsOrderItems As New DataSet
        Dim strQuery As String
        Dim drQueryRows() As DataRow
        Dim objSO As New SalesOrdersFunctions.SalesOrders
        Dim strProductCode As String
        Dim dsRowsToAdd As New DataSet
        lblMsg.Text = ""


        lngProductId = ProductSelect1.ProductId

        strProductCode = ProductSelect1.ProductCode

        'Check that product is not already in order.
        '   dsOrderItems = ObjTele.GetOrderItems(Me.CurrentSession.VT_SalesOrderID)

        strQuery = " ProductID = " & CStr(lngProductId)
        drQueryRows = dsOrderItems.Tables(0).Select(strQuery)

        'If UBound(drQueryRows) >= 0 Then
        '    If UBound(drQueryRows) > 0 Or drQueryRows(0).Item("ProductID") = lngProductId Then
        '        '  objDisp.DisplayMessage(Me, "You cannot add a product that is already in the order, edit the item in the order instead.")

        '        ' lblMsg.Text = "You cannot add a product that is already in the order, edit the item in the order instead."
        '        ' if the Ok button is clicked on the popup execution will continue at cmdOKConfirm_Click
        '        ' if the Cancel button is clicked nothing further will happen
        '        '    ModalPopupExtender1.Show()

        '        'Exit Sub
        '    End If
        'End If


        dsRowsToAdd = ProductSelect1.GridRowData

        dblQuantity = ProductSelect1.Quantity
        dblWeight = ProductSelect1.Weight
        intUnitOfSale = objbpa.GetUnitOfSale(lngProductId)
        If intUnitOfSale = 1 Then  ' by unit
            If dblQuantity = 0 Then
                'objDisp.DisplayMessage(Me, "You must enter a quantity for this product before it can be added to the Order.")
                lblMsg.Text = "You must enter a quantity for this product before it can be added to the Quote."

                Exit Sub
            End If
        Else
            If dblWeight = 0 Then
                lblMsg.Text = "You must enter a Weight for this product before it can be added to the Quote."
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

        Dim strChargedByType As String
        strChargedByType = "Use Default"




        ObjQuote.InsertQuoteItem(Me.CurrentSession.VT_QuoteID, lngProductId, dblQuantity, dblWeight, "", dblUnitPrice, dblVATAmount, dblTotalPrice, strChargedByType, ProductSelect1.VatPercent)


     



            objSO.UpdateOrderTotal(Me.CurrentSession.VT_SalesOrderID, Me.CurrentSession.VT_SalesOrderNum)

            'Dim objCommon As New BPADotNetCommonFunctions.VT_Forms.Forms
            'Dim strAuditLog As String

            'strAuditLog = "New Item added to order, Product: " & ProductSelect1.ProductName & ", Qty: " & CStr(dblQuantity) & " , Wgt: " & CStr(dblWeight)

            'objCommon.WriteToAuditLog(Me.CurrentSession.VT_JobID, "Telesales", Now, Session("_VT_CurrentUserName"), _
            '                            Session("_VT_CurrentUserId"), strAuditLog, "Audit Record", "", "ASP")





        Select Case Session("VT_FromPage")
            Case "QuoteProducts"
                Response.Redirect("~/Quotes/QuoteProducts.aspx")
            Case Else
                Response.Redirect("~/Quotes/QuoteProducts.aspx")

        End Select
    End Sub

    Protected Sub btnBack_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnBack.Click
      Select Case Session("VT_FromPage")
            Case "QuoteProducts"
                Response.Redirect("~/Quotes/QuoteProducts.aspx")
            Case Else
                Response.Redirect("~/Quotes/QuoteProducts.aspx")

        End Select
    End Sub
End Class
