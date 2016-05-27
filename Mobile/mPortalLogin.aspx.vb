Imports BPADotNetCommonFunctions

Partial Class MobilePages_mPortalLogin
    Inherits MyBasePage

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        Dim objBPA As New VT_Forms.Forms
        Dim ds As New Data.DataSet


        If Not IsPostBack Then

            If Not Session("_VT_SessionLive") Is Nothing Then
                ' Response.Redirect("AlreadyRunning.aspx")
                'need a replacement for this for mobile

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


            ' if the following QueryString variables exist then the User has already
            ' been authenticated 
            If Request.QueryString("Auth") = "Ok" Then
                Session("_VT_UserAuthenticated") = "YES"
                Session("_VT_CurrentUserId") = Request.QueryString("UID")
                btnLogin_Click(sender, e)
            Else
                Session("_VT_UserAuthenticated") = "NO"
            End If

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

            Session("_VT_ThisPortalName") = "Sales and Dispatches Portal - mobile"
            ' the URL base for this portal is the Request.URL with the QueryString removed
            Session("_VT_URLBase") = Replace(Request.Url.ToString, Request.Url.Query, "")
            Dim strTemp As String = Session("_VT_URLBase")
            Session("_VT_JQueryFileLocation") = Left(strTemp, InStrRev(strTemp, "/"))


            ' get the configured banner file name
            Dim objCommonFuncs As New BPADotNetCommonFunctions.VT_CommonFunctions.CommonFunctions
            Dim strBanner As String = objCommonFuncs.GetConfigItem("BannerPath")
            If strBanner <> "" Then
                'imgBanner.ImageUrl = strBanner
            End If

            ' read the current company from the database we are connected to
            Me.CurrentSession.VT_CurrentCompany = objCommonFuncs.GetConfigItem("CompanyName")

        End If
    End Sub

    Protected Sub btnLogin_Click(sender As Object, e As EventArgs) Handles btnLogin.Click
        Dim objCommonFuncs As New BPADotNetCommonFunctions.VT_CommonFunctions.CommonFunctions
        Dim strPersonnelLocation As String = objCommonFuncs.GetConfigItem("WhereIsPersonnel")
        Dim strNumFields As String = objCommonFuncs.GetConfigItem("tlsNumFieldsInGridDataTables")
        Dim str As String
        Dim objcommonf As New BPADotNetCommonFunctions.VT_CommonFunctions.CommonFunctions
        Dim strAccounts As String = UCase(objcommonf.GetConfigItem("AccountsPackage"))
        Session("Accounts") = strAccounts
        Session("InstallationCompany") = UCase(objcommonf.GetConfigItem("InstallationCompany"))

        If IsNumeric(strNumFields) Then
            Me.CurrentSession.VT_tlsNumFields = CInt(strNumFields)
        Else
            Me.CurrentSession.VT_tlsNumFields = 0

        End If
        ' remove various characters to help prevent hacking
        Dim strUserName As String = txtUserName.Text
        'strUserName = ReplaceMultiple(strUserName, "", "<", ">", "%", ";", ")", "(", "&", "+", "-", Chr(34), Chr(39))

        If strUserName = "" Then
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
            Dim intUserId As Integer
            Dim strStoredPassword As String

            Dim objPass As New VT_Password.Password
            Dim objCommon As New VT_CommonFunctions.CommonFunctions
            Dim objP As New PersonnelModuleFunctions

            ' check for expired session
            If Session("_VT_SessionLive") <> "YES" Then
                Dim objDisp As New VT_Display.DisplayFuncs
                objDisp.DisplayMessage(Me, VT_Constants.cTimeOutMessage)
                Exit Sub
            End If


            intUserId = objP.GetUserId(Session("_VT_DotNetConnString"), txtUserName.Text)
            Session("_VT_CurrentUserCompany") = objP.GetUsersCategoryName(Session("_VT_DotNetConnString"), intUserId)

            ' retrieve and decrypt the user's password from eQOffice
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


                Me.CurrentSession.VT_Top_CurrentTab = "Orders"
                Me.CurrentSession.VT_TopTab_Clicked = "Orders"

              
    

             
                Response.Redirect("mOrdersOpening.aspx")



            End If

        ElseIf UCase(strPersonnelLocation) = "SQL" Then
            Dim s As String
            ' send the Javascript to display a message
            s = "<SCRIPT LANGUAGE='javascript'>" '<!--
            s = s + "alert('The Configured Personnel Location is not currently supported please configure a different Location. Contact the System Administrator ');</SCRIPT>"
            Response.Write(s)

            Exit Sub
        Else 'Default is eQOffice Personnel
            Dim s As String
            Dim intUserId As Integer
            Dim strStoredPassword As String

            Dim objDisp As New VT_Display.DisplayFuncs


            Dim objVT As New VT_eQOInterface.eQOInterface
            Dim objPass As New VT_Password.Password
            'Dim recUser As DAO.Recordset

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

            ' if we didn't get the user ID from another page
            If Session("_VT_UserAuthenticated") = "NO" Then
                intUserId = objVT.GetEQOUserID(txtUserName.Text)

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
                    Session("_VT_UserAuthenticated") = "YES"
                    Session("_VT_CurrentUserId") = intUserId
                End If

            End If

            If Session("_VT_UserAuthenticated") = "YES" Then
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



                Me.CurrentSession.VT_Top_CurrentTab = "Orders"
                Me.CurrentSession.VT_TopTab_Clicked = "Orders"

              

               

                Response.Redirect("mOrdersOpening.aspx")

                ' Use this code to launch the portal in a new window
                'Dim strWindow As String = "window.open('TabPages/Orders_Opening.aspx','_blank','left=30,top=30,height=650,width=1200,status=yes,toolbar=no,menubar=no,location=no,scrollbars=yes');"
                'objDisp.SendJavaScript(Page, strWindow)

        End If

        End If

    End Sub
End Class
