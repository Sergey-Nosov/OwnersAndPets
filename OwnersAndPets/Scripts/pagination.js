// JavaScript source code
function pagination(contentSelector, pagesContainerSelector) {
    var content = document.querySelectorAll(contentSelector);
    if (content.length == 0) {
        return;
    }
    var idContainer = document.getElementById(pagesContainerSelector);
    var pageNumber = getURLParameters("#");
    for (var i = 0; i < content.length; i++) {
        content[i].id = i;
        if (i != pageNumber-1)
            content[i].className = "hidden";
        else
            content[i].className = "visible";
        var a = document.createElement('a');
        var linkText = document.createTextNode(i+1);
        a.appendChild(linkText);
        a.href = "#" + (i + 1).toString();
        a.className = "page-link";
        a.setAttribute("target-id", i);
        idContainer.appendChild(a);
        if (i != content.length - 1) {
            idContainer.appendChild(document.createTextNode(", "));
        }
        a.onclick = function (event) {
            for (var j = 0; j < content.length; j++) {
                content[j].className = "hidden";
            }
            var targetId = event.target.getAttribute("target-id");
            document.getElementById(targetId).className = "visible";
        }
    }


    for (var j = 0; j < content.length; j++) {
        if (j == 0) {
            content[0].className = "visible";
        } else {
            content[j].className = "hidden";
        }

        }
}