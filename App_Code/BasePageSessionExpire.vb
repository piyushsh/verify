Imports Microsoft.VisualBasic

Public Class BasePageSessionExpire
    Inherits System.Web.UI.Page

    Protected Overrides Sub OnInit(ByVal e As System.EventArgs)
        ' Call the base OnInit method.
        MyBase.OnInit(e)

        If Not Context.Session Is Nothing Then
            If Session.IsNewSession Then
                Dim strCookieHeader As String = Request.Headers("Cookie")

                If Not strCookieHeader Is Nothing Then
                    If strCookieHeader.ToUpper().IndexOf("ASP.NET_SESSIONID") >= 0 Then
                        'On timeouts, redirect user to timeout page.
                        Response.Redirect("SessionExpire.aspx")
                    End If
                End If
            End If
        End If
    End Sub

End Class

