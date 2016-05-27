Imports BPADotNetCommonFunctions
Imports BPADotNetCommonFunctions.PersonnelModuleFunctions
Imports BPADotNetCommonFunctions.VT_CommonFunctions.MatrixFunctions
Imports System.Drawing
Imports VTDBFunctions.VTDBFunctions
Imports BPADotNetCommonFunctions.VT_CommonFunctions
Imports System.Data
Imports AjaxControlToolkit

Partial Class Mobile_mDetailsMaster
    Inherits MyMasterPage

    Protected Sub btnTOP_Details_Click(sender As Object, e As EventArgs) Handles btnTOP_Details.Click
        GoToNewPage("mOrderDetails.aspx")
    End Sub
    Private Function CallContentPageBooleanFunction(ByVal FunctionName As String, Optional ByVal aParamArray() As String = Nothing) As Boolean
        '**********************************

        Dim Outer_CP As ContentPlaceHolder
        Outer_CP = TryCast(Me.FindControl("mProjectMAIN_content"), ContentPlaceHolder)


        CallContentPageBooleanFunction = CallByName(Outer_CP.Page, FunctionName, vbMethod, aParamArray)

    End Function
    Private Function CallContentPageStringFunction(ByVal FunctionName As String, Optional ByVal aParamArray() As String = Nothing) As String

        '**********************************
   Dim Outer_CP As ContentPlaceHolder
        Outer_CP = TryCast(Me.FindControl("mProjectMAIN_content"), ContentPlaceHolder)



        CallContentPageStringFunction = CallByName(Outer_CP.Page, FunctionName, vbMethod, aParamArray)

    End Function
    Private Sub CallContentPageSub(ByVal FunctionName As String, Optional ByVal aParamArray() As Object = Nothing)
        Dim Outer_CP As ContentPlaceHolder
        Outer_CP = TryCast(Me.FindControl("mProjectMAIN_content"), ContentPlaceHolder)


        CallByName(Outer_CP.Page, FunctionName, vbMethod, aParamArray)

    End Sub
    Function SavePageData(ByRef strMessage As String) As Boolean
        Dim intItemToSaveId As Integer
        Dim strFormName As String
        Dim objForms As New VT_Forms.Forms
        Dim strComment As String

        Dim objData As New BPADotNetCommonFunctions.VT_DataPreserve.DataPreserve
        Dim objVT As New VTDBFunctions.VTDBFunctions.VTMatrixFunctions
        Dim astrParam() As Object


        SavePageData = True

        strMessage = CallContentPageStringFunction("ValidateMe")
        If strMessage <> "" Then
            SavePageData = False
            Exit Function
        End If

        If CallContentPageBooleanFunction("DataToSave") = False Then
            SavePageData = True
            Exit Function
        End If

        ' get the record for this item using the Reference field
        Dim objMatrix As New VT_CommonFunctions.MatrixFunctions
        Dim strItemName As String = Me.CurrentSession.SelectedItemName
        Dim intItemId As Integer = Me.CurrentSession.SelectedItemID

        intItemToSaveId = Me.CurrentSession.SelectedItemID

        ' call the Form specific sub that stores any special data for the current form
        strMessage = CallContentPageStringFunction("StoreFormSpecificData")
        If strMessage <> "" Then
            SavePageData = False
            Exit Function
        End If


        SavePageData = True


        ' store the approval details if they are in use
        If Me.CurrentSession.ApprovalDetails.AllItems IsNot Nothing Then
            Dim objComm As New DataBaseFunctions.Commonfunctions

            objComm.StoreItemApprovalDetails(intItemToSaveId, Me.CurrentSession.ApprovalDetails)
        End If

        ' we need to cause the summary grid to be redrawn
        Me.CurrentSession.VT_CategoryGridData = Nothing

    End Function


    Sub GoToNewPage(ByVal strPage As String)
        Dim strMessage As String = ""

        Dim blnOKToChangePage As Boolean = True
        blnOKToChangePage = SavePageData(strMessage)
        'blnOKToChangePage = HandleSaveButton()

        If blnOKToChangePage Then

            Response.Redirect(strPage)

        Else
            lblMessage.Text = strMessage

        End If

    End Sub

    Private Sub btnTOP_Products_Click(sender As Object, e As EventArgs) Handles btnTOP_Products.Click
        GoToNewPage("mOrderProducts.aspx")
    End Sub

    Private Sub btnTOP_Payment_Click(sender As Object, e As EventArgs) Handles btnTOP_Payment.Click
        GoToNewPage("mOrderPayment.aspx")
    End Sub


End Class

