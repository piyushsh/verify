
Partial Class AlreadyRunning
    Inherits System.Web.UI.Page


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            lblCaption.Text = "You already have a window open with the sales portal running. You can't open two copies of the sales portal at once on one pc. Please close one." 'GetGlobalResourceObject("resource", "AlreadyRunning")
        End If
    End Sub
End Class
