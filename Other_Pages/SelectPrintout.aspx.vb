Imports BPADotNetCommonFunctions
Imports System.Data.SqlClient
Imports System.Data
Imports System.IO
Imports VTDBFunctions.VTDBFunctions

Partial Class Other_Pages_SelectPrintout
    Inherits MyBasePage


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'Put user code to initialize the page here
        If Not IsPostBack Then
            'Lamont have a different template for Woolworths customer for sales orders. if that template is listed in the wfo_config file then we show only 
            'the regular pdf template and the woolworths template in the dropdown
            Dim objcommon As New VT_CommonFunctions.CommonFunctions
            Dim strWWTemplate As String = objcommon.GetConfigItem("WoolworthsTemplate")
            Dim strWWSalesTemplate As String = objcommon.GetConfigItem("WoolworthsSalesTemplate")
            Dim strDiffDocketPrintouts As String = objcommon.GetConfigItem("DiffDocketPrintouts")
            Dim strBasicTemplate As String
            If strWWTemplate <> "" And Session("VT_FromPage") = "Deliveries" Then
                strBasicTemplate = objcommon.GetConfigItem("TelesalesDocketTemplate")

                If Session("VT_InclPricing") = True Then
                    strWWTemplate = "Priced" & strWWTemplate
                    strBasicTemplate = "Priced" & strBasicTemplate
                End If
                ddlReport.Items.Clear()

                Dim li As New ListItem

                li.Text = Left(strBasicTemplate, InStr(strBasicTemplate, ".") - 1)
                li.Value = strBasicTemplate
                ddlReport.Items.Add(li)

                li = New ListItem

                li.Text = Left(strWWTemplate, InStr(strWWTemplate, ".") - 1)
                li.Value = strWWTemplate
                ddlReport.Items.Add(li)

            ElseIf strWWSalesTemplate <> "" And Session("VT_FromPage") = "Details" Then
                strBasicTemplate = objcommon.GetConfigItem("TelesalesOrderTemplate")

                ddlReport.Items.Clear()

                Dim li As New ListItem

                li.Text = Left(strBasicTemplate, InStr(strBasicTemplate, ".") - 1)
                li.Value = strBasicTemplate
                ddlReport.Items.Add(li)

                li = New ListItem

                li.Text = Left(strWWSalesTemplate, InStr(strWWSalesTemplate, ".") - 1)
                li.Value = strWWSalesTemplate
                ddlReport.Items.Add(li)

            ElseIf strDiffDocketPrintouts <> "" Then 'Added 26/0602014 to cater for customers with different delivery docket printout requirements
                Dim strLocation As String = Server.MapPath("~") + "\FormTemplates\DeliveryDockets"
                lblOrderId.Text = Me.CurrentSession.VT_DocketNumber
                lblOrderNumber.Text = Me.CurrentSession.VT_DocketNumber
                lblTypeTitle.Text = "Docket Num:"

                Dim astrSubs() As String = Split(strDiffDocketPrintouts, ",")

                For i = LBound(astrSubs) To UBound(astrSubs)
                    Dim li As New ListItem
                    li.Text = Path.GetFileNameWithoutExtension(astrSubs(i))
                    li.Value = astrSubs(i)
                    ddlReport.Items.Add(li)
                Next

                ddlReport_SelectedIndexChanged(sender, e)
            Else
                '  lblOrderNumber.Text = Me.CurrentSession.VT_SalesOrderNum
                ' lblOrderId.Text = Me.CurrentSession.VT_SalesOrderID

                ' list all the HTM files in the selected folder
                Dim i As Int16
                Dim strLocation As String '= Server.MapPath("~") + "\Reports\Orders"
                If UCase(Session("VT_FromPage")) = "DETAILS" Then
                    strLocation = Server.MapPath("~") + "\Reports\Orders"
                    lblOrderNumber.Text = Me.CurrentSession.VT_SalesOrderNum
                    lblOrderId.Text = Me.CurrentSession.VT_SalesOrderID
                    lblTypeTitle.Text = "Order Num:"
                Else
                    strLocation = Server.MapPath("~") + "\Reports\Deliveries"
                    lblOrderId.Text = Me.CurrentSession.VT_DocketNumber
                    lblOrderNumber.Text = Me.CurrentSession.VT_DocketNumber
                    lblTypeTitle.Text = "Docket Num:"
                End If

                Dim astrSubs() As String = Directory.GetFiles(strLocation)


                For i = LBound(astrSubs) To UBound(astrSubs)
                    If Right(UCase(astrSubs(i)), 4) = ".HTM" Then
                        ' do not show the resolved files
                        If InStr(astrSubs(i), "_Resolved") = 0 Then
                            Dim li As New ListItem

                            li.Text = Path.GetFileNameWithoutExtension(astrSubs(i))
                            li.Value = astrSubs(i)
                            ddlReport.Items.Add(li)
                        End If
                    End If
                Next

                ddlReport_SelectedIndexChanged(sender, e)
            End If




        End If
    End Sub



    Private Sub ddlReport_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlReport.SelectedIndexChanged

    End Sub

    Private Sub btnRun_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRun.Click

        Dim strFile As String = ddlReport.SelectedValue
        Dim strNewFile As String
        Dim objDisp As New BPADotNetCommonFunctions.VT_Display.DisplayFuncs
        Dim objDB As New SalesOrdersFunctions.SalesOrders

        Dim objCommonFuncs As New BPADotNetCommonFunctions.VT_CommonFunctions.CommonFunctions
        Dim strPrintoutPopup As String = objCommonFuncs.GetConfigItem("PrintoutPopup")
        Dim strDiffDocketPrintouts As String = objCommonFuncs.GetConfigItem("DiffDocketPrintouts")

        ' build the RunTime param string
        Dim strRTParamValues As String
        Dim objcommon As New VT_CommonFunctions.CommonFunctions
        Dim strWWTemplate As String = objcommon.GetConfigItem("WoolworthsTemplate")
        Dim strWWSalesTemplate As String = objcommon.GetConfigItem("WoolworthsSalesTemplate")
        Dim strTemplate As String
        Dim strTemplatePath As String

        If strWWTemplate <> "" And UCase(Session("VT_FromPage")) = "DELIVERIES" Then 'this is Lamont and they have pdf printouts from here specially for woolworths
            strTemplate = ddlReport.SelectedValue
            strTemplatePath = "FormTemplates\DeliveryDockets\" + strTemplate


            Response.Redirect("~\" + strTemplatePath)
        ElseIf strWWSalesTemplate <> "" And UCase(Session("VT_FromPage")) = "DETAILS" Then
            strTemplate = ddlReport.SelectedValue
            strTemplatePath = "FormTemplates\SalesOrders\" + strTemplate


            Response.Redirect("~\" + strTemplatePath)
        ElseIf strDiffDocketPrintouts <> "" Then 'Added 26/06/2014 by JD for customers with different Docket requirements (eg aliyas)
            If UCase(Session("VT_FromPage")) = "DELIVERIES" Then
                strTemplate = ddlReport.SelectedValue
                strTemplatePath = "FormTemplates\DeliveryDockets\" + strTemplate + ".aspx"
                ' Session("_VT_PageToReturnToAfterDisplay") = "~/TabPages/WarehouseManager.aspx"
                Session("_VT_PageToReturnToAfterDisplay") = "~/Other_Pages/SelectPrintout.aspx"
                Response.Redirect("~\" + strTemplatePath)
            End If

        Else

            Dim objCO As New TTHTMLObjectCoordinator.Interface

            If UCase(Session("VT_FromPage")) = "DELIVERIES" Then
                objDB.ExtraDataDocketPrint(Me.CurrentSession.VT_DocketNumber)
                ' strRTParamValues = strRTParamValues + CStr(lblOrderId.Text) + "~"
                strRTParamValues = strRTParamValues + CStr(lblOrderId.Text) + "~"


            ElseIf UCase(Session("VT_FromPage")) = "DETAILS" Then
                objDB.ExtraDataSOPrint(Me.CurrentSession.VT_SalesOrderID)
                strRTParamValues = strRTParamValues + CStr(lblOrderId.Text) + "~" + CStr(Session("_VT_CurrentUserId")) + "~"

            Else
                strRTParamValues = strRTParamValues + CStr(lblOrderId.Text) + "~"

            End If

            '  strRTParamValues = strRTParamValues + CStr(lblOrderId.Text) + "~"

            strNewFile = objCO.ProcessHTMLTemplate(strFile, strRTParamValues, Session("_VT_BPA.NetConnString"))

            ' strip out the portion of the file name that is under the app path
            If UCase(strPrintoutPopup) = "YES" Then
                strNewFile = "../" + Right(strNewFile, Len(strNewFile) - (InStr(strNewFile, Server.MapPath("~")) + Len(Server.MapPath("~"))))

                strNewFile = Replace(strNewFile, "\", "/")

                '  Response.Write("<script type='text/javascript'>detailedresults=window.open('" & strNewFile & "');</script>")

                ScriptManager.RegisterStartupScript(Page, GetType(Page), "OpenWindow", "window.open('" & strNewFile & "');", True)




            Else
                strNewFile = "~\" + Right(strNewFile, Len(strNewFile) - (InStr(strNewFile, Server.MapPath("~")) + Len(Server.MapPath("~"))))
                Response.Redirect(strNewFile)
            End If

        End If

    End Sub

    Protected Sub btnBack_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnBack.Click

        Dim objTelesales As New VTDBFunctions.VTDBFunctions.TelesalesFunctions
        objTelesales.ClearDocketPrintTable(Session("_VT_CurrentUserId"))
        objTelesales.Dispose()

        Select Case Session("VT_FromPage")
            Case "Details"
                Response.Redirect("~/TabPages/Details_Opening.aspx")
            Case "Planning"
                Response.Redirect("~/TabPages/Planning_Opening.aspx")
            Case "OrderList"
                Response.Redirect("~/TabPages/Orders_Opening.aspx")
            Case "Deliveries"
                Response.Redirect("~/TabPages/WarehouseManager.aspx")
            Case Else
                Response.Redirect("~/TabPages/Orders_Opening.aspx")

        End Select
    End Sub


End Class
