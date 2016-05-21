Imports BPADotNetCommonFunctions
Imports BPADotNetCommonFunctions.VT_CommonFunctions.MatrixFunctions
Imports System.Data.SqlClient
Imports System.Data
Imports System.Drawing
Imports System.Data.DataTableExtensions
Imports Infragistics.Web.UI.GridControls

Partial Class TabPages_LatePlannedOrders
    Inherits MyBasePage


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        cmdInputOK.OnClientClick = String.Format("MessageUpdate('{0}','{1}')", cmdInputOK.UniqueID, "")
        FillData()

    End Sub

    Sub FillData()
        Dim strLateOrders As String = Session("LatePlannedOrders")
        'this is a string containing all late orders
        Dim dt As New DataTable
        Dim aOrders() As String
        Dim drrow As DataRow
        Dim i As Integer
        Dim objDatapreserve As New BPADotNetCommonFunctions.VT_DataPreserve.DataPreserve
        Dim objSales As New BPADotNetCommonFunctions.SalesFunctions.Sales_Orders
        Dim strComment As String

        dt.Columns.Add("SO_ContiguousNum")
        dt.Columns.Add("SalesOrderNum")
        dt.Columns.Add("Comment")

        aOrders = Split(strLateOrders, ";")

        For i = 0 To aOrders.Length - 2
            drrow = dt.Rows.Add
            drrow.Item("SO_ContiguousNum") = Left(aOrders(i), InStr(aOrders(i), ",") - 1)
            drrow.Item("SalesOrderNum") = Mid(aOrders(i), InStr(aOrders(i), ",") + 1)
            strComment = objSales.GetItemFromSalesMatrixDetails("LatePlanningComment", CInt(drrow.Item("SalesOrderNum")), Session("_VT_DotNetConnString"))
            If strComment = "" Then
                strComment = "[" & Session("_VT_CurrentUserName") & "] Late planning comment: "
                objSales.SaveGeneralItemsToSalesMatrix("LatePlanningComment", "Details", "[" & Session("_VT_CurrentUserName") & "] Late planning comment: ", CInt(drrow.Item("SalesOrderNum")), Session("_VT_DotNetConnString"))
                objSales.SaveGeneralItemsToSalesMatrix("LatePlanningComment_Name", "Details", Session("_VT_CurrentUserName"), CInt(drrow.Item("SalesOrderNum")), Session("_VT_DotNetConnString"))
                objSales.SaveGeneralItemsToSalesMatrix("LatePlanningComment_ID", "Details", CStr(Session("_VT_CurrentUserId")), CInt(drrow.Item("SalesOrderNum")), Session("_VT_DotNetConnString"))
                objSales.GetItemFromSalesMatrixDetails("LatePlanningComment", CInt(drrow.Item("SalesOrderNum")), Session("_VT_DotNetConnString"))

            End If
            drrow.Item("Comment") = strComment
        Next

        objDatapreserve.BindDataToWDG(dt, wdgOrdersGrid)
    End Sub

    Protected Sub cmdInputOK_Click(sender As Object, e As ImageClickEventArgs) Handles cmdInputOK.Click
        '    'audit log
        Dim strType As String
        Dim strAuditComment As String
        Dim objforms As New VT_Forms.Forms
        Dim objSales As New BPADotNetCommonFunctions.SalesFunctions.Sales_Orders
        Dim strCurrentSalesOrderNum As String = hdnSalesOrderNum.Value


        strAuditComment = "Late planning Comment: " & txtInputbox.Text
        strType = "Sales Order"
        objforms.WriteToAuditLog(CLng(strCurrentSalesOrderNum), strType, Now, Session("_VT_CurrentUserName"), Session("_VT_CurrentUserId"), strAuditComment, "Audit Record", "SysAdmin")

        'Save the Planning comment in the matrix

        objSales.SaveGeneralItemsToSalesMatrix("LatePlanningComment", "Details", "[" & Session("_VT_CurrentUserName") & "] " & strAuditComment, CInt(strCurrentSalesOrderNum), Session("_VT_DotNetConnString"))
        objSales.SaveGeneralItemsToSalesMatrix("LatePlanningComment_Name", "Details", Session("_VT_CurrentUserName"), CInt(strCurrentSalesOrderNum), Session("_VT_DotNetConnString"))
        objSales.SaveGeneralItemsToSalesMatrix("LatePlanningComment_ID", "Details", CStr(Session("_VT_CurrentUserId")), CInt(strCurrentSalesOrderNum), Session("_VT_DotNetConnString"))
        objSales.GetItemFromSalesMatrixDetails("LatePlanningComment", CInt(strCurrentSalesOrderNum), Session("_VT_DotNetConnString"))
        'rewrite the grid
        FillData()

    End Sub

    Protected Sub wdgOrdersGrid_ActiveCellChanged(sender As Object, e As ActiveCellEventArgs) Handles wdgOrdersGrid.ActiveCellChanged
        Dim dt As New DataTable
        Dim objC As New VT_CommonFunctions.CommonFunctions

        Dim intActiveRowIndex As Integer = e.CurrentActiveCell.Row.Index

        dt = objC.SerialiseWebDataGridRow(wdgOrdersGrid, intActiveRowIndex)
        Session("SelectedOrderRow") = dt

    End Sub

    Protected Sub btnAddNewComment_Click(sender As Object, e As ImageClickEventArgs) Handles btnAddNewComment.Click

        Dim dt As New DataTable

        If Not Session("SelectedOrderRow") Is Nothing Then
            dt = Session("SelectedOrderRow")

            lblInputMessage.Text = "Please add the reason for the late planning of order number: " & dt.Rows(0).Item("SO_ContiguousNum")
            hdnSalesOrderNum.Value = dt.Rows(0).Item("SalesOrderNum")
            txtInputbox.Text = ""
            ModalPopupExtenderInput.Show()

        End If

    End Sub

    Protected Sub btnBack1_Click(sender As Object, e As ImageClickEventArgs) Handles btnBack1.Click
        Response.Redirect("PlanningOrdersNew.aspx")

    End Sub
End Class



