function onUpdating() {
    // get the update progress div
    //var updateProgressDiv = $get('updateProgressDiv');
    var updateProgressDiv = document.getElementById("updateProgressDiv");
    // make it visible
    updateProgressDiv.style.display = '';
    var region = null;

    region = document.getElementById("box01");
    //region = $get('<%= this.box01.ClientID %>');

    // get the bounds of both the region and the progress div
    var regionBounds = Sys.UI.DomElement.getBounds(region);
    var updateProgressDivBounds = Sys.UI.DomElement.getBounds(updateProgressDiv);

    //    do the math to figure out where to position the element (the center of the region)
    var x = regionBounds.x + Math.round(regionBounds.width / 2) - Math.round(updateProgressDivBounds.width / 2);
    var y = regionBounds.y + Math.round(regionBounds.height / 2) - Math.round(updateProgressDivBounds.height / 2);

    //    set the progress element to this position
    Sys.UI.DomElement.setLocation(updateProgressDiv, x, y);
}

function onUpdated() {
    // get the update progress div
    //var updateProgressDiv = $get('updateProgressDiv');
    var updateProgressDiv = document.getElementById("updateProgressDiv");
    // make it invisible
    updateProgressDiv.style.display = 'none';
}