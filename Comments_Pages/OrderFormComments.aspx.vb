Imports BPADotNetCommonFunctions
Imports System.Drawing

Partial Class Order_OrderFormComment
    Inherits MyBasePage



    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'ScriptManager1.RegisterAsyncPostBackControl(btnAddComment)
        'ScriptManager1.RegisterAsyncPostBackControl(btnNewThread)



        btnAddComment.Attributes.Add("onclick", "return LoadCommentPanel();")
        btnNewThread.Attributes.Add("onclick", "return LoadNewThreadPanel();")
        cmdNewOk.OnClientClick = String.Format("AddNewCommentClickUpdate('{0}','{1}')", cmdNewOk.UniqueID, "")
        cmdNewThreadOk.OnClientClick = String.Format("AddNewThreadClickUpdate('{0}','{1}')", cmdNewThreadOk.UniqueID, "")

        btnGoToControl.Attributes.Add("onclick", "return goToCommentControl();")
        btnClearComment.Attributes.Add("onclick", "return ClearComment();")

        If Not IsPostBack Then
            ' load the Comments panel title
            Dim strTitle As String
            'strTitle = GetGlobalResourceObject("Resource", "ThreadsTitle1") + GetGlobalResourceObject("Resource", "Course") + " - " + Me.CurrentSession.VT_CurrentCourseName + _
            '    ", " + GetGlobalResourceObject("Resource", "ApplicationId") + " - " + Me.CurrentSession.SelectedItemID.ToString

            strTitle = Me.CurrentSession.VT_NewOrderNum


            lblThreadsTitle.Text = strTitle

            ' load the application threads grid
            RefreshPage()


            ' load the NoThreadSelected text from the resource file into 
            ' the hidden field from where the script can access it
            hdnNoPageForComment.Value = GetGlobalResourceObject("Resource", "NoPageForComment")
            hdnNoThreadSelected.Value = GetGlobalResourceObject("Resource", "NoThreadSelected")
            hdnNoCommentPrompt.Value = GetGlobalResourceObject("Resource", "NoCommentSupplied")
            hdnNoCommentSelected.Value = GetGlobalResourceObject("Resource", "NoCommentSelected")
        Else
            Dim objData As New BPADotNetCommonFunctions.VT_DataPreserve.DataPreserve
            wdgApplicationThreads.DataSource = objData.GetWDGDataFromSession(wdgApplicationThreads)
            wdgApplicationComments.DataSource = objData.GetWDGDataFromSession(wdgApplicationComments)

        End If
    End Sub

    Protected Sub btnBack_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnBack.Click

        Response.Redirect(Me.CurrentSession.OptionsPageToReturnTo)

    End Sub



    Protected Sub wdgApplicationThreads_ActiveCellChanged(sender As Object, e As Infragistics.Web.UI.GridControls.ActiveCellEventArgs) Handles wdgApplicationThreads.ActiveCellChanged
        Dim objC As New VT_CommonFunctions.CommonFunctions
        Dim intRowIndex As Integer = e.CurrentActiveCell.Row.Index
        Dim dtGridRow As Data.DataTable = objC.SerialiseWebDataGridRow(wdgApplicationThreads, intRowIndex)

        Dim intThreadId As Integer = dtGridRow.Rows(0).Item("Id")
        ShowCommentsForThread(intThreadId)


    End Sub


    Sub ShowCommentsForThread(ByVal intThreadId As Integer)
        Dim objApp As New CommentFunctions

        Dim dtComments As Data.DataTable = objApp.GetThreadComments(intThreadId)

        ' change the dates in the data from strings to dates
        'Dim astrColNames(1, 1) As String
        'astrColNames(0, 0) = "Field4"
        'astrColNames(0, 1) = "CONVERT_TO_DATE"
        'astrColNames(1, 0) = "Field2"
        'astrColNames(1, 1) = "CONVERT_TO_DATE"
        'Dim objG As New BPADotNetCommonFunctions.VT_ReportFunctions.GeneralFunctions
        'dtComments = objG.CleanColumnFormats(dtComments, astrColNames)


        Dim objData As New BPADotNetCommonFunctions.VT_DataPreserve.DataPreserve
        objData.BindDataToWDG(dtComments, wdgApplicationComments)


    End Sub

    Sub RefreshPage()
        ' load the application threads grid
        Dim objApp As New CommentFunctions

        Dim intIdToUse As Integer
        intIdToUse = Me.CurrentSession.VT_NewOrderMatrixID

        Dim dtThreads As Data.DataTable = objApp.GetThreads(intIdToUse)

        ' change the dates in the data from strings to dates
        'Dim astrColNames(0, 1) As String
        'astrColNames(0, 0) = "Field2"
        'astrColNames(0, 1) = "CONVERT_TO_DATE"
        'Dim objG As New BPADotNetCommonFunctions.VT_ReportFunctions.GeneralFunctions
        'dtThreads = objG.CleanColumnFormats(dtThreads, astrColNames)

        Dim objData As New BPADotNetCommonFunctions.VT_DataPreserve.DataPreserve
        objData.BindDataToWDG(dtThreads, wdgApplicationThreads)


        If wdgApplicationThreads.Rows.Count > 0 Then
            Dim intRowIndex As Integer = 0
            Dim objC As New VT_CommonFunctions.CommonFunctions
            Dim dtGridRow As Data.DataTable = objC.SerialiseWebDataGridRow(wdgApplicationThreads, intRowIndex)

            wdgApplicationThreads.Behaviors.Activation.ActiveCell = wdgApplicationThreads.Rows(0).Items(0)
            Dim intThreadId As Integer = dtGridRow.Rows(0).Item("Id")
            ShowCommentsForThread(intThreadId)

        End If

    End Sub

    Protected Sub cmdNewOk_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles cmdNewOk.Click
        ' add the new comment here

        Dim objApp As New CommentFunctions

        ' javascript will already have checked that we have a valid thread to which we can attach the comment but check again here for safety
        If wdgApplicationThreads.Behaviors.Activation.ActiveCell IsNot Nothing Then
            ' add a Comment to the selected Thread
            Dim objC As New VT_CommonFunctions.CommonFunctions
            Dim intRowIndex As Integer = wdgApplicationThreads.Behaviors.Activation.ActiveCell.Row.Index
            Dim dtGridRow As Data.DataTable = objC.SerialiseWebDataGridRow(wdgApplicationThreads, intRowIndex)

            Dim intThreadId As Integer = dtGridRow.Rows(0).Item("Id")
            Dim strCommentTarget As String
            Dim intCommentTargetId As Integer
            If ddlCommentTarget.SelectedValue = "" Then
                strCommentTarget = GetGlobalResourceObject("Resource", "Everybody")
                intCommentTargetId = 0
            Else
                strCommentTarget = ddlCommentTarget.SelectedItem.Text
                intCommentTargetId = ddlCommentTarget.SelectedValue
            End If

            Dim strCommentSource As String = Session("_VT_CurrentUserName")
            Dim intCommentSourceId As Integer = Session("_VT_CurrentUserId")



            Dim strPage As String = dtGridRow.Rows(0).Item("hdnPage")

            Dim intIdToUse As Integer
            intIdToUse = Me.CurrentSession.VT_NewOrderMatrixID

            objApp.StoreComment(intIdToUse, txtComment.Text, strPage, Nothing, strCommentSource, strCommentTarget, intCommentSourceId, intCommentTargetId, intThreadId)

            ShowCommentsForThread(intThreadId)
        End If

    End Sub

    Protected Sub btnClearComment_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnClearComment.Click
        If wdgApplicationThreads.Behaviors.Activation.ActiveCell IsNot Nothing Then
            Dim objC As New VT_CommonFunctions.CommonFunctions
            Dim intRowIndex As Integer = wdgApplicationThreads.Behaviors.Activation.ActiveCell.Row.Index
            Dim dtGridRow As Data.DataTable = objC.SerialiseWebDataGridRow(wdgApplicationThreads, intRowIndex)

            Dim intThreadId As Integer = dtGridRow.Rows(0).Item("Id")

            If wdgApplicationComments.Behaviors.Activation.ActiveCell IsNot Nothing Then
                intRowIndex = wdgApplicationComments.Behaviors.Activation.ActiveCell.Row.Index
                Dim dtCommentGridRow As Data.DataTable = objC.SerialiseWebDataGridRow(wdgApplicationComments, intRowIndex)

                Dim intCommentId As Integer = dtCommentGridRow.Rows(0).Item("Id")

                Dim objApp As New CommentFunctions
                objApp.ClearComment(intCommentId)

                ShowCommentsForThread(intThreadId)
            End If
        End If



    End Sub

    Protected Sub cmdClearAllComments_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles cmdClearAllComments.Click
        Dim objApp As New CommentFunctions

        If wdgApplicationThreads.Behaviors.Activation.ActiveCell IsNot Nothing Then
            Dim objC As New VT_CommonFunctions.CommonFunctions
            Dim intRowIndex As Integer = wdgApplicationThreads.Behaviors.Activation.ActiveCell.Row.Index
            Dim dtGridRow As Data.DataTable = objC.SerialiseWebDataGridRow(wdgApplicationThreads, intRowIndex)

            Dim intThreadId As Integer = dtGridRow.Rows(0).Item("Id")

            For Each ThisRow As Infragistics.Web.UI.GridControls.GridRecord In wdgApplicationComments.Rows
                If ThisRow.Items.FindItemByKey("DateCleared").Value = "" Then
                    objApp.ClearComment(CInt(ThisRow.Items.FindItemByKey("Id").Value))
                End If
            Next


            ShowCommentsForThread(intThreadId)
        End If

    End Sub


    Protected Sub cmdNewThreadOk_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles cmdNewThreadOk.Click
        Dim objApp As New CommentFunctions
        Dim strCommentTarget As String
        Dim intCommentTargetId As Integer
        If ddlThreadTarget.SelectedValue = "" Then
            strCommentTarget = GetGlobalResourceObject("Resource", "Everybody")
            intCommentTargetId = 0
        Else
            strCommentTarget = ddlThreadTarget.SelectedItem.Text
            intCommentTargetId = ddlThreadTarget.SelectedValue
        End If

        Dim strCommentSource As String = Session("_VT_CurrentUserName")
        Dim intCommentSourceId As Integer = Session("_VT_CurrentUserId")
        Dim intIdToUse As Integer

        intIdToUse = Me.CurrentSession.VT_NewOrderMatrixID


        objApp.StoreComment(intIdToUse, txtNewThreadComment.Text, "", Nothing, strCommentSource, strCommentTarget, intCommentSourceId, intCommentTargetId)
        RefreshPage()

    End Sub

    Protected Sub btnGoToControl_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnGoToControl.Click
        If wdgApplicationThreads.Behaviors.Activation.ActiveCell IsNot Nothing Then
            Dim objC As New VT_CommonFunctions.CommonFunctions
            Dim intRowIndex As Integer = wdgApplicationThreads.Behaviors.Activation.ActiveCell.Row.Index
            Dim dtGridRow As Data.DataTable = objC.SerialiseWebDataGridRow(wdgApplicationThreads, intRowIndex)

            Dim strCommentPage As String = dtGridRow.Rows(0).Item("hdnPage")
            Dim strControltoGoTo As String = dtGridRow.Rows(0).Item("hdnControl")
            'Me.CurrentSession.VT_AppPageToShow = strCommentPage
            Me.CurrentSession.VT_ControlToJumpTo = strControltoGoTo
            Response.Redirect(strCommentPage)
        End If


    End Sub


End Class
