Imports BPADotNetCommonFunctions

Partial Class DetailsMasterNoBar
    Inherits System.Web.UI.MasterPage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        '     ScriptManager1.RegisterAsyncPostBackControl(uwgOptions)

        If Not IsPostBack Then

       
        End If


    End Sub


    Protected Sub btnBack_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnBack.Click
    

        Select Case Session("VT_FromPage")
            Case "Details"
                Response.Redirect("~/TabPages/Details_Opening.aspx")
            Case "Planning"
                Response.Redirect("~/TabPages/Planning_Opening.aspx")
            Case "OrderList"
                Response.Redirect("~/TabPages/Orders_Opening.aspx")
            Case "Deliveries"
                Response.Redirect("~/TabPages/WarehouseManager.aspx")
            Case Else
                Response.Redirect("~/TabPages/Orders_Opening.aspx")

        End Select

    End Sub


End Class

