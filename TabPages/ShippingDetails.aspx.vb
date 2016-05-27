Imports BPADotNetCommonFunctions.VT_CommonFunctions.MatrixFunctions

Partial Class TabPages_ShippingDetails
    Inherits MyBasePage

    Private Sub TabPages_ShippingDetails_Load(sender As Object, e As EventArgs) Handles Me.Load
        Dim objData As New BPADotNetCommonFunctions.VT_DataPreserve.DataPreserve

        ' Outer Master Page
        Dim Outer_CP As ContentPlaceHolder
        Outer_CP = TryCast(Me.Master.FindControl("ProjectMAIN_content"), ContentPlaceHolder)

        Dim objMatrix As New VTDBFunctions.VTDBFunctions.VTMatrixFunctions
        Dim objD As New BPADotNetCommonFunctions.VT_DataPreserve.DataPreserve

        Dim lngRootId As Long
        Dim lngOrderFolder As Long
        Dim lngOrderCategory As Long
        Dim intMatrixLink As Integer
        Dim lngOrderItemsForm As Long
        Dim objDBConn As New VTDBFunctions.VTDBFunctions.DBConn

        Dim strConn As String = objDBConn.SQLConnStringForDotNet
        Me.CurrentSession.VT_SelectedFormName = "ShippingDetails"

        lblOrderNum.Text = Me.CurrentSession.VT_SalesContiguousNum

        lngRootId = objMatrix.GetFormId(strConn, SalesOrderFormListTable, SalesOrderModuleRootNode, 0)
        lngOrderFolder = objMatrix.GetFormId(strConn, SalesOrderFormListTable, SalesOrderModuleCategoriesNode, lngRootId)
        lngOrderCategory = objMatrix.GetFormId(strConn, SalesOrderFormListTable, "Standard", lngOrderFolder)
        intMatrixLink = objMatrix.GetFormId(strConn, SalesOrderFormListTable, CStr(Me.CurrentSession.VT_SalesOrderNum), lngOrderCategory)

        'Write out the grid data under the specified Node
        lngOrderItemsForm = objMatrix.GetFormId(strConn, SalesOrderFormListTable, "ShippingDetails", intMatrixLink)

        objData.RecoverFormData(Outer_CP, Me.CurrentSession.VT_SelectedFormName, intMatrixLink, "tls_FormList", "tls_FormData")

    End Sub
End Class
