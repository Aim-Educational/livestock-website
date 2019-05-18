$(function () {
    if (CAN_USE_COOKIES) {
        var design = Cookies.get("critterDesign");
        if (design !== null && design !== undefined)
            changeDesign(design);
    }
});
function changeDesign(divName) {
    var div = document.getElementById(divName);
    if (div === null) {
        alert("Dev error: No div called " + divName);
        return;
    }
    var designs = document.querySelectorAll('*[id^="design-"]');
    designs.forEach(function (d) { return d.classList.add("d-none"); });
    div.classList.remove("d-none");
    if (CAN_USE_COOKIES)
        Cookies.set("critterDesign", divName);
}
//# sourceMappingURL=design_select.js.map