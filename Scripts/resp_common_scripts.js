'use strict';

var hideThisShowAnother = function (element, elementToShowCSSSelector) {
        $(element).hide();
        $(elementToShowCSSSelector).show();
    };
