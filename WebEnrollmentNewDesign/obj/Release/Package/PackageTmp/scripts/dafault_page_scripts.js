// defaultpage

function SetZIndex(control, args) {
    // Set auto complete extender control's z-index to a high value
    // so it will appear on top of, not under, the ModalPopUp extended panel.
    control._completionListElement.style.zIndex = 99999999;
}



function OnClientCompleted(sender, e) {
    sender._element.className = "";
}

function OnClientSelected(sender, e) {
    alert('selected');
    sender._element.className = "valid_esiid";
}

var isItemSelected = false;
//Handler for AutoCompleter OnClientItemSelected event
function onItemSelected() {
    isItemSelected = true;
}

//Handler for textbox blur event
function checkItemSelected(txt) {
    if (!isItemSelected) {
        //  alert("Please select item from the list only");
        txt.focus();
    }
}
//service addresss popup related

function SetZIndex(control, args) {
    // Set auto complete extender control's z-index to a high value
    // so it will appear on top of, not under, the ModalPopUp extended panel.
    control._completionListElement.style.zIndex = 99999999;
}

function OnClientPopulating(sender, e) {
    sender._element.className = "loading";
}

function OnClientCompleted(sender, e) {
    if (document.getElementById('btnConfirm').style.display  == true) {
        document.getElementById("MainContent_txtServAddress").className = "selectcomplete";
    }
    //sender._element.className = "valid_esiid";
}

function OnClientSelected(sender, e) {
    //alert('selected');
    sender._element.className = "valid_esiid";
}

var isItemSelected = false;
//Handler for AutoCompleter OnClientItemSelected event
function onItemSelected() {
    isItemSelected = true;
}

//Handler for textbox blur event
function checkItemSelected(txt) {
    if (!isItemSelected) {
        //  alert("Please select item from the list only");
        txt.focus();
    }
}

function ClearStatus() {
    document.getElementById('lblServAddrStatus').innerHTML = "";
}



function parseJson(results) {

    //alert('serviceAddrStatus : ' + results.serviceAddrStatus);
    var retval = results.serviceAddrStatus == 'OK' ? true : false;
    //alert(retval);
    return retval;
}