//
// Function to check if the browser supports local storage. if it does it will support sessionStorage also
function supports_local_storage() {
    try {
        return 'localStorage' in window && window['localStorage'] !== null;
    } catch (e) {
        return false;
    }
}

// function to disable a button on a panel and then cause it to postback. Diabling the button prevents multiple clicks
function VTPostback(sender, e) {
    // disable the button
   var theButton = document.getElementById(sender);
   theButton.disabled = true;

    __doPostBack(sender, e);
}

// function to round the value in num to the number of decimal places in dec
function roundNumber(num, dec) {
    var result = Math.round(num * Math.pow(10, dec)) / Math.pow(10, dec);
    return result;
}


///////////////////////////////
function SplitBarcode(TheData) {

    // Split the barcode in TheData into its EAN128 components

    var strCode;
    var strSplitting;
    var strTerminator;
    strTerminator = '$';

    var BarcodeElements = {
        "SSCC": "",
        "GTIN": "",
        "BatchNumber": "",
        "ProductionDate": "",
        "SellByDate": "",
        "NumberOfUnits": "",
        "Weight": "",
        "Length": "",
        "Width": "",
        "Depth": "",
        "Area": "",
        "AI92": "",
        "AI93": "",
        "AI94": "",
        "AI95": "",
        "AI96": "",
        "AI97": "",
        "AI98": "",
        "AI99": ""
    };

    //310200120094564876$101234567891234

    strSplitting = TheData;
    while (strSplitting.length > 2) {
        // ignore any non-numeric keys
        if (!isNumeric(Left(strSplitting, 1)))
            strSplitting = Mid(strSplitting, 1, (strSplitting.length - 1));

        // look at the next 2 characters
        strCode = Left(strSplitting, 2);
        switch (strCode) {
            case '00':
                // 00 indicates that the next data is the SSCC code - 18 digits

                // jump over the 00
                strSplitting = Mid(strSplitting, 2, (strSplitting.length - 1));

                BarcodeElements.SSCC = Left(strSplitting, 18);
                // Move on to the next data
                strSplitting = Mid(strSplitting, 18, (strSplitting.length - 18))
                break;
            case '01':
                // 00 indicates that the next data is the GTIN ProductNumber - 14 digits

                // jump over the 01
                strSplitting = Mid(strSplitting, 2, (strSplitting.length - 1));

                BarcodeElements.GTIN = Left(strSplitting, 14);

                // Move on to the next data
                strSplitting = Mid(strSplitting, 14, (strSplitting.length - 14))
                break;
            case '10':
                // 10 indicates that the next data is the BatchNumber - 1 to 20 alphanumeric
                // This is either terminated by strTerminator or it comes at the end of the Barcode

                // jump over the 10
                strSplitting = Mid(strSplitting, 2, (strSplitting.length - 1));

                // Find the Terminator
                var intLocn;
                intLocn = strSplitting.indexOf(strTerminator);
                if (intLocn == -1) {
                    // terminator not found so the data is at the end of the Barcode
                    BarcodeElements.BatchNumber = strSplitting;
                    strSplitting = "";
                }
                else {
                    BarcodeElements.BatchNumber = Left(strSplitting, intLocn);
                    // Move on to the next data
                    strSplitting = Mid(strSplitting, intLocn + 1, (strSplitting.length - intLocn - 1));
                }

                break;

            case '11':
                // 11 indicates that the next data is the ProductionDate - 6 digits YYMMDD

                // jump over the 15
                strSplitting = Mid(strSplitting, 2, (strSplitting.length - 1));

                BarcodeElements.ProductionDate = Left(strSplitting, 6);

                // Move on to the next data
                strSplitting = Mid(strSplitting, 6, (strSplitting.length - 6))
                break;

            case '15':
                // 15 indicates that the next data is the SellByDate - 6 digits YYMMDD

                // jump over the 15
                strSplitting = Mid(strSplitting, 2, (strSplitting.length - 1));

                BarcodeElements.SellByDate = Left(strSplitting, 6);

                // Move on to the next data
                strSplitting = Mid(strSplitting, 6, (strSplitting.length - 6))
                break;

            case '21':
                //21 indicates that the next data is the Serial Number (for a box or case) - 9 digits 

                // jump over the 21

                strSplitting = Mid(strSplitting, 2, (strSplitting.length - 1));

                // Find the Terminator
                var intLocn;
                intLocn = strSplitting.indexOf(strTerminator);
                if (intLocn == -1) {
                    // terminator not found so the data is at the end of the Barcode
                    BarcodeElements.SerialNumber = strSplitting;
                    strSplitting = "";
                }
                else {
                    BarcodeElements.SerialNumber = Left(strSplitting, intLocn);
                    // Move on to the next data
                    strSplitting = Mid(strSplitting, intLocn + 1, (strSplitting.length - intLocn - 1));
                }

                break;


            case '31':
                var strSubCode;
                strSubCode = Mid(strSplitting, 2, 2);
                // the first character in the subCode indicates what this item is
                // 0 means weight, 1 means Length, 2 means Width, 3 means Depth, 4 means Area ...
                // we only return these but we could extend it in the future
                var strType = Left(strSubCode, 1);

                // the second character in the subCode tells us how many decimal places to use
                var intDecimal = parseInt(Right(strSubCode, 1));

                // jump over the 31xx
                strSplitting = Mid(strSplitting, 4, (strSplitting.length - 4));
                var strTemp;
                strTemp = Left(strSplitting, 6 - intDecimal);
                strTemp = strTemp + ".";
                strTemp = strTemp + Mid(strSplitting, (6 - intDecimal), intDecimal);

                switch (strType) {
                    case '0':
                        BarcodeElements.Weight = parseFloat(strTemp);
                        break;
                    case '1':
                        BarcodeElements.Length = parseFloat(strTemp);
                        break;
                    case '2':
                        BarcodeElements.Width = parseFloat(strTemp);
                        break;
                    case '3':
                        BarcodeElements.Depth = parseFloat(strTemp);
                        break;
                    case '4':
                        BarcodeElements.Area = parseFloat(strTemp);
                        break;
                }
                // Move on to the next data
                strSplitting = Mid(strSplitting, 6, (strSplitting.length - 6));

                break;

            case '37':
                //// 37 indicates that the next data is the Number of Units - 1 to 8 digits
                //// This is either terminated by strTerminator or it comes at the end of the Barcode

                //// jump over the 37
                //strSplitting = Mid(strSplitting, 2, (strSplitting.length - 1));

                //// Find the Terminator
                //var intLocn;
                //intLocn = strSplitting.indexOf(strTerminator);
                //if (intLocn == -1) {
                //    // terminator not found so the data is at the end of the Barcode
                //    BarcodeElements.NumberOfUnits = strSplitting;
                //    strSplitting = "";
                //}
                //else {
                //    BarcodeElements.NumberOfUnits = Left(strSplitting, intLocn);
                //    // Move on to the next data
                //    strSplitting = Mid(strSplitting, intLocn + 1, (strSplitting.length - intLocn - 1));
                //}

                //break;

                
                // 37 indicates that the next data is the Number of Units - 8 digits 
                //Changed for Sunflower and to match HH where 37 is treated as a fixed 8 digit lenght item.
                //05/03/14 JW

                // jump over the 37
                strSplitting = Mid(strSplitting, 2, (strSplitting.length - 1));

                BarcodeElements.NumberOfUnit = Left(strSplitting, 8);

                // Move on to the next data
                strSplitting = Mid(strSplitting, 6, (strSplitting.length - 8))
                break;


            case '92': case '93': case '94': case '95': case '96': case '97': case '98': case '99':
                // 92 - 99 indicates that what follows is a variable length Internal Company code
                // a Terminator sign indicates the end of the number. If there is no terminator assume the code is at the end of the BarCode

                // jump over the code
                strSplitting = Mid(strSplitting, 2, (strSplitting.length - 1));

                // Find the Terminator
                var intLocn;
                var strTemp;
                intLocn = strSplitting.indexOf(strTerminator);
                if (intLocn == -1) {
                    // terminator not found so the data is at the end of the Barcode
                    strTemp = strSplitting;
                    strSplitting = "";
                }
                else {
                    strTemp = Left(strSplitting, intLocn);
                    // Move on to the next data
                    strSplitting = Mid(strSplitting, intLocn + 1, (strSplitting.length - intLocn - 1));
                }

                switch (strCode) {
                    case '92':
                        BarcodeElements.AI92 = strTemp;
                        break;
                    case '93':
                        BarcodeElements.AI93 = strTemp;
                        break;
                    case '94':
                        BarcodeElements.AI94 = strTemp;
                        break;
                    case '95':
                        BarcodeElements.AI95 = strTemp;
                        break;
                    case '96':
                        BarcodeElements.AI96 = strTemp;
                        break;
                    case '97':
                        BarcodeElements.AI97 = strTemp;
                        break;
                    case '98':
                        BarcodeElements.AI98 = strTemp;
                        break;
                    case '99':
                        BarcodeElements.AI99 = strTemp;
                        break;
                }

                break;
            default:
                alert("AI code not recognised. Not a valid EAN128 barcode. Contact technical support");
                strSplitting = "";
                break;
        }
    }

    return BarcodeElements;
}


