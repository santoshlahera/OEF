
//Event Tracking for Client's jQuery - START
jQuery(document).ready(function () {
    jQuery("a[href^='mailto:']").click(function () {
        if (_gaq) {
            var mAddress = jQuery(this).attr('href').replace('mailto:', '');
            _gaq.push(['_trackEvent', 'MailTo', 'click', mAddress]);
        }
    });
    jQuery("a[href*='.doc'],a[href*='.docx'],a[href*='.dwg'],a[href*='.pdf'],a[href*='.xls'],a[href*='.xlsx'],a[href*='.ppt'],a[href*='.pptx'],a[href*='.zip'],a[href*='.txt'],a[href*='.vsd'],a[href*='.vxd'],a[href*='.js'],a[href*='.css'],a[href*='.rar'],a[href*='.exe'],a[href*='.wma'],a[href*='.mov'],a[href*='.avi'],a[href*='.wmv'],a[href*='.mp3']").click(function (e) {
        if (_gaq) {
            _gaq.push(['_trackEvent', 'Download', 'click', jQuery(this).attr('href')]);
        }
    });
    jQuery("a[href*='http']:not([href*='" + window.location.hostname + "'])").click(function (e) {
        if (_gaq) {
            //The following block added 2-21-11
            var activeURL = window.location.href;
            if (jQuery(this).attr('rel') == 'print') {
                _gaq.push(['_trackEvent', 'Print', 'click', activeURL]);
                //alert('something to print');
                return;
            }

            var socialSites = new Array(  //Array of Social Media Sites (Social Event)
                "linkedin.com",
                "www.linkedin.com",
                "twitter.com",
                "www.twitter.com",
                "facebook.com",
                "www.facebook.com",
                "www.google.com/bookmarks",
                "google.com/bookmarks",
                "youtube.com",
                "www.youtube.com"
            );

            var excludedSites = new Array(
                "frontierutilities.com",
                "www.frontierutilities.com"
            );

            var linkHost = this.hostname;

            if (jQuery.inArray(linkHost, excludedSites) == -1) {
                if (jQuery.inArray(linkHost, socialSites) >= 0) {
                    //alert('This link triggered "Social" ');
                    _gaq.push(['_trackEvent', 'Social', 'click', jQuery(this).attr('href')]);
                } else {
                    //alert('This link triggered "Outboud" ');
                    _gaq.push(['_trackEvent', 'Outbound', 'click', jQuery(this).attr('href')]);
                }

            } else {
                //alert("not tracked");
            }
        }
    });
});
//Event Tracking for Client's jQuery - END


//used at dataaccordian
function creditscoresuccess() {
     alert("creditscoresuccess");
    _gaq.push(['_trackPageview', '/creditscoresuccess']);
}

function creditscorefailed() {
      alert("creditscorefailed");
    _gaq.push(['_trackPageview', '/creditscorefailed']);
}


