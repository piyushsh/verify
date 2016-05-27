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
Imports BPADotNetCommonFunctions.VT_eQOInterface.PersonnelSQL

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



Partial Class SalesOrderPricedTemplatePDF


    Inherits MyBasePage


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        '  This Sales Order Template was created for clients such as Burren Smoke House that use the old Access Database for personnel ids 

        Dim objC As New VTDBFunctions.VTDBFunctions.CustomerFunctions
        Dim strConn As String = Session("_VT_DotNetConnString")
        Dim strCleanedSoldToAddress As String = ""
        Dim strCleanedDeliveryAddress As String = ""
        Dim dsOrderItems As New DataSet
        Dim objTele As New TelesalesFunctions
        Dim objProd As New VTDBFunctions.VTDBFunctions.ProductsFunctions
        Dim objDataPreserve As New BPADotNetCommonFunctions.VT_DataPreserve.DataPreserve
        Dim objP As New VT_eQOInterface.eQOInterface
        Dim strSoldToCustomerName As String = ""
        Dim strShipToCustomerName As String = ""
        Dim strSoldToCustomerPhone As String = ""
        Dim strShipToCustomerPhone As String = ""
        Dim strSoldToCustomerFax As String = ""
        Dim strShipToCustomerFax As String = ""
        Dim intID As Integer = 16
        Dim intPersonLoggingId As Integer = 0
        Dim strPONum As String = ""
        'Dim strPersonLogEntry As String = ""
        Dim objSales As New VT_ReportFunctions.SalesDataFunctions
        Dim dtSalesOrderTable As DataTable = objSales.GetSalesOrderDetails(strConn, Me.CurrentSession.VT_SalesOrderNum)


        If dtSalesOrderTable IsNot Nothing AndAlso dtSalesOrderTable.Rows.Count > 0 Then
            strPONum = dtSalesOrderTable.Rows(0).Item("CustomerPO").ToString
        End If

        If dtSalesOrderTable IsNot Nothing AndAlso dtSalesOrderTable.Rows.Count > 0 Then
            intPersonLoggingId = dtSalesOrderTable.Rows(0).Item("PersonLoggingOrder").ToString
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

        Dim strPersonLoggingName As String = objP.GetEQOUserName(intPersonLoggingId)


        'Calculate subtotal and tax
        ''Dim rowCount As Integer
        ''Dim dblSubTotal As Double = 0.0
        ''Dim tax As Double = 0.0
        ''If dtSalesOrderTable.Rows.Count > 0 Then
        ''    For rowCount = 0 To dtSalesOrderTable.Rows.Count - 1
        ''        dblSubTotal = dblSubTotal + If(IsDBNull(dtSalesOrderTable.Rows(rowCount).Item("TotalPrice")), 0, dtSalesOrderTable.Rows(rowCount).Item("TotalPrice"))
        ''        Dim intProdId As Integer = dtSalesOrderTable.Rows(rowCount).Item("ProductId")
        ''        Dim intUoS As Integer = objProds.GetUnitOfSale(intProdId)
        ''        If intUoS = 1 Then   ' by unit
        ''            tax = tax + If(IsDBNull(dtSalesOrderTable.Rows(rowCount).Item("VATCharged")), 0, dtSalesOrderTable.Rows(rowCount).Item("VATCharged")) * dtSalesOrderTable.Rows(rowCount).Item("Quantity")
        ''        Else
        ''            tax = tax + If(IsDBNull(dtSalesOrderTable.Rows(rowCount).Item("VATCharged")), 0, dtSalesOrderTable.Rows(rowCount).Item("VATCharged")) * dtSalesOrderTable.Rows(rowCount).Item("Weight")
        ''        End If
        ''    Next
        ''End If

        Dim dblTotalPrice As Double = 0.0


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

        'Apply the pattern to the tables

        tableheader.Width = New RelativeWidth(100)
        tableheader.ApplyPattern(BlankTablePattern)

        tableRow = tableheader.AddRow()
        tableCell = tableRow.AddCell
        tableCell.Width = New RelativeWidth(100)


        'The relevent Header text should be stored on the path specified in wfo_config, where DataItemName = TeleSalesDocketHeaderTextPath
        'get header text path

        Dim strHeaderTextPath As String = objCommonFuncs.GetConfigItem("SalesOrderHeaderTextPath")
        strHeaderTextPath = Server.MapPath(strHeaderTextPath)
        Dim lineCount As Integer = 0
        'read header text file 
        Dim TextLine As String = ""
        If System.IO.File.Exists(strHeaderTextPath) Then
            lineCount = File.ReadAllLines(strHeaderTextPath).Length
            TextLine = ""
            Dim objReader As New System.IO.StreamReader(strHeaderTextPath)
            TextLine = TextLine & objReader.ReadToEnd

            tableCellText = tableCell.AddText()
            tableCellText.Style = New Infragistics.Documents.Reports.Report.Text.Style _
                     (New Infragistics.Documents.Reports.Graphics.Font("Arial Bold", 8), Infragistics.Documents.Reports.Graphics.Brushes.Black)
            tableCellText.Alignment = New TextAlignment(Alignment.Center, Alignment.Middle)
            tableCellText.AddContent(TextLine)

            'Create TableInHeader  Row x (A blank row) 
            tableRow = tableheader.AddRow()
            tableCell = tableRow.AddCell()
            tableCell.Width = New RelativeWidth(100)
            tableCellText = tableCell.AddText()
            tableCellText.AddContent(" ")
        End If

        tableRow = tableheader.AddRow()
        tableCell = tableRow.AddCell()
        tableCell.Width = New RelativeWidth(100)
        tableCellText = tableCell.AddText()
        tableCellText.AddContent(" ")

        tableRow = tableheader.AddRow()

        'The relevent Logo should be stored on the path specified in wfo_config, where DataItemName = TeleSalesInvoiceLogoPath
        'get logo path

        Dim strImg_Name As String = objCommonFuncs.GetConfigItem("TeleSalesInvoiceLogoPath")
        strImg_Name = Server.MapPath(strImg_Name)

        tableCell = tableRow.AddCell()
        tableCell.Width = New RelativeWidth(63)
        ' tableBlankCellPattern.Apply(tableCell)
        'ResizeImage("sources\verify_reports", "Sirkoski_Logo1.jpg", 0.3)
        'Dim tableCellImage = tableCell.AddImage(New Infragistics.Documents.Reports.Graphics.Image(System.Drawing.Image.FromFile("c:\sources\verify_reports\Sirkoski_Logo1.jpg")))
        Dim tableCellImage = tableCell.AddImage(New Infragistics.Documents.Reports.Graphics.Image(System.Drawing.Image.FromFile(strImg_Name)))

        'Add cell in row
        tableCell = tableRow.AddCell()
        tableCell.Width = New RelativeWidth(5)
        tableCellText = tableCell.AddText()
        tableCellText.AddContent(" ")


        'Add cell in row
        tableCell = tableRow.AddCell()
        tableCell.Width = New RelativeWidth(32)
        'add iBand to this cell
        Dim tableCellBand As IBand = tableCell.AddBand()
        Dim tableInCell As ITable = tableCellBand.AddTable()
        tableRow = tableInCell.AddRow()
        tableCell = tableRow.AddCell()
        tableCellText = tableCell.AddText()
        tableCellText.Style = New Infragistics.Documents.Reports.Report.Text.Style(New Infragistics.Documents.Reports.Graphics.Font("Arial BOLD", 20), Infragistics.Documents.Reports.Graphics.Brushes.Black)
        tableCellText.Alignment = New TextAlignment(Alignment.Left, Alignment.Middle)
        tableCellText.AddContent("Sales Order")


        'Create TableInHeader  Row x (A blank row) 
        tableRow = tableInCell.AddRow()
        tableCell = tableRow.AddCell()
        tableCell.Width = New RelativeWidth(100)
        tableCellText = tableCell.AddText()
        tableCellText.AddContent(" ")


        tableRow = tableInCell.AddRow()
        tableCell = tableRow.AddCell()
        tableCellText = tableCell.AddText()
        tableCellText.Style = New Infragistics.Documents.Reports.Report.Text.Style(New Infragistics.Documents.Reports.Graphics.Font("Arial BOLD", 11), Infragistics.Documents.Reports.Graphics.Brushes.Black)
        tableCellText.Alignment = New TextAlignment(Alignment.Left, Alignment.Middle)
        tableCellText.AddContent(" Purchase Order No.")


        tableCell = tableRow.AddCell()
        tableCellText = tableCell.AddText()
        tableCellText.Style = New Infragistics.Documents.Reports.Report.Text.Style(New Infragistics.Documents.Reports.Graphics.Font("Arial BOLD", 11), Infragistics.Documents.Reports.Graphics.Brushes.Black)
        tableCellText.Alignment = New TextAlignment(Alignment.Left, Alignment.Middle)
        tableCellText.AddContent(Trim(Me.CurrentSession.VT_SalesOrderNum))

        tableRow = tableInCell.AddRow()
        tableCell = tableRow.AddCell()
        tableCell.Width = New RelativeWidth(40)
        tableCellText = tableCell.AddText()
        tableCellText.Style = New Infragistics.Documents.Reports.Report.Text.Style(New Infragistics.Documents.Reports.Graphics.Font("Arial", 11), Infragistics.Documents.Reports.Graphics.Brushes.Black)
        tableCellText.Alignment = New TextAlignment(Alignment.Left, Alignment.Middle)
        tableCellText.AddContent("Delivery Date:")


        tableCell = tableRow.AddCell()
        tableCellText = tableCell.AddText()
        tableCell.Width = New RelativeWidth(60)
        tableCellText.Alignment = New TextAlignment(Alignment.Left, Alignment.Middle)
        tableCellText.Style = New Infragistics.Documents.Reports.Report.Text.Style(New Infragistics.Documents.Reports.Graphics.Font("Arial", 11), Infragistics.Documents.Reports.Graphics.Brushes.Black)
        tableCellText.AddContent(Format(dtSalesOrderTable.Rows(0).Item("ExpectedArrivalDate"), "D"))

        'Create TableInHeader  Row x (A blank row) 
        tableRow = tableInCell.AddRow()
        tableCell = tableRow.AddCell()
        tableCell.Width = New RelativeWidth(100)
        tableCellText = tableCell.AddText()
        tableCellText.Style = New Infragistics.Documents.Reports.Report.Text.Style(New Infragistics.Documents.Reports.Graphics.Font("Arial", 5), Infragistics.Documents.Reports.Graphics.Brushes.Black)
        tableCellText.AddContent(" ")

        tableRow = tableInCell.AddRow()
        tableCell = tableRow.AddCell()
        tableCell.Width = New RelativeWidth(40)
        tableCellText = tableCell.AddText()
        tableCellText.Style = New Infragistics.Documents.Reports.Report.Text.Style(New Infragistics.Documents.Reports.Graphics.Font("Arial", 11), Infragistics.Documents.Reports.Graphics.Brushes.Black)
        tableCellText.Alignment = New TextAlignment(Alignment.Left, Alignment.Middle)
        tableCellText.AddContent("Order Date:")


        tableCell = tableRow.AddCell()
        tableCellText = tableCell.AddText()
        tableCell.Width = New RelativeWidth(60)
        tableCellText.Alignment = New TextAlignment(Alignment.Left, Alignment.Middle)
        tableCellText.Style = New Infragistics.Documents.Reports.Report.Text.Style(New Infragistics.Documents.Reports.Graphics.Font("Arial", 11), Infragistics.Documents.Reports.Graphics.Brushes.Black)
        tableCellText.AddContent(Format(dtSalesOrderTable.Rows(0).Item("DateCreated"), "D"))

        'Create TableInHeader  Row x (A blank row) 
        tableRow = tableInCell.AddRow()
        tableCell = tableRow.AddCell()
        tableCell.Width = New RelativeWidth(100)
        tableCellText = tableCell.AddText()
        tableCellText.Style = New Infragistics.Documents.Reports.Report.Text.Style(New Infragistics.Documents.Reports.Graphics.Font("Arial", 5), Infragistics.Documents.Reports.Graphics.Brushes.Black)
        tableCellText.AddContent(" ")

        tableRow = tableInCell.AddRow()
        tableCell = tableRow.AddCell()
        tableCell.Width = New RelativeWidth(40)
        tableCellText = tableCell.AddText()
        tableCellText.Style = New Infragistics.Documents.Reports.Report.Text.Style(New Infragistics.Documents.Reports.Graphics.Font("Arial", 11), Infragistics.Documents.Reports.Graphics.Brushes.Black)
        tableCellText.Alignment = New TextAlignment(Alignment.Left, Alignment.Bottom)
        tableCellText.AddContent("Requested Dispatch Date:")


        tableCell = tableRow.AddCell()
        tableCellText = tableCell.AddText()
        tableCell.Width = New RelativeWidth(60)
        tableCellText.Alignment = New TextAlignment(Alignment.Left, Alignment.Bottom)
        tableCellText.Style = New Infragistics.Documents.Reports.Report.Text.Style(New Infragistics.Documents.Reports.Graphics.Font("Arial", 11), Infragistics.Documents.Reports.Graphics.Brushes.Black)
        tableCellText.AddContent(Format(dtSalesOrderTable.Rows(0).Item("RequestedDeliveryDate"), "D"))

        tableRow = tableInCell.AddRow()
        tableCell = tableRow.AddCell()
        tableCell.Width = New RelativeWidth(100)
        tableCellText = tableCell.AddText()
        tableCellText.Style = New Infragistics.Documents.Reports.Report.Text.Style(New Infragistics.Documents.Reports.Graphics.Font("Arial", 5), Infragistics.Documents.Reports.Graphics.Brushes.Black)
        tableCellText.AddContent(" ")

        tableRow = tableInCell.AddRow()
        tableCell = tableRow.AddCell()
        tableCell.Width = New RelativeWidth(40)
        tableCellText = tableCell.AddText()
        tableCellText.Style = New Infragistics.Documents.Reports.Report.Text.Style(New Infragistics.Documents.Reports.Graphics.Font("Arial", 11), Infragistics.Documents.Reports.Graphics.Brushes.Black)
        tableCellText.Alignment = New TextAlignment(Alignment.Left, Alignment.Middle)
        tableCellText.AddContent("PO #:")


        tableCell = tableRow.AddCell()
        tableCellText = tableCell.AddText()
        tableCell.Width = New RelativeWidth(60)
        tableCellText.Alignment = New TextAlignment(Alignment.Left, Alignment.Middle)
        tableCellText.Style = New Infragistics.Documents.Reports.Report.Text.Style(New Infragistics.Documents.Reports.Graphics.Font("Arial", 11), Infragistics.Documents.Reports.Graphics.Brushes.Black)
        tableCellText.AddContent(Format(strPONum))

        tableRow = tableInCell.AddRow()
        tableCell = tableRow.AddCell()
        tableCell.Width = New RelativeWidth(40)
        tableCellText = tableCell.AddText()
        tableCellText.Style = New Infragistics.Documents.Reports.Report.Text.Style(New Infragistics.Documents.Reports.Graphics.Font("Arial", 11), Infragistics.Documents.Reports.Graphics.Brushes.Black)
        tableCellText.Alignment = New TextAlignment(Alignment.Left, Alignment.Middle)
        tableCellText.AddContent("Processed By:")

        tableCell = tableRow.AddCell()
        tableCellText = tableCell.AddText()
        tableCell.Width = New RelativeWidth(60)
        tableCellText.Alignment = New TextAlignment(Alignment.Left, Alignment.Bottom)
        tableCellText.Style = New Infragistics.Documents.Reports.Report.Text.Style(New Infragistics.Documents.Reports.Graphics.Font("Arial", 11), Infragistics.Documents.Reports.Graphics.Brushes.Black)
        tableCellText.AddContent(Format(strPersonLoggingName))


        'Create TableInHeader  Row x (A blank row) 
        tableRow = tableInCell.AddRow()
        tableCell = tableRow.AddCell()
        tableCell.Width = New RelativeWidth(100)
        tableCellText = tableCell.AddText()
        tableCellText.AddContent(" ")


        'Create TableInHeader  Row x (A blank row) 
        tableRow = tableInCell.AddRow()
        tableCell = tableRow.AddCell()
        tableCell.Width = New RelativeWidth(100)
        tableCellText = tableCell.AddText()
        tableCellText.AddContent(" ")

        ' Get a reference to the section's PageNumbering object
        Dim PageNo As Infragistics.Documents.Reports.Report.Section.PageNumbering = section1.PageNumbering
        PageNo.Style = New Infragistics.Documents.Reports.Report.Text.Style(New Infragistics.Documents.Reports.Graphics.Font("Arial", 11), Infragistics.Documents.Reports.Graphics.Brushes.Black)
        PageNo.Template = "PAGE [Page #] OF [TotalPages]"
        PageNo.SkipFirst = False
        PageNo.Alignment.Horizontal = Infragistics.Documents.Reports.Report.Alignment.Right
        PageNo.Alignment.Vertical = Infragistics.Documents.Reports.Report.Alignment.Top
        PageNo.OffsetY = 200
        PageNo.OffsetX = -112

        'Create TableInHeader  Row x (A blank row) 
        tableRow = tableheader.AddRow()
        tableCell = tableRow.AddCell()
        tableCell.Width = New RelativeWidth(100)
        tableCellText = tableCell.AddText()
        tableCellText.AddContent(" ")

        'Create TableInHeader  Row x (A blank row) 
        tableRow = tableheader.AddRow()
        tableCell = tableRow.AddCell()
        tableCell.Width = New RelativeWidth(100)
        tableCellText = tableCell.AddText()
        tableCellText.AddContent(" ")


        ''Create TableInHeader Row x
        tableRow = tableheader.AddRow()
        tableCell = tableRow.AddCell()
        tableCell.Width = New RelativeWidth(31)
        tableCellText = tableCell.AddText()
        tableCellText.Style = New Infragistics.Documents.Reports.Report.Text.Style(New Infragistics.Documents.Reports.Graphics.Font("Arial Bold", 12), Infragistics.Documents.Reports.Graphics.Brushes.Black)
        tableCellText.Alignment = New TextAlignment(Alignment.Left, Alignment.Middle)
        tableCellText.AddContent("Sold To:")

        tableCell = tableRow.AddCell()
        tableCell.Width = New RelativeWidth(6)
        tableCellText = tableCell.AddText()
        tableCellText.AddContent(" ")

        tableCell = tableRow.AddCell()
        tableCell.Width = New RelativeWidth(35)
        tableCellText = tableCell.AddText()
        tableCellText.Style = New Infragistics.Documents.Reports.Report.Text.Style(New Infragistics.Documents.Reports.Graphics.Font("Arial Bold", 12), Infragistics.Documents.Reports.Graphics.Brushes.Black)
        tableCellText.Alignment = New TextAlignment(Alignment.Left, Alignment.Middle)
        tableCellText.AddContent("Ship To:")


        tableCell = tableRow.AddCell()
        tableCell.Width = New RelativeWidth(23)
        tableCellText = tableCell.AddText()
        tableCellText.AddContent(" ")


        tableRow = tableheader.AddRow()

        tableCell = tableRow.AddCell()
        tableCell.Width = New RelativeWidth(31)
        tableCellText = tableCell.AddText()
        tableCellText.Style = New Infragistics.Documents.Reports.Report.Text.Style(New Infragistics.Documents.Reports.Graphics.Font("Arial MT Bold", 10), Infragistics.Documents.Reports.Graphics.Brushes.Black)
        tableCellText.Alignment = New TextAlignment(Alignment.Left, Alignment.Middle)
        tableCellText.AddContent(strSoldToCustomerName)

        tableCell = tableRow.AddCell()
        tableCell.Width = New RelativeWidth(6)
        tableCellText = tableCell.AddText()
        tableCellText.AddContent(" ")

        tableCell = tableRow.AddCell()
        tableCell.Width = New RelativeWidth(35)
        tableCellText = tableCell.AddText()
        tableCellText.Style = New Infragistics.Documents.Reports.Report.Text.Style(New Infragistics.Documents.Reports.Graphics.Font("Arial MT Bold", 10), Infragistics.Documents.Reports.Graphics.Brushes.Black)
        tableCellText.Alignment = New TextAlignment(Alignment.Left, Alignment.Middle)

        tableCellText.AddContent(strShipToCustomerName)


        tableCell = tableRow.AddCell()
        tableCell.Width = New RelativeWidth(23)
        tableCellText = tableCell.AddText()
        tableCellText.AddContent(" ")


        ''Create TableInHeader Row x
        tableRow = tableheader.AddRow()

        tableCell = tableRow.AddCell()
        tableCell.Width = New RelativeWidth(31)
        tableCellText = tableCell.AddText()
        tableCellText.Style = New Infragistics.Documents.Reports.Report.Text.Style(New Infragistics.Documents.Reports.Graphics.Font("Arial", 10), Infragistics.Documents.Reports.Graphics.Brushes.Black)
        tableCellText.Alignment = New TextAlignment(Alignment.Left, Alignment.Middle)
        tableCellText.AddContent(strCleanedSoldToAddress)

        tableCell = tableRow.AddCell()
        tableCell.Width = New RelativeWidth(6)
        tableCellText = tableCell.AddText()
        tableCellText.AddContent(" ")

        tableCell = tableRow.AddCell()
        tableCell.Width = New RelativeWidth(35)
        tableCellText = tableCell.AddText()
        tableCellText.Style = New Infragistics.Documents.Reports.Report.Text.Style(New Infragistics.Documents.Reports.Graphics.Font("Arial", 10), Infragistics.Documents.Reports.Graphics.Brushes.Black)
        tableCellText.Alignment = New TextAlignment(Alignment.Left, Alignment.Middle)
        tableCellText.AddContent(strCleanedDeliveryAddress)

        tableCell = tableRow.AddCell()
        tableCell.Width = New RelativeWidth(23)
        tableCellText = tableCell.AddText()
        tableCellText.AddContent(" ")

        ''Create TableInHeader Row 
        tableRow = tableheader.AddRow()

        tableCell = tableRow.AddCell()
        tableCell.Width = New RelativeWidth(5)
        tableCellText = tableCell.AddText()
        tableCellText.Alignment = New TextAlignment(Alignment.Left, Alignment.Middle)
        tableCellText.AddContent("Tel. ")

        tableCell = tableRow.AddCell()
        tableCell.Width = New RelativeWidth(26)
        tableCellText = tableCell.AddText()
        tableCellText.Alignment = New TextAlignment(Alignment.Left, Alignment.Middle)
        tableCellText.AddContent(strSoldToCustomerPhone)

        tableCell = tableRow.AddCell()
        tableCell.Width = New RelativeWidth(6)
        tableCellText = tableCell.AddText()
        tableCellText.AddContent(" ")

        tableCell = tableRow.AddCell()
        tableCell.Width = New RelativeWidth(5)
        tableCellText = tableCell.AddText()
        tableCellText.Alignment = New TextAlignment(Alignment.Left, Alignment.Middle)
        tableCellText.AddContent("Tel. ")

        tableCell = tableRow.AddCell()
        tableCell.Width = New RelativeWidth(35)
        tableCellText = tableCell.AddText()
        tableCellText.Alignment = New TextAlignment(Alignment.Left, Alignment.Middle)
        tableCellText.AddContent(strShipToCustomerPhone)

        tableCell = tableRow.AddCell()
        tableCell.Width = New RelativeWidth(23)
        tableCellText = tableCell.AddText()
        tableCellText.AddContent(" ")

        'Create new row
        tableRow = tableheader.AddRow()

        tableCell = tableRow.AddCell()
        tableCell.Width = New RelativeWidth(5)
        tableCellText = tableCell.AddText()
        tableCellText.Alignment = New TextAlignment(Alignment.Left, Alignment.Middle)
        tableCellText.AddContent("Fax. ")


        tableCell = tableRow.AddCell()
        tableCell.Width = New RelativeWidth(26)
        tableCellText = tableCell.AddText()
        tableCellText.Alignment = New TextAlignment(Alignment.Left, Alignment.Middle)
        tableCellText.AddContent(strSoldToCustomerFax)

        tableCell = tableRow.AddCell()
        tableCell.Width = New RelativeWidth(6)
        tableCellText = tableCell.AddText()
        tableCellText.AddContent(" ")

        tableCell = tableRow.AddCell()
        tableCell.Width = New RelativeWidth(5)
        tableCellText = tableCell.AddText()
        tableCellText.Alignment = New TextAlignment(Alignment.Left, Alignment.Middle)
        tableCellText.AddContent("Fax. ")

        tableCell = tableRow.AddCell()
        tableCell.Width = New RelativeWidth(35)
        tableCellText = tableCell.AddText()
        tableCellText.Alignment = New TextAlignment(Alignment.Left, Alignment.Middle)
        tableCellText.AddContent(strShipToCustomerFax)

        tableCell = tableRow.AddCell()
        tableCell.Width = New RelativeWidth(23)
        tableCellText = tableCell.AddText()
        tableCellText.AddContent(" ")

        'Create TableInHeader  Row x (A blank row) 
        tableRow = tableheader.AddRow()
        tableCell = tableRow.AddCell()
        tableCell.Width = New RelativeWidth(100)
        tableCellText = tableCell.AddText()
        tableCellText.AddContent(" ")

        tableRow = tableheader.AddRow()

        tableCell = tableRow.AddCell()
        tableCell.Width = New RelativeWidth(5)
        tableCellText = tableCell.AddText()
        tableCellText.Alignment = New TextAlignment(Alignment.Left, Alignment.Middle)
        tableCellText.AddContent("Status: ")


        tableCell = tableRow.AddCell()
        tableCell.Width = New RelativeWidth(30)
        tableCellText = tableCell.AddText()
        tableCellText.Alignment = New TextAlignment(Alignment.Left, Alignment.Middle)
        Dim ci As Globalization.CultureInfo
        ci = Globalization.CultureInfo.CurrentCulture
        'tableCellText.AddContent(ci.NumberFormat.CurrencySymbol())
        tableCellText.AddContent(dtSalesOrderTable.Rows(0).Item("Status").ToString)

        tableRow = tableheader.AddRow()
        tableCell = tableRow.AddCell()
        tableCell.Width = New RelativeWidth(100)
        tableCellText = tableCell.AddText()
        tableCellText.AddContent(" ")


        'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx end of band header xxxxxxxxxxxxxxxxxxxxxxxxxxxxxx




        'create a table (for invoice data), with a table in its header for the invoice data headers
        Dim table1 As ITable = Band1.AddTable()
        Dim table1Header As ITableHeader = table1.Header
        Dim table1HeaderCell As ITableCell = table1Header.AddCell
        'Dim table1HeaderCellBand As IBand = table1HeaderCell.AddBand()
        Dim tableInHeader As ITable = table1HeaderCell.AddTable()

        Dim blnUsePricing As Boolean = Me.CurrentSession.VT_blnInclPricesOnDocket

        'Row 3
        tableRow = tableInHeader.AddRow()

        ' Create a new pattern for the header cells.
        Dim tableCellHeaderPattern As New Infragistics.Documents.Reports.Report.Table.TableCellPattern()
        tableCellHeaderPattern.Background = New Background(Infragistics.Documents.Reports.Graphics.Brushes.LightGray)
        tableCellHeaderPattern.Borders = New Borders(New Infragistics.Documents.Reports.Graphics.Pen(Infragistics.Documents.Reports.Graphics.Colors.Black))

        Dim strColNames(7) As String
        strColNames(0) = "Item No"
        strColNames(1) = "Product"
        strColNames(2) = "Product Code"
        strColNames(3) = "Qty"
        strColNames(4) = "Wgt"
        strColNames(5) = "Status"

        ' If blnUsePricing Then
        strColNames(6) = "Unit Price"
        ' Else
        'strColNames(7) = ""
        '  End If

        '   If blnUsePricing Then
        strColNames(7) = "Total"
        '   Else
        'strColNames(8) = ""
        '   End If



        For intArrayCount = 0 To strColNames.Length - 1
            tableCell = tableRow.AddCell
            Dim tableHeaderCellText As IText = tableCell.AddText()
            tableCellHeaderPattern.Apply(tableCell)
            tableHeaderCellText.Style = New Infragistics.Documents.Reports.Report.Text.Style(New Infragistics.Documents.Reports.Graphics.Font("Arial MT Bold", 11), Infragistics.Documents.Reports.Graphics.Brushes.Black)
            tableHeaderCellText.Alignment = New TextAlignment(Alignment.Center, Alignment.Middle)
            Select Case intArrayCount
                Case 0
                    tableCell.Width = New RelativeWidth(5)
                Case 1
                    tableCell.Width = New RelativeWidth(20)
                Case 2
                    tableCell.Width = New RelativeWidth(20)
                Case 3
                    tableCell.Width = New RelativeWidth(5)
                Case 4
                    tableCell.Width = New RelativeWidth(5)
                Case 5
                    tableCell.Width = New RelativeWidth(15)
                Case 6
                    tableCell.Width = New RelativeWidth(15)
                Case 7
                    tableCell.Width = New RelativeWidth(15)
                    tableCell.Background = New Background(Infragistics.Documents.Reports.Graphics.Brushes.DarkSlateGray)
                    tableHeaderCellText.Style = New Infragistics.Documents.Reports.Report.Text.Style(New Infragistics.Documents.Reports.Graphics.Font("Arial MT Bold", 11), Infragistics.Documents.Reports.Graphics.Brushes.White)

            End Select

            tableHeaderCellText.AddContent(" " + strColNames(intArrayCount) + " ")

        Next

        table1Header.Repeat = True

        'Create table Rows
        'Dim dblTotalPrice As Double = 0
        'Dim dblTotalTax As Double = 0
        ' Dim dblTotalPriceIncludingTax As Double = 0


        For intRowCount As Integer = 0 To dsOrderItems.Tables(0).Rows.Count - 1
            tableRow = table1.AddRow()
            For intArrayCount = 0 To strColNames.Length - 1
                tableCell = tableRow.AddCell()
                tableCellPattern.Apply(tableCell)
                tableCellText = tableCell.AddText()
                tableCellText.Style = New Infragistics.Documents.Reports.Report.Text.Style(New Infragistics.Documents.Reports.Graphics.Font("Arial MT", 9), Infragistics.Documents.Reports.Graphics.Brushes.Black)
                tableCellText.Alignment = New TextAlignment(Alignment.Left, Alignment.Middle)
                Select Case intArrayCount
                    ' Case 0
                    ' tableCell.Width = New RelativeWidth(6)
                    'tableCellText.AddContent(" " + dtRep.Rows(intRowCount).Item("Item_No").ToString + " ")
                    Case 0
                        tableCell.Width = New RelativeWidth(5)
                        tableCellText.AddContent(" " + (intRowCount + 1).ToString + " ")
                    Case 1
                        tableCell.Width = New RelativeWidth(20)
                        tableCellText.AddContent(" " + objProds.GetProductName(dsOrderItems.Tables(0).Rows(intRowCount).Item("ProductId")) + " ")
                    Case 2
                        tableCell.Width = New RelativeWidth(20)
                        tableCellText.AddContent(" " + objProds.GetProductCode(dsOrderItems.Tables(0).Rows(intRowCount).Item("ProductId")) + " ")
                    Case 3
                        tableCell.Width = New RelativeWidth(5)
                        tableCellText.AddContent(" " + dsOrderItems.Tables(0).Rows(intRowCount).Item("QuantityRequested").ToString + " ")
                    Case 4
                        tableCell.Width = New RelativeWidth(5)
                        tableCellText.AddContent(" " + dsOrderItems.Tables(0).Rows(intRowCount).Item("WeightRequested").ToString + " ")
                    Case 5
                        tableCell.Width = New RelativeWidth(15)
                        tableCellText.AddContent(" " + dsOrderItems.Tables(0).Rows(intRowCount).Item("Status").ToString + " ")
                    Case 6
                        tableCell.Width = New RelativeWidth(15)
                        tableCellText.AddContent(" " + Format(dsOrderItems.Tables(0).Rows(intRowCount).Item("UnitPrice"), "0.00") + " ")
                        ' dblTotalPrice += CDbl(dsOrderItems.Tables(0).Rows(intRowCount).Item("UnitPrice"))
                    Case 7
                        tableCell.Width = New RelativeWidth(15)
                        tableCellText.AddContent(" " + Format(dsOrderItems.Tables(0).Rows(intRowCount).Item("TotalPrice"), "0.00") + " ")
                        dblTotalPrice += CDbl(dsOrderItems.Tables(0).Rows(intRowCount).Item("TotalPrice"))

                End Select

            Next
        Next


        ' Dim table1Foot As ITable = Band1.AddTable()

        ' table1Foot.ApplyPattern(BlankTablePattern)

        'add row 1 to tableInFooter
        tableRow = table1.AddRow()

        tableCell = tableRow.AddCell()

        tableCell.Width = New RelativeWidth(85)
        tableCellText = tableCell.AddText()
        tableCellText.Alignment = New TextAlignment(Alignment.Center, Alignment.Middle)
        tableCellText.AddContent(" ")

        tableCell = tableRow.AddCell()
        tableCellPattern.Apply(tableCell)
        tableCell.Width = New RelativeWidth(15)

        tableCellText = tableCell.AddText()
        tableCellText.Alignment = New TextAlignment(Alignment.Center, Alignment.Middle)

        ' If blnUsePricing Then
        tableCellText.AddContent("Total Price")
        ' End If

        tableRow = table1.AddRow()

        tableCell = tableRow.AddCell()
        tableCell.Width = New RelativeWidth(85)
        tableCellText = tableCell.AddText()
        tableCellText.Alignment = New TextAlignment(Alignment.Center, Alignment.Middle)
        tableCellText.AddContent(" ")


        tableCell = tableRow.AddCell()
        tableCell.Width = New RelativeWidth(15)
        tableCellPattern.Apply(tableCell)
        tableCellText = tableCell.AddText()
        tableCellText.Style = New Infragistics.Documents.Reports.Report.Text.Style _
             (New Infragistics.Documents.Reports.Graphics.Font("Arial Bold", 9), Infragistics.Documents.Reports.Graphics.Brushes.Black)
        tableCellText.Alignment = New TextAlignment(Alignment.Right, Alignment.Middle)
        ' If blnUsePricing Then
        tableCellText.AddContent(dblTotalPrice.ToString("0.00"))
        '  End If



        ' '' add a row for totals
        ''tableRow = table1.AddRow()
        ''For intArrayCount = 0 To strColNames.Length - 1
        ''    tableCell = tableRow.AddCell()
        ''    tableCellPattern.Apply(tableCell)
        ''    tableCellText = tableCell.AddText()
        ''    tableCellText.Style = New Infragistics.Documents.Reports.Report.Text.Style(New Infragistics.Documents.Reports.Graphics.Font("Arial MT", 9), Infragistics.Documents.Reports.Graphics.Brushes.Black)
        ''    tableCellText.Alignment = New TextAlignment(Alignment.Left, Alignment.Middle)
        ''    Select Case intArrayCount
        ''        ' Case 0
        ''        ' tableCell.Width = New RelativeWidth(6)
        ''        'tableCellText.AddContent(" " + dtRep.Rows(intRowCount).Item("Item_No").ToString + " ")
        ''        Case 0
        ''            tableCell.Width = New RelativeWidth(9)
        ''            tableCellText.AddContent("Totals ")
        ''        Case 1
        ''            tableCell.Width = New RelativeWidth(31)
        ''            tableCellText.AddContent(" ")
        ''        Case 2
        ''            tableCell.Width = New RelativeWidth(11)
        ''            tableCellText.AddContent(" ")
        ''        Case 3
        ''            tableCell.Width = New RelativeWidth(7)
        ''            tableCellText.AddContent(" ")
        ''        Case 4
        ''            tableCell.Width = New RelativeWidth(7)
        ''            tableCellText.AddContent(" ")
        ''        Case 5
        ''            tableCell.Width = New RelativeWidth(10)
        ''            tableCellText.AddContent(" " + Format(dblTotalPrice, "0.00") + " ")
        ''        Case 6
        ''            tableCell.Width = New RelativeWidth(9)
        ''            tableCellText.AddContent(" " + Format(dblTotalTax, "0.00") + " ")
        ''        Case 7
        ''            tableCell.Width = New RelativeWidth(9)
        ''            tableCellText.AddContent(" " + Format(dblTotalPriceIncludingTax, "0.00") + " ")
        ''        Case 8
        ''            tableCell.Width = New RelativeWidth(16)
        ''            tableCellText.AddContent(" ")
        ''    End Select

        ''Next


        Dim table1Foot As ITable = Band1.AddTable()

        table1Foot.ApplyPattern(BlankTablePattern)

        'add row 1 to tableInFooter
        tableRow = table1Foot.AddRow()

        tableCell = tableRow.AddCell()
        tableCell.Width = New RelativeWidth(100)
        tableCellText = tableCell.AddText()
        tableCellText.Alignment = New TextAlignment(Alignment.Center, Alignment.Middle)
        tableCellText.AddContent(" ")


        'add row 2 to tableInFooter
        tableRow = table1Foot.AddRow()


        tableCell = tableRow.AddCell()
        tableCell.Width = New RelativeWidth(28)
        tableCellText = tableCell.AddText()
        tableCellText.Style = New Infragistics.Documents.Reports.Report.Text.Style _
                (New Infragistics.Documents.Reports.Graphics.Font("Arial Bold", 10), Infragistics.Documents.Reports.Graphics.Brushes.Black)
        tableCellText.Alignment = New TextAlignment(Alignment.Left, Alignment.Middle)
        tableCellText.AddContent("Notes: ")

        tableCell = tableRow.AddCell()
        tableCell.Width = New RelativeWidth(72)
        tableCellText = tableCell.AddText()
        tableCellText.Style = New Infragistics.Documents.Reports.Report.Text.Style _
                (New Infragistics.Documents.Reports.Graphics.Font("Arial Bold", 10), Infragistics.Documents.Reports.Graphics.Brushes.Black)
        tableCellText.Alignment = New TextAlignment(Alignment.Left, Alignment.Top)
        tableCellText.AddContent(" " + dtSalesOrderTable.Rows(0).Item("Comment").ToString)

        tableRow = table1Foot.AddRow()


        'Dim table3Foot As ITable = Band1.AddTable()

        'table1Foot.ApplyPattern(BlankTablePattern)

        'add row 1 to tableInFooter 3 with notes for Burren Smoke House
        'tableRow = table3Foot.AddRow()

        tableCell = tableRow.AddCell()
        tableCell.Width = New RelativeWidth(100)
        tableCellText = tableCell.AddText()
        tableCellText.Alignment = New TextAlignment(Alignment.Center, Alignment.Bottom)
        tableCellText.AddContent(" ")


        'add row 2 to tableInFooter

        'Dim Band1Footer As Infragistics.Documents.Reports.Report.Band.IBandFooter = Band1.Footer
        'Dim tableFooter As ITable = Band1Footer.AddTable

        'Band1Footer.Repeat = False

        tableRow = table1Foot.AddRow
        tableCell = tableRow.AddCell
        tableCell.Width = New RelativeWidth(100)

        'The relevent footer should be stored on the path specified in wfo_config, where DataItemName = TeleSalesOrderFooterPath
        'Otherwise a hard-coded default footer will be used
        'get footer text path

        Dim strFooterPathtxt As String = objCommonFuncs.GetConfigItem("TeleSalesOrderFooterPath")
        strFooterPathtxt = Server.MapPath(strFooterPathtxt)
        'read footer text file or logo
        Dim TextLineFooter As String = ""
        If System.IO.File.Exists(strFooterPathtxt) Then
            TextLineFooter = ""
            Dim objReader As New System.IO.StreamReader(strFooterPathtxt)
            TextLineFooter = TextLineFooter & objReader.ReadToEnd

            tableCellText = tableCell.AddText()
            tableCellText.Style = New Infragistics.Documents.Reports.Report.Text.Style _
                     (New Infragistics.Documents.Reports.Graphics.Font("Arial Bold", 8), Infragistics.Documents.Reports.Graphics.Brushes.Black)
            tableCellText.Alignment = New TextAlignment(Alignment.Left, Alignment.Top)
            tableCellText.AddContent(TextLineFooter)
        End If

        tableRow = table1Foot.AddRow
        tableCell = tableRow.AddCell
        tableCell.Width = New RelativeWidth(100)

        Dim strImg_NameFooter As String = objCommonFuncs.GetConfigItem("TeleSalesDocketFooterLogoPath")
        strImg_NameFooter = Server.MapPath(strImg_NameFooter)
        If System.IO.File.Exists(strImg_NameFooter) Then
            Dim tableCellFooterImage = tableCell.AddImage(New Infragistics.Documents.Reports.Graphics.Image(System.Drawing.Image.FromFile(strImg_NameFooter)))
        End If

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
        Dim strFileName As String = filePath + "\SalesOrder_User" + Session("_VT_CurrentUserId").ToString() + ".pdf"
        Dim strFileNameWeb As String = strRootPathWeb + "\Published\SalesOrder_User" + Session("_VT_CurrentUserId").ToString() + ".pdf"
        ' add a random (timer based) bit to the url in order to overcome IIS caching of the PDF file
        strFileNameWeb += String.Format("?t={0}", DateTime.Now.Ticks.ToString())

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

    Sub ResizeImage(ByVal dir As String, ByVal fileName As String, ByVal percentResize As Double)
        'following code resizes picture to fit
        Dim bm As New Bitmap("C:\" & dir & "\" & fileName)
        Dim width As Integer = bm.Width - (bm.Width * percentResize) 'image width. 
        Dim height As Integer = bm.Height - (bm.Height * percentResize)  'image height
        Dim thumb As New Bitmap(width, height)
        Dim g As Graphics = Graphics.FromImage(thumb)
        g.InterpolationMode = Drawing2D.InterpolationMode.HighQualityBicubic
        g.DrawImage(bm, New Rectangle(0, 0, width, height), New Rectangle(0, 0, bm.Width, _
bm.Height), GraphicsUnit.Pixel)
        g.Dispose()
        bm.Dispose()
        'image path.
        thumb.Save("C:\" & dir & "\" & fileName, _
System.Drawing.Imaging.ImageFormat.Jpeg) 'can use any image format 
        thumb.Dispose()

    End Sub

End Class






