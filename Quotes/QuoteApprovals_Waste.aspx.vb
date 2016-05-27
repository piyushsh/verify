Imports BPADotNetCommonFunctions
Imports BPADotNetCommonFunctions.VT_CommonFunctions.MatrixFunctions
Imports System.Data.SqlClient
Imports System.Data
Imports System.Globalization


Partial Class Quotes_QuoteApprovals_Waste
    Inherits MyBasePage

    Const cView = 0
    Const cReference = 1
    Const cName = 2


    ''' <summary>
    ''' This function validates the data on the form before it can be saved
    ''' </summary>
    ''' <returns>False if there is a Validation problem, True otherwise</returns>
    ''' <remarks></remarks>
    Public Function ValidateMe(ByVal ParamArray aParams() As Object) As String
        Dim objDisp As New VT_Display.DisplayFuncs
        Dim mpContentPlaceHolder As ContentPlaceHolder

        ValidateMe = ""

        mpContentPlaceHolder = aParams(0)

        ' sample code

        'If CType(mpContentPlaceHolder.FindControl("txtSetupLm"), TextBox).Text = "" Then
        '    ValidateMe = "You must enter the Setup LM value"
        '    Exit Function
        'End If



    End Function

    ''' <summary>
    ''' This function is for any special data items stored after the ScrapeFormData in the SavePageData function
    ''' </summary>
    ''' <param name="intItemToSaveId"></param>
    ''' <remarks></remarks>
    Sub StoreFormSpecificData(ByVal intItemToSaveId As Integer)

        Dim objData As New BPADotNetCommonFunctions.VT_DataPreserve.DataPreserve
        Dim objVT As New VTDBFunctions.VTDBFunctions.VTMatrixFunctions


        Dim Inner_CP As ContentPlaceHolder
        Inner_CP = GetInnerContentPlaceHolder()

        objData.ScrapeFormData(Inner_CP, Me.CurrentSession.VT_QuoteMatrixID, Me.CurrentSession.VT_QuoteSelectedFormName, QuoteFormListTable, QuoteFormDataTable, Me.CurrentSession.VT_tlsNumFields)


        ' store the item id
        objVT.InsertVTMatrixDataItem(Session("_VT_DotNetConnString"), QuoteFormListTable, QuoteFormDataTable, Me.CurrentSession.VT_QuoteMatrixID, Me.CurrentSession.VT_QuoteSelectedFormName, "ParentId", Me.CurrentSession.VT_QuoteMatrixID)


    End Sub

    Function GetInnerContentPlaceHolder() As ContentPlaceHolder
        'Because we are in a nested Master we need to go to the Outer Master first
        'Outer Master Page
        Dim Outer_CP As ContentPlaceHolder
        Outer_CP = TryCast(Me.Master.Master.FindControl("ProjectMAIN_content"), ContentPlaceHolder)

        ' Inner Master Page
        Dim Inner_CP As ContentPlaceHolder
        Inner_CP = TryCast(Outer_CP.FindControl("DetailsMaster_ContentPlaceHolder"), ContentPlaceHolder)

        GetInnerContentPlaceHolder = Inner_CP
    End Function

    ''' <summary>
    '''This function returns whether we should save the current page or not. The Default is to save
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function DataToSave(ByVal ParamArray aParams() As Object) As Boolean

        DataToSave = True

    End Function

    ''' <summary>
    ''' This function checks if there is any reason why the form should not be submitted
    ''' </summary>
    ''' <returns>A string with the text of the problem if there is a  problem, an empty string otherwise</returns>
    ''' <remarks></remarks>
    Public Function CanSubmit(ByVal ParamArray aParams() As Object) As String

        CanSubmit = ""
    End Function


    ''' <summary>
    ''' This function checks if there is any reason why the form should not be Signed Off
    ''' </summary>
    ''' <returns>A string with the text of the problem if there is a  problem, an empty string otherwise</returns>
    ''' <remarks></remarks>
    Public Function CanSignOff(ByVal ParamArray aParams() As Object) As String

        CanSignOff = ""


    End Function

    Sub SetupForPrinting()
        'btnAdd.Visible = False
        'btnEdit.Visible = False
        ''uwgHBagMCStartTestResults.DisplayLayout.FrameStyle.BorderStyle = BorderStyle.None
        ''uwgHBagMCStartTestResults.DisplayLayout.RowStyleDefault.BorderStyle = BorderStyle.None

        'uwgBakingChecks.DisplayLayout.StationaryMargins = Infragistics.WebUI.UltraWebGrid.StationaryMargins.No
        'uwgBakingChecks.DisplayLayout.StationaryMargins = Infragistics.WebUI.UltraWebGrid.StationaryMargins.No

    End Sub


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        cmdOKPass.OnClientClick = String.Format("return Close('{0}','{1}')", cmdOKPass.UniqueID, "")

        btnHSApprove.Attributes.Add("onclick", "return ShowPasswordPanel('HSApprove');")
        btnHSReject.Attributes.Add("onclick", "return ShowPasswordPanel('HSReject');")
        btnClassApprove.Attributes.Add("onclick", "return ShowPasswordPanel('ClassApprove');")
        btnClassReject.Attributes.Add("onclick", "return ShowPasswordPanel('ClassReject');")


        If Not IsPostBack Then
            'IF the Quote ID is blank this is a new Quote so fill with default 

            Me.CurrentSession.VT_QuoteSelectedFormName = "QuoteApprovals"

            If Me.CurrentSession.VT_QuoteID = 0 Then

            Else
                FillData(Me.CurrentSession.VT_QuoteID)
            End If


            'lbl_WorkOrderID.Text = Me.CurrentSession.VT_CurrentJobWorkflowID.ToString


            Dim objData As New BPADotNetCommonFunctions.VT_DataPreserve.DataPreserve

            Dim Inner_CP As ContentPlaceHolder
            Inner_CP = GetInnerContentPlaceHolder()

            objData.RecoverFormData(Inner_CP, Me.CurrentSession.VT_QuoteSelectedFormName, Me.CurrentSession.VT_QuoteMatrixID, QuoteFormListTable, QuoteFormDataTable)

        End If



    End Sub




    Sub FillData(ByVal OrderId As Integer)

        Dim objeqOInterface As New VT_eQOInterface.eQOInterface
        Dim dsTasks As New DataSet
        Dim sender As Object
        Dim e As System.EventArgs


        If Me.CurrentSession.VT_QuoteNum <> 0 Then

            dsTasks = objeqOInterface.GetTasksForJob(Me.CurrentSession.VT_QuoteNum)

        End If

        uwgTasks_NS.DataSource = dsTasks
        uwgTasks_NS.DataBind()


    End Sub


    ''' <summary>
    ''' This function validates the data on the form before it can be saved
    ''' </summary>
    ''' <returns>If there is a Validation problem return a message, Empty String otherwise</returns>
    ''' <remarks></remarks>
    Public Function ValidateMe() As String
        Dim objDisp As New VT_Display.DisplayFuncs
        Dim mpContentPlaceHolder As ContentPlaceHolder

        ValidateMe = ""

        mpContentPlaceHolder = CType(Master.FindControl("DetailsMaster_ContentPlaceHolder"), ContentPlaceHolder)

        ' sample code

        'If CType(mpContentPlaceHolder.FindControl(Me.CurrentSession.PrimaryItemName), TextBox).Text = "" Then
        '    ValidateMe = Me.CurrentSession.PrimaryItemMessage
        '    Exit Function
        'Else
        '    Session("_VT_SelectedItemName") = CType(mpContentPlaceHolder.FindControl(Me.CurrentSession.PrimaryItemName), TextBox).Text
        '    CType(Master.FindControl("lblDetailsItemName"), Label).Text = Me.CurrentSession.SelectedItemName
        '    CType(Master.FindControl("lblItemData"), Label).Text = Me.CurrentSession.SelectedItemName
        'End If



    End Function

    ''' <summary>
    '''This function returns whether we should save the current page or not. The Default is to save
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function DataToSave() As Boolean

        DataToSave = True

    End Function





    Protected Sub cmdOKPass_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles cmdOKPass.Click
        ' check the password
        Dim objP As New PersonnelModuleFunctions

        If Not objP.IsPasswordValid(Session("_VT_DotNetConnString"), Session("_VT_CurrentUserId"), txtPassword.Text) Then
            lblInfo.Text = GetGlobalResourceObject("resource", "InvalidPassword")
            ModalPopupExtenderMsg.Show()
        Else

            Select Case hdnWhichButton.Value
                Case "HSApprove"
                    txtHSStatus.Text = "Approved"
                    txtHSCompletedBy.Text = Trim(Session("_VT_CurrentUserName"))
                    txtHSDate.Text = Now

                Case "HSReject"
                    txtHSStatus.Text = "Reject"
                    txtHSCompletedBy.Text = Trim(Session("_VT_CurrentUserName"))
                    txtHSDate.Text = Now

                Case "ClassApprove"
                    txtClassStatus.Text = "Approved"
                    txtClassCompletedBy.Text = Trim(Session("_VT_CurrentUserName"))
                    txtClassDate.Text = Now

                Case "ClassReject"
                    txtClassStatus.Text = "Reject"
                    txtClassCompletedBy.Text = Trim(Session("_VT_CurrentUserName"))
                    txtClassDate.Text = Now

            End Select


        End If
    End Sub
End Class
