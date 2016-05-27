Imports BPADotNetCommonFunctions
Imports BPADotNetCommonFunctions.PersonnelModuleFunctions

Partial Class DetailsMaster
    Inherits System.Web.UI.MasterPage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        ScriptManager1.RegisterAsyncPostBackControl(uwgOptions)

        If Not IsPostBack Then
            Dim astrOptions() As String = Session("_VT_DetailsPageOptions")
            Dim astrOptionsPages() As String = Session("_VT_DetailsPageOptionsPages")


            uwgOptions.Rows.Clear()
            For i As Integer = LBound(astrOptions) To UBound(astrOptions)
                uwgOptions.Rows.Add()
                uwgOptions.Rows(i).Cells.FromKey("Option").Text = astrOptions(i)
                uwgOptions.Rows(i).Cells.FromKey("OptionPage").Text = astrOptionsPages(i)
                uwgOptions.Rows(i).DataKey = astrOptions(i)
            Next i

            If Session("_VT_SelectedDetailsGridRow") Is Nothing Then
                uwgOptions.DisplayLayout.ActiveRow = uwgOptions.Rows(0)
                Session("_VT_SelectedDetailsGridRow") = uwgOptions.Rows(0)
            Else
                uwgOptions.DisplayLayout.ActiveRow = Session("_VT_SelectedDetailsGridRow")
            End If


            ' hide the Back button if we are in StartUp mode
            If Session("_VT_StartUpPersonnelId") <> 0 Then
                btnBack.Visible = False
            End If


        End If



    End Sub



  



    Protected Sub btnSaveDetails_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnSaveDetails.Click, btnSaveDetailsTop.Click
        SavePageData()
    End Sub

    Protected Sub btnBack_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnBack.Click
        If Session("VT_CameFromInTray") = "YES" Then
            Response.Redirect("~/TabPages/InTray_Opening.aspx")
        Else
            Response.Redirect("~/TabPages/Jobs_Opening.aspx")
        End If

    End Sub

    Function SavePageData() As Boolean
        'Dim mpContentPlaceHolder As ContentPlaceHolder
        'Dim intItemToSaveId, intParentID As Integer
        'Dim strFormName As String
        'Dim objDisp As New VT_Display.DisplayFuncs

        'mpContentPlaceHolder = CType(FindControl("DetailsMaster_ContentPlaceHolder"), ContentPlaceHolder)
        'strFormName = Session("_VT_CurrentDetailsPage")

        '' if we are saving the Details form we must update the Personnel name label with 
        '' the contents of the txtPerReference box
        'If strFormName = "PersonnelDetails" Then
        '    If CType(mpContentPlaceHolder.FindControl("txtFirstName"), TextBox).Text = "" Or _
        '    CType(mpContentPlaceHolder.FindControl("txtSurname"), TextBox).Text = "" Then
        '        objDisp.DisplayMessage(Page, "You must supply a First Name and Surname for the User!")
        '        ' return False to indicate the data was not saved
        '        SavePageData = False
        '        Exit Function
        '    Else
        '        Session("_VT_SelectedPersonnelName") = CType(mpContentPlaceHolder.FindControl("txtFirstName"), TextBox).Text + " " + CType(mpContentPlaceHolder.FindControl("txtSurname"), TextBox).Text
        '        lblDetailsItemName.Text = Session("_VT_SelectedPersonnelName")
        '    End If
        'ElseIf strFormName = "PersonnelAccessDetails" Then
        '    If CType(mpContentPlaceHolder.FindControl("txtPass"), TextBox).Text <> CType(mpContentPlaceHolder.FindControl("txtReTypePass"), TextBox).Text Then
        '        objDisp.DisplayMessage(Page, "You must enter the same password in both entry boxes!")
        '        ' return False to indicate the data was not saved
        '        SavePageData = False
        '        Exit Function

        '    End If
        'End If

        'SavePageData = True

        '' get the record for this Personnel using the Reference field
        'Dim objPer As New PersonnelModuleFunctions
        'Dim strPersonnelName As String = lblDetailsItemName.Text
        'Dim intPerId As Integer = Session("_VT_SelectedPersonnelID")
        'If intPerId = 0 Then
        '    ' we must create a new Personnel record in VTMatrix
        '    intPerId = objPer.CreatePersonnelRecord(Session("_VT_DotNetConnString"), Session("_VT_SelectedPersonnelCategoryId"), strPersonnelName)
        '    Session("_VT_SelectedPersonnelID") = intPerId
        'End If

        'intItemToSaveId = intPerId
        'If Session("_VT_SelectedPersonnelCategoryId") <> 0 Then
        '    intParentID = Session("_VT_SelectedPersonnelCategoryId")
        'Else
        '    intParentID = objPer.GetUsersCategoryId(Session("_VT_DotNetConnString"), intPerId)
        'End If

        'Dim objData As New BPADotNetCommonFunctions.VT_DataPreserve.DataPreserve
        'Dim objVT As New VTDBFunctions.VTDBFunctions.VTMatrixFunctions


        'objData.ScrapeFormData(FindControl("DetailsMaster_ContentPlaceHolder"), intItemToSaveId, strFormName, PersonnelModuleFunctions.g_strFormListTable, PersonnelModuleFunctions.g_strFormDataTable)

        '' if we are storing the Personnel Access form we need to overwrite the Password controls
        '' with encrypted passwords
        'If strFormName = "PersonnelAccessDetails" Then
        '    Dim strError As String
        '    Dim strPass As String = CType(mpContentPlaceHolder.FindControl("txtReTypePass"), TextBox).Text
        '    Dim objPass As New VT_Password.Password
        '    strPass = objPass.EncryptPassword(strPass, strError)
        '    If strError = "" Then
        '        objVT.InsertVTMatrixDataItem(Session("_VT_DotNetConnString"), PersonnelModuleFunctions.g_strFormListTable, PersonnelModuleFunctions.g_strFormDataTable, intItemToSaveId, strFormName, "txtPass", strPass)
        '        objVT.InsertVTMatrixDataItem(Session("_VT_DotNetConnString"), PersonnelModuleFunctions.g_strFormListTable, PersonnelModuleFunctions.g_strFormDataTable, intItemToSaveId, strFormName, "txtReTypePass", strPass)
        '    Else
        '        ' restore the original password
        '        strPass = objPass.EncryptPassword(Session("_VT_CurrentUserPassword"))

        '        objVT.InsertVTMatrixDataItem(Session("_VT_DotNetConnString"), PersonnelModuleFunctions.g_strFormListTable, PersonnelModuleFunctions.g_strFormDataTable, intItemToSaveId, strFormName, "txtPass", strPass)
        '        objVT.InsertVTMatrixDataItem(Session("_VT_DotNetConnString"), PersonnelModuleFunctions.g_strFormListTable, PersonnelModuleFunctions.g_strFormDataTable, intItemToSaveId, strFormName, "txtReTypePass", strPass)

        '        objDisp.DisplayMessage(Page, strError)
        '    End If
        'End If


        '' store the item id and parent Id and the RoleId also
        'objVT.InsertVTMatrixDataItem(Session("_VT_DotNetConnString"), PersonnelModuleFunctions.g_strFormListTable, PersonnelModuleFunctions.g_strFormDataTable, intItemToSaveId, strFormName, "ItemId", intItemToSaveId)
        'If strFormName = "PersonnelDetails" Then
        '    objVT.InsertVTMatrixDataItem(Session("_VT_DotNetConnString"), PersonnelModuleFunctions.g_strFormListTable, PersonnelModuleFunctions.g_strFormDataTable, intItemToSaveId, strFormName, "ParentId", intParentID)
        '    Dim strRoleId As String = CType(mpContentPlaceHolder.FindControl("ddlRole"), DropDownList).SelectedValue.ToString

        '    objVT.InsertVTMatrixDataItem(Session("_VT_DotNetConnString"), PersonnelModuleFunctions.g_strFormListTable, PersonnelModuleFunctions.g_strFormDataTable, intItemToSaveId, strFormName, "RoleId", strRoleId)
        'End If



    End Function

    Protected Sub ImageButton1_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ImageButton1.Click, ImageButton2.Click
        'Dim strVTStore As String = Replace(ConfigurationManager.AppSettings("VTStore"), "XXXX", Session("_VT_CurrentDID"))
        Dim strVTStore As String = "~/VTStoreVer2/vt_passthrough.aspx" + "?DID=" + Session("_VT_CurrentDID").ToString
        strVTStore = strVTStore + "&Auth=Ok&UID=4&StartNodeText=&ShowBanner=No"
        Session("_VT_PageToReturnToFromAttachments") = Session("_VT_DetailsPageOptionsPages")(uwgOptions.DisplayLayout.ActiveRow.Index)
        Session("_VT_DotNetConnString") = ""

        Response.Redirect(strVTStore)

    End Sub

 
    Protected Sub uwgOptions_Click(ByVal sender As Object, ByVal e As Infragistics.WebUI.UltraWebGrid.ClickEventArgs) Handles uwgOptions.Click
        If uwgOptions.DisplayLayout.ActiveRow IsNot Nothing Then
            Dim blnOKToChangePage As Boolean = True
            If Session("_VT_CurrentDetailsPage") = "PersonnelDetails" Or _
                Session("_VT_CurrentDetailsPage") = "PersonnelAccessDetails" Or _
                Session("_VT_CurrentDetailsPage") = "PersonnelContacts" Then
                blnOKToChangePage = SavePageData()

            End If

            If blnOKToChangePage Then
                Session("_VT_DetailsPageType") = uwgOptions.DisplayLayout.ActiveRow.Cells.FromKey("Option").Text
                If Not String.IsNullOrEmpty(uwgOptions.DisplayLayout.ActiveRow.Cells.FromKey("OptionPage").Text) Then
                    Response.Redirect(uwgOptions.DisplayLayout.ActiveRow.Cells.FromKey("OptionPage").Text)

                    Dim upl As UpdatePanel
                    upl = DetailsMaster_ContentPlaceHolder.FindControl("UpdatePanel1")
                    upl.Update()
                End If

            End If



        End If
    End Sub

    
End Class

