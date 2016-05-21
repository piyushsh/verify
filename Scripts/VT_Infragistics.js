/*
Use this file for functions that deal with Infragistics controls
*/

/*
Functions for WebDataGrids
*/
// function to get a reference to a grid
// You must pass it the ClientId of the grid. In a Master-Child page situation this is usually in the form "<%=Calibrations.ClientID%>"
// returns: a reference to the grid or null if the named grid is not found

// Created by John Gleeson 21/10/2013
// Modified by:
function getGrid(gridClientId) {
    return $find(gridClientId);
}

// function to check if there is an active row on the supplied grid
// You must pass it the ClientId of the grid. In a Master-Child page situation this is usually in the form "<%=Calibrations.ClientID%>"
// returns: a reference to the active cell or null if none is active

// Created by John Gleeson 12/09/2013
// Modified by:
function getActiveCell(gridClientId) {
    var grid = $find(gridClientId);
    var myCell = grid.get_behaviors().get_activation().get_activeCell();
    return myCell;
}

// function to return the row on which a cell is located
// You must pass it a reference to a cell
// returns: a reference to the row 

// Created by John Gleeson 12/09/2013
// Modified by:
function getRowForCell(theCell) {
    return theCell.get_row();
}


// function to return the index of a row 
// You must pass it a reference to a row
// returns: index of the row 

// Created by Judy Walsh 09/10/2013
// Modified by:
function getIndexForRow(theRow) {
    return theRow.get_index();
}

// function to return the value of a cell (identified by its key) from a row
// You must pass it a reference to a row and a key string
// returns: a reference to the row 

// Created by John Gleeson 12/09/2013
// Modified by:
function getCellValueFromKey(theRow, theKey) {
    return theRow.get_cellByColumnKey(theKey).get_value();
}

// function to SET the value of a cell (identified by its key) from a row
// You must pass it a reference to a row and a key string
// returns: nothing is returned
// Created by SmcN 12/09/2013
// Modified by:
function SetCellValueFromKey(theRow, theKey, setValue) {
    theRow.get_cellByColumnKey(theKey).set_value(setValue) ;
}




/*
End of Functions for WebDataGrids
*/

/*
Functions for WebDateChooser
*/

// function to get a reference to a WebDateChooser
// You must pass it the ClientId of the WebDateChooser. In a Master-Child page situation this is usually in the form "<%=Calibrations.ClientID%>"
// returns: a reference to the WebDatChooser or null if not found

// Created by John Gleeson 12/09/2013
// Modified by:
function getWebDateChooser(theClientId) {
    return igdrp_getComboById(theClientId);
}



/*
End of Functions for WebDateChooser
*/

