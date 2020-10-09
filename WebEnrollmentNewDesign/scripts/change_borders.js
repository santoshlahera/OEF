
function change_color(ct, color) {
    if (color == "Red")
        ct.style.border = "2px Red Solid";
    else
        ct.style.border = "2px Green Solid";
}

function change_forecolor(ct, color) {
    if (color == "Red")
        ct.style.color = "Red";
    else
        ct.style.color = "Green";
}

function ChangeBorder(txt, rf) {
    if (rf.isvalid == true) { txt.style.border = "2px Green Solid"; }
    else { txt.style.border = "2px Red Solid"; }
}

function SetBorderColor(ct, bl) {
    //alert(ct.toString());
    //alert(bl);
    ct.style.border = bl == true ? "2px Green Solid" : "2px Red Solid";
}

function inputgroup_borderchange(ct1, ct2, ct3, color) {
    change_color(ct1, color);
    change_color(ct2, color);
    change_color(ct3, color);
}