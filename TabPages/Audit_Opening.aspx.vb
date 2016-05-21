Imports System.Data.SqlClient
Imports System.Data
Imports System.Drawing

Partial Class TabPages_Audit_Opening
    Inherits MyBasePage



    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not IsPostBack Then
            LoadHeaderInfo()
            FillAuditGrid(Me.CurrentSession.VT_JobID)

        End If

    End Sub


    Protected Sub LoadHeaderInfo()
        Dim lngSalesOrderId As Long
        Dim dsOrder As New DataSet
        Dim objBPA As New VTDBFunctions.VTDBFunctions.TelesalesFunctions
        Dim lngBillingCustomerID As Long
        Dim lngDeliveryCustomerID As Long
        Dim dsCustomer As New DataSet
        Dim objCust As New VTDBFunctions.VTDBFunctions.CustomerFunctions

        lngSalesOrderId = Me.CurrentSession.VT_SalesOrderID
        dsOrder = objBPA.GetOrderForId(lngSalesOrderId)
        lblComment.Text = IIf(IsDBNull(dsOrder.Tables(0).Rows(0).Item("Comment")), "", dsOrder.Tables(0).Rows(0).Item("Comment"))
        lblOrderDate.Text = IIf(IsDBNull(dsOrder.Tables(0).Rows(0).Item("DateCreated")), "", dsOrder.Tables(0).Rows(0).Item("DateCreated"))
        lblRequestedDeliveryDate.Text = IIf(IsDBNull(dsOrder.Tables(0).Rows(0).Item("RequestedDeliveryDate")), "", dsOrder.Tables(0).Rows(0).Item("RequestedDeliveryDate"))
        lblPONumber.Text = IIf(IsDBNull(dsOrder.Tables(0).Rows(0).Item("CustomerPO")), "", dsOrder.Tables(0).Rows(0).Item("CustomerPO"))
        lblorderStatus.Text = IIf(IsDBNull(dsOrder.Tables(0).Rows(0).Item("Status")), "", dsOrder.Tables(0).Rows(0).Item("Status"))

        lblPriority.Text = Me.CurrentSession.VT_OrderPriority

        If Me.CurrentSession.VT_OrderTypesEnabled = "YES" Then
            If IsDBNull(dsOrder.Tables(0).Rows(0).Item("Type")) = True OrElse dsOrder.Tables(0).Rows(0).Item("Type") = "" Then
                lblOrderType.Text = "Standard"
            Else
                lblOrderType.Text = dsOrder.Tables(0).Rows(0).Item("Type")
            End If
        Else
            lblOrderType.Visible = False
            lblOrderTypeLabel.Visible = False
        End If


        'need to get the customer details using the ID from this dataset
        lngBillingCustomerID = dsOrder.Tables(0).Rows(0).Item("CustomerId")
        lngDeliveryCustomerID = dsOrder.Tables(0).Rows(0).Item("DeliveryCustomerId")
        If lngBillingCustomerID = lngDeliveryCustomerID Then
            dsCustomer = objCust.GetCustomerDetailsForId(lngBillingCustomerID)
            If dsCustomer.Tables(0).Rows.Count > 0 Then
                lblCustomerName.Text = Trim(dsCustomer.Tables(0).Rows(0).Item("CustomerName"))
                lblDeliveryCustomer.Text = Trim(dsCustomer.Tables(0).Rows(0).Item("CustomerName"))
                If IsDBNull(dsCustomer.Tables(0).Rows(0).Item("DeliveryAddress")) OrElse dsCustomer.Tables(0).Rows(0).Item("DeliveryAddress") = "" Then
                    lblCustAddress.Text = Trim(IIf(IsDBNull(dsCustomer.Tables(0).Rows(0).Item("BillingAddress")), "", dsCustomer.Tables(0).Rows(0).Item("BillingAddress")))
                Else
                    lblCustAddress.Text = Trim(IIf(IsDBNull(dsCustomer.Tables(0).Rows(0).Item("DeliveryAddress")), "", dsCustomer.Tables(0).Rows(0).Item("DeliveryAddress")))
                End If
            Else
                lblCustAddress.Text = ""
                lblCustomerName.Text = ""
                lblDeliveryCustomer.Text = ""
            End If
        Else
            lblCustomerName.Text = Trim(objCust.GetCustomerNameForId(lngBillingCustomerID))
            dsCustomer = objCust.GetCustomerDetailsForId(lngDeliveryCustomerID)
            If dsCustomer.Tables(0).Rows.Count > 0 Then
                lblDeliveryCustomer.Text = Trim(dsCustomer.Tables(0).Rows(0).Item("CustomerName"))
                lblCustAddress.Text = Trim(IIf(IsDBNull(dsCustomer.Tables(0).Rows(0).Item("BillingAddress")), "", dsCustomer.Tables(0).Rows(0).Item("BillingAddress")))
            Else
                lblCustAddress.Text = ""
                lblCustomerName.Text = ""
            End If

        End If


        lblWO.Text = "Order: " & CStr(Me.CurrentSession.VT_JobID)


    End Sub

    Protected Sub FillAuditGrid(ByRef lngJobID As Long)

        Dim objDB As New SalesOrdersFunctions.SalesOrders
        Dim dt As New Datatable

        dt = objDB.GetAuditTrailForOrder(lngJobID)


        For Each drRow As DataRow In dt.Rows
            If UCase(Left(drRow.Item("SourceDescription"), 5)) = "HACCP" Then
                drRow.Delete()
            End If
        Next
        dt.AcceptChanges()
      
        wdgAudit.DataSource = dt
        wdgAudit.DataBind()

     

    End Sub


   
    Protected Sub BtnBack_Click(sender As Object, e As ImageClickEventArgs) Handles BtnBack.Click
        If Me.CurrentSession.OptionsPageToReturnTo <> "" Then
            Response.Redirect(Me.CurrentSession.OptionsPageToReturnTo)
        End If
    End Sub
End Class
