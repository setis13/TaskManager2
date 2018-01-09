function GetAntiForgeryToken() {
    var tokenField = $("input[type='hidden'][name$='RequestVerificationToken']");
    if (tokenField.length === 0) {
        return null;
    } else {
        return {
            name: (<any>tokenField[0]).name,
            value: (<any>tokenField[0]).value
        };
    }
}

$.ajaxPrefilter(
    (options, localOptions, jqXHR) => {
        if (options.type !== "GET") {
            var token = GetAntiForgeryToken();
            if (token !== null) {
                jqXHR.setRequestHeader(token.name, token.value);
            }
        }
    }
);