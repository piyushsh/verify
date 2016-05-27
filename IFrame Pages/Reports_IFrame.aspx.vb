
Partial Class Reports_IFrame
    Inherits MyBasePage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        ' disable the countdown timer
        Session("_VT_TimeoutRunning") = "NO"

        ReportModulesFrame.Attributes("src") = Me.CurrentSession.VT_ProjectInIFrame + "&PName=" + Session("_VT_ThisPortalName") + "&URLBase=" + Session("_VT_URLBase") + "&Timeout=" + Session("_VT_SessionTimeoutValue").ToString

    End Sub
End Class
