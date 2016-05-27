Imports BPADotNetCommonFunctions
Imports System.Data.SqlClient
Imports System.Data
Imports System.IO
Imports VTDBFunctions.VTDBFunctions

Partial Class TabPages_Printouts_Opening
    Inherits MyBasePage

    Const cView = 0
    Const cReference = 1
    Const cName = 2
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'Put user code to initialize the page here
        If Not IsPostBack Then
            Dim strLocation As String = Server.MapPath("~") + "\Reports"
            Dim i As Int16

            Dim astrSubs() As String = Directory.GetDirectories(strLocation)
            For i = LBound(astrSubs) To UBound(astrSubs)
                ddlCategory.Items.Add(Right(astrSubs(i), Len(astrSubs(i)) - InStrRev(astrSubs(i), "\")))
            Next

            ddlCategory_SelectedIndexChanged(sender, e)

        End If
    End Sub

    Private Sub ddlCategory_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlCategory.SelectedIndexChanged
        ' list all the HTM files in the selected folder
        Dim i As Int16
        Dim strLocation As String = Server.MapPath("~") + "\Reports"
        Dim astrSubs() As String = Directory.GetFiles(strLocation + "\" + ddlCategory.SelectedItem.Text)

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

    End Sub

    Private Sub ddlReport_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlReport.SelectedIndexChanged
        ' retrieve the Runtime parameters from the HTM file
        Dim strReportFile As String = ddlReport.SelectedValue
        Dim sr As New StreamReader(strReportFile)
        Dim strData, strRTParamNames, strParamName, strParamType As String
        Dim intRTStart, lngRTEnd, i, j As Integer

        strData = sr.ReadToEnd
        sr.Close()

        intRTStart = InStr(strData, "RunTimeParamNames>")
        If intRTStart <> 0 Then
            intRTStart = InStr(intRTStart, strData, ">")
            lngRTEnd = InStr(intRTStart + 1, strData, "<")
            strRTParamNames = Mid(strData, intRTStart + 1, (lngRTEnd - intRTStart) - 1)
        Else
            Dim objDisp As New BPADotNetCommonFunctions.VT_Display.DisplayFuncs
            objDisp.DisplayMessage(Page, "Run Time Parameter string not found in Report file: " + strReportFile)
            Exit Sub
        End If


        ' now load the grid with the Parameter details
        Dim astrParamDetails() As String
        Dim astrParams() As String = Split(strRTParamNames, "^")
        Dim MyRow As Infragistics.WebUI.UltraWebGrid.UltraGridRow
        uwgParams.Rows.Clear()
        For i = LBound(astrParams) To UBound(astrParams) - 1
            astrParamDetails = Split(astrParams(i), ":")
            strParamName = astrParamDetails(0)
            strParamType = astrParamDetails(1)
            ' add a new row using the ProductId as a key
            uwgParams.Rows.Add(strParamName)

            'Get a reference to the new row
            MyRow = uwgParams.Rows.FromKey(strParamName)
            MyRow.Cells.FromKey("Parameter Name").Value = strParamName
            MyRow.Cells.FromKey("Type").Value = strParamType

            Select strParamType
                Case "Number"
                Case "Date"
                    MyRow.Cells.FromKey("Value").Value = "dd/MM/yyyy"
                Case "Boolean"
                Case Else
            End Select

                'Select Case strParamType
                '    Case "Number"
                '        uwgParams.Columns.FromKey("Value").DataType = "System.Int32"
                '    Case "Date"
                '        uwgParams.Columns.FromKey("Value").DataType = "System.DateTime"
                '        uwgParams.Columns.FromKey("Value").Format = "dd/MM/yyyy"
                '    Case "Boolean"
                '        uwgParams.Columns.FromKey("Value").DataType = "System.Boolean"
                '    Case Else
                '        uwgParams.Columns.FromKey("Value").DataType = "System.String"
                'End Select

        Next

    End Sub

    Private Sub btnRun_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRun.Click
        Dim strFile As String = ddlReport.SelectedValue
        Dim objCO As New TTHTMLObjectCoordinator.Interface
        Dim strNewFile As String
        Dim MyRow As Infragistics.WebUI.UltraWebGrid.UltraGridRow
        Dim strParamType As String
        Dim objDisp As New BPADotNetCommonFunctions.VT_Display.DisplayFuncs
        Dim objDB As New SalesOrdersFunctions.SalesOrders

        For Each MyRow In uwgParams.Rows
            If MyRow.Cells.FromKey("Value").Value = "" Then
                objDisp.DisplayMessage(Page, "You must supply a value for the " + MyRow.Cells.FromKey("Parameter Name").Value + " parameter!")
                Exit Sub
            End If
        Next

        ' validate the data
        For Each MyRow In uwgParams.Rows
            strParamType = MyRow.Cells.FromKey("Type").Value
            Select Case strParamType
                Case "Number"
                    If Not IsNumeric(MyRow.Cells.FromKey("Value").Value) Then
                        objDisp.DisplayMessage(Page, "Parameter: " + MyRow.Cells.FromKey("Parameter Name").Value + " must be Numeric!")
                        Exit Sub
                    End If
                Case "Date"
                    If Not IsDate(MyRow.Cells.FromKey("Value").Value) Then
                        objDisp.DisplayMessage(Page, "Parameter: " + MyRow.Cells.FromKey("Parameter Name").Value + " must be a Date!")
                        Exit Sub
                    End If
                    MyRow.Cells.FromKey("Value").Value = Format(MyRow.Cells.FromKey("Value").Value, "Short Date")
                Case "Boolean"
                    If UCase(MyRow.Cells.FromKey("Value").Value) <> "TRUE" And UCase(MyRow.Cells.FromKey("Value").Value) <> "FALSE" Then
                        objDisp.DisplayMessage(Page, "Parameter: " + MyRow.Cells.FromKey("Parameter Name").Value + " must be either True or False!")
                        Exit Sub
                    End If
                Case Else
            End Select

        Next
        Select Case strParamType
            Case "Number"
                uwgParams.Columns.FromKey("Value").DataType = "System.Int32"
            Case "Date"
                uwgParams.Columns.FromKey("Value").DataType = "System.DateTime"
                uwgParams.Columns.FromKey("Value").Format = "Date"
            Case "Boolean"
                uwgParams.Columns.FromKey("Value").DataType = "System.Boolean"
            Case Else
                uwgParams.Columns.FromKey("Value").DataType = "System.String"
        End Select

        ' build the RunTime param string
        Dim strRTParamValues As String
        For Each MyRow In uwgParams.Rows
            strRTParamValues = strRTParamValues + CStr(MyRow.Cells.FromKey("Value").Value) + "~"
        Next


        If Right(strFile, 23) = "DeliveryDocketPrice.htm" Then
            objDB.PricedDocketPrint(strRTParamValues)
        End If
        If Right(strFile, 11) = "Invoice.htm" Then
            objDB.PricedInvoicePrint(strRTParamValues)
        End If
        If Right(strFile, 37) = "UniboardCustomerOutstandingOrders.htm" Then
            objDB.UniboardReportForCustomer(strRTParamValues)
        End If





        strNewFile = objCO.ProcessHTMLTemplate(strFile, strRTParamValues)

        ' strip out the portion of the file name that is under the app path
        strNewFile = "~\" + Right(strNewFile, Len(strNewFile) - (InStr(strNewFile, Server.MapPath("~")) + Len(Server.MapPath("~"))))
        Response.Redirect(strNewFile)

    End Sub

    
End Class
