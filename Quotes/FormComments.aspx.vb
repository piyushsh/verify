Imports System.Drawing

Partial Class FormComments
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

            strTitle = Me.CurrentSession.VT_QuoteNum


            lblThreadsTitle.Text = strTitle

            ' load the application threads grid
            Dim objApp As New CommentFunctions
            Dim intIdToUse As Integer
         
            intIdToUse = Me.CurrentSession.VT_QuoteNum


            Dim dtThreads As Data.DataTable = objApp.GetThreads(intIdToUse)
            'If Me.CurrentSession.CurrentUserCategory = GetGlobalResourceObject("Resource", "UserCategory_Applicants") Then
            '    ' if the current user is an applicant filter the list to show only the appropriate threads 
            '    dtThreads.DefaultView.RowFilter = "Field10 = 'YES'"
            'End If
            'If Me.CurrentSession.CurrentUserCategory = GetGlobalResourceObject("Resource", "UserCategory_Reviewers") Then
            '    ' if the current user is a reviewer filter the list to show only the appropriate threads 
            '    dtThreads.DefaultView.RowFilter = "Field11 = 'YES'"
            'End If


            ApplicationThreads.DataSource = dtThreads
            ApplicationThreads.DataBind()

            FormatThreadsTable()

            If ApplicationThreads.Rows.Count > 0 Then
                ApplicationThreads.DisplayLayout.ActiveRow = ApplicationThreads.Rows(0)

                Dim intThreadId As Integer = ApplicationThreads.DisplayLayout.ActiveRow.Cells.FromKey("Id").Value
                ShowCommentsForThread(intThreadId)

            End If

            'If Me.CurrentSession.CurrentUserCategory = GetGlobalResourceObject("Resource", "UserCategory_Administrators") Then
            '    ' set up the Comment Target dropdown
            '    lblCommentTarget.Visible = True
            '    ddlCommentTarget.Visible = True
            '    ddlCommentTarget.Items.Clear()
            '    ddlCommentTarget.Items.Add(GetGlobalResourceObject("Resource", "UserCategory_Reviewers"))
            '    ddlCommentTarget.Items.Add(GetGlobalResourceObject("Resource", "UserCategory_Applicants"))

            '    ' set up the New thread Target dropdown
            '    lblNewThreadTarget.Visible = True
            '    ddlNewThreadTarget.Visible = True
            '    ddlNewThreadTarget.Items.Clear()
            '    ddlNewThreadTarget.Items.Add(GetGlobalResourceObject("Resource", "UserCategory_Reviewers"))
            '    ddlNewThreadTarget.Items.Add(GetGlobalResourceObject("Resource", "UserCategory_Applicants"))
            'Else
            '    lblCommentTarget.Visible = False
            '    ddlCommentTarget.Visible = False

            '    lblNewThreadTarget.Visible = False
            '    ddlNewThreadTarget.Visible = False
            'End If

            ' load the NoThreadSelected text from the resource file into 
            ' the hidden field from where the script can access it
            hdnNoPageForComment.Value = GetGlobalResourceObject("Resource", "NoPageForComment")
            hdnNoThreadSelected.Value = GetGlobalResourceObject("Resource", "NoThreadSelected")
            hdnNoCommentPrompt.Value = GetGlobalResourceObject("Resource", "NoCommentSupplied")
            hdnNoCommentSelected.Value = GetGlobalResourceObject("Resource", "NoCommentSelected")

        End If
    End Sub

    Protected Sub btnBack_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnBack.Click
        Response.Redirect(Me.CurrentSession.OptionsPageToReturnTo)
    End Sub

    Sub FormatThreadsTable()
        For Each ThisRow As Infragistics.WebUI.UltraWebGrid.UltraGridRow In ApplicationThreads.Rows
            ' display the dates in ShortDate format
            ThisRow.Cells.FromKey("DateCreated").Value = FormatDateTime(ThisRow.Cells.FromKey("DateCreated").Value, DateFormat.GeneralDate)
        Next
    End Sub
    Sub FormatCommentsTable()
        For Each ThisRow As Infragistics.WebUI.UltraWebGrid.UltraGridRow In ApplicationComments.Rows
            ' display the dates in ShortDate format
            ThisRow.Cells.FromKey("DateCreated").Value = FormatDateTime(ThisRow.Cells.FromKey("DateCreated").Value, DateFormat.GeneralDate)
            If ThisRow.Cells.FromKey("DateCleared").Value <> "" Then
                ThisRow.Cells.FromKey("DateCleared").Value = FormatDateTime(ThisRow.Cells.FromKey("DateCleared").Value, DateFormat.GeneralDate)
                ' show the Cleared comments in a different color
                ThisRow.Style.ForeColor = Color.Wheat
                ThisRow.Style.BackColor = Color.Green
            End If
        Next
    End Sub

    Protected Sub ApplicationThreads_Click(ByVal sender As Object, ByVal e As Infragistics.WebUI.UltraWebGrid.ClickEventArgs) Handles ApplicationThreads.Click
        'get the comments for the selected thread
        If ApplicationThreads.DisplayLayout.ActiveRow IsNot Nothing Then
            Dim intThreadId As Integer = ApplicationThreads.DisplayLayout.ActiveRow.Cells.FromKey("Id").Value
            ShowCommentsForThread(intThreadId)
        End If
    End Sub

    Sub ShowCommentsForThread(ByVal intThreadId As Integer)
        Dim objApp As New CommentFunctions

        Dim dtComments As Data.DataTable = objApp.GetThreadComments(intThreadId)


        'If Me.CurrentSession.CurrentUserCategory <> GetGlobalResourceObject("Resource", "UserCategory_Administrators") Then
        '    ' if the current user is not an administrator filter the list to show only the appropriate threads 
        '    dtComments.DefaultView.RowFilter = "Field8='" + Me.CurrentSession.CurrentUserCategory + "' OR Field9='" + Me.CurrentSession.CurrentUserCategory + "'"
        'End If

        ApplicationComments.DataSource = dtComments
        ApplicationComments.DataBind()

        FormatCommentsTable()


    End Sub

    Sub RefreshPage()
        ' load the application threads grid
        Dim objApp As New CommentFunctions

        Dim intIdToUse As Integer
      
        intIdToUse = Me.CurrentSession.VT_QuoteNum


        Dim dtThreads As Data.DataTable = objApp.GetThreads(intIdToUse)
        'If Me.CurrentSession.CurrentUserCategory = GetGlobalResourceObject("Resource", "UserCategory_Applicants") Then
        '    ' if the current user is not an applicant filter the list to show only the appropriate threads 
        '    dtThreads.DefaultView.RowFilter = "Field10 = 'YES'"
        'End If
        'If Me.CurrentSession.CurrentUserCategory = GetGlobalResourceObject("Resource", "UserCategory_Reviewers") Then
        '    ' if the current user is a reviewer filter the list to show only the appropriate threads 
        '    dtThreads.DefaultView.RowFilter = "Field11 = 'YES'"
        'End If


        ApplicationThreads.DataSource = dtThreads
        ApplicationThreads.DataBind()

        FormatThreadsTable()

        If ApplicationThreads.Rows.Count > 0 Then
            ApplicationThreads.DisplayLayout.ActiveRow = ApplicationThreads.Rows(0)

            Dim intThreadId As Integer = ApplicationThreads.DisplayLayout.ActiveRow.Cells.FromKey("Id").Value
            ShowCommentsForThread(intThreadId)

        End If

    End Sub

    Protected Sub cmdNewOk_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles cmdNewOk.Click
        ' add the new comment here
        Dim objApp As New CommentFunctions

        If ApplicationThreads.DisplayLayout.ActiveRow IsNot Nothing Then
            Dim intThreadId As Integer = ApplicationThreads.DisplayLayout.ActiveRow.Cells.FromKey("Id").Value
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

            Dim strPage As String = ApplicationThreads.DisplayLayout.ActiveRow.Cells.FromKey("hdnPage").Value

            Dim intIdToUse As Integer
         
            intIdToUse = Me.CurrentSession.VT_QuoteNum


            objApp.StoreComment(intIdToUse, txtComment.Text, strPage, Nothing, strCommentSource, strCommentTarget, intCommentSourceId, intCommentTargetId, intThreadId)

            RefreshPage()
        Else
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

            '  Dim strPage As String = ApplicationThreads.DisplayLayout.ActiveRow.Cells.FromKey("hdnPage").Value


            Dim intIdToUse As Integer
         
            intIdToUse = Me.CurrentSession.VT_QuoteNum

            objApp.StoreComment(intIdToUse, txtComment.Text, "", Nothing, strCommentSource, strCommentTarget, intCommentSourceId, intCommentTargetId)
            RefreshPage()
        End If
    End Sub

    Protected Sub btnClearComment_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnClearComment.Click
        If ApplicationThreads.DisplayLayout.ActiveRow IsNot Nothing Then
            Dim intThreadId As Integer = ApplicationThreads.DisplayLayout.ActiveRow.Cells.FromKey("Id").Value
            If ApplicationComments.DisplayLayout.ActiveRow IsNot Nothing Then

                Dim strCommentId As String = ApplicationComments.DisplayLayout.ActiveRow.Cells.FromKey("Id").Value
                Dim objApp As New CommentFunctions

                objApp.ClearComment(CInt(strCommentId))

                ShowCommentsForThread(intThreadId)
            End If
        End If


    End Sub

    Protected Sub cmdClearAllComments_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles cmdClearAllComments.Click
        Dim objApp As New CommentFunctions


        If ApplicationThreads.DisplayLayout.ActiveRow IsNot Nothing Then
            For Each ThisRow As Infragistics.WebUI.UltraWebGrid.UltraGridRow In ApplicationComments.DisplayLayout.Rows
                If ThisRow.Cells.FromKey("DateCleared").Value = "" Then
                    objApp.ClearComment(CInt(ThisRow.Cells.FromKey("Id").Value))
                End If
            Next

            Dim intThreadId As Integer = ApplicationThreads.DisplayLayout.ActiveRow.Cells.FromKey("Id").Value

            ShowCommentsForThread(intThreadId)
        End If

    End Sub


    Protected Sub cmdNewThreadOk_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles cmdNewThreadOk.Click
        Dim objApp As New CommentFunctions
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


        Dim intIdToUse As Integer
        
        intIdToUse = Me.CurrentSession.VT_QuoteNum


        objApp.StoreComment(intIdToUse, txtNewThreadComment.Text, "", Nothing, strCommentSource, strCommentTarget, intCommentSourceId, intCommentTargetId)
        RefreshPage()

    End Sub

    Protected Sub btnGoToControl_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnGoToControl.Click
        Dim strCommentPage As String = ApplicationThreads.DisplayLayout.ActiveRow.Cells.FromKey("hdnPage").Value
        Dim strControltoGoTo As String = ApplicationThreads.DisplayLayout.ActiveRow.Cells.FromKey("hdnControl").Value
        Me.CurrentSession.VT_AppPageToShow = strCommentPage
        Me.CurrentSession.VT_ControlToJumpTo = strControltoGoTo
        Response.Redirect(strCommentPage)

    End Sub

   
End Class
