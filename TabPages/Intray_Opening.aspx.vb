Imports BPADotNetCommonFunctions

Partial Class Intray_Opening
    Inherits MyBasePage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            ddlType.SelectedRow = ddlType.Rows(0)
            FillGrid()
        End If
    End Sub
    Sub LaunchModulePages()
        Const NumberOfTabPages As Integer = 3
        Dim astrOptions(NumberOfTabPages - 1) As String
        Dim astrOptionPages(NumberOfTabPages - 1) As String



        ' insert the pages in the arrays in the order of the OrderNum field
        astrOptions(0) = "Job Details"
        astrOptionPages(0) = "~/Job_Pages/JobDetails.aspx"

        astrOptions(1) = "History"
        astrOptionPages(1) = "~/Job_Pages/JobHistory.aspx"

        astrOptions(2) = "Forms"
        astrOptionPages(2) = "~/Job_Pages/JobDetails.aspx"



        Session("_VT_DetailsPageOptions") = astrOptions

        Session("_VT_DetailsPageOptionsPages") = astrOptionPages

        Response.Redirect(astrOptionPages(0))

    End Sub
    Protected Sub btnAction_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Me.CurrentSession.VT_CameFromInTray = "YES"
        Session("VT_CameFromInTray") = "YES"
        Me.CurrentSession.VT_CurrentTaskId = CInt(uwgMyTasks.DisplayLayout.ActiveRow.Cells.FromKey("TaskID").Text)
        Me.CurrentSession.VT_JobID = uwgMyTasks.DisplayLayout.ActiveRow.Cells.FromKey("HiddenJobId").Text

        Dim strNextPage = uwgMyTasks.DisplayLayout.ActiveRow.Cells.FromKey("HiddenNextPage").Text
        'Response.Redirect(strNextPage)
        LaunchModulePages()


    End Sub

    Protected Sub ddlType_SelectedRowChanged(ByVal sender As Object, ByVal e As Infragistics.WebUI.WebCombo.SelectedRowChangedEventArgs) Handles ddlType.SelectedRowChanged
        FillGrid()
    End Sub

    Sub FillGrid()
        Dim objVT As New VT_eQOInterface.eQOInterface
        Dim objPers As New PersonnelModuleFunctions
        Dim objCommon As New TTWFOCommonFunctions.TOFunctions
        Dim objForms As New BPADotNetCommonFunctions.VT_Forms.Forms

        Dim ds As New Data.DataSet
        If rblType.SelectedValue = 0 Then
            ds = objVT.GetTasksForUser(Session("_VT_CurrentUserId"))

            ' display the Sender name
            uwgMyTasks.Columns.FromKey("UserID").BaseColumnName = "OriginatorID"
            uwgMyTasks.Columns.FromKey("UserID").Header.Caption = "Activity Sender:"

            ' show the action column
            uwgMyTasks.Columns.FromKey("Action").Hidden = False
        Else
            ds = objVT.GetTasksWhereUserIsOriginator(Session("_VT_CurrentUserId"))
            ' display who the task was sent to
            uwgMyTasks.Columns.FromKey("UserID").BaseColumnName = "eQOID"
            uwgMyTasks.Columns.FromKey("UserID").Header.Caption = "Activity Sent To:"

            ' hide the action column
            uwgMyTasks.Columns.FromKey("Action").Hidden = True
        End If

        Select Case ddlType.SelectedCell.Text
            Case "Open Activities"
                ds.Tables(0).DefaultView.RowFilter = "IsNull(DateCompleted, '#01/01/99#') = '#01/01/99#'"
            Case "Completed Activities"
                ds.Tables(0).DefaultView.RowFilter = "IsNull(DateCompleted, '#01/01/99#') <> '#01/01/99#'"
        End Select

        uwgMyTasks.DataSource = ds
        uwgMyTasks.DataBind()

        If uwgMyTasks.Rows.Count = 0 Then
            lblNotfound.Visible = True
            lblNotfound.Text = "You have no tasks of the selected type in the system at this time."
        Else
            lblNotfound.Visible = False
            ' now go through the grid  
            For Each row As Infragistics.WebUI.UltraWebGrid.UltraGridRow In uwgMyTasks.Rows
                Dim intJobId As Integer = row.Cells.FromKey("JobId").Value
                'fill the status cell based on whether the DateCompleted is Null or not
                If IsNothing(row.Cells.FromKey("HiddenDateCompleted").Value) Then
                    row.Cells.FromKey("Status").Value = "Open"
                Else
                    row.Cells.FromKey("Status").Value = "Closed"
                End If

                ' fill in the Job status
                If objForms.GetJobStatus(intJobId) <> "" Then
                    row.Cells.FromKey("Status").Value = objForms.GetJobStatus(intJobId)
                End If


                ' find the UserName for the stored OriginatorID
                If row.Cells.FromKey("UserId").Value IsNot Nothing And row.Cells.FromKey("UserId").Value <> 0 Then
                    ' get the user name from the ID
                    Dim astrData(1) As String
                    astrData(0) = "txtFirstName"
                    astrData(1) = "txtSurname"
                    Dim dt As Data.DataTable = objPers.GetPersonnelDataItems(Session("_VT_DotNetConnString"), row.Cells.FromKey("UserId").Value, astrData)

                    row.Cells.FromKey("UserId").Value = dt.Rows(0).Item("Field0") + " " + dt.Rows(0).Item("Field1")
                Else
                    row.Cells.FromKey("UserId").Value = ""
                End If


            Next
        End If

    End Sub


    Protected Sub rblType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rblType.SelectedIndexChanged
        FillGrid()
    End Sub


End Class
