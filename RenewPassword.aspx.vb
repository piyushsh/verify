Imports BPADotNetCommonFunctions
Imports System.Net.Mail
Imports System.IO
Imports System.Text
Imports System.Data
Imports System.Data.SqlClient
Imports System.Configuration
Imports System.Security.Cryptography
Partial Class RenewPassword
    Inherits System.Web.UI.Page


    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Dim objCommon As New VT_CommonFunctions.CommonFunctions

            If Session("_VT_PasswordRequest") = "YES" Then
                lblExpired.Text = GetGlobalResourceObject("resource", "PasswordExpired_Request")
            Else
                Dim strRenewalDays As String = objCommon.GetConfigItem("PasswordRenewalDays")
                lblExpired.Text = String.Format(GetGlobalResourceObject("resource", "PasswordExpired_Time"), strRenewalDays)
            End If

            lblInst.Text = GetGlobalResourceObject("resource", "PasswordExpiredInstruction")

            hdnPasswordComplex.Value = objCommon.GetConfigItem("PasswordMustBeComplex").ToUpper
            If hdnPasswordComplex.Value = "YES" Then
                ' read the RegularExpression used for validating the Password from the database
                hdnPasswordCheckRegEx.Value = objCommon.GetConfigItem("PasswordCheckRegEx")
                lblPasswordInst.Visible = True
                lblPasswordInst.Text = objCommon.GetConfigItem("PasswordCheckMessage")
            Else
                lblPasswordInst.Visible = False

            End If
            hdnPasswordCannotReuse.Value = objCommon.GetConfigItem("PasswordCannotReuse").ToUpper
            If hdnPasswordCannotReuse.Value = "YES" Then
                lblPasswordReuse.Visible = True
                lblPasswordReuse.Text = GetGlobalResourceObject("resource", "PasswordCannotReuse")
            Else
                lblPasswordReuse.Visible = False
            End If


            btnSet.Attributes.Add("onclick", "return RenewPassword();")

            txtPass.Focus()

        End If
    End Sub
    Protected Sub btnBack_Click(sender As Object, e As EventArgs) Handles btnBack.Click
        Response.Redirect("~/PortalLogin2.aspx?DID=" + Session("_VT_CurrentDID"))
    End Sub


    Protected Sub btnSet_Click(sender As Object, e As EventArgs) Handles btnSet.Click
        Dim objP As New PersonnelModuleFunctions
        Dim objU As New BPADotNetCommonFunctions.UtilFunctions
        Dim objPass As New VT_Password.Password


        ' if required check if the password has been used before
        If hdnPasswordCannotReuse.Value = "YES" Then
            Dim strEncrypted As String
            If hdnPasswordComplex.Value = "YES" Then
                ' change JG 08-10-15 Use the new Encryption
                strEncrypted = objU.Encrypt(txtPass.Text)
            Else
                strEncrypted = objPass.EncryptPassword(txtPass.Text)
            End If

            Dim dtHistory As DataTable = objP.GetUserPasswordHistory(Session("_VT_DotNetConnString"), Session("_VT_CurrentUserId"))
            If dtHistory.Select("Password='" + strEncrypted + "'").Length > 0 Then
                ' password has been used before so reject it
                lblMsg.Text = GetGlobalResourceObject("Resource", "PasswordUsedBefore")
                ModalPopupExtenderMsg.Show()
                Exit Sub
            Else
                ' we must store the password in the user's password history
                objP.WritePasswordHistoryItem(Session("_VT_CurrentUserId"), strEncrypted, PortalFunctions.Now.Date)
            End If
        End If

        ' store the password - this function encrypts the password so we use the textbox data
        objP.WriteUserPassword(Session("_VT_DotNetConnString"), Session("_VT_CurrentUserId"), txtPass.Text)
        objP.WritePasswordRenewalDate(Session("_VT_DotNetConnString"), Session("_VT_CurrentUserId"), PortalFunctions.Now.Date.ToString("s"))

        ' audit log the password change
        Dim objForms As New VT_Forms.Forms
        Dim strAuditComment, strType As String
        strAuditComment = "Password changed in respose to "
        If Session("_VT_PasswordRequest") = "YES" Then
            strAuditComment += "Forgot My Password request."
        Else
            strAuditComment += "Password expiry."
        End If
        strType = "Personnel"
        Dim strUser As String = objP.GetUserName(Session("_VT_DotNetConnString"), Session("_VT_CurrentUserId"))

        objForms.WriteToAuditLog(Session("_VT_CurrentUserId"), strType, Now, strUser, Session("_VT_CurrentUserId"), strAuditComment, "Audit Record", "SysAdmin")


        lblMsg.Text = GetGlobalResourceObject("Resource", "PasswordRenewedOk")
        ModalPopupExtenderMsg.Show()
    End Sub



End Class
