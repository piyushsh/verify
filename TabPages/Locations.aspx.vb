Imports BPADotNetCommonFunctions

Partial Class LocationsPage

    Inherits MyBasePage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim objDataPreserve As New BPADotNetCommonFunctions.VT_DataPreserve.DataPreserve
        If Not IsPostBack Then

            btnDelete.Attributes.Add("onclick", String.Format("return ConfirmDeleteLocal('{0}');", wdgLocations.UniqueID))
            btnAdd.Attributes.Add("onclick", "return LoadPanel('Add');")
            btnEdit.Attributes.Add("onclick", "return LoadPanel('Edit');")
            cmdOk.OnClientClick = String.Format("return LocationClose('{0}','{1}')", cmdOk.UniqueID, "")

            LoadLocations()
        Else
            wdgLocations.DataSource = objDataPreserve.GetWDGDataFromSession(wdgLocations)
        End If
    End Sub
    Sub LoadLocations()
        ' load the Locations into the grid
        Dim objTrace As New VTDBFunctions.VTDBFunctions.TraceFunctions
        Dim dsLoc As Data.DataSet = objTrace.GetAllLocations
        Dim objDataPreserve As New BPADotNetCommonFunctions.VT_DataPreserve.DataPreserve

        objDataPreserve.BindDataToWDG(dsLoc.Tables(0), wdgLocations)

        'wdgLocations.DataSource = dsLoc
        'wdgLocations.DataBind()

    End Sub
    Protected Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnDelete.Click
        Dim objT As New VTDBFunctions.VTDBFunctions.TraceFunctions
        Dim objDB As New VTDBFunctions.VTDBFunctions.DBFunctions

        If Session("LocationsGridClickedRow") Is Nothing Then Exit Sub

        Dim intLocationId As Integer = Session("LocationsGridClickedRow").rows(0).item("LocationId")

        ' Location with Id 0 cannot be deleted
        If intLocationId = 0 Then
            Master.ShowMessagePanel("You cannot delete the Location with Id 0. You can use the Edit function to change its Details if required.")
            Exit Sub
        End If

        ' if there are entries in the LocationData table for this Location it cannot be deleted
        Dim dtData As Data.DataTable = objT.GetLocationContentsForLocationId(intLocationId)
        If dtData IsNot Nothing AndAlso dtData.Rows.Count = 0 Then
            ' objT.DeleteLocationFromSchema(intLocationId)
            objDB.ExecuteSQLQuery("Delete from trc_Locations where LocationID =" & intLocationId)

            'refill the locations grid
            LoadLocations()
            Master.ShowMessagePanel("The location has been sucessfully deleted")
        Else
            Master.ShowMessagePanel("You cannot delete this Location because it contains Products")
            Exit Sub
        End If


    End Sub

    Protected Sub cmdOk_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles cmdOk.Click

        Dim intRowCount As Integer
        Dim objTrace As New VTDBFunctions.VTDBFunctions.TraceFunctions
        Dim objDB As New VTDBFunctions.VTDBFunctions.DBFunctions


        Dim intLocationId As Integer
        Dim dt As New Data.DataTable
        If Not Session("LocationsGridClickedRow") Is Nothing Then
            dt = Session("LocationsGridClickedRow")
        End If

        If dt.Rows.Count > 0 Then
            intLocationId = dt.Rows(0).Item("LocationId")

            If hdnAddOrEdit.Value = "Edit" Then
                objDB.ExecuteSQLQuery("Update trc_Locations set LocationText = '" & txtLocationText.Text & "', LocationPosition= '" & txtPosition.Text & "' where LocationId = " & intLocationId)

                LoadLocations()

            Else
                ' check that the LocationText is unique
                dt = objDB.ExecuteSQLReturnDT("Select * from trc_Locations where LocationText='" & txtLocationText.Text & "'")
                If dt.Rows.Count > 0 Then

                    Master.ShowMessagePanel("There is already a Location with this Text. The LocationText must be unique!")
                    Exit Sub
                End If

                Dim intLastAddedLocation = objTrace.GetLastAddedLocation
                If intLastAddedLocation = -2 Then
                    ' an error occurred
                    Master.ShowMessagePanel("Error accessing Locations table. Check the Log file for details of the error.")
                    Exit Sub
                Else
                    intLocationId = intLastAddedLocation + 1
                    ' insert the new location
                    objTrace.InsertNewLocation(intLocationId, txtLocationText.Text)
                    objTrace.SetLocationPositionForId(intLocationId, txtPosition.Text)

                End If
                LoadLocations()

            End If
        End If



    End Sub

    Protected Sub wdgLocations_ActiveCellChanged(sender As Object, e As Infragistics.Web.UI.GridControls.ActiveCellEventArgs) Handles wdgLocations.ActiveCellChanged
        Dim objC As New BPADotNetCommonFunctions.VT_CommonFunctions.CommonFunctions
     
        'Serialise the Selected DataRow
        Dim intActiveRowIndex As Integer = wdgLocations.Behaviors.Activation.ActiveCell.Row.Index
       

        Session("LocationsGridClickedRow") = objC.SerialiseWebDataGridRow(wdgLocations, intActiveRowIndex)

        wdgLocations.Behaviors.Activation.ActiveCell = wdgLocations.Rows(intActiveRowIndex).Items(1)

    End Sub



    Protected Sub wdgLocations_RowSelectionChanged(sender As Object, e As Infragistics.Web.UI.GridControls.SelectedRowEventArgs) Handles wdgLocations.RowSelectionChanged
        Dim objC As New BPADotNetCommonFunctions.VT_CommonFunctions.CommonFunctions

        'Serialise the Select DataRow
        Dim RowSelected As Infragistics.Web.UI.GridControls.SelectedRowCollection = wdgLocations.Behaviors.Selection.SelectedRows
        Dim intActiveRowIndex As Integer = RowSelected(0).Index

        Session("LocationsGridClickedRow") = objC.SerialiseWebDataGridRow(wdgLocations, intActiveRowIndex)

        wdgLocations.Behaviors.Activation.ActiveCell = wdgLocations.Rows(intActiveRowIndex).Items(1)
   
    End Sub
End Class
