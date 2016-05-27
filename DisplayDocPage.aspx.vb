
Partial Class DisplayPage
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            lblMsg.Text = Session("_VT_DisplayDocPageMessage")
            PageFrame.Attributes("src") = Session("_VT_DocInIFrame")
        End If

    End Sub

    Protected Sub btnBack_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnBack.Click, btnBack0.Click
        Response.Redirect(Session("_VT_PageToReturnToAfterDisplay"))
    End Sub


End Class
