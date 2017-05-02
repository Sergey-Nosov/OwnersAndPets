window.onload = function () {
    pagination("#main2 tbody", "footer");

    var totalCount = document.querySelectorAll("#main2 tbody tr").length;
    document.querySelector("#total-count td").innerHTML += totalCount;
};
    window.addEventListener("load", function () {
        document.querySelector("#sort-header-name").onclick = function () {
            var directionInput = document.querySelector("input[name='sortDirection']");
            var image = document.querySelector("#sort-direction-image");
            var direction;
            if (directionInput.value === "asc") {
                direction = "desc";
                image.className = "sort-desc";

            } else if (directionInput.value === "desc" || directionInput.value === "") {
                direction = "asc";
                image.className = "sort-asc";
            }
            directionInput.value = direction;
            var form = document.querySelector("#sort-header-name");
            form.submit();
        }
    });
