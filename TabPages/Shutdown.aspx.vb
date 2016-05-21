
Partial Class Shutdown
    Inherits MyBasePage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        ' if we have a valid FormInstanceId clear the form lock
        Dim strTemp As String = Request.QueryString("OrderIdToClear")
        Dim FormInstanceId As Integer = If(strTemp = "", 0, CInt(strTemp))
        If FormInstanceId <> 0 Then
            ' if we get here the Session has probably been cleared so we reload the DB Connstring
            Dim strConnString As String = System.Configuration.ConfigurationManager.AppSettings("SQLConnStringForDotNet")
            ' replace the XXXX with the database name read from the querystring
            strConnString = Replace(strConnString, "XXXX", Request.QueryString("DID"))

            Dim objVTM As New VTDBFunctions.VTDBFunctions.VTMatrixFunctions
            objVTM.LockFormForWriting(strConnString, FormInstanceId, "tls_FormData", "")
        End If

        ' End The Session
        If Not Session("Abandon") Is Nothing Then
            If Session("Abandon") = "YES" Then
                Session.Abandon()
            End If
        End If
        ' 

        Dim obj As New BPADotNetCommonFunctions.VT_Display.DisplayFuncs
        obj.SendJavaScript(Page, "window.close();")


    End Sub
End Class
