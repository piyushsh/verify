Imports System.Data.SqlClient
Imports System.Data
Imports System.Configuration
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Drawing
Imports System.Web.UI.HtmlControls


Imports BPADotNetCommonFunctions.VT_CommonFunctions.MatrixFunctions
Imports BPADotNetCommonFunctions
Imports VTDBFunctions




Namespace SupportFunctions
    Public Class Support
        Inherits System.Web.UI.Page

        Public Const g_strFormListTable = "vt_Folders"
        Public Const g_strFormDataTable = "vt_FileData"

        Function GetSageUploadDllName() As String
            Dim objIni As New ntsUtils.cIniFile


            With objIni
                .Path = "QAO.INI"
                ' Source info
                .Section = "General Options"
                .Key = "UploadDllName"
                GetSageUploadDllName = Trim$(.Value)
            End With

        End Function
        Function GetFolderPathWithHyphens(ByVal lngFolderId As Long) As String
            Dim strFolderPath As String
            Dim lngTempFolder As Long

            Dim dsFolder As New Data.DataSet
            Dim objDBFuncs As New DataBaseFunctions.Commonfunctions

            dsFolder = objDBFuncs.GetFolder(SupportFunctions.Support.g_strFormListTable, lngFolderId)

            strFolderPath = dsFolder.Tables(0).Rows(0).Item("FormName")
            lngTempFolder = dsFolder.Tables(0).Rows(0).Item("JobId")
            If lngTempFolder <> 0 Then
                ' if we are not at the root folder build the path to the current folder
                While lngTempFolder <> objDBFuncs.GetRootFolderId(SupportFunctions.Support.g_strFormListTable)
                    dsFolder = objDBFuncs.GetFolder(SupportFunctions.Support.g_strFormListTable, lngTempFolder)
                    strFolderPath = dsFolder.Tables(0).Rows(0).Item("FormName") + " - " + strFolderPath
                    lngTempFolder = dsFolder.Tables(0).Rows(0).Item("JobId")
                End While

            End If

            GetFolderPathWithHyphens = strFolderPath
        End Function

        Function GetFolderFullPath(ByVal lngFolderId As Long, Optional ByVal blnPublic As Boolean = False) As String
            Dim RootPath As String
            Dim ds As Data.DataSet
            Dim objdbfuncs As New DataBaseFunctions.Commonfunctions
            Dim Fullpath As String
            Dim lngParentId As Long

            If blnPublic = True Then 'And InStr(Session("_VT_Referrer"), "localhost") = 0 Then
                RootPath = Session("_VT_RootFolderPathWeb")
            Else
                RootPath = Session("_VT_RootFolderPath")
            End If


            'now have to build up the rest of the path by going back through each parent level
            ds = objdbfuncs.GetFolder(SupportFunctions.Support.g_strFormListTable, lngFolderId)
            If ds.Tables(0).Rows.Count > 0 Then
                ' 
                If ds.Tables(0).Rows(0).Item("JobId") <> 0 Then
                    Fullpath = ds.Tables(0).Rows(0).Item("FormName") & "\"
                End If

                'we are building the path name from the bottom up to the root
                ds = objdbfuncs.GetFolder(SupportFunctions.Support.g_strFormListTable, ds.Tables(0).Rows(0).Item("JobId"))
                If ds.Tables(0).Rows.Count > 0 Then
                    Do While ds.Tables(0).Rows(0).Item("JobId") <> 0
                        Fullpath = ds.Tables(0).Rows(0).Item("FormName") & "\" & Fullpath
                        lngParentId = ds.Tables(0).Rows(0).Item("JobId")
                        ds = objdbfuncs.GetFolder(SupportFunctions.Support.g_strFormListTable, lngParentId)
                    Loop
                Else
                    ds = objdbfuncs.GetFolder(SupportFunctions.Support.g_strFormListTable, lngFolderId)
                End If

                'now put it all together along with the root folder name. the current
                'ds must be the root one or it would still be in the do loop

                RootPath = RootPath & ds.Tables(0).Rows(0).Item("FormName") & "\"
                Fullpath = RootPath & Fullpath

                GetFolderFullPath = Fullpath
            End If
        End Function
    End Class
End Namespace
Namespace DataBaseFunctions
    Public Class VersionControlConfig
        Inherits MyBasePage
        Function GetVersionConfigSettings() As Data.DataTable
            Dim objCfg As New VTDBFunctions.VTDBFunctions.VTMatrixFunctions
            Dim intRoot As Integer = Session("_VT_RootId")
            Dim intVersionConfigFolder As Integer = objCfg.GetFormId(Session("_VT_DotNetConnString"), Me.CurrentSession.FormListTable, "VersionControlConfig", intRoot)
            GetVersionConfigSettings = objCfg.GetVTMatrixFormData(Session("_VT_DotNetConnString"), Me.CurrentSession.FormDataTable, intVersionConfigFolder)

        End Function


    End Class
    Public Class CustomerFunctions
        Inherits System.Web.UI.Page
        Function GetAllCustomers() As DataTable


            Dim strConnString As String = Session("_VT_DotNetConnString")
            Dim myConnection As New SqlConnection(strConnString)

            Dim comSQL As New SqlCommand("cus_SelectStmt", myConnection)
            Dim myAdapter As New SqlDataAdapter(comSQL)
            Dim ds As New DataTable


            comSQL.CommandType = CommandType.StoredProcedure

            myConnection.Open()
            myAdapter.Fill(ds)

            GetAllCustomers = ds

            myConnection.Close()
            myConnection = Nothing
        End Function
    End Class


    Public Class Commonfunctions
        Inherits MyBasePage

        Sub SaveFolderName(ByVal FolderName As String, ByVal FormId As Long)

            Dim strConnString As String = Session("_VT_DotNetConnString")
            Dim myConnection As New Data.SqlClient.SqlConnection(strConnString)
            Dim comSQL As New Data.SqlClient.SqlCommand("vtm_updFolderName", myConnection)
            Dim objIDParam, objNameParam As Data.SqlClient.SqlParameter


            comSQL.CommandType = Data.CommandType.StoredProcedure


            objIDParam = comSQL.Parameters.Add("@FormId", Data.SqlDbType.Int)
            objIDParam.Direction = Data.ParameterDirection.Input
            objIDParam.Value = FormId

            objNameParam = comSQL.Parameters.Add("@Name", Data.SqlDbType.Char)
            objNameParam.Direction = Data.ParameterDirection.Input
            objNameParam.Value = Trim(FolderName)

            Try
                myConnection.Open()
                comSQL.ExecuteScalar()

            Catch ex As Exception
            Finally
                ' Close the connection when done with it.
                myConnection.Close()
            End Try

        End Sub

        Function IsRecycleBin(ByVal FolderId As Long) As Boolean
            Dim ds As New Data.DataSet

            ' get the Data.DataSet for this folder
            ds = GetFolder(SupportFunctions.Support.g_strFormListTable, FolderId)

            If ds.Tables(0).Rows.Count > 0 Then
                ' if the selected folder is the Recycle bin
                If IIf(IsDBNull(ds.Tables(0).Rows(0).Item("IsRecycleBin")), 0, ds.Tables(0).Rows(0).Item("IsRecycleBin")) = True Then
                    IsRecycleBin = True
                Else
                    IsRecycleBin = False
                End If
            Else
                IsRecycleBin = False
            End If

        End Function

        Sub UpdateAuditLogs(ByVal NewJobId As Long, ByVal OldJobId As Long)

            Dim strConnString As String = Session("_VT_DotNetConnString")
            Dim myConnection As New Data.SqlClient.SqlConnection(strConnString)
            Dim comSQL As New Data.SqlClient.SqlCommand("wfo_updAuditRecords", myConnection)
            Dim objIDParam, objNameParam As Data.SqlClient.SqlParameter


            comSQL.CommandType = Data.CommandType.StoredProcedure


            objIDParam = comSQL.Parameters.Add("@NewJobId", Data.SqlDbType.Int)
            objIDParam.Direction = Data.ParameterDirection.Input
            objIDParam.Value = NewJobId

            objNameParam = comSQL.Parameters.Add("@OldJobId", Data.SqlDbType.Char)
            objNameParam.Direction = Data.ParameterDirection.Input
            objNameParam.Value = OldJobId

            Try
                myConnection.Open()
                comSQL.ExecuteScalar()

            Catch ex As Exception
            Finally
                ' Close the connection when done with it.
                myConnection.Close()
            End Try

        End Sub

        Function getIndexDetails(ByVal Fieldname As String) As Data.DataSet
            Dim strConnString As String = Session("_VT_DotNetConnString")
            Dim myConnection As New SqlConnection(strConnString)
            Dim comSQL As New SqlCommand("vt_resIndexForFieldName", myConnection)
            Dim myAdapter As New SqlDataAdapter(comSQL)
            Dim ds As New Data.DataSet

            Dim objParam As SqlParameter

            comSQL.CommandType = Data.CommandType.StoredProcedure

            objParam = comSQL.Parameters.Add("@FieldName", Data.SqlDbType.NVarChar)
            objParam.Direction = Data.ParameterDirection.Input
            objParam.Value = Fieldname


            myConnection.Open()
            myAdapter.Fill(ds)

            getIndexDetails = ds
            myConnection.Close()
            myConnection = Nothing

        End Function

        Function GetFolderIdForPath(ByVal strListOfFolders As String) As Integer
            Dim intFolderId, intParentId As Integer
            Dim strFolderName As String
            Dim objDBFunc As New DataBaseFunctions.Commonfunctions

            If InStr(strListOfFolders, "\") Then
                strFolderName = Left(strListOfFolders, InStr(strListOfFolders, "\") - 1)
                strListOfFolders = Mid(strListOfFolders, InStr(strListOfFolders, "\") + 1)
            Else
                strFolderName = strListOfFolders
                strListOfFolders = ""
            End If

            intParentId = objDBFunc.GetRootFolderId(SupportFunctions.Support.g_strFormListTable)
            intFolderId = objDBFunc.GetFolderIdForName(SupportFunctions.Support.g_strFormListTable, strFolderName, intParentId)

            Do While strListOfFolders <> ""
                If InStr(strListOfFolders, "\") Then
                    strFolderName = Left(strListOfFolders, InStr(strListOfFolders, "\") - 1)
                    strListOfFolders = Mid(strListOfFolders, InStr(strListOfFolders, "\") + 1)
                Else
                    strFolderName = strListOfFolders
                    strListOfFolders = ""
                End If

                intParentId = intFolderId
                intFolderId = objDBFunc.GetFolderIdForName(SupportFunctions.Support.g_strFormListTable, strFolderName, intParentId)

            Loop


            GetFolderIdForPath = intFolderId
        End Function



        Sub SaveIndexName(ByVal FieldName As String, ByVal IndexName As String)

            Dim strConnString As String = Session("_VT_DotNetConnString")
            Dim myConnection As New Data.SqlClient.SqlConnection(strConnString)
            Dim comSQL As New Data.SqlClient.SqlCommand("vtm_updSaveIndex", myConnection)
            Dim objIDParam, objNameParam As Data.SqlClient.SqlParameter


            comSQL.CommandType = Data.CommandType.StoredProcedure

            'need to first check if this index exists, if it does then update it, otherwise add it
            Dim ds As New Data.DataSet
            ds = getIndexDetails(FieldName)
            If ds.Tables(0).Rows.Count = 0 Then
                'this index is not yet in the table, so insert it
                comSQL.CommandText = "vtm_insNewIndex"

            End If

            objNameParam = comSQL.Parameters.Add("@IndexName", Data.SqlDbType.Char)
            objNameParam.Direction = Data.ParameterDirection.Input
            objNameParam.Value = Trim(IndexName)

            objIDParam = comSQL.Parameters.Add("@FieldName", Data.SqlDbType.Char)
            objIDParam.Direction = Data.ParameterDirection.Input
            objIDParam.Value = FieldName


            Try
                myConnection.Open()
                comSQL.ExecuteScalar()

            Catch ex As Exception
            Finally
                ' Close the connection when done with it.
                myConnection.Close()
            End Try
        End Sub

        Sub SetIsRecycleBin(ByVal FolderId As Long, ByVal blnIsRecycleBin As Boolean)
            Dim strConnString As String = Session("_VT_DotNetConnString")
            Dim myConnection As New Data.SqlClient.SqlConnection(strConnString)
            Dim comSQL As New Data.SqlClient.SqlCommand("vtm_updIsRecycleBin", myConnection)
            Dim objIDParam, objNameParam As Data.SqlClient.SqlParameter


            comSQL.CommandType = Data.CommandType.StoredProcedure


            'objReturnParam = comSQL.Parameters.Add("@ReturnValue", Data.SqlDbType.Int)
            'objReturnParam.Direction = Data.ParameterDirection.ReturnValue
            objNameParam = comSQL.Parameters.Add("@FolderId", Data.SqlDbType.Int)
            objNameParam.Direction = Data.ParameterDirection.Input
            objNameParam.Value = FolderId

            objIDParam = comSQL.Parameters.Add("@IsRecycleBin", Data.SqlDbType.Bit)
            objIDParam.Direction = Data.ParameterDirection.Input
            objIDParam.Value = blnIsRecycleBin

            Try
                myConnection.Open()
                comSQL.ExecuteScalar()

            Catch ex As Exception
            Finally
                ' Close the connection when done with it.
                myConnection.Close()
            End Try
        End Sub

        Sub EditDocInDB(ByVal strFileDataTable As String, ByVal lngFileId As Long, ByVal FileType As Integer, ByVal Title As String, _
ByVal Index1 As String, ByVal Index2 As String, ByVal Index3 As String, _
ByVal Index4 As String, ByVal Index5 As String, ByVal Index6 As String, ByVal Index7 As String, ByVal Index8 As String, ByVal Index9 As String, ByVal Index10 As String)

            Dim strConnString As String = Session("_VT_DotNetConnString")
            Dim myConnection As New SqlConnection(strConnString)
            Dim comSQL As New SqlCommand("vt_updFile", myConnection)
            Dim myAdapter As New SqlDataAdapter(comSQL)
            Dim ds As New Data.DataSet

            Dim objParam As SqlParameter

            comSQL.CommandType = Data.CommandType.StoredProcedure
            objParam = comSQL.Parameters.Add("@TableName", Data.SqlDbType.NVarChar)
            objParam.Direction = Data.ParameterDirection.Input
            objParam.Value = strFileDataTable

            objParam = comSQL.Parameters.Add("@FormID", Data.SqlDbType.NChar)
            objParam.Direction = Data.ParameterDirection.Input
            objParam.Value = lngFileId

            objParam = comSQL.Parameters.Add("@FileType", Data.SqlDbType.Int)
            objParam.Direction = Data.ParameterDirection.Input
            objParam.Value = FileType

            objParam = comSQL.Parameters.Add("@Title", Data.SqlDbType.NChar)
            objParam.Direction = Data.ParameterDirection.Input
            objParam.Value = Title

            objParam = comSQL.Parameters.Add("@Index1", Data.SqlDbType.NChar)
            objParam.Direction = Data.ParameterDirection.Input
            objParam.Value = Index1

            objParam = comSQL.Parameters.Add("@Index2", Data.SqlDbType.NChar)
            objParam.Direction = Data.ParameterDirection.Input
            objParam.Value = Index2

            objParam = comSQL.Parameters.Add("@Index3", Data.SqlDbType.NChar)
            objParam.Direction = Data.ParameterDirection.Input
            objParam.Value = Index3

            objParam = comSQL.Parameters.Add("@Index4", Data.SqlDbType.NChar)
            objParam.Direction = Data.ParameterDirection.Input
            objParam.Value = Index4

            objParam = comSQL.Parameters.Add("@Index5", Data.SqlDbType.NChar)
            objParam.Direction = Data.ParameterDirection.Input
            objParam.Value = Index5

            objParam = comSQL.Parameters.Add("@Index6", Data.SqlDbType.NChar)
            objParam.Direction = Data.ParameterDirection.Input
            objParam.Value = Index6

            objParam = comSQL.Parameters.Add("@Index7", Data.SqlDbType.NChar)
            objParam.Direction = Data.ParameterDirection.Input
            objParam.Value = Index7

            objParam = comSQL.Parameters.Add("@Index8", Data.SqlDbType.NChar)
            objParam.Direction = Data.ParameterDirection.Input
            objParam.Value = Index8

            objParam = comSQL.Parameters.Add("@Index9", Data.SqlDbType.NChar)
            objParam.Direction = Data.ParameterDirection.Input
            objParam.Value = Index9

            objParam = comSQL.Parameters.Add("@Index10", Data.SqlDbType.NChar)
            objParam.Direction = Data.ParameterDirection.Input
            objParam.Value = Index10

            myConnection.Open()
            myAdapter.Fill(ds)

            myConnection.Close()
            myConnection = Nothing

        End Sub






        Sub StoreItemApprovalDetails(ByVal intItemId As Integer, ByVal TheDetails As GenericFunctions.ApprovalDetails)
            Dim objMatrix As New VT_CommonFunctions.MatrixFunctions

            objMatrix.InsertDataItem(Session("_VT_DotNetConnString"), intItemId, Me.CurrentSession.FormDataTable, "VersionOwner", Session("_VT_CurrentUserId"))

            Dim intApprovalDetailsNode As Integer = objMatrix.GetGenericFolderId(Session("_VT_DotNetConnString"), Me.CurrentSession.FormListTable, intItemId, "ApprovalDetails")

            objMatrix.InsertDataItem(Session("_VT_DotNetConnString"), intApprovalDetailsNode, Me.CurrentSession.FormDataTable, "AndOr", TheDetails.UseAndOrOr)

            ' create an AllOf node
            Dim intAllOfNode As Integer = objMatrix.GetGenericFolderId(Session("_VT_DotNetConnString"), Me.CurrentSession.FormListTable, intApprovalDetailsNode, "AllOf")
            ' go through the AllOf array and create a node to store the data
            Dim intCount, intNodeId As Integer
            For intCount = LBound(TheDetails.AllItems) To UBound(TheDetails.AllItems)
                intNodeId = objMatrix.GetGenericFolderId(Session("_VT_DotNetConnString"), Me.CurrentSession.FormListTable, intAllOfNode, "AllOf" + intCount.ToString)

                objMatrix.InsertDataItem(Session("_VT_DotNetConnString"), intNodeId, Me.CurrentSession.FormDataTable, "Type", TheDetails.AllItems(intCount).Type)
                objMatrix.InsertDataItem(Session("_VT_DotNetConnString"), intNodeId, Me.CurrentSession.FormDataTable, "Name", TheDetails.AllItems(intCount).Name)
                objMatrix.InsertDataItem(Session("_VT_DotNetConnString"), intNodeId, Me.CurrentSession.FormDataTable, "Id", TheDetails.AllItems(intCount).Id)
                objMatrix.InsertDataItem(Session("_VT_DotNetConnString"), intNodeId, Me.CurrentSession.FormDataTable, "Default", TheDetails.AllItems(intCount).IsDefault)
                objMatrix.InsertDataItem(Session("_VT_DotNetConnString"), intNodeId, Me.CurrentSession.FormDataTable, "ItemId", intAllOfNode.ToString)

            Next

            ' create a OneOf node
            Dim intOneOfNode As Integer = objMatrix.GetGenericFolderId(Session("_VT_DotNetConnString"), Me.CurrentSession.FormListTable, intApprovalDetailsNode, "OneOf")
            For intCount = LBound(TheDetails.OneOfItems) To UBound(TheDetails.OneOfItems)
                intNodeId = objMatrix.GetGenericFolderId(Session("_VT_DotNetConnString"), Me.CurrentSession.FormListTable, intOneOfNode, "OneOf" + intCount.ToString)

                objMatrix.InsertDataItem(Session("_VT_DotNetConnString"), intNodeId, Me.CurrentSession.FormDataTable, "Type", TheDetails.OneOfItems(intCount).Type)
                objMatrix.InsertDataItem(Session("_VT_DotNetConnString"), intNodeId, Me.CurrentSession.FormDataTable, "Name", TheDetails.OneOfItems(intCount).Name)
                objMatrix.InsertDataItem(Session("_VT_DotNetConnString"), intNodeId, Me.CurrentSession.FormDataTable, "Id", TheDetails.OneOfItems(intCount).Id)
                objMatrix.InsertDataItem(Session("_VT_DotNetConnString"), intNodeId, Me.CurrentSession.FormDataTable, "Default", TheDetails.OneOfItems(intCount).IsDefault)
                objMatrix.InsertDataItem(Session("_VT_DotNetConnString"), intNodeId, Me.CurrentSession.FormDataTable, "ItemId", intOneOfNode.ToString)

            Next


        End Sub


        Function AreWeDoingVersionControl(ByVal strType As String) As String

            Dim objCfg As New VT_CommonFunctions.MatrixFunctions
            Dim intRoot As Integer = objCfg.GetModuleRootId(Session("_VT_DotNetConnString"), "cfg_FormList", "ConfigModuleRoot")
            Dim intModuleSetupsId As Integer = objCfg.GetGenericFolderId(Session("_VT_DotNetConnString"), "cfg_FormList", intRoot, "ModuleSetups")
            Dim intThisModuleSetupId As Integer = objCfg.GetGenericFolderId(Session("_VT_DotNetConnString"), "cfg_FormList", intModuleSetupsId, strType)

            AreWeDoingVersionControl = UCase(objCfg.GetDataItemInFolder(Session("_VT_DotNetConnString"), "cfg_FormData", intThisModuleSetupId, "VersionTrackingEnabled"))
        End Function
        ''' <summary>
        ''' Gets the Id of the current version of an item or 0 if there is no current version
        ''' </summary>
        ''' <param name="intItemId">The id of the base item  - not a version id</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function GetCurrentVersionIdOfItem(ByVal strConnString As String, ByVal intItemId As Integer, ByVal strFormListTable As String, ByVal strFormDataTable As String, ByVal strPrimaryFormName As String) As Integer
            ' get all the versions of the selected item
            Dim objMatrix As New VT_CommonFunctions.MatrixFunctions
            Dim dt As Data.DataTable = objMatrix.GetItemsInCategory(strConnString, intItemId, strFormListTable, True)
            dt.DefaultView.Sort = "FormId desc"

            Dim intCurrentVersionId As Integer = 0
            For Each dr As Data.DataRow In dt.Rows
                Dim intPrimaryFormId As Integer = objMatrix.FindGenericFolderId(strConnString, strFormListTable, dr.Item("FormId"), strPrimaryFormName)
                Dim strTemp As String = objMatrix.GetDataItemInFolder(strConnString, strFormDataTable, intPrimaryFormId, "VersionStatus")

                If strTemp = HttpContext.GetGlobalResourceObject("resource", GenericFunctions.StatusTags.VersionStatus_Current.ToString) Then
                    intCurrentVersionId = dr.Item("FormId")
                End If
            Next

            If intCurrentVersionId = 0 Then
                ' take the first verion as the current version
                intCurrentVersionId = dt.Rows(0).Item("FormId")
            End If
            GetCurrentVersionIdOfItem = intCurrentVersionId
        End Function

        Function GetVersionStatus(ByVal intVersionId As Integer) As String
            Dim objMatrix As New VT_CommonFunctions.MatrixFunctions
            Dim intPrimaryFormId As Integer = objMatrix.FindGenericFolderId(Session("_VT_DotNetConnString"), Me.CurrentSession.FormListTable, intVersionId, Me.CurrentSession.PrimaryFormName)
            Dim strTemp As String = objMatrix.GetDataItemInFolder(Session("_VT_DotNetConnString"), Me.CurrentSession.FormDataTable, intPrimaryFormId, "VersionStatus")

            GetVersionStatus = strTemp
        End Function


        Function GetItemVersionNumber(ByVal intVersionId As Integer) As String
            Dim objMatrix As New VT_CommonFunctions.MatrixFunctions
            Dim intPrimaryFormId As Integer = objMatrix.FindGenericFolderId(Session("_VT_DotNetConnString"), Me.CurrentSession.FormListTable, intVersionId, Me.CurrentSession.PrimaryFormName)
            Dim strTemp As String = objMatrix.GetDataItemInFolder(Session("_VT_DotNetConnString"), Me.CurrentSession.FormDataTable, intPrimaryFormId, "ItemVersion")

            GetItemVersionNumber = strTemp
        End Function


        Sub SaveDocToDB(ByVal strFileDataTable As String, ByVal FileName As String, ByVal FileType As Integer, ByVal Title As String, _
        ByVal FolderId As Integer, ByVal Index1 As String, ByVal Index2 As String, ByVal Index3 As String, _
        ByVal Index4 As String, ByVal Index5 As String, ByVal Index6 As String, ByVal Index7 As String, ByVal Index8 As String, ByVal Index9 As String, ByVal Index10 As String)

            Dim strConnString As String = Session("_VT_DotNetConnString")
            Dim myConnection As New SqlConnection(strConnString)
            Dim comSQL As New SqlCommand("vt_insNewFile", myConnection)
            Dim myAdapter As New SqlDataAdapter(comSQL)
            Dim ds As New Data.DataSet

            Dim objParam As SqlParameter

            comSQL.CommandType = Data.CommandType.StoredProcedure
            objParam = comSQL.Parameters.Add("@TableName", Data.SqlDbType.NVarChar)
            objParam.Direction = Data.ParameterDirection.Input
            objParam.Value = strFileDataTable

            objParam = comSQL.Parameters.Add("@FileName", Data.SqlDbType.NChar)
            objParam.Direction = Data.ParameterDirection.Input
            objParam.Value = FileName

            objParam = comSQL.Parameters.Add("@FileType", Data.SqlDbType.Int)
            objParam.Direction = Data.ParameterDirection.Input
            objParam.Value = FileType

            objParam = comSQL.Parameters.Add("@Title", Data.SqlDbType.NChar)
            objParam.Direction = Data.ParameterDirection.Input
            objParam.Value = Title

            objParam = comSQL.Parameters.Add("@FolderId", Data.SqlDbType.Int)
            objParam.Direction = Data.ParameterDirection.Input
            objParam.Value = FolderId

            objParam = comSQL.Parameters.Add("@Index1", Data.SqlDbType.NChar)
            objParam.Direction = Data.ParameterDirection.Input
            objParam.Value = Index1

            objParam = comSQL.Parameters.Add("@Index2", Data.SqlDbType.NChar)
            objParam.Direction = Data.ParameterDirection.Input
            objParam.Value = Index2

            objParam = comSQL.Parameters.Add("@Index3", Data.SqlDbType.NChar)
            objParam.Direction = Data.ParameterDirection.Input
            objParam.Value = Index3

            objParam = comSQL.Parameters.Add("@Index4", Data.SqlDbType.NChar)
            objParam.Direction = Data.ParameterDirection.Input
            objParam.Value = Index4

            objParam = comSQL.Parameters.Add("@Index5", Data.SqlDbType.NChar)
            objParam.Direction = Data.ParameterDirection.Input
            objParam.Value = Index5

            objParam = comSQL.Parameters.Add("@Index6", Data.SqlDbType.NChar)
            objParam.Direction = Data.ParameterDirection.Input
            objParam.Value = Index6

            objParam = comSQL.Parameters.Add("@Index7", Data.SqlDbType.NChar)
            objParam.Direction = Data.ParameterDirection.Input
            objParam.Value = Index7

            objParam = comSQL.Parameters.Add("@Index8", Data.SqlDbType.NChar)
            objParam.Direction = Data.ParameterDirection.Input
            objParam.Value = Index8

            objParam = comSQL.Parameters.Add("@Index9", Data.SqlDbType.NChar)
            objParam.Direction = Data.ParameterDirection.Input
            objParam.Value = Index9

            objParam = comSQL.Parameters.Add("@Index10", Data.SqlDbType.NChar)
            objParam.Direction = Data.ParameterDirection.Input
            objParam.Value = Index10

            myConnection.Open()
            myAdapter.Fill(ds)

            myConnection.Close()
            myConnection = Nothing

        End Sub
        Function GetFileTypes() As Data.DataSet
            Dim strConnString As String = Session("_VT_DotNetConnString")
            Dim myConnection As New SqlConnection(strConnString)
            Dim comSQL As New SqlCommand("vt_resFiletypes", myConnection)
            Dim myAdapter As New SqlDataAdapter(comSQL)
            Dim ds As New Data.DataSet

            comSQL.CommandType = Data.CommandType.StoredProcedure

            myConnection.Open()
            myAdapter.Fill(ds)

            GetFileTypes = ds
            myConnection.Close()
            myConnection = Nothing

        End Function
        Function GetIndices() As Data.DataSet

            Dim strConnString As String = Session("_VT_DotNetConnString")
            Dim myConnection As New SqlConnection(strConnString)
            Dim comSQL As New SqlCommand("vt_resGetIndices", myConnection)
            Dim myAdapter As New SqlDataAdapter(comSQL)
            Dim ds As New Data.DataSet


            comSQL.CommandType = Data.CommandType.StoredProcedure

            myConnection.Open()
            myAdapter.Fill(ds)

            GetIndices = ds
            myConnection.Close()
            myConnection = Nothing
        End Function
        Function GetDocsForFolder(ByVal strFileDataTable As String, ByVal lngFolderId As Long, Optional ByVal strFilter As String = "") As Data.DataSet

            Dim strConnString As String = Session("_VT_DotNetConnString")
            Dim myConnection As New SqlConnection(strConnString)
            Dim comSQL As New SqlCommand("vt_resDocsForFolder", myConnection)
            Dim myAdapter As New SqlDataAdapter(comSQL)
            Dim ds As New Data.DataSet

            Dim objParam As SqlParameter

            comSQL.CommandType = Data.CommandType.StoredProcedure

            objParam = comSQL.Parameters.Add("@TableName", Data.SqlDbType.NVarChar)
            objParam.Direction = Data.ParameterDirection.Input
            objParam.Value = strFileDataTable

            objParam = comSQL.Parameters.Add("@FolderId", Data.SqlDbType.Int)
            objParam.Direction = Data.ParameterDirection.Input
            objParam.Value = lngFolderId

            objParam = comSQL.Parameters.Add("@Filter", Data.SqlDbType.NVarChar)
            objParam.Direction = Data.ParameterDirection.Input
            objParam.Value = strFilter

            myConnection.Open()
            myAdapter.Fill(ds)

            GetDocsForFolder = ds
            myConnection.Close()
            myConnection = Nothing
        End Function

        Function GetDocsForSearch(ByVal strFileDataTable As String, Optional ByVal strFilter As String = "") As Data.DataSet

            Dim strConnString As String = Session("_VT_DotNetConnString")
            Dim myConnection As New SqlConnection(strConnString)
            Dim comSQL As New SqlCommand("vt_resDocsForSearch", myConnection)
            Dim myAdapter As New SqlDataAdapter(comSQL)
            Dim ds As New Data.DataSet

            Dim objParam As SqlParameter

            comSQL.CommandType = Data.CommandType.StoredProcedure

            objParam = comSQL.Parameters.Add("@TableName", Data.SqlDbType.NVarChar)
            objParam.Direction = Data.ParameterDirection.Input
            objParam.Value = strFileDataTable

            objParam = comSQL.Parameters.Add("@Filter", Data.SqlDbType.NVarChar)
            objParam.Direction = Data.ParameterDirection.Input
            objParam.Value = strFilter

            myConnection.Open()
            myAdapter.Fill(ds)

            GetDocsForSearch = ds
            myConnection.Close()
            myConnection = Nothing
        End Function


        Sub SaveNewFolder(ByVal FormListTableName As String, ByVal FolderName As String, ByVal ParentId As Long)

            Dim strConnString As String = Session("_VT_DotNetConnString")
            Dim myConnection As New Data.SqlClient.SqlConnection(strConnString)
            Dim comSQL As New Data.SqlClient.SqlCommand("vtm_insNewFormInstance", myConnection)
            Dim objIDParam, objNameParam As Data.SqlClient.SqlParameter


            comSQL.CommandType = Data.CommandType.StoredProcedure


            'objReturnParam = comSQL.Parameters.Add("@ReturnValue", Data.SqlDbType.Int)
            'objReturnParam.Direction = Data.ParameterDirection.ReturnValue
            objNameParam = comSQL.Parameters.Add("@TableName", Data.SqlDbType.Char)
            objNameParam.Direction = Data.ParameterDirection.Input
            objNameParam.Value = Trim(FormListTableName)

            objIDParam = comSQL.Parameters.Add("@ParentId", Data.SqlDbType.Int)
            objIDParam.Direction = Data.ParameterDirection.Input
            objIDParam.Value = ParentId

            objNameParam = comSQL.Parameters.Add("@Name", Data.SqlDbType.Char)
            objNameParam.Direction = Data.ParameterDirection.Input
            objNameParam.Value = Trim(FolderName)

            Try
                myConnection.Open()
                comSQL.ExecuteScalar()

            Catch ex As Exception
            Finally
                ' Close the connection when done with it.
                myConnection.Close()
            End Try

        End Sub
        Sub RemoveAccessToChildFolders(ByVal FolderId As Long, ByVal UserType As Long, ByVal UserId As Long)
            Dim dt As New Data.DataTable
            Dim dr As Data.DataRow

            dt = GetFoldersAtLevel(SupportFunctions.Support.g_strFormListTable, FolderId)
            For Each dr In dt.Rows
                DeleteACLRecord(dr.Item("FormId"), UserType, UserId)
                ' do recursive call
                RemoveAccessToChildFolders(dr.Item("FormId"), UserType, UserId)
            Next
        End Sub
        Sub AllowAccessToChildFolders(ByVal FolderId As Long, ByVal UserType As Long, ByVal UserId As Long)
            Dim dt As New Data.DataTable
            Dim dr As Data.DataRow

            dt = GetFoldersAtLevel(SupportFunctions.Support.g_strFormListTable, FolderId)
            For Each dr In dt.Rows
                AddACLRecord(dr.Item("FormId"), UserType, UserId)
                ' do recursive call
                AllowAccessToChildFolders(dr.Item("FormId"), UserType, UserId)
            Next
        End Sub
        Sub AddACLRecord(ByVal FolderId As Long, ByVal UserType As Long, ByVal UserId As Long)

            Dim strConnString As String = Session("_VT_DotNetConnString")
            Dim myConnection As New SqlConnection(strConnString)
            Dim comSQL As New SqlCommand("vt_insNewACLRecord", myConnection)
            Dim myAdapter As New SqlDataAdapter(comSQL)
            Dim ds As New Data.DataSet

            Dim objParam As SqlParameter

            comSQL.CommandType = Data.CommandType.StoredProcedure
            objParam = comSQL.Parameters.Add("@FolderId", Data.SqlDbType.Int)
            objParam.Direction = Data.ParameterDirection.Input
            objParam.Value = FolderId

            objParam = comSQL.Parameters.Add("@UserType", Data.SqlDbType.Int)
            objParam.Direction = Data.ParameterDirection.Input
            objParam.Value = UserType

            objParam = comSQL.Parameters.Add("@UserId", Data.SqlDbType.Int)
            objParam.Direction = Data.ParameterDirection.Input
            objParam.Value = UserId

            myConnection.Open()
            myAdapter.Fill(ds)

            myConnection.Close()
            myConnection = Nothing

        End Sub
        Function GetFolderIdForDocId(ByVal lngDocId As Long) As Long
            Dim ds As New Data.DataSet

            ds = GetDocument(SupportFunctions.Support.g_strFormDataTable, lngDocId)
            GetFolderIdForDocId = ds.Tables(0).Rows(0).Item("FolderId")

        End Function
        Function GetDocument(ByVal strFileDataTable As String, ByVal lngDocId As Long) As Data.DataSet

            Dim strConnString As String = Session("_VT_DotNetConnString")
            Dim myConnection As New SqlConnection(strConnString)
            Dim comSQL As New SqlCommand("vt_resDocforId", myConnection)
            Dim myAdapter As New SqlDataAdapter(comSQL)
            Dim ds As New Data.DataSet

            Dim objParam As SqlParameter

            comSQL.CommandType = Data.CommandType.StoredProcedure
            objParam = comSQL.Parameters.Add("@TableName", Data.SqlDbType.NVarChar)
            objParam.Direction = Data.ParameterDirection.Input
            objParam.Value = strFileDataTable

            objParam = comSQL.Parameters.Add("@Id", Data.SqlDbType.Int)
            objParam.Direction = Data.ParameterDirection.Input
            objParam.Value = lngDocId

            myConnection.Open()
            myAdapter.Fill(ds)

            GetDocument = ds
            myConnection.Close()
            myConnection = Nothing
        End Function
        Function ExecuteSearchQuery(ByVal strquery As String) As Data.DataSet
            Dim strConnString As String = Session("_VT_DotNetConnString")
            Dim myConnection As New SqlConnection(strConnString)
            Dim comsql As New SqlCommand(strquery, myConnection)

            Dim myAdapter As New SqlDataAdapter(comsql)
            Dim ds As New Data.DataSet

            comsql.CommandType = Data.CommandType.Text

            myConnection.Open()
            myAdapter.Fill(ds)

            ExecuteSearchQuery = ds
            myConnection.Close()
            myConnection = Nothing
        End Function

        Function GetFolder(ByVal strFileListTable As String, ByVal lngFolderId As Long) As Data.DataSet

            Dim strConnString As String = Session("_VT_DotNetConnString")
            Dim myConnection As New SqlConnection(strConnString)
            Dim comSQL As New SqlCommand("vtm_resGetFolderForId", myConnection)
            Dim myAdapter As New SqlDataAdapter(comSQL)
            Dim ds As New Data.DataSet

            Dim objParam As SqlParameter

            comSQL.CommandType = Data.CommandType.StoredProcedure
            objParam = comSQL.Parameters.Add("@TableName", Data.SqlDbType.NVarChar)
            objParam.Direction = Data.ParameterDirection.Input
            objParam.Value = strFileListTable

            objParam = comSQL.Parameters.Add("@Id", Data.SqlDbType.Int)
            objParam.Direction = Data.ParameterDirection.Input
            objParam.Value = lngFolderId

            myConnection.Open()
            myAdapter.Fill(ds)

            GetFolder = ds
            myConnection.Close()
            myConnection = Nothing
        End Function

        Function GetFolderIdForName(ByVal strFileListTable As String, ByVal strFolderName As String, ByVal lngParentId As Long) As Long

            Dim strConnString As String = Session("_VT_DotNetConnString")
            Dim myConnection As New SqlConnection(strConnString)
            Dim comSQL As New SqlCommand("vtm_resGetFolderForName", myConnection)
            Dim myAdapter As New SqlDataAdapter(comSQL)
            Dim ds As New Data.DataSet

            Dim objParam As SqlParameter

            comSQL.CommandType = Data.CommandType.StoredProcedure
            objParam = comSQL.Parameters.Add("@TableName", Data.SqlDbType.NVarChar)
            objParam.Direction = Data.ParameterDirection.Input
            objParam.Value = strFileListTable

            objParam = comSQL.Parameters.Add("@Name", Data.SqlDbType.NChar)
            objParam.Direction = Data.ParameterDirection.Input
            objParam.Value = strFolderName

            objParam = comSQL.Parameters.Add("@ParentId", Data.SqlDbType.Int)
            objParam.Direction = Data.ParameterDirection.Input
            objParam.Value = lngParentId

            myConnection.Open()
            myAdapter.Fill(ds)

            If ds.Tables(0).Rows.Count > 0 Then
                GetFolderIdForName = ds.Tables(0).Rows(0).Item("FormId")
            End If

            myConnection.Close()
            myConnection = Nothing
        End Function

        Function GetACLRecord(ByVal lngFolderId As Long, ByVal lngUserType As Long, ByVal lngUserId As Long) As Data.DataSet

            Dim strConnString As String = Session("_VT_DotNetConnString")
            Dim myConnection As New SqlConnection(strConnString)
            Dim comSQL As New SqlCommand("vt_resACLRecord", myConnection)
            Dim myAdapter As New SqlDataAdapter(comSQL)
            Dim ds As New Data.DataSet

            Dim objParam As SqlParameter

            comSQL.CommandType = Data.CommandType.StoredProcedure
            objParam = comSQL.Parameters.Add("@FolderId", Data.SqlDbType.Int)
            objParam.Direction = Data.ParameterDirection.Input
            objParam.Value = lngFolderId

            objParam = comSQL.Parameters.Add("@UserType", Data.SqlDbType.Int)
            objParam.Direction = Data.ParameterDirection.Input
            objParam.Value = lngUserType

            objParam = comSQL.Parameters.Add("@UserId", Data.SqlDbType.Int)
            objParam.Direction = Data.ParameterDirection.Input
            objParam.Value = lngUserId

            myConnection.Open()
            myAdapter.Fill(ds)

            GetACLRecord = ds
            myConnection.Close()
            myConnection = Nothing
        End Function
        Function GetFolderAccess(ByVal lngFolderId As Long, ByVal lngUserId As Long, ByVal intDeptId As Int16) As Boolean


            Dim strConnString As String = Session("_VT_DotNetConnString")
            Dim myConnection As New SqlConnection(strConnString)
            Dim comSQL As New SqlCommand("vt_resAllowedFoldersAtLevel", myConnection)
            Dim myAdapter As New SqlDataAdapter(comSQL)
            Dim ds As New Data.DataSet

            Dim objParam As SqlParameter

            comSQL.CommandType = Data.CommandType.StoredProcedure

            objParam = comSQL.Parameters.Add("@TableName", Data.SqlDbType.NVarChar)
            objParam.Direction = Data.ParameterDirection.Input
            objParam.Value = SupportFunctions.Support.g_strFormListTable

            objParam = comSQL.Parameters.Add("@ParentId", Data.SqlDbType.Int)
            objParam.Direction = Data.ParameterDirection.Input
            objParam.Value = lngFolderId

            objParam = comSQL.Parameters.Add("@UserId", Data.SqlDbType.Int)
            objParam.Direction = Data.ParameterDirection.Input
            objParam.Value = lngUserId

            objParam = comSQL.Parameters.Add("@DeptId", Data.SqlDbType.Int)
            objParam.Direction = Data.ParameterDirection.Input
            objParam.Value = intDeptId

            myConnection.Open()
            Dim myReader As SqlDataReader = comSQL.ExecuteReader()

            If myReader.Read Then
                GetFolderAccess = True
            Else
                GetFolderAccess = False
            End If

            myConnection.Close()
            myConnection = Nothing
        End Function
        Function GetFoldersForAllParentsAtLevel(ByVal strFileListTable As String, ByVal ParentIds As String) As Data.DataTable
            Dim strConnString As String = Session("_VT_DotNetConnString")
            Dim myConnection As New SqlConnection(strConnString)
            Dim comSQL As New SqlCommand("Select * from " + strFileListTable + " where ParentId In " & ParentIds & " ORDER BY FolderName", myConnection)
            Dim myAdapter As New SqlDataAdapter(comSQL)
            Dim dt As New Data.DataTable


            comSQL.CommandType = Data.CommandType.Text

            myConnection.Open()
            myAdapter.Fill(dt)

            dt.DefaultView.Sort = "FormName"

            GetFoldersForAllParentsAtLevel = dt
            myConnection.Close()
            myConnection = Nothing
        End Function
        Function GetAllAllowedFoldersForAllParentsAtLevel(ByVal ParentIds As String, ByVal UserId As Int16) As Data.DataTable
            Dim dtMain, dtTemp As Data.DataTable
            Dim dr As Data.DataRow
            Dim objBPA As New VT_eQOInterface.PersonnelSQL
            Dim intDeptId As Integer = objBPA.GetDepartmentForUser(UserId)
            Dim intDivisionId As Integer = objBPA.GetDivisionForDepartment(intDeptId)

            Const cUSER = 3
            Const cDEPARTMENT = 2
            Const cDIVISION = 1


            ' no limit on folder access for super users
            If Session("_VT_SuperUser") = "YES" Or Session("_VT_SysAdmin") = "YES" Or Session("_VT_UseACL") <> "YES" Then
                GetAllAllowedFoldersForAllParentsAtLevel = GetFoldersForAllParentsAtLevel(SupportFunctions.Support.g_strFormListTable, ParentIds)
            Else
                ' Get the folders where the user has rights in her own name
                dtMain = GetAllowedFoldersForAllParentsAtLevel(SupportFunctions.Support.g_strFormListTable, ParentIds, cUSER, UserId)

                ' Get the folders where the user has rights because of her department
                dtTemp = GetAllowedFoldersForAllParentsAtLevel(SupportFunctions.Support.g_strFormListTable, ParentIds, cDEPARTMENT, intDeptId)
                ' append these records to the main table
                For Each dr In dtTemp.Rows
                    dtMain.Rows.Add(dr.ItemArray)
                Next

                ' Get the folders where the user has rights because of her division
                dtTemp = GetAllowedFoldersForAllParentsAtLevel(SupportFunctions.Support.g_strFormListTable, ParentIds, cDIVISION, intDivisionId)
                ' append these records to the main table
                For Each dr In dtTemp.Rows
                    dtMain.Rows.Add(dr.ItemArray)
                Next

                GetAllAllowedFoldersForAllParentsAtLevel = dtMain

            End If



        End Function

        Function GetAllowedFoldersForAllParentsAtLevel(ByVal strFileListTable As String, ByVal ParentIds As String, ByVal UserType As Int16, ByVal UserId As Int16) As Data.DataTable
            Dim strConnString As String = Session("_VT_DotNetConnString")
            Dim myConnection As New SqlConnection(strConnString)
            Dim comSQL As New SqlCommand("Select * from " + strFileListTable + " where ParentId In " & ParentIds & " and Id in (select folderid from vt_Folder_ACL where UserType=" & UserType.ToString & " and UserId= " & UserId.ToString & ")", myConnection)
            Dim myAdapter As New SqlDataAdapter(comSQL)
            Dim dt As New Data.DataTable


            comSQL.CommandType = Data.CommandType.Text

            myConnection.Open()
            myAdapter.Fill(dt)

            GetAllowedFoldersForAllParentsAtLevel = dt
            myConnection.Close()
            myConnection = Nothing
        End Function
        ' 
        ' Returns a DataTable containing all the Folder records
        ' with a given ParentId
        '
        Function GetFoldersAtLevel(ByVal strFileListTable As String, ByVal ParentId As Long) As Data.DataTable
            Dim strConnString As String = Session("_VT_DotNetConnString")
            Dim myConnection As New SqlConnection(strConnString)
            Dim comSQL As New SqlCommand("vtm_resGetChildFolders", myConnection)
            Dim myAdapter As New SqlDataAdapter(comSQL)
            Dim dt As New Data.DataTable

            Dim objParam As SqlParameter

            comSQL.CommandType = Data.CommandType.StoredProcedure
            objParam = comSQL.Parameters.Add("@TableName", Data.SqlDbType.NVarChar)
            objParam.Direction = Data.ParameterDirection.Input
            objParam.Value = strFileListTable

            objParam = comSQL.Parameters.Add("@ParentId", Data.SqlDbType.Int)
            objParam.Direction = Data.ParameterDirection.Input
            objParam.Value = ParentId

            myConnection.Open()
            myAdapter.Fill(dt)

            dt.DefaultView.Sort = "FormName"

            GetFoldersAtLevel = dt
            myConnection.Close()
            myConnection = Nothing
        End Function
        Function GetRootFolderId(ByVal strFileListTable As String) As Long
            Dim strConnString As String = Session("_VT_DotNetConnString")
            Dim myConnection As New SqlConnection(strConnString)
            Dim comSQL As New SqlCommand("vtm_resGetFolderForName", myConnection)
            Dim myAdapter As New SqlDataAdapter(comSQL)
            Dim dt As New Data.DataTable
            Dim objParam As SqlParameter



            comSQL.CommandType = Data.CommandType.StoredProcedure
            objParam = comSQL.Parameters.Add("@TableName", Data.SqlDbType.NVarChar)
            objParam.Direction = Data.ParameterDirection.Input
            objParam.Value = strFileListTable

            objParam = comSQL.Parameters.Add("@Name", Data.SqlDbType.NVarChar)
            objParam.Direction = Data.ParameterDirection.Input
            objParam.Value = "DocumentModuleRoot"

            objParam = comSQL.Parameters.Add("@ParentId", Data.SqlDbType.Int)
            objParam.Direction = Data.ParameterDirection.Input
            objParam.Value = 0

            myConnection.Open()
            myAdapter.Fill(dt)

            If dt.Rows.Count > 0 Then
                GetRootFolderId = dt.Rows(0).Item("FormId")
            End If

            myConnection.Close()
            myConnection = Nothing
        End Function

        ' 
        ' Returns a DataTable containing all the Folder records
        ' with a given ParentId which are accessable by the given UserType and UserId
        '
        Function GetAllowedFoldersAtLevel(ByVal strFileListTable As String, ByVal ParentId As Long, ByVal DeptId As Int16, ByVal UserId As Int16) As Data.DataTable
            Dim strConnString As String = Session("_VT_DotNetConnString")
            Dim myConnection As New SqlConnection(strConnString)
            Dim comSQL As New SqlCommand("vt_resAllowedFoldersAtLevel", myConnection)
            Dim myAdapter As New SqlDataAdapter(comSQL)
            Dim dt As New Data.DataTable

            Dim objParam As SqlParameter

            comSQL.CommandType = Data.CommandType.StoredProcedure
            objParam = comSQL.Parameters.Add("@TableName", Data.SqlDbType.NVarChar)
            objParam.Direction = Data.ParameterDirection.Input
            objParam.Value = strFileListTable

            objParam = comSQL.Parameters.Add("@ParentId", Data.SqlDbType.Int)
            objParam.Direction = Data.ParameterDirection.Input
            objParam.Value = ParentId


            objParam = comSQL.Parameters.Add("@DeptId", Data.SqlDbType.Int)
            objParam.Direction = Data.ParameterDirection.Input
            objParam.Value = DeptId

            objParam = comSQL.Parameters.Add("@UserId", Data.SqlDbType.Int)
            objParam.Direction = Data.ParameterDirection.Input
            objParam.Value = UserId


            Try
                myConnection.Open()
                myAdapter.Fill(dt)
                dt.DefaultView.Sort = "FormName"

            Catch ex As Exception

            Finally
                ' Close the connection when done with it.
                myConnection.Close()
                myConnection = Nothing
            End Try



            GetAllowedFoldersAtLevel = dt
        End Function
        Function GetAllAllowedFoldersAtLevel(ByVal ParentId As Long, ByVal UserId As Int16) As Data.DataTable
            Dim dtMain As Data.DataTable
            Dim objPer As New PersonnelModuleFunctions
            Dim intDeptId As Integer = objPer.GetUsersCategoryId(Session("_VT_DotNetConnString"), UserId)
            Dim intRowId As Integer
            Dim dtRow As Data.DataRow

            ' no limit on folder access for super users
            If Session("_VT_ShowingAccessControl") = "YES" Or Session("_VT_UseACL") <> "YES" Then
                GetAllAllowedFoldersAtLevel = GetFoldersAtLevel(SupportFunctions.Support.g_strFormListTable, ParentId)
            Else
                ' Get the folders where the user has rights 
                'dtMain = GetAllowedFoldersAtLevel(SupportFunctions.Support.g_strFormListTable, ParentId, intDeptId, UserId)
                dtMain = GetFoldersAtLevel(SupportFunctions.Support.g_strFormListTable, ParentId)
                For intRowId = dtMain.Rows.Count - 1 To 0 Step -1
                    If Not CanAccessFolder(dtMain.Rows(intRowId).Item("FormId"), UserId) Then
                        dtRow = dtMain.Rows(intRowId)
                        dtMain.Rows.Remove(dtRow)
                    End If
                Next
                '' create a keys array with which we can create a PrimaryKey for the table
                Dim keys(0) As Data.DataColumn
                '' Add the column to the array.
                keys(0) = dtMain.Columns("FormId")
                dtMain.PrimaryKey = keys

                GetAllAllowedFoldersAtLevel = dtMain

            End If
        End Function
        ' 
        ' Returns a Data.DataSet containing all the Folder records
        ' with a given ParentId
        '
        Function GetFoldersDSAtLevel(ByVal strFileListTable As String, ByVal ParentId As Long) As Data.DataSet
            Dim strConnString As String = Session("_VT_DotNetConnString")
            Dim myConnection As New SqlConnection(strConnString)
            Dim comSQL As New SqlCommand("vtm_resGetChildFolders", myConnection)
            Dim myAdapter As New SqlDataAdapter(comSQL)
            Dim dt As New Data.DataSet

            Dim objParam As SqlParameter

            comSQL.CommandType = Data.CommandType.StoredProcedure
            objParam = comSQL.Parameters.Add("@TableName", Data.SqlDbType.NVarChar)
            objParam.Direction = Data.ParameterDirection.Input
            objParam.Value = strFileListTable

            objParam = comSQL.Parameters.Add("@ParentId", Data.SqlDbType.Int)
            objParam.Direction = Data.ParameterDirection.Input
            objParam.Value = ParentId

            myConnection.Open()
            myAdapter.Fill(dt)

            GetFoldersDSAtLevel = dt
            myConnection.Close()
            myConnection = Nothing
        End Function
        Sub DeleteDocument(ByVal strFileDataTable As String, ByVal DocId As Long)
            Dim strConnString As String = Session("_VT_DotNetConnString")
            Dim myConnection As New SqlConnection(strConnString)
            Dim comSQL As New SqlCommand("vt_delDocument", myConnection)
            Dim myAdapter As New SqlDataAdapter(comSQL)
            Dim ds As New Data.DataSet

            Dim objParam As SqlParameter

            comSQL.CommandType = Data.CommandType.StoredProcedure

            objParam = comSQL.Parameters.Add("@TableName", Data.SqlDbType.NVarChar)
            objParam.Direction = Data.ParameterDirection.Input
            objParam.Value = strFileDataTable

            objParam = comSQL.Parameters.Add("@Id", Data.SqlDbType.Int)
            objParam.Direction = Data.ParameterDirection.Input
            objParam.Value = DocId

            myConnection.Open()
            myAdapter.Fill(ds)

            myConnection.Close()
            myConnection = Nothing
        End Sub

        Sub DeleteACLRecord(ByVal FolderId As Long, ByVal UserType As Long, ByVal UserId As Long)
            Dim strConnString As String = Session("_VT_DotNetConnString")
            Dim myConnection As New SqlConnection(strConnString)
            Dim comSQL As New SqlCommand("vt_delACLRecord", myConnection)
            Dim objParam As SqlParameter

            comSQL.CommandType = Data.CommandType.StoredProcedure
            objParam = comSQL.Parameters.Add("@FolderId", Data.SqlDbType.Int)
            objParam.Direction = Data.ParameterDirection.Input
            objParam.Value = FolderId

            objParam = comSQL.Parameters.Add("@UserType", Data.SqlDbType.Int)
            objParam.Direction = Data.ParameterDirection.Input
            objParam.Value = UserType

            objParam = comSQL.Parameters.Add("@UserId", Data.SqlDbType.Int)
            objParam.Direction = Data.ParameterDirection.Input
            objParam.Value = UserId

            myConnection.Open()
            comSQL.ExecuteNonQuery()

            myConnection.Close()
            myConnection = Nothing
        End Sub
        Sub DeleteFolder(ByVal strFileListTable As String, ByVal FolderId As Long)
            Dim strConnString As String = Session("_VT_DotNetConnString")
            Dim myConnection As New SqlConnection(strConnString)
            Dim comSQL As New SqlCommand("vt_delFolder", myConnection)
            Dim myAdapter As New SqlDataAdapter(comSQL)
            Dim ds As New Data.DataSet

            Dim objParam As SqlParameter

            comSQL.CommandType = Data.CommandType.StoredProcedure

            objParam = comSQL.Parameters.Add("@TableName", Data.SqlDbType.NVarChar)
            objParam.Direction = Data.ParameterDirection.Input
            objParam.Value = strFileListTable

            objParam = comSQL.Parameters.Add("@FolderId", Data.SqlDbType.Int)
            objParam.Direction = Data.ParameterDirection.Input
            objParam.Value = FolderId

            myConnection.Open()
            myAdapter.Fill(ds)

            myConnection.Close()
            myConnection = Nothing
        End Sub


        Function CanAccessFolder(ByVal intFolderId As Integer, ByVal intUserId As Integer) As Boolean
            Dim objPer As New PersonnelModuleFunctions
            Dim intDeptId As Integer = objPer.GetUsersCategoryId(Session("_VT_DotNetConnString"), intUserId)
            Dim ds As New Data.DataSet
            Dim dtAccess As New Data.DataTable


            ' if Access Control is not switched on return True
            If Session("_VT_UseACL") <> "YES" Then
                CanAccessFolder = True
                Exit Function
            End If

            ' the default is to deny access to the folder
            If Session("_VT_DefaultAccessMode") = "DENY" Then
                CanAccessFolder = False
            Else
                CanAccessFolder = True
            End If

            ' super user can see all folders
            If Session("_VT_SuperUser") = "YES" Or Session("_VT_SysAdmin") = "YES" Then
                CanAccessFolder = True
                Exit Function
            End If

            ' get the Data.DataSet for this folder
            ds = GetFolder(SupportFunctions.Support.g_strFormListTable, intFolderId)

            ' if the selected folder is the Root allow access
            If ds.Tables(0).Rows(0).Item("JobId") = 0 Then
                CanAccessFolder = True
            Else
                ' if there is no ACL entry for this folder revert to the default setting
                dtAccess = GetACLIds(intFolderId)
                If (dtAccess Is Nothing) OrElse (dtAccess.Rows.Count = 0) Then
                    If Session("_VT_DefaultAccessMode") = "DENY" Then
                        CanAccessFolder = False
                    Else
                        CanAccessFolder = True
                    End If
                    Exit Function
                Else
                    ' is the User on the Access Control List for this folder
                    dtAccess = GetACLIds(intFolderId, intUserId, intDeptId)
                    If dtAccess.Rows.Count <= 0 Then
                        ' not on list so access denied
                        CanAccessFolder = False
                    Else
                        CanAccessFolder = True
                    End If


                End If

            End If
        End Function

        Function GetACLIds(ByVal intFolderId As Integer, Optional ByVal intUserId As Integer = 0, Optional ByVal intDeptId As Integer = 0) As Data.DataTable
            Dim dt As New Data.DataTable
            Dim strConnString As String = Session("_VT_DotNetConnString")
            Dim myConnection As New SqlConnection(strConnString)
            Dim strSQL As String
            Const cUSER = 1
            Const cDEPARTMENT = 2

            strSQL = "SELECT * FROM vt_Folder_ACL WHERE FolderId = " + CStr(intFolderId)

            If intUserId <> 0 Or intDeptId <> 0 Then
                strSQL = strSQL + " AND ("
            End If
            If intUserId <> 0 Then
                strSQL = strSQL + " (UserType = " + CStr(cUSER) + " AND UserId = " + CStr(intUserId) + ")"
            End If
            If intDeptId <> 0 Then
                If intUserId <> 0 Then
                    strSQL = strSQL + " OR "
                End If
                strSQL = strSQL + " (UserType = " + CStr(cDEPARTMENT) + " AND UserId = " + CStr(intDeptId) + ")"
            End If
            If intUserId <> 0 Or intDeptId <> 0 Then
                strSQL = strSQL + ")"
            End If


            Dim comSQL As New SqlCommand(strSQL, myConnection)
            Dim myAdapter As New SqlDataAdapter(comSQL)

            comSQL.CommandType = Data.CommandType.Text

            myConnection.Open()
            myAdapter.Fill(dt)

            GetACLIds = dt

            myConnection.Close()
            myConnection = Nothing
        End Function

        Function GetLevelDataTable(ByVal TableName As String, ByVal UserId As Integer) As Data.DataTable
            Dim strConnString As String = Session("_VT_DotNetConnString")
            Dim myConnection As New SqlConnection(strConnString)
            Dim comSQL As New SqlCommand()
            Dim objBPA As New VT_eQOInterface.PersonnelSQL
            comSQL.Connection = myConnection

            Dim intDeptId As Integer = objBPA.GetDepartmentForUser(UserId)
            Dim intDivisionId As Integer = objBPA.GetDivisionForDepartment(intDeptId)

            Const cUSER = 3
            Const cDEPARTMENT = 2
            Const cDIVISION = 1

            ' no limit on folder access for super users
            If Session("_VT_SuperUser") = "YES" Or Session("_VT_SysAdmin") = "YES" Or Session("_VT_UseACL") <> "YES" Then
                comSQL.CommandText = "Select * from " + TableName + " Order By Name"
            Else
                comSQL.CommandText = "Select * from " + TableName + " WHERE Id IN (SELECT FolderId from vt_Folder_ACL WHERE (UserType=3 and Userid= " + CStr(UserId) + ") or (UserType=2 and Userid= " + CStr(intDeptId) + ") or (UserType=1 and Userid= " + CStr(intDivisionId) + "))"
                comSQL.CommandText = comSQL.CommandText + " Order By Name"
            End If

            Dim myAdapter As New SqlDataAdapter(comSQL)
            Dim dt As New Data.DataTable


            comSQL.CommandType = Data.CommandType.Text

            myConnection.Open()
            myAdapter.Fill(dt)


            GetLevelDataTable = dt
            myConnection.Close()
            myConnection = Nothing
        End Function

        Function DoesTableExist(ByVal TableName As String) As String
            Dim strConnString As String = Session("_VT_DotNetConnString")
            Dim myConnection As New SqlConnection(strConnString)
            Dim comSQL As New SqlCommand("SELECT * FROM SysObjects WHERE Name = '" + TableName + "' and xType = 'U'", myConnection)
            Dim myAdapter As New SqlDataAdapter(comSQL)
            Dim dt As New Data.DataTable


            comSQL.CommandType = Data.CommandType.Text

            myConnection.Open()
            myAdapter.Fill(dt)
            If dt.Rows.Count = 0 Then
                DoesTableExist = "NO"
            Else
                DoesTableExist = "YES"
            End If

            myConnection.Close()
            myConnection = Nothing
        End Function
    End Class

End Namespace
Namespace SalesOrdersFunctions
    Public Class SalesOrders
        Inherits MyBasePage


        Public Function getProductionForToday() As Data.DataTable
            Dim dt As New Data.DataTable
            Dim strsql As String
            Dim strconn As String
            Dim strstartDate As String = PortalFunctions.Now.Date.ToString("yyyy-MM-dd")
            Dim strEndDate As String = PortalFunctions.Now.Date.AddDays(1).ToString("yyyy-MM-dd")

            strsql = "Select tls_SalesOrderItems.ProductId, min(prd_ProductTable.Product_Name) as ProductName, " & _
            "sum(tls_SalesOrderItems.QuantityREquested - tls_SalesOrderItems.quantity) as TotalQty, " & _
            "sum(tls_SalesOrderItems.WeightRequested - tls_SalesOrderItems.weight)as TotalWgt, sum(prd_ProductTable.InStock) as InStock " & _
            "from tls_SalesOrderItems inner join tls_SalesOrders on tls_SalesOrderItems.SalesOrderId = tls_SalesOrders.SalesOrderid " & _
            "inner join prd_ProductTable on tls_SalesOrderItems.ProductId = prd_ProductTable.ProductId " & _
            "where (tls_SalesOrders.requestedDeliveryDate >= '" & strstartDate & "' and tls_SalesOrders.requestedDeliveryDate < '" & strEndDate & "') " & _
            "and (tls_SalesOrderItems.Status not like ('%Closed%') and tls_SalesOrderItems.Status not like ('%Complete%'))" & _
            "group by tls_SalesOrderItems.ProductId"

            strconn = Session("_VT_DotNetConnString")

            Dim myConnection As New SqlConnection(strconn)
            Dim comSQL As New SqlCommand(strsql, myConnection)

            Dim myAdapter As New SqlDataAdapter(comSQL)

            myConnection.Open()
            myAdapter.Fill(dt)

            getProductionForToday = dt

        End Function
        Public Function getProductionForAllOpenOrders(ByVal dteStartDate As Date, ByVal dteEndDate As Date, ByVal blnAll As Boolean) As Data.DataTable
            Dim dt As New Data.DataTable
            Dim strsql As String
            Dim strconn As String

            If blnAll = True Then ' Don't limit by date
                strsql = "Select tls_SalesOrderItems.ProductId, min(prd_ProductTable.Product_Name) as ProductName, " & _
                "sum(tls_SalesOrderItems.QuantityREquested - tls_SalesOrderItems.quantity) as TotalQty, " & _
                "sum(tls_SalesOrderItems.WeightRequested - tls_SalesOrderItems.weight)as TotalWgt, max(prd_ProductTable.InStock) as InStock " & _
                "from tls_SalesOrderItems inner join tls_SalesOrders on tls_SalesOrderItems.SalesOrderId = tls_SalesOrders.SalesOrderid " & _
                "inner join prd_ProductTable on tls_SalesOrderItems.ProductId = prd_ProductTable.ProductId " & _
                "where (tls_SalesOrderItems.Status not like ('%Closed%') and tls_SalesOrderItems.Status not like ('%Complete%'))" & _
                "group by tls_SalesOrderItems.ProductId Order By ProductName"

            Else
                Dim strstartDate As String = dteStartDate.ToString("yyyy-MM-dd") '"2008-06-15" 'Date.Today.ToString("yyyy-MM-dd")
                Dim strEndDate As String = dteEndDate.ToString("yyyy-MM-dd") ' "2008-06-18" 'Date.Today.AddDays(1).ToString("yyyy-MM-dd")


                strsql = "Select tls_SalesOrderItems.ProductId, min(prd_ProductTable.Product_Name) as ProductName, " & _
                "sum(tls_SalesOrderItems.QuantityREquested - tls_SalesOrderItems.quantity) as TotalQty, " & _
                "sum(tls_SalesOrderItems.WeightRequested - tls_SalesOrderItems.weight)as TotalWgt, max(prd_ProductTable.InStock) as InStock " & _
                "from tls_SalesOrderItems inner join tls_SalesOrders on tls_SalesOrderItems.SalesOrderId = tls_SalesOrders.SalesOrderid " & _
                "inner join prd_ProductTable on tls_SalesOrderItems.ProductId = prd_ProductTable.ProductId " & _
                "where (tls_SalesOrderItems.Status not like ('%Closed%') and tls_SalesOrderItems.Status not like ('%Complete%'))" & _
                "and (tls_SalesOrders.requestedDeliveryDate >= '" & strstartDate & "' and tls_SalesOrders.requestedDeliveryDate < '" & strEndDate & "') " & _
                "group by tls_SalesOrderItems.ProductId Order By ProductName"
            End If

            strconn = Session("_VT_DotNetConnString")

            Dim myConnection As New SqlConnection(strconn)
            Dim comSQL As New SqlCommand(strsql, myConnection)

            Dim myAdapter As New SqlDataAdapter(comSQL)

            myConnection.Open()
            myAdapter.Fill(dt)

            getProductionForAllOpenOrders = dt

        End Function

        Public Function getAllOpenOrdersForProduct(ByVal dteStartDate As Date, ByVal dteEndDate As Date, ByVal lngProductId As Long, ByVal blnAll As Boolean) As Data.DataTable
            Dim dt As New Data.DataTable
            Dim strsql As String
            Dim strconn As String

            If blnAll = True Then ' Don't limit by date
                strsql = "select tls_SalesOrders.SalesOrderNum as OrderNum, tls_SalesOrders.SalesOrderId,tls_SalesOrders.RequestedDeliveryDate as ReqDate, cus_Customers.CustomerName as DeliveryCustomer, tls_SalesOrders.Status as Status, " & _
                   "(tls_SalesOrderItems.QuantityRequested - tls_SalesOrderItems.Quantity) as reqQty , " & _
                   "(tls_SalesOrderItems.WeightREquested - tls_SalesOrderItems.Weight) as reqWgt " & _
                   "from tls_SalesORders " & _
                   "inner join cus_Customers on tls_SalesOrders.DeliveryCustomerId = cus_Customers.CustomerId " & _
                   "inner join tls_SalesOrderItems on tls_SalesOrders.SalesOrderId = tls_SalesOrderItems.SalesOrderId " & _
                   "where(tls_SalesOrderItems.ProductId = " & lngProductId & ") " & _
                   "And  (tls_SalesOrders.Status not like ('%Closed%'))"
            Else

                Dim strstartDate As String = dteStartDate.ToString("yyyy-MM-dd") '"2008-06-15" ' Date.Today.ToString("yyyy-MM-dd")
                Dim strEndDate As String = dteEndDate.ToString("yyyy-MM-dd") '"2008-06-18" 'Date.Today.AddDays(1).ToString("yyyy-MM-dd")

                strsql = "select tls_SalesOrders.SalesOrderNum as OrderNum, tls_SalesOrders.SalesOrderId,tls_SalesOrders.RequestedDeliveryDate as ReqDate, cus_Customers.CustomerName as DeliveryCustomer, tls_SalesOrders.Status as Status, " & _
                    "(tls_SalesOrderItems.QuantityRequested - tls_SalesOrderItems.Quantity) as reqQty , " & _
                    "(tls_SalesOrderItems.WeightREquested - tls_SalesOrderItems.Weight) as reqWgt " & _
                    "from tls_SalesORders " & _
                    "inner join cus_Customers on tls_SalesOrders.DeliveryCustomerId = cus_Customers.CustomerId " & _
                    "inner join tls_SalesOrderItems on tls_SalesOrders.SalesOrderId = tls_SalesOrderItems.SalesOrderId " & _
                    "where(tls_SalesOrderItems.ProductId = " & lngProductId & ") " & _
                    "And  (tls_SalesOrders.Status not like ('%Closed%'))" & _
               "And  (tls_SalesOrders.requestedDeliveryDate >= '" & strstartDate & "' and tls_SalesOrders.requestedDeliveryDate < '" & strEndDate & "')"
            End If

            strconn = Session("_VT_DotNetConnString")

            Dim myConnection As New SqlConnection(strconn)
            Dim comSQL As New SqlCommand(strsql, myConnection)

            Dim myAdapter As New SqlDataAdapter(comSQL)

            myConnection.Open()
            myAdapter.Fill(dt)

            getAllOpenOrdersForProduct = dt

            myConnection.Close()
            myConnection = Nothing

        End Function

        Public Function getTodaysOrdersForProduct(ByVal lngproductId As Long) As Data.DataTable
            Dim dt As New Data.DataTable
            Dim strsql As String
            Dim strconn As String
            Dim strstartDate As String = PortalFunctions.Now.Date.ToString("yyyy-MM-dd")
            Dim strEndDate As String = PortalFunctions.Now.Date.AddDays(1).ToString("yyyy-MM-dd")

            strsql = "select tls_SalesOrders.SalesOrderNum as OrderNum, tls_SalesOrders.SalesOrderId, cus_Customers.CustomerName as DeliveryCustomer, " & _
                "(tls_SalesOrderItems.QuantityRequested - tls_SalesOrderItems.Quantity) as reqQty , " & _
                "(tls_SalesOrderItems.WeightREquested - tls_SalesOrderItems.Weight) as reqWgt " & _
                "from tls_SalesORders " & _
                "inner join cus_Customers on tls_SalesOrders.DeliveryCustomerId = cus_Customers.CustomerId " & _
                "inner join tls_SalesOrderItems on tls_SalesOrders.SalesOrderId = tls_SalesOrderItems.SalesOrderId " & _
                "where(tls_SalesOrderItems.ProductId = " & lngproductId & ") " & _
                "And  (tls_SalesOrders.requestedDeliveryDate >= '" & strstartDate & "' and tls_SalesOrders.requestedDeliveryDate < '" & strEndDate & "')"

            strconn = Session("_VT_DotNetConnString")

            Dim myConnection As New SqlConnection(strconn)
            Dim comSQL As New SqlCommand(strsql, myConnection)

            Dim myAdapter As New SqlDataAdapter(comSQL)

            myConnection.Open()
            myAdapter.Fill(dt)

            getTodaysOrdersForProduct = dt

            myConnection.Close()
            myConnection = Nothing

        End Function





        Public Function GetSalesOrderBatchDetailsForOrderNum(ByVal strSalesOrderNum As String) As DataTable
            Dim dt As New Data.DataTable
            Dim strsql As String
            Dim strconn As String

            Dim objCommonFuncs As New BPADotNetCommonFunctions.VT_CommonFunctions.CommonFunctions
            Dim strSalesOrderJobId As String = objCommonFuncs.GetConfigItem("SalesOrderJobID")
            Dim strOrderTypes As String = objCommonFuncs.GetConfigItem("DifferentSalesOrderTypes")


            strsql = "SELECT DISTINCT wfo_BatchTable.JobId, wfo_BatchTable.ParentId, wfo_BatchTable.ModelType, wfo_BatchTable.BatchTimeStarted, wfo_BatchTable.BatchDateStarted, "
            strsql = strsql + "wfo_BatchTable.BatchDateFinished, wfo_BatchTable.LastActivity, wfo_BatchTable.BatchTimeFinished, wfo_BatchTable.CurrentWFO, wfo_BatchTable.JobStatusText, "
            strsql = strsql + "wfo_BatchTable.ExtraData1, wfo_BatchTable.ExtraData2, wfo_BatchTable.ExtraData3, wfo_BatchTable.ExtraData4, wfo_BatchTable.ExtraData5, wfo_BatchTable.OperatorId  "
            strsql = strsql + ", tls_SalesOrders.SalesOrderId, tls_SalesOrders.RequestedDeliveryDate, tls_SalesOrders.Route,tls_SalesOrders.Comment, wfo_BatchTable.SystemId, wfo_BatchTable.MatrixLinkId"
            If UCase(strOrderTypes) = "YES" Then
                strsql = strsql + ", tls_SalesOrders.Type"
            End If
            strsql = strsql + " FROM wfo_BatchTable INNER JOIN tls_SalesOrders ON wfo_BatchTable.JobId = tls_SalesOrders.SalesOrderNum "
            strsql = strsql + " where wfo_BatchTable.Programid in ( " + strSalesOrderJobId + ")  AND wfo_BatchTable.ParentId = 0 "

            strsql = strsql + " AND wfo_BatchTable.JobId = '" & strSalesOrderNum & "'"


            'Run the Query
            strconn = Session("_VT_DotNetConnString")
            Dim myConnection As New SqlConnection(strconn)
            Dim comSQL As New SqlCommand(strsql, myConnection)

            comSQL.CommandTimeout = 300

            Dim myAdapter As New SqlDataAdapter(comSQL)

            myConnection.Open()
            myAdapter.Fill(dt)

            GetSalesOrderBatchDetailsForOrderNum = dt

            '  Close the connection when done with it.
            myConnection.Close()



        End Function




        Public Function GetTSOrderIDForOrderNum(ByVal strOrderNum As String) As Integer
            Dim strsql As String
            strsql = "SELECT * from tls_SalesOrders  WHERE (SalesOrderNum = '" & strOrderNum & "')"

            Dim dt As New Data.DataTable

            Dim strconn As String

            strconn = Session("_VT_DotNetConnString")

            Dim myConnection As New SqlConnection(strconn)
            Dim comSQL As New SqlCommand(strsql, myConnection)

            Dim myAdapter As New SqlDataAdapter(comSQL)

            myConnection.Open()
            myAdapter.Fill(dt)

            If dt.Rows.Count > 0 Then

                GetTSOrderIDForOrderNum = dt.Rows(0).Item("SalesOrderID")
            Else
                GetTSOrderIDForOrderNum = 0

            End If

            myConnection.Close()
            myConnection = Nothing
        End Function
        Public Function getOrderDetailsFromHHForOrderNum(ByVal strOrderNum As String) As DataTable
            Dim strsql As String
            strsql = "SELECT * from tcd_tblSalesOrders  WHERE (tcd_tblSalesOrders.OrderNumber = '" & strOrderNum & "')"

            Dim dt As New Data.DataTable

            Dim strconn As String

            strconn = Session("_VT_DotNetConnString")

            Dim myConnection As New SqlConnection(strconn)
            Dim comSQL As New SqlCommand(strsql, myConnection)

            Dim myAdapter As New SqlDataAdapter(comSQL)

            myConnection.Open()
            myAdapter.Fill(dt)

            getOrderDetailsFromHHForOrderNum = dt

            myConnection.Close()
            myConnection = Nothing
        End Function
        Public Function GetItemNumForRecordID(ByVal lngRecordID As Long) As Long
            Dim strsql As String
            strsql = "SELECT * FROM tcd_tblSalesOrders WHERE RecordId = " & lngRecordID

            Dim dt As New Data.DataTable

            Dim strconn As String

            strconn = Session("_VT_DotNetConnString")

            Dim myConnection As New SqlConnection(strconn)
            Dim comSQL As New SqlCommand(strsql, myConnection)

            Dim myAdapter As New SqlDataAdapter(comSQL)

            myConnection.Open()
            myAdapter.Fill(dt)

            If dt.Rows.Count > 0 Then
                GetItemNumForRecordID = dt.Rows(0).Item("ItemNumber")
            End If

            myConnection.Close()
            myConnection = Nothing
        End Function
        Public Function GetOrderItemForRecordId(ByVal lngRecordId As Long) As DataTable
            Dim strsql As String
            strsql = "SELECT * FROM tcd_tblSalesOrders  WHERE RecordId = " & lngRecordId

            Dim dt As New Data.DataTable

            Dim strconn As String

            strconn = Session("_VT_DotNetConnString")

            Dim myConnection As New SqlConnection(strconn)
            Dim comSQL As New SqlCommand(strsql, myConnection)

            Dim myAdapter As New SqlDataAdapter(comSQL)

            myConnection.Open()
            myAdapter.Fill(dt)

            GetOrderItemForRecordId = dt

            myConnection.Close()
            myConnection = Nothing
        End Function
        Public Function GetDetailsGridItemsByOrder(ByVal strOrderNum As String) As DataTable
            Dim strsql As String
            '  strsql = "SELECT  tcd_tblSalesOrders.RecordId, tcd_tblSalesOrders.ProductId, tcd_tblSalesOrders.OrderedQty, tcd_tblSalesOrders.Quantity AS Fulfill_Qty, tcd_tblSalesOrders.OrderedWgt, tcd_tblSalesOrders.QtyOrWeight AS Fulfill_Weight, tcd_tblSalesOrders.TraceCode, tcd_tblSalesOrders.PriceSoldFor AS Price FROM tcd_tblSalesOrders  WHERE (tcd_tblSalesOrders.OrderNumber = '" & strOrderNum & "') and (tcd_tblSalesOrders.OrderComplete = 0)"
            strsql = "SELECT  tcd_tblSalesOrders.RecordId, tcd_tblSalesOrders.ProductId, tcd_tblSalesOrders.OrderedQty, tcd_tblSalesOrders.Quantity, tcd_tblSalesOrders.OrderedWgt, tcd_tblSalesOrders.QtyOrWeight, tcd_tblSalesOrders.TraceCode, tcd_tblSalesOrders.PriceSoldFor, tcd_tblSalesOrders.Location, tcd_tblSalesOrders.SerialNum, tcd_tblSalesOrders.Barcode FROM tcd_tblSalesOrders  WHERE (tcd_tblSalesOrders.OrderNumber = '" & strOrderNum & "') and (tcd_tblSalesOrders.OrderComplete = 0) Order By ProductId"

            Dim dt As New Data.DataTable

            Dim strconn As String

            strconn = Session("_VT_DotNetConnString")

            Dim myConnection As New SqlConnection(strconn)
            Dim comSQL As New SqlCommand(strsql, myConnection)

            Dim myAdapter As New SqlDataAdapter(comSQL)

            myConnection.Open()
            myAdapter.Fill(dt)

            GetDetailsGridItemsByOrder = dt

            myConnection.Close()
            myConnection = Nothing
        End Function
        Sub InsertFulfillSOToTrace(ByVal PONumber As String, ByVal OrderDate As Date, ByVal customerId As Long, ByVal ItemNumber As Long, ByVal ProductCode As String, _
         ByVal Weight As Double, ByVal Quantity As Long, ByVal IsBackorder As Boolean, ByVal OrderNumber As Integer, ByVal OrderComplete As Integer, ByVal DriverId As Integer, _
        ByVal TransactionType As Integer, ByVal CommodityCode As String, ByVal SalesOrderItemId As Integer, ByVal SaleType As Integer, ByVal TraceCode As String, ByVal VAT As Double, _
              ByVal Price As Double, ByVal lngDeliveryCustomerId As Long, ByVal lngLocationId As Long, ByVal strChargedByType As String, Optional ByVal strSerialNum As String = "", _
              Optional ByVal strBarcode As String = "")

            Dim strConnString As String = Session("_VT_DotNetConnString")
            Dim myConnection As New SqlConnection(strConnString)

            Dim comSQL As New SqlCommand("tcd_insFulfillSalesOrderItem", myConnection)
            Dim myAdapter As New SqlDataAdapter(comSQL)
            Dim ds As New DataSet

            Dim objParam As SqlParameter

            comSQL.CommandType = CommandType.StoredProcedure

            objParam = comSQL.Parameters.Add("@OrderNumber", SqlDbType.Int)
            objParam.Direction = ParameterDirection.Input
            objParam.Value = OrderNumber

            objParam = comSQL.Parameters.Add("@OrderDate", SqlDbType.DateTime)
            objParam.Direction = ParameterDirection.Input
            objParam.Value = OrderDate

            objParam = comSQL.Parameters.Add("@CustomerId", SqlDbType.Int)
            objParam.Direction = ParameterDirection.Input
            objParam.Value = customerId

            objParam = comSQL.Parameters.Add("@ProductCode", SqlDbType.NVarChar)
            objParam.Direction = ParameterDirection.Input
            objParam.Value = ProductCode

            objParam = comSQL.Parameters.Add("@QtyOrWeight", SqlDbType.Float)
            objParam.Direction = ParameterDirection.Input
            objParam.Value = Weight

            objParam = comSQL.Parameters.Add("@Quantity", SqlDbType.Int)
            objParam.Direction = ParameterDirection.Input
            objParam.Value = Quantity

            objParam = comSQL.Parameters.Add("@OrderComplete", SqlDbType.Int)
            objParam.Direction = ParameterDirection.Input
            objParam.Value = OrderComplete

            objParam = comSQL.Parameters.Add("@DriverId", SqlDbType.Int)
            objParam.Direction = ParameterDirection.Input
            objParam.Value = DriverId

            objParam = comSQL.Parameters.Add("@TransactionType", SqlDbType.Int)
            objParam.Direction = ParameterDirection.Input
            objParam.Value = TransactionType

            objParam = comSQL.Parameters.Add("@CommodityCode", SqlDbType.NVarChar)
            objParam.Direction = ParameterDirection.Input
            objParam.Value = CommodityCode

            objParam = comSQL.Parameters.Add("@PONumber", SqlDbType.NVarChar)
            objParam.Direction = ParameterDirection.Input
            objParam.Value = PONumber

            objParam = comSQL.Parameters.Add("@ItemId", SqlDbType.Int)
            objParam.Direction = ParameterDirection.Input
            objParam.Value = SalesOrderItemId

            objParam = comSQL.Parameters.Add("@SaleType", SqlDbType.Int)
            objParam.Direction = ParameterDirection.Input
            objParam.Value = SaleType

            objParam = comSQL.Parameters.Add("@TraceCode", SqlDbType.NVarChar)
            objParam.Direction = ParameterDirection.Input
            objParam.Value = TraceCode

            objParam = comSQL.Parameters.Add("@VAT", SqlDbType.Float)
            objParam.Direction = ParameterDirection.Input
            'objParam.Value = Val(FormatCurrency(VAT, 2))
            objParam.Value = VAT

            objParam = comSQL.Parameters.Add("@Price", SqlDbType.Float)
            objParam.Direction = ParameterDirection.Input
            'objParam.Value = Val(FormatCurrency(Price, 2))
            objParam.Value = Price

            objParam = comSQL.Parameters.Add("@DeliveryCustomerId", SqlDbType.Int)
            objParam.Direction = ParameterDirection.Input
            objParam.Value = lngDeliveryCustomerId

            objParam = comSQL.Parameters.Add("@Origin", SqlDbType.NVarChar)
            objParam.Direction = ParameterDirection.Input
            objParam.Value = "Telesales"

            objParam = comSQL.Parameters.Add("@LocationId", SqlDbType.Int)
            objParam.Direction = ParameterDirection.Input
            objParam.Value = lngLocationId

            objParam = comSQL.Parameters.Add("@ChargedByType", SqlDbType.NChar)
            objParam.Direction = ParameterDirection.Input
            objParam.Value = strChargedByType

            objParam = comSQL.Parameters.Add("@SerialNum", SqlDbType.NVarChar)
            objParam.Direction = ParameterDirection.Input
            objParam.Value = strSerialNum

            objParam = comSQL.Parameters.Add("@Barcode", SqlDbType.NVarChar)
            objParam.Direction = ParameterDirection.Input
            objParam.Value = strBarcode

            myConnection.Open()
            myAdapter.Fill(ds)

            myConnection.Close()
            myConnection = Nothing
        End Sub

        Public Function InitialiseEmptyMatrixSalesOrderItems() As DataTable
            'SmcN 04/06/2014 This function sets up an empty datatable structure for SalesOrder Items
            '   The name of this structure (i.e. datatable) is set to  wdgOrderItems for compatibility with the older method that used to use a hidden WebDataGrid as the structure.

            Dim dtItemsForGrid As New DataTable

            dtItemsForGrid.TableName = "wdgOrderItems"

            dtItemsForGrid.Columns.Add("SalesItemNum")          '0
            dtItemsForGrid.Columns.Add("ProductCode")           '1
            dtItemsForGrid.Columns.Add("CustomerCode")          '2
            dtItemsForGrid.Columns.Add("CustomerRev")           '3
            dtItemsForGrid.Columns.Add("ProductName")           '4
            dtItemsForGrid.Columns.Add("Site")                  '5
            dtItemsForGrid.Columns.Add("QuantityRequested")     '6
            dtItemsForGrid.Columns.Add("txtUoM")                '7
            dtItemsForGrid.Columns.Add("WeightRequested")       '8
            dtItemsForGrid.Columns.Add("PO_UnitPrice")          '9
            dtItemsForGrid.Columns.Add("PO_TotalPrice")         '10
            dtItemsForGrid.Columns.Add("PriceDifference")       '11
            dtItemsForGrid.Columns.Add("UnitPrice")             '12
            dtItemsForGrid.Columns.Add("TotalExclVAT")          '13
            dtItemsForGrid.Columns.Add("VAT")                   '14
            dtItemsForGrid.Columns.Add("TotalPrice")            '15
            dtItemsForGrid.Columns.Add("DimLength")             '16
            dtItemsForGrid.Columns.Add("DimWidth")              '17
            dtItemsForGrid.Columns.Add("Item_DateRequested")    '18
            dtItemsForGrid.Columns.Add("Item_DateOut")          '19
            dtItemsForGrid.Columns.Add("Item_DateArrival")      '20
            dtItemsForGrid.Columns.Add("Comment")               '21
            dtItemsForGrid.Columns.Add("Item_QuoteReference")   '22
            dtItemsForGrid.Columns.Add("VATRate")               '23
            dtItemsForGrid.Columns.Add("Item_SalesDBUnit")      '24
            dtItemsForGrid.Columns.Add("Item_SalesDBTotal")     '25
            dtItemsForGrid.Columns.Add("Item_CustomUnit")       '26
            dtItemsForGrid.Columns.Add("Item_CustomTotal")      '27
            dtItemsForGrid.Columns.Add("Item_PartNumGrandTotal")            '28
            dtItemsForGrid.Columns.Add("Item_OnHold")                       '29
            dtItemsForGrid.Columns.Add("Item_OnHoldDate")                   '30
            dtItemsForGrid.Columns.Add("Item_OnHoldPersonResponsible")      '31
            dtItemsForGrid.Columns.Add("Item_OnHoldPersonResponsibleID")    '32
            dtItemsForGrid.Columns.Add("Item_OnHoldTVComment")  '33
            dtItemsForGrid.Columns.Add("ProductId")             '34
            dtItemsForGrid.Columns.Add("SalesOrderItemId")      '35
            dtItemsForGrid.Columns.Add("TraceCodeDesc")         '36
            dtItemsForGrid.Columns.Add("LocationId")            '37
            dtItemsForGrid.Columns.Add("AdditionOrder")         '38
            dtItemsForGrid.Columns.Add("UnitOfSale")            '39
            dtItemsForGrid.Columns.Add("ProductSellBy")         '40
            dtItemsForGrid.Columns.Add("UnitPriceCurrency")     '41
            dtItemsForGrid.Columns.Add("ProductCategory")       '42
            dtItemsForGrid.Columns.Add("VT_UniqueIndex")        '43
            dtItemsForGrid.Columns.Add("SalesOrderNum")         '44
            dtItemsForGrid.Columns.Add("SalesOrderId")          '45
            dtItemsForGrid.Columns.Add("SO_ContiguousNum")      '46
            dtItemsForGrid.Columns.Add("CustomerId")            '47


            InitialiseEmptyMatrixSalesOrderItems = dtItemsForGrid

        End Function



        Public Function CheckIfCustPOExists(ByVal CustomerId As Integer, ByVal CustomerPO As String) As Integer

            'SmcN 07/03/2014  This function checks if a combination of CustomerID and Customer PO already exists in the system
            'This can be used to avoid the creation of duplicate Sales Orders in the system that referecne the Same Customer ID and Customer PO
            'This function returns the SalesOrderNum of the first sales order that matches the input parameters if it exists. Other vise it returns 0 if nothing found

            Dim dt As New DataTable
            Dim strConnString As String = Session("_VT_DotNetConnString")
            Dim myConnection As New SqlConnection(strConnString)
            Dim objCust As New VTDBFunctions.VTDBFunctions.CustomerFunctions
            Dim strCustCode As String = objCust.GetCustomerRefForId(CustomerId)
            Dim strsql As String

            strsql = "SELECT * FROM tls_SalesOrders WHERE CustomerId = '" & CustomerId.ToString & "' AND CustomerPO = '" & CustomerPO & "'"

            Dim comSQL As New SqlCommand(strsql, myConnection)
            Dim myAdapter As New SqlDataAdapter(comSQL)

            myConnection.Open()
            myAdapter.Fill(dt)

            If dt.Rows.Count > 0 Then
                CheckIfCustPOExists = dt.Rows(0).Item("SalesOrderNum")
            Else
                'check in MFGPro
                Dim objMFG As New SteripackMFGProInterface.MFGProFunctions
                Dim strMFGProConnString As String = System.Configuration.ConfigurationManager.AppSettings("MFGPro_DB_CONN")
                Dim strMFGProDomain As String = System.Configuration.ConfigurationManager.AppSettings("MFGPro_Domain")
                Dim strMFGProError As String = ""

                strsql = String.Format("SELECT  a.sod_nbr, b.so_bill, a.sod_contr_id FROM PUB.sod_det a, PUB.so_mstr b WHERE b.so_domain = a.sod_domain AND b.so_nbr = a.sod_nbr and b.so_bill = '" & strCustCode & "' and a.sod_contr_id ='" & CustomerPO & "'")

                'Session("MfgProLive") = "NO"


                Dim dtSO As DataTable = objMFG.GetDataTableFromMFGPro(strMFGProConnString, strsql, strMFGProError)
                If dtSO.Rows.Count > 0 Then
                    CheckIfCustPOExists = dtSO.Rows(0).Item("sod_nbr")
                Else
                    CheckIfCustPOExists = 0
                End If

            End If

            myConnection.Close()
            myConnection = Nothing


        End Function

        Public Sub DeleteOrderItemFromTCDTable(ByVal lngRecordId As Long)
            Dim strConnString As String = Session("_VT_DotNetConnString")
            Dim myConnection As New SqlConnection(strConnString)

            Dim strsql As String

            strsql = "delete from tcd_tblSalesOrders where RecordId =" & lngRecordId

            Dim comSQL As New SqlCommand(strsql, myConnection)
            Dim myAdapter As New SqlDataAdapter(comSQL)

            myConnection.Open()
            comSQL.ExecuteNonQuery()

            '  Close the connection when done with it.
            myConnection.Close()
            myConnection = Nothing


        End Sub
        Public Sub SaveOrderItemChanges(ByVal lngRecordId As Long, ByVal strTraceCode As String, ByVal dblFulfillQty As Double, ByVal dblFulfillWgt As Double, ByVal lngLocationId As Long, _
                                        ByVal strPiceSoldFor As String, ByVal strVATCharged As String, ByVal dblAllocated As Double, Optional ByVal StrSerialNum As String = "", Optional ByVal strBarcode As String = "")

            Dim strConnString As String = Session("_VT_DotNetConnString")
            Dim myConnection As New SqlConnection(strConnString)

            Dim strsql As String

            If UCase(Trim(strVATCharged)) = "NAN" Then
                strVATCharged = "0"
            End If

            strsql = "Update tcd_tblSalesOrders set Tracecode = '" & strTraceCode & "', Quantity = " & dblFulfillQty & ", QtyOrWeight =" & dblFulfillWgt & ", "
            strsql = strsql & " Location =" & lngLocationId & ", DateOfTransaction = '" & PortalFunctions.Now & "', PriceSoldFor = '" & strPiceSoldFor & "',"
            strsql = strsql & " VATCharged = " & strVATCharged & ", Allocated = " & dblAllocated & ", "
            strsql = strsql & " SerialNum = '" & UCase(StrSerialNum) & "', CommodityCode = '" & strBarcode & "' , Barcode = '" & strBarcode & "'  where RecordId =" & lngRecordId


            Dim comSQL As New SqlCommand(strsql, myConnection)
            Dim myAdapter As New SqlDataAdapter(comSQL)

            myConnection.Open()
            comSQL.ExecuteNonQuery()

            '  Close the connection when done with it.
            myConnection.Close()
            myConnection = Nothing


        End Sub

        Public Function GetDetailsGridItemsByProduct(ByVal strProductCode As String) As DataTable
            Dim strsql As String
            strsql = "SELECT  tcd_tblSalesOrders.RecordId, tcd_tblSalesOrders.OrderNumber, tcd_tblSalesOrders.OrderedQty, tcd_tblSalesOrders.Quantity, tcd_tblSalesOrders.OrderedWgt, tcd_tblSalesOrders.QtyOrWeight, tcd_tblSalesOrders.TraceCode, tcd_tblSalesOrders.PriceSoldFor,  cus_Customers.CustomerName, tcd_tblLocations.LocationText, tcd_tblSalesOrders.ProductId, tcd_tblSalesOrders.SerialNum, tcd_tblSalesOrders.Barcode FROM  tcd_tblSalesOrders INNER JOIN cus_Customers ON tcd_tblSalesOrders.CustomerID = cus_Customers.CustomerID INNER JOIN tcd_tblLocations ON tcd_tblSalesOrders.Location = tcd_tblLocations.LocationId WHERE (tcd_tblSalesOrders.ProductId = '" & strProductCode & "') and (tcd_tblSalesOrders.OrderComplete = 0)"

            Dim dt As New Data.DataTable

            Dim strconn As String

            strconn = Session("_VT_DotNetConnString")

            Dim myConnection As New SqlConnection(strconn)
            Dim comSQL As New SqlCommand(strsql, myConnection)

            Dim myAdapter As New SqlDataAdapter(comSQL)

            myConnection.Open()
            myAdapter.Fill(dt)

            GetDetailsGridItemsByProduct = dt

            myConnection.Close()
            myConnection = Nothing
        End Function

        Public Function GetRecordsForIds(ByVal strIds As String) As DataTable
            Dim strsql As String
            strsql = "SELECT  * FROM  tcd_tblSalesOrders WHERE tcd_tblSalesOrders.RecordId in(" & strIds & ")"

            Dim dt As New Data.DataTable

            Dim strconn As String

            strconn = Session("_VT_DotNetConnString")

            Dim myConnection As New SqlConnection(strconn)
            Dim comSQL As New SqlCommand(strsql, myConnection)

            Dim myAdapter As New SqlDataAdapter(comSQL)

            myConnection.Open()
            myAdapter.Fill(dt)

            GetRecordsForIds = dt

            myConnection.Close()
            myConnection = Nothing
        End Function

        Public Function GetAllTraceLocations() As DataTable
            Dim strsql As String

            strsql = "select * from trc_Locations"
            Dim dt As New Data.DataTable

            Dim strconn As String

            strconn = Session("_VT_DotNetConnString")

            Dim myConnection As New SqlConnection(strconn)
            Dim comSQL As New SqlCommand(strsql, myConnection)

            Dim myAdapter As New SqlDataAdapter(comSQL)

            myConnection.Open()
            myAdapter.Fill(dt)

            If dt.Rows.Count > 0 Then
                GetAllTraceLocations = dt
            End If

            myConnection.Close()
            myConnection = Nothing
        End Function
        Public Function GetTraceLocationIDForName(ByVal strLocation As String) As Long
            Dim strsql As String
            Dim lngLocation As Long

            strsql = "select LocationID from tcd_tblLocations where LocationText = '" & strLocation & "'"
            Dim dt As New Data.DataTable

            Dim strconn As String

            strconn = Session("_VT_DotNetConnString")

            Dim myConnection As New SqlConnection(strconn)
            Dim comSQL As New SqlCommand(strsql, myConnection)

            Dim myAdapter As New SqlDataAdapter(comSQL)

            myConnection.Open()
            myAdapter.Fill(dt)

            If dt.Rows.Count > 0 Then
                GetTraceLocationIDForName = dt.Rows(0).Item(0)
            End If

            myConnection.Close()
            myConnection = Nothing

        End Function
        Public Function GetDetailsGridItemsByLocation(ByVal strLocation As String, ByVal strProductCode As String) As DataTable
            Dim strsql As String
            Dim lngLocation As Long
            lngLocation = GetTraceLocationIDForName(strLocation)

            strsql = "SELECT tcd_tblSalesOrders.RecordId, tcd_tblSalesOrders.OrderNumber, tcd_tblSalesOrders.OrderedQty, tcd_tblSalesOrders.Quantity, tcd_tblSalesOrders.OrderedWgt, tcd_tblSalesOrders.QtyOrWeight, tcd_tblSalesOrders.TraceCode, cus_Customers.CustomerName, tcd_tblSalesOrders.ProductId, tcd_tblSalesOrders.SerialNum, tcd_tblSalesOrders.Barcode FROM tcd_tblSalesOrders INNER JOIN cus_Customers ON tcd_tblSalesOrders.CustomerID = cus_Customers.CustomerID WHERE (tcd_tblSalesOrders.ProductId = '" & strProductCode & "') AND (tcd_tblSalesOrders.Location = " & lngLocation & ") and (tcd_tblSalesOrders.OrderComplete = 0)"

            Dim dt As New Data.DataTable

            Dim strconn As String

            strconn = Session("_VT_DotNetConnString")

            Dim myConnection As New SqlConnection(strconn)
            Dim comSQL As New SqlCommand(strsql, myConnection)

            Dim myAdapter As New SqlDataAdapter(comSQL)

            myConnection.Open()
            myAdapter.Fill(dt)

            GetDetailsGridItemsByLocation = dt

            myConnection.Close()
            myConnection = Nothing
        End Function

        Public Function GetTodaysOrderItemsByOrder(Optional ByVal blnPCOnly As Boolean = False) As DataTable
            Dim strsql As String


            If Me.CurrentSession.VT_AllowOrderEditBeforeCommit = True Then
                If blnPCOnly = False Then
                    strsql = "Select distinct tcd_tblSalesOrders.OrderNumber, cus_Customers.CustomerName,  tcd_tblOrderExtras.RequestedDeliveryDate, tcd_tblOrderExtras.Priority, tcd_tblSalesOrders.OrderComplete, tcd_tblSalesOrders.DocketStatus  from tcd_tblSalesOrders, cus_Customers,  tcd_tblOrderExtras where (tcd_tblSalesOrders.deliveryCustomerId = cus_Customers.CustomerId) and (tcd_tblOrderExtras.OrderNumber = tcd_tblSalesOrders.OrderNumber  ) AND (tcd_tblSalesOrders.DocketStatus IS NULL OR tcd_tblSalesOrders.DocketStatus <> '" & GetGlobalResourceObject("Resource", "Docket_AwaitingCommitment") & "') Order By tcd_tblSalesOrders.OrderNumber"  'MLHIDE
                Else
                    strsql = "Select distinct tcd_tblSalesOrders.OrderNumber, cus_Customers.CustomerName,  tcd_tblOrderExtras.RequestedDeliveryDate, tcd_tblOrderExtras.Priority, tcd_tblSalesOrders.OrderComplete, tcd_tblSalesOrders.PCOnly, tcd_tblSalesOrders.DocketStatus  from tcd_tblSalesOrders, cus_Customers, tcd_tblOrderExtras where (tcd_tblSalesOrders.deliveryCustomerId = cus_Customers.CustomerId) and (tcd_tblOrderExtras.OrderNumber = tcd_tblSalesOrders.OrderNumber  ) and (tcd_tblSalesOrders.PCOnly = 1 ) AND (tcd_tblSalesOrders.DocketStatus IS NULL OR tcd_tblSalesOrders.DocketStatus <> '" & GetGlobalResourceObject("Resource", "Docket_AwaitingCommitment") & "') Order By tcd_tblSalesOrders.OrderNumber"  'MLHIDE
                End If
            Else
                'strsql = "SELECT DISTINCT tcd_tblSalesOrders.OrderNumber, cus_Customers.CustomerName FROM tcd_tblSalesOrders INNER JOIN cus_Customers ON tcd_tblSalesOrders.CustomerID = cus_Customers.CustomerID WHERE (tcd_tblSalesOrders.OrderComplete = 0)"
                If blnPCOnly = False Then
                    strsql = "Select distinct tcd_tblSalesOrders.OrderNumber, cus_Customers.CustomerName,  tcd_tblOrderExtras.RequestedDeliveryDate, tcd_tblOrderExtras.Priority, tcd_tblSalesOrders.OrderComplete  from tcd_tblSalesOrders, cus_Customers, tcd_tblOrderExtras where (tcd_tblSalesOrders.deliveryCustomerId = cus_Customers.CustomerId) and (tcd_tblOrderExtras.OrderNumber = tcd_tblSalesOrders.OrderNumber  ) Order By tcd_tblSalesOrders.OrderNumber"  'MLHIDE
                Else
                    strsql = "Select distinct tcd_tblSalesOrders.OrderNumber, cus_Customers.CustomerName,  tcd_tblOrderExtras.RequestedDeliveryDate, tcd_tblOrderExtras.Priority, tcd_tblSalesOrders.OrderComplete, tcd_tblSalesOrders.PCOnly  from tcd_tblSalesOrders, cus_Customers, tcd_tblOrderExtras where (tcd_tblSalesOrders.deliveryCustomerId = cus_Customers.CustomerId) and (tcd_tblOrderExtras.OrderNumber = tcd_tblSalesOrders.OrderNumber  ) and (tcd_tblSalesOrders.PCOnly = 1 ) Order By tcd_tblSalesOrders.OrderNumber"  'MLHIDE

                End If
            End If
            Dim dt As New Data.DataTable

            Dim strconn As String

            strconn = Session("_VT_DotNetConnString")

            Dim myConnection As New SqlConnection(strconn)
            Dim comSQL As New SqlCommand(strsql, myConnection)

            Dim myAdapter As New SqlDataAdapter(comSQL)

            myConnection.Open()
            myAdapter.Fill(dt)

            GetTodaysOrderItemsByOrder = dt

            myConnection.Close()
            myConnection = Nothing

        End Function
        Public Function GetTodaysOrderItemsByProduct() As DataTable
            Dim strsql As String


            If Me.CurrentSession.VT_AllowOrderEditBeforeCommit = True Then
                strsql = "SELECT DISTINCT tcd_tblSalesOrders.ProductId, prd_ProductTable.Product_Name FROM tcd_tblSalesOrders INNER JOIN prd_ProductTable ON tcd_tblSalesOrders.ProductId = prd_ProductTable.Catalog_Number WHERE (tcd_tblSalesOrders.OrderComplete = 0) AND (tcd_tblSalesOrders.DocketStatus IS NULL OR tcd_tblSalesOrders.DocketStatus <> '" & GetGlobalResourceObject("Resource", "Docket_AwaitingCommitment") & "')"

            Else
                strsql = "SELECT DISTINCT tcd_tblSalesOrders.ProductId, prd_ProductTable.Product_Name FROM tcd_tblSalesOrders INNER JOIN prd_ProductTable ON tcd_tblSalesOrders.ProductId = prd_ProductTable.Catalog_Number WHERE (tcd_tblSalesOrders.OrderComplete = 0)"
            End If




            Dim dt As New Data.DataTable

            Dim strconn As String

            strconn = Session("_VT_DotNetConnString")

            Dim myConnection As New SqlConnection(strconn)
            Dim comSQL As New SqlCommand(strsql, myConnection)

            Dim myAdapter As New SqlDataAdapter(comSQL)

            myConnection.Open()
            myAdapter.Fill(dt)

            GetTodaysOrderItemsByProduct = dt

            myConnection.Close()
            myConnection = Nothing
        End Function
        Public Function GetTodaysOrderItemsByLocation() As DataTable
            Dim strsql As String


            If Me.CurrentSession.VT_AllowOrderEditBeforeCommit = True Then

                strsql = "SELECT DISTINCT tcd_tblLocations.LocationText, prd_ProductTable.Catalog_Number, prd_ProductTable.Product_Name, "
                strsql = strsql & " tcd_tblsalesorders.OrderedWgt, tcd_tblsalesorders.OrderedQty, tcd_tblsalesorders.Quantity , tcd_tblsalesorders.qtyorWeight "
                strsql = strsql & " FROM tcd_tblSalesOrders INNER JOIN tcd_tblLocations ON tcd_tblSalesOrders.Location = tcd_tblLocations.LocationId "
                strsql = strsql & " INNER JOIN prd_ProductTable ON tcd_tblSalesOrders.ProductId = prd_ProductTable.Catalog_Number Where OrderComplete=0 AND (tcd_tblSalesOrders.DocketStatus IS NULL OR tcd_tblSalesOrders.DocketStatus <> '" & GetGlobalResourceObject("Resource", "Docket_AwaitingCommitment") & "')"

            Else

                strsql = "SELECT DISTINCT tcd_tblLocations.LocationText, prd_ProductTable.Catalog_Number, prd_ProductTable.Product_Name, "
                strsql = strsql & " tcd_tblsalesorders.OrderedWgt, tcd_tblsalesorders.OrderedQty, tcd_tblsalesorders.Quantity , tcd_tblsalesorders.qtyorWeight "
                strsql = strsql & " FROM tcd_tblSalesOrders INNER JOIN tcd_tblLocations ON tcd_tblSalesOrders.Location = tcd_tblLocations.LocationId "
                strsql = strsql & " INNER JOIN prd_ProductTable ON tcd_tblSalesOrders.ProductId = prd_ProductTable.Catalog_Number Where OrderComplete=0"

            End If

            'strsql = "SELECT  tcd_tblproducts.ProductDesc, tcd_tblsalesorders.Quantity , tcd_tblsalesorders.qtyorWeight, tcd_tblsalesorders.OrderedWgt, tcd_tblsalesorders.OrderedQty,tcd_tblproducts.ProductCode , tcd_tblproducts.PricingMethod , tcd_tblsalesorders.TraceCode , tcd_tblsalesorders.Location, tcd_tbllocations.LocationText, tcd_tblsalesorders.OrderNumber, tcd_tblsalesorders.rowguid, tcd_tblsalesorders.OrderComplete ,tcd_tblsalesorders.CustomerID, tcd_tblCustomers.customername, tcd_tblsalesorders.allocated, tcd_tblSalesOrders.Origin " 'MLHIDE
            'strsql = strsql & " FROM  tcd_tblsalesorders inner join tcd_tbllocations on tcd_tblsalesorders.location = tcd_tbllocations.locationid" 'MLHIDE
            'strsql = strsql & " Join tcd_tblproducts on tcd_tblsalesorders.productid = tcd_tblproducts.productcode " 'MLHIDE
            'strsql = strsql & " Join tcd_tblcustomers on tcd_tblsalesorders.customerid = tcd_tblcustomers.customerid order by tcd_tbllocations.LocationText, tcd_tblproducts.ProductCode" 'MLHIDE


            'Const LOCATIONDESC As Integer = 0
            'Const PRODUCTDESC As Integer = 1
            'Const ORDEREDQUANTITY As Integer = 2
            'Const ORDEREDWEIGHT As Integer = 3
            'Const QUANTITY As Integer = 4
            'Const WEIGHT As Integer = 5
            'Const ISCOMPLETE As Integer = 6
            'Const LOCATIONID As Integer = 7
            'Const PRODUCTCODE As Integer = 8




            Dim dt As New Data.DataTable

            Dim strconn As String

            strconn = Session("_VT_DotNetConnString")

            Dim myConnection As New SqlConnection(strconn)
            Dim comSQL As New SqlCommand(strsql, myConnection)

            Dim myAdapter As New SqlDataAdapter(comSQL)

            myConnection.Open()
            myAdapter.Fill(dt)

            GetTodaysOrderItemsByLocation = dt

            myConnection.Close()
            myConnection = Nothing
        End Function
        Public Function GetItemsOnHandheld(ByVal lngSalesOrderNum As Long) As DataTable


            Dim dt As New DataTable
            Dim dsSync As New DataSet

            Dim objCommonFuncs As New BPADotNetCommonFunctions.VT_CommonFunctions.CommonFunctions
            Dim strUsesSyncService As String = objCommonFuncs.GetConfigItem("UsesSyncService")

            If UCase(strUsesSyncService) = "YES" Then
                Dim strsql As String
                Dim strconnOrder As String

                strsql = "SELECT * from tcd_tblSalesOrders where OrderNumber =  " & lngSalesOrderNum

                strconnOrder = Session("_VT_DotNetConnString")

                Dim myConnectionOrder As New SqlConnection(strconnOrder)
                Dim comSQLOrder As New SqlCommand(strsql, myConnectionOrder)

                Dim myAdapterOrder As New SqlDataAdapter(comSQLOrder)

                myConnectionOrder.Open()
                myAdapterOrder.Fill(dt)


                '  Close the connection when done with it.
                myConnectionOrder.Close()
                myConnectionOrder = Nothing


                dt.Columns.Add("Synched", System.Type.GetType("System.String"))
                dt.Columns.Add("Return", System.Type.GetType("System.String"))

                For Each drRow As DataRow In dt.Rows

                    If IsDBNull(drRow.Item("DateTimeSynced")) = True Then
                        drRow.Item("Synched") = "No"
                    Else
                        drRow.Item("Synched") = "Yes"
                    End If


                    If drRow.Item("SaleType") = 2 Then
                        drRow.Item("Return") = "This is a Return or a Negative Adjustment"
                    Else
                        drRow.Item("Return") = ""
                    End If
                Next

                dt.AcceptChanges()

            Else
                Dim strConnString As String = Session("_VT_DotNetConnString")
                Dim myConnection As New SqlConnection(strConnString)
                Dim comSQL As New SqlCommand("PBS_sp_ShowPendingMergeChanges", myConnection)
                Dim objParam As SqlParameter
                Dim myAdapter As New SqlDataAdapter(comSQL)

                comSQL.CommandType = Data.CommandType.StoredProcedure
                objParam = comSQL.Parameters.Add("@p_articlename", Data.SqlDbType.NVarChar)
                objParam.Direction = Data.ParameterDirection.Input
                objParam.Value = "tcd_tblSalesOrders"

                myConnection.Open()
                myAdapter.Fill(dsSync)
                myConnection.Close()
                myConnection = Nothing


                Dim strsql As String
                Dim strconnOrder As String


                strsql = "SELECT * from tcd_tblSalesOrders where OrderNumber =  " & lngSalesOrderNum


                strconnOrder = Session("_VT_DotNetConnString")

                Dim myConnectionOrder As New SqlConnection(strconnOrder)
                Dim comSQLOrder As New SqlCommand(strsql, myConnectionOrder)

                Dim myAdapterOrder As New SqlDataAdapter(comSQLOrder)

                myConnectionOrder.Open()
                myAdapterOrder.Fill(dt)



                '  Close the connection when done with it.
                myConnectionOrder.Close()
                myConnectionOrder = Nothing


                dt.Columns.Add("Synched", System.Type.GetType("System.String"))
                dt.Columns.Add("Return", System.Type.GetType("System.String"))

                For Each drRow As DataRow In dt.Rows

                    Dim strExpression As String
                    Dim foundRows() As DataRow

                    ' Use the Select method to find all rows matching the filter.
                    strExpression = "PrimaryKey =  '" & drRow.Item("ROWGUID").ToString & "'"
                    foundRows = dsSync.Tables(0).Select(strExpression)

                    If foundRows.GetUpperBound(0) > -1 Then
                        If foundRows(0).Item("Type") <> "Del" Then
                            drRow.Item("Synched") = "No"
                        Else
                            drRow.Item("Synched") = "Yes"
                        End If
                    Else
                        drRow.Item("Synched") = "Yes"
                    End If

                    If drRow.Item("SaleType") = 2 Then
                        drRow.Item("Return") = "This is a Return or a Negative Adjustment"
                    Else
                        drRow.Item("Return") = ""
                    End If
                Next

                dt.AcceptChanges()

            End If

            GetItemsOnHandheld = dt



        End Function


        Public Function GetAuditTrailForOrder(ByVal lngSalesOrderNum As Long) As DataTable

            Dim dt As New DataTable

            Dim strConnString As String = Session("_VT_DotNetConnString")
            Dim myConnection As New SqlConnection(strConnString)
            Dim comSQL As New SqlCommand("wfo_resGetAuditRecords", myConnection)
            Dim objParam As SqlParameter
            Dim myAdapter As New SqlDataAdapter(comSQL)


            comSQL.CommandType = Data.CommandType.StoredProcedure
            objParam = comSQL.Parameters.Add("@JobId", Data.SqlDbType.Int)
            objParam.Direction = Data.ParameterDirection.Input
            objParam.Value = lngSalesOrderNum

            comSQL.CommandType = Data.CommandType.StoredProcedure
            objParam = comSQL.Parameters.Add("@RecordType", Data.SqlDbType.NVarChar)
            objParam.Direction = Data.ParameterDirection.Input
            objParam.Value = "All"

            myConnection.Open()
            myAdapter.Fill(dt)
            myConnection.Close()
            myConnection = Nothing

            GetAuditTrailForOrder = dt



        End Function

        Public Function GetDeliverySummaryItems(ByVal strDocketNumber As String) As DataTable

            Dim dt As New DataTable
            Dim ds As New DataSet
            Dim objBPA As New VTDBFunctions.VTDBFunctions.TelesalesFunctions


            ds = objBPA.GetTransactionsForDocketNum(strDocketNumber)

            dt.Columns.Add("DocketNumber", System.Type.GetType("System.String"))
            dt.Columns.Add("TxnDate", System.Type.GetType("System.DateTime"))
            dt.Columns.Add("Driver", System.Type.GetType("System.String"))
            dt.Columns.Add("ItemsDelivered", System.Type.GetType("System.Int64"))
            dt.Columns.Add("SentToAccounts", System.Type.GetType("System.String"))
            dt.Columns.Add("Invoice", System.Type.GetType("System.String"))
            dt.Columns.Add("Comment", System.Type.GetType("System.String"))
            dt.Columns.Add("DeliveredPrice", System.Type.GetType("System.String"))
            dt.Columns.Add("OrderNum", System.Type.GetType("System.String"))
            dt.Columns.Add("DeliveryCustomer", System.Type.GetType("System.String"))



            Dim lngProductID As Long
            Dim intUnitOfSale As Integer
            Dim dblVat As Double
            Dim dblTotalCost As Double
            Dim objProducts As New VTDBFunctions.VTDBFunctions.ProductsFunctions
            Dim objCust As New VTDBFunctions.VTDBFunctions.CustomerFunctions


            For Each TxnRow As DataRow In ds.Tables(0).Rows
                lngProductID = TxnRow.Item("ProductId")
                intUnitOfSale = objProducts.GetUnitOfSale(lngProductID)
                If intUnitOfSale = 1 Then
                    dblVat = TxnRow.Item("Quantity") * TxnRow.Item("VATCharged")
                    dblTotalCost = dblTotalCost + dblVat + (TxnRow.Item("Quantity") * TxnRow.Item("PriceCharged"))
                Else
                    dblVat = TxnRow.Item("Weight") * TxnRow.Item("VATCharged")
                    dblTotalCost = dblTotalCost + dblVat + (TxnRow.Item("Weight") * TxnRow.Item("PriceCharged"))
                End If

            Next

            Dim newRow As DataRow = dt.NewRow()

            newRow("DocketNumber") = strDocketNumber
            newRow("TxnDate") = ds.Tables(0).Rows(0).Item("DateOfTransaction")
            newRow("Driver") = ds.Tables(0).Rows(0).Item("Driver")
            newRow("ItemsDelivered") = ds.Tables(0).Rows.Count

            If ds.Tables(0).Rows(0).Item("SentToSage") = 1 Then
                newRow("SentToAccounts") = "Yes"
            Else
                newRow("SentToAccounts") = "No"
            End If

            newRow("Invoice") = ds.Tables(0).Rows(0).Item("InvoiceNum")
            newRow("Comment") = IIf(IsDBNull(ds.Tables(0).Rows(0).Item("Comment")), "", ds.Tables(0).Rows(0).Item("Comment"))

            newRow("DeliveredPrice") = FormatCurrency(dblTotalCost, 2)
            newRow("OrderNum") = ds.Tables(0).Rows(0).Item("SalesOrderNum")

            newRow("DeliveryCustomer") = objCust.GetCustomerNameForId(ds.Tables(0).Rows(0).Item("DeliveryCustomerID"))

            dt.Rows.Add(newRow)


            GetDeliverySummaryItems = dt

        End Function


        Public Function GetDeliveriesForDateRange(ByVal dteStartDate As Date, ByVal dteEndDate As Date, ByVal strfilter As String) As DataSet

            Dim myConnection As New SqlConnection(Session("_VT_DotNetConnString"))
            Dim comSQL As New SqlCommand("trc_resDeliveriesForDateRange", myConnection)
            Dim adpSQL As New SqlDataAdapter(comSQL)
            Dim ds As New DataSet

            Dim objParam As SqlParameter

            comSQL.CommandType = CommandType.StoredProcedure
            objParam = comSQL.Parameters.Add("@StartDate", Data.SqlDbType.DateTime)
            objParam.Direction = ParameterDirection.Input
            objParam.Value = dteStartDate

            objParam = comSQL.Parameters.Add("@EndDate", Data.SqlDbType.DateTime)
            objParam.Direction = ParameterDirection.Input
            objParam.Value = dteEndDate

            myConnection.Open()
            adpSQL.Fill(ds)

            GetDeliveriesForDateRange = ds


            '  Close the connection when done with it.
            myConnection.Close()


        End Function


        Public Sub PricedDocketPrint(ByVal strDocketNum As String)
            Dim dsTransactions As New DataSet
            Dim strProductCode As String
            Dim strProductDesc As String
            Dim strTraceCode As String
            Dim dblRowTotal As Double
            Dim dblRowVAT As Double
            Dim dblTotal As Double
            Dim dblTotalVAT As Double
            Dim dsProduct As New DataSet
            Dim dsTraceCode As New DataSet
            Dim dblQuantity As Double
            Dim dblWeight As Double
            Dim dtDeliveryDate As Date
            Dim dblPriceCharged As Double
            Dim intOrderNum As Integer
            Dim objProds As New VTDBFunctions.VTDBFunctions.ProductsFunctions
            Dim objTrace As New VTDBFunctions.VTDBFunctions.TraceDataFunctions
            Dim objTelesales As New VTDBFunctions.VTDBFunctions.TelesalesFunctions
            Dim intSellByType As Integer
            Dim dblProdAvgWeight As Double
            Dim dblAvgWeight As Double


            objTelesales.ClearDocketPrintTable()

            'Get all the transactions for the docket number
            dsTransactions = objTelesales.GetTransactionsForDocketNum(strDocketNum)

            dblTotal = 0
            dblTotalVAT = 0

            Dim i As Integer

            'for each transaction in the dataset 
            For i = 0 To dsTransactions.Tables(0).Rows.Count - 1

                'get the Product code
                'get the product Description
                dsProduct = objProds.GetProductForId(dsTransactions.Tables(0).Rows(i).Item("ProductID"))
                strProductCode = dsProduct.Tables(0).Rows(0).Item("Catalog_Number")
                strProductDesc = dsProduct.Tables(0).Rows(0).Item("Product_Name")

                'get the trace code
                dsTraceCode = objTrace.GetBatchDetailsForId(dsTransactions.Tables(0).Rows(i).Item("TraceCodeId"))
                strTraceCode = dsTraceCode.Tables(0).Rows(0).Item("TraceCodeDesc")
                'get the sell by type


                'Add the calculated weight from avg unit weight
                If IsDBNull(dsProduct.Tables(0).Rows(0).Item("AvgWeightPerUnit")) = False Then
                    dblProdAvgWeight = dsProduct.Tables(0).Rows(0).Item("AvgWeightPerUnit")
                Else
                    dblProdAvgWeight = 0
                End If



                intSellByType = objProds.GetUnitOfSale(dsTransactions.Tables(0).Rows(i).Item("ProductID"))
                'calculate the total price 
                'calcualte the total vat
                If intSellByType = 1 Then
                    dblRowTotal = CDbl(dsTransactions.Tables(0).Rows(i).Item("Quantity")) * CDbl(dsTransactions.Tables(0).Rows(i).Item("PriceCharged"))
                    dblRowVAT = CDbl(dsTransactions.Tables(0).Rows(i).Item("Quantity")) * CDbl(dsTransactions.Tables(0).Rows(i).Item("VATCharged"))
                    dblAvgWeight = CDbl(dsTransactions.Tables(0).Rows(i).Item("Quantity")) * dblProdAvgWeight
                Else
                    dblRowTotal = CDbl(dsTransactions.Tables(0).Rows(i).Item("Weight")) * CDbl(dsTransactions.Tables(0).Rows(i).Item("PriceCharged"))
                    dblRowVAT = CDbl(dsTransactions.Tables(0).Rows(i).Item("Weight")) * CDbl(dsTransactions.Tables(0).Rows(i).Item("VATCharged"))
                    dblAvgWeight = CDbl(dsTransactions.Tables(0).Rows(i).Item("Weight"))
                End If

                dblRowTotal = FormatNumber(dblRowTotal, 2)
                dblRowVAT = FormatNumber(dblRowVAT, 2)

                'Add to overall total 
                dblTotal = dblTotal + dblRowTotal
                'add to overall VAT
                dblTotalVAT = dblTotalVAT + dblRowVAT
                'write row record to trc_DocketReport table

                dtDeliveryDate = dsTransactions.Tables(0).Rows(i).Item("DateOfTransaction")


                dblWeight = dsTransactions.Tables(0).Rows(i).Item("Weight")

                dblQuantity = dsTransactions.Tables(0).Rows(i).Item("Quantity")

                dblPriceCharged = FormatNumber(dsTransactions.Tables(0).Rows(i).Item("PriceCharged"), 2)




                intOrderNum = dsTransactions.Tables(0).Rows(i).Item("SalesOrderNum")

                objTelesales.InsertDocketPrintRecord(strProductCode, strProductDesc, strTraceCode, dtDeliveryDate, dblWeight, dblQuantity, dblPriceCharged, 0, dblRowTotal, strDocketNum, dblRowVAT, 0, dblAvgWeight, intOrderNum)




            Next i

            'write totals row to trc_DocketReport table

            objTelesales.InsertDocketPrintRecord("", "", "", dtDeliveryDate, 0, 0, 0, 1, (dblTotal + dblTotalVAT), strDocketNum, 0, dblTotalVAT, dblTotal, intOrderNum)


        End Sub

        Public Sub PricedInvoicePrint(ByVal strInvoiceNum As String)
            Dim dsTransactions As New DataSet
            Dim strProductCode As String
            Dim strProductDesc As String
            Dim strTraceCode As String
            Dim dblRowTotal As Double
            Dim dblRowVAT As Double
            Dim dblTotal As Double
            Dim dblTotalVAT As Double
            Dim dsProduct As New DataSet
            Dim dsTraceCode As New DataSet
            Dim dblQuantity As Double
            Dim dblWeight As Double
            Dim dtDeliveryDate As Date
            Dim dblPriceCharged As Double
            Dim intOrderNum As Integer
            Dim objProds As New VTDBFunctions.VTDBFunctions.ProductsFunctions
            Dim objTrace As New VTDBFunctions.VTDBFunctions.TraceDataFunctions
            Dim objTelesales As New VTDBFunctions.VTDBFunctions.TelesalesFunctions
            Dim intSellByType As Integer
            Dim strDocketNum As String
            Dim intInvoiceNum As Integer
            Dim intPosn As Integer

            objTelesales.ClearDocketPrintTable()

            intPosn = InStr(strInvoiceNum, "~")
            If intPosn > 0 Then
                strInvoiceNum = Left(strInvoiceNum, intPosn - 1)
            End If
            intInvoiceNum = CInt(strInvoiceNum)

            '   strDocketNum = Left(strDocketNum, Len(strDocketNum) - 1)
            'Get all the transactions for the docket number
            dsTransactions = objTelesales.GetTransactionsForInvoiceNum(intInvoiceNum)

            dblTotal = 0
            dblTotalVAT = 0

            Dim i As Integer

            'for each transaction in the dataset 
            For i = 0 To dsTransactions.Tables(0).Rows.Count - 1

                'get the Product code
                'get the product Description
                dsProduct = objProds.GetProductForId(dsTransactions.Tables(0).Rows(i).Item("ProductID"))
                strProductCode = dsProduct.Tables(0).Rows(0).Item("Catalog_Number")
                strProductDesc = dsProduct.Tables(0).Rows(0).Item("Product_Name")

                'get the trace code
                dsTraceCode = objTrace.GetBatchDetailsForId(dsTransactions.Tables(0).Rows(i).Item("TraceCodeId"))
                strTraceCode = dsTraceCode.Tables(0).Rows(0).Item("TraceCodeDesc")
                'get the sell by type

                intSellByType = objProds.GetUnitOfSale(dsTransactions.Tables(0).Rows(i).Item("ProductID"))
                'calculate the total price 
                'calcualte the total vat
                If intSellByType = 1 Then
                    dblRowTotal = CDbl(dsTransactions.Tables(0).Rows(i).Item("Quantity")) * CDbl(dsTransactions.Tables(0).Rows(i).Item("PriceCharged"))
                    dblRowVAT = CDbl(dsTransactions.Tables(0).Rows(i).Item("Quantity")) * CDbl(dsTransactions.Tables(0).Rows(i).Item("VATCharged"))
                Else
                    dblRowTotal = CDbl(dsTransactions.Tables(0).Rows(i).Item("Weight")) * CDbl(dsTransactions.Tables(0).Rows(i).Item("PriceCharged"))
                    dblRowVAT = CDbl(dsTransactions.Tables(0).Rows(i).Item("Weight")) * CDbl(dsTransactions.Tables(0).Rows(i).Item("VATCharged"))
                End If

                dblRowTotal = FormatNumber(dblRowTotal, 2)
                dblRowVAT = FormatNumber(dblRowVAT, 2)

                'Add to overall total 
                dblTotal = dblTotal + dblRowTotal
                'add to overall VAT
                dblTotalVAT = dblTotalVAT + dblRowVAT
                'write row record to trc_DocketReport table

                dtDeliveryDate = dsTransactions.Tables(0).Rows(i).Item("DateOfTransaction")


                dblWeight = dsTransactions.Tables(0).Rows(i).Item("Weight")

                dblQuantity = dsTransactions.Tables(0).Rows(i).Item("Quantity")

                dblPriceCharged = FormatNumber(dsTransactions.Tables(0).Rows(i).Item("PriceCharged"), 2)

                strDocketNum = dsTransactions.Tables(0).Rows(i).Item("DocketNum")

                intOrderNum = dsTransactions.Tables(0).Rows(i).Item("SalesOrderNum")

                objTelesales.InsertDocketPrintRecord(strProductCode, strProductDesc, strTraceCode, dtDeliveryDate, dblWeight, dblQuantity, dblPriceCharged, 0, dblRowTotal, CStr(intInvoiceNum), dblRowVAT, 0, 0, intOrderNum)
            Next i

            'write totals row to trc_DocketReport table

            objTelesales.InsertDocketPrintRecord("", "", "", dtDeliveryDate, 0, 0, 0, 1, (dblTotal + dblTotalVAT), CStr(intInvoiceNum), 0, dblTotalVAT, dblTotal, intOrderNum)


        End Sub


        Public Sub ExtraDataDocketPrint(ByVal strDocketNum As String)
            Dim dsTransactions As New DataSet
            Dim strProductCode As String
            Dim strProductDesc As String
            Dim strTraceCode As String
            Dim dblRowTotal As Double
            Dim dblRowVAT As Double
            Dim dblTotal As Double
            Dim dblTotalVAT As Double
            Dim dsProduct As New DataSet
            Dim dsTraceCode As New DataSet
            Dim dblQuantity As Double
            Dim dblWeight As Double
            Dim dtDeliveryDate As Date
            Dim dblPriceCharged As Double
            Dim intOrderNum As Integer
            Dim objProds As New VTDBFunctions.VTDBFunctions.ProductsFunctions
            Dim objTrace As New VTDBFunctions.VTDBFunctions.TraceDataFunctions
            Dim objTelesales As New VTDBFunctions.VTDBFunctions.TelesalesFunctions
            Dim objCust As New VTDBFunctions.VTDBFunctions.CustomerFunctions
            Dim intSellByType As Integer
            Dim strExtra1 As String
            Dim strExtra2 As String
            Dim strExtra3 As String
            Dim strExtra4 As String
            Dim strExtra5 As String
            Dim strExtra6 As String
            Dim strExtra7 As String
            Dim strExtra8 As String
            Dim strExtra9 As String
            Dim strExtra10 As String
            Dim dsExtraData As New DataSet
            Dim dsCust As New DataSet
            Dim strSerailNumber As String

            objTelesales.ClearDocketPrintTable(Session("_VT_CurrentUserId"))


            'Get all the transactions for the docket number
            dsTransactions = objTelesales.GetTransactionsForDocketNum(strDocketNum)


            dsCust = objCust.GetCustomerDetailsForId(dsTransactions.Tables(0).Rows(0).Item("DeliveryCustomerId"))


            dblTotal = 0
            dblTotalVAT = 0

            Dim i As Integer

            'for each transaction in the dataset 
            For i = 0 To dsTransactions.Tables(0).Rows.Count - 1

                'get the Product code
                'get the product Description
                dsProduct = objProds.GetProductForId(dsTransactions.Tables(0).Rows(i).Item("ProductID"))
                strProductCode = dsProduct.Tables(0).Rows(0).Item("Catalog_Number")
                strProductDesc = dsProduct.Tables(0).Rows(0).Item("Product_Name")

                strExtra1 = ""
                strExtra2 = ""
                strExtra3 = ""
                strExtra4 = ""
                strExtra5 = ""
                strExtra6 = ""
                strExtra7 = ""
                strExtra8 = ""
                strExtra9 = ""
                strExtra10 = ""

                Dim objCommonFuncs As New BPADotNetCommonFunctions.VT_CommonFunctions.CommonFunctions
                Dim strcompany As String = objCommonFuncs.GetConfigItem("Company")

                Select Case UCase(strcompany)
                    Case "UNIBOARD"

                        dsExtraData = objProds.GetProductAdditionalData(dsTransactions.Tables(0).Rows(i).Item("ProductID"))

                        If dsExtraData.Tables(0).Rows.Count > 0 Then
                            For Each drRow As DataRow In dsExtraData.Tables(0).Rows
                                Select Case UCase(drRow.Item("DataItemName"))
                                    Case "DETAIL"
                                        strExtra1 = IIf(IsDBNull(drRow.Item("DataItemValue")), "", drRow.Item("DataItemValue"))
                                    Case "GMPERM2"
                                        strExtra2 = IIf(IsDBNull(drRow.Item("DataItemValue")), "", drRow.Item("DataItemValue"))
                                    Case "THICKNESS"
                                        strExtra3 = IIf(IsDBNull(drRow.Item("DataItemValue")), "", drRow.Item("DataItemValue"))
                                    Case "AREA"
                                        strExtra4 = IIf(IsDBNull(drRow.Item("DataItemValue")), "", drRow.Item("DataItemValue"))
                                    Case "TYPE"
                                        strExtra5 = IIf(IsDBNull(drRow.Item("DataItemValue")), "", drRow.Item("DataItemValue"))
                                    Case "WIDTH"
                                        strExtra6 = IIf(IsDBNull(drRow.Item("DataItemValue")), "", drRow.Item("DataItemValue"))
                                    Case "LENGTH"
                                        strExtra7 = IIf(IsDBNull(drRow.Item("DataItemValue")), "", drRow.Item("DataItemValue"))

                                End Select
                            Next
                        End If
                        strExtra9 = IIf(IsDBNull(dsTransactions.Tables(0).Rows(i).Item("SerialNum")), "", dsTransactions.Tables(0).Rows(i).Item("SerialNum"))
                        strExtra10 = IIf(IsDBNull(dsTransactions.Tables(0).Rows(i).Item("Barcode")), "", dsTransactions.Tables(0).Rows(i).Item("Barcode"))

                        strExtra9 = Trim(strExtra9)
                        strExtra10 = Trim(strExtra10)

                    Case Else
                End Select


                'get the trace code
                dsTraceCode = objTrace.GetBatchDetailsForId(dsTransactions.Tables(0).Rows(i).Item("TraceCodeId"))
                strTraceCode = dsTraceCode.Tables(0).Rows(0).Item("TraceCodeDesc")

                'get the sell by type
                intSellByType = objProds.GetUnitOfSale(dsTransactions.Tables(0).Rows(i).Item("ProductID"))
                'calculate the total price 
                'calcualte the total vat
                If intSellByType = 1 Then
                    dblRowTotal = CDbl(dsTransactions.Tables(0).Rows(i).Item("Quantity")) * CDbl(dsTransactions.Tables(0).Rows(i).Item("PriceCharged"))
                    dblRowVAT = CDbl(dsTransactions.Tables(0).Rows(i).Item("Quantity")) * CDbl(dsTransactions.Tables(0).Rows(i).Item("VATCharged"))
                Else
                    dblRowTotal = CDbl(dsTransactions.Tables(0).Rows(i).Item("Weight")) * CDbl(dsTransactions.Tables(0).Rows(i).Item("PriceCharged"))
                    dblRowVAT = CDbl(dsTransactions.Tables(0).Rows(i).Item("Weight")) * CDbl(dsTransactions.Tables(0).Rows(i).Item("VATCharged"))
                End If

                dblRowTotal = FormatNumber(dblRowTotal, 2)
                dblRowVAT = FormatNumber(dblRowVAT, 2)

                'Add to overall total 
                dblTotal = dblTotal + dblRowTotal
                'add to overall VAT
                dblTotalVAT = dblTotalVAT + dblRowVAT
                'write row record to trc_DocketReport table

                dtDeliveryDate = dsTransactions.Tables(0).Rows(i).Item("DateOfTransaction")


                strExtra8 = IIf(IsDBNull(dsCust.Tables(0).Rows(0).Item("Comment")), "", dsCust.Tables(0).Rows(0).Item("Comment"))
                strExtra8 = Trim(strExtra8)

                dblWeight = dsTransactions.Tables(0).Rows(i).Item("Weight")

                dblQuantity = dsTransactions.Tables(0).Rows(i).Item("Quantity")

                dblPriceCharged = FormatNumber(dsTransactions.Tables(0).Rows(i).Item("PriceCharged"), 2)



                intOrderNum = dsTransactions.Tables(0).Rows(i).Item("SalesOrderNum")

                objTelesales.InsertDocketPrintRecordExtended(strProductCode, strProductDesc, strTraceCode, dtDeliveryDate, dblWeight, dblQuantity, dblPriceCharged, 0, dblRowTotal, strDocketNum, dblRowVAT, 0, 0, intOrderNum, Session("_VT_CurrentUserId"), Me.CurrentSession.VT_SalesOrderID, _
                                                             strExtra1, strExtra2, strExtra3, strExtra4, strExtra5, strExtra6, strExtra7, strExtra8, strExtra9, strExtra10)
            Next i

            'write totals row to trc_DocketReport table

            strExtra8 = IIf(IsDBNull(dsCust.Tables(0).Rows(0).Item("Comment")), "", dsCust.Tables(0).Rows(0).Item("Comment"))
            strExtra8 = Trim(strExtra8)

            objTelesales.InsertDocketPrintRecordExtended("", "", "", dtDeliveryDate, 0, 0, 0, 1, (dblTotal + dblTotalVAT), strDocketNum, 0, dblTotalVAT, dblTotal, intOrderNum, Session("_VT_CurrentUserId"), Me.CurrentSession.VT_SalesOrderID, , , , , , , , strExtra8, , )


        End Sub


        Public Sub ExtraDataSOPrint(ByVal lngSalesOrderID As Long)
            Dim dsOrderItems As New DataSet
            Dim strProductCode As String
            Dim strProductDesc As String
            Dim strTraceCode As String
            Dim dblRowTotal As Double
            Dim dblRowVAT As Double
            Dim dblTotal As Double
            Dim dblTotalVAT As Double
            Dim dsProduct As New DataSet
            Dim dsTraceCode As New DataSet
            Dim dblQuantity As Double
            Dim dblWeight As Double
            Dim dtDeliveryDate As Date
            Dim dblPriceCharged As Double
            Dim intOrderNum As Integer
            Dim objProds As New VTDBFunctions.VTDBFunctions.ProductsFunctions
            Dim objTrace As New VTDBFunctions.VTDBFunctions.TraceDataFunctions
            Dim objTelesales As New VTDBFunctions.VTDBFunctions.TelesalesFunctions
            Dim objCust As New VTDBFunctions.VTDBFunctions.CustomerFunctions
            Dim intSellByType As Integer
            Dim strExtra1 As String
            Dim strExtra2 As String
            Dim strExtra3 As String
            Dim strExtra4 As String
            Dim strExtra5 As String
            Dim strExtra6 As String
            Dim strExtra7 As String
            Dim strExtra8 As String
            Dim strExtra9 As String
            Dim strExtra10 As String
            Dim dsExtraData As New DataSet
            Dim dsCust As New DataSet
            Dim dsSalesOrder As New DataSet
            Dim dblQtySentForPicking As Double
            Dim dblWgtSentForPicking As Double
            Dim dsHHSalesOrderItem As New DataSet

            objTelesales.ClearDocketPrintTable(Session("_VT_CurrentUserId"))

            'Get all the transactions for the docket number
            dsOrderItems = objTelesales.GetOrderItems(lngSalesOrderID)

            dblTotal = 0
            dblTotalVAT = 0

            dsSalesOrder = objTelesales.GetOrderForId(lngSalesOrderID)
            dsCust = objCust.GetCustomerDetailsForId(dsSalesOrder.Tables(0).Rows(0).Item("DeliveryCustomerId"))


            Dim i As Integer

            'for each transaction in the dataset 
            For i = 0 To dsOrderItems.Tables(0).Rows.Count - 1

                'get the Product code
                'get the product Description
                dsProduct = objProds.GetProductForId(dsOrderItems.Tables(0).Rows(i).Item("ProductID"))
                strProductCode = dsProduct.Tables(0).Rows(0).Item("Catalog_Number")
                strProductDesc = dsProduct.Tables(0).Rows(0).Item("Product_Name")


                strExtra1 = ""
                strExtra2 = ""
                strExtra3 = ""
                strExtra4 = ""
                strExtra5 = ""
                strExtra6 = ""
                strExtra7 = ""
                strExtra8 = ""
                strExtra9 = ""
                strExtra10 = ""

                Dim objCommonFuncs As New BPADotNetCommonFunctions.VT_CommonFunctions.CommonFunctions
                Dim strcompany As String = objCommonFuncs.GetConfigItem("Company")

                Select Case UCase(strcompany)
                    Case "UNIBOARD"

                        dsExtraData = objProds.GetProductAdditionalData(dsOrderItems.Tables(0).Rows(i).Item("ProductID"))

                        If dsExtraData.Tables(0).Rows.Count > 0 Then
                            For Each drRow As DataRow In dsExtraData.Tables(0).Rows
                                Select Case UCase(drRow.Item("DataItemName"))
                                    Case "DETAIL"
                                        strExtra1 = IIf(IsDBNull(drRow.Item("DataItemValue")), "", drRow.Item("DataItemValue"))
                                    Case "GMPERM2"
                                        strExtra2 = IIf(IsDBNull(drRow.Item("DataItemValue")), "", drRow.Item("DataItemValue"))
                                    Case "THICKNESS"
                                        strExtra3 = IIf(IsDBNull(drRow.Item("DataItemValue")), "", drRow.Item("DataItemValue"))
                                    Case "AREA"
                                        strExtra4 = IIf(IsDBNull(drRow.Item("DataItemValue")), "", drRow.Item("DataItemValue"))
                                    Case "TYPE"
                                        strExtra5 = IIf(IsDBNull(drRow.Item("DataItemValue")), "", drRow.Item("DataItemValue"))

                                End Select
                            Next
                        End If

                    Case Else
                End Select



                'get the trace code
                '  dsTraceCode = objTrace.GetBatchDetailsForId(dsTransactions.Tables(0).Rows(i).Item("TraceCodeId"))
                strTraceCode = "" ' dsTraceCode.Tables(0).Rows(0).Item("TraceCodeDesc")
                'get the sell by type

                intSellByType = objProds.GetUnitOfSale(dsOrderItems.Tables(0).Rows(i).Item("ProductID"))
                'calculate the total price 
                'calcualte the total vat
                If intSellByType = 1 Then
                    dblRowTotal = CDbl(dsOrderItems.Tables(0).Rows(i).Item("QuantityRequested")) * CDbl(dsOrderItems.Tables(0).Rows(i).Item("UnitPrice"))
                    dblRowVAT = CDbl(dsOrderItems.Tables(0).Rows(i).Item("VAT"))
                Else
                    dblRowTotal = CDbl(dsOrderItems.Tables(0).Rows(i).Item("WeightRequested")) * CDbl(dsOrderItems.Tables(0).Rows(i).Item("UnitPrice"))
                    dblRowVAT = CDbl(dsOrderItems.Tables(0).Rows(i).Item("VAT"))
                End If

                dblRowTotal = FormatNumber(dblRowTotal, 2)
                dblRowVAT = FormatNumber(dblRowVAT, 2)

                'Add to overall total 
                dblTotal = dblTotal + dblRowTotal
                'add to overall VAT
                dblTotalVAT = dblTotalVAT + dblRowVAT
                'write row record to trc_DocketReport table

                dtDeliveryDate = PortalFunctions.Now.Date


                strExtra8 = IIf(IsDBNull(dsCust.Tables(0).Rows(0).Item("Comment")), "", dsCust.Tables(0).Rows(0).Item("Comment"))
                strExtra8 = Trim(strExtra8)

                dblWeight = dsOrderItems.Tables(0).Rows(i).Item("WeightRequested")

                dblQuantity = dsOrderItems.Tables(0).Rows(i).Item("QuantityRequested")

                '    dblPriceCharged = FormatNumber(dsOrderItems.Tables(0).Rows(i).Item("TotalPrice"), 2)

                dblPriceCharged = FormatNumber(dsOrderItems.Tables(0).Rows(i).Item("UnitPrice"), 2)


                dblQtySentForPicking = 0
                dblWgtSentForPicking = 0

                dsHHSalesOrderItem = objTrace.GetTDSalesOrderItem(dsOrderItems.Tables(0).Rows(i).Item("SalesOrderItemID"))
                For Each drRow As DataRow In dsHHSalesOrderItem.Tables(0).Rows
                    dblQtySentForPicking = dblQtySentForPicking + drRow.Item("OrderedQty")
                    dblWgtSentForPicking = dblWgtSentForPicking + drRow.Item("OrderedWgt")

                Next

                strExtra6 = CStr(dblQtySentForPicking)
                strExtra7 = CStr(dblWgtSentForPicking)

                intOrderNum = Me.CurrentSession.VT_SalesOrderNum

                objTelesales.InsertDocketPrintRecordExtended(strProductCode, strProductDesc, strTraceCode, dtDeliveryDate, dblWeight, dblQuantity, dblPriceCharged, 0, dblRowTotal, "", dblRowVAT, 0, 0, intOrderNum, Session("_VT_CurrentUserId"), lngSalesOrderID, _
                                                             strExtra1, strExtra2, strExtra3, strExtra4, strExtra5, strExtra6, strExtra7, strExtra8, strExtra9, strExtra10)
            Next i

            'write totals row to trc_DocketReport table
            strExtra8 = IIf(IsDBNull(dsCust.Tables(0).Rows(0).Item("Comment")), "", dsCust.Tables(0).Rows(0).Item("Comment"))
            strExtra8 = Trim(strExtra8)

            strExtra9 = CStr(Me.CurrentSession.VT_OrderPriority)


            objTelesales.InsertDocketPrintRecordExtended("", "", "", dtDeliveryDate, 0, 0, 0, 1, (dblTotal + dblTotalVAT), "", 0, dblTotalVAT, dblTotal, intOrderNum, Session("_VT_CurrentUserId"), lngSalesOrderID, , , , , , , , strExtra8, strExtra9, )


        End Sub


        Sub UpdateTransactionDocketNumber(ByVal strDocketNumNew As String, ByVal strDocketNumOld As String, ByVal strOrderNum As String)

            Dim myConnection As New SqlConnection(Session("_VT_DotNetConnString"))
            Dim comSQL As New SqlCommand("trc_updTransactionDocketNum", myConnection)
            Dim adpSQL As New SqlDataAdapter(comSQL)
            Dim dsadded As New DataSet

            Dim objParam As SqlParameter


            comSQL.CommandType = CommandType.StoredProcedure
            objParam = comSQL.Parameters.Add("@DocketNumNew", SqlDbType.NChar)
            objParam.Direction = ParameterDirection.Input
            objParam.Value = strDocketNumNew

            objParam = comSQL.Parameters.Add("@DocketNum", SqlDbType.NChar)
            objParam.Direction = ParameterDirection.Input
            objParam.Value = strDocketNumOld

            objParam = comSQL.Parameters.Add("@OrderNum", SqlDbType.NChar)
            objParam.Direction = ParameterDirection.Input
            objParam.Value = strOrderNum


            myConnection.Open()
            adpSQL.Fill(dsadded)

            myConnection.Close()

        End Sub


        Public Sub UpdateOrderTotal(ByVal lngOrderId As Long, ByVal lngOrderNum As Long)

            Dim ObjTele As New VTDBFunctions.VTDBFunctions.TelesalesFunctions
            Dim dsOrderItems As New DataSet
            Dim dblNewTotal As Double
            Dim objCommon As New TTWFOCommonFunctions.TOFunctions

            'Check that product is not already in order.
            dsOrderItems = ObjTele.GetOrderItems(Me.CurrentSession.VT_SalesOrderID)

            dblNewTotal = 0
            For Each drRow As DataRow In dsOrderItems.Tables(0).Rows
                dblNewTotal = dblNewTotal + drRow.Item("TotalPrice")

            Next

            ObjTele.SaveTotalValueForSalesOrder(lngOrderId, dblNewTotal)

            'objCommon.SetExtraDataItem(lngOrderNum, "ExtraData3", CStr(dblNewTotal))



            Dim strConnString As String = Session("_VT_DotNetConnString")
            Dim myConnection As New SqlConnection(strConnString)

            Dim strsql As String

            strsql = "Update wfo_BatchTable SET ExtraData3  = '" + Replace(CStr(dblNewTotal), "'", "''") + "' WHERE JobId = " + CStr(lngOrderNum)



            Dim comSQL As New SqlCommand(strsql, myConnection)
            Dim myAdapter As New SqlDataAdapter(comSQL)

            myConnection.Open()
            comSQL.ExecuteNonQuery()

            '  Close the connection when done with it.
            myConnection.Close()
            myConnection = Nothing


        End Sub

        Public Sub UniboardReportForCustomer(ByVal strCustomerCode As String)
            Dim dsOrderItems As New DataSet
            Dim dsOrders As New DataSet
            Dim strProductCode As String
            Dim strProductDesc As String
            Dim dblRowTotal As Double
            Dim strPONumber As String
            Dim dsProduct As New DataSet
            Dim dblOrderedQuantity As Double
            Dim dblOrderedWeight As Double
            Dim dblOustandingQuantity As Double
            Dim dblOutstandingWeight As Double
            Dim dtOrderDate As Date
            Dim dblPriceCharged As Double
            Dim intOrderNum As Integer
            Dim dblTotalSum As Double
            Dim objProds As New VTDBFunctions.VTDBFunctions.ProductsFunctions
            Dim objTrace As New VTDBFunctions.VTDBFunctions.TraceDataFunctions
            Dim objTelesales As New VTDBFunctions.VTDBFunctions.TelesalesFunctions
            Dim objCust As New VTDBFunctions.VTDBFunctions.CustomerFunctions
            Dim intSellByType As Integer
            Dim strExtra1 As String
            Dim strExtra2 As String
            Dim strExtra3 As String
            Dim strExtra4 As String
            Dim strExtra5 As String
            Dim strExtra6 As String
            Dim strExtra7 As String
            Dim strExtra8 As String
            Dim strExtra9 As String
            Dim strExtra10 As String
            Dim dsExtraData As New DataSet
            Dim dsCust As New DataSet
            Dim lngCustomerID As Long
            Dim i As Integer
            Dim j As Integer
            Dim intPosn As Integer

            objTelesales.ClearDocketPrintTable(Session("_VT_CurrentUserId"))
            dblTotalSum = 0

            intPosn = InStr(strCustomerCode, "~")
            If intPosn > 0 Then
                strCustomerCode = Left(strCustomerCode, intPosn - 1)
            End If

            'Get all open orders for the customer
            If strCustomerCode <> "" And UCase(strCustomerCode) <> "ALL" Then
                lngCustomerID = objCust.GetCustomerIdForRef(strCustomerCode)
            Else
                lngCustomerID = 0
            End If
            dsOrders = GetOpenOrdersForCustomer(lngCustomerID)
            Try
                For j = 0 To dsOrders.Tables(0).Rows.Count - 1
                    'Get open items for each order
                    dsOrderItems = objTelesales.GetOrderItems(dsOrders.Tables(0).Rows(j).Item("SalesOrderId"))


                    'for each transaction in the dataset 
                    For i = 0 To dsOrderItems.Tables(0).Rows.Count - 1
                        If Trim(dsOrderItems.Tables(0).Rows(i).Item("Status")) <> "Complete" And Left(dsOrderItems.Tables(0).Rows(i).Item("Status"), 6) <> "Closed" Then
                            'get the Product code
                            'get the product Description
                            dsProduct = objProds.GetProductForId(dsOrderItems.Tables(0).Rows(i).Item("ProductID"))
                            strProductCode = dsProduct.Tables(0).Rows(0).Item("Catalog_Number")
                            strProductDesc = dsProduct.Tables(0).Rows(0).Item("Product_Name")

                            strExtra1 = ""
                            strExtra2 = ""
                            strExtra3 = ""
                            strExtra4 = ""
                            strExtra5 = ""
                            strExtra6 = ""
                            strExtra7 = ""
                            strExtra8 = ""
                            strExtra9 = ""
                            strExtra10 = ""

                            Dim objCommonFuncs As New BPADotNetCommonFunctions.VT_CommonFunctions.CommonFunctions
                            Dim strcompany As String = objCommonFuncs.GetConfigItem("Company")

                            Select Case UCase(strcompany)
                                Case "UNIBOARD"

                                    dsExtraData = objProds.GetProductAdditionalData(dsOrderItems.Tables(0).Rows(i).Item("ProductID"))

                                    If dsExtraData.Tables(0).Rows.Count > 0 Then
                                        For Each drRow As DataRow In dsExtraData.Tables(0).Rows
                                            Select Case UCase(drRow.Item("DataItemName"))
                                                Case "DETAIL"
                                                    strExtra1 = IIf(IsDBNull(drRow.Item("DataItemValue")), "", drRow.Item("DataItemValue"))
                                                Case "GMPERM2"
                                                    strExtra2 = IIf(IsDBNull(drRow.Item("DataItemValue")), "", drRow.Item("DataItemValue"))
                                                Case "THICKNESS"
                                                    strExtra3 = IIf(IsDBNull(drRow.Item("DataItemValue")), "", drRow.Item("DataItemValue"))
                                                Case "AREA"
                                                    strExtra4 = IIf(IsDBNull(drRow.Item("DataItemValue")), "", drRow.Item("DataItemValue"))
                                                Case "TYPE"
                                                    strExtra5 = IIf(IsDBNull(drRow.Item("DataItemValue")), "", drRow.Item("DataItemValue"))
                                                Case "WIDTH"
                                                    strExtra6 = IIf(IsDBNull(drRow.Item("DataItemValue")), "", drRow.Item("DataItemValue"))
                                                Case "LENGTH"
                                                    strExtra7 = IIf(IsDBNull(drRow.Item("DataItemValue")), "", drRow.Item("DataItemValue"))

                                            End Select
                                        Next
                                    End If


                                Case Else
                            End Select


                            dtOrderDate = Format(dsOrders.Tables(0).Rows(j).Item("DateCreated"), "short date")

                            intOrderNum = dsOrders.Tables(0).Rows(j).Item("SalesOrderNum")

                            dblOrderedQuantity = dsOrderItems.Tables(0).Rows(i).Item("QuantityRequested")
                            dblOrderedWeight = dsOrderItems.Tables(0).Rows(i).Item("WeightRequested")
                            dblOustandingQuantity = dblOrderedQuantity - dsOrderItems.Tables(0).Rows(i).Item("Quantity")
                            dblOutstandingWeight = dblOrderedWeight - dsOrderItems.Tables(0).Rows(i).Item("Weight")

                            dblPriceCharged = dsOrderItems.Tables(0).Rows(i).Item("UnitPrice")
                            dblPriceCharged = FormatNumber(dblPriceCharged, 2)

                            'get the sell by type
                            intSellByType = dsProduct.Tables(0).Rows(0).Item("UnitOfSale")
                            'calculate the total price 
                            'calcualte the total vat
                            If intSellByType = 1 Then
                                dblRowTotal = dblOustandingQuantity * CDbl(dblPriceCharged)
                            Else
                                dblRowTotal = dblOutstandingWeight * CDbl(dblPriceCharged)
                            End If
                            dblRowTotal = FormatNumber(dblRowTotal, 2)
                            dblTotalSum = dblTotalSum + dblRowTotal

                            If IsDBNull(dsProduct.Tables(0).Rows(0).Item("InStock")) = False Then
                                strExtra8 = CStr(dsProduct.Tables(0).Rows(0).Item("InStock"))
                            End If
                            strExtra9 = CStr(CInt(dblOustandingQuantity))
                            strExtra10 = CStr(FormatNumber(dblOutstandingWeight, 3))

                            strPONumber = Trim(dsOrders.Tables(0).Rows(j).Item("CustomerPO"))

                            objTelesales.InsertDocketPrintRecordExtended(strProductCode, strProductDesc, strCustomerCode, dtOrderDate, dblOrderedWeight, dblOrderedQuantity, dblPriceCharged, 0, dblRowTotal, strPONumber, 0, 0, 0, intOrderNum, Session("_VT_CurrentUserId"), Me.CurrentSession.VT_SalesOrderID, _
                                                                         strExtra1, strExtra2, strExtra3, strExtra4, strExtra5, strExtra6, strExtra7, strExtra8, strExtra9, strExtra10)
                        End If
                    Next i
                Next j


                'write totals row to trc_DocketReport table

                objTelesales.InsertDocketPrintRecordExtended("", "", strCustomerCode, dtOrderDate, 0, 0, 0, 1, 0, "", 0, 0, dblTotalSum, 0, Session("_VT_CurrentUserId"))


            Catch ex As Exception
                MsgBox(ex.Message)
            End Try

        End Sub





        Public Function GetOpenOrdersForCustomer(ByVal lngCustomerID As Long) As DataSet

            Dim myConnection As New SqlConnection(Session("_VT_DotNetConnString"))
            Dim comSQL As New SqlCommand("tls_resOpenOrdersforCustomer", myConnection)
            Dim adpSQL As New SqlDataAdapter(comSQL)
            Dim ds As New DataSet

            Dim objParam As SqlParameter

            comSQL.CommandType = CommandType.StoredProcedure
            objParam = comSQL.Parameters.Add("@CustomerId", Data.SqlDbType.Int)
            objParam.Direction = ParameterDirection.Input
            objParam.Value = lngCustomerID



            myConnection.Open()
            adpSQL.Fill(ds)

            GetOpenOrdersForCustomer = ds


            '  Close the connection when done with it.
            myConnection.Close()


        End Function

        Public Function FillOrderPagesArray() As Boolean

            FillOrderPagesArray = True

            '' find the Pages folder for this category
            'Dim objMatrix As New VT_CommonFunctions.MatrixFunctions

            'Dim objCfg As New VT_CommonFunctions.MatrixFunctions
            'Dim intRoot As Integer = objCfg.GetModuleRootId(HttpContext.Current.Session("_VT_DotNetConnString"), "cfg_FormList", "ConfigModuleRoot")
            'Dim intModuleSetupsId As Integer = objCfg.GetGenericFolderId(HttpContext.Current.Session("_VT_DotNetConnString"), "cfg_FormList", intRoot, "ModuleSetups")
            'Dim intQuoteFolder As Integer = objMatrix.FindGenericFolderId(HttpContext.Current.Session("_VT_DotNetConnString"), "cfg_FormList", intModuleSetupsId, "Sales")
            'Dim intSetupFolder As Integer = objMatrix.FindGenericFolderId(HttpContext.Current.Session("_VT_DotNetConnString"), "cfg_FormList", intQuoteFolder, "Setup")
            'Dim intPagesFolder As Integer = objMatrix.FindGenericFolderId(HttpContext.Current.Session("_VT_DotNetConnString"), "cfg_FormList", intSetupFolder, "Pages")


            Dim astrOptions() As String
            Dim astrOptionPages() As String
            'Dim objComm As New VT_CommonFunctions.CommonFunctions
            'Dim dtPages As Data.DataTable

            'If intPagesFolder <> 0 Then
            '    ' if there is a pages folder for this category we can get the data from there
            '    Dim astrItems(2) As String
            '    astrItems(0) = "Order"
            '    astrItems(1) = "Page Title"
            '    astrItems(2) = "Page FileName"

            '    dtPages = objMatrix.GetData(HttpContext.Current.Session("_VT_DotNetConnString"), "ParentId", intPagesFolder.ToString, "cfg_FormData", astrItems)

            '    ReDim astrOptions(dtPages.Rows.Count - 1)
            '    ReDim astrOptionPages(dtPages.Rows.Count - 1)

            '    ' insert the pages in the arrays in the order of the OrderNum field
            '    For intIndex = 0 To dtPages.Rows.Count - 1
            '        astrOptions(dtPages.Rows(intIndex).Item("Field0") - 1) = dtPages.Rows(intIndex).Item("Field1")
            '        If Left(dtPages.Rows(intIndex).Item("Field2"), 1) = "~" Then
            '            astrOptionPages(dtPages.Rows(intIndex).Item("Field0") - 1) = dtPages.Rows(intIndex).Item("Field2")
            '        Else
            '            astrOptionPages(dtPages.Rows(intIndex).Item("Field0") - 1) = "~/CreateOrder/" + dtPages.Rows(intIndex).Item("Field2")
            '        End If
            '    Next

            'If Session("InstallationCompany") = "BALLINROE" Then
            '    ReDim astrOptions(3)
            '    ReDim astrOptionPages(3)


            '    astrOptions(0) = "Details"
            '    astrOptions(1) = "Products"
            '    astrOptions(2) = "Audit"
            '    astrOptions(3) = "Documents"

            '    astrOptionPages(0) = "~/CreateOrder/NewOrderDetails_New.aspx"
            '    astrOptionPages(1) = "~/CreateOrder/NewOrderProductsNew.aspx"
            '    astrOptionPages(2) = "~/CreateOrder/NewOrderAudit.aspx"
            '    astrOptionPages(3) = "~/CreateOrder/BallinroeDocumentCheck.aspx"

            '    Me.CurrentSession.aVT_DetailsPageOptions = astrOptions
            '    Me.CurrentSession.aVT_DetailsPageOptionsPages = astrOptionPages

            'Else
            ReDim astrOptions(2)
            ReDim astrOptionPages(2)

            astrOptions(0) = "Details"
            astrOptions(1) = "Products"
            astrOptions(2) = "Audit"

            astrOptionPages(0) = "~/CreateOrder/NewOrderDetails_New.aspx"
            astrOptionPages(1) = "~/CreateOrder/NewOrderProductsNew.aspx"
            astrOptionPages(2) = "~/CreateOrder/NewOrderAudit.aspx"

            Me.CurrentSession.aVT_DetailsPageOptions = astrOptions
            Me.CurrentSession.aVT_DetailsPageOptionsPages = astrOptionPages

            'End If

            'Else
            '    FillOrderPagesArray = False
            'End If


        End Function

        Public Function FillModulePagesArray() As Boolean

            FillModulePagesArray = True

            ' find the Pages folder for this category
            'Dim objMatrix As New VT_CommonFunctions.MatrixFunctions

            'Dim objCfg As New VT_CommonFunctions.MatrixFunctions
            'Dim intRoot As Integer = objCfg.GetModuleRootId(HttpContext.Current.Session("_VT_DotNetConnString"), "cfg_FormList", "ConfigModuleRoot")
            'Dim intModuleSetupsId As Integer = objCfg.GetGenericFolderId(HttpContext.Current.Session("_VT_DotNetConnString"), "cfg_FormList", intRoot, "ModuleSetups")
            'Dim intQuoteFolder As Integer = objMatrix.FindGenericFolderId(HttpContext.Current.Session("_VT_DotNetConnString"), "cfg_FormList", intModuleSetupsId, "Sales")
            'Dim intSetupFolder As Integer = objMatrix.FindGenericFolderId(HttpContext.Current.Session("_VT_DotNetConnString"), "cfg_FormList", intQuoteFolder, "Setup")
            'Dim intPagesFolder As Integer = objMatrix.FindGenericFolderId(HttpContext.Current.Session("_VT_DotNetConnString"), "cfg_FormList", intSetupFolder, "ModulePages")


            Dim astrOptions(2) As String
            Dim astrOptionPages(2) As String
            'Dim objComm As New VT_CommonFunctions.CommonFunctions
            'Dim dtPages As Data.DataTable

            'If intPagesFolder <> 0 Then
            '    ' if there is a pages folder for this category we can get the data from there
            '    Dim astrItems(2) As String
            '    astrItems(0) = "Order"
            '    astrItems(1) = "Page Title"
            '    astrItems(2) = "Page FileName"

            '    dtPages = objMatrix.GetData(HttpContext.Current.Session("_VT_DotNetConnString"), "ParentId", intPagesFolder.ToString, "cfg_FormData", astrItems)

            '    ReDim astrOptions(dtPages.Rows.Count - 1)
            '    ReDim astrOptionPages(dtPages.Rows.Count - 1)

            '    ' insert the pages in the arrays in the order of the OrderNum field
            '    For intIndex = 0 To dtPages.Rows.Count - 1
            '        astrOptions(dtPages.Rows(intIndex).Item("Field0") - 1) = dtPages.Rows(intIndex).Item("Field1")
            '        If Left(dtPages.Rows(intIndex).Item("Field2"), 1) = "~" Then
            '            astrOptionPages(dtPages.Rows(intIndex).Item("Field0") - 1) = dtPages.Rows(intIndex).Item("Field2")
            '        Else
            '            astrOptionPages(dtPages.Rows(intIndex).Item("Field0") - 1) = dtPages.Rows(intIndex).Item("Field2")
            '        End If
            '    Next

            astrOptions(0) = "Sales Order Opening"
            astrOptions(1) = "Sales Order Details"
            astrOptions(2) = "Planning Opening"

            astrOptionPages(0) = "Orders_Opening_New.aspx"
            astrOptionPages(1) = "Details_Opening.aspx"
            astrOptionPages(2) = "PlanningOrdersNew.aspx"

            Me.CurrentSession.aVT_ModulePageOptions = astrOptions
            Me.CurrentSession.aVT_ModulePageOptionsPages = astrOptionPages

            'Else
            'FillModulePagesArray = False
            'End If


        End Function


        Public Function getOrderHistoryForProductDeliveryCustomer(ByVal lngProductId As Long, ByVal lngDeliveryCustomerId As Long, ByVal intNumRecords As Integer) As Data.DataTable
            Dim dt As New Data.DataTable
            Dim strsql As String
            Dim strconn As String


            strsql = "SELECT TOP " & intNumRecords & " tls_SalesOrders.SalesOrderNum as OrderNum, tls_SalesOrders.SalesOrderId,tls_SalesOrders.RequestedDeliveryDate as ReqDate, " & _
                     "cus_Customers.CustomerName as DeliveryCustomer, tls_SalesOrders.Status as Status,   " & _
                     "(tls_SalesOrderItems.QuantityRequested ) as reqQty ,  " & _
                     "(tls_SalesOrderItems.Quantity) as Quantity, " & _
                    "(tls_SalesOrderItems.WeightREquested ) as reqWgt , " & _
                    "(tls_SalesOrderItems.Weight) as Weight,  " & _
                    "(tls_SalesOrderItems.UnitPrice), " & _
                    "(tls_SalesOrderItems.Vat), " & _
                    "(tls_SalesOrderItems.TotalPrice) " & _
                    "FROM tls_SalesOrders  " & _
                    "INNER JOIN cus_Customers on tls_SalesOrders.DeliveryCustomerId = cus_Customers.CustomerId  " & _
                    "INNER JOIN tls_SalesOrderItems on tls_SalesOrders.SalesOrderId = tls_SalesOrderItems.SalesOrderId  " & _
                    "WHERE (tls_SalesOrderItems.ProductId =  " & lngProductId & ")  " & _
                    " AND  (tls_SalesOrders.DeliveryCustomerId =  " & lngDeliveryCustomerId & " ) " & _
                    " ORDER BY OrderNum desc "


            strconn = Session("_VT_DotNetConnString")

            Dim myConnection As New SqlConnection(strconn)
            Dim comSQL As New SqlCommand(strsql, myConnection)

            Dim myAdapter As New SqlDataAdapter(comSQL)

            myConnection.Open()
            myAdapter.Fill(dt)

            getOrderHistoryForProductDeliveryCustomer = dt

            myConnection.Close()
            myConnection = Nothing

        End Function




        Public Function getDocketsAwaitingCommitment() As Data.DataTable
            Dim dt As New Data.DataTable
            Dim strsql As String
            Dim strconn As String

            Try

                strsql = "Select distinct tcd_tblSalesOrders.DocketNumString, tcd_tblSalesOrders.DateOfTransaction,tcd_tblSalesOrders.OrderNumber, "
                strsql = strsql + "cus_Customers.CustomerName, tcd_tblSalesOrders.DriverID "
                strsql = strsql + " FROM tcd_tblSalesOrders, cus_Customers "
                strsql = strsql + " WHERE tcd_tblSalesOrders.deliveryCustomerId = cus_Customers.CustomerId "
                strsql = strsql + " AND  tcd_tblSalesOrders.DocketStatus = '" & GetGlobalResourceObject("Resource", "Docket_AwaitingCommitment") & "' "
                strsql = strsql + "  Order By tcd_tblSalesOrders.OrderNumber"  'MLHIDE


                strconn = Session("_VT_DotNetConnString")

                Dim myConnection As New SqlConnection(strconn)
                Dim comSQL As New SqlCommand(strsql, myConnection)

                Dim myAdapter As New SqlDataAdapter(comSQL)

                myConnection.Open()
                myAdapter.Fill(dt)

                getDocketsAwaitingCommitment = dt

                myConnection.Close()
                myConnection = Nothing
            Catch ex As Exception
                Dim f As New UtilFunctions
                f.LogAction("Error in WasteSalesPortal:DBFunctions:SalesOrders:getDocketsAwaitingCommitment: " + ex.Message)

            Finally

            End Try

        End Function


        Public Function getDocketAwaitingCommitmentBySONum(ByVal lngOrderNum As Long) As Data.DataTable
            Dim dt As New Data.DataTable
            Dim strsql As String
            Dim strconn As String

            Try

                strsql = "Select distinct tcd_tblSalesOrders.DocketNumString, tcd_tblSalesOrders.DateOfTransaction,tcd_tblSalesOrders.OrderNumber, "
                strsql = strsql + "cus_Customers.CustomerName, tcd_tblSalesOrders.DriverID "
                strsql = strsql + " FROM tcd_tblSalesOrders, cus_Customers "
                strsql = strsql + " WHERE tcd_tblSalesOrders.deliveryCustomerId = cus_Customers.CustomerId "
                strsql = strsql + " AND  tcd_tblSalesOrders.DocketStatus = '" & GetGlobalResourceObject("Resource", "Docket_AwaitingCommitment") & "' "
                strsql = strsql + "  AND tcd_tblSalesOrders.OrderNumber = " & CStr(lngOrderNum)


                strconn = Session("_VT_DotNetConnString")

                Dim myConnection As New SqlConnection(strconn)
                Dim comSQL As New SqlCommand(strsql, myConnection)

                Dim myAdapter As New SqlDataAdapter(comSQL)

                myConnection.Open()
                myAdapter.Fill(dt)

                getDocketAwaitingCommitmentBySONum = dt

                myConnection.Close()
                myConnection = Nothing
            Catch ex As Exception
                Dim f As New UtilFunctions
                f.LogAction("Error in WasteSalesPortal:DBFunctions:SalesOrders:getDocketsAwaitingCommitment: " + ex.Message)

            Finally

            End Try

        End Function



        Public Function GetUncommitedDocketByDocNum(ByVal strDocketNum As String) As DataTable

            Dim strsql As String
            strsql = "SELECT  * FROM tcd_tblSalesOrders  WHERE (tcd_tblSalesOrders.DocketNumString = '" & strDocketNum & "')"

            Dim dt As New Data.DataTable

            Dim strconn As String

            strconn = Session("_VT_DotNetConnString")

            Dim myConnection As New SqlConnection(strconn)
            Dim comSQL As New SqlCommand(strsql, myConnection)

            Dim myAdapter As New SqlDataAdapter(comSQL)

            myConnection.Open()
            myAdapter.Fill(dt)

            GetUncommitedDocketByDocNum = dt

            myConnection.Close()
            myConnection = Nothing
        End Function


        Public Sub UpdateHHSOPriceAndVATByRecordId(ByVal lngRecordId As Long, ByVal dblNetPrice As Double, ByVal dblVAT As Double, ByVal dblUnitPrice As Double)

            Dim ObjHH As New VTDBFunctions.VTDBFunctions.Handheld
            Dim objDBFuncs As New VTDBFunctions.VTDBFunctions.DBFunctions


            Dim strConnString As String = Session("_VT_DotNetConnString")
            Dim myConnection As New SqlConnection(strConnString)

            Dim strsql As String

            strsql = "Update tcd_tblSalesOrders SET PriceSoldFor = " & CStr(dblNetPrice) & " , VATCharged  = " & CStr(dblVAT) & " WHERE RecordId = " + CStr(lngRecordId)



            Dim comSQL As New SqlCommand(strsql, myConnection)
            Dim myAdapter As New SqlDataAdapter(comSQL)

            myConnection.Open()
            comSQL.ExecuteNonQuery()

            '  Close the connection when done with it.
            myConnection.Close()
            myConnection = Nothing




            'If the unit price column exists then add the unit price
            If objDBFuncs.DoesColumnExist("tcd_tblSalesOrders", "UnitPrice") Then
                ObjHH.UpdateHHSOUnitPriceByRecord(lngRecordId, dblUnitPrice)
            End If


        End Sub





        Protected Overrides Sub Finalize()
            MyBase.Finalize()
        End Sub





    End Class
End Namespace

Namespace MatrixFunctionsForSales

    Public Class MatrixFunctions_Sales
        Inherits MyBasePage


    End Class
End Namespace

Namespace WorkFlowFunctionsForSales
    Public Class WorkFlowFunctions_Sales
        Inherits MyBasePage



        Public Function SteripackOrderItemsValidationChecks(ByRef dtItemRows As DataTable) As Boolean

            'SMcN 14/04/2014  This function passes in 'dtItemRows' datatable BY REFERENCE and the code will update the 'Item_OnHold' column as required
            ' The Function returns a FALSE if any line items rows have failed the validation checks

            Dim objForms As New VT_Forms.Forms
            Dim objTasks As New VT_eQOInterface.eQOInterface
            Dim objTele As New VTDBFunctions.VTDBFunctions.TelesalesFunctions
            Dim strAuditComment As String = ""
            Dim strOnHoldComment As String = ""

            Dim objC As New VT_CommonFunctions.CommonFunctions
            Dim objDataPreserve As New BPADotNetCommonFunctions.VT_DataPreserve.DataPreserve
            Dim objG As New BPADotNetCommonFunctions.VT_ReportFunctions.GeneralFunctions
            Dim objProd As New VTDBFunctions.VTDBFunctions.ProductsFunctions
            Dim objSales As New BPADotNetCommonFunctions.SalesFunctions.Sales_Orders

            Session("HoldDataToSave") = "NO"
            '****************************
            'AM 09/06/2014 need to order the datatable by item number
            'SMcN 09/10/2013 Change the types of the relevant columns to be numeric
            'This is necessary to facilite proper sum totaling on the WebDataGrid
            'Also you cannot change the data TYPE of a datatable column if it already has data so need to use a tempory table
            Dim dtSwap As New DataTable
            dtSwap = dtItemRows.Clone


            dtSwap.Columns("SalesItemNum").DataType = GetType(System.Int32)


            For intCnt As Integer = 0 To dtItemRows.Rows.Count - 1
                dtSwap.ImportRow(dtItemRows.Rows(intCnt))
            Next

            dtItemRows.Clear()


            dtSwap.DefaultView.Sort = "SalesItemNum ASC"
            dtItemRows = dtSwap.DefaultView.ToTable

            Dim strType As String = "Sales Order"
            Dim dtDetailsMatrix As New DataTable
            Dim strErr As String
            Dim objM As New VTDBFunctions.VTDBFunctions.VTMatrixFunctions
            Dim strConn As String = Session("_VT_DotNetConnString")
            Dim blnOrderValidationOk, blnLineValidationOk As Boolean

            Dim lngOrderNum As Long = Me.CurrentSession.VT_SalesOrderNum
            Dim lngOrderId As Long = Me.CurrentSession.VT_SalesOrderID
            Dim dsOrder As New DataSet

            Dim strOrderStatus As String
            Dim objSalesFuncs As New VTDBFunctions.VTDBFunctions.TelesalesFunctions
            dsOrder = objSalesFuncs.GetOrderForId(lngOrderId)
            If dsOrder.Tables(0).Rows.Count > 0 Then
                strOrderStatus = Trim(dsOrder.Tables(0).Rows(0).Item("Status"))
            Else
                strOrderStatus = Trim(Me.CurrentSession.VT_CurrentNewOrderStatus)
            End If

            If lngOrderId > 0 Then

                'if the Sales Order was already put on hold Manually then skip these checks and leave the order on-hold. It can only be take off hold manually
                If Trim(UCase(strOrderStatus)) = Trim(UCase(GetGlobalResourceObject("Resource", "Order_OnHold_Manual"))) Then

                    SteripackOrderItemsValidationChecks = True
                    Exit Function

                End If

                'only run validation checks for orders that are either on hold or pre issue. Do not run checks for orders already gone to planning and further
                If Trim(UCase(strOrderStatus)) = Trim(UCase(GetGlobalResourceObject("Resource", "Order_AwaitingPlanning"))) Or Trim(UCase(strOrderStatus)) = Trim(UCase(GetGlobalResourceObject("Resource", "Order_SendAcknowledge"))) Or InStr(UCase(strOrderStatus), "CLOSED") > 0 Then

                    SteripackOrderItemsValidationChecks = True
                    Exit Function

                End If
                'Setup the 'dtDetailsMatrix' for later use
                dtDetailsMatrix.Columns.Add("MatrixLinkId")
                dtDetailsMatrix.Rows.Add()

                'SmcN  11/03/2014 Added this line to serialise the comlete grid as currently on the client surface because Infragisitcs CRUD doesnt work for all behaviours.
                'dtItemRows = objC.SerialiseFullWebDataGrid(wdgThis)

                blnOrderValidationOk = True
                strAuditComment = ""
                Dim objP As New ProductsFunctions.Products
                Dim dblPriceTolerance As Double
                Dim dblTotalPrice As Double
                Dim strPriceDiff As String
                Dim dblpricediff As Double
                Dim objDB As New VTDBFunctions.VTDBFunctions.DBFunctions

                If dtItemRows.Rows.Count > 0 Then
                    'Only run this code if there are items in the grid
                    For intCnt As Integer = 0 To dtItemRows.Rows.Count - 1

                        'Set the default for 'blnLineValidationOk'
                        blnLineValidationOk = True

                        'if this is a service item we don't run the validation checks

                        If (objP.ProductIsAService(dtItemRows.Rows(intCnt).Item("ProductID")) = False) And (dtItemRows.Rows(intCnt).Item("QuantityRequested") > 0 Or dtItemRows.Rows(intCnt).Item("WeightRequested") > 0) Then
                            'Check for ---- a price difference'
                            'in Malaysia Steripack do not validate pricing in the portal so orders don't go on hold for pricing issues
                            If UCase(objC.GetConfigItem("SkipPriceValidation")) <> "YES" Then
                                If Not IsDBNull(dtItemRows.Rows(intCnt).Item("PriceDifference")) Then

                                    If dtItemRows.Rows(intCnt).Item("PriceDifference") <> 0 Then

                                        dblTotalPrice = IIf(IsDBNull(dtItemRows.Rows(intCnt).Item("PO_TotalPrice")), 0, dtItemRows.Rows(intCnt).Item("PO_TotalPrice"))
                                        If dblTotalPrice <> 0 Then
                                            dblPriceTolerance = (dblTotalPrice / 100) * 0.25
                                            strPriceDiff = dtItemRows.Rows(intCnt).Item("PriceDifference")
                                            If Left(strPriceDiff, 1) = "-" Then
                                                strPriceDiff = Mid(strPriceDiff, 2)

                                            End If
                                            dblpricediff = CDbl(strPriceDiff)


                                        Else 'if there is no po price it should go on hold
                                            blnLineValidationOk = False

                                            'audit log
                                            'if there are any items on hold due to part num missing then the order on hold comment should be for missing part num
                                            'regardless of whether any items are missing a price. The price can't be added until the part num is added
                                            'engineering will not see that the order is missing a part num in the orders opening page if the on hold comment
                                            'is for missing price.
                                            If strAuditComment = "" Then
                                                strAuditComment = "Order Put On Hold: Price Difference on item on row [" & (intCnt + 1).ToString & "] Product: " & dtItemRows.Rows(intCnt).Item("ProductName")
                                            End If

                                            'Write the reason for being on hold to the Sales Order Matrix
                                            If strOnHoldComment = "" Then
                                                strOnHoldComment = "[System]  Price Difference on item on row [" & (intCnt + 1).ToString & "] Product: " & dtItemRows.Rows(intCnt).Item("ProductName")
                                            End If

                                            'save true value to this variable so that the status will update on the orders grid
                                            Me.CurrentSession.blnOrderStatusChanged = True
                                        End If

                                        If dblpricediff > dblPriceTolerance Or dblpricediff < 0 Then
                                            blnLineValidationOk = False

                                            'audit log
                                            'if there are any items on hold due to part num missing then the order on hold comment should be for missing part num
                                            'regardless of whether any items are missing a price. The price can't be added until the part num is added
                                            'engineering will not see that the order is missing a part num in the orders opening page if the on hold comment
                                            'is for missing price.
                                            If strAuditComment = "" Then
                                                strAuditComment = "Order Put On Hold: Price Difference on item on row [" & (intCnt + 1).ToString & "] Product: " & dtItemRows.Rows(intCnt).Item("ProductName")
                                            End If

                                            'Write the reason for being on hold to the Sales Order Matrix
                                            If strOnHoldComment = "" Then
                                                strOnHoldComment = "[System]  Price Difference on item on row [" & (intCnt + 1).ToString & "] Product: " & dtItemRows.Rows(intCnt).Item("ProductName")
                                            End If

                                            'save true value to this variable so that the status will update on the orders grid
                                            Me.CurrentSession.blnOrderStatusChanged = True

                                        End If
                                    End If

                                End If

                                'Check for ---- No price on product item
                                If IsDBNull(dtItemRows.Rows(intCnt).Item("TotalPrice")) Or Val(dtItemRows.Rows(intCnt).Item("TotalPrice")) <= 0 Then
                                    blnLineValidationOk = False

                                    'audit log
                                    If strAuditComment = "" Then
                                        strAuditComment = "Order Put On Hold: No Price for item on row [" & (intCnt + 1).ToString & "] Product: " & dtItemRows.Rows(intCnt).Item("ProductName")
                                    End If

                                    'Write the reason for being on hold to the Sales Order Matrix
                                    If strOnHoldComment = "" Then
                                        strOnHoldComment = "[System]  No Price for item on row [" & (intCnt + 1).ToString & "] Product: " & dtItemRows.Rows(intCnt).Item("ProductName")
                                    End If

                                    'save true value to this variable so that the status will update on the orders grid
                                    Me.CurrentSession.blnOrderStatusChanged = True
                                End If
                            End If

                            'Check for --- No product code 
                            If Not IsDBNull(dtItemRows.Rows(intCnt).Item("ProductCode")) Then
                                If dtItemRows.Rows(intCnt).Item("ProductCode") = "" Then

                                    blnLineValidationOk = False

                                    'audit log
                                    strAuditComment = "Order Put On Hold: No Product Code for item on row [" & (intCnt + 1).ToString & "] Product: " & dtItemRows.Rows(intCnt).Item("ProductName")

                                    'Write the reason for being on hold to the Sales Order Matrix
                                    strOnHoldComment = "[System]  No Product Code for item on row [" & (intCnt + 1).ToString & "] Product: " & dtItemRows.Rows(intCnt).Item("ProductName")

                                    'save true value to this variable so that the status will update on the orders grid
                                    Me.CurrentSession.blnOrderStatusChanged = True
                                End If


                            End If

                            'Check for --- Wrong product code 
                            If Not IsDBNull(dtItemRows.Rows(intCnt).Item("ProductCode")) Then
                                If dtItemRows.Rows(intCnt).Item("ProductCode") <> "" Then

                                    'Check if a product code exists for this product
                                    Dim strTemp As String = objProd.GetProductIdForCode(dtItemRows.Rows(intCnt).Item("ProductCode"))

                                    If strTemp = "" Then
                                        'No product code found
                                        blnLineValidationOk = False

                                        'audit log
                                        strAuditComment = "Order Put On Hold: Product Code not in database for item on row [" & (intCnt + 1).ToString & "] Product: " & dtItemRows.Rows(intCnt).Item("ProductName")

                                        'Write the reason for being on hold to the Sales Order Matrix
                                        strOnHoldComment = "[System]  Product Code not in database for item on row [" & (intCnt + 1).ToString & "] Product: " & dtItemRows.Rows(intCnt).Item("ProductName")

                                        'save true value to this variable so that the status will update on the orders grid
                                        Me.CurrentSession.blnOrderStatusChanged = True
                                    Else
                                        'need to check if this is a product that has been added. if so we should take the order
                                        'off hold and add the product desc, dimensions etc
                                        If dtItemRows.Rows(intCnt).Item("CustomerCode") = "" And dtItemRows.Rows(intCnt).Item("ProductName") = "" Then
                                            Dim dtProd As New DataTable
                                            dtProd = GetProductDataForCode_Steri(dtItemRows.Rows(intCnt).Item("ProductCode"))
                                            If Not IsNothing(dtProd) And dtProd.Rows.Count > 0 Then

                                                dtItemRows.Rows(intCnt).Item("CustomerCode") = IIf(IsDBNull(dtProd.Rows(0).Item("CustPartNum")), "", dtProd.Rows(0).Item("CustPartNum"))
                                                dtItemRows.Rows(intCnt).Item("CustomerRev") = IIf(IsDBNull(dtProd.Rows(0).Item("CustPartNumRev")), "", dtProd.Rows(0).Item("CustPartNumRev"))

                                                dtItemRows.Rows(intCnt).Item("DimWidth") = IIf(IsDBNull(dtProd.Rows(0).Item("DimWidth")), "", dtProd.Rows(0).Item("DimWidth"))

                                                dtItemRows.Rows(intCnt).Item("DimLength") = IIf(IsDBNull(dtProd.Rows(0).Item("DimLength")), "", dtProd.Rows(0).Item("DimLength"))
                                                dtItemRows.Rows(intCnt).Item("ProductName") = IIf(IsDBNull(dtProd.Rows(0).Item("Product_Description")), "", dtProd.Rows(0).Item("Product_Description"))
                                                Session("HoldDataToSave") = "YES"
                                                Dim strsql As String = "Update tls_SalesOrderItems set ProductId = " & dtProd.Rows(0).Item("ProductId") & " where OrderItemId = " & dtItemRows.Rows(intCnt).Item("SalesOrderItemId")
                                                objDB.ExecuteSQLQuery(strsql)

                                            End If
                                        End If
                                    End If

                                End If

                            End If


                            'if the product item Line validation check is false then process line actions here
                            If blnLineValidationOk = False Then


                                dtItemRows.Rows(intCnt).Item("Item_OnHold") = "Yes"
                                If Not IsDBNull(dtItemRows.Rows(intCnt).Item("Item_OnHoldDate")) AndAlso dtItemRows.Rows(intCnt).Item("Item_OnHoldDate") = "" Then
                                    dtItemRows.Rows(intCnt).Item("Item_OnHoldDate") = Format(PortalFunctions.Now, "s")
                                End If

                                blnOrderValidationOk = False

                            Else
                                'If here then this line validation is OK
                                dtItemRows.Rows(intCnt).Item("Item_OnHold") = "No"
                                dtItemRows.Rows(intCnt).Item("Item_OnHoldDate") = ""
                            End If

                        End If

                    Next

                    'Save the Status first
                    'objDBF.SaveGeneralItemsToSalesMatrix("StatusBefore_OnHold", "Details", Me.CurrentSession.VT_CurrentNewOrderStatus)

                    SteripackOrderItemsValidationChecks = True

                Else

                    SteripackOrderItemsValidationChecks = False

                End If



                'Check the order Validation status and process this
                If blnOrderValidationOk = True Then
                    'If we are here then it means that all validation checks have passed
                    'If the Current status on ON HOLD then take this job  OFF HOLD

                    If strOrderStatus = GetGlobalResourceObject("Resource", "Order_OnHold_System") Or strOrderStatus = "" Then
                        'Set the Sales order back to the Status it was at before it was put on hold
                        Dim strPreviousStatus As String = ""
                        If Not IsNothing(Me.CurrentSession.VT_SelectedSalesOrderRow_V2) Then
                            strPreviousStatus = Me.CurrentSession.VT_SelectedSalesOrderRow_V2.Rows(0).Item("StatusBefore_OnHold")
                        End If

                        'If there is no previous status stored then set it to PreIssued
                        If strPreviousStatus = "" Or strPreviousStatus = GetGlobalResourceObject("Resource", "Order_OnHold_System") Or strPreviousStatus = GetGlobalResourceObject("Resource", "Order_OnHold_Manual") Then
                            strPreviousStatus = GetGlobalResourceObject("Resource", "Order_PreIssued")
                        End If

                        objForms.SetJobStatusText(strPreviousStatus, lngOrderNum)
                        objTasks.SetLastActivity(lngOrderNum, PortalFunctions.Now.Date)
                        objTele.UpdateOrderStatus(lngOrderId, strPreviousStatus)
                        Me.CurrentSession.VT_CurrentNewOrderStatus = strPreviousStatus


                        'Clear the ReasonOnHold comment in the matrix
                        objSales.SaveGeneralItemsToSalesMatrix("ReasonOnHold", "Details", "", CInt(Me.CurrentSession.VT_SalesOrderNum), strConn)

                        'clear the date put on hold 
                        'SP do not want this cleared. They want the initial date on hold saved so that the num days on hold counts from this
                        '  objSales.SaveGeneralItemsToSalesMatrix("DateOnHold", "Details", "", CInt(Me.CurrentSession.VT_SalesOrderNum), strConn)


                        'audit log
                        objForms.WriteToAuditLog(lngOrderNum, strType, PortalFunctions.Now, "System:", 0, "Order taken OFF Hold: All product item validation checks passed", "Audit Record", "SysAdmin")
                    End If

                Else
                    'Order Validation check has failed so Set status to On Hold
                    objForms.SetJobStatusText(GetGlobalResourceObject("Resource", "Order_OnHold_System"), lngOrderNum)
                    objTasks.SetLastActivity(lngOrderNum, PortalFunctions.Now.Date)
                    objTele.UpdateOrderStatus(lngOrderId, GetGlobalResourceObject("Resource", "Order_OnHold_System"))
                    Me.CurrentSession.VT_CurrentNewOrderStatus = GetGlobalResourceObject("Resource", "Order_OnHold_System")

                    ' Get the existing 'ReasonOnHold' comment from the Matrix
                    dtDetailsMatrix.Rows(0).Item("MatrixLinkId") = Me.CurrentSession.VT_OrderMatrixID
                    Dim astrItems(0) As String
                    astrItems(0) = "ReasonOnHold"

                    Dim strFormName As String = "Details"
                    Dim strKeyField As String = "MatrixLinkId"

                    strErr = objM.GetBlockOfMatrixDataItems(strConn, strFormName, VTDBFunctions.VTDBFunctions.VTMatrixFunctions.Sales, astrItems, dtDetailsMatrix, strKeyField)

                    'Check if this comment is different from the existing comment and if it is, then log it.
                    If dtDetailsMatrix.Rows.Count > 0 Then
                        If Not IsDBNull(dtDetailsMatrix.Rows(0).Item("ReasonOnHold")) AndAlso dtDetailsMatrix.Rows(0).Item("ReasonOnHold") <> "" Then
                            If Trim(strOnHoldComment) <> Trim(dtDetailsMatrix.Rows(0).Item("ReasonOnHold")) Then
                                objForms.WriteToAuditLog(lngOrderNum, strType, PortalFunctions.Now, "System:", 0, strAuditComment, "Audit Record", "SysAdmin")
                                objSales.SaveGeneralItemsToSalesMatrix("ReasonOnHold", "Details", strOnHoldComment, Me.CurrentSession.VT_NewOrderNum, strConn)
                            End If
                        Else
                            'We will be here if this is the first time an Audit Log was written
                            objForms.WriteToAuditLog(lngOrderNum, strType, PortalFunctions.Now, "System:", 0, strAuditComment, "Audit Record", "SysAdmin")
                            objSales.SaveGeneralItemsToSalesMatrix("ReasonOnHold", "Details", strOnHoldComment, Me.CurrentSession.VT_NewOrderNum, strConn)
                        End If

                    Else
                        objForms.WriteToAuditLog(lngOrderNum, strType, PortalFunctions.Now, "System:", 0, strAuditComment, "Audit Record", "SysAdmin")
                        objSales.SaveGeneralItemsToSalesMatrix("ReasonOnHold", "Details", strOnHoldComment, Me.CurrentSession.VT_NewOrderNum, strConn)

                    End If

                    'save the date on hold. first check if there is already a date there, don't overwrite it
                    astrItems(0) = "DateOnHold"
                    strErr = objM.GetBlockOfMatrixDataItems(strConn, strFormName, VTDBFunctions.VTDBFunctions.VTMatrixFunctions.Sales, astrItems, dtDetailsMatrix, strKeyField)


                    If dtDetailsMatrix.Rows.Count > 0 Then
                        If IsDBNull(dtDetailsMatrix.Rows(0).Item("DateOnHold")) Then

                            objSales.SaveGeneralItemsToSalesMatrix("DateOnHold", "Details", PortalFunctions.Now.Date.ToString("s"), Me.CurrentSession.VT_NewOrderNum, strConn)
                        ElseIf dtDetailsMatrix.Rows(0).Item("DateOnHold") = "" Then
                            objSales.SaveGeneralItemsToSalesMatrix("DateOnHold", "Details", PortalFunctions.Now.Date.ToString("s"), Me.CurrentSession.VT_NewOrderNum, strConn)

                        End If
                    Else
                        objSales.SaveGeneralItemsToSalesMatrix("DateOnHold", "Details", PortalFunctions.Now.Date.ToString("s"), Me.CurrentSession.VT_NewOrderNum, strConn)

                    End If

                    'Clean up the datatable for the next loop
                    dtDetailsMatrix.Rows.Clear()
                    dtDetailsMatrix.Columns.Remove("ReasonOnHold")

                End If

            Else
                SteripackOrderItemsValidationChecks = False
            End If



        End Function

        Function GetProductDataForCode_Steri(strProdCode As String) As DataTable
            Dim objProd As New VTDBFunctions.VTDBFunctions.ProductsFunctions
            Dim objG As New BPADotNetCommonFunctions.VT_ReportFunctions.GeneralFunctions
            Dim strSearch As String
            Dim dtTemp As New DataTable
            Dim dsMatrixItems As New DataSet
            Dim dsTemp As New DataSet
            Dim dtSearch As New DataTable
            Dim intDimNodeId As Integer
            Dim strDataTableName As String = VT_CommonFunctions.MatrixFunctions.MaterialsFormDataTable
            Dim objMatrix As New VT_CommonFunctions.MatrixFunctions
            Dim objVTM As New VTDBFunctions.VTDBFunctions.VTMatrixFunctions

            Dim dtP As DataTable = objProd.GetProductForCode(strProdCode).Tables(0)

            'Add Dimension columns to this grid and fill them
            If dtP IsNot Nothing And dtP.Rows.Count > 0 Then
                'Add the additional columns needed
                dtP.Columns.Add("DimLength")
                dtP.Columns.Add("DimWidth")
                dtP.Columns.Add("CustPartNum")
                dtP.Columns.Add("CustPartNumRev")

                intDimNodeId = dtP.Rows(0).Item("MatrixID")

                dtTemp = objProd.GetAllDimensionsForProduct(HttpContext.Current.Session("_VT_DotNetConnString"), intDimNodeId)

                If dtTemp.Rows.Count > 0 Then
                    strSearch = "Field1 = 'Length OD'"
                    dtSearch = objG.SearchDataTable(strSearch, dtTemp)
                    If dtSearch IsNot Nothing AndAlso dtSearch.Rows.Count > 0 Then
                        'Note: The specification value is stored in 'Field3'
                        dtP.Rows(0).Item("DimLength") = dtSearch.Rows(0).Item("Field3")
                    End If

                    strSearch = "Field1 = 'Width OD'"
                    dtSearch = objG.SearchDataTable(strSearch, dtTemp)
                    If dtSearch IsNot Nothing AndAlso dtSearch.Rows.Count > 0 Then
                        'Note: The specification value is stored in 'Field3'
                        dtP.Rows(0).Item("DimWidth") = dtSearch.Rows(0).Item("Field3")
                    End If

                End If


                'Add the Customer PartNumber to the Output Table
                Dim intDetailsId As Integer = objVTM.GetGenericFolderId(HttpContext.Current.Session("_VT_DotNetConnString"), "mat_FormList", intDimNodeId, "ProductDetails")
                Dim strTemp As String = objVTM.GetVTMatrixFormDataItem(HttpContext.Current.Session("_VT_DotNetConnString"), "mat_FormData", intDetailsId, "txtCustPartNum")
                dtP.Rows(0).Item("CustPartNum") = strTemp

                strTemp = objVTM.GetVTMatrixFormDataItem(HttpContext.Current.Session("_VT_DotNetConnString"), "mat_FormData", intDetailsId, "txtCustPartNumRev")
                dtP.Rows(0).Item("CustPartNumRev") = strTemp

                'Add the Price break info ?
                GetProductDataForCode_Steri = dtP


            Else

                GetProductDataForCode_Steri = dtP

            End If
        End Function
    End Class
End Namespace

Namespace ProductsFunctions
    Public Class Products
        Inherits MyBasePage

        Structure TProductRecord
            Public lngProductId As Long
            Public dblPrice As Double
        End Structure

        Structure TCustomerRecord
            Public lngCustomerId As Long
            Public dblDiscountPercent As Double
        End Structure

        Public udtCustomer As TCustomerRecord

        Public udtProduct As TProductRecord
        Function GetTraceCodesDTForProductSelect(lngProductId As Long) As Data.DataTable

            Dim objDataAccess As New VTDBFunctions.VTDBFunctions.ProductsFunctions
            Dim dsBatch As New DataSet
            Dim objBPA As New VT_CommonFunctions.CommonFunctions
            Dim dsProducts As New DataSet
            Dim dblVatRate As Double
            Dim dsLocations As New DataSet
            Dim dblTotalInStock As Double
            Dim i As Integer
            Dim row As New Infragistics.WebUI.UltraWebGrid.UltraGridRow
            Dim objTrace As New VTDBFunctions.VTDBFunctions.TraceFunctions
            Dim dblQtyInStock, dblAllocated As Double
            Dim objTelesales As New VTDBFunctions.VTDBFunctions.TelesalesFunctions

            Dim strDisplay As String = objBPA.GetConfigItem("UseByOrBestBefore")
            Dim strSort As String
            If strDisplay = "SellBy" Then
                strSort = "SellbyDate"
            Else
                strSort = "UseByDate"
            End If
            dsBatch = objDataAccess.GetSortedTraceCodesForProduct(lngProductId, strSort)

            dsProducts = objDataAccess.GetProductForId(lngProductId)
            If dsProducts.Tables(0).Rows.Count > 0 Then
                If Session("_VT_VatExempt") = True Then
                    dblVatRate = 0
                Else
                    If IsDBNull(dsProducts.Tables(0).Rows(0).Item("Vat_Rate")) Then
                        dblVatRate = 0
                    Else
                        If Trim(dsProducts.Tables(0).Rows(0).Item("Vat_Rate")) <> "" Then
                            dblVatRate = IIf(IsDBNull(dsProducts.Tables(0).Rows(0).Item("Vat_Rate")), 0, dsProducts.Tables(0).Rows(0).Item("Vat_Rate"))
                        Else
                            dblVatRate = 0
                        End If

                    End If

                End If

            End If
            '
            Dim dtReturn As New DataTable
            Dim intNumRows As Integer
            With dtReturn
                .Columns.Add("TraceCodeId")
                .Columns.Add("TraceCode")
                .Columns.Add("InStock")
                .Columns.Add("Qty")
                .Columns.Add("Wgt")
                .Columns.Add("UnitPrice")
                .Columns.Add("Location")
                .Columns.Add("LocationId")
                .Columns.Add("NetPrice")
                .Columns.Add("Vat")
                .Columns.Add("TotalPrice")
                .Columns.Add("SerialNum")
                .Columns.Add("Barcode")
                .Columns.Add("UseByOrSellBy")
                .Columns.Add("DateRec")

            End With
            For i = 0 To dsBatch.Tables(0).Rows.Count - 1
                If dsBatch.Tables(0).Rows(i).Item("NumInStores") <> 0 Then
                    'Now populate the grid
                    With dtReturn
                        .Rows.Add()
                        intNumRows = intNumRows + 1

                        .Rows(intNumRows - 1).Item("TraceCodeID") = dsBatch.Tables(0).Rows(i).Item("TraceCodeId")

                        If IsDBNull(dsBatch.Tables(0).Rows(i).Item("TraceCodeDesc")) Then
                            .Rows(intNumRows - 1).Item("TraceCode") = ""
                        Else
                            .Rows(intNumRows - 1).Item("TraceCode") = Trim(dsBatch.Tables(0).Rows(i).Item("TraceCodeDesc"))
                        End If

                        ' DateReceived
                        If IsDBNull(dsBatch.Tables(0).Rows(i).Item("DateEnteredInSystem")) Then
                            .Rows(intNumRows - 1).Item("DateRec") = ""
                        Else
                            .Rows(intNumRows - 1).Item("DateRec") = Trim(dsBatch.Tables(0).Rows(i).Item("DateEnteredInSystem"))
                        End If

                        ' SellBy or UseBy
                        If strDisplay = "SellBy" Then
                            If IsDBNull(dsBatch.Tables(0).Rows(i).Item("SellbyDate")) Then
                                .Rows(intNumRows - 1).Item("UseByOrSellBy") = ""
                            Else
                                .Rows(intNumRows - 1).Item("UseByOrSEllBy") = Trim(dsBatch.Tables(0).Rows(i).Item("SellbyDate"))
                            End If
                        Else

                            If IsDBNull(dsBatch.Tables(0).Rows(i).Item("UseByDate")) Then
                                .Rows(intNumRows - 1).Item("UseByOrSellBy") = ""
                            Else
                                .Rows(intNumRows - 1).Item("UseByOrSellBy") = Trim(dsBatch.Tables(0).Rows(i).Item("UseByDate"))
                            End If
                        End If


                        ' QtyInStock
                        If IsDBNull(dsBatch.Tables(0).Rows(i).Item("NumInStores")) Then
                            dblQtyInStock = 0
                        Else
                            dblQtyInStock = CDbl(Trim(dsBatch.Tables(0).Rows(i).Item("NumInStores")))
                        End If

                        If IsDBNull(dsBatch.Tables(0).Rows(i).Item("TraceAllocated")) Then
                            dblAllocated = 0
                        Else
                            dblAllocated = CDbl(Trim(dsBatch.Tables(0).Rows(i).Item("TraceAllocated")))
                        End If
                        .Rows(intNumRows - 1).Item("InStock") = dblQtyInStock - dblAllocated

                        ' default the Qty and Wgt columns to 1
                        .Rows(intNumRows - 1).Item("Qty") = 1
                        .Rows(intNumRows - 1).Item("Wgt") = 1

                        'Price Charged
                        'just display the unit price and the user can edit it
                        .Rows(intNumRows - 1).Item("UnitPrice") = FormatCurrency(GetPriceCharged(lngProductId), 2)

                        'location
                        dsLocations = objTrace.GetLocationData(dsBatch.Tables(0).Rows(i).Item("TraceCodeID"), 0)
                        If dsLocations.Tables(0).Rows.Count > 0 Then
                            .Rows(intNumRows - 1).Item("Location") = objTrace.GetLocationTextForId(dsLocations.Tables(0).Rows(0).Item("LocationId"))
                            .Rows(intNumRows - 1).Item("LocationId") = dsLocations.Tables(0).Rows(0).Item("LocationId")

                        End If
                        'vat rate
                        .Rows(intNumRows - 1).Item("Vat") = dblVatRate
                        ' store the TracecodeId in the row Tag
                        .Rows(intNumRows - 1).Item("TraceCodeID") = dsBatch.Tables(0).Rows(i).Item("TraceCodeID")
                    End With
                End If

            Next

            GetTraceCodesDTForProductSelect = dtReturn
        End Function

        Public Function GetDTForProductSelectWithoutbatches(lngproductId As Long) As DataTable
            Dim dsProducts As New DataSet
            Dim objDataAccess As New VTDBFunctions.VTDBFunctions.ProductsFunctions
            Dim objTrace As New VTDBFunctions.VTDBFunctions.TraceFunctions
            Dim dsbatch As New DataSet
            Dim dblVatRate As Double
            Dim objBPA As New VT_CommonFunctions.CommonFunctions

            dsProducts = objDataAccess.GetProductForId(lngproductId)
            Dim strDisplay As String = objBPA.GetConfigItem("UseByOrBestBefore")
            Dim strSort As String
            If strDisplay = "SellBy" Then
                strSort = "SellbyDate"
            Else
                strSort = "UseByDate"
            End If
            dsbatch = objDataAccess.GetSortedTraceCodesForProduct(lngproductId, strSort)

            dsProducts = objDataAccess.GetProductForId(lngproductId)
            If dsProducts.Tables(0).Rows.Count > 0 Then
                If Session("_VT_VatExempt") = True Then
                    dblVatRate = 0
                Else
                    If IsDBNull(dsProducts.Tables(0).Rows(0).Item("Vat_Rate")) Then
                        dblVatRate = 0
                    Else
                        If Trim(dsProducts.Tables(0).Rows(0).Item("Vat_Rate")) <> "" Then
                            dblVatRate = IIf(IsDBNull(dsProducts.Tables(0).Rows(0).Item("Vat_Rate")), 0, dsProducts.Tables(0).Rows(0).Item("Vat_Rate"))
                        Else
                            dblVatRate = 0
                        End If

                    End If

                End If

            End If

            Dim dtReturn As DataTable
            Dim intNumRows As Integer
            With dtReturn
                .Columns.Add("TraceCodeId")
                .Columns.Add("TraceCode")
                .Columns.Add("InStock")
                .Columns.Add("Qty")
                .Columns.Add("Wgt")
                .Columns.Add("UnitPrice")
                .Columns.Add("Location")
                .Columns.Add("LocationId")
                .Columns.Add("NetPrice")
                .Columns.Add("Vat")
                .Columns.Add("TotalPrice")
                .Columns.Add("SerialNum")
                .Columns.Add("Barcode")
                .Columns.Add("UseByOrSellBy")
                .Columns.Add("DateRec")

            End With
            Dim dblQtyInStock As Double
            Dim dblAllocated As Double
            Dim dsLocations As New DataSet

            With dtReturn
                .Rows.Add()

                ' Free Stock
                dblQtyInStock = IIf(IsDBNull(dsProducts.Tables(0).Rows(0).Item("InStock")), 0, dsProducts.Tables(0).Rows(0).Item("InStock"))
                dblAllocated = IIf(IsDBNull(dsProducts.Tables(0).Rows(0).Item("Allocated")), 0, dsProducts.Tables(0).Rows(0).Item("Allocated"))

                ' default the Qty and Wgt columns to 1
                .Rows(0).Item("Qty") = 1
                .Rows(0).Item("Wgt") = 1

                .Rows(0).Item("InStock") = dblQtyInStock - dblAllocated
                'Price Charged
                'just display the unit price and the user can edit it
                .Rows(0).Item("UnitPrice") = FormatCurrency(GetPriceCharged(lngproductId), 2)
                'location

                dsLocations = objTrace.GetLocationData(dsbatch.Tables(0).Rows(0).Item("TraceCodeID"), 0)
                If dsLocations.Tables(0).Rows.Count > 0 Then
                    .Rows(0).Item("Location") = objTrace.GetLocationTextForId(dsLocations.Tables(0).Rows(0).Item("LocationId"))
                    .Rows(0).Item("LocationId") = dsLocations.Tables(0).Rows(0).Item("LocationID")
                End If
                'vat rate
                .Rows(0).Item("VAT") = dblVatRate
            End With

            GetDTForProductSelectWithoutbatches = dtReturn
        End Function
        Public Function GetPriceCharged(ByVal lngProductId As Long) As Double

            Dim strListPrice As String
            Dim objDataAccess As New VTDBFunctions.VTDBFunctions.ProductsFunctions

            Dim strPriceToDisplay As String

            'we need both the customer and product selected before we can calculate the price

            With udtCustomer
                .lngCustomerId = Session("_VT_CustomerID")
                .dblDiscountPercent = Session("_VT_CustDiscount")
            End With

            With udtProduct
                .lngProductId = CLng(lngProductId)
                strListPrice = objDataAccess.GetProductListPrice(.lngProductId)
                If strListPrice <> "" Then
                    .dblPrice = CDbl(strListPrice)
                End If
            End With

            strPriceToDisplay = calculateCustomerDiscount(udtProduct, udtCustomer)
            If strPriceToDisplay <> "" And IsNumeric(strPriceToDisplay) = True Then
                GetPriceCharged = CDbl(strPriceToDisplay)
            Else
                GetPriceCharged = udtProduct.dblPrice
            End If

            Exit Function

        End Function

        Public Function GetAlternativeUMDTForPart(strpartNum As String) As DataTable
            Dim dtp As Data.DataTable
            Dim strsql As String
            Dim objMFG As New SteripackMFGProInterface.MFGProFunctions
            Dim strMFGProConnString As String = System.Configuration.ConfigurationManager.AppSettings("MFGPro_DB_CONN")
            Dim strMFGProDomain As String = System.Configuration.ConfigurationManager.AppSettings("MFGPro_Domain")
            Dim strMFGProError As String = ""
            Dim i As Integer
            Dim strUOM As String
            Dim dtp2 As New DataTable

            strsql = "Select pi_um as um from PUB.pi_mstr where pi_part_code='" & strpartNum & "'"

            dtp = objMFG.GetDataTableFromMFGPro(strMFGProConnString, strsql, strMFGProError)


            strsql = "Select um_alt_um as um from PUB.um_mstr where um_part ='" & strpartNum & "'"
            dtp2 = objMFG.GetDataTableFromMFGPro(strMFGProConnString, strsql, strMFGProError)

            For Each dr As DataRow In dtp2.Rows
                dtp.Rows.Add(dr.ItemArray)
            Next

            strsql = "Select um_um as um from pub.um_mstr where um_part = '" & strpartNum & "'"
            dtp2 = objMFG.GetDataTableFromMFGPro(strMFGProConnString, strsql, strMFGProError)

            For Each dr As DataRow In dtp2.Rows
                dtp.Rows.Add(dr.ItemArray)
            Next

            'remove duplicates
            Dim colNames(0) As String
            colNames(0) = "um"

            dtp = dtp.DefaultView.ToTable(True, colNames)

            GetAlternativeUMDTForPart = dtp
            '    End If
            'End If

        End Function
        Public Function GetProductPriceForCodeQtyAndCust_Steripack(ByVal strProdCode As String, ByVal dblQtyOrWeight As Double, Optional ByVal strAltUM As String = "") As DataTable

            'SMcN This function get the appropriate price for a specified product code and quantity.
            ' This function looks up the customer for the relevant sales order ID

            Dim objProd As New VTDBFunctions.VTDBFunctions.ProductsFunctions
            Dim objG As New BPADotNetCommonFunctions.VT_ReportFunctions.GeneralFunctions
            Dim objTele As New VTDBFunctions.VTDBFunctions.TelesalesFunctions
            Dim objCus As New VTDBFunctions.VTDBFunctions.CustomerFunctions
            Dim objMatrix As New VT_CommonFunctions.MatrixFunctions
            Dim objVTM As New VTDBFunctions.VTDBFunctions.VTMatrixFunctions

            Dim dblPrice As Double
            Dim lngProductId As Long
            Dim lngCustomerId As Long
            Dim DatabaseTotalPrice As Double
            Dim dtOutPut As New DataTable
            Dim dsOrderDetails As DataSet

            'Build a blank output table
            dtOutPut.Columns.Add("ProductCode")
            dtOutPut.Columns.Add("ProductId")
            dtOutPut.Columns.Add("Customer")
            dtOutPut.Columns.Add("CustomerId")
            dtOutPut.Columns.Add("dblQtyOrWeight")
            dtOutPut.Columns.Add("DataBaseUnitPrice")
            dtOutPut.Columns.Add("DataBaseTotalPrice")
            dtOutPut.Columns.Add("QuoteReference")
            dtOutPut.Columns.Add("Comment")
            dtOutPut.Rows.Add()

            'Get the ProductId for this product code
            Dim strTemp As String = objProd.GetProductIdForCode(strProdCode)
            lngProductId = If(strTemp = "", 0, CInt(strTemp))


            'Fill output values
            dtOutPut.Rows(0).Item("ProductCode") = strProdCode
            dtOutPut.Rows(0).Item("ProductId") = lngProductId
            dtOutPut.Rows(0).Item("dblQtyOrWeight") = dblQtyOrWeight

            ' Get Order details so that we can find the customer ID
            dsOrderDetails = objTele.GetOrderForId(Me.CurrentSession.VT_SalesOrderID)

            If dsOrderDetails.Tables(0).Rows.Count > 0 Then

                lngCustomerId = dsOrderDetails.Tables(0).Rows(0).Item("CustomerID")

                dtOutPut.Rows(0).Item("Customer") = objCus.GetCustomerNameForId(lngCustomerId)
                dtOutPut.Rows(0).Item("CustomerId") = lngCustomerId

                Dim dtCust As DataSet = objCus.GetCustomerForId(lngCustomerId)

                Dim strCustCode As String
                If dtCust.Tables(0).Rows.Count > 0 Then
                    strCustCode = dtCust.Tables(0).Rows(0).Item("CustomerReference")

                End If

                'dblPrice = objTele.GetPriceForProductAndCustomer(lngProductId, lngCustomerId, , dblQtyOrWeight, False)
                Dim dtp As Data.DataTable
                Dim strsql As String
                Dim objMFG As New SteripackMFGProInterface.MFGProFunctions
                Dim strMFGProConnString As String = System.Configuration.ConfigurationManager.AppSettings("MFGPro_DB_CONN")
                Dim strMFGProDomain As String = System.Configuration.ConfigurationManager.AppSettings("MFGPro_Domain")
                Dim strMFGProError As String = ""
                Dim i As Integer

                If lngProductId > 0 And lngCustomerId > 0 Then
                    'Only lookup the price database if there is a valid Product code AND customer code
                    If UCase(strMFGProDomain) = "QAD" Then
                        'need to first check the pi_mstr table for a record that corresponds to the customer and part with the correct date range


                        'strsql = String.Format("SELECT  pc_curr, pc_start, pc_expire, pc_min_qty[01], pc_amt[01], pc_min_qty[02], pc_amt[02], pc_min_qty[03], pc_amt[03],pc_min_qty[04], pc_amt[04],pc_min_qty[05], pc_amt[05], pc_min_qty[06], pc_amt[06], pc_min_qty[07], pc_amt[07], pc_min_qty[08], pc_amt[08], pc_min_qty[09], pc_amt[09], pc_min_qty[10], pc_amt[10], pc_min_qty[11], pc_amt[11], pc_min_qty[12], pc_amt[12], pc_min_qty[13], pc_amt[13], pc_min_qty[14], pc_amt[14], pc_min_qty[15], pc_amt[15] FROM pc_mstr, cm_mstr WHERE cm_mstr.cm_pr_list = pc_mstr.pc_list, and cm_mstr.cm_addr =  = '" & strCustCode & "' and pc_part ='" & strProdCode & "'")
                        strsql = String.Format("SELECT pi_cs_code, pi_part_code, pi_start, pi_expire, pi_list_id, pi_list_price, pi_um FROM pub.pi_mstr WHERE pi_cs_code =  '" & Trim(strCustCode) & "' and pi_part_code ='" & Trim(strProdCode) & "' order by pi_start asc")
                        'this will return all prices including historical ones. we need to go through the datatable and make sure that todays date is in the date range for the record
                        'Session("MfgProLive") = "NO"


                        dtp = objMFG.GetDataTableFromMFGPro(strMFGProConnString, strsql, strMFGProError)


                    Else
                        dtp = objProd.GetPriceBreakDataForProductCustomerAndQty(CInt(lngProductId), CInt(lngCustomerId), CInt(dblQtyOrWeight))

                    End If

                    Dim j As Integer
                    Dim dt2 As New Data.DataTable
                    Dim dteExpireDate As Date
                    Dim dblConRate As Double
                    Dim objC As New BPADotNetCommonFunctions.VT_CommonFunctions.CommonFunctions

                    Dim intNumDecPlaces As Integer = objC.GetConfigItem("TelesalesDecimalPlaces")
                    If intNumDecPlaces = 0 Then
                        intNumDecPlaces = 4
                    End If

                    If dtp IsNot Nothing AndAlso dtp.Rows.Count > 0 Then
                        'do a live lookup of mfg pro for prices for Steripack Malaysia
                        If UCase(strMFGProDomain) = "QAD" Then

                            For i = 0 To dtp.Rows.Count - 1
                                'first find the record that applies to todays date
                                dteExpireDate = IIf(IsDBNull(dtp.Rows(i).Item("pi_expire")), PortalFunctions.Now.Date.AddYears(1), dtp.Rows(i).Item("pi_expire"))

                                If (dtp.Rows(i).Item("pi_start") <= PortalFunctions.Now.Date) And (dteExpireDate >= PortalFunctions.Now.Date) Then
                                    'this is the right row
                                    'get the price list id and check if there are records in that table
                                    'if there are records it means that there are price breaks, if there are no records then use the price in the pi_mstr table
                                    strsql = "Select * from pub.pid_det where pid_list_id='" & dtp.Rows(i).Item("pi_List_Id") & "' order by pid_qty asc"
                                    dt2 = objMFG.GetDataTableFromMFGPro(strMFGProConnString, strsql, strMFGProError)

                                    If dt2.Rows.Count > 0 Then
                                        'there are price breaks, find the right one
                                        'if the qty is less than the qty for the first price break, return -100 so the system knows that it is less than min qty
                                        Dim blnset As Boolean = True
                                        Dim dblHighestQty As Double
                                        Dim DblHighestPrice As Double

                                        For j = 0 To dt2.Rows.Count - 1
                                            If j = 0 Then
                                                dblHighestQty = dt2.Rows(j).Item("pid_qty")
                                                DblHighestPrice = dt2.Rows(j).Item("pid_amt")
                                            Else
                                                If dt2.Rows(j).Item("pid_qty") > dblHighestQty Then
                                                    dblHighestQty = dt2.Rows(j).Item("pid_qty")
                                                    DblHighestPrice = dt2.Rows(j).Item("pid_amt")
                                                End If
                                            End If

                                            If dt2.Rows(j).Item("pid_qty") > dblQtyOrWeight Then
                                                blnset = False

                                                If j = 0 Then
                                                    dblPrice = -100

                                                Else
                                                    dblPrice = dt2.Rows(j - 1).Item("pid_amt")
                                                    'if an alternative UM was passed in we need to get the conversion rate from MFG pro and recalc the price
                                                    If strAltUM <> "" Then
                                                        strsql = "Select * from pub.um_mstr where um_part='" & dtp.Rows(i).Item("pi_part_code") & "' and um_alt_um ='" & strAltUM & "' and um_um='" & dtp.Rows(i).Item("pi_um") & "'"
                                                        dt2 = objMFG.GetDataTableFromMFGPro(strMFGProConnString, strsql, strMFGProError)
                                                        If dt2.Rows.Count > 0 Then 'there is a conversion rate for this alt um
                                                            dblConRate = dt2.Rows(0).Item("um_conv")
                                                            dblPrice = dblPrice * dblConRate
                                                            dblPrice = FormatNumber(dblPrice, intNumDecPlaces)
                                                        Else
                                                            'the um may be in the alt um column. if that's the case get the conversion and divide instead of multiply
                                                            strsql = "Select * from pub.um_mstr where um_part='" & dtp.Rows(i).Item("pi_part_code") & "' and um_um ='" & strAltUM & "' and um_alt_um='" & dtp.Rows(i).Item("pi_um") & "'"
                                                            dt2 = objMFG.GetDataTableFromMFGPro(strMFGProConnString, strsql, strMFGProError)
                                                            If dt2.Rows.Count > 0 Then 'there is a conversion rate for this alt um
                                                                dblConRate = dt2.Rows(0).Item("um_conv")
                                                                If dblPrice > 0 Then
                                                                    dblPrice = dblPrice / dblConRate
                                                                    dblPrice = FormatNumber(dblPrice, intNumDecPlaces)
                                                                End If

                                                            End If
                                                        End If
                                                    End If
                                                    Exit For

                                                End If
                                            End If


                                        Next

                                        If blnset = True Then
                                            If dblQtyOrWeight >= dblHighestQty And dblHighestQty > 0 Then
                                                dblPrice = DblHighestPrice


                                                'if an alternative UM was passed in we need to get the conversion rate from MFG pro and recalc the price
                                                If strAltUM <> "" Then
                                                    strsql = "Select * from pub.um_mstr where um_part='" & strProdCode & "' and um_alt_um ='" & strAltUM & "' and um_um='" & dtp.Rows(i).Item("pi_um") & "'"
                                                    dt2 = objMFG.GetDataTableFromMFGPro(strMFGProConnString, strsql, strMFGProError)
                                                    If dt2.Rows.Count > 0 Then 'there is a conversion rate for this alt um
                                                        dblConRate = dt2.Rows(0).Item("um_conv")
                                                        dblPrice = dblPrice * dblConRate
                                                        dblPrice = FormatNumber(dblPrice, intNumDecPlaces)
                                                    Else
                                                        'the um may be in the alt um column. if that's the case get the conversion and divide instead of multiply
                                                        strsql = "Select * from pub.um_mstr where um_part='" & strProdCode & "' and um_um ='" & strAltUM & "' and um_alt_um='" & dtp.Rows(i).Item("pi_um") & "'"
                                                        dt2 = objMFG.GetDataTableFromMFGPro(strMFGProConnString, strsql, strMFGProError)
                                                        If dt2.Rows.Count > 0 Then 'there is a conversion rate for this alt um
                                                            dblConRate = dt2.Rows(0).Item("um_conv")
                                                            If dblPrice > 0 Then
                                                                dblPrice = dblPrice / dblConRate
                                                                dblPrice = FormatNumber(dblPrice, intNumDecPlaces)
                                                            End If

                                                        End If
                                                    End If
                                                End If
                                            End If
                                        End If
                                    Else
                                        'there are no price breaks, use the price in the pi_mstr table
                                        dblPrice = dtp.Rows(i).Item("pi_list_price")
                                        If strAltUM <> "" Then
                                            strsql = "Select * from pub.um_mstr where um_part='" & dtp.Rows(i).Item("pi_part_code") & "' and um_alt_um ='" & strAltUM & "' and um_um='" & dtp.Rows(i).Item("pi_um") & "'"
                                            dt2 = objMFG.GetDataTableFromMFGPro(strMFGProConnString, strsql, strMFGProError)
                                            If dt2.Rows.Count > 0 Then 'there is a conversion rate for this alt um
                                                dblConRate = dt2.Rows(0).Item("um_conv")
                                                dblPrice = dblPrice * dblConRate
                                                dblPrice = FormatNumber(dblPrice, intNumDecPlaces)
                                            Else
                                                'the um may be in the alt um column. if that's the case get the conversion and divide instead of multiply
                                                strsql = "Select * from pub.um_mstr where um_part='" & dtp.Rows(i).Item("pi_part_code") & "' and um_um ='" & strAltUM & "' and um_alt_um='" & dtp.Rows(i).Item("pi_um") & "'"
                                                dt2 = objMFG.GetDataTableFromMFGPro(strMFGProConnString, strsql, strMFGProError)
                                                If dt2.Rows.Count > 0 Then 'there is a conversion rate for this alt um
                                                    dblConRate = dt2.Rows(0).Item("um_conv")
                                                    If dblPrice > 0 Then
                                                        dblPrice = dblPrice / dblConRate
                                                        dblPrice = FormatNumber(dblPrice, intNumDecPlaces)
                                                    End If

                                                End If
                                            End If
                                        End If
                                    End If
                                End If


                            Next

                            '' ''For i = 0 To dtp.Rows.Count - 1
                            '' ''    'go through each row to find the right dates
                            '' ''    If (Trim(dtp.Rows(i).Item("pc_Start")) = "" Or dtp.Rows(i).Item("pc_start") <= Date.Today) And (Trim(dtp.Rows(i).Item("pc_expire")) = "" Or dtp.Rows(i).Item("pc_expire") >= Date.Today) Then
                            '' ''        'when we find the right row we have to get the correct qty, there are a maximum of 5 price breaks in MFG pro
                            '' ''        For j = 15 To 1 Step -1
                            '' ''            'start at the highest price break and work back
                            '' ''            If j >= 10 Then
                            '' ''                If dtp.Rows(i).Item("pc_min_qty[" & j.ToString & "]") <= dblQtyOrWeight Then
                            '' ''                    'get the related price for that qty
                            '' ''                    dblPrice = dtp.Rows(i).Item("pc_amt[" & j.ToString & "]")
                            '' ''                    dtOutPut.Rows(0).Item("Comment") = ""

                            '' ''                    dtOutPut.Rows(0).Item("QuoteReference") = ""

                            '' ''                End If
                            '' ''            Else
                            '' ''                If dtp.Rows(i).Item("pc_min_qty[0" & j.ToString & "]") <= dblQtyOrWeight Then
                            '' ''                    'get the related price for that qty
                            '' ''                    dblPrice = dtp.Rows(i).Item("pc_amt[0" & j.ToString & "]")
                            '' ''                    dtOutPut.Rows(0).Item("Comment") = ""

                            '' ''                    dtOutPut.Rows(0).Item("QuoteReference") = ""

                            '' ''                End If
                            '' ''            End If


                            '' ''        Next

                            '' ''    End If

                            '' ''Next

                        Else
                            dblPrice = dtp.Rows(0).Item("Special_Price")
                            dtOutPut.Rows(0).Item("Comment") = dtp.Rows(0).Item("Comment")
                            If dtp.Columns.Contains("QuoteReference") Then
                                dtOutPut.Rows(0).Item("QuoteReference") = dtp.Rows(0).Item("QuoteReference")
                            Else
                                dtOutPut.Rows(0).Item("QuoteReference") = ""
                            End If
                        End If

                    Else
                        'if this is steripack Q then we dont go here. They only have prices in MFG pro
                        If UCase(strMFGProDomain) <> "QAD" Then
                            dblPrice = 0
                            dtOutPut.Rows(0).Item("QuoteReference") = ""
                            dtOutPut.Rows(0).Item("Comment") = ""
                            'if this is not steripack then we want to also look for customer special prices or a regular list price
                            Dim objProducts As New VTDBFunctions.VTDBFunctions.ProductsFunctions
                            dblPrice = objProducts.GetProdPackPrice(lngProductId, lngCustomerId, "")
                            If dblPrice > 0 Then

                                dtOutPut.Rows(0).Item("DataBaseUnitPrice") = dblPrice
                                dtOutPut.Rows(0).Item("DataBaseTotalPrice") = Math.Round((dblPrice * dblQtyOrWeight), Me.CurrentSession.VT_DecimalPlaces)

                            End If
                            'AM 2014-05-29 no price is found in our price db so we need to look in the excel archive table
                            'if the price is found there then we import all prices for this product and customer, and mark them imported in the archive db
                            Dim objVTDB As New VTDBFunctions.VTDBFunctions.DBFunctions
                            Dim objVTProd As New VTDBFunctions.VTDBFunctions.ProductsFunctions
                            Dim objVTCust As New VTDBFunctions.VTDBFunctions.CustomerFunctions
                            Dim objVTPrice As New VTDBFunctions.VTDBFunctions.PricingFunctions

                            Dim strproductName = objVTProd.GetProductNameForId(lngProductId)
                            strCustCode = objVTCust.GetCustomerRefForId(lngCustomerId)

                            strsql = "Select * from prd_CustomerSpecialItemsArchive where ProductName = '" & strproductName & "' and CustomerCode = '" & strCustCode & "' and Uploaded Is Null"
                            Dim dtExcelArchive As Data.DataTable = objVTDB.ExecuteSQLReturnDT(strsql)

                            For i = 0 To dtExcelArchive.Rows.Count - 1
                                objVTPrice.InsertSpecialPrice_Steripack(lngProductId, lngCustomerId, dtExcelArchive.Rows(i).Item("Special_Price"), "Q", "", _
                                                                         dtExcelArchive.Rows(i).Item("Comment"), dtExcelArchive.Rows(i).Item("FromQty"), _
                                                                         dtExcelArchive.Rows(i).Item("QuoteReference"))
                                strsql = "Update prd_CustomerSpecialItemsArchive set Uploaded = 1 where RecordId = " & dtExcelArchive.Rows(i).Item("RecordId")
                                objVTDB.ExecuteSQLQuery(strsql)


                            Next

                            'run the lookup again, now it should find a price if one was found in the excel archive
                            dtp = objProd.GetPriceBreakDataForProductCustomerAndQty(CInt(lngProductId), CInt(lngCustomerId), CInt(dblQtyOrWeight))

                            If dtp IsNot Nothing AndAlso dtp.Rows.Count > 0 Then
                                dblPrice = dtp.Rows(0).Item("Special_Price")
                                dtOutPut.Rows(0).Item("Comment") = dtp.Rows(0).Item("Comment")
                                If dtp.Columns.Contains("QuoteReference") Then
                                    dtOutPut.Rows(0).Item("QuoteReference") = dtp.Rows(0).Item("QuoteReference")
                                Else
                                    dtOutPut.Rows(0).Item("QuoteReference") = ""
                                End If
                            End If

                        End If
                    End If

                Else
                    'Set values to 0 for this case
                    dblPrice = 0
                    dtOutPut.Rows(0).Item("QuoteReference") = ""
                    dtOutPut.Rows(0).Item("Comment") = ""
                End If


                DatabaseTotalPrice = Math.Round((dblQtyOrWeight * dblPrice), Me.CurrentSession.VT_DecimalPlaces)

                dtOutPut.Rows(0).Item("DataBaseUnitPrice") = dblPrice
                dtOutPut.Rows(0).Item("DataBaseTotalPrice") = DatabaseTotalPrice


            Else
                'No Order details found
                dtOutPut.Rows(0).Item("Customer") = ""
                dtOutPut.Rows(0).Item("CustomerId") = ""
                dtOutPut.Rows(0).Item("DataBaseUnitPrice") = "0"
                dtOutPut.Rows(0).Item("DataBaseTotalPrice") = "0"
                dtOutPut.Rows(0).Item("QuoteReference") = ""
                dtOutPut.Rows(0).Item("Comment") = ""



            End If

            GetProductPriceForCodeQtyAndCust_Steripack = dtOutPut

        End Function



        Public Function calculateCustomerDiscount(ByVal udtProduct As TProductRecord, ByVal udtCustomer As TCustomerRecord, Optional ByVal bApplyDiscountToBasePrice As Boolean = True) As String

            Dim objDataAccess As New VTDBFunctions.VTDBFunctions.ProductsFunctions

            Dim curSpecialPrice As Double
            Dim curTemp As Double
            ' Calculated according to the following formula
            ' Discount = (Base price * (Customer discount/100))
            If (Len(udtProduct.dblPrice) > 0) Then
                Dim curDiscount As Double

                curDiscount = (udtProduct.dblPrice * (CDbl(udtCustomer.dblDiscountPercent) / 100))

                curTemp = udtProduct.dblPrice - curDiscount
            End If

            ' some systems require "special" prices for some customers
            ' We use the tblSpecialItems table to hold these prices
            If udtCustomer.lngCustomerId > 0 And udtProduct.lngProductId > 0 Then

                ' if a special support DLL is defined for this installation then try to get a special price from it
                Dim strCustomerDLL As String = System.Configuration.ConfigurationSettings.AppSettings("InstallationDLL")
                If strCustomerDLL <> "" Then
                    Dim objCustDLL As TTIInstallationSpecific.Interface
                    Dim strReason As String
                    objCustDLL = CreateObject(strCustomerDLL)
                    ' If this function returns -2 it implies that there is no Special price for this customer
                    ' If this function returns -1 it implies that this Product is not for sale to this Customer
                    curTemp = objCustDLL.GetCustomerSpecialPrice(Session("_VT_DBConn"), udtProduct.lngProductId, udtCustomer.lngCustomerId, PortalFunctions.Now.Date)
                Else    ' use the standard special prices routine
                    If objDataAccess.GetCustomerSpecialPrice(udtProduct.lngProductId, udtCustomer.lngCustomerId) <> "" Then
                        curSpecialPrice = objDataAccess.GetCustomerSpecialPrice(udtProduct.lngProductId, udtCustomer.lngCustomerId)
                        If curSpecialPrice > 0 Then
                            curTemp = curSpecialPrice
                        End If
                    End If
                End If

            End If

            If curTemp < 0 Then ' revert to the standard price
                calculateCustomerDiscount = ""
            Else
                calculateCustomerDiscount = FormatCurrency(curTemp, 2)
            End If


        End Function

        Function GetUnitOfSale(ByVal lngProductId As Long, Optional ByVal lngCustomerId As Long = 0) As Integer
            Dim ds As New DataSet

            ' if a special support DLL is defined for this installation then try to get a special unit of sale
            Dim strCustomerDLL As String = System.Configuration.ConfigurationSettings.AppSettings("InstallationDLL")
            Dim strUOS As String = ""


            Dim strConnString As String = Session("_VT_DotNetConnString")


            If strCustomerDLL <> "" Then
                Dim objCustDLL As TTIInstallationSpecific.Interface
                objCustDLL = CreateObject(strCustomerDLL)
                strUOS = objCustDLL.GetCustomerChargedByType(strConnString, lngProductId, lngCustomerId, PortalFunctions.Now.Date)

            End If

            If strUOS = "" Or strUOS = "Use Default" Then
                ' use the standard Product Unit of Sale
                ds = GetProductForId(lngProductId)
                If ds.Tables(0).Rows.Count > 0 Then
                    GetUnitOfSale = IIf(IsDBNull(ds.Tables(0).Rows(0).Item("UnitOfSale")), 1, ds.Tables(0).Rows(0).Item("UnitOfSale"))
                Else
                    GetUnitOfSale = 1
                End If
            Else
                ' transform the UoS string into the number required by the rest of the code
                If strUOS = "Weight" Then
                    GetUnitOfSale = 0
                Else
                    GetUnitOfSale = 1
                End If
            End If

        End Function

        Sub SaveNewQtyInStock(ByVal lngproductId As Long, ByVal dblInStock As Double)
            Dim strConnString As String = Session("_VT_DotNetConnString")

            Dim myConnection As New SqlConnection(strConnString)

            Dim comSQL As New SqlCommand("prd_updInStockQty", myConnection)
            Dim myAdapter As New SqlDataAdapter(comSQL)
            Dim dt As New DataSet

            Dim objParam As SqlParameter
            objParam = comSQL.Parameters.Add("@ProductId", SqlDbType.Int)
            objParam.Direction = ParameterDirection.Input
            objParam.Value = lngproductId

            objParam = comSQL.Parameters.Add("@InStock", SqlDbType.Float)
            objParam.Direction = ParameterDirection.Input
            objParam.Value = dblInStock

            comSQL.CommandType = CommandType.StoredProcedure

            myConnection.Open()
            myAdapter.Fill(dt)

            myConnection.Close()
            myConnection = Nothing
        End Sub

        Function getTraceCodeDesc(ByVal lngTraceCode As Long) As String
            Dim strConnString As String = Session("_VT_DotNetConnString")
            Dim myConnection As New SqlConnection(strConnString)

            Dim comSQL As New SqlCommand("trc_resGetTraceCode", myConnection)
            Dim myAdapter As New SqlDataAdapter(comSQL)
            Dim ds As New DataSet

            Dim objParam As SqlParameter

            comSQL.CommandType = CommandType.StoredProcedure
            objParam = comSQL.Parameters.Add("@TraceCodeId", SqlDbType.Int)
            objParam.Direction = ParameterDirection.Input
            objParam.Value = lngTraceCode

            myConnection.Open()
            myAdapter.Fill(ds)

            If ds.Tables(0).Rows.Count > 0 Then
                getTraceCodeDesc = ds.Tables(0).Rows(0).Item("TraceCodeDesc")

            Else
                getTraceCodeDesc = ""
            End If

            myConnection.Close()
            myConnection = Nothing
        End Function
        Function GetTraceCodeforID(ByVal lngTraceCodeID As Long) As DataTable
            Dim strConnString As String = Session("_VT_DotNetConnString")
            Dim myConnection As New SqlConnection(strConnString)

            Dim comSQL As New SqlCommand("trc_resGetTraceCode", myConnection)
            Dim myAdapter As New SqlDataAdapter(comSQL)
            Dim ds As New DataSet

            Dim objParam As SqlParameter

            comSQL.CommandType = CommandType.StoredProcedure
            objParam = comSQL.Parameters.Add("@TraceCodeId", SqlDbType.Int)
            objParam.Direction = ParameterDirection.Input
            objParam.Value = lngTraceCodeID

            myConnection.Open()
            myAdapter.Fill(ds)

            GetTraceCodeforID = ds.Tables(0)

            myConnection.Close()
            myConnection = Nothing

        End Function
        Function GetTraceCodeForProductAndName(ByVal lngProductId As Long, ByVal strTraceCodeDesc As String) As Long

            Dim strConnString As String = Session("_VT_DotNetConnString")
            Dim myConnection As New SqlConnection(strConnString)

            Dim comSQL As New SqlCommand("prd_resGetTraceCodesForProd", myConnection)
            Dim myAdapter As New SqlDataAdapter(comSQL)
            Dim ds As New DataSet

            Dim objParam As SqlParameter

            comSQL.CommandType = CommandType.StoredProcedure
            objParam = comSQL.Parameters.Add("@ProductId", SqlDbType.Int)
            objParam.Direction = ParameterDirection.Input
            objParam.Value = lngProductId

            objParam = comSQL.Parameters.Add("@TraceCode", SqlDbType.NChar)
            objParam.Direction = ParameterDirection.Input
            objParam.Value = strTraceCodeDesc

            myConnection.Open()
            myAdapter.Fill(ds)

            If ds.Tables(0).Rows.Count > 0 Then
                GetTraceCodeForProductAndName = ds.Tables(0).Rows(0).Item("TraceCodeID")

            Else
                GetTraceCodeForProductAndName = 0
            End If

            myConnection.Close()
            myConnection = Nothing

        End Function

        Public Function UsesTraceCode(ByVal lngProductId As Long) As Boolean
            Dim dt As New DataTable
            Dim strsql As String
            Dim strconn As String

            strsql = "Select usesTraceCode from prd_ProductTable where ProductId = " & lngProductId

            strconn = Session("_VT_DotNetConnString")

            Dim myConnection As New SqlConnection(strconn)
            Dim comSQL As New SqlCommand(strsql, myConnection)

            Dim myAdapter As New SqlDataAdapter(comSQL)

            myConnection.Open()
            myAdapter.Fill(dt)

            If dt.Rows.Count > 0 Then
                UsesTraceCode = dt.Rows(0).Item(0)
            Else
                UsesTraceCode = True
            End If

        End Function
        Public Function GetUploadDll(ByRef objUploadDll As Object) As Boolean

            Dim strRetDllName As String

            strRetDllName = System.Configuration.ConfigurationSettings.AppSettings("UploadDllName")
            If strRetDllName <> "" Then

                objUploadDll = CreateObject(strRetDllName)
                If objUploadDll Is Nothing Then
                    'MsgBox("The upload dll name specified in the config file is: " & strRetDllName & ". This dll could not be created.", MsgBoxStyle.OkOnly, "Error")
                    GetUploadDll = False
                Else
                    GetUploadDll = True
                End If

            Else 'this customer doesn't use sage, no need for uploads
                GetUploadDll = False
            End If
        End Function

        Public Function GetTraceCodesForProduct(ByVal lngproductId As Long, ByVal strTraceCode As String) As DataTable
            Dim strConnString As String = Session("_VT_DotNetConnString")
            Dim myConnection As New SqlConnection(strConnString)

            Dim comSQL As New SqlCommand("prd_resGetTraceCodesForProd", myConnection)
            Dim myAdapter As New SqlDataAdapter(comSQL)
            Dim ds As New DataTable

            Dim objParam As SqlParameter

            comSQL.CommandType = CommandType.StoredProcedure
            objParam = comSQL.Parameters.Add("@ProductId", SqlDbType.Int)
            objParam.Direction = ParameterDirection.Input
            objParam.Value = lngproductId

            comSQL.CommandType = CommandType.StoredProcedure
            objParam = comSQL.Parameters.Add("@TraceCode", SqlDbType.NChar)
            objParam.Direction = ParameterDirection.Input
            objParam.Value = strTraceCode

            myConnection.Open()
            myAdapter.Fill(ds)

            GetTraceCodesForProduct = ds
            myConnection.Close()
            myConnection = Nothing
        End Function


        Public Function GetTraceCodesLocationData(ByVal lngProductID As Long) As Data.DataSet
            Dim strConnString As String = Session("_VT_DotNetConnString")
            Dim myConnection As New SqlConnection(strConnString)

            Dim comSQL As New SqlCommand("trc_resGetLocationDataForProduct", myConnection)
            Dim myAdapter As New SqlDataAdapter(comSQL)
            Dim ds As New DataSet

            Dim objParam As SqlParameter

            comSQL.CommandType = CommandType.StoredProcedure
            objParam = comSQL.Parameters.Add("@ProductId", SqlDbType.NVarChar)
            objParam.Direction = ParameterDirection.Input
            objParam.Value = lngProductID

            comSQL.CommandType = CommandType.StoredProcedure
            objParam = comSQL.Parameters.Add("@LocationId", SqlDbType.NVarChar)
            objParam.Direction = ParameterDirection.Input
            objParam.Value = 0


            myConnection.Open()
            myAdapter.Fill(ds)

            GetTraceCodesLocationData = ds

            myConnection.Close()
            myConnection = Nothing
        End Function

        Public Function GetSerialNumberDataForProduct(ByVal lngProductID As Long) As Data.DataSet
            Dim strConnString As String = Session("_VT_DotNetConnString")
            Dim myConnection As New SqlConnection(strConnString)

            Dim comSQL As New SqlCommand("trc_resSerialNumsForProduct", myConnection)
            Dim myAdapter As New SqlDataAdapter(comSQL)
            Dim ds As New DataSet

            Dim objParam As SqlParameter

            comSQL.CommandType = CommandType.StoredProcedure
            objParam = comSQL.Parameters.Add("@ProductId", SqlDbType.NVarChar)
            objParam.Direction = ParameterDirection.Input
            objParam.Value = lngProductID


            myConnection.Open()
            myAdapter.Fill(ds)

            GetSerialNumberDataForProduct = ds

            myConnection.Close()
            myConnection = Nothing
        End Function





        Sub AddComponentForJob(ByVal lngJobId As Long, ByVal intComponentType As Integer, ByVal lngitem As Long, ByVal intDocType As Integer, ByVal strDescription As String)

            Dim strConnString As String = Session("_VT_DotNetConnString")
            Dim myConnection As New SqlConnection(strConnString)

            Dim comSQL As New SqlCommand("prj_insComponentForJob", myConnection)
            Dim myAdapter As New SqlDataAdapter(comSQL)
            Dim ds As New DataSet

            Dim objParam As SqlParameter

            comSQL.CommandType = CommandType.StoredProcedure
            objParam = comSQL.Parameters.Add("@JobId", SqlDbType.Int)
            objParam.Direction = ParameterDirection.Input
            objParam.Value = lngJobId

            objParam = comSQL.Parameters.Add("@TypeId", SqlDbType.Int)
            objParam.Direction = ParameterDirection.Input
            objParam.Value = intComponentType

            objParam = comSQL.Parameters.Add("@LinkItemId", SqlDbType.Int)
            objParam.Direction = ParameterDirection.Input
            objParam.Value = lngitem

            objParam = comSQL.Parameters.Add("@DocType", SqlDbType.Int)
            objParam.Direction = ParameterDirection.Input
            objParam.Value = intDocType

            objParam = comSQL.Parameters.Add("@Description", SqlDbType.NChar)
            objParam.Direction = ParameterDirection.Input
            objParam.Value = strDescription

            myConnection.Open()
            myAdapter.Fill(ds)


            myConnection.Close()
            myConnection = Nothing
        End Sub
        Function getProductIdForTraceCode(ByVal lngTraceCode As Long) As String
            Dim strConnString As String = Session("_VT_DotNetConnString")
            Dim myConnection As New SqlConnection(strConnString)

            Dim comSQL As New SqlCommand("trc_resGetTraceCode", myConnection)
            Dim myAdapter As New SqlDataAdapter(comSQL)
            Dim ds As New DataSet

            Dim objParam As SqlParameter

            comSQL.CommandType = CommandType.StoredProcedure
            objParam = comSQL.Parameters.Add("@TraceCodeId", SqlDbType.Int)
            objParam.Direction = ParameterDirection.Input
            objParam.Value = lngTraceCode

            myConnection.Open()
            myAdapter.Fill(ds)

            If ds.Tables(0).Rows.Count > 0 Then
                getProductIdForTraceCode = ds.Tables(0).Rows(0).Item("ProductId")

            Else
                getProductIdForTraceCode = 0
            End If

            myConnection.Close()
            myConnection = Nothing
        End Function
        Function GetProductNameForCode(ByVal strProductCode As String) As String
            Dim dt As New Data.DataTable
            Dim strsql As String
            Dim strconn As String


            strsql = "select Product_Name from prd_ProductTable where Catalog_Number = '" & strProductCode & "'"

            strconn = Session("_VT_DotNetConnString")

            Dim myConnection As New SqlConnection(strconn)
            Dim comSQL As New SqlCommand(strsql, myConnection)

            Dim myAdapter As New SqlDataAdapter(comSQL)

            myConnection.Open()
            myAdapter.Fill(dt)

            If dt.Rows.Count > 0 Then
                GetProductNameForCode = dt.Rows(0).Item(0)
            Else
                GetProductNameForCode = ""
            End If

            myConnection.Close()
            myConnection = Nothing
        End Function
        Function GetProductIDForCode(ByVal strProductCode As String) As Long
            Dim dt As New Data.DataTable
            Dim strsql As String
            Dim strconn As String


            strsql = "select ProductID from prd_ProductTable where Catalog_Number = '" & strProductCode & "'"

            strconn = Session("_VT_DotNetConnString")

            Dim myConnection As New SqlConnection(strconn)
            Dim comSQL As New SqlCommand(strsql, myConnection)

            Dim myAdapter As New SqlDataAdapter(comSQL)

            myConnection.Open()
            myAdapter.Fill(dt)

            If dt.Rows.Count > 0 Then
                GetProductIDForCode = dt.Rows(0).Item(0)
            Else
                GetProductIDForCode = 0
            End If

            myConnection.Close()
            myConnection = Nothing
        End Function

        Function GetProductCode(ByVal ProductId As Long) As String
            Dim ds As DataSet

            ds = GetProductForId(ProductId)

            If ds.Tables(0).Rows.Count > 0 Then
                GetProductCode = ds.Tables(0).Rows(0).Item("Catalog_Number")
            Else
                GetProductCode = ""
            End If
        End Function
        Function GetProductName(ByVal ProductId As Long) As String
            Dim ds As DataSet

            ds = GetProductForId(ProductId)

            If ds.Tables(0).Rows.Count > 0 Then
                GetProductName = ds.Tables(0).Rows(0).Item("Product_Name")
            Else
                GetProductName = ""
            End If
        End Function

        Function ProductIsAService(ByVal lngProductId As Long) As Boolean

            Dim ds As New DataSet
            ds = GetProductForId(lngProductId)
            Dim strServiceItemCategory As String
            Dim objcommon As New VT_CommonFunctions.CommonFunctions
            Dim strCategory As String
            Dim objDB As New VTDBFunctions.VTDBFunctions.ProductsFunctions
            Dim DSProductCategory As New DataTable

            strServiceItemCategory = objcommon.GetConfigItem("ServiceItemCategory")


            If ds.Tables(0).Rows.Count > 0 Then
                DSProductCategory = objDB.GetProductLineForId(ds.Tables(0).Rows(0).Item("Product_LineID"))
                If DSProductCategory.Rows.Count > 0 Then
                    If UCase(Trim(DSProductCategory.Rows(0).Item("Product_Line_Text"))) = UCase(Trim(strServiceItemCategory)) Then
                        ProductIsAService = True
                    Else
                        ProductIsAService = False

                    End If
                Else
                    ProductIsAService = False
                End If

            Else
                ProductIsAService = False
            End If
        End Function
        Function GetProductForId(ByVal intProductId As Long) As DataSet

            Dim strConnString As String = Session("_VT_DotNetConnString")
            Dim myConnection As New SqlConnection(strConnString)

            Dim comSQL As New SqlCommand("prd_resGetProductForID", myConnection)
            Dim myAdapter As New SqlDataAdapter(comSQL)
            Dim ds As New DataSet

            Dim objParam As SqlParameter

            comSQL.CommandType = CommandType.StoredProcedure
            objParam = comSQL.Parameters.Add("@ProductId", SqlDbType.Int)
            objParam.Direction = ParameterDirection.Input
            objParam.Value = intProductId

            myConnection.Open()
            myAdapter.Fill(ds)

            GetProductForId = ds
            myConnection.Close()
            myConnection = Nothing
        End Function

        Public Function GetProductInStockQty(ByVal lngproductId As Long) As Double
            Dim dt As New Data.DataTable
            Dim strsql As String
            Dim strconn As String


            strsql = "select InStock from prd_ProductTable where ProductId = " & lngproductId

            strconn = Session("_VT_DotNetConnString")

            Dim myConnection As New SqlConnection(strconn)
            Dim comSQL As New SqlCommand(strsql, myConnection)

            Dim myAdapter As New SqlDataAdapter(comSQL)

            myConnection.Open()
            myAdapter.Fill(dt)

            If dt.Rows.Count > 0 Then
                GetProductInStockQty = dt.Rows(0).Item(0)
            Else
                GetProductInStockQty = 0
            End If

            myConnection.Close()
            myConnection = Nothing
        End Function

        Public Function GetProductUnitCostByBatchID(ByVal BatchID As Long) As Double

            Dim objTraceData As New VTDBFunctions.VTDBFunctions.ProductsFunctions
            Dim lngProductID As Long

            lngProductID = objTraceData.GetProductIdForTraceCode(BatchID)

            Dim myConnection As New SqlConnection(Session("_VT_DotNetConnString"))
            Dim comSQL As New SqlCommand("prd_resGetProductForID", myConnection)
            Dim adpSQL As New SqlDataAdapter(comSQL)
            Dim dsadded As New Data.DataSet
            Dim dblProductCost As Double

            Dim objParam As SqlParameter

            comSQL.CommandType = Data.CommandType.StoredProcedure
            objParam = comSQL.Parameters.Add("@ProductID", Data.SqlDbType.Int)
            objParam.Direction = Data.ParameterDirection.Input
            objParam.Value = lngProductID


            myConnection.Open()
            adpSQL.Fill(dsadded)


            '  Close the connection when done with it.
            myConnection.Close()

            dblProductCost = 0

            dblProductCost = dsadded.Tables(0).Rows(0).Item("CostPrice")

            GetProductUnitCostByBatchID = dblProductCost

        End Function


        Public Function GetCurrentQtyWgtForSerialNum(ByVal strSerialNum As String, ByVal lngTracecode As Long, ByVal lngLocation As Long, ByVal intunitOfSale As Integer) As Double


            Dim dsQtyIn As New DataSet
            Dim dsQtyOut As New DataSet
            Dim dblQtyIn As Double
            Dim dblWgtIn As Double
            Dim dblCurrentQty As Double
            Dim dblCurrentwgt As Double
            Dim lngProduct As Long
            Dim dsStockTake As New DataSet

            Dim strConnString As String = Session("_VT_DotNetConnString")
            Dim myConnection As New SqlConnection(strConnString)

            Dim comSQL As New SqlCommand("trc_resStockTakeTxnForSerialNum", myConnection)
            Dim myAdapter As New SqlDataAdapter(comSQL)
            '   Dim ds As New DataSet

            Dim objParam As SqlParameter

            comSQL.CommandType = CommandType.StoredProcedure
            objParam = comSQL.Parameters.Add("@TraceCodeId", SqlDbType.Int)
            objParam.Direction = ParameterDirection.Input
            objParam.Value = lngTracecode

            comSQL.CommandType = CommandType.StoredProcedure
            objParam = comSQL.Parameters.Add("@LocationId", SqlDbType.Int)
            objParam.Direction = ParameterDirection.Input
            objParam.Value = lngLocation

            comSQL.CommandType = CommandType.StoredProcedure
            objParam = comSQL.Parameters.Add("@SerialNum", SqlDbType.NVarChar)
            objParam.Direction = ParameterDirection.Input
            objParam.Value = strSerialNum

            myConnection.Open()
            myAdapter.Fill(dsStockTake)

            myConnection.Close()

            If dsStockTake.Tables(0).Rows.Count > 0 Then

                lngProduct = getProductIdForTraceCode(lngTracecode)

                Dim comSQLQtyIn As New SqlCommand("trc_resQtyInForSerialNumWDate", myConnection)
                Dim myAdapterQtyIn As New SqlDataAdapter(comSQLQtyIn)
                Dim objParamQtyIn As SqlParameter

                comSQLQtyIn.CommandType = CommandType.StoredProcedure
                objParamQtyIn = comSQLQtyIn.Parameters.Add("@SerialNum", SqlDbType.NVarChar)
                objParamQtyIn.Direction = ParameterDirection.Input
                objParamQtyIn.Value = strSerialNum

                comSQLQtyIn.CommandType = CommandType.StoredProcedure
                objParamQtyIn = comSQLQtyIn.Parameters.Add("@TraceCodeId", SqlDbType.Int)
                objParamQtyIn.Direction = ParameterDirection.Input
                objParamQtyIn.Value = lngTracecode

                comSQLQtyIn.CommandType = CommandType.StoredProcedure
                objParamQtyIn = comSQLQtyIn.Parameters.Add("@LocationId", SqlDbType.Int)
                objParamQtyIn.Direction = ParameterDirection.Input
                objParamQtyIn.Value = lngLocation

                comSQLQtyIn.CommandType = CommandType.StoredProcedure
                objParamQtyIn = comSQLQtyIn.Parameters.Add("@DateOfTransaction", SqlDbType.DateTime)
                objParamQtyIn.Direction = ParameterDirection.Input
                objParamQtyIn.Value = dsStockTake.Tables(0).Rows(0).Item("DateOfTransaction")

                myConnection.Open()
                myAdapterQtyIn.Fill(dsQtyIn)

                myConnection.Close()

                If dsQtyIn.Tables(0).Rows.Count > 0 Then
                    dblQtyIn = IIf(IsDBNull(dsQtyIn.Tables(0).Rows(0).Item("Qty")), 0, dsQtyIn.Tables(0).Rows(0).Item("Qty"))
                    dblWgtIn = IIf(IsDBNull(dsQtyIn.Tables(0).Rows(0).Item("Weight")), 0, dsQtyIn.Tables(0).Rows(0).Item("Weight"))
                Else
                    dblQtyIn = 0
                    dblWgtIn = 0
                End If


                Dim comSQLQtyOut As New SqlCommand("trc_resSerialNumQtysOutWDate", myConnection)
                Dim myAdapterQtyOut As New SqlDataAdapter(comSQLQtyOut)
                Dim objParamQtyOut As SqlParameter

                comSQLQtyOut.CommandType = CommandType.StoredProcedure
                objParamQtyOut = comSQLQtyOut.Parameters.Add("@SerialNum", SqlDbType.NVarChar)
                objParamQtyOut.Direction = ParameterDirection.Input
                objParamQtyOut.Value = strSerialNum

                comSQLQtyOut.CommandType = CommandType.StoredProcedure
                objParamQtyOut = comSQLQtyOut.Parameters.Add("@TraceCodeId", SqlDbType.Int)
                objParamQtyOut.Direction = ParameterDirection.Input
                objParamQtyOut.Value = lngTracecode

                comSQLQtyOut.CommandType = CommandType.StoredProcedure
                objParamQtyOut = comSQLQtyOut.Parameters.Add("@LocationId", SqlDbType.Int)
                objParamQtyOut.Direction = ParameterDirection.Input
                objParamQtyOut.Value = lngLocation

                comSQLQtyOut.CommandType = CommandType.StoredProcedure
                objParamQtyOut = comSQLQtyOut.Parameters.Add("@DateOfTransaction", SqlDbType.DateTime)
                objParamQtyOut.Direction = ParameterDirection.Input
                objParamQtyOut.Value = dsStockTake.Tables(0).Rows(0).Item("DateOfTransaction")

                myConnection.Open()
                myAdapterQtyOut.Fill(dsQtyOut)

                myConnection.Close()

                If dsQtyOut.Tables(0).Rows.Count > 0 Then

                    dblCurrentQty = dblQtyIn - dsQtyOut.Tables(0).Rows(0).Item("Qty")
                    dblCurrentwgt = dblWgtIn - dsQtyOut.Tables(0).Rows(0).Item("Wgt")
                Else
                    dblCurrentQty = dblQtyIn
                    dblCurrentwgt = dblWgtIn
                End If

            Else

                lngProduct = getProductIdForTraceCode(lngTracecode)

                Dim comSQLQtyIn As New SqlCommand("trc_resQtyInForSerialNum", myConnection)
                Dim myAdapterQtyIn As New SqlDataAdapter(comSQLQtyIn)
                Dim objParamQtyIn As SqlParameter

                comSQLQtyIn.CommandType = CommandType.StoredProcedure
                objParamQtyIn = comSQLQtyIn.Parameters.Add("@SerialNum", SqlDbType.NVarChar)
                objParamQtyIn.Direction = ParameterDirection.Input
                objParamQtyIn.Value = strSerialNum

                comSQLQtyIn.CommandType = CommandType.StoredProcedure
                objParamQtyIn = comSQLQtyIn.Parameters.Add("@TraceCodeId", SqlDbType.Int)
                objParamQtyIn.Direction = ParameterDirection.Input
                objParamQtyIn.Value = lngTracecode

                comSQLQtyIn.CommandType = CommandType.StoredProcedure
                objParamQtyIn = comSQLQtyIn.Parameters.Add("@LocationId", SqlDbType.Int)
                objParamQtyIn.Direction = ParameterDirection.Input
                objParamQtyIn.Value = lngLocation

                myConnection.Open()
                myAdapterQtyIn.Fill(dsQtyIn)

                myConnection.Close()

                If dsQtyIn.Tables(0).Rows.Count > 0 Then
                    dblQtyIn = IIf(IsDBNull(dsQtyIn.Tables(0).Rows(0).Item("Qty")), 0, dsQtyIn.Tables(0).Rows(0).Item("Qty"))
                    dblWgtIn = IIf(IsDBNull(dsQtyIn.Tables(0).Rows(0).Item("Weight")), 0, dsQtyIn.Tables(0).Rows(0).Item("Weight"))
                Else
                    dblQtyIn = 0
                    dblWgtIn = 0
                End If

                Dim comSQLQtyOut As New SqlCommand("trc_resSerialNumQtysOut", myConnection)
                Dim myAdapterQtyOut As New SqlDataAdapter(comSQLQtyOut)
                Dim objParamQtyOut As SqlParameter

                comSQLQtyOut.CommandType = CommandType.StoredProcedure
                objParamQtyOut = comSQLQtyOut.Parameters.Add("@SerialNum", SqlDbType.NVarChar)
                objParamQtyOut.Direction = ParameterDirection.Input
                objParamQtyOut.Value = strSerialNum

                comSQLQtyOut.CommandType = CommandType.StoredProcedure
                objParamQtyOut = comSQLQtyOut.Parameters.Add("@TraceCodeId", SqlDbType.Int)
                objParamQtyOut.Direction = ParameterDirection.Input
                objParamQtyOut.Value = lngTracecode

                comSQLQtyOut.CommandType = CommandType.StoredProcedure
                objParamQtyOut = comSQLQtyOut.Parameters.Add("@LocationId", SqlDbType.Int)
                objParamQtyOut.Direction = ParameterDirection.Input
                objParamQtyOut.Value = lngLocation


                myConnection.Open()
                myAdapterQtyOut.Fill(dsQtyOut)

                myConnection.Close()

                If dsQtyOut.Tables(0).Rows.Count > 0 Then
                    dblCurrentQty = dblQtyIn - dsQtyOut.Tables(0).Rows(0).Item("Qty")
                    dblCurrentwgt = dblWgtIn - dsQtyOut.Tables(0).Rows(0).Item("Wgt")
                Else
                    dblCurrentQty = dblQtyIn
                    dblCurrentwgt = dblWgtIn
                End If
            End If
            If intunitOfSale = 1 Then
                GetCurrentQtyWgtForSerialNum = dblCurrentQty
            Else
                GetCurrentQtyWgtForSerialNum = dblCurrentwgt
            End If


            myConnection.Close()
            myConnection = Nothing


        End Function


        Public Function GetPackageTypes(Optional ByVal PackageTypeId As Long = 0) As DataTable
            Dim strConnString As String = Session("_VT_DotNetConnString")
            Dim myConnection As New SqlConnection(strConnString)

            Dim comSQL As New SqlCommand("IBS_resGetPackageTypes", myConnection)
            Dim myAdapter As New SqlDataAdapter(comSQL)
            Dim ds As New DataTable

            Dim objParam As SqlParameter

            Try
                comSQL.CommandType = CommandType.StoredProcedure
                objParam = comSQL.Parameters.Add("@PackageTypeId", SqlDbType.Int)
                objParam.Direction = ParameterDirection.Input
                objParam.Value = PackageTypeId


                myConnection.Open()
                myAdapter.Fill(ds)

                GetPackageTypes = ds


            Catch ex As Exception

                Dim f As New UtilFunctions
                f.LogAction("Error in App_Code:DBFunctions:Products:GetPackageTypes: " + ex.Message)
            Finally

                myConnection.Close()
                myConnection = Nothing
            End Try


        End Function


        ''' <summary>
        ''' Retrieves an item from the ProductSpecification folder for a product
        ''' </summary>
        ''' <param name="intProdId">The Id of the version of the product for which we want to retrieve the spec item</param>
        ''' <param name="strItemName">the name of the Spec item</param>
        ''' <returns>String</returns>
        ''' <remarks></remarks>
        Public Function GetSpecItemForProduct(ByVal intProdId As Integer, ByVal strItemName As String) As String
            Dim objMatrix As New VTDBFunctions.VTDBFunctions.VTMatrixFunctions
            GetSpecItemForProduct = objMatrix.GetVTMatrixDataItem(Session("_VT_DotNetConnString"), VT_CommonFunctions.MatrixFunctions.MaterialsFormListTable, VT_CommonFunctions.MatrixFunctions.MaterialsFormDataTable, intProdId, "ProductSpecification", strItemName)

        End Function



        Public Function GetProductClassificationDataForID(ByVal intProductId As Integer) As DataTable

            Dim objMatrix As New VTDBFunctions.VTDBFunctions.VTMatrixFunctions
            Dim objVTProducts As New VTDBFunctions.VTDBFunctions.ProductsFunctions
            Dim dsProd As New DataSet
            Dim intProdMatrix As Integer

            dsProd = objVTProducts.GetProductForId(intProductId)
            If dsProd.Tables(0).Rows.Count > 0 Then
                intProdMatrix = dsProd.Tables(0).Rows(0).Item("MatrixId")
            End If

            ' get the Materials items fom the grid
            Dim intProdNumFields As Integer = 8
            Dim astrProdItems(intProdNumFields - 1) As String
            Dim dsProdClassificationItems As Data.DataSet

            astrProdItems(0) = "txtPhysicalForm"
            astrProdItems(1) = "txtEWCCode"
            astrProdItems(2) = "txtHazard"
            astrProdItems(3) = "txtUNNumber"
            astrProdItems(4) = "txtShippingName"
            astrProdItems(5) = "txtPrimaryHazard"
            astrProdItems(6) = "txtSecondaryHazard"
            astrProdItems(7) = "ddlPackagingGroup"

            dsProdClassificationItems = objMatrix.GetVTMatrixDataComplex(Session("_VT_DotNetConnString"), VT_CommonFunctions.MatrixFunctions.MaterialsFormDataTable, intProdNumFields, _
                                                                  astrProdItems, "FormType,ItemId", "Specification," + CStr(intProdMatrix))

            GetProductClassificationDataForID = dsProdClassificationItems.Tables(0)

        End Function



    End Class
End Namespace

Namespace ProductionFunctions
    Public Class Production
        Inherits MyBasePage
        Public Function getSkidNumberParentFolders(ByVal strSkidNumber As String) As Data.DataTable
            Dim objMatrix As New VTDBFunctions.VTDBFunctions.VTMatrixFunctions
            Dim objG As New BPADotNetCommonFunctions.VT_ReportFunctions.GeneralFunctions
            Dim dt As New Data.DataTable
            Dim strsql As String
            Dim strconn As String

            Dim objCommonFuncs As New BPADotNetCommonFunctions.VT_CommonFunctions.CommonFunctions
            Dim strProductionJobId As String = objCommonFuncs.GetConfigItem("ProductionJobID")

            strsql = "Select FormId from pdn_FormList where FormName = 'Scanned Serial Numbers For Step'"

            strconn = Session("_VT_DotNetConnString")

            Dim myConnection As New SqlConnection(strconn)
            Dim comSQL As New SqlCommand(strsql, myConnection)
            Dim strSearch As String
            Dim myAdapter As New SqlDataAdapter(comSQL)

            myConnection.Open()
            myAdapter.Fill(dt)

            'Now get all the serial numbers and formId's for serial numbers
            Dim strKeyfield As String = "FormId"
            Dim dtSkidInfo As DataTable = objMatrix.GetMatrixUnNamedChildIDs(strconn, "Production", dt, strKeyfield)
            'Now get the information stored under the Serial Number Nodes
            Dim astrItems(5) As String
            astrItems(0) = "TransactionId"  'this is the Role ID
            astrItems(1) = "SkidNumber"
            astrItems(2) = "JulianDate"
            astrItems(3) = "ProductionJobId"
            astrItems(4) = "QAInitials"
            astrItems(5) = "QADate"

            strKeyfield = "FormId"
            Dim strErr As String = objMatrix.GetBlockOfDataItemsNON_FORM(strconn, "Production", astrItems, dtSkidInfo, strKeyfield)

            'FIlter on required skid number
            strSearch = "SkidNumber = " + strSkidNumber + " AND TransactionID is not NULL"
            dtSkidInfo = objG.SearchDataTable(strSearch, dtSkidInfo)

            getSkidNumberParentFolders = dtSkidInfo



        End Function
        Public Function getIngredientsForAllOpenWorkOrders(ByVal dteStartDate As Date, ByVal dteEndDate As Date) As Data.DataTable
            Dim dt As New Data.DataTable
            Dim strsql As String
            Dim strconn As String

            Dim strstartDate As String = dteStartDate.ToString("yyyy-MM-dd") '"2008-06-15" 'Date.Today.ToString("yyyy-MM-dd")
            Dim strEndDate As String = dteEndDate.ToString("yyyy-MM-dd") ' "2008-06-18" 'Date.Today.AddDays(1).ToString("yyyy-MM-dd")

            'strstartDate = Me.CurrentSession.VT_PlanningStartDate
            'strEndDate = Me.CurrentSession.VT_PlanningEndDate

            '      strstartDate = "2008-08-21" 'Date.Today.ToString("yyyy-MM-dd")
            '     strEndDate = "2008-08-23" 'Date.Today.AddDays(1).ToString("yyyy-MM-dd")

            Dim strProductionJobId As String = ConfigurationManager.AppSettings("ProductionJobId")

            strsql = "Select pdn_ingredients.ingredientProductId, min(prd_ProductTable.Product_Name) as ProductName, " & _
            "sum(pdn_ingredients.Quantity) as TotalQty," & _
            "sum(pdn_ingredients.Weight)as TotalWgt, " & _
            "sum(prd_ProductTable.InStock) as InStock, " & _
            "min(prd_ProductTable.Catalog_Number) as Catalog_Number " & _
            "from pdn_ingredients inner join wfo_batchtable on pdn_ingredients.jobid = wfo_batchtable.jobid " & _
            "inner join prd_ProductTable on pdn_ingredients.ingredientProductId = prd_ProductTable.ProductId " & _
            "where(wfo_batchtable.programid = " + strProductionJobId + ") and JobStatusText <> 'Finished' " & _
            "and  (wfo_batchtable.dateduetostart >= '" & strstartDate & "' and wfo_batchtable.dateduetostart < '" & strEndDate & "') " & _
            "group by pdn_ingredients.ingredientProductId"

            strconn = Session("_VT_DotNetConnString")

            Dim myConnection As New SqlConnection(strconn)
            Dim comSQL As New SqlCommand(strsql, myConnection)

            Dim myAdapter As New SqlDataAdapter(comSQL)

            myConnection.Open()
            myAdapter.Fill(dt)

            getIngredientsForAllOpenWorkOrders = dt

        End Function

        Public Function DoesBatchAlreadyExist(ByVal strBatchName As String, ByVal lngProductId As Long) As Boolean
            Dim myConnection As New SqlConnection(Session("_VT_DotNetConnString"))
            Dim comSQL As New SqlCommand("wfo_resGetBatchForName", myConnection)
            Dim adpSQL As New SqlDataAdapter(comSQL)
            Dim dsadded As New DataSet

            Dim objParam As SqlParameter

            comSQL.CommandType = CommandType.StoredProcedure
            objParam = comSQL.Parameters.Add("@JobIdDesc", SqlDbType.NChar)
            objParam.Direction = ParameterDirection.Input
            objParam.Value = strBatchName

            myConnection.Open()
            adpSQL.Fill(dsadded)
            If dsadded.Tables(0).Rows.Count > 0 Then
                For i = 0 To dsadded.Tables(0).Rows.Count - 1
                    If dsadded.Tables(0).Rows(i).Item("ProductId") = lngProductId Then
                        DoesBatchAlreadyExist = True
                        Exit Function
                    End If
                Next

            Else
                DoesBatchAlreadyExist = False
                '  Close the connection when done with it.
            End If

            myConnection.Close()

        End Function
        Sub SaveJobComment(ByVal lngJobId As Long, ByVal strComment As String)

            Dim myConnection As New SqlConnection(Session("_VT_DotNetConnString"))
            Dim comSQL As New SqlCommand("pdn_updJobComment", myConnection)
            Dim adpSQL As New SqlDataAdapter(comSQL)
            Dim dsadded As New DataSet

            Dim objParam As SqlParameter

            comSQL.CommandType = CommandType.StoredProcedure
            objParam = comSQL.Parameters.Add("@JobId", SqlDbType.Int)
            objParam.Direction = ParameterDirection.Input
            objParam.Value = lngJobId

            objParam = comSQL.Parameters.Add("@Comment", SqlDbType.NChar)
            objParam.Direction = ParameterDirection.Input
            objParam.Value = strComment

            myConnection.Open()
            adpSQL.Fill(dsadded)


            myConnection.Close()

        End Sub

        Sub SaveJobSerialID(ByVal lngJobId As Long, ByVal lngSerialID As Long)

            '  Dim lngSerialID As Long



            'save the serial ID for the new job. It will be needed later for the logTestExecution

            Dim myConnection As New SqlConnection(Session("_VT_DotNetConnString"))
            Dim comSQL As New SqlCommand("prj_updSerialId", myConnection)
            Dim adpSQL As New SqlDataAdapter(comSQL)
            Dim dsadded As New DataSet

            Dim objParam As SqlParameter

            comSQL.CommandType = CommandType.StoredProcedure
            objParam = comSQL.Parameters.Add("@JobId", SqlDbType.Int)
            objParam.Direction = ParameterDirection.Input
            objParam.Value = lngJobId

            objParam = comSQL.Parameters.Add("@SerialId", SqlDbType.Int)
            objParam.Direction = ParameterDirection.Input
            objParam.Value = lngSerialID

            myConnection.Open()
            adpSQL.Fill(dsadded)


            myConnection.Close()

        End Sub

        Sub UpdateJobSpecifics(ByVal lngJobId As Long, ByVal dblQtyToBeProduced As Double, ByVal strOrderNum As String, ByVal strProductionLine As String)

            Dim myConnection As New SqlConnection(Session("_VT_DotNetConnString"))
            Dim comSQL As New SqlCommand("prj_insJobSpecifics", myConnection)
            Dim adpSQL As New SqlDataAdapter(comSQL)
            Dim dsadded As New DataSet

            Dim objParam As SqlParameter

            comSQL.CommandType = CommandType.StoredProcedure
            objParam = comSQL.Parameters.Add("@JobId", SqlDbType.Int)
            objParam.Direction = ParameterDirection.Input
            objParam.Value = lngJobId

            objParam = comSQL.Parameters.Add("@OrderNumber", SqlDbType.NChar)
            objParam.Direction = ParameterDirection.Input
            objParam.Value = strOrderNum

            objParam = comSQL.Parameters.Add("@ProductionLine", SqlDbType.NChar)
            objParam.Direction = ParameterDirection.Input
            objParam.Value = strProductionLine

            objParam = comSQL.Parameters.Add("@QtyToBeProduced", SqlDbType.Float)
            objParam.Direction = ParameterDirection.Input
            objParam.Value = dblQtyToBeProduced


            myConnection.Open()
            adpSQL.Fill(dsadded)


            myConnection.Close()

        End Sub

        Sub SaveJobHistoryItem(ByVal lngJobId As Long, ByVal strHistoryText As String, ByVal lngUserId As Long)

            Dim myConnection As New SqlConnection(Session("_VT_DotNetConnString"))
            Dim comSQL As New SqlCommand("job_insJobHistoryItem", myConnection)
            Dim adpSQL As New SqlDataAdapter(comSQL)
            Dim dsadded As New DataSet

            Dim objParam As SqlParameter

            comSQL.CommandType = CommandType.StoredProcedure
            objParam = comSQL.Parameters.Add("@JobId", SqlDbType.Int)
            objParam.Direction = ParameterDirection.Input
            objParam.Value = lngJobId

            objParam = comSQL.Parameters.Add("@JobStatus", SqlDbType.NChar)
            objParam.Direction = ParameterDirection.Input
            objParam.Value = strHistoryText

            objParam = comSQL.Parameters.Add("@DateStatusAchieved", SqlDbType.DateTime)
            objParam.Direction = ParameterDirection.Input
            objParam.Value = PortalFunctions.Now.Date

            objParam = comSQL.Parameters.Add("@ChangedBy", SqlDbType.Float)
            objParam.Direction = ParameterDirection.Input
            objParam.Value = lngUserId


            myConnection.Open()
            adpSQL.Fill(dsadded)


            myConnection.Close()



        End Sub

        Sub UpdateJobDateTimeStarted(ByVal lngJobID As Long, ByVal strTime As String, ByVal dteDate As Date)

            Dim myConnection As New SqlConnection(Session("_VT_DotNetConnString"))
            Dim comSQL As New SqlCommand("pdn_updDateAndTimeStarted", myConnection)
            Dim adpSQL As New SqlDataAdapter(comSQL)
            Dim dsadded As New DataSet

            Dim objParam As SqlParameter

            comSQL.CommandType = CommandType.StoredProcedure
            objParam = comSQL.Parameters.Add("@JobId", SqlDbType.Int)
            objParam.Direction = ParameterDirection.Input
            objParam.Value = lngJobID

            objParam = comSQL.Parameters.Add("@TimeStarted", SqlDbType.NChar)
            objParam.Direction = ParameterDirection.Input
            objParam.Value = strTime

            objParam = comSQL.Parameters.Add("@DateStarted", SqlDbType.DateTime)
            objParam.Direction = ParameterDirection.Input
            objParam.Value = dteDate

            myConnection.Open()
            adpSQL.Fill(dsadded)


            myConnection.Close()
        End Sub

        Sub UpdateJobStatusText(ByVal lngJobID As Long, ByVal strStatus As String)

            Dim myConnection As New SqlConnection(Session("_VT_DotNetConnString"))
            Dim comSQL As New SqlCommand("wfo_updBatchStatusText", myConnection)
            Dim adpSQL As New SqlDataAdapter(comSQL)
            Dim dsadded As New DataSet

            Dim objParam As SqlParameter

            comSQL.CommandType = CommandType.StoredProcedure
            objParam = comSQL.Parameters.Add("@JobId", SqlDbType.Int)
            objParam.Direction = ParameterDirection.Input
            objParam.Value = lngJobID

            objParam = comSQL.Parameters.Add("@StatusText", SqlDbType.NChar)
            objParam.Direction = ParameterDirection.Input
            objParam.Value = strStatus

            myConnection.Open()
            adpSQL.Fill(dsadded)


            myConnection.Close()
        End Sub

        Sub SaveBatchFinished(ByVal lngJobId As Long, ByVal lngQty As Long, ByVal dblWeight As Double, ByVal intLocation As Integer)

            Dim myConnection As New SqlConnection(Session("_VT_DotNetConnString"))
            Dim comSQL As New SqlCommand("prj_updCompleteBatchSpecifics", myConnection)
            Dim adpSQL As New SqlDataAdapter(comSQL)
            Dim dsadded As New DataSet

            Dim objParam As SqlParameter

            comSQL.CommandType = CommandType.StoredProcedure
            objParam = comSQL.Parameters.Add("@JobId", SqlDbType.Int)
            objParam.Direction = ParameterDirection.Input
            objParam.Value = lngJobId

            objParam = comSQL.Parameters.Add("@Weight", SqlDbType.Float)
            objParam.Direction = ParameterDirection.Input
            objParam.Value = dblWeight

            objParam = comSQL.Parameters.Add("@Quantity", SqlDbType.Float)
            objParam.Direction = ParameterDirection.Input
            objParam.Value = lngQty


            objParam = comSQL.Parameters.Add("@Locationid", SqlDbType.Int)
            objParam.Direction = ParameterDirection.Input
            objParam.Value = intLocation


            myConnection.Open()
            adpSQL.Fill(dsadded)


            myConnection.Close()
        End Sub
        Sub UpdateBatchDateTimeFinished(ByVal lngJobId As Long)

            Dim myConnection As New SqlConnection(Session("_VT_DotNetConnString"))
            Dim comSQL As New SqlCommand("wfo_updBatchDateTimeFinished", myConnection)
            Dim adpSQL As New SqlDataAdapter(comSQL)
            Dim dsadded As New DataSet

            Dim objParam As SqlParameter

            comSQL.CommandType = CommandType.StoredProcedure
            objParam = comSQL.Parameters.Add("@JobId", SqlDbType.Int)
            objParam.Direction = ParameterDirection.Input
            objParam.Value = lngJobId

            objParam = comSQL.Parameters.Add("@BatchDateFinished", SqlDbType.DateTime)
            objParam.Direction = ParameterDirection.Input
            objParam.Value = PortalFunctions.Now.Date

            objParam = comSQL.Parameters.Add("@BatchTimeFinished", SqlDbType.DateTime)
            objParam.Direction = ParameterDirection.Input
            objParam.Value = PortalFunctions.Now.Date

            myConnection.Open()
            adpSQL.Fill(dsadded)

            myConnection.Close()
        End Sub

        Sub UpdateJobPriority(ByVal lngJobId As Long, ByVal strPriority As String)

            Dim myConnection As New SqlConnection(Session("_VT_DotNetConnString"))
            Dim comSQL As New SqlCommand("wfo_updJobPriority", myConnection)
            Dim adpSQL As New SqlDataAdapter(comSQL)
            Dim dsadded As New DataSet

            Dim objParam As SqlParameter

            comSQL.CommandType = CommandType.StoredProcedure
            objParam = comSQL.Parameters.Add("@JobId", SqlDbType.Int)
            objParam.Direction = ParameterDirection.Input
            objParam.Value = lngJobId

            objParam = comSQL.Parameters.Add("@Priority", SqlDbType.NVarChar)
            objParam.Direction = ParameterDirection.Input
            objParam.Value = strPriority


            myConnection.Open()
            adpSQL.Fill(dsadded)

            myConnection.Close()
        End Sub

        Sub UpdateJobType(ByVal lngjobId As Long, ByVal intJobType As Integer)

            Dim myConnection As New SqlConnection(Session("_VT_DotNetConnString"))
            Dim comSQL As New SqlCommand("prj_updJobType", myConnection)
            Dim adpSQL As New SqlDataAdapter(comSQL)
            Dim dsadded As New DataSet

            Dim objParam As SqlParameter

            comSQL.CommandType = CommandType.StoredProcedure
            objParam = comSQL.Parameters.Add("@JobId", SqlDbType.Int)
            objParam.Direction = ParameterDirection.Input
            objParam.Value = lngjobId

            objParam = comSQL.Parameters.Add("@WhichJobIsThis", SqlDbType.Int)
            objParam.Direction = ParameterDirection.Input
            objParam.Value = 1

            myConnection.Open()
            adpSQL.Fill(dsadded)


            myConnection.Close()

        End Sub
        Function GetJobSpecifics(ByVal lngJobId As Long) As DataSet

            Dim myConnection As New SqlConnection(Session("_VT_DotNetConnString"))
            Dim comSQL As New SqlCommand("prj_resGetJobSpecifics", myConnection)
            Dim adpSQL As New SqlDataAdapter(comSQL)
            Dim ds As New DataSet

            Dim objParam As SqlParameter

            comSQL.CommandType = CommandType.StoredProcedure
            objParam = comSQL.Parameters.Add("@JobID", SqlDbType.Int)
            objParam.Direction = ParameterDirection.Input
            objParam.Value = lngJobId

            myConnection.Open()
            adpSQL.Fill(ds)
            GetJobSpecifics = ds
            '  Close the connection when done with it.
            myConnection.Close()
        End Function

        Function GetWFOBatchTableData(ByVal lngJobId As Long) As DataSet

            Dim myConnection As New SqlConnection(Session("_VT_DotNetConnString"))
            Dim comSQL As New SqlCommand("wfo_resGetBatchTableData", myConnection)
            Dim adpSQL As New SqlDataAdapter(comSQL)
            Dim ds As New DataSet

            Dim objParam As SqlParameter

            comSQL.CommandType = CommandType.StoredProcedure
            objParam = comSQL.Parameters.Add("@JobID", SqlDbType.Int)
            objParam.Direction = ParameterDirection.Input
            objParam.Value = lngJobId

            myConnection.Open()
            adpSQL.Fill(ds)
            GetWFOBatchTableData = ds
            '  Close the connection when done with it.
            myConnection.Close()
        End Function

        Sub SaveTraceCodeforBatch(ByVal lngBatchId As Long, ByVal lngTraceCodeId As Long)
            Dim myConnection As New SqlConnection(Session("_VT_DotNetConnString"))
            Dim comSQL As New SqlCommand("prj_updBatchTraceCode", myConnection)
            Dim adpSQL As New SqlDataAdapter(comSQL)
            Dim dsadded As New DataSet

            Dim objParam As SqlParameter

            comSQL.CommandType = CommandType.StoredProcedure
            objParam = comSQL.Parameters.Add("@JobId", SqlDbType.Int)
            objParam.Direction = ParameterDirection.Input
            objParam.Value = lngBatchId

            objParam = comSQL.Parameters.Add("@TraceCodeId", SqlDbType.Int)
            objParam.Direction = ParameterDirection.Input
            objParam.Value = lngTraceCodeId

            myConnection.Open()
            adpSQL.Fill(dsadded)


            myConnection.Close()
        End Sub
        Sub SaveNewJob(ByVal strBatchId As String, ByVal intJobWeek As Integer, ByVal intJobYear As Integer, ByVal dteDateDueToStart As Date, ByVal lngCustomerId As Long, ByVal lngpersRespId As Long, ByVal lngProductId As Long, ByVal lngBatchId As Long)
            Dim myConnection As New SqlConnection(Session("_VT_DotNetConnString"))
            Dim comSQL As New SqlCommand("prj_UpdNewBatch", myConnection)
            Dim adpSQL As New SqlDataAdapter(comSQL)
            Dim dsadded As New DataSet

            Dim objParam As SqlParameter

            comSQL.CommandType = CommandType.StoredProcedure
            objParam = comSQL.Parameters.Add("@JobIdDesc", SqlDbType.NChar)
            objParam.Direction = ParameterDirection.Input
            objParam.Value = strBatchId

            objParam = comSQL.Parameters.Add("@DateCreated", SqlDbType.DateTime)
            objParam.Direction = ParameterDirection.Input
            objParam.Value = PortalFunctions.Now.Date

            objParam = comSQL.Parameters.Add("@ProgramId", SqlDbType.Int)
            objParam.Direction = ParameterDirection.Input
            objParam.Value = 12

            objParam = comSQL.Parameters.Add("@JobWeek", SqlDbType.Int)
            objParam.Direction = ParameterDirection.Input
            objParam.Value = intJobWeek

            objParam = comSQL.Parameters.Add("@JobYear", SqlDbType.Int)
            objParam.Direction = ParameterDirection.Input
            objParam.Value = intJobYear

            objParam = comSQL.Parameters.Add("@DateDueToStart", SqlDbType.DateTime)
            objParam.Direction = ParameterDirection.Input
            objParam.Value = dteDateDueToStart

            objParam = comSQL.Parameters.Add("@CustomerId", SqlDbType.Int)
            objParam.Direction = ParameterDirection.Input
            objParam.Value = lngCustomerId

            objParam = comSQL.Parameters.Add("@JobStatusText", SqlDbType.NChar)
            objParam.Direction = ParameterDirection.Input
            objParam.Value = "New"

            objParam = comSQL.Parameters.Add("@PersonResponsible", SqlDbType.Int)
            objParam.Direction = ParameterDirection.Input
            objParam.Value = lngpersRespId

            objParam = comSQL.Parameters.Add("@ProductId", SqlDbType.Int)
            objParam.Direction = ParameterDirection.Input
            objParam.Value = lngProductId

            objParam = comSQL.Parameters.Add("@JobId", SqlDbType.Int)
            objParam.Direction = ParameterDirection.Input
            objParam.Value = lngBatchId


            myConnection.Open()
            adpSQL.Fill(dsadded)


            myConnection.Close()

        End Sub
        Public Sub AddIngredientsForJob(ByVal lngproductID As Long, ByVal lngJobId As Long, ByVal intMultiple As Integer)
            Dim ds As New DataSet
            Dim i As Integer
            Dim intQty As Integer
            Dim dblWgt As Double


            ds = GetIngredientsForProduct(lngproductID)

            For i = 0 To ds.Tables(0).Rows.Count - 1
                With ds.Tables(0).Rows(i)
                    intQty = IIf(IsDBNull(.Item("Quantity")), 0, .Item("Quantity")) * intMultiple
                    dblWgt = IIf(IsDBNull(.Item("Weight")), 0, .Item("Weight")) * intMultiple

                    AddIngredientToBatch(lngproductID, .Item("IngredientProductId"), intQty, dblWgt, lngJobId, IIf(IsDBNull(.Item("AdditionOrder")), 0, .Item("AdditionOrder")))

                End With
            Next
        End Sub
        Public Function getAllProductionJobsForIngredient(ByVal dteStartDate As Date, ByVal dteEndDate As Date, ByVal lngIngredientID As Long) As Data.DataTable
            Dim dt As New Data.DataTable
            Dim strsql As String
            Dim strconn As String
            Dim strstartDate As String = dteStartDate.ToString("yyyy-MM-dd") '"2008-06-15" 'Date.Today.ToString("yyyy-MM-dd")
            Dim strEndDate As String = dteEndDate.ToString("yyyy-MM-dd") ' "2008-06-18" 'Date.Today.AddDays(1).ToString("yyyy-MM-dd")
            ' Dim lngProductID As Long


            'strstartDate = Me.CurrentSession.VT_PlanningStartDate
            'strEndDate = Me.CurrentSession.VT_PlanningEndDate

            ' strstartDate = "2008-08-21" ' Date.Today.ToString("yyyy-MM-dd")
            ' strEndDate = "2008-08-23" 'Date.Today.AddDays(1).ToString("yyyy-MM-dd")


            '    lngProductID = Me.CurrentSession.VT_IngredProductID
            Dim strProductionJobId As String = ConfigurationManager.AppSettings("ProductionJobId")

            strsql = "select wfo_batchtable.JobIDDesc as JobDesc, wfo_batchtable.JobID, " & _
            "wfo_batchtable.dateduetostart as DateDue, prd_ProductTable.Product_Name as ProductName," & _
            "prd_ProductTable.Catalog_number as ProductCode,pdn_ingredients.quantity, " & _
            "pdn_ingredients.weight from wfo_batchtable " & _
            "inner join pdn_ingredients on wfo_batchtable.jobid = pdn_ingredients.jobid " & _
            "inner join prd_ProductTable on wfo_batchtable.productid = prd_ProductTable.productid " & _
            "where(pdn_ingredients.ingredientProductId =" & lngIngredientID & ") " & _
            "And (wfo_batchtable.programid = " + strProductionJobId + ") and JobStatusText <> 'Finished' " & _
            "And (wfo_batchtable.dateduetostart >= '" & strstartDate & "' and wfo_batchtable.dateduetostart < '" & strEndDate & "')"



            strconn = Session("_VT_DotNetConnString")

            Dim myConnection As New SqlConnection(strconn)
            Dim comSQL As New SqlCommand(strsql, myConnection)

            Dim myAdapter As New SqlDataAdapter(comSQL)

            myConnection.Open()
            myAdapter.Fill(dt)

            getAllProductionJobsForIngredient = dt

            myConnection.Close()
            myConnection = Nothing

        End Function

        Public Function GetJobsForDates(ByVal dteStartDate As Date, ByVal dteEndDate As Date, ByVal strfilter As String) As DataTable
            Dim dt As New Data.DataTable
            Dim strsql As String
            Dim strconn As String


            'adding one day to the end date so that the sql will search for dates >= startdate and < end date. If we dont add
            'a day to the end date and use <=Enddate it will not return sales orders for that date that include a time because
            'it is greater than end date if it includes a time
            dteEndDate = dteEndDate.AddDays(1)

            If strfilter <> "" Then
                strfilter = " AND " & strfilter
            End If

            Dim objCommonFuncs As New BPADotNetCommonFunctions.VT_CommonFunctions.CommonFunctions
            Dim strProductionJobId As String = objCommonFuncs.GetConfigItem("ProductionJobID")

            strsql = "Select wfo_BatchTable.*, job_Specifics.*, prd_ProductTable.Product_Name  from wfo_BatchTable, job_Specifics, prd_ProductTable where wfo_BatchTable.Programid in ( " + strProductionJobId + ")"
            strsql = strsql + " and wfo_BatchTable.ProductId =  prd_ProductTable.ProductID and wfo_BatchTable.JobId = job_Specifics.JobId and DateDueToStart >= '" & Format(dteStartDate, "yyyy-MM-dd") & "' and DateDueToStart < '" & Format(dteEndDate, "yyyy-MM-dd") & "'" & strfilter & " order by wfo_BatchTable.JobID desc"


            strconn = Session("_VT_DotNetConnString")

            Dim myConnection As New SqlConnection(strconn)
            Dim comSQL As New SqlCommand(strsql, myConnection)

            Dim myAdapter As New SqlDataAdapter(comSQL)

            myConnection.Open()
            myAdapter.Fill(dt)

            GetJobsForDates = dt

        End Function
        Public Function GetTraceCodeForJobId(ByVal lngJobId As Long) As Long

            Dim myConnection As New SqlConnection(Session("_VT_DotNetConnString"))
            Dim comSQL As New SqlCommand("prj_resGetJobSpecifics", myConnection)
            Dim adpSQL As New SqlDataAdapter(comSQL)
            Dim dsadded As New DataSet

            Dim objParam As SqlParameter

            comSQL.CommandType = CommandType.StoredProcedure
            objParam = comSQL.Parameters.Add("@Jobid", SqlDbType.Int)
            objParam.Direction = ParameterDirection.Input
            objParam.Value = lngJobId

            myConnection.Open()
            adpSQL.Fill(dsadded)
            If dsadded.Tables(0).Rows.Count > 0 Then
                GetTraceCodeForJobId = dsadded.Tables(0).Rows(0).Item("TraceCodeID")
                '  Close the connection when done with it.
            End If

            myConnection.Close()

        End Function
        Function GetProductionBatchName(ByVal lngJobId As Long) As String


            Dim myConnection As New SqlConnection(Session("_VT_DotNetConnString"))
            Dim comSQL As New SqlCommand("prj_resGetBatchTableRecordset", myConnection)
            Dim adpSQL As New SqlDataAdapter(comSQL)
            Dim dsadded As New DataSet

            Dim objParam As SqlParameter

            comSQL.CommandType = CommandType.StoredProcedure
            objParam = comSQL.Parameters.Add("@Jobid", SqlDbType.Int)
            objParam.Direction = ParameterDirection.Input
            objParam.Value = lngJobId

            myConnection.Open()
            adpSQL.Fill(dsadded)
            If dsadded.Tables(0).Rows.Count > 0 Then
                GetProductionBatchName = dsadded.Tables(0).Rows(0).Item("JobIDDesc")
            Else
                GetProductionBatchName = ""
                '  Close the connection when done with it.
            End If

            myConnection.Close()
        End Function
        Function GetTotalQtyIngredientAdded(ByVal lngJobId As Long, ByVal lngProduct As Long, ByRef lngAddedQty As Long, ByRef dblAddedWgt As Double) As Double
            Dim myConnection As New SqlConnection(Session("_VT_DotNetConnString"))
            Dim comSQL As New SqlCommand("trc_resGetProductionForParent", myConnection)
            Dim adpSQL As New SqlDataAdapter(comSQL)
            Dim dsadded As New DataSet
            Dim intunitofSale As Integer
            Dim objproducts As New ProductsFunctions.Products

            Dim objParam As SqlParameter

            comSQL.CommandType = CommandType.StoredProcedure
            objParam = comSQL.Parameters.Add("@ParentID", SqlDbType.Int)
            objParam.Direction = ParameterDirection.Input
            objParam.Value = lngJobId

            myConnection.Open()
            adpSQL.Fill(dsadded)

            intunitofSale = objproducts.GetUnitOfSale(lngProduct)

            If dsadded.Tables(0).Rows.Count > 0 Then
                If intunitofSale = 1 Then
                    GetTotalQtyIngredientAdded = dsadded.Tables(0).Rows(0).Item("Quantity") + lngAddedQty
                Else
                    GetTotalQtyIngredientAdded = dsadded.Tables(0).Rows(0).Item("Quantity") + dblAddedWgt
                End If

                '  Close the connection when done with it.
            End If

            myConnection.Close()


        End Function

        Public Function GetIngredientsAdded(ByVal lngTraceID As Long) As DataSet
            Dim myConnection As New SqlConnection(Session("_VT_DotNetConnString"))
            Dim comSQL As New SqlCommand("trc_resGetProductionForParent", myConnection)
            Dim adpSQL As New SqlDataAdapter(comSQL)
            Dim dsadded As New DataSet

            Dim objParam As SqlParameter

            comSQL.CommandType = CommandType.StoredProcedure
            objParam = comSQL.Parameters.Add("@ParentID", SqlDbType.Int)
            objParam.Direction = ParameterDirection.Input
            objParam.Value = lngTraceID


            myConnection.Open()
            adpSQL.Fill(dsadded)
            GetIngredientsAdded = dsadded
            '  Close the connection when done with it.
            myConnection.Close()
        End Function

        Public Function GetIngredientsForProduct(ByVal lngProductId As Long) As DataSet
            Dim myConnection As New SqlConnection(Session("_VT_DotNetConnString"))
            Dim comSQL As New SqlCommand("prd_resGetIngredients", myConnection)
            Dim adpSQL As New SqlDataAdapter(comSQL)
            Dim ds As New DataSet

            Dim objParam As SqlParameter

            comSQL.CommandType = CommandType.StoredProcedure
            objParam = comSQL.Parameters.Add("@ProductID", SqlDbType.Int)
            objParam.Direction = ParameterDirection.Input
            objParam.Value = lngProductId

            myConnection.Open()
            adpSQL.Fill(ds)
            GetIngredientsForProduct = ds
            '  Close the connection when done with it.
            myConnection.Close()
        End Function

        Public Sub SaveAdditionalCostForJob(ByVal lngJobId As Long, ByVal lngCostId As Long, ByVal strCostDesc As String, ByVal Unitcost As Double, ByVal dblQty As Double)

            Dim myConnection As New SqlConnection(Session("_VT_DotNetConnString"))
            Dim comSQL As New SqlCommand("pdn_insJobOtherCost", myConnection)
            Dim adpSQL As New SqlDataAdapter(comSQL)
            Dim dsadded As New DataSet

            Dim objParam As SqlParameter

            comSQL.CommandType = CommandType.StoredProcedure
            objParam = comSQL.Parameters.Add("@JobId", SqlDbType.Int)
            objParam.Direction = ParameterDirection.Input
            objParam.Value = lngJobId

            objParam = comSQL.Parameters.Add("@CostId", SqlDbType.Int)
            objParam.Direction = ParameterDirection.Input
            objParam.Value = lngCostId


            objParam = comSQL.Parameters.Add("@CostDescription", SqlDbType.NChar)
            objParam.Direction = ParameterDirection.Input
            objParam.Value = strCostDesc


            objParam = comSQL.Parameters.Add("@Unitcost", SqlDbType.Float)
            objParam.Direction = ParameterDirection.Input
            objParam.Value = Unitcost


            objParam = comSQL.Parameters.Add("@Qty", SqlDbType.Float)
            objParam.Direction = ParameterDirection.Input
            objParam.Value = dblQty

            myConnection.Open()
            adpSQL.Fill(dsadded)


            myConnection.Close()
        End Sub
        Public Function GetAdditionalCostForId(ByVal lngCostId As Long) As DataTable
            Dim myConnection As New SqlConnection(Session("_VT_DotNetConnString"))
            Dim comSQL As New SqlCommand("pdn_resOtherCostForId", myConnection)
            Dim adpSQL As New SqlDataAdapter(comSQL)
            Dim ds As New DataTable

            Dim objParam As SqlParameter

            comSQL.CommandType = CommandType.StoredProcedure
            objParam = comSQL.Parameters.Add("@CostId", SqlDbType.Int)
            objParam.Direction = ParameterDirection.Input
            objParam.Value = lngCostId

            myConnection.Open()
            adpSQL.Fill(ds)
            GetAdditionalCostForId = ds
            '  Close the connection when done with it.
            myConnection.Close()
        End Function

        Public Function GetAdditionalCostsForJob(ByVal lngJobId As Long) As DataTable
            Dim myConnection As New SqlConnection(Session("_VT_DotNetConnString"))
            Dim comSQL As New SqlCommand("pdn_resGetJobOtherCosts", myConnection)
            Dim adpSQL As New SqlDataAdapter(comSQL)
            Dim ds As New DataTable

            Dim objParam As SqlParameter

            comSQL.CommandType = CommandType.StoredProcedure
            objParam = comSQL.Parameters.Add("@JobId", SqlDbType.Int)
            objParam.Direction = ParameterDirection.Input
            objParam.Value = lngJobId

            myConnection.Open()
            adpSQL.Fill(ds)
            GetAdditionalCostsForJob = ds
            '  Close the connection when done with it.
            myConnection.Close()
        End Function
        Public Function GetAllAdditionalCosts() As DataSet
            Dim myConnection As New SqlConnection(Session("_VT_DotNetConnString"))
            Dim comSQL As New SqlCommand("pdn_resGetAllAdditionalCosts", myConnection)
            Dim adpSQL As New SqlDataAdapter(comSQL)
            Dim ds As New DataSet

            comSQL.CommandType = CommandType.StoredProcedure

            myConnection.Open()
            adpSQL.Fill(ds)
            GetAllAdditionalCosts = ds
            '  Close the connection when done with it.
            myConnection.Close()
        End Function
        Public Sub SaveNewAdditionalCost(ByVal CostDescription As String, ByVal UnitCost As Double)


            Dim myConnection As New SqlConnection(Session("_VT_DotNetConnString"))
            Dim comSQL As New SqlCommand("pdn_insAdditionalCost", myConnection)
            Dim adpSQL As New SqlDataAdapter(comSQL)
            Dim dsadded As New DataSet

            Dim objParam As SqlParameter

            comSQL.CommandType = CommandType.StoredProcedure
            objParam = comSQL.Parameters.Add("@CostDescription", SqlDbType.NChar)
            objParam.Direction = ParameterDirection.Input
            objParam.Value = CostDescription

            objParam = comSQL.Parameters.Add("@UnitCost", SqlDbType.Float)
            objParam.Direction = ParameterDirection.Input
            objParam.Value = UnitCost


            myConnection.Open()
            adpSQL.Fill(dsadded)


            myConnection.Close()
        End Sub
        Public Function GetIngredientsForJob(ByVal lngJobId As Long) As DataSet
            Dim myConnection As New SqlConnection(Session("_VT_DotNetConnString"))
            Dim comSQL As New SqlCommand("pdn_resGetIngredients", myConnection)
            Dim adpSQL As New SqlDataAdapter(comSQL)
            Dim ds As New DataSet

            Dim objParam As SqlParameter

            comSQL.CommandType = CommandType.StoredProcedure
            objParam = comSQL.Parameters.Add("@JobID", SqlDbType.Int)
            objParam.Direction = ParameterDirection.Input
            objParam.Value = lngJobId

            myConnection.Open()
            adpSQL.Fill(ds)
            GetIngredientsForJob = ds
            '  Close the connection when done with it.
            myConnection.Close()
        End Function
        Public Sub AddIngredientToBatch(ByVal intParentProdId As Integer, ByVal intIngredientProdId As Integer, ByVal intQty As Integer, ByVal dblWeight As Double, ByVal intJobId As Integer, ByVal intAdditionOrder As Integer)

            Dim strConnString As String = Session("_VT_DotNetConnString")
            Dim myConnection As New SqlConnection(strConnString)

            Dim comSQL As New SqlCommand("pdn_insIngredients", myConnection)

            Dim objParam As SqlParameter

            comSQL.CommandType = CommandType.StoredProcedure

            objParam = comSQL.Parameters.Add("@ParentProductId", SqlDbType.Int)
            objParam.Direction = ParameterDirection.Input
            objParam.Value = intParentProdId

            objParam = comSQL.Parameters.Add("@IngredientProductId", SqlDbType.Int)
            objParam.Direction = ParameterDirection.Input
            objParam.Value = intIngredientProdId

            objParam = comSQL.Parameters.Add("@Quantity", SqlDbType.Int)
            objParam.Direction = ParameterDirection.Input
            objParam.Value = intQty

            objParam = comSQL.Parameters.Add("@Weight", SqlDbType.Float)
            objParam.Direction = ParameterDirection.Input
            objParam.Value = dblWeight

            objParam = comSQL.Parameters.Add("@JobId", SqlDbType.Int)
            objParam.Direction = ParameterDirection.Input
            objParam.Value = intJobId

            objParam = comSQL.Parameters.Add("@AdditionOrder", SqlDbType.Int)
            objParam.Direction = ParameterDirection.Input
            objParam.Value = intAdditionOrder


            myConnection.Open()
            comSQL.ExecuteNonQuery()
        End Sub


        Public Function GetProductsTraceCodeUsedIn(ByVal intTraceCodeID As Integer) As DataSet

            Dim myConnection As New SqlConnection(Session("_VT_DotNetConnString"))
            Dim comSQL As New SqlCommand("trc_resGetProductionOutToStoresForChild", myConnection)
            Dim adpSQL As New SqlDataAdapter(comSQL)
            Dim ds As New DataSet

            Dim objParam As SqlParameter
            '********************************* TEMPORARY TEST VALUE *************************
            '   Me.CurrentSession.VT_JobTraceCodeID = 841
            '********************************* TEMPORARY TEST VALUE *************************

            comSQL.CommandType = CommandType.StoredProcedure
            objParam = comSQL.Parameters.Add("@ChildID", SqlDbType.Int)
            objParam.Direction = ParameterDirection.Input
            objParam.Value = intTraceCodeID

            myConnection.Open()
            adpSQL.Fill(ds)

            GetProductsTraceCodeUsedIn = ds


            '  Close the connection when done with it.
            myConnection.Close()


        End Function


        Public Function GetWasteTransactionsforTraceCodeID(ByVal intTraceCodeID As Integer) As DataSet

            Dim myConnection As New SqlConnection(Session("_VT_DotNetConnString"))
            Dim comSQL As New SqlCommand("trc_resGetProductionOutToStoresForChild", myConnection)
            Dim adpSQL As New SqlDataAdapter(comSQL)
            Dim ds As New DataSet

            Dim objParam As SqlParameter
            '********************************* TEMPORARY TEST VALUE *************************
            '   Me.CurrentSession.VT_JobTraceCodeID = 841
            '********************************* TEMPORARY TEST VALUE *************************

            comSQL.CommandType = CommandType.StoredProcedure
            objParam = comSQL.Parameters.Add("@ChildID", SqlDbType.Int)
            objParam.Direction = ParameterDirection.Input
            objParam.Value = intTraceCodeID

            myConnection.Open()
            adpSQL.Fill(ds)

            GetWasteTransactionsforTraceCodeID = ds


            '  Close the connection when done with it.
            myConnection.Close()


        End Function




        Public Function SaveReturnedToStores(ByVal lngProductId As Long, ByVal strTraceCode As String, ByVal intQuantity As Integer, _
                                        ByVal dblWeight As Double, ByVal lngLocationId As Long, ByVal strLocationDesc As String, _
                                        Optional ByVal strSerial As String = "", Optional ByVal strBarcode As String = "", Optional ByVal strComment As String = "") As Long


            '     Dim objTrace As New prjTTeQTraceDBAccess.clsTrace
            Dim objTrace As New VTDBFunctions.VTTraceDBAccess.VTTraceDBAccess
            Dim objProducts As New ProductsFunctions.Products
            Dim objTele As New VTDBFunctions.VTDBFunctions.TelesalesFunctions
            Dim strerror As String = ""
            Dim intUnitOfSale As Integer = objProducts.GetUnitOfSale(lngProductId)
            Dim lngInternalStockLocn As Long
            Dim lngTxnId As Long


            If lngLocationId < 0 Then
                lngInternalStockLocn = 0
            Else
                lngInternalStockLocn = lngLocationId
            End If

            'Save to Returned to Stores Traceability
            If strSerial <> "" Or strBarcode <> "" Then

                objTrace.ReturnToStock(lngProductId, strTraceCode, "", intQuantity, dblWeight, PortalFunctions.Now.Date, strerror, lngInternalStockLocn,
                                         intUnitOfSale, strSerial, strBarcode, , , 0, strComment)



            Else
                objTrace.ReturnToStock(lngProductId, strTraceCode, "", intQuantity, dblWeight, PortalFunctions.Now.Date, strerror, lngInternalStockLocn,
                                         intUnitOfSale, , , , , 0, strComment)
            End If

            lngTxnId = objTele.GetLastAddedTransaction

            'If locationID is -1 it's to an external location/supplier so do a "Return to Supplier"
            If lngLocationId < 0 Then

                If strSerial <> "" Or strBarcode <> "" Then

                    objTrace.ItemsReturnedToSupplier(lngProductId, strTraceCode, intQuantity, dblWeight, PortalFunctions.Now.Date, strerror, 0, lngInternalStockLocn,
                                                            intUnitOfSale, , , strComment, , strSerial, strBarcode, , 0)
                Else
                    objTrace.ItemsReturnedToSupplier(lngProductId, strTraceCode, intQuantity, dblWeight, PortalFunctions.Now.Date, strerror, 0, lngInternalStockLocn,
                                                     intUnitOfSale, , , strComment, , , , , 0)

                End If

            End If


            'if we are using sage we need to send this to sage also
            Dim objcommon As New SupportFunctions.Support
            Dim objSage As Object
            Dim strClassName As String
            Dim strtxndata As String

            strClassName = objcommon.GetSageUploadDllName
            If strClassName <> "" Then
                objSage = CreateObject(strClassName)
                strtxndata = objProducts.GetProductCode(lngProductId) & "," & PortalFunctions.Now.Date & ",3," &
                    IIf((intUnitOfSale = 1), intQuantity, dblWeight) & ",," & strTraceCode & ","

                objSage.SendStockMovementToSage(strtxndata)
            End If

            SaveReturnedToStores = lngTxnId

        End Function



    End Class
End Namespace

Namespace LocalTraceFunctions
    Public Class Trace
        Inherits MyBasePage

        Public Function GetRawTableData(ByVal strTableName As String, Optional ByVal strQuery As String = "", Optional ByVal strOrderBy As String = "") As DataTable
            Dim dt As New Data.DataTable
            Dim strsql As String
            Dim strconn As String

            Dim objCommonFuncs As New BPADotNetCommonFunctions.VT_CommonFunctions.CommonFunctions

            If strQuery <> "" Then
                strQuery = " WHERE " & strQuery
            End If

            If strOrderBy <> "" Then
                strOrderBy = " ORDER BY " & strOrderBy
            End If

            strsql = "SELECT * FROM " & strTableName & " " & strQuery & " " & strOrderBy

            strconn = Session("_VT_DotNetConnString")

            Dim myConnection As New SqlConnection(strconn)
            Dim comSQL As New SqlCommand(strsql, myConnection)

            Dim myAdapter As New SqlDataAdapter(comSQL)

            myConnection.Open()
            myAdapter.Fill(dt)

            GetRawTableData = dt


            '  Close the connection when done with it.
            myConnection.Close()



        End Function





    End Class



End Namespace
Namespace TransactionFunctions
    Public Class TransactionFunctions

        Public Function getTxDataForDocketNum(ByVal strConn As String, ByVal strDeliveryDocket As String) As Data.DataTable
            Dim strsql As String = ""
            Dim dt As New DataTable
            Dim dtemp As New DataTable
            Dim objPers As New BPADotNetCommonFunctions.PersonnelModuleFunctions

            strsql = "SELECT trc_Transactions.TransactionID, trc_Transactions.ProductLineId, trc_Transactions.ProductId, trc_Transactions.TraceCodeId, trc_Transactions.Driver, trc_Transactions.DateOfTransaction,cus_Customers.CustomerName, "
            strsql = strsql + " trc_Transactions.Quantity, trc_Transactions.TransactionType, trc_Transactions.PONumber, trc_Transactions.CustomerID, trc_Transactions.CustomerName, trc_Transactions.PricePerItem, "
            strsql = strsql + " trc_Transactions.Weight, trc_Transactions.DocketNum, trc_Transactions.CustomerRefNum, trc_Transactions.PaymentType, trc_Transactions.Comment, trc_Transactions.CurrentStoresQty, "
            strsql = strsql + " trc_Transactions.PriceCharged, trc_Transactions.LocationID, trc_Transactions.SupplierId, trc_Transactions.SentToSage, trc_Transactions.SupplierBatchNum, trc_Transactions.SellByDate, "
            strsql = strsql + " trc_Transactions.InvoiceNum, trc_Transactions.VatCharged, trc_Transactions.DeliveryCustomerId, trc_Transactions.Rack, trc_Transactions.UserId, trc_Transactions.SalesOrderNum, "
            strsql = strsql + " trc_Transactions.SalesOrderId, trc_Transactions.SalesOrderItemId, trc_Transactions.IsComplete, trc_Transactions.TotalPrice, trc_Transactions.SyncId, trc_Transactions.DeliveryDocItemId, "
            strsql = strsql + " trc_Transactions.PurOrderNum, trc_Transactions.PurOrderItemId, trc_Transactions.SerialNum, trc_Transactions.Barcode, trc_Transactions.JobId, trc_Transactions.ReverseTxnId, "
            strsql = strsql + " prd_ProductTable.Product_Name, prd_ProductTable.Catalog_Number, prd_ProductTable.UoM,trc_TraceCodes.TraceCodeDesc "
            strsql = strsql + " FROM trc_Transactions"
            strsql = strsql + " INNER JOIN prd_ProductTable ON trc_Transactions.ProductId = prd_ProductTable.ProductID "
            strsql = strsql + " INNER JOIN trc_TraceCodes ON trc_Transactions.TraceCodeId = trc_TraceCodes.TraceCodeId "
            strsql = strsql + " INNER JOIN cus_Customers ON trc_Transactions.CustomerId = cus_Customers.CustomerID "
            strsql = strsql + " WHERE (trc_Transactions.DocketNum = '" & strDeliveryDocket & "')"
            strsql = strsql + " AND (trc_Transactions.TransactionType = '3' OR trc_Transactions.TransactionType = '6') "
            strsql = strsql + " AND (trc_Transactions.PaymentType <> '2') "


            Dim myConnection As New SqlConnection(strConn)
            Dim comSQL As New SqlCommand(strsql, myConnection)

            comSQL.CommandTimeout = 300

            Dim myAdapter As New SqlDataAdapter(comSQL)

            Try
                myConnection.Open()
                myAdapter.Fill(dt)

                'Now look up The following items as add them to the DataTable
                dt.Columns.Add("DeliveryCustomerName")
                dt.Columns.Add("DeliveryCustomerReference")
                dt.Columns.Add("DeliveryCustomerAddress")
                dt.Columns.Add("DeliveryCustomerPhone")
                dt.Columns.Add("DeliveryCustomerFax")
                dt.Columns.Add("BillingAddress")
                dt.Columns.Add("BillingPhone")
                dt.Columns.Add("BillingFax")

                If dt.Rows.Count > 0 Then
                    'look up the Customer Details For the Billing Customer ID
                    strsql = "SELECT cus_CustomerDetails.BillingAddress, cus_CustomerDetails.PhoneNumber, cus_CustomerDetails.FaxNumber "
                    strsql = strsql + " FROM cus_CustomerDetails "
                    strsql = strsql + " WHERE(cus_CustomerDetails.CustomerID = " + dt.Rows(0).Item("CustomerId").ToString + ") "

                    comSQL.CommandText = strsql
                    comSQL.CommandTimeout = 300
                    myAdapter.Fill(dtemp)

                    If dtemp.Rows.Count > 0 Then
                        dt.Rows(0).Item("BillingAddress") = dtemp.Rows(0).Item("BillingAddress")
                        dt.Rows(0).Item("BillingPhone") = dtemp.Rows(0).Item("PhoneNumber")
                        dt.Rows(0).Item("BillingFax") = dtemp.Rows(0).Item("FaxNumber")

                    End If

                    dtemp.Clear()
                    'look up the Customer Details For the Delivery Customer ID
                    strsql = "SELECT cus_CustomerDetails.BillingAddress, cus_CustomerDetails.PhoneNumber, cus_CustomerDetails.FaxNumber "
                    strsql = strsql + " FROM cus_CustomerDetails "
                    strsql = strsql + " WHERE(cus_CustomerDetails.CustomerID = " + dt.Rows(0).Item("DeliveryCustomerId").ToString + ") "

                    comSQL.CommandText = strsql
                    comSQL.CommandTimeout = 300
                    myAdapter.Fill(dtemp)

                    If dtemp.Rows.Count > 0 Then
                        dt.Rows(0).Item("DeliveryCustomerAddress") = dtemp.Rows(0).Item("BillingAddress")
                        dt.Rows(0).Item("DeliveryCustomerPhone") = dtemp.Rows(0).Item("PhoneNumber")
                        dt.Rows(0).Item("DeliveryCustomerFax") = dtemp.Rows(0).Item("FaxNumber")

                    End If

                    dtemp.Clear()
                    'look up the Customer Name For the Delivery Customer ID
                    strsql = "SELECT cus_Customers.CustomerName, cus_Customers.CustomerReference "
                    strsql = strsql + " FROM cus_Customers "
                    strsql = strsql + " WHERE(cus_Customers.CustomerID = " + dt.Rows(0).Item("DeliveryCustomerId").ToString + ") "

                    comSQL.CommandText = strsql
                    comSQL.CommandTimeout = 300
                    myAdapter.Fill(dtemp)

                    If dtemp.Rows.Count > 0 Then
                        dt.Rows(0).Item("DeliveryCustomerName") = dtemp.Rows(0).Item("CustomerName")
                        dt.Rows(0).Item("DeliveryCustomerReference") = dtemp.Rows(0).Item("CustomerReference")
                    End If

                End If

            Catch ex As Exception


            Finally
                '  Close the connection when done with it.
                myConnection.Close()
            End Try

            getTxDataForDocketNum = dt




        End Function


    End Class
End Namespace
Namespace SerialFunctions
            Public Class SerialFunctions



                'Public Function getCurrentWeightForS()
                'Public Function GetDetailsForSerialNumAndTraceId(ByVal SerialNum As String, ByVal lngTraceCodeId As Long) As Data.DataSet

                '    Dim ds As New Data.DataSet
                '    Dim dsSerialNum As New Data.DataSet
                '    Dim objProds As New ProductsFunctions
                '    Dim objTrace As New TraceFunctions

                '    ds.Tables.Add("SerialNums")
                '    With ds.Tables("SerialNums").Columns
                '        .Add("ProductCode", System.Type.GetType("System.String"))
                '        .Add("ProductName", System.Type.GetType("System.String"))
                '        .Add("Weight", System.Type.GetType("System.Double"))
                '        .Add("Quantity", System.Type.GetType("System.Double"))
                '        .Add("TraceCode", System.Type.GetType("System.String"))
                '        .Add("SerialNum", System.Type.GetType("System.String"))
                '        .Add("Barcode", System.Type.GetType("System.String"))
                '        .Add("Location", System.Type.GetType("System.String"))
                '        .Add("SellByDate", System.Type.GetType("System.String"))
                '        .Add("LocationPosition", System.Type.GetType("System.String"))
                '        .Add("ProductionDate", System.Type.GetType("System.String"))
                '        .Add("ProductId", System.Type.GetType("System.Int64"))
                '        .Add("LocationId", System.Type.GetType("System.Int64"))




            End Class
        End Namespace
Namespace LocalQuoteFunctions
            Public Class Quotes
                Inherits MyBasePage
                Public Sub FillQuotesPagesArray()


                    ' find the Pages folder for this category
                    Dim objMatrix As New VT_CommonFunctions.MatrixFunctions

                    Dim objCfg As New VT_CommonFunctions.MatrixFunctions
                    Dim intRoot As Integer = objCfg.GetModuleRootId(HttpContext.Current.Session("_VT_DotNetConnString"), "cfg_FormList", "ConfigModuleRoot")
                    Dim intModuleSetupsId As Integer = objCfg.GetGenericFolderId(HttpContext.Current.Session("_VT_DotNetConnString"), "cfg_FormList", intRoot, "ModuleSetups")
                    Dim intQuoteFolder As Integer = objMatrix.FindGenericFolderId(HttpContext.Current.Session("_VT_DotNetConnString"), "cfg_FormList", intModuleSetupsId, "Quotes")
                    Dim intSetupFolder As Integer = objMatrix.FindGenericFolderId(HttpContext.Current.Session("_VT_DotNetConnString"), "cfg_FormList", intQuoteFolder, "Setup")
                    Dim intPagesFolder As Integer = objMatrix.FindGenericFolderId(HttpContext.Current.Session("_VT_DotNetConnString"), "cfg_FormList", intSetupFolder, "Pages")


                    Dim astrOptions() As String
                    Dim astrOptionPages() As String
                    Dim objComm As New VT_CommonFunctions.CommonFunctions
                    Dim dtPages As Data.DataTable

                    If intPagesFolder <> 0 Then
                        ' if there is a pages folder for this category we can get the data from there
                        Dim astrItems(2) As String
                        astrItems(0) = "Order"
                        astrItems(1) = "Page Title"
                        astrItems(2) = "Page FileName"

                        dtPages = objMatrix.GetData(HttpContext.Current.Session("_VT_DotNetConnString"), "ParentId", intPagesFolder.ToString, "cfg_FormData", astrItems)

                    Else

                        'Dim objCfg As New ConfigModuleFunctions
                        'Dim intRoot As Integer = objCfg.GetConfigModuleRootId(HttpContext.Current.Session("_VT_DotNetConnString"))
                        'Dim intCatId As Integer = objCfg.GetConfigCategoryFolderId(HttpContext.Current.Session("_VT_DotNetConnString"), intRoot)
                        'Dim intModuleCatId As Integer = objCfg.GetConfigCategoryId(HttpContext.Current.Session("_VT_DotNetConnString"), "ModuleSetups")


                        'Dim astrData(2) As String
                        'astrData(0) = "OrderNum"
                        'astrData(1) = "Title"
                        'astrData(2) = "Page"
                        'Dim objDBF As New VTDBFunctions.VTDBFunctions.VTMatrixFunctions
                        'Dim ds As New Data.DataSet
                        'Dim intIndex As Integer

                        'ds = objDBF.GetVTMatrixData(HttpContext.Current.Session("_VT_DotNetConnString"), ConfigModuleFunctions.g_strFormDataTable, UBound(astrData) + 1, astrData, Me.CurrentSession.ConfigModuleFolder, CStr(intModuleCatId))

                    End If

                    ReDim astrOptions(dtPages.Rows.Count - 1)
                    ReDim astrOptionPages(dtPages.Rows.Count - 1)

                    ' insert the pages in the arrays in the order of the OrderNum field
                    For intIndex = 0 To dtPages.Rows.Count - 1
                        astrOptions(dtPages.Rows(intIndex).Item("Field0") - 1) = dtPages.Rows(intIndex).Item("Field1")
                        If Left(dtPages.Rows(intIndex).Item("Field2"), 1) = "~" Then
                            astrOptionPages(dtPages.Rows(intIndex).Item("Field0") - 1) = dtPages.Rows(intIndex).Item("Field2")
                        Else
                            astrOptionPages(dtPages.Rows(intIndex).Item("Field0") - 1) = "~/Quotes/" + dtPages.Rows(intIndex).Item("Field2")
                        End If
                    Next


                    Me.CurrentSession.aVT_DetailsPageOptions = astrOptions
                    Me.CurrentSession.aVT_DetailsPageOptionsPages = astrOptionPages


                End Sub



                Public Function GetQuotesForDates(ByVal dteStartDate As Date, ByVal dteEndDate As Date, ByVal strfilter As String, ByVal blnAll As Boolean) As DataTable
                    Dim dt As New Data.DataTable
                    Dim strsql As String
                    Dim strconn As String

                    Dim objCommonFuncs As New BPADotNetCommonFunctions.VT_CommonFunctions.CommonFunctions
                    Dim strQuotesJobId As String = objCommonFuncs.GetConfigItem("QuotesJobID")


                    If blnAll = True Then ' Don't limit by date

                        If strfilter <> "" Then
                            strfilter = " AND " & strfilter
                        End If


                        strsql = "SELECT DISTINCT wfo_BatchTable.JobId, wfo_BatchTable.ParentId, wfo_BatchTable.ModelType, wfo_BatchTable.BatchTimeStarted, wfo_BatchTable.BatchDateStarted, "
                        strsql = strsql + "wfo_BatchTable.BatchDateFinished, wfo_BatchTable.LastActivity, wfo_BatchTable.BatchTimeFinished, wfo_BatchTable.CurrentWFO, wfo_BatchTable.JobStatusText, "
                        strsql = strsql + "wfo_BatchTable.ExtraData1, wfo_BatchTable.ExtraData2, wfo_BatchTable.ExtraData3, wfo_BatchTable.ExtraData4, wfo_BatchTable.ExtraData5, wfo_BatchTable.OperatorId  "
                        strsql = strsql + ", quo_Quotes.QuoteId, quo_Quotes.RequestedDeliveryDate, quo_Quotes.DateIssued, wfo_BatchTable.SystemId"
                        strsql = strsql + " FROM wfo_BatchTable INNER JOIN quo_Quotes ON wfo_BatchTable.JobId = quo_Quotes.QuoteNum "
                        strsql = strsql + " where wfo_BatchTable.Programid in ( " + strQuotesJobId + ")  AND wfo_BatchTable.ParentId = 0 " & strfilter & " order by wfo_BatchTable.JobID desc"

                    Else

                        'adding one day to the end date so that the sql will search for dates >= startdate and < end date. If we dont add
                        'a day to the end date and use <=Enddate it will not return sales orders for that date that include a time because
                        'it is greater than end date if it includes a time
                        Dim strDateString As String
                        strDateString = ""

                        If dteStartDate <> #12:00:00 AM# Then
                            strDateString = "AND wfo_BatchTable.BatchDateStarted >= '" & Format(dteStartDate, "yyyy-MM-dd") & "' "
                        End If

                        If dteEndDate <> #12:00:00 AM# Then
                            dteEndDate = dteEndDate.AddDays(1)
                            strDateString = strDateString & " AND wfo_BatchTable.BatchDateStarted < '" & Format(dteEndDate, "yyyy-MM-dd") & "'"
                        End If

                        If strfilter <> "" Then
                            strfilter = " AND " & strfilter
                        End If


                        strsql = "SELECT DISTINCT wfo_BatchTable.JobId, wfo_BatchTable.ParentId, wfo_BatchTable.ModelType, wfo_BatchTable.BatchTimeStarted, wfo_BatchTable.BatchDateStarted, "
                        strsql = strsql + "wfo_BatchTable.BatchDateFinished, wfo_BatchTable.LastActivity, wfo_BatchTable.BatchTimeFinished, wfo_BatchTable.CurrentWFO, wfo_BatchTable.JobStatusText, "
                        strsql = strsql + "wfo_BatchTable.ExtraData1, wfo_BatchTable.ExtraData2, wfo_BatchTable.ExtraData3, wfo_BatchTable.ExtraData4, wfo_BatchTable.ExtraData5, wfo_BatchTable.OperatorId  "
                        strsql = strsql + ", quo_Quotes.QuoteId, quo_Quotes.RequestedDeliveryDate, quo_Quotes.DateIssued,  wfo_BatchTable.SystemId"
                        strsql = strsql + " FROM wfo_BatchTable INNER JOIN quo_Quotes ON wfo_BatchTable.JobId = quo_Quotes.QuoteNum "
                        strsql = strsql + " where wfo_BatchTable.Programid in ( " + strQuotesJobId + ")  " & strDateString & " AND wfo_BatchTable.ParentId = 0 " & strfilter & " order by wfo_BatchTable.JobID desc"



                    End If


                    strconn = Session("_VT_DotNetConnString")
                    Dim myConnection As New SqlConnection(strconn)
                    Dim comSQL As New SqlCommand(strsql, myConnection)

                    comSQL.CommandTimeout = 300

                    Dim myAdapter As New SqlDataAdapter(comSQL)

                    myConnection.Open()
                    myAdapter.Fill(dt)

                    GetQuotesForDates = dt


                    '  Close the connection when done with it.
                    myConnection.Close()



                End Function




                Public Sub SaveQuoteProductClassification(ByVal QuoteItemId As Integer, ByVal ProductId As Integer, ByVal strPhysicalForm As String, ByVal strEWC As String, ByVal strHazard As String, _
                    ByVal strUNNum As String, ByVal strProperShipping As String, ByVal strPriHazard As String, ByVal strSecHazard As String, ByVal strPackingGroup As String)


                    Dim objMatrix As New VTDBFunctions.VTDBFunctions.VTMatrixFunctions
                    Dim intProductForm As Integer
                    Dim intQuoteItemRow As Integer


                    'Set product for and header info
                    intProductForm = objMatrix.GetFormId(Session("_VT_DotNetConnString"), VT_CommonFunctions.MatrixFunctions.QuoteFormListTable, "Products", Me.CurrentSession.VT_QuoteMatrixID)
                    objMatrix.InsertVTMatrixFormDataItem(Session("_VT_DotNetConnString"), VT_CommonFunctions.MatrixFunctions.QuoteFormDataTable, intProductForm, "WorkflowId", Me.CurrentSession.VT_QuoteNum)
                    objMatrix.InsertVTMatrixFormDataItem(Session("_VT_DotNetConnString"), VT_CommonFunctions.MatrixFunctions.QuoteFormDataTable, intProductForm, "ParentId", Me.CurrentSession.VT_QuoteMatrixID)
                    objMatrix.InsertVTMatrixFormDataItem(Session("_VT_DotNetConnString"), VT_CommonFunctions.MatrixFunctions.QuoteFormDataTable, intProductForm, "ItemId", intProductForm)

                    'Add form for Quote ItemId
                    intQuoteItemRow = objMatrix.GetFormId(Session("_VT_DotNetConnString"), VT_CommonFunctions.MatrixFunctions.QuoteFormListTable, CStr(QuoteItemId), intProductForm)

                    objMatrix.InsertVTMatrixFormDataItem(Session("_VT_DotNetConnString"), VT_CommonFunctions.MatrixFunctions.QuoteFormDataTable, intQuoteItemRow, "WorkflowId", Me.CurrentSession.VT_QuoteNum)
                    objMatrix.InsertVTMatrixFormDataItem(Session("_VT_DotNetConnString"), VT_CommonFunctions.MatrixFunctions.QuoteFormDataTable, intQuoteItemRow, "ParentId", Me.CurrentSession.VT_QuoteMatrixID)
                    objMatrix.InsertVTMatrixFormDataItem(Session("_VT_DotNetConnString"), VT_CommonFunctions.MatrixFunctions.QuoteFormDataTable, intQuoteItemRow, "ItemId", intQuoteItemRow)

                    objMatrix.InsertVTMatrixFormDataItem(Session("_VT_DotNetConnString"), VT_CommonFunctions.MatrixFunctions.QuoteFormDataTable, intQuoteItemRow, "QuoteItemIdForClass", CStr(QuoteItemId))
                    objMatrix.InsertVTMatrixFormDataItem(Session("_VT_DotNetConnString"), VT_CommonFunctions.MatrixFunctions.QuoteFormDataTable, intQuoteItemRow, "ProductIdForClass", CStr(ProductId))
                    objMatrix.InsertVTMatrixFormDataItem(Session("_VT_DotNetConnString"), VT_CommonFunctions.MatrixFunctions.QuoteFormDataTable, intQuoteItemRow, "PhysicalForm", strPhysicalForm)
                    objMatrix.InsertVTMatrixFormDataItem(Session("_VT_DotNetConnString"), VT_CommonFunctions.MatrixFunctions.QuoteFormDataTable, intQuoteItemRow, "EWC", strEWC)
                    objMatrix.InsertVTMatrixFormDataItem(Session("_VT_DotNetConnString"), VT_CommonFunctions.MatrixFunctions.QuoteFormDataTable, intQuoteItemRow, "Hazard", strHazard)
                    objMatrix.InsertVTMatrixFormDataItem(Session("_VT_DotNetConnString"), VT_CommonFunctions.MatrixFunctions.QuoteFormDataTable, intQuoteItemRow, "UNNum", strUNNum)
                    objMatrix.InsertVTMatrixFormDataItem(Session("_VT_DotNetConnString"), VT_CommonFunctions.MatrixFunctions.QuoteFormDataTable, intQuoteItemRow, "ProperShipping", strProperShipping)
                    objMatrix.InsertVTMatrixFormDataItem(Session("_VT_DotNetConnString"), VT_CommonFunctions.MatrixFunctions.QuoteFormDataTable, intQuoteItemRow, "PriHazard", strPriHazard)
                    objMatrix.InsertVTMatrixFormDataItem(Session("_VT_DotNetConnString"), VT_CommonFunctions.MatrixFunctions.QuoteFormDataTable, intQuoteItemRow, "SecHazard", strSecHazard)
                    objMatrix.InsertVTMatrixFormDataItem(Session("_VT_DotNetConnString"), VT_CommonFunctions.MatrixFunctions.QuoteFormDataTable, intQuoteItemRow, "PackingGroup", strPackingGroup)


                End Sub


                Public Sub StandardWasteQuotePreProcessing(ByVal intQuoteNum As Integer)
                    Dim dsQuoteItems As New DataSet
                    Dim strProductCode As String
                    Dim strProductDesc As String
                    Dim strComment As String
                    Dim dblRowTotal As Double
                    Dim dblRowVAT As Double
                    Dim dblTotal As Double
                    Dim dblTotalVAT As Double
                    Dim dsProduct As New DataSet
                    Dim dsTraceCode As New DataSet
                    Dim dblQuantity As Double
                    Dim dblWeight As Double
                    Dim dtDeliveryDate As Date
                    Dim dblPriceCharged As Double
                    Dim intQuoteId As Integer
                    Dim objProds As New VTDBFunctions.VTDBFunctions.ProductsFunctions
                    Dim objQuotes As New VTDBFunctions.VTDBFunctions.QuoteFunctions
                    Dim dtQuoteHeader As New DataTable
                    Dim strPackageType As String
                    Dim dblUnitPrice As Double

                    Dim PhysicalForm As String
                    Dim EWC As String
                    Dim Hazard As String
                    Dim UNNum As String
                    Dim ProperShipping As String
                    Dim PriHazard As String
                    Dim SecHazard As String
                    Dim PackingGroup As String
                    Dim intQuoteItemId As Integer

                    Dim dteDateIssued As Date
                    Dim dteRequestedDeliveryDate As Date

                    Dim objMatrix As New VTDBFunctions.VTDBFunctions.VTMatrixFunctions



                    objQuotes.ClearReportTable(Session("_VT_CurrentUserId"))

                    dtQuoteHeader = objQuotes.GetQuoteHeaderInfoForQuoteNum(intQuoteNum)

                    If dtQuoteHeader.Rows.Count < 1 Then
                        Exit Sub
                    End If


                    Dim defaultdate As New DateTime(2000, 1, 1)

                    dteDateIssued = If(IsDBNull(dtQuoteHeader.Rows(0).Item("DateIssued")), defaultdate, dtQuoteHeader.Rows(0).Item("DateIssued"))
                    dteRequestedDeliveryDate = If(IsDBNull(dtQuoteHeader.Rows(0).Item("RequestedDeliveryDate")), defaultdate, dtQuoteHeader.Rows(0).Item("RequestedDeliveryDate"))


                    intQuoteId = dtQuoteHeader.Rows(0).Item("QuoteId")

                    Dim objCust As New VTDBFunctions.VTDBFunctions.CustomerFunctions
                    Dim dsCust As DataSet = objCust.GetCustomerForId(dtQuoteHeader.Rows(0).Item("CustomerId"))
                    Dim strCustomer As String = If(IsDBNull(dsCust.Tables(0).Rows(0).Item("CustomerName")), "", dsCust.Tables(0).Rows(0).Item("CustomerName"))
                    Dim strBillingAddress As String = If(IsDBNull(dsCust.Tables(0).Rows(0).Item("BillingAddress")), "", dsCust.Tables(0).Rows(0).Item("BillingAddress"))
                    Dim strCustomerContact As String = If(IsDBNull(dsCust.Tables(0).Rows(0).Item("ContactName")), "", dsCust.Tables(0).Rows(0).Item("ContactName"))


                    'Get all the items for the quote
                    dsQuoteItems = objQuotes.GetQuoteItemsForId(intQuoteId)

                    dblTotal = 0
                    dblTotalVAT = 0

                    Dim i As Integer

                    'for each transaction in the dataset 
                    For Each drItemRow In dsQuoteItems.Tables(0).Rows


                        '	QuoteItemId	
                        '	QuoteId	int	4	1
                        '	ProductId	int	4	1
                        '	Quantity	float	8	1
                        '	Weight	float	8	1
                        '	Comment	nvarchar	2000	1
                        '	UnitPrice	float	8	1
                        '	VATRate	float	8	1
                        '	VAT	float	8	1
                        '	TotalPrice	float	8	1
                        '	Status	nvarchar	50	1
                        '	ChargedByType	nvarchar	20	1
                        '	PackageTypeId	int	4	1
                        '	PackageType	nvarchar	50	1


                        'get the Product code
                        'get the product Description
                        dsProduct = objProds.GetProductForId(drItemRow.Item("ProductId"))
                        strProductCode = dsProduct.Tables(0).Rows(0).Item("Catalog_Number")
                        strProductDesc = dsProduct.Tables(0).Rows(0).Item("Product_Name")

                        'get the comment
                        strComment = If(IsDBNull(drItemRow.item("Comment")), "", Trim(drItemRow.item("Comment")))

                        'Package Type
                        strPackageType = If(IsDBNull(drItemRow.item("PackageType")), "", Trim(drItemRow.item("PackageType")))

                        'Weight
                        dblWeight = drItemRow.item("Weight")
                        dblUnitPrice = drItemRow.item("UnitPrice")


                        dblRowVAT = drItemRow.item("VAT")
                        dblRowTotal = drItemRow.item("TotalPrice")

                        dblRowTotal = FormatNumber(dblRowTotal, 2)
                        dblRowVAT = FormatNumber(dblRowVAT, 2)

                        'Add to overall total 
                        dblTotal = dblTotal + dblRowTotal
                        'add to overall VAT
                        dblTotalVAT = dblTotalVAT + dblRowVAT
                        'write row record to trc_DocketReport table


                        Dim dsClassificationItems As Data.DataSet
                        intQuoteItemId = drItemRow.item("QuoteItemId")

                        'IF the quote item has been saved it will be in the matrix


                        ' get the Materials items fom the grid
                        Dim intNumFields As Integer = 8
                        Dim astrItems(intNumFields - 1) As String


                        astrItems(0) = "PhysicalForm"
                        astrItems(1) = "EWC"
                        astrItems(2) = "Hazard"
                        astrItems(3) = "UNNum"
                        astrItems(4) = "ProperShipping"
                        astrItems(5) = "PriHazard"
                        astrItems(6) = "SecHazard"
                        astrItems(7) = "PackingGroup"

                        dsClassificationItems = objMatrix.GetVTMatrixData(Session("_VT_DotNetConnString"), VT_CommonFunctions.MatrixFunctions.QuoteFormDataTable, intNumFields, astrItems, _
                                                                          "QuoteItemIdForClass", CStr(intQuoteItemId))

                        If dsClassificationItems.Tables(0).Rows.Count > 0 Then
                            With dsClassificationItems.Tables(0).Rows(0)


                                PhysicalForm = .Item("Field0").ToString
                                EWC = .Item("Field1").ToString
                                Hazard = .Item("Field2").ToString
                                UNNum = .Item("Field3").ToString
                                ProperShipping = .Item("Field4").ToString
                                PriHazard = .Item("Field5").ToString
                                SecHazard = .Item("Field6").ToString
                                PackingGroup = .Item("Field7").ToString

                            End With
                        Else
                            PhysicalForm = ""
                            EWC = ""
                            Hazard = ""
                            UNNum = ""
                            ProperShipping = ""
                            PriHazard = ""
                            SecHazard = ""
                            PackingGroup = ""
                        End If




                        objQuotes.InsertQuotePrintRecord(intQuoteId, intQuoteNum, strCustomer, strCustomerContact, strBillingAddress, strProductCode, strProductDesc, strPackageType, dteDateIssued, _
                                                         dteRequestedDeliveryDate, dblWeight, 0, dblUnitPrice, 0, dblRowTotal, dblRowVAT, 0, 0, Session("_VT_CurrentUserId"), strComment, _
                                                         PhysicalForm, EWC, Hazard, UNNum, ProperShipping, PriHazard, SecHazard, PackingGroup)

                    Next



                    '            3	QuoteId	int	4	0
                    '0	QuoteNum	int	4	1
                    '0	CustomerId	int	4	1
                    '0	CustomerContactId	int	4	1
                    '0	ContactName	nvarchar	100	1
                    '0	DeliveryCustomerId	int	4	1
                    '0	RequestedDeliveryDate	datetime	8	1
                    '0	PersonLoggingQuote	int	4	1
                    '0	Comment	nvarchar	500	1
                    '0	DateCreated	datetime	8	1
                    '0	CustomerPO	nvarchar	50	1
                    '0	PersonLoggingName	nvarchar	50	1
                    '0	Status	nvarchar	100	1
                    '0	TotalValue	float	8	1
                    '0	TimeCreated	nvarchar	50	1
                    '0	DateCompleted	datetime	8	1
                    '0	SalesaOrderNum	int	4	1
                    '0	WorkflowId	int	4	1
                    '0	Route	nvarchar	50	1
                    '0	DateIssued	datetime	8	1






                    'write totals row to quo_DocketReport table

                    strComment = IIf(IsDBNull(dtQuoteHeader.Rows(0).Item("Comment")), "", dtQuoteHeader.Rows(0).Item("Comment"))

                    Dim dsPrintItems As Data.DataSet
                    Dim strHeader As String
                    Dim strBody As String
                    Dim strFooter As String


                    ' get the Materials items fom the grid
                    Dim intNumFieldsH As Integer = 4
                    Dim astrItemsH(intNumFieldsH - 1) As String

                    astrItemsH(0) = "PrintQuoteNum"
                    astrItemsH(1) = "txtQuoteHeader"
                    astrItemsH(2) = "txtQuoteBody"
                    astrItemsH(3) = "txtQuoteFooter"


                    dsPrintItems = objMatrix.GetVTMatrixData(Session("_VT_DotNetConnString"), VT_CommonFunctions.MatrixFunctions.QuoteFormDataTable, intNumFieldsH, astrItemsH, _
                                                                          "PrintQuoteNum", CStr(Me.CurrentSession.VT_QuoteNum))


                    If dsPrintItems.Tables(0).Rows.Count > 0 Then
                        strHeader = dsPrintItems.Tables(0).Rows(0).Item("Field1").ToString
                        strBody = dsPrintItems.Tables(0).Rows(0).Item("Field2").ToString
                        strFooter = dsPrintItems.Tables(0).Rows(0).Item("Field3").ToString
                    Else
                        strHeader = ""
                        strBody = ""
                        strFooter = ""
                    End If



                    objQuotes.InsertQuotePrintRecord(intQuoteId, intQuoteNum, strCustomer, strCustomerContact, strBillingAddress, "", "", "", dteDateIssued, dteRequestedDeliveryDate, 0, 0, 0, 1, 0, 0, dblTotal, dblTotalVAT, _
                                                     Session("_VT_CurrentUserId"), strComment, "", "", "", "", "", "", "", "", "", "", "", "", "", "", strHeader, strBody, strFooter)

                End Sub


            End Class
        End Namespace


