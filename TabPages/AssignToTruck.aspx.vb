
Partial Class TabPages_AssignToTruck
    Inherits MyBasePage

    Protected Sub TabPages_AssignToTruck_Load(sender As Object, e As EventArgs) Handles Me.Load
        Dim lngSalesOrderID As Long
        lngSalesOrderID = Me.CurrentSession.VT_SalesOrderID
        Dim objDatapreserve As New BPADotNetCommonFunctions.VT_DataPreserve.DataPreserve

        Dim dt As New Data.DataTable
        Dim objDB As New VTDBFunctions.VTDBFunctions.DBFunctions
        If Not IsPostBack Then
            dt = objDB.ExecuteSQLReturnDT("Select * from wfo_BatchTable where JobId = " & Me.CurrentSession.VT_SalesOrderNum)
            If dt.Rows.Count > 0 Then
                lblSelectedOrder.Text = "SO" & dt.Rows(0).Item("ContiguousRefNum")

            End If

            Dim dt1 As New Data.DataTable
            dt1.Columns.Add("DriverID")
            dt1.Columns.Add("Driver")

            objDatapreserve.BindDataToWDG(dt1, wdgDrivers)
            'wdgDrivers.DataSource = dt1
            'wdgDrivers.DataBind()

            PopulateDriversCombo()
            PopulateLocationsCombo()
        End If

    End Sub

    Public Sub PopulateDriversCombo()

        Dim dt As Data.DataTable
        Dim objProd As New VTDBFunctions.VTDBFunctions.DBFunctions


        ddlDrivers.Items.Clear()

        dt = objProd.ExecuteSQLReturnDT("SELECT FirstName + ' ' + LastName AS DriverName, DriverId FROM tcd_tblDrivers")

        ddlDrivers.DataSource = dt
        ddlDrivers.DataTextField = "DriverName"
        ddlDrivers.DataValueField = "DriverId"
        ddlDrivers.DataBind()

    End Sub

    Public Sub PopulateLocationsCombo()
        Dim ds As Data.DataSet
        Dim dt As Data.DataTable
        Dim objProd As New VTDBFunctions.VTDBFunctions.TraceFunctions


        ddlTrucks.Items.Clear()

        ds = objProd.GetAllLocations

        dt = ds.Tables(0)
        For Each dr As Data.DataRow In dt.Rows
            dr.Item("LocationText") = Trim(dr.Item("LocationText"))
        Next

        ddlTrucks.DataSource = dt
        ddlTrucks.DataTextField = "LocationText"
        ddlTrucks.DataValueField = "LocationID"
        ddlTrucks.DataBind()



    End Sub

    Protected Sub btnBack1_Click(sender As Object, e As ImageClickEventArgs) Handles btnBack1.Click
        Response.Redirect("WarehouseManager.aspx")

    End Sub

    Protected Sub btnSaveAndClose_Click(sender As Object, e As EventArgs) Handles btnSaveAndClose.Click
        Dim objMatrix As New VTDBFunctions.VTDBFunctions.VTMatrixFunctions
        Dim SalesOrderFormListTable As String = "tls_FormList"
        Dim SalesOrderFormDataTable As String = "tls_FormData"
        Dim SalesOrderModuleRootNode As String = "SalesOrderModuleRoot"
        Dim SalesOrderModuleCategoriesNode As String = "SalesOrderCategories"

        Dim lngRootId As Long = objMatrix.GetFormId(Session("_VT_DotNetConnString"), SalesOrderFormListTable, SalesOrderModuleRootNode, 0)
        Dim lngOrderFolder As Long = objMatrix.GetFormId(Session("_VT_DotNetConnString"), SalesOrderFormListTable, SalesOrderModuleCategoriesNode, lngRootId)
        Dim lngOrderCategory As Long
        Dim intMatrixLink As Integer
        Dim lngDetailsForm As Long

        Dim i As Integer
        Dim strDriverIDs As String
        Dim strDriverNames As String

        Dim dtSource As New Data.DataTable

        Dim objDataPreserve As New BPADotNetCommonFunctions.VT_DataPreserve.DataPreserve
        If Not Session("DriversGridDT") Is Nothing Then
            dtSource = Session("DriversGridDT")
        Else
            dtSource.Columns.Add("DriverId")
            dtSource.Columns.Add("Driver")
        End If

        For i = 0 To dtSource.Rows.Count - 1
            strDriverIDs = strDriverIDs & dtSource.Rows(i).Item("DriverID") & ","
            strDriverNames = strDriverNames & dtSource.Rows(i).Item("Driver") & ","
        Next

        lngOrderCategory = objMatrix.GetFormId(Session("_VT_DotNetConnString"), SalesOrderFormListTable, "Standard", lngOrderFolder)

        intMatrixLink = objMatrix.GetFormId(Session("_VT_DotNetConnString"), SalesOrderFormListTable, CStr(Me.CurrentSession.VT_SalesOrderNum), lngOrderCategory)

        lngDetailsForm = objMatrix.GetFormId(Session("_VT_DotNetConnString"), SalesOrderFormListTable, "Details", intMatrixLink)

        objMatrix.InsertVTMatrixFormDataItem(Session("_VT_DotNetConnString"), SalesOrderFormDataTable, lngDetailsForm, "DriverIDs", strDriverIDs)
        objMatrix.InsertVTMatrixFormDataItem(Session("_VT_DotNetConnString"), SalesOrderFormDataTable, lngDetailsForm, "DriverNames", strDriverNames)
        objMatrix.InsertVTMatrixFormDataItem(Session("_VT_DotNetConnString"), SalesOrderFormDataTable, lngDetailsForm, "TruckName", ddlTrucks.SelectedItem.Text)
        objMatrix.InsertVTMatrixFormDataItem(Session("_VT_DotNetConnString"), SalesOrderFormDataTable, lngDetailsForm, "TruckId", ddlTrucks.SelectedValue)

        'save the truck id as the location for all items on that order in the HH table
        Dim objDB As New VTDBFunctions.VTDBFunctions.DBFunctions

        objDB.ExecuteSQLQuery("Update tcd_tblSalesOrders set Location = " & ddlTrucks.SelectedValue & " where OrderNumber = '" & Me.CurrentSession.VT_SalesOrderNum & "'")

        Response.Redirect("WarehouseManager.aspx")
    End Sub

    Protected Sub cmdAdd_Click(sender As Object, e As EventArgs) Handles cmdAdd.Click
        Dim dt As New Data.DataTable
        If wdgDrivers.Rows.Count > 0 Then
            dt = Session("DriversGridDT")
        Else
            dt.Columns.Add("DriverID")
            dt.Columns.Add("Driver")
        End If

        Dim dr As Data.DataRow

        dr = dt.Rows.Add
        dr.Item("DriverId") = ddlDrivers.SelectedItem.Value
        dr.Item("Driver") = ddlDrivers.SelectedItem.Text

        Session("DriversGridDT") = dt

        wdgDrivers.ClearDataSource()
        wdgDrivers.DataSource = dt
        wdgDrivers.DataBind()

    End Sub

    Protected Sub btnClear_Click(sender As Object, e As EventArgs) Handles btnClear.Click
        Dim dt1 As New Data.DataTable
        dt1.Columns.Add("DriverID")
        dt1.Columns.Add("Driver")

        wdgDrivers.ClearDataSource()
        Session("DriversGridDT") = dt1
        wdgDrivers.DataSource = dt1
        wdgDrivers.DataBind()
    End Sub
End Class
