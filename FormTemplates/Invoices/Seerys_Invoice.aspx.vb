Imports System.Data.SqlClient
Imports System.Data
Imports System.Configuration
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Drawing
Imports System.Web.UI.HtmlControls
Imports AjaxControlToolkit
Imports BPADotNetCommonFunctions
Imports BPADotNetCommonFunctions.VT_CommonFunctions.MatrixFunctions

Partial Class SeerysInvoiceTemplate

    Inherits MyBasePage


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim objS As New VerifyIntellisense.Sales
        Dim objD As New VerifyIntellisense.SalesDispatch
        Dim SalesOrder1 As New VerifyIntellisense.Sales.SalesOrder
        'Dim DocketItems1() As VerifyIntellisense.SalesDispatch.SalesDispatchItem
        Dim dtRep As New DataTable
        Dim dtReturns As New DataTable

        Dim strConn As String = Session("_VT_DotNetConnString")
        Dim objR As New BPADotNetCommonFunctions.VT_ReportFunctions.TransactionDataFunctions
        Dim objC As New VTDBFunctions.VTDBFunctions.CustomerFunctions
        Dim strWhereClause As String


        'Get Dispatch Transactions

        strWhereClause = " WHERE trc_transactions.DocketNum = '" & Me.CurrentSession.VT_DocketNumber & "' "
        'strWhereClause = " WHERE trc_transactions.DocketNum = '20130423142642' "
        strWhereClause = strWhereClause + " AND (trc_Transactions.TransactionType = '3' OR trc_Transactions.TransactionType = '6') "
        strWhereClause = strWhereClause + " AND (trc_Transactions.PaymentType <> '2') "

        dtRep = objR.GetTransactionsForWhereClause(strWhereClause, strConn)


        'Calculate subtotal and tax
        Dim rowCount As Integer
        Dim subtotal As Double = 0.0
        Dim tax As Double = 0.0
        If dtRep.Rows.Count > 0 Then
            For rowCount = 0 To dtRep.Rows.Count - 1
                subtotal = subtotal + If(IsDBNull(dtRep.Rows(rowCount).Item("TotalPrice")), 0, dtRep.Rows(rowCount).Item("TotalPrice"))
                tax = tax + If(IsDBNull(dtRep.Rows(rowCount).Item("VATCharged")), 0, dtRep.Rows(rowCount).Item("VATCharged")) * subtotal
            Next
        End If

        'Add Item No column
        dtRep.Columns.Add("Item_No", GetType(Integer))


        'Clean up the DataTable columns
        Dim astrColNames(0, 1) As String
        astrColNames(0, 0) = "Item_No"
        astrColNames(0, 1) = "ADD_INCREMENTAL_INDEX"

        Dim objG As New BPADotNetCommonFunctions.VT_ReportFunctions.GeneralFunctions
        If dtRep IsNot Nothing AndAlso dtRep.Rows.Count > 0 Then
            dtRep = objG.CleanColumnFormats(dtRep, astrColNames)
        End If

        lblSubTotal.Text = subtotal.ToString
        lblTax.Text = tax.ToString



        wdgRepGrid.DataSource = dtRep
        wdgRepGrid.DataBind()

        Session("dtRep") = wdgRepGrid.DataSource
        Session("BadBind") = "NO"


        'Get Return Transactions
        strWhereClause = " WHERE trc_transactions.DocketNum = '" & Me.CurrentSession.VT_DocketNumber & "' "
        'strWhereClause = " WHERE trc_transactions.DocketNum = '20130423142642' "
        strWhereClause = strWhereClause + " AND (trc_Transactions.TransactionType = '3' OR trc_Transactions.TransactionType = '6') "
        strWhereClause = strWhereClause + " AND (trc_Transactions.PaymentType = '2') "

        dtReturns = objR.GetTransactionsForWhereClause(strWhereClause, strConn)

        'Calculate returns total plus tax
        Dim returns As Double = 0.0
        Dim returnsTax As Double = 0.0
        If dtReturns.Rows.Count > 0 Then
            For rowCount = 0 To dtReturns.Rows.Count - 1
                returns = returns + dtReturns.Rows(rowCount).Item("TotalPrice")
                returnsTax = returnsTax + dtReturns.Rows(rowCount).Item("VATCharged")
            Next
        End If

        lblReturns.Text = (returns + returnsTax).ToString


        If dtReturns.Rows.Count > 0 Then

            'there are returns so display them
            pnlReturns.Visible = True
            wdgRepReturns.DataSource = dtReturns
            wdgRepReturns.DataBind()

            Session("dtReturns") = wdgRepReturns.DataSource
            Session("BadBind") = "NO"
        Else
            pnlReturns.Visible = False
        End If



        'RunReport = True
        Dim intSalesOrderID As Integer = 0
        If dtRep.Rows.Count > 0 Then
            intSalesOrderID = dtRep.Rows(0).Item("SalesOrderNum")
        Else
            If dtReturns.Rows.Count > 0 Then
                intSalesOrderID = dtReturns.Rows(0).Item("SalesOrderNum")
            End If
        End If

        If intSalesOrderID > 0 Then
            Dim objCommonFuncs As New BPADotNetCommonFunctions.VT_CommonFunctions.CommonFunctions

            lblInvoiceNum.Text = If(IsDBNull(dtRep.Rows(0).Item("InvoiceNum")), "", dtRep.Rows(0).Item("InvoiceNum"))
            If lblInvoiceNum.Text <> "" Then
                lblInvoiceNum.Text = objCommonFuncs.GetConfigItem("InvoiceNumberPrefix") + lblInvoiceNum.Text
            End If

            lblInvoiceDate.Text = Format(dtRep.Rows(0).Item("DateOfTransaction"), "D")
            lblShipDate.Text = Format(dtRep.Rows(0).Item("DateOfTransaction"), "D")
            lblDueDate.Text = Format(dtRep.Rows(0).Item("DateOfTransaction"), "D")

            Dim objRep As New VT_ReportFunctions.SalesDataFunctions
            Dim dtSalesOrder As DataTable = objRep.GetSalesOrderDetails(strConn, dtRep.Rows(0).Item("SalesOrderNum"))
            Dim dsCustDetails As DataSet = objC.GetCustomerDetailsForId(dtSalesOrder.Rows(0).Item("CustomerId"))
            Dim dtCustDetails As DataTable = dsCustDetails.Tables(0)
            ' SalesOrder1 = objS.SalesOrderDetails(intSalesOrderID)

            Dim discount As Double = dtCustDetails.Rows(0).Item("DiscountPercent")



            discount = (discount * subtotal) / 100
            lblDiscount.Text = "-" & Math.Round(discount, 2).ToString("F2")
            If Val(lblDiscount.Text) <= 0 Then
                lblDiscount.Visible = False
                lblCaptionDiscount.Visible = False
            End If


            'Calculate Grand Total
            Dim grandTotal As Double = 0.0
            grandTotal = grandTotal + subtotal - discount + returns - tax
            '   lblGrandTotal.Text = Math.Round(grandTotal, 2).ToString("F2")
            lblGrandTotal.Text = Math.Round(grandTotal, 2).ToString("F2")


            Dim ci As Globalization.CultureInfo
            ci = Globalization.CultureInfo.CurrentCulture
            lblCurrency.Text = ci.NumberFormat.CurrencySymbol

            lblFreight.Text = ""



            If Not IsDBNull(dtCustDetails.Rows(0).Item("PaymentTerms")) Then
                lblTerms.Text = dtCustDetails.Rows(0).Item("PaymentTerms")
            End If
            If Not IsDBNull(dtCustDetails.Rows(0).Item("Route")) Then
                lblShipVia.Text = dtCustDetails.Rows(0).Item("Route")
            End If
            If Not IsDBNull(dtSalesOrder.Rows(0).Item("PersonLoggingName")) Then
                lblSalesPerson.Text = dtSalesOrder.Rows(0).Item("PersonLoggingName")
            End If

            'The relevent Logo should be stored on the path specified in wfo_config, where DataItemName = TeleSalesInvoiceLogoPath
            'get logo path
            Dim strImg_Name As String = objCommonFuncs.GetConfigItem("TeleSalesInvoiceLogoPath")
            Image1.ImageUrl = strImg_Name

            'The relevent footer should be stored on the path specified in wfo_config, where DataItemName = TeleSalesInvoiceFooterPath
            'Otherwise a hard-coded default footer will be used
            'get footer text path
            Dim strFooterPath As String = objCommonFuncs.GetConfigItem("TeleSalesInvoiceFooterPath")

            'read footer text file
            Dim TextLine As String = ""
            If strFooterPath <> "" AndAlso System.IO.File.Exists(strFooterPath) Then
                TextLine = ""
                Dim objReader As New System.IO.StreamReader(strFooterPath)
                TextLine = TextLine & objReader.ReadToEnd
                lblFooter.Text = TextLine
            End If



            lblRequiredDate.Text = Format(dtSalesOrder.Rows(0).Item("RequestedDeliveryDate"), "D")
            lblBillingCustomer.Text = dtSalesOrder.Rows(0).Item("CustomerName")
            lblBillAddress.Text = dtSalesOrder.Rows(0).Item("BillingAddress")
            lblBillPhone.Text = dtSalesOrder.Rows(0).Item("BillingPhone")
            lblBillFax.Text = dtSalesOrder.Rows(0).Item("BillingFax")

            lblDeliveryCustomer.Text = dtSalesOrder.Rows(0).Item("DeliveryCustomerName")
            lblShipAddress.Text = dtSalesOrder.Rows(0).Item("DeliveryCustomerAddress")
            lblShipPhone.Text = dtSalesOrder.Rows(0).Item("DeliveryCustomerPhone")
            lblShipFax.Text = dtSalesOrder.Rows(0).Item("DeliveryCustomerFax")

            lblCustID.Text = dtSalesOrder.Rows(0).Item("CustomerId")
            lblCustomerPO.Text = dtSalesOrder.Rows(0).Item("CustomerPO")
            lblComment.Text = dtSalesOrder.Rows(0).Item("Comment")
            lblOrderDate.Text = Format(dtSalesOrder.Rows(0).Item("DateCreated"), "D")



        End If


    End Sub
End Class