function Mid(str, start, len) {
    /***
    IN: str - the input string
    start - our string's starting position (0 based!!)
    len - how many characters from start we want to get

    RETVAL: The substring from start to start+len
    ***/

    // Make sure start and len are within proper bounds
    if (start < 0 || len < 0) return "";

    var iEnd, iLen = String(str).length;
    if (start + len > iLen)
        iEnd = iLen;
    else
        iEnd = start + len;

    return String(str).substring(start, iEnd);
}


function Left(str, n) {
    /***
    IN: str - the input string
    n - how many characters from start we want to get

    RETVAL: The first n characters
    ***/
    if (n <= 0)
        return "";
    else if (n > String(str).length)
        return str;
    else
        return String(str).substring(0, n);
}

function Right(str, n) {
    /***
    IN: str - the input string
    n - how many characters from the end we want to get

    RETVAL: The last n characters
    ***/
    if (n <= 0)
        return "";
    else if (n > String(str).length)
        return str;
    else {
        var iLen = String(str).length;
        return String(str).substring(iLen, iLen - n);
    }
}

function leftTrim(sString) {
    /***
    IN: str - the input string
    RETVAL: The input with leading spaces removed
    ***/
    while (sString.substring(0, 1) == ' ') {
        sString = sString.substring(1, sString.length);
    }
    return sString;
}

