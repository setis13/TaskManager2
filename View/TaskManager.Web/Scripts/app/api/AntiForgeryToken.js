function GetAntiForgeryToken() {
    var tokenField = $("input[type='hidden'][name$='RequestVerificationToken']");
    if (tokenField.length === 0) {
        return null;
    }
    else {
        return {
            name: tokenField[0].name,
            value: tokenField[0].value
        };
    }
}
$.ajaxPrefilter(function (options, localOptions, jqXHR) {
    if (options.type !== "GET") {
        var token = GetAntiForgeryToken();
        if (token !== null) {
            jqXHR.setRequestHeader(token.name, token.value);
        }
    }
});
//# sourceMappingURL=AntiForgeryToken.js.map