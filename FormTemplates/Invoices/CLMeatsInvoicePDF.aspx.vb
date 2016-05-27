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


Partial Class CLMeatsInvoiceTemplatePDF

    Inherits MyBasePage


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim objCommonFuncs As New BPADotNetCommonFunctions.VT_CommonFunctions.CommonFunctions
        Dim objS As New VerifyIntellisense.Sales
        Dim objD As New VerifyIntellisense.SalesDispatch
        Dim SalesOrder1 As New VerifyIntellisense.Sales.SalesOrder
        'Dim DocketItems1() As VerifyIntellisense.SalesDispatch.SalesDispatchItem
        Dim dtRep As New DataTable
        Dim dtReturns As New DataTable

        Dim strConn As String = Session("_VT_DotNetConnString")
        Dim objR As New BPADotNetCommonFunctions.VT_ReportFunctions.TransactionDataFunctions
        Dim objSales As New BPADotNetCommonFunctions.VT_ReportFunctions.SalesDataFunctions
        Dim objC As New VTDBFunctions.VTDBFunctions.CustomerFunctions
        Dim objT As New VTDBFunctions.VTDBFunctions.TelesalesFunctions
        Dim objProds As New VTDBFunctions.VTDBFunctions.ProductsFunctions
        Dim objTraceData As New VTDBFunctions.VTDBFunctions.TraceDataFunctions

        Dim strWhereClause As String

        'Get Dispatch Transactions

        strWhereClause = " WHERE trc_transactions.DocketNum = '" & Me.CurrentSession.VT_DocketNumber & "' "
        'strWhereClause = " WHERE trc_transactions.DocketNum = '20130423142642' "
        strWhereClause = strWhereClause + " AND (trc_Transactions.TransactionType = '3' OR trc_Transactions.TransactionType = '6') "
        strWhereClause = strWhereClause + " AND (trc_Transactions.PaymentType <> '2') "

        dtRep = objR.GetTransactionsForWhereClause(strWhereClause, strConn)


        'Calculate subtotal and tax
        Dim rowCount As Integer
        Dim dblSubTotal As Double = 0.0
        Dim tax As Double = 0.0
        If dtRep.Rows.Count > 0 Then
            For rowCount = 0 To dtRep.Rows.Count - 1
                dblSubTotal = dblSubTotal + If(IsDBNull(dtRep.Rows(rowCount).Item("TotalPrice")), 0, dtRep.Rows(rowCount).Item("TotalPrice"))
                Dim intProdId As Integer = dtRep.Rows(rowCount).Item("ProductId")
                Dim intUoS As Integer = objProds.GetUnitOfSale(intProdId)
                If intUoS = 1 Then   ' by unit
                    ' drNew.Item("VAT") = Math.Round(tax, Me.CurrentSession.VT_DecimalPlaces)

                    tax = tax + If(IsDBNull(dtRep.Rows(rowCount).Item("VATCharged")), 0, dtRep.Rows(rowCount).Item("VATCharged")) * dtRep.Rows(rowCount).Item("Quantity")
                Else
                    tax = tax + If(IsDBNull(dtRep.Rows(rowCount).Item("VATCharged")), 0, dtRep.Rows(rowCount).Item("VATCharged")) * dtRep.Rows(rowCount).Item("Weight")
                End If
            Next
            '   tax = Math.Round(tax, Me.CurrentSession.VT_DecimalPlaces)
        End If

        'Add Item No column
        dtRep.Columns.Add("Item_No", GetType(Integer))
        dtRep.Columns.Add("Comments", GetType(String))

        'Clean up the DataTable columns
        Dim astrColNames(0, 1) As String
        astrColNames(0, 0) = "Item_No"
        astrColNames(0, 1) = "ADD_INCREMENTAL_INDEX"

        Dim objG As New BPADotNetCommonFunctions.VT_ReportFunctions.GeneralFunctions
        If dtRep IsNot Nothing AndAlso dtRep.Rows.Count > 0 Then
            dtRep = objG.CleanColumnFormats(dtRep, astrColNames)
        End If


        Dim intSalesOrderNumber As Integer = 0
        If dtRep.Rows.Count > 0 Then
            intSalesOrderNumber = dtRep.Rows(0).Item("SalesOrderNum")
        Else
            'If dtReturns.Rows.Count > 0 Then
            '    intSalesOrderNumber = dtReturns.Rows(0).Item("SalesOrderNum")
            'End If
        End If


        Dim dtComment As DataTable = objSales.GetSalesOrderDetails(strConn, intSalesOrderNumber)


        'Get Return Transactions
        strWhereClause = " WHERE trc_transactions.DocketNum = '" & Me.CurrentSession.VT_DocketNumber & "' "
        'strWhereClause = " WHERE trc_transactions.DocketNum = '20130423142642' "
        strWhereClause = strWhereClause + " AND (trc_Transactions.TransactionType = '3' OR trc_Transactions.TransactionType = '6') "
        strWhereClause = strWhereClause + " AND (trc_Transactions.PaymentType = '2') "

        dtReturns = objR.GetTransactionsForWhereClause(strWhereClause, strConn)

        'Calculate returns total plus tax
        Dim returns As Double = 0.0
        Dim returnsTax As Double = 0.0
        If dtReturns.Rows.Count > 0 Then
            For rowCount = 0 To dtReturns.Rows.Count - 1
                returns = returns + dtReturns.Rows(rowCount).Item("TotalPrice")
                returnsTax = returnsTax + dtReturns.Rows(rowCount).Item("VATCharged")
            Next
            ' returnsTax = Math.Round(returnsTax, Me.CurrentSession.VT_DecimalPlaces)
        End If

        dtReturns.Columns.Add("Comments", GetType(String))

        If intSalesOrderNumber > 0 Then

            Dim objRep As New VT_ReportFunctions.SalesDataFunctions
            Dim dtSalesOrder As DataTable = objRep.GetSalesOrderDetails(strConn, dtRep.Rows(0).Item("SalesOrderNum"))
            Dim dsCustDetails As DataSet = objC.GetCustomerDetailsForId(dtSalesOrder.Rows(0).Item("CustomerId"))
            Dim strCustCode As String = objC.GetCustomerRefForId(dtSalesOrder.Rows(0).Item("CustomerId"))
            Dim dtCustDetails As DataTable = dsCustDetails.Tables(0)
            'SalesOrder1 = objS.SalesOrderDetails(intSalesOrderNumber)

            Dim discount As Double = dtCustDetails.Rows(0).Item("DiscountPercent")
            discount = (discount * dblSubTotal) / 100

            'Calculate Grand Total
            Dim grandTotal As Double = 0.0
            grandTotal = dblSubTotal - returns - returnsTax + tax

            Dim strCustomerName As String = dtSalesOrder.Rows(0).Item("CustomerName")
            Dim strDeliveryName As String = dtSalesOrder.Rows(0).Item("DeliveryCustomerName")


            Dim strBillingAddress As String = dtSalesOrder.Rows(0).Item("BillingAddress")
            Dim strSplit() As String = strBillingAddress.Split(",")
            Dim strCleanedBillingAddress As String = ""
            For i = 0 To UBound(strSplit)

                If strSplit(i) <> "" AndAlso strSplit(i) <> " " Then
                    strCleanedBillingAddress = strCleanedBillingAddress + strSplit(i) + Environment.NewLine
                End If

            Next i
            'lblBillAddress.Text = strCleanedBillingAddress

            Dim strShipAddress As String = dtSalesOrder.Rows(0).Item("DeliveryCustomerAddress")
            Dim strSplitShip() As String = strShipAddress.Split(",")
            Dim strCleanedShippingAddress As String = ""
            For i = 0 To UBound(strSplitShip)

                If strSplitShip(i) <> "" AndAlso strSplitShip(i) <> " " Then
                    strCleanedShippingAddress = strCleanedShippingAddress + strSplitShip(i) + Environment.NewLine
                End If
            Next i


            'SmcN 11/08/2013 Get the Matrix Folder Id for Addtional Item Details Data for this Sales Order  ------------------------
            Dim objMatrix As New VTDBFunctions.VTDBFunctions.VTMatrixFunctions
            Dim objB As New BPADotNetCommonFunctions.VT_CommonFunctions.MatrixFunctions
            Dim dtCategories As New DataTable
            Dim intStandardNodeId As Integer

            dtCategories = objB.GetCategories(Session("_VT_DotNetConnString"), SalesOrderFormListTable, SalesOrderModuleRootNode, SalesOrderModuleCategoriesNode)

            'Filter to Find the 'Standard' category
            'Get the ID of the "Standard" node
            Dim strSearch As String = "FormName = 'Standard'"
            dtCategories = objG.SearchDataTable(strSearch, dtCategories)

            If dtCategories IsNot Nothing AndAlso dtCategories.Rows.Count > 0 Then
                intStandardNodeId = dtCategories.Rows(0).Item("FormId")
            End If

            'Get the ID of this SalesOrder (WorkflowID)
            Dim intSalesOrderID_Matrix As Integer = objMatrix.GetGenericFolderId(Session("_VT_DotNetConnString"), SalesOrderFormListTable, intStandardNodeId, intSalesOrderNumber)
            'Get the ItemDetails folder for Parent Node
            Dim intItemDetailsFolderId As Integer = objMatrix.GetGenericFolderId(Session("_VT_DotNetConnString"), SalesOrderFormListTable, intSalesOrderID_Matrix, "ItemDetails")

            'Now get all of the Item details data under this node
            Dim strErr As String
            Dim dtSalesExtraData As New DataTable
            dtSalesExtraData.Columns.Add("ParentId")
            dtSalesExtraData.Rows.Add()
            dtSalesExtraData.Rows(0).Item("ParentId") = intItemDetailsFolderId

            dtSalesExtraData = objMatrix.GetMatrixUnNamedChildIDs(Session("_VT_DotNetConnString"), VTMatrixFunctions.Sales, dtSalesExtraData, "ParentId")

            Dim astrItems(2) As String
            astrItems(0) = "ProductUnitPriceOnTheDay"
            astrItems(1) = "ProductId"
            astrItems(2) = "SalesOrderItemId"

            strErr = objMatrix.GetBlockOfDataItemsNON_FORM(Session("_VT_DotNetConnString"), VTMatrixFunctions.Sales, astrItems, dtSalesExtraData, "FormId")
            '
            ' get the Docket's service items and calculate the freight
            '
            Dim dblFreight As Double = 0.0
            ' read the name of the special Freight product from the config
            Dim strFreightProductName As String = objCommonFuncs.GetConfigItem("FreightProductName ")
            Dim dsDocketServiceItems As DataSet = objTraceData.GetDocketServiceItemsForDocketNumOrderId(Me.CurrentSession.VT_DocketNumber, Me.CurrentSession.VT_SalesOrderID)
            For Each drItem As DataRow In dsDocketServiceItems.Tables(0).Rows
                ' find the item's product name
                Dim strProdCode As String = objProds.GetProductName(drItem.Item("ProductId"))
                If strProdCode.Trim = strFreightProductName.Trim Then
                    ' this is a freight item so add its value to the Freight total
                    dblFreight += drItem.Item("Weight")
                End If

            Next

            grandTotal += dblFreight
            ' -----------------------------------------------------------------------------------------------------------------------


            '---- Start building the Physical Report template ----------------------

            Dim report As Report = New Report()
            Dim section1 As ISection = report.AddSection()
            ' section1.PagePaddings.All = 20
            'section1.PageSize = New PageSize(600, 1000)

            section1.PagePaddings.Top = 100
            section1.PagePaddings.Left = 10
            section1.PagePaddings.Right = 10
            section1.PagePaddings.Bottom = 25

            Dim Band1 As IBand = section1.AddBand()
            Band1.Stretch = True


            Dim bandHeader As Infragistics.Documents.Reports.Report.Band.IBandHeader = Band1.Header


            ' Cause the header to repeat on every page.
            bandHeader.Repeat = True
            ' bandHeader.Height = New Infragistics.Documents.Reports.Report.FixedHeight(20)
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
        
            'The relevent Logo should be stored on the path specified in wfo_config, where DataItemName = TeleSalesInvoiceLogoPath
            'get logo path

            Dim strImg_Name As String = objCommonFuncs.GetConfigItem("TeleSalesInvoiceLogoPath")
            strImg_Name = Server.MapPath(strImg_Name)

            'tableCell = tableRow.AddCell()
            'tableCell.Width = New RelativeWidth(100)
            'Dim tableCellImage = tableCell.AddImage(New Infragistics.Documents.Reports.Graphics.Image(System.Drawing.Image.FromFile(strImg_Name)))

            tableRow = tableheader.AddRow()
            tableCell = tableRow.AddCell()
            tableCell.Width = New RelativeWidth(100)

            tableRow = tableheader.AddRow()
            tableCell = tableRow.AddCell()
            tableCell.Width = New RelativeWidth(100)

            tableRow = tableheader.AddRow()
            tableCell = tableRow.AddCell()
            tableCell.Width = New RelativeWidth(100)
            tableCellText = tableCell.AddText()
            tableCellText.AddContent(" ")

            'Add cell in row
            tableRow = tableheader.AddRow()
            tableCell = tableRow.AddCell()
            tableCell.Width = New RelativeWidth(60)
            tableCellText = tableCell.AddText()
            tableCellText.Style = New Infragistics.Documents.Reports.Report.Text.Style(New Infragistics.Documents.Reports.Graphics.Font("Arial Bold", 20), Infragistics.Documents.Reports.Graphics.Brushes.Black)
            tableCellText.Alignment = New TextAlignment(Alignment.Left, Alignment.Middle)
            tableCellText.AddContent("INVOICE")


            'Add cell in row
            tableCell = tableRow.AddCell()
            tableCell.Width = New RelativeWidth(40)
            'add iBand to this cell
            Dim tableCellBand As IBand = tableCell.AddBand()
            Dim tableInCell As ITable = tableCellBand.AddTable()
            tableRow = tableInCell.AddRow()
            tableCell = tableRow.AddCell()
            tableCellText = tableCell.AddText()
         
            '' ''Create TableInHeader  Row x (A blank row) 
            ' ''tableRow = tableInCell.AddRow()
            ' ''tableCell = tableRow.AddCell()
            ' ''tableCell.Width = New RelativeWidth(100)
            ' ''tableCellText = tableCell.AddText()
            ' ''tableCellText.AddContent(" ")

            tableRow = tableInCell.AddRow()
            tableCell = tableRow.AddCell()
            tableCellText = tableCell.AddText()
            tableCell.Width = New RelativeWidth(60)
            tableCellText.Style = New Infragistics.Documents.Reports.Report.Text.Style(New Infragistics.Documents.Reports.Graphics.Font("Arial", 11), Infragistics.Documents.Reports.Graphics.Brushes.Black)
            tableCellText.Alignment = New TextAlignment(Alignment.Left, Alignment.Middle)
            tableCellText.AddContent("Invoice No:")


            tableCell = tableRow.AddCell()
            tableCellText = tableCell.AddText()
            tableCell.Width = New RelativeWidth(40)
            tableCellText.Style = New Infragistics.Documents.Reports.Report.Text.Style(New Infragistics.Documents.Reports.Graphics.Font("Arial", 11), Infragistics.Documents.Reports.Graphics.Brushes.Black)
            tableCellText.Alignment = New TextAlignment(Alignment.Left, Alignment.Middle)
            Dim strInvoiceNum As String = If(IsDBNull(dtRep.Rows(0).Item("InvoiceNum")), "", dtRep.Rows(0).Item("InvoiceNum"))
            If strInvoiceNum <> "" Then
                strInvoiceNum = objCommonFuncs.GetConfigItem("InvoiceNumberPrefix") + strInvoiceNum
            End If
            tableCellText.AddContent(strInvoiceNum)


            ''Create TableInHeader  Row x (A blank row) 
            'tableRow = tableInCell.AddRow()
            'tableCell = tableRow.AddCell()
            'tableCell.Width = New RelativeWidth(70)
            'tableCellText = tableCell.AddText()
            'tableCellText.AddContent(" ")

            tableRow = tableInCell.AddRow()
            tableCell = tableRow.AddCell()
            tableCell.Width = New RelativeWidth(60)
            tableCellText = tableCell.AddText()
            tableCellText.Style = New Infragistics.Documents.Reports.Report.Text.Style(New Infragistics.Documents.Reports.Graphics.Font("Arial", 11), Infragistics.Documents.Reports.Graphics.Brushes.Black)
            tableCellText.Alignment = New TextAlignment(Alignment.Left, Alignment.Middle)
            tableCellText.AddContent("Invoice Date:")


            tableCell = tableRow.AddCell()
            tableCellText = tableCell.AddText()
            tableCell.Width = New RelativeWidth(40)
            tableCellText.Alignment = New TextAlignment(Alignment.Left, Alignment.Middle)
            tableCellText.Style = New Infragistics.Documents.Reports.Report.Text.Style(New Infragistics.Documents.Reports.Graphics.Font("Arial", 11), Infragistics.Documents.Reports.Graphics.Brushes.Black)
            tableCellText.AddContent(Format(dtRep.Rows(0).Item("DateOfTransaction"), "D"))

            ''Create TableInHeader  Row x (A blank row) 
            'tableRow = tableInCell.AddRow()
            'tableCell = tableRow.AddCell()
            'tableCell.Width = New RelativeWidth(100)
            'tableCellText = tableCell.AddText()
            'tableCellText.AddContent(" ")

            tableRow = tableInCell.AddRow()
            tableCell = tableRow.AddCell()
            tableCellText = tableCell.AddText()
            tableCell.Width = New RelativeWidth(60)
            tableCellText.Style = New Infragistics.Documents.Reports.Report.Text.Style(New Infragistics.Documents.Reports.Graphics.Font("Arial", 11), Infragistics.Documents.Reports.Graphics.Brushes.Black)
            tableCellText.Alignment = New TextAlignment(Alignment.Left, Alignment.Middle)
            tableCellText.AddContent("Sales Order No:")

            tableCell = tableRow.AddCell()
            tableCellText = tableCell.AddText()
            tableCell.Width = New RelativeWidth(40)
            tableCellText.Style = New Infragistics.Documents.Reports.Report.Text.Style(New Infragistics.Documents.Reports.Graphics.Font("Arial", 11), Infragistics.Documents.Reports.Graphics.Brushes.Black)
            tableCellText.Alignment = New TextAlignment(Alignment.Left, Alignment.Middle)
            tableCellText.AddContent(dtSalesOrder.Rows(0).Item("SalesOrderNum").ToString)

            tableRow = tableInCell.AddRow()
            tableCell = tableRow.AddCell()
            tableCellText = tableCell.AddText()
            tableCell.Width = New RelativeWidth(60)
            tableCellText.Style = New Infragistics.Documents.Reports.Report.Text.Style(New Infragistics.Documents.Reports.Graphics.Font("Arial", 11), Infragistics.Documents.Reports.Graphics.Brushes.Black)
            tableCellText.Alignment = New TextAlignment(Alignment.Left, Alignment.Middle)
            tableCellText.AddContent("Account No:")

            tableCell = tableRow.AddCell()
            tableCellText = tableCell.AddText()
            tableCell.Width = New RelativeWidth(40)
            tableCellText.Style = New Infragistics.Documents.Reports.Report.Text.Style(New Infragistics.Documents.Reports.Graphics.Font("Arial", 11), Infragistics.Documents.Reports.Graphics.Brushes.Black)
            tableCellText.Alignment = New TextAlignment(Alignment.Left, Alignment.Middle)
            tableCellText.AddContent(strCustCode)

            'Create TableInHeader  Row x (A blank row) 
            tableRow = tableInCell.AddRow()
            tableCell = tableRow.AddCell()
            tableCell.Width = New RelativeWidth(100)
            tableCellText = tableCell.AddText()
            tableCellText.AddContent(" ")


            '' ''Create TableInHeader Row x
            tableRow = tableheader.AddRow()
            ' ''tableCell = tableRow.AddCell()
            ' ''tableCell.Width = New RelativeWidth(35)
            ' ''tableCellText = tableCell.AddText()
            ' ''tableCellText.Style = New Infragistics.Documents.Reports.Report.Text.Style(New Infragistics.Documents.Reports.Graphics.Font("Arial Bold", 14), Infragistics.Documents.Reports.Graphics.Brushes.Black)
            ' ''tableCellText.Alignment = New TextAlignment(Alignment.Left, Alignment.Middle)
            ' ''tableCellText.AddContent("SHIP TO:")

            tableCell = tableRow.AddCell()
            tableCell.Width = New RelativeWidth(50)
            tableCellText = tableCell.AddText()
            tableCellText.Style = New Infragistics.Documents.Reports.Report.Text.Style(New Infragistics.Documents.Reports.Graphics.Font("Arial Bold", 12), Infragistics.Documents.Reports.Graphics.Brushes.Black)
            tableCellText.Alignment = New TextAlignment(Alignment.Left, Alignment.Middle)
            tableCellText.AddContent("BILL TO:")

            ' '' ''tableCell = tableRow.AddCell()
            ' '' ''tableCell.Width = New RelativeWidth(50)
            ' '' ''tableCellText = tableCell.AddText()
            ' '' ''tableCellText.AddContent(" ")

            tableRow = tableheader.AddRow()
            tableCell = tableRow.AddCell()
            tableCell.Width = New RelativeWidth(40)
            tableCellText = tableCell.AddText()
            tableCellText.Style = New Infragistics.Documents.Reports.Report.Text.Style(New Infragistics.Documents.Reports.Graphics.Font("Arial MT Bold", 10), Infragistics.Documents.Reports.Graphics.Brushes.Black)
            tableCellText.AddContent(strCustomerName)



            ''Create TableInHeader Row x
            tableRow = tableheader.AddRow()
            tableCell = tableRow.AddCell()
            tableCell.Width = New RelativeWidth(50)
            tableCellText = tableCell.AddText()
            tableCellText.Style = New Infragistics.Documents.Reports.Report.Text.Style(New Infragistics.Documents.Reports.Graphics.Font("Arial", 10), Infragistics.Documents.Reports.Graphics.Brushes.Black)
            tableCellText.AddContent(strCleanedBillingAddress)


            'Clean up Payment terms time interval, and add to invoice date
            Dim strOutput As String = ""
            If Not IsDBNull(dtCustDetails.Rows(0).Item("PaymentTerms")) Then
                Dim strPaymentTerms = dtCustDetails.Rows(0).Item("PaymentTerms")
                Dim output As New StringBuilder("")
                For Each c As Char In strPaymentTerms
                    If Char.IsDigit(c) Then
                        output.Append(c)
                        strOutput = output.ToString
                    End If
                Next
                Dim dlbOutput As Double = Convert.ToDouble(strOutput)
                Dim dteInvoice As Date = dtRep.Rows(0).Item("DateOfTransaction")
                Dim IntervalPaymentTerms As DateInterval = strOutput
                Dim dteDueDate As DateTime = DateAdd("d", dlbOutput, dteInvoice)

                tableCell = tableRow.AddCell()
                tableCell.Width = New RelativeWidth(18)
                tableCellText = tableCell.AddText()
                tableCellText.Style = New Infragistics.Documents.Reports.Report.Text.Style(New Infragistics.Documents.Reports.Graphics.Font("Arial Bold", 12), Infragistics.Documents.Reports.Graphics.Brushes.Black)
                tableCellText.Alignment = New TextAlignment(Alignment.Left, Alignment.Bottom)
                tableCellText.AddContent(Format(dteDueDate, "D"))
            Else

                tableCell = tableRow.AddCell()
                tableCell.Width = New RelativeWidth(18)
                tableCellText = tableCell.AddText()
                tableCellText.Style = New Infragistics.Documents.Reports.Report.Text.Style(New Infragistics.Documents.Reports.Graphics.Font("Arial Bold", 10), Infragistics.Documents.Reports.Graphics.Brushes.Black)
                tableCellText.Alignment = New TextAlignment(Alignment.Left, Alignment.Bottom)
                tableCellText.AddContent(" ")
            End If


            'Create TableInHeader  Row x (A blank row) 
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

            ''Create TableInHeader Row 1
            tableRow = tableInHeader.AddRow()
            
            tableCell = tableRow.AddCell()
            tableCell.Width = New RelativeWidth(85)
            ' tableCellPattern.Apply(tableCell)
            tableCellText = tableCell.AddText()
            tableCellText.Alignment = New TextAlignment(Alignment.Center, Alignment.Middle)
            tableCellText.AddContent(" ")

            tableCell = tableRow.AddCell()
            tableCell.Width = New RelativeWidth(15)
            'tableCellPattern.Apply(tableCell)
            'tableCell.Background = New Background(Infragistics.Documents.Reports.Graphics.Brushes.LightSlateGray)
            tableCellText = tableCell.AddText()
            tableCellText.Alignment = New TextAlignment(Alignment.Center, Alignment.Middle)
            tableCellText.AddContent("Currency")

            ''Create TableInHeader Row 2
            tableRow = tableInHeader.AddRow()
            tableCell = tableRow.AddCell()
            tableCell.Width = New RelativeWidth(85)
            'tableCellPattern.Apply(tableCell)
            tableCellText = tableCell.AddText()
            tableCellText.Alignment = New TextAlignment(Alignment.Center, Alignment.Middle)
            tableCellText.AddContent(" ")

            tableCell = tableRow.AddCell()
            tableCell.Width = New RelativeWidth(15)
            ' tableCellPattern.Apply(tableCell)
            tableCellText = tableCell.AddText()
            tableCellText.Style = New Infragistics.Documents.Reports.Report.Text.Style(New Infragistics.Documents.Reports.Graphics.Font("Arial MT Bold", 9), Infragistics.Documents.Reports.Graphics.Brushes.Black)
            tableCellText.Alignment = New TextAlignment(Alignment.Center, Alignment.Middle)
            Dim ci As Globalization.CultureInfo
            ci = Globalization.CultureInfo.CurrentCulture
           
            tableCellText.AddContent(ci.NumberFormat.CurrencySymbol())
            'End If



            'Row 3
            tableRow = tableInHeader.AddRow()

            ' Create a new pattern for the header cells.
            Dim tableCellHeaderPattern As New Infragistics.Documents.Reports.Report.Table.TableCellPattern()
            tableCellHeaderPattern.Background = New Background(Infragistics.Documents.Reports.Graphics.Brushes.LightGray)
            tableCellHeaderPattern.Borders = New Borders(New Infragistics.Documents.Reports.Graphics.Pen(Infragistics.Documents.Reports.Graphics.Colors.Black))

            Dim strColNames(5) As String
            'strColNames(0) = "ITEM NO"
            strColNames(0) = "Product Name"
            strColNames(1) = "Batch Number"
            strColNames(2) = "Quantity"
            strColNames(3) = "Weight"
            strColNames(4) = "Unit Price"
            strColNames(5) = "TOTAL"

            For intArrayCount = 0 To strColNames.Length - 1
                tableCell = tableRow.AddCell
                Dim tableHeaderCellText As IText = tableCell.AddText()
                tableCellHeaderPattern.Apply(tableCell)
                tableHeaderCellText.Style = New Infragistics.Documents.Reports.Report.Text.Style(New Infragistics.Documents.Reports.Graphics.Font("Arial MT Bold", 11), Infragistics.Documents.Reports.Graphics.Brushes.Black)
                tableHeaderCellText.Alignment = New TextAlignment(Alignment.Center, Alignment.Middle)
                Select Case intArrayCount
                    'Case 0
                    '    tableCell.Width = New RelativeWidth(6)
                    Case 0
                        tableCell.Width = New RelativeWidth(25)
                    Case 1
                        tableCell.Width = New RelativeWidth(15)
                    Case 2
                        tableCell.Width = New RelativeWidth(15)
                    Case 3
                        tableCell.Width = New RelativeWidth(15)
                    Case 4
                        tableCell.Width = New RelativeWidth(15)
                    Case 5
                        tableCell.Width = New RelativeWidth(15)
                        tableCell.Background = New Background(Infragistics.Documents.Reports.Graphics.Brushes.DarkSlateGray)
                        tableHeaderCellText.Style = New Infragistics.Documents.Reports.Report.Text.Style(New Infragistics.Documents.Reports.Graphics.Font("Arial MT Bold", 11), Infragistics.Documents.Reports.Graphics.Brushes.White)
                End Select



                tableHeaderCellText.AddContent(" " + strColNames(intArrayCount) + " ")

            Next

            table1Header.Repeat = True

            'Create table Rows


            For intRowCount As Integer = 0 To dtRep.Rows.Count - 1
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
                            tableCell.Width = New RelativeWidth(25)
                            tableCellText.AddContent(" " + dtRep.Rows(intRowCount).Item("Product_Name").ToString + " ")
                            tableCellText.Alignment = New TextAlignment(Alignment.Left, Alignment.Middle)
                        Case 1
                            tableCell.Width = New RelativeWidth(15)
                            tableCellText.AddContent(" " + dtRep.Rows(intRowCount).Item("TraceCodeDesc").ToString + " ")
                            tableCellText.Alignment = New TextAlignment(Alignment.Left, Alignment.Middle)

                        Case 2
                            tableCell.Width = New RelativeWidth(15)
                            If dtRep.Rows(intRowCount).Item("Quantity").ToString = "0" Then
                                tableCellText.AddContent(" ")
                            Else
                                tableCellText.AddContent(" " + dtRep.Rows(intRowCount).Item("Quantity").ToString + " ")
                            End If
                            tableCellText.Alignment = New TextAlignment(Alignment.Right, Alignment.Middle)
                        Case 3
                            tableCell.Width = New RelativeWidth(15)
                            tableCellText.AddContent(" " + dtRep.Rows(intRowCount).Item("Weight").ToString + " ")
                            tableCellText.Alignment = New TextAlignment(Alignment.Right, Alignment.Middle)
                        Case 4
                            tableCell.Width = New RelativeWidth(15)
                            'tableCellText.AddContent(" " + dtRep.Rows(intRowCount).Item("PriceCharged").ToString("0.00") + " ")
                            tableCellText.AddContent(" " + Format(dtRep.Rows(intRowCount).Item("PriceCharged"), "0.00") + " ")
                            tableCellText.Alignment = New TextAlignment(Alignment.Right, Alignment.Middle)
                        Case 5
                            tableCell.Width = New RelativeWidth(15)
                            'tableCellText.AddContent(" " + dtRep.Rows(intRowCount).Item("TotalPrice").ToString("0.00") + " ")
                            tableCellText.AddContent(" " + Format(dtRep.Rows(intRowCount).Item("TotalPrice"), "0.00") + " ")
                            tableCellText.Alignment = New TextAlignment(Alignment.Right, Alignment.Middle)
                    End Select

                Next
            Next

            Dim table1Foot As ITable = Band1.AddTable()

            table1Foot.ApplyPattern(BlankTablePattern)

            'add row 1 to tableInFooter
            tableRow = table1Foot.AddRow()

            tableCell = tableRow.AddCell()
            tableCell.Width = New RelativeWidth(70)
            tableCellText = tableCell.AddText()
            tableCellText.Alignment = New TextAlignment(Alignment.Center, Alignment.Middle)
            tableCellText.AddContent(" ")

            tableCell = tableRow.AddCell()
            tableCell.Width = New RelativeWidth(15)
            tableCellPattern.Apply(tableCell)
            tableCellText = tableCell.AddText()
            tableCellText.Alignment = New TextAlignment(Alignment.Center, Alignment.Middle)
            tableCellText.AddContent("SubTotal")

            tableCell = tableRow.AddCell()
            tableCell.Width = New RelativeWidth(15)
            tableCellPattern.Apply(tableCell)
            tableCellText = tableCell.AddText()
            tableCellText.Style = New Infragistics.Documents.Reports.Report.Text.Style _
                 (New Infragistics.Documents.Reports.Graphics.Font("Arial Bold", 9), Infragistics.Documents.Reports.Graphics.Brushes.Black)
            tableCellText.Alignment = New TextAlignment(Alignment.Right, Alignment.Middle)
            tableCellText.AddContent(dblSubTotal.ToString("0.00"))


            'add row 2 to tableInFooter
            tableRow = table1Foot.AddRow()
            tableCell = tableRow.AddCell()
            tableCell.Width = New RelativeWidth(15)
            tableCellText = tableCell.AddText()
            tableCellText.Style = New Infragistics.Documents.Reports.Report.Text.Style _
                    (New Infragistics.Documents.Reports.Graphics.Font("Arial Bold", 10), Infragistics.Documents.Reports.Graphics.Brushes.Black)
            tableCellText.Alignment = New TextAlignment(Alignment.Left, Alignment.Middle)
            tableCellText.AddContent("Notes: ")
            ' tableRow = table1Foot.AddRow()

            tableCell = tableRow.AddCell()
            tableCell.Width = New RelativeWidth(55)
            tableCellText = tableCell.AddText()
            tableCellText.Alignment = New TextAlignment(Alignment.Center, Alignment.Middle)
            tableCellText.AddContent(" ")

            tableCell = tableRow.AddCell()
            tableCell.Width = New RelativeWidth(15)
            tableCellPattern.Apply(tableCell)
            tableCellText = tableCell.AddText()
            tableCellText.Alignment = New TextAlignment(Alignment.Center, Alignment.Middle)
            tableCellText.AddContent("Tax")

            tableCell = tableRow.AddCell()
            tableCell.Width = New RelativeWidth(15)
            tableCellPattern.Apply(tableCell)
            tableCellText = tableCell.AddText()
            tableCellText.Style = New Infragistics.Documents.Reports.Report.Text.Style _
                     (New Infragistics.Documents.Reports.Graphics.Font("Arial Bold", 9), Infragistics.Documents.Reports.Graphics.Brushes.Black)
            tableCellText.Alignment = New TextAlignment(Alignment.Right, Alignment.Middle)
            tableCellText.AddContent(tax.ToString("0.00"))

           

            'add row 6 to tableInFooter
            tableRow = table1Foot.AddRow()

            tableCell = tableRow.AddCell()
            tableCell.Width = New RelativeWidth(70)
            tableCellText = tableCell.AddText()
            tableCellText.Alignment = New TextAlignment(Alignment.Center, Alignment.Middle)
            tableCellText.AddContent(" ")

            tableCell = tableRow.AddCell()
            tableCell.Width = New RelativeWidth(15)
            tableCellPattern.Apply(tableCell)
            tableCell.Background = New Background(Infragistics.Documents.Reports.Graphics.Brushes.DarkSlateGray)
            tableCellText = tableCell.AddText()
            tableCellText.Style = New Infragistics.Documents.Reports.Report.Text.Style _
                     (New Infragistics.Documents.Reports.Graphics.Font("Arial Bold", 12), Infragistics.Documents.Reports.Graphics.Brushes.White)
            tableCellText.Alignment = New TextAlignment(Alignment.Center, Alignment.Middle)
            tableCellText.AddContent("TOTAL")

            tableCell = tableRow.AddCell()
            tableCell.Width = New RelativeWidth(15)
            tableCellPattern.Apply(tableCell)
            tableCellText = tableCell.AddText()
            tableCellText.Style = New Infragistics.Documents.Reports.Report.Text.Style _
                     (New Infragistics.Documents.Reports.Graphics.Font("Arial Bold", 9), Infragistics.Documents.Reports.Graphics.Brushes.Black)
            tableCellText.Alignment = New TextAlignment(Alignment.Right, Alignment.Middle)
            tableCellText.AddContent(Math.Round(grandTotal, 2).ToString("F2"))


            'Create TableInFooter  Row 7 (A blank row) 
            tableRow = table1Foot.AddRow()
            tableCell = tableRow.AddCell()
            tableCell.Width = New RelativeWidth(100)
            tableCellText = tableCell.AddText()
            tableCellText.AddContent(" ")


            'Create TableInFooter  Row 9 (A blank row) 
            tableRow = table1Foot.AddRow()
            tableCell = tableRow.AddCell()
            tableCell.Width = New RelativeWidth(100)
            tableCellText = tableCell.AddText()
            tableCellText.AddContent(" ")

            'Create TableInFooter  Row 10 (A blank row) 
            tableRow = table1Foot.AddRow()
            tableCell = tableRow.AddCell()
            tableCell.Width = New RelativeWidth(100)
            tableCellText = tableCell.AddText()
            tableCellText.AddContent(" ")

            '
            '
            ' hide the returns data until we work out how to handle it
            '
            '
            ''Create TableInFooter  Row 11 
            'tableRow = table1Foot.AddRow()
            'tableCell = tableRow.AddCell()
            'tableCell.Width = New RelativeWidth(100)
            'tableCellText = tableCell.AddText()
            'tableCellText.AddContent("Returns")


            If dtReturns.Rows.Count > 0 Then
                'Build the returns table
                Dim table2 As ITable = Band1.AddTable()
                table2.ApplyPattern(BlankTablePattern)
                Dim table2Header As ITableHeader = table2.Header
                'Dim table2HeaderCell As ITableCell

                'For intArrayCount = 0 To strColNames.Length - 1
                '    table2HeaderCell = table2Header.AddCell
                '    Dim table2HeaderCellText As IText = table2HeaderCell.AddText()
                '    tableCellHeaderPattern.Apply(table2HeaderCell)
                '    table2HeaderCellText.Style = New Infragistics.Documents.Reports.Report.Text.Style(New Infragistics.Documents.Reports.Graphics.Font("Arial MT Bold", 11), Infragistics.Documents.Reports.Graphics.Brushes.Black)
                '    Select Case intArrayCount
                '        'Case 0
                '        '    tableCell.Width = New RelativeWidth(6)
                '        Case 0
                '            table2HeaderCell.Width = New RelativeWidth(10)
                '        Case 1
                '            table2HeaderCell.Width = New RelativeWidth(13)
                '        Case 2
                '            table2HeaderCell.Width = New RelativeWidth(20)
                '        Case 3
                '            table2HeaderCell.Width = New RelativeWidth(24)
                '        Case 4
                '            table2HeaderCell.Width = New RelativeWidth(6)
                '        Case 5
                '            table2HeaderCell.Width = New RelativeWidth(7)
                '        Case 6
                '            table2HeaderCell.Width = New RelativeWidth(6)
                '        Case 7
                '            table2HeaderCell.Width = New RelativeWidth(6)
                '        Case 8
                '            table2HeaderCell.Width = New RelativeWidth(8)
                '            table2HeaderCell.Background = New Background(Infragistics.Documents.Reports.Graphics.Brushes.DarkSlateGray)
                '            table2HeaderCellText.Style = New Infragistics.Documents.Reports.Report.Text.Style(New Infragistics.Documents.Reports.Graphics.Font("Arial MT Bold", 11), Infragistics.Documents.Reports.Graphics.Brushes.White)
                '    End Select

                '    table2HeaderCellText.AddContent(" " + strColNames(intArrayCount))

                'Next

              
                'Create (A blank row) 
                tableRow = table2.AddRow()
                tableCell = tableRow.AddCell()
                tableCell.Width = New RelativeWidth(100)
                tableCellText = tableCell.AddText()
                tableCellText.AddContent(" ")
                'Create (A blank row) 
                tableRow = table2.AddRow()
                tableCell = tableRow.AddCell()
                tableCell.Width = New RelativeWidth(100)
                tableCellText = tableCell.AddText()
                tableCellText.AddContent(" ")


                tableRow = table2.AddRow()
                tableCell = tableRow.AddCell()
                tableCell.Width = New RelativeWidth(77)
                tableCellText = tableCell.AddText()
                tableCellText.Style = New Infragistics.Documents.Reports.Report.Text.Style _
                         (New Infragistics.Documents.Reports.Graphics.Font("Arial Bold", 9), Infragistics.Documents.Reports.Graphics.Brushes.Black)
                tableCellText.Alignment = New TextAlignment(Alignment.Left, Alignment.Middle)
                tableCellText.AddContent("CUSTOMER SIGNATURE _______________________________ ")

                tableCell = tableRow.AddCell()
                tableCell.Width = New RelativeWidth(23)
                tableCellText = tableCell.AddText()
                tableCellText.Style = New Infragistics.Documents.Reports.Report.Text.Style _
                         (New Infragistics.Documents.Reports.Graphics.Font("Arial Bold", 9), Infragistics.Documents.Reports.Graphics.Brushes.Black)
                tableCellText.Alignment = New TextAlignment(Alignment.Left, Alignment.Middle)
                tableCellText.AddContent("Thank you for your Business!")


                'Create TableInFooter  Row 9 (A blank row) 
                tableRow = table2.AddRow()
                tableCell = tableRow.AddCell()
                tableCell.Width = New RelativeWidth(100)
                tableCellText = tableCell.AddText()
                tableCellText.AddContent(" ")

                'Create TableInFooter  Row 10 (A blank row) 
                tableRow = table2.AddRow()
                tableCell = tableRow.AddCell()
                tableCell.Width = New RelativeWidth(100)
                tableCellText = tableCell.AddText()
                tableCellText.AddContent(" ")
                'add end row tableInFooter

                tableRow = table2.AddRow()
                tableCell = tableRow.AddCell()
                tableCell.Width = New RelativeWidth(100)
                tableCellText = tableCell.AddText()
                tableCellText.Style = New Infragistics.Documents.Reports.Report.Text.Style _
                         (New Infragistics.Documents.Reports.Graphics.Font("Arial Bold", 8), Infragistics.Documents.Reports.Graphics.Brushes.Black)
                tableCellText.Alignment = New TextAlignment(Alignment.Center, Alignment.Middle)

                'The relevent footer should be stored on the path specified in wfo_config, where DataItemName = TeleSalesInvoiceFooterPath
                'Otherwise a hard-coded default footer will be used
                'get footer text path

                Dim strFooterPath As String = objCommonFuncs.GetConfigItem("TeleSalesInvoiceFooterPath")

                'read footer text file
                Dim TextLine As String = ""
                If System.IO.File.Exists(strFooterPath) Then
                    TextLine = ""
                    Dim objReader As New System.IO.StreamReader(strFooterPath)
                    TextLine = TextLine & objReader.ReadToEnd
                    tableCellText.AddContent(TextLine)
                End If


            Else
               
                'Create (A blank row) 
                tableRow = table1Foot.AddRow()
                tableCell = tableRow.AddCell()
                tableCell.Width = New RelativeWidth(100)
                tableCellText = tableCell.AddText()
                tableCellText.AddContent(" ")
                'Create (A blank row) 
                tableRow = table1Foot.AddRow()
                tableCell = tableRow.AddCell()
                tableCell.Width = New RelativeWidth(100)
                tableCellText = tableCell.AddText()
                tableCellText.AddContent(" ")


                tableRow = table1Foot.AddRow()
                tableCell = tableRow.AddCell()
                tableCell.Width = New RelativeWidth(77)
                tableCellText = tableCell.AddText()
                tableCellText.Style = New Infragistics.Documents.Reports.Report.Text.Style _
                         (New Infragistics.Documents.Reports.Graphics.Font("Arial Bold", 9), Infragistics.Documents.Reports.Graphics.Brushes.Black)
                tableCellText.Alignment = New TextAlignment(Alignment.Left, Alignment.Middle)
                tableCellText.AddContent("CUSTOMER SIGNATURE _______________________________ ")

                tableCell = tableRow.AddCell()
                tableCell.Width = New RelativeWidth(23)
                tableCellText = tableCell.AddText()
                tableCellText.Style = New Infragistics.Documents.Reports.Report.Text.Style _
                         (New Infragistics.Documents.Reports.Graphics.Font("Arial Bold", 9), Infragistics.Documents.Reports.Graphics.Brushes.Black)
                tableCellText.Alignment = New TextAlignment(Alignment.Left, Alignment.Middle)
                tableCellText.AddContent("Thank you for your Business!")


                'Create TableInFooter  Row 9 (A blank row) 
                tableRow = table1Foot.AddRow()
                tableCell = tableRow.AddCell()
                tableCell.Width = New RelativeWidth(100)
                tableCellText = tableCell.AddText()
                tableCellText.AddContent(" ")

                'Create TableInFooter  Row 10 (A blank row) 
                tableRow = table1Foot.AddRow()
                tableCell = tableRow.AddCell()
                tableCell.Width = New RelativeWidth(100)
                tableCellText = tableCell.AddText()
                tableCellText.AddContent(" ")
                'add end row tableInFooter

                tableRow = table1Foot.AddRow()
                tableCell = tableRow.AddCell()
                tableCell.Width = New RelativeWidth(100)
                tableCellText = tableCell.AddText()
                tableCellText.Style = New Infragistics.Documents.Reports.Report.Text.Style _
                         (New Infragistics.Documents.Reports.Graphics.Font("Arial Bold", 8), Infragistics.Documents.Reports.Graphics.Brushes.Black)
                tableCellText.Alignment = New TextAlignment(Alignment.Center, Alignment.Middle)

                'The relevent footer should be stored on the path specified in wfo_config, where DataItemName = TeleSalesInvoiceFooterPath
                'Otherwise a hard-coded default footer will be used
                'get footer text path

                Dim strFooterPath As String = objCommonFuncs.GetConfigItem("TeleSalesInvoiceFooterPath")

                'read footer text file
                Dim TextLine As String = ""
                If System.IO.File.Exists(strFooterPath) Then
                    TextLine = ""
                    Dim objReader As New System.IO.StreamReader(strFooterPath)
                    TextLine = TextLine & objReader.ReadToEnd
                    tableCellText.AddContent(TextLine)
                End If



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
            Dim strFileName As String = filePath + "\Invoice_User" + Session("_VT_CurrentUserId").ToString() + ".pdf"
            Dim strFileNameWeb As String = strRootPathWeb + "\Published\Invoice_User" + Session("_VT_CurrentUserId").ToString() + ".pdf"
            ' add a random (timer based) bit to the url in order to overcome IIS caching of the PDF file
            strFileNameWeb += String.Format("?t={0}", DateTime.Now.Ticks.ToString())

            If System.IO.File.Exists(strFileName) Then
                System.IO.File.Delete(strFileName)
            End If
            report.Publish(strFileName, FileFormat.PDF)

            Session("_VT_DocInIFrame") = strFileNameWeb
            Dim strDocDisplayPage As String = "~/DisplayDocPage.aspx"
            Response.Redirect(strDocDisplayPage)

        End If
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