function rightTrim(sString) {
    /***
    IN: str - the input string
    RETVAL: The input with trailing spaces removed
    ***/
    while (sString.substring(sString.length - 1, sString.length) == ' ') {
        sString = sString.substring(0, sString.length - 1);
    }
    return sString;
}


// checks a character to see if it is numeric
function isNumeric(TheChar) {
    // allow .(46), " "(32) and 0 - 9 characters
    if ((TheChar == '.') || (TheChar == ' ') || (TheChar >= '0' && TheChar <= '9'))
        return true;

    return false;
}


function isNumberKey(evt) {
    var charCode = (evt.which) ? evt.which : event.keyCode
    // allow Backspace(8), .(46), " "(32) and 0 - 9 characters
    if ((charCode == 8) || (charCode == 46) || (charCode == 32) || (charCode >= 48 && charCode <= 57))
        return true;

    return false;
}

function isAlphaKey(evt) {
    var charCode = (evt.which) ? evt.which : event.keyCode
    // allow Backspace(8), /(47), \(92) and upper and lower characters
    if ((charCode == 8) || (charCode == 47) || (charCode == 92) || (charCode > 64 && charCode < 91) || (charCode > 96 && charCode < 123))
        return true;

    return false;
}

function isAlphaNumericKey(evt) {
    var charCode = (evt.which) ? evt.which : event.keyCode
    // allow Backspace(8), /(47), \(92) and upper and lower characters
    if ((charCode == 8) || (charCode == 47) || (charCode == 92) || (charCode > 64 && charCode < 91) || (charCode > 96 && charCode < 123))
        return true;
    // allow .(46), " "(32) and 0 - 9 characters
    if ((charCode == 46) || (charCode == 32) || (charCode >= 48 && charCode <= 57))
        return true;

    return false;
}

