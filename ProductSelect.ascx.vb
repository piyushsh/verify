Imports VTDBFunctions.VTDBFunctions.ProductsFunctions
Imports BPADotNetCommonFunctions
Imports System.Data

Public Class ProductSelect
    Inherits System.Web.UI.UserControl

    'Private glngCustomerId As Long
    'Private gblnShowProductCodewithName As Boolean
    'Private gstrCustomerName As String
    'Private gblnVatExempt As Boolean
    'Private gblnBatchMode As Boolean
    'Private gdblCustDiscount As Double
    'Private glngProductId As Long
    'Private gstrDBConn As String



    Structure TraceCode
        Public TracecodeId As Long
        Public TraceCodeDesc As String
        Public lngProductId As Long
    End Structure

    Public Const cSEPARATOR = "  :  "


    Structure Location
        Public LocationId As Long
        Public LocationText As String
    End Structure

    Public udtLocations() As Location

    Structure TProductRecord
        Public lngProductId As Long
        Public dblPrice As Double
    End Structure

    Structure TCustomerRecord
        Public lngCustomerId As Long
        Public dblDiscountPercent As Double
    End Structure

    Public udtCustomer As TCustomerRecord
   
    Public udtProduct As TProductRecord

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()

        Dim dsLocationData As New DataSet
        Dim objTrace As New VTDBFunctions.VTDBFunctions.TraceFunctions
        Dim i As Integer

        If IsPostBack = False Then
            Session("_VT_BatchMode") = True
            chkShowBatches.Visible = False

            ReDim udtLocations(0)

            dsLocationData = objTrace.GetAllLocations

            'set up the locations
            For i = 0 To dsLocationData.Tables(0).Rows.Count - 1
                ReDim Preserve udtLocations(UBound(udtLocations) + 1)
                udtLocations(UBound(udtLocations)).LocationId = dsLocationData.Tables(0).Rows(i).Item("LocationId")
                udtLocations(UBound(udtLocations)).LocationText = dsLocationData.Tables(0).Rows(i).Item("LocationText")

            Next
        End If

        ' assign the TypeAhead function to react when a key is pressed
        cboProduct.Attributes.Add("onKeyDown", "typeAhead();")
        ' assign the function to cause a PostBack when the Product Selector loses focus
        cboProduct.Attributes.Add("onblur", "lostFocus();")
        'cboProduct.Attributes.Add("onClick", "clearHiddenField();")
    End Sub

