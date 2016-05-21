Imports BPADotNetCommonFunctions
Imports BPADotNetCommonFunctions.VT_CommonFunctions.MatrixFunctions
Imports BPADotNetCommonFunctions.SalesFunctions.Sales_Orders

Imports System.Data.SqlClient
Imports System.Data
Imports System.Globalization
Imports System.Web.UI

Partial Class Mobile_mAddDelCust
    Inherits MyBasePage

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        'show the selected billing customer and the entered Delivery customer name if there was one
        Dim objCust As New VTDBFunctions.VTDBFunctions.CustomerFunctions

        Dim strBillingCustName As String
        If Not IsPostBack Then
            'make sure a billing customer was selected
            If IsNumeric(Session("VT_BillingCustId")) = False Then
                Response.Redirect("mOrderDetails.aspx")
            End If

            If Session("VT_BillingCustId") = 0 Then
                Response.Redirect("mOrderDetails.aspx")
            End If

            strBillingCustName = objCust.GetCustomerNameForId(Session("VT_BillingCustId"))
            lblBillingCust.Text = strBillingCustName

            txtNewCust0.Text = Session("VT_DelCustomerName")
        End If

    End Sub

    Private Sub btnSave0_Click(sender As Object, e As EventArgs) Handles btnSave0.Click
        'save the new delivery customer and then redirect back to the details page
        Dim lngbillingcust As Long
        Dim strCategory As String
        Dim intCategory As Integer
        Dim lngNewCustomer As Long
        Dim strRef As String
        Dim lngRef As Long
        Dim objcommon As New BPADotNetCommonFunctions.VT_CommonFunctions.CommonFunctions
        Dim i As Integer
        Dim dtcust As New DataTable
        Dim strsql As String
        Dim objDB As New VTDBFunctions.VTDBFunctions.DBFunctions

        Dim objCust As New VTDBFunctions.VTDBFunctions.CustomerFunctions

        If Session("VT_BillingCustId") > 0 Then
            lngbillingcust = Session("VT_BillingCustId")
            If Trim(txtNewCust0.Text) <> "" Then
                strCategory = UCase(Left(txtNewCust0.Text, 1))
                intCategory = objCust.SaveNewCustomerArea(strCategory)

                strRef = objcommon.GetConfigItem("NextCustRef")
                If strRef = "" Then
                    strRef = "1000" 'start the customer refs at 1000 and increment from there
                    lngRef = 1000
                End If
                'check if that ref exists, if it does then increment until we find the next available ref num
                lngNewCustomer = objCust.GetCustomerIdForRef(strRef)
                Do While lngNewCustomer <> 0
                    lngRef = CLng(strRef) + 1
                    lngNewCustomer = objCust.GetCustomerIdForRef(lngRef.ToString)

                Loop
                strRef = lngRef.ToString
                'write the next ref back to the config file
                lngRef = lngRef + 1
                objcommon.SetConfigItem("NextCustRef", lngRef.ToString)
                lngNewCustomer = 0

                lngNewCustomer = objCust.SaveNewCustomer(txtNewCust0.Text, intCategory, 0, strRef)
                'System.DateTime.UtcPortalFunctions.Now   save the customer details
                If lngNewCustomer > 0 Then
                    strsql = "Update cus_CustomerDetails set BillingAddress='" & txtAddress.Text & "', DeliveryAddress='" & txtAddress.Text & "', EmailAddress='" & txtemail.Text & "', PhoneNumber ='" & txtPhone.Text & "', BillingCust=" & lngbillingcust & ", NotBillingCustomer=1 where CustomerId =" & lngNewCustomer
                    objDB.ExecuteSQLQuery(strsql)
                    Session("VT_DelCustomerCode") = strRef
                    Session("VT_DelCustomerId") = lngNewCustomer
                    Session("VT_DelCustomerName") = txtNewCust0.Text

                    'save contact details
                    strsql = "update cus_CustomerContacts set ContactName ='" & txtContact.Text & "' where customerId = " & lngNewCustomer
                    objDB.ExecuteSQLQuery(strsql)

                End If
            End If
        End If

        Response.Redirect("mOrderDetails.aspx")
    End Sub
End Class

