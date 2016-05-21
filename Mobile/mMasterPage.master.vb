
Partial Class Mobile_mMasterPage
    Inherits MyMasterPage

    Protected Sub btnTOP_Details_Click(sender As Object, e As EventArgs) Handles btnTOP_Orders.Click

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
            Response.Redirect("mOrdersOpening.aspx")
        End If




    End Sub

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        lblUserName.Text = Session("_VT_CurrentUserName")
    End Sub
    

    Protected Sub btnTOP_More_Click(sender As Object, e As EventArgs) Handles btnTOP_More.Click


        Dim objCommonFuncs As New BPADotNetCommonFunctions.VT_CommonFunctions.CommonFunctions
        Dim strPersonnelLocation As String = objCommonFuncs.GetConfigItem("WhereIsPersonnel")

        If UCase(strPersonnelLocation) = "MATRIX" Then
            Dim objPer As New BPADotNetCommonFunctions.PersonnelModuleFunctions

            If Not objPer.GetAccessRight(Session("_VT_DotNetConnString"), Session("_VT_CurrentUserId"), "CanViewSysAdminTab") Then
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


    Protected Sub btnLogout_Click(sender As Object, e As EventArgs) Handles btnLogout.Click


        Session("Abandon") = "YES"

        Response.Redirect("~/TabPages/Shutdown.aspx")

    End Sub

End Class

