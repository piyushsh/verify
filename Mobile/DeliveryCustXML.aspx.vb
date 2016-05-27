Imports System
Imports System.Collections
Imports System.ComponentModel
Imports System.Data
Imports System.Drawing
Imports System.Web
Imports System.Web.SessionState
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.UI.HtmlControls


Partial Class DeliveryCustXML

    Inherits MyBasePage


    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs)

        Dim ds As New DataTable
        ' This page makes the contents of the dataset available as an XML file
        ' that the javascript XmlHttpRequest object can download
        Response.ContentType = "text/xml"



        If Request("SearchString") IsNot Nothing Then
            ds = FillCustPartInquiryDataset(Request("SearchString"), Request("BillingCustID"))
        End If

        ds.WriteXml(Response.OutputStream)
        Response.[End]()
    End Sub


    Private Function FillCustPartInquiryDataset(ByVal strSearchString As String, ByVal lngBillingCust As Long) As Data.DataTable


        Dim strsql As String

        Dim dtp As Data.DataTable

        Dim objdb As New VTDBFunctions.VTDBFunctions.DBFunctions

        Dim dtTemp As DataTable
        Dim drNewRow As DataRow


        dtTemp = New DataTable("CustParts")

        dtTemp.Columns.Add("CustomerCode", Type.GetType("System.String"))
        dtTemp.Columns.Add("CustomerName", Type.GetType("System.String"))
        dtTemp.Columns.Add("CustomerId", Type.GetType("System.String"))


        If strSearchString <> "" And lngBillingCust <> 0 Then
            'do a search based on both part nums
            '            cp_mstr Table

            '1.	Cp_cust_part – Customer Part Number 
            '2.	Cp_part – Steripack Part Number
            '3.	Cp_cust – Customer Code

            strsql = "SELECT cus_Customers.CustomerName as CustomerName, cus_Customers.CustomerCode as CustomerCode, cus_Customers.CustomerId as CustomerId from cus_Customers, cus_customerDetails where cus_customerDetails.billingCust = " & lngBillingCust & " and cus_Customers.CustomerName like '%" & strSearchString & "' and Cus_customers.customerId = cus_customerDetails.customerId"


            dtp = objdb.ExecuteSQLReturnDT(strsql)


        End If

        FillCustPartInquiryDataset = dtTemp

    End Function



End Class
