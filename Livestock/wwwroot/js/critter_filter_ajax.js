function updateDesignLayout(breedDropdown, typeDropdown, cache, div, designType) {
    var typeName = typeDropdown.selectedOptions[0].innerHTML;
    var typeValue = parseInt(typeDropdown.selectedOptions[0].value);
    var breedName = breedDropdown.selectedOptions[0].innerHTML;
    var breedValue = parseInt(breedDropdown.selectedOptions[0].value);
    var key = designType + breedName + "-" + typeName;
    if (key in cache) {
        div.innerHTML = cache[key];
        return;
    }
    $.ajax({
        type: "POST",
        url: "/CritterEx/GetCrittersFiltered",
        contentType: "application/json",
        dataType: "html",
        data: JSON.stringify({
            BreedId: breedValue,
            CritterTypeId: typeValue,
            Design: designType
        })
    }).done(function (response) {
        div.innerHTML = response;
        cache[key] = response;
    });
}
function handleCritterFilter(typeDropdown, breedDropdown) {
    var divCardHoriz = document.getElementById("design-card-horiz");
    var divCardVert = document.getElementById("design-card-vert");
    var divTable = document.getElementById("design-table");
    var cache = {};
    breedDropdown.addEventListener("change", function () {
        updateDesignLayout(breedDropdown, typeDropdown, cache, divCardHoriz, "card-horiz");
        updateDesignLayout(breedDropdown, typeDropdown, cache, divCardVert, "card-vert");
        updateDesignLayout(breedDropdown, typeDropdown, cache, divTable, "table");
    });
}
//# sourceMappingURL=critter_filter_ajax.js.map