function Upper(e, r) {
    r.value = r.value.toUpperCase();
}

/*
// add an event Handler to trap the begin and end of the AJAX update process

This line will cause a client-side error if the script reference appears in the page before the ScriptManager is declared.
The best way to set it up is in the Scripts node of the ScriptManager definition
<asp:ScriptManager ID="ScriptManager1"    runat="server">
<Scripts>
<asp:ScriptReference Path="../Scripts/CommonScripts.js" />
</Scripts> 
</asp:ScriptManager>
*/
Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(BeginRequestHandler);

function BeginRequestHandler(sender, args) {
    ShowSpinner();
}


function ShowSpinner() {
    //SmcN 14/08/2014 This is the new 'Spin' contorl to show page loading and activity

    var opts = {
        lines: 13, // The number of lines to draw
        length: 20, // The length of each line
        width: 10, // The line thickness
        radius: 55, // The radius of the inner circle
        corners: 1, // Corner roundness (0..1)
        rotate: 18, // The rotation offset
        direction: 1, // 1: clockwise, -1: counterclockwise
        color: '#089850', // #rgb or #rrggbb or array of colors
        speed: 1.2, // Rounds per second
        trail: 40, // Afterglow percentage
        shadow: false, // Whether to render a shadow
        hwaccel: false, // Whether to use hardware acceleration
        className: 'spinner', // The CSS class to assign to the spinner
        zIndex: 2e9, // The z-index (defaults to 2000000000)
        top: '10px', // Top position relative to parent
        left: '50%' // Left position relative to parent
    };

    //SmcN get the posiiton for the spinner
    if (window.pageYOffset !== undefined) { // All browsers, except IE9 and earlier
        var offset1 = window.pageYOffset + 400;
    } else { // IE9 and earlier
        var offset1 = window.parent.document.documentElement.scrollTop + 400;
    }

    //SmcN get the posiiton for the spinner
    var TopOffset = offset1 + 'px';



    //Show the 'Please wait' Spinner
    var target = document.getElementById('SheaBusyBox');
    if (target != null) {
        target.style.top = TopOffset;
        target.style.visibility = 'visible';
        var spinner = new Spinner(opts).spin(target);
    }

    var target1 = document.getElementById('SheaBusyBoxPopUp');
    if (target1 != null) {
        target1.style.top = TopOffset;
        target1.style.visibility = 'visible';
        var spinner = new Spinner(opts).spin(target1);
    }

    var target2 = document.getElementById('SheaBusyBoxPopUpAdd');
    if (target2 != null) {
        target2.style.top = TopOffset;
        target2.style.visibility = 'visible';
        var spinner = new Spinner(opts).spin(target2);
    }


}

Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);

function EndRequestHandler(sender, args) {
    HideSpinner();
}

function HideSpinner() {
    //Turn off the 'Please wait' Spinner
    var target = document.getElementById('SheaBusyBox');
    if (target != null) {
        target.style.visibility = 'hidden';
        var spinner = new Spinner().stop(target);
    }

    var target1 = document.getElementById('SheaBusyBoxPopUp');
    if (target1 != null) {
        target1.style.visibility = 'hidden';
        var spinner = new Spinner().stop(target1);
    }

    var target2 = document.getElementById('SheaBusyBoxPopUpAdd');
    if (target2 != null) {
        target2.style.visibility = 'hidden';
        var spinner = new Spinner().stop(target2);
    }


}

// this function will ensure that the div containing a grid uses the correct positioning
// this prevents the Overflow and FixedHeader problems
// the function must be declared as the InitializeLayoutHandler for the grid
function InitializeGridLayoutHandler(gridName) {
    //alert('jg');
    var grid = igtbl_getGridById(gridName);
    var div = grid.getDivElement();
    div.style.position = "relative";
}


// Set the value of a dropdown
function setDropDown(obj, value) {
    var o = obj.options;
    for (var i = 0; i < o.length; i++) {
        if (o[i].value == value) { o[i].selected = true; }
        else { o[i].selected = false; }
    }
    return true;
}

// Set the value of a dropdown
function setDropDownByText(obj, text) {
    var o = obj.options;
    for (var i = 0; i < o.length; i++) {
        if (o[i].text == text) { o[i].selected = true; }
        else { o[i].selected = false; }
    }
    return true;
}

