Imports System.Data.SqlClient
Imports System.Data
Imports System.Configuration
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Drawing
Imports System.Web.UI.HtmlControls
Imports AjaxControlToolkit
Imports BPADotNetCommonFunctions
Imports BPADotNetCommonFunctions.VT_CommonFunctions.MatrixFunctions
Imports VTDBFunctions.VTDBFunctions
Imports Infragistics.Documents.Reports.Report
Imports Infragistics.Documents.Reports.Report.Section
Imports Infragistics.Documents.Reports.Report.TOC
Imports Infragistics.Documents.Reports.Report.Index
Imports Infragistics.Documents.Reports.Report.Band
Imports Infragistics.Documents.Reports.Report.Flow
Imports Infragistics.Documents.Reports.Report.Table
Imports Infragistics.Documents.Reports.Report.Text
Imports System.IO


Partial Class BillOfLadingAliyasPDF

    Inherits MyBasePage


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim objC As New VTDBFunctions.VTDBFunctions.CustomerFunctions
        Dim strConn As String = Session("_VT_DotNetConnString")
        Dim strCleanedSoldToAddress As String = ""
        Dim strCleanedDeliveryAddress As String = ""
        Dim dsOrderItems As New DataSet
        Dim objTele As New TelesalesFunctions
        Dim objProd As New VTDBFunctions.VTDBFunctions.ProductsFunctions
        Dim objDataPreserve As New BPADotNetCommonFunctions.VT_DataPreserve.DataPreserve
        Dim strSoldToCustomerName As String = ""
        Dim strShipToCustomerName As String = ""
        Dim strSoldToCustomerPhone As String = ""
        Dim strShipToCustomerPhone As String = ""
        Dim strSoldToCustomerFax As String = ""
        Dim strShipToCustomerFax As String = ""
        Dim strPONum As String = ""
        Dim objR As New BPADotNetCommonFunctions.VT_ReportFunctions.TransactionDataFunctions
        Dim strWhereClause As String
        Dim dtRep As DataTable
        Dim intQuantity As Integer = 0
        Dim intTotalQuantity As Integer = 0
        Dim strDateOfTransaction As String

        'Get Dispatch Transactions

        strWhereClause = " WHERE trc_transactions.DocketNum = '" & Me.CurrentSession.VT_DocketNumber & "' "
        'strWhereClause = " WHERE trc_transactions.DocketNum = '20130423142642' "
        strWhereClause = strWhereClause + " AND (trc_Transactions.TransactionType = '3' OR trc_Transactions.TransactionType = '6') "
        strWhereClause = strWhereClause + " AND (trc_Transactions.PaymentType <> '2') "

        dtRep = objR.GetTransactionsForWhereClause(strWhereClause, strConn)

        'group and sum quantity by product (catalog number)
        Dim dtUniqueProductId As DataTable = dtRep.DefaultView.ToTable(True, "Catalog_Number")
        dtUniqueProductId.Columns.Add("ProductName")
        dtUniqueProductId.Columns.Add("Quantity", GetType(Integer))
        For intRowCount As Integer = 0 To dtUniqueProductId.Rows.Count - 1
            For intInnerRowCount As Integer = 0 To dtRep.Rows.Count - 1
                If dtRep.Rows(intInnerRowCount).Item("Catalog_Number") = dtUniqueProductId.Rows(intRowCount).Item("Catalog_Number") Then
                    intQuantity = intQuantity + CInt(dtRep.Rows(intInnerRowCount).Item("Quantity"))
                    intTotalQuantity = intTotalQuantity + CInt(dtRep.Rows(intInnerRowCount).Item("Quantity"))
                    dtUniqueProductId.Rows(intRowCount).Item("ProductName") = dtRep.Rows(intInnerRowCount).Item("Product_Name")
                End If
            Next
            dtUniqueProductId.Rows(intRowCount).Item("Quantity") = intQuantity

            intQuantity = 0
        Next

        If IsNothing(dtRep.Rows(0).Item("DateOfTransaction")) = False Then
            strDateOfTransaction = dtRep.Rows(0).Item("DateOfTransaction").ToString
        Else
            strDateOfTransaction = ""
        End If

        Dim strSalesOrderNumber As String = Me.CurrentSession.VT_SalesOrderNum
        If strSalesOrderNumber = "0" Then
            strSalesOrderNumber = dtRep.Rows(0).Item("SalesOrderNum").ToString
        End If
        Dim objSales As New VT_ReportFunctions.SalesDataFunctions
        Dim dtSalesOrderTable As DataTable = objSales.GetSalesOrderDetails(strConn, strSalesOrderNumber)

        If dtSalesOrderTable IsNot Nothing AndAlso dtSalesOrderTable.Rows.Count > 0 Then
            strPONum = dtSalesOrderTable.Rows(0).Item("CustomerPO").ToString
        End If

        Dim dsSoldToCustomerDetails As DataSet = objC.GetCustomerDetailsForId(dtSalesOrderTable.Rows(0).Item("CustomerId"))
        Dim dsShipToCustomerDetails As DataSet = objC.GetCustomerDetailsForId(dtSalesOrderTable.Rows(0).Item("DeliveryCustomerId"))

        If dtSalesOrderTable.Rows(0).Item("CustomerId").ToString = dtSalesOrderTable.Rows(0).Item("DeliveryCustomerId").ToString Then
            If Not IsNothing(dsSoldToCustomerDetails) Then
                Dim dtSoldToCustomerDetails As DataTable = dsSoldToCustomerDetails.Tables(0)
                Dim strSoldToAddress As String = dtSoldToCustomerDetails.Rows(0).Item("BillingAddress")
                Dim strSplitSoldTo() As String = strSoldToAddress.Split(",")
                For i = 0 To UBound(strSplitSoldTo)
                    If strSplitSoldTo(i) <> "" AndAlso strSplitSoldTo(i) <> " " Then
                        strCleanedSoldToAddress = strCleanedSoldToAddress + Trim(strSplitSoldTo(i)) + Environment.NewLine
                    End If
                Next i
                strSoldToCustomerName = dtSoldToCustomerDetails.Rows(0).Item("CustomerName")
                strSoldToCustomerPhone = dtSoldToCustomerDetails.Rows(0).Item("PhoneNumber")
                strSoldToCustomerFax = dtSoldToCustomerDetails.Rows(0).Item("FaxNumber")
            End If
            If Not IsNothing(dsShipToCustomerDetails) Then
                Dim dtShipToCustomerDetails As DataTable = dsShipToCustomerDetails.Tables(0)
                Dim strDeliveryAddress As String = dtShipToCustomerDetails.Rows(0).Item("DeliveryAddress")
                Dim strSplit() As String = strDeliveryAddress.Split(",")
                For i = 0 To UBound(strSplit)
                    If strSplit(i) <> "" AndAlso strSplit(i) <> " " Then
                        strCleanedDeliveryAddress = strCleanedDeliveryAddress + Trim(strSplit(i)) + Environment.NewLine
                    End If
                Next i
                strShipToCustomerName = dtShipToCustomerDetails.Rows(0).Item("CustomerName")
                strShipToCustomerPhone = dtShipToCustomerDetails.Rows(0).Item("PhoneNumber")
                strShipToCustomerFax = dtShipToCustomerDetails.Rows(0).Item("FaxNumber")
            End If
        Else
            If Not IsNothing(dsSoldToCustomerDetails) Then
                Dim dtSoldToCustomerDetails As DataTable = dsSoldToCustomerDetails.Tables(0)
                Dim strSoldToAddress As String = dtSoldToCustomerDetails.Rows(0).Item("BillingAddress")
                Dim strSplitSoldTo() As String = strSoldToAddress.Split(",")
                For i = 0 To UBound(strSplitSoldTo)
                    If strSplitSoldTo(i) <> "" AndAlso strSplitSoldTo(i) <> " " Then
                        strCleanedSoldToAddress = strCleanedSoldToAddress + Trim(strSplitSoldTo(i)) + Environment.NewLine
                    End If
                Next i
                strSoldToCustomerName = dtSoldToCustomerDetails.Rows(0).Item("CustomerName")
                strSoldToCustomerPhone = dtSoldToCustomerDetails.Rows(0).Item("PhoneNumber")
                strSoldToCustomerFax = dtSoldToCustomerDetails.Rows(0).Item("FaxNumber")
            End If
            If Not IsNothing(dsShipToCustomerDetails) Then
                Dim dtShipToCustomerDetails As DataTable = dsShipToCustomerDetails.Tables(0)
                Dim strDeliveryAddress As String = dtShipToCustomerDetails.Rows(0).Item("BillingAddress")
                Dim strSplit() As String = strDeliveryAddress.Split(",")
                For i = 0 To UBound(strSplit)
                    If strSplit(i) <> "" AndAlso strSplit(i) <> " " Then
                        strCleanedDeliveryAddress = strCleanedDeliveryAddress + Trim(strSplit(i)) + Environment.NewLine
                    End If
                Next i
                strShipToCustomerName = dtShipToCustomerDetails.Rows(0).Item("CustomerName")
                strShipToCustomerPhone = dtShipToCustomerDetails.Rows(0).Item("PhoneNumber")
                strShipToCustomerFax = dtShipToCustomerDetails.Rows(0).Item("FaxNumber")
            End If
        End If

        dsOrderItems = objTele.GetOrderItems(Me.CurrentSession.VT_SalesOrderID)
        '=======================
        Dim objCommonFuncs As New BPADotNetCommonFunctions.VT_CommonFunctions.CommonFunctions
        Dim objProds As New VTDBFunctions.VTDBFunctions.ProductsFunctions


        '---- Start building the Physical Report template ----------------------

        Dim report As Report = New Report()
        Dim section1 As ISection = report.AddSection()
        section1.PagePaddings.All = 25
        'section1.PageSize = New PageSize(600, 1000)

        'section1.PagePaddings.Top = 10
        'section1.PagePaddings.Left = 10
        'section1.PagePaddings.Right = 10
        'section1.PagePaddings.Bottom = 25

        Dim Band1 As IBand = section1.AddBand()
        Band1.Stretch = True


        'Dim bandHeader As Infragistics.Documents.Reports.Report.Band.IBandHeader = Band1.Header


        ' Cause the header to repeat on every page.
        'bandHeader.Repeat = True
        ''bandHeader.Height = New Infragistics.Documents.Reports.Report.FixedHeight(150)
        'bandHeader.Alignment = New Infragistics.Documents.Reports.Report.ContentAlignment _
        '    (Infragistics.Documents.Reports.Report.Alignment.Left, Infragistics.Documents.Reports.Report.Alignment.Middle)

        '' Add 5 pixels of padding around the left and right edges.
        'bandHeader.Paddings.Horizontal = 5


        'Dim tableheader As Infragistics.Documents.Reports.Report.Table.ITable = bandHeader.AddTable
        Dim tableRow As Infragistics.Documents.Reports.Report.Table.ITableRow
        Dim tableRow2 As Infragistics.Documents.Reports.Report.Table.ITableRow
        Dim tableCell As Infragistics.Documents.Reports.Report.Table.ITableCell
        Dim tableCellText As IText


        ' Create a new pattern for the table as a whole.
        Dim tablePattern As New Infragistics.Documents.Reports.Report.Table.TablePattern()
        tablePattern.Borders = New Borders(New Infragistics.Documents.Reports.Graphics.Pen(Infragistics.Documents.Reports.Graphics.Colors.Black))

        Dim BlankTablePattern As New Infragistics.Documents.Reports.Report.Table.TablePattern()

        ' Create a new pattern for the cells.
        Dim tableCellPattern As New Infragistics.Documents.Reports.Report.Table.TableCellPattern()
        tableCellPattern.Borders = New Borders(New Infragistics.Documents.Reports.Graphics.Pen(Infragistics.Documents.Reports.Graphics.Colors.Black))

        Dim tableBlankCellPattern As New Infragistics.Documents.Reports.Report.Table.TableCellPattern()
        tableBlankCellPattern.Borders = New Borders(New Infragistics.Documents.Reports.Graphics.Pen(Infragistics.Documents.Reports.Graphics.Colors.White))

        tableCellPattern.Paddings = New Paddings(2, 3)



        Dim table1 As ITable = Band1.AddTable()

        table1.ApplyPattern(BlankTablePattern)

        'Add first row
        tableRow = table1.AddRow()
        tableCell = tableRow.AddCell()
        tableCellPattern.Apply(tableCell)
        tableCell.Width = New RelativeWidth(100)
        tableCellText = tableCell.AddText()
        tableCellText.Style = New Infragistics.Documents.Reports.Report.Text.Style _
        (New Infragistics.Documents.Reports.Graphics.Font("Arial Bold", 15), Infragistics.Documents.Reports.Graphics.Brushes.Black)
        tableCellText.Alignment = New TextAlignment(Alignment.Center, Alignment.Middle)
        tableCellText.AddContent(" ")


        'add Blank Row
        tableRow = table1.AddRow()
        tableCell = tableRow.AddCell()
        tableCell.Width = New RelativeWidth(100)
        tableCellText = tableCell.AddText()
        tableCellText.AddContent(" ")
        'add Blank Row
        tableRow = table1.AddRow()
        tableCell = tableRow.AddCell()
        tableCell.Width = New RelativeWidth(100)
        tableCellText = tableCell.AddText()
        tableCellText.AddContent(" ")


        tableRow = table1.AddRow()
        tableCell.Width = New RelativeWidth(100)
        tableCellText = tableCell.AddText()
        tableCellText.Style = New Infragistics.Documents.Reports.Report.Text.Style _
        (New Infragistics.Documents.Reports.Graphics.Font("Arial Rounded MT Bold", 11), Infragistics.Documents.Reports.Graphics.Brushes.Black)
        tableCellText.Alignment = New TextAlignment(Alignment.Center, Alignment.Middle)
        tableCellText.AddContent("Standard Uniform Straight Bill of lading - Not Negotiable")

        tableRow = table1.AddRow()
        tableCell.Width = New RelativeWidth(100)
        tableCellText = tableCell.AddText()
        tableCellText.Style = New Infragistics.Documents.Reports.Report.Text.Style _
        (New Infragistics.Documents.Reports.Graphics.Font("Arial", 8), Infragistics.Documents.Reports.Graphics.Brushes.Black)
        tableCellText.Alignment = New TextAlignment(Alignment.Center, Alignment.Middle)
        tableCellText.AddContent("Received, subject to the classification and tariffs in effect on the date of receipt of the property described in the original Bill of Lading")

        'add Blank Row
        tableRow = table1.AddRow()
        tableCell = tableRow.AddCell()
        tableCell.Width = New RelativeWidth(100)
        tableCellText = tableCell.AddText()
        tableCellText.AddContent(" ")

        tableRow = table1.AddRow()
        tableCell = tableRow.AddCell()
        tableCellPattern.Apply(tableCell)
        tableCell.Width = New RelativeWidth(100)
        'add iBand to this cell
        Dim CellBand As IBand = tableCell.AddBand()
        Dim tableInBand As ITable = CellBand.AddTable()
        tableRow2 = tableInBand.AddRow()

        tableCell = tableRow2.AddCell()
        tableCell.Width = New RelativeWidth(40)
        Dim Table As ITable = tableCell.AddTable()

        tableRow = Table.AddRow()
        tableCell = tableRow.AddCell()
        tableCell.Width = New RelativeWidth(100)
        tableCellText = tableCell.AddText()
        tableCellText.Style = New Infragistics.Documents.Reports.Report.Text.Style _
        (New Infragistics.Documents.Reports.Graphics.Font("Arial Bold", 11), Infragistics.Documents.Reports.Graphics.Brushes.Black)
        tableCellText.Alignment = New TextAlignment(Alignment.Left, Alignment.Middle)
        tableCellText.AddContent("Origin:")

        'add Blank Row
        tableRow = Table.AddRow()
        tableCell = tableRow.AddCell()
        tableCell.Width = New RelativeWidth(100)
        tableCellText = tableCell.AddText()
        tableCellText.AddContent(" ")

        tableRow = Table.AddRow()
        tableCell = tableRow.AddCell()
        tableCell.Width = New RelativeWidth(100)
        tableCellText = tableCell.AddText()
        tableCellText.Style = New Infragistics.Documents.Reports.Report.Text.Style _
        (New Infragistics.Documents.Reports.Graphics.Font("Arial", 9), Infragistics.Documents.Reports.Graphics.Brushes.Black)
        tableCellText.Alignment = New TextAlignment(Alignment.Left, Alignment.Middle)
        tableCellText.AddContent("ALIYA'S FOODS LIMITED")

        tableRow = Table.AddRow()
        tableCell = tableRow.AddCell()
        tableCell.Width = New RelativeWidth(100)
        tableCellText = tableCell.AddText()
        tableCellText.Style = New Infragistics.Documents.Reports.Report.Text.Style _
        (New Infragistics.Documents.Reports.Graphics.Font("Arial", 9), Infragistics.Documents.Reports.Graphics.Brushes.Black)
        tableCellText.Alignment = New TextAlignment(Alignment.Left, Alignment.Middle)
        tableCellText.AddContent("6364 Ropers Road NW.")

        tableRow = Table.AddRow()
        tableCell = tableRow.AddCell()
        tableCell.Width = New RelativeWidth(100)
        tableCellText = tableCell.AddText()
        tableCellText.Style = New Infragistics.Documents.Reports.Report.Text.Style _
        (New Infragistics.Documents.Reports.Graphics.Font("Arial", 9), Infragistics.Documents.Reports.Graphics.Brushes.Black)
        tableCellText.Alignment = New TextAlignment(Alignment.Left, Alignment.Middle)
        tableCellText.AddContent("Edmonton, AB")

        tableRow = Table.AddRow()
        tableCell = tableRow.AddCell()
        tableCell.Width = New RelativeWidth(100)
        tableCellText = tableCell.AddText()
        tableCellText.Style = New Infragistics.Documents.Reports.Report.Text.Style _
        (New Infragistics.Documents.Reports.Graphics.Font("Arial", 9), Infragistics.Documents.Reports.Graphics.Brushes.Black)
        tableCellText.Alignment = New TextAlignment(Alignment.Left, Alignment.Middle)
        tableCellText.AddContent("T6B 3P9")




        tableCell = tableRow2.AddCell()
        tableCell.Width = New RelativeWidth(10)
        tableCellText = tableCell.AddText()
        tableCellText.AddContent(" ")

        tableCell = tableRow2.AddCell()
        tableCell.Width = New RelativeWidth(25)
        Table = tableCell.AddTable()

        tableRow = Table.AddRow()
        tableCell = tableRow.AddCell()
        tableCell.Width = New RelativeWidth(100)
        tableCellText = tableCell.AddText()
        tableCellText.Style = New Infragistics.Documents.Reports.Report.Text.Style _
        (New Infragistics.Documents.Reports.Graphics.Font("Arial Bold", 11), Infragistics.Documents.Reports.Graphics.Brushes.Black)
        tableCellText.Alignment = New TextAlignment(Alignment.Left, Alignment.Middle)
        tableCellText.AddContent("Date:")

        'add Blank Row
        tableRow = Table.AddRow()
        tableCell = tableRow.AddCell()
        tableCell.Width = New RelativeWidth(100)
        tableCellText = tableCell.AddText()
        tableCellText.AddContent(" ")

        tableRow = Table.AddRow()
        tableCell = tableRow.AddCell()
        tableCell.Width = New RelativeWidth(100)
        tableCellText = tableCell.AddText()
        tableCellText.Style = New Infragistics.Documents.Reports.Report.Text.Style _
        (New Infragistics.Documents.Reports.Graphics.Font("Arial Bold", 11), Infragistics.Documents.Reports.Graphics.Brushes.Black)
        tableCellText.Alignment = New TextAlignment(Alignment.Left, Alignment.Middle)
        tableCellText.AddContent("Consignor's #:")

        'add Blank Row
        tableRow = Table.AddRow()
        tableCell = tableRow.AddCell()
        tableCell.Width = New RelativeWidth(100)
        tableCellText = tableCell.AddText()
        tableCellText.AddContent(" ")

        tableRow = Table.AddRow()
        tableCell = tableRow.AddCell()
        tableCell.Width = New RelativeWidth(100)
        tableCellText = tableCell.AddText()
        tableCellText.Style = New Infragistics.Documents.Reports.Report.Text.Style _
        (New Infragistics.Documents.Reports.Graphics.Font("Arial Bold", 11), Infragistics.Documents.Reports.Graphics.Brushes.Black)
        tableCellText.Alignment = New TextAlignment(Alignment.Left, Alignment.Middle)
        tableCellText.AddContent("Telephone #:")

        tableCell = tableRow2.AddCell()
        tableCell.Width = New RelativeWidth(25)
        Table = tableCell.AddTable()

        tableRow = Table.AddRow()
        tableCell = tableRow.AddCell()
        tableCell.Width = New RelativeWidth(100)
        tableCellText = tableCell.AddText()
        tableCellText.Style = New Infragistics.Documents.Reports.Report.Text.Style _
        (New Infragistics.Documents.Reports.Graphics.Font("Arial Bold", 11), Infragistics.Documents.Reports.Graphics.Brushes.Black)
        tableCellText.Alignment = New TextAlignment(Alignment.Left, Alignment.Middle)
        tableCellText.AddContent(strDateOfTransaction)

        'add Blank Row
        tableRow = Table.AddRow()
        tableCell = tableRow.AddCell()
        tableCell.Width = New RelativeWidth(100)
        tableCellText = tableCell.AddText()
        tableCellText.AddContent(" ")

        tableRow = Table.AddRow()
        tableCell = tableRow.AddCell()
        tableCell.Width = New RelativeWidth(100)
        tableCellText = tableCell.AddText()
        tableCellText.Style = New Infragistics.Documents.Reports.Report.Text.Style _
        (New Infragistics.Documents.Reports.Graphics.Font("Arial Bold", 11), Infragistics.Documents.Reports.Graphics.Brushes.Black)
        tableCellText.Alignment = New TextAlignment(Alignment.Left, Alignment.Middle)
        tableCellText.AddContent(Trim(Me.CurrentSession.VT_SalesOrderNum))

        'add Blank Row
        tableRow = Table.AddRow()
        tableCell = tableRow.AddCell()
        tableCell.Width = New RelativeWidth(100)
        tableCellText = tableCell.AddText()
        tableCellText.AddContent(" ")

        tableRow = Table.AddRow()
        tableCell = tableRow.AddCell()
        tableCell.Width = New RelativeWidth(100)
        tableCellText = tableCell.AddText()
        tableCellText.Style = New Infragistics.Documents.Reports.Report.Text.Style _
        (New Infragistics.Documents.Reports.Graphics.Font("Arial Bold", 11), Infragistics.Documents.Reports.Graphics.Brushes.Black)
        tableCellText.Alignment = New TextAlignment(Alignment.Left, Alignment.Middle)
        tableCellText.AddContent("(780) 467-4600:")

        'add Blank Row
        tableRow = table1.AddRow()
        tableCell = tableRow.AddCell()
        tableCell.Width = New RelativeWidth(100)
        tableCellText = tableCell.AddText()
        tableCellText.AddContent(" ")

        tableRow = table1.AddRow()
        tableCell = tableRow.AddCell()
        tableCell.Width = New RelativeWidth(100)
        tableCellText = tableCell.AddText()
        tableCellText.Style = New Infragistics.Documents.Reports.Report.Text.Style _
        (New Infragistics.Documents.Reports.Graphics.Font("Arial Bold", 11), Infragistics.Documents.Reports.Graphics.Brushes.Black)
        tableCellText.Alignment = New TextAlignment(Alignment.Left, Alignment.Middle)
        tableCellText.AddContent("CONSIGNED TO:")
        '-----------------------------------------------------------------------------------------------------------------------
        tableRow = table1.AddRow()
        tableCell = tableRow.AddCell()
        tableCellPattern.Apply(tableCell)
        tableCell.Width = New RelativeWidth(100)
        'add iBand to this cell
        CellBand = tableCell.AddBand()
        tableInBand = CellBand.AddTable()
        tableRow2 = tableInBand.AddRow()

        tableCell = tableRow2.AddCell()
        tableCell.Width = New RelativeWidth(40)
        Table = tableCell.AddTable()

        tableRow = Table.AddRow()
        tableCell = tableRow.AddCell()
        tableCell.Width = New RelativeWidth(100)
        tableCellText = tableCell.AddText()
        tableCellText.Style = New Infragistics.Documents.Reports.Report.Text.Style _
        (New Infragistics.Documents.Reports.Graphics.Font("Arial", 9), Infragistics.Documents.Reports.Graphics.Brushes.Black)
        tableCellText.Alignment = New TextAlignment(Alignment.Left, Alignment.Middle)
        tableCellText.AddContent(strShipToCustomerName)

        tableRow = Table.AddRow()
        tableCell = tableRow.AddCell()
        tableCell.Width = New RelativeWidth(100)
        tableCellText = tableCell.AddText()
        tableCellText.Style = New Infragistics.Documents.Reports.Report.Text.Style _
        (New Infragistics.Documents.Reports.Graphics.Font("Arial", 9), Infragistics.Documents.Reports.Graphics.Brushes.Black)
        tableCellText.Alignment = New TextAlignment(Alignment.Left, Alignment.Middle)
        tableCellText.AddContent(strCleanedDeliveryAddress)


        tableCell = tableRow2.AddCell()
        tableCell.Width = New RelativeWidth(10)
        tableCellText = tableCell.AddText()
        tableCellText.AddContent(" ")

        tableCell = tableRow2.AddCell()
        tableCell.Width = New RelativeWidth(25)
        Table = tableCell.AddTable()

        'add Blank Row
        tableRow = Table.AddRow()
        tableCell = tableRow.AddCell()
        tableCell.Width = New RelativeWidth(100)
        tableCellText = tableCell.AddText()
        tableCellText.AddContent(" ")

        tableRow = Table.AddRow()
        tableCell = tableRow.AddCell()
        tableCell.Width = New RelativeWidth(100)
        tableCellText = tableCell.AddText()
        tableCellText.Style = New Infragistics.Documents.Reports.Report.Text.Style _
        (New Infragistics.Documents.Reports.Graphics.Font("Arial Bold", 11), Infragistics.Documents.Reports.Graphics.Brushes.Black)
        tableCellText.Alignment = New TextAlignment(Alignment.Left, Alignment.Middle)
        tableCellText.AddContent("Consignee's P.O:")

        'add Blank Row
        tableRow = Table.AddRow()
        tableCell = tableRow.AddCell()
        tableCell.Width = New RelativeWidth(100)
        tableCellText = tableCell.AddText()
        tableCellText.AddContent(" ")

        tableRow = Table.AddRow()
        tableCell = tableRow.AddCell()
        tableCell.Width = New RelativeWidth(100)
        tableCellText = tableCell.AddText()
        tableCellText.Style = New Infragistics.Documents.Reports.Report.Text.Style _
        (New Infragistics.Documents.Reports.Graphics.Font("Arial Bold", 11), Infragistics.Documents.Reports.Graphics.Brushes.Black)
        tableCellText.Alignment = New TextAlignment(Alignment.Left, Alignment.Middle)
        tableCellText.AddContent("Telephone #:")



        tableCell = tableRow2.AddCell()
        tableCell.Width = New RelativeWidth(25)
        Table = tableCell.AddTable()

        'add Blank Row
        tableRow = Table.AddRow()
        tableCell = tableRow.AddCell()
        tableCell.Width = New RelativeWidth(100)
        tableCellText = tableCell.AddText()
        tableCellText.AddContent(" ")

        tableRow = Table.AddRow()
        tableCell = tableRow.AddCell()
        tableCell.Width = New RelativeWidth(100)
        tableCellText = tableCell.AddText()
        tableCellText.Style = New Infragistics.Documents.Reports.Report.Text.Style _
        (New Infragistics.Documents.Reports.Graphics.Font("Arial Bold", 11), Infragistics.Documents.Reports.Graphics.Brushes.Black)
        tableCellText.Alignment = New TextAlignment(Alignment.Left, Alignment.Middle)
        tableCellText.AddContent(Format(strPONum))

        'add Blank Row
        tableRow = Table.AddRow()
        tableCell = tableRow.AddCell()
        tableCell.Width = New RelativeWidth(100)
        tableCellText = tableCell.AddText()
        tableCellText.AddContent(" ")

        tableRow = Table.AddRow()
        tableCell = tableRow.AddCell()
        tableCell.Width = New RelativeWidth(100)
        tableCellText = tableCell.AddText()
        tableCellText.Style = New Infragistics.Documents.Reports.Report.Text.Style _
        (New Infragistics.Documents.Reports.Graphics.Font("Arial Bold", 11), Infragistics.Documents.Reports.Graphics.Brushes.Black)
        tableCellText.Alignment = New TextAlignment(Alignment.Left, Alignment.Middle)
        tableCellText.AddContent(strShipToCustomerPhone)
        '--------------------------------------------------------------------------------------------------------------------------------------
        'add Blank Row
        tableRow = table1.AddRow()
        tableCell = tableRow.AddCell()
        tableCell.Width = New RelativeWidth(100)
        tableCellText = tableCell.AddText()
        tableCellText.AddContent(" ")

        tableRow = table1.AddRow()
        tableCell = tableRow.AddCell()
        tableCell.Width = New RelativeWidth(100)
        tableCellText = tableCell.AddText()
        tableCellText.Style = New Infragistics.Documents.Reports.Report.Text.Style _
