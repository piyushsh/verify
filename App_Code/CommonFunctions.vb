Imports Microsoft.VisualBasic
Imports BPADotNetCommonFunctions
Imports System.Data.SqlClient
Imports System.Data
Imports VTDBFunctions.VTDBFunctions
Imports System.Collections.Generic
Imports Microsoft.Office.Interop


Public Class MySessionInfo


    '''''''''''''''
    ' The following are values that are currently being stored in Session
    ' It was hoped to use the MySessionInfo class to tidy up the usage of these values.
    '
    ' However this was not possible because some of these variables are referenced in 
    ' other projects such as BPADotNetCommonFunctions.dll
    '
    '''''''''''''''
    '' Stores the ConnectionString for all .NET code
    'Public _VT_DotNetConnString As String
    '' Stores the ConnectionString to be passed to all COM functions
    'Public _VT_BPANetConnString As String
    '' Flag to be checked to see if the Session is still Live
    'Public _VT_SessionLive As String
    '' Flag read from Web.Config to controls if we log errors
    'Public _VT_LoggingState As String
    '' Role assigned to current user
    'Public _VT_Role As String
    '' The visible name for the Current User
    'Public _VT_CurrentUserName As String
    '' The number of invalid login attempts in the current session
    'Public _VT_NumLoginAttempts As Integer
    '' Flag that says if we are in Demo mode or not
    'Public _VT_DemoMode As String
    '' Flag that says if we are in Training mode or not
    'Public _VT_TrainingMode As String
    '' the current Database ID 
    'Public _VT_CurrentDID As String
    '' The current user's company as read from the Personnel module
    'Public _VT_CurrentUserCompany As String
    '' The current user's ID as read from the Personnel module
    'Public _VT_CurrentUserId As String
    '' the current user's eMail as read from the Personnel module
    'Public _VT_CurrentUserEMail As String
    '' The current user's password as read from the Personnel module
    'Public _VT_CurrentUserPassword As String
    '' The current user's IP address read from Request.UserHostAddress
    'Public _VT_UserHostAddress As String
    '' The current user's originating website read from Request.UrlReferrer
    'Public _VT_Referrer As String
    '''''''''''''''

    '
    '
    ' New variables that require to be available across pages should be stored here
    '
    ' the company to which the data applies
    Public VT_CurrentCompany As String
    ' Error values that need to be passed between pages
    Public VT_Error As String
    ' Current active Tab in Grid1_Open page
    Public VT_Top_CurrentTab As String
    ' Tab that was clicked in Grid1_Open page
    Public VT_TopTab_Clicked As String
    ' Current active doc type selected in Grid1_Open page
    Public VT_Grid_CurrentButton As String
    ' Button that was clicked in Grid1_Open page
    Public VT_GridButton_Clicked As String
    ' Stores the external project we want to go to through the IFrame page
    Public VT_ProjectInIFrame As String


    Public VT_GridFilterText As String
    Public VT_GridFilterTextQuote As String

    'SmcN 25/05/2014 Added a string to handle filtering on Matrix columns within the Sales Order grid
    Public VT_MatrixFilterText As String
    Public VT_SOPrimaryOrderNumKey As String

    'SmcN 02/06/2014 Added Values relating to the Client Browsers configuration
    Public VT_BrowserWindowWidth As String

    Public VT_CameFromInTray As String

    Public VT_CurrentTaskId As Integer
    Public VT_CurrentJobWorkflowID As Long

    Public VT_JobName As String

    Public VT_JobID As Long
    Public VT_JobType As String
    Public VT_AllowAdjustments As String
    Public VT_JobStatus As String

    Public VT_SalesOrderID As Long
    Public VT_SalesOrderNum As Long
    Public VT_SalesContiguousNum As String
    Public VT_CustomerID As Integer
    Public VT_CustomerName As String
    Public VT_CustomerPO As String
    Public VT_OrderTypesEnabled As String
    Public VT_OrderType As String


    Public VT_PlanningStartDate As String
    Public VT_PlanningEndDate As String


    Public VT_SelectedFormType As String
    Public VT_FilterOn As Boolean
    Public VT_CurrentApplicationWorkflowID As Long
    Public VT_MailServer As String
    Public VT_MailUser As String
    Public VT_MailPW As String
    Public VT_JobComment As String


    Public FormListTable As String
    Public ModuleRootNode As String
    Public ModuleCategoriesNodea As String
    Public ModuleRootNodeID As String
    Public ModuleCategoriesNode As String

    Public VT_DeliveryCurrentStartDate As Date
    Public VT_DeliveryCurrentEndDate As Date

    Public VT_ProdSelectProdID As Long
    Public VT_OrdersDataTable As Data.DataTable
    Public VT_SelectedOrderRecordId As Long
    Public VT_SelectedUnitPrice As Double

    Public VT_OrderPriority As String
    Public VT_DocketNumber As String
    Public VT_CalcQtyFromWgt As Boolean
    Public VT_DeliveryCustomerID As Long
    Public VT_OrderPO As String
    Public VT_OrderDate As Date
    Public VT_SearchString As String
    Public VT_SearchStringQuotes As String
    Public VT_UserSerialNumbers As Boolean
    Public VT_DecimalPlaces As Integer

    Public FormDataTable As String
    Public CurrentModuleType As String

    'Variables for CheckIN/CHeckOut and Form Locking
    Public VT_FormCheckedOutToCurrentUser As String

    ' stores the Id of the currently selected item
    Public SelectedItemID As Integer
    ' stores the current company/department passed in from the calling portal
    Public CurrentCompanyName As String
    ' stores the name of the currently selected item
    Public SelectedItemName As String
    ' stores the VersionId of the currently selected item
    Public SelectedItemVersionID As Integer

    ' stores the status of the currently selected item
    Public SelectedItemStatus As String
    ' stores the version status of the currently selected item - draft, current etc
    Public SelectedItemVersionStatus As String
    ' stores the version of the currently selected item - V1.00, V1.23 etc
    Public SelectedItemVersion As String
    ' stores the Description of the selected category
    Public SelectedCategoryDesc As String
    ' stores the name of the selected category
    Public SelectedCategory As String
    ' stores the name of the selected category
    Public SelectedCategoryID As Integer

    ' Variables for storing the set of form pages 
    Public aVT_DetailsPageOptionsPages() As String
    Public aVT_DetailsPageOptions() As String

    ' Variables for storing the set of Module pages 
    Public aVT_ModulePageOptionsPages() As String
    Public aVT_ModulePageOptions() As String

    ' Variable to store the current selected form page row
    Public VT_SelectedItemGridRow As Infragistics.WebUI.UltraWebGrid.UltraGridRow
    ' Variable to store the current selected form page row
    Public VT_SelectedDetailsGridRow As Infragistics.WebUI.UltraWebGrid.UltraGridRow
    Public VT_SelectedSectionPageGridRow_V2 As DataTable

    Public VT_SelectedSalesOrderRow_V2 As DataTable

    Public VT_SelectedItemGridRow_V2 As DataTable
    Public VT_SelectedDetailsGridRow_V2 As DataTable
    Public VT_SelectedOrderItemGridRow_V2 As DataTable
    Public VT_SelectedItemProdsGridRow_V2 As DataTable
    Public VT_SelectedItemLocnsGridRow_V2 As DataTable

    Public VT_SelectedFulfillDetailsGridRow_V2 As DataTable
    Public VT_SelectedFulfillDetailsProdsGridRow_V2 As DataTable
    Public VT_SelectedFulfillDetailsLocnsGridRow_V2 As DataTable

    Public VT_wdgTraceCodes As DataTable ''full grid data
    Public VT_wdgTraceCodesSelectedRow As DataTable ''only the data for the selected row

    'this is for the top grid on the fulfill page only

    Public VT_SelectedFulfilOrderItemsGridRow_V2 As DataTable

    Public VT_AddingProductsSelectedRow_V2 As DataTable
    Public VT_ServiceSelectedRow_V2 As DataTable

    Public VT_SelectedDeliverySummaryRow_V2 As DataTable
    Public VT_SelectedDeliveryItemRow_V2 As DataTable
    Public VT_SelectedDeliveryServiceItemRow_V2 As DataTable
    Public VT_blnInclPricesOnDocket As Boolean

    'for the grid on the customer select page
    Public VT_SelectedDeliveryCustRow As DataTable

    ' stores the Id of the CurrentUser's Role
    Public CurrentUserRoleId As Integer
    ' stores the name of the CurrentUser's Role
    Public CurrentUserRoleName As String
    ' store the category of the current user
    Public CurrentUserCategory As String

    ' stores the FirstName of a newly created user
    Public NewUserFirstName As String
    ' stores the Surname of a newly created user
    Public NewUserSurname As String
    ' stores the Name of a newly created item
    Public NewItemName As String


    ' Store the currently selected Competency type
    Public CurrentCompetencyType As String
    ' stores the page to return to after selecting a competency
    Public ReturnFromSelectCompetency As String
    ' stores the page to return to after selecting a course provider
    Public ReturnFromSelectProvider As String
    ' stores the page to return to after changing a user's role
    Public ReturnFromSelectRole As String

    ' stores the page to return to after viewing AuditLog etc
    Public OptionsPageToReturnTo As String
    'stores a true value when the status of an order has changed. used to refresh grid or not when clicking between tabs
    Public blnOrderStatusChanged As Boolean
    'stores the customer name for the billing customer that is selected for a new order on the new order details page
    Public strNewOrderCustomerName As String

    ' Store the contents of the  category grid so we can redraw it quickly
    Public VT_CategoryGridData As Data.DataSet

    ' variable to store the WorkflowId of the current Item
    Public CurrentItemWorkflowId As Integer

    ' Variable to pass the selected page from the New form page
    Public VT_AppPageToShow As String

    ' Flag that controls whether to AuditLog the SavePageData function or not
    Public DoAuditLog As Boolean

    ' stores the Name of the primary form (usually the Details one) which is used to create the record for a new item
    Public PrimaryFormName As String
    ' stores the name of the field on the Primary form that gives the items Name
    Public PrimaryItemName As String
    ' stores the Message that appears if you try to Save a new item without supplying thr PrimaryItem data
    Public PrimaryItemMessage As String

    ' name of the control we are jumping to from the comments window
    Public VT_ControlToJumpTo As String

    ' variable to save the approvalDetails for a new item
    Public ApprovalDetails As GenericFunctions.ApprovalDetails = Nothing


    Public VT_QuotesDataTable As Data.DataTable
    Public VT_SelectedQuoteRecordId As Long
    Public VT_QuoteNum As Long
    Public VT_QuoteID As Long
    Public VT_CurrentQuoteStatus As String
    Public VT_QuoteDataTable As Data.DataTable
    Public VT_QuoteCurrentStartDate As Date
    Public VT_QuoteCurrentEndDate As Date
    Public VT_QuoteCustomerID As Long
    Public VT_QuoteDeliveryCustomerID As Long
    Public VT_QuoteUseBillingCustomer As Boolean
    Public VT_QuoteSelectedFormName As String
    Public VT_QuoteMatrixID As Integer

    Public VT_IsWasteSystem As Boolean
    Public VT_SelectedFormPath As String

    Public VT_NewOrderNum As Long
    Public VT_NewOrderID As Long
    Public VT_CurrentNewOrderStatus As String
    Public VT_NewOrderCustomerID As Long
    Public VT_NewOrderDeliveryCustomerID As Long
    Public VT_NewOrderUseBillingCustomer As Boolean
    Public VT_NewOrderSelectedFormName As String

    Public VT_blnAllowDifferentOrderTypes As Boolean
    Public VT_strDefaultOrderType As String
    Public VT_inclCustomerCodes As Boolean

    Public VT_NewOrderMatrixID As Integer
    Public VT_OrderMatrixID As Long

    Public VT_UserAuthenticated As Boolean


    Public VT_AllowOrderEditBeforeCommit As Boolean

    'to store the selected trace code and locationid when adding a specific batch to a new order
    Public VT_SelectedTraceCode As String
    Public VT_SelectedLocationId As Integer
    Public VT_SelectedLocationText As String

    Public VT_SelectedSoldToIdInVerifyDB As Long
    Public VT_SelectedDeliverToIdInVerifyDB As Long
    Public VT_SelectedBillToIdInVerifyDB As Long
    'session variable to store the number of fields in the gridData tables
    Public VT_tlsNumFields As Integer
    Public VT_SelectedFormName As String

End Class

Public Module PortalFunctions
    Public Function Now() As DateTime
        Dim objNow As New VTDBFunctions.VTDBFunctions.UtilFunctions
        Return objNow.VerifyNow()
    End Function
End Module


Public Class MyBasePage
    Inherits System.Web.UI.Page


    Public Property CurrentSession() As MySessionInfo
        Get
            If Session("_VT_MySessionInfo") Is Nothing Then
                Dim thisSession As New MySessionInfo
                Session("_VT_MySessionInfo") = thisSession
            End If

            Return Session("_VT_MySessionInfo")
        End Get

        Set(ByVal value As MySessionInfo)
            Session("_VT_MySessionInfo") = value
        End Set
    End Property

    <System.Web.Services.WebMethod()>
    Public Shared Function IsThisOrderLockedForEdit(ByVal strSalesOrderNum As String) As String
        Dim strFormLock As String
        Dim objVTM As New VTDBFunctions.VTDBFunctions.VTMatrixFunctions
        Dim lngId As Long
        Dim objForms As New VT_Forms.Forms
        Dim lngUserId As Long = HttpContext.Current.Session("_VT_CurrentUserId")

        lngId = objForms.GetMatrixLinkIdForJob(strSalesOrderNum)
        ' get the CheckIn status for this item
        strFormLock = objVTM.ReadFormLock(HttpContext.Current.Session("_VT_DotNetConnString"), lngId, "tls_FormData")
        If strFormLock <> "" Then 'it is locked
            IsThisOrderLockedForEdit = strFormLock

        Else
            IsThisOrderLockedForEdit = "NO"
        End If


    End Function

    <System.Web.Services.WebMethod()>
    Public Shared Function GetTraceCodeIDForTraceDescAndProdId(ByVal strTraceCodeDesc As String, ByVal lngproductID As Long) As String

        Dim objTrace As New VTDBFunctions.VTDBFunctions.TraceFunctions
        Dim strTraceId As String

        strTraceId = objTrace.GetTraceCodeIDForDescAndProduct(lngproductID, strTraceCodeDesc)

        GetTraceCodeIDForTraceDescAndProdId = strTraceId

    End Function

    <System.Web.Services.WebMethod()>
    Public Shared Function GetTraceCodeExistsForTraceDescAndProdId(ByVal strTraceCodeDesc As String, ByVal lngproductID As Long) As String

        Dim objTrace As New VTDBFunctions.VTDBFunctions.TraceFunctions
        Dim lngTraceId As Long

        lngTraceId = objTrace.GetTraceCodeIDForDescAndProduct(lngproductID, strTraceCodeDesc)

        If lngTraceId = 0 Then
            GetTraceCodeExistsForTraceDescAndProdId = "Entered Trace Code Does not Exist for selected Product."
        Else
            GetTraceCodeExistsForTraceDescAndProdId = ""

        End If

    End Function

    <System.Web.Services.WebMethod()>
    Public Shared Function CheckExistsForSerialNumAndTraceId(ByVal strSerial As String, ByVal lngTraceCodeIdID As Long) As String

        Dim objSerial As New VTDBFunctions.VTDBFunctions.SerialNumFunctions
        Dim dsSerial As New DataSet



        '.Add("ProductCode", System.Type.GetType("System.String"))
        '.Add("ProductName", System.Type.GetType("System.String"))
        '.Add("Weight", System.Type.GetType("System.Double"))
        '.Add("Quantity", System.Type.GetType("System.Double"))
        '.Add("TraceCode", System.Type.GetType("System.String"))
        '.Add("SerialNum", System.Type.GetType("System.String"))
        '.Add("Barcode", System.Type.GetType("System.String"))
        '.Add("Location", System.Type.GetType("System.String"))
        '.Add("SellByDate", System.Type.GetType("System.String"))
        '.Add("LocationPosition", System.Type.GetType("System.String"))
        '.Add("ProductionDate", System.Type.GetType("System.String"))
        '.Add("ProductId", System.Type.GetType("System.Int64"))
        '.Add("LocationId", System.Type.GetType("System.Int64"))

        If lngTraceCodeIdID <> 0 Then
            dsSerial = objSerial.GetDetailsForSerialNumAndTraceId(strSerial, lngTraceCodeIdID)


            If dsSerial.Tables(0).Rows.Count <> 0 Then
                CheckExistsForSerialNumAndTraceId = ""
            Else
                CheckExistsForSerialNumAndTraceId = "Serial Number Does not Exist in selected batch"
            End If
        Else
            CheckExistsForSerialNumAndTraceId = ""
        End If

    End Function

    <System.Web.Services.WebMethod()>
    Public Shared Function GetCurrentQtyForSerialNumAndTraceId(ByVal strSerial As String, ByVal lngTraceCodeIdID As Long) As String

        Dim objSerial As New VTDBFunctions.VTDBFunctions.SerialNumFunctions
        Dim dsSerial As New DataSet

        If lngTraceCodeIdID <> 0 Then

            dsSerial = objSerial.GetDetailsForSerialNumAndTraceId(strSerial, lngTraceCodeIdID)

            If dsSerial.Tables(0).Rows.Count <> 0 Then
                GetCurrentQtyForSerialNumAndTraceId = dsSerial.Tables(0).Rows(0).Item("Quantity")
            Else
                GetCurrentQtyForSerialNumAndTraceId = ""
            End If
        Else
            GetCurrentQtyForSerialNumAndTraceId = ""
        End If
    End Function

    <System.Web.Services.WebMethod()>
    Public Shared Function GetCurrentWgtForSerialNumAndTraceId(ByVal strSerial As String, ByVal lngTraceCodeIdID As Long) As String

        Dim objSerial As New VTDBFunctions.VTDBFunctions.SerialNumFunctions
        Dim dsSerial As New DataSet

        If lngTraceCodeIdID <> 0 Then

            dsSerial = objSerial.GetDetailsForSerialNumAndTraceId(strSerial, lngTraceCodeIdID)

            If dsSerial.Tables(0).Rows.Count <> 0 Then
                GetCurrentWgtForSerialNumAndTraceId = dsSerial.Tables(0).Rows(0).Item("Weight")
            Else
                GetCurrentWgtForSerialNumAndTraceId = ""
            End If
        Else
            GetCurrentWgtForSerialNumAndTraceId = ""
        End If
    End Function

    <System.Web.Services.WebMethod()>
    Public Shared Function GetCurrentLocnForSerialNumAndTraceId(ByVal strSerial As String, ByVal lngTraceCodeIdID As Long) As Long

        Dim objSerial As New VTDBFunctions.VTDBFunctions.SerialNumFunctions
        Dim dsSerial As New DataSet

        If lngTraceCodeIdID <> 0 Then
            dsSerial = objSerial.GetDetailsForSerialNumAndTraceId(strSerial, lngTraceCodeIdID)

            If dsSerial.Tables(0).Rows.Count <> 0 Then
                GetCurrentLocnForSerialNumAndTraceId = dsSerial.Tables(0).Rows(0).Item("LocationId")
            Else
                GetCurrentLocnForSerialNumAndTraceId = 0
            End If
        Else
            GetCurrentLocnForSerialNumAndTraceId = 0
        End If

    End Function

    <System.Web.Services.WebMethod()>
    Public Shared Function GetUnitOfSaleForProdId(ByVal lngproductID As Long) As String

        Dim objProd As New VTDBFunctions.VTDBFunctions.ProductsFunctions
        Dim intSaleType As Integer
        Dim strSaleType As String

        intSaleType = objProd.GetUnitOfSale(lngproductID)

        If intSaleType = 1 Then
            strSaleType = "1"
        Else
            strSaleType = "0"
        End If

        GetUnitOfSaleForProdId = strSaleType

    End Function

    <System.Web.Services.WebMethod()>
    Public Shared Function GetAvgWeightPerUnitForProdId(ByVal lngproductID As Long) As String

        Dim objProd As New VTDBFunctions.VTDBFunctions.ProductsFunctions
        Dim strAvgWeightPerUnit As String


        strAvgWeightPerUnit = objProd.GetProductAvgWeightPerUnit(lngproductID)


        GetAvgWeightPerUnitForProdId = strAvgWeightPerUnit

    End Function


    ''' <summary>
    ''' This method can be called from Javascript and will return a string representation of the data table entry for this product
    ''' This string can be transformed by the JSON.parse function into a useable recordset
    ''' </summary>
    ''' <param name="intProdId"></param>
    ''' <returns></returns>
    <System.Web.Services.WebMethod()>
    Public Shared Function GetProductDataForIdWebMethod(ByVal intProdId As Integer) As String
        Dim objProd As New VTDBFunctions.VTDBFunctions.ProductsFunctions
        Dim dtP As DataTable = objProd.GetProductForId(intProdId).Tables(0)

        Dim serializer As System.Web.Script.Serialization.JavaScriptSerializer = New System.Web.Script.Serialization.JavaScriptSerializer()
        Dim rows As New List(Of Dictionary(Of String, Object))
        Dim row As Dictionary(Of String, Object)

        For Each dr As DataRowView In dtP.DefaultView
            row = New Dictionary(Of String, Object)
            For Each col As DataColumn In dtP.DefaultView.Table.Columns
                row.Add(col.ColumnName, dr.Item(col.Ordinal))
            Next
            rows.Add(row)
        Next


        GetProductDataForIdWebMethod = serializer.Serialize(rows)



    End Function

    <System.Web.Services.WebMethod()>
    Public Shared Sub SetSessionVariableValue(ByVal strSessionVarName As String, ByVal strValue As String)
        Dim objVTDBfunctions As New VTDBFunctions.VTDBFunctions.TelesalesFunctions

        ''objVTDBfunctions.storeValueToCurrentSession(strSessionVarName, strValue)


        Select Case strSessionVarName
            Case "Location"
                HttpContext.Current.Session("VT_SelectedLocationText") = strValue
            Case "TraceCode"
                HttpContext.Current.Session("VT_SelectedTraceCode") = strValue
            Case Else
                HttpContext.Current.Session(strSessionVarName) = strValue
        End Select





    End Sub

    <System.Web.Services.WebMethod()>
    Public Shared Function GetProductDataForCodeWebMethod(ByVal strProdCode As String) As String
        Dim objProd As New VTDBFunctions.VTDBFunctions.ProductsFunctions
        Dim dtP As DataTable = objProd.GetProductForCode(strProdCode).Tables(0)


        Dim serializer As System.Web.Script.Serialization.JavaScriptSerializer = New System.Web.Script.Serialization.JavaScriptSerializer()
        Dim rows As New List(Of Dictionary(Of String, Object))
        Dim row As Dictionary(Of String, Object)

        For Each dr As DataRowView In dtP.DefaultView
            row = New Dictionary(Of String, Object)
            For Each col As DataColumn In dtP.DefaultView.Table.Columns
                row.Add(col.ColumnName, dr.Item(col.Ordinal))
            Next
            rows.Add(row)
        Next


        GetProductDataForCodeWebMethod = serializer.Serialize(rows)



    End Function

    <System.Web.Services.WebMethod()>
    Public Shared Function GetProductDataForCodeWebMethod_Steripack(ByVal strProdCode As String) As String
        Dim objProd As New VTDBFunctions.VTDBFunctions.ProductsFunctions
        Dim objG As New BPADotNetCommonFunctions.VT_ReportFunctions.GeneralFunctions
        Dim strSearch As String
        Dim dtTemp As New DataTable
        Dim dsMatrixItems As New DataSet
        Dim dsTemp As New DataSet
        Dim dtSearch As New DataTable
        Dim intDimNodeId As Integer
        Dim strDataTableName As String = VT_CommonFunctions.MatrixFunctions.MaterialsFormDataTable
        Dim objMatrix As New VT_CommonFunctions.MatrixFunctions
        Dim objVTM As New VTDBFunctions.VTDBFunctions.VTMatrixFunctions

        Dim dtP As DataTable = objProd.GetProductForCode(strProdCode).Tables(0)

        'Add Dimension columns to this grid and fill them
        If dtP IsNot Nothing And dtP.Rows.Count > 0 Then
            'Add the additional columns needed
            dtP.Columns.Add("DimLength")
            dtP.Columns.Add("DimWidth")
            dtP.Columns.Add("CustPartNum")
            dtP.Columns.Add("CustPartNumRev")

            intDimNodeId = dtP.Rows(0).Item("MatrixID")
            'need to get the most recent version of this product, even if it is still in draft
            'first get the parent of this item
            Dim intParent As Integer = objVTM.GetParentIdForFormId(HttpContext.Current.Session("_VT_DotNetConnString"), "mat_formList", intDimNodeId)
            Dim dtChildren As New DataTable
            dtChildren = objVTM.GetVTMatrixChildFolders(HttpContext.Current.Session("_VT_DotNetConnString"), "mat_formList", intParent)
            Dim dvchildren As New DataView
            dvchildren = dtChildren.DefaultView
            dvchildren.Sort = ("FormId desc")
            dtChildren = dvchildren.ToTable
            If dtChildren.Rows.Count > 0 Then
                intDimNodeId = dtChildren.Rows(0).Item("FormId")
            End If
            dtTemp = objProd.GetAllDimensionsForProduct(HttpContext.Current.Session("_VT_DotNetConnString"), intDimNodeId)

            If dtTemp.Rows.Count > 0 Then
                strSearch = "Field1 = 'Length OD'"
                dtSearch = objG.SearchDataTable(strSearch, dtTemp)
                If dtSearch IsNot Nothing AndAlso dtSearch.Rows.Count > 0 Then
                    'Note: The specification value is stored in 'Field3'
                    dtP.Rows(0).Item("DimLength") = dtSearch.Rows(0).Item("Field3")
                End If

                strSearch = "Field1 = 'Width OD'"
                dtSearch = objG.SearchDataTable(strSearch, dtTemp)
                If dtSearch IsNot Nothing AndAlso dtSearch.Rows.Count > 0 Then
                    'Note: The specification value is stored in 'Field3'
                    dtP.Rows(0).Item("DimWidth") = dtSearch.Rows(0).Item("Field3")
                End If

            End If


            'Add the Customer PartNumber to the Output Table
            Dim intDetailsId As Integer = objVTM.GetGenericFolderId(HttpContext.Current.Session("_VT_DotNetConnString"), "mat_FormList", intDimNodeId, "ProductDetails")
            Dim strTemp As String = objVTM.GetVTMatrixFormDataItem(HttpContext.Current.Session("_VT_DotNetConnString"), "mat_FormData", intDetailsId, "txtCustPartNum")
            dtP.Rows(0).Item("CustPartNum") = strTemp

            strTemp = objVTM.GetVTMatrixFormDataItem(HttpContext.Current.Session("_VT_DotNetConnString"), "mat_FormData", intDetailsId, "txtCustPartNumRev")
            dtP.Rows(0).Item("CustPartNumRev") = strTemp

            'Add the Price break info ?


            Dim serializer As System.Web.Script.Serialization.JavaScriptSerializer = New System.Web.Script.Serialization.JavaScriptSerializer()
            Dim rows As New List(Of Dictionary(Of String, Object))
            Dim row As Dictionary(Of String, Object)

            For Each dr As DataRowView In dtP.DefaultView
                row = New Dictionary(Of String, Object)
                For Each col As DataColumn In dtP.DefaultView.Table.Columns
                    row.Add(col.ColumnName, dr.Item(col.Ordinal))
                Next
                rows.Add(row)
            Next


            GetProductDataForCodeWebMethod_Steripack = serializer.Serialize(rows)
        Else
            GetProductDataForCodeWebMethod_Steripack = "[]"
        End If


    End Function

    <System.Web.Services.WebMethod()>
    Public Shared Function GetAltUMWebMethod_Steripack(ByVal strPartNum As String) As String


        Dim objP As New ProductsFunctions.Products

        Dim dtOutput As New DataTable
        dtOutput = objP.GetAlternativeUMDTForPart(strPartNum)
        Dim serializer As System.Web.Script.Serialization.JavaScriptSerializer = New System.Web.Script.Serialization.JavaScriptSerializer()
        Dim rows As New List(Of Dictionary(Of String, Object))
        Dim row As Dictionary(Of String, Object)

        For Each dr As DataRowView In dtOutput.DefaultView
            row = New Dictionary(Of String, Object)
            For Each col As DataColumn In dtOutput.DefaultView.Table.Columns
                row.Add(col.ColumnName, dr.Item(col.Ordinal))
            Next
            rows.Add(row)
        Next


        GetAltUMWebMethod_Steripack = serializer.Serialize(rows)


    End Function

    <System.Web.Services.WebMethod()>
    Public Shared Function GetProductDataForCustCodeWebMethod_Steripack(ByVal strCustCode As String) As String
        Dim objProd As New VTDBFunctions.VTDBFunctions.ProductsFunctions
        Dim objG As New BPADotNetCommonFunctions.VT_ReportFunctions.GeneralFunctions
        Dim strSearch As String
        Dim dtTemp As New DataTable
        Dim dsMatrixItems As New DataSet
        Dim dsTemp As New DataSet
        Dim dtSearch As New DataTable
        Dim intDimNodeId As Integer
        Dim strDataTableName As String = VT_CommonFunctions.MatrixFunctions.MaterialsFormDataTable
        Dim objMatrix As New VT_CommonFunctions.MatrixFunctions
        Dim objVTM As New VTDBFunctions.VTDBFunctions.VTMatrixFunctions


        Dim objVT As New VTDBFunctions.VTDBFunctions.VTMatrixFunctions
        Dim objM As New BPADotNetCommonFunctions.MaterialsModuleFunctions
        Dim intNumFields As Integer = 3


        Dim astrItems(intNumFields - 1) As String
        astrItems(0) = "txtCustPartNum"
        astrItems(1) = "ItemId"
        astrItems(2) = "txtPartNum"

        Dim dsProd As DataSet = objVT.GetVTMatrixData(HttpContext.Current.Session("_VT_DotNetConnString"), "mat_FormData", intNumFields, astrItems, "txtCustPartNum", strCustCode)
        Dim lngproduct As Long
        Dim strprodCode As String

        If dsProd.Tables(0).Rows.Count > 0 Then
            strprodCode = dsProd.Tables(0).Rows(0).Item("Field2")
        End If

        If strprodCode <> "" Then
            Dim dtP As DataTable = objProd.GetProductForCode(strprodCode).Tables(0)

            'Add Dimension columns to this grid and fill them
            If dtP IsNot Nothing And dtP.Rows.Count > 0 Then
                'Add the additional columns needed
                dtP.Columns.Add("DimLength")
                dtP.Columns.Add("DimWidth")
                dtP.Columns.Add("CustPartNum")
                dtP.Columns.Add("CustPartNumRev")


                intDimNodeId = dtP.Rows(0).Item("MatrixID")

                dtTemp = objProd.GetAllDimensionsForProduct(HttpContext.Current.Session("_VT_DotNetConnString"), intDimNodeId)

                If dtTemp.Rows.Count > 0 Then
                    strSearch = "Field1 = 'Length OD'"
                    dtSearch = objG.SearchDataTable(strSearch, dtTemp)
                    If dtSearch IsNot Nothing AndAlso dtSearch.Rows.Count > 0 Then
                        'Note: The specification value is stored in 'Field3'
                        dtP.Rows(0).Item("DimLength") = dtSearch.Rows(0).Item("Field3")
                    End If

                    strSearch = "Field1 = 'Width OD'"
                    dtSearch = objG.SearchDataTable(strSearch, dtTemp)
                    If dtSearch IsNot Nothing AndAlso dtSearch.Rows.Count > 0 Then
                        'Note: The specification value is stored in 'Field3'
                        dtP.Rows(0).Item("DimWidth") = dtSearch.Rows(0).Item("Field3")
                    End If

                End If


                'Add the Customer PartNumber to the Output Table
                Dim intDetailsId As Integer = objVTM.GetGenericFolderId(HttpContext.Current.Session("_VT_DotNetConnString"), "mat_FormList", intDimNodeId, "ProductDetails")
                Dim strTemp As String = objVTM.GetVTMatrixFormDataItem(HttpContext.Current.Session("_VT_DotNetConnString"), "mat_FormData", intDetailsId, "txtCustPartNum")
                dtP.Rows(0).Item("CustPartNum") = strTemp

                strTemp = objVTM.GetVTMatrixFormDataItem(HttpContext.Current.Session("_VT_DotNetConnString"), "mat_FormData", intDetailsId, "txtCustPartNumRev")
                dtP.Rows(0).Item("CustPartNumRev") = strTemp

                'Add the Price break info ?


                Dim serializer As System.Web.Script.Serialization.JavaScriptSerializer = New System.Web.Script.Serialization.JavaScriptSerializer()
                Dim rows As New List(Of Dictionary(Of String, Object))
                Dim row As Dictionary(Of String, Object)

                For Each dr As DataRowView In dtP.DefaultView
                    row = New Dictionary(Of String, Object)
                    For Each col As DataColumn In dtP.DefaultView.Table.Columns
                        row.Add(col.ColumnName, dr.Item(col.Ordinal))
                    Next
                    rows.Add(row)
                Next


                GetProductDataForCustCodeWebMethod_Steripack = serializer.Serialize(rows)
            Else
                GetProductDataForCustCodeWebMethod_Steripack = "[]"
            End If
        Else
            GetProductDataForCustCodeWebMethod_Steripack = "[]"
        End If


    End Function

    ''' <summary>
    ''' this sub can be used by a Javascript function to write a value to a table in the matrix
    ''' </summary>
    ''' <param name="strFormDataTable"></param>
    ''' <param name="intFormId"></param>
    ''' <param name="strDataItemName"></param>
    ''' <param name="strDataItemValue"></param>
    ''' <remarks></remarks>
    <System.Web.Services.WebMethod()>
    Public Shared Sub WriteToMatrix(ByVal strFormDataTable As String, ByVal intFormId As Integer, strDataItemName As String, strDataItemValue As String)
        Dim objVTM As New VTDBFunctions.VTDBFunctions.VTMatrixFunctions
        objVTM.InsertVTMatrixFormDataItem(HttpContext.Current.Session("_VT_DotNetConnString"), strFormDataTable, intFormId, strDataItemName, strDataItemValue)
    End Sub

    ''' this sub can be used by a Javascript function to write a value to session
    ''' </summary>
    ''' <param name="strItemName"></param>
    ''' <param name="strDataItemValue"></param>
    ''' <remarks></remarks>
    <System.Web.Services.WebMethod()>
    Public Shared Sub WriteToSession(ByVal strItemName As String, strDataItemValue As String)
        HttpContext.Current.Session(strItemName) = strDataItemValue
    End Sub

    <System.Web.Services.WebMethod()>
    Public Shared Function GetVerifyNow() As DateTime
        Dim objVTDB As New VTDBFunctions.VTDBFunctions.UtilFunctions

        GetVerifyNow = objVTDB.VerifyNow

    End Function

    <System.Web.Services.WebMethod()>
    Public Shared Function GetProductPriceForCodeQtyUMAndCustWebMethod_Steripack(ByVal strProdCode As String, ByVal dblQtyOrWeight As Double, ByVal strAltUM As String) As String
        'SmcN 13/03/2014  This function get the Appripriate price for a specified product code and quantity
        'The data table serialised in this function also includes
        'ProductCode'
        'ProductId
        'Customer
        'CustomerId
        'dblQtyOrWeight
        'DataBaseUnitPrice
        'DataBaseTotalPrice)
        'QuoteReference
        'Comment
        Dim objP As New ProductsFunctions.Products

        Dim dtOutput As New DataTable

        dtOutput = objP.GetProductPriceForCodeQtyAndCust_Steripack(strProdCode, dblQtyOrWeight, strAltUM)


        Dim serializer As System.Web.Script.Serialization.JavaScriptSerializer = New System.Web.Script.Serialization.JavaScriptSerializer()
        Dim rows As New List(Of Dictionary(Of String, Object))
        Dim row As Dictionary(Of String, Object)

        For Each dr As DataRowView In dtOutput.DefaultView
            row = New Dictionary(Of String, Object)
            For Each col As DataColumn In dtOutput.DefaultView.Table.Columns
                row.Add(col.ColumnName, dr.Item(col.Ordinal))
            Next
            rows.Add(row)
        Next


        GetProductPriceForCodeQtyUMAndCustWebMethod_Steripack = serializer.Serialize(rows)


    End Function

    <System.Web.Services.WebMethod()>
    Public Shared Function GetProductPriceForCodeQtyAndCustWebMethod_Steripack(ByVal strProdCode As String, ByVal dblQtyOrWeight As Double) As String
        'SmcN 13/03/2014  This function get the Appripriate price for a specified product code and quantity
        'The data table serialised in this function also includes
        'ProductCode'
        'ProductId
        'Customer
        'CustomerId
        'dblQtyOrWeight
        'DataBaseUnitPrice
        'DataBaseTotalPrice)
        'QuoteReference
        'Comment
        Dim objP As New ProductsFunctions.Products

        Dim dtOutput As New DataTable

        dtOutput = objP.GetProductPriceForCodeQtyAndCust_Steripack(strProdCode, dblQtyOrWeight)


        Dim serializer As System.Web.Script.Serialization.JavaScriptSerializer = New System.Web.Script.Serialization.JavaScriptSerializer()
        Dim rows As New List(Of Dictionary(Of String, Object))
        Dim row As Dictionary(Of String, Object)

        For Each dr As DataRowView In dtOutput.DefaultView
            row = New Dictionary(Of String, Object)
            For Each col As DataColumn In dtOutput.DefaultView.Table.Columns
                row.Add(col.ColumnName, dr.Item(col.Ordinal))
            Next
            rows.Add(row)
        Next


        GetProductPriceForCodeQtyAndCustWebMethod_Steripack = serializer.Serialize(rows)


    End Function


    <System.Web.Services.WebMethod()>
    Public Shared Function GetUnitOfSaleTextForProdId(ByVal lngproductID As Long) As String

        Dim objProd As New VTDBFunctions.VTDBFunctions.ProductsFunctions
        Dim intSaleType As Integer
        Dim strSaleType As String

        intSaleType = objProd.GetUnitOfSale(lngproductID)

        If intSaleType = 1 Then
            strSaleType = "QUANTITY"
        Else
            strSaleType = "WEIGHT"
        End If

        GetUnitOfSaleTextForProdId = strSaleType

    End Function

    ''' <summary>
    ''' Function that can be called from Javascript to determine if a given TraceCode is valid for a given Product
    ''' </summary>
    ''' <param name="strTraceCodeDesc"></param>
    ''' <param name="lngproductID"></param>
    ''' <returns>An empty string if the Trace Code is valid</returns>
    ''' <remarks></remarks>
    <System.Web.Services.WebMethod()>
    Public Shared Function GetScanIsValid(ByVal strTraceCodeDesc As String, ByVal lngproductID As Long, ByVal strCommCode As String) As String

        Dim objTrace As New VTDBFunctions.VTDBFunctions.TraceFunctions
        Dim lngTraceId As Long

        lngTraceId = objTrace.GetTraceCodeIDForDescAndProduct(lngproductID, strTraceCodeDesc)

        GetScanIsValid = ""

        Dim objProd As New VTDBFunctions.VTDBFunctions.ProductsFunctions
        Dim dtProd As DataTable = objProd.GetProductForCommodityCode(strCommCode)
        If dtProd.Rows.Count = 0 Then
            GetScanIsValid = "Scanned Barcode does not contain a valid product code."
        ElseIf lngproductID <> dtProd.Rows(0).Item("ProductId") Then
            GetScanIsValid = "Scanned Product Does not match selected Product."
            'ElseIf lngTraceId = 0 Then
            '    GetScanIsValid = "Entered Trace Code Does not Exist for selected Product. "
        End If


    End Function

End Class

Public Class MyMasterPage
    Inherits System.Web.UI.MasterPage

    Public Sub ShowMessage(ByVal strMsg As String)
        ' Get a reference to the controls on the current page
        ' Because we are in a nested Master we need to go to the Outer Master first
        ' Outer Master Page

        'Dim Outer_CP As ContentPlaceHolder
        'Outer_CP = TryCast(Me.Master.FindControl("ProjectMAIN_content"), ContentPlaceHolder)

        'Dim TheLabel As Label = TryCast(Outer_CP.FindControl("lblMsg"), Label)

        Dim TheLabel As Label = TryCast(Me.Master.FindControl("lblMsgMandatory"), Label)
        TheLabel.Text = strMsg
        Dim TheExtender As AjaxControlToolkit.ModalPopupExtender = TryCast(Me.Master.FindControl("ModalPopupExtenderMsg"), AjaxControlToolkit.ModalPopupExtender)
        TheExtender.Show()

    End Sub
    Function GetInnerContentPlaceHolder() As ContentPlaceHolder
        'Because we are in a nested Master we need to go to the Outer Master first
        'Outer Master Page
        Dim Outer_CP As ContentPlaceHolder
        Outer_CP = TryCast(Me.Master.FindControl("ProjectMAIN_content"), ContentPlaceHolder)

        ' Inner Master Page
        Dim Inner_CP As ContentPlaceHolder
        Inner_CP = TryCast(Outer_CP.FindControl("DetailsMaster_ContentPlaceHolder"), ContentPlaceHolder)

        GetInnerContentPlaceHolder = Inner_CP
    End Function
    Public Property CurrentSession() As MySessionInfo
        Get
            If Session("_VT_MySessionInfo") Is Nothing Then
                Dim thisSession As New MySessionInfo
                Session("_VT_MySessionInfo") = thisSession
            End If

            Return Session("_VT_MySessionInfo")
        End Get

        Set(ByVal value As MySessionInfo)
            Session("_VT_MySessionInfo") = value
        End Set
    End Property


End Class
Public Class MyWebServicePage
    Inherits System.Web.Services.WebService


    Public Property CurrentSession() As MySessionInfo
        Get
            If Session("_VT_MySessionInfo") Is Nothing Then
                Dim thisSession As New MySessionInfo
                Session("_VT_MySessionInfo") = thisSession
            End If

            Return Session("_VT_MySessionInfo")
        End Get

        Set(ByVal value As MySessionInfo)
            Session("_VT_MySessionInfo") = value
        End Set
    End Property

End Class
Public Class CommonFunctions
    Public Const g_strWebsiteVersionNumber As String = "V5.04"


    '' Scheme Status definitions
    Public Const g_cSchemeNew = "New"
    Public Const g_cSchemeAwaitingActivation = "Awaiting Activation"
    Public Const g_cSchemeActive = "Awaiting Selections"
    Public Const g_cSchemeClosed = "Closed"

    ' document types
    Public Enum Sales_DocTypes
        Invoice = 0
        CreditNote = 1
        PurchaseOrder = 2
        PoD = 3
    End Enum


    Public Enum Purchases_DocTypes
        Invoice = 0
        CreditNote = 1
        PurchaseOrder = 2
        PoD = 3
    End Enum


    Function AddOrderToCIMCSV(strPathToCSV As String) As String
        Dim xlApp As Excel.Application
        Dim xlWorkBook As Excel.Workbook
        Dim xlWorkSheet As Excel.Worksheet
        Dim misValue As Object = System.Reflection.Missing.Value

        xlApp = New Excel.ApplicationClass
        If System.IO.File.Exists(strPathToCSV) Then
            xlWorkBook = xlApp.Workbooks.Open(strPathToCSV)
        Else
            xlWorkBook = xlApp.Workbooks.Add(misValue)
        End If

        xlWorkSheet = xlWorkBook.Sheets("Sheet1")

        ' find the next row in the sheet
        Dim theNextRow As Integer = xlWorkSheet.Range("A" & xlWorkSheet.Rows.Count).End(Excel.XlDirection.xlUp).Row


        ' write the order data
        xlWorkSheet.Cells(theNextRow, 1) = "12123"
        xlWorkSheet.SaveAs(strPathToCSV)

        xlWorkBook.Close()
        xlApp.Quit()

        releaseObject(xlApp)
        releaseObject(xlWorkBook)
        releaseObject(xlWorkSheet)


    End Function


    Private Sub releaseObject(ByVal obj As Object)
        Try
            System.Runtime.InteropServices.Marshal.ReleaseComObject(obj)
            obj = Nothing
        Catch ex As Exception
            obj = Nothing
        Finally
            GC.Collect()
        End Try
    End Sub

    Public Function CleanVTStoreFolderName(ByVal strItemName As String) As String
        strItemName = Replace(strItemName, ".", "")
        strItemName = Replace(strItemName, "/", "_")
        strItemName = Replace(strItemName, "\", "_")
        strItemName = Replace(strItemName, "'", "")
        strItemName = Replace(strItemName, "*", "")
        strItemName = Replace(strItemName, "<", "")
        strItemName = Replace(strItemName, ">", "")
        strItemName = Replace(strItemName, "?", "")
        strItemName = Replace(strItemName, "|", "")
        strItemName = Replace(strItemName, Chr(34), "")   ' strip " character
        strItemName = Replace(strItemName, ",", " ")

        CleanVTStoreFolderName = strItemName
    End Function
    Public Function GetWoolworthsSellBy(lngProductId As Long) As String
        Dim objProd As New ProductsFunctions.Products
        Dim dsprod As New DataSet
        Dim strNumDays As String
        Dim strWindowDays As String
        Dim objprodschema As New VTDBFunctions.VTDBFunctions.ProductsFunctions
        Dim dteRefDate As Date
        Dim dteDateToday As Date
        Dim strDayOfWeek As String


        'get the use by interval for the product from the matrix
        dsprod = objprodschema.GetProductForId(lngProductId)
        Dim intProductMatrixId As Integer = dsprod.Tables(0).Rows(0).Item("MatrixId")
        strNumDays = objProd.GetSpecItemForProduct(intProductMatrixId, "txtNumDays")
        dteDateToday = PortalFunctions.Now.Date
        strDayOfWeek = dteDateToday.ToString("ddd")
        Select Case UCase(strDayOfWeek)
            Case "MON"
                dteRefDate = dteDateToday.AddDays(-4)
            Case "TUE"
                dteRefDate = dteDateToday.AddDays(-5)
            Case "WED"
                dteRefDate = dteDateToday.AddDays(-6)
            Case "THU"
                dteRefDate = dteDateToday
            Case "FRI"
                dteRefDate = dteDateToday.AddDays(-1)
            Case "SAT"
                dteRefDate = dteDateToday.AddDays(-2)
            Case "SUN"
                dteRefDate = dteDateToday.AddDays(-3)
        End Select
        If Trim(strNumDays) = "" Then
            strNumDays = "0"
        End If
        If IsNumeric(strNumDays) = False Then
            strNumDays = "0"
        End If
        GetWoolworthsSellBy = dteRefDate.AddDays(CInt(strNumDays))

    End Function
    Public Function GetWoolworthsUseBy(lngproductId As Long) As String

        Dim objProd As New ProductsFunctions.Products
        Dim dsprod As New DataSet
        Dim strNumDays As String
        Dim objprodschema As New VTDBFunctions.VTDBFunctions.ProductsFunctions
        Dim dteRefDate As Date
        Dim dteDateToday As Date
        Dim strDayOfWeek As String
        Dim strWindowDays As String


        'get the use by interval for the product from the matrix
        dsprod = objprodschema.GetProductForId(lngproductId)
        Dim intProductMatrixId As Integer = dsprod.Tables(0).Rows(0).Item("MatrixId")
        strNumDays = objProd.GetSpecItemForProduct(intProductMatrixId, "txtNumDays")
        strWindowDays = objProd.GetSpecItemForProduct(intProductMatrixId, "txtWindowDays")

        dteDateToday = PortalFunctions.Now.Date
        strDayOfWeek = dteDateToday.ToString("ddd")
        Select Case UCase(strDayOfWeek)
            Case "MON"
                dteRefDate = dteDateToday.AddDays(-4)
            Case "TUE"
                dteRefDate = dteDateToday.AddDays(-5)
            Case "WED"
                dteRefDate = dteDateToday.AddDays(-6)
            Case "THU"
                dteRefDate = dteDateToday
            Case "FRI"
                dteRefDate = dteDateToday.AddDays(-1)
            Case "SAT"
                dteRefDate = dteDateToday.AddDays(-2)
            Case "SUN"
                dteRefDate = dteDateToday.AddDays(-3)
        End Select

        If Trim(strNumDays) = "" Then
            strNumDays = "0"
        End If
        If IsNumeric(strNumDays) = False Then
            strNumDays = "0"
        End If

        If Trim(strWindowDays) = "" Then
            strWindowDays = "0"
        End If
        If IsNumeric(strWindowDays) = False Then
            strWindowDays = "0"
        End If

        Dim intDays As Integer
        intDays = CInt(strNumDays) + CInt(strWindowDays)


        GetWoolworthsUseBy = dteRefDate.AddDays(intDays)

    End Function
End Class

Public Class CustomerFunctions

    Public Function GetCustomerNameforID(ByVal intCustomerID As Integer, ByVal strConnectionString As String) As String


        Dim myConnection As New SqlConnection(strConnectionString)
        Dim strSQL As String

        Dim strName As String


        strSQL = "Select CustomerName from cus_Customers where CustomerId = " & intCustomerID

        myConnection.Open()

        Dim myCmd As SqlCommand = New SqlCommand(strSQL, myConnection)

        strName = myCmd.ExecuteScalar

        myConnection.Close()

        GetCustomerNameforID = strName

    End Function



End Class



Public Class JobFunctions

    Public Function GetC1sForJobID(ByVal lngJobID As Long, ByVal strConnectionString As String) As String

        Dim strC1s As String
        Dim i As Integer

        Dim myConnection As New SqlConnection(strConnectionString)
        Dim comSQL As New SqlCommand("job_resGetC1sForJob", myConnection)
        Dim adpSQL As New SqlDataAdapter(comSQL)
        Dim ds As New DataSet

        Dim objParam As SqlParameter

        comSQL.CommandType = CommandType.StoredProcedure
        objParam = comSQL.Parameters.Add("@JobID", SqlDbType.Int)
        objParam.Direction = ParameterDirection.Input
        objParam.Value = lngJobID

        myConnection.Open()
        adpSQL.Fill(ds)




        ' lblJobDesc.Text = Trim(ds.Tables(0).Rows(0)("JobIDDEsc"))
        strC1s = ""
        For i = 0 To ds.Tables(0).Rows.Count - 1
            strC1s = strC1s & Trim(ds.Tables(0).Rows(0)("C1Number")) & ","
        Next i

        ' Close the connection when done with it.
        myConnection.Close()

        GetC1sForJobID = strC1s

    End Function


    Public Function GetDispatchDetails(ByVal lngDispatchID As Long, ByVal strConnectionString As String) As DataSet



        Dim myConnection As New SqlConnection(strConnectionString)
        Dim ds As New DataSet
        Dim strSQL As String

        myConnection.Open()

        strSQL = "select job_dispatchdetails.*,job_dispatchstatus.statustext from job_dispatchdetails, job_dispatchstatus where job_dispatchdetails.dispatchstatus =  job_dispatchstatus.statusid and job_dispatchdetails.dispatchid=" & lngDispatchID
        Dim adpSQL As New SqlDataAdapter(strSQL, myConnection)

        adpSQL.Fill(ds)





        ' Close the connection when done with it.
        myConnection.Close()

        GetDispatchDetails = ds

    End Function


    Public Function GetDispatchLocationForId(ByVal lngDispatchID As Long, ByVal strConnectionString As String) As String



        Dim myConnection As New SqlConnection(strConnectionString)
        Dim ds As New DataSet
        Dim strSQL As String
        Dim strName As String
        myConnection.Open()

        strSQL = "select ,job_dispatchstatus.statustext from job_dispatchdetails, job_dispatchstatus where job_dispatchdetails.dispatchstatus =  job_dispatchstatus.statusid and job_dispatchdetails.dispatchid=" & lngDispatchID


        Dim myCmd As SqlCommand = New SqlCommand(strSQL, myConnection)

        strName = myCmd.ExecuteScalar



        ' Close the connection when done with it.
        myConnection.Close()

        GetDispatchLocationForId = strName

    End Function


    Public Function GetInvoiceNumForJob(ByVal lngJobID As Long, ByVal strConnectionString As String) As String



        Dim myConnection As New SqlConnection(strConnectionString)
        Dim strSQL As String
        Dim strInvoiceNum As String
        myConnection.Open()

        strSQL = "select job_invoicedetails.Invoicenum from job_invoicedetails where job_invoicedetails.istotalscolumn = 1 and  job_invoicedetails.jobid=" & lngJobID


        Dim myCmd As SqlCommand = New SqlCommand(strSQL, myConnection)

        If IsDBNull(myCmd.ExecuteScalar) = False Then
            strInvoiceNum = myCmd.ExecuteScalar
        Else
            strInvoiceNum = ""
        End If
        ' Close the connection when done with it.
        myConnection.Close()

        GetInvoiceNumForJob = strInvoiceNum

    End Function



    Public Function GetProductForID(ByVal lngProductID As Long, ByVal strConnectionString As String) As String

        Dim myConnection As New SqlConnection(strConnectionString)
        Dim strSQL As String
        Dim strProdName As String
        myConnection.Open()

        strSQL = "select Product_Name from prd_ProductTable where ProductID=" & lngProductID


        Dim myCmd As SqlCommand = New SqlCommand(strSQL, myConnection)

        If IsDBNull(myCmd.ExecuteScalar) = False Then
            strProdName = myCmd.ExecuteScalar
        Else
            strProdName = ""
        End If
        ' Close the connection when done with it.
        myConnection.Close()

        GetProductForID = Trim(strProdName)

    End Function


    Public Function CheckForOutOfStockIngredients(ByVal lngJobid As Long) As Boolean

        Dim ds As New DataSet
        Dim dr As DataRow
        Dim objproduction As New ProductionFunctions.Production
        Dim blnOutOfStock As Boolean
        Dim objProdfuncs As New ProductsFunctions.Products
        Dim dblInStock As Double



        ds = objproduction.GetIngredientsForJob(lngJobid)

        blnOutOfStock = False

        For Each dr In ds.Tables(0).Rows
            dblInStock = objProdfuncs.GetProductInStockQty(CInt(dr.Item("IngredientProductID")))

            If CInt(dr.Item("Quantity")) <> 0 Then
                If dblInStock - CDbl(dr.Item("Quantity")) < 0 Then
                    blnOutOfStock = True
                End If
            End If

            If CDbl(dr.Item("Weight")) <> 0 Then
                If dblInStock - CDbl(dr.Item("Weight")) < 0 Then
                    blnOutOfStock = True
                End If
            End If

        Next

        CheckForOutOfStockIngredients = blnOutOfStock

    End Function

End Class

Public Class GenericFunctions
    Public Const g_strWebsiteVersionNumber As String = "V5.16"
    ' constants for the StatusTags
    'Public Const StatusTag_Draft As String = "VersionStatus_Draft"
    'Public Const StatusTag_Current As String = "VersionStatus_Current"
    'Public Const StatusTag_Approved As String = "VersionStatus_Approved"
    'Public Const StatusTag_Retired As String = "VersionStatus_Retired"

    Public Enum StatusTags
        VersionStatus_Draft
        VersionStatus_Pending
        VersionStatus_Current
        VersionStatus_Approved
        VersionStatus_Retired
        VersionStatus_OnHold
    End Enum

    Public Enum NCIStatusTags
        NCIStatus_New
        NCIStatus_Active
        NCIStatus_Approved
        NCIStatus_Closed
    End Enum

    ' constants for the Module Types
    Public Const g_strMTypePersonnel As String = "Personnel"
    Public Const g_strMTypeCustomers As String = "Customers"
    Public Const g_strMTypeCourses As String = "Courses"
    Public Const g_strMTypeCompetencies As String = "Competencies"
    Public Const g_strMTypeRoles As String = "Roles"
    Public Const g_strMTypeModules As String = "Modules"
    Public Const g_strMTypeAssets As String = "Assets"
    Public Const g_strMTypeProducts As String = "Materials"
    Public Const g_strMTypePrivileges As String = "Privileges"
    Public Const g_strMTypeJobSteps As String = "JobSteps"
    Public Const g_strMTypeJobSequences As String = "JobSequences"
    Public Const g_strMTypePPSSheets As String = "PPSSheets"
    Public Const g_strMTypeNonConformances As String = "NonConformance"
    Public Const g_strMTypeSuppliers As String = "Suppliers"
    Public Const g_strMTypeHACCPForms As String = "HACCP Forms"

    Structure ApprovalItem
        Dim Type As String
        Dim Name As String
        Dim Id As Integer
        Dim IsDefault As String
    End Structure

    Structure ApprovalDetails
        Dim AllItems() As ApprovalItem
        Dim OneOfItems() As ApprovalItem
        Dim UseAndOrOr As Integer
        Dim PutOnHold As Integer
    End Structure



    Public Function CleanVTStoreFolderName(ByVal strItemName As String) As String
        strItemName = Replace(strItemName, ".", "")
        strItemName = Replace(strItemName, "/", "_")
        strItemName = Replace(strItemName, "\", "_")
        strItemName = Replace(strItemName, "'", "")
        strItemName = Replace(strItemName, "*", "")
        strItemName = Replace(strItemName, "<", "")
        strItemName = Replace(strItemName, ">", "")
        strItemName = Replace(strItemName, "?", "")
        strItemName = Replace(strItemName, "|", "")
        strItemName = Replace(strItemName, Chr(34), "")   ' strip " character
        strItemName = Replace(strItemName, ",", " ")
        strItemName = Replace(strItemName, ":", "")
        CleanVTStoreFolderName = strItemName
    End Function


    Function ReadCultureFromConfig() As String
        Dim objCfg As New ConfigModuleFunctions
        Dim intRoot As Integer = objCfg.GetConfigModuleRootId(HttpContext.Current.Session("_VT_DotNetConnString"))
        Dim intCatId As Integer = objCfg.GetConfigCategoryFolderId(HttpContext.Current.Session("_VT_DotNetConnString"), intRoot)
        Dim intModuleCatId As Integer = objCfg.GetConfigCategoryId(HttpContext.Current.Session("_VT_DotNetConnString"), "ModuleSetups")
        Dim astrItems(0) As String

        astrItems(0) = "CultureToUse"

        ' need to use an absolute path to store the file
        Dim dt As Data.DataTable = objCfg.GetConfigDataItems(HttpContext.Current.Session("_VT_DotNetConnString"), intModuleCatId, astrItems)
        If dt.Rows.Count > 0 AndAlso (Not IsDBNull(dt.Rows(0).Item("Field0"))) Then
            ReadCultureFromConfig = dt.Rows(0).Item("Field0")
        Else
            ReadCultureFromConfig = ""
        End If

    End Function
End Class
Public Class DisplayFunctions
    Inherits MyBasePage
    Sub FillGrid(wdgJobs As Infragistics.Web.UI.GridControls.WebDataGrid, strConn As String, inttlsNumFields As Integer, dteStart As Date, dteEnd As Date, Optional VT_GridFilterText As String = "", Optional VT_MatrixFilterText As String = "", Optional VT_OrderTypesEnabled As String = "")

        Dim strFilter As String
        Dim dteStartDate As Date
        Dim dteEndDate As Date
        Dim objM As New VTDBFunctions.VTDBFunctions.VTMatrixFunctions
        Dim strErr As String
        'Dim strConn As String = Session("_VT_DotNetConnString")
        Dim dtTemp As DataTable
        Dim objDataPreserve As New BPADotNetCommonFunctions.VT_DataPreserve.DataPreserve
        Dim objG As New BPADotNetCommonFunctions.VT_ReportFunctions.GeneralFunctions
        Dim objSales As New BPADotNetCommonFunctions.SalesFunctions.Sales_Orders


        strFilter = ""

        'Me.CurrentSession.VT_JobID = 0
        If VT_GridFilterText <> "" Then
            strFilter = VT_GridFilterText
        End If

        If dteStart = Nothing Then
            dteStartDate = #12:00:00 AM#
        Else
            dteStartDate = dteStart
        End If

        If dteEnd = Nothing Then
            dteEndDate = #12:00:00 AM#
        Else
            dteEndDate = dteEnd
        End If


        If VT_GridFilterText = "" And dteStart = Nothing And dteEnd = Nothing Then
            Exit Sub
        End If


        Dim dt As New DataTable
        Dim objdbFunctions As New SalesOrdersFunctions.SalesOrders

        dt = objSales.GetSalesOrdersForDates(dteStartDate, dteEndDate, strFilter, False, strConn)

        'if there are old orders in the db the following code will result in errors because there is no matrix id for those orders

        Dim blnContainsOldOrders As Boolean

        Dim arows() As DataRow
        arows = dt.Select("MatrixLinkId is null")
        If arows.GetUpperBound(0) > 0 Then
            blnContainsOldOrders = True
        End If

        If blnContainsOldOrders = False Then
            'Add in the extra data items to this datatable These items are read from the matrix     SmcN 15/01/2014
            Dim astrItems(5) As String
            astrItems(0) = "ReasonOnHold"
            astrItems(1) = "DeliverTo_DateOut"
            astrItems(2) = "StatusBefore_OnHold"
            astrItems(3) = "SO_ContiguousNum"
            astrItems(4) = "CustomerOrderDate"
            astrItems(5) = "LatePlanningComment"


            Dim strFormName As String = "Details"
            Dim strKeyField As String = "MatrixLinkId"

            strErr = objM.GetBlockOfMatrixDataItems(strConn, strFormName, VTMatrixFunctions.Sales, astrItems, dt, strKeyField)
            ReDim astrItems(0)
            astrItems(0) = "SoldTo_Code"

            strFormName = "CustomerItems"
            strErr = objM.GetBlockOfMatrixDataItems(strConn, strFormName, VTMatrixFunctions.Sales, astrItems, dt, strKeyField)


            'dt.Columns.Add("SoldTo_CustId")
            'dt.Columns.Add("SoldTo_CustName")

            'Add the extra data to the datatable
            If dt.Columns.Contains("UploadComplete") = False Then
                dt.Columns.Add("UploadComplete")
            End If
            If dt.Columns.Contains("Type") = False Then
                dt.Columns.Add("Type")
            End If

            If dt.Columns.Contains("Attachments") = False Then
                dt.Columns.Add("Attachments")
            End If


            ''SmcN 25/05/2014 Further filter the table here on any Matrix Item columns if a filter was defined relating to these
            If VT_MatrixFilterText <> "" Then
                dt = objG.SearchDataTable(VT_MatrixFilterText, dt)
            End If
        Else
            'Add the extra data to the datatable
            If dt.Columns.Contains("UploadComplete") = False Then
                dt.Columns.Add("UploadComplete")
            End If
            If dt.Columns.Contains("Type") = False Then
                dt.Columns.Add("Type")
            End If
            'dt.Columns.Add("SoldTo_CustId")
            'dt.Columns.Add("SoldTo_CustName")

            'dt.Columns.Add("UploadComplete")
        End If

        If dt.Columns.Contains("Priority") = False Then

            dt.Columns.Add("Priority")
        End If

        If dt.Columns.Contains("LatePlanningComment") = False Then
            dt.Columns.Add("LatePlanningComment")
        End If
        'Find the 'Posted' status of each sales order in the table and set it accordingly
        Dim dsDeliveries As DataSet
        Dim objTele As New VTDBFunctions.VTDBFunctions.TelesalesFunctions
        Dim strUploaded As String
        Dim strDocketNum As String
        Dim i, j As Integer
        Dim dsTrans As DataSet
        '  Dim objSales As New BPADotNetCommonFunctions.SalesFunctions.Sales_Orders
        Dim objCust As New VTDBFunctions.VTDBFunctions.CustomerFunctions
        Dim strInvNum As String

        If dt.Columns.Contains("InvoiceNum") = False Then
            dt.Columns.Add("InvoiceNum")
        End If

        For intCnt As Integer = 0 To dt.Rows.Count - 1

            Dim dtCust As New DataTable
            Dim intCurrentSalesOrderNum As Integer = dt.Rows(intCnt).Item("JobID")

            If Trim(dt.Rows(intCnt).Item("ExtraData1")) = "" Then
                'Read the customer items for this Sales Order
                dtCust = objSales.GetSalesOrderItemsFromMatrix("CUSTOMERITEMS", CInt(intCurrentSalesOrderNum), strConn, inttlsNumFields)
                If dtCust.Rows.Count > 0 Then
                    'dt.Rows(0).Item("CustomerID") = objCust.GetCustomerIdForRef(Left(dtCust.Rows(0).Item("SoldTo_Name"), InStr(dtCust.Rows(0).Item("SoldTo_Name"), ":") - 1))
                    dt.Rows(intCnt).Item("ExtraData1") = Mid(dtCust.Rows(0).Item("SoldTo_Name"), InStr(dtCust.Rows(0).Item("SoldTo_Name"), ":") + 1)

                End If
            End If

            ' get the set of Dockets for this order
            dsDeliveries = objTele.GetDeliveriesForOrderNum(CLng(dt.Rows(intCnt).Item("JobId")))
            If dsDeliveries.Tables(0).Rows.Count > 0 Then
                strUploaded = "Yes"

                i = 0
                ' use While statement so that if any transaction is not SentToSage we can exit the loop
                While (i < dsDeliveries.Tables(0).Rows.Count) And (strUploaded = "Yes")
                    strDocketNum = Trim(dsDeliveries.Tables(0).Rows(i).Item("DocketNum"))
                    ' get all the transactions for this docket
                    dsTrans = objTele.GetTransactionsForDocketNum(strDocketNum)
                    If dsTrans.Tables(0).Rows.Count > 0 Then
                        strInvNum = strInvNum & IIf(IsDBNull(dsTrans.Tables(0).Rows(0).Item("InvoiceNum")), "", dsTrans.Tables(0).Rows(0).Item("InvoiceNum")) & ","
                    End If

                    j = 0
                    While (j < dsTrans.Tables(0).Rows.Count) And (strUploaded = "Yes")
                        If dsTrans.Tables(0).Rows(j).Item("SentToSage") = 0 Then
                            strUploaded = "No"
                        End If
                        j += 1
                    End While
                    i += 1
                End While
            Else
                strUploaded = "No"
            End If

            dt.Rows(intCnt).Item("UploadComplete") = strUploaded
            If Len(strInvNum) > 0 Then
                'trim off the last comma
                strInvNum = Left(strInvNum, Len(strInvNum) - 1)
            End If
            dt.Rows(intCnt).Item("InvoiceNum") = strInvNum
            strInvNum = ""

            If VT_OrderTypesEnabled = "YES" Then
                dt.Rows(intCnt).Item("Type") = If(IsDBNull(dt.Rows(intCnt).Item("Type")), "Standard", dt.Rows(intCnt).Item("Type"))
            End If
            dt.Rows(intCnt).Item("JobStatusText") = Trim(dt.Rows(intCnt).Item("JobStatusText"))

        Next 'intRows


        If blnContainsOldOrders = False Then
            'SmcN 29/04/2014  Format the Date columns so that they display correclty in the grid (need for Sortable date formats)
            'BatchDateStarted
            'CustomerOrderDate
            'RequestedDeliveryDate
            'DeliverTo_DateOut
            'Convert the Date colums to a date field and proper format
            Dim astrColNames(3, 1) As String
            astrColNames(0, 0) = "BatchDateStarted"
            astrColNames(0, 1) = "CONVERT_TO_DATE"
            astrColNames(1, 0) = "CustomerOrderDate"
            astrColNames(1, 1) = "CONVERT_TO_DATE"
            astrColNames(2, 0) = "RequestedDeliveryDate"
            astrColNames(2, 1) = "CONVERT_TO_DATE"
            astrColNames(3, 0) = "DeliverTo_DateOut"
            astrColNames(3, 1) = "CONVERT_TO_DATE"

            dt = objG.CleanColumnFormats(dt, astrColNames)
        End If

        objDataPreserve.BindDataToWDG(dt, wdgJobs)


        Dim objcommon As New VT_CommonFunctions.CommonFunctions

        If UCase(objcommon.GetConfigItem("Accounts")) = "MFGPRO" Then
            wdgJobs.Columns("InvoiceNum").Hidden = True
            wdgJobs.Columns("LatePlanningComment").Hidden = False
        Else
            wdgJobs.Columns("InvoiceNum").Hidden = False
            wdgJobs.Columns("LatePlanningComment").Hidden = True
        End If

        'AM 2015-11-04 Need to move this to the form

        '' ''Me.CurrentSession.VT_OrdersDataTable = dt
        ' '' ''clear any previously selected rows, the data in the grid may be different now but the old row will remain selected
        '' ''wdgJobs.Behaviors.Selection.SelectedRows.Clear()
        '' ''wdgJobs.Behaviors.Activation.ActiveCell = Nothing
        '' ''Session("SelectedRowIndex") = Nothing
        '' ''Me.CurrentSession.VT_SelectedSalesOrderRow_V2 = Nothing

        If UCase(objcommon.GetConfigItem("SkipPlanning")) = "YES" Then
            wdgJobs.Columns("DeliverTo_DateOut").Hidden = True
        Else
            wdgJobs.Columns("DeliverTo_DateOut").Hidden = False
        End If
    End Sub

    Sub SetupStatusDropdown(ddlStatus As DropDownList, strConn As String)

        Dim dtStatuses As Data.DataTable
        Dim objOrders As New BPADotNetCommonFunctions.SalesFunctions.Sales_Orders
        'Dim strConn As String = Session("_VT_DotNetConnString")

        dtStatuses = objOrders.GetOrderStatuses(strConn)

        If dtStatuses IsNot Nothing AndAlso dtStatuses.Rows.Count > 0 Then
            dtStatuses.DefaultView.Sort = "DataItemName"
            For Each drRow As DataRow In dtStatuses.Rows
                ddlStatus.Items.Add(drRow.Item("DataItemValue"))

            Next
        Else

            ddlStatus.Items.Add("")
            ddlStatus.Items.Add(GetGlobalResourceObject("Resource", "Order_Open"))
            ddlStatus.Items.Add(GetGlobalResourceObject("Resource", "Order_PreIssued"))
            ddlStatus.Items.Add(GetGlobalResourceObject("Resource", "Order_New"))
            ddlStatus.Items.Add(GetGlobalResourceObject("Resource", "Order_OpenPartShipped"))
            ddlStatus.Items.Add(GetGlobalResourceObject("Resource", "Order_Closed"))
            ddlStatus.Items.Add(GetGlobalResourceObject("Resource", "Order_ClosedPartShipped"))
            ddlStatus.Items.Add(GetGlobalResourceObject("Resource", "Order_ClosedComplete"))


        End If




    End Sub



End Class