Imports BPADotNetCommonFunctions


Namespace CashManager

    Partial Class PortalLogin2
        Inherits MyBasePage

#Region " Web Form Designer Generated Code "

        'This call is required by the Web Form Designer.
        <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

        End Sub
        Protected WithEvents Label2 As System.Web.UI.WebControls.Label
        Protected WithEvents lblTreeViewHeader As System.Web.UI.WebControls.Label
        Protected WithEvents Label5 As System.Web.UI.WebControls.Label
        Protected WithEvents Button1 As System.Web.UI.WebControls.Button
        Protected WithEvents Label6 As System.Web.UI.WebControls.Label
        Protected WithEvents HyperLink3 As System.Web.UI.WebControls.HyperLink
        Protected WithEvents imgVerifyLogo As System.Web.UI.WebControls.Image
        Protected WithEvents imgVerifyAd As System.Web.UI.WebControls.Image
        Protected WithEvents hypEMailVerify As System.Web.UI.WebControls.HyperLink
        Protected WithEvents hypVerifyWeb As System.Web.UI.WebControls.HyperLink
        Protected WithEvents lblPoweredBy As System.Web.UI.WebControls.Label


        Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
            'CODEGEN: This method call is required by the Web Form Designer
            'Do not modify it using the code editor.
            InitializeComponent()
        End Sub

