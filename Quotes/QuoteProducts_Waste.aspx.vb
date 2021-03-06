﻿Imports BPADotNetCommonFunctions
Imports BPADotNetCommonFunctions.VT_CommonFunctions.MatrixFunctions
Imports System.Data.SqlClient
Imports System.Data



Partial Class Quotes_QuoteProducts_Waste
    Inherits MyBasePage

    Const cView = 0
    Const cReference = 1
    Const cName = 2

    Enum ShowHide
        ShowEmpty = 0
        HideEmpty = 1
        ShowAll = 2
    End Enum


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        'ddlProducts.Attributes.Add("onblur", "javascript:HandleProductOnAddPanel('" & ddlProducts.ClientID & "', '" & lblPnlProdMessage.ClientID & "')")

        cmdProdAdd.OnClientClick = String.Format("ShowAddClose('{0}','{1}')", cmdProdAdd.UniqueID, "")
        btnAddProduct.Attributes.Add("onclick", "return ShowProductAddPopup();")

        btnRemoveFromList.Attributes.Add("onclick", "return DeleteItem();")
        btnSaveClassification.Attributes.Add("onclick", "return SaveClassification();")

        If Not IsPostBack Then
            'IF the Quote ID is blank this is a new Quote so fill with default 

            Me.CurrentSession.VT_QuoteUseBillingCustomer = False

            PopulatePackageTypeCombo()

            FillData(Me.CurrentSession.VT_QuoteID)

            ShowTotals()
        End If



    End Sub

    ''' <summary>
    ''' This function validates the data on the form before it can be saved
    ''' </summary>
    ''' <returns>False if there is a Validation problem, True otherwise</returns>
    ''' <remarks></remarks>
    Public Function ValidateMe(ByVal ParamArray aParams() As Object) As String
        Dim objDisp As New VT_Display.DisplayFuncs
        Dim mpContentPlaceHolder As ContentPlaceHolder

        ValidateMe = ""

        mpContentPlaceHolder = aParams(0)

        ' sample code

        'If CType(mpContentPlaceHolder.FindControl("txtSetupLm"), TextBox).Text = "" Then
        '    ValidateMe = "You must enter the Setup LM value"
        '    Exit Function
        'End If



    End Function

    ''' <summary>
    ''' This function is for any special data items stored after the ScrapeFormData in the SavePageData function
    ''' </summary>
    ''' <param name="intItemToSaveId"></param>
    ''' <remarks></remarks>
    Sub StoreFormSpecificData(ByVal intItemToSaveId As Integer)



        SaveQuoteItems()

    End Sub

    ''' <summary>
    '''This function returns whether we should save the current page or not. The Default is to save
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function DataToSave(ByVal ParamArray aParams() As Object) As Boolean

        DataToSave = True

    End Function

    ''' <summary>
    ''' This function checks if there is any reason why the form should not be submitted
    ''' </summary>
    ''' <returns>A string with the text of the problem if there is a  problem, an empty string otherwise</returns>
    ''' <remarks></remarks>
    Public Function CanSubmit(ByVal ParamArray aParams() As Object) As String

        CanSubmit = ""
    End Function


    ''' <summary>
    ''' This function checks if there is any reason why the form should not be Signed Off
    ''' </summary>
    ''' <returns>A string with the text of the problem if there is a  problem, an empty string otherwise</returns>
    ''' <remarks></remarks>
    Public Function CanSignOff(ByVal ParamArray aParams() As Object) As String

        CanSignOff = ""


    End Function

    Sub SetupForPrinting()
        'btnAdd.Visible = False
        'btnEdit.Visible = False
        ''uwgHBagMCStartTestResults.DisplayLayout.FrameStyle.BorderStyle = BorderStyle.None
        ''uwgHBagMCStartTestResults.DisplayLayout.RowStyleDefault.BorderStyle = BorderStyle.None

        'uwgBakingChecks.DisplayLayout.StationaryMargins = Infragistics.WebUI.UltraWebGrid.StationaryMargins.No
        'uwgBakingChecks.DisplayLayout.StationaryMargins = Infragistics.WebUI.UltraWebGrid.StationaryMargins.No

    End Sub



    Sub FillData(ByVal QuoteId As Integer)

        Dim dsQuoteItems As New DataSet
        Dim objQuote As New VTDBFunctions.VTDBFunctions.QuoteFunctions
        Dim dtItemsForGrid As New DataTable
        Dim dtQouteDetails As New DataTable
        Dim objProd As New VTDBFunctions.VTDBFunctions.ProductsFunctions


        uwgQuoteItems.Clear()

        dtQouteDetails = objQuote.GetQuoteHeaderInfoForID(QuoteId)
        dsQuoteItems = objQuote.GetQuoteItemsForId(QuoteId)

        Me.CurrentSession.VT_QuoteCustomerID = dtQouteDetails.Rows(0).Item("CustomerId")
        Me.CurrentSession.VT_QuoteDeliveryCustomerID = dtQouteDetails.Rows(0).Item("DeliveryCustomerId")

        ' disabled the history function JG
        'dtItemsForGrid = AddHistoryForCustomer(dtQouteDetails.Rows(0).Item("DeliveryCustomerId"), Me.CurrentSession.VT_QuoteUseBillingCustomer, dsQuoteItems.Tables(0))
        'uwgQuoteItems.DataSource = dtItemsForGrid


        uwgQuoteItems.DataSource = dsQuoteItems
        uwgQuoteItems.DataBind()


        For Each uwgRow In uwgQuoteItems.Rows

            uwgRow.Cells.FromKey("ProductName").Value = Trim(objProd.GetProductNameForId(uwgRow.Cells.FromKey("ProductName").Value))
            uwgRow.Cells.FromKey("ProductCode").Value = Trim(objProd.GetProductCode(uwgRow.Cells.FromKey("ProductCode").Value))
        Next

        Dim intMaxNumQuotes As Integer
        Dim strtemp As String

        Dim objCommon As New VT_CommonFunctions.CommonFunctions
        strtemp = objCommon.GetConfigItem("MaxNumOrdersForGrid")

        If strtemp <> "" And IsNumeric(strtemp) Then
            intMaxNumQuotes = CInt(strtemp)
        Else
            intMaxNumQuotes = 5
        End If

        For i = 0 To 15
            If i > intMaxNumQuotes Then

                Select Case i
                    Case 1
                        uwgQuoteItems.Columns.FromKey("A").Hidden = True
                    Case 2
                        uwgQuoteItems.Columns.FromKey("B").Hidden = True
                    Case 3
                        uwgQuoteItems.Columns.FromKey("C").Hidden = True
                    Case 4
                        uwgQuoteItems.Columns.FromKey("D").Hidden = True
                    Case 5
                        uwgQuoteItems.Columns.FromKey("E").Hidden = True
                    Case 6
                        uwgQuoteItems.Columns.FromKey("F").Hidden = True
                    Case 7
                        uwgQuoteItems.Columns.FromKey("G").Hidden = True
                    Case 8
                        uwgQuoteItems.Columns.FromKey("H").Hidden = True
                    Case 9
                        uwgQuoteItems.Columns.FromKey("I").Hidden = True
                    Case 10
                        uwgQuoteItems.Columns.FromKey("J").Hidden = True
                    Case 11
                        uwgQuoteItems.Columns.FromKey("K").Hidden = True
                    Case 12
                        uwgQuoteItems.Columns.FromKey("L").Hidden = True
                    Case 13
                        uwgQuoteItems.Columns.FromKey("M").Hidden = True
                    Case 14
                        uwgQuoteItems.Columns.FromKey("N").Hidden = True
                    Case 15
                        uwgQuoteItems.Columns.FromKey("O").Hidden = True
                End Select
            End If
        Next i

    End Sub
    Sub ShowTotals()
        lblNumItems.Text = uwgQuoteItems.Rows.Count

        Dim dblTotal As Double = 0
        Dim dblTotalVAT As Double = 0
        For Each uwgRow As Infragistics.WebUI.UltraWebGrid.UltraGridRow In uwgQuoteItems.Rows
            dblTotal += uwgRow.Cells.FromKey("Price").Value
            dblTotalVAT += uwgRow.Cells.FromKey("VATCharged").Value
        Next

        lblTotal.Text = FormatCurrency(dblTotal)

        lblTotalVAT.Text = FormatCurrency(dblTotalVAT)
    End Sub

    ''' <summary>
    ''' This function validates the data on the form before it can be saved
    ''' </summary>
    ''' <returns>If there is a Validation problem return a message, Empty String otherwise</returns>
    ''' <remarks></remarks>
    Public Function ValidateMe() As String
        Dim objDisp As New VT_Display.DisplayFuncs
        Dim mpContentPlaceHolder As ContentPlaceHolder

        ValidateMe = ""

        mpContentPlaceHolder = CType(Master.FindControl("DetailsMaster_ContentPlaceHolder"), ContentPlaceHolder)

        ' sample code

        'If CType(mpContentPlaceHolder.FindControl(Me.CurrentSession.PrimaryItemName), TextBox).Text = "" Then
        '    ValidateMe = Me.CurrentSession.PrimaryItemMessage
        '    Exit Function
        'Else
        '    Session("_VT_SelectedItemName") = CType(mpContentPlaceHolder.FindControl(Me.CurrentSession.PrimaryItemName), TextBox).Text
        '    CType(Master.FindControl("lblDetailsItemName"), Label).Text = Me.CurrentSession.SelectedItemName
        '    CType(Master.FindControl("lblItemData"), Label).Text = Me.CurrentSession.SelectedItemName
        'End If



    End Function

    ''' <summary>
    '''This function returns whether we should save the current page or not. The Default is to save
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function DataToSave() As Boolean

        DataToSave = True

    End Function




    Public Function SaveNewQuoteItem() As Long


    End Function

    Public Sub UpdateQuoteItem()



    End Sub


    Sub SaveQuoteItems()

        Dim i As Integer
        Dim lngProductId As Long
        Dim dsProduct As New DataSet
        Dim objProducts As New VTDBFunctions.VTDBFunctions.ProductsFunctions
        Dim intUnitOfSale As Integer
        Dim dblQuantity, dblWeight, dblUnitPrice, dblVATRate, dblVATRatePercent As Double
        Dim dblVatAmount, dblExtendedPrice, dblTotalPrice As Double
        Dim strComment As String
        Dim objQuotes As New VTDBFunctions.VTDBFunctions.QuoteFunctions
        Dim strChargedByType As String
        Dim lngQuoteItemId As Long
        Dim intPackageTypeId As Integer
        Dim strPackageType As String
        Dim objLocalProd As New ProductsFunctions.Products
        Dim objLocalQuote As New LocalQuoteFunctions.Quotes
        Dim intNewQuoteItemId As Integer
        Dim objForms As New VT_Forms.Forms

        ' Go through grid 
        For i = 0 To uwgQuoteItems.Rows.Count - 1
            If uwgQuoteItems.Rows(i).Hidden = False Then
                If IsNumeric(uwgQuoteItems.Rows(i).Cells.FromKey("ProductId").Text) Then
                    lngProductId = CLng(uwgQuoteItems.Rows(i).Cells.FromKey("ProductId").Text)
                    dsProduct = objProducts.GetProductForId(lngProductId)
                    intUnitOfSale = objProducts.GetUnitOfSale(lngProductId)

                    ' Find available Qty or Weight
                    If IsNumeric(uwgQuoteItems.Rows(i).Cells.FromKey("Quantity").Text) Then
                        dblQuantity = CDbl(uwgQuoteItems.Rows(i).Cells.FromKey("Quantity").Text)
                    Else
                        dblQuantity = 0
                    End If

                    If IsNumeric(uwgQuoteItems.Rows(i).Cells.FromKey("Weight").Text) Then
                        dblWeight = CDbl(uwgQuoteItems.Rows(i).Cells.FromKey("Weight").Text)
                    Else
                        dblWeight = 0
                    End If

                    If dblQuantity <> 0 Or dblWeight <> 0 Then
                        'If intUnitOfSale = 1 And dblQuantity = 0 Then
                        '    strTemp = "The item in Row " + CStr(i + 1) + " - "
                        '    strTemp = strTemp + Trim(dsProduct.Tables(0).Rows(0).Item("Product_Name")) + " - is measured by Quantity and the Quantity selected is 0.\n\n"
                        '    strTemp = strTemp + "This item will be omitted from the order."
                        '    MsgBox(strTemp)
                        'ElseIf intUnitOfSale = 0 And dblWeight = 0 Then
                        '    strTemp = "The item in Row " + CStr(i + 1) + " - "
                        '    strTemp = strTemp + Trim(dsProduct.Tables(0).Rows(0).Item("Product_Name")) + " - is measured by Weight and the Weight selected is 0.\n\n"
                        '    strTemp = strTemp + "This item will be omitted from the order."
                        '    MsgBox(strTemp)
                        'Else

                        If IsNumeric(uwgQuoteItems.Rows(i).Cells.FromKey("UnitPrice").Text) Then
                            dblUnitPrice = CDbl(uwgQuoteItems.Rows(i).Cells.FromKey("UnitPrice").Text)
                        Else
                            dblUnitPrice = 0
                        End If


                        dblVATRate = Val(dsProduct.Tables(0).Rows(0).Item("VAT_Rate"))
                        dblVATRatePercent = dblVATRate

                        'If intUnitOfSale = 1 Then
                        '    dblVatAmount = (dblQuantity * dblUnitPrice) * (dblVATRate / 100)
                        '    ' reformat VATRate for easier calculation
                        '    dblVATRate = 1 + (dblVATRate / 100)
                        '    dblExtendedPrice = dblQuantity * dblUnitPrice * dblVATRate
                        'Else
                        dblVatAmount = (dblWeight * dblUnitPrice) * (dblVATRate / 100)
                        ' reformat VATRate for easier calculation
                        dblVATRate = 1 + (dblVATRate / 100)
                        dblExtendedPrice = dblWeight * dblUnitPrice * dblVATRate
                        '  End If
                        dblTotalPrice = dblTotalPrice + dblExtendedPrice


                        strChargedByType = "Use Default"

                        strComment = IIf(IsDBNull(uwgQuoteItems.Rows(i).Cells.FromKey("Comment").Text), "", uwgQuoteItems.Rows(i).Cells.FromKey("Comment").Text)
                        strComment = Trim(strComment)
                        lngQuoteItemId = IIf(IsDBNull(uwgQuoteItems.Rows(i).Cells.FromKey("QuoteItemId").Text), 0, uwgQuoteItems.Rows(i).Cells.FromKey("QuoteItemId").Text)


                        strPackageType = uwgQuoteItems.Rows(i).Cells.FromKey("PackageType").Text
                        intPackageTypeId = uwgQuoteItems.Rows(i).Cells.FromKey("PackageTypeId").Text

                        If lngQuoteItemId = 0 Then

                            intNewQuoteItemId = 0
                            'Add new
                            objQuotes.InsertQuoteItem(Me.CurrentSession.VT_QuoteID, lngProductId, dblQuantity, dblWeight, strComment, dblUnitPrice, dblVatAmount, _
                                                      dblExtendedPrice, strChargedByType, dblVATRatePercent, strPackageType, intPackageTypeId)

                            intNewQuoteItemId = objQuotes.GetLastAddedQuoteItemID

                            Dim dtProdClassificationItems As New DataTable

                            'astrProdItems(0) = "txtPhysicalForm"
                            'astrProdItems(1) = "txtEWCCode"
                            'astrProdItems(2) = "txtHazard"
                            'astrProdItems(3) = "txtUNNumber"
                            'astrProdItems(4) = "txtShippingName"
                            'astrProdItems(5) = "txtPrimaryHazard"
                            'astrProdItems(6) = "txtSecondaryHazard"
                            'astrProdItems(7) = "ddlPackagingGroup"


                            'Save a new product classification
                            dtProdClassificationItems = objLocalProd.GetProductClassificationDataForID(lngProductId)

                            If dtProdClassificationItems.Rows.Count > 0 Then
                                With dtProdClassificationItems.Rows(0)

                                    Dim strPackingGroup As String

                                    strPackingGroup = .Item("Field7").ToString
                                    strPackingGroup = Left(strPackingGroup, InStr(strPackingGroup, vbTab))

                                    objLocalQuote.SaveQuoteProductClassification(intNewQuoteItemId, lngProductId, .Item("Field0").ToString, .Item("Field1").ToString, _
                                                                                 .Item("Field2").ToString, .Item("Field3").ToString, .Item("Field4").ToString, .Item("Field5").ToString, _
                                                                                 .Item("Field6").ToString, strPackingGroup)
                                End With
                            End If
                        Else
                            'update existing
                            objQuotes.UpdateQuoteItem(lngQuoteItemId, lngProductId, dblQuantity, dblWeight, strComment, dblUnitPrice, dblVatAmount, _
                                                      dblExtendedPrice, strChargedByType, strPackageType, intPackageTypeId)
                        End If
                    End If
                End If
            End If
        Next i

        ' Save Totals

        'only do this if some items were actually saved for today. 
        objQuotes.SaveTotalValueForQuote(Me.CurrentSession.VT_QuoteID, dblTotalPrice)
        ' store the total value in ExtraData3
        objForms.SetWFOExtraDataItem(Me.CurrentSession.VT_QuoteNum, "ExtraData3", CStr(dblTotalPrice))


        FillData(Me.CurrentSession.VT_QuoteID)

    End Sub


    Private Sub ShowHideGridRows(ByVal intMode As ShowHide)
        Dim i As Integer
        Dim dblQty As Double
        Dim dblWeight As Double
        Dim intNumItems As Integer = 0
        Dim lngItemId As Long
        Dim objQuote As New VTDBFunctions.VTDBFunctions.QuoteFunctions

        'hide the empty rows if blnhidden = true
        For i = 0 To uwgQuoteItems.Rows.Count - 1

            dblQty = IIf(IsNumeric(uwgQuoteItems.Rows(i).Cells.FromKey("Quantity").Text), uwgQuoteItems.Rows(i).Cells.FromKey("Quantity").Text, 0)

            dblWeight = IIf(IsNumeric(uwgQuoteItems.Rows(i).Cells.FromKey("Weight").Text), uwgQuoteItems.Rows(i).Cells.FromKey("Weight").Text, 0)

            lngItemId = IIf(IsNumeric(uwgQuoteItems.Rows(i).Cells.FromKey("QuoteItemId").Text), CLng(uwgQuoteItems.Rows(i).Cells.FromKey("QuoteItemId").Text), 0)


            Select Case intMode
                Case ShowHide.HideEmpty
                    If dblQty = 0 And dblWeight = 0 Then
                        uwgQuoteItems.Rows(i).Hidden = True
                        If lngItemId > 0 Then
                            objQuote.DeleteQuoteItemForItemId(lngItemId)
                        End If
                    Else
                        uwgQuoteItems.Rows(i).Hidden = False
                        intNumItems += 1
                    End If
                Case ShowHide.ShowEmpty
                    If dblQty = 0 And dblWeight = 0 Then
                        uwgQuoteItems.Rows(i).Hidden = True
                    End If
                Case ShowHide.ShowAll
                    uwgQuoteItems.Rows(i).Hidden = False
                    intNumItems += 1
            End Select
        Next

        If intMode = ShowHide.ShowAll Then
            '    uwgQuoteItems.DisplayLayout.Bands(0).Columns("ProductName").SortIndicator.Ascending = True
        End If
        'now update the number of items label
        lblNumItems.Text = CStr(intNumItems)

    End Sub


    Private Function AddHistoryForCustomer(ByVal lngCustForOrders As Long, ByVal blnIsBillingCustomer As Boolean, ByVal dtExistingItems As DataTable) As DataTable
        Dim dsProductDetails As DataSet
        Dim objProducts As New VTDBFunctions.VTDBFunctions.ProductsFunctions
        Dim objQuote As New VTDBFunctions.VTDBFunctions.QuoteFunctions
        Dim dsQuoteItems As DataSet
        Dim cell As Infragistics.WebUI.UltraWebGrid.UltraGridCell
        Dim row As Infragistics.WebUI.UltraWebGrid.UltraGridRow
        Dim col As Infragistics.WebUI.UltraWebGrid.UltraGridColumn
        Dim dsQuoteDetails As DataSet
        Dim intCount, intIndex, intNumQuotes, intUnitOfSale As Integer
        Dim i, j As Integer
        Dim lngProductId, lngQuoteId As Long
        Dim strHeader As String
        Dim objTele As New VTDBFunctions.VTDBFunctions.TelesalesFunctions
        Dim dblHistoryValue As Double

        Try

            ' get all previous orders for this customer ordered by SalesOrderId DESC
            If blnIsBillingCustomer = True Then
                dsQuoteDetails = objQuote.GetQuoteHistoryForCustomer(lngCustForOrders)
            Else
                dsQuoteDetails = objQuote.GetQuoteHistoryForDeliveryCustomer(lngCustForOrders)
            End If

            ' now we want to display the last five orders (or fewer if that is all there are) 
            ' for this customer and show details of the products on each

            'need to loop through these to count them (exluding BackOrders)
            intNumQuotes = 0
            intNumQuotes = dsQuoteDetails.Tables(0).Rows.Count

            ' limit to last number of orders specified in config. If not specified then use 5
            Dim intMaxNumQuotes As Integer
            Dim strtemp As String

            Dim objCommon As New VT_CommonFunctions.CommonFunctions
            strtemp = objCommon.GetConfigItem("MaxNumOrdersForGrid")

            If strtemp <> "" And IsNumeric(strtemp) Then
                intMaxNumQuotes = CInt(strtemp)
            Else
                intMaxNumQuotes = 5
            End If

            If intNumQuotes > intMaxNumQuotes Then
                intNumQuotes = intMaxNumQuotes
            End If


            dtExistingItems.Columns.Add("A")
            dtExistingItems.Columns.Add("B")
            dtExistingItems.Columns.Add("C")
            dtExistingItems.Columns.Add("D")
            dtExistingItems.Columns.Add("E")
            dtExistingItems.Columns.Add("F")
            dtExistingItems.Columns.Add("G")
            dtExistingItems.Columns.Add("H")
            dtExistingItems.Columns.Add("I")
            dtExistingItems.Columns.Add("J")
            dtExistingItems.Columns.Add("K")
            dtExistingItems.Columns.Add("L")
            dtExistingItems.Columns.Add("M")
            dtExistingItems.Columns.Add("N")
            dtExistingItems.Columns.Add("O")





            ' create a key collection
            'Dim Keys(1) As DataColumn
            'Keys(0) = dtProducts.Columns("ProductId")
            'Keys(0) = dtProducts.Columns("ItemIndex")
            'dtProducts.PrimaryKey = Keys

            ' first we go through the orders (ignoring backorders) and show all the products 
            ' on the last 5 (or fewer if that is all there are)
            Dim intIncludesOrder As Integer

            If dtExistingItems.Rows.Count = 0 Then
                intIncludesOrder = 0
            Else
                intIncludesOrder = 1
            End If

            intIndex = 0
            intCount = 0
            While intIndex <= intNumQuotes + intIncludesOrder

                lngQuoteId = dsQuoteDetails.Tables(0).Rows(intIndex).Item("QuoteId")
                If lngQuoteId <> Me.CurrentSession.VT_QuoteID Then
                    ' get the items on the order
                    dsQuoteItems = objQuote.GetQuoteItemsForId(lngQuoteId)

                    For j = 0 To dsQuoteItems.Tables(0).Rows.Count - 1
                        ' add each product if it is not already added
                        lngProductId = dsQuoteItems.Tables(0).Rows(j).Item("ProductId")
                        intUnitOfSale = 0
                        intUnitOfSale = objProducts.GetUnitOfSale(lngProductId)

                        'If intUnitOfSale = 0 Then
                        dblHistoryValue = dsQuoteItems.Tables(0).Rows(j).Item("Weight")
                        'Else
                        'dblHistoryValue = dsQuoteItems.Tables(0).Rows(j).Item("Quantity")
                        'End If


                        intCount = intCount + 1

                        Select Case intCount
                            Case 1
                                strHeader = "A"
                            Case 2
                                strHeader = "B"
                            Case 3
                                strHeader = "C"
                            Case 4
                                strHeader = "D"
                            Case 5
                                strHeader = "E"
                            Case 6
                                strHeader = "F"
                            Case 7
                                strHeader = "G"
                            Case 8
                                strHeader = "H"
                            Case 9
                                strHeader = "I"
                            Case 10
                                strHeader = "J"
                            Case 11
                                strHeader = "K"
                            Case 12
                                strHeader = "L"
                            Case 13
                                strHeader = "M"
                            Case 14
                                strHeader = "N"
                            Case 15
                                strHeader = "O"
                        End Select


                        If dtExistingItems.Rows.Count > 0 Then

                            For i = 0 To dtExistingItems.Rows.Count

                                If dtExistingItems.Rows(i).Item("ProductID") = lngProductId Then

                                    dtExistingItems.Rows(i).Item(strHeader) = dblHistoryValue

                                Else
                                    Dim drNew As DataRow = dtExistingItems.NewRow()
                                    Dim dblUnitPrice As Double

                                    dblUnitPrice = objTele.GetPriceForProductAndCustomer(lngProductId, lngCustForOrders)

                                    drNew.Item("QuoteItemId") = 0 '(Int)
                                    drNew.Item("QuoteId") = Me.CurrentSession.VT_QuoteID '(Int)
                                    drNew.Item("ProductId") = Trim(dsQuoteItems.Tables(0).Rows(j).Item("ProductId")) '(Int)
                                    drNew.Item("Quantity") = 0 '(float)
                                    drNew.Item("Weight") = 0 '(float)
                                    drNew.Item("Comment") = "" '(nvarchar)
                                    drNew.Item("UnitPrice") = dblUnitPrice '(float)
                                    drNew.Item("VAT") = 0 '(float)
                                    drNew.Item("TotalPrice") = 0 '(float)
                                    drNew.Item("Status") = "" '(nvarchar)
                                    drNew.Item("ChargedByType") = "Use Default" '(nvarchar)

                                    drNew.Item(strHeader) = dblHistoryValue

                                    dtExistingItems.Rows.Add(drNew)

                                End If

                            Next i
                        Else
                            Dim drNew As DataRow = dtExistingItems.NewRow()
                            Dim dblUnitPrice As Double

                            dblUnitPrice = objTele.GetPriceForProductAndCustomer(lngProductId, lngCustForOrders)

                            drNew.Item("QuoteItemId") = 0 '(Int)
                            drNew.Item("QuoteId") = Me.CurrentSession.VT_QuoteID '(Int)
                            drNew.Item("ProductId") = Trim(dsQuoteItems.Tables(0).Rows(j).Item("ProductId")) '(Int)
                            drNew.Item("Quantity") = 0 '(float)
                            drNew.Item("Weight") = 0 '(float)
                            drNew.Item("Comment") = "" '(nvarchar)
                            drNew.Item("UnitPrice") = dblUnitPrice '(float)
                            drNew.Item("VAT") = 0 '(float)
                            drNew.Item("TotalPrice") = 0 '(float)
                            drNew.Item("Status") = "" '(nvarchar)
                            drNew.Item("ChargedByType") = "Use Default" '(nvarchar)

                            drNew.Item(strHeader) = dblHistoryValue

                            dtExistingItems.Rows.Add(drNew)
                        End If

                    Next j

                End If
                intIndex += 1 'move to next record



            End While



            AddHistoryForCustomer = dtExistingItems
        Catch ex As Exception
            Dim f As New UtilFunctions
            f.LogAction("Error in QuoteProducts:AddHistoryForCustomer: " + ex.Message)
            AddHistoryForCustomer = dtExistingItems
        End Try


        '' ''ugOrderItems.DataSource = dsGrid

        ' '' ''If strCustType = cCustDelivery Then
        '' '' '' now we go through the orders (ignoring backorders) and add the A,B,C,D,E columns
        '' ''intIndex = 0
        '' ''intCount = 0
        '' ''g_lngMostRecentSalesOrder = 0
        '' ''blnFinished = False
        '' ''While Not blnFinished And intIndex <= dsOrderDetails.Tables(0).Rows.Count - 1

        '' ''    If IIf(IsDBNull(dsOrderDetails.Tables(0).Rows(intIndex).Item("IsBackOrder")), False, dsOrderDetails.Tables(0).Rows(intIndex).Item("IsBackOrder")) = False Then
        '' ''        ' if not a back order

        '' ''        ' first set the column header
        '' ''        intCount = intCount + 1
        '' ''        Select Case intCount
        '' ''            Case 1
        '' ''                strHeader = "A"
        '' ''            Case 2
        '' ''                strHeader = "B"
        '' ''            Case 3
        '' ''                strHeader = "C"
        '' ''            Case 4
        '' ''                strHeader = "D"
        '' ''            Case 5
        '' ''                strHeader = "E"
        '' ''            Case 6
        '' ''                strHeader = "F"
        '' ''            Case 7
        '' ''                strHeader = "G"
        '' ''            Case 8
        '' ''                strHeader = "H"
        '' ''            Case 9
        '' ''                strHeader = "I"
        '' ''            Case 10
        '' ''                strHeader = "J"
        '' ''            Case 11
        '' ''                strHeader = "K"
        '' ''            Case 12
        '' ''                strHeader = "L"
        '' ''            Case 13
        '' ''                strHeader = "M"
        '' ''            Case 14
        '' ''                strHeader = "N"
        '' ''            Case 15
        '' ''                strHeader = "O"
        '' ''        End Select

        '' ''        ' store the most recent sales Order
        '' ''        If intIndex = 0 Then
        '' ''            If (strCustType = cCustBilling) Then
        '' ''                g_lngMostRecentBillingOrder = dsOrderDetails.Tables(0).Rows(intIndex).Item("SalesOrderId")
        '' ''            ElseIf (strCustType = cCustDelivery) Then
        '' ''                g_lngMostRecentDelOrder = dsOrderDetails.Tables(0).Rows(intIndex).Item("SalesOrderId")
        '' ''            End If
        '' ''        End If

        '' ''        ' add a new column using the salesorderId as the key
        '' ''        Dim strColumnKey As String = CStr(dsOrderDetails.Tables(0).Rows(intIndex).Item("SalesOrderId")) & " " & strCustType
        '' ''        If ugOrderItems.DisplayLayout.Bands(0).Columns.Exists(strColumnKey) Then
        '' ''            col = ugOrderItems.DisplayLayout.Bands(0).Columns(strColumnKey)
        '' ''        Else
        '' ''            col = ugOrderItems.DisplayLayout.Bands(0).Columns.Add(strColumnKey, strHeader)
        '' ''            'col = ugOrderItems.DisplayLayout.Bands(0).Columns.Add(strHeader, strHeader)
        '' ''        End If

        '' ''        'set the header colour for the column
        '' ''        col.Header.Appearance.BackColor = Color.Blue
        '' ''        col.Header.Appearance.ForeColor = Color.White
        '' ''        col.Width = 30

        '' ''        ' now fill in the quantity on the Order for each product that we 
        '' ''        ' previously added to the orders rows using the productId as the key
        '' ''        lngSalesOrderId = dsOrderDetails.Tables(0).Rows(intIndex).Item("SalesOrderId")
        '' ''        ' get the items on the order
        '' ''        dsOrderItems = objBPA.GetOrderItems(lngSalesOrderId)
        '' ''        For j = 0 To dsOrderItems.Tables(0).Rows.Count - 1
        '' ''            lngProductId = dsOrderItems.Tables(0).Rows(j).Item("ProductId")
        '' ''            row = FindGridRowByProductId(ugOrderItems, lngProductId, strCustType)


        '' ''            ' get details of this product
        '' ''            dsProductDetails = objProducts.GetProductForId(lngProductId)
        '' ''            If dsProductDetails.Tables(0).Rows.Count > 0 Then
        '' ''                intUnitOfSale = objProducts.GetUnitOfSale(lngProductId)
        '' ''            Else
        '' ''                intUnitOfSale = 0
        '' ''            End If

        '' ''            cell = row.Cells(strColumnKey)

        '' ''            If intUnitOfSale = 1 Then
        '' ''                cell.Value = FormatNumber(dsOrderItems.Tables(0).Rows(j).Item("QuantityRequested"), 2)
        '' ''            Else
        '' ''                cell.Value = FormatNumber(dsOrderItems.Tables(0).Rows(j).Item("WeightRequested"), 2)
        '' ''            End If

        '' ''        Next

        '' ''    End If

        '' ''    intIndex += 1 'move to next record

        '' ''    If intCount = intNumOrders Then
        '' ''        blnFinished = True
        '' ''    End If
        '' ''End While


        'End If

    End Function

    Protected Sub btmAddProduct_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnAddProduct.Click

        'If Not uwgOrderItems.DisplayLayout.ActiveRow Is Nothing Then
        '    Session("VT_FromPage") = "Fulfill_Opening"

        '    Response.Redirect("~/Other_Pages/AddProductToDelivery.aspx")
        '    'FillAddNewPanel()
        '    'ModalPopupExtender2.Show()
        'End If


    End Sub


    Protected Sub cmdProdAdd_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles cmdProdAdd.Click

        Dim lngProductId As Long
        Dim dsProduct As New DataSet
        Dim objProducts As New VTDBFunctions.VTDBFunctions.ProductsFunctions
        Dim objTele As New VTDBFunctions.VTDBFunctions.TelesalesFunctions
        Dim objCustomer As New VTDBFunctions.VTDBFunctions.CustomerFunctions
        Dim lngCustomerId As Long
        Dim dsCustomer As New DataSet
        Dim dblQtyOrWeight As Double
        Dim dblPrice As Double
        Dim strPackage As String


        lngProductId = CLng(ddlProducts.SelectedValue)
        dsProduct = objProducts.GetProductForId(lngProductId)
        strPackage = ddlPackageType.SelectedItem.ToString

        Dim uwgThis As Infragistics.WebUI.UltraWebGrid.UltraWebGrid
        Dim intRows As Integer
     
        uwgThis = uwgQuoteItems
        Dim NewRow As Infragistics.WebUI.UltraWebGrid.UltraGridRow
        intRows = uwgThis.Rows.Count
        uwgThis.Rows.Add(intRows)

        NewRow = uwgThis.Rows.FromKey(intRows)

        NewRow.Cells.FromKey("ProductCode").Value = dsProduct.Tables(0).Rows(0).Item("Catalog_Number")

        NewRow.Cells.FromKey("ProductName").Value = dsProduct.Tables(0).Rows(0).Item("Product_Name")
        'If dsProduct.Tables(0).Rows(0).Item("UnitOfSale") = 1 Then
        '    NewRow.Cells.FromKey("Quantity").Value = Trim(txtQuantity.Text)
        '    If Trim(txtQuantity.Text) = "" Then
        '        dblQtyOrWeight = 0
        '    Else
        '        dblQtyOrWeight = CDbl(txtQuantity.Text)
        '    End If
        'Else
        NewRow.Cells.FromKey("Quantity").Value = Trim(txtQuantity.Text)
        NewRow.Cells.FromKey("Weight").Value = Trim(txtWeight.Text)
        dblQtyOrWeight = CDbl(txtWeight.Text)
        If Trim(txtWeight.Text) = "" Then
            dblQtyOrWeight = 0
        Else
            dblQtyOrWeight = CDbl(txtWeight.Text)
        End If

        '  End If


        If Me.CurrentSession.VT_QuoteUseBillingCustomer = True Then
            lngCustomerId = Me.CurrentSession.VT_QuoteCustomerID
        Else
            lngCustomerId = Me.CurrentSession.VT_QuoteDeliveryCustomerID
        End If

        dsCustomer = objCustomer.GetCustomerForId(lngCustomerId)


        dblPrice = objProducts.GetProdPackPrice(lngProductId, lngCustomerId, strPackage)
        NewRow.Cells.FromKey("UnitPrice").Value = dblPrice

        Dim blnIsVATExempt As Boolean
        Dim dblVATRate As Double

        blnIsVATExempt = IIf(IsDBNull(dsCustomer.Tables(0).Rows(0).Item("VatExempt")), False, dsCustomer.Tables(0).Rows(0).Item("VatExempt"))

        If blnIsVATExempt = True Then
            dblVATRate = 0
        Else
            If IsDBNull(dsProduct.Tables(0).Rows(0).Item("Vat_Rate")) Then
                dblVATRate = 0
            Else
                If Trim(dsProduct.Tables(0).Rows(0).Item("Vat_Rate")) <> "" Then
                    dblVATRate = If(IsDBNull(dsProduct.Tables(0).Rows(0).Item("Vat_Rate")), 0, Val(dsProduct.Tables(0).Rows(0).Item("Vat_Rate")))
                Else
                    dblVATRate = 0
                End If

            End If

        End If

        NewRow.Cells.FromKey("VATRate").Value = dblVATRate

        Dim strProductCode As String = If(IsDBNull(dsProduct.Tables(0).Rows(0).Item("Catalog_Number")), "", dsProduct.Tables(0).Rows(0).Item("Catalog_Number"))
        Dim dblVATAmount As Double
        dblVATAmount = (dblQtyOrWeight * dblPrice) * (dblVATRate / 100)
        ' reformat VATRate for easier calculation
        dblVATRate = 1 + (dblVATRate / 100)


        NewRow.Cells.FromKey("VATCharged").Value = dblVATAmount
        NewRow.Cells.FromKey("ProductCode").Value = strProductCode
        NewRow.Cells.FromKey("Price").Value = FormatCurrency((dblQtyOrWeight * dblPrice) * dblVATRate, 2)
        NewRow.Cells.FromKey("ProductId").Value = lngProductId
        NewRow.Cells.FromKey("QuoteItemId").Value = 0
        NewRow.Cells.FromKey("Comment").Value = Trim(txtComment.Text)
        NewRow.Cells.FromKey("PackageType").Value = ddlPackageType.SelectedItem.ToString
        NewRow.Cells.FromKey("PackageTypeID").Value = ddlPackageType.SelectedValue


        ShowTotals()
    End Sub


    Public Sub PopulatePackageTypeCombo()
        Dim dt As Data.DataTable
        Dim objProd As New ProductsFunctions.Products


        ddlPackageType.Items.Clear()

        dt = objProd.GetPackageTypes

        For Each dr As DataRow In dt.Rows
            dr.Item("PackageTypeText") = Trim(dr.Item("PackageTypeText"))
        Next

        ddlPackageType.DataSource = dt
        ddlPackageType.DataTextField = "PackageTypeText"
        ddlPackageType.DataValueField = "PackageTypeID"
        ddlPackageType.DataBind()

    

    End Sub

    Protected Sub uwgQuoteItems_Click(ByVal sender As Object, ByVal e As Infragistics.WebUI.UltraWebGrid.ClickEventArgs) Handles uwgQuoteItems.Click


        Dim objProducts As New ProductsFunctions.Products
        Dim dtExtraDataRow As New DataTable
        Dim objMatrix As New VTDBFunctions.VTDBFunctions.VTMatrixFunctions
        Dim strPackingGroup As String
        Dim intQuoteItemId As Integer

        uwgProductClassification.Clear()

        dtExtraDataRow.Columns.Add("QuoteItemId")
        dtExtraDataRow.Columns.Add("ProductId")
        dtExtraDataRow.Columns.Add("PhysicalForm")
        dtExtraDataRow.Columns.Add("EWC")
        dtExtraDataRow.Columns.Add("Hazard")
        dtExtraDataRow.Columns.Add("UNNum")
        dtExtraDataRow.Columns.Add("ProperShipping")
        dtExtraDataRow.Columns.Add("PriHazard")
        dtExtraDataRow.Columns.Add("SecHazard")
        dtExtraDataRow.Columns.Add("PackingGroup")


        Dim drNew As DataRow = dtExtraDataRow.NewRow()


        Dim dtMatrixData As New DataTable

        Dim dsClassificationItems As Data.DataSet
        intQuoteItemId = uwgQuoteItems.DisplayLayout.ActiveRow.Cells.FromKey("QuoteItemId").Value

        'IF the quote item has been saved it will be in the matrix
        If intQuoteItemId > 0 Then

            ' get the Materials items fom the grid
            Dim intNumFields As Integer = 10
            Dim astrItems(intNumFields - 1) As String

            astrItems(0) = "QuoteItemIdForClass"
            astrItems(1) = "ProductIdForClass"
            astrItems(2) = "PhysicalForm"
            astrItems(3) = "EWC"
            astrItems(4) = "Hazard"
            astrItems(5) = "UNNum"
            astrItems(6) = "ProperShipping"
            astrItems(7) = "PriHazard"
            astrItems(8) = "SecHazard"
            astrItems(9) = "PackingGroup"

            dsClassificationItems = objMatrix.GetVTMatrixData(Session("_VT_DotNetConnString"), VT_CommonFunctions.MatrixFunctions.QuoteFormDataTable, intNumFields, astrItems, _
                                                              "QuoteItemIdForClass", CStr(intQuoteItemId))
        End If


        'Otherwise get the  details of the product from the spec sheet
        If intQuoteItemId = 0 OrElse dsClassificationItems.Tables(0).Rows.Count < 1 Then

            Dim dtProdClassificationItems As New DataTable

            'astrProdItems(0) = "txtPhysicalForm"
            'astrProdItems(1) = "txtEWCCode"
            'astrProdItems(2) = "txtHazard"
            'astrProdItems(3) = "txtUNNumber"
            'astrProdItems(4) = "txtShippingName"
            'astrProdItems(5) = "txtPrimaryHazard"
            'astrProdItems(6) = "txtSecondaryHazard"
            'astrProdItems(7) = "ddlPackagingGroup"


            dtProdClassificationItems = objProducts.GetProductClassificationDataForID(uwgQuoteItems.DisplayLayout.ActiveRow.Cells.FromKey("ProductId").Value)

            If dtProdClassificationItems.Rows.Count > 0 Then
                With dtProdClassificationItems.Rows(0)

                    strPackingGroup = .Item("Field7").ToString
                    strPackingGroup = Left(strPackingGroup, InStr(strPackingGroup, vbTab))


                    drNew.Item("QuoteItemId") = 0
                    drNew.Item("ProductId") = uwgQuoteItems.DisplayLayout.ActiveRow.Cells.FromKey("ProductId").Value
                    drNew.Item("PhysicalForm") = .Item("Field0").ToString
                    drNew.Item("EWC") = .Item("Field1").ToString
                    drNew.Item("Hazard") = .Item("Field2").ToString
                    drNew.Item("UNNum") = .Item("Field3").ToString
                    drNew.Item("ProperShipping") = .Item("Field4").ToString
                    drNew.Item("PriHazard") = .Item("Field5").ToString
                    drNew.Item("SecHazard") = .Item("Field6").ToString
                    drNew.Item("PackingGroup") = strPackingGroup
                End With
            Else
                drNew.Item("QuoteItemId") = 0 '(Int)
                drNew.Item("ProductId") = uwgQuoteItems.DisplayLayout.ActiveRow.Cells.FromKey("ProductId").Value
                drNew.Item("PhysicalForm") = "" '(nvarchar)
                drNew.Item("EWC") = "" '(nvarchar)
                drNew.Item("Hazard") = "" '(nvarchar)
                drNew.Item("UNNum") = "" '(nvarchar)
                drNew.Item("ProperShipping") = "" '(nvarchar)
                drNew.Item("PriHazard") = "" '(nvarchar)
                drNew.Item("SecHazard") = "" '(nvarchar)
                drNew.Item("PackingGroup") = "" '(nvarchar)
            End If

        Else

            If dsClassificationItems.Tables(0).Rows.Count > 0 Then
                With dsClassificationItems.Tables(0).Rows(0)

                    drNew.Item("QuoteItemId") = intQuoteItemId
                    drNew.Item("ProductId") = uwgQuoteItems.DisplayLayout.ActiveRow.Cells.FromKey("ProductId").Value
                    drNew.Item("PhysicalForm") = .Item("Field2").ToString
                    drNew.Item("EWC") = .Item("Field3").ToString
                    drNew.Item("Hazard") = .Item("Field4").ToString
                    drNew.Item("UNNum") = .Item("Field5").ToString
                    drNew.Item("ProperShipping") = .Item("Field6").ToString
                    drNew.Item("PriHazard") = .Item("Field7").ToString
                    drNew.Item("SecHazard") = .Item("Field8").ToString
                    drNew.Item("PackingGroup") = .Item("Field9").ToString
                End With
            Else
                drNew.Item("QuoteItemId") = 0 '(Int)
                drNew.Item("ProductId") = uwgQuoteItems.DisplayLayout.ActiveRow.Cells.FromKey("ProductId").Text
                drNew.Item("PhysicalForm") = "" '(nvarchar)
                drNew.Item("EWC") = "" '(nvarchar)
                drNew.Item("Hazard") = "" '(nvarchar)
                drNew.Item("UNNum") = "" '(nvarchar)
                drNew.Item("ProperShipping") = "" '(nvarchar)
                drNew.Item("PriHazard") = "" '(nvarchar)
                drNew.Item("SecHazard") = "" '(nvarchar)
                drNew.Item("PackingGroup") = "" '(nvarchar)
            End If


        End If

        dtExtraDataRow.Rows.Add(drNew)

        uwgProductClassification.DataSource = dtExtraDataRow
        uwgProductClassification.DataBind()

    End Sub


   

  
    Protected Sub btnSaveClassification_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnSaveClassification.Click

        Dim objLocalQuote As New LocalQuoteFunctions.Quotes



        If uwgQuoteItems.DisplayLayout.ActiveRow.Cells.FromKey("QuoteItemId").Value = 0 Then

            lblMsg.Text = "You must save the quote items before editing their classifications. Please Save and try again."
            ModalPopupExtenderMessage.Show()
            Exit Sub

        Else
            '****Grid Columns *****
            'PhysicalForm
            '"EWC" 
            '"Hazard" 
            '"UNNum" 
            '"ProperShipping" 
            '"PriHazard" 
            '"SecHazard" 
            '"PackingGroup" 
            '"ProductId"
            '"QuoteItemId"

            With uwgProductClassification.Rows(0).Cells
                objLocalQuote.SaveQuoteProductClassification(.FromKey("QuoteItemId").Value, .FromKey("ProductId").Value, .FromKey("PhysicalForm").Value, .FromKey("EWC").Value, _
                                                                       .FromKey("Hazard").Value, .FromKey("UNNum").Value, .FromKey("ProperShipping").Value, .FromKey("PriHazard").Value, _
                                                                       .FromKey("SecHazard").Value, .FromKey("PackingGroup").Value)

            End With
            lblMsg.Text = "Classification changes saved to quote item."
            ModalPopupExtenderMessage.Show()

        End If


    End Sub

    Protected Sub btnRemoveFromList_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnRemoveFromList.Click
        Dim objVTDBF As New VTDBFunctions.VTDBFunctions.QuoteFunctions
        objVTDBF.DeleteQuoteItemForItemId(uwgQuoteItems.DisplayLayout.ActiveRow.Cells.FromKey("QuoteItemId").Value)
    End Sub

    Protected Sub btnRefreshList_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnRefreshList.Click

    End Sub

    Protected Sub btnClearEmptyRows_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnClearEmptyRows.Click

    End Sub
End Class