#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        If IsPostBack Then
            '    ' The Hidden textbox will be loaded with the item selected in the Product 
            '    ' selector on the client. So we load this value and click the selector
            '    Dim s As String = hdnValue.Value
            '    If s <> "" And s <> "hidden" And s <> "-1" Then
            '        hdnValue.Value = ""
            '        cboProduct.SelectedIndex = CInt(s)
            '        cboProduct_SelectedIndexChanged(sender, e)
            '    End If
            'cboProduct_SelectedIndexChanged(sender, e)



        End If
    End Sub
    '------------------------------------------------------------
    'If customer special pricing should be applied for the product then customer id, name and vatExempt should be passed in
    '------------------------------------------------------------

    Public Property ApplyPricingForCustomer()
        Set(ByVal lngCustomerId)
            Session("_VT_CustomerID") = lngCustomerId

        End Set
        Get

        End Get
    End Property
    

    Public WriteOnly Property CalcQtyFromWeight() As Boolean
        Set(ByVal blnCalcQtyFromWgt As Boolean)
            Session("_VT_CalcQtyFromWgt") = blnCalcQtyFromWgt
        End Set

    End Property

    '------------------------------------------------------------
    ' Pass in a DBConn string for use by any Installation Specific DLL that may be encountered
    '------------------------------------------------------------
    Public WriteOnly Property DBConnForInstallationDLL() As String
        Set(ByVal strDBConn As String)
            Session("_VT_DBConn") = strDBConn
        End Set
    End Property
    Public WriteOnly Property CustomerName() As String
        Set(ByVal strName As String)
            Session("_VT_CustomerName") = strName
        End Set
    End Property
    Public WriteOnly Property IsVatExempt() As Boolean
        Set(ByVal blnExempt As Boolean)
            Session("_VT_VatExempt") = blnExempt
        End Set

    End Property
    Public WriteOnly Property PreSelectProductID() As Long
        Set(ByVal lngProductId As Long)
            Session("VT_ProdSelectProdID") = lngProductId
            Session("VT_PassedInProductID") = lngProductId
        End Set
    End Property

    Public WriteOnly Property PreSelectUnitPrice() As Double
        Set(ByVal dblUnitPrice As Double)
            Session("VT_PassedInUnitPrice") = dblUnitPrice

        End Set
    End Property
    '-----------------------------------------------------------
    'True is passed in here if the format of the products dropdown should be Code:Name
    'if false, then just the product name is shown here
    '------------------------------------------------------------

    Public WriteOnly Property ShowProductCodeWithName() As Boolean
        Set(ByVal blnShow As Boolean)
            Session("_VT_ShowProductCodewithName") = blnShow
        End Set
    End Property

    Public WriteOnly Property CustomerDiscount() As Double
        Set(ByVal dblDiscount As Double)
            Session("_VT_CustDiscount") = dblDiscount
        End Set
    End Property

    Public WriteOnly Property BatchMode() As Boolean

        Set(ByVal blnMode As Boolean)
            Session("_VT_BatchMode") = blnMode
            ' if batchmode is False we show the "Show all Batches" checkbox
            If Session("_VT_BatchMode") = False Then
                chkShowBatches.Visible = True

            Else
                chkShowBatches.Visible = False
            End If
        End Set
    End Property

    Public WriteOnly Property BatchesOn() As Boolean

        Set(ByVal blnMode As Boolean)
            Session("_VT_BatchesOn") = blnMode
            ' if batchmode is true tick the "Show all Batches" checkbox
            If Session("_VT_BatchesOn") = True Then
                chkShowBatches.Checked = True

            Else
                chkShowBatches.Checked = False
            End If
        End Set

    End Property

    '---------------------------------------------------------------
    'All of the following properties are read only
    '---------------------------------------------------------------

    Public ReadOnly Property SelectedCustomer() As Integer
        Get
            SelectedCustomer = Session("_VT_CustomerID")
        End Get
    End Property

    Public ReadOnly Property ProductId() As Integer
        Get
            Dim strName As String
            Dim objDataAccess As New VTDBFunctions.VTDBFunctions.ProductsFunctions

            If Session("_VT_ShowProductCodewithName") = True Then
                strName = Mid(cboProduct.SelectedValue, InStr(cboProduct.SelectedValue, cSEPARATOR) + Len(cSEPARATOR))
            Else
                strName = cboProduct.SelectedValue
            End If

            Session("_VT_ProductId") = objDataAccess.GetProductIdForName(strName)
            ProductId = Session("_VT_ProductId")
        End Get
    End Property

    Public ReadOnly Property ProductCode() As String
        Get
            ProductCode = Trim(txtProductCode.Text)
        End Get
    End Property

    Public ReadOnly Property CategoryId() As Integer
        Get
            Dim objDataAccess As New VTDBFunctions.VTDBFunctions.ProductsFunctions


            If cboProdCategory.SelectedValue <> "" Then
                CategoryId = objDataAccess.GetProductLineIDForName(cboProdCategory.SelectedValue)

            End If
        End Get

    End Property

    Public ReadOnly Property ProductName() As String
        Get
            Dim strName As String
            If Session("_VT_ShowProductCodewithName") = True Then
                strName = Mid(cboProduct.SelectedValue, InStr(cboProduct.SelectedValue, cSEPARATOR) + Len(cSEPARATOR))
            Else
                strName = cboProduct.SelectedValue
            End If
            ProductName = Trim(strName)
        End Get

    End Property

    Public ReadOnly Property Quantity() As Double
        Get

            Dim cell As New Infragistics.WebUI.UltraWebGrid.UltraGridCell
            Dim row As New Infragistics.WebUI.UltraWebGrid.UltraGridRow

            row = grdTraceCodes.DisplayLayout.ActiveRow


            If row Is Nothing Then
                Quantity = 0
            Else
                cell = row.Cells.FromKey("Qty")
                If IsNumeric(cell.Value) Then
                    Quantity = cell.Value
                Else
                    Quantity = 0
                End If

            End If


        End Get

    End Property

    Public ReadOnly Property Weight() As Double
        Get

            Dim cell As New Infragistics.WebUI.UltraWebGrid.UltraGridCell
            Dim row As New Infragistics.WebUI.UltraWebGrid.UltraGridRow

            row = grdTraceCodes.DisplayLayout.ActiveRow


            If row Is Nothing Then
                Weight = 0
            Else
                cell = row.Cells.FromKey("Wgt")
                If IsNumeric(cell.Value) Then
                    Weight = cell.Value
                Else
                    Weight = 0
                End If

            End If


        End Get

    End Property

    Public ReadOnly Property TracecodeId() As Long
        Get
            Dim strTraceCode As String
            Dim i As Integer

            If lblUsesTrace.Visible = False Then Exit Property

            Dim cell As New Infragistics.WebUI.UltraWebGrid.UltraGridCell
            Dim row As New Infragistics.WebUI.UltraWebGrid.UltraGridRow
            row = grdTraceCodes.DisplayLayout.ActiveRow

            If row Is Nothing Then
                TracecodeId = 0
            Else
                TracecodeId = row.Tag
            End If



        End Get
    End Property

    Public ReadOnly Property TraceCodeDesc() As String
        Get
            Dim intCurrentCol As Integer

            If lblUsesTrace.Visible = False Or (Session("_VT_BatchMode") = False And chkShowBatches.Checked = False) Then
                TraceCodeDesc = "N/A"
            Else

                Dim cell As New Infragistics.WebUI.UltraWebGrid.UltraGridCell
                Dim row As New Infragistics.WebUI.UltraWebGrid.UltraGridRow

                row = grdTraceCodes.DisplayLayout.ActiveRow

                If row Is Nothing Then
                    TraceCodeDesc = ""
                Else
                    cell = row.Cells.FromKey("TraceCode")
                    TraceCodeDesc = cell.Text
                End If
            End If
        End Get

    End Property

    Public ReadOnly Property LocationId() As Long
        Get

            Dim Location As String
            Dim i As Integer

            Dim cell As New Infragistics.WebUI.UltraWebGrid.UltraGridCell
            Dim row As New Infragistics.WebUI.UltraWebGrid.UltraGridRow
            Dim objTrace As New VTDBFunctions.VTDBFunctions.TraceFunctions


            row = grdTraceCodes.DisplayLayout.ActiveRow

            If row Is Nothing Then
                Location = ""
                LocationId = 0
            Else
                cell = row.Cells.FromKey("Location")
                Location = cell.Text
                LocationId = objTrace.GetLocationIdForText(Location)
            End If

        End Get

    End Property

    Public ReadOnly Property NetPrice() As Double
        Get

            Dim cell As New Infragistics.WebUI.UltraWebGrid.UltraGridCell
            Dim row As New Infragistics.WebUI.UltraWebGrid.UltraGridRow

            row = grdTraceCodes.DisplayLayout.ActiveRow


            If row Is Nothing Then
                NetPrice = 0
            Else
                cell = row.Cells.FromKey("NetPrice")
                If IsNumeric(cell.Value) Then
                    NetPrice = cell.Value
                Else
                    NetPrice = 0
                End If

            End If


        End Get

    End Property

    Public ReadOnly Property QtyInStock() As Double
        Get

            Dim cell As New Infragistics.WebUI.UltraWebGrid.UltraGridCell
            Dim row As New Infragistics.WebUI.UltraWebGrid.UltraGridRow

            row = grdTraceCodes.DisplayLayout.ActiveRow


            If row Is Nothing Then
                QtyInStock = 0
            Else
                cell = row.Cells.FromKey("InStock")
                If IsNumeric(cell.Value) Then
                    QtyInStock = cell.Value
                Else
                    QtyInStock = 0
                End If

            End If


        End Get

    End Property

    Public ReadOnly Property UnitPrice() As Double
        Get

            Dim cell As New Infragistics.WebUI.UltraWebGrid.UltraGridCell
            Dim row As New Infragistics.WebUI.UltraWebGrid.UltraGridRow

            row = grdTraceCodes.DisplayLayout.ActiveRow


            If row Is Nothing Then
                UnitPrice = 0
            Else
                cell = row.Cells.FromKey("UnitPrice")
                If IsNumeric(cell.Value) Then
                    UnitPrice = cell.Value
                Else
                    UnitPrice = 0
                End If

            End If


        End Get

    End Property
    Public ReadOnly Property VatPercent() As Double
        Get

            Dim cell As New Infragistics.WebUI.UltraWebGrid.UltraGridCell
            Dim row As New Infragistics.WebUI.UltraWebGrid.UltraGridRow

            row = grdTraceCodes.DisplayLayout.ActiveRow


            If row Is Nothing Then
                VatPercent = 0
            Else
                cell = row.Cells.FromKey("VAT")
                If IsNumeric(cell.Value) Then
                    VatPercent = cell.Value
                Else
                    VatPercent = 0
                End If

            End If
        End Get
    End Property

    Public ReadOnly Property VatPrice() As Double
        Get
            If VatPercent > 0 And NetPrice > 0 Then
                VatPrice = FormatNumber((NetPrice / 100) * VatPercent, 2)
            Else
                VatPrice = 0
            End If
        End Get
    End Property

    Public ReadOnly Property TotalInclVat() As Double
        Get
            Dim cell As New Infragistics.WebUI.UltraWebGrid.UltraGridCell
            Dim row As New Infragistics.WebUI.UltraWebGrid.UltraGridRow

            row = grdTraceCodes.DisplayLayout.ActiveRow


            If row Is Nothing Then
                TotalInclVat = 0
            Else
                cell = row.Cells.FromKey("TotalPrice")
                If IsNumeric(cell.Value) Then
                    TotalInclVat = cell.Value
                Else
                    TotalInclVat = 0
                End If

            End If
        End Get
    End Property

    Public ReadOnly Property SerialNum() As String
        Get

            Dim cell As New Infragistics.WebUI.UltraWebGrid.UltraGridCell
            Dim row As New Infragistics.WebUI.UltraWebGrid.UltraGridRow

            row = grdTraceCodes.DisplayLayout.ActiveRow


            If row Is Nothing Then
                SerialNum = ""
            Else
                cell = row.Cells.FromKey("SerialNum")
                SerialNum = cell.Value


            End If


        End Get
    End Property


    Public ReadOnly Property GridRowData As DataSet
        Get

            Dim cell As New Infragistics.WebUI.UltraWebGrid.UltraGridCell
            Dim row As New Infragistics.WebUI.UltraWebGrid.UltraGridRow


            Dim ds As New Data.DataSet

            Dim objTrace As New VTDBFunctions.VTDBFunctions.TraceFunctions


            '"TraceCode" 
            '"InStock" 
            ' Key="Qty" 
            ' Key="UseByOrSellBy" 
            ' Key="Wgt" 
            'Key="UnitPrice" 
            ' Key="Location"
            ' Key="NetPrice" 
            ' Key="VAT" 
            '            Key = "TotalPrice"
            '            Key = "SerialNum"
            ' Key="Barcode" 
            ' Key="UseByOrSellBy" 
            ' Key="DateRec" 



            ds.Tables.Add("GridRows")
            With ds.Tables("GridRows").Columns

                .Add("TraceCodeId", System.Type.GetType("System.Int64"))
                .Add("TraceCode", System.Type.GetType("System.String"))
                .Add("Location", System.Type.GetType("System.String"))
                .Add("LocationId", System.Type.GetType("System.Int64"))
                .Add("InStock", System.Type.GetType("System.Double"))
                .Add("Weight", System.Type.GetType("System.Double"))
                .Add("Quantity", System.Type.GetType("System.Double"))
                .Add("UnitPrice", System.Type.GetType("System.Double"))
                .Add("NetPrice", System.Type.GetType("System.Double"))
                .Add("VAT", System.Type.GetType("System.Double"))
                .Add("TotalPrice", System.Type.GetType("System.Double"))
                .Add("SerialNum", System.Type.GetType("System.String"))
                .Add("Barcode", System.Type.GetType("System.String"))

            End With

            For Each row In grdTraceCodes.Rows

                If row.Cells.FromKey("Qty").Value = 0 And row.Cells.FromKey("Wgt").Value = 0 Then
                Else
                    Dim objRow As System.Data.DataRow

                    objRow = ds.Tables("GridRows").NewRow

                    objRow("TraceCodeId") = IIf(IsNothing(row.Tag), 0, row.Tag)
                    objRow("TraceCode") = Trim(row.Cells.FromKey("TraceCode").Value)
                    objRow("Location") = row.Cells.FromKey("Location").Value
                    objRow("LocationId") = objTrace.GetLocationIdForText(row.Cells.FromKey("Location").Value)
                    objRow("InStock") = row.Cells.FromKey("InStock").Value
                    objRow("Weight") = row.Cells.FromKey("Wgt").Value
                    objRow("Quantity") = row.Cells.FromKey("Qty").Value
                    objRow("UnitPrice") = CDbl(row.Cells.FromKey("UnitPrice").Value)
                    objRow("NetPrice") = CDbl(row.Cells.FromKey("NetPrice").Value)
                    objRow("VAT") = CDbl(row.Cells.FromKey("VAT").Value)
                    objRow("TotalPrice") = CDbl(row.Cells.FromKey("TotalPrice").Value)
                    objRow("SerialNum") = row.Cells.FromKey("SerialNum").Value
                    objRow("Barcode") = row.Cells.FromKey("Barcode").Value

                    ds.Tables("GridRows").Rows.Add(objRow)
                End If
            Next

            GridRowData = ds

        End Get
    End Property

    '----------------------------------------------------------------------
    'This should be called to populate the control before it is shown in the calling code
    'If a customer Id was passed in, it will show the selected customer and use it to apply
    'customer special pricing.
    'If ShowProductCodeWith name is set to true then the product list will have the format
    'Code:Name
    '
    '---------------------------------------------------------------------------
    Public Sub FillControl()
        ' set defaults
        Dim dsLocationData As DataSet
        Dim ds As New DataSet
        Dim objdataAccess As New VTDBFunctions.VTDBFunctions.ProductsFunctions
        Dim objTrace As New VTDBFunctions.VTDBFunctions.TraceFunctions
        Dim i As Integer
        Dim sender As Object
        Dim e As System.EventArgs

        If IsPostBack = False Then


            'clear the combos
            cboProdCategory.Items.Clear()
            cboProduct.Items.Clear()

            'fill the product category recordset
            ds = objdataAccess.GetProductLines

            'now fill the combo setting
            cboProdCategory.Items.Add("Show All")

            Dim SortedRows() As DataRow
            SortedRows = ds.Tables(0).Select(Nothing, "Product_Line_Text")

            For i = 0 To SortedRows.GetUpperBound(0)
                cboProdCategory.Items.Add(SortedRows(i).Item("Product_Line_Text"))
            Next

            ClearTraceCodeGrid()

            cboProdCategory_SelectedIndexChanged(sender, e)

            ' load the Locations UDT
            ds = objTrace.GetAllLocations
            For i = 0 To ds.Tables(0).Rows.Count - 1
                ReDim Preserve udtLocations(UBound(udtLocations) + 1)
                udtLocations(UBound(udtLocations)).LocationId = ds.Tables(0).Rows(i).Item("LocationId")
                udtLocations(UBound(udtLocations)).LocationText = ds.Tables(0).Rows(i).Item("LocationText")

            Next i
            'if a product is being preloaded then do it now
            Dim strname As String
            If (Not Session("VT_ProdSelectProdID") Is Nothing) And (IIf(IsNumeric(Session("VT_ProdSelectProdID")), Session("VT_ProdSelectProdID"), 0) > 0) Then
                If Session("_VT_ShowProductCodewithName") = True Then
                    strname = objdataAccess.GetProductCode(Session("VT_ProdSelectProdID")) & cSEPARATOR & objdataAccess.GetProductNameForId(Session("VT_ProdSelectProdID"))
                Else
                    strname = objdataAccess.GetProductNameForId(Session("VT_ProdSelectProdID"))

                End If
                Session("VT_ProdSelectProdID") = 0
            End If


            cboProduct.SelectedValue = strname
            cboProduct_SelectedIndexChanged(sender, e)


        End If


    End Sub

    Private Sub cboProdCategory_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboProdCategory.SelectedIndexChanged
        Dim ds As New DataSet
        Dim objDataAccess As New VTDBFunctions.VTDBFunctions.ProductsFunctions
        Dim lngProductLine As Long
        Dim strTemp As String
        Dim i As Integer

        If cboProdCategory.SelectedValue = "" Then Exit Sub

        'clear the combo
        cboProduct.Items.Clear()

        'if show all is selected show a message that if they have multiple products with the same name it could cause confusion
        If cboProdCategory.SelectedValue = "Show All" Then
            ds = objDataAccess.GetProducts

        Else 'if a category is selected then only show the products for that category
            lngProductLine = objDataAccess.GetProductLineIDForName(cboProdCategory.SelectedValue)
            ds = objDataAccess.GetProductsForLine(lngProductLine)
        End If

        'sort the recordset by product line text so that they show alphabetically
        Dim SortedRows() As DataRow
        If ds.Tables(0).Rows.Count > 0 Then
            If Session("_VT_ShowProductCodewithName") = True Then
                SortedRows = ds.Tables(0).Select(Nothing, "Catalog_Number")
            Else
                SortedRows = ds.Tables(0).Select(Nothing, "Product_Name")
            End If
        End If

        'now fill the combo
        For i = 0 To ds.Tables(0).Rows.Count - 1
            If Session("_VT_ShowProductCodewithName") = True Then
                'if ShowCodeWithName has been chosen then display all products in the product
                ' combo with the product code in front of them
                strTemp = Trim(IIf(IsDBNull(SortedRows(i).Item("Catalog_Number")), "", SortedRows(i).Item("Catalog_Number")))
                cboProduct.Items.Add(strTemp & cSEPARATOR & Trim(SortedRows(i).Item("Product_Name")))
            Else
                cboProduct.Items.Add(RTrim(SortedRows(i).Item("Product_Name")))
            End If

        Next

        cboProduct_SelectedIndexChanged(sender, e)


    End Sub

    Private Sub chkShowBatches_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkShowBatches.CheckedChanged
        ' change the Trace code grid layout

        Dim objDataAccess As New VTDBFunctions.VTDBFunctions.ProductsFunctions
        Dim lngProductId As Long
        Dim strName As String
        'if ShowCodeWithName has been chosen then the combo will contain Code : ProductName
        ' so we must extract the product name to search for the code

        If Session("_VT_ShowProductCodewithName") = True Then
            strName = Mid(cboProduct.SelectedValue, InStr(cboProduct.SelectedValue, cSEPARATOR) + Len(cSEPARATOR))
        Else
            strName = cboProduct.SelectedValue
        End If

        'get the product id for the name
        lngProductId = objDataAccess.GetProductIdForName(strName)

        'set up the trace code grid for the selected product
        'FillTraceCodeGrid(lngProductId)
        FillTraceCodeGridNew(lngProductId)

    End Sub

    '---------------------------------------------------------------
    'Function FillTraceCodeGrid
    'Populates the trace code grid for the selected product.
    'takes a long parameter productId
    '---------------------------------------------------------------

    Private Sub FillTraceCodeGrid(ByVal lngProductId As Long)
        Dim varparams() As Object
        Dim objDataAccess As New VTDBFunctions.VTDBFunctions.ProductsFunctions
        Dim dsBatch As New DataSet
        Dim dsProducts As New DataSet
        Dim dblVatRate As Double
        Dim dsLocations As New DataSet
        Dim dblTotalInStock As Double
        Dim i As Integer
        Dim row As New Infragistics.WebUI.UltraWebGrid.UltraGridRow
        Dim objTrace As New VTDBFunctions.VTDBFunctions.TraceFunctions
        Dim objBPA As New VT_CommonFunctions.CommonFunctions
        Dim dblQtyInStock, dblAllocated As Double
        Dim objTelesales As New VTDBFunctions.VTDBFunctions.TelesalesFunctions


        Dim strDisplay As String = objBPA.GetConfigItem("UseByOrBestBefore")
        Dim strSort As String
        If strDisplay = "SellBy" Then
            strSort = "SellbyDate"
        Else
            strSort = "UseByDate"
        End If
        dsBatch = objDataAccess.GetSortedTraceCodesForProduct(lngProductId, strSort)



        'get the product recordset too because we need the vat rate
        dsProducts = objDataAccess.GetProductForId(lngProductId)
        If dsProducts.Tables(0).Rows.Count > 0 Then
            If Session("_VT_VatExempt") = True Then
                dblVatRate = 0
            Else
                If IsDBNull(dsProducts.Tables(0).Rows(0).Item("Vat_Rate")) Then
                    dblVatRate = 0
                Else
                    If Trim(dsProducts.Tables(0).Rows(0).Item("Vat_Rate")) <> "" Then
                        dblVatRate = IIf(IsDBNull(dsProducts.Tables(0).Rows(0).Item("Vat_Rate")), 0, dsProducts.Tables(0).Rows(0).Item("Vat_Rate"))
                    Else
                        dblVatRate = 0
                    End If

                End If

            End If

        End If
        '

        ClearTraceCodeGrid()

        If dsBatch.Tables(0).Rows.Count > 0 Then
            If Session("_VT_BatchMode") = True Or (Session("_VT_BatchMode") = False And chkShowBatches.Checked = True) Then
                ' show all batches

                For i = 0 To dsBatch.Tables(0).Rows.Count - 1
                    If dsBatch.Tables(0).Rows(i).Item("NumInStores") <> 0 Then
                        'Now populate the grid
                        With grdTraceCodes
                            .Rows.Add(dsBatch.Tables(0).Rows(i).Item("TraceCodeId"))
                            row = .Rows.FromKey(dsBatch.Tables(0).Rows(i).Item("TraceCodeId"))
                            If IsDBNull(dsBatch.Tables(0).Rows(i).Item("TraceCodeDesc")) Then
                                row.Cells.FromKey("TraceCode").Value = ""
                            Else
                                row.Cells.FromKey("TraceCode").Value = Trim(dsBatch.Tables(0).Rows(i).Item("TraceCodeDesc"))
                            End If

                            ' DateReceived
                            If IsDBNull(dsBatch.Tables(0).Rows(i).Item("DateEnteredInSystem")) Then
                                row.Cells.FromKey("DateRec").Value = ""
                            Else
                                row.Cells.FromKey("DateRec").Value = Trim(dsBatch.Tables(0).Rows(i).Item("DateEnteredInSystem"))
                            End If

                            ' SellBy or UseBy
                            If strDisplay = "SellBy" Then
                                .Columns.FromKey("UseByOrSellBy").Header.Caption = "Sell By Date"
                                If IsDBNull(dsBatch.Tables(0).Rows(i).Item("SellbyDate")) Then
                                    row.Cells.FromKey("UseByOrSellBy").Value = ""
                                Else
                                    row.Cells.FromKey("UseByOrSellBy").Value = Trim(dsBatch.Tables(0).Rows(i).Item("SellbyDate"))
                                End If
                            Else
                                .Columns.FromKey("UseByOrSellBy").Header.Caption = "Use By Date"
                                If IsDBNull(dsBatch.Tables(0).Rows(i).Item("UseByDate")) Then
                                    row.Cells.FromKey("UseByOrSellBy").Value = ""
                                Else
                                    row.Cells.FromKey("UseByOrSellBy").Value = Trim(dsBatch.Tables(0).Rows(i).Item("UseByDate"))
                                End If
                            End If


                            ' QtyInStock
                            If IsDBNull(dsBatch.Tables(0).Rows(i).Item("NumInStores")) Then
                                dblQtyInStock = 0
                            Else
                                dblQtyInStock = CDbl(Trim(dsBatch.Tables(0).Rows(i).Item("NumInStores")))
                            End If

                            If IsDBNull(dsBatch.Tables(0).Rows(i).Item("TraceAllocated")) Then
                                dblAllocated = 0
                            Else
                                dblAllocated = CDbl(Trim(dsBatch.Tables(0).Rows(i).Item("TraceAllocated")))
                            End If
                            row.Cells.FromKey("InStock").Value = dblQtyInStock - dblAllocated

                            ' default the Qty and Wgt columns to 1
                            row.Cells.FromKey("Qty").Value = 1
                            row.Cells.FromKey("Wgt").Value = 1

                            'Price Charged
                            'just display the unit price and the user can edit it
                            row.Cells.FromKey("UnitPrice").Value = FormatCurrency(GetPriceCharged(lngProductId), 2)

                            'location
                            dsLocations = objTrace.GetLocationData(dsBatch.Tables(0).Rows(i).Item("TraceCodeID"), 0)
                            If dsLocations.Tables(0).Rows.Count > 0 Then
                                row.Cells.FromKey("Location").Value = objTrace.GetLocationTextForId(dsLocations.Tables(0).Rows(0).Item("LocationId"))
                            End If
                            'vat rate
                            row.Cells.FromKey("VAT").Value = dblVatRate
                            ' store the TracecodeId in the row Tag
                            row.Tag = dsBatch.Tables(0).Rows(i).Item("TraceCodeID")
                        End With
                    End If

                Next

                'Determine which way you want the column sorted
                'grdTraceCodes.Columns.FromKey("UseByOrSellBy").SortIndicator = Infragistics.WebUI.UltraWebGrid.SortIndicator.Descending
                ''Then add it to the sorted columns collection for the band that it is in
                'grdTraceCodes.Bands(0).SortedColumns.Add(grdTraceCodes.Columns.FromKey("UseByOrSellBy"))


            Else

                ' only show one row where the Qty is the qty from the Product table
                With grdTraceCodes
                    .Rows.Add(dsProducts.Tables(0).Rows(0).Item("ProductID"))
                    row = .Rows.FromKey(dsProducts.Tables(0).Rows(0).Item("ProductID"))
                    ' Free Stock
                    dblQtyInStock = IIf(IsDBNull(dsProducts.Tables(0).Rows(0).Item("InStock")), 0, dsProducts.Tables(0).Rows(0).Item("InStock"))
                    dblAllocated = IIf(IsDBNull(dsProducts.Tables(0).Rows(0).Item("Allocated")), 0, dsProducts.Tables(0).Rows(0).Item("Allocated"))

                    ' default the Qty and Wgt columns to 1
                    row.Cells.FromKey("Qty").Value = 1
                    row.Cells.FromKey("Wgt").Value = 1

                    row.Cells.FromKey("InStock").Value = dblQtyInStock - dblAllocated
                    'Price Charged
                    'just display the unit price and the user can edit it
                    row.Cells.FromKey("UnitPrice").Value = FormatCurrency(GetPriceCharged(lngProductId), 2)
                    'location

                    dsLocations = objTrace.GetLocationData(dsBatch.Tables(0).Rows(0).Item("TraceCodeID"), 0)
                    If dsLocations.Tables(0).Rows.Count > 0 Then
                        row.Cells.FromKey("Location").Value = objTrace.GetLocationTextForId(dsLocations.Tables(0).Rows(0).Item("LocationId"))
                    End If
                    'vat rate
                    row.Cells.FromKey("VAT").Value = dblVatRate
                End With



            End If
        Else
            ' there are no batches in the system for this product so just display an empty row
            grdTraceCodes.Rows.Add("Default")
            row = grdTraceCodes.Rows.FromKey("Default")
            ' QtyInStock
            row.Cells.FromKey("InStock").Value = 0
            row.Cells.FromKey("UnitPrice").Value = FormatCurrency(GetPriceCharged(lngProductId), 2)
            '''''row.Cells.FromKey("UnitPrice").Value = FormatCurrency(IIf(IsNumeric(objTelesales.GetPriceForProductAndCustomer(lngProductId, CLng(Session("_VT_CustomerID")), "ASP")), objTelesales.GetPriceForProductAndCustomer(lngProductId, CLng(Session("_VT_CustomerID")), "ASP"), 0), 2)



            'vat rate
            row.Cells.FromKey("VAT").Value = dblVatRate
        End If

        Exit Sub


    End Sub

    '---------------------------------------------------------------
    'Function FillTraceCodeGrid
    'Populates the trace code grid for the selected product.
    'takes a long parameter productId
    '---------------------------------------------------------------

    Private Sub FillTraceCodeGridNew(ByVal lngProductId As Long)

        Dim varparams() As Object
        Dim objDataAccess As New VTDBFunctions.VTDBFunctions.ProductsFunctions
        Dim dsBatch As New DataSet
        Dim dsProducts As New DataSet
        Dim dblVatRate As Double
        Dim dsLocations As New DataSet
        Dim dblTotalInStock As Double
        Dim i As Integer
        Dim row As New Infragistics.WebUI.UltraWebGrid.UltraGridRow
        Dim objTrace As New VTDBFunctions.VTDBFunctions.TraceFunctions
        Dim objBPA As New VT_CommonFunctions.CommonFunctions
        Dim dblQtyInStock, dblAllocated As Double
        Dim objTelesales As New VTDBFunctions.VTDBFunctions.TelesalesFunctions
        Dim blnUseSerialNums As Boolean
        Dim dbltotalQTYBatches As Double
        Dim lngLocationId As Long
        Dim intGridRowCount As Integer
        Dim intDecimalPlaces As Integer

        Dim objCommonFuncs As New BPADotNetCommonFunctions.VT_CommonFunctions.CommonFunctions
        Dim strSerialNums As String = objCommonFuncs.GetConfigItem("SerialNums")

        If UCase(strSerialNums) = "YES" Then
            blnUseSerialNums = True
        Else
            blnUseSerialNums = False
        End If

        Dim strTempDecimal As String = objCommonFuncs.GetConfigItem("TelesalesDecimalPlaces")
        If strTempDecimal = "" OrElse IsNumeric(strTempDecimal) = False Then
            intDecimalPlaces = 4
        Else
            If CInt(strTempDecimal) < 3 Then
                intDecimalPlaces = 2
            Else
                intDecimalPlaces = CInt(strTempDecimal)
            End If
        End If


        Dim strDisplay As String = objBPA.GetConfigItem("UseByOrBestBefore")
        Dim strSort As String

        If blnUseSerialNums = True Then ' Load the grid with serial nums rather than trace codes

            dsBatch = objDataAccess.GetSerialNumsForProduct(lngProductId)

        Else
            Select Case UCase(strDisplay)
                Case "SELLBYDATE"
                    strSort = "SellbyDate"
                Case "USEBYDATE"
                    strSort = "UseByDate"
                Case Else
                    strSort = ""
            End Select

            If strSort = "" Then
                dsBatch = objDataAccess.GetTraceCodesForProduct(lngProductId)
            Else
                dsBatch = objDataAccess.GetSortedTraceCodesForProduct(lngProductId, strSort)
            End If
        End If


        'get the product recordset too because we need the vat rate
        dsProducts = objDataAccess.GetProductForId(lngProductId)
        If dsProducts.Tables(0).Rows.Count > 0 Then
            If Session("_VT_VatExempt") = True Then
                dblVatRate = 0
            Else
                If IsDBNull(dsProducts.Tables(0).Rows(0).Item("Vat_Rate")) Then
                    dblVatRate = 0
                Else
                    If Trim(dsProducts.Tables(0).Rows(0).Item("Vat_Rate")) <> "" Then
                        dblVatRate = IIf(IsDBNull(dsProducts.Tables(0).Rows(0).Item("Vat_Rate")), 0, dsProducts.Tables(0).Rows(0).Item("Vat_Rate"))
                    Else
                        dblVatRate = 0
                    End If

                End If

            End If

        End If
        '

        ClearTraceCodeGrid()

        If dsBatch.Tables(0).Rows.Count > 0 Then
            If Session("_VT_BatchMode") = True Or (Session("_VT_BatchMode") = False And chkShowBatches.Checked = True) Then
                ' show all batches

                Dim dblCurrentQty As Double
                Dim dblCurrentwgt As Double
                Dim blnShowTC As Boolean
                Dim blnDontAddToGrid As Boolean
                Dim lngTempCurrentLocationID As Long
                Dim lngTempLatestLocationID As Long
                Dim strCurrentSerialNum As String
                Dim dblNumInStores As Double
                Dim dsSerialNumDetails As New DataSet
                Dim objSerial As New VTDBFunctions.VTDBFunctions.SerialNumFunctions
                Dim intRowToUse As Integer
                Dim j As Integer


                For i = 0 To dsBatch.Tables(0).Rows.Count - 1

                    blnShowTC = True
                    If blnUseSerialNums = False Then

                        If dsBatch.Tables(0).Rows(i).Item("NumInStores") = 0 Then

                            blnShowTC = False
                        End If
                    End If

                    blnDontAddToGrid = False

                    If blnUseSerialNums = True Then
                        With dsBatch.Tables(0).Rows(i)

                            strCurrentSerialNum = IIf(IsDBNull(.Item("SerialNum")), "", .Item("SerialNum"))
                            lngTempCurrentLocationID = IIf(IsDBNull(.Item("LocationID")), 0, .Item("LocationID"))

                            dsSerialNumDetails = objSerial.GetDetailsForSerialNumAndTraceId(strCurrentSerialNum, .Item("TraceCodeID"))

                            intRowToUse = 0

                            If strCurrentSerialNum <> "" Then
                                lngTempLatestLocationID = objSerial.GetMostRecentLocationIdForSerial(strCurrentSerialNum, .Item("TraceCodeID"))

                                For j = 0 To dsSerialNumDetails.Tables(0).Rows.Count - 1

                                    If lngTempLatestLocationID = dsSerialNumDetails.Tables(0).Rows(j).Item("LocationId") Then
                                        intRowToUse = j
                                    End If
                                Next
                            Else
                                For j = 0 To dsSerialNumDetails.Tables(0).Rows.Count - 1

                                    If .Item("LocationID") = dsSerialNumDetails.Tables(0).Rows(j).Item("LocationId") Then
                                        intRowToUse = j
                                    End If
                                Next
                            End If

                            dblCurrentwgt = dsSerialNumDetails.Tables(0).Rows(intRowToUse).Item("Weight")
                            dblCurrentQty = dsSerialNumDetails.Tables(0).Rows(intRowToUse).Item("Quantity")

                            If blnDontAddToGrid = False And (dblCurrentwgt < 0.0001 Or dblCurrentQty < 5) Then
                                blnDontAddToGrid = True
                            ElseIf strCurrentSerialNum <> "" And lngTempLatestLocationID <> lngTempCurrentLocationID Then
                                blnDontAddToGrid = True
                            End If

                            dblCurrentwgt = 0
                            dblCurrentQty = 0
                        End With
                    End If


                    If blnShowTC = True And blnDontAddToGrid = False Then

                        'Now populate the grid
                        With grdTraceCodes
                            intGridRowCount = .Rows.Count

                            .Rows.Add(intGridRowCount)
                            row = .Rows.FromKey(intGridRowCount)

                            If IsDBNull(dsBatch.Tables(0).Rows(i).Item("TraceCodeDesc")) Then
                                row.Cells.FromKey("TraceCode").Value = ""
                            Else
                                row.Cells.FromKey("TraceCode").Value = Trim(dsBatch.Tables(0).Rows(i).Item("TraceCodeDesc"))
                            End If

                            ' DateReceived
                            If IsDBNull(dsBatch.Tables(0).Rows(i).Item("DateEnteredInSystem")) Then
                                row.Cells.FromKey("DateRec").Value = ""
                            Else
                                row.Cells.FromKey("DateRec").Value = Trim(dsBatch.Tables(0).Rows(i).Item("DateEnteredInSystem"))
                            End If

                            ' SellBy or UseBy
                            If strDisplay = "SellBy" Then
                                .Columns.FromKey("UseByOrSellBy").Header.Caption = "Sell By Date"
                                If IsDBNull(dsBatch.Tables(0).Rows(i).Item("SellbyDate")) Then
                                    row.Cells.FromKey("UseByOrSellBy").Value = ""
                                Else
                                    row.Cells.FromKey("UseByOrSellBy").Value = Trim(dsBatch.Tables(0).Rows(i).Item("SellbyDate"))
                                End If
                            Else
                                .Columns.FromKey("UseByOrSellBy").Header.Caption = "Use By Date"
                                If IsDBNull(dsBatch.Tables(0).Rows(i).Item("UseByDate")) Then
                                    row.Cells.FromKey("UseByOrSellBy").Value = ""
                                Else
                                    row.Cells.FromKey("UseByOrSellBy").Value = Trim(dsBatch.Tables(0).Rows(i).Item("UseByDate"))
                                End If
                            End If

                            If blnUseSerialNums = True Then

                                If strCurrentSerialNum <> "" Then
                                    dblCurrentQty = dsSerialNumDetails.Tables(0).Rows(intRowToUse).Item("Quantity")
                                    dblCurrentwgt = dsSerialNumDetails.Tables(0).Rows(intRowToUse).Item("Weight")
                                Else
                                    dblCurrentQty = dsBatch.Tables(0).Rows(i).Item("Qty")
                                    dblCurrentwgt = dsBatch.Tables(0).Rows(i).Item("Wgt")
                                End If

                                dblCurrentwgt = Math.Round(dblCurrentwgt, intDecimalPlaces)

                                If objDataAccess.GetUnitOfSale(lngProductId) = 1 Then
                                    row.Cells.FromKey("InStock").Value = dblCurrentQty
                                    dbltotalQTYBatches = dbltotalQTYBatches + dblCurrentQty

                                Else
                                    row.Cells.FromKey("InStock").Value = dblCurrentwgt
                                    dbltotalQTYBatches = dbltotalQTYBatches + dblCurrentwgt

                                End If
                            Else
                                ' QtyInStock
                                If IsDBNull(dsBatch.Tables(0).Rows(i).Item("NumInStores")) Then
                                    dblQtyInStock = 0
                                Else
                                    dblQtyInStock = CDbl(Trim(dsBatch.Tables(0).Rows(i).Item("NumInStores")))
                                End If

                                If IsDBNull(dsBatch.Tables(0).Rows(i).Item("TraceAllocated")) Then
                                    dblAllocated = 0
                                Else
                                    dblAllocated = CDbl(Trim(dsBatch.Tables(0).Rows(i).Item("TraceAllocated")))
                                End If

                                dblNumInStores = dblQtyInStock - dblAllocated
                                dblNumInStores = Math.Round(dblNumInStores, intDecimalPlaces)

                                row.Cells.FromKey("InStock").Value = dblNumInStores
                                dbltotalQTYBatches = dbltotalQTYBatches + dblNumInStores

                            End If
                            ' default the Qty and Wgt columns to 1
                            row.Cells.FromKey("Qty").Value = 0
                            row.Cells.FromKey("Wgt").Value = 0

                            'Price Charged
                            'just display the unit price and the user can edit it
                            'IF there is a price passed in relating to the passed in product then use this rather than looking it up
                            If lngProductId = Session("VT_PassedInProductID") And Session("VT_PassedInUnitPrice") > 0 Then
                                row.Cells.FromKey("UnitPrice").Value = FormatCurrency(Session("VT_PassedInUnitPrice"), 2)
                            Else
                                row.Cells.FromKey("UnitPrice").Value = FormatCurrency(GetPriceCharged(lngProductId), 2)
                            End If

                            'vat rate
                            row.Cells.FromKey("VAT").Value = dblVatRate
                            ' store the TracecodeId in the row Tag
                            row.Tag = dsBatch.Tables(0).Rows(i).Item("TraceCodeID")

                            If blnUseSerialNums = True Then
                                row.Cells.FromKey("Location").Value = dsSerialNumDetails.Tables(0).Rows(intRowToUse).Item("Location")
                                row.Cells.FromKey("SerialNum").Value = dsSerialNumDetails.Tables(0).Rows(intRowToUse).Item("SerialNum")
                                row.Cells.FromKey("Barcode").Value = dsSerialNumDetails.Tables(0).Rows(intRowToUse).Item("Barcode")
                            Else
                                'location
                                dsLocations = objTrace.GetLocationData(dsBatch.Tables(0).Rows(i).Item("TraceCodeID"), 0)
                                If dsLocations.Tables(0).Rows.Count > 0 Then
                                    row.Cells.FromKey("Location").Value = objTrace.GetLocationTextForId(dsLocations.Tables(0).Rows(0).Item("LocationId"))
                                End If

                            End If

                        End With
                    End If

                Next

                If dbltotalQTYBatches = 0 Then 'if there are batches but the qty in stores is 0 add another row for selecting
                    'there are no batches for this product so just add one row to the table for the product
                    ' only show one row where the Qty is the InStock value from the Product record
                    With grdTraceCodes
                        .Rows.Add(dsProducts.Tables(0).Rows(0).Item("ProductID"))
                        row = .Rows.FromKey(dsProducts.Tables(0).Rows(0).Item("ProductID"))
                        ' Free Stock
                        dblQtyInStock = IIf(IsDBNull(dsProducts.Tables(0).Rows(0).Item("InStock")), 0, dsProducts.Tables(0).Rows(0).Item("InStock"))
                        dblAllocated = IIf(IsDBNull(dsProducts.Tables(0).Rows(0).Item("Allocated")), 0, dsProducts.Tables(0).Rows(0).Item("Allocated"))

                        ' default the Qty and Wgt columns to 1
                        row.Cells.FromKey("Qty").Value = 0
                        row.Cells.FromKey("Wgt").Value = 0

                        row.Cells.FromKey("InStock").Value = dblQtyInStock - dblAllocated
                        'Price Charged
                        'just display the unit price and the user can edit it
                        'IF there is a price passed in relating to the passed in product then use this rather than looking it up
                        If lngProductId = Session("VT_PassedInProductID") And Session("VT_PassedInUnitPrice") > 0 Then
                            row.Cells.FromKey("UnitPrice").Value = FormatCurrency(Session("VT_PassedInUnitPrice"), 2)
                        Else
                            row.Cells.FromKey("UnitPrice").Value = FormatCurrency(GetPriceCharged(lngProductId), 2)
                        End If
                        'location

                        dsLocations = objTrace.GetLocationData(dsBatch.Tables(0).Rows(0).Item("TraceCodeID"), 0)
                        If dsLocations.Tables(0).Rows.Count > 0 Then
                            row.Cells.FromKey("Location").Value = objTrace.GetLocationTextForId(dsLocations.Tables(0).Rows(0).Item("LocationId"))
                        End If
                        'vat rate
                        row.Cells.FromKey("VAT").Value = dblVatRate
                    End With
                End If

            Else
                ' only show one row where the Qty is the qty from the Product table
                With grdTraceCodes
                    .Rows.Add(dsProducts.Tables(0).Rows(0).Item("ProductID"))
                    row = .Rows.FromKey(dsProducts.Tables(0).Rows(0).Item("ProductID"))
                    ' Free Stock
                    dblQtyInStock = IIf(IsDBNull(dsProducts.Tables(0).Rows(0).Item("InStock")), 0, dsProducts.Tables(0).Rows(0).Item("InStock"))
                    dblAllocated = IIf(IsDBNull(dsProducts.Tables(0).Rows(0).Item("Allocated")), 0, dsProducts.Tables(0).Rows(0).Item("Allocated"))

                    ' default the Qty and Wgt columns to 1
                    row.Cells.FromKey("Qty").Value = 0
                    row.Cells.FromKey("Wgt").Value = 0

                    row.Cells.FromKey("InStock").Value = dblQtyInStock - dblAllocated
                    'Price Charged
                    'just display the unit price and the user can edit it
                    'IF there is a price passed in relating to the passed in product then use this rather than looking it up
                    If lngProductId = Session("VT_PassedInProductID") And Session("VT_PassedInUnitPrice") > 0 Then
                        row.Cells.FromKey("UnitPrice").Value = FormatCurrency(Session("VT_PassedInUnitPrice"), 2)
                    Else
                        row.Cells.FromKey("UnitPrice").Value = FormatCurrency(GetPriceCharged(lngProductId), 2)
                    End If
                    'location

                    dsLocations = objTrace.GetLocationData(dsBatch.Tables(0).Rows(0).Item("TraceCodeID"), 0)
                    If dsLocations.Tables(0).Rows.Count > 0 Then
                        row.Cells.FromKey("Location").Value = objTrace.GetLocationTextForId(dsLocations.Tables(0).Rows(0).Item("LocationId"))
                    End If
                    'vat rate
                    row.Cells.FromKey("VAT").Value = dblVatRate
                End With

            End If
        Else
            ' there are no batches in the system for this product so just display an empty row
            grdTraceCodes.Rows.Add("Default")
            row = grdTraceCodes.Rows.FromKey("Default")
            ' QtyInStock
            row.Cells.FromKey("InStock").Value = 0
            'IF there is a price passed in relating to the passed in product then use this rather than looking it up
            If lngProductId = Session("VT_PassedInProductID") And Session("VT_PassedInUnitPrice") > 0 Then
                row.Cells.FromKey("UnitPrice").Value = FormatCurrency(Session("VT_PassedInUnitPrice"), 2)
            Else
                row.Cells.FromKey("UnitPrice").Value = FormatCurrency(GetPriceCharged(lngProductId), 2)
            End If

            'vat rate
            row.Cells.FromKey("VAT").Value = dblVatRate
        End If



    End Sub



    Public Function GetPriceCharged(ByVal lngProductId As Long) As Double

        Dim strListPrice As String
        Dim objDataAccess As New VTDBFunctions.VTDBFunctions.ProductsFunctions

        Dim strPriceToDisplay As String

        'we need both the customer and product selected before we can calculate the price

        With udtCustomer
            .lngCustomerId = Session("_VT_CustomerID")
            .dblDiscountPercent = Session("_VT_CustDiscount")
        End With

        With udtProduct
            .lngProductId = CLng(lngProductId)
            strListPrice = objDataAccess.GetProductListPrice(.lngProductId)
            If strListPrice <> "" Then
                .dblPrice = CDbl(strListPrice)
            End If
        End With

        strPriceToDisplay = calculateCustomerDiscount(udtProduct, udtCustomer)
        If strPriceToDisplay <> "" And IsNumeric(strPriceToDisplay) = True Then
            GetPriceCharged = CDbl(strPriceToDisplay)
        Else
            GetPriceCharged = udtProduct.dblPrice
        End If

        Exit Function

    End Function
    Public Function calculateCustomerDiscount _
        ( _
    ByVal udtProduct As TProductRecord, _
    ByVal udtCustomer As TCustomerRecord, _
    Optional ByVal bApplyDiscountToBasePrice As Boolean = True) As String
        Dim objDataAccess As New VTDBFunctions.VTDBFunctions.ProductsFunctions

        Dim curSpecialPrice As Double
        Dim curTemp As Double
        ' Calculated according to the following formula
        ' Discount = (Base price * (Customer discount/100))
        If (Len(udtProduct.dblPrice) > 0) Then
            Dim curDiscount As Double

            curDiscount = (udtProduct.dblPrice * (CDbl(udtCustomer.dblDiscountPercent) / 100))

            curTemp = udtProduct.dblPrice - curDiscount
        End If

        ' some systems require "special" prices for some customers
        ' We use the tblSpecialItems table to hold these prices
        If udtCustomer.lngCustomerId > 0 And udtProduct.lngProductId > 0 Then

            ' if a special support DLL is defined for this installation then try to get a special price from it
            Dim strCustomerDLL As String = System.Configuration.ConfigurationSettings.AppSettings("InstallationDLL")
            If strCustomerDLL <> "" Then
                Dim objCustDLL As TTIInstallationSpecific.Interface
                Dim strReason As String
                objCustDLL = CreateObject(strCustomerDLL)
                ' If this function returns -2 it implies that there is no Special price for this customer
                ' If this function returns -1 it implies that this Product is not for sale to this Customer
                curTemp = objCustDLL.GetCustomerSpecialPrice(Session("_VT_DBConn"), udtProduct.lngProductId, udtCustomer.lngCustomerId, System.DateTime.UtcNow.Date)
            Else    ' use the standard special prices routine
                If objDataAccess.GetCustomerSpecialPrice(udtProduct.lngProductId, udtCustomer.lngCustomerId) <> "" Then
                    curSpecialPrice = objDataAccess.GetCustomerSpecialPrice(udtProduct.lngProductId, udtCustomer.lngCustomerId)
                    If curSpecialPrice > 0 Then
                        curTemp = curSpecialPrice
                    End If
                End If
            End If

        End If

        If curTemp < 0 Then ' revert to the standard price
            calculateCustomerDiscount = ""
        Else
            calculateCustomerDiscount = FormatCurrency(curTemp, 2)
        End If


    End Function

    Private Sub txtProductCode_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtProductCode.TextChanged
        If txtProductCode.Text <> "" Then
            FindProductCode()
        End If

    End Sub

    Private Sub cboProduct_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboProduct.SelectedIndexChanged
        'fill the product code text box for the selected product
        If cboProduct.SelectedValue = "" Then Exit Sub

        Dim ds As New DataSet
        Dim objDataAccess As New VTDBFunctions.VTDBFunctions.ProductsFunctions
        Dim strName As String


        'if ShowCodeWithName has been chosen then the combo will contain Code : ProductName
        ' so we must extract the product name to search for the code

        If Session("_VT_ShowProductCodewithName") = True Then
            strName = Mid(cboProduct.SelectedValue, InStr(cboProduct.SelectedValue, cSEPARATOR) + Len(cSEPARATOR))
        Else
            strName = cboProduct.SelectedValue
        End If

        ds = objDataAccess.GetProductForName(strName)
        Session("_VT_ProductId") = objDataAccess.GetProductIdForName(strName)

        Dim dblQtyInStock, dblAllocated, dblFreeStock As Double
        Dim intDecimalPlaces As Integer

        Dim objCommonFuncs As New BPADotNetCommonFunctions.VT_CommonFunctions.CommonFunctions
       

        Dim strTempDecimal As String = objCommonFuncs.GetConfigItem("TelesalesDecimalPlaces")
        If strTempDecimal = "" OrElse IsNumeric(strTempDecimal) = False Then
            intDecimalPlaces = 4
        Else
            If CInt(strTempDecimal) < 3 Then
                intDecimalPlaces = 2
            Else
                intDecimalPlaces = CInt(strTempDecimal)
            End If
        End If

        If ds.Tables(0).Rows.Count > 0 Then
            ' Check if there is any installation specific reason why we cannot sell this product
            ' to this customer today
            ' if a special support DLL is defined for this installation then try to get a special price from it
            Dim strCustomerDLL As String = System.Configuration.ConfigurationSettings.AppSettings("InstallationDLL")
            If strCustomerDLL <> "" Then
                Dim objCustDLL As TTIInstallationSpecific.Interface
                Dim strCanWe, strReason As String
                objCustDLL = CreateObject(strCustomerDLL)
                ' If this function doesn't return YES it implies that this Product is not for sale to this Customer
                strCanWe = objCustDLL.CanProductBeSoldToCustomer(Session("_VT_DBConn"), ds.Tables(0).Rows(0).Item("ProductId"), Session("_VT_CurrentCustomerId"), System.DateTime.UtcNow.Date, strReason)

                If strCanWe <> "YES" Then
                    Dim objDisp As New VT_Display.DisplayFuncs
                    objDisp.DisplayMessage(Page, "The selected product is not currently available for sale. " + strReason + ".")
                    Exit Sub
                End If
            End If

            ' display the Product quantities in the Grid
            ' Free Stock
            If IsDBNull(ds.Tables(0).Rows(0).Item("InStock")) Then
                dblQtyInStock = 0
            Else
                dblQtyInStock = CDbl(Trim(ds.Tables(0).Rows(0).Item("InStock")))
                dblQtyInStock = Math.Round(dblQtyInStock, intDecimalPlaces)
            End If
            grdProduct.Rows(0).Cells(0).Text = CStr(dblQtyInStock)

            If IsDBNull(ds.Tables(0).Rows(0).Item("Allocated")) Then
                dblAllocated = 0
            Else
                dblAllocated = CDbl(Trim(ds.Tables(0).Rows(0).Item("Allocated")))
                dblAllocated = Math.Round(dblAllocated, intDecimalPlaces)
            End If
            grdProduct.Rows(0).Cells(1).Text = CStr(dblAllocated)

            dblFreeStock = (dblQtyInStock - dblAllocated)
            dblFreeStock = Math.Round(dblFreeStock, intDecimalPlaces)

            grdProduct.Rows(0).Cells(2).Text = CStr(dblFreeStock)

            'now edit the label that says whether it is by qty or by weight
            If objDataAccess.GetUnitOfSale(Session("_VT_ProductId")) = 1 Then
                Session("UnitOfSale") = 1
                lblQtyOrWeight.Text = "This product is managed by Quantity"
            Else
                Session("UnitOfSale") = 0
                lblQtyOrWeight.Text = "This product is managed by Weight"
            End If

            'make the label visible
            lblQtyOrWeight.Visible = True

            ' put the UnitOfSale value in a hidden text box from where the Javascript can access it
            hdnUnitOfSale.Value = Session("UnitOfSale")

            If IsDBNull(ds.Tables(0).Rows(0).Item("usesTraceCode")) = False And ds.Tables(0).Rows(0).Item("UsesTraceCode") = True Then
                lblUsesTrace.Visible = True

                'make the trace code and date rec cols visible 
                With grdTraceCodes
                    .Columns(0).Hidden = False
                    .Columns(1).Hidden = False
                End With
            Else
                lblUsesTrace.Visible = False

                'hide the trace code and date rec cols
                With grdTraceCodes
                    .Columns(0).Hidden = True
                End With
            End If

            'set up the trace code grid for the selected product
            'FillTraceCodeGrid(ds.Tables(0).Rows(0).Item("ProductId"))
            FillTraceCodeGridNew(ds.Tables(0).Rows(0).Item("ProductId"))

        End If


        'if ShowCodeWithName has been chosen then the combo will contain Code : ProductName
        ' so we must extract the product name to search for the code

        If Session("_VT_ShowProductCodewithName") = True Then
            strName = Mid(cboProduct.SelectedValue, InStr(cboProduct.SelectedValue, cSEPARATOR) + Len(cSEPARATOR))
        Else
            strName = cboProduct.SelectedValue
        End If

        ds = objDataAccess.GetProductForName(strName)
        If ds.Tables(0).Rows.Count > 0 Then
            txtProductCode.Text = Trim(IIf(IsDBNull(ds.Tables(0).Rows(0).Item("Catalog_Number")), "", ds.Tables(0).Rows(0).Item("Catalog_Number")))

        End If

    End Sub

    Private Sub ClearTraceCodeGrid()
        grdTraceCodes.Rows.Clear()

        '       Key = "TraceCode"
        ' Key="InStock"
        ' Key="Qty"
        ' Key="Wgt" 
        ' Key="UnitPrice" 
        ' Key="Location" 
        'Key="NetPrice" 
        ' Key="VAT" 
        '       Key = "TotalPrice"
        'Key="SerialNum" 
        ' Key="Barcode" 
        'Key="UseByOrSellBy"
        'Key="DateRec"


        If lblUsesTrace.Visible = True And (Session("_VT_BatchMode") = True Or (Session("_VT_BatchMode") = False And chkShowBatches.Checked = True)) Then
            With grdTraceCodes
                .Columns.FromKey("TraceCode").Hidden = False
                .Columns.FromKey("InStock").Hidden = False
                .Columns.FromKey("Location").Hidden = False
                .Columns.FromKey("SerialNum").Hidden = False
                .Columns.FromKey("Barcode").Hidden = False
                .Columns.FromKey("UseByOrSellBy").Hidden = False
                .Columns.FromKey("DateRec").Hidden = False
            End With
        Else
            With grdTraceCodes
                .Columns.FromKey("TraceCode").Hidden = True
                .Columns.FromKey("InStock").Hidden = True
                .Columns.FromKey("Location").Hidden = True
                .Columns.FromKey("SerialNum").Hidden = True
                .Columns.FromKey("Barcode").Hidden = True
                .Columns.FromKey("UseByOrSellBy").Hidden = True
                .Columns.FromKey("DateRec").Hidden = True
            End With
        End If
    End Sub
    Private Sub grdTraceCodes_UpdateCell(ByVal sender As System.Object, ByVal e As Infragistics.WebUI.UltraWebGrid.CellEventArgs)
        Dim dblQty As Double
        Dim dblNetPrice As Double
        Dim dblTotal As Double
        Dim intCurrentRow As Integer
        Dim intCurrentCol As Integer
        Dim dblVat As Double
        Dim dblVatRate As Double
        Dim dblUnitPrice As Double
        Dim dblWeight As Double
        Dim cell As New Infragistics.WebUI.UltraWebGrid.UltraGridCell

        'need to recalculate the total price if qty or price were changed
        If e.Cell.Column.Key = "Qty" And Session("UnitOfSale") = 1 Then ' changing qty
            If IsNumeric(e.Cell.Value) Then
                dblQty = CDbl(e.Cell.Value)

            End If

            cell = e.Cell.Row.Cells.FromKey("UnitPrice")
            If IsNumeric(cell.Value) Then
                dblUnitPrice = CDbl(cell.Value)
            End If


            dblNetPrice = dblQty * dblUnitPrice

            cell = e.Cell.Row.Cells.FromKey("NetPrice")

            cell.Value = FormatCurrency(dblNetPrice, 2)

        ElseIf e.Cell.Column.Key = "Wgt" And Session("UnitOfSale") = 0 Then 'changing weight
            If IsNumeric(e.Cell.Value) Then
                dblWeight = CDbl(e.Cell.Value)
            End If

            cell = e.Cell.Row.Cells.FromKey("UnitPrice")
            If IsNumeric(cell.Value) Then
                dblUnitPrice = CDbl(cell.Value)
            End If

            dblNetPrice = dblWeight * dblUnitPrice

            cell = e.Cell.Row.Cells.FromKey("NetPrice")

            cell.Value = FormatCurrency(dblNetPrice, 2)
        ElseIf e.Cell.Column.Key = "UnitPrice" Then ' changing unit price
            If IsNumeric(e.Cell.Value) Then
                dblUnitPrice = CDbl(e.Cell.Value)
            End If

            ' replace the currency symbol if it is missing
            cell.Value = FormatCurrency(dblUnitPrice, 2)

            If Session("UnitOfSale") = 1 Then 'by unit
                cell = e.Cell.Row.Cells.FromKey("Qty")

            Else
                cell = e.Cell.Row.Cells.FromKey("Wgt")
            End If

            If IsNumeric(cell.Value) Then
                dblQty = CDbl(cell.Value)
            End If

            dblNetPrice = dblQty * dblUnitPrice
            cell = e.Cell.Row.Cells.FromKey("NetPrice")

            cell.Value = FormatCurrency(dblNetPrice, 2)

        ElseIf e.Cell.Column.Key = "Qty" And Session("UnitOfSale") = 0 Then 'qty is being changed for a by weight product
            Exit Sub
        ElseIf e.Cell.Column.Key = "Wgt" And Session("UnitOfSale") = 1 Then 'weight is being changed for a qty product
            Exit Sub
        End If

        cell = e.Cell.Row.Cells.FromKey("VAT") 'vat rate
        If IsNumeric(cell.Value) Then
            dblVatRate = CDbl(cell.Value)
        End If

        'MSHFlexGridBatch.Col = 8 'vat
        dblVat = dblNetPrice * dblVatRate / 100
        'MSHFlexGridBatch.Text = FormatCurrency(dblVat, 2)

        dblTotal = dblNetPrice + dblVat
        cell = e.Cell.Row.Cells.FromKey("TotalPrice") 'total
        cell.Value = FormatCurrency(dblTotal, 2)

    End Sub

    Private Sub FindProductCode()
        'if a product code has been entered then fill the product combo with the associated product
        Dim dsProduct As New DataSet
        Dim dsProductLine As New DataSet
        Dim objDataAccess As New VTDBFunctions.VTDBFunctions.ProductsFunctions
        Dim lngProductId As Long
        Dim lngProductLine As Long
        Dim sender As Object
        Dim e As System.EventArgs

        'if no code was entered dont do anything
        If txtProductCode.Text = "" Then Exit Sub

        dsProduct = objDataAccess.GetProductForCode(txtProductCode.Text)

        If dsProduct.Tables(0).Rows.Count = 0 Then
            Dim objDisp As New VT_Display.DisplayFuncs
            objDisp.DisplayMessage(Page, "This Product Code does not exist")
            txtProductCode.Text = ""
            Exit Sub
        Else
            ' find the correct product line
            lngProductLine = dsProduct.Tables(0).Rows(0).Item("Product_LineID")
            dsProductLine = objDataAccess.GetProductLines(lngProductLine)
            cboProdCategory.SelectedValue = dsProductLine.Tables(0).Rows(0).Item("Product_Line_Text")

            cboProdCategory_SelectedIndexChanged(sender, e)

            'if ShowCodeWithName has been chosen then display the product in the product combo with the code.
            If Session("_VT_ShowProductCodewithName") = True Then
                cboProduct.SelectedValue = Trim(IIf(IsDBNull(dsProduct.Tables(0).Rows(0).Item("Catalog_Number")), "", dsProduct.Tables(0).Rows(0).Item("Catalog_Number"))) & cSEPARATOR & Trim(dsProduct.Tables(0).Rows(0).Item("Product_Name"))
            Else
                cboProduct.SelectedValue = Trim(dsProduct.Tables(0).Rows(0).Item("Product_Name"))
            End If

            cboProduct_SelectedIndexChanged(sender, e)
            'lngProductId = dsProduct.Tables(0).Rows(0).Item("ProductId")
            ''set up the trace code grid for the selected product
            'FillTraceCodeGrid(lngProductId)

        End If


    End Sub




    Private Sub grdTraceCodes_UpdateCell1(ByVal sender As Object, ByVal e As Infragistics.WebUI.UltraWebGrid.CellEventArgs) Handles grdTraceCodes.UpdateCell
        ' find the unit of Sale and then calculate the price
        Dim cell As New Infragistics.WebUI.UltraWebGrid.UltraGridCell
        Dim row As New Infragistics.WebUI.UltraWebGrid.UltraGridRow
        Dim dblValue As Double
        Dim blnUpdatePrice As Boolean

        cell = e.Cell
        row = cell.Row


        Dim lngProductId As Long = CLng(Session("_VT_ProductId"))
        Dim objProducts As New VTDBFunctions.VTDBFunctions.ProductsFunctions

        Dim dblWgtPerUnit As Double
        Dim dblCalcWgt As Double
        Dim blnCalcQtyFromWgt As Boolean
        Dim dblCalcQty As Double

        blnCalcQtyFromWgt = CBool(Session("_VT_CalcQtyFromWgt"))

        If blnCalcQtyFromWgt = True And ((e.Cell.Column.Key = "Qty") Or (e.Cell.Column.Key = "Wgt")) Then
            If (Not IsDBNull(e.Cell.Value)) And e.Cell.Value <> "" Then
                dblWgtPerUnit = objProducts.GetProductAvgWeightPerUnit(lngProductId)

                If e.Cell.Column.Key = "Qty" And dblWgtPerUnit <> 0 Then
                    blnCalcQtyFromWgt = False
                    dblCalcWgt = CDbl(e.Cell.Value) * dblWgtPerUnit
                    row.Cells.FromKey("Wgt").Value = Math.Round(dblCalcWgt, 4)
                    blnCalcQtyFromWgt = True

                ElseIf e.Cell.Column.Key = "Wgt" And dblWgtPerUnit <> 0 Then
                    blnCalcQtyFromWgt = False
                    dblCalcQty = CDbl(e.Cell.Value) / dblWgtPerUnit
                    row.Cells.FromKey("Qty").Value = Math.Round(dblCalcQty, 0)
                    blnCalcQtyFromWgt = True

                End If
            End If
        End If

        Session("_VT_CalcQtyFromWgt") = blnCalcQtyFromWgt

        blnUpdatePrice = False
        If cell.Key = "Qty" Or cell.Key = "Wgt" Or cell.Key = "UnitPrice" Then
            If Session("UnitOfSale") = 1 Then    ' by Qty product
                If cell.Key = "Qty" Then
                    dblValue = cell.Value
                    blnUpdatePrice = True
                End If
            Else
                If cell.Key = "Wgt" Then
                    dblValue = cell.Value
                    blnUpdatePrice = True
                End If
            End If

            If cell.Key = "UnitPrice" Then
                cell.Value = FormatCurrency(cell.Value, 2)
                blnUpdatePrice = True
                If Session("UnitOfSale") = 1 Then    ' by Qty product
                    dblValue = row.Cells.FromKey("Qty").Value
                Else
                    dblValue = row.Cells.FromKey("Wgt").Value
                End If
            End If

            If blnUpdatePrice Then
                ' multiply the qty by the unit price to give the net price
                row.Cells.FromKey("NetPrice").Value = FormatCurrency(row.Cells.FromKey("UnitPrice").Value * dblValue, 2)
                row.Cells.FromKey("TotalPrice").Value = FormatCurrency(row.Cells.FromKey("NetPrice").Value + (row.Cells.FromKey("NetPrice").Value * row.Cells.FromKey("VAT").Value / 100), 2)
            End If

        End If
    End Sub
End Class
