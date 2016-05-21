Imports BPADotNetCommonFunctions
Imports BPADotNetCommonFunctions.PersonnelModuleFunctions
Imports BPADotNetCommonFunctions.VT_CommonFunctions.MatrixFunctions
Imports System.Drawing
Imports VTDBFunctions.VTDBFunctions
Imports BPADotNetCommonFunctions.VT_CommonFunctions
Imports System.Data

Partial Class FormsMaster
    Inherits MyMasterPage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ' register the Add buttons with the ScriptManager
        '   ScriptManager1.RegisterAsyncPostBackControl(btnCommentAdd)

        cmdOk.OnClientClick = String.Format("AddMasterClickUpdate('{0}','{1}')", cmdOk.UniqueID, "")

        cmdOKSaveChanges.OnClientClick = String.Format("ClickUpdate('{0}','{1}')", cmdOKSaveChanges.UniqueID, "")
        cmdNoSaveChanges.OnClientClick = String.Format("ClickUpdate('{0}','{1}')", cmdNoSaveChanges.UniqueID, "")
        cmdCancelSaveChanges.OnClientClick = String.Format("ClickUpdate('{0}','{1}')", cmdCancelSaveChanges.UniqueID, "")

        btnCommentAdd.Attributes.Add("onclick", "return LoadCommentPanel();")
        btnCommentClear.Attributes.Add("onclick", "return NoComment();")
        btnCommentNEXT.Attributes.Add("onclick", "return NoComment();")
        btnCommentPrevious.Attributes.Add("onclick", "return NoComment();")


        btnApprove.Attributes.Add("onclick", "return ShowPasswordModalPopup('Approve')")
        btnReject.Attributes.Add("onclick", "return ShowPasswordModalPopup('Reject')")


        cmdOKPass.OnClientClick = String.Format("return PasswordClickUpdate('{0}','{1}')", cmdOKPass.UniqueID, "")

        'btnSetStatus.Attributes.Add("onclick", "return ShowStatusOptions();")
        'btnOfferOptions.Attributes.Add("onclick", "return ShowOfferOptions();")



        If Not IsPostBack Then



            Dim astrOptions() As String = Me.CurrentSession.aVT_DetailsPageOptions
            Dim astrOptionsPages() As String = Me.CurrentSession.aVT_DetailsPageOptionsPages


            uwgOptions.Rows.Clear()



            For i As Integer = LBound(astrOptions) To UBound(astrOptions)
                uwgOptions.Rows.Add()
                uwgOptions.Rows(i).Cells.FromKey("Option").Text = astrOptions(i)
                uwgOptions.Rows(i).Cells.FromKey("OptionPage").Text = astrOptionsPages(i)
                uwgOptions.Rows(i).DataKey = astrOptions(i)
                ' ensure the correct page is selected on the selection grid
                If Not Me.CurrentSession.VT_AppPageToShow Is Nothing Then
                    If InStr(uwgOptions.Rows(i).Cells.FromKey("OptionPage").Text, Me.CurrentSession.VT_AppPageToShow) Then
                        Me.CurrentSession.VT_SelectedDetailsGridRow = uwgOptions.Rows(i)
                        Me.CurrentSession.VT_AppPageToShow = Nothing
                    End If

                End If
            Next i

            uwgOptions.Height = (4 + uwgOptions.Rows.Count) * uwgOptions.DisplayLayout.RowHeightDefault.Value


            If Me.CurrentSession.VT_SelectedDetailsGridRow Is Nothing Then
                uwgOptions.DisplayLayout.ActiveRow = uwgOptions.Rows(0)
                Me.CurrentSession.VT_SelectedDetailsGridRow = uwgOptions.Rows(0)
            Else
                uwgOptions.DisplayLayout.ActiveRow = Me.CurrentSession.VT_SelectedDetailsGridRow
            End If



            ' load the NoControlForComment text from the resource file into 
            ' the hidden field from where the script can access it
            hdnCommentPrompt.Value = GetGlobalResourceObject("Resource", "NoControlForCommentToAdd")
            hdnNoCommentPrompt.Value = GetGlobalResourceObject("Resource", "NoCommentSupplied")
            hdnNoCommentSelected.Value = GetGlobalResourceObject("Resource", "NoCommentSelected")
            hdnClearACommentPrompt.Value = GetGlobalResourceObject("Resource", "ClearACommentPrompt")
            hdnClearAllCommentsPrompt.Value = GetGlobalResourceObject("Resource", "ClearAllCommentsPrompt")
            hdnNoCourseSelected.Value = GetGlobalResourceObject("Resource", "NoCourseSelected")




            Me.CurrentSession.FormDataTable = QuoteFormDataTable
            Me.CurrentSession.FormListTable = QuoteFormListTable

            ' read the X co-ordinate for the floating panel from web.config
            hdnFloatingX.Value = System.Configuration.ConfigurationManager.AppSettings("FloatingX")

            ' Load The comments into the page controls
            Dim intIdToUse As Integer
          
                intIdToUse = Me.CurrentSession.SelectedItemID


            If intIdToUse <> 0 Then
                SetupComments(intIdToUse)
            End If


            ' if we got here using the GoToComment function we must set the focus
            If Me.CurrentSession.VT_ControlToJumpTo <> "" Then
                Dim TheControl As Control
                TheControl = FindContentPageControl(Me.CurrentSession.VT_ControlToJumpTo)
                Me.CurrentSession.VT_ControlToJumpTo = ""
                TheControl.Focus()
            End If

            ' store the current status in a hidden field
            hdnCurrentStatus.Value = Me.CurrentSession.SelectedItemStatus

            ' set up the labels on the Master page
            'lblCategoryText.Text = Me.CurrentSession.BasicModuleItem + " Category:"
            'lblItemText.Text = Me.CurrentSession.BasicModuleItem + " Name:"
            'If Me.CurrentSession.SelectedCategory <> "" Then
            '    lblCategoryData.Text = Me.CurrentSession.SelectedCategory
            'End If
            'If Me.CurrentSession.SelectedItemName <> "" Then
            '    lblItemData.Text = Me.CurrentSession.SelectedItemName
            lblDetailsItemName.Text = Me.CurrentSession.SelectedItemName

            'End If





            SetUpUI()



        End If
    End Sub

    Public Sub SetItemStatusDisplay()
        lblStatusDisplay.Text = Me.currentsession.VT_CurrentQuoteStatus

        lblDetailsItemName.Text = Me.currentsession.VT_Quotenum
          
    

    End Sub

    Sub SetupComments(ByVal intIdToUse As Integer)
        Dim objApp As New CommentFunctions
        Dim dtComments As Data.DataTable = objApp.GetThreads(intIdToUse)
        Dim strPage As String = uwgOptions.DisplayLayout.ActiveRow.Cells.FromKey("OptionPage").Text
        LoadCommentsOnPage(dtComments, strPage)
        If dtComments.Rows.Count > 0 Then
            imgComments.ImageUrl = "~/App_Themes/Buttons/Comments-Header - Green.jpg"
        Else
            imgComments.ImageUrl = "~/App_Themes/Buttons/Comments-Header.gif"
        End If

    End Sub
    Sub LoadCommentsOnPage(ByVal dtComments As Data.DataTable, ByVal strPage As String)
        Dim TheControl As Control
        For Each dr As Data.DataRow In dtComments.Rows
            ' if the Page for this comment (Field6) is the current page 
            If InStr(strPage, dr.Item("Field6")) <> 0 Then
                ' if the comment's DateCleared(Field4) is empty
                If IsDBNull(dr.Item("Field4")) OrElse dr.Item("Field4") = "" Then
                    ' find the control given by the comment's Control item (Field7)
                    If Not IsDBNull(dr.Item("Field7")) AndAlso dr.Item("Field7") <> "" Then
                        TheControl = FindContentPageControl(dr.Item("Field7"))
                        ' Display the Comment (Field1) with the control
                        ApplyCommentToControl(TheControl, dr.Item("Field1"))
                    End If
                End If
            End If
        Next

    End Sub

    Sub ApplyCommentToControl(ByVal ThisControl As Control, ByVal strComment As String)
        Dim cBorderColor As System.Drawing.Color = Drawing.Color.Green
        Dim cBorderStyle As BorderStyle = BorderStyle.Solid
        Dim cBorderWidth As Integer = 2

        If ThisControl.GetType.Equals(GetType(TextBox)) Then
            CType(ThisControl, TextBox).ToolTip = strComment
            CType(ThisControl, TextBox).BorderColor = cBorderColor
            CType(ThisControl, TextBox).BorderStyle = cBorderStyle
            CType(ThisControl, TextBox).BorderWidth = cBorderWidth
        ElseIf ThisControl.GetType.Equals(GetType(DropDownList)) Then
            CType(ThisControl, DropDownList).ToolTip = strComment
            CType(ThisControl, DropDownList).BackColor = cBorderColor
        ElseIf ThisControl.GetType.Equals(GetType(RadioButton)) Then
            CType(ThisControl, RadioButton).ToolTip = strComment
            CType(ThisControl, RadioButton).BorderColor = cBorderColor
            CType(ThisControl, RadioButton).BorderStyle = cBorderStyle
            CType(ThisControl, RadioButton).BorderWidth = cBorderWidth
        ElseIf ThisControl.GetType.Equals(GetType(RadioButtonList)) Then
            CType(ThisControl, RadioButtonList).ToolTip = strComment
            CType(ThisControl, RadioButtonList).BackColor = cBorderColor
        ElseIf ThisControl.GetType.Equals(GetType(CheckBox)) Then
            CType(ThisControl, CheckBox).ToolTip = strComment
            CType(ThisControl, CheckBox).BorderColor = cBorderColor
            CType(ThisControl, CheckBox).BorderStyle = cBorderStyle
            CType(ThisControl, CheckBox).BorderWidth = cBorderWidth
        ElseIf ThisControl.GetType.Equals(GetType(CheckBoxList)) Then
            CType(ThisControl, CheckBoxList).ToolTip = strComment
            CType(ThisControl, CheckBoxList).BackColor = cBorderColor
        ElseIf ThisControl.GetType.Equals(GetType(ListBox)) Then
            CType(ThisControl, ListBox).ToolTip = strComment
            CType(ThisControl, ListBox).BackColor = cBorderColor
        ElseIf ThisControl.GetType.Equals(GetType(Infragistics.WebUI.WebDataInput.WebDateTimeEdit)) Then
            CType(ThisControl, Infragistics.WebUI.WebDataInput.WebDateTimeEdit).ToolTip = strComment
            CType(ThisControl, Infragistics.WebUI.WebDataInput.WebDateTimeEdit).BorderColor = cBorderColor
            CType(ThisControl, Infragistics.WebUI.WebDataInput.WebDateTimeEdit).BorderStyle = cBorderStyle
            CType(ThisControl, Infragistics.WebUI.WebDataInput.WebDateTimeEdit).BorderWidth = cBorderWidth
        ElseIf ThisControl.GetType.Equals(GetType(Infragistics.WebUI.WebDataInput.WebMaskEdit)) Then
            CType(ThisControl, Infragistics.WebUI.WebDataInput.WebMaskEdit).ToolTip = strComment
            CType(ThisControl, Infragistics.WebUI.WebDataInput.WebMaskEdit).BorderColor = cBorderColor
            CType(ThisControl, Infragistics.WebUI.WebDataInput.WebMaskEdit).BorderStyle = cBorderStyle
            CType(ThisControl, Infragistics.WebUI.WebDataInput.WebMaskEdit).BorderWidth = cBorderWidth
        ElseIf ThisControl.GetType.Equals(GetType(Infragistics.WebUI.WebDataInput.WebNumericEdit)) Then
            CType(ThisControl, Infragistics.WebUI.WebDataInput.WebNumericEdit).ToolTip = strComment
            CType(ThisControl, Infragistics.WebUI.WebDataInput.WebNumericEdit).BorderColor = cBorderColor
            CType(ThisControl, Infragistics.WebUI.WebDataInput.WebNumericEdit).BorderStyle = cBorderStyle
            CType(ThisControl, Infragistics.WebUI.WebDataInput.WebNumericEdit).BorderWidth = cBorderWidth
        ElseIf ThisControl.GetType.Equals(GetType(Infragistics.WebUI.WebDataInput.WebTextEdit)) Then
            CType(ThisControl, Infragistics.WebUI.WebDataInput.WebTextEdit).ToolTip = strComment
            CType(ThisControl, Infragistics.WebUI.WebDataInput.WebTextEdit).BorderColor = cBorderColor
            CType(ThisControl, Infragistics.WebUI.WebDataInput.WebTextEdit).BorderStyle = cBorderStyle
            CType(ThisControl, Infragistics.WebUI.WebDataInput.WebTextEdit).BorderWidth = cBorderWidth
        ElseIf ThisControl.GetType.Equals(GetType(Infragistics.WebUI.UltraWebGrid.UltraWebGrid)) Then
            CType(ThisControl, Infragistics.WebUI.UltraWebGrid.UltraWebGrid).ToolTip = strComment
            CType(ThisControl, Infragistics.WebUI.UltraWebGrid.UltraWebGrid).BorderColor = cBorderColor
            CType(ThisControl, Infragistics.WebUI.UltraWebGrid.UltraWebGrid).BorderStyle = cBorderStyle
            CType(ThisControl, Infragistics.WebUI.UltraWebGrid.UltraWebGrid).BorderWidth = cBorderWidth
        End If



    End Sub


    Function FindContentPageControl(ByVal strControlName) As Control
        Dim Inner_CP As ContentPlaceHolder
        Inner_CP = GetInnerContentPlaceHolder()

        FindContentPageControl = Inner_CP.FindControl(strControlName)

    End Function

    Function GetInnerContentPlaceHolder() As ContentPlaceHolder
        ' Because we are in a nested Master we need to go to the Outer Master first
        ' Outer Master Page
        Dim Outer_CP As ContentPlaceHolder
        Outer_CP = TryCast(Me.Master.FindControl("ProjectMAIN_content"), ContentPlaceHolder)

        ' Inner Master Page
        Dim Inner_CP As ContentPlaceHolder
        Inner_CP = TryCast(Outer_CP.FindControl("DetailsMaster_ContentPlaceHolder"), ContentPlaceHolder)

        GetInnerContentPlaceHolder = Inner_CP
    End Function

    Sub SetUpUI()
    
       
        btnAudit.Visible = True
        btnAttachments.Visible = True
        btnBack.Visible = True


        btnSaveDetails.Visible = False

        btnSubmit.Visible = False
        btnSignOff.Visible = False
        btnApprove.Visible = False
        btnReject.Visible = False
        btnClose.Visible = False
        btnConvert.Visible = False
        btnSendToCustomer.Visible = False
        btnSendForProdApproval.Visible = False

        imgLockGraphic.Visible = False
         

        Dim Inner_CP As ContentPlaceHolder = GetInnerContentPlaceHolder()
        Dim objData As New BPADotNetCommonFunctions.VT_DataPreserve.DataPreserve



        ' store the current status in a hidden field
        hdnCurrentStatus.Value = Me.CurrentSession.SelectedItemStatus

        SetItemStatusDisplay()


        Select Case Trim(Me.CurrentSession.VT_CurrentQuoteStatus)

            Case GetGlobalResourceObject("Resource", "Quote_New")
                btnSaveDetails.Visible = True
                btnSubmit.Visible = True
                btnClose.Visible = True
                btnSendForProdApproval.Visible = True

            Case GetGlobalResourceObject("Resource", "Quote_SubmittedForProd")
                btnSaveDetails.Visible = True
                btnApprove.Visible = True
                btnReject.Visible = True
                btnClose.Visible = True


            Case GetGlobalResourceObject("Resource", "Quote_ProdApproved")
                btnSaveDetails.Visible = True
                btnSubmit.Visible = True
                btnClose.Visible = True

            Case GetGlobalResourceObject("Resource", "Quote_Submitted")
                btnApprove.Visible = True
                btnReject.Visible = True
                btnClose.Visible = True

            Case GetGlobalResourceObject("Resource", "Quote_ReadytoSend")
                btnSaveDetails.Visible = True
                btnSendToCustomer.Visible = True
                btnClose.Visible = True

            Case GetGlobalResourceObject("Resource", "Quote_Sent")
                btnClose.Visible = True

            Case GetGlobalResourceObject("Resource", "Quote_POReceived")
                btnClose.Visible = True
                btnConvert.Visible = False

            Case GetGlobalResourceObject("Resource", "Quote_NotSent")
                imgLockGraphic.Visible = True
                objData.EnableDisableContentPageControls(Inner_CP, False)
                imgLockGraphic.Visible = True

            Case GetGlobalResourceObject("Resource", "Quote_Unsuccesful")
                imgLockGraphic.Visible = True
                objData.EnableDisableContentPageControls(Inner_CP, False)
                imgLockGraphic.Visible = True

            Case GetGlobalResourceObject("Resource", "Quote_Converted")
                imgLockGraphic.Visible = True
                objData.EnableDisableContentPageControls(Inner_CP, False)
                imgLockGraphic.Visible = True

            Case Else
                btnSaveDetails.Visible = True

        End Select

    End Sub


    Protected Sub btnAttachments_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnAttachments.Click
     Dim strVTStore As String = Replace(ConfigurationManager.AppSettings("VTStore"), "XXXX", Session("_VT_CurrentDID"))
        Dim StartNodeText As String = "Quotes\QuoteNum_" + Me.CurrentSession.VT_QuoteNum.ToString
        strVTStore = strVTStore + "&Auth=Ok&UID=" + Session("_VT_CurrentUserId").ToString + "&ShowBanner=No&StartNodeText=" + StartNodeText + "&UserName=" + Session("_VT_CurrentUserName")


        Session("_VT_PageToReturnToFromModulesIFrame") = Me.CurrentSession.VT_SelectedFormPath


        Me.CurrentSession.VT_ProjectInIFrame = strVTStore
        Response.Redirect("~/IFrame Pages/ModulesIFrame.aspx")

    End Sub


    Private Function CallContentPageBooleanFunction(ByVal FunctionName As String, Optional ByVal aParamArray() As String = Nothing) As Boolean
        '**********************************

        Dim Outer_CP As ContentPlaceHolder
        Outer_CP = TryCast(Me.Master.FindControl("ProjectMAIN_content"), ContentPlaceHolder)

        ' Inner Master Page
        Dim Inner_CP As ContentPlaceHolder
        Inner_CP = TryCast(Outer_CP.FindControl("DetailsMaster_ContentPlaceHolder"), ContentPlaceHolder)

        CallContentPageBooleanFunction = CallByName(Inner_CP.Page, FunctionName, vbMethod, aParamArray)
    End Function
    Private Function CallContentPageStringFunction(ByVal FunctionName As String, Optional ByVal aParamArray() As String = Nothing) As String

        '**********************************
    
        Dim Outer_CP As ContentPlaceHolder
        Outer_CP = TryCast(Me.Master.FindControl("ProjectMAIN_content"), ContentPlaceHolder)

        ' Inner Master Page
        Dim Inner_CP As ContentPlaceHolder
        Inner_CP = TryCast(Outer_CP.FindControl("DetailsMaster_ContentPlaceHolder"), ContentPlaceHolder)


        CallContentPageStringFunction = CallByName(Inner_CP.Page, FunctionName, vbMethod, aParamArray)

    End Function
    Private Sub CallContentPageSub(ByVal FunctionName As String, Optional ByVal aParamArray() As Object = Nothing)
        Dim Outer_CP As ContentPlaceHolder
        Outer_CP = TryCast(Me.Master.FindControl("ProjectMAIN_content"), ContentPlaceHolder)

        ' Inner Master Page
        Dim Inner_CP As ContentPlaceHolder
        Inner_CP = TryCast(Outer_CP.FindControl("DetailsMaster_ContentPlaceHolder"), ContentPlaceHolder)

        CallByName(Inner_CP.Page, FunctionName, vbMethod, aParamArray)

    End Sub

    Protected Sub uwgOptions_SelectedRowsChange(ByVal sender As Object, ByVal e As Infragistics.WebUI.UltraWebGrid.SelectedRowsEventArgs) Handles uwgOptions.SelectedRowsChange
        If uwgOptions.DisplayLayout.ActiveRow IsNot Nothing Then
            Me.CurrentSession.VT_SelectedFormPath = uwgOptions.DisplayLayout.ActiveRow.Cells.FromKey("OptionPage").Text
            GoToNewPage(uwgOptions.DisplayLayout.ActiveRow.Cells.FromKey("OptionPage").Text)
        End If
    End Sub


    Protected Sub btnCommentClear_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnCommentClear.Click
        ' check for expired session
        If Session("_VT_SessionLive") <> "YES" Then
            ' if it has expired display a message and exit
            Dim objDisp As New VT_Display.DisplayFuncs
            objDisp.DisplayMessage(Page, VT_Constants.cTimeOutMessage)
            Exit Sub
        End If

        ''''''''''''''''''''''''''''''
        ' we need to use the name of the Control that currently has the focus
        ' to find its entry in the Comments list
        ''''''''''''''''''''
        Dim ControlWithFocus As Control = GetControlWithFocus()

        ' get the application comments
        Dim objApp As New CommentFunctions

        Dim intIdToUse As Integer
      
            intIdToUse = Me.CurrentSession.SelectedItemID


        Dim dtThreads As Data.DataTable = objApp.GetThreads(intIdToUse)
        Dim dvThreads As Data.DataView = dtThreads.DefaultView
        dvThreads.RowFilter = "Field7 <> ''"

        ' loop through the comments
        Dim blnControlFound As Boolean = False
        For Each dr As Data.DataRowView In dvThreads
            If dr.Item("Field7") = ControlWithFocus.ID Then
                blnControlFound = True
                objApp.ClearComment(dr.Item("Field0"))
                ' now clear all the coment in the thread
                Dim dtComments As Data.DataTable = objApp.GetThreadComments(dr.Item("Field0"))
                For Each drComment In dtComments.Rows
                    objApp.ClearComment(drComment.Item("Field0"))
                Next drComment
            End If
        Next

        SetupComments(intIdToUse)
    End Sub


    Sub GoToNewPage(ByVal strPage As String)
        Dim strMessage As String

        Dim blnOKToChangePage As Boolean = True
        blnOKToChangePage = SavePageData(strMessage)

        If blnOKToChangePage Then
            lblAlert.Text = ""
            Me.CurrentSession.VT_SelectedDetailsGridRow = uwgOptions.DisplayLayout.ActiveRow

            Response.Redirect(strPage)

            'Dim upl As UpdatePanel
            'upl = DetailsMaster_ContentPlaceHolder.FindControl("UpdatePanel1")
            'upl.Update()
        Else
            uwgOptions.DisplayLayout.ActiveRow = Me.CurrentSession.VT_SelectedDetailsGridRow
            uwgOptions.DisplayLayout.SelectedRows.Clear()
            uwgOptions.DisplayLayout.Rows(uwgOptions.DisplayLayout.ActiveRow.Index).Selected = True

            lblMessage.Text = strMessage
            ModalPopupExtender6.Show()

            ' need to display to the user the fact that some items are not filled
            lblAlert.Text = strMessage
        End If


    End Sub

    Function GetControlWithFocus() As Control
        ' Because we are in a nested Master we need to go to the Outer Master first
        ' Outer Master Page
        'Dim Outer_CP As ContentPlaceHolder
        'Outer_CP = TryCast(Me.Master.FindControl("ProjectMAIN_content"), ContentPlaceHolder)

        ' Inner Master Page
        Dim Inner_CP As ContentPlaceHolder
        Inner_CP = TryCast(FindControl("DetailsMaster_ContentPlaceHolder"), ContentPlaceHolder)

        'Dim hdnControlName As HiddenField = TryCast(Inner_CP.FindControl("hdnWhoHasFocus"), HiddenField)

        Dim hdnControlName As HiddenField = hdnWhoHasFocus

        ' the name of the control with the focus is returned in the form 
        '   ctl00_ctl00_ProjectMAIN_content_DetailsMaster_ContentPlaceHolder_TextBox2
        ' we need to use just the last bit
        ' however if it is a mandatory field it will be in the form
        '   ctl00_ctl00_ProjectMAIN_content_DetailsMaster_ContentPlaceHolder_TextBox2_RY
        ' or if it is a not-to be-datascraped field it will be in the form
        '   ctl00_ctl00_ProjectMAIN_content_DetailsMaster_ContentPlaceHolder_TextBox2_NS
        ' so we need the last two bits
        Dim strTemp As String
        If UCase(Right(hdnControlName.Value, 3)) = "_RY" Or UCase(Right(hdnControlName.Value, 3)) = "_NS" Then
            'If UCase(Right(hdnControlName.Value, 3)) = "_RY" Then
            Dim intPosn As Integer
            intPosn = InStrRev(hdnControlName.Value, "_")
            strTemp = Left(hdnControlName.Value, intPosn - 1)

            intPosn = InStrRev(strTemp, "_")
            strTemp = Mid(hdnControlName.Value, intPosn + 1)
        ElseIf InStr(hdnControlName.Value, "xDetailsMasterxContentPlaceHolderx") Then
            ' or if it is a grid it will be in the form
            '   ctl00xDetailsMaster_ContentPlaceHolderxGridName
            ' we need to find the first x after ContentPlaceHolder and then extract the gridName
            Dim intPosn As Integer
            intPosn = InStr(hdnControlName.Value, "ContentPlaceHolder") + Len("ContentPlaceHolder")
            strTemp = Mid(hdnControlName.Value, intPosn + 1)
            ' if the grid is marked _NS the Infragistics code will return its Id as "xNS" so we need to fix that
            If Right(strTemp, 3) = "xNS" Then
                strTemp = Left(strTemp, Len(strTemp) - 3) + "_NS"
            End If
        Else
            strTemp = Mid(hdnControlName.Value, InStrRev(hdnControlName.Value, "_") + 1)
        End If
        GetControlWithFocus = Inner_CP.FindControl(strTemp)

    End Function

    Protected Sub cmdOk_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles cmdOk.Click
        Dim ControlWithFocus As Control
        If ddlAssociateComment.Visible = True And ddlAssociateComment.SelectedValue = 0 Then
            ControlWithFocus = GetControlWithFocus()
        Else
            ControlWithFocus = Nothing
        End If

        ' if a control has the focus attach the comment to it
        If Not ControlWithFocus Is Nothing Then
            ApplyCommentToControl(ControlWithFocus, txtComment.Text)
        End If

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

        Dim objApps As New CommentFunctions

        Dim intIdToUse As Integer
       
            intIdToUse = Me.CurrentSession.SelectedItemID
        
        objApps.StoreComment(intIdToUse, txtComment.Text, uwgOptions.DisplayLayout.ActiveRow.Cells.FromKey("OptionPage").Text, ControlWithFocus, strCommentSource, strCommentTarget, intCommentSourceId, intCommentTargetId)

        SetupComments(intIdToUse)
    End Sub

    Protected Sub btnBack_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnBack.Click, btnBack1.Click
        Dim objData As New BPADotNetCommonFunctions.VT_DataPreserve.DataPreserve

        Dim intIdToUse As Integer

        intIdToUse = Me.CurrentSession.SelectedItemID


        ' if saving is enabled check if the form was dirty
        If btnSaveDetails.Enabled = True Then
        
                If CallContentPageBooleanFunction("DataToSave") = False Then
              

                ' a change has been made to the form so ask if the user wants to save it
                ModalPopupExtender7.Show()
                ' execution will continue from the button that the user clicks on the FormIsDirty panel
            Else
                Response.Redirect("~/TabPages/Quotes_Opening.aspx")
            End If
        Else
            Response.Redirect("~/TabPages/Quotes_Opening.aspx")
        End If

    End Sub


    Sub ShowMessagePanel(ByVal strMessage As String)
        lblMessage.Text = strMessage
        ModalPopupExtender6.Show()
    End Sub

    Function SavePageData(ByRef strMessage As String) As Boolean
        Dim intItemToSaveId As Integer
        Dim strFormName As String
        Dim objForms As New VT_Forms.Forms
        Dim strComment As String

        Dim objData As New BPADotNetCommonFunctions.VT_DataPreserve.DataPreserve
        Dim objVT As New VTDBFunctions.VTDBFunctions.VTMatrixFunctions
        Dim astrParam() As Object

        strFormName = Session("_VT_CurrentDetailsPage")


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
        ReDim astrParam(0)
       
            astrParam(0) = Me.CurrentSession.SelectedItemID


        CallContentPageSub("StoreFormSpecificData", astrParam)

        strFormName = Me.CurrentSession.VT_QuoteSelectedFormName

        '      intItemToSaveId = Me.CurrentSession.VT_QuoteNum


        SavePageData = True


        '' Get a reference to the controls on the current page
        '' Because we are in a nested Master we need to go to the Outer Master first
        '' Outer Master Page
        'Dim Outer_CP As ContentPlaceHolder
        'Outer_CP = TryCast(Me.Master.FindControl("ProjectMAIN_content"), ContentPlaceHolder)

        '' Inner Master Page
        'Dim Inner_CP As ContentPlaceHolder
        'Inner_CP = TryCast(Outer_CP.FindControl("FormsMaster_ContentPlaceHolder"), ContentPlaceHolder)
        '' check to see if the current form is to be saved
        'Dim blnSave As Boolean = CallContentPageBooleanFunction("DataToSave")
        'If Not blnSave Then
        '    Exit Function
        'End If


        'objData.ScrapeFormData(Inner_CP, intItemToSaveId, strFormName, MatrixFunctions.QuoteFormListTable, MatrixFunctions.QuoteFormDataTable)
        '' store the item id
        'objVT.InsertVTMatrixDataItem(Session("_VT_DotNetConnString"), MatrixFunctions.QuoteFormListTable, MatrixFunctions.QuoteFormDataTable, intItemToSaveId, strFormName, "ItemId", intItemToSaveId)

        '' we need to store the FormCompleted status
        'If strMessage = "" Then
        '    objVT.InsertVTMatrixDataItem(Session("_VT_DotNetConnString"), MatrixFunctions.QuoteFormListTable, MatrixFunctions.QuoteFormDataTable, intItemToSaveId, strFormName, "FormComplete", "YES")
        'Else
        '    objVT.InsertVTMatrixDataItem(Session("_VT_DotNetConnString"), MatrixFunctions.QuoteFormListTable, MatrixFunctions.QuoteFormDataTable, intItemToSaveId, strFormName, "FormComplete", "NO")
        'End If



        Dim strAuditComment As String = "Form saved: " + Me.CurrentSession.VT_QuoteSelectedFormName
        If Session("_VT_CurrentUserName") = "" Then
            Session("_VT_CurrentUserName") = "UnRegistered User"
        End If
        objForms.WriteToAuditLog(Me.CurrentSession.VT_QuoteNum, "Form", Now, Session("_VT_CurrentUserName"), Session("_VT_CurrentUserId"), strAuditComment, "Audit Record", "SysAdmin")




        ' store the approval details if they are in use
        If Me.CurrentSession.ApprovalDetails.AllItems IsNot Nothing Then
            Dim objComm As New DataBaseFunctions.Commonfunctions

            objComm.StoreItemApprovalDetails(intItemToSaveId, Me.CurrentSession.ApprovalDetails)
        End If


        SetItemStatusDisplay()

        ' we need to cause the summary grid to be redrawn
        Me.CurrentSession.VT_CategoryGridData = Nothing

    End Function

    Function RunCustomValidation(ByVal CP As ContentPlaceHolder, ByVal strFormName As String) As String
        Dim ThisGrid As Infragistics.WebUI.UltraWebGrid.UltraWebGrid

        ' return empty string if no problem found
        RunCustomValidation = ""


    End Function

    Protected Sub btnViewComments_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnCommentVIEW.Click
        ' check for expired session
        If Session("_VT_SessionLive") <> "YES" Then
            ' if it has expired display a message and exit
            Dim objDisp As New VT_Display.DisplayFuncs
            objDisp.DisplayMessage(Page, VT_Constants.cTimeOutMessage)
            Exit Sub
        End If

        ' jump to the comments page
        If uwgOptions.DisplayLayout.ActiveRow Is Nothing Then
            Me.CurrentSession.OptionsPageToReturnTo = Me.CurrentSession.aVT_DetailsPageOptionsPages(0)
        Else
            Me.CurrentSession.OptionsPageToReturnTo = Me.CurrentSession.aVT_DetailsPageOptionsPages(uwgOptions.DisplayLayout.ActiveRow.Index)
        End If


        Response.Redirect("~/Quotes/FormComments.aspx")
    End Sub


    Protected Sub btnNextPreviousComment_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnCommentNEXT.Click, btnCommentPrevious.Click
        ' check for expired session
        If Session("_VT_SessionLive") <> "YES" Then
            ' if it has expired display a message and exit
            Dim objDisp As New VT_Display.DisplayFuncs
            objDisp.DisplayMessage(Page, VT_Constants.cTimeOutMessage)
            Exit Sub
        End If

        ' first check if we are going next or previous
        Dim blnNext As Boolean
        If InStr(UCase(CType(sender, System.Web.UI.WebControls.ImageButton).ID), "NEXT") Then
            blnNext = True
        Else
            blnNext = False
        End If

        Dim ControlWithFocus As Control = GetControlWithFocus()

        ' get the application comments
        Dim objApp As New CommentFunctions
        Dim dtComments As Data.DataTable = objApp.GetThreads(Me.CurrentSession.SelectedItemID)
        Dim dvComments As Data.DataView = dtComments.DefaultView
        dvComments.RowFilter = "Field7 <> ''"

        Dim intNumComments As Integer = dvComments.Count

        ' find the comment connected to the control that has the focus
        Dim blnControlFound As Boolean = False
        Dim intIndex As Integer
        For intIndex = 0 To intNumComments - 1
            If Not IsDBNull(dvComments(intIndex).Item("Field7")) AndAlso dvComments(intIndex).Item("Field7") = ControlWithFocus.ID Then
                blnControlFound = True
                Exit For
            End If
        Next

        If blnControlFound = True Then
            If blnNext Then
                ' if we clicked Next and we are at the end of the list go back to the start
                If intIndex = intNumComments - 1 Then
                    intIndex = 0
                Else ' go to the next
                    intIndex += 1
                End If
            Else
                ' if we clicked Previous and we are at the start of the list go to the end
                If intIndex = 0 Then
                    intIndex = intNumComments - 1
                Else ' go to previous
                    intIndex -= 1
                End If
            End If

            Dim strCommentPage As String = dvComments(intIndex).Item("Field6")
            Dim strControlToGoTo As String = dvComments(intIndex).Item("Field7")
            Me.CurrentSession.VT_AppPageToShow = strCommentPage
            Me.CurrentSession.VT_ControlToJumpTo = strControlToGoTo
            GoToNewPage(strCommentPage)
        End If



    End Sub
    Protected Sub btnPreviousSection_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnPreviousSection.Click
        ' check for expired session
        If Session("_VT_SessionLive") <> "YES" Then
            ' if it has expired display a message and exit
            Dim objDisp As New VT_Display.DisplayFuncs
            objDisp.DisplayMessage(Page, VT_Constants.cTimeOutMessage)
            Exit Sub
        End If

        If uwgOptions.DisplayLayout.ActiveRow IsNot Nothing Then
            Dim intRowIndex As Integer = uwgOptions.DisplayLayout.ActiveRow.Index
            If intRowIndex = 0 Then
                intRowIndex = uwgOptions.Rows.Count - 1
            Else
                intRowIndex = intRowIndex - 1
            End If

            uwgOptions.DisplayLayout.ActiveRow = uwgOptions.Rows(intRowIndex)
            If Not String.IsNullOrEmpty(uwgOptions.DisplayLayout.ActiveRow.Cells.FromKey("OptionPage").Text) Then
                GoToNewPage(uwgOptions.DisplayLayout.ActiveRow.Cells.FromKey("OptionPage").Text)
            End If

        End If

    End Sub

    Protected Sub btnNextSection_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnNextSection.Click
        ' check for expired session
        If Session("_VT_SessionLive") <> "YES" Then
            ' if it has expired display a message and exit
            Dim objDisp As New VT_Display.DisplayFuncs
            objDisp.DisplayMessage(Page, VT_Constants.cTimeOutMessage)
            Exit Sub
        End If

        If uwgOptions.DisplayLayout.ActiveRow IsNot Nothing Then
            Dim intRowIndex As Integer = uwgOptions.DisplayLayout.ActiveRow.Index
            If intRowIndex = uwgOptions.Rows.Count - 1 Then
                intRowIndex = 0
            Else
                intRowIndex = intRowIndex + 1
            End If

            uwgOptions.DisplayLayout.ActiveRow = uwgOptions.Rows(intRowIndex)
            If Not String.IsNullOrEmpty(uwgOptions.DisplayLayout.ActiveRow.Cells.FromKey("OptionPage").Text) Then
                GoToNewPage(uwgOptions.DisplayLayout.ActiveRow.Cells.FromKey("OptionPage").Text)
            End If

        End If

    End Sub


    Protected Sub cmdOKSaveChanges_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles cmdOKSaveChanges.Click
        Dim strMessage As String

        Dim blnOKToChangePage As Boolean = True
        blnOKToChangePage = SavePageData(strMessage)

        If blnOKToChangePage Then
            Response.Redirect("~/TabPages/Quotes_Opening.aspx")
        Else
            ' need to display to the user the fact that the save didn't work
            lblAlert.Text = strMessage
        End If

    End Sub

    Protected Sub cmdCancelSaveChanges_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles cmdCancelSaveChanges.Click
        'Response.Redirect("~/CategoriesView.aspx")
    End Sub


    Protected Sub btnSaveDetailsTop_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnSaveDetails.Click
        ' check for expired session
        If Session("_VT_SessionLive") <> "YES" Then
            ' if it has expired display a message and exit
            Dim objDisp As New VT_Display.DisplayFuncs
            objDisp.DisplayMessage(Page, VT_Constants.cTimeOutMessage)
            Exit Sub
        End If

        Me.CurrentSession.DoAuditLog = True

        Dim blnOKToChangePage As Boolean = True
        Dim strMessage As String
        blnOKToChangePage = SavePageData(strMessage)
        If Not blnOKToChangePage Then
            lblMessage.Text = strMessage
            ModalPopupExtender6.Show()
        End If

    End Sub

    Protected Sub btnAudit_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnAudit.Click
        ' check for expired session
        If Session("_VT_SessionLive") <> "YES" Then
            ' if it has expired display a message and exit
            Dim objDisp As New VT_Display.DisplayFuncs
            objDisp.DisplayMessage(Page, VT_Constants.cTimeOutMessage)
            Exit Sub
        End If

        If uwgOptions.DisplayLayout.ActiveRow Is Nothing Then
            Me.CurrentSession.OptionsPageToReturnTo = Me.CurrentSession.aVT_DetailsPageOptionsPages(0)
        Else
            Me.CurrentSession.OptionsPageToReturnTo = Me.CurrentSession.aVT_DetailsPageOptionsPages(uwgOptions.DisplayLayout.ActiveRow.Index)
        End If


        Response.Redirect("Quote_AuditLog.aspx")

    End Sub

    Protected Sub btnCommentsPage_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnCommentsPage.Click
        ' check for expired session
        If Session("_VT_SessionLive") <> "YES" Then
            ' if it has expired display a message and exit
            Dim objDisp As New VT_Display.DisplayFuncs
            objDisp.DisplayMessage(Page, VT_Constants.cTimeOutMessage)
            Exit Sub
        End If

        If uwgOptions.DisplayLayout.ActiveRow Is Nothing Then
            Me.CurrentSession.OptionsPageToReturnTo = Me.CurrentSession.aVT_DetailsPageOptionsPages(0)
        Else
            Me.CurrentSession.OptionsPageToReturnTo = Me.CurrentSession.aVT_DetailsPageOptionsPages(uwgOptions.DisplayLayout.ActiveRow.Index)
        End If


        Response.Redirect("~/ApplicationPages/ApplicationComments.aspx")

    End Sub

    Protected Sub cmdNoSaveChanges_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles cmdNoSaveChanges.Click
        Response.Redirect("~/TabPages/Quotes_Opening.aspx")

    End Sub

    Sub GetDropDownData(ByVal strInput As String, ByRef strText As String, ByRef intValue As Integer)
        ' the value comes from a data-scraped dropdown so is in the format Text+vbTab+Value
        ' parse out the value
        Dim intPosn As Integer = InStr(strInput, vbTab)
        intValue = Val(Mid(strInput, intPosn + 1))
        strText = Left(strInput, intPosn)
    End Sub

    Protected Sub cmdOKPass_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles cmdOKPass.Click
        Dim objPer As New PersonnelModuleFunctions

        Dim objDisp As New VT_Display.DisplayFuncs

        ' check the password
        If Not objPer.IsPasswordValid(Session("_VT_DotNetConnString"), Session("_VT_CurrentUserId"), txtPassword.Text) Then
            ShowMessagePanel(GetGlobalResourceObject("Resource", "InvalidPassword"))
            Exit Sub
        End If

        Select Case hdnButtonClicked.Value
            Case "Approve"
                HandleApproveButton()
            Case "Reject"
                HandleRejectButton()
            Case Else

        End Select
    End Sub


    Sub HandleApproveButton()


        Dim strMessage As String

        Dim dtQuote As New DataTable
        Dim objQuote As New VTDBFunctions.VTDBFunctions.QuoteFunctions
        Dim objPer As New PersonnelModuleFunctions
        Dim objTasks As New VT_eQOInterface.eQOInterface
        Dim strAuditComment As String
        Dim objForms As New VT_Forms.Forms
        Dim lngQuoteNum As Long

        Dim objSequencer As New WFOSequencer
        objSequencer.DllPath = Server.MapPath("~") & "\Bin"

        lngQuoteNum = Me.CurrentSession.VT_QuoteNum

        'Check that user is allow to send for approval
        dtQuote = objQuote.GetQuoteHeaderInfoForID(Me.CurrentSession.VT_QuoteID)
        Select Case Trim(Me.CurrentSession.VT_CurrentQuoteStatus)

            Case GetGlobalResourceObject("Resource", "Quote_SubmittedForProd")

                If objPer.GetAccessRight(Session("_VT_DotNetConnString"), Session("_VT_CurrentUserId"), "QuoteProductApproval") = True OrElse objPer.GetAccessRight(Session("_VT_DotNetConnString"), Session("_VT_CurrentUserId"), "QuoteOverRide") = True Then

                    'Clear Waiting for Approval
                    objTasks.MarkTaskCompleteByTaskRef(lngQuoteNum, "WaitForProdClass", Session("_VT_CurrentUserName"), "PASS")

                    'Set status to Approved
                    objForms.SetJobStatusText(GetGlobalResourceObject("Resource", "Quote_ProdApproved"), lngQuoteNum)
                    objTasks.SetLastActivity(lngQuoteNum, System.DateTime.UtcNow.Date)
                    objQuote.UpdateQuoteStatus(Me.CurrentSession.VT_QuoteID, GetGlobalResourceObject("Resource", "Quote_ProdApproved"))
                    Me.CurrentSession.VT_CurrentQuoteStatus = GetGlobalResourceObject("Resource", "Quote_ProdApproved")

                    ' Run sequence
                    objSequencer.AdvanceSequence(lngQuoteNum, Session("_VT_CurrentUserId"), False, 0, 0, Session("_VT_DotNetConnString"))

                    'audit log
                    Dim strType As String
                    strAuditComment = "Product Classification approved."
                    strType = "Quote"
                    objForms.WriteToAuditLog(lngQuoteNum, strType, Now, Session("_VT_CurrentUserName"), Session("_VT_CurrentUserId"), strAuditComment, "Audit Record", "SysAdmin")

                    'Set the buttons up again
                    SetUpUI()

                    strMessage = "Product Classification has been completed and Approved."
                    lblMsg.Text = strMessage
                    ModalPopupExtenderMessage.Show()


                Else
                    strMessage = "You do not have privileges to approve this quote"
                    '  objDisp.DisplayMessage(Page, strTemp)
                    lblMsg.Text = strMessage
                    ModalPopupExtenderMessage.Show()
                    Exit Sub
                End If


            Case GetGlobalResourceObject("Resource", "Quote_Submitted")


                If objPer.GetAccessRight(Session("_VT_DotNetConnString"), Session("_VT_CurrentUserId"), "QuoteSignoff") = True OrElse objPer.GetAccessRight(Session("_VT_DotNetConnString"), Session("_VT_CurrentUserId"), "QuoteOverRide") = True Then

                    'Clear Waiting for Approval
                    objTasks.MarkTaskCompleteByTaskRef(lngQuoteNum, "ApprovalSignoff", Session("_VT_CurrentUserName"), "PASS")

                    'Set status to Approved
                    objForms.SetJobStatusText(GetGlobalResourceObject("Resource", "Quote_ReadytoSend"), lngQuoteNum)
                    objTasks.SetLastActivity(lngQuoteNum, System.DateTime.UtcNow.Date)
                    objQuote.UpdateQuoteStatus(Me.CurrentSession.VT_QuoteID, GetGlobalResourceObject("Resource", "Quote_ReadytoSend"))
                    Me.CurrentSession.VT_CurrentQuoteStatus = GetGlobalResourceObject("Resource", "Quote_ReadytoSend")

                    ' Run sequence
                    objSequencer.AdvanceSequence(lngQuoteNum, Session("_VT_CurrentUserId"), False, 0, 0, Session("_VT_DotNetConnString"))

                    'audit log
                    Dim strType As String
                    strAuditComment = "Quote customer Signoff complete."
                    strType = "Quote"
                    objForms.WriteToAuditLog(lngQuoteNum, strType, Now, Session("_VT_CurrentUserName"), Session("_VT_CurrentUserId"), strAuditComment, "Audit Record", "SysAdmin")


                    'Set the buttons up again
                    SetUpUI()

                    strMessage = "Quote Signoff has been completed."
                    lblMsg.Text = strMessage
                    ModalPopupExtenderMessage.Show()

                Else
                    strMessage = "You do not have privileges to sign off this quote"
                    '  objDisp.DisplayMessage(Page, strTemp)
                    lblMsg.Text = strMessage
                    ModalPopupExtenderMessage.Show()
                    Exit Sub
                End If



            Case Else
                strMessage = "The Quote is not at the correct status to Approve. "
                '  objDisp.DisplayMessage(Page, strTemp)
                lblMsg.Text = strMessage
                ModalPopupExtenderMessage.Show()
                Exit Sub
        End Select


    End Sub

    Sub HandleRejectButton()

        Dim strMessage As String

        Dim dtQuote As New DataTable
        Dim objQuote As New VTDBFunctions.VTDBFunctions.QuoteFunctions
        Dim objPer As New PersonnelModuleFunctions
        Dim objTasks As New VT_eQOInterface.eQOInterface
        Dim strAuditComment As String
        Dim objForms As New VT_Forms.Forms
        Dim lngQuoteNum As Long

        Dim objSequencer As New WFOSequencer
        objSequencer.DllPath = Server.MapPath("~") & "\Bin"


        lngQuoteNum = Me.CurrentSession.VT_QuoteNum

        'Check that user is allow to send for approval
        dtQuote = objQuote.GetQuoteHeaderInfoForID(Me.CurrentSession.VT_QuoteID)
        Select Case Trim(Me.CurrentSession.VT_CurrentQuoteStatus)

            Case GetGlobalResourceObject("Resource", "Quote_SubmittedForProd")

                If objPer.GetAccessRight(Session("_VT_DotNetConnString"), Session("_VT_CurrentUserId"), "QuoteProductApproval") = True OrElse objPer.GetAccessRight(Session("_VT_DotNetConnString"), Session("_VT_CurrentUserId"), "QuoteOverRide") = True Then

                    'Clear Waiting for Approval
                    objTasks.MarkTaskCompleteByTaskRef(lngQuoteNum, "WaitForProdClass", Session("_VT_CurrentUserName"), "FAIL")

                    'Set status to New Rejected
                    objForms.SetJobStatusText(GetGlobalResourceObject("Resource", "Quote_New"), lngQuoteNum)
                    objTasks.SetLastActivity(lngQuoteNum, System.DateTime.UtcNow.Date)
                    objQuote.UpdateQuoteStatus(Me.CurrentSession.VT_QuoteID, GetGlobalResourceObject("Resource", "Quote_New"))
                    Me.CurrentSession.VT_CurrentQuoteStatus = GetGlobalResourceObject("Resource", "Quote_New")

                    ' Run sequence
                    objSequencer.AdvanceSequence(lngQuoteNum, Session("_VT_CurrentUserId"), False, 0, 0, Session("_VT_DotNetConnString"))

                    'audit log
                    Dim strType As String
                    strAuditComment = "Product Classification rejected."
                    strType = "Quote"
                    objForms.WriteToAuditLog(lngQuoteNum, strType, Now, Session("_VT_CurrentUserName"), Session("_VT_CurrentUserId"), strAuditComment, "Audit Record", "SysAdmin")


                    'Set the buttons up again
                    SetUpUI()

                    strMessage = "Product Classification has been rejected."
                    lblMsg.Text = strMessage
                    ModalPopupExtenderMessage.Show()


                Else
                    strMessage = "You do not have privileges to approve this quote"
                    '  objDisp.DisplayMessage(Page, strTemp)
                    lblMsg.Text = strMessage
                    ModalPopupExtenderMessage.Show()
                    Exit Sub
                End If


            Case GetGlobalResourceObject("Resource", "Quote_Submitted")


                If objPer.GetAccessRight(Session("_VT_DotNetConnString"), Session("_VT_CurrentUserId"), "QuoteSignoff") = True OrElse objPer.GetAccessRight(Session("_VT_DotNetConnString"), Session("_VT_CurrentUserId"), "QuoteOverRide") = True Then

                    'Clear Waiting for Approval
                    objTasks.MarkTaskCompleteByTaskRef(lngQuoteNum, "ApprovalSignoff", Session("_VT_CurrentUserName"), "FAIL")

                    'Set status to New rejected
                    objForms.SetJobStatusText(GetGlobalResourceObject("Resource", "Quote_New"), lngQuoteNum)
                    objTasks.SetLastActivity(lngQuoteNum, System.DateTime.UtcNow.Date)
                    objQuote.UpdateQuoteStatus(Me.CurrentSession.VT_QuoteID, GetGlobalResourceObject("Resource", "Quote_New"))
                    Me.CurrentSession.VT_CurrentQuoteStatus = GetGlobalResourceObject("Resource", "Quote_New")

                    ' Run sequence
                    objSequencer.AdvanceSequence(lngQuoteNum, Session("_VT_CurrentUserId"), False, 0, 0, Session("_VT_DotNetConnString"))


                    'audit log
                    Dim strType As String
                    strAuditComment = "Quote customer Signoff rejected."
                    strType = "Quote"
                    objForms.WriteToAuditLog(lngQuoteNum, strType, Now, Session("_VT_CurrentUserName"), Session("_VT_CurrentUserId"), strAuditComment, "Audit Record", "SysAdmin")

                    'Set the buttons up again
                    SetUpUI()

                    strMessage = "Quote Signoff has been rejected."
                    lblMsg.Text = strMessage
                    ModalPopupExtenderMessage.Show()

                Else
                    strMessage = "You do not have privileges to sign off this quote"
                    '  objDisp.DisplayMessage(Page, strTemp)
                    lblMsg.Text = strMessage
                    ModalPopupExtenderMessage.Show()
                    Exit Sub
                End If



            Case Else
                strMessage = "The Quote is not at the correct status to Approve. "
                '  objDisp.DisplayMessage(Page, strTemp)
                lblMsg.Text = strMessage
                ModalPopupExtenderMessage.Show()
                Exit Sub
        End Select

    End Sub

    'Sub ApproveHACCPForm()
    '    CType(FindContentPageControl("txtApprovedBy"), TextBox).Text = Session("_VT_CurrentUserName")
    '    CType(FindContentPageControl("txtApprovedDate"), TextBox).Text = Today.ToShortDateString
    '    CType(FindContentPageControl("hdnStatus"), TextBox).Text = GetGlobalResourceObject("Resource", "ApprovedStatus")

    '    Dim strDummy As String
    '    SavePageData(strDummy)

    '    btnSignOff.Enabled = False
    '    Dim objData As New BPADotNetCommonFunctions.VT_DataPreserve.DataPreserve

    '    objData.EnableDisableContentPageControls(FindControl("DetailsMaster_ContentPlaceHolder"), False)


    'End Sub




   
    Protected Sub btnSendForProdApproval_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnSendForProdApproval.Click

        Dim strMessage As String
        Dim dtQuote As New DataTable
        Dim objQuote As New VTDBFunctions.VTDBFunctions.QuoteFunctions
        Dim objPer As New PersonnelModuleFunctions
        Dim objTasks As New VT_eQOInterface.eQOInterface
        Dim strAuditComment As String
        Dim objForms As New VT_Forms.Forms
        Dim lngQuoteNum As Long

        Dim objSequencer As New WFOSequencer
        objSequencer.DllPath = Server.MapPath("~") & "\Bin"


        lngQuoteNum = Me.CurrentSession.VT_QuoteNum

        'Check that user is allow to send for approval
        dtQuote = objQuote.GetQuoteHeaderInfoForID(Me.CurrentSession.VT_QuoteID)

        If Session("_VT_CurrentUserId") = dtQuote.Rows(0).Item("PersonLoggingQuote") OrElse objPer.GetAccessRight(Session("_VT_DotNetConnString"), Session("_VT_CurrentUserId"), "QuoteOverRide") = True Then

            'Clear Waiting for Submit task
            objTasks.MarkTaskCompleteByTaskRef(lngQuoteNum, "WaitForSubmit", Session("_VT_CurrentUserName"))

            'Set status to Submitted for Product Approval
            objForms.SetJobStatusText(GetGlobalResourceObject("Resource", "Quote_SubmittedForProd"), lngQuoteNum)
            objTasks.SetLastActivity(lngQuoteNum, System.DateTime.UtcNow.Date)
            objQuote.UpdateQuoteStatus(Me.CurrentSession.VT_QuoteID, GetGlobalResourceObject("Resource", "Quote_SubmittedForProd"))
            Me.CurrentSession.VT_CurrentQuoteStatus = GetGlobalResourceObject("Resource", "Quote_SubmittedForProd")

            ' Run sequence
            objSequencer.AdvanceSequence(lngQuoteNum, Session("_VT_CurrentUserId"), False, 0, 0, Session("_VT_DotNetConnString"))

            'audit log
            Dim strType As String
            strAuditComment = "Quote Sent for Product Classification and Approval"
            strType = "Quote"
            objForms.WriteToAuditLog(lngQuoteNum, strType, Now, Session("_VT_CurrentUserName"), Session("_VT_CurrentUserId"), strAuditComment, "Audit Record", "SysAdmin")

            'Set the buttons up again
            SetUpUI()


            strMessage = "Quote has been sent for Product Classification and Approval."
            lblMsg.Text = strMessage
            ModalPopupExtenderMessage.Show()

        Else
            strMessage = "You are not the owner of this quote or do not have privileges to send it for product classification approval"
            '  objDisp.DisplayMessage(Page, strTemp)
            lblMsg.Text = strMessage
            ModalPopupExtenderMessage.Show()
            Exit Sub
        End If



       



    End Sub

    Protected Sub btnSubmit_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnSubmit.Click

        Dim strMessage As String
        Dim dtQuote As New DataTable
        Dim objQuote As New VTDBFunctions.VTDBFunctions.QuoteFunctions
        Dim objPer As New PersonnelModuleFunctions
        Dim objTasks As New VT_eQOInterface.eQOInterface
        Dim strAuditComment As String
        Dim objForms As New VT_Forms.Forms
        Dim lngQuoteNum As Long

        Dim objSequencer As New WFOSequencer
        objSequencer.DllPath = Server.MapPath("~") & "\Bin"

        lngQuoteNum = Me.CurrentSession.VT_QuoteNum

        'Check that user is allow to send for approval
        dtQuote = objQuote.GetQuoteHeaderInfoForID(Me.CurrentSession.VT_QuoteID)

        If Session("_VT_CurrentUserId") = dtQuote.Rows(0).Item("PersonLoggingQuote") OrElse objPer.GetAccessRight(Session("_VT_DotNetConnString"), Session("_VT_CurrentUserId"), "QuoteOverRide") = True Then

            'Clear Waiting for Submit task
            objTasks.MarkTaskCompleteByTaskRef(lngQuoteNum, "WaitForSubmit", Session("_VT_CurrentUserName"))

            'Set status to Submitted for Product Approval
            objForms.SetJobStatusText(GetGlobalResourceObject("Resource", "Quote_Submitted"), lngQuoteNum)
            objTasks.SetLastActivity(lngQuoteNum, System.DateTime.UtcNow.Date)
            objQuote.UpdateQuoteStatus(Me.CurrentSession.VT_QuoteID, GetGlobalResourceObject("Resource", "Quote_Submitted"))
            Me.CurrentSession.VT_CurrentQuoteStatus = GetGlobalResourceObject("Resource", "Quote_Submitted")

            ' Run sequence
            objSequencer.AdvanceSequence(lngQuoteNum, Session("_VT_CurrentUserId"), False, 0, 0, Session("_VT_DotNetConnString"))

            'audit log
            Dim strType As String
            strAuditComment = "Quote Submitted for Approval"
            strType = "Quote"
            objForms.WriteToAuditLog(lngQuoteNum, strType, Now, Session("_VT_CurrentUserName"), Session("_VT_CurrentUserId"), strAuditComment, "Audit Record", "SysAdmin")

            'Set the buttons up again
            SetUpUI()

            strMessage = "Quote has been Submitted for Approval."
            lblMsg.Text = strMessage
            ModalPopupExtenderMessage.Show()

        Else
            strMessage = "You are not the owner of this quote or do not have privileges to send it for approval"
            '  objDisp.DisplayMessage(Page, strTemp)
            lblMsg.Text = strMessage
            ModalPopupExtenderMessage.Show()
            Exit Sub
        End If



    End Sub

    Protected Sub btnSendToCustomer_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnSendToCustomer.Click

        Dim strMessage As String
        Dim dtQuote As New DataTable
        Dim objQuote As New VTDBFunctions.VTDBFunctions.QuoteFunctions
        Dim objPer As New PersonnelModuleFunctions
        Dim objTasks As New VT_eQOInterface.eQOInterface
        Dim strAuditComment As String
        Dim objForms As New VT_Forms.Forms
        Dim lngQuoteNum As Long

        Dim objSequencer As New WFOSequencer
        objSequencer.DllPath = Server.MapPath("~") & "\Bin"


        lngQuoteNum = Me.CurrentSession.VT_QuoteNum

        'Check that user is allow to send for approval
        dtQuote = objQuote.GetQuoteHeaderInfoForID(Me.CurrentSession.VT_QuoteID)

        If Session("_VT_CurrentUserId") = dtQuote.Rows(0).Item("PersonLoggingQuote") OrElse objPer.GetAccessRight(Session("_VT_DotNetConnString"), Session("_VT_CurrentUserId"), "QuoteOverRide") = True Then

            'Clear Waiting for Submit task
            objTasks.MarkTaskCompleteByTaskRef(lngQuoteNum, "WaitForSentToCust", Session("_VT_CurrentUserName"))

            'Set status to Submitted for Product Approval
            objForms.SetJobStatusText(GetGlobalResourceObject("Resource", "Quote_Sent"), lngQuoteNum)
            objTasks.SetLastActivity(lngQuoteNum, System.DateTime.UtcNow.Date)
            objQuote.UpdateQuoteStatus(Me.CurrentSession.VT_QuoteID, GetGlobalResourceObject("Resource", "Quote_Sent"))
            Me.CurrentSession.VT_CurrentQuoteStatus = GetGlobalResourceObject("Resource", "Quote_Sent")

            ' Run sequence
            objSequencer.AdvanceSequence(lngQuoteNum, Session("_VT_CurrentUserId"), False, 0, 0, Session("_VT_DotNetConnString"))

            'audit log
            Dim strType As String
            strAuditComment = "Quote Sent To Customer"
            strType = "Quote"
            objForms.WriteToAuditLog(lngQuoteNum, strType, Now, Session("_VT_CurrentUserName"), Session("_VT_CurrentUserId"), strAuditComment, "Audit Record", "SysAdmin")

            'Update the date  issued of the quote
            objQuote.UpdateQuoteDateIssued(Me.CurrentSession.VT_QuoteID, Now)


            'Set the buttons up again
            SetUpUI()

            strMessage = "Quote has been marked Sent to Customer."
            lblMsg.Text = strMessage
            ModalPopupExtenderMessage.Show()

        Else
            strMessage = "You are not the owner of this quote or do not have privileges to send it for approval"
            '  objDisp.DisplayMessage(Page, strTemp)
            lblMsg.Text = strMessage
            ModalPopupExtenderMessage.Show()
            Exit Sub
        End If


    End Sub


    Protected Sub btnClose_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnClose.Click



        Dim strMessage As String

        Dim dtQuote As New DataTable
        Dim objQuote As New VTDBFunctions.VTDBFunctions.QuoteFunctions
        Dim objPer As New PersonnelModuleFunctions
        Dim objTasks As New VT_eQOInterface.eQOInterface
        Dim strAuditComment As String
        Dim objForms As New VT_Forms.Forms
        Dim lngQuoteNum As Long

        Dim objSequencer As New WFOSequencer
        objSequencer.DllPath = Server.MapPath("~") & "\Bin"


        lngQuoteNum = Me.CurrentSession.VT_QuoteNum

        'Check that user is allow to send for approval
        dtQuote = objQuote.GetQuoteHeaderInfoForID(Me.CurrentSession.VT_QuoteID)
        Select Case Trim(Me.CurrentSession.VT_CurrentQuoteStatus)

            Case GetGlobalResourceObject("Resource", "Quote_Sent")



                If Session("_VT_CurrentUserId") = dtQuote.Rows(0).Item("PersonLoggingQuote") OrElse objPer.GetAccessRight(Session("_VT_DotNetConnString"), Session("_VT_CurrentUserId"), "QuoteOverRide") = True Then

                    'Set status to Closed Quote unsuccessful
                    objForms.SetJobStatusText(GetGlobalResourceObject("Resource", "Quote_Unsuccesful"), lngQuoteNum)
                    objTasks.SetLastActivity(lngQuoteNum, System.DateTime.UtcNow.Date)
                    objQuote.UpdateQuoteStatus(Me.CurrentSession.VT_QuoteID, GetGlobalResourceObject("Resource", "Quote_Unsuccesful"))
                    Me.CurrentSession.VT_CurrentQuoteStatus = GetGlobalResourceObject("Resource", "Quote_Unsuccesful")

                    ' Set Sequence to Finished   
                    FinishQuoteProgram()

                    'Clear clear all tasks 
                    objTasks.MarkTasksCompleteForJob(lngQuoteNum, Session("_VT_CurrentUserName"), Session("_VT_DotNetConnString"))


                    'audit log
                    Dim strType As String
                    strAuditComment = "Quote Unsuccessful."
                    strType = "Quote"
                    objForms.WriteToAuditLog(lngQuoteNum, strType, Now, Session("_VT_CurrentUserName"), Session("_VT_CurrentUserId"), strAuditComment, "Audit Record", "SysAdmin")

                    'Set the buttons up again
                    SetUpUI()

                    strMessage = "Quote has been closed as unsuccessful."
                    lblMsg.Text = strMessage
                    ModalPopupExtenderMessage.Show()

                Else
                    strMessage = "You do not have privileges to close this quote"
                    '  objDisp.DisplayMessage(Page, strTemp)
                    lblMsg.Text = strMessage
                    ModalPopupExtenderMessage.Show()
                    Exit Sub
                End If


            Case Else


                If Session("_VT_CurrentUserId") = dtQuote.Rows(0).Item("PersonLoggingQuote") OrElse objPer.GetAccessRight(Session("_VT_DotNetConnString"), Session("_VT_CurrentUserId"), "QuoteOverRide") = True Then

                 
                    'Set status to Closed not sent
                    objForms.SetJobStatusText(GetGlobalResourceObject("Resource", "Quote_NotSent"), lngQuoteNum)
                    objTasks.SetLastActivity(lngQuoteNum, System.DateTime.UtcNow.Date)
                    objQuote.UpdateQuoteStatus(Me.CurrentSession.VT_QuoteID, GetGlobalResourceObject("Resource", "Quote_NotSent"))
                    Me.CurrentSession.VT_CurrentQuoteStatus = GetGlobalResourceObject("Resource", "Quote_NotSent")

                    ' Set Sequence to Finished   
                    FinishQuoteProgram()

                    'Clear clear all tasks 
                    objTasks.MarkTasksCompleteForJob(lngQuoteNum, Session("_VT_CurrentUserName"), Session("_VT_DotNetConnString"))


                    'audit log
                    Dim strType As String
                    strAuditComment = "Quote Closed not sent."
                    strType = "Quote"
                    objForms.WriteToAuditLog(lngQuoteNum, strType, Now, Session("_VT_CurrentUserName"), Session("_VT_CurrentUserId"), strAuditComment, "Audit Record", "SysAdmin")

                    'Set the buttons up again
                    SetUpUI()

                    strMessage = "Quote has been closed as not set to customer."
                    lblMsg.Text = strMessage
                    ModalPopupExtenderMessage.Show()

                Else
                    strMessage = "You do not have privileges to close this quote"
                    '  objDisp.DisplayMessage(Page, strTemp)
                    lblMsg.Text = strMessage
                    ModalPopupExtenderMessage.Show()
                    Exit Sub
                End If



           
        End Select




    End Sub

    Protected Sub btnConvert_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnConvert.Click

        Dim strMessage As String

        Dim dtQuote As New DataTable
        Dim objQuote As New VTDBFunctions.VTDBFunctions.QuoteFunctions
        Dim objPer As New PersonnelModuleFunctions
        Dim objTasks As New VT_eQOInterface.eQOInterface
        Dim strAuditComment As String
        Dim objForms As New VT_Forms.Forms
        Dim lngQuoteNum As Long

        Dim objSequencer As New WFOSequencer
        objSequencer.DllPath = Server.MapPath("~") & "\Bin"


        lngQuoteNum = Me.CurrentSession.VT_QuoteNum

        'Check that user is allow to send for approval
        dtQuote = objQuote.GetQuoteHeaderInfoForID(Me.CurrentSession.VT_QuoteID)


        If Session("_VT_CurrentUserId") = dtQuote.Rows(0).Item("PersonLoggingQuote") OrElse objPer.GetAccessRight(Session("_VT_DotNetConnString"), Session("_VT_CurrentUserId"), "QuoteOverRide") = True Then

        
            'Set status to Closed Converted to Sales Order
            objForms.SetJobStatusText(GetGlobalResourceObject("Resource", "Quote_Converted"), lngQuoteNum)
            objTasks.SetLastActivity(lngQuoteNum, System.DateTime.UtcNow.Date)
            objQuote.UpdateQuoteStatus(Me.CurrentSession.VT_QuoteID, GetGlobalResourceObject("Resource", "Quote_Converted"))
            Me.CurrentSession.VT_CurrentQuoteStatus = GetGlobalResourceObject("Resource", "Quote_Converted")

            ' Set Sequence to Finished   
            FinishQuoteProgram()

            'Clear clear all tasks 
            objTasks.MarkTasksCompleteForJob(lngQuoteNum, Session("_VT_CurrentUserName"), Session("_VT_DotNetConnString"))



            'audit log
            Dim strType As String
            strAuditComment = "Quote Converted to Sales Order."
            strType = "Quote"
            objForms.WriteToAuditLog(lngQuoteNum, strType, Now, Session("_VT_CurrentUserName"), Session("_VT_CurrentUserId"), strAuditComment, "Audit Record", "SysAdmin")

            'Set the buttons up again
            SetUpUI()

            strMessage = "Converting to a sales order is not currently active in the system."
            lblMsg.Text = strMessage
            ModalPopupExtenderMessage.Show()

        Else
            strMessage = "You do not have privileges to close this quote"
            '  objDisp.DisplayMessage(Page, strTemp)
            lblMsg.Text = strMessage
            ModalPopupExtenderMessage.Show()
            Exit Sub
        End If


          
        'Go to the order creation screens with some data prefilled.




    End Sub

    Sub FinishQuoteProgram()
        Dim objWFOCommon As New WFOCommonFunctions
        Dim objSequencer As New WFOSequencer
        objSequencer.DllPath = Server.MapPath("~") & "\Bin"

        Dim dtwfoSequenceData As New DataTable
        Dim objCommonFuncs As New BPADotNetCommonFunctions.VT_CommonFunctions.CommonFunctions

        Dim objeQO As New BPADotNetCommonFunctions.VT_eQOInterface.eQOInterface
        Dim strQuotesProgramId As String = objCommonFuncs.GetConfigItem("QuotesJobID")
        Dim intQuoteProgramId As Integer
        Dim intQuoteNum As Integer

        If strQuotesProgramId = "" Then
            intQuoteProgramId = 0
        Else
            intQuoteProgramId = CInt(strQuotesProgramId)
        End If

        intQuoteNum = Me.CurrentSession.VT_QuoteNum

        'Get the last wfo in the program
        dtwfoSequenceData = objWFOCommon.GetSequenceDataForLastWFOStep(intQuoteProgramId, Session("_VT_DotNetConnString"))


        'set the wfo for the  job to this wfo
        objeQO.SetCurrentWFO(intQuoteNum, dtwfoSequenceData.Rows(0).Item("UniqueReferenceNumber"), Session("_VT_DotNetConnString"))

        'Run the program
        objSequencer.AdvanceSequence(intQuoteNum, Session("_VT_CurrentUserId"), False, 0, 0, Session("_VT_DotNetConnString"))


    End Sub
End Class

