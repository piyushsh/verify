Imports Microsoft.VisualBasic
Imports BPADotNetCommonFunctions

Public Class CommentFunctions
    Inherits MyBasePage

    Function GetThreads(ByVal intFormId As Integer) As Data.DataTable

        Dim objMatrix As New VTDBFunctions.VTDBFunctions.VTMatrixFunctions
        Dim intThreadsFolderId As Integer = objMatrix.GetFormId(Session("_VT_DotNetConnString"), Me.CurrentSession.FormListTable, "Threads", intFormId)

        ' get all comments for this application 
        Dim intNumFields As Integer = 12
        Dim astrItemNames(intNumFields - 1) As String
        Dim ds As New Data.DataSet

        astrItemNames(0) = "ItemId"
        'astrItemNames(1) = "InitialComment"
        'astrItemNames(2) = "DateCreated"
        astrItemNames(1) = "LatestComment"
        astrItemNames(2) = "DateLatestCreated"
        astrItemNames(3) = "CreatedBy"
        astrItemNames(4) = "DateCleared"
        astrItemNames(5) = "ClearedBy"
        astrItemNames(6) = "Page"
        astrItemNames(7) = "ControlForThread"
        astrItemNames(8) = "FieldLinked"
        astrItemNames(9) = "NumComments"
        astrItemNames(10) = "VisibleToApplicant"
        astrItemNames(11) = "VisibleToReviewer"

        ds = objMatrix.GetVTMatrixData(Session("_VT_DotNetConnString"), Me.CurrentSession.FormDataTable, intNumFields, astrItemNames, "ParentId", intThreadsFolderId)
        Dim dv As Data.DataView = ds.Tables(0).DefaultView
        dv.Sort = "Field2 desc"

        GetThreads = dv.Table


    End Function

    Function GetThreadDetails(ByVal intThreadId As Integer) As Data.DataTable

        Dim objMatrix As New VTDBFunctions.VTDBFunctions.VTMatrixFunctions

        ' get all comments for this application 
        Dim intNumFields As Integer = 11
        Dim astrItemNames(intNumFields - 1) As String
        Dim ds As New Data.DataSet

        astrItemNames(0) = "ItemId"
        'astrItemNames(1) = "InitialComment"
        'astrItemNames(2) = "DateCreated"
        astrItemNames(1) = "LatestComment"
        astrItemNames(2) = "DateLatestCreated"
        astrItemNames(3) = "CreatedBy"
        astrItemNames(4) = "DateCleared"
        astrItemNames(5) = "ClearedBy"
        astrItemNames(6) = "Page"
        astrItemNames(7) = "ControlForThread"
        astrItemNames(8) = "Target"
        astrItemNames(9) = "FieldLinked"
        astrItemNames(10) = "NumComments"

        ds = objMatrix.GetVTMatrixData(Session("_VT_DotNetConnString"), Me.CurrentSession.FormDataTable, intNumFields, astrItemNames, "ItemId", intThreadId)

        GetThreadDetails = ds.Tables(0)
    End Function

    Function GetThreadComments(ByVal intThreadId As Integer) As Data.DataTable

        Dim objMatrix As New VTDBFunctions.VTDBFunctions.VTMatrixFunctions

        ' get all comments for this application 
        Dim intNumFields As Integer = 12
        Dim astrItemNames(intNumFields - 1) As String
        Dim ds As New Data.DataSet

        astrItemNames(0) = "ItemId"
        astrItemNames(1) = "Comment"
        astrItemNames(2) = "DateCreated"
        astrItemNames(3) = "CreatedBy"
        astrItemNames(4) = "DateCleared"
        astrItemNames(5) = "ClearedBy"
        astrItemNames(6) = "Page"
        astrItemNames(7) = "Control"
        astrItemNames(8) = "Source"
        astrItemNames(9) = "Target"
        astrItemNames(10) = "SourceId"
        astrItemNames(11) = "TargetId"

        ds = objMatrix.GetVTMatrixData(Session("_VT_DotNetConnString"), Me.CurrentSession.FormDataTable, intNumFields, astrItemNames, "ParentId", intThreadId)
        Dim dv As Data.DataView = ds.Tables(0).DefaultView
        dv.Sort = "Field2 desc"

        GetThreadComments = dv.Table


    End Function


    Sub StoreComment(ByVal intFormId As Integer, ByVal strComment As String, ByVal strPage As String, ByVal TheControl As Control, ByVal strCommentSource As String, ByVal strCommentTarget As String, ByVal intCommentSourceId As Integer, ByVal intCommentTargetId As Integer, Optional ByVal intThreadId As Integer = 0)
        ' get the comments folder for this application
        Dim objMatrix As New VTDBFunctions.VTDBFunctions.VTMatrixFunctions
        Dim intThreadsFolderId As Integer = objMatrix.GetFormId(Session("_VT_DotNetConnString"), Me.CurrentSession.FormListTable, "Threads", intFormId)
        Dim strTemp As String
        Dim dtThread As Data.DataTable

        ' find if there is an existing thread for this comment
        Dim blnNewThread As Boolean = True
        Dim intThisThreadId As Integer
        If intThreadId <> 0 Then
            ' we have been supplied with a thread to attach the comment to
            blnNewThread = False
            intThisThreadId = intThreadId
        ElseIf Not (TheControl Is Nothing) Then
            ' we have a control to attach the comment to - does it have an existing thread
            Dim dtThreads As Data.DataTable = GetThreads(intFormId)
            ' Filter the threads on the Control name
            dtThreads.DefaultView.RowFilter = "Field7='" + TheControl.ID + "'"
            Dim dv As Data.DataView = dtThreads.DefaultView

            If dv.Count = 0 Then
                ' there is no thread for this control so we need a new thread
                blnNewThread = True
            Else
                intThisThreadId = dv.Item(0).Row.Item("Field0")
                blnNewThread = False
            End If
        Else
            ' we have no thread and no control to check against so we need a new thread
            blnNewThread = True
        End If

        If blnNewThread Then

            'SmcN 22/04/2014 Modified this section to take account of creating a VT_SYSTEM_THREAD Node

            If strCommentSource = "[System]" Then
                strTemp = "VT_SYSTEM_THREAD"
            Else
                ' use the Date/time as a unique name for the thread
                strTemp = "Thread" + Format(PortalFunctions.Now, "ddMMHHmmss")
            End If

            intThisThreadId = objMatrix.GetFormId(Session("_VT_DotNetConnString"), Me.CurrentSession.FormListTable, strTemp, intThreadsFolderId)

            objMatrix.InsertVTMatrixFormDataItem(Session("_VT_DotNetConnString"), Me.CurrentSession.FormDataTable, intThisThreadId, "ItemId", intThisThreadId.ToString)
            objMatrix.InsertVTMatrixFormDataItem(Session("_VT_DotNetConnString"), Me.CurrentSession.FormDataTable, intThisThreadId, "ParentId", intThreadsFolderId.ToString)
            ' store the first 100 chars of the comment as the thread title
            objMatrix.InsertVTMatrixFormDataItem(Session("_VT_DotNetConnString"), Me.CurrentSession.FormDataTable, intThisThreadId, "InitialComment", Left(strComment, 100) + " ...")
            objMatrix.InsertVTMatrixFormDataItem(Session("_VT_DotNetConnString"), Me.CurrentSession.FormDataTable, intThisThreadId, "LatestComment", strComment)
            objMatrix.InsertVTMatrixFormDataItem(Session("_VT_DotNetConnString"), Me.CurrentSession.FormDataTable, intThisThreadId, "DateCreated", Format(PortalFunctions.Now, "s"))

            If strCommentSource = "[System]" Then
                'Set the System as the Createdby source
                objMatrix.InsertVTMatrixFormDataItem(Session("_VT_DotNetConnString"), Me.CurrentSession.FormDataTable, intThisThreadId, "CreatedBy", "System")
                objMatrix.InsertVTMatrixFormDataItem(Session("_VT_DotNetConnString"), Me.CurrentSession.FormDataTable, intThisThreadId, "LatestCreatedBy", "System")

            Else
                'Set the User as the Createdby source
                objMatrix.InsertVTMatrixFormDataItem(Session("_VT_DotNetConnString"), Me.CurrentSession.FormDataTable, intThisThreadId, "CreatedBy", Session("_VT_CurrentUserName"))
                objMatrix.InsertVTMatrixFormDataItem(Session("_VT_DotNetConnString"), Me.CurrentSession.FormDataTable, intThisThreadId, "CreatedById", Session("_VT_CurrentUserId"))
                objMatrix.InsertVTMatrixFormDataItem(Session("_VT_DotNetConnString"), Me.CurrentSession.FormDataTable, intThisThreadId, "LatestCreatedBy", Session("_VT_CurrentUserName"))
            End If

            objMatrix.InsertVTMatrixFormDataItem(Session("_VT_DotNetConnString"), Me.CurrentSession.FormDataTable, intThisThreadId, "DateLatestCreated", Format(PortalFunctions.Now, "s"))
            objMatrix.InsertVTMatrixFormDataItem(Session("_VT_DotNetConnString"), Me.CurrentSession.FormDataTable, intThisThreadId, "NumComments", 1)

            objMatrix.InsertVTMatrixFormDataItem(Session("_VT_DotNetConnString"), Me.CurrentSession.FormDataTable, intThisThreadId, "Page", strPage)
            If Not TheControl Is Nothing Then
                objMatrix.InsertVTMatrixFormDataItem(Session("_VT_DotNetConnString"), Me.CurrentSession.FormDataTable, intThisThreadId, "ControlForThread", TheControl.ID)
                objMatrix.InsertVTMatrixFormDataItem(Session("_VT_DotNetConnString"), Me.CurrentSession.FormDataTable, intThisThreadId, "FieldLinked", "True")
            Else
                objMatrix.InsertVTMatrixFormDataItem(Session("_VT_DotNetConnString"), Me.CurrentSession.FormDataTable, intThisThreadId, "FieldLinked", "False")
            End If

            ' initialise the  Visibility settings
            objMatrix.InsertVTMatrixFormDataItem(Session("_VT_DotNetConnString"), Me.CurrentSession.FormDataTable, intThisThreadId, "VisibleToApplicant", "NO")
            objMatrix.InsertVTMatrixFormDataItem(Session("_VT_DotNetConnString"), Me.CurrentSession.FormDataTable, intThisThreadId, "VisibleToReviewer", "NO")

        Else
            Dim intNumComments As Integer
            dtThread = GetThreadDetails(intThisThreadId)
            If IsDBNull(dtThread.Rows(0).Item("Field10")) OrElse dtThread.Rows(0).Item("Field10") = "" Then
                intNumComments = 0
            Else
                intNumComments = dtThread.Rows(0).Item("Field10")
            End If
            intNumComments += 1
            objMatrix.InsertVTMatrixFormDataItem(Session("_VT_DotNetConnString"), Me.CurrentSession.FormDataTable, intThisThreadId, "NumComments", intNumComments)


        End If

        objMatrix.InsertVTMatrixFormDataItem(Session("_VT_DotNetConnString"), Me.CurrentSession.FormDataTable, intThisThreadId, "LatestComment", strComment)
        objMatrix.InsertVTMatrixFormDataItem(Session("_VT_DotNetConnString"), Me.CurrentSession.FormDataTable, intThisThreadId, "DateLatestCreated", Format(PortalFunctions.Now, "s"))
        objMatrix.InsertVTMatrixFormDataItem(Session("_VT_DotNetConnString"), Me.CurrentSession.FormDataTable, intThisThreadId, "LatestCreatedBy", Session("_VT_CurrentUserName"))
        If strCommentSource = GetGlobalResourceObject("Resource", "UserCategory_Applicants") Or strCommentTarget = GetGlobalResourceObject("Resource", "UserCategory_Applicants") Then
            objMatrix.InsertVTMatrixFormDataItem(Session("_VT_DotNetConnString"), Me.CurrentSession.FormDataTable, intThisThreadId, "VisibleToApplicant", "YES")
        End If
        If strCommentSource = GetGlobalResourceObject("Resource", "UserCategory_Reviewers") Or strCommentTarget = GetGlobalResourceObject("Resource", "UserCategory_Reviewers") Then
            objMatrix.InsertVTMatrixFormDataItem(Session("_VT_DotNetConnString"), Me.CurrentSession.FormDataTable, intThisThreadId, "VisibleToReviewer", "YES")
        End If


        ' use the Date/time as a unique name for the comment
        strTemp = "Comment" + Format(PortalFunctions.Now, "ddMMHHmmss")
        Dim intThisCommentId As Integer = objMatrix.GetFormId(Session("_VT_DotNetConnString"), Me.CurrentSession.FormListTable, strTemp, intThisThreadId)

        objMatrix.InsertVTMatrixFormDataItem(Session("_VT_DotNetConnString"), Me.CurrentSession.FormDataTable, intThisCommentId, "ItemId", intThisCommentId.ToString)
        objMatrix.InsertVTMatrixFormDataItem(Session("_VT_DotNetConnString"), Me.CurrentSession.FormDataTable, intThisCommentId, "ParentId", intThisThreadId.ToString)
        objMatrix.InsertVTMatrixFormDataItem(Session("_VT_DotNetConnString"), Me.CurrentSession.FormDataTable, intThisCommentId, "Comment", strComment)
        objMatrix.InsertVTMatrixFormDataItem(Session("_VT_DotNetConnString"), Me.CurrentSession.FormDataTable, intThisCommentId, "DateCreated", Format(PortalFunctions.Now, "s"))

        If strCommentSource = "[System]" Then
            'Set the System as the Createdby source
            objMatrix.InsertVTMatrixFormDataItem(Session("_VT_DotNetConnString"), Me.CurrentSession.FormDataTable, intThisCommentId, "CreatedBy", "System")
        Else
            'Set the User as the Createdby source
            objMatrix.InsertVTMatrixFormDataItem(Session("_VT_DotNetConnString"), Me.CurrentSession.FormDataTable, intThisCommentId, "CreatedBy", Session("_VT_CurrentUserName"))
            objMatrix.InsertVTMatrixFormDataItem(Session("_VT_DotNetConnString"), Me.CurrentSession.FormDataTable, intThisCommentId, "CreatedById", Session("_VT_CurrentUserId"))
        End If


        objMatrix.InsertVTMatrixFormDataItem(Session("_VT_DotNetConnString"), Me.CurrentSession.FormDataTable, intThisCommentId, "DateCleared", "")
        objMatrix.InsertVTMatrixFormDataItem(Session("_VT_DotNetConnString"), Me.CurrentSession.FormDataTable, intThisCommentId, "ClearedBy", "")

        objMatrix.InsertVTMatrixFormDataItem(Session("_VT_DotNetConnString"), Me.CurrentSession.FormDataTable, intThisCommentId, "Page", strPage)
        If Not TheControl Is Nothing Then
            objMatrix.InsertVTMatrixFormDataItem(Session("_VT_DotNetConnString"), Me.CurrentSession.FormDataTable, intThisCommentId, "Control", TheControl.ID)
        End If

        objMatrix.InsertVTMatrixFormDataItem(Session("_VT_DotNetConnString"), Me.CurrentSession.FormDataTable, intThisCommentId, "Source", strCommentSource)
        objMatrix.InsertVTMatrixFormDataItem(Session("_VT_DotNetConnString"), Me.CurrentSession.FormDataTable, intThisCommentId, "SourceId", intCommentSourceId)
        objMatrix.InsertVTMatrixFormDataItem(Session("_VT_DotNetConnString"), Me.CurrentSession.FormDataTable, intThisCommentId, "Target", strCommentTarget)
        objMatrix.InsertVTMatrixFormDataItem(Session("_VT_DotNetConnString"), Me.CurrentSession.FormDataTable, intThisCommentId, "TargetId", intCommentTargetId)

        ''''''''''''''''''''''''''''
        ' write to audit log
        ''''''''''''''''''''''''''''
        Dim objForms As New VT_Forms.Forms
        Dim strAuditComment, strType As String
        strAuditComment = "New Comment created with Id: " + intThisCommentId.ToString
        strType = Me.CurrentSession.CurrentModuleType
        objForms.WriteToAuditLog(Me.CurrentSession.SelectedItemID, strType, PortalFunctions.Now, Session("_VT_CurrentUserName"), Session("_VT_CurrentUserId"), strAuditComment, "Audit Record", "SysAdmin")

    End Sub
    ''' <summary>
    ''' Clears the selected comment
    ''' </summary>
    ''' <param name="intCommentId"></param>
    ''' <remarks></remarks>
    Sub ClearComment(ByVal intCommentId As Integer)
        Dim objMatrix As New VTDBFunctions.VTDBFunctions.VTMatrixFunctions

        objMatrix.InsertVTMatrixFormDataItem(Session("_VT_DotNetConnString"), Me.CurrentSession.FormDataTable, intCommentId, "DateCleared", Format(PortalFunctions.Now, "s"))
        objMatrix.InsertVTMatrixFormDataItem(Session("_VT_DotNetConnString"), Me.CurrentSession.FormDataTable, intCommentId, "ClearedBy", Session("_VT_CurrentUserName"))

        ''''''''''''''''''''''''''''
        ' write to audit log
        ''''''''''''''''''''''''''''
        Dim objForms As New VT_Forms.Forms
        Dim strAuditComment, strType As String
        strAuditComment = "Cleared Comment with Id: " + intCommentId.ToString
        strType = Me.CurrentSession.CurrentModuleType
        objForms.WriteToAuditLog(Me.CurrentSession.SelectedItemID, strType, PortalFunctions.Now, Session("_VT_CurrentUserName"), Session("_VT_CurrentUserId"), strAuditComment, "Audit Record", "SysAdmin")

    End Sub



End Class
