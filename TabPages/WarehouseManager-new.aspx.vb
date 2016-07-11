Imports BPADotNetCommonFunctions
Imports BPADotNetCommonFunctions.VT_CommonFunctions.MatrixFunctions
Imports System.Data.SqlClient
Imports System.Data
Imports System.Drawing
Imports System.Data.DataTableExtensions
Imports VTDBFunctions.VTDBFunctions


Partial Class TabPages_WarehouseManager
    Inherits MyBasePage


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim objCommonfuncs As New BPADotNetCommonFunctions.VT_CommonFunctions.CommonFunctions
        Dim objDataPreserve As New BPADotNetCommonFunctions.VT_DataPreserve.DataPreserve
        If Not IsPostBack Then
            If Session("Accounts") = "MFGPRO" Then
                btnWarehouseTV.Visible = True
                chkShowRedandAmberOnly.Visible = True
            Else
                btnWarehouseTV.Visible = False
                chkShowRedandAmberOnly.Visible = False
            End If

            Dim objC As New VT_CommonFunctions.CommonFunctions
            Dim objD As New BPADotNetCommonFunctions.VT_DataPreserve.DataPreserve

            Dim dtBlankTemp As New DataTable
            'dtBlankTemp = objC.GetDataTableStructureForWebDataGrid(wdgOrderItems)
            'objD.BindDataToWDG(dtBlankTemp, wdgOrderItems)

            'dtBlankTemp = objC.GetDataTableStructureForWebDataGrid(wdgOrders)
            'objD.BindDataToWDG(dtBlankTemp, wdgOrders)

            dtBlankTemp = objC.GetDataTableStructureForWebDataGrid(wdgRepGrid)

            '****  Add demo rows to the grid

            ' Define elements and values for Row 1
            Dim drRow1 As DataRow = dtBlankTemp.NewRow

            drRow1.Item("Customer") = "Mac's Fresh Bakery"
            drRow1.Item("SONum") = "10101"
            drRow1.Item("ProdLine") = "Cakes"
            drRow1.Item("LineK") = "C1"
            drRow1.Item("ProductName") = "Wiskey Cake"
            drRow1.Item("ItemNum") = "1"
            drRow1.Item("DueDate") = "24/06/2016"
            drRow1.Item("QtyOrdered") = "800"
            drRow1.Item("WgtOrdered") = "120"
            drRow1.Item("UOM") = "Qty"
            drRow1.Item("QtyOnHand") = "1500"
            drRow1.Item("Tracecode") = "AB3456"
            drRow1.Item("QtyShipped") = "700"
            drRow1.Item("WgtShipped") = "100"
            drRow1.Item("DocketNum") = "124513062016"
            drRow1.Item("QtyToInvoice") = "700"
            drRow1.Item("OpenSOQty") = "100"
            drRow1.Item("AwaitPicking") = "100"
            drRow1.Item("AwaitInvoicing") = "700"
            drRow1.Item("PercentReadyToShip") = "85"
            drRow1.Item("JobId") = "3456"
            drRow1.Item("JobStatus") = "Part Shipped"
            drRow1.Item("PriceSoldFor") = "2014.89"
            drRow1.Item("RecordId") = " 2223"
            drRow1.Item("VatCharged") = "75.35"
            drRow1.Item("SerialNum") = "1234565"
            drRow1.Item("Barcode") = "89788"
            drRow1.Item("SaleType") = "Standard"
            drRow1.Item("InvoiceNum") = "19898"

            'Add Row 1 to the Datatable
            dtBlankTemp.Rows.Add(drRow1)

            ' Define elements and values for Row 2
            Dim drRow2 As DataRow = dtBlankTemp.NewRow

            drRow2.Item("Customer") = "Aldi Stores"
            drRow2.Item("SONum") = "10102"
            drRow2.Item("ProdLine") = "Buns"
            drRow2.Item("LineK") = "C2"
            drRow2.Item("ProductName") = "Muffins"
            drRow2.Item("ItemNum") = "2"
            drRow2.Item("DueDate") = "27/06/2016"
            drRow2.Item("QtyOrdered") = "200"
            drRow2.Item("WgtOrdered") = "100"
            drRow2.Item("UOM") = "Wgt"
            drRow2.Item("QtyOnHand") = "150"
            drRow2.Item("Tracecode") = "776577"
            drRow2.Item("QtyShipped") = "200"
            drRow2.Item("WgtShipped") = "100"
            drRow2.Item("DocketNum") = "234314062016"
            drRow2.Item("QtyToInvoice") = "200"
            drRow2.Item("OpenSOQty") = "0"
            drRow2.Item("AwaitPicking") = "0"
            drRow2.Item("AwaitInvoicing") = "200"
            drRow2.Item("PercentReadyToShip") = "100"
            drRow2.Item("JobId") = "2345"
            drRow2.Item("JobStatus") = "Complete"
            drRow2.Item("PriceSoldFor") = "950.00"
            drRow2.Item("RecordId") = " 1234"
            drRow2.Item("VatCharged") = "50.75"
            drRow2.Item("SerialNum") = "9987575"
            drRow2.Item("Barcode") = "44747"
            drRow2.Item("SaleType") = "Standard"
            drRow2.Item("InvoiceNum") = "19989"

            'Add Row 2 to the Datatable
            dtBlankTemp.Rows.Add(drRow2)

            ' Define elements and values for Row 3
            Dim drRow3 As DataRow = dtBlankTemp.NewRow

            drRow3.Item("Customer") = "Tesco Super Stores"
            drRow3.Item("SONum") = "79858"
            drRow3.Item("ProdLine") = "Pies"
            drRow3.Item("LineK") = "C3"
            drRow3.Item("ProductName") = "Apple Tarts"
            drRow3.Item("ItemNum") = "3"
            drRow3.Item("DueDate") = "28/06/2016"
            drRow3.Item("QtyOrdered") = "2000"
            drRow3.Item("WgtOrdered") = "1000"
            drRow3.Item("UOM") = "Qty"
            drRow3.Item("QtyOnHand") = "1500"
            drRow3.Item("Tracecode") = "45656BA"
            drRow3.Item("QtyShipped") = "1500"
            drRow3.Item("WgtShipped") = "1000"
            drRow3.Item("DocketNum") = "678314062016"
            drRow3.Item("QtyToInvoice") = "1500"
            drRow3.Item("OpenSOQty") = "500"
            drRow3.Item("AwaitPicking") = "500"
            drRow3.Item("AwaitInvoicing") = "1500"
            drRow3.Item("PercentReadyToShip") = "80"
            drRow3.Item("JobId") = "78987"
            drRow3.Item("JobStatus") = "Part shipped"
            drRow3.Item("PriceSoldFor") = "2234.00"
            drRow3.Item("RecordId") = " 3456"
            drRow3.Item("VatCharged") = "960.75"
            drRow3.Item("SerialNum") = "567575675"
            drRow3.Item("Barcode") = "87676"
            drRow3.Item("SaleType") = "Standard"
            drRow3.Item("InvoiceNum") = "19991"

            'Add Row 3 to the Datatable
            dtBlankTemp.Rows.Add(drRow3)

            'Bind the datatable to the grid
            objD.BindDataToWDG(dtBlankTemp, wdgRepGrid)

        Else
            wdgRepGrid.DataSource = objDataPreserve.GetWDGDataFromSession(wdgRepGrid)

        End If


    End Sub



    Sub InitialiseAndBindEmptyOrderItemsGrid()

        Dim dtItemsForGrid As New DataTable
        Dim dtServiceItems As New DataTable
        Dim objDataPreserve As New BPADotNetCommonFunctions.VT_DataPreserve.DataPreserve

        'There is no sales order yet so build a blank datatable and bind the grids to these
        dtItemsForGrid.Columns.Add("SONum")
        dtItemsForGrid.Columns.Add("ItemNum")
        dtItemsForGrid.Columns.Add("DueDate")
        dtItemsForGrid.Columns.Add("QtyOrdered")
        dtItemsForGrid.Columns.Add("UOM")
        dtItemsForGrid.Columns.Add("QtyOnHand")
        dtItemsForGrid.Columns.Add("QtyShipped")
        dtItemsForGrid.Columns.Add("QtyToInvoice")
        dtItemsForGrid.Columns.Add("OpenSoQty")



        'Bind the Grid
        objDataPreserve.BindDataToWDG(dtItemsForGrid, wdgRepGrid)


    End Sub


End Class



