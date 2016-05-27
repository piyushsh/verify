Imports BPADotNetCommonFunctions
Imports BPADotNetCommonFunctions.VT_CommonFunctions.MatrixFunctions
Imports System.Data.SqlClient
Imports System.Data
Imports System.Drawing
Imports System.Data.DataTableExtensions
Imports VTDBFunctions.VTDBFunctions


Partial Class TabPages_WarehouseTV
    Inherits MyBasePage


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'Add behavior 

        'If Not Page.IsPostBack Then

        Me.wdgRepGrid.Behaviors.CreateBehavior(Of Infragistics.Web.UI.GridControls.Paging)()

        'Handle PageIndexChanged event 
        ' Me.wdgRepGrid.Behaviors.Paging.PagingClientEvents.PageIndexChanged = "WebDataGrid1_PageIndexChanged"


        Me.wdgRepGrid.Behaviors.Paging.PagerTemplate = New CustomPagerTemplate()



        Dim objDataPreserve As New BPADotNetCommonFunctions.VT_DataPreserve.DataPreserve

        FillData()



        'SmcN 02/06/2014 Set the Grid width to the Client Browser width
        If Not Me.CurrentSession.VT_BrowserWindowWidth Is Nothing Then
            wdgRepGrid.Width = Me.CurrentSession.VT_BrowserWindowWidth - 54
        End If

    End Sub




    Sub FillData()
        Dim objG As New BPADotNetCommonFunctions.VT_ReportFunctions.GeneralFunctions
        Dim objDBF As New SalesOrdersFunctions.SalesOrders
        Dim objM As New VTDBFunctions.VTDBFunctions.VTMatrixFunctions
        Dim objSales As New BPADotNetCommonFunctions.SalesFunctions.Sales_Orders

        Dim objDataPreserve As New BPADotNetCommonFunctions.VT_DataPreserve.DataPreserve
        Dim dtoutput As New DataTable
        Dim strStatusSearch As String = ""

        Dim strConn As String = Session("_VT_DotNetConnString")

        Dim objutil As New VTDBFunctions.VTDBFunctions.UtilFunctions

        Dim objMFG As New SteripackMFGProInterface.MFGProFunctions
        Dim strMFGProConnString As String = System.Configuration.ConfigurationManager.AppSettings("MFGPro_DB_CONN")
        Dim strMFGProDomain As String = System.Configuration.ConfigurationManager.AppSettings("MFGPro_Domain")
        Dim strMFGProError As String = ""


        'Get the customers from MFG Pro that match this Product
        Dim strSQL As String

        Dim strStartDate As String
        Dim strEndDate As String
        Try


            strStartDate = Session("WarehouseTVSTartDate")
            strEndDate = Session("WarehouseTVEndDate")

            strSQL = String.Format("SELECT a.sod_nbr, b.so_cust, b.so_nbr, a.sod_line, a.sod_part, a.sod_qty_ord, a.sod_um, a.sod_due_date, a.sod_qty_ship, a.sod_qty_inv, c.pt_prod_line FROM PUB.sod_det a, PUB.so_mstr b, PUB.pt_mstr c  WHERE (b.so_domain = a.sod_domain) AND (b.so_nbr = a.sod_nbr) AND (a.sod_due_date >='" & strStartDate & "') AND (a.sod_due_date <='" & strEndDate & "') AND (a.sod_part = c.pt_part) AND (c.pt_prod_line <> '1800')AND(c.pt_prod_line<>'7000')AND(c.pt_prod_line<>'7550')AND(c.pt_prod_line<>'FREI') order by a.sod_due_date asc")

            Session("MfgProLive") = "NO"


            Dim dtSO As New DataTable
            Dim blnTryAgain As Boolean = True

            Do While blnTryAgain = True
                Try
                    dtSO = objMFG.GetDataTableFromMFGPro(strMFGProConnString, strSQL, strMFGProError)
                    blnTryAgain = False
                Catch ex As Exception
                    'failed so try again
                    blnTryAgain = True
                    objutil.LogAction("ERROR getting SO table from Mfg pro. Error is: " & ex.Message, "WarehouseTVLog")

                End Try
            Loop



            strSQL = String.Format("SELECT a.ld_part as PartNum, sum(a.ld_qty_oh) as QtyOnhand, a.ld_status as IdStatus FROM PUB.ld_det a WHERE a.ld_qty_oh > 0 AND a.ld_status = 'a' group by a.ld_part, a.ld_status")


            Dim dtStock As New DataTable
            blnTryAgain = True
            Do While blnTryAgain = True
                Try
                    dtStock = objMFG.GetDataTableFromMFGPro(strMFGProConnString, strSQL, strMFGProError)
                    blnTryAgain = False
                Catch ex As Exception
                    'failed so try again
                    blnTryAgain = True
                    objutil.LogAction("ERROR getting dtStock table from Mfg pro. Error is: " & ex.Message, "WarehouseTVLog")
                End Try
            Loop

            Dim adrStockThisProduct() As DataRow

            Dim dtRes As New DataTable
            'set up the dtres table
            dtRes.Columns.Add("ProdLine")
            dtRes.Columns.Add("Customer")
            dtRes.Columns.Add("SONum")
            dtRes.Columns.Add("LineK")
            dtRes.Columns.Add("ItemNum")
            dtRes.Columns.Add("DueDate")
            dtRes.Columns.Add("QtyOrdered")
            dtRes.Columns.Add("UOM")
            dtRes.Columns.Add("QtyOnHand")
            dtRes.Columns.Add("QtyShipped")
            dtRes.Columns.Add("QtyToInvoice")
            dtRes.Columns.Add("OpenSoQty")
            dtRes.Columns.Add("Colour")
            dtRes.Columns.Add("AwaitProduction")
            dtRes.Columns.Add("AwaitPicking")
            dtRes.Columns.Add("AwaitInvoicing")
            dtRes.Columns.Add("PercentReadyToShip")
            Dim adate As Date = PortalFunctions.Now

            Dim i As Integer

            Dim objcust As New VTDBFunctions.VTDBFunctions.CustomerFunctions
            Dim intcustid As Integer


            'artificially fill the grid for testing
            '' ''For i = 0 To 20 'rows
            '' ''    Session("StartCounter") = Session("StartCounter") + 1

            '' ''    dtRes.Rows.Add()
            '' ''    With dtRes.Rows(i)

            '' ''        .Item("ProdLine") = 1

            '' ''        .Item("Customer") = "Stryker"


            '' ''        .Item("SONum") = "SO" & Session("StartCounter")  '("a.sod_nbr")
            '' ''        .Item("Linek") = 1
            '' ''        .Item("ItemNum") = "23455" '("a.sod_part")
            '' ''        .Item("DueDate") = Format(adate, "dddd, dd/MM/yyyy") '("a.sod_due_date")
            '' ''        .Item("QtyOrdered") = 50 '"a.sod_qty_ord"
            '' ''        .Item("UOM") = "each"


            '' ''        .Item("QtyOnHand") = 0


            '' ''        .Item("QtyShipped") = 50 '"a.sod_qty_ship"

            '' ''        .Item("QtyToInvoice") = 50 '"a.sod_qty_inv"

            '' ''        .Item("OpenSoQty") = .Item("QtyOrdered") - .Item("QtyShipped")
            '' ''        .Item("PercentReadyToShip") = "%"
            '' ''        If i < 5 Then
            '' ''            .Item("Colour") = "RED"
            '' ''        ElseIf i < 15 Then
            '' ''            .Item("Colour") = "AMBER"
            '' ''        Else
            '' ''            .Item("Colour") = "RED"
            '' ''        End If

            '' ''    End With


            '' ''Next
            Dim dblQtyOrdered As Double
            Dim dblQtyShipped As Double
            Dim dblQtyOnHand As Double
            Dim strtemp As String

            For i = 0 To dtSO.Rows.Count - 1
                dtRes.Rows.Add()
                With dtRes.Rows(i)

                    .Item("ProdLine") = dtSO.Rows(i).Item(10)
                    blnTryAgain = True
                    Do While blnTryAgain = True
                        Try
                            intcustid = objcust.GetCustomerIdForRef(dtSO.Rows(i).Item(1))
                            If intcustid > 0 Then
                                .Item("Customer") = objcust.GetCustomerNameForId(intcustid)
                            Else
                                .Item("Customer") = dtSO.Rows(i).Item(1)
                            End If
                            blnTryAgain = False
                        Catch ex As Exception
                            'failed so try again
                            blnTryAgain = True
                            objutil.LogAction("ERROR getting customer data from verify db. Error is: " & ex.Message, "WarehouseTVLog")
                        End Try
                    Loop

                    .Item("SONum") = dtSO.Rows(i).Item(0) '("a.sod_nbr")
                    .Item("Linek") = dtSO.Rows(i).Item(3)
                    .Item("ItemNum") = dtSO.Rows(i).Item(4) '("a.sod_part")
                    .Item("DueDate") = Format(dtSO.Rows(i).Item(7), "dddd, dd/MM/yyyy") '("a.sod_due_date")

                    dblQtyOrdered = IIf(IsNumeric(dtSO.Rows(i).Item(5)), dtSO.Rows(i).Item(5), 0)
                    If dblQtyOrdered > 0 Then
                        '  .Item("QtyOrdered") = Format(Math.Round(dblQtyOrdered, 0), "#,###,###") '"a.sod_qty_ord"
                        strtemp = Format(Math.Round(dblQtyOrdered, 0), "#,###,###")
                        .Item("QtyOrdered") = If(strtemp = "", 0, CDbl(strtemp))
                    Else
                        .Item("QtyOrdered") = 0
                    End If

                    .Item("UOM") = dtSO.Rows(i).Item(6) '("a.sod_um")

                    adrStockThisProduct = dtStock.Select(" PartNum = '" & .Item("ItemNum") & "'")
                    If adrStockThisProduct.Length > 0 Then
                        dblQtyOnHand = IIf(IsNumeric(adrStockThisProduct(0)(1)), adrStockThisProduct(0)(1), 0)
                        If dblQtyOnHand > 0 Then
                            ' .Item("QtyOnHand") = Format(Math.Round(dblQtyOnHand, 0), "#,###,###")
                            strtemp = Format(Math.Round(dblQtyOnHand, 0), "#,###,###")
                            .Item("QtyOnHand") = If(strtemp = "", 0, CDbl(strtemp))
                        Else
                            .Item("QtyOnHand") = 0
                        End If

                    Else
                        dblQtyOnHand = 0
                        .Item("QtyOnHand") = 0
                    End If
                    dblQtyShipped = IIf(IsNumeric(dtSO.Rows(i).Item(8)), dtSO.Rows(i).Item(8), 0)
                    If dblQtyShipped > 0 Then
                        ' .Item("QtyShipped") = Format(Math.Round(dblQtyShipped, 0), "#,###,###") '"a.sod_qty_ship"
                        strtemp = Format(Math.Round(dblQtyShipped, 0), "#,###,###")
                        .Item("QtyShipped") = If(strtemp = "", 0, CDbl(strtemp))
                    Else
                        .Item("QtyShipped") = 0
                    End If

                    .Item("QtyToInvoice") = Format(Math.Round(IIf(IsNumeric(dtSO.Rows(i).Item(9)), dtSO.Rows(i).Item(9), 0), 0), "#,###,###") '"a.sod_qty_inv"

                    .Item("OpenSoQty") = Format(dblQtyOrdered - dblQtyShipped, "#,###,###")
                    .Item("PercentReadyToShip") = CStr(Math.Round((dblQtyOnHand / dblQtyOrdered) * 100, 0)) & "%"
                End With

            Next


            If strMFGProError <> "" Then
                'No data came back form MFGPro 
                InitialiseAndBindEmptyOrderItemsGrid()



            Else
                If Session("Accounts") = "MFGPRO" Then
                    Session("MfgProLive") = "YES"

                End If



                Dim dblTotalOutstanding As Double
                Dim aRow As DataRow
                Dim dtgrid As New DataTable

                dtgrid.Columns.Add("ProdLine")
                dtgrid.Columns.Add("Customer")
                dtgrid.Columns.Add("SONum")
                dtgrid.Columns.Add("LineK")
                dtgrid.Columns.Add("ItemNum")
                dtgrid.Columns.Add("DueDate")
                dtgrid.Columns.Add("QtyOrdered")
                dtgrid.Columns.Add("UOM")
                dtgrid.Columns.Add("QtyOnHand")
                dtgrid.Columns.Add("QtyShipped")
                dtgrid.Columns.Add("QtyToInvoice")
                dtgrid.Columns.Add("OpenSoQty")
                dtgrid.Columns.Add("Colour")
                dtgrid.Columns.Add("AwaitProduction")
                dtgrid.Columns.Add("AwaitPicking")
                dtgrid.Columns.Add("AwaitInvoicing")
                dtgrid.Columns.Add("PercentReadyToShip")

                'For i = 0 To 100
                '    dtgrid.Rows.Add()

                '    With dtgrid.Rows(i)
                '        .Item("SONum") = i

                '        .Item("Colour") = "RED"
                '    End With
                'Next
                '    'Apply Grid Formatting here
                Dim dblOpenQty As Double

                For intLoop As Integer = 0 To dtRes.Rows.Count - 1
                    adrStockThisProduct = dtRes.Select(" ItemNum ='" & dtRes.Rows(intLoop).Item("ItemNum") & "'")
                    dblTotalOutstanding = 0
                    For Each aRow In adrStockThisProduct
                        dblOpenQty = IIf(IsNumeric(aRow("OpenSoQty")), aRow("OpenSoQty"), 0)
                        dblTotalOutstanding = dblTotalOutstanding + dblOpenQty
                    Next
                    dblQtyOnHand = IIf(IsNumeric(dtRes.Rows(intLoop).Item("QtyOnHand")), dtRes.Rows(intLoop).Item("QtyOnHand"), 0)
                    dblOpenQty = IIf(IsNumeric(dtRes.Rows(intLoop).Item("OpenSoQty")), dtRes.Rows(intLoop).Item("OpenSoQty"), 0)

                    If dblTotalOutstanding > dblQtyOnHand Then
                        dtRes.Rows(intLoop).Item("Colour") = "RED"
                        dtgrid.ImportRow(dtRes.Rows(intLoop))
                        'wdgRepGrid.Rows(intLoop).CssClass = "Row_Salmon"
                    ElseIf dblOpenQty > 0 Then
                        ' wdgRepGrid.Rows(intLoop).CssClass = "Row_Yellow"
                        dtRes.Rows(intLoop).Item("Colour") = "AMBER"
                        dtgrid.ImportRow(dtRes.Rows(intLoop))
                    Else
                        'if the tickbox to show red and amber only is checked then hide the rest of the rows
                        If Session("_VT_ShowRedAndAmberOnly") <> "TRUE" Then

                            dtRes.Rows(intLoop).Item("Colour") = "NONE"
                            dtgrid.ImportRow(dtRes.Rows(intLoop))
                        End If
                    End If

                Next

                objDataPreserve.BindDataToWDG(dtgrid, wdgRepGrid)

                'now apply the colour coding
                'Apply Grid Formatting here
                For intLoop As Integer = 0 To wdgRepGrid.Rows.Count - 1
                    Select Case dtgrid.Rows(intLoop).Item("Colour")

                        Case "RED"
                            'wdgRepGrid.Rows(intLoop).CssClass = "Row_Salmon"
                            wdgRepGrid.Rows(intLoop).Items.FindItemByKey("AwaitProduction").CssClass = "CellColor_Red"
                        Case "AMBER"
                            ' wdgRepGrid.Rows(intLoop).CssClass = "Row_Yellow"
                            wdgRepGrid.Rows(intLoop).Items.FindItemByKey("AwaitPicking").CssClass = "CellColor_Yellow"
                        Case "NONE"
                            wdgRepGrid.Rows(intLoop).Items.FindItemByKey("AwaitInvoicing").CssClass = "CellColor_Green"
                    End Select

                Next
            End If


            '  objutil.LogAction("Filldata function called on warehousetv", "WarehouseTVLog")
        Catch ex As Exception
            objutil.LogAction("General error in FillData function. Error is:" & ex.Message, "WarehouseTVLog")
        End Try
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
        objDataPreserve.BindDataToWDG(dtItemsForGrid, wdgOrderItems)


    End Sub



    ''' <summary>
    ''' This function validates the data on the form before it can be saved
    ''' </summary>
    ''' <returns>If there is a Validation problem return a message, Empty String otherwise</returns>
    ''' <remarks></remarks>
    Public Function ValidateMe() As String

    End Function

    ''' <summary>
    '''This function returns whether we should save the current page or not. The Default is to save
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function DataToSave() As Boolean

        DataToSave = True

    End Function



    Protected Sub btnBack1_Click(sender As Object, e As ImageClickEventArgs) Handles btnBack1.Click, btnBack2.Click

        Response.Redirect("~/TabPages/WarehouseManager.aspx")

    End Sub




    Public Function PortalRootNode() As String
        ' retrieve the Root node of the current portal
        ' we will use it to create a consistent path to the image files
        Dim strCompletePath As String = Server.MapPath("~")

        'ServerRootPath = strCompletePath
        PortalRootNode = Mid(strCompletePath, InStrRev(strCompletePath, "\") + 1)

    End Function



    Private Class CustomPagerTemplate
        Implements ITemplate
#Region "ITemplate Members"

        Public Sub InstantiateIn(ByVal container As Control) Implements ITemplate.InstantiateIn
            'Dim prevButton As New HtmlButton()
            'prevButton.ID = "PrevButton"
            'prevButton.InnerText = "Prev"
            'prevButton.Attributes("onclick") = "return PreviousPage()"


            Dim nextButton As New HtmlButton()
            nextButton.InnerText = "Next"
            nextButton.Attributes("onclick") = "return NextPage()"

            'container.Controls.Add(prevButton)
            'container.Controls.Add(ddl)
            container.Controls.Add(nextButton)
        End Sub

#End Region
    End Class



    Protected Sub wdgRepGrid_PageIndexChanged(sender As Object, e As Infragistics.Web.UI.GridControls.PagingEventArgs) Handles wdgRepGrid.PageIndexChanged
        'add colour coding to the next page
        Dim i As Integer
        For i = 0 To wdgRepGrid.Rows.Count - 1
            Select Case wdgRepGrid.Rows(i).Items.FindItemByKey("Colour").Text
                Case "RED"
                    wdgRepGrid.Rows(i).Items.FindItemByKey("AwaitProduction").CssClass = "CellColor_Red"
                Case "AMBER"
                    wdgRepGrid.Rows(i).Items.FindItemByKey("AwaitPicking").CssClass = "CellColor_Yellow"
                Case Else
                    wdgRepGrid.Rows(i).Items.FindItemByKey("AwaitInvoicing").CssClass = "CellColor_Green"
            End Select
        Next

        'Dim objutil As New VTDBFunctions.VTDBFunctions.UtilFunctions
        'objutil.LogAction("TV page down function called on warehousetv", "WarehouseTVLog")

    End Sub
End Class



