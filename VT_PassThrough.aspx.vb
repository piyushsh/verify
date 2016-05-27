Imports BPADotNetCommonFunctions
Imports System.Data.SqlClient
Imports System.Data
Imports VTDBFunctions.VTDBFunctions

Public Class VT_PassThrough
    Inherits MyBasePage

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        ' if we are coming in other than through the login page we musat setup our connections
        Dim objCommonFuncs As New BPADotNetCommonFunctions.VT_CommonFunctions.CommonFunctions
        Dim strNumFields As String = objCommonFuncs.GetConfigItem("tlsNumFieldsInGridDataTables")

        If IsNumeric(strNumFields) Then
            Me.CurrentSession.VT_tlsNumFields = CInt(strNumFields)
        Else
            Me.CurrentSession.VT_tlsNumFields = 0

        End If
        ' set the session timeout value in minutes
        Session.Timeout = CInt(System.Configuration.ConfigurationManager.AppSettings("Timeout"))
        ' then set the session variable we can subsequently check to see if the session is live
        Session("_VT_SessionLive") = "YES"

        If Request.QueryString("DID") = "" Then
            Dim objDisp As New VT_Display.DisplayFuncs
            objDisp.DisplayMessage(Page, "No DID value was supplied.\n\nThe Telesales Module cannot start")
            Exit Sub
        End If

        If Request.QueryString("UID") = "" Then
            Dim objDisp As New VT_Display.DisplayFuncs
            objDisp.DisplayMessage(Page, "No UserID value was supplied.\n\nThe Telesales Module cannot start")
            Exit Sub
        End If

        If Request.QueryString("SOID") = "" Then
            Me.CurrentSession.VT_SalesOrderID = 0
        Else
            Me.CurrentSession.VT_SalesOrderID = CLng(Request.QueryString("SOID"))
        End If


        If Session("_VT_DotNetConnString") = "" Then

            ' read in the database path
            Dim strConnString As String = System.Configuration.ConfigurationManager.AppSettings("SQLConnStringForDotNet")
            ' replace the XXXX with the database name read from the querystring
            strConnString = Replace(strConnString, "XXXX", Request.QueryString("DID"))
            ' Store the ConnString in session
            Session("_VT_DotNetConnString") = strConnString

            strConnString = System.Configuration.ConfigurationManager.AppSettings("SQLConnString")
            ' replace the XXXX with the database name read from the querystring
            strConnString = Replace(strConnString, "XXXX", Request.QueryString("DID"))
            ' Store the ConnString in session
            Session("_VT_BPA.NetConnString") = strConnString

            Session("_VT_CurrentUserId") = Request.QueryString("UID")



        End If


         Dim strPersonnelLocation As String = objCommonFuncs.GetConfigItem("WhereIsPersonnel")

        If UCase(strPersonnelLocation) = "MATRIX" Then
            Dim intUserId As Integer

            Dim objPass As New VT_Password.Password
            Dim objCommon As New VT_CommonFunctions.CommonFunctions
            Dim objP As New PersonnelModuleFunctions


            intUserId = Session("_VT_CurrentUserId")
            Session("_VT_CurrentUserCompany") = objP.GetUsersCategoryName(Session("_VT_DotNetConnString"), intUserId)


            Session("_VT_CurrentUserId") = intUserId

            ' get the user name from the ID
            Dim astrData(2) As String
            astrData(0) = "txtFirstName"
            astrData(1) = "txtSurname"
            astrData(2) = "txtEMail"
            Dim dt As Data.DataTable = objP.GetPersonnelDataItems(Session("_VT_DotNetConnString"), intUserId, astrData)

            Session("_VT_CurrentUserName") = dt.Rows(0).Item("Field0") + " " + dt.Rows(0).Item("Field1")
            Session("_VT_CurrentUserEMail") = dt.Rows(0).Item("Field2")

            Session("_VT_UserHostAddress") = Request.UserHostAddress()
            Session("_VT_Referrer") = Request.UrlReferrer.ToString


            ' log the time and date of the successful login
            Dim strRef As String
            If IsNothing(Request.UrlReferrer) Then
                strRef = "Not available"
            Else
                strRef = Request.UrlReferrer.ToString
            End If
            Session("_VT_Referrer") = strRef
            objPass.AddLoginRecord(CLng(intUserId), "Telesales Management System User", Session("_VT_CurrentUserName"), Now, Request.UserHostAddress, Request.Browser.Type, Request.Browser.Platform, strRef)


        ElseIf UCase(strPersonnelLocation) = "SQL" Then
            Dim s As String
            ' send the Javascript to display a message
            s = "<SCRIPT LANGUAGE='javascript'>" '<!--
            s = s + "alert('The Configured Personnel Location is not currently supported please configure a different Location. Contact the System Administrator ');</SCRIPT>"
            Response.Write(s)

            Exit Sub
        Else 'Default is eQOffice Personnel
            Dim intUserId As Integer

            Dim objDisp As New VT_Display.DisplayFuncs


            Dim objVT As New VT_eQOInterface.eQOInterface
            Dim objPass As New VT_Password.Password
            'Dim recUser As DAO.Recordset


            intUserId = Session("_VT_CurrentUserId")


            Dim dtUser As Data.DataTable = objVT.GetEQOPersonalDetails(intUserId)


            Session("_VT_CurrentUserId") = intUserId
            Session("_VT_CurrentUserName") = objVT.GetEQOUserName(intUserId)
            Session("_VT_UserHostAddress") = Request.UserHostAddress()
            Session("_VT_CurrentUserEMail") = dtUser.Rows(0).Item("EmployeeEMail")


            ' reset menu item control variable
            Session("_VT_MenuKeyPressed") = ""

            ' log the time and date  of the successful login
            Dim strRef As String
            'If IsNothing(Request.UrlReferrer.ToString) Then
            strRef = "Not available"
            'Else
            '    strRef = Request.UrlReferrer.ToString
            'End If
            objPass.AddLoginRecord(CLng(intUserId), "TelesalesUser", Session("_VT_CurrentUserName"), Now, Request.UserHostAddress, Request.Browser.Type, Request.Browser.Platform, strRef)

        End If

        Dim strProduct As String
        strProduct = Request.QueryString("ProductID")
        If strProduct <> "" Then ' launched from telesales interface to see the product stock position
            Me.CurrentSession.VT_ProdSelectProdID = CLng(IIf(IsNumeric(strProduct), strProduct, 0))
            Response.Redirect("OtherPages/ProductStockDetails.aspx")
            Exit Sub
        End If

        If Me.CurrentSession.VT_SalesOrderID = 0 Then

            Me.CurrentSession.VT_Top_CurrentTab = "Orders"
            Me.CurrentSession.VT_TopTab_Clicked = "Orders"
            Response.Redirect("TabPages/Orders_Opening.aspx")
        Else

            Dim objdbfuncs As New VTDBFunctions.VTDBFunctions.TelesalesFunctions
            Dim dsOrder As New DataSet
            Dim lngSalesOrderId As Long
            Dim lngID As Long
            Dim objCust As New VTDBFunctions.VTDBFunctions.CustomerFunctions

            lngSalesOrderId = Me.CurrentSession.VT_SalesOrderID

            dsOrder = objdbfuncs.GetOrderForId(lngSalesOrderId)
            If dsOrder.Tables(0).Rows.Count > 0 Then
                lngID = dsOrder.Tables(0).Rows(0).Item("SalesOrderNum")
                Me.CurrentSession.VT_JobID = lngID
                Me.CurrentSession.VT_SalesOrderNum = lngID
                Me.CurrentSession.VT_SalesOrderID = lngSalesOrderId
                Dim strCustomerName As String = objCust.GetCustomerNameForId(dsOrder.Tables(0).Rows(0).Item("DeliveryCustomerID"))
                Me.CurrentSession.VT_CustomerName = strCustomerName
                Dim strStatus As String = dsOrder.Tables(0).Rows(0).Item("Status")
                Me.CurrentSession.VT_JobStatus = strStatus

                Dim strOrderTypes As String = objCommonFuncs.GetConfigItem("DifferentSalesOrderTypes")
                If UCase(strOrderTypes) = "YES" Then

                    If dsOrder.Tables(0).Rows.Count > 0 AndAlso IsDBNull(dsOrder.Tables(0).Rows(0).Item("Type")) = False Then
                        Me.CurrentSession.VT_OrderType = dsOrder.Tables(0).Rows(0).Item("Type")
                    End If
                Else
                    Me.CurrentSession.VT_OrderType = ""
                End If

                Dim ds As New DataSet
                Dim objProduction As New ProductionFunctions.Production
                ds = objProduction.GetJobSpecifics(lngID)
                If ds.Tables(0).Rows.Count > 0 Then
                    Me.CurrentSession.VT_JobComment = IIf(IsDBNull(ds.Tables(0).Rows(0).Item("Comment")), "", ds.Tables(0).Rows(0).Item("Comment"))
                Else
                    Me.CurrentSession.VT_JobComment = ""
                End If

                Me.CurrentSession.VT_TopTab_Clicked = "DETAILS"
                Response.Redirect("~/TabPages/Details_Opening.aspx")


            Else
                Me.CurrentSession.VT_Top_CurrentTab = "Orders"
                Me.CurrentSession.VT_TopTab_Clicked = "Orders"
                Response.Redirect("TabPages/Orders_Opening.aspx")
            End If

        End If

    End Sub

End Class
