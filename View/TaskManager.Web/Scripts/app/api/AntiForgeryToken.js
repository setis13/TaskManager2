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
            console.log(token);
            console.log(options.data);
            // todo
            //if (options.data.indexOf("X-Requested-With") === -1) {
            //    options.data = "X-Requested-With=XMLHttpRequest" + ((options.data === "") ? "" : "&" + options.data);
            //}
            //options.data = options.data + "&" + token.name + '=' + token.value;
            if (options.data.length > 0) {
                var obj = JSON.parse(options.data);
                obj[token.name] = token.value;
                options.data = JSON.stringify(obj);
                console.log(options.data);
            }
        }
    }
});
//# sourceMappingURL=AntiForgeryToken.js.map