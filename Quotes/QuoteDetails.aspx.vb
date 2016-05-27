Imports BPADotNetCommonFunctions
Imports BPADotNetCommonFunctions.VT_CommonFunctions.MatrixFunctions
Imports System.Data.SqlClient
Imports System.Data
Imports System.Globalization


Partial Class Quotes_QuoteDetails
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

        If Me.CurrentSession.VT_QuoteID = 0 Then
            Me.CurrentSession.VT_QuoteID = SaveNewQuote()
        Else
            UpdateQuoteHeaderData()
        End If


    End Sub

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

        If Not IsPostBack Then


            'IF the Quote ID is blank this is a new Quote so fill with default 
            SetupCustomers()

            If Me.CurrentSession.VT_QuoteID = 0 Then

            Else
                FillData(Me.CurrentSession.VT_QuoteID)
            End If

        End If



    End Sub

    Private Sub SetupCustomers()
        Dim objCust As New VTDBFunctions.VTDBFunctions.CustomerFunctions
        Dim dsCusts As New DataSet
        dsCusts = objCust.GetAllCustomers

        ' we have to manually add the items to the combos because databinding doesn't allow us to 
        ' trim the CustomerNames which often come from SQL with extra spaces added at the end
        Dim i As Integer

        Dim dtCustClone As DataTable = dsCusts.Tables(0).Clone
        dtCustClone.Rows.Clear()

        Dim drNewRoww0 As DataRow = dtCustClone.NewRow()
        drNewRoww0.Item("CustomerId") = 0
        drNewRoww0.Item("CustomerName") = ""
        dtCustClone.Rows.Add(drNewRoww0)

        For i = 0 To dsCusts.Tables(0).Rows.Count - 1
            If (IsDBNull(dsCusts.Tables(0).Rows(i).Item("NotBillingCustomer")) = True Or IIf(IsDBNull(dsCusts.Tables(0).Rows(i).Item("NotBillingCustomer")), 0, dsCusts.Tables(0).Rows(i).Item("NotBillingCustomer")) = 0) And (dsCusts.Tables(0).Rows(i).Item("Status") <> 2) Then

                Dim drNew As DataRow = dtCustClone.NewRow()
                drNew.Item("CustomerId") = Trim(dsCusts.Tables(0).Rows(i).Item("CustomerId"))
                drNew.Item("CustomerName") = Trim(dsCusts.Tables(0).Rows(i).Item("CustomerName"))
                dtCustClone.Rows.Add(drNew)

            End If

        Next

        ddlBillingCustomer.DataSource = dtCustClone
        ddlBillingCustomer.DataTextField = "CustomerName"
        ddlBillingCustomer.DataValueField = "CustomerId"
        ddlBillingCustomer.DataBind()

        'ddlDeliveryCustomer.DataSource = dtCustClone
        'ddlDeliveryCustomer.DataTextField = "CustomerName"
        'ddlDeliveryCustomer.DataValueField = "CustomerId"
        'ddlDeliveryCustomer.DataBind()


    End Sub

    Protected Sub ddlBillingCustomer_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlBillingCustomer.SelectedIndexChanged

            ' set the delivery customer to the same as the selected customer

            'ddlDeliveryCusts.SelectedIndex = ddlCustomers.SelectedIndex
       
            HandleNewCustomerSelection()


     
    End Sub


    Private Sub HandleNewCustomerSelection()
        Dim dsCust As New DataSet
        Dim dsContacts As New DataSet
        Dim objCust As New VTDBFunctions.VTDBFunctions.CustomerFunctions
        Dim lngBillingCustId As Long
        Dim objBPA As New VT_CommonFunctions.CommonFunctions
        Dim j As Integer

        lngBillingCustId = ddlBillingCustomer.SelectedValue

        'If cmbBillingItem Is Nothing OrElse IsNumeric(cmbBillingItem.ID) = False OrElse cmbBillingItem.ID = 0 Then
        '    MsgBox("The billing customer has not been selected properly. Please select the billing customer again.", MsgBoxStyle.OkOnly, "Customer not properly selected")
        '    Exit Sub
        'End If

        dsCust = objCust.GetCustomerDetailsForId(lngBillingCustId)


        ' display the selected customer's details
        If dsCust.Tables(0).Rows.Count > 0 Then
            With dsCust.Tables(0).Rows(0)
                If IIf(IsDBNull(.Item("AccountOnHold")), False, .Item("AccountOnHold")) = True Then

                    lblOnHold.Text = "Yes"
                Else
                    lblOnHold.Text = "No"
                End If

                If IIf(IsDBNull(.Item("VATExempt")), False, .Item("VATExempt")) = True Then
                    lblVATExempt.Text = "Yes"
                Else
                    lblVATExempt.Text = "No"
                End If

                lblTerms.Text = IIf(IsDBNull(.Item("PaymentTerms")), "", .Item("PaymentTerms"))


                lblDiscount.Text = CStr(.Item("DiscountPercent")) + "%"
                lblAddress.Text = Trim(IIf(IsDBNull(.Item("Comment")), "", .Item("Comment")))
                lblPhone.Text = Trim(IIf(IsDBNull(.Item("PhoneNumber")), "", .Item("PhoneNumber")))

            End With
        Else
            lblOnHold.Text = ""
            lblTerms.Text = ""
            txtComment.Text = ""
            lblPhone.Text = ""
        End If

        ' display contacts
        dsContacts = objCust.GetContactsForCustomer(lngBillingCustId)
        If dsContacts.Tables(0).Rows.Count > 0 Then


            'lblContact.Text = dsContacts.Tables(0).Rows(0).Item("ContactName")
            'hdnCustomerContactId.Value = dsContacts.Tables(0).Rows(0).Item("CustomerContactID")




            Dim dtContactClone As DataTable = dsContacts.Tables(0).Clone
            dtContactClone.Rows.Clear()

            Dim drNewRow0 As DataRow = dtContactClone.NewRow()
            drNewRow0.Item("CustomerContactId") = 0
            drNewRow0.Item("ContactName") = ""
            dtContactClone.Rows.Add(drNewRow0)

            For j = 0 To dsContacts.Tables(0).Rows.Count - 1

                Dim drNew As DataRow = dtContactClone.NewRow()
                drNew.Item("CustomerContactId") = Trim(dsContacts.Tables(0).Rows(j).Item("CustomerContactId"))
                drNew.Item("ContactName") = Trim(dsContacts.Tables(0).Rows(j).Item("ContactName"))
                dtContactClone.Rows.Add(drNew)

            Next j

            ddlContact.DataSource = dtContactClone
            ddlContact.DataTextField = "ContactName"
            ddlContact.DataValueField = "CustomerContactId"
            ddlContact.DataBind()

        Else
            ddlContact.Items.Clear()
            hdnCustomerContactId.Value = 0

        End If

        Dim dsCustomersForBilling As New DataSet
        Dim i As Integer
        Dim intIndex As Integer

        'if the checkbox is ticked then show only the customers that have the selected cust as billing customer
        'only do this when the billing customer is selected. NOT the delivery one
        If chkDeliveryCustsOnly.Checked = True Then

            ddlDeliveryCustomer.Items.Clear()

            dsCustomersForBilling = objCust.GetCustomersForBillingCust(lngBillingCustId)

            Dim dtCustClone As DataTable = dsCustomersForBilling.Tables(0).Clone
            dtCustClone.Rows.Clear()

            Dim drNewRoww0 As DataRow = dtCustClone.NewRow()
            drNewRoww0.Item("CustomerId") = lngBillingCustId
            drNewRoww0.Item("CustomerName") = ddlBillingCustomer.SelectedItem.Text
            dtCustClone.Rows.Add(drNewRoww0)

            For i = 0 To dsCustomersForBilling.Tables(0).Rows.Count - 1
                Dim drNew As DataRow = dtCustClone.NewRow()
                drNew.Item("CustomerId") = Trim(dsCustomersForBilling.Tables(0).Rows(i).Item("CustomerId"))
                drNew.Item("CustomerName") = Trim(dsCustomersForBilling.Tables(0).Rows(i).Item("CustomerName"))
                dtCustClone.Rows.Add(drNew)
            Next

            ddlDeliveryCustomer.DataSource = dtCustClone
            ddlDeliveryCustomer.DataTextField = "CustomerName"
            ddlDeliveryCustomer.DataValueField = "CustomerId"
            ddlDeliveryCustomer.DataBind()

            'If ddlBillingCustomer.SelectedItem.Text <> "" Then
            '    ddlDeliveryCustomer.SelectedValue = ddlDeliveryCustomer.Items.FindByText(ddlBillingCustomer.SelectedItem.Text).Value
            'End If
        Else
            dsCustomersForBilling = objCust.GetAllCustomers
            With ddlDeliveryCustomer
                .Items.Clear()
                Dim dtCustClone As DataTable = dsCustomersForBilling.Tables(0).Clone
                dtCustClone.Rows.Clear()


                For i = 0 To dsCustomersForBilling.Tables(0).Rows.Count - 1
                    Dim drNew As DataRow = dtCustClone.NewRow()
                    drNew.Item("CustomerId") = Trim(dsCustomersForBilling.Tables(0).Rows(i).Item("CustomerId"))
                    drNew.Item("CustomerName") = Trim(dsCustomersForBilling.Tables(0).Rows(i).Item("CustomerName"))
                    dtCustClone.Rows.Add(drNew)
                Next

                .DataSource = dtCustClone
                .DataTextField = "CustomerName"
                .DataValueField = "CustomerId"
                .DataBind()


                'If ddlBillingCustomer.SelectedItem.Text <> "" Then
                '    .SelectedValue = .Items.FindByText(ddlBillingCustomer.SelectedItem.Text).Value
                'End If



            End With
        End If

        Dim dsDeliveryCust As New DataSet
        Dim lngDeliveryCustId As Long

        lngDeliveryCustId = ddlDeliveryCustomer.SelectedValue
        dsDeliveryCust = objCust.GetCustomerDetailsForId(lngDeliveryCustId)
        If dsDeliveryCust.Tables(0).Rows.Count > 0 Then
            With dsDeliveryCust.Tables(0).Rows(0)
                lblAddress.Text = Trim(IIf(IsDBNull(.Item("DeliveryAddress")), "", .Item("DeliveryAddress")))
                lblRoute.Text = Trim(IIf(IsDBNull(.Item("Route")), "", .Item("Route")))
            End With
        End If
    End Sub

    Protected Sub ddlDeliveryCustomer_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlDeliveryCustomer.SelectedIndexChanged

        Dim dsCust As New DataSet
        Dim objCust As New VTDBFunctions.VTDBFunctions.CustomerFunctions
        Dim lngDeliveryCustId As Long

        lngDeliveryCustId = ddlDeliveryCustomer.SelectedValue
        dsCust = objCust.GetCustomerDetailsForId(lngDeliveryCustId)
        If dsCust.Tables(0).Rows.Count > 0 Then
            With dsCust.Tables(0).Rows(0)
                lblAddress.Text = Trim(IIf(IsDBNull(.Item("DeliveryAddress")), "", .Item("DeliveryAddress")))
                lblRoute.Text = Trim(IIf(IsDBNull(.Item("Route")), "", .Item("Route")))
            End With
        End If
    End Sub




    Sub FillData(ByVal OrderId As Integer)

       

        Dim dtQuoteDetails As New DataTable
        Dim objQuote As New VTDBFunctions.VTDBFunctions.QuoteFunctions
        Dim sender As Object
        Dim e As System.EventArgs


        dtQuoteDetails = objQuote.GetQuoteHeaderInfoForID(OrderId)

        If dtQuoteDetails Is Nothing OrElse dtQuoteDetails.Rows.Count < 1 Then
        Else

            ddlBillingCustomer.SelectedIndex = (ddlBillingCustomer.Items.IndexOf(ddlBillingCustomer.Items.FindByValue(dtQuoteDetails.Rows(0).Item("CustomerId"))))
            ddlBillingCustomer_SelectedIndexChanged(sender, e)
            ddlDeliveryCustomer.SelectedIndex = (ddlDeliveryCustomer.Items.IndexOf(ddlDeliveryCustomer.Items.FindByValue(dtQuoteDetails.Rows(0).Item("DeliveryCustomerId"))))
            ddlDeliveryCustomer_SelectedIndexChanged(sender, e)
            txtComment.Text = Trim(IIf(IsDBNull(dtQuoteDetails.Rows(0).Item("Comment")), "", dtQuoteDetails.Rows(0).Item("Comment")))
            txtCustomerRef.Text = Trim(IIf(IsDBNull(dtQuoteDetails.Rows(0).Item("CustomerPO")), "", dtQuoteDetails.Rows(0).Item("CustomerPO")))
            lblRoute.Text = Trim(IIf(IsDBNull(dtQuoteDetails.Rows(0).Item("Route")) = True, "", dtQuoteDetails.Rows(0).Item("Route")))
            lblDateStarted.Text = FormatDateTime(dtQuoteDetails.Rows(0).Item("DateCreated"), DateFormat.ShortDate)
            lblDateIssued.Text = If(IsDBNull(dtQuoteDetails.Rows(0).Item("DateIssued")), "", (FormatDateTime(dtQuoteDetails.Rows(0).Item("DateIssued"), DateFormat.ShortDate)))

            ddlContact.SelectedIndex = (ddlContact.Items.IndexOf(ddlContact.Items.FindByValue(dtQuoteDetails.Rows(0).Item("CustomerContactId"))))
            dteRequestedDeliveryDate.Value = dtQuoteDetails.Rows(0).Item("RequestedDeliveryDate")

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
        If ddlBillingCustomer.SelectedItem.Text = "" Then
            ValidateMe = GetGlobalResourceObject("resource", "NoCustomerSelectedForQuote")
            Exit Function
        End If
        If dteRequestedDeliveryDate.Text = dteRequestedDeliveryDate.NullDateLabel Then
            ValidateMe = GetGlobalResourceObject("resource", "NoRequestedDeliveryDate")
            Exit Function

        End If
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


   

    Public Function SaveNewQuote() As Long

        Dim objEQO As New VT_eQOInterface.eQOInterface
        Dim lngQuoteNum As Long
        Dim ds As DataSet
        Dim strTemp As String
        '  Dim objCommon As New TTWFOCommonFunctions.TOFunctions
        Dim objTrace As New VTDBFunctions.VTDBFunctions.TraceFunctions
        Dim objMatrix As New VTDBFunctions.VTDBFunctions.VTMatrixFunctions

        Dim strPO As String
        Dim lngQuoteId As Long
      
        Dim objForms As New VT_Forms.Forms
        Dim objDisp As New VT_Display.DisplayFuncs
      
        Dim objQuotes As New VTDBFunctions.VTDBFunctions.QuoteFunctions
        Dim intProgramId As Integer
        Dim dteDeliveryDate As Date
        Dim strContactName As String
        Dim intContactId As Long


        Dim lngCustForOrders, lngDeliveryCustForOrders As Long


        lngCustForOrders = ddlBillingCustomer.SelectedValue
        lngDeliveryCustForOrders = ddlDeliveryCustomer.SelectedValue

        ' create new job

        ' *********** 'Get programid
        Dim objCommonFuncs As New BPADotNetCommonFunctions.VT_CommonFunctions.CommonFunctions
        Dim strProgramID As String = objCommonFuncs.GetConfigItem("QuotesJobID")
        If strProgramID <> "" Then
            intProgramId = CInt(strProgramID)
        Else
            intProgramId = 0
        End If

        ''''''''''''''''''''''''''''
        ' 1. Create a new workflow Job for this application
        ''''''''''''''''''''''''''''
        ' we need to find the Workflow ProgramId for this JobStep
        Dim strAuditComment As String = "New Quote Workflow (ProgramID:" + CStr(intProgramId) + ") started."
     

        'extract the Week and the Year from the date
        Dim myCI As New System.Globalization.CultureInfo("en-US")
        Dim myCal As System.Globalization.Calendar = myCI.Calendar
        ' Gets the DTFI properties required by GetWeekOfYear.
        Dim myCWR As System.Globalization.CalendarWeekRule = myCI.DateTimeFormat.CalendarWeekRule
        Dim myFirstDOW As DayOfWeek = myCI.DateTimeFormat.FirstDayOfWeek



        lngQuoteNum = objEQO.StartWorkflow(intProgramId, "Quote", strAuditComment, Session("_VT_CurrentUserName"), Session("_VT_CurrentUserId"), "Quote Taker", "", 0)



      


        Me.CurrentSession.VT_QuoteNum = lngQuoteNum

        ''''''''''''''''''''''''''''
        ' 2. Add the batch details to the db
        ''''''''''''''''''''''''''''
        objForms.SetBatchDatesForJob(lngQuoteNum, System.DateTime.UtcNow.Date, System.DateTime.UtcNow.Date.Year, myCal.GetWeekOfYear(System.DateTime.UtcNow.Date, myCWR, myFirstDOW))
        '      objForms.set()


        objForms.SetJobStatusText(GetGlobalResourceObject("Resource", "Quote_New"), lngQuoteNum)
        objEQO.SetLastActivity(lngQuoteNum, System.DateTime.UtcNow.Date)

        Me.CurrentSession.VT_CurrentQuoteStatus = GetGlobalResourceObject("Resource", "Quote_New")


        ' save items to the ExtraData fields in the BatchTable so we can see them in the Order grid
        ' 1. get the customer name  store it in ExtraData1
        strTemp = Trim(ddlBillingCustomer.SelectedItem.Text)

        objForms.SetWFOExtraDataItem(lngQuoteNum, "ExtraData1", strTemp)

        ' 2. store the PO Number in ExtraData2
        strPO = Trim(txtCustomerRef.Text)

        objForms.SetWFOExtraDataItem(lngQuoteNum, "ExtraData2", strPO)


        ' 4. get the delivery customer name  and store it in ExtraData4
        strTemp = Trim(ddlDeliveryCustomer.SelectedItem.Text)

        objForms.SetWFOExtraDataItem(lngQuoteNum, "ExtraData4", strTemp)



        ''''''''''''''''''''''''''''
        ' 3. Write the WorkflowJobId, JobStep, JobStepId into the application's record in the Matrix
        ''''''''''''''''''''''''''''
        Dim lngRootId As Long = objMatrix.GetFormId(Session("_VT_DotNetConnString"), QuoteFormListTable, QuoteModuleRootNode, 0)
        Dim lngQuotesFolder As Long = objMatrix.GetFormId(Session("_VT_DotNetConnString"), QuoteFormListTable, QuoteModuleCategoriesNode, lngRootId)
        Dim lngQuoteCategory As Long
        Dim intMatrixLink As Integer
        Dim lngDetailsForm As Long

        lngQuoteCategory = objMatrix.GetFormId(Session("_VT_DotNetConnString"), QuoteFormListTable, "Standard", lngQuotesFolder)


        intMatrixLink = objMatrix.GetFormId(Session("_VT_DotNetConnString"), QuoteFormListTable, CStr(lngQuoteNum), lngQuoteCategory)
      
        objMatrix.InsertVTMatrixFormDataItem(Session("_VT_DotNetConnString"), QuoteFormDataTable, intMatrixLink, "WorkflowId", lngQuoteNum)
        objMatrix.InsertVTMatrixFormDataItem(Session("_VT_DotNetConnString"), QuoteFormDataTable, intMatrixLink, "ParentId", lngQuoteCategory)
        objMatrix.InsertVTMatrixFormDataItem(Session("_VT_DotNetConnString"), QuoteFormDataTable, intMatrixLink, "ItemId", intMatrixLink)
        objMatrix.InsertVTMatrixFormDataItem(Session("_VT_DotNetConnString"), QuoteFormDataTable, intMatrixLink, "OwnerId", Session("_VT_CurrentUserId"))
        objMatrix.InsertVTMatrixFormDataItem(Session("_VT_DotNetConnString"), QuoteFormDataTable, intMatrixLink, "OwnerName", Session("_VT_CurrentUserName"))


        lngDetailsForm = objMatrix.GetFormId(Session("_VT_DotNetConnString"), QuoteFormListTable, "Details", intMatrixLink)

        objMatrix.InsertVTMatrixFormDataItem(Session("_VT_DotNetConnString"), QuoteFormDataTable, lngDetailsForm, "WorkflowId", lngQuoteNum)
        objMatrix.InsertVTMatrixFormDataItem(Session("_VT_DotNetConnString"), QuoteFormDataTable, lngDetailsForm, "ParentId", intMatrixLink)
        objMatrix.InsertVTMatrixFormDataItem(Session("_VT_DotNetConnString"), QuoteFormDataTable, lngDetailsForm, "ItemId", lngDetailsForm)
        objMatrix.InsertVTMatrixFormDataItem(Session("_VT_DotNetConnString"), QuoteFormDataTable, lngDetailsForm, "OwnerId", Session("_VT_CurrentUserId"))
        objMatrix.InsertVTMatrixFormDataItem(Session("_VT_DotNetConnString"), QuoteFormDataTable, lngDetailsForm, "OwnerName", Session("_VT_CurrentUserName"))
   

        ''''''''''''''''''''''''''''
        ' 4. Write the MatrixId, ProductionLine and StepId to the batch Table
        ''''''''''''''''''''''''''''
        objForms.SetMatrixLinkForJob(lngQuoteNum, intMatrixLink)

        Me.CurrentSession.VT_QuoteMatrixID = intMatrixLink
     

        ''''''''''''''''''''''''''''
        ' 9. we must also create a new node in VTStore to store any attachments for this item
        '' '' '' ''''''''''''''''''''''''''''
        Dim objVTStore As New VTStoreVer2Functions

        Dim strNodeText As String = "Quotes\QuoteNum_" + Trim(lngQuoteNum.ToString)

        objVTStore.CreateVTStoreFolderFromPath(strNodeText)
        ' ====================



        ''''''''''''''''''''''''''''
        ' 10. Audit Logging
        ''''''''''''''''''''''''''''
        Dim strType As String
        ' strAuditComment = "New Quote created"
        strType = "Quote"
        objForms.WriteToAuditLog(lngQuoteNum, strType, Now, Session("_VT_CurrentUserName"), Session("_VT_CurrentUserId"), strAuditComment, "Audit Record", "SysAdmin")




        ' now store to Quote table
        strPO = Trim(txtCustomerRef.Text)

        If dteRequestedDeliveryDate.Value = Nothing OrElse dteRequestedDeliveryDate.Value = #12:00:00 AM# Then
            dteDeliveryDate = System.DateTime.UtcNow.Date
        Else
            dteDeliveryDate = dteRequestedDeliveryDate.Value
        End If

        strContactName = ddlContact.SelectedItem.Text
        intContactId = ddlContact.SelectedValue


        ' create new Quote
        objQuotes.SaveNewQuote(lngQuoteNum, lngCustForOrders, lngDeliveryCustForOrders, intContactId, strContactName, dteDeliveryDate, Session("_VT_CurrentUserId"), Trim(txtComment.Text),
                                System.DateTime.UtcNow.Date, strPO, Session("_VT_CurrentUserName"), lngQuoteNum, Trim(lblRoute.Text))

        ' now get the id of the quote
        ' this function gets the most recent quote 
        lngQuoteId = objQuotes.GetLastAddedQuoteID


        'Start the sequence
        Dim objSequencer As New WFOSequencer
        objSequencer.DllPath = Server.MapPath("~") & "\Bin"

        objSequencer.AdvanceSequence(Me.CurrentSession.VT_QuoteNum, Session("_VT_CurrentUserId"), False, 0, 0, Session("_VT_DotNetConnString"))


        ' clear the variable where we stored the table of quotes so that it is redrawn when we go back to the Quotes page
        Me.CurrentSession.VT_QuotesDataTable = Nothing


        SaveNewQuote = lngQuoteId
    End Function

    Public Sub UpdateQuoteHeaderData()

        Dim objQuotes As New VTDBFunctions.VTDBFunctions.QuoteFunctions
        Dim lngCustForOrders, lngDeliveryCustForOrders As Long
        Dim strPO As String
        Dim strStatus As String
        Dim strTemp As String
        Dim objForms As New VT_Forms.Forms
        Dim strContactName As String
        Dim intContactId As Integer

        lngCustForOrders = ddlBillingCustomer.SelectedValue
        lngDeliveryCustForOrders = ddlDeliveryCustomer.SelectedValue
        strPO = Trim(txtCustomerRef.Text)
        strStatus = Me.CurrentSession.VT_CurrentQuoteStatus


        strContactName = ddlContact.SelectedItem.Text
        intContactId = ddlContact.SelectedValue

        objQuotes.UpdateQuoteHeaderDetails(Me.CurrentSession.VT_QuoteID, lngCustForOrders, intContactId, strContactName, lngDeliveryCustForOrders, dteRequestedDeliveryDate.Value, Session("_VT_CurrentUserId"), _
                                           Trim(txtComment.Text), strPO, Session("_VT_CurrentUserName"), strStatus, Trim(lblRoute.Text))


        ' save items to the ExtraData fields in the BatchTable so we can see them in the Order grid
        ' 1. get the customer name  store it in ExtraData1
        strTemp = Trim(ddlBillingCustomer.SelectedItem.Text)

        objForms.SetWFOExtraDataItem(Me.CurrentSession.VT_QuoteNum, "ExtraData1", strTemp)

        ' 2. store the PO Number in ExtraData2
        strPO = Trim(txtCustomerRef.Text)

        objForms.SetWFOExtraDataItem(Me.CurrentSession.VT_QuoteNum, "ExtraData2", strPO)


        ' 4. get the delivery customer name  and store it in ExtraData4
        strTemp = Trim(ddlDeliveryCustomer.SelectedItem.Text)

        objForms.SetWFOExtraDataItem(Me.CurrentSession.VT_QuoteNum, "ExtraData4", strTemp)


    End Sub
   

    Protected Sub Button1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim objSequencer As New WFOSequencer
        objSequencer.DllPath = Server.MapPath("~") & "\Bin"

        objSequencer.AdvanceSequence(Me.CurrentSession.VT_QuoteNum, Session("_VT_CurrentUserId"), False, 0, 0, Session("_VT_DotNetConnString"))

    End Sub
End Class
