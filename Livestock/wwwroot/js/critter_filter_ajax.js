function getRadioGroupValue(group) {
    var value = "BUG";
    group.forEach(function (b) {
        if (b.checked) {
            value = b.value;
            return;
        }
    });
    if (value === "ALL")
        value = null;
    if (value === "BUG")
        alert("Dev error: value still has the 'BUG' value.");
    return value;
}
function updateDesignLayout(nameTagTextbox, nameTagRadioButtons, breedDropdown, typeDropdown, genderRadioButtons, canReproduceButtons, cache, div, designType) {
    var typeName = typeDropdown.selectedOptions[0].innerHTML;
    var typeValue = parseInt(typeDropdown.selectedOptions[0].value);
    var breedName = breedDropdown.selectedOptions[0].innerHTML;
    var breedValue = parseInt(breedDropdown.selectedOptions[0].value);
    var genderValue = getRadioGroupValue(genderRadioButtons);
    var reproduceValue = getRadioGroupValue(canReproduceButtons);
    var nameTagValue = nameTagTextbox.value;
    var isNameFilter = getRadioGroupValue(nameTagRadioButtons) == "Name";
    if (reproduceValue !== null)
        reproduceValue = (reproduceValue === "true");
    var key = designType + "-" + breedName + "-" + typeName + "-" + genderValue + "-" + reproduceValue + "-" + nameTagValue + "-" + isNameFilter;
    if (key in cache) {
        div.innerHTML = cache[key];
        doLazyLoading();
        return;
    }
    $.ajax({
        type: "POST",
        url: "/CritterEx/GetCrittersFilteredAndRender",
        contentType: "application/json",
        dataType: "html",
        data: JSON.stringify({
            BreedId: breedValue,
            CritterTypeId: typeValue,
            Design: designType,
            Gender: genderValue,
            CanReproduce: reproduceValue,
            Name: (isNameFilter) ? nameTagValue : null,
            Tag: (!isNameFilter) ? nameTagValue : null
        })
    }).done(function (response) {
        if (response === null || response === "\n")
            response = '<div class="alert alert-info">No results found.</div>';
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
function handleCritterFilter(nameTagTextbox, nameTagRadioButtons, typeDropdown, breedDropdown, genderRadioButtons, canReproduceButtons) {
    var divCardHoriz = document.getElementById("design-card-horiz");
    var divCardVert = document.getElementById("design-card-vert");
    var divTable = document.getElementById("design-table");
    var breedCache = {};
    var typeCache = {};
    breedDropdown.addEventListener("change", function () {
        updateDesignLayout(nameTagTextbox, nameTagRadioButtons, breedDropdown, typeDropdown, genderRadioButtons, canReproduceButtons, breedCache, divCardHoriz, "card-horiz");
        updateDesignLayout(nameTagTextbox, nameTagRadioButtons, breedDropdown, typeDropdown, genderRadioButtons, canReproduceButtons, breedCache, divCardVert, "card-vert");
        updateDesignLayout(nameTagTextbox, nameTagRadioButtons, breedDropdown, typeDropdown, genderRadioButtons, canReproduceButtons, breedCache, divTable, "table");
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
    genderRadioButtons.forEach(function (b) {
        b.addEventListener("change", function () {
            breedDropdown.dispatchEvent(new Event("change"));
        });
    });
    canReproduceButtons.forEach(function (b) {
        b.addEventListener("change", function () {
            breedDropdown.dispatchEvent(new Event("change"));
        });
    });
    nameTagRadioButtons.forEach(function (b) {
        b.addEventListener("change", function () {
            breedDropdown.dispatchEvent(new Event("change"));
        });
    });
    nameTagTextbox.addEventListener("change", function () {
        breedDropdown.dispatchEvent(new Event("change"));
    });
}
//# sourceMappingURL=critter_filter_ajax.js.map