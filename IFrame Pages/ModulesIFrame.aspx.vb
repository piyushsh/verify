
Partial Class ModulesIFrame
    Inherits MyBasePage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        ' disable the countdown timer
        Session("_VT_TimeoutRunning") = "NO"

        ModulesFrame.Attributes("src") = Me.CurrentSession.VT_ProjectInIFrame & "&PName=" & Session("_VT_ThisPortalName") & "&URLBase=" & Session("_VT_URLBase") & "&Timeout=" & Session("_VT_SessionTimeoutValue").ToString & "&Bwidth=" & Me.CurrentSession.VT_BrowserWindowWidth

        ModulesFrame.Attributes("Height") = Session("_VT_iFrameHeight")
    End Sub

    Protected Sub btnback_Click(sender As Object, e As ImageClickEventArgs) Handles btnback.Click
        If Not Session("_VT_PageToReturnToFromModulesIFrame") Is Nothing Then
            Response.Redirect(Session("_VT_PageToReturnToFromModulesIFrame"))
        Else
            Response.Redirect("~/TabPages/Orders_Opening_New.aspx")
        End If

    End Sub
End Class
