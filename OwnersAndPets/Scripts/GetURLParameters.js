function getURLParameters(paramName) {
    var sURL = window.document.URL.toString();
    if (sURL.indexOf("#") > 0)
        return sURL.split("#")[1];
    return 1;
}