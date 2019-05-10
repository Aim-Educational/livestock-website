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
function setBreedValues(breedDropdown, breeds) {
    breeds.unshift({ value: -999, description: "All" });
    breeds.forEach(function (b) {
        var option = document.createElement("option");
        option.innerHTML = b.description;
        option.value = b.value.toString();
        breedDropdown.options.add(option);
    });
    breedDropdown.dispatchEvent(new Event("change"));
}
function handleCritterFilter(typeDropdown, breedDropdown) {
    var divCardHoriz = document.getElementById("design-card-horiz");
    var divCardVert = document.getElementById("design-card-vert");
    var divTable = document.getElementById("design-table");
    var breedCache = {};
    var typeCache = {};
    breedDropdown.addEventListener("change", function () {
        updateDesignLayout(breedDropdown, typeDropdown, breedCache, divCardHoriz, "card-horiz");
        updateDesignLayout(breedDropdown, typeDropdown, breedCache, divCardVert, "card-vert");
        updateDesignLayout(breedDropdown, typeDropdown, breedCache, divTable, "table");
    });
    typeDropdown.addEventListener("change", function () {
        var typeId = parseInt(typeDropdown.selectedOptions[0].value);
        breedDropdown.innerHTML = "";
        if (typeDropdown.selectedOptions[0].innerHTML == "All") {
            var option = document.createElement("option");
            option.value = "-999";
            breedDropdown.options.add(option);
            breedDropdown.dispatchEvent(new Event("change"));
            return;
        }
        if (typeId.toString() in typeCache) {
            setBreedValues(breedDropdown, typeCache[typeId]);
            return;
        }
        $.ajax({
            type: "POST",
            url: "/CritterEx/GetBreedList",
            contentType: "application/json",
            dataType: "json",
            data: JSON.stringify({ CritterTypeId: typeId })
        }).done(function (response) {
            setBreedValues(breedDropdown, response);
            typeCache[typeId] = response;
        });
    });
}
//# sourceMappingURL=critter_filter_ajax.js.map