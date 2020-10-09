
//confirmation page
function gotoURL(btnType) {
    var url = '';
    if (btnType == 'tos') {
        url = ($get('hidden_tos')).value;
    } else if (btnType == 'eflen') {
        url = ($get('hidden_eflen')).value;
    } else if (btnType == 'efles') {
        url = ($get('hidden_efles')).value;
    } else if (btnType == 'yrasc') {
        url = ($get('hidden_yrasc')).value;
    }
    window.open(url, '_blank');

}
