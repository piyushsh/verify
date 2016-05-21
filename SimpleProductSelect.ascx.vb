Imports VTDBFunctions.VTDBFunctions.ProductsFunctions
Imports BPADotNetCommonFunctions
Imports System.Data

Public Class SimpleProductSelect
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



            cboProdCategory_SelectedIndexChanged(sender, e)

            ' load the Locations UDT
           
            'if a product is being preloaded then do it now
            Dim strname As String
            If (Not Session("VT_ProdSelectProdID") Is Nothing) And (Session("VT_ProdSelectProdID") > 0) Then
                If Session("_VT_ShowProductCodewithName") = True Then
                    strname = objdataAccess.GetProductCode(Session("VT_ProdSelectProdID")) & cSEPARATOR & objdataAccess.GetProductNameForId(Session("VT_ProdSelectProdID"))
                Else
                    strname = objdataAccess.GetProductNameForId(Session("VT_ProdSelectProdID"))

                End If
                Session("VT_ProdSelectProdID") = ""
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

        Dim dblQtyInStock, dblAllocated As Double

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

    
End Class
