Imports BPADotNetCommonFunctions
Imports BPADotNetCommonFunctions.VT_CommonFunctions.MatrixFunctions
Imports BPADotNetCommonFunctions.SalesFunctions.Sales_Orders

Imports System.Data.SqlClient
Imports System.Data
Imports System.Globalization
Imports System.Web.UI

Partial Class Mobile_mOrderDetails
    Inherits MyBasePage

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        Dim objDataPreserve As New BPADotNetCommonFunctions.VT_DataPreserve.DataPreserve
        Dim dtCustomerItems As New DataTable
        Dim objUtil As New VTDBFunctions.VTDBFunctions.UtilFunctions
        Dim objSales As New BPADotNetCommonFunctions.SalesFunctions.Sales_Orders

        Dim strConn As String = Session("_VT_DotNetConnString")

        'btnSearch.Attributes.Add("onclick", "return ShowCustSearchPopup();")
        'cmdSearch.OnClientClick = "SearchDeliveryCust()"
        '' cmdSearch.Attributes.Add("onclick", "SearchDeliveryCust()")
        'cmdSelect.OnClientClick = String.Format("return SelectCustomer('{0}','{1}')", cmdSelect.UniqueID, "")

        If Not IsPostBack Then

            Me.CurrentSession.VT_SelectedSectionPageGridRow_V2 = Nothing
            Dim objcommon As New BPADotNetCommonFunctions.VT_CommonFunctions.CommonFunctions
            Dim strAccounts As String = UCase(objcommon.GetConfigItem("AccountsPackage"))
            Session("Accounts") = strAccounts

        

            Dim objCommonFuncs As New BPADotNetCommonFunctions.VT_CommonFunctions.CommonFunctions
            Dim strAllowDifferentOrderTypes As String = objCommonFuncs.GetConfigItem("DifferentSalesOrderTypes")
            Dim strDefaultOrderType As String = objCommonFuncs.GetConfigItem("DefaultOrderType")
            'Display the customer code as part of the name or not
            Dim inclCustomerCodes As String = objCommonFuncs.GetConfigItem("InclCustomerCodes")

            If UCase(strAllowDifferentOrderTypes) = "YES" Then
                Me.CurrentSession.VT_blnAllowDifferentOrderTypes = True
            Else
                Me.CurrentSession.VT_blnAllowDifferentOrderTypes = False
            End If

            Me.CurrentSession.VT_strDefaultOrderType = Trim(UCase(strDefaultOrderType))

            'Display the customer code as part of the name or not
            Me.CurrentSession.VT_inclCustomerCodes = False
            If UCase(inclCustomerCodes) = "YES" Then
                Me.CurrentSession.VT_inclCustomerCodes = True
            End If




            'IF the Order ID is 0 this is a new order so fill with default 
            If Me.CurrentSession.VT_NewOrderID = 0 Then
                dteDelDate.Value = System.DateTime.UtcNow.Date
                dteDelDate.Text = System.DateTime.UtcNow.Date
                dteOrderDate.Value = System.DateTime.UtcNow.Date
                dteOrderDate.Text = System.DateTime.UtcNow.Date

                InitialiseAndBindEmptyCustomerItemsGrid()

                SetSoldToDropDownDefaults("")

            Else

                SetSoldToDropDownDefaults("")

                'Read sales order data and fill relevant fields
                FillData(Me.CurrentSession.VT_NewOrderID)
                'Get Customer Items from the Matrix
                dtCustomerItems = objSales.GetSalesOrderItemsFromMatrix("CustomerItems", CInt(Me.CurrentSession.VT_NewOrderNum), strConn, Me.CurrentSession.VT_tlsNumFields)

                'SmcN 28/04/2014 Check if the 'BillTo_Address' column exists and if not then add it.
                'Needed to add this here to ensure compatiblty with existing live orders.
                If dtCustomerItems.Columns.Contains("BillTo_Address") = False Then
                    dtCustomerItems.Columns.Add("BillTo_Address")
                End If
                If dtCustomerItems.Columns.Contains("Other_SalesPersonCode") = False Then
                    dtCustomerItems.Columns.Add("Other_SalesPersonCode")
                End If



                objDataPreserve.BindDataToWDG(dtCustomerItems, wdgCustomerItems)

                'Re-set the Customer drop downs and any other items that need intial loading from the Matrix
                If Not IsNothing(dtCustomerItems) AndAlso dtCustomerItems.Rows.Count > 0 Then
                    If dtCustomerItems.Rows.Count > 0 Then
                        Dim objcust As New VTDBFunctions.VTDBFunctions.CustomerFunctions
                        Dim dtCustSchema As New DataSet


                        With dtCustomerItems.Rows(0)
                            If IsNumeric(.Item("SoldTo_ID")) Then
                                dtCustSchema = objcust.GetCustomerForId(.Item("SoldTo_ID"))
                                If dtCustSchema.Tables(0).Rows.Count > 0 Then
                                    .Item("SoldTo_CategoryID") = dtCustSchema.Tables(0).Rows(0).Item("CustomerAreaID")
                                End If
                            End If


                            'select the customer for this list
                            ddlBillingCustomer.SelectedValue = .Item("SoldTo_Id")
                            Me.CurrentSession.strNewOrderCustomerName = ddlBillingCustomer.SelectedItem.Text

                            'set up delivery customers

                            'Select the customers for this list
                            lblDeliverTo.Text = .Item("DeliverTo_Name")


                            'Select the customers for this list
                            If Val(.Item("BillTo_Id")) > 0 Then
                                Me.CurrentSession.VT_SelectedBillToIdInVerifyDB = .Item("BillTo_Id")
                            Else
                                Me.CurrentSession.VT_SelectedBillToIdInVerifyDB = 0
                            End If

                            'ddlContact.SelectedIndex = .Item("BillTo_ContactId")

                            'Setup the extra Data fields
                            If .Item("DeliverTo_OrderDate") <> "" Then
                                Dim dteTemp As Date = .Item("DeliverTo_OrderDate")
                                dteOrderDate.Text = Format(dteTemp, "d")
                                dteOrderDate.Value = Format(dteTemp, "d")
                            Else
                                dteOrderDate.Text = System.DateTime.UtcNow
                                dteOrderDate.Text = System.DateTime.UtcNow
                            End If




                        End With

                    End If
                End If

            End If


            Session("SoldToCustChanged") = False
            Session("DeliveryCustChanged") = False
            Session("SoldToContactChanged") = False
            Session("BillingAddressChanged") = False
            Session("DeliveryAddressChanged") = False
            Session("CustomerPOChanged") = False
            Session("ReqDeliveryDateChanged") = False
            Session("CustomerOrderDateChanged") = False
            'show or hide the top mfg pro customer selection panel

            If Session("VT_DelCustomerName") <> "" AndAlso Not Session("VT_DelCustomerName") Is Nothing Then
                lblDeliverTo.Text = Session("VT_DelCustomerName")
                hdnDeliveryCust.Value = Session("VT_DelCustomerID")
            End If

            'if a delivery customer has been selected then fill the controls
            If Not Session("VT_DelCustomerName") Is Nothing Then
                lblDeliverTo.Text = Session("VT_DelCustomerName")
                hdnDeliveryCust.Value = Session("VT_DelCustomerID")
                ddlBillingCustomer.SelectedItem.Value = Session("VT_BillingCustId")
                ddlBillingCustomer.SelectedIndex = Session("VT_BillingCustSelectedIndex")
            End If
        Else


            'This is a postback so rebind the grid
            wdgCustomerItems.DataSource = objDataPreserve.GetWDGDataFromSession(wdgCustomerItems)

        End If


        Me.CurrentSession.VT_CustomerPO = txtCustPO.Text

    End Sub

    Sub FillData(ByVal OrderId As Integer)

        Dim dsOrderDetails As New DataSet
        Dim objTelesales As New VTDBFunctions.VTDBFunctions.TelesalesFunctions

        Dim dsOrderJobDetails As New DataSet
        Dim objPdn As New VTDBFunctions.VTDBFunctions.Production
        Dim objcust As New VTDBFunctions.VTDBFunctions.CustomerFunctions


        dsOrderDetails = objTelesales.GetOrderForId(OrderId)

        If dsOrderDetails.Tables(0) Is Nothing OrElse dsOrderDetails.Tables(0).Rows.Count < 1 Then

        Else
            lblOrderNum.Text = CStr(Me.CurrentSession.VT_NewOrderNum) & " ___ Sales Order Num [ " & CStr(Me.CurrentSession.VT_SalesContiguousNum) & " ]"

            ddlBillingCustomer.SelectedIndex = (ddlBillingCustomer.Items.IndexOf(ddlBillingCustomer.Items.FindByValue(dsOrderDetails.Tables(0).Rows(0).Item("CustomerId"))))
            lblDeliverTo.Text = objcust.GetCustomerNameForId(dsOrderDetails.Tables(0).Rows(0).Item("DeliveryCustomerId"))
            hdnDeliveryCust.Value = dsOrderDetails.Tables(0).Rows(0).Item("DeliveryCustomerId")

            txtComment.Text = Trim(IIf(IsDBNull(dsOrderDetails.Tables(0).Rows(0).Item("Comment")), "", dsOrderDetails.Tables(0).Rows(0).Item("Comment")))
            txtCustPO.Text = Trim(IIf(IsDBNull(dsOrderDetails.Tables(0).Rows(0).Item("CustomerPO")), "", dsOrderDetails.Tables(0).Rows(0).Item("CustomerPO")))

            dteDelDate.Value = dsOrderDetails.Tables(0).Rows(0).Item("RequestedDeliveryDate")
            dteDelDate.Text = dsOrderDetails.Tables(0).Rows(0).Item("RequestedDeliveryDate")
            Dim dteTemp As Date
            dteTemp = dteDelDate.Text
            Session("_VT_SalesPortal_DefaultRequestedDate") = dteTemp.ToString("s")

            'Get the priority from the wfo_batchtable 
            dsOrderJobDetails = objPdn.GetWFOBatchTableData(dsOrderDetails.Tables(0).Rows(0).Item("SalesOrderNum"))

        End If


        'Get order type settings

        Session("SoldToCustChanged") = False
        Session("DeliveryCustChanged") = False
        Session("SoldToContactChanged") = False
        Session("BillingAddressChanged") = False
        Session("DeliveryAddressChanged") = False
        Session("CustomerPOChanged") = False
        Session("ReqDeliveryDateChanged") = False
        Session("CustomerOrderDateChanged") = False


    End Sub


    Sub SetSoldToDropDownDefaults(ByVal strMessage As String)

        Dim objCust As New VTDBFunctions.VTDBFunctions.CustomerFunctions
        Dim objG As New BPADotNetCommonFunctions.VT_ReportFunctions.GeneralFunctions
        Dim dtSearch As New DataTable
        Dim dtSellToCustomers As New DataTable
        Dim dtCustCategories As New DataTable
        Dim strConn As String = Session("_VT_DotNetConnString")
        Dim dsTemp As New DataSet
        Dim dtTemp As New DataTable
        Dim objUtil As New VTDBFunctions.VTDBFunctions.UtilFunctions



        dtTemp.Clear()
        dtTemp = objCust.getAllCustomersAndReferences


        If dtTemp.Rows.Count > 0 Then
            ddlBillingCustomer.DataSource = dtTemp
            ddlBillingCustomer.DataTextField = "CustNameAndRef"
            ddlBillingCustomer.DataValueField = "CustomerId"
            ddlBillingCustomer.DataBind()
        Else
            ' Set the drop down for no data
            ddlBillingCustomer.Items.Add(New ListItem("No Customers saved!", "-20"))

        End If


    End Sub
    Sub InitialiseAndBindEmptyCustomerItemsGrid()

        Dim dtCustomerItems As New DataTable
        Dim objDataPreserve As New BPADotNetCommonFunctions.VT_DataPreserve.DataPreserve

        'There is no sales order yet so build a blank datatable and bind the grid to this
        dtCustomerItems.Columns.Add("SoldTo_CategoryID")
        dtCustomerItems.Columns.Add("SoldTo_Category")
        dtCustomerItems.Columns.Add("SoldTo_Id")
        dtCustomerItems.Columns.Add("SoldTo_Name")
        dtCustomerItems.Columns.Add("SoldTo_Code")
        dtCustomerItems.Columns.Add("SoldTo_Address")
        dtCustomerItems.Columns.Add("SoldTo_OnHold")
        dtCustomerItems.Columns.Add("SoldTo_FaxNum")
        dtCustomerItems.Columns.Add("SoldTo_TaxExempt")
        dtCustomerItems.Columns.Add("SoldTo_ContactId")
        dtCustomerItems.Columns.Add("SoldTo_ContactName")
        dtCustomerItems.Columns.Add("SoldTo_Phone")
        dtCustomerItems.Columns.Add("SoldTo_Terms")

        dtCustomerItems.Columns.Add("DeliverTo_CategoryID")
        dtCustomerItems.Columns.Add("DeliverTo_Category")
        dtCustomerItems.Columns.Add("DeliverTo_Id")
        dtCustomerItems.Columns.Add("DeliverTo_Name")
        dtCustomerItems.Columns.Add("DeliverTo_Code")
        dtCustomerItems.Columns.Add("DeliverTo_Route")
        dtCustomerItems.Columns.Add("DeliverTo_ContactId")
        dtCustomerItems.Columns.Add("DeliverTo_ContactName")
        dtCustomerItems.Columns.Add("DeliverTo_Phone")
        dtCustomerItems.Columns.Add("DeliverTo_Address")
        dtCustomerItems.Columns.Add("DeliverTo_OrderDate")
        dtCustomerItems.Columns.Add("DeliverTo_RequestedDate")
        dtCustomerItems.Columns.Add("DeliverTo_DateOut")
        dtCustomerItems.Columns.Add("DeliverTo_DateArrival")

        dtCustomerItems.Columns.Add("BillTo_Id")
        dtCustomerItems.Columns.Add("BillTo_Name")
        dtCustomerItems.Columns.Add("BillTo_Code")
        dtCustomerItems.Columns.Add("BillTo_ContactId")
        dtCustomerItems.Columns.Add("BillTo_ContactName")
        dtCustomerItems.Columns.Add("BillTo_Phone")
        dtCustomerItems.Columns.Add("BillTo_Address")

        dtCustomerItems.Columns.Add("Other_OrderType")
        dtCustomerItems.Columns.Add("Other_CustPo")
        dtCustomerItems.Columns.Add("Other_Priority")
        dtCustomerItems.Columns.Add("Other_Site")
        dtCustomerItems.Columns.Add("Other_QuoteRef")
        dtCustomerItems.Columns.Add("Other_Comment")
        dtCustomerItems.Columns.Add("Other_Interstat")
        dtCustomerItems.Columns.Add("Other_DeliveryTerms")
        dtCustomerItems.Columns.Add("Other_SalesPersonCode")



        'Bind the Grid
        objDataPreserve.BindDataToWDG(dtCustomerItems, wdgCustomerItems)

    End Sub

    Public Function SaveNewOrder(ByRef strMessage As String) As Long

        Dim objSales As New SalesFunctions.Sales_Orders
        Dim lngNewOrderID As Long

        If Val(ddlBillingCustomer.SelectedValue) = 0 Then
            SaveNewOrder = 0
            strMessage = "Invalid customer selection"
            Exit Function
        End If
        If IsNumeric(hdnDeliveryCust.Value) = False Then
            hdnDeliveryCust.Value = ddlBillingCustomer.SelectedValue
        End If

        lngNewOrderID = objSales.SaveNewSalesOrder(ddlBillingCustomer.SelectedValue, hdnDeliveryCust.Value, dteOrderDate.Value, dteDelDate.Value, txtComment.Text, IIf(rbPaymenttype.SelectedIndex = 0, "Cash", "Credit"), txtCustPO.Text)
        Dim objSalesFuncs As New VTDBFunctions.VTDBFunctions.TelesalesFunctions

        Dim strConn As String = Session("_VT_DotNetConnString")
        Dim dt As New DataSet

        dt = objSalesFuncs.GetOrderForId(lngNewOrderID)
        Dim lngneworderNum As Long
        If dt.Tables(0).Rows.Count > 0 Then
            lngneworderNum = dt.Tables(0).Rows(0).Item("SalesOrderNum")
        End If


        Me.CurrentSession.VT_NewOrderNum = lngNewOrderNum
        Me.CurrentSession.VT_SalesOrderNum = lngNewOrderNum
        'Set the selected Item ID
        Me.CurrentSession.SelectedItemID = lngneworderNum

        Me.CurrentSession.VT_SalesContiguousNum = Session("VT_SalesContiguousNum")
        lblOrderNum.Text = Me.CurrentSession.VT_SalesContiguousNum

        Me.CurrentSession.VT_CurrentNewOrderStatus = GetGlobalResourceObject("Resource", "Order_PreIssued")

        Me.CurrentSession.VT_NewOrderMatrixID = Session("VT_NewOrderMatrixID")
        Me.CurrentSession.VT_OrderMatrixID = Session("VT_NewOrderMatrixID")

        Me.CurrentSession.VT_SalesOrderID = lngNewOrderID

        SaveNewOrder = lngNewOrderID

    End Function

    Protected Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        Dim strMessage As String
        Dim lngNewOrderId As Long

        lngNewOrderId = SaveNewOrder(strMessage)

        If lngNewOrderId = 0 Then
            'show message

        End If
    End Sub


    Protected Sub btnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.Click
        If ddlBillingCustomer.SelectedValue = 0 Then
            Exit Sub
        End If
        Session("VT_BillingCustId") = ddlBillingCustomer.SelectedValue
        Session("VT_BillingCustSelectedIndex") = ddlBillingCustomer.SelectedIndex
        Response.Redirect("mSelectCustomer.aspx")

    End Sub

    Private Sub ddlBillingCustomer_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlBillingCustomer.SelectedIndexChanged
        'by default the delivery customer should be set as the billing customer that was selected
        If ddlBillingCustomer.SelectedValue > 0 Then
            hdnDeliveryCust.Value = ddlBillingCustomer.SelectedValue
            lblDeliverTo.Text = ddlBillingCustomer.SelectedItem.Text
            Session("VT_DelCustomerId") = ddlBillingCustomer.SelectedItem.Value
            Session("VT_DelCustomerName") = ddlBillingCustomer.SelectedItem.Text
            Session("VT_BillingCustId") = ddlBillingCustomer.SelectedValue
        End If
    End Sub

    Private Sub btnAddCust_Click(sender As Object, e As EventArgs) Handles btnAddCust.Click
        If txtNewCust.Text = "" Then
            Exit Sub

        End If

        'if a customer name is added here then it will be added to the verify system only, not sage, as a delivery
        'customer for the selected billing customer.

        Session("VT_BillingCustId") = ddlBillingCustomer.SelectedValue
        If ddlBillingCustomer.SelectedValue > 0 Then
            Session("VT_DelCustomerId") = 0
            Session("VT_DelCustomerName") = txtNewCust.Text
            Response.Redirect("mAddDelCust.aspx")
        End If

    End Sub
    Public Function ValidateMe() As String

        ValidateMe = ""
    End Function
    Function DataToSave() As Boolean

        DataToSave = True

    End Function

    Public Function StoreFormSpecificData() As String

        Dim strMessage As String = ""

        If Me.CurrentSession.VT_NewOrderID = 0 Then
            Me.CurrentSession.VT_NewOrderID = SaveNewOrder(strMessage)
        End If

        If Me.CurrentSession.VT_NewOrderID <> 0 Then
            UpdateOrderHeaderData()
        End If
        StoreFormSpecificData = strMessage

        Session("SoldToCustChanged") = False
        Session("DeliveryCustChanged") = False
        Session("SoldToContactChanged") = False
        Session("BillingAddressChanged") = False
        Session("DeliveryAddressChanged") = False
        Session("CustomerPOChanged") = False
        Session("ReqDeliveryDateChanged") = False
        Session("CustomerOrderDateChanged") = False
    End Function


    Public Sub UpdateOrderHeaderData()

        Dim objTele As New VTDBFunctions.VTDBFunctions.TelesalesFunctions
        Dim lngCustForOrders, lngDeliveryCustForOrders As Long
        Dim strPO As String
        Dim strStatus As String
        Dim strTemp As String
        Dim objForms As New VT_Forms.Forms
        Dim strContactName As String
        Dim intContactId As Integer
        Dim strType As String
        Dim dteDeliveryDate As Date
        Dim objCust As New VTDBFunctions.VTDBFunctions.CustomerFunctions
        Dim objDataPreserve As New BPADotNetCommonFunctions.VT_DataPreserve.DataPreserve
        Dim objSales As New BPADotNetCommonFunctions.SalesFunctions.Sales_Orders

        Dim strConn As String = Session("_VT_DotNetConnString")
        Dim aTemp() As String

        Dim objData As New BPADotNetCommonFunctions.VT_DataPreserve.DataPreserve
        Dim objVTM As New VTDBFunctions.VTDBFunctions.VTMatrixFunctions
        Dim intIdToUse As Integer
        Dim strAuditComment As String

        'SmcN 25/05/2014 Only run this save function if the sales order is checked out to the current user (i.e. the user is actually editing this Sales Order)
        intIdToUse = Me.CurrentSession.VT_NewOrderMatrixID
        Dim blnIsFormCheckedOutByCurrentUser As Boolean = objData.IsFormCheckedOutByCurrentUser(Session("_VT_DotNetConnString"), Session("_VT_CurrentUserId"), intIdToUse, Me.CurrentSession.FormDataTable)
        If blnIsFormCheckedOutByCurrentUser = False Then
            'This Sales Order is not checked out to the current user so EXIT
            Exit Sub
        End If



        lngCustForOrders = ddlBillingCustomer.SelectedItem.Value
        lngDeliveryCustForOrders = hdnDeliveryCust.Value

        strPO = Trim(txtCustPO.Text)
        strStatus = Me.CurrentSession.VT_CurrentNewOrderStatus

        Dim dsContact As New Data.DataSet

        dsContact = objCust.GetContactsForCustomer(lngCustForOrders)
        If dsContact.Tables(0).Rows.Count > 0 Then
            strContactName = dsContact.Tables(0).Rows(0).Item("ContactName")
            intContactId = dsContact.Tables(0).Rows(0).Item("ContactID")
        End If

        Dim objTrace As New VTDBFunctions.VTDBFunctions.TraceDataFunctions

        If dteDelDate.Value = Nothing OrElse dteDelDate.Value = #12:00:00 AM# Then
            dteDeliveryDate = System.DateTime.UtcNow.Date
        Else
            dteDeliveryDate = dteDelDate.Value
        End If



        'Save the delivery contact
        Dim strDelContactName As String
        Dim lngDelContactId As Long


        dsContact = objCust.GetContactsForCustomer(lngDeliveryCustForOrders)
        If dsContact.Tables(0).Rows.Count > 0 Then
            strDelContactName = dsContact.Tables(0).Rows(0).Item("ContactName")
            lngDelContactId = dsContact.Tables(0).Rows(0).Item("ContactID")
        End If


        'need to get the sold to and deliver to categories
        Dim lngSoldToCategory As Long
        Dim strsoldToCategory As String
        Dim lngDeliverToCategory As Long
        Dim strDeliverToCategory As String
        Dim dtCustomer As DataSet
        Dim strBillingAddress As String
        Dim strDelAddress As String

        dtCustomer = objCust.GetCustomerForId(lngCustForOrders)
        If dtCustomer.Tables(0).Rows.Count > 0 Then
            lngSoldToCategory = dtCustomer.Tables(0).Rows(0).Item("CustomerAreaId")
            strsoldToCategory = Left(ddlBillingCustomer.Text, 1)
            strBillingAddress = IIf(IsDBNull(dtCustomer.Tables(0).Rows(0).Item("BillingAddress")), "", dtCustomer.Tables(0).Rows(0).Item("BillingAddress"))
        End If

        dtCustomer = objCust.GetCustomerForId(lngDeliveryCustForOrders)
        If dtCustomer.Tables(0).Rows.Count > 0 Then
            lngDeliverToCategory = dtCustomer.Tables(0).Rows(0).Item("CustomerAreaId")
            strDeliverToCategory = Left(lblDeliverTo.Text, 1)
            strDelAddress = IIf(IsDBNull(dtCustomer.Tables(0).Rows(0).Item("BillingAddress")), "", dtCustomer.Tables(0).Rows(0).Item("BillingAddress"))

        End If

        strType = ""

        strPO = Trim(txtCustPO.Text)

        Dim dtCustomerItems As New DataTable
        Dim objCustFunctions As New BPADotNetCommonFunctions.CustomerModuleFunctions

        dtCustomerItems = objCustFunctions.UpdateOrderHeaderDataReturnDT(Me.CurrentSession.VT_NewOrderMatrixID, lngCustForOrders, lngDeliveryCustForOrders, strPO, strStatus, strContactName,
                                                                         intContactId, dteDeliveryDate, dteOrderDate.Value, Trim(txtComment.Text), Me.CurrentSession.VT_NewOrderID, "1", strDelContactName, lngDelContactId, "", wdgCustomerItems,
                                                                         lngSoldToCategory, strsoldToCategory, "", "", "", "", strDeliverToCategory, lngDeliverToCategory, strBillingAddress, strDelAddress)

        'Now bind the grid to these items
        objDataPreserve.BindDataToWDG(dtCustomerItems, wdgCustomerItems)

        Dim objcommon As New BPADotNetCommonFunctions.VT_CommonFunctions.CommonFunctions
        Me.CurrentSession.VT_tlsNumFields = objcommon.GetConfigItem("tlsNumFieldsInGridDataTables")
        Dim strTeleDebugLog As String = objcommon.GetConfigItem("TelesalesDebugLog")
        If UCase(strTeleDebugLog) = "ON" Then
            Dim strLog As String
            Dim f As New BPADotNetCommonFunctions.VT_CommonFunctions.CommonFunctions
            strLog = "Me.CurrentSession.VT_NewOrderNum: " & Me.CurrentSession.VT_NewOrderNum & " wdgCustomerItems.count: " & wdgCustomerItems.Rows.Count & " strConn: " & strConn & " Me.CurrentSession.VT_tlsNumFields: " & Me.CurrentSession.VT_tlsNumFields
            f.DebugLog(0, strLog, "Sales Portal", System.DateTime.UtcNow)
        End If

        'Then save this grid to the Matrix
        objSales.SaveSalesItemsToMatrix("CustomerItems", wdgCustomerItems, CInt(Me.CurrentSession.VT_NewOrderNum), strConn, Me.CurrentSession.VT_tlsNumFields)


    End Sub
End Class