// Is any item on a checkBoxList or RadioButtonList checked?
// Returns Yes if there is or No otherwise
function IsAnyItemChecked(cbl) {
    var chkText = 'No';
    var chktable = document.getElementById(cbl);
    var chktr = chktable.getElementsByTagName('tr');
    for (var i = 0; i < chktr.length; i++) {
        var chktd = chktr[i].getElementsByTagName('td');
        for (var j = 0; j < chktd.length; j++) {
            var chkinput = chktd[j].getElementsByTagName('input');
            for (k = 0; k < chkinput.length; k++) {
                var chkopt = chkinput[k];
                if (chkopt.checked) {
                    chkText = 'Yes';
                }
            }
        }
    }
    return (chkText);
}

// Get the value of the selected item from a RadioList
function getValueOfRadioList(radiolist) {
    var radio = radiolist.getElementsByTagName("input");

    for (var x = 0; x < radio.length; x++) {
        if (radio[x].type === "radio" && radio[x].checked) {
            return radio[x].value;
        }
    }
}

// function to check if a string is a valid eMail address
function validateEmail(elementValue) {
    // returns true if elementValue matches the pattern
    var emailPattern = /^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$/;
    return emailPattern.test(elementValue);
}


function ConfirmDelete(gridname) {
    var myrow = igtbl_getActiveRow(gridname);


    if (myrow == null) {
        alert('You must select an item to delete!');
        return false;
    }
    else {
        var answer = confirm("Are you sure you want to Delete this item?\n\n Click Ok if you wish to continue.")
        if (answer)
            return true;
        else
            return false;

    }

}


