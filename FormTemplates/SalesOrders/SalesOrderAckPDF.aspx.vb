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


Partial Class SalesOrderAckTemplatePDF

    Inherits MyBasePage


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim objC As New VTDBFunctions.VTDBFunctions.CustomerFunctions
        Dim objSales As New BPADotNetCommonFunctions.SalesFunctions.Sales_Orders
        Dim strConn As String = Session("_VT_DotNetConnString")

        Dim dsOrderItems As New DataSet
        Dim objTele As New TelesalesFunctions
        Dim objProd As New VTDBFunctions.VTDBFunctions.ProductsFunctions
        Dim objDataPreserve As New BPADotNetCommonFunctions.VT_DataPreserve.DataPreserve
        Dim objDBF As New MatrixFunctionsForSales.MatrixFunctions_Sales

        Dim dtOrderItems, dtCustomerItems, dtServiceItems As DataTable

        'Me.CurrentSession.VT_SalesOrderNum = "20792"

        Dim strSalesOrderNum, strAttention, strCreditTerms, strCustomerPONum, strComments As String



        strSalesOrderNum = Me.CurrentSession.VT_SalesOrderNum
        dtOrderItems = objSales.GetSalesOrderItemsFromMatrix("ORDERITEMS", CInt(Me.CurrentSession.VT_SalesOrderNum), strConn, Me.CurrentSession.VT_tlsNumFields)
        dtCustomerItems = objSales.GetSalesOrderItemsFromMatrix("CUSTOMERITEMS", CInt(Me.CurrentSession.VT_SalesOrderNum), strConn, Me.CurrentSession.VT_tlsNumFields)
        dtServiceItems = objSales.GetSalesOrderItemsFromMatrix("SERVICEITEMS", CInt(Me.CurrentSession.VT_SalesOrderNum), strConn, Me.CurrentSession.VT_tlsNumFields)

        If dtOrderItems IsNot Nothing AndAlso dtOrderItems.Rows.Count > 0 _
            AndAlso dtCustomerItems IsNot Nothing AndAlso dtCustomerItems.Rows.Count > 0 Then

        End If


        'Dim dsCustomerDetails As DataSet = objC.GetCustomerDetailsForId(dtCustomerItems.Rows(0).Item("SoldTo_Id"))
        'Dim dtCustomerDetails As DataTable = dsCustomerDetails.Tables(0)



        'Dim strDeliveryAddress As String = dtSalesOrderTable.Rows(0).Item("DeliveryCustomerAddress")
        'Dim strSplit() As String = strDeliveryAddress.Split(",")
        'Dim strCleanedDeliveryAddress As String = ""
        'For i = 0 To UBound(strSplit)

        '    If strSplit(i) <> "" AndAlso strSplit(i) <> " " Then
        '        strCleanedDeliveryAddress = strCleanedDeliveryAddress + strSplit(i) + Environment.NewLine
        '    End If

        'Next i

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


        Dim bandHeader As Infragistics.Documents.Reports.Report.Band.IBandHeader = Band1.Header


        ' Cause the header to repeat on every page.
        bandHeader.Repeat = True
        'bandHeader.Height = New Infragistics.Documents.Reports.Report.FixedHeight(150)
        bandHeader.Alignment = New Infragistics.Documents.Reports.Report.ContentAlignment _
            (Infragistics.Documents.Reports.Report.Alignment.Left, Infragistics.Documents.Reports.Report.Alignment.Middle)

        ' Add 5 pixels of padding around the left and right edges.
        bandHeader.Paddings.Horizontal = 5


        Dim tableheader As Infragistics.Documents.Reports.Report.Table.ITable = bandHeader.AddTable
        Dim tableRow As Infragistics.Documents.Reports.Report.Table.ITableRow
        Dim tableRowA As Infragistics.Documents.Reports.Report.Table.ITableRow
        Dim tableCellTableRow As Infragistics.Documents.Reports.Report.Table.ITableRow
        Dim tableCell As Infragistics.Documents.Reports.Report.Table.ITableCell
        Dim tableCellText As IText

        ' Create a new pattern for the header cells.
        Dim tableCellHeaderPattern As New Infragistics.Documents.Reports.Report.Table.TableCellPattern()
        tableCellHeaderPattern.Background = New Background(Infragistics.Documents.Reports.Graphics.Brushes.LightGray)
        tableCellHeaderPattern.Borders = New Borders(New Infragistics.Documents.Reports.Graphics.Pen(Infragistics.Documents.Reports.Graphics.Colors.Black))

        ' Create a new pattern for the table as a whole.
        Dim tablePattern As New Infragistics.Documents.Reports.Report.Table.TablePattern()
        tablePattern.Borders = New Borders(New Infragistics.Documents.Reports.Graphics.Pen(Infragistics.Documents.Reports.Graphics.Colors.Black))

        Dim BlankTablePattern As New Infragistics.Documents.Reports.Report.Table.TablePattern()

        ' Create a new pattern for the cells.
        Dim tableCellPattern As New Infragistics.Documents.Reports.Report.Table.TableCellPattern()
        tableCellPattern.Borders = New Borders(New Infragistics.Documents.Reports.Graphics.Pen(Infragistics.Documents.Reports.Graphics.Colors.Black))

        Dim tableBlankCellPattern As New Infragistics.Documents.Reports.Report.Table.TableCellPattern()
        tableBlankCellPattern.Borders = New Borders(New Infragistics.Documents.Reports.Graphics.Pen(Infragistics.Documents.Reports.Graphics.Colors.White))

        Dim tableGreyCellPattern As New Infragistics.Documents.Reports.Report.Table.TableCellPattern()
        tableCellHeaderPattern.Background = New Background(Infragistics.Documents.Reports.Graphics.Brushes.LightGray)
        tableBlankCellPattern.Borders = New Borders(New Infragistics.Documents.Reports.Graphics.Pen(Infragistics.Documents.Reports.Graphics.Colors.White))

        tableCellPattern.Paddings = New Paddings(2, 3)

        'Apply the pattern to the tables

        tableheader.Width = New RelativeWidth(100)
        tableheader.ApplyPattern(BlankTablePattern)

        tableRow = tableheader.AddRow()
        tableCell = tableRow.AddCell()
        tableCell.Width = New RelativeWidth(91)
        tableCellText = tableCell.AddText()
        tableCellText.AddContent(" ")

        tableCell = tableRow.AddCell()
        tableCell.Width = New RelativeWidth(9)
        tableCellText = tableCell.AddText()
        tableCellText.Style = New Infragistics.Documents.Reports.Report.Text.Style(New Infragistics.Documents.Reports.Graphics.Font("Arial BOLD", 10), Infragistics.Documents.Reports.Graphics.Brushes.DeepSkyBlue)
        tableCellText.AddContent(strSalesOrderNum)

        'Blank Row
        tableRow = tableheader.AddRow()
        tableCell = tableRow.AddCell()
        tableCell.Width = New RelativeWidth(100)
        tableCellText = tableCell.AddText()
        tableCellText.AddContent(" ")

        tableRow = tableheader.AddRow()
        tableCell = tableRow.AddCell()
        tableCell.Width = New RelativeWidth(55)

        'add iBand
        Dim tableCellBand As IBand = tableCell.AddBand()
        'add table to iBand
        Dim tableInCell As ITable = tableCellBand.AddTable()

        'Blank Row
        tableCellTableRow = tableInCell.AddRow()
        tableCell = tableCellTableRow.AddCell()
        tableCell.Width = New RelativeWidth(100)
        tableCellText = tableCell.AddText()
        tableCellText.AddContent(" ")

        tableCellTableRow = tableInCell.AddRow()
        tableCell = tableCellTableRow.AddCell()
        tableCell.Width = New RelativeWidth(100)
        tableCellText = tableCell.AddText()
        tableCellText.AddContent(" ")

        tableCellTableRow = tableInCell.AddRow()
        tableCell = tableCellTableRow.AddCell()
        tableCell.Width = New RelativeWidth(100)
        tableCellText = tableCell.AddText()
        tableCellText.Style = New Infragistics.Documents.Reports.Report.Text.Style(New Infragistics.Documents.Reports.Graphics.Font("Arial BOLD", 22), Infragistics.Documents.Reports.Graphics.Brushes.Black)
        tableCellText.AddContent("Sales Order Acknowledgement")

        'end of table in cell

        tableCell = tableRow.AddCell()
        tableCell.Width = New RelativeWidth(15)
        tableCellText = tableCell.AddText()
        tableCellText.AddContent(" ")

        'new table in cell
        tableCell = tableRow.AddCell()
        tableCell.Width = New RelativeWidth(30)
        'add iBand
        tableCellBand = tableCell.AddBand()
        'add table to iBand
        tableInCell = tableCellBand.AddTable()


        tableCellTableRow = tableInCell.AddRow()
        tableCell = tableCellTableRow.AddCell()
        tableCell.Width = New RelativeWidth(100)

        'The relevent Logo should be stored on the path specified in wfo_config, where DataItemName = TeleSalesInvoiceLogoPath
        'get logo path

        Dim tableCellImage = tableCell.AddImage(New Infragistics.Documents.Reports.Graphics.Image(System.Drawing.Image.FromFile(Server.MapPath("~/App_Themes/Images/Steripack_Logo.jpeg"))))


        'Blank Row
        tableCellTableRow = tableInCell.AddRow()
        tableCell = tableCellTableRow.AddCell()
        tableCell.Width = New RelativeWidth(100)
        tableCellText = tableCell.AddText()
        tableCellText.AddContent(" ")
        'Blank Row
        tableCellTableRow = tableInCell.AddRow()
        tableCell = tableCellTableRow.AddCell()
        tableCell.Width = New RelativeWidth(100)
        tableCellText = tableCell.AddText()
        tableCellText.AddContent(" ")

        '---------------------------------------------------------------------------------------------

        'Blank Row
        tableRow = tableheader.AddRow()
        tableCell = tableRow.AddCell()
        tableCell.Width = New RelativeWidth(100)
        tableCellText = tableCell.AddText()
        tableCellText.AddContent(" ")

        '---------------------------------------------------------------------------------------------------
        Dim tableCellHeaderPatternGrey As New Infragistics.Documents.Reports.Report.Table.TableCellPattern()
        tableCellHeaderPatternGrey.Background = New Background(Infragistics.Documents.Reports.Graphics.Brushes.LightGray)




        tableRowA = tableheader.AddRow()
        'Invoice to
        tableCell = tableRowA.AddCell()
        tableCell.Width = New RelativeWidth(45)

        'add iBand to this cell
        tableCellBand = tableCell.AddBand()
        'add table to iBand
        tableInCell = tableCellBand.AddTable()

        tableRow = tableInCell.AddRow()
        tableCell = tableRow.AddCell()
        tableCellHeaderPatternGrey.Apply(tableCell)
        tableCell.Width = New RelativeWidth(20)
        tableCellText = tableCell.AddText()
        tableCellText.Style = New Infragistics.Documents.Reports.Report.Text.Style(New Infragistics.Documents.Reports.Graphics.Font("Arial BOLD", 10), Infragistics.Documents.Reports.Graphics.Brushes.Black)
        tableCellText.Alignment = New TextAlignment(Alignment.Left, Alignment.Middle)
        tableCellText.AddContent("Sold-To: ")

        tableCell = tableRow.AddCell()
        tableCellHeaderPatternGrey.Apply(tableCell)
        tableCell.Width = New RelativeWidth(80)
        tableCellText = tableCell.AddText()
        tableCellText.Style = New Infragistics.Documents.Reports.Report.Text.Style(New Infragistics.Documents.Reports.Graphics.Font("Arial BOLD", 10), Infragistics.Documents.Reports.Graphics.Brushes.Black)
        tableCellText.Alignment = New TextAlignment(Alignment.Left, Alignment.Middle)
        tableCellText.AddContent(dtCustomerItems.Rows(0).Item("SoldTo_Code").ToString)

        'add blank row
        tableRow = tableInCell.AddRow()
        tableCell = tableRow.AddCell()
        tableCell.Width = New RelativeWidth(100)
        tableCellText = tableCell.AddText()
        tableCellText.AddContent(" ")


        tableRow = tableInCell.AddRow()
        tableCell = tableRow.AddCell()
        tableCell.Width = New RelativeWidth(100)
        tableCellText = tableCell.AddText()
        tableCellText.Style = New Infragistics.Documents.Reports.Report.Text.Style(New Infragistics.Documents.Reports.Graphics.Font("Arial", 10), Infragistics.Documents.Reports.Graphics.Brushes.Black)
        tableCellText.Alignment = New TextAlignment(Alignment.Left, Alignment.Middle)
        tableCellText.AddContent(dtCustomerItems.Rows(0).Item("SoldTo_Address").ToString)

        'add blank dividing cell
        tableCell = tableRowA.AddCell()
        tableCell.Width = New RelativeWidth(10)
        tableCellText = tableCell.AddText()
        tableCellText.AddContent(" ")


        tableCell = tableRowA.AddCell()
        tableCell.Width = New RelativeWidth(45)
        'add iBand to this cell
        tableCellBand = tableCell.AddBand()
        'add table to iBand
        tableInCell = tableCellBand.AddTable()
        tableInCell.ApplyPattern(tablePattern)

        tableRow = tableInCell.AddRow()
        tableCell = tableRow.AddCell()

        tableCellHeaderPattern.Apply(tableCell)
        tableCell.Width = New RelativeWidth(50)
        tableCellText = tableCell.AddText()
        tableCellText.Style = New Infragistics.Documents.Reports.Report.Text.Style(New Infragistics.Documents.Reports.Graphics.Font("Arial BOLD", 10), Infragistics.Documents.Reports.Graphics.Brushes.Black)
        tableCellText.Alignment = New TextAlignment(Alignment.Left, Alignment.Middle)
        tableCellText.AddContent("Attention:")

        tableCell = tableRow.AddCell()
        tableCellPattern.Apply(tableCell)
        tableCell.Width = New RelativeWidth(50)
        tableCellText = tableCell.AddText()
        tableCellText.Style = New Infragistics.Documents.Reports.Report.Text.Style(New Infragistics.Documents.Reports.Graphics.Font("Arial BOLD", 10), Infragistics.Documents.Reports.Graphics.Brushes.Black)
        tableCellText.Alignment = New TextAlignment(Alignment.Left, Alignment.Middle)
        If Not IsDBNull(dtCustomerItems.Rows(0).Item("SoldTo_ContactName")) Then
            strAttention = dtCustomerItems.Rows(0).Item("SoldTo_ContactName").ToString
            tableCellText.AddContent(strAttention)
        Else
            tableCellText.AddContent(" ")
        End If


        tableRow = tableInCell.AddRow()
        tableCell = tableRow.AddCell()
        tableCellHeaderPattern.Apply(tableCell)
        tableCell.Width = New RelativeWidth(50)
        tableCellText = tableCell.AddText()
        tableCellText.Style = New Infragistics.Documents.Reports.Report.Text.Style(New Infragistics.Documents.Reports.Graphics.Font("Arial BOLD", 10), Infragistics.Documents.Reports.Graphics.Brushes.Black)
        tableCellText.Alignment = New TextAlignment(Alignment.Left, Alignment.Bottom)
        tableCellText.AddContent("Fax No:")

        tableCell = tableRow.AddCell()
        tableCellPattern.Apply(tableCell)
        tableCell.Width = New RelativeWidth(50)
        tableCellText = tableCell.AddText()
        tableCellText.Style = New Infragistics.Documents.Reports.Report.Text.Style(New Infragistics.Documents.Reports.Graphics.Font("Arial BOLD", 10), Infragistics.Documents.Reports.Graphics.Brushes.Black)
        tableCellText.Alignment = New TextAlignment(Alignment.Left, Alignment.Middle)
        tableCellText.AddContent(dtCustomerItems.Rows(0).Item("SoldTo_FaxNum").ToString)

        tableRow = tableInCell.AddRow()
        tableCell = tableRow.AddCell()
        tableCellHeaderPattern.Apply(tableCell)
        tableCell.Width = New RelativeWidth(50)
        tableCellText = tableCell.AddText()
        tableCellText.Style = New Infragistics.Documents.Reports.Report.Text.Style(New Infragistics.Documents.Reports.Graphics.Font("Arial BOLD", 10), Infragistics.Documents.Reports.Graphics.Brushes.Black)
        tableCellText.Alignment = New TextAlignment(Alignment.Left, Alignment.Bottom)
        tableCellText.AddContent("Sales Order No:")

        tableCell = tableRow.AddCell()
        tableCellPattern.Apply(tableCell)
        tableCell.Width = New RelativeWidth(50)
        tableCellText = tableCell.AddText()
        tableCellText.Style = New Infragistics.Documents.Reports.Report.Text.Style(New Infragistics.Documents.Reports.Graphics.Font("Arial BOLD", 10), Infragistics.Documents.Reports.Graphics.Brushes.Black)
        tableCellText.Alignment = New TextAlignment(Alignment.Left, Alignment.Middle)
        tableCellText.AddContent(strSalesOrderNum)


        tableRow = tableInCell.AddRow()
        tableCell = tableRow.AddCell()
        tableCellHeaderPattern.Apply(tableCell)
        tableCell.Width = New RelativeWidth(50)
        tableCellText = tableCell.AddText()
        tableCellText.Style = New Infragistics.Documents.Reports.Report.Text.Style(New Infragistics.Documents.Reports.Graphics.Font("Arial BOLD", 10), Infragistics.Documents.Reports.Graphics.Brushes.Black)
        tableCellText.Alignment = New TextAlignment(Alignment.Left, Alignment.Bottom)
        tableCellText.AddContent("Credit Terms")


        tableCell = tableRow.AddCell()
        tableCellPattern.Apply(tableCell)
        tableCell.Width = New RelativeWidth(50)
        tableCellText = tableCell.AddText()
        tableCellText.Style = New Infragistics.Documents.Reports.Report.Text.Style(New Infragistics.Documents.Reports.Graphics.Font("Arial BOLD", 10), Infragistics.Documents.Reports.Graphics.Brushes.Black)
        tableCellText.Alignment = New TextAlignment(Alignment.Left, Alignment.Middle)
        If Not IsDBNull(dtCustomerItems.Rows(0).Item("SoldTo_Terms")) Then
            strCreditTerms = dtCustomerItems.Rows(0).Item("SoldTo_Terms").ToString
            tableCellText.AddContent(strCreditTerms)
        Else
            tableCellText.AddContent(" ")
        End If

        tableRow = tableInCell.AddRow()
        tableCell = tableRow.AddCell()
        tableCellHeaderPattern.Apply(tableCell)
        tableCell.Width = New RelativeWidth(50)
        tableCellText = tableCell.AddText()
        tableCellText.Style = New Infragistics.Documents.Reports.Report.Text.Style(New Infragistics.Documents.Reports.Graphics.Font("Arial BOLD", 10), Infragistics.Documents.Reports.Graphics.Brushes.Black)
        tableCellText.Alignment = New TextAlignment(Alignment.Left, Alignment.Bottom)
        tableCellText.AddContent("Purchase Order No:")

        tableCell = tableRow.AddCell()
        tableCellPattern.Apply(tableCell)
        tableCell.Width = New RelativeWidth(50)
        tableCellText = tableCell.AddText()
        tableCellText.Style = New Infragistics.Documents.Reports.Report.Text.Style(New Infragistics.Documents.Reports.Graphics.Font("Arial BOLD", 10), Infragistics.Documents.Reports.Graphics.Brushes.Black)
        tableCellText.Alignment = New TextAlignment(Alignment.Left, Alignment.Middle)
        If Not IsDBNull(dtCustomerItems.Rows(0).Item("Other_CustPO")) Then
            strCustomerPONum = dtCustomerItems.Rows(0).Item("Other_CustPO").ToString
            tableCellText.AddContent(strCustomerPONum)
        Else
            tableCellText.AddContent(" ")
        End If


        tableRow = tableInCell.AddRow()
        tableCell = tableRow.AddCell()
        tableCellHeaderPattern.Apply(tableCell)
        tableCell.Width = New RelativeWidth(50)
        tableCellText = tableCell.AddText()
        tableCellText.Style = New Infragistics.Documents.Reports.Report.Text.Style(New Infragistics.Documents.Reports.Graphics.Font("Arial BOLD", 10), Infragistics.Documents.Reports.Graphics.Brushes.Black)
        tableCellText.Alignment = New TextAlignment(Alignment.Left, Alignment.Bottom)
        tableCellText.AddContent("Delivery Terms:")

        tableCell = tableRow.AddCell()
        tableCellPattern.Apply(tableCell)
        tableCell.Width = New RelativeWidth(50)
        tableCellText = tableCell.AddText()
        tableCellText.Style = New Infragistics.Documents.Reports.Report.Text.Style(New Infragistics.Documents.Reports.Graphics.Font("Arial BOLD", 10), Infragistics.Documents.Reports.Graphics.Brushes.Black)
        tableCellText.Alignment = New TextAlignment(Alignment.Left, Alignment.Middle)
        tableCellText.AddContent(dtCustomerItems.Rows(0).Item("Other_DeliveryTerms").ToString)

        tableRow = tableInCell.AddRow()
        tableCell = tableRow.AddCell()
        tableCellHeaderPattern.Apply(tableCell)
        tableCell.Width = New RelativeWidth(50)
        tableCellText = tableCell.AddText()
        tableCellText.Style = New Infragistics.Documents.Reports.Report.Text.Style(New Infragistics.Documents.Reports.Graphics.Font("Arial BOLD", 10), Infragistics.Documents.Reports.Graphics.Brushes.Black)
        tableCellText.Alignment = New TextAlignment(Alignment.Left, Alignment.Bottom)
        tableCellText.AddContent("Order Date:")

        tableCell = tableRow.AddCell()
        tableCellPattern.Apply(tableCell)
        tableCell.Width = New RelativeWidth(50)
        tableCellText = tableCell.AddText()
        tableCellText.Style = New Infragistics.Documents.Reports.Report.Text.Style(New Infragistics.Documents.Reports.Graphics.Font("Arial BOLD", 10), Infragistics.Documents.Reports.Graphics.Brushes.Black)
        tableCellText.Alignment = New TextAlignment(Alignment.Left, Alignment.Middle)
        tableCellText.AddContent(dtCustomerItems.Rows(0).Item("DeliverTo_OrderDate").ToString)

        'Blank Row
        tableRow = tableheader.AddRow()
        tableCell = tableRow.AddCell()
        tableCell.Width = New RelativeWidth(100)
        tableCellText = tableCell.AddText()
        tableCellText.AddContent(" ")

        '---------------------------------------------------------------------------------------------------------------


        tableRowA = tableheader.AddRow()
        'Ship to
        tableCell = tableRowA.AddCell()
        tableCell.Width = New RelativeWidth(45)

        'add iBand to this cell
        tableCellBand = tableCell.AddBand()
        'add table to iBand
        tableInCell = tableCellBand.AddTable()


        tableRow = tableInCell.AddRow()
        tableCell = tableRow.AddCell()
        tableCellHeaderPatternGrey.Apply(tableCell)
        tableCell.Width = New RelativeWidth(20)
        tableCellText = tableCell.AddText()
        tableCellText.Style = New Infragistics.Documents.Reports.Report.Text.Style(New Infragistics.Documents.Reports.Graphics.Font("Arial BOLD", 10), Infragistics.Documents.Reports.Graphics.Brushes.Black)
        tableCellText.Alignment = New TextAlignment(Alignment.Left, Alignment.Middle)
        tableCellText.AddContent("Invoice-To: ")

        tableCell = tableRow.AddCell()
        tableCellHeaderPatternGrey.Apply(tableCell)
        tableCell.Width = New RelativeWidth(80)
        tableCellText = tableCell.AddText()
        tableCellText.Style = New Infragistics.Documents.Reports.Report.Text.Style(New Infragistics.Documents.Reports.Graphics.Font("Arial BOLD", 10), Infragistics.Documents.Reports.Graphics.Brushes.Black)
        tableCellText.Alignment = New TextAlignment(Alignment.Left, Alignment.Middle)
        tableCellText.AddContent(dtCustomerItems.Rows(0).Item("BillTo_Code").ToString)

        'add blank row
        tableRow = tableInCell.AddRow()
        tableCell = tableRow.AddCell()
        tableCell.Width = New RelativeWidth(100)
        tableCellText = tableCell.AddText()
        tableCellText.AddContent(" ")


        tableRow = tableInCell.AddRow()
        tableCell = tableRow.AddCell()
        tableCell.Width = New RelativeWidth(100)
        tableCellText = tableCell.AddText()
        tableCellText.Style = New Infragistics.Documents.Reports.Report.Text.Style(New Infragistics.Documents.Reports.Graphics.Font("Arial", 10), Infragistics.Documents.Reports.Graphics.Brushes.Black)
        tableCellText.Alignment = New TextAlignment(Alignment.Left, Alignment.Middle)
        'tableCellText.AddContent(dtCustomerItems.Rows(0).Item("BillTo_Address").ToString)


        'add blank dividing cell
        tableCell = tableRowA.AddCell()
        tableCell.Width = New RelativeWidth(10)
        tableCellText = tableCell.AddText()
        tableCellText.AddContent(" ")

        'Shop to

        tableCell = tableRowA.AddCell()
        tableCell.Width = New RelativeWidth(45)

        'add iBand to this cell
        tableCellBand = tableCell.AddBand()
        'add table to iBand
        tableInCell = tableCellBand.AddTable()

        tableRow = tableInCell.AddRow()
        tableCell = tableRow.AddCell()
        tableCellHeaderPatternGrey.Apply(tableCell)
        tableCell.Width = New RelativeWidth(20)
        tableCellText = tableCell.AddText()
        tableCellText.Style = New Infragistics.Documents.Reports.Report.Text.Style(New Infragistics.Documents.Reports.Graphics.Font("Arial BOLD", 10), Infragistics.Documents.Reports.Graphics.Brushes.Black)
        tableCellText.Alignment = New TextAlignment(Alignment.Left, Alignment.Middle)
        tableCellText.AddContent("Ship-To:")

        tableCell = tableRow.AddCell()
        tableCellHeaderPatternGrey.Apply(tableCell)
        tableCell.Width = New RelativeWidth(80)
        tableCellText = tableCell.AddText()
        tableCellText.Style = New Infragistics.Documents.Reports.Report.Text.Style(New Infragistics.Documents.Reports.Graphics.Font("Arial BOLD", 10), Infragistics.Documents.Reports.Graphics.Brushes.Black)
        tableCellText.Alignment = New TextAlignment(Alignment.Left, Alignment.Middle)
        tableCellText.AddContent(dtCustomerItems.Rows(0).Item("DeliverTo_Code").ToString)

        'add blank row
        tableRow = tableInCell.AddRow()
        tableCell = tableRow.AddCell()
        tableCell.Width = New RelativeWidth(100)
        tableCellText = tableCell.AddText()
        tableCellText.AddContent(" ")


        tableRow = tableInCell.AddRow()
        tableCell = tableRow.AddCell()
        tableCell.Width = New RelativeWidth(100)
        tableCellText = tableCell.AddText()
        tableCellText.Style = New Infragistics.Documents.Reports.Report.Text.Style(New Infragistics.Documents.Reports.Graphics.Font("Arial", 10), Infragistics.Documents.Reports.Graphics.Brushes.Black)
        tableCellText.Alignment = New TextAlignment(Alignment.Left, Alignment.Middle)
        tableCellText.AddContent(dtCustomerItems.Rows(0).Item("DeliverTo_Address").ToString)

        'Blank Row
        tableRow = tableheader.AddRow()
        tableCell = tableRow.AddCell()
        tableCell.Width = New RelativeWidth(100)
        tableCellText = tableCell.AddText()
        tableCellText.AddContent(" ")


        'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx end of band header xxxxxxxxxxxxxxxxxxxxxxxxxxxxxx




        'create a table (for Sales data), with a table in its header for the Sales data headers
        Dim table1 As ITable = Band1.AddTable()
        Dim table1Header As ITableHeader = table1.Header
        Dim table1HeaderCell As ITableCell = table1Header.AddCell
        'Dim table1HeaderCellBand As IBand = table1HeaderCell.AddBand()
        Dim tableInHeader As ITable = table1HeaderCell.AddTable()



        'Row 3
        tableRow = tableInHeader.AddRow()



        Dim strColNames(7) As String

        strColNames(0) = "Item"
        strColNames(1) = "Customer Part"
        strColNames(2) = "Description"
        strColNames(3) = "EDA*"
        strColNames(4) = "Qty"
        strColNames(5) = "UoM"
        strColNames(6) = "Unit Price"
        strColNames(7) = "Total"



        For intArrayCount = 0 To strColNames.Length - 1
            tableCell = tableRow.AddCell
            Dim tableHeaderCellText As IText = tableCell.AddText()
            tableCellHeaderPattern.Apply(tableCell)
            tableHeaderCellText.Style = New Infragistics.Documents.Reports.Report.Text.Style(New Infragistics.Documents.Reports.Graphics.Font("Arial MT Bold", 11), Infragistics.Documents.Reports.Graphics.Brushes.Black)
            tableHeaderCellText.Alignment = New TextAlignment(Alignment.Left, Alignment.Middle)
            Select Case intArrayCount

                Case 0
                    tableCell.Width = New RelativeWidth(10)
                Case 1
                    tableCell.Width = New RelativeWidth(15)
                Case 2
                    tableCell.Width = New RelativeWidth(25)
                Case 3
                    tableCell.Width = New RelativeWidth(10)
                Case 4
                    tableCell.Width = New RelativeWidth(10)
                Case 5
                    tableCell.Width = New RelativeWidth(10)
                Case 6
                    tableCell.Width = New RelativeWidth(10)
                Case 7
                    tableCell.Width = New RelativeWidth(10)

            End Select



            tableHeaderCellText.AddContent(" " + strColNames(intArrayCount) + " ")

        Next

        table1Header.Repeat = True

        'Create table Rows
        Dim dblTotalPrice As Double = 0

        For intRowCount As Integer = 0 To dtOrderItems.Rows.Count - 1
            tableRow = table1.AddRow()
            For intArrayCount = 0 To strColNames.Length - 1
                tableCell = tableRow.AddCell()
                tableCellPattern.Apply(tableCell)
                tableCellText = tableCell.AddText()
                tableCellText.Style = New Infragistics.Documents.Reports.Report.Text.Style(New Infragistics.Documents.Reports.Graphics.Font("Arial MT", 9), Infragistics.Documents.Reports.Graphics.Brushes.Black)
                tableCellText.Alignment = New TextAlignment(Alignment.Left, Alignment.Middle)
                Select Case intArrayCount

                    Case 0
                        tableCell.Width = New RelativeWidth(10)
                        tableCellText.AddContent(" (" + (intRowCount + 1).ToString + ") " + dtOrderItems.Rows(intRowCount).Item("ProductCode").ToString + " ")
                    Case 1
                        tableCell.Width = New RelativeWidth(15)
                        tableCellText.AddContent(" " + dtOrderItems.Rows(intRowCount).Item("ProductId").ToString + " ")
                    Case 2
                        tableCell.Width = New RelativeWidth(25)
                        tableCellText.AddContent(" " + dtOrderItems.Rows(intRowCount).Item("ProductName").ToString + " ")
                    Case 3
                        tableCell.Width = New RelativeWidth(10)
                        tableCellText.AddContent(" " + dtOrderItems.Rows(intRowCount).Item("Item_DateArrival").ToString + " ")
                    Case 4
                        tableCell.Width = New RelativeWidth(10)
                        tableCellText.AddContent(" " + dtOrderItems.Rows(intRowCount).Item("QuantityRequested").ToString + " ")
                    Case 5
                        tableCell.Width = New RelativeWidth(10)
                        tableCellText.AddContent(" EA ")
                    Case 6
                        tableCell.Width = New RelativeWidth(10)
                        tableCellText.AddContent(" " + dtOrderItems.Rows(intRowCount).Item("PO_UnitPrice").ToString + " ")
                    Case 7
                        tableCell.Width = New RelativeWidth(10)
                        tableCellText.AddContent(" " + dtOrderItems.Rows(intRowCount).Item("PO_TotalPrice").ToString + " ")

                End Select

            Next
            dblTotalPrice = dblTotalPrice + dtOrderItems.Rows(intRowCount).Item("PO_TotalPrice")
        Next

        'Now add service items to the output 'grid'
        For intRowCount As Integer = 0 To dtServiceItems.Rows.Count - 1
            tableRow = table1.AddRow()
            For intArrayCount = 0 To strColNames.Length - 1
                tableCell = tableRow.AddCell()
                tableCellPattern.Apply(tableCell)
                tableCellText = tableCell.AddText()
                tableCellText.Style = New Infragistics.Documents.Reports.Report.Text.Style(New Infragistics.Documents.Reports.Graphics.Font("Arial MT", 9), Infragistics.Documents.Reports.Graphics.Brushes.Black)
                tableCellText.Alignment = New TextAlignment(Alignment.Left, Alignment.Middle)
                Select Case intArrayCount

                    Case 0
                        tableCell.Width = New RelativeWidth(10)
                        tableCellText.AddContent(" ")
                    Case 1
                        tableCell.Width = New RelativeWidth(15)
                        tableCellText.AddContent(" " + dtServiceItems.Rows(intRowCount).Item("ProductCode").ToString + " ")
                    Case 2
                        tableCell.Width = New RelativeWidth(25)
                        tableCellText.AddContent(" " + dtServiceItems.Rows(intRowCount).Item("ProductName").ToString + " ")
                    Case 3
                        tableCell.Width = New RelativeWidth(10)
                        tableCellText.AddContent(" " + dtServiceItems.Rows(intRowCount).Item(" ").ToString + " ")
                    Case 4
                        tableCell.Width = New RelativeWidth(10)
                        tableCellText.AddContent(" " + dtServiceItems.Rows(intRowCount).Item("Weight").ToString + " ")
                    Case 5
                        tableCell.Width = New RelativeWidth(10)
                        tableCellText.AddContent("  ")
                    Case 6
                        tableCell.Width = New RelativeWidth(10)
                        tableCellText.AddContent(" " + dtServiceItems.Rows(intRowCount).Item("UnitPrice").ToString + " ")
                    Case 7
                        tableCell.Width = New RelativeWidth(10)
                        tableCellText.AddContent(" " + dtServiceItems.Rows(intRowCount).Item("TotalExclVAT").ToString + " ")

                End Select

            Next
            dblTotalPrice = dblTotalPrice + dtServiceItems.Rows(intRowCount).Item("TotalExclVAT")
        Next

        Dim table0Foot As ITable = Band1.AddTable()
        'Blank Row
        tableRow = table0Foot.AddRow()
        tableCell = tableRow.AddCell()
        tableCell.Width = New RelativeWidth(100)
        tableCellText = tableCell.AddText()
        tableCellText.AddContent(" ")
        'Blank Row
        tableRow = table0Foot.AddRow()
        tableCell = tableRow.AddCell()
        tableCell.Width = New RelativeWidth(100)
        tableCellText = tableCell.AddText()
        tableCellText.AddContent(" ")
        'Blank Row
        tableRow = table0Foot.AddRow()
        tableCell = tableRow.AddCell()
        tableCell.Width = New RelativeWidth(100)
        tableCellText = tableCell.AddText()
        tableCellText.AddContent(" ")



        Dim table1Foot As ITable = Band1.AddTable()
        table1Foot.ApplyPattern(tablePattern)

        'add row 1 to tableInFooter
        tableRow = table1Foot.AddRow()

        tableCell = tableRow.AddCell()
        tableCellPattern.Apply(tableCell)
        tableCell.Width = New RelativeWidth(75)
        tableCellText = tableCell.AddText()
        tableCellText.Alignment = New TextAlignment(Alignment.Center, Alignment.Middle)
        tableCellText.AddContent(" ")

        tableCell = tableRow.AddCell()
        tableCellHeaderPattern.Apply(tableCell)
        tableCell.Width = New RelativeWidth(13)
        tableCellText = tableCell.AddText()
        tableCellText.Alignment = New TextAlignment(Alignment.Center, Alignment.Middle)
        tableCellText.AddContent("Total EUR")

        tableCell = tableRow.AddCell()
        tableCellHeaderPattern.Apply(tableCell)
        tableCell.Width = New RelativeWidth(12)
        tableCellText = tableCell.AddText()
        tableCellText.Alignment = New TextAlignment(Alignment.Center, Alignment.Middle)
        tableCellText.AddContent(dblTotalPrice)

        'add New Table
        Dim table2Foot As ITable = Band1.AddTable()
        table2Foot.ApplyPattern(tablePattern)
        tableRow = table2Foot.AddRow()

        'blank cell
        tableCell = tableRow.AddCell()
        tableCell.Width = New RelativeWidth(50)
        tableCellText = tableCell.AddText()
        tableCellText.Alignment = New TextAlignment(Alignment.Left, Alignment.Middle)
        tableCellText.AddContent(" ")

        tableCell = tableRow.AddCell()
        tableCell.Width = New RelativeWidth(12)
        tableCellText = tableCell.AddText()
        tableCellText.Style = New Infragistics.Documents.Reports.Report.Text.Style _
                (New Infragistics.Documents.Reports.Graphics.Font("Arial Bold", 10), Infragistics.Documents.Reports.Graphics.Brushes.Black)
        tableCellText.Alignment = New TextAlignment(Alignment.Left, Alignment.Top)
        tableCellText.AddContent("Signed:")

        tableCell = tableRow.AddCell()
        tableCell.Width = New RelativeWidth(18)
        tableCellText = tableCell.AddText()
        tableCellText.Alignment = New TextAlignment(Alignment.Left, Alignment.Middle)
        tableCellText.AddContent(" ")

        tableCell = tableRow.AddCell()
        tableCell.Width = New RelativeWidth(10)
        tableCellText = tableCell.AddText()
        tableCellText.Style = New Infragistics.Documents.Reports.Report.Text.Style _
                (New Infragistics.Documents.Reports.Graphics.Font("Arial Bold", 10), Infragistics.Documents.Reports.Graphics.Brushes.Black)
        tableCellText.Alignment = New TextAlignment(Alignment.Left, Alignment.Top)
        tableCellText.AddContent("Date:")

        tableCell = tableRow.AddCell()
        tableCell.Width = New RelativeWidth(10)
        tableCellText = tableCell.AddText()
        tableCellText.Alignment = New TextAlignment(Alignment.Left, Alignment.Middle)
        tableCellText.AddContent(" ")

        'Blank Row
        tableRow = table2Foot.AddRow()
        tableCell = tableRow.AddCell()
        tableCell.Width = New RelativeWidth(100)
        tableCellText = tableCell.AddText()
        tableCellText.AddContent(" ")

        'Add new table
        Dim table3Foot As ITable = Band1.AddTable()
        table3Foot.ApplyPattern(tablePattern)
        tableRow = table3Foot.AddRow()

        tableCell = tableRow.AddCell()
        tableCell.Width = New RelativeWidth(15)
        tableCellText = tableCell.AddText()
        tableCellText.Style = New Infragistics.Documents.Reports.Report.Text.Style _
                (New Infragistics.Documents.Reports.Graphics.Font("Arial Bold", 10), Infragistics.Documents.Reports.Graphics.Brushes.Black)
        tableCellText.Alignment = New TextAlignment(Alignment.Left, Alignment.Top)
        tableCellText.AddContent("Comments:")

        tableCell = tableRow.AddCell()
        tableCell.Width = New RelativeWidth(85)
        tableCellText = tableCell.AddText()
        tableCellText.Style = New Infragistics.Documents.Reports.Report.Text.Style _
                (New Infragistics.Documents.Reports.Graphics.Font("Arial Bold", 10), Infragistics.Documents.Reports.Graphics.Brushes.Black)
        tableCellText.Alignment = New TextAlignment(Alignment.Left, Alignment.Top)
        If Not IsDBNull(dtCustomerItems.Rows(0).Item("Other_Comment")) Then
            strComments = dtCustomerItems.Rows(0).Item("Other_Comment").ToString
            tableCellText.AddContent(strComments)
        Else
            tableCellText.AddContent(" ")
        End If


        'Blank Row
        tableRow = table3Foot.AddRow()
        tableCell = tableRow.AddCell()
        tableCell.Width = New RelativeWidth(100)
        tableCellText = tableCell.AddText()
        tableCellText.AddContent(" ")

        'Add new table
        Dim table4Foot As ITable = Band1.AddTable()
        table4Foot.ApplyPattern(BlankTablePattern)

        'Blank Row
        tableRow = table4Foot.AddRow()
        tableCell = tableRow.AddCell()
        tableCell.Width = New RelativeWidth(100)
        tableCellText = tableCell.AddText()
        tableCellText.AddContent(" ")

        tableRow = table4Foot.AddRow()
        tableCell = tableRow.AddCell()
        tableCell.Width = New RelativeWidth(100)
        tableCellText = tableCell.AddText()
        tableCellText.Style = New Infragistics.Documents.Reports.Report.Text.Style _
        (New Infragistics.Documents.Reports.Graphics.Font("Arial Bold", 8), Infragistics.Documents.Reports.Graphics.Brushes.Black)
        tableCellText.AddContent(" E&OE (Errors & Omissions excepted)")

        tableRow = table4Foot.AddRow()
        tableCell = tableRow.AddCell()
        tableCell.Width = New RelativeWidth(100)
        tableCellText = tableCell.AddText()
        tableCellText.Style = New Infragistics.Documents.Reports.Report.Text.Style _
        (New Infragistics.Documents.Reports.Graphics.Font("Arial Bold", 8), Infragistics.Documents.Reports.Graphics.Brushes.Black)
        tableCellText.AddContent(" *EDA = Expected Date of Arrival")

        tableRow = table4Foot.AddRow()
        tableCell = tableRow.AddCell()
        tableCell.Width = New RelativeWidth(100)
        tableCellText = tableCell.AddText()
        tableCellText.Style = New Infragistics.Documents.Reports.Report.Text.Style _
        (New Infragistics.Documents.Reports.Graphics.Font("Arial Bold", 8), Infragistics.Documents.Reports.Graphics.Brushes.Black)
        tableCellText.AddContent(" Please note EDA is not confirmed until receipt of approved artwork or specification.")

        tableRow = table4Foot.AddRow()
        tableCell = tableRow.AddCell()
        tableCell.Width = New RelativeWidth(100)
        tableCellText = tableCell.AddText()
        tableCellText.Style = New Infragistics.Documents.Reports.Report.Text.Style _
        (New Infragistics.Documents.Reports.Graphics.Font("Arial Bold", 8), Infragistics.Documents.Reports.Graphics.Brushes.Black)
        tableCellText.AddContent(" This order acknowledgement is subject to out Terms and Conditions of Sale and Delivery as detailed on our website www.steripackgroup.com")

        Dim Band1Footer As Infragistics.Documents.Reports.Report.Band.IBandFooter = Band1.Footer
        Dim tableFooter As ITable = Band1Footer.AddTable

        Band1Footer.Repeat = True

        tableRow = tableFooter.AddRow
        tableCell = tableRow.AddCell
        Dim SectionFooterCellBand As IBand = tableCell.AddBand()
        Dim tableInFooter As ITable = SectionFooterCellBand.AddTable()
        tableInFooter.ApplyPattern(BlankTablePattern)

        tableRow = tableInFooter.AddRow()
        tableCell = tableRow.AddCell()
        tableCell.Width = New RelativeWidth(50)
        tableCellText = tableCell.AddText()
        tableCellText.AddContent(" ")

        tableCell = tableRow.AddCell()
        tableCell.Width = New RelativeWidth(49)
        tableCellText = tableCell.AddText()
        tableCellText.Style = New Infragistics.Documents.Reports.Report.Text.Style _
                 (New Infragistics.Documents.Reports.Graphics.Font("Arial Bold", 6), Infragistics.Documents.Reports.Graphics.Brushes.Black)
        tableCellText.Alignment = New TextAlignment(Alignment.Right, Alignment.Middle)

        Dim objBPA As New BPADotNetCommonFunctions.VT_CommonFunctions.CommonFunctions

        'The relevent footer should be stored on the path specified in wfo_config, where DataItemName = CoCCompanyAddressFilePath
        'Otherwise a hard-coded default footer will be used
        'get footer text path
        Dim strFooterPath As String = objBPA.GetConfigItem("SalesOrderAckCompanyAddressFilePath")

        'read footer text file
        Dim TextLine As String = ""
        Dim strFooterTxt As String
        If System.IO.File.Exists(strFooterPath) Then
            TextLine = ""
            Dim objReader As New System.IO.StreamReader(strFooterPath)
            TextLine = TextLine & objReader.ReadToEnd
            ' replace any \n markers in the text with Newlines
            strFooterTxt = Replace(TextLine, "\n", Environment.NewLine)
        Else
            strFooterTxt = " Steripack Ltd.  " + Environment.NewLine + "Kilbeggan Road, Clara, Co. Offaly,"
            strFooterTxt = strFooterTxt + " Ireland, tel. +353 57 9331888. fax +353 57 9331887  "
            strFooterTxt = strFooterTxt + Environment.NewLine + " e-mail: info@steripackgroup.com, www.steripackgroup.com  "
            strFooterTxt = strFooterTxt + Environment.NewLine + "VAT no.: IE8214158A. Co. Registration no.214158  "

        End If

        tableCellText.AddContent(strFooterTxt)

        tableCell = tableRow.AddCell()
        tableCell.Width = New RelativeWidth(1)
        Dim tableFooterPattern As New Infragistics.Documents.Reports.Report.Table.TableCellPattern()
        tableFooterPattern.Borders.Right = New Border(New Infragistics.Documents.Reports.Graphics.Pen(Infragistics.Documents.Reports.Graphics.Colors.Black))
        tableFooterPattern.Apply(tableCell)
        tableCellText = tableCell.AddText()
        tableCellText.AddContent(" ")

        section1.PagePaddings.All = 25

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


        Dim strFileName As String = filePath + "\SalesOrderAck_User" + Session("_VT_CurrentUserId").ToString() + ".pdf"
        Dim strFileNameWeb As String = strRootPathWeb + "\Published\SalesOrderAck_User" + Session("_VT_CurrentUserId").ToString() + ".pdf"
        ' add a random (timer based) bit to the url in order to overcome IIS caching of the PDF file
        strFileNameWeb += String.Format("?t={0}", DateTime.Now.Ticks.ToString())

        If System.IO.File.Exists(strFileName) Then
            System.IO.File.Delete(strFileName)
        End If
        report.Publish(strFileName, FileFormat.PDF)

        Session("_VT_DocInIFrame") = strFileNameWeb
        Dim strDocDisplayPage As String = "~/DisplayDocPage.aspx"
        Response.Redirect(strDocDisplayPage)


    End Sub

End Class






