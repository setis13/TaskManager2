﻿app.filter('nl2br', function ($sce) {
    return function (msg, is_xhtml) {
        var is_xhtml = is_xhtml || true;
        var breakTag = (is_xhtml) ? '<br />' : '<br>';
        var html = (msg + '').replace(/ /g, "&nbsp;").replace(/([^>\r\n]?)(\r\n|\n\r|\r|\n)/g, '$1' + breakTag + '$2');
        return $sce.trustAsHtml(html);
    }
});