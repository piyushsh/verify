
Partial Class SystemAdmin_Opening
    Inherits MyBasePage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then


            Dim objCommonFuncs As New BPADotNetCommonFunctions.VT_CommonFunctions.CommonFunctions
            Dim strPersonnelLocation As String = objCommonFuncs.GetConfigItem("WhereIsPersonnel")

            If UCase(strPersonnelLocation) = "MATRIX" Then
                btnPersonnelModule.Visible = True
                lblPersonnel.Visible = True
            ElseIf UCase(strPersonnelLocation) = "SQL" Then
                btnPersonnelModule.Visible = False
                lblPersonnel.Visible = False
            Else 'Default is eQOffice Personnel
                btnPersonnelModule.Visible = False
                lblPersonnel.Visible = False
            End If

            Dim blnHasSageInvoicePermissions As Boolean

            If UCase(strPersonnelLocation) = "MATRIX" Then

                Dim objPer As New BPADotNetCommonFunctions.PersonnelModuleFunctions
                If objPer.GetAccessRight(Session("_VT_DotNetConnString"), Session("_VT_CurrentUserId"), "CanViewSageInvoices") Then
                    blnHasSageInvoicePermissions = True
                Else
                    blnHasSageInvoicePermissions = False
                End If

            ElseIf UCase(strPersonnelLocation) = "SQL" Then
                blnHasSageInvoicePermissions = False

            Else
                'DEfault is eQOffice personnel. Always allow this to view invoices.
                blnHasSageInvoicePermissions = True

            End If

            'Show hide invoice check
            If blnHasSageInvoicePermissions = False Then

                btnInvoiceCheck.Visible = True
                lblSageInvoices.Visible = True

            End If


        End If
    End Sub
    Protected Sub btnPersonnelModule_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnPersonnelModule.Click

        Dim strPer = Replace(ConfigurationManager.AppSettings("VersionManager"), "XXXX", Session("_VT_CurrentDID"))

        Session("_VT_iFrameHeight") = 580
        Me.CurrentSession.VT_ProjectInIFrame = strPer + "&Auth=Ok&UID=" + CStr(Session("_VT_CurrentUserId")) + "&MType=Personnel"
        Response.Redirect("~/IFrame Pages/ModulesIFrame.aspx")


    End Sub





    Protected Sub btnTransactions_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnTransactions.Click

        Response.Redirect("~/TabPages/HandheldTxns.aspx")
    End Sub

    Protected Sub btnInvoiceCheck_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnInvoiceCheck.Click

        Response.Redirect("~/Other_Pages/InvoiceCheck.aspx")
    End Sub


    Protected Sub btnProductsModule_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnProductsModule.Click
        Dim strLink = Replace(ConfigurationManager.AppSettings("VersionManager"), "XXXX", Session("_VT_CurrentDID"))

        Session("_VT_iFrameHeight") = 580
        Me.CurrentSession.VT_ProjectInIFrame = strLink + "&Auth=Ok&UID=" + CStr(Session("_VT_CurrentUserId")) + "&MType=Materials"
        Response.Redirect("~/IFrame Pages/ModulesIFrame.aspx")


    End Sub

    Protected Sub btnManageLocations_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnManageLocations.Click

        Response.Redirect("Locations.aspx")
    End Sub

    Protected Sub btnCustomers_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnCustomers.Click
        Dim strLink = Replace(ConfigurationManager.AppSettings("VersionManager"), "XXXX", Session("_VT_CurrentDID"))

        Session("_VT_iFrameHeight") = 580
        Me.CurrentSession.VT_ProjectInIFrame = strLink + "&Auth=Ok&UID=" + CStr(Session("_VT_CurrentUserId")) + "&MType=Customers"
        Response.Redirect("~/IFrame Pages/ModulesIFrame.aspx")

    End Sub




    Protected Sub btnPriceLists_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnPriceLists.Click
        Dim strLink = Replace(ConfigurationManager.AppSettings("VersionManager"), "XXXX", Session("_VT_CurrentDID"))

        Session("_VT_iFrameHeight") = 1200
        Me.CurrentSession.VT_ProjectInIFrame = strLink + "&Auth=Ok&UID=" + CStr(Session("_VT_CurrentUserId")) + "&MType=CustomerPricing"
        Response.Redirect("~/IFrame Pages/ModulesIFrame.aspx")


    End Sub

    Protected Sub btnContracts_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles btnContracts.Click
        Dim strPer = Replace(ConfigurationManager.AppSettings("VersionManager"), "XXXX", Session("_VT_CurrentDID"))

        Session("_VT_iFrameHeight") = 580
        Me.CurrentSession.VT_ProjectInIFrame = strPer + "&Auth=Ok&UID=" + CStr(Session("_VT_CurrentUserId")) + "&MType=Contracts"
        Response.Redirect("~/IFrame Pages/ModulesIFrame.aspx")


    End Sub

    Protected Sub btnInventoryCounts_Click(sender As Object, e As ImageClickEventArgs) Handles btnInventoryCounts.Click

        Dim objPer As New BPADotNetCommonFunctions.PersonnelModuleFunctions
        If Not objPer.GetAccessRight(Session("_VT_DotNetConnString"), Session("_VT_CurrentUserId"), "CanViewInventoryCounts") Then
            Dim objDisp As New BPADotNetCommonFunctions.VT_Display.DisplayFuncs
            objDisp.DisplayMessage(Page, "You do not have the correct privilege [CanViewInventoryCounts] to access the Inventory Count Management module. Contact your System Administrator.")
            Exit Sub
        End If


        Dim strSupp = Replace(ConfigurationManager.AppSettings("VerifyModules"), "XXXX", Session("_VT_CurrentDID"))
        Me.CurrentSession.VT_ProjectInIFrame = strSupp + "&Auth=Ok&UID=" + CStr(Session("_VT_CurrentUserId")) + "&MType=StockTakes"
        Session("_VT_iFrameHeight") = 1500

        'Response.Redirect("~/IFrame Pages/ModulesIFrame_Responsive.aspx")
        Response.Redirect("~/IFrame Pages/ModulesIFrame.aspx")
    End Sub

    Protected Sub btnTransactionManager_Click(sender As Object, e As ImageClickEventArgs) Handles btnTransactionManager.Click

        Dim objPer As New BPADotNetCommonFunctions.PersonnelModuleFunctions
        If Not objPer.GetAccessRight(Session("_VT_DotNetConnString"), Session("_VT_CurrentUserId"), "CanAccessTransactionManager") Then
            Dim objDisp As New BPADotNetCommonFunctions.VT_Display.DisplayFuncs
            objDisp.DisplayMessage(Page, "You do not have the correct privilege [CanAccessTransactionManager] to access the Transaction Manager module. Contact your System Administrator.")
            Exit Sub
        End If

        Dim strSupp = Replace(ConfigurationManager.AppSettings("VerifyModules"), "XXXX", Session("_VT_CurrentDID"))
        Me.CurrentSession.VT_ProjectInIFrame = strSupp + "&Auth=Ok&UID=" + CStr(Session("_VT_CurrentUserId")) + "&MType=Transactions"
        Session("_VT_iFrameHeight") = 1500

        '  Response.Redirect("~/IFrame Pages/ModulesIFrame_Responsive.aspx")
        Response.Redirect("~/IFrame Pages/ModulesIFrame.aspx")

    End Sub

    Protected Sub btnJobSteps_Click(sender As Object, e As EventArgs) Handles btnJobSteps.Click
        Dim strPer = Replace(ConfigurationManager.AppSettings("VersionManager"), "XXXX", Session("_VT_CurrentDID"))
        Session("_VT_iFrameHeight") = 1500

        Me.CurrentSession.VT_ProjectInIFrame = strPer + "&Auth=Ok&UID=" + CStr(Session("_VT_CurrentUserId")) + "&MType=Workflow Sequences"
        Response.Redirect("~/IFrame Pages/ModulesIFrame.aspx")
    End Sub

    Protected Sub btnRoles_Click(sender As Object, e As EventArgs) Handles btnRoles.Click
        Dim objPer As New BPADotNetCommonFunctions.PersonnelModuleFunctions

        Dim strUsePrivileges As String = UCase(System.Configuration.ConfigurationManager.AppSettings("UsePrivileges"))
        If strUsePrivileges = "YES" Then

            If Not objPer.GetAccessRight(Session("_VT_DotNetConnString"), Session("_VT_CurrentUserId"), "RolesCanAdd") Then
                Dim objDisp As New BPADotNetCommonFunctions.VT_Display.DisplayFuncs
                objDisp.DisplayMessage(Page, "You do not have the correct privilege to access the Roles module. Contact your System Administrator.")
                Exit Sub
            End If
        End If
        Session("_VT_iFrameHeight") = 1500
        Dim strSupp = Replace(ConfigurationManager.AppSettings("RolesModule"), "XXXX", Session("_VT_CurrentDID"))
        Me.CurrentSession.VT_ProjectInIFrame = strSupp + "&Auth=Ok&UID=" + CStr(Session("_VT_CurrentUserId")) + "&MType=Roles"
        If Me.CurrentSession.VT_CurrentCompany <> "" Then
            Me.CurrentSession.VT_ProjectInIFrame += "&Company=" + Me.CurrentSession.VT_CurrentCompany
        End If

        Response.Redirect("~/IFrame Pages/ModulesIFrame.aspx")

    End Sub


    Protected Sub btnImportOrders_Click(sender As Object, e As EventArgs) Handles btnImportOrders.Click
        Dim objPer As New BPADotNetCommonFunctions.PersonnelModuleFunctions

        Dim strUsePrivileges As String = UCase(System.Configuration.ConfigurationManager.AppSettings("UsePrivileges"))
        If strUsePrivileges = "YES" Then

            If Not objPer.GetAccessRight(Session("_VT_DotNetConnString"), Session("_VT_CurrentUserId"), "CanRunExcelImport") Then
                Dim objDisp As New BPADotNetCommonFunctions.VT_Display.DisplayFuncs
                objDisp.DisplayMessage(Page, "You do not have the correct privilege to access the Excel import. Contact your System Administrator.")
                Exit Sub
            End If
        End If
        Session("_VT_iFrameHeight") = 1500
        Dim strSupp = Replace(ConfigurationManager.AppSettings("VersionManager"), "XXXX", Session("_VT_CurrentDID"))
        Me.CurrentSession.VT_ProjectInIFrame = strSupp + "&Auth=Ok&UID=" + CStr(Session("_VT_CurrentUserId")) + "&MType=ExcelImport"
        If Me.CurrentSession.VT_CurrentCompany <> "" Then
            Me.CurrentSession.VT_ProjectInIFrame += "&Company=" + Me.CurrentSession.VT_CurrentCompany
        End If

        Response.Redirect("~/IFrame Pages/ModulesIFrame.aspx")

    End Sub
End Class
