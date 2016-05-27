Imports BPADotNetCommonFunctions
Imports System.Net.Mail

Partial Class ForgotPassword
    Inherits System.Web.UI.Page


    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            lblForgot.Text = GetGlobalResourceObject("resource", "ForgotPassword")
            lblInst.Text = GetGlobalResourceObject("resource", "ForgotPasswordInstruction")

            btnSend.Attributes.Add("onclick", "return GetPassword();")

            txtLogin.Focus()

        End If
    End Sub
    Protected Sub btnBack_Click(sender As Object, e As EventArgs) Handles btnBack.Click
        Response.Redirect("~/PortalLogin2.aspx?DID=" + Session("_VT_CurrentDID"))
    End Sub

    Protected Sub btnSend_Click(sender As Object, e As EventArgs) Handles btnSend.Click
        Dim objP As New PersonnelModuleFunctions
        Dim objU As New BPADotNetCommonFunctions.UtilFunctions

        ' remove various characters to help prevent hacking
        Dim strUserName As String = txtLogin.Text
        strUserName = ReplaceMultiple(strUserName, "", "<", ">", "%", ";", ")", "(", "&", "+", "-", Chr(34), Chr(39))

        Dim intUserId As Integer = objP.GetUserId(Session("_VT_DotNetConnString"), strUserName)
        If intUserId = 0 Then
            lblMsg.Text = GetGlobalResourceObject("resource", "UnknownUsername")
            ModalPopupExtenderMsg.Show()
            Exit Sub
        End If


        ' get the user name from the ID
        Dim astrData(2) As String
        astrData(0) = "txtFirstName"
        astrData(1) = "txtSurname"
        astrData(2) = "txtEMail"
        Dim dt As Data.DataTable = objP.GetPersonnelDataItems(Session("_VT_DotNetConnString"), intUserId, astrData)
        Dim strUserFirstName As String = If(IsDBNull(dt.Rows(0).Item("Field0")), "", dt.Rows(0).Item("Field0"))
        Dim strUserSurname As String = If(IsDBNull(dt.Rows(0).Item("Field1")), "", dt.Rows(0).Item("Field1"))
        Dim strUserEMail As String = If(IsDBNull(dt.Rows(0).Item("Field2")), "", dt.Rows(0).Item("Field2"))

        If strUserEMail = "" Then
            Dim str As String = String.Format(GetGlobalResourceObject("Resource", "NoEMailAddress"), strUserFirstName)
            lblMsg.Text = str
            ModalPopupExtenderMsg.Show()
            Exit Sub
        End If

        ' retrieve the password
        Dim strError As String = ""
        Dim strPass As String = objP.GetUserPassword(Session("_VT_DotNetConnString"), intUserId)

        Dim objC As New BPADotNetCommonFunctions.VT_CommonFunctions.CommonFunctions

        Dim strMailFrom As String = objC.GetConfigItem("SMTPMailFrom")
        Dim strMailServer As String = objC.GetConfigItem("SMTPServer")
        Dim strMailPass As String = objC.GetConfigItem("SMTPPassword")
        Dim strSubject As String = String.Format(GetGlobalResourceObject("Resource", "PasswordMailSubject"), Session("_VT_ThisPortalName"))
        Dim strURLLink As String = Session("_VT_URLBase") + "?DID=" + Session("_VT_CurrentDID")

        ' create a string that tells the portal we want to do a password renewal
        Dim strData As String = "&RP="
        ' encrypt the userId and the date/time into the string
        Dim strEncryptedData As String = objU.Encrypt(intUserId.ToString + "|" + Now.ToString("s"))
        strData += strEncryptedData
        strURLLink += strData

        Dim strMailTo As String = strUserFirstName
        If strUserSurname <> "" Then
            strMailTo += " " + strUserSurname
        End If

        strError = ""
        Dim strText As String = String.Format(GetGlobalResourceObject("Resource", "PasswordMailLine1"), strMailTo) + "<br/>" + "<br/>"
        strText += String.Format(GetGlobalResourceObject("Resource", "PasswordMailLine2"), Session("_VT_ThisPortalName"), Session("_VT_CurrentDID")) + "<br/>" + "<br/><b><i>" + strPass + "</i></b><br/>" + "<br/>"
        strText += "<a href=" + Chr(34) + strURLLink + Chr(34) + ">" + GetGlobalResourceObject("Resource", "PasswordMailLine3") + "</a>"
        strText += "<br/>" + "<br/>" + "<br/>"
        strText += GetGlobalResourceObject("Resource", "PasswordMailLine4")

        objC.SendMailMessage(strSubject, strText, strMailFrom, strUserEMail, strMailServer, , System.Web.Mail.MailFormat.Html, strError, , , strMailFrom, strMailPass)

        If strError <> "" Then
            lblMsg.Text = String.Format(GetGlobalResourceObject("Resource", "PasswordMailError"), strError)


        Else
            lblMsg.Text = GetGlobalResourceObject("Resource", "PasswordMailSent")
            ' audit log the password request
            Dim objForms As New VT_Forms.Forms
            Dim strAuditComment, strType As String
            strAuditComment = "Password recovery request was made and password was sent to: " + strUserEMail
            strType = "Personnel"
            objForms.WriteToAuditLog(intUserId, strType, Now, strMailTo, Session("_VT_CurrentUserId"), strAuditComment, "Audit Record", "SysAdmin")
        End If

        ModalPopupExtenderMsg.Show()

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