#End Region



        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            Dim objBPA As New VT_Forms.Forms
            Dim ds As New Data.DataSet
            Dim objCommonFuncs As New BPADotNetCommonFunctions.VT_CommonFunctions.CommonFunctions


            If Not IsPostBack Then

                'If you're coming from another modulde don't do the check for a second tab being open.
                If Request.QueryString("AT") = "" Then
                    ' if this Session variable is currently valid we must be starting in a second tab in a multi-tab browser - we don't allow that
                    If Session("_VT_VerifySessionLiveInBrowser") IsNot Nothing AndAlso Session("_VT_VerifySessionLiveInBrowser") = "YES" Then
                        Response.Redirect("AlreadyRunning.aspx")
                        Exit Sub
                    End If
                End If


                ' read the version number from the commonfunction class
                lblVer.Text = CommonFunctions.g_strWebsiteVersionNumber

                ' set the session timeout value in minutes - we now set it to a very large number and use our own timeout mechanism
                Session.Timeout = 1000
                Session("_VT_SessionTimeoutValue") = CInt(System.Configuration.ConfigurationManager.AppSettings("Timeout"))    ' store the initial timeout value so we can use it to reset 

                ' then set the session variable we can subsequently check to see if the session is live
                Session("_VT_SessionLive") = "YES"

                Session("_VT_LoggingState") = UCase(System.Configuration.ConfigurationManager.AppSettings("LoggingState"))


                Session("_VT_Role") = VT_Constants.cProposer
                Session("_VT_CurrentUserName") = "Telesales Portal User"
                Session("_VT_NumLoginAttempts") = 0


                '... Set the Session Variable to indicate this is NOT Demo mode
                Session("_VT_DemoMode") = "NO"
                Session("_VT_TrainingMode") = "NO"

                Dim objDisp As New BPADotNetCommonFunctions.VT_Display.DisplayFuncs

                objDisp.SetInitialFocus(txtUserName)

                ' read in the database path
                Dim strConnString As String = System.Configuration.ConfigurationManager.AppSettings("SQLConnStringForDotNet")
                ' replace the XXXX with the database name read from the querystring
                strConnString = Replace(strConnString, "XXXX", Request.QueryString("DID"))

                ' Store the ConnString in session
                Session("_VT_DotNetConnString") = strConnString

                strConnString = System.Configuration.ConfigurationManager.AppSettings("SQLConnString")
                ' replace the XXXX with the database name read from the querystring
                strConnString = Replace(strConnString, "XXXX", Request.QueryString("DID"))
                Session("_VT_BPA.NetConnString") = strConnString

                Session("_VT_CurrentDID") = Request.QueryString("DID")

                Session("_VT_ThisPortalName") = "Sales and Dispatches Portal"
                ' the URL base for this portal is the Request.URL with the QueryString removed
                Session("_VT_URLBase") = Replace(Request.Url.ToString, Request.Url.Query, "")
                Dim strTemp As String = Session("_VT_URLBase")
                Session("_VT_JQueryFileLocation") = Left(strTemp, InStrRev(strTemp, "/"))


                ' get the configured banner file name
                Dim strBanner As String = objCommonFuncs.GetConfigItem("BannerPath")
                If strBanner <> "" Then
                    'imgBanner.ImageUrl = strBanner
                End If


                ' change JG 05/10/15 Load the text on the password hyperlink - if the personnel data is in the matrix
                Dim strPersonnelLocation As String = objCommonFuncs.GetConfigItem("WhereIsPersonnel")
                If UCase(strPersonnelLocation) = "MATRIX" Then
                    hypForgot.Text = GetGlobalResourceObject("resource", "ForgotPassword")
                Else
                    ' we don't support password retrieval for eQOffice passwords
                    hypForgot.Text = ""
                End If

                ' read the current company from the database we are connected to
                Me.CurrentSession.VT_CurrentCompany = objCommonFuncs.GetConfigItem("CompanyName")
                Dim objU As New BPADotNetCommonFunctions.UtilFunctions
                ' the QueryString function replaces '+' characters with spaces so we need to restore them before decrypting
                If Request.QueryString("AT") <> "" Then
                    Me.CurrentSession.VT_UserAuthenticated = True
                    Dim strEncrypted As String = Request.QueryString("AT")
                    Dim strData As String = objU.Decrypt(Replace(strEncrypted, " ", "+"))
                    Session("_VT_CurrentUserId") = strData
                    HandleLogin()
                ElseIf Request.QueryString("RP") <> "" Then
                    ' the url says we want to Renew a Password so get the UserId from the encrypted data and go to the RenewPassword page
                    Dim strEncrypted As String = Request.QueryString("RP")
                    Dim strData As String = objU.Decrypt(Replace(strEncrypted, " ", "+"))
                    Dim astrData() As String = Split(strData, "|")
                    ' the data contains the userId and the date the link was created
                    Session("_VT_CurrentUserId") = CInt(astrData(0))

                    ' check the date the link was created and if it is more than 1 hour old reject it
                    If DateDiff(DateInterval.Hour, CDate(astrData(1)), PortalFunctions.Now) >= 1 Then
                        ' send the Javascript to display a message
                        Dim s As String = "<SCRIPT LANGUAGE='javascript'>" '<!--
                        s = s + String.Format("alert('{0}');", GetGlobalResourceObject("resource", "LinkHasExpired")) + "</SCRIPT>"
                        Response.Write(s)

                        Exit Sub
                    End If
                    ' jump to the RenewPassword page
                    Session("_VT_PasswordRequest") = "YES"
                    Response.Redirect("RenewPassword.aspx")
                Else
                    Me.CurrentSession.VT_UserAuthenticated = False
                End If

            End If
        End Sub


        Sub HandleLogin()
            Dim objPass As New VT_Password.Password
            Dim objCommon As New VT_CommonFunctions.CommonFunctions
            Dim objP As New PersonnelModuleFunctions
            Dim objCommonFuncs As New BPADotNetCommonFunctions.VT_CommonFunctions.CommonFunctions
            Dim strPersonnelLocation As String = objCommonFuncs.GetConfigItem("WhereIsPersonnel")
            Dim strNumFields As String = objCommonFuncs.GetConfigItem("tlsNumFieldsInGridDataTables")
            Dim str As String
            Dim intUserId As Integer



            Dim objcommonf As New BPADotNetCommonFunctions.VT_CommonFunctions.CommonFunctions
            Dim strAccounts As String = UCase(objcommonf.GetConfigItem("AccountsPackage"))
            Session("Accounts") = strAccounts
            Session("InstallationCompany") = UCase(objcommonf.GetConfigItem("InstallationCompany"))

            If IsNumeric(strNumFields) Then
                Me.CurrentSession.VT_tlsNumFields = CInt(strNumFields)
            Else
                Me.CurrentSession.VT_tlsNumFields = 0

            End If


            If Me.CurrentSession.VT_UserAuthenticated = False Then
                ' remove various characters to help prevent hacking
                Dim strUserName As String = txtUserName.Text
                strUserName = ReplaceMultiple(strUserName, "", "<", ">", "%", ";", ")", "(", "&", "+", "-", Chr(34), Chr(39))

                If strUserName = "" Then
                    ' clear the flag that says we are already running in this browser session
                    Session("_VT_VerifySessionLiveInBrowser") = "NO"

                    ' send the Javascript to display a message
                    str = "<SCRIPT LANGUAGE='javascript'>" '<!--
                    str = str + "alert('You must supply a UserName ');</SCRIPT>"
                    Response.Write(str)

                    Exit Sub


                End If

                ' enable the countdown timer
                Session("_VT_TimeoutRunning") = "YES"



                If UCase(strPersonnelLocation) = "MATRIX" Then
                    Dim s As String
                    Dim strStoredPassword As String


                    ' check for expired session
                    If Session("_VT_SessionLive") <> "YES" Then
                        Dim objDisp As New VT_Display.DisplayFuncs
                        objDisp.DisplayMessage(Me, VT_Constants.cTimeOutMessage)
                        Exit Sub
                    End If


                    intUserId = objP.GetUserId(Session("_VT_DotNetConnString"), txtUserName.Text)
                    Session("_VT_CurrentUserCompany") = objP.GetUsersCategoryName(Session("_VT_DotNetConnString"), intUserId)

                    ' retrieve and decrypt the user's password
                    strStoredPassword = objP.GetUserPassword(Session("_VT_DotNetConnString"), intUserId)
                    If strStoredPassword = "VTLOCKED" Then
                        ' send the Javascript to display a message
                        s = "<SCRIPT LANGUAGE='javascript'>" '<!--
                        s = s + "alert('You are locked out of the Sales Management system. Contact the System Administrator ');</SCRIPT>"
                        Response.Write(s)

                        Exit Sub


                    End If

                    Session("_VT_NumLoginAttempts") = Session("_VT_NumLoginAttempts") + 1


                    If Session("_VT_NumLoginAttempts") = 4 Then
                        ' send the Javascript to display a message
                        s = "<SCRIPT LANGUAGE='javascript'>" '<!--
                        s = s + "alert('You have exceeded the allowed number of Login attempts. Contact the System Administrator ');</SCRIPT>"
                        Response.Write(s)
                        ' set the user's password to VTLOCKED to indicate that they are locked out
                        objP.WriteUserPassword(Session("_VT_DotNetConnString"), intUserId, "VTLOCKED")
                        Exit Sub

                    End If

                    If intUserId = 0 Then
                        ' send the Javascript to display a message
                        s = "<SCRIPT LANGUAGE='javascript'>" '<!--
                        s = s + "alert('Unknown Username.. ');</SCRIPT>"
                        Response.Write(s)
                    ElseIf Not objP.IsPasswordValid(Session("_VT_DotNetConnString"), intUserId, txtPassword.Text) Then
                        ' send the Javascript to display a message
                        s = "<SCRIPT LANGUAGE='javascript'>" '<!--
                        s = s + "alert('Invalid Password.. ');</SCRIPT>"
                        Response.Write(s)
                    Else
                        Me.CurrentSession.VT_UserAuthenticated = True
                        Session("_VT_CurrentUserId") = intUserId

                    End If

                Else
                    ' check password in eQOffice
                    Dim s As String
                    Dim strStoredPassword As String

                    Dim objDisp As New VT_Display.DisplayFuncs


                    Dim objVT As New VT_eQOInterface.eQOInterface

                    ' check for expired session
                    If Session("_VT_SessionLive") <> "YES" Then
                        objDisp.DisplayMessage(Me, VT_Constants.cTimeOutMessage)
                        Exit Sub
                    End If

                    intUserId = objVT.GetEQOUserID(txtUserName.Text)

                    'Dim objDBF As New prjTTQAODBFunctions.Personnel
                    ' retrieve and decrypt the user's password from eQOffice
                    strStoredPassword = objPass.RetrieveEQOPassword(intUserId)
                    If strStoredPassword = "VTLOCKED" Then
                        ' send the Javascript to display a message
                        s = "<SCRIPT LANGUAGE='javascript'>" '<!--
                        s = s + "alert('You are locked out of the Telesales system. Contact the System Administrator ');</SCRIPT>"
                        Response.Write(s)
                        Exit Sub
                    End If


                    Session("_VT_NumLoginAttempts") = Session("_VT_NumLoginAttempts") + 1


                    If Session("_VT_NumLoginAttempts") = 4 Then
                        ' send the Javascript to display a message
                        s = "<SCRIPT LANGUAGE='javascript'>" '<!--
                        s = s + "alert('You have exceeded the allowed number of Login attempts. Contact the System Administrator ');</SCRIPT>"
                        Response.Write(s)
                        ' set the user's password to VTLOCKED to indicate that they are locked out
                        objPass.WritePassword(intUserId, "VTLOCKED")
                        Exit Sub

                    End If

                    If intUserId = 0 Then
                        ' send the Javascript to display a message
                        s = "<SCRIPT LANGUAGE='javascript'>" '<!--
                        s = s + "alert('Unknown Username.. ');</SCRIPT>"
                        Response.Write(s)
                    ElseIf Not objPass.IsEQOPasswordValid(intUserId, txtPassword.Text) Then
                        ' send the Javascript to display a message
                        s = "<SCRIPT LANGUAGE='javascript'>" '<!--
                        s = s + "alert('Invalid Password.. ');</SCRIPT>"
                        Response.Write(s)
                    Else
                        Me.CurrentSession.VT_UserAuthenticated = True
                        Session("_VT_CurrentUserId") = intUserId
                    End If
                End If
            End If


            If Me.CurrentSession.VT_UserAuthenticated = True Then

                If UCase(strPersonnelLocation) = "MATRIX" Then

                    ' change JG 06/10/15. Has the Password expired?
                    Session("_VT_CurrentUserId") = intUserId
                    Dim strPasswordRenewal As String = objCommonFuncs.GetConfigItem("PasswordRenewalEnabled").ToUpper
                    If strPasswordRenewal = "YES" Then
                        Dim strRenewalDate As String = objP.GetPasswordRenewalDate(Session("_VT_DotNetConnString"), intUserId)
                        If strRenewalDate = "" Then
                            ' first login for this user since Renewal code added
                            Session("_VT_PasswordRequest") = "NO"
                            Response.Redirect("RenewPassword.aspx")
                        Else
                            ' check if the Renewal Date is more than the configured number of days in the past
                            Dim strRenewalDays As String = objCommonFuncs.GetConfigItem("PasswordRenewalDays")
                            If strRenewalDays <> "" Then
                                If DateDiff(DateInterval.Day, CDate(strRenewalDate), PortalFunctions.Now.Date) > CInt(strRenewalDays) Then
                                    ' redirect to RenewPassword page
                                    Session("_VT_PasswordRequest") = "NO"
                                    Response.Redirect("RenewPassword.aspx")
                                End If
                            End If
                        End If
                    End If

                    ' set the flag that says we are already running in this browser session
                    Session("_VT_VerifySessionLiveInBrowser") = "YES"


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
                    Dim strNow As String = PortalFunctions.Now

                    objPass.AddLoginRecord(CLng(intUserId), "Telesales Management System User", Session("_VT_CurrentUserName"), strNow, Session("_VT_UserHostAddress"), Request.Browser.Type, Request.Browser.Platform, strRef)


                    Me.CurrentSession.VT_Top_CurrentTab = "Orders"
                    Me.CurrentSession.VT_TopTab_Clicked = "Orders"

                    'SmcN 02/06/2014 Set the Values for the Client Browser configuration
                    If Val(hdnBrowserWidth.Value) < 1024 Then
                        Me.CurrentSession.VT_BrowserWindowWidth = 1024
                    Else
                        Me.CurrentSession.VT_BrowserWindowWidth = Val(hdnBrowserWidth.Value)
                    End If



                    'SmcN 20/01/2014  Added this section to load the appropriate Module pages and Sales Order Editing pages
                    'Fill pages array
                    Dim objTelesales As New SalesOrdersFunctions.SalesOrders
                    objTelesales.FillOrderPagesArray()
                    objTelesales.FillModulePagesArray()

                    'Loop through the ModulePages Array to find the index of the 'Sales Order Opening'  page
                    Dim intCnt, intIndex As Integer
                    intIndex = -99

                    If Me.CurrentSession.aVT_ModulePageOptionsPages IsNot Nothing AndAlso Me.CurrentSession.aVT_ModulePageOptionsPages.Length > 0 Then
                        For intCnt = 0 To Me.CurrentSession.aVT_ModulePageOptionsPages.Length
                            If Trim(Me.CurrentSession.aVT_ModulePageOptions(intCnt)) = "Sales Order Opening" Then
                                intIndex = intCnt
                                Exit For
                            End If
                        Next
                    End If

                    ' Launch the portal 
                    If intIndex >= 0 Then
                        'Launch the appropriate page
                        Response.Redirect("TabPages/" & Me.CurrentSession.aVT_ModulePageOptionsPages(intIndex))
                    Else
                        'Otherwise launch the Default page
                        Response.Redirect("TabPages/Orders_Opening.aspx")
                    End If



                Else
                    Dim objVT As New VT_eQOInterface.eQOInterface
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

                    Dim strNow As String = PortalFunctions.Now

                    objPass.AddLoginRecord(CLng(intUserId), "TelesalesUser", Session("_VT_CurrentUserName"), strNow, Request.UserHostAddress, Request.Browser.Type, Request.Browser.Platform, strRef)



                    Me.CurrentSession.VT_Top_CurrentTab = "Orders"
                    Me.CurrentSession.VT_TopTab_Clicked = "Orders"

                    'SmcN 20/01/2014  Added this section to load the appropriate Module pages and Sales Order Editing pages
                    'Fill pages array
                    Dim objTelesales As New SalesOrdersFunctions.SalesOrders
                    objTelesales.FillOrderPagesArray()
                    objTelesales.FillModulePagesArray()

                    'Loop through the ModulePages Array to find the index of the  'Sales Order Opening'  page
                    Dim intCnt, intIndex As Integer
                    intIndex = -99

                    If Me.CurrentSession.aVT_ModulePageOptionsPages IsNot Nothing AndAlso Me.CurrentSession.aVT_ModulePageOptionsPages.Length > 0 Then
                        For intCnt = 0 To Me.CurrentSession.aVT_ModulePageOptionsPages.Length
                            If Trim(Me.CurrentSession.aVT_ModulePageOptions(intCnt)) = "Sales Order Opening" Then
                                intIndex = intCnt
                                Exit For
                            End If
                        Next
                    End If



                    ' Launch the portal 
                    If intIndex >= 0 Then
                        'Launch the appropriate page
                        Response.Redirect("TabPages/" & Me.CurrentSession.aVT_ModulePageOptionsPages(intIndex))
                    Else
                        'Otherwise launch the Default page
                        Response.Redirect("TabPages/Orders_Opening.aspx")
                    End If


                End If



            End If


        End Sub

        Private Sub btnLogin_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLogin.Click
            If Val(hdnBrowserWidth.Value) < 1024 Then
                Me.CurrentSession.VT_BrowserWindowWidth = 1024
            Else
                Me.CurrentSession.VT_BrowserWindowWidth = Val(hdnBrowserWidth.Value)
            End If


            ' if this Session variable is currently valid we must be starting in a second tab in a multi-tab browser - we don't allow that
            If Session("_VT_VerifySessionLiveInBrowser") IsNot Nothing AndAlso Session("_VT_VerifySessionLiveInBrowser") = "YES" Then
                Response.Redirect("AlreadyRunning.aspx")
                Exit Sub
            End If


            HandleLogin()


        End Sub

        '*********************************************************
        'PURPOSE: Replaces multiple substrings in a string with the
        'character or string specified by ReplaceString

        'PARAMETERS: OrigString -- The string to replace characters in
        '            ReplaceString -- The replacement string
        '            FindChars -- comma-delimited list of
        '                 strings to replace with ReplaceString
        '
        'RETURNS:    The String with all instances of all the strings
        '            in FindChars replaced with Replace String
        'EXAMPLE:    s= ReplaceMultiple("H;*()ello", "", ";", ",", "*", "(", ")") -
        'Returns Hello
        'CAUTIONS:   'Overlap Between Characters in ReplaceString and 
        '             FindChars Will cause this function to behave 
        '             incorrectly unless you are careful about the 
        '             order of strings in FindChars
        '***************************************************************

        Public Function ReplaceMultiple(ByVal OrigString As String, ByVal ReplaceString As String, ByVal ParamArray FindChars() As String) As String

            Dim lLBound As Long
            Dim lUBound As Long
            Dim lCtr As Long
            Dim sAns As String

            lLBound = LBound(FindChars)
            lUBound = UBound(FindChars)

            sAns = OrigString

            For lCtr = lLBound To lUBound
                sAns = Replace(sAns, CStr(FindChars(lCtr)), ReplaceString)
            Next

            ReplaceMultiple = sAns


        End Function

    End Class

End Namespace
