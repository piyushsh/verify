Imports System.Data.SqlClient
Imports System.Data
Imports System.Configuration
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Drawing
Imports System.Web.UI.HtmlControls
Imports BPADotNetCommonFunctions
Imports BPADotNetCommonFunctions.SupplierModuleFunctions
Imports BPADotNetCommonFunctions.VT_CommonFunctions.MatrixFunctions
Imports AjaxControlToolkit
Imports VTDBFunctions.VTDBFunctions


Partial Class OtherPages_CCP_FormsForDispatch
    'Inherits System.Web.UI.Page
    Inherits MyBasePage
    'Inherits MyMasterPage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim objP As New VT_DataPreserve.DataPreserve
        lblDocketNum.Text = Me.CurrentSession.VT_DocketNumber

        If Not IsPostBack Then
            'Hide the Report Header Panel on entry
            '    Master.PanelReference.Visible = False

            'Hide/Show the relevant Panels on entry


        Else

            wdgCookCoolData.DataSource = objP.GetWDGDataFromSession(wdgCookCoolData)
            wdgStartData.DataSource = objP.GetWDGDataFromSession(wdgStartData)
            wdgSpotTempData.DataSource = objP.GetWDGDataFromSession(wdgSpotTempData)
            wdgIngredientsHierarchy.DataSource = objP.GetWDGDataFromSession(wdgIngredientsHierarchy)
        End If

    End Sub

    Protected Sub btnRunReport_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnRunReport.Click

        RunReport()

    End Sub

    Public Function RunReport() As Boolean
        Dim objCommonFuncs As New BPADotNetCommonFunctions.VT_CommonFunctions.CommonFunctions
        Dim objG As New BPADotNetCommonFunctions.VT_ReportFunctions.GeneralFunctions
        Dim objTransactions As New VTDBFunctions.VTDBFunctions.TelesalesFunctions
        Dim objR As New BPADotNetCommonFunctions.VT_ReportFunctions.SalesDataFunctions
        Dim objT As New BPADotNetCommonFunctions.VT_ReportFunctions.TracabilityFunctions
        Dim objP As New VT_DataPreserve.DataPreserve
        Dim objM As New VTDBFunctions.VTDBFunctions.VTMatrixFunctions
        Dim objVTDBF As New VTMatrixFunctions
        Dim dtRep, dtRepCopy, dtParent, dtSearch, dt1, dt2, dt3, dt4, dt5, dtRepClone As DataTable
        Dim dsFirstGeneration, dsSecondGeneration, dsThirdGeneration, dsAdded, dsFourthGeneration, dsFifthGeneration As New DataSet
        Dim strconn As String = Session("_VT_DotNetConnString")
        Dim intFormid As Integer
        Dim strGridName As String
        Dim strKeyField, strKeyField2 As String
        Dim dtRepNew As New DataTable
        Dim dr As DataRow

        Dim strSourceTable, strDocketNumber As String

        strDocketNumber = Me.CurrentSession.VT_DocketNumber

        dtRep = getProductInfoForDocketNum(strconn, strDocketNumber)

        If dtRep IsNot Nothing AndAlso dtRep.Rows.Count > 0 Then
            dtRep.Columns.Add("Generation", GetType(Integer))
            dtRep.Columns.Add("ParentId")
            dtRep.Columns.Add("ChildId")
            'dtRep.Columns.Add("LineItemNum")
            dtRep.Columns.Add("Parent_Product_Name")
            For intRowCount = 0 To dtRep.Rows.Count - 1
                dtRep.Rows(intRowCount).Item("Generation") = 0
            Next

            dtRepClone = dtRep.Clone
            For intRowCount = 0 To dtRep.Rows.Count - 1
                dr = dtRep.NewRow
                dr = dtRep.Rows(intRowCount)
                dtRepClone.ImportRow(dr)
                'get first and subsequent generations
                dsAdded = GetIngredientsAdded(dtRepClone.Rows(0).Item("TraceCodeId"))
                If dsAdded IsNot Nothing AndAlso dsAdded.Tables(0).Rows.Count > 0 Then
                    dt1 = (dsAdded.Tables(0).DefaultView.ToTable(True, "ParentId", "ChildId"))
                    addColumns(dt1)
                    For intInnerRowCount = 0 To dt1.Rows.Count - 1
                        dt1.Rows(intInnerRowCount).Item("Generation") = 1
                        dt1.Rows(intInnerRowCount).Item("TraceCodeId") = dt1.Rows(intInnerRowCount).Item("ChildId")
                        '  dt1.Rows(intInnerRowCount).Item("LineItemNum") = dtRep.Rows(intRowCount).Item("LineItemNum")
                        dtRepClone.ImportRow(dt1.Rows(intInnerRowCount))
                        dsSecondGeneration = GetIngredientsAdded(dt1.Rows(intInnerRowCount).Item("ChildId"))
                        If dsSecondGeneration IsNot Nothing AndAlso dsSecondGeneration.Tables(0).Rows.Count > 0 Then
                            dt2 = (dsSecondGeneration.Tables(0).DefaultView.ToTable(True, "ParentId", "ChildId"))
                            addColumns(dt2)
                            For intInner2RowCount = 0 To dt2.Rows.Count - 1
                                dt2.Rows(intInner2RowCount).Item("Generation") = 2
                                dt2.Rows(intInner2RowCount).Item("TraceCodeId") = dt2.Rows(intInner2RowCount).Item("ChildId")
                                ' dt2.Rows(intInner2RowCount).Item("LineItemNum") = dtRep.Rows(intRowCount).Item("LineItemNum")
                                dtRepClone.ImportRow(dt2.Rows(intInner2RowCount))
                                dsThirdGeneration = GetIngredientsAdded(dsSecondGeneration.Tables(0).Rows(intInner2RowCount).Item("ChildId"))
                                If dsThirdGeneration IsNot Nothing AndAlso dsThirdGeneration.Tables(0).Rows.Count > 0 Then
                                    dt3 = (dsThirdGeneration.Tables(0).DefaultView.ToTable(True, "ParentId", "ChildId"))
                                    addColumns(dt3)
                                    For intInner3RowCount = 0 To dt3.Rows.Count - 1
                                        dt3.Rows(intInner3RowCount).Item("Generation") = 3
                                        dt3.Rows(intInner3RowCount).Item("TraceCodeId") = dt3.Rows(intInner3RowCount).Item("ChildId")
                                        ' dt3.Rows(intInner3RowCount).Item("LineItemNum") = dtRep.Rows(intRowCount).Item("LineItemNum")
                                        dtRepClone.ImportRow(dt3.Rows(intInner3RowCount))
                                        dsFourthGeneration = GetIngredientsAdded(dsThirdGeneration.Tables(0).Rows(intInner3RowCount).Item("ChildId"))
                                        If dsFourthGeneration IsNot Nothing AndAlso dsFourthGeneration.Tables(0).Rows.Count > 0 Then
                                            dt4 = (dsFourthGeneration.Tables(0).DefaultView.ToTable(True, "ParentId", "ChildId"))
                                            addColumns(dt4)
                                            For intInner4RowCount = 0 To dt4.Rows.Count - 1
                                                dt4.Rows(intInner4RowCount).Item("Generation") = 4
                                                dt4.Rows(intInner4RowCount).Item("TraceCodeId") = dt4.Rows(intInner4RowCount).Item("ChildId")
                                                ' dt3.Rows(intInner3RowCount).Item("LineItemNum") = dtRep.Rows(intRowCount).Item("LineItemNum")
                                                dtRepClone.ImportRow(dt4.Rows(intInner4RowCount))
                                                dsFifthGeneration = GetIngredientsAdded(dsFourthGeneration.Tables(0).Rows(intInner4RowCount).Item("ChildId"))
                                                If dsFifthGeneration IsNot Nothing AndAlso dsFifthGeneration.Tables(0).Rows.Count > 0 Then
                                                    dt5 = (dsFifthGeneration.Tables(0).DefaultView.ToTable(True, "ParentId", "ChildId"))
                                                    addColumns(dt5)
                                                    For intInner5RowCount = 0 To dt5.Rows.Count - 1
                                                        dt5.Rows(intInner5RowCount).Item("Generation") = 5
                                                        dt4.Rows(intInner5RowCount).Item("TraceCodeId") = dt5.Rows(intInner5RowCount).Item("ChildId")
                                                        ' dt3.Rows(intInner3RowCount).Item("LineItemNum") = dtRep.Rows(intRowCount).Item("LineItemNum")
                                                        dtRepClone.ImportRow(dt5.Rows(intInner5RowCount))
                                                    Next
                                                End If
                                            Next
                                        End If
                                    Next
                                End If
                            Next
                        End If
                    Next
                        End if
                dtRepNew.Merge(dtRepClone)
                dtRepClone.Rows.Clear()

            Next
        End If

        'get batch of ProductDetails for list of TraceCodeId's
        dtRepCopy = dtRepNew

        Dim strSQL As String = "Select  trc_TraceCodes.TraceCodeId, trc_TraceCodes.ProductId, trc_TraceCodes.TraceCodeDesc, prd_ProductTable.Product_Name"
        strSQL = strSQL + " FROM trc_TraceCodes JOIN prd_ProductTable ON trc_TraceCodes.ProductId = prd_ProductTable.ProductId"
        strSQL = strSQL + " WHERE trc_TraceCodes.TraceCodeId IN ("

        strKeyField = "TraceCodeId"
        Dim astrItems(2) As String
        astrItems(0) = "ProductId"
        astrItems(1) = "TraceCodeDesc"
        astrItems(2) = "Product_Name"

        Dim strErr As String = GetBlockOfProductDataItems(strconn, strSQL, astrItems, dtRepCopy, strKeyField)
        Dim strSearch As String
        'Get Parent Product Information if relevent
        strSearch = "ParentId is not null"
        dtParent = objG.SearchDataTable(strSearch, dtRepCopy)
        If dtParent IsNot Nothing AndAlso dtParent.Rows.Count > 0 Then

            strSQL = "Select  trc_TraceCodes.TraceCodeId, trc_TraceCodes.ProductId, trc_TraceCodes.TraceCodeDesc, prd_ProductTable.Product_Name AS Parent_Product_Name"
            strSQL = strSQL + " FROM trc_TraceCodes JOIN prd_ProductTable ON trc_TraceCodes.ProductId = prd_ProductTable.ProductId"
            strSQL = strSQL + " WHERE trc_TraceCodes.TraceCodeId IN ("
            strKeyField = "ParentId"
            ReDim astrItems(1)
            astrItems(1) = "Product_Name"

            strErr = GetBlockOfProductDataItems(strconn, strSQL, astrItems, dtRepCopy, strKeyField)
        End If

        'Get batch of jobId, MatrixLinkId for TraceCodeDesc and ProductId from wfo_BatchTable

        ReDim astrItems(1)
        astrItems(0) = "JobId"
        astrItems(1) = "MatrixLinkId"
        strKeyField = "TraceCodeDesc"
        strKeyField2 = "ProductId"
        strErr = GetBlockOfBatchDataItems(strconn, astrItems, dtRepCopy, strKeyField, strKeyField2)


        ' Dim dtHierarchy As DataTable = dtRepCopy.Clone
        dtRepCopy.Columns.Add("Dispatch_Item")
        dtRepCopy.Columns.Add("Ingredients_Level_1")
        dtRepCopy.Columns.Add("Ingredients_Level_2")
        dtRepCopy.Columns.Add("Ingredients_Level_3")
        dtRepCopy.Columns.Add("Ingredients_Level_4")
        dtRepCopy.Columns.Add("Ingredients_Level_5")




        'Sort the data for the Ingredients Hierarchy Grid.
        For intRowCount = 0 To dtRepCopy.Rows.Count - 1
            If Not IsDBNull(dtRepCopy.Rows(intRowCount).Item("Generation")) Then
                Select Case dtRepCopy.Rows(intRowCount).Item("Generation")
                    Case 0
                        dtRepCopy.Rows(intRowCount).Item("Dispatch_Item") = Trim(dtRepCopy.Rows(intRowCount).Item("Product_Name")).ToString + " " _
                        + "[" + Trim(dtRepCopy.Rows(intRowCount).Item("TraceCodeDesc")).ToString + "]"
                    Case 1
                        dtRepCopy.Rows(intRowCount).Item("Ingredients_Level_1") = Trim(dtRepCopy.Rows(intRowCount).Item("Product_Name")).ToString + " " _
                                + "[" + Trim(dtRepCopy.Rows(intRowCount).Item("TraceCodeDesc")).ToString + "]"
                    Case 2
                        dtRepCopy.Rows(intRowCount).Item("Ingredients_Level_2") = Trim(dtRepCopy.Rows(intRowCount).Item("Product_Name")).ToString + " " _
                                           + "[" + Trim(dtRepCopy.Rows(intRowCount).Item("TraceCodeDesc")).ToString + "]"
                    Case 3
                        dtRepCopy.Rows(intRowCount).Item("Ingredients_Level_3") = Trim(dtRepCopy.Rows(intRowCount).Item("Product_Name")).ToString + " " _
                                                        + "[" + Trim(dtRepCopy.Rows(intRowCount).Item("TraceCodeDesc")).ToString + "]"
                    Case 4
                        dtRepCopy.Rows(intRowCount).Item("Ingredients_Level_4") = Trim(dtRepCopy.Rows(intRowCount).Item("Product_Name")).ToString + " " _
                                                                    + "[" + Trim(dtRepCopy.Rows(intRowCount).Item("TraceCodeDesc")).ToString + "]"
                    Case 5
                        dtRepCopy.Rows(intRowCount).Item("Ingredients_Level_5") = Trim(dtRepCopy.Rows(intRowCount).Item("Product_Name")).ToString + " " _
                                                                                + "[" + Trim(dtRepCopy.Rows(intRowCount).Item("TraceCodeDesc")).ToString + "]"
                End Select

            End If
        Next

        Dim dtHierarchy As DataTable = dtRepCopy.Copy



        Dim dtData As New DataTable
        strSearch = "MatrixLinkId is not null AND MatrixLinkId <> ''"
        dtData = objG.SearchDataTable(strSearch, dtRepCopy)
        'and we need unique MatrixLinkId's
        dtData = dtData.DefaultView.ToTable(True, "DocketNum", "TraceCodeId", "ProductId", "Product_Name", "TraceCodeDesc", "Generation", "ChildId", "Parent_Product_Name", "JobId", "MatrixLinkId")

        If dtData IsNot Nothing AndAlso dtData.Rows.Count > 0 Then

            dtData.Columns.Add("GridName")
            dtData.Columns.Add("Field0")
            dtData.Columns.Add("Field1")
            dtData.Columns.Add("Field2")
            dtData.Columns.Add("Field3")
            dtData.Columns.Add("Field4")
            dtData.Columns.Add("Field5")
            dtData.Columns.Add("Field6")
            dtData.Columns.Add("Field7")
            dtData.Columns.Add("Field8")
            dtData.Columns.Add("Field9")



            'Get Matrix ID's of the  forms required for getting the relevent grid data - we have to do a bit of drilling down
            If dtData IsNot Nothing AndAlso dtData.Rows.Count > 0 Then

                dtData.Columns("MatrixLinkId").ColumnName = "FormId"
                Dim strFormName As String = "Forms"
                strKeyField = "FormId"
                strErr = objM.GetMatrixNamedChildIDs(strconn, VTMatrixFunctions.Production, dtData, strKeyField, strFormName)

                'Get copy of dtData for subsequent sets of forms
                Dim dtTempSpotCheck As DataTable = dtData.Copy ' 


                Dim strSystemFormsFolder As String = objCommonFuncs.GetConfigItem("SystemFormsFolder")
                If strSystemFormsFolder = "Aliyas" Then

                    'Get Matrix ID's for Cooking and Cooling CCP Data Parent folders (also called 'Cooking and Cooling CCP Data')
                    strFormName = "Cooking and Cooling CCP Data"
                    strKeyField = "[ChildId_Forms]"
                    strErr = objM.GetMatrixNamedChildIDs(strconn, VTMatrixFunctions.Production, dtData, strKeyField, strFormName)
                    'prevent duplicate col name error in GetMatrixNamedChildIDs function
                    If Not dtData.Columns.IndexOf("[ChildId_Cooking and Cooling CCP Data]") = -1 Then
                        dtData.Columns("[ChildId_Cooking and Cooling CCP Data]").ColumnName = "[Parent_ChildId_Cooking and Cooling CCP Data]"
                    End If


                    'Get Matrix ID's for Cooking and Cooling CCP Data
                    'Check is there data
                    Dim intIndex As Integer = 0
                    For intRowCount = 0 To dtData.Rows.Count - 1
                        If Not IsDBNull(dtData.Rows(intRowCount).Item("[Parent_ChildId_Cooking and Cooling CCP Data]")) Then
                            intIndex = intIndex + 1
                        End If
                    Next
                    Dim dtDataClone As New DataTable
                    If intIndex > 0 Then
                        strFormName = "Cooking and Cooling CCP Data"
                        strKeyField = "[Parent_ChildId_Cooking and Cooling CCP Data]"
                        strErr = objM.GetMatrixNamedChildIDs(strconn, VTMatrixFunctions.Production, dtData, strKeyField, strFormName)

                        dtDataClone = dtData.Clone
                        For intRowCount = 0 To dtData.Rows.Count - 1
                            If Not IsDBNull(dtData.Rows(intRowCount).Item("[ChildId_Cooking and Cooling CCP Data]")) Then
                                intFormid = dtData.Rows(intRowCount).Item("[ChildId_Cooking and Cooling CCP Data]")

                                strGridName = "wdgcookData"
                                strSourceTable = "pdn_GridData"
                                getCCPData(dtData, dtDataClone, strGridName, intFormid, strSourceTable, intRowCount)

                                strGridName = "wdgcoolingData"
                                getCCPData(dtData, dtDataClone, strGridName, intFormid, strSourceTable, intRowCount)

                                strGridName = "wdgcoolingDataStep2"
                                getCCPData(dtData, dtDataClone, strGridName, intFormid, strSourceTable, intRowCount)

                                strGridName = "wdgcoolingDataStep3"
                                getCCPData(dtData, dtDataClone, strGridName, intFormid, strSourceTable, intRowCount)

                            End If

                        Next

                    End If
                    'Get Matrix ID's for Temperature Spot Check CCP Data Parent folders (also called 'Temperature Spot Check CCP Data')
                    strFormName = "TemperatureSpotCheck CCP Data"
                    strKeyField = "[ChildId_Forms]"
                    strErr = objM.GetMatrixNamedChildIDs(strconn, VTMatrixFunctions.Production, dtTempSpotCheck, strKeyField, strFormName)
                    'prevent duplicate col name error in GetMatrixNamedChildIDs function
                    If Not dtTempSpotCheck.Columns.IndexOf("[ChildId_TemperatureSpotCheck CCP Data]") = -1 Then
                        'dtTempSpotCheck.Columns("[ChildId_Temperature Spot Check CCP Data]").ColumnName = "[Parent_ChildId_Temperature_Spot_Check_CCP_Data]"
                        dtTempSpotCheck.Columns("[ChildId_TemperatureSpotCheck CCP Data]").ColumnName = "ParentFormIdTemperatureSpotCheckCCPData"
                    End If

                    'Get Matrix ID's for Temperature Spot Check CCP Data




                    strFormName = "TemperatureSpotCheck CCP Data"
                    strKeyField = "ParentFormIdTemperatureSpotCheckCCPData"
                    'First check are there any of these forms 
                    If dtTempSpotCheck IsNot Nothing AndAlso dtTempSpotCheck.Rows.Count > 0 Then
                        'strSearch = dtTempSpotCheck.Columns(dtTempSpotCheck.Columns.Count - 1).ColumnName.ToString + " is not null"
                        strSearch = "ParentFormIdTemperatureSpotCheckCCPData is not null"
                        dtSearch = objG.SearchDataTable(strSearch, dtTempSpotCheck)
                        If dtSearch IsNot Nothing AndAlso dtSearch.Rows.Count > 0 Then
                            strErr = objM.GetMatrixNamedChildIDs(strconn, VTMatrixFunctions.Production, dtTempSpotCheck, strKeyField, strFormName)
                        End If
                    End If

                    Dim dtTempSpotCheckClone As DataTable = dtTempSpotCheck.Clone
                    Dim dtTemp As DataTable = dtTempSpotCheck.Clone
                    Dim dtTempSpotCheckCopy = dtTempSpotCheck.Copy
                    If dtTempSpotCheck.Columns().Contains("[ChildId_TemperatureSpotCheck CCP Data]") Then
                        For intRowCount = 0 To dtTempSpotCheck.Rows.Count - 1
                            If Not IsDBNull(dtTempSpotCheck.Rows(intRowCount).Item("[ChildId_TemperatureSpotCheck CCP Data]")) Then
                                intFormid = dtTempSpotCheck.Rows(intRowCount).Item("[ChildId_TemperatureSpotCheck CCP Data]")

                                strGridName = "wdgStartData"
                                strSourceTable = "pdn_GridData"
                                getCCPData(dtTempSpotCheckCopy, dtTemp, strGridName, intFormid, strSourceTable, intRowCount)

                                strGridName = "wdgSpotTempData"
                                strSourceTable = "pdn_GridData"
                                getCCPData(dtTempSpotCheck, dtTempSpotCheckClone, strGridName, intFormid, strSourceTable, intRowCount)

                            End If
                        Next
                    End If
                    'populate dtTemp with correct tracecode data (for ingredients)
                    Dim drSearch() As DataRow
                    For intRowCount = 0 To dtTemp.Rows.Count - 1
                        If Not IsDBNull(dtTemp.Rows(intRowCount).Item("Field3")) Then
                            strSearch = "Product_Name = '" & Replace(dtTemp.Rows(intRowCount).Item("Field3").ToString, "'", "''") + "'"
                            drSearch = dtRepCopy.Select(strSearch)
                            If drSearch.Length > 0 Then
                                dtTemp.Rows(intRowCount).Item("TraceCodeDesc") = drSearch(0).Item("TraceCodeDesc")
                                dtTemp.Rows(intRowCount).Item("Generation") = drSearch(0).Item("Generation")
                            End If
                        Else
                            dtTemp.Rows(intRowCount).Item("TraceCodeDesc") = ""
                            dtTemp.Rows(intRowCount).Item("Generation") = ""
                        End If
                    Next

                    lblDocketNum.Text = Me.CurrentSession.VT_DocketNumber


                    ''Add Blank Rows as separator (if necessary)
                    'If dtDataClone IsNot Nothing AndAlso dtDataClone.Rows.Count > 0 Then
                    '    dtDataClone = AddBlankRow(dtRep, dtDataClone)
                    'End If
                    'If dtTemp IsNot Nothing AndAlso dtTemp.Rows.Count > 0 Then
                    '    dtTemp = AddBlankRow(dtRep, dtTemp)
                    'End If
                    'If dtTempSpotCheckClone IsNot Nothing AndAlso dtTempSpotCheckClone.Rows.Count > 0 Then
                    '    dtTempSpotCheckClone = AddBlankRow(dtRep, dtTempSpotCheckClone)
                    'End If

                    If dtDataClone.Rows.Count > 0 Then
                        objP.BindDataToWDG(dtDataClone, wdgCookCoolData)
                    End If
                    If dtTemp.Rows.Count > 0 Then
                        objP.BindDataToWDG(dtTemp, wdgStartData)
                    End If
                    If dtTempSpotCheckClone.Rows.Count > 0 Then
                        objP.BindDataToWDG(dtTempSpotCheckClone, wdgSpotTempData)
                    End If
                    objP.BindDataToWDG(dtHierarchy, wdgIngredientsHierarchy)

                    'Process the rows and set column colours
                    Dim intRowCnt As Integer
                    For intRowCnt = 0 To (wdgIngredientsHierarchy.Rows.Count - 1)

                        If IsDBNull(wdgIngredientsHierarchy.Rows(intRowCnt).Items.FindItemByKey("MatrixLinkId").Value) Then
                            For intColCount = 0 To wdgIngredientsHierarchy.Rows(intRowCnt).Items.Count - 1
                                wdgIngredientsHierarchy.Rows(intRowCnt).Items.Item(intColCount).CssClass = "GreenText"
                            Next
                        End If
                    Next

                    RunReport = True
                End If
            End If
        End If

    End Function
    Protected Function AddBlankRow(ByRef dtRep As DataTable, ByVal dtInput As DataTable) As DataTable
        'Order the data
        dtInput.DefaultView.Sort = "LineItemNum ASC"
        Dim dv As Data.DataView = dtInput.DefaultView
        dtInput = dv.ToTable
        Dim dtDataClone2 As DataTable = dtInput.Clone
        'add dividing blank line if required
        Dim objG As New BPADotNetCommonFunctions.VT_ReportFunctions.GeneralFunctions
        Dim strLineItemNo As String
        Dim dtSort As DataTable
        Dim drSort As DataRow
        Dim strSearch As String
        For intRowCount = 0 To dtRep.Rows.Count - 1
            strLineItemNo = dtRep.Rows(intRowCount).Item("LineItemNum")
            strSearch = "LineItemNum = '" & strLineItemNo & "'"
            dtSort = objG.SearchDataTable(strSearch, dtInput)
            If dtSort IsNot Nothing AndAlso dtSort.Rows.Count > 0 Then
                If intRowCount < dtRep.Rows.Count - 1 Then
                    drSort = dtSort.NewRow
                    dtSort.Rows.Add(drSort)
                End If
                dtDataClone2.Merge(dtSort)
            End If
        Next
        AddBlankRow = dtDataClone2


    End Function
    Protected Sub getCCPData(ByRef dtInput As DataTable, ByRef dtOutput As DataTable, ByVal strGridName As String, ByVal intFormId As Integer, ByVal strSourceTable As String, ByVal intRowCount As Integer)
        Dim objVTDBF As New VTMatrixFunctions
        Dim drNew As DataRow
        Dim dt As DataTable

        dt = objVTDBF.GetMatrix2GridDataForFormId(Session("_VT_DotNetConnString"), intFormId, strGridName, strSourceTable, "D")
        For intInnerRowCount = 0 To dt.Rows.Count - 1
            drNew = dtInput.Rows(intRowCount)
            drNew.Item("GridName") = dt.Rows(intInnerRowCount).Item("GridName").ToString.Substring(3)
            drNew.Item("Field0") = dt.Rows(intInnerRowCount).Item("Field0")
            drNew.Item("Field1") = dt.Rows(intInnerRowCount).Item("Field1")
            drNew.Item("Field2") = dt.Rows(intInnerRowCount).Item("Field2")
            drNew.Item("Field3") = dt.Rows(intInnerRowCount).Item("Field3")
            drNew.Item("Field4") = dt.Rows(intInnerRowCount).Item("Field4")
            drNew.Item("Field5") = dt.Rows(intInnerRowCount).Item("Field5")
            drNew.Item("Field6") = dt.Rows(intInnerRowCount).Item("Field6")
            drNew.Item("Field7") = dt.Rows(intInnerRowCount).Item("Field7")
            drNew.Item("Field8") = dt.Rows(intInnerRowCount).Item("Field8")
            drNew.Item("Field9") = dt.Rows(intInnerRowCount).Item("Field9")
            dtOutput.ImportRow(drNew)
        Next
        dt.Rows.Clear()
    End Sub

    Protected Sub addColumns(ByRef dtInput As DataTable)
        dtInput.Columns.Add("TraceCodeDesc")
        dtInput.Columns.Add("ProductId")
        dtInput.Columns.Add("TraceCodeId")
        dtInput.Columns.Add("Generation", GetType(Integer))
        dtInput.Columns.Add("Product_Name")
        dtInput.Columns.Add("Parent_Product_Name")
        dtInput.Columns.Add("LineItemNum")
    End Sub

    Public Function GetIngredientsAdded(ByVal lngTraceID As Long) As DataSet
        Dim myConnection As New SqlConnection(Session("_VT_DotNetConnString"))
        Dim comSQL As New SqlCommand("trc_resGetProductionForParent", myConnection)
        Dim adpSQL As New SqlDataAdapter(comSQL)
        Dim dsadded As New DataSet
        Dim objParam As SqlParameter

        comSQL.CommandType = CommandType.StoredProcedure
        objParam = comSQL.Parameters.Add("@ParentID", SqlDbType.Int)
        objParam.Direction = ParameterDirection.Input
        objParam.Value = lngTraceID
        myConnection.Open()
        adpSQL.Fill(dsadded)
        GetIngredientsAdded = dsadded
        '  Close the connection when done with it.
        myConnection.Close()
    End Function

    Public Function GetBlockOfProductDataItems(ByVal strconn As String, ByVal strSQLIn As String, ByVal astrItems() As String, ByRef dt As DataTable, ByVal strKeyField As String) As String


        Dim intCnt As Integer = 0
        Dim intstart, intstop, intRemainder As Integer
        Dim dtOutput, dtOut As New DataTable
        Dim dtData As New DataTable
        Dim strInClause As String
        Dim strSQL As String

        Dim myConnection As New SqlConnection(strconn)
        myConnection.Open()

        'initialise the control variable for the first loop through
        If (dt.Rows.Count - 1) > 500 Then
            intstart = 0
            intstop = 500
        Else
            intstart = 0
            intstop = (dt.Rows.Count - 1)
        End If

        'This is the main 'Outer' Loop
        While intCnt <= (dt.Rows.Count - 1)

            'build the string to specify the WHERE clause 
            'NOTE: This is done in "'Chunks' of 500 items (or less) to avoid the WHERE clause becoming too big
            strInClause = ""
            For intCnt = intstart To intstop

                If dt.Rows(intCnt).Item(strKeyField).ToString <> "" Then
                    If intCnt > intstart And strInClause <> "" Then
                        strInClause = strInClause & ","
                    End If
                    strInClause = strInClause + dt.Rows(intCnt).Item(strKeyField).ToString
                End If
            Next


            strSQL = strSQLIn & strInClause & ")"
            Dim comSQL As New SqlCommand(strSQL, myConnection)
            comSQL.CommandTimeout = 300
            Dim myAdapter As New SqlDataAdapter(comSQL)
            myAdapter.Fill(dtOutput)
            Dim intCounter As Integer = dtOutput.Rows.Count
            strSQL = ""
            'Now Fill in the relevant DataItem values to the appropriate columns in the 'dt' datatable 
            Dim strSearch As String
            Dim dr() As DataRow

            For intCnt = intstart To intstop
                If Not IsDBNull(dt.Rows(intCnt).Item(strKeyField)) Then
                    strSearch = "TraceCodeId = '" & dt.Rows(intCnt).Item(strKeyField).ToString + "'"
                    dr = dtOutput.Select(strSearch)
                    If dr.Length > 0 Then
                        If dtOutput.Columns(3).ColumnName.ToString = "Product_Name" Then
                            dt.Rows(intCnt).Item("ProductId") = dr(0).Item("ProductId")
                            dt.Rows(intCnt).Item("TraceCodeDesc") = dr(0).Item("TraceCodeDesc")
                            dt.Rows(intCnt).Item("Product_Name") = dr(0).Item("Product_Name")
                        Else
                            dt.Rows(intCnt).Item("Parent_Product_Name") = dr(0).Item("Parent_Product_Name")
                        End If
                    End If
                End If
            Next
            dtOutput.Rows.Clear()

            'Initialise control variable for the next loop (if necessary)
            intstart = intstop + 1
            If (dt.Rows.Count - 1) > intstart Then
                If (dt.Rows.Count - 1) > (intstart + 500) Then
                    intstop = intstop + 500
                Else
                    intRemainder = (dt.Rows.Count - 1) - intstop
                    intstop = intstop + intRemainder
                End If
            Else
                intstop = (dt.Rows.Count - 1)
            End If

        End While

        myConnection.Close()

    End Function

    Public Function GetBlockOfBatchDataItems(ByVal strconn As String, ByVal astrItems() As String, ByRef dt As DataTable, ByVal strKeyField As String, ByVal strKeyField2 As String) As String
        Dim intCnt As Integer = 0
        Dim intstart, intstop, intRemainder As Integer
        Dim dtOutput As New DataTable
        Dim dtData As New DataTable
        Dim strTarceCodeDesc, strProductId As String
        Dim strSQL As String = ""

        Dim intNumItems As Integer = astrItems.Length - 1
        'Add the required Columns
        For intCnt = 0 To intNumItems
            dt.Columns.Add(astrItems(intCnt).ToString)
        Next

        Dim myConnection As New SqlConnection(strconn)
        myConnection.Open()

        'initialise the control variable for the first loop through
        If (dt.Rows.Count - 1) > 500 Then
            intstart = 0
            intstop = 500
        Else
            intstart = 0
            intstop = (dt.Rows.Count - 1)
        End If

        'This is the main 'Outer' Loop
        While intCnt <= (dt.Rows.Count - 1)

            'build the string to specify the WHERE clause 
            'NOTE: This is done in "'Chunks' of 500 items (or less) to avoid the WHERE clause becoming too big

            strSQL = strSQL + "SELECT JobId, MatrixLinkId, JobIdDesc, ProductId  from wfo_Batchtable WHERE "
            For intCnt = intstart To intstop
                If dt.Rows(intCnt).Item(strKeyField).ToString <> "" Then
                    If dt.Rows(intCnt).Item(strKeyField2).ToString <> "" Then
                        strTarceCodeDesc = dt.Rows(intCnt).Item("TraceCodeDesc")
                        strProductId = dt.Rows(intCnt).Item("ProductId")
                        strSQL = strSQL + " (JobIdDesc = '" + strTarceCodeDesc + "' AND ProductId = '" + strProductId + "'  AND ParentId <> 0) OR"
                    End If
                End If
            Next

            strSQL = strSQL.Substring(0, strSQL.Length - 2)


            Dim comSQL As New SqlCommand(strSQL, myConnection)
            comSQL.CommandTimeout = 300
            Dim myAdapter As New SqlDataAdapter(comSQL)
            myAdapter.Fill(dtOutput)


            'Now Fill in the relevant DataItem values to the appropriate columns in the 'dt' datatable 
            Dim strSearch As String
            Dim dr() As DataRow

            For intCnt = intstart To intstop
                If Not IsDBNull(dt.Rows(intCnt).Item(strKeyField)) Then
                    If Not IsDBNull(dt.Rows(intCnt).Item(strKeyField2)) Then
                        strSearch = "JobIdDesc = '" & dt.Rows(intCnt).Item(strKeyField).ToString + "' AND ProductId = '" & dt.Rows(intCnt).Item(strKeyField2).ToString + "'"
                        dr = dtOutput.Select(strSearch)
                        If dr.Length > 0 Then
                            dt.Rows(intCnt).Item("JobId") = dr(0).Item("JobId")
                            dt.Rows(intCnt).Item("MatrixLinkId") = dr(0).Item("MatrixLinkId")
                        End If
                    End If
                End If
            Next

            dtOutput.Rows.Clear()
            'Initialise control variable for the next loop (if necessary)
            intstart = intstop + 1
            If (dt.Rows.Count - 1) > intstart Then
                If (dt.Rows.Count - 1) > (intstart + 500) Then
                    intstop = intstop + 500
                Else
                    intRemainder = (dt.Rows.Count - 1) - intstop
                    intstop = intstop + intRemainder
                End If
            Else
                intstop = (dt.Rows.Count - 1)
            End If

        End While

        myConnection.Close()
    End Function
    Public Function getProductInfoForDocketNum(ByVal strconn As String, ByVal strDocketNum As String) As DataTable

        Dim strsql As String = ""
        Dim dt As New DataTable

        strsql = "SELECT Distinct trc_transactions.DocketNum, trc_transactions.TraceCodeId, trc_transactions.ProductId, prd_ProductTable.Product_Name, trc_tracecodes.TraceCodeDesc"
        strsql = strsql + " FROM trc_transactions JOIN prd_ProductTable  ON trc_transactions.ProductId = prd_ProductTable.ProductId"
        strsql = strsql + " JOIN trc_tracecodes ON trc_transactions.TraceCodeId = trc_tracecodes.TraceCodeId"
        strsql = strsql + " WHERE (trc_transactions.DocketNum = '" + strDocketNum + "') "


        Dim myConnection As New SqlConnection(strconn)
        Dim comSQL As New SqlCommand(strsql, myConnection)

        comSQL.CommandTimeout = 300

        Dim myAdapter As New SqlDataAdapter(comSQL)
        myConnection.Open()
        myAdapter.Fill(dt)

        '  Close the connection when done with it.
        myConnection.Close()

        getProductInfoForDocketNum = dt

    End Function
End Class
