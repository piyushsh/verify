
Partial Class ModulesIFrame_Responsive
    Inherits MyBasePage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        ' disable the countdown timer
        Session("_VT_TimeoutRunning") = "NO"
        If Val(hdnBrowserWidth.Value) < 1024 Then
            Me.CurrentSession.VT_BrowserWindowWidth = 1024
        Else
            Me.CurrentSession.VT_BrowserWindowWidth = Val(hdnBrowserWidth.Value)
        End If
        ModulesFrame.Attributes("src") = Me.CurrentSession.VT_ProjectInIFrame + "&PName=" + Session("_VT_ThisPortalName") + "&URLBase=" + Session("_VT_URLBase") + "&Timeout=" + Session("_VT_SessionTimeoutValue").ToString
        'ModulesFrame.Attributes("Height") = Session("_VT_iFrameHeight")

        'ModulesFrame.Attributes("src") = "Test1.aspx"

    End Sub
End Class
