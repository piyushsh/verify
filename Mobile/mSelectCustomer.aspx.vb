Imports BPADotNetCommonFunctions
Imports BPADotNetCommonFunctions.VT_CommonFunctions.MatrixFunctions
Imports BPADotNetCommonFunctions.SalesFunctions.Sales_Orders

Imports System.Data.SqlClient
Imports System.Data
Imports System.Globalization
Imports System.Web.UI
Imports Infragistics.Web.UI.GridControls

Partial Class Mobile_mSelectCustomer
    Inherits MyBasePage
    Dim objDataPreserve As New BPADotNetCommonFunctions.VT_DataPreserve.DataPreserve

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

        If Not IsPostBack Then

            InitialiseAndBindEmptyCustomerItemsGrid()
        Else
            wdgCust.DataSource = objDataPreserve.GetWDGDataFromSession(wdgCust)
        End If

    End Sub

    Private Function FillCustPartInquiryDataset(ByVal strSearchString As String, ByVal lngBillingCust As Long) As Data.DataTable

        Dim strsql As String

        Dim dtp As Data.DataTable

        Dim objdb As New VTDBFunctions.VTDBFunctions.DBFunctions

        Dim dtTemp As DataTable

        dtTemp = New DataTable("CustParts")

        dtTemp.Columns.Add("CustomerCode", Type.GetType("System.String"))
        dtTemp.Columns.Add("CustomerName", Type.GetType("System.String"))
        dtTemp.Columns.Add("CustomerId", Type.GetType("System.String"))

        If strSearchString <> "" And lngBillingCust <> 0 Then

            strsql = "SELECT cus_Customers.CustomerName as CustomerName, cus_Customers.CustomerReference as CustomerCode, cus_Customers.CustomerId as CustomerId from cus_Customers, cus_customerDetails where cus_customerDetails.billingCust = " & lngBillingCust & " and cus_Customers.CustomerName like '%" & strSearchString & "%' and Cus_customers.customerId = cus_customerDetails.customerId"

            dtp = objdb.ExecuteSQLReturnDT(strsql)
        ElseIf strSearchString = "" And lngBillingCust > 0 Then
            strsql = "SELECT cus_Customers.CustomerName as CustomerName, cus_Customers.CustomerReference as CustomerCode, cus_Customers.CustomerId as CustomerId from cus_Customers, cus_customerDetails where cus_customerDetails.billingCust = " & lngBillingCust & " and Cus_customers.customerId = cus_customerDetails.customerId"

            dtp = objdb.ExecuteSQLReturnDT(strsql)
        ElseIf strSearchString <> "" And lngBillingCust = 0 Then
            strsql = "SELECT cus_Customers.CustomerName as CustomerName, cus_Customers.CustomerReference as CustomerCode, cus_Customers.CustomerId as CustomerId from cus_Customers, cus_customerDetails where cus_Customers.CustomerName like '%" & strSearchString & "%' and Cus_customers.customerId = cus_customerDetails.customerId"

            dtp = objdb.ExecuteSQLReturnDT(strsql)

        End If

        FillCustPartInquiryDataset = dtp

    End Function



    Sub InitialiseAndBindEmptyCustomerItemsGrid()

        Dim dtCustomerItems As New DataTable
        Dim objDataPreserve As New BPADotNetCommonFunctions.VT_DataPreserve.DataPreserve

        'There is no sales order yet so build a blank datatable and bind the grid to this
        dtCustomerItems.Columns.Add("CustomerName")
        dtCustomerItems.Columns.Add("CustomerCode")
        dtCustomerItems.Columns.Add("CustomerId")

        'Bind the Grid
        objDataPreserve.BindDataToWDG(dtCustomerItems, wdgCust)

    End Sub




    Protected Sub cmdSelect_Click(sender As Object, e As EventArgs) Handles cmdSelect.Click
        If wdgCust.Behaviors.Selection.SelectedRows.Count = 0 Then Exit Sub

        If Not Me.CurrentSession.VT_SelectedDeliveryCustRow Is Nothing Then
            Session("VT_DelCustomerCode") = Me.CurrentSession.VT_SelectedDeliveryCustRow.Rows(0).Item("CustomerCode")
            Session("VT_DelCustomerId") = Me.CurrentSession.VT_SelectedDeliveryCustRow.Rows(0).Item("CustomerID")
            Session("VT_DelCustomerName") = Me.CurrentSession.VT_SelectedDeliveryCustRow.Rows(0).Item("CustomerName")

        End If

        Response.Redirect("mOrderDetails.aspx")


    End Sub

    Protected Sub cmdSearch_Click(sender As Object, e As EventArgs) Handles cmdSearch.Click
        Dim dt As New DataTable
        dt = FillCustPartInquiryDataset(Trim(txtSearchString.Text), Session("VT_BillingCustId"))
        Dim objdata As New BPADotNetCommonFunctions.VT_DataPreserve.DataPreserve

        objdata.BindDataToWDG(dt, wdgCust)

    End Sub

    Protected Sub cmdCancel_Click(sender As Object, e As EventArgs) Handles cmdCancel.Click
        Response.Redirect("mOrderDetails.aspx")

    End Sub

    Private Sub wdgCust_ActiveCellChanged(sender As Object, e As ActiveCellEventArgs) Handles wdgCust.ActiveCellChanged
        Dim objC As New VT_CommonFunctions.CommonFunctions

        'serialise the selected data row
        Dim RowSelected As Infragistics.Web.UI.GridControls.SelectedRowCollection = wdgCust.Behaviors.Selection.SelectedRows
        Dim intActiveRowIndex As Integer = RowSelected(0).Index

        Me.CurrentSession.VT_SelectedDeliveryCustRow = objC.SerialiseWebDataGridRow(wdgCust, intActiveRowIndex)

    End Sub

    Private Sub wdgCust_RowSelectionChanged(sender As Object, e As SelectedRowEventArgs) Handles wdgCust.RowSelectionChanged
        Dim objC As New VT_CommonFunctions.CommonFunctions

        'serialise the selected data row
        Dim RowSelected As Infragistics.Web.UI.GridControls.SelectedRowCollection = wdgCust.Behaviors.Selection.SelectedRows
        Dim intActiveRowIndex As Integer = RowSelected(0).Index

        Me.CurrentSession.VT_SelectedDeliveryCustRow = objC.SerialiseWebDataGridRow(wdgCust, intActiveRowIndex)


    End Sub
End Class
