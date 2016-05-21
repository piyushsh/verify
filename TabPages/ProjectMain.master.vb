Imports BPADotNetCommonFunctions

Partial Class ProjectMain
    Inherits MyMasterPage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'display the time zone in the web_config table, if this is not present display the Server time zone
        Dim objCommon As New BPADotNetCommonFunctions.VT_CommonFunctions.CommonFunctions

        Dim strTimeZone As String = objCommon.GetConfigItem("Timezone")
        If strTimeZone = "" Then
            strTimeZone = "W. Europe Standard Time"
        End If
        lblTimezone.Text = strTimeZone
        hdnTimeZone.Value = strTimeZone
        clock.text = format(PortalFunctions.now, "hh:mm")



        '... Set the date
        lblTodayDate.Text = Format(PortalFunctions.Now.Date(), "d")
        lblUserName.Text = Session("_VT_CurrentUserName")
        lblDataBaseName.Text = "Database [ " & StrConv(Session("_VT_CurrentDID"), VbStrConv.ProperCase) & " ]   "

        ' read the version number from the commonfunction class
        lblVer.Text = CommonFunctions.g_strWebsiteVersionNumber

        ' connect each of the tabs to the function that unlocks any items locked in the VersionManager
        btnTOP_Contracts.Attributes.Add("onclick", "return ClearLockedFormIdInsessionStorage();")
        btnTOP_Deliveries.Attributes.Add("onclick", "return ClearLockedFormIdInsessionStorage();")
        btnTOP_Details.Attributes.Add("onclick", "return ClearLockedFormIdInsessionStorage();")
        btnTOP_Fulfill.Attributes.Add("onclick", "return ClearLockedFormIdInsessionStorage();")
        btnTOP_Orders.Attributes.Add("onclick", "return ClearLockedFormIdInsessionStorage();")
        btnTOP_Planning.Attributes.Add("onclick", "return ClearLockedFormIdInsessionStorage();")
        btnTOP_Printouts.Attributes.Add("onclick", "return ClearLockedFormIdInsessionStorage();")
        btnTOP_SYSADMIN.Attributes.Add("onclick", "return ClearLockedFormIdInsessionStorage();")
        btnTOP_Quotes.Attributes.Add("onclick", "return ClearLockedFormIdInsessionStorage();")
        btnLogout.Attributes.Add("onclick", "return ClearLockedFormIdInsessionStorage();")
        btnTOP_SliceAndDice.Attributes.Add("onclick", "return ClearLockedFormIdInsessionStorage();")

        Me.CurrentSession.VT_Top_CurrentTab = UCase(Me.CurrentSession.VT_Top_CurrentTab)
        Me.CurrentSession.VT_TopTab_Clicked = UCase(Me.CurrentSession.VT_TopTab_Clicked)

        ' ... Set the SALES tab "on" as default
        If (Me.CurrentSession.VT_TopTab_Clicked = "" And Me.CurrentSession.VT_Top_CurrentTab = "") Then
            btnTOP_Orders.CssClass = "VT_SelectTab_Light"
            Me.CurrentSession.VT_Top_CurrentTab = "ORDERS"
            Me.CurrentSession.VT_TopTab_Clicked = "ORDERS"
        End If

        If UCase(objCommon.GetConfigItem("ShowContractsTab")) = "TRUE" Then
            btnTOP_Contracts.Visible = True
        Else
            btnTOP_Contracts.Visible = False

        End If
        If UCase(objCommon.GetConfigItem("ShowSliceAndDiceTab")) = "TRUE" Then
            btnTOP_SliceAndDice.Visible = True
        Else
            btnTOP_SliceAndDice.Visible = False

        End If

        ' btnTOP_Deliveries.Visible = False
        btnTOP_Fulfill.Visible = False




        '... Swap TAB images on a click (only come in here if a Tab was clicked)
        If Me.CurrentSession.VT_TopTab_Clicked <> "" Then

            '... Set the Image OFF on the TAB Previous Tab
            Select Case Me.CurrentSession.VT_Top_CurrentTab
                Case "INTRAY"
                    'btnTOP_InTray.ImageUrl = "~/App_Themes/TabButtons/In-Tray-OFF.gif"
                Case "ORDERS"
                    btnTOP_Orders.CssClass = "VT_UnSelectTab_Light"
                Case "DETAILS"
                    btnTOP_Details.CssClass = "VT_UnSelectTab_Light"
                Case "DELIVERIES"
                    btnTOP_Deliveries.CssClass = "VT_UnSelectTab_Light"
                Case "PRINTOUTS"
                    btnTOP_Printouts.CssClass = "VT_UnSelectTab_Light"
                Case "SLICEANDDICE"
                    btnTOP_SliceAndDice.CssClass = "VT_UnSelectTab_Light"
                Case "AUDIT"
                    btnTOP_Contracts.CssClass = "VT_UnSelectTab_Light"
                Case "SYSTEM-ADMIN"
                    btnTOP_SYSADMIN.CssClass = "VT_UnSelectTab_Light"
                Case "PLANNING"
                    btnTOP_Planning.CssClass = "VT_UnSelectTab_Light"
                Case "FULFILL"
                    btnTOP_Fulfill.CssClass = "VT_UnSelectTab_Light"
                Case "QUOTES"
                    btnTOP_Quotes.CssClass = "VT_UnSelectTab_Light"
                Case "CONTRACTS"
                    btnTOP_Quotes.CssClass = "VT_UnSelectTab_Light"
                Case Else
                    'if we are in here then assume it was the Schemes tab
                    'btnTOP_Jobs.ImageUrl = "~/APP_THEMES/BIlling2/Tabs/Campaigns-OFF.gif"

            End Select

            '... Set the Image ON on the TAB required
            Select Case UCase(Me.CurrentSession.VT_TopTab_Clicked)

                Case "INTRAY"
                    'btnTOP_InTray.ImageUrl = "~/App_Themes/TabButtons/In-Tray-On.gif"
                    Me.CurrentSession.VT_Top_CurrentTab = "INTRAY"

                Case "ORDERS"
                    btnTOP_Orders.CssClass = "VT_SelectTab_Light"
                    Me.CurrentSession.VT_Top_CurrentTab = "ORDERS"

                Case "DETAILS"
                    btnTOP_Details.CssClass = "VT_SelectTab_Light"
                    Me.CurrentSession.VT_Top_CurrentTab = "DETAILS"

                Case "DELIVERIES"
                    btnTOP_Deliveries.CssClass = "VT_SelectTab_Light"
                    Me.CurrentSession.VT_Top_CurrentTab = "DELIVERIES"

                Case "PRINTOUTS"
                    btnTOP_Printouts.CssClass = "VT_SelectTab_Light"
                    Me.CurrentSession.VT_Top_CurrentTab = "PRINTOUTS"

                Case "SLICEANDDICE"
                    btnTOP_SliceAndDice.CssClass = "VT_SelectTab_Light"
                    Me.CurrentSession.VT_Top_CurrentTab = "SLICEANDDICE"

                Case "SYSTEM-ADMIN"
                    btnTOP_SYSADMIN.CssClass = "VT_SelectTab_Light"
                    Me.CurrentSession.VT_Top_CurrentTab = "SYSTEM-ADMIN"

                Case "PLANNING"
                    btnTOP_Planning.CssClass = "VT_SelectTab_Light"
                    Me.CurrentSession.VT_Top_CurrentTab = "PLANNING"

                Case "FULFILL"
                    btnTOP_Fulfill.CssClass = "VT_SelectTab_Light"
                    Me.CurrentSession.VT_Top_CurrentTab = "FULFILL"

                Case "QUOTES"
                    btnTOP_Quotes.CssClass = "VT_SelectTab_Light"
                    Me.CurrentSession.VT_Top_CurrentTab = "QUOTES"

                Case "CONTRACTS"
                    btnTOP_Quotes.CssClass = "VT_SelectTab_Light"
                    Me.CurrentSession.VT_Top_CurrentTab = "QUOTES"
                Case Else

            End Select



        End If  '... end of Swap TAB images Section


    End Sub

    Public Sub ShowMessagePanel(ByVal strMessage As String)
        lblMsgMandatory.Text = strMessage
        ModalPopupExtenderMsg.Show()
    End Sub


    Protected Sub btnTOP_SYSADMIN_Click(sender As Object, e As EventArgs) Handles btnTOP_SYSADMIN.Click
        ClearFormLockAndSaveForm()


        Dim objCommonFuncs As New BPADotNetCommonFunctions.VT_CommonFunctions.CommonFunctions
        Dim strPersonnelLocation As String = objCommonFuncs.GetConfigItem("WhereIsPersonnel")

        If UCase(strPersonnelLocation) = "MATRIX" Then
            Dim objPer As New PersonnelModuleFunctions

            If Not objPer.GetAccessRight(Session("_VT_DotNetConnString"), Session("_VT_CurrentUserId"), "CanViewSysAdminTab") Then
                'Dim objDisp As New VT_Display.DisplayFuncs
                'objDisp.DisplayMessage(Page, "You do not have the correct privilege to access the Sys. Admin. area.. Contact your System Administrator.")

                lblMsgMandatory.Text = "You do not have the correct privilege to access the Sys. Admin. area.. Contact your System Administrator."
                ModalPopupExtenderMsg.Show()

                Exit Sub
            End If
        ElseIf UCase(strPersonnelLocation) = "SQL" Then
        Else 'Default is eQOffice Personnel
            Dim objVT As New VT_eQOInterface.eQOInterface
            Dim dt As New Data.DataTable

            dt = objVT.GetEQOPersonalDetails(Session("_VT_CurrentUserId"))


            If Not dt.Rows(0).Item("IsSysAdministrator") Then
                'Dim objDisp As New VT_Display.DisplayFuncs
                'objDisp.DisplayMessage(Page, "You do not have the correct privilege to access the Sys. Admin. area.. Contact your System Administrator.")

                lblMsgMandatory.Text = "You do not have the correct privilege to access the Sys. Admin. area.. Contact your System Administrator."
                ModalPopupExtenderMsg.Show()
                Exit Sub
            End If

        End If

        ' enable the countdown timer
        Session("_VT_TimeoutRunning") = "YES"

        '    GetActiveProjectName()
        Me.CurrentSession.VT_TopTab_Clicked = "SYSTEM-ADMIN"
        Response.Redirect("~/TabPages/SystemAdmin_Opening.aspx")


    End Sub





    Public Function GetActiveProjectName() As String
        ' find a reference to the Projects grid on the content page
        Dim DocsGrid As Infragistics.WebUI.UltraWebGrid.UltraWebGrid = ProjectMAIN_content.FindControl("uwgJobs")
        Dim strProjectName As String
        If DocsGrid IsNot Nothing AndAlso DocsGrid.DisplayLayout.ActiveRow IsNot Nothing Then
            strProjectName = Trim(DocsGrid.DisplayLayout.ActiveRow.Cells(1).GetText())
            Me.CurrentSession.VT_JobName = strProjectName
            ' save the selected project in session
            Session("_VT_SelectedWorkOrderGridRow") = DocsGrid.DisplayLayout.ActiveRow
            Me.CurrentSession.VT_JobID = CType(DocsGrid.DisplayLayout.ActiveRow.Cells.FromKey("JobID").Text, Long)
            Me.CurrentSession.VT_JobType = DocsGrid.DisplayLayout.ActiveRow.Cells.FromKey("ModelType").Text
            Dim objProj As New BPADotNetCommonFunctions.ProjectModuleFunctions
            Dim intProjectRoot As Integer = objProj.GetProjectModuleRootId(Session("_VT_DotNetConnString"))
            ' get the project folder id
            '    Me.CurrentSession.VT_SelectedProjectMatrixID = objProj.GetProjectFolderId(Session("_VT_DotNetConnString"), intProjectRoot, Trim(DocsGrid.DisplayLayout.ActiveRow.Cells.FromKey("SchemeName").GetText()))

            '   Me.CurrentSession.VT_ProjectProspectsId = objProj.GetProjectGenericFolderId(Session("_VT_DotNetConnString"), Me.CurrentSession.VT_SelectedProjectMatrixID, "Prospects")
            ' Me.CurrentSession.VT_ProjectSalesOppsId = objProj.GetProjectGenericFolderId(Session("_VT_DotNetConnString"), Me.CurrentSession.VT_SelectedProjectMatrixID, "SalesOpps")
        Else
            'strProjectName = Me.CurrentSession.VT_SelectedProject
        End If

        GetActiveProjectName = strProjectName
    End Function




    Protected Sub btnTOP_Details_Click(sender As Object, e As EventArgs) Handles btnTOP_Details.Click
        ClearFormLockAndSaveForm()

        If Me.CurrentSession.VT_JobID = 0 Then
            'Dim objDisp As New VT_Display.DisplayFuncs
            'objDisp.DisplayMessage(Page, "Please select an Order to see the details of the order")

            lblMsgMandatory.Text = "Please select an Order to see the details of the order"
            ModalPopupExtenderMsg.Show()

            Exit Sub
        End If

        ' enable the countdown timer
        Session("_VT_TimeoutRunning") = "YES"

        ' GetActiveProjectName()
        Me.CurrentSession.VT_TopTab_Clicked = "DETAILS"

        'SmcN 20/01/2014  Added this section to allow different Details pages to be loaded
        'Loop through the ModulePages Array to find the index of the  'Sales Order Opening'  page
        Dim intCnt, intIndex As Integer
        intIndex = -99

        If Me.CurrentSession.aVT_ModulePageOptionsPages IsNot Nothing AndAlso Me.CurrentSession.aVT_ModulePageOptionsPages.Length > 0 Then

            For intCnt = 0 To Me.CurrentSession.aVT_ModulePageOptionsPages.Length
                If Trim(Me.CurrentSession.aVT_ModulePageOptions(intCnt)) = "Sales Order Details" Then
                    intIndex = intCnt
                    Exit For
                End If
            Next
        End If

        ' Launch the portal 
        If intIndex >= 0 Then
            'Launch the appropriate page
            Response.ClearHeaders()
            Response.Redirect("~/TabPages/" & Me.CurrentSession.aVT_ModulePageOptionsPages(intIndex))
        Else
            'Otherwise launch the Default page
            Response.Redirect("~/TabPages/Details_Opening.aspx")
        End If


    End Sub



    Protected Sub btnTOP_Deliveries_Click(sender As Object, e As EventArgs) Handles btnTOP_Deliveries.Click
        ClearFormLockAndSaveForm()
        'If Me.CurrentSession.VT_JobID = 0 Then
        '    Dim objDisp As New VT_Display.DisplayFuncs

        '    objDisp.DisplayMessage(Page, "Please select a job from the grid to see the associated forms and documents")
        '    Exit Sub
        'End If
        'GetActiveProjectName()
        ' enable the countdown timer
        Session("_VT_TimeoutRunning") = "YES"
        Me.CurrentSession.VT_Top_CurrentTab = "DELIVERIES"
        Me.CurrentSession.VT_TopTab_Clicked = "DELIVERIES"
        Response.Redirect("~/TabPages/WarehouseManager.aspx")

    End Sub

    Protected Sub btnTOP_Orders_Click(sender As Object, e As EventArgs) Handles btnTOP_Orders.Click
        ClearFormLockAndSaveForm()


        ' GetActiveProjectName()
        ' enable the countdown timer
        Session("_VT_TimeoutRunning") = "YES"

        Me.CurrentSession.VT_TopTab_Clicked = "Orders"
        'Response.Redirect("~/TabPages/Orders_Opening.aspx")

        '   Response.Redirect("~/TabPages/Orders_Opening_Steripack.aspx")


        'SmcN 20/01/2014  Added this section to load the appropriate Module pages and Sales Order Editing pages
        'Loop through the ModulePages Array to find the index of the 'Sales Order Opening'  page
        Dim intCnt, intIndex As Integer
        intIndex = -99

        If Me.CurrentSession.aVT_ModulePageOptionsPages IsNot Nothing AndAlso Me.CurrentSession.aVT_ModulePageOptionsPages.Length > 0 Then
            For intCnt = 0 To Me.CurrentSession.aVT_ModulePageOptionsPages.Length
                If Trim(Me.CurrentSession.aVT_ModulePageOptions(intCnt)) = "Sales Order Opening" Then
                    intIndex = intCnt
                    Exit For
                End If
            Next
        End If

        ' Launch the portal 
        If intIndex >= 0 Then
            'Launch the appropriate page
            Response.Redirect("~/TabPages/" & Me.CurrentSession.aVT_ModulePageOptionsPages(intIndex))
        Else
            'Otherwise launch the Default page
            Response.Redirect("~/TabPages/Orders_Opening.aspx")
        End If








    End Sub


    Protected Sub btnTOP_Printouts_Click(sender As Object, e As EventArgs) Handles btnTOP_Printouts.Click
        ClearFormLockAndSaveForm()


        Dim objPer As New PersonnelModuleFunctions
        If Not objPer.GetAccessRight(Session("_VT_DotNetConnString"), Session("_VT_CurrentUserId"), "CanViewReportsTab") Then
            Dim objDisp As New VT_Display.DisplayFuncs
            objDisp.DisplayMessage(Page, "You do not have the correct privilege [CanViewReportsTab] to access the Reports system. Contact your System Administrator.")
            Exit Sub
        End If

        Session("_VT_iFrameHeight") = 1200
        Dim strSupp = Replace(ConfigurationManager.AppSettings("ReportsModule"), "XXXX", Session("_VT_CurrentDID"))
        Me.CurrentSession.VT_ProjectInIFrame = strSupp + "&Auth=Ok&UID=" + CStr(Session("_VT_CurrentUserId")) + "&RType=Sales-Dispatch-Margin-Transaction&RETURN=SalesPortal"
        ' enable the countdown timer
        Session("_VT_TimeoutRunning") = "YES"

        '.... Code to run in an iFrame
        '  Response.Redirect("~/IFrame Pages/Reports_IFrame.aspx")


        Response.Redirect("~/IFrame Pages/ModulesIFrame.aspx")
        '....Code to run in a New window (or TAB depending on browser configuration)
        'Response.Clear()
        'Response.ClearHeaders()
        'Response.Write("<script>window.open('/IFrame Pages/Reports_IFrame.aspx','newwin');</script>")

    End Sub


    Protected Sub btnTOP_Planning_Click(sender As Object, e As EventArgs) Handles btnTOP_Planning.Click
        ClearFormLockAndSaveForm()

        Me.CurrentSession.VT_TopTab_Clicked = "Planning"
        ' enable the countdown timer
        Session("_VT_TimeoutRunning") = "YES"

        'check if planning is enabled or not, if not then show the module not available page
        Dim objdb As New BPADotNetCommonFunctions.VT_CommonFunctions.CommonFunctions

        Dim strSKipPlanning As String = objdb.GetConfigItem("SkipPlanning")
        If UCase(strSKipPlanning) = "YES" Then
            Response.Redirect("~/TabPages/Quotes_Not_Configured.aspx")
            Exit Sub
        End If

        'SmcN 15/02/2014  Added this section to allow different Planning pages to be loaded
        'Loop through the ModulePages Array to find the index of the  'Planning Opening'  page
        Dim intCnt, intIndex As Integer
        intIndex = -99

        If Me.CurrentSession.aVT_ModulePageOptionsPages IsNot Nothing AndAlso Me.CurrentSession.aVT_ModulePageOptionsPages.Length > 0 Then

            For intCnt = 0 To Me.CurrentSession.aVT_ModulePageOptionsPages.Length
                If Trim(Me.CurrentSession.aVT_ModulePageOptions(intCnt)) = "Planning Opening" Then
                    intIndex = intCnt
                    Exit For
                End If
            Next
        End If

        ' Launch the portal 
        If intIndex >= 0 Then
            'Launch the appropriate page
            Response.ClearHeaders()
            Response.Redirect("~/TabPages/" & Me.CurrentSession.aVT_ModulePageOptionsPages(intIndex))
        Else
            'Otherwise launch the Default page
            Response.Redirect("~/TabPages/Planning_Opening.aspx")
        End If


    End Sub




    Public Function PortalRootNode() As String
        ' retrieve the Root node of the current portal
        ' we will use it to create a consistent path to the image files
        Dim strCompletePath As String = Server.MapPath("~")

        'ServerRootPath = strCompletePath
        PortalRootNode = Mid(strCompletePath, InStrRev(strCompletePath, "\") + 1)

    End Function

    Protected Sub btnTOP_Fulfill_Click(sender As Object, e As EventArgs) Handles btnTOP_Fulfill.Click
        ClearFormLockAndSaveForm()

        Me.CurrentSession.VT_TopTab_Clicked = "Fulfill"
        ' enable the countdown timer
        Session("_VT_TimeoutRunning") = "YES"

        ' Response.Redirect("~/TabPages/Fulfill_Opening.aspx")
        Response.Redirect("~/TabPages/WarehouseManager.aspx")

    End Sub


    Protected Sub btnTOP_Quotes_Click(sender As Object, e As EventArgs) Handles btnTOP_Quotes.Click
        ClearFormLockAndSaveForm()


        Me.CurrentSession.VT_TopTab_Clicked = "Quotes"

        Dim objCommonFuncs As New BPADotNetCommonFunctions.VT_CommonFunctions.CommonFunctions
        Dim strQuotesEnabled As String = objCommonFuncs.GetConfigItem("QuotesEnabled")

        ' enable the countdown timer
        Session("_VT_TimeoutRunning") = "YES"

        If UCase(strQuotesEnabled) = "Q80T4S" Or UCase(strQuotesEnabled) = "YES" Then
            Response.Redirect("~/TabPages/Quotes_Opening.aspx")
        Else
            Response.Redirect("~/TabPages/Quotes_Not_Configured.aspx")
        End If
    End Sub

    Protected Sub btnTOP_Contracts_Click(sender As Object, e As EventArgs) Handles btnTOP_Contracts.Click
        ClearFormLockAndSaveForm()

        Me.CurrentSession.VT_TopTab_Clicked = "Contracts"

        Dim strPer = Replace(ConfigurationManager.AppSettings("VersionManager"), "XXXX", Session("_VT_CurrentDID"))

        ' enable the countdown timer
        Session("_VT_TimeoutRunning") = "YES"

        Me.CurrentSession.VT_ProjectInIFrame = strPer + "&Auth=Ok&UID=" + CStr(Session("_VT_CurrentUserId")) + "&MType=Contracts"
        Response.Redirect("~/IFrame Pages/ModulesIFrame.aspx")


    End Sub
    Sub ClearFormLockAndSaveForm()
        ' 
        ' sub checks if the a page owned by the NewOrderMaster was being displayed.
        ' if it was the FormLock for the form is cleared and if the form was in Editing mode it is saved.
        '

        Dim Outer_CP As ContentPlaceHolder
        Outer_CP = TryCast(Me.FindControl("ProjectMAIN_content"), ContentPlaceHolder)
        If InStr(Outer_CP.Page.Master.TemplateControl.AppRelativeVirtualPath, "NewOrderMasterSteri") Or InStr(Outer_CP.Page.Master.TemplateControl.AppRelativeVirtualPath, "NewOrderMaster") Then

            ' we can clear the form lock here
            Dim objVTM As New VTDBFunctions.VTDBFunctions.VTMatrixFunctions
            objVTM.LockFormForWriting(Session("_VT_DotNetConnString"), Me.CurrentSession.VT_NewOrderMatrixID, "tls_FormData", "")

            ' Find the Lock Graphic on the FormsMaster
            Dim imgLock As Image
            imgLock = TryCast(Outer_CP.FindControl("imgLockGraphic"), Image)
            If imgLock.Visible = False Then 'only check for changes if the form was not locked
                Dim aParamArray(0) As String
                CallByName(Outer_CP.Page.Master, "SavePageData", vbMethod, aParamArray)

            End If

        End If

    End Sub



    Protected Sub btnLogout_Click(sender As Object, e As EventArgs) Handles btnLogout.Click
        ClearFormLockAndSaveForm()
        Session("Abandon") = "YES"

        Response.Redirect("~/TabPages/Shutdown.aspx")
    End Sub



End Class

