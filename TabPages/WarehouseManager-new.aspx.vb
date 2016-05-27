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



