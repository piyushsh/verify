
Partial Class ErrorPage
    Inherits MyBasePage

    
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        lblError.Text = Me.CurrentSession.VT_Error
        Me.CurrentSession.VT_Error = ""


    End Sub
End Class