(New Infragistics.Documents.Reports.Graphics.Font("Arial Bold", 11), Infragistics.Documents.Reports.Graphics.Brushes.Black)
        tableCellText.Alignment = New TextAlignment(Alignment.Left, Alignment.Middle)
        tableCellText.AddContent("Deliver on: ________________________")



        'add Blank Row
        tableRow = table1.AddRow()
        tableCell = tableRow.AddCell()
        tableCell.Width = New RelativeWidth(100)
        tableCellText = tableCell.AddText()
        tableCellText.AddContent(" ")

        tableRow = table1.AddRow()
        tableCell = tableRow.AddCell()
        ' tableCellPattern.Apply(tableCell)
        tableCell.Width = New RelativeWidth(100)
        'add iBand to this cell
        ' CellBand = tableCell.AddBand()
        'tableInBand = CellBand.AddTable()
        'tableRow2 = tableInBand.AddRow()


        Table = tableCell.AddTable()
        tableRow2 = Table.AddRow()
        tableCell = tableRow2.AddCell()
        tableCell.Width = New RelativeWidth(70)

        Table = tableCell.AddTable()
        tableRow = Table.AddRow()

        '' Create a new pattern for the header cells.
        Dim tableCellHeaderPattern As New Infragistics.Documents.Reports.Report.Table.TableCellPattern()
        'tableCellHeaderPattern.Background = New Background(Infragistics.Documents.Reports.Graphics.Brushes.LightGray)
        tableCellHeaderPattern.Borders = New Borders(New Infragistics.Documents.Reports.Graphics.Pen(Infragistics.Documents.Reports.Graphics.Colors.Black))

        Dim strColNames(2) As String
        strColNames(0) = "No Of Cases"
        strColNames(1) = "Description of Articles"
        strColNames(2) = "Weights "


        For intArrayCount = 0 To strColNames.Length - 1
            tableCell = tableRow.AddCell
            tableCellHeaderPattern.Apply(tableCell)
            Dim tableHeaderCellText As IText = tableCell.AddText()
            tableHeaderCellText.Style = New Infragistics.Documents.Reports.Report.Text.Style(New Infragistics.Documents.Reports.Graphics.Font("Arial MT Bold", 11), Infragistics.Documents.Reports.Graphics.Brushes.Black)
            tableHeaderCellText.Alignment = New TextAlignment(Alignment.Center, Alignment.Middle)
            Select Case intArrayCount
                Case 0
                    tableCell.Width = New RelativeWidth(20)
                Case 1
                    tableCell.Width = New RelativeWidth(60)
                Case 2
                    tableCell.Width = New RelativeWidth(20)

            End Select



            tableHeaderCellText.AddContent(" " + strColNames(intArrayCount) + " ")

        Next

        'table1Header.Repeat = True

        ''Create table Rows
        'Dim dblTotalPrice As Double = 0
        'Dim dblTotalTax As Double = 0
        'Dim dblTotalPriceIncludingTax As Double = 0


        For intRowCount As Integer = 0 To dtUniqueProductId.Rows.Count - 1
            tableRow = Table.AddRow()
            For intArrayCount = 0 To strColNames.Length - 1
                tableCell = tableRow.AddCell()
                tableCellPattern.Apply(tableCell)
                tableCellText = tableCell.AddText()
                tableCellText.Style = New Infragistics.Documents.Reports.Report.Text.Style(New Infragistics.Documents.Reports.Graphics.Font("Arial MT", 9), Infragistics.Documents.Reports.Graphics.Brushes.Black)
                tableCellText.Alignment = New TextAlignment(Alignment.Left, Alignment.Middle)
                Select Case intArrayCount
                    Case 0
                        tableCell.Width = New RelativeWidth(20)
                        tableCellText.AddContent(" " + dtUniqueProductId.Rows(intRowCount).Item("Quantity").ToString + " ")
                    Case 1
                        tableCell.Width = New RelativeWidth(60)
                        tableCellText.AddContent(" " + dtUniqueProductId.Rows(intRowCount).Item("ProductName").ToString + " ")
                    Case 2
                        tableCell.Width = New RelativeWidth(20)

                End Select

            Next
        Next
        'add a row for totals
        tableRow = Table.AddRow()

        tableCell = tableRow.AddCell()
        tableCellPattern.Apply(tableCell)
        tableCellText = tableCell.AddText()
        tableCellText.Style = New Infragistics.Documents.Reports.Report.Text.Style(New Infragistics.Documents.Reports.Graphics.Font("Arial MT", 9), Infragistics.Documents.Reports.Graphics.Brushes.Black)
        tableCellText.Alignment = New TextAlignment(Alignment.Left, Alignment.Middle)
        tableCell.Width = New RelativeWidth(20)
        tableCellText.AddContent(" Total  " + intTotalQuantity.ToString)

        tableCell = tableRow.AddCell()
        tableCellPattern.Apply(tableCell)
        tableCellText = tableCell.AddText()
        tableCellText.Style = New Infragistics.Documents.Reports.Report.Text.Style(New Infragistics.Documents.Reports.Graphics.Font("Arial MT", 9), Infragistics.Documents.Reports.Graphics.Brushes.Black)
        tableCellText.Alignment = New TextAlignment(Alignment.Left, Alignment.Middle)
        tableCell.Width = New RelativeWidth(60)
        tableCellText.AddContent(" ")

        tableCell = tableRow.AddCell()
        tableCellPattern.Apply(tableCell)
        tableCellText = tableCell.AddText()
        tableCellText.Style = New Infragistics.Documents.Reports.Report.Text.Style(New Infragistics.Documents.Reports.Graphics.Font("Arial MT", 9), Infragistics.Documents.Reports.Graphics.Brushes.Black)
        tableCellText.Alignment = New TextAlignment(Alignment.Left, Alignment.Middle)
        tableCell.Width = New RelativeWidth(20)
        tableCellText.AddContent(" ")

        tableCell = tableRow2.AddCell()
        tableCell.Width = New RelativeWidth(3)
        tableCellText = tableCell.AddText()
        tableCellText.Style = New Infragistics.Documents.Reports.Report.Text.Style(New Infragistics.Documents.Reports.Graphics.Font("Arial MT", 9), Infragistics.Documents.Reports.Graphics.Brushes.Black)
        tableCellText.Alignment = New TextAlignment(Alignment.Left, Alignment.Middle)
        tableCellText.AddContent(" ")

        tableCell = tableRow2.AddCell()
        tableCell.Width = New RelativeWidth(27)


        Table = tableCell.AddTable()

        tableRow = Table.AddRow
        tableCell = tableRow.AddCell
        tableCellPattern.Apply(tableCell)
        Dim table2 As ITable = tableCell.AddTable
        tableRow = table2.AddRow
        tableCell = tableRow.AddCell
        tableCell.Width = New RelativeWidth(100)
        tableCellText = tableCell.AddText()
        tableCellText.Style = New Infragistics.Documents.Reports.Report.Text.Style(New Infragistics.Documents.Reports.Graphics.Font("Arial MT", 9), Infragistics.Documents.Reports.Graphics.Brushes.Black)
        tableCellText.Alignment = New TextAlignment(Alignment.Center, Alignment.Middle)
        tableCell.Width = New RelativeWidth(20)
        tableCellText.AddContent("Freight Charges")

        'add Blank Row
        tableRow = table2.AddRow()
        tableCell = tableRow.AddCell()
        tableCell.Width = New RelativeWidth(100)
        tableCellText = tableCell.AddText()
        tableCellText.AddContent(" ")
        'add row with checkboxes
        tableRow = table2.AddRow()
        tableCell = tableRow.AddCell()
        tableCell.Width = New RelativeWidth(17)
        tableCellText = tableCell.AddText()
        tableCellText.AddContent(" ")

        'square
        tableCell = tableRow.AddCell()
        tableCellPattern.Apply(tableCell)
        tableCell.Width = New RelativeWidth(17)
        tableCellText = tableCell.AddText()
        tableCellText.AddContent(" ")

        tableCell = tableRow.AddCell()
        tableCell.Width = New RelativeWidth(16)
        tableCellText = tableCell.AddText()
        tableCellText.AddContent(" ")

        tableCell = tableRow.AddCell()
        tableCell.Width = New RelativeWidth(16)
        tableCellText = tableCell.AddText()
        tableCellText.AddContent(" ")

        'square
        tableCell = tableRow.AddCell()
        tableCellPattern.Apply(tableCell)
        tableCell.Width = New RelativeWidth(17)
        tableCellText = tableCell.AddText()
        tableCellText.AddContent("X")

        tableCell = tableRow.AddCell()
        tableCell.Width = New RelativeWidth(17)
        tableCellText = tableCell.AddText()
        tableCellText.AddContent(" ")

        'Add row with text
        tableRow = table2.AddRow()
        tableCell = tableRow.AddCell()
        tableCell.Width = New RelativeWidth(17)
        tableCellText = tableCell.AddText()
        tableCellText.AddContent(" ")

        'square
        tableCell = tableRow.AddCell()
        tableCell.Width = New RelativeWidth(18)
        tableCellText = tableCell.AddText()
        tableCellText.Style = New Infragistics.Documents.Reports.Report.Text.Style(New Infragistics.Documents.Reports.Graphics.Font("Arial MT", 7), Infragistics.Documents.Reports.Graphics.Brushes.Black)
        tableCellText.Alignment = New TextAlignment(Alignment.Center, Alignment.Middle)
        tableCellText.AddContent("Collect")

        tableCell = tableRow.AddCell()
        tableCell.Width = New RelativeWidth(15)
        tableCellText = tableCell.AddText()
        tableCellText.AddContent(" ")

        tableCell = tableRow.AddCell()
        tableCell.Width = New RelativeWidth(15)
        tableCellText = tableCell.AddText()
        tableCellText.AddContent(" ")

        'square
        tableCell = tableRow.AddCell()
        tableCell.Width = New RelativeWidth(18)
        tableCellText = tableCell.AddText()
        tableCellText.Style = New Infragistics.Documents.Reports.Report.Text.Style(New Infragistics.Documents.Reports.Graphics.Font("Arial MT", 7), Infragistics.Documents.Reports.Graphics.Brushes.Black)
        tableCellText.Alignment = New TextAlignment(Alignment.Center, Alignment.Middle)
        tableCellText.AddContent("Prepaid")

        tableCell = tableRow.AddCell()
        tableCell.Width = New RelativeWidth(17)
        tableCellText = tableCell.AddText()
        tableCellText.AddContent(" ")

        'add Blank Row
        tableRow = table2.AddRow()
        tableCell = tableRow.AddCell()
        tableCell.Width = New RelativeWidth(100)
        tableCellText = tableCell.AddText()
        tableCellText.AddContent(" ")
        'add Blank Row
        tableRow = table2.AddRow()
        tableCell = tableRow.AddCell()
        tableCell.Width = New RelativeWidth(100)
        tableCellText = tableCell.AddText()
        tableCellText.AddContent(" ")

        tableRow = table2.AddRow()
        tableCell = tableRow.AddCell()
        tableCell.Width = New RelativeWidth(17)
        tableCellText = tableCell.AddText()
        tableCellText.Style = New Infragistics.Documents.Reports.Report.Text.Style(New Infragistics.Documents.Reports.Graphics.Font("Arial MT", 9), Infragistics.Documents.Reports.Graphics.Brushes.Black)
        tableCellText.Alignment = New TextAlignment(Alignment.Center, Alignment.Middle)
        tableCellText.AddContent("Declared Value")

        'add Blank Row
        tableRow = table2.AddRow()
        tableCell = tableRow.AddCell()
        tableCell.Width = New RelativeWidth(100)
        tableCellText = tableCell.AddText()
        tableCellText.AddContent(" ")
        'add Blank Row
        tableRow = table2.AddRow()
        tableCell = tableRow.AddCell()
        tableCell.Width = New RelativeWidth(100)
        tableCellText = tableCell.AddText()
        tableCellText.AddContent(" ")


        tableRow = Table.AddRow
        tableCell = tableRow.AddCell
        tableCellPattern.Apply(tableCell)
        table2 = tableCell.AddTable


        'add Blank Row
        tableRow = table2.AddRow()
        tableCell = tableRow.AddCell()
        tableCell.Width = New RelativeWidth(100)
        tableCellText = tableCell.AddText()
        tableCellText.AddContent(" ")


        tableRow = table2.AddRow()
        tableCell = tableRow.AddCell()
        tableCell.Width = New RelativeWidth(30)
        tableCellText = tableCell.AddText()
        tableCellText.Style = New Infragistics.Documents.Reports.Report.Text.Style(New Infragistics.Documents.Reports.Graphics.Font("Arial MT", 7), Infragistics.Documents.Reports.Graphics.Brushes.Black)
        tableCellText.Alignment = New TextAlignment(Alignment.Left, Alignment.Middle)
        tableCell.Width = New RelativeWidth(20)
        tableCellText.AddContent("# of Pallets")

        tableRow = Table.AddRow
        tableCell = tableRow.AddCell
        tableCellPattern.Apply(tableCell)
        Dim table3 As ITable = tableCell.AddTable


        'add Blank Row
        tableRow = table3.AddRow()
        tableCell = tableRow.AddCell()
        tableCell.Width = New RelativeWidth(100)
        tableCellText = tableCell.AddText()
        tableCellText.AddContent(" ")


        tableRow = table3.AddRow()
        tableCell = tableRow.AddCell()
        tableCell.Width = New RelativeWidth(30)
        tableCellText = tableCell.AddText()
        tableCellText.Style = New Infragistics.Documents.Reports.Report.Text.Style(New Infragistics.Documents.Reports.Graphics.Font("Arial MT", 7), Infragistics.Documents.Reports.Graphics.Brushes.Black)
        tableCellText.Alignment = New TextAlignment(Alignment.Left, Alignment.Middle)
        tableCell.Width = New RelativeWidth(20)
        tableCellText.AddContent("# of Chep Pallets")

        tableCell = tableRow.AddCell()
        tableCell.Width = New RelativeWidth(70)
        tableCellText = tableCell.AddText()
        tableCellText.Style = New Infragistics.Documents.Reports.Report.Text.Style(New Infragistics.Documents.Reports.Graphics.Font("Arial MT", 7), Infragistics.Documents.Reports.Graphics.Brushes.Black)
        tableCellText.Alignment = New TextAlignment(Alignment.Left, Alignment.Middle)
        tableCell.Width = New RelativeWidth(20)
        tableCellText.AddContent(" ")

        tableRow = table1.AddRow()
        tableCell = tableRow.AddCell()
        tableCell.Width = New RelativeWidth(100)
        tableCellText = tableCell.AddText()
        tableCellText.Style = New Infragistics.Documents.Reports.Report.Text.Style _
        (New Infragistics.Documents.Reports.Graphics.Font("Arial Bold", 12), Infragistics.Documents.Reports.Graphics.Brushes.Black)
        tableCellText.Alignment = New TextAlignment(Alignment.Center, Alignment.Middle)
        tableCellText.AddContent(" ")

        tableRow = table1.AddRow()
        tableCell = tableRow.AddCell()
        tableCell.Width = New RelativeWidth(100)
        tableCellText = tableCell.AddText()
        tableCellText.Style = New Infragistics.Documents.Reports.Report.Text.Style _
        (New Infragistics.Documents.Reports.Graphics.Font("Arial Rounded MT Bold", 12), Infragistics.Documents.Reports.Graphics.Brushes.Black)
        tableCellText.Alignment = New TextAlignment(Alignment.Center, Alignment.Middle)
        tableCellText.AddContent("PERISHABLE GOODS, HOLD AT -18 DEGREES CELSIUS")

        tableRow = table1.AddRow()
        tableCell = tableRow.AddCell()
        tableCell.Width = New RelativeWidth(100)
        tableCellText = tableCell.AddText()
        tableCellText.Style = New Infragistics.Documents.Reports.Report.Text.Style _
        (New Infragistics.Documents.Reports.Graphics.Font("Arial Bold", 13), Infragistics.Documents.Reports.Graphics.Brushes.Black)
        tableCellText.Alignment = New TextAlignment(Alignment.Center, Alignment.Middle)
        tableCellText.AddContent(" ")

        tableRow = table1.AddRow()
        tableCell = tableRow.AddCell()
        tableCell.Width = New RelativeWidth(15)
        tableCellText = tableCell.AddText()
        tableCellText.Style = New Infragistics.Documents.Reports.Report.Text.Style _
        (New Infragistics.Documents.Reports.Graphics.Font("Arial Bold", 13), Infragistics.Documents.Reports.Graphics.Brushes.Black)
        tableCellText.Alignment = New TextAlignment(Alignment.Center, Alignment.Middle)
        tableCellText.AddContent(" ")

        tableCell = tableRow.AddCell()
        tableCell.Width = New RelativeWidth(85)
        tableCellText = tableCell.AddText()
        tableCellText.Style = New Infragistics.Documents.Reports.Report.Text.Style _
        (New Infragistics.Documents.Reports.Graphics.Font("Arial Bold", 11), Infragistics.Documents.Reports.Graphics.Brushes.Black)
        tableCellText.Alignment = New TextAlignment(Alignment.Left, Alignment.Middle)
        tableCellText.AddContent("Carrier:")
        'add blank row
        tableRow = table1.AddRow()
        tableCell = tableRow.AddCell()
        tableCell.Width = New RelativeWidth(100)
        tableCellText = tableCell.AddText()
        tableCellText.Style = New Infragistics.Documents.Reports.Report.Text.Style _
        (New Infragistics.Documents.Reports.Graphics.Font("Arial Bold", 13), Infragistics.Documents.Reports.Graphics.Brushes.Black)
        tableCellText.Alignment = New TextAlignment(Alignment.Center, Alignment.Middle)
        tableCellText.AddContent(" ")

        tableRow = table1.AddRow()
        tableCell = tableRow.AddCell()
        tableCell.Width = New RelativeWidth(15)
        tableCellText = tableCell.AddText()
        tableCellText.Style = New Infragistics.Documents.Reports.Report.Text.Style _
        (New Infragistics.Documents.Reports.Graphics.Font("Arial Bold", 13), Infragistics.Documents.Reports.Graphics.Brushes.Black)
        tableCellText.Alignment = New TextAlignment(Alignment.Center, Alignment.Middle)
        tableCellText.AddContent(" ")

        tableCell = tableRow.AddCell()
        tableCell.Width = New RelativeWidth(35)
        tableCellText = tableCell.AddText()
        tableCellText.Style = New Infragistics.Documents.Reports.Report.Text.Style _
        (New Infragistics.Documents.Reports.Graphics.Font("Arial Bold", 11), Infragistics.Documents.Reports.Graphics.Brushes.Black)
        tableCellText.Alignment = New TextAlignment(Alignment.Left, Alignment.Middle)
        tableCellText.AddContent("Per: ____________________________")

        tableCell = tableRow.AddCell()
        tableCell.Width = New RelativeWidth(7)
        tableCellText = tableCell.AddText()
        tableCellText.Style = New Infragistics.Documents.Reports.Report.Text.Style _
        (New Infragistics.Documents.Reports.Graphics.Font("Arial Bold", 13), Infragistics.Documents.Reports.Graphics.Brushes.Black)
        tableCellText.Alignment = New TextAlignment(Alignment.Center, Alignment.Middle)
        tableCellText.AddContent(" ")

        tableCell = tableRow.AddCell()
        tableCell.Width = New RelativeWidth(43)
        tableCellText = tableCell.AddText()
        tableCellText.Style = New Infragistics.Documents.Reports.Report.Text.Style _
        (New Infragistics.Documents.Reports.Graphics.Font("Arial Bold", 11), Infragistics.Documents.Reports.Graphics.Brushes.Black)
        tableCellText.Alignment = New TextAlignment(Alignment.Left, Alignment.Middle)
        tableCellText.AddContent("Unit #: _________")

        'add blank row
        tableRow = table1.AddRow()
        tableCell = tableRow.AddCell()
        tableCell.Width = New RelativeWidth(100)
        tableCellText = tableCell.AddText()
        tableCellText.Style = New Infragistics.Documents.Reports.Report.Text.Style _
        (New Infragistics.Documents.Reports.Graphics.Font("Arial Bold", 13), Infragistics.Documents.Reports.Graphics.Brushes.Black)
        tableCellText.Alignment = New TextAlignment(Alignment.Center, Alignment.Middle)
        tableCellText.AddContent(" ")

        tableRow = table1.AddRow()
        tableCell = tableRow.AddCell()
        tableCell.Width = New RelativeWidth(15)
        tableCellText = tableCell.AddText()
        tableCellText.Style = New Infragistics.Documents.Reports.Report.Text.Style _
        (New Infragistics.Documents.Reports.Graphics.Font("Arial Bold", 13), Infragistics.Documents.Reports.Graphics.Brushes.Black)
        tableCellText.Alignment = New TextAlignment(Alignment.Center, Alignment.Middle)
        tableCellText.AddContent(" ")

        tableCell = tableRow.AddCell()
        tableCell.Width = New RelativeWidth(35)
        tableCellText = tableCell.AddText()
        tableCellText.Style = New Infragistics.Documents.Reports.Report.Text.Style _
        (New Infragistics.Documents.Reports.Graphics.Font("Arial Bold", 11), Infragistics.Documents.Reports.Graphics.Brushes.Black)
        tableCellText.Alignment = New TextAlignment(Alignment.Left, Alignment.Middle)
        tableCellText.AddContent("Date: _________________________")

        tableCell = tableRow.AddCell()
        tableCell.Width = New RelativeWidth(7)
        tableCellText = tableCell.AddText()
        tableCellText.Style = New Infragistics.Documents.Reports.Report.Text.Style _
        (New Infragistics.Documents.Reports.Graphics.Font("Arial Bold", 13), Infragistics.Documents.Reports.Graphics.Brushes.Black)
        tableCellText.Alignment = New TextAlignment(Alignment.Center, Alignment.Middle)
        tableCellText.AddContent(" ")

        tableCell = tableRow.AddCell()
        tableCell.Width = New RelativeWidth(43)
        tableCellText = tableCell.AddText()
        tableCellText.Style = New Infragistics.Documents.Reports.Report.Text.Style _
        (New Infragistics.Documents.Reports.Graphics.Font("Arial Bold", 11), Infragistics.Documents.Reports.Graphics.Brushes.Black)
        tableCellText.Alignment = New TextAlignment(Alignment.Left, Alignment.Middle)
        tableCellText.AddContent("No. of Pcs. _________")

        'add blank row
        tableRow = table1.AddRow()
        tableCell = tableRow.AddCell()
        tableCell.Width = New RelativeWidth(100)
        tableCellText = tableCell.AddText()
        tableCellText.Style = New Infragistics.Documents.Reports.Report.Text.Style _
        (New Infragistics.Documents.Reports.Graphics.Font("Arial Bold", 13), Infragistics.Documents.Reports.Graphics.Brushes.Black)
        tableCellText.Alignment = New TextAlignment(Alignment.Center, Alignment.Middle)
        tableCellText.AddContent(" ")
        'add blank row
        tableRow = table1.AddRow()
        tableCell = tableRow.AddCell()
        tableCell.Width = New RelativeWidth(100)
        tableCellText = tableCell.AddText()
        tableCellText.Style = New Infragistics.Documents.Reports.Report.Text.Style _
        (New Infragistics.Documents.Reports.Graphics.Font("Arial Bold", 13), Infragistics.Documents.Reports.Graphics.Brushes.Black)
        tableCellText.Alignment = New TextAlignment(Alignment.Center, Alignment.Middle)
        tableCellText.AddContent(" ")

        tableRow = table1.AddRow()
        tableCell = tableRow.AddCell()
        tableCell.Width = New RelativeWidth(100)
        tableCellText = tableCell.AddText()
        tableCellText.Style = New Infragistics.Documents.Reports.Report.Text.Style _
        (New Infragistics.Documents.Reports.Graphics.Font("Arial Rounded MT Bold", 20), Infragistics.Documents.Reports.Graphics.Brushes.Black)
        tableCellText.Alignment = New TextAlignment(Alignment.Center, Alignment.Middle)
        tableCellText.AddContent("FREEZER")





        Dim objBPA As New BPADotNetCommonFunctions.VT_CommonFunctions.CommonFunctions
        Dim strRootPath As String
        Dim strRootPathWeb As String
        strRootPathWeb = objBPA.GetConfigItem("VTStoreRootFolderWeb")
        strRootPath = objBPA.GetConfigItem("VTStoreRootFolder")

        ' Publish the report using the current user's id as prt of the filename so that we don't end up with masses of PDF files

        ' The FileFormat enum can be used to publish the report as an XPS or plain text file as well.

        Dim filePath As String = strRootPath + "\Published"
        If System.IO.Directory.Exists(filePath) = False Then
            System.IO.Directory.CreateDirectory(filePath)
        End If
        Dim strFileName As String = filePath + "\BillOfLading_User" + Session("_VT_CurrentUserId").ToString() + ".pdf"
        Dim strFileNameWeb As String = strRootPathWeb + "\Published\BillOfLading_User" + Session("_VT_CurrentUserId").ToString() + ".pdf"
        ' add a random (timer based) bit to the url in order to overcome IIS caching of the PDF file
        strFileNameWeb += String.Format("?t={0}", PortalFunctions.Now.Ticks.ToString())

        If System.IO.File.Exists(strFileName) Then
            System.IO.File.Delete(strFileName)
        End If
        report.Publish(strFileName, FileFormat.PDF)
        ' The Process.Start method runs the specified file
        ' using the application registered to run that file.
        'Response.Cache.SetCacheability(HttpCacheability.NoCache)

        'Response.Redirect(strFileNameWeb)

        'System.Diagnostics.Process.Start((fileName))

        Session("_VT_DocInIFrame") = strFileNameWeb
        Dim strDocDisplayPage As String = "~/DisplayDocPage.aspx"
        Response.Redirect(strDocDisplayPage)



    End Sub



End Class