function UpdatePriceItems(e, decimals) {
    // function to update price items on a row in a grid
    // The grid name is passed to this function in   'e'
    // The grid must have columns names as follows
    //   Quantity; Weight; UnitPrice; TotalExclVAT; Price; VATCharged; VATRate; UnitOfSale
    // returns: nothing is returned

    // Created by SmcN 30/09/2013
    // Modified by:

    var cell = e.getCell();   //Returns the cell that is about to exit edit mode
    var row = cell.get_row();   //Gets reference to the row object that contains the cell 
    var value = cell.get_value();  // Returns the cell's value
    var index = row.get_index();  //Returns index of the row
    var column = cell.get_column();  //Gets reference to the column object that contains the cell
    var key = column.get_key();  //Gets the key of the column

    //Get the values needed for Price calculations and eliminate any Nulls
    var cQuantity = getCellValueFromKey(row, 'Quantity');
    if (cQuantity == null || cQuantity == 'NaN') { cQuantity = '0'; }
   

    var cWeight = getCellValueFromKey(row, 'Weight');
    if (cWeight == null || cWeight == 'NaN') { cWeight = '0'; }
  
    var cUnitPrice = getCellValueFromKey(row, 'UnitPrice');
    if (cUnitPrice == null || cUnitPrice == 'NaN') { cUnitPrice = '0'; }
    
    var cTotalExclVAT = getCellValueFromKey(row, 'TotalExclVAT');
    if (cTotalExclVAT == null || cTotalExclVAT == 'NaN') { cTotalExclVAT = '0'; }
    
    var cPrice = getCellValueFromKey(row, 'Price');
    if (cPrice == null || cPrice == 'NaN') { cPrice = '0'; }
    
    var cVatCharged = getCellValueFromKey(row, 'VATCharged');
    if (cVatCharged == null || cVatCharged == 'NaN') { cVatCharged = '0'; }
    
    var cVatRate = getCellValueFromKey(row, 'VATRate');
    if (cVatRate == null || cVatRate == 'NaN') { cVatRate = '0'; }
    
    //Convert everything to a number
    var nQuantity = new Number(cQuantity);
    var nWeight = new Number(cWeight);
    var nUnitPrice = new Number(cUnitPrice);
    var nTotalExclVAT = new Number(cTotalExclVAT);
    var nVatCharged = new Number(cVatCharged);
    var nPrice = new Number(cPrice);
    var nVatRate = new Number(cVatRate);

    var UnitOfSale = getCellValueFromKey(row, 'UnitOfSale');
    if (UnitOfSale == null || UnitOfSale == 'NaN') { UnitOfSale = '0'; }
    //var decimals = '<%=Session("_VT_MySessionInfo").VT_DecimalPlaces.ToString()%>';

    // Now recalulate values depending on which grid column was modified
    if (key == 'Quantity' && UnitOfSale == '1') {

        nTotalExclVAT = nUnitPrice * nQuantity;
        nVatCharged = nTotalExclVAT * (nVatRate / 100);
        nPrice = nTotalExclVAT + nVatCharged;
    }

    if (key == 'Weight' && UnitOfSale == '0') {

        nTotalExclVAT = nUnitPrice * nWeight;
        nVatCharged = nTotalExclVAT * (nVatRate / 100);
        nPrice = nTotalExclVAT + nVatCharged;
    }

    if (key == 'UnitPrice') {
        if (UnitOfSale == '1') {
            nTotalExclVAT = nUnitPrice * nQuantity;
        }
        else {
            nTotalExclVAT = nUnitPrice * nWeight;
        }

        nVatCharged = nTotalExclVAT * (nVatRate / 100);
        nPrice = nTotalExclVAT + nVatCharged;

    }

    if (key == 'TotalExclVAT') {
        if (UnitOfSale == '1') {
            if (nQuantity > '0') {
                nUnitPrice = nTotalExclVAT / nQuantity;
                
            }
            else {
                nUnitPrice = 0
            }

        }
        else {
            if (cWeight > '0') {
                nUnitPrice = nTotalExclVAT / nWeight;
                
            }
            else {
                nUnitPrice = 0
            }
        }

        nVatCharged = nTotalExclVAT * (nVatRate / 100);
        nPrice = nTotalExclVAT + nVatCharged;

    }

    if (key == 'VATCharged') {
        nPrice = nTotalExclVAT + nVatCharged;
        if (nTotalExclVAT > '0') {
            nVatRate = ((nPrice - nTotalExclVAT) / nTotalExclVAT) * 100 ;
        }

    }

    if (key == 'Price') {

        nTotalExclVAT = nPrice / (1 + (nVatRate / 100));
        nVatCharged = nTotalExclVAT * (nVatRate / 100);
        //Also recalulate the unit price
        if (UnitOfSale == '1') {
            if (nQuantity > '0') {
                nUnitPrice = nTotalExclVAT / nQuantity
            }
            else {
                nUnitPrice = 0
            }

        }
        else {
            if (cWeight > '0') {
                nUnitPrice = nTotalExclVAT / nWeight
            }
            else {
                nUnitPrice = 0
            }
        }

    }


    //Format Numbers and write to grid
    nQuantity = nQuantity.toFixed(0);
    SetCellValueFromKey(row, 'Quantity', nQuantity);

    nWeight = nWeight.toFixed(decimals);
    SetCellValueFromKey(row, 'Weight', nWeight);

    nUnitPrice = nUnitPrice.toFixed(decimals);
    SetCellValueFromKey(row, 'UnitPrice', nUnitPrice);

    nTotalExclVAT = nTotalExclVAT.toFixed(decimals);
    SetCellValueFromKey(row, 'TotalExclVAT', nTotalExclVAT);

    nVatCharged = nVatCharged.toFixed(decimals);
    SetCellValueFromKey(row, 'VATCharged', nVatCharged);

    nPrice = nPrice.toFixed(decimals);
    SetCellValueFromKey(row, 'Price', nPrice);

    nVatRate = nVatRate.toFixed(2);
    SetCellValueFromKey(row, 'VATRate', nVatRate);


}


function SortableDateToJavaDate(SortableDate) {
    // SmcN 27/04/2014   This function take a date sting which is in a Sortable date format (i.e was an 's' format) and returns a jave date object
    // Javascript cannot work with the standard vb sortable date format so it has to be converted

    var datearray = SortableDate.split('T');
    var datepart = datearray[0];
    var javadatestring = datepart.replace(/-/g, "/");
    javadatestring = javadatestring + ' ' + datearray[1];

    var javadate = new Date(javadatestring);

    return javadate
}









