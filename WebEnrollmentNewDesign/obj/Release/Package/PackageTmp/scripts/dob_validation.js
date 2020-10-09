function leapYear(year) {
    return ((year % 4 == 0) && (year % 100 != 0)) || (year % 400 == 0);
}

function updatedaysinmonth() {
    var yy = document.getElementById("ddlYear").value;
    var mm = document.getElementById("ddlMonth").value;
    var dd = document.getElementById("ddlday").value;
    var ddl = document.getElementById('ddlday.ClientID');

    var ddl_length = ddl.length;
    var i = 0;
    ddl.options.length = 0;
    for (i = 1; i < 29; i++) {
        option = document.createElement("option");
        option.value = i;
        option.innerHTML = i;
        ddl.appendChild(option);
    }
    if (leapYear(yy) && mm == 2) {
        option = document.createElement("option");
        option.value = 29; option.innerHTML = 29;
        ddl.appendChild(option);
        ddl.selectedIndex = (dd - 1 > ddl.length) ? 0 : dd - 1;
        return;
    }
    if (mm != 2) {
        var nDays = 0;
        if (mm == 1 || mm == 3 || mm == 5 || mm == 7 || mm == 8 || mm == 10 || mm == 12)
            nDays = 31;
        else
            nDays = 30

        for (i = 29; i <= nDays; i++) {
            option = document.createElement("option");
            option.value = i;
            option.innerHTML = i;
            ddl.appendChild(option);
        }
    }
    ddl.selectedIndex = (dd - 1 > ddl.length) ? 0 : dd - 1;
    //validateDOB();
}

function getdayselected() {
    var dd = document.getElementById("ddlday").value;
    document.getElementById("dobDay_HF").value = dd;
    //validateDOB();
}

function validateDOB() {

    var cdate = document.getElementById("SysDate_HF").value;

    var ddly = document.getElementById('ddlYear');
    var ddlm = document.getElementById('ddlMonth');
    var ddld = document.getElementById('ddlday');

    var yy = parseInt(ddly.options[ddly.selectedIndex].value);
    var mm = parseInt(ddlm.options[ddlm.selectedIndex].value);
    var dd = parseInt(ddld.options[ddld.selectedIndex].value);
    var first = new Date(yy, mm - 1, dd);

    //alert(cdate);
    var second = parseDate(cdate);
    var dif = daydiff(first, second);
    //alert('first :'+ first);
    //alert('second :' + second);
    //alert('dif :' + dif);
    if (dif >= 6570) {
        change_color(ddly, "Green");
        change_color(ddlm, "Green");
        change_color(ddld, "Green");
        return true;
    }
    else {
        change_color(ddly, "Red");
        change_color(ddlm, "Red");
        change_color(ddld, "Red");
        return false;
    }
}

function parseDate(str) {
    //alert('parseDate :' + str);
    var mdy = str.split('/')
    //alert(str + '--->' + mdy[2] + ' '+ mdy[0]-1 + ' '+ mdy[1] );
    return new Date(mdy[2], mdy[0] - 1, mdy[1]);
}

function daydiff(first, second) {
    return (second - first) / (1000 * 60 * 60 * 24);
}

//---DOB DDLs populating and validating ends....