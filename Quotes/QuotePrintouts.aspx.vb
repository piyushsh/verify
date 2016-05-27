Imports BPADotNetCommonFunctions
Imports BPADotNetCommonFunctions.VT_CommonFunctions.MatrixFunctions
Imports System.Data.SqlClient
Imports System.Data
Imports System.IO

Partial Class Quotes_QuotePrintouts
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
        objVT.InsertVTMatrixDataItem(Session("_VT_DotNetConnString"), QuoteFormListTable, QuoteFormDataTable, Me.CurrentSession.VT_QuoteMatrixID, Me.CurrentSession.VT_QuoteSelectedFormName, "PrintQuoteNum", Me.CurrentSession.VT_QuoteNum)


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


        'Put user code to initialize the page here
        If Not IsPostBack Then
            Dim strLocation As String = Server.MapPath("~") + "\Quote-Reports"
            Dim i As Int16
            Dim astrSubs() As String = Directory.GetFiles(strLocation)

            For i = LBound(astrSubs) To UBound(astrSubs)
                If Right(UCase(astrSubs(i)), 4) = ".HTM" Then
                    ' do not show the resolved files
                    If InStr(astrSubs(i), "_Resolved") = 0 Then
                        Dim li As New ListItem

                        li.Text = Path.GetFileNameWithoutExtension(astrSubs(i))
                        li.Value = astrSubs(i)
                        ddlReports_NS.Items.Add(li)
                    End If
                End If
            Next


            'IF the Quote ID is blank this is a new Quote so fill with default 

            Me.CurrentSession.VT_QuoteSelectedFormName = "QuotePrintouts"

            If Me.CurrentSession.VT_QuoteID = 0 Then

            Else
                '   FillData(Me.CurrentSession.VT_QuoteID)
            End If


            'lbl_WorkOrderID.Text = Me.CurrentSession.VT_CurrentJobWorkflowID.ToString


            Dim objData As New BPADotNetCommonFunctions.VT_DataPreserve.DataPreserve

            Dim Inner_CP As ContentPlaceHolder
            Inner_CP = GetInnerContentPlaceHolder()

            objData.RecoverFormData(Inner_CP, Me.CurrentSession.VT_QuoteSelectedFormName, Me.CurrentSession.VT_QuoteMatrixID, QuoteFormListTable, QuoteFormDataTable)



        End If

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


    Protected Sub btnPrint_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnPrint.Click





        Dim strFile As String = ddlReports_NS.SelectedValue
        Dim objCO As New TTHTMLObjectCoordinator.Interface
        Dim strNewFile As String
        Dim objDisp As New BPADotNetCommonFunctions.VT_Display.DisplayFuncs
        Dim lngReportid As Long

        Dim objCommonFuncs As New BPADotNetCommonFunctions.VT_CommonFunctions.CommonFunctions
        Dim strPrintoutPopup As String = objCommonFuncs.GetConfigItem("PrintoutPopup")


        ' build the RunTime param string
        Dim strRTParamValues As String


        Dim objDB As New LocalQuoteFunctions.Quotes


        Select Case Trim(CStr(ddlReports_NS.SelectedItem.Text))
            Case "StandardWasteQuote"
                objDB.StandardWasteQuotePreProcessing(Me.CurrentSession.VT_QuoteNum)
           
            Case Else
        End Select


        strRTParamValues = CStr(Me.CurrentSession.VT_QuoteNum) + "~"



        strNewFile = objCO.ProcessHTMLTemplate(strFile, strRTParamValues, Session("_VT_BPA.NetConnString"))

        ' strip out the portion of the file name that is under the app path
        If UCase(strPrintoutPopup) = "YES" Then
            strNewFile = "../" + Right(strNewFile, Len(strNewFile) - (InStr(strNewFile, Server.MapPath("~")) + Len(Server.MapPath("~"))))

            strNewFile = Replace(strNewFile, "\", "/")

            Response.Write("<script type='text/javascript'>detailedresults=window.open('" & strNewFile & "');</script>")

        Else
            strNewFile = "~\" + Right(strNewFile, Len(strNewFile) - (InStr(strNewFile, Server.MapPath("~")) + Len(Server.MapPath("~"))))
            Response.Redirect(strNewFile)
        End If



    End Sub
End Class